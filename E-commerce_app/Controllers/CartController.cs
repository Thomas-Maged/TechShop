using E_commerce_app.Models;
using E_commerce_entities.Models;
using E_commerce_entities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_commerce_app.Controllers
{
    public class CartController : Controller
    {
        IUnitOfWork unitOfWork;
        UserManager<ApplicationUser> userManager;
        public CartController(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
        }
        [Authorize(Roles ="user")]
        public async Task<IActionResult> Index()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(id);
            List<CartItems> cartItems = unitOfWork.cartRepo.Find(ci => ci.User == user, "Product").ToList();
            CartViewModel cartViewModel = new CartViewModel();
            foreach (var item in cartItems)
            {
                if (item.Quantity > item.Product.StockQuantity)
                {
                    ModelState.AddModelError("", $"we only have {item.Product.StockQuantity} {item.Product.Name} left in stock, Please adjust your quantity.");
                }
                CartItemViewModel itemViewModel = new CartItemViewModel();
                itemViewModel.ProductID = item.ProductID;
                itemViewModel.CartItemID = item.CartItemID;

                itemViewModel.Name = item.Product.Name;
                itemViewModel.Price = item.Product.Price;
                itemViewModel.StockQuantity = item.Product.StockQuantity;
                itemViewModel.DiscountPercentage = item.Product.DiscountPercentage;
                itemViewModel.Image = item.Product.Image;
                itemViewModel.UnitPrice = itemViewModel.Price - (itemViewModel.Price * itemViewModel.DiscountPercentage/100);
                itemViewModel.Discount = itemViewModel.Price - itemViewModel.UnitPrice;
                itemViewModel.Quantity = item.Quantity;

                cartViewModel.TotalSavings += itemViewModel.Discount * itemViewModel.Quantity;
                cartViewModel.TotalPrice += itemViewModel.UnitPrice * itemViewModel.Quantity;

                cartViewModel.Items.Add(itemViewModel);
            }
            return View(cartViewModel);
        }

        [Authorize(Roles ="user")]
        [HttpPost]
        public async Task<IActionResult> Add(string productID, int quantity = 1)
        {
            var product = unitOfWork.productRepo.GetByID(productID);
            if (quantity <= product.StockQuantity)
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userManager.FindByIdAsync(id);
                bool itemExists = unitOfWork.cartRepo.Exists(ci => ci.User == user && ci.Product == product);
                if (itemExists)
                {
                    var currentCartItem = unitOfWork.cartRepo.Find(ci => ci.User == user && ci.Product == product).ToList()[0];
                    currentCartItem.Quantity = currentCartItem.Quantity + quantity;
                    unitOfWork.cartRepo.Update(currentCartItem);
                    unitOfWork.SaveChanges();
                }
                else
                {
                    CartItems cartItem = new CartItems();
                    cartItem.User = user;
                    cartItem.Product = product;
                    cartItem.UnitPrice = product.Price - (product.Price * (product.DiscountPercentage/100));
                    cartItem.Quantity = quantity;
                    unitOfWork.cartRepo.Add(cartItem);
                    unitOfWork.SaveChanges();
                }
            }
            // Get the URL of the current page from the Request Headers
            string referer = Request.Headers["Referer"].ToString();
            return Redirect(referer);
        }

        [Authorize(Roles ="user")]

        [HttpPost]
        public async Task<IActionResult> Remove(string cartItemID)
        {
            bool itemExists = unitOfWork.cartRepo.Exists(ci => ci.CartItemID == cartItemID);
            if (itemExists)
            {
                unitOfWork.cartRepo.Delete(cartItemID);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="user")]
        public async Task<IActionResult> UpdateQuantity(string productID, int quantity)
        {
            var product = unitOfWork.productRepo.GetByID(productID);
            if (quantity <= product.StockQuantity)
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userManager.FindByIdAsync(id);
                bool itemExists = unitOfWork.cartRepo.Exists(ci => ci.User == user && ci.Product == product);
                if (itemExists)
                {
                    var currentCartItem = unitOfWork.cartRepo.Find(ci => ci.User == user && ci.Product == product).ToList()[0];
                    currentCartItem.Quantity = quantity;
                    unitOfWork.cartRepo.Update(currentCartItem);
                    unitOfWork.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        }

    }
}
