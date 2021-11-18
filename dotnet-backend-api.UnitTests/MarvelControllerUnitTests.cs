using System;
using NUnit.Framework;
using dotnet_backend_api.Controllers;
using dotnet_backend_api.Services.MarvelService;
using Moq.AutoMock;
using Moq;
using dotnet_backend_api.Data;
using dotnet_backend_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MockQueryable.Moq;
using dotnet_backend_api.Dtos.Marvel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_backend_api.UnitTests
{
    public class MarvelControllerUnitTests
    {
        MarvelController marvelController;

        MarvelService marvelService;

        AutoMapper.IMapper autoMapper;

        DataContext dataContext;

        DbContextOptions<DataContext> dbContextOptions;

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;

            dataContext = new DataContext(dbContextOptions);

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

        public GetMarvelDto createGetMarvelDTO()
        {
            GetMarvelDto marvel = new GetMarvelDto();

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

        [Test]
        public async Task Test_GetAllMarvels_NotNull()
        {

            GetMarvelDto getMarvelDTO = createGetMarvelDTO();

            var datamock = new Mock<DataContext>(dbContextOptions);
           
            var serviceMock = new Mock<IMarvelService>();

            List<GetMarvelDto> list = new List<GetMarvelDto>();
            list.Add(getMarvelDTO);

            var serviceResponse = new ServiceResponse<List<GetMarvelDto>>();
            serviceResponse.Data = list;

            serviceMock.Setup(service => service.GetAllMarvels()).Returns(Task.FromResult(serviceResponse));

            marvelController = new MarvelController(serviceMock.Object);
            var response = await marvelController.Get();

            System.Console.WriteLine("Hello");
            System.Console.WriteLine("response is :" + ((OkObjectResult)(response.Result)).Value);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedServiceResponse = (ServiceResponse<List<GetMarvelDto>>)okResult.Value;
            Assert.AreEqual(returnedServiceResponse.Data.FirstOrDefault().Id, list.FirstOrDefault().Id);

        }

        [Test]
        public async Task Test_GetSingle_NotNull()
        {
            GetMarvelDto getMarvelDTO = createGetMarvelDTO();

            var datamock = new Mock<DataContext>(dbContextOptions);
            
            var serviceMock = new Mock<IMarvelService>();

            var serviceResponse = new ServiceResponse<GetMarvelDto>();
            serviceResponse.Data = getMarvelDTO;

            serviceMock.Setup(service => service.GetMarvelById(1)).Returns(Task.FromResult(serviceResponse));

            marvelController = new MarvelController(serviceMock.Object);
            var response = await marvelController.GetSingle(1);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedServiceResponse = (ServiceResponse<GetMarvelDto>)okResult.Value;
            Assert.AreEqual(returnedServiceResponse.Data.Id, getMarvelDTO.Id);

        }

        [Test]
        public async Task Test_AddCharacter_NotNull()
        {
            GetMarvelDto getMarvelDTO = createGetMarvelDTO();

            AddMarvelDto addMarvelDTO = createAddMarvelObject();

            var datamock = new Mock<DataContext>(dbContextOptions);

            var serviceMock = new Mock<IMarvelService>();

            var serviceResponse = new ServiceResponse<GetMarvelDto>();
            serviceResponse.Data = getMarvelDTO;

            serviceMock.Setup(service => service.AddMarvel(addMarvelDTO)).Returns(Task.FromResult(serviceResponse));

            marvelController = new MarvelController(serviceMock.Object);
            var response = await marvelController.AddCharacter(addMarvelDTO);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedServiceResponse = (ServiceResponse<GetMarvelDto>)okResult.Value;
            Assert.AreEqual(returnedServiceResponse.Data.RealName, addMarvelDTO.RealName);
            Assert.NotNull(returnedServiceResponse.Data.Id);

        }

        [Test]
        public async Task Test_UpdateCharacter_NotNull()
        {
            GetMarvelDto getMarvelDTO = createGetMarvelDTO();

            UpdateMarvelDto updateMarvelDto = createUpdateMarvelDTO();

            var datamock = new Mock<DataContext>(dbContextOptions);

            var serviceMock = new Mock<IMarvelService>();

            var serviceResponse = new ServiceResponse<GetMarvelDto>();
            serviceResponse.Data = getMarvelDTO;

            serviceMock.Setup(service => service.UpdateMarvel(updateMarvelDto)).Returns(Task.FromResult(serviceResponse));

            marvelController = new MarvelController(serviceMock.Object);
            var response = await marvelController.UpdateCharacter(updateMarvelDto);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedServiceResponse = (ServiceResponse<GetMarvelDto>)okResult.Value;
            Assert.AreEqual(returnedServiceResponse.Data.Id, updateMarvelDto.Id);
            Assert.NotNull(returnedServiceResponse.Data.Id);

        }

        [Test]
        public async Task Test_DeleteCharacter_NotNull()
        {
            GetMarvelDto getMarvelDTO = createGetMarvelDTO();

            var datamock = new Mock<DataContext>(dbContextOptions);

            var serviceMock = new Mock<IMarvelService>();

            var serviceResponse = new ServiceResponse<List<GetMarvelDto>>();

            List<GetMarvelDto> list = new List<GetMarvelDto>();
            list.Add(getMarvelDTO);
            serviceResponse.Data = list;

            serviceMock.Setup(service => service.DeleteMarvel(1)).Returns(Task.FromResult(serviceResponse));

            marvelController = new MarvelController(serviceMock.Object);
            var response = await marvelController.Delete(1);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedServiceResponse = (ServiceResponse<List<GetMarvelDto>>)okResult.Value;
            Assert.AreEqual(returnedServiceResponse.Data.FirstOrDefault().Id , 1);
            Assert.NotNull(returnedServiceResponse.Data.FirstOrDefault().Id);

        }

        [Test]
        public void Test_Health_OK()
        {
            var serviceMock = new Mock<IMarvelService>();

            marvelController = new MarvelController(serviceMock.Object);

            var response =  marvelController.Health();
            var okResult = response as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }

    }
}
