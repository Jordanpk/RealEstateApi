using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;

namespace RealEstate.Domain.Entities
{
    public class Owner
    {
        public int Id { get; set; }            // IdOwner en DB (mapeo en DbContext)
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Photo { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
