using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public partial class SocialMedia
{
    private SocialMedia()
    {
    }

    private SocialMedia(
        string title,
        string url,
        string icon,
        int displayOrder)
    {
        Validate(title, url, icon, displayOrder);

        this.Title = title;
        this.Url = url;
        this.Icon = icon;
        this.DisplayOrder = displayOrder;
    }

    internal static SocialMedia Create(
        string title,
        string url,
        string icon,
        int displayOrder)
    {
        return new SocialMedia(
            title: title,
            url: url,
            icon: icon,
            displayOrder: displayOrder);
    }

    public void Modify(
        string title,
        string url,
        string icon,
        int displayOrder)
    {
        Validate(title, url, icon, displayOrder);

        this.Title = title;
        this.Url = url;
        this.Icon = icon;
        this.DisplayOrder = displayOrder;
    }

    private static void Validate(
        string title,
        string url,
        string icon,
        int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException(
                "Social media title cannot be empty.");

        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException(
                "Social media URL cannot be empty.");

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException(
                "Social media URL must be a valid HTTP or HTTPS URL.");
        }

        if (string.IsNullOrWhiteSpace(icon))
            throw new InvalidOperationException(
                "Social media icon cannot be empty.");

        if (displayOrder < 0)
            throw new InvalidOperationException(
                "Display order cannot be negative.");
    }
}
