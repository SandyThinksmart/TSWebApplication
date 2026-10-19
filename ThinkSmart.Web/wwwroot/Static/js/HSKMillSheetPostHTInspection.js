var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var internal_quality_CD = "";
let selectedRow = false;
var postHTInspectionGridData = 0;

$(document).ready(function () {
    $.support.cors = true;
    var processPlanCD = getURLParameters(window.location.href).trim();
    GetHSKMillSheetPostHTInspectionDetails(processPlanCD);
});

function openScreens() {
    debugger;
    for (let i = 1; i <= postHTInspectionGridData.length; i++) {
        checked = $('#selectRow' + i).is(':checked');
        if (checked) {
            internal_quality_CD = $("#internal_quality_CD" + i).text();
            selectedRow = true;
            break;
        }                    
    }
    if (selectedRow) {
        window.close();
        window.open('/HSKMillSheet/HSKMillSheetSaveEdit?internal_quality_CD=' + internal_quality_CD, 'HSKMillSheetSaveEdit', 'height=1000px,width=2000px,resizable=no');
       
    }
    else {
        const focusFieldID = $(':focus').attr("id");
        const message = $.trim($("#lblErrorMessageE_SystemException").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }
}

function GetHSKMillSheetPostHTInspectionDetails(processPlanCD) {
   
    var ajaxTime = new Date().getTime();
  
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetHSKMillSheetPostHTInspectionDetails?processPlanCD="+processPlanCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            if (response.isSuccess) {
                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/HSKMillSheet/GetHSKMillSheetPostHTInspectionDetails",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        debugger;
                        var result = response1.message;
                        postHTInspectionGridData = response1.message;
                        if (response1.isSuccess) {
                            $("#divPostHTInspectionGrid").html(response1.message);

                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_SystemException").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });
            }
            else {
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
                    centerVertical: true
                });
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('ProcessPlanCD=');
    result = sParameterName[1];
    return result;
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