using EJApplication.ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository.IRepository
{
    public interface ICart:IRepository<ShopingCartModel>
    {
        public void Update(ShopingCartModel obj);
    }
}
