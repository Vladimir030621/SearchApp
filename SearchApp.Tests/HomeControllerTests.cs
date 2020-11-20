using Microsoft.AspNetCore.Mvc;
using Moq;
using SearchApp.Controllers;
using SearchApp.Domain;
using SearchApp.Domain.Interfaces;
using SearchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SearchApp.Tests
{
    public class HomeControllerTests
    {
        private IEnumerable<SearchResult> GetTestResults()
        {
            var searchResults = new List<SearchResult>
            {
                new SearchResult {SearchingInput = "Link1", Title = "Link1", Link = "https://www.google.com/link1", Snippet = "SnippetAboutLink1"},
                new SearchResult {SearchingInput = "Link2", Title = "Link2", Link = "https://www.google.com/link2", Snippet = "SnippetAboutLink2"},
                new SearchResult {SearchingInput = "Link3", Title = "Link3", Link = "https://www.google.com/link3", Snippet = "SnippetAboutLink3"},
                new SearchResult {SearchingInput = "Link4", Title = "Link4", Link = "https://www.google.com/link4", Snippet = "SnippetAboutLink4"},
                new SearchResult {SearchingInput = "Link5", Title = "Link5", Link = "https://www.google.com/link5", Snippet = "SnippetAboutLink5"},
                new SearchResult {SearchingInput = "Link6", Title = "Link6", Link = "https://www.google.com/link6", Snippet = "SnippetAboutLink6"}
            };
            return searchResults;
        }

        [Fact]
        public void IndexReturnsView()
        {
            var mock = new Mock<ISearchResultRepository>();
            mock.Setup(repo => repo.GetSearchResults()).Returns(GetTestResults());
            HomeController controller = new HomeController(mock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void ShowResultsReturnsDataFromDB()
        {
            var mock = new Mock<ISearchResultRepository>();
            mock.Setup(repo => repo.GetSearchResults()).Returns(GetTestResults());
            HomeController controller = new HomeController(mock.Object);
            string inputString = "Link1";
            var list = GetTestResults().Where(s => s.SearchingInput == inputString).ToList();

            var result = controller.ShowResults(inputString);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<SearchResult>>(viewResult.ViewData.Model);         
            Assert.Equal(list.Count(), model.Count());
        }

        [Fact]
        public void ShowResultsReturnsNewSearchedData()
        {
            int numberOfSelectedResults = 10;
            var mock = new Mock<ISearchResultRepository>();
            mock.Setup(repo => repo.GetSearchResults()).Returns(GetTestResults());
            HomeController controller = new HomeController(mock.Object);

            var result = controller.ShowResults("newInput");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<SearchResult>>(viewResult.ViewData.Model);
            Assert.Equal(numberOfSelectedResults, model.Count());

            foreach(var item in model)
            {
                mock.Verify(r => r.SaveSearchResult(item));
            }          
        }
    }
}
