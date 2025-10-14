using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            // _memoryCache.Set<string>("zaman", DateTime.Now.ToString());

            // 1. Yöntem

            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());


            // 2. Yöntem

            //if (!_memoryCache.TryGetValue("zaman", out string zamanCache))
            //{
            //    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //    MemoryCacheEntryOptions options = new()
            //    {
            //        //AbsoluteExpiration = DateTime.Now.AddSeconds(10) // Kesin zamanlı

            //        AbsoluteExpiration = DateTime.Now.AddMinutes(1),
            //        SlidingExpiration = TimeSpan.FromSeconds(5) // Son erişimden itibaren

            //    };

            //    MemoryCacheEntryOptions options = new()
            //    {
            //        Size = 1024, // Varsayılan olarak boyut sınırlaması yoktur. Boyut sınırlaması yapılacaksa, MemoryCacheOptions ile boyut sınırı belirtilmelidir.
            //        //AbsoluteExpiration = DateTime.Now.AddMinutes(1),
            //        AbsoluteExpiration = DateTime.Now.AddMinutes(10),
            //        //SlidingExpiration = TimeSpan.FromSeconds(5), // Son erişimden itibaren
            //        //Priority = CacheItemPriority.High // Bu data benim için önemli, bellek dolduğunda öncelikli olarak silinmesin.
            //        //Priority = CacheItemPriority.NeverRemove // Memory dolsa dahi, bu data kesinlikle silinmesin. ANCAK! Memory dolarsa yeni data eklenemez ve Exception fırlatılır. Eğer hepsini NeverRemove yaparsan.
            //        //Priority = CacheItemPriority.Low // Bu data öncelikli olarak silinebilir. 
            //        //Priority = CacheItemPriority.Normal // Varsayılan değer
            //        Priority = CacheItemPriority.High,

            //    };

            //    options.RegisterPostEvictionCallback((key, value, reason, state) =>
            //    {
            //        _memoryCache.Set<string>("callback", $"{key} -> {value} => sebep: {reason}");
            //    });

            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            //}

            //zamanCache = _memoryCache.Get<string>("zaman");

            var options = new MemoryCacheEntryOptions{
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                Priority = CacheItemPriority.High,
            };

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"{key} -> {value} => sebep: {reason}");
            });

            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };

            _memoryCache.Set<Product>("product:1", product, options); //Kendisi otomatik bir şekilde serialize eder. Ama redis tarafında serialize işlemlerini kendimiz yapıyor olacağız.

            return View();
        }

        public IActionResult ShowTime()
        {

            //Remove methodu
            //_memoryCache.Remove("zaman");

            //GetOrCreate methodu
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString())
            //);

            //ViewBag.zaman = _memoryCache.Get<string>("zaman");

            //_memoryCache.TryGetValue("zaman", out string? zamanCache);
            //_memoryCache.TryGetValue("callback", out string? callback);
            
            //ViewBag.zaman = zamanCache;
            //ViewBag.callback = callback;

            //ViewBag.product = _memoryCache.TryGetValue("product:1", out Product? product) ? product : null;

            return View();
        }

        public IActionResult ShowProduct()
        {
            return View();
        }
    }
}
