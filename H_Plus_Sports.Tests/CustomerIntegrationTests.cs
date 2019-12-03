using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace H_Plus_Sports.Tests
{
    [TestClass]
    public class CustomerIntegrationTests
    {
       
        private readonly HttpClient client;
        public CustomerIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [TestMethod]
        public void CustomerGetAllTest()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Customers");

            //Act
            var response = client.SendAsync(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response);
        }
    }
}
