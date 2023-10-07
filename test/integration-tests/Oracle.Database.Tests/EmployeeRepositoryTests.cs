using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Outgoing;

namespace Oracle.Database.Tests;

public class EmployeeRepositoryTests : TestBase
{
    private CancellationTokenSource _cts;
    private EmployeeRepository _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new EmployeeRepository(SetupFixture.DbContext);
        _cts = new CancellationTokenSource();
    }

    [Test]
    public async Task InsertEmployeeAsync_WhenEmployeeInserted_AssertEmployeeInDatabase()
    {
        //arrange
        var userName = Guid.NewGuid().ToString();
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData
            {
                Name = Guid.NewGuid().ToString()
            },
            _cts.Token);

        //act
        var act = await _sut.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = userName,
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);

        //assert
        var allEmployees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        allEmployees.Count.Should().Be(1);
        allEmployees.Single().EmployeeId.Should().Be(act.EmployeeId);
        allEmployees.Single().UserName.Should().Be(userName);
    }

    [Test]
    public async Task DeleteEmployeeAsync_WhenEmployeeDeleted_AssertEmployeeNotInDatabase()
    {
        //arrange
        var userName = Guid.NewGuid().ToString();
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData { Name = Guid.NewGuid().ToString() },
            _cts.Token);
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = userName,
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);
        var allEmployees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        allEmployees.Count.Should().Be(1);
        allEmployees.Single().UserName.Should().Be(userName);

        //act
        var act = await _sut.DeleteEmployeeAsync(
            employee.EmployeeId,
            _cts.Token);

        //assert
        act.Should().BeTrue();
        allEmployees = await DatabaseHelper.GetEmployeesAsync(_cts.Token);
        allEmployees.Count.Should().Be(0);
    }

    [Test]
    public async Task GetEmployeesAsync_WhenEmployeeInserted_AssertOneEmployeeReturnedWithDepartment()
    {
        //arrange
        var departmentName = Guid.NewGuid().ToString();
        var userName = Guid.NewGuid().ToString();
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData { Name = departmentName },
            _cts.Token);
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = userName,
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);

        //act
        var act = await _sut.GetEmployeesAsync(_cts.Token);

        //assert
        act.Count.Should().Be(1);
        act.Single().EmployeeId.Should().Be(employee.EmployeeId);
        act.Single().UserName.Should().Be(userName);
        act.Single().Department.Should().NotBeNull();
        act.Single().Department.Name.Should().Be(departmentName);
    }

    [Test]
    public async Task GetEmployeeAsync_WhenEmployee_AssertEmployeeReturnedWithDepartment()
    {
        //arrange
        var departmentName = Guid.NewGuid().ToString();
        var userName = Guid.NewGuid().ToString();
        var department = await DatabaseHelper.InsertDepartmentAsync(
            new DepartmentData { Name = departmentName },
            _cts.Token);
        var employee = await DatabaseHelper.InsertEmployeeAsync(
            new EmployeeData
            {
                UserName = userName,
                Password = "testPassword",
                FirstName = "testFirstName",
                LastName = "testLastName",
                DepartmentId = department.DepartmentId
            },
            _cts.Token);

        //act
        var act = await _sut.GetEmployeeAsync(employee.EmployeeId, _cts.Token);

        //assert
        act.Should().NotBeNull();
        act!.EmployeeId.Should().Be(employee.EmployeeId);
        act.UserName.Should().Be(userName);
        act.Department.Name.Should().Be(departmentName);
    }

    [Test]
    public async Task GetEmployeeAsync_WhenEmployeeDoesNotExistForEmployeeId_AssertNullEmployeeReturned()
    {
        //act
        var act = await _sut.GetEmployeeAsync(-1, _cts.Token);

        //assert
        act.Should().BeNull();
    }
}