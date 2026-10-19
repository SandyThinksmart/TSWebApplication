var isStartDateValid = false;

$(document).ready(function () {

    if ($("#hdfLocationName").val() != "")
    {
        $("#txtLocation").val($("#hdfLocationName").val());
    }
    $("#LoadingImage").hide();
    $("#txtStartDate").focus();
    Mask();
    setSysDateTime();
   
    
});
$(window).on("load", function () {
   
    if ($("#hdfLocationName").val() != "") {
        $("#txtLocation").val($("#hdfLocationName").val());
    }
 
});

function Mask() {
    $("#txtStartDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtEndTime").mask("99:99", { placeholder: "HH:MM" });
}
$("#txtStartDate").change(function () {
    if ($("#txtStartDate").val() == "") {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_001").text().trim(), "txtStartDate", WARNING);
        return false;
    }
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetEndDateByStartDate?date=" + $("#txtStartDate").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (result.starT_DATE == "" && result.enD_DATE == "") {
                showMessageDialog($("#lblErrorMessageE_Hikinuki_002").text().trim(), "txtStartDate", WARNING);
                return false;
            }

            if (result) {
                $("#txtStartDate").val($.format.date(new Date(result.starT_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
                $("#txtEndDate").val($.format.date(new Date(result.enD_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
                $("#txtEndTime").val($.format.date(new Date(result.enD_DATE), "HH:mm")).mask("99:99", { placeholder: "HH:MM" });
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Hikinuki").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
});
function setSysDateTime() {
    var ajaxTime = new Date().getTime();
  
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetDefaultSystemDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (result) {
                $("#txtStartDate").val($.format.date(new Date(result.starT_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
                $("#txtEndDate").val($.format.date(new Date(result.enD_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
                $("#txtEndTime").val($.format.date(new Date(result.enD_DATE), "HH:mm")).mask("99:99", { placeholder: "HH:MM" });
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Hikinuki").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

$("#btnReprint").click(function () {
    if ($("#txtStartDate").val() == "") {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_001").text().trim(), "txtStartDate", WARNING);
        return false;

    }
    if ($("#txtEndTime").val() == "") {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_005").text().trim(), "txtEndTime", WARNING);
        return false;

    }

    var startDate = checkDate($("#txtStartDate").val());

    if (startDate == false) {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_002").text().trim(), "txtStartDate", WARNING);
        return false;
    }
    var endDateTime = checkDate($("#txtEndDate").val() + " " + $("#txtEndTime").val());

    if (endDateTime == false) {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_006").text().trim(), "txtEndTime", WARNING);
        return false;
    }

    var endDateT = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/ReprintReport?startDate=" + $("#txtStartDate").val() + "&endDate=" + endDateT + "&userID=" + $("#hdfLoginId").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
          
            if (response.isSuccess == true) {
                window.open(REPORTURL + "/Report/HikinukiToHostDataTransferReport.aspx?startDate=" + $("#txtStartDate").val() + "&endDate=" + $("#txtEndDate").val() + " " + $("#txtEndTime").val() + "&userID=" + $("#hdfLoginId").val() + "&printType=reprint","_blank","toolbar=1, scrollbars=1, resizable=1, width=" + 1900 + ", height=" + 1490);
            } else {
                showMessageDialog($("#lblErrorMessageE_Hikinuki_007").text(), "txtStartDate", WARNING);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Hikinuki").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

});

$("#btnTransfer").click(function () {
   
    if ($("#txtStartDate").val() == "") {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_001").text().trim(), "txtStartDate", WARNING);
        return false;

    }
    if ($("#txtEndTime").val() == "") {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_005").text().trim(), "txtEndTime", WARNING);
        return false;

    }

    var startDate = checkDate($("#txtStartDate").val());

    if (startDate == false) {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_002").text().trim(), "txtStartDate", WARNING);
        return false;
    }
    var endDateTime = checkDate($("#txtEndDate").val() + " " + $("#txtEndTime").val());

    if (endDateTime == false) {
        showMessageDialog($("#lblErrorMessageE_Hikinuki_006").text().trim(), "txtEndTime", WARNING);
        return false;
    }

    var endDate = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        beforeSend: function () {
            $("#LoadingImage").show();
        },
        complete: function () {
            $("#LoadingImage").hide();
        },
        success: function () {
            $("#LoadingImage").hide();
        },
        error: function () {
            $("#LoadingImage").hide();
        },
        url: APIURL + "/HikinukiCoilToBar/TransferDataToHost?startDate=" + $("#txtStartDate").val() + "&endDate=" + endDate + "&userID=" + $("#hdfLoginId").val() ,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response;
            // code 1 for data not found, 2 for data transfer sucess and 3 for exception
            if (result == 2) {


                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#lblSuccessMessageE_Hikinuki").text() + "</td></tr></table>",
                    buttons: {
                        ok: {
                            label: 'OK',
                            className: 'btn-primary'
                        }
                    },
                    centerVertical: true,
                    callback: function () {
                        window.open(REPORTURL + "/Report/HikinukiToHostDataTransferReport.aspx?startDate=" + $("#txtStartDate").val() + "&endDate=" + $("#txtEndDate").val() + " " + $("#txtEndTime").val() + "&userID=" + $("#hdfLoginId").val() +"&printType=print", "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1900 + ", height=" + 1490);
                    }
                });
            } else {
                if (result == 1) {
                    showMessageDialog($("#lblErrorMessageE_Hikinuki_003").text(), "txtStartDate", WARNING);

                } else if (result == 3) {
                    showMessageDialog($("#lblErrorMessageE_Hikinuki_004").text(), "txtStartDate", ERROR);
                }
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Hikinuki").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
});


function checkDate(date) {
    var returnVal = false;

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/CheckDate?date=" + date,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnVal = response.message;
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Hikinuki").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
//if (!checkDate($("#txtStartDate"))) {
//    var message = $("#lblErrorMessageE_Cold_Drawing_010").text().trim(); // This message will be displayed on dialog box
//    showMessageDialog(message, "txtStartDate", WARNING);
//    return false;
//}
//if (new Date(startDateTime) > new Date(endDateTime)) {
//    var message = $("#lblErrorMessageE_Cold_Drawing_007").text().trim();
//    var focusFieldID = "txtEndDate";
//    showMessageDialog(message, focusFieldID, WARNING);
//    return false;
//}

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

