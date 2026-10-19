var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var typeDropDown = "select-one";
var typeText = "text";
var DISABLED = "disabled";
var operatorCD = 0;
var disabled = "disabled";

userName = "";
$(document).ready(function () {
    debugger;
    window.resize(930, 610);
    setStartSysDateTime();
    let ProcessPlanCD = getURLParameters(window.location.href).trim();
    if (parseInt(ProcessPlanCD) > 0) {
        getMessageListByProcessPlanCD(ProcessPlanCD);
    }
    $("#hdfProcessPlanCd").val(ProcessPlanCD);
    //Mask();
    $("#BtnEdit").prop("disabled", true);
    $("#BtnDelete").prop("disabled", true);

  
})
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
        url: APIURL + "/SalesProductionStatusSearch/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var sysDateTime = response.message.enD_DATETIME_STRING;
            //  console.log("Server time is: " + serverTime.toLocaleTimeString());
            $("#txtMessageDateTime").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            showErrorDiv(errorCode);
        });

}

function Mask() {
    $("#txtMessageDateTime").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm 99:99" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "YYYY/MM/DD HH:mm" });
    SetDecimalFormat();
    // alert("mask");
}
function SetDecimalFormat() {
    //$(".structure").keyup(function (event) {
    //    RestrictToValidDecimalFormat($(this), 2, 1)
    //});

    //$(".Center").keyup(function (event) {
    //    RestrictToValidDecimalFormat($(this), 3, 1)
    //});


    //$(".HrdD").keyup(function (event) {
    //    RestrictToValidDecimalFormat($(this), 3, 1)
    //});
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
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top' style='font-size:13px'>" + message + "</td></tr></table>",
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
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/error.png'/ height='50px' width='50px'> </td><td vertical-align='top' style='font-size:13px'>" + message + "</td></tr></table>",
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
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top' style='font-size:13px'>" + message + "</td></tr></table>",
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

function getMessageListByProcessPlanCD(processPlanCD) {
    debugger;
    var ajaxTime = new Date().getTime();
    var jsonData1 = "{PP_PROCESS_PLAN_CD :'" + processPlanCD + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/getMessageListByProcessPlanCD?processPlanCD=" + parseInt(processPlanCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData1 + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            debugger;
            $("#hdfProcessPlanCd").val(processPlanCD);
            if (response.isSuccess) {
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.pP_PROCESS_NAME = object.procesS_NAME_EN;
                    });
                } else {
                    $.each(jsonResult, function (key, object) {
                        object.pP_PROCESS_NAME = object.procesS_NAME_JP;
                    });
                }
                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/SalesProductionStatusSearch/getMessageListByProcessPlanCD",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        var jsonResult = response1.message;
                        if (response1.isSuccess) {
                            $("#divMessageListGridMain").html(response1.message);
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_SalesProcessMessage_001").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesProcessMessage_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

$("#btnYobinaOpeartorSearch").click(function (event) {
    var yobina = $.trim($("#txtYobinaOperatorSearch").val());
    getOperatorList(yobina, "");
});

function getOperatorList(yobina, $target) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/GetOperatorList?name=" + yobina + "&operatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            bindOperatorDropdown(response.message, yobina);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesProcessMessage_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        })
}

function bindOperatorDropdown(jsonResult) {
    $("#ddlSalesInchargeValue").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlSalesInchargeValue").append($('<option value="' + jsonObj.cd + '" yobina= "' + jsonObj.yobina + '">' + jsonObj.name + '</option>'));
    });
}

function bindOperatorDropdownSelectedValue(jsonResult) {
    /*   $("#ddlSalesInchargeValue").html("").append($('<option value="0"></option>'));*/
    /* $.each(jsonResult, function (key, jsonObj) {*/
    $("#ddlSalesInchargeValue").append($('<option value="' + jsonResult.cd + '" yobina= "' + jsonResult.yobina + '">' + jsonResult.yobina + '</option>'));
    /* });*/
}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {

        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });
    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        showMessageDialog(message, focusFieldID, WARNING)
    }
    return returnVal;
}

$("#BtnRegister").click(function (event) {
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    SaveMessage();
    ProcessPlanCD = $("#hdfProcessPlanCd").val();
    if (ProcessPlanCD != 0) {
        getMessageListByProcessPlanCD(ProcessPlanCD);
    }
});


function SaveMessage() {
    debugger;
    var ajaxTime = new Date().getTime();

    var jsonDataForSave = JSON.stringify({
        PP_PROCESS_PLAN_CD: $("#hdfProcessPlanCd").val(),
        MESSAGE_DATE_TIME: $("#txtMessageDateTime").val(),
        END_OPERATOR_NAME: $("#ddlSalesInchargeValue").val(),
        PP_PROCESS_PLAN_REMARK: $("#txtDeliveryRemark").val(),
        USER_ID: $("#hdfLoginId").val()
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/MessageSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var successMessage = $("#lblSuccessMessageS_SalesProcessMessage_001").text();
            showMessageDialog(successMessage, "", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesProcessMessage_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

$("#BtnEdit").click(function (event) {
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    UpdateMessage();
    ProcessPlanCD = $("#hdfProcessPlanCd").val();
    if (ProcessPlanCD != 0) {
        getMessageListByProcessPlanCD(ProcessPlanCD);
    }
});

function UpdateMessage() {
    debugger;
    var ajaxTime = new Date().getTime();

    var jsonDataForSave = JSON.stringify({
        MSG_CD: $("#hdnMsgCd").val(),
        PP_PROCESS_PLAN_CD: $("#hdfProcessPlanCd").val(),
        MESSAGE_DATE_TIME: $("#txtMessageDateTime").val(),
        END_OPERATOR_NAME: $("#ddlSalesInchargeValue").val(),
        PP_PROCESS_PLAN_REMARK: $("#txtDeliveryRemark").val(),
        USER_ID: $("#hdfLoginId").val()
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/MessageUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var successMessage = $("#lblSuccessMessageS_SalesProcessMessage_001").text();
            showMessageDialog(successMessage, "", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesProcessMessage_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
$("#BtnDelete").click(function (event) {
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    DeleteMessage();
    ProcessPlanCD = $("#hdfProcessPlanCd").val();
    if (ProcessPlanCD != 0) {
        getMessageListByProcessPlanCD(ProcessPlanCD);
    }
});

function DeleteMessage() {
    var MSG_CD = $("#hdnMsgCd").val();
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/MessageDelete?MSG_CD=" + MSG_CD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var successMessage = $("#lblSuccessMessageS_SalesProcessMessage_001").text();
            showMessageDialog(successMessage, "", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesProcessMessage_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        })



}


function ClearControl() {
    ClearValue();

    setStartSysDateTime();
    $("#BtnEdit").prop("disabled", true);
    $("#BtnDelete").prop("disabled", true);

    $("#BtnRegister").prop("disabled", false);
}


$("#BtnInitialize").click(function (event) {
    ClearControl();
});
function ClearValue() {
    $("#ddlSalesInchargeValue").val("");
   // $("#ddlSalesInchargeValue").prop("selectedIndex", 0);
    $("#txtDeliveryRemark").val("");
    $("#txtMessageDateTime").val("");
    $("#hdnMsgCd").val("");
}

function GetMessageDetailsByMSGCD(editButton) {
    debugger;


    var internalGroupNo = $.trim(editButton.closest("tr").attr("data-lblMSGCD"));
    var ajaxTime = new Date().getTime();


    var row = editButton.closest("tr");
    var processName = row.find("td:eq(4)").text().trim();
    if (processName == "") {
        var msgCd = row.data("lblmsgcd");
        var OprCd = row.find(".OprCd").text().trim();
        getOperatorNameByCD(OprCd);
        var messageDate = row.find("td:eq(2)").text().trim();

        var operatorName = row.find("td:eq(3)").text().trim();
        var processName = row.find("td:eq(4)").text().trim();
        var machineName = row.find("td:eq(5)").text().trim();
        var message = row.find("td:eq(6)").text().trim();

        $("#hdnMsgCd").val(msgCd);
        $("#txtMessageDateTime").val(messageDate);        
        $("#txtDeliveryRemark").val(message);
        $("#BtnEdit").prop("disabled", false);
        $("#BtnDelete").prop("disabled", false);
        
        $("#BtnRegister").prop("disabled", true);

       
    }
    else {
        focusFieldID = $(':focus').attr("id");
        message = $("#lblErrorMessageE_SalesProcessCanNotEditdelete_002").text().trim();
        showMessageDialog(message, focusFieldID, WARNING);
        ClearControl();
        return false;
    }

   


}

function getOperatorNameByCD(OprCd) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/GetOperatorNamebyCD?OperatorCD=" + OprCd,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            $("#ddlSalesInchargeValue").text(result.yobina);
            bindOperatorDropdownSelectedValue(response.message, result.yobina);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesProcessMessage_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        })



}