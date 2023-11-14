using Epsilon.Abstractions.Services;
using Moq;

namespace Epsilon.UnitTest.Services.LearningDomainService;

public class GetOutcomes
{
    
    private readonly Mock<ILearningDomainService> _learningDomainServiceMock;
    public GetOutcomes()
    {
        _learningDomainServiceMock = new Mock<ILearningDomainService>();
    }
    [Fact]
    public void GetOutcomes_givenValidDataStructure_thenResultIsAValidDomainModel()
    {
        //Arrange
        _learningDomainServiceMock.Setup(static m => m.GetOutcomes());
        //Act
        var result = _learningDomainServiceMock.Object.GetOutcomes();

        //Assert
        Assert.NotNull(result);
    }
}