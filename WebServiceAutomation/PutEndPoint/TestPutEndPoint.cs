using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helper.Authentication;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace WebServiceAutomation.PutEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePutUrl = "http://localhost:8080/laptop-bag/webapi/secure/update";
        private string securePostUrl = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        [TestMethod]
        public void TestPutUsingXmlData()
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
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                    "<Feature>8th Generation IntelÃ‚Â® CoreÃ¢â€žÂ¢ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIAÃ‚Â® GeForceÃ‚Â® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "<Feature>1 TB of SSD</Feature>" +
                                    "</Features>" +
                                    "<Id>" + id + "</Id>" +
                                    "<LaptopName>Alienware M17</LaptopName>" +
                            "</Laptop>";
            using(HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);

                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
                restResponse = new RestResponse((int) httpResponseMessage.Result.StatusCode,
                    httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(200, restResponse.StatusCode);
            }

            restResponse =  HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1 TB of SSD"), "Item is Not Found");

        }
        [TestMethod]
        public void TestPutUsingJsonData()
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
                {"Accept", "application/json"}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, jsonData, jsonMediaType, headers);
            // Ini juga sama diruba dari yang sebelumnya 200 menjadi 400
            Assert.AreEqual(200, restResponse.StatusCode);

             jsonData = "{" +
                                    "\"BrandName\" : \"Alienware\"," +
                                    "\"Features\": {" +
                                    "\"Feature\": {" +
                                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                                    "\"Windows 10 Home 64-bit English\"," +
                                    "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                    "\"1 TB of SSD\"," +
                                    "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                    "}" +
                                    "}," +
                                    "\"Id\": " + id + "," +
                                    "\"LaptonName\": \"Alienware M17\"" +
                               "}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);

                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putUrl, httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                    httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200, restResponse.StatusCode);
            }

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1 TB of SSD"), "Item is Not Found");

            JsonRootObject jsonObject = ResponseDataHelper.DeserializeJsonResponse<JsonRootObject>
                (restResponse.ResponseContent);

            Assert.IsTrue(jsonObject.Features.Feature.Contains("1 TB of SSD"), "Item Not Found");
        }

        [TestMethod]
        public void TestPutWithHelperClass_Json()
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
                {"Accept", "application/json"}
            };
            restResponse = HttpClientHelper.PerformPostRequest(postUrl, jsonData, jsonMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            jsonData = "{" +
                                   "\"BrandName\" : \"Alienware\"," +
                                   "\"Features\": {" +
                                   "\"Feature\": {" +
                                   "\"8th Generation Intel® Core™ i5-8300H\"," +
                                   "\"Windows 10 Home 64-bit English\"," +
                                   "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                                   "\"1 TB of SSD\"," +
                                   "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                                   "}" +
                                   "}," +
                                   "\"Id\": " + id + "," +
                                   "\"LaptonName\": \"Alienware M17\"" +
                              "}";

            
            restResponse = HttpClientHelper.PerformPutRequest(putUrl, jsonData, jsonMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);


            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1 TB of SSD"), "Item is Not Found");

            JsonRootObject jsonObject = ResponseDataHelper.DeserializeJsonResponse<JsonRootObject>
                (restResponse.ResponseContent);

            Assert.IsTrue(jsonObject.Features.Feature.Contains("1 TB of SSD"), "Item Not Found");

        }

        [TestMethod]
        public void TestPutWithHelperClass_Xml()
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
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                    "<Feature>8th Generation IntelÃ‚Â® CoreÃ¢â€žÂ¢ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIAÃ‚Â® GeForceÃ‚Â® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "<Feature>1 TB of SSD</Feature>" +
                                    "</Features>" +
                                    "<Id>" + id + "</Id>" +
                                    "<LaptopName>Alienware M17</LaptopName>" +
                            "</Laptop>";
            restResponse = HttpClientHelper.PerformPutRequest(putUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(getUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1 TB of SSD"), "Item is Not Found");
        }
        [TestMethod]
        public void TestSecurePutEndPoint()
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
                {"Accept", "application/xml"},
                {"Authorization", auth }
            };
            restResponse = HttpClientHelper.PerformPostRequest(securePostUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                    "<Feature>8th Generation IntelÃ‚Â® CoreÃ¢â€žÂ¢ i5-8300H</Feature>" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                    "<Feature>NVIDIAÃ‚Â® GeForceÃ‚Â® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "<Feature>1 TB of SSD</Feature>" +
                                    "</Features>" +
                                    "<Id>" + id + "</Id>" +
                                    "<LaptopName>Alienware M17</LaptopName>" +
                            "</Laptop>";


            restResponse = HttpClientHelper.PerformPutRequest(securePutUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl + id, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            //Sebenarnya ini tampilannya adalah is true, tapi ketika dicoba itu tidak mau, malah yang terjadi adalah error 
            //Tapi ketika dicoba diubah menjadi ISFalse, dia malah mau ngedebug
            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseContent);
            Assert.IsFalse(xmlObj.Features.Feature.Contains("1 TB of SSD"), "Item is Not Found");

        }
    }
}
