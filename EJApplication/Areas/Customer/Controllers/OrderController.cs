using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace EJApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
      
      
        private readonly IUnionOfWork _db;

        public OrderController(ILogger<HomeController> logger, IUnionOfWork db)
        {
            _db = db;
        }
        public IActionResult Index(string? SearchString,string ?sort,string? value)
        {




            var claimId = (ClaimsIdentity)User.Identity;
            var userId = claimId.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Initialize the list
            List<OrderHeader> list = new List<OrderHeader>();

            if (User.IsInRole(StaticDetails.admin))
            {
                value = string.IsNullOrEmpty(value) ? "All" : value;

                // Filter orders based on the value
                switch (value)
                {
                    case "Pending":
                        list = _db.MyOrderHeaderRepo.GetAllItems(
                            u => u.OrderStatus == StaticDetails.StatusPending,
                            includeValue: "ApplicationUser"
                        ).ToList();
                        ViewData["active"] = "Pending";
                        break;

                    case "Processing":
                        list = _db.MyOrderHeaderRepo.GetAllItems(
                            u => u.OrderStatus == StaticDetails.StatusInProcess,
                            includeValue: "ApplicationUser"
                        ).ToList();
                        ViewData["active"] = "Processing";
                        break;

                    case "Approved":
                        list = _db.MyOrderHeaderRepo.GetAllItems(
                            u => u.OrderStatus == StaticDetails.StatusApproved,
                            includeValue: "ApplicationUser"
                        ).ToList();
                        ViewData["active"] = "Approved";
                        break;

                    case "Shipted":
                        list = _db.MyOrderHeaderRepo.GetAllItems(
                            u => u.OrderStatus == StaticDetails.StatusShipped,
                            includeValue: "ApplicationUser"
                        ).ToList();
                        ViewData["active"] = "Shipted";
                        break;

                    case "Refunding":
                        list = _db.MyOrderHeaderRepo.GetAllItems(
                            u => u.OrderStatus == StaticDetails.StatusRefunded,
                            includeValue: "ApplicationUser"
                        ).ToList();
                        ViewData["active"] = "Refunding";
                        break;

                    case "All":
                    default:
                        list = _db.MyOrderHeaderRepo.GetAllItems(
                            includeValue: "ApplicationUser"
                        ).ToList();
                        ViewData["active"] = "All";
                        break;
                }

                // Apply search filtering
                if (!string.IsNullOrEmpty(SearchString))
                {
                    list = list.Where(u => u.Name.ToLower().Contains(SearchString.ToLower())).ToList();
                }

                // Apply sorting
                ViewData["sort"] = sort == "asc" ? "dsc" : "asc";
                list = sort == "asc"
                    ? list.OrderBy(u => u.Name).ToList()
                    : list.OrderByDescending(u => u.Name).ToList();
                return View(list);
            }

         



            if (User.IsInRole(StaticDetails.employee))
            {
                list = _db.MyOrderHeaderRepo.GetAllItems(u=>u.OrderStatus==StaticDetails.StatusInProcess,includeValue: "ApplicationUser").ToList();
                if(!string.IsNullOrEmpty(SearchString))
                {
                    list = list.Where(u => u.Name.ToLower().Contains(SearchString.ToLower())).ToList();
                }
                ViewData["sort"] = sort == "asc" ? "dsc" : "asc";
                if (sort == "asc")
                {
                    list = list.OrderBy(u => u.Name).ToList();
                }
                else
                {
                    list = list.OrderByDescending(u => u.Name).ToList();
                }
            }
            else
            {
                list = _db.MyOrderHeaderRepo.GetAllItems(u=>u.ApplicationId==userId,includeValue: "ApplicationUser").ToList();
                if (!string.IsNullOrEmpty(SearchString))
                {
                    list = list.Where(u => u.Name.ToLower().Contains(SearchString.ToLower())).ToList();
                }
                ViewData["sort"] = sort == "asc" ? "dsc" : "asc";
                if (sort == "asc")
                {
                    list = list.OrderBy(u => u.Name).ToList();
                }
                else
                {
                    list = list.OrderByDescending(u => u.Name).ToList();
                }
            }

            return View(list);
        }

        public IActionResult OrderDetail(int id)
        {
            OrderHeader list = _db.MyOrderHeaderRepo.GetSingleItem(u=>u.Id==id, includeValue: "ApplicationUser");
            return View(list);
        }
        public IActionResult StartProcess(int id) 
        {
            OrderHeader order = _db.MyOrderHeaderRepo.GetSingleItem(u => u.Id == id);
            order.OrderStatus = StaticDetails.StatusInProcess;
            _db.MyOrderHeaderRepo.Update(order);
            _db.Save();
            return RedirectToAction("Index");
        }
        public IActionResult ShipProcess(int id)
        {
            OrderHeader order = _db.MyOrderHeaderRepo.GetSingleItem(u => u.Id == id);
            order.OrderStatus = StaticDetails.StatusShipped;
            _db.MyOrderHeaderRepo.Update(order);
            _db.Save();
            return RedirectToAction("Index");
        }
        public IActionResult CancelProcess(int id)
        {
            var order = _db.MyOrderHeaderRepo.GetSingleItem(u => u.Id == id);

            if (order == null || string.IsNullOrEmpty(order.PaymentIntentId))
            {
                // If order not found or no PaymentIntentId, redirect to error page
                return RedirectToAction("Error", "Home");
            }

            if (order.PaymentStatus != StaticDetails.PaymentStatusApproved)
            {
                // If the payment is not approved, redirect to error page
                return RedirectToAction("Error", "Home");
            }

            try
            {
                // Initialize Stripe RefundService
                var refundService = new RefundService();

                // Create refund options with PaymentIntentId
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = order.PaymentIntentId, // Reference PaymentIntentId
                };

                // Process the refund
                var refund = refundService.Create(refundOptions);

                // Update order status after successful refund
                order.PaymentStatus = StaticDetails.StatusRefunded;
                order.OrderStatus = StaticDetails.StatusRefunded;

                _db.MyOrderHeaderRepo.Update(order);
                _db.Save();

                // Redirect to the home page on success
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Handle any exception and redirect to error page
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
