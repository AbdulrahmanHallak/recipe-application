using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;
[ApiController]
public class FruitController : ControllerBase
{
    private readonly static List<string> _fruit = new()
    {
        "Pear",
        "Lemon",
        "Peach"
    };
    [HttpGet("fruit")]
    public IEnumerable<string> Index() => _fruit;

    [HttpGet("greetings")]
    public string Hello() => "hello world";

    [HttpGet("fuite/{id?}")]
    public ActionResult<string> View(int id)
    {
        if(id >= 0 && id < _fruit.Count)
            Ok(_fruit[id]);

        return NotFound();
    }
}
