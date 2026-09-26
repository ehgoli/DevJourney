using System;
using System.Collections.Generic;
using System.Text;
using DevJourney.Domain.Entities.Common;

namespace DevJourney.Domain.Entities.Identity;

public sealed partial class User : BaseEntity
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string? ProfileImage { get; private set; }
    
    public string PasswordHash { get; private set; } = null!;
    public string? ActivationCode { get; set; } = null!;
    
    public UserStatus Status { get; private set; }
    
    public IReadOnlyCollection<UserRole> Roles => _roles;
    private readonly List<UserRole> _roles = [];
}