using Epsilon.Abstractions;
using Epsilon.Services;
using Moq;
using Tpcly.Persistence.Abstractions;

namespace Epsilon.UnitTest.Services;

public class LearningDomainServiceTests
{
    private readonly Mock<IReadOnlyRepository<LearningDomain>> _learningDomainRepositoryMock = new Mock<IReadOnlyRepository<LearningDomain>>();
    private readonly Mock<IReadOnlyRepository<LearningDomainOutcome>> _learningDomainOutcomeRepositoryMock = new Mock<IReadOnlyRepository<LearningDomainOutcome>>();
    private readonly IEnumerable<LearningDomainOutcome> _outcomes = TestDataGenerator.GenerateRandomLearningDomainOutcomes(10);
    private LearningDomain _learningDomain = TestDataGenerator.GenerateRandomLearningDomain();
    private readonly LearningDomainService _learningDomainService;

    public LearningDomainServiceTests()
    {
        _learningDomainService = new LearningDomainService(_learningDomainRepositoryMock.Object, _learningDomainOutcomeRepositoryMock.Object);
    }

    [Fact]
    public async Task GetOutcomes_ReturnsExpectedOutcomes()
    {
        // Arrange
        _learningDomainOutcomeRepositoryMock.Setup(static repo => repo.AllToListAsync(null, It.IsAny<string[]?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                                            .ReturnsAsync(_outcomes);

        // Act
        var result = await _learningDomainService.GetOutcomes();

        // Assert
        Assert.Equal(_outcomes, result);
    }

    [Fact]
    public async Task GetDomain_ReturnsExpectedDomain()
    {
        // Arrange
        var domainName = _learningDomain.Id;
        _learningDomainRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(d => d.Id == domainName, It.IsAny<string[]>())).ReturnsAsync(_learningDomain);

        // Act
        var result = await _learningDomainService.GetDomain(domainName);

        // Assert
        Assert.Equal(_learningDomain, result);
        Assert.NotNull(_learningDomain.ValuesSet);
        Assert.NotNull(_learningDomain.RowsSet);
    }


    [Fact]
    public async Task GetDomain_ReturnsExpectedDomainWithColumnTypes()
    {
        _learningDomain = TestDataGenerator.GenerateRandomLearningDomain();
        _learningDomain.ColumnsSet = TestDataGenerator.GenerateRandomLearningDomainTypeSet();
        // Arrange
        var domainName = _learningDomain.Id;
        _learningDomainRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(d => d.Id == domainName, It.IsAny<string[]>())).ReturnsAsync(_learningDomain);

        // Act
        var result = await _learningDomainService.GetDomain(domainName);

        // Assert
        Assert.Equal(_learningDomain, result);
        Assert.NotNull(_learningDomain.ValuesSet);
        Assert.NotNull(_learningDomain.RowsSet);
    }

    [Fact]
    public async Task GetDomainsFromTenant_ReturnsCorrectDomains()
    {
        // Arrange
        var expectedDomains = TestDataGenerator.GenerateRandomLearningDomains(3);
        _learningDomainRepositoryMock.Setup(static repo => repo.AllToList(null, It.IsAny<string[]?>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(expectedDomains);

        // Act
        var result = _learningDomainService.GetDomainsFromTenant();

        // Assert
        Assert.Equal(expectedDomains, result);
    }

}