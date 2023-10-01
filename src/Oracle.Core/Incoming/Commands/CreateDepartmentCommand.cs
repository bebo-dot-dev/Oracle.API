using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Commands;

public sealed record CreateDepartmentCommand : CreateDepartmentRequest, ICommand<Department?>;

public sealed class CreateDepartmentCommandHandler : ICommandHandler<CreateDepartmentCommand, Department?>
{
    private readonly IDepartmentRepository _repository;

    public CreateDepartmentCommandHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<Department?> Handle(
        CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var departmentData = new DepartmentData { Name = command.Name! };
        departmentData = await _repository.InsertDepartmentAsync(departmentData, cancellationToken);
        return departmentData.ToDepartment();
    }
}