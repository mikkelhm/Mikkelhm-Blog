using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Examine;
using Examine.LuceneEngine.SearchCriteria;
using Mikkelhm.Core.Extensions;
using umbraco;
using umbraco.interfaces;

namespace Mikkelhm.Core.Handlers
{
    public class LegacyRouteHandler : INotFoundHandler
    {
        public bool Execute(string url)
        {
            // its from the old BlogEngine.NET blog.
            var lowerUrl = url.ToLower();
            var regexPattern = @"(?<oldurl>post\/[0-9]{4}\/[0-9]{2}\/[0-9]{2}\/.*\.aspx)";
            var match = Regex.Match(lowerUrl, regexPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {


                var oldUrl = match.Groups["oldurl"].Value;
                var searcher = ExamineManager.Instance;
                var criteria = searcher.CreateSearchCriteria();
                criteria.Field("legacyPostUrl", oldUrl);
                var result = searcher.Search(criteria);
                if (result.TotalItemCount == 1)
                {
                    var redirectId = result.FirstOrDefault().Id;
                    var redirectUrl = library.NiceUrl(redirectId);
                    HttpContext.Current.Response.RedirectPermanent(redirectUrl);
                    return true;
                }
                return false;
            }
            regexPattern = @"category\/(?<category>.*)\.aspx";
            match = Regex.Match(lowerUrl, regexPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var category = match.Groups["category"].Value;
                HttpContext.Current.Response.RedirectPermanent(String.Format("/search?c={0}", category.ToFreindlyUrl()));
                return true;
            }

            regexPattern = @"tag=\/(?<tag>.*)";
            match = Regex.Match(lowerUrl, regexPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var tag = match.Groups["tag"].Value;
                HttpContext.Current.Response.RedirectPermanent(String.Format("/search?t={0}", tag.ToFreindlyUrl()));
                return true;
            }
            return false;
        }

        public bool CacheUrl { get; private set; }
        public int redirectID { get; private set; }
    }
}
