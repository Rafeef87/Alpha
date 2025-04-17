﻿using Business.Services;
using Data.Context;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDB")));

builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IStatusService, StatusService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/SignIn";
    options.AccessDeniedPath = "/auth/denied";
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

/* OBS: Jag har kommentera External Authentication eftersom jag har haft problem med github
 * men jag har provat linken och dem funger som vanlig */

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
// .AddCookie()
// .AddGoogle(options =>
// {
//     options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
//     options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
// });
//.AddMicrosoftAccount(microsoftOptions =>
//{
//    microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
//    microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
//});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };

    foreach(var roleName in roleNames)
    {
        var exists = await roleManager.RoleExistsAsync(roleName);
        if(!exists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    //var user = new IdentityUser { UserName = "admin@domin.com", Email = "admin@domin.com" };

    //var userExists = await userManager.Users.AnyAsync(x => x.Email == user.Email);
    //if (!userExists)
    //{
    //    var result = await userManager.CreateAsync(user, "BytMig123");
    //    if (result.Succeeded)
    //        await userManager.AddToRoleAsync(user, "Administrator");
    //}
}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();


app.Run();
