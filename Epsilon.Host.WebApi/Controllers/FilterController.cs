using Epsilon.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tpcly.Canvas.Abstractions.GraphQl;
using User = Tpcly.Canvas.Abstractions.Rest.User;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FilterController : Controller
{
    private readonly IFilterService _filterService;

    public FilterController(IFilterService filterService)
    {
        _filterService = filterService;
    }

    [HttpGet("participated-terms")]
    public async Task<IEnumerable<EnrollmentTerm>> GetParticipatedTerms(string studentId)
    {
        return await _filterService.GetParticipatedTerms(studentId);
    }

    [HttpGet("accessible-students")]
    public async Task<IOrderedEnumerable<User>> GetAccessibleStudents()
    {
        return await _filterService.GetAccessibleStudents();
    }
}