using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Apps_Gallery_Search_Term
{
    [Binding]
    public class SearchAppNameSteps
    {

        private string _theUrl;
        private string _theResponse;
        private string _name;

        [Given(@"the alteryx service is running at ""(.*)""")]
        public void GivenTheAlteryxServiceIsRunningAt(string alteryxUrl)
        {
            _theUrl = alteryxUrl;
        }

        [When(@"I invoke GET at application details at ""(.*)"" with search term ""(.*)""")]
        public void WhenIInvokeGETAtApplicationDetailsAtWithSearchTerm(string apiurl, string searchterm)
        {
            string search = Regex.Replace(searchterm, @"\s+", "+");
            string Url = _theUrl + "/" + apiurl + "?search=" + search;
            WebRequest webRequest = System.Net.WebRequest.Create(Url);
            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new System.IO.StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            _theResponse = responseFromServer;
        }

        [Then(@"I see primaryapplication\.metainfo\.name contains ""(.*)""")]
        public void ThenISeePrimaryapplication_Metainfo_NameContains(string expectedname)
        {
            var dict =
                new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(
                    _theResponse);
            int count = dict["recordCount"];
            if (count == 1)
            {
                _name = dict["records"][0]["primaryApplication"]["metaInfo"]["name"];
            }
            else
            {
                int i = 0;
                for (i = 0; i <= count - 1; i++)
                {
                    if (dict["records"][i]["primaryApplication"]["metaInfo"]["name"] == expectedname)
                    {
                        _name = dict["records"][i]["primaryApplication"]["metaInfo"]["name"];
                        break;
                    }

                }
            }
            Assert.AreEqual(expectedname, _name);
        }
    }
}
