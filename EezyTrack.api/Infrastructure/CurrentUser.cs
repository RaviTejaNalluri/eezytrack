using EezyTrack.api.Application.Contracts;

namespace EezyTrack.api.Infrastructure;

public class CurrentUser : ICurrentUser
{
    public string Email { get; }

    public CurrentUser(IHttpContextAccessor accessor)
    {
        Email = accessor.HttpContext?.Request.Headers["X-User-Email"].FirstOrDefault()
                ?? "test@example.com";
    }
}


