## Separated Migrations — EF Core Code First
## InvoiceSystem — Data Access IV Assignment
## ============================================================
##
## IMPORTANT: Run these ONE AT A TIME in Package Manager Console
## or via dotnet CLI. Each migration is SEPARATE as required.
##
## ORDER MATTERS — respect FK dependencies:
##   Categories → Products → Customers → Cars → Invoices → InvoiceItems
## ============================================================

## ── Step 0: Ensure EF Core tools are installed ──────────────
dotnet tool install --global dotnet-ef

## ── Migration 1: Categories table ───────────────────────────
## No FK dependencies — standalone table
Add-Migration M01_AddCategories
Update-Database

## ── Migration 2: Products table ─────────────────────────────
## Depends on: Categories (FK: CategoryId)
Add-Migration M02_AddProducts
Update-Database

## ── Migration 3: Customers table ────────────────────────────
## No FK dependencies — standalone table
Add-Migration M03_AddCustomers
Update-Database

## ── Migration 4: Cars table ─────────────────────────────────
## Depends on: Customers (FK: CustomerId — 1:1 relationship)
Add-Migration M04_AddCars
Update-Database

## ── Migration 5: Invoices table ─────────────────────────────
## Depends on: Customers (FK: CustomerId — 1:M relationship)
Add-Migration M05_AddInvoices
Update-Database

## ── Migration 6: InvoiceItems table ─────────────────────────
## Depends on: Invoices (FK: InvoiceId) AND Products (FK: ProductId)
## This resolves the N:M relationship between Product and Invoice
Add-Migration M06_AddInvoiceItems
Update-Database

## ── Alternative: dotnet CLI commands ───────────────────────
## dotnet ef migrations add M01_AddCategories
## dotnet ef database update
## dotnet ef migrations add M02_AddProducts
## dotnet ef database update
## ... and so on

## ── To rollback a specific migration ────────────────────────
## Update-Database M04_AddCars   ← rolls back to after M04
## Update-Database 0             ← rolls back ALL migrations

## ── To view migration history ────────────────────────────────
## Get-Migration
## dotnet ef migrations list
