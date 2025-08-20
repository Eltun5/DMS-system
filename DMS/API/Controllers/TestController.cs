using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.DBContext;

namespace WebApplication1.API.Controllers;

[Route("api/v1/test")]
[ApiController]
public class TestController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IEnumerable<Message> Get()
    {
        return db.Messages.ToList();
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

    [HttpPut]
    public string Update([FromQuery]int id)
    {
        return "Hello Update id : " + id;
    }
    
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public string Delete([FromQuery]int id)
    {
        return "Hello Admin Delete id : " + id;
    }
}