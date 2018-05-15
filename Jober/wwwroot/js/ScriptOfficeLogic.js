"use strict";

$(document).ready(function () {

    $("[data-action]").click(loadOfficePages);

    addBtnClickEvent();

    addFilterLogic();

});

function loadOfficePages(evt) {
    evt.preventDefault();
    var $target = $(evt.currentTarget);
    if ($target.hasClass("active")) {
        return;
    }
    changeStateLoader("on");

    $("#j-office-nav>li").each(function (index, elem) {
        if ($(elem).hasClass("active")) {
            $(elem).removeClass("active");
        }
    });

    $("#j-office-content").load("/User/" + $target.data("action"), null,
        function () {
            $("[data-action=" + $target.data("action") + "]").addClass("active");
            addBtnClickEvent();
            addFilterLogic();
            changeStateLoader("off");
            $("a[data-action]").click(loadOfficePages);
        });
}

function addBtnClickEvent() {
    $(".j-getorder-btn, .j-revokeorder-btn").click(putOrder);
    $("#j-savesettings-btn").click(putWorkerSettings);
    $(".j-office-categorysetting").click(function (evt) {
        evt.preventDefault();
        $(evt.currentTarget).next().toggleClass("hidden");
    });
}

function putOrder(evt) {
    evt.preventDefault();
    var $target = $(evt.currentTarget);
    $target.prop("disabled", true);
    var $order = $target.parent().parent();
    var $data = $("#j-office-data");

    $.ajax({
        url: "/api/order/" + $order.data("order_id"),
        contentType: "application/json",
        method: "PUT",
        data: JSON.stringify({
            guid: $data.data("guid"),
            workerId: +($data.data("worker_id")),
            status: $target.hasClass("j-getorder-btn") ? 5 : 1
        }),
        success: function (order) {
            $order.parent().detach();
            $target.hasClass("j-getorder-btn") ? alert("Вы приняли заказ.") : alert("Вы отказались от заказа.");
        },
        error: function (jxqr, error, status) {
            alert("Ваша заявка отклонена.");
            var $page = $("#j-office-nav>li.active");
            $page.removeClass("active");
            $page.trigger('click');
        }
    });
}

function putWorkerSettings(evt) {
    evt.preventDefault();
    var $inputsCategoty = $("[name='categoryIds']:checked");
    var $inputsDistrict = $("[name='districtIds']:checked");
    var $data = $("#j-office-data");
    var cityId = +$("#j-worker-city").data("city_id");

    var categoryIds = [];
    $inputsCategoty.each(function (index, elem) {
        categoryIds.push(+elem.value);
    });

    var districtIds = [];
    $inputsDistrict.each(function (index, elem) {
        districtIds.push(+elem.value);
    });

    $.ajax({
        url: "/api/worker/",
        contentType: "application/json",
        method: "PUT",
        data: JSON.stringify({
            guid: $data.data("guid"),
            workerId: +($data.data("worker_id")),
            cityId: cityId,
            districtIds: districtIds,
            categoryIds: categoryIds
        }),
        success: function (order) {
            setWokerSettingToLocalStorage();
            alert("Настройки сохранены.");
        },
        error: function (jxqr, error, status) {
            alert("Ошибка. Настройки не сохранены.");
        }
    });
}

function setWokerSettingToLocalStorage() {
    if (window.localStorage) {
        let $categoryIds = [];
        $("input[name=categoryIds]").each(function (index, elem) {
            if ($(elem).prop("checked")) {
                $categoryIds.push($(elem).val());
            }
        });

        let $districtIds = [];
        $("input[name=districtIds]").each(function (index, elem) {
            if ($(elem).prop("checked")) {
                $districtIds.push($(elem).val());
            }
        });

        let $cityId = $("#j-worker-city").data("city_id");
        
        if ($categoryIds.length && $cityId != "" && $districtIds.length) {
            let settings = JSON.stringify({
                categoryIds: $categoryIds,
                cityId: String($cityId),
                districtIds: $districtIds
            });
            window.localStorage.setItem("WorkerSettingsJSON", settings);
            return true;
        }
        return false;
    }
}

function addFilterLogic() {
    var $sortFilterItems = $("#j-filter-sort + ul a[data-id]");
    $sortFilterItems.click(sortOrders);

    var $filterItems = $("#j-filter-district + ul a[data-id], #j-filter-category + ul a[data-id]");
    $filterItems.click(filteringOrders);
}

function sortOrders(evt) {
    evt.preventDefault();
    var $target = $(evt.currentTarget);
    var targetData = $target.data("id");
    var $sortFilter = $("#j-filter-sort");
    if (targetData === $sortFilter.data("id"))
        return;
    $sortFilter.data("id", targetData);
    var $orders = $("#j-office-flex-grid > div");

    $orders.sort(function (a, b) {
        let aData, bData;
        if (targetData === "sum") {
            aData = $(a).data("sum");
            bData = $(b).data("sum");
            if (aData && bData) {
                if (aData > bData)
                    return -1;
                if (aData < bData)
                    return 1;
            }
        }
        else {
            aData = $(a).data("date");
            bData = $(b).data("date");
            if (aData && bData) {
                if (aData > bData)
                    return 1;
                if (aData < bData)
                    return -1;
            }
        }
        return 0;
    });

    $sortFilter.html($target.text() + '<span class="caret"></span>');
    $orders.detach().appendTo($("#j-office-flex-grid"));
}

function filteringOrders(evt) {
    evt.preventDefault();
    var $target = $(evt.currentTarget);
    var targetData = $target.data("id");
    var $targetFilter = $target.parent().parent().prev();
    var $filter1 = $("#j-filter-district");
    var $filter2 = $("#j-filter-category");
    if (targetData === $targetFilter.data("id"))
        return;

    $targetFilter.data("id", targetData);
    var filter1Data = $filter1.data("id");
    var filter2Data = $filter2.data("id");
    var $orders = $("#j-office-flex-grid > div");

    $orders.each(function (index, elem) {
        var $elem = $(elem);
        if ((filter1Data === 0 || $elem.data("district_id") === filter1Data)
            && (filter2Data === 0 || $elem.data("category_id") === filter2Data)) {
            if ($elem.hasClass("hidden")) {
                $elem.removeClass("hidden");
            }
        }
        else {
            if (!$elem.hasClass("hidden")) {
                $elem.addClass("hidden");
            }
        }
    });

    $targetFilter.html($target.text() + '<span class="caret"></span>');
}