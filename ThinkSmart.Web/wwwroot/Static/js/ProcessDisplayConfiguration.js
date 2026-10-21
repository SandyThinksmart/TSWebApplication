userName = "";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var getNextProcessCount = "";
var typeDropDown = "select-one";
var typeText = "text";
var DISABLED = "disabled";

$(document).ready(function () {
    window.resize(400, 810);
    getProcessKubunDetails();
    setProcessKubun();
    for (var j = 1; j <= getNextProcessCount; j++) {
        $("#txtPriority" + j).addClass("disabled");
    }
    bindEvent();
});

function bindEvent() {
    for (var k = 0; k <= getNextProcessCount; k++) {

        $("#chkShow" + k).click(function () {
            for (var a = 1; a <= getNextProcessCount; a++) {
                if ($("#chkShow" + a).prop("checked") == true) {
                    $("#txtPriority" + a).removeClass("disabled");
                    $("#txtPriority" + a).addClass("Mandatory");
                } else {
                    $("#txtPriority" + a).addClass("disabled");
                    $("#txtPriority" + a).removeClass("Mandatory");
                    $("#txtPriority" + a).val("");
                }
            }
        });
    }

}

$("#btnInitialize").click(function (event) {
    $("#ddlProcess").prop("selectedIndex", 0);
    Initialize();
});
function Initialize()
{
    for (var j = 1; j <= getNextProcessCount; j++) {
        $("#chkShow" + j).prop('checked', false);
        $("#txtPriority" + j).val("");
        $("#txtPriority" + j).addClass("disabled");
        $("#txtPriority" + j).removeClass("Mandatory1");
    }
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

function setProcessKubun() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessDisplayConfig/GetProcessKubun",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if ($("#hdfCultureName").val() == "en") {
                $.each(jsonResult, function (key, object) {

                    object.pP_PROCESS_NAME = object.kubuN_NAME_EN;
                });
            } else {
                $.each(jsonResult, function (key, object) {

                    object.pP_PROCESS_NAME = object.kubuN_NAME;
                });
            }
            bindProcessKubunDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProcessDisplayConfig001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcessKubunDropdown(jsonResult) {
    $("#ddlProcess").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcess").append($('<option value="' + jsonObj.kubuN_ID + '"> ' + jsonObj.pP_PROCESS_NAME + '</option>'));
    });
}

function getProcessKubunDetails() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessDisplayConfig/GetProcessKubunDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var jsonResult = response.message;
            getNextProcessCount = jsonResult.length
            if (response.isSuccess) {
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {

                        object.kubuN_ID;
                        object.pP_PROCESS_NAME = object.kubuN_NAME_EN;
                    });
                } else {
                    $.each(jsonResult, function (key, object) {
                        object.kubuN_ID;
                        object.pP_PROCESS_NAME = object.kubuN_NAME;
                    });
                }

                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/ProcessDisplayConfig/GetProcessKubunDetails",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        var jsonResult = response1.message;

                        if (response1.isSuccess) {
                            $("#ProcessDisplayConfigGrid").html(response1.message);
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_ProcessDisplayConfig001").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ProcessDisplayConfig001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields
    var message = ""; // This message will be displayed on dialog box
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
    });

    if (message !== "") {
        $("#dialogMessageIcon img").hide();

        if (focusFieldID == 'checkActualDimension') {
            if ($("#txtActual_Start").val().trim() == "") {
                focusFieldID = "txtActual_Start";
            } else {
                focusFieldID = "txtActual_End";
            }
        }

        showMessageDialog(message, focusFieldID, WARNING)

    }
    return returnVal;
}

$("#btnRegister").click(function (event) {
    debugger;
    var priorityList = [];
    var isCheckShow = false
    var priority = "";

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    for (var b = 1; b <= getNextProcessCount; b++) {
        if ($("#chkShow" + b).is(':checked')) {
            isCheckShow = true;
        }
        if ($("#txtPriority" + b).val() == "" && $("#chkShow" + b).is(':checked')) {
            var message = "";
            if (priority == "") {
                priority = b;
            } else {
                priority = priority + ", " + b;
            }
        }
    }
    if (priority != "") {
        message = $("#lblErrorMessageE_ProcessDisplayConfig002").text().trim().replace("NN", priority);
        showMessageDialog(message, "", WARNING);
        return false;
    }
    
    if ($("#chkPostGrid" + b).prop('checked', false) && isCheckShow == false) {
        var message = $("#lblErrorMessageE_ProcessDisplayConfig003").text().trim(); 
        showMessageDialog(message, "", WARNING);
        return false;
    }
   
    let checkBoxcount = $('#DivProcess input[type="checkbox"]:checked').length;
   
    for (var k = 1; k <= getNextProcessCount; k++) {
        if ($("#chkShow" + k).is(':checked')) {
            priorityVal = $("#txtPriority" + k).val();
            priorityList.push(priorityVal);
        }
    }
    var chk = new Set();
    var duplicatePriority = priorityList.filter(n => chk.size === chk.add(n).size);

    if (duplicatePriority.length > 0) {
        if (duplicatePriority.length > 1) {
            message = duplicatePriority[0] + ", " + duplicatePriority[1];
        } else {
            message = duplicatePriority[0];
        }
        message1 = $("#lblErrorMessageE_ProcessDisplayConfig004").text().trim().replace("NN", message);// "Duplicate[ " + message + " ]Order. not allowed" // $("#lblSuccessMessageS_ProcessDisplayConfig_001").text();
        showMessageDialog(message1, "", WARNING);
        return true;
    }

    SaveProcessKubunDetails();
});

function SaveProcessKubunDetails() {
    var ListData = [];
    var ajaxTime = new Date().getDate();
    for (var i = 1; i <= getNextProcessCount; i++) {
        if ($("#chkShow" + i).is(':checked')) {
            var jsonDataForSave = ({
                PROCESS_TYPE_CD: $("#ddlProcess").val(),
                SHOW_PRIORITY: $("#txtPriority" + i).val(),
                NEXT_PROCESS_TYPE_CD: $("#hdfProcessName" + i).val(),
                USER_ID: $("#hdfLoginId").val()
            });
            ListData.push(jsonDataForSave);
        }
    }
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessDisplayConfig/ProcessKubunDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(ListData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var successMessage = $("#lblSuccessMessageS_ProcessDisplayConfig_001").text();
            showMessageDialog(successMessage, "txtEndOperator", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ProcessDisplayConfig001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function getProcessVal(SelectProcess) {
    Initialize();
    //alert(SelectProcess.value);
    var KubunCD = SelectProcess.value;
    GetActiveNextProcessKubun(KubunCD);
}

function GetActiveNextProcessKubun(KubunCD) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessDisplayConfig/GetActiveNextProcessKubun?kubunCD=" + KubunCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonBindNextProcessKubun= response.message;
            setBindGridNextProcessKubun(jsonBindNextProcessKubun);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProcessDisplayConfig001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setBindGridNextProcessKubun(jsonResult) {
   for (var i = 1; i <= getNextProcessCount; i++) {
        var processCD = $("#hdfProcessName" + i).val()
        $.each(jsonResult, function (key, jsonObj) {
            key = parseInt(key) + 1;
            if ((jsonObj.nexT_PROCESS_TYPE_CD) == processCD) {
               document.getElementById("chkShow" + i).checked = true;
                if ($("#chkShow" + i).prop("checked") == true) {
                    $("#txtPriority" + i).removeClass("disabled");
                    $("#txtPriority" + i).addClass("Mandatory");
                } else {
                    $("#txtPriority" + i).addClass("disabled");
                    $("#txtPriority" + i).removeClass("Mandatory");
                    $("#txtPriority" + i).val("");
                }
                $("#txtPriority" + i).val(jsonObj.shoW_PRIORITY);
            }
    

       });
   }

 }
    
     
   
    
    
   
    
    
