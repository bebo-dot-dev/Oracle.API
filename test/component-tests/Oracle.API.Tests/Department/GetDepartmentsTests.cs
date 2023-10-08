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

namespace Oracle.API.Tests.Department;

public class GetDepartmentsTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task GetDepartments_WhenNoDepartmentsExists_AssertNoDepartments()
    {
        //act
        var responseMessage = await Client.GetAsync("v1/departments");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await responseMessage.Content.ReadFromJsonAsync<DepartmentsResponse>();
        response!.Departments.Count.Should().Be(0);
    }

    [Test]
    public async Task GetDepartments_WhenDepartmentsExists_AssertDepartments()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData
            {
                Name = Guid.NewGuid().ToString()
            }, _cts.Token);

        //act
        var responseMessage = await Client.GetAsync("v1/departments");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await responseMessage.Content.ReadFromJsonAsync<DepartmentsResponse>();
        response!.Departments.Count.Should().Be(1);
        response.Departments.Single().DepartmentId.Should().Be(department.DepartmentId);
        response.Departments.Single().Name.Should().Be(department.Name);
    }
}