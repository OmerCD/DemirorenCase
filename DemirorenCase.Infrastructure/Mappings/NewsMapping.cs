using DemirorenCase.Core.Entities.Mongo;
using DemirorenCase.Infrastructure.Abstractions.DTO.News;
using Mapster;

namespace DemirorenCase.Infrastructure.Mappings
{
    public class NewsMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CreateNewsGroupDto, NewsGroup>()
                .Map(group => group.Name, dto => dto.GroupName);
            config.ForType<NewsGroup, GetNewsGroupDto>()
                .Map(dto => dto.GroupName, group => group.Name);
            config.ForType<NewsGroup, GetNewsGroupListItemDto>()
                .Map(dto => dto.GroupName, group => group.Name);
            
            
        }
    }
}