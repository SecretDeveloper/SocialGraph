using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SocialGraph;
using SocialGraph.Models;

namespace SocialGraph.Tests
{
    [TestClass]
    public class SiteExtractorTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var info = new NetworkInfo();
            info.Depth = 1000;
            info.MinimumSocialScore = 20;
            info.ApiRoot = "https://hubski.com/";
            info.Endpoints.Add("links", new Endpoint(){PersistencePrefix = "link_", Title="", UriTemplate = @"followedby?id={username}"});
            info.Endpoints.Add("profile",new Endpoint(){PersistencePrefix = "profile_", Title = "", UriTemplate = @"user?id={username}"});

            SocialGraph.Extract.SiteExtractor ex = new SocialGraph.Extract.SiteExtractor();
            var g = ex.GetGraphBase(info, "Kaius");

            File.WriteAllText("c:\\temp\\hubski2.json",JsonConvert.SerializeObject(g));

            Assert.IsNotNull(g);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var o = JsonConvert.DeserializeObject(File.ReadAllText("c:\\temp\\hubski2.json"), typeof(GraphBase)) as GraphBase;
            o.Purge();
            File.WriteAllText("c:\\temp\\hubski2_purged.json", JsonConvert.SerializeObject(o));
        }
    }
}
