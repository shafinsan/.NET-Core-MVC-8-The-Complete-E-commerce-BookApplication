using EJApplication.DataAccessLayer.Data;
using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository
{
    public class CompanyRepo:Repository<CompanyModel>,ICompany
    {
        private MyDbApplicationContext _db;
        public CompanyRepo(MyDbApplicationContext db) : base(db) { 
            _db = db;
        }

        public void Update(CompanyModel obj)
        {
            _db.Update(obj);
        }
    }
}
