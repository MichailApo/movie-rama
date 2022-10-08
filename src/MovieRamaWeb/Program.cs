using Microsoft.EntityFrameworkCore;
using MovieRamaWeb.Data;
using Infrastructure.Sql.Extensions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MovieRamaDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MovieRamaDbContextConnection' not found.");


builder.Services.AddSQLInfrastructure(builder.Configuration);

//since we use default UI
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MovieRamaDbContext>();


// Add services to the container.
builder.Services.AddRazorPages( options =>
{
    options.Conventions.AddPageRoute("/index", "/creator/{id}");

});
builder.Services.AddControllers();

var app = builder.Build();

app.Services.EnsureDatabaseCreated();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
