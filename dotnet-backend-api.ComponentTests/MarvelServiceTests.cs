using System;
using NUnit.Framework;
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

namespace dotnet_backend_api.ComponentTests
{
    public class MarvelServiceTests
    {
        DbContextOptions<DataContext> dbContextOptions;

        IMapper mapper;

        DataContext dataContext;

        MarvelService marvelService;

        public MarvelServiceTests()
        { }

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            mapper = config.CreateMapper();

            dataContext = new DataContext(dbContextOptions);

            marvelService = new MarvelService(mapper, dataContext);
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
           
            dataContext.Add(marvel);
            _ = dataContext.SaveChangesAsync();

            var marvelsDTOs = await marvelService.GetAllMarvels();

            Assert.IsNotNull(marvelsDTOs.Data);
            Assert.IsTrue(marvelsDTOs.Success);
            Assert.IsTrue(marvelsDTOs.Data.Count.Equals(1));
        }

        [Test]
        public async Task Test_GetAllMarvels_Empty()
        {
            Marvel marvel = createMarvelObject();
           

            var marvelsDTOs = await marvelService.GetAllMarvels();

            Assert.IsNotNull(marvelsDTOs.Data);
            Assert.IsTrue(marvelsDTOs.Data.Count.Equals(0));
            Assert.IsTrue(marvelsDTOs.Success);

        }

        [Test]
        public async Task Test_AddMarvels_NotNull()
        {
            AddMarvelDto addMarvel = createAddMarvelObject();
            var marvelsDTOs = await marvelService.AddMarvel(addMarvel);


            Assert.IsNotNull(marvelsDTOs.Data.Id);
            Assert.AreSame(marvelsDTOs.Data.Name, addMarvel.Name);
            Assert.IsTrue(marvelsDTOs.Success);

        }

        [Test]
        public async Task Test_GetMarvelByID_NotNull()
        {
            Marvel marvel = createMarvelObject();
            var marvelsDTOs = await marvelService.GetMarvelById(1);

            Assert.IsNotNull(marvelsDTOs.Data.Id);
            Assert.IsTrue(marvelsDTOs.Success);

        }

        [Test]
        public async Task Test_UpdateMarvel_NotNull()
        {
            UpdateMarvelDto updateMarvel = createUpdateMarvelDTO();
            var marvelsDTOs = await marvelService.UpdateMarvel(updateMarvel);

            Assert.IsNotNull(marvelsDTOs.Data.Id);
            Assert.IsTrue(marvelsDTOs.Success);

        }


        [Test]
        public async Task Test_DeleteMarvel_NotNull()
        {
            Marvel marvel = createMarvelObject();
            var marvelsDTOs = await marvelService.DeleteMarvel(1);


            Assert.IsNotNull(marvelsDTOs.Data.FirstOrDefault().Id);
            Assert.IsTrue(marvelsDTOs.Success);

        }
    }
}
