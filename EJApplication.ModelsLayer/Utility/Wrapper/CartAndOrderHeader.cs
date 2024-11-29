using EJApplication.ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Utility.Wrapper
{
    public class CartAndOrderHeader
    {
        public IEnumerable<ShopingCartModel> list { get; set; }
        public OrderHeader order { get; set; }
        public double total { get; set; } = 0;
    }
}
