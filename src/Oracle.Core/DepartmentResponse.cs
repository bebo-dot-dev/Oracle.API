namespace Oracle.Core;

public sealed record DepartmentResponse
{
    public int DepartmentId { get; init; }

    public string Name { get; init; } = null!;
}