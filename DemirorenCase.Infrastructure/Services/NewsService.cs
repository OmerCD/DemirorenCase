using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;

namespace DemirorenCase.Infrastructure.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRepository;
        private readonly IRepository<NewsGroup> _newsGroupRepository;
        private readonly IMapper _mapper;

        public NewsService(IRepository<News> newsRepository, IMapper mapper, IRepository<NewsGroup> newsGroupRepository)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _newsGroupRepository = newsGroupRepository;
        }

        public async Task<GetNewsDto> CreateNewsAsync(CreateNewsDto createNewsDto, CancellationToken token = default)
        {
            var news = _mapper.Map<News>(createNewsDto);
            await _newsRepository.InsertAsync(news, token);
            return _mapper.Map<GetNewsDto>(news);
        }

        public async Task<GetNewsGroupDto> CreateNewsGroupAsync(CreateNewsGroupDto createNewsGroupDto,
            CancellationToken token = default)
        {
            var newsGroup = _mapper.Map<NewsGroup>(createNewsGroupDto);
            await _newsGroupRepository.InsertAsync(newsGroup, token);
            return _mapper.Map<GetNewsGroupDto>(newsGroup);
        }

        public async Task<GetNewsGroupDto> InsertNewsToGroupAsync(InsertNewsToGroupDto insertNewsToGroupDto,
            CancellationToken token = default)
        {
            var newsGroup = await _newsGroupRepository.GetAsync(insertNewsToGroupDto.GroupId, token);
            var news = await _newsRepository.GetAsync(insertNewsToGroupDto.NewsId, token);
            newsGroup.OrderedNews.Add(new OrderedNews
            {
                News = news,
                Order = insertNewsToGroupDto.Order
            });
             await _newsGroupRepository.UpdateAsync(newsGroup, token);
             return _mapper.Map<GetNewsGroupDto>(newsGroup);
        }

        public async Task<GetNewsGroupDto> GetNewsGroupAsync(string id, CancellationToken token = default)
        {
            var newsGroup = await _newsGroupRepository.GetAsync(id, token);
            return newsGroup != null ? _mapper.Map<GetNewsGroupDto>(newsGroup) : null;
        }

        public IEnumerable<GetNewsGroupListItemDto> GetAllNewsGroups(CancellationToken token = default)
        {
            var groups = _newsGroupRepository.GetAll(token);
            return _mapper.Map<IEnumerable<GetNewsGroupListItemDto>>(groups);
        }

        public IEnumerable<GetNewsDto> GetAllNews(CancellationToken token = default)
        {
            var allNews = _newsRepository.GetAll(token);
            return _mapper.Map<IEnumerable<GetNewsDto>>(allNews);
        }
    }
}