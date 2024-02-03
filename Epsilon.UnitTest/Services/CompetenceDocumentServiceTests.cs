using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;
using Epsilon.Services;
using Moq;
using Xunit.Abstractions;

namespace Epsilon.UnitTest.Services;

public class CompetenceDocumentServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Mock<ILearningDomainService> _domainServiceMock = new Mock<ILearningDomainService>();
    private readonly Mock<ILearningOutcomeCanvasResultService> _canvasResultServiceMock = new Mock<ILearningOutcomeCanvasResultService>();
    private CompetenceDocumentService _competenceDocumentService;
    private readonly IAsyncEnumerable<LearningDomainSubmission> _submissions = TestDataGenerator.GenerateRandomLearningDomainSubmissions(40);
    private readonly IEnumerable<LearningDomainOutcome> _outcomes = TestDataGenerator.GenerateRandomLearningDomainOutcomes(50);
    private readonly IEnumerable<LearningDomain> _domains = TestDataGenerator.GenerateRandomLearningDomains(2);

    public CompetenceDocumentServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _competenceDocumentService = new CompetenceDocumentService(_domainServiceMock.Object, _canvasResultServiceMock.Object);
    }

    [Fact]
    public async void WriteDocument_WritesToStream()
    {
        // Arrange
        _canvasResultServiceMock.Setup(static s => s.GetSubmissions(It.IsAny<string>())).Returns(_submissions);
        _domainServiceMock.Setup(static s => s.GetDomainsFromTenant()).Returns(_domains);
        _domainServiceMock.Setup(static s => s.GetOutcomes()).ReturnsAsync(_outcomes);
        var document = await _competenceDocumentService.GetDocument("01010", DateTime.Now.AddDays(-365), DateTime.Now);
        using var stream = new MemoryStream();
        
        // Act
        await _competenceDocumentService.WriteDocument(stream, document);
        
        // Assert
        Assert.True(stream.Length > 0);
    }

    [Fact]
    private async void ValidateOpnXmlWordGeneration()
    {
        // Arrange
        _canvasResultServiceMock.Setup(static s => s.GetSubmissions(It.IsAny<string>())).Returns(_submissions);
        _domainServiceMock.Setup(static s => s.GetDomainsFromTenant()).Returns(_domains);
        _domainServiceMock.Setup(static s => s.GetOutcomes()).ReturnsAsync(_outcomes);
        //Act
        var document = await _competenceDocumentService.GetDocument("01010", DateTime.Now.AddDays(-365), DateTime.Now);
        using var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, document);
        using var wordprocessingDocument = WordprocessingDocument.Open(stream, false);
        var validator = new OpenXmlValidator();
        var count = 0;
        try
        {
            foreach (var error in
                     validator.Validate(wordprocessingDocument))
            {
                count++;
                _testOutputHelper.WriteLine("Error " + count);
                _testOutputHelper.WriteLine("Description: " + error.Description);
                _testOutputHelper.WriteLine("ErrorType: " + error.ErrorType);
                _testOutputHelper.WriteLine("Node: " + error.Node);
                if (error.Path is not null)
                {
                    _testOutputHelper.WriteLine("Path: " + error.Path.XPath);
                }

                if (error.Part is not null)
                {
                    _testOutputHelper.WriteLine("Part: " + error.Part.Uri);
                }

                _testOutputHelper.WriteLine("-------------------------------------------");
            }

            _testOutputHelper.WriteLine("count={0}", count);
        }

        catch (Exception ex)
        {
            _testOutputHelper.WriteLine(ex.Message);
        }
        
        //Assert
        Assert.Equal(0, count);
    }
}