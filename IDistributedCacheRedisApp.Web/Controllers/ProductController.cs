using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        //public async Task<IActionResult> Index()
        //{
        //    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
        //    {
        //        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1), // Mutlaka 1 dakika sonra silinecek
        //        //SlidingExpiration = TimeSpan.FromSeconds(5),
        //    };

        //    _distributedCache.SetString("name", "Baran", options);
        //    await _distributedCache.SetStringAsync("surname", "Aydın", options);

        //    return View();
        //}

        public async Task<IActionResult> Index()
        {

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            List<Product> products = new List<Product>
            {
                new Product {Id = 1, Name = "Kalem", Price = 100 },
                new Product {Id = 2, Name = "Silgi", Price = 150 },
                new Product {Id = 3, Name = "Defter", Price = 120 },
            };

            Product product = new Product { Id = 1, Name = "Tükenmez Kalem", Price = 200 };

            string jsonProducts = JsonConvert.SerializeObject(products);
            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetStringAsync("product:1", jsonProducts, options);
            await _distributedCache.SetStringAsync("product:2", jsonProduct, options);

            _distributedCache.Set("product:3", byteProduct, options);

            return View();
        }

        public IActionResult Show()
        {
            //ViewBag.name = _distributedCache.GetString("name");

            string jsonProducts = _distributedCache.GetString("product:1");
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonProducts);

            string jsonProduct = _distributedCache.GetString("product:2");

            Byte[] byteProduct = _distributedCache.Get("product:3");
            Product productByte = JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(byteProduct)); 

            ViewBag.jsonProduct = jsonProduct;
            ViewBag.productByte = productByte;

            return View(products);
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", "download.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image",imageByte);            

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("image");

            return File(imageByte, "image/jpg");
        }
    }
}
