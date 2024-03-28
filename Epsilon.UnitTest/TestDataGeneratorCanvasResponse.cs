using System.Globalization;
using AutoBogus;
using Bogus;
using Epsilon.Abstractions;
using Tpcly.Canvas.Abstractions.GraphQl;

namespace Epsilon.UnitTest;

public static class TestDataGeneratorCanvasResponse
{

    public static Faker<GraphQlSchema> GenerateUsersEnrollmentsCourse()
    {
        var enrolments = new Faker<Enrollment>()
                         .CustomInstantiator(static f =>
                             new Enrollment(f.Random.Bool()
                                     ? "StudentEnrollment"
                                     : f.Random.String(),
                                 new User(f.Random.Int(0, 20).ToString(CultureInfo.CurrentCulture), f.Person.FullName, null),
                                 null))
                         .Generate(30);
        var courses = new Faker<Course>()
                      .CustomInstantiator(f => new Course(f.Random.String(), f.Random.String(), null, null, new GraphQlConnection<Enrollment>(enrolments)))
                      .Generate(10);
        return new Faker<GraphQlSchema>()
            .CustomInstantiator(f => new GraphQlSchema(null, f.PickRandom(courses), null));
    }


    public static Faker<GraphQlSchema> GenerateUsersParticipatedTerms(Course currentCourse)
    {
        var terms = GenerateEnrollmentTerms().Generate(15);
        var enrolments = new Faker<Enrollment>().CustomInstantiator(f =>
                                                    new Enrollment(f.Random.String(10),
                                                        null,
                                                        new Course(f.Random.Int().ToString(CultureInfo.CurrentCulture), f.Random.String(), terms[0], null, null)))
                                                .Generate(5);
        return new Faker<GraphQlSchema>()
            .CustomInstantiator(f => new GraphQlSchema(null, null, new LegacyNode(enrolments)));
    }


    public static Faker<EnrollmentTerm> GenerateEnrollmentTerms()
    {
        return new AutoFaker<EnrollmentTerm>().RuleFor(static f => f.Id, static f => f.Random.String())
                                              .RuleFor(static f => f.Name, static f => f.Random.String())
                                              .RuleFor(static f => f.StartAt, static f => f.Date.Past())
                                              .RuleFor(static f => f.EndAt, static f => f.Date.Soon(80, f.Date.Past()));
    }


    public static Faker<GraphQlSchema> GenerateCanvasSubmissionsResult(List<LearningDomainOutcome> outcomes)
    {
        var enrollments = new Faker<Enrollment>().CustomInstantiator(f => new Enrollment(null, null, GenerateCourse(outcomes).Generate()));
        return new Faker<GraphQlSchema>()
            .CustomInstantiator(f => new GraphQlSchema(null, null, new LegacyNode(enrollments.Generate(4))));
    }


    private static Faker<Submission> GenerateSubmissions(List<LearningDomainOutcome> outcomes)
    {
        var rubric = GenerateRubric(outcomes);
        var submissionHistory = GenerateSubmissionHistory(rubric);
        return new Faker<Submission>()
            .CustomInstantiator(f =>
                new Submission(f.Date.Past(), f.Date.Past(), new Assignment(f.Random.String(), null, rubric, new Uri(f.Internet.Url())), new GraphQlConnection<SubmissionHistory>(submissionHistory.Generate(3)), null));
    }


    private static Faker<Course> GenerateCourse(List<LearningDomainOutcome> outcomes)
    {
        var submissionList = new GraphQlConnection<Submission>(GenerateSubmissions(outcomes).Generate(10));
        return new Faker<Course>()
            .CustomInstantiator(f => new Course(f.Random.String(), null, null, submissionList, null));
    }

    private static Rubric GenerateRubric(List<LearningDomainOutcome> outcomes)
    {
        return new Rubric(new AutoFaker<Criteria>()
                          .RuleFor(static c => c.Outcome, f => new Outcome(f.PickRandom(outcomes).Id, f.Random.String(), f.Random.Int(0, 10)))
                          .Generate(6));
    }


    private static Faker<SubmissionHistory> GenerateSubmissionHistory(Rubric rubric)
    {
        return new Faker<SubmissionHistory>().CustomInstantiator(f => new SubmissionHistory(f.Random.Int(0, 4), f.Date.Past(), null, new GraphQlConnection<RubricAssessment>(GenerateRubricAssessment(rubric).Generate(10))));
    }
    private static Faker<RubricAssessment> GenerateRubricAssessment(Rubric rubric)
    {
        var assesment = new List<AssessmentRating?>();
        foreach (var criteria in rubric.Criteria!)
        {
            assesment.Add(new AssessmentRating(new Criterion(3, criteria.Outcome), 4));
        }
        return new Faker<RubricAssessment>().CustomInstantiator(f => new RubricAssessment(assesment));
    }
}