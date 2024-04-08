﻿using Epsilon.Abstractions;
using Epsilon.Services;
using Moq;
using Tpcly.Persistence.Abstractions;

namespace Epsilon.UnitTest.Services;

public class LearningDomainServiceTests
{
    private readonly Mock<IReadOnlyRepository<LearningDomain>> _learningDomainRepositoryMock = new Mock<IReadOnlyRepository<LearningDomain>>();
    private readonly Mock<IReadOnlyRepository<LearningDomainOutcome>> _learningDomainOutcomeRepositoryMock = new Mock<IReadOnlyRepository<LearningDomainOutcome>>();
    private readonly IEnumerable<LearningDomainOutcome> _outcomes;
    private readonly LearningDomain _learningDomain = TestDataGenerator.GenerateRandomLearningDomain().Generate();
    private readonly LearningDomainService _learningDomainService;

    public LearningDomainServiceTests()
    {
        _learningDomainService = new LearningDomainService(_learningDomainRepositoryMock.Object, _learningDomainOutcomeRepositoryMock.Object);
        _outcomes = TestDataGenerator.GenerateRandomLearningDomainOutcome(_learningDomain).Generate(60);
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
        _learningDomain.ColumnsSet = TestDataGenerator.GenerateRandomLearningDomainTypeSet().Generate();
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
        var expectedDomains = TestDataGenerator.GenerateRandomLearningDomain().Generate(3);
        _learningDomainRepositoryMock.Setup(static repo => repo.AllToList(null, It.IsAny<string[]?>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(expectedDomains);

        // Act
        var result = _learningDomainService.GetDomainsFromTenant();

        // Assert
        Assert.Equal(expectedDomains, result);
    }

}