﻿using DocumentFormat.OpenXml.Packaging;
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
    private readonly CompetenceDocumentService _competenceDocumentService;
    private readonly IAsyncEnumerable<LearningDomainSubmission> _submissions = TestDataGenerator.GenerateRandomLearningDomainSubmissions(40);

    public CompetenceDocumentServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _competenceDocumentService = new CompetenceDocumentService(_domainServiceMock.Object, _canvasResultServiceMock.Object);
    }

    [Fact]
    public async Task GetDocument_ReturnsCorrectDocumentWithFilteredDates()
    {
        // Arrange
        var userId = "testUser";
        var from = DateTime.Now.AddDays(-7);
        var to = DateTime.Now;
        var expectedDocument = CompetenceDocumentService.FetchComponents(_submissions, new List<LearningDomain?>(){TestDataGenerator.GenerateRandomLearningDomain(), TestDataGenerator.GenerateRandomLearningDomain(),});
        
        _canvasResultServiceMock.Setup(service => service.GetSubmissions(userId)).Returns(_submissions);
        
        // Act
        var result = await _competenceDocumentService.GetDocument(userId, from, to);
        
        // Assert
        Assert.Equal(await expectedDocument.CountAsync(), await result.Components.CountAsync());
    }
    
    
    [Fact]
    public async Task GetDocument_ReturnsCorrectDocumentWithNoDates()
    {
        // Arrange
        var userId = "testUser";
        var expectedDocument = CompetenceDocumentService.FetchComponents(_submissions, new List<LearningDomain?>(){TestDataGenerator.GenerateRandomLearningDomain(), TestDataGenerator.GenerateRandomLearningDomain(),});
        
        _canvasResultServiceMock.Setup(service => service.GetSubmissions(userId)).Returns(_submissions);
        // Act
        var result = await _competenceDocumentService.GetDocument(userId, null, null);
        
        // Assert
        Assert.Equal(await expectedDocument.CountAsync(), await result.Components.CountAsync());
    }

    [Fact]
    public async void WriteDocument_WritesToStream()
    {
        // Arrange
        
        var document = new CompetenceDocument(CompetenceDocumentService.FetchComponents(_submissions, new List<LearningDomain?>(){TestDataGenerator.GenerateRandomLearningDomain(), TestDataGenerator.GenerateRandomLearningDomain(),}));
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
        var document = new CompetenceDocument(CompetenceDocumentService.FetchComponents(_submissions, new List<LearningDomain?>(){TestDataGenerator.GenerateRandomLearningDomain(), TestDataGenerator.GenerateRandomLearningDomain(),}));
        using var stream = new MemoryStream();
        await _competenceDocumentService.WriteDocument(stream, document);
        using var wordprocessingDocument = WordprocessingDocument.Open(stream, false);
        var validator = new OpenXmlValidator();
        var count = 0;
        //Act
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

        wordprocessingDocument.Dispose();
        
        //Assert
        Assert.True(count == 0);
    }
}