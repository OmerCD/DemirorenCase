using System;
using System.Collections.Generic;

namespace DemirorenCase.Contract.News.Response
{
    public class NewsGroupViewModel
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<NewsGroupOrderedNewsItemViewModel> OrderedNews { get; set; }
    }

    public class NewsGroupOrderedNewsItemViewModel
    {
        public NewsGroupNewsItemViewModel News { get; set; }
        public int Order { get; set; }
    }

    public class NewsGroupNewsItemViewModel
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}