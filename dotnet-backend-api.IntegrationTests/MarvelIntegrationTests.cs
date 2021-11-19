using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using dotnet_backend_api.Data;
using dotnet_backend_api.Models;
using dotnet_backend_api.Dtos.Marvel;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_backend_api.Services.MarvelService;
using dotnet_backend_api.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace dotnet_backend_api.IntegrationTests
{
    public class MarvelIntegrationTests
    {
       
        DbContextOptions<DataContext> dbContextOptions;

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;

        }

        
        public MarvelIntegrationTests()
        {}


        public Marvel createMarvelObject()
        {
            Marvel marvel = new Marvel();
      
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
        public async Task Test_GetAllMarvels_Fetch()
        {
            DataContext dataContext = new DataContext(dbContextOptions);
            Marvel marvel = createMarvelObject();

            dataContext.Add(marvel);

            _ = dataContext.SaveChanges();

            marvel = dataContext.Marvels.ToList().FirstOrDefault();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            MarvelService marvelService = new MarvelService(mapper, dataContext);

            MarvelController marvelController = new MarvelController(marvelService);

            
            var response = await marvelController.Get();

            OkObjectResult okResult = (OkObjectResult)response.Result;

            System.Console.WriteLine("Hello");
            System.Console.WriteLine("response is :" + ((OkObjectResult)(response.Result)).Value);

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedResponse = (ServiceResponse<List<GetMarvelDto>>)okResult.Value;
            System.Console.WriteLine("Marvel local is :" + marvel.Id);
            System.Console.WriteLine("returned response is :" + returnedResponse.Data.FirstOrDefault().Id);
            Assert.AreEqual(returnedResponse.Data.FirstOrDefault().Id, marvel.Id);

            dataContext.Remove(marvel);

            _ = dataContext.SaveChangesAsync();


        }

        [Test]
        public async Task Test_GetById_NotNull()
        {
            DataContext dataContext = new DataContext(dbContextOptions);
            Marvel marvel = createMarvelObject();

            dataContext.Add(marvel);

            _ = dataContext.SaveChanges();

            marvel = dataContext.Marvels.ToList().FirstOrDefault();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            MarvelService marvelService = new MarvelService(mapper, dataContext);

            MarvelController marvelController = new MarvelController(marvelService);


            var response = await marvelController.GetSingle(marvel.Id);

            OkObjectResult okResult = (OkObjectResult)response.Result;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedResponse = (ServiceResponse<GetMarvelDto>)okResult.Value;

            Assert.AreEqual(returnedResponse.Data.Id, marvel.Id);

            dataContext.Remove(marvel);

            _ = dataContext.SaveChangesAsync();

        }

        [Test]
        public async Task Test_AddMarvel_NotNull()
        {

            DataContext dataContext = new DataContext(dbContextOptions);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            MarvelService marvelService = new MarvelService(mapper, dataContext);

            MarvelController marvelController = new MarvelController(marvelService);


            AddMarvelDto addMarvelDto = createAddMarvelObject();

            var response = await marvelController.AddCharacter(addMarvelDto);

            OkObjectResult okResult = (OkObjectResult)response.Result;


            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedResponse = (ServiceResponse<GetMarvelDto>)okResult.Value;

            Assert.NotNull(returnedResponse.Data.Id);

            dataContext.Marvels.RemoveRange(dataContext.Marvels.Where(x => x.Id == returnedResponse.Data.Id));

            _ = dataContext.SaveChangesAsync();

        }

        [Test]
        public async Task Test_UpdateMarvel_Success()
        {
            DataContext dataContext = new DataContext(dbContextOptions);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();


            Marvel marvel = createMarvelObject();

            dataContext.Add(marvel);

            _ = dataContext.SaveChangesAsync();

            marvel = dataContext.Marvels.ToList().FirstOrDefault();


            MarvelService marvelService = new MarvelService(mapper, dataContext);

            MarvelController marvelController = new MarvelController(marvelService);

            UpdateMarvelDto updateMarvelDto = createUpdateMarvelDTO();
            updateMarvelDto.Id = marvel.Id;
            updateMarvelDto.Bio = "Updated";

            System.Console.WriteLine("Updated marvel is" + updateMarvelDto.Bio);
            System.Console.WriteLine("Updated marvel id is" + updateMarvelDto.Id);
            System.Console.WriteLine(" marvel id is" + updateMarvelDto.Id);

            var response = await marvelController.UpdateCharacter(updateMarvelDto);

            OkObjectResult okResult = (OkObjectResult)response.Result;


            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedResponse = (ServiceResponse<GetMarvelDto>)okResult.Value;

            Assert.AreEqual(returnedResponse.Data.Id, updateMarvelDto.Id);

            Assert.AreEqual(returnedResponse.Data.Bio, updateMarvelDto.Bio);

            dataContext.Remove(marvel);

            _ = dataContext.SaveChangesAsync();
        }


        [Test]
        public async Task Test_DeleteMarvel_Success()
        {
            DataContext dataContext = new DataContext(dbContextOptions);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            Marvel marvel = createMarvelObject();

            dataContext.Add(marvel);

            _ = dataContext.SaveChangesAsync();

            marvel = dataContext.Marvels.ToList().FirstOrDefault();

            MarvelService marvelService = new MarvelService(mapper, dataContext);

            MarvelController marvelController = new MarvelController(marvelService);

            var response = await marvelController.Delete(marvel.Id);

            OkObjectResult okResult = (OkObjectResult)response.Result;


            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedResponse = (ServiceResponse<List<GetMarvelDto>>)okResult.Value;
            //System.Console.WriteLine("Marvel local is :" + returnedResponse.Data.ToArray()[0].Id);
            //System.Console.WriteLine("Marvel local is :" + returnedResponse.Data.ToArray()[1].Id);
            //System.Console.WriteLine("Marvel local is :" + returnedResponse.Data.ToArray()[0].Name);
            //System.Console.WriteLine("Marvel local is :" + returnedResponse.Data.ToArray()[1].Name);

            System.Console.WriteLine("Count is :" + dataContext.Marvels.ToArray().Count());

            Assert.AreEqual(returnedResponse.Data.ToArray().Count(), 1);

            dataContext.Remove(marvel);
        }


        [TearDown]
        public void teardown ()
        {
          
        }

    }
}
