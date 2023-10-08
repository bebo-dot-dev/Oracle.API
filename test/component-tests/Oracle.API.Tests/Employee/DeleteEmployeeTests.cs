using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Outgoing;

namespace Oracle.API.Tests.Employee;

public class DeleteEmployeeTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task DeleteEmployee_WhenNoEmployeeExists_AssertNotFoundStatusCode()
    {
        //act
        var responseMessage = await Client.DeleteAsync("v1/employee/1");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task DeleteEmployee_WhenEmployeeDelete_AssertEmployeeDeleted()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData
            {
                Name = Guid.NewGuid().ToString()
            }, _cts.Token);
        
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = "test userName",
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            }, _cts.Token);
        
        var employees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        employees.Count.Should().Be(1);

        //act
        var responseMessage = await Client.DeleteAsync($"v1/employee/{employee.EmployeeId}", _cts.Token);

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);

        employees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        employees.Count.Should().Be(0);
    }
}