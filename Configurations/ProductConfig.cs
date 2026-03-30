using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSystem.Configurations
{
    /// <summary>
    /// Fluent API Configuration for Product Entity
    /// Config File approach — implements IEntityTypeConfiguration<T>
    /// </summary>
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // ── Table Name ──────────────────────────────────────────
            builder.ToTable("Products");

            // ── Primary Key ─────────────────────────────────────────
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .UseIdentityColumn();

            // ── Properties ──────────────────────────────────────────
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200)
                   .HasColumnType("nvarchar(200)");

            builder.Property(p => p.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // ── FK Property ─────────────────────────────────────────
            // CategoryId FK — configured in CategoryConfig via HasMany/WithOne
            builder.Property(p => p.CategoryId)
                   .IsRequired();

            // ── Note on N:M Relationship ────────────────────────────
            // The N:M relationship between Product and Invoice is INDIRECT.
            // It is resolved through the InvoiceItem junction entity.
            // The HasMany(p => p.InvoiceItems) side is configured in InvoiceItemConfig.
        }
    }
}
