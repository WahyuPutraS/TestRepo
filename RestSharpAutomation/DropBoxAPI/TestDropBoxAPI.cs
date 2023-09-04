using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.DropBoxAPI
{
    [TestClass]
    public class TestDropBoxAPI
    {
        private const string ListEndPointUrl = "https://api.dropboxapi.com/2/files/list_folder";
        private const string CreateEndPointUrl = "https://api.dropboxapi.com/2/files/create_folder_v2";
        private const string DownloadEndPointUrl = "https://content.dropboxapi.com/2/files/download";
        private const string AccessToken = "sl.BlLxWq2g2ZagaP8H0NLMdEAwLL4wDBdZKfrOzKWIFIU9ftItxJv6nyJfgPZnlDitpRi3kuyK4_qxysFZ9Z-hqo3cTSNZKp7d7YK5ByxhHyIeZPCrSF38A2CaPtOqWS3ZP8wu2InXUfpL";

        [TestMethod]
        public void TestListFolder()
        {
            string body = "{\\\"include_deleted\\\":false,\\\"include_has_explicit_shared_members\\\":false,\\\"include_media_info\\\":false,\\\"include_mounted_folders\\\":true,\\\"include_non_downloadable_files\\\":true,\\\"path\\\":\\\"/Homework/math\\\",\\\"recursive\\\":false}\"";

            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = ListEndPointUrl
            };

            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(body);

            var response = client.Post<RestSharpAutomation.DropBoxAPI.ListFolderModel.RootObject>(request);
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void TestCreateFolder()
        {
            string body = "\"{\\\"autorename\\\":true,\\\"path\\\":\\\"/TestFolder\\\"}\"";
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = CreateEndPointUrl
            };

            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(body);
            var response = client.Post(request);
            Assert.AreEqual(200, (int)response.StatusCode);
        }
        [TestMethod]
        public void TestDownloadFile()
        {
            string location = "{\"path\": \"/Book.xlsx\"}";
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest()
            {
                Resource = DownloadEndPointUrl
            };

            request.AddHeader("Authorization", "Bearer " + AccessToken);
            request.AddHeader("Dropbox-API-Arg", location);
            request.RequestFormat = DataFormat.Json;
            var dataInByte = client.DownloadData(request);
            File.WriteAllBytes("Test.xlsx", dataInByte);
        }

        [TestMethod] 
        public void TestFileDownloadParaller()
        {
            string locationOfFile1 = "{\"path\":\"/Book.txt\"}";
            string locationOfFile2 = "{\"path\":\"/Test.txt\"}";

            IRestRequest file1 = new RestRequest()
            {
                Resource = DownloadEndPointUrl
            };
            file1.AddHeader("Authorization", "Bearer " + AccessToken);
            file1.AddHeader("DropBox-API-Arg", locationOfFile1);

            IRestRequest file2 = new RestRequest()
            {
                Resource = DownloadEndPointUrl
            };
            file2.AddHeader("Authorization", "Bearer " + AccessToken);
            file2.AddHeader("DropBox-API-Arg", locationOfFile2);

            IRestClient client = new RestClient();

            byte[] downloadDataFile1 = null;
            byte[] downloadDataFile2 = null;

            var task1 = Task.Factory.StartNew(() =>
            {
                downloadDataFile1 = client.DownloadData(file1);
            });
            var task2 = Task.Factory.StartNew(() =>
            {
                downloadDataFile2 = client.DownloadData(file2);
            });

            task1.Wait();
            task2.Wait();

            if(null != downloadDataFile1)
                File.WriteAllBytes("Book.txt", downloadDataFile1);

            if (null != downloadDataFile2)
            File.WriteAllBytes("Test.txt", downloadDataFile2);
        }
    }
}

