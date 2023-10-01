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
}