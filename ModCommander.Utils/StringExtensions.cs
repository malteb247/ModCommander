
using System.Text.RegularExpressions;
namespace ModCommander.Utils
{
    public static class StringExtensions
    {
        public static string RemoveEx(this string s, string[] lookup)
        {
            foreach (string l in lookup) { s = s.Replace(l, string.Empty); }
            return s;
        }

        public static string ReplaceEx(this string s, string[] lookup, string replacement)
        {
            foreach (string l in lookup) { s = s.Replace(l, replacement); }
            return s;
        }

        public static string Args(this string s, params object[] param)
        {
            return string.Format(s, param);
        }

        public static string NormalizeText(this string s)
        {
            NormalizeText(ref s);
            return s;
        }

        static void NormalizeText(ref string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Replace("\r\n", " ");
                s = s.Replace("\t", " ");
                s = new Regex(@"\s+", RegexOptions.Compiled).Replace(s, " ");
                s = new Regex(@"\A\s+", RegexOptions.Compiled).Replace(s, string.Empty);
            }
        }
    }
}
