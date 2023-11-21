namespace Epsilon.Abstractions.Services;

public interface IAuthorizationUser
{
    Task<bool> HasCurrentUserAccessToUser(string userId);
}