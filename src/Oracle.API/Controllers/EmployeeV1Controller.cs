using Mediator;
using Microsoft.AspNetCore.Mvc;
using Oracle.Core;
using Oracle.Core.Incoming.Commands;
using Oracle.Core.Incoming.Queries;

namespace Oracle.API.Controllers;

[ApiController]
[Route("/v1/")]
public class EmployeeV1Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeV1Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all employees.
    /// </summary>
    /// <returns><see cref="EmployeesResponse"/></returns>
    [HttpGet("employees")]
    [ProducesResponseType(typeof(EmployeesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployees()
    {
        var request = new GetEmployeesQuery();
        return Ok(await _mediator.Send(request));
    }
    
    /// <summary>
    /// Returns the employee matching the supplied employeeId.
    /// </summary>
    /// <param name="employeeId">The employeeId of the employee to return.</param>
    /// <returns><see cref="Core.Employee"/></returns>
    [HttpGet("employee/{employeeId:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Employee([FromRoute]int employeeId)
    {
        var request = new GetEmployeeQuery(employeeId);
        var employee = await _mediator.Send(request);
        return employee is not null ? Ok(employee) : NotFound();
    }

    /// <summary>
    /// Inserts a new employee.
    /// </summary>
    /// <param name="request">The request containing the details of the employee to create.</param>
    /// <returns><see cref="Core.Employee"/></returns>
    [HttpPost("employee")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEmployee([FromBody]CreateEmployeeRequest request)
    {
        var command = request.ToCreateEmployeeCommand();
        var employee = await _mediator.Send(command);
        return CreatedAtAction(
            nameof(Employee), 
            new{employeeId = employee?.EmployeeId}, 
            employee);
    }
    
    /// <summary>
    /// Deletes the employee matching the supplied employeeId.
    /// </summary>
    /// <param name="employeeId">The employeeId of the employee to delete.</param>
    /// <returns><see cref="Core.Employee"/></returns>
    [HttpDelete("employee/{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee([FromRoute]int employeeId)
    {
        var request = new DeleteEmployeeCommand(employeeId);
        var deleted = await _mediator.Send(request);
        return deleted ? NoContent() : NotFound();
    }
}