using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Rest;

public record Page
{
    public Page(string id)
    {
        // Required due to the fact that Canvas uses both the PageId and Url as identifiers.
        if (int.TryParse(id, out var pageId))
        {
            PageId = pageId;
        }
        else
        {
            Url = id;
        }
    }

    [JsonIgnore]
    public string Id => (
        PageId != null
            ? PageId.ToString()
            : Url
    ) ?? throw new InvalidOperationException("Both should not be null, either PageId or Url has a value");

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; init; }

    [JsonPropertyName("url")]
    private string? Url { get; init; }

    [JsonPropertyName("editing_roles")]
    public string? EditingRoles { get; init; }

    [JsonPropertyName("page_id")]
    private int? PageId { get; init; }

    [JsonPropertyName("published")]
    public bool? Published { get; init; }

    [JsonPropertyName("hide_from_students")]
    public bool? HideFromStudents { get; init; }

    [JsonPropertyName("front_page")]
    public bool? FrontPage { get; init; }

    [JsonPropertyName("html_url")]
    public Uri? HtmlUrl { get; init; }

    [JsonPropertyName("todo_date")]
    public DateTime? TodoDate { get; init; }

    [JsonPropertyName("publish_at")]
    public DateTime? PublishAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; init; }

    [JsonPropertyName("locked_for_user")]
    public bool? LockedForUser { get; init; }

    [JsonPropertyName("body")]
    public string? Body { get; init; }
}