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
        var enrolments = new AutoFaker<Enrollment>()
                         .RuleFor(static e => e.Type,
                             static f => f.Random.Bool()
                                 ? "StudentEnrollment"
                                 : f.Random.String())
                         .RuleFor(static e => e.User, static f => new User(f.Random.Int(0, 20).ToString(CultureInfo.CurrentCulture), f.Person.FullName, null))
                         .Generate(30);
        var courses = new AutoFaker<Course>()
                      .RuleFor(static c => c.Id, static f => f.Random.String())
                      .RuleFor(static c => c.Name, static f => f.Random.String())
                      .RuleFor(static c => c.Enrollments,  f => new GraphQlConnection<Enrollment>(enrolments))
                      .Generate(10);
        return new AutoFaker<GraphQlSchema>()
            .RuleFor(static g => g.Course,
                f => f.PickRandom(courses));
    }


    public static Faker<GraphQlSchema> GenerateUsersParticipatedTerms(Course currentCourse)
    {
        var terms = GenerateEnrollmentTerms().Generate(15);
        var enrolments = new AutoFaker<Enrollment>()
                         .RuleFor(static e => e.User, static _ => null)
                         .RuleFor(static e => e.Type, static _ => null)
                         .RuleFor(static e => e.Course, f => new Course(f.Random.Int().ToString(CultureInfo.CurrentCulture), f.Random.String(), terms[0], null, null));
        return new AutoFaker<GraphQlSchema>()
            .RuleFor(static g => g.LegacyNode, f => new LegacyNode(enrolments.Generate(1)));
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
        return new AutoFaker<GraphQlSchema>()
            .RuleFor(static g => g.LegacyNode,
                f => new AutoFaker<LegacyNode>()
                     .RuleFor(static l => l.Enrollments,
                         f =>
                             new AutoFaker<Enrollment>().RuleFor(static e => e.Course, f => GenerateCourse(outcomes).Generate()).Generate(20))
                     .Generate());
    }


    private static Faker<Submission> GenerateSubmissions(List<LearningDomainOutcome> outcomes)
    {
        return new AutoFaker<Submission>()
               .RuleFor(static s => s.PostedAt, static f => f.Date.Past())
               .RuleFor(static s => s.SubmittedAt, static f => f.Date.Past())
               .RuleFor(static s => s.Assignment, f => new Assignment(f.Random.String(), null, GenerateRubric(outcomes), new Uri(f.Internet.Url())));
    }


    private static Faker<Course> GenerateCourse(List<LearningDomainOutcome> outcomes)
    {
        return new AutoFaker<Course>()
               .RuleFor(static c => c.Id, static f => f.Random.String())
               .RuleFor(static c => c.Submissions, f => new GraphQlConnection<Submission>(GenerateSubmissions(outcomes).Generate(30)));
    }

    private static Rubric GenerateRubric(List<LearningDomainOutcome> outcomes)
    {
        return new Rubric(new AutoFaker<Criteria>()
                          .RuleFor(static c => c.Outcome, f => new Outcome(f.PickRandom(outcomes).Id, f.Random.String(), f.Random.Int(0, 10)))
                          .Generate(6));
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