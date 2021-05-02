using System;

namespace DemirorenCase.Infrastructure.Abstractions.DTO.News
{
    public class GetNewsDto
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}