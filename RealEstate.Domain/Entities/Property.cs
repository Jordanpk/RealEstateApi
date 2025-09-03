using System;
using System.Collections.Generic;

namespace RealEstate.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }           // IdProperty en DB
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = null!;
        public int? Year { get; set; }

        public int OwnerId { get; set; }      // FK -> (IdOwner)
        public Owner? Owner { get; set; }

        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public ICollection<PropertyTrace> Traces { get; set; } = new List<PropertyTrace>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0) throw new ArgumentException("El nuevo precio debe ser positivo");
            Price = newPrice;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
