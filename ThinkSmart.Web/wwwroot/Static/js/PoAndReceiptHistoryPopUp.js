var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var batch_Roll;
$(document).ready(function () {
    getBatchRollFromConfig();
    GetMakerWisePurchaseOrderQtyList();
    GetVendorWisePurchaseOrderQtyList();

});
function getBatchRollFromConfig() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PoAndReceiptHistoryPopUp/GetBatchRollFromConfig",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            batch_Roll = response.message;
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetMakerWisePurchaseOrderQtyList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PoAndReceiptHistoryPopUp/GetMakerWisePurchaseOrderQtyList?Batch_Roll=" + batch_Roll, 
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
           
            var orderArray = [];
            //for (i = 0; i < result.length; i++) {
            //    if (result[i].internaL_NAME_STRING != result[i - 0].internaL_NAME_STRING) {
            //        orderArray.push(result[i].internaL_NAME_STRING);
            //        orderArray.push(result[i].matL_KUBUN_NAME);
            //    }
            //    else {
            //        orderArray.push(result[i].matL_KUBUN_NAME);
            //    }
            //}
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/MakerWisePurchaseOrderQtyListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {
                    debugger;
                    var result = response1.message;
                    $("#divMakerWisePurchaseOrderQtyList").html(response1.message);
                    //var next_Roll = batch_Roll; //batch_Roll + 2;logic changed
                    var resultRoll = calLastForecastRollHeaders(batch_Roll);
                    $("#lblLastRoll3").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R1));
                    $("#lblLastRoll2").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
                    $("#lblLastRoll1").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R3));
                    $("#lblCurrentRoll").text($("#lblCurrentRoll").text().replace("xx", batch_Roll));
                    $("#lblNextRoll").text($("#lblNextRoll").text().replace("xx", resultRoll.forecastRoll_R5));
                    $("#lblLastRoll13").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R1));
                    $("#lblLastRoll12").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
                    $("#lblLastRoll11").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R3));
                    $("#lblCurrentRoll1").text($("#lblCurrentRoll").text().replace("xx", batch_Roll));
                    $("#lblNextRoll1").text($("#lblNextRoll").text().replace("xx", resultRoll.forecastRoll_R5));
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetVendorWisePurchaseOrderQtyList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PoAndReceiptHistoryPopUp/GetVendorWisePurchaseOrderQtyList?Batch_Roll=" + batch_Roll,
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;

            var orderArray = [];
            //for (i = 0; i < result.length; i++) {
            //    if (result[i].internaL_NAME_STRING != result[i - 0].internaL_NAME_STRING) {
            //        orderArray.push(result[i].internaL_NAME_STRING);
            //        orderArray.push(result[i].matL_KUBUN_NAME);
            //    }
            //    else {
            //        orderArray.push(result[i].matL_KUBUN_NAME);
            //    }
            //}
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/VendorWisePurchaseOrderQtyListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {
                    debugger;
                    var result = response1.message;
                    $("#divVendorWisePurchaseOrderQtyList").html(response1.message);
                    //var next_Roll = batch_Roll; //batch_Roll + 2;logic changed
                    var resultRoll = calLastForecastRollHeaders(batch_Roll);
                    $("#lblLastRoll3").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R1));
                    $("#lblLastRoll2").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
                    $("#lblLastRoll1").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R3));
                    $("#lblCurrentRoll").text($("#lblCurrentRoll").text().replace("xx", batch_Roll));
                    $("#lblNextRoll").text($("#lblNextRoll").text().replace("xx", resultRoll.forecastRoll_R5));
                    $("#lblLastRoll13").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R1));
                    $("#lblLastRoll12").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
                    $("#lblLastRoll11").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R3));
                    $("#lblCurrentRoll1").text($("#lblCurrentRoll").text().replace("xx", batch_Roll));
                    $("#lblNextRoll1").text($("#lblNextRoll").text().replace("xx", resultRoll.forecastRoll_R5));
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
$("#btnRefreshData").click(function () {
    getBatchRollFromConfig();
    GetMakerWisePurchaseOrderQtyList();
    GetVendorWisePurchaseOrderQtyList();
});