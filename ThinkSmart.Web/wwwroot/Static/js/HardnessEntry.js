
var $btnChuubanSearch;
var $btnSave;
var combinedDecisionOK = "";
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var operatorStartModified = false;
let enterPressedStartOpr = false;

var tsCD;
var registeredThabaNumbersAndDecision = {};
var fICombinedDecisions = {};
var m_hardnessStd = null;
let CombinedDecisionArr = [];
let SparkDecisionArr = [];

let validMinVal = null;
let validMaxVal = null;
const FIResultOk = $("#hdfFIResultOk").val();
var FIResultNg = $("#hdfFIResultNg").val();
var FIResultUntested = $("#hdfFIResultUntested").val();
var FIResultSparkUntested = $("#hdfFIResultSparkUntested").val();
var FISparkUntested = $("#hdfFISparkUntested").val();
var FISparkOk = $("#hdfFISparkOk").val();
var FISparkNg = $("#hdfFISparkNg").val();

// Error log information. To be sent with each AJAX request. 
userName = ""; // assigned at Document.ready
// End error log information./


// Following decision legends are to show decision colors on Top Thaba Grid.
//var decisionLegends = {
//    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
//    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
//    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
//    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>"
//}

function ShowMessageDialog(message, focusFieldID, messageType) {

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

function SetGlobalVariables() {
    $btnChuubanSearch = $("#btnChuubanSearch");
    $btnSave = $("#btnSave");
    $hdfSYSDATE = $("#hdfSYSDATE");
    $hdfCultureLoaded = $("#hdfCultureLoaded");
    $hdfCultureEN = $("#hdfCultureEN");
    $hdfCultureJP = $("#hdfCultureJP");
}

function ResetGlobalVariables() {
    $("#hdfDBOperation").val("");

}

function DisableDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#divProcessStartData");
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
    var $endDataPanel = $("#pnlProcessEndData");
    $.each($endDataPanel.find("input:submit"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });
}

$('#txtOperator')
    .on('focus', function () {
        operatorStartModified = false;
    })
    .on('input', function () {
        operatorStartModified = true;
    });

$('#txtOperator').on('keydown focusout', function (e) {
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
        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_017").text().trim().replace("*****", $("#lblInspector").text());

        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_018").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_018").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Hardness/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",


        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;


                if (jsonResult == null || jsonResult == "") {
                    var message = $("#lblErrorMessageE_HARDNESS_ENTRY_019").text().trim().replace("*****", $("#lblInspector").text());
                    ShowMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }//employee is inactive.
                else if (jsonResult.iN_ACTIVE_FLAG == 1) {

                    var message = $("#lblErrorMessageE_HARDNESS_ENTRY_020").text().trim();
                    ShowMessageDialog(message, $target.attr("id"), WARNING);
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
                message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            })
    }
}

function ClearData() {
    combinedDecisionOK = "";
    SetSysDateTime();
    EnableSaveButton();
    EnableSearchPanel();
    $(".FocusDefault").focus();
    DisableDataPanel();
    ClearPanel($("#pnlHardnessPattern"));
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    ClearPanel($("#pnlBottomData"));
    ClearPanel($("#pnlProcessInfo")); // Tab 4
    ClearPanel($("#pnlTestSpecifications")); // Tab 4    
    ClearThabaDetailsGrid();
    ClearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4

    ClearPanel($("#divProcessStartData"));

    HideToleranceErrorLabels();

    ResetGlobalVariables();
    ResetMessageDetailsGrid();

    $("#chkActualDim").prop('checked', true);
   // $("#chkIncludesCenterInAvg").prop('checked', true);

}

//btn Initialize click methods
function Initialize() {
    combinedDecisionOK = "";
    registeredThabaNumbersAndDecision = {};
    EnableSaveButton();
    EnableSearchPanel();
    $(".FocusDefault").focus();
    DisableDataPanel();
    ClearPanel($("#pnlHardnessPattern"));
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    ClearPanel($("#pnlBottomData"));
    ClearPanel($("#pnlProcessInfo")); // Tab 4
    ClearPanel($("#pnlTestSpecifications")); // Tab 4    
    ClearThabaDetailsGrid();
    ClearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4


    $("#txtChuuban1").val("");
    $("#txtGyouban1").val("");
    $("#txtCenterRef").removeClass("Mandatory");
    ClearPanel($("#divProcessStartData"));

    HideToleranceErrorLabels();

    ResetGlobalVariables();
    ResetMessageDetailsGrid();
    //AddProdPointDataRow();

    $("#chkActualDim").prop('checked', true);
    $("#chkIncludesCenterInAvg").prop('checked', true);
    fICombinedDecisions = {};
    document.getElementById('FIDoneWarning').style.display = 'none';
}

function ClearPanel($panel) {
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

    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
    });

    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}

function ClearThabaDetailsGrid() {
    $pnlThabaDetails = $("#DivThabaDetails");
    $.each($pnlThabaDetails.find("label[id^='lblThabaNo']"), function (index, span) {
        $(span).html("");
    });
    $.each($pnlThabaDetails.find("label[id^='lblDecisionNo']"), function (index, span) {
        $(span).html("");
    });
}

function ClearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}

function CalculateD4Avg() {

    var D4_1;
    var D4_2;
    var D4_3;
    var D4_4;
    var centerVal;
    var D4Avg;
    var totalsum;
    if ($("#txtD1").val() == "" || $("#txtD1").val() == null) {
        D4_1 = 0;

    } else {

        D4_1 = $("#txtD1").val();
    }
    if ($("#txtD2").val() == "" || $("#txtD2").val() == null) {
        D4_2 = 0;
    } else {

        D4_2 = $("#txtD2").val();
    }

    if ($("#txtD3").val() == "" || $("#txtD3").val() == null) {
        D4_3 = 0;
    } else {

        D4_3 = $("#txtD3").val();
    }
    if ($("#txtD4").val() == "" || $("#txtD4").val() == null) {
        D4_4 = 0;
    } else {

        D4_4 = $("#txtD4").val();
    }

    if ($("#txtCenterRef").val() == "" || $("#txtCenterRef").val() == null) {
        centerVal = 0;
    } else {

        centerVal = $("#txtCenterRef").val();
    }

    if ($("#txtD1").val() == "" || $("#txtD1").val() == null || $("#txtD2").val() == "" || $("#txtD2").val() == null || $("#txtD3").val() == "" || $("#txtD3").val() == null) {
        $("#txtD4Avg").val("");
        $("#txtcheckD4").addClass("Mandatory");

    } else {
        $("#txtcheckD4").removeClass("Mandatory");
        if ($("#txtD4").val() == "" || $("#txtD4").val() == null) {

            if (centerVal > 0 && $("#chkIncludesCenterInAvg").prop("checked") == true) {
                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3) + parseFloat(centerVal);
                D4Avg = totalsum / 4;

            } else {

                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3);
                D4Avg = totalsum / 3;
            }


        } else {


            if (centerVal > 0 && $("#chkIncludesCenterInAvg").prop("checked") == true) {
                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3) + parseFloat(D4_4) + parseFloat(centerVal);
                D4Avg = totalsum / 5;

            } else {
                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3) + parseFloat(D4_4);

                D4Avg = totalsum / 4;
            }
        }
        $("#txtD4Avg").val(D4Avg.toFixed(2));
    }
}

function EnableSearchPanel() {
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

function DisableSaveButton() {
    $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#btnChuubanSearch").attr(DISABLED, DISABLED).addClass(DISABLED);
}

function EnableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnChuubanSearch").removeAttr(DISABLED).removeClass(DISABLED);
}

function HideToleranceErrorLabels() {
    $("#lblDimToleranceMsgT1").hide();
    $("#lblLengthMsgT1").hide();
    $("#lblLengthToleranceMsgT1").hide();
    $("#lblDimToleranceMsgT4").hide();
    $("#lblLengthMsgT4").hide();
    $("#lblLengthToleranceMsgT4").hide();
}

function ResetResponseTime() {
    responseTime = ""; // Clear server response time
}

function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        ShowMessageDialog(message, firstFieldID, WARNING);
    }
    return returnVal;
}

function SetChuubanDetails(jsonResult) {

    debugger;

    combinedDecisionOK = jsonResult.decisioN_OK;

    //  stamppingCD = $.isNumeric(jsonResult.DRAWING_CD) ? parseInt(jsonResult.DRAWING_CD) : 0;
    DrawingCD = $.isNumeric(jsonResult.drawinG_CD) ? parseInt(jsonResult.drawinG_CD) : 0;
    $("#txtSplitNo").focus();
    var newSplitNo = jsonResult.lasT_DETAIL_NO + 1;
    $("#txtSplitNo").val(newSplitNo);
    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);


    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN);
    $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN);
    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);

    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtCustomerInchargeT4").val(jsonResult.pP_EMPLOYEE_NAME);

    $("#txtHostRefNoT1").val(jsonResult.pP_NYUKA_CD);
    $("#txtHostRefNoT4").val(jsonResult.pP_NYUKA_CD);

    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtStdDeliveryDateT4").val(jsonResult.pP_STD_DEL_PERIOD_STRING);

    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtCustomerT4").val(jsonResult.pP_CUSTOMER_NAME);

    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);
    $("#txtReceiverT4").val(jsonResult.pP_RECEIVER_NAME);

    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
        $("#txtChamferT4").val(jsonResult.pP_CHAMFER);
    }
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) != null) {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
        $("#txtUnitWtT4").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
    }

    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtColorT4").val(jsonResult.pP_COLOR);

    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMaterialNameMakerT4").val(jsonResult.pP_MATL_NAME_MAK);

    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlShapeT4").val(jsonResult.pP_MATL_SHAPE);


    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);
    $("#txtMatlDimT4").val(jsonResult.pP_MATL_DIM_D);


    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtChargeT4").val(jsonResult.pP_CHARGE);

    $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtBendStandardT4").val(jsonResult.pP_MAGARI_BEND_TOL);

    $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdNameCustomerT4").val(jsonResult.pP_PROD_NAME_CUST);

    $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdShapeT4").val(jsonResult.pP_PROD_SHAPE);

    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);
    $("#txtProdDimT4").val(jsonResult.pP_PROD_DIM_D);

    $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtProdDimOtherT4").val(jsonResult.pP_PROD_DIM_OTHER);

    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtExtTextureT4").val(jsonResult.pP_EXT_TEXTURE);

    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDimToleranceT4").val(jsonResult.pP_DIM_TOLERANCE);

    if (jsonResult.pP_DIM_TOL_MIN_CHR != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    if (jsonResult.pP_DIM_TOL_MAX_CHR != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    dimStd = jsonResult.pP_PROD_DIM_D;
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtConditionT4").val(jsonResult.pP_PRODUCT_CONDITION);

    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);

    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtStampT4").val(jsonResult.pP_STAMP);

    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_STRING);

    //$("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    //$("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);


    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
        $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
    }
    else {
        $("#txtLengthToleranceT1").val('');
        $("#txtLengthToleranceT4").val('');
    }

    if (jsonResult.pP_LENGTH_TOL_MIN_CHR != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    if (jsonResult.pP_LENGTH_TOL_MAX_CHR != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    lenStd = jsonResult.pP_PROD_LENGTH;

    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtPackingT4").val(jsonResult.pP_PACKING_DECODE);

    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtNoOfUnitsT4").val(jsonResult.pP_NO_OF_UNITS_DECODE);

    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtQuantityT4").val(jsonResult.pP_ACT_ALLOC_QTY);

    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtIntDiaMinT4").val(jsonResult.pP_COIL_DIA_INTERNAL);

    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtExtDiaMaxT4").val(jsonResult.pP_COIL_DIA_EXTERNAL);

    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtWindingDirectionT4").val(jsonResult.pP_COIL_DIA_WINDING);

    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtKakouiraiRemarksT4").val(jsonResult.pP_KAKOUIRAI_REMARKS);

    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    $("#txtProdOrderRemarkT4").val(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);

    $("#txtLastSplitNo").val(jsonResult.lasT_DETAIL_NO);


    //if (jsonResult.dW_EC_DONE_CHK == "1") {
    //    $("#chkECOnline").checked = true;
    //}
    //else {
    //    $("#chkECOnline").checked = false;
    //}

    // 3rd Tab
    ResetMessageDetailsGrid();

    if (jsonResult.mI_MESSAGE_LIST) {
        SetMessageDetailsGrid(jsonResult.mI_MESSAGE_LIST);
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

    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4      

    // Set ProdPoint details
    if (jsonResult.pP_PROCESS_1 !== null && jsonResult.pP_PROCESS_1 !== "") {
        AddProdPointDataRow("1", jsonResult.pP_PROCESS_1, jsonResult.pP_PROD_POINT_1)
    }
    if (jsonResult.pP_PROCESS_2 !== null && jsonResult.pP_PROCESS_2 !== "") {
        AddProdPointDataRow("2", jsonResult.pP_PROCESS_2, jsonResult.pP_PROD_POINT_2)
    }
    if (jsonResult.pP_PROCESS_3 !== null && jsonResult.pP_PROCESS_3 !== "") {
        AddProdPointDataRow("3", jsonResult.pP_PROCESS_3, jsonResult.pP_PROD_POINT_3)
    }
    if (jsonResult.pP_PROCESS_4 !== null && jsonResult.pP_PROCESS_4 !== "") {
        AddProdPointDataRow("4", jsonResult.pP_PROCESS_4, jsonResult.pP_PROD_POINT_4)
    }
    if (jsonResult.pP_PROCESS_5 !== null && jsonResult.pP_PROCESS_5 !== "") {
        AddProdPointDataRow("5", jsonResult.pP_PROCESS_5, jsonResult.pP_PROD_POINT_5)
    }
    if (jsonResult.pP_PROCESS_6 !== null && jsonResult.pP_PROCESS_6 !== "") {
        AddProdPointDataRow("6", jsonResult.pP_PROCESS_6, jsonResult.pP_PROD_POINT_6)
    }
    if (jsonResult.pP_PROCESS_7 !== null && jsonResult.pP_PROCESS_7 !== "") {
        AddProdPointDataRow("7", jsonResult.pP_PROCESS_7, jsonResult.pP_PROD_POINT_7)
    }
    if (jsonResult.pP_PROCESS_8 !== null && jsonResult.pP_PROCESS_8 !== "") {
        AddProdPointDataRow("8", jsonResult.pP_PROCESS_8, jsonResult.pP_PROD_POINT_8)
    }
    if (jsonResult.pP_PROCESS_9 !== null && jsonResult.pP_PROCESS_9 !== "") {
        AddProdPointDataRow("9", jsonResult.pP_PROCESS_5, jsonResult.pP_PROD_POINT_9)
    }
    if (jsonResult.pP_PROCESS_10 !== null && jsonResult.pP_PROCESS_10 !== "") {
        AddProdPointDataRow("10", jsonResult.pP_PROCESS_6, jsonResult.pP_PROD_POINT_10)
    }
    if (jsonResult.pP_PROCESS_11 !== null && jsonResult.pP_PROCESS_11 !== "") {
        AddProdPointDataRow("11", jsonResult.pP_PROCESS_11, jsonResult.pP_PROD_POINT_11)
    }
    if (jsonResult.pP_PROCESS_12 !== null && jsonResult.pP_PROCESS_12 !== "") {
        AddProdPointDataRow("12", jsonResult.pP_PROCESS_12, jsonResult.pP_PROD_POINT_12)
    }
    if (jsonResult.pP_PROCESS_13 !== null && jsonResult.pP_PROCESS_13 !== "") {
        AddProdPointDataRow("13", jsonResult.pP_PROCESS_13, jsonResult.pP_PROD_POINT_13)
    }


    if (jsonResult.hardnesS_PATTERN1_CODE > 0) {
        $("#txtHardnessData1").val(jsonResult.hardnesS_PATTERN1_CODE_STRING);
        $("#txtHardnessData2").val(jsonResult.hardnesS_PATTERN1_MIN_MAX_STRING);
        $("#txtHardnessData3").val(jsonResult.hardnesS_PATTERN1);
    }
    else if (jsonResult.hardnesS_PATTERN2_CODE > 0) {
        $("#txtHardnessData1").val(jsonResult.hardnesS_PATTERN2_CODE_STRING);
        $("#txtHardnessData2").val(jsonResult.hardnesS_PATTERN2_MIN);
        $("#txtHardnessData3").val(jsonResult.hardnesS_PATTERN2_MAX);
    } else {
        $("#txtHardnessData1").val("");
        $("#txtHardnessData2").val("");
        $("#txtHardnessData3").val("");
    }

}

function GetDecision(val) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{Decision :'" + val + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Decision: val })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // BindNextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetNextProcess() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindNextProcessDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindNextProcessDropdown(jsonResult) {

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

function SetHardnessKubuns() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Hardness/SetHardnessKubuns",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindHardnessKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindHardnessKubunDropdown(jsonResult) {
    $("#ddlHardnessKubun2").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlHardnessKubun2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function SetDecisionList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Hardness/SetDecisionList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDecisionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindDecisionDropdown(jsonResult) {
    /*$("#ddlDecision").html("").append($('<option value="0"></option>'));*/
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function SetSysDateTime() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;


            $("#txtDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });




        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

//function getOperatorList(yobina, $target) {

//    var ajaxTime = new Date().getTime();
//    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

//    var settings = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/Hardness/GetOperatorList?name=" + yobina,
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonData + "}",

//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            bindOperatorDropdown(yobina, $target, response.message);

//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
//            ShowMessageDialog(message, focusFieldID, ERROR);
//        })
//}

//function bindOperatorDropdown(yobina, $target, jsonResult) {

//    $target.html("").append($('<option value="0"></option>'));
//    jsonResult = jsonResult.filter(row => row.iN_ACTIVE_FLAG == 0)


//    $.each(jsonResult, function (key, jsonObj) {

//        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + jsonObj.yobina.toLowerCase() + '">' + jsonObj.name + '</option>'));
//    });

//}

function ResetMessageDetailsGrid() {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $('<td class="GridAlignmentDateTime">').text(""),
        $('<td class="GridAlignmentMachineName">').text(""),
        $('<td class="GridAlignmentProcessName">').text(""),
        $('<td class="GridAlignmentOperator">').text(""),
        $('<td class="GridAlignmentRemarks">').text("")
    );
    $grdvMessage.append($tr);
}

function SetMessageDetailsGrid(messageDetailsList) {

    $grdvMessage = $("#grdvMessage");

    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        //var processName = jsonObject.procesS_NAME_JP;
        if ($("#hdfCultureName").val() == "en") // > -1 means substring is present
        {
            processName = jsonObject.procesS_NAME_EN;
        }
        else {
            processName = jsonObject.procesS_NAME_JP;
        }

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

function AddProdPointDataRow(srNo, processName, prodPoint) {
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

function EnableDataPanel(jsonResult) {

    var $startDataPanel = $("#divProcessStartData");

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
}

function SetStartDataPanelDefaultData(jsonResult) {
    $("#btnDateTime").click();

    SetSysDateTime();
}
function IsProcessPlanExist(chuuban, gyouban) {
    var returnVal = true;
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
        url: APIURL + "/Hardness/ValidateChubban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData

    }

    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            debugger;
            if (jsonResult.responce == 2) {

                // Chuuban not found
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_002").text();
                ShowMessageDialog(errorMessage, "txtChuuban1", WARNING);
                returnVal = false;

            } else if (jsonResult.responce == 3) {


                // process plan not started in system
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_011").text().replace("99-999999-99", jsonResult.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(errorMessage, "txtChuuban1", WARNING);
                returnVal = false;

            } else if (jsonResult.responce == 4) {


                // Chuuban is marked as completed in the system
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban1", WARNING);
                returnVal = false;

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
        url: APIURL + "/Hardness/GetFICombinedDecisionDetails",
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

function GetHardnessDetailsByChuuban(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetHardnessDetailsByChuuban?chuuban=" + chuuban + "&gyouban=" + gyouban + "&CULTURE_NAME=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }

    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            ClearData();

            if (jsonResult === null) {

                $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
                $("#chkIncludesCenterInAvg").prop('checked', true);
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_002").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;

            } else if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {

                $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
                // Chuuban is marked as completed in the system
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false; F
            } else if (jsonResult.procesS_PLAN_START_DATE == null) {

                $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
                // process plan not started in system
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_011").text().replace("99-999999-99", jsonResult.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            } else {
                GetThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
                $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
                DisableSearchPanel();
                SetChuubanDetails(jsonResult);
                EnableDataPanel();
               // SetOperatorDropdown($("#ddlOperatorName"), 0);

                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndOperator").focus();

            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function GetThabaDetailsByChuuban(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonThabaDetailData = response.message;
            debugger;
            SetThabaDetails(jsonThabaDetailData);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetThabaDetails(jsonResult) {

    // First, clear thaba details
    //$("[id$='lblDecisionNo']").html("");

    $("#lblDecisionNo").html("");

    // Then, set thaba details
    $.each(jsonResult, function (key, jsonObj) {

        if (jsonObj.tabA_NO) {
            var thabaNumbers = jsonObj.tabA_NO.split(",");
            var decision = jsonObj.decision;
            var decision_value = jsonObj.decisioN_VALUE;

            $.each(thabaNumbers, function (index, thabaNo) {
                thabaNo = thabaNo.trim();
                registeredThabaNumbersAndDecision["" + thabaNo] = decision;


                var decisionLegend = $(decisionLegends[decision_value]);
                $("#lblThabaNo" + thabaNo).text(thabaNo);
                $("#lblDecisionNo" + thabaNo).html("").append(decisionLegend);
            });
        }
    });


}

function SetDecimalFormats() {
    $("#txtActDim").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtCenterRef, #txtThaba1, #txtThaba2, #txtThaba3,#txtThaba4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 1);
    });

    $("#txtD4Avg").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
    $("#txtD1, #txtD2, #txtD3, #txtD4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 1);
    });
}

function CheckDate(field) {

    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
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

function CheckTime(field) {
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

function IsMandatoryFieldsFilled() {

    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {

        if (firstFieldID == 'txtcheckD4') {
            if ($("#txtD1").val().trim() == "") {
                firstFieldID = "txtD1";
            } else if ($("#txtD2").val().trim() == "") {
                firstFieldID = "txtD2";
            } else if ($("#txtD3").val().trim() == "") {
                firstFieldID = "txtD3";
            } else if ($("#txtD4").val().trim() == "") {
                firstFieldID = "txtD4";
            }
        }

        ShowMessageDialog(message, firstFieldID, WARNING);
    }
    return returnVal;
}

function IsDuplicateThabaNo() {
    debugger;
    var inputThaba1 = $("#txtSplitNo").val().trim();

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";
    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK) {
            firstFieldID = "txtSplitNo";
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_009").text().trim().replace("NNN", inputThaba1);
            ShowMessageDialog(message, "txtSplitNo", WARNING);
            return true;
        }
    }

    return false;
}

//Save Data
function SaveDetails($callFrom) {
    debugger;
  
    var ajaxTime = new Date().getDate();
    var jsonDataForSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban1").val() ? $("#txtChuuban1").val() : null,
        PP_GYOUBAN: $("#txtGyouban1").val() ? $("#txtGyouban1").val() : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        DETAIL_NO: $("#txtSplitNo").val() ? $("#txtSplitNo").val() : null,
        HD_TEST_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null, 
        HD_TEST_DATE: ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        //HD_FURNACE_NO: ($("#txtFurnaceNum").val()).toUpperCase() ? $("#txtFurnaceNum").val() : null,
        HD_FURNACE_NO: ($("#txtFurnaceNum").val() || "").toUpperCase() || null,
        IS_ACT_DIM_IN_RANGE: ($('#chkActualDim').is(':checked') ? 1 : 0),
        HD_ACT_DIM: $("#txtActDim").val() ? $("#txtActDim").val() : null,
        HD_PATTERN_ACT: $("#ddlHardnessKubun2").val(),
        HD_SUTEOSHI: $("#txtCenterRef").val() ? $("#txtCenterRef").val() : null,
        HD_D4_1: $("#txtD1").val(),
        HD_D4_2: $("#txtD2").val(),
        HD_D4_3: $("#txtD3").val(),
        HD_D4_4: $("#txtD4").val() ? $("#txtD4").val() : null,
        HD_D4_AVG: $("#txtD4Avg").val(),
        IS_INCLUDE_CENTER_AVG: ($('#chkIncludesCenterInAvg').is(':checked') ? 1 : 0),
        HD_DECISION: $("#ddlDecision").val(),
        //HD_NEXT_PROCESS: $("#ddlnextprocess").val(),
        REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        USER_ID: $("#hdfLoginId").val()


    });
    var jsonData = "{"
        // START DATA

        + "PP_CHUUBAN" + " : '" + $("#txtChuuban1").val() + "',"
        + "PP_GYOUBAN" + " :  '" + $("#txtGyouban1").val() + "',"
        + "PROCESS_PLAN_CD" + " :  '" + $("#hdfldProcessPlanCd").val() + "',"
        + "DETAIL_NO" + " :  '" + $("#txtSplitNo").val() + "',"//
        + "HD_TEST_OPERATOR_CD" + " : '" + $("#ddlOperatorName").val() + "',"//
        + "HD_TEST_DATE" + " : '" + $("#txtDate").val() + "',"
        + "HD_FURNACE_NO" + " : '" + $("#txtFurnaceNum").val() + "',"
        + "IS_ACT_DIM_IN_RANGE" + " : '" + ($('#chkActualDim').is(':checked') ? 1 : 0) + "',"
        + "HD_ACT_DIM" + " : '" + $("#txtActDim").val() + "',"
        + "HD_PATTERN_ACT" + " : '" + $("#ddlHardnessKubun2").val() + "',"
        + "HD_SUTEOSHI" + " : '" + $("#txtCenterRef").val() + "',"
        + "HD_D4_1" + " : '" + $("#txtD1").val() + "',"//
        + "HD_D4_2" + " : '" + $("#txtD2").val() + "',"//
        + "HD_D4_3" + " : '" + $("#txtD3").val() + "',"//
        //+ "HD_D4_4" + " : '" + $("#txtD4").val() + "',"//
        + "HD_D4_AVG" + " : '" + $("#txtD4Avg").val() + "',"//
        + "IS_INCLUDE_CENTER_AVG" + " : '" + ($('#chkIncludesCenterInAvg').is(':checked') ? 1 : 0) + "',"
        + "HD_DECISION" + " : '" + $("#ddlDecision").val() + "',"
        //+ "HD_NEXT_PROCESS" + " : '" + $("#ddlnextprocess").val() + "',"
        + "REMARKS" + " : '" + $("#txtRemarks").val() + "',"
        + "USER_ID" + " : '" + $("#hdfLoginId").val()

        + "USER_NAME" + " : '" + userName
        + "'}";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/SaveHardnessDetail",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonDataForSave + "}"
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            DisableDataPanel();
            GetThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
            DisableSaveButton();
            var successMessage = $("#lblSuccessMessageS_HARDNESS_ENTRY_001").text();
            ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);
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
function UpdateFICombinedDecision() {
    var ajaxTime = new Date().getTime();
    var hdDecision = $("#ddlDecision").val();
    var processPlanCd =  $("#hdfldProcessPlanCd").val(); 
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/UpdateFICombinedDecision?hddecision=" + hdDecision + "&processplancd=" + processPlanCd ,
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
function BindEvents() {

    $("#btnInitialize").click(function (event) {
        Initialize();
        $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
    });

    $btnChuubanSearch.click(function (event) {
        registeredThabaNumbersAndDecision = {};
        ResetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }
        SetSysDateTime();
        if (!IsProcessPlanExist($("#txtChuuban1").val(), $("#txtGyouban1").val())) {
            return false;
        }
        GetFICombinedDecisionDetails($("#txtChuuban1").val(), $("#txtGyouban1").val());
        GetHardnessDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);

        //  getMachineDetails($("#hdfMachineCD").val(), $callFrom);


    });

    $("#btnDateTime").click(function (event) {
        SetSysDateTime();
    });
    $("#txtOperator").change(function () {
        $("#hdfOperatorCD").val($(this).val().trim().replace(/\D/g, ""));

    });

    $("#txtD1").change(function () {

        CalculateD4Avg();
    });
    $("#txtD2").change(function () {

        CalculateD4Avg();
    });

    $("#txtD3").change(function () {

        CalculateD4Avg();
    });
    $("#txtD4").change(function () {

        CalculateD4Avg();
    });

    $("#txtCenterRef").change(function () {

        CalculateD4Avg();
    });

    $btnSave.click(function (event) {
        debugger;
        var isDataSaved = false;
        var actualDim = true;
        ResetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();
       
       
        //var D4Avg = parseFloat($("#txtD4Avg").val());
        //var hardnessValue1 = parseFloat($("#txtHardnessData3").val());
        //$("#txtHardnessData1")
        //$("#txtHardnessData2")
        //$("#txtHardnessData3")
        if (!IsMandatoryFieldsFilled()) {
            return false;
        }

        if (!CheckDate($("#txtDate"))) {
            var message = $("#lblErrorMessageE_HARDNESS_ENTRY_014").text().trim(); // This message will be displayed on dialog box
            ShowMessageDialog(message, "txtDate", WARNING);
            return false;
        }

        //if (!IsDateDifferentFromRange($("#txtDate").val())) {
        //    ShowMessageDialog($("#lblErrorMessageE_HARDNESS_ENTRY_005").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_ENTRY_013").text().trim() + commonDateRange + ")");
        //    return false;
        //}
        if (!IsDateDifferentFromRange(true)) {
            var message = $("#lblErrorMessageE_HARDNESS_ENTRY_005").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_ENTRY_013").text().trim() + commonDateRange + ")";
            var focusFieldID = "txtDate";
            ShowMessageDialog(message, focusFieldID, WARNING);
            return false;
        }
        GetThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
        if (IsDuplicateThabaNo()) {
            return false;
        }
        //Check Furnance no in Tensile table Again this chuuban same or not?
        if ($("#txtFurnaceNum").val() != "") {
            if (!IsDifferentFurnanceNo()) {
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_016").text();
                ShowMessageDialog(errorMessage, "txtFurnaceNum", WARNING);
                return false;
            }
        }
        //if (D4Avg >= hardnessStandardValue) {
        //    debugger;
        //    if ($("#ddlDecision").val() == 10) {
        //        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_015").text().trim(); //Avg
        //        var focusFieldID = "txtD4Avg";
        //        ShowMessageDialog(message, focusFieldID, WARNING);
        //        return false;
        //    }
        //}
        if ($("#chkActualDim").prop("checked") == true) { //$("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK  now removing okdecision condition for all decision type as per windows
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
            var message = $("#lblConfirmationMessageC_HARDNESS_ENTRY_001").text().trim(); //ActDimOutOfTolerance
            var focusFieldID = "txtActDim";
            ShowMessageDialog(message, focusFieldID, WARNING);
            return false;

        }
        
      

        var D4Avg = parseFloat($("#txtD4Avg").val());
        var hardnessPatternValue = $("#txtHardnessData1").val();
        var hardnessValue1 = $("#txtHardnessData2").val();
        var hardnessValue2 = $("#txtHardnessData3").val();
        var txtD1 = $("#txtD1").val();
        var txtD2 = $("#txtD2").val();
        var txtD3 = $("#txtD3").val();
        var txtD4 = $("#txtD4").val();

        if (hardnessPatternValue.length > 0) {

            let hdPatternValue = String(hardnessPatternValue || "");
            let hardness2Value = String(hardnessValue1 || "");
            let hardness3Value = String(hardnessValue2 || "");

            let validMinVal = null;
            let validMaxVal = null;

            // If hdPatternValue is not empty
            if (hdPatternValue !== "") {

                // If HARDNESS_1 != JITSU
                // if (hdPatternValue !== DAO.CONSTANTS.JITSU) {

                // Check if HARDNESS_2 is numeric
                if (!isNaN(Number(hardness2Value))) {
                    validMinVal = Number(hardness2Value);
                    validMaxVal = Number(hardness3Value);
                }

                // HARDNESS_2 = MIN
                if (hardness2Value.includes("MIN")) {
                    validMinVal = Number(hardness3Value);
                }

                // HARDNESS_2 = MAX
                if (hardness2Value.includes("MAX")) {
                    validMaxVal = Number(hardness3Value);
                }
                // }
            }

            let avgVal = Number(D4Avg);

            let isOutOfRangeFlag = false;

            // Check AVG < MIN
            if (!isNaN(validMinVal) && validMinVal != null) {
                //50            60
                if (avgVal <= validMinVal) {
                    isOutOfRangeFlag = true;
                }
            }

            // Check AVG > MAX
            if (!isNaN(validMaxVal) && validMaxVal != null) {

                //100   150      
                if (avgVal >= validMaxVal) {
                    isOutOfRangeFlag = true;
                }
            }
            if (isOutOfRangeFlag == true) {

                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#lblConfirmationMessageC_HARDNESS_ENTRY_002").text().trim() + "</td></tr></table>",
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
                            SetHardnessStdValue();
                            var result = CheckHardnessStdValue(txtD1, txtD2, txtD3, txtD4, validMinVal, validMaxVal);
                            if (result == true) {
                                if (fICombinedDecisions.length > 0) {
                                    var hardnessDecision = $("#ddlDecision").val();
                                    for (i = 0; i < fICombinedDecisions.length; i++) {
                                        if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                                            if (hardnessDecision == FIResultOk) {
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
                                            else if (hardnessDecision != FIResultOk) {
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
                                            if (hardnessDecision != FIResultOk) {
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
                            }

                        }
                        else {
                            bootbox.hideAll();
                            $("#txtD4Avg").focus();
                        }


                    },
                    centerVertical: true
                });
                debugger;
            }
            else {
                debugger;
                SetHardnessStdValue();
                var result = CheckHardnessStdValue(txtD1, txtD2, txtD3, txtD4, validMinVal, validMaxVal);
                if (result == true) {
                    if (fICombinedDecisions.length > 0) {
                        var hardnessDecision = $("#ddlDecision").val();
                        for (i = 0; i < fICombinedDecisions.length; i++) {
                            if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                                if (hardnessDecision == FIResultOk) {
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
                                else if (hardnessDecision != FIResultOk) {
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
                                if (hardnessDecision != FIResultOk) {
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
                }

            }
        }
        else {
            if (fICombinedDecisions.length > 0) {
                var hardnessDecision = $("#ddlDecision").val();
                for (i = 0; i < fICombinedDecisions.length; i++) {
                   
                    if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                        if (hardnessDecision == FIResultOk) {
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
                        else if (hardnessDecision != FIResultOk) {
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
                        if (hardnessDecision != FIResultOk) {
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
        }
    });

}

function SetHardnessStdValue() {
    var hardnessPatternValue = $("#txtHardnessData1").val();
    var hardnessValue1 = $("#txtHardnessData2").val();
    var hardnessValue2 = $("#txtHardnessData3").val();
    const minVal = hardnessValue1;
    const maxVal = hardnessValue2;

    if (minVal.includes("MIN") && maxVal) {
        m_hardnessStd = "MIN" + maxVal;

    } else if (minVal.includes("MAX") && maxVal) {
        m_hardnessStd = "MAX" + maxVal;

    } else if (minVal && maxVal) {
        m_hardnessStd = minVal + "-" + maxVal;

    } else {
        m_hardnessStd = "";
    }
}

function CheckHardnessStdValue(txtD1, txtD2, txtD3, txtD4, validMinVal, validMaxVal) {
    var txtD1 = txtD1;
    var txtD2 = txtD2;
    var txtD3 = txtD3;
    var txtD4 = txtD4;
    var validMinVal = validMinVal;
    var validMaxVal = validMaxVal;
    var returnVal = true;
        if (m_hardnessStd != "" && $("#ddlDecision").val() == 10) {
            if (CheckHardnessPatterValidation(m_hardnessStd, txtD1, validMinVal, validMaxVal)) {
                returnval = false;
                var message = $("#lblErrorMessageE_HARDNESS_ENTRY_015").text().trim();
                showMessageDialog(message, "", WARNING);
                //returnVal = true;
                return false;
            }
            else {
                returnval = true;
            }
            if (CheckHardnessPatterValidation(m_hardnessStd, txtD2, validMinVal, validMaxVal)) {
                returnval = false;
                var message = $("#lblErrorMessageE_HARDNESS_ENTRY_015").text().trim();
                showMessageDialog(message, "", WARNING);
                //returnVal = true;
                return false;
            }
            else {
                returnval = true;
            }
            if (CheckHardnessPatterValidation(m_hardnessStd, txtD3, validMinVal, validMaxVal)) {
                returnval = false;
                var message = $("#lblErrorMessageE_HARDNESS_ENTRY_015").text().trim();
                showMessageDialog(message, "", WARNING);
                //returnVal = true;
                return false;
            }
            else {
                returnval = true;
            }
            if (CheckHardnessPatterValidation(m_hardnessStd, txtD4, validMinVal, validMaxVal)) {
                returnval = false;
                var message = $("#lblErrorMessageE_HARDNESS_ENTRY_015").text().trim();
                showMessageDialog(message, "", WARNING);
                //returnVal = true;
                return false;
            }
            else {
                returnval = true;
            }
    }
    return returnVal;
    }
    function CheckHardnessPatterValidation(stdPatternValue, calVal, hardnessMinVal, hardnessMaxVal) {
        var returnval = false;

        if (stdPatternValue && stdPatternValue.trim() !== "" && calVal != "") {

            const pattern = stdPatternValue.trim().substring(0, 3).toUpperCase();
            let stdMinLimitVal, stdMaxLimitVal, calculateVal;

            if (pattern.includes("MIN")) {

                stdMinLimitVal = parseInt(hardnessMinVal, 10);
                calculateVal = parseFloat(calVal);

                if (calculateVal < stdMinLimitVal) {
                    returnval = true;  // validation error
                } else {
                    returnval = false; // no error
                }

            } else if (pattern.includes("MAX")) {

                stdMaxLimitVal = parseInt(hardnessMaxVal, 10);
                calculateVal = parseFloat(calVal);

                if (calculateVal > stdMaxLimitVal) {
                    returnval = true;  // validation error
                } else {
                    returnval = false; // no error
                }

            } else if (stdPatternValue.includes("-")) {

                stdMinLimitVal = parseInt(hardnessMinVal, 10);
                stdMaxLimitVal = parseInt(hardnessMaxVal, 10);
                calculateVal = parseFloat(calVal);

                if (calculateVal < stdMinLimitVal || calculateVal > stdMaxLimitVal) {
                    returnval = true;  // validation error
                } else {
                    returnval = false; // no error
                }

            } else {
                returnval = false; // no validation error
            }
        }

        return returnval; // default
    }

    function IsDifferentFurnanceNo() {

        var chuuban = $("#txtChuuban1").val();
        var gyouban = $("#txtGyouban1").val();
        var splitNo = $("#txtSplitNo").val();

        var returnval = false;

        // $callFrom.data('requestRunning', true);
        var ajaxTime = new Date().getTime();
        var jsonData = "{PP_CHUUBAN :'" + chuuban + "', HD_FURNACE_NO : '" + $("#txtFurnaceNum").val() + "'  , PP_GYOUBAN : '" + gyouban + "', USER_NAME: '" + userName + "' }";

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Hardness/GetFurnanceNo",
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
                    if (response.message.tS_FURNACE_NO != null) {
                        var txtFurnance_No = ($("#txtFurnaceNum").val()).toUpperCase();
                        var TS_Furnace_No = jsonResult.tS_FURNACE_NO;
                        if (TS_Furnace_No != txtFurnance_No) {
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
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
        return returnval;
    }
function IsDateDifferentFromRange(boolVal) {

        var returnval = false;
        var getDate;
        if (boolVal === true) {
            getDate = ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

        }


        var ajaxTime = new Date().getTime();

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Hardness/IsValidDateRange",
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
                var message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_001").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
            });




        return returnval;
    }

    $(document).ready(function () {

        $('#tdLblMachineType').hide();
        $('#tdTxtMachineType').hide();
        $('#tdLblChuuban').hide();
        $('#tdtxtNifudaNo1').hide();
        $('#tdTxtNifudaNo').hide();
        $('#TbHistorySearch-tab').hide();
        $('#lblLengthMsgT1').hide();
        $('#lblLengthToleranceMsgT1').hide();

        $.support.cors = true; // Special case for IE7. To get past weird errors.
        // window.moveTo(0, 0);
        window.resize(1215, 800);
        SetGlobalVariables();
        DisableDataPanel();

        BindEvents();
        SetDecimalFormats();
        SetDecisionList();
        SetHardnessKubuns();

        SetNextProcess();
        $("#txtChuuban1").focus();
        $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);

    });

    //function IsDateDifferentFromRange(dateVal) {

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

    $("#chkIncludesCenterInAvg").click(function () {
        CalculateD4Avg();
        if ($("#chkIncludesCenterInAvg").prop("checked") == true) {
            $("#txtCenterRef").addClass("Mandatory");
        } else
        {
            $("#txtCenterRef").removeClass("Mandatory");
        }
    });

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
                url: APIURL + "/Hardness/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                    message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
                    ShowMessageDialog(message, focusFieldID, ERROR);
                });
        }

    });
    $("#ddlDecision").focusout(function (event) {
        $("#btnSave").focus();
    });