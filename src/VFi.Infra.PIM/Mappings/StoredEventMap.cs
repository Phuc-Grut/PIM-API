using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VFi.NetDevPack.Events;

namespace VFi.Infra.PIM.Mappings
{    
    public class StoredEventMap : IEntityTypeConfiguration<StoredEvent>
    {
        public void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            builder.ToTable("StoredEvent", "pim");

            builder.Property(c => c.Timestamp)
                .HasColumnName("CreationDate");

            builder.Property(c => c.MessageType)
                .HasColumnName("Action")
                .HasColumnType("varchar(100)");
        }
    }
}