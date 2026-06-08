using E_commerce_entities.Data;
using E_commerce_entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce_entities.Repo
{
    public class UnitOfWork:IUnitOfWork
    {
        public IEntityRepo<Address> addressRepo { get; }
        public IEntityRepo<ApplicationUser> userRepo { get; }
        public IEntityRepo<Category> categoryRepo { get; }
        public IEntityRepo<Order> orderRepo { get; }
        public IEntityRepo<Order_Item> order_itemRepo { get; }
        public IEntityRepo<Product> productRepo { get; }
        public IEntityRepo<CartItems> cartRepo { get; }
        public Context context { get; }

        public UnitOfWork(Context _context)
        {
            context = _context;
            addressRepo = new EntityRepo<Address>(context);
            userRepo = new EntityRepo<ApplicationUser>(context);
            categoryRepo = new EntityRepo<Category>(context);
            orderRepo = new EntityRepo<Order>(context);
            order_itemRepo = new EntityRepo<Order_Item>(context);
            productRepo = new EntityRepo<Product>(context);
            cartRepo = new EntityRepo<CartItems>(context);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
