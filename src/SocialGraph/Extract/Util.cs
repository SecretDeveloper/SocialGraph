using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SocialGraph.Extract
{
    internal class Util
    {
        public static string Get(string url)
        {
            WebClient client = new WebClient();

            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
            return s;
        }

        public static string FindRegex(string body, string pattern)
        {
            return Regex.Match(body, pattern).Value;
        }

        public static MatchCollection FindRegexs(string body, string pattern)
        {
            return Regex.Matches(body, pattern);
        }
    }
}