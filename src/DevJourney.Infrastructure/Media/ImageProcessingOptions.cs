using System.ComponentModel.DataAnnotations;

namespace DevJourney.Infrastructure.Media;

public sealed class ImageProcessingOptions
{
    public const string SectionName = "ImageProcessing";
    
    [Range(
        1,
        100,
        ErrorMessage = "WebP quality must be between 1 and 100.")]
    public int WebPQuality { get; set; } = 82;

    [Range(
        1,
        16384,
        ErrorMessage = "Maximum image width must be between 1 and 16384 pixels.")]
    public int MaxWidth { get; set; } = 4000;

    [Range(
        1,
        16384,
        ErrorMessage = "Maximum image height must be between 1 and 16384 pixels.")]
    public int MaxHeight { get; set; } = 4000;

    [Range(
        typeof(uint),
        "1",
        "100",
        ErrorMessage = "Maximum frame count must be between 1 and 100.")]
    public uint MaxFrames { get; set; } = 1;

    [Range(
        typeof(long),
        "1048576",
        "1073741824",
        ErrorMessage = "Maximum pixel memory must be between 1 MB and 1 GB.")]
    public long MaxPixelMemoryBytes { get; set; } =
        256L * 1024 * 1024;
}