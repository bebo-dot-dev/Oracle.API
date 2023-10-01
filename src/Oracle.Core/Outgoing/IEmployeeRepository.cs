namespace Oracle.Core.Outgoing;

public interface IEmployeeRepository
{
    Task<List<EmployeeData>> GetEmployeesAsync(
        CancellationToken cancellationToken);
    
    Task<EmployeeData?> GetEmployeeAsync(
        int employeeId, CancellationToken cancellationToken);

    Task<EmployeeData> InsertEmployeeAsync(
        EmployeeData employee, CancellationToken cancellationToken);
    
    Task<bool> DeleteEmployeeAsync(
        int employeeId, CancellationToken cancellationToken);
}