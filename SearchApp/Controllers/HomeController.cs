using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchApp.Domain;
using SearchApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SearchApp.Controllers
{
    public class HomeController : Controller
    {
        private const string SEARCH_ENGINE_ID_CX = "41743ddc9baced361";
        private const string API_KEY = "AIzaSyBnMYeWT5A4RE3TQ_DRsCBhkzPirAj-vUI";

        private DataManager dataManager;

        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShowResults(string search)
        {
            string searchQuery = search;
            var results = new List<SearchResult>();
            int numberOfSelectedResults = 10;

            if (dataManager.SearchResults.GetSearchResults().Any(s => s.SearchingInput == search))
            {
                results = dataManager.SearchResults.GetSearchResults().Where(s => s.SearchingInput == search).ToList();
            }
            else
            {
                var request = WebRequest.Create("https://www.googleapis.com/customsearch/v1?key=" + API_KEY + "&cx=" + SEARCH_ENGINE_ID_CX + "&q=" + searchQuery);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                string responseString = reader.ReadToEnd();
                dynamic jsonData = JsonConvert.DeserializeObject(responseString);

                for(int i = 0; i < numberOfSelectedResults; i++)
                {
                    SearchResult currentResult = new SearchResult
                    {
                        Title = jsonData.items[i].title,
                        Link = jsonData.items[i].link,
                        Snippet = jsonData.items[i].snippet,
                        SearchingInput = search
                    };

                    results.Add(currentResult);

                    dataManager.SearchResults.SaveSearchResult(currentResult);
                }
            }

            return View(results);
        }
    }
}
