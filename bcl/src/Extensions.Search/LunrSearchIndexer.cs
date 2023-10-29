namespace GnomeStack.Extensions.Search;

public class LunrSearchIndexer
{
    public void Index<T>(T data)
    {
        Lunr.Index.Build((builder) =>
        {
            builder.
        })
    }
}