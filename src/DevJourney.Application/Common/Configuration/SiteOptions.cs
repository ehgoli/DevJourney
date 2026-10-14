using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevJourney.Application.Common.Configuration;

public sealed class SiteOptions
{
    public const string SectionName = "Site";

    [Required]
    [Url]
    public string BaseUrl { get; set; } = string.Empty;
}