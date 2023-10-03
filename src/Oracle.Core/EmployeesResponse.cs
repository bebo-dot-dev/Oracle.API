namespace Oracle.Core;

public sealed record EmployeesResponse
{
    public List<Employee> Employees { get; init; } = new();
}