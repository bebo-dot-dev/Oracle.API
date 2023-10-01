namespace Oracle.Core.Outgoing;

public class UserData
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
    
    public int DepartmentId { get; set; }

    public DepartmentData Department { get; set; } = null!;
}