using System;
using System.Collections.Generic;
using System.Text;


namespace DevJourney.Domain.Entities.Profile;

public partial class Skill
{
    private Skill()
    {
    }

    private Skill(string name, int level, int? yearsOfExperience = null)
    {
        this.Name = name;
        this.Level = level;
        this.YearsOfExperience = yearsOfExperience;
    }

    internal static Skill Create(
        string name,
        int level,
        int? yearsOfExperience = null)
    {
        Validate(name, level, yearsOfExperience);
        
        if (level > 100)
            throw new InvalidOperationException("Level must be between 0 and 100.");
        
        return new Skill(
            name: name,
            level: level,
            yearsOfExperience: yearsOfExperience
        );
    }

    public void Modify(string name, int level, int? yearsOfExperience = null)
    {
        Validate(name, level, yearsOfExperience);
        
        if (level > 100)
            throw new InvalidOperationException("Level must be between 0 and 100.");
        
        this.Name = name;
        this.Level = level;
        this.YearsOfExperience = yearsOfExperience;
    }
    
    
    private static void Validate(
        string name,
        int level,
        int? yearsOfExperience)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException(
                "Skill name cannot be empty.");

        if (level is < 0 or > 100)
            throw new InvalidOperationException(
                "Skill level must be between 0 and 100.");

        if (yearsOfExperience.HasValue && yearsOfExperience < 0)
            throw new InvalidOperationException(
                "Years of experience cannot be negative.");
    }
}