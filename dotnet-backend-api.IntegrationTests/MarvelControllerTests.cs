using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using dotnet_backend_api.Data;
using dotnet_backend_api.Services;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using dotnet_backend_api.Dtos.Marvel;
using dotnet_backend_api.Models;

namespace dotnet_backend_api.IntegrationTests
{
    public class MarvelControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public MarvelControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        public Marvel createMarvelObject()
        {
            Marvel marvel = new Marvel();
            marvel.Id = 1;
            marvel.Name = "Hulk";
            marvel.RealName = "Hulk Real Name";
            marvel.Team = "Avengers";
            marvel.CreatedBy = "Marvels";
            marvel.Publisher = "Publisher";
            marvel.ImageURL = "https://someImageUrl.com";
            marvel.Bio = "This is the bio of marvel";
            return marvel;
        }

        public AddMarvelDto createAddMarvelObject()
        {
            AddMarvelDto marvel = new AddMarvelDto();

            marvel.Name = "Hulk";
            marvel.RealName = "Hulk Real Name";
            marvel.Team = "Avengers";
            marvel.CreatedBy = "Marvels";
            marvel.Publisher = "Publisher";
            marvel.ImageURL = "https://someImageUrl.com";
            marvel.Bio = "This is the bio of marvel";
            return marvel;
        }

        public UpdateMarvelDto createUpdateMarvelDTO()
        {
            UpdateMarvelDto marvel = new UpdateMarvelDto();

            marvel.Id = 1;
            marvel.Name = "Hulk";
            marvel.RealName = "Hulk Real Name";
            marvel.Team = "Avengers";
            marvel.CreatedBy = "Marvels";
            marvel.Publisher = "Publisher";
            marvel.ImageURL = "https://someImageUrl.com";
            marvel.Bio = "This is the bio of marvel";
            return marvel;
        }

       
        [Fact]
        public async Task Test_GetMarvel_NoResult()
        {
            
            var response = await _client.GetAsync("/Marvel");

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Test_GetAllMarvels_Success()
        {

            GetMarvelDto marvel = await AddMarvelForTest();
            GetMarvelDto marvel2 = await AddMarvelForTest();
            var response = await _client.GetAsync("/Marvel");

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<List<GetMarvelDto>>>(stringResponse);

            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.IsType<List<GetMarvelDto>>(result.Data);
            Assert.Equal(2, result.Data.Count);

            await DeleteMarvelForTest(marvel.Id);
            await DeleteMarvelForTest(marvel2.Id);

        }

        [Fact]
        public async Task Test_GetMarvelById_Success()
        {

            GetMarvelDto marvel = await AddMarvelForTest();

            var response = await _client.GetAsync("/Marvel/" + marvel.Id);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<GetMarvelDto>>(stringResponse);

            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.IsType<GetMarvelDto>(result.Data);
            Assert.Equal(marvel.Id, result.Data.Id);

            await DeleteMarvelForTest(marvel.Id);

        }

        [Fact]
        public async Task PostMarvel_WithValidData_Returned200()
        {

            var marvel = createAddMarvelObject();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(marvel);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

           
            var response = await _client.PostAsync("/Marvel", data);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<GetMarvelDto>>(stringResponse);
            var addedMarvel = result.Data;
            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.IsType<GetMarvelDto>(result.Data );

            await DeleteMarvelForTest(addedMarvel.Id);
        }

        [Fact]
        public async Task UpdateMarvel_WithExistingObject_Returned200()
        {

            GetMarvelDto marvel = await AddMarvelForTest();
            UpdateMarvelDto updatMarvel = createUpdateMarvelDTO();
            updatMarvel.Id = marvel.Id;
            updatMarvel.Bio = "This is updated Bio";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(updatMarvel);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/Marvel", data);

            response.EnsureSuccessStatusCode(); 
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<GetMarvelDto>>(stringResponse);

            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.IsType<GetMarvelDto>(result.Data);
            Assert.Equal(result.Data.Bio, updatMarvel.Bio);

            await DeleteMarvelForTest(marvel.Id);
        }


        [Fact]
        public async Task Test_DeleteMarvel_Success()
        {

            GetMarvelDto marvel = await AddMarvelForTest();

            var response = await _client.DeleteAsync("/Marvel?id=" + marvel.Id);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<List<GetMarvelDto>>>(stringResponse);

            Assert.NotNull(response.Content.ReadAsStringAsync());
            Assert.IsType<List<GetMarvelDto>>(result.Data);
            Assert.Single(result.Data);
            Assert.Equal(marvel.Id, result.Data[0].Id);


        }

        private async Task<GetMarvelDto> AddMarvelForTest()
        {

            var marvel = createAddMarvelObject();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(marvel);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("/Marvel", data);

            response.EnsureSuccessStatusCode(); 
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResponse<GetMarvelDto>>(stringResponse);
            return result.Data;

        }

        
        private async Task DeleteMarvelForTest(int id)
        {

            var response = await _client.DeleteAsync("/Marvel?id=" + id);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var stringResponse = await response.Content.ReadAsStringAsync();
          
        }
    }
}
