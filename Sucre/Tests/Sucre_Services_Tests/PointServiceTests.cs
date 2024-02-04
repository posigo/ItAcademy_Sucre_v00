using MediatR;
using Microsoft.Extensions.Configuration;
using NSubstitute.ReturnsExtensions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services;

namespace Sucre_Services_Tests
{
    public class PointServiceTests 
    {   
        private readonly IConfiguration _configMock;
        private readonly IMediator _mediatorMock;
        private readonly ISucreUnitOfWork _uowMock;
        private readonly PointService _pointService;

        public PointServiceTests()
        {
            _configMock = Substitute.For<IConfiguration>(); 
            _mediatorMock = Substitute.For<IMediator>();
            _uowMock = Substitute.For<ISucreUnitOfWork>();
            _pointService = new PointService(
                _configMock,
                _uowMock,
                _mediatorMock,
                new CanalMapper(),
                new PointMapper());
        }

        [Fact]
        public async Task GetPointByIdAsync_ReturnPointDto()
        {
            //arrange
            //var uowMock = Substitute.For<ISucreUnitOfWork>();
            //var mediatorMock = Substitute.For<IMediator>();
            //var configMock = Substitute.For<IConfiguration>();
            //uowMock.repoSucrePoint.GetById();

            Point point = new Point()
            { 
                Id = 1,
            };
            

            _uowMock.repoSucrePoint.FindAsync(Arg.Is<int>(1))
               .Returns(point);
            //uowMock.repoSucrePoint.Find(1)
            //   .Returns(point);
            
            //var pointService = new PointService(
            //    configMock,
            //    uowMock,
            //    mediatorMock,
            //    new CanalMapper(),
            //    new PointMapper());
            //act
            var result = _pointService.GetPointByIdAsync(point.Id).GetAwaiter().GetResult();
            //assert
            Assert.Equal(result.Id, point.Id);
        }

        [Fact]
        public async Task GetPointByIdAsync_ReturnNull_NotExist()
        {
            //arrange
            _uowMock.repoSucrePoint.FindAsync(Arg.Any<int>()).ReturnsNull();
            
            //act
            var result = _pointService.GetPointByIdAsync(Arg.Any<int>()).GetAwaiter().GetResult();
            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetListPointsAsync_ReturnCountPointDto()
        {
            //arrange
            var lpoint = new List<Point>()
            {
                new Point() {Id = 1},
                new Point() {Id = 2},
            };
            _uowMock.repoSucrePoint.GetAllAsync().Returns(lpoint);               
            
            //act
            var result = (await _pointService.GetListPointsAsync()).Count();
            //assert
            Assert.Equal(result, lpoint.Count);
        }

        [Fact]
        public async Task GetListPointsAsync_ReturnTipe()
        {
            //arrange            
            //IEnumerable<Point> points = new HashSet<Point>();
            //_uowMock.repoSucrePoint.GetAllAsync().Returns(points);
            
            //act
            var result = (await _pointService.GetListPointsAsync()).ToList();
            //assert
            var viewRes = Assert.IsType<List<PointDto>>(result);
           //Assert.IsAssignableFrom.Equal(result, lpoint.Count);
        }

        [Fact]
        public async Task GetPointCanalesFullAsync_LoggerEx_WhenIdZero()
        {
            //Arrange
            var name = "GetPointCanalesFullAsync";
            _mediatorMock.Send(Arg.Any<GetPointByIdChannalesQuery>()).ReturnsNull();           

            //Act
            var result = await _pointService.GetPointCanalesFullAsync(0);

            //assert
            Assert.Null(result);
            
        }
        
    }
}