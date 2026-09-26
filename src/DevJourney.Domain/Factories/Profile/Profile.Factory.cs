using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public sealed partial class Profile
{
    private Profile()
    {
    }
    
    private Profile(string email, string phone)
    {
        this.Email = email;
        this.Phone = phone;
    }

    public static Profile Create(string email, string phone)
    {
        return new Profile(
            email: email,
            phone: phone);
    }
    
    public void Modify(string email, string phone)
    {
        this.Email = email;
        this.Phone = phone;
    }
    
    
    public void AddSkill(string name, int level, int? yearsOfExperience = null)
    {
        return Skill.Create(
            name: name,
            level: level,
            yearsOfExperience: yearsOfExperience
        );
    }

    public void AddTimeline(DateOnly fromDate, DateOnly? toDate)
    {
        return Timeline.Create(
            fromDate: fromDate,
            toDate: toDate
        );
    }
    
    public void AddTimeline(
        string title,
        string url,
        string icon,
        int displayOrder)
    {
        return SocialMedia.Create(
            title: title,
            url: url,
            icon: icon,
            displayOrder: displayOrder);
    }
}