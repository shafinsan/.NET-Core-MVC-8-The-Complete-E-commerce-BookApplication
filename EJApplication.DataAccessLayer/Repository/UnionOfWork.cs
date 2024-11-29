using EJApplication.DataAccessLayer.Data;
using EJApplication.DataAccessLayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository
{
    public class UnionOfWork : IUnionOfWork
    {
        private readonly MyDbApplicationContext _db;
        public ICategory MyCategoryRepo { get; }
        public IBook MyBookRepo { get; }

        public ICompany MyCompanyRepo { get; }

        public IApplicationUser MyApplicationUserRepo {  get; }
        public ICart MyCartRepo { get; }
        public IOrderHeader MyOrderHeaderRepo { get; }

        public UnionOfWork(MyDbApplicationContext db) {
            _db = db;
            MyCategoryRepo=new CategoryRepo(db);
            MyBookRepo=new BookRepo(db);
            MyCompanyRepo=new CompanyRepo(db);
            MyApplicationUserRepo=new ApplicationRepo(db);
            MyCartRepo=new CartRepo(db);
            MyOrderHeaderRepo=new OrderHeaderRepo(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
