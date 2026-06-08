using E_commerce_entities.Models;
using E_commerce_entities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_app.Controllers
{
    [Authorize(Roles ="admin")]
    public class CategoryController : Controller
    {
        IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categories = unitOfWork.categoryRepo.GetAll();
            return View(categories);
        }

        [HttpPost]
        public IActionResult Add(string categoryName)
        {
            Category category = new Category();
            category.Name = categoryName;
            unitOfWork.categoryRepo.Add(category);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(string id, string categoryName)
        {
            Category category = unitOfWork.categoryRepo.GetByID(id);
            category.Name = categoryName;
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            Category unCategorized = unitOfWork.categoryRepo.Find(c => c.Name == "uncategorized").FirstOrDefault();
            Category category = unitOfWork.categoryRepo.Find(c => c.CategoryID == id, "Products").FirstOrDefault();
            foreach (var product in category.Products)
            {
                product.Category = unCategorized;
            }
            unitOfWork.categoryRepo.Delete(category.CategoryID);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
