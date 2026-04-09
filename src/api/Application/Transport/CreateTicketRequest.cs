namespace Todo.Api.Application.Transport;

/// <summary>
/// Inbound JSON body for <c>POST /api/v1/tickets</c>. Does not include requester identity;
/// the API resolves <c>requesterId</c> from the JWT and passes it to the service separately.
/// </summary>
public sealed class CreateTicketRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public string BusinessImpact { get; set; } = string.Empty;
    public string? CostContext { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string UrgencyLevelId { get; set; } = string.Empty;
}
