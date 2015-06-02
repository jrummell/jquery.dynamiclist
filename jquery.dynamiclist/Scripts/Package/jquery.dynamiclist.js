/// <reference path="jquery-1.9.1.js" />
/// <reference path="jquery.mobile-1.3.1.js" />
/// <reference path="jquery-ui-1.10.2.js" />
/// <reference path="kendo/2013.1.319/kendo.web.min.js" />
/// <reference path="jquery.validate.unobtrusive.dynamic.js" />

(function(jQuery)
{
    var uiTypes = {
        mobile: "mobile",
        kendo: "kendo",
        ui: "ui",
        bootstrap: "bootstrap",
        none: "none"
    };

    //
    // plugin methods
    //
    var methods = {
        init: function(options)
        {
            options = jQuery.extend({}, jQuery.fn.dynamiclist.defaults, options);

            return this.each(function()
            {
                var $list = jQuery(this);
                var data = $list.data("dynamiclist");

                // only initialize once
                if (!data)
                {
                    $list.wrap("<div class='dynamic-list-container'><\/div>");
                    if (options.uiType == "ui")
                    {
                        $list.parent().addClass("ui-widget ui-widget-content ui-corner-all");
                    }
                    $list.addClass("dynamic-list");

                    options.selector = $list.selector;

                    // add the delete column
                    if (options.listType == "table")
                    {
                        jQuery("thead tr", $list).append("<th><\/th>");
                        jQuery("tbody tr", $list).append("<td class='delete-cell'><\/td>");
                        jQuery("tfoot tr", $list).append("<td><\/td>");
                    }

                    // init existing items
                    jQuery(options.itemSelector, $list).each(function(i)
                    {
                        var htmlFieldPrefix = methods._getIndexName(options.htmlFieldPrefix, options.property);
                        methods._appendRemoveAndIndex(jQuery(this), i, htmlFieldPrefix, options.removeLabel, options.uiType, options.listType, options.itemRemoved);
                    });

                    // add the add button
                    if (options.listType == "table")
                    {
                        var colspan = $list.find("thead td, thead th").length;
                        var $tfoot = $list.find("tfoot");
                        if ($tfoot.length == 0)
                        {
                            $list.append("<tfoot><\/tfoot>");
                            $tfoot = $list.find("tfoot");
                        }
                        $tfoot.append('<tr><td colspan="' + colspan + '" class="add-item-container ui-widget-header k-header ui-corner-bottom"><button class="add-item">' + (options.addLabel || "") + '<\/button><\/td><\/tr>');
                    }
                    else
                    {
                        $list.append('<li class="add-item-container ui-widget-header k-header ui-corner-bottom" data-role="list-divider"><button class="add-item">' + (options.addLabel || "") + '<\/button><\/li>');
                    }

                    if (options.uiType == uiTypes.mobile)
                    {
                        $list.attr("data-inset", "true");
                        $list.listview();
                    }

                    var $button = $list.find(".add-item");
                    var hasLabel = options.addLabel != null && options.addLabel != "";
                    if (options.uiType == uiTypes.mobile)
                    {
                        $button.buttonMarkup({ icon: "plus", iconpos: hasLabel ? "left" : "notext" });
                    }
                    else if (options.uiType == uiTypes.kendo)
                    {
                        $button.prepend("<span class='k-icon k-i-plus'><\/span>");
                    }
                    else if (options.uiType == uiTypes.ui)
                    {
                        var addButtonOptions = { icons: { primary: "ui-icon-new ui-icon-circle-plus" } };
                        if (hasLabel)
                        {
                            addButtonOptions.label = options.addLabel;
                        }
                        else
                        {
                            addButtonOptions.text = false;
                        }
                        $button.button(addButtonOptions);
                    }
                    else if (options.uiType == uiTypes.bootstrap)
                    {
                        $button.addClass("btn btn-default");
                        $button.prepend("<span class='glyphicon glyphicon-plus'><\/span>");
                    }

                    $button.click(function()
                    {
                        jQuery(this).prop("disabled", true);

                        // build the html field prefix
                        var index = methods._getNewIndex($list, options.itemSelector);
                        var htmlFieldPrefix = methods._getNewItemPrefix(options.htmlFieldPrefix, options.property, index);
                        // append the new item via ajax
                        var getUrl = options.newItemUrl;
                        if (getUrl.indexOf("?") < 0)
                        {
                            getUrl += "?";
                        }
                        jQuery.get(getUrl + '&htmlFieldPrefix=' + htmlFieldPrefix, function(html)
                        {
                            if (options.listType == "table")
                            {
                                $list.find("tbody").append(html);
                                $list.find(options.itemSelector + ":last").append("<td class='delete-cell'>");
                            }
                            else
                            {
                                var $lastItem = $list.find(options.itemSelector + ":last");
                                if ($lastItem.length > 0)
                                {
                                    $lastItem.after(html);
                                }
                                else
                                {
                                    $list.prepend(html);
                                }
                            }

                            var $item = jQuery(options.itemSelector + ":last", $list);
                            var indexName = methods._getIndexName(options.htmlFieldPrefix, options.property);
                            methods._appendRemoveAndIndex($item, index, indexName, options.removeLabel, options.uiType, options.listType, options.itemRemoved);

                            if (jQuery.validator && jQuery.validator.unobtrusive && jQuery.validator.unobtrusive.parseDynamicContent)
                            {
                                jQuery.validator.unobtrusive.parseDynamicContent($item);
                            }

                            if (options.uiType == uiTypes.mobile)
                            {
                                jQuery(jQuery.mobile.textinput.prototype.options.initSelector, $item).textinput();
                                $list.listview('refresh');
                            }

                            if (options.itemAdded)
                            {
                                options.itemAdded($item);
                            }

                            $button.prop("disabled", false);
                        }).error(function(xhr)
                        {
                            if (xhr.status == 401)
                            {
                                // let the user know that their session timed out.
                                alert("Your session has timed out. Please refresh the page and try again.");
                            }
                        });

                        return false;
                    });

                    // save options
                    $list.data("dynamiclist", options);

                    if (options.itemAdded)
                    {
                        $list.each(function (i, list)
                        {
                            $(list).each(function(j, item)
                            {
                                options.itemAdded(item);
                            });
                        });
                    }
                }
            });
        },
        _appendRemoveAndIndex: function($item, index, indexName, removeLabel, uiType, listType, itemRemoved)
        {
            /// <summary>Appends the remove button and hidden field for the item index.</summary>

            var buttonHtml = '<button type="button" class="delete-item">' + (removeLabel || "") + '</button>';

            // This hidden index field is required so that we can dynamically remove items from the middle of the list.
            // If we didn't have this, we'd have to use sequential indexes and MVC would only bind some values.
            // See http://haacked.com/archive/2008/10/23/model-binding-to-a-list.aspx
            var hiddenHtml = '<input type="hidden" name="' + indexName + '" class="index" value="' + index + '" />';
            var buttonAndHiddenHtml = buttonHtml + hiddenHtml;

            if (listType == "table")
            {
                $item.find("td.delete-cell").append(buttonAndHiddenHtml);
            }
            else
            {
                $item.append(buttonAndHiddenHtml);
            }


            // delete button handler
            // IE7 has a problem with using this with live(), other wise I would use that instead
            jQuery(".delete-item", $item).click(function()
            {
                $item.remove();

                if (itemRemoved)
                {
                    itemRemoved($item);
                }
            });

            var hasLabel = removeLabel != null && removeLabel != "";
            if (uiType == uiTypes.mobile)
            {
                jQuery(".delete-item", $item).buttonMarkup({ icon: "delete", iconpos: "notext", inline: "true" });
            }
            else if (uiType == uiTypes.kendo)
            {
                jQuery(".delete-item", $item).prepend("<span class='k-icon k-i-cancel'><\/span>");
            }
            else if (uiType == uiTypes.ui)
            {
                var removeButtonOptions = { text: hasLabel, label: removeLabel, icons: { primary: "ui-icon-delete ui-icon-trash" } };
                jQuery(".delete-item", $item).button(removeButtonOptions);
            }
            else if (uiType == uiTypes.bootstrap)
            {
                jQuery(".delete-item", $item).addClass("btn btn-danger").prepend("<span class='glyphicon glyphicon-remove'><\/span>");
            }
        },
        _getBaseItemPrefix: function(baseHtmlFieldPrefix, property)
        {
            // build the html field prefix
            var prefix = baseHtmlFieldPrefix;
            if (prefix != "")
            {
                prefix += ".";
            }
            prefix += property;

            return prefix;
        },
        _getNewItemPrefix: function(baseHtmlFieldPrefix, property, index)
        {
            return methods._getBaseItemPrefix(baseHtmlFieldPrefix, property) + "[" + index + "]";
        },
        _getIndexName: function(baseHtmlFieldPrefix, property)
        {
            return methods._getBaseItemPrefix(baseHtmlFieldPrefix, property) + ".Index";
        },
        _getNewIndex: function($list, itemSelector)
        {
            /// <summary>Returns a unique index whose value is greater than any existing index.</summary>

            var $items = jQuery(itemSelector, $list);
            if ($items.length == 0)
            {
                return 0;
            }

            var maxIndex = 0;
            $items.each(function()
            {
                var index = parseInt(jQuery("input.index", jQuery(this)).val());
                if (isNaN(index))
                {
                    index = 0;
                }

                maxIndex = Math.max(maxIndex, index);
            });

            return maxIndex + 1;
        },
        options: function(options)
        {
            return this.each(function()
            {
                var $list = jQuery(this);
                var currentOptions = $list.data("dynamiclist") || {};
                options = jQuery.extend({}, currentOptions, options);
                $list.dynamiclist("destroy").dynamiclist("init", options);
            });
        },
        add: function()
        {
            return this.each(function()
            {
                var $list = jQuery(this);
                $list.find(".add-item").click();
            });
        },
        destroy: function()
        {
            return this.each(function()
            {
                var $list = jQuery(this);

                //TODO: restore items to original state

                $list.removeData("dynamiclist");
            });
        }
    };

    jQuery.fn.dynamiclist = function(method)
    {
        /// <summary>Creates the necessary elements for a list of items with support for adding and removing items.</summary>

        if (methods[method])
        {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof(method) === 'object' || !method)
        {
            return methods.init.apply(this, arguments);
        }
        else
        {
            jQuery.error("Method " + method + " does not exist on jQuery.dynamiclist");
        }
    };

    //
    // plugin defaults
    //
    jQuery.fn.dynamiclist.defaults = {
        uiType: (typeof(jQuery.mobile) != "undefined") ? "mobile" : (typeof(kendo) != "undefined") ? "kendo" : (typeof(jQuery.ui) != "undefined") ? "ui" : "bootstrap", // The ui framework
        itemSelector: ".item", // Selector for each item in the list
        addLabel: "Additional", // Label for the add button
        removeLabel: "Remove", // Label for the remove button
        htmlFieldPrefix: "", // ASP.NET MVC html field prefix
        property: "Items", // Model property that contains this list. Each item input is assumed to have a name of HtmlFieldPrefix.Property[index].BindingProperty
        newItemUrl: "NewItem", // Action url for the new item partial view. It should accept a htmlFieldPrefix parameter. E.g. Controller/Action?htmlFieldPrefix=Model.Property
        listType: "list", // list or table
        itemAdded: function(item)
        {
        }, // Occurs after an item is added to the list
        itemRemoved: function(item)
        {
        } // Occurs after an item is removed from the list
    };
})(jQuery);