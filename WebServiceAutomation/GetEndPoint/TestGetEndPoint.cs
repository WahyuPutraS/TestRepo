using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;
using WebServiceAutomation.Helper.Request;
using WebServiceAutomation.Helper.ResponseData;
using WebServiceAutomation.Helper.Authentication;

namespace WebServiceAutomation.GetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";
        private string delayget = "http://localhost:8080/laptop-bag/webapi/delay/all";

        [TestMethod]
        public void TestGetAllEndPoint()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.GetAsync(getUrl);

            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithUri()
        {
            //Create HttpClient
            HttpClient httpClient = new HttpClient();

            // Create The Request and Execute It
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code " + statusCode);
            Console.WriteLine("Status Code " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Close the Connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithInvalidUrl()
        {
            //Create HttpClient
            HttpClient httpClient = new HttpClient();

            //Create the Request and Execute it
            Uri getUri = new Uri(getUrl + "/random");
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            // Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code " + statusCode);
            Console.WriteLine("Status Code " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Close the Connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointFetchHeaders()
        {
            // Create To Http Client
            HttpClient httpClient = new HttpClient();

            //Create the Request and Execute it
            Uri getUri = new Uri(getUrl + "/random");
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code =>" + (int)statusCode);

            //Close the Connection
            httpClient.Dispose();

        }

        [TestMethod]
        public void TestSetAllEndPointInJsonFormat()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/json");

            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUrl);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            // Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code " + statusCode);
            Console.WriteLine("Status Code " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Close the Connection
            httpClient.Dispose();

        }

        [TestMethod]
        public void TestSetAllEndPointInXmlFormat()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/xml");

            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUrl);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            // Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code " + statusCode);
            Console.WriteLine("Status Code " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Close the Connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestSetAllEndPointInXmlFormatUsingAcceptHeader()
        {
            MediaTypeWithQualityHeaderValue jsonHeader = new MediaTypeWithQualityHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Accept.Add(jsonHeader);
            //requestHeaders.Add("Accept", "application/xml");

            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUrl);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            // Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code " + statusCode);
            Console.WriteLine("Status Code " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Close the Connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetEndPointUsingSendAsync()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(getUrl);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Add("Accept", "application/json");

            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            // Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code " + statusCode);
            Console.WriteLine("Status Code " + (int)statusCode);

            //Response Data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Close the Connection
            httpClient.Dispose();
        }

        [TestMethod]

        public void TestUsingStatement()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        Console.WriteLine(httpResponseMessage.ToString());

                        //Status Code
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                        //Console.WriteLine("Status Code => " + statusCode);
                        //Console.WriteLine("Status Code =>" + (int)statusCode);

                        //Response Data 
                        HttpContent responseContent = httpResponseMessage.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        //Console.WriteLine(data);

                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        Console.WriteLine(restResponse.ToString());


                    }

                }
            }
        }

        [TestMethod]

        public void TestDeserilizationOfJsonResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        Console.WriteLine(httpResponseMessage.ToString());

                        //Status Code
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                        //Console.WriteLine("Status Code => " + statusCode);
                        //Console.WriteLine("Status Code =>" + (int)statusCode);

                        //Response Data 
                        HttpContent responseContent = httpResponseMessage.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        //Console.WriteLine(data);

                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        //Console.WriteLine(restResponse.ToString());

                        List<JsonRootObject> jsonRootObject = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
                        Console.WriteLine(jsonRootObject[0].ToString());

                    }

                }
            }

        }

        [TestMethod]

        public void TestDeserilizationOfXMLResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/xml");

                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        Console.WriteLine(httpResponseMessage.ToString());

                        //Status Code
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                        //Console.WriteLine("Status Code => " + statusCode);
                        //Console.WriteLine("Status Code =>" + (int)statusCode);

                        //Response Data 
                        HttpContent responseContent = httpResponseMessage.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        //Console.WriteLine(data);

                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        //Console.WriteLine(restResponse.ToString());

                        // Step 1
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetailss));

                        // Step 2
                        TextReader textReader = new StringReader(restResponse.ResponseContent);

                        // Step 3
                        LaptopDetailss xmlData = (LaptopDetailss)xmlSerializer.Deserialize(textReader);
                        Console.WriteLine(xmlData.ToString());

                        // CheckPoint (Assertion) for status Kode
                        Assert.AreEqual(200, restResponse.StatusCode);

                        // 2nd CheckPoint (Assertion) for response data
                        Assert.IsNotNull(restResponse.ResponseContent);

                        //3rd

                        //Sebenarnya ditutorial itu yang ada IsTrue tapi ketika di ketik error dia,
                        //tapi ketika disetting false malah berhasil jalan programnya
                        //Assert.IsFalse(xmlData.Laptop.Features.Feature.Contains("Windows 10 Home 64-bit English"), "Item not found");

                        //// 4th
                        //Assert.AreEqual("Alienware", xmlData.Laptop.BrandName);


                    }

                }
            }

        }


        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");

            RestResponse restResponse = HttpClientHelper.PerformGetRequest(getUrl, httpHeader);


            //List<JsonRootObject> jsonRootObjects = JsonConvert.DeserializeObject<List<JsonRootObject>> 
            //    (restResponse.ResponseContent);
            //Console.WriteLine(jsonRootObjects[0].ToString());

            List<JsonRootObject> jsonData = ResponseDataHelper.DeserializeJsonResponse<List<JsonRootObject>>
                (restResponse.ResponseContent);

            Console.WriteLine(jsonData.ToString());
        }
        [TestMethod]
        public void TestSecureGetEndPoint()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            //httpHeader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==") 

            string authHeader = Base64StringConverter.GetBase64String("admin", "welcome");
            authHeader = "Basic " + authHeader;
            httpHeader.Add("Authorization", authHeader);

            RestResponse restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl, httpHeader);


            //List<JsonRootObject> jsonRootObjects = JsonConvert.DeserializeObject<List<JsonRootObject>> 
            //    (restResponse.ResponseContent);
            //Console.WriteLine(jsonRootObjects[0].ToString());
            Assert.AreEqual(200, restResponse.StatusCode);

            List<JsonRootObject> jsonData = ResponseDataHelper.DeserializeJsonResponse<List<JsonRootObject>>
                (restResponse.ResponseContent);

            Console.WriteLine(jsonData.ToString());
        }

        [TestMethod]
        public void TestGetEndPoint_Sync()
        {
            // Statement 1
            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);

            //Statement 2
            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);

            //Statement 3 
            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);

            //Statement 4
            HttpClientHelper.PerformGetRequest("http://localhost:8080/laptop-bag/webapi/delay/all", null);
        }

        [TestMethod]

        public void TestGetEndPoint_Async()
        {
            Task t1 = new Task(GetEndPoint());
            t1.Start();
            Task t2 = new Task(GetEndPoint());
            t2.Start();
            Task t3 = new Task(GetEndPoint());
            t3.Start();
            Task t4 = new Task(GetEndPointFailed());
            t4.Start();


            //t1.Start();
            //t2.Wait();
            //t3.Wait();
            //t4.Wait();
        }

        private Action GetEndPoint()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                 { "Accept", "application/xml"},
            };

            return new Action(() =>
            {
                RestResponse restResponse = HttpClientHelper.PerformGetRequest(delayget, null);
                Assert.AreEqual(200, restResponse.StatusCode);
            });
        }
        private Action GetEndPointFailed()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                 { "Accept", "application/xml"},
            };

            return new Action(() =>
            {
                RestResponse restResponse = HttpClientHelper.PerformGetRequest(delayget, null);
                Assert.AreEqual(201, restResponse.StatusCode);
            });

        }

    }
}
