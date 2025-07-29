using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Domain.Models;

namespace DepartmentManagementApp.Infrastructure.Interfaces;

public interface IDepartmentRepository
{
    Task<Department> CreateDepartment(DepartmentRequest request);

    Task<Department> GetDepartmentById(string id);
    
    Task<Department> GetDepartmentByName(string name);
    
    Task<IEnumerable<Department>> GetAllDepartments();
    
    Task<IEnumerable<Department>> GetActiveDepartments();
    
    Task<Department> UpdateDepartment(DepartmentRequest request);
    
    Task<Department> AddEmployeeInDepartment(string departmentId, string employeeId);
    
    Task<Department> RemoveEmployeeFromDepartment(string departmentId, string employeeId);
    
    Task DeleteDepartment(string id);
}