using FRDocmgmt.Application.Documents;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FRDocmgmt.Api.Endpoints;

public static class DocumentEndpoints
{
    public static IEndpointRouteBuilder MapDocumentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/documents").WithTags("Documents");

        group.MapGet("/", async (DocumentService service, CancellationToken cancellationToken) =>
            TypedResults.Ok(await service.ListAsync(cancellationToken)))
            .WithName("ListDocuments");

        group.MapGet("/{id:guid}", async Task<Results<Ok<DocumentDto>, NotFound>> (
            Guid id, DocumentService service, CancellationToken cancellationToken) =>
        {
            var document = await service.GetByIdAsync(id, cancellationToken);
            return document is null ? TypedResults.NotFound() : TypedResults.Ok(document);
        }).WithName("GetDocument");

        group.MapPost("/", async (CreateDocumentRequest request, DocumentService service,
            CancellationToken cancellationToken) =>
        {
            var document = await service.CreateAsync(request, cancellationToken);
            return TypedResults.Created($"/api/documents/{document.Id}", document);
        }).WithName("CreateDocument");

        return endpoints;
    }
}
