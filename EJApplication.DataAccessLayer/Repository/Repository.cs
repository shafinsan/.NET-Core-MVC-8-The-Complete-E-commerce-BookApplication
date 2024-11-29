using EJApplication.DataAccessLayer.Data;
using EJApplication.DataAccessLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MyDbApplicationContext _db;
        private readonly DbSet<T> _dbset;
        public Repository(MyDbApplicationContext db)
        {
            _db = db;
            _dbset = _db.Set<T>();
            _db.MyBook.Include(u => u.CategoryId).Include(u=>u.Category);
            _db.MyApplication.Include(u=>u.EmployeeId).Include(u=>u.Employee).Include(u=>u.CompanyId).Include(u=>u.Company);
            _db.MyShopingCard.Include(u => u.ApplicationId).Include(u => u.ApplicationUser).Include(u => u.BookId).Include(u=>u.Book);
            _db.MyOrderHead.Include(u => u.ApplicationId).Include(u => u.ApplicationUser);
        }
        public void AddItem(T entity)
        {
            _dbset.Add(entity);
        }

        public void DeleteAllItem(IEnumerable<T> entity)
        {
            _dbset.RemoveRange(entity);
        }

        public void DeleteItem(T entity)
        {
            _dbset.Remove(entity);
        }

        public IEnumerable<T> GetAllItems(Expression<Func<T, bool>>? filter = null, string? includeValue=null)
        {
            IQueryable<T> query = _dbset;
            if (filter != null)
            {
                query=query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeValue))
            {
                foreach (var obj in includeValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(obj);
                }
            }
            return query.ToList();
        }

        public T GetSingleItem(Expression<Func<T, bool>> filter, string? includeValue = null)
        {
            IQueryable<T> query = _dbset;
            query=query.Where(filter);
            if (!string.IsNullOrEmpty(includeValue))
            {
                foreach(var obj in includeValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(obj);
                }
            }
            return query.FirstOrDefault();
        }
    }
}
