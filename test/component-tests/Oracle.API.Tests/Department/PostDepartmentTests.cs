using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core;

namespace Oracle.API.Tests.Department;

public class PostDepartmentTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task PostDepartment_WhenDepartmentPosted_AssertDepartmentCreated()
    {
        //arrange
        var departments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        departments.Count.Should().Be(0);

        //act
        var payload = new CreateDepartmentRequest
        {
            Name = Guid.NewGuid().ToString()
        };
        var responseMessage = await Client.PostAsJsonAsync("v1/department", payload, _cts.Token);

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        var response = await responseMessage.Content.ReadFromJsonAsync<Core.Department>();
        response.Should().NotBeNull();

        departments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        departments.Count.Should().Be(1);

        response!.DepartmentId.Should().Be(departments.Single().DepartmentId);
        response.Name.Should().Be(departments.Single().Name);
    }

    [Test]
    public async Task PostDepartment_WhenBadDepartmentPosted_AssertBadRequest()
    {
        //arrange
        var departments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        departments.Count.Should().Be(0);

        //act
        var payload = new CreateDepartmentRequest();
        var responseMessage = await Client.PostAsJsonAsync("v1/department", payload, _cts.Token);

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        departments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        departments.Count.Should().Be(0);
    }
}