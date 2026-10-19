var $btnLnkEdit;
var $btnDelete;
var $hdfSYSDATE;
var combinedDecisionOK = "";
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";

var registeredThabaNumbersAndDecision = {};

var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var IsStartOprExists = false;
var IsEndOprExists = false;;
var IsExists = false;
var chuuban = null;
var gyouban = null;
var registeredThabaNumbersWithDecisionOK = {};
// Error log information. To be sent with each AJAX request. 
userName = ""; // assigned at Document.ready
// End error log information.

var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;



function resetGlobalVariables() {

    dimTolMin = "";
    dimTolMax = "";
    dimStd = "";
    lenTolMin = "";
    lenTolMax = "";
    lenStd = "";

    //  isStraightingRemarksUpdate = 0;
    chuuban = null;
    gyouban = null;
    registeredThabaNumbersWithDecisionOK = {};
}

function SetGlobalVariables() {
    $btnLnkEdit = $("#btnLnkEdit");
    $btnUpdate = $("#btnUpdate");
    $btnDelete = $("#btnDelete");
    $hdfSYSDATE = $("#hdfSYSDATE");

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


function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown) {
            if (parseInt(mandatoryField.value) === indexDDLDecisionNothing) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText) {
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
        $("#dialogMessageIcon img").hide();
        //$("#imgError").show();
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
function updateChamferingDetails($callFrom) {
    
    if ($("#hfld_MessageCD").val() == 0) {
        $("#hfld_MessageCD").val(null)
    }


    var ajaxTime = new Date().getDate();
    var jsonDataSaveEditDelete = JSON.stringify({
        CH_MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
        CHAMFERING_CD: $("#hfld_ChamferingCd").val(),
        PROCESS_PLAN_CD: $("#hfld_ProcessPlanCD").val(),
        MACHINE_CD: $("#ddlModel").val(),
        CH_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        CH_START_OPERATOR_CD: $("#ddlStartOperatorNameValue").find(":selected").val(),
        CH_METALLIC_PLATE_CODE: $("#txtMetalicPlateCode").val(),
        CH_SHAPE: $("#txtChShape").val(),
        FURNACE_NO: $("#txtFurnaceNum").val(),
        CH_ACT_LENGTH: $("#txtActualLength").val() ? $("#txtActualLength").val() : null,
        RECHAMFERING: ($('#chkReChamfering').is(':checked') ? 1 : 0),
        CH_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        CH_END_OPERATOR_CD: $("#ddlEndOperatorNameValue").find(":selected").val(),
        CH_FINISHED_BARS: $("#txtFinishedBarCount").val(),
        UNIT_TYPE: $("#ddlUnitType").val(),
        CH_FINISHED_QTY: $("#txtFinishedQuantity").val(),
        CH_TABA_NO_1: $("#txtThaba1").val(),
        CH_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        CH_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        CH_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        CH_DECISION: $("#ddlDecision").val(),
        CH_NEXT_PROCESS: $("#ddlNextProcess").val(),
        CH_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        USER_ID: $("#hdfLoginId").val(),
        USER_NAME: userName
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/ChamferDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            disableEditDeleteButtons();
            if (parseInt(response.message) > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }
            var jsonResult = response.message;
            var successMessage = $.trim($("#lblSuccessMessageS_CHA_EDIT_DELETE_001").text());
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            enableEditDeleteButtons();

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_CHA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function DeleteChamferingDetails() {
    debugger;
    if ($("#hfld_MessageCD").val() == 0) {
        $("#hfld_MessageCD").val(0)
    }

    var ajaxTime = new Date().getTime();
    var jsonDataDelete = JSON.stringify({

        CHAMFERING_CD: parseInt($("#hfld_ChamferingCd").val()),

        PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),
       
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,

        CH_TABA_NO_1: $("#txtThaba1").val(),
        CH_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        CH_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        CH_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/ChamferDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            //clearPanel($("#pnlProductDetails"));
            //clearPanel($("#pnlProcessStartData"));
            //clearPanel($("#pnlProcessEndData"));
            //resetGlobalVariables();
            disableEditDeleteButtons();

            var successMessage = $("#lblSuccessMessageS_CHA_EDIT_DELETE_002").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            //    switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"))

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);

        });
}

function getChamferingDetailsByChamferingCD($callFrom) {

    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var chamferingCD = $.trim($callFrom.closest("tr").attr("data-lblCHAMFERINGCD"));
    var jsonData = "{CHAMFERING_CD :'" + chamferingCD + "', USER_NAME: '" + userName + "' }";
    // setProcessDefectList();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetChamferingDetailsByChamferingCD?chamferingCD=" + chamferingCD + "&cultureName=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = null;
            jsonResult = response.message;

            setChamferingDetails(jsonResult);
            $("#btnUpdate").unbind("click")
            $("#btnUpdate").click(function (event) {
                debugger;
                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }
                //if (!IsDropDownMandatory()) {
                //    return false;
                //}
                if (!IsEndDateValid()) {
                    return false;
                }

                if (!checkDate($("#txtStartDate"))) {
                    var message = $("#lblErrorMessageE_CHA_EDIT_DELETE_006").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtStartDate", WARNING);
                    $("#txtStartDate").focus;
                    return false;
                }
                if (!checkDate($("#txtEndDate"))) {
                    var message = $("#lblErrorMessageE_CHA_EDIT_DELETE_007").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtEndDate", WARNING);
                    $("#txtEndDate").focus;
                    return false;
                }
                //if (!IsActiveMachine(jsonResult.machinE_CD)) {
                //    return false;
                //}
                if (jsonResult.pP_GYOUBAN.toString().length == 1) {
                    jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
                }

                
                if (!IsDuplicateThabaNo(jsonResult.pP_CHUUBAN, jsonResult.pP_GYOUBAN, $callFrom)) {
                    return false;
                }
                if ($("#ddlModel option:selected").attr("flag") == 1) {

                    MachineIsInactive();
                    return false;
                }

                if ($("#ddlStartOperatorNameValue option:selected").attr("flag") == 1) {

                    OperatorIsInactive("ddlStartOperatorNameValue");
                    return false;
                }

                if ($("#ddlEndOperatorNameValue option:selected").attr("flag") == 1) {

                    OperatorIsInactive("ddlEndOperatorNameValue");
                    return false;
                }
                updateChamferingDetails();

            });
            $("#btnStartDateTime").click(function (event) {
                setStartSysDateTime();
            });

            $("#btnEndDateTime").click(function (event) {
                setEndSysDateTime();
            });
            $("#btnYobinaStartOpeartorSearch").click(function (event) {
                getOperatorList($("#txtYobinaStartOperatorSearch").val(), $("#ddlStartOperatorNameValue"));
            });
            $("#btnYobinaEndOpeartorSearch").click(function (event) {

                getOperatorList($("#txtYobinaEndOperatorSearch").val(), $("#ddlEndOperatorNameValue"));
            });

            $("#ddlModel").focus();

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function IsActiveMachine(machineCd) {

    if (IsExists == false && $("#ddlModel").val() == machineCd) {
        var focusFieldID = "ddlModel";
        var message = $("#HdfModelInactiveMessage").val();     //$.trim($("#lblErrorMessageE_STR_EDIT_DELETE_013").text());
        showMessageDialog(message, "ddlModel", WARNING);
        return false;
    }
    else {
        return true;
    }
}
function OperatorIsInactive(dropdownName) {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Chamffer/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            showMessageDialog(response.message, dropdownName, WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_CHA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function IsDuplicateThabaNo(chuuban, gyouban, $callFrom) {
    var thabaList = [];
    var Thaba1 = $("#txtThaba1").val();
    var Thaba2 = $("#txtThaba2").val();
    var Thaba3 = $("#txtThaba3").val();
    var Thaba4 = $("#txtThaba4").val();
    var t1 = "";
    var t2 = "";
    var t3 = "";
    var t4 = "";
    var Thaba_No_String = "";
    var chamferingCd = $("#hfld_ChamferingCd").val();
    var returnval = true;
    var thabamessage = "";
    var chk;
    thabaList.push(Thaba1);
    if (Thaba2 != "") {
        thabaList.push(Thaba2);
    }
    if (Thaba3 != "") {
        thabaList.push(Thaba3);
    }
    if (Thaba4 != "") {
        thabaList.push(Thaba4);
    }

    chk = new Set();
    var duplicateThaba = thabaList.filter(n => chk.size === chk.add(n).size);


    if (duplicateThaba.length > 0) {
        if (duplicateThaba.length > 1) {
            message = $("#lblErrorMessageE_CHA_EDIT_DELETE_002").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_CHA_EDIT_DELETE_002").text().trim().replace("NNNN", duplicateThaba[0]);;
        }

        showMessageDialog(message, "txtThaba1", WARNING);
        returnval = false;
        return false;
    }

    if (Thaba1 != null && Thaba1 != 0) {
        Thaba_No_String = Thaba1.toString();
    }
    if (Thaba2 != null && Thaba2 != 0) {
        Thaba_No_String = Thaba_No_String + "','" + Thaba2;
    }
    if (Thaba3 != null && Thaba3 != 0) {
        Thaba_No_String = Thaba_No_String + "','" + Thaba3;
    }
    if (Thaba4 != null && Thaba4 != 0) {
        Thaba_No_String = Thaba_No_String + "','" + Thaba4;
    }
    var rechamfer = ($('#chkReChamfering').is(':checked') ? 1 : 0);

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', Thaba_No_String: '" + Thaba_No_String + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonThabaDetailData + "}"
        data: JSON.stringify({
            PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, TABA_NO_STRING: Thaba_No_String, RECHAMFERING: rechamfer, CHAMFERING_CD: $("#hfld_ChamferingCd").val(),
            CH_TABA_NO_1: Thaba1 ? Thaba1 : null, CH_TABA_NO_2: Thaba2 ? Thaba2 : null, CH_TABA_NO_3: Thaba3 ? Thaba3 : null, CH_TABA_NO_4: Thaba4 ? Thaba4 : null
        })
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            var sucess = response.isSuccess
            if (sucess == false) {
                firstFieldID = "txtThaba1";
                message = $("#lblErrorMessageE_CHA_EDIT_DELETE_005").text().trim().replace("NNN", jsonResult);
                showMessageDialog(message, firstFieldID, WARNING);
                returnval = false;
            } else {
                returnval = true;
            }
            //var jsonThabaDetailData = [];
            //jsonThabaDetailData = response.message;
            //arrayData = [];
            //$.each(jsonThabaDetailData, function (index, jsonObject) {

            //    if (jsonObject.tabA_NO) {

            //        var thabaNumbers = jsonObject.tabA_NO.split(",");
            //        var cd = jsonObject.chamferinG_CD; 
            //        combinedDecisionOK = jsonObject.cH_DECISION;

            //        $.each(thabaNumbers, function (index, thabaNo) {
            //            thabaNo = thabaNo.trim();
            //            arrayData.push({ Thaba: thabaNo, chamferinG_CD: + cd, Decision: combinedDecisionOK });
            //        });
            //    }
            //});
            
            //for (var i = 0; i <= arrayData.length - 1; i++) {
            //    if (Thaba1 == arrayData[i].Thaba && arrayData[i].Decision == arrayData[i].Decision && arrayData[i].chamferinG_CD != chamferingCd && $("#chkReChamfering").prop("checked") == false) {
            //        t1 = Thaba1;

            //    } else {
            //        if (t1 == "") {
            //            t1 = "";
            //        }
            //    }

            //}

            //for (var i = 0; i <= arrayData.length - 1; i++) {
            //    if (Thaba2 == arrayData[i].Thaba && arrayData[i].Decision == arrayData[i].Decision && arrayData[i].chamferinG_CD != chamferingCd && $("#chkReChamfering").prop("checked") == false) {
            //        t2 = Thaba2;

            //    } else {
            //        if (t2 == "") {
            //            t2 = "";
            //        }
            //    }

            //}

            //for (var i = 0; i <= arrayData.length - 1; i++) {
            //    if (Thaba3 == arrayData[i].Thaba && arrayData[i].Decision == arrayData[i].Decision && arrayData[i].chamferinG_CD != chamferingCd && $("#chkReChamfering").prop("checked") == false) {
            //        t3 = Thaba3;

            //    } else {
            //        if (t3 == "") {
            //            t3 = "";
            //        }
            //    }

            //}

            //for (var i = 0; i <= arrayData.length - 1; i++) {
            //    if (Thaba4 == arrayData[i].Thaba && arrayData[i].Decision == arrayData[i].Decision && arrayData[i].chamferinG_CD != chamferingCd && $("#chkReChamfering").prop("checked") == false) {
            //        t4 = Thaba4;

            //    } else {
            //        if (t4 == "") {
            //            t4 = "";
            //        }
            //    }

            //}

            //if (t1 != "") {
            //    thabamessage = t1 + ", ";
            //}
            //if (t2 != "") {
            //    thabamessage = thabamessage + t2 + ", ";
            //}
            //if (t3 != "") {
            //    thabamessage = thabamessage + t3 + ", ";
            //}
            //if (t4 != "") {
            //    thabamessage = thabamessage + t4;
            //}
            //if (thabamessage != "") {
            //    firstFieldID = "txtThaba1";
            //    message = $("#lblErrorMessageE_CHA_EDIT_DELETE_005").text().trim().replace("NNN", thabamessage.replace(/(.+), $/, '$1'));    //lblErrorMessageE_STRAIGHTENING_ENTRY_005
            //    showMessageDialog(message, firstFieldID, WARNING);
            //    returnval = false;
            //}
            //else {

            //    returnval = true;
            //}

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblSuccessMessageS_CHA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);

        });

    return returnval;
}

function IsDropDownMandatory() {

    var returnVal = true;
    var message = "";
    if ($("#ddlStartOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_008").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_009").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }

    return returnVal;
}

$("#btnDelete").click(function (event) {
    
    DeleteChamferingDetails();
    $("#btnInitialize").focus();
});

$("#btnInitialize").click(function (event) {
    //$("#ddlModel").html("").append($('<option value="0"></option>'));
   initialize();
   //btnSearch_Click(true);
   switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    $("#txtChuubanSearch").focus();

});

//onclick='$("#tabs").tabs( "select", "tabs-2" )'

function fillEmptyGrid() {
    var emptyList = [];
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Chamffer/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divChamfferGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//$("#btnYobinaStartOpeartorSearch").click(function (event) {
//    var yobina = $.trim($("#txtYobinaStartOperatorSearch").val());
//    setStartOperatorList(yobina, "");
//});

//$("#btnYobinaEndOpeartorSearch").click(function (event) {
//    var yobina = $.trim($("#txtYobinaEndOperatorSearch").val());
//    setEndOperatorList(yobina, "");
//});

function disableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

function initialize() {
    window.resize(1090, 870);
    fillEmptyGrid();
    $("#txtChuubanSearch").val("");
    $("#txtCustomerName").val("");
    $("#txtProdNameCust").val("");
    $("#txtProdNameInt").val("");
    $("#txtDimDMin").val("");
    $("#txtDimDMax").val("");
    $("#txtShap").val("");
    $("#txtLengthSearchMin").val("");
    $("#txtLengthSearchMax").val("");
   
    $(".FocusDefault").focus();
    clearPanel($("#pnlModel"));
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearTable($("#grdvProductList"), "", "GridNormalRow");
    clearTable($("#grdvProductList"), "", "GridAlternateRow");
    clearTable($("#grdvChamferList"), "", "GridNormalRow");
    clearTable($("#grdvChamferList"), "", "GridAlternateRow");
    resetGlobalVariables();
    disableEditDeleteButtons();
    InitSortPopup();
   
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

function clearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
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
        url: APIURL + "/Chamffer/GetSysDateTime",
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
            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
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
        url: APIURL + "/Chamffer/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

            //$("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd hh:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function MachineIsInactive() {
    debugger;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Chamffer/MachineIsInactive?machineName=" + $("#ddlModel option:selected").text(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            showMessageDialog(response.message, "ddlModel", WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_CHA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function setChamferingDetails(jsonResult) {
    debugger;
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    enableEditDeleteButtons();
    combinedDecisionOK = jsonResult.decisioN_OK;
    debugger;
    $("#hdfMachineCD").val(jsonResult.machinE_CD);
    if (jsonResult.msG_CD == 0) {

        $("#hfld_MessageCD").val(null);
    } else {
        $("#hfld_MessageCD").val(jsonResult.msG_CD);
    }
    $("#hdfProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#hfld_ChamferingCd").val(jsonResult.chamferinG_CD);
    $("#hfld_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#txt_HostName").val(jsonResult.pP_NYUKA_CD);
   

    /*Check machine is Active/Inactive*/
    setMachineList();

    startOperatorCD = jsonResult.cH_START_OPERATOR_CD;
    operatorCD = startOperatorCD;
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), operatorCD);

    endOperatorCD = jsonResult.cH_END_OPERATOR_CD;
    operatorCD = endOperatorCD;
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), operatorCD);

   
    //$('#ddlModel option').each(function () {
    //    if (this.value == jsonResult.machinE_CD)
    //        IsExists = true;
    //});

    //// If machine is inactive then first add it in drop down
    //if (IsExists === false) {
    //    $("#ddlModel").append(new Option(jsonResult.cH_MACHINE_NAME, jsonResult.machinE_CD));
    //}

    $("#ddlModel").val(jsonResult.machinE_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.cH_CREATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.cH_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOUBAN);
    $("#txtDeliveryDateT1").val($.format.date(new Date(jsonResult.pP_DELIVERY_DATE_STRING), "yyyy/MM/dd"));
    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);
    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
    }
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) > 0) {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_KUBUN_STRING);
    }
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);  //PP_MATL_DIM_D_STRING
    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);    //PP_PROD_DIM_D_STRING
    $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);            //text(jsonResult.PP_NO_OF_UNITS + '' + jsonResult.PP_UNIT);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    debugger;
    
        $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
    $("#hfld_ChamferingCd").val(jsonResult.chamferinG_CD);
    //$("#txtStartDate").val($.format.date(new Date(jsonResult.cH_START_DATE_STRING), "yyyy/MM/dd hh:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtStartDate").val($.format.date(new Date(jsonResult.cH_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    // $("#txtStartTime").val($.format.date(new Date(jsonResult.cH_START_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#ddlStartOperatorNameValue").val(jsonResult.cH_START_OPERATOR_CD);
    $("#txtMetalicPlateCode").val(jsonResult.cH_METALLIC_PLATE_CODE);

    $("#txtChShape").val(jsonResult.cH_SHAPE);
    $("#txtFurnaceNum").val(jsonResult.furnacE_NO);

    if (jsonResult.cH_ACT_LENGTH == 0) {
        $("#txtActualLength").val("");
    }
    else {
        $("#txtActualLength").val(jsonResult.cH_ACT_LENGTH);
    }

    if ($.isNumeric(jsonResult.rechamfering) && parseInt(jsonResult.rechamfering) == 1) {

        $("#chkReChamfering").prop('checked', true);
    }
    else {
        $("#chkReChamfering").prop('checked', false);
    }

    //$("#txtEndDate").val($.format.date(new Date(jsonResult.cH_END_DATE_STRING), "yyyy/MM/dd hh:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtEndDate").val($.format.date(new Date(jsonResult.cH_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //$("#txtEndTime").val($.format.date(new Date(jsonResult.cH_END_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#ddlEndOperatorNameValue").val(jsonResult.cH_END_OPERATOR_CD);
    $("#ddlDecision").val(jsonResult.cH_DECISION);


    $("#txtFinishedBarCount").val(jsonResult.cH_FINISHED_BARS);
    $("#ddlUnitType").val(jsonResult.uniT_TYPE);
    $("#txtFinishedQuantity").val(jsonResult.cH_FINISHED_QTY);
    $("#ddlNextProcess").val(jsonResult.cH_NEXT_PROCESS);
    //$("#ddlStartOperatorNameValue").val(jsonResult.cH_START_OPERATOR_CD);
    //$("#ddlEndOperatorNameValue").val(toString(jsonResult.cH_END_OPERATOR_CD));


    //$('#ddlStartOperatorNameValue option').each(function () {
    //    if (this.value == jsonResult.cH_START_OPERATOR_CD)
    //        IsStartOprExists = true;
    //});

    // If start opr is inactive then first add it in drop down
    //if (IsStartOprExists === false)
    //    $("#ddlStartOperatorNameValue").append(new Option(jsonResult.cH_START_OPERATOR_NAME, jsonResult.cH_START_OPERATOR_CD));

    //$("#ddlStartOperatorNameValue").val(jsonResult.cH_START_OPERATOR_CD);
    //$("#ddlModel").val(jsonResult.STR_MACHINE_CD);

    /*Check end opr is Active/Inactive*/
    //$('#ddlEndOperatorNameValue option').each(function () {
    //    if (this.value == jsonResult.cH_END_OPERATOR_CD)
    //        IsEndOprExists = true;
    //});

    //// If end opr is inactive then first add it in drop down
    //if (IsEndOprExists === false)
    //    $("#ddlEndOperatorNameValue").append(new Option(jsonResult.cH_END_OPERATOR_NAME, jsonResult.cH_END_OPERATOR_CD));

    //$("#ddlEndOperatorNameValue").val(jsonResult.cH_END_OPERATOR_CD);



    $("#txtThaba1").val(jsonResult.cH_TABA_NO_1);

    if (jsonResult.cH_TABA_NO_2 == 0) {
        jsonResult.cH_TABA_NO_2 = "";
    }

    $("#txtThaba2").val(jsonResult.cH_TABA_NO_2);
    if (jsonResult.cH_TABA_NO_3 == 0) {
        jsonResult.cH_TABA_NO_3 = "";
    }
    $("#txtThaba3").val(jsonResult.cH_TABA_NO_3);
    if (jsonResult.cH_TABA_NO_4 == 0) {
        jsonResult.cH_TABA_NO_4 = "";
    }
    $("#txtThaba4").val(jsonResult.cH_TABA_NO_4);
    $("#txtRemarks").val(jsonResult.cH_REMARKS);
    setDecimalFormats();
 
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

function IsActiveMachine(machineCd) {

    if (IsExists == false && $("#ddlModel").val() == machineCd) {
        var focusFieldID = "ddlModel";
        var message = $("#HdfModelInactiveMessage").val();     //$.trim($("#lblErrorMessageE_STR_EDIT_DELETE_013").text());
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    else {
        return true;
    }
}

function IsActiveStartOpr(startOprCD) {

    if (IsStartOprExists == false && $("#ddlStartOperatorNameValue").val() == startOprCD) {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $("#hdfOperatorInactiveMessage").val();
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    else {
        return true;
    }
}

function IsActiveEndOpr(endOprCD) {

    if (IsEndOprExists == false && $("#ddlEndOperatorNameValue").val() == endOprCD) {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $("#hdfOperatorInactiveMessage").val();
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    else {
        return true;
    }
}


//function setStartOperatorList(yobina, defaultSelectID) {
//    //setOperatorList(yobina, $("#ddlStartOperatorNameValue"), defaultSelectID);
//    getOperatorList(yobina, $("#ddlStartOperatorNameValue"));
//};

//function setEndOperatorList(yobina, defaultSelectID) {
//    //setOperatorList(yobina, $("#ddlEndOperatorNameValue"), defaultSelectID);
//    getOperatorList(yobina, $("#ddlEndOperatorNameValue"));
//};

function getOperatorList(yobina, $target) {

    var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetOperatorList?name=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",

    }
    $.ajax(settings)
        .done(function (response) {


            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
            bindOperatorDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblSuccessMessageS_CHA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}

function bindOperatorDropdown(yobina, $target, jsonResult) {

    var dropdownName = $target.attr("id");

    if (dropdownName == "ddlStartOperatorNameValue") {
        var getResult = jsonResult;
        jsonResult = jsonResult.filter(row => row.iN_ACTIVE_FLAG == 0)
        var startOperatorList = getResult.filter(r => r.cd == startOperatorCD);

        if (startOperatorList.length > 0) {
            if (startOperatorList[0].iN_ACTIVE_FLAG == 1) {
                jsonResult.push({ cd: startOperatorList[0].cd, name: startOperatorList[0].name, yobina: startOperatorList[0].yobina, iN_ACTIVE_FLAG: startOperatorList[0].iN_ACTIVE_FLAG });
            }
        }
    } else {
        var getResult = jsonResult;
        jsonResult = jsonResult.filter(row1 => row1.iN_ACTIVE_FLAG == 0)
        var endOperatorList = getResult.filter(r1 => r1.cd == endOperatorCD);

        if (endOperatorList.length > 0) {
            if (endOperatorList[0].iN_ACTIVE_FLAG == 1) {
                jsonResult.push({ cd: endOperatorList[0].cd, name: endOperatorList[0].name, yobina: endOperatorList[0].yobina, iN_ACTIVE_FLAG: endOperatorList[0].iN_ACTIVE_FLAG });
            }
        }
    }

    $target.html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });

}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

function setDecimalFormats() {

    $("#txtActualLength").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });

    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtDimDMin").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtDimDMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthSearchMin").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtLengthSearchMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
}

function Mask() {
    $("#txtOperationDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtOperationDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });


    $("#txtDimDMin").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtDimDMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtLengthSearchMin").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtLengthSearchMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
}
function IsEndDateValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_CHA_EDIT_DELETE_003").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_CHA_EDIT_DELETE_004").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
}
function btnSearch_Click(allownull = false) {

    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val();
    var txtDimDMax = $("#txtDimDMax").val();
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val();
    var txtLengthSearchMax = $("#txtLengthSearchMax").val();
    var userId = $("#hdfLoginId").val();
    var checkFocusField = "";
    var errorMessage = "";

    if ($("#txtDimDMin").val().trim() == "" && $("#txtDimDMax").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_010").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_011").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_011").text());
        }
        
    }

    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }

    if (allownull || txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0) {

        ChamfferGridSearch(null);
        //var ajaxTime = new Date().getTime();
        ////var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        //var settings = {
        //    async: true,
        //    crossDomain: true,
        //    cache: false,
        //    dataType: "json",
        //    url: APIURL + "/Chamffer/GridSearch",
        //    method: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    data: JSON.stringify({
        //        CHUUBAN: txtChuubanSearch, CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin, PROD_DIM_D_MAX: txtDimDMax, SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax,
        //        USER_ID: userId
        //    })
        //}
        //$.ajax(settings)
        //    .done(function (response) {
        //        responseTime = new Date().getTime() - ajaxTime;
        //        if (response.isSuccess) {
        //            var settings1 = {
        //                async: true,
        //                crossDomain: true,
        //                cache: false,
        //                dataType: "json",
        //                url: "/Chamffer/GridSearch",
        //                method: "POST",
        //                contentType: "application/json; charset=utf-8",
        //                data: JSON.stringify(response.message)
        //                //data: "{jsonData : " + jsonData + "}"
        //            }
        //            $.ajax(settings1)
        //                .done(function (response1) {
        //                    if (response1.isSuccess) {
        //                        $("#divChamfferGridMain").html(response1.message);
        //                    }
        //                })
        //                .fail(function (xhr, textStatus, errorThrown) {
        //                    responseTime = new Date().getTime() - ajaxTime;
        //                    var errorCode = $.parseJSON(xhr.responseText).Message;

        //                    focusFieldID = $(':focus').attr("id");
        //                    message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
        //                    showMessageDialog(message, focusFieldID, ERROR);
        //                });
        //        }
        //        else {
        //            bootbox.alert({
        //                title: "<div style='color:white'></div>",
        //                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
        //                centerVertical: true
        //            });
        //        }

        //    })
        //    .fail(function (xhr, textStatus, errorThrown) {
        //        responseTime = new Date().getTime() - ajaxTime;
        //        var errorCode = $.parseJSON(xhr.responseText).Message;

        //        focusFieldID = $(':focus').attr("id");
        //        message = $.trim($("#lblSuccessMessageS_CHA_EDIT_DELETE_001").text());
        //        showMessageDialog(message, focusFieldID, ERROR);
        //    });
    }
    else {
        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_Chamffer_SearchField").text()) + "</td></tr></table>",

            centerVertical: true
        });
    }
}

function bindOperatorList(search) {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        // url: "/Stampping/EmployeeList?name=" + search,
        url: APIURL + "/Chamffer/EmployeeList?name=" + search,
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            $('#ddlNameValue').find('option')
                .remove()
                .end();
            for (var i = 0; i < response.message.length; i++) {
                $('#ddlNameValue').append($("<option></option>")
                    .attr("value", response.message[i].cd)
                    .text(response.message[i].name));
            }
            //$.each(response.message, function (key, value) {
            //    $('#ddlNameValue').find('option')
            //        .remove()
            //        .end()
            //        .append($("<option></option>")
            //            .attr("value", key)
            //            .text(value));
            //});
        })
}

$(document).on('click', '#CustomerSearch', function () {
    var name = $("#txtInputSearch").val();
    bindOperatorList(name);
})

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})


function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortAdd_Click() {
    var selectedColumns = $("#lstSortFields").val();
    if (selectedColumns.length > 0) {
        $("#lstSortFields option:selected").each(function () {
            var $this = $(this);
            if ($this.length) {
                var selText = $this.text();
                console.log(selText);
                $('#lstSelectedSortFields').append($("<option></option>")
                    .attr("value", $this.val())
                    .text(selText + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
                $this.remove();
            }
        });


        //for (var i = 0; i < selectedColumns.length; i++) {
        //    var text = $("#lstSortFields option[value='" + selectedColumns[i] + "']").text();
        //    $("#lstSortFields option[value='" + selectedColumns[i] + "']").remove();
        //    $('#lstSelectedSortFields').append($("<option></option>")
        //        .attr("value", selectedColumns[i])
        //        .text(text + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
        //}
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

function btnSortRemove_Click() {
    var selectedColumns = $("#lstSelectedSortFields").val();
    if (selectedColumns.length > 0) {
        var selectedColumns = $("#lstSelectedSortFields").val();
        if (selectedColumns.length > 0) {
            $("#lstSelectedSortFields option:selected").each(function () {
                var $this = $(this);
                if ($this.length) {
                    var selText = $this.text().split(' ');
                    console.log(selText);
                    selText.splice(-1);
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", $this.val())
                        .text(selText.join(' ')));
                    $this.remove();
                }
            });
        }
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

function btnSortOk_Click() {
    $("#btnSort").focus();
    var selectedColumns = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            selectedColumns.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });
    ChamfferGridSearch(selectedColumns);
}

function ChamfferGridSearch(sortFields) {

    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val() ? $("#txtDimDMin").val():null;
    var txtDimDMax = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var txtLengthSearchMax = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    var userId = $("#hdfLoginId").val();

    //if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0) {


        var ajaxTime = new Date().getTime();
        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Chamffer/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: txtChuubanSearch, CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin, PROD_DIM_D_MAX: txtDimDMax, PROD_SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax,
                USER_ID: userId, SORT_FIELDS: sortFields
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/Chamffer/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divChamfferGridMain").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                else {
                    bootbox.alert({
                        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                        message: response.message,
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    //}
    //else {

    //    if (sortFields.length > 0) {
    //        var ajaxTime = new Date().getTime();
    //        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
    //        var settings = {
    //            async: true,
    //            crossDomain: true,
    //            cache: false,
    //            dataType: "json",
    //            url: APIURL + "/Chamffer/GridSearch",
    //            method: "POST",
    //            contentType: "application/json; charset=utf-8",
    //            data: JSON.stringify({
    //                USER_ID: userId, SORT_FIELDS: sortFields
    //            })
    //        }
    //        $.ajax(settings)
    //            .done(function (response) {
    //                responseTime = new Date().getTime() - ajaxTime;
    //                if (response.isSuccess) {
    //                    var settings1 = {
    //                        async: true,
    //                        crossDomain: true,
    //                        cache: false,
    //                        dataType: "json",
    //                        url: "/Chamffer/GridSearch",
    //                        method: "POST",
    //                        contentType: "application/json; charset=utf-8",
    //                        data: JSON.stringify(response.message)
    //                        //data: "{jsonData : " + jsonData + "}"
    //                    }
    //                    $.ajax(settings1)
    //                        .done(function (response1) {
    //                            if (response1.isSuccess) {
    //                                $("#divChamfferGridMain").html(response1.message);

    //                            }
    //                        })
    //                        .fail(function (xhr, textStatus, errorThrown) {
    //                            responseTime = new Date().getTime() - ajaxTime;
    //                            var errorCode = $.parseJSON(xhr.responseText).Message;

    //                            focusFieldID = $(':focus').attr("id");
    //                            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
    //                            showMessageDialog(message, focusFieldID, ERROR);
    //                        });
    //                }
    //                else {
    //                    bootbox.alert({
    //                        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
    //                        message: response.message,
    //                        centerVertical: true
    //                    });
    //                }

    //            })
    //            .fail(function (xhr, textStatus, errorThrown) {
    //                responseTime = new Date().getTime() - ajaxTime;
    //                var errorCode = $.parseJSON(xhr.responseText).Message;

    //                focusFieldID = $(':focus').attr("id");
    //                message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
    //                showMessageDialog(message, focusFieldID, ERROR);
    //            });
    //    }
    //    //bootbox.alert({
    //    //    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
    //    //    message: $.trim($("#lblErrorMessageE_ColdDrawing_SearchField").text()),
    //    //    centerVertical: true
    //    //});
    //}
    closepopup();
}

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url:  "/Chamffer/InitializeSortPopup",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            $('#lstSortFields').find('option')
                .remove()
                .end();
            $('#lstSelectedSortFields').find('option')
                .remove()
                .end();
            for (var i = 0; i < response.message.length; i++) {
                if (response.message[i].isDefaultSort) {
                    $('#lstSelectedSortFields').append($("<option></option>")
                        .attr("value", response.message[i].fieldName)
                        .text(response.message[i].displayName + GetSortDirectionSuffix(response.message[i].sortOrder)));
                }
                else {
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", response.message[i].fieldName)
                        .text(response.message[i].displayName));
                }

            }
            EnableDisableSortButtons();
            $("#rdoSortAsc").prop("checked", true);
            $("#rdoSortDsc").prop("checked", false);
        })
}

function EnableDisableSortButtons() {
    var available = $("#lstSortFields option").length;
    var selected = $("#lstSelectedSortFields option").length;
    if (available > 0) {
        $("#SortPopUpModal #btnAdd").prop('disabled', false);
    }
    else {
        $("#SortPopUpModal #btnAdd").prop('disabled', true);
    }

    if (selected > 0) {
        $("#SortPopUpModal #btnRemove").prop('disabled', false);
    }
    else {
        $("#SortPopUpModal #btnRemove").prop('disabled', true);
    }
}

function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
   
}
$("#btnSort").focusout(function () {
    $("#btnLnkEdit").focus();
});
function LoadReSizeScreen() {

    window.resizeTo(1080, 750);
    document.getElementById('TabContainerPMS').style.height = '380px';

}

function setMachineList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetMachineList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindMachineDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMachineDropdown(jsonResult) {

    var getResult = jsonResult;
    jsonResult = jsonResult.filter(row2 => row2.iN_ACTIVE_FLAG == 0)
    var activeMachineList = getResult.filter(row3 => row3.cd == $("#hdfMachineCD").val());

    if (activeMachineList.length > 0) {
        if (activeMachineList[0].iN_ACTIVE_FLAG == 1) {
            jsonResult.push({ cd: activeMachineList[0].cd, name: activeMachineList[0].name, yobina: activeMachineList[0].yobina, iN_ACTIVE_FLAG: activeMachineList[0].iN_ACTIVE_FLAG });

        }
    }


    $("#ddlModel").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '" >' + jsonObj.name + '</option>'));

    });
}
function btnSortClose_Click() {
    $("#btnSort").focus();
    closepopup();
}

function closepopup() {
    $("#SortPopUpModal").modal('hide');
}

function setNextProcess() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_CHA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {

    $("#ddlNextProcess").html("").append($('<option value="0"></option>'));
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

function binddecisionDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
        url: "/Chamffer/GetDecision",
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
            message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
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
        url: "/Chamffer/GetUnitType",
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
            var errorCode = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$(document).ready(function () {

    //switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    //switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // window.moveTo(0, 0);

    $("#txtChuubanSearch").focus();
    window.resize(1090, 880);
    SetGlobalVariables();
    //setStartOperatorList("", "");
    //setEndOperatorList("", "");
    //setMachineList();
    setNextProcess();
    setDecision();
    setUnitType();

    $(".FocusDefault").focus();
    disableEditDeleteButtons();

    Mask();
    userName = $("#hdfUserName").val();
    //FreeseHeaders();
    InitSortPopup();
    //btnSearch_Click(true);
});

function switchTab($tabFrom, $tabTo) {

    $tabTo.click();
    //enableEditDeleteButtons();
}
$('#txtChuubanSearch').keypress(function (e) {
    var orderNo = $('#txtChuubanSearch').val() ? $('#txtChuubanSearch').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Chamffer/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuubanSearch').val(
                        jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN
                    );
                    $('#txtCustomerName').focus();
                } else {
                    $('#txtChuubanSearch').val("");
                   
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_CHAMFER_ENTRY_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});