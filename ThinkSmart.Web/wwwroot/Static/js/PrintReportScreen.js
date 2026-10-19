//const { debug } = require("util");

var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";

$(document).ready(function () {
    // setLocation($("#hdfLoginId").val());
    if ($("#hdfLocationName").val() != "") {
        $("#txtLocation").val($("#hdfLocationName").val());
    }
});
$(window).on("load", function () {
    // setLocation($("#hdfLoginId").val());
    if ($("#hdfLocationName").val() != "") {
        $("#txtLocation").val($("#hdfLocationName").val());
    }
    
});

function setLocation(USER_ID, $callFrom) {
    debugger;

    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }

    var ajaxTime = new Date().getTime();
    // var jsonData = "{USER_ID: '" + USER_ID + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetLocation?USER_ID=" + USER_ID,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;
            var jsonResult = jResult[0];
            $('#txtLocation').val(jsonResult.name);

            // bindlocationdropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
$("#btnPrint").click(function (event) {
    debugger;
    var userId = $("#hdfLoginId").val();
var ajaxTime = new Date().getDate();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPProcessData/IsWIPProcessDataExist?userId=" + userId, 
        method: "POST",
        contentType: "application/json; charset=utf-8",
       // data: JSON.stringify({ userID: userId })
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            var jsonResult = response;
            if (jsonResult.isSuccess != true) {
                var message = $.trim($("#lblNoDataToPrint").text());
                showMessageDialog(message, "", WARNING);

            }
            else {
                debugger;
                
                    
                    var userID = $("#hdfLoginId").val();
                var myWindow = window.open(REPORTURL + "/Report/R_WIPProductProcess.aspx?userID=" + userId, "width=1940,height=1140", "Resizable = no"); 
               
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lbl_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


   


});