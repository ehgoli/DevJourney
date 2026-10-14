namespace DevJourney.Application.DTOs.Auth.Register;

public sealed record RegisterRequest(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password);