using FRDocmgmt.Domain.Documents;

namespace FRDocmgmt.Application.Documents;

public interface IDocumentRepository
{
    Task<IReadOnlyList<Document>> ListAsync(CancellationToken cancellationToken = default);
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Document document, CancellationToken cancellationToken = default);
}
