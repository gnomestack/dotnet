using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

using AngleSharp.Dom;
using AngleSharp.Html.Dom;

using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.Html;

[Cmdlet(VerbsCommon.Select, "Html")]
[OutputType(typeof(IHtmlElement[]), typeof(IHtmlElement))]
[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
public class SelectHtmlCmdlet : PSCmdlet
{
    private const string QueryParameterSet = "Query";
    private const string QuerySingleParameterSet = "QuerySingle";

    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = QueryParameterSet)]
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ParameterSetName = QuerySingleParameterSet)]
    public object InputObject { get; set; } = null!;

    [Parameter(Position = 1, ParameterSetName = QueryParameterSet)]
    public string? Query { get; set; }

    [Parameter(ParameterSetName = QuerySingleParameterSet)]
    public string? QuerySingle { get; set; }

    protected override void ProcessRecord()
    {
        if (this.InputObject is null)
        {
            this.WriteError(new PSArgumentNullException(nameof(this.InputObject)));
            return;
        }

        if (this.Query.IsNullOrWhiteSpace() && this.QuerySingle.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Query)));
            return;
        }

        try
        {
            HtmlDocument? html = this.InputObject switch
            {
                HtmlDocument html2 => html2,
                string content => new HtmlDocument(content),
                IHtmlDocument document => new HtmlDocument(document),
                IDocument document2 => new HtmlDocument(document2),
                _ => null,
            };

            if (html is not null)
            {
                if (!this.Query.IsNullOrWhiteSpace())
                {
                    var elements = html.Document.QuerySelectorAll(this.Query);
                    this.WriteObject(elements.ToArray(), false);
                    return;
                }

                if (!this.QuerySingle.IsNullOrWhiteSpace())
                {
                    var element = html.Document.QuerySelector(this.QuerySingle);
                    this.WriteObject(element, false);
                    return;
                }
            }

            if (this.InputObject is IHtmlElement element2)
            {
                if (!this.Query.IsNullOrWhiteSpace())
                {
                    var elements = element2.QuerySelectorAll(this.Query);
                    this.WriteObject(elements.ToArray(), false);
                    return;
                }

                if (!this.QuerySingle.IsNullOrWhiteSpace())
                {
                    var element = element2.QuerySelector(this.QuerySingle);
                    this.WriteObject(element, false);
                    return;
                }
            }

            if (this.InputObject is IEnumerable<IHtmlElement> elements2)
            {
                if (!this.Query.IsNullOrWhiteSpace())
                {
                    var elements = elements2.SelectMany(e => e.QuerySelectorAll(this.Query));
                    this.WriteObject(elements.ToArray(), false);
                    return;
                }

                if (!this.QuerySingle.IsNullOrWhiteSpace())
                {
                    var element = elements2.SelectMany(e => e.QuerySelectorAll(this.QuerySingle));
                    this.WriteObject(element, false);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            this.WriteError(ex);
        }

        this.WriteError(
            new NotSupportedException($"InputObject of type {this.InputObject.GetType()} is not supported"));
    }
}