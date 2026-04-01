using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q_A.BLL.Interface;
using Q_A.Contract;
using Q_A.web.Areas.Admin.Models;

namespace Q_A.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categoryService.CreateAsync(new CreateCategoryDTO
            {
                Name = model.Name,
                Slug = model.Slug,
                Description = model.Description
            });
            return RedirectToAction(nameof(Index));
        }
    }
}
