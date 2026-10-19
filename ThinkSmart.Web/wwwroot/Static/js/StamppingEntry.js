// var APIURL ="https://localhost:7269";
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
var stamppingCD;
var registeredThabaNumbersAndDecision = {};
var combinedDecisionOK = 1;
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
let enterPressedMachine = false;
var machineModified = false;
var operatorStartModified = false;
var operatorEndModified = false;
let enterPressedStartOpr = false;
let enterPressedEndOpr = false;

userName = ""; // assigned at Document.ready

// Following decision legends are to show decision colors on Top Thaba Grid.
//var decisionLegends = {
//    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
//    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
//    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
//    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>",
//    "5": "<span id='lblStar' class='labelClass' style='color:Darkred;'>■</span>"

//}

var machineGridDataTr1 = " <tr id='tr1ItemGrdvStampingMachine'> "
    + " <td class='td tdH2' rowspan='2' style='background-color:COLOR;'> "
    + " <span class='txtColor' style='background-color:COLOR; height:80px; width:35px;'>&nbsp;</span>"
    + " </td>"
    + " <td class='td tdH2 dataNowrap padddingDataLeft' rowspan='2' style='text-align:left'>"
    + " <span class='txtGridCustomerName labelClass'> valtxtGridCustomerName</span>"
    + " </td>"
    + " <td class='td tdH2 padddingDataLeft'  style='padding-right: 0px; padding-left: 0px;'>"
    + " <span class='txtGridChuban labelClass' style='font-size: 12.2px;'>valtxtGridChuban</span>"
    + " </td>"

    + " <td class='td tdH2 padddingDataLeft dataNowrap' rowspan='2'  style='text-align:left'>"
    + " <span class='txtGridVerification labelClass' >valtxtMetallicPlateCode</span></td>"
    + " <td class='td tdH2 dataNowrap padddingDataLeft' rowspan='2'  style='text-align:left'>"
    + " <span class='txtProdNameCust labelClass' >valtxtProdNameCust</span></td>"
    + " <td class='td tdH2 padddingDataLeft dataNowrap' rowspan='2'  style='text-align:left'>"
    + " <span class='txtGridCharge labelClass' >valtxtGridCharge</span></td>"
    + " <td class='td tdH2 padddingDataRight dataNowrap panelPaddingLeft' rowspan='2'  style='text-align:right'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtMaterialDimD</span></td>"

    + "<td class='td tdH2 padddingDataRight dataNowrap' rowspan='2' style='text-align:right'> "
    + "<span class='txtGridLength labelClass' >valtxtGridLength</span> "
    + "</td> "
    + "<td class='td tdH2 dataNowrap padddingDataRight' rowspan='2' style='text-align:right'> "
    + "<span class='txtGridTabaNo labelClass' >valtxtGridTabaNo</span> "
    + "</td> "
    + " <td class='td tdH2 padddingDataRight dataNowrap panelPaddingLeft' rowspan='2' style='text-align:right'>"
    + " <span class='txtGridNoOfCoils labelClass' >valtxtGridOkBars</span>"
    + " </td>"
    + " <td class='td tdH2 padddingDataRight dataNowrap panelPaddingLeft' rowspan='2' style='text-align:right'>"
    + " <span class='txtGridQuantity labelClass' >valtxtGridWeight</span>"
    + " </td>"
    + "<td class='td tdH2 padddingDataLeft dataNowrap' rowspan='2' style='text-align:left'> "
    + "<span class='txtGridStampDetails labelClass' >valtxtGridStampDetails</span> "
    + " </td> "
    + "<td class='td tdH2 padddingDataLeft dataNowrap' rowspan='2' style='text-align:left'> "
    + "<span class='txtGridRemarkValue labelClass' >valtxtGridRemark</span> "
    + " </td> "


    + "  </tr>";

var machineGridDataTr2 = "<tr id='tr2ItemGrdvStampingMachine'>"
    + "  <td class='td tdH2 GridAlignmentProdSpecNo dataNowrap padddingDataLeft' style='text-align:Center'> "
    + "<span class='txtGridSpecification labelClass' >valtxtGridSpecification</span> "
    + "</td> "

    + " </tr> ";

function resetMachineDetailsGrid() {
    ClearTable($("#grdvStampingMachine"), "ItemGrdvStampingMachine", "");
    $grdvStampingMachine = $("#grdvStampingMachine");
    $grdvStampingMachine.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow" style="border:1px solid #FF9900">').append(
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        $('<td rowspan=2>').text(""),
        
    );
    var $tr1 = $('<tr class="GridNormalRow" style="border:1px solid #FF9900">').append(
        $('<td>').text(""),
    );
    $grdvStampingMachine.append($tr,$tr1);
}
function Initialize() {
    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
   
    EnableSearchPanel();
    $(".FocusDefault").focus();
    DisableStartDataPanel();
    DisableEndDataPanel();
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    ClearPanel($("#pnlProcessInfo")); // Tab 3
    ClearPanel($("#pnlTestSpecifications")); // Tab 3
    ClearThabaDetailsGrid();
    ClearTable($("#grdvStampingMachine"), "ItemGrdvStampingMachine", "");     // 24 Hrs Machine details grid should not be initialized. It should be kept as it is. 
    ClearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 2
    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 3

    $("#txtMachineType").val("");
    $("#txtNifudaNo1").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $("#txtStartDate").val("");
    $("#txtStartTime").val("");
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
    $("#txtStamp").val("");
    //$("#ddlStamp").html("");

    ResetGlobalVariables();
    DisableSaveButton();
    ResetMessageDetailsGrid();
    ResetProcessPlanDetailsGrid();
    resetMachineDetailsGrid();
    EnableSearchOrderNoPanel();
    //SetStamp();
}

function ClearData() {
    $("#btnSave").attr("disabled", "disabled");    
    EnableSearchPanel();
    ClearThabaDetailsGrid(); 
    ClearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 2
    ClearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 3

    ClearPanel($("#pnlProductDetails"));
    DisableEndDataPanel();
    ClearPanel($("#pnlProcessStartData"));   
    ClearPanel($("#pnlProcessEndData"));

    $(".FocusDefault").focus();
    ClearPanel($("#pnlProcessInfo")); // Tab 3
    ClearPanel($("#pnlTestSpecifications")); // Tab 3
    EnableSearchOrderNoPanel();
    $("#txtStamp").val("");
    //$("#ddlStamp").html("");
   
}


function OKQtyRegionClassChange() {
    
    var txtNGWeight1 = $("#txtNGWeight1").val();
    var ddlDefects1 = document.getElementById("ddlDefects1");
    if (txtNGWeight1 > 0) {
        ChangeControlDisplay('#ddlDefects1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlDefects1").removeAttr("disabled")
    }
    else {
        ChangeControlDisplay('#ddlDefects1', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);
        $("#ddlDefects1 option:selected").prop("selected", false);
        $("#ddlDefects1").attr("disabled", "disabled");
    }
    var txtNGWeight2 = $("#txtNGWeight2").val();
    var ddlDefects2 = document.getElementById("ddlDefects2");
    if (txtNGWeight2 > 0) {
        ChangeControlDisplay('#ddlDefects2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlDefects2").removeAttr("disabled")
    }
    else {
        ChangeControlDisplay('#ddlDefects2', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);
        $("#ddlDefects2 option:selected").prop("selected", false);
        $("#ddlDefects2").attr("disabled", "disabled");
    }
    var txtNGWeight3 = $("#txtNGWeight3").val();
    var ddlDefects3 = document.getElementById("ddlDefects3");
    if (txtNGWeight3 > 0) {
        ChangeControlDisplay('#ddlDefects3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlDefects3").removeAttr("disabled")
    }
    else {
        ChangeControlDisplay('#ddlDefects3', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);
        $("#ddlDefects3 option:selected").prop("selected", false);
        $("#ddlDefects3").attr("disabled", "disabled");
    }
    var add = 0;
    $(".amt").each(function () {
        add += Number($(this).val());
   
        if (add == 0) {
        $("#txtNgWeight").val("");
        }
        else {
            $("#txtNgWeight").val(add);
            }
        });
}

function DisableStartDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#pnlProcessStartData");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("select"), function (index, dropdown) {        
        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    //$("#txtStamp").css({ display: 'none' });
    //$("#ddlStamp").html("");
    //$("#ddlStamp").append($('<option>' + $("#txtStamp").val() + '</option>'));
    //$("#txtStamp").val("");
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

function ResetProcessPlanDetailsGrid() {
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

    // Add data rows
    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
       
        if ($("#hdfCultureName").val() == "en") {
            processName = jsonObject.procesS_NAME_EN;
        }
        else {
            processName = jsonObject.procesS_NAME_JP;
        }

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


function ClearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
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
        "<td class='ProdPointDetails' style='font-weight: normal;text-align:left;'>" + $_prodPint + "</td>" +
        "</tr>");
    $grdvProdPointData.append($tr);
}

function SetStartSysDateTime() {
    
    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            //$("#txtStartTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetEndSysDateTime() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetSysDateTime",
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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetMachineNamebyCD($target) {
    debugger;
     var ajaxTime = new Date().getTime();  
    var machineCD = $target.val().trim();
    var machineval = machineCD.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineNumber = machineCD.substring(3, 7);

    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_STA_ENTRY_034").text().trim().replace("*****", $("#tdLblMachineType").text());
        
        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }

    if (machineNumber == "") {
        var message = $("#lblErrorMessageE_STA_ENTRY_08").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
    else if (machineNumber.length > 4) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {
        var jsonData = "{machineCD: '" + machineCD + "', USER_NAME : '" + userName + "'}"
        var settings = {
            async: false,
            crossDomain: true,
            dataType: "json",
            url: APIURL + "/Stampping/GetMachineNamebyCD?machineCD=" + parseInt(machineNumber),
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var machineName = response.message;
                console.log(machineName);
                if (machineName.key == null || machineName.key == "") {
                    var message = $("#lblErrorMessageE_Stampping_ENTRY_013").text().trim().replace("*****",$("#tdLblMachineType").text());
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the machineName barcode again
                }
                else if (machineName.value == "inactive") {
                    var message = $("#lblErrorMessageE_HIKI_ENTRY_016").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
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
                message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
   
}




function GetMachineDetails(machineCd, $callFrom) {
    
    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }
    var ajaxTime = new Date().getTime();

    var jsonData = "{machineCd: '" + machineCd + "', USER_NAME: '" + userName + "'}"

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetMachineDetails?machineCD=" + machineCd + "&cultureName=" + $("#hdfCultureName").val(),
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
                resetMachineDetailsGrid();

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
        });
}

function Set24HrMachineData(jsonResult) {
    debugger;
    ClearTable($("#grdvStampingMachine"), "ItemGrdvStampingMachine", "");
    ClearTable($("#grdvStampingMachine"), "", "GridNormalRow");
    $.each(jsonResult, function (index, jsonObject) {
        if (jsonObject.machinE_CD) {
            var firstRow = machineGridDataTr1;
            if (jsonObject.sT_DECISION != null) {
                if (jsonObject.sT_DECISION == 1) {
                    firstRow = firstRow.replace("COLOR", "green");    // $("●").css('color', 'green');             
                    firstRow = firstRow.replace("#E2E2E2", "green");
                }
                else if (jsonObject.sT_DECISION == 2) {
                    firstRow = firstRow.replace("COLOR", "red");    //firstRow = $("●").css('color', 'red');
                    firstRow = firstRow.replace("#E2E2E2", "red");
                }
                else if (jsonObject.sT_DECISION == 3) {
                    firstRow = firstRow.replace("COLOR", "blue");    //firstRow = $("●").css('color', 'blue');
                    firstRow = firstRow.replace("#E2E2E2", "blue");
                }
                else if (jsonObject.sT_DECISION == 4) {
                    firstRow = firstRow.replace("COLOR", "Yellow");  //firstRow = $("●").css('color', 'Yellow');
                    firstRow = firstRow.replace("#E2E2E2", "Yellow");
                }
                else if (jsonObject.sT_DECISION == 5) {
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

            if (jsonObject.sT_METALLIC_PLATE_CODE != null)
                firstRow = firstRow.replace("valtxtMetallicPlateCode", jsonObject.sT_METALLIC_PLATE_CODE);
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


            if (jsonObject.pP_PROD_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtMaterialDimD", jsonObject.pP_PROD_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtMaterialDimD", "");
            
            if (jsonObject.sT_OK_BARS != null && jsonObject.sT_OK_BARS != 0)
                firstRow = firstRow.replace("valtxtGridOkBars", jsonObject.sT_OK_BARS + " " + jsonObject.sT_UNIT_TYPE_STRING);
            else
                firstRow = firstRow.replace("valtxtGridOkBars", "");
            
            if (jsonObject.sT_OK_QTY != null && jsonObject.sT_OK_QTY != 0)
                firstRow = firstRow.replace("valtxtGridWeight", jsonObject.sT_OK_QTY);
            else
                firstRow = firstRow.replace("valtxtGridWeight", "");

            if (jsonObject.pROD_DIM_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridDimDShape", jsonObject.pP_MATL_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridDimDShape", "");

            if (jsonObject.pP_PROD_LENGTH_STRING != null)
                firstRow = firstRow.replace("valtxtGridLength", jsonObject.pP_PROD_LENGTH_STRING);
            else
                firstRow = firstRow.replace("valtxtGridLength", "");

            if (jsonObject.sT_TABA_NO != null && jsonObject.sT_TABA_NO != 0)
                firstRow = firstRow.replace("valtxtGridTabaNo", jsonObject.sT_TABA_NO);
            else
                firstRow = firstRow.replace("valtxtGridTabaNo", "");

            if (jsonObject.stamP_DETAILS != null)
                firstRow = firstRow.replace("valtxtGridStampDetails", jsonObject.stamP_DETAILS );
            else
                firstRow = firstRow.replace("valtxtGridStampDetails", "");

            if (jsonObject.sT_REMARKS != null)
                firstRow = firstRow.replace("valtxtGridRemark", jsonObject.sT_REMARKS);
            else
                firstRow = firstRow.replace("valtxtGridRemark", "");

            $("#grdvStampingMachine").append($(firstRow));

            // tr2 (Second row)

            var secondtRow = machineGridDataTr2;

            if (jsonObject.proD_SPEC_NO != null)
                secondtRow = secondtRow.replace("valtxtGridSpecification", jsonObject.proD_SPEC_NO);
            else
                secondtRow = secondtRow.replace("valtxtGridSpecification", "");           

            if (jsonObject.sT_NG_BARS1 != null)
                secondtRow = secondtRow.replace("valtxtGridEcNgBarTotal", jsonObject.sT_NG_BARS1);
            else
                secondtRow = secondtRow.replace("valtxtGridEcNgBarTotal", "");

            $("#grdvStampingMachine").append($(secondtRow));
        }
    });
}

function GetOperatorNameByCD($target) {
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

        var message = $("#lblErrorMessageE_STA_ENTRY_013").text().trim().replace("*****", $("#lblStartOperator").text());
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    //else if (operatorNumber.length > 6) {
    //    var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
    //    showMessageDialog(message, $target.attr("id"), WARNING);
    //    $target.val("");
    //    return false;
    //}
    else {
        var jsonData = "{OperatorCD: '" + operatorNumber + "', USER_NAME : '" + userName + "'}"

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Stampping/GetOperatorNamebyCD?OperatorCD=" + operatorNumber,
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonresult = response.message;

                if (jsonresult == null || jsonresult == "") {
                    var message = $("#lblErrorMessageE_Stampping_ENTRY_013").text().trim().replace("*****", $("#lblStartOperator").text());
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                    $target.focus();
                }

                else {
                    if (jsonresult.iN_ACTIVE_FLAG == 1) {
                       // var message = $("#lblErrorMessageE_HIKI_ENTRY_016").text().trim(); 
                        var message = $("#lblErrorMessageE_STA_ENTRY_015").text().trim();
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
                message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            })
    }

   
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
    combinedDecisionOK = 1;
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
    enterPressedMachine = false;
    machineModified = false;
    operatorStartModified = false;
    operatorEndModified = false;
    DISABLED = "disabled";
    typeDropDown = "select-one";
    typeText = "text";
    //stamppingCD = "";
}

function BindEvents() {   
    $("#btnInitialize").click(function (event) {
        Initialize();
    });
    DisableSaveButton();

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
                ClearTable($("#grdvStampingMachine"), "ItemGrdvStampingMachine", "");               
                var message = $("#lblErrorMessageE_STA_ENTRY_08").text().trim();
                showMessageDialog(message, "txtMachineType", WARNING);
            } else {
                var machineCD = $(this).val().trim();
                var machineName = machineCD.substring(0, 3);
                var machineNumber = machineCD.substring(3, 7);
                $("#hdfMachineCD").val(machineNumber);
                enterPressedMachine = true;
                GetMachineNamebyCD($(this));
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
            GetMachineNamebyCD($(this));
            $("#txtNifudaNo1").focus();
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

        $("#txtMetalicPltCode").focus();

    }

    if (e.type === 'focusout' && operator && operatorStartModified == true) {
        if (enterPressedStartOpr) {

            enterPressedStartOpr = false; // Reset the flag
            return false;
        }
        $("#hdfStartOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtMetalicPltCode").focus();
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

          
            $("#txtFinishedBarCount").focus();
        }
        if (e.type === 'focusout' && operator && operatorEndModified == true) {
            if (enterPressedEndOpr) {

                enterPressedEndOpr = false; // Reset the flag
                return false;
            }
            $("#hdfEndOperatorCD").val(operatorNumber);
            GetOperatorNameByCD($(this));
          
            $("#txtFinishedBarCount").focus();
        }
        enterPressedEndOpr = false; // Reset the flag
    });


    $("#btnStartDateTime").click(function (event) {
        SetStartSysDateTime();
    });
    $("#btnEndDateTime").click(function (event) {
        SetEndSysDateTime();
    });

    $("#btnChuubanSearch").click(function (event) {
        debugger;        
        ClearPanel($("#pnlProcessStartData"));
        ClearPanel($("#pnlProcessEndData"));
        ResetResponseTime();
        //EnableSaveButton();       
        var $callFrom = $(this);
        event.preventDefault();
        var val = $("#hdfMachineCD").val();

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }

        var nifuda = $("#txtNifudaNo1").val() ? $("#txtNifudaNo1").val().trim() : null;
        var chuuban = $("#txtChuuban2").val() ? $("#txtChuuban2").val().trim() : null;
        var gyouban = $("#txtGyouban2").val() ? $("#txtGyouban2").val().trim() : null;

        var orderNoEntered = (chuuban && gyouban);


        if (nifuda && orderNoEntered) {

            var message = $("#lblErrorMessageE_STA_ENTRY_036").text().trim();
            showMessageDialog(message, "txtNifudaNo1", WARNING);

            return false;
        }

        if ($("#txtNifudaNo1").val().trim()) {
            GetChuubanGyuobanByNifuda($("#txtNifudaNo1").val(), $callFrom)
        }
        else
        {
            ValidateChubban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        }

               
        GetMachineDetails($("#hdfMachineCD").val(), $callFrom);      
    });
    function ValidateChubban(chuuban, gyouban, $callFrom) {

        $callFrom.data('requestRunning', true);
        var ajaxTime = new Date().getTime();
        var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Stampping/ValidateChubban",
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
                    var errorMessage = $("#lblErrorMessageE_STA_ENTRY_004").text();
                    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                    return false;
                }

                else if (result.responce == 2) {
                    DisableStartDataPanel();                    
                    // Chuuban not found in system.
                    var errorMessage = $("#lblErrorMessageE_STA_ENTRY_003").text();
                    showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                    return false;
                }
                else if (result.responce == 3) {
                    DisableStartDataPanel();
                    DisableEndDataPanel();                    
                    // OrNo Not Started FromHandOver
                    var message = $("#lblErrorMessageE_STA_ENTRY_021").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                    showMessageDialog(message, "txtChuuban2", WARNING);
                    // ClearData();
                    return false;
                }
                else if (result.responce == 1) {                    
                    
                        getStampingDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $("#txtNifudaNo1").val(), $callFrom);
                                         

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

                    var errorMessage = $("#lblErrorMessageE_STA_ENTRY_037").text();
                    showMessageDialog(errorMessage, "txtNifudaNo1", WARNING);                   
                    return false;
                }

                // nifuda is marked as completed in the system.
                else if (jsonResult.nifudA_COMPLETE_DATE != null) {
                    var errorMessage = $("#lblErrorMessageE_STA_ENTRY_038").text();
                    showMessageDialog(errorMessage, "txtNifudaNo1", WARNING);                   
                    return false;
                }
                else {

                    $("#txtChuuban2").val(jsonResult.chuuban);
                    $("#txtGyouban2").val(jsonResult.gyouban);
                    ValidateChubban(jsonResult.chuuban, jsonResult.gyouban, $callFrom);
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;
            });
    }

    function getStampingDetailsByChuuban(chuuban, gyouban, mechineCD, nifudaNo, $callFrom) {
        debugger;
        $callFrom.data('requestRunning', true);
        var ajaxTime = new Date().getTime();
        var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "' ,mechineCD : '" + mechineCD + "', USER_NAME : '" + userName + "' }";

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Stampping/GetStampingDetailsByChuuban",
            method: "POST",
            contentType: "application/json; charset=utf-8",
           
            data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, MACHINE_CD: mechineCD, NIFUDA_NO: nifudaNo, CULTURE_NAME: $("#hdfCultureName").val() })
        }

        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                ClearData();
                GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
                EnableStartDataPanel();
                DisableSearchOrderNoPanel();
                SetChuubanDetails(jsonResult);
                SetStartData(jsonResult);
                //SetStamp();
                if ($("#hdfDBOperation").val() == "Insert") {
                    //if (jsonResult.sT_START_DATE_STRING == null && jsonResult.sT_END_DATE_STRING !== null) {
                        
                    //    EnableStartDataPanel();
                       

                    //} else {
                        
                        EnableStartDataPanel();
                        DisableEndDataPanel();       
                        
                        $("#txtBundleNo").removeClass("Mandatory");                        
                        $("#txtBundleNo").removeAttr(DISABLED).removeClass(DISABLED);

                       // ClearPanel($("#pnlProcessStartData"));
                    //ClearPanel($("#pnlProcessEndData"));
                   // $("#txtStamp").val(jsonResult.pP_STAMP);

                    //if (jsonResult.stamP_DETAILS != null) {
                    //    $("#ddlStamp").val(jsonResult.stamP_DETAILS || $("#ddlStamp").prop('selectedIndex'));
                    //    $("#txtStamp").val(jsonResult.stamP_DETAILS);
                    //}
                    //else
                    //{
                    //    $("#txtStamp").val(jsonResult.pP_STAMP);
                    //}
                    
                        $("#txtStartDate").focus();
                   // }
                    
                }
                else if ($("#hdfDBOperation").val() == "Update") {
                    SetStartData(jsonResult);

                    //DisableStartDataPanel();
                    EnableEndDataPanel();
                    $("#txtBundleNo").addClass("Mandatory");                      

                    $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
                    $("#txtEndOperator").val($("#txtStartOperator").val());
                    $("#btnEndDateTime").click(); 
                    $("#txtEndDate").focus();
                    DisableSavedThaba();
                    DisablemandatoryNG();
                    if (parseInt(jsonResult.uniT_TYPE) > 0) {
                        $("#ddlUnitType").val(jsonResult.uniT_TYPE);
                    }
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


    $("#btnSave").click(function (event) {
        
        ResetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

        if (!IsMandatoryFieldsFilled()) {
            return false;
        }
       
         if (IsDuplicateThabaNo()) {
             return false;
         }

        if (!CheckDate($("#txtStartDate"))) {
            var message = $("#lblErrorMessageE_STA_ENTRY_017").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtStartDate", WARNING);
            return false;
        }
       

        if ($("#hdfDBOperation").val() == "Insert") {
            if (!IsDateValid(true)) {
                var message = $("#lblErrorMessageE_STA_ENTRY_022").text().trim() + "(" + $("#lblErrorMessageE_STA_ENTRY_033").text().trim() + commonDateRange + ")";
                var focusFieldID = "txtStartDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
                
            }

            if (($("#txtBundleNo").val() == '')) {
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
                            SaveStampingDetails($callFrom);($callFrom);
                        }
                        else {
                            // return false;

                        }
                    },
                    centerVertical: true
                });

            }
            else {
                SaveStampingDetails($callFrom);
            }
            
        }
        else {
            // Check all error conditions
            if (!IsEndDateValid()) {
                return false;
            }
            if (!CheckDate($("#txtEndDate"))) {
                var message = $("#lblErrorMessageE_STA_ENTRY_018").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, "txtEndDate", WARNING);
                return false;
            }
            if (!IsDateValid(false)) {
                var message = $("#lblErrorMessageE_STA_ENTRY_023").text().trim() + "(" + $("#lblErrorMessageE_STA_ENTRY_033").text().trim() + commonDateRange + ")";

                var focusFieldID = "txtEndDate";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
                
            }
           
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
                            UpdateStampingDetails($callFrom);
                        }
                        else
                        {
                            $("#txtEndDate").focus();
                        }

                    },
                    centerVertical: true
                });

            }
            else {

                UpdateStampingDetails($callFrom);
            }
            

            
        }
    });

    $("#txtNGWeight1").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGWeight2").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGWeight3").keyup(function (event) {
        OKQtyRegionClassChange();
    });
}
    

function EnableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
}

function DisableSaveButton() {
    $("#btnSave").attr("DISABLED", "DISABLED");
}

function DisableNGWeight() {
    $("#txtNgWeight").attr("DISABLED", "DISABLED").addClass(DISABLED);
}


function CheckDate(field) {
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
            else if (regs[4] > 23 || regs[5] > 59)
            {
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

function IsEndDateValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_STA_ENTRY_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_STA_ENTRY_009").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
   

    return true;
}

function ResetResponseTime() {
    responseTime = ""; // Clear server response time
}

//function IsMandatoryChuubanSearchFieldsFilled() {
//    var returnVal = true;  // default true is required for screens with no mandatory fields

//    var message = ""; // This message will be displayed on dialog box
//    var focusFieldID = "";

//    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {
//        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
//            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
//                returnVal = false;
//                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
//                if (focusFieldID === "") {
//                    focusFieldID = $(mandatoryField).attr("id");
//                }
//            }
//        }
//        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
//            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
//                returnVal = false;
//                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
//                if (focusFieldID === "") {
//                    focusFieldID = $(mandatoryField).attr("id");
//                }
//            }
//        }
//    });

//    if (message !== "") {
//        showMessageDialog(message, focusFieldID, WARNING);
//        DisableSaveButton();
//    }
//    return returnVal;
//}
function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;
    var message = "";
    var focusFieldID = "";   

    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {

        // Skip validation for txtChuuban2
        if (mandatoryField.id === "txtNifudaNo1" || mandatoryField.id === "txtChuuban2" || mandatoryField.id === "txtGyouban2") {
            return true; // continue loop
        }

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

        var commonMsg = $("#lblErrorMessageE_STA_ENTRY_035").text().trim();

        message += "<br>" + commonMsg;

        if (focusFieldID === "") {
            focusFieldID = "txtNifudaNo1";
        }

        returnVal = false;
    }



    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
        DisableSaveButton();
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
        $("#dialogMessageIcon img").hide();
        //  $("#imgError").show();

        showMessageDialog(message, focusFieldID, WARNING)
       
    }
    return returnVal;
}
function IsDuplicateThabaNo() {
    
    var inputThaba1 = $("#txtBundleNo").val().trim();


    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK) {

            firstFieldID = "txtBundleNo";
            message = $("#lblErrorMessageE_STA_ENTRY_086").text().trim().replace("NNN", inputThaba1);
            showMessageDialog(message, firstFieldID, WARNING);
            return true;
        }
    }

    return false;
}



function GetThabaDetailsByChuuban(chuuban, gyouban, $callFrom) {   
    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonThabaDetailData + "}"
        data: JSON.stringify({ CHUUBAN: chuuban, GYOUBAN: gyouban })
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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);

        });
}

function SetThabaDetails(jsonResult) {
    console.log(jsonResult);
    ClearThabaDetailsGrid();
    // First, clear thaba details
    $("[id$='lblDecisionNo']").html("");

    // Then, set thaba details
    $.each(jsonResult, function (index, jsonObject) {

        if (jsonObject.tabA_NO) {
            var thabaNumbers = jsonObject.tabA_NO.split(",");
            var decision = jsonObject.decision;

            $.each(thabaNumbers, function (index, thabaNo) {
                thabaNo = thabaNo.trim();
                registeredThabaNumbersAndDecision["" + thabaNo] = decision;

                if (parseInt(thabaNo) >= parseInt(maxRegisteredThabaNo)) {
                    maxRegisteredThabaNo = parseInt(thabaNo);
                }

                var decisionLegend = $(decisionLegends[decision]);
                $("#lblThabaNo" + thabaNo).text(thabaNo);
                $("#lblDecisionNo" + thabaNo).html("").append(decisionLegend);
            });
        }
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


function SetChuubanDetails(jsonResult) {
    
    $("#btnSave").removeAttr("disabled");
    dbOperation = "Insert";
    if ((jsonResult.sT_START_DATE_STRING != null) && (jsonResult.sT_END_DATE_STRING == null) && jsonResult.pasT_DATA_EXIST == "YES") {
        dbOperation = "Update";
    }
    $("#hdfDBOperation").val(dbOperation);

    stamppingCD = $.isNumeric(jsonResult.stampinG_CD) ? parseInt(jsonResult.stampinG_CD) : 0;

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

    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) != null) {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
        $("#txtUnitWtT4").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
    }

    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtColorT4").val(jsonResult.pP_COLOR);

    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMaterialNameMakerT4").val(jsonResult.pP_MATL_NAME_MAK);

    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtConditionT4").val(jsonResult.pP_PRODUCT_CONDITION);

    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlShapeT4").val(jsonResult.pP_MATL_SHAPE);

    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);
    $("#txtMatlDimT4").val(jsonResult.pP_MATL_DIM_D);


    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtChargeT4").val(jsonResult.pP_CHARGE);

    $("#txtHostRefNoT1").val(jsonResult.nyukA_CD);
    $("#txtHostRefNoT4").val(jsonResult.nyukA_CD);

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
  
    if (jsonResult.dW_EC_DONE_CHK == "1") {
        $("#chkECOnline").checked = true;
    }
    else {
        $("#chkECOnline").checked = false;
    }

    // 3rd Tab
    ResetMessageDetailsGrid();
    if (jsonResult.dW_MESSAGE_LIST) {
        SetMessageDetailsGrid(jsonResult.dW_MESSAGE_LIST);
    }

    //4th Tab 

    $("#txtElongation").val(jsonResult.pP_ELONGATION);
    $("#txtTensileStrength").val(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtHardness").text(jsonResult.pP_HARDNESS);
    $("#txtDecarb").val(jsonResult.pP_DECARB);
    $("#txtYieldPoint").val(jsonResult.pP_YEILD);
    $("#txtReduction").val(jsonResult.pP_PRESS);// 

    $("#txtJISTestSpecimen").text(jsonResult.pP_JIS_TEST_SPEC);

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
   

}

function SetStartData(jsonResult) {
    debugger;
    if (jsonResult.sT_START_DATE_STRING && jsonResult.sT_END_DATE_STRING == null && jsonResult.pasT_DATA_EXIST == "YES") {
        $("#txtStartOperator").val(jsonResult.sT_START_OPERATOR_NAME);
        $("#hdfStartOperatorCD").val(jsonResult.sT_START_OPERATOR_CD);
        $("#txtMetalicPltCode").val(jsonResult.sT_METALLIC_PLATE_CODE);
        //$("#ddlStamp option:contains(" + jsonResult.stamP_DETAILS + ")").prop('selected', true);
        //$("#ddlStamp").val(jsonResult.stamP_DETAILS || $("#ddlStamp").prop('selectedIndex'));
        $("#txtStamp").val(jsonResult.stamP_DETAILS);
        $("#ddlTrialResult1").val(jsonResult.sT_TRIAL_RESULT);
        $("#txtBundleNo").val(jsonResult.sT_TABA_NO);
       
       
        EnableEndDataPanel();
        DisableNGWeight();

    }
    //else if (jsonResult.shoW_DEFAULT == 1)//1 = latest Record found in the last 30 days.
    else if (jsonResult.sT_START_DATE_STRING != null && jsonResult.sT_END_DATE_STRING != null && jsonResult.pasT_DATA_EXIST == "YES" && jsonResult.shoW_DEFAULT == 1)
    {
        $("#txtMetalicPltCode").val(jsonResult.sT_METALLIC_PLATE_CODE);
        if (jsonResult.stamP_DETAILS != null) {
           // $("#ddlStamp").val(jsonResult.stamP_DETAILS || $("#ddlStamp").prop('selectedIndex'));
            $("#txtStamp").val(jsonResult.stamP_DETAILS);
        }
        else {
            $("#txtStamp").val(jsonResult.pP_STAMP);
        }
        $("#ddlUnitType").val(jsonResult.uniT_TYPE);

        //$("#ddlStamp").val(jsonResult.stamP_DETAILS || $("#ddlStamp").prop('selectedIndex'));
        // $("#txtStamp").val(jsonResult.stamP_DETAILS);
    }
    else if (jsonResult.pasT_DATA_EXIST == "NO") {
        $("#txtStamp").val(jsonResult.pP_STAMP);
    }

    
    if (dbOperation == "Insert") {
        SetStartSysDateTime();
    }
    else {
        $("#txtStartDate").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        //$("#txtStartTime").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    }

}

function EnableSearchPanel() {
    $("#txtMachineType").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtNifudaNo1").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtChuuban2").removeAttr(DISABLED).removeClass(DISABLED);
    $("#txtGyouban2").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnChuubanSearch").removeAttr(DISABLED).removeClass(DISABLED);
}

function DisableSearchPanel() {
    $("#txtMachineType").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtNifudaNo1").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtChuuban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtGyouban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#btnChuubanSearch").attr(DISABLED, DISABLED).addClass(DISABLED);
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
        }
    });

    $.each($startDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });
    //$("#txtStamp").css({ display: 'revert' });
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
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });
    $("#txtNgWeight").attr("DISABLED", "DISABLED").addClass("DISABLED");

    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
   
    //$('#ddlUnitType option:eq(0)').prop('selected', true);
   
}

function SetStartDataPanelDefaultData(jsonResult) {

    if (jsonResult.DW_START_DATE_STRING != null) {
        var startDateTime = jsonResult.DW_START_DATE_STRING;
        $("#txtStartDate").val(new Date(startDateTime).format("yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        //$("#txtStartTime").val(new Date(startDateTime).format("HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    }
    else {
        $("#btnEndDateTime").click();
    }
}

function SetProcessDefectList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL+ "/Stampping/GetProcessDefect",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindProcessDefectDropdown(jsonResult) {
    $("#ddlDefects1").html("").append($('<option></option>'));
    $("#ddlDefects2").html("").append($('<option></option>'));
    $("#ddlDefects3").html("").append($('<option></option>'));
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefects1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
            $("#ddlDefects2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
            $("#ddlDefects3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefects1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlDefects2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlDefects3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }

    //$.each(jsonResult, function (key, jsonObj) {
    //    $("#ddlDefects1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    //    $("#ddlDefects2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    //    $("#ddlDefects3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    //});
}


function SaveStampingDetails($callFrom) {
    
    var ajaxTime = new Date().getDate();

    var jsonDataForSave = JSON.stringify({
        PP_CHUUBAN: $("#txtChuuban2").val(),
        PP_GYOUBAN: $("#txtGyouban2").val(),
        STAMPING_CD: stamppingCD,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        MACHINE_CD: $("#hdfMachineCD").val(),        
        ST_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ST_START_OPERATOR_CD: $("#hdfStartOperatorCD").val(),
       // ST_START_OPERATOR_CD: $("#txtStartOperator").val(),        
        ST_METALLIC_PLATE_CODE: $("#txtMetalicPltCode").val(),
        //STAMP_DETAILS:  $('#ddlStamp :selected').text(),
        STAMP_DETAILS: $("#txtStamp").val() ? $("#txtStamp").val() : null,
        ST_TRIAL_RESULT: $("#ddlTrialResult1 option:selected").val(),
        ST_TABA_NO: $("#txtBundleNo").val() ? $("#txtBundleNo").val() : null,
        USER_ID: $("#hdfLoginId").val(),
        //USER_NAME: $("#txtChuuban2").val(),
    });
  
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL+ "/Stampping/StamppingDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            /* After StartData is saved sucessfully, enable EndData Panel for input, 
              and show success message. */
          
            var successMessage = $("#lblSuccessMessageS_HIKI_ENTRY_001").text();

            showMessageDialog(successMessage, "txtEndDate", SUCCESS);
            //DisableStartDataPanel();
            EnableEndDataPanel();
            $("#txtBundleNo").addClass("Mandatory");  

            var jsonResult = response.message;

            SetChuubanDetails(jsonResult);
            SetStartData(jsonResult);
            

            DisablemandatoryNG();
            DisableSavedThaba();
            GetMachineDetails($("#hdfMachineCD").val(), $callFrom);
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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function UpdateStampingDetails($callFrom) {
    
    var ajaxTime = new Date().getDate();

    var jsonDataForUpdate = JSON.stringify({
       
        STAMPING_CD: stamppingCD ? stamppingCD : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val():null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val():null,
        ST_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ST_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val():null,
        ST_METALLIC_PLATE_CODE: $("#txtMetalicPltCode").val() ? $("#txtMetalicPltCode").val():null,
        ///STAMP_DETAILS: $("#ddlStamp :selected").text() ? $("#ddlStamp :selected").text() : null,
        STAMP_DETAILS: $("#txtStamp").val() ? $("#txtStamp").val() : null,
        ST_TRIAL_RESULT: $("#ddlTrialResult1 option:selected").val() ? $("#ddlTrialResult1 option:selected").val():null,
        ST_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ST_END_OPERATOR_CD: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
        ST_DECISION: $("#ddlTrialResult2 option:selected").val() ? $("#ddlTrialResult2 option:selected").val() : null,
        ST_NG_QTY: $("#txtNgWeight").val() ? $("#txtNgWeight").val() : null,
        ST_OK_QTY: $("#txtOKQty").val() ? $("#txtOKQty").val() : null,
        ST_NG_BARS1: $("#txtNGWeight1").val() ? $("#txtNGWeight1").val() : null,
        ST_NG_BARS2: $("#txtNGWeight2").val() ? $("#txtNGWeight2").val() : null,
        ST_NG_BARS3: $("#txtNGWeight3").val() ? $("#txtNGWeight3").val() : null,
        ST_NG_DETAILS1: $("#ddlDefects1 option:selected").val() ? $("#ddlDefects1 option:selected").val() : null,
        ST_NG_DETAILS2: $("#ddlDefects2 option:selected").val() ? $("#ddlDefects2 option:selected").val() : null,
        ST_NG_DETAILS3: $("#ddlDefects3 option:selected").val() ? $("#ddlDefects3 option:selected").val() : null,
        ST_TABA_NO: $("#txtBundleNo").val() ? $("#txtBundleNo").val() : null,
        UNIT_TYPE: $("#ddlUnitType option:selected").val() ? $("#ddlUnitType option:selected").val() : null,
        ST_NEXT_PROCESS: $("#ddlNextProcess option:selected").val() ? $("#ddlNextProcess option:selected").val() : null,
        ST_OK_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val():null, 
        ST_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        USER_ID: $("#hdfLoginId").val(),
       USER_NAME: userName ? userName : null,
    });

    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL+ "/Stampping/StamppingDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
        data: jsonDataForUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            

            var successMessage = $("#lblSuccessMessageS_HIKI_ENTRY_001").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
                    DisableNGWeight();                                       
                    DisableEndDataPanel();
                    DisableStartDataPanel();
                    DisableSearchPanel();
                    DisableSaveButton();
                    GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
                    GetMachineDetails($("#hdfMachineCD").val(), $callFrom);
               

            })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function DisableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "none");
}
function EnableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
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

function SetDecimalFormats() {
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtOKQty, #txtNgWeight").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
}

function SetStamp() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetStamp",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindStampDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindStampDropdown(jsonResult) {
    $("#ddlStamp").html("");
   // $("#ddlStamp").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlStamp").append($('<option value="' + jsonObj.name + '"> ' + jsonObj.name + '</option>'));
        if ($("#txtStamp").val() == "") {
            $("#txtStamp").val(jsonObj.name);
        }
    });
}

function BinddecisionDropdown(jsonResult) {
    //$("#ddlTrialResult2").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTrialResult2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setDecision() {
    
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Stampping/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BinddecisionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindTrialResultDropdown(jsonResult) {
    //$("#ddlTrialResult1").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTrialResult1").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function SetTrialResult() {
    
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetTrialResult",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (respose) {
            responseTime = new Date().getTime() - ajaxTime;
            BindTrialResultDropdown(respose.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
    //$("#ddlUnitType").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function SetUnitType() {
    
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Stampping/GetUnitType",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindNextProcessDropdown(jsonResult) {

   /* $("#ddlNextProcess").html("").append($('<option value="0"></option>'));*/
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

$(document).ready(function () {

    DisablemandatoryNG();
    
    $.support.cors = true; // Special case for IE7. To get past weird errors.
   
    
    if (!$("#txtChuuban2").hasClass("Mandatory")) {
        $("#txtChuuban2").addClass("Mandatory");
    }
    if (!$("#txtGyouban2").hasClass("Mandatory")) {
        $("#txtGyouban2").addClass("Mandatory");
    }
    DisableNGWeight();
    SetNextProcess();
    SetGlobalVariables();
    BindEvents();
    SetDecimalFormats();
    //SetStamp();
    setDecision();
    SetTrialResult();
    SetUnitType();
    SetProcessDefectList();
    DisableStartDataPanel();
    DisableEndDataPanel();
    resetMachineDetailsGrid();

    $(".FocusDefault").focus();
    userName = $("#hdfUserName").val();
});

function DisablemandatoryNG() {
    $("#ddlDefects1").attr("disabled", "disabled");
    $("#ddlDefects2").attr("disabled", "disabled");
    $("#ddlDefects3").attr("disabled", "disabled");

    $("#ddlDefects1").removeClass("Mandatory");
    $("#ddlDefects2").removeClass("Mandatory");
    $("#ddlDefects3").removeClass("Mandatory");
}
   




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
        url: APIURL + "/Stampping/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });




    return returnval;
}
$("#ddlNextProcess").focusout(function () {
    $("#btnSave").focus();
}); 
$("#ddlTrialResult1").focusout(function () {
    $("#btnSave").focus();
}); 
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
            url: APIURL + "/Stampping/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});
$('#txtNifudaNo1').keypress(function (e) {
    var nifudaNo = $('#txtNifudaNo1').val() ? $('#txtNifudaNo1').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && nifudaNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Stampping/MappNifudaByBarcode?nifudaNo=" + nifudaNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
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

    if (parseInt($("#txtBundleNo").val()) > 0) {
        $("#txtBundleNo").attr(DISABLED, DISABLED).addClass(DISABLED);
    }
      
}
