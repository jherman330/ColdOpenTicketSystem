namespace Todo.Api.Application.Transport;

/// <summary>
/// Response payload after a ticket is created (201 Created body per Request Submission contract).
/// </summary>
public sealed class TicketConfirmationDto
{
    public string TicketId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public string RequesterId { get; set; } = string.Empty;
}
