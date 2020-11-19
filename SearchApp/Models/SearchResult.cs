using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchApp.Models
{
    public class SearchResult
    {
        public int Id { get; set; }

        public string SearchingInput { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Snippet { get; set; }
    }
}
