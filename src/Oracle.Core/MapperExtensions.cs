using Oracle.Core.Incoming.Commands;
using Oracle.Core.Outgoing;
using Riok.Mapperly.Abstractions;

namespace Oracle.Core;

[Mapper]
public static partial class MapperExtensions
{
    public static partial List<Department> ToDepartments(this List<DepartmentData> input);
    
    public static partial Department? ToDepartment(this DepartmentData? input);
    
    public static partial CreateDepartmentCommand ToCreateDepartmentCommand(this CreateDepartmentRequest input);
    
    public static partial List<Employee> ToEmployees(this List<EmployeeData> input);
    
    public static partial Employee? ToEmployee(this EmployeeData? input);
    
    public static partial CreateEmployeeCommand ToCreateEmployeeCommand(this CreateEmployeeRequest input);
}