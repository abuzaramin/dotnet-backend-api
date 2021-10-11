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
using System;

namespace dotnet_backend_api.UnitTests
{
    public static class DbContextMock
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            return dbSet.Object;
        }
    }

    public class MarvelServiceUnitTests
    {
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
        public void Test_GetAllMarvels_NotNull()
        {
            Marvel marvel = createMarvelObject();

            var datamock = new Mock<DataContext>(dbContextOptions);

            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);

            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<List<GetMarvelDto>>> marvelsDTOs = marvelService.GetAllMarvels();

            Assert.IsNotNull(marvelsDTOs.Result.Data);
            Assert.IsTrue(marvelsDTOs.Result.Success);


        }

        [Test]
        public void Test_GetAllMarvels_Exception()
        {
            Marvel marvel = createMarvelObject();

            var datamock = new Mock<DataContext>(dbContextOptions);

            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            Exception e = new Exception("Exception message");
            datamock.Setup(dataContext1 => dataContext1.Marvels).Throws(e);

            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<List<GetMarvelDto>>> marvelsDTOs = marvelService.GetAllMarvels();

            Assert.IsFalse(marvelsDTOs.Result.Success);
            Assert.AreEqual(marvelsDTOs.Result.Message, e.Message);

        }

        [Test]
        public void Test_AddMarvels_NotNull()
        {
            AddMarvelDto addMarvel = createAddMarvelObject();

            Marvel marvel = createMarvelObject();
            var datamock = new Mock<DataContext>(dbContextOptions);

            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);

            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<GetMarvelDto>> marvelsDTOs = marvelService.AddMarvel(addMarvel);

            Assert.IsNotNull(marvelsDTOs.Result.Data.Id);
            Assert.AreSame(marvelsDTOs.Result.Data.Name, addMarvel.Name);
            Assert.IsTrue(marvelsDTOs.Result.Success);

        }

        [Test]
        public void Test_GetMarvelByID_NotNull()
        {
            Marvel marvel = createMarvelObject();

            var datamock = new Mock<DataContext>(dbContextOptions);

            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();


            System.Console.WriteLine("mock Dbset" + mockDbSet.Object);
            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);

            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<GetMarvelDto>> marvelsDTOs = marvelService.GetMarvelById(1);

            System.Console.WriteLine("Hello <<< " + marvelsDTOs.Result.Data.Id);
            Console.ReadLine();
            Assert.IsNotNull(marvelsDTOs.Result.Data.Id);
            Assert.IsTrue(marvelsDTOs.Result.Success);

        }

        [Test]
        public void Test_UpdateMarvel_NotNull()
        {
            UpdateMarvelDto updateMarvel = createUpdateMarvelDTO();

            var datamock = new Mock<DataContext>(dbContextOptions);

            Marvel marvel = createMarvelObject();
            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);
            datamock.Setup(dc => dc.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<GetMarvelDto>> marvelsDTOs = marvelService.UpdateMarvel(updateMarvel);

            System.Console.WriteLine("Hello <<< " + marvelsDTOs.Result.Data.Id);
            Console.ReadLine();
            Assert.IsNotNull(marvelsDTOs.Result.Data.Id);
            Assert.IsTrue(marvelsDTOs.Result.Success);

        }

        [Test]
        public void Test_UpdateMarvel_Exception()
        {
            UpdateMarvelDto updateMarvel = createUpdateMarvelDTO();

            var datamock = new Mock<DataContext>(dbContextOptions);

            Marvel marvel = createMarvelObject();
            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            Exception e = new Exception("Exception message");
            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);
            datamock.Setup(dc => dc.SaveChangesAsync(default)).Throws(e);
            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<GetMarvelDto>> marvelsDTOs = marvelService.UpdateMarvel(updateMarvel);

            Assert.IsFalse(marvelsDTOs.Result.Success);
            Assert.AreEqual(marvelsDTOs.Result.Message, e.Message);

        }

        [Test]
        public void Test_DeleteMarvel_NotNull()
        { 

            var datamock = new Mock<DataContext>(dbContextOptions);

            Marvel marvel = createMarvelObject();
            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();

            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);
            datamock.Setup(dc => dc.Remove(default)).Verifiable();
            datamock.Setup(dc => dc.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<List<GetMarvelDto>>> marvelsDTOs = marvelService.DeleteMarvel(1);

            Assert.IsNotNull(marvelsDTOs.Result.Data.FirstOrDefault().Id);
            Assert.IsTrue(marvelsDTOs.Result.Success);

        }

        [Test]
        public void Test_DeleteMarvel_Exception()
        {

            var datamock = new Mock<DataContext>(dbContextOptions);

            Marvel marvel = createMarvelObject();
            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();
            Exception e = new Exception("Exception message");

            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);
            datamock.Setup(dc => dc.Remove(default)).Verifiable();
            datamock.Setup(dc => dc.SaveChangesAsync(default)).Throws(e);
            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<List<GetMarvelDto>>> marvelsDTOs = marvelService.DeleteMarvel(1);

            Assert.IsFalse(marvelsDTOs.Result.Success);
            Assert.AreEqual(marvelsDTOs.Result.Message, e.Message);

        }
    }
}