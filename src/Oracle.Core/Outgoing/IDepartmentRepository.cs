namespace Oracle.Core.Outgoing;

public interface IDepartmentRepository
{
    Task<List<DepartmentData>> GetDepartmentsAsync(
        CancellationToken cancellationToken);
    
    Task<DepartmentData?> GetDepartmentAsync(
        int departmentId, CancellationToken cancellationToken);

    Task<DepartmentData> InsertDepartmentAsync(
        DepartmentData department, CancellationToken cancellationToken);
    
    Task<bool> DeleteDepartmentAsync(
        int departmentId, CancellationToken cancellationToken);
}