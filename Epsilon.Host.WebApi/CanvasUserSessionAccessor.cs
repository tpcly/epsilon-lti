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

        var courseId = _canvasOptions.OverrideCourseId ?? null;
        var userId = _canvasOptions.OverrideUserId ?? null;
        var isTeacher = _canvasOptions.OverrideCourseId is not null && _canvasOptions.OverrideUserId is not null;

        if (courseId == null && userId == null)
        {
            var ltiMessage = await context.GetLtiMessageAsync();
            if (ltiMessage?.Custom == null
                || !ltiMessage.Custom.TryGetValue("course_id", out var courseIdRaw)
                || !ltiMessage.Custom.TryGetValue("user_id", out var userIdRaw))
            {
                return null;
            }
            courseId = int.Parse(courseIdRaw.ToString()!, CultureInfo.InvariantCulture);
            userId = int.Parse(userIdRaw.ToString()!, CultureInfo.InvariantCulture);
            isTeacher = ltiMessage.Roles?.Contains("http://purl.imsglobal.org/vocab/lis/v2/membership#Instructor") ?? false;
        }
        
        
        return new CanvasUserSession(
            courseId!.Value,
            userId!.Value,
            isTeacher
        );
    }
}