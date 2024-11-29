using EJApplication.DataAccessLayer.Data;
using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using EJApplication.ModelsLayer.Utility.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EJApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.admin)]
    public class AllUserController : Controller
    {
        private readonly IUnionOfWork _db;
        private readonly MyDbApplicationContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AllUserController(IUnionOfWork db, MyDbApplicationContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationModel>list=_db.MyApplicationUserRepo.GetAllItems(includeValue: "Employee,Company").ToList();
            
            foreach(var obj in list)
            {
                var userRole = _context.UserRoles.FirstOrDefault(u=>u.UserId==obj.Id).RoleId;
                var role = _context.Roles.FirstOrDefault(u => u.Id == userRole).Name;
                obj.tempRole = role;

            }
            return View(list);
        }
        public IActionResult LockUnlock(string? id)
        {
            ApplicationModel target = _db.MyApplicationUserRepo.GetSingleItem(u => u.Id == id);
            var userRole = _context.UserRoles.FirstOrDefault(u => u.UserId == target.Id).RoleId;
            var role = _context.Roles.FirstOrDefault(u => u.Id == userRole).Name;
            if (role == StaticDetails.admin)
            {
                return RedirectToAction("Index");
            }
            if (target.LockoutEnd != null)
            {
                target.LockoutEnd = null;
                TempData["LockStatus"] = "Unlocked";
                _db.Save();
            }
            else
            {
                target.LockoutEnd = DateTime.Now.AddYears(100);
                TempData["LockStatus"] = "Lock";
                _db.Save();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(string ?id) 
        {
            ApplicationModel target = _db.MyApplicationUserRepo.GetSingleItem(u => u.Id == id);
            var userRole = _context.UserRoles.FirstOrDefault(u => u.UserId == target.Id).RoleId;
            var role = _context.Roles.FirstOrDefault(u => u.Id == userRole).Name;
            target.tempRole= role;
            ApplicationUserAndRole applicationUserAndRole = new ApplicationUserAndRole()
            {
                myModel = target,
                Role=_roleManager.Roles.Select(u=>u.Name).Select(u=>new SelectListItem
                {
                    Text=u,
                    Value=u
                })
            };
            return View(applicationUserAndRole);
        }
        [HttpPost]
        public IActionResult Edit(string Name,string myRole,string Id)
        {
            ApplicationModel target = _db.MyApplicationUserRepo.GetSingleItem(u => u.Id == Id);
            var userRole = _context.UserRoles.FirstOrDefault(u => u.UserId == target.Id).RoleId;
            var oldRole = _context.Roles.FirstOrDefault(u => u.Id == userRole).Name;
            var currentRole = oldRole;


            if (oldRole != myRole)
            {
                currentRole = myRole;
                _userManager.RemoveFromRoleAsync(target,oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(target, myRole).GetAwaiter().GetResult();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                target.Name= Name;
                _db.MyApplicationUserRepo.Update(target);
                _db.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
