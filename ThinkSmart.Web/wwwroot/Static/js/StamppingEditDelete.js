/*var APIURL = "https://localhost:7269"; */
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
var DISABLED = "disabled";
var registeredThabaNumbersAndDecision = {};
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
    isStamppingRemarksUpdate = 0;

    registeredThabaNumbersAndDecision = {};
    combinedDecisionOK = 1;
    indexDDLDecisionNothing = 0;
    typeDropDown = "select-one";
    typeText = "text";
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
    IsExists = false;
    DISABLED = "disabled";
    operatorCD = 0;
    PRODUCTSHAPE_R = "R";
    PRODUCTSHAPE_H = "H";
    PRODUCTSHAPE_S = "S";   
    userName = ""; 
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
        url: APIURL + "/Stampping/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

           // $("#txtStartDate").val($.format.date(new Date(sysDateTime),"yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            //$("#txtStartTime").val($.format.date(new Date(sysDateTime),"HH:mm")).mask("99:99", { placeholder: "hh:mm" });
            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
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
        url: APIURL + "/Stampping/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

           // $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


$("#btnDelete").click(function (event) {
    deleteStampingDetails();
});

function enableStartDataPanel() {

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
function enableModelDataPanel() {

    $("#ddlModel").removeClass(DISABLED).removeAttr(DISABLED);
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
    $("#txtNgWeight").attr("DISABLED", "DISABLED").addClass("DISABLED");

    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    //$('#ddlUnitType option:eq(0)').prop('selected', true);

}

function getStamppingDetailsByStamppingCD($callFrom) {
  
    // Do not proceed if request is already running
    //$callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var stamppingCD = $.trim($callFrom.closest("tr").attr("data-lblStampingCD"));

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: APIURL + "/Stampping/GetStamppingDetailsByStamppingCD?stamppingCD=" + parseInt(stamppingCD) + "&cultureName=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            setProcessDefectList();
            debugger;
            setStamppingDetails(jsonResult);
            enableStartDataPanel();
            enableEndDataPanel();
            enableModelDataPanel();
            $("#btnStartDateTime").unbind("click")
            $("#btnStartDateTime").click(function (event) {
                setStartSysDateTime();
            });

            $("#btnEndDateTime").unbind("click")
            $("#btnEndDateTime").click(function (event) {
                setEndSysDateTime();
            });

            $("#btnUpdate").unbind("click")
            $("#btnUpdate").click(function (event) {
                
                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }

                //if (!IsDropDownMandatory()) {
                //    return false;
                //}
                if (!checkDate($("#txtStartDate"))) {
                    var message = $("#lblErrorMessageE_STA_EDIT_ENTRY_017").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, 'txtStartDate', WARNING);
                    $("#txtStartDate").focus();
                    //$("#txtChuubanSearch").focus();
                    return false;
                }
                
                if (!checkDate($("#txtEndDate"))) {
                    var message = $("#lblErrorMessageE_STA_EDIT_ENTRY_018").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtEndDate", WARNING);
                    $("#txtEndDate").focus;
                    return false;
                }
                
                if (!IsEndDateValid()) {
                    return false;
                }
                //if (!IsActiveMachine($("#hdfMachineCD").val())) {
                //    return false;
                //}
                if ((jsonResult.pP_GYOUBAN.toString().length) == 1) {
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

                updateStamppingDetails();

            });
            

            //$("#btnYobinaStartOpeartorSearch").click(function (event) {
            //    getOperatorList($("#txtYobinaStartOperatorSearch").val(), $("#ddlStartOperatorNameValue"));
            //});
            //$("#btnYobinaEndOpeartorSearch").click(function (event) {

            //    getOperatorList($("#txtYobinaEndOperatorSearch").val(), $("#ddlEndOperatorNameValue"));
            //});

            $("#ddlModel").focus();
            
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
   
}
function MachineIsInactive() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Stampping/MachineIsInactive?machineName=" + $("#ddlModel option:selected").text(),
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
            message = $("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setStamppingDetails(jsonResult) {
    console.log(jsonResult);
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    $("#txtYobinaStartOperatorSearch").val("");
    $("#txtYobinaEndOperatorSearch").val("");
    enableEditDeleteButtons();
   // setProcessDefectList();
   // setNextProcess();
    $("#hdfMachineCD").val(jsonResult.machinE_CD);
    setMachineList();

    startOperatorCD = jsonResult.sT_START_OPERATOR_CD;
    operatorCD = startOperatorCD;
    //getOperatorList("", $("#ddlStartOperatorNameValue"));
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), operatorCD)

    endOperatorCD = jsonResult.sT_END_OPERATOR_CD;
    operatorCD = endOperatorCD;
    //getOperatorList("", $("#ddlEndOperatorNameValue"));
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), operatorCD)

    $("#hdfProcessPlanCD").val(jsonResult.procesS_PLAN_CD);    
    $("#hfld_MessageCD").val(jsonResult.msG_CD);
    /*Check machine is Active/Inactive*/
    //$('#ddlModel option').each(function () {
    //    if (this.value == jsonResult.machinE_CD)
    //        IsExists = true;
    //});

    //// If machine is inactive then first add it in drop down
    //if (IsExists === false) {
    //    $("#ddlModel").append(new Option(jsonResult.sT_MACHINE_NAME, jsonResult.machinE_CD));
    //}
    $("#ddlModel").val(jsonResult.machinE_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.sT_CREATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.sT_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    
    //$("#hfld_MessageCD").val(jsonResult.sT_MSG_CD);
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOUBAN);
    $("#HiGyouban").val(jsonResult.pP_GYOUBAN);
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
    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);// $("#txtMatlDimT1").val(jsonResult.PP_MATL_DIM_D_STRING);
    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);//$("#txtProdDimT1").val(jsonResult.PP_PROD_DIM_D_STRING);
    $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);               //text(jsonResult.PP_NO_OF_UNITS);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    
        $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
  
    $("#txt_HostName").val(jsonResult.nyukA_CD);
    
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    //Start Data
    $("#hfld_StamppingCd").val(jsonResult.stampinG_CD);
    $("#txtStartDate").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //$("#txtStartDate").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
  //  $("#txtStartTime").val($.format.date(new Date(jsonResult.sT_START_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#ddlStartOperatorNameValue").val(jsonResult.sT_START_OPERATOR_CD);
    $("#txtMetalicPltNo").val(jsonResult.sT_METALLIC_PLATE_CODE);
    //$("#ddlStamp option:contains(" + jsonResult.stamP_DETAILS + ")").prop('selected', true);
    //$("#ddlStamp").val(jsonResult.stamP_DETAILS);
    $("#txtStamp").val(jsonResult.stamP_DETAILS);

    $("#ddlTrialResult1").val(jsonResult.sT_TRIAL_RESULT);

    //End Data
    $("#txtEndDate").val($.format.date(new Date(jsonResult.sT_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
   // $("#txtEndDate").val($.format.date(new Date(jsonResult.sT_END_DATE_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    //$("#txtEndTime").val($.format.date(new Date(jsonResult.sT_END_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#ddlEndOperatorNameValue").val(jsonResult.sT_END_OPERATOR_CD);
    $("#txtFinishedBarCount").val(jsonResult.sT_OK_BARS);
    $("#ddlUnitType").val(jsonResult.uniT_TYPE);
    $("#txtNGQty").val(jsonResult.sT_OK_QTY);
    if (jsonResult.sT_NG_QTY == 0)
        $("#txtNgWeight").val("");
    else
        $("#txtNgWeight").val(jsonResult.sT_NG_QTY);
    
    $("#txtBundleNo").val(jsonResult.sT_TABA_NO);

    $("#txtNGWeight1").val(jsonResult.sT_NG_BARS1);    
    if (jsonResult.sT_NG_DETAILS1 > 0) {
        $("#ddlDefects1").val(jsonResult.sT_NG_DETAILS1)
        ChangeControlDisplay('#ddlDefects1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#ddlDefects1").removeAttr("disabled")
    }
    else {        
        $("#ddlDefects1").val(jsonResult.sT_NG_DETAILS1);
        ChangeControlDisplay('#ddlDefects1', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlDefects1").attr("disabled", "disabled")
        $("#txtNGWeight1").val("");
    }

    $("#txtNGWeight2").val(jsonResult.sT_NG_BARS2);
    if (jsonResult.sT_NG_DETAILS2 > 0) {
        $("#ddlDefects2").val(jsonResult.sT_NG_DETAILS2)
        ChangeControlDisplay('#ddlDefects2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#ddlDefects2").removeAttr("disabled")
    }
    else {
        $("#ddlDefects2").val(jsonResult.sT_NG_DETAILS2);
        ChangeControlDisplay('#ddlDefects2', true, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlDefects2").attr("disabled", "disabled")
        $("#txtNGWeight2").val("");
    }

    $("#txtNGWeight3").val(jsonResult.sT_NG_BARS3);
    if (jsonResult.sT_NG_DETAILS3 > 0) {       
        $("#ddlDefects3").val(jsonResult.sT_NG_DETAILS3);
        ChangeControlDisplay('#ddlDefects3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory');
        $("#ddlDefects3").removeAttr("disabled")
    }
    else {        
        $("#ddlDefects3").val(jsonResult.sT_NG_DETAILS3);
        ChangeControlDisplay('#ddlDefects3', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass', 0);
        $("#ddlDefects3").attr("disabled", "disabled")
        jsonResult.sT_NG_DETAILS3 == null;
      $("#txtNGWeight3").val("");
    }
    
    $("#ddlDecision").val(jsonResult.sT_DECISION || $("#ddlDecision").prop('selectedIndex'));
   // $("#ddlDecision").val(jsonResult.sT_DECISION);
    $("#ddlNextProcess").val(jsonResult.sT_NEXT_PROCESS);
    $("#txtRemarks").val(jsonResult.sT_REMARKS);
    //$("#ddlStartOperatorNameValue").val(jsonResult.sT_START_OPERATOR_CD);
    //$("#ddlEndOperatorNameValue").val(jsonResult.sT_END_OPERATOR_CD);
    

    /*dimTolMin = jsonResult.PP_DIM_TOL_MIN;
    dimTolMax = jsonResult.PP_DIM_TOL_MAX;
    dimStd = jsonResult.PP_PROD_DIM_D;
    lenTolMin = jsonResult.PP_LENGTH_TOL_MIN;
    lenTolMax = jsonResult.PP_LENGTH_TOL_MAX;
    lenStd = jsonResult.PP_PROD_LENGTH;
    setDecimalFormats();*/
    //bindEvents();
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
        url: APIURL+ "/Stampping/GetStamp",
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
        url: APIURL + "/Stampping/GetMachineList",
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
        //$("#ddlModel").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '" >' + jsonObj.name + '</option>'));
    });
}

function bindStampDropdown(jsonResult) {
   // $("#ddlStamp").html("").append($('<option value="0"></option>'));
    $("#ddlStamp").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlStamp").append($('<option value="' + jsonObj.name + '">' + jsonObj.name + '</option>'));
        if ($("#txtStamp").val() == "") {
            $("#txtStamp").val(jsonObj.name);
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
        url: APIURL+ "/Stampping/GetProcessDefect",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcessDefectDropdown(jsonResult) {
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


function bindEvents() {
    $("#txtNGWeight1").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGWeight2").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGWeight3").keyup(function (event) {
        OKQtyRegionClassChange();
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
function OKQtyRegionClassChange() {
    debugger;
    var txtNGWeight1 = $("#txtNGWeight1").val();
    var ddlDefects1 = document.getElementById("ddlDefects1");
    if (txtNGWeight1 > 0) {
        ChangeControlDisplay('#ddlDefects1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlDefects1").removeAttr("disabled");
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
        $("#ddlDefects2").removeAttr("disabled");
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
        $("#ddlDefects3").removeAttr("disabled");
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
    //$("#txtNgWeight").val(add);

}

function deleteStampingDetails() {
    
    var ajaxTime = new Date().getTime();
    var jsonData = JSON.stringify({
        STAMPING_CD: $("#hfld_StamppingCd").val(),
        PROCESS_PLAN_CD: $("#hdfProcessPlanCD").val(),
        ST_TABA_NO: $("#txtBundleNo").val(),
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null

    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/Stampping/StamppingDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            clearPanel($("#pnlProductDetails"));
            clearPanel($("#pnlProcessStartData"));
            clearPanel($("#pnlProcessEndData"));
            clearPanel($("#pnlModel"));
            resetGlobalVariables();
           
            disableEditDeleteButtons();
            disableEndDataPanel();
            disableModelDataPanel();
            disableStartDataPanel();
            
            var successMessage = $("#lblSuccessMessageS_STA_EDIT_DELETE_002").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            //switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"))

            StamppingGridSearch(null);
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function updateStamppingDetails() {
    debugger;
    if ($("#hfld_MessageCD").val() == 0) {
        $("#hfld_MessageCD").val(null)
    }

    var ajaxTime = new Date().getTime();
    var jsonDataSaveEditDelete = JSON.stringify({
        STAMPING_CD: parseInt($("#hfld_StamppingCd").val()),
        MACHINE_CD: parseInt($("#ddlModel").val()),
        PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),
        MSG_CD: parseInt($("#hfld_MessageCD").val()),
        ST_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,
        ST_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ST_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),
        ST_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val() ? ($("#txtMetalicPltNo").val()) : null,
        //STAMP_DETAILS: $("#ddlStamp :selected").text() ? $("#ddlStamp :selected").text() : null,
        STAMP_DETAILS: $("#txtStamp").val() ? $("#txtStamp").val() : null,
        ST_TRIAL_RESULT: $("#ddlTrialResult1 option:selected").val() ? $("#ddlTrialResult1").val() : null,
        ST_DECISION: parseInt($("#ddlDecision").val()) ? ($("#ddlDecision").val()) : null,
        ST_OK_QTY: parseFloat($("#txtNGQty").val()) ? ($("#txtNGQty").val()) : null,
        ST_NG_QTY : parseFloat($("#txtNgWeight").val()) ? ($("#txtNgWeight").val()) : null,
        ST_NG_BARS1: parseInt($("#txtNGWeight1").val()) ? ($("#txtNGWeight1").val()) : null,
        ST_NG_DETAILS1: parseInt($("#ddlDefects1").val()) ? ($("#ddlDefects1").val()) : null,
        ST_NG_BARS2: parseInt($("#txtNGWeight2").val()) ? ($("#txtNGWeight2").val()) : null,
        ST_NG_DETAILS2: parseInt($("#ddlDefects2").val()) ? ($("#ddlDefects2").val()) : null,
        ST_NG_BARS3: parseInt($("#txtNGWeight3").val()) ? ($("#txtNGWeight3").val()) : null,
        ST_NG_DETAILS3: parseInt($("#ddlDefects3").val()) ? ($("#ddlDefects3").val()) : null,
        ST_TABA_NO: parseInt($("#txtBundleNo").val()) ? ($("#txtBundleNo").val()) : null,
        ST_OK_BARS: parseInt($("#txtFinishedBarCount").val()) ? ($("#txtFinishedBarCount").val()) : null,
        UNIT_TYPE: parseInt($("#ddlUnitType").val()) ? ($("#ddlUnitType").val()) : null,
        ST_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        ST_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),
        ST_NEXT_PROCESS: parseInt($("#ddlNextProcess").val()) ? ($("#ddlNextProcess").val()) : null,
        ST_REMARKS: $("#txtRemarks").val(),
        USER_ID: parseInt($("#hdfLoginId").val()),
        USER_NAME: userName
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/StamppingDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({STAMPING_CD: $("#hfld_StamppingCd").val(),MACHINE_CD: $("#ddlModel").val(),PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),MSG_CD: parseInt($("#hfld_MessageCD").val()),ST_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,ST_START_DATE: $("#txtStartDate").val() + " " + $("#txtStartTime").val(),ST_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),ST_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val(),ST_DECISION: parseInt($("#ddlDecision").val()),ST_NG_QTY: parseFloat($("#txtNGQty").val()),ST_OK_QTY: parseFloat($("#txtNgWeight").val()),ST_NG_BARS1: parseInt($("#txtNGWeight1").val()),ST_NG_DETAILS1: parseInt($("#ddlDefects1").val()),ST_NG_BARS2: parseInt($("#txtNGWeight2").val()),ST_NG_DETAILS2: parseInt($("#ddlDefects2").val()),ST_NG_BARS3: parseInt($("#ddlDefects3").val()),ST_NG_DETAILS3: parseInt($("#txtNGWeight3").val()),ST_TABA_NO: parseInt($("#txtBundleNo").val()),ST_OK_BARS: parseInt($("#txtFinishedBarCount").val()),UNIT_TYPE: parseInt($("#ddlUnitType").val()),ST_END_DATE: $("#txtEndDate").val() + " " + $("#txtEndTime").val(),ST_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),ST_NEXT_PROCESS: parseInt($("#ddlNextProcess").val()),ST_REMARKS: $("#txtRemarks").val(),USER_NAME: userName})
        data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            //getThabaDetailsByChuuban($("#txtChuuban1").val(), $("#txtGyouban1").val(), $callFrom);
            if (parseInt(response.message) > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }
            var successMessage = $("#lblSuccessMessageS_STA_EDIT_DELETE_001").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            //disableEditDeleteButtons();
            //disableStartDataPanel();
            //disableEndDataPanel();
            //disableModelDataPanel();
            //switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
            //btnSearch_Click();
            StamppingGridSearch(null);
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
    productStdShape = $.trim($("#txtProdShapeT1").val());

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

function IsEndDateValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_STA_EDIT_DELETE_008").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_STA_EDIT_DELETE_009").text().trim();
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

function IsActiveMachine(machineCd) {

    if (IsExists == false && $("#ddlModel").val() == machineCd) {
        var focusFieldID = "ddlModel";
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_014").text());
        var focusFieldID = "txtStartDate";
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
    debugger;
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        
        //if (mandatoryField.type === typeDropDown) {
        //    if (parseInt(mandatoryField.value) === indexDDLDecisionNothing) {
        //        returnVal = false;
        //        message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
        //        if (firstFieldID === "") {
        //            firstFieldID = $(mandatoryField).attr("id");
        //        }
        //    }
        //}
        //else if (mandatoryField.type === typeText) {
        //    if (!mandatoryField.value) {
        //        returnVal = false;
        //        message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
        //        if (firstFieldID === "") {
        //            firstFieldID = $(mandatoryField).attr("id");
        //        }
        //    }
        //}
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
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

function IsDropDownMandatory() {
    
    var returnVal = true;
    var message = "";
    if ($("#ddlStartOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_004").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_004").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
  return returnVal;
}

$("#btnInitialize").click(function (event) {  
    $("#ddlModel").html("").append($('<option value="0"></option>'));
    initialize();
    $("#txtChuubanSearch").focus();
    switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
});

function initialize() {
    
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
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearTable($("#grdvProductList"), "", "GridNormalRow");
    clearTable($("#grdvProductList"), "", "GridAlternateRow");
    resetGlobalVariables();
    fillEmptyGrid();
    disableStartDataPanel();
    disableEndDataPanel();
    disableModelDataPanel();
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
    //btnSearch_Click(true);
    SetStamp();
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

function fillEmptyGrid() {
    var emptyList = [];
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Stampping/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divStamppingGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//function setStartOperatorList(yobina, defaultSelectID) {
//    setOperatorList(yobina, $("#ddlStartOperatorNameValue"), defaultSelectID);
//};
function disableStartDataPanel() {


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

function disableEndDataPanel() {

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
function disableModelDataPanel() {

    $("#ddlModel").attr(DISABLED, DISABLED).addClass(DISABLED);
}
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






//***************************************************
//****************************************************
function btnSearch_Click() {
    debugger;
    var txtChuubanSearch = $("#txtChuubanSearch").val() ? $("#txtChuubanSearch").val() : null;
    var txtCustomerName = $("#txtCustomerName").val() ? $("#txtCustomerName").val() : null;
    var txtProdNameCust = $("#txtProdNameCust").val() ? $("#txtProdNameCust").val() : null;
    var txtProdNameInt = $("#txtProdNameInt").val() ? $("#txtProdNameInt").val() : null;
    var txtDimDMin = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var txtDimDMax = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var txtShap = $("#txtShap").val() ? $("#txtShap").val() : null;
    var txtLengthSearchMin = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var txtLengthSearchMax = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    var userId = $("#hdfLoginId").val();
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

    if (txtChuubanSearch != null || txtCustomerName != null || txtProdNameCust != null || txtProdNameInt != null || txtDimDMin != null || txtDimDMax != null || txtShap != null || txtLengthSearchMin != null || txtLengthSearchMax != null) {
       

        StamppingGridSearch(null);
    }
    else {
        //bootbox.alert({
        //    title: "<div style='color:white'></div>",
        //    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_Stampping_SearchField").text()) + "</td></tr></table>",
        //    centerVertical: true
        //});
        message = $.trim($("#lblErrorMessage_Stampping_SearchField").text());
        showMessageDialog(message, 'txtChuubanSearch', WARNING);
       
    }
}

function bindOperatorList(search) {
   
    var valname = search;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        // url: APIURL + "/Stampping/EmployeeList?name=" + search + "&addEmpty=false",
        url: APIURL + "/Stampping/EmployeeList?name=" + search + "&addEmpty=false",
        method: "POST",
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
        });
}

$(document).on('click', '#CustomerSearch', function () {
    var name = $("#txtInputSearch").val();
    bindOperatorList(search);
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
    StamppingGridSearch(selectedColumns);
}
function StamppingGridSearch(sortFields) {
    debugger;
    $("#btnSort").focus();
    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val() ? $("#txtDimDMin").val(): null;
    var txtDimDMax = $("#txtDimDMax").val() ? $("#txtDimDMax").val(): null; 
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var txtLengthSearchMax = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    var userId = $("#hdfLoginId").val();

    

        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Stampping/GridSearch",
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
                        url: "/Stampping/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divStamppingGridMain").html(response1.message);                               
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                else {
                    bootbox.alert({
                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
   
    
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
        url: "/Stampping/InitializeSortPopup",
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
        });
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


function btnSortClose_Click() {
    closepopup();
    $("#btnSort").focus();
}
function closepopup() {
        $("#SortPopUpModal").modal('hide');
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
        url: APIURL + "/Stampping/GetUnitType",
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

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindTrialResultDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTrialResult1").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setTrialResult() {
    
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
            bindTrialResultDropdown(respose.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
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
        url: "/Stampping/GetDecision",
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
        url: APIURL + "/Stampping/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {

    /*$("#ddlNextProcess").html("").append($('<option value="0"></option>'));*/
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



function IsDuplicateThabaNo(chuuban, gyouban, $callFrom) {
    var t1 = "";
    var Thaba1 = $("#txtBundleNo").val();
    var returnVal = true;
    var Thaba_No = $("#txtBundleNo").val();
    var stamppingCD = $("#hfld_StamppingCd").val();
    $callFrom.data('requestRunning', false)
    var ajaxTime = new Date().getDate();
    var thabamessage = "";
    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', Thaba_No : '" + Thaba_No + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            chuuban: chuuban, gyouban: gyouban, TABA_NO: Thaba_No
        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            debugger;
           
            var jsonResult = response.message;
            if (jsonResult == 0) {
                returnVal = true;
            } else {


                if (jsonResult[0].stampinG_CD == stamppingCD) {
                    returnVal = true;
                } else {
                    focusFieldID = "txtBundleNo";
                    message = $("#lblErrorMessageE_STA_EDIT_ENTRY_086").text().trim().replace("NNN", Thaba_No);

                    showMessageDialog(message, focusFieldID, WARNING);

                    returnVal = false;
                }

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

    return returnVal;
}
function OperatorIsInactive(dropdownName) {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Stampping/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
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
            message = $("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$(document).ready(function () {
    $("#txtChuubanSearch").focus();
    disableStartDataPanel();
    disableEndDataPanel();
    disableModelDataPanel();
    $("#ddlDefects1").attr("disabled", "disabled");
    $("#ddlDefects2").attr("disabled", "disabled");
    $("#ddlDefects3").attr("disabled", "disabled");

    $("#txtNgWeight").attr("DISABLED", "DISABLED").addClass("DISABLED");

    //setStartOperatorList("", "");
    //setEndOperatorList("", "");
    InitSortPopup();
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // CheckNumberValidation();
    SetGlobalVariables();
    bindEvents();
    //OKQtyRegionClassChange();
    window.resizeTo(1255, 945);
 

    //    setStartOperatorList("", "");
    //  setEndOperatorList("", "");

    setDecimalFormats();
    /*  setProcessDefectList();
      setNextProcess();
      setStamp();*/
    disableEditDeleteButtons();
    //setStamp();
    setUnitType();
    setTrialResult();
    setDecision();
    setNextProcess();
    //btnSearch_Click(true);
    userName = $("#hdfUserName").val();
    Mask();
    $("#lbl_DateOfIssue").show();
    $("#txtCreatedDate").show();
    $("#lblUpdatedDate").show();
    $("#txtUpdatedDate").show();
    //FreeseHeaders();
});

$("#btnSort").focusout(function () {
    $("#btnLnkEdit").focus();
}); 

$("#ddlNextProcess").focusout(function (event) {
    $("#btnUpdate").focus();
});

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
            url: APIURL + "/Stampping/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_STA_EDIT_DELETE_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});