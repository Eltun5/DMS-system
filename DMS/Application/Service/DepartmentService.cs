using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Application.DTOs.Responses;
using DepartmentManagementApp.Application.Interfaces;

namespace DepartmentManagementApp.Application.Service;

public class DepartmentService  : IDepartmentService
{
    public Task<DepartmentResponseWithUsers> CreateDepartment(DepartmentRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentResponseWithUsers> GetDepartmentById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentResponseWithUsers> GetDepartmentByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DepartmentResponseWithUsers>> GetAllDepartments()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DepartmentResponseWithUsers>> GetActiveDepartments()
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentResponseWithUsers> UpdateDepartment(DepartmentRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentResponseWithUsers> AddEmployeeInDepartment(string departmentId, string employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentResponseWithUsers> RemoveEmployeeFromDepartment(string departmentId, string employeeId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDepartment(string id)
    {
        throw new NotImplementedException();
    }
}