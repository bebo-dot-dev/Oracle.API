namespace Oracle.Core;

public sealed record EmployeesResponse
{
    public List<Employee> Users { get; init; } = new();
}