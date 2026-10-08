namespace FRDocmgmt.Application.Documents;

public sealed record DocumentDto(Guid Id, string Title, DateTimeOffset CreatedAtUtc);
