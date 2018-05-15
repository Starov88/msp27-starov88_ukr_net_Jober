"use strict";
console.log("OK!");

$(document).ready(function () {

    $("#j-account-setting").click(loadUserPages);

});

function loadUserPages(evt) {
    changeStateLoader("on");
    var $target = $(evt.currentTarget).find("span");
    if ($target.hasClass("glyphicon-cog")) {
        $("#j-account-form").load("/user/indexwrite", null,
            function () {
                changeStateLoader("off");
                $("#j-account-setting").click(loadUserPages);
            });
    }
    else {
        $("#j-account-form").load("/user/indexread", null,
            function () {
                changeStateLoader("off");
                $("#j-account-setting").click(loadUserPages);
            });
    }
    
}