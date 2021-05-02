using System.Collections.Generic;
using System.Linq;
using DemirorenCase.Contract.News.Response;
using DemirorenCase.Domain.Queries.News;
using Mapster;

namespace DemirorenCase.API.Mappings
{
    public class NewsMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<GetNewsGroupsQueryResponse, NewsGroupsViewModel>()
                .Map(model => model.NewsGroupsListItems, response => response.GetNewsGroupsQueryResponseItems);
            config.ForType<GetAllNewsQueryResponse, IEnumerable<NewsViewModel>>()
                .MapWith(response => response.News.Select(dto=> new NewsViewModel()
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    CreateDate = dto.CreateDate,
                    IsDeleted = dto.IsDeleted,
                    Headline = dto.Headline,
                    UpdateDate = dto.UpdateDate,
                    Link = dto.Link
                }));
        }
    }
}