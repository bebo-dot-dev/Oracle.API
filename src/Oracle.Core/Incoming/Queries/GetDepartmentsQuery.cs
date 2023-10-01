using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Queries;

public sealed record GetDepartmentsQuery : IRequest<DepartmentsResponse>;

public sealed class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, DepartmentsResponse>
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentsQueryHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<DepartmentsResponse> Handle(
        GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departmentsData = await _repository.GetDepartmentsAsync(cancellationToken);
        return new DepartmentsResponse
        {
            Departments = departmentsData.ToDepartments()
        };
    }
}