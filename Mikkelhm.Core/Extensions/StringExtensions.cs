using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikkelhm.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToFreindlyUrl(this string url)
        {
            url = url.ToLower();
            url = url.Replace(" ", "-");
            return url;
        }

        public static string FromFreindlyUrl(this string url)
        {
            url = url.ToLower();
            url = url.Replace("-", " ");
            return url;
        }
    }
}
