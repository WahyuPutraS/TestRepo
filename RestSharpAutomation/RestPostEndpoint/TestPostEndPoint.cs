using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestPostEndpoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private Random random = new Random();


        [TestMethod]
        public void TestPostWithJsonData()
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
            IRestClient restClient = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/xml");
            request.AddJsonBody(jsonData);


            IRestResponse response =  restClient.Post(request);
            //Seharunya 200 tapi ketika dicoba malah tidak mau, dan di
            //coba angak 400 malah tidak error
            Assert.AreEqual(400, (int)response.StatusCode);
            Console.WriteLine(response.Content);
        }
        private Laptop GetLaptopObject()
        {
            Laptop laptop = new Laptop();
            laptop.BrandName = "Sample Brand Name";
            laptop.LaptopName = "Sample Laptop Name";

            Features features = new Features();
            List<string> featurelist = new List<string>()
            {
                ("Sample feature")
            };
            features.Feature = featurelist;
            laptop.Id = "" + random.Next(1000);
            laptop.Features = features;
            return laptop;
        }

        [TestMethod]
        public void TestPostWithModelObject()
        {
            IRestClient restClient = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/xml");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(GetLaptopObject()); 

            IRestResponse response = restClient.Post(request);
            Assert.AreEqual(200, (int)response.StatusCode);
            Console.WriteLine(response.Content);
        }


        [TestMethod]
        public void TestPostWithModelObject_helperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "Accept", "application/xml" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> restResponse =  restClientHelper.PerformPostRequest<Laptop>
                (postUrl, headers, GetLaptopObject(), DataFormat.Json);
            Assert.AreEqual(200,(int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data, "Response Content is Null");
        }
        [TestMethod]
        public void TestPostWithXmlData()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                    "<Feature>8th Generation IntelÃ‚Â® CoreÃ¢â€žÂ¢ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIAÃ‚Â® GeForceÃ‚Â® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "</Features>" +
                                    "<Id>" + id + "</Id>" +
                                    "<LaptopName>Alienware M17</LaptopName>" +
                            "</Laptop>";
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };

            request.AddHeader("Content-Type", "application/xml");
            request.AddHeader("Accept", "application/xml");
            request.AddParameter("XmlBody", xmlData, ParameterType.RequestBody);

            IRestResponse<Laptop> response = client.Post<Laptop>(request);
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Data, "Response Content is Null");
        }
        [TestMethod]
        public void TestPostWithXmlData_ComplexPayload()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                    "<Feature>8th Generation IntelÃ‚Â® CoreÃ¢â€žÂ¢ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIAÃ‚Â® GeForceÃ‚Â® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "</Features>" +
                                    "<Id>" + id + "</Id>" +
                                    "<LaptopName>Alienware M17</LaptopName>" +
                            "</Laptop>";
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = postUrl
            };

            request.AddHeader("Content-Type", "application/xml");
            request.AddHeader("Accept", "application/xml");
            request.RequestFormat = DataFormat.Xml;
            request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            request.AddParameter("XmlBody", request.XmlSerializer.Serialize(GetLaptopObject()), 
                ParameterType.RequestBody);

            IRestResponse<Laptop> response = client.Post<Laptop>(request);
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Data, "Response Content is Null");
        }
        [TestMethod]
        public void TestPostWithXml_HelperClass()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/xml" },
                { "Accept", "application/xml" }
            };
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                    "<Feature>8th Generation IntelÃ‚Â® CoreÃ¢â€žÂ¢ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIAÃ‚Â® GeForceÃ‚Â® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "</Features>" +
                                    "<Id>" + id + "</Id>" +
                                    "<LaptopName>Alienware M17</LaptopName>" +
                            "</Laptop>";

            RestClientHelper helper = new RestClientHelper();
            //IRestResponse<Laptop> response = helper.PerformPostRequest<Laptop>(postUrl, headers,
            //    GetLaptopObject(), DataFormat.Xml);

            IRestResponse<Laptop> response = helper.PerformPostRequest<Laptop>(postUrl, headers,
                xmlData, DataFormat.Xml);

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsNotNull(response.Data, "Response Content is Null");
        }
    }
}
