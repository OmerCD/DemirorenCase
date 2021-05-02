using System.Collections.Generic;
using System.Linq;
using DemirorenCase.Domain.Queries.News;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using Mapster;

namespace DemirorenCase.Domain.Mappings
{
    public class NewsMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<IEnumerable<GetNewsGroupListItemDto>, GetNewsGroupsQueryResponse>()
                .MapWith(dtos => new GetNewsGroupsQueryResponse()
                {
                    GetNewsGroupsQueryResponseItems = dtos.Select(dto => new GetNewsGroupsQueryResponseItem
                    {
                        Id = dto.Id,
                        GroupName = dto.GroupName
                    }) 
                }).TwoWays();
            config.ForType<IEnumerable<GetNewsDto>, GetAllNewsQueryResponse>()
                .MapWith(dtos => new GetAllNewsQueryResponse
                {
                    News = dtos.Select(dto => new GetAllNewsQueryListItemResponse
                    {
                        Id = dto.Id,
                        Description = dto.Description,
                        CreateDate = dto.CreateDate,
                        IsDeleted = dto.IsDeleted,
                        Headline = dto.Headline,
                        UpdateDate = dto.UpdateDate,
                        Link = dto.Link
                    })
                });
        }
    }
}