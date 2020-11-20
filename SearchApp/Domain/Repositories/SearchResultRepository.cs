using SearchApp.Domain.Interfaces;
using SearchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchApp.Domain.Repositories
{
    public class SearchResultRepository : ISearchResultRepository
    {
        private readonly AppDbContext context;

        public SearchResultRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<SearchResult> GetSearchResults()
        {
            return context.SearchResults;
        }

        public void SaveSearchResult(SearchResult searchResult)
        {
            context.SearchResults.Add(searchResult);
            context.SaveChanges();
        }
    }
}
