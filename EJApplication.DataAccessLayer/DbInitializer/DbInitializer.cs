using EJApplication.DataAccessLayer.Data;
using EJApplication.DataAccessLayer.DbInitializer.IDbInitializer;
using EJApplication.ModelsLayer.Models;
using EJApplication.ModelsLayer.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.DataAccessLayer.DbInitializer
{
    public class DbInitializer : IDbInitalizer
    {
        private readonly MyDbApplicationContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(MyDbApplicationContext db, 
            UserManager<IdentityUser>userManager, RoleManager<IdentityRole>roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void seedInitializer()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }
            if (!_roleManager.RoleExistsAsync(StaticDetails.client).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.client)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationModel
                {
                    UserName="eliasjabershafin599@gmail.com",
                    Email= "eliasjabershafin599@gmail.com",
                    PhoneNumber="01670319232",
                    Name="Elias Jaber",
                },"Elias123@").GetAwaiter().GetResult();
                ApplicationModel user=_db.MyApplication.FirstOrDefault(u=>u.Email== "eliasjabershafin599@gmail.com");
           
                user.EmailConfirmed = true;
                _db.SaveChanges();
                _userManager.AddToRoleAsync(user, StaticDetails.admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
