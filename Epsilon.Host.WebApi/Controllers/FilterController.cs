using Epsilon.Abstractions.Services;
using Epsilon.Canvas.Abstractions.GraphQl;
using Epsilon.Canvas.Abstractions.Rest;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Host.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<IEnumerable<User>> GetAccessibleStudents()
    {
        return await _filterService.GetAccessibleStudents();
    }
}