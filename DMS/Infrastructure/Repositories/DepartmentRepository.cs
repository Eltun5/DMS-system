using DepartmentManagementApp.Application.DTOs.Requests;
using DepartmentManagementApp.Domain.Models;
using DepartmentManagementApp.Infrastructure.DBContext;
using DepartmentManagementApp.Infrastructure.Interfaces;

namespace DepartmentManagementApp.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Department> CreateDepartment(DepartmentRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Department> GetDepartmentById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Department> GetDepartmentByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Department>> GetAllDepartments()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Department>> GetActiveDepartments()
    {
        throw new NotImplementedException();
    }

    public Task<Department> UpdateDepartment(DepartmentRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Department> AddEmployeeInDepartment(string departmentId, string employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<Department> RemoveEmployeeFromDepartment(string departmentId, string employeeId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDepartment(string id)
    {
        throw new NotImplementedException();
    }
}