let pageNumber = null;
let KoubanName = null;
const MESSAGE_TYPE = {
    SUCCESS: "success",
    ERROR: "error",
    WARNING: "warning"
};

$(document).ready(function () {
    //
    /* window.resizeTo(1930, 1300);*/


    //DisableEditDeleteButton();
    //BindDropdown();



});

$("#btnChooseSeibunCSVFile").click(function () {
    $("#hiddenSeibunCSVFile").click();
});

$("#hiddenSeibunCSVFile").on("change", function () {
    let file = this.files[0];

    if (file) {
        $("#selectSeibunCSVFile").val(file.name);
    } else {
        $("#selectSeibunCSVFile").val("");
    }
});


$("#btnChooseOrderCSVFile").click(function () {
    $("#hiddenOrderCSVFile").click();
});

$("#hiddenOrderCSVFile").on("change", function () {
    let file = this.files[0];

    if (file) {
        $("#selectOrderCSVFile").val(file.name);
    } else {
        $("#selectOrderCSVFile").val("");
    }
});


//$(document).on('change', '#hiddenCSVFile', function () {

//    const files = this.files;

//    // No file selected
//    if (files.length === 0) {

//        message = "Please select CSV files.";
//        focusFieldID = $(':focus').attr("id");

//        showMessageDialog(message, focusFieldID, "error");

//        return;
//    }

//    // Only 2 files allowed
//    //if (files.length !== 2) {

//    //    message = "Please select two CSV files.";
//    //    focusFieldID = $(':focus').attr("id");

//    //    showMessageDialog(message, focusFieldID, "error");

//    //    $(this).val('');
//    //    $('#selectCSVFile').val('');

//    //    return;
//    //}

//    // Check empty files
//    for (let i = 0; i < files.length; i++) {

//        const file = files[i];

//        // Empty file
//        if (file.size === 0) {

//            message = file.name + " is empty.";
//            focusFieldID = $(':focus').attr("id");

//            showMessageDialog(message, focusFieldID, "error");

//            $(this).val('');
//            $('#selectCSVFile').val('');

//            return;
//        }
//    }

//    //let fileNames = [];

//    //for (let i = 0; i < files.length; i++) {

//    //    const file = files[i];
//    //    const fileName = file.name.toLowerCase();

//    //    // Only CSV allowed
//    //    if (!fileName.endsWith('.csv')) {

//    //        message = "Only CSV files are allowed.";
//    //        focusFieldID = $(':focus').attr("id");

//    //        showMessageDialog(message, focusFieldID, "error");

//    //        $(this).val('');
//    //        $('#selectCSVFile').val('');

//    //        return;
//    //    }

//    //    fileNames.push(file.name);
//    //}

//    $('#selectCSVFile').val(fileNames.join(' , '));
//});


$("#btnImport").click(async function () {
    debugger;

    let isValid = true;
    let errorMessages = [];

    var seibunFilePath = $("#hiddenSeibunCSVFile").val();
    var orderFilePath = $("#hiddenOrderCSVFile").val();

    // Actual File objects - used for file size/content validation
    var seibunFile = $("#hiddenSeibunCSVFile")[0].files[0];
    var orderFile = $("#hiddenOrderCSVFile")[0].files[0];

    var seibunMatch = null;
    var orderMatch = null;

    // Check file selected
    if (seibunFilePath === "") {
        errorMessages.push(
            $.trim($("#lblErrorMessageE_Maker_MillSheet_004").text())
        );
    }

    if (orderFilePath === "") {
        errorMessages.push(
            $.trim($("#lblErrorMessageE_Maker_MillSheet_005").text())
        );
    }

    // Check empty file
        if (seibunFile) {
        var seibunHasData = await checkCSVHasData(seibunFile);

        if (!seibunHasData) {
            errorMessages.push(
                $.trim($("#lblErrorMessageE_Maker_MillSheet_009").text())
            );
        }
    }

    if (orderFile) {
        var orderHasData = await checkCSVHasData(orderFile);

        if (!orderHasData) {
            errorMessages.push(
                $.trim($("#lblErrorMessageE_Maker_MillSheet_010").text())
            );
        }
    }

    // Get only file name from path
    var seibunFileName = seibunFilePath.split("\\").pop();
    var orderFileName = orderFilePath.split("\\").pop();

    // Regex
    var seibunRegex = /^HAYASHI_SEIBUN_(\d{8}_\d{6})\.csv$/i;
    var orderRegex = /^HAYASHI_ORDER_(\d{8}_\d{6})\.csv$/i;

    // Validate Seibun filename
    if (seibunFileName !== "") {

        seibunMatch = seibunFileName.match(seibunRegex);

        if (!seibunMatch) {
            errorMessages.push(
                $.trim($("#lblErrorMessageE_Maker_MillSheet_006").text())
            );
        }
    }

    // Validate Order filename
    if (orderFileName !== "") {

        orderMatch = orderFileName.match(orderRegex);

        if (!orderMatch) {
            errorMessages.push(
                $.trim($("#lblErrorMessageE_Maker_MillSheet_007").text())
            );
        }
    }

    // Compare date/time
    if (seibunMatch && orderMatch) {

        if (seibunMatch[1] !== orderMatch[1]) {
            errorMessages.push(
                $.trim($("#lblErrorMessageE_Maker_MillSheet_008").text())
            );
        }
    }

    // Show errors
    if (errorMessages.length > 0) {

        focusFieldID = $(':focus').attr("id");

        message = errorMessages.join("<br/>");

        showMessageDialog(message, focusFieldID, WARNING);

        isValid = false;
    }

    if (!isValid) {
        return;
    }

    makerMillSheetEDIImport();
});



function makerMillSheetEDIImport() {
    debugger;

    var fileInput1 = document.getElementById("hiddenSeibunCSVFile");
    var fileInput2 = document.getElementById("hiddenOrderCSVFile");


    var formData = new FormData();

    formData.append("file1", fileInput1.files[0]);
    formData.append("file2", fileInput2.files[0]);

    var ajaxTime = new Date().getTime();
    var settings = {

        url: "/MakerMillSheetJfeEDI/ReadCSVFile",
        method: "POST",
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
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: APIURL + "/MakerMillSheetJfeEDI/MillSheetJfeEDIDataInsert?cultureName="
                        + culture + "&userID=" + userID,
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.data)
                }
                $.ajax(settings1)
                    .done(function (response1) {

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
                                                "/Report/MakerMillSheetJfeEDIErrorReport.aspx?runID=" +
                                                response1.runId,
                                                "_blank",
                                                "toolbar=1,scrollbars=1,resizable=1,width=1900,height=1490"
                                            );
                                        }

                                        // 2. Open All Data report
                                        if (response1.allData.length > 0) {

                                            window.open(
                                                REPORTURL +
                                                "/Report/MakerMillSheetJfeEDIDataReport.aspx?runID=" +
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

function checkCSVHasData(file) {

    return new Promise(function (resolve) {

        var reader = new FileReader();

        reader.onload = function (e) {

            var content = e.target.result;

            // Split CSV into rows
            var rows = content.split(/\r?\n/);

            // Remove blank rows
            rows = rows.filter(function (row) {
                return $.trim(row) !== "";
            });

            // First row = header
            // If only header exists, there is no data
            if (rows.length <= 1) {
                resolve(false);
                return;
            }

            // At least one row exists after header
            resolve(true);
        };

        reader.onerror = function () {
            resolve(false);
        };

        reader.readAsText(file, "Shift-JIS");
    });
}