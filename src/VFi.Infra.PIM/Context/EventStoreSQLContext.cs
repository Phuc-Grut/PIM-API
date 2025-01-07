using VFi.NetDevPack.Events;
using VFi.Infra.PIM.Mappings;
using Microsoft.EntityFrameworkCore;


namespace VFi.Infra.PIM.Context
{
    public class EventStoreSqlContext : DbContext
    {
        public EventStoreSqlContext(DbContextOptions<EventStoreSqlContext> options) : base(options) { }

        public DbSet<StoredEvent> StoredEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}