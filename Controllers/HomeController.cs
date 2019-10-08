using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordPredictor.Models;
using Newtonsoft.Json;

namespace WordPredictor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/api/{snippet}")]
        [HttpGet]
        public ActionResult GetSnippetResponses(string snippet)
        {
            var Collection = new Dictionary<string, List<string>>();

            //From SQLite DB
            List<string> DictionaryMatch = SQLiteDb.GetDictionaryMatches(snippet);
            Collection.Add("CustomDict", DictionaryMatch);

            //From word prediction website
            List<string> WordPredictMatches = WordPrediction.GetWordPredictMatches(snippet);
            Collection.Add("PreDict", WordPredictMatches);

            var jsonSerialiser = new JsonSerializer();
            var json = JsonConvert.SerializeObject(Collection, Formatting.Indented);
            return Ok(json);
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
