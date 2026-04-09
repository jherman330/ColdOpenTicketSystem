using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Todo.Api.Domain.Entities;
using Todo.Api.Domain.Repositories;

namespace Todo.Api.Infrastructure.Data;

/// <summary>
/// Cosmos DB repository for tickets. Mirrors <see cref="CosmosDbRepositoryBase{T}"/> patterns (point read/write by <c>/id</c>, RU logging)
/// with ticket-specific audit rules: <c>schemaVersion</c> and audit fields are set from the entity's <see cref="Ticket.RequesterId"/> on create.
/// </summary>
public sealed class TicketRepository : ITicketRepository
{
    private const int InitialSchemaVersion = 1;

    private readonly Container _container;
    private readonly ILogger<TicketRepository> _logger;

    public TicketRepository(
        CosmosClient client,
        string databaseId,
        string containerId,
        ILogger<TicketRepository> logger)
    {
        _logger = logger;
        _container = client.GetContainer(databaseId, containerId);
    }

    /// <inheritdoc />
    public async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        PopulateWriteMetadata(ticket);

        var response = await _container.CreateItemAsync(
                ticket,
                new PartitionKey(ticket.Id),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        LogRequestCharge("Create", response.RequestCharge);
        return response.Resource;
    }

    /// <inheritdoc />
    public async Task<Ticket?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        try
        {
            var response = await _container.ReadItemAsync<Ticket>(
                    id,
                    new PartitionKey(id),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            LogRequestCharge("Read", response.RequestCharge);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    private void PopulateWriteMetadata(Ticket ticket)
    {
        var now = DateTime.UtcNow;
        ticket.SchemaVersion = InitialSchemaVersion;
        ticket.CreatedAt = now;
        ticket.UpdatedAt = now;
        ticket.CreatedBy = ticket.RequesterId;
        ticket.UpdatedBy = ticket.RequesterId;
    }

    private void LogRequestCharge(string operation, double requestCharge)
    {
        if (requestCharge > 0)
            _logger.LogDebug("Cosmos DB {Operation} request charge: {RequestCharge} RUs", operation, requestCharge);
    }
}
