using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Outgoing;

namespace Oracle.API.Tests.Employee;

public class GetEmployeeTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task GetEmployee_WhenNoEmployeeExists_AssertNotFoundStatusCode()
    {
        //act
        var responseMessage = await Client.GetAsync("v1/employee/1");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetEmployee_WhenEmployeeExists_AssertEmployee()
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
                UserName = Guid.NewGuid().ToString(),
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            }, _cts.Token);

        //act
        var responseMessage = await Client.GetAsync($"v1/employee/{employee.EmployeeId}");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await responseMessage.Content.ReadFromJsonAsync<Core.Employee>();
        response.Should().NotBeNull();
        response!.EmployeeId.Should().Be(employee.EmployeeId);
        response.UserName.Should().Be(employee.UserName);
    }
}