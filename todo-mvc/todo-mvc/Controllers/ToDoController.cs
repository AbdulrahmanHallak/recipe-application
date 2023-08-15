using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using todo_mvc.Models;

namespace todo_mvc.Controllers;

public class ToDoController : Controller
{
    private readonly ToDoService _toDoService;

    public ToDoController(ToDoService service)
    {
        _toDoService = service;
    }
    public IActionResult Category(string category)
    {
        var items = _toDoService.GetItemsForCategory(category);
        return View(items);
    }
    // Displays the todo items in the homepage.
    public IActionResult Index() 
    {
        var items = _toDoService.GetItems();
        return View(items);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
