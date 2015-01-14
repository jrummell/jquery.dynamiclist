# jQuery Dynamiclist for MVC

A jQuery UI plugin for MVC that supports adding and removing items from a list.


    <h2>
        List Example</h2>
    <ul id="listExample" class="list">
        @for (int i = 0; i < Model.ListItems.Count; i++)
        {
            @Html.EditorFor(model => model.ListItems[i], "DynamicListItemModel")
        }
    </ul>
    <h2>
        Empty List Example</h2>
    <ul id="emptyListExample" class="list">
    </ul>
    <p>
        <input type="submit" value="Save" />
    </p>

    <script src="~/Scripts/jquery.validate.unobtrusive.dynamic.js" type="text/javascript"> </script>
    <script src="~/Scripts/jquery.dynamiclist.js" type="text/javascript"> </script>
    <script type="text/javascript">
            $("#listExample").dynamiclist(
                {
                    itemSelector: "li.item", // Selector for each item in the list
                    addLabel: "Additional", // Label for the add button
                    removeLabel: "Remove", // Label for the remove button
                    htmlFieldPrefix: "@ViewData.TemplateInfo.HtmlFieldPrefix", // ASP.NET MVC html field prefix
                    property: "ListItems", // Model property that contains this list. Each item input is assumed to have a name of HtmlFieldPrefix.Property[index].BindingProperty
                    newItemUrl: '@Url.Action("NewListItem")', // Action url for the new item partial view. It should accept a htmlFieldPrefix parameter. E.g. Controller/Action?htmlFieldPrefix=Model.Property
                    listType: "list", // list or table
                    itemAdded: function(item)
                    {
                        // Occurs after an item is added to the list
                    },
                    itemRemoved: function(item)
                    {
                        // Occurs after an item is removed from the list
                        $("#listExample").after(" Item " + $("input:first", item).val() + " removed.");
                    }
                });
    </script>



[Continuous Integration](http://teamcity.codebetter.com/project.html?projectId=project346) provided by Codebetter:

![TeamCity](http://www.jetbrains.com/img/banners/Codebetter.png)

[keyboard-centric bug tracker](http://www.jetbrains.com/youtrack "YouTrack by JetBrains"),
[continuous integration server](http://www.jetbrains.com/teamcity "TeamCity by JetBrains")
