using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Outgoing;

namespace Oracle.API.Tests.Department;

public class DeleteDepartmentTests : TestBase
{
    private CancellationTokenSource _cts;

    [SetUp]
    public void TestSetUp()
    {
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task DeleteDepartment_WhenNoDepartmentExists_AssertNotFoundStatusCode()
    {
        //act
        var responseMessage = await Client.DeleteAsync("v1/department/1");

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task DeleteDepartment_WhenDepartmentDelete_AssertDepartmentDeleted()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData
            {
                Name = Guid.NewGuid().ToString()
            }, _cts.Token);
        var departments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        departments.Count.Should().Be(1);

        //act
        var responseMessage = await Client.DeleteAsync($"v1/department/{department.DepartmentId}", _cts.Token);

        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);

        departments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        departments.Count.Should().Be(0);
    }
}