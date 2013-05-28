using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using asafaweb.console.Enums;


namespace asafaweb.console.Logic
{
    public class StatusLogic
    {
        public static AsafaResult GetStatus(string status)
        {
            switch (status)
            {
                case "Pass":
                    return AsafaResult.Pass;
                case "Fail":
                    return AsafaResult.Fail;
                case "Warning":
                    return AsafaResult.Warning;
                default:
                    return AsafaResult.NotTested;
            }
        }

        public static Dictionary<string, AsafaResult> GetTestResults(string url)
        {
            HtmlGetLogic htmlLogic = new HtmlGetLogic();
            try
            {
                HtmlDocument response = htmlLogic.LoadHtmlResponse(string.Format(@"https://asafaweb.com/Scan?Url={0}", HttpUtility.UrlEncode(url)));
                HtmlNodeCollection nodesMatchingXPath = response.DocumentNode.SelectNodes("//div[@id='StatusSummary']/span");
                return nodesMatchingXPath != null
                    ? nodesMatchingXPath.ToDictionary(element => element.Attributes["id"].Value.Replace("Summary", ""), element => GetStatus(element.Attributes["class"].Value))
                    : new Dictionary<string, AsafaResult>();
            }
            catch (WebException)
            {
                return new Dictionary<string, AsafaResult>();
            }
        }
    }
}
