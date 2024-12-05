using Bogus;
using Epsilon.Abstractions.Services;
using Epsilon.Services;
using Moq;
using Tpcly.Canvas.Abstractions.GraphQl;
using Tpcly.Canvas.Abstractions.Rest;

namespace Epsilon.UnitTest.Services;

public class LearningOutcomeCanvasResultServiceTests
{
    private readonly Mock<ICanvasGraphQlApi> _mockCanvasGraphQl = new Mock<ICanvasGraphQlApi>();
    private readonly Mock<ICanvasRestApi> _mockCanvasRest = new Mock<ICanvasRestApi>();
    private readonly Mock<ILearningDomainService> _mockLearningDomainService = new Mock<ILearningDomainService>();
    private readonly LearningOutcomeCanvasResultService _learningOutcomeCanvasResultService;


    public LearningOutcomeCanvasResultServiceTests()
    {
        _learningOutcomeCanvasResultService = new LearningOutcomeCanvasResultService(_mockCanvasGraphQl.Object, _mockCanvasRest.Object, _mockLearningDomainService.Object);
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
        Assert.NotNull(results);
        Assert.Equal(40, results.Count);
        Assert.Equal(80, outcomes.Count);

        foreach (var submission in results)
        {
            foreach (var criteria in submission.Criteria)
            {
                Assert.Contains(submission.Results, r => r.Outcome.Id == criteria.Id);
                Assert.NotNull(criteria.MasteryPoints);
            }
        }
    }
}