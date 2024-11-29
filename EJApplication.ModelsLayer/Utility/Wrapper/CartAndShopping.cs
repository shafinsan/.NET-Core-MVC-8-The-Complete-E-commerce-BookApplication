using EJApplication.ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Utility.Wrapper
{
    public class CartAndShopping
    {
        public IEnumerable<ShopingCartModel> shoppingCard {  get; set; }
        public double totalPrice {  get; set; }
     
    }
}
