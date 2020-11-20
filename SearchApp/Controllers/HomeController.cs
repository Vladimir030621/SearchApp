using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SearchApp.Domain;
using SearchApp.Domain.Interfaces;
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

        //private readonly DataManager dataManager;
        private readonly ISearchResultRepository context;

        public HomeController(ISearchResultRepository searchResultRepository)
        {
            context = searchResultRepository;
        }

        public IActionResult Index()
        {
            return View("Index");
        }


        [HttpPost]
        public IActionResult ShowResults(string searchingInput)
        {
            List<SearchResult> results = new List<SearchResult>();

            if (string.IsNullOrWhiteSpace(searchingInput))
            {
                throw new ArgumentNullException("SearchInput is null", nameof(searchingInput));
            }

            string searchQuery = searchingInput;

            if (context.GetSearchResults().Any(s => s.SearchingInput == searchingInput))
            {
                results = context.GetSearchResults().Where(s => s.SearchingInput == searchingInput).ToList();
            }
            else
            {
                var request = WebRequest.Create("https://www.googleapis.com/customsearch/v1?key=" + API_KEY + "&cx=" + SEARCH_ENGINE_ID_CX + "&q=" + searchQuery);

                results = RequestResultsDeserialize(request, searchingInput);

                int numberOfSelectedResults = 10;
                for (int i = 0; i < numberOfSelectedResults; i++)
                {
                    context.SaveSearchResult(results[i]);
                }
            }

            return View(results);
        }


        private List<SearchResult> RequestResultsDeserialize(WebRequest request, string searchingInput)
        {
            List<SearchResult> results = new List<SearchResult>();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseString = reader.ReadToEnd();
            dynamic jsonData = JsonConvert.DeserializeObject(responseString);

            foreach(var item in jsonData.items)
            {
                results.Add(new SearchResult
                {
                    Title = item.title,
                    Link = item.link,
                    Snippet = item.snippet,
                    SearchingInput = searchingInput
                });
            }

            return results;
        }
    }
}
