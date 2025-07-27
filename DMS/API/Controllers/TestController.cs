using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentManagementApp.API.Controllers;

[Route("api/v1/test")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public string Get()
    {
        return "Hello!";
    }

    [HttpGet("test")]
    public string GetTest()
    {
        return "Hello Test";
    }

    [HttpPost]
    public string Create()
    {
        return "Hello Poost";
    }

    [HttpPut("{id}")]
    public string Update(int id)
    {
        return "Hello Update id : " + id;
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public string Delete(int id)
    {
        return "Hello Delete id : " + id;
    }
}