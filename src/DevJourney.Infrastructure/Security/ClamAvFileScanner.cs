using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;
using DevJourney.Application.Interfaces.Infrastructure.Security;
using Microsoft.Extensions.Options;


namespace DevJourney.Infrastructure.Security;

public sealed class ClamAvFileScanner(
    IOptions<ClamAvOptions> options) : IFileScanner
{
    private readonly ClamAvOptions _options = options.Value;

    public async Task<FileScanResult> ScanAsync(
        Stream content,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        cancellationToken.ThrowIfCancellationRequested();

        if (!content.CanRead)
            throw new InvalidOperationException(
                "The provided stream is not readable.");

        if (_options.Port is < 1 or > 65535)
            throw new InvalidOperationException(
                "ClamAV port is invalid.");

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken);

        timeoutCts.CancelAfter(
            TimeSpan.FromSeconds(_options.TimeoutSeconds));

        using var client = new TcpClient();

        try
        {
            await client.ConnectAsync(
                _options.Host,
                _options.Port,
                timeoutCts.Token);

            await using var networkStream =
                client.GetStream();

            await SendCommandAsync(
                networkStream,
                cancellationToken: timeoutCts.Token);

            await SendContentAsync(
                networkStream,
                content,
                timeoutCts.Token);

            var response = await ReadResponseAsync(
                networkStream,
                timeoutCts.Token);

            return ParseResponse(response);
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            throw new IOException(
                "ClamAV scan timed out.");
        }
        catch (SocketException ex)
        {
            throw new IOException(
                "Unable to connect to ClamAV.",
                ex);
        }
    }

    private static async Task SendCommandAsync(
        NetworkStream stream,
        CancellationToken cancellationToken)
    {
        var command = Encoding.ASCII.GetBytes(
            "zINSTREAM\0");

        await stream.WriteAsync(
            command,
            cancellationToken);
    }

    private static async Task SendContentAsync(
        NetworkStream stream,
        Stream content,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[64 * 1024];

        while (true)
        {
            var bytesRead = await content.ReadAsync(
                buffer,
                cancellationToken);

            if (bytesRead == 0)
                break;

            var lengthBuffer = new byte[4];

            BinaryPrimitives.WriteUInt32BigEndian(
                lengthBuffer,
                (uint)bytesRead);

            await stream.WriteAsync(
                lengthBuffer,
                cancellationToken);

            await stream.WriteAsync(
                buffer.AsMemory(0, bytesRead),
                cancellationToken);
        }

        await stream.WriteAsync(
            new byte[4],
            cancellationToken);
    }

    private static async Task<string> ReadResponseAsync(
        NetworkStream stream,
        CancellationToken cancellationToken)
    {
        var response = new MemoryStream();
        var buffer = new byte[4096];

        while (true)
        {
            var bytesRead = await stream.ReadAsync(
                buffer,
                cancellationToken);

            if (bytesRead == 0)
                break;

            for (var i = 0; i < bytesRead; i++)
            {
                if (buffer[i] is 0 or (byte)'\n')
                {
                    return Encoding.UTF8
                        .GetString(response.ToArray())
                        .Trim();
                }

                response.WriteByte(buffer[i]);
            }
        }

        return Encoding.UTF8
            .GetString(response.ToArray())
            .Trim();
    }

    private static FileScanResult ParseResponse(
        string response)
    {
        if (string.IsNullOrWhiteSpace(response))
        {
            throw new IOException(
                "ClamAV returned an empty response.");
        }

        if (response.EndsWith(
                "OK",
                StringComparison.OrdinalIgnoreCase))
        {
            return new FileScanResult(
                IsSafe: true);
        }

        if (response.EndsWith(
                "FOUND",
                StringComparison.OrdinalIgnoreCase))
        {
            var threatName = ExtractThreatName(
                response);

            return new FileScanResult(
                IsSafe: false,
                ThreatName: threatName);
        }

        throw new IOException(
            $"ClamAV returned an unexpected response: {response}");
    }

    private static string? ExtractThreatName(
        string response)
    {
        const string prefix = "stream: ";

        if (!response.StartsWith(
                prefix,
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var value = response[prefix.Length..];

        const string suffix = " FOUND";

        if (value.EndsWith(
                suffix,
                StringComparison.OrdinalIgnoreCase))
        {
            value = value[..^suffix.Length];
        }

        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}