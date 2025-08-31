using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.API.Controllers;

[ApiController]
[Route("api/v1/department")]
[SwaggerTag("API controller for managing department-related operations.")]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Creates a new department.")]
    public async Task<ActionResult<string>> AddDepartment([FromBody] DepartmentRequest request)
    {
        return await departmentService.CreateDepartment(request);
    }

    [HttpGet("id")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves department details by ID")]
    public async Task<ActionResult<DepartmentResponseWithUsers>> GetDepartmentById([FromQuery] string id)
    {
        return await departmentService.GetDepartmentById(id);
    }

    
    [HttpGet("name")]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves department details by Name.")]
    public async Task<ActionResult<DepartmentResponseWithUsers>> GetDepartmentByName(
        [FromQuery, DefaultValue("HR")] 
        string departmentName)
    {
        return await departmentService.GetDepartmentByName(departmentName);
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Retrieves all departments.")]
    public async Task<ActionResult<IEnumerable<DepartmentResponseWithUsers>>> GetDepartments()
    {
        return new OkObjectResult(await departmentService.GetAllDepartments());
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Retrieves all active departments.")]
    public async Task<ActionResult<IEnumerable<DepartmentResponseWithUsers>>> GetActiveDepartments()
    {
        return new OkObjectResult(await departmentService.GetActiveDepartments());
    }

    [HttpPut]
    [Authorize]
    [SwaggerOperation(Summary = "Updates an existing department.")]
    public async Task<ActionResult<string>> UpdateDepartment(
        [FromQuery] string id,
        [FromBody] DepartmentRequest request)
    {
        return await departmentService.UpdateDepartment(id, request);
    }

    [HttpPatch("add")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Adds an employee to a department.")]
    public async Task<ActionResult<DepartmentResponseWithUsers>> AddEmployeeInDepartment(
        [FromQuery] string departmentId,
        [FromQuery] string employeeId)
    {
        return await departmentService.AddEmployeeInDepartment(departmentId, employeeId);
    }

    [HttpPatch("remove")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Removes an employee from a department.")]
    public async Task<ActionResult<DepartmentResponseWithUsers>> RemoveEmployeeInDepartment(
        [FromQuery] string departmentId,
        [FromQuery] string employeeId)
    {
        return await departmentService.RemoveEmployeeFromDepartment(departmentId, employeeId);
    }

    [HttpPost("deactivate")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Deactivates an existing department by its ID.")]
    public async Task DeactivateDepartment([FromQuery]string id)
    {
        await departmentService.DeactivateDepartment(id);
    }

    [HttpPost("activate")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Activates a department by its ID.")]
    public async Task ActivateDepartment([FromQuery]string id)
    {
        await departmentService.ActivateDepartment(id);
    }
}
