using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Tpcly.Lti;

namespace Epsilon.Host.WebApi;

public class LtiJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly LtiSecurityTokenValidator _ltiTokenValidator;

    public LtiJwtBearerOptions(LtiSecurityTokenValidator ltiTokenValidator)
    {
        _ltiTokenValidator = ltiTokenValidator;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.SecurityTokenValidators.Clear();
        options.SecurityTokenValidators.Add(_ltiTokenValidator);

        options.IncludeErrorDetails = true;
        options.SaveToken = true;
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        Configure(options);
    }
}