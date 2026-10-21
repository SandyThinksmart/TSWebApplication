let pageNumber = null;
let KoubanName = null;
const MESSAGE_TYPE = {
    SUCCESS: "success",
    ERROR: "error",
    WARNING: "warning"
};

$(document).ready(function () {

    /* window.resizeTo(1930, 1300);*/


    //DisableEditDeleteButton();
    //BindDropdown();



});

$("#btnChooseTextFile").click(function () {
    $("#hiddenTextFile").click();
});

$("#hiddenTextFile").on("change", function () {
    let file = this.files[0];

    if (file) {
        $("#selectTextFile").val(file.name);
    } else {
        $("#selectTextFile").val("");
    }
});



$("#btnImport").click(function () {
    debugger;
    let isValid = true;

    // Check file selected
    if ($("#hiddenTextFile").val() === "") {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#lblErrorMessageE_Maker_MillSheet_003").text());
        /*message = " Select File "*/
        showMessageDialog(message, focusFieldID, "warning");
        isValid = false;
    }

   
    if (!isValid) {
        return;
    }


    makerMillSheetEDIImport();
});



function makerMillSheetEDIImport() {
    debugger;

    var fileInput = document.getElementById("hiddenTextFile");

    if (fileInput.files.length === 0) {
        return;
    }

    var formData = new FormData();
    formData.append("file", fileInput.files[0]);

        var ajaxTime = new Date().getTime();
        var settings = {
           
            url: "/MakerMillSheetEDI/ReadTextFile",
            method: "POST",
            async: true,
            processData: false,
            contentType: false,
            data: formData

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
               
                if (response.isSuccess) {
                    let culture = $("#hdfCultureName").val();
                    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: APIURL + "/MakerMillSheetEDI/MillSheetEDIDataInsert?cultureName=" + culture + "&userID=" + userID,
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.data)
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;
                            var jsonResult = response1.message;
                            if (response1.isSuccess) {

                                if (response1.errorData.length > 0 || response1.allData.length > 0) {

                                    bootbox.alert({
                                        title: "<div style='color:white'></div>",

                                        message: "<table width='100%'>" +
                                            "<tr>" +
                                            "<td width='70px'><br/>" +
                                            "<img src='/Images/sucess.png' height='50px' width='50px'>" +
                                            "</td>" +
                                            "<td vertical-align='top'>" +
                                            $.trim($("#lblSuccessMessageS_Maker_MillSheet_001").text()) +
                                            "</td>" +
                                            "</tr>" +
                                            "</table>",

                                        buttons: {
                                            ok: {
                                                label: 'OK',
                                                className: 'btn-primary'
                                            }
                                        },

                                        centerVertical: true,

                                        callback: function () {

                                            // 1. Open Error Data report
                                            if (response1.errorData.length > 0) {

                                                window.open(
                                                    REPORTURL +
                                                    "/Report/MakerMillSheetDaidoEDIErrorReport.aspx?runID=" +
                                                    response1.runId,
                                                    "_blank",
                                                    "toolbar=1,scrollbars=1,resizable=1,width=1900,height=1490"
                                                );
                                            }

                                            // 2. Open All Data report
                                            if (response1.allData.length > 0) {

                                                window.open(
                                                    REPORTURL +
                                                    "/Report/MakerMillSheetDaidoEDIDataReport.aspx?runID=" +
                                                    response1.runId,
                                                    "_blank",
                                                    "toolbar=1,scrollbars=1,resizable=1,width=1900,height=1490"
                                                );
                                            }
                                        }
                                    });
                                }
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_Maker_MillSheet_001").text());
                            showMessageDialog(message, focusFieldID, "error");
                            return false;
                        });
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_Maker_MillSheet_001").text());
                showMessageDialog(message, focusFieldID, "error");
                return false;
            });
    

    /*closepopup();*/
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