using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Commands;

public sealed record DeleteDepartmentCommand(int DepartmentId) : ICommand<bool>;

public sealed class DeleteDepartmentCommandHandler : ICommandHandler<DeleteDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _repository;

    public DeleteDepartmentCommandHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<bool> Handle(
        DeleteDepartmentCommand command, CancellationToken cancellationToken)
    {
        return await _repository.DeleteDepartmentAsync(
            command.DepartmentId, cancellationToken);
    }
}