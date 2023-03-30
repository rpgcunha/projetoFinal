using apoio_decisao_medica.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSession();


//escolhe a string connection a usar
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(
                        builder.Configuration.GetConnectionString("LOCAL2")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
