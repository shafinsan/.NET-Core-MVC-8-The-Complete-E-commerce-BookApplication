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
    public class CartRepo:Repository<ShopingCartModel>,ICart
    {
        private readonly MyDbApplicationContext _db;
        public CartRepo(MyDbApplicationContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShopingCartModel obj)
        {
            _db.MyShopingCard.Update(obj);
        }
    }
}
