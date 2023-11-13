using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

namespace GnomeStack.PowerShell.Html;

public sealed class HtmlDocument : IDisposable
{
    public HtmlDocument(string content)
    {
        var parser = new AngleSharp.Html.Parser.HtmlParser();
        this.Document = parser.ParseDocument(content);
    }

    public HtmlDocument(IHtmlDocument document)
    {
        this.Document = document;
    }

    public HtmlDocument(IDocument document)
    {
        this.Document = document;
    }

    public static HtmlDocument Empty { get; } = Create();

    public IDocument Document { get; }

    public static implicit operator HtmlDocument(string html)
        => new HtmlDocument(html);

    public static implicit operator HtmlDocument(AngleSharp.Dom.Document document)
        => new HtmlDocument((IHtmlDocument)document);

    public static implicit operator string(HtmlDocument htmlDocument)
        => htmlDocument.ToString();

    public static HtmlDocument From(IHtmlDocument document)
        => new HtmlDocument(document);

    public static HtmlDocument From(IDocument document)
        => new HtmlDocument(document);

    public static HtmlDocument Create()
    {
        return new HtmlDocument("<html><head></head><body></body></html>");
    }

    public static HtmlDocument Request(Uri uri)
    {
        return Request(uri.ToString());
    }

    public static HtmlDocument Request(string uri)
    {
        var config = new Configuration().WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = context.OpenAsync(uri).GetAwaiter().GetResult();
        if (document is null)
            throw new InvalidOperationException("Document is null from the request.");
        return new HtmlDocument(document);
    }

    public override string ToString()
    {
        return this.Document.ToString();
    }

    public void Dispose()
    {
        this.Document.Dispose();
    }
}