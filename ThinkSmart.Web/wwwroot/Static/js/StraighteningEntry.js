
var $btnChuubanSearch;
var $btnSave;
var $btnSearchT2;
var indexDDLDecisionNothing = 0;
var combinedDecisionOK = 1;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var DISABLED = "disabled";
var dbOperation; // Value will set to "Insert" or "Update" at runtime
var $hdfSYSDATE;
var $hdfCultureLoaded;
var $hdfCultureEN;
var $hdfCultureJP;
var straighteningCD;
var registeredThabaNumbersAndDecision = {};
var maxRegisteredThabaNo = 0;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var lengthValue = true;
var finishedWeightValue = true;
var DimentionValue = true;
let enterPressedMachine = false;
var BNO;
var machineModified = false;
var operatorStartModified = false;
var operatorEndModified = false;



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
//var $txtGyouban;



var machineGridDataTr1 = " <tr id='tr1ItemGrdvStraighteningMachine'> "
    + " <td class='td tdH2' rowspan='2' style='background-color:COLOR; color:#E2E2E2;'> "
    + " <span class='txtColor' style='background-color:COLOR; color:#E2E2E2; height:80px;width:35px'>&nbsp;</span>"     //  + " <input name='txtGridDecisionColor' type='text' class='txtGridDecisionColor' style='height:30px;width:5px '>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2'  style='text-align:left'>"   //  + " <td class='td tdH2'  style='text-align:left'>"
    + " <span class='txtGridCustomerName labelClass nitesh'> valtxtGridCustomerName</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:center'>"
    + " <span class='txtGridChuban labelClass' >valtxtGridChuban</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:left'>"
    + " <span class='txtMetallicPlateCode labelClass' >valtxtMetallicPlateCode</span></td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:left'>"
    + " <span class='txtProdNameCust labelClass' style='text-align:left'>valtxtProdNameCust</span>"
    + " </td>"
    + "<td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:left'> "
    + "<span class='txtGridCharge labelClass' >valtxtGridCharge</span> "
    + "</td> "
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:right'>"
    + " <span class='txtGridDimDShape labelClass' >valtxtGridDimDShape</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:center'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimTol</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:center'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridTabaNo1_2</span> <br>"
    + "<span class='txtGridTabaNo3 labelClass'  >valtxtGridTabaNo3_4</span> "
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:left' colspan='2'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimBefore1</span>"
    + " </td>"
    //+ " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:right'>"
    //+ "  <span class='txtGridTabaNo labelClass' >valtxtGridDimAfter1</span>"
    //+ " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:right'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridLengthAfter1</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:right'>"
    + " <span class='txtGridNoOfCoils labelClass' >valtxtGridNoOfCoils</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:right'>"
    + " <span class='txtGridQuantity labelClass' >valtxtGridQuantity</span>"
    + " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' rowspan='2' style='text-align:left' >"
    + " <span class='txtGridDieDim labelClass'>valtxtGridRemarks</span>"
    + " </td>"
    + "  </tr>";


var machineGridDataTr2 = "<tr id='tr2ItemGrdvStraighteningMachine'>"

    // + "  <td class='td tdH2' > "
    //+ "<span class='txtGridSpecification labelClass bagal' >valtxtGridCustomerName</span> "
    //+ "</td> "
    + "  <td class='td tdH2 tdNoWrap padddingLeftRight'> "
    + "<span class='txtGridSpecification labelClass' >valtxtGridSpecification</span> "
    + "</td> "

    + "<td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:right'> "
    + "<span class='txtGridLength labelClass' >valtxtGridLength</span> "
    + "</td> "
    + "<td class='td tdH2 tdNoWrap padddingLeftRight'> "
    + "<span class='txtGridLength labelClass' >valtxtGridLengthTol</span> "
    + "</td> "
    //  + "<td class='td tdH2'> "
    //  + "<span class='txtGridTabaNo3 labelClass' >valtxtGridTabaNo3_4</span> "
    // + "</td> "
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:left' colspan='2'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimAfter2</span>"
    + " </td>"
    //+ " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:right'>"
    //+ "  <span class='txtGridTabaNo labelClass' >valtxtGridDimAfter2</span>"
    //+ " </td>"
    + " <td class='td tdH2 tdNoWrap padddingLeftRight' style='text-align:right'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridLengthAfter2</span>"
    + " </td>"
    + " </tr> ";

var processHistoryGridataRow1 = " <tr id='tr1ItemGrdvProcessHistory'> "
    //+ " <td class= 'td GridAlignmentChuuban tdNoWrap padddingLeftRight' rowspan= '2 '  > "
    //+ " <span id= 'lblGridProcLocationT2' class= 'txtChuubaGyouban labelClass' style= 'color:Black; '> </span> "
    //+ " </td> "

    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridProcLocationT2' class= 'txtChuubaGyouban labelClass' style= 'color:Black; '>valtxtProcLocation</span> "
    + " </td> "
    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight' > "
    + " <span id= 'lblGridOperationDayStartT2' class='GridCenter txtStartDateTime1 labelClass ' style= 'display:inline-block;color:Black;height:21px; padding-top:3px;'>valtxtOperationDayStart</span> "
    + " </td> "
    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridOperationDayEnd' class= 'txtStartOperator labelClass ' style= 'color:Black; '>valtxtOperationDayEnd</span> "
    + " </td> "

    + " <td class= 'td GridAlignmentCharge tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridCustomerT2 ' class= 'txtCharge labelClass ' style= 'color:Black; '>valtxtCustomer</span> "
    + " </td> "

    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight' > "
    + " <span id= 'lblGridChuubanGyoubanT1 ' class= 'txtMetallicLabelNo labelClass ' style= 'color:Black; '>valtxtChuubanGyouban</span> "
    + " </td> "

    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridVerification_T2 ' class= 'txtGridStrDimBefore labelClass ' style= 'color:Black; '>valtxtProdNameCust</span> "
    + " </td> "
    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridVerification_T2 ' class= 'txtGridStrDimBefore labelClass ' style= 'color:Black; '>valtxtCharge</span> "
    + " </td> "
    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridVerification_T2 ' class= 'txtGridStrDimBefore labelClass ' style= 'color:Black; '>valtxtMatelicPlatNo</span> "
    + " </td> "
    + " <td class= 'td GridRight tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridStrDimD_T2 ' class= 'txtGridStrDimBefore labelClass ' style= 'color:Black; '>valtxtDimD</span> "
    + " </td> "

    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridStrDimTolerance_T2 ' class= 'txtGridStrLenBefore labelClass ' style= 'color:Black; '>valtxtstrDimTolerance</span> "
    + " </td> "
    + " <td class= 'td GridAlignmentLength tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridStrActNoUnits_T2 ' class= 'txtGridStrLenBefore labelClass ' style= 'color:Black; '>valtxtstrActNoUnits</span> "
    + " </td> "

    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridThabaNo_T2 ' class= 'txtGridExternalSurfaceCondition labelClass ' style= 'color:Black; '>valtxtThabaNo</span> "
    + " </td> "
    + " <td class= 'td GridAlignmentBendStatus tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridAfterStraightening1_T2 ' class= 'txtGridBendStatus labelClass ' style= 'color:Black; '>valtxtAfterStraightening1</span> "
    + " </td> "

    + " <td class= 'td GridRight tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridstrActualLength_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtstrActualLength</span> "
    + " </td> "
    + " <td class= 'td GridRight tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridstrNoofUnits_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtstrNoofUnits</span> "
    + " </td> "
    + " <td class= 'td GridRight tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridstrWeightWithoutKG_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtstrWeightWithoutKG</span> "
    + " </td> "
    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridstrRemark_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtstrRemark</span> "
    + " </td> "

    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridstrDecision_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtstrDecision</span> "
    + " </td> "
    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight' rowspan='2'> "
    + " <span id= 'lblGridstrDecision_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtstrNextProcess</span> "
    + " </td> "
    

    + " </tr>  ";


var processHistoryGridataRow2 = " <tr id='tr2ItemGrdvProcessHistory'> "
    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight' colspan= '2'> "
    + "     <span id= 'lblGridHTMachineT2 ' class= 'txtEndDatetime labelClass ' style= 'color:Black; '>valtxtHTMachine</span> "
    + " </td> "
    + " <td class= 'td GridAlignmentOperator tdNoWrap padddingLeftRight'> "
    + "     <span id= 'lblGridHTFurnaceVerifyByT2 ' class= 'txtEndOperator labelClass ' style= 'color:Black; '>valtxtOperatorName</span> "
    + " </td> "

    + " <td class= 'td GridCenter tdNoWrap padddingLeftRight' > "
    + "     <span id= 'lblGridHTManufacturingSpecificationT2 ' class= 'txtUpperLowerRollerAngle1 labelClass ' style= 'color:Black; '>valtxtSpec_No</span> "
    + " </td> "
    + " <td class= 'td GridAlignmentUpperLowerRoller tdNoWrap padddingLeftRight'> "
    + "     <span id= 'lblGridHTDiaT2 ' class= 'txtUpperLowerRollerAngle2 labelClass ' style= 'color:Black; '>valtxtProd_Len</span> "
    + " </td> "

    + " <td class= 'td GridAlignmentUpperRoller tdNoWrap padddingLeftRight'> "
    + "     <span id= 'lblGridHTRollMonthT2 ' class= 'txtUpperRollerElevation labelClass ' style= 'color:Black; '>valtxtLenTol</span> "
    + " </td> "
    + " <td class= 'td GridAlignmentCompressionTonnage tdNoWrap padddingLeftRight'> "
    + "     <span id= 'lblGridHTCoilNo' class= 'txtCompressionTonnage labelClass ' style= 'color:Black; '>valtxtACTAlloc_qty</span> "
    + " </td> "

    + " <td class= 'td GridLeft tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridHTProcCondition_T2 ' class= 'txtGridStrDimAfter labelClass ' style= 'color:Black; '>valtxtDIm_Before</span> "
    + " </td> "
    + " <td class= 'td GridAlignmentDimD tdNoWrap padddingLeftRight'> "
    + " <span id= 'lblGridHTPatternNo_T2 ' class= 'txtGridStrDimAfter labelClass ' style= 'color:Black; '>valtxtAllDimAfter</span> "
    + " </td> "

    //+ " <td class= 'td GridAlignmentAEdgeDistortion tdNoWrap padddingLeftRight'> "
    //+ " <span id= 'lblGridNextProcess_T2 ' class= 'txtGridAEdgeDistortion labelClass ' style= 'color:Black; '>valtxtStrLenAfter</span> "
    //+ " </td> "
    + " </tr> ";
    

function setGlobalVariables() {
    $btnChuubanSearch = $("#btnChuubanSearch");
    $btnSave = $("#btnSave");
    $btnSearchT2 = $("#btnSearchT2");
    $hdfSYSDATE = $("#hdfSYSDATE");
    $hdfCultureLoaded = $("#hdfCultureLoaded");
    $hdfCultureEN = $("#hdfCultureEN");
    $hdfCultureJP = $("#hdfCultureJP");
}

function resetGlobalVariables() {
    $("#hdfDBOperation").val("");
    dimTolMin = "";
    dimTolMax = "";
    dimStd = "";
    lenTolMin = "";
    lenTolMax = "";
    lenStd = "";
    registeredThabaNumbersAndDecision = {};
    maxRegisteredThabaNo = 0;
}

function setDecimalFormats() {
    $("#txtAEdgeDistortion1, #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtBEdgeDistortion1, #txtBEdgeDistortion2, #txtBEdgeDistortion3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtUpperLowerRollerAngle1, #txtUpperLowerRollerAngle2, #txtSTRTonnage").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });

    $("#txtC, #txtMn, #txtUpperRoleElevation, #txtAEdgeDistortion1,  #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtAEdgeDistortion4,#txtBEdgeDistortion1,  #txtBEdgeDistortion2, #txtBEdgeDistortion3, #txtBEdgeDistortion4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtDieDim, #txtActualDimDBeforeStraightening1, #txtActualDimDBeforeStraightening2, #txtActualDimDBeforeStraightening3, #txtActualDimDBeforeStraightening4, #txtActualDimDBeforeStraightening5,#txtActualDimDBeforeStraightening6,#txtActualDim1, #txtActualDim2, #txtActualDim3, #txtActualDim4, #txtActualDim5, #txtActualDim6").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtActualLength1BeforeStraightening, #txtActualLength2BeforeStraightening, #txtActualLength1AfterStraightening,#txtActualLength2AfterStraightening").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
    });

    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
}

function initialize() {

    enableSaveButton();
    enableSearchPanel();
    $(".FocusDefault").focus();
    disableStartDataPanel();
    disableEndDataPanel();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlCustomerDetails")); // Tab 2
    clearPanel($("#divProductDimensionDetails")); // Tab 2
    clearPanel($("#divProcessPlanDetails")); // Tab 2
    clearPanel($("#divMaterialDetails")); // Tab 2
    clearPanel($("#pnlProcessInfo")); // Tab 4
    clearPanel($("#pnlTestSpecifications")); // Tab 4    
    clearThabaDetailsGrid();
    //clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");    // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
    //clearTable($("#gridProcessHistory "), "GridNormalRow");// Tab 2
    clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");

    $("#txtNifudaNo").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $('#txtMachineType').val("");
    $("#txtNifudaNo1").val("");

    hideToleranceErrorLabels();
    resetStraighteningMachineDetailsGrid();
    resetHistoryDetailsGrid();
    resetGlobalVariables();
    resetMessageDetailsGrid();
    resetProcessPlanDetailsGrid();
    disableSaveButton();
    enableSearchOrderNoPanel();

}

function ClearData() {
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProductDetails"));
    disableSaveButton();
    disableStartDataPanel();
    disableEndDataPanel();
    clearThabaDetailsGrid();
    //clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");    // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
    //clearTable($("#gridProcessHistory "), "GridNormalRow");// Tab 2
    clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");
    enableSearchOrderNoPanel();
    // $(".FocusDefault").focus();
}

function actualDimTxtBoxClassChange(ActualDimLength) {


    var txtActualDim4 = document.getElementById("txtActualDim4");
    var txtActualDimLength4 = txtActualDim4.value.length;
    var txtActualDim5 = document.getElementById("txtActualDim5");
    var txtActualDimLength5 = txtActualDim5.value.length;
    var txtActualDim6 = document.getElementById("txtActualDim6");
    var txtActualDimLength6 = txtActualDim6.value.length;

    var chkActualDim = document.getElementById("chkActualDim");
    if (chkActualDim.checked) {


        if (txtActualDimLength4 > 0 || txtActualDimLength5 > 0 || txtActualDimLength6 > 0) {
            ChangeControlDisplay('#txtActualDim4', true, 'TextFieldsClassN', 'MandatoryRigntAlignTextBox', 0);
            ChangeControlDisplay('#txtActualDim5', true, 'TextFieldsClassN', 'MandatoryRigntAlignTextBox', 0);
            ChangeControlDisplay('#txtActualDim6', true, 'TextFieldsClassN', 'MandatoryRigntAlignTextBox', 0);
        }
        else {
            ChangeControlDisplay('#txtActualDim4', true, 'MandatoryRigntAlignTextBox', 'TextFieldsClassN', 0);
            ChangeControlDisplay('#txtActualDim5', true, 'MandatoryRigntAlignTextBox', 'TextFieldsClassN', 0);
            ChangeControlDisplay('#txtActualDim6', true, 'MandatoryRigntAlignTextBox', 'TextFieldsClassN', 0);
        }
    }
    else {
        ChangeControlDisplay('#txtActualDim4', true, 'MandatoryRigntAlignTextBox', 'TextFieldsClassN', 0);
        ChangeControlDisplay('#txtActualDim5', true, 'MandatoryRigntAlignTextBox', 'TextFieldsClassN', 0);
        ChangeControlDisplay('#txtActualDim6', true, 'MandatoryRigntAlignTextBox', 'TextFieldsClassN', 0);
    }
}


function OKQtyRegionClassChange() {

    var txtNGWeight1 = $("#txtNGQuantity1").val();
    var ddlDefects1 = document.getElementById("ddlDefects1");
    if (txtNGWeight1 > 0) {
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason1").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason1").removeClass("Mandatory");
        $("#ddlNGReason1 option:selected").prop("selected", false);
        $("#ddlNGReason1").attr("disabled", "disabled");
    }
    var txtNGWeight2 = $("#txtNGQuantity2").val();
    var ddlDefects2 = document.getElementById("ddlDefects2");
    if (txtNGWeight2 > 0) {
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason2").removeAttr("disabled")
    }
    else {

        $("#ddlNGReason2").removeClass("Mandatory");
        $("#ddlNGReason2 option:selected").prop("selected", false);
        $("#ddlNGReason2").attr("disabled", "disabled");
    }
    var txtNGWeight3 = $("#txtNGQuantity3").val();
    var ddlDefects3 = document.getElementById("ddlDefects3");
    if (txtNGWeight3 > 0) {
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason3").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason3").removeClass("Mandatory");
        $("#ddlNGReason3 option:selected").prop("selected", false);
        $("#ddlNGReason3").attr("disabled", "disabled");
    }
}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

function setThabaDetails(jsonResult) {
    debugger;
    // console.log(jsonResult);

    clearThabaDetailsGrid();
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

                var decisionLegend = $(decisionLegends[decision_value]);
                $("#lblThabaNo" + thabaNo).text(thabaNo);
                $("#lblDecisionNo" + thabaNo).html("").append(decisionLegend);
            });
        }
    });


    //  $("#txtFinalRegisteredThabaNoValue").text(maxRegisteredThabaNo);
}


function Set24HrMachineData(jsonResult) {
    clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");

    $.each(jsonResult, function (index, jsonObject) {
        if (jsonObject.stR_MACHINE_CD) {
            var firstRow = machineGridDataTr1;
            if (jsonObject.stR_DECISION != null) {
                if (jsonObject.stR_DECISION == 1) {
                    firstRow = firstRow.replace("COLOR", "green");    // $("●").css('color', 'green');             

                    $(".tdFirst").css('background-color', 'green'); style = 'color:Blue;'
                    firstRow = firstRow.replace("#E2E2E2", "green");
                }
                else if (jsonObject.stR_DECISION == 2) {
                    firstRow = firstRow.replace("COLOR", "red");    //firstRow = $("●").css('color', 'red');
                    firstRow = firstRow.replace("#E2E2E2", "red");
                }
                else if (jsonObject.stR_DECISION == 3) {
                    firstRow = firstRow.replace("COLOR", "blue");    //firstRow = $("●").css('color', 'blue');
                    firstRow = firstRow.replace("#E2E2E2", "blue");
                }
                else if (jsonObject.stR_DECISION == 4) {
                    firstRow = firstRow.replace("COLOR", "Yellow");  //firstRow = $("●").css('color', 'Yellow');
                    firstRow = firstRow.replace("#E2E2E2", "Yellow");
                }
                else if (jsonObject.stR_DECISION == 5) {
                    firstRow = firstRow.replace("COLOR", "darkred");  
                    firstRow = firstRow.replace("#E2E2E2", "darkred");
                }
                else {
                    firstRow = firstRow.replace("COLOR", "White");   // firstRow = $("●").css('color', 'White');
                    firstRow = firstRow.replace("#E2E2E2", "White");
                }
            }




            // old code
            //if (jsonObject.pP_CUSTOMER_NAME != null)                    
            //    firstRow = firstRow.replace("valtxtGridPrcoessStatus", "");        // [30/08/2017] Sagar B. replaced "STR" with ""
            //else
            //    firstRow = firstRow.replace("valtxtGridPrcoessStatus", "");
            if (jsonObject.pP_CUSTOMER_NAME != null)
                firstRow = firstRow.replace("valtxtGridCustomerName", jsonObject.pP_CUSTOMER_NAME);        // [11/10/2019] Nitesh B.  replaced "STR" with " value"
            else
                firstRow = firstRow.replace("valtxtGridCustomerName", "");


            if (jsonObject.pP_CHUUBAN_GYOUBAN != null)
                firstRow = firstRow.replace("valtxtGridChuban", jsonObject.pP_CHUUBAN_GYOUBAN);
            else
                firstRow = firstRow.replace("valtxtGridChuban", "");

            if (jsonObject.stR_METALLIC_PLATE_CODE != null)
                firstRow = firstRow.replace("valtxtMetallicPlateCode", jsonObject.stR_METALLIC_PLATE_CODE);
            else
                firstRow = firstRow.replace("valtxtMetallicPlateCode", "");

            if (jsonObject.pP_PROD_NAME_CUST != null)
                firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
            else
                firstRow = firstRow.replace("valtxtProdNameCust", "");

            if (jsonObject.pP_CHARGE != null)
                firstRow = firstRow.replace("valtxtGridCharge", jsonObject.pP_CHARGE);
            else
                firstRow = firstRow.replace("valtxtGridCharge", "");


            if (jsonObject.pP_PROD_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridDimDShape", jsonObject.pP_PROD_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridDimDShape", "");

            if (jsonObject.pP_DIM_TOLERANCE != null)
                firstRow = firstRow.replace("valtxtGridDimTol", jsonObject.pP_DIM_TOLERANCE);
            else
                firstRow = firstRow.replace("valtxtGridDimTol", "");

            if (jsonObject.stR_TABA_NO_1_2 != null)
                firstRow = firstRow.replace("valtxtGridTabaNo1_2", jsonObject.stR_TABA_NO_1_2);
            else
                firstRow = firstRow.replace("valtxtGridTabaNo1_2", "");
            if (jsonObject.stR_TABA_NO_3_4 != null)
                firstRow = firstRow.replace("valtxtGridTabaNo3_4", jsonObject.stR_TABA_NO_3_4);
            else
                firstRow = firstRow.replace("valtxtGridTabaNo3_4", "");

            if (jsonObject.stR_DIM_BEFORE_STRING != null)
                firstRow = firstRow.replace("valtxtGridDimBefore1", jsonObject.stR_DIM_BEFORE_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDimBefore1", "");

            if (jsonObject.stR_DIM_AFTER_STRING != null)
                firstRow = firstRow.replace("valtxtGridDimAfter1", jsonObject.stR_DIM_AFTER_1);
            else
                firstRow = firstRow.replace("valtxtGridDimAfter1", "");

            if (jsonObject.stR_LENGTH_AFTER_1 != null)
                firstRow = firstRow.replace("valtxtGridLengthAfter1", jsonObject.stR_LENGTH_AFTER_1);
            else
                firstRow = firstRow.replace("valtxtGridLengthAfter1", "");

            if (jsonObject.stR_BARS != null)
                firstRow = firstRow.replace("valtxtGridNoOfCoils", jsonObject.stR_BARS_DECODE);

            else
                firstRow = firstRow.replace("valtxtGridNoOfCoils", "");

            if (jsonObject.stR_QTY != null && jsonObject.stR_QTY != 0)
                firstRow = firstRow.replace("valtxtGridQuantity", jsonObject.stR_QTY);
            else
                firstRow = firstRow.replace("valtxtGridQuantity", "");

            if (jsonObject.stR_REMARKS != null)
                firstRow = firstRow.replace("valtxtGridRemarks", jsonObject.stR_REMARKS);
            else
                firstRow = firstRow.replace("valtxtGridRemarks", "");



            $("#grdvStraighteningMachine").append($(firstRow));



            // tr2    (Second row)

            var secondtRow = machineGridDataTr2;


            //if (jsonObject.pP_CUSTOMER_NAME != null)
            //    secondtRow = secondtRow.replace("valtxtGridCustomerName", jsonObject.pP_CUSTOMER_NAME);
            //else
            //    secondtRow = secondtRow.replace("valtxtGridCustomerName", "");

            if (jsonObject.pP_PROD_SPEC_NO != null)
                secondtRow = secondtRow.replace("valtxtGridSpecification", jsonObject.pP_PROD_SPEC_NO);
            else
                secondtRow = secondtRow.replace("valtxtGridSpecification", "");

            if (jsonObject.pP_PROD_LENGTH_STRING != null)
                secondtRow = secondtRow.replace("valtxtGridLength", jsonObject.pP_PROD_LENGTH_STRING);
            else
                secondtRow = secondtRow.replace("valtxtGridLength", "");

            if (jsonObject.pP_LEN_TOL != null)
                secondtRow = secondtRow.replace("valtxtGridLengthTol", jsonObject.pP_LEN_TOL);
            else
                secondtRow = secondtRow.replace("valtxtGridLengthTol", "");


            //if (jsonObject.stR_TABA_NO_3_4 != null)
            //    secondtRow = secondtRow.replace("valtxtGridTabaNo3_4", jsonObject.stR_TABA_NO_3_4);
            //else
            //    secondtRow = secondtRow.replace("valtxtGridTabaNo3_4", "");

            //if (jsonObject.stR_DIM_BEFORE_2 != null)
            //    secondtRow = secondtRow.replace("valtxtGridDimBefore2", jsonObject.stR_DIM_BEFORE_2);
            //else
            //    secondtRow = secondtRow.replace("valtxtGridDimBefore2", "");

            if (jsonObject.stR_DIM_AFTER_STRING != null)
                secondtRow = secondtRow.replace("valtxtGridDimAfter2", jsonObject.stR_DIM_AFTER_STRING);
            else
                secondtRow = secondtRow.replace("valtxtGridDimAfter2", "");

            if (jsonObject.stR_LENGTH_AFTER_2 != null)
                secondtRow = secondtRow.replace("valtxtGridLengthAfter2", jsonObject.stR_LENGTH_AFTER_2);
            else
                secondtRow = secondtRow.replace("valtxtGridLengthAfter2", "");

            $("#grdvStraighteningMachine").append($(secondtRow));
        }
    });

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
        url: APIURL + "/Straightening/GetThabaDetailsByChuubanGyouban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonThabaDetailData + "}"
        data: JSON.stringify({ chuuban: chuuban, gyouban: String(gyouban) })
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

        });

}

function toggleUnitTypeFieldsEnabled($this) {
    //if ($this.find(":selected").val() == '1') {
    //    $("#chkFinishedQuantity").removeAttr("disabled");
    //    $("#chkFinishedQuantity").prop('checked', true);
    //} else {
    //    $("#chkFinishedQuantity").attr("disabled", "disabled");
    //    $("#chkFinishedQuantity").prop('checked', false);
    //}
}

function toggleActualLengthFieldsMandatory($this) {
    if ($this.is(':checked')) {
        $("#txtActualLength1AfterStraightening").addClass("Mandatory");
        $("#txtActualLength2AfterStraightening").addClass("Mandatory");
    } else {
        $("#txtActualLength1AfterStraightening").removeClass("Mandatory");
        $("#txtActualLength2AfterStraightening").removeClass("Mandatory");
    }
}

function toggleActualDimFieldsMandatory($this) {

    if ($this.is(':checked')) {
        $("#txtActualDim1").addClass("Mandatory");
    } else {
        $("#txtActualDim1").removeClass("Mandatory");
    }

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

function clearThabaDetailsGrid() {
    $pnlThabaDetails = $("#pnlThabaDetails");
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

function setStartDataPanelDefaultData(jsonResult) {

    if (jsonResult.dW_START_DATE_STRING != null) {
        var startDateTime = jsonResult.dW_START_DATE_STRING;
        //$("#txtStartDate").val($.format.date(new Date(startDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
        $("#txtStartDate").val($.format.date(new Date(startDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        //$("#txtStartTime").val($.format.date(new Date(startDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    }
    else {
        $("#btnStartDateTime").click();
    }

    $("#txtNoOfCoils").val(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtCoilQuantity").val(jsonResult.pP_ACT_ALLOC_QTY);
}

function enableStartDataPanel(jsonResult) {

    var $startDataPanel = $("#pnlProcessStartData");

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

    // $("#txtStartDate").val(new Date($hdfSYSDATE.val()).format("yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    // $("#txtStartTime").val(new Date($hdfSYSDATE.val()).format("HH:mm")).mask("99:99", { placeholder: "hh:mm" });



}

function enableEndDataPanel() {

    var $endDataPanel = $("#pnlProcessEndData");


    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(disabled).removeAttr(disabled);

        }
    });

    $.each($endDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(disabled).removeAttr(disabled);
        }
    });

    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(disabled).removeAttr(disabled);
            $(checkBox).prop("checked", true);
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(disabled).removeAttr(disabled);
        }
    });

    // Set default values
    $("#chkActualDim").prop('checked', true);
    toggleActualDimFieldsMandatory($("#chkActualDim"));

    $("#chkActualLength").prop('checked', true);
    toggleActualLengthFieldsMandatory($("#chkActualLength"));

    /*  $('#ddlUnitType option:eq(0)').prop('selected', true);*/
    toggleUnitTypeFieldsEnabled($("#ddlUnitType"));

    //$("#txtEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //$("#txtEndTime").mask("99:99", { placeholder: "hh:mm" });


    /* $('#ddlUnitType option:eq(0)').prop('selected', true);*/
    $('#ddlBendStatus option:eq(0)').prop('selected', true);
    // $('#ddlTPTest option:eq(0)').prop('selected', true);

    $('#ddlExternalSurfaceCondition option:eq(0)').prop('selected', true);

    SetLengthControlProperties($("#txtLengthT1").val());
}

function enableSearchPanel() {
    $("#txtMachineType").removeAttr(disabled).removeClass(disabled);
    $("#txtNifudaNo").removeAttr(disabled).removeClass(disabled);
    $("#txtChuuban2").removeAttr(disabled).removeClass(disabled);
    $("#txtGyouban2").removeAttr(disabled).removeClass(disabled);
    $("#btnChuubanSearch").removeAttr(disabled).removeClass(disabled);
    $("#txtNifudaNo1").removeAttr(disabled).removeClass(disabled);
}

function disableSearchPanel() {
    $("#txtMachineType").attr(disabled, disabled).addClass(disabled);
    $("#txtNifudaNo").attr(disabled, disabled).addClass(disabled);
    $("#txtChuuban2").attr(disabled, disabled).addClass(disabled);
    $("#txtGyouban2").attr(disabled, disabled).addClass(disabled);
    $("#btnChuubanSearch").attr(disabled, disabled).addClass(disabled);
    $("#txtNifudaNo1").attr(disabled, disabled).addClass(disabled);
}


function disableSaveButton() {
    $("#btnSave").attr("DISABLED", "DISABLED");
    // $("#btnUpdate").attr("disabled", "disabled");
}

function enableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
}

function disableStartDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#pnlProcessStartData");
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
}

function disableEndDataPanel() {
    // Set EndData Panel Elements to Disabled
    var $endDataPanel = $("#pnlProcessEndData");
    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($endDataPanel.find("select"), function (index, dropdown) {
        //$("#" + dropdown.id + " option:selected").prop("selected", false);
        //$("#" + dropdown.id + " option:first").prop("selected", "selected");
        $(dropdown).attr(disabled, disabled).addClass(disabled);
    });

    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(disabled, disabled).addClass(disabled);

    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });
}



function getStraighteningDetailsByChuuban(chuuban, gyouban, machineCD, $callFrom) {

    // Do not proceed if request is already running
    //if ($callFrom.data('requestRunning')) {
    //    return;
    //}

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "',machineCD : '" + machineCD + "', USER_NAME: '" + userName + "' }";
    if (chuuban === null) {
        // Chuuban not found in system.


        // clearPanel($("#pnlProcessStartData"));
        //clearPanel($("#pnlProcessEndData"));
        var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_003").text();
        showMessageDialog(errorMessage, "txtChuuban2", WARNING);
        clearPanel($("#CustomerT2"));
        clearPanel($("#CustomerT3"));
        clearPanel($("#divMaterialDetails"));
        clearPanel($("#divProductDimensionDetails"));
        //ClearData();
        clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
        clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
        clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2

        clearThabaDetailsGrid();
        return false;
    }
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetStraighteningDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, machine_CD: machineCD, CULTURE_NAME: $("#hdfCultureName").val() })
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            //if (jsonResult === null) {
            //    debugger;
            //    // Chuuban not found in system.
            //    disableStartDataPanel();
            //    disableEndDataPanel();
            //    clearPanel($("#pnlProductDetails"));
            //    clearPanel($("#pnlProcessStartData"));
            //    clearPanel($("#pnlProcessEndData"));
            //    var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    //ClearData();
            //    clearPanel($("#pnlCustomerDetails")); // Tab 2
            //    clearPanel($("#divProductDimensionDetails")); // Tab 2
            //    clearPanel($("#divMaterialDetails")); // Tab 2
            //    clearPanel($("#pnlProcessInfo")); // Tab 4
            //    clearPanel($("#pnlTestSpecifications")); // Tab 4    
            //    clearThabaDetailsGrid();
            //    //clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");    // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
            //    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
            //    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
            //    clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
            //    resetMessageDetailsGrid();
            //    resetStraighteningMachineDetailsGrid();
            //    return false;
            //}

            //if ($.isNumeric(jsonResult.pP_PROCESS_PLAN_CMPL_DATE)
            //      && parseInt(jsonResult.pP_PROCESS_PLAN_CMPL_DATE) != null)
            //if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
            //    // Chuuban is marked as completed in the system/
            //    var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_004").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    ClearData();
            //    return false;
            //} 

            //if (jsonResult.procesS_PLAN_START_DATE == null) {
            //    // OrNo Not Started FromHandOver
            //  //   var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_024").text();
            //    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_024").text().trim().replace("99-999999-99", jsonResult.pP_CHUUBAN);

            //    showMessageDialog(message, "", WARNING);
            //    ClearData();
            //    return false;
            //}

            //setChuubanDetails(jsonResult);
            //setStartData(jsonResult);
            ////setProcessDefectList();
            ////setNextProcess();
            disableSearchOrderNoPanel();
            enableStartDataPanel();
            setChuubanDetails(jsonResult);
            setStartData(jsonResult);
            setProcessDefectList();
            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);

            if ($("#hdfDBOperation").val() == "Insert") {
                setStartSysDateTime();
                enableStartDataPanel(jsonResult);
                setStartDataPanelDefaultData(jsonResult);
                //  resetStraighteningMachineDetailsGrid(); //COMMITED BY SHOBHANA BECAUSE ONE EMPTY ROW BIND WITH DATA
                // clearPanel($("#pnlProcessStartData"));
                // clearPanel($("#pnlProcessEndData"));
                setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed before disableEndDataPanel(). It is because setChuubanDimLenToleranceMessages() will enable some fields based on values of Dim and Len Tolerances, and disableEndDataPanel() will disable fields again. 
                disableEndDataPanel();
                $("#txtThaba1").removeClass("Mandatory");
                $("#txtThaba1").removeAttr(DISABLED).removeClass(DISABLED);
                $("#txtThaba2").removeClass("Mandatory");
                $("#txtThaba2").removeAttr(DISABLED).removeClass(DISABLED);
                $("#txtThaba3").removeClass("Mandatory");
                $("#txtThaba3").removeAttr(DISABLED).removeClass(DISABLED);
                $("#txtThaba4").removeClass("Mandatory");
                $("#txtThaba4").removeAttr(DISABLED).removeClass(DISABLED);
                $("#txtStartDate").focus();
            }
            else if ($("#hdfDBOperation").val() == "Update") {

                setStartData(jsonResult);
                enableStartDataPanel(jsonResult);
                enableEndDataPanel();
                DisableSavedThaba();
                $("#txtThaba1").addClass("Mandatory");
                SetLengthControlProperties($("#txtLengthT1").val());
                
                // resetStraighteningMachineDetailsGrid();   //COMMITED BY SHOBHANA BECAUSE ONE EMPTY ROW BIND WITH DATA

                // Set End Operator same as Start Operator
                $("#txtEndOperator").val($("#txtStartOperator").val());
                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());

                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndDate").focus();

                DisablemandatoryNG();

                $('#chkActualDim').attr("checked", "checked");
                $('#chkActualLength').attr("checked", "checked");
                //  $('#chkActualLength').prop("checked", "checked");
                $('#chkFinishedQuantity').prop("checked", "checked");

                if (parseInt(jsonResult.stR_UNIT_TYPE) > 0) {
                    $("#ddlUnitType").val(jsonResult.stR_UNIT_TYPE);
                }

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            //showErrorDiv(errorCode);
        });

    //.always(function () {

    //    $callFrom.data('requestRunning', false);
    //}

}


function setChuubanDimLenToleranceMessages() {
    // Show/hide Tolerance Messages
    if ((dimTolMin == null || dimTolMin == "" || Math.abs(parseFloat(dimTolMin)) == parseFloat(0))
        && (dimTolMax == null || dimTolMax == "" || Math.abs(parseFloat(dimTolMax)) == parseFloat(0))) {
        $("#lblDimToleranceMsgT1").show();
        $("#lblDimToleranceMsgT4").show();
        //$("#chkActualDim").prop('checked', true);
        //$("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
        //toggleActualDimFieldsMandatory($("#chkActualDim"));
    }
    else {
        $("#lblDimToleranceMsgT1").hide();
        $("#lblDimToleranceMsgT4").hide();
        //$("#chkActualDim").prop('checked', false);
        //$("#chkActualDim").removeClass(DISABLED).removeAttr(DISABLED);
        //toggleActualDimFieldsMandatory($("#chkActualDim"));
    }

    if ((lenTolMin == null || lenTolMin == "" || Math.abs(parseFloat(lenTolMin)) == parseFloat(0))
        && (lenTolMax == null || lenTolMax == "" || Math.abs(parseFloat(lenTolMax)) == parseFloat(0))) {
        $("#lblLengthToleranceMsgT1").show();
        $("#lblLengthToleranceMsgT4").show();
    }
    else {
        $("#lblLengthToleranceMsgT1").hide();
        $("#lblLengthToleranceMsgT4").hide();
    }


    if ((lenStd == null || lenStd == "" || Math.abs(parseFloat(lenStd)) == parseFloat(0))) {
        $("#lblLengthMsgT1").show();
        $("#lblLengthMsgT4").show();

        //$("#chkFinishedQuantity").prop('checked', true);
        //$("#chkFinishedQuantity").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
    else {
        $("#lblLengthMsgT1").hide();
        $("#lblLengthMsgT4").hide();

        $("#chkFinishedQuantity").prop('checked', true);
        $("#chkFinishedQuantity").removeClass(DISABLED).removeAttr(DISABLED);
    }


    // For setting chkActualLength, we need to check both StdLength and LengthTolerance
    if (((lenTolMin == null || lenTolMin == "" || Math.abs(parseFloat(lenTolMin)) == parseFloat(0))
        && (lenTolMax == null || lenTolMax == "" || Math.abs(parseFloat(lenTolMax)) == parseFloat(0)))
        ||
        (lenStd == null || lenStd == "" || Math.abs(parseFloat(lenStd)) == parseFloat(0))) {
        $("#chkActualLength").prop('checked', true);
        //$("#chkActualLength").removeClass(DISABLED).removeAttr(DISABLED);
        //// $("#chkActualLength").attr(DISABLED, DISABLED).addClass(DISABLED);
        //toggleActualLengthFieldsMandatory($("#chkActualLength"));
    }
    else {
        //$("#chkActualLength").prop('checked', true);
        //$("#chkActualLength").removeClass(DISABLED).removeAttr(DISABLED);
        //toggleActualLengthFieldsMandatory($("#chkActualLength"));
    }
}


function getStraighteningHistoryByChuuban(chuuban, gyouban, $callFrom) {

    // Do not proceed if request is already running
    //if ($callFrom.data('requestRunning')) {
    //    return;
    //}
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetStraighteningHistoryByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            console.log(jsonResult);

            //if (jsonResult.processPlanDetails == null) {
            //    var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);

            //    clearPanel($("#CustomerT2"));
            //    clearPanel($("#CustomerT3"));
            //    clearPanel($("#divMaterialDetails"));
            //    clearPanel($("#divProductDimensionDetails"));

            //   // ClearData();
            //    clearPanel($("#pnlCustomerDetails")); // Tab 2
            //    clearPanel($("#divProductDimensionDetails")); // Tab 2
            //    clearPanel($("#divMaterialDetails")); // Tab 2
            //    clearPanel($("#pnlProcessInfo")); // Tab 4
            //    clearPanel($("#pnlTestSpecifications")); // Tab 4    
            //    clearThabaDetailsGrid();
            //    //clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");    // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
            //    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
            //    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
            //    clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
            //    resetMessageDetailsGrid();
            //    return false;
            //}

            //if ($.isNumeric(jsonResult.processPlanDetails.pP_PROCESS_PLAN_CMPL_DATE)
            //    && parseInt(jsonResult.processPlanDetails.pP_PROCESS_PLAN_CMPL_DATE) != null) {
            //    // Chuuban is marked as completed in the system/
            //    // TODO Show error message;
            //    focusFieldID = $(':focus').attr("id");
            //    message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_004").text().trim();
            //    showMessageDialog(message, focusFieldID, ERROR);
            //    return false;
            //}

            setChuubanDetailsOnHistoryTab(jsonResult.processPlanDetails);
            var straighteningHistory = jsonResult.straighteningHistory;
            //setHistoryGrid(straighteningHistory);

            if (straighteningHistory.length > 0) {
                clearTable($("#grdvHistory"), "ItemGrdvHistory", "");
                clearTable($("#grdvHistory"), "", "GridNormalRow");// Tab 2
                setHistoryGrid(straighteningHistory);
            } else {
                clearTable($("#grdvHistory"), "grdvHistory", "");
                resetHistoryDetailsGrid();
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            //showErrorDiv(errorCode);
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

    //.always(function () {

    //    $callFrom.data('requestRunning', false);
    //}

}

function setTPTest() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetTPTest",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindTPTestDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindTPTestDropdown(jsonResult) {

    $("#ddlTPTest").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setUnitType() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetUnitType",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            $.each(response.message, function (key, jsonObj) {
                if (jsonObj.name == "束") {
                    $("#lblBN").text(jsonObj.cd);
                    BNO = $("#lblBN").text();
                }
            });
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
    // $("#ddlUnitType").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setExternalSurfaceCondition() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetExternalSurfaceCondition",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindExternalSurfaceConditionDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindExternalSurfaceConditionDropdown(jsonResult) {
    $("#ddlExternalSurfaceCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalSurfaceCondition").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setBendStatus() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetBendStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindddlBendStatusDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindddlBendStatusDropdown(jsonResult) {
    $("#ddlBendStatus").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setDecision() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            binddecisionDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function binddecisionDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));

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
        url: APIURL + "/Straightening/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {

    /* $("#ddlNextProcess").html("").append($('<option></option>'));*/

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


function setProcessDefectList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetProcessDefect",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           
            bindProcessDefectDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcessDefectDropdown(jsonResult) {
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
    //$.each(jsonResult, function (key, jsonObj) {
    //    $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.procesS_DEFECT_STRING + '</option>'));
    //    $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.procesS_DEFECT_STRING + '</option>'));
    //    $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.procesS_DEFECT_STRING + '</option>'));
    //});
    /*if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $("#ddlNGReason1").attr("id") + " option:selected").prop("selected", false);
        $("#" + $("#ddlNGReason1").attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }*/
}

function getMachineDetails(machineCd, $callFrom) {

    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }
    var ajaxTime = new Date().getTime();
    var jsonData = "{machineCd: '" + machineCd + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetMachineDetails?machineCD=" + machineCd + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var jsonResult = response.message;
            if (jsonResult.length > 0) {

                clearTable($("#grdvStraighteningMachine"), "", "ItemGrdvStraighteningMachine");
                clearTable($("#grdvStraighteningMachine"), "", "GridNormalRow");// Tab 2
                Set24HrMachineData(jsonResult);
            } else {
                clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");
                resetStraighteningMachineDetailsGrid();
            }
            //
            //resetStraighteningMachineDetailsGrid();
            //if (jsonResult) {
            //    setStraighteningMachineDetailsGrid(jsonResult);
            //}
            //Set24HrMachineData(jsonResult);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
        });

}

function setHistoryGrid(jsonResult) {
    $tableProcessHistory = $("#grdvHistory");
    clearTable($tableProcessHistory, "ItemGrdvProcessHistory", "");
   
    $.each(jsonResult, function (index, jsonObject) {
        var firstRow = processHistoryGridataRow1;
        if (jsonObject.pP_LOCATION_NAME != null)
            firstRow = firstRow.replace("valtxtProcLocation", jsonObject.pP_LOCATION_NAME);
        else
            firstRow = firstRow.replace("valtxtProcLocation", "");

        if (jsonObject.stR_START_DATE_STRING != null)
            firstRow = firstRow.replace("valtxtOperationDayStart", jsonObject.stR_START_DATE_STRING);
        else
            firstRow = firstRow.replace("valtxtOperationDayStart", "");

        if (jsonObject.stR_END_DATE_STRING != null)
            firstRow = firstRow.replace("valtxtOperationDayEnd", jsonObject.stR_END_DATE_STRING);
        else
            firstRow = firstRow.replace("valtxtOperationDayEnd", "");

        if (jsonObject.pP_CUSTOMER_NAME != null)
            firstRow = firstRow.replace("valtxtCustomer", jsonObject.pP_CUSTOMER_NAME);
        else
            firstRow = firstRow.replace("valtxtCustomer", "");

        if (jsonObject.pP_CHUUBAN != null)
            firstRow = firstRow.replace("valtxtChuubanGyouban", jsonObject.pP_CHUUBAN);
        else
            firstRow = firstRow.replace("valtxtChuubanGyouban", "");
            //rem
        if (jsonObject.pP_PROD_NAME_CUST != null)
            firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
        else
            firstRow = firstRow.replace("valtxtProdNameCust", "");

        if (jsonObject.pP_PROD_NAME_CUST != null)
            firstRow = firstRow.replace("valtxtCharge", jsonObject.pP_CHARGE);
        else
            firstRow = firstRow.replace("valtxtCharge", "");

        if (jsonObject.pP_PROD_NAME_CUST != null)
            firstRow = firstRow.replace("valtxtMatelicPlatNo", jsonObject.stR_METALLIC_PLATE_CODE);
        else
            firstRow = firstRow.replace("valtxtMatelicPlatNo", "");
            
        if (jsonObject.pP_PROD_DIM_D != null)
            firstRow = firstRow.replace("valtxtDimD", jsonObject.pP_PROD_DIM_D + jsonObject.pP_PROD_SHAPE);
        else
            firstRow = firstRow.replace("valtxtDimD", "");

        debugger;
        if (jsonObject.diM_TOL_MIN_MAX_CHR != null)
            firstRow = firstRow.replace("valtxtstrDimTolerance", jsonObject.diM_TOL_MIN_MAX_CHR);
        else
            firstRow = firstRow.replace("valtxtstrDimTolerance", "");

        if (jsonObject.pP_NO_OF_UNITS_DECODE != null)
            firstRow = firstRow.replace("valtxtstrActNoUnits", jsonObject.pP_NO_OF_UNITS_DECODE);
        else
            firstRow = firstRow.replace("valtxtstrActNoUnits", "");
       
        if (jsonObject.stR_TABA_NO != null)
            firstRow = firstRow.replace("valtxtThabaNo", jsonObject.stR_TABA_NO);
        else
            firstRow = firstRow.replace("valtxtThabaNo", "");

        if (jsonObject.stR_LENGTH_AFTER_1 != null)
            firstRow = firstRow.replace("valtxtAfterStraightening1", jsonObject.stR_LENGTH_AFTER_1);
        else
            firstRow = firstRow.replace("valtxtAfterStraightening1", "");

        if (jsonObject.stR_LENGTH_AFTER_1 != null)
            firstRow = firstRow.replace("valtxtstrActualLength", jsonObject.stR_LENGTH_AFTER_1);
        else
            firstRow = firstRow.replace("valtxtstrActualLength", "");

        if (jsonObject.stR_BARS_DECODE != null)
            firstRow = firstRow.replace("valtxtstrNoofUnits", jsonObject.stR_BARS_DECODE);
        else
            firstRow = firstRow.replace("valtxtstrNoofUnits", "");

        if (jsonObject.totaL_STR_QTY != null)
            firstRow = firstRow.replace("valtxtstrWeightWithoutKG", jsonObject.totaL_STR_QTY);
        else
            firstRow = firstRow.replace("valtxtstrWeightWithoutKG", "");

        var secondRow = processHistoryGridataRow2;
        if (jsonObject.stR_REMARKS != null)
            firstRow = firstRow.replace("valtxtstrRemark", jsonObject.stR_REMARKS);
        else
            firstRow = firstRow.replace("valtxtstrRemark", "");

        if (jsonObject.stR_DECISION_STRING != null)
            firstRow = firstRow.replace("valtxtstrDecision", jsonObject.stR_DECISION_STRING);
        else
            firstRow = firstRow.replace("valtxtstrDecision", ""); 

        if ($("#hdfCultureName").val() == "en") {
            if (jsonObject.nexT_PROCESS_EN != null)
                firstRow = firstRow.replace("valtxtstrNextProcess", jsonObject.nexT_PROCESS_EN);
            else
                firstRow = firstRow.replace("valtxtstrNextProcess", "");
        }
        else {
            if (jsonObject.nexT_PROCESS_JP != null)
                firstRow = firstRow.replace("valtxtstrNextProcess", jsonObject.nexT_PROCESS_JP);
            else
                firstRow = firstRow.replace("valtxtstrNextProcess", "");
        }

          ///
        if (jsonObject.stR_MACHINE_NAME != null)
            secondRow = secondRow.replace("valtxtHTMachine", jsonObject.stR_MACHINE_NAME);
        else
            secondRow = secondRow.replace("valtxtHTMachine", "");

        if (jsonObject.stR_END_OPERATOR_NAME != null)
            secondRow = secondRow.replace("valtxtOperatorName", jsonObject.stR_END_OPERATOR_NAME);
        else
            secondRow = secondRow.replace("valtxtOperatorName", "");

        if (jsonObject.pP_PROD_SPEC_NO != null)
            secondRow = secondRow.replace("valtxtSpec_No", jsonObject.pP_PROD_SPEC_NO);
        else
            secondRow = secondRow.replace("valtxtSpec_No", "");

        if (jsonObject.pP_PROD_LENGTH_STRING != null)
            secondRow = secondRow.replace("valtxtProd_Len", jsonObject.pP_PROD_LENGTH_STRING);
        else
            secondRow = secondRow.replace("valtxtProd_Len", "");

        if (jsonObject.leNGTH_TOL_MIN_MAX_CHR != null)
            secondRow = secondRow.replace("valtxtLenTol", jsonObject.leNGTH_TOL_MIN_MAX_CHR);
        else
            secondRow = secondRow.replace("valtxtLenTol", "");

        if (jsonObject.pP_ACT_ALLOC_QTY != null)
            secondRow = secondRow.replace("valtxtACTAlloc_qty", jsonObject.pP_ACT_ALLOC_QTY);
        else
            secondRow = secondRow.replace("valtxtACTAlloc_qty", "");

        if (jsonObject.alL_DIM_BEFORE != null)
            secondRow = secondRow.replace("valtxtDIm_Before", jsonObject.alL_DIM_BEFORE);
        else
            secondRow = secondRow.replace("valtxtDIm_Before", "");

        if (jsonObject.alL_DIM_AFTER != null)
            secondRow = secondRow.replace("valtxtAllDimAfter", jsonObject.alL_DIM_AFTER);
        else
            secondRow = secondRow.replace("valtxtAllDimAfter", "");


        //if (jsonObject.stR_LENGTH_AFTER_2 != null)
        //    firstRow = firstRow.replace("valtxtStrLenAfter", jsonObject.stR_LENGTH_AFTER_2);
        //else
        //    firstRow = firstRow.replace("valtxtStrLenAfter", "");
            
        $tableProcessHistory.append($(firstRow));
        $tableProcessHistory.append($(secondRow));
   
    });

}

function setChuubanDetailsOnHistoryTab(jsonResult) {
    debugger;
    $("#txtProductSpecNoT2").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT2").val(jsonResult.pP_CHUUBAN);
    $("#txtDeliveryDateT2").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtCustomerInchargeT2").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT2").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT2").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT2").val(jsonResult.pP_RECEIVER_NAME);
    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT2").val(jsonResult.pP_CHAMFER);
    }
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) > 0) {
        $("#txtUnitWtT2").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
        
    }
    $("#txtColorT2").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT2").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT2").val(jsonResult.pP_MATL_SHAPE);
    

    $("#txtMatlDimT2").val(jsonResult.pP_MATL_DIM_D);
    


    $("#txtChargeT2").val(jsonResult.pP_CHARGE);
    $("#txtConditionT2").val(jsonResult.conditioN_DESCRIPTION); 

    $("#txtBendStandardT2").val(jsonResult.pP_MAGARI_BEND_TOL);
    
    $("#txtProdNameCustomerT2").val(jsonResult.pP_PROD_NAME_CUST);
    

    $("#txtProdShapeT2").val(jsonResult.pP_PROD_SHAPE);
    

    $("#txtProdDimT2").val(jsonResult.pP_PROD_DIM_D);
    

    $("#txtProdDimOtherT2").val(jsonResult.pP_PROD_DIM_OTHER);
   
    $("#txtExtTextureT2").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT2").val(jsonResult.pP_DIM_TOLERANCE);
    
    if (jsonResult.pP_DIM_TOL_MIN != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_DIM_TOL_MAX != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    dimStd = jsonResult.pP_PROD_DIM_D;
    $("#txtDisplayT2").val(jsonResult.pP_DISPLAY);
    $("#txtStampT2").val(jsonResult.pP_STAMP);
    $("#txtLengthT2").val(jsonResult.pP_PROD_LENGTH_STRING);
   
  
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT2").val(jsonResult.pP_LEN_TOL);
       
        //$("#txtLengthT2").val(jsonResult.pP_LEN_TOL);
    }
    //else {
    //    $("#txtLengthToleranceT2").val('');
        
    //    $("#txtLengthT2").val('');
    //}


    if (jsonResult.pP_LENGTH_TOL_MIN != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_LENGTH_TOL_MAX != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    lenStd = jsonResult.pP_PROD_LENGTH;

    $("#txtPackingT2").val(jsonResult.pP_PACKING);
   

    $("#txtNoOfUnitsT2").val(jsonResult.pP_NO_OF_UNITS_DECODE);
   

    $("#txtQuantityT2").val(jsonResult.pP_ACT_ALLOC_QTY);
   

    $("#txtIntDiaMinT2").val(jsonResult.pP_COIL_DIA_INTERNAL);
   
    $("#txtHostRefNoT2").val(jsonResult.producT_NYUKO_CD);
  

    $("#txtExtDiaMaxT2").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    

    $("#txtWindingDirectionT2").val(jsonResult.pP_COIL_DIA_WINDING);
    

    $("#txtKakouiraiRemarksT2").val(jsonResult.pP_KAKOUIRAI_REMARKS);
   

    $("#txtProdOrderRemarkT2").val(jsonResult.pP_PROCESS_PLAN_REMARK);
  

    $("#txtProductionProcessT2").val(jsonResult.pP_PRODUCTION_PROCESS);
////////////////////////////////////


    $("#txtCustomerT2").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT2").val(jsonResult.pP_RECEIVER_NAME);
    $("#txtSalesInchargeT2").val(jsonResult.pP_EMPLOYEE_NAME);

    $("#txtProdNameCustT2").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdNameIntT2").val(jsonResult.pP_PROD_NAME_INT);
    $("#txtShapeDimLengthT2").val(jsonResult.pP_PROD_SHAPE + " " + jsonResult.pP_PROD_DIM_D + " " + jsonResult.pP_PROD_LENGTH_STRING);

    $("#txtDimToleranceT2").val(jsonResult.pP_DIM_TOLERANCE);

    $("#txtBendStdT2").val(jsonResult.pP_MAGARI_BEND_TOL);

    $("#txtProductionProcessT2").val(jsonResult.pP_PRODUCTION_PROCESS);

    $("#txtMaterialT2").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtDimT2").val(jsonResult.pP_MATL_DIM_D_SHAPE); //text(jsonResult.pP_MATL_DIM_D + " " + jsonResult.pP_MATL_SHAPE);
}

function resetMessageDetailsGrid() {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove();  // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow" >').append(
        $('<td class="GridAlignmentDateTime">').text(""),
        $('<td class="GridAlignmentMachineName">').text(""),
        $('<td class="GridAlignmentProcessName">').text(""),
        $('<td class="GridAlignmentOperator">').text(""),
        $('<td class="GridAlignmentRemarks">').text("")
    );
    $grdvMessage.append($tr);
}

function resetStraighteningMachineDetailsGrid() {

    clearTable($("#grdvStraighteningMachine"), "ItemGrdvHikinukiMachine", "");
    $grdvMessage = $("#grdvStraighteningMachine");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr1 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'> ").text(""),
        $("<td  rowspan='2' align='center' valign='top'> ").text(""),

        $("<td align='center' valign='top' colspan='2'>").text(""),
        $("<td  align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
    );
    var $tr2 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
        $("<td  align='center' valign='top'>").text(""),
        $("<td  align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),

        $("<td align='center' valign='top' colspan='2'>").text(""),
        $("<td align='center' valign='top'>").text(""),

    );
    //var $tr3 = $('<tr class="GridNormalRow" style="height:10px;border:1px solid #FF9900;">').append(
    //    $("<td align='center' valign='top'>").text(""),

    //);
    $grdvMessage.append($tr1);
    $grdvMessage.append($tr2);
}

function setStraighteningMachineDetailsGrid(StraighteningMachineDetailsList) {

    $grdvStraighteningMachine = $("#grdvMessage");

    $grdvStraighteningMachine.find("tr.GridNormalRow").remove(); // Clear the rows

    $.each(StraighteningMachineDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
        if (($hdfCultureLoaded.val()).indexOf($hdfCultureEN.val()) > -1) // > -1 means substring is present
        {
            processName = jsonObject.procesS_NAME_EN;
        }

        // Add data rows


        var $tr = $('<tr class="GridNormalRow">').append(
            $('<td class="GridAlignmentDateTime">').text(jsonObject.msG_DATE_STRING),
            $('<td class="GridAlignmentMachineName">').text(jsonObject.machinE_NAME),
            $('<td class="GridAlignmentProcessName">').text(processName),
            $('<td class="GridAlignmentOperator">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks">').text(jsonObject.msG_CONTENTS)
        );

        $grdvStraighteningMachine.append($tr);
    });
}

function resetHistoryDetailsGrid() {
    clearTable($("#grdvHistory"), "ItemGrdvHikinukiMachine", "");
    $grdvHistory = $("#grdvHistory");
    $grdvHistory.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    $grdvHistory.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    //var $tr1 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
    //    $("<td rowspan='2' align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td colspan='2' align='center' valign='top'> ").text(""),
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //    $("<td colspan='2' align='center' valign='top'> ").text(""),
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'> ").text(""),
    //    $("<td align='center' valign='top'> ").text(""),
    //    $("<td colspan='3' align='center' valign='top'>").text(""),
    //    //$("<td align='center' valign='top'>").text(""),
    //    //$("<td align='center' valign='top'>").text(""),
    //    //$("<td  align='center' valign='top'>").text(""),
    //    //$("<td rowspan='2'align='center' valign='top'>").text(""),
    //    //$("<td rowspan='2'align='center' valign='top'>").text(""),
    //);
    //var $tr2 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
    //    $("<td  align='center' valign='top'>").text(""),
    //    $("<td  align='center' valign='top'>").text(""),
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //    //  $("<td align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //    /* $("<td align='center' valign='top'>").text(""),*/
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //    $("<td colspan='2' align='center' valign='top'>").text(""),
    //);
    //var $tr3 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
    //    $("<td  align='center' valign='top'>").text(""),
    //    $("<td  align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td colspan='6'align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td align='center' valign='top'>").text(""),
    //    $("<td colspan='3' align='center' valign='top'>").text(""),
    //);
    //var $tr4 = $('<tr class="GridNormalRow" style="height:10px;border:1px solid #FF9900;">').append(
    //    $("<td align='center' valign='top'>").text(""),
    //);
    // Blank Row 1
    var $tr1 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
       
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;"),
         $("<td rowspan='2'>").html("&nbsp;"),
        $("<td rowspan='2'>").html("&nbsp;")
    );

    // Blank Row 2
    var $tr2 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
        $("<td colspan='2'>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;"),
        $("<td>").html("&nbsp;")
    );
    $grdvHistory.append($tr1);
    $grdvHistory.append($tr2);
   
}

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
function setMessageDetailsGrid(messageDetailsList) {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
        if ($("#hdfCultureName").val() == "en") {
            processName = jsonObject.procesS_NAME_EN;
        }

        // Add data rows
        var $tr = $('<tr class="GridNormalRow">').append(
            $('<td class="GridAlignmentDateTime">').text(jsonObject.msG_DATE_STRING),
            $('<td class="GridAlignmentMachineName">').text(processName),
            $('<td class="GridAlignmentProcessName">').text(jsonObject.machinE_NAME),
            $('<td class="GridAlignmentOperator">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks">').text(jsonObject.msG_CONTENTS)
        );

        $grdvMessage.append($tr);
    });
}



function setChuubanDetails(jsonResult) {
    debugger;
    dbOperation = "Insert";
    if ((jsonResult.stR_START_DATE_STRING != null) && (jsonResult.stR_END_DATE_STRING == null) && jsonResult.pasT_DATA_EXIST == "YES") {
        dbOperation = "Update";
    }

    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#hdfDBOperation").val(dbOperation);

    straighteningCD = $.isNumeric(jsonResult.stR_STRAIGHTENING_CD) ? parseInt(jsonResult.stR_STRAIGHTENING_CD) : 0;

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
    
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) > 0) {
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

    $("#txtConditionT1").val(jsonResult.conditioN_DESCRIPTION); 

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

    if (jsonResult.pP_DIM_TOL_MIN != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_DIM_TOL_MAX != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    dimStd = jsonResult.pP_PROD_DIM_D;

    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);

    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtStampT4").val(jsonResult.pP_STAMP);

    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_STRING);

    // $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    // $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
        $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
        $("#txtLengthT2").val(jsonResult.pP_LEN_TOL);
    }
    else {
        $("#txtLengthToleranceT1").val('');
        $("#txtLengthToleranceT4").val('');
        $("#txtLengthT2").val('');
    }


    if (jsonResult.pP_LENGTH_TOL_MIN != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_LENGTH_TOL_MAX != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    lenStd = jsonResult.pP_PROD_LENGTH;

    $("#txtPackingT1").val(jsonResult.pP_PACKING);
    $("#txtPackingT4").val(jsonResult.pP_PACKING);

    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtNoOfUnitsT4").val(jsonResult.pP_NO_OF_UNITS_DECODE);

    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtQuantityT4").val(jsonResult.pP_ACT_ALLOC_QTY);

    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtIntDiaMinT4").val(jsonResult.pP_COIL_DIA_INTERNAL);

    $("#txtHostRefNoT1").val(jsonResult.producT_NYUKO_CD);
    $("#txtHostRefNoT4").val(jsonResult.producT_NYUKO_CD);

    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtExtDiaMaxT4").val(jsonResult.pP_COIL_DIA_EXTERNAL);

    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtWindingDirectionT4").val(jsonResult.pP_COIL_DIA_WINDING);

    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtKakouiraiRemarksT4").val(jsonResult.pP_KAKOUIRAI_REMARKS);

    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    $("#txtProdOrderRemarkT4").val(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);

    // 3rd Tab
    resetMessageDetailsGrid();
    if (jsonResult.dW_MESSAGE_LIST) {
        setMessageDetailsGrid(jsonResult.dW_MESSAGE_LIST);
    }

    //4th Tab 

    $("#txtJISTestSpecimen").val(jsonResult.pP_JIS_TEST_SPEC);
    $("#txtTensileStrength").val(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtYieldPoint").val(jsonResult.pP_YEILD);
    $("#txtElongation").val(jsonResult.pP_ELONGATION);
    $("#txtReduction").val(jsonResult.pP_PRESS);// 
    $("#txtHardness").val(jsonResult.pP_HARDNESS);
    $("#txtDecarb").val(jsonResult.pP_DECARB);

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

function setStartData(jsonResult) {

    if (jsonResult.stR_START_DATE_STRING != null && jsonResult.stR_END_DATE_STRING == null && jsonResult.pasT_DATA_EXIST == "YES") {
        $("#txtStartOperator").val(jsonResult.stR_START_OPERATOR_NAME);
        $("#hdfStartOperatorCD").val(jsonResult.stR_START_OPERATOR);
        $("#ddlTPTest").val(parseInt(jsonResult.stR_TP_TEST) || $("#ddlTPTest").prop('selectedIndex')); // Note : This logic is very crucial . Either set the data from DB. If data is not in DB then set what is default selected index on HTML.

        if (jsonResult.stR_TP_TEST == null) {
            $('#ddlTPTest option:eq(0)').prop('selected', true);
        }

        $("#txtFurnaceNum").val(jsonResult.stR_FURNACE_NO);
        $("#txtMetallicLabelNo").val(jsonResult.stR_METALLIC_PLATE_CODE);
        $("#txtActualDimDBeforeStraightening1").val(jsonResult.stR_DIM_BEFORE_1);
        $("#txtActualDimDBeforeStraightening2").val(jsonResult.stR_DIM_BEFORE_2);
        $("#txtActualDimDBeforeStraightening3").val(jsonResult.stR_DIM_BEFORE_3);
        $("#txtActualDimDBeforeStraightening4").val(jsonResult.stR_DIM_BEFORE_4);
        $("#txtActualDimDBeforeStraightening5").val(jsonResult.stR_DIM_BEFORE_5);
        $("#txtActualDimDBeforeStraightening6").val(jsonResult.stR_DIM_BEFORE_6);
        $("#txtUpperLowerRollerAngle1").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE1);
        $("#txtUpperLowerRollerAngle2").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE2);
        $("#txtActualLength1BeforeStraightening").val(jsonResult.stR_LENGTH_BEFORE_1);
        $("#txtActualLength2BeforeStraightening").val(jsonResult.stR_LENGTH_BEFORE_2);
        $("#txtUpperLowerRollerAngle1").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE1);
        $("#txtUpperLowerRollerAngle2").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE2);
        $("#txtSTRTonnage").val(jsonResult.stR_TONNAGE);
        $("#txtReservationNum").val(jsonResult.stR_RESERVATION_NO);
        $("#txtUpperRoleElevation").val(jsonResult.stR_UPPER_ROLLER_ELEVATION);
        $("#txtThaba1").val(jsonResult.stR_TABA_NO_1);
        $("#txtThaba2").val(jsonResult.stR_TABA_NO_2);
        $("#txtThaba3").val(jsonResult.stR_TABA_NO_3);
        $("#txtThaba4").val(jsonResult.stR_TABA_NO_4);


        if (jsonResult.stR_RESTRAIGHTING == 0) {
            $("#chkRestraighting").prop('checked', false);
        }
        else {
            $("#chkRestraighting").prop('checked', true);
        }

        if (dbOperation == "Insert") {
            setStartSysDateTime();
        }
        else {

            $("#txtStartDate").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

        }
    }
    else {
        debugger;
        if (jsonResult.shoW_DEFAULT == 1 && jsonResult.stR_START_DATE_STRING != null && jsonResult.stR_END_DATE_STRING != null && jsonResult.pasT_DATA_EXIST == "YES") {
            $("#txtMetallicLabelNo").val(jsonResult.stR_METALLIC_PLATE_CODE);
            $("#txtFurnaceNum").val(jsonResult.stR_FURNACE_NO);
            $("#txtUpperLowerRollerAngle1").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE1);
            $("#txtUpperLowerRollerAngle2").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE2);
            $("#txtUpperRoleElevation").val(jsonResult.stR_UPPER_ROLLER_ELEVATION);
            $("#txtSTRTonnage").val(jsonResult.stR_TONNAGE);
            $("#ddlTPTest").val(parseInt(jsonResult.stR_TP_TEST) || $("#ddlTPTest").prop('selectedIndex'));
            $("#ddlUnitType").val(jsonResult.stR_UNIT_TYPE);
        }
    }
}


function getOperatorNameByCD($target) {

    var ajaxTime = new Date().getTime();
    var operatorCD = $target.val().trim();
    var operatorVal = operatorCD.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorNumber = operatorCD.substring(3, 9);
    //  var operatorCd = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_STA_ENTRY_008").text().trim().replace("*****", $("#lblStartOperator").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    else if (operatorNumber == "") {
        var message = $("#lblErrorMessageE_BarCodeNotFound_002").text().trim().replace("*****", $("#lblStartOperator").text());
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorNumber.length > 6) {
        var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_013").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {
        var jsonData = "{OperatorCD: '" + operatorNumber + "', USER_NAME: '" + userName + "' }";
        var jsonRetval = "";

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening/GetOperatorNamebyCD?OperatorCD=" + operatorNumber,
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;

                operatorName = jsonResult;
                if (jsonResult == null || jsonResult == "") {
                    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_012").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }
                else if (jsonResult.iN_ACTIVE_FLAG == 1) {
                    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_022").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }

                else {

                    $target.val(jsonResult.yobina);
                    // $("#ddlDimDecision").focus();
                }
            })

            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            })

    }
}

function getMachineNamebyCD($target) {

    var ajaxTime = new Date().getTime();
    var machineCD = $target.val().trim();
    var machineval = machineCD.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineNumber = machineCD.substring(3, 7);

    // var machineCD = $target.val().trim().replace(/\D/g, ""); // this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits. 
    //if (machineCD == "") {
    //    $('#txtMachineType').val("");
    //  return false;
    //}
    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_STA_ENTRY_034").text().trim().replace("*****", $("#tdLblMachineType").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }

    if (machineNumber == "") {
        var message = $("#lblErrorMessageE_ScanMachine_015").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
    else if (machineNumber.length > 4) {
        var message = $("#lblErrorMessageE_BarCodeNotFound_002").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {
        var jsonData = "{machineCD: '" + machineNumber + "', USER_NAME: '" + userName + "' }";
        var settings = {
            async: false,
            crossDomain: true,
            dataType: "json",
            url: APIURL + "/Straightening/GetMachineNamebyCD?machineCD=" + parseInt(machineNumber),
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {

                responseTime = new Date().getTime() - ajaxTime;
                var machineName = response.message;

                if (machineName.key == null || machineName.key == "") {
                    var message = $("#lblErrorMessageE_BarCodeNotFound_002").text().trim().replace("*****", $("#tdLblMachineType").text());
                    // var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_013").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the machineName barcode again
                }
                else if (machineName.value == "inactive") {
                    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_016").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val("");
                }
                else {

                    $target.val(machineName.key);
                    getMachineDetails($("#hdfMachineCD").val());
                    $("#txtChuuban2").focus();
                }


            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

}
//Added because after extract click disable chuuban.(All panel)
function disableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "none");
}
function enableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
}


function setEndData(jsonResult) {

}

function saveStraighteningDetails($callFrom) {

    var ajaxTime = new Date().getDate();
    var jsonDataSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val() ? $("#txtChuuban2").val() : null,
        PP_GYOUBAN: $("#txtGyouban2").val() ? $("#txtGyouban2").val() : null,
        STR_STRAIGHTENING_CD: straighteningCD ? straighteningCD : null,
        PP_PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        STR_MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_START_OPERATOR: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        STR_TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
        STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
        STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
        STR_DIM_BEFORE_3: $("#txtActualDimDBeforeStraightening3").val() ? $("#txtActualDimDBeforeStraightening3").val() : null,
        STR_DIM_BEFORE_4: $("#txtActualDimDBeforeStraightening4").val() ? $("#txtActualDimDBeforeStraightening4").val() : null,
        STR_DIM_BEFORE_5: $("#txtActualDimDBeforeStraightening5").val() ? $("#txtActualDimDBeforeStraightening5").val() : null,
        STR_DIM_BEFORE_6: $("#txtActualDimDBeforeStraightening6").val() ? $("#txtActualDimDBeforeStraightening6").val() : null,
        STR_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        STR_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
        STR_LENGTH_BEFORE_1: $("#txtActualLength1BeforeStraightening").val() ? $("#txtActualLength1BeforeStraightening").val() : null,
        STR_LENGTH_BEFORE_2: $("#txtActualLength2BeforeStraightening").val() ? $("#txtActualLength2BeforeStraightening").val() : null,
        STR_RESERVATION_NUMBER: $("#txtReservationNum").val() ? $("#txtReservationNum").val() : null,
        STR_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
        STR_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
        STR_TONNAGE: $("#txtSTRTonnage").val() ? $("#txtSTRTonnage").val() : null,
        STR_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        STR_RESTRAIGHTING: ($('#chkRestraighting').is(':checked') ? 1 : 0),
        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        STR_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        CULTURE_NAME: $("#hdfCultureName").val(),
        USER_NAME: userName ? userName : null
    });

    var jsonData = "{"
        // START DATA

        + "PP_CHUUBAN" + " : '" + $("#txtChuuban2").val() + "',"
        + "PP_GYOUBAN" + " :  '" + $("#txtGyouban2").val() + "',"
        + "STR_STRAIGHTENING_CD" + " : '" + straighteningCD + "',"
        + "PP_PROCESS_PLAN_CD" + " :  '" + $("#hdfldProcessPlanCd").val() + "',"//
        + "STR_MACHINE_CD" + " : '" + $("#hdfMachineCD").val() + "',"
        + "STR_START_DATE" + " : '" + $("#txtStartDate").val() + " " + $("#txtStartTime").val() + "',"//
        + "STR_START_OPERATOR" + " : '" + $("#hdfStartOperatorCD").val() + "',"//
        + "STR_TP_TEST" + " : '" + $("#ddlTPTest option:selected").val() + "',"//
        + "STR_METALLIC_PLATE_CODE" + " : '" + $("#txtMetallicLabelNo").val() + "',"//
        + "STR_DIM_BEFORE_1" + " : '" + $("#txtActualDimDBeforeStraightening1").val() + "',"//
        + "STR_DIM_BEFORE_2" + " : '" + $("#txtActualDimDBeforeStraightening2").val() + "',"//
        + "STR_DIM_BEFORE_3" + " : '" + $("#txtActualDimDBeforeStraightening3").val() + "',"//
        + "STR_DIM_BEFORE_4" + " : '" + $("#txtActualDimDBeforeStraightening4").val() + "',"//
        + "STR_FURNACE_NO" + " : '" + $("#txtFurnaceNum").val() + "',"//
        + "STR_UPPER_ROLLER_ELEVATION" + " : '" + $("#txtUpperRoleElevation").val() + "',"
        + "STR_LENGTH_BEFORE_1" + " : '" + $("#txtActualLength1BeforeStraightening").val() + "',"//
        + "STR_LENGTH_BEFORE_2" + " : '" + $("#txtActualLength2BeforeStraightening").val() + "',"//
        + "STR_RESERVATION_NUMBER" + " : '" + $("#txtReservationNum").val() + "',"//
        + "STR_UPPER_LOWER_ROLLER_ANGLE1" + " : '" + $("#txtUpperLowerRollerAngle1").val() + "',"//
        + "STR_UPPER_LOWER_ROLLER_ANGLE2" + " : '" + $("#txtUpperLowerRollerAngle2").val() + "',"//
        + "STR_TONNAGE" + " : '" + $("#txtSTRTonnage").val() + "',"
        + "STR_DIM_CHK" + " : '" + ($('#chkActualDim').is(':checked') ? 1 : 0) + "',"
        + "STR_RESTRAIGHTING" + " :  '" + ($('#chkRestraighting').is(':checked') ? 1 : 0) + "',"
        + "STR_USER_ID" + " : '" + $("#hdfLoginId").val() + "',"
        + "USER_NAME" + " : '" + userName
        + "'}";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/StraighteningDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            /* After StartData is saved sucessfully, enable EndData Panel for input, 
              and show success message. */

            $("#txtEndOperator").val($("#txtStartOperator").val());
            $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());

            SetLengthControlProperties($("#txtLengthT1").val());
            
            var successMessage = $("#lblSuccessMessageS_STRAIGHTENING_ENTRY_001").text();
            showMessageDialog(successMessage, "SUCCESS", SUCCESS);
            var jsonResult = response.message;
            // clearPanel($("#pnlProcessEndData"));
            enableEndDataPanel();
            DisableSavedThaba();
            setChuubanDetails(jsonResult);
            setStartData(jsonResult);
            $("#txtThaba1").addClass("Mandatory");

            DisablemandatoryNG();


            getMachineDetails($("#hdfMachineCD").val(), $callFrom);
            $("#hdfDBOperation").val("Update");
            $("#btnEndDateTime").click(); // Get current time for end operation.
            $("#txtEndDate").focus();
            setChuubanDimLenToleranceMessages();
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function SetLengthControlProperties(lenghtVal) {

    var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
    if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
        $("#chkActualLength").removeAttr("disabled", "disabled");
        $("#chkActualLength").prop("checked", true);
        $("#txtActualLength1AfterStraightening").addClass("Mandatory");
        $("#txtActualLength2AfterStraightening").addClass("Mandatory");
    } else {
        $("#chkActualLength").attr("disabled", "disabled");
        $("#chkActualLength").prop("checked", false);
        $("#txtActualLength1AfterStraightening").removeClass("Mandatory");
        $("#txtActualLength2AfterStraightening").removeClass("Mandatory");
    }

}

function updateStraighteningDetails($callFrom) {

    var ajaxTime = new Date().getDate();




    // END DATA
    var jsonDataEditDelete = JSON.stringify({
        STR_STRAIGHTENING_CD: straighteningCD ? straighteningCD : null,
        STR_PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        STR_MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_START_OPERATOR: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        STR_LENGTH_BEFORE_1: $("#txtActualLength1BeforeStraightening").val() ? $("#txtActualLength1BeforeStraightening").val() : null,
        STR_LENGTH_BEFORE_2: $("#txtActualLength2BeforeStraightening").val() ? $("#txtActualLength2BeforeStraightening").val() : null,
        STR_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
        STR_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
        STR_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
        STR_TONNAGE: $("#txtSTRTonnage").val() ? $("#txtSTRTonnage").val() : null,
        STR_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_END_OPERATOR: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
        STR_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        STR_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        STR_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
        STR_LENGTH_AFTER_1: $("#txtActualLength1AfterStraightening").val() ? $("#txtActualLength1AfterStraightening").val() : null,
        STR_LENGTH_AFTER_2: $("#txtActualLength2AfterStraightening").val() ? $("#txtActualLength2AfterStraightening").val() : null,
        STR_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
        STR_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
        STR_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : null,
        STR_NG_DETAILS1: $("#ddlNGReason1").val() ? $("#ddlNGReason1").val() : null,
        STR_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : null,
        STR_NG_DETAILS2: $("#ddlNGReason2").val() ? $("#ddlNGReason2").val() : null,
        STR_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : null,
        STR_NG_DETAILS3: $("#ddlNGReason3").val() ? $("#ddlNGReason3").val() : null,
        STR_EXT_TEXTURE: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
        STR_RESERVATION_NUMBER: $("#txtReservationNum").val() ? $("#txtReservationNum").val() : null,
        STR_A_SIDE_FURE1: $("#txtAEdgeDistortion1").val() ? $("#txtAEdgeDistortion1").val() : null,
        STR_A_SIDE_FURE2: $("#txtAEdgeDistortion2").val() ? $("#txtAEdgeDistortion2").val() : null,
        STR_A_SIDE_FURE3: $("#txtAEdgeDistortion3").val() ? $("#txtAEdgeDistortion3").val() : null,
        STR_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
        STR_B_SIDE_FURE1: $("#txtBEdgeDistortion1").val() ? $("#txtBEdgeDistortion1").val() : null,
        STR_B_SIDE_FURE2: $("#txtBEdgeDistortion2").val() ? $("#txtBEdgeDistortion2").val() : null,
        STR_B_SIDE_FURE3: $("#txtBEdgeDistortion3").val() ? $("#txtBEdgeDistortion3").val() : null,
        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        STR_TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
        STR_UNIT_TYPE: $("#ddlUnitType").find(":selected").val() ? $("#ddlUnitType").find(":selected").val() : null,
        STR_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        STR_FINISHED_QTY_CHK: ($('#chkFinishedQuantity').is(':checked') ? 1 : 0),
        STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
        STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
        STR_DIM_BEFORE_3: $("#txtActualDimDBeforeStraightening3").val() ? $("#txtActualDimDBeforeStraightening3").val() : null,
        STR_DIM_BEFORE_4: $("#txtActualDimDBeforeStraightening4").val() ? $("#txtActualDimDBeforeStraightening4").val() : null,
        STR_DIM_BEFORE_5: $("#txtActualDimDBeforeStraightening5").val() ? $("#txtActualDimDBeforeStraightening5").val() : null,
        STR_DIM_BEFORE_6: $("#txtActualDimDBeforeStraightening6").val() ? $("#txtActualDimDBeforeStraightening6").val() : null,
        STR_DIM_AFTER_1: $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null,
        STR_DIM_AFTER_2: $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null,
        STR_DIM_AFTER_3: $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null,
        STR_DIM_AFTER_4: $("#txtActualDim4").val() ? $("#txtActualDim4").val() : null,
        STR_DIM_AFTER_5: $("#txtActualDim5").val() ? $("#txtActualDim5").val() : null,
        STR_DIM_AFTER_6: $("#txtActualDim6").val() ? $("#txtActualDim6").val() : null,
        STR_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
        STR_RESTRAIGHTING: ($('#chkRestraighting').is(':checked') ? 1 : 0),
        STR_REMARKS: $.trim($("#txtRemarks").val()) ? $.trim($("#txtRemarks").val()) : null,
        STR_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        USER_NAME: userName ? userName : null
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/StraighteningDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            /* After EndData is saved sucessfully:
                1. Disable all panels
                2. Show success message
                3. Refresh Thaba details grid 
            */

            $("#dialogMessageIcon img").hide();
            $("#imgSuccess").show();

            var successMessage = $("#lblSuccessMessageS_STRAIGHTENING_ENTRY_001").text();
            showMessageDialog(successMessage, "SUCCESS", SUCCESS);

            disableStartDataPanel();
            disableEndDataPanel();
            disableSearchPanel();
            disableSaveButton();
            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
            getMachineDetails($("#hdfMachineCD").val(), $callFrom);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
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

//function IsMandatoryChuubanSearchFieldsFilled() {
//    var returnVal = true;  // default true is required for screens with no mandatory fields

//    var message = ""; // This message will be displayed on dialog box
//    var firstFieldID = "";

//    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {
//        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {
//            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
//                returnVal = false;
//                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
//                if (firstFieldID === "") {
//                    firstFieldID = $(mandatoryField).attr("id");
//                }
//            }
//        }
//        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
//            if (!mandatoryField.value) {
//                returnVal = false;
//                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
//                if (firstFieldID === "") {
//                    firstFieldID = $(mandatoryField).attr("id");
//                }
//            }
//        }
//    });
function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;
    var message = "";
    var focusFieldID = "";

    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {

        // Skip validation for txtNifudaNo1,txtChuuban2,txtGyouban2
        if (mandatoryField.id === "txtNifudaNo1" || mandatoryField.id === "txtChuuban2" || mandatoryField.id === "txtGyouban2") {
            return true; // continue loop
        }

        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");

                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length <= 0)) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");

                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });
    var nifuda = $("#txtNifudaNo1").val().trim();
    var chuuban = $("#txtChuuban2").val().trim();
    var gyouban = $("#txtGyouban2").val().trim();

    // Common validation
    //if (!nifuda && !(chuuban && gyouban)) {
    if (!(
        (nifuda && !chuuban && !gyouban) ||     // only nifuda
        (!nifuda && chuuban && gyouban) ||     // chuuban + gyouban
        (nifuda && chuuban && gyouban)         // all three (allowed now)
    )) {
    //Nothing is filled
    //Only chuuban
    //Only gyouban
    //nifuda + only one of(chuuban or gyouban)

        var commonMsg = $("#lblErrorMessageE_NifudaOrChubbanIsrequired").text().trim();

        message += "<br>" + commonMsg;

        if (focusFieldID === "") {
            focusFieldID = "txtNifudaNo1";
        }

        returnVal = false;
    }



    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
        disableSaveButton();
    }

    return returnVal;
}

function IsMandatoryChuubanSearchFieldsFilledHistoryTab() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box

    var firstFieldID = "";

    $.each($.find("#pnlCustomerDetails .Mandatory1"), function (index, mandatoryField) {
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
        showMessageDialog(message, firstFieldID, WARNING);
        disableSaveButton();
        //var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_020").text();
        //showMessageDialog(errorMessage, firstFieldID, WARNING);
    }
    //if (message !== "") {
    //    bootbox.alert({
    //        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
    //        message: message,
    //        centerVertical: true,
    //        callback: function () {
    //            if (firstFieldID !== "") {
    //                $("#" + firstFieldID).focus();
    //            }
    //        }
    //    });
    //}

    return returnVal;
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
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        // $("#imgError").show();
        //$("#imgWarning").show();

        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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

function isDuplicateThabaNo() {

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


    // Check if in input thaba textboxes, same thaba no is entered more than once

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
            message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_006").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_006").text().trim().replace("NNNN", duplicateThaba[0]);;
        }

        showMessageDialog(message, firstFieldID, WARNING);
        return true;
    }
    // Check if entered thaba number is already registered and its combined decision is OK
    //  $.each(inputThaba1, function (index, registeredThabaNumbersAndDecision) {

    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {
            t1 = inputThaba1;
            firstFieldID = inputThaba1;
        }
        else {
            t1 = "";
        }
    }

    if (inputThaba2 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba2]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {
            t2 = inputThaba2;
            firstFieldID = inputThaba2;
        }
        else {
            t2 = "";
        }
    }
    if (inputThaba3 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba3]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {
            t3 = inputThaba3;
            firstFieldID = inputThaba3;
        }
        else {
            t3 = "";
        }
    }
    if (inputThaba4 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba4]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {
            t4 = inputThaba4;
            firstFieldID = inputThaba4;
        }
        else {
            t4 = "";
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
        message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_112").text().replace("NNN", thabamessage.replace(/(.+), $/, '$1'));
        showMessageDialog(message, firstFieldID, WARNING);
        return true;
    } else {

        return false;
    }
    return false;
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
        url: APIURL + "/Straightening/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            showErrorDiv(errorCode);
        });

}



function setEndSysDateTime() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
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
            showErrorDiv(errorCode);
        });

}

function IsEndDataValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_007").text().trim();
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_009").text().trim();
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }



    return true;
}

function checkDate(field) {
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    //re = /^(\d{4})\/(\d{1,2})\/(\d{1,2})$/;
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
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
$('#chkActualDim').click(function () {
    if ($("#chkActualDim").prop("checked") == true) {
        DimentionValue = false;
    } else {
        DimentionValue = true;
    }
});

$('#chkActualLength').click(function () {

    if ($("#chkActualLength").prop("checked") == true) {
        lengthValue = false;
    } else {
        lengthValue = true;
    }
});

$("#ddlNextProcess").focusout(function (event) {
    $("#btnSave").focus();
});
$('#chkFinishedQuantity').click(function () {

    if ($("#chkFinishedQuantity").prop("checked") == true) {
        finishedWeightValue = false;
    } else {
        finishedWeightValue = true;
    }
});

function SetDropDownLists() {
    setNextProcess();
    setTPTest();
    setUnitType();
    setExternalSurfaceCondition();
    setBendStatus();
    setDecision();
    setProcessDefectList();
}


$('#txtMachineType')
    .on('focus', function () {
        machineModified = false;
    })
    .on('input', function () {
        machineModified = true;
    });
$('#txtMachineType').on('input keydown focusout', function (e) {
    var machine = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);

    if (e.which == 13 && machine && machineModified == true) {

        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
            clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");
            clearData();
            resetHistoryDetailsGrid();
            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_023").text().trim();
            showMessageDialog(message, "txtMachineType", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
            enterPressedMachine = true;
            getMachineNamebyCD($(this));
            $("#txtNifudaNo1").focus();
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
        getMachineNamebyCD($(this));
        //$("#txtChuuban2").focus();
        $("#txtNifudaNo1").focus();
    }
    enterPressedMachine = false; // Reset the flag
});

//$('#txtMachineType').on('keydown focusout', function (e) {
//    debugger;
//    var machine = e.target.value ? e.target.value : null;
//    var keycode = (e.keyCode ? e.keyCode : e.which);

//    if (e.which == 13 && machine) {

//        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
//            clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");

//            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_023").text().trim();
//            showMessageDialog(message, "txtMachineType", WARNING);
//        } else {
//            var machineCD = $(this).val().trim();
//            var machineName = machineCD.substring(0, 3);
//            var machineNumber = machineCD.substring(3, 7);
//            $("#hdfMachineCD").val(machineNumber);
//            enterPressedMachine = true;
//            getMachineNamebyCD($(this));

//        }
//    }

//    if (e.type === 'focusout' && machine) {
//        if (enterPressedMachine) {

//            enterPressedMachine = false; // Reset the flag
//            return false;
//        }
//        var machineCD = $(this).val().trim();
//        var machineName = machineCD.substring(0, 3);
//        var machineNumber = machineCD.substring(3, 7);
//        $("#hdfMachineCD").val(machineNumber);
//        getMachineNamebyCD($(this));
//        $("#txtChuuban2").focus();
//    }
//    enterPressedMachine = false; // Reset the flag
//});




function bindEvents() {
    $("#btnInitialize").click(function (event) {
        initialize();
    });

    disableSaveButton();


    //$("#txtStartOperator").keydown(function (e) {
    //    var operator = e.target.value ? e.target.value : null;
    //    var keycode = (e.keyCode ? e.keyCode : e.which);

    //    if (e.which == 13 || e.which == 9 && operator) {
    //        var operatorCD = $(this).val().trim();
    //        var operatorName = operatorCD.substring(0, 3);
    //        var operatorNumber = operatorCD.substring(3, 9);
    //        $("#hdfStartOperatorCD").val(operatorNumber);
    //        getOperatorNameByCD($(this));
    //        $("#txtMetallicLabelNo").focus();
    //    }

    //});

    //Start Operator validation on Machine Textbox(tab,enter,click)
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
            getOperatorNameByCD($(this));

            $("#txtMetallicLabelNo").focus();

        }

        if (e.type === 'focusout' && operator && operatorStartModified == true) {
            if (enterPressedStartOpr) {

                enterPressedStartOpr = false; // Reset the flag
                return false;
            }
            $("#hdfStartOperatorCD").val(operatorNumber);
            getOperatorNameByCD($(this));
            $("#txtMetallicLabelNo").focus();
        }
        enterPressedStartOpr = false; // Reset the flag
    });


    //End Operator validation on Machine Textbox(tab,enter,click)
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
            //  $("#hdfEndOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
            enterPressedEndOpr = true;
            getOperatorNameByCD($(this));

            if ($("#chkActualDim").attr('disabled')) {
                $("#txtActualDim1").focus();
            } else {
                $("#chkActualDim").focus();
            }
        }
        if (e.type === 'focusout' && operator && operatorEndModified == true) {
            if (enterPressedEndOpr) {

                enterPressedEndOpr = false; // Reset the flag
                return false;
            }
            $("#hdfEndOperatorCD").val(operatorNumber);
            getOperatorNameByCD($(this));
            if ($("#chkActualDim").attr('disabled')) {
                $("#txtActualDim1").focus();
            } else {
                $("#chkActualDim").focus();
            }
        }
        enterPressedEndOpr = false; // Reset the flag
    });

    // Bind events
    $("#chkActualDim").change(function (event) {
        toggleActualDimFieldsMandatory($(this));
    });
    $("#chkActualLength").change(function (event) {
        toggleActualLengthFieldsMandatory($(this));
    });
    $("#ddlUnitType").change(function (event) {
        toggleUnitTypeFieldsEnabled($(this));
    });
    $("#btnStartDateTime").click(function (event) {
        setStartSysDateTime();
    });
    $("#btnEndDateTime").click(function (event) {
        setEndSysDateTime();
    });

    //$("#ddlDecision").change(function (event) {               //Commented on 17/8/2017 by Sagar B.When Next Process will be implemented then uncomment this event
    //    decisionVal = $(this).val();
    //    $ddlNextProcess = $("#ddlNextProcess");
    //    if (decisionVal == 1 || decisionVal == 4) {
    //        $ddlNextProcess.addClass("Mandatory");
    //    }
    //    else {
    //        $ddlNextProcess.removeClass("Mandatory");
    //        $("#" + $ddlNextProcess.attr("id") + " option:selected").prop("selected", false);
    //        $("#" + $ddlNextProcess.attr("id") + " option:first").prop("selected", "selected");
    //    }
    //});

    $("#txtNGQuantity1").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGQuantity2").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGQuantity3").keyup(function (event) {
        OKQtyRegionClassChange();
    });


    $btnChuubanSearch.click(function (event) {

        resetResponseTime();
        // enableEndDataPanel();
        //   enableStartDataPanel();
        enableSaveButton();
        var $callFrom = $(this);
        event.preventDefault();

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }

        getMachineDetails($("#hdfMachineCD").val(), $callFrom);

        var nifuda = $("#txtNifudaNo1").val() ? $("#txtNifudaNo1").val().trim() : null;
        var chuuban = $("#txtChuuban2").val() ? $("#txtChuuban2").val().trim() : null;
        var gyouban = $("#txtGyouban2").val() ? $("#txtGyouban2").val().trim() : null;

        var orderNoEntered = (chuuban && gyouban);


        if (nifuda && orderNoEntered) {

            var message = $("#lblErrorMessageE_PleaseEnterNifudaOrChubbanOnly").text().trim();
            showMessageDialog(message, "txtNifudaNo1", WARNING);

            return false;
        }

        if ($("#txtNifudaNo1").val().trim()) {
            GetChuubanGyuobanByNifuda($("#txtNifudaNo1").val(), $callFrom)
        }
        else {
            ValidateChubban("Registration", $("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        }

        
        //getStraighteningDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);
        //getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);

        setMessageDetailsGrid();

    });
    $btnSave.click(function (event) {

        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();
        var focusTab = "";
        if (!IsMandatoryFieldsFilled()) {
            return false;
        }

        if ($("#hdfDBOperation").val().length == 0) {
            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_002").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }

        if (isDuplicateThabaNo()) {
            return false;
        }

        //if (!checkDate($("#txtStartDate"))) {
        //    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_017").text().trim(); // This message will be displayed on dialog box
        //    showMessageDialog(message, $("#txtStartDate"), WARNING);
        //    return false;
        //}

        if (!checkDate($("#txtStartDate"))) {
            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_017").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtStartDate", WARNING);
            return false;
        }

        if ($("#hdfDBOperation").val() == "Insert") {
            if (!IsDateValid(true)) {
                var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_031").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtStartDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
            if (($("#txtThaba1").val() == '') && ($("#txtThaba2").val() == '') && ($("#txtThaba3").val() == '') && ($("#txtThaba4").val() == '')) {
                var message = "";

                message = $.trim($("#lblconfMsg_BundleNoWtNotDisplayed").text());

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
                            saveStraighteningDetails($callFrom);
                        }
                        else {
                            // return false;

                        }
                    },
                    centerVertical: true
                });

            }
            else {

                saveStraighteningDetails($callFrom);
            }


        }

        else {
            // Check all error conditions

            if (!IsEndDataValid()) {
                return false;
            }

            if (isDuplicateThabaNo()) {
                return false;
            }

            if (!checkDate($("#txtEndDate"))) {
                var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_018").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "txtEndDate", WARNING);
                return false;
            }

            if (!IsDateValid(false)) {
                var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_032").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtStartDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }

            if (!checkDate($("#txtEndDate"))) {
                var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_018").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "txtEndDate", WARNING);
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
                dimValues["DIM5"] = $("#txtActualDim5").val() ? $("#txtActualDim5").val() : null;;
                dimValues["DIM6"] = $("#txtActualDim6").val() ? $("#txtActualDim6").val() : null;;

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
                    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_010").text().trim(); //ActDimOutOfTolerance
                    showMessageDialog(message, focusText, WARNING);
                    return false;
                }
            }


            if ($("#chkActualLength").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK && $("#txtLengthToleranceT1").val() != "") {
                var lengthValues = {};
                lengthValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;
                lengthValues["LENGTH_TOLERANCE"] = $("#txtLengthToleranceT1").val() ? $("#txtLengthToleranceT1").val() : null;
                lengthValues["LENGTH1"] = $("#txtActualLength1AfterStraightening").val() ? $("#txtActualLength1AfterStraightening").val() : null;
                lengthValues["LENGTH2"] = $("#txtActualLength2AfterStraightening").val() ? $("#txtActualLength2AfterStraightening").val() : null;
                lengthValues["LENGTH3"] = null;


                var result = IsActualLengthValid(lengthValues);

                if (result.IsSuccess == false) {
                    var focusText = "";
                    if (result.InvalidLength == $("#txtActualLength1AfterStraightening").val()) {
                        focusText = "txtActualLength1AfterStraightening";
                    }
                    if (result.InvalidLength == $("#txtActualLength2AfterStraightening").val() && focusText == "") {
                        focusText = "txtActualLength2AfterStraightening";
                    }
                    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_028").text().trim(); // LengthOutOfTolerance
                    showMessageDialog(message, focusText, WARNING);
                    return false;
                }

            }

            // If +-2% checkbox is checked and Finished Qty is not in 2% Range, show +-2% message.
            // If checkbox is Not checked and Finished Qty is not in 2 % Range, show confirmation message.
            // calculate, Possible causes include the following:
            //  1. PRODUCT_SHAPE, PRODUCT_DIM, and PRODUCT_LENGTH are not properly added in Process Plan.
            //  2. PRODUCT_LENGTH is a text like 2 cuts, so calculations are not possible.
            //  3. The UnitType of numbers is bundles, so calculations are not possible.
            //  4. The NO_OF_UNITS and weight have not been entered during Process end.
            var weightValues = {};
            weightValues["PRODUCT_WEIGHT"] = $("#txtQuantityT1").val() ? $("#txtQuantityT1").val() : null;
            weightValues["INPUT_WEIGHT"] = $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null;
            weightValues["PRODUCT_SHAPE"] = $("#txtProdShapeT1").val() ? $("#txtProdShapeT1").val() : null;
            weightValues["PRODUCT_DIM"] = $("#txtProdDimT1").val() ? $("#txtProdDimT1").val() : null;
            weightValues["NO_OF_UNITS"] = $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null;
            weightValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;


            if ($("#chkFinishedQuantity").prop("checked") == true) {

                if ([
                    "PRODUCT_SHAPE",
                    "PRODUCT_DIM",
                    "PRODUCT_LENGTH"
                ].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== "")) {

                    var value = $("#txtLengthT1").val();

                    if (!isStrictNumeric(value) || value.trim() == "" || $("#ddlUnitType").val() == 2) {

                        var message = $("#lblErrorMessageE_WeightCanNotValidate").text().trim();
                        showMessageDialog(message, "chkFinishedQuantity", WARNING);
                        return false;
                    }

                }
                else {
                    var message = $("#lblErrorMessageE_WeightCanNotValidate").text().trim();
                    showMessageDialog(message, "chkFinishedQuantity", WARNING);
                    return false;
                }
            }

            var format = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            var getResponse = {};

            if (!format.test($("#txtLengthT1").val()) && $("#ddlUnitType").val() != 2) {
                getResponse = IsFinishedQuantityValid(weightValues);
            } else {
                getResponse["IsSuccess"] = true;
            }

            if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true) {

                var message = $("#lblConfirmationMessageC_STRAIGHTENING_FIWeight_025").text().trim(); // FinishedQtyNotIn2PercentRange
                showMessageDialog(message, "txtFinishedQuantity", WARNING);

            } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false) {
                var message = "";

                message = $.trim($("#lblConfirmationMessageC_Hikinuki_FIWeight_027").text());  //WeightIsInvalid

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
                            debugger;
                            //If end time is less than 10 minutes after start time, show confirmation message.
                            if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                                updateStraighteningDetails($callFrom);
                                getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);

                            } else {

                                bootbox.confirm({
                                    title: "<div style='color:white'></div>",
                                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblconfMsg_EndTimeLessThan10MinAfterStartTime").text()) + "</td></tr></table>",
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
                                            updateStraighteningDetails($callFrom);
                                            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
                                        } else {
                                            $("#txtEndDate").focus();
                                        }
                                    },
                                    centerVertical: true
                                });
                            }
                        }
                        else {
                            // return false;

                        }
                    },
                    centerVertical: true
                });

            } else {
                //If end time is less than 10 minutes after start time, show confirmation message.
                if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                    updateStraighteningDetails($callFrom);
                   // getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);

                } else {
                    bootbox.confirm({
                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblconfMsg_EndTimeLessThan10MinAfterStartTime").text()) + "</td></tr></table>",
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
                                updateStraighteningDetails($callFrom);
                            } else {
                                $("#txtEndDate").focus();
                            }

                        },
                        centerVertical: true
                    });
                }
            }
        }
    });

    function isStrictNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
        return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
    }

    $btnSearchT2.click(function (event) {

        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

        if (!IsMandatoryChuubanSearchFieldsFilledHistoryTab()) {
            return false;
        }

        //var retval =
        ValidateChubban("PastHistory", $("#txtChubanT2").val(), $("#txtGyobanT2").val(), $callFrom);
        //if (retval == false) {
        //    return false;
        //}

        //getMachineDetails($("#hdMachineCD").val(), $callFrom);  
    });
}


function isActDimWithinTolerance() {
    retval = true;
    focusFieldID = "";

    // If Dim tolerance is NULL or ZERO, do not calculate further
    if (!$.isNumeric(dimTolMin) && !$.isNumeric(dimTolMax)) {
        retval = true;
    }
    else if ((parseFloat(dimTolMin) == parseFloat("0")) && (parseFloat(dimTolMax) == parseFloat("0"))) {
        retval = true;
    }
    else {
        var stdMaxDimD = parseFloat(dimStd, 3) + parseFloat(dimTolMax, 3);
        var stdMinDimD = parseFloat(dimStd, 3) + parseFloat(dimTolMin, 3);

        var $txtActualDim1 = $("#txtActualDim1");
        var $txtActualDim2 = $("#txtActualDim2");
        var $txtActualDim3 = $("#txtActualDim3");
        var $txtActualDim4 = $("#txtActualDim4");
        var $txtActualDim5 = $("#txtActualDim5");
        var $txtActualDim6 = $("#txtActualDim6");


        $.each([$txtActualDim1, $txtActualDim2, $txtActualDim3, $txtActualDim4, $txtActualDim5, $txtActualDim6],
            function (index, $actDim) {
                if ($.isNumeric($actDim.val()) && (parseFloat($actDim.val()) < stdMinDimD || parseFloat($actDim.val()) > stdMaxDimD)) {
                    focusFieldID = $actDim.attr("id");
                    retval = false;
                }
            });
    }

    return { retval: retval, focusFieldID: focusFieldID };
}

function isActLengthWithinTolerance() {
    retval = true;
    focusFieldID = "";

    // If Length tolerance is NULL or ZERO, do not calculate further
    if (!$.isNumeric(lenTolMin) && !$.isNumeric(lenTolMax)) {
        retval = true;
    }
    else if ((parseFloat(lenTolMin) == parseFloat("0")) && (parseFloat(lenTolMax) == parseFloat("0"))) {
        retval = true;
    }
    else {
        var stdMaxLength = parseFloat(lenStd, 3) + parseFloat(lenTolMax, 3);
        var stdMinLength = parseFloat(lenStd, 3) + parseFloat(lenTolMin, 3);

        var $txtActualLen1 = $("#txtActualLength1AfterStraightening");
        var $txtActualLen2 = $("#txtActualLength2AfterStraightening");


        $.each([$txtActualLen1, $txtActualLen2],
            function (index, $actLen) {
                if ($.isNumeric($actLen.val()) && (parseFloat($actLen.val()) < stdMinLength || parseFloat($actLen.val()) > stdMaxLength)) {
                    focusFieldID = $actLen.attr("id");
                    retval = false;
                }
            });
    }


    return { retval: retval, focusFieldID: focusFieldID };
}


function hideToleranceErrorLabels() {
    $("#lblDimToleranceMsgT1").hide();
    $("#lblLengthMsgT1").hide();
    $("#lblLengthToleranceMsgT1").hide();
    $("#lblDimToleranceMsgT4").hide();
    $("#lblLengthMsgT4").hide();
    $("#lblLengthToleranceMsgT4").hide();
}

function isActiveOperater() {

}

$(document).ready(function () {

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // window.moveTo(0, 0);

    DisablemandatoryNG();

    window.resize(1950, 1070);
    //window.resize(1906, 1040);
    checkBoxCheked();
    SetDropDownLists();
    setGlobalVariables();
    bindEvents();
    setDecimalFormats();
    hideToleranceErrorLabels();
    disableStartDataPanel();
    disableEndDataPanel();
    resetStraighteningMachineDetailsGrid();
    resetHistoryDetailsGrid();
    $(".FocusDefault").focus();

    userName = $("#hdfUserName").val();
});
function DisablemandatoryNG() {
    $("#ddlNGReason1").attr("disabled", "disabled");
    $("#ddlNGReason2").attr("disabled", "disabled");
    $("#ddlNGReason3").attr("disabled", "disabled");

    $("#ddlNGReason1").removeClass("Mandatory");
    $("#ddlNGReason2").removeClass("Mandatory");
    $("#ddlNGReason3").removeClass("Mandatory");
}
function checkBoxCheked() {
    $('#chkActualDim').attr("checked", "checked");
    $('#chkActualLength').prop("checked", "checked");
    $('#chkFinishedQuantity').prop("checked", "checked");
}


//function IsDateValid(boolVal) {
//    debugger;
//    var returnval = true;
//    //var currentDate = new Date();

//    //var minDate = new Date();
//    //var maxDate = new Date();
//    //minDate.setDate(currentDate.getDate() - parseInt(commonDateRange));
//    //maxDate.setDate(currentDate.getDate() + parseInt(commonDateRange));

//    //var minimumDate = minDate.getFullYear() + "/" + String('00' + minDate.getMonth() + 1).slice(-2) + "/" + String('00' + minDate.getDate()).slice(-2) + " " + String('00' + minDate.getHours()).slice(-2) + ":" + String('00' + minDate.getMinutes()).slice(-2);
//    //var maximumDate = maxDate.getFullYear() + "/" + String('00' + maxDate.getMonth() + 1).slice(-2) + "/" + String('00' + maxDate.getDate()).slice(-2) + " " + String('00' + maxDate.getHours()).slice(-2) + ":" + String('00' + maxDate.getMinutes()).slice(-2);

//    var currentDate = new Date();

//    var minDate = new Date(currentDate);
//    minDate.setDate(currentDate.getDate() - parseInt(commonDateRange));

//    var maxDate = new Date(currentDate);
//    maxDate.setDate(currentDate.getDate() + parseInt(commonDateRange));

//    function formatDate(date) {
//        return `${date.getFullYear()}/${String('00' + (date.getMonth() + 1)).slice(-2)}/${String('00' + date.getDate()).slice(-2)} ${String('00' + date.getHours()).slice(-2)}:${String('00' + date.getMinutes()).slice(-2)}`;
//    }

//    var minimumDate = formatDate(minDate);
//    var maximumDate = formatDate(maxDate);
//    if (boolVal === true) {
//        if (Date.parse($("#txtStartDate").val()) <= Date.parse(minimumDate) || Date.parse($("#txtStartDate").val()) >= Date.parse(maximumDate)) {
//            returnval = false;
//          //  var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_031").text().trim() + "(" + $("#lblErrorMessageE_Cold_Drawing_033").text().trim() + commonDateRange + ")";
//            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_029").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";
//            var focusFieldID = "txtStartDate";
//            showMessageDialog(message, focusFieldID, WARNING);

//        }

//    }

//    if (boolVal === false) {
//        if (Date.parse($("#txtEndDate").val()) < Date.parse(minimumDate) || Date.parse($("#txtEndDate").val()) > Date.parse(maximumDate)) {
//            returnval = false;
//            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_030").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";

//            var focusFieldID = "txtEndDate";
//            showMessageDialog(message, focusFieldID, WARNING);

//        }
//    }
//    return returnval;
//}

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
        url: APIURL + "/Straightening2/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    return returnval;
}

function ValidateChubban(tabName, chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/ValidateChubban",
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
                disableStartDataPanel();
                disableEndDataPanel();
                ClearData();
                // Chuuban is marked as completed in the system/
                var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_004").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                debugger;
                disableStartDataPanel();
                disableEndDataPanel();
                disableSaveButton();
                ClearData();
                clearPanel($("#pnlProductDetails"));
                clearPanel($("#pnlProcessStartData"));
                clearPanel($("#pnlProcessEndData"));
                clearPanel($("#pnlCustomerDetails")); // Tab 2
                clearPanel($("#divProductDimensionDetails")); // Tab 2
                clearPanel($("#divMaterialDetails")); // Tab 2
                clearPanel($("#pnlProcessInfo")); // Tab 4
                clearPanel($("#pnlTestSpecifications")); // Tab 4
                clearThabaDetailsGrid();
                clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
                clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
                clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
                resetMessageDetailsGrid();
                resetStraighteningMachineDetailsGrid();
                clearPanel($("#CustomerT2"));
                clearPanel($("#CustomerT3"));

                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_024").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                showMessageDialog(message, "", WARNING);
                ClearData();
                disableStartDataPanel();
                disableEndDataPanel();
                return false;
            }
            else if (result.responce == 1) {

                if (tabName == "Registration") {
                    getStraighteningDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);
                }
                if (tabName == "PastHistory") {
                    getStraighteningHistoryByChuuban($("#txtChubanT2").val(), $("#txtGyobanT2").val(), $callFrom);
                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
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
            url: APIURL + "/Straightening/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
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
            url: APIURL + "/Straightening/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
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
                disableSaveButton();
                var errorMessage = $("#lblErrorMessageE_TagNotFoundRegisterFromHandOver").text();
                showMessageDialog(errorMessage, "txtNifudaNo1", WARNING);
                return false;
            }

            // nifuda is marked as completed in the system.
            else if (jsonResult.nifudA_COMPLETE_DATE != null) {
                disableSaveButton();
                var errorMessage = $("#lblErrorMessageE_NifudaCompleted").text();
                showMessageDialog(errorMessage, "txtNifudaNo1", WARNING);
                return false;
            }
            else {

                $("#txtChuuban2").val(jsonResult.chuuban);
                $("#txtGyouban2").val(jsonResult.gyouban);
                //ValidateChubban(jsonResult.chuuban, jsonResult.gyouban, $callFrom);
                ValidateChubban("Registration", jsonResult.chuuban, jsonResult.gyouban, $callFrom);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
        });
}
$('#txtNifudaNo1').keypress(function (e) {
    debugger;
    var nifudaNo = $('#txtNifudaNo1').val() ? $('#txtNifudaNo1').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && nifudaNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening/MappNifudaByBarcode?nifudaNo=" + nifudaNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;

                if (response.isSuccess) {
                    $('#txtNifudaNo1').val(jsonResult.nifudA_NO);
                    $('#txtChuuban2').focus();
                } else {
                    $('#txtNifudaNo1').val("");

                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});

function DisableSavedThaba() {
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