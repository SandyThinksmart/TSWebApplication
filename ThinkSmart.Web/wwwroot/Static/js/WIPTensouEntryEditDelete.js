
var $btnSearchChuuban;

var $btnInitialize;
var $btnSave;
var $btnEdit;
var $btnDelete;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var WIP_TRANSFER_CD;
var MSG_CD;
//var TransferId=0;
var $btnSearch;
var $btnClose;
userName = "";// assigned at Document.ready
var $btnLnkEdit;
var combinedDecisionOK = 1;
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";
var $hdfSYSDATE;
var dimTolMin;
var TransferID;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var EditCheck = "";
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var oldHikinukiRemarks;
var isStamppingRemarksUpdate = 0;
var IsExists = false;
var DISABLED = "disabled";
var combinedDecisionOK = "";
var IsAcceptExists = false;

function setGlobalVariables() {
    $("#hdfDBOperation").val("");
    $btnSearchChuuban = $("#btnSearchChuuban");

    $btnSave = $("#btnSave");
    $btnInitialize = $("#btnInitialize");
    $btnSearch = $("#btnSearch");
    $btnClose = $("#btnClose");
    // $txtDate = $("#txtDate");
}

function disableEditDeleteButtons() {
    $("#btnEdit").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
    $("#btnSave").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnEdit").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

//GetTensouDetailsByChuubanGyouban
//Scan Nifuda but Get Tensou  Details on Chuuban Gyouban-both side get the Tensou  Details. 
function GetTensouDetailsByChuubanGyouban(chuuban, $callFrom) {
    debugger;
    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();
    var culture = $("#hdfCultureName").val();
    var jsonTensouDetailData = "{NIFUDA_STRING : '" + chuuban + "' , CULTURE_NAME : '" + culture + "' , USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetTensouDetailsByChuubanGyouban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonThabaDetailData + "}"
        data: JSON.stringify({ NIFUDA_STRING: chuuban, CULTURE_NAME: culture })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            $("#btnSave").removeAttr("disabled");
            debugger;
            if (jsonResult == null || jsonResult.length == 0) {

                $("#btnSave").attr("disabled", "disabled");
                var nifuda = $("#txtNifuda").val();
                if (nifuda != "") {
                    var errorMessage = $("#lblErrorMessageE_WIPTensou_nifudanotfound").text();// Nifuda not found in system.
                    showMessageDialog(errorMessage, "txtNifuda", WARNING);
                    return false;
                }
                else {
                    var errorMessage = $("#lblErrorMessageE_WIPTensou_chuubannotfound").text();// Chuuban not found in system.
                    showMessageDialog(errorMessage, "txtNifuda", WARNING);
                    return false;
                }

            }

            if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {                // Chuuban is marked as completed in the system
                $("#btnSave").attr("disabled", "disabled");
                var errorMessage = $("#lblErrorMessageE_WIPTensou_004").text();
                showMessageDialog(errorMessage, "txtChuuban", WARNING);
                return false;
            }


            if (jsonResult.procesS_PLAN_START_DATE == null) {
                $("#btnSave").attr("disabled", "disabled");
                var message = $("#lblErrorMessageE_WIPTensou_015").text().trim().replace("99-999999-99", jsonResult.pP_CHUUBAN);  //OrNo Not Started From HandOver.
                showMessageDialog(message, "txtChuuban", WARNING);
                return false;
            }

            $("#ddlVehicleName").focus();
            //setChuubanDetails
            setChuubanDetails(jsonResult);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            var btn = document.getElementById("Button");
            btn.disabled = false;
        });

}

//
//$(document).on('click', '#lnkEdit', function (event) {
//    debugger;
//    resetResponseTime();
//    var $callFrom = $(this);
//    $("#hdWIPTensouCD").val() = $callFrom;
//    getWIPTransferDetailsByTrasferCD($callFrom);
//   // self.close();
//})

$("#btnSearchChuuban").click(function (event) {
    debugger;
    var $callFrom = $(this);
    event.preventDefault();
    setStartSysDateTime();

    var chuuban = $("#txtChuuban").val().trim();

    if (chuuban != "") {

        GetTensouDetailsByChuubanGyouban(chuuban, $callFrom);
        return false;
    }
    else {
        var errorMessage = $("#lblErrorMessageE_WIPTensou_001").text();// Chuuban not found in system.
        showMessageDialog(errorMessage, "txtNifuda", WARNING);
        return false;
    }
    setNextProcess();

    $("#btnSave").removeAttr("disabled");

});

$("#btnSearchNifuda").click(function (event) {
    debugger;
    event.preventDefault();
    debugger;
    setStartSysDateTime();
    var $callFrom = $(this);

    var nifuda = $("#txtNifuda").val().trim();
    if (nifuda !== "") {
        GetChuubanGyuobanByNifuda(nifuda, $callFrom);
        return false;
    }
    else {
        var errorMessage = $("#lblErrorMessageE_WIPTensou_valMsg_NifudaRequired").text();// Chuuban not found in system.
        showMessageDialog(errorMessage, "txtNifuda", WARNING);
        return false;
    }

    setNextProcess();
    $("#btnSave").removeAttr("disabled");
});


$(document).ready(function () {
    $.support.cors = true;
    $("#hdfScreenName").val("WIP TENSOU"); //Bind screen Name

    if (!$("#txtNifuda").hasClass("Mandatory")) {
        $("#txtNifuda").addClass("Mandatory");
    }
    $("#txtNifuda").focus();
    $("#txtChuubanSearch").focus();

    //Display today Transfer date(start) .
    setStartSysDateTime();
    setProductUnitKubun();
    setDecimalFormats();
    if ($("#hdfLoginId").val() != "") {
        setSourceLocation($("#hdfLoginId").val());
    }
    RestrictToDigitsOnly();
    setForwadingDestination();
    setVehicleName();
    setGlobalVariables();
    Mask();
    InitSortPopup();
    setConditionName();
    //set window resize
    var currentUrl = window.location.href;
    var url = currentUrl.split('WIPTensou/')[1];
    if (url == "WIPTensouList") {
        const ua = navigator.userAgent;
        // Detect Tablet (iPad, Android Tablet, etc.)
        const isTablet = /(iPad|Tablet|Nexus 7|Nexus 10|KFAPWI|SM-T|Lenovo Tab|Pixel C)/i.test(ua);
        // Detect Mobile (Phones)
        const isMobile = /(Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini)/i.test(ua);
        if (isTablet) {
            // Tablet device
            window.resizeTo(768, 1024);   // typical tablet size (only works in popup windows)
            document.body.style.zoom = "100%";
        } else if (isMobile) {
            // Mobile device
            window.resizeTo(360, 780);    // typical mobile size
            document.body.style.zoom = "50%";
        } else {
            // Desktop
            window.resizeTo(1100, 650);
        }
    }
    else {
        const ua = navigator.userAgent;
        // Detect Tablet (iPad, Android Tablet, etc.)
        const isTablet = /(iPad|Tablet|Nexus 7|Nexus 10|KFAPWI|SM-T|Lenovo Tab|Pixel C)/i.test(ua);
        // Detect Mobile (Phones)
        const isMobile = /(Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini)/i.test(ua);

        if (isTablet) {
            // Tablet device
            $("#divWIPTensou").height(290);
            $("#pnlTensouDetails").height(300);
            //alert("Tab View");
            window.resizeTo(1040, 750);
            document.body.style.zoom = "100%";
            const myTextbox = document.getElementById("txtUnitWt");
            myTextbox.style.width = "80px";
        } else if (isMobile) {
            // Mobile device
            window.resizeTo(360, 780);    // typical mobile size
            document.body.style.zoom = "50%";
        } else {
            // Desktop
            window.resizeTo(1040, 870);
            const myTextbox = document.getElementById("txtUnitWt");
            myTextbox.style.width = "88px";
        }
    }
    setNextProcess();
    const queryString1 = window.location.search;
    let TransferCD = queryString1.split('=')[1];
    $("#hdWIPTensouCD").val(TransferCD);

    if (TransferCD != null) {
        getTransferDetailsByTrasferCD(TransferCD);
        enableEditDeleteButtons();
        // $("#btnSelect").attr("disabled", "disabled");
        $("#btnSave").attr("disabled", "disabled");
    }
    else {
        disableEditDeleteButtons();
    }
});

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
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message.sysDate;
            var nextDayDateTime = response.message.nextDayDate;
            //$("#txtDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            $("#txtDate").val($.format.date(new Date(nextDayDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtPlanDate").val($.format.date(new Date(nextDayDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
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
    $(document.getElementsByClassName('DateField')).mask("9999/99/99 99:99", { placeholder: "yyyy/MM/dd HH:mm" });

    $("#txtTransferStartDateSearch").mask("9999/99/99", { placeholder: "yyyy/MM/dd" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "yyyy/MM/dd" });

    $("#txtTransferEndDateSearch").mask("9999/99/99", { placeholder: "yyyy/MM/dd" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "yyyy/MM/dd" });

}

function setChuubanDetails(jsonResult) {
    debugger;
    // jsonResult = jsonResult[0];

    dbOperation = "Insert";

    $("#hdfDBOperation").val(dbOperation);

    $("#txtChuuban").val(jsonResult.pP_CHUUBAN);
    $("#txtCustomer").text(jsonResult.pP_CUSTOMER_NAME);
    $("#txtChuuban").text(jsonResult.pP_CHUUBAN);
    $("#txtDeliveryDate").text(jsonResult.pP_DELIVERY_DATE_STRING);
    $('#txtCustomerIncharge1').text(jsonResult.pP_EMPLOYEE_NAME);
    if (jsonResult.pP_STD_DEL_PERIOD == 0) {
        $('#txtStdDeliveryDate').text("");
    } else {
        $('#txtStdDeliveryDate').text(jsonResult.pP_STD_DEL_PERIOD);
    }
    // $("#txtStdDeliveryDate").text(jsonResult.pP_STD_DEL_PERIOD);
    $('#txtReceiver').text(jsonResult.pP_RECEIVER_NAME);
    if (jsonResult.pP_CHAMFER == 0) {
        $('#txtChamfer').text("");
    } else {
        $('#txtChamfer').text(jsonResult.pP_CHAMFER);
    }
    // $('#txtUnitWt').text(jsonResult.pP_UNIT_WT);
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) > 0) {
        $('#txtUnitWt').text(jsonResult.pP_UNIT_WT_KUBUN_STRING);
    }
    //if (jsonResult.pP_UNIT_WT == 0) {
    //    $('#txtUnitWt').text("");
    //} else {
    //    $('#txtUnitWt').text(jsonResult.pP_UNIT_WT);
    //}
    $('#txtColor').text(jsonResult.pP_COLOR);
    $('#txtMaterialNameMaker').text(jsonResult.pP_MATL_NAME_MAK);
    $('#txtMatlShape').text(jsonResult.pP_MATL_SHAPE);
    $('#txtMatlDimD').text(jsonResult.pP_MATL_DIM_D);
    $('#txtCharge').text(jsonResult.pP_CHARGE);
    if (jsonResult.pP_MAGARI_BEND_TOL == 0) {
        $('#txtBendStandard').text("");
    } else {
        $('#txtBendStandard').text(jsonResult.pP_MAGARI_BEND_TOL);
    }
    // $('#txtBendStandard').text(jsonResult.pP_MAGARI_BEND_TOL);
    $('#txtExtTexture').text(jsonResult.pP_EXT_TEXTURE);
    $('#txtDisplay').text(jsonResult.pP_DISPLAY);
    $('#txtPacking').text(jsonResult.pP_PACKING);
    $('#txtLengthTolerance').text(jsonResult.pP_LEN_TOL);
    $('#txtLength').text(jsonResult.pP_PROD_LENGTH_STRING);
    $('#txtProdShape').text(jsonResult.pP_PROD_SHAPE);
    $('#txtProdNameCustomer').text(jsonResult.pP_PROD_NAME_CUST);
    $('#txtQuantity').text(jsonResult.pP_ACT_ALLOC_QTY);
    $('#txtKakouiraiRemarks').text(jsonResult.pP_KAKOUIRAI_REMARKS);
    // $('#txtHostRefNo').text(jsonResult.pP_PROD_LENGTH_STRING);
    $('#txtStamp').text(jsonResult.pP_STAMP);
    $('#txtNoOfUnitsT1').text(jsonResult.pP_NO_OF_UNITS_DECODE);
    $('#txtDimTolerance').text(jsonResult.pP_DIM_TOLERANCE);
    $('#txtProdDimOther').text(jsonResult.pP_PROD_DIM_OTHER);
    $('#txtProdOrderRemark').text(jsonResult.pP_PROCESS_PLAN_REMARK);
    $('#txtIntDiaMin').text(jsonResult.pP_COIL_DIA_INTERNAL);
    $('#txtExtDiaMax').text(jsonResult.pP_COIL_DIA_EXTERNAL);
    $('#txtWindingDirection').text(jsonResult.pP_COIL_DIA_WINDING);
    $('#txtProdDimD').text(jsonResult.pP_PROD_DIM_D);
    $('#hdProcessPlanCD').val(jsonResult.pP_PROCESS_PLAN_CD);
    $('#hdMsgCD').val(jsonResult.MSG_CD);
    $('#hdfNifudaCD').val(jsonResult.nifudA_CD);
    $('#ddlCondition').val(jsonResult.pP_CONDITION_CD)

}

$(document).on('click', '#btnInitialize', function (event) {
    initialize();

});

function clearPanel($panel) {
    // Set specified panel elements to disabled
    $.each($panel.find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });
    $.each($panel.find("select"), function (index, dropdown) {
        $("#" + dropdown.id + " option:selected").prop("selected", false);
        $("#" + dropdown.id + " option:first").prop("selected", "selected");
    });
    $.each($panel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).prop('checked', false);
    });

    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}

//bind next Process dropdown
function setNextProcess() {
    debugger;
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindNextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            //var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
    debugger;
    var count = $('#tblWIPTensou tbody tr').length;
    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);
        var $ddlNextProcess = $tr.find("#ddlNextProcess");
        // $ddlNextProcess.html("").append($('<option value="0"></option>'));
        if ($("#hdfCultureName").val() == "en") {
            $.each(jsonResult, function (key, jsonObj) {
                $ddlNextProcess.append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
            });
        } else {
            $.each(jsonResult, function (key, jsonObj) {
                $ddlNextProcess.append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
            });
        }
    });
}


$("#btnSelect").click(function event() {
    location.href = "/WIPTensou/WIPTensouList";
    window.resizeTo(1100, 650);
});

$("#btnTransfer").click(function event() {
    location.href = "/WIPTensou//WIPTensouEntryEditDelete";
    window.resizeTo(1100, 990);
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

function setProductUnitKubun() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/WIPTensou/GetUnitType",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindproductunitkubundropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindproductunitkubundropdown(jsonResult) {
    debugger;
    var count = $('#tblWIPTensou tbody tr').length;
    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);
        var $ddlUnits = $tr.find("#ddlUnits");
        //  $ddlUnits.html("").append($('<option value="0"></option>'));
        $.each(jsonResult, function (key, jsonObj) {
            $ddlUnits.append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });
    });

}


function setSourceLocation(USER_ID, $callFrom) {

    debugger;
    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_ID: '" + USER_ID + "', USER_NAME: '" + userName + "' }";
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

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;
            var jsonResult = jResult[0];
            $('#txtSource1').val(jsonResult.name);

            // bindlocationdropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setForwadingDestination() {

    var ajaxTime = new Date().getTime();
    // var jsonData = "{USER_ID: '" + USER_ID + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetForwordingLocation",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;

            bindlocationdropdown(response.message);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindlocationdropdown(jsonResult) {

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTensoDestination").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setVehicleName() {

    var ajaxTime = new Date().getTime();
    // var jsonData = "{USER_ID: '" + USER_ID + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetVehicleName",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;

            bindVehicledropdown(response.message);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindVehicledropdown(jsonResult) {

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlVehicleName").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function initialize() {
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlTensouDetails"));
    clearPanel($("#pnlKSKList"));
    clearPanel($("#dvSearch"));
    $('#hdProcessPlanCD').val("");
    $('#hdfTransfer_CD').val("");
    $('#hdMsgCD').val("");
    InitSortPopup();
    $("#btnSave").removeAttr("disabled");
    $("#btnSelect").removeAttr("disabled");
    disableEditDeleteButtons();
    setSourceLocation($("#hdfLoginId").val());
    $('[id="ddlUnits"]').each(function () {
        $(this).prop('selectedIndex', 0).trigger('change');
    });
    $('[id="ddlNextProcess"]').each(function () {
        $(this).prop('selectedIndex', 0).trigger('change');
    });
    setStartSysDateTime();
    $('#ddlCondition').val("");
}

function formatStartEndDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('/');
}

$(document).on('click', '#btnSave', function (event) {
    debugger;
    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();

    debugger;
    //Transfer date required.
    if ($("#txtDate").val() == "") {
        var message = $("#lblErrorMessageE_WIPTensou_003").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtDate", WARNING);
        $("#txtDate").focus;
        return false;
    }

    //Transfer date Invalid.
    if (!checkDate($("#txtDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_005").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtDate", WARNING);
        $("#txtDate").focus;
        return false;
    }
    //Acceptance date required.
    if ($("#txtPlanDate").val() == "") {
        var message = $("#lblErrorMessageE_WIPTensou_021").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtPlanDate", WARNING);
        $("#txtPlanDate").focus;
        return false;
    }
    //Acceptance date Invalid.
    if (!checkDate($("#txtPlanDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_022").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtPlanDate", WARNING);
        $("#txtPlanDate").focus;
        return false;
    }


    var transferDate = document.getElementById("txtDate").value;
    var planDate = document.getElementById("txtPlanDate").value;

    var inspStartDate = formatStartEndDate(transferDate);
    var inspEndDate = formatStartEndDate(planDate);

    if (inspEndDate < inspStartDate) {
        var message = $("#lblErrorMessageE_WIPTensou_023").text().trim();
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(message, "txtDate", WARNING);
        return false;
    }
    
    //Date Range
    if (!IsDateDifferentFromRange(true)) {
        var message = $("#lblErrorMessageE_WIPTensou_013").text().trim() + "(" + $("#lblErrorMessageE_WIPTensou_014").text().trim() + commonDateRange + ")";
        var focusFieldID = "txtDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    if (!IsDropDownMandatory()) {
        return false;
    }

    if (!IsMandatoryFieldsFilledOnGrid()) {
        return false;
    }

    if ($("#hdfDBOperation").val() == "Insert") {
        SaveWIPTensouDetails($callFrom);
    }
    else {
        updateWIPTensouDetails();
    }
});

//save data
function SaveWIPTensouDetails($callFrom) {
    debugger;
    var i = 0;
    var ajaxTime = new Date().getDate();
    var gridData = [];
    var unit = [];
    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);
        var noOfUnits = $tr.find("#txtNoOfUnits").val();
        if (noOfUnits != "") {
            unit.push(noOfUnits);
        }
    });

    const n = unit.length;
    var jsonDataForSave = [];

    $('table#tblWIPTensou tbody tr').each(function (index, item) {

        if (i < unit.length) {
            var $tr = $(this);

            jsonDataForSave = {
                WIP_TENSOU_ROW: i,
                WIP_TRANSFER_CD: WIP_TRANSFER_CD ? WIP_TRANSFER_CD : null,
                PROCESS_PLAN_CD: $("#hdProcessPlanCD").val() ? $("#hdProcessPlanCD").val() : null,
                LOCATION_CD_TO: $("#ddlTensoDestination").val() ? $("#ddlTensoDestination").val() : null,
                START_VEHICLEL_CD: $("#ddlVehicleName").val() ? $("#ddlVehicleName").val() : null,
                NIFUDA_CD: $("#hdfNifudaCD").val() ? $("#hdfNifudaCD").val() : null,
                //TRANSFER_DATE: ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                TRANSFER_DATE_STRING: $("#txtDate").val(),
                TABA_NO_1: $tr.find("#txtbundleNo1").val() ? $tr.find("#txtbundleNo1").val() : null,
                TABA_NO_2: $tr.find("#txtbundleNo2").val() ? $tr.find("#txtbundleNo2").val() : null,
                TABA_NO_3: $tr.find("#txtbundleNo3").val() ? $tr.find("#txtbundleNo3").val() : null,
                TABA_NO_4: $tr.find("#txtbundleNo4").val() ? $tr.find("#txtbundleNo4").val() : null,
                NO_OF_UNITS: $tr.find("#txtNoOfUnits").val() ? $tr.find("#txtNoOfUnits").val() : 0,
                UNIT_TYPE: $tr.find("#ddlUnits").val() ? $tr.find("#ddlUnits").val() : 0,
                QTY: $tr.find("#txtWeight").val() ? $tr.find("#txtWeight").val() : 0,
                MSG_CONTENTS: $tr.find("#txtMessage").val() ? $tr.find("#txtMessage").val() : null,
                NEXT_PROSESS: $tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : 0,
                USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
                USER_NAME: userName ? userName : null,
                PP_CONDITION_CD: $("#ddlCondition").val() != 0 ? $("#ddlCondition").val() : null,
                ACCEPT_PLANNED_DATE: $("#txtPlanDate").val()// Add by komal 
            }
            i++;
            gridData.push(jsonDataForSave);
        }

    });
    
    var settings = {

        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/SaveWIPTensouDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        // data: gridData
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            /* After StartData is saved sucessfully, enable EndData Panel for input, 
              and show success message. */
            //disableEndDataPanel();
            debugger;
            var jsonResult = response.message;
            //let transferGrpCD = jsonResult.wiP_TRANSFER_GROUP_NO;
            //$("#hdWIPTensouCD").val(transferGrpCD);

            $.each(jsonResult, function (index, jsonObj) {
                $("#hdfTransfer_CD" + index).val(jsonObj.wiP_TRANSFER_CD);
                $("#hdWIPTensouCD").val(jsonObj.wiP_TRANSFER_GROUP_NO);
            });
            //$("#hdfTransfer_CD").val(transferCD);
            var successMessage = $("#lblSuccessMessageS_WIPTensou_001").text();
            showMessageDialog(successMessage, "txtEndOperator", SUCCESS);
            $("#btnSave").attr("disabled", "disabled");
            enableEditDeleteButtons();
            //if ($("#txtDate").val == null) {
            //    var message = $("#lblErrorMessageE_WIPTensou_003").text().trim(); // This message will be displayed on dialog box
            //    showMessageDialog(message, $("#txtDate"), WARNING);
            //    $("#txtDate").focus;
            //    return false;

            //}
            // setChuubanDetails(jsonResult);


        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });


}


$(document).on('click', '#btnEdit', function (event) {

    //Transfer date required.
    if ($("#txtDate").val() == "") {
        var message = $("#lblErrorMessageE_WIPTensou_003").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtDate", WARNING);
        $("#txtDate").focus;
        return false;
    }

    //Transfer date Invalid.
    if (!checkDate($("#txtDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_005").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtDate", WARNING);
        $("#txtDate").focus;
        return false;
    }
    //Acceptance date required.
    if ($("#txtPlanDate").val() == "") {
        var message = $("#lblErrorMessageE_WIPTensou_021").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtPlanDate", WARNING);
        $("#txtPlanDate").focus;
        return false;
    }
    //Acceptance date Invalid.
    if (!checkDate($("#txtPlanDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_022").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtPlanDate", WARNING);
        $("#txtPlanDate").focus;
        return false;
    }
    var transferDate = document.getElementById("txtDate").value;
    var planDate = document.getElementById("txtPlanDate").value;

    var inspStartDate = formatStartEndDate(transferDate);
    var inspEndDate = formatStartEndDate(planDate);

    if (inspEndDate < inspStartDate) {
        var message = $("#lblErrorMessageE_WIPTensou_023").text().trim();
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(message, "txtDate", WARNING);
        return false;
    }
    
    if (!IsMandatoryFieldsFilledOnGrid()) {
        return false;
    }

    updateWIPTensouDetails();
});

//Update Date
function updateWIPTensouDetails() {
    debugger;
    resetResponseTime();
    var $callFrom = $(this);
    var i = 0;
    var ajaxTime = new Date().getDate();
    var gridData = [];
    var thaba = [];
    let TransferCD;

    const queryString1 = window.location.search;
    if (queryString1 != "") {
        TransferCD = queryString1.split('=')[1];
        $("#hdWIPTensouCD").val(TransferCD);
    }

    var drop = $("#ddlTensoDestination").val();
    if ($("#ddlTensoDestination").val() == null) {
        var message = $("#lblErrorMessageE_WIPTensou_006").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "ddlTensoDestination", WARNING);
        $("#ddlTensoDestination").focus;
        return false;
    }

    // Check Done - Acceptance then show validation message (Transfer For Process cannot be edited).
    if (TransferCD && TransferCD !== "0") {
      
        IsAcceptanceCompleted(TransferCD, $callFrom);
        if (IsAcceptExists !== false) {
            return false;
        }
    }
    else {
        var tensouCD = $("#hdWIPTensouCD").val();
        if (tensouCD && tensouCD !== "0") {
            
            IsAcceptanceCompleted(tensouCD, $callFrom);
            if (IsAcceptExists !== false) {
                return false;
            }
        }
    }

    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);
        var bundleNo1 = $tr.find("#txtNoOfUnits").val();
        if (bundleNo1 != "") {
            thaba.push(bundleNo1);
        }
    });

    const n = thaba.length;
    var jsonDataForUpdate = [];

    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        if (i < n) {

            var $tr = $(this);

            jsonDataForUpdate = {
                WIP_TENSOU_ROW: i,
                TRANSFER_CD: $tr.find("#hdfTransfer_CD" + i).val() ? $tr.find("#hdfTransfer_CD" + i).val() : 0,//$("#hdfTransfer_CD").val() ? $("#hdfTransfer_CD").val() : 0,//WIP_TRANSFER_CD ? WIP_TRANSFER_CD : null,
                //SERIAL_NUMBER: i + 1,
                WIP_TRANSFER_GROUP_NO: $("#hdWIPTensouCD").val() ? $("#hdWIPTensouCD").val() : 0,
                PROCESS_PLAN_CD: $("#hdProcessPlanCD").val() ? $("#hdProcessPlanCD").val() : 0,
                // LOCATION_CD_TO: $tr.find("#ddlTensoDestination").val() ? $tr.find("#ddlTensoDestination").val() : 0,
                LOCATION_CD_TO: $("#ddlTensoDestination").val() ? $("#ddlTensoDestination").val() : null,
                START_VEHICLEL_CD: $("#ddlVehicleName").val() ? $("#ddlVehicleName").val() : null,
                NIFUDA_CD: $("#hdfNifudaCD").val() ? $("#hdfNifudaCD").val() : null,
                TRANSFER_DATE_STRING: $("#txtDate").val(),
                TABA_NO_1: $tr.find("#txtbundleNo1").val() ? $tr.find("#txtbundleNo1").val() : null,
                TABA_NO_2: $tr.find("#txtbundleNo2").val() ? $tr.find("#txtbundleNo2").val() : null,
                TABA_NO_3: $tr.find("#txtbundleNo3").val() ? $tr.find("#txtbundleNo3").val() : null,
                TABA_NO_4: $tr.find("#txtbundleNo4").val() ? $tr.find("#txtbundleNo4").val() : null,
                NO_OF_UNITS: $tr.find("#txtNoOfUnits").val() ? $tr.find("#txtNoOfUnits").val() : 0,
                UNIT_TYPE: $tr.find("#ddlUnits").val() ? $tr.find("#ddlUnits").val() : 0,
                QTY: $tr.find("#txtWeight").val() ? $tr.find("#txtWeight").val() : 0,
                MSG_CONTENTS: $tr.find("#txtMessage").val() ? $tr.find("#txtMessage").val() : null,
                MSG_CD: $("#hdMsgCD").val() ? $("#hdMsgCD").val() : 0,
                NEXT_PROSESS: $tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : 0,
                USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
                USER_NAME: userName ? userName : null,
                PP_CONDITION_CD: $("#ddlCondition").val() != 0 ? $("#ddlCondition").val() : null,
                ACCEPT_PLANNED_DATE: $("#txtPlanDate").val()//Add by komal
            }
            i++;
            gridData.push(jsonDataForUpdate);
        }
    });
    //debugger;
    var jsonData = "{"
        // START DATA

        + "WIP_TRANSFER_CD" + " : '" + WIP_TRANSFER_CD + "',"
        + "PROCESS_PLAN_CD" + " :  '" + $("#hdProcessPlanCD").val() + "',"
        + "LOCATION_CD_TO" + " : '" + $("#ddlTensoDestination").val() + "',"
        + "TABA_NO_1" + " : '" + $("#txtbundleNo1").val() + "',"
        + "TABA_NO_2" + " : '" + $("#txtbundleNo2").val() + "',"
        + "TABA_NO_3" + " : '" + $("#txtbundleNo3").val() + "',"
        + "TABA_NO_4" + " : '" + $("#txtbundleNo4").val() + "',"
        + "NO_OF_UNITS" + " : '" + $("#txtNoOfUnits").val() + "',"
        + "UNIT_TYPE" + " : '" + $("#ddlUnits option:selected").val() + "',"
        + "QTY" + " : '" + $("#txtWeight").val() + "',"//
        + "MSG_CONTENTS" + " : '" + $("#txtMessage").val() + "',"
        + "NEXT_PROSESS" + " : '" + $("#ddlNextProcess").val() + "',"
        + "USER_ID" + " : '" + $("#hdfLoginId").val() + "',"
        + "USER_NAME" + " : '" + userName
        + "'}";
    //var jsonDataSaveEditDelete = {

    //    WIP_TRANSFER_CD: parseInt($("#hdWIPTensouCD").val()),
    //    PROCESS_PLAN_CD: parseInt($("#hdProcessPlanCD").val()),

    //    LOCATION_CD_TO: $("#ddlTensoDestination").val(),
    //    WIP_TRANSFER_CD: parseInt($("#hdWIPTensouCD").val()),
    //    TABA_NO_1: $("#txtbundleNo1").val(),
    //    TABA_NO_2: $("#txtbundleNo2").val() ? $("#txtbundleNo2").val() : null,
    //    TABA_NO_3: $("#txtbundleNo3").val() ? $("#txtbundleNo3").val() : null,
    //    TABA_NO_4: $("#txtbundleNo4").val() ? $("#txtbundleNo4").val() : null,
    //    NO_OF_UNITS: $("#txtNoOfUnits").val(),
    //    // TRANSFER_DATE: $("#txtDate").val(),
    //    TRANSFER_DATE: ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
    //    UNIT_TYPE: $("#ddlUnits option:selected").val(),
    //    QTY: $("#txtWeight").val(),
    //    MSG_CONTENTS: $("#txtMessage").val() ? $("#txtMessage").val() : null,
    //    NEXT_PROSESS: parseInt($("#ddlNextProcess").find(":selected").val()),
    //    USER_ID: parseInt($("#hdfLoginId").val()),

    //}

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/WIPTensouDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({STAMPING_CD: $("#hfld_StamppingCd").val(),MACHINE_CD: $("#ddlModel").val(),PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),MSG_CD: parseInt($("#hfld_MessageCD").val()),ST_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,ST_START_DATE: $("#txtStartDate").val() + " " + $("#txtStartTime").val(),ST_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),ST_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val(),ST_DECISION: parseInt($("#ddlDecision").val()),ST_NG_QTY: parseFloat($("#txtNGQty").val()),ST_OK_QTY: parseFloat($("#txtNgWeight").val()),ST_NG_BARS1: parseInt($("#txtNGWeight1").val()),ST_NG_DETAILS1: parseInt($("#ddlDefects1").val()),ST_NG_BARS2: parseInt($("#txtNGWeight2").val()),ST_NG_DETAILS2: parseInt($("#ddlDefects2").val()),ST_NG_BARS3: parseInt($("#ddlDefects3").val()),ST_NG_DETAILS3: parseInt($("#txtNGWeight3").val()),ST_TABA_NO: parseInt($("#txtBundleNo").val()),ST_OK_BARS: parseInt($("#txtFinishedBarCount").val()),UNIT_TYPE: parseInt($("#ddlUnitType").val()),ST_END_DATE: $("#txtEndDate").val() + " " + $("#txtEndTime").val(),ST_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),ST_NEXT_PROCESS: parseInt($("#ddlNextProcess").val()),ST_REMARKS: $("#txtRemarks").val(),USER_NAME: userName})
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            $.each(jsonResult, function (index, jsonObj) {
                $("#hdfTransfer_CD" + index).val(jsonObj.wiP_TRANSFER_CD);
                $("#hdWIPTensouCD").val(jsonObj.wiP_TRANSFER_GROUP_NO);
            });
            // disableEditDeleteButtons();
            var successMessage = $("#lblSuccessMessageS_WIPTensou_DataUpdated").text();
            showMessageDialog(successMessage, "Updated", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


}

function disableEndDataPanel() {
    // Set EndData Panel Elements to Disabled
    var $endDataPanel = $("#pnlProcessEndData");
    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("select"), function (index, dropdown) {
        //$("#" + dropdown.id + " option:selected").prop("selected", false);
        //$("#" + dropdown.id + " option:first").prop("selected", "selected");
        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
}


//Select Code 
$(document).on('click', '#btnSearch', function (event) {
    debugger;
    //Required Field
    if ($('#txtDimDEndSearch').val()) {
        if (!$('#txtDimDStartSearch').val()) {
            var message = $("#lblErrorMessageE_WIPTensou_020").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtDimDStartSearch", WARNING);
            $("#txtDimDStartSearch").focus;
            return false;
        }
    }

    if ($('#txtTransferEndDateSearch').val()) {
        if (!$('#txtTransferStartDateSearch').val()) {
            var message = $("#lblErrorMessageE_WIPTensou_016").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtTransferStartDateSearch", WARNING);
            $("#txtTransferStartDateSearch").focus;
            return false;
        }
    }

    //Invalid Date
    var date = ($('#txtTransferStartDateSearch').val());
    if (date != "") {
        if (!checkDateOnListPage($("#txtTransferStartDateSearch"))) {
            var message = $("#lblErrorMessageE_WIPTensou_015").text(); // Transfer Start date
            showMessageDialog(message, "", WARNING);
            $("#txtTransferStartDateSearch").focus;
            return false;
        }
    }

    var endDate = ($('#txtTransferEndDateSearch').val());
    if (endDate != "") {
        if (!checkDateOnListPage($("#txtTransferEndDateSearch"))) {
            var message = $("#lblErrorMessageE_WIPTensou_015").text().trim(); // Transfer End date
            showMessageDialog(message, "", WARNING);
            $("#txtTransferEndDateSearch").focus;
            return false;
        }
    }


    var CULTURE = $("#hdfCultureName").val();
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerSearch").val();
    var PROD_NAME_CUST = $("#txtProdNameSearch").val();
    var INTERNAL_NAME_STRING = $("#txtInternalNameSearch").val();
    var PP_DIM_TOL_MIN = $("#txtDimDStartSearch").val();
    var PP_DIM_TOL_MAX = $("#txtDimDEndSearch").val();
    var PROD_SHAPE = $("#txtShapeSearch").val();
    var CHARGE = $("#txtChargeSearch").val();
    var TRANSFER_DATE_START = $("#txtTransferStartDateSearch").val();
    var TRANSFER_DATE_END = $("#txtTransferEndDateSearch").val();
    var USER_ID = $("#hdfLoginId").val();

    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PP_DIM_TOL_MIN.length > 0 || PP_DIM_TOL_MAX.length > 0 || PROD_SHAPE.length > 0 || CHARGE.length > 0 || TRANSFER_DATE_START.length > 0 || TRANSFER_DATE_END.length > 0) {
        var ajaxTime = new Date().getTime();
        // var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch,txtCustomerSearch:txtCustomerSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/TensouDataSearchInGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PP_DIM_TOL_MIN: PP_DIM_TOL_MIN, PP_DIM_TOL_MAX: PP_DIM_TOL_MAX, PROD_SHAPE: PROD_SHAPE, CHARGE: CHARGE, TRANSFER_DATE_START: TRANSFER_DATE_START, TRANSFER_DATE_END: TRANSFER_DATE_END,
                USER_ID: USER_ID, CULTURE_NAME: CULTURE,
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    debugger;
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/WIPTensou/TensouDataSearchInGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        // data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;
                            jsonResult = response1.message;
                            if (response1.isSuccess) {
                                $("#divTensouGridMain").html(response1.message);
                            }
                            //else {
                            //    var message = response1.message; // This message will be displayed on dialog box
                            //        showMessageDialog(message, "", WARNING);
                            //        return false;
                            //}

                            //if (!checkDate($("#txtTransferStartDateSearch"))) {
                            //    var message = $("#lblErrorMessageE_WIPTensou_005").text().trim(); // This message will be displayed on dialog box
                            //    showMessageDialog(message, "txtTransferStartDateSearch", WARNING);
                            //    $("#txtTransferStartDateSearch").focus;
                            //    return false;
                            //}


                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                else {
                    bootbox.alert({
                        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                        message: response.message,
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });

    } else {

        var Message = $("#lblErrorMessageE_WIPTensou_017").text();
        showMessageDialog(Message, "", WARNING);
        // focusFieldID = $(':focus').attr("id");
        //       var successMessage = $.trim($("#lblErrorMessageE_WIPTensou_014").text()); 
        //     showMessageDialog(successMessage, "", WARNING);
    }

});

function getWIPTransferDetailsByTrasferCD($callFrom) {

    EditCheck = "0";
    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var TransferCD = $.trim($callFrom.closest("tr").attr("data-lblWIPTransferCD"));
    var CultureName = $("#hdfCultureName").val();
    // var jsonData = "{WIP_TRANSFER_CD :'" + TransferCD + "'}";
    var jsonTensouDetailData = "{WIP_TRANSFER_CD : '" + TransferCD + "' , CULTURE_NAME : '" + CultureName + "' , USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/getWIPTransferDetailsByTrasferCD",
        // url: APIURL + "/WIPTensou/getWIPTransferDetailsByTrasferCD?TransferCD=" + TransferCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ WIP_TRANSFER_CD: TransferCD, CULTURE_NAME: CultureName })

    }
    $.ajax(settings)
        .done(function (response) {
            debugger;

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            setWIPTensouDetails(jsonResult);
            location.href = "/WIPTensou/WIPTensouEntryEditDelete?WIP_TRANSFER_GROUP_NO=" + TransferCD;  //list page to pass id on edit delete screen(list page close & open edit delete screen)


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function getTransferDetailsByTrasferCD(TransferCD) {
    window.resizeTo(1050, 990);
    EditCheck = "1";

    var ajaxTime = new Date().getTime();
    var CultureName = $("#hdfCultureName").val();

    var jsonTensouDetailData = "{WIP_TRANSFER_CD : '" + TransferCD + "' , CULTURE_NAME : '" + CultureName + "' , USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        //url: APIURL + "/WIPTensou/getWIPTransferDetailsByTrasferCD?TransferCD=" + parseInt(TransferCD),
        url: APIURL + "/WIPTensou/getWIPTransferDetailsByTrasferCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ WIP_TRANSFER_CD: TransferCD, CULTURE_NAME: CultureName })
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            setWIPTensouDetails(jsonResult);
            //location.href = "/WIPTensou/WIPTensouEntryEditDelete?TransferCD=" + TransferCD;

            $("#btnUpdate").click(function (event) {
                debugger;
                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }

                if (!IsDropDownMandatory()) {
                    return false;
                }
                //if (!checkDate($("#txtDate"))) {

                //    var message = $("#lblErrorMessageE_WIPTensou_005").text().trim(); // This message will be displayed on dialog box
                //    showMessageDialog(message, $("#txtDate"), WARNING);
                //    $("#txtDate").focus;
                //    return false;
                //}

                if (EditCheck == 0) {
                    updateWIPTensouDetails();
                    EditCheck = "1";
                }

                $("#txtDate").focus();
            });


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setWIPTensouDetails(jsonResult) {

    debugger;
    $('#ddlModel option').each(function () {
        if (this.value == jsonResult.machinE_CD)
            IsExists = true;
    });

    var i = 0;

    if (IsExists === false) {
        $("#ddlModel").append(new Option(jsonResult.dW_MACHINE_NAME, jsonResult.machinE_CD));
    }

    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#txtCustomer").text(jsonResult[0].pP_CUSTOMER_NAME);
    $("#txtDeliveryDate").text(jsonResult[0].pP_DELIVERY_DATE_STRING);
    $('#txtCustomerIncharge1').text(jsonResult[0].pP_EMPLOYEE_NAME);
    //$("#txtStdDeliveryDate").text(jsonResult[0].pP_STD_DEL_PERIOD);
    if (jsonResult[0].pP_STD_DEL_PERIOD == 0) {
        $('#txtStdDeliveryDate').text("");
    } else {
        $('#txtStdDeliveryDate').text(jsonResult.pP_STD_DEL_PERIOD);
    }
    $('#txtReceiver').text(jsonResult[0].pP_RECEIVER_NAME);
    // $('#txtChamfer').text(jsonResult[0].pP_CHAMFER);
    if (jsonResult[0].pP_CHAMFER == 0) {
        $('#txtChamfer').text("");
    } else {
        $('#txtChamfer').text(jsonResult[0].pP_CHAMFER);
    }
    //$('#txtUnitWt').text(jsonResult[0].pP_UNIT_WT);
    if (parseInt(jsonResult[0].pP_UNIT_WT_KUBUN_STRING) > 0) {
        $('#txtUnitWt').text(jsonResult[0].pP_UNIT_WT_KUBUN_STRING);
    }
    //if (jsonResult[0].pP_UNIT_WT == 0) {
    //    $('#txtUnitWt').text("");
    //} else {
    //    $('#txtUnitWt').text(jsonResult[0].pP_UNIT_WT);
    //}
    $('#txtColor').text(jsonResult[0].pP_COLOR);
    $('#txtMaterialNameMaker').text(jsonResult[0].pP_MATL_NAME_MAK);
    $('#txtMatlShape').text(jsonResult[0].pP_MATL_SHAPE);
    $('#txtMatlDimD').text(jsonResult[0].pP_MATL_DIM_D);
    $('#txtCharge').text(jsonResult[0].pP_CHARGE);
    $('#txtBendStandard').text(jsonResult[0].pP_MAGARI_BEND_TOL);
    $('#txtExtTexture').text(jsonResult[0].pP_EXT_TEXTURE);
    $('#txtDisplay').text(jsonResult[0].pP_DISPLAY);
    $('#txtPacking').text(jsonResult[0].pP_PACKING);
    $('#txtLengthTolerance').text(jsonResult[0].pP_LEN_TOL);
    $('#txtLength').text(jsonResult[0].pP_PROD_LENGTH_STRING);
    $('#txtProdShape').text(jsonResult[0].pP_PROD_SHAPE);
    $('#txtProdNameCustomer').text(jsonResult[0].pP_PROD_NAME_CUST);
    $('#txtQuantity').text(jsonResult[0].pP_ACT_ALLOC_QTY);
    $('#txtKakouiraiRemarks').text(jsonResult[0].pP_KAKOUIRAI_REMARKS);
    // $('#txtHostRefNo').text(jsonResult[0].pP_PROD_LENGTH_STRING);
    $('#txtStamp').text(jsonResult[0].pP_STAMP);
    $('#txtNoOfUnitsT1').text(jsonResult[0].pP_NO_OF_UNITS_DECODE);
    $('#txtDimTolerance').text(jsonResult[0].pP_DIM_TOLERANCE);
    $('#txtProdDimOther').text(jsonResult[0].pP_PROD_DIM_OTHER);
    $('#txtProdOrderRemark').text(jsonResult[0].pP_PROCESS_PLAN_REMARK);
    $('#txtIntDiaMin').text(jsonResult[0].pP_COIL_DIA_INTERNAL);
    $('#txtExtDiaMax').text(jsonResult[0].pP_COIL_DIA_EXTERNAL);
    $('#txtWindingDirection').text(jsonResult[0].pP_COIL_DIA_WINDING);
    $('#txtProdDimD').text(jsonResult[0].pP_PROD_DIM_D);
    $('#hdProcessPlanCD').val(jsonResult[0].pP_PROCESS_PLAN_CD);
    $('#hdMsgCD').val(jsonResult[0].MSG_CD);
    $('#txtDate').val($.format.date(new Date(jsonResult[0].transfeR_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

    $('#txtChuuban').val(jsonResult[0].pP_CHUUBAN);
    // $('#txtGyoban').val(jsonResult[0].pP_GYOUBAN);
    $('#txtNifuda').val(jsonResult[0].nifudA_NO);
    $('#ddlTensoDestination').val(jsonResult[0].locatioN_CD_TO);
    $('#ddlVehicleName').val(jsonResult[0].starT_VEHICLEL_CD);
    $('#ddlCondition').val(jsonResult[0].pP_CONDITION_CD);
    debugger;
    $('#txtPlanDate').val($.format.date(new Date(jsonResult[0].planneD_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    //Grid Data
    $('table#tblWIPTensou tbody tr').each(function () {
        if (i < jsonResult.length) {
            debugger;
            //  $(this).find('#ddlTensoDestination').val(jsonResult[i].LOCATION_CD_TO);
            $(this).find('#txtbundleNo1').val(jsonResult[i].tabA_NO_1);
            $(this).find('#txtbundleNo2').val(jsonResult[i].tabA_NO_2);
            $(this).find('#txtbundleNo3').val(jsonResult[i].tabA_NO_3);
            $(this).find('#txtbundleNo4').val(jsonResult[i].tabA_NO_4);
            $(this).find('#txtNoOfUnits').val(jsonResult[i].nO_OF_UNITS);
            $(this).find('#ddlUnits').val(jsonResult[i].uniT_TYPE);
            $(this).find('#ddlWindingshape').val(jsonResult[i].dW_WINDING_SHAPE);
            $(this).find('#txtWeight').val(jsonResult[i].qty);
            $(this).find('#ddlNextProcess').val(jsonResult[i].nexT_PROSESS);
            $(this).find('#txtMessage').val(jsonResult[i].msG_CONTENTS);
            $(this).find('#hdfTransfer_CD' + i).val(jsonResult[i].transfeR_CD);

        }
        i++;
    });


}


//function IsMandatoryFieldsFilled() {
//    debugger;
//    var returnVal = true;  // default true is required for screens with no mandatory fields

//    var message = ""; // This message will be displayed on dialog box
//    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
//    var focusFieldID = "";

//    $.each($.find(".Mandatory"), function (index, mandatoryField) {
//        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
//            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
//                returnVal = false;
//                message = message + "<br>" + $(mandatoryField).attr("errorLabel");
//                if (focusFieldID === "") {
//                    focusFieldID = $(mandatoryField).attr("id");
//                }
//            }
//        }
//        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
//            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
//                returnVal = false;
//                message = message + "<br>" + $(mandatoryField).attr("errorLabel");
//                if (focusFieldID === "") {
//                    focusFieldID = $(mandatoryField).attr("id");
//                }
//            }
//        }
//    });

//    if (message !== "") {
//        $("#dialogMessageIcon img").hide();
//        //  $("#imgError").show();

//        showMessageDialog(message, focusFieldID, WARNING)

//    }
//    return returnVal;
//}

//sort popup
function WIPTensouGridSearch(sortFields) {


    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerSearch").val();
    var txtProdNameCust = $("#txtProdNameSearch").val();
    var txtProdNameInt = $("#txtInternalNameSearch").val();
    var txtDimDMin = $("#txtDimDStartSearch").val();
    var txtDimDMax = $("#txtDimDEndSearch").val();
    var txtShap = $("#txtShapeSearch").val();
    var txtcharge = $("#txtChargeSearch").val();
    var txtTransferStartDate = $("#txtTransferStartDateSearch").val();
    var txtTransferEndDate = $("#txtTransferEndDateSearch").val();
    var userId = $("#hdfLoginId").val();

    if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtcharge.length > 0 || txtTransferStartDate.length > 0 || txtTransferEndDate.length > 0) {

        var ajaxTime = new Date().getTime();
        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/TensouDataSearchInGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                txtChuubanSearch: txtChuubanSearch, txtCustomerName: txtCustomerName, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax,
                userId: userId, sortFields: sortFields
            })
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
                        url: "/WIPTensou/TensouDataSearchInGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divColdDrawingWireDrawingGridMain").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                else {
                    bootbox.alert({
                        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                        message: response.message,
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {

        if (sortFields.length > 0) {
            var ajaxTime = new Date().getTime();
            //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
            var settings = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/WIPTensou/TensouDataSearchInGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    userId: userId, sortFields: sortFields
                })
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
                            url: "/WIPTensou/TensouDataSearchInGrid",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)
                            //data: "{jsonData : " + jsonData + "}"
                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#divColdDrawingWireDrawingGridMain").html(response1.message);

                                }
                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                var errorCode = $.parseJSON(xhr.responseText).Message;

                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                                showMessageDialog(message, focusFieldID, ERROR);
                            });
                    }
                    else {
                        bootbox.alert({
                            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                            message: response.message,
                            centerVertical: true
                        });
                    }

                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        }
        //bootbox.alert({
        //    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
        //    message: $.trim($("#lblErrorMessageE_ColdDrawing_SearchField").text()),
        //    centerVertical: true
        //});
    }
    closepopup();
}

function IsAcceptanceCompleted(TransferCD,$callFrom) {
    debugger;
    
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var CultureName = $("#hdfCultureName").val();

    var jsonTensouDetailData = "{WIP_TRANSFER_CD : '" + TransferCD + "' , CULTURE_NAME : '" + CultureName + "' , USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/IsAcceptanceCompleted",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ WIP_TRANSFER_CD: TransferCD, CULTURE_NAME: CultureName })

    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            IsAcceptExists = false;
            $.each(jsonResult, function (index, item) {

                if (item.acceptancE_DATE != null && item.acceptancE_DATE !== "") {
                    var message = $("#lblErrorMessageE_TransferForProcessCanNotEdit").text().trim();
                    showMessageDialog(message, "", WARNING);
                    IsAcceptExists = true;

                   // return false;
                }
            });
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function btnSortOk_Click() {

    var selectedColumns = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            selectedColumns.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });
    WIPTensouGridSearch(selectedColumns);
}

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})
$(document).on('click', '#btnTransfer', function () {
    location.href = "WIPTensouEntryEditDelete";
})


function btnSortClose_Click() {
    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}

function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortAdd_Click() {
    var selectedColumns = $("#lstSortFields").val();
    if (selectedColumns.length > 0) {
        $("#lstSortFields option:selected").each(function () {
            var $this = $(this);
            if ($this.length) {
                var selText = $this.text();
                console.log(selText);
                $('#lstSelectedSortFields').append($("<option></option>")
                    .attr("value", $this.val())
                    .text(selText + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
                $this.remove();
            }
        });
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

function btnSortRemove_Click() {
    var selectedColumns = $("#lstSelectedSortFields").val();
    if (selectedColumns.length > 0) {
        var selectedColumns = $("#lstSelectedSortFields").val();
        if (selectedColumns.length > 0) {
            $("#lstSelectedSortFields option:selected").each(function () {
                var $this = $(this);
                if ($this.length) {
                    var selText = $this.text().split(' ');
                    console.log(selText);
                    selText.splice(-1);
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", $this.val())
                        .text(selText.join(' ')));
                    $this.remove();
                }
            });
        }
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/WIPTensou/InitializeSortPopup",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            $('#lstSortFields').find('option')
                .remove()
                .end();
            $('#lstSelectedSortFields').find('option')
                .remove()
                .end();
            for (var i = 0; i < response.message.length; i++) {
                if (response.message[i].isDefaultSort) {
                    $('#lstSelectedSortFields').append($("<option></option>")
                        .attr("value", response.message[i].fieldName)
                        .text(response.message[i].displayName + GetSortDirectionSuffix(response.message[i].sortOrder)));
                }
                else {
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", response.message[i].fieldName)
                        .text(response.message[i].displayName));
                }

            }
            EnableDisableSortButtons();
            $("#rdoSortAsc").prop("checked", true);
            $("#rdoSortDsc").prop("checked", false);
        })
}

function EnableDisableSortButtons() {
    var available = $("#lstSortFields option").length;
    var selected = $("#lstSelectedSortFields option").length;
    if (available > 0) {
        $("#SortPopUpModal #btnAdd").prop('disabled', false);
    }
    else {
        $("#SortPopUpModal #btnAdd").prop('disabled', true);
    }

    if (selected > 0) {
        $("#SortPopUpModal #btnRemove").prop('disabled', false);
    }
    else {
        $("#SortPopUpModal #btnRemove").prop('disabled', true);
    }
}

function btnSortReset_Click() {
    InitSortPopup();
}

//Delete
$("#btnDelete").click(function (event) {
    deleteWIPTensouDetails();
});

function deleteWIPTensouDetails() {

    var ajaxTime = new Date().getTime();

    var i = 0;
    var ajaxTime = new Date().getDate();
    var gridData = [];
    var thaba = [];

    const queryString1 = window.location.search;
    if (queryString1 != "") {
        let TransferCD = queryString1.split('=')[1];
        $("#hdWIPTensouCD").val(TransferCD);
    }

    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);
        var bundleNo1 = $tr.find("#txtNoOfUnits").val();
        if (bundleNo1 != "") {
            thaba.push(bundleNo1);
        }
    });

    const n = thaba.length;
    var jsonDataForDelete = [];


    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        if (i < n) {
            var $tr = $(this);

            jsonDataForDelete = {
                WIP_TENSOU_ROW: i,

                WIP_TRANSFER_CD: WIP_TRANSFER_CD ? WIP_TRANSFER_CD : null,
                SERIAL_NUMBER: i + 1,
                WIP_TRANSFER_GROUP_NO: $("#hdWIPTensouCD").val() ? $("#hdWIPTensouCD").val() : 0,
                PROCESS_PLAN_CD: $("#hdProcessPlanCD").val() ? $("#hdProcessPlanCD").val() : 0,
                LOCATION_CD_TO: $tr.find("#ddlTensoDestination").val() ? $tr.find("#ddlTensoDestination").val() : 0,
                TABA_NO_1: $tr.find("#txtbundleNo1").val() ? $tr.find("#txtbundleNo1").val() : null,
                TABA_NO_2: $tr.find("#txtbundleNo2").val() ? $tr.find("#txtbundleNo2").val() : null,
                TABA_NO_3: $tr.find("#txtbundleNo3").val() ? $tr.find("#txtbundleNo3").val() : null,
                TABA_NO_4: $tr.find("#txtbundleNo4").val() ? $tr.find("#txtbundleNo4").val() : null,
                NO_OF_UNITS: $tr.find("#txtNoOfUnits").val() ? $tr.find("#txtNoOfUnits").val() : 0,
                UNIT_TYPE: $tr.find("#ddlUnits").val() ? $tr.find("#ddlUnits").val() : 0,
                QTY: $tr.find("#txtWeight").val() ? $tr.find("#txtWeight").val() : 0,
                MSG_CONTENTS: $tr.find("#txtMessage").val() ? $tr.find("#txtMessage").val() : null,
                MSG_CD: $("#hdMsgCD").val() ? $("#hdMsgCD").val() : 0,
                NEXT_PROSESS: $tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : 0,
                USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
                USER_NAME: userName ? userName : null
            }
            i++;
            gridData.push(jsonDataForDelete);
        }
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/WIPTensou/WIPTensouDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            if (response.message == 1) {
                var successMessage = $("#lblSuccessMessageS_WIPTensou_DataDeleted").text();
                showMessageDialog(successMessage, "", SUCCESS);
                initialize();
            }
            else {
                /******    transaction scope blocking reference table name,
                            hence added following msg. ******/
                if (response.message == -1) {
                    var warningMessage = $.trim($("#lblErrorMessageE_TransferForProcessCanNotDeleted").text());
                    showMessageDialog(warningMessage, "", WARNING);
                }
                else {
                    if (response.message == 0) {
                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    }
                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

//Validation
function IsMandatoryFieldsFilled() {

    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box

    var focusFieldID = "";
    $.each($.find(".MandatoryTextBox"), function (index, mandatoryField) {

        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("errorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("errorLabel");
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

function IsMandatoryFieldsFilledOnGrid() {
    debugger;
    var i = 0;
    var gridData = [];
    var bNoData = [];

    //
    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);
        var bundleNo1 = $tr.find("#txtbundleNo1").val();
        var bundleNo2 = $tr.find("#txtbundleNo2").val();
        var bundleNo3 = $tr.find("#txtbundleNo3").val();
        var bundleNo4 = $tr.find("#txtbundleNo4").val();
        var nOfUnits = $tr.find("#txtNoOfUnits").val();
        var weight = $tr.find("#txtWeight").val();
        if (nOfUnits != "" || weight != "") {
            bNoData.push(1);
        }
    });
    var n = bNoData.length;


    $('table#tblWIPTensou tbody tr').each(function (index, item) {
        var $tr = $(this);

        if (n === 0) {
            n = n + 1;
        }
        if (i < n) {
            var data = {
                WIP_TENSOU_ROW: i,
                // WIP_TENSOU_ORDER_NO : transferNo,
                WIP_TENSOU_TABA_NO_1: $tr.find("#txtbundleNo1").val() ? $tr.find("#txtbundleNo1").val() : null,
                WIP_TENSOU_TABA_NO_2: $tr.find("#txtbundleNo2").val() ? $tr.find("#txtbundleNo2").val() : null,
                WIP_TENSOU_TABA_NO_3: $tr.find("#txtbundleNo3").val() ? $tr.find("#txtbundleNo3").val() : null,
                WIP_TENSOU_TABA_NO_4: $tr.find("#txtbundleNo4").val() ? $tr.find("#txtbundleNo4").val() : null,
                WIP_TENSOU_NO_OF_UNITS: $tr.find("#txtNoOfUnits").val() ? $tr.find("#txtNoOfUnits").val() : 0,
                WIP_TENSOU_UNIT_TYPE: $tr.find("#ddlUnits").val() ? $tr.find("#ddlUnits").val() : 0,
                WIP_TENSOU_QTY: $tr.find("#txtWeight").val() ? $tr.find("#txtWeight").val() : 0,
                WIP_TENSOU_MSG_CONTENTS: $tr.find("#txtMessage").val() ? $tr.find("#txtMessage").val() : null,
                WIP_TENSOU_NEXT_PROCESS: $tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : 0,//$("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : 0,
                //$tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : 0,
            };
            i++;
            gridData.push(data);
        }

    });

    /*console.log(JSON.stringify(gridData));*/
    var success = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        //url: APIURL + "/WIPTensou/IsFillGridViewMandatoryFields",
        url: "/WIPTensou/IsFillGridViewMandatoryFields",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                success = true;
            }
            else {
                showMessageDialog(response.message, '', WARNING);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    return success;
}

function IsDropDownMandatory() {
    debugger;
    var returnVal = true;
    var message = "";
    if ($("#ddlTensoDestination").val() == "") {
        var focusFieldID = "ddlTensoDestination";
        var message = $.trim($("#lblErrorMessageE_WIPTensou_006").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }

    return returnVal;
}

//checkDate
function checkDate(field) {

    var minYear = 2019;
    var maxYear = 2100;
    //re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/ 
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/      //CHECK ONLY DATE
    // re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/   //CHECK DATE TIME
    // re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/    
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


function checkDateOnListPage(field) {

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
function IsDateDifferentFromRange(boolVal) {
    debugger;
    var returnval = false;
    var getDate;
    if (boolVal === true) {
        // getDate = ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
        getDate = ($("#txtDate").val()).replaceAll('/', '-');
    }
    else {
        getDate = ($("#txtDate").val()).replaceAll('/', '-');
    }

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/IsValidDateRange",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DATERANGE: commonDateRange, RANGE_DATETIME: getDate })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnval = response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


    return returnval;
}

function setDecimalFormats() {
    $("#txtNoOfUnits").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtWeight").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 9, 2);
    });

}

$('#txtChuuban').keypress(function (e) {
    var orderNo = $('#txtChuuban').val() ? $('#txtChuuban').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuuban').val(jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN);

                    //$('#txtChuuban').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    //$('#txtGyoban').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btnSearchChuuban').focus();
                } else {
                    $('#txtChuuban').val("");
                    $('#txtGyoban').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_WIPTensou").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});

$('#txtNifuda').keypress(function (e) {
    var nifudaNo = $('#txtNifuda').val() ? $('#txtNifuda').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && nifudaNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/MappNifudaByBarcode?nifudaNo=" + nifudaNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;

                if (response.isSuccess) {
                    $('#txtNifuda').val(jsonResult.nifudA_NO);
                    $('#btnSearchNifuda').focus();

                } else {
                    $('#txtNifuda').val("");

                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }

});

function GetChuubanGyuobanByNifuda(nifudaNo, $callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetChuubanGyuobanByNifuda?nifudaNo=" + nifudaNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            debugger;
            // nifuda no not found in system.
            if (jsonResult === null) {
                //disableSaveButton();
                var errorMessage = $("#lblErrorMessageE_TagNotFoundRegisterFromHandOver").text();
                showMessageDialog(errorMessage, "txtNifudaNo1", WARNING);
                return false;
            }

            // nifuda is marked as completed in the system.
            else if (jsonResult.nifudA_COMPLETE_DATE != null) {
                // disableSaveButton();
                var errorMessage = $("#lblErrorMessageE_NifudaCompleted").text();
                showMessageDialog(errorMessage, "txtNifudaNo1", WARNING);
                return false;
            }
            else {
                var chuubanGyouban = jsonResult.chuuban + jsonResult.gyouban;
                GetTensouDetailsByChuubanGyouban(chuubanGyouban, $callFrom);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
        });
}

//Only Allow Numbers both side[Mobile,Desk]
$('#txtNoOfUnits,#txtWeight').on('input', function () {
    let v = $(this).val().replace(/,/g, '.');
    const parts = v.split('.');
    parts[0] = parts[0].replace(/\D+/g, '');
    if (parts.length > 1) {
        parts[1] = parts.slice(1).join('').replace(/\D+/g, '');
        v = parts[0] + '.' + parts[1];
    } else {
        v = parts[0];
    }
    $(this).val(v);
});

$('#txtNoOfUnits,#txtWeight').on('paste', function (e) {
    e.preventDefault();
    const text = (e.originalEvent.clipboardData || window.clipboardData).getData('text');
    $(this).val(text.replace(/,/g, '.').replace(/[^0-9.]/g, '').replace(/\.(?=.*\.)/g, ''));
});

function RestrictToDigitsOnly() {
    $("#txtbundleNo1,#txtbundleNo2,#txtbundleNo3,#txtbundleNo4,#txtChuubanSearch").on('input', function () {
        // Replace all non-digit characters
        this.value = this.value.replace(/[^0-9]/g, '');
    });
}
function setConditionName() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetConditionName",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindConditionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindConditionDropdown(jsonResult) {
    $("#ddlCondition").html("");
    $("#ddlCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

