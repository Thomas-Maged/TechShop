using E_commerce_app.Models;
using E_commerce_entities.Models;
using E_commerce_entities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_commerce_app.Controllers
{
    public class ProductController : Controller
    {
        IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index(int pg, string search = null, string catID = "all", int minPrice = 0, int maxPrice = 0)
        {
            ProductsViewModel productsViewModel = new ProductsViewModel();
            int pageSize = 12;
            int toSkip = (pg - 1) * pageSize;
            int toTake = pageSize;
            productsViewModel.CategoryID = catID;
            productsViewModel.Page = pg;
            if (search != null)
            {
                if (User.IsInRole("admin"))
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.Name.Contains(search)).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.Name.Contains(search)).Count();
                }
                else
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.Name.Contains(search) && p.IsActive == true).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.Name.Contains(search) && p.IsActive == true).Count();
                }
            }
            else if (catID != "all" && maxPrice > 0 && minPrice >= 0)
            {
                if (User.IsInRole("admin"))
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.Category.CategoryID == catID && p.Price >= minPrice && p.Price <= maxPrice, "Category").Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.Category.CategoryID == catID && p.Price >= minPrice && p.Price <= maxPrice).Count();
                }
                else
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.IsActive == true && p.Category.CategoryID == catID && p.Price >= minPrice && p.Price <= maxPrice, "Category").Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.IsActive == true && p.Category.CategoryID == catID && p.Price >= minPrice && p.Price <= maxPrice).Count();
                }
                productsViewModel.MaxPrice = maxPrice;
                productsViewModel.MinPrice = minPrice;
            }
            else if (catID != "all")
            {
                if (User.IsInRole("admin"))
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.Category.CategoryID == catID, "Category").Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.Category.CategoryID == catID).Count();
                }
                else
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.IsActive == true && p.Category.CategoryID == catID, "Category").Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.IsActive == true && p.Category.CategoryID == catID).Count();
                }
            }
            else if (maxPrice > 0 && minPrice >= 0)
            {
                if (User.IsInRole("admin"))
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.Price >= minPrice && p.Price <= maxPrice, "Category").Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.Price >= minPrice && p.Price <= maxPrice, "Category").Count();
                }
                else
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.IsActive == true && p.Price >= minPrice && p.Price <= maxPrice, "Category").Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.IsActive == true && p.Price >= minPrice && p.Price <= maxPrice, "Category").Count();
                }
                productsViewModel.MaxPrice = maxPrice;
                productsViewModel.MinPrice = minPrice;
            }
            else
            {
                if (User.IsInRole("admin"))
                {
                    productsViewModel.Products = unitOfWork.productRepo.GetAll("Category", toSkip, toTake);
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(_ => true).Count();
                }
                else
                {
                    productsViewModel.Products = unitOfWork.productRepo.Find(p => p.IsActive == true).Skip(toSkip).Take(toTake).ToList();
                    productsViewModel.TotalNumberOfProducts = unitOfWork.productRepo.Find(p => p.IsActive == true).Count();
                }
            }
            productsViewModel.Categories = unitOfWork.categoryRepo.GetAll();
            productsViewModel.FromProduct = toSkip + 1;
            productsViewModel.ToProduct = productsViewModel.FromProduct + productsViewModel.Products.Count() - 1;

            return View(productsViewModel);
        }

        public IActionResult Details(String productID)
        {
            var product = unitOfWork.productRepo.Find(p => p.ProductID == productID, "Category").ToList()[0];
            return View(product);
        }

        [Authorize(Roles ="admin")]
        public IActionResult Add()
        {
            ViewBag.Categories = unitOfWork.categoryRepo.GetAll();
            return View();
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }
            Category category = unitOfWork.categoryRepo.GetByID(productViewModel.CategoryID);
            Product product = new Product()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                CreatedAt = DateTime.Now,
                StockQuantity = productViewModel.StockQuantity,
                DiscountPercentage = productViewModel.DiscountPercentage,
                IsActive = productViewModel.IsActive,
                Description = productViewModel.Description,
                Category = category
            };

            // 1. Define where to save (wwwroot/images/products)
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

            // 2. Generate a unique name using a GUID
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + productViewModel.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 3. Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await productViewModel.Image.CopyToAsync(fileStream);
            }

            product.Image = Path.Combine("/images/products", uniqueFileName);
            unitOfWork.productRepo.Add(product);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index", new { pg = 1 });
        }

        [Authorize(Roles ="admin")]
        public IActionResult Edit(String id)
        {
            ViewBag.Categories = unitOfWork.categoryRepo.GetAll();
            Product product = unitOfWork.productRepo.GetByID(id);
            ProductViewModel productViewModel = new ProductViewModel()
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                DiscountPercentage = product.DiscountPercentage,
                CategoryID = product.Category.CategoryID,
                IsActive = product.IsActive,
                StockQuantity = product.StockQuantity,
                ImagePath = product.Image
            };
            return View(productViewModel);
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }
            Category category = unitOfWork.categoryRepo.GetByID(productViewModel.CategoryID);
            Product product = new Product()
            {
                ProductID = productViewModel.ProductID,
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                StockQuantity = productViewModel.StockQuantity,
                DiscountPercentage = productViewModel.DiscountPercentage,
                IsActive = productViewModel.IsActive,
                Description = productViewModel.Description,
                Category = category
            };

            if (productViewModel.Image == null)
            {
                product.Image = productViewModel.ImagePath;
            }
            else
            {
                // Delet the old image
                string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productViewModel.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // 1. Define where to save (wwwroot/images/products)
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                // 2. Generate a unique name using a GUID
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + productViewModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 3. Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await productViewModel.Image.CopyToAsync(fileStream);
                }

                product.Image = Path.Combine("/images/products", uniqueFileName);
            }
            unitOfWork.productRepo.Update(product);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index", new {pg = 1});
        }

        [Authorize(Roles ="admin")]
        public IActionResult Delete(string id)
        {
            Product product = unitOfWork.productRepo.GetByID(id);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.Image.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            unitOfWork.productRepo.Delete(id);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index", new { pg = 1 });
        }
    }
}
