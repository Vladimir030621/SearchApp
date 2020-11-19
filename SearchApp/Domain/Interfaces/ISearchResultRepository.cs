﻿using SearchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchApp.Domain.Interfaces
{
    public interface ISearchResultRepository
    {
        IQueryable<SearchResult> GetSearchResults();

        void SaveSearchResult(SearchResult searchResult);
    }
}
