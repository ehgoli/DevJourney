using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Exceptions;


namespace DevJourney.Domain.Entities.Profile;

public partial class Timeline
{
    private Timeline()
    {
    }

    private Timeline(DateOnly fromDate, DateOnly? toDate)
    {
        this.FromDate = fromDate;
        this.ToDate = toDate;
    }
    
    internal static Timeline Create(DateOnly fromDate, DateOnly? toDate)
    {
        if (toDate.HasValue && fromDate > toDate.Value)
            throw new DomainException(
                "From date cannot be later than To date.");
        
        return new Timeline(
            fromDate: fromDate,
            toDate: toDate
        );
    }
    
    public void Modify(DateOnly fromDate, DateOnly? toDate)
    {
        if (toDate.HasValue && fromDate > toDate.Value)
            throw new DomainException(
                "From date cannot be later than To date.");
        
        this.FromDate = fromDate;
        this.ToDate = toDate;
    }
}
