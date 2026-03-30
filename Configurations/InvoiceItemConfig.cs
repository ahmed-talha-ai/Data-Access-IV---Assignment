using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSystem.Configurations
{
    /// <summary>
    /// Fluent API Configuration for InvoiceItem Entity (Junction/Bridge Table)
    /// Config File approach — implements IEntityTypeConfiguration<T>
    ///
    /// This config handles the INDIRECT N:M relationship between Product and Invoice.
    /// InvoiceItem is the resolving entity with extra attributes: Qty, UnitPrice.
    /// </summary>
    public class InvoiceItemConfig : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            // ── Table Name ──────────────────────────────────────────
            builder.ToTable("InvoiceItems");

            // ── Primary Key ─────────────────────────────────────────
            builder.HasKey(ii => ii.Id);

            builder.Property(ii => ii.Id)
                   .UseIdentityColumn();

            // ── Descriptive / Relationship Attributes ───────────────
            // These attributes come from the ERD relationship diamond (Has)
            builder.Property(ii => ii.Qty)
                   .IsRequired()
                   .HasColumnType("int");

            builder.Property(ii => ii.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // ── FK Properties ───────────────────────────────────────
            builder.Property(ii => ii.InvoiceId)
                   .IsRequired();

            builder.Property(ii => ii.ProductId)
                   .IsRequired();

            // ── Relationship 1: M InvoiceItems → 1 Product ──────────
            // Part of the resolved N:M between Product and Invoice
            // Cardinality Ratio : N:M (Product:Invoice) resolved as 1:M here
            // Degree            : Binary
            // Participation     : Total on InvoiceItem side
            builder.HasOne(ii => ii.Product)              // Navigation on InvoiceItem
                   .WithMany(p => p.InvoiceItems)         // Navigation on Product
                   .HasForeignKey(ii => ii.ProductId)     // FK on InvoiceItems table
                   .IsRequired()                          // Every item must reference a product
                   .OnDelete(DeleteBehavior.Restrict);    // Don't cascade from product side

            // ── Note ────────────────────────────────────────────────
            // The InvoiceId FK relationship is configured in InvoiceConfig (HasMany/WithOne)
            // This avoids duplicate relationship registration in EF Core.
        }
    }
}
