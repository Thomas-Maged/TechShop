using E_commerce_entities.Data;
using E_commerce_entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce_entities.Repo
{
    public interface IUnitOfWork
    {
        public IEntityRepo<Address> addressRepo { get; }
        public IEntityRepo<ApplicationUser> userRepo { get; }
        public IEntityRepo<Category> categoryRepo { get; }
        public IEntityRepo<Order> orderRepo { get; }
        public IEntityRepo<Order_Item> order_itemRepo { get; }
        public IEntityRepo<Product> productRepo { get; }
        public IEntityRepo<CartItems> cartRepo { get; }
        public Context context { get; }
        public void SaveChanges();
    }
}
