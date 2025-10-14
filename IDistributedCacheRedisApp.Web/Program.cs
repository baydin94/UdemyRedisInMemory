 var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddStackExchangeRedisCache(options =>
{
    //options.Configuration = "localhost:6379"; // API ve Docker Redis ayný makinadaysa doðru
    options.Configuration = "localhost:6380"; // my-redis-6380 olduðu için container 

    //options.Configuration = "my-redis-6380:6379"; // Eðer Api ve Docker Redis farklý containerlardaysa ismiyle tanýtmak zorundasýn, yani API'yi de containera koydun bunu kullanacaksýn
    //options.Configuration = "127.0.0.1:6380"; // Docker Redis (farklý port)
    //options.Configuration = "password@redis-server:6379"; // Þifreli baðlantý
    //options.Configuration = "redis-10481.c270.us-east-1-3.ec2.cloud.redislabs.com:10481,password=xyz"; // Redis Cloud
    //options.Configuration = "redis-10481.c270.us-east-1-3.ec2.cloud.redislabs.com:10481,password=xyz,ssl=True,abortConnect=False"; // Redis Cloud SSL

    ////Production ortamýnda ise
    //options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    //options.InstanceName = "DieticianProject_";

    //appsettings.json içerði de aþaðýdaki gibi olmalý
    //{
    //    "ConnectionStrings": {
    //        "RedisConnection": "localhost:6379"
    //    }
    //}
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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
