using E_commerce_app.Models;
using E_commerce_entities.Enums;
using E_commerce_entities.Models;
using E_commerce_entities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_commerce_app.Controllers
{
    public class OrderController: Controller
    {
        IUnitOfWork unitOfWork;
        UserManager<ApplicationUser> userManager;
        public OrderController(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index(int orderNumber = 0, StatusEnum? status = null)
        {
            List<Order> orders;
            if (User.IsInRole("admin"))
            {

                orders = unitOfWork.orderRepo.Find(_ => true).OrderBy(o => o.OrderNumber).ToList();
            }
            else
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userManager.FindByIdAsync(id);
                orders = unitOfWork.orderRepo.Find(o => o.User == user).OrderByDescending(o => o.OrderNumber).ToList();
            }
            List<OrderSummaryViewModel> ordersSummary = new List<OrderSummaryViewModel>();
            foreach (var order in orders)
            {
                OrderSummaryViewModel orderSummary = new OrderSummaryViewModel()
                {
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status
                };
                ordersSummary.Add(orderSummary);
            }
            
            return View(ordersSummary);
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(id);
            OrderViewModel orderrViewModel = new OrderViewModel();

            //User Info
            orderrViewModel.FullName = user.FullName;
            //var address = user.Addresses.ToList()[0];
            var address = unitOfWork.addressRepo.Find(a => a.UserID==id && a.IsDefault == true).FirstOrDefault();
            if (address != null)
            {
                orderrViewModel.Country = address.Country;
                orderrViewModel.City = address.City;
                orderrViewModel.Street = address.Street;
                orderrViewModel.ZIP = address.Zip;
                orderrViewModel.Email = user.Email;
                orderrViewModel.Phone = user.PhoneNumber;
            }

            //Cart Items
            List<CartItems> items = unitOfWork.cartRepo.Find(ci => ci.User == user, "Product").ToList();
            foreach (var item in items)
            {
                OrderItemViewModel orderItemViewModel = new OrderItemViewModel();
                orderItemViewModel.Name = item.Product.Name;
                orderItemViewModel.Quantity = item.Quantity;
                orderItemViewModel.Image = item.Product.Image;
                if (item.Product.DiscountPercentage > 0)
                {
                    decimal discount = item.Product.Price * (item.Product.DiscountPercentage / 100);
                    orderrViewModel.TotalSavings += discount * item.Quantity;
                    orderItemViewModel.UnitPrice = item.Product.Price - discount;
                    orderrViewModel.TotalPrice += orderItemViewModel.UnitPrice * item.Quantity;
                }
                else
                {
                    orderItemViewModel.UnitPrice = item.Product.Price;
                    orderrViewModel.TotalPrice += orderItemViewModel.UnitPrice * item.Quantity;
                }
                orderrViewModel.Items.Add(orderItemViewModel);
            }
            return View(orderrViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderViewModel orderrViewModel)
        {
            //user
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(id);

            //Cart items to order items
            List<CartItems> cartItems = unitOfWork.cartRepo.Find(c => c.User == user, "Product").ToList();
            List<Order_Item> orderItems = new List<Order_Item>();
            List<OrderItemViewModel> orderItemsViewModel = new List<OrderItemViewModel>(); //this list will be assigned to the orderViewModel to return with the full data to the view
            decimal totalDiscount = 0;
            foreach (var cartItem in cartItems)
            {

                if (cartItem.Product.StockQuantity < cartItem.Quantity)
                {
                    ModelState.AddModelError("", $"we only have {cartItem.Product.StockQuantity} {cartItem.Product.Name} left in stock, Please adjust your quantity to continue.");
                }

                //adding orderitemviewmodel to assign it to the view model to return it back to the view in case their is an error 
                decimal discount;
                discount = cartItem.Product.DiscountPercentage > 0
                    ? cartItem.Product.Price * (cartItem.Product.DiscountPercentage / 100)
                    : 0;
                totalDiscount += discount * cartItem.Quantity;
                OrderItemViewModel orderItemViewModel = new OrderItemViewModel()
                {
                    Name = cartItem.Product.Name,
                    Quantity = cartItem.Quantity,
                    Image = cartItem.Product.Image,
                    UnitPrice = cartItem.Product.Price - discount
                };
                orderItemsViewModel.Add(orderItemViewModel);

                Order_Item orderItem = new Order_Item()
                {
                    Product = cartItem.Product,
                    UnitPrice = cartItem.UnitPrice,
                    Quantity = cartItem.Quantity,
                    LineTotal = cartItem.UnitPrice * cartItem.Quantity
                };
                orderItems.Add(orderItem);

                //decreasing quantity of the product in the stock
                Product product = cartItem.Product;
                product.StockQuantity = product.StockQuantity - cartItem.Quantity;
                unitOfWork.productRepo.Update(product);
                unitOfWork.cartRepo.Delete(cartItem.CartItemID);
            }

            //create the address for the order
            Address address = new Address()
            {
                Country = orderrViewModel.Country,
                City = orderrViewModel.City,
                Street = orderrViewModel.Street,
                Zip = orderrViewModel.ZIP,
                IsDefault = false,
                User = user
            };

            //create a new order
            Order order = new Order()
            {
                User = user,
                Fullname = orderrViewModel.FullName,
                Email = orderrViewModel.Email,
                Phone = orderrViewModel.Phone,
                TotalAmount = orderrViewModel.TotalPrice,
                TotalDiscount = totalDiscount,
                Address = address,
                OrderDate = DateTime.Now,
                order_Items = orderItems
            };

            orderrViewModel.Items = orderItemsViewModel;
            if (!ModelState.IsValid)
            {
                return View(orderrViewModel);
            }

            unitOfWork.orderRepo.Add(order);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult orderDetails(int orderNum)
        {
            Order order = unitOfWork.orderRepo.Find(o => o.OrderNumber == orderNum, "order_Items", "Address").FirstOrDefault();
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderID = order.OrderID,
                OrderNumber = orderNum,
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                TotalDiscount = order.TotalDiscount,
                Fullname = order.Fullname,
                Email = order.Email,
                Phone = order.Phone,
                Address = order.Address.Country + ", " + order.Address.City + ", " + order.Address.Street + ", " + order.Address.Zip,
            };
            foreach (var item in order.order_Items)
            {
                Product product = unitOfWork.productRepo.GetByID(item.ProductID);
                OrderItemViewModel orderItemViewModel = new OrderItemViewModel()
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Image = product.Image,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                orderDetailsViewModel.Items.Add(orderItemViewModel);
            }

            return View(orderDetailsViewModel);
        }

        [Authorize(Roles ="admin")]
        public IActionResult UpdateStatus(string id, StatusEnum status)
        {
            Order order = unitOfWork.orderRepo.GetByID(id);
            if (order != null)
            {
                order.Status = status;
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
