using System.Diagnostics;
using Gestão_de_Preço.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gestão_de_Preço.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MongoDbContext _mongoDbContext;

        public HomeController(ILogger<HomeController> logger, MongoDbContext mongoDbContext)
        {
            _logger = logger;
            _mongoDbContext = mongoDbContext;
        }

        public IActionResult Index()
        {
            var collection = _mongoDbContext.GetCollection<BsonDocument>("TestCollection");
            var count = collection.CountDocuments(FilterDefinition<BsonDocument>.Empty);
            ViewBag.MongoCount = count;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
