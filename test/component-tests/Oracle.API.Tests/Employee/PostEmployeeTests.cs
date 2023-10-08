using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core;
using Oracle.Core.Outgoing;

namespace Oracle.API.Tests.Employee;

public class PostEmployeeTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task PostEmployee_WhenEmployeePosted_AssertEmployeeCreated()
    {
        //arrange
        var employees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        employees.Count.Should().Be(0);
        
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData
            {
                Name = Guid.NewGuid().ToString()
            }, _cts.Token);
        
        //act
        var payload = new CreateEmployeeRequest
        {
            UserName = Guid.NewGuid().ToString(),
            Password = "testPassword",
            FirstName = "testFirstName",
            LastName = "testLastName",
            DepartmentId = department.DepartmentId
        };
        
        var responseMessage = await Client.PostAsJsonAsync("v1/employee", payload, _cts.Token);

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        var response = await responseMessage.Content.ReadFromJsonAsync<Core.Employee>();
        response.Should().NotBeNull();

        employees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        employees.Count.Should().Be(1);

        response!.EmployeeId.Should().Be(employees.Single().EmployeeId);
        response.UserName.Should().Be(employees.Single().UserName);
    }

    [Test]
    public async Task PostEmployee_WhenBadEmployeePosted_AssertBadRequest()
    {
        //arrange
        var employees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        employees.Count.Should().Be(0);

        //act
        var payload = new CreateEmployeeRequest();
        var responseMessage = await Client.PostAsJsonAsync("v1/employee", payload, _cts.Token);

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        employees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        employees.Count.Should().Be(0);
    }
}