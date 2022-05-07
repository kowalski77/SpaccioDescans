﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SpaccioDescans.SharedKernel.DDD;

namespace SpaccioDescans.Infrastructure.Transactions;

public static class MessagesDispatcherExtensions
{
    public static async Task PublishDomainEventsAsync(
        this IMediator mediator,
        DbContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any()).ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async domainEvent =>
            {
                await mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            });

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}