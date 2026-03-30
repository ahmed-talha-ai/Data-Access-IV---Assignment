namespace InvoiceSystem.Models
{
    /// <summary>
    /// Car Entity
    /// ERD: Strong Entity | Key Attribute: Id | Simple Attributes: Color, MaxSpeed
    /// Relationship: 1-to-1 with Customer (One Customer OWNs One Car)
    /// Cardinality Ratio: 1:1
    /// Participation: TOTAL (every Car must be owned by a Customer — FK is required)
    /// Degree: Binary Relationship
    /// </summary>
    public class Car
    {
        // ── Key Attribute (PK) ──────────────────────────────────────
        public int Id { get; set; }

        // ── Simple Attributes ───────────────────────────────────────
        public string Color { get; set; } = string.Empty;
        public decimal MaxSpeed { get; set; }

        // ── Foreign Key (Direct 1:1 Relationship to Customer) ───────
        // CustomerId is on the Car side — Car is the dependent entity
        public int CustomerId { get; set; }

        // ── Navigation Property (Direct) ────────────────────────────
        public Customer Customer { get; set; } = null!;
    }
}
