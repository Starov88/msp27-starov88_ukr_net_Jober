//import { setTimeout } from "timers";

"use strict";

var hubUrl = document.location.protocol + "//" + document.location.host + "/orderhub";
var httpConnection = new signalR.HttpConnection(hubUrl);
var hubConnection = new signalR.HubConnection(httpConnection);
hubConnection.start();

$(document).ready(function () {
    // Navigation
    $("[data-category_id]").click(loadAjaxPage);

    $("#j-loginmenu > span > ul > li > a").click(function () { changeStateLoader("on"); });

    $("#j-toggle-menu").click(function (evt) {
        $("#j-side-menu").toggleClass("hidden-xs");
        $("#j-side-menu").toggleClass("hidden-sm");
    });

    $("#j-robot").click(touchRobot);
    setInterval(touchRobot, 60000);

    $("#j-logout").click(logOut);

    hubConnection.on("Send", function (data) {
        console.log("signal");
        
        if (window.localStorage) {
            if (window.localStorage["WorkerSettingsJSON"]) {

                var objWorkerSetting = JSON.parse(window.localStorage["WorkerSettingsJSON"]);
                if (objWorkerSetting) {

                    let objOrderInfo = JSON.parse(data);
                    if (objOrderInfo["cityId"] == objWorkerSetting["cityId"]) {

                        var districts = objWorkerSetting["districtIds"];
                        for (var i = 0; i < districts.length; i++) {

                            if (districts[i] == objOrderInfo["districtId"]) {
                                var categories = objWorkerSetting["categoryIds"];
                                for (var i2 = 0; i2 < categories.length; i2++) {

                                    if (categories[i2] == objOrderInfo["categoryId"]) {
                                        let $dialog = $("#j-robot-dialog");
                                        $dialog.text(objOrderInfo["message"]);
                                        $dialog.removeClass("hidden");
                                        //beep();
                                        setTimeout(function () { $dialog.addClass("hidden"); }, 8000);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    });
});

function beep() {
    //$("#j-beep").attr("autoplay", true);
    //var audio = new Audio();
    //audio.src = "beep.waw";
    //audio.autoplay = true;
    //$("#j-beep").removeAttr("autoplay");
}

function loadAjaxPage(evt) {
    
    var $target = $(evt.currentTarget);
    changeStateLoader("on");
    if ($target.hasClass("j-submenu-item"))
        return;
    evt.preventDefault();
    
    $("#j-content").load('/?category=' + $target.data("category_id") + ' #j-flex-grid', null,
        function () {
            menuActiveLogic($target);
            changeStateLoader("off");
            $("a[data-category_id]").click(loadAjaxPage);
            $(".j-sub-category").click(function () {
                changeStateLoader("on");
            });
            var text = $target.text() == "" ? "Главная" : $target.text();
            $("#j-content").prev().text(text);
        });
}

function menuActiveLogic($activeMenuItem) {
    var $menuItems = $(".j-side-menu-item");
    $menuItems.each(function (indx, element) {
        if ($(element).data("category_id") === $activeMenuItem.data("category_id")) {
            if (!$(element).hasClass("active")) {
                $(element).addClass("active");
            }
            var subMenu = $(element).next();
            if (subMenu.hasClass("j-submenu-hidden")) {
                subMenu.removeClass("j-submenu-hidden");
            }
            subMenu.children().removeClass("active");
        }
        else {
            if ($(element).hasClass("active")) {
                $(element).removeClass("active");
            }
            if (!$(element).next().hasClass("j-submenu-hidden")) {
                $(element).next().addClass("j-submenu-hidden");
            }
        }
    });
}

function changeStateLoader(state, $clearElem = null) {

    if ($clearElem !== null)
        $clearElem.html("");

    if (state === "on" || state === true) {
        $("#j-loader, #j-loader-shade").removeClass("hidden");
    }
    else {
        $("#j-loader, #j-loader-shade").addClass("hidden");
    }
}

var timerId;
function touchRobot(evt) {
    var $target = $("#j-robot");
    var imgPath = "/images/animation/" + getRandomInt(1,4) +".gif";
    $target.attr("src", imgPath);
    if (timerId)
        clearTimeout(timerId);
    timerId = setTimeout(function () { $target.attr("src", "/images/animation/0.gif"); }, 10000);
}

function logOut(evt) {
    $.ajax({
        url: "/Account/Logout",
        method: "POST",
        success: function () {
        },
        error: function (jxqr, error, status) {
            alert("Ошибка при выходе");
        }
    });
}

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}



