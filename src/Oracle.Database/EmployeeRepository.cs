using Microsoft.EntityFrameworkCore;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

internal sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly OracleDbContext _context;

    public EmployeeRepository(OracleDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<EmployeeData>> GetEmployeesAsync(CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Include(d => d.Department)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeData?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Where(d => d.EmployeeId == employeeId)
            .Include(u => u.Department)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<EmployeeData> InsertEmployeeAsync(EmployeeData employee, CancellationToken cancellationToken)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task<bool> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken)
    {
        var affectedRowCount = await _context.Employees
            .Where(d => d.EmployeeId == employeeId)
            .ExecuteDeleteAsync(cancellationToken);
        return affectedRowCount == 1;
    }
}