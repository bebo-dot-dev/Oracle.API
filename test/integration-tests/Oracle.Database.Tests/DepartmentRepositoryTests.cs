using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Outgoing;

namespace Oracle.Database.Tests;

public class DepartmentRepositoryTests : TestBase
{
    private CancellationTokenSource _cts;
    private DepartmentRepository _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new DepartmentRepository(SetupFixture.DbContext);
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task InsertDepartmentAsync_WhenDepartmentInserted_AssertDepartmentInDatabase()
    {
        //arrange
        var departmentName = Guid.NewGuid().ToString();

        //act
        var act = await _sut.InsertDepartmentAsync(
            new DepartmentData { Name = departmentName },
            _cts.Token);

        //assert
        var allDepartments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        allDepartments.Count.Should().Be(1);
        allDepartments.Single().DepartmentId.Should().Be(act.DepartmentId);
        allDepartments.Single().Name.Should().Be(departmentName);
    }

    [Test]
    public async Task DeleteDepartmentAsync_WhenDepartmentDeleted_AssertDepartmentNotInDatabaseAndEmployeeDeleteCascades()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData { Name = Guid.NewGuid().ToString() },
            _cts.Token);
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = Guid.NewGuid().ToString(),
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);
        var allDepartments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        allDepartments.Count.Should().Be(1);
        allDepartments.Single().DepartmentId.Should().Be(department.DepartmentId);
        allDepartments.Single().Employees.Count.Should().Be(1);
        allDepartments.Single().Employees.Single().UserName.Should().Be(employee.UserName);

        //act
        var act = await _sut.DeleteDepartmentAsync(
            department.DepartmentId,
            _cts.Token);

        //assert
        act.Should().BeTrue();
        allDepartments = await DatabaseHelper.GetDepartmentsAsync(_cts.Token);
        allDepartments.Count.Should().Be(0);
        var allEmployees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        allEmployees.Count.Should().Be(0);
    }

    [Test]
    public async Task GetDepartmentsAsync_WhenDepartmentInsertedWithEmployee_AssertOneDepartmentReturnedWithEmployee()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData { Name = Guid.NewGuid().ToString() },
            _cts.Token);
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = Guid.NewGuid().ToString(),
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);

        //act
        var act = await _sut.GetDepartmentsAsync(_cts.Token);

        //assert
        act.Count.Should().Be(1);
        act.Single().DepartmentId.Should().Be(department.DepartmentId);
        act.Single().Name.Should().Be(department.Name);
        act.Single().Employees.Count.Should().Be(1);
        act.Single().Employees.Single().EmployeeId.Should().Be(employee.EmployeeId);
        act.Single().Employees.Single().UserName.Should().Be(employee.UserName);
    }

    [Test]
    public async Task GetDepartmentAsync_WhenDepartmentInsertedWithEmployee_AssertDepartmentReturnedWithEmployee()
    {
        //arrange
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData { Name = Guid.NewGuid().ToString() },
            _cts.Token);
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = Guid.NewGuid().ToString(),
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);

        //act
        var act = await _sut.GetDepartmentAsync(department.DepartmentId, _cts.Token);

        //assert
        act.Should().NotBeNull();
        act!.DepartmentId.Should().Be(department.DepartmentId);
        act.Name.Should().Be(department.Name);
        act.Employees.Count.Should().Be(1);
        act.Employees.Single().EmployeeId.Should().Be(employee.EmployeeId);
        act.Employees.Single().UserName.Should().Be(employee.UserName);
    }

    [Test]
    public async Task GetDepartmentAsync_WhenDepartmentDoesNotExistForDepartmentId_AssertNullDepartmentReturned()
    {
        //act
        var act = await _sut.GetDepartmentAsync(-1, _cts.Token);

        //assert
        act.Should().BeNull();
    }
}