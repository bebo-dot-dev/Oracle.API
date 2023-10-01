using System.ComponentModel.DataAnnotations;

namespace Oracle.Core;

public record CreateEmployeeRequest
{
    [Required]
    public string? UserName { get; init; }

    [Required]
    public string? Password { get; init; }

    [Required]
    public string? FirstName { get; init; }

    [Required]
    public string? LastName { get; init; }
    
    [Required]
    public int? DepartmentId { get; init; }
}