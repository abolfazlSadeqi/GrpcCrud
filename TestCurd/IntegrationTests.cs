using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Core.Resource;
using DAL.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DefaultAPI.UnitTesting
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<API.Startup>>
    {
        private readonly HttpClient httpClient;

        public IntegrationTests(WebApplicationFactory<API.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }


        [Fact]
        public async Task CustomerQueryAll()
        {
            // Act
            var response = await httpClient.GetAsync("ApiPerson/GetAllPersons");

            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var terms = JsonSerializer.Deserialize<List<Person>>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Assert
          
            Assert.NotNull(terms);
            Assert.True(terms.Count() > 0);

         
        }



        [Fact]
        public async Task GetById()
        {
            // Act
            var response = await httpClient.GetAsync("ApiPerson/GetByIdPersons?id=2");

            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var terms = JsonSerializer.Deserialize<Person>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //Assert
            Assert.Equal(2, terms.ID);
            Assert.NotNull(terms);
          
        }





    }
}