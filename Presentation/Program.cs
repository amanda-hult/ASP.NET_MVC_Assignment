using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Presentation.Handlers;
using Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
var containerName = "images";
builder.Services.AddScoped<IFileHandler>(_ => new AzureFileHandler(connectionString, containerName));

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<INotificationDismissedRepository, NotificationDismissedRepository>();



builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
    {
        x.Password.RequiredLength = 8;
        x.User.RequireUniqueEmail = true;
        x.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(x =>
    {
        x.LoginPath = "/auth/signin";
        x.LogoutPath = "/auth/signout";
        x.AccessDeniedPath = "/auth/denied";
        x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        x.SlidingExpiration = true;
        x.Cookie.HttpOnly = true;
        x.Cookie.SameSite = SameSiteMode.None;
        x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    var user = new UserEntity { UserName = "admin@domain.com", Email = "admin@domain.com", FirstName = "System", LastName = "Administrator", EmailConfirmed = true };

    var roleNames = new[] { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var userExists = await userManager.Users.AnyAsync(x => x.Email == user.Email);
    if (!userExists)
    {
        var result = await userManager.CreateAsync(user, "Admin@123");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignUp}/{id?}")
    .WithStaticAssets();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
