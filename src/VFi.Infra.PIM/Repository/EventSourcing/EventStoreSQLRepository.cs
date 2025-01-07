using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VFi.NetDevPack.Events;
using VFi.Infra.PIM.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Infra.PIM.Repository.EventSourcing
{
    public class EventStoreSqlRepository : IEventStoreRepository
    {
        private readonly EventStoreSqlContext _context;

        public EventStoreSqlRepository(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<EventStoreSqlContext>();
        }

        public async Task<IList<StoredEvent>> All(Guid aggregateId)
        {
            return await (from e in _context.StoredEvent where e.AggregateId == aggregateId select e).ToListAsync();
        }

        public void Store(StoredEvent theEvent)
        {
            _context.StoredEvent.Add(theEvent);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}