using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

internal sealed class EmployeeDataConfiguration : IEntityTypeConfiguration<EmployeeData>
{
    public void Configure(EntityTypeBuilder<EmployeeData> builder)
    {
        builder.Property(u => u.EmployeeId).HasColumnName("employeeId");
        builder.Property(u => u.UserName).HasColumnName("userName");
        builder.Property(u => u.Password).HasColumnName("password");
        builder.Property(u => u.FirstName).HasColumnName("firstName");
        builder.Property(u => u.LastName).HasColumnName("lastName");
        builder.Property(u => u.DepartmentId).HasColumnName("departmentId");
        
        builder.HasKey(x => x.EmployeeId);
        builder.HasIndex(x => x.DepartmentId, name: "IX_employee_departmentId");

        builder
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
        
        builder.ToTable("employee");
    }
}