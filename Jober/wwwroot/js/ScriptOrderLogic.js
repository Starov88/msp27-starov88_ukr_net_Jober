"use strict";

$(document).ready(function () {

    var $serviceListItems = $("#j-services > li");
    var $serviceCheckboxes = $serviceListItems.find("input[type='checkbox']");
    var $totalCost = $("#j-totalcost");

    $serviceListItems.find("select.j-service-quantity").val(0);
    $serviceCheckboxes.click(onCheckboxClick);
    $serviceListItems.find("select.j-service-quantity").change(function (evt) {
        setTotalCost();
        console.log("Order:");
        console.log(getOrderJSON());
    });
    setTotalCost();

    function onCheckboxClick(evt) {

        var $targetElem = $(evt.currentTarget);
        var $selectNumber = $targetElem.next();

        if ($targetElem.prop('checked')) {
            $selectNumber.removeClass("j-hidden");
            $selectNumber.val($targetElem.val() + "-1");
        }
        else {
            $selectNumber.addClass("j-hidden");
            $selectNumber.val(0);
        }
        setTotalCost();
    }

    function setTotalCost() {

        var total = 0;
        $serviceCheckboxes.each(function (index, elem) {
            if ($(elem).prop('checked')) {
                var price = $(elem).data("price");
                var value = $(elem).next().val();
                if (value !== null)
                    total += price * (value.split('-')[1]);
            }
        });
        $totalCost.text("Сумма заказа: " + total.toFixed(2));
    }

    function getOrderJSON() {
        let $categoryId = $("input[name=SelectedCategoryId]").val();
        let $cityId = $("input[name=CityId]").val();
        let $districtId = $("select[name=DistrictId]").val();
        let $categoryName = $("input[name=SelectedCategoryName]").val();
        if ($categoryId != "" && $cityId != "" && $districtId != "") {
            return JSON.stringify({
                categoryId: $categoryId,
                cityId: $cityId,
                districtId: $districtId,
                message: "Поступил новый заказ в категории '" + $categoryName + "'."
            });
        }
        return false;
    }

    
    $("#j-order-submit").click(function (evt) {
        let message = getOrderJSON();
        if (message !== false) {
            hubConnection.invoke("Send", message);
        }
    });
    

});






