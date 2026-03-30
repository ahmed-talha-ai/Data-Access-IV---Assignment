using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceSystem.Configurations
{
    /// <summary>
    /// Fluent API Configuration for Customer Entity
    /// Config File approach — implements IEntityTypeConfiguration<T>
    /// </summary>
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // ── Table Name ──────────────────────────────────────────
            builder.ToTable("Customers");

            // ── Primary Key ─────────────────────────────────────────
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .UseIdentityColumn();

            // ── Properties ──────────────────────────────────────────
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150)
                   .HasColumnType("nvarchar(150)");

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(200)
                   .HasColumnType("nvarchar(200)");

            // Unique constraint on Email
            builder.HasIndex(c => c.Email)
                   .IsUnique();

            // ── Relationship 1: 1 Customer OWNs 1 Car ───────────────
            // Cardinality Ratio : 1:1
            // Degree            : Binary
            // Participation     : Total on Car side (Car must have an owner)
            //                     Partial on Customer side (Customer may not have a Car)
            builder.HasOne(c => c.Car)               // Navigation on Customer
                   .WithOne(car => car.Customer)     // Navigation on Car
                   .HasForeignKey<Car>(car => car.CustomerId)  // FK lives on Cars table (Car = dependent)
                   .IsRequired()                     // Car must reference a Customer
                   .OnDelete(DeleteBehavior.Cascade); // Deleting Customer cascades to Car

            // ── Relationship 2: 1 Customer HAS Many Invoices ────────
            // Cardinality Ratio : 1:M
            // Degree            : Binary
            // Participation     : Partial on Invoice side (customer may have 0 invoices)
            builder.HasMany(c => c.Invoices)              // Navigation on Customer
                   .WithOne(i => i.Customer)              // Navigation on Invoice
                   .HasForeignKey(i => i.CustomerId)      // FK lives on Invoices table
                   .IsRequired()                          // Invoice must have a customer
                   .OnDelete(DeleteBehavior.Restrict);    // Don't cascade delete invoices
        }
    }
}
