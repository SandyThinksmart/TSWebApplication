var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var typeDropDown = "select-one";
var typeText = "text";
userName = "";
function onLoad() {
    // window.resizeTo(1220, 100);
}

$(document).ready(function () {

    const queryString1 = window.location.search;
    let screenName = queryString1.split('=')[1];

    if (screenName == "WIPTensouRequest") {
        document.getElementById("WIPrequestPanel").style.display = "block";
        document.getElementById("acceptancePanel").style.display = "none";
        setStartSysDateTime();
      

    } else if (screenName == "WIPTensouAcceptance") {
        document.getElementById("WIPrequestPanel").style.display = "none";
        document.getElementById("acceptancePanel").style.display = "block";
        setStartSysDateTime();
     
    }
        var userID = $("#hdfLoginId").val();
        if (userID != "") {
            setSourceLocation(userID);
        }
      
});

Date.prototype.addMonths = function (months) {
    var date = new Date(this.valueOf());
    date.setMonth(date.getMonth() + months);
    return date;
};

function setStartSysDateTime() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           // var sysDateTime = response.message;
            var sysDateTime = response.message.sysDate;
            var nextDayDateTime = response.message.nextDayDate;
            $("#txtDate").val($.format.date(new Date(nextDayDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#toDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            
            var dt = new Date(sysDateTime);
            dt.setMonth(dt.getMonth() - 1);
            var oneMonthBefore = dt.toISOString();
            $("#frmDate").val($.format.date(new Date(oneMonthBefore), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function Mask() {
    $("#txtDate").mask("9999/99/99", { placeholder: "yyyy/MM/dd" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "yyyy/MM/dd" });

    $("#frmDate").mask("9999/99/99", { placeholder: "yyyy/MM/dd" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "yyyy/MM/dd" });

    $("#toDate").mask("9999/99/99", { placeholder: "yyyy/MM/dd" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "yyyy/MM/dd" });

}

$("#btnPrint").click(function (event) {
    debugger;
    var date = $("#txtDate").val() ? $("#txtDate").val() : null;
    var sdate = $("#frmDate").val() ? $("#frmDate").val() : null;
    var edate = $("#toDate").val() ? $("#toDate").val() : null;

    const queryString1 = window.location.search;
    let screenName = queryString1.split('=')[1];

    if (screenName == "WIPTensouRequest") {

        if (date == null)
        {
            var message = $.trim($("#lblTransferDateRequired").text());
            showMessageDialog(message, "txtDate", WARNING);
            return false;
        }
       
            if (!checkDate($("#txtDate"))) {
                var message = $.trim($("#lblTransferDateInvalid").text()); // This message will be displayed on dialog box
                focusFieldID = $(':focus').attr("#txtDate");
                showMessageDialog(message, focusFieldID, WARNING);

                return false;
        }
        debugger;
        var ajaxTime = new Date().getDate();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/GetWIPRequestReportData?transferDate=" + date + "&userId=" + $("#hdfLoginId").val(),
            method: "POST",
            contentType: "application/json; charset=utf-8",
           // data: JSON.stringify({ TRANSFER_DATE_STRING: date, USER_ID: $("#hdfLoginId").val() })
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                var jsonResult = response;
                if (jsonResult.isSuccess != true) {
                    var message = $.trim($("#lblNoDataToPrint").text());
                    showMessageDialog(message, "txtDate", WARNING);

                } else {
                                     
                    var myWindow = window.open(REPORTURL + "/Report/WIPTensouRequestListReport.aspx?Date=" + date + "&userId=" + $("#hdfLoginId").val(), "width=1940,height=1140", "Resizable = no");
                   
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lbl_SystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

    if (screenName == "WIPTensouAcceptance") {

        var messages = "";
        var focus = "";
        if (sdate == null) {
            messages = $.trim($("#lblTensoueAcceptanceStartDateRequired").text()); 
            focus = "frmDate";
        }
        if (edate == null) {
            if (messages == "") {
                messages = $.trim($("#lblTensoueAcceptanceEndDateRequired").text());
            } else {
                messages = messages + "<br>"  + $.trim($("#lblTensoueAcceptanceEndDateRequired").text());
            }
            if (focus == "") {
                focus = "toDate";
            }
        }
        if (messages != "") {
            showMessageDialog(messages, focus, WARNING);
            return false;
        }

        if (sdate != "") {
            if (!checkDate($("#frmDate"))) {
                var message = $("#lblErrorMessageE_WIPTensouPrint_001").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "frmDate", WARNING);
                return false;
            }
        }

        if (edate != "") {
            if (!checkDate($("#toDate"))) {
                var message = $("#lblErrorMessageE_WIPTensouPrint_002").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "toDate", WARNING);
                return false;
            }
        }

        var ajaxTime = new Date().getDate();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/GetWIPAcceptancetReportData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ TRANSFER_DATE_START: sdate, TRANSFER_DATE_END: edate, USER_ID: $("#hdfLoginId").val() })
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                var jsonResult = response;
                if (jsonResult.isSuccess != true) {
                    var message = $.trim($("#lblNoDataToPrint").text());
                    showMessageDialog(message, "frmDate", WARNING);

                } else {
                    var myWindow = window.open(REPORTURL + "/Report/WIPTensouAcceptanceListReport.aspx?userId=" + $("#hdfLoginId").val() + "&TRANSFER_DATE_START=" + sdate + "&TRANSFER_DATE_END=" + edate, "width=1940,height=1140", "Resizable = no");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lbl_SystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    //if (document.getElementById("WIPrequestPanel").style.display != "none") {
    //    if (date == "") {
    //        var message = $.trim($("#lblNoDataToPrint").text()); // This message will be displayed on dialog box
    //        focusFieldID = $(':focus').attr("#txtDate");

    //        showMessageDialog(message, focusFieldID, WARNING);

    //        return false;
    //    }
    //}

    //if (document.getElementById("WIPrequestPanel").style.display == "none") {
    //    if (!IsMandatoryFieldsFilled()) {
    //        return false;
    //    }
    //}


   

  


});

function checkDate(field) {

    var minYear = 2019;
    var maxYear = 2100;

    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/      //CHECK ONLY DATE
   
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
            console.log(regs);
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

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown) {
            if (parseInt(mandatoryField.value) === indexDDLDecisionNothing) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }


    });



    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        //$("#imgError").show();
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


function setSourceLocation(USER_ID) {
  
    debugger;
   

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_ID: '" + USER_ID + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetLocation?USER_ID=" + USER_ID,
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
            $('#tbLocation').val(jsonResult.name);

            // bindlocationdropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lbl_SystemException").text());
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


$("#btnClose").click(function (event) {
    self.close();
});

function btnIssue_Click() {
    //debugger;
    //var rowsCount = 12;
    //var startDate = $("#txtDate").val();
    //var frmDate = $("#frmDate").val();
    //var toDate = $("#toDate").val();
    //var location = $("#tbLocation").val();
    //startDate = startDate.replace(/[/]/g, '').substring(0);


    //        const queryString1 = window.location.search;
    //        let screenName = queryString1.split('=')[1];

    //        if (screenName == "WIPTensouRequest") {

    //            var myWindow = window.open(aspxurl + "/Report/WIPTensouRequestListReport.aspx?Location=" + location + "&StartDate=" + startDate, "_blank", "width=1940,height=1140", "resizable=no");

    //        }
    //        else {
    //            var myWindow = window.open(aspxurl + "/Report/WIPTensouRequestListReport.aspx?Location=" + location + "&StartDate=" + frmDate + "&EndDate=" + toDate + "&isStampChecked=" + isStampChecked, "_blank", "width=1940,height=1140", "Resizable=no");
    //        }

}






