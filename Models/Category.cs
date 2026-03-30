namespace InvoiceSystem.Models
{
    /// <summary>
    /// Category Entity
    /// ERD: Strong Entity | Key Attribute: Id | Simple Attributes: Name, IsActive
    /// Relationship: 1-to-M with Product (One Category HAS Many Products)
    /// Participation: Total participation of Product in Category (every product must belong to a category)
    /// </summary>
    public class Category
    {
        // ── Key Attribute (PK) ──────────────────────────────────────
        public int Id { get; set; }

        // ── Simple Attributes ───────────────────────────────────────
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // ── Navigation Property (Indirect - Collection) ─────────────
        // Cardinality Ratio: 1 (Category) : M (Products)
        // Degree: Binary Relationship
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
