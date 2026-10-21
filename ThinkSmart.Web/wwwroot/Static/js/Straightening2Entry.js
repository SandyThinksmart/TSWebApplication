
var $btnChuubanSearch;
var $btnSave;
var $btnSearchT2;
var indexDDLDecisionNothing = 0;
var combinedDecisionOK = 1;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
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
var DISABLED = "disabled";
let enterPressedMachine = false;
var machineModified = false;
var operatorStartModified = false;
var operatorEndModified = false;
let enterPressedStartOpr = false;
let enterPressedEndOpr = false;

// Error log information. To be sent with each AJAX request. 
userName = ""; // assigned at Document.ready
// End error log information.


// Following decision legends are to show decision colors on Top Thaba Grid.
//var decisionLegends = {
//    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
//    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
//    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
//    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>",
//    "5": "<span id='lblStar' class='labelClass' style='color:Darkred;'>■</span>"
//}



var machineGridDataTr1 = " <tr id='tr1ItemGrdvStraighteningMachine'> "
    + " <td class='td tdH2' rowspan='2' style='background-color:COLOR; color:#E2E2E2;'> "
    + " <span class='txtColor' style='background-color:COLOR; color:#E2E2E2; height:80px;width:35px'>&nbsp;</span>"     //  + " <input name='txtGridDecisionColor' type='text' class='txtGridDecisionColor' style='height:30px;width:5px '>"
    + " </td>"
    + " <td class='td tdH2 tdpadddingLeft' rowspan='2' style='text-align:left;white-space:nowrap'>"
    + " <span class='txtMetallicPlateCode labelClass' >valtxtGridCustomerName</span></td>"
    + " <td class='td tdH2 tdpadddingLeft tdpadddingRight' style='text-align:center;white-space:nowrap' rowspan='2' >"
    + " <span class='txtGridChuban labelClass' >valtxtGridChuban</span>"
    + " </td>"
   
    + " <td class='td tdH2 tdpadddingLeft' rowspan='2' style='text-align:left;white-space:nowrap'>"
    + " <span class='txtProdNameCust labelClass' >valtxtProdNameCust</span>"
    + " </td>"
    + " <td class='td tdH2' rowspan='2' style='text-align:center;white-space:nowrap'>"
    + " <span class='txtGridShape labelClass' >valtxtGridShape</span>"
    + " </td>"
    + " <td class='td tdH2 tdpadddingRight tdpadddingLeft' rowspan='2' style='text-align:right;white-space:nowrap'>"
    + " <span class='txtGridDimD labelClass' >valtxtGridDimD</span>"
    + " </td>"
    + " <td class='td tdH2 padddingLeftRight' rowspan='2' style='white-space:nowrap;text-align:center;'>"
    + "  <span class='txtGridDimTol labelClass' >valtxtGridDimTol</span>"
    + " </td>"
    + " <td class='td tdH2 tdpadddingRight tdpadddingLeft' rowspan='2' style='text-align:right;white-space:nowrap' >"
    + "  <span class='txtGridDimD labelClass' >valtxtGridMatlDimD</span> "
    + " </td>"
    + "<td class='td tdH2 tdpadddingLeft'  style='text-align:left;white-space:nowrap'> "
    + "<span class='txtGridCharge labelClass' >valtxtGridCharge</span> "
    + "</td> "
    + "<td class='td tdH2 tdpadddingRight tdpadddingLeft' rowspan='2' style='text-align:Right;white-space:nowrap'> "
    + "<span class='labelClass' >valtxtGridDimBefore1</span> "
    + "</td> "
    + "<td class='td tdH2 tdpadddingRight tdpadddingLeft' rowspan='2' style='text-align:Right;white-space:nowrap'> "
    + "<span class='labelClass' >valtxtGridDimBefore2</span> "
    + "</td> "
    + "<td class='td tdH2 padddingLeftRight' rowspan='2' style='text-align:left;white-space:nowrap'> "
    + "<span class='labelClass'>valDimAfter</span> "
    + "</td> "


    + "<td class='td tdH2 tdpadddingRight tdpadddingLeft' style='text-align:Right;white-space:nowrap'> "
    + "<span class='txtGridCharge labelClass'>valtxtGridBars</span> "
    + "</td> "
    + "<td class='td tdH2 tdpadddingLeft' style='text-align:left;white-space:nowrap'> "
    + "<span class='txtGridCharge labelClass'>valtxtGridExtTexture</span> "
    + "</td> "
    + "<td class='td tdH2 tdpadddingLeft' rowspan='2' style='text-align:left;white-space:nowrap'> "
    + "<span class='txtGridDimD labelClass'>valtxtGridRemarks</span> "
    + "</td> "


    /*   
       + " <td class='td tdH2'>"
      + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimBefore1</span>"
      + " </td>"
       + " <td class='td tdH2'>"
      + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimAfter1</span>"
      + " </td>"
        + " <td class='td tdH2'>"
      + "  <span class='txtGridTabaNo labelClass' >valtxtGridLengthAfter1</span>"
      + " </td>"
      + " <td class='td tdH2' rowspan='2' style='text-align:right'>"
      + " <span class='txtGridNoOfCoils labelClass' >valtxtGridNoOfCoils</span>"
      + " </td>"
      + " <td class='td tdH2' rowspan='2' style='text-align:right'>"
      + " <span class='txtGridQuantity labelClass' >valtxtGridQuantity</span>"
      + " </td>"
      + " <td class='td tdH2' rowspan='2' style='text-align:left' >"
      + " <span class='txtGridDieDim labelClass'>valtxtGridRemarks</span>"
      + " </td>"*/
    + "  </tr>";


var machineGridDataTr2 = "<tr id='tr2ItemGrdvStraighteningMachine'>"

    //+ "  <td class='td tdH2'  > "
    //+ " <span class='txtGridHTMatl labelClass' >valtxtGridHTMatl</span>"
    //+ "</td> "
    + "  <td class='td tdH2 padddingLeftRight' style='text-align:left;white-space:nowrap'> "
    + "<span class='txtGridMetallicPlateCode labelClass' >valtxtMetallicPlateCode</span> "
    + "</td> "

    + "  <td class='td tdH2 tdpadddingRight tdpadddingLeft' style='text-align:right;white-space:nowrap'> "
    + "<span class='labelClass' >valtxtWeight</span> "
    + "</td> "

    + "  <td class='td tdH2 tdpadddingLeft' style='text-align:left;white-space:nowrap'> "
    + "<span class='labelClass' >valtxtBendStatus</span> "
    + "</td> "

    /*  + "<td class='td tdH2' style='text-align:right'> "
      + "<span class='txtGridLength labelClass' >valtxtGridLength</span> "
      + "</td> "
        + "<td class='td tdH2'> "
      + "<span class='txtGridLength labelClass' >valtxtGridLengthTol</span> "
      + "</td> "
    //  + "<td class='td tdH2'> "
    //  + "<span class='txtGridTabaNo3 labelClass' >valtxtGridTabaNo3_4</span> "
     // + "</td> "
      + " <td class='td tdH2'>"
        + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimBefore2</span>"
        + " </td>"
         + " <td class='td tdH2'>"
        + "  <span class='txtGridTabaNo labelClass' >valtxtGridDimAfter2</span>"
        + " </td>"
          + " <td class='td tdH2'>"
        + "  <span class='txtGridTabaNo labelClass' >valtxtGridLengthAfter2</span>"
        + " </td>"*/
    + " </tr> ";













function hideToleranceErrorLabels() {
    $("#lblDimToleranceMsgT1").hide();
    $("#lblLengthMsgT1").hide();
    $("#lblLengthToleranceMsgT1").hide();
    $("#lblDimToleranceMsgT4").hide();
    $("#lblLengthMsgT4").hide();
    $("#lblLengthToleranceMsgT4").hide();
}

function setGlobalVariables() {
    $btnChuubanSearch = $("#btnChuubanSearch");
    $btnSave = $("#btnSave");
    // $btnSearchT2 = $("#btnSearchT2");
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
    indexDDLDecisionNothing = 0;
    combinedDecisionOK = 1;
    typeDropDown = "select-one";
    typeText = "text";
    disabled = "disabled";
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
    PRODUCTSHAPE_R = "R";
    PRODUCTSHAPE_H = "H";
    PRODUCTSHAPE_S = "S";
    DISABLED = "disabled";
    enterPressedMachine = false;
    machineModified = false;
    operatorStartModified = false;
    operatorEndModified = false;

}


function enableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
}

function enableSearchPanel() {
    $("#txtMachineType").removeAttr(disabled).removeClass(disabled);
    $("#txtNifudaNo").removeAttr(disabled).removeClass(disabled);
    $("#txtChuuban2").removeAttr(disabled).removeClass(disabled);
    $("#txtGyouban2").removeAttr(disabled).removeClass(disabled);
    $("#btnChuubanSearch").removeAttr(disabled).removeClass(disabled);
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

function setStartData(jsonResult) {
    debugger;
    if (jsonResult.stR_END_DATE_STRING == null && jsonResult.stR_START_DATE_STRING !== null && jsonResult.pasT_DATA_EXIST == "YES") {
        enableSaveButton();
        /*  $("#btnEndDateTime").click();*/
        $("#txtStartOperator").val(jsonResult.stR_START_OPERATOR_NAME);
        $("#hdfStartOperatorCD").val(jsonResult.stR_START_OPERATOR_CD);

        $("#ddlTPTest").val(jsonResult.tP_TEST || $("#ddlTPTest").prop('selectedIndex'));

        //if (jsonResult.tP_TEST == null) {
        //    $('#ddlTPTest option:eq(0)').prop('selected', true);
        //}


        $("#txtMetallicLabelNo").val(jsonResult.stR_METALLIC_PLATE_CODE);
        $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
        $("#txtActualDimDBeforeStraightening1").val(jsonResult.stR_DIM_BEFORE_1);
        $("#txtActualDimDBeforeStraightening2").val(jsonResult.stR_DIM_BEFORE_2);
        $("#txtActualLengthBeforeStraightening1").val(jsonResult.stR_LENGTH_BEFORE_1);
        $("#txtActualLengthBeforeStraightening2").val(jsonResult.stR_LENGTH_BEFORE_2);

        $("#txtReservationNum").val(jsonResult.reservatioN_NUMBER);
        $("#txtThaba1").val(jsonResult.stR_TABA_NO_1);
        $("#txtThaba2").val(jsonResult.stR_TABA_NO_2);
        $("#txtThaba3").val(jsonResult.stR_TABA_NO_3);
        $("#txtThaba4").val(jsonResult.stR_TABA_NO_4);
       
        debugger;
        $("#txtStartDate").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    }
    //else if (jsonResult.shoW_DEFAULT == 1) //1 = latest Record found in the last 30 days. 
    else if (jsonResult.stR_END_DATE_STRING != null && jsonResult.stR_START_DATE_STRING != null && jsonResult.pasT_DATA_EXIST == "YES" && jsonResult.shoW_DEFAULT == 1)
    {
        $("#txtMetallicLabelNo").val(jsonResult.stR_METALLIC_PLATE_CODE);
        $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
        $("#ddlTPTest").val(jsonResult.tP_TEST || $("#ddlTPTest").prop('selectedIndex'));
        $("#ddlUnitType").val(jsonResult.uniT_TYPE);
    }
}


function saveStraighteningDetails($callFrom) {

    var ajaxTime = new Date().getDate();
    var jsonDataSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val() ? $("#txtChuuban2").val() : null,
        PP_GYOUBAN: $("#txtGyouban2").val() ? $("#txtGyouban2").val() : null,
        STRAIGHTENING_CD: straighteningCD ? straighteningCD : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
        STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
        STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
        STR_LENGTH_BEFORE_1: $("#txtActualLengthBeforeStraightening1").val() ? $("#txtActualLengthBeforeStraightening1").val() : null,
        STR_LENGTH_BEFORE_2: $("#txtActualLengthBeforeStraightening2").val() ? $("#txtActualLengthBeforeStraightening2").val() : null,
        RESERVATION_NUMBER: $("#txtReservationNum").val() ? $("#txtReservationNum").val() : null,
        RESTRAIGHTING: ($('#chkRestraighting').is(':checked') ? 1 : 0),
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        USER_NAME: userName ? userName : null,
        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,

        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,

        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        CULTURE_NAME: $("#hdfCultureName").val(),
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/StraighteningDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataSave
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
           
            var successMessage = $("#lblSuccessMessageS_STRAIGHTENING2_ENTRY_001").text();
            showMessageDialog(successMessage, "txtEndDate", SUCCESS);            

            var jsonResult = response.message;            

            //clearPanel($("#pnlProcessEndData"));
            setChuubanDetails(jsonResult);
            setStartData(jsonResult);
            getMachineDetails($("#hdfMachineCD").val(), $callFrom);
            $("#hdfDBOperation").val("Update");

            $("#btnEndDateTime").click(); // Get current time for end operation.
            $("#txtEndDate").focus();
            setChuubanDimLenToleranceMessages();
            enableEndDataPanel();
            enableDimAfterMandatory();
            // disableDefectDropdown();
            DisablemandatoryNG();
            $("#txtThaba1").addClass("Mandatory");
            var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;

            if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                $("#chkActualLength").removeAttr("disabled", "disabled");
                $("#chkActualLength").prop("checked", true);
                $("#txtActualLength1AfterStraightening").addClass("MandatoryTextBox");
                $("#txtActualLength2AfterStraightening").addClass("MandatoryTextBox");
            } else {
                $("#chkActualLength").attr("disabled", "disabled");
                $("#chkActualLength").prop("checked", false);
                $("#txtActualLength1AfterStraightening").removeClass("MandatoryTextBox");
                $("#txtActualLength2AfterStraightening").removeClass("MandatoryTextBox");
            }

            DisableSavedThaba();
            $("#txtEndOperator").val($("#txtStartOperator").val());
            $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function updateStraighteningDetails($callFrom) {
    debugger;
    let val = $("#hdfldProcessPlanCd").val();

    var ajaxTime = new Date().getDate();
    debugger;
    var jsonDataUpdate = JSON.stringify({
        STRAIGHTENING_CD: straighteningCD ? straighteningCD : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null, //$("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
        STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
        STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
        STR_LENGTH_BEFORE_1: $("#txtActualLengthBeforeStraightening1").val() ? $("#txtActualLengthBeforeStraightening1").val() : null,
        STR_LENGTH_BEFORE_2: $("#txtActualLengthBeforeStraightening2").val() ? $("#txtActualLengthBeforeStraightening2").val() : null,
        RESERVATION_NUMBER: $("#txtReservationNum").val() ? $("#txtReservationNum").val() : null,
        RESTRAIGHTING: ($('#chkRestraighting').is(':checked') ? 1 : 0),
        STR_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_END_OPERATOR_CD: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
        STR_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        STR_DIM_AFTER_X_1: $("#txtDimAfterX1").val() ? $("#txtDimAfterX1").val() : null,
        STR_DIM_AFTER_X_2: $("#txtDimAfterX2").val() ? $("#txtDimAfterX2").val() : null,
        STR_DIM_AFTER_X_3: $("#txtDimAfterX3").val() ? $("#txtDimAfterX3").val() : null,
        STR_DIM_AFTER_Y_1: $("#txtDimAfterY1").val() ? $("#txtDimAfterY1").val() : null,
        STR_DIM_AFTER_Y_2: $("#txtDimAfterY2").val() ? $("#txtDimAfterY2").val() : null,
        STR_DIM_AFTER_Y_3: $("#txtDimAfterY3").val() ? $("#txtDimAfterY3").val() : null,
        STR_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
        STR_LENGTH_AFTER_1: $("#txtActualLength1AfterStraightening").val() ? $("#txtActualLength1AfterStraightening").val() : null,
        STR_LENGTH_AFTER_2: $("#txtActualLength2AfterStraightening").val() ? $("#txtActualLength2AfterStraightening").val() : null,
        STR_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
        UNIT_TYPE: $("#ddlUnitType").val() ? $("#ddlUnitType").val() : null,
        STR_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
        STR_FINISHED_QTY_CHK: ($('#chkFinishedQuantity').is(':checked') ? 1 : 0),
        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        STR_EXT_TEXTURE: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
        STR_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
        STR_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : null,
        STR_NG_DETAILS1: $("#ddlNGReason1").val() ? $("#ddlNGReason1").val() : null,
        STR_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : null,
        STR_NG_DETAILS2: $("#ddlNGReason2").val() ? $("#ddlNGReason2").val() : null,
        STR_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : null,
        STR_NG_DETAILS3: $("#ddlNGReason3").val() ? $("#ddlNGReason3").val() : null,
        STR_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        STR_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
        STR_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        STR_REMARKS: $.trim($("#txtRemarks").val()) ? $.trim($("#txtRemarks").val()) : null,
        USER_NAME: userName ? userName : null
    });

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/StraighteningDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            $("#dialogMessageIcon img").hide();
            $("#imgSuccess").show();

            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);

            var successMessage = $("#lblSuccessMessageS_STRAIGHTENING2_ENTRY_001").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            getMachineDetails($("#hdfMachineCD").val(), $callFrom);
            disableStartDataPanel();
            disableSaveButton();
            disableEndDataPanel();

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
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
function toggleActualDimFieldsMandatory($this) {

    /* if ($this.is(':checked')) {
         $("#txtActualDim1").addClass("Mandatory");
     } else {
         $("#txtActualDim1").removeClass("Mandatory");
     }*/
}

/*
function toggleUnitTypeFieldsEnabled($this) {
    if ($this.find(":selected").val() == '1') {
        $("#chkFinishedQuantity").removeAttr("disabled");
        $("#chkFinishedQuantity").prop('checked', true);
    } else {
        $("#chkFinishedQuantity").attr("disabled", "disabled");
        $("#chkFinishedQuantity").prop('checked', false);
    }
}
*/

function initialize() {
    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    enableSaveButton();
    enableSearchPanel();
    enableSearchOrderNoPanel();
    $(".FocusDefault").focus();
    disableStartDataPanel();
    //disableEndDataPanel();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlCustomerDetails")); // Tab 2
    clearPanel($("#pnlProductDimensionDetails")); // Tab 2
    clearPanel($("#pnlMaterialDetails")); // Tab 2
    clearPanel($("#pnlProcessInfo")); // Tab 4
    clearPanel($("#pnlTestSpecifications")); // Tab 4    
    clearThabaDetailsGrid();
    //clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");    // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
    //clearTable($("#gridProcessHistory "), "GridNormalRow");// Tab 2
    clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");
    disableSaveButton();
    disableEndDataPanel();
    $("#txtNifudaNo").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $('#txtMachineType').val("");
    hideToleranceErrorLabels();
    resetGlobalVariables();
    resetMessageDetailsGrid();
    $("#txtActualLength1AfterStraightening").removeClass("MandatoryTextBox");
    $("#txtActualLength2AfterStraightening").removeClass("MandatoryTextBox");
    resetStraighteningMachineDetailsGrid();

}


function ClearPanelData() {

    disableEndDataPanel();

    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearThabaDetailsGrid();
    //clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");    // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
    //clearTable($("#gridProcessHistory "), "GridNormalRow");// Tab 2
    // clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");

    $("#txtNifudaNo").val("");

    hideToleranceErrorLabels();

    resetGlobalVariables();
    resetMessageDetailsGrid();
}

function getThabaDetailsByChuuban(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
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
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

}


function setThabaDetails(jsonResult) {
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
    debugger;

    //  $("#txtFinalRegisteredThabaNoValue").text(maxRegisteredThabaNo);
}

//Added because after extract click disable chuuban.(All panel)
function disableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "none");
}
function enableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
}

function getMachineNamebyCD($target) {
    debugger;
    var ajaxTime = new Date().getTime();
    var machineCD = $target.val().trim();
    var machineval = machineCD.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineNumber = machineCD.substring(3, 7);
    // var machineCD = $target.val().trim().replace(/\D/g, ""); // this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits. 
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
            url: APIURL + "/Straightening2/GetMachineNamebyCD?machineCD=" + parseInt(machineNumber),
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                var machineName = response.message;

                if (machineName.key == null || machineName.key == "") {
                    var message = $("#lblErrorMessageE_BarCodeNotFound_002").text().trim().replace("*****", $("#tdLblMachineType").text());
                    // var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_013").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the machineName barcode again
                }
                else if (machineName.value == "inactive") {
                    var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_020").text().trim();
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
                message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
}

function getOperatorNameByCD($target) {
    debugger;
    var ajaxTime = new Date().getTime();

    var operatorCD = $target.val().trim();
    var operatorVal = operatorCD.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorNumber = operatorCD.substring(3, 9);

    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_STA_ENTRY_034").text().trim().replace("*****", $("#lblStartOperator").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    else if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_STA_ENTRY_008").text().trim().replace("*****", $("#lblStartOperator").text());
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    //else if (operatorNumber.length > 6) {
    //    var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_013").text().trim();
    //    showMessageDialog(message, $target.attr("id"), WARNING);
    //    $target.val("");
    //    return false;
    //}
    else {
        var jsonData = "{OperatorCD: '" + operatorNumber + "', USER_NAME: '" + userName + "' }";
        var jsonRetval = "";

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening2/GetOperatorNamebyCD?OperatorCD=" + operatorNumber,
            method: "Get",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                var jsonresult = response.message;

                if (jsonresult == null || jsonresult == "") {
                    var message = $("#lblErrorMessageE_BarCodeNotFound_002").text().trim().replace("*****", $("#lblStartOperator").text());
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                    $target.focus();
                }

                else {
                    if (jsonresult.iN_ACTIVE_FLAG == 1) {
                        var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_014").text().trim();
                        showMessageDialog(message, $target.attr("id"), WARNING);
                        $target.val("");
                        $target.focus();
                    } else {
                        $target.val(jsonresult.yobina);
                        $("#ddlDimDecision").focus();
                    }

                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            })
    }
}
function bindTPTestList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/GetTPTestList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

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
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindTPTestDropdown(jsonResult) {
    $("#ddlTPTest").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
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
        url: APIURL + "/Straightening2/GetSysDateTime",
        method: "GET",
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

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
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
        url: APIURL + "/Straightening2/GetSysDateTime",
        method: "GET",
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
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function IsEndDataValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_008").text().trim();
        var focusFieldID = "txtEndDate";
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

function setUnitType() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/GetUnitType",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
    //$("#ddlUnitType").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
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
        url: APIURL + "/Straightening2/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {

    /*  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));*/
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
        });
    }
}

function setDecision() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/GetDecision",
        method: "GET",
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
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function binddecisionDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));

    });
}
function setProcessDefectList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetProcessDefect",
        method: "GET",
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
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
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
function setExteriorConditionKubunGet() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/GetExteriorCondition",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindExteriorConditionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindExteriorConditionDropdown(jsonResult) {
    $("#ddlExternalSurfaceCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalSurfaceCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setBendStatus() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/GetBendStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindBendStatusDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindBendStatusDropdown(jsonResult) {
    $("#ddlBendStatus").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function Set24HrMachineData(jsonResult) {
    debugger;
    clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");

    $.each(jsonResult, function (index, jsonObject) {
        if (jsonObject.machinE_CD) {
            var firstRow = machineGridDataTr1;
            debugger;
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
                    firstRow = firstRow.replace("COLOR", "darkred");  //firstRow = $("●").css('color', 'Yellow');
                    firstRow = firstRow.replace("#E2E2E2", "darkred");
                }
                else {
                    firstRow = firstRow.replace("COLOR", "White");   // firstRow = $("●").css('color', 'White');
                    firstRow = firstRow.replace("#E2E2E2", "White");
                }
            }

            if (jsonObject.pP_CHUUBAN_GYOUBAN != null)
                firstRow = firstRow.replace("valtxtGridChuban", jsonObject.pP_CHUUBAN_GYOUBAN);
            else
                firstRow = firstRow.replace("valtxtGridChuban", "");

            if (jsonObject.pP_CUSTOMER_NAME != null)
                firstRow = firstRow.replace("valtxtGridCustomerName", jsonObject.pP_CUSTOMER_NAME);        // [11/10/2019] Nitesh B.  replaced "STR" with " value"
            else
                firstRow = firstRow.replace("valtxtGridCustomerName", "");

            if (jsonObject.pP_PROD_NAME_CUST != null)
                firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
            else
                firstRow = firstRow.replace("valtxtProdNameCust", "");

            if (jsonObject.pP_PROD_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridShape", jsonObject.pP_PROD_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridShape", "");
            debugger;
            if (jsonObject.pP_PROD_DIM_D_SHAPE_STRING != null)
                firstRow = firstRow.replace("valtxtGridDimD", jsonObject.pP_PROD_DIM_D_SHAPE_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDimD", "");

            if (jsonObject.pP_DIM_TOLERANCE != null)
                firstRow = firstRow.replace("valtxtGridDimTol", jsonObject.pP_DIM_TOLERANCE);
            else
                firstRow = firstRow.replace("valtxtGridDimTol", "");

            if (jsonObject.pP_MATL_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridMatlDimD", jsonObject.pP_MATL_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridMatlDimD", "");

            if (jsonObject.stR_DIM_BEFORE_1 != null)
                firstRow = firstRow.replace("valtxtGridDimBefore1", jsonObject.stR_DIM_BEFORE_1_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDimBefore1", "");

            if (jsonObject.stR_DIM_BEFORE_2 != null)
                firstRow = firstRow.replace("valtxtGridDimBefore2", jsonObject.stR_DIM_BEFORE_2_STRING);
            else
                firstRow = firstRow.replace("valtxtGridDimBefore2", "");

            if (jsonObject.pP_CHARGE != null)
                firstRow = firstRow.replace("valtxtGridCharge", jsonObject.pP_CHARGE);
            else
                firstRow = firstRow.replace("valtxtGridCharge", "");


            if (jsonObject.alL_DIM_AFTER_X != null)
                firstRow = firstRow.replace("valDimAfter", jsonObject.alL_DIM_AFTER_X);
            else
                firstRow = firstRow.replace("valDimAfter", "");


            if (jsonObject.stR_BARS != null)
                firstRow = firstRow.replace("valtxtGridBars", jsonObject.stR_BARS_DECODE);
            else
                firstRow = firstRow.replace("valtxtGridBars", "");

            if (jsonObject.stR_EXT_TEXTURE_STATUS != null)
                firstRow = firstRow.replace("valtxtGridExtTexture", jsonObject.stR_EXT_TEXTURE_STATUS);
            else
                firstRow = firstRow.replace("valtxtGridExtTexture", "");


            if (jsonObject.stR_REMARKS != null)
                firstRow = firstRow.replace("valtxtGridRemarks", jsonObject.stR_REMARKS);
            else
                firstRow = firstRow.replace("valtxtGridRemarks", "");

            /*   
   
               
   
               
   
             
   
            
   
              
               if (jsonObject.stR_TABA_NO_3_4 != null)
                   firstRow = firstRow.replace("valtxtGridTabaNo3_4", jsonObject.stR_TABA_NO_3_4);
               else
                   firstRow = firstRow.replace("valtxtGridTabaNo3_4", "");
   
               if (jsonObject.stR_DIM_BEFORE_1 != null)
                   firstRow = firstRow.replace("valtxtGridDimBefore1", jsonObject.stR_DIM_BEFORE_1);
               else
                   firstRow = firstRow.replace("valtxtGridDimBefore1", "");
   
               if (jsonObject.stR_DIM_AFTER_1 != null)
                   firstRow = firstRow.replace("valtxtGridDimAfter1", jsonObject.stR_DIM_AFTER_1);
               else
                   firstRow = firstRow.replace("valtxtGridDimAfter1", "");
   
               if (jsonObject.stR_LENGTH_AFTER_1 != null)
                   firstRow = firstRow.replace("valtxtGridLengthAfter1", jsonObject.stR_LENGTH_AFTER_1);
               else
                   firstRow = firstRow.replace("valtxtGridLengthAfter1", "");
   
               if (jsonObject.stR_BARS_DECODE != null)
                   firstRow = firstRow.replace("valtxtGridNoOfCoils", jsonObject.stR_BARS_DECODE);
               else
                   firstRow = firstRow.replace("valtxtGridNoOfCoils", "");
   
               if (jsonObject.stR_QTY != null)
                   firstRow = firstRow.replace("valtxtGridQuantity", jsonObject.stR_QTY);
               else
                   firstRow = firstRow.replace("valtxtGridQuantity", "");
   
               if (jsonObject.stR_REMARKS != null)
                   firstRow = firstRow.replace("valtxtGridRemarks", jsonObject.stR_REMARKS);
               else
                   firstRow = firstRow.replace("valtxtGridRemarks", "");
   
                   */

            $("#grdvStraighteningMachine").append($(firstRow));



            // tr2    (Second row)

            var secondtRow = machineGridDataTr2;

            if (jsonObject.stR_METALLIC_PLATE_CODE != null)
                secondtRow = secondtRow.replace("valtxtGridHTMatl", "");
            else
                secondtRow = secondtRow.replace("valtxtGridHTMatl", "");

            if (jsonObject.stR_METALLIC_PLATE_CODE != null)
                secondtRow = secondtRow.replace("valtxtMetallicPlateCode", jsonObject.stR_METALLIC_PLATE_CODE);
            else
                secondtRow = secondtRow.replace("valtxtMetallicPlateCode", "");

            if (jsonObject.stR_QTY != null)
                secondtRow = secondtRow.replace("valtxtWeight", jsonObject.stR_QTY);
            else
                secondtRow = secondtRow.replace("valtxtWeight", "");

            if (jsonObject.stR_BEND_STATUS_VAL != null)
                secondtRow = secondtRow.replace("valtxtBendStatus", jsonObject.stR_BEND_STATUS_VAL);
            else
                secondtRow = secondtRow.replace("valtxtBendStatus", "");



            /*  if (jsonObject.pP_PROD_SPEC_NO != null)
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
  
  
            secondtRow = secondtRow.replace("valtxtGridTabaNo3_4", "");
  
              if (jsonObject.stR_DIM_BEFORE_2 != null)
                  secondtRow = secondtRow.replace("valtxtGridDimBefore2", jsonObject.stR_DIM_BEFORE_2);
              else
                  secondtRow = secondtRow.replace("valtxtGridDimBefore2", "");
  
              if (jsonObject.stR_DIM_AFTER_2 != null)
                  secondtRow = secondtRow.replace("valtxtGridDimAfter2", jsonObject.stR_DIM_AFTER_2);
              else
                  secondtRow = secondtRow.replace("valtxtGridDimAfter2", "");
  
              if (jsonObject.stR_LENGTH_AFTER_2 != null)
                  secondtRow = secondtRow.replace("valtxtGridLengthAfter2", jsonObject.stR_LENGTH_AFTER_2);
              else
                  secondtRow = secondtRow.replace("valtxtGridLengthAfter2", "");*/

            $("#grdvStraighteningMachine").append($(secondtRow));
        }
    });

}
function disableDefectDropdown() {

    $("#ddlNGReason1").attr("DISABLED", "DISABLED").addClass("DISABLED");
    $("#ddlNGReason2").attr("DISABLED", "DISABLED").addClass("DISABLED");
    $("#ddlNGReason3").attr("DISABLED", "DISABLED").addClass("DISABLED");

}

$("#txtNGQuantity1").keyup(function () {
    OKQtyRegionClassChange();
    //if ($("#txtNGQuantity1").val() > 0) {
    //    $("#ddlNGReason1").addClass("Mandatory");
    //    $("#ddlNGReason1").removeClass("DISABLED").removeAttr("DISABLED");
    //}
    //else {
    //    $("#ddlNGReason1").removeClass("Mandatory");
    //    $("#txtNGQuantity1").val("");
    //    $("#ddlNGReason1").val("0");
    //    $("#ddlNGReason1").attr("DISABLED", "DISABLED").addClass("DISABLED");
    //}

});

$("#txtNGQuantity2").keyup(function () {

    OKQtyRegionClassChange();
    //if ($("#txtNGQuantity2").val() > 0) {
    //    $("#ddlNGReason2").addClass("Mandatory");
    //    $("#ddlNGReason2").removeClass("DISABLED").removeAttr("DISABLED");
    //}
    //else {
    //    $("#ddlNGReason2").removeClass("Mandatory");
    //    $("#txtNGQuantity2").val("");
    //    $("#ddlNGReason2").val("0");
    //    $("#ddlNGReason2").attr("DISABLED", "DISABLED").addClass("DISABLED");
    //}

});
$("#txtNGQuantity3").keyup(function () {
    OKQtyRegionClassChange();
    //if ($("#txtNGQuantity3").val() > 0) {
    //    $("#ddlNGReason3").addClass("Mandatory");
    //    $("#ddlNGReason3").removeClass("DISABLED").removeAttr("DISABLED");
    //}
    //else {
    //    $("#ddlNGReason3").removeClass("Mandatory");
    //    $("#txtNGQuantity3").val("");
    //    $("#ddlNGReason3").val("0");
    //    $("#ddlNGReason3").attr("DISABLED", "DISABLED").addClass("DISABLED");
    //}

});


$('#chkActualLength').click(function () {
    debugger;
    if ($("#chkActualLength").prop("checked") == true) {
        // lengthValue = false;
        $("#txtActualLength1AfterStraightening").addClass("MandatoryTextBox");
        $("#txtActualLength2AfterStraightening").addClass("MandatoryTextBox");

    } else {
        $("#txtActualLength1AfterStraightening").removeClass("MandatoryTextBox");
        $("#txtActualLength2AfterStraightening").removeClass("MandatoryTextBox");
        // lengthValue = true;
    }
});
//$('#chkFinishedQuantity').click(function () {
//    debugger;
//    if ($("#chkFinishedQuantity").prop("checked") == true) {
//        finishedWeightValue = false;
//    } else {
//        finishedWeightValue = true;
//    }
//});


function getMachineDetails(machineCd, $callFrom) {

    debugger;
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
        url: APIURL + "/Straightening2/GetMachineDetails?machineCD=" + machineCd + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            debugger;
            if (jsonResult.length > 0) {

                clearTable($("#grdvStraighteningMachine"), "", "ItemGrdvStraighteningMachine");
                clearTable($("#grdvStraighteningMachine"), "", "GridNormalRow");// Tab 2
                Set24HrMachineData(jsonResult);
            } else {
                clearTable($("#grdvStraighteningMachine"), "ItemGrdvStraighteningMachine", "");
                resetStraighteningMachineDetailsGrid();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
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
        // if (!($(checkBox).hasClass("AlwaysDisabled"))) {
        $(checkBox).removeClass(disabled).removeAttr(disabled);
        $(checkBox).prop("checked", true);
        // }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(disabled).removeAttr(disabled);
        }
    });

    // Set default values

    toggleActualDimFieldsMandatory($("#chkActualDim"));

    /* $("#chkActualLength").prop('checked', true);
     toggleActualLengthFieldsMandatory($("#chkActualLength"));*/

    /* $('#ddlUnitType option:eq(0)').prop('selected', true);
     toggleUnitTypeFieldsEnabled($("#ddlUnitType"));*/

    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });


    //$('#ddlUnitType option:eq(0)').prop('selected', true);
    $('#ddlBendStatus option:eq(0)').prop('selected', true);
    // $('#ddlTPTest option:eq(0)').prop('selected', true);

    $('#ddlExternalSurfaceCondition option:eq(0)').prop('selected', true);
}

function enableSearchPanel() {
    $("#txtMachineType").removeAttr(disabled).removeClass(disabled);
    $("#txtNifudaNo").removeAttr(disabled).removeClass(disabled);
    $("#txtChuuban2").removeAttr(disabled).removeClass(disabled);
    $("#txtGyouban2").removeAttr(disabled).removeClass(disabled);
    $("#btnChuubanSearch").removeAttr(disabled).removeClass(disabled);
}

function disableSearchPanel() {
    $("#txtMachineType").attr(disabled, disabled).addClass(disabled);
    $("#txtNifudaNo").attr(disabled, disabled).addClass(disabled);
    $("#txtChuuban2").attr(disabled, disabled).addClass(disabled);
    $("#txtGyouban2").attr(disabled, disabled).addClass(disabled);
    $("#btnChuubanSearch").attr(disabled, disabled).addClass(disabled);
}

function disableSaveButton() {
    $("#btnSave").attr("disabled", "disabled");
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
    $.each($endDataPanel.find("select"), function s(index, dropdown) {
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

function setStartDataPanelDefaultData(jsonResult) {

    if (jsonResult.sT_START_DATE_STRING != null) {
        var startDateTime = jsonResult.sT_START_DATE_STRING;
        $("#txtStartDate").val($.format.date(new Date(startDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    }
    else {
        $("#btnStartDateTime").click();
    }

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

function getStraighteningDetailsByChuuban(chuuban, gyouban, machine_CD, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "',machine_CD : '" + machine_CD + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetStraighteningDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, machine_CD: machine_CD, CULTURE_NAME: $("#hdfCultureName").val() })
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            //if (jsonResult === null) {
            //    disableStartDataPanel();
            //    disableSaveButton();
            //    ClearPanelData();
            //    // Chuuban not found in system.
            //    var errorMessage = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //} else {

            //    enableSaveButton();
            //}


            //if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
            //    disableStartDataPanel();
            //    // Chuuban is marked as completed in the system/
            //    var errorMessage = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_004").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}

            //if (jsonResult.procesS_PLAN_START_DATE == null) {
            //    // OrNo Not Started FromHandOver
            //    var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_022").text().trim().replace("99-999999-99", jsonResult.pP_CHUUBAN);
            //    showMessageDialog(message, "", WARNING);
            //   // ClearData();
            //    return false;
            //}

            enableStartDataPanel();
            setChuubanDetails(jsonResult);
            setStartData(jsonResult);
            setProcessDefectList();
            disableSearchOrderNoPanel();
            if ($("#hdfDBOperation").val() == "Insert") {
                setStartSysDateTime();
                enableStartDataPanel(jsonResult);
                setStartDataPanelDefaultData(jsonResult);
               // clearPanel($("#pnlProcessEndData"));
                setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed before disableEndDataPanel(). It is because setChuubanDimLenToleranceMessages() will enable some fields based on values of Dim and Len Tolerances, and disableEndDataPanel() will disable fields again. 
                disableEndDataPanel();
                $("#txtThaba1").val("");
                $("#txtThaba2").val("");
                $("#txtThaba3").val("");
                $("#txtThaba4").val("");
                $("#txtThaba1").removeClass("Mandatory");
                $('#txtThaba1').removeAttr("disabled").removeClass("disabled");
                $('#txtThaba2').removeAttr("disabled").removeClass("disabled");
                $('#txtThaba3').removeAttr("disabled").removeClass("disabled");
                $('#txtThaba4').removeAttr("disabled").removeClass("disabled");
                $("#txtStartOperator").focus();
            }
            else if ($("#hdfDBOperation").val() == "Update") {

                enableEndDataPanel();
                enableDimAfterMandatory();
                $("#txtThaba1").addClass("Mandatory");
                var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;

                if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                    $("#chkActualLength").removeAttr("disabled", "disabled");
                    $("#chkActualLength").prop("checked", true);
                    $("#txtActualLength1AfterStraightening").addClass("MandatoryTextBox");
                    $("#txtActualLength2AfterStraightening").addClass("MandatoryTextBox");
                } else {
                    $("#chkActualLength").attr("disabled", "disabled");
                    $("#chkActualLength").prop("checked", false);
                    $("#txtActualLength1AfterStraightening").removeClass("MandatoryTextBox");
                    $("#txtActualLength2AfterStraightening").removeClass("MandatoryTextBox");
                }
                // disableDefectDropdown();
                DisablemandatoryNG();
                // Set End Operator same as Start Operator
                $("#txtEndOperator").val($("#txtStartOperator").val());
                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());

                DisableSavedThaba();

                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndOperator").focus();
                if (parseInt(jsonResult.uniT_TYPE) > 0) {
                    $("#ddlUnitType").val(jsonResult.uniT_TYPE);
                }
                setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed after enableEndDataPanel(). It is because enableEndDataPanel() enables all fields, and setChuubanDimLenToleranceMessages() will again disable some fields based on values of Dim and Len Tolerances.
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function ValidateChubban(chuuban, gyouban, $callFrom) {


    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/ValidateChubban",
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
                // Chuuban is marked as completed in the system/
                var errorMessage = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_004").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                disableStartDataPanel();
                disableSaveButton();
                ClearPanelData();
                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_022").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                showMessageDialog(message, "", WARNING);
                // ClearData();
                return false;
            }
            else if (result.responce == 1) {
                enableSaveButton();
                getStraighteningDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function setMessageDetailsGrid(messageDetailsList) {

    $grdvMessage = $("#grdvMessage");

    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        //var processName = jsonObject.procesS_NAME_JP;
        //if (($hdfCultureLoaded.val()).indexOf($hdfCultureEN.val()) > -1) // > -1 means substring is present
        //{
        //    processName = jsonObject.procesS_NAME_EN;
        //}
        if ($("#hdfCultureName").val() == "en") {
            processName = jsonObject.procesS_NAME_EN;
        }
        else {
            processName = jsonObject.procesS_NAME_JP;
        }

        // Add data rows


        var $tr = $('<tr class="GridNormalRow">').append(
            $('<td class="GridAlignmentDateTime">').text(jsonObject.msG_DATE_STRING),
            $('<td class="GridAlignmentProcessName">').text(processName),
            $('<td class="GridAlignmentMachineName">').text(jsonObject.machinE_NAME),            
            $('<td class="GridAlignmentOperator">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks">').text(jsonObject.msG_CONTENTS)
        );

        $grdvMessage.append($tr);
    });
}



function setChuubanDimLenToleranceMessages() {
    // Show/hide Tolerance Messages
    debugger;
    if ((dimTolMin == null || dimTolMin == "" || Math.abs(parseFloat(dimTolMin)) == parseFloat(0))
        && (dimTolMax == null || dimTolMax == "" || Math.abs(parseFloat(dimTolMax)) == parseFloat(0))) {
        $("#lblDimToleranceMsgT1").show();
        $("#lblDimToleranceMsgT4").show();
        //$("#chkActualDim").prop('checked', false);
        //$("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
        // toggleActualDimFieldsMandatory($("#chkActualDim"));
    }
    else {
        $("#lblDimToleranceMsgT1").hide();
        $("#lblDimToleranceMsgT4").hide();
        //$("#chkActualDim").prop('checked', true);
        //$("#chkActualDim").removeClass(DISABLED).removeAttr(DISABLED);
        // toggleActualDimFieldsMandatory($("#chkActualDim"));
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

        //  $("#chkFinishedQuantity").prop('checked', false);
        //   $("#chkFinishedQuantity").attr(DISABLED, DISABLED).addClass(DISABLED);
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
        //$("#chkActualLength").prop('checked', true);
        //$("#chkActualLength").removeClass(DISABLED).removeAttr(DISABLED);
        // toggleActualLengthFieldsMandatory($("#chkActualLength"));
    }
    else {
        //$("#chkActualLength").prop('checked', true);
        //$("#chkActualLength").removeClass(DISABLED).removeAttr(DISABLED);
        // toggleActualLengthFieldsMandatory($("#chkActualLength"));
    }
}


function setChuubanDetails(jsonResult) {
    debugger;
    combinedDecisionOK = jsonResult.decisioN_OK;
    dbOperation = "Insert";
    if ((jsonResult.stR_START_DATE_STRING != null) && (jsonResult.stR_END_DATE_STRING == null) && jsonResult.pasT_DATA_EXIST == "YES") {
        dbOperation = "Update";
    }
    $("#hdfDBOperation").val(dbOperation);

    straighteningCD = $.isNumeric(jsonResult.straighteninG_CD) ? parseInt(jsonResult.straighteninG_CD) : 0;

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
   

    $("#txtProductSpecNoT1").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN);
    $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN);

    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);


    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtCustomerInchargeT4").val(jsonResult.pP_EMPLOYEE_NAME);

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

    if (jsonResult.diM_TOL_MIN_CHR != null)
        dimTolMin = jsonResult.diM_TOL_MIN_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.diM_TOL_MAX_CHR != null)
        dimTolMax = jsonResult.diM_TOL_MAX_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    dimStd = jsonResult.pP_PROD_DIM_D;

    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);

    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtStampT4").val(jsonResult.pP_STAMP);

    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_STRING);
    debugger;
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
        $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
    }
    else {
        $("#txtLengthToleranceT1").val('');
        $("#txtLengthToleranceT4").val('');
    }

    if (jsonResult.lengtH_TOL_MIN_CHR != null)
        lenTolMin = jsonResult.lengtH_TOL_MIN_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.lengtH_TOL_MAX_CHR != null)
        lenTolMax = jsonResult.lengtH_TOL_MAX_CHR.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
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

    $("#txtHostRefNoT1").val(jsonResult.nyukA_CD);
    $("#txtHostRefNoT4").val(jsonResult.nyukA_CD);

    $("#txtConditionT1").val(jsonResult.pP_CONDITION_DESCRIPTION);
    $("#txtConditionT4").val(jsonResult.pP_CONDITION_DESCRIPTION);

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

    if (firstFieldID == 'txtActualDimBeforeCheck') {
        if ($("#txtActualDimDBeforeStraightening1").val().trim() == "") {
            firstFieldID = "txtActualDimDBeforeStraightening1";
        } else {
            firstFieldID = "txtActualDimDBeforeStraightening2";
        }
    }
    if (firstFieldID == 'txtActualLengthBeforeCheck') {
        if ($("#txtActualLengthBeforeStraightening1").val().trim() == "") {
            firstFieldID = "txtActualLengthBeforeStraightening1";
        } else {
            firstFieldID = "txtActualLengthBeforeStraightening2";
        }
    }

    //================================Start Data========================================

    if (firstFieldID == 'txtDimAfterXCheck') {
        if ($("#txtDimAfterX1").val().trim() == "") {
            firstFieldID = "txtDimAfterX1";
        }
        //} else if ($("#txtDimAfterX2").val().trim() == "") {
        //    firstFieldID = "txtDimAfterX2";
        //} else if ($("#txtDimAfterX3").val().trim() == "") {
        //    firstFieldID = "txtDimAfterX3";
        //}

    }

    if (firstFieldID == 'txtActualLengthAftercheck') {
        if ($("#txtActualLength1AfterStraightening").val().trim() == "") {
            firstFieldID = "txtActualLength1AfterStraightening";
        } else {
            firstFieldID = "txtActualLength2AfterStraightening";
        }
    }

    //if (firstFieldID == 'txtDimAfterYcheck') {
    //    if ($("#txtDimAfterY1").val().trim() == "") {
    //        firstFieldID = "txtDimAfterY1";
    //    } else if ($("#txtDimAfterY2").val().trim() == "") {
    //        firstFieldID = "txtDimAfterY2";
    //    } else if ($("#txtDimAfterY3").val().trim() == "") {
    //        firstFieldID = "txtDimAfterY3";
    //    }

    //}

    if (firstFieldID == 'txtFinishedBarcheck') {
        if ($("#txtFinishedBarCount").val().trim() == "") {
            firstFieldID = "txtFinishedBarCount";
        } else {
            firstFieldID = "ddlUnitType";
        }
    }


    //================================End Data========================================


    if (message !== "") {
        showMessageDialog(message, firstFieldID, WARNING);
    }
    return returnVal;
}

$("#btnInitialize").click(function (event) {
    initialize();
});
function bindEvents() {

    setUnitType();
    setNextProcess();
    setDecision();
    setExteriorConditionKubunGet();
    setBendStatus();
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
           // resetHistoryDetailsGrid();
            var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_015").text().trim();
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

        $("#ddlTPTest").focus();

    }

    if (e.type === 'focusout' && operator && operatorStartModified == true) {
        if (enterPressedStartOpr) {

            enterPressedStartOpr = false; // Reset the flag
            return false;
        }
        $("#hdfStartOperatorCD").val(operatorNumber);
        getOperatorNameByCD($(this));
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
        //  $("#hdfEndOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
        enterPressedEndOpr = true;
        getOperatorNameByCD($(this));

        //if ($("#chkActualDim").attr('disabled')) {
        //    $("#txtActualDim1").focus();
        //} else {
        //    $("#chkActualDim").focus();
        //}
        $("#txtDimAfterX1").focus();
    }
    if (e.type === 'focusout' && operator && operatorEndModified == true) {
        if (enterPressedEndOpr) {

            enterPressedEndOpr = false; // Reset the flag
            return false;
        }
        $("#hdfEndOperatorCD").val(operatorNumber);
        getOperatorNameByCD($(this));
        //if ($("#chkActualDim").attr('disabled')) {
        //    $("#txtActualDim1").focus();
        //} else {
        //    $("#chkActualDim").focus();
        //}
        $("#txtDimAfterX1").focus();
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
    //  ClearPanelData();
    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();

    if (!IsMandatoryChuubanSearchFieldsFilled()) {
        return false;
    }

    ValidateChubban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
    getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
    getMachineDetails($("#hdfMachineCD").val(), $callFrom);

});

$("#btnSave").click(function (event) {

    debugger;
    var lengthValue = true;
    var finishedWeightValue = true;
    var actualDimValue = true;
    var focusTab = "";
    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();


    if ($("#txtActualDimDBeforeStraightening1").val().trim() == '' || $("#txtActualDimDBeforeStraightening2").val().trim() == '') {
        $("#txtActualDimBeforeCheck").addClass("Mandatory");
        $("#txtActualDimBeforeCheck").removeClass("MandatoryTextBox");

    } else {
        $("#txtActualDimBeforeCheck").addClass("MandatoryTextBox");
        $("#txtActualDimBeforeCheck").removeClass("Mandatory");

    }

    //if ($("#txtActualLengthBeforeStraightening1").val().trim() == '' || $("#txtActualLengthBeforeStraightening2").val().trim() == '') {

    //    $("#txtActualLengthBeforeCheck").addClass("Mandatory");
    //    $("#txtActualLengthBeforeCheck").removeClass("MandatoryTextBox");
    //} else {
    //    $("#txtActualLengthBeforeCheck").addClass("MandatoryTextBox");
    //    $("#txtActualLengthBeforeCheck").removeClass("Mandatory");
    //}


    if ($('#chkActualDim').prop("checked") == true && $("#txtDimAfterX1").val().trim() == '') {

        $("#txtDimAfterXCheck").addClass("Mandatory");
        $("#txtDimAfterXCheck").removeClass("MandatoryTextBox");
    } else {
        $("#txtDimAfterXCheck").addClass("MandatoryTextBox");
        $("#txtDimAfterXCheck").removeClass("Mandatory");
    }

    //if ($('#chkActualDim').prop("checked") == true && ($("#txtDimAfterY1").val().trim() == '' || $("#txtDimAfterY2").val().trim() == '' || $("#txtDimAfterY3").val().trim() == '')) {

    //    $("#txtDimAfterYcheck").addClass("Mandatory");
    //    $("#txtDimAfterYcheck").removeClass("MandatoryTextBox");
    //} else {
    //    $("#txtDimAfterYcheck").addClass("MandatoryTextBox");
    //    $("#txtDimAfterYcheck").removeClass("Mandatory");
    //}


    if ($("#chkActualLength").prop("checked") == true && ($("#txtActualLength1AfterStraightening").val().trim() == '' || $("#txtActualLength2AfterStraightening").val().trim() == '')) {

        $("#txtActualLengthAftercheck").addClass("Mandatory");
        $("#txtActualLengthAftercheck").removeClass("MandatoryTextBox");
    } else {
        $("#txtActualLengthAftercheck").addClass("MandatoryTextBox");
        $("#txtActualLengthAftercheck").removeClass("Mandatory");
    }

    if ($("#txtFinishedBarCount").val().trim() == '' || $("#ddlUnitType").val().trim() == '0') {

        $("#txtFinishedBarcheck").addClass("Mandatory");
        $("#txtFinishedBarcheck").removeClass("MandatoryTextBox");
    } else {
        $("#txtFinishedBarcheck").addClass("MandatoryTextBox");
        $("#txtFinishedBarcheck").removeClass("Mandatory");
    }

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    if (!checkDate($("#txtStartDate"))) {
        var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_010").text().trim();
        //  var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_010").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";// This message will be displayed on dialog box
        showMessageDialog(message, "txtStartDate", WARNING);
        return false;
    }
    if (isDuplicateThabaNo()) {
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

            message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_029").text());

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
        debugger;

        // Check all error conditions
        if (!IsEndDataValid()) {
            return false;
        }

        if (!checkDate($("#txtEndDate"))) {
            var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_011").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtEndDate", WARNING);
            return false;
        }

        if (!IsDateValid(false)) {
            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_032").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";
            var focusFieldID = "txtEndDate";
            showMessageDialog(message, focusFieldID, WARNING);
            return false;
        }

        if (isDuplicateThabaNo()) {
            return false;
        }

        // check Actual dim & length out of tolerance.

        if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK) {
            var dimValues = {};
            dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
            dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
            dimValues["DIM1"] = $("#txtDimAfterX1").val() ? $("#txtDimAfterX1").val() : null;
            dimValues["DIM2"] = $("#txtDimAfterX2").val() ? $("#txtDimAfterX2").val() : null;
            dimValues["DIM3"] = $("#txtDimAfterX3").val() ? $("#txtDimAfterX3").val() : null;
            dimValues["DIM4"] = $("#txtDimAfterY1").val() ? $("#txtDimAfterY1").val() : null;
            dimValues["DIM5"] = $("#txtDimAfterY2").val() ? $("#txtDimAfterY2").val() : null;
            dimValues["DIM6"] = $("#txtDimAfterY3").val() ? $("#txtDimAfterY3").val() : null;

            var result = IsActualDimentionValid(dimValues);
            if (result.IsSuccess == false) {
                var focusText = "";
                if (result.InvalidDim == $("#txtDimAfterX1").val()) {
                    focusText = "txtDimAfterX1";
                }
                if (result.InvalidDim == $("#txtDimAfterX2").val() && focusText == "") {
                    focusText = "txtDimAfterX2";
                }
                if (result.InvalidDim == $("#txtDimAfterX3").val() && focusText == "") {
                    focusText = "txtDimAfterX3";
                }
                if (result.InvalidDim == $("#txtDimAfterY1").val() && focusText == "") {
                    focusText = "txtDimAfterY1";
                }
                if (result.InvalidDim == $("#txtDimAfterY2").val() && focusText == "") {
                    focusText = "txtDimAfterY2";
                }
                if (result.InvalidDim == $("#txtDimAfterY3").val() && focusText == "") {
                    focusText = "txtDimAfterY3";
                }
                var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_026").text().trim(); //ActDimOutOfTolerance
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
                var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_027").text().trim(); // LengthOutOfTolerance
                showMessageDialog(message, focusText, WARNING);
                return false;
            }


        }



        //if (!isFinishedQuantityValid()) {

        //    var message = $("#lblConfirmationMessageC_STRAIGHTENING2_ENTRY_002").text().trim(); // This message will be displayed on dialog box
        //    showMessageDialog(message, "txtFinishedQuantity", WARNING);
        //    return false;

        //}

        //if (!isActualDimentionValid()) {

        //    focusTab = "txtDimAfterX1";
        //    //var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_012").text().trim(); // This message will be displayed on dialog box
        //    //showMessageDialog(message, "txtFinishedQuantity", WARNING);
        //    actualDimValue = false;


        //}

        //if (!isActualLengthValid()) {

        //    if (focusTab == "")
        //    {
        //        focusTab = "txtActualLength1AfterStraightening";
        //    }
        //        var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_012").text().trim(); // This message will be displayed on dialog box
        //        showMessageDialog(message, "txtFinishedQuantity", WARNING);
        //        lengthValue = false;

        //}



        //debugger;
        //var message = "";
        //if (lengthValue == false || actualDimValue == false) {

        //    if (lengthValue == false || actualDimValue == false) {
        //        message = $.trim($("#lblConfirmationMessageC_STRAIGHTENING_Lenght_026").text());
        //    }


        //    if (finishedWeightValue == false) {
        //        if (message == "") {
        //            message = $.trim($("#lblConfirmationMessageC_STRAIGHTENING2_ENTRY_002").text());

        //        } else {
        //            message = message + "<br>" + $.trim($("#lblConfirmationMessageC_STRAIGHTENING2_ENTRY_002").text());
        //        }

        //    }



        // If +-2% checkbox is not checked and Finished Qty is not in 2% Range, show confirmation message
        var weightValues = {};
        weightValues["PRODUCT_WEIGHT"] = $("#txtQuantityT1").val() ? $("#txtQuantityT1").val() : null;
        weightValues["INPUT_WEIGHT"] = $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null;
        weightValues["PRODUCT_SHAPE"] = $("#txtProdShapeT1").val() ? $("#txtProdShapeT1").val() : null;
        weightValues["PRODUCT_DIM"] = $("#txtProdDimT1").val() ? $("#txtProdDimT1").val() : null;
        weightValues["NO_OF_UNITS"] = $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null;
        weightValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;

        if ($("#chkFinishedQuantity").prop("checked") == true) {
            debugger;
            if ([
                "PRODUCT_SHAPE",
                "PRODUCT_DIM",
                "PRODUCT_LENGTH"
            ].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== "")) {

                var value = $("#txtLengthT1").val();
                // Check - Length only allow numeric &  Unit Type - Not allow BN 

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
        debugger;
        if (!format.test($("#txtLengthT1").val()) && $("#ddlUnitType").val() != 2) {
            getResponse = IsFinishedQuantityValid(weightValues);
        } else {
            getResponse["IsSuccess"] = true;
        }

        if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true) {
          
            var message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_028").text().trim(); // FinishedQtyNotIn2PercentRange
            showMessageDialog(message, "txtFinishedQuantity", WARNING);

        } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false) {
            var message = "";

            message = $.trim($("#lblErrorMessageE_STRAIGHTENING_ENTRY_034").text());  //WeightIsInvalid

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
                        //return true;
                      
                       // getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);

                        if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                            updateStraighteningDetails($callFrom);


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
                        //  $("#" + focusTab).focus();
                        // return false;

                    }


                },
                centerVertical: true
            });

        } else {
           
            if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
                updateStraighteningDetails($callFrom);


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

//$("#txtNGQuantity1").keyup(function (event) {
//    OKQtyRegionClassChange();
//});
//$("#txtNGQuantity2").keyup(function (event) {
//    OKQtyRegionClassChange();
//});
//$("#txtNGQuantity3").keyup(function (event) {
//    OKQtyRegionClassChange();
//});



//
function OKQtyRegionClassChange() {

    var txtNGWeight1 = $("#txtNGQuantity1").val();
    var ddlDefects1 = document.getElementById("ddlDefects1");
    if (txtNGWeight1 > 0) {
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlNGReason1").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason1").removeClass("Mandatory");
        $("#ddlNGReason1 option:selected").prop("selected", false);
        $("#ddlNGReason1").attr("disabled", "disabled");
    }
    var txtNGWeight2 = $("#txtNGQuantity2").val();
    var ddlDefects2 = document.getElementById("ddlNGReason2");
    if (txtNGWeight2 > 0) {
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'setChuubanDimLenToleranceMessages DropdownClass Mandatory', 0);
        $("#ddlNGReason2").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason2").removeClass("Mandatory");
        $("#ddlNGReason2 option:selected").prop("selected", false);
        $("#ddlNGReason2").attr("disabled", "disabled");
    }
    var txtNGWeight3 = $("#txtNGQuantity3").val();
    var ddlDefects3 = document.getElementById("ddlNGReason3");
    if (txtNGWeight3 > 0) {
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlNGReason3").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason3").removeClass("Mandatory");
        $("#ddlNGReason3 option:selected").prop("selected", false);
        $("#ddlNGReason3").attr("disabled", "disabled");
    }
    var add = 0;
    $(".amt").each(function () {
        add += Number($(this).val());
        //$("#txtNgWeight").val(add);

        if (add == 0) {
            $("#txtNgWeight").val("");
        }
        else {
            $("#txtNgWeight").val(add);
        }
    });
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
    debugger;
    chk = new Set();
    var duplicateThaba = thabaList.filter(n => chk.size === chk.add(n).size);

    if (duplicateThaba.length > 0) {
        if (duplicateThaba.length > 1) {
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_021").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_021").text().trim().replace("NNNN", duplicateThaba[0]);;
        }

        showMessageDialog(message, firstFieldID, WARNING);
        return true;
    }

    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {

            t1 = inputThaba1;
        }
    }
    if (inputThaba2 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba2]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {

            t2 = inputThaba2;
        }
    }
    if (inputThaba3 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba3]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {
            t3 = inputThaba3;
        }
    }
    if (inputThaba4 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba4]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {

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
        message = $("#lblErrorMessageE_Duplicate_thaba_WithOk").text().trim().replace("NNN", thabamessage.replace(/(.+), $/, '$1'));
        showMessageDialog(message, firstFieldID, WARNING);
        return true;
    } else {

        return false;
    }


    return false;
}

function setDecimalFormats() {
    $("#txtActualDimDBeforeStraightening1, #txtActualDimDBeforeStraightening2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtActualLengthBeforeStraightening1, #txtActualLengthBeforeStraightening2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
    });

    $("#txtDimAfterX1, #txtDimAfterX2, #txtDimAfterX3, #txtDimAfterY1,#txtDimAfterY2, #txtDimAfterY3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtActualLength1AfterStraightening, #txtActualLength2AfterStraightening").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
    });
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
}

$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // window.moveTo(0, 0);
    window.resize(1920, 1080);
    hideToleranceErrorLabels();
    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    setGlobalVariables();
    bindEvents();
    bindTPTestList();
    setDecimalFormats();
    hideToleranceErrorLabels();
    disableStartDataPanel();
    disableEndDataPanel();
    disableSaveButton();
    resetStraighteningMachineDetailsGrid();

    DisablemandatoryNG();

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

//function IsDateValid(boolVal) {
//    debugger;
//    var returnval = true;
//    var currentDate = new Date();

//    var minDate = new Date();
//    var maxDate = new Date();
//    minDate.setDate(currentDate.getDate() - parseInt(commonDateRange));
//    maxDate.setDate(currentDate.getDate() + parseInt(commonDateRange));

//    var minimumDate = minDate.getFullYear() + "/" + String('00' + minDate.getMonth() + 1).slice(-2) + "/" + String('00' + minDate.getDate()).slice(-2) + " " + String('00' + minDate.getHours()).slice(-2) + ":" + String('00' + minDate.getMinutes()).slice(-2);
//    var maximumDate = maxDate.getFullYear() + "/" + String('00' + maxDate.getMonth() + 1).slice(-2) + "/" + String('00' + maxDate.getDate()).slice(-2) + " " + String('00' + maxDate.getHours()).slice(-2) + ":" + String('00' + maxDate.getMinutes()).slice(-2);
//    if (boolVal === true) {
//        if (Date.parse($("#txtStartDate").val()) <= Date.parse(minimumDate) || Date.parse($("#txtStartDate").val()) >= Date.parse(maximumDate)) {
//            returnval = false;
//            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_031").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";
//            var focusFieldID = "txtStartDate";
//            showMessageDialog(message, focusFieldID, WARNING);

//        }

//    }

//    if (boolVal === false) {

//        if (Date.parse($("#txtEndDate").val()) < Date.parse(minimumDate) || Date.parse($("#txtEndDate").val()) > Date.parse(maximumDate)) {
//            returnval = false;
//            var message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_032").text().trim() + "(" + $("#lblErrorMessageE_STRAIGHTENING_ENTRY_033").text().trim() + commonDateRange + ")";

//            var focusFieldID = "txtEndDate";
//            showMessageDialog(message, focusFieldID, WARNING);

//        }
//    }
//    return returnval;
//}

function IsDateValid(boolVal) {
    debugger;
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
            var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });




    return returnval;
}

function resetStraighteningMachineDetailsGrid() {
    clearTable($("#grdvStraighteningMachine"), "ItemGrdvHikinukiMachine", "");
    $grdvMessage = $("#grdvStraighteningMachine");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr1 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
        $("<td rowspan='2' align='center' valign='top'  style='padding-left:4px;padding-right:4px;'>").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td rowspan='2' align='center' valign='top'>").text(""),
        $("<td rowspan='2' align='center' valign='top'> ").text(""),
        $("<td align='center' valign='top'> ").text(""),
        $("<td colspan='2' align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td  align='center' valign='top'>").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
        /*$("<td rowspan='2'align='center' valign='top'>").text(""),*/
    );
    var $tr2 = $('<tr class="GridNormalRow" style="height:18px;border:1px solid #FF9900;">').append(
       /* $("<td  align='center' valign='top'>").text(""),*/
        $("<td  align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
    );
    var $tr3 = $('<tr class="GridNormalRow" style="height:10px;border:1px solid #FF9900;">').append(
        $("<td align='center' valign='top'>").text(""),
    );
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
            url: APIURL + "/Straightening2/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});


$('#chkActualDim').click(function () {
    debugger;
    if ($("#chkActualDim").prop("checked") == true) {
        enableDimAfterMandatory();
       
    } else {
       
        disableDimAfterMandatory();
    }
});

function enableDimAfterMandatory() {
    $("#txtDimAfterX1").addClass("MandatoryTextBox");
    //$("#txtDimAfterX2").addClass("MandatoryTextBox");
    //$("#txtDimAfterX3").addClass("MandatoryTextBox");

    //$("#txtDimAfterY1").addClass("MandatoryTextBox");
    //$("#txtDimAfterY2").addClass("MandatoryTextBox");
    //$("#txtDimAfterY3").addClass("MandatoryTextBox");
}
function disableDimAfterMandatory() {

    $("#txtDimAfterX1").removeClass("MandatoryTextBox");
    //$("#txtDimAfterX2").removeClass("MandatoryTextBox");
    //$("#txtDimAfterX3").removeClass("MandatoryTextBox");


    //$("#txtDimAfterY1").removeClass("MandatoryTextBox");
    //$("#txtDimAfterY2").removeClass("MandatoryTextBox");
    //$("#txtDimAfterY3").removeClass("MandatoryTextBox");

}

$("#ddlNextProcess").focusout(function (event) {
    $("#btnSave").focus();
});

$("#txtThaba4").focusout(function (event) {
    if ($("#ddlExternalSurfaceCondition").attr(DISABLED)) {
        $("#btnSave").focus();
    } else {
        $("#txtNGQuantity1").focus();
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

