# jQuery Dynamiclist for MVC

A jQuery plugin for MVC that supports adding and removing items from a list.

You can install with NuGet:

## jQuery UI
http://nuget.org/List/Packages/jQuery.Dynamiclist

    PM> Install-Package jQuery.Dynamiclist

## jQuery Mobile
http://nuget.org/List/Packages/jQuery.Dynamiclist.Mobile

    PM> Install-Package jQuery.Dynamiclist.Mobile

## Kendo UI
http://nuget.org/List/Packages/jQuery.Dynamiclist.Kendo

    PM> Install-Package jQuery.Dynamiclist.Kendo

## bootstrap
http://nuget.org/List/Packages/jQuery.Dynamiclist.Bootstrap

    PM> Install-Package jQuery.Dynamiclist.Bootstrap

[![Build status](https://ci.appveyor.com/api/projects/status/l2gcqej17f39sb8u?svg=true)](https://ci.appveyor.com/project/jrummell/jquery-dynamiclist)

    <h2>
        List Example</h2>
    <ul id="listExample" class="list">
        @for (int i = 0; i < Model.ListItems.Count; i++)
        {
            @Html.EditorFor(model => model.ListItems[i], "DynamicListItemModel")
        }
    </ul>

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
