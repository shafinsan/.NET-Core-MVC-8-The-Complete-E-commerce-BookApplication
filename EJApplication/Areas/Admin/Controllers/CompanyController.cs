using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EJApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.admin +","+ StaticDetails.company)]

    public class CompanyController : Controller
    {
        private readonly IUnionOfWork _db;
        private readonly IPasswordHasher<ApplicationModel> _passwordHasher;
        private readonly UserManager<IdentityUser> _userManager;
        public CompanyController(IUnionOfWork db, IPasswordHasher<ApplicationModel> passwordHasher, UserManager<IdentityUser> userManager) { 
            _db = db;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<CompanyModel>list=_db.MyCompanyRepo.GetAllItems().ToList();
            return View(list);
        }
        public IActionResult Upsert(int ?id)
        {
            if(id==null || id == 0)
            {
                return View(new CompanyModel());
            }
            CompanyModel obj = _db.MyCompanyRepo.GetSingleItem(u=>u.Id==id);
            return View(obj);
           
        }
        [HttpPost]
        public IActionResult Upsert(CompanyModel? obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _db.MyCompanyRepo.AddItem(obj);
                    _db.Save();
                    ApplicationModel applicationModel = new ApplicationModel();
                    applicationModel.CompanyId=obj.Id;
                    applicationModel.Email=obj.Email;
                    applicationModel.UserName = obj.Email;
                    applicationModel.PhoneNumber= obj.PhoneNumber;
                    applicationModel.Name=obj.CompanyName;
                    applicationModel.PasswordHash = _passwordHasher.HashPassword(applicationModel,obj.Password);
                    _db.MyApplicationUserRepo.AddItem(applicationModel);
            
                    _db.Save();
                    _userManager.AddToRoleAsync(applicationModel, StaticDetails.company).GetAwaiter().GetResult();

                    return RedirectToAction("Index");
                }
                var applicationUser = _db.MyApplicationUserRepo.GetSingleItem(u => u.CompanyId == obj.Id);
                var result = _passwordHasher.VerifyHashedPassword(applicationUser, applicationUser.PasswordHash, obj.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    _db.MyCompanyRepo.Update(obj);
                    _db.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Password", "The old password does not match.");
                    return View(obj);
                }
              
            }
            return View(obj);
            

        }
        public IActionResult Delete(int id)
        {
            CompanyModel obj = _db.MyCompanyRepo.GetSingleItem(u => u.Id == id);
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(CompanyModel obj)
        {
            ApplicationModel model = _db.MyApplicationUserRepo.GetSingleItem(u=>u.Email==obj.Email);
            _db.MyApplicationUserRepo.DeleteItem(model);
            _db.Save();
            _db.MyCompanyRepo.DeleteItem(obj);
            _db.Save();
            return RedirectToAction("Index");
        }
    }
}
