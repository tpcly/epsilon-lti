using Epsilon.Abstractions.Components;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Rest;
using HtmlAgilityPack;

namespace Epsilon.Components;

public class PageComponentFetcher : IFetcher<PageComponent>
{
    private readonly ICanvasRestApi _canvasRestApi;
    private readonly CanvasUserSession _canvasUserSession;
    private readonly string _pageName;

    public PageComponentFetcher(ICanvasRestApi canvasRestApi, CanvasUserSession canvasUserSession, string pageName)
    {
        _canvasRestApi = canvasRestApi;
        _canvasUserSession = canvasUserSession;
        _pageName = pageName;
    }

    public async Task<PageComponent> Fetch(DateTime from, DateTime to)
    {
        var personaPage = await _canvasRestApi.Pages.GetPage(_canvasUserSession.CourseId, _pageName);
        if (personaPage == null)
        {
            return new PageComponent($"<p>Page {_pageName} could not be found</p>");
        }

        if (personaPage.Body == null)
        {
            return new PageComponent($"<p>Page {_pageName} has an empty body</p>");
        }

        var updatedPersonaHtml = await SubstituteImagesWithBase64(personaPage.Body);

        return new PageComponent(updatedPersonaHtml);
    }

    private async Task<string> SubstituteImagesWithBase64(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        if (document.DocumentNode.SelectNodes("//img") == null)
        {
            return html;
        }

        foreach (var node in document.DocumentNode.SelectNodes("//img"))
        {
            var imageSrc = node.SelectNodes("//img").First().Attributes["src"].Value;

            if (imageSrc == null)
            {
                continue;
            }

            var imageBytes = await _canvasRestApi.Files.GetByteArray(new Uri(imageSrc));

            if (imageBytes != null)
            {
                var imageBase64 = Convert.ToBase64String(imageBytes.ToArray());
                node.SetAttributeValue("src", $"data:image/jpeg;base64,{imageBase64}");
            }
        }

        return document.Text;
    }
}