using System;

namespace DemirorenCase.Contract.News.Response
{
    public class NewsViewModel
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