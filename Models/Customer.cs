namespace InvoiceSystem.Models
{
    /// <summary>
    /// Customer Entity
    /// ERD: Strong Entity | Key Attribute: Id | Simple Attributes: Name, Email
    /// Relationships:
    ///   - 1-to-1 with Car      (One Customer OWNs One Car) — Binary, Degree=2
    ///   - 1-to-M with Invoice  (One Customer HAS Many Invoices) — Binary, Degree=2
    /// Cardinality Ratios:
    ///   - Customer:Car     = 1:1
    ///   - Customer:Invoice = 1:M
    /// Participation:
    ///   - Car is TOTAL participant (every car must belong to a customer)
    ///   - Invoice is PARTIAL participant (a customer may have 0+ invoices)
    /// </summary>
    public class Customer
    {
        // ── Key Attribute (PK) ──────────────────────────────────────
        public int Id { get; set; }

        // ── Simple Attributes ───────────────────────────────────────
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // ── Navigation Properties ───────────────────────────────────

        // 1:1 — Customer owns one Car (Direct relationship)
        public Car? Car { get; set; }

        // 1:M — Customer has many Invoices (Direct relationship)
        public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();
    }
}
