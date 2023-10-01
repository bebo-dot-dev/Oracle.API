using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Queries;

public sealed record GetEmployeesQuery : IRequest<EmployeesResponse>;

public sealed class GetUsersQueryHandler : IRequestHandler<GetEmployeesQuery, EmployeesResponse>
{
    private readonly IEmployeeRepository _repository;

    public GetUsersQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<EmployeesResponse> Handle(
        GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetEmployeesAsync(cancellationToken);
        return new EmployeesResponse
        {
            Users = data.ToEmployees()
        };
    }
}