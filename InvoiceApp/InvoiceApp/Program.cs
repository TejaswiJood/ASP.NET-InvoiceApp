// TEJASWI SINGH JOOD
// 8845744

using Microsoft.EntityFrameworkCore;
using Invoicing.DataAccess.Entities;
using Invoicing.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<InvoicingDbContext>(options => options.UseSqlServer(connectionString));

// in console => Add-Migration Initial -StartupProject InvoiceApp -Project Invoicing.DataAccess

builder.Services.AddScoped<IInvoicingService, InvoicingService>();


var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
