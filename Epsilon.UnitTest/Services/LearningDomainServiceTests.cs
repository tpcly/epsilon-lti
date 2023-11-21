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
        _learningDomainOutcomeRepositoryMock.Setup(static repo => repo.AllToListAsync(null, It.IsAny<string[]?>(), It.IsAny<int?>(), It.IsAny<int?>())).ReturnsAsync(_outcomes);

        // Act
        var result = await _learningDomainService.GetOutcomes();

        // Assert
        Assert.Equal(_outcomes, result);
    }

    [Fact]
    public async Task GetDomain_ReturnsExpectedDomain()
    {
        _learningDomain = TestDataGenerator.GenerateRandomLearningDomain();
        // Arrange
        var domainName = _learningDomain.Id;
        _learningDomainRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(d => d.Id == domainName, It.IsAny<string[]>())).ReturnsAsync(_learningDomain);

        // Act
        var result = await _learningDomainService.GetDomain(domainName);

        // Assert
        Assert.Equal(_learningDomain, result);
        Assert.NotNull(_learningDomain.ValuesSet);
        Assert.NotNull(_learningDomain.RowsSet);
        Assert.Null(_learningDomain.ColumnsSet);
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
        Assert.NotNull(_learningDomain.ColumnsSet);
    }


    [Fact]
    public async Task GetDomain_ReturnsColumnNull()
    {
        // Arrange
        var domainName = _learningDomain.Id;
        _learningDomainRepositoryMock.Setup(repo => repo.SingleOrDefaultAsync(d => d.Id == domainName, It.IsAny<string[]>())).ReturnsAsync(_learningDomain);

        // Act
        var result = await _learningDomainService.GetDomain(domainName);

        // Assert
        Assert.Equal(_learningDomain, result);
        Assert.Null(_learningDomain.ColumnsSet);
    }
}