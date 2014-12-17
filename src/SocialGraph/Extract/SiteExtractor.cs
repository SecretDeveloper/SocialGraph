using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using SocialGraph.Models;

namespace SocialGraph.Extract
{
    public class SiteExtractor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="startingNode"></param>
        /// <returns></returns>
        public Models.GraphBase GetGraphBase(NetworkInfo info, string startingNode)
        {
            var graph = new Models.GraphBase();

            var discovered = new List<string>();

            while (graph.nodes.Count < info.Depth)
            {
                Debug.WriteLine("Collected " + graph.nodes.Count + " nodes");
                var n = GetNode(info, startingNode);
                if(n!=null && (n.Followers >= info.MinimumSocialScore || n.Following >= info.MinimumSocialScore))
                    graph.nodes.Add(n);

                var links = GetLinks(info, startingNode);
                foreach (var link in links)
                {
                    if (graph.nodes.Find(x => x.Name == link.source) == null)
                    {
                        if (discovered.Find(x => x == link.source) == null) // undiscovered territory!!
                        {
                            discovered.Add(link.source);
                        }
                    }
                }
                graph.links.AddRange(links);
                graph.Purge();

                if (discovered.Count == 0) break;
                startingNode = discovered[0];
                discovered.Remove(startingNode);
            }
            return graph;
        }

        private List<GraphLink> GetLinks(NetworkInfo info, string userid)
        {
            try
            {
                var p = new Dictionary<string, string>();
                p.Add("username", userid);
                var url = info.ApiRoot + info.GetEndpoint("links").GetURI(p);

                var s = Util.Get(url);
                return ParseLinks(s, userid);
            }
            catch
            {
                return new List<GraphLink>();
            }
        }

        public List<GraphLink> ParseLinks(string response, string userid)
        {
            var links = new List<GraphLink>();

            MatchCollection matches = Util.FindRegexs(response, @"user\?id=\w*");
            var list = matches.Cast<Match>().Select(match => match.Value.Replace("user?id=", "")).ToList();

            foreach (var d in list)
            {
                var link = new GraphLink() {source = d, target = userid, Value = 1};
                links.Add(link);
            }

            return links;
        }

        private GraphNode GetNode(NetworkInfo info, string userId)
        {
            var p = new Dictionary<string, string>();
            p.Add("username", userId);
            var url = info.ApiRoot + info.GetEndpoint("profile").GetURI(p);

            var s = Util.Get(url);

            return ParseNode(userId, s);
        }

        private GraphNode ParseNode(string name, string body)
        {
            try
            {
                var node = new GraphNode();
                node.Name = name;

                node.AgeInDays = int.Parse(Util.FindRegex(body, @"member for: \d*").Replace("member for: ", ""));

                var prof = Util.FindRegexs(body, @">x \d*");
                node.Badges = int.Parse(prof[0].Value.Replace(">x ", ""));
                node.Followers = int.Parse(prof[1].Value.Replace(">x ", ""));

                var f = Util.FindRegex(body, @"following: .*\d");
                node.Following = int.Parse(f.Substring(f.LastIndexOf(">", System.StringComparison.Ordinal) + 1));

                node.DateStamp = DateTime.UtcNow;

                return node;
            }
            catch
            {
                return null;
            }
        }
    }
}
