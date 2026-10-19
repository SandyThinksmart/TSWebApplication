var $btnChuubanSearch;
var $btnSave;
var $btnSearchT2;
var indexDDLDecisionNothing = 0;
var combinedDecisionOK = "";
var dbOperation; // Value will set to "Insert" or "Update" at runtime
var $hdfSYSDATE;
var $hdfCultureLoaded;
var $hdfCultureEN;
var $hdfCultureJP;
var hikinukiCD;
var registeredThabaNumbersAndDecision = {};
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var maxRegisteredThabaNo = 0;
let enterPressedMachine = false;
let enterPressedStartOpr = false;
let enterPressedEndOpr = false;
var machineModified = false;
var operatorStartModified = false;
var operatorEndModified = false;

var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var DISABLED = "disabled";
var HIDDEN = "Hidden"
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
// Error log information. To be sent with each AJAX request. 
userName = ""; // assigned at Document.ready
// End error log information.

var machineGridDataTr1 = " <tr id='tr1ItemGrdvHikinukiMachine'> "
    + " <td class='td tdH2' rowspan='2' style='background-color:white;padding-left:1px;'> "
    + " <span class='txtColor' style='background-color:COLOR; height:45px; width:100%; display: inline-block;'></span>"
    + " </td>"
    //+ " </td>"
    + " <td class='td tdH2 GridAlignmentCustomer' rowspan='2' >"
    + " <span class='txtGridCustomerName labelClass padding24Machine'> valtxtGridCustomerName</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentChuuban'  style='padding-right: 0px; padding-left: 0px;'>"
    + " <span class='txtGridChuban labelClass padding24Machine'>valtxtGridChuban</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentProductNameCust' rowspan='2' >"
    + " <span class='txtProdNameCust labelClass padding24Machine' >valtxtProdNameCust</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentMetallicPlateCode'>"
    + " <span class='txtMetallicPlateCode labelClass padding24Machine' >valtxtMetallicPlateCode</span></td>"
    + " <td class='td tdH2 GridAlignmentDimD ' rowspan='2' id='tdGridMaterialDimD'>"
    + " <span class='txtMaterialDimD labelClass padding24Machine' >valtxtMaterialDimD</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNoOfUnitsWithUnit ' rowspan='2'>"
    + " <span class='txtGridNoOfCoils labelClass padding24Machine' >valtxtGridNoOfCoils</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentQuantity' rowspan='2'>"
    + " <span class='txtGridQuantity labelClass padding24Machine' >valtxtGridQuantity</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentActualDim'>"
    + " <span class='txtGridDimDShape labelClass padding24Machine' >valtxtGridDimDShape</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentDieDim' rowspan='2'>"
    + " <span class='txtGridDieDim labelClass padding24Machine'>valtxtGridDieDim</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentTabaNo' style='text-align: center;' rowspan='2'>"
    + "  <span class='txtGridTabaNo labelClass padding24Machine' >valtxtGridTabaNo</span> <br>"
    + "  <span class='txtGridTabaNo3 labelClass padding24Machine' >valtxtGridTabaNo3</span> "
    + " </td>"
    + " <td class='td tdH2 GridAlignmentActualDim123' style='min-width:45px;'>"
    + " <span class='txtActAllocDim1 labelClass padding24Machine' style='color:Black;'>valtxtActAllocDim1</span></td>"
    + " <td class='td tdH2 GridAlignmentActualDim123' style='min-width:45px;'>"
    + "  <span class='txtActAllocDim2 labelClass padding24Machine' class='labelClass' style='color:Black;'>valtxtActAllocDim2</span></td>"
    + " <td class='td tdH2 GridAlignmentActualDim123' style='min-width:45px;'>"
    + " <span class='txtActAllocDim3 labelClass padding24Machine' class='labelClass' style='color:Black;'>valtxtActAllocDim3</span></td>"
    + " <td class='td tdH2 GridAlignmentActualLength123' style='min-width:45px;'>"
    + " <span class='txtActAllocLength1 labelClass padding24Machine' style='color:Black;'>valtxtActAllocLength1</span></td>"
    + " <td class='td tdH2 GridAlignmentActualLength123' style='min-width:45px;'>"
    + " <span class='txtActAllocLength2 labelClass padding24Machine'  style='color:Black;'>valtxtActAllocLength2</span></td>"
    + " <td class='td tdH2 GridAlignmentActualLength123' style='min-width:45px;'>"
    + " <span class='txtActAllocLength3 labelClass padding24Machine' class='labelClass' style='color:Black;'>valtxtActAllocLength3</span></td>"
    + " <td class='td tdH2 GridAlignmentFinishedBarCount'>"
    + " <span class='txtGridFinishedBars labelClass padding24Machine' class='labelClass' style='color:Black;'>valtxtGridFinishedBars</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentFinishedQuantity'>"
    + " <span class='txtGridFinishedQty labelClass padding24Machine' class='labelClass'>valtxtGridFinishedQty</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentBendStatus'>"
    + "  <span class='txtGridBendStatus labelClass padding24Machine' class='labelClass'>valtxtGridBendStatus</span>"
    + " </td>"
    + "  <td class='td tdH2 GridAlignmentExternalSurfaceCondition'>"
    + " <span class='txtGridExtSurf labelClass padding24Machine' class='labelClass'>valtxtGridExtSurf</span>"
    + "  </td>"
    + "  </tr>";


var machineGridDataTr2 = "<tr id='tr2ItemGrdvHikinukiMachine'>"
    + "  <td class='td tdH2 GridAlignmentProdSpecNo'> "
    + "<span class='txtGridSpecification labelClass padding24Machine' >valtxtGridSpecification</span> "
    + "</td> "
    + "<td class='td tdH2 GridAlignmentCharge'> "
    + "<span class='txtGridCharge labelClass padding24Machine' >valtxtGridCharge</span> "
    + "</td> "
    + "<td class='td tdH2 GridAlignmentLength'> "
    + "<span class='txtGridLength labelClass padding24Machine' >valtxtGridLength</span> "
    + "</td> "
    
    + "<td class='td tdH2 GridAlignmentActualDim123' id='tdActAllocDim4'> "
    + "<span class='txtActAllocDim4 labelClass padding24Machine'  style='color:Black;'>valtxtActAllocDim4</span></td> "

    + "<td class='td tdH2 GridAlignmentActualDim123' id='tdActAllocDim5'> "
    + "<span class='txtActAllocDim5 labelClass padding24Machine'  style='color:Black;'>valtxtActAllocDim5</span></td> "
    + "<td class='td tdH2 GridAlignmentActualDim123' id='tdActAllocDim6'> "
    + "<span class='txtActAllocDim6 labelClass padding24Machine'  style='color:Black;'>valtxtActAllocDim6</span></td> "
    + "<td class='td tdH2 GridAlignmentRemarks' colspan='4' > "
    + "<span class='txtGridMsgContent labelClass padding24Machine' >valtxtGridMsgContent</span> "
    + "</td> "
    + "<td class='td tdH2 GridAlignmentNumeric' > "
    + "<span class='txtGridMagnetism labelClass padding24Machine' >valtxtGridMagnetism</span> "
    + " </td> "
    + "<td class='td tdH2 GridAlignmentNumeric'> "
    + "<span class='txtGridShikiiValue labelClass padding24Machine' >valtxtGridShikiiValue</span> "
    + " </td> "
    + "<td class='td tdH2 GridAlignmentNumeric'> "
    + "<span class='txtGridEcNgBarTotal labelClass padding24Machine' >valtxtGridEcNgBarTotal</span></td> "
    + " </tr> ";



var processHistoryGridataRow1 = " <tr id='tr1ItemGrdvProcessHistory'> "
    + " <td class= 'td GridLeft'  > "
    + " <span id= 'lblLocationName' class= 'labelClass' style= 'color:Black; '>valtxtLocationName</span> "
    + " </td> "
    + " <td class= 'td GridCenter'  > "
    + " <span id= 'lblStartDate' class= 'labelClass' style= 'color:Black; '>valtxtStartDate</span> "
    + " </td> "
    + " <td class= 'td GridCenter'  > "
    + " <span id= 'lblEndDate' class= 'labelClass' style= 'color:Black; '>valtxtEndDate</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='4' > "
    + " <span id= 'lblCustomerName' class= 'labelClass' style= 'color:Black; '>valtxtCustomerName</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='2' > "
    + " <span id= 'lblChuuban' class= 'labelClass' style= 'color:Black; '>valtxtChuuban</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='4'> "
    + " <span id= 'lblProdNameCust' class= 'labelClass' style= 'color:Black; '>valtxtProdNameCust</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='2'> "
    + " <span id= 'lblMetalicPlate' class= 'labelClass' style= 'color:Black; '>valtxtMetalicPlate</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='4'> "
    + " <span id= 'lblMatlDim_Shape' class= 'labelClass' style= 'color:Black; '>valtxtMatlDim_Shape</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='4'> "
    + " <span id= 'lblNoOfUnits' class= 'labelClass' style= 'color:Black; '>valtxtNoOfUnits</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='4'> "
    + " <span id= 'lblActAllocQty' class= 'labelClass' style= 'color:Black; '>valtxtActAllocQty</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + " <span id= 'lblProdDim_Shape' class= 'labelClass' style= 'color:Black; '>valtxtProdDim_Shape</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='4'> "
    + " <span id= 'lblDieDim' class= 'labelClass' style= 'color:Black; '>valtxtDieDim</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='4'> "
    + " <span id= 'lblThabaNo' class= 'labelClass' style= 'color:Black; '>valtxtThabaNo</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblActDim1' class= 'labelClass' style= 'color:Black; '>valtxtActDim1</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblActDim2' class= 'labelClass' style= 'color:Black; '>valtxtActDim2</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblActDim3' class= 'labelClass' style= 'color:Black; '>valtxtActDim3</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + " <span id= 'lblActLength1' class= 'labelClass' style= 'color:Black; '>valtxtActLength1</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + " <span id= 'lblActLength2' class= 'labelClass' style= 'color:Black; '>valtxtActLength2</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + " <span id= 'lblActLength3' class= 'labelClass' style= 'color:Black; '>valtxtActLength3</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + " <span id= 'lblFinishedBars' class= 'labelClass' style= 'color:Black; '>valtxtFinishedBars</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + " <span id= 'lblFinishedQty' class= 'labelClass' style= 'color:Black; '>valtxtFinishedQty</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='2'> "
    + " <span id= 'lblBendStatus' class= 'labelClass' style= 'color:Black; '>valtxtBendStatus</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='2'> "
    + " <span id= 'lblExtTexture' class= 'labelClass' style= 'color:Black; '>valtxtExtTexture</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='4'> "
    + " <span id= 'lblDecision' class= 'labelClass' style= 'color:Black; '>valtxtDecision</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='4'> "
    + " <span id= 'lblNextProcess' class= 'labelClass' style= 'color:Black; '>valtxtNextProcess</span> "
    + " </td> "
    + " </tr>  ";


var processHistoryGridataRow2 = " <tr id='tr2ItemGrdvProcessHistory'> "
    + " <td class= 'td GridLeft' rowspan='3' colspan='2'> "
    + "     <span id= 'lblMachineName' class= 'labelClass' style= 'color:Black; '>valtxtMachineName</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='3'> "
    + "     <span id= 'lblEndOperatoName' class= 'labelClass' style= 'color:Black; '>valtxtEndOperatorName</span> "
    + " </td> "
    
    + " </tr> ";


var processHistoryGridataRow3 = " <tr id='tr3ItemGrdvProcessHistory'> "
    + " <td class= 'td GridCenter' rowspan='2'> "
    + "     <span id= 'lblProdSpecNo' class= 'labelClass' style= 'color:Black; '>valtxtProdSpecNo</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='2'> "
    + "     <span id= 'lblCharge' class= 'labelClass' style= 'color:Black; '>valtxtCharge</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblProductLength' class= 'labelClass' style= 'color:Black; '>valtxtProductLength</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblActDim4' class= 'labelClass' style= 'color:Black; '>valtxtActDim4</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblActDim5' class= 'labelClass' style= 'color:Black; '>valtxtActDim5</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblActDim6' class= 'labelClass' style= 'color:Black; '>valtxtActDim6</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='2' colspan='4'> "
    + "     <span id= 'lblNGDetails' class= 'labelClass' style= 'color:Black; '>valtxtNGDetails</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblMagnetism' class= 'labelClass' style= 'color:Black; '>valtxtMagnetism</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblShikiiValue' class= 'labelClass' style= 'color:Black; '>valtxtShikiiValue</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='2'> "
    + "     <span id= 'lblNGBarTotal' class= 'labelClass' style= 'color:Black; '>valtxtNGBarTotal</span> "
    + " </td> "
    + "  </tr> ";

var processHistoryGridataRow4 = " <tr id='tr3ItemGrdvProcessHistory'> "
   
    + "  </tr> ";


function SetGlobalVariables() {
    $btnChuubanSearch = $("#btnChuubanSearch");
    $btnSave = $("#btnSave");
    $btnSearchT2 = $("#btnSearchT2");
    $hdfSYSDATE = $("#hdfSYSDATE");
    $hdfCultureLoaded = $("#hdfCultureLoaded");
    $hdfCultureEN = $("#hdfCultureEN");
    $hdfCultureJP = $("#hdfCultureJP");
}

function ResetGlobalVariables() {
    $("#hdfDBOperation").val("");
    registeredThabaNumbersAndDecision = {};
    dimTolMin = "";
    dimTolMax = "";
    dimStd = "";
    lenTolMin = "";
    lenTolMax = "";
    lenStd = "";
    txtShikii = "";
    txtInspectionCoilSize = "";
    txtInspectionCoilShape = "";
    txtIsOK = "";
    maxRegisteredThabaNo = 0;
}

function SetDecimalFormats() {
    $("#txtAEdgeDistortion1, #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtBEdgeDistortion1, #txtBEdgeDistortion2, #txtBEdgeDistortion3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 1, 1);
    });

    $("#txtUpperLowerRollerAngle1, #txtUpperLowerRollerAngle2, #txtCompressionTonnage, #txtHexR").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });
    $("#txtMn").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
    $("#txtC, #txtUpperRoleElevation, #txtInspectionCoilSize, #txtMagnetism").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtDieDim, #txtDrawDim1, #txtDrawDim2, #txtActualDim1, #txtActualDim2, #txtActualDim3, #txtActualDim4, #txtActualDim5, #txtActualDim6").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    $("#txtActualLength1, #txtActualLength2, #txtActualLength3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
}
$("#txtNGQuantity1").keyup(function (event) {
    DdlNGReasonMandatory($("#txtNGQuantity1"), $("#ddlNGReason1"));
    if ($("#chkECOnline").prop("checked") == true)
    {
        CalculateBarCount();
    }
   
});
$("#txtNGQuantity2").keyup(function (event) {
    DdlNGReasonMandatory($("#txtNGQuantity2"), $("#ddlNGReason2"));
    if ($("#chkECOnline").prop("checked") == true)
    {
        CalculateBarCount();
    }
});
$("#txtNGQuantity3").keyup(function (event) {
    DdlNGReasonMandatory($("#txtNGQuantity3"), $("#ddlNGReason3"));
    if ($("#chkECOnline").prop("checked") == true)
    {
        CalculateBarCount();
    }
    
});
function CalculateBarCount() {
    var bar1 = 0;
    var bar2 = 0;
    var bar3 = 0;
    if ($("#txtNGQuantity1").val() != "") {
        bar1 = $("#txtNGQuantity1").val();
    }
    if ($("#txtNGQuantity2").val() != "") {
        bar2 = $("#txtNGQuantity2").val();
    }
    if ($("#txtNGQuantity3").val() != "") {
        bar3 = $("#txtNGQuantity3").val();
    }
    var totalValue = parseInt(bar1) + parseInt(bar2) + parseInt(bar3);
    if (totalValue > 0) {
        $("#txtECNGBars").val(totalValue);
      //  calculateDefectRate();

    } else {
        $("#txtECNGBars").val("");
      //  $("#txtDefectRate").val("");
    }
}
function DdlNGReasonMandatory($txtTarget, $ddlTarget) {

    if ($txtTarget.val() > 0) {
        $ddlTarget.removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
        $ddlTarget.addClass("Mandatory");

    } else {
        $ddlTarget.attr(DISABLED, DISABLED).addClass(DISABLED);
        $ddlTarget.removeClass("Mandatory");
        $ddlTarget.val(0);
    }
}

function DisableDefaultField() {
    $("#ddlNGReason1").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#ddlNGReason2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#ddlNGReason3").attr(DISABLED, DISABLED).addClass(DISABLED);
   // $("#txtECNGBars").attr(DISABLED, DISABLED).addClass(DISABLED); 
    $("#txtCoilQuantity").attr(DISABLED, DISABLED).addClass(DISABLED); 
    $("#txtNoOfCoils").attr(DISABLED, DISABLED).addClass(DISABLED); 
    
}
function HideToleranceErrorLabels() {
    $("#lblDimToleranceMsgT1").hide();
    $("#lblLengthMsgT1").hide();
    $("#lblLengthToleranceMsgT1").hide();
    $("#lblDimToleranceMsgT4").hide();
    $("#lblLengthMsgT4").hide();
    $("#lblLengthToleranceMsgT4").hide();
}
function ClearData() {
    EnableSearchOrderNoPanel();
    combinedDecisionOK = "";
    ClearThabaDetailsGrid();
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));

    ClearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    ResetMessageDetailsGrid();
    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    ResetProdPointGrid();
    ClearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
    ResetHistoryGrid();

    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableSaveButton();
   
    $("#txtFinalRegisteredThabaNoValue").text("0");
}
function Initialize() {
    EnableSearchOrderNoPanel();
    combinedDecisionOK = "";
    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    EnableSaveButton();
    EnableSearchPanel();
    $(".FocusDefault").focus();
    DisableStartDataPanel();
    DisableEndDataPanel();
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    ClearPanel($("#pnlCustomerDetails")); // Tab 2
    ClearPanel($("#pnlProductDimensionDetails")); // Tab 2
    ClearPanel($("#divProcessPlanDetails")); // Tab 2
    ClearPanel($("#divMaterialDetails")); // Tab 2
    ClearPanel($("#pnlProcessInfo")); // Tab 4
    ClearPanel($("#pnlTestSpecifications")); // Tab 4    
    ClearThabaDetailsGrid();
    //clearTable($("#grdvHikinukiMachine"), "ItemGrdvHikinukiMachine", "");     // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    ClearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    ClearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2

    $("#txtNifudaNo").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $("#txtMachineType").val("");
    $("#txtFinalRegisteredThabaNoValue").text("0"); 
    HideToleranceErrorLabels();

    ResetGlobalVariables();
    ResetMessageDetailsGrid();
    ResetHistoryGrid();
    ResetProdPointGrid();
    Reset24MachineGrid();
    $("#txtMachineType").focus();
}

function ResetResponseTime() {
    responseTime = ""; // Clear server response time
}

function SetThabaDetails(jsonResult) {
    maxRegisteredThabaNo = 0;
    // First, clear thaba details
    $("[id$='lblDecisionNo']").html("");

    // Then, set thaba details
    $.each(jsonResult, function (index, jsonObject) {

        if (jsonObject.tabA_NO) {
            var thabaNumbers = jsonObject.tabA_NO.split(",");
            var decision = jsonObject.decision;
            var decision_value = jsonObject.decisioN_VALUE;

            $.each(thabaNumbers, function (index, thabaNo) {
                thabaNo = thabaNo.trim();
                registeredThabaNumbersAndDecision["" + thabaNo] = decision;

                if (parseInt(thabaNo) >= parseInt(maxRegisteredThabaNo)) {
                    maxRegisteredThabaNo = parseInt(thabaNo);
                }

                var decisionLegend = $(decisionLegends[decision_value]);
                $("#lblThabaNo" + thabaNo).text(thabaNo);
                $("#lblDecisionNo" + thabaNo).html("").append(decisionLegend);
            });
        }
    });
    $("#txtFinalRegisteredThabaNoValue").text(maxRegisteredThabaNo);
}

function IsEndDateValid() {

    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_007").text().trim();
        var focusFieldID = "txtEndDate";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_009").text().trim();
        var focusFieldID = "txtEndDate";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
}
function Set24HrMachineData(jsonResult) {
    ClearTable($("#grdvHikinukiMachine"), "ItemGrdvHikinukiMachine", "");
    ClearTable($("#grdvHikinukiMachine"), "", "GridNormalRow");
    $.each(jsonResult, function (index, jsonObject) {
        if (jsonObject.dW_MACHINE_CD) {
            var firstRow = machineGridDataTr1;
            if (jsonObject.dW_DECISION != null) {
                if (jsonObject.dW_DECISION == 1) {
                    firstRow = firstRow.replace("COLOR", "green");    // $("●").css('color', 'green');             
                    firstRow = firstRow.replace("#E2E2E2", "green");
                }
                else if (jsonObject.dW_DECISION == 2) {
                    firstRow = firstRow.replace("COLOR", "red");    //firstRow = $("●").css('color', 'red');
                    firstRow = firstRow.replace("#E2E2E2", "red");
                }
                else if (jsonObject.dW_DECISION == 3) {
                    firstRow = firstRow.replace("COLOR", "blue");    //firstRow = $("●").css('color', 'blue');
                    firstRow = firstRow.replace("#E2E2E2", "blue");
                }
                else if (jsonObject.dW_DECISION == 4) {
                    firstRow = firstRow.replace("COLOR", "Yellow");  //firstRow = $("●").css('color', 'Yellow');
                    firstRow = firstRow.replace("#E2E2E2", "Yellow");
                } else if (jsonObject.dW_DECISION == 5) {
                    firstRow = firstRow.replace("COLOR", "darkred");  //firstRow = $("●").css('color', 'darkred');
                    firstRow = firstRow.replace("#E2E2E2", "darkred");
                }
                else {
                    firstRow = firstRow.replace("COLOR", "White");   // firstRow = $("●").css('color', 'White');
                    firstRow = firstRow.replace("#E2E2E2", "White");
                }
            }


            if (jsonObject.pP_CUSTOMER_NAME != null)
                firstRow = firstRow.replace("valtxtGridCustomerName", jsonObject.pP_CUSTOMER_NAME);
            else
                firstRow = firstRow.replace("valtxtGridCustomerName", "");

            if (jsonObject.pP_CHUUBAN_GYOUBAN != null)
                firstRow = firstRow.replace("valtxtGridChuban", jsonObject.pP_CHUUBAN_GYOUBAN);
            else
                firstRow = firstRow.replace("valtxtGridChuban", "");

            if (jsonObject.pP_PROD_NAME_CUST != null)
                firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
            else
                firstRow = firstRow.replace("valtxtProdNameCust", "");

            if (jsonObject.dW_METALLIC_PLATE_CODE != null)
                firstRow = firstRow.replace("valtxtMetallicPlateCode", jsonObject.dW_METALLIC_PLATE_CODE);
            else
                firstRow = firstRow.replace("valtxtMetallicPlateCode", "");

            if (jsonObject.pP_MATL_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtMaterialDimD", jsonObject.pP_MATL_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtMaterialDimD", "");

            if (jsonObject.pP_NO_OF_UNITS_DECODE != null)
                firstRow = firstRow.replace("valtxtGridNoOfCoils", jsonObject.pP_NO_OF_UNITS_DECODE);
            else
                firstRow = firstRow.replace("valtxtGridNoOfCoils", "");

            if (jsonObject.pP_ACT_ALLOC_QTY != null)
                firstRow = firstRow.replace("valtxtGridQuantity", jsonObject.pP_ACT_ALLOC_QTY);
            else
                firstRow = firstRow.replace("valtxtGridQuantity", "");

            if (jsonObject.pP_PROD_DIM_D_STRING != null)
                firstRow = firstRow.replace("valtxtGridDimDShape", jsonObject.pP_PROD_DIM_D_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDimDShape", "");
          
            if (jsonObject.dW_DIE_DIM_STRING != null)
                firstRow = firstRow.replace("valtxtGridDieDim", jsonObject.dW_DIE_DIM_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDieDim", "");

            if (jsonObject.dW_TABA_NO_1_2 != null)
                firstRow = firstRow.replace("valtxtGridTabaNo", jsonObject.dW_TABA_NO_1_2);
            else
                firstRow = firstRow.replace("valtxtGridTabaNo", "");


            if (jsonObject.dW_TABA_NO_2_3 != null)
                firstRow = firstRow.replace("valtxtGridTabaNo3", jsonObject.dW_TABA_NO_2_3);
            else
                firstRow = firstRow.replace("valtxtGridTabaNo3", "");

            if (jsonObject.dW_ACT_DIM1 != null)
                firstRow = firstRow.replace("valtxtActAllocDim1", jsonObject.dW_ACT_DIM1.toFixed(3));
            else
                firstRow = firstRow.replace("valtxtActAllocDim1", "");

            if (jsonObject.dW_ACT_DIM2 != null)
                firstRow = firstRow.replace("valtxtActAllocDim2", jsonObject.dW_ACT_DIM2.toFixed(3));
            else
                firstRow = firstRow.replace("valtxtActAllocDim2", "");

            if (jsonObject.dW_ACT_DIM3 != null)
                firstRow = firstRow.replace("valtxtActAllocDim3", jsonObject.dW_ACT_DIM3.toFixed(3));
            else
                firstRow = firstRow.replace("valtxtActAllocDim3", "");

            if (jsonObject.dW_ACT_LENGTH1 != null)
                firstRow = firstRow.replace("valtxtActAllocLength1", jsonObject.dW_ACT_LENGTH1);
            else
                firstRow = firstRow.replace("valtxtActAllocLength1", "");

            if (jsonObject.dW_ACT_LENGTH2 != null)
                firstRow = firstRow.replace("valtxtActAllocLength2", jsonObject.dW_ACT_LENGTH2);
            else
                firstRow = firstRow.replace("valtxtActAllocLength2", "");

            if (jsonObject.dW_ACT_LENGTH3 != null)
                firstRow = firstRow.replace("valtxtActAllocLength3", jsonObject.dW_ACT_LENGTH3);
            else
                firstRow = firstRow.replace("valtxtActAllocLength3", "");

            if (jsonObject.dW_FINISHED_BARS != null)
                firstRow = firstRow.replace("valtxtGridFinishedBars", jsonObject.dW_FINISHED_BARS + jsonObject.dW_UNIT_TYPE_STRING);
            else
                firstRow = firstRow.replace("valtxtGridFinishedBars", "");

            if (jsonObject.dW_FINISHED_QTY != null)
                firstRow = firstRow.replace("valtxtGridFinishedQty", jsonObject.dW_FINISHED_QTY);
            else
                firstRow = firstRow.replace("valtxtGridFinishedQty", "");

            if (jsonObject.dW_BEND_STATUS_DECODE != null)
                firstRow = firstRow.replace("valtxtGridBendStatus", jsonObject.dW_BEND_STATUS_DECODE);
            else
                firstRow = firstRow.replace("valtxtGridBendStatus", "");

            if (jsonObject.dW_EXT_TEXTURE_STATUS_DECODE != null)
                firstRow = firstRow.replace("valtxtGridExtSurf", jsonObject.dW_EXT_TEXTURE_STATUS_DECODE);
            else
                firstRow = firstRow.replace("valtxtGridExtSurf", "");

            $("#grdvHikinukiMachine").append($(firstRow));



            // tr2    (Second row)
           
            var secondtRow = machineGridDataTr2;

            if (jsonObject.proD_SPEC_NO != null)
                secondtRow = secondtRow.replace("valtxtGridSpecification", jsonObject.proD_SPEC_NO);
            else
                secondtRow = secondtRow.replace("valtxtGridSpecification", "");

            if (jsonObject.pP_CHARGE != null)
                secondtRow = secondtRow.replace("valtxtGridCharge", jsonObject.pP_CHARGE);
            else
                secondtRow = secondtRow.replace("valtxtGridCharge", "");

            if (jsonObject.pP_PROD_LENGTH_STRING != null)
                secondtRow = secondtRow.replace("valtxtGridLength", jsonObject.pP_PROD_LENGTH_STRING);
            else
                secondtRow = secondtRow.replace("valtxtGridLength", "");

            if (jsonObject.dW_ACT_DIM4 != null)
                secondtRow = secondtRow.replace("valtxtActAllocDim4", jsonObject.dW_ACT_DIM4.toFixed(3));
            else
                secondtRow = secondtRow.replace("valtxtActAllocDim4", "");

            if (jsonObject.dW_ACT_DIM5 != null)
                secondtRow = secondtRow.replace("valtxtActAllocDim5", jsonObject.dW_ACT_DIM5.toFixed(3));
            else
                secondtRow = secondtRow.replace("valtxtActAllocDim5", "");

            if (jsonObject.dW_ACT_DIM6 != null)
                secondtRow = secondtRow.replace("valtxtActAllocDim6", jsonObject.dW_ACT_DIM6.toFixed(3));
            else
                secondtRow = secondtRow.replace("valtxtActAllocDim6", "");

            if (jsonObject.dW_REMARKS != null)
                secondtRow = secondtRow.replace("valtxtGridMsgContent", jsonObject.dW_REMARKS);
            else
                secondtRow = secondtRow.replace("valtxtGridMsgContent", "");
           
            if (jsonObject.dW_MAGNETISM != null)
                secondtRow = secondtRow.replace("valtxtGridMagnetism", jsonObject.dW_MAGNETISM);
            else
                secondtRow = secondtRow.replace("valtxtGridMagnetism", "");


            if (jsonObject.dW_SHIKII_VALUE != null)
                secondtRow = secondtRow.replace("valtxtGridShikiiValue", jsonObject.dW_SHIKII_VALUE);
            else
                secondtRow = secondtRow.replace("valtxtGridShikiiValue", "");

            if (jsonObject.dW_EC_NG_BAR_TOTAL != null)
                secondtRow = secondtRow.replace("valtxtGridEcNgBarTotal", jsonObject.dW_EC_NG_BAR_TOTAL);
            else
                secondtRow = secondtRow.replace("valtxtGridEcNgBarTotal", "");

            $("#grdvHikinukiMachine").append($(secondtRow));
        }
    });

}
function GetThabaDetailsByChuuban(chuuban, gyouban) {

    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: String(gyouban) })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonThabaDetailData = response.message;


            SetThabaDetails(jsonThabaDetailData);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

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

    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}

function ClearThabaDetailsGrid() {
    $pnlThabaDetails = $("#pnlThabaDetails");
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

function EnableStartDataPanel() {

    var $startDataPanel = $("#pnlProcessStartData");

    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($startDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(DISABLED).removeAttr(DISABLED);
            $(checkBox).prop('checked', true);
        }
    });

    $.each($startDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

  
}

function EnableEndDataPanel() {

    var $endDataPanel = $("#pnlProcessEndData");

    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($endDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(DISABLED).removeAttr(DISABLED);
            $(checkBox).prop('checked', true);
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    ToggleClassForMagnetism();
}
function ToggleClassForMagnetism()
{
    if ($("#chkECOnline").prop("checked") == true) {
        if (!$("#txtMagnetism").hasClass("Mandatory")) {
            $("#txtMagnetism").addClass("Mandatory");
        }
        $("#txtECNGBars").addClass("Mandatory");
        if ($("#txtECNGBars").val()=="")
        {
            $("#txtECNGBars").val(0);
        }
        
    } else {
        $("#txtMagnetism").removeClass("Mandatory");
        $("#txtECNGBars").removeClass("Mandatory");
        $("#txtECNGBars").val("");
    }
}
function EnableSearchPanel() {
    $("#txtMachineType").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtNifudaNo").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtChuuban2").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtGyouban2").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnChuubanSearch").removeAttr(DISABLED).removeClass(DISABLED);
}

function DisableSearchPanel() {
    $("#txtMachineType").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtNifudaNo").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtChuuban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtGyouban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#btnChuubanSearch").attr(DISABLED, DISABLED).addClass(DISABLED);
}

function DisableSaveButton() {
    $("#btnSave").attr("disabled", "disabled");
}

function EnableSaveButton() {
    $("#btnSave").removeAttr("disabled");
}

function DisableStartDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#pnlProcessStartData");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("select"), function (index, dropdown) {
        //$("#" + dropdown.id + " option:selected").prop("selected", false);
        //$("#" + dropdown.id + " option:first").prop("selected", "selected");
        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
}

function DisableEndDataPanel() {
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

function DisableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events","none");
}
function EnableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
}

function GetHikinukiDetailsByChuuban(chuuban, gyouban, mechineCD) {
    
    
    
    var ajaxTime = new Date().getTime();
   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetHikinukiDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, DW_MACHINE_CD: mechineCD, CULTURE_NAME:$("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if (jsonResult === null) {
                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

         
            if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
                // Chuuban is marked as completed in the system/
                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_004").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            if (jsonResult.procesS_PLAN_START_DATE == null) {
                
                DisableStartDataPanel();
                DisableEndDataPanel();
                var OrNo = jsonResult.pP_CHUUBAN;
                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_005").text().replace("99-999999-99", OrNo);
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            
            DisableSearchOrderNoPanel();
            BindTPTestList();
            BindDieDiaShape();
            BindUnitType();
            EnableStartDataPanel();
            DisableDefaultField();
            $("#ReDrawing").prop("checked", false);
            $("#chkECOnline").prop("checked", false);
            SetChuubanDetails(jsonResult);
            SetStartData(jsonResult);
            GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val());
            if ($("#hdfDBOperation").val() == "Insert") {

                DisableEndDataPanel();
                $("#txtStartOperator").focus();
                $("#txtThaba1").val("");
                $("#txtThaba2").val("");
                $("#txtThaba3").val("");
                $("#txtThaba4").val("");
                for (var i = 1; i <=4; i++) { 
                $("#txtThaba" +i).removeClass("Mandatory");
                    $('#txtThaba' + i).removeAttr("disabled").removeClass("disabled");
                    if (parseInt(jsonResult.dW_UNIT_TYPE) > 0) {
                        $("#ddlUnitType").val(jsonResult.dW_UNIT_TYPE);
                    }
            }
            }
            else if ($("#hdfDBOperation").val() == "Update") {
                EnableEndDataPanel();
                $("#txtThaba1").addClass("Mandatory");
                DisableDefaultField();
                DisableSavedThaba();
               // BindProcessDefectList();
                BindBendStatus();
                BindExteriorCondition();
                BindDecision();
                BindNextProcess();
                SetEndSysDateTime();
                IsActualLengthMandatory();
                IsActualDimMandatory();
                SetProcessDefectList();
                if (parseInt(jsonResult.dW_UNIT_TYPE) > 0) {
                    $("#ddlUnitType").val(jsonResult.dW_UNIT_TYPE);
                }

                var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;

                if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                    $("#chkActualLength").removeAttr("disabled", "disabled");
                    $("#chkActualLength").prop("checked", true);
                    $("#txtActualLength1, #txtActualLength2, #txtActualLength3").addClass("Mandatory");
                } else {
                    $("#chkActualLength").attr("disabled", "disabled");
                    $("#chkActualLength").prop("checked", false);
                    $("#txtActualLength1, #txtActualLength2, #txtActualLength3").removeClass("Mandatory");
                }

                $("#txtEndDate").focus();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function getHikinukiHistoryByChuuban(chuuban, gyouban) {
   
   var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/getHikinukiHistoryByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CHUUBAN: chuuban, GYOUBAN: String(gyouban), CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            console.log(jsonResult);
            if (jsonResult === null) {
                // Chuuban not found in system.
                // TODO Show error message;
                return false;
            }
            var jsonchuubanResult = jsonResult.chuban;

            if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
                // Chuuban is marked as completed in the system/
                // TODO Show error message;
                return false;
            }

            SetChuubanDetailsOnHistoryTab(jsonchuubanResult);
            SetHistoryGrid(jsonResult.processdetails);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    //.always(function () {

    //    $callFrom.data('requestRunning', false);
    //}

}

function SetProcessDefectList() {
    var ajaxTime = new Date().getTime();
       var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
           url: APIURL + "/HikinukiCoilToBar/GetProcessDefect",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindProcessDefectDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindProcessDefectDropdown(jsonResult) {
    $("#ddlNGReason1").html("").append($('<option></option>'));
    $("#ddlNGReason2").html("").append($('<option></option>'));
    $("#ddlNGReason3").html("").append($('<option></option>'));
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
            $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
            $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }
}

function GetMachineDetails(machineCd) {
    
    var ajaxTime = new Date().getTime();

     var settings = {
         async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
         url: APIURL + "/HikinukiCoilToBar/GetMachineDetails?machineCd=" + machineCd + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            
            if (jsonResult.length > 0) {
                Set24HrMachineData(jsonResult);
            } else {
                Reset24MachineGrid();

            }
            $("#txtChuuban2").focus(); 
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });





}

function SetHistoryGrid(jsonResult) {
    
    $tableProcessHistory = $("#grdvHistory");
    ClearTable($tableProcessHistory, "ItemGrdvProcessHistory", "");
    ClearTable($tableProcessHistory, "", "GridNormalRow");

    
    var jsonListObj = jsonResult.procesS_HISTORY;
    $.each(jsonListObj, function (index, jsonObject) {
        
        var firstRow = processHistoryGridataRow1;
        if (jsonObject.pP_LOCATION_NAME != null)
            firstRow = firstRow.replace("valtxtLocationName", jsonObject.pP_LOCATION_NAME);
        else
            firstRow = firstRow.replace("valtxtLocationName", "");

        if (jsonObject.dW_START_DATE_STRING != null)
            firstRow = firstRow.replace("valtxtStartDate", jsonObject.dW_START_DATE_STRING);
        else
            firstRow = firstRow.replace("valtxtStartDate", "");

        if (jsonObject.dW_END_DATE_STRING != null)
            firstRow = firstRow.replace("valtxtEndDate", jsonObject.dW_END_DATE_STRING);
        else
            firstRow = firstRow.replace("valtxtEndDate", "");

        if (jsonObject.pP_CUSTOMER_NAME != null)
            firstRow = firstRow.replace("valtxtCustomerName", jsonObject.pP_CUSTOMER_NAME);
        else
            firstRow = firstRow.replace("valtxtCustomerName", "");

        if (jsonObject.pP_CHUUBAN != null)
            firstRow = firstRow.replace("valtxtChuuban", jsonObject.pP_CHUUBAN);
        else
            firstRow = firstRow.replace("valtxtChuuban", "");

        if (jsonObject.pP_PROD_NAME_CUST != null)
            firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
        else
            firstRow = firstRow.replace("valtxtProdNameCust", "");

        if (jsonObject.dW_METALLIC_PLATE_CODE != null)
            firstRow = firstRow.replace("valtxtMetalicPlate", jsonObject.dW_METALLIC_PLATE_CODE);
        else
            firstRow = firstRow.replace("valtxtMetalicPlate", "");
        
        if (jsonObject.pP_MATL_DIM_D != null)
            firstRow = firstRow.replace("valtxtMatlDim_Shape", jsonObject.pP_MATL_DIM_D + jsonObject.pP_MATL_SHAPE);
        else
            firstRow = firstRow.replace("valtxtMatlDim_Shape", "");

        if (jsonObject.dW_FINISHED_BARS_DECODE != null)
            firstRow = firstRow.replace("valtxtNoOfUnits", jsonObject.dW_FINISHED_BARS_DECODE);
        else
            firstRow = firstRow.replace("valtxtNoOfUnits", "");

        if (jsonObject.pP_ACT_ALLOC_QTY != null)
            firstRow = firstRow.replace("valtxtActAllocQty", jsonObject.pP_ACT_ALLOC_QTY);
        else
            firstRow = firstRow.replace("valtxtActAllocQty", "");

        if (jsonObject.pP_PROD_DIM_D != null)
            firstRow = firstRow.replace("valtxtProdDim_Shape", jsonObject.pP_PROD_DIM_D + jsonObject.pP_PROD_SHAPE);
        else
            firstRow = firstRow.replace("valtxtProdDim_Shape", "");

        if (jsonObject.dW_DIE_DIM != null)
            firstRow = firstRow.replace("valtxtDieDim", jsonObject.dW_DIE_DIM);
        else
            firstRow = firstRow.replace("valtxtDieDim", "");
        var thaba = "";
        if (jsonObject.dW_TABA_NO_1_2 != null)
        {
            thaba = jsonObject.dW_TABA_NO_1_2;

           
        }
        
        if (jsonObject.dW_TABA_NO_3_4 != null)
        {
            if (thaba == "") {
                thaba = jsonObject.dW_TABA_NO_3_4;
            } else
            {
                thaba = thaba + "<br/>" + jsonObject.dW_TABA_NO_3_4;
            }
        }
        if (thaba != "")
            firstRow = firstRow.replace("valtxtThabaNo", thaba);
        else
            firstRow = firstRow.replace("valtxtThabaNo", "");

        if (jsonObject.dW_ACT_DIM1_STRING != null)
            firstRow = firstRow.replace("valtxtActDim1", jsonObject.dW_ACT_DIM1_STRING);
        else
            firstRow = firstRow.replace("valtxtActDim1", "")

        if (jsonObject.dW_ACT_DIM2_STRING != null)
            firstRow = firstRow.replace("valtxtActDim2", jsonObject.dW_ACT_DIM2_STRING);
        else
            firstRow = firstRow.replace("valtxtActDim2", "")

        if (jsonObject.dW_ACT_DIM3_STRING != null)
            firstRow = firstRow.replace("valtxtActDim3", jsonObject.dW_ACT_DIM3_STRING);
        else
            firstRow = firstRow.replace("valtxtActDim3", "")

        if (jsonObject.dW_ACT_LENGTH1 != null)
            firstRow = firstRow.replace("valtxtActLength1", jsonObject.dW_ACT_LENGTH1);
        else
            firstRow = firstRow.replace("valtxtActLength1", "");

        if (jsonObject.dW_ACT_LENGTH2 != null)
            firstRow = firstRow.replace("valtxtActLength2", jsonObject.dW_ACT_LENGTH2);
        else
            firstRow = firstRow.replace("valtxtActLength2", "");

        if (jsonObject.dW_ACT_LENGTH3 != null)
            firstRow = firstRow.replace("valtxtActLength3", jsonObject.dW_ACT_LENGTH3);
        else
            firstRow = firstRow.replace("valtxtActLength3", "");

        if (jsonObject.dW_FINISHED_BARS_DECODE != null)
            firstRow = firstRow.replace("valtxtFinishedBars", jsonObject.dW_FINISHED_BARS_DECODE);
        else
            firstRow = firstRow.replace("valtxtFinishedBars", "");

        if (jsonObject.dW_FINISHED_QTY != null)
            firstRow = firstRow.replace("valtxtFinishedQty", jsonObject.dW_FINISHED_QTY);
        else
            firstRow = firstRow.replace("valtxtFinishedQty", "");

        if (jsonObject.dW_BEND_STATUS_DECODE != null)
            firstRow = firstRow.replace("valtxtBendStatus", jsonObject.dW_BEND_STATUS_DECODE);
        else
            firstRow = firstRow.replace("valtxtBendStatus", "");

        if (jsonObject.dW_EXT_TEXTURE_STATUS_DECODE != null)
            firstRow = firstRow.replace("valtxtExtTexture", jsonObject.dW_EXT_TEXTURE_STATUS_DECODE);
        else
            firstRow = firstRow.replace("valtxtExtTexture", "");

        if (jsonObject.dW_DECISION_STRING != null)
            firstRow = firstRow.replace("valtxtDecision", jsonObject.dW_DECISION_STRING);
        else
            firstRow = firstRow.replace("valtxtDecision", "");
        
        if (jsonObject.nexT_PROCESS_NAME != null)
            firstRow = firstRow.replace("valtxtNextProcess", jsonObject.nexT_PROCESS_NAME);
        else
            firstRow = firstRow.replace("valtxtNextProcess", "");
        
        
        var secondRow = processHistoryGridataRow2;
        if (jsonObject.dW_MACHINE_NAME != null)
            secondRow = secondRow.replace("valtxtMachineName", jsonObject.dW_MACHINE_NAME);
        else
            secondRow = secondRow.replace("valtxtMachineName", "");

        if (jsonObject.dW_END_OPERATOR_NAME != null)
            secondRow = secondRow.replace("valtxtEndOperatorName", jsonObject.dW_END_OPERATOR_NAME);
        else
            secondRow = secondRow.replace("valtxtEndOperatorName", "");

       

        var thirdRow = processHistoryGridataRow3;
        if (jsonObject.pP_PROD_SPEC_NO != null)
            thirdRow = thirdRow.replace("valtxtProdSpecNo", jsonObject.pP_PROD_SPEC_NO);
        else
            thirdRow = thirdRow.replace("valtxtProdSpecNo", "");

        if (jsonObject.pP_CHARGE != null)
            thirdRow = thirdRow.replace("valtxtCharge", jsonObject.pP_CHARGE);
        else
            thirdRow = thirdRow.replace("valtxtCharge", "")
        
        if (jsonObject.pP_PROD_LENGTH_STRING != null)
            thirdRow = thirdRow.replace("valtxtProductLength", jsonObject.pP_PROD_LENGTH_STRING);
        else
            thirdRow = thirdRow.replace("valtxtProductLength", "")

        if (jsonObject.dW_ACT_DIM4_STRING != null)
            thirdRow = thirdRow.replace("valtxtActDim4", jsonObject.dW_ACT_DIM4_STRING);
        else
            thirdRow = thirdRow.replace("valtxtActDim4", "")

        if (jsonObject.dW_ACT_DIM5_STRING != null)
            thirdRow = thirdRow.replace("valtxtActDim5", jsonObject.dW_ACT_DIM5_STRING);
        else
            thirdRow = thirdRow.replace("valtxtActDim5", "")

        if (jsonObject.dW_ACT_DIM6_STRING != null)
            thirdRow = thirdRow.replace("valtxtActDim6", jsonObject.dW_ACT_DIM6_STRING);
        else
            thirdRow = thirdRow.replace("valtxtActDim6", "")
       
        var ngValue = "";
        if (jsonObject.dW_NG_DETAILS1_DECODE != null || jsonObject.dW_NG_DETAILS2_DECODE != null || jsonObject.dW_NG_DETAILS3_DECODE != null) {
            if (jsonObject.dW_NG_DETAILS1_DECODE != null) {
                ngValue = $("#lblDefectDetails").text() + " " +  jsonObject.dW_NG_DETAILS1_DECODE;
            }
            if (jsonObject.dW_NG_DETAILS2_DECODE != null) {
                if (ngValue == "") {
                    ngValue = $("#lblDefectDetails").text() + " " + jsonObject.dW_NG_DETAILS2_DECODE;
                } else {
                    ngValue = ngValue + "/ " + jsonObject.dW_NG_DETAILS2_DECODE;
                }
            }
            if (jsonObject.dW_NG_DETAILS3_DECODE != null) {
                if (ngValue == "") {
                    ngValue = $("#lblDefectDetails").text() + " " + jsonObject.dW_NG_DETAILS3_DECODE;
                } else {
                    ngValue = ngValue + "/ " + jsonObject.dW_NG_DETAILS3_DECODE;
                }
            }
        }
        if (ngValue != "")
            thirdRow = thirdRow.replace("valtxtNGDetails", ngValue);
        else
            thirdRow = thirdRow.replace("valtxtNGDetails", "");

        if (jsonObject.dW_MAGNETISM != null)
            thirdRow = thirdRow.replace("valtxtMagnetism", jsonObject.dW_MAGNETISM);
        else
            thirdRow = thirdRow.replace("valtxtMagnetism", "");
        
        if (jsonObject.dW_SHIKII_VALUE != null)
            thirdRow = thirdRow.replace("valtxtShikiiValue", jsonObject.dW_SHIKII_VALUE);
        else
            thirdRow = thirdRow.replace("valtxtShikiiValue", "");

        if (jsonObject.dW_EC_NG_BAR_TOTAL != null)
            thirdRow = thirdRow.replace("valtxtNGBarTotal", jsonObject.dW_EC_NG_BAR_TOTAL);
        else
            thirdRow = thirdRow.replace("valtxtNGBarTotal", "");


        var forthRow = processHistoryGridataRow4;

        //if (jsonObject.dW_ACT_DIM4_STRING != null)
        //    forthRow = forthRow.replace("valtxtActDim4", jsonObject.dW_ACT_DIM4_STRING);
        //else
        //    forthRow = forthRow.replace("valtxtActDim4", "")

        //if (jsonObject.dW_ACT_DIM5_STRING != null)
        //    forthRow = forthRow.replace("valtxtActDim5", jsonObject.dW_ACT_DIM5_STRING);
        //else
        //    forthRow = forthRow.replace("valtxtActDim5", "")

        //if (jsonObject.dW_ACT_DIM6_STRING != null)
        //    forthRow = forthRow.replace("valtxtActDim6", jsonObject.dW_ACT_DIM6_STRING);
        //else
        //    forthRow = forthRow.replace("valtxtActDim6", "")
        
        

        $tableProcessHistory.append($(firstRow));
        $tableProcessHistory.append($(secondRow));
        $tableProcessHistory.append($(thirdRow));
        $tableProcessHistory.append($(forthRow));
    });

}

function SetChuubanDetailsOnHistoryTab(jsonResult) {

    if (jsonResult !== null) {
        $("#txtChuubanT2").val(jsonResult.pP_CHUUBAN);       
        $("#txtCustomerT2").val(jsonResult.pP_CUSTOMER_NAME);
        $("#txtMaterialNameMakerT2").val(jsonResult.pP_MATL_NAME_MAK);
        $("#txtProdNameCustomerT2").val(jsonResult.pP_PROD_NAME_CUST);
        $("#txtHostRefNoT2").val(jsonResult.producT_NYUKO_CD);
        $("#txtStampT2").val(jsonResult.pP_STAMP);
        $("#txtNoOfUnitsT2").val(jsonResult.pP_NO_OF_UNITS_DECODE);
        $("#txtKakouiraiRemarksT2").val(jsonResult.pP_KAKOUIRAI_REMARKS);
        $("#txtProdOrderRemarkT2").val(jsonResult.pP_PROCESS_PLAN_REMARK);
        $("#txtProductionProcessT2").val(jsonResult.pP_PRODUCTION_PROCESS);

        $("#txtDeliveryDateT2").val(jsonResult.pP_DELIVERY_DATE_STRING);
        $("#txtReceiverT2").val(jsonResult.pP_RECEIVER_NAME);
        $("#txtMatlShapeT2").val(jsonResult.pP_MATL_SHAPE);
        $("#txtProdShapeT2").val(jsonResult.pP_PROD_SHAPE);
        $("#txtMatlDimT2").val(jsonResult.pP_MATL_DIM_D);
        $("#txtProdDimT2").val(jsonResult.pP_PROD_DIM_D);
        $("#txtDimToleranceT2").val(jsonResult.pP_DIM_TOLERANCE);
        $("#txtLengthT2").val(jsonResult.pP_PROD_LENGTH_STRING);
        $("#txtQuantityT2").val(jsonResult.pP_ACT_ALLOC_QTY);

        $("#txtCustomerInchargeT2").val(jsonResult.pP_EMPLOYEE_NAME);
        if (parseInt(jsonResult.pP_CHAMFER) > 0) {
            $("#txtChamferT2").val(jsonResult.pP_CHAMFER);
        }
        if (jsonResult.pP_UNIT_WT_STRING != "") {

            $("#txtUnitWtT2").val(jsonResult.pP_UNIT_WT_STRING);
        }
        $("#txtChargeT2").val(jsonResult.pP_CHARGE);
        $("#txtProdDimOtherT2").val(jsonResult.pP_PROD_DIM_OTHER);
        if (jsonResult.pP_LEN_TOL != "0,0") {
            $("#txtLengthToleranceT2").val(jsonResult.pP_LEN_TOL);
        }
        $("#txtIntDiaMinT2").val(jsonResult.pP_COIL_DIA_INTERNAL);
        $("#txtExtDiaMaxT2").val(jsonResult.pP_COIL_DIA_EXTERNAL);
        $("#txtWindingDirectionT2").val(jsonResult.pP_COIL_DIA_WINDING);

        $("#txtProductSpecNoT2").val(jsonResult.pP_PROD_SPEC_NO);
        $("#txtStdDeliveryDateT2").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
        $("#txtColorT2").val(jsonResult.pP_COLOR);
        $("#txtBendStandardT2").val(jsonResult.pP_MAGARI_BEND_TOL);
        $("#txtExtTextureT2").val(jsonResult.pP_EXT_TEXTURE);
        $("#txtDisplayT2").val(jsonResult.pP_DISPLAY);
        $("#txtPackingT2").val(jsonResult.pP_PACKING_DECODE);

        $("#txtConditionT2").val(jsonResult.pP_PRODUCT_CONDITION);
        $("#txtMaterialT2").val(jsonResult.pP_MATL_NAME_MAK);
        $("#txtDimT2").val(jsonResult.pP_MATL_DIM_D_SHAPE);              

    }
}

function ResetMessageDetailsGrid() {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $('<td class="GridAlignmentDateTime">').text(""),
        $('<td class="GridAlignmentProcessName">').text(""),
        $('<td class="GridAlignmentMachineName">').text(""),
        $('<td class="GridAlignmentOperator">').text(""),
        $('<td class="GridAlignmentRemarks GridRemarks200PX">').text("")
    );
    $grdvMessage.append($tr);
}

function SetMessageDetailsGrid(messageDetailsList) {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add data rows
    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
        if ($("#hdfCultureName").val() == "en")
        {
            processName = jsonObject.procesS_NAME_EN;
        }

        var $tr = $('<tr class="GridNormalRow">').append(
            $('<td class="GridAlignmentDateTime">').text(jsonObject.msG_DATE_STRING),
            
            $('<td class="GridAlignmentProcessName GridLeft">').text(processName),
            $('<td class="GridAlignmentMachineName GridLeft">').text(jsonObject.machinE_NAME),
            $('<td class="GridAlignmentOperator GridLeft">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks GridLeft">').text(jsonObject.msG_CONTENTS)
        );

        $grdvMessage.append($tr);
    });
}
function SetChuubanDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#btnSave").removeAttr("disabled");
    dbOperation = "Insert";
    if ((jsonResult.dW_START_DATE_STRING != null) && (jsonResult.dW_END_DATE_STRING == null) && jsonResult.pasT_DATA_EXIST == "YES") {
        dbOperation = "Update";
    }
    $("#hdfDBOperation").val(dbOperation);

    hikinukiCD = $.isNumeric(jsonResult.dW_DRAWING_CD) ? parseInt(jsonResult.dW_DRAWING_CD) : 0;

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);

    $("#txtProductSpecNoT1").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN);
    $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN);

    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);


    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtCustomerInchargeT4").text(jsonResult.pP_EMPLOYEE_NAME);

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

    if (jsonResult.pP_UNIT_WT_STRING !="") {

        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_STRING);
        $("#txtUnitWtT4").val(jsonResult.pP_UNIT_WT_STRING);
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

    $("#txtHostRefNoT1").val(jsonResult.producT_NYUKO_CD);
    $("#txtHostRefNoT4").val(jsonResult.producT_NYUKO_CD);

    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtExtTextureT4").val(jsonResult.pP_EXT_TEXTURE);

    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDimToleranceT4").val(jsonResult.pP_DIM_TOLERANCE);

    if (jsonResult.pP_DIM_TOL_MIN != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    if (jsonResult.pP_DIM_TOL_MAX != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    dimStd = jsonResult.pP_PROD_DIM_D;

    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtConditionT4").val(jsonResult.pP_PRODUCT_CONDITION);

    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);

    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtStampT4").val(jsonResult.pP_STAMP);

    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_STRING);

    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
        $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
    }

    if (jsonResult.pP_LENGTH_TOL_MIN != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    if (jsonResult.pP_LENGTH_TOL_MAX != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

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
    
    if (jsonResult.dW_EC_DONE_CHK == "1") {
        $("#chkECOnline").checked = true;
    }
    else {
        $("#chkECOnline").checked = false;
    }

    if (jsonResult.dW_REDRAWING == "1") {
        $("#ReDrawing").checked = true;
    }
    else {
        $("#ReDrawing").checked = false;
    }
    
    // 3rd Tab
    ResetMessageDetailsGrid();
    if (jsonResult.dW_MESSAGE_LIST) {
        SetMessageDetailsGrid(jsonResult.dW_MESSAGE_LIST);
    }

    //4th Tab 

    $("#txtElongation").val(jsonResult.pP_ELONGATION);
    $("#txtTensileStrength").val(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtHardness").val(jsonResult.pP_HARDNESS);
    $("#txtDecarb").val(jsonResult.pP_DECARB);
    $("#txtYieldPoint").val(jsonResult.pP_YEILD);
    $("#txtReduction").val(jsonResult.pP_PRESS);// 

    $("#txtJISTestSpecimen").val(jsonResult.pP_JIS_TEST_SPEC);


    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4      //Added by Sagar B on 21/8/2017 .

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
        "<td class='ProcessName GridLeft' style='font-weight: normal;text-align:left;'>" + processName + "</td>" +
        "<td class='ProdPointDetails GridLeft'  style='font-weight: normal;text-align:left;'>" + $_prodPint + "</td>" +
        "</tr>");
    $grdvProdPointData.append($tr);
}

function SetStartData(jsonResult) {
    
    SetStartSysDateTime();
    $("#txtNoOfCoils").val(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtCoilQuantity").val(jsonResult.pP_ACT_ALLOC_QTY);
    if (jsonResult.dW_START_DATE_STRING != null && jsonResult.dW_END_DATE_STRING == null && jsonResult.pasT_DATA_EXIST == "YES") {
        $("#txtStartOperator").val(jsonResult.dW_START_OPERATOR_NAME);
        $("#hdfStartOperatorCD").val(jsonResult.dW_START_OPERATOR);
        $("#ddlTPTest").val(jsonResult.dW_TP_TEST);
        $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);

        $("#txtMetallicLabelNo").val(jsonResult.dW_METALLIC_PLATE_CODE);
        $("#txtC").val(jsonResult.dW_CARBON_PERCENTAGE);
        $("#txtMn").val(jsonResult.dW_MAGNIZ_PERCENTAGE);

        $("#txtDieDim").val(jsonResult.dW_DIE_DIM);


        $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
        $("#txtDieNo").val(jsonResult.dW_DIE_NO);


        $("#txtDrawDim1").val(jsonResult.dW_DIM1);
        $("#txtDrawDim2").val(jsonResult.dW_DIM2);

        $("#txtUpperLowerRollerAngle1").val(jsonResult.dW_UPPER_LOWER_ROLLER_ANGLE1);
        $("#txtUpperLowerRollerAngle2").val(jsonResult.dW_UPPER_LOWER_ROLLER_ANGLE2);


        $("#txtUpperRoleElevation").val(jsonResult.dW_UPPER_ROLLER_ELEVATION);

        $("#txtCompressionTonnage").val(jsonResult.dW_TONNAGE);

        $("#txtHexR").val(jsonResult.dW_HEX_R);

        if (parseInt(jsonResult.dW_EC_DONE_CHK) == 1) {
            $("#chkECOnline").prop('checked', true);
        }
        else {
            $("#chkECOnline").prop('checked', false);
        }
        if (parseInt(jsonResult.dW_REDRAWING) == 1) {
            $("#ReDrawing").prop('checked', true);
        }
        else {
            $("#ReDrawing").prop('checked', false);
        }


        $("#txtShikii").val(jsonResult.dW_SHIKII_VALUE);
        $("#txtInspectionCoilSize").val(jsonResult.dW_EC_COIL_SIZE);
        $("#txtInspectionCoilShape").val(jsonResult.dW_EC_COIL_SHAPE);
        $("#txtIsOK").val(jsonResult.dW_EC_IS_OK);

        $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        $("#txtEndOperator").val(jsonResult.dW_START_OPERATOR_NAME);
        $("#hdfEndOperatorCD").val(jsonResult.dW_START_OPERATOR);
        $("#txtThaba1").val(jsonResult.dW_TABA_NO_1);
        $("#txtThaba2").val(jsonResult.dW_TABA_NO_2);
        $("#txtThaba3").val(jsonResult.dW_TABA_NO_3);
        $("#txtThaba4").val(jsonResult.dW_TABA_NO_4);


    } else if (jsonResult.dW_START_DATE_STRING != null && jsonResult.dW_END_DATE_STRING != null && jsonResult.pasT_DATA_EXIST == "YES")
    {
        $("#ddlTPTest").val(jsonResult.dW_TP_TEST);
        $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);

        $("#txtMetallicLabelNo").val(jsonResult.dW_METALLIC_PLATE_CODE);
        $("#txtC").val(jsonResult.dW_CARBON_PERCENTAGE);
        $("#txtMn").val(jsonResult.dW_MAGNIZ_PERCENTAGE);

        $("#txtDieDim").val(jsonResult.dW_DIE_DIM);


        $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
        $("#txtDieNo").val(jsonResult.dW_DIE_NO);


        $("#txtDrawDim1").val(jsonResult.dW_DIM1);
        $("#txtDrawDim2").val(jsonResult.dW_DIM2);

        $("#txtUpperLowerRollerAngle1").val(jsonResult.dW_UPPER_LOWER_ROLLER_ANGLE1);
        $("#txtUpperLowerRollerAngle2").val(jsonResult.dW_UPPER_LOWER_ROLLER_ANGLE2);


        $("#txtUpperRoleElevation").val(jsonResult.dW_UPPER_ROLLER_ELEVATION);

        $("#txtCompressionTonnage").val(jsonResult.dW_TONNAGE);

        $("#txtHexR").val(jsonResult.dW_HEX_R);

        if (parseInt(jsonResult.dW_EC_DONE_CHK) == 1) {
            $("#chkECOnline").prop('checked', true);
        }
        else {
            $("#chkECOnline").prop('checked', false);
        }
       


        $("#txtShikii").val(jsonResult.dW_SHIKII_VALUE);
        $("#txtInspectionCoilSize").val(jsonResult.dW_EC_COIL_SIZE);
        $("#txtInspectionCoilShape").val(jsonResult.dW_EC_COIL_SHAPE);
        $("#txtIsOK").val(jsonResult.dW_EC_IS_OK);
    }
}


function GetOperatorNameByCD($target) {
    var ajaxTime = new Date().getTime();

    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_030").text().trim().replace("*****", $("#tdlblStartOperator").text());

        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_HIKI_ENTRY_015").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_015").text().trim();
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
            url: APIURL + "/HikinukiCoilToBar/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",


        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;


                if (jsonResult == null || jsonResult == "") {
                    var message = $("#lblErrorMessageE_HIKI_ENTRY_031").text().trim().replace("*****", $("#tdlblStartOperator").text());
                    ShowMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }//employee is inactive.
                else if (jsonResult.iN_ACTIVE_FLAG == 1) {

                    var message = $("#lblErrorMessageE_HIKI_ENTRY_024").text().trim();
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
                message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            })
    }
}

function GetMachineNamebyCD($target) {
    var ajaxTime = new Date().getTime();
   // var machineCD = $target.val().trim().replace(/\D/g, ""); 

    var ajaxTime = new Date().getTime();
    var machineNumber = $target.val().trim();
    var machineval = machineNumber.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineCD = machineNumber.substring(3, 7);
    
    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_030").text().trim().replace("*****", $("#tdLblMachineType").text());

        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
    if (machineCD == "") {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_020").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }   
    
    if (machineCD.length > 4) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {
    var settings = {
        async: false,
        crossDomain: true,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetMachineNamebyCD?machineCD=" + parseInt(machineCD),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var machineName = response.message;

            if (machineName.key == null || machineName.key == "") {
                var message = $("#lblErrorMessageE_HIKI_ENTRY_031").text().trim().replace("*****", $("#tdLblMachineType").text());
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }
            else if (machineName.value == "inactive") {
                var message = $("#lblErrorMessageE_HIKI_ENTRY_016").text().trim();
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }
            else {
                $target.val(machineName.key);
                GetMachineDetails($("#hdfMachineCD").val());
                $("#txtChuuban2").focus(); 
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    }
}


function SaveHikinukiDetails() {

    var ajaxTime = new Date().getDate();

    var jsonDataForSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val() ? $("#txtChuuban2").val() : null,
        PP_GYOUBAN: $("#txtGyouban2").val() ? $("#txtGyouban2").val() : null,
        DW_DRAWING_CD: hikinukiCD ? hikinukiCD : null,
        PP_PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        DW_MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        DW_TP_TEST: $("#ddlTPTest").val() ? $("#ddlTPTest").val() : null,
        DW_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        DW_NO_OF_COILS: $("#txtNoOfCoils").val() ? $("#txtNoOfCoils").val() : null,
        DW_QTY: $("#txtCoilQuantity").val() ? $("#txtCoilQuantity").val() : null,
        DW_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        DW_CARBON_PERCENTAGE: $("#txtC").val() ? $("#txtC").val() : null,
        DW_MAGNIZ_PERCENTAGE: $("#txtMn").val() ? $("#txtMn").val() : null,

        DW_DIE_DIM: $("#txtDieDim").val() ? $("#txtDieDim").val() : null,
        DW_DIE_SHAPE: $("#ddlDieShape").val() ? $("#ddlDieShape").val() : null,
        DW_DIE_NO: $("#txtDieNo").val() ? $("#txtDieNo").val() : null,
        DW_DIM1: $("#txtDrawDim1").val() ? $("#txtDrawDim1").val() : null,
        DW_DIM2: $("#txtDrawDim2").val() ? $("#txtDrawDim2").val() : null,
        DW_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
        DW_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
        DW_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,

        DW_TONNAGE: $("#txtCompressionTonnage").val() ? $("#txtCompressionTonnage").val() : null,
        DW_HEX_R: $("#txtHexR").val() ? $("#txtHexR").val() : null,
        DW_EC_DONE_CHK: ($('#chkECOnline').is(':checked') ? 1 : 0),
        DW_SHIKII_VALUE: $("#txtShikii").val() ? $("#txtShikii").val() : null,
        DW_EC_COIL_SIZE: $("#txtInspectionCoilSize").val() ? $("#txtInspectionCoilSize").val() : null,
        DW_EC_COIL_SHAPE: $("#txtInspectionCoilShape").val() ? $("#txtInspectionCoilShape").val() : null,
        DW_EC_IS_OK: $("#txtIsOK").val() ? $("#txtIsOK").val() : null,
        DW_REDRAWING: ($('#ReDrawing').is(':checked') ? 1 : 0),
        DW_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        DW_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
});

 
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/HikinukiDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            EnableEndDataPanel();
            $("#txtThaba1").addClass("Mandatory");
            DisableDefaultField();
            BindUnitType();
            BindProcessDefectList();
            BindBendStatus();
            BindExteriorCondition();
            BindDecision();
            BindNextProcess();
            IsActualLengthMandatory();
            IsActualDimMandatory();
            var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;

            if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                $("#chkActualLength").removeAttr("disabled", "disabled");
                $("#chkActualLength").prop("checked", true);
                $("#txtActualLength1, #txtActualLength2, #txtActualLength3").addClass("Mandatory");
            } else {
                $("#chkActualLength").attr("disabled", "disabled");
                $("#chkActualLength").prop("checked", false);
                $("#txtActualLength1, #txtActualLength2, #txtActualLength3").removeClass("Mandatory");
            }
            DisableSavedThaba();
            var successMessage = $("#lblSuccessMessageS_HIKI_ENTRY_001").text();
            ShowMessageDialog(successMessage, "txtEndOperator", SUCCESS);
            var jsonResult = response.message;
            if (jsonResult.dW_START_DATE != null && jsonResult.dW_END_DATE == null)
            {
                SetChuubanDetails(jsonResult);
                SetStartData(jsonResult);
                SetProcessDefectList();
                GetMachineDetails($("#hdfMachineCD").val());
                $("#hdfDBOperation").val("Update");

                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndOperator").focus();

                $("#txtEndOperator").val($("#txtStartOperator").val());
                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
                if (parseInt(jsonResult.dW_UNIT_TYPE) > 0) {
                    $("#ddlUnitType").val(jsonResult.dW_UNIT_TYPE);
                }
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function UpdateHikinukiDetails() {
   
    var ajaxTime = new Date().getDate();

    var jsonDataForUpdate = JSON.stringify({
        DW_DRAWING_CD: hikinukiCD ? hikinukiCD : null,
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        DW_TP_TEST: $("#ddlTPTest").val() ? $("#ddlTPTest").val() : null,
        DW_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        DW_NO_OF_COILS: $("#txtNoOfCoils").val() ? $("#txtNoOfCoils").val() : null,
        DW_QTY: $("#txtCoilQuantity").val() ? $("#txtCoilQuantity").val() : null,
        DW_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        DW_CARBON_PERCENTAGE: $("#txtC").val() ? $("#txtC").val() : null,
        DW_MAGNIZ_PERCENTAGE: $("#txtMn").val() ? $("#txtMn").val() : null,
        DW_DIE_DIM: $("#txtDieDim").val() ? $("#txtDieDim").val() : null,
        DW_DIE_SHAPE: $("#ddlDieShape").val() ? $("#ddlDieShape").val() : null,
        DW_DIE_NO: $("#txtDieNo").val() ? $("#txtDieNo").val() : null,
        DW_DIM1: $("#txtDrawDim1").val() ? $("#txtDrawDim1").val() : null,
        DW_DIM2: $("#txtDrawDim2").val() ? $("#txtDrawDim2").val() : null,
        DW_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
        DW_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
        DW_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
        DW_TONNAGE: $("#txtCompressionTonnage").val() ? $("#txtCompressionTonnage").val() : null,
        DW_HEX_R: $("#txtHexR").val() ? $("#txtHexR").val() : null,
        DW_EC_DONE_CHK: ($('#chkECOnline').is(':checked') ? 1 : 0),
        DW_SHIKII_VALUE: $("#txtShikii").val() ? $("#txtShikii").val() : null,
        DW_EC_COIL_SIZE: $("#txtInspectionCoilSize").val() ? $("#txtInspectionCoilSize").val() : null,
        DW_EC_COIL_SHAPE: $("#txtInspectionCoilShape").val() ? $("#txtInspectionCoilShape").val() : null,
        DW_EC_IS_OK: $("#txtIsOK").val() ? $("#txtIsOK").val() : null,

        DW_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        PP_PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        DW_END_OPERATOR: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
        DW_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        DW_ACT_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        DW_ACT_DIM1: $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null,
        DW_ACT_DIM2: $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null,
        DW_ACT_DIM3: $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null,
        DW_ACT_DIM4: $("#txtActualDim4").val() ? $("#txtActualDim4").val() : null,
        DW_ACT_DIM5: $("#txtActualDim5").val() ? $("#txtActualDim5").val() : null,
        DW_ACT_DIM6: $("#txtActualDim6").val() ? $("#txtActualDim6").val() : null,
        DW_ACT_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
        DW_ACT_LENGTH1: $("#txtActualLength1").val() ? $("#txtActualLength1").val() : null,
        DW_ACT_LENGTH2: $("#txtActualLength2").val() ? $("#txtActualLength2").val() : null,
        DW_ACT_LENGTH3: $("#txtActualLength3").val() ? $("#txtActualLength3").val() : null,
        DW_FINISHED_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
        DW_UNIT_TYPE: $("#ddlUnitType").val() ? $("#ddlUnitType").val() : null,
        DW_FINISHED_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
        DW_FINISHED_QTY_CHK: ($("#chkFinishedQuantity").is(':checked') ? 1 : 0),
        DW_CORRECTED_BARS: $("#txtCorrectedBars").val() ? $("#txtCorrectedBars").val() : null,
        DW_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : null,
        DW_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : null,
        DW_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : null,
        DW_NG_DETAILS1: $("#ddlNGReason1").val() > 0 ? $("#ddlNGReason1").val() : null,
        DW_NG_DETAILS2: $("#ddlNGReason2").val() > 0 ? $("#ddlNGReason2").val() : null,
        DW_NG_DETAILS3: $("#ddlNGReason3").val() > 0 ? $("#ddlNGReason3").val() : null,
        DW_A_SIDE_FURE1: $("#txtAEdgeDistortion1").val() ? $("#txtAEdgeDistortion1").val() : null,
        DW_A_SIDE_FURE2: $("#txtAEdgeDistortion2").val() ? $("#txtAEdgeDistortion2").val() : null,
        DW_A_SIDE_FURE3: $("#txtAEdgeDistortion3").val() ? $("#txtAEdgeDistortion3").val() : null,
        DW_B_SIDE_FURE1: $("#txtBEdgeDistortion1").val() ? $("#txtBEdgeDistortion1").val() : null,
        DW_B_SIDE_FURE2: $("#txtBEdgeDistortion2").val() ? $("#txtBEdgeDistortion2").val() : null,
        DW_B_SIDE_FURE3: $("#txtBEdgeDistortion3").val() ? $("#txtBEdgeDistortion3").val() : null,
        DW_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        DW_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
        DW_EXT_TEXTURE_STATUS: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
        DW_DISPOSAL_NO: $("#txtDisposalBars").val() ? $("#txtDisposalBars").val() : null,
        DW_EC_NG_BAR_TOTAL: $("#txtECNGBars").val() ? $("#txtECNGBars").val() : null,
        DW_MAGNETISM: $("#txtMagnetism").val() ? $("#txtMagnetism").val() : null,
        DW_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        DW_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
        DW_REDRAWING: ($('#ReDrawing').is(':checked') ? 1 : 0),
        DW_MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        DW_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        
    });

  
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/HikinukiDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

           
            var successMessage = $("#lblSuccessMessageS_HIKI_ENTRY_001").text();
            ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);
            GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val());
                    DisableStartDataPanel();
                    DisableEndDataPanel();
                    DisableSearchPanel();
                    DisableSaveButton();
             
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

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
function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === TYPEDROPDOWN && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === TYPETEXT && !($(mandatoryField).hasClass(DISABLED))) {
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
        ShowMessageDialog(message, focusFieldID, WARNING);
    }

    return returnVal;
}

function IsMandatoryChuubanSearchFieldsFilledHistoryTab() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find("#pnlCustomerDetails .Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === TYPEDROPDOWN && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel") + " " + isRequired;
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === TYPETEXT && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel") + " " + isRequired;
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        bootbox.alert({
            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
            message: message,
            centerVertical: true,
            callback: function () {
                if (firstFieldID !== "") {
                    $("#" + firstFieldID).focus();
                }
            }
        });
    }

    return returnVal;
}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === TYPEDROPDOWN && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === TYPETEXT && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        ShowMessageDialog(message, focusFieldID, WARNING);
    }

    return returnVal;

}

function IsDuplicateThabaNo() {
    var thabaList = [];
    var inputThaba1 = $("#txtThaba1").val().trim();
    var inputThaba2 = $("#txtThaba2").val().trim();
    var inputThaba3 = $("#txtThaba3").val().trim();
    var inputThaba4 = $("#txtThaba4").val().trim();
    var t1 = "";
    var t2 = "";
    var t3 = "";
    var t4 = "";
    var thabamessage = "";
    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "txtThaba1";

    var chk;
    thabaList.push(inputThaba1);
    if (inputThaba2 != "") {
        thabaList.push(inputThaba2);
    }
    if (inputThaba3 != "") {
        thabaList.push(inputThaba3);
    }
    if (inputThaba4 != "") {
        thabaList.push(inputThaba4);
    }

    chk = new Set();
    var duplicateThaba = thabaList.filter(n => chk.size === chk.add(n).size);

    
    if (duplicateThaba.length > 0) {
        if (duplicateThaba.length > 1) {
            message = $("#lblErrorMessageE_HIKI_ENTRY_006").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_HIKI_ENTRY_006").text().trim().replace("NNNN", duplicateThaba[0]);;
        }

        ShowMessageDialog(message, firstFieldID, WARNING);
        return true;
    }
    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK && $('#ReDrawing').prop("checked") == false) {

            t1 = inputThaba1;
        }
    }
    if (inputThaba2 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba2]) == combinedDecisionOK && $('#ReDrawing').prop("checked") == false) {

            t2 = inputThaba2;
        }
    }
    if (inputThaba3 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba3]) == combinedDecisionOK && $('#ReDrawing').prop("checked") == false) {
            t3 = inputThaba3;
        }
    }
    if (inputThaba4 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba4]) == combinedDecisionOK && $('#ReDrawing').prop("checked") == false) {

            t4 = inputThaba4;
        }
    }
    if (t1 != "") {
        thabamessage = t1 + ", ";
    }
    if (t2 != "") {
        thabamessage = thabamessage + t2 + ", ";
    }
    if (t3 != "") {
        thabamessage = thabamessage + t3 + ", ";
    }
    if (t4 != "") {
        thabamessage = thabamessage + t4 + ", ";
    }
    if (thabamessage != "") {
        message = $("#lblErrorMessageE_HIKI_ENTRY_029").text().trim().replace("NNN", thabamessage.replace(/(.+), $/, '$1'));
        ShowMessageDialog(message, firstFieldID, WARNING);
        return true;
    } else {

        return false;
    }


    return false;
}


function SetStartSysDateTime() {
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var sysDateTime = response.message;

            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}


function SetEndSysDateTime() {
    var ajaxTime = new Date().getTime();
   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var sysDateTime = response.message;

            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
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
            console.log(regs);
            
           var totdalDays = new Date(regs[1], regs[2], 0).getDate();
            if (parseInt(regs[3]) > totdalDays) {

                return false;
            }else if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {

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
$("#chkECOnline").click(function () {
    ToggleClassForMagnetism();
});

$("#chkActualDim").click(function () {
    IsActualDimMandatory();
});
function IsActualDimMandatory() {
    if ($("#chkActualDim").is(':checked')) {
        $("#txtActualDim1, #txtActualDim2, #txtActualDim3").addClass("Mandatory");
    } else {
        $("#txtActualDim1, #txtActualDim2, #txtActualDim3").removeClass("Mandatory");
    }
}
$("#chkActualLength").click(function () {
    IsActualLengthMandatory();
});
function IsActualLengthMandatory() {
    if ($("#chkActualLength").is(':checked')) {
        $("#txtActualLength1, #txtActualLength2, #txtActualLength3").addClass("Mandatory");
    } else {
        $("#txtActualLength1, #txtActualLength2, #txtActualLength3").removeClass("Mandatory");
    }
}

function SetActDimD456() {

    if ($("#txtActualDim4").val().length > 0 ||
        $("#txtActualDim5").val().length > 0 ||
        $("#txtActualDim6").val().length > 0) {
        //set ActualDim4, ActualDim5, ActualDim6 Mandatory

       
    }

    if ($("#txtActualDim4").val().length == 0 &&
        $("#txtActualDim5").val().length == 0 &&
        $("#txtActualDim6").val().length == 0) {
        //set ActualDim4, ActualDim5, ActualDim6 Non-Mandatory
        $("#txtActualDim4, #txtActualDim5, #txtActualDim6").removeClass("Mandatory");
    }

}



    $("#btnInitialize").click(function (event) {
        Initialize();
        $("#btnSave").attr("disabled", "disabled"); 
    });
$('#txtMachineType')
    .on('focus', function () {
        machineModified = false;
    })
    .on('input', function () {
        machineModified = true;
    });

$('#txtMachineType').on('input keydown focusout', function (e) {
        Reset24MachineGrid();
        var machine = e.target.value ? e.target.value : null;
        var keycode = (e.keyCode ? e.keyCode : e.which);

    if (e.which == 13 && machine && machineModified == true) {
        
        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
            ClearTable($("#grdvHikinukiMachine"), "ItemGrdvHikinukiMachine", "");
            var message = $("#lblErrorMessageE_HIKI_ENTRY_020").text().trim();
            ShowMessageDialog(message, "txtMachineType", WARNING);

        } else {

            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
            enterPressedMachine = true;
            GetMachineNamebyCD($(this));
            $("#txtChuuban2").focus();
        }
    }
        if (e.type === 'focusout' && machine && machineModified == true) {
            if (enterPressedMachine) {

                enterPressedMachine = false; // Reset the flag
                return false;
            }
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
            GetMachineNamebyCD($(this));
            $("#txtChuuban2").focus();
        }
        enterPressedMachine = false; // Reset the flag
   
   
    });
$('#txtStartOperator')
    .on('focus', function () {
        operatorStartModified = false;
    })
    .on('input', function () {
        operatorStartModified = true;
    });

$('#txtStartOperator').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorStartModified == true) {
      
        $("#hdfStartOperatorCD").val(operatorNumber);
        enterPressedStartOpr = true;
        GetOperatorNameByCD($(this));
        $("#ddlTPTest").focus();
    }
    if (e.type === 'focusout' && operator && operatorStartModified == true) {
        if (enterPressedStartOpr) {

            enterPressedStartOpr = false; // Reset the flag
            return false;
        }
        $("#hdfStartOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#ddlTPTest").focus();
    }
    enterPressedStartOpr = false; // Reset the flag
    });


$('#txtEndOperator')
    .on('focus', function () {
        operatorEndModified = false;
    })
    .on('input', function () {
        operatorEndModified = true;
    });

$('#txtEndOperator').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which == 13 && operator && operatorEndModified == true) {
        
        $("#hdfEndOperatorCD").val(operatorNumber);
        enterPressedEndOpr = true;
        GetOperatorNameByCD($(this));
        if ($("#chkActualLength").attr('disabled')) {
            $("#txtActualLength1").focus();
        } else {
            $("#chkActualLength").focus();
        }
    }
    if (e.type === 'focusout' && operator && operatorEndModified == true) {
        if (enterPressedEndOpr) {

            enterPressedEndOpr = false; // Reset the flag
            return false;
        }
        $("#hdfEndOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        if ($("#chkActualLength").attr('disabled')) {
            $("#txtActualLength1").focus();
        } else {
            $("#chkActualLength").focus();
        }
    }
    enterPressedEndOpr = false; // Reset the flag
    });

       
$("#btnChuubanSearch").click(function (event) {
 
        ClearData();
        ResetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
    }

     ValidateChubban("Registration",$("#txtChuuban2").val(), $("#txtGyouban2").val());
    // getHikinukiDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val());
   
       
        
    });
$("#btnSave").click(function (event) {
  
        ResetResponseTime();
       
        event.preventDefault();

        if (!IsMandatoryFieldsFilled()) {
            return false;
        }
        if ($("#hdfDBOperation").val().length == 0) {
            var message = $("#lblErrorMessageE_HIKI_ENTRY_002").text().trim(); // This message will be displayed on dialog box
            ShowMessageDialog(message, "", WARNING);
            return false;
        }
        if (IsDuplicateThabaNo()) {
            return false;
        }

        if (!CheckDate($("#txtStartDate"))) {
            var message = $("#lblErrorMessageE_HIKI_ENTRY_017").text().trim();
            ShowMessageDialog(message, "txtStartDate", WARNING);
            return false;
        }

        

        if ($("#hdfDBOperation").val() == "Insert") {
            if (!IsDateValid(true)) {
                var message = $("#lblErrorMessageE_HIKI_ENTRY_021").text().trim() + "(" + $("#lblErrorMessageE_HIKI_ENTRY_023").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtStartDate";
                ShowMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
            if (($("#txtThaba1").val() == '') && ($("#txtThaba2").val() == '') && ($("#txtThaba3").val() == '') && ($("#txtThaba4").val() == '')) {
                var message = "";

                message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_032").text());

                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {

                        if (result) {
                            SaveHikinukiDetails();
                        }
                        else {
                            // return false;
                            $("#txtThaba1").focus();

                        }
                    },
                    centerVertical: true
                });

            }
            else {
                SaveHikinukiDetails();
            }
            
        }
        else {
            // Check all error conditions
           
            if (!CheckDate($("#txtEndDate"))) {
                var message = $("#lblErrorMessageE_HIKI_ENTRY_018").text().trim();
                ShowMessageDialog(message, "txtEndDate", WARNING);
                return false;
            }

            if (!IsDateValid(false)) {
                var message = $("#lblErrorMessageE_HIKI_ENTRY_022").text().trim() + "(" + $("#lblErrorMessageE_HIKI_ENTRY_023").text().trim() + commonDateRange + ")";

                var focusFieldID = "txtEndDate";
                ShowMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
            if (!IsEndDateValid()) {

                return false;
            }
            GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val());
            if (IsDuplicateThabaNo()) {
                return false;
            }
           
            if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK) {
                var dimValues = {};
                dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
                dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
                dimValues["DIM1"] = $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null;
                dimValues["DIM2"] = $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null;
                dimValues["DIM3"] = $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null;
                dimValues["DIM4"] = $("#txtActualDim4").val() ? $("#txtActualDim4").val() : null;
                dimValues["DIM5"] = $("#txtActualDim5").val() ? $("#txtActualDim5").val() : null;
                dimValues["DIM6"] = $("#txtActualDim6").val() ? $("#txtActualDim6").val() : null;

                var result = IsActualDimentionValid(dimValues);
                if (result.IsSuccess == false) {
                    var focusText = "";
                    if (result.InvalidDim == $("#txtActualDim1").val()) {
                        focusText = "txtActualDim1";
                    }
                    if (result.InvalidDim == $("#txtActualDim2").val() && focusText == "") {
                        focusText = "txtActualDim2";
                    }
                    if (result.InvalidDim == $("#txtActualDim3").val() && focusText == "") {
                        focusText = "txtActualDim3";
                    }
                    if (result.InvalidDim == $("#txtActualDim4").val() && focusText == "") {
                        focusText = "txtActualDim4";
                    }
                    if (result.InvalidDim == $("#txtActualDim5").val() && focusText == "") {
                        focusText = "txtActualDim5";
                    }
                    if (result.InvalidDim == $("#txtActualDim6").val() && focusText == "") {
                        focusText = "txtActualDim6";
                    }
                    var message = $("#lblErrorMessageE_HIKI_ENTRY_025").text().trim(); //ActDimOutOfTolerance
                    ShowMessageDialog(message, focusText, WARNING);
                    return false;
                }
            }

            if ($("#chkActualLength").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK && $("#txtLengthToleranceT1").val() != "") {
                var lengthValues = {};
                lengthValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;
                lengthValues["LENGTH_TOLERANCE"] = $("#txtLengthToleranceT1").val() ? $("#txtLengthToleranceT1").val() : null;
                lengthValues["LENGTH1"] = $("#txtActualLength1").val() ? $("#txtActualLength1").val() : null;
                lengthValues["LENGTH2"] = $("#txtActualLength2").val() ? $("#txtActualLength2").val() : null;
                lengthValues["LENGTH3"] = $("#txtActualLength3").val() ? $("#txtActualLength3").val() : null;


                var result = IsActualLengthValid(lengthValues);

                if (result.IsSuccess == false) {
                    var focusText = "";
                    if (result.InvalidLength == $("#txtActualLength1").val()) {
                        focusText = "txtActualLength1";
                    }
                    if (result.InvalidLength == $("#txtActualLength2").val() && focusText == "") {
                        focusText = "txtActualLength2";
                    }
                    if (result.InvalidLength == $("#txtActualLength3").val() && focusText == "") {
                        focusText = "txtActualLength3";
                    }
                    var message = $("#lblErrorMessageE_HIKI_ENTRY_026").text().trim(); // LengthOutOfTolerance
                    ShowMessageDialog(message, focusText, WARNING);
                    return false;
                }


            }

            var weightValues = {};
            weightValues["PRODUCT_WEIGHT"] = $("#txtQuantityT1").val() ? $("#txtQuantityT1").val() : null;
            weightValues["INPUT_WEIGHT"] = $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null;
            weightValues["PRODUCT_SHAPE"] = $("#txtProdShapeT1").val() ? $("#txtProdShapeT1").val() : null;
            weightValues["PRODUCT_DIM"] = $("#txtProdDimT1").val() ? $("#txtProdDimT1").val() : null;
            weightValues["NO_OF_UNITS"] = $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null;
            weightValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;

            if ($("#chkFinishedQuantity").prop("checked") == true) {

                if (["PRODUCT_SHAPE", "PRODUCT_DIM", "PRODUCT_LENGTH"].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== "")) {

                    var value = $("#txtLengthT1").val();
                    // Check - Length only allow numeric &  Unit Type - Not allow BN 

                    if (!IsNotNumeric(value) || value.trim() == "" || $("#ddlUnitType").val() == 2) {

                        var message = $("#lblErrorMessageE_HIKI_ENTRY_033").text().trim();
                        ShowMessageDialog(message, "chkFinishedQuantity", WARNING);
                        return false;
                    }

                } else {
                    var message = $("#lblErrorMessageE_HIKI_ENTRY_033").text().trim();
                    ShowMessageDialog(message, "chkFinishedQuantity", WARNING);
                    return false;
                }
            }

            var format = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            var getResponse = {};
            if ($("#ddlUnitType").val() == 2 && $("#chkFinishedQuantity").prop("checked") == false) {
                getResponse["IsSuccess"] = true;
            }else if (!format.test($("#txtLengthT1").val())) {
                getResponse = IsFinishedQuantityValid(weightValues);
            } else {
                getResponse["IsSuccess"] = true;
            }

           
            if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true) {

                var message = "";

                //message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_028").text());  //WeightIsInvalid

                //bootbox.confirm({
                //    title: "<div style='color:white'></div>",
                //    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                //    buttons: {
                //        confirm: {
                //            label: 'Yes',
                //            className: 'btn-success'
                //        },
                //        cancel: {
                //            label: 'No',
                //            className: 'btn-danger'
                //        }
                //    },
                //    callback: function (result) {

                //        if (result) {
                //            var message = $("#lblErrorMessageE_HIKI_ENTRY_027").text().trim(); // FinishedQtyNotIn2PercentRange
                //            showMessageDialog(message, "txtFinishedQuantity", WARNING);
                //        }

                //    },
                //    centerVertical: true
                //});
                 message = $("#lblErrorMessageE_HIKI_ENTRY_027").text().trim(); // FinishedQtyNotIn2PercentRange
                 ShowMessageDialog(message, "txtFinishedQuantity", WARNING); 

               
            } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false) {
                var message = "";
                
                    message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_028").text());  //WeightIsInvalid
               
                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {
                       
                        if (result) {

                            if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                                UpdateHikinukiDetails();
                                GetMachineDetails($("#hdfMachineCD").val());
                                DisableSaveButton();

                            } else
                            {

                                bootbox.confirm({
                                    title: "<div style='color:white'></div>",
                                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblConfirmationMessageC_HIKI_ENTRY_002").text()) + "</td></tr></table>",
                                    buttons: {
                                        confirm: {
                                            label: 'Yes',
                                            className: 'btn-success'
                                        },
                                        cancel: {
                                            label: 'No',
                                            className: 'btn-danger'
                                        }
                                    },
                                    callback: function (result2) {

                                        if (result2) {
                                            UpdateHikinukiDetails();
                                            GetMachineDetails($("#hdfMachineCD").val());
                                            DisableSaveButton();
                                        } else {
                                            $("#txtEndDate").focus();
                                        }

                                    },
                                    centerVertical: true
                                });

                            }
                           
                        }
                        else {
                            $("#txtFinishedQuantity").focus();
                          
                        }


                    },
                    centerVertical: true
                });

            } else {
               
                

                if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                    UpdateHikinukiDetails();
                    GetMachineDetails($("#hdfMachineCD").val());
                    DisableSaveButton();

                } else {

                    bootbox.confirm({
                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblConfirmationMessageC_HIKI_ENTRY_002").text()) + "</td></tr></table>",
                        buttons: {
                            confirm: {
                                label: 'Yes',
                                className: 'btn-success'
                            },
                            cancel: {
                                label: 'No',
                                className: 'btn-danger'
                            }
                        },
                        callback: function (result3) {

                            if (result3) {
                                UpdateHikinukiDetails();                                
                                GetMachineDetails($("#hdfMachineCD").val());
                                DisableSaveButton();
                            } else
                            {
                                $("#txtEndDate").focus();
                            }

                        },
                        centerVertical: true
                    });

                }
            }
        }
    });
function BindTPTestList() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetTPTestList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindTPTestDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function BindTPTestDropdown(jsonResult) {
    $("#ddlTPTest").html("");//.append($('<option></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindDieDiaShape() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetDieDiaShape",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDieDiaList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindDieDiaList(jsonResult) {
    $("#ddlDieShape").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDieShape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function BindUnitType() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetUnitType",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindUnitList(jsonResult) {
    $("#ddlUnitType").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function BindProcessDefectList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetProcessDefect",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindProcessList(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindProcessList(jsonResult) {
    $("#ddlNGReason1").html("").append($('<option></option>'));
    $("#ddlNGReason2").html("").append($('<option></option>'));
    $("#ddlNGReason3").html("").append($('<option></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindBendStatus() {

    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetBendStatus",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindBendStatusList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindBendStatusList(jsonResult) {
    $("#ddlBendStatus").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindExteriorCondition() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetExteriorCondition",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindExteriorList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindExteriorList(jsonResult) {
    $("#ddlExternalSurfaceCondition").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalSurfaceCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function BindDecision() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetDecision",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDecisionList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindDecisionList(jsonResult) {
    $("#ddlDecision").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindNextProcess() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindNextProcessList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}


function BindNextProcessList(jsonResult) {
  
    $("#ddlNextProcess").html("");//.append($('<option></option>'));

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));

        });
    }
    if ($("#hdfCultureName").val() == "ja-JP") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));

        });
    }

}
function IsNotNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
    return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
}
$(document).ready(function () {
   
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    $("#24HrsMachineData").css({ 'height': '845px' })
    window.resize(1920, 1030); /*window.resize(1920, 1080);*/

    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    SetGlobalVariables();
    
    SetDecimalFormats();
    HideToleranceErrorLabels();
    DisableStartDataPanel();
    DisableEndDataPanel();
    Reset24MachineGrid();
    ResetHistoryGrid();
    DisableDefaultField();
    $("#btnSave").attr("disabled", "disabled");
    $(".FocusDefault").focus();

    userName = $("#hdfUserName").val();

});


function ResetHistoryGrid()
{
    ClearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");
    ClearTable($("#grdvHistory"), "", "GridNormalRow");
    $grdvMessage = $("#grdvHistory");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    var $tr1 = $('<tr class="GridNormalRow" >').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle' colspan='2'>").text(""),
        $("<td align='center' valign='middle' rowspan='4'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle' colspan='3'>").text(""),
        $("<td align='center' valign='middle' colspan='2'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle' rowspan='4'>").text(""),
        $("<td align='center' valign='middle' rowspan='4'>").text(""),
        $("<td align='center' valign='middle' rowspan='2' colspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='2' colspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='2' colspan='2'>").text(""),
        $("<td align='center' valign='middle' rowspan='2'>").text(""),
        $("<td align='center' valign='middle' rowspan='2'>").text(""),
        $("<td align='center' valign='middle' rowspan='4'>").text(""),
        $("<td align='center' valign='middle' rowspan='4'>").text("")
    );
    var $tr2 = $('<tr class="GridNormalRow" >').append(
        $("<td align='center' valign='middle' colspan='2' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='2'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
        $("<td align='center' valign='middle' rowspan='3'>").text(""),
      
    );
    var $tr3 = $('<tr class="GridNormalRow" >').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle' colspan='2'>").text("")

    );
    var $tr4 = $('<tr class="GridNormalRow" >').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle' colspan='4'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text("")
     
    );
    $grdvMessage.append($tr1);
    $grdvMessage.append($tr2);
    $grdvMessage.append($tr3);
    $grdvMessage.append($tr4);
}
function Reset24MachineGrid() {

    ClearTable($("#grdvHikinukiMachine"), "ItemGrdvHikinukiMachine", "");
    $grdvMessage = $("#grdvHikinukiMachine");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows
   
    var $tr1 = $('<tr class="GridNormalRow" style="height:18px;">').append(
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
       
        $("<td align='center' valign='middle'>").text(""),
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td colspan='2' align='center' valign='middle'>").text(""),
        $("<td  align='center' valign='middle'>").text(""),
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td colspan='3' align='center' valign='middle'>").text(""),
        $("<td colspan='3' align='center' valign='middle'>").text(""),
        $("<td colspan='2' align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text("")
        
    );
    var $tr2 = $('<tr class="GridNormalRow" style="height:18px;">').append(
        $("<td rowspan='2'  align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td colspan='2' align='center' valign='middle'>").text(""),
       

    );
    var $tr3 = $('<tr class="GridNormalRow" style="height:18px;">').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td colspan='4' align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text("")
    );
    
   
    $grdvMessage.append($tr1);
    $grdvMessage.append($tr2);
    $grdvMessage.append($tr3);
   
   
}
function ResetProdPointGrid() {
    
    $grdvMessage = $("#grdvProdPointData");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

   
    var $tr1 = $('<tr class="GridNormalRow" >').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        
       
    );
  
    $grdvMessage.append($tr1);
   
}


$("#btnStartDateTime").click(function () {
    SetStartSysDateTime();
});

$("#btnEndDateTime").click(function () {
    SetEndSysDateTime();
});
function IsDateValid(boolVal) {
   
    var returnval = false;
    var getDate;
    if (boolVal === true) {
        getDate = ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

    } else {
        getDate = ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    }


    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });




    return returnval;
}


//validate Or.no check comman method 
function ValidateChubban(tabName, chuuban, gyouban) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/ValidateChubban",
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
                DisableStartDataPanel();
                // Chuuban is marked as completed in the system/
                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_004").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                DisableStartDataPanel();
                DisableSaveButton();
               // ClearPanelData();
                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                var message = $("#lblErrorMessageE_HIKI_ENTRY_005").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(message, "", WARNING);
                // ClearData();
                return false;
            }
            else if (result.responce == 1) {
                if (tabName == "Registration") {
                    GetHikinukiDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val());
                }
                if (tabName == "PastHistory") {
                    SetPastHistory();
                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
$("#ddlNextProcess").focusout(function () {
    $("#btnSave").focus();
}); 

$("#ReDrawing").focusout(function () {
   
    if ($("#txtEndDate").attr(DISABLED)) {
        $("#txtThaba1").focus();
    } else
    {
        $("#txtEndDate").focus();
    }
  
});
$("#txtThaba4").focusout(function () {
    if ($("#txtDisposalBars").attr(DISABLED))
    {
        $("#btnSave").focus();
    }
});

$("#btnSearchT2").click(function () {

//    resetECHistoryGrid();
    var errorMessage = "";
    var firstFieldID = "";
    var chuuban = $("#txtChubanT2").val();
    var gyouban = $("#txtGyobanT2").val();


    ClearPanel($("#TbHistorySearch"));
    ResetHistoryGrid();
    $("#txtChubanT2").val(chuuban);
    $("#txtGyobanT2").val(gyouban);

    if ($.trim($("#txtChubanT2").val()) == "") {
        errorMessage = $("#txtChubanT2").attr("ErrorLabel")
        firstFieldID = "txtChubanT2";
    }

    if ($.trim($("#txtGyobanT2").val()) == "") {
        if (errorMessage == "") {
            errorMessage = $("#txtGyobanT2").attr("ErrorLabel")
        } else {
            errorMessage = errorMessage + "<br>" + $("#txtGyobanT2").attr("ErrorLabel")
        }
        if (firstFieldID == "") {
            firstFieldID = "txtGyobanT2";
        }
    }

    if (errorMessage != "") {
        ShowMessageDialog(errorMessage, firstFieldID, WARNING);
        return false;
    }

    ValidateChubban("PastHistory", $("#txtChubanT2").val(), $("#txtGyobanT2").val());
   
});
function SetPastHistory() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/getHikinukiHistoryByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({ CHUUBAN: $("#txtChubanT2").val(), GYOUBAN: $("#txtGyobanT2").val(), CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;


            if (jsonResult === null) {

                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChubanT2", WARNING);
                return false;
            }


            // Chuuban is marked as completed in the system
            if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {

                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_004").text();
                ShowMessageDialog(errorMessage, "txtChubanT2", WARNING);
                return false;
            }
        
            var jsonObj = jsonResult.procesS_HISTORY;
            if (jsonResult != null && jsonObj == null) {

                var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChubanT2", WARNING);
                return false;
            }

       
            SetChuubanDetailsOnHistoryTab(jsonResult);
            SetHistoryGrid(jsonResult);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
$('#txtChuuban2').keypress(function (e) {
    var orderNo = $('#txtChuuban2').val() ? $('#txtChuuban2').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HikinukiCoilToBar/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuuban2').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtGyouban2').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btnChuubanSearch').focus();
                } else {
                    $('#txtChuuban2').val("");
                    $('#txtGyouban2').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});
$('#txtChubanT2').keypress(function (e) {
    var orderNo = $('#txtChubanT2').val() ? $('#txtChubanT2').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HikinukiCoilToBar/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChubanT2').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtGyobanT2').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btnSearchT2').focus();
                } else {
                    $('#txtChubanT2').val("");
                    $('#txtGyobanT2').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HIKI_ENTRY_014").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});
function DisableSavedThaba()
{
    if (parseInt($("#txtThaba1").val()) > 0) {
        $("#txtThaba1").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
    if (parseInt($("#txtThaba2").val()) > 0) {
        $("#txtThaba2").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
    if (parseInt($("#txtThaba3").val()) > 0) {
        $("#txtThaba3").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
    if (parseInt($("#txtThaba4").val()) > 0) {
        $("#txtThaba4").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
}