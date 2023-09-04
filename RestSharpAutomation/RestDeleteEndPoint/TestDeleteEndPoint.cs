using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.RestDeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string deleteEndPoint = "http://localhost:8080/laptop-bag/webapi/api/delete/";
        private Random random = new Random();


        [TestMethod]
        public void TestDeleteRequest()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                                    "\"BrandName\" : \"Alienware\"," +
                                    "\"Features\": {" +
                                    "\"Feature\": {" +
                                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                                    "\"Windows 10 Home 64-bit English\"," +
                                    "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                    "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                    "}" +
                                    "}," +
                                    "\"Id\": " + id + "," +
                                    "\"LaptonName\": \"Alienware M17\"" +
                               "}";
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "Accept", "application/json" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformPostRequest(postUrl, headers, jsonData,
                RestSharp.DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = deleteEndPoint + id
            };
            request.AddHeader("Accept", "*/*");
            IRestResponse restResponse1 = client.Delete(request);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);

            restResponse1 = client.Delete(request);
            Assert.AreEqual(404,(int)restResponse1.StatusCode);

        }
        [TestMethod]
        public void TestDeleteRequest_HelperClass()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                                    "\"BrandName\" : \"Alienware\"," +
                                    "\"Features\": {" +
                                    "\"Feature\": {" +
                                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                                    "\"Windows 10 Home 64-bit English\"," +
                                    "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                    "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                    "}" +
                                    "}," +
                                    "\"Id\": " + id + "," +
                                    "\"LaptonName\": \"Alienware M17\"" +
                               "}";
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "Accept", "application/json" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformPostRequest(postUrl, headers, jsonData,
                RestSharp.DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            headers = new Dictionary<string, string>()
            {
                { "Accept", "*/*" }
            };

            IRestResponse restResponse1 = restClientHelper.PerformDeleteRequest(deleteEndPoint + id, headers);

            Assert.AreEqual(200, (int)restResponse1.StatusCode);

            restResponse1 = restClientHelper.PerformDeleteRequest(deleteEndPoint + id, headers);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);

        }
    }
}
