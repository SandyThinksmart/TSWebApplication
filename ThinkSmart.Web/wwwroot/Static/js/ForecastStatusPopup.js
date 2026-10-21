var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
$(document).ready(function () {

    GetSalesForecastDataList();
});
//Bind RollSalesForcast data on ForcastStatusPopup Screen  
function GetSalesForecastDataList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ForecastStatusPopup/GetSalesForecastStatusData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/ForecastStatusListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {
                    debugger;
                    var result = response1.message;
                    $("#divForecastStatusDataList").html(response1.message);
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



