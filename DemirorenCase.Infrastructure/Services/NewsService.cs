using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using DemirorenCase.Infrastructure.Abstractions.Repositories;
using DemirorenCase.Infrastructure.Abstractions.Services;
using MapsterMapper;
using StackExchange.Redis;

namespace DemirorenCase.Infrastructure.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRepository;
        private readonly IRepository<NewsGroup> _newsGroupRepository;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;

        public NewsService(IRepository<News> newsRepository, IMapper mapper, IRepository<NewsGroup> newsGroupRepository,
            ICacheClient cacheClient = null)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _newsGroupRepository = newsGroupRepository;
            _cacheClient = cacheClient;
        }

        public async Task<GetNewsDto> CreateNewsAsync(CreateNewsDto createNewsDto, CancellationToken token = default)
        {
            var news = _mapper.Map<News>(createNewsDto);
            await _newsRepository.InsertAsync(news, token);
            _cacheClient?.RemoveKey("news");
            return _mapper.Map<GetNewsDto>(news);
        }

        public async Task<GetNewsGroupDto> CreateNewsGroupAsync(CreateNewsGroupDto createNewsGroupDto,
            CancellationToken token = default)
        {
            var newsGroup = _mapper.Map<NewsGroup>(createNewsGroupDto);
            await _newsGroupRepository.InsertAsync(newsGroup, token);
            return _mapper.Map<GetNewsGroupDto>(newsGroup);
        }

        public async Task<InsertNewsOrderResult> InsertNewsToGroupAsync(InsertNewsToGroupDto insertNewsToGroupDto,
            CancellationToken token = default)
        {
            var newsGroup = await _newsGroupRepository.GetAsync(insertNewsToGroupDto.GroupId, token);
            var news = await _newsRepository.GetAsync(insertNewsToGroupDto.NewsId, token);
            var result = new InsertNewsOrderResult();
            newsGroup.OrderedNews ??= new List<OrderedNews>();
            if (newsGroup.OrderedNews.Any(x=>x.Order == insertNewsToGroupDto.Order))
            {
                result.Error = "Order is already taken";
                return result;
            }

            if (newsGroup.OrderedNews.Any(x => x.News.Id == insertNewsToGroupDto.NewsId))
            {
                result.Error = "News is already added";
                return result;
            }
            newsGroup.OrderedNews.Add(new OrderedNews
            {
                News = news,
                Order = insertNewsToGroupDto.Order
            });
            news.UsedGroups ??= new List<string>();
            news.UsedGroups.Add(newsGroup.Id);
            await _newsRepository.UpdateAsync(news, token);
            await _newsGroupRepository.UpdateAsync(newsGroup, token);
            var newsGroupDto = _mapper.Map<GetNewsGroupDto>(newsGroup);
            result.IsSuccessful = true;
            result.NewsGroupDto = newsGroupDto;
            return result;
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
            IEnumerable<News> news = _cacheClient?.JsonGet<News[]>("news");
            if (news != null) return _mapper.Map<IEnumerable<GetNewsDto>>(news);
            news = _newsRepository.GetAll(token);
            _cacheClient?.JsonSet("news", news);
            return _mapper.Map<IEnumerable<GetNewsDto>>(news);
        }

        public async Task<ChangeNewsOrderResult> ChangeNewsOrderAsync(ChangeNewsOrderDto changeNewsOrderDto,
            CancellationToken cancellationToken = default)
        {
            var group = await _newsGroupRepository.GetAsync(changeNewsOrderDto.GroupId, cancellationToken);
            var foundNews = group.OrderedNews.FirstOrDefault(x => x.News.Id == changeNewsOrderDto.NewsId);
            if (foundNews == null)
            {
                return new ChangeNewsOrderResult
                {
                    Error = "News not found",
                    IsSuccessful = false
                };
            }
            if (group.OrderedNews.Any(x=> x.Order == changeNewsOrderDto.Order))
            {
                return new ChangeNewsOrderResult
                {
                    Error = "Order is already taken",
                    IsSuccessful = false
                };
            }

            foundNews.Order = changeNewsOrderDto.Order;
            await _newsGroupRepository.UpdateAsync(group, cancellationToken);
            return new ChangeNewsOrderResult
            {
                IsSuccessful = true
            };
        }

        public async Task<UpdateNewsResult> UpdateNewsAsync(UpdateNewsDto dto, CancellationToken cancellationToken)
        {
            var foundNews = await _newsRepository.GetAsync(dto.Id, cancellationToken);
            if (foundNews != null)
            {
                foundNews.Description = dto.Description;
                foundNews.Headline = dto.Headline;
                foundNews.Link = dto.Link;
                foundNews.UpdateDate = DateTime.UtcNow;
                await _newsRepository.UpdateAsync(foundNews, cancellationToken);
                foreach (var usedGroupId in foundNews.UsedGroups)
                {
                    var group = await _newsGroupRepository.GetAsync(usedGroupId, cancellationToken);
                    var groupNews = @group?.OrderedNews.FirstOrDefault(x => x.News.Id == dto.Id);
                    if (groupNews != null)
                    {
                        groupNews.News = foundNews;
                    }

                    await _newsGroupRepository.UpdateAsync(group, cancellationToken);
                }
            }
            else
            {
                return new UpdateNewsResult()
                {
                    Error = "News not found"
                };
            }

            _cacheClient.RemoveKey("news");
            return new UpdateNewsResult()
            {
                IsSuccessful = true
            };
        }
    }
}