using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Oracle.Core.Incoming.Commands;
using Oracle.Core.Outgoing;

namespace Oracle.Core.Tests;

public class MapperExtensionTests
{
    [Test]
    public void ToDepartments_WhenGivenListOfDepartmentData_AssertMapsCorrectly()
    {
        var input = new List<DepartmentData>
        {
            new()
            {
                DepartmentId = 1,
                Name = "test department name"
            }
        };

        var act = input.ToDepartments();

        act.Should().BeEquivalentTo(
            new List<Department>
            {
                new()
                {
                    DepartmentId = 1,
                    Name = "test department name"
                }
            });
    }
    
    [Test]
    public void ToDepartment_WhenGivenDepartmentData_AssertMapsCorrectly()
    {
        var input = new DepartmentData
        {
            DepartmentId = 1,
            Name = "test department name"
        };

        var act = input.ToDepartment();

        act.Should().BeEquivalentTo(
            new Department
            {
                DepartmentId = 1,
                Name = "test department name"
            });
    }
    
    [Test]
    public void ToDepartment_WhenGivenNullDepartmentData_AssertMapsNullCorrectly()
    {
        var act = ((DepartmentData)null).ToDepartment();
        act.Should().BeNull();
    }

    [Test]
    public void ToCreateDepartmentCommand_WhenGivenCreateDepartmentRequest_AssertMapsCorrectly()
    {
        var input = new CreateDepartmentRequest
        {
            Name = "test department name"
        };
        var act = input.ToCreateDepartmentCommand();
        act.Should().BeEquivalentTo(
            new CreateDepartmentCommand
            {
                Name = "test department name"
            });
    }

    [Test]
    public void ToEmployees_WhenGivenListOfEmployeeData_AssertMapsCorrectly()
    {
        var input = new List<EmployeeData>
        {
            new()
            {
                EmployeeId = 1,
                UserName = "test employee username",
                Department = new DepartmentData
                {
                    DepartmentId = 1,
                    Name = "test department name"
                }
            }
        };

        var act = input.ToEmployees();

        act.Should().BeEquivalentTo(
            new List<Employee>
            {
                new()
                {
                    EmployeeId = 1,
                    UserName = "test employee username",
                    Department = new Department
                    {
                        DepartmentId = 1,
                        Name = "test department name"
                    }
                }
            });
    }
    
    [Test]
    public void ToEmployee_WhenGivenEmployeeData_AssertMapsCorrectly()
    {
        var input = new EmployeeData
        {
            EmployeeId = 1,
            UserName = "test employee username",
            Department = new DepartmentData
            {
                DepartmentId = 1,
                Name = "test department name"
            }
        };

        var act = input.ToEmployee();

        act.Should().BeEquivalentTo(
            new Employee
            {
                EmployeeId = 1,
                UserName = "test employee username",
                Department = new Department
                {
                    DepartmentId = 1,
                    Name = "test department name"
                }
            });
    }
    
    [Test]
    public void ToEmployee_WhenGivenNullEmployeeData_AssertMapsNullCorrectly()
    {
        var act = ((EmployeeData)null).ToEmployee();
        act.Should().BeNull();
    }

    [Test]
    public void ToCreateEmployeeCommand_WhenGivenCreateEmployeeRequest_AssertMapsCorrectly()
    {
        var input = new CreateEmployeeRequest
        {
            UserName = "test employee username",
            DepartmentId = 1
        };
        var act = input.ToCreateEmployeeCommand();
        act.Should().BeEquivalentTo(
            new CreateEmployeeCommand
            {
                UserName = "test employee username",
                DepartmentId = 1
            });
    }
}