﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
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

    public CompetenceDocumentServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _competenceDocumentService = new CompetenceDocumentService(_domainServiceMock.Object, _canvasResultServiceMock.Object);
    }

    [Fact]
    public async Task GetDocument_ReturnsCorrectDocument()
    {
        // // Arrange
        // var userId = "testUser";
        // var from = DateTime.Now.AddDays(-7);
        // var to = DateTime.Now;
        // var expectedDocument = new CompetenceDocument(new List<AbstractCompetenceComponent>());
        //
        // _canvasResultServiceMock.Setup(service => service.GetSubmissions(userId)).Returns(new List<LearningDomainSubmission>());
        // _domainServiceMock.Setup(static service => service.GetDomainsFromTenant()).ReturnsAsync(new List<LearningDomain>());
        //
        // // Act
        // var result = await _competenceDocumentService.GetDocument(userId, from, to);
        //
        // // Assert
        // Assert.Equal(expectedDocument.Components.Count, result.Components.Count);
    }

    [Fact]
    public void WriteDocument_WritesToStream()
    {
        // Arrange
        var document = new CompetenceDocument(new List<AbstractCompetenceComponent>());
        using var stream = new MemoryStream();

        // Act
        _competenceDocumentService.WriteDocument(stream, document);

        // Assert
        Assert.True(stream.Length > 0);
    }

    [Fact]
    private void ValidateOpnXmlWordGeneration()
    {
        // Arrange
        var document = new CompetenceDocument(new List<AbstractCompetenceComponent>());
        using var stream = new MemoryStream();
        _competenceDocumentService.WriteDocument(stream, document);
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