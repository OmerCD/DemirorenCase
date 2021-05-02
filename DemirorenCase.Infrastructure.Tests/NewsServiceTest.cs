using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Abstractions.Services;
using DemirorenCase.Infrastructure.Respositories;
using DemirorenCase.Infrastructure.Services;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Moq;
using Xunit;

namespace DemirorenCase.Infrastructure.Tests
{
    public class SetupBeforeEachTest
    {
        public Mock<IRepository<News>> NewsRepository { get; set; }
        public Mock<IRepository<NewsGroup>> NewsGroupRepository { get; set; }
        public IMapper Mapper { get; set; }

        public SetupBeforeEachTest()
        {
            NewsRepository = new Mock<IRepository<News>>();
            var newsListMock = new List<News>();

            NewsRepository.Setup(x => x.InsertAsync(It.IsAny<News>(), default))
                .Callback((News news, CancellationToken token) =>
            {
                newsListMock.Add(news);
            });
            NewsRepository.Setup(x => x.GetAll(default)).Returns(newsListMock);
            NewsGroupRepository = new Mock<IRepository<NewsGroup>>();
            var config = new TypeAdapterConfig();
            config.Scan(
                typeof(IScopedService).Assembly,
                typeof(Domain.Domain).Assembly, typeof(BaseRepository<>).Assembly);
            Mapper = new Mapper(config);
        }
    }

    public class NewsServiceTest : IClassFixture<SetupBeforeEachTest>
    {
        private SetupBeforeEachTest _setupBeforeEachTest;

        public NewsServiceTest(SetupBeforeEachTest setupBeforeEachTest)
        {
            _setupBeforeEachTest = setupBeforeEachTest;
        }

        [Fact]
        public async void Should_NewsService_InsertTo_Database()
        {
            var service = new NewsService(_setupBeforeEachTest.NewsRepository.Object, _setupBeforeEachTest.Mapper,
                _setupBeforeEachTest.NewsGroupRepository.Object);
            await service.CreateNewsAsync(new CreateNewsDto()
            {
                Headline = "TestHeadline"
            });
            var found = service.GetAllNews().FirstOrDefault(x => x.Headline.Equals("TestHeadline"));
            found.Should().NotBeNull();
        }
    }
}