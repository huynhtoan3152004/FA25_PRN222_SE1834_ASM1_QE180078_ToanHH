using aDealerEDVMS.RazorWebApps.ToanHH.Hubs;
using aDealerEDVMS.Service.ToanHH;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
//builder.Services.AddDbContext<aDealerEDVMS.Repository.ToanHH.DBcontext.FA25_PRN221_SE1834_G5_EVDMSContext>();
builder.Services.AddScoped<IDealerHhtService, DearlerHhtService>();
builder.Services.AddScoped<DealerContractsHhtService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Account/Forbidden";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages().RequireAuthorization(); ///cau hinh authorization
app.MapHub<aDealer>("/DealerHub");
app.Run();
