using System.ComponentModel.DataAnnotations;
using FRDocmgmt.Domain.Documents;

namespace FRDocmgmt.Application.Documents;

public sealed record CreateDocumentRequest(
    [property: Required]
    [property: StringLength(Document.MaximumTitleLength)] string Title);
