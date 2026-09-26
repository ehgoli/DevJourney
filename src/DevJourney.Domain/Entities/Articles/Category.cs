using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;


namespace DevJourney.Domain.Entities.Articles;

public sealed partial class Category : BaseEntity
{
    public string? CoverImage { get; private set; }
    
    private readonly List<CategoryTranslation> _translations = [];
    public IReadOnlyCollection<CategoryTranslation> Translations =>
        _translations;    
}