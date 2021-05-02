using System.Collections.Generic;

namespace DemirorenCase.Contract.News.Response
{
    public class NewsGroupsViewModel
    {
        public IEnumerable<NewsGroupsListItemViewModel> NewsGroupsListItems { get; set; }
    }

    public class NewsGroupsListItemViewModel
    {
        public string GroupName { get; set; }
        public string Id { get; set; }
    }
}