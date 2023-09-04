using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.QueryParameter
{
    [TestClass]
    public class QueryParameter
    {
        private string searchUrl = "http://localhost:8080/laptop-bag/webapi/api/query";

        [TestMethod]
        public void TestQueryParameters()
        {
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = searchUrl
            };
            request.AddHeader("Accept", "application/xml");
            request.AddQueryParameter("id", "1");
            request.AddQueryParameter("LaptopName", "AlienWare M17");

            var response = client.Get<Laptop>(request);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("AlienWare", response.Data.BrandName);
        }
    }
}
