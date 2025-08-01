using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Application.Interfaces;

public interface IDepartmentService
{
    Task<string> CreateDepartment(DepartmentRequest request);

    Task<DepartmentResponseWithUsers> GetDepartmentById(string id);
    
    Task<DepartmentResponseWithUsers> GetDepartmentByName(string name);
    
    Task<IEnumerable<DepartmentResponseWithUsers>> GetAllDepartments();
    
    Task<IEnumerable<DepartmentResponseWithUsers>> GetActiveDepartments();
    
    Task<string> UpdateDepartment(string departmentId, DepartmentRequest request);
    
    Task<DepartmentResponseWithUsers> AddEmployeeInDepartment(string departmentId, string employeeId);
    
    Task<DepartmentResponseWithUsers> RemoveEmployeeFromDepartment(string departmentId, string employeeId);
    
    Task DeactivateDepartment(string id);
    
    Task ActivateDepartment(string id);
}