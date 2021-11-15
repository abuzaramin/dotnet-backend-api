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

        [Test]
        public async Task Test_GetAllMarvels_NotNull()
        {
            Marvel marvel = createMarvelObject();


            var datamock = new Mock<DataContext>(dbContextOptions);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });
            IMapper mapper = config.CreateMapper();
            var serviceMock = new Mock<IMarvelService>();

            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);

            List<GetMarvelDto> getMarvelDTOs = list.Select(c => mapper.Map<GetMarvelDto>(c)).ToList();
            var serviceResponse = new ServiceResponse<List<GetMarvelDto>>();
            serviceResponse.Data = list.Select(c => mapper.Map<GetMarvelDto>(c)).ToList();

            serviceMock.Setup(service => service.GetAllMarvels()).Returns(Task.FromResult(serviceResponse));

            // marvelService = new MarvelService(mapper, datamock.Object);

            marvelController = new MarvelController(serviceMock.Object);
            var response = await marvelController.Get();

            System.Console.WriteLine("Hello");
            System.Console.WriteLine("response is :" + ((OkObjectResult)(response.Result)).Value);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedServiceResponse = (ServiceResponse<List<GetMarvelDto>>)okResult.Value;
            Assert.AreEqual(returnedServiceResponse.Data.FirstOrDefault().Id, getMarvelDTOs.FirstOrDefault().Id);


        }

    }
}
