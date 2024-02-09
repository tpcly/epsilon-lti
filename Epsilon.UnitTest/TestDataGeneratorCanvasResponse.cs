using System.Globalization;
using Bogus;
using Epsilon.Abstractions;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest;

public static class TestDataGeneratorCanvasResponse
{

    public static Faker<GraphQlSchema> GenerateUsersEnrollmentsCourse()
    {
        return new Faker<GraphQlSchema>().CustomInstantiator(static f =>
            new GraphQlSchema(null,
                new Course(null,
                    null,
                    null,
                    null,
                    new GraphQlConnection<Enrollment>(
                        new Faker<Enrollment>().CustomInstantiator(static f =>
                                                   new Enrollment(
                                                       f.Random.Bool()
                                                           ? "StudentEnrollment"
                                                           : f.Random.String(),
                                                       new User(f.Random.Int(0, 20).ToString(CultureInfo.CurrentCulture), f.Person.FullName, null),
                                                       null))
                                               .Generate(40))),
                null));
    }


    public static Faker<GraphQlSchema> GenerateUsersParticipatedTerms(Course currentCourse)
    {
        var terms = GenerateEnrollmentTerms().Generate(15);
        return new Faker<GraphQlSchema>().CustomInstantiator(_ =>
            new GraphQlSchema(null,
                null,
                new LegacyNode(new Faker<Enrollment>().CustomInstantiator(f =>
                                                          new Enrollment(null,
                                                              null,
                                                              new Course(f.Random.Int().ToString(CultureInfo.CurrentCulture), f.Random.String(), f.PickRandom(terms), null, null)))
                                                      .Generate(30).Append(new Enrollment(null, null, currentCourse)))));
    }


    public static Faker<EnrollmentTerm> GenerateEnrollmentTerms()
    {
        return new Faker<EnrollmentTerm>().CustomInstantiator(static f =>
        {
            var startDate = f.Date.Past();
            return new EnrollmentTerm(f.Random.String(), f.Random.String(), startDate, f.Date.Soon(80, startDate));
        });
    }


    public static Faker<GraphQlSchema> GenerateCanvasSubmissionsResult(List<LearningDomainOutcome> outcomes)
    {
        return new Faker<GraphQlSchema>().CustomInstantiator(f =>
            new GraphQlSchema(null,
                null,
                new LegacyNode(
                    new Faker<Enrollment>().CustomInstantiator(_ => new Enrollment(null,
                        null,
                        new Course(f.Random.String(),
                            f.Random.String(),
                            null,
                            new GraphQlConnection<Submission>(
                                GenerateSubmissions(outcomes).Generate(30)),
                            null)
                    )).Generate(3)
                )));
    }


    private static Faker<Submission> GenerateSubmissions(List<LearningDomainOutcome> outcomes)
    {
        return new Faker<Submission>()
            .CustomInstantiator(f => new Submission(f.Date.Past(),
                f.Date.Past(),
                new Assignment(f.Random.String(),
                    null,
                    GenerateRubric(outcomes),
                    new Uri(f.Internet.Url())),
                null,
                null));
    }
    
    private static Rubric GenerateRubric(List<LearningDomainOutcome> outcomes)
    {
        return new Rubric(new Faker<Criteria>().CustomInstantiator(f =>
        {
            var o = f.PickRandom(outcomes);
            return new Criteria(new Outcome(o.Id, o.Name, f.Random.Int(0, 10)));
        }).Generate(6));
    }
    
    
    // private static Faker<SubmissionHistory> GenerateSubmissionHistory(Rubric rubric)
    // {
    //     return new Faker<SubmissionHistory>().CustomInstantiator(f => new SubmissionHistory(f.Random.Int(0, 4), f.Date.Past(), null, new GraphQlConnection<RubricAssessment>()));
    // }
    //
    // private static Faker<RubricAssessment> GenerateRubricAssessment(Rubric rubric)
    // {
    //     return new Faker<RubricAssessment>().CustomInstantiator(f => new RubricAssessment());
    // }
}