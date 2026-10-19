var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var finalInspectionGridData = 0;

$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    debugger;
    //Get Process plan CD 
    var processPlanCD = getURLParameters(window.location.href).trim();
    if (processPlanCD != "") {
        GetFinalInspectionDetailsByProcessPlanCD(processPlanCD);
    }
});

//Get Process Plan CD by url. 
function getURLParameters(url) {
    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('ProcessPlanCD=');
    result = sParameterName[1];
    return result;
}

function GetFinalInspectionDetailsByProcessPlanCD(ProcessPlanCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetHSKMillSheetFinalInspectionDetails?processPlanCD=" + ProcessPlanCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            finalInspectionGridData = response.message;
            //debugger;
            //finalInspectionGridData = response.message;
            //if (response.isSuccess) {
            //    $("#divHSKMillSheetFinalInspectionList").html(response.message);

            //}
            if (response.isSuccess) {
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/HSKMillSheet/GetHSKMillSheetFinalInspectionDetails",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divHSKMillSheetFinalInspectionList").html(response1.message);
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
          
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
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


$("#btnChuubanSearch").click(function (event) {
    debugger;
    for (let i = 1; i <= finalInspectionGridData.length; i++) {
        checked = $('#selectRow' + i).is(':checked');

        if (checked) {
            finalInsp_CD = $("#FINAL_INSP_CD" + i).text();
            selectedRow = true;
            break;
        }
    }
    if (selectedRow) {
        // window.open("/HSKMillSheet/HSKMillSheetSaveEdit?finalInspCD=" + finalInsp_CD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1240 + ", height=" + 550);
        window.location.href = "/HSKMillSheet/HSKMillSheetSaveEdit?finalInspCD=" + finalInsp_CD;
    }
    else {
        const focusFieldID = $(':focus').attr("id");
        const message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }
});