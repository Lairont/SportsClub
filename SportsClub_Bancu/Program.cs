using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SporClub_Bancu.DAL;
using SportClub_Bancu;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
{
    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
})

.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.SaveTokens = true;

    options.Events.OnCreatingTicket = async context =>
    {
        Console.WriteLine(">>>> ¿¬“Œ–»«¿÷»ﬂ œŒÿÀ¿");

        if (context.User.TryGetProperty("picture", out var pictureUrl))
        {
            var userId = context.Principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown";
            var fileName = $"avatar_{userId}.jpg";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            try
            {
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(pictureUrl.ToString());
                await File.WriteAllBytesAsync(filePath, bytes);

                context.Identity.AddClaim(new System.Security.Claims.Claim("local_pic", $"/images/{fileName}"));
                Console.WriteLine(">>>>  ¿–“»Õ ¿ —Œ’–¿Õ≈Õ¿: " + filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>>> Œÿ»¡ ¿ — ¿◊»¬¿Õ»ﬂ: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine(">>>> √”√À Õ≈ œ–»—À¿À œŒÀ≈ 'PICTURE'!");
        }
    };
});

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SiteInformation}/{id?}");

app.Run();
