using SearchApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchApp.Domain
{
    public class DataManager
    {
        public ISearchResultRepository SearchResults { get; set; }

        public DataManager(ISearchResultRepository searchResultRepository)
        {
            SearchResults = searchResultRepository;
        }
    }
}
