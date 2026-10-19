var userName = "";
var combinedDecisionOK = 1;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var $btnInitialize;
var $btnSave;
var $btnEdit;
var $btnDelete;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";

function Mask() {
    $("#txtStartDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtDelDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
   

}

$('#txtStartDate').click(function (event) {

  
    this.select();

});

$('#txtDelDateEnd').click(function (event) {

  
    this.select();

});

$("#txtStartDate").change(function () {
    
    var StartDate = String($("#txtStartDate").val());
    let mydate = new Date(StartDate);
    let LastOneMonth = new Date(mydate.setMonth(mydate.getMonth() + 1));

    if (LastOneMonth > 0) {
        let date = new Date(LastOneMonth.setDate(LastOneMonth.getDate() - 1));

        $("#txtDelDateEnd").val($.format.date(new Date(date), "yyyy/MM/dd"));
    } else {
        $("#txtDelDateEnd").val("");
    }

});

$("#rdoDate").click(function () {

    disableChuban();
    enableDate();
    $("#rdoChuban").prop("checked", false);
});

$("#rdoChuban").click(function () {

    enableChuban();
    disableDate();
    $("#rdoDate").prop("checked", false);
});



$(document).ready(function () {
  
    window.resizeTo(460, 390);
    // $.support.cors = true; // Special case for IE7. To get past weird errors.
    Mask();
   
    disableChuban();
    enableDate();
});

function enableChuban() {
    $("#txtChubanNo").removeAttr("DISABLED").removeClass("DISABLED");
   
    $("#txtChubanNo").addClass("Mandatory");
   
  

}

function disableChuban() {
  
    $("#txtChubanNo").attr("DISABLED", "DISABLED").addClass("DISABLED");
  
    $("#txtChubanNo").removeClass("Mandatory");

}


function enableDate() {
    $("#txtStartDate").removeAttr("DISABLED").removeClass("DISABLED");
    $("#txtDelDateEnd").removeAttr("DISABLED").removeClass("DISABLED");
    $("#txtStartDate").addClass("Mandatory");
    $("#txtDelDateEnd").addClass("Mandatory");
    $("#rdoDate").prop("checked", true);
}

function disableDate() {

    $("#txtStartDate").attr("DISABLED", "DISABLED").addClass("DISABLED");
    $("#txtDelDateEnd").attr("DISABLED", "DISABLED").addClass("DISABLED");
    $("#txtStartDate").removeClass("Mandatory");
    $("#txtDelDateEnd").removeClass("Mandatory");

}

$("#btnRePrint").click(function (event) {

  
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    
    if (!checkDate($("#txtStartDate"))) {

        var message = $("#lblErrorMessage_StartDate").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, $("#txtStartDate"), WARNING);
        $("#txtStartDate").focus;
        return false;
    }
    if (!checkDate($("#txtDelDateEnd"))) {

        var message = $("#lblErrorMessage_EndDate").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, $("#txtDelDateEnd"), WARNING);
        $("#txtDelDateEnd").focus;
        return false;
    }
    if (!IsEndDataValid()) {
        return false;
    }

    
    var StartDate = $('#txtStartDate').val();
   // StartDate = StartDate.replace(/[/]/g, '');//.substring(2);
    var EndDate = $('#txtDelDateEnd').val();
   // EndDate = EndDate.replace([/]/g, '');//.substring(2);

  
   
    var ajaxTime = new Date().getDate();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessPlanPrintReport/GetProcessPlanData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DeliveryStartDate: StartDate, DeliveryEndDate: EndDate })
    }
    $.ajax(settings)
        .done(function (response) {
           
            var jsonResult = response;
            if (jsonResult.isSuccess != true) {
                var message = $.trim($("#lblNoDataToPrint").text());
                showMessageDialog(message, "", WARNING);

            } else {

                if (response.message.length > 0) {

                    
                    var jsonresult = response.message;

                    var coiltobar = jsonresult[0];
                    if (coiltobar > 0) {

                       // var myWindow = window.open(REPORTURL + "/Report/R_FinalInvoice.aspx?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");
                        var myWindow = window.open(REPORTURL + "/Report/ProcessPlanReportCoilToBar.aspx","_blank","width = 1940, height = 1140", "Resizable = no");//?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");
                    }
                    var coiltocoil = jsonresult[1];
                    if (coiltocoil > 0) {

                       // var myWindow = window.open(REPORTURL + "/Report/R_FinalInvoice.aspx?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");
                        var myWindow = window.open(REPORTURL + "/Report/ProcessPlanReportCoilToCoilOrange.aspx", "_blank", "width = 1940, height = 1140", "Resizable = no"); //?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");
                    }

                    var bartobar = jsonresult[2];
                    if (bartobar > 0) {

                        // var myWindow = window.open(REPORTURL + "/Report/R_FinalInvoice.aspx?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");
                        var myWindow = window.open(REPORTURL + "/Report/ProcessPlanReportBarToBar.aspx", "_blank", "width = 1940, height = 1140", "Resizable = no"); //?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");
                    }


                    }


                

            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lbl_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


    //var Kubun = 3;
    //var ajaxTime = new Date().getDate();
    //var settings = {
    //    async: true,
    //    crossDomain: true,
    //    cache: false,
    //    dataType: "json",
    //    url: APIURL + "/ProcessPlanPrintReport/GetProcessPlanData",
    //    method: "POST",
    //    contentType: "application/json; charset=utf-8",
    //    data: JSON.stringify({ DeliveryStartDate: StartDate, DeliveryEndDate: EndDate, Product_Kubun: Kubun })
    //}
    //$.ajax(settings)
    //    .done(function (response) {
    //        debugger;
    //        var jsonResult = response;
    //        if (jsonResult.isSuccess != true) {
    //            var message = $.trim($("#lblNoDataToPrint").text());
    //            showMessageDialog(message, "", WARNING);

    //        } else {


    //            var myWindow = window.open(aspxurl + "/Report/R_FinalInvoice.aspx?ClosureDate=" + ClosureDate + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DeptCode=" + DeptCode + "&EmpCode=" + EmpCode + "&CustCode=" + CustCode, "_blank", "width=1940,height=1140", "Resizable=no");

    //        }


    //    })
    //    .fail(function (xhr, textStatus, errorThrown) {
    //        responseTime = new Date().getTime() - ajaxTime;
    //        focusFieldID = $(':focus').attr("id");
    //        message = $.trim($("#lbl_SystemException").text());
    //        showMessageDialog(message, focusFieldID, ERROR);
    //    });


});


function checkDate(field) {

    var minYear = 2000;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
    if (field.val() != '') {

        if (regs = field.val().match(re)) {

            console.log(regs);
            if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {


                return false;
            }
            else {

                return true;
            }
        }
    }
}


function IsEndDataValid() {
    var returndate = true;

    // 1. If Start Date is greater than End Date, show error
    var startDate = $("#txtStartDate").val();
    var endDate = $("#txtDelDateEnd").val();

    if (new Date(startDate) > new Date(endDate)) {
        var message = $("#lblStartDateGreaterThanEndDate").text().trim();
        var focusFieldID = "txtStartDate";
        showMessageDialog(message, focusFieldID, WARNING);
        returndate = false;
    }

    return returndate;


}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {

        //if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {

        //    if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
        //        returnVal = false;
        //        message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
        //        if (firstFieldID === "") {
        //            firstFieldID = $(mandatoryField).attr("id");
        //        }
        //    }
        //}
        if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }

    });

    if (message !== "") {

        //  $("#dialogMessageIcon img").hide();
        // $("#imgError").show();
        //$("#imgWarning").show();

        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
            centerVertical: true,
            callback: function () {
                if (firstFieldID !== "") {
                    $("#" + firstFieldID).focus();
                }
            }
        });

    }
    return returnVal;

}

function IsMandatoryFieldsFilledEnd() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory1"), function (index, mandatoryField) {

        if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }

    });

    if (message !== "") {

        //  $("#dialogMessageIcon img").hide();
        // $("#imgError").show();
        //$("#imgWarning").show();

        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
            centerVertical: true,
            callback: function () {
                if (firstFieldID !== "") {
                    $("#" + firstFieldID).focus();
                }
            }
        });

    }
    return returnVal;

}

function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
}

function showMessageDialog(message, focusFieldID, messageType) {
    $("#dialogMessageIcon img").hide();
    switch (messageType) {
        case WARNING:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='65px'><br/> <img src='/images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
                message: "<table width='100%'><tr><td width='65px'><br/> <img src='/images/error.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
                message: "<table width='100%'><tr><td width='65px'><br/> <img src='/images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}