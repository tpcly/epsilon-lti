using Epsilon.Abstractions.Components;
using Epsilon.Canvas.Abstractions.Rest;
using HtmlAgilityPack;

namespace Epsilon.Components;

public class PageComponentManager : IPageComponentManager
{
    private readonly ICanvasRestApi _canvasRestApi;

    public PageComponentManager(ICanvasRestApi canvasRestApi)
    {
        _canvasRestApi = canvasRestApi;
    }

    public async Task<PageComponent> Fetch(int courseId, string pageName)
    {
        var page = await _canvasRestApi.Pages.GetPage(courseId, pageName);

        return await ConvertPageToComponent(pageName, page);
    }

    public async Task<PageComponent> CreateOrUpdate(int courseId, string pageName, string body)
    {
        var page = await _canvasRestApi.Pages.UpdateOrCreatePage(courseId, new Page(pageName) { Title = pageName, Body = body, });

        return await ConvertPageToComponent(pageName, page);
    }

    private async Task<PageComponent> ConvertPageToComponent(string pageName, Page? page)
    {
        if (page == null)
        {
            return new PageComponent($"<p>Page {pageName} could not be found</p>");
        }

        if (page.Body == null)
        {
            return new PageComponent($"<p>Page {pageName} has an empty body</p>");
        }

        var html = await SubstituteImagesWithBase64(page.Body);

        return new PageComponent(html);
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