using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Requests;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.API.Controllers;

/// <summary>
/// API controller for managing department-related operations.
/// </summary>
[ApiController]
[Route("api/v1/department")]
public class DepartmentController(IDepartmentService departmentService)
{
    /// <summary>
    /// Creates a new department.
    /// </summary>
    /// <param name="request">The department information to be created.</param>
    /// <returns>A success message if the department was created successfully.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<string>> AddDepartment([FromBody] DepartmentRequest request)
    {
        return await departmentService.CreateDepartment(request);
    }

    /// <summary>
    /// Retrieves department details by ID.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <returns>The department with its associated users.</returns>
    [HttpGet("id")]
    [Authorize]
    public async Task<ActionResult<DepartmentResponseWithUsers>> GetDepartmentById([FromQuery] string id)
    {
        return await departmentService.GetDepartmentById(id);
    }

    /// <summary>
    /// Retrieves department details by name.
    /// </summary>
    /// <param name="departmentName">The name of the department to retrieve.</param>
    /// <returns>The department with its associated users.</returns>
    [HttpGet("name")]
    [Authorize]
    public async Task<ActionResult<DepartmentResponseWithUsers>> GetDepartmentByName([FromQuery] string departmentName)
    {
        return await departmentService.GetDepartmentByName(departmentName);
    }

    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <returns>A list of all departments and their associated users.</returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<DepartmentResponseWithUsers>>> GetDepartments()
    {
        return new OkObjectResult(await departmentService.GetAllDepartments());
    }

    /// <summary>
    /// Retrieves all active departments.
    /// </summary>
    /// <returns>A list of active departments and their associated users.</returns>
    [HttpGet("active")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<IEnumerable<DepartmentResponseWithUsers>>> GetActiveDepartments()
    {
        return new OkObjectResult(await departmentService.GetActiveDepartments());
    }

    /// <summary>
    /// Updates an existing department.
    /// </summary>
    /// <param name="id">The ID of the department to update.</param>
    /// <param name="request">The updated department information.</param>
    /// <returns>A success message if the update was successful.</returns>
    [HttpPut]
    [Authorize]
    public async Task<ActionResult<string>> UpdateDepartment(
        [FromQuery] string id,
        [FromBody] DepartmentRequest request)
    {
        return await departmentService.UpdateDepartment(id, request);
    }

    /// <summary>
    /// Adds an employee to a department.
    /// </summary>
    /// <param name="departmentId">The ID of the department.</param>
    /// <param name="employeeId">The ID of the employee to add.</param>
    /// <returns>The updated department with the new employee added.</returns>
    [HttpPatch("add")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<DepartmentResponseWithUsers>> AddEmployeeInDepartment(
        [FromQuery] string departmentId,
        [FromQuery] string employeeId)
    {
        return await departmentService.AddEmployeeInDepartment(departmentId, employeeId);
    }

    /// <summary>
    /// Removes an employee from a department.
    /// </summary>
    /// <param name="departmentId">The ID of the department.</param>
    /// <param name="employeeId">The ID of the employee to remove.</param>
    /// <returns>The updated department with the employee removed.</returns>
    [HttpPatch("remove")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<DepartmentResponseWithUsers>> RemoveEmployeeInDepartment(
        [FromQuery] string departmentId,
        [FromQuery] string employeeId)
    {
        return await departmentService.RemoveEmployeeFromDepartment(departmentId, employeeId);
    }

    /// <summary>
    /// Deactivates an existing department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to be deactivated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpPost("deactivate")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task DeactivateDepartment([FromQuery]string id)
    {
        await departmentService.DeactivateDepartment(id);
    }

    /// <summary>
    /// Activates a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to be activated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [HttpPost("activate")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task ActivateDepartment([FromQuery]string id)
    {
        await departmentService.ActivateDepartment(id);
    }
}
