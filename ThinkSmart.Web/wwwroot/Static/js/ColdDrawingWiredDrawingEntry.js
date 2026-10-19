
$("#24HrsMachineData").css({ 'height': '850px' });
var combinedDecisionOK = "";
var $btnChuubanSearch;
var $btnSave;
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";
var dbOperation;
var maxRegisteredThabaNo = 0;
var $hdfSYSDATE;
var $hdfCultureLoaded;
var $hdfCultureEN;
var $hdfCultureJP;
var DrawingCD;
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
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var isOperatorValid = true;
var txtStartOperatorfocus = false;
var operatorStartModified = false;
var operatorEndModified = false;


userName = ""; // assigned at Document.ready

// Following decision legends are to show decision colors on Top Thaba Grid.
//var decisionLegends = {
//    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
//    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
//    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
//    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>"
//}

var machineGridDataTr1 = " <tr id='tr1ItemGrdvColdDrawingWireDrawingMachine'> "
    + " <td class='td tdH2' rowspan='2' style='background-color:COLOR; color:#E2E2E2;'> "
    + " <span class='txtColor' style='background-color:COLOR; color:#E2E2E2; height:35px; width:6px; display: inline-block;'></span>"
    + " </td>"

    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align:center'>"
    + " <span class='txtGridChuban labelClass' >valtxtGridChuban</span>"
    + " </td>"
    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align: left'>"
    + " <span class='txtGridCustomerName labelClass' > valtxtGridCustomerName</span>"
    + " </td>"

    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align: left'>"
    + " <span class='txtProdNameCust labelClass' >valtxtProdNameCust</span></td>"

    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align:right'>"
    + " <span class='txtGridProdDimD labelClass' >valtxtGrid_PROD_DIM_D</span></td>"

    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align:right'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_DIM_TOL</span></td>"

    + " <td class='td tdH2 padding24Machine' style='text-align:right'>"
    + " <span class='txtMaterialDimD labelClass'>valtxtMaterialDimD</span></td>"


    + " <td class='td tdH2 padding24Machine' style='text-align: left'>"
    + " <span class='txtMaterialDimD labelClass'>valtxtGrid_Charge</span></td>"

    + " <td class='td tdH2 padding24Machine' style='text-align:right'>"
    + " <span class='txtMaterialDimD labelClass'>valtxtGrid_Die_Dim</span></td>"


    + " <td class='td tdH2 padding24Machine' colspan='2' style='text-align: left'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_Furnace_No</span></td>"

    + " <td class='td tdH2 padding24Machine' style='text-align:left'>"
    + " <span class='txtMaterialDimD labelClass'>valtxt_Act_Dimentional</span></td>"

    + " <td class='td tdH2 padding24Machine' style='text-align:right'>"
    + " <span class='txtMaterialDimD labelClass'>valtxtGrid_Finished_Weight</span></td>"

    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxt_TABA_NO_1</span><span class='txtMaterialDimD labelClass' >valtxt_TABA_NO_2</span></td>"

    + " <td class='td tdH2 padding24Machine' rowspan='2' style='text-align: left'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_Remarks</span></td>"
    + "  </tr>";

var machineGridDataTr2 = "<tr id='tr2ItemGrdvColdDrawingWireDrawingMachine'>"
    //+ "  <td class='td tdH2 padding24Machine'> "
    //+ "<span class='txtGridSpecification labelClass' >valtxtGridSpecification</span> "
    //+ "</td> "
    + "  <td class='td tdH2 padding24Machine' style='text-align: left' colspan='2'> "
    + "<span class='txtGridSpecification labelClass' >valtxtGridRollingNo</span> "
    + "</td> "
    + "  <td class='td tdH2 padding24Machine' style='text-align: left'> "
    + "<span class='txtGridSpecification labelClass' >valtxtGridDieNo</span> "
    + "</td> "
    + " <td class='td tdH2 padding24Machine'colspan='2' style='text-align: left'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_W_Shape</span>"
    + " </td > "
    + " <td class='td tdH2 padding24Machine' style='text-align: left'>"
    + " <span class='txtMaterialDimD labelClass' >valtxt_Ext_condition</span></td>"

    + " <td class='td tdH2 padding24Machine' style='text-align: left'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_MAGNETIC_INSPECTIONS</span></td>"
    + " </tr> ";



function initialize() {
    combinedDecisionOK = "";
    enableSearchOrderNoPanel();
    registeredThabaNumbersAndDecision = {};

    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    //enableSaveButton();
    enableSearchPanel();
    $(".FocusDefault").focus();
    disableStartDataPanel();
    disableEndDataPanel();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlProcessInfo")); // Tab 3
    clearPanel($("#pnlTestSpecifications")); // Tab 3
    clearThabaDetailsGrid();
    clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");     // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 2
    clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 3

    $("#txtMachineType").val("");
    $("#txtNifudaNo1").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $("#txtStartDate").val("");

    $("#txtStartOperator").val("");
    $("#txtNoOfCoils").val("");
    $("#txtEndDate").val("");

    $("#txtEndOperator").val("");
    $("#txtFinishedBarCount").val("");
    $("#txtFinishedQuantity").val("");
    $("#txtThaba1").val("");
    $("#txtThaba2").val("");
    $("#txtThaba3").val("");
    $("#txtThaba4").val("");
    $("#txtBundleNo").val("");
    $("#txtNgWeight").val("");
    $("#txtNGWeight1").val("");
    $("#txtNGWeight2").val("");
    $("#txtNGWeight3").val("");
    $("#txtRemarks").val("");

    //hideToleranceErrorLabels();
    resetGlobalVariables();
    //resetMessageDetailsGrid();
    resetColdDrawingWireHistoryGrid();
    resetMessageDetailsGrid();
    resetProdDataGrid();
}

function resetColdDrawingWireHistoryGrid() {

    clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
    $grdvMessage = $("#grdvColdDrawingWireDrawingMachine");
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

        $("<td align='center' valign='top'> ").text(""),
        $("<td align='center' valign='top'> ").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td colspan='2' align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td align='center' valign='top'>").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
        $("<td rowspan='2'align='center' valign='top'>").text(""),
    );
    var $tr2 = $('<tr class="GridNormalRow style="height:15px;">').append(
        $("<td  colspan='2' align='center' valign='top'>").text(""),
        $("<td   align='center' valign='top'>").text(""),
        $("<td  colspan='2'align='center' valign='top'>").text(""),
        $("<td  align='center' valign='top'>").text(""),
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

function ClearData() {
    combinedDecisionOK = "";
    enableSearchOrderNoPanel();
    $("#btnSave").attr("disabled", "disabled");
    //enableSaveButton();
    enableSearchPanel();
    $(".FocusDefault").focus();
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
    $("#txtThaba2").val("");
    $("#txtThaba3").val("");
    $("#txtThaba4").val("");
    $("#txtBundleNo").val("");
    $("#txtNgWeight").val("");
    $("#txtNGWeight1").val("");
    $("#txtNGWeight2").val("");
    $("#txtNGWeight3").val("");
    $("#txtRemarks").val("");

    //hideToleranceErrorLabels();
    resetGlobalVariables();
    //resetColdDrawingWireHistoryGrid();
    resetMessageDetailsGrid();
    resetProdDataGrid();
}

function Mask() {
}
$("#chkActualDim").click(function () {

    if ($("#chkActualDim").is(":checked")) {
        $("#txtActualDieDimStart").addClass("Mandatory1");
        $("#txtActualDieDimEnd").addClass("Mandatory1");


    } else {

        $("#txtActualDieDimStart").removeClass("Mandatory1");
        $("#txtActualDieDimEnd").removeClass("Mandatory1");
    }

});



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
    // Fllowing is to clear readonly hidden fields
    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
    });

    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}

//function resetMessageDetailsGrid() {
//    $grdvMessage = $("#grdvMessage");
//    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

//    // Add one blank data row
//    var $tr = $('<tr class="GridNormalRow">').append(
//        $('<td class="GridAlignmentDateTime">').text(""),
//        $('<td class="GridAlignmentMachineName">').text(""),
//        $('<td class="GridAlignmentProcessName">').text(""),
//        $('<td class="GridAlignmentOperator">').text(""),
//        $('<td class="GridAlignmentRemarks GridRemarks200PX">').text("")
//    );
//    $grdvMessage.append($tr);
//}

function setMessageDetailsGrid(messageDetailsList) {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add data rows
    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
       
        if ($("#hdfCultureName").val() == "en") // > -1 means substring is present
        {
            processName = jsonObject.procesS_NAME_EN;
        }

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


function clearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
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
        "<td class='ProdPointDetails' style='font-weight: normal;text-align:left;'>" + $_prodPint + "</td>" +
        "</tr>");
    $grdvProdPointData.append($tr);
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
        url: APIURL + "/ColdDrawingWIREDRAWING/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;


            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });


            // $("#txtStartTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });

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
        url: APIURL + "/ColdDrawingWIREDRAWING/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;
            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            //  $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            //$("#txtEndTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
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

function getMachineNamebyCD($target) {

    var ajaxTime = new Date().getTime();
    var machineNumber = $target.val().trim();
    var machineval = machineNumber.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineCD = machineNumber.substring(3, 7);

 
    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_Cold_Drawing_034").text().trim().replace("*****", $("#tdLblMachineType").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }

    if (machineCD == "") {
        var message = $("#lblErrorMessageE_Cold_Drawing_ScanMachineRequired").text().trim();

        showMessageDialog(message, "txtMachineType", WARNING);
        
        $('#txtMachineType').val("");
        return false;
    }
    var jsonData = "{machineCD: '" + machineCD + "', USER_NAME : '" + userName + "'}"


    if (machineCD.length > 4) {
        var message = $("#lblErrorMessageE_Cold_Drawing_002").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }


    var settings = {
        async: false,
        crossDomain: true,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetMachineNamebyCD?machineCD=" + parseInt(machineCD),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var machineName = response.message;

            if (machineName.key == null || machineName.key == "") {
                var message = $("#lblErrorMessageE_Cold_Drawing_035").text().trim().replace("*****", $("#tdLblMachineType").text());
                showMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }else if (machineName.key == null || machineName.key == "") {
                clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
                resetColdDrawingWireHistoryGrid();
                var message = $("#lblErrorMessageE_Cold_Drawing_002").text().trim();
                showMessageDialog(message, $target.attr("id"), WARNING);
                $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the machineName barcode again
            }
            else if (machineName.value == "inactive") {
                var message = $("#lblErrorMessageE_Cold_Drawing_003").text().trim();
                clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
                resetColdDrawingWireHistoryGrid();
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
            url: APIURL + "/ColdDrawingWIREDRAWING/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
function getMachineDetails(machineCd, $callFrom) {

    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }

    var ajaxTime = new Date().getTime();

    var jsonData = "{machineCd: '" + machineCd + "', USER_NAME: '" + userName + "'}"

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetMachineDetails?machineCD=" + machineCd,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
                clearTable($("#grdvColdDrawingWireDrawingMachine"), "", "GridNormalRow");// Tab 2
                Set24HrMachineData(jsonResult);
            } else {
                clearTable($("#grdvColdDrawingWireDrawingMachine"), "grdvColdDrawingWireDrawingMachine", "");
                resetColdDrawingWireHistoryGrid();
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

function Set24HrMachineData(jsonResult) {

    clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");

    $.each(jsonResult, function (index, jsonObject) {

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
            }
            else if (jsonObject.dW_DECISION == 5) {
                firstRow = firstRow.replace("COLOR", "darkred");  //firstRow = $("●").css('color', 'Yellow');
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



        /*   if (jsonObject.PP_PROD_NAME_CUST != null)
               firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.PP_PROD_NAME_CUST);
           else
               firstRow = firstRow.replace("valtxtProdNameCust", "");*/



        if (jsonObject.pP_PROD_NAME_CUST != null)
            firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
        else
            firstRow = firstRow.replace("valtxtProdNameCust", "");


        if (jsonObject.pP_PROD_DIM_D_STRING != null)
            firstRow = firstRow.replace("valtxtGrid_PROD_DIM_D", jsonObject.pP_PROD_DIM_D_STRING);
        else
            firstRow = firstRow.replace("valtxtGrid_PROD_DIM_D", "");


        if (jsonObject.pP_MATL_DIM_D_STRING != null)
            firstRow = firstRow.replace("valtxtMaterialDimD", jsonObject.pP_MATL_DIM_D_STRING);
        else
            firstRow = firstRow.replace("valtxtMaterialDimD", "");

        if (jsonObject.pP_DIM_TOLERANCE != null)
            firstRow = firstRow.replace("valtxtGrid_DIM_TOL", jsonObject.pP_DIM_TOLERANCE);
        else
            firstRow = firstRow.replace("valtxtGrid_DIM_TOL", "");

        if (jsonObject.pP_CHARGE != null)
            firstRow = firstRow.replace("valtxtGrid_Charge", jsonObject.pP_CHARGE);
        else
            firstRow = firstRow.replace("valtxtGrid_Charge", "");

        if (jsonObject.dW_DIE_SHAPE != null)
            if (jsonObject.pP_MATL_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtGrid_Die_Dim", jsonObject.pP_MATL_DIM_D_SHAPE + "" + jsonObject.dW_DIE_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGrid_Die_Dim", "");

        if (jsonObject.dW_FURNACE_NO != null)
            firstRow = firstRow.replace("valtxtGrid_Furnace_No", jsonObject.dW_FURNACE_NO);
        else
            firstRow = firstRow.replace("valtxtGrid_Furnace_No", "");

        
        if (jsonObject.dW_ACT_DIM_START_STRING != null && jsonObject.dW_ACT_DIM_START_STRING != 0) 
            firstRow = firstRow.replace("valtxt_Act_Dimentional", jsonObject.dW_ACT_DIM_START_STRING + "～" + jsonObject.dW_ACT_DIM_END_STRING);
        else
            firstRow = firstRow.replace("valtxt_Act_Dimentional", "");



        if (jsonObject.dW_DW_EXT_TEXTURE_STATUS != null)
            firstRow = firstRow.replace("valtxt_Ext_condition", jsonObject.dW_DW_EXT_TEXTURE_STATUS);
        else
            firstRow = firstRow.replace("valtxt_Ext_condition", "");

        if (jsonObject.dW_FINISHED_QTY != null && jsonObject.dW_FINISHED_QTY != 0)
            firstRow = firstRow.replace("valtxtGrid_Finished_Weight", jsonObject.dW_FINISHED_QTY);
        else
            firstRow = firstRow.replace("valtxtGrid_Finished_Weight", "");

        if (jsonObject.dW_DW_TABA_NO_1_STRING != null)
            firstRow = firstRow.replace("valtxt_TABA_NO_1", jsonObject.dW_DW_TABA_NO_1_STRING);
        else
            firstRow = firstRow.replace("valtxt_TABA_NO_1", "");

        if (jsonObject.dW_DW_TABA_NO_2_STRING != null)
            firstRow = firstRow.replace("valtxt_TABA_NO_2", jsonObject.dW_DW_TABA_NO_2_STRING);
        else
            firstRow = firstRow.replace("valtxt_TABA_NO_2", "");




        if (jsonObject.dW_REMARKS != null)
            firstRow = firstRow.replace("valtxtGrid_Remarks", jsonObject.dW_REMARKS);
        else
            firstRow = firstRow.replace("valtxtGrid_Remarks", "");







        $("#grdvColdDrawingWireDrawingMachine").append($(firstRow));



        // tr2    (Second row)

        var secondtRow = machineGridDataTr2;

        if (jsonObject.pROD_SPEC_NO != null)
            secondtRow = secondtRow.replace("valtxtGridSpecification", jsonObject.pROD_SPEC_NO);
        else
            secondtRow = secondtRow.replace("valtxtGridSpecification", "");

        if (jsonObject.dW_ROLLING_NUMBER != null)
            secondtRow = secondtRow.replace("valtxtGridRollingNo", jsonObject.dW_ROLLING_NUMBER);
        else
            secondtRow = secondtRow.replace("valtxtGridRollingNo", "");

        if (jsonObject.dW_DW_EXT_TEXTURE_STATUS != null)
            secondtRow = secondtRow.replace("valtxt_Ext_condition", jsonObject.dW_DW_EXT_TEXTURE_STATUS);
        else
            secondtRow = secondtRow.replace("valtxt_Ext_condition", "");

        if (jsonObject.dW_DIE_NO != null)
            secondtRow = secondtRow.replace("valtxtGridDieNo", jsonObject.dW_DIE_NO);
        else
            secondtRow = secondtRow.replace("valtxtGridDieNo", "");

        if (jsonObject.dW_WINDING_SHAPE1 != null)

            secondtRow = secondtRow.replace("valtxtGrid_W_Shape", jsonObject.dW_WINDING_SHAPE1);

        else
            secondtRow = secondtRow.replace("valtxtGrid_W_Shape", "");

        if (jsonObject.dW_DW_MAGNETIC_INSPECTIONS != null)
            secondtRow = secondtRow.replace("valtxtGrid_MAGNETIC_INSPECTIONS", jsonObject.dW_DW_MAGNETIC_INSPECTIONS);
        else
            secondtRow = secondtRow.replace("valtxtGrid_MAGNETIC_INSPECTIONS", "");




        //if (jsonObject.DW_ACT_DIM4 != null)
        //    secondtRow = secondtRow.replace("valtxtActAllocDim4", jsonObject.DW_ACT_DIM4);
        //else
        //    secondtRow = secondtRow.replace("valtxtActAllocDim4", "");

        //if (jsonObject.DW_ACT_DIM5 != null)
        //    secondtRow = secondtRow.replace("valtxtActAllocDim5", jsonObject.DW_ACT_DIM5);
        //else
        //    secondtRow = secondtRow.replace("valtxtActAllocDim5", "");

        //if (jsonObject.DW_ACT_DIM6 != null)
        //    secondtRow = secondtRow.replace("valtxtActAllocDim6", jsonObject.DW_ACT_DIM6);
        //else
        //    secondtRow = secondtRow.replace("valtxtActAllocDim6", "");

        //if (jsonObject.DW_REMARKS != null)
        //    secondtRow = secondtRow.replace("valtxtGridMsgContent", jsonObject.DW_REMARKS);
        //else
        //    secondtRow = secondtRow.replace("valtxtGridMsgContent", "");

        //if (jsonObject.DW_MAGNETISM_DECODE != null)
        //    secondtRow = secondtRow.replace("valtxtGridMagnetism", jsonObject.DW_MAGNETISM_DECODE);
        //else
        //    secondtRow = secondtRow.replace("valtxtGridMagnetism", "");


        //if (jsonObject.DW_SHIKII_VALUE != null)
        //    secondtRow = secondtRow.replace("valtxtGridShikiiValue", jsonObject.DW_SHIKII_VALUE);
        //else
        //    secondtRow = secondtRow.replace("valtxtGridShikiiValue", "");

        //if (jsonObject.sT_NG_BARS1 != null)
        //    secondtRow = secondtRow.replace("valtxtGridEcNgBarTotal", jsonObject.sT_NG_BARS1);
        //else
        //    secondtRow = secondtRow.replace("valtxtGridEcNgBarTotal", "");

        $("#grdvColdDrawingWireDrawingMachine").append($(secondtRow));
        //}
    });

}


function getOperatorNameByCD($target) {

    var ajaxTime = new Date().getTime();


    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);

    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_Cold_Drawing_034").text().trim().replace("*****", $("#lblStartOperator").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_Cold_Drawing_006").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageE_Cold_Drawing_006").text().trim();
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
            url: APIURL + "/ColdDrawingWIREDRAWING/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;

                var jsonresult = response.message;

                if (jsonresult == null || jsonresult == "") {
                    var message = $("#lblErrorMessageE_Cold_Drawing_035").text().trim().replace("*****", $("#lblStartOperator").text());
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
                    } else {
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
function disableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "none");
}
function enableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
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

        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
            clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
            resetColdDrawingWireHistoryGrid();
            var message = $("#lblErrorMessageE_Cold_Drawing_ScanMachineRequired").text().trim();
            showMessageDialog(message, "txtMachineType", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
            enterPressedMachine = true;
            getMachineNamebyCD($(this));
            BindDropDwon();

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
        BindDropDwon();
        $("#txtChuuban2").focus();
    }
    enterPressedMachine = false; // Reset the flag
});

//$('#txtMachineType').keydown(function (e) {
//    var machine = e.target.value ? e.target.value : null;
//    var keycode = (e.keyCode ? e.keyCode : e.which);
    
//    if (e.which == 13 || e.which == 9 && machine) {
//        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
//            clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
//            resetColdDrawingWireHistoryGrid();
//            var message = $("#lblErrorMessageE_Cold_Drawing_ScanMachineRequired").text().trim();
//            showMessageDialog(message, "txtMachineType", WARNING);

//        } else {
//            var machineCD = $(this).val().trim();
//            var machineName = machineCD.substring(0, 3);
//            var machineNumber = machineCD.substring(3, 7);
//            $("#hdfMachineCD").val(machineNumber);
//            getMachineNamebyCD($(this));
//            BindDropDwon();
           
//        }
//    }
//});

function BindDropDwon() {
    setNextProcess();
    setExteriorConditionKubunGet();
    setMagneticDust();
    setDiceInspEnd();
    setDiceInspStart();
    setwindingShape();
    setDieDiaShape();
    setDecision();
}

function bindEvents() {

    $("#btnInitialize").click(function (event) {
        initialize();
        $("#btnSave").attr("disabled", "disabled");
    });


    //$("#txtStartOperator").keydown(function (e) {

    //    var operator = e.target.value ? e.target.value : null;
    //    var keycode = (e.keyCode ? e.keyCode : e.which);

    //    if (e.which == 13 || e.which == 9 && operator) {

    //        if ($("#txtStartOperator").val() == null || $("#txtStartOperator").val() == "") {

    //            var message = $("#lblErrorMessageE_Cold_Drawing_006").text().trim();
    //            showMessageDialog(message, "txtStartOperator", WARNING);

    //        } else {
    //            var operatorCD = $(this).val().trim();
    //            var operatorName = operatorCD.substring(0, 3);
    //            var operatorNumber = operatorCD.substring(3, 9);
    //            $("#hdfStartOperatorCD").val(operatorNumber);

    //            //  $("#hdfStartOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
    //            getOperatorNameByCD($(this));
    //            $("#txtRollingnumber").focus();
    //        }
    //    }
    //});

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

            $("#txtRollingnumber").focus();

        }

        if (e.type === 'focusout' && operator && operatorStartModified == true) {
            if (enterPressedStartOpr) {

                enterPressedStartOpr = false; // Reset the flag
                return false;
            }
            $("#hdfStartOperatorCD").val(operatorNumber);
            getOperatorNameByCD($(this));
            $("#txtRollingnumber").focus();
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
        var operatorCD = $(this).val().trim();
        var operatorName = operatorCD.substring(0, 3);
        var operatorNumber = operatorCD.substring(3, 9);

        if (e.which == 13 && operator && operatorEndModified == true) {
            $("#hdfEndOperatorCD").val(operatorNumber);
            //  $("#hdfEndOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
            enterPressedEndOpr = true;
            getOperatorNameByCD($(this));

            if ($("#chkActualDim").attr('disabled')) {
                $("#txtActualDieDimStart").focus();
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

        ValidateChubban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        //getColdDrawingWireDrawingDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);

        //  getMachineDetails($("#hdfMachineCD").val(), $callFrom);
        // getDrawingStartdetails($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfldProcessPlanCd").val(), $callFrom);

    });

    //ValidateChubban
    function ValidateChubban(chuuban, gyouban, $callFrom) {
        $callFrom.data('requestRunning', true);
        var ajaxTime = new Date().getTime();
        var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/ColdDrawingWIREDRAWING/ValidateChubban",
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
                    var errorMessage = $("#lblErrorMessageE_Cold_Drawing_004").text();
                    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                    return false;
                }

                else if (result.responce == 2) {
                    disableStartDataPanel();
                    $("#btnSave").attr("disabled", "disabled");
                 //   ClearPanelData();
                    // Chuuban not found in system.
                    var errorMessage = $("#lblErrorMessageE_Cold_Drawing_chuubannotfound").text();
                    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                    return false;
                }
                else if (result.responce == 3) {
                    // OrNo Not Started FromHandOver
                    var message = $("#lblErrorMessageE_Cold_Drawing_030").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                    showMessageDialog(message, "", WARNING);
                    // ClearData();
                    return false;
                }
                else if (result.responce == 1) {

                    getColdDrawingWireDrawingDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);
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
   
    $("#btnSave").click(function (event) {
       
        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();
        var actualDim = true;


        if ($("#chkActualDim").prop("checked") == true) {

            if ($("#txtActualDieDimStart").val() != "" && $("#txtActualDieDimEnd").val() != "") {
                $("#checkActualDimension").removeClass("Mandatory");
            } else {
                $("#checkActualDimension").addClass("Mandatory");
            }

        } else {
            $("#checkActualDimension").removeClass("Mandatory");
        }



        if (!IsMandatoryFieldsFilled()) {
            return false;
        }


        if ($("#hdfDBOperation").val() == "") {
            var message = $("#lblErrorMessageE_Cold_Drawing_009").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }



        if (!checkDate($("#txtStartDate"))) {
            var message = $("#lblErrorMessageE_Cold_Drawing_010").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtStartDate", WARNING);
            return false;
        }

        if (isDuplicateThabaNo()) {
            return false;
        }

        if ($("#hdfDBOperation").val() == "Insert") {

            if (!IsDateValid(true)) {
                var message = $("#lblErrorMessageE_Cold_Drawing_031").text().trim() + "(" + $("#lblErrorMessageE_Cold_Drawing_033").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtStartDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
            if (($("#txtThaba1").val() == '') && ($("#txtThaba2").val() == '') && ($("#txtThaba3").val() == '') && ($("#txtThaba4").val() == '')) {
                var message = "";

                message = $.trim($("#lblErrorMessageE_Cold_Drawing_036").text());

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
                            SaveColdDrawingWireDrawingDetails($callFrom);
                        }
                        else {
                            // return false;

                        }
                    },
                    centerVertical: true
                });

            }
            else {
                SaveColdDrawingWireDrawingDetails($callFrom);
            }
            

        }
        else {

            // Check all error conditions
            if (!IsEndDateValid()) {

                return false;
            }

            if (!checkDate($("#txtEndDate"))) {

                var message = $("#lblErrorMessageE_Cold_Drawing_011").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "txtEndDate", WARNING);
                return false;

            }
            if (!IsDateValid(false)) {
                var message = $("#lblErrorMessageE_Cold_Drawing_032").text().trim() + "(" + $("#lblErrorMessageE_Cold_Drawing_033").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtEndDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
            if (isDuplicateThabaNo()) {
                return false;
            }

            if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK) {
                var dimValues = {};
                dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
                dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
                dimValues["DIM1"] = $("#txtActualDieDimStart").val() ? $("#txtActualDieDimStart").val() : null;
                dimValues["DIM2"] = $("#txtActualDieDimEnd").val() ? $("#txtActualDieDimEnd").val() : null;
                dimValues["DIM3"] = null;
                dimValues["DIM4"] = null;
                dimValues["DIM5"] = null;
                dimValues["DIM6"] = null;
                var result = IsActualDimentionValid(dimValues);
                if (result.IsSuccess == false) {
                    var focusText = "";
                    if (result.InvalidDim == $("#txtActualDieDimStart").val()) {
                        focusText = "txtActualDieDimStart";
                    }
                    if (result.InvalidDim == $("#txtActualDieDimEnd").val() && focusText == "") {
                        focusText = "txtActualDieDimEnd";
                    }
                   
                    var message = $("#lblConfirmationMessageC_DRAWING_W_ENTRY_002").text().trim(); //ActDimOutOfTolerance
                    showMessageDialog(message, focusText, WARNING);
                    return false;
                }
            }

            //If end time is less than 10 minutes after start time, show confirmation message.
            var startDateTime = $("#txtStartDate").val();
            var endDateTime = $("#txtEndDate").val();
            var result = IsEndDateTimeGreaterThanStartDateTimeByTenMin(startDateTime, endDateTime);
            if (result == false) {

                var message = "";

                message = $.trim($("#lblconfMsg_EndTimeLessThan10MinAfterStartTime").text());

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
                            UpdateColdDrawingWireDrawingDetails($callFrom);
                        }
                        else {
                            $("#txtEndDate").focus();
                        }

                    },
                    centerVertical: true
                });

            }
            else {

                UpdateColdDrawingWireDrawingDetails($callFrom);
            }
            
           // UpdateColdDrawingWireDrawingDetails($callFrom);
        }
    });
}

function enableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnChuubanSearch").removeAttr(DISABLED).removeClass(DISABLED);
}

function disableSaveButton() {

    $("#btnSave").attr("disabled", "disabled");
    $("#btnChuubanSearch").attr("disabled", "disabled");

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
            } else {
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


function IsEndDateValid() {

    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_Cold_Drawing_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_Cold_Drawing_DateTime").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
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


        if (focusFieldID == 'checkActualDimension') {
            if ($("#txtActualDieDimStart").val().trim() == "") {
                focusFieldID = "txtActualDieDimStart";
            } else {
                focusFieldID = "txtActualDieDimEnd";
            }
        }


        showMessageDialog(message, focusFieldID, WARNING)

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

    // Check if in input thaba textboxes, same thaba no is entered more than once
    //if ((($.inArray(inputThaba1, [inputThaba2, inputThaba3, inputThaba4]) > -1) && inputThaba1 != "") ||
    //    (($.inArray(inputThaba2, [inputThaba1, inputThaba3, inputThaba4]) > -1) && inputThaba2 != "") ||
    //    (($.inArray(inputThaba3, [inputThaba1, inputThaba2, inputThaba4]) > -1) && inputThaba3 != "") ||
    //    (($.inArray(inputThaba4, [inputThaba1, inputThaba2, inputThaba3]) > -1) && inputThaba4 != "")) {
    //    firstFieldID = "txtThaba1";
    //    message = $("#lblErrorMessageE_Multiple_SameThaba").text().trim();
    //    showMessageDialog(message, firstFieldID, WARNING);
    //    return true;
    //}
    if (duplicateThaba.length > 0) {
        if (duplicateThaba.length > 1) {
            message = $("#lblErrorMessageE_Multiple_SameThaba").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_Multiple_SameThaba").text().trim().replace("NNNN", duplicateThaba[0]);;
        }

        showMessageDialog(message, firstFieldID, WARNING);
        return true;
    }


    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK && $('#chkReDrawing').prop("checked") == false) {

            t1 = inputThaba1;
        }
    }
    if (inputThaba2 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba2]) == combinedDecisionOK && $('#chkReDrawing').prop("checked") == false) {

            t2 = inputThaba2;
        }
    }
    if (inputThaba3 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba3]) == combinedDecisionOK && $('#chkReDrawing').prop("checked") == false) {
            t3 = inputThaba3;
        }
    }
    if (inputThaba4 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba4]) == combinedDecisionOK && $('#chkReDrawing').prop("checked") == false) {

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
function getDrawingStartdetails(chuuban, gyouban, mechineCD, $callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "' ,mechineCD : '" + mechineCD + "', USER_NAME : '" + userName + "' }";

    var settings = {

        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetDrawingStartDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, mechineCD: mechineCD })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // bindStampDropdown(response.message);

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

function setNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetNextProcess",
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
        url:"/ColdDrawingWIREDRAWING/GetDecision",
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
    $("#ddlDecision").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setExteriorConditionKubunGet() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetExteriorCondition",
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindExteriorConditionDropdown(jsonResult) {
    $("#ddlExternalSurfaceCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalSurfaceCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function setMagneticDust() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetMDust",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindMDustDropdown(response.message);
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

function bindMDustDropdown(jsonResult) {
    $("#ddldustdetection").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddldustdetection").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setDiceInspStart() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetDiceInspStart",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindDiceStartdropdown(response.message);
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

function bindDiceStartdropdown(jsonResult) {
    $("#ddlDiceInspectionStart").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDiceInspectionStart").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setwindingShape() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetwindingShape",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindwindingshapedropdown(response.message);
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

function bindwindingshapedropdown(jsonResult) {
    $("#ddlWindingshape").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlWindingshape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function setDieDiaShape() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetDieDiaShape",
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
        $("#ddlDieShape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setDiceInspEnd() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetDiceInspEnd",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindDiceEnddropdown(response.message);
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

function bindDiceEnddropdown(jsonResult) {
    $("#ddlDiceInspectionEnd").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDiceInspectionEnd").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function bindStampDropdown(jsonResult) {
    //  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));
    $("#ddlStamp").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlStamp").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
}


function getColdDrawingWireDrawingDetailsByChuuban(chuuban, gyouban, mechineCD, $callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    //var jsonData = "{CHUUBAN :'" + chuuban + "'  , gyouban : '" + gyouban + "' ,mechineCD : '" + mechineCD + "', USER_NAME : '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetColdDrawingWireDrawingDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, MACHINE_CD: mechineCD, CULTURE_NAME: $("#hdfCultureName").val() })
    }

    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            //ClearData();
            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
            //if (jsonResult === null) {
            //    ClearData();
            //    disableStartDataPanel();
            //    var errorMessage = $("#lblErrorMessageE_Cold_Drawing_chuubannotfound").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}
            //if (jsonResult.procesS_PLAN_START_DATE == null) {
            //    ClearData();
            //    disableStartDataPanel();
            //    disableEndDataPanel();
            //    var OrNo = jsonResult.pP_CHUUBAN;
            //    var errorMessage = $("#lblErrorMessageE_Cold_Drawing_030").text().replace("99-999999-99", OrNo);
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}
            //if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
            //    ClearData();
            //    disableStartDataPanel();
            //    var errorMessage = $("#lblErrorMessageE_Cold_Drawing_004").text();
            //    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
            //    return false;
            //}
            disableSearchOrderNoPanel();
            enableStartDataPanel();
            setChuubanDetails(jsonResult);
            setStartData(jsonResult);

            if ($("#hdfDBOperation").val() == "Insert") {
                if (jsonResult.dW_END_DATE_STRING == null && jsonResult.dW_START_DATE_STRING !== null) {
                    enableStartDataPanel();
                    //enableEndDataPanel();
                    disableEndDataPanel();
                    $("#txtNoOfUnits").val(1);

                } else {
                    enableStartDataPanel();
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
                    $("#txtStartDate").focus();

                }

            } else if ($("#hdfDBOperation").val() == "Update") {
                setStartData(jsonResult);
                DisableSavedThaba();
                //  disableStartDataPanel();
                enableEndDataPanel();
                $("#txtNoOfUnits").val(1);
                $("#txtThaba1").addClass("Mandatory");
                // Set End Operator same as Start Operator

                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndDate").focus();
                //setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed after enableEndDataPanel(). It is because enableEndDataPanel() enables all fields, and setChuubanDimLenToleranceMessages() will again disable some fields based on values of Dim and Len Tolerances.
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

function getThabaDetailsByChuuban(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonThabaDetailData + "}"
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
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
            message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });
}

function setThabaDetails(jsonResult) {
    // console.log(jsonResult);
  //  clearThabaDetailsGrid();
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

                //if (parseInt(thabaNo) >= parseInt(maxRegisteredThabaNo)) {
                //    maxRegisteredThabaNo = parseInt(thabaNo);
                //}
                var decisionLegend = $(decisionLegends[decision_value]);
                $("#lblThabaNo" + thabaNo).text(thabaNo);
                $("#lblDecisionNo" + thabaNo).html("").append(decisionLegend);
            });
        }
    });

    //  $("#txtFinalRegisteredThabaNoValue").text(maxRegisteredThabaNo);
}


function setChuubanDimLenToleranceMessages() {
    // Show/hide Tolerance Messages
    if ((dimTolMin == null || dimTolMin == "" || Math.abs(parseFloat(dimTolMin)) == parseFloat(0))
        && (dimTolMax == null || dimTolMax == "" || Math.abs(parseFloat(dimTolMax)) == parseFloat(0))) {
        $("#lblDimToleranceMsgT1").show();
        $("#lblDimToleranceMsgT4").show();
        $("#chkActualDim").prop('checked', false);
        $("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
        toggleActualDimFieldsMandatory($("#chkActualDim"));
    }
    else {
        $("#lblDimToleranceMsgT1").hide();
        $("#lblDimToleranceMsgT4").hide();
        $("#chkActualDim").prop('checked', true);
        $("#chkActualDim").removeClass(DISABLED).removeAttr(DISABLED);
        toggleActualDimFieldsMandatory($("#chkActualDim"));
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

        $("#chkFinishedQuantity").prop('checked', false);
        $("#chkFinishedQuantity").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
    else {
        $("#lblLengthMsgT1").hide();
        $("#lblLengthMsgT4").hide();

        $("#chkFinishedQuantity").prop('checked', true);
        $("#chkFinishedQuantity").removeClass(DISABLED).removeAttr(DISABLED);
    }

    // For setting chkActualDim, we need to check both StdLength and LengthTolerance
    if (((lenTolMin == null || lenTolMin == "" || Math.abs(parseFloat(lenTolMin)) == parseFloat(0))
        && (lenTolMax == null || lenTolMax == "" || Math.abs(parseFloat(lenTolMax)) == parseFloat(0)))
        ||
        (lenStd == null || lenStd == "" || Math.abs(parseFloat(lenStd)) == parseFloat(0))) {
        $("#chkActualDim").prop('checked', false);
        $("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
        toggleActualLengthFieldsMandatory($("#chkActualDim"));
    }
    else {
        $("#chkActualDim").prop('checked', true);
        $("#chkActualDim").removeClass(DISABLED).removeAttr(DISABLED);
        toggleActualLengthFieldsMandatory($("#chkActualDim"));
    }
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

function setChuubanDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#btnSave").removeAttr("disabled");
   dbOperation = "Insert";
    if ((jsonResult.dW_START_DATE_STRING != null) && (jsonResult.dW_END_DATE_STRING == null) && jsonResult.pasT_DATA_EXIST == "YES") {
        dbOperation = "Update";
    }
    $("#hdfDBOperation").val(dbOperation);
    $("#txtThaba1").val(jsonResult.dW_TABA_NO_1);
    $("#txtThaba2").val(jsonResult.dW_TABA_NO_2);
    $("#txtThaba3").val(jsonResult.dW_TABA_NO_3);
    $("#txtThaba4").val(jsonResult.dW_TABA_NO_4);
    DrawingCD = $.isNumeric(jsonResult.drawinG_CD) ? parseInt(jsonResult.drawinG_CD) : 0;

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);

    $("#txtProductSpecNoT1").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN);
    $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN);
    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);

    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtCustomerInchargeT4").val(jsonResult.pP_EMPLOYEE_NAME);

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
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) > 0) {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
        $("#txtUnitWtT4").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
    }

    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtColorT4").val(jsonResult.pP_COLOR);

    $("#txtConditionT1").val(jsonResult.conditioN_DESCRIPTION); 
    $("#txtConditionT4").val(jsonResult.conditioN_DESCRIPTION); 
    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMaterialNameMakerT4").val(jsonResult.pP_MATL_NAME_MAK);

    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlShapeT4").val(jsonResult.PP_MATL_SHAPE);

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

    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);

    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtStampT4").val(jsonResult.pP_STAMP);

    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_STRING);

    $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);

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
    
    // 3rd Tab
    resetMessageDetailsGrid();
    if (jsonResult.dW_MESSAGE_LIST) {
        setMessageDetailsGrid(jsonResult.dW_MESSAGE_LIST);
    }

    //4th Tab 

    $("#txtElongation").val(jsonResult.pP_ELONGATION);
    $("#txtTensileStrength").val(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtHardness").val(jsonResult.pP_HARDNESS);
    $("#txtDecarb").val(jsonResult.pP_DECARB);
    $("#txtYieldPoint").val(jsonResult.pP_YEILD);
    $("#txtReduction").val(jsonResult.pP_PRESS);// 

    $("#txtJISTestSpecimen").val(jsonResult.pP_JIS_TEST_SPEC);

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


function setStartData(jsonResult) {
    debugger;
    //$("#chkmiddle").prop('checked', true);
    //$("#chkReDrawing").prop('checked', true);
    //$("#chkActualDim").prop('checked', true);
    //$("#txtStartOperator").val(jsonResult.DW_START_OPERATOR_NAME); textbox will blank if removed comment
    if (jsonResult.dW_END_DATE_STRING == null && jsonResult.dW_START_DATE_STRING !== null && jsonResult.pasT_DATA_EXIST == "YES") {

        $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        $("#txtStartOperator").val(jsonResult.dW_START_OPERATOR_NAME);
        $("#txtRollingnumber").val(jsonResult.dW_ROLLING_NUMBER);
        $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);
        $("#txtDieNo").val(jsonResult.dW_DIE_NO);
        $("#txtDieDim").val(jsonResult.dW_DIE_DIM);
        $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
        $("#ddlWindingshape").val(jsonResult.dW_WINDING_SHAPE || $("#ddlWindingshape").prop('selectedIndex'));

        $("#ddlDiceInspectionStart").val(jsonResult.dW_DIE_CHK_START || $("#ddlDiceInspectionStart").prop('selectedIndex'));


        $("#hdfStartOperatorCD").val(jsonResult.dW_START_OPERATOR_CD);
        if (jsonResult.dW_IM_DRAWING == 0) {
            $("#chkmiddle").prop('checked', false);
            $("#chkReDrawing").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
        } else {

            $("#chkmiddle").prop('checked', true);
        }
        if (jsonResult.dW_REDRAWING == 0) {
            $("#chkReDrawing").prop('checked', false);
        } else {

            $("#chkReDrawing").prop('checked', true);
        }
        
    }
    else {
        debugger;
        //if (jsonResult.shoW_DEFAULT == 1) {
        if (jsonResult.dW_START_DATE_STRING != null && jsonResult.dW_END_DATE_STRING != null && jsonResult.shoW_DEFAULT == 1 && jsonResult.pasT_DATA_EXIST == "YES") { 
            $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);
            $("#ddlDiceInspectionStart").val(jsonResult.dW_DIE_CHK_START || $("#ddlDiceInspectionStart").prop('selectedIndex'));
            $("#txtDieDim").val(jsonResult.dW_DIE_DIM);
            $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
            $("#ddlWindingshape").val(jsonResult.dW_WINDING_SHAPE || $("#ddlWindingshape").prop('selectedIndex'));
            $("#txtRollingnumber").val(jsonResult.dW_ROLLING_NUMBER);
            $("#txtDieNo").val(jsonResult.dW_DIE_NO);
        }
    }


    if (dbOperation == "Insert") {
        setStartSysDateTime();
    }



}

function enableSearchPanel() {
    $("#txtMachineType").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtNifudaNo1").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtChuuban2").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtGyouban2").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnChuubanSearch").removeAttr(DISABLED).removeClass(DISABLED);
}

function disableSearchPanel() {
    $("#txtMachineType").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtNifudaNo1").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtChuuban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtGyouban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#btnChuubanSearch").attr(DISABLED, DISABLED).addClass(DISABLED);
}

function enableStartDataPanel(jsonResult) {
    debugger
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
        }
    });

    $.each($startDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    //$("#chkmiddle").prop("checked", true);
    //disableRedrawCheckbox();
}

function enableEndDataPanel() {

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
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });


    if ($("#chkmiddle").prop("checked") == true) {
        disableRedrawCheckbox();
    } else {
        enableRedrawCheckbox();
    }

    $('#ddlUnitType option:eq(0)').prop('selected', true);

}
function enableRedrawCheckbox() {
    $("#chkReDrawing").prop("checked", false);
    $("#chkActualDim").prop("checked", false);
    $("#chkReDrawing").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    if ($("#txtEndDate").hasClass("disabled") != true) {
        $("#chkActualDim").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    }
}
function disableRedrawCheckbox() {
    $("#chkReDrawing").prop("checked", false);
    $("#chkActualDim").prop("checked", false);
    $("#chkReDrawing").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
}
$("#chkmiddle").click(function () {
    if ($("#chkmiddle").is(":checked") == true) {
        disableRedrawCheckbox();
        $("#chkActualDim").click();
    } else {
        enableRedrawCheckbox();
        $("#chkActualDim").click();
    }
});
function setStartDataPanelDefaultData(jsonResult) {

    if (jsonResult.DW_START_DATE_STRING != null) {
        var startDateTime = jsonResult.DW_START_DATE_STRING;
        $("#txtStartDate").val(new Date(startDateTime).format("yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
        $("#txtStartTime").val(new Date(startDateTime).format("HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    }
    else {
        $("#btnEndDateTime").click();
    }
}


function bindProcessDefectDropdown(jsonResult) {
    $("#ddlDefects1").html("").append($('<option value="0"></option>'));
    $("#ddlDefects2").html("").append($('<option value="0"></option>'));
    $("#ddlDefects3").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDefects1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlDefects2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlDefects3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function SaveColdDrawingWireDrawingDetails($callFrom) {

    var ajaxTime = new Date().getDate();

    var jsonDataForSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val().trim(),
        PP_GYOUBAN: $("#txtGyouban2").val().trim(),
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val().trim(),
        MACHINE_CD: $("#hdfMachineCD").val().trim(),
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0].trim() + "T" + $("#txtStartDate").val().split(" ")[1].trim() + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val().trim(),
        ROLLING_NUMBER: $("#txtRollingnumber").val().trim(),
        FURNACE_NO: $("#txtFurnaceNum").val(),

        DIE_DIM: $("#txtDieDim").val().trim(),
        DIE_SHAPE: $("#ddlDieShape option:selected").val().trim(),
        DIE_NO: $("#txtDieNo").val().trim(),
        DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val().trim(),
        WINDING_SHAPE: $("#ddlWindingshape option:selected").val().trim(),
        IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),

        USER_ID: $("#hdfLoginId").val().trim(),
        DW_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val().trim() : null,
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val().trim() : null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val().trim() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val().trim() : null,

        //PP_CHUUBAN: $("#txtChuuban2").val(),
        //PP_GYOUBAN: $("#txtGyouban2").val(),
        //PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        //MACHINE_CD: $("#hdfMachineCD").val(),
        //DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        //DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val(),
        //ROLLING_NUMBER: $("#txtRollingnumber").val().trim(),
        //FURNACE_NO: $("#txtFurnaceNum").val(),

        //DIE_DIM: $("#txtDieDim").val(),
        //DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        //DIE_NO: $("#txtDieNo").val(),
        //DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        ////  WINDING_SHAPE: $("#ddlWindingshape option:selected").val() ? $("#ddlWindingshape option:selected").val() : null,
        //WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        //IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),

        //REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),

        //USER_ID: $("#hdfLoginId").val(),
        //DW_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,

        //DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,

        //DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        //DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,

    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonDataForSave + "}"
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            enableEndDataPanel();
            DisableSavedThaba();
            $("#txtNoOfUnits").val(1);
            $("#txtThaba1").addClass("Mandatory");
            //  disableStartDataPanel()
            var successMessage = $("#lblSuccessMessageS_Cold_Drawing_ENTRY_001").text();
            showMessageDialog(successMessage, "txtEndOperator", SUCCESS);
            var jsonResult = response.message;

            setChuubanDetails(jsonResult);
            //setStartData(jsonResult);

            //disableSaveButton();
            getMachineDetails($("#hdfMachineCD").val(), $callFrom);
            $("#hdfDBOperation").val("Update");

            $("#btnEndDateTime").click(); // Get current time for end operation.
            $("#txtEndOperator").focus();

            $("#txtEndOperator").val($("#txtStartOperator").val());
            $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
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

function UpdateColdDrawingWireDrawingDetails($callFrom) {

    var ajaxTime = new Date().getDate();
    var jsonDataForUpdate = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val().trim(),
        PP_GYOUBAN: $("#txtGyouban2").val().trim(),
        DRAWING_CD: DrawingCD,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        MACHINE_CD: $("#hdfMachineCD").val(),
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0].trim() + "T" + $("#txtStartDate").val().split(" ")[1].trim() + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val(),
        ROLLING_NUMBER: $("#txtRollingnumber").val().trim(),
        FURNACE_NO: $("#txtFurnaceNum").val(),
       // MSG_CD: $("#hfld_MessageCD").val().trim(),
        DIE_DIM: $("#txtDieDim").val().trim(),
        DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        DIE_NO: $("#txtDieNo").val().trim(),
        DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),
        DW_END_DATE: ($("#txtEndDate").val().split(" ")[0].trim() + "T" + $("#txtEndDate").val().split(" ")[1].trim() + ":00.000Z").replaceAll('/', '-'),
        DW_END_OPERATOR_CD: $("#hdfEndOperatorCD").val(),
        DW_ACT_DIM_CHK: ($("#chkActualDim").is(':checked') ? 1 : 0),
        DW_ACT_DIM_START: $("#txtActualDieDimStart").val() ? $("#txtActualDieDimStart").val().trim() : null,
        DW_ACT_DIM_END: $("#txtActualDieDimEnd").val() ? $("#txtActualDieDimEnd").val().trim() : null,
        DW_FINISHED_COILS: $("#txtNoOfUnits").val().trim(),
        DW_FINISHED_QTY: $("#txtWeight").val().trim(),
        DW_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val().trim() : null,
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val().trim() : null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val().trim() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val().trim() : null,
        DW_EXT_TEXTURE_STATUS: $("#ddlExternalSurfaceCondition option:selected").val(),
        DW_MAGNETIC_INSPECTIONS: $("#ddldustdetection option:selected").val(),
        DIE_CHK_END: $("#ddlDiceInspectionEnd option:selected").val(),
        DW_DECISION: $("#ddlDecision option:selected").val(),
        DW_NEXT_PROCESS: $("#ddlNextProcess option:selected").val(),
        DW_REMARKS: $("#txtRemarks").val(),
        USER_ID: $("#hdfLoginId").val()

        //PP_CHUUBAN: $("#txtChuuban2").val(),
        //PP_GYOUBAN: $("#txtGyouban2").val(),
        //DRAWING_CD: DrawingCD,
        //PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        //MACHINE_CD: $("#hdfMachineCD").val(),
        //DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ////  DW_START_DATE: $("#txtStartDate").val(),
        //DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val(),
        //ROLLING_NUMBER: $("#txtRollingnumber").val().trim(),
        //FURNACE_NO: $("#txtFurnaceNum").val(),
        //MSG_CD: parseInt($("#hfld_MessageCD").val()),
        //DIE_DIM: $("#txtDieDim").val(),
        //DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        //DIE_NO: $("#txtDieNo").val(),
        //DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        //WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        //IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        //REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),

        //DW_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ////  DW_END_DATE: $("#txtEndDate").val(),
        //DW_END_OPERATOR_CD: $("#hdfEndOperatorCD").val(),
        //DW_ACT_DIM_CHK: ($("#chkActualDim").is(':checked') ? 1 : 0),
        //DW_ACT_DIM_START: $("#txtActualDieDimStart").val() ? $("#txtActualDieDimStart").val() : null,
        //DW_ACT_DIM_END: $("#txtActualDieDimEnd").val() ? $("#txtActualDieDimEnd").val() : null,
        //DW_FINISHED_COILS: $("#txtNoOfUnits").val(),
        
        //DW_FINISHED_QTY: $("#txtWeight").val(),
        //DW_TABA_NO_1: $("#txtThaba1").val(),

        //DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,

        //DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        //DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        //DW_EXT_TEXTURE_STATUS: $("#ddlExternalSurfaceCondition option:selected").val(),
        //DW_MAGNETIC_INSPECTIONS: $("#ddldustdetection option:selected").val(),
        //DIE_CHK_END: $("#ddlDiceInspectionEnd option:selected").val(),
        //DW_DECISION: $("#ddlDecision option:selected").val(),
        //DW_NEXT_PROCESS: $("#ddlNextProcess option:selected").val(),

        //DW_REMARKS: $("#txtRemarks").val(),
        //USER_ID: $("#hdfLoginId").val()
        //USER_NAME: $("#txtChuuban2").val(),
    });

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataForUpdate
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            disableEndDataPanel();
            disableStartDataPanel();
            //disableSearchPanel();
            disableSaveButton();
            getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
            getMachineDetails($("#hdfMachineCD").val(), $callFrom);

            var successMessage = $("#lblSuccessMessageS_Cold_Drawing_ENTRY_001").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            var jsonResult = response.message;


            //  disableStartDataPanel();


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
function GetLocationCd() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetUserLocationById?userId=",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        cache: false,
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

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

function setDecimalFormats() {
    $("#txtFinishedBarCount, #txtNoOfUnits").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtNGQty, #txtNgWeight, #txtWeight").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtDieDim, #txtActual_Start, #txtActual_End , #txtActualDieDimStart, #txtActualDieDimEnd").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    }); 
}

$(document).ready(function () {


    resetColdDrawingWireHistoryGrid();
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    window.resize(1920, 1080);

    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }

    //GetLocationCd();

    setGlobalVariables();

    bindEvents();
    setDecimalFormats();

    //hideToleranceErrorLabels();
    disableStartDataPanel();

    disableEndDataPanel();
    $("#btnSave").attr("disabled", "disabled");
    $(".FocusDefault").focus();
    userName = $("#hdfUserName").val();


    $("#tdLblChuuban").hide();
    $("#tdtxtNifudaNo1").hide();
    $("#dvMessage").hide();
    $("#TbHistorySearch-tab").hide();

});

$("#chkReDrawing").focusout(function (event) {
    if ($("#txtEndDate").attr(DISABLED)) {
        $("#txtThaba1").focus();
    } else
    {
        $("#txtEndDate").focus();
    }
    
});
$("#txtThaba4").focusout(function (event) {
    if ($("#ddlExternalSurfaceCondition").attr(DISABLED)) {
        $("#btnSave").focus();
    } else {
        $("#ddlExternalSurfaceCondition").focus();
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
        url: APIURL + "/ColdDrawingWIREDRAWING/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });




    return returnval;
}

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