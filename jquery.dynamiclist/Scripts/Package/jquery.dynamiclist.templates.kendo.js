/// <reference path="jquery.dynamiclist.js"/>

(function(jQuery)
{
    //
    // plugin defaults
    //
    jQuery.fn.dynamiclist.defaults.templates = {
        table: {
            container: "<div class='dynamic-list-container k-content'><\/div>",
            addItem: "<div class=\"add-item-container\"><button class=\"add-item k-button k-button-icontext\"><span class='k-icon k-i-plus'><\/span>{addLabel}<\/button><\/div>",
            removeItem: "<button type=\"button\" class=\"delete-item k-button k-button-icontext\"><span class='k-icon k-i-cancel'><\/span>{removeLabel}</button>"
        },
        list: {
            container: "<div class='dynamic-list-container k-content'><\/div>",
            addItem: "<li class=\"add-item-container k-header\"><button class=\"add-item k-button k-button-icontext\"><span class='k-icon k-i-plus'><\/span>{addLabel}<\/button><\/li>",
            removeItem: "<button type=\"button\" class=\"delete-item k-button k-button-icontext\"><span class='k-icon k-i-cancel'><\/span>{removeLabel}</button>"
        },
        div: {
            container: "<div class='dynamic-list-container k-content'><\/div>",
            addItem: "<div class=\"add-item-container k-header\"><button class=\"add-item k-button k-button-icontext\"><span class='k-icon k-i-plus'><\/span>{addLabel}<\/button><\/div>",
            removeItem: "<button type=\"button\" class=\"delete-item k-button k-button-icontext\"><span class='k-icon k-i-cancel'><\/span>{removeLabel}</button>"
        }
    };
})(jQuery);