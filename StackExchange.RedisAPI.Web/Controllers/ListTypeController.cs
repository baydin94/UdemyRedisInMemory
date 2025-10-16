using Microsoft.AspNetCore.Mvc;
using StackExchange.RedisAPI.Web.Services;

namespace StackExchange.RedisAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDb(1);

            List<string> names = new List<string>();
            
            if(db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    names.Add(x.ToString());
                });
            }

            return View(names);
        }

        [HttpPost]
        public IActionResult AddList(string name)
        {
            var db = _redisService.GetDb(1);

            //db.ListRightPush(listKey, name);
            db.ListLeftPush(listKey, name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveItem(string name)
        {
            var db = _redisService.GetDb(1);
            await db.ListRemoveAsync(listKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFirstItem()
        {
            var db = _redisService.GetDb(1);

            //db.ListLeftPop(listKey);
            db.ListRightPop(listKey);

            return RedirectToAction("Index");
        }

        private string listKey = "names";
    }
}
