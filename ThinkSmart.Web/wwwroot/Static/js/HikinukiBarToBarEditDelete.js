var $btnLnkEdit;
var combinedDecisionOK = "";
var indexDDLDecisionNothing = 0;
var disabled = "disabled";
var DISABLED = "disabled";
var HIDDEN = "Hidden"
var typeDropDown = "select-one";
var typeText = "text";
var $hdfSYSDATE;
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var oldHikinukiRemarks;

var IsExists = false;
var IsStartOprExists = false;
var IsEndOprExists = false;
var registeredThabaNumbersAndDecision = {};
var maxRegisteredThabaNo = 0;
var IsNGReason1 = false;
var IsNGReason2 = false;
var IsNGReason3 = false;
var lengthValue = true;
var finishedWeightValue = true;
var DimentionValue = true;
var activeMachineCD = "";
var startOperatorCD = "";
var endOperatorCD = "";
var operatorCD = 0;


// Error log information. To be sent with each AJAX request. 
userName = ""; // assigned at Document.ready
// End error log information.

function resetGlobalVariables() {

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
    registeredThabaNumbersAndDecision = {};
}
function SetGlobalVariables() {
    $btnLnkEdit = $("#btnLnkEdit");
    $hdfSYSDATE = $("#hdfSYSDATE");
}

function disableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

function disableStartDataPanel() {
    // Set StartData Panel Elements to Disabled
    $("#ddlModel").attr(DISABLED, DISABLED).addClass(DISABLED);
    
    var $startDataPanel = $("#tblProcessStartData");
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
    var $endDataPanel = $("#tblProcessEndData");
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
        url: APIURL + "/HikinukiBarToBar/GetSysDateTime",
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

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function MachineIsInactive() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/MachineIsInactive?machineName=" + $("#ddlModel option:selected").text(),
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
            message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function OperatorIsInactive(dropdownName) {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiBarToBar/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
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
            message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function getHikinukiDetailsByDrawingCD($callFrom) {

    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    //var hikinukiCD = $.trim($callFrom.closest("tr").find("#lblDrawingCD").text());
    var hikinukiCD = $.trim($callFrom.closest("tr").attr("data-lblDrawingCD"));
    var jsonData = "{DrawingCD :'" + hikinukiCD + "', USER_NAME: '" + userName + "' }";
    //setProcessDefectList();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetHikinukiDetailsByDrawingCD?DrawingCD=" + hikinukiCD + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            bindEvents();
            enableStartDataPanel();
            enableEndDataPanel();
           // SetLengthControlProperties($("#txtLengthT1").val());
            setHikinukiDetails(jsonResult);
           
            window.resizeTo(1090, 875);

            $("#btnUpdate").unbind("click");
            $("#btnUpdate").click(function (event) {
              
                if ($("#chkActualDim").prop("checked") == true) {

                    if ($("#txtActualDim1").val().trim() != "" && $("#txtActualDim2").val().trim() != "" && $("#txtActualDim3").val().trim() != "") {
                        $("#checkActualDim").removeClass("Mandatory");
                    } else {
                        $("#checkActualDim").addClass("Mandatory");
                    }

                } else {
                    $("#checkActualDim").removeClass("Mandatory");
                }

                if ($("#chkActualLength").prop("checked") == true) {
                    if ($("#txtActualLength1").val().trim() != "" && $("#txtActualLength2").val().trim() != "" && $("#txtActualLength3").val().trim() != "") {
                        $("#checkActualLength").removeClass("Mandatory");
                    } else {
                        $("#checkActualLength").addClass("Mandatory");
                    }
                } else {
                    $("#checkActualLength").removeClass("Mandatory");
                }



                if (!IsEndDataValid()) {
                    return false;
                }
                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }
                if (!checkDate($("#txtStartDate"))) {
                    var message = $("#lblErrorMessageE_Hikinuki_EditDelete_027").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtStartDate", WARNING);
                    return false;
                }

                if (!checkDate($("#txtEndDate"))) {
                    var message = $("#lblErrorMessageE_Hikinuki_EditDelete_028").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtEndDate", WARNING);
                    return false;
                }

                if (jsonResult.pP_GYOUBAN.toString().length == 1) {
                    jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
                }

                //Check thaba no is duplicate
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

                        var message = $("#lblErrorMessageE_Hikinuki_EditDelete_029").text().trim(); //ActDimOutOfTolerance
                        showMessageDialog(message, focusText, WARNING);
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
                        var message = $("#lblErrorMessageC_Hikinuki_EditDelete_030").text().trim(); // LengthOutOfTolerance
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


                if ($("#chkFinishedQuantity").prop("checked") == true) {
                    
                    if (["PRODUCT_SHAPE", "PRODUCT_DIM", "PRODUCT_LENGTH"].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== ""))
                    {

                        var value = $("#txtLengthT1").val();
                        // Check - Length only allow numeric &  Unit Type - Not allow BN 

                        if (!IsNotNumeric(value) || value.trim() == "" || $("#ddlUnitType").val() == 2) {

                            var message = $("#lblConfirmationMessageC_Hikinuki_EditDelete_033").text().trim();
                            showMessageDialog(message, "chkFinishedQuantity", WARNING);
                            return false;
                        }

                    } else
                    {
                        var message = $("#lblConfirmationMessageC_Hikinuki_EditDelete_033").text().trim();
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
                    getResponse = IsFinishedQuantityValid(weightValues);
                } else {
                    getResponse["IsSuccess"] = true;
                }

                
                if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true)
                {
                    //var message = "";

                    //message = $.trim($("#lblConfirmationMessageC_Hikinuki_EditDelete_032").text());

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
                    //            var message = $("#lblErrorMessageE_Hikinuki_EditDelete_031").text().trim(); // FinishedQtyNotIn2PercentRange
                    //            showMessageDialog(message, "txtFinishedQuantity", WARNING);

                    //        }
                    //    },
                    //    centerVertical: true
                    //});
                    var message = $("#lblErrorMessageE_Hikinuki_EditDelete_031").text().trim(); // FinishedQtyNotIn2PercentRange
                    showMessageDialog(message, "txtFinishedQuantity", WARNING);
                   
                } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false)
                {
                    var message = "";
                   
                        message = $.trim($("#lblConfirmationMessageC_Hikinuki_EditDelete_032").text());
                   
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
                        callback: function (getVal) {
                            if (getVal) {
                                updateHikinukiDetails();
                                btnSearch_Click();
                            }
                        },
                        centerVertical: true
                    });
                }
                else {
                    updateHikinukiDetails();
                    btnSearch_Click();
                }

            });

            $("#btnStartDateTime").click(function (event) {
                setStartSysDateTime();
            });
            $("#btnEndDateTime").click(function (event) {
                setEndSysDateTime();
            });

            $("#ddlModel").focus();
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function IsNotNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
    return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
}
$("#TbPMSEditDelTab1-tab").click(function () {
    window.resizeTo(1090, 890);
});
$("#TbPMSEditDelTab2-tab").click(function () {
    window.resizeTo(1090, 875);
});

$('#chkActualDim').click(function () {

    if ($("#chkActualDim").prop("checked") == true) {
        DimentionValue = false;
        $("#checkActualDim").addClass("Mandatory");

        $("#txtActualDim1").addClass("Mandatory1");
        $("#txtActualDim2").addClass("Mandatory1");
        $("#txtActualDim3").addClass("Mandatory1");
    } else {
        DimentionValue = true;
        $("#chkActualDim").prop("checked", false)

        $("#checkActualDim").removeClass("Mandatory");
        $("#txtActualDim1").removeClass("Mandatory1");
        $("#txtActualDim2").removeClass("Mandatory1");
        $("#txtActualDim3").removeClass("Mandatory1");
    }
});

$('#chkActualLength').click(function () {

    if ($("#chkActualLength").prop("checked") == true) {
        lengthValue = false;
        $("#checkActualLength").addClass("Mandatory");

        $("#txtActualLength1").addClass("Mandatory1");
        $("#txtActualLength2").addClass("Mandatory1");
        $("#txtActualLength3").addClass("Mandatory1");
    } else {
        lengthValue = true;
        $("#chkActualLength").prop("checked", false)

        $("#checkActualLength").removeClass("Mandatory");
        $("#txtActualLength1").removeClass("Mandatory1");
        $("#txtActualLength2").removeClass("Mandatory1");
        $("#txtActualLength3").removeClass("Mandatory1");
    }
});

$('#chkFinishedQuantity').click(function () {

    if ($("#chkFinishedQuantity").prop("checked") == true) {
        finishedWeightValue = false;
    } else {
        finishedWeightValue = true;
    }
});


function switchTab($tabFrom, $tabTo) {

    $tabTo.click();

    enableEditDeleteButtons();
}

function SetLengthControlProperties(lenghtVal) {
  
    //var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
    //if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
    //    $("#chkActualLength").removeAttr("disabled", "disabled");

    //} else {
    //    $("#chkActualLength").attr("disabled", "disabled");
    //    $("#chkActualLength").prop("checked", false);

    //}
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
function setHikinukiDetails(jsonResult) {
    
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    $("#txtYobinaStartOperatorSearch").val("");  
    $("#txtYobinaEndOperatorSearch").val(""); 

    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#hdfMachineCD").val(jsonResult.machinE_CD);
    
    $("#hdfProcessPlanCD").val(jsonResult.procesS_PLAN_CD);

    $("#hfld_MessageCD").val(jsonResult.msG_CD);
    $("#hfld_DrawingCd").val(jsonResult.drawinG_CD);

    $("#hdfLoginId").val(jsonResult.useR_ID);

    $("#HiGyouban").text(jsonResult.pP_GYOUBAN);
    /*Check machine is Active/Inactive*/
    setMachineList();

    startOperatorCD = jsonResult.dW_START_OPERATOR_CD;
    operatorCD = startOperatorCD;
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), operatorCD);

    endOperatorCD = jsonResult.dW_END_OPERATOR_CD;
    operatorCD = endOperatorCD;
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), operatorCD);

    $("#ddlModel").val(jsonResult.machinE_CD);

    DRAWING_CD = $.isNumeric(jsonResult.drawinG_CD) ? parseInt(jsonResult.drawinG_CD) : 0;

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
  
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
   
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOUBAN);

    $("#txtDeliveryDateT1").val(jsonResult.pP_DELIVERY_DATE_STRING);

    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);

    $("#txt_HostName").val(jsonResult.producT_NYUKO_CD);

    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);

    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);

    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);

    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
    }
    if (jsonResult.pP_UNIT_WT_STRING !="") {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_STRING);
    }

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

    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);

    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);

    if (jsonResult.pP_DIM_TOL_MIN != null)
        dimTolMin = jsonResult.pP_DIM_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_DIM_TOL_MAX != null)
        dimTolMax = jsonResult.pP_DIM_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    dimStd = jsonResult.pP_PROD_DIM_D;

    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);

    $("#txtStampT1").val(jsonResult.pP_STAMP);

    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);

    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    }

    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
   
    if (jsonResult.pP_LENGTH_TOL_MIN != null)
        lenTolMin = jsonResult.pP_LENGTH_TOL_MIN.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    if (jsonResult.pP_LENGTH_TOL_MAX != null)
        lenTolMax = jsonResult.pP_LENGTH_TOL_MAX.replace(/\s+/g, ''); // REGEX to remove whitespaces. Otherwise, it fails in mathematical calculations.
    lenStd = jsonResult.pP_PROD_LENGTH;

    $("#txtPackingT1").val(jsonResult.pP_PACKING);

    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);

    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);

    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);

    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);

    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);

    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);

    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
  
    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);

    $("#txtStartOperator").val(jsonResult.dW_START_OPERATOR_NAME);
    $("#hdfStartOperatorCD").val(jsonResult.dW_START_OPERATOR_CD);

    $("#txtMetallicLabelNo").val(jsonResult.dW_METALLIC_PLATE_CODE);
    $("#txtDieDim").val(jsonResult.diE_DIM);
   // $("#ddlDieShape").val(jsonResult.diE_SHAPE || $("#ddlDieShape").prop('selectedIndex'));
    $("#ddlDieShape option:contains(" + jsonResult.diE_SHAPE + ")").prop('selected', true);
    $("#txtDieNo").val(jsonResult.diE_NO);

    $("#ddlProcCondition").val(jsonResult.hT_CONDITION);
    $("#txtProcCondition").val(jsonResult.hT_CONDITION);
    
  
    if (jsonResult.iM_DRAWING == "1") {
        $("#chkmiddle").prop('checked', true);
        disableRedrawCheckbox();
    }
    else {
        $("#chkmiddle").prop('checked', false);
        enableRedrawCheckbox();
    }
    if (jsonResult.redrawing == "1") {
        $("#chkREDRAWING").prop('checked', true);
    }
    else {
        $("#chkREDRAWING").prop('checked', false);
    }
   
    // If start opr is inactive then first add it in drop down
    $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    
   // $("#ddlStartOperatorNameValue").val(jsonResult.dW_START_OPERATOR_CD);

    $("#txtEndDate").val($.format.date(new Date(jsonResult.dW_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
  

  //  $("#ddlEndOperatorNameValue").val(jsonResult.dW_END_OPERATOR_CD);
    
    if (jsonResult.dW_ACT_DIM_CHK == "1") {
        $("#chkActualDim").prop('checked', true);

        $("#txtActualDim1").val(jsonResult.dW_ACT_DIM1).addClass("Mandatory1");
        $("#txtActualDim2").val(jsonResult.dW_ACT_DIM2).addClass("Mandatory1");
        $("#txtActualDim3").val(jsonResult.dW_ACT_DIM3).addClass("Mandatory1");
    }
    else {
        $("#chkActualDim").prop('checked', false);

        $("#checkActualDim").removeClass("Mandatory");
        $("#txtActualDim1").val(jsonResult.dW_ACT_DIM1).removeClass("Mandatory1");
        $("#txtActualDim2").val(jsonResult.dW_ACT_DIM2).removeClass("Mandatory1");
        $("#txtActualDim3").val(jsonResult.dW_ACT_DIM3).removeClass("Mandatory1");
    }
    SetLengthControlProperties($("#txtLengthT1").val());
  
    if (jsonResult.dW_ACT_LENGTH_CHK == "1") {
        $("#chkActualLength").prop('checked', true);
        $("#txtActualLength1").val(jsonResult.dW_ACT_LENGTH1);
        $("#txtActualLength2").val(jsonResult.dW_ACT_LENGTH2);
        $("#txtActualLength3").val(jsonResult.dW_ACT_LENGTH3);
        $("#checkActualLength").addClass("Mandatory");
        $("#txtActualLength1").addClass("Mandatory1");
        $("#txtActualLength2").addClass("Mandatory1");
        $("#txtActualLength3").addClass("Mandatory1");
    }
    else {
        $("#chkActualLength").prop('checked', false);

       
        $("#txtActualLength1").val(jsonResult.dW_ACT_LENGTH1);
        $("#txtActualLength2").val(jsonResult.dW_ACT_LENGTH2);
        $("#txtActualLength3").val(jsonResult.dW_ACT_LENGTH3);
        $("#checkActualLength").removeClass("Mandatory");
        $("#txtActualLength1").removeClass("Mandatory1");
        $("#txtActualLength2").removeClass("Mandatory1");
        $("#txtActualLength3").removeClass("Mandatory1");
    }
    $("#txtFinishedBarCount").val(jsonResult.dW_FINISHED_BARS);
    $("#ddlUnitType").val(jsonResult.uniT_TYPE);
    $("#txtFinishedQuantity").val(jsonResult.dW_FINISHED_QTY);

    if (jsonResult.dW_FINISHED_QTY_CHK == "1") {
        $("#chkFinishedQuantity").prop('checked', true);
    }
    else {
        $("#chkFinishedQuantity").prop('checked', false);
    }

    $("#ddlExternalCondition").val(jsonResult.dW_EXT_TEXTURE_STATUS);
    $("#ddlBendStatus").val(jsonResult.dW_BEND_STATUS);
    $("#txtThaba").val(jsonResult.dW_TABA_NO_);
    
   
    $("#txtrepaircount").val(jsonResult.dW_CORRECTED_BARS);
    $("#txtdisposalbars").val(jsonResult.dW_DISPOSAL_NO);

    $("#ddlDecision").val(jsonResult.dW_DECISION);

    $("#ddlNextProcess").val(jsonResult.dW_NEXT_PROCESS);

    $("#txtdefectcount").val(jsonResult.dW_NG_BARS1);
    if (jsonResult.dW_NG_BARS1 > 0) {
        $("#ddlNGReason1").val(jsonResult.dW_NG_DETAILS1)
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#ddlNGReason1").removeAttr("disabled")
    }
    else {
        $("#txtdefectcount").val(jsonResult.DW_NG_DETAILS1);
        ChangeControlDisplay('#ddlNGReason1', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlNGReason1").attr("disabled", "disabled")
        $("#txtdefectcount").val("");
    }

    $("#txtdefectcount1").val(jsonResult.dW_NG_BARS2);
    if (jsonResult.dW_NG_BARS2 > 0) {
        $("#ddlNGReason2").val(jsonResult.dW_NG_DETAILS2)
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#txtdefectcount1").removeAttr("disabled")
    }
    else {
        $("#txtdefectcount1").val(jsonResult.dW_NG_BARS2);
        ChangeControlDisplay('#ddlNGReason2', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlNGReason2").attr("disabled", "disabled")
        $("#txtdefectcount1").val("");
    }

    $("#txtdefectcount2").val(jsonResult.dW_NG_BARS3);
    if (jsonResult.dW_NG_BARS3 > 0) {
        $("#ddlNGReason3").val(jsonResult.dW_NG_DETAILS3)
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#txtdefectcount2").removeAttr("disabled")
    }
    else {
        $("#txtdefectcount2").val(jsonResult.dW_NG_BARS3);
        ChangeControlDisplay('#ddlNGReason3', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlNGReason3").attr("disabled", "disabled")
        $("#txtdefectcount2").val("");
    }
  
    $("#txtRemarks").val(jsonResult.dW_REMARKS);

    dimTolMin = jsonResult.pP_DIM_TOL_MIN;
    dimTolMax = jsonResult.pP_DIM_TOL_MAX;
    dimStd = jsonResult.pP_PROD_DIM_D;
    lenTolMin = jsonResult.pP_LENGTH_TOL_MIN;
    lenTolMax = jsonResult.pP_LENGTH_TOL_MAX;
    lenStd = jsonResult.pP_PROD_LENGTH;
    setDecimalFormats();
    $(".FocusDefault").focus();
    oldHikinukiRemarks = $.trim(jsonResult.dW_REMARKS);
}
$("#chkmiddle").click(function () {
  
    if ($("#chkmiddle").is(":checked") == true) {
        disableRedrawCheckbox();
        $("#txtActualLength1").removeClass("Mandatory1");
        $("#txtActualLength2").removeClass("Mandatory1");
        $("#txtActualLength3").removeClass("Mandatory1");
        $("#chkActualDim").click();
    } else {
        enableRedrawCheckbox();
        $("#chkActualDim").click();
        SetLengthControlProperties($("#txtLengthT1").val());
    }
});
$("#chkREDRAWING").click(function () {
    if ($("#chkREDRAWING").is(":checked") == true) {
        $("#chkmiddle").prop("checked", false);
        $("#chkmiddle").attr(DISABLED, DISABLED).addClass(DISABLED);

    } else {
        $("#chkmiddle").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    }


});

function enableRedrawCheckbox() {
    $("#chkREDRAWING").prop("checked", false);
    $("#chkActualDim").prop("checked", false);
    $("#chkActualLength").prop("checked", false);
    $("#chkActualLength").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    $("#chkREDRAWING").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    if ($("#txtEndDate").hasClass("disabled") != true) {
        $("#chkActualDim").removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
    }
}
function disableRedrawCheckbox() {
    $("#chkREDRAWING").prop("checked", false);
    $("#chkActualDim").prop("checked", false);
    $("#chkActualLength").prop("checked", false);
    $("#chkActualLength").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#chkREDRAWING").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#chkActualDim").attr(DISABLED, DISABLED).addClass(DISABLED);
}
function bindEvents() {
    setHTMaterialList();
    setProcessDefectList();
    setNextProcess();
    setUnitType();
    setDecision();
    setExternalCondition();
    setBendStatus();
    setDieDiaShape();
}


    $("#btnInitialize").click(function (event) {
        switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
        initialize();
        window.resizeTo(1090, 890);
        location.reload();
    });


$("#btnDelete").click(function (event) {

    deleteHikinukiDetails();
    btnSearch_Click();
});

function deleteHikinukiDetails() {

    var ajaxTime = new Date().getTime();
        
    var jsonData = JSON.stringify({
        DRAWING_CD: $("#hfld_DrawingCd").val(),
        PP_PROCESS_PLAN_CD: $("#hdfProcessPlanCD").val(),
        DW_TABA_NO_: $("#txtThaba").val(),
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null

    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/HikinukiDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            //clearPanel($("#pnlProductDetails"));
            //clearPanel($("#pnlProcessStartData"));
            //clearPanel($("#pnlProcessEndData"));
            //resetGlobalVariables();
            disableEditDeleteButtons();
            // $("#btnSearch").trigger("click");
            var successMessage = $("#lblSuccessMessageS_Hikinukibartobar_EDIT_DELETE_002").text();
            showMessageDialog(successMessage, "", SUCCESS);
            //switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"))

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

//function setStartOperatorList(yobina, defaultSelectID) {
//    getOperatorList(yobina, $("#ddlStartOperatorNameValue"), defaultSelectID);
//};

//function setEndOperatorList(yobina, defaultSelectID) {
//    getOperatorList(yobina, $("#ddlEndOperatorNameValue"), defaultSelectID);
//};
function updateHikinukiDetails() {
    
    var ajaxTime = new Date().getTime();
    var jsonDataForUpdate = JSON.stringify({
        DRAWING_CD: $("#hfld_DrawingCd").val() ? $("#hfld_DrawingCd").val() : null,
        //PROCESS_PLAN_CD: parseInt($("#hdfldProcessPlanCd").val(),
        PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),
        MACHINE_CD: $("#ddlModel").val() ? $("#ddlModel").val() : null,
        MSG_CD: parseInt($("#hfld_MessageCD").val()),
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR_CD: $("#ddlStartOperatorNameValue").val() ? $("#ddlStartOperatorNameValue").val() : null,
        DW_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        DIE_DIM: $("#txtDieDim").val() ? $("#txtDieDim").val() : null,
        DIE_SHAPE: $("#ddlDieShape option:selected").text() ? $("#ddlDieShape option:selected").text() : null,
        DIE_NO: $("#txtDieNo").val() ? $("#txtDieNo").val() : null,
        HT_CONDITION: $("#txtProcCondition").val() ? $("#txtProcCondition").val() : null,
        IM_DRAWING: ($('#chkmiddle').is(':checked') ? 1 : 0), 
        REDRAWING: ($('#chkREDRAWING').is(':checked') ? 1 : 0),

        DW_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_END_OPERATOR_CD: $("#ddlEndOperatorNameValue").val() ? $("#ddlEndOperatorNameValue").val() : null,
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
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/HikinukiBarToBarDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
          
            if (parseInt(response.message) > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }

            var successMessage = $("#lblSuccessMessageS_Hikinukibartobar_EDIT_DELETE_001").text();
            showMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
        var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_007").text().trim();
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_ENTRY_009").text().trim();
        var focusFieldID = "txtEndTime";
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

function IsDuplicateThabaNo(chuuban, gyouban, $callFrom) {
  
    var returnVal = true;
    var Thaba_No = $("#txtThaba").val();
    var drawingCd = $("#hfld_DrawingCd").val();
    $callFrom.data('requestRunning', false)
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, TABA_NO: Thaba_No
        })
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
          
            //var reRedrawingList = jsonResult.filter(function (r) {
            //    return r.redrawing == 0;
            //});
            //if (jsonResult == 0) {
            //    returnVal = true;
            //} else {
            //  
            //    if ((reRedrawingList.length == 0) || (reRedrawingList[0].drawinG_CD == drawingCd) || (reRedrawingList[0].drawinG_CD != drawingCd && $("#chkREDRAWING").is(":checked") == true)) {
            //        returnVal = true;
            //    }
            //    else {
            //        message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EDIT_DELETE_019").text().trim().replace("NNN", Thaba_No);
            //        showMessageDialog(message, "txtThaba", WARNING);
            //        returnVal = false;
            //    }
            //}
          
            if (jsonResult == null || jsonResult.length == 0) {
                returnVal = true;
            } else {
                if (jsonResult.drawinG_CD == drawingCd || jsonResult.drawinG_CD != drawingCd && $("#chkREDRAWING").is(":checked") == true) {
                    returnVal = true;
                } else {
                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EDIT_DELETE_019").text().trim().replace("NNN", Thaba_No);
                    showMessageDialog(message, "txtTabaNo", WARNING);

                    returnVal = false;
                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
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
            message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

}

function setThabaDetails(jsonResult) {

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
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".Mandatory"), function (DISABLED, mandatoryField) {

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

function initialize() {
    combinedDecisionOK = "";
    activeMachineCD = "";
    startOperatorCD = "";
    endOperatorCD = "";
    operatorCD = 0;
    $(".FocusDefault").focus();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#tblProcessStartData"));
    clearPanel($("#tblProcessEndData"));
    clearTable($("#grdvProductList"), "", "GridNormalRow");
    clearTable($("#grdvProductList"), "", "GridAlternateRow");
    resetGlobalVariables();
    disableEditDeleteButtons();
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

function setDecimalFormats() {
    $("#txtAEdgeDistortion1, #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtBEdgeDistortion1, #txtBEdgeDistortion2, #txtBEdgeDistortion3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 1, 1);
    });

    $("#txtUpperLowerRollerAngle1, #txtUpperLowerRollerAngle2, #txtCompressionTonnage, #txtHexR").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });

    $("#txtC, #txtMn, #txtUpperRoleElevation, #txtInspectionCoilSize, #txtMagnetism").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtDrawDim1, #txtDrawDim2, #txtActualDim1, #txtActualDim2, #txtActualDim3, #txtActualDim4, #txtActualDim5, #txtActualDim6").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtActualLength1, #txtActualLength2, #txtActualLength3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

    $("#txtFinishedQuantity, #txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
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

}
const fixedWidth1 = 1090;
const fixedHeight1 = 890;

const fixedWidth2 = 1090;
const fixedHeight2 = 875;

let resizeTimer;

window.addEventListener("resize", function () { // alert popup shown if screen resize
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function () {
        if ((window.outerWidth !== fixedWidth1 || window.outerHeight !== fixedHeight1) && $("#TbPMSEditDelTab1-tab").hasClass("active")) {
            var message = $.trim($("#lblErrorMessageE_Hikinuki_EditDelete_034").text());
            showMessageDialog(message, "", WARNING);
        } else if ((window.outerWidth !== fixedWidth2 || window.outerHeight !== fixedHeight2) && $("#TbPMSEditDelTab2-tab").hasClass("active")) {
            var message = $.trim($("#lblErrorMessageE_Hikinuki_EditDelete_034").text());
            showMessageDialog(message, "", WARNING);
        }
    }, 300); // Fires after resizing stops
});

$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // CheckNumberValidation();
    SetGlobalVariables();
    // SetMaxLengthAndFormat();

    window.resizeTo(1090, 890);
    //setStartOperatorList("", "");
    //setEndOperatorList("", "");

   
    disableStartDataPanel();
    disableEndDataPanel();

    setDecimalFormats();
    disableEditDeleteButtons();

    userName = $("#hdfUserName").val();
    Mask();
    //FreeseHeaders();
    $("#txtChuubanSearch").focus();
});


//***************************************************
//****************************************************

function btnSearch_Click() {

    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var PROD_DIM_D_MAX = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    //var ddlNameValue = $("#ddlNameValue").val();
    var USER_ID = $("#hdfLoginId").val();
    var sortFields = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            sortFields.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });
    var errorMessage = "";
    var checkFocusField = "";

    if ($("#txtDimDMin").val().trim() == "" && $("#txtDimDMax").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_DimDStartRequired").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_LengthStartRequired").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_LengthStartRequired").text());
        }
    }

    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }

    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || parseInt(PROD_DIM_D_MIN) > 0 || parseInt(PROD_DIM_D_MAX) > 0 || PROD_SHAPE.length > 0 || parseInt(PROD_LENGTH_MIN) > 0 || parseInt(PROD_LENGTH_MAX) > 0) {
        HikinukiGridSearch(sortFields)
    }
    else {
        message = $.trim($("#lblErrorMessage_Hikinuki_SearchField").text());
        showMessageDialog(message, 'txtChuubanSearch', WARNING);
    }
}



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
    HikinukiGridSearch(selectedColumns);
}

function HikinukiGridSearch(sortFields) {

    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : 0;
    var PROD_DIM_D_MAX = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : 0;
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : 0;
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : 0;
    var USER_ID = $("#hdfLoginId").val();
  
    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PROD_DIM_D_MIN.length > 0
        || PROD_DIM_D_MAX.length > 0 || PROD_SHAPE.length > 0 || PROD_LENGTH_MIN.length > 0 || PROD_LENGTH_MAX.length > 0) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HikinukiBarToBar/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX, PROD_SHAPE: PROD_SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX,
                USER_ID: USER_ID, SORT_FIELDS: sortFields
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
                        url: "/HikinukiBarToBar/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divHikinukiGridMain").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                            return false;
                        });
                }
                else {
                    bootbox.alert({
                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }
    else {

        if (sortFields.length > 0) {
            var ajaxTime = new Date().getTime();
            //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
            var settings = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/HikinukiBarToBar/GridSearch",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    USER_ID: USER_ID, SORT_FIELDS: sortFields
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
                            url: "/HikinukiBarToBar/GridSearch",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)
                            //data: "{jsonData : " + jsonData + "}"
                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#divHikinukiGridMain").html(response1.message);

                                }
                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
                                showMessageDialog(message, focusFieldID, ERROR);
                                return false;
                            });
                    }
                    else {
                        bootbox.alert({
                            title: "<div style='color:white'></div>",
                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
                            centerVertical: true
                        });
                    }

                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                    return false;
                });
        }
    }
    closepopup();
}

function btnSortClose_Click() {
    closepopup();
}
function closepopup() {
    $("#SortPopUpModal").modal('hide');
}

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/HikinukiBarToBar/InitializeSortPopup",
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


function LoadReSizeScreen() {

    window.resizeTo(1080, 750);
    document.getElementById('TabContainerPMS').style.height = '380px';

}

function DefaultReSizeScreen() {

    window.resizeTo(1080, 855);
}

function setMachineList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetMachineList?machineType=60",
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });
}

function IsDropDownMandatory() {
    var returnVal = true;
    var message = "";
    if ($("#ddlStartOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_Hikinuki_EditDelete").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_Hikinuki_EditDelete").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    return returnVal;
}

function enableStartDataPanel() {

    $("#ddlModel").removeClass(disabled).removeAttr(disabled);
    var $startDataPanel = $("#tblProcessStartData");

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

    var $endDataPanel = $("#tblProcessEndData");

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

    //$("#txtEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //$("#txtEndTime").mask("99:99", { placeholder: "hh:mm" });
}

function setHTMaterialList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:"/HikinukiBarToBar/GetHTMaterial",
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindHTMaterialDropdown(jsonResult) {
    //$("#listHtMatl").html("").append($('<option value="kkk"></option>'));
    //$.each(jsonResult, function (key, jsonObj) {


    //    $("#listHtMatl").append($('<option value="' + jsonObj.name + '" </option>'));

    //});
    //if ($("#hdfCultureName").val() == "en") {

    //        document.querySelector('input[list="listHtMatl"]').value = "No";

    //}
    //else {

    //        document.querySelector('input[list="listHtMatl"]').value = "無し";

    //}

    $("#ddlProcCondition").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcCondition").append($('<option value="' + jsonObj.name + '">' + jsonObj.name + '</option>'));
        if ($("#txtProcCondition").val() == "") {
            $("#txtProcCondition").val(jsonObj.name);
        }
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
    } else
    {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }
}

function setNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
            var errorCode = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindUnitTypeDropdown(jsonResult) {
    $("#ddlUnitType").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
            bindBendStatusdropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindBendStatusdropdown(jsonResult) {
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
            message = $.trim($("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text());
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
            url: APIURL + "/HikinukiBarToBar/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuubanSearch').val(jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#txtCustomerName').focus();
                } else {
                    $('#txtChuubanSearch').val("");
                    
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});