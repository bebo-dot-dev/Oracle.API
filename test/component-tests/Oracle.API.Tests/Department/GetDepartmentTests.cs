using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Outgoing;

namespace Oracle.API.Tests.Department;

public class GetDepartmentTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task GetDepartment_WhenNoDepartmentExists_AssertNotFoundStatusCode()
    {
        //act
        var responseMessage = await Client.GetAsync("v1/department/1");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetDepartment_WhenDepartmentExists_AssertDepartment()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData
            {
                Name = Guid.NewGuid().ToString()
            }, _cts.Token);

        //act
        var responseMessage = await Client.GetAsync($"v1/department/{department.DepartmentId}");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await responseMessage.Content.ReadFromJsonAsync<Core.Department>();
        response.Should().NotBeNull();
        response!.DepartmentId.Should().Be(department.DepartmentId);
        response.Name.Should().Be(department.Name);
    }
}