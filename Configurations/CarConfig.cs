using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSystem.Configurations
{
    /// <summary>
    /// Fluent API Configuration for Car Entity
    /// Config File approach — implements IEntityTypeConfiguration<T>
    /// </summary>
    public class CarConfig : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            // ── Table Name ──────────────────────────────────────────
            builder.ToTable("Cars");

            // ── Primary Key ─────────────────────────────────────────
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .UseIdentityColumn();

            // ── Properties ──────────────────────────────────────────
            builder.Property(c => c.Color)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnType("nvarchar(50)");

            builder.Property(c => c.MaxSpeed)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            // ── FK Property ─────────────────────────────────────────
            // CustomerId is the FK for the 1:1 relationship with Customer
            // Car is the DEPENDENT entity — FK lives here
            // Relationship fully configured in CustomerConfig (HasOne/WithOne)
            builder.Property(c => c.CustomerId)
                   .IsRequired();

            // ── Unique Constraint on FK ──────────────────────────────
            // Enforces the 1:1 cardinality at database level
            builder.HasIndex(c => c.CustomerId)
                   .IsUnique();
        }
    }
}
