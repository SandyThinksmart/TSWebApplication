var $btnChuubanSearch;
var $btnSave;
var $btnActual;
var combinedDecisionOK = "";
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var yobina = "";
var indexDDLDecisionNothing = 0;
var maxRegisteredThabaNo = 0;

var operatorStartModified = false;
let enterPressedStartOpr = false;


var tsCD;
var registeredThabaNumbersAndDecision = {};
var fICombinedDecisions = {};
let CombinedDecisionArr = [];
let SparkDecisionArr = [];
var FIResultOk = $("#hdfFIResultOk").val();
var FIResultNg = $("#hdfFIResultNg").val();
var FIResultUntested = $("#hdfFIResultUntested").val();
var FIResultSparkUntested = $("#hdfFIResultSparkUntested").val();
var FISparkUntested = $("#hdfFISparkUntested").val();
var FISparkOk = $("#hdfFISparkOk").val();
var FISparkNg = $("#hdfFISparkNg").val();
// Error log information. To be sent with each AJAX request. 
userName = ""; // assigned at Document.ready
// End error log information.


// Following decision legends are to show decision colors on Top Thaba Grid.
//var decisionLegends = {
//    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
//    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
//    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
//    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>"
//}

function setGlobalVariables() {
    $btnChuubanSearch = $("#btnChuubanSearch");
    $btnSave = $("#btnSave");
    $btnActual = $("#btnActual");
    $hdfSYSDATE = $("#hdfSYSDATE");
    $hdfCultureLoaded = $("#hdfCultureLoaded");
    $hdfCultureEN = $("#hdfCultureEN");
    $hdfCultureJP = $("#hdfCultureJP");

}
function resetGlobalVariables() {
    $("#hdfDBOperation").val("");
    maxRegisteredThabaNo = 0;
    /* dimTolMin = "";
     dimTolMax = "";
     dimStd = "";
     lenTolMin = "";
     lenTolMax = "";
     lenStd = "";
     registeredThabaNumbersAndDecision = {};
     maxRegisteredThabaNo = 0;*/
}


function disableDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#pnlBottomData");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("select"), function (index, dropdown) {
        //$("#" + dropdown.id + " option:selected").prop("selected", false);
        //$("#" + dropdown.id + " option:first").prop("selected", "selected");
        $(dropdown).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });
    var $endDataPanel = $("#pnlBottomData");
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });

}

function setShapeList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetDimShape",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindShapeDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindShapeDropdown(jsonResult) {

    $("#ddlDimShape").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDimShape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setDecisionList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Tensile/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindDecisionDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}




function bindDecisionDropdown(jsonResult) {

    //  $("#ddlDecision").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}



function setJISList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{ USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetJISTestList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);
            bindJISDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindJISDropdown(jsonResult) {
    $("#ddlJISTest").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlJISTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function clearTable($table, trId, trClass) {
    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
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
function IsMandatoryChuubanSearchFieldsFilled() {

    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {
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
        showMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
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
        //  $("#imgError").show();

        showMessageDialog(message, focusFieldID, WARNING)
        //$("#imgWarning").show();

        //$("#dialogMessageText").html("").append($("<p>" + message + "<p>"));
        //$("#dialog").dialog({
        //    modal: true,
        //    title: document.title,
        //    buttons: {
        //        Ok: function () {
        //            $(this).dialog("close");
        //            if (focusFieldID !== "") {
        //                $("#" + focusFieldID).focus();
        //            }
        //        }
        //    }
        //});
    }

    return returnVal;
}
function setChuubanDetails(jsonResult) {
    debugger;
    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#txtSplitNo").focus();
    var newSplitNo = jsonResult.tS_LAST_DETAIL_NO + 1;  
    $("#txtSplitNo").val(newSplitNo);

    dbOperation = "Insert";

    $("#hdfDBOperation").val(dbOperation);

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);

    if (jsonResult.pP_PROD_SPEC_NO != null) {
        $("#txtProductSpecNoT2").val(jsonResult.pP_PROD_SPEC_NO);
        $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);
    }

    if (jsonResult.pP_CHUUBAN != null) {
        $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN);
        $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN);
    }

    $("#txtHostRefNoT1").val(jsonResult.pP_NYUKA_CD);
    $("#txtHostRefNoT4").val(jsonResult.pP_NYUKA_CD);

    if (jsonResult.pP_DELIVERY_DATE_STRING != null) {
        $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
        $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);
    }

    if (jsonResult.pP_EMPLOYEE_NAME != null) {
        $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
        $("#txtCustomerInchargeT4").val(jsonResult.pP_EMPLOYEE_NAME);
    }

    if (jsonResult.pP_STD_DEL_PERIOD != null) {
        $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
        $("#txtStdDeliveryDateT4").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    }

    if (jsonResult.pP_CUSTOMER_NAME != null) {
        $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
        $("#txtCustomerT4").val(jsonResult.pP_CUSTOMER_NAME);
    }

    if (jsonResult.pP_RECEIVER_NAME != null) {
        $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);
        $("#txtReceiverT4").val(jsonResult.pP_RECEIVER_NAME);
    }

    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
        $("#txtChamferT4").val(jsonResult.pP_CHAMFER);
    }

    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) != null) {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
        $("#txtUnitWtT4").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
    }

    if (jsonResult.pP_COLOR != null) {
        $("#txtColorT1").val(jsonResult.pP_COLOR);
        $("#txtColorT4").val(jsonResult.pP_COLOR);
    }

    if (jsonResult.pP_MATL_NAME_MAK != null) {
        $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
        $("#txtMaterialNameMakerT4").val(jsonResult.pP_MATL_NAME_MAK);
    }

    if (jsonResult.pP_MATL_SHAPE != null) {
        $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
        $("#txtMatlShapeT4").val(jsonResult.pP_MATL_SHAPE);
    }

    if (jsonResult.pP_MATL_DIM_D != null) {
        $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);
        $("#txtMatlDimT4").val(jsonResult.pP_MATL_DIM_D);
    }

    if (jsonResult.pP_CHARGE != null) {
        $("#txtChargeT1").val(jsonResult.pP_CHARGE);
        $("#txtChargeT4").val(jsonResult.pP_CHARGE);
    }

    if (jsonResult.pP_MAGARI_BEND_TOL != null) {
        $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
        $("#txtBendStandardT4").val(jsonResult.pP_MAGARI_BEND_TOL);
    }

    if (jsonResult.pP_PROD_NAME_CUST != null) {
        $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
        $("#txtProdNameCustomerT4").val(jsonResult.pP_PROD_NAME_CUST);
    }

    if (jsonResult.pP_PROD_SHAPE != null) {
        $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
        $("#txtProdShapeT4").val(jsonResult.pP_PROD_SHAPE);
    }

    if (jsonResult.pP_PROD_DIM_D != null) {
        $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);
        $("#txtProdDimT4").val(jsonResult.pP_PROD_DIM_D);
    }

    if (jsonResult.pP_PROD_DIM_OTHER != null) {
        $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
        $("#txtProdDimOtherT4").val(jsonResult.pP_PROD_DIM_OTHER);
    }

    if (jsonResult.pP_EXT_TEXTURE != null) {
        $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
        $("#txtExtTextureT4").val(jsonResult.pP_EXT_TEXTURE);
    }
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
        $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
    }
    else {
        $("#txtLengthToleranceT1").val('');
        $("#txtLengthToleranceT4").val('');
    }


    if (jsonResult.pP_DIM_TOLERANCE != null) {
        $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
        $("#txtDimToleranceT4").val(jsonResult.pP_DIM_TOLERANCE);
    }

    if (jsonResult.pP_DIM_TOL_MIN != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN;
    if (jsonResult.pP_DIM_TOL_MAX != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX;
    dimStd = jsonResult.pP_PROD_DIM_D;

    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtConditionT4").val(jsonResult.pP_PRODUCT_CONDITION);

    if (jsonResult.pP_DISPLAY != null) {
        $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
        $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);
    }

    if (jsonResult.pP_STAMP != null) {
        $("#txtStampT1").val(jsonResult.pP_STAMP);
        $("#txtStampT4").val(jsonResult.pP_STAMP);
    }

    if (jsonResult.pP_PROD_LENGTH_STRING != null) {
        $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
        $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_STRING);
    }


    if (jsonResult.pP_LENGTH_TOL_MIN != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN; // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_LENGTH_TOL_MAX != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX; // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    lenStd = jsonResult.pP_PROD_LENGTH;

    if (jsonResult.pP_PACKING != null) {
        $("#txtPackingT1").val(jsonResult.pP_PACKING);
        $("#txtPackingT4").val(jsonResult.pP_PACKING);
    }

    if (jsonResult.pP_NO_OF_UNITS_DECODE != null) {
        $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);
        $("#txtNoOfUnitsT4").val(jsonResult.pP_NO_OF_UNITS_DECODE);
    }

    if (jsonResult.pP_ACT_ALLOC_QTY != null) {
        $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
        $("#txtQuantityT4").val(jsonResult.pP_ACT_ALLOC_QTY);
    }

    if (jsonResult.pP_COIL_DIA_INTERNAL != null) {
        $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
        $("#txtIntDiaMinT4").val(jsonResult.pP_COIL_DIA_INTERNAL);
    }
    if (jsonResult.pP_COIL_DIA_EXTERNAL != null) {
        $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
        $("#txtExtDiaMaxT4").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    }

    if (jsonResult.pP_COIL_DIA_WINDING != null) {
        $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
        $("#txtWindingDirectionT4").val(jsonResult.pP_COIL_DIA_WINDING);
    }

    if (jsonResult.pP_KAKOUIRAI_REMARKS != null) {
        $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
        $("#txtKakouiraiRemarksT4").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    }

    if (jsonResult.pP_PROCESS_PLAN_REMARK != null) {
        $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
        $("#txtProdOrderRemarkT4").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    }

    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);


    if (jsonResult.tS_LAST_DETAIL_NO != null) {
        $("#txtLastSplitNo").val(jsonResult.tS_LAST_DETAIL_NO);
    }

    // 3rd Tab
    resetMessageDetailsGrid();
    if (jsonResult.messagE_LIST != null) {
        setMessageDetailsGrid(jsonResult.messagE_LIST);
    }

    //4th Tab 
    if (jsonResult.pP_JIS_TEST_SPEC != null) {
        $("#txtJISTestSpecimen").val(jsonResult.pP_JIS_TEST_SPEC);
    }
    if (jsonResult.pP_TENSILE_STRENGTH != null) {
        $("#txtTensileStrength").val(jsonResult.pP_TENSILE_STRENGTH);
    }
    if (jsonResult.pP_YEILD != null) {
        $("#txtYieldPoint").val(jsonResult.pP_YEILD);
    }
    if (jsonResult.pP_ELONGATION != null) {
        $("#txtElongation").val(jsonResult.pP_ELONGATION);
    }
    if (jsonResult.pP_PRESS != null) {
        $("#txtReduction").val(jsonResult.pP_PRESS);// 
    }
    if (jsonResult.pP_HARDNESS != null) {
        $("#txtHardness").val(jsonResult.pP_HARDNESS);
    }
    if (jsonResult.pP_DECARB != null) {
        $("#txtDecarb").val(jsonResult.pP_DECARB);
    }


    //1st Tab : JIS Details
    if (jsonResult.pP_JIS_TEST_SPEC != null) {
        $("#txtJISTestSpecimen1").val(jsonResult.pP_JIS_TEST_SPEC);
    }
    if (jsonResult.pP_TENSILE_STRENGTH != null) {
        $("#txtTensileStrength1").val(jsonResult.pP_TENSILE_STRENGTH);
    }

    if (jsonResult.pP_YEILD != null) {
        $("#txtYieldPoint1").val(jsonResult.pP_YEILD);
    }
    if (jsonResult.pP_ELONGATION != null) {
        $("#txtElongation1").val(jsonResult.pP_ELONGATION);
    }
    if (jsonResult.pP_PRESS != null) {
        $("#txtReduction1").val(jsonResult.pP_PRESS);
    }
    if (jsonResult.pP_HARDNESS != null) {
        $("#txtHardness1").val(jsonResult.pP_HARDNESS);
    }
    if (jsonResult.elongatioN_PATTERN1_MIN_MAX != null) {
        $("#hdfElongationKubunType").val(jsonResult.elongatioN_PATTERN1_MIN_MAX);
    }
    if (jsonResult.tensilE_STRN_PATTERN1_MIN_MAX != null) {
        $("#hdfTensileKubunType").val(jsonResult.tensilE_STRN_PATTERN1_MIN_MAX);
    }
    if (jsonResult.hardnesS_PATTERN1_MIN_MAX != null) {
        $("#hdfHardnessKubunType").val(jsonResult.hardnesS_PATTERN1_MIN_MAX);
    }
    if (jsonResult.yeilD_PATTERN1_MIN_MAX != null) {
        $("#hdfYeildKubunType").val(jsonResult.yeilD_PATTERN1_MIN_MAX);
    }
    if (jsonResult.presS_PATTERN1_MIN_MAX != null) {
        $("#hdfPressKubunType").val(jsonResult.presS_PATTERN1_MIN_MAX);
    }

    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4 

    // Set ProdPoint details
    if (jsonResult.pP_PROCESS_1 !== null && jsonResult.pP_PROCESS_1 !== "") {
        addProdPointDataRow("1", jsonResult.pP_PROCESS_1, jsonResult.pP_PROD_POINT_1)
    }
    if (jsonResult.pP_PROCESS_2 !== null && jsonResult.pP_PROCESS_2 !== "") {
        addProdPointDataRow("2", jsonResult.pP_PROCESS_2, jsonResult.pP_PROD_POINT_2)
    }
    if (jsonResult.pP_PROCESS_3 !== null && jsonResult.pP_PROCESS_3 !== "") {
        addProdPointDataRow("3", jsonResult.pP_PROCESS_3, jsonResult.pP_PROD_POINT_3)
    }
    if (jsonResult.pP_PROCESS_4 !== null && jsonResult.pP_PROCESS_4 !== "") {
        addProdPointDataRow("4", jsonResult.pP_PROCESS_4, jsonResult.pP_PROD_POINT_4)
    }
    if (jsonResult.pP_PROCESS_5 !== null && jsonResult.pP_PROCESS_5 !== "") {
        addProdPointDataRow("5", jsonResult.pP_PROCESS_5, jsonResult.pP_PROD_POINT_5)
    }
    if (jsonResult.pP_PROCESS_6 !== null && jsonResult.pP_PROCESS_6 !== "") {
        addProdPointDataRow("6", jsonResult.pP_PROCESS_6, jsonResult.pP_PROD_POINT_6)
    }
    if (jsonResult.pP_PROCESS_7 !== null && jsonResult.pP_PROCESS_7 !== "") {
        addProdPointDataRow("7", jsonResult.pP_PROCESS_7, jsonResult.pP_PROD_POINT_7)
    }
    if (jsonResult.pP_PROCESS_8 !== null && jsonResult.pP_PROCESS_8 !== "") {
        addProdPointDataRow("8", jsonResult.pP_PROCESS_8, jsonResult.pP_PROD_POINT_8)
    }
    if (jsonResult.pP_PROCESS_9 !== null && jsonResult.pP_PROCESS_9 !== "") {
        addProdPointDataRow("9", jsonResult.pP_PROCESS_5, jsonResult.pP_PROD_POINT_9)
    }
    if (jsonResult.pP_PROCESS_10 !== null && jsonResult.pP_PROCESS_10 !== "") {
        addProdPointDataRow("10", jsonResult.pP_PROCESS_6, jsonResult.pP_PROD_POINT_10)
    }
    if (jsonResult.pP_PROCESS_11 !== null && jsonResult.pP_PROCESS_11 !== "") {
        addProdPointDataRow("11", jsonResult.pP_PROCESS_11, jsonResult.pP_PROD_POINT_11)
    }
    if (jsonResult.pP_PROCESS_12 !== null && jsonResult.pP_PROCESS_12 !== "") {
        addProdPointDataRow("12", jsonResult.pP_PROCESS_12, jsonResult.pP_PROD_POINT_12)
    }
    if (jsonResult.pP_PROCESS_13 !== null && jsonResult.pP_PROCESS_13 !== "") {
        addProdPointDataRow("13", jsonResult.pP_PROCESS_13, jsonResult.pP_PROD_POINT_13)
    }

}

function resetMessageDetailsGrid() {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $('<td class="GridAlignmentDateTime">').text(""),
        $('<td class="GridAlignmentMachineName">').text(""),
        $('<td class="GridAlignmentProcessName">').text(""),
        $('<td class="GridAlignmentOperator">').text(""),
        $('<td class="GridAlignmentRemarks GridRemarks200PX">').text("")
    );
    $grdvMessage.append($tr);
}

function setMessageDetailsGrid(messageDetailsList) {

    $grdvMessage = $("#grdvMessage");

    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        if ($("#hdfCultureName").val() == "en") // > -1 means substring is present
        {
            processName = jsonObject.procesS_NAME_EN;
        }
        else {
            processName = jsonObject.procesS_NAME_JP;
        }

        // Add data rows
        var $tr = $('<tr class="GridNormalRow">').append(
            $('<td class="GridAlignmentDateTime">').text(jsonObject.msG_DATE_STRING),
            $('<td class="GridAlignmentMachineName">').text(jsonObject.machinE_NAME),
            $('<td class="GridAlignmentProcessName">').text(processName),
            $('<td class="GridAlignmentOperator">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks GridRemarks200PX">').text(jsonObject.msG_CONTENTS)
        );

        $grdvMessage.append($tr);
    });
}

function addProdPointDataRow(srNo, processName, prodPoint) {
    var $grdvProdPointData = $("#grdvProdPointData");

    var $_prodPint;
    if (prodPoint !== null) {
        $_prodPint = prodPoint;
    } else {
        $_prodPint = "";
    }

    var $tr = $("<tr class='GridNormalRow'> " +
        "<td class='ProcessDetailsNumber' style='font-weight: normal;' >" + srNo + "</td>" +
        "<td class='ProcessName' style='font-weight: normal;text-align:left;'>" + processName + "</td>" +
        "<td class='ProdPointDetails' style='font-weight: normal; text-align:left;'>" + $_prodPint + "</td>" +
        "</tr>");
    $grdvProdPointData.append($tr);
}

function enableDataPanel(jsonResult) {

    var $startDataPanel = $("#pnlBottomData");

    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(disabled).removeAttr(disabled);
        }
    });

    $.each($startDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(disabled).removeAttr(disabled);
        }
    });

    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(disabled).removeAttr(disabled);
        }
    });

    $.each($startDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(disabled).removeAttr(disabled);
        }
    });

    var $endDataPanel = $("#pnlBottomData");
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).removeClass(disabled).removeAttr(disabled);
    });
    $("#txtActualDim99").attr(disabled, disabled).addClass(disabled);
    $("#txtLastSplitNo").attr(disabled, disabled).addClass(disabled);
}


function setStartDataPanelDefaultData(jsonResult) {
    $("#btnDateTime").click();
    /*  if (jsonResult.DW_START_DATE_STRING != null) {
          var startDateTime = jsonResult.DW_START_DATE_STRING;
          $("#txtStartDate").val(new Date(startDateTime).format("yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
          $("#txtStartTime").val(new Date(startDateTime).format("HH:mm")).mask("99:99", { placeholder: "hh:mm" });
      }
      else {
          $("#btnStartDateTime").click();
      }*/
}

function getTensileDetailsByChuuban(chuuban, gyouban, $callFrom) {

    // Do not proceed if request is already running
    /*if ($callFrom.data('requestRunning')) {
        return;
    }*/
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetTensileDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
            //  console.log(jsonResult);
            //if (jsonResult === null) {
            //    // Chuuban not found in system.
            //    ClearData();
            //    var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_002").text();
            //    showMessageDialog(errorMessage, "txtChuuban1", WARNING);
            //    return false;

            //}

            //if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
            //    ClearData();
            //    //    // Chuuban is marked as completed in the system/
            //    var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}


            //if (jsonResult.procesS_PLAN_START_DATE == null) {
            //    // OrNo Not Started FromHandOver

            //    var message = $("#lblErrorMessageE_Tensile_ENTRY_009").text().trim().replace("99-999999-99", jsonResult.pP_CHUUBAN);
            //    showMessageDialog(message, "", WARNING);
            //    ClearData();
            //    return false;
            //}



            //else {

            enableDataPanel();
            $("#ddlDimShape").attr(disabled, disabled).addClass(disabled);
            //SetOperatorDropdown($("#ddlOperatorName"), 0);
            $("#btnSave").removeAttr(disabled, disabled).removeClass(disabled);
            $("#btnActual").removeAttr(disabled, disabled).removeClass(disabled);
            DisableSearchPanel();
            setChuubanDetails(jsonResult);
            //getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
            // setStartData(jsonResult);
            // setProcessDefectList();

            //}

            //if (jsonResult.PP_PROCESS_PLAN_CMPL_DATE != null) {
            //    // Chuuban is marked as completed in the system/
            //    var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChuuban1", WARNING);
            //    return false;
            //}


            //if ($("#hdfDBOperation").val() == "Insert") {
            //    enableStartDataPanel();
            //    setStartDataPanelDefaultData(jsonResult);
            //    setStartSysDateTime();
            //    setOperatorList("");
            //    /*   clearPanel($("#pnlProcessEndData"));
            //       // setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed before disableEndDataPanel(). It is because setChuubanDimLenToleranceMessages() will enable some fields based on values of Dim and Len Tolerances, and disableEndDataPanel() will disable fields again. 
            //       disableEndDataPanel();
            //       $("#txtStartOperator").focus();*/
            //}
            //else if ($("#hdfDBOperation").val() == "Update") {
            //    setStartData(jsonResult);
            //    enableStartDataPanel();
            //    enableEndDataPanel();
            //    // Set End Operator same as Start Operator
            //    $("#txtEndOperator").val($("#txtStartOperator").val());
            //    $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());

            //    $("#btnEndDateTime").click(); // Get current time for end operation.
            //    $("#txtEndOperator").focus();

            //    // setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed after enableEndDataPanel(). It is because enableEndDataPanel() enables all fields, and setChuubanDimLenToleranceMessages() will again disable some fields based on values of Dim and Len Tolerances.
            //}

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            //showErrorDiv(errorCode);
        });


}
function setNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetNextProcess",
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
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {

    /*$("#ddlnextprocess").html("").append($('<option value="0"></option>'));*/
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlnextprocess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlnextprocess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
        });
    }
}
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
        url: APIURL + "/Tensile/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;


            $("#txtInspDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });




        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Tensile_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function checkDate(field) {
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    //re = /^(\d{4})\/(\d{1,2})\/(\d{1,2})$/;
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
            // console.log(regs);
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


function checkTime(field) {
    var validTime = true;
    // regular expression to match required time format
    re = /^(\d{1,2}):(\d{2})(:00)?([ap]m)?$/;

    if (field.val() != "") {
        if (regs = field.val().match(re)) {
            if (regs[4]) {
                // 12-hour time format with am/pm
                if (regs[1] < 1 || regs[1] > 12) {
                    validTime = false;
                }
            } else {
                // 24-hour time format
                if (regs[1] > 23) {
                    validTime = false;
                }
            }
            if (regs[2] > 59) {
                validTime = false;
            }
        } else {
            validTime = false;
        }
    }

    if (!validTime) {

        return false;
    }

    return true;
}



function IsDuplicateThabaNo() {

    var inputThaba1 = $("#txtSplitNo").val().trim();


    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK) {

            firstFieldID = "txtSplitNo";
            message = $("#lblErrorMessageE_Tensile_ENTRY_006").text().trim().replace("NNN", inputThaba1);
            showMessageDialog(message, firstFieldID, WARNING);
            return true;
        }
    }

    return false;
}


function getThabaDetailsByChuuban(chuuban, gyouban, $callFrom) {
    //if($callFrom.data('requestRunning')){
    //    return;
    //}

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        // data: "{jsonData : " + jsonThabaDetailData + "}"
        data: JSON.stringify({
            PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban
        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonThabaDetailData = response.message;
            setThabaDetails(jsonThabaDetailData);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Tensile_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function setThabaDetails(jsonResult) {
    
    // First, clear thaba details
    //$("[id$='lblDecisionNo']").html("");

    $("#lblDecisionNo").html("");

    // Then, set thaba details
    $.each(jsonResult, function (key, jsonObj) {

        if (jsonObj.tabA_NO_1) {
            var thabaNumbers = jsonObj.tabA_NO_1.split(",");
            var decision = jsonObj.decision;
            var decision_value = jsonObj.decisioN_VALUE;

            $.each(thabaNumbers, function (index, thabaNo) {
                thabaNo = thabaNo.trim();
                registeredThabaNumbersAndDecision["" + thabaNo] = decision;

                var decisionLegend = $(decisionLegends[decision_value]);
                // var decisionLegend = $(decisionLegends[decision]);
                $("#lblThabaNo" + thabaNo).text(thabaNo);
                $("#lblDecisionNo" + thabaNo).html("").append(decisionLegend);
            });
        }
    });


}
function setDecimalFormats() {
    $("#txtMaxLoadN").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 1);
    });
    $("#txtGaugeLength").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtTestSpecimentDim, #txtCuttingElongation").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
    $("#txtActDim").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtYieldLoadN").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 1);
    });

    $("#txtCutDim").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });
}

function SaveDetails($callFrom) {

    var ajaxTime = new Date().getDate();
    var jsonDataForSave = JSON.stringify({
        // START DATA

        PP_CHUUBAN: $("#txtChuuban1").val(),
        PP_GYOUBAN: $("#txtGyouban1").val(),
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        TS_TEST_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null, 
        TS_TEST_DATE: ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        TS_FURNACE_NO: ($("#txtFurnaceNum").val() || "").toUpperCase() || null,
        DETAIL_NO: $("#txtSplitNo").val(),
        TS_MAX_LOAD_N: $("#txtMaxLoadN").val(),

        TEST_SPECIMEN_CD: $("#ddlJISTest option:selected").val() ? $("#ddlJISTest option:selected").val() : null,
        IS_ACT_DIM_RANGE_CHECK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        TS_ACT_DIM: $("#txtActDim").val() ? $("#txtActDim").val() : null,

        TS_ACT_DIM_SHAPE: $("#ddlDimShape").val(),
        TS_TEST_SPECIMEN_DIM: $("#txtTestSpecimentDim").val() ? $("#txtTestSpecimentDim").val() : null,
        TS_GAUGE_LENGTH: $("#txtGaugeLength").val() ? $("#txtGaugeLength").val() : null,
        TS_YIELD_LOAD_N: $("#txtYieldLoadN").val() ? $("#txtYieldLoadN").val() : null,
        TS_TEST_IMPACT_1: $("#txtBreakTest1").val() ? $("#txtBreakTest1").val() : null,
        TS_TEST_IMPACT_2: $("#txtBreakTest2").val() ? $("#txtBreakTest2").val() : null,
        TS_TEST_IMPACT_3: $("#txtBreakTest3").val() ? $("#txtBreakTest3").val() : null,
        TS_CUT_DIM: $("#txtCutDim").val() ? $("#txtCutDim").val() : null,
        TS_EXTENDED_LENGTH: $("#txtCuttingElongation").val() ? $("#txtCuttingElongation").val() : null,

        TS_DECISION: $("#ddlDecision").val(),
        //TS_NEXT_PROCESS: $("#ddlnextprocess").val(),
        REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,

        TS_TENSILE: $("#txtEndTensileStrength").val() ? $("#txtEndTensileStrength").val() : 0,
        TS_YIELD_STRENGTH: $("#txtEndYieldYS").val() ? $("#txtEndYieldYS").val() : 0,
        TS_ELONGATION: $("#txtEndElongation").val() ? $("#txtEndElongation").val() : 0,
        TS_REDUCTION: $("#txtEndReduction").val() ? $("#txtEndReduction").val() : 0,




        USER_ID: $("#hdfLoginId").val()

        //USER_NAME:  userName
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/TensileDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonDataForSave + "}"
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            disableSaveButton();
            disableDataPanel();
            getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
            var successMessage = $("#lblSuccessMessageS_Tensile_ENTRY_001").text();
            showMessageDialog(successMessage, "Saved", SUCCESS);
            /*var jsonResult = $.parseJSON(response.d);*/

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Tensile_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//btn initialize click methods
function ClearData() {
    $("#btnSave").attr("disabled", "disabled");
    //  enableSaveButton();
    enableSearchPanel();
    $(".FocusDefault").focus();
    disableDataPanel();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlBottomData"));
    clearPanel($("#pnlProcessInfo")); // Tab 4
    clearPanel($("#pnlTestSpecifications")); // Tab 4    
    clearThabaDetailsGrid();
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    $("#txtProductSpecNoT2").val("");

    hideToleranceErrorLabels();

    resetGlobalVariables();
    resetMessageDetailsGrid();
}
function initialize() {
    /*  if (!$("#txtChuuban1").hasClass("Mandatory")) {
          $("#txtChuuban1").addClass("Mandatory");
      }
      if (!$("#txtGyouban1").hasClass("Mandatory")) {
          $("#txtGyouban1").addClass("Mandatory");
      }*/
    enableSaveButton();
    enableSearchPanel();
    $(".FocusDefault").focus();
    disableDataPanel();
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlBottomData"));
    clearPanel($("#pnlProcessInfo")); // Tab 4
    clearPanel($("#pnlTestSpecifications")); // Tab 4    
    clearThabaDetailsGrid();
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4

    hideToleranceErrorLabels();

    resetGlobalVariables();
    resetMessageDetailsGrid();
    resetProcessPlanDetailsGrid();
    combinedDecisionOK = "";
    typeDropDown = "select-one";
    typeText = "text";
    disabled = "disabled";
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
    DISABLED = "disabled";
    yobina = "";
    indexDDLDecisionNothing = 0;
    maxRegisteredThabaNo = 0;
    tsCD;
    //registeredThabaNumbersAndDecision = {};
    userName = "";
    $.support.cors = true;
    setGlobalVariables();
    disableDataPanel();

    setDecimalFormats();
    $("#txtChuuban1").focus();
    $("#btnActual").attr(disabled, disabled).addClass(disabled);
    $("#btnSave").attr("disabled", "disabled");
    setShapeList();
    //tDecisionList();
    setNextProcess();
    setJISList();
    //setOperatorList("");
    $("#chkActualDim").prop('checked', true);
    document.getElementById('FIDoneWarning').style.display = 'none';
}

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

//Add Empty grid after Initilize on Process plan Grid.
function resetProcessPlanDetailsGrid() {
    $grdvMessage = $("#grdvProdPointData");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $('<td>').text(""),
        $('<td>').text(""),
        $('<td>').text(""),

    );
    $grdvMessage.append($tr);
}

function clearThabaDetailsGrid() {
    $pnlThabaDetails = $("#DivThabaDetails");
    $.each($pnlThabaDetails.find("label[id^='lblThabaNo']"), function (index, span) {
        $(span).html("");
    });
    $.each($pnlThabaDetails.find("label[id^='lblDecisionNo']"), function (index, span) {
        $(span).html("");
    });
}

function clearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}


function enableSearchPanel() {
    // $("#txtMachineType").removeAttr(disabled).removeClass(disabled);
    //$("#txtNifudaNo").removeAttr(disabled).removeClass(disabled);
    $("#txtChuuban1").removeAttr(disabled).removeClass(disabled);
    $("#txtGyouban1").removeAttr(disabled).removeClass(disabled);
    $("#btnChuubanSearch").removeAttr(disabled).removeClass(disabled);
}

function DisableSearchPanel() {
    // $("#txtMachineType").attr(disabled, disabled).addClass(disabled);
    //$("#txtNifudaNo").attr(disabled, disabled).addClass(disabled);
    $("#txtChuuban1").attr(disabled, disabled).addClass(disabled);
    $("#txtGyouban1").attr(disabled, disabled).addClass(disabled);
    $("#btnChuubanSearch").attr(disabled, disabled).addClass(disabled);
}

function disableSaveButton() {
    // $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
    //$("#btnActual").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#btnSave").attr("disabled", "disabled");
    $("#btnActual").attr("disabled", "disabled");
    $("#btnChuubanSearch").attr("disabled", "disabled");
}

function enableSaveButton() {
    // $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
    // $("#btnActual").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnSave").removeAttr("disabled");
    $("#btnActual").removeAttr("disabled");
    $("#btnChuubanSearch").removeAttr("disabled");
}
function hideToleranceErrorLabels() {
    $("#lblDimToleranceMsgT1").hide();
    $("#lblLengthMsgT1").hide();
    $("#lblLengthToleranceMsgT1").hide();
    $("#lblDimToleranceMsgT4").hide();
    $("#lblLengthMsgT4").hide();
    $("#lblLengthToleranceMsgT4").hide();
}



$("#btnChuubanSearch").click(function (event) {
    registeredThabaNumbersAndDecision = {};
    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();

    if (!IsMandatoryChuubanSearchFieldsFilled()) {
        return false;
    }
    setStartSysDateTime();
    ValidateChubban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
    GetFICombinedDecisionDetails($("#txtChuuban1").val(), $("#txtGyouban1").val());
    //  getTensileDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
    // getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
    //  getMachineDetails($("#hdfMachineCD").val(), $callFrom);


});
$("#txtBreakTest1").change(function (event) {

    calculateD4Avg();
});
$("#txtBreakTest2").change(function (event) {

    calculateD4Avg();
});
$("#txtBreakTest3   ").change(function (event) {

    calculateD4Avg();
});
function calculateD4Avg() {

    var Test1;
    var Test2;
    var Test3;

    var TestAvg;
    var dividedby = 0;
    if ($("#txtBreakTest1").val() == "" || $("#txtBreakTest1").val() == null) {
        Test1 = 0;

    } else {

        Test1 = $("#txtBreakTest1").val();
    }
    if ($("#txtBreakTest2").val() == "" || $("#txtBreakTest2").val() == null) {
        Test2 = 0;
    } else {

        Test2 = $("#txtBreakTest2").val();
    }

    if ($("#txtBreakTest3").val() == "" || $("#txtBreakTest3").val() == null) {
        Test3 = 0;
    } else {

        Test3 = $("#txtBreakTest3").val();
    }


    if (Test1 == 0 && Test2 == 0 && Test3 == 0) {
        $("#txtActualDim99").val("");


    } else {

        if (Test1 > 0) {
            dividedby = parseInt(dividedby) + 1;
        } else {
            dividedby = parseInt(dividedby) + 0;
        }
        if (Test2 > 0) {
            dividedby = parseInt(dividedby) + 1;
        } else {
            dividedby = parseInt(dividedby) + 0;
        }
        if (Test3 > 0) {
            dividedby = parseInt(dividedby) + 1;
        } else {
            dividedby = parseInt(dividedby) + 0;
        }

        TestAvg = (parseInt(Test1) + parseInt(Test2) + parseInt(Test3)) / dividedby;
        $("#txtActualDim99").val(TestAvg.toFixed(2));
    }
}
function bindEvents() {

    $("#btnInitialize").click(function (event) {
        initialize();
        $("#btnSave").attr("disabled", "disabled");
        $("#btnActual").attr("disabled", "disabled");
    });



    $("#btnDateTime").click(function (event) {
        setStartSysDateTime();
    });
    //$("#txtOperator").change(function () {
    //    $("#hdfOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
    //    getOperatorNameByCD($(this));
    //});

    function toggleActualDimFieldsMandatory($this) {

        if ($this.is(':checked')) {
            $("#txtActualDim1").addClass("Mandatory");
        } else {
            $("#txtActualDim1").removeClass("Mandatory");
        }

    }
    function UpdateFICombinedDecision() {
        var ajaxTime = new Date().getTime();
        var tensileDecision = $("#ddlDecision").val();
        var processPlanCd = $("#hdfldProcessPlanCd").val();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Tensile/UpdateFICombinedDecision?tensileDecision=" + tensileDecision + "&processplancd=" + processPlanCd,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }

        $.ajax(settings)
            .done(function (response) {

                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }
    $btnSave.click(function (event) {
        var actualDim = true;
        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();
       
        if (!IsMandatoryFieldsFilled()) {
            return false;
        }
        if (!checkDate($("#txtInspDate"))) {
            var message = $("#lblErrorMessageE_Tensile_ENTRY_005").text().trim(); // This message will be displayed on dialog box
            var focusFieldID = "txtInspDate";
            showMessageDialog(message, focusFieldID, WARNING);
            return false;
        }

        if (!IsDateDifferentFromRange(true)) {
            var message = $("#lblErrorMessageE_Tensile_ENTRY_021").text().trim() + "(" + $("#lblErrorMessageE_Tensile_ENTRY_008").text().trim() + commonDateRange + ")";
            var focusFieldID = "txtStartDate";
            showMessageDialog(message, focusFieldID, WARNING);
            return false;
        }
        if (IsDuplicateThabaNo()) {
            return false;
        }

        //Check Furnance no in Hardness table Again this chuuban same or not?
        if ($("#txtFurnaceNum").val() != "") {
            if (!IsDifferentFurnanceNo()) {
                var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_010").text();
                showMessageDialog(errorMessage, "txtFurnaceNum", WARNING);
                return false;
            }
        }

       

        if ($("#chkActualDim").prop("checked") == true) {  //$("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK  now removing okdecision condition for all decision type as per windows
            var dimValues = {};
            dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
            dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
            dimValues["DIM1"] = $("#txtActDim").val() ? $("#txtActDim").val() : null;
            dimValues["DIM2"] = null;
            dimValues["DIM3"] = null;
            dimValues["DIM4"] = null;
            dimValues["DIM5"] = null;
            dimValues["DIM6"] = null;

            var result = IsActualDimentionValid(dimValues);
            if (result.IsSuccess == false) {
                actualDim = false;
            }
        }
        if (actualDim == false) {
            var message = $("#lblConfirmationMessageC_TENSILE_ENTRY_001").text().trim(); //ActDimOutOfTolerance
            var focusFieldID = "txtActDim";
            showMessageDialog(message, focusFieldID, WARNING);
            return false;

        }
        //Invalid value actual btn click value
        if (!InvalidTensileValues()) {
            //if ($("#ddlDecision").val() == 10) {
            //var message = $("#lblErrorMessageE_Tensile_ENTRY_011").text().trim();
            //showMessageDialog(message, "", WARNING);
            return false;
            //}
        }
        else if (fICombinedDecisions.length > 0) {
            var tensileDecision = $("#ddlDecision").val();
            for (i = 0; i < fICombinedDecisions.length; i++) {
                if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null)) ) {
                    if (tensileDecision == FIResultOk) {
                        bootbox.confirm({
                            title: "<div style='color:white'></div>",
                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#confMsgFIExistUpdateFIAsOK").text().trim() + "</td></tr></table>", 
                            buttons: {
                                confirm: {
                                    label: $("#lblYes").text(),
                                    className: 'btn-success'
                                },
                                cancel: {
                                    label: $("#lblNo").text(),
                                    className: 'btn-danger'
                                }
                            },
                            callback: function (result) {

                                if (result) {
                                    debugger;
                                    bootbox.hideAll();
                                    UpdateFICombinedDecision();
                                    SaveDetails($callFrom);
                                }
                                else {
                                    bootbox.hideAll();
                                    $("#ddlDecision").focus();
                                }


                            },
                            centerVertical: true
                        });
                    }
                    else if (tensileDecision != FIResultOk) {
                        bootbox.confirm({
                            title: "<div style='color:white'></div>",
                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                            buttons: {
                                confirm: {
                                    label: $("#lblYes").text(),
                                    className: 'btn-success'
                                },
                                cancel: {
                                    label: $("#lblNo").text(),
                                    className: 'btn-danger'
                                }
                            },
                            callback: function (result) {

                                if (result) {
                                    debugger;
                                    bootbox.hideAll();
                                    SaveDetails($callFrom);
                                }
                                else {
                                    bootbox.hideAll();
                                    $("#ddlDecision").focus();
                                }


                            },
                            centerVertical: true
                        });
                    }

                    else {
                        SaveDetails($callFrom);
                    }
                }
                else if (CombinedDecisionArr.includes(parseInt(FIResultOk))) {
                    if (tensileDecision != FIResultOk) {
                        bootbox.confirm({
                            title: "<div style='color:white'></div>",
                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                            buttons: {
                                confirm: {
                                    label: $("#lblYes").text(),
                                    className: 'btn-success'
                                },
                                cancel: {
                                    label: $("#lblNo").text(),
                                    className: 'btn-danger'
                                }
                            },
                            callback: function (result) {

                                if (result) {
                                    debugger;
                                    bootbox.hideAll();
                                    SaveDetails($callFrom);
                                }
                                else {
                                    bootbox.hideAll();
                                    $("#ddlDecision").focus();
                                }


                            },
                            centerVertical: true
                        });
                    }
                    else {
                        SaveDetails($callFrom);
                    }
                }
                else {
                    SaveDetails($callFrom);
                }
                break;
            }
        }
        else {
            SaveDetails($callFrom);
        }
       

    });

    function IsDifferentFurnanceNo() {

        var chuuban = $("#txtChuuban1").val();
        var gyouban = $("#txtGyouban1").val();
        var splitNo = $("#txtSplitNo").val();

        var returnval = false;

        // $callFrom.data('requestRunning', true);
        var ajaxTime = new Date().getTime();
        //var jsonData = "{PP_CHUUBAN :'" + chuuban + "', TS_FURNACE_NO : '" + $("#txtFurnaceNum").val()+ "'  , PP_GYOUBAN : '" + gyouban + "', USER_NAME: '" + userName + "' }";

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Tensile/GetFurnanceNo",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, DETAIL_NO: splitNo })
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;

                var jsonResult = response.message;
              
                if (response.message != null) {
                    if (response.message.hD_FURNACE_NO != null) {
                        var txtFurnance_No = ($("#txtFurnaceNum").val()).toUpperCase();
                        var hd_Furnace_No = jsonResult.hD_FURNACE_NO;
                        if (hd_Furnace_No != txtFurnance_No) {
                            returnval = false;
                        }
                        else {
                            returnval = true;
                        }
                    }

                    else {
                        returnval = true;
                    }
                }
                else {
                    returnval = true;

                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;
                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
        return returnval;
    }

    function InvalidTensileValues() {
        debugger;
        var endTensileStrength = $("#txtEndTensileStrength").val();
        var endYieldPoint = $("#txtEndYieldYS").val();
        var endElongation = $("#txtEndElongation").val();
        var endReduction = $("#txtEndReduction").val();
        var tensileStrength1 = $("#txtTensileStrength1").val().split(' ');
        var yieldPoint1 = ($("#txtYieldPoint1").val().split(' '));
        var elongation1 = ($("#txtElongation1").val().split(' '));
        var reduction1 = ($("#txtReduction1").val().split(' '));
        var returnval = true;
        var tensilKubun = $("#hdfTensileKubunType").val();
        var elongationKubun = $("#hdfElongationKubunType").val();
        //var hardnessKubun = $("#hdfHardnessKubunType").val();
        var yeildKubun = $("#hdfYeildKubunType").val();
        var pressKubun = $("#hdfPressKubunType").val();
        var result = ActualTensileValue();
        if (result == false) {
            return false;
        }
        else {
            if ($("#ddlDecision").val() == 10) {
                //Tensile Strength
                if (tensileStrength1 != "") {
                    if (CheckPatternValidation(tensilKubun, tensileStrength1, parseFloat($("#txtEndTensileStrength").val()))) {
                        returnval = true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    returnval = true
                }
                //Yield Point
                if (yieldPoint1 != "") {
                    if (CheckPatternValidation(yeildKubun, yieldPoint1, parseFloat($("#txtEndYieldYS").val()))) {
                        returnval = true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    returnval = true
                }
                //Elongation
                if (elongation1 != "") {
                    if (CheckPatternValidation(elongationKubun, elongation1, parseFloat($("#txtEndElongation").val()))) {
                        returnval = true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    returnval = true
                }
                //Reduction
                if (reduction1 != "") {
                    if (CheckPatternValidation(pressKubun, reduction1, parseFloat($("#txtEndReduction").val()))) {
                        returnval = true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    returnval = true
                }
            }
        }
        
        return returnval;
    }
    function CheckPatternValidation(kubunType,stdPatternVal, calVal) {
        var stdMinLimitVal;
        var stdMaxLimitVal;
        var calculateVal;
        var returnval = false;
        var prodSpecMinVal = $("#hdfProdSpecMin").val();
        var prodSpecMaxVal = $("#hdfProdSpecMax").val();
        if (!Number.isNaN(calVal)) {
            if (kubunType == prodSpecMinVal) {
                    stdMinLimitVal = parseFloat(stdPatternVal[0]); // 980
                    calculateVal = calVal; //900

                    if (calculateVal > stdMinLimitVal) {
                        returnval = true;  // data will save
                    }
                    else {
                        returnval = false; // message will display
                        var message = $("#lblErrorMessageE_Tensile_ENTRY_011").text().trim();
                        showMessageDialog(message, "", WARNING);
                        return false;
                    }
                }
            else if (kubunType == prodSpecMaxVal) {
                    stdMinLimitVal = parseFloat(stdPatternVal[0]);
                    calculateVal = calVal;
                    if (calculateVal < stdMinLimitVal) {
                        returnval = true;
                    }
                    else {
                        returnval = false;
                        var message = $("#lblErrorMessageE_Tensile_ENTRY_011").text().trim();
                        showMessageDialog(message, "", WARNING);
                        return false;
                    }
                }
            else if (String(stdPatternVal).includes("～") && kubunType == 0) {
                    var stdMinMaxVal = String(stdPatternVal).split("～");
                    stdMinLimitVal = parseFloat(stdMinMaxVal[0]);
                    stdMaxLimitVal = parseFloat(stdMinMaxVal[1]);
                    calculateVal = calVal;
                    if (calculateVal > stdMinLimitVal && calculateVal < stdMaxLimitVal) {
                        returnval = true;   
                    } else {
                        returnval = false;  
                        var message = $("#lblErrorMessageE_Tensile_ENTRY_011").text().trim();
                        showMessageDialog(message, "", WARNING);
                        return false;
                    }
                }
                else {
                    returnval = true;
                }
            
        }
        else {
            returnval = true;
        }
        return returnval;
    }
    function ActualTensileValue() {
        debugger;
        var yieldLoadN = null;
        var actDim = null;
        var cutDim = null;
        $("#txtEndYieldYS").val("");
        $("#txtEndReduction").val("");
        $("#txtEndElongation").val("");
        $("#txtEndTensileStrength").val("");
        var returnVal = true;
        //TS_YIELD_STRENGTH  - NUMBER(4,0)
        if ($("#txtYieldLoadN").val().length >= 1 && $("#txtActDim").val().length >= 1) {
            yieldLoadN = $("#txtYieldLoadN").val();
            actDim = $("#txtActDim").val();
            var result1 = (yieldLoadN / (3.14 * (actDim / 2) * (actDim / 2))).toFixed(2);

            var numResult = Number(result1);
            // Check if result fits within NUMBER(4,0)

            if (numResult >= -999 && numResult <= 9999) {
                $("#txtEndYieldYS").val(Math.round(Number(result1)));
                var txtEndYieldYS = document.getElementById("txtEndYieldYS");
                txtEndYieldYS.style.color = "black";
                txtEndYieldYS.style.textAlign = "right";
                returnVal = true;
            } else {
                var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
                var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
                $("#txtEndYieldYS").val(errorLable);
                var txtEndYieldYS = document.getElementById("txtEndYieldYS");
                txtEndYieldYS.style.color = "red";
                txtEndYieldYS.style.textAlign = "left";
                focusFieldID = $(':focus').attr("id");
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;
                return false;

            }
        }

        //TS_REDUCTION - NUMBER(3,1)
        if ($("#txtCutDim").val().length >= 1 && $("#txtActDim").val().length >= 1) {
            cutDim = $("#txtCutDim").val();
            actDim = $("#txtActDim").val();
            //var result2 = ((((cutDim * cutDim * 0.785) - (actDim * actDim * 0.785)) * (-100)) / (-(actDim * actDim * 0.785))).toFixed(1);    //for '-' sign line commented as per windows code
            var result2 = ((((cutDim * cutDim * 0.785) - (actDim * actDim * 0.785)) * (-100)) / ((actDim * actDim * 0.785))).toFixed(1);

            var numResult = Number(result2);
            // Check if result fits within NUMBER(3,1)
            debugger;
            if (numResult >= -99.9 && numResult <= 99.9) {
                $("#txtEndReduction").val(result2);
                var txtEndReduction = document.getElementById("txtEndReduction");
                txtEndReduction.style.color = "black";
                txtEndReduction.style.textAlign = "right";
            } else {
                var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
                var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
                $("#txtEndReduction").val(errorLable);
                var txtEndReduction = document.getElementById("txtEndReduction");
                txtEndReduction.style.color = "red";
                txtEndReduction.style.textAlign = "left";
                focusFieldID = $(':focus').attr("id");
                showMessageDialog(message, focusFieldID, WARNING);
                return false;

            }

        }
        //TS_ELONGATION- NUMBER(3,1)
        if ($("#txtCuttingElongation").val().length >= 1 && $("#txtGaugeLength").val().length >= 1) {
            var cuttingElongation = $("#txtCuttingElongation").val();
            var gaugeLength = $("#txtGaugeLength").val();
            var result3 = (((cuttingElongation - gaugeLength) * 100) / gaugeLength).toFixed(1);
            //159900 =(((800 - 0.5) * 100) / 0.5).toFixed(1); //error
            var numResult = Number(result3);

            // Check if result fits within NUMBER(3,1)
            debugger;
            if (numResult >= -99.9 && numResult <= 99.9) {
                $("#txtEndElongation").val(result3);
                var txtEndElongation = document.getElementById("txtEndElongation");
                txtEndElongation.style.color = "black";
                txtEndElongation.style.textAlign = "right";
            } else {
                var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
                var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
                $("#txtEndElongation").val(errorLable);
                var txtEndElongation = document.getElementById("txtEndElongation");
                txtEndElongation.style.color = "red";
                txtEndElongation.style.textAlign = "left";
                focusFieldID = $(':focus').attr("id");
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }

        }


        if ($("#txtMaxLoadN").val().length >= 1 && $("#txtActDim").val().length >= 1) {
            var maxLoadN = parseFloat($("#txtMaxLoadN").val());
            var actDim = parseFloat($("#txtActDim").val());


            var result;

            // Calculate based on selected shape
            if ($("#ddlDimShape").val() == 2) {
                result = (maxLoadN / (actDim * actDim * 0.866)).toFixed(3);
            } else if ($("#ddlDimShape").val() == 1) {
                result = (maxLoadN / (actDim * actDim * 0.785)).toFixed(3);
            } else if ($("#ddlDimShape").val() == 3) {
                result = (maxLoadN / (actDim * actDim)).toFixed(3);
            }

            // Convert to number and validate NUMBER(7,3)
            var numResult = parseFloat(result);
            if (numResult >= -9999.999 && numResult <= 9999.999) {
                $("#txtEndTensileStrength").val(Math.round(result));
                var txtEndTensileStrength = document.getElementById("txtEndTensileStrength");
                txtEndTensileStrength.style.color = "black";
                txtEndTensileStrength.style.textAlign = "right";
            } else {
                var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
                var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();

                $("#txtEndTensileStrength").val(errorLable);
                var txtEndTensileStrength = document.getElementById("txtEndTensileStrength");
                txtEndTensileStrength.style.color = "red";
                txtEndTensileStrength.style.textAlign = "left";
                focusFieldID = $(':focus').attr("id");
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
        }

        return returnVal;

    }
    $btnActual.click(function (event) {
        ActualTensileValue();
    });

}

$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // window.moveTo(0, 0);
   
    window.resize(1220, 970);
    setGlobalVariables();
    disableDataPanel();

    bindEvents();
    setDecimalFormats();


    $("#txtChuuban1").focus();

    $("#btnActual").attr(disabled, disabled).addClass(disabled);
    $("#btnSave").attr("disabled", "disabled");


    setShapeList();
    setDecisionList();
    setNextProcess();

    setJISList();
    //setOperatorList("");

});

//function IsDateDifferentFromRange(dateVal) {
//    debugger;
//    var returnval = true;
//    var currentDate = new Date();

//    var minDate = new Date();
//    var maxDate = new Date();
//    minDate.setDate(currentDate.getDate() - parseInt(commonDateRange));
//    maxDate.setDate(currentDate.getDate() + parseInt(commonDateRange));

//    var minimumDate = minDate.getFullYear() + "/" + String('00' + minDate.getMonth() + 1).slice(-2) + "/" + String('00' + minDate.getDate()).slice(-2) + " " + String('00' + minDate.getHours()).slice(-2) + ":" + String('00' + minDate.getMinutes()).slice(-2);
//    var maximumDate = maxDate.getFullYear() + "/" + String('00' + maxDate.getMonth() + 1).slice(-2) + "/" + String('00' + maxDate.getDate()).slice(-2) + " " + String('00' + maxDate.getHours()).slice(-2) + ":" + String('00' + maxDate.getMinutes()).slice(-2);

//    if (Date.parse(dateVal) <= Date.parse(minimumDate) || Date.parse(dateVal) >= Date.parse(maximumDate)) {
//        returnval = false;
//    }


//    return returnval;
//}

function IsDateDifferentFromRange(boolVal) {
    debugger;
    var returnval = false;
    var getDate;
    if (boolVal === true) {
        getDate = ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

    }
    //else {
    //    getDate = ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    //}

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });




    return returnval;
}
function ValidateChubban(chuuban, gyouban, $callFrom) {


    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/ValidateChubban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message
            if (result.responce == 4) {
                disableDataPanel();
                ClearData();
                // Chuuban is marked as completed in the system/
                var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuuban1", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                disableDataPanel();
                disableSaveButton();
                ClearData();
                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_002").text();
                showMessageDialog(errorMessage, "txtChuuban1", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                var message = $("#lblErrorMessageE_Tensile_ENTRY_009").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                showMessageDialog(message, "txtChuuban1", WARNING);
                ClearData();
                disableDataPanel();
                return false;
            }
            else if (result.responce == 1) {

                getTensileDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_003").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function GetFICombinedDecisionDetails(chuuban, gyouban) {
    var returnVal = true;
    CombinedDecisionArr = [];
    SparkDecisionArr = [];
    //var displayNote = false;

    var jsonData = JSON.stringify({
        CHUUBAN: chuuban,
        GYOUBAN: gyouban
    });
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetFICombinedDecisionDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData

    }

    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            fICombinedDecisions = jsonResult;
            debugger;
            if (jsonResult.length > 0) {
                debugger;
                document.getElementById('FIDoneWarning').style.display = 'block';

                for (i = 0; i < fICombinedDecisions.length; i++) {
                    CombinedDecisionArr.push(fICombinedDecisions[i].combineD_DECISION);
                    SparkDecisionArr.push(fICombinedDecisions[i].fI_SPARK_DECISION);
                }

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnVal;

}
$('#txtChuuban1').keypress(function (e) {
    var orderNo = $('#txtChuuban1').val() ? $('#txtChuuban1').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Tensile/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuuban1').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtGyouban1').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btnChuubanSearch').focus();
                } else {
                    $('#txtChuuban1').val("");
                    $('#txtGyouban1').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_Tensile_ENTRY_003").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});

$("#btnActual").focusout(function (event) {
    $("#btnSave").focus();
});
$("#txtActDim").change(function (event) {
    if ($("#txtActDim").val() != null && $("#txtActDim").val().trim() !== "") {
        $("#ddlDimShape").removeAttr(disabled).removeClass(disabled);
        $("#ddlDimShape").addClass("Mandatory");
        $("#ddlDimShape").focus();
    }
    else {
        $("#txtTestSpecimentDim").focus();
        $("#ddlDimShape").attr(disabled, disabled).addClass(disabled);
        $("#ddlDimShape").removeClass("Mandatory");
        setShapeList();

    }
});
$("#ddlJISTest").change(function (event) {
    var ajaxTime = new Date().getTime();
    var testSpecCd = $("#ddlJISTest option:selected").val() ? $("#ddlJISTest option:selected").val() : null;
    var jsonData = "{ USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetJISTestMagnification?testSpecCd=" + testSpecCd,
        method: "POST",
        contentType: "application/json; charset=utf-8"
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var testSpecMagnification = response.message;
            if ($("#txtActDim").val() != null && $("#txtActDim").val().trim() !== "" && testSpecCd !=0) {
                var actDimVal = $("#txtActDim").val();
                $("#txtGaugeLength").val(Number((Number(actDimVal) * testSpecMagnification).toFixed(2)));
                $("#txtGaugeLength").attr(disabled, disabled).addClass(disabled);
            }
            else {
                $("#txtGaugeLength").removeClass(disabled).removeAttr(disabled);
                $("#txtGaugeLength").val("");
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
});

$('#txtOperator')
    .on('focus', function () {
        operatorStartModified = false;
    })
    .on('input', function () {
        operatorStartModified = true;
    });

$('#txtOperator').on('keydown focusout', function (e) {
    debugger;
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorStartModified == true) {

        $("#hdfStartOperatorCD").val(operatorNumber);
        enterPressedStartOpr = true;
        GetOperatorNameByCD($(this));
        $("#txtFurnaceNum").focus();
    }
    if (e.type === 'focusout' && operator && operatorStartModified == true) {
        if (enterPressedStartOpr) {

            enterPressedStartOpr = false; // Reset the flag
            return false;
        }
        $("#hdfStartOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtFurnaceNum").focus();
    }
    enterPressedStartOpr = false; // Reset the flag
});

function GetOperatorNameByCD($target) {
    debugger;
    var ajaxTime = new Date().getTime();

    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_Tensile_ENTRY_017").text().trim().replace("*****", $("#lblInspector").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_Tensile_ENTRY_018").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageE_Tensile_ENTRY_018").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Tensile/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",


        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;


                if (jsonResult == null || jsonResult == "") {
                    var message = $("#lblErrorMessageE_Tensile_ENTRY_019").text().trim().replace("*****", $("#lblInspector").text());
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }//employee is inactive.
                else if (jsonResult.iN_ACTIVE_FLAG == 1) {

                    var message = $("#lblErrorMessageE_Tensile_ENTRY_020").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val("");
                }

                else {
                    $target.val(jsonResult.yobina);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_Tensile_ENTRY_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            })
    }
}