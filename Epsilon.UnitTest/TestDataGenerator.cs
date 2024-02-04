using System.Collections.ObjectModel;
using System.Globalization;
using Bogus;
using Epsilon.Abstractions;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest;

public static class TestDataGenerator
{
    private static readonly Random s_random = new Random();
    private static readonly Faker s_faker = new Faker();

    public static LearningDomainType GenerateRandomLearningDomainType()
    {
        return new Faker<LearningDomainType>()
               .RuleFor(static o => o.Id, static f => f.Random.String())
               .RuleFor(static o => o.Name, static f => f.Random.String2(10))
               .RuleFor(static o => o.ShortName, static f => f.Random.String2(10))
               .RuleFor(static o => o.HexColor, static f => "FFFFFF")
               .RuleFor(static o => o.Order, static f => f.Random.Int())
               .Generate();
    }

    public static LearningDomainTypeSet GenerateRandomLearningDomainTypeSet()
    {
        return new LearningDomainTypeSet
        {
            Types = new List<LearningDomainType> { GenerateRandomLearningDomainType(), GenerateRandomLearningDomainType(), GenerateRandomLearningDomainType(), },
        };
    }

    public static LearningDomain GenerateRandomLearningDomain()
    {
        return new Faker<LearningDomain>()
               .RuleFor(static o => o.Id, static f => f.Random.String())
               .RuleFor(static o => o.ColumnsSet,
                   static f => f.Random.Bool()
                       ? GenerateRandomLearningDomainTypeSet()
                       : null)
               .RuleFor(static o => o.ValuesSet, static f => GenerateRandomLearningDomainTypeSet())
               .RuleFor(static o => o.RowsSet, static f => GenerateRandomLearningDomainTypeSet())
               .Generate();
    }

    public static LearningDomainOutcome GenerateRandomLearningDomainOutcome(LearningDomain domain)
    {
        return new Faker<LearningDomainOutcome>()
               .RuleFor(static o => o.Id, static f => f.Random.Int())
               .RuleFor(static o => o.Name, static f => f.Random.String2(10))
               .RuleFor(static o => o.Row, domain.RowsSet.Types.First())
               .RuleFor(static o => o.Column, domain.ColumnsSet?.Types.First())
               .RuleFor(static o => o.Value, domain.ValuesSet.Types.First())
               .Generate();
    }


    public static LearningDomainCriteria GenerateRandomLearningDomainCriteria(LearningDomainOutcome outcome)
    {
        return new LearningDomainCriteria(outcome.Id, s_faker.Random.Int(5, 10));
    }

    public static LearningDomainOutcomeRecord GenerateRandomLearningDomainResult(LearningDomainOutcome outcome)
    {
        return new LearningDomainOutcomeRecord(outcome, s_faker.Random.Int(0, 10));
    }

    public static Collection<LearningDomainOutcome> GenerateRandomLearningDomainOutcomes(int count, List<LearningDomain> domains)
    {
        var outcomes = new Collection<LearningDomainOutcome>();
        for (var i = 0; i < count; i++)
        {
            outcomes.Add(GenerateRandomLearningDomainOutcome(s_faker.PickRandom(domains)));
        }

        return outcomes;
    }


    public static Collection<User> GenerateUsers(int count)
    {
        var users = new Collection<User>();

        for (var i = 0; i < count; i++)
        {
            users.Add(new User(i.ToString(CultureInfo.CurrentCulture), $"test{i}", null));
        }

        return users;
    }


    public static IAsyncEnumerable<LearningDomainSubmission> GenerateRandomLearningDomainSubmissions(int count, List<LearningDomainOutcome> outcomes)
    {
        var l = new List<LearningDomainSubmission>();
        for (var i = 0; i < count; i++)
        {
            l.Add(GenerateRandomLearningDomainSubmission(outcomes.OrderBy(static x => Guid.NewGuid()).Take(5).ToList()));
        }

        return l.ToAsyncEnumerable();
    }


    public static LearningDomainSubmission GenerateRandomLearningDomainSubmission(List<LearningDomainOutcome> outcomes)
    {
        return new LearningDomainSubmission(s_faker.Name.JobTitle(),
            new Uri(s_faker.Internet.Url()),
            s_faker.Date.Past(),
            GenerateRandomLearningDomainCriteriaList(outcomes),
            s_faker.Random.Bool()
                ? new List<LearningDomainOutcomeRecord>()
                : GenerateRandomLearningDomainResultList(outcomes));
    }

    private static IEnumerable<LearningDomainCriteria> GenerateRandomLearningDomainCriteriaList(IEnumerable<LearningDomainOutcome> outcomes)
    {
        var l = new List<LearningDomainCriteria>();
        foreach (var outcome in outcomes)
        {
            l.Add(GenerateRandomLearningDomainCriteria(outcome));

        }
        return l;
    }
    
    
    public static IEnumerable<LearningDomain> GenerateRandomLearningDomains(int count)
    {
        var l = new List<LearningDomain>();
        for (var i = 0; i < count; i++)
        {
            l.Add(GenerateRandomLearningDomain());
        }

        return l;
    }


    private static IEnumerable<LearningDomainOutcomeRecord> GenerateRandomLearningDomainResultList(IEnumerable<LearningDomainOutcome> outcomes)
    {
        var l = new List<LearningDomainOutcomeRecord>();
        foreach (var outcome in outcomes)
        {
            l.Add(GenerateRandomLearningDomainResult(outcome));
        }
        return l;
    }
}