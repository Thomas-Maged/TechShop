using E_commerce_entities.Data;
using E_commerce_entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_commerce_entities.Repo
{
    public class EntityRepo<T> : IEntityRepo<T> where T : class
    {
        protected Context context;
        protected DbSet<T> set;
        public EntityRepo(Context _context)
        {
            context = _context;
            set = context.Set<T>();
        }
        public void Add(T entity)
        {
            set.Add(entity);
        }

        public void Delete(string id)
        {
            T entity = GetByID(id);
            set.Remove(entity);
        }

        //Get All that handles pagination
        public List<T> GetAll(string navProp = null, int toSkip = 0, int toTake = -1)
        {
            IQueryable<T> query = set;
            if (toTake == -1)
            {
                toTake = set.Count();
            }
            if (navProp != null)
            {
                query = query.Include(navProp);
            }

            var keyName = context.Model
                .FindEntityType(typeof(T))
                ?.FindPrimaryKey()
                ?.Properties
                .Select(x => x.Name)
                .FirstOrDefault();

            if (keyName != null)
            {
                query = query.OrderBy(e => EF.Property<object>(e, keyName));
            }

            return query.Skip(toSkip).Take(toTake).ToList();
        }

        public T GetByID(string id)
        {
            return set.Find(id);
        }

        public void Update(T entity)
        {
            set.Update(entity);
        }

        public IQueryable<T> Find(Expression<Func<T,bool>> cond, string navProp = null, string navProp2 = null)
        {
            if (navProp != null && navProp2 != null)
            {
                return set.Where(cond).Include(navProp).Include(navProp2);
            }
            if (navProp != null)
            {
                 return set.Where(cond).Include(navProp);
            }
            return set.Where(cond);
        }

        public bool Exists(Expression<Func<T, bool>> cond)
        {
            return set.Any(cond);
        }
    }
}
