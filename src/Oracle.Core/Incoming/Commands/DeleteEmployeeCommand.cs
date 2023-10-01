using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Commands;

public sealed record DeleteEmployeeCommand(int EmployeeId) : ICommand<bool>;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _repository;

    public DeleteUserCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<bool> Handle(
        DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        return await _repository.DeleteEmployeeAsync(
            command.EmployeeId, cancellationToken);
    }
}