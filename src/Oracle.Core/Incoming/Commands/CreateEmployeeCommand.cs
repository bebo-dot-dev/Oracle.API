using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Commands;

public sealed record CreateEmployeeCommand : CreateEmployeeRequest, ICommand<Employee?>;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateEmployeeCommand, Employee?>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public CreateUserCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }
    
    public async ValueTask<Employee?> Handle(
        CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeData = new EmployeeData
        {
            UserName = command.UserName!,
            Password = command.Password!,
            FirstName = command.FirstName!,
            LastName = command.LastName!,
            DepartmentId = command.DepartmentId!.Value
        };
        employeeData = await _employeeRepository.InsertEmployeeAsync(employeeData, cancellationToken);
        var departmentData = await _departmentRepository.GetDepartmentAsync(
            command.DepartmentId.Value, cancellationToken);
        employeeData.Department = departmentData!;
        return employeeData.ToEmployee();
    }
}