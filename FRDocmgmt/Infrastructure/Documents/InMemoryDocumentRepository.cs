using System.Collections.Concurrent;
using FRDocmgmt.Application.Documents;
using FRDocmgmt.Domain.Documents;

namespace FRDocmgmt.Infrastructure.Documents;

internal sealed class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly ConcurrentDictionary<Guid, Document> documents = new();

    public Task<IReadOnlyList<Document>> ListAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyList<Document> result = documents.Values
            .OrderByDescending(document => document.CreatedAtUtc)
            .ThenBy(document => document.Id)
            .ToArray();
        return Task.FromResult(result);
    }

    public Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        documents.TryGetValue(id, out var document);
        return Task.FromResult(document);
    }

    public Task AddAsync(Document document, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!documents.TryAdd(document.Id, document))
        {
            throw new InvalidOperationException($"Document {document.Id} already exists.");
        }

        return Task.CompletedTask;
    }
}
