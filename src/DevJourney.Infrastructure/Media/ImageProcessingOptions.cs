namespace DevJourney.Infrastructure.Media;

public sealed class ImageProcessingOptions
{
    public int WebPQuality { get; set; } = 82;

    public int MaxWidth { get; set; } = 4000;

    public int MaxHeight { get; set; } = 4000;

    public uint MaxFrames { get; set; } = 1;

    public long MaxPixelMemoryBytes { get; set; } =
        256L * 1024 * 1024;
}