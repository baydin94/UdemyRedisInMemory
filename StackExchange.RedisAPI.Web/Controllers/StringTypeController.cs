using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.RedisAPI.Web.Services;

namespace StackExchange.RedisAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Set()
        {
            var db = _redisService.GetDb(0);
            db.StringSet("name", "Baransel Aydın");
            db.StringSet("ziyaretçi", 100);

            return View();
        }

        public IActionResult Get()
        {
            var db = _redisService.GetDb(0);
            //var values = db.StringGet("name");

            //var values = db.StringGetRange("name", 0, -1);
            //var values = db.StringGetRange("name", 0, 4);

            var values = db.StringLength("name");

            //if(values.HasValue)
            //    ViewBag.Name = values;

            ViewBag.Name = values;

            return View();
        }

        public IActionResult Incremant()
        {
            var db = _redisService.GetDb(0);
            db.StringIncrement("ziyaretçi", 1);

            return View();
        }

        public IActionResult Decremant()
        {
            var db = _redisService.GetDb(0);
            db.StringDecrement("ziyaretçi", 5);
            return View();
        }
    }
}
