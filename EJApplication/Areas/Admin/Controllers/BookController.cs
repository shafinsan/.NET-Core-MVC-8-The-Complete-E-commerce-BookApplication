using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using EJApplication.ModelsLayer.Utility.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;

namespace EJApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =StaticDetails.admin)]
    public class BookController : Controller
    {
        private readonly IUnionOfWork _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IUnionOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db=db;
            _webHostEnvironment=webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<BookModel> list = _db.MyBookRepo.GetAllItems(includeValue: "Category").ToList();
            return View(list);
        }
       public IActionResult Upsert(int? id)
        {
            BookCategoryWrapper bookCategoryWrapper = new();
            if (id == 0 || id == null){
                //add new books
                
                bookCategoryWrapper=new()
                {
                    BookWrapper = new BookModel(),
                    CategoryList = _db.MyCategoryRepo.GetAllItems().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })
                };
                return View(bookCategoryWrapper);

            }
            //update books
            bookCategoryWrapper = new()
            {
                BookWrapper = _db.MyBookRepo.GetSingleItem(u=>u.Id==id),
                CategoryList = _db.MyCategoryRepo.GetAllItems().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(bookCategoryWrapper);
        }
        [HttpPost]
        public IActionResult Upsert(BookCategoryWrapper obj,IFormFile? file)
        {

            if (string.IsNullOrEmpty(obj.BookWrapper.ImageUrl))
            {
                if (!string.IsNullOrEmpty(file.FileName) && file != null)
                {
                    Random ran = new Random();
                    var name = ran.Next();
                    string globalPath = "Image/Book";
                    var www = _webHostEnvironment.WebRootPath;
                    var getExtention = Path.GetExtension(file.FileName);
                    var newFileName = name + getExtention;
                    var total_path = Path.Combine(www, globalPath);
                    var FinalPath = Path.Combine(total_path, newFileName);
                    using (var createFile = new FileStream(FinalPath, FileMode.Create))
                    {
                        file.CopyTo(createFile);
                    }
                    obj.BookWrapper.ImageUrl = $"/Image/Book/{newFileName}";
                }
            }
            else {
                if (!string.IsNullOrEmpty(file?.FileName) && file != null)
                {
                    var oldImagePath = _webHostEnvironment.WebRootPath + obj.BookWrapper.ImageUrl;

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                    Random ran = new Random();
                    var name = ran.Next();
                    string globalPath = "Image/Book";
                    var www = _webHostEnvironment.WebRootPath;
                    var getExtention = Path.GetExtension(file.FileName);
                    var newFileName = name + getExtention;
                    var total_path = Path.Combine(www, globalPath);
                    var FinalPath = Path.Combine(total_path, newFileName);
                    using (var createFile = new FileStream(FinalPath, FileMode.Create))
                    {
                        file.CopyTo(createFile);
                    }
                    obj.BookWrapper.ImageUrl = $"/Image/Book/{newFileName}";
                }
            }
            if (ModelState.IsValid)
            {
                if (obj.BookWrapper.Id == 0)
                {
                    _db.MyBookRepo.AddItem(obj.BookWrapper);
                    _db.Save();
                    return RedirectToAction("Index");
                }

                // update book logic
                _db.MyBookRepo.Update(obj.BookWrapper);
                _db.Save();
                return RedirectToAction("Index");
            }

            // Reload CategoryList if ModelState is invalid
            obj.CategoryList = _db.MyCategoryRepo.GetAllItems().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);

        }
        public IActionResult Delete(int? id)
        {
            BookModel Book = _db.MyBookRepo.GetSingleItem(u => u.Id == id,includeValue: "Category");
            return View(Book);
        }
        [HttpPost]
        public IActionResult Delete(BookModel obj)
        {
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var oldImagePath = _webHostEnvironment.WebRootPath + obj.ImageUrl;

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _db.MyBookRepo.DeleteItem(obj);
                _db.Save();
                return RedirectToAction("Index");
            }
            _db.MyBookRepo.DeleteItem(obj);
            _db.Save();
            return RedirectToAction("Index");
        }
    }
}
