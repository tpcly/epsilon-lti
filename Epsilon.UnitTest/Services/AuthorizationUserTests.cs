﻿using System.Globalization;
using Bogus;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Epsilon.Services;
using Moq;

namespace Epsilon.UnitTest.Services;

public class AuthorizationUserTests
{
    private readonly Mock<IFilterService> _mockFilterService;
    private readonly Mock<ICanvasUserSessionAccessor> _mockSessionAccessor;
    private readonly AuthorizationUser _authorizationUser;
    private static readonly Faker s_faker = new Faker();


    public AuthorizationUserTests()
    {
        _mockFilterService = new Mock<IFilterService>();
        _mockSessionAccessor = new Mock<ICanvasUserSessionAccessor>();
        _authorizationUser = new AuthorizationUser(_mockFilterService.Object, _mockSessionAccessor.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task HasCurrentUserAccessToUser_ReturnTrue(bool isTeacher = false)
    {
        // Arrange
        var users = TestDataGenerator.GenerateUser().Generate(10);
        var user = s_faker.PickRandom(users);

        _mockFilterService.Setup(static fs => fs.GetAccessibleStudents()).ReturnsAsync(users);
        _mockSessionAccessor.Setup(static sa => sa.GetSessionAsync()).ReturnsAsync(new CanvasUserSession(0,int.Parse(user.LegacyId!, CultureInfo.InvariantCulture), isTeacher));

        // Act
        var result = await _authorizationUser.HasCurrentUserAccessToUser(user.LegacyId!);

        // Assert
        Assert.NotNull(user.LegacyId);
        Assert.True(result);
    }
    
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task HasCurrentUserAccessToUser_ReturnFalse(bool isTeacher = false)
    {
        // Arrange
        var users = TestDataGenerator.GenerateUser().Generate(10);
        var requestedUser = TestDataGenerator.GenerateUser().Generate();
        var currentUser = TestDataGenerator.GenerateUser().Generate();

        _mockFilterService.Setup(static fs => fs.GetAccessibleStudents()).ReturnsAsync(users);
        _mockSessionAccessor.Setup(static sa => sa.GetSessionAsync()).ReturnsAsync(new CanvasUserSession(0,int.Parse(currentUser.LegacyId!, CultureInfo.InvariantCulture), isTeacher));

        // Act
        var result = await _authorizationUser.HasCurrentUserAccessToUser(requestedUser.LegacyId!);

        // Assert
        Assert.NotNull(requestedUser.LegacyId);
        Assert.False(result);
    }
}