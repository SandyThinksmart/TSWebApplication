
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";


$(document).ready(function () {
    debugger;
    //  window.resizeTo(1080, 830);
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    
    var FileName = "Ｊ条_fdsf_2025060317041.pdf";
    MakerMillSheetChemicalCompositionPopup(FileName)
});

function MakerMillSheetChemicalCompositionPopup(FileName) {
    debugger;
    
    var ajaxTime = new Date().getTime();
    var jsonData = "{filename :'" + FileName + "'}";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/MakerMillSheetChemicalCompositionPopup",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ FILENAME: FileName })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;
            MakerMillSheetChemicalDetail(jsonResult);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function MakerMillSheetChemicalDetail(jsonResult) {
    $("#txtStd1").val(jsonResult.harD_STD1);
    $("#txtStd2").val(jsonResult.harD_STD2);
    $("#txtStd3").val(jsonResult.harD_STD3);
    $("#txtStd4").val(jsonResult.harD_STD4);
    $("#txtStd5").val(jsonResult.harD_STD5);
    $("#txtStd6").val(jsonResult.harD_STD6);
    $("#txtStd7").val(jsonResult.harD_STD7);
    $("#txtStd8").val(jsonResult.harD_STD8);
    $("#txtStd9").val(jsonResult.harD_STD9);
    $("#txtStd10").val(jsonResult.harD_STD10);
    $("#txtStd11").val(jsonResult.harD_STD11);
    $("#txtStd12").val(jsonResult.harD_STD12);
    $("#txtStd13").val(jsonResult.harD_STD13);
    $("#txtStd14").val(jsonResult.harD_STD14);
    $("#txtStd15").val(jsonResult.harD_STD15);
    $("#txtStd16").val(jsonResult.harD_STD16);
    $("#txtStd17").val(jsonResult.harD_STD17);
    $("#txtStd18").val(jsonResult.harD_STD18);
    $("#txtStd19").val(jsonResult.harD_STD19);
    $("#txtStd20").val(jsonResult.harD_STD20);
    $("#txtStd21").val(jsonResult.harD_STD21);
    $("#txtStd22").val(jsonResult.harD_STD22);
    $("#txtStd23").val(jsonResult.harD_STD23);
    $("#txtStd24").val(jsonResult.harD_STD24);
    $("#txtStd25").val(jsonResult.harD_STD25);

    $("#txtMin1").val(jsonResult.harD_MIN1);
    $("#txtMin2").val(jsonResult.harD_MIN2);
    $("#txtMin3").val(jsonResult.harD_MIN3);
    $("#txtMin4").val(jsonResult.harD_MIN4);
    $("#txtMin5").val(jsonResult.harD_MIN5);
    $("#txtMin6").val(jsonResult.harD_MIN6);
    $("#txtMin7").val(jsonResult.harD_MIN7);
    $("#txtMin8").val(jsonResult.harD_MIN8);
    $("#txtMin9").val(jsonResult.harD_MIN9);
    $("#txtMin10").val(jsonResult.harD_MIN10);
    $("#txtMin11").val(jsonResult.harD_MIN11);
    $("#txtMin12").val(jsonResult.harD_MIN12);
    $("#txtMin13").val(jsonResult.harD_MIN13);
    $("#txtMin14").val(jsonResult.harD_MIN14);
    $("#txtMin15").val(jsonResult.harD_MIN15);
    $("#txtMin16").val(jsonResult.harD_MIN16);
    $("#txtMin17").val(jsonResult.harD_MIN17);
    $("#txtMin18").val(jsonResult.harD_MIN18);
    $("#txtMin19").val(jsonResult.harD_MIN19);
    $("#txtMin20").val(jsonResult.harD_MIN20);
    $("#txtMin21").val(jsonResult.harD_MIN21);
    $("#txtMin22").val(jsonResult.harD_MIN22);
    $("#txtMin23").val(jsonResult.harD_MIN23);
    $("#txtMin24").val(jsonResult.harD_MIN24);
    $("#txtMin25").val(jsonResult.harD_MIN25);
    $("#txtMax1").val(jsonResult.harD_MAX1);
    $("#txtMax2").val(jsonResult.harD_MAX2);
    $("#txtMax3").val(jsonResult.harD_MAX3);
    $("#txtMax4").val(jsonResult.harD_MAX4);
    $("#txtMax5").val(jsonResult.harD_MAX5);
    $("#txtMax6").val(jsonResult.harD_MAX6);
    $("#txtMax7").val(jsonResult.harD_MAX7);
    $("#txtMax8").val(jsonResult.harD_MAX8);
    $("#txtMax9").val(jsonResult.harD_MAX9);
    $("#txtMax10").val(jsonResult.harD_MAX10);
    $("#txtMax11").val(jsonResult.harD_MAX11);
    $("#txtMax12").val(jsonResult.harD_MAX12);
    $("#txtMax13").val(jsonResult.harD_MAX13);
    $("#txtMax14").val(jsonResult.harD_MAX14);
    $("#txtMax15").val(jsonResult.harD_MAX15);
    $("#txtMax16").val(jsonResult.harD_MAX16);
    $("#txtMax17").val(jsonResult.harD_MAX17);
    $("#txtMax18").val(jsonResult.harD_MAX18);
    $("#txtMax19").val(jsonResult.harD_MAX19);
    $("#txtMax20").val(jsonResult.harD_MAX20);
    $("#txtMax21").val(jsonResult.harD_MAX21);
    $("#txtMax22").val(jsonResult.harD_MAX22);
    $("#txtMax23").val(jsonResult.harD_MAX23);
    $("#txtMax24").val(jsonResult.harD_MAX24);
    $("#txtMax25").val(jsonResult.harD_MAX25);

    $("#txtActual1").val(jsonResult.harD_ACTUAL1);
    $("#txtActual2").val(jsonResult.harD_ACTUAL2);
    $("#txtActual3").val(jsonResult.harD_ACTUAL3);
    $("#txtActual4").val(jsonResult.harD_ACTUAL4);
    $("#txtActual5").val(jsonResult.harD_ACTUAL5);
    $("#txtActual6").val(jsonResult.harD_ACTUAL6);
    $("#txtActual7").val(jsonResult.harD_ACTUAL7);
    $("#txtActual8").val(jsonResult.harD_ACTUAL8);
    $("#txtActual9").val(jsonResult.harD_ACTUAL9);
    $("#txtActual10").val(jsonResult.harD_ACTUAL10);
    $("#txtActual11").val(jsonResult.harD_ACTUAL11);
    $("#txtActual12").val(jsonResult.harD_ACTUAL12);
    $("#txtActual13").val(jsonResult.harD_ACTUAL13);
    $("#txtActual14").val(jsonResult.harD_ACTUAL14);
    $("#txtActual15").val(jsonResult.harD_ACTUAL15);
    $("#txtActual16").val(jsonResult.harD_ACTUAL16);
    $("#txtActual17").val(jsonResult.harD_ACTUAL17);
    $("#txtActual18").val(jsonResult.harD_ACTUAL18);
    $("#txtActual19").val(jsonResult.harD_ACTUAL19);
    $("#txtActual20").val(jsonResult.harD_ACTUAL20);
    $("#txtActual21").val(jsonResult.harD_ACTUAL21);
    $("#txtActual22").val(jsonResult.harD_ACTUAL22);
    $("#txtActual23").val(jsonResult.harD_ACTUAL23);
    $("#txtActual24").val(jsonResult.harD_ACTUAL24);
    $("#txtActual25").val(jsonResult.harD_ACTUAL25);
    
}

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})


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
