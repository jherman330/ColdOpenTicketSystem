namespace Todo.Api.Domain.Entities;

/// <summary>
/// Canonical domain record for a submitted request (Cosmos DB document model).
/// Audit fields are populated by <c>TicketRepository</c> on write, not by callers.
/// </summary>
public sealed class Ticket : IDomainEntity
{
    /// <summary>Document id (UUID, partition key <c>/id</c>).</summary>
    public string Id { get; set; } = string.Empty;

    /// <inheritdoc />
    public object PartitionKeyValue => Id;

    /// <summary>Schema version for lazy migration (Data Layer convention).</summary>
    public int SchemaVersion { get; set; }

    /// <summary>
    /// Lifecycle state (e.g. New, UnderReview, …). Initial creation uses <c>New</c> (enforced in TicketService).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public string BusinessImpact { get; set; } = string.Empty;

    /// <summary>Optional cost context (max 1000 chars at validation layer).</summary>
    public string? CostContext { get; set; }

    /// <summary>UUID of an admin-managed category.</summary>
    public string CategoryId { get; set; } = string.Empty;

    /// <summary>UUID of an admin-managed urgency level.</summary>
    public string UrgencyLevelId { get; set; } = string.Empty;

    /// <summary>Authenticated requester identity (JWT <c>sub</c>), never from the intake body.</summary>
    public string RequesterId { get; set; } = string.Empty;

    /// <summary>UTC timestamp when the requester submitted the ticket.</summary>
    public DateTime SubmittedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}
