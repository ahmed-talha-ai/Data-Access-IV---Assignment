namespace InvoiceSystem.Models
{
    /// <summary>
    /// Invoice Entity
    /// ERD: Strong Entity | Key Attribute: Id | Simple Attribute: Date
    /// Relationships:
    ///   - M-to-1 with Customer    (Many Invoices belong to One Customer)
    ///   - 1-to-M with InvoiceItem (One Invoice HAS Many InvoiceItems)
    /// Cardinality Ratios:
    ///   - Invoice:Customer     = M:1
    ///   - Invoice:InvoiceItem  = 1:M
    /// Participation:
    ///   - Invoice is TOTAL in InvoiceItem (every invoice must have at least one item)
    /// </summary>
    public class Invoice
    {
        // ── Key Attribute (PK) ──────────────────────────────────────
        public int Id { get; set; }

        // ── Simple Attributes ───────────────────────────────────────
        public DateTime Date { get; set; }

        // ── Foreign Key (Direct Relationship to Customer) ───────────
        // Cardinality: M Invoices → 1 Customer
        public int CustomerId { get; set; }

        // ── Navigation Properties ───────────────────────────────────

        // Direct relationship: Many Invoices belong to One Customer
        public Customer Customer { get; set; } = null!;

        // Direct relationship: One Invoice has Many InvoiceItems
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new HashSet<InvoiceItem>();
    }
}
