using System.Collections.Generic;

namespace DemirorenCase.Infrastructure.Abstractions.DTO.News
{
    public class GetNewsGroupDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<GetOrderedNewsDto> OrderedNews { get; set; }
    }
}