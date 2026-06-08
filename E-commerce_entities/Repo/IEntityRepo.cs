using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_commerce_entities.Repo
{
    public interface IEntityRepo<T>
    {
        public List<T> GetAll(string navProp = null, int toSkip = 0, int toTake = -1);
        public T GetByID(string id);
        public void Add(T entity);
        public void Delete(string id);
        public void Update(T entity);
        public IQueryable<T> Find(Expression<Func<T, bool>> cond, string navProp = null, string navProp2 = null);
        public bool Exists(Expression<Func<T, bool>> cond);
    }
}
