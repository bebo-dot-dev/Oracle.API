using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Queries;

public sealed record GetEmployeeQuery(int UserId) : IRequest<Employee?>;

public sealed class GetUserQueryHandler : IRequestHandler<GetEmployeeQuery, Employee?>
{
    private readonly IEmployeeRepository _repository;

    public GetUserQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<Employee?> Handle(
        GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employeeData = await _repository.GetEmployeeAsync(request.UserId, cancellationToken);
        return employeeData.ToEmployee();
    }
}