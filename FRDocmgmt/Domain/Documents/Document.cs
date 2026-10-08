namespace FRDocmgmt.Domain.Documents;

public sealed class Document
{
    public const int MaximumTitleLength = 200;

    public Guid Id { get; }
    public string Title { get; }
    public DateTimeOffset CreatedAtUtc { get; }

    private Document(Guid id, string title, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Title = title;
        CreatedAtUtc = createdAtUtc;
    }

    public static Document Create(string title, DateTimeOffset createdAtUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        var normalizedTitle = title.Trim();
        if (normalizedTitle.Length > MaximumTitleLength)
        {
            throw new ArgumentException($"Title must not exceed {MaximumTitleLength} characters.", nameof(title));
        }

        return new Document(Guid.NewGuid(), normalizedTitle, createdAtUtc.ToUniversalTime());
    }
}
