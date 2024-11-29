using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace EJApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnionOfWork _db;

        public HomeController(ILogger<HomeController> logger, IUnionOfWork db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var claimId = (ClaimsIdentity)User.Identity;
            var userId = claimId?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                List<ShopingCartModel> dataForCookie = _db.MyCartRepo.GetAllItems(u => u.ApplicationId == userId).ToList();
                HttpContext.Session.SetInt32(StaticDetails.shoppingCard, dataForCookie.Count());
            }

            List<BookModel>list=_db.MyBookRepo.GetAllItems(includeValue: "Category").ToList();
            

            return View(list);
        }
        [Authorize]
        public IActionResult Detail(int id)
        {
            ShopingCartModel model = new()
            {
                BookId=id,
                Book=_db.MyBookRepo.GetSingleItem(u=>u.Id==id,includeValue: "Category"),
                Count=1,
            };
            
            return View(model);
        }
        [HttpPost]
        public IActionResult Detail(ShopingCartModel obj)
        {
            var claimId = (ClaimsIdentity)User.Identity;
            var userId = claimId.FindFirst(ClaimTypes.NameIdentifier).Value;
            obj.ApplicationId= userId;
            ShopingCartModel currentUserExits = _db.MyCartRepo.GetSingleItem(u=>u.ApplicationId==userId && u.BookId==obj.BookId);
            if (currentUserExits != null)
            {
                currentUserExits.Count += obj.Count;
                _db.MyCartRepo.Update(currentUserExits);
            }
            else
            {
                obj.Id = 0;
                _db.MyCartRepo.AddItem(obj);
            }
            _db.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
