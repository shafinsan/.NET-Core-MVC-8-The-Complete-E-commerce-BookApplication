using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EJApplication.DataAccessLayer.Data;
using EJApplication.ModelsLayer.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using EJApplication.DataAccessLayer.DbInitializer.IDbInitializer;
using EJApplication.DataAccessLayer.DbInitializer;
using EJApplication.DataAccessLayer.Repository.IRepository;
using EJApplication.DataAccessLayer.Repository;
using EJApplication.ModelsLayer.Models;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDbApplicationContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => 
options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MyDbApplicationContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/LogOut";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnionOfWork,UnionOfWork>();
builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddScoped<IPasswordHasher<ApplicationModel>, PasswordHasher<ApplicationModel>>();
builder.Services.AddScoped<IDbInitalizer, DbInitializer>(); // Register DbInitializer

//builder.Services.AddScoped<IDbInitalizer, DbInitializer>();

//strip
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddSingleton<StripeClient>(serviceProvider =>
{
    var stripeSettings = serviceProvider.GetRequiredService<IOptions<StripeSettings>>().Value;
    return new StripeClient(stripeSettings.SecretKey);
});



var app = builder.Build();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
seedInitializer();
app.MapRazorPages();
//SeedInitalize();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void seedInitializer()
{
    using (var scoped = app.Services.CreateScope())
    {
        var Myinitializer = scoped.ServiceProvider.GetRequiredService<IDbInitalizer>();
        Myinitializer.seedInitializer();
    }
}
