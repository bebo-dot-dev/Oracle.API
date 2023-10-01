using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

internal sealed class DepartmentDataConfiguration : IEntityTypeConfiguration<DepartmentData>
{
    public void Configure(EntityTypeBuilder<DepartmentData> builder)
    {
        builder.Property(d => d.DepartmentId).HasColumnName("departmentId");
        builder.Property(d => d.Name).HasColumnName("name");
        
        builder.HasKey(d => d.DepartmentId);

        builder
            .HasMany(d => d.Users)
            .WithOne(u => u.Department);
        
        builder.ToTable("department");
    }
}