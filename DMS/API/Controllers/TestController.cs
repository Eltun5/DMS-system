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