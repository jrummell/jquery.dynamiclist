# jQuery Dynamiclist for MVC

A jQuery plugin for ASP.NET MVC that supports adding and removing items from a list with ajax.

## Installation

Install with npm:

```bash
> npm install jquery-dynamiclist
```

The npm version is a single package. Include the appropriate scripts based on your UI framework:

Base scripts for all frameworks:

- Unobtrusive validation with ajax support:
  - [`jquery.validate.unobtrusive.dynamic.js`](./src/jquery.validate.unobtrusive.dynamic.js)
- jQuery plugin:
  - [`jquery.dynamiclist.js`](./src/jquery.dynamiclist.js)

### Kendo UI

- Kendo UI templates:
  - [`jquery.dynamiclist.templates.kendo.js`](./src/jquery.dynamiclist.templates.kendo.js)

Note: Kendo UI is no longer a package dependency. You should either install it separately, or use their CDN.

### Bootstrap

- Bootstrap templates:
  - [`jquery.dynamiclist.templates.kendo.js`](./src/jquery.dynamiclist.templates.kendo.js)

[![Build Status](https://dev.azure.com/jrummell/jquery.dynamiclist/_apis/build/status/jrummell.jquery.dynamiclist?branchName=master)](https://dev.azure.com/jrummell/jquery.dynamiclist/_build/latest?definitionId=1&branchName=master)

Live example: <http://jquery-dynamiclist.azurewebsites.net/>

## New in version 2.0

- Removed support for jquery ui and jquery mobile
- Added templates
- newItemUrl option can now be a string or function
- Added add and remove methods
- Remove confirmation option that will display a confirm dialog if set

### Example Usage

```html
<h2>List Example</h2>
<ul id="listExample" class="list">
  @Html.EditorFor(model => model.ListItems)
</ul>

<!-- base scripts -->
<script
  src="~/Scripts/jquery.validate.unobtrusive.dynamic.js"
  type="text/javascript"
></script>
<script src="~/Scripts/jquery.dynamiclist.js" type="text/javascript"></script>

<!-- Bootstrap templates -->
<script
  src="~/Scripts/jquery.dynamiclist.templates.bootstrap.js"
  type="text/javascript"
></script>

<script type="text/javascript">
  // Initialize
  $("#listExample").dynamiclist(
      {
          // Selector for each item in the list
          itemSelector: ".item",
          // Label for the add button
          addLabel: "Add",
          // Label for the remove button
          removeLabel: "Remove",
          // ASP.NET MVC html field prefix
          htmlFieldPrefix: "@ViewData.TemplateInfo.HtmlFieldPrefix",
          // Model property that contains this list. Each item input is assumed to have a name of HtmlFieldPrefix.Property[index].BindingProperty
          property: "@Html.NameFor(m => m.Items)",
          // Action url for the new item partial view. This can be a string or function.
          // The url should accept a htmlFieldPrefix parameter. E.g. Controller/Action?htmlFieldPrefix=Model.Property
          newItemUrl: "@Url.Action("NewListItem")",
          // list, table, or div
          listType: "list",
          // Triggered after an item is added to the list
          itemAdded: function (item) { },
          // Triggered after an item is removed from the list
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

      // Methods

      // add
      $("#listExample").dynamiclist("add");

      // remove
      var $firstItem = $("#listExample .item:first");
      $("#listExample").dynamiclist("remove", $firstItem);
</script>
```
