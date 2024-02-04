using System.Globalization;
using Bogus;
using Epsilon.Abstractions;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest;

public static class TestDataGenerator
{
    private static Faker s_faker = new Faker();
    private static Faker<LearningDomainType> GenerateRandomLearningDomainType()
    {
        return new Faker<LearningDomainType>()
               .RuleFor(static o => o.Id, static f => f.Random.String())
               .RuleFor(static o => o.Name, static f => f.Random.String2(10))
               .RuleFor(static o => o.ShortName, static f => f.Random.String2(10))
               .RuleFor(static o => o.HexColor, static _ => "FFFFFF")
               .RuleFor(static o => o.Order, static f => f.Random.Int());
    }

    public static Faker<LearningDomainTypeSet> GenerateRandomLearningDomainTypeSet()
    {
        return new Faker<LearningDomainTypeSet>()
               .RuleFor(static o => o.Id, static f => f.Random.Guid())
               .RuleFor(static o => o.Types, static _ => GenerateRandomLearningDomainType().Generate(3));
    }

    public static Faker<LearningDomain> GenerateRandomLearningDomain()
    {
        return new Faker<LearningDomain>()
               .RuleFor(static o => o.Id, static f => f.Random.String())
               .RuleFor(static o => o.ColumnsSet,
                   static f => f.Random.Bool()
                       ? GenerateRandomLearningDomainTypeSet().Generate()
                       : null)
               .RuleFor(static o => o.ValuesSet, static _ => GenerateRandomLearningDomainTypeSet().Generate())
               .RuleFor(static o => o.RowsSet, static _ => GenerateRandomLearningDomainTypeSet().Generate());
    }

    public static Faker<LearningDomainOutcome> GenerateRandomLearningDomainOutcome(LearningDomain domain)
    {
        return new Faker<LearningDomainOutcome>()
               .RuleFor(static o => o.Id, static f => f.Random.Int())
               .RuleFor(static o => o.Name, static f => f.Random.String2(10))
               .RuleFor(static o => o.Row, f => f.PickRandom(domain.RowsSet.Types))
               .RuleFor(static o => o.Column, f => domain.ColumnsSet?.Types != null ? f.PickRandom(domain.ColumnsSet?.Types): null)
               .RuleFor(static o => o.Value, f => f.PickRandom(domain.ValuesSet.Types));
    }


    public static Faker<LearningDomainCriteria> GenerateRandomLearningDomainCriteria(LearningDomainOutcome outcome)
    {
        return new Faker<LearningDomainCriteria>().CustomInstantiator(f => new LearningDomainCriteria(outcome.Id, f.Random.Int(0, 10)));
    }

    public static Faker<LearningDomainOutcomeRecord> GenerateRandomLearningDomainResult(LearningDomainOutcome outcome)
    {
        return new Faker<LearningDomainOutcomeRecord>().CustomInstantiator(f => new LearningDomainOutcomeRecord(outcome, f.Random.Int(0, 10)));
    }


    public static Faker<User> GenerateUser()
    {
        return new Faker<User>()
            .CustomInstantiator(static f => new User(f.Random.Int(0).ToString(CultureInfo.CurrentCulture), f.Name.FullName(), new Uri(f.Image.PicsumUrl())));
    }

    public static Faker<LearningDomainSubmission> GenerateRandomLearningDomainSubmission(List<LearningDomainOutcome> outcomes)
    {
        var usedOutcomes = s_faker.PickRandom(outcomes, s_faker.Random.Int(3, 10)).ToList();
        return new Faker<LearningDomainSubmission>()
               .CustomInstantiator(f => new LearningDomainSubmission(f.Name.JobTitle(), new Uri(f.Internet.Url()), f.Date.Past(), GenerateRandomLearningDomainCriteriaList(usedOutcomes), GenerateRandomLearningDomainResultList(usedOutcomes)));
    }

    private static IEnumerable<LearningDomainCriteria> GenerateRandomLearningDomainCriteriaList(IEnumerable<LearningDomainOutcome> outcomes)
    {
        foreach (var outcome in outcomes)
        {
            yield return GenerateRandomLearningDomainCriteria(outcome).Generate();
        }
    }


    private static IEnumerable<LearningDomainOutcomeRecord> GenerateRandomLearningDomainResultList(IEnumerable<LearningDomainOutcome> outcomes)
    {
        foreach (var outcome in outcomes)
        {
            yield return GenerateRandomLearningDomainResult(outcome).Generate();
        }
    }
}