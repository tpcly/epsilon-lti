using System.Globalization;
using Epsilon.Abstractions;
using Epsilon.Host.WebApi.Options;
using Microsoft.Extensions.Options;
using Tpcly.Lti;

namespace Epsilon.Host.WebApi;

public class CanvasUserSessionAccessor : ICanvasUserSessionAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly CanvasOptions _canvasOptions;

    public CanvasUserSessionAccessor(IHttpContextAccessor contextAccessor, IOptions<CanvasOptions> canvasOptions)
    {
        _contextAccessor = contextAccessor;
        _canvasOptions = canvasOptions.Value;
    }

    public async Task<CanvasUserSession?> GetSessionAsync()
    {
        var context = _contextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        var ltiMessage = await context.GetLtiMessageAsync();
        var courseId = _canvasOptions.OverrideCourseId ?? int.Parse(ltiMessage.Custom["course_id"].ToString(), CultureInfo.InvariantCulture);
        var userId = _canvasOptions.OverrideUserId ?? int.Parse(ltiMessage.Custom["user_id"].ToString(), CultureInfo.InvariantCulture);

        return new CanvasUserSession(
            courseId,
            userId
        );
    }
}