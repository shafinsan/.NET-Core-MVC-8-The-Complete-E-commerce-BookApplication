using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using EJApplication.ModelsLayer.Utility.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Security.Claims;

namespace EJApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnionOfWork _db;
        private readonly StripeClient _stripeClient;
        private readonly IOptions<StripeSettings> _stripeSettings;
        public CartController(IUnionOfWork db, StripeClient stripeClient, IOptions<StripeSettings> stripeSettings)
        {
            _db = db;
            _stripeClient = stripeClient;
            _stripeSettings = stripeSettings;
        }
        [Authorize]
        public IActionResult Index()
        {
            var claimId = (ClaimsIdentity)User.Identity;
            var userId=claimId.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<ShopingCartModel> list = _db.MyCartRepo.GetAllItems(u=>u.ApplicationId==userId,includeValue: "Book,ApplicationUser").ToList();
            if (list==null)
            {
                return View(new CartAndShopping());
            }
            CartAndShopping model = new()
            {
                shoppingCard = list,
                totalPrice = 0

            };
            foreach(var obj in list)
            {
                if (obj.Count >= 1 && obj.Count<=50)
                {
                    obj.Price = (double)obj.Book.Price;
                    model.totalPrice += (double)obj.Price*obj.Count;
                }
                else if (obj.Count >= 51 && obj.Count <= 100)
                {
                    obj.Price = (double)obj.Book.Price50;
                    model.totalPrice += (double)obj.Price * obj.Count;
                }
                else
                {
                    obj.Price = (double)obj.Book.Price100;
                    model.totalPrice += (double)obj.Price * obj.Count;
                }
            }
            
            return View(model);
        }
        public IActionResult Plush(int ?id)
        {
            ShopingCartModel list = _db.MyCartRepo.GetSingleItem(u=>u.Id==id);
            list.Count += 1;
            _db.MyCartRepo.Update(list);
            _db.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int? id)
        {
            ShopingCartModel list = _db.MyCartRepo.GetSingleItem(u => u.Id == id);
            if (list.Count > 1)
            {
                list.Count -= 1;
                _db.MyCartRepo.Update(list);
            }
            else
            {
                
                _db.MyCartRepo.DeleteItem(list);
            }
            
            _db.Save();
            var claimId = (ClaimsIdentity)User.Identity;
            var userId = claimId?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                List<ShopingCartModel> dataForCookie = _db.MyCartRepo.GetAllItems(u => u.ApplicationId == userId).ToList();
                HttpContext.Session.SetInt32(StaticDetails.shoppingCard, dataForCookie.Count());
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
           
            ShopingCartModel list = _db.MyCartRepo.GetSingleItem(u => u.Id == id);
            _db.MyCartRepo.DeleteItem(list);
            _db.Save();
            var claimId = (ClaimsIdentity)User.Identity;
            var userId = claimId?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                List<ShopingCartModel> dataForCookie = _db.MyCartRepo.GetAllItems(u => u.ApplicationId == userId).ToList();
                HttpContext.Session.SetInt32(StaticDetails.shoppingCard, dataForCookie.Count());
            }
            return RedirectToAction("Index");
        }
        [Authorize]
      
        public IActionResult Summary()
        {
            var ClaimId = (ClaimsIdentity)User.Identity;
            var userId = ClaimId.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<ShopingCartModel> list = _db.MyCartRepo.GetAllItems(u => u.ApplicationId == userId,includeValue: "Book,ApplicationUser").ToList();
            CartAndOrderHeader header = new()
            {
                list = list,
                order = new(),
                total=0.0,
            };
            foreach (var obj in list)
            {
                if (obj.Count >= 1 && obj.Count <= 50)
                {
                    obj.Price = (double)obj.Book.Price;
                    header.total += (double)obj.Book.Price*obj.Count;
                   
                }
                else if (obj.Count >= 51 && obj.Count <= 100)
                {
                    obj.Price = (double)obj.Book.Price50;
                    header.total += (double)obj.Book.Price50 * obj.Count;

                }
                else
                {
                    obj.Price = (double)obj.Book.Price100;
                    header.total += (double)obj.Book.Price100 * obj.Count;

                }
            }
           
            return View(header);
        }
        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> Summary(CartAndOrderHeader myObj)
        {
            var ClaimId = (ClaimsIdentity)User.Identity;
            var userId = ClaimId.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartAndOrderHeader header = new()
            {
                list = null,
                order = myObj.order,
                total = 0.0,
            };

            if (!ModelState.IsValid)
            {
               
                List<ShopingCartModel> list = _db.MyCartRepo.GetAllItems(u => u.ApplicationId == userId, includeValue: "Book,ApplicationUser").ToList();
                header.list = list;
                foreach (var obj in list)
                {
                    if (obj.Count >= 1 && obj.Count <= 50)
                    {
                        obj.Price = (double)obj.Book.Price;
                        header.total += (double)obj.Book.Price * obj.Count;

                    }
                    else if (obj.Count >= 51 && obj.Count <= 100)
                    {
                        obj.Price = (double)obj.Book.Price50;
                        header.total += (double)obj.Book.Price50 * obj.Count;

                    }
                    else
                    {
                        obj.Price = (double)obj.Book.Price100;
                        header.total += (double)obj.Book.Price100 * obj.Count;

                    }
                }
                return View(header);
            }



            //strip setting

            if (ModelState.IsValid)
            {

                List<ShopingCartModel> list1 = _db.MyCartRepo.GetAllItems(u => u.ApplicationId == userId, includeValue: "Book,ApplicationUser").ToList();

                header.list = list1;


                foreach (var obj in list1)
                {
                    if (obj.Count >= 1 && obj.Count <= 50)
                    {
                        obj.Price = (double)obj.Book.Price;
                        header.total += (double)obj.Book.Price * obj.Count;
                    }
                    else if (obj.Count >= 51 && obj.Count <= 100)
                    {
                        obj.Price = (double)obj.Book.Price50;
                        header.total += (double)obj.Book.Price50 * obj.Count;
                    }
                    else
                    {
                        obj.Price = (double)obj.Book.Price100;
                        header.total += (double)obj.Book.Price100 * obj.Count;
                    }
                }
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = list1.Select(item => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // Convert price to cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Book.Title,
                            },
                        },
                        Quantity = item.Count,
                    }).ToList(),
                    Mode = "payment",
                    SuccessUrl = Url.Action("SuccessFull", "Cart", new { paymentSuccess = true, accountId = userId,netTotal=myObj.total }, Request.Scheme),
                    CancelUrl = Url.Action("Summary", "Cart", new { paymentSuccess = false }, Request.Scheme),
                };

                var service = new SessionService();
                var session = service.Create(options);
                //add to db
                OrderHeader orderHeader = header.order;
                orderHeader.ApplicationId = userId;
                orderHeader.SessionId = session.Id;
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                orderHeader.PaymentStatus=StaticDetails.PaymentStatusPending;
                orderHeader.TotalOrder = header.total;
                orderHeader.OrderStatus = StaticDetails.StatusPending;
                TempData["session"] = session.Id;
                _db.MyOrderHeaderRepo.AddItem(orderHeader);
                _db.Save();

                // Redirect to Stripe Checkout
                Response.Headers.Add("Location", session.Url);
                return Redirect(session.Url);
                
            }
            return View(header);

        }
        public IActionResult SuccessFull(string accountId,string netTotal)
        {
           

            ViewBag.AccountId = accountId;
            ViewBag.NetTotal = netTotal;

            var sessionId = TempData["session"].ToString();
            var service = new SessionService();
            var session = service.Get(sessionId);


            OrderHeader header = _db.MyOrderHeaderRepo.GetSingleItem(u=>u.ApplicationId==accountId && u.SessionId==sessionId);
            header.PaymentStatus = StaticDetails.PaymentStatusApproved;
            header.OrderStatus = StaticDetails.StatusApproved;
            header.PaymentIntentId = session.PaymentIntentId;
           
            header.PaymentDate=DateTime.Now;
            header.OrderDate = DateTime.Now;
            header.ShippingDate=DateTime.Now;
            _db.MyOrderHeaderRepo.Update(header);
            _db.Save();

            List<ShopingCartModel> allrelatedData = _db.MyCartRepo.GetAllItems(u=>u.ApplicationId==accountId).ToList();
            _db.MyCartRepo.DeleteAllItem(allrelatedData);
            _db.Save();
            HttpContext.Session.SetInt32(StaticDetails.shoppingCard,0);

            return View();
        }

    }
}
