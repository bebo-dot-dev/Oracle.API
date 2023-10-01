namespace Oracle.Core.Outgoing;

public class DepartmentData
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public List<EmployeeData> Employees { get; set; } = new();
}