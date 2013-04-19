using System.Web.Mvc;
using jquery.dynamiclist.Models;

namespace jquery.dynamiclist.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var listItems = new[]
                            {
                                new DynamicListItemModel {Id = 1, Name = "One"},
                                new DynamicListItemModel {Id = 2, Name = "Two"}
                            };
            var tableItems = new[]
                             {
                                 new DynamicListItemModel {Id = 3, Name = "Three"},
                                 new DynamicListItemModel {Id = 4, Name = "Four"}
                             };
            DynamicListExampleModel model = new DynamicListExampleModel {ListItems = listItems, TableItems = tableItems};
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DynamicListExampleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ViewBag.Success = true;

            return View(model);
        }

        public PartialViewResult NewListItem(string htmlFieldPrefix)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;

            return PartialView(new DynamicListItemModel());
        }

        public PartialViewResult NewTableItem(string htmlFieldPrefix)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;

            return PartialView(new DynamicListItemModel());
        }
    }
}