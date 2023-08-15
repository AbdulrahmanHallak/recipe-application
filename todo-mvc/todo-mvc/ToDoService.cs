using todo_mvc.Models;

namespace todo_mvc;
public class ToDoService
{
    private readonly List<ToDoListModel> _items = new()
    {
            new ToDoListModel{Category= "Simple", Title="Bread"},
            new ToDoListModel{Category= "Simple", Title="Milk"},
            new ToDoListModel{Category= "Simple", Title="Get Gas"},
            new ToDoListModel{Category= "Long", Title="Write Book"},
            new ToDoListModel{Category= "Long", Title="Build Application"},
    };

    public CategoryViewModel GetItemsForCategory(string category)
    {
        var list = _items.Where(x => string.Equals(category, x.Category, StringComparison.OrdinalIgnoreCase)).ToList();
        return new CategoryViewModel(items: list , categoryName: category);
    }

    public List<ToDoListModel> GetItems() => _items.Take(5).ToList(); // Return todo items to view in the homepage.
}
