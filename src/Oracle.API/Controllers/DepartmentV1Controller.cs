using Mediator;
using Microsoft.AspNetCore.Mvc;
using Oracle.Core;
using Oracle.Core.Incoming.Commands;
using Oracle.Core.Incoming.Queries;

namespace Oracle.API.Controllers;

[ApiController]
[Route("/v1/")]
public class DepartmentV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all departments.
    /// </summary>
    /// <returns><see cref="DepartmentsResponse"/></returns>
    [HttpGet("departments")]
    [ProducesResponseType(typeof(DepartmentsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Departments()
    {
        var request = new GetDepartmentsQuery();
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Returns the department matching the supplied departmentId.
    /// </summary>
    /// <param name="departmentId">The departmentId of the department to return.</param>
    /// <returns><see cref="Department"/></returns>
    [HttpGet("department/{departmentId:int}")]
    [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Department([FromRoute]int departmentId)
    {
        var request = new GetDepartmentQuery(departmentId);
        var department = await _mediator.Send(request);
        return department is not null ? Ok(department) : NotFound();
    }

    /// <summary>
    /// Inserts a new department.
    /// </summary>
    /// <param name="request">The request containing the name of the department to create.</param>
    /// <returns><see cref="Department"/></returns>
    [HttpPost("department")]
    [ProducesResponseType(typeof(Department), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody]CreateDepartmentRequest request)
    {
        var command = request.ToCreateDepartmentCommand();
        var department = await _mediator.Send(command);
        return CreatedAtAction(
            nameof(Department), 
            new{departmentId = department?.DepartmentId}, 
            department);
    }
    
    /// <summary>
    /// Deletes the department matching the supplied departmentId.
    /// </summary>
    /// <param name="departmentId">The departmentId of the department to delete.</param>
    /// <returns><see cref="Department"/></returns>
    [HttpDelete("department/{departmentId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDepartment([FromRoute]int departmentId)
    {
        var request = new DeleteDepartmentCommand(departmentId);
        var deleted = await _mediator.Send(request);
        return deleted ? NoContent() : NotFound();
    }
}