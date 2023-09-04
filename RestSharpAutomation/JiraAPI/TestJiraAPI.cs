using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.JiraAPI.Request;
using RestSharpAutomation.JiraAPI.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//Sebenarnya ini adalah project yang salah di dalam http, sebenarnya itu bukan http yang benar hanya dibuat buat 
// asal memasukan saja karna ditutorial sama yang di cari itu beda
namespace RestSharpAutomation.JiraAPI
{
    [TestClass]
    public class TestJiraAPI
    {
        private const string LoginEndPoint = "http://localhost9191/rest/auth/1/session";

        [TestMethod]
        public void TestJiraLogin()
        {
            JiraLogin jiraLogin = new JiraLogin()
            {
                username = "WahyuPutra",
                password = "Suartama123."
            };

            IRestClient client = new RestClient()
            {
                BaseUrl = new Uri("http://localhost9191")
            };
            IRestRequest request = new RestRequest()
            {
                Resource = LoginEndPoint
            };
            
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(jiraLogin);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Post<LoginResponse>(request);

            Console.WriteLine(response.Data);
        }
    }
}
