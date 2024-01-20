
using Ekad.Voucher.domain.context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Ekad.Voucher.logic.Repository.UnitofWork;
using Ekad.Voucher.logic.Service.Interfaces;
using Ekad.Voucher.logic.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var cultures = new[]
{
                new CultureInfo("ar-EG"),
                new CultureInfo("en"),
            };
var cultureInfo = new CultureInfo("ar-EG") { NumberFormat = NumberFormatInfo.InvariantInfo };
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
    options.DefaultRequestCulture = new RequestCulture("ar-EG");
    options.AddSupportedUICultures("ar-EG");
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<ILoginHistoryService, LoginHistoryService>();
builder.Services.AddScoped<IUserNotificationService, UserNotificationService>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IInqueryService, InqueryService>();
builder.Services.AddScoped<ITblServiceService, TblServiceService>();
//
builder.Services.AddScoped<ITblYearDayService, TblYearDayService>();
//

//
builder.Services.AddScoped <ITblAdvertisementTypeYearDayService ,TblAdvertisementTypeYearDayService>();
//
builder.Services.AddDbContext<EkadVoucherDbContext>(options =>
 options.UseSqlServer(
 builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeFolder("/");
        options.Conventions.AllowAnonymousToPage("/Account/login");
    })
    .AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization().AddMvcLocalization();

builder.Services.AddSingleton<HtmlEncoder>(
  HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                            UnicodeRanges.Arabic }));

builder.Services.AddAuthentication(options =>
{

    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddLocalization();
builder.Services.AddSession();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy =>
    {
        policy.RequireRole("SuperAdmin");
        policy.RequireClaim(ClaimTypes.Role, "SuperAdmin");
    });
    options.AddPolicy("SectorManager", policy =>
    {
        policy.RequireRole("SectorManager");
        policy.RequireClaim(ClaimTypes.Role, "SectorManager");
    });
    options.AddPolicy("CompaneManager", policy =>
    {
        policy.RequireRole("CompaneManager");
        policy.RequireClaim(ClaimTypes.Role, "CompaneManager");
    });
    options.AddPolicy("CompaneRep", policy =>
    {
        policy.RequireRole("CompaneRep");
        policy.RequireClaim(ClaimTypes.Role, "CompaneRep");
    });
    options.AddPolicy("CompaneEmp", policy =>
    {
        policy.RequireRole("CompaneEmp");
        policy.RequireClaim(ClaimTypes.Role, "CompaneEmp");
    });
    options.AddPolicy("ProviderAdmin", policy =>
    {
        policy.RequireRole("ProviderAdmin");
        policy.RequireClaim(ClaimTypes.Role, "ProviderAdmin");
    });
    options.AddPolicy("ProviderEmp", policy =>
    {
        policy.RequireRole("ProviderEmp");
        policy.RequireClaim(ClaimTypes.Role, "ProviderEmp");
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<EkadVoucherDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.Replace(new ServiceDescriptor(
//        serviceType: typeof(IPasswordHasher<ApplicationUser>),
//        implementationType: typeof(Md5PasswordHasher<ApplicationUser>),
//        ServiceLifetime.Scoped));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 3;
});
builder.Services.ConfigureApplicationCookie(options =>
{

    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            var requestPath = ctx.Request.Path;
            var query = ctx.Request.QueryString;
            ctx.Response.Redirect("/Account/Login?ReturnUrl=" + WebUtility.UrlEncode(requestPath + (!query.HasValue ? "" : query.Value)));

            return System.Threading.Tasks.Task.CompletedTask;
        },
        OnRedirectToAccessDenied = ctx =>
        {
            var requestPath = ctx.Request.Path;
            var query = ctx.Request.QueryString;
            ctx.Response.Redirect("/Account/Login?ReturnUrl=" + WebUtility.UrlEncode(requestPath + (!query.HasValue ? "" : query.Value)));
            return System.Threading.Tasks.Task.CompletedTask;
        }

    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//builder.Services.AddDbContext<akmContext>(options =>
// options.UseSqlServer(
// builder.Configuration.GetConnectionString("DefaultConnection")));


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(builder.Environment.ContentRootPath, "assets")),
    RequestPath = "/assets"
});

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

var supportedCultures = new[] { "en-US", "ar-EG" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

