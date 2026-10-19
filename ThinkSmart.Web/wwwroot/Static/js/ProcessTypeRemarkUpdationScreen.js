var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var remark;

$(document).ready(function () {
    window.resize(355, 441);
    var kubunCD = 0;
    setProcessKubun(kubunCD);
});

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

function setProcessKubun(KubunCD) {
    debugger;
    let kubunCD = KubunCD;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessTypeRemarkUpdation/GetProcessKubun",
        method: "Get",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            debugger;
            if ($("#hdfCultureName").val() == "en") {
                $.each(jsonResult, function (key, object) {
                    object.pP_PROCESS_NAME = object.kubuN_NAME_EN;
                });
            } else {
                $.each(jsonResult, function (key, object) {
                    
                    object.pP_PROCESS_NAME = object.kubuN_NAME;
                   
                });
            }
            if (kubunCD == 0) {
                bindProcessKubunDropdown(response.message);
            }
            setBindProcessKubun(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProcessTypeRemark001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function getProcessVal(SelectProcess) {
    var KubunCD = $("#ddlProcess option:selected").val();
    setProcessKubun(KubunCD);
}

function setBindProcessKubun(jsonResult) {
    debugger;
    const processCount = jsonResult.length;
    let kubunID = $("#ddlProcess option:selected").val();
    for (var i = 1; i <= processCount; i++) {
        $.each(jsonResult, function (key, jsonObj) {
            key = parseInt(key) + 1;

            if (kubunID == jsonObj.kubuN_ID) {
                $("#txtRemark").val(jsonObj.remarK_TEMPLATE);
            }


        });
    }

}

function bindProcessKubunDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcess").append($('<option value="' + jsonObj.kubuN_ID + '"> ' + jsonObj.pP_PROCESS_NAME + '</option>'));
       
    });
}

$("#btnUpdate").click(function (event) {
    updateProcessKubunRemark();
})

function updateProcessKubunRemark() {
    debugger
    var ajaxTime = new Date().getTime();
    var jsonDataSaveEditDelete = JSON.stringify({

        KUBUN_ID: parseInt($("#ddlProcess").val()),
        REMARK_TEMPLATE: $("#txtRemark").val(),
        USER_ID: parseInt($("#hdfLoginId").val()),
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessTypeRemarkUpdation/UpdateProcessKubunRemark",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var successMessage = $("#lblSuccessMessageS_ProcessTypeRemark_001").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProcessTypeRemark001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnInitialize").click(function (event) {
    $("#ddlProcess").prop("selectedIndex", 0);
    $("#txtRemark").val("");
});