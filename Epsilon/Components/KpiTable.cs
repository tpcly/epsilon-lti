using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
using LeftBorder = DocumentFormat.OpenXml.Wordprocessing.LeftBorder;
using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using TopBorder = DocumentFormat.OpenXml.Wordprocessing.TopBorder;

namespace Epsilon.Components;

public class KpiTable : AbstractCompetenceComponent
{
    public override async Task<Body>? AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();

        // Create a table to display outcomes, assignments, and grades
        var table = new Table();

        // Define column header texts
        var columnsHeaders = new Dictionary<string, string> { { "KPI", "3000" }, { "Assignments", "5000" }, { "Grades", "1000" }, }; 

        // Create the table header row
        var headerRow = new TableRow();

        // Create the header cells
        foreach (var columnHeader in columnsHeaders)
        {
            headerRow.AppendChild(CreateTableCellWithBorders(columnHeader.Value, new Paragraph(new Run(new Text(columnHeader.Key)))));
        }

        // Add the header row to the table
        table.AppendChild(headerRow);
        
        // // Create the table body rows and cells
        // foreach (var entry in KpiTableEntries.ToList().OrderByDescending(static e => e.Value.Kpi))
        // {
        //     var tableRow = new TableRow();
        //     
        //     // Outcome (KPI) column
        //     tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(entry.Value.Kpi)))));
        //     
        //     // Assignments column
        //     var assignmentsParagraph = new Paragraph();
        //     var assignmentsRun = assignmentsParagraph.AppendChild(new Run());
        //     
        //     foreach (var assignment in entry.Value.Assignments)
        //     {
        //         var rel = mainDocumentPart.AddHyperlinkRelationship(assignment.Link, true);
        //         var relationshipId = rel.Id;
        //         
        //         var runProperties = new RunProperties(
        //             new Underline { Val = UnderlineValues.Single, });
        //         
        //         assignmentsRun.AppendChild(new Hyperlink(new Run(runProperties, new Text(assignment.Name)))
        //         {
        //             History = OnOffValue.FromBoolean(true),
        //             Id = relationshipId,
        //         });
        //     
        //         assignmentsRun.AppendChild(new Break());
        //     }
        //     
        //     tableRow.AppendChild(CreateTableCellWithBorders("5000", assignmentsParagraph));
        //
        //     // Grades column
        //     var grades = entry.Value.Assignments.Select(static a => a.Grade);
        //     var gradesParagraph = new Paragraph();
        //     var gradesRun = gradesParagraph.AppendChild(new Run());
        //
        //     foreach (var grade in grades)
        //     {
        //         gradesRun.AppendChild(new Text(grade));
        //         gradesRun.AppendChild(new Break());
        //     }
        //
        //     tableRow.AppendChild(CreateTableCellWithBorders("1000", gradesParagraph));
        //     
        //     // Add the row to the table
        //     table.AppendChild(tableRow);
        // }
        
        // sort all outcomes from the 

        var allOutcomes = Submissions
            .SelectMany(static e => e.Results
                                     .Select(static result => result.Outcome)
                                     .ToAsyncEnumerable());

        // Create the table body rows and cells
        await foreach (var outcome in allOutcomes
                                      .OrderBy(static e => e.Value.Order)
                                      .Distinct())
        {
            var tableRow = new TableRow();
            
             // Outcome (KPI) column
            tableRow.AppendChild(CreateTableCellWithBorders("3000", new Paragraph(new Run(new Text(outcome.Name)))));
            
            // Assignments column
            var assignmentsParagraph = new Paragraph();
            var assignmentsRun = assignmentsParagraph.AppendChild(new Run());

            await foreach (var submission in Submissions.Select(static e => e))
            {
                if (submission.Results.Any(r => r.Outcome.Id == outcome.Id))
                {
                    var rel = mainDocumentPart.AddHyperlinkRelationship(new Uri(submission.AssignmentUrl!.ToString()), true);
                    var relationshipId = rel.Id;

                    var hyperlink = new Hyperlink(new RunProperties(
                        new Underline { Val = UnderlineValues.Single, }), new Text(submission.Assignment!)) { Id = relationshipId, };
                    
                    assignmentsRun.AppendChild(hyperlink);
                    assignmentsRun.AppendChild(new Paragraph());
                }
            }
            
            #region start of questionable code
            // var o = 0;
            // await foreach (var results in Submissions
            //                    .Select(static e => e.Results!))
            // {
            //     Console.WriteLine("foreach loop " + 2);
            //     var a = 0;
            //     await foreach (var criteriaConnectedAssignment in results.ToAsyncEnumerable())
            //     {
            //         Console.WriteLine("foreach loop " + 3);
            //         Console.WriteLine("Connected assignment: " + criteriaConnectedAssignment + " - a: " + a);
            //         if (criteriaConnectedAssignment.Outcome.Id == outcome.Id)
            //         {
            //             Console.WriteLine(true);
                            #region hyperlink
            
                                // await foreach (var uri in Submissions.Select(static e => e.AssignmentUrl))
                                // {
                                //     HyperlinkRelationship rel;
                                //     rel = mainDocumentPart.AddHyperlinkRelationship(uri, true);
                                // }
                                //
                                // var relationshipId = "0";
                                //
                                // var runProperties = new RunProperties(
                                //     new Underline { Val = UnderlineValues.Single, });
                                //
                                // assignmentsRun.AppendChild(new Hyperlink(new Run(runProperties, new Text(assignment.ToString())))
                                // {
                                //     History = OnOffValue.FromBoolean(true),
                                //     Id = relationshipId,
                                // });
                                // await foreach (var sub in Submissions.Select(static e => e.Assignment!.ToAsyncEnumerable().ToString()))
                                // {
                                //     var n = 0;
                                //     if (i == n)
                                //     {
                                //         assignmentsRun.AppendChild(new Text(sub!));
                                //     }
                                //
                                //     n++;
                                // }
                        
            
                    #endregion
            //                 await foreach (var sub in Submissions.Select(static e => e.AssignmentUrl!.ToString()))
            //                 {
            //                     Console.WriteLine("foreach loop " + 4);
            //                     Console.WriteLine("submission URL: "+sub + " - o: " + o);
            //                         if (a == o)
            //                         {
            //                             Console.WriteLine("Add submission to outcome " + a + "=" + o);
            //                             assignmentsRun.AppendChild(new Text(criteriaConnectedAssignment.Outcome.Name!));
            //                             assignmentsRun.AppendChild(new Paragraph());
            //                             assignmentsRun.AppendChild(new Text(sub!));
            //                             assignmentsRun.AppendChild(new Paragraph());
            //                             break;
            //                         }
            //                         Console.WriteLine("before add on a: "+a);
            //                         a++;
            //                         Console.WriteLine("after add on a: "+a);
            //                         // assignmentsRun.AppendChild(new Text(Submissions.Select(static e => e.Assignment).ToString()!));
            //                 }
            //         }
            //         else
            //         {
            //             Console.WriteLine(false);
            //         }
            //         Console.WriteLine("before add on o: "+o);
            //         o++;
            //         Console.WriteLine("after add on o: "+o);
            //     }
            // }
            //
            tableRow.AppendChild(CreateTableCellWithBorders("5000", assignmentsParagraph));
            //
            // // Grades column
            // var grades = entry.Value.Assignments.Select(static a => a.Grade);
            // var gradesParagraph = new Paragraph();
            // var gradesRun = gradesParagraph.AppendChild(new Run());
            //
            // foreach (var grade in grades)
            // {
            //     gradesRun.AppendChild(new Text(grade));
            //     gradesRun.AppendChild(new Break());
            // }
            //
            // tableRow.AppendChild(CreateTableCellWithBorders("1000", gradesParagraph));
            #endregion 
            
             // Add the row to the table
            table.AppendChild(tableRow);
        }
        
        // Newline to separate the table from the rest of the document
        body.Append(new Paragraph(new Run(new Text(""))));
        
        // Add the table to the document
        body.AppendChild(table);

        mainDocumentPart.Document.AppendChild(body);
        return null;
    }

    private static TableCell CreateTableCellWithBorders(string? width, params OpenXmlElement[] elements)
    {
        var cell = new TableCell();
        var cellProperties = new TableCellProperties();
        var borders = new TableCellBorders(
            new LeftBorder
            {
                Val = BorderValues.Single,
            },
            new RightBorder
            {
                Val = BorderValues.Single,
            },
            new TopBorder
            {
                Val = BorderValues.Single,
            },
            new BottomBorder
            {
                Val = BorderValues.Single,
            });

        foreach (var element in elements)
        {
            cell.Append(element);
        }

        if (width != null)
        {
            cellProperties.Append(new TableCellWidth
            {
                Type = TableWidthUnitValues.Dxa,
                Width = width,
            });
        }

        cellProperties.Append(borders);
        cell.PrependChild(cellProperties);

        return cell;
    }

    public KpiTable(IAsyncEnumerable<LearningDomainSubmission> submissions, IEnumerable<LearningDomain?> domains)
        : base(submissions, domains)
    {
    }
}