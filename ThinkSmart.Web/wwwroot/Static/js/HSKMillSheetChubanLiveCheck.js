var $btnLnkEdit;
var combinedDecisionOK = 1;
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";
var $hdfSYSDATE;
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var oldHikinukiRemarks;
var isHikinukiRemarksUpdate = 0;
var IsExists = false;
var IsStartOprExists = false;
var IsEndOprExists = false;
var registeredThabaNumbersAndDecision = {};
var maxRegisteredThabaNo = 0;
var IsNGReason1 = false;
var IsNGReason2 = false;
var IsNGReason3 = false;
var IsProcessDefect1Exists = false;
var IsProcessDefect2Exists = false;
var IsProcessDefect3Exists = false;
var startOperatorCD = "";
var endOperatorCD = "";
var DISABLED = "disabled";
userName = "";
let screenUrl = false;
$(document).ready(function () {
    debugger;
    //  window.resizeTo(1080, 830);
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    //setDecimalFormats();
    ////Mask();


    //InitSortPopup();
    //$("#txtChuubanSearch").focus();
    //userName = $("#hdfUserName").val();
    //bindEvents();


    //$("#txtDatestart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    //$("#txtDateend").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

   
    const params = new URLSearchParams(window.location.search);

    const chuuban = params.get("chuuban");
    const gyouban = params.get("gyouban");

    HSKMillSheetGridSearchChuubanLiveCheck(chuuban, gyouban);

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


function HSKMillSheetGridSearchChuubanLiveCheck(chuuban, gyouban) {
    debugger;
  


    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/HSKMillSheetGridSearchChuubanLiveCheck?chuuban=" + chuuban + '&gyouban=' + gyouban,
        method: "POST",
        contentType: "application/json; charset=utf-8",
      //  data: JSON.stringify({ PROD_NAME_CUST: txtProdNameCust, PROD_SHAPE: txtshape, PROD_DIM_D_MIN: txtdim1, PROD_DIM_D_MAX: txtdim2, PROD_LENGTH_MIN: txtLength1, PROD_LENGTH_MAX: txtLength2, CHARGE: txtcharge, DELIVERY_DATE_MIN: txtDatestart, DELIVERY_DATE_MAX: txtDateend, CHUUBAN: txtChuuban, GYOUBAN: txtgyouban, USER_ID: userID, SORT_FIELDS: sortFields })

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/HSKMillSheet/HSKMillSheetGridSearchChuubanLiveCheck",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#DivHSKMillSheetChubanLiveCheckPopUpGrid").html(response1.message);
                           
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });




}



function OnEditClick(editButton) {
    var ProcessPlanCD = $.trim(editButton.closest("tr").attr("data-lblProcessPlanCD"));
    // location.href = '/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=' + ProcessPlanCD;
    window.open("/HSKMillSheet/HSKMillSheetSaveEdit?ProcessPlanCD=" + ProcessPlanCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1880 + ", height=" + 950);
}

