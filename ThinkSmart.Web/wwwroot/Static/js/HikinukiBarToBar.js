var $btnChuubanSearch;
var $btnSave;
var disabled = "disabled";
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";
var HIDDEN = "Hidden"
var dbOperation;
var maxRegisteredThabaNo = 0;
var $hdfSYSDATE;
var $hdfCultureLoaded;
var $hdfCultureEN;
var $hdfCultureJP;

var DRAWING_CD;
var registeredThabaNumbersAndDecision = {};
var combinedDecisionOK = "";

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
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var lengthValue = true;
var finishedWeightValue = true;
var DimentionValue = true;
let enterPressedMachine = false;
let enterPressedStartOpr = false;
let enterPressedEndOpr = false;
var machineModified = false;
var operatorStartModified = false;
var operatorEndModified = false;
var unitType = "";

userName = ""; // assigned at Document.ready

var machineGridDataTr1 = " <tr id='tr1ItemGrdvHikinukiBartoBarMachine'> "
    + " <td class='td tdH2  padding24Machine' rowspan='2' style='background-color:COLOR; color:#E2E2E2;'> "
    + " <span class='txtColor' style='background-color:COLOR; color:#E2E2E2; height:80px;width:35px'>&nbsp;</span>"     //  + " <input name='txtGridDecisionColor' type='text' class='txtGridDecisionColor' style='height:30px;width:5px '>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' rowspan='2' style='text-align:center'>"   //  + " <td class='td tdH2'  style='text-align:left'>"
    + " <span class='txtGridCustomerName labelClass'>valtxtGridChuban</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' rowspan='2' style='text-align:left'>"
    + " <span class='txtGridChuban labelClass' >valtxtGridCustomerName</span>"
    + " </td>"

    + " <td class='td tdH2  padding24Machine' rowspan='2' style='text-align:left'>"
    + " <span class='txtProdNameCust labelClass' >valtxtProdNameCust</span>"
    + " </td>"

    + " <td class='td tdH2  padding24Machine' rowspan='2' style='text-align:center'>"
    + " <span class='txtGridDimDShape labelClass' >valtxtGridDimDShape</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' rowspan='2' style='text-align:right'>"
    + " <span class='txtGridProdDimD labelClass' >valtxtGridProdDimD</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' rowspan='2' style='text-align:right'>"
    + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimTol</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' style='text-align:right'>"
    + "  <span class=' labelClass' >valtxtGrid_MATL_DIM_D</span>"

    + " </td>"
    + " <td class='td tdH2  padding24Machine' style='text-align:left'>"
    + "  <span class='txtGridCharge labelClass' >valtxtGridCharge</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' style='text-align:left'>"
    + "  <span class='txtGridDIE_DIM labelClass' >valtxtGridDIE_DIM</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' colspan='2' style='text-align:left'>"
    + "  <span class='txtGridDW_ACT_DIM1 labelClass' >valtxtGridDW_ACT_DIM1</span>"
    + " </td>"

    + " <td class='td tdH2  padding24Machine'  style='text-align:right'>"
    + " <span class='txtGridDW_FINISHED_BARS labelClass' >valtxtGridDW_FINISHED_BARS</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine'  style='text-align:right'>"
    + " <span class='txtGridDW_FINISHED_QTY labelClass' >valtxtGridDW_FINISHED_QTY</span>"
    + " </td>"
    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align:left' >"
    + " <span class='txtGridRemarks labelClass'>valtxtGridRemarks</span>"
    + " </td>"
    + "  </tr>";


var machineGridDataTr2 = "<tr id='tr2ItemGrdvHikinukiBartoBarMachine'>"

    + "<td class='td tdH2  padding24Machine'style='text-align:left'> "
    + "<span class='txtGridLength labelClass' >valtxtGridHT_METL</span> "
    + "</td> "
    + "<td class='td tdH2  padding24Machine' style='text-align:left'> "
    + " <span class='txtMetallicPlateCode labelClass' >valtxtMetallicPlateCode</span></td>"
    + "</td> "
    + " <td class='td tdH2  padding24Machine' style='text-align:left'>"
    + "  <span class='txtGridDIE_NO labelClass' >valtxtGridDIE_NO</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' style='text-align:left' >"
    + "  <span class='txtGridDW_EXT_TEXTURE_STATUS labelClass' >valtxtGridDW_EXT_TEXTURE_STATUS</span>"
    + " </td>"
    + " <td class='td tdH2  padding24Machine' colspan='3' style='text-align:left' >"
    + "  <span class='txtGridBEND_STATUS labelClass' >valtxtGridBEND_STATUS</span>"
    + " </td>"

    + " </tr> ";


function Set24HrMachineData(jsonResult) {
    clearTable($("#grdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiBartoBarMachine", "");

    $.each(jsonResult, function (index, jsonObject) {
        if (jsonObject.machinE_CD) {
            var firstRow = machineGridDataTr1;
            if (jsonObject.dW_DECISION != null) {
                if (jsonObject.dW_DECISION == 1) {
                    firstRow = firstRow.replace("COLOR", "green");    // $("●").css('color', 'green');             

                    $(".tdFirst").css('background-color', 'green'); style = 'color:Blue;'
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



            if (jsonObject.pP_PROD_NAME_CUST != null)
                firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
            else
                firstRow = firstRow.replace("valtxtProdNameCust", "");

            if (jsonObject.pP_PROD_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridDimDShape", jsonObject.pP_PROD_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridDimDShape", "");

            if (jsonObject.pP_ACT_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridProdDimD", jsonObject.pP_ACT_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridProdDimD", "");

            if (jsonObject.pP_DIM_TOLERANCE != null)
                firstRow = firstRow.replace("valtxtGridDimTol", jsonObject.pP_DIM_TOLERANCE);
            else
                firstRow = firstRow.replace("valtxtGridDimTol", "");

            if (jsonObject.pP_MATL_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtGrid_MATL_DIM_D", jsonObject.pP_MATL_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGrid_MATL_DIM_D", "");

            if (jsonObject.pP_CHARGE != null)
                firstRow = firstRow.replace("valtxtGridCharge", jsonObject.pP_CHARGE);
            else
                firstRow = firstRow.replace("valtxtGridCharge", "");

            if (jsonObject.diE_SHAPE_STRING != null)
                if (jsonObject.diE_DIM != null)
                    firstRow = firstRow.replace("valtxtGridDIE_DIM", jsonObject.diE_DIM + "" + jsonObject.diE_SHAPE_STRING);
                else
                    firstRow = firstRow.replace("valtxtGridDIE_DIM", "");

            var ACT_DIM_STRING = "";

            if (jsonObject.dW_ACT_DIM1 != null)
            {
                ACT_DIM_STRING = jsonObject.dW_ACT_DIM1.toFixed(3);
            }

            if (jsonObject.dW_ACT_DIM2 != null)
            {
                if (ACT_DIM_STRING == "") {
                    jsonObject.dW_ACT_DIM2.toFixed(3);
                } else
                {
                    ACT_DIM_STRING = ACT_DIM_STRING + ", " + jsonObject.dW_ACT_DIM2.toFixed(3);
                }
                
            }

            if (jsonObject.dW_ACT_DIM3 != null) {
                if (ACT_DIM_STRING == "") {
                    jsonObject.dW_ACT_DIM3.toFixed(3);
                } else {
                    ACT_DIM_STRING = ACT_DIM_STRING + ", " + jsonObject.dW_ACT_DIM3.toFixed(3);
                }

            }

            if (ACT_DIM_STRING != "")
                firstRow = firstRow.replace("valtxtGridDW_ACT_DIM1", ACT_DIM_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDW_ACT_DIM1", "");


            if (jsonObject.uniT_TYPE_STRING != null)
                if (jsonObject.dW_FINISHED_BARS != null)
                    firstRow = firstRow.replace("valtxtGridDW_FINISHED_BARS", jsonObject.dW_FINISHED_BARS + "" + jsonObject.uniT_TYPE_STRING);
                else
                    firstRow = firstRow.replace("valtxtGridDW_FINISHED_BARS", "");


            if (jsonObject.dW_FINISHED_QTY != null)
                firstRow = firstRow.replace("valtxtGridDW_FINISHED_QTY", jsonObject.dW_FINISHED_QTY);
            else
                firstRow = firstRow.replace("valtxtGridDW_FINISHED_QTY", "");

            if (jsonObject.msG_CONTENTS != null)
                firstRow = firstRow.replace("valtxtGridRemarks", jsonObject.msG_CONTENTS);
            else
                firstRow = firstRow.replace("valtxtGridRemarks", "");




            $("#grdvHikinukiBartoBarMachine").append($(firstRow));



            // tr2    (Second row)

            var secondtRow = machineGridDataTr2;

            if (jsonObject.hT_CONDITION_STRING != null)
                secondtRow = secondtRow.replace("valtxtGridHT_METL", jsonObject.hT_CONDITION_STRING);
            else
                secondtRow = secondtRow.replace("valtxtGridHT_METL", "");

            if (jsonObject.dW_METALLIC_PLATE_CODE != null)
                secondtRow = secondtRow.replace("valtxtMetallicPlateCode", jsonObject.dW_METALLIC_PLATE_CODE);
            else
                secondtRow = secondtRow.replace("valtxtMetallicPlateCode", "");


            if (jsonObject.diE_NO != null)
                secondtRow = secondtRow.replace("valtxtGridDIE_NO", jsonObject.diE_NO);
            else
                secondtRow = secondtRow.replace("valtxtGridDIE_NO", "");
            if (jsonObject.dW_EXT_TEXTURE_STATUS != null)
                secondtRow = secondtRow.replace("valtxtGridDW_EXT_TEXTURE_STATUS", jsonObject.dW_EXT_TEXTURE_STATUS_DECODE);
            else
                secondtRow = secondtRow.replace("valtxtGridDW_EXT_TEXTURE_STATUS", "");

            if (jsonObject.dW_BEND_STATUS != null)
                secondtRow = secondtRow.replace("valtxtGridBEND_STATUS", jsonObject.dW_BEND_STATUS_DECODE);
            else
                secondtRow = secondtRow.replace("valtxtGridBEND_STATUS", "");


            $("#grdvHikinukiBartoBarMachine").append($(secondtRow));
        }
    });

}

function setGlobalVariables() {
    $btnChuubanSearch = $("#btnChuubanSearch");
    $btnSave = $("#btnSave");
    $hdfSYSDATE = $("#hdfSYSDATE");
    $hdfCultureLoaded = $("#hdfCultureLoaded");
    $hdfCultureEN = $("#hdfCultureEN");
    $hdfCultureJP = $("#hdfCultureJP");
}

function resetGlobalVariables() {
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

function setDecimalFormats() {

    $("#txtUpperLowerRollerAngle1, #txtUpperLowerRollerAngle2, #txtSTRTonnage").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });

    $("#txtC, #txtMn, #txtUpperRoleElevation, #txtAEdgeDistortion1,  #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtAEdgeDistortion4,#txtBEdgeDistortion1,  #txtBEdgeDistortion2, #txtBEdgeDistortion3, #txtBEdgeDistortion4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtActualDim1, #txtActualDim2, #txtActualDim3, #txtActualDim4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtActualLength1, #txtActualLength2, #txtActualLength3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
}

function hideToleranceErrorLabels() {
    $("#lblDimToleranceMsgT1").hide();
    $("#lblLengthMsgT1").hide();
    $("#lblLengthToleranceMsgT1").hide();
    $("#lblDimToleranceMsgT4").hide();
    $("#lblLengthMsgT4").hide();
    $("#lblLengthToleranceMsgT4").hide();
}




function initialize() {
    enableSearchOrderNoPanel();
    setDecimalFormats(); 
    combinedDecisionOK = "";
    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    disableSaveButton();
    enableSearchPanel();
    $(".FocusDefault").focus();
    disableStartDataPanel();
    disableEndDataPanel();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlCustomerDetails")); // Tab 2
    clearPanel($("#pnlProductDimensionDetails")); // Tab 2
    clearPanel($("#pnlMaterialDetails")); // Tab 2
    clearPanel($("#pnlProcessInfo")); // Tab 4
    clearPanel($("#pnlTestSpecifications")); // Tab 4 

    clearThabaDetailsGrid();
    clearPanel($("#pnlProcessInfo")); // Tab 4
    clearPanel($("#pnlTestSpecifications")); // Tab 4    
    clearPanel($("#pnlFIData"));
    clearTable($("#grdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiBartoBarMachine", "");

    $("#txtNifudaNo").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $('#txtMachineType').val("");

    $("#txtStartDate").val("");
    $("#txtStartTime").val("");
    $("#txtStartOperator").val("");
    $("#txtDieDim").val("");
    $("#txtDieNo").val("");

    $("#txtEndDate").val("");
    $("#txtEndTime").val("");
    $("#txtEndOperator").val("");
    $("#txtActualDim1").val("");
    $("#txtActualDim2").val("");
    $("#txtActualDim3").val("");
    $("#txtActualLength1").val("");
    $("#txtActualLength2").val("");
    $("#txtActualLength3").val("");


    $("#txtFinishedBarCount").val("");
    $("#txtFinishedQuantity").val("");
    $("#txtThaba").val("");
    $("#txtrepaircount").val("");
    $("#txtdisposalbars").val("");
    $("#txtdefectcount").val("");
    $("#txtdefectcount1").val("");
    $("#txtdefectcount2").val("");
    $("#txtRemarks").val("");

    setHTMaterialList();
    // hideToleranceErrorLabels();
    resetProdDataGrid();
    resetGlobalVariables();
    resetHikinukiBtoBHistoryGrid();
    resetMessageDetailsGrid();
    
}

function resetHikinukiBtoBHistoryGrid() {
    clearTable($("#grdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiBartoBarMachine", "");
    $grdvMessage = $("#grdvHikinukiBartoBarMachine");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr1 = $('<tr class="GridNormalRow" style="height:18px;">').append(
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'> ").text(""),
        $("<td align='center' valign='top'> ").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td colspan='2' align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
    );
    var $tr2 = $('<tr class="GridNormalRow" style="height:15px;">').append(
        $("<td  align='center' valign='top'>").text(""),
        $("<td  align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td colspan='2' align='center' valign='top'>").text(""),
        $("<td colspan='2' align='center' valign='top'>").text(""),


    );
    var $tr3 = $('<tr class="GridNormalRow" style="height:10px;>').append(
        $("<td align='center' valign='top'>").text(""),

    );
    $grdvMessage.append($tr1);
    $grdvMessage.append($tr2);
    //  $grdvMessage.append($tr3);

}

function resetMessageDetailsGrid() {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text("")
    );
    $grdvMessage.append($tr);
}

function resetProdDataGrid() {
    $grdvMessage = $("#grdvProdPointData");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text("")
    );
    $grdvMessage.append($tr);

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

function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

function setThabaDetails(jsonResult) {
  
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
        url: APIURL + "/HikinukiBarToBar/GetThabaDetailsByChuuban",
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

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

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

function enableStartDataPanel() {

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
    $("#txtProcCondition").css({display:'revert'});


    //$("#chkmiddle").prop("checked", true);
    //disableRedrawCheckbox();
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
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(disabled).removeAttr(disabled);
        }
    });

    // Set default values

   
    //$("#chkFinishedQuantity").prop('checked', true);

    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $('#ddlUnitType option:eq(0)').prop('selected', true);
    $('#ddlBendStatus option:eq(0)').prop('selected', true);
    $('#ddlExternalSurfaceCondition option:eq(0)').prop('selected', true);

    if ($("#chkmiddle").is(":checked") == true) {
        $("#chkActualDim").prop("checked", false);
        $("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
        $("#txtActualDim1").removeClass("Mandatory1");
        $("#txtActualDim2").removeClass("Mandatory1");
        $("#txtActualDim3").removeClass("Mandatory1");
        $("#checkActualDim").removeClass("Mandatory");

        $("#chkActualLength").prop("checked", false);
        $("#chkActualLength").attr(DISABLED, DISABLED).addClass(DISABLED);
    } else {
        $("#chkActualDim").prop("checked", true);
        $("#chkActualDim").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
        $("#txtActualDim1").addClass("Mandatory1");
        $("#txtActualDim2").addClass("Mandatory1");
        $("#txtActualDim3").addClass("Mandatory1");
        $("#checkActualDim").addClass("Mandatory");
        SetLengthControlProperties($("#txtLengthT1").val());

    }
    
    if (parseInt(unitType) > 0)
    {
        $("#ddlUnitType").val(unitType)
    }
    
}

function enableSearchPanel() {
    $("#txtMachineType").removeAttr(disabled).removeClass(disabled);
    $("#txtChuuban2").removeAttr(disabled).removeClass(disabled);
    $("#txtGyouban2").removeAttr(disabled).removeClass(disabled);
    $("#btnChuubanSearch").removeAttr(disabled).removeClass(disabled);
}

function disableSearchPanel() {

    $("#txtMachineType").attr(disabled, disabled).addClass(disabled);
    $("#txtChuuban2").attr(disabled, disabled).addClass(disabled);
    $("#txtGyouban2").attr(disabled, disabled).addClass(disabled);
    $("#btnChuubanSearch").attr(disabled, disabled).addClass(disabled);
}

function disableSaveButton() {
    $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
}

function enableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
}
function disableStartDataPanel() {
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
    $("#txtProcCondition").css({ display: 'none' });
    $("#ddlProcCondition").html("");
    $("#ddlProcCondition").append($('<option>' + $("#txtProcCondition").val() + '</option>'));
    $("#txtProcCondition").val("");
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
function ClearData() {
    enableSearchOrderNoPanel();
    combinedDecisionOK = "";
    $("#btnSave").attr("disabled", "disabled");
    //enableSaveButton();
    enableSearchPanel();
   // $(".FocusDefault").focus();
    disableStartDataPanel();
    disableEndDataPanel();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlProcessInfo")); // Tab 3
    clearPanel($("#pnlTestSpecifications")); // Tab 3
    clearThabaDetailsGrid();
    //  clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");     // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 2
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 3

    $("#ddlProcCondition").html("");
    $("#txtNifudaNo1").val("");

    $("#txtStartDate").val("");

    $("#txtStartOperator").val("");
    $("#txtNoOfCoils").val("");
    $("#txtEndDate").val("");
    $("#txtEndTime").val("");
    $("#txtEndOperator").val("");
    $("#txtFinishedBarCount").val("");
    $("#txtFinishedQuantity").val("");
    $("#txtThaba1").val("");
    $("#txtBundleNo").val("");
    $("#txtNgWeight").val("");
    $("#txtNGWeight1").val("");
    $("#txtNGWeight2").val("");
    $("#txtNGWeight3").val("");
    $("#txtRemarks").val("");

    //hideToleranceErrorLabels();
    resetGlobalVariables();
    //resetMessageDetailsGrid();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#divPanelCphStartEndData"));
    clearPanel($("#TbProcessInfo")); // Tab 2

    resetMessageDetailsGrid();
    resetProdDataGrid();
}


function GethikinukibartobarDetailsByChuuban(chuuban, gyouban, mechineCD, $callFrom) {

    // Do not proceed if request is already running
    //if ($callFrom.data('requestRunning')) {
    //    return;
    //}
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "',machineCD : '" + mechineCD + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GethikinukibartobarDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, machine_CD: mechineCD, CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
          
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            unitType = "";
            //if (jsonResult === null) {
            //    // Chuuban not found in system.
            //    ClearData();
            //    var errorMessage = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;

            //}
            //
            //if (jsonResult.procesS_PLAN_START_DATE == null) {
            //    disableStartDataPanel();
            //    disableEndDataPanel();
            //    ClearData();
            //    var OrNo = jsonResult.pP_CHUUBAN;
            //    var errorMessage = $("#lblErrorMessageE_HIKI_ENTRY_019").text().replace("99-999999-99", OrNo);
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}

            //if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
            //    // Chuuban is marked as completed in the system/
            //    ClearData();
            //    var errorMessage = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_004").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}
            disableSearchOrderNoPanel();
            setChuubanDetails(jsonResult);
            // setStartData(jsonResult);
            if ($("#hdfDBOperation").val() == "Insert") {

                enableStartDataPanel();
              
                
                setStartSysDateTime();
                clearPanel($("#pnlProcessEndData"));

                disableEndDataPanel();
                $("#txtThaba").removeClass("Mandatory");
                $('#txtThaba').removeAttr("disabled").removeClass("disabled");
                setStartData(jsonResult);
                enableSaveButton();
                if (parseInt(unitType) > 0) {
                    $("#ddlUnitType").val(unitType)
                }
                $("#txtStartOperator").focus();
            }
            else if ($("#hdfDBOperation").val() == "Update") {
               
                enableStartDataPanel();
                setStartData(jsonResult);
                enableEndDataPanel();
                $("#txtThaba").addClass("Mandatory");
                if (parseInt($("#txtThaba").val()) > 0)
                {
                    $("#txtThaba").attr(DISABLED, DISABLED).addClass(DISABLED);
                }
                enableSaveButton();

                SetLengthControlProperties($("#txtLengthT1").val());
               /* var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
                
                if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                    $("#chkActualLength").removeAttr("disabled", "disabled");
                    $("#chkActualLength").prop("checked", true);
                    $("#txtActualLength1").addClass("Mandatory1");
                    $("#txtActualLength2").addClass("Mandatory1");
                    $("#txtActualLength3").addClass("Mandatory1");
                    $("#checkActualLength").addClass("Mandatory"); 
                } else {
                    $("#chkActualLength").attr("disabled", "disabled");
                    $("#chkActualLength").prop("checked", false);
                    $("#txtActualLength1").removeClass("Mandatory1");
                    $("#txtActualLength2").removeClass("Mandatory1");
                    $("#txtActualLength3").removeClass("Mandatory1");
                    $("#checkActualLength").removeClass("Mandatory"); 

                }*/
              
                DisableMandatoryNG();
                // Set End Operator same as Start Operator
                $("#txtEndOperator").val($("#txtStartOperator").val());
                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());

                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndDate").focus();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function getMachineDetails(machineCd, $callFrom) {

    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }

    var ajaxTime = new Date().getTime();

    var jsonData = "{machineCd: '" + machineCd + "', USER_NAME: '" + userName + "' }";


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetMachineDetails?machineCD=" + machineCd + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult.length > 0) {

                clearTable($("#grdvHikinukiBartoBarMachine"), "", "ItemGrdvHikinukiMachine");
                clearTable($("#grdvHikinukiBartoBarMachine"), "", "GridNormalRow");// Tab 2
                Set24HrMachineData(jsonResult);
            } else {
                clearTable($("#grdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiBartoBarMachine", "");
                resetHikinukiBtoBHistoryGrid();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
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
        $('<td class="GridAlignmentRemarks">').text("")
    );
    $grdvMessage.append($tr);
}


function setMessageDetailsGrid(messageDetailsList) {

    $grdvMessage = $("#grdvMessage");

    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
        if ($("#hdfCultureName").val() == "en")
        {
            processName = jsonObject.procesS_NAME_EN;
        }

        // Add data rows


        var $tr = $('<tr class="GridNormalRow">').append(
            $('<td class="GridAlignmentDateTime GridCenter">').text(jsonObject.msG_DATE_STRING),
            $('<td class="GridAlignmentProcessName GridLeft">').text(processName),
            $('<td class="GridAlignmentMachineName GridLeft">').text(jsonObject.machinE_NAME),
            $('<td class="GridAlignmentOperator GridLeft">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks GridLeft">').text(jsonObject.msG_CONTENTS)
        );

        $grdvMessage.append($tr);
    });
}



function setChuubanDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    if (parseInt(jsonResult.uniT_TYPE) > 0) {
        unitType = jsonResult.uniT_TYPE;
    }

    dbOperation = "Insert";
    if ((jsonResult.dW_START_DATE_STRING != null) && (jsonResult.dW_END_DATE_STRING == null) && jsonResult.pasT_DATA_EXIST == "YES") {
        dbOperation = "Update";
    }
    $("#hdfDBOperation").val(dbOperation);

    DRAWING_CD = $.isNumeric(jsonResult.drawinG_CD) ? parseInt(jsonResult.drawinG_CD) : 0;

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#txtThaba").val(jsonResult.dW_TABA_NO_);

    $("#txtProductSpecNoT1").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN);
    $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN);

    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);


    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtCustomerInchargeT4").text(jsonResult.pP_EMPLOYEE_NAME);

    $("#txtHostRefNoT1").val(jsonResult.producT_NYUKO_CD);
    $("#txtHostRefNoT4").val(jsonResult.producT_NYUKO_CD);

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

    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtExtTextureT4").val(jsonResult.pP_EXT_TEXTURE);

    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDimToleranceT4").val(jsonResult.pP_DIM_TOLERANCE);

    //if (jsonResult.pP_DIM_TOL_MIN != null)
    //    dimTolMin = jsonResult.pP_DIM_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    //if (jsonResult.pP_DIM_TOL_MAX != null)
    //    dimTolMax = jsonResult.pP_DIM_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
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

    $("#txtPackingT1").val(jsonResult.pP_PACKING);
    $("#txtPackingT4").val(jsonResult.pP_PACKING);

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


    // 3rd Tab
    //resetMessageDetailsGrid();
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
    if (jsonResult.dW_START_DATE_STRING != null && jsonResult.dW_END_DATE_STRING == null && jsonResult.pasT_DATA_EXIST == "YES") {
               
        $("#txtStartOperator").val(jsonResult.dW_START_OPERATOR_NAME);
        $("#hdfStartOperatorCD").val(jsonResult.dW_START_OPERATOR_CD);
        $("#txtMetallicLabelNo").val(jsonResult.dW_METALLIC_PLATE_CODE);
        $("#txtDieDim").val(jsonResult.diE_DIM);
        //$("#ddlDieShape").val(parseInt(jsonResult.dW_DIE_SHAPE) || $("#ddlDieShape").prop('selectedIndex'));  // Note : This logic is very crucial . Either set the data from DB. If data is not in DB then set what is default selected index on HTML.

        $("#ddlDieShape option:contains(" + jsonResult.diE_SHAPE + ")").prop('selected', true);
        $("#txtDieNo").val(jsonResult.diE_NO);

        $("#ddlProcCondition").val(jsonResult.hT_CONDITION || $("#ddlProcCondition").prop('selectedIndex'));
        $("#txtProcCondition").val(jsonResult.hT_CONDITION);

        if (jsonResult.iM_DRAWING == "1") {
            $("#chkmiddle").prop('checked', true);
            $("#chkRedrawing").attr(DISABLED, DISABLED).addClass(DISABLED);
        }
        else {
            $("#chkmiddle").prop('checked', false);
            $("#chkRedrawing").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
        }

        if (jsonResult.redrawing == "1") {
            $("#chkRedrawing").prop('checked', true);
            $("#chkmiddle").attr(DISABLED, DISABLED).addClass(DISABLED);
        }
        else {
            $("#chkRedrawing").prop('checked', false);
            $("#chkmiddle").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
        }
        if (dbOperation == "Insert") {
            setStartSysDateTime();
        }
        else {
            $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        }

    } else if (jsonResult.dW_START_DATE_STRING != null && jsonResult.dW_END_DATE_STRING != null && jsonResult.pasT_DATA_EXIST == "YES")
    {
        $("#txtMetallicLabelNo").val(jsonResult.dW_METALLIC_PLATE_CODE);
        $("#txtDieDim").val(jsonResult.diE_DIM);
        
        $("#ddlDieShape option:contains(" + jsonResult.diE_SHAPE + ")").prop('selected', true);
        $("#txtDieNo").val(jsonResult.diE_NO);

        $("#ddlProcCondition").val(jsonResult.hT_CONDITION || $("#ddlProcCondition").prop('selectedIndex'));
        $("#txtProcCondition").val(jsonResult.hT_CONDITION);
    }


}


function getOperatorNameByCD($target) {
    var ajaxTime = new Date().getTime();
    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_STA_ENTRY_034").text().trim().replace("*****", $("#tdlblStartOperator").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorCD == "") {
        var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_015").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {
        var jsonData = "{OperatorCD: '" + operatorCD + "', USER_NAME : '" + userName + "'}"

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HikinukiBarToBar/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;

                var jsonresult = response.message;

                if (jsonresult == null || jsonresult == "") {
                    var message = $("#lblErrorMessageHikiNuki_BarToBar_ENTRY_035").text().trim().replace("*****", $("#tdlblStartOperator").text());
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                    $target.focus();
                }

                else {
                    if (jsonresult.iN_ACTIVE_FLAG == 1) {
                        var message = $("#lblErrorMessage_EmployeeInactive").text().trim();
                        showMessageDialog(message, $target.attr("id"), WARNING);
                        $target.val("");
                        $target.focus();
                    }
                    else {
                        $target.val(jsonresult.yobina);
                    }

                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            })
    }
}




function getMachineNamebyCD($target) {

  
    var ajaxTime = new Date().getTime();
    var machineNumber = $target.val().trim();
    var machineval = machineNumber.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineCD = machineNumber.substring(3, 7);
    //  var m = $('#txtMachineType').val("");
    //var machineCD = $target.val().trim().replace(/\D/g, ""); // this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits.

    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_STA_ENTRY_034").text().trim().replace("*****", $("#tdLblMachineType").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
    if (machineCD == "") {
        var message = $("#lblErrorMessageE_Hikinuki_ScanMachineRequired").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
    else if (machineCD.length > 4) {
        var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_013").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    //if (m != "") {
    //    var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_013").text().trim();
    //    showMessageDialog(message, $target.attr("id"), WARNING);
    //    $target.val("");
    //    return false;
    //}
    else {
        var jsonData = "{machineCD: '" + machineCD + "', USER_NAME: '" + userName + "' }";
        var settings = {
            async: false,
            crossDomain: true,
            dataType: "json",
            url: APIURL + "/HikinukiBarToBar/GetMachineNamebyCD?machineCD=" + parseInt(machineCD),
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)

            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var machineName = response.message;
              
                // machine not found.
                if (machineName.key == null || machineName.key == "") {
                    var message = $("#lblErrorMessageHikiNuki_BarToBar_ENTRY_035").text().trim().replace("*****", $("#tdLblMachineType").text());
                    //var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val("");
                }
                //machine is inactive.
                else if (machineName.value == "inactive") {

                    var message = $("#lblErrorMessageE_HIKI_ENTRY_020").text().trim();
                    //clearTable($("#tr1ItemGrdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiBartoBarMachine", "");
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
                message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }


}
$("#chkmiddle").click(function () {
  
    if ($("#chkmiddle").is(":checked") == true) {
        disableRedrawCheckbox();
        //$("#txtActualLength1").removeClass("Mandatory1");
        //$("#txtActualLength2").removeClass("Mandatory1");
        //$("#txtActualLength3").removeClass("Mandatory1");
        $("#chkActualDim").click();  
        $("#chkActualLength").click();
    } else
    {
        enableRedrawCheckbox();
        $("#chkActualDim").click();
        $("#chkActualLength").click();
        SetLengthControlProperties($("#txtLengthT1").val());
    }
});

$("#chkRedrawing").click(function () {
    if ($("#chkRedrawing").is(":checked") == true) {
        $("#chkmiddle").prop("checked", false);
        $("#chkmiddle").attr(DISABLED, DISABLED).addClass(DISABLED);
      
    } else {
        $("#chkmiddle").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    }
});

$("#chkActualDim").click(function () {
    if ($("#chkActualDim").is(":checked") == true) {
        $("#txtActualDim1").addClass("Mandatory1");
        $("#txtActualDim2").addClass("Mandatory1");
        $("#txtActualDim3").addClass("Mandatory1");
        $("#checkActualDim").addClass("Mandatory");
        DimentionValue = false;
    } else {
        $("#txtActualDim1").removeClass("Mandatory1");
        $("#txtActualDim2").removeClass("Mandatory1");
        $("#txtActualDim3").removeClass("Mandatory1");
        $("#checkActualDim").removeClass("Mandatory");
        DimentionValue = true;
    }
});
$("#chkActualLength").click(function () {
    if ($("#chkActualLength").is(":checked") == true) {
        $("#txtActualLength1").addClass("Mandatory1");
        $("#txtActualLength2").addClass("Mandatory1");
        $("#txtActualLength3").addClass("Mandatory1");
        $("#checkActualLength").addClass("Mandatory"); 

    } else {
        $("#txtActualLength1").removeClass("Mandatory1");
        $("#txtActualLength2").removeClass("Mandatory1");
        $("#txtActualLength3").removeClass("Mandatory1");
        $("#checkActualLength").removeClass("Mandatory"); 
    }
});

function enableRedrawCheckbox() {
    
    $("#chkRedrawing").prop("checked", false);
    $("#chkActualDim").prop("checked", false);
    $("#chkActualLength").prop("checked", false);
    $("#chkRedrawing").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    $("#chkActualLength").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    if ($("#txtEndDate").hasClass("disabled") != true) {
        $("#chkActualDim").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    }
}
function disableRedrawCheckbox() {
    $("#chkRedrawing").prop("checked", false);
    $("#chkActualDim").prop("checked", false);
    $("#chkActualLength").prop("checked", false); 
    $("#chkRedrawing").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#chkActualLength").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
}

function SaveHikinukiBartoBarDetails($callFrom) {
  
    var ajaxTime = new Date().getDate();
    var jsonDataForSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val() ? $("#txtChuuban2").val() : null,
        PP_GYOUBAN: $("#txtGyouban2").val() ? $("#txtGyouban2").val() : null,
        DRAWING_CD: DRAWING_CD ? DRAWING_CD : null,
        PP_PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        DIE_DIM: $("#txtDieDim").val() ? $("#txtDieDim").val() : null,
        DIE_SHAPE: $("#ddlDieShape option:selected").text() ? $("#ddlDieShape option:selected").text() : null,
        DIE_NO: $("#txtDieNo").val() ? $("#txtDieNo").val() : null,
        HT_CONDITION: $("#txtProcCondition").val() ? $("#txtProcCondition").val() : null,
        DW_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        IM_DRAWING: ($('#chkmiddle').is(':checked') ? 1 : 0), 
        REDRAWING: ($('#chkRedrawing').is(':checked') ? 1 : 0),
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        CULTURE_NAME: $("#hdfCultureName").val() ? $("#hdfCultureName").val() : null,
        DW_TABA_NO_: $("#txtThaba").val() ? $("#txtThaba").val() : null,
    });

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/HikinukiBarToBarDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            /* After StartData is saved sucessfully, enable EndData Panel for input, 
              and show success message. */
            enableEndDataPanel();
            $("#txtThaba").addClass("Mandatory");
            setDecision();
            setNextProcess();
            
            SetLengthControlProperties($("#txtLengthT1").val());

            var successMessage = $("#lblSuccessMessageS_Hikinukibartobar_ENTRY_001").text();
            showMessageDialog(successMessage, "txtEndDate", SUCCESS);
            var jsonResult = response.message;

            $("#hdfProcessKubunCd").val(jsonResult.fdg);
            setChuubanDetails(jsonResult);
            //setStartData(jsonResult);

            getMachineDetails($("#hdfMachineCD").val(), $callFrom);
            DisableMandatoryNG();

            setEndSysDateTime();
            $("#hdfDBOperation").val("Update");

            $("#btnEndDateTime").click(); // Get current time for end operation.
            $("#txtEndDate").focus();

            $("#txtEndOperator").val($("#txtStartOperator").val());
            $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
            if (parseInt($("#txtThaba").val()) > 0) {
                $("#txtThaba").attr(DISABLED, DISABLED).addClass(DISABLED);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}


function SetLengthControlProperties(lenghtVal) {
  
    var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;

    if ($('#chkmiddle').prop('checked') == false) {
        if (!checkFormat.test(lenghtVal) && lenghtVal != "") {
            $("#chkActualLength").removeAttr("disabled", "disabled");
            $("#chkActualLength").prop("checked", true);
            $("#txtActualLength1").addClass("Mandatory1");
            $("#txtActualLength2").addClass("Mandatory1");
            $("#txtActualLength3").addClass("Mandatory1");
            $("#checkActualLength").addClass("Mandatory");
        } else {
            $("#chkActualLength").attr("disabled", "disabled");
            $("#chkActualLength").prop("checked", false);
            $("#txtActualLength1").removeClass("Mandatory1");
            $("#txtActualLength2").removeClass("Mandatory1");
            $("#txtActualLength3").removeClass("Mandatory1");
            $("#checkActualLength").removeClass("Mandatory");
        }
    }

}
function UpdateHikinukiBarToBarDetails($callFrom) {

    var ajaxTime = new Date().getDate();

    // END DATA
    var jsonDataForUpdate = JSON.stringify({
        DRAWING_CD: DRAWING_CD ? DRAWING_CD : null,
        PP_PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        DW_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        DIE_DIM: $("#txtDieDim").val() ? $("#txtDieDim").val() : null,
        DIE_SHAPE: $("#ddlDieShape option:selected").text() ? $("#ddlDieShape option:selected").text() : null,
        DIE_NO: $("#txtDieNo").val() ? $("#txtDieNo").val() : null,
        HT_CONDITION: $("#txtProcCondition").val() ? $("#txtProcCondition").val() : null,
        IM_DRAWING: ($('#chkmiddle').is(':checked') ? 1 : 0), 
        REDRAWING: ($('#chkRedrawing').is(':checked') ? 1 : 0),

        DW_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_END_OPERATOR_CD: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
        DW_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        DW_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
        DW_ACT_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        DW_ACT_DIM1: $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null,
        DW_ACT_DIM2: $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null,
        DW_ACT_DIM3: $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null,

        DW_ACT_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
        DW_ACT_LENGTH1: $("#txtActualLength1").val() ? $("#txtActualLength1").val() : null,
        DW_ACT_LENGTH2: $("#txtActualLength2").val() ? $("#txtActualLength2").val() : null,
        DW_ACT_LENGTH3: $("#txtActualLength3").val() ? $("#txtActualLength3").val() : null,
        DW_FINISHED_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
        UNIT_TYPE: $("#ddlUnitType").val() ? $("#ddlUnitType").val() : null,
        DW_FINISHED_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
        DW_FINISHED_QTY_CHK: ($('#chkFinishedQuantity').is(':checked') ? 1 : 0),
        DW_TABA_NO_: $("#txtThaba").val() ? $("#txtThaba").val() : null,
        DW_EXT_TEXTURE_STATUS: $("#ddlExternalCondition").val() ? $("#ddlExternalCondition").val() : null,
        DW_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
        DW_NG_BARS1: $("#txtdefectcount").val() ? $("#txtdefectcount").val() : null,
        DW_NG_BARS2: $("#txtdefectcount1").val() ? $("#txtdefectcount1").val() : null,
        DW_NG_BARS3: $("#txtdefectcount2").val() ? $("#txtdefectcount2").val() : null,
        DW_NG_DETAILS1: $("#ddlNGReason1").val() ? $("#ddlNGReason1").val() : null,
        DW_NG_DETAILS2: $("#ddlNGReason2").val() ? $("#ddlNGReason2").val() : null,
        DW_NG_DETAILS3: $("#ddlNGReason3").val() ? $("#ddlNGReason3").val() : null,
        DW_CORRECTED_BARS: $("#txtrepaircount").val() ? $("#txtrepaircount").val() : null,
        DW_DISPOSAL_NO: $("#txtdisposalbars").val() ? $("#txtdisposalbars").val() : null,

        DW_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        USER_NAME: userName ? userName : null
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/HikinukiBarToBarDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataForUpdate
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            disableEndDataPanel();
            disableStartDataPanel();
            disableSearchPanel();
            disableSaveButton();
            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
            getMachineDetails($("#hdfMachineCD").val(), $callFrom);

            var successMessage = $("#lblSuccessMessageS_Hikinukibartobar_ENTRY_001").text();
            showMessageDialog(successMessage, "", SUCCESS);
            var jsonResult = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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

      
        showMessageDialog(message, firstFieldID, WARNING);
    }

    return returnVal;
}

function IsMandatoryChuubanSearchFieldsFilledHistoryTab() {
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
        disableSaveButton();
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

        if (focusFieldID == 'checkActualLength') {
            if ($("#txtActualLength1").val().trim() == "") {
                focusFieldID = "txtActualLength1";
            }
            else if ($("#txtActualLength2").val().trim() == "") {
                focusFieldID = "txtActualLength2";
            }
            else {
                focusFieldID = "txtActualLength3";
            }
        }

        if (focusFieldID == 'checkActualDim') {
            if ($("#txtActualDim1").val().trim() == "") {
                focusFieldID = "txtActualDim1";
            }
            else if ($("#txtActualDim2").val().trim() == "") {
                focusFieldID = "txtActualDim2";
            }
            else {
                focusFieldID = "txtActualDim3";
            }
        }


        showMessageDialog(message, focusFieldID, WARNING)

    }

    return returnVal;
}

function IsDuplicateThabaNo() {

    var inputThaba1 = $("#txtThaba").val().trim();
    
    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK && $("#chkRedrawing").prop("checked") == false) {

            firstFieldID = "txtThaba";
            message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_019").text().trim().replace("NNN", inputThaba1);
            showMessageDialog(message, firstFieldID, WARNING);
            return true;
        }
    }

    return false;
}


function setStartSysDateTime() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
          
            var sysDateTime = response.message;

            //$("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            //$("#txtStartTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
        url: APIURL + "/HikinukiBarToBar/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function IsEndDataValid() {

    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_009").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }


    return true;
}

function checkDate(field) {

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
$("#btnInitialize").click(function (event) {
    initialize();
});
function bindEvents() {
    disableSaveButton();
    setDieDiaShape();
    setExternalCondition();
    setBendStatus();
    setProcessDefectList();
    setHTMaterialList();
    setUnitType();
    setDecision();
    setNextProcess();
  }

//$('#txtMachineType').keypress(function (e) {
//  
//    var machine = e.target.value ? e.target.value : null;
//    var keycode = (e.keyCode ? e.keyCode : e.which);

//    if (keycode == 13 && machine) {
//        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
//            clearTable($("#grdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiMachine", "");
//            resetHikinukiBtoBHistoryGrid();
//            var message = $("#lblErrorMessageE_Hikinuki_ScanMachineRequired").text().trim();
//            showMessageDialog(message, "txtMachineType", WARNING);

//        } else {
//            var machineCD = $(this).val().trim();
//            var machineName = machineCD.substring(0, 3);
//            var machineNumber = machineCD.substring(3, 7);
//            $("#hdfMachineCD").val(machineNumber);
//            //$("#hdfMachineCD").val($(this).val().trim().replace(/\D/g, ""));// this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits.
//            getMachineNamebyCD($(this));
//            if (machineNumber != "") {
//                getMachineDetails($("#hdfMachineCD").val());
//            }

//        }
//    }
//});

//Keydown use because tab & Enter both side use this code.
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
        resetHikinukiBtoBHistoryGrid();
        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
            clearTable($("#grdvHikinukiBartoBarMachine"), "ItemGrdvHikinukiMachine", "");
          
            var message = $("#lblErrorMessageE_Hikinuki_ScanMachineRequired").text().trim();
            showMessageDialog(message, "txtMachineType", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
            enterPressedMachine = true;
            getMachineNamebyCD($(this));
           
        }
    } 

    if (e.type === 'focusout' && machine && machineModified == true)
    {
        if (enterPressedMachine) {
         
            enterPressedMachine = false; // Reset the flag
            return false;
        }
        resetHikinukiBtoBHistoryGrid();
       var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
        getMachineNamebyCD($(this));
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

function disableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "none");
}
function enableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
}

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

    $("#btnStartDateTime").click(function (event) {
        setStartSysDateTime();
    });
    $("#btnEndDateTime").click(function (event) {
        setEndSysDateTime();
    });

    $("#btnChuubanSearch").click(function (event) {

        clearPanel($("#pnlProcessStartData"));
        clearPanel($("#pnlProcessEndData"));

        resetResponseTime();
        var $callFrom = $(this);

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }
        setHTMaterialList();
        ValidateChubban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        getMachineDetails($("#hdfMachineCD").val(), $callFrom);
    });

    $("#btnSave").click(function (event) {

        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();
        if ($("#chkActualLength").is(":checked") == true) {
            if ($("#txtActualLength1").val() == "" || $("#txtActualLength2").val() == "" || $("#txtActualLength3").val() == "") {
                $("#checkActualLength").addClass("Mandatory");
            }
            else {
                $("#checkActualLength").removeClass("Mandatory");
            }
        }
        if ($("#chkActualDim").is(":checked") == true) {
            if ($("#txtActualDim1").val() == "" || $("#txtActualDim2").val() == "" || $("#txtActualDim3").val() == "") {
                $("#checkActualDim").addClass("Mandatory");
            }
            else {
                $("#checkActualDim").removeClass("Mandatory");
            }
        }
        if (!IsMandatoryFieldsFilled()) {
            return false;
        }
        if ($("#hdfDBOperation").val().length == 0) {
            var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_002").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }

        if (IsDuplicateThabaNo()) {
            return false;
        }

        if (!checkDate($("#txtStartDate"))) {
            var message = $("#lblErrorMessageE_HIKI_ENTRY_017").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtStartDate", WARNING);
            return false;
        }

        if ($("#hdfDBOperation").val() == "Insert") {
            if (!IsDateValid(true)) {
                var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_017").text().trim() + "(" + $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_021").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtStartDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
            if (($("#txtThaba").val() == '')) {
                var message = "";

                message = $.trim($("#lblErrorMessageHikiNuki_BarToBar_ENTRY_036").text());

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
                            SaveHikinukiBartoBarDetails($callFrom);
                        } 
                        else {
                            // return false;

                        }
                    },
                    centerVertical: true
                });

            }
            else {
                SaveHikinukiBartoBarDetails($callFrom);
            }
         
        }


        else {
            // Check all error conditions
            if (!IsEndDataValid()) {
                return false;
            }

            if (!checkDate($("#txtEndDate"))) {
                var message = $("#lblErrorMessageE_HIKI_ENTRY_018").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "txtEndDate", WARNING);
                return false;
            }
            if (!IsDateValid(false)) {
                var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_018").text().trim() + "(" + $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_021").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtEndDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }


            if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK) {
                var dimValues = {};
                dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
                dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
                dimValues["DIM1"] = $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null;
                dimValues["DIM2"] = $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null;
                dimValues["DIM3"] = $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null;
                dimValues["DIM4"] = null;
                dimValues["DIM5"] = null;
                dimValues["DIM6"] = null;

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

                    var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_020").text().trim(); //ActDimOutOfTolerance
                    showMessageDialog(message, focusText, WARNING);
                    return false;
                }
            }
            
            if ($("#chkActualLength").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK && $("#txtLengthToleranceT1").val() !="") {
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
                    var message = $("#lblErrorMessageC_Hikinuki_FILenght_026").text().trim(); // LengthOutOfTolerance
                    showMessageDialog(message, focusText, WARNING);
                    return false;
                }


            }




            // Check all confirmation conditions
            // If +-2% checkbox is not checked and Finished Qty is not in 2% Range, show confirmation message

            var weightValues = {};
            weightValues["PRODUCT_WEIGHT"] = $("#txtQuantityT1").val() ? $("#txtQuantityT1").val() : null;
            weightValues["INPUT_WEIGHT"] = $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null;
            weightValues["PRODUCT_SHAPE"] = $("#txtProdShapeT1").val() ? $("#txtProdShapeT1").val() : null;
            weightValues["PRODUCT_DIM"] = $("#txtProdDimT1").val() ? $("#txtProdDimT1").val() : null;
            weightValues["NO_OF_UNITS"] = $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null;
            weightValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;

            if ($("#chkFinishedQuantity").prop("checked") == true)
            {
                
                if (["PRODUCT_SHAPE", "PRODUCT_DIM", "PRODUCT_LENGTH"].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== "")) {

                    var value = $("#txtLengthT1").val();
                    // Check - Length only allow numeric &  Unit Type - Not allow BN 

                    if (!IsNotNumeric(value) || value.trim() == "" || $("#ddlUnitType").val() == 2) {

                        var message = $("#lblErrorMessageHikiNuki_BarToBar_ENTRY_037").text().trim();
                        showMessageDialog(message, "chkFinishedQuantity", WARNING);
                        return false;
                    }

                } else
                {
                    var message = $("#lblErrorMessageHikiNuki_BarToBar_ENTRY_037").text().trim();
                    showMessageDialog(message, "chkFinishedQuantity", WARNING);
                    return false;
                }
            }
            var format = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            var getResponse = {};
            if ($("#ddlUnitType").val() == 2 && $("#chkFinishedQuantity").prop("checked") == false)
            {
                getResponse["IsSuccess"] = true;
            }
            else if (!format.test($("#txtLengthT1").val())) {
                getResponse  = IsFinishedQuantityValid(weightValues);
            } else
            {
                getResponse["IsSuccess"] = true;
            }

           
            if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true) {
                var message = "";

               



                //message = $.trim($("#lblConfirmationMessageC_Hikinuki_FIWeight_025").text());

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
                //    callback: function (getVal) {

                //        if (getVal) {
                //            message = $("#lblErrorMessageE_HIKI_ENTRY_011").text().trim();
                //            showMessageDialog(message, "txtFinishedQuantity", WARNING);

                //        }

                //    },
                //    centerVertical: true
                //});
                 message = $("#lblErrorMessageE_HIKI_ENTRY_011").text().trim();
                            showMessageDialog(message, "txtFinishedQuantity", WARNING);

            } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false)
            {
                var message = "";

                message = $.trim($("#lblConfirmationMessageC_Hikinuki_FIWeight_025").text());

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
                                UpdateHikinukiBarToBarDetails($callFrom);
                               

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
                                            UpdateHikinukiBarToBarDetails($callFrom);
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
                if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                    UpdateHikinukiBarToBarDetails($callFrom);


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
                                UpdateHikinukiBarToBarDetails($callFrom);
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

function IsNotNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
    return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
}
$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // window.moveTo(0, 0);
    window.resize(1980, 1030);
    hideToleranceErrorLabels();

    DisableMandatoryNG();

    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    setGlobalVariables();
    bindEvents();
    setDecimalFormats();
    //hideToleranceErrorLabels();
    disableStartDataPanel();
    disableEndDataPanel();
    resetHikinukiBtoBHistoryGrid();
    $(".FocusDefault").focus();
    userName = $("#hdfUserName").val();
    $("#24HrsMachineData").css({ 'height': '845px' });
});

$("#btnSearchT2").click(function () {
    resetHikinukiBtoBHistoryGrid();
    resetResponseTime();

    if (!IsMandatoryChuubanSearchFieldsFilledHistoryTab()) {
        return false;
    }
    getMachineDetails($("#hdMachineCD").val(), $callFrom);
});


function setDieDiaShape() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetDieDiaShape",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindDieDiashapedropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindDieDiashapedropdown(jsonResult) {
    $("#ddlDieShape").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDieShape").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setExternalCondition() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/GetExteriorCondition",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindExternalConditiondropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindExternalConditiondropdown(jsonResult) {
    $("#ddlExternalCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalCondition").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setBendStatus() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/GetBendStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindBendStatusList(jsonResult) {
    $("#ddlBendStatus").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setDecision() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            binddecisionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function binddecisionDropdown(jsonResult) {
    $("#ddlDecision").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setProcessDefectList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetProcessDefect",
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
}

function setHTMaterialList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/GetHTMaterial",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindHTMaterialDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindHTMaterialDropdown(jsonResult) {
    //$("#listHtMatl").html("").append($('<option value=""></option>'));
    //$.each(jsonResult, function (key, jsonObj) {
    //  
    //     $("#listHtMatl").append($('<option value="' + jsonObj.name + '" </option>'));
    //});
    $("#ddlProcCondition").html("");//.append($('<option value=""></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcCondition").append($('<option value="' + jsonObj.name + '">' + jsonObj.name + '</option>'));
        if ($("#txtProcCondition").val() == "") {
            $("#txtProcCondition").val(jsonObj.name);
        }
    });
    
}

function setNextProcess() {
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindNextProcessDropdown(jsonResult) {
    $("#ddlNextProcess").html("");//.append($('<option value="0"></option>'));

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
        });
    }
}

function setUnitType() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/GetUnitType",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindUnitTypeDropdown(jsonResult) {
    // $("#ddlUnitType").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

$('#chkActualLength').click(function () {

    if ($("#chkActualLength").prop("checked") == true) {
        lengthValue = false;
    } else {
        lengthValue = true;
    }
});
$('#chkFinishedQuantity').click(function () {

    if ($("#chkFinishedQuantity").prop("checked") == true) {
        finishedWeightValue = false;
    } else {
        finishedWeightValue = true;
    }
});

$("#txtdefectcount").keyup(function (event) {
    OKQtyRegionClassChange();
});
$("#txtdefectcount1").keyup(function (event) {
    OKQtyRegionClassChange();
});
$("#txtdefectcount2").keyup(function (event) {
    OKQtyRegionClassChange();
});

function OKQtyRegionClassChange() {

    var defectcount = $("#txtdefectcount").val();
    if (defectcount > 0) {
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason1").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason1").removeClass("Mandatory");
        $("#ddlNGReason1 option:selected").prop("selected", false);
        $("#ddlNGReason1").attr("disabled", "disabled");
    }
    var defectcount1 = $("#txtdefectcount1").val();
    if (defectcount1 > 0) {
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason2").removeAttr("disabled")
    }
    else {

        $("#ddlNGReason2").removeClass("Mandatory");
        $("#ddlNGReason2 option:selected").prop("selected", false);
        $("#ddlNGReason2").attr("disabled", "disabled");
    }
    var defectcount2 = $("#txtdefectcount2").val();
    if (defectcount2 > 0) {
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason3").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason3").removeClass("Mandatory");
        $("#ddlNGReason3 option:selected").prop("selected", false);
        $("#ddlNGReason3").attr("disabled", "disabled");
    }
}

$("#TbProcessInfo-tab").click(function () {
    window.resizeTo(2000, 1070);
});
$("#chkRedrawing").focusout(function (event) {
    $("#txtThaba").focus();
});
$("#txtThaba").focusout(function (event) {
    if ($("#ddlExternalCondition").attr(DISABLED)) {
        $("#btnSave").focus();
    } else
    {
        $("#ddlExternalCondition").focus();
    }
});

$("#ddlNextProcess").focusout(function (event) {
    $("#btnSave").focus();
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
        url: APIURL + "/HikinukiBarToBar/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
        url: APIURL + "/HikinukiBarToBar/ValidateChubban",
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

                // Chuuban is marked as completed in the system/
                ClearData();
                var errorMessage = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_004").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                // Chuuban not found in system.
                disableStartDataPanel();
                disableSaveButton();
                ClearData();
                var errorMessage = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                disableStartDataPanel();
                disableEndDataPanel();
                ClearData();
                var message = $("#lblErrorMessageE_HIKI_ENTRY_019").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                showMessageDialog(message, "", WARNING);
                return false;
            }
            else if (result.responce == 1) {

                GethikinukibartobarDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
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
            url: APIURL + "/HikinukiBarToBar/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});

$("#chkmiddle").focusout(function (event) {

    if ($("#chkRedrawing").hasClass("disabled") == true) {
        $("#txtThaba").focus();
    } else {
        $("#chkRedrawing").focus();
    }
});

function DisableMandatoryNG() {
    $("#ddlNGReason1").attr("disabled", "disabled"); 
    $("#ddlNGReason2").attr("disabled", "disabled");
    $("#ddlNGReason3").attr("disabled", "disabled");

    $("#ddlNGReason1").removeClass("Mandatory");
    $("#ddlNGReason2").removeClass("Mandatory");
    $("#ddlNGReason3").removeClass("Mandatory");
}

//$(document).on("keydown", function (e) {

//    switch (e.key) {

//        case "F1":
//            e.preventDefault();

//            if ($('#pnlSearch').css('pointer-events') !== 'none')
//            {
//                $("#btnChuubanSearch").click();
//            }
//            break;

//        case "F2":
//            e.preventDefault();
//            $("#btnSave").click();
//            break;

//        case "F3":
//            e.preventDefault();
//            $("#btnInitialize").click();
//            break;
//    }

//});