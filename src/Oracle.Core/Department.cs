namespace Oracle.Core;

public record Department
{
    public int DepartmentId { get; init; }

    public string Name { get; init; } = null!;
}