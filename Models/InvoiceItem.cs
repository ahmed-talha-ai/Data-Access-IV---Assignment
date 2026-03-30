namespace InvoiceSystem.Models
{
    /// <summary>
    /// InvoiceItem Entity — Junction / Bridge Entity
    /// ERD: Resolves the N:M relationship between Product and Invoice.
    ///      This is an INDIRECT relationship entity.
    /// Key Attribute: Id (PK)
    /// Descriptive/Relationship Attributes (from ERD diamond): Qty, UnitPrice
    /// Relationships:
    ///   - M-to-1 with Invoice  (Many InvoiceItems belong to One Invoice)
    ///   - M-to-1 with Product  (Many InvoiceItems reference One Product)
    /// Cardinality Ratio:
    ///   - Product:Invoice = N:M (resolved here)
    /// Degree: Binary (between Product and Invoice via this junction)
    /// Participation:
    ///   - TOTAL participation on both sides (every InvoiceItem must have a valid Invoice AND Product)
    /// </summary>
    public class InvoiceItem
    {
        // ── Key Attribute (PK) ──────────────────────────────────────
        public int Id { get; set; }

        // ── Relationship/Descriptive Attributes (from ERD diamond) ──
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }

        // ── Foreign Keys ────────────────────────────────────────────
        // M InvoiceItems → 1 Invoice
        public int InvoiceId { get; set; }

        // M InvoiceItems → 1 Product
        public int ProductId { get; set; }

        // ── Navigation Properties (Direct) ──────────────────────────
        public Invoice Invoice { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
