# InvoiceSystem — EF Core Code First
## Data Access IV Assignment | Full Implementation

---

## ✅ Assignment Checklist

| Requirement                 | Status | Implementation                                      |
|-----------------------------|--------|-----------------------------------------------------|
| Code First Approach         | ✔      | All entities defined as C# classes                  |
| Direct Relationships        | ✔      | Navigation properties with FK properties            |
| Indirect Relationships      | ✔      | N:M via InvoiceItem junction entity                 |
| Separated Migrations        | ✔      | M01–M06 — one migration per entity group            |
| Console Query Logger        | ✔      | `LogTo(Console.WriteLine, ...)` in OnConfiguring    |
| Config Files (Fluent API)   | ✔      | `IEntityTypeConfiguration<T>` per entity            |
| Connection from Config File | ✔      | `appsettings.json` read via `ConfigurationBuilder`  |

---

## 📁 Project Structure

```
InvoiceSystem/
├── Models/
│   ├── Category.cs          ← Strong Entity
│   ├── Product.cs           ← Strong Entity
│   ├── Customer.cs          ← Strong Entity
│   ├── Car.cs               ← Strong Entity (dependent in 1:1)
│   ├── Invoice.cs           ← Strong Entity
│   └── InvoiceItem.cs       ← Junction Entity (resolves N:M)
│
├── Configurations/          ← Config Files (Fluent API)
│   ├── CategoryConfig.cs
│   ├── ProductConfig.cs
│   ├── CustomerConfig.cs
│   ├── CarConfig.cs
│   ├── InvoiceConfig.cs
│   └── InvoiceItemConfig.cs
│
├── Data/
│   └── AppDbContext.cs      ← DbContext with Console Logger + Config reader
│
├── Migrations/              ← Separated Migrations
│   ├── M01_AddCategories.cs
│   ├── M02_AddProducts.cs
│   ├── M03_AddCustomers.cs
│   ├── M04_AddCars.cs
│   ├── M05_AddInvoices.cs
│   ├── M06_AddInvoiceItems.cs
│   └── MIGRATIONS_GUIDE.md
│
├── Program.cs               ← Entry point + demo queries
├── appsettings.json         ← Connection string config
└── InvoiceSystem.csproj     ← NuGet packages
```

---

## 🗃️ ERD → Logical Schema Mapping

### Entities & Attribute Types

| Entity       | PK (Key) | Simple Attributes     | Derived/Multi | FK (Relationship Attr)    |
|--------------|----------|-----------------------|---------------|---------------------------|
| Category     | Id       | Name, IsActive        | —             | —                         |
| Product      | Id       | Name, Price           | —             | CategoryId                |
| Customer     | Id       | Name, Email           | —             | —                         |
| Car          | Id       | Color, MaxSpeed       | —             | CustomerId (1:1)          |
| Invoice      | Id       | Date                  | —             | CustomerId                |
| InvoiceItem  | Id       | Qty, UnitPrice *      | —             | InvoiceId, ProductId      |

> *Qty and UnitPrice are **relationship attributes** — they come from the ERD diamond (Has) between Product and Invoice.

---

## 🔗 Relationships Analysis

### 1. Category → Product  (1:M)
```
Category ──────────< Product
   1                    M
Type      : Binary
Ratio     : 1:M
Direct    : Yes (FK CategoryId on Product)
Part.     : Total on Product (must belong to category)
OnDelete  : Restrict (can't delete category with products)
```

### 2. Product ↔ Invoice  (N:M — Indirect via InvoiceItem)
```
Product >────────── InvoiceItem ──────────< Invoice
   N                  (Qty, UnitPrice)          M
Type      : Binary (resolved as two 1:M)
Ratio     : N:M
Direct    : INDIRECT — resolved via junction entity
Part.     : Total on InvoiceItem side
OnDelete  : Cascade from Invoice, Restrict from Product
```

### 3. Customer → Invoice  (1:M)
```
Customer ──────────< Invoice
    1                   M
Type      : Binary
Ratio     : 1:M
Direct    : Yes (FK CustomerId on Invoice)
Part.     : Partial on Invoice (customer may have 0+)
OnDelete  : Restrict
```

### 4. Customer → Car  (1:1)
```
Customer ────────── Car
    1                 1
Type      : Binary
Ratio     : 1:1
Direct    : Yes (FK CustomerId on Car — Car is dependent)
Part.     : Total on Car (car must have an owner)
OnDelete  : Cascade
DB Enforce: UNIQUE index on Cars.CustomerId
```

---

## 🔄 Separated Migrations Order

```
M01_AddCategories   ← No dependencies
M02_AddProducts     ← Needs Categories
M03_AddCustomers    ← No dependencies
M04_AddCars         ← Needs Customers
M05_AddInvoices     ← Needs Customers
M06_AddInvoiceItems ← Needs Invoices + Products
```

### PMC Commands (run one at a time)
```powershell
Add-Migration M01_AddCategories   ; Update-Database
Add-Migration M02_AddProducts     ; Update-Database
Add-Migration M03_AddCustomers    ; Update-Database
Add-Migration M04_AddCars         ; Update-Database
Add-Migration M05_AddInvoices     ; Update-Database
Add-Migration M06_AddInvoiceItems ; Update-Database
```

---

## 📋 Console Query Logger Output Sample

When you run the application, you will see SQL printed to the console like:

```
info: 2026-03-30T10:00:00.000Z RelationalEventId.CommandExecuted
      Executed DbCommand (5ms) [Parameters=[@p0='Electronics' (Size = 100), @p1='True'],
      CommandType='Text', CommandTimeout='30']
      INSERT INTO [Categories] ([IsActive], [Name])
      VALUES (@p0, @p1);
      SELECT [Id] FROM [Categories]
      OUTPUT INSERTED.[Id]
      WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();
```

---

## 🚀 How to Run

```bash
# 1. Restore packages
dotnet restore

# 2. Update connection string in appsettings.json

# 3. Run migrations (PMC or CLI)
dotnet ef migrations add M01_AddCategories
dotnet ef database update
# ... repeat for M02–M06

# 4. Run the application
dotnet run
```
