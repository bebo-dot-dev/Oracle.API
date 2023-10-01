namespace Oracle.Core;

public class Employee
{
    public int EmployeeId { get; init; }

    public string UserName { get; init; } = null!;

    public string Password { get; init; } = null!;

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;
    
    public int DepartmentId { get; init; }

    public Department Department { get; init; } = null!;
}