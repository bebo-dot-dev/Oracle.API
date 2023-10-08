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

public class GetEmployeesTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task GetEmployees_WhenNoEmployeesExists_AssertNoEmployees()
    {
        //act
        var responseMessage = await Client.GetAsync("v1/employees");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await responseMessage.Content.ReadFromJsonAsync<EmployeesResponse>();
        response!.Employees.Count.Should().Be(0);
    }

    [Test]
    public async Task GetEmployees_WhenEmployeesExists_AssertEmployees()
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
        var responseMessage = await Client.GetAsync("v1/employees");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await responseMessage.Content.ReadFromJsonAsync<EmployeesResponse>();
        response!.Employees.Count.Should().Be(1);
        response.Employees.Single().EmployeeId.Should().Be(employee.EmployeeId);
        response.Employees.Single().UserName.Should().Be(employee.UserName);
    }
}