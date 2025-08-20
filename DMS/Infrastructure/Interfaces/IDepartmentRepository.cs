using WebApplication1.Domain.Models;

namespace WebApplication1.Infrastructure.Interfaces;

public interface IDepartmentRepository
{
    Task CreateDepartment(Department department);

    Task<Department?> GetDepartmentById(string id);
    
    Task<Department?> GetDepartmentByName(string name);
    
    Task<IEnumerable<Department>> GetAllDepartments();
    
    Task<IEnumerable<Department>> GetActiveDepartments();
    
    Task UpdateDepartment(Department department);
    
    Task<Department?> AddEmployeeInDepartment(string departmentId, string employeeId);
    
    Task<Department?> RemoveEmployeeFromDepartment(string departmentId, string employeeId);
    
    Task DeactivateDepartment(string id);
    
    Task ActivateDepartment(string id);
    
    bool ExistsByName(string name);
}