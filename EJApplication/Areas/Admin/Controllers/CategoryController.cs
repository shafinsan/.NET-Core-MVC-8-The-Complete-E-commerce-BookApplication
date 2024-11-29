using EJApplication.DataAccessLayer.Repository;
using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Drawing.Printing;

namespace EJApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.admin)]
    public class CategoryController : Controller
    {
        private readonly IUnionOfWork _db;
        public CategoryController(IUnionOfWork db)
        {
            _db = db;
        }

        public IActionResult Index(string ? SearchString,string? sort, int pageNumber=1, int pageSize= 5)
        {
            pageNumber = (pageNumber == null || pageNumber == 0) ? 1 : pageNumber;
            Console.WriteLine(pageNumber);
            List<CategoryModel> list = _db.MyCategoryRepo.GetAllItems().ToList();
            Console.WriteLine(sort);
            if (!string.IsNullOrEmpty(SearchString))
            {
                list = list.Where(u=>u.Name.Contains(SearchString,StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["sort"] = sort == "asc" ? "dsc" : "asc";
            if (!string.IsNullOrEmpty(sort))
            {
                list = sort == "asc" ? list.OrderBy(x => x.Name).ToList() : list.OrderByDescending(x => x.Name).ToList();
            }

            int total = list.Count;
            int totalPage = (int)Math.Ceiling((double)total / pageSize);

            ViewData["totalPage"] = totalPage;
            ViewData["pageNumber"] = pageNumber;
            list = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return View(list);
        }
        public IActionResult Upsert(int ?id)
        {
            if (ModelState.IsValid)
            {
                if(id!=null && id != 0)
                {
                    //update
                    CategoryModel value = _db.MyCategoryRepo.GetSingleItem(u=>u.Id==id);
                    return View(value); 
                }
                else
                {
                    //add new item
                    return View(new CategoryModel());
                }
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Upsert(CategoryModel obj)
        {
            if (ModelState.IsValid) 
            {
                if (obj.Id == 0)
                {
                    TempData["success"] = "Add Successfull";
                    _db.MyCategoryRepo.AddItem(obj);
                }
                else
                {
                    TempData["success"] = "Update Successfull";
                    _db.MyCategoryRepo.Update(obj);
                }
                _db.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Upsert");
        }
        public IActionResult Delete(int? id)
        {
            if (id != 0)
            {
                CategoryModel item = _db.MyCategoryRepo.GetSingleItem(u => u.Id == id);
                return View(item);
            }
            return View("Delete");
        }
        [HttpPost]
        public IActionResult Delete(CategoryModel obj)
        {
            if (obj.Id != 0)
            {
                CategoryModel item = _db.MyCategoryRepo.GetSingleItem(u=>u.Id==obj.Id);
                _db.MyCategoryRepo.DeleteItem(item);
                _db.Save();
                TempData["success"] = "Delete Successfull";
                return RedirectToAction("Index");
            }
            return View("Delete");
        }
    }
}
