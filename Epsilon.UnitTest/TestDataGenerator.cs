using System.Globalization;
using Epsilon.Abstractions;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest;

public static class TestDataGenerator
{
    private static readonly Random s_random = new Random();

    public static LearningDomainType GenerateRandomLearningDomainType()
    {
        return new LearningDomainType
        {
            Name = $"Type {s_random.Next(1, 100)}", ShortName = $"T{s_random.Next(1, 100)}", HexColor = $"#{s_random.Next(0x1000000):X6}", Order = s_random.Next(1, 100),
        };
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
        return new LearningDomain
        {
            Id = $"Domain {s_random.Next(1, 100)}",
            RowsSet = GenerateRandomLearningDomainTypeSet(),
            ValuesSet = GenerateRandomLearningDomainTypeSet(),
        };
    }

    public static LearningDomainOutcome GenerateRandomLearningDomainOutcome()
    {
        return new LearningDomainOutcome
        {
            TenantId = Guid.NewGuid(),
            Row = GenerateRandomLearningDomainType(),
            Column = GenerateRandomLearningDomainType(),
            Value = GenerateRandomLearningDomainType(),
            Name = $"Outcome {s_random.Next(1, 100)}",
        };
    }

    public static List<LearningDomainOutcome> GenerateRandomLearningDomainOutcomes(int count)
    {
        var outcomes = new List<LearningDomainOutcome>();
        for (var i = 0; i < count; i++)
        {
            outcomes.Add(GenerateRandomLearningDomainOutcome());
        }

        return outcomes;
    }
    
    
    public static List<User> GenerateUsers(int count)
    {
        var users = new List<User>();

        for (var i = 0; i < count; i++)
        {
            users.Add(new User(i.ToString(CultureInfo.CurrentCulture), $"test{i}", null));
        }

        return users;
    }
}