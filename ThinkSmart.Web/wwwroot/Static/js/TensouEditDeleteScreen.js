var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";
var $btnExtractKouban;
userName = "";
var IsExists = false;

function onLoad() {
    window.resizeTo(1220, 100);
}
$(document).ready(function () {
    Mask();
    bindEvents();
    setUnitType();
    BindDropdown();
    getFutouList("", "");
    getMochikomisakiList("","")
    const queryString1 = window.location.search;
    let TransferCD = queryString1.split('=')[1];
    $("#hdfTranferCD").val(TransferCD);
    if (TransferCD != null) {
        getTransferDetailsByTrasferCD(TransferCD);
    }
});

function Mask() {
    $("#txtTransferDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtStartDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
}
function BindDropdown() {
    setMatlMakerList();
}

function disableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

function setMatlMakerList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/GeMatlMakerList",
        method: "GET",
        contentType: "application/json; charset=utf-8", 
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindMachineDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMachineDropdown(jsonResult) {
    $("#ddlMaterialMaker").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMaterialMaker").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function bindEvents() {
    $("#btnInitialize").click(function (event) {
        initialize();
    });
    $("#btnExtractKouban").click(function (event) {
        KoubanSearch();
    });
    $("#btnYobinaFutouSearch").click(function (event) {
        getFutouList($("#txtFutouInputSearch").val(), $("#ddlFutou"));
    });
    $("#btnYobinaMochikomisakiSearch").click(function (event) {
        getMochikomisakiList($("#txtMochikomisakiInputSearch").val(), $("#ddlMochikomisaki"));
    });
    $("#btnSave").click(function (event) {
        
    var $callFrom = $(this);
        event.preventDefault();
        if (!IsMandatoryFieldsFilled()) {
            
            return false;
        }
        if (!checkDate($("#txtTransferDate"))) {
            var message = $("#lblErrorMessageE_TransferDateInvalid").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }
        if ($("#hdfDBOperation").val() == "Insert")
        {
            saveTensouDetails();

        }
        else
        {
            updateTensouDetails(); 
        }

}); 
}

function initialize() {
    $("txtSrchKouban").val("");
    $("#ddlFutou").empty();
    $("#txtTransferDate").val("");
    $("#txtVoucherNumber").val("");
    $("#ddlMochikomisaki").empty();
    $("#txtArrivalStockDate").val("");
    $("#txtMaterialMaker").val("");
    $("#txtRawMatl").val("");
    $("#txtDimD").val("");
    $("#txtShape").val("");
    $("#txtlength").val("");
    $("#txtMatlKubun").val("");
    $("#txtInternalName").val("");
    $("#txtMukesaki").val("");
    $("#txtCondition").val("");
    $("#txtGeneralSpecial").val("");
    $("#txtkouban").val("");
    $("#txtMatlKubun").val("");
    $("#txtProvisionNoteNo").val("");
    $("#txtDimW").val("");
    $("#txtDimOther").val("");
    $("#txtNoOfUnits").val("");
    $("#ddlUnit").empty();
    $("#txtQuantity").val("");
    $("#txtRemark").val("");
    $("#txtCharge").val("");
    $("#ddlMaterialMaker").val("");
    $("#txtRawMatl").val("");
    $("#txtInternalName").val("");
    $("#txtSrchDimDStart").val("");
    $("#txtSrchDimDEnd").val("");
    $("#txtShape").val("");
    $("#txtLengthMin").val("");
    $("#txtLengthMax").val("");
    $("#txtKouban").val("");
    $("#txtCharge").val("");
    $("#txtStartDate").val("");
    $("#txtEndDate").val("");
    setUnitType();
    
}

function KoubanSearch() {
    var Kouban = $("#txtSrchKouban").val();
    if (($('#txtSrchKouban').val() != "")) {
        var ajaxTime = new Date().getTime();

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WarehouseToSupplier/SearchBykouban?kouban=" + Kouban,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                
                responseTime = new Date().getTime() - ajaxTime;
                jsonResult = response.message;
                if (jsonResult != null) {
                    setKoubanDetails(jsonResult)
                }
                else {

                }

            })
            //.fail(function (xhr, textStatus, errorThrown) {
            //    debugger;
            //    responseTime = new Date().getTime() - ajaxTime;
            //    var errorCode = $.parseJSON(xhr.responseText).Message;
            //    focusFieldID = $(':focus').attr("id");
            //    message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            //    showMessageDialog(message, focusFieldID, ERROR);
            //});
    }
}
    
function setKoubanDetails(jsonResult) {
    console.log(jsonResult);
    dbOperation = "Insert";
    $("#hdfDBOperation").val(dbOperation);
    $("#ddlFutou").text(jsonResult.futoU_CD);
    $("#ddlMochikomisaki").text(jsonResult.mochikomisakI_CD);
    $("#txtArrivalStockDate").val($.format.date(new Date(jsonResult.nyukA_DATE), "yyyy/MM/dd"));
    $("#txtMaterialMaker").val(jsonResult.matL_MAKER_NAME);
    $("#txtRawMatl").val(jsonResult.makeR_NAME);
    $("#txtInternalName").val(jsonResult.internaL_NAME);
    $("#txtkouban").val(jsonResult.kouban);
    $("#txtDimD").val(jsonResult.diM_D);
    $("#txtShape").val(jsonResult.shape);
    $("#txtlength").val(jsonResult.materiaL_LENGTH);
    $("#txtMatlKubun").val(jsonResult.materiaL_KUBUN);
    $("#txtMukesaki").val(jsonResult.mukesakI_NAME);
    $("#txtCondition").val(jsonResult.conditioN_DESCRIPTION);
    $("#txtGeneralSpecial").val(jsonResult.matL_SPECIAL_KUBUN);// $("#txtMatlDimT1").text(jsonResult.PP_MATL_DIM_D_STRING);
    $("#txtDimW").val(jsonResult.diM_W);
    $("#txtDimOther").val(jsonResult.diM_OTHER);
    $("#hdfKoubanCD").val(jsonResult.koubaN_CD);
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

function setUnitType() {
  var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/GetUnitType",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
             var errorCode = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
    $("#ddlUnit").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnit").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}


function getFutouList(yobina, $target) {
         var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
      var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/GetFutouList?name=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
           responseTime = new Date().getTime() - ajaxTime;
           bindFutouDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}

function bindFutouDropdown(yobina, $target, jsonResult) {
    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
     $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + jsonObj.yobina + '">' + jsonObj.name + '</option>'));
    });

}

function getMochikomisakiList(yobina, $target) {
    var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/GetMochikomisakiList?name=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           bindMochikomisakiDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}

function bindMochikomisakiDropdown(yobina, $target, jsonResult) {
    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + jsonObj.yobina + '">' + jsonObj.name + '</option>'));
    });

}

function saveTensouDetails() {
        var ajaxTime = new Date().getDate();
        var jsonDataForSave = JSON.stringify({
        KOUBAN_CD: $("#hdfKoubanCD").val() ? $("#hdfKoubanCD").val() : null,
        FUTOU_CD: $("#ddlFutou").val() ? $("#ddlFutou").val() : null,
        MOCHIKOMISAKI_CD: $("#ddlMochikomisaki").val() ? $("#ddlMochikomisaki").val() : null,
        TRANSFER_DATE: ($("#txtArrivalStockDate").val().split(" ")[0]).replaceAll('/', '-'), //date format replace('/', '-') because of  undefine value
        CHARGE: $("#txtCharge").val() ? $("#txtCharge").val() : null,                 
        UNIT: $("#ddlUnit option:selected").val() ? $("#ddlUnit option:selected").val() : null,
        NO_OF_UNITS: $("#txtNoOfUnits").val() ? $("#txtNoOfUnits").val() : null,
        QUANTITY: $("#txtQuantity").val() ? $("#txtQuantity").val() : null,
        CHUUBAN: $("#txtProvisionNoteNo").val() ? $("#txtProvisionNoteNo").val() : null,
        SLIP_NUMBER: $("#txtVoucherNumber").val() ? $("#txtVoucherNumber").val() : null,
        REMARKS: $("#txtRemark").val() ? $("#txtRemark").val() : null,
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        
    });
var settings = {
       async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/saveWarehouseToSupplierDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var successMessage = $("#lblSuccessMessageS_MaterialTensou_DataSave").text();
            showMessageDialog(successMessage, "", SUCCESS);
            $("#btnSave").attr(disable);
            EditMode(true);
            var jsonResult = response.message;

        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//Update Date
function updateTensouDetails() {
 
    
    var ajaxTime = new Date().getTime();
    
    var jsonDataSaveEditDelete = JSON.stringify({

        TRANSFER_CD: $("#hdfTranferCD").val() ? $("#hdfTranferCD").val() : null,
        KOUBAN_CD: $("#hdfKoubanCD").val() ? $("#hdfKoubanCD").val() : null,
        FUTOU_CD: $("#ddlFutou").val() ? $("#ddlFutou").val() : null,
        MOCHIKOMISAKI_CD: $("#ddlMochikomisaki").val() ? $("#ddlMochikomisaki").val() : null,
        TRANSFER_DATE: ($("#txtTransferDate").val().split(" ")[0]).replaceAll('/', '-'), 

        CHARGE: $("#txtCharge").val() ? $("#txtCharge").val() : null,
        UNIT: $("#ddlUnit option:selected").val() ? $("#ddlUnit option:selected").val() : null,
        NO_OF_UNITS: $("#txtNoOfUnits").val() ? $("#txtNoOfUnits").val() : null,
        QUANTITY: $("#txtQuantity").val() ? $("#txtQuantity").val() : null,
        CHUUBAN: $("#txtProvisionNoteNo").val() ? $("#txtProvisionNoteNo").val() : null,
        SLIP_NUMBER: $("#txtVoucherNumber").val() ? $("#txtVoucherNumber").val() : null,
        REMARKS: $("#txtRemark").val() ? $("#txtRemark").val() : null,
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        // USER_NAME: userName

    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/updateWarehouseToSupplierDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({STAMPING_CD: $("#hfld_StamppingCd").val(),MACHINE_CD: $("#ddlModel").val(),PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),MSG_CD: parseInt($("#hfld_MessageCD").val()),ST_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,ST_START_DATE: $("#txtStartDate").val() + " " + $("#txtStartTime").val(),ST_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),ST_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val(),ST_DECISION: parseInt($("#ddlDecision").val()),ST_NG_QTY: parseFloat($("#txtNGQty").val()),ST_OK_QTY: parseFloat($("#txtNgWeight").val()),ST_NG_BARS1: parseInt($("#txtNGWeight1").val()),ST_NG_DETAILS1: parseInt($("#ddlDefects1").val()),ST_NG_BARS2: parseInt($("#txtNGWeight2").val()),ST_NG_DETAILS2: parseInt($("#ddlDefects2").val()),ST_NG_BARS3: parseInt($("#ddlDefects3").val()),ST_NG_DETAILS3: parseInt($("#txtNGWeight3").val()),ST_TABA_NO: parseInt($("#txtBundleNo").val()),ST_OK_BARS: parseInt($("#txtFinishedBarCount").val()),UNIT_TYPE: parseInt($("#ddlUnitType").val()),ST_END_DATE: $("#txtEndDate").val() + " " + $("#txtEndTime").val(),ST_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),ST_NEXT_PROCESS: parseInt($("#ddlNextProcess").val()),ST_REMARKS: $("#txtRemarks").val(),USER_NAME: userName})
        data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            // disableEditDeleteButtons();
            var successMessage = $("#lblSuccessMessageS_MaterialTensou_DataUpdated").text();
            showMessageDialog(successMessage, "Updated", SUCCESS);
            var jsonResult = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".MandatoryTextBox "), function (index, mandatoryField) {
        if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }

    });
    $.each($.find(".MandatoryDropdown "), function (index, mandatoryField) {
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
        // $("#imgError").show();
        bootbox.alert({
            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
            message: message,
            centerVertical: true,
            callback: function () {
                if (focusFieldID !== "") {
                    $("#" + focusFieldID).focus();
                }
            }
        });
    }

    return returnVal;

}

function checkDate(field) {

    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
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

//Select Code

$("#btnSelect").click(function event() {

    location.href = "/WarehouseToSupplier/TensouListPage";
    window.resizeTo(1640, 900);
});

$(document).on('click', '#btnSearch', function (event) {
   
    var MATL_NAME_MAKER_CD = $("#ddlMaterialMaker").val();
    var MATL_MAKER_NAME = $("#txtRawMatl").val();
    var INTERNAL_NAME_STRING = $("#txtInternalName").val();
    var MAT_START_DIM_D = $("#txtSrchDimDStart").val();
    var MAT_END_DIM_D = $("#txtSrchDimDEnd").val();
    var MATL_SHAPE = $("#txtShape").val();
    var MATL_START_LENGTH = $("#txtLengthMin").val();
    var MATL_END_LENGTH = $("#txtLengthMax").val();
    var KOUBAN = $("#txtKouban").val();
    var CHARGE = $("#txtCharge").val();
    var START_DATETIME = $("#txtStartDate").val();
    var END_DATETIME = $("#txtEndDate").val();
    var FUTOU_CD = $("#ddlFutou").val() ? $("#ddlFutou").val() : 0;
    var MOCHIKOMISAKI_CD = $("#ddlMochikomisaki").val() ? $("#ddlMochikomisaki").val() : 0;
    var USER_ID = $("#hdfLoginId").val();

    if (MATL_NAME_MAKER_CD.length > 1 || MATL_MAKER_NAME.length > 0
        || INTERNAL_NAME_STRING.length > 0 || MAT_START_DIM_D.length > 0
        || MAT_END_DIM_D.length > 0 || MATL_SHAPE.length > 0 ||
        MATL_START_LENGTH.length > 0 || MATL_END_LENGTH.length > 0 ||
        KOUBAN.length > 0 || CHARGE.length > 0 || START_DATETIME.length > 0
        || END_DATETIME.length > 0 || FUTOU_CD.length > 1 || MOCHIKOMISAKI_CD.length > 1
    )
        {
        var ajaxTime = new Date().getTime();
        //var jsonData = JSON.stringify({ ddlMaterialMaker: ddlMaterialMaker, txtRawMatl: txtRawMatl, txtInternalName: txtInternalName, txtSrchDimDStart: txtSrchDimDStart, txtSrchDimDEnd: txtSrchDimDEnd, txtShape: txtShape, txtLengthMin: txtLengthMin, txtLengthMax: txtLengthMax, txtKouban: txtKouban, });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WarehouseToSupplier/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                MATL_NAME_MAKER_CD: MATL_NAME_MAKER_CD, MATL_MAKER_NAME: MATL_MAKER_NAME, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, MAT_START_DIM_D: MAT_START_DIM_D, MAT_END_DIM_D: MAT_END_DIM_D, MATL_SHAPE: MATL_SHAPE, MATL_START_LENGTH: MATL_START_LENGTH, MATL_END_LENGTH: MATL_END_LENGTH, KOUBAN: KOUBAN, 
                CHARGE: CHARGE, START_DATETIME: START_DATETIME, END_DATETIME: END_DATETIME, 
                USER_ID: USER_ID,
            })
        }
        $.ajax(settings)
            .done( (response) => {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/WarehouseToSupplier/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;
                            if (response1.isSuccess) {
                                $("#divTensouGridMain").html(response1.message);
                            }
                            
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_MaterialTensou_Error").text());
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
                message = $.trim($("#lblErrorMessageE_MaterialTensou_Error").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });

    }
    else {

        debugger;
        
        message = $.trim($("#lblErrorMessageE_MaterialTensou_014").text());
        showMessageDialog(message, 'MATL_NAME_MAKER_CD', WARNING);
        
    }

})


function getMaterialDataTransferDetailsByTrasferCD($callFrom) {
    var strUser = document.getElementById('ddlMaterialMaker').selectedOptions[0].value;
    console.log("ddlMaterialMaker :", strUser);
   
    EditCheck = "0";
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var TransferCD = $.trim($callFrom.closest("tr").attr("data-lblTensouTransferCD"));
    var jsonData = "{TRANSFER_CD :'" + TransferCD + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/getMaterialDataTransferDetailsByTrasferCD?TransferCD=" + parseInt(TransferCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = response.message;
            // setWIPTensouDetails(jsonResult);  
            location.href = "/WarehouseToSupplier/TensouEntryEditDelete?TransferCD=" + TransferCD;  //list page to pass id on edit delete screen(list page close & open edit delete screen)


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function getTransferDetailsByTrasferCD(TransferCD) {
    window.resizeTo(1290, 450);
    EditCheck = "1";
    var ajaxTime = new Date().getTime();
    var jsonData = "{TRANSFER_CD :'" + TransferCD + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WarehouseToSupplier/getMaterialDataTransferDetailsByTrasferCD?TransferCD=" + parseInt(TransferCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            setTensouDetails(jsonResult);
            $("#btnUpdate").click(function (event) {
               if (!IsMandatoryFieldsFilled()) {
                    return false;
                }
               if (!IsDropDownMandatory()) {
                    return false;
               }
               if (!checkDate($("#txtTransferDate"))) {

                    var message = $("#lblErrorMessageE_WIPTensou_005").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, $("#txtTransferDate"), WARNING);
                    $("#txtTransferDate").focus;
                    return false;
                }

                if (EditCheck == 0) {
                   
                    updateTensouDetails();
                    EditCheck = "1";
                }

                $("#txtTransferDate").focus();
            });


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MaterialTensou_Error").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setTensouDetails(jsonResult) {
    
    $('#ddlFutou option').each(function () {
        if (this.value == jsonResult.futoU_CD)
            IsExists = true;
    });
    $('#ddlMochikomisaki option').each(function () {
        if (this.value == jsonResult.mochikomisakI_CD)
            IsExists = true;
    });
    if (IsExists === false) {
        $("#ddlFutou").append(new Option(jsonResult.futoU_NAME, jsonResult.futoU_CD)); 
    }
    $("#ddlFutou").val(jsonResult.futoU_CD);
    if (IsExists === false) {
        $("#ddlMochikomisaki").append(new Option(jsonResult.mksakI_NAME, jsonResult.mochikomisakI_CD));  
    }
    $("#ddlFutou").val(jsonResult.futoU_CD);
   $("#txtArrivalStockDate").val($.format.date(new Date(jsonResult.nyukA_DATE), "yyyy/MM/dd"));
    $("#txtMaterialMaker").val(jsonResult.matL_MAKER_NAME);
    $("#txtRawMatl").val(jsonResult.makeR_NAME);
    $("#txtInternalName").val(jsonResult.internaL_NAME);
    $("#txtkouban").val(jsonResult.kouban);
    $("#txtDimD").val(jsonResult.diM_D);
    $("#txtShape").val(jsonResult.shape);
    $("#txtlength").val(jsonResult.materiaL_LENGTH);
    $("#txtMatlKubun").val(jsonResult.materiaL_KUBUN);
    $("#txtMukesaki").val(jsonResult.mukesakI_NAME);
    $("#txtCondition").val(jsonResult.conditioN_DESCRIPTION);
    $("#txtGeneralSpecial").val(jsonResult.matL_SPECIAL_KUBUN);// $("#txtMatlDimT1").text(jsonResult.PP_MATL_DIM_D_STRING);
    $("#txtDimW").val(jsonResult.diM_W);
    $("#txtDimOther").val(jsonResult.diM_OTHER);
    $("#hdfKoubanCD").val(jsonResult.koubaN_CD);
    $("#txtTransferDate").val($.format.date(new Date(jsonResult.transfeR_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtCharge").val(jsonResult.charge);
   // $("#ddlUnit option:selected").
    $("#txtNoOfUnits").val(jsonResult.nO_OF_UNITS) 
    $("#txtQuantity").val(jsonResult.quantity) 
    $("#txtProvisionNoteNo").val(jsonResult.chuuban) 
    $("#txtVoucherNumber").val(jsonResult.sliP_NUMBER) 
    $("#txtRemark").val(jsonResult.remarks) 
   
   
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
    TensouGridSearch(selectedColumns);
}
function TensouGridSearch(sortFields) {
    var ddlMaterialMaker = $("#ddlMaterialMaker").val();
    var txtInternalName = $("#txtInternalName").val();
    var txtSrchDimDStart = $("#txtSrchDimDStart").val();
    var txtSrchDimDEnd = $("#txtSrchDimDEnd").val();
    var txtDimDEndSearch = $("#txtDimDEndSearch").val();
    var txtShape = $("#txtShape").val();
    var txtLengthMin = $("#txtLengthMin").val();
    var txtLengthMax = $("#txtLengthMax").val();
    var txtKouban = $("#txtKouban").val();
    var txtCharge = $("#txtCharge").val();
    var txtStartDate = $("#txtStartDate").val();
    var txtEndDate = $("#txtEndDate").val();
    var ddlFutou = $("#ddlFutou").val();
    var ddlMochikomisaki = $("#ddlMochikomisaki").val();
    var condtion = $("#txtCondition")
    var userId = $("#hdfLoginId").val();

    if (ddlMaterialMaker.length > 0 || txtRawMatl.length > 0 || txtInternalName.length > 0 || txtSrchDimDStart.length > 0 || txtSrchDimDEnd.length > 0 || txtShape.length > 0 || txtLengthMax.length > 0 || txtLengthMin.length > 0 || txtKouban.length > 0 || txtCharge.length > 0 || txtStartDate.length > 0 || txtEndDate.length > 0 || ddlFutou.length > 0 || ddlMochikomisaki.length > 0) {

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
$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
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
        url: APIURL + "/WarehouseToSupplier/InitializeSortPopup",
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