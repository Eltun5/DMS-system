using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;

namespace DepartmentManagementApp.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentResponseWithUsers> CreateDepartment(DepartmentRequest request);

    Task<DepartmentResponseWithUsers> GetDepartmentById(string id);
    
    Task<DepartmentResponseWithUsers> GetDepartmentByName(string name);
    
    Task<IEnumerable<DepartmentResponseWithUsers>> GetAllDepartments();
    
    Task<IEnumerable<DepartmentResponseWithUsers>> GetActiveDepartments();
    
    Task<DepartmentResponseWithUsers> UpdateDepartment(DepartmentRequest request);
    
    Task<DepartmentResponseWithUsers> AddEmployeeInDepartment(string departmentId, string employeeId);
    
    Task<DepartmentResponseWithUsers> RemoveEmployeeFromDepartment(string departmentId, string employeeId);
    
    Task DeleteDepartment(string id);
}