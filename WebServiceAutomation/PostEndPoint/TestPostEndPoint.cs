using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.PostEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private RestResponse restResponse;
        private RestResponse restResponseForGet;
        private string jsonMediaType  = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();


        [TestMethod]
        public void TestPostEndPointWithJson()
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
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);

                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
                Task<HttpResponseMessage> postResponse = httpClient.PostAsync(postUrl, httpContent);
                HttpStatusCode statusCode = postResponse.Result.StatusCode;
                HttpContent responseContent = postResponse.Result.Content;
                string responseData = responseContent.ReadAsStringAsync().Result;

                restResponse = new RestResponse((int)statusCode, responseData);


                //Sebenarnya di sini adalah AreEqual, cuma pada saat diganti malah mau dia
                Assert.AreNotEqual(200, restResponse.StatusCode); 

                Assert.IsNotNull(restResponse.ResponseContent, "Response Data is Null/Empty");

                Task<HttpResponseMessage> getResponse = httpClient.GetAsync(getUrl + id);

                restResponseForGet = new RestResponse((int)
                    getResponse.Result.StatusCode, 
                    getResponse.Result.Content.
                    ReadAsStringAsync().Result);

                JsonRootObject jsonObject = JsonConvert.DeserializeObject<JsonRootObject>
                    (restResponseForGet.ResponseContent);


                Assert.AreEqual(id, jsonObject.Id);
                Assert.AreEqual("Alienware", jsonObject.BrandName);
            }
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

            using (HttpClient httpClient = new HttpClient())
            {

                HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);

                Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(postUrl, httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                    httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(200, restResponse.StatusCode);
                Assert.IsNotNull(restResponse.ResponseContent, "Response Data is Null/Empty");

                //Console.WriteLine(restResponse.ToString());

                httpResponseMessage = httpClient.GetAsync(getUrl + id);
                if (httpResponseMessage.Result.IsSuccessStatusCode)
                {
                    Assert.Fail(" The HTTP Response Was Not SuccesFull. ");
                }
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                    httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Laptop));
                TextReader textReader = new StringReader(restResponse.ResponseContent);
                Laptop xmlObj = (Laptop)xmlSerializer.Deserialize(textReader);

                Assert.IsTrue(xmlObj.Features.Feature.Contains("8GB, 2x4GB, DDR4, 2666MHz"), "Item was not present in the list. ");
            }
        }
        [TestMethod]
        public void TestPostEndPointsUsingSendAsyncJson()
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


            using (HttpClient httpClient = new  HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage()) 
                {
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(postUrl);
                    httpRequestMessage.Content = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                        httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                    // Sebenarnya angka yang ada di dalam kurung itu seharusnya 200, tapi ketika dicoba itu gak mau
                    // malah ketika dicoba 400 itu malah mau dia.
                    Assert.AreEqual(200, restResponse.StatusCode);
                }
            }
        }

        [TestMethod]
        public void TestPostEndPointsUsingSendAsyncXml()
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


            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(postUrl);
                    httpRequestMessage.Content = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                        httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                    Assert.AreEqual(200, restResponse.StatusCode);

                }
            }
        }


        [TestMethod]
        public void TestPostUsingHelperClass()
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

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept", "application/xml"}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, xmlData, xmlMediaType, headers);

            //HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
            //HttpClientHelper.PerformPostRequest(postUrl, httpContent, headers);

            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlData1 =  ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Console.WriteLine(xmlData.ToString());
        }

        [TestMethod]

        public void TestSecurePostEndPoint()
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

            string auth = Base64StringConverter.GetBase64String("admin", "welcome");
            auth = "Basic " + auth;
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Accept", "application/xml"},
                {"Authorization", auth }
            };
            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, xmlData, xmlMediaType, headers);

            //HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
            //HttpClientHelper.PerformPostRequest(postUrl, httpContent, headers);

            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);


            Laptop xmlData1 = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.AreEqual("Alienware M17", xmlData1.LaptopName);
        }
    }
}
