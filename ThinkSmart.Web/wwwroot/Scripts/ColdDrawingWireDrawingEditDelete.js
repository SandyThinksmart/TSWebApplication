
var $btnLnkEdit;
var combinedDecisionOK = 1;
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";
var $hdfSYSDATE;
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var EditCheck = "";
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
var isStamppingRemarksUpdate = 0;
var IsExists = false;

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
    isStamppingRemarksUpdate = 0;
}
function SetGlobalVariables() {
    $btnLnkEdit = $("#btnLnkEdit");
    $btnUpdate = $("#btnUpdate");
    $btnDelete = $("#btnDelete");
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

function toggleActualLengthFieldsMandatory($this) {                     //Uncommented by sagar B on 18/8/2017

    if ($this.is(':checked')) {
        $("#txtActualLength1").addClass("Mandatory");
        $("#txtActualLength2").addClass("Mandatory");
        $("#txtActualLength3").addClass("Mandatory");
    } else {
        $("#txtActualLength1").removeClass("Mandatory");
        $("#txtActualLength2").removeClass("Mandatory");
        $("#txtActualLength3").removeClass("Mandatory");
    }
}

function toggleActualDimFieldsMandatory($this) {

    if ($this.is(':checked')) {
        $("#txtActualDim1").addClass("Mandatory");
        $("#txtActualDim2").addClass("Mandatory");
        $("#txtActualDim3").addClass("Mandatory");
    } else {
        $("#txtActualDim1").removeClass("Mandatory");
        $("#txtActualDim2").removeClass("Mandatory");
        $("#txtActualDim3").removeClass("Mandatory");
        $("#txtActualDim4").removeClass("Mandatory");
        $("#txtActualDim5").removeClass("Mandatory");
        $("#txtActualDim6").removeClass("Mandatory");
    }
    //SetActDimD456();
}

function toggleECOnlineFieldsMandatory($this) {
    if ($this.is(':checked')) {
        $("#txtShikii").removeAttr("disabled").val(txtShikii);
        $("#txtInspectionCoilSize").removeAttr("disabled").val(txtInspectionCoilSize);
        $("#txtInspectionCoilShape").removeAttr("disabled").val(txtInspectionCoilShape);
        $("#txtIsOK").removeAttr("disabled").val(txtIsOK);
        $("#txtECNGBars").addClass("Mandatory");

        $("#txtECNGBars").addClass("Mandatory");

    } else {
        $("#txtShikii").attr("disabled", "disabled");
        $("#txtShikii").val("");
        $("#txtInspectionCoilSize").attr("disabled", "disabled");
        $("#txtInspectionCoilSize").val("");
        $("#txtInspectionCoilShape").attr("disabled", "disabled");
        $("#txtInspectionCoilShape").val("");
        $("#txtIsOK").attr("disabled", "disabled");
        $("#txtIsOK").val("");
        $("#txtECNGBars").removeClass("Mandatory");
    }
}

function toggleUnitTypeFieldsEnabled($this) {
    if ($this.find(":selected").val() == '1') {

        // $("#ddlTPTest").find(":selected").val()
        $("#chkFinishedQuantity").removeAttr("disabled");
        $("#chkFinishedQuantity").prop('checked', true);
    } else {
        $("#chkFinishedQuantity").attr("disabled", "disabled");
        $("#chkFinishedQuantity").prop('checked', false);
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
        url: "/ColdDrawingWIREDRAWING/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

            //      $("#txtStartDate").val($.format.date(new Date(sysDateTime),"yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            //  $("#txtStartTime").val($.format.date(new Date(sysDateTime),"HH:mm")).mask("99:99", { placeholder: "hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
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

            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtEndTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function getColdDrawingWireDrawingDetailsByDrawingCD($callFrom) {
  
   //GetWindingShape();
    EditCheck = "0";
    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var drawingCD = $.trim($callFrom.closest("tr").attr("data-lblDrawingCD"));
    var jsonData = "{DrawingCD :'" + drawingCD + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/getColdDrawingWireDrawingDetailsByDrawingCD?drawingCD=" + parseInt(drawingCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
           // setProcessDefectList();
            setNextProcess();
            //setStamp();
            setMachineList();
            setColdDrawingWireDrawingDetails(jsonResult);
            setStartOperatorList("", jsonResult.dW_START_OPERATOR_CD);
            setEndOperatorList("", jsonResult.dW_END_OPERATOR_CD);

            //setActiveTab("TbPMSEditDelTab2");
            //SetActDimD456();
            $("#btnUpdate").click(function (event) {
             
                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }

                if (!IsDropDownMandatory()) {
                    return false;
                }
                if (!IsEndDataValid()) {
                    return false;
                }
                if (!checkDate($("#txtStartDate"))) {
                    alert($("#txtStartDate"));
                    var message = $("#lblErrorMessageE_STA_ENTRY_017").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, $("#txtStartDate"), WARNING);
                    return false;
                }

                if (!checkDate($("#txtEndDate"))) {
                    alert($("#txtEndDate"));
                    var message = $("#lblErrorMessageE_STA_EDIT_DELETE_007").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, $("#txtEndDate"), WARNING);
                    $("#txtEndDate").focus;
                    return false;
                }

                if (!IsActiveMachine(jsonResult.machinE_CD)) {
                    return false;
                }
          if (EditCheck == 0) {
                    updateColdDrawingWireDrawingDetails();
                    EditCheck = "1";
              }
                /*
                if (!$('#chkFinishedQuantity').is(':checked') && !isFinishedQuantityValid()) {
                    var message = $.trim($("#lblConfirmationMessageC_HIKI_EDIT_DELETE_001").text());
                    var focusFieldID = "txtFinishedQuantity";

                    $("#dialogMessageIcon img").hide();
                    $("#imgWarning").show();

                    $("#dialogMessageText").html("").append($("<p>" + message + "<p>"));
                    $("#dialog").dialog({
                        modal: true,
                        buttons: {
                            Yes: function () {
                                $(this).dialog("close");
                                updateStamppingDetails();
                            },
                            No: function () {
                                $(this).dialog("close");
                                if (focusFieldID !== "") {
                                    $("#" + focusFieldID).focus();
                                }
                            }
                        }
                    });
                }
                else {

                    updateStamppingDetails();
                }
                */
            });
            $("#btnDelete").click(function (event) {
                deleteColdDrawingWireDrawingDetails();
            });
            $("#btnInitialize").click(function (event) {

                initialize();
            });

            $("#btnStartDateTime").click(function (event) {
                setStartSysDateTime();
            });
            $("#btnEndDateTime").click(function (event) {
                setEndSysDateTime();
            });
            /*
            $("#chkActualDim").change(function (event) {
                toggleActualDimFieldsMandatory($(this));
            });
            $("#chkActualLength").change(function (event) {
                toggleActualLengthFieldsMandatory($(this));
            });
            $("#chkECOnline").change(function (event) {
                toggleECOnlineFieldsMandatory($(this));
            });

          

            $("#ddlUnitType").change(function (event) {

                toggleUnitTypeFieldsEnabled($(this));
            });   */

            //setOperatorList("");
            $("#btnYobinaStartOpeartorSearch").click(function (event) {
                var yobina = $.trim($("#txtYobinaStartOperatorSearch").val());
                setStartOperatorList(yobina, "");
            });
            $("#btnYobinaEndOpeartorSearch").click(function (event) {
                var yobina = $.trim($("#txtYobinaEndOperatorSearch").val());
                setEndOperatorList(yobina, "");
            });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function switchTab($tabFrom, $tabTo) {

    $tabTo.click();

    enableEditDeleteButtons();
}

function setColdDrawingWireDrawingDetails(jsonResult) {
    
    console.log(jsonResult);
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    // setProcessDefectList();
    // setNextProcess();
    $("#hdfMachineCD").val(jsonResult.machinE_CD);
    $("#hdfProcessPlanCD").val(jsonResult.procesS_PLAN_CD);
    // $("#hfld_MessageCD").val(jsonResult.machinE_CD);

    /*Check machine is Active/Inactive*/

    $('#ddlModel option').each(function () {
        if (this.value == jsonResult.machinE_CD)
            IsExists = true;
    });

    // If machine is inactive then first add it in drop down
    if (IsExists === false) {
        $("#ddlModel").append(new Option(jsonResult.dW_MACHINE_NAME, jsonResult.machinE_CD));
    }
    $("#ddlModel").val(jsonResult.machinE_CD);
    $("#txtCreatedDate").text($.format.date(new Date(jsonResult.sT_CREATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").text($.format.date(new Date(jsonResult.sT_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#hfld_MessageCD").val(jsonResult.dW_MSG_CD);
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").text(jsonResult.pP_CHUUBAN);
    $("#HiGyouban").text(jsonResult.pP_GYOUBAN);
    $("#txtDeliveryDateT1").text($.format.date(new Date(jsonResult.pP_DELIVERY_DATE_STRING), "yyyy/MM/dd"));
    $("#txtCustomerInchargeT1").text(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT1").text(jsonResult.pP_STD_DEL_PERIOD);
    $("#txtCustomerT1").text(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").text(jsonResult.pP_RECEIVER_NAME);
    $("#txtChamferT1").text(jsonResult.pP_CHAMFER);
    $("#txtUnitWtT1").text(jsonResult.pP_UNIT);
    $("#txtColorT1").text(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").text(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT1").text(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT1").text(jsonResult.pP_MATL_DIM_D);// $("#txtMatlDimT1").text(jsonResult.PP_MATL_DIM_D_STRING);
    $("#txtChargeT1").text(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").text(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT1").text(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdShapeT1").text(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT1").text(jsonResult.pP_PROD_DIM_D);//$("#txtProdDimT1").text(jsonResult.PP_PROD_DIM_D_STRING);
    $("#txtProdDimOtherT1").text(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT1").text(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT1").text(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT1").text(jsonResult.pP_DISPLAY);
    $("#txtStampT1").text(jsonResult.pP_STAMP);
    $("#txtLengthT1").text(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthToleranceT1").text(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").text(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").text(jsonResult.pP_NO_OF_UNITS_DECODE);               //text(jsonResult.PP_NO_OF_UNITS);
    $("#txtQuantityT1").text(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").text(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").text(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").text(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").text(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").text(jsonResult.pP_PROCESS_PLAN_REMARK);
    $("#txtProductionProcessT1").text(jsonResult.pP_PRODUCTION_PROCESS);


    //Start Data
    $("#hfld_DrawingCd").val(jsonResult.drawinG_CD);

    $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    $("#ddlStartOperatorNameValue").val(jsonResult.dW_START_OPERATOR_NAME);
    $("#txtRollingnumber").val(jsonResult.dW_ROLLING_NUMBER);
    $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);
    $("#txtDieNo").val(jsonResult.dW_DIE_NO);
    $("#txtDieDim").val(jsonResult.dW_DIE_DIM);
    $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
    $("#ddlWindingshape").val(jsonResult.dW_WINDING_SHAPE || $("#ddlWindingshape").prop('selectedIndex'));
    $("#ddlDiceInspectionStart").val(jsonResult.dW_DIE_CHK_START || $("#ddlDiceInspectionStart").prop('selectedIndex'));

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

    //End Data
    

    $("#txtEndDate").val($.format.date(new Date(jsonResult.dW_END_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    
    $("#ddlEndOperatorNameValue").val(jsonResult.dW_END_OPERATOR_NAME);
    $("#ddlDecision").val(jsonResult.dW_DECISION || $("#ddlDecision").prop('selectedIndex'));
    $("#ddlNextProcess").val(jsonResult.dW_DW_NEXT_PROCESS || $("#ddlNextProcess").prop('selectedIndex'));
    
    $("#txtActual_Start").val(jsonResult.dW_ACT_DIM_START);
    $("#txtActual_End").val(jsonResult.dW_ACT_DIM_END);
    $("#txtNoOfUnits").val(jsonResult.dW_ACT_DIM_START);
    $("#txtWeight").val(jsonResult.dW_ACT_DIM_END);
    $("#ddlExternalSurfaceCondition").val(jsonResult.dW_EXT_TEXTURE_STATUS || $("#ddlExternalSurfaceCondition").prop('selectedIndex'));

    $("#ddlDiceInspectionEnd").val(jsonResult.dW_DIE_CHK_END || $("#ddlDiceInspectionEnd").prop('selectedIndex'));
    $("#ddldustdetection").val(jsonResult.dW_MAGNETIC_INSPECTIONS || $("#ddldustdetection").prop('selectedIndex'));
    $("#txtThaba1").val(jsonResult.dW_DW_TABA_NO_1);
    $("#txtThaba2").val(jsonResult.dW_DW_TABA_NO_2);
    $("#txtThaba3").val(jsonResult.dW_DW_TABA_NO_3);
    $("#txtThaba4").val(jsonResult.dW_DW_TABA_NO_4);
    $("#txtRemarks").val(jsonResult.dW_REMARKS);

    if (jsonResult.dW_DW_ACT_DIM_CHK == 0) {
        $("#chkActualLength").prop('checked', false);
    } else {

        $("#chkActualLength").prop('checked', true);
    }


   bindEvents();
    // oldHikinukiRemarks = $.trim(jsonResult.DW_REMARKS);
}


function SetActDimD456() {

    if ($("#txtActualDim4").val().length > 0 ||
        $("#txtActualDim5").val().length > 0 ||
        $("#txtActualDim6").val().length > 0) {
        //set ActualDim4, ActualDim5, ActualDim6 Mandatory

        if ($("#chkActualDim").is(':checked')) {
            $("#txtActualDim4, #txtActualDim5, #txtActualDim6").addClass("Mandatory");
        }
    }

    if ($("#txtActualDim4").val().length == 0 &&
        $("#txtActualDim5").val().length == 0 &&
        $("#txtActualDim6").val().length == 0) {
        //set ActualDim4, ActualDim5, ActualDim6 Non-Mandatory
        $("#txtActualDim4, #txtActualDim5, #txtActualDim6").removeClass("Mandatory");
    }

}

function setStamp() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Stampping/GetStamp",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindStampDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setMachineList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetMachineList",
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
    //  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
}

function bindStampDropdown(jsonResult) {
    //  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlStamp").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

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
        method: "GET",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetWindingShape() {
  
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/GetWindingShape",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            $.each(response.message, function (key, jsonObj) {
                $("#ddlWindingshape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

            });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
   
    //  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlNextProcess").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

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
        url: "/Stampping/GetProcessDefect",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
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


function bindEvents() {
    $("#txtNGWeight1").keyup(function (event) {
        NGRegionClassChange();
    });
    $("#txtNGWeight2").keyup(function (event) {
        NGRegionClassChange();
    });
    $("#txtNGWeight3").keyup(function (event) {
        NGRegionClassChange();
    });
    //$("#txtActualDim4, #txtActualDim5, #txtActualDim6").change(function (event) {
    //    SetActDimD456();

    //});

    $("#txtRemarks").change(function (event) {
      
        if (oldHikinukiRemarks != $.trim($(this).val())) {
            isStamppingRemarksUpdate = 1;
        }
    });

}
function NGRegionClassChange() {

    var txtNGWeight1 = document.getElementById("txtNGWeight1");
    var txtNGWeight1Length = txtNGWeight1.value.length;
    var ddlDefects1 = document.getElementById("ddlDefects1");
    if (txtNGWeight1Length > 0) {
        ChangeControlDisplay('#ddlDefects1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
    }
    else {
        ChangeControlDisplay('#ddlDefects1', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);

    }

    var txtNGWeight2 = document.getElementById("txtNGWeight2");
    var txtNGWeight2Length = txtNGWeight2.value.length;
    var ddlDefects2 = document.getElementById("ddlDefects2");
    if (txtNGWeight2Length > 0) {
        ChangeControlDisplay('#ddlDefects2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
    }
    else {
        ChangeControlDisplay('#ddlDefects2', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);

    }

    var txtNGWeight3 = document.getElementById("txtNGWeight3");
    var txtNGWeight3Length = txtNGWeight3.value.length;
    var ddlDefects3 = document.getElementById("ddlDefects3");
    if (txtNGWeight3Length > 0) {
        ChangeControlDisplay('#ddlDefects3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
    }
    else {
        ChangeControlDisplay('#ddlDefects3', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);

    }
}

function deleteColdDrawingWireDrawingDetails() {
    var ajaxTime = new Date().getTime();
    var jsonDataSaveEditDelete = "{"

        + "DRAWING_CD" + " : '" + $("#hfld_DrawingCd").val() + "',"
        + "MSG_CD" + " : '" + $("#hfld_MessageCD").val() + "',"
        + "USER_NAME" + " : '" + userName
        + "'}";


    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsDelete?drawingCD=" + parseInt($("#hfld_DrawingCd").val()),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonDataSaveEditDelete + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            clearPanel($("#pnlProductDetails"));
            clearPanel($("#pnlProcessStartData"));
            clearPanel($("#pnlProcessEndData"));
            resetGlobalVariables();
            disableEditDeleteButtons();

            var successMessage = $("#lblSuccessMessageS_STA_EDIT_DELETE_002").text();
            showMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setStartOperatorList(yobina, defaultSelectID) {
    setOperatorList(yobina, $("#ddlStartOperatorNameValue"), defaultSelectID);
};

function setEndOperatorList(yobina, defaultSelectID) {
    setOperatorList(yobina, $("#ddlEndOperatorNameValue"), defaultSelectID);
};

function setOperatorList(yobina, $target, defaultSelectID) {

    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "', USER_NAME: '" + userName + "' , employeeCD: '" + defaultSelectID + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/EmployeeList?name=" + yobina + "&addEmpty=false",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);
            bindOperatorDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindOperatorDropdown(jsonResult, $target, defaultSelectID) {

    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina) + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

function updateColdDrawingWireDrawingDetails() {


    if ($("#hfld_MessageCD").val() == 0) {
        $("#hfld_MessageCD").val(null)
    }

    var ajaxTime = new Date().getTime();
    var jsonDataSaveEditDelete = JSON.stringify({


        DRAWING_CD: parseInt($("#hfld_DrawingCd").val()),
        MACHINE_CD: parseInt($("#ddlModel").val()),
        PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),
        MSG_CD: parseInt($("#hfld_MessageCD").val()),
        DW_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,
        DW_START_DATE: $("#txtStartDate").val(),
        DW_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),
        ROLLING_NUMBER: $("#txtRollingnumber").val(),
        FURNACE_NO: $("#txtFurnaceNum").val(),
        DIE_DIM: $("#txtDieDim").val(),
        DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        DIE_NO: $("#txtDieNo").val(),
        DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),
        //end Operator
        DW_END_DATE: $("#txtEndDate").val(),
        DW_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),
        DW_ACT_DIM_CHK: ($("#chkActualLength").is(':checked') ? 1 : 0),
        DW_ACT_DIM_START: $("#txtActual_Start").val(),
        DW_ACT_DIM_END: $("#txtActual_End").val(),
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
        USER_ID: parseInt($("#hdfLoginId").val()),
        USER_NAME: userName
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({STAMPING_CD: $("#hfld_StamppingCd").val(),MACHINE_CD: $("#ddlModel").val(),PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),MSG_CD: parseInt($("#hfld_MessageCD").val()),ST_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,ST_START_DATE: $("#txtStartDate").val() + " " + $("#txtStartTime").val(),ST_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),ST_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val(),ST_DECISION: parseInt($("#ddlDecision").val()),ST_NG_QTY: parseFloat($("#txtNGQty").val()),ST_OK_QTY: parseFloat($("#txtNgWeight").val()),ST_NG_BARS1: parseInt($("#txtNGWeight1").val()),ST_NG_DETAILS1: parseInt($("#ddlDefects1").val()),ST_NG_BARS2: parseInt($("#txtNGWeight2").val()),ST_NG_DETAILS2: parseInt($("#ddlDefects2").val()),ST_NG_BARS3: parseInt($("#ddlDefects3").val()),ST_NG_DETAILS3: parseInt($("#txtNGWeight3").val()),ST_TABA_NO: parseInt($("#txtBundleNo").val()),ST_OK_BARS: parseInt($("#txtFinishedBarCount").val()),UNIT_TYPE: parseInt($("#ddlUnitType").val()),ST_END_DATE: $("#txtEndDate").val() + " " + $("#txtEndTime").val(),ST_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),ST_NEXT_PROCESS: parseInt($("#ddlNextProcess").val()),ST_REMARKS: $("#txtRemarks").val(),USER_NAME: userName})
        data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            disableEditDeleteButtons();
            var successMessage = $("#lblSuccessMessageS_ColdDrawing_EditDelete_001").text();
            showMessageDialog(successMessage, "Updated", SUCCESS);
            var jsonResult = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
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

        var $txtActualLen1 = $("#txtActualLength1");
        var $txtActualLen2 = $("#txtActualLength2");
        var $txtActualLen3 = $("#txtActualLength3");

        $.each([$txtActualLen1, $txtActualLen2, $txtActualLen3],
            function (index, $actLen) {
                if ($.isNumeric($actLen.val()) && (parseFloat($actLen.val()) < stdMinLength || parseFloat($actLen.val()) > stdMaxLength)) {
                    focusFieldID = $actLen.attr("id");
                    retval = false;
                }
            });
    }


    return { retval: retval, focusFieldID: focusFieldID };
}

function isFinishedQuantityValid() {
    var constant = 0;
    var toleranceWeightMean;
    var toleranceWeightMin;
    var toleranceWeightMax;
    var returnVal = false;
    var toleranceWeightPercent = 2;

    finishedBarCount = $("#txtFinishedBarCount").val();
    finishedQuantity = $("#txtFinishedQuantity").val();
    productStdShape = $.trim($("#txtProdShapeT1").text());

    // Set appropriate constant
    switch (productStdShape.toLowerCase()) {
        case PRODUCTSHAPE_R.toLowerCase():
            constant = 0.00617;
            break;
        case PRODUCTSHAPE_H.toLowerCase():
            constant = 0.00676;
            break;
        case PRODUCTSHAPE_S.toLowerCase():
            constant = 0.00785;
            break;
        default:
            break;
    };

    if (constant != 0) {
        // Calculate tolerance valid weight range
        toleranceWeightMean = (dimStd * dimStd * constant * lenStd * finishedBarCount) / 1000;
        toleranceWeightMin = toleranceWeightMean * (1 - (toleranceWeightPercent / 100));
        toleranceWeightMax = toleranceWeightMean * (1 + (toleranceWeightPercent / 100));

        // Check if entered weight is valid
        if (finishedQuantity >= toleranceWeightMin && finishedQuantity <= toleranceWeightMax)
            returnVal = true;
    }

    return returnVal;
}

function IsEndDataValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_008").text());
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_009").text());
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    /*
        // 3. If ECOnline checkbox is unchecked, then ECNGBar count cannot be set as Zero
        if (!$('#chkECOnline').is(':checked') && $.isNumeric($("#txtECNGBars").val())) {
            if (parseInt($("#txtECNGBars").val()) == 0) {
                var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_008").text());
                var focusFieldID = "txtECNGBars";
                showMessageDialog(message, focusFieldID, WARNING);
                return false;
            }
        }
    
        // 4. If ActDim Or ActLength is out of tolerance, show error
        if ($('#chkActualDim').is(':checked') && $("#ddlDecision option:selected").val() == combinedDecisionOK) {
            var returnObj = isActDimWithinTolerance();
            if (returnObj.retval == false) {
                var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_010").text());
                showMessageDialog(message, returnObj.focusFieldID);
                return false;
            }
        }
        if ($('#chkActualLength').is(':checked') && $("#ddlDecision option:selected").val() == combinedDecisionOK) {
            var returnObj = isActLengthWithinTolerance();
            if (returnObj.retval == false) {
                var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_010").text());
                showMessageDialog(message, returnObj.focusFieldID);
                return false;
            }
        }
    
        // 5. If +-2% checkbox is checked and Finished Qty is not in 2% Range, show error
        if ($('#chkFinishedQuantity').is(':checked') && !isFinishedQuantityValid()) {
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_011").text());
            var focusFieldID = "txtFinishedQuantity";
            showMessageDialog(message, focusFieldID, WARNING);
            return false;
        }
        */
    return true;
}

function checkDate(field) {
    // var startDate = field.val();

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

function IsActiveMachine(machineCd) {

    if (IsExists == false && $("#ddlModel").val() == machineCd) {
        var focusFieldID = "ddlModel";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_014").text());
        showMessageDialog(message, focusFieldID);
        return false;
    }
    else {
        return true;
    }
}

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


    function IsInActiveOperator(dwStartOperater) {
        alert(dwStartOperater);
        var returnVal = true;  // default true is required for screens with no mandatory fields

        var message = ""; // This message will be displayed on dialog box
        var isRequired = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_001").text());

    };


    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        //$("#imgError").show();
        //$("#imgWarning").show();

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

function IsDropDownMandatory() {
    var returnVal = true;
    var message = "";
    if ($("#ddlStartOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if (!$("txtNGWeight1").val() && $("#ddlDefects1").val() == "0") {
        var focusFieldID = "ddlDefects1";
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_005").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if (!$("txtNGWeight2").val() && $("#ddlDefects2").val() == "0") {
        var focusFieldID = "ddlDefects2";
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_005").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if (!$("txtNGWeight3").val() && $("#ddlDefects3").val() == "0") {
        var focusFieldID = "ddlDefects3";
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_005").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }


    return returnVal;
}

function initialize() {
   
    $(".FocusDefault").focus();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearTable($("#grdvProductList"), "", "GridNormalRow");
    clearTable($("#grdvProductList"), "", "GridAlternateRow");
    resetGlobalVariables();
    disableEditDeleteButtons();
    EditCheck = "0";
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

function setStartOperatorList(yobina, defaultSelectID) {
    setOperatorList(yobina, $("#ddlStartOperatorNameValue"), defaultSelectID);
};


function setDecimalFormats() {
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtNGQty, #txtNgWeight").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
}

function Mask() {
    $("#txtOperationDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtOperationDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

}


var gridViewScroll = null;



$(document).ready(function () {
    bindOperatorList('');
    InitSortPopup();
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // CheckNumberValidation();
    SetGlobalVariables();
    // SetMaxLengthAndFormat();
    window.resizeTo(1080, 785);
    /*   $("#btnLnkEdit").click(function (event) {
   
           resetResponseTime();
           var $callFrom = $(this);
           var stamppingCD = $(event.target).find("#lblStampingCD").val();
           getStamppingDetailsByStamppingCD(stamppingCD, $callFrom);
   
       });*/

    //    setStartOperatorList("", "");
    //  setEndOperatorList("", "");

    setDecimalFormats();
    /*  setProcessDefectList();
      setNextProcess();
      setStamp();*/
    disableEditDeleteButtons();

    userName = $("#hdfUserName").val();
    Mask();
    $("#lbl_DateOfIssue").show();
    $("#txtCreatedDate").show();
    $("#lblUpdatedDate").show();
    $("#txtUpdatedDate").show();
    //FreeseHeaders();
});


//***************************************************
//****************************************************

function btnSearch_Click() {
    
    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val();
    var txtDimDMax = $("#txtDimDMax").val();
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val();
    var txtLengthSearchMax = $("#txtLengthSearchMax").val();
    
    if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0) {
        var ajaxTime = new Date().getTime();
        var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtCustomerName: txtCustomerName, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/ColdDrawingWIREDRAWING/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtCustomerName: txtCustomerName, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax })
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess)
                    $("#divColdDrawingWireDrawingGridMain").html(response.message);
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
                message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {
        
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            url: "/ColdDrawingWIREDRAWING/ColdDrawingWIREDRAWINGEditDelete",
            method: "POST",
            contentType: "application/json; charset=utf-8",
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                alert(response.message);
                $("#divColdDrawingWireDrawingGridMain").html(response.message);
            })
       
        //bootbox.alert({


        //    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
        //    message: "This is test message",
        //    centerVertical: true
        //});
        var message = $("#lblErrorMessageE_ColdDrawing_SearchField").text().trim();
        showMessageDialog(message, "SearchField", WARNING);
      
    }
}

function bindOperatorList(search) {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/ColdDrawingWIREDRAWING/EmployeeList?name=" + search,
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
    ColdDrawingGridSearch(selectedColumns);
}

function ColdDrawingGridSearch(sortFields) {
   
    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val();
    var txtDimDMax = $("#txtDimDMax").val();
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val();
    var txtLengthSearchMax = $("#txtLengthSearchMax").val();
  //  if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0) {
  
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/ColdDrawingWIREDRAWING/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtCustomerName: txtCustomerName, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, sortFields: sortFields })
            //data: { request: JSON.stringify(objRequest), sortFields: JSON.stringify(sortFields) }
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess)
                    $("#divColdDrawingWireDrawingGridMain").html(response.message);
               
                else {
                    bootbox.alert({
                        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                        message: response.message,
                        centerVertical: true
                    });
                }
                closepopup();
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
   // else {
        //bootbox.alert({
        //    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
        //    message: "This is test message",
        //    centerVertical: true
        //});
  //  }
//}

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/ColdDrawingWIREDRAWING/InitializeSortPopup",
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
    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}