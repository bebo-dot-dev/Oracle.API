using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

public class UserDataConfiguration : IEntityTypeConfiguration<UserData>
{
    public void Configure(EntityTypeBuilder<UserData> builder)
    {
        builder.Property(u => u.UserId).HasColumnName("userId");
        builder.Property(u => u.UserName).HasColumnName("userName");
        builder.Property(u => u.Password).HasColumnName("password");
        builder.Property(u => u.FirstName).HasColumnName("firstName");
        builder.Property(u => u.LastName).HasColumnName("lastName");
        builder.Property(u => u.DepartmentId).HasColumnName("departmentId");
        
        builder.HasKey(x => x.UserId);
        builder.HasIndex(x => x.DepartmentId, name: "ix_user_departmentId");

        builder
            .HasOne(u => u.Department)
            .WithMany(x =>x.Users)
            .HasForeignKey(k => k.DepartmentId);
        
        builder.ToTable("user");
    }
}