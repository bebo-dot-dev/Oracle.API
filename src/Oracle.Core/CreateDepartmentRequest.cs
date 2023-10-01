using System.ComponentModel.DataAnnotations;

namespace Oracle.Core;

public record CreateDepartmentRequest
{
    [Required]
    public string? Name { get; init; }
}