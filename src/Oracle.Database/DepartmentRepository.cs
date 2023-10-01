using Microsoft.EntityFrameworkCore;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

internal class DepartmentRepository : IDepartmentRepository
{
    private readonly OracleDbContext _context;

    public DepartmentRepository(OracleDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<DepartmentData>> GetDepartmentsAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Include(d => d.Users)
            .ToListAsync(cancellationToken);
    }

    public async Task<DepartmentData?> GetDepartmentAsync(
        int departmentId, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Where(d => d.DepartmentId == departmentId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<DepartmentData> InsertDepartmentAsync(
        DepartmentData department, CancellationToken cancellationToken)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);
        return department;
    }

    public async Task<bool> DeleteDepartmentAsync(
        int departmentId, CancellationToken cancellationToken)
    {
        var affectedRowCount = await _context.Departments
            .Where(d => d.DepartmentId == departmentId)
            .ExecuteDeleteAsync(cancellationToken);
        return affectedRowCount == 1;
    }
}