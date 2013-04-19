using System.ComponentModel.DataAnnotations;

namespace jquery.dynamiclist.Models
{
    public class DynamicListItemModel
    {
        [Required, Range(1, 5)]
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Name { get; set; }
    }
}