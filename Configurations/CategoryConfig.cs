using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSystem.Configurations
{
    /// <summary>
    /// Fluent API Configuration for Category Entity
    /// Config File approach — implements IEntityTypeConfiguration<T>
    /// </summary>
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // ── Table Name ──────────────────────────────────────────
            builder.ToTable("Categories");

            // ── Primary Key ─────────────────────────────────────────
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .UseIdentityColumn(); // Auto-increment PK

            // ── Properties ──────────────────────────────────────────
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnType("nvarchar(100)");

            builder.Property(c => c.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            // ── Relationship: 1 Category HAS Many Products ──────────
            // Cardinality Ratio : 1:M
            // Degree            : Binary
            // Participation     : Total on Product side (every product must have a category)
            builder.HasMany(c => c.Products)              // Navigation on Category side
                   .WithOne(p => p.Category)              // Navigation on Product side
                   .HasForeignKey(p => p.CategoryId)      // FK lives on Products table
                   .IsRequired()                          // Total participation of Product
                   .OnDelete(DeleteBehavior.Restrict);    // Prevent orphan products
        }
    }
}
