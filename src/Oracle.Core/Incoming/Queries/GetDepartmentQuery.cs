using Mediator;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Incoming.Queries;

public sealed record GetDepartmentQuery(int DepartmentId) : IRequest<Department?>;

public sealed class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, Department?>
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentQueryHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<Department?> Handle(
        GetDepartmentQuery request, CancellationToken cancellationToken)
    {
        var departmentData = await _repository.GetDepartmentAsync(request.DepartmentId, cancellationToken);
        return departmentData.ToDepartment();
    }
}