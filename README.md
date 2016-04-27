# jQuery Dynamiclist for MVC

A jQuery plugin for ASP.NET MVC that supports adding and removing items from a list with ajax.

You can install with NuGet:

## Kendo UI
http://nuget.org/List/Packages/jQuery.Dynamiclist.Kendo

    PM> Install-Package jQuery.Dynamiclist.Kendo

## bootstrap
http://nuget.org/List/Packages/jQuery.Dynamiclist.Bootstrap

    PM> Install-Package jQuery.Dynamiclist.Bootstrap

[![Build status](https://ci.appveyor.com/api/projects/status/l2gcqej17f39sb8u?svg=true)](https://ci.appveyor.com/project/jrummell/jquery-dynamiclist)

### New in version 2.0

- Removed support for jquery ui and jquery mobile
- Added templates
- newItemUrl option can now be a string or function

### Example Usage

    <h2>List Example</h2>
    <ul id="listExample" class="list">
        @Html.EditorFor(model => model.ListItems)
    </ul>

    <script src="~/Scripts/jquery.validate.unobtrusive.dynamic.js" type="text/javascript"> </script>
    <script src="~/Scripts/jquery.dynamiclist.js" type="text/javascript"> </script>
    <script src="~/Scripts/jquery.dynamiclist.templates.bootstrap.js" type="text/javascript"> </script>
    <script type="text/javascript">
        $("#listExample").dynamiclist(
            {
                // The ui framework. bootstrap, kendo, or none
                uiType: "bootstrap",
                // Selector for each item in the list
                itemSelector: ".item",
                // Label for the add button
                addLabel: "Add",
                // Label for the remove button
                removeLabel: "Remove",
                // ASP.NET MVC html field prefix
                htmlFieldPrefix: "@ViewData.TemplateInfo.HtmlFieldPrefix",
                // Model property that contains this list. Each item input is assumed to have a name of HtmlFieldPrefix.Property[index].BindingProperty
                property: "Items",
                // Action url for the new item partial view. This can be a string or function. The url should accept a htmlFieldPrefix parameter. E.g. Controller/Action?htmlFieldPrefix=Model.Property
                newItemUrl: "@Url.Action("NewListItem")",
                // list, table, or div
                listType: "list",
                // Occurs after an item is added to the list
                itemAdded: function (item) { },
                // Occurs after an item is removed from the list
                itemRemoved: function (item) { },
                // define custom templates if you need more control over the styling
                templates : {
                    table: {
                        container: "<div class='dynamic-list-container'><\/div>",
                        addItem: "<div class=\"add-item-container\"><button class=\"add-item\">{addLabel}<\/button><\/div>",
                        removeItem: "<button type=\"button\" class=\"delete-item\">{removeLabel}</button>"
                    },
                    list: {
                        container: "<div class='dynamic-list-container'><\/div>",
                        addItem: "<li class=\"add-item-container\"><button class=\"add-item\">{addLabel}<\/button><\/li>",
                        removeItem: "<button type=\"button\" class=\"delete-item\">{removeLabel}</button>"
                    },
                    div: {
                        container: "<div class='dynamic-list-container'><\/div>",
                        addItem: "<div class=\"add-item-container\"><button class=\"add-item\">{addLabel}<\/button><\/div>",
                        removeItem: "<button type=\"button\" class=\"delete-item\">{removeLabel}</button>"
                    }
                }
            });
    </script>
