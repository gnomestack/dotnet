namespace GnomeStack.Extensions.Search;

public interface IAsyncSearchIndex
{
    Task AddDocumentAsync<T>(T data, string primaryKey, CancellationToken cancellationToken = default)
        where T : notnull;

    Task AddDocumentsAsync<T>(IEnumerable<T> data, string primaryKey, CancellationToken cancellationToken = default)
        where T : notnull;

    Task RemoveDocumentAsync<TId>(TId primaryKey, CancellationToken cancellationToken = default);

    Task RemoveDocumentsAsync<TId>(IEnumerable<TId> primaryKeys, CancellationToken cancellationToken = default);

    Task UpdateDocumentAsync<T>(T data, string primaryKey, CancellationToken cancellationToken = default)
        where T : notnull;

    Task UpdateDocumentsAsync<T>(IEnumerable<T> data, string primaryKey, CancellationToken cancellationToken = default)
        where T : notnull;
}