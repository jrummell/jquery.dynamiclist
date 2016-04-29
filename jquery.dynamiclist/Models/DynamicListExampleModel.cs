using System.Collections.Generic;

namespace jquery.dynamiclist.Models
{
    public class DynamicListExampleModel
    {
        public string Ui { get; set; }

        public IList<DynamicListItemModel> ListItems { get; set; } = new List<DynamicListItemModel>();
        public IList<DynamicListItemModel> EmptyListItems { get; set; } = new List<DynamicListItemModel>();
        public IList<DynamicListItemModel> TableItems { get; set; } = new List<DynamicListItemModel>();
        public IList<DynamicListItemModel> DivItems { get; set; } = new List<DynamicListItemModel>();
    }
}