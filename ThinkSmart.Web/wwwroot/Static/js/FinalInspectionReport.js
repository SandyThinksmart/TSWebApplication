
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var operatorCD = 0;
var isPrintedFinishProduct = 0;
$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    window.resizeTo(565, 310);

    bindOperatorddlFI();

    $("#txtEndDateFI").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtEndTimeFI").mask("99:99", { placeholder: "hh:mm" });
    $("#txtStartDateFI").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtStartTimeFI").mask("99:99", { placeholder: "hh:mm" });
    SetStartEndDateTimeFI();

    var userID = $("#hdfLoginId").val();
    if (userID != "") {
        setLocation(userID);
    }

});


function checkDate(field) {

    var minYear = 2019;
    var maxYear = 2100;
    /*   re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/*/
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/      //CHECK ONLY DATE
    field.trim();

    if (field != '') {
        if (regs = field.match(re)) {
            // console.log(regs);
            var totdalDays = new Date(regs[1], regs[2], 0).getDate();
            if (parseInt(regs[3]) > totdalDays) {

                return false;
            } else if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {

                return false;
            }
            else if (regs[4] > 23 || regs[5] > 59) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}

function checkTime(field) {
    // Regular expression to match time in HH:MM format (24-hour)
    let re = /^([01]?[0-9]|2[0-3]):([0-5][0-9])$/;

    if (field !== '') {
        let regs = field.match(re);  // Match the time input
        if (regs) {
            let hours = parseInt(regs[1], 10);  // Extract hours
            let minutes = parseInt(regs[2], 10);  // Extract minutes

            // Ensure the hours and minutes are within valid ranges
            if (hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59) {
                return true;  // Valid time
            }
        }
    }
    return false;  // Invalid time
}

function SetStartEndDateTimeFI() {

    var dt = new Date($.now());
    var month = dt.getMonth() + 1;
    var day = dt.getDate();

    var output = dt.getFullYear() + '/' + (('' + month).length < 2 ? '0' : '') + month + '/' + (('' + day).length < 2 ? '0' : '') + day;

    $('#txtStartDateFI').val(output);
    $("#txtStartTimeFI").val("08:00");

    var end_dt = new Date($('#txtStartDateFI').val());
    var changed_date = new Date(end_dt.setDate(end_dt.getDate() + 1));
    $('#txtEndDateFI').val(changed_date.getFullYear() + "/" + (('' + (changed_date.getMonth() + 1)).length < 2 ? '0' : '') + (changed_date.getMonth() + 1) + "/" + (('' + changed_date.getDate()).length < 2 ? '0' : '') + changed_date.getDate());
    $("#txtEndTimeFI").val("07:59");
}

//Bind Operator ddl for FI
$("#btnOperatorSearchFI").click(function (event) {
    bindOperatorddlFI();
});

function bindOperatorddlFI() {
    var yobina = $("#txtOperatorSearchFI").val().trim();
    setOperatorList(yobina, $("#ddlOperatorNameValueFI"));
}


function setOperatorList(yobina, $target) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetOperatorList?name=" + yobina + "&operatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindOperatorDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageSystemException").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}

function bindOperatorDropdown(yobina, $target, jsonResult) {

    $target.html("").append($('<option value="0"></option>'));
    jsonResult = jsonResult.filter(row => row.iN_ACTIVE_FLAG == 0)

    $.each(jsonResult, function (key, jsonObj) {

        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + jsonObj.yobina.toLowerCase() + '">' + jsonObj.name + '</option>'));
    });

}

function setLocation(USER_ID) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetLocation?USER_ID=" + USER_ID,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;
            var jsonResult = jResult[0];
            $('#lblLocationVal').text(jsonResult.name);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            alert("check");
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function DisableUnprintedCheckbox()
{
    $("#txtOperatorSearchFI").prop("disabled", true);
    $("#txtStartDateFI").prop("readonly", true);
    $("#txtStartTimeFI").prop("readonly", true);
    $("#txtEndDateFI").prop("readonly", true);
    $("#txtEndTimeFI").prop("readonly", true);

    $("#btnOperatorSearchFI").prop("disabled", true);
    $("#ddlOperatorNameValueFI").prop("disabled", true);
    $("#chkPrintReportFI").prop("disabled", true);
    $("#chkOnlyPrintedFinishProduct").prop("disabled", true);
    $("#chkPrintReportFI").prop("checked", false).prop("disabled", true);
    $("#chkOnlyPrintedFinishProduct").prop("checked", false).prop("disabled", true);
    $("#chkOnlySimpleFI").prop("checked", true).prop("disabled", false);
   
}
function EnableUnprintedCheckbox() {
    $("#txtOperatorSearchFI").prop("disabled", false);
    $("#txtStartDateFI").prop("readonly", false);
    $("#txtStartTimeFI").prop("readonly", false);
    $("#txtEndDateFI").prop("readonly", false);
    $("#txtEndTimeFI").prop("readonly", false);

    $("#btnOperatorSearchFI").prop("disabled", false);
    $("#ddlOperatorNameValueFI").prop("disabled", false);
    $("#chkPrintReportFI").prop("checked", false).prop("disabled", false);
    $("#chkOnlyPrintedFinishProduct").prop("checked", false).prop("disabled", false);
    $("#chkOnlySimpleFI").prop("checked", false).prop("disabled", false);
}
$("#chkOnlyUnPrinted").change(function () {
    if ($(this).is(":checked")) {
        // Checked
        DisableUnprintedCheckbox();
    } else {
        // Unchecked
        EnableUnprintedCheckbox();
    }
});
$("#btnInitilize").click(function (event) {
    Clear();
    EnableUnprintedCheckbox();
    $("#rdoOnlyPrintedFinishProduct").prop('checked', 'checked');
    //$('#chkOnlyUnPrinted input:checkbox').attr('checked', false);
    $('#ddlPrintSortOrderFI').get(0).selectedIndex = 0;
});

function Clear() {
    $("#txtOperatorSearchFI").val('');
    bindOperatorddlFI();
    SetStartEndDateTimeFI();
    $('input[type="checkbox"]:checked').prop('checked', false);
}

$("#btnClose").click(function (event) {
    window.close();
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


//Print Button click
$("#btnPrint").click(function (event) {
    var url;
    var StartDateFI;
    var EndDateFI;
    var OperatornameFI;
    var OperatorIDFI;
    var SortOrderFI;
    var isPrintByPrintFlag;
    var OnlyUnPrintedChk;
    var OnlyUnPrintedChk;
    var chkOnlySimpleFI;
    var chkOnlyPrintedFinishProduct;
    var FinalInspectionChk;
    var features = "status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no,height=890,width=1160";

    var errorMessage = "";

    resetResponseTime();
    var ajaxTime = new Date().getTime();

    OnlyUnPrintedChk = ($("#chkOnlyUnPrinted").is(':checked') ? 1 : 0);
    chkOnlyPrintedFinishProduct = ($("#chkOnlyPrintedFinishProduct").is(':checked') ? 1 : 0);
    StartDateFI = $("#txtStartDateFI").val().trim() + "  " + $("#txtStartTimeFI").val().trim();          //For FI
    EndDateFI = $("#txtEndDateFI").val().trim() + "  " + $("#txtEndTimeFI").val().trim();
    chkOnlySimpleFI = ($("#chkOnlySimpleFI").is(':checked') ? 1 : 0);
    FinalInspectionChk = ($("#chkPrintReportFI").is(':checked') ? 1 : 0);
    
    OperatorIDFI = $("#ddlOperatorNameValueFI").find(":selected").val();
    if (OperatorIDFI == undefined) {
        OperatorIDFI = 0;
    }
    else {
        OperatorIDFI = $("#ddlOperatorNameValueFI").find(":selected").val();
    }
    if (OnlyUnPrintedChk == 1) {
        isPrintByPrintFlag = 1;
    }
    else {
        isPrintByPrintFlag = 0;
    }
    
    OperatornameFI = $("#ddlOperatorNameValueFI").find("option:selected").attr("yobina");
    SortOrderFI = $("#ddlPrintSortOrderFI").find(":selected").val().trim();                              //

    //date required
    if ($("#txtStartDateFI").val().trim() == "" && $("#txtStartDateFI").val().trim() == "") {
        message = $("#lblErrorMessageE_FIPrintStartDateRequired").text().trim();
        showMessageDialog(message, "chkOnlyUnPrinted", WARNING);
        return false;
    }

    if ($("#txtEndDateFI").val().trim() == "" && $("#txtEndTimeFI").val().trim() == "") {
        message = $("#lblErrorMessageE_FIPrintEndDateRequired").text().trim();
        showMessageDialog(message, "chkOnlyUnPrinted", WARNING);
        return false;
    }
    //Check start & End Date invalid.
    if (!checkDate($("#txtStartDateFI").val())) {

        var message = $.trim($("#lblErrorMessageE_FIPrintStartDateInvalid").text());
        focusFieldID = $(':focus').attr($("#txtStartDateFI").val());
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    if (!checkDate($("#txtEndDateFI").val())) {
        var message = $.trim($("#lblErrorMessageE_FIPrintDateEndInvalid").text());
        focusFieldID = $(':focus').attr($("#txtEndDateFI").val());
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    //Check Invalid Time start & End .
    if (!checkTime($("#txtStartTimeFI").val())) {
        var message = $.trim($("#lblErrorMessageE_FIPrintStartDateInvalid").text());
        focusFieldID = $(':focus').attr($("#txtStartTimeFI").val());
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    if (!checkTime($("#txtEndTimeFI").val())) {
        var message = $.trim($("#lblErrorMessageE_FIPrintDateEndInvalid").text());
        focusFieldID = $(':focus').attr($("#txtEndTimeFI").val());
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 1. If Start Date is greater than End Date, show error
    if (new Date(StartDateFI) > new Date(EndDateFI)) {
        var message = $("#lblMsg_FIPrintStartDateGreaterThanEndDate").text().trim();
        var focusFieldID = "";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // chkPrintReportFI

  //  var userID = $("#hdfLoginId").val();
    var reportSelected = false;
    var userID = $("#hdfLoginId").val();
    // Finished Product List
    if (chkOnlyPrintedFinishProduct == 1) {

        reportSelected = true;

        if (!FinishProductDataExist(StartDateFI, EndDateFI, OperatorIDFI, SortOrderFI, isPrintByPrintFlag)) {
            showMessageDialog($.trim($("#lblErrorMessageE_PMS_REPORT_PRINT_SCREEN_006").text()), "", WARNING);
        }
        else {
            window.open(
                REPORTURL + "/Report/R_FinishProduct.aspx?startDate=" + StartDateFI +
                "&endDate=" + EndDateFI +
                "&endOperator=" + OperatorIDFI +
                "&orderByID=" + SortOrderFI +
                "&isPrintByPrintFlag=" + isPrintByPrintFlag +
                "&printFlag=0" +
                "&userID=" + userID,
                "_blank",
                "width=1940,height=1140,resizable=no"
            );
        }
    }

    // Final Inspection Report
    if (FinalInspectionChk == 1) {

        reportSelected = true;

        if (!FIPrintDataExist(StartDateFI, EndDateFI, OperatorIDFI, SortOrderFI, isPrintByPrintFlag)) {
            showMessageDialog($.trim($("#lblErrorMessageE_PMS_REPORT_PRINT_SCREEN_006").text()), "", WARNING);
        }
        else {
            window.open(
                REPORTURL + "/Report/R_FinalInspection.aspx?startDate=" + StartDateFI +
                "&endDate=" + EndDateFI +
                "&endOperator=" + OperatorIDFI +
                "&orderByID=" + SortOrderFI +
                "&isPrintByPrintFlag=" + isPrintByPrintFlag +
                "&printFlag=0" +
                "&userID=" + userID,
                "_blank",
                "width=1940,height=1140,resizable=no"
            );
        }
    }

    // Final Inspection Simple
    if (chkOnlySimpleFI == 1) {

        reportSelected = true;

        if (!FinalInspectionSimpleDataExist(StartDateFI, EndDateFI, OperatorIDFI, SortOrderFI, isPrintByPrintFlag)) {
            showMessageDialog($.trim($("#lblErrorMessageE_PMS_REPORT_PRINT_SCREEN_006").text()), "", WARNING);
        }
        else {
            window.open(
                REPORTURL + "/Report/R_FinalInspectionSimple.aspx?startDate=" + StartDateFI +
                "&endDate=" + EndDateFI +
                "&endOperator=" + OperatorIDFI +
                "&orderByID=" + SortOrderFI +
                "&isPrintByPrintFlag=" + isPrintByPrintFlag +
                "&printFlag=0" +
                "&userID=" + userID,
                "_blank",
                "width=1940,height=1140,resizable=no"
            );
        }
    }

    
    // If nothing selected, open default Final Inspection Report
    //if (!reportSelected) {

       
    //}

});
function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

function FIPrintDataExist(StartDate, EndDate, OperatorID, SortOrder, isPrintByPrintFlag) {
    var returnval = false;
    var ajaxTime = new Date().getTime();

    var jsonData = "{"

        + "StartDate" + " : '" + StartDate + "',"
        + "EndDate" + " :  '" + EndDate + "',"
        + "OperatorID" + " :  '" + OperatorID + "',"
        + "SortOrder" + " :  '" + SortOrder + "',"
        + "isPrintByPrintFlag" + " : '" + isPrintByPrintFlag
        + "'}";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/isFIPrintDataExist?StartDate=" + StartDate + "&EndDate=" + EndDate + "&OperatorID=" + OperatorID + "&SortOrder=" + SortOrder + "&isPrintByPrintFlag=" + isPrintByPrintFlag + "&userId=" + $("#hdfLoginId").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess == true) {
                returnval = true;
            } else {
                returnval = false;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    return returnval;
}

function FinishProductDataExist(StartDate, EndDate, OperatorID, SortOrder, isPrintByPrintFlag) {
    var returnval = false;
    var ajaxTime = new Date().getTime();

    var jsonData = "{"

        + "StartDate" + " : '" + StartDate + "',"
        + "EndDate" + " :  '" + EndDate + "',"
        + "OperatorID" + " :  '" + OperatorID + "',"
        + "SortOrder" + " :  '" + SortOrder + "',"
        + "isPrintByPrintFlag" + " : '" + isPrintByPrintFlag
        + "'}";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/isFinishProductDataExist?StartDate=" + StartDate + "&EndDate=" + EndDate + "&OperatorID=" + OperatorID + "&SortOrder=" + SortOrder + "&isPrintByPrintFlag=" + isPrintByPrintFlag + "&userId=" + $("#hdfLoginId").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess == true) {
                returnval = true;
            } else {
                returnval = false;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    return returnval;
}

function FinalInspectionSimpleDataExist(StartDate, EndDate, OperatorID, SortOrder, isPrintByPrintFlag) {
    var returnval = false;
    var ajaxTime = new Date().getTime();

    var jsonData = "{"

        + "StartDate" + " : '" + StartDate + "',"
        + "EndDate" + " :  '" + EndDate + "',"
        + "OperatorID" + " :  '" + OperatorID + "',"
        + "SortOrder" + " :  '" + SortOrder + "',"
        + "isPrintByPrintFlag" + " : '" + isPrintByPrintFlag
        + "'}";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/isFISimpleRecordDataExist?StartDate=" + StartDate + "&EndDate=" + EndDate + "&OperatorID=" + OperatorID + "&SortOrder=" + SortOrder + "&isPrintByPrintFlag=" + isPrintByPrintFlag + "&userId=" + $("#hdfLoginId").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess == true) {
                returnval = true;
            } else {
                returnval = false;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    return returnval;
}