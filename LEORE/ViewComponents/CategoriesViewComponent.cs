using LEORE.Data;
using LEORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoriesViewComponent : ViewComponent
{
    private readonly LEOREContext _context;

    public CategoriesViewComponent(LEOREContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }
}

