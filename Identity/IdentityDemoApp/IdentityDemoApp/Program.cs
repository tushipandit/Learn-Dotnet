using IdentityDemoApp.Data;
using IdentityDemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDBContext>(options=>options.UseSqlServer(
    builder.Configuration.GetConnectionString("DbConnection")
    ));

// implement authorization globally
builder.Services.AddMvc(options =>{
    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));

}).AddXmlSerializerFormatters();


builder.Services.ConfigureApplicationCookie(options => {
    options.AccessDeniedPath = new PathString("/Administation/AccessDenied");
});

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => {
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;

})
    .AddEntityFrameworkStores<ApplicationDBContext>();


//builder.Services.Configure<IdentityOptions>(options => {
//    options.Password.RequiredLength = 10;
//    options.Password.RequiredUniqueChars = 3;

//});

builder.Services.AddAuthorization(options => {

    options.AddPolicy("DeleteRolePolicy",
        policy=> policy.RequireClaim("Delete Role" , "true")
                        .RequireClaim("Create Role"));

    //options.AddPolicy("EditRolePolicy",
    //policy => policy.RequireClaim("Edit Role" ,"true" ));

    options.AddPolicy("EditRolePolicy", 
        policy => policy.RequireAssertion(context => 
            context.User.IsInRole("Admin") &&
            context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true" ) ||
            context.User.IsInRole("Super Admin")
        
        ));



    //Role is a type od claim in Role type
    options.AddPolicy("AdminRolePolicy",
        policy => policy.RequireRole("Admin, user"));
});




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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
