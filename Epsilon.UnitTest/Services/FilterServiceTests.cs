using System.Globalization;
using Bogus;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Epsilon.Services;
using Moq;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest.Services;

public class FilterServiceTests
{
    private readonly IFilterService _filterService;
    private readonly Mock<ICanvasUserSessionAccessor> _mockSessionAccessor;
    private readonly Mock<ICanvasGraphQlApi> _mockCanvasGraphQl;
    private static readonly Faker s_faker = new Faker();


    public FilterServiceTests()
    {
        _mockSessionAccessor = new Mock<ICanvasUserSessionAccessor>();
        _mockCanvasGraphQl = new Mock<ICanvasGraphQlApi>();
        _filterService = new FilterService(_mockSessionAccessor.Object, _mockCanvasGraphQl.Object);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetAccessibleStudents_ReturnsValidStudentsListOfCurrentCourse(bool isTeacher = true)
    {
        //Arrange
        var fakedResult = TestDataGenerator.GenerateUsersEnrollmentsCourse().Generate();
        var session = new CanvasUserSession(s_faker.Random.Int(0, 1000), s_faker.Random.Int(0, 20), isTeacher);
        _mockSessionAccessor.Setup(static m => m.GetSessionAsync()).ReturnsAsync(session);
        _mockCanvasGraphQl.Setup(static m => m.Query(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                          .ReturnsAsync(fakedResult);
        //Act
        var students = (await _filterService.GetAccessibleStudents())?.ToList();
        //Assert
        //Validate that there are only unique students in the list. 
        Assert.True(students?.DistinctBy(static s => s.LegacyId).Count() == students?.Count());
        //Validate that only students will be returned
        Assert.False(fakedResult.Course?.Enrollments?.Nodes.Where(static e => e.Type != "StudentEnrollment")
                                .DistinctBy(static e => e.User!.LegacyId).Any(e => students?.Contains(e.User!) ?? false));
        //Validate that the student list is in alphabetical order
        Assert.True(students?.OrderBy(static u => u.Name).SequenceEqual(students));
    }
}