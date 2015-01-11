using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using Examine.SearchCriteria;
using Mikkelhm.Core.Extensions;
using Umbraco.Core.Models;

namespace Mikkelhm.Core.Search
{
    public class SearchHelper
    {
        public static SearchResult Search(string q, string category, string tag)
        {

            if (String.IsNullOrWhiteSpace(q))
            {
                q = "";
            }
            string searchTerm = q;
            var helper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
            var searcher = ExamineManager.Instance;
            var searchCriteria = searcher.CreateSearchCriteria();
            IEnumerable<IPublishedContent> results;
            ISearchCriteria query;
            // show me something with category, if not null
            if (!string.IsNullOrEmpty(category))
            {
                // this search is SQL
                results = helper.TagQuery.GetContentByTag(category.FromFreindlyUrl(), "General");
                searchTerm = category.FromFreindlyUrl();
            }
            // show me errors, if not null
            else if (!string.IsNullOrEmpty(tag))
            {
                // this search is SQL
                results = helper.TagQuery.GetContentByTag(tag.FromFreindlyUrl(), "Loosly");
                searchTerm = tag.FromFreindlyUrl();
            }
            // finally search everything
            else
            {
                query = searchCriteria
                    .GroupedOr(new[] { "nodeName", "content", "listViewShortText", "metaTitle", "generalCategoriesTag", "looslyDefinedCategoriesTag" }, q.MultipleCharacterWildcard())
                    .Compile();
                // this is using Lucene(Examine)
                results = helper.TypedSearch(query);
            }

            return new SearchResult()
            {
                Results = results,
                TotalResults = results.Count(),
                SearchTerm = searchTerm
            };
        }

    }
}
