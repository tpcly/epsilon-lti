using System.Collections.ObjectModel;
using System.Globalization;
using Bogus;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
               .RuleFor(static o => o.HexColor, static f => f.Random.String2(10))
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

    public static LearningDomainOutcome GenerateRandomLearningDomainOutcome()
    {
        return new Faker<LearningDomainOutcome>()
               .RuleFor(static o => o.Id, static f => f.Random.Int())
               .RuleFor(static o => o.Name, static f => f.Random.String2(10))
               .RuleFor(static o => o.Row, GenerateRandomLearningDomainType())
               .RuleFor(static o => o.Column, GenerateRandomLearningDomainType())
               .RuleFor(static o => o.Value, GenerateRandomLearningDomainType())
               .Generate();
    }


    public static LearningDomainCriteria GenerateRandomLearningDomainCriteria()
    {
        return new LearningDomainCriteria(s_faker.Random.Int(), s_faker.Random.Int(5, 10));
    }

    public static LearningDomainOutcomeRecord GenerateRandomLearningDomainResult()
    {
        return new LearningDomainOutcomeRecord(GenerateRandomLearningDomainOutcome(), s_faker.Random.Int(0, 10));
    }

    public static Collection<LearningDomainOutcome> GenerateRandomLearningDomainOutcomes(int count)
    {
        var outcomes = new Collection<LearningDomainOutcome>();
        for (var i = 0; i < count; i++)
        {
            outcomes.Add(GenerateRandomLearningDomainOutcome());
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


    public static IAsyncEnumerable<LearningDomainSubmission> GenerateRandomLearningDomainSubmissions(int count)
    {
        var l = new List<LearningDomainSubmission>();
        for (var i = 0; i < count; i++)
        {
            l.Add(GenerateRandomLearningDomainSubmission());
        }

        return l.ToAsyncEnumerable();
    }


    public static LearningDomainSubmission GenerateRandomLearningDomainSubmission()
    {
        return new LearningDomainSubmission(s_faker.Name.JobTitle(),
            new Uri(s_faker.Internet.Url()),
            s_faker.Date.Past(),
            GenerateRandomLearningDomainCriteriaList(5),
            s_faker.Random.Bool()
                ? new List<LearningDomainOutcomeRecord>()
                : GenerateRandomLearningDomainResultList(5));
    }

    private static IEnumerable<LearningDomainCriteria> GenerateRandomLearningDomainCriteriaList(int count)
    {
        var l = new List<LearningDomainCriteria>();
        for (var i = 0; i < count; i++)
        {
            l.Add(GenerateRandomLearningDomainCriteria());
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


    private static IEnumerable<LearningDomainOutcomeRecord> GenerateRandomLearningDomainResultList(int count)
    {
        var l = new List<LearningDomainOutcomeRecord>();
        for (var i = 0; i < count; i++)
        {
            l.Add(GenerateRandomLearningDomainResult());
        }

        return l;
    }
}