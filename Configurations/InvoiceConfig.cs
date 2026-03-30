using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSystem.Configurations
{
    /// <summary>
    /// Fluent API Configuration for Invoice Entity
    /// Config File approach — implements IEntityTypeConfiguration<T>
    /// </summary>
    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // ── Table Name ──────────────────────────────────────────
            builder.ToTable("Invoices");

            // ── Primary Key ─────────────────────────────────────────
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .UseIdentityColumn();

            // ── Properties ──────────────────────────────────────────
            builder.Property(i => i.Date)
                   .IsRequired()
                   .HasColumnType("datetime2")
                   .HasDefaultValueSql("GETDATE()"); // Default to current date

            // ── FK Property ─────────────────────────────────────────
            builder.Property(i => i.CustomerId)
                   .IsRequired();

            // ── Relationship: 1 Invoice HAS Many InvoiceItems ────────
            // Cardinality Ratio : 1:M
            // Degree            : Binary
            // Participation     : Total on InvoiceItem side (every item must belong to an invoice)
            builder.HasMany(i => i.InvoiceItems)             // Navigation on Invoice
                   .WithOne(ii => ii.Invoice)                // Navigation on InvoiceItem
                   .HasForeignKey(ii => ii.InvoiceId)        // FK on InvoiceItems table
                   .IsRequired()                             // Total participation
                   .OnDelete(DeleteBehavior.Cascade);        // Delete invoice → delete its items
        }
    }
}
