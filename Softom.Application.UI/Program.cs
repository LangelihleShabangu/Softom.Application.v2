using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.BusinessRules.Contract;
using Softom.Application.BusinessRules.Services.Implementation;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Infrustructure.Emails;
using Softom.Application.Infrustructure.Repository;
using Softom.Application.Models.Entities;
using Stripe;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Transient);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = "/Account/AccessDenied";
    option.LoginPath = "/Account/Login";
});

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequiredLength = 6;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IContactInformationService, ContactInformationService>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IBookingService, BookingService>();


builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IPaymentTypeService, PaymentTypeService>();
builder.Services.AddScoped<IPaymentStatusService, PaymentStatusService>();


builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAssociationService, AssociationService>();
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("Syncfusion:Licensekey").Get<string>());

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
SeedDatabase();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}