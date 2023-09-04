using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.JiraAPI.Request;
using RestSharpAutomation.JiraAPI.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.JiraAPI
{
    [TestClass]
    public class TestJiraEndToEndLow
    {
        private const string LoginEndPoint = "/rest/auth/1/session";
        private const string LogOutEndPoint = "/rest/auth/1/session";
        private const string CreateProjectEndPoint = "/rest/api/2/project";
        private static IRestClient client;
        private static IRestResponse<LoginResponse> loginResponse;

        [ClassInitialize]
        public static void Login(TestContext context)
        {
            client = new RestClient()
            {
                BaseUrl = new Uri("http://localhost9191")  
            };

            IRestRequest request = new RestRequest()
            {
                Resource = LoginEndPoint
            };
            JiraLogin jiraLogin = new JiraLogin()
            {
                username = "Wahyu",
                password = "Suartama123."
            };

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(jiraLogin);
            request.AddHeader("Content-Type", "application/json");
            loginResponse = client.Post<LoginResponse>(request);
            Assert.AreEqual(200, (int)loginResponse.StatusCode);
        }

        [ClassCleanup]
        public static void Logout()
        {
            IRestRequest request = new RestRequest()
            {
                Resource = LogOutEndPoint
            };

            request.AddCookie(loginResponse.Data.session.name, loginResponse.Data.session.value);
            var response = client.Delete(request);
            Assert.AreEqual(204, (int)response.StatusCode);
        }

        [TestMethod]
        public void CreateProject()
        {
            CreateProjectPayload createProjectPayload = new CreateProjectPayload();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = CreateProjectEndPoint
            };
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(createProjectPayload);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddCookie(loginResponse.Data.session.name, loginResponse.Data.session.value);
            var response = client.Post<CreateProjectResponse>(restRequest);
            Assert.AreEqual(201, (int)response.StatusCode);
        }
    }
}
