using EJApplication.ModelsLayer.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository.IRepository
{
    public interface IOrderHeader:IRepository<OrderHeader>
    {
        public void Update(OrderHeader obj);
    }
}
