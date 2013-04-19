using System.Collections.Generic;

namespace jquery.dynamiclist.Models
{
    public class DynamicListExampleModel
    {
        public IList<DynamicListItemModel> ListItems { get; set; }
        public IList<DynamicListItemModel> TableItems { get; set; }
    }
}