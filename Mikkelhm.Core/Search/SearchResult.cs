using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Mikkelhm.Core.Search
{
    public class SearchResult
    {
        public IEnumerable<IPublishedContent> Results { get; set; }
        public string SearchTerm { get; set; }
        public int TotalResults { get; set; }
        public SearchResult()
        {

        }
    }
}
