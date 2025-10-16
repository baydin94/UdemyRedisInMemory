using Microsoft.AspNetCore.Mvc;
using StackExchange.RedisAPI.Web.Services;

namespace StackExchange.RedisAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private string listKey = "hostnames";
        public IActionResult Index()
        {
            var db = _redisService.GetDb(2);
            HashSet<string> namesList = new HashSet<string>();

            if (db.KeyExists(listKey))
                db.SetMembers(listKey).ToList().ForEach(x => namesList.Add(x.ToString()));

            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            var db = _redisService.GetDb(2);

            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            db.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }




    }
}
