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

        AutoMocker mocker = new AutoMocker();

        AutoMapper.IMapper autoMapper;

        DataContext dataContext;

        [SetUp]
        public void Setup()
        {



        }

        public Marvel createMarvelObject() {
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

        [Test]
        public void Test_GetAllMarvels_NotNull()
        {
            Marvel marvel = createMarvelObject();

          DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: "PrimeDb")
        .Options;

            dataContext = new DataContext(dbContextOptions);


            var datamock = new Mock<DataContext>(dbContextOptions);

            List<Marvel> list = new List<Marvel>();
            list.Add(marvel);
            var mockDbSet = list.AsQueryable().BuildMockDbSet();


            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Marvel, GetMarvelDto>();
                cfg.CreateMap<AddMarvelDto, Marvel>();
            });

            IMapper mapper = config.CreateMapper();
            mocker = new AutoMocker();

            datamock.Setup(dataContext1 => dataContext1.Marvels).Returns(mockDbSet.Object);
     
            marvelService = new MarvelService(mapper, datamock.Object);

            Task<ServiceResponse<List<GetMarvelDto>>> marvelsDTOs = marvelService.GetAllMarvels();

            Assert.IsNotNull(marvelsDTOs);


        }
    }
}