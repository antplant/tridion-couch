using System.Text.RegularExpressions;

namespace TridionCouch.Templates.Extensions
{
    public static class StringExtensions
    {
        public static string Clean(this string str)
        {
            var regex = new Regex("[^a-zA-Z0-9]");
            return regex.Replace(str, "");
        }
    }
}
