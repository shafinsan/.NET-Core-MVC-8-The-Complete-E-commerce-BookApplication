using EJApplication.ModelsLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.Data
{
    public class MyDbApplicationContext:IdentityDbContext
    {
        public MyDbApplicationContext(DbContextOptions<MyDbApplicationContext>options):base(options) { }

        public DbSet<ApplicationModel> MyApplication { get; set; }
        public DbSet<CompanyModel> MyCompany {  get; set; }
        public DbSet<EmployeeModel> MyEmployee { get; set; }
        public DbSet<CategoryModel>MyCategory { get; set; }
        public DbSet<BookModel>MyBook { get; set; }
        public DbSet<ShopingCartModel> MyShopingCard { get; set; }
        public DbSet<OrderHeader> MyOrderHead { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BookModel>().Property(b => b.ListPrice).HasPrecision(18, 2);
            builder.Entity<BookModel>().Property(b => b.Price).HasPrecision(18, 2);
            builder.Entity<BookModel>().Property(b => b.Price50).HasPrecision(18, 2);
            builder.Entity<BookModel>().Property(b => b.Price100).HasPrecision(18, 2);
            builder.Entity<CategoryModel>().HasData(
       new CategoryModel { Id = 1, Name = "Programming" },
       new CategoryModel { Id = 2, Name = "Web Development" },
       new CategoryModel { Id = 3, Name = "Data Science" },
       new CategoryModel { Id = 4, Name = "Software Engineering" },
       new CategoryModel { Id = 5, Name = "Cloud Computing" }
   );

            // Book seeding
            builder.Entity<BookModel>().HasData(
                new BookModel { Id = 1, Title = "The Art of Programming", ISBN = "978-0134000343", Description = "An extensive guide...", Author = "Donald Knuth", ListPrice = 79.99m, Price = 69.99m, Price50 = 64.99m, Price100 = 59.99m, CategoryId = 1, ImageUrl = "" },
                new BookModel { Id = 2, Title = "Mastering ASP.NET Core", ISBN = "978-1484231158", Description = "A comprehensive guide...", Author = "Ricardo Peres", ListPrice = 49.99m, Price = 44.99m, Price50 = 39.99m, Price100 = 34.99m, CategoryId = 2, ImageUrl = "" }
                // Add other books here
            );

        }
    }
}
