using DevJourney.Domain.Entities.Identity;

namespace DevJourney.Application.Interfaces.Infrastructure.Identity;

public interface ISignInManager
{
    Task SignInAsync(
        User user,
        bool rememberMe,
        CancellationToken cancellationToken = default);

    Task SignOutAsync(
        CancellationToken cancellationToken = default);
}