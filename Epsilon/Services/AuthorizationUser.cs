﻿using System.Globalization;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class AuthorizationUser : IAuthorizationUser
{
    private readonly IFilterService _filterService;
    private readonly ICanvasUserSessionAccessor _sessionAccessor;

    public AuthorizationUser(ICanvasUserSessionAccessor sessionAccessor, IFilterService filterService)
    {
        _filterService = filterService;
        _sessionAccessor = sessionAccessor;
    }


    public async Task<bool> HasCurrentUserAccessToUser(string userId)
    {
        var acceptedStudentList = await _filterService.GetAccessibleStudents();
        var canvasUser = await _sessionAccessor.GetSessionAsync();
        var currentUserId = canvasUser?.UserId.ToString(CultureInfo.InvariantCulture);
        if (userId != currentUserId)
            return acceptedStudentList?.Any(u => u.Id.ToString(CultureInfo.CurrentCulture) == userId) ?? false;

        return true;
    }
}