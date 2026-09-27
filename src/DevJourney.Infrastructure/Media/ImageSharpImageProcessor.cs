using DevJourney.Application.Interfaces.Infrastructure.Media;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace DevJourney.Infrastructure.Media;

public sealed class ImageSharpImageProcessor(
    IOptions<ImageProcessingOptions> options) : IImageProcessor
{
    private readonly ImageProcessingOptions _options = options.Value;

    public async Task<ImageProcessingResult> ProcessAsync(
        Stream content,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        cancellationToken.ThrowIfCancellationRequested();

        var input = await EnsureSeekableAsync(
            content,
            cancellationToken);

        var ownsInput = !ReferenceEquals(
            input,
            content);

        try
        {
            input.Position = 0;

            IImageFormat format;

            try
            {
                format = await Image.DetectFormatAsync(
                    input,
                    cancellationToken);
            }
            catch (UnknownImageFormatException)
            {
                input.Position = 0;

                return new ImageProcessingResult(
                    IsImage: false,
                    Content: input,
                    FileName: Path.GetFileName(fileName),
                    ContentType: GetContentType(fileName),
                    Length: input.Length);
            }

            input.Position = 0;

            var info = await Image.IdentifyAsync(
                input,
                cancellationToken);

            ValidateImage(info);

            input.Position = 0;

            var decoderOptions = new DecoderOptions
            {
                MaxFrames = _options.MaxFrames,
                SegmentIntegrityHandling =
                    SegmentIntegrityHandling.Strict
            };

            using var image = await Image.LoadAsync(
                decoderOptions,
                input,
                cancellationToken);

            image.Mutate(x => x.AutoOrient());

            RemoveUnnecessaryMetadata(image);

            var output = new MemoryStream();

            var encoder = new WebpEncoder
            {
                FileFormat = WebpFileFormatType.Lossy,
                Quality = _options.WebPQuality,
                Method = WebpEncodingMethod.BestQuality
            };

            await image.SaveAsWebpAsync(
                output,
                encoder,
                cancellationToken);

            output.Position = 0;

            return new ImageProcessingResult(
                IsImage: true,
                Content: output,
                FileName: Path.ChangeExtension(
                    Path.GetFileName(fileName),
                    ".webp"),
                ContentType: "image/webp",
                Length: output.Length);
        }
        finally
        {
            if (ownsInput)
                await input.DisposeAsync();
        }
    }

    private void ValidateImage(
        ImageInfo info)
    {
        if (info.Width > _options.MaxWidth)
        {
            throw new InvalidOperationException(
                $"Image width cannot exceed {_options.MaxWidth}px.");
        }

        if (info.Height > _options.MaxHeight)
        {
            throw new InvalidOperationException(
                $"Image height cannot exceed {_options.MaxHeight}px.");
        }

        if (info.FrameCount > _options.MaxFrames)
        {
            throw new InvalidOperationException(
                $"Image cannot contain more than {_options.MaxFrames} frame(s).");
        }

        var estimatedMemory =
            info.GetPixelMemorySize();

        if (estimatedMemory > _options.MaxPixelMemoryBytes)
        {
            throw new InvalidOperationException(
                "Image requires too much memory to decode.");
        }
    }

    private static void RemoveUnnecessaryMetadata(
        Image image)
    {
        image.Metadata.ExifProfile = null;
        image.Metadata.IptcProfile = null;
        image.Metadata.XmpProfile = null;
    }

    private static async Task<Stream> EnsureSeekableAsync(
        Stream content,
        CancellationToken cancellationToken)
    {
        if (content.CanSeek)
            return content;

        var buffer = new MemoryStream();

        await content.CopyToAsync(
            buffer,
            cancellationToken);

        buffer.Position = 0;

        return buffer;
    }

    private static string GetContentType(
        string fileName)
    {
        var extension = Path
            .GetExtension(fileName)
            .ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            _ => "application/octet-stream"
        };
    }

}