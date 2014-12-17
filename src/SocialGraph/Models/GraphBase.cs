using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGraph.Models
{
    public class GraphBase
    {
        public List<GraphNode> nodes { get; set; }
        public List<GraphLink> links { get; set; }

        public GraphBase()
        {
            this.nodes = new List<GraphNode>();
            this.links = new List<GraphLink>();
        }

        public void Purge()
        {
            var graphLinks = new List<GraphLink>();
            foreach (var link in this.links)
            {
                if (nodes.Select(x => x.Name == link.source) != null)
                {
                    if (nodes.Select(x => x.Name == link.target) != null)
                    {
                        graphLinks.Add(link);
                    }
                }
            }
            this.links = graphLinks;
        }
    }

    public class GraphNode
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public int Group { get; set; }
        public int AgeInDays { get; set; }
        public int Badges { get; set; }
        public int Following { get; set; }
        public DateTime DateStamp { get; set; }
        public int Followers { get; set; }
    }

    public class GraphLink
    {
        public string source { get; set; }
        public string target { get; set; }
        public int Value { get; set; }
    }
}
