using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Persistence
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options) { }

        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
        public DbSet<PropertyTrace> PropertyTraces => Set<PropertyTrace>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Owner
            modelBuilder.Entity<Owner>(b =>
            {
                b.ToTable("Owner");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnName("IdOwner");
                b.Property(x => x.Name).HasMaxLength(150).IsRequired();
                b.Property(x => x.Email).HasMaxLength(150).IsRequired();
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.Address).HasMaxLength(250);
            });

            // Property
            modelBuilder.Entity<Property>(b =>
            {
                b.ToTable("Property");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnName("IdProperty");
                b.Property(x => x.Name).HasMaxLength(150).IsRequired();
                b.Property(x => x.Address).HasMaxLength(250).IsRequired();
                b.Property(x => x.Price).HasColumnType("decimal(18,2)");
                b.Property(x => x.CodeInternal).HasMaxLength(50).IsRequired();
                b.HasIndex(p => p.CodeInternal).IsUnique();

                b.Property(p => p.OwnerId).HasColumnName("IdOwner");
                b.HasOne(p => p.Owner)
                 .WithMany(o => o.Properties)
                 .HasForeignKey(p => p.OwnerId)
                 .HasConstraintName("FK_Property_Owner");
            });

            // PropertyImage
            modelBuilder.Entity<PropertyImage>(b =>
            {
                b.ToTable("PropertyImage");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnName("IdPropertyImage");
                b.Property(x => x.PropertyId).HasColumnName("IdProperty");
                b.HasOne(pi => pi.Property)
                 .WithMany(p => p.Images)
                 .HasForeignKey(pi => pi.PropertyId)
                 .HasConstraintName("FK_PropertyImage_Property");
                b.Property(x => x.File).IsRequired();
                b.Property(x => x.Enabled).HasDefaultValue(true);
                b.HasIndex(pi => new { pi.PropertyId, pi.File }).IsUnique();
            });

            // PropertyTrace
            modelBuilder.Entity<PropertyTrace>(b =>
            {
                b.ToTable("PropertyTrace");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnName("IdPropertyTrace");
                b.Property(x => x.PropertyId).HasColumnName("IdProperty");
                b.HasOne(t => t.Property)
                 .WithMany(p => p.Traces)
                 .HasForeignKey(t => t.PropertyId)
                 .HasConstraintName("FK_PropertyTrace_Property");
                b.Property(t => t.Value).HasColumnType("decimal(18,2)");
                b.Property(t => t.Tax).HasColumnType("decimal(18,2)");
            });
        }
    }
}
