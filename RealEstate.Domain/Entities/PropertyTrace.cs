using System;

namespace RealEstate.Domain.Entities
{
    public class PropertyTrace
    {
        public int Id { get; set; }            // IdPropertyTrace en DB
        public int PropertyId { get; set; }
        public Property? Property { get; set; }

        public DateTime DateSale { get; set; }
        public string Name { get; set; } = null!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
