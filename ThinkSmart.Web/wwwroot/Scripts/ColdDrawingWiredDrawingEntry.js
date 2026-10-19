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

userName = ""; // assigned at Document.ready

// Following decision legends are to show decision colors on Top Thaba Grid.
var decisionLegends = {
    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>"
}

var machineGridDataTr1 = " <tr id='tr1ItemGrdvColdDrawingWireDrawingMachine'> "
    + " <td class='td tdH2' rowspan='2'> "
    + " <span class='txtColor' style='background-color:COLOR; height:50px; width:6px; display: inline-block;'></span>"
    + " </td>"
   
    + " <td class='td tdH2 GridAlignmentChuuban'  style='padding-right: 0px; padding-left: 0px;'>"
    + " <span class='txtGridChuban labelClass' style='font-size: 12.2px;'>valtxtGridChuban</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentCustomer padddingLeft' rowspan='2'  style='white-space:nowrap;text-align:center' >"
    + " <span class='txtGridCustomerName labelClass'> valtxtGridCustomerName</span>"
    + " </td>"
  
    + " <td class='td tdH2 GridAlignmentProductNameCust padddingLeft' rowspan='2'  style='white-space:nowrap;text-align:center'>"
    + " <span class='txtProdNameCust labelClass' >valtxtProdNameCust</span></td>"

    + " <td class='td tdH2 GridAlignmentCharge padddingLeft' rowspan='2' style='white-space:nowrap;text-align:center'>"
    + " <span class='txtGridCharge labelClass' >valtxtGrid_PROD_DIM_D</span></td>"
    
    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2'  style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_LEN_TOL</span></td>"

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2'  style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtMaterialDimD</span></td>"
  

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2'  style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_Charge</span></td>"

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2'  style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_Die_Dim</span></td>"


    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2' colspan='2'style='white-space:nowrap;text-align:center' >"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_W_Shape</span></td>"

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2' style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxt_Ext_condition</span></td>"

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2' style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_MAGNETIC_INSPECTIONS</span></td>"

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2' style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxt_TABA_NO_1</span></td>"

    + " <td class='td tdH2 GridAlignmentDimD' rowspan='2' style='white-space:nowrap;text-align:center'>"
    + " <span class='txtMaterialDimD labelClass' >valtxtGrid_Remarks</span></td>"

    + "  </tr>";

var machineGridDataTr2 = "<tr id='tr2ItemGrdvColdDrawingWireDrawingMachine'>"
    + "  <td class='td tdH2 GridAlignmentProdSpecNo'> "
    + "<span class='txtGridSpecification labelClass' >valtxtGridSpecification</span> "
    + "</td> "

    + " </tr> ";



function initialize() {
    
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

    //hideToleranceErrorLabels();
    resetGlobalVariables();
    //resetMessageDetailsGrid();
}

function Mask() {
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

    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
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
        $('<td class="GridAlignmentRemarks GridRemarks200PX">').text("")
    );
    $grdvMessage.append($tr);
}

function setMessageDetailsGrid(messageDetailsList) {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add data rows
    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
        if (($hdfCultureLoaded.val()).indexOf($hdfCultureEN.val()) > -1) // > -1 means substring is present
        {
            processName = jsonObject.procesS_NAME_EN;
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
    $.each($pnlThabaDetails.find("span[id^='lblThabaNo']"), function (index, span) {
        $(span).html("");
    });
    $.each($pnlThabaDetails.find("span[id^='lblDecisionNo']"), function (index, span) {
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
        url: "/ColdDrawingWIREDRAWING/GetSysDateTime",
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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
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
        url: "/ColdDrawingWIREDRAWING/GetSysDateTime",
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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function getMachineNamebyCD($target) {    
    var ajaxTime = new Date().getTime();
    var machineCD = $target.val().trim().replace(/\D/g, ""); // this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits. 
    if (machineCD == "") {
        $('#txtMachineType').val("");
        return false;
    }
    var jsonData = "{machineCD: '" + machineCD + "', USER_NAME : '" + userName + "'}"


    if (machineCD.length > 4) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }


    var settings = {
        async: true,
        crossDomain: true,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetMachineNamebyCD?machineCD=" + parseInt(machineCD),
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
                var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
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

function getMachineDetails(machineCd, $callFrom) {
   
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
        url: "/ColdDrawingWIREDRAWING/GetMachineDetails?machineCD=" + machineCd,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            console.log(jsonResult);
            Set24HrMachineData(jsonResult);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function Set24HrMachineData(jsonResult) {
    
    clearTable($("#grdvColdDrawingWireDrawingMachine"), "ItemGrdvColdDrawingWireDrawingMachine", "");
     
    $.each(jsonResult, function (index, jsonObject) {

      
        
        
           
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
                else if (jsonObject.stdecision == 4) {
                    firstRow = firstRow.replace("COLOR", "Yellow");  //firstRow = $("●").css('color', 'Yellow');
                    firstRow = firstRow.replace("#E2E2E2", "Yellow");
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


            if (jsonObject.pP_PROD_DIM_D != null)
                firstRow = firstRow.replace("valtxtGrid_PROD_DIM_D", jsonObject.pP_PROD_DIM_D);
            else
                firstRow = firstRow.replace("valtxtGrid_PROD_DIM_D", "");


            if (jsonObject.pP_MATL_DIM_D != null)
                firstRow = firstRow.replace("valtxtMaterialDimD", jsonObject.pP_MATL_DIM_D);
            else
                firstRow = firstRow.replace("valtxtMaterialDimD", "");

            if (jsonObject.pP_LEN_TOL != null)
                firstRow = firstRow.replace("valtxtGrid_LEN_TOL", jsonObject.pP_LEN_TOL);
            else
                firstRow = firstRow.replace("valtxtGrid_LEN_TOL", "");

            if (jsonObject.pP_CHARGE != null)
                firstRow = firstRow.replace("valtxtGrid_Charge", jsonObject.pP_CHARGE);
            else
                firstRow = firstRow.replace("valtxtGrid_Charge", "");

            if (jsonObject.pP_MATL_DIM_D_SHAPE != null)
                firstRow = firstRow.replace("valtxtGrid_Die_Dim", jsonObject.pP_MATL_DIM_D_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGrid_Die_Dim", "");

        if (jsonObject.dW_WINDING_SHAPE1 != null)
                
            firstRow = firstRow.replace("valtxtGrid_W_Shape", jsonObject.dW_WINDING_SHAPE1);
               
            else
                firstRow = firstRow.replace("valtxtGrid_W_Shape","");

            if (jsonObject.dW_DW_EXT_TEXTURE_STATUS != null)
                firstRow = firstRow.replace("valtxt_Ext_condition", jsonObject.dW_DW_EXT_TEXTURE_STATUS);
            else
                firstRow = firstRow.replace("valtxt_Ext_condition", "");
         
            if (jsonObject.dW_DW_MAGNETIC_INSPECTIONS != null)
                firstRow = firstRow.replace("valtxtGrid_MAGNETIC_INSPECTIONS", jsonObject.dW_DW_MAGNETIC_INSPECTIONS);
            else
                firstRow = firstRow.replace("valtxtGrid_MAGNETIC_INSPECTIONS", "");

            if (jsonObject.dW_DW_TABA_NO_1 != null)
                firstRow = firstRow.replace("valtxt_TABA_NO_1", jsonObject.dW_DW_TABA_NO_1);
            else
                firstRow = firstRow.replace("valtxt_TABA_NO_1", "");

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
    var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorCD == "") {

        var message = $("#lblErrorMessageE_CoreDrawingWireDrawing_ENTRY_006").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    var jsonData = "{OperatorCD: '" + operatorCD + "', USER_NAME : '" + userName + "'}"

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var operatorName = response.message;
            
            if (operatorName == null || operatorName == "") {
                var message = $("#lblErrorMessageE_CoreDrawingWireDrawing_ENTRY_006").text().trim();
                showMessageDialog(message, $target.attr("id"), WARNING);
                $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
            }

            else {
                $target.val(operatorName);
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

function bindEvents() {
    $("#btnInitialize").click(function (event) {
        initialize();
    });
   
    $('#txtMachineType').change(function () {
        $("#hdfMachineCD").val($(this).val().trim().replace(/\D/g, ""));// this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits. 
        getMachineNamebyCD($(this));
        setNextProcess();
        //setExteriorConditionKubunGet();
        //setMagneticDust();
        //setDiceInspEnd();
        //setDiceInspStart();
        getMachineDetails($("#hdfMachineCD").val());
    });

    $("#txtStartOperator").change(function () {
        $("#hdfStartOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
        getOperatorNameByCD($(this));
    });

    $("#txtEndOperator").change(function () {
        $("#hdfEndOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
        getOperatorNameByCD($(this));
    });

    $("#btnStartDateTime").click(function (event) {
        setStartSysDateTime();
    });
    $("#btnEndDateTime").click(function (event) {
        setEndSysDateTime();
    });

    $("#btnChuubanSearch").click(function (event) {
        
        alert("hi");
        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

       // if (!IsMandatoryChuubanSearchFieldsFilled()) {
          //  return false;
      //  }
        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }
        getColdDrawingWireDrawingDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfMachineCD").val(), $callFrom);
      getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        getMachineDetails($("#hdfMachineCD").val(), $callFrom);
        getDrawingStartdetails($("#txtChuuban2").val(), $("#txtGyouban2").val(), $("#hdfldProcessPlanCd").val(), $callFrom);
    });

    $("#btnSave").click(function (event) {
       
        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();
        
        if (!IsMandatoryFieldsFilled()) {
            return false;
        }
        if ($("#hdfDBOperation").val() == "") {
            var message = $("#lblErrorMessageE_HIKI_ENTRY_002").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }
     
        /* if (IsDuplicateThabaNo()) {
             return false;
         }*/

        //if (!checkDate($("#txtStartDate"))) {
        //    var message = $("#lblErrorMessageE_STA_ENTRY_017").text().trim(); // This message will be displayed on dialog box
        //    showMessageDialog(message, $("#txtStartDate"), WARNING);
        //    return false;
        //  }
        
        
        if ($("#hdfDBOperation").val() == "Insert") {

           
            SaveColdDrawingWireDrawingDetails($callFrom);
            
        }
        else {
            
            // Check all error conditions
            if (!IsEndDataValid()) {
               
                return false;
            }
            
            if (!checkDate($("#txtEndDate"))) {
               
                var message = $("#lblErrorMessageE_ColdDrawingWireDrawing_ENTRY_EndDate").text().trim(); // This message will be displayed on dialog box
                showMessageDialog(message, $("#txtEndDate"), WARNING);
                return false;
               
            }
          
            UpdateColdDrawingWireDrawingDetails($callFrom);
        }
    });
}

function enableSaveButton() {
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
}

function disableSaveButton() {
    $("#btnSave").attr(DISABLED, DISABLED).addClass(DISABLED);
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
            if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {

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

function IsEndDataValid() {
   
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_HIKI_ENTRY_009").text().trim();
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
/*
function IsDuplicateThabaNo() {

    var inputThaba1 = $("#txtBundleNo").val().trim();
    
    var message = ""; // This message will be displayed on dialog box
    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    // Check if in input thaba textboxes, same thaba no is entered more than once
    if (($.inArray(inputThaba1) > -1) && inputThaba1 != "") {
        focusFieldID = "txtThaba1";
        message = $("#lblErrorMessageE_HIKI_ENTRY_006").text().trim();
        showMessageDialog(message, focusFieldID, WARNING);
        return true;
    }

    // Check if entered thaba number is already registered and its combined decision is OK
    if (inputThaba1 in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba1]) == combinedDecisionOK && $('#chkRestraighting').prop("checked") == false) {
            focusFieldID = "txtBundleNo";
            message = $("#lblErrorMessageE_HIKI_ENTRY_019").text().trim().replace("NNNN", inputThaba1);
            showMessageDialog(message, focusFieldID, WARNING);
            return true;
        }
    }
    return false;
}

*/
function getDrawingStartdetails(chuuban, gyouban, mechineCD, $callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "' ,mechineCD : '" + mechineCD + "', USER_NAME : '" + userName + "' }";
 
    var settings = {
        
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetDrawingStartDetails",
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
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
        url: "/ColdDrawingWIREDRAWING/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlNextProcess").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindExteriorConditionDropdown(jsonResult) {
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMDustDropdown(jsonResult) {
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindDiceStartdropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDiceInspectionStart").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindDiceEnddropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDiceInspectionEnd").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
//function setStamp() {
//    var ajaxTime = new Date().getTime();
//    var jsonData = "{USER_NAME: '" + userName + "' }";
//    var settings = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: "/ColdDrawingWIREDRAWING/GetStamp",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonData + "}"
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            bindStampDropdown(response.message);
//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}

function bindStampDropdown(jsonResult) {
    //  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlStamp").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
}


function getColdDrawingWireDrawingDetailsByChuuban(chuuban, gyouban, mechineCD, $callFrom) {
    
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "' ,mechineCD : '" + mechineCD + "', USER_NAME : '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetColdDrawingWireDrawingDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, mechineCD: mechineCD })
    }

    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            console.log(jsonResult);

            if (jsonResult === null) {
                // Chuuban not found in system.
                              
                var errorMessage = $("#lblErrorMessageE_STA_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
          

            if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
                //    // Chuuban is marked as completed in the system/
              
                var errorMessage = $("#lblErrorMessageE_Cold_Drawing_004").text();
                showMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
           
           
            setChuubanDetails(jsonResult);
            setStartData(jsonResult);
         //   setProcessDefectList();
            

           
          //  setStamp();
            if ($("#hdfDBOperation").val() == "Insert") {
                if (jsonResult.dW_END_DATE_STRING == null && jsonResult.dW_START_DATE_STRING !== null) {
                disableStartDataPanel();
                    enableEndDataPanel();
                   
                } else {
                    enableStartDataPanel();
                    disableEndDataPanel();
                   
                  


                }
              

                //setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed before disableEndDataPanel(). It is because setChuubanDimLenToleranceMessages() will enable some fields based on values of Dim and Len Tolerances, and disableEndDataPanel() will disable fields again. 
            } else if ($("#hdfDBOperation").val() == "Update") {
                setStartData(jsonResult);
                    // enableStartDataPanel(jsonResult);
                    disableStartDataPanel();
                enableEndDataPanel();
                // Set End Operator same as Start Operator
               
                $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
                $("#btnEndDateTime").click(); // Get current time for end operation.
                $("#txtEndOperator").focus();
                //setChuubanDimLenToleranceMessages(); // Note: This function call is deliberately placed after enableEndDataPanel(). It is because enableEndDataPanel() enables all fields, and setChuubanDimLenToleranceMessages() will again disable some fields based on values of Dim and Len Tolerances.
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

function getThabaDetailsByChuuban(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetThabaDetailsByChuuban",
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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);

        });
}

function setThabaDetails(jsonResult) {
    console.log(jsonResult);
 clearThabaDetailsGrid();
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

    $("#txtFinalRegisteredThabaNoValue").text(maxRegisteredThabaNo);
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

    // For setting chkActualLength, we need to check both StdLength and LengthTolerance
    if (((lenTolMin == null || lenTolMin == "" || Math.abs(parseFloat(lenTolMin)) == parseFloat(0))
        && (lenTolMax == null || lenTolMax == "" || Math.abs(parseFloat(lenTolMax)) == parseFloat(0)))
        ||
        (lenStd == null || lenStd == "" || Math.abs(parseFloat(lenStd)) == parseFloat(0))) {
        $("#chkActualLength").prop('checked', false);
        $("#chkActualLength").attr(DISABLED, DISABLED).addClass(DISABLED);
        toggleActualLengthFieldsMandatory($("#chkActualLength"));
    }
    else {
        $("#chkActualLength").prop('checked', true);
        $("#chkActualLength").removeClass(DISABLED).removeAttr(DISABLED);
        toggleActualLengthFieldsMandatory($("#chkActualLength"));
    }
}

//function showMessageDialog(message, focusFieldID, messageType) {

//    $("#dialogMessageIcon img").hide();
//    switch (messageType) {
//        case WARNING:
//            $("#imgWarning").show();
//            break;
//        case ERROR:
//            $("#imgError").show();
//            break;
//        case SUCCESS:
//            $("#imgSuccess").show();
//            break;
//        default:
//            break;
//    };

//    $("#dialogMessageText").html("").append($("<p>" + message + "<p>"));
//    $("#dialog").dialog({
//        modal: true,
//        title: document.title,
//        buttons: {
//            Ok: function () {
//                $(this).dialog("close");
//                if (focusFieldID !== "") {
//                    $("#" + focusFieldID).focus();
//                }
//            }
//        },
//        minWidth: 550,
//        width: 550
//    });
//}

function showMessageDialog(message, focusFieldID, messageType) {
    $("#dialogMessageIcon img").hide();
    switch (messageType) {
        case WARNING:
            bootbox.alert({
                title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                message: message,
                centerVertical: true
            });
            break;
        case ERROR:
            bootbox.alert({
                title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Error",
                message: message,
                centerVertical: true
            });
            break;
        case SUCCESS:
            bootbox.alert({
                title: "<i class='fas fa-check-circle' aria-hidden='true'></i> " + "Success",
                message: message,
                centerVertical: true
            });
            break;
        default:
            break;
    };

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
    //    },
    //    minWidth: 550,
    //    width: 550
    //});
}

function setChuubanDetails(jsonResult) {

   
    dbOperation = "Insert";
    if ((jsonResult.dW_START_DATE_STRING != null) && (jsonResult.dW_END_DATE_STRING == null)) {
        dbOperation = "Update";
    }
    $("#hdfDBOperation").val(dbOperation);

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);

    DrawingCD = $.isNumeric(jsonResult.drawinG_CD) ? parseInt(jsonResult.drawinG_CD) : 0;

    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);

    $("#txtChuubanT1").text(jsonResult.pP_CHUUBAN);
    $("#txtChuubanT4").text(jsonResult.pP_CHUUBAN);
    $("#txtDeliveryDateT1").text(jsonResult.pP_DELIVERY_DATE_STRING);

      $("#txtCustomerInchargeT1").text(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtCustomerInchargeT4").text(jsonResult.pP_EMPLOYEE_NAME);

    $("#txtStdDeliveryDateT1").text(jsonResult.pP_STD_DEL_PERIOD);
    $("#txtStdDeliveryDateT4").text(jsonResult.pP_STD_DEL_PERIOD);

    $("#txtCustomerT1").text(jsonResult.pP_CUSTOMER_NAME);
    $("#txtCustomerT4").text(jsonResult.pP_CUSTOMER_NAME);

    $("#txtReceiverT1").text(jsonResult.pP_RECEIVER_NAME);
    $("#txtReceiverT4").text(jsonResult.pP_RECEIVER_NAME);

    $("#txtChamferT1").text(jsonResult.pP_CHAMFER);
    $("#txtChamferT4").text(jsonResult.pP_CHAMFER);

    $("#txtUnitWtT1").text(jsonResult.pP_UNIT_WT);
    $("#txtUnitWtT4").text(jsonResult.pP_UNIT_WT);

    $("#txtColorT1").text(jsonResult.pP_COLOR);
    $("#txtColorT4").text(jsonResult.pP_COLOR);

    $("#txtMaterialNameMakerT1").text(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMaterialNameMakerT4").text(jsonResult.pP_MATL_NAME_MAK);

    $("#txtMatlShapeT1").text(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlShapeT4").text(jsonResult.PP_MATL_SHAPE);

    $("#txtMatlDimT1").text(jsonResult.pP_MATL_DIM_D);
    $("#txtMatlDimT4").text(jsonResult.pP_MATL_DIM_D);


    $("#txtChargeT1").text(jsonResult.pP_CHARGE);
    $("#txtChargeT4").text(jsonResult.pP_CHARGE);

    $("#txtBendStandardT1").text(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtBendStandardT4").text(jsonResult.pP_MAGARI_BEND_TOL);

    $("#txtProdNameCustomerT1").text(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdNameCustomerT4").text(jsonResult.pP_PROD_NAME_CUST);

    $("#txtProdShapeT1").text(jsonResult.pP_PROD_SHAPE);
    $("#txtProdShapeT4").text(jsonResult.pP_PROD_SHAPE);

    $("#txtProdDimT1").text(jsonResult.pP_PROD_DIM_D);
    $("#txtProdDimT4").text(jsonResult.pP_PROD_DIM_D);

    $("#txtProdDimOtherT1").text(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtProdDimOtherT4").text(jsonResult.pP_PROD_DIM_OTHER);

    $("#txtExtTextureT1").text(jsonResult.pP_EXT_TEXTURE);
    $("#txtExtTextureT4").text(jsonResult.pP_EXT_TEXTURE);

    $("#txtDimToleranceT1").text(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDimToleranceT4").text(jsonResult.pP_DIM_TOLERANCE);
   
    if (jsonResult.pP_DIM_TOL_MIN != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    if (jsonResult.pP_DIM_TOL_MAX != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    dimStd = jsonResult.pP_PROD_DIM_D;

    $("#txtDisplayT1").text(jsonResult.pP_DISPLAY);
    $("#txtDisplayT4").text(jsonResult.pP_DISPLAY);

    $("#txtStampT1").text(jsonResult.pP_STAMP);
    $("#txtStampT4").text(jsonResult.pP_STAMP);

    $("#txtLengthT1").text(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthT4").text(jsonResult.pP_PROD_LENGTH_STRING);

    $("#txtLengthToleranceT1").text(jsonResult.pP_LEN_TOL);
    $("#txtLengthToleranceT4").text(jsonResult.pP_LEN_TOL);

    if (jsonResult.pP_LENGTH_TOL_MIN != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    if (jsonResult.pP_LENGTH_TOL_MAX != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.

    lenStd = jsonResult.pP_PROD_LENGTH;

    $("#txtPackingT1").text(jsonResult.pP_PACKING_DECODE);
    $("#txtPackingT4").text(jsonResult.pP_PACKING_DECODE);

    $("#txtNoOfUnitsT1").text(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtNoOfUnitsT4").text(jsonResult.pP_NO_OF_UNITS_DECODE);

    $("#txtQuantityT1").text(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtQuantityT4").text(jsonResult.pP_ACT_ALLOC_QTY);

    $("#txtIntDiaMinT1").text(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtIntDiaMinT4").text(jsonResult.pP_COIL_DIA_INTERNAL);

    $("#txtExtDiaMaxT1").text(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtExtDiaMaxT4").text(jsonResult.pP_COIL_DIA_EXTERNAL);

    $("#txtWindingDirectionT1").text(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtWindingDirectionT4").text(jsonResult.pP_COIL_DIA_WINDING);

    $("#txtKakouiraiRemarksT1").text(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtKakouiraiRemarksT4").text(jsonResult.pP_KAKOUIRAI_REMARKS);

    $("#txtProdOrderRemarkT1").text(jsonResult.pP_PROCESS_PLAN_REMARK);
    $("#txtProdOrderRemarkT4").text(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtProductionProcessT1").text(jsonResult.pP_PRODUCTION_PROCESS);

    $("#txtHostRefNoT1").val(jsonResult.producT_NYUKO_CD);
    $("#txtHostRefNoT4").val(jsonResult.producT_NYUKO_CD)

    //if (jsonResult.dW_EC_DONE_CHK == "1") {
    //    $("#chkECOnline").checked = true;
    //}
    //else {
    //    $("#chkECOnline").checked = false;
    //}

    // 3rd Tab
    resetMessageDetailsGrid();
    if (jsonResult.dW_MESSAGE_LIST) {
        setMessageDetailsGrid(jsonResult.dW_MESSAGE_LIST);
    }

    //4th Tab 

    $("#txtElongation").text(jsonResult.pP_ELONGATION);
    $("#txtTensileStrength").text(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtHardness").text(jsonResult.pP_HARDNESS);
    $("#txtDecarb").text(jsonResult.pP_DECARB);
    $("#txtYieldPoint").text(jsonResult.pP_YEILD);
    $("#txtReduction").text(jsonResult.pP_PRESS);// 

    $("#txtJISTestSpecimen").text(jsonResult.pP_JIS_TEST_SPEC);

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
   
    //$("#txtStartOperator").val(jsonResult.DW_START_OPERATOR_NAME); textbox will blank if removed comment
    if (jsonResult.dW_END_DATE_STRING == null && jsonResult.dW_START_DATE_STRING !== null) {
        $("#txtStartDate").val(jsonResult.dW_START_DATE_STRING); 
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
        } else {

            $("#chkmiddle").prop('checked', true);
        }

        if (jsonResult.dW_REDRAWING == 0) {
            $("#chkReDrawing").prop('checked', false);
        } else {

            $("#chkReDrawing").prop('checked', true);
        }

      
       
    }
   
    $("#ddlTPTest").val(parseInt(jsonResult.DW_TP_TEST) || $("#ddlTPTest").prop('selectedIndex')); // Note : This logic is very crucial . Either set the data from DB. If data is not in DB then set what is default selected index on HTML.
  //  $("#txtFurnaceNum").val(jsonResult.DW_FURNACE_NO);
    $("#txtNoOfCoils").val(jsonResult.DW_NO_OF_COILS);
    $("#txtCoilQuantity").val(jsonResult.DW_QTY);
    
    $("#txtC").val(jsonResult.DW_CARBON_PERCENTAGE);
    $("#txtMn").val(jsonResult.DW_MAGNIZ_PERCENTAGE);

    
    //$("#ddlDieShape").val(parseInt(jsonResult.DW_DIE_SHAPE) || $("#ddlDieShape").prop('selectedIndex'));  // Note : This logic is very crucial . Either set the data from DB. If data is not in DB then set what is default selected index on HTML.

  
 


    $("#txtDrawDim1").val(jsonResult.DW_DIM1);
    $("#txtDrawDim2").val(jsonResult.DW_DIM2);

    $("#txtUpperLowerRollerAngle1").val(jsonResult.DW_UPPER_LOWER_ROLLER_ANGLE1);
    $("#txtUpperLowerRollerAngle2").val(jsonResult.DW_UPPER_LOWER_ROLLER_ANGLE2);

    $("#txtUpperRoleElevation").val(jsonResult.DW_UPPER_ROLLER_ELEVATION);

    $("#txtCompressionTonnage").val(jsonResult.DW_TONNAGE);

    $("#txtHexR").val(jsonResult.DW_HEX_R);

    //if ($.isNumeric(jsonResult.DW_EC_DONE_CHK) && parseInt(jsonResult.DW_EC_DONE_CHK) == 1) {
    //    $("#chkECOnline").prop('checked', true);
    //}
    //else {
    //    $("#chkECOnline").prop('checked', false);
    //}

    $("#txtShikii").val(jsonResult.DW_SHIKII_VALUE);
    $("#txtInspectionCoilSize").val(jsonResult.DW_EC_COIL_SIZE);
    $("#txtInspectionCoilShape").val(jsonResult.DW_EC_COIL_SHAPE);
    $("#txtIsOK").val(jsonResult.DW_EC_IS_OK);

    txtShikii = jsonResult.DW_SHIKII_VALUE;
    txtInspectionCoilSize = jsonResult.DW_EC_COIL_SIZE;
    txtInspectionCoilShape = jsonResult.DW_EC_COIL_SHAPE;
    txtIsOK = jsonResult.DW_EC_IS_OK;

    if (dbOperation == "Insert") {
        setStartSysDateTime();
    }
    else {
       // $("#txtStartDate").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
       // $("#txtStartTime").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
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

    //if ($.isNumeric(jsonResult.DW_EC_DONE_CHK)) {
    //    if (parseInt(jsonResult.DW_EC_DONE_CHK) == 1) {
    //        $("#chkECOnline").prop('checked', true);
    //    }
    //    else {
    //        $("#chkECOnline").prop('checked', false);
    //    }
    //}
    //toggleECOnlineFieldsMandatory($("#chkECOnline"));
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

    // Set default values

    /* $('#ddlUnitType option:eq(0)').prop('selected', true);
    toggleUnitTypeFieldsEnabled($("#ddlUnitType"));*/

    $("#txtEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtEndTime").mask("99:99", { placeholder: "hh:mm" });

    $('#ddlUnitType option:eq(0)').prop('selected', true);
    //$('#ddlBendStatus option:eq(0)').prop('selected', true);
    // $('#ddlTPTest option:eq(0)').prop('selected', true);
    //  $('#ddlExternalSurfaceCondition option:eq(0)').prop('selected', true);
}

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

//function setProcessDefectList() {
//    var ajaxTime = new Date().getTime();
//    var jsonData = "{USER_NAME: '" + userName + "' }";
//    var settings = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: "/ColdDrawingWIREDRAWING/GetProcessDefect",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonData + "}"
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;

//            bindProcessDefectDropdown(response.message);

//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}

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
       
        PP_CHUUBAN: $("#txtChuuban2").val(),
        PP_GYOUBAN: $("#txtGyouban2").val(),
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        MACHINE_CD: $("#hdfMachineCD").val(),
        DW_START_DATE: $("#txtStartDate").val(),
        DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val(),
        ROLLING_NUMBER: $("#txtRollingnumber").val(),
        FURNACE_NO: $("#txtFurnaceNum").val(),
      
        DIE_DIM: $("#txtDieDim").val(),
        DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        DIE_NO: $("#txtDieNo").val(),
      DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
      //  WINDING_SHAPE: $("#ddlWindingshape option:selected").val() ? $("#ddlWindingshape option:selected").val() : null,
        WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0), 
      
        REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),
       
        USER_ID: $("#hdfLoginId").val()
       

       
     
    });
    
      var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonDataForSave + "}"
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            enableEndDataPanel();
            disableStartDataPanel()
            var successMessage = $("#lblSuccessMessageS_ColdDrawingWireDrawing_ENTRY_001").text();
            showMessageDialog(successMessage, "txtEndOperator", SUCCESS);
            var jsonResult = response.message;

            //setChuubanDetails(jsonResult);
            //setStartData(jsonResult);
            DrawingCD = $.isNumeric(jsonResult.drawinG_CD) ? parseInt(jsonResult.drawinG_CD) : 0;

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
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function UpdateColdDrawingWireDrawingDetails($callFrom) {
    
    var ajaxTime = new Date().getDate();

    var jsonDataForUpdate = JSON.stringify({

       PP_CHUUBAN: $("#txtChuuban2").val(),
       PP_GYOUBAN: $("#txtGyouban2").val(),
        DRAWING_CD: DrawingCD,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val(),
        MACHINE_CD: $("#hdfMachineCD").val(),
        DW_START_DATE: $("#txtStartDate").val(),
        DW_START_OPERATOR_CD: $("#hdfStartOperatorCD").val(),
        ROLLING_NUMBER: $("#txtRollingnumber").val(),
        FURNACE_NO: $("#txtFurnaceNum").val(),
        MSG_CD: parseInt($("#hfld_MessageCD").val()),
        DIE_DIM: $("#txtDieDim").val(),
        DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        DIE_NO: $("#txtDieNo").val(),
        DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),


        DW_END_DATE: $("#txtEndDate").val(),
        DW_END_OPERATOR_CD: $("#hdfEndOperatorCD").val(),
        DW_ACT_DIM_CHK: ($("#chkActualLength").is(':checked') ? 1 : 0),
        DW_ACT_DIM_START: $("#txtActualDieDimStart").val(),
        DW_ACT_DIM_END: $("#txtActualDieDimEnd").val(),
        DW_FINISHED_COILS: $("#txtNoOfUnits").val(),
      
        DW_FINISHED_QTY: $("#txtWeight").val(),
        DW_TABA_NO_1: $("#txtThaba1").val(),
        DW_TABA_NO_2: $("#txtThaba2").val(),
        DW_TABA_NO_3: $("#txtThaba3").val(),
        DW_TABA_NO_4: $("#txtThaba4").val(),
        DW_EXT_TEXTURE_STATUS: $("#ddlExternalSurfaceCondition option:selected").val(),
        DW_MAGNETIC_INSPECTIONS: $("#ddldustdetection option:selected").val(),
        DIE_CHK_END: $("#ddlDiceInspectionEnd option:selected").val(),
        DW_DECISION: $("#ddlDecision option:selected").val(),
        DW_NEXT_PROCESS: $("#ddlNextProcess option:selected").val(),
       
      DW_REMARKS: $("#txtRemarks").val(),
        USER_ID: $("#hdfLoginId").val()
       // USER_NAME: $("#txtChuuban2").val(),
    });
  
   
    var settings = {
        
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataForUpdate
    }
    
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            //var jsonResult = $.parseJSON(response.d);

            //$("#dialogMessageIcon img").hide();
            //$("#imgSuccess").show();

            var successMessage = $("#lblSuccessMessageS_ColdDrawingWireDrawing_ENTRY_001").text();
         //   showMessageDialog(successMessage, "txtEndOperator", SUCCESS);
            
            bootbox.alert({
                title: "<i class='fas fa-check-circle' aria-hidden='true'></i> " + "Success",
                message: successMessage,
                centerVertical: true,
                callback: function () {
                    disableStartDataPanel();
                    disableEndDataPanel();
                    disableSearchPanel();
                    disableSaveButton();
                    getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
                    getMachineDetails($("#hdfMachineCD").val(), $callFrom);
                }
            });

            //$("#dialogMessageText").html("").append($("<p>" + successMessage + "<p>"));
            //$("#dialog").dialog({
            //    modal: true,
            //    buttons: {
            //        Ok: function () {
            //            disableStartDataPanel();
            //            disableEndDataPanel();
            //            disableSearchPanel();
            //            disableSaveButton();
            //            getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
                   getMachineDetails($("#hdfMachineCD").val(), $callFrom);
            //            $(this).dialog("close");
            //        }
            //    }
            //});
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
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
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtNGQty, #txtNgWeight").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
}

$(document).ready(function () {

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // window.moveTo(0, 0);
    //window.resize(1920, 1030); /*window.resize(1920, 1080);*/

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

    $(".FocusDefault").focus();
    userName = $("#hdfUserName").val();
});
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           