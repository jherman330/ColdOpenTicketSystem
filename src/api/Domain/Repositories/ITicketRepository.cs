using Todo.Api.Domain.Entities;

namespace Todo.Api.Domain.Repositories;

/// <summary>
/// Persistence for <see cref="Ticket"/> documents in the <c>ticket</c> Cosmos DB container (partition key <c>/id</c>).
/// </summary>
public interface ITicketRepository
{
    Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken = default);

    Task<Ticket?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
