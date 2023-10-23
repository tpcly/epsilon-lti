using System.Globalization;
using Epsilon.Abstractions;
using Tpcly.Lti;

namespace Epsilon.Host.WebApi;

public class CanvasUserSessionAccessor : ICanvasUserSessionAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CanvasUserSessionAccessor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public async Task<CanvasUserSession?> GetSessionAsync()
    {
        var context = _contextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        var ltiMessage = await context.GetLtiMessageAsync();

        return new CanvasUserSession(
            int.Parse(ltiMessage.Custom["course_id"].ToString(), CultureInfo.InvariantCulture),
            int.Parse(ltiMessage.Custom["user_id"].ToString(), CultureInfo.InvariantCulture)
        );
    }
}