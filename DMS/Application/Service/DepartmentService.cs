using AutoMapper;
using Serilog;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Interfaces;

namespace WebApplication1.Application.Service;

public class DepartmentService(
    IConfiguration config,
    IDepartmentRepository departmentRepository,
    IUserRepository userRepository,
    IMapper mapper)
    : IDepartmentService
{

    public async Task<string> CreateDepartment(DepartmentRequest request)
    { 
        Log.Information(config["log:department:service:create:try"]!);
        
        if (!await VerifyCanExistDepartmentWithThisField(request))
        {
            return config["log:department:service:create:failed"]!;
        }

        var department = await InitializeDepartment(request);

        await departmentRepository.CreateDepartment(department);
        Log.Information(config["log:department:service:create:success"]!);
        return config["log:department:service:create:success"]!;
    }

    public async Task<DepartmentResponseWithUsers> GetDepartmentById(string id)
    {
        Log.Information(config["log:department:service:get-by-id"]!);
        return mapper.Map<DepartmentResponseWithUsers>( await departmentRepository.GetDepartmentById(id));
    }

    public async Task<DepartmentResponseWithUsers> GetDepartmentByName(string name)
    {
        Log.Information(config["log:department:service:get-by-name"]!);
        return mapper.Map<DepartmentResponseWithUsers>( await departmentRepository.GetDepartmentByName(name));
    }

    public async Task<IEnumerable<DepartmentResponseWithUsers>> GetAllDepartments()
    {
        Log.Information(config["log:department:service:all-departments"]!);
        IEnumerable<Department> allDepartments = await departmentRepository.GetAllDepartments();
        return mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentResponseWithUsers>>(allDepartments);
    }

    public async Task<IEnumerable<DepartmentResponseWithUsers>> GetActiveDepartments()
    {
        Log.Information(config["log:department:service:get-active-departments"]!);
        return mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentResponseWithUsers>>( await departmentRepository.GetActiveDepartments());
    }

    public async Task<string> UpdateDepartment(string departmentId, DepartmentRequest request)
    {
        Log.Information(config["log:department:service:update:try"]!);
        Department? oldDepartment = await departmentRepository.GetDepartmentById(departmentId);
        if (!await VerifyCanExistDepartmentWithThisField(request) || oldDepartment == null)
        {
            Log.Information(config["log:department:service:update:failed"]!);
            return config["log:department:service:update:failed"]!;
        }
        Department department = await UpdateDepartment(request, oldDepartment);
        await departmentRepository.UpdateDepartment(department);
        Log.Information(config["log:department:service:update:success"]!);
        return config["log:department:service:update:success"]!;
    }

    public async Task<DepartmentResponseWithUsers> AddEmployeeInDepartment(string departmentId, string employeeId)
    {
        Log.Information(config["log:department:service:add-employee"]!);
        return mapper.Map<DepartmentResponseWithUsers>(await departmentRepository.AddEmployeeInDepartment(departmentId, employeeId));
    }

    public async Task<DepartmentResponseWithUsers> RemoveEmployeeFromDepartment(string departmentId, string employeeId)
    {
        Log.Information(config["log:department:service:remove-employee"]!);
        return mapper.Map<DepartmentResponseWithUsers>(await departmentRepository.RemoveEmployeeFromDepartment(departmentId, employeeId));
    }

    public async Task DeactivateDepartment(string id)
    {
        Log.Information(config["log:department:service:deactivate"]!);
        await departmentRepository.DeactivateDepartment(id);
    }

    public async Task ActivateDepartment(string id)
    {
        Log.Information(config["log:department:service:activate"]!);
        await departmentRepository.ActivateDepartment(id);
    }

    private Task<Department> InitializeDepartment(DepartmentRequest request)
    {
        return Task.FromResult(new Department()
        {
            DepartmentName = request.Name,
            Description = request.Description,
            IsActive = true,
            ManagerId = new Guid(request.ManagerId)
        });
    }
    
    private async Task<Department> UpdateDepartment(DepartmentRequest request, Department oldDepartment)
    {
        oldDepartment.DepartmentName = request.Name;
        oldDepartment.Description = request.Description;
        oldDepartment.ManagerId = new Guid(request.ManagerId);
        return oldDepartment;
    }

    private async Task<bool> VerifyCanExistDepartmentWithThisField(DepartmentRequest request)
    {
        User? user = await userRepository.GetById(request.ManagerId);
        if (user == null || user.Role != Role.Manager)
        {
            return false;
        }

        if (departmentRepository.ExistsByName(request.Name))
        {
            return false;
        }
        return true;
    }
}