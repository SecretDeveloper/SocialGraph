using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGraph
{
    public class SocialGrapher
    {
        public void CreateGraph(NetworkInfo info)
        {
            
        }
    }

    public class NetworkInfo
    {
        public string Name { get; set; }
        public string ApiRoot { get; set; }

        public Dictionary<string, Endpoint> Endpoints { get; set; }
        public int Depth { get; set; }
        public int MinimumSocialScore { get; set; }

        public NetworkInfo()
        {
            this.Endpoints = new Dictionary<string, Endpoint>();
            this.Depth = -1;
        }

        public Endpoint GetEndpoint(string title)
        {
            return Endpoints[title];
        }
    }

    public class Endpoint
    {
        public string Title { get; set; }
        public string UriTemplate { get; set; }
        public string PersistencePrefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointParameters"></param>
        /// <returns></returns>
        public string GetURI(Dictionary<string, string> endpointParameters)
        {
            return endpointParameters.Keys.Aggregate(UriTemplate, (current, key) => current.Replace(@"{" + key + @"}", endpointParameters[key]));
        }
    }
}
