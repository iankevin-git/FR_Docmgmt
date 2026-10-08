using FRDocmgmt.Domain.Documents;

namespace FRDocmgmt.Application.Documents;

public sealed class DocumentService(IDocumentRepository repository, TimeProvider timeProvider)
{
    public async Task<IReadOnlyList<DocumentDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var documents = await repository.ListAsync(cancellationToken);
        return documents.Select(ToDto).ToArray();
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var document = await repository.GetByIdAsync(id, cancellationToken);
        return document is null ? null : ToDto(document);
    }

    public async Task<DocumentDto> CreateAsync(CreateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        var document = Document.Create(request.Title, timeProvider.GetUtcNow());
        await repository.AddAsync(document, cancellationToken);
        return ToDto(document);
    }

    private static DocumentDto ToDto(Document document) =>
        new(document.Id, document.Title, document.CreatedAtUtc);
}
