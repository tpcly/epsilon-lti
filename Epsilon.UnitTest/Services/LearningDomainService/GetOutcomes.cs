using Epsilon.Abstractions.Services;
using Moq;

namespace Epsilon.UnitTest.Services.LearningDomainService;

public class GetOutcomes
{
    [Fact]
    public void GetOutcomes_givenValidDataStructure_thenResultIsAValidDomainModel()
    {
        //Arrange
        var learningDomainServiceMock = new Mock<ILearningDomainService>();
        learningDomainServiceMock.Setup(m => m.)
        //Act
        var result = learningDomainServiceMock.Object.GetOutcomes();

        //Assert
        Assert.T(result)
    }
}