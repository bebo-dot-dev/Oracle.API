namespace Oracle.Core;

public sealed record DepartmentsResponse
{
    public List<Department> Departments { get; init; } = new();
}