using EJApplication.DataAccessLayer.Data;
using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Repository
{
    public class BookRepo : Repository<BookModel>, IBook
    {
        private readonly MyDbApplicationContext _db;   
        public BookRepo(MyDbApplicationContext db):base(db)
        {
            _db = db;
        }
        public void Update(BookModel obj)
        {
            BookModel temp = new BookModel();
            temp.Id = obj.Id;
            temp.ISBN = obj.ISBN;
            temp.Author = obj.Author;
            temp.Description = obj.Description;
            temp.Title = obj.Title;
            temp.Category = obj.Category;
            temp.CategoryId = obj.CategoryId;
            temp.ListPrice = obj.ListPrice;
            temp.Price = obj.Price;
            temp.Price50 = obj.Price50;
            temp.Price100 = obj.Price100;
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                temp.ImageUrl = obj.ImageUrl;
            }
            else
            {
                temp.ImageUrl = "/Image/Book/noImage.jpg";
            }
            _db.MyBook.Update(temp);
        }
    }
}
