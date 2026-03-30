using InvoiceSystem.Data;
using InvoiceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem
{
    /// <summary>
    /// Entry Point — Demonstrates:
    ///   ✔ DbContext usage
    ///   ✔ Console Query Logger output (visible in terminal)
    ///   ✔ All relationships working correctly
    ///   ✔ Sample CRUD operations per entity
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("   InvoiceSystem — EF Core Code First | Data IV   ");
            Console.WriteLine("═══════════════════════════════════════════════════\n");

            using var context = new AppDbContext();

            // ── Ensure database is created ──────────────────────────
            // In Code First with migrations, use: Update-Database
            // For quick testing without CLI, uncomment the line below:
            // await context.Database.MigrateAsync();

            // ──────────────────────────────────────────────────────────
            // DEMO 1: Seed Data — All entities & relationships
            // Console Query Logger will print the SQL to terminal
            // ──────────────────────────────────────────────────────────
            Console.WriteLine("▶ Seeding data...\n");

            // ── Category (1:M → Product) ────────────────────────────
            var electronics = new Category { Name = "Electronics", IsActive = true };
            var clothing     = new Category { Name = "Clothing",    IsActive = true };
            context.Categories.AddRange(electronics, clothing);
            await context.SaveChangesAsync();
            Console.WriteLine($"  ✔ Categories saved — Ids: {electronics.Id}, {clothing.Id}");

            // ── Products (M:1 → Category | N:M → Invoice via InvoiceItems) ──
            var laptop = new Product { Name = "Laptop Pro",    Price = 1500.00m, CategoryId = electronics.Id };
            var phone  = new Product { Name = "SmartPhone X",  Price = 900.00m,  CategoryId = electronics.Id };
            var shirt  = new Product { Name = "Cotton Shirt",  Price = 45.00m,   CategoryId = clothing.Id    };
            context.Products.AddRange(laptop, phone, shirt);
            await context.SaveChangesAsync();
            Console.WriteLine($"  ✔ Products saved — Ids: {laptop.Id}, {phone.Id}, {shirt.Id}");

            // ── Customer (1:1 → Car | 1:M → Invoice) ───────────────
            var customer1 = new Customer { Name = "Ahmed Ali",   Email = "ahmed@example.com" };
            var customer2 = new Customer { Name = "Sara Hassan", Email = "sara@example.com"  };
            context.Customers.AddRange(customer1, customer2);
            await context.SaveChangesAsync();
            Console.WriteLine($"  ✔ Customers saved — Ids: {customer1.Id}, {customer2.Id}");

            // ── Cars (1:1 → Customer) ───────────────────────────────
            var car1 = new Car { Color = "Black", MaxSpeed = 220.5m, CustomerId = customer1.Id };
            var car2 = new Car { Color = "White", MaxSpeed = 180.0m, CustomerId = customer2.Id };
            context.Cars.AddRange(car1, car2);
            await context.SaveChangesAsync();
            Console.WriteLine($"  ✔ Cars saved — Ids: {car1.Id}, {car2.Id}");

            // ── Invoices (M:1 → Customer | 1:M → InvoiceItems) ─────
            var invoice1 = new Invoice { Date = DateTime.UtcNow, CustomerId = customer1.Id };
            var invoice2 = new Invoice { Date = DateTime.UtcNow, CustomerId = customer2.Id };
            context.Invoices.AddRange(invoice1, invoice2);
            await context.SaveChangesAsync();
            Console.WriteLine($"  ✔ Invoices saved — Ids: {invoice1.Id}, {invoice2.Id}");

            // ── InvoiceItems (M:1 → Invoice | M:1 → Product) ───────
            // This is the junction table resolving Product ↔ Invoice (N:M)
            var items = new List<InvoiceItem>
            {
                new() { InvoiceId = invoice1.Id, ProductId = laptop.Id, Qty = 1, UnitPrice = 1500.00m },
                new() { InvoiceId = invoice1.Id, ProductId = phone.Id,  Qty = 2, UnitPrice = 900.00m  },
                new() { InvoiceId = invoice2.Id, ProductId = shirt.Id,  Qty = 3, UnitPrice = 45.00m   },
                new() { InvoiceId = invoice2.Id, ProductId = laptop.Id, Qty = 1, UnitPrice = 1480.00m }
            };
            context.InvoiceItems.AddRange(items);
            await context.SaveChangesAsync();
            Console.WriteLine($"  ✔ InvoiceItems saved — {items.Count} line items\n");

            // ──────────────────────────────────────────────────────────
            // DEMO 2: Query with Eager Loading — Console logger prints SQL
            // ──────────────────────────────────────────────────────────
            Console.WriteLine("▶ Query 1 — Invoices with Customer, Items, and Products:");
            Console.WriteLine("  (Watch SQL printed by Console Query Logger below)\n");

            var invoices = await context.Invoices
                .Include(i => i.Customer)
                    .ThenInclude(c => c.Car)
                .Include(i => i.InvoiceItems)
                    .ThenInclude(ii => ii.Product)
                        .ThenInclude(p => p.Category)
                .ToListAsync();

            foreach (var inv in invoices)
            {
                Console.WriteLine($"\n  Invoice #{inv.Id} | Date: {inv.Date:yyyy-MM-dd}");
                Console.WriteLine($"  Customer : {inv.Customer.Name} ({inv.Customer.Email})");
                Console.WriteLine($"  Car      : {inv.Customer.Car?.Color} @ {inv.Customer.Car?.MaxSpeed} km/h");
                Console.WriteLine($"  Items    :");
                foreach (var item in inv.InvoiceItems)
                {
                    Console.WriteLine($"    → [{item.Product.Category.Name}] " +
                                      $"{item.Product.Name} × {item.Qty} @ {item.UnitPrice:C}");
                }
                var total = inv.InvoiceItems.Sum(x => x.Qty * x.UnitPrice);
                Console.WriteLine($"  TOTAL    : {total:C}");
            }

            // ──────────────────────────────────────────────────────────
            // DEMO 3: Query — Categories with their Products
            // ──────────────────────────────────────────────────────────
            Console.WriteLine("\n▶ Query 2 — Active Categories and their Products:\n");

            var categories = await context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Products)
                .ToListAsync();

            foreach (var cat in categories)
            {
                Console.WriteLine($"  [{cat.Id}] {cat.Name} — {cat.Products.Count} product(s)");
                foreach (var p in cat.Products)
                    Console.WriteLine($"       • {p.Name} — {p.Price:C}");
            }

            // ──────────────────────────────────────────────────────────
            // DEMO 4: Query — Products that appear on more than 1 invoice
            // Demonstrates the N:M resolved relationship in action
            // ──────────────────────────────────────────────────────────
            Console.WriteLine("\n▶ Query 3 — Products sold on multiple invoices (N:M proof):\n");

            var multiInvoiceProducts = await context.Products
                .Where(p => p.InvoiceItems
                    .Select(ii => ii.InvoiceId)
                    .Distinct()
                    .Count() > 1)
                .Include(p => p.InvoiceItems)
                .ToListAsync();

            foreach (var p in multiInvoiceProducts)
            {
                var invoiceIds = p.InvoiceItems.Select(ii => ii.InvoiceId).Distinct();
                Console.WriteLine($"  {p.Name} appears on invoices: [{string.Join(", ", invoiceIds)}]");
            }

            Console.WriteLine("\n═══════════════════════════════════════════════════");
            Console.WriteLine("   All Done! Check console above for SQL logs.");
            Console.WriteLine("═══════════════════════════════════════════════════\n");
        }
    }
}
