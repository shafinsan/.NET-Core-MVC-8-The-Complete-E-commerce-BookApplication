using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        public void AddItem(T entity);
        public void DeleteItem(T entity);
        public void DeleteAllItem(IEnumerable<T> entity);
        public T GetSingleItem(Expression<Func<T,bool>>filter, string? includeValue = null);
        public IEnumerable<T> GetAllItems(Expression<Func<T, bool>>? filter = null, string? includeValue = null);


    }
}
