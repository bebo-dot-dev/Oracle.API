using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.Core.Outgoing;

namespace Oracle.Database.Tests;

internal static class DatabaseHelper
{
    public static async Task<List<DepartmentData>> GetDepartmentsAsync(CancellationToken cancellationToken)
    {
        return await SetupFixture.DbContext.Departments
            .Include(d => d.Employees)
            .ToListAsync(cancellationToken);
    }

    public static async Task<DepartmentData> InsertDepartmentAsync(
        DepartmentData department, CancellationToken cancellationToken)
    {
        SetupFixture.DbContext.Departments.Add(department);
        await SetupFixture.DbContext.SaveChangesAsync(cancellationToken);
        return department;
    }

    public static async Task<EmployeeData> InsertEmployeeAsync(
        EmployeeData employee, CancellationToken cancellationToken)
    {
        SetupFixture.DbContext.Employees.Add(employee);
        await SetupFixture.DbContext.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public static async Task<List<EmployeeData>> GetEmployeesAsync(CancellationToken cancellationToken)
    {
        return await SetupFixture.DbContext.Employees
            .Include(d => d.Department)
            .ToListAsync(cancellationToken);
    }
}