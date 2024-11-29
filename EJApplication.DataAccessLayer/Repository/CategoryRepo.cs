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
    public class CategoryRepo:Repository<CategoryModel>,ICategory
    {
        private readonly MyDbApplicationContext _db;
        public CategoryRepo(MyDbApplicationContext db):base(db)
        {
          _db = db;
        }
        public void Update(CategoryModel obj)
        {
            _db.Update(obj);
        }
    }
}
