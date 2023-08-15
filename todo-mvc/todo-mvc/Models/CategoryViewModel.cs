namespace todo_mvc.Models;
public class CategoryViewModel
{
    public List<ToDoListModel> Items { get; set; }
    public string CategoryName { get; set; }
    public CategoryViewModel(List<ToDoListModel> items, string categoryName)
    {
        Items = items;
        CategoryName = categoryName;
    }
}
