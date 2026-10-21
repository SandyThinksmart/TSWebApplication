var $btnChuubanSearch;
var $btnSave;
var $btnSearchT2;
var maxRegisteredThabaNo = 0;
var $hdfCultureLoaded;
var $hdfCultureEN;
var $hdfCultureJP;
var registeredThabaNumbersAndDecision = {};
var combinedDecisionOK = "";
var isOperatorValid = true;
userName = "";
let enterPressedMachine = false;
let enterPressedStartOpr = false;
let enterPressedEndOpr = false;
var machineModified = false;
var operatorStartModified = false;
var operatorEndModified = false;

var DISABLED = "disabled";
var HIDDEN = "Hidden"
var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";

var machineGridDataTr1 = " <tr id='tr1ItemGrdvHikinukiMachine'> "
    + " <td class='td tdH2' rowspan='2' style='padding-left:revert;background-color:white;'> "
    + " <span class='txtColor' style='background-color:COLOR; height:35px; width:100%; display: list-item;'></span>"
    + " </td>"
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
    + " <td class='td tdH2 GridAlignmentNumeric' >"
    + " <span class='txtGridDimDShape labelClass padding24Machine' >valtxtGridDimDShape</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric' rowspan='2'>"
    + " <span class='txtGridNoOfCoils labelClass padding24Machine' >valtxtGridNoOfCoils</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric' rowspan='2'>"
    + " <span class='txtGridQuantity labelClass padding24Machine' >valtxtGridQuantity</span>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric' rowspan='2'>"
    + "  <span class='txtGridTabaNo labelClass padding24Machine' >valtxtGridTabaNo</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric' rowspan='2'>"
    + "  <span class='txtGridTabaNo labelClass padding24Machine' >valtxtOKBars</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric'  rowspan='2'>"
    + "  <span id='txtOKQty' class='txtGridTabaNo labelClass padding24Machine' >valtxtOKQty</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric'  rowspan='2' >"
    + "  <span id='txtNGBars'  class='txtGridTabaNo labelClass padding24Machine' >valtxtNGBars</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric'  rowspan='2' colspan='2'>"
    + "  <span id='txtProcessDefect' class='txtGridTabaNo labelClass padding24Machine' >valtxtProcessDefect</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric' >"
    + "  <span id='txtSensation' class='GridAlignmentNumeric labelClass padding24Machine' >valtxtSensation</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric'>"
    + "  <span id='txtLine' class='txtGridTabaNo labelClass padding24Machine' >valtxtLine</span> <br>"
    + " </td>"
    + " <td class='td tdH2 GridAlignmentNumeric'>"
    + "  <span id='txtMagnetism' class='txtGridTabaNo labelClass padding24Machine' >valtxtMagnetism</span> <br>"
    + " </td>"
    + " </tr>  ";

var machineGridDataTr2 = "<tr id='tr2ItemGrdvHikinukiMachine'>"
    + "  <td class='td tdH2 GridAlignmentProdSpecNo'> "
    + "<span id ='txtGridSpecification' class='txtGridSpecification labelClass padding24Machine' >valtxtGridSpecification</span> "
    + "</td> "
    + "<td class='td tdH2 GridAlignmentCharge'> "
    + "<span id='txtGridCharge' class='txtGridCharge labelClass padding24Machine' >valtxtGridCharge</span> "
    + "</td> "
    + "<td class='td tdH2 GridAlignmentLength'> "
    + "<span id='txtGridLength' class='txtGridLength labelClass padding24Machine' >valtxtGridLength</span> "
    + "</td> "
    + "<td class='td tdH2 GridAlignmentRemarks' colspan='3' rowspan='1'> "
    + "<span id='txtGridMsgContent' class='txtGridMsgContent labelClass padding24Machine' >valtxtGridMsgContent</span> "
    + "</td> "
    + " </tr> ";

var processHistoryGridataRow1 = " <tr id='tr1ItemGrdvProcessHistory'> "
    + " <td class= 'td GridLeft'> "
    + " <span id= 'lblProcLocation' class= 'labelClass' style= 'color:Black;white-space:nowrap;'>valtxtProcLocation</span> "
    + " </td> "
    + " <td class= 'td GridCenter'> "
    + " <span id= 'lblStartDate' class= 'labelClass' style= 'color:Black;white-space:nowrap;'>valtxtStartDate</span> "
    + " </td> "
    + " <td class= 'td GridCenter'> "
    + " <span id= 'lblEndDate' class= 'labelClass' style= 'color:Black;white-space:nowrap;'>valtxtEndDate</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='3'> "
    + " <span id= 'lblCustomerName' class= 'labelClass' style= 'color:Black;white-space:nowrap;'>valtxtCustomerName</span> "
    + " </td> "
    + " <td class= 'td GridCenter'> "
    + " <span id= 'lblChuuban' class= 'labelClass' style= 'color:Black;white-space:nowrap;'>valtxtChuuban</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='3'> "
    + " <span id= 'lblChuuban' class= 'labelClass' style= 'color:Black;white-space:nowrap;'>valtxtProdNameCust</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan='2'> "
    + "     <span id= 'lblMetalicPlate' class= 'labelClass' style= 'color:Black; '>valtxtMetalicPlate</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblProdDimD' class= 'labelClass' style= 'color:Black; '>valtxtProdDimD</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='3'> "
    + "     <span id= 'lblNoOfUnits' class= 'labelClass' style= 'color:Black; '>valtxtNoOfUnits</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='3'> "
    + "     <span id= 'lblActAllocQty' class= 'labelClass' style= 'color:Black; '>valtxtActAllocQty</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='3'> "
    + "     <span id= 'lblThabaNo' class= 'labelClass' style= 'color:Black; '>valtxtThabaNo</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='3'> "
    + "     <span id= 'lblOkBars' class= 'labelClass' style= 'color:Black; '>valtxtOkBars</span> "
    + " </td> "
    + " <td class= 'td GridRight' rowspan='3'> "
    + "     <span id= 'lblOkQty' class= 'labelClass' style= 'color:Black; '>valtxtOkQty</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblNGBars1' class= 'labelClass' style= 'color:Black; '>valtxtNGBars1</span> "
    + " </td> "
    + " <td class= 'td GridLeft'> "
    + "     <span id= 'lblNGDetails1' class= 'labelClass' style= 'color:Black; '>valtxtNGDetails1</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblIsOk' class= 'labelClass' style= 'color:Black; '>valtxtIsOk</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblLineSpeed' class= 'labelClass' style= 'color:Black; '>valtxtLineSpeed</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblMagnetism' class= 'labelClass' style= 'color:Black; '>valtxtMagnetism</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='3'> "
    + "     <span id= 'lblDecision' class= 'labelClass' style= 'color:Black; '>valtxtDecision</span> "
    + " </td> "
    + " <td class= 'td GridCenter' rowspan='3'> "
    + "     <span id= 'lblNextProcess' class= 'labelClass' style= 'color:Black; '>valtxtNextProcess</span> "
    + " </td> "
    + " </tr>  ";






var processHistoryGridataRow2 = " <tr id='tr2ItemGrdvProcessHistory'> "
    + " <td class= 'td GridLeft' colspan='2' rowspan= '2'> "
    + "     <span id= 'lblMachineName' class= 'labelClass' style= 'color:Black; '>valtxtMachineName</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan= '2'> "
    + "     <span id= 'lblEndOperator' class= 'labelClass' style= 'color:Black; '>valtxtEndOperator</span> "
    + " </td> "
    + " <td class= 'td GridCenter'  rowspan= '2'> "
    + "     <span id= 'lblProdSpecNo' class= 'labelClass' style= 'color:Black; '>valtxtProdSpecNo</span> "
    + " </td> "
    + " <td class= 'td GridRight'  rowspan= '2'> "
    + "     <span id= 'lblProdLength' class= 'labelClass' style= 'color:Black; '>valtxtProdLength</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblNGBars2' class= 'labelClass' style= 'color:Black; '>valtxtNGBars2</span> "
    + " </td> "
    + " <td class= 'td GridLeft'> "
    + "     <span id= 'lblNGDetails2' class= 'labelClass' style= 'color:Black; '>valtxtNGDetails2</span> "
    + " </td> "
    + " <td class= 'td GridLeft' rowspan= '2' colspan='3'> "
    + "     <span id= 'lblRemark' class= 'labelClass' style= 'color:Black; '>valtxtRemark</span> "
    + " </td> "
    + " </tr>  ";


var processHistoryGridataRow3 = " <tr id='tr3ItemGrdvProcessHistory' > "
    + " <td class= 'td GridLeft'> "
    + "     <span id= 'lblCharge' class= 'labelClass' style= 'color:Black; '>valtxtCharge</span> "
    + " </td> "
    + " <td class= 'td GridRight'> "
    + "     <span id= 'lblNGBars3' class= 'labelClass' style= 'color:Black; '>valtxtNGBars3</span> "
    + " </td> "
    + " <td class= 'td GridLeft'> "
    + "     <span id= 'lblNGDetails3' class= 'labelClass' style= 'color:Black; '>valtxtNGDetails3</span> "
    + " </td> "
    + " </tr>  ";
function SetDecimalFormats() {
    $("#txtNoOfCoils").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });

    $("#txtCoilQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });

    $("#txtCoilSize").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });

    $("#txtNGBarsTotal").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });

    $("#txtOKBars").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });


    $("#txtNGWt").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });

    $("#EC_NG_QTY, #txtOKQty").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });

    $("#txtActualLength").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

    $("#txtActDim1 #txtActDim2 #txtActDim3 #txtActDim4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtMagnetism").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });
    $("#txtActDim1, #txtActDim2, #txtActDim3, #txtActDim4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
}

$(document).ready(function () {
    $("#24HrsMachineData").css({ 'height': '845px' });
    Reset24HrsMachineGrid();
    ResetECHistoryGrid();
    DisableSaveButton();
    SetDecimalFormats();

    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableFIDataPanel();
    window.resize(1920, 1030);
    $(".FocusDefault").focus();
    $("#txtChuuban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtGyouban2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtNifudaNo1").addClass("Mandatory");

});

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

function ClearTable($table, trId, trClass) {
    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
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
    if (keycode == 13 && machine && machineModified == true) {
        Reset24HrsMachineGrid();
        if ($('#txtMachineType').val() == "" || $('#txtMachineType').val() == null) {
            ClearTable($("#grdECMachine"), "ItemGrdvHikinukiMachine", "");
            var message = $("#lblErrorMessageE_EC_ENTRY_023").text().trim();
            ShowMessageDialog(message, "txtMachineType", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfMachineCD").val(machineNumber);
            enterPressedMachine = true;
            // $("#hdfMachineCD").val($(this).val().trim().replace(/\D/g, ""));// this works for MAC20, 20MAC, mac20, 20mac. . /\D/g is a regular expression meaning non-digits. 
            GetMachineNamebyCD($(this));
            $("#txtNifudaNo1").focus();
        }
    }
    if (e.type === 'focusout' && machine && machineModified == true) {
        if (enterPressedMachine) {

            enterPressedMachine = false; // Reset the flag
            return false;
        }
        Reset24HrsMachineGrid();
        var machineCD = $(this).val().trim();
        var machineName = machineCD.substring(0, 3);
        var machineNumber = machineCD.substring(3, 7);
        $("#hdfMachineCD").val(machineNumber);
        GetMachineNamebyCD($(this));
        $("#txtNifudaNo1").focus();
    }
    enterPressedMachine = false; // Reset the flag

});
function GetMachineNamebyCD($target) {

    var ajaxTime = new Date().getTime();
    var machineCD = $target.val().trim().replace(/\D/g, "");

    var ajaxTime = new Date().getTime();
    var machineNumber = $target.val().trim();
    var machineval = machineNumber.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineCD = machineNumber.substring(3, 7);


    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_EC_ENTRY_031").text().trim().replace("*****", $("#tdLblMachineType").text());

        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
    if (machineCD == "") {
        var message = $("#lblErrorMessageE_EC_ENTRY_023").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $('#txtMachineType').val("");
        return false;
    }
   
    if (machineCD.length > 4) {
        var message = $("#lblErrorMessageE_EC_ENTRY_013").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }


    var settings = {
        async: false,
        crossDomain: true,
        dataType: "json",
        url: APIURL + "/EC/GetMachineNamebyCD?machineCD=" + parseInt(machineCD),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var machineName = response.message;

            if (machineName.key == null || machineName.key == "") {
                var message = $("#lblErrorMessageE_EC_ENTRY_032").text().trim().replace("*****", $("#tdLblMachineType").text());
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            } else if (machineName.value == "inactive")        //machine is inactive.
            {
                var message = $("#lblErrorMessageE_EC_ENTRY_020").text().trim();
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }
            else {
                $target.val(machineName.key);
                GetMachineDetails($("#hdfMachineCD").val());
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function GetMachineDetails(machineCd) {

  
    var ajaxTime = new Date().getTime();

    var jsonData = "{machineCd: '" + machineCd + "'}"

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetMachineDetails?machineCd=" + machineCd + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
          
            if (jsonResult.length > 0) {
               
                Set24HrMachineData(jsonResult);
            } else {
                Reset24HrsMachineGrid();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function Set24HrMachineData(jsonResult) {
   
    ClearTable($("#grdECMachine"), "ItemGrdvHikinukiMachine", "");
    ClearTable($("#grdECMachine"), "", "GridNormalRow");

    $.each(jsonResult, function (index, jsonObject) {
        if (jsonObject.machinE_CD) {
            var firstRow = machineGridDataTr1;
            if (jsonObject.eC_DECISION != null) {
                if (jsonObject.eC_DECISION == 1) {
                    firstRow = firstRow.replace("COLOR", "green");    // $("●").css('color', 'green');             
                    firstRow = firstRow.replace("#E2E2E2", "green");
                }
                else if (jsonObject.eC_DECISION == 2) {
                    firstRow = firstRow.replace("COLOR", "red");    //firstRow = $("●").css('color', 'red');
                    firstRow = firstRow.replace("#E2E2E2", "red");
                }
                else if (jsonObject.eC_DECISION == 3) {
                    firstRow = firstRow.replace("COLOR", "blue");    //firstRow = $("●").css('color', 'blue');
                    firstRow = firstRow.replace("#E2E2E2", "blue");
                }
                else if (jsonObject.eC_DECISION == 4) {
                    firstRow = firstRow.replace("COLOR", "Yellow");  //firstRow = $("●").css('color', 'Yellow');
                    firstRow = firstRow.replace("#E2E2E2", "Yellow");
                }
                else if (jsonObject.eC_DECISION == 5) {
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

            if (jsonObject.pP_CHUUBAN != null)
                firstRow = firstRow.replace("valtxtGridChuban", jsonObject.pP_CHUUBAN);
            else
                firstRow = firstRow.replace("valtxtGridChuban", "");

            if (jsonObject.pP_PROD_NAME_CUST != null)
                firstRow = firstRow.replace("valtxtProdNameCust", jsonObject.pP_PROD_NAME_CUST);
            else
                firstRow = firstRow.replace("valtxtProdNameCust", "");

            if (jsonObject.eC_METALLIC_PLATE_CODE != null)
                firstRow = firstRow.replace("valtxtMetallicPlateCode", jsonObject.eC_METALLIC_PLATE_CODE);
            else
                firstRow = firstRow.replace("valtxtMetallicPlateCode", "");


            if (jsonObject.pP_PROD_DIMD_SHAPE != null)
                firstRow = firstRow.replace("valtxtGridDimDShape", jsonObject.pP_PROD_DIMD_SHAPE);
            else
                firstRow = firstRow.replace("valtxtGridDimDShape", "");

            if (jsonObject.pP_NO_OF_UNITS_STRING != null)
                firstRow = firstRow.replace("valtxtGridNoOfCoils", jsonObject.pP_NO_OF_UNITS_STRING);
            else
                firstRow = firstRow.replace("valtxtGridNoOfCoils", "");

            if (jsonObject.pP_ACT_ALLOC_QTY != null)
                firstRow = firstRow.replace("valtxtGridQuantity", jsonObject.pP_ACT_ALLOC_QTY);
            else
                firstRow = firstRow.replace("valtxtGridQuantity", "");

            if (jsonObject.eC_TABA_NO != null)
                firstRow = firstRow.replace("valtxtGridTabaNo", jsonObject.eC_TABA_NO);
            else
                firstRow = firstRow.replace("valtxtGridTabaNo", "");

            if (jsonObject.eC_OK_BARS_STRING != null)
                firstRow = firstRow.replace("valtxtOKBars", jsonObject.eC_OK_BARS_STRING);
            else
                firstRow = firstRow.replace("valtxtOKBars", "");

            if (jsonObject.eC_OK_QTY != null)
                firstRow = firstRow.replace("valtxtOKQty", jsonObject.eC_OK_QTY);
            else
                firstRow = firstRow.replace("valtxtOKQty", "");

            if (jsonObject.eC_NG_BAR_TOTAL != null)
                firstRow = firstRow.replace("valtxtNGBars", jsonObject.eC_NG_BAR_TOTAL);
            else
                firstRow = firstRow.replace("valtxtNGBars", "");
          
            //if (jsonObject.eC_NG_PERCENTAGE != null)
            //    firstRow = firstRow.replace("valtxtProcessDefect", jsonObject.eC_NG_PERCENTAGE);
            //else
            //    firstRow = firstRow.replace("valtxtProcessDefect", "");

            var ngDetails = "";
         
            if (jsonObject.eC_NG_BARS1 != null) {
                if (jsonObject.eC_NG_DETAILS1_STRING != null) 
                   ngDetails = jsonObject.eC_NG_BARS1 + " (" + jsonObject.eC_NG_DETAILS1_STRING + ")";
               else
                   ngDetails = jsonObject.eC_NG_BARS1;
            }

            if (jsonObject.eC_NG_BARS2 != null) {

                if (ngDetails == "" && ngDetails != 0) {
                    // ngDetails = jsonObject.eC_NG_BARS2 + " (" + jsonObject.eC_NG_DETAILS2_STRING + ")";
                    if (jsonObject.eC_NG_DETAILS2_STRING != null)
                        ngDetails = jsonObject.eC_NG_BARS2 + " (" + jsonObject.eC_NG_DETAILS2_STRING + ")";
                    else
                        ngDetails = jsonObject.eC_NG_BARS2;
                } else {
                    if (jsonObject.eC_NG_DETAILS2_STRING != null)
                        ngDetails += " " + jsonObject.eC_NG_BARS2 + " (" + jsonObject.eC_NG_DETAILS2_STRING + ")";
                    else
                        ngDetails += " " + jsonObject.eC_NG_BARS2 ;
                   // ngDetails += " " + jsonObject.eC_NG_BARS2 + " (" + jsonObject.eC_NG_DETAILS2_STRING + ")";
                }
            }

            if (jsonObject.eC_NG_BARS3 != null) {

                if (ngDetails == "" && ngDetails != 0) {
                    // ngDetails = jsonObject.eC_NG_BARS3 + " (" + jsonObject.eC_NG_DETAILS3_STRING + ")";
                    if (jsonObject.eC_NG_DETAILS3_STRING != null)
                        ngDetails = jsonObject.eC_NG_BARS3 + " (" + jsonObject.eC_NG_DETAILS3_STRING + ")";
                    else
                        ngDetails = jsonObject.eC_NG_BARS3;
                } else {
                    if (jsonObject.eC_NG_DETAILS3_STRING != null)
                        ngDetails += " " + jsonObject.eC_NG_BARS3 + " (" + jsonObject.eC_NG_DETAILS3_STRING + ")";
                    else
                     ngDetails += " " + jsonObject.eC_NG_BARS3;
                   // ngDetails += " " + jsonObject.eC_NG_BARS3 + " (" + jsonObject.eC_NG_DETAILS3_STRING + ")";
                }


            }
           
            if (ngDetails != "")
                firstRow = firstRow.replace("valtxtProcessDefect", ngDetails);
            else
                firstRow = firstRow.replace("valtxtProcessDefect", "");

            if (jsonObject.iS_OK != null)
                firstRow = firstRow.replace("valtxtSensation", jsonObject.iS_OK);
            else
                firstRow = firstRow.replace("valtxtSensation", "");

            if (jsonObject.linE_SPEED != null)
                firstRow = firstRow.replace("valtxtLine", jsonObject.linE_SPEED);
            else
                firstRow = firstRow.replace("valtxtLine", "");

            if (jsonObject.magnetism != null)
                firstRow = firstRow.replace("valtxtMagnetism", jsonObject.magnetism);
            else
                firstRow = firstRow.replace("valtxtMagnetism", "");

            $("#grdECMachine").append($(firstRow));

            // tr2    (Second row)

            var secondtRow = machineGridDataTr2;

            if (jsonObject.pP_PROD_SPEC_NO != null)
                secondtRow = secondtRow.replace("valtxtGridSpecification", jsonObject.pP_PROD_SPEC_NO);
            else
                secondtRow = secondtRow.replace("valtxtGridSpecification", "");

            if (jsonObject.pP_CHARGE != null)
                secondtRow = secondtRow.replace("valtxtGridCharge", jsonObject.pP_CHARGE);
            else
                secondtRow = secondtRow.replace("valtxtGridCharge", "");

            if (jsonObject.pP_PROD_LENGTH_OTHER != null)
                secondtRow = secondtRow.replace("valtxtGridLength", jsonObject.pP_PROD_LENGTH_OTHER);
            else
                secondtRow = secondtRow.replace("valtxtGridLength", "");

            if (jsonObject.eC_REMARKS != null)
                secondtRow = secondtRow.replace("valtxtGridMsgContent", jsonObject.eC_REMARKS);
            else
                secondtRow = secondtRow.replace("valtxtGridMsgContent", "");

            $("#grdECMachine").append($(secondtRow));
        }
    });

}
$('#txtStartOperator')
    .on('focus', function () {
        operatorStartModified = false;
    })
    .on('input', function () {
        operatorStartModified = true;
    });

$('#txtStartOperator').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);
    if (keycode == 13 && operator && operatorStartModified == true) {
        
            $("#hdfStartOperatorCD").val(operatorNumber);
            enterPressedStartOpr = true;
            //  $("#hdfStartOperatorCD").val($(this).val().trim().replace(/\D/g, ""));
            GetOperatorNameByCD($(this));
            $("#txtTabaNo").focus();
        
    }
    if (e.type === 'focusout' && operator && operatorStartModified == true) {
        if (enterPressedStartOpr) {

            enterPressedStartOpr = false; // Reset the flag
            return false;
        }
        $("#hdfStartOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtTabaNo").focus();
    }
    enterPressedStartOpr = false; // Reset the flag
});


function GetOperatorNameByCD($target) {

    var ajaxTime = new Date().getTime();

    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_EC_ENTRY_031").text().trim().replace("*****", $("#lblStartOperator").text());

        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_EC_ENTRY_015").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageE_EC_ENTRY_015").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
  
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;


            var operator = response.message;

            if (operator == null || operator == "") {
                var message = $("#lblErrorMessageE_EC_ENTRY_032").text().trim().replace("*****", $("#lblStartOperator").text());
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }
            //employee is inactive.
            else if (operator.iN_ACTIVE_FLAG == 1) {

                var message = $("#lblErrorMessageE_EC_ENTRY_029").text().trim();
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }

            else {
                $target.val(operator.yobina);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        })
}
function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#tblPnlSearch .Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === TYPETEXT && !($(mandatoryField).hasClass(DISABLED))) {
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
function IsMandatoryFieldsFilled() {
    var returnVal = true;
    var message = "";
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
        
        if (focusFieldID == "checkDetectionCoil")
        {
            if ($("#txtCoilSize").val() == "") {
                focusFieldID = "txtCoilSize";
            } else {
                focusFieldID = "txtCoilShape"; 
            }

        }
        ShowMessageDialog(message, focusFieldID, WARNING)

    }

    return returnVal;
}


function ResetGlobalVariables() {
    $("#hdfDBOperation").val("");
    registeredThabaNumbersAndDecision = {};
    maxRegisteredThabaNo = 0;
}

function DisableSaveButton() {
    
    $("#btnSave").attr("disabled", "disabled");
}
function EnableSaveButton() {
    $("#btnSave").removeAttr("disabled");
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
}

function DisableEndDataPanel() {
    // Set EndData Panel Elements to Disabled
    var $endDataPanel = $("#pnlProcessEndData");
    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("select"), function (index, dropdown) {

        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
}


function DisableFIDataPanel() {
    // Set FI Data Panel Elements to Disabled
    var $fiDataPanel = $("#pnlFIData");
    $.each($fiDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });

    $.each($fiDataPanel.find("select"), function (index, dropdown) {
        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });

    $.each($fiDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });

    $.each($fiDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
   
}


function EnableFIDataPanel() {

    var $startDataPanel = $("#pnlFIData");

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
    // Fllowing is to clear hidden fields
    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
    });
    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}
function DisableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "none");
}
function EnableSearchOrderNoPanel() {
    $("#pnlSearch").css("pointer-events", "auto");
}
$("#btnChuubanSearch").click(function (event) {
    ClearData();
    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableFIDataPanel();

    if (!IsMandatoryChuubanSearchFieldsFilled()) {
        return false;
    }
   
    GetECDetailsByChuuban($("#txtNifudaNo1").val(), $("#hdfMachineCD").val());
    
});

function GetECDetailsByChuuban(nifuda, mechineCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetECDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ NIFUDA_NO: nifuda, MACHINE_CD: mechineCD, CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            
            // nifuda no not found in system.
            if (jsonResult === null) {
               
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_034").text();
                ShowMessageDialog(errorMessage, "txtNifudaNo1", WARNING);
              //  initialize();
                return false;
            }

            // nifuda is marked as completed in the system.
            if (jsonResult.nifudA_COMPLETE_DATE != null) {
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_021").text();
                ShowMessageDialog(errorMessage, "txtNifudaNo1", WARNING);
              //  initialize();
                return false;
            }

            if (jsonResult.pP_PROCESS_PLAN_CD === null) {
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_004").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

              // OrNo Not Started FromHandOver
            if (jsonResult.pP_PROCESS_PLAN_PRINT_DATE_STR == null) {
                var message = $("#lblErrorMessageE_EC_ENTRY_033").text().trim().replace("99-999999-99", jsonResult.pP_CHUUBAN_GYOBAN); 
                ShowMessageDialog(message, "", WARNING);
                // ClearData();
                return false;
            }
            DisableSearchOrderNoPanel();
            EnableSaveButton();
            EnableStartDataPanel();
            $("#txtStartOperator").focus();
            SetStartSysDateTime();
            SetChuubanDetails(jsonResult);
            GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val());
            BindTPTestList();
            SetUnit();
            
            if (jsonResult.eC_START_DATE_STRING != null && jsonResult.eC_END_DATE_STRING == null && jsonResult.pasT_DATA_EXIST == "YES") {

                SetStartData(jsonResult);
                EnableEndDataPanel();

                SetNextProcess();
                SetFINextProcess();
                SetDecision();
                SetFIDropdown();
                SetProcessDefectList();
                DisableDefaultField();
                SetEndSysDateTime();
                //  EnableFIDataPanel();
                DisableFIDefaultField();
                if (parseInt(jsonResult.uniT_TYPE) > 0)
                {
                    $("#ddlUnit").val(jsonResult.uniT_TYPE);
                }
                $("#txtEndDate").focus();
            } else if (jsonResult.eC_START_DATE_STRING != null && jsonResult.eC_END_DATE_STRING != null && jsonResult.pasT_DATA_EXIST == "YES")
            {
                if (parseInt(jsonResult.uniT_TYPE) > 0) {
                    $("#ddlUnit").val(jsonResult.uniT_TYPE);
                }

                if (parseInt(jsonResult.tP_TEST) > 0)
                {
                  $("#ddlTPTest").val(parseInt(jsonResult.tP_TEST));
                }
                
                $("#txtMetalicPltNo").val(jsonResult.eC_METALLIC_PLATE_CODE);
                $("#txtNoOfCoils").val(jsonResult.acT_ALLOC_UNITS);
                $("#txtCoilQuantity").val(jsonResult.acT_ALLOC_QTY);
                $("#txtIsOK").val(jsonResult.iS_OK);
                $("#txtCoilSize").val(jsonResult.eC_COIL_SIZE);
                $("#txtCoilShape").val(jsonResult.eC_COIL_SHAPE);
                $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
                $("#txtShikii").val(jsonResult.shikiI_VALUE);
                    $("#txtLineSpeed").val(jsonResult.linE_SPEED);

            }
            if (parseInt($("#txtTabaNo").val()) > 0) {
                $("#txtTabaNo").attr(DISABLED, DISABLED).addClass(DISABLED);
            }
            //setNextProcess();


            //if ($("#hdfDBOperation").val() == "Insert") {
            //    enableStartDataPanel(jsonResult);

            //    clearPanel($("#pnlProcessEndData"));
            //    disableEndDataPanel();
            //    disableFIDetailsDataPanel();
            //    disableFIEntryDataPanel();
            //    $("#txtStartDate").focus();
            //}
            //else if ($("#hdfDBOperation").val() == "Update") {

            //    enableStartDataPanel(jsonResult);
            //    enableEndDataPanel();
            //    enableFUDataPanel();

            //    $("#txtEndOperator").val($("#txtStartOperator").val());
            //    $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());

            //    $("#btnEndDateTime").click();
            //    $("#txtEndDate").focus();
            //}
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function SetStartData(jsonResult) {

    $("#txtStartDate").val($.format.date(new Date(jsonResult.eC_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    $("#txtStartOperator").val(jsonResult.eC_START_OPERATOR_NAME);
    $("#hdfStartOperatorCD").val(jsonResult.eC_START_OPERATOR_CD);

    $("#txtEndOperator").val(jsonResult.eC_START_OPERATOR_NAME);
    $("#hdfEndOperatorCD").val(jsonResult.eC_START_OPERATOR_CD);

    $("#txtTabaNo").val(jsonResult.eC_TABA_NO);
    $("#ddlTPTest").val(parseInt(jsonResult.tP_TEST) || $("#ddlTPTest").prop('selectedIndex'));
    $("#txtMetalicPltNo").val(jsonResult.eC_METALLIC_PLATE_CODE);
    $("#txtNoOfCoils").val(jsonResult.acT_ALLOC_UNITS);
    $("#txtCoilQuantity").val(jsonResult.acT_ALLOC_QTY);
    $("#txtIsOK").val(jsonResult.iS_OK);
    $("#txtCoilSize").val(jsonResult.eC_COIL_SIZE);
    $("#txtCoilShape").val(jsonResult.eC_COIL_SHAPE);
    $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
    $("#txtShikii").val(jsonResult.shikiI_VALUE);
    $("#txtLineSpeed").val(jsonResult.linE_SPEED);
    $("#txtActDim1").val(jsonResult.eC_ACT_DIM1);
    $("#txtActDim2").val(jsonResult.eC_ACT_DIM2);
    $("#txtActDim3").val(jsonResult.eC_ACT_DIM3);
    $("#txtActDim4").val(jsonResult.eC_ACT_DIM4);
    $("#txtDeadLengthA").val(jsonResult.deaD_LENGTH_A); 
    $("#txtDeadLengthB").val(jsonResult.deaD_LENGTH_B); 
    $("#txtActualLength").val(jsonResult.eC_ACT_LENGTH);
    
    if ($.isNumeric(jsonResult.reec) && parseInt(jsonResult.reec) == 1) {
        $("#chkReEC").prop('checked', true);
    }
    else {
        $("#chkReEC").prop('checked', false);
    }
    

}


function SetChuubanDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#hfldEC_CD").val(jsonResult.eC_CD);
    $("#hdfNifudaCD").val(jsonResult.nifudA_CD)
    $("#txtChuuban2").val(jsonResult.pP_CHUUBAN);
    if (String(jsonResult.pP_GYOUBAN).length == 1) {
        jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
    }
    $("#txtGyouban2").val(jsonResult.pP_GYOUBAN);
    $("#hdfNifudaCD").val(jsonResult.nifudA_CD);
    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#txtProductSpecNoT1").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOBAN);
    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
   
    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);
    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
        $("#txtChamferT4").val(jsonResult.pP_CHAMFER);
    }
    if (jsonResult.pP_UNIT_WT_STRING !="") {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_STRING);
        $("#txtUnitWtT4").val(jsonResult.pP_UNIT_WT_STRING);
    }
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtConditionT4").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);
    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);
    $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtHostRefNoT1").val(jsonResult.producT_NYUKO_CD);
    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_OTHER);
    $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").val(jsonResult.pP_PACKING);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_STRING);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING_STRING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
        $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
   
    // 3rd Tab
    ResetMessageDetailsGrid();
    if (jsonResult.eC_MESSAGE_LIST) {
        SetMessageDetailsGrid(jsonResult.eC_MESSAGE_LIST);
    }
    
    /*Tab 4*/
    $("#txtProductSpecNoT4").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT4").val(jsonResult.pP_CHUUBAN_GYOBAN);
    $("#txtDeliveryDateT4").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtCustomerInchargeT4").text(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT4").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT4").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT4").val(jsonResult.pP_RECEIVER_NAME);
    
    $("#txtColorT4").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT4").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT4").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT4").val(jsonResult.pP_MATL_DIM_D);
    $("#txtChargeT4").val(jsonResult.pP_CHARGE);
    $("#txtBendStandardT4").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT4").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdShapeT4").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT4").val(jsonResult.pP_PROD_DIM_D);
    $("#txtProdDimOtherT4").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT4").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtHostRefNoT4").val(jsonResult.producT_NYUKO_CD);
    $("#txtDimToleranceT4").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT4").val(jsonResult.pP_DISPLAY);
    $("#txtStampT4").val(jsonResult.pP_STAMP);
    $("#txtLengthT4").val(jsonResult.pP_PROD_LENGTH_OTHER);
    $("#txtLengthToleranceT4").val(jsonResult.pP_LEN_TOL);
    $("#txtPackingT4").val(jsonResult.pP_PACKING);
    $("#txtNoOfUnitsT4").val(jsonResult.pP_NO_OF_UNITS_STRING);
    $("#txtQuantityT4").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT4").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT4").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT4").val(jsonResult.pP_COIL_DIA_WINDING_STRING);
    $("#txtKakouiraiRemarksT4").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT4").val(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtJISTestSpecimen").val(jsonResult.pP_JIS_TEST_SPEC);
    $("#txtTensileStrength").val(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtYieldPoint").val(jsonResult.pP_YEILD);
    $("#txtElongation").val(jsonResult.pP_ELONGATION);
    $("#txtReduction").val(jsonResult.pP_PRESS);
    $("#txtHardness").val(jsonResult.pP_HARDNESS);
    $("#txtDecarb").val(jsonResult.pP_DECARB);

    ClearTable($("#grdvProdPointData"), "", "GridNormalRow");

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
    $("#txtColorDecision").val(jsonResult.pP_COLOR);
    if (jsonResult.pP_CHAMFER > 0) {
        $("#txtChamferDecision").val("C-" + jsonResult.pP_CHAMFER);
    }
}

function Reset24HrsMachineGrid() {
    ClearTable($("#grdECMachine"), "ItemGrdvHikinukiMachine", "");
    $grdvMessage = $("#grdECMachine");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr1 = $('<tr class="GridNormalRow" style="height:18px;">').append(
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td rowspan='3' align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td colspan='2'align='center' valign='middle'> ").text(""),
        $("<td rowspan='3'align='center' valign='middle'>").text(""),
        $("<td colspan='2'align='center' valign='middle'> ").text(""),
        $("<td rowspan='3'align='center' valign='middle'>").text(""),
        $("<td colspan='2'align='center' valign='middle'> ").text(""),
        $("<td colspan='3'align='center' valign='middle'> ").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
    );
    var $tr2 = $('<tr class="GridNormalRow style="height:15px;">').append(
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' colspan='2' align='center' valign='middle'>").text(""),
        $("<td rowspan='2' colspan='3' align='center' valign='middle'>").text(""),
    );
    var $tr3 = $('<tr class="GridNormalRow" style="height:10px;>').append(
        $("<td align='center' valign='middle'>").text(""),
        
    );
    $grdvMessage.append($tr1);
    $grdvMessage.append($tr2);
    $grdvMessage.append($tr3);
     
}
function ResetMessageDetailsGrid() {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text("")
    );
    $grdvMessage.append($tr);
}

function ResetProdDataGrid() {
    $grdvMessage = $("#grdvProdPointData");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr = $('<tr class="GridNormalRow">').append(
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text(""),
        $("<td align='center' valign='middle'>").text("")
    );
    $grdvMessage.append($tr);

}
function ResetECHistoryGrid() {
    ClearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2
    ClearTable($("#grdvHistory"), "GridNormalRow", "");// Tab 2

    $grdvMessage = $("#grdvHistory");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add one blank data row
    var $tr1 = $('<tr class="GridNormalRow">').append(
        $("<td>").text(""),
        $("<td colspan='2'>").text(""),
        $("<td rowspan='3'>").text(""),
        $("<td>").text(""),
        $("<td colspan='2' style='border-top:revert;'>").text(""),
        $("<td>").text(""),
        $("<td>").text(""),
        $("<td>").text(""),
        $("<td rowspan='3'>").text(""),
        $("<td colspan='2'>").text(""),
        $("<td colspan='2'>").text(""),
        $("<td>").text(""),
        $("<td>").text(""),
        $("<td>").text(""),
        $("<td rowspan='3'>").text(""),
        $("<td rowspan='3'>").text(""),
    );
    var $tr2 = $('<tr class="GridNormalRow">').append(
        $("<td colspan='2' rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td rowspan='2'>").text(""),
        $("<td colspan='3' rowspan='2'>").text(""),
    );

    var $tr3 = $('<tr class="GridNormalRow">').append(
       
        $("<td>").text(""),
       
    );
 
    
       
    $grdvMessage.append($tr1);
    $grdvMessage.append($tr2);
    $grdvMessage.append($tr3);
    

}
function SetMessageDetailsGrid(messageDetailsList) {
    $grdvMessage = $("#grdvMessage");
    $grdvMessage.find("tr.GridNormalRow").remove(); // Clear the rows

    // Add data rows
    $.each(messageDetailsList, function (index, jsonObject) {

        // Show Process name in Japanese or English according to the culture.
        var processName = jsonObject.procesS_NAME_JP;
        if ($("#hdfCultureName").val() == "en") {
            processName = jsonObject.procesS_NAME_EN;
        }

        var $tr = $('<tr class="GridNormalRow tdPaddingLeftandRight">').append(
            $('<td class="GridAlignmentDateTime GridCenter">').text(jsonObject.msG_DATE_STRING),
            $('<td class="GridAlignmentProcessName GridLeft">').text(processName),
            $('<td class="GridAlignmentMachineName GridLeft">').text(jsonObject.machinE_NAME),
            $('<td class="GridAlignmentOperator GridLeft">').text(jsonObject.operatoR_NAME),
            $('<td class="GridAlignmentRemarks GridLeft">').text(jsonObject.msG_CONTENTS)
        );

        $grdvMessage.append($tr);
    });
}

function BindTPTestList() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ECEntry/GetTPTestList",
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
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindTPTestDropdown(jsonResult) {
    $("#ddlTPTest").html("");//.append($('<option></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
    $("#ddlTPTest").prop("selectedIndex", 0);
}

function SetStartSysDateTime() {

   
        var ajaxTime = new Date().getTime();
       
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/EC/GetSysDateTime",
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
                message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }
   


function SetEndSysDateTime() {

    var ajaxTime = new Date().getTime();
   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetSysDateTime",
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
            message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
   
}
$("#btnSave").click(function (event) {

    if ($("#txtCoilSize").val() == "" || $("#txtCoilShape").val() == "") {
        $("#checkDetectionCoil").addClass("Mandatory");
    } else {
        $("#checkDetectionCoil").removeClass("Mandatory");
    }
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    
    if (!CheckDate($("#txtStartDate"))) {
        var message = $("#lblErrorMessageE_EC_ENTRY_024").text().trim();
        ShowMessageDialog(message, "txtStartDate", WARNING);
        return false;
    }
    if (IsDuplicateThabaNo()) {
        return false;
    }

    if (!IsFIAlreadyDone() && $("#chkReEC").prop("checked") == false) {
        
        var message = $("#lblErrorMessageE_EC_ENTRY_030").text().trim();
        var focusFieldID = "txtNifudaNo1";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    if ($("#txtEndOperator").val() == "" && $("#txtEndOperator").hasClass("disabled")) {
        if (!IsDateValid(true)) {
            var message = $("#lblErrorMessageE_EC_ENTRY_026").text().trim() + "(" + $("#lblErrorMessageE_EC_ENTRY_028").text().trim() + commonDateRange + ")";
            var focusFieldID = "txtStartDate";
            ShowMessageDialog(message, focusFieldID, WARNING);
            return false;
        }
        
        SaveStartECDetails();
        GetMachineDetails($("#hdfMachineCD").val());
    } else {

        if (!CheckDate($("#txtEndDate"))) {
            var message = $("#lblErrorMessageE_EC_ENTRY_025").text().trim();
            ShowMessageDialog(message, "txtEndDate", WARNING);
            return false;
        }

       
        if (!IsEndDateValid()) {

            return false;
        }

        if (!IsDateValid(false)) {
            var message = $("#lblErrorMessageE_EC_ENTRY_027").text().trim() + "(" + $("#lblErrorMessageE_EC_ENTRY_028").text().trim() + commonDateRange + ")";

            var focusFieldID = "txtEndDate";
            ShowMessageDialog(message, focusFieldID, WARNING);
            return false;
        }
        if ($("#chkFICheck").prop("checked") == true && $("#ddlCombinedDecision").val() == combinedDecisionOK)
        {
            if (($("#txtHardnessDecision").val() != $("#lblOK").text() && $("#txtHardnessDecision").val() != $("#lblNA").text()))
            {
                ShowMessageDialog($("#lblErrorMessageE_EC_ENTRY_035").text().trim(), "ddlCombinedDecision", WARNING);
                 return false;
            } else if (($("#txtTensileDecision").val() != $("#lblOK").text() && $("#txtTensileDecision").val() != $("#lblNA").text()))
            {
                ShowMessageDialog($("#lblErrorMessageE_EC_ENTRY_035").text().trim(), "ddlCombinedDecision", WARNING);
                return false;
             }
        }

        if (IsEndDateTimeGreaterThanStartDateTimeByTenMin($("#txtStartDate").val(), $("#txtEndDate").val())) {
            UpdateECDetails();
            GetMachineDetails($("#hdfMachineCD").val());

        } else {

            bootbox.confirm({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblConfirmationMessageC_EC_ENTRY_001").text()) + "</td></tr></table>",
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
                        UpdateECDetails();
                        GetMachineDetails($("#hdfMachineCD").val());
                    } else {
                        $("#txtEndDate").focus();
                    }

                },
                centerVertical: true
            });

        }
        
    }


});
function IsFIAlreadyDone() {
    var ajaxTime = new Date().getTime();
    var returnval = false;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/IsFIAlreadyDone?nifudaCD=" + $("#hdfNifudaCD").val() + "&thabaNo=" + $("#txtTabaNo").val() ,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {

            returnval = response.isSuccess;
          
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnval;
}
function IsEndDateValid() {

    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_EC_ENTRY_007").text().trim();
        var focusFieldID = "txtEndDate";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_EC_ENTRY_009").text().trim();
        var focusFieldID = "txtEndDate";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
}

function CheckDate(field) {

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
function SaveStartECDetails() {
    var ajaxTime = new Date().getDate();

    var jsonDataForSave = JSON.stringify({
        NIFUDA_NO: $("#txtNifudaNo1").val() ? $("#txtNifudaNo1").val() : null,
        NIFUDA_CD: $("#hdfNifudaCD").val(),
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        EC_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        EC_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        EC_TABA_NO: $("#txtTabaNo").val() ? $("#txtTabaNo").val() : null,
        TP_TEST: $("#ddlTPTest option:selected").val() ? $("#ddlTPTest option:selected").val() : null,
        EC_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val() ? $("#txtMetalicPltNo").val() : null,
        ACT_ALLOC_UNITS: $("#txtNoOfCoils").val() ? $("#txtNoOfCoils").val() : null,
        ACT_ALLOC_QTY: $("#txtCoilQuantity").val() ? $("#txtCoilQuantity").val() : null,
        IS_OK: $("#txtIsOK").val() ? $("#txtIsOK").val() : null,
        EC_COIL_SIZE: $("#txtCoilSize").val() ? $("#txtCoilSize").val() : null,
        EC_COIL_SHAPE: $("#txtCoilShape").val() ? $("#txtCoilShape").val() : null,
        FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        SHIKII_VALUE: $("#txtShikii").val() ? $("#txtShikii").val() : null,
        LINE_SPEED: $("#txtLineSpeed").val() ? $("#txtLineSpeed").val() : null,
        EC_ACT_DIM1: $("#txtActDim1").val() ? $("#txtActDim1").val() : null,
        EC_ACT_DIM2: $("#txtActDim2").val() ? $("#txtActDim2").val() : null,
        EC_ACT_DIM3: $("#txtActDim3").val() ? $("#txtActDim3").val() : null,
        EC_ACT_DIM4: $("#txtActDim4").val() ? $("#txtActDim4").val() : null,
        DEAD_LENGTH_A: $("#txtDeadLengthA").val() ? $("#txtDeadLengthA").val() : null,
        DEAD_LENGTH_B: $("#txtDeadLengthB").val() ? $("#txtDeadLengthB").val() : null,
        EC_ACT_LENGTH: $("#txtActualLength").val() ? $("#txtActualLength").val() : null,
        REEC: ($('#chkReEC').is(':checked') ? 1 : 0),
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,

    });


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/ECDetailsSave",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            
            var jsonResult = response.message;
            $("#hfldEC_CD").val(jsonResult.eC_CD);
            responseTime = new Date().getTime() - ajaxTime;
            SetNextProcess();
            SetFINextProcess();
            SetDecision();
            SetFIDropdown();
            SetProcessDefectList();
            SetEndSysDateTime();
            EnableEndDataPanel();
           // EnableFIDataPanel();
            DisableDefaultField();
            DisableFIDefaultField();
            SetUnit();
            if (parseInt($("#txtTabaNo").val()) > 0) {
                $("#txtTabaNo").attr(DISABLED, DISABLED).addClass(DISABLED);
            }
            if (parseInt(jsonResult.uniT_TYPE) > 0) {
                $("#ddlUnit").val(jsonResult.uniT_TYPE);
            }
            var successMessage = $("#lblSuccessMessageS_EC_ENTRY_001").text();
            ShowMessageDialog(successMessage, "txtEndDate", SUCCESS);       

            $("#txtEndOperator").val($("#txtStartOperator").val());
            $("#hdfEndOperatorCD").val($("#hdfStartOperatorCD").val());
          

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function UpdateECDetails() {
    var ajaxTime = new Date().getDate();

    var EC_Percentage = null;
    if ($("#txtDefectRate").val() > 0) {
        EC_Percentage = Math.round($("#txtDefectRate").val());
    }
   
    var jsonDataForSave = JSON.stringify({
        EC_CD: $("#hfldEC_CD").val() ? $("#hfldEC_CD").val() : null,

        PROCESS_PLAN_CD: $("#hdfldProcessPlanCd").val() ? $("#hdfldProcessPlanCd").val() : null,
        MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
        EC_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        EC_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
        EC_TABA_NO: $("#txtTabaNo").val() ? $("#txtTabaNo").val() : null,
        TP_TEST: $("#ddlTPTest option:selected").val() ? $("#ddlTPTest option:selected").val() : null,
        EC_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val() ? $("#txtMetalicPltNo").val() : null,
        ACT_ALLOC_UNITS: $("#txtNoOfCoils").val() ? $("#txtNoOfCoils").val() : null,
        ACT_ALLOC_QTY: $("#txtCoilQuantity").val() ? $("#txtCoilQuantity").val() : null,
        IS_OK: $("#txtIsOK").val() ? $("#txtIsOK").val() : null,
        EC_COIL_SIZE: $("#txtCoilSize").val() ? $("#txtCoilSize").val() : null,
        EC_COIL_SHAPE: $("#txtCoilShape").val() ? $("#txtCoilShape").val() : null,
        FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        SHIKII_VALUE: $("#txtShikii").val() ? $("#txtShikii").val() : null,
        LINE_SPEED: $("#txtLineSpeed").val() ? $("#txtLineSpeed").val() : null,
        REEC: ($('#chkReEC').is(':checked') ? 1 : 0),

        EC_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        EC_END_OPERATOR_CD: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
        EC_VISUAL_CHK: $('#ddlInspection').val(), 
        EC_ACT_DIM1: $("#txtActDim1").val() ? $("#txtActDim1").val() : null,
        EC_ACT_DIM2: $("#txtActDim2").val() ? $("#txtActDim2").val() : null,
        EC_ACT_DIM3: $("#txtActDim3").val() ? $("#txtActDim3").val() : null,
        EC_ACT_DIM4: $("#txtActDim4").val() ? $("#txtActDim4").val() : null,
        DEAD_LENGTH_A: $("#txtDeadLengthA").val() ? $("#txtDeadLengthA").val() : null,
        DEAD_LENGTH_B: $("#txtDeadLengthB").val() ? $("#txtDeadLengthB").val() : null,
        EC_ACT_LENGTH: $("#txtActualLength").val() ? $("#txtActualLength").val() : null,
        MAGNETISM: $("#txtMagnetism").val() ? $("#txtMagnetism").val() : null,
        EC_OK_BARS: $("#txtOKBars").val() ? $("#txtOKBars").val() : null,
        UNIT_TYPE: $("#ddlUnit").val() ? $("#ddlUnit").val() : null,
        EC_OK_QTY: $("#txtOKQty").val() ? $("#txtOKQty").val() : null,
        EC_NG_QTY: $("#txtNGWt").val() ? $("#txtNGWt").val() : null,
        EC_NG_BARS1: $("#txtNGBars1").val() ? $("#txtNGBars1").val() : null,
        EC_NG_DETAILS1: $("#ddlNGReason1").val()  > 0 ? $("#ddlNGReason1").val() : null,
        EC_NG_BARS2: $("#txtNGBars2").val() ? $("#txtNGBars2").val() : null,
        EC_NG_DETAILS2: $("#ddlNGReason2").val() > 0 ? $("#ddlNGReason2").val() : null,
        EC_NG_BARS3: $("#txtNGBars3").val() ? $("#txtNGBars3").val() : null,
        EC_NG_DETAILS3: $("#ddlNGReason3").val() > 0 ? $("#ddlNGReason3").val() : null,
        EC_NG_BAR_TOTAL: $("#txtNGBarsTotal").val() ? $("#txtNGBarsTotal").val() : null,
        EC_NG_PERCENTAGE: EC_Percentage,
        EC_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        EC_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,

        EC_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,

        NIFUDA_CD: $("#hdfNifudaCD").val() ? $("#hdfNifudaCD").val() : null,
       // FINAL_INSP: ($('#chkFI').is(':checked') ? 1 : 0),
        FI_DIM_DECISION: $("#ddlActDecision").val() ? $("#ddlActDecision").val() : null,
        FI_LENGTH_DECISION: $("#ddlActLenDecision").val() ? $("#ddlActLenDecision").val() : null,
        COMBINED_DECISION: $("#ddlCombinedDecision").val() ? $("#ddlCombinedDecision").val() : null,
        FI_NEXT_PROCESS: $("#ddlNextProcessFI").val() ? $("#ddlNextProcessFI").val() : null,
        FI_COLOR_DECISION: $("#ddlColorDecision").val() ? $("#ddlColorDecision").val() : null,
        FI_CHAMFERING_DECISION: $("#ddlChamferDecision").val() ? $("#ddlChamferDecision").val() : null,
        FI_EXT_TEXTURE_DECISION: $("#ddlExtTextureDecision").val() ? $("#ddlExtTextureDecision").val() : null,
        FINAL_INSP: ($('#chkFICheck').is(':checked') ? 1 : 0), 
        IS_FI_CHECK: ($('#chkFICheck').is(':checked') ? 1 : 0), 
        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,

    });



    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/ECDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;


            var successMessage = $("#lblSuccessMessageS_EC_ENTRY_001").text();
            ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);


            DisableStartDataPanel();
            DisableEndDataPanel();
            DisableFIDataPanel();
            
            DisableSaveButton();
          //  disableFIDetailsDataPanel();
          //  disableFIEntryDataPanel();
            GetThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val());
           

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
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
        url: APIURL + "/EC/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",

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
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}


function BindNextProcessDropdown(jsonResult) {
    $("#ddlNextProcess").html("");

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
function SetFINextProcess() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetFINextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindFINextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindFINextProcessDropdown(jsonResult) {
   
    $("#ddlNextProcessFI").html("");

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcessFI").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));

        });
    }
    if ($("#hdfCultureName").val() == "ja-JP") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcessFI").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));

        });
    }

}
function SetDecision() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ECEntry/GetDecision",
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
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindDecisionDropdown(jsonResult) {
    $("#ddlDecision").html("");
    $("#ddlCombinedDecision").html("");
   
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlCombinedDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function SetFIDropdown() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ECEntry/GetFIDropdownList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindFIDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindFIDropdown(jsonResult) {
    $("#ddlActDecision").html("").append($('<option></option>'));
    $("#ddlActLenDecision").html("").append($('<option></option>'));
    
    $("#ddlColorDecision").html("").append($('<option></option>'));
    $("#ddlChamferDecision").html("").append($('<option></option>'));
    $("#ddlExtTextureDecision").html("").append($('<option></option>'));
    $("#ddlInspection").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlActDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlActLenDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        
        $("#ddlColorDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlChamferDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlExtTextureDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlInspection").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}


function SetProcessDefectList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetProcessDefect",
        method: "POST",
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
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
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
function DisableDefaultField() {
    $("#ddlNGReason1").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#ddlNGReason2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#ddlNGReason3").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtDefectRate").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtNGBarsTotal").attr(DISABLED, DISABLED).addClass(DISABLED);
   
}
function DisableFIDefaultField() {
    $("#txtColorDecision").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtChamferDecision").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtHardnessDecision").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtTensileDecision").attr(DISABLED, DISABLED).addClass(DISABLED);
}
$("#btnStartDateTime").click(function (event) {
    SetStartSysDateTime();
});
$("#btnEndDateTime").click(function (event) {
    SetEndSysDateTime();
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

    if (keycode == 13 && operator && operatorEndModified == true)
    {
        $("#hdfEndOperatorCD").val(operatorNumber);
        enterPressedEndOpr = true;
            GetOperatorNameByCD($(this));
        $("#ddlInspection").focus();
    }
    if (e.type === 'focusout' && operator && operatorEndModified == true) {
        if (enterPressedEndOpr) {

            enterPressedEndOpr = false; // Reset the flag
            return false;
        }
        $("#hdfEndOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#ddlInspection").focus();
       
    }
    enterPressedEndOpr = false; // Reset the flag
});

$("#txtNGBars1").keyup(function (event) {
    DdlNGReasonMandatory($("#txtNGBars1"), $("#ddlNGReason1"));
    CalculateBarCount();
});
$("#txtNGBars2").keyup(function (event) {
    DdlNGReasonMandatory($("#txtNGBars2"), $("#ddlNGReason2"));
    CalculateBarCount();
});
$("#txtNGBars3").keyup(function (event) {
    DdlNGReasonMandatory($("#txtNGBars3"), $("#ddlNGReason3"));
    CalculateBarCount();
});
function CalculateBarCount() {
    var bar1 = 0;
    var bar2 = 0;
    var bar3 = 0;
    if ($("#txtNGBars1").val() != "") {
        bar1 = $("#txtNGBars1").val();
    }
    if ($("#txtNGBars2").val() != "") {
        bar2 = $("#txtNGBars2").val();
    }
    if ($("#txtNGBars3").val() != "") {
        bar3 = $("#txtNGBars3").val();
    }
    var totalValue = parseInt(bar1) + parseInt(bar2) + parseInt(bar3);
    if (totalValue > 0) {
        $("#txtNGBarsTotal").val(totalValue);
        CalculateDefectRate();
       
    } else {
        $("#txtNGBarsTotal").val("");
        $("#txtDefectRate").val("");
    }
}
$("#txtOKBars").change(function () {
    if ($("#txtNGBarsTotal").val() > 0) {
        CalculateDefectRate();
    }
});
function CalculateDefectRate() {
    if ($("#txtOKBars").val() > 0) {
        var defectPercent = parseFloat($("#txtNGBarsTotal").val()) / (parseFloat($("#txtOKBars").val()) + parseFloat($("#txtNGBarsTotal").val())) * 100;
        //var defectPercent = parseFloat($("#txtNGBarsTotal").val()) / (parseFloat($("#txtOKBars").val())) * 100;
        $("#txtDefectRate").val(defectPercent.toFixed(2));
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
        url: APIURL + "/EC/IsValidDateRange",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DATERANGE: commonDateRange,  RANGE_DATETIME: getDate })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           returnval= response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

    return returnval;
}

function SetUnit() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ECEntry/GetProductUnitKubun",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitDropdown(jsonResult) {
    $("#ddlUnit").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnit").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

$("#chkFICheck").click(function () {
    if ($("#chkFICheck").prop("checked") == true) {
        EnableFIDataPanel();
        DisableFIDefaultField();
        GetHardnessTensileDecision();
        $("#ddlCombinedDecision").prop('selectedIndex', 0);
        $("#ddlNextProcessFI").prop('selectedIndex', 0);
        
    } else {
        DisableFIDataPanel();
        $("#ddlActDecision").val(0);
        $("#ddlColorDecision").val(0);
        $("#ddlActLenDecision").val(0);
        $("#ddlChamferDecision").val(0);
        $("#ddlExtTextureDecision").val(0);
        $("#ddlCombinedDecision").prop('selectedIndex', -1);
        $("#ddlNextProcessFI").prop('selectedIndex', -1);
        $("#txtHardnessDecision").val("");
        $("#txtTensileDecision").val("");
    }
});

$("#btnInitialize").click(function (event) {
    
    Initialize();
    $("#txtMachineType").focus();
});
function ClearData() {
    EnableSearchOrderNoPanel();
    combinedDecisionOK = "";
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#divPanelCphStartEndData"));
    ClearPanel($("#TbProcessInfo")); // Tab 2
   // reset24HrsMachineGrid();
    ResetMessageDetailsGrid();
    ResetProdDataGrid();
    ClearThabaDetailsGrid();
}
function Initialize() {
    EnableSearchOrderNoPanel();
    combinedDecisionOK = "";
    DisableSaveButton();
    
    $(".FocusDefault").focus();
    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableFIDataPanel();
    DisableSaveButton();
    
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    ClearPanel($("#TbHistorySearch")); // Tab 2
  
    ClearPanel($("#pnlProcessInfo")); // Tab 4
    ClearPanel($("#pnlTestSpecifications")); // Tab 4    
    ClearPanel($("#pnlFIData"));
    
    ClearThabaDetailsGrid();
  
    ClearTable($("#grdECMachine"), "ItemGrdvHikinukiMachine", "");
    //clearTable($("#grdvMessage"), "", "GridNormalRow"); // Tab 3
    //clearTable($("#grdvProdPointData"), "", "GridNormalRow"); // Tab 4
    //clearTable($("#grdvHistory"), "ItemGrdvProcessHistory", "");// Tab 2

    $("#txtNifudaNo1").val("");
    $("#txtChuuban2").val("");
    $("#txtGyouban2").val("");
    $("#txtMachineType").val("");

    

    ResetGlobalVariables();
    Reset24HrsMachineGrid()
    ResetECHistoryGrid();
    ResetMessageDetailsGrid();
    ResetProdDataGrid();
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
function GetThabaDetailsByChuuban(chuuban, gyouban) {

    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: String(gyouban) })
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
            var message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);

        });

}
function SetThabaDetails(jsonResult) {

    ClearThabaDetailsGrid();
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
function IsDuplicateThabaNo() {

    var inputThaba = $("#txtTabaNo").val().trim();

    var message = "";

    var firstFieldID = "";

    // Check if entered thaba number is already registered and its combined decision is O
    if (inputThaba in registeredThabaNumbersAndDecision) {
        if (parseInt(registeredThabaNumbersAndDecision["" + inputThaba]) == combinedDecisionOK && $('#chkReEC').prop("checked") == false) {
            firstFieldID = "txtTabaNo";
          var  message = $("#lblErrorMessageE_EC_ENTRY_005").text();
            ShowMessageDialog(message, firstFieldID, WARNING);
            return true;
        }
    }
}

$("#txtNifudaNo1").focusout(function () {
    $("#btnChuubanSearch").focus();
});

$("#chkReEC").focusout(function () {
    if ($("#txtEndDate").hasClass("disabled")) {
        $("#btnSave").focus();
    } else {
        $("#txtEndDate").focus();
    }
    
});

$("#ddlNextProcessFI").focusout(function () {
    $("#btnSave").focus();
});

$("#ddlNextProcess").focusout(function () {

    if ($("#ddlActDecision").hasClass("disabled")) {
        $("#btnSave").focus();
    } else {
        $("#ddlActDecision").focus();
    }
});
$("#btnSearchT2").click(function () {
    ResetECHistoryGrid();
    var errorMessage = "";
    var firstFieldID = "";
    var $callFrom = $(this);
    var chuuban = $("#txtChubanT2").val();
    var gyouban = $("#txtGyobanT2").val();
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
    else {
        ValidateChubban($("#txtChubanT2").val(), $("#txtGyobanT2").val(), $callFrom);
    }
    
});
function ValidateChubban(chuuban, gyouban, $callFrom) {


    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChubanT2").val();
    var gyouban = $("#txtGyobanT2").val();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/ValidateChubban",
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
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_004").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban2", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                var message = $("#lblErrorMessageE_EC_ENTRY_033").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(message, "", WARNING);
                // ClearData();
                return false;
            }
            else if (result.responce == 1) {
                GetECHistoryByChubban();
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_EC_ENTRY_014").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}
 function GetECHistoryByChubban(){
    //resetECHistoryGrid();
    //var errorMessage = "";
    //var firstFieldID = "";
    var chuuban = $("#txtChubanT2").val();
     var gyouban = $("#txtGyobanT2").val();
     var ajaxTime = new Date().getTime();
   

    var ajaxTime = new Date().getTime();
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetECHistoryByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ CHUUBAN: $("#txtChubanT2").val(), GYOUBAN: $("#txtGyobanT2").val(),CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            
            ClearPanel($("#TbHistorySearch"));
            $("#txtChubanT2").val(chuuban);
            $("#txtGyobanT2").val(gyouban);
           
           
            //if (jsonResult === null) {
               
            //    var errorMessage = $("#lblErrorMessageE_EC_ENTRY_003").text();
            //    showMessageDialog(errorMessage, "txtChubanT2", WARNING);
            //    return false;
            //}


            //// Chuuban is marked as completed in the system
            //if (jsonResult.pP_PROCESS_PLAN_CMPL_DATE != null) {
               
            //    var errorMessage = $("#lblErrorMessageE_EC_ENTRY_004").text();
            //    showMessageDialog(errorMessage, "txtChubanT2", WARNING);
            //    return false;
            //}
            
            var jsonObj = jsonResult.procesS_HISTORY;
            if (jsonResult != null && jsonObj == null) {
               
                var errorMessage = $("#lblErrorMessageE_EC_ENTRY_003").text();
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
            message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
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
          $("#txtConditionT2").val(jsonResult.pP_PRODUCT_CONDITION);
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
          $("#txtWindingDirectionT2").val(jsonResult.pP_COIL_DIA_WINDING_STRING);

          $("#txtProductSpecNoT2").val(jsonResult.pP_PROD_SPEC_NO);
          $("#txtStdDeliveryDateT2").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
          $("#txtColorT2").val(jsonResult.pP_COLOR);
          $("#txtBendStandardT2").val(jsonResult.pP_MAGARI_BEND_TOL);
          $("#txtExtTextureT2").val(jsonResult.pP_EXT_TEXTURE);
          $("#txtDisplayT2").val(jsonResult.pP_DISPLAY);
          $("#txtPackingT2").val(jsonResult.pP_PACKING);
               
          $("#txtMaterialT2").val(jsonResult.pP_MATL_NAME_MAK);
          $("#txtDimT2").val(jsonResult.pP_MATL_DIM_D_SHAPE);

    }
}
function SetHistoryGrid(jsonResult) {

   
    $tableProcessHistory = $("#grdvHistory");
    ClearTable($tableProcessHistory, "ItemGrdvProcessHistory", "");
    ClearTable($tableProcessHistory, "", "GridNormalRow");
    debugger;
   // Tab 2

    
    var jsonListObj = jsonResult.procesS_HISTORY;
    $.each(jsonListObj, function (index, jsonObject) {
        var firstRow = processHistoryGridataRow1;

        if (jsonObject.pP_PROCESS_LOCATION != null)
            firstRow = firstRow.replace("valtxtProcLocation", jsonObject.pP_PROCESS_LOCATION);
        else
            firstRow = firstRow.replace("valtxtProcLocation", "");

        if (jsonObject.eC_START_DATE_STRING != null)
            firstRow = firstRow.replace("valtxtStartDate", jsonObject.eC_START_DATE_STRING);
        else
            firstRow = firstRow.replace("valtxtStartDate", "");

        if (jsonObject.eC_END_DATE_STRING != null)
            firstRow = firstRow.replace("valtxtEndDate", jsonObject.eC_END_DATE_STRING);
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

        if (jsonObject.eC_METALLIC_PLATE_CODE != null)
            firstRow = firstRow.replace("valtxtMetalicPlate", jsonObject.eC_METALLIC_PLATE_CODE);
        else
            firstRow = firstRow.replace("valtxtMetalicPlate", "");

        if (jsonObject.pP_PROD_DIM_D != null)
            firstRow = firstRow.replace("valtxtProdDimD", jsonObject.pP_PROD_DIM_D + jsonObject.pP_PROD_SHAPE);
        else
            firstRow = firstRow.replace("valtxtProdDimD", "");

        if (jsonObject.pP_NO_OF_UNITS_DECODE != null)
            firstRow = firstRow.replace("valtxtNoOfUnits", jsonObject.pP_NO_OF_UNITS_DECODE);
        else
            firstRow = firstRow.replace("valtxtNoOfUnits", "");

        if (jsonObject.pP_ACT_ALLOC_QTY != null)
            firstRow = firstRow.replace("valtxtActAllocQty", jsonObject.pP_ACT_ALLOC_QTY);
        else
            firstRow = firstRow.replace("valtxtActAllocQty", "");

        if (jsonObject.eC_TABA_NO != null)
            firstRow = firstRow.replace("valtxtThabaNo", jsonObject.eC_TABA_NO);
        else
            firstRow = firstRow.replace("valtxtThabaNo", "");

        if (jsonObject.eC_OK_BARS_STRING != null)
            firstRow = firstRow.replace("valtxtOkBars", jsonObject.eC_OK_BARS_STRING );
        else
            firstRow = firstRow.replace("valtxtOkBars", "");

        if (jsonObject.eC_OK_QTY != null)
            firstRow = firstRow.replace("valtxtOkQty", jsonObject.eC_OK_QTY);
        else
            firstRow = firstRow.replace("valtxtOkQty", "");

        if (jsonObject.eC_NG_BARS1 != null)
            firstRow = firstRow.replace("valtxtNGBars1", jsonObject.eC_NG_BARS1);
        else
            firstRow = firstRow.replace("valtxtNGBars1", "");
        
        if (jsonObject.eC_NG_DETAILS1_STRING != null)
            firstRow = firstRow.replace("valtxtNGDetails1", jsonObject.eC_NG_DETAILS1_STRING);
        else
            firstRow = firstRow.replace("valtxtNGDetails1", "");

        if (jsonObject.iS_OK != null)
            firstRow = firstRow.replace("valtxtIsOk", jsonObject.iS_OK);
        else
            firstRow = firstRow.replace("valtxtIsOk", "");

        if (jsonObject.linE_SPEED != null)
            firstRow = firstRow.replace("valtxtLineSpeed", jsonObject.linE_SPEED);
        else
            firstRow = firstRow.replace("valtxtLineSpeed", "");

        if (jsonObject.eC_MAGNETISM != null)
            firstRow = firstRow.replace("valtxtMagnetism", jsonObject.eC_MAGNETISM);
        else
            firstRow = firstRow.replace("valtxtMagnetism", "");

        if (jsonObject.eC_DECISION_STRING != null)
            firstRow = firstRow.replace("valtxtDecision", jsonObject.eC_DECISION_STRING);
        else
            firstRow = firstRow.replace("valtxtDecision", "");

        if (jsonObject.nexT_PROCESS != null)
            firstRow = firstRow.replace("valtxtNextProcess", jsonObject.nexT_PROCESS);
        else
            firstRow = firstRow.replace("valtxtNextProcess", "");
        
        var secondRow = processHistoryGridataRow2;

        if (jsonObject.eC_MACHINE_NAME != null)
            secondRow = secondRow.replace("valtxtMachineName", jsonObject.eC_MACHINE_NAME);
        else
            secondRow = secondRow.replace("valtxtMachineName", "");

        if (jsonObject.eC_END_OPERATOR_NAME != null)
            secondRow = secondRow.replace("valtxtEndOperator", jsonObject.eC_END_OPERATOR_NAME);
        else
            secondRow = secondRow.replace("valtxtEndOperator", "");

        if (jsonObject.pP_PROD_SPEC_NO != null)
            secondRow = secondRow.replace("valtxtProdSpecNo", jsonObject.pP_PROD_SPEC_NO);
        else
            secondRow = secondRow.replace("valtxtProdSpecNo", "");

        if (jsonObject.pP_PROD_LENGTH_STRING != null)
            secondRow = secondRow.replace("valtxtProdLength", jsonObject.pP_PROD_LENGTH_STRING);
        else
            secondRow = secondRow.replace("valtxtProdLength", "");

        if (jsonObject.eC_NG_BARS2 != null)
            secondRow = secondRow.replace("valtxtNGBars2", jsonObject.eC_NG_BARS2);
        else
            secondRow = secondRow.replace("valtxtNGBars2", "");

        if (jsonObject.eC_NG_DETAILS2_STRING != null)
            secondRow = secondRow.replace("valtxtNGDetails2", jsonObject.eC_NG_DETAILS2_STRING);
        else
            secondRow = secondRow.replace("valtxtNGDetails2", "");

        if (jsonObject.eC_REMARKS != null)
            secondRow = secondRow.replace("valtxtRemark", jsonObject.eC_REMARKS);
        else
            secondRow = secondRow.replace("valtxtRemark", "");

        

        var thirdRow = processHistoryGridataRow3;
        if (jsonObject.pP_CHARGE != null)
            thirdRow = thirdRow.replace("valtxtCharge", jsonObject.pP_CHARGE);
        else
            thirdRow = thirdRow.replace("valtxtCharge", "");

        if (jsonObject.eC_NG_BARS3 != null)
            thirdRow = thirdRow.replace("valtxtNGBars3", jsonObject.eC_NG_BARS3);
        else
            thirdRow = thirdRow.replace("valtxtNGBars3", "");

        if (jsonObject.eC_NG_DETAILS3_STRING != null)
            thirdRow = thirdRow.replace("valtxtNGDetails3", jsonObject.eC_NG_DETAILS3_STRING);
        else
            thirdRow = thirdRow.replace("valtxtNGDetails3", "");

        $tableProcessHistory.append($(firstRow));
        $tableProcessHistory.append($(secondRow));
        $tableProcessHistory.append($(thirdRow));
        
    });

}
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
            url: APIURL + "/EC/MappNifudaByBarcode?nifudaNo=" + nifudaNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
            
                if (response.isSuccess) {
                    $('#txtNifudaNo1').val(jsonResult.nifudA_NO);
                    $('#btnChuubanSearch').focus();
                } else {
                    $('#txtNifudaNo1').val("");
                  
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }

});
function GetHardnessTensileDecision()
{
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetHardnessTensileDescion?processPlanCD=" + $("#hdfldProcessPlanCd").val() + "&cultureName=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess) {
                $('#txtHardnessDecision').val(jsonResult.hardnesS_DESICION);
                $('#txtTensileDecision').val(jsonResult.tensilE_DECISION);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_EC_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}