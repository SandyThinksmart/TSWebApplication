var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var batch_Roll;
$(document).ready(function () {
    getBatchRollFromConfig();
    GetMatlNameWiseOrderQty();
});
function getBatchRollFromConfig() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MatlNameWiseOrderQty/GetBatchRollFromConfig",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            batch_Roll = response.message;
            var next_Roll = batch_Roll; //batch_Roll + 2;logic changed
            var resultRoll = calLastForecastRollHeaders(next_Roll);
            $("#lblLastRoll3").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R1));
            $("#lblLastRoll2").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
            $("#lblLastRoll1").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R3));
            $("#lblCurrentRoll").text($("#lblCurrentRoll").text().replace("xx", batch_Roll));
            $("#lblNextRoll").text($("#lblNextRoll").text().replace("xx", resultRoll.forecastRoll_R5));
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetMatlNameWiseOrderQty() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MatlNameWiseOrderQty/GetMatlNameWiseOrderQty?Batch_Roll=" + batch_Roll,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
            var orderArray = [];
            for (i = 0; i < result.length; i++) {
                if (result[i].internaL_NAME_STRING != result[i - 0].internaL_NAME_STRING) {
                    orderArray.push(result[i].internaL_NAME_STRING);
                    orderArray.push(result[i].matL_KUBUN_NAME);
                }
                else {
                    orderArray.push(result[i].matL_KUBUN_NAME);
                }
            }
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/MatlNameWiseOrderQtyListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {
                    debugger;
                    var result = response1.message;
                    $("#divMatlNameWiseOrderQtyList").html(response1.message);
                    getBatchRollFromConfig();
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
    GetMatlNameWiseOrderQty();
});
function showMessageDialog(message, focusFieldID, messageType) {

    $("#dialogMessageIcon img").hide();
    switch (messageType) {
        case WARNING:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true,
                callback: function () {
                    if (focusFieldID !== "") {
                        $("#" + focusFieldID).focus();
                    }
                }
            });
            break;
        case ERROR:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/error.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true,
                callback: function () {
                    if (focusFieldID !== "") {
                        $("#" + focusFieldID).focus();
                    }
                }
            });
            break;
        case SUCCESS:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true,
                callback: function () {
                    if (focusFieldID !== "") {
                        $("#" + focusFieldID).focus();
                    }
                }
            });
            break;
        default:
            break;
    };


}