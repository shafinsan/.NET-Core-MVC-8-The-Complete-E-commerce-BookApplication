using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository.IRepository
{
    public interface IUnionOfWork
    {
        public ICategory MyCategoryRepo { get; }
        public IBook MyBookRepo { get; }
        public ICompany MyCompanyRepo { get; }
        public IApplicationUser MyApplicationUserRepo { get; }
        public ICart MyCartRepo { get; }
        public IOrderHeader MyOrderHeaderRepo { get; }
        public void Save();
    }
}
