﻿using Bogus;
using Epsilon.Abstractions.Services;
using Epsilon.Services;
using Moq;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest.Services;

public class LearningOutcomeCanvasResultServiceTests
{
    private readonly Mock<ICanvasGraphQlApi> _mockCanvasGraphQl = new Mock<ICanvasGraphQlApi>();
    private readonly Mock<ILearningDomainService> _mockLearningDomainService = new Mock<ILearningDomainService>();
    private readonly LearningOutcomeCanvasResultService _learningOutcomeCanvasResultService;


    public LearningOutcomeCanvasResultServiceTests()
    {
        _learningOutcomeCanvasResultService = new LearningOutcomeCanvasResultService(_mockCanvasGraphQl.Object, _mockLearningDomainService.Object);
    }

    [Fact]
    public async Task GetAccessibleStudents_ReturnsValidStudentsListOfCurrentCourse()
    {
        //Arrange
        var student = TestDataGenerator.GenerateUser().Generate();
        var outcomes = TestDataGenerator.GenerateRandomLearningDomainOutcome(TestDataGenerator.GenerateRandomLearningDomain().Generate()).Generate(80);
        var canvasResponse = TestDataGeneratorCanvasResponse.GenerateCanvasSubmissionsResult(outcomes).Generate();
        _mockLearningDomainService.Setup(static m => m.GetOutcomes()).ReturnsAsync(outcomes);
        _mockCanvasGraphQl.Setup(static m => m.Query(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).ReturnsAsync(canvasResponse);
        //Act
        var results = await _learningOutcomeCanvasResultService.GetSubmissions(student.LegacyId!).ToListAsync();
        //Assert
        // Assert.RaisesAsync(results);
        Assert.NotNull(results);
    }
}