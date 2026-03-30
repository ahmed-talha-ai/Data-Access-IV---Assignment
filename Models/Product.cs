namespace InvoiceSystem.Models
{
    /// <summary>
    /// Product Entity
    /// ERD: Strong Entity | Key Attribute: Id | Simple Attributes: Name, Price
    /// Relationships:
    ///   - M-to-1 with Category  (Many Products belong to One Category)
    ///   - N-to-M with Invoice   (Many Products appear in Many Invoices)
    ///     → Resolved via InvoiceItem junction entity (INDIRECT relationship)
    ///       with additional attributes: Qty, UnitPrice
    /// Cardinality Ratio: N (Product) : M (Invoice)
    /// Participation: Partial on both sides (not all products must be invoiced)
    /// </summary>
    public class Product
    {
        // ── Key Attribute (PK) ──────────────────────────────────────
        public int Id { get; set; }

        // ── Simple Attributes ───────────────────────────────────────
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // ── Foreign Key (Direct Relationship to Category) ───────────
        // Cardinality Ratio: M (Products) : 1 (Category)
        public int CategoryId { get; set; }

        // ── Navigation Properties ───────────────────────────────────
        // Direct relationship to parent Category
        public Category Category { get; set; } = null!;

        // Indirect relationship to Invoice via junction entity InvoiceItem
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new HashSet<InvoiceItem>();
    }
}
