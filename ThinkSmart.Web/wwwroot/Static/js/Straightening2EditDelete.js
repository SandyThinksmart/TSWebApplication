
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";
var $btnLnkEdit;
var $btnDelete;
var combinedDecisionOK = "";
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
var oldStraightingRemarks;
var isStraightingRemarksUpdate = 0;
var IsExists = false;
var IsStartOprExists = false;
var IsEndOprExists = false;
var IsProcessDefect1Exists = false;
var IsProcessDefect2Exists = false;
var IsProcessDefect3Exists = false;
var chuuban = null;
var gyouban = null;
var registeredThabaNumbersWithDecisionOK = {};
var registeredThabaNumbersAndDecision = {};
var arrayData = [];
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
    isStraightingRemarksUpdate = 0;
    chuuban = null;
    gyouban = null;
    registeredThabaNumbersWithDecisionOK = {};
    DISABLED = "disabled";
    typeDropDown = "select-one";
    typeText = "text";
    combinedDecisionOK = "";
    indexDDLDecisionNothing = 0;
    PRODUCTSHAPE_R = "R";
    PRODUCTSHAPE_H = "H";
    PRODUCTSHAPE_S = "S";
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";    
    registeredThabaNumbersAndDecision = {};
    arrayData = [];
    operatorCD = 0;
    IsExists = false;
    IsStartOprExists = false;
    IsEndOprExists = false;
    IsProcessDefect1Exists = false;
    IsProcessDefect2Exists = false;
    IsProcessDefect3Exists = false;  
    
}

function SetGlobalVariables() {
    $btnLnkEdit = $("#btnLnkEdit");
    $btnUpdate = $("#btnUpdate");
    $btnDelete = $("#btnDelete");
    $hdfSYSDATE = $("#hdfSYSDATE");
    //$txtChuuban = $("#$txtChuuban1");
    //$txtGyouban = $("#txtGyouban1");
}

function disableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

function initialize() {
    debugger;
    $("#txtChuubanSearch").focus();
    $("#ddlModel").html("").append($('<option value="0"></option>'));
    $(".FocusDefault").focus();
    clearPanel($("#SearchContent"));
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearTable($("#grdvProductList"), "", "GridNormalRow");
    clearTable($("#grdvProductList"), "", "GridAlternateRow");
    clearTable($("#grdvStraighteningDetails"), "", "GridNormalRow");
    clearTable($("#grdvStraighteningDetails"), "", "GridAlternateRow");
    resetGlobalVariables();
    disableEditDeleteButtons();
    disableModelDataPanel();
    disableStartDataPanel();
    disableEndDataPanel();
}
function enableModelDataPanel() {

    $("#ddlModel").removeClass(DISABLED).removeAttr(DISABLED);
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


}

function enableEndDataPanel() {
    debugger;
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
            $(checkBox).removeClass(DISABLED).removeAttr(DISABLED); // Corrected to string "disabled"
            
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });


    //  $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //  $("#txtEndTime").mask("99:99", { placeholder: "hh:mm" });

    $('#ddlUnitType option:eq(0)').prop('selected', true);

}
function disableModelDataPanel() {

    $("#ddlModel").attr(DISABLED, DISABLED).addClass(DISABLED);
}
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
$("#btnInitialize").click(function (event) {
    initialize();
    switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
   // btnSearch_Click(true);
});

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

function toggleActualLengthFieldsMandatory($this) {
    // Note: In windows application, fields mandatory color is not shown on "Act Length Check" checkbox uncheck,
    // but they are still mandatory.
    // So, commenting the below code, so that they will always remain mandatory.

    //if ($this.is(':checked')) {
    //    $("#txtActualLengthAfterStraightening1").addClass("Mandatory");
    //    $("#txtActualLengthAfterStraightening2").addClass("Mandatory");
    //} else {
    //    $("#txtActualLengthAfterStraightening2").removeClass("Mandatory");
    //}
}

function toggleActualDimFieldsMandatory($this) {
    // Note: In windows application, fields mandatory color is not shown on "Act Dim Check" checkbox uncheck,
    // but they are still mandatory.
    // So, commenting the below code, so that they will always remain mandatory.

    //if ($this.is(':checked')) {
    //    $("#txtActualDimDAfterStraightening1").addClass("Mandatory");
    //    $("#txtActualDimDAfterStraightening2").addClass("Mandatory");
    //    $("#txtActualDimDAfterStraightening3").addClass("Mandatory");
    //    $("#txtActualDimDAfterStraightening4").addClass("Mandatory");
    //} else {
    //    $("#txtActualDimDAfterStraightening2").removeClass("Mandatory");
    //    $("#txtActualDimDAfterStraightening3").removeClass("Mandatory");
    //    $("#txtActualDimDAfterStraightening4").removeClass("Mandatory");
    //}
}

function toggleECOnlineFieldsMandatory($this) {
    if ($this.is(':checked')) {
        $("#txtShikii").removeAttr("disabled");
        $("#txtInspectionCoilSize").removeAttr("disabled");
        $("#txtInspectionCoilShape").removeAttr("disabled");
        $("#txtIsOK").removeAttr("disabled");
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
        url: APIURL + "/Straightening2/GetSysDateTime",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            //var jsonResult = $.parseJSON(response.d);
            var sysDateTime = response.message;

            //$("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            //$("#txtStartTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
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

            //$("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            //$("#txtEndTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function switchTab($tabFrom, $tabTo) {
    $tabTo.click();
   enableEditDeleteButtons();
}

//This method check inactive machine or not.
function MachineIsInactive() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/MachineIsInactive?machineName=" + $("#ddlModel option:selected").text(),
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
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//This method check inactive Operator or not.
function OperatorIsInactive(dropdownName) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setStraighteningDetails(jsonResult) {
    debugger;
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    $("#txtYobinaStartOperatorSearch").val("");
    $("#txtYobinaEndOperatorSearch").val("");
    $("#hdfMachineCD").val(jsonResult.machinE_CD);
    $("#hfld_MessageCD").val(jsonResult.msG_CD);
    setMachineList();

    //$('#ddlModel option').each(function () {
    //    if (this.value == jsonResult.machinE_CD)
    //        IsExists = true;
    //});


    //// If machine is inactive then first add it in drop down
    //if (IsExists === false) {
    //    $("#ddlModel").append(new Option(jsonResult.stR_MACHINE_NAME, jsonResult.machinE_CD));
    //} 

    //Bind Operator Start and end
    startOperatorCD = jsonResult.stR_START_OPERATOR_CD;
    operatorCD = startOperatorCD;
    //getOperatorList("", $("#ddlStartOperatorNameValue"));
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), operatorCD)

    endOperatorCD = jsonResult.stR_END_OPERATOR_CD;
    operatorCD = endOperatorCD;
    //getOperatorList("", $("#ddlEndOperatorNameValue"));
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), operatorCD)

    //$('#ddlStartOperatorNameValue option').each(function () {
    //    if (this.value == jsonResult.stR_START_OPERATOR_CD)
    //        IsStartOprExists = true;
    //});

    // If start opr is inactive then first add it in drop down
    //if (IsStartOprExists === false)
    //    $("#ddlStartOperatorNameValue").append(new Option(jsonResult.stR_START_OPERATOR_NAME, jsonResult.stR_START_OPERATOR_CD));

    //$('#ddlEndOperatorNameValue option').each(function () {
    //    if (this.value == jsonResult.stR_END_OPERATOR_CD)
    //        IsEndOprExists = true;
    //});

    // If end opr is inactive then first add it in drop down
    //if (IsEndOprExists === false)
    //    $("#ddlEndOperatorNameValue").append(new Option(jsonResult.stR_END_OPERATOR_NAME, jsonResult.stR_END_OPERATOR_CD));

    
    if (jsonResult.msG_CD == 0)
     $("#hfld_MessageCD").val("");
    $("#hfld_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#hfld_StraighteningCd").val(jsonResult.straighteninG_CD);
    $("#hfld_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);

    $("#ddlModel").val(jsonResult.machinE_CD);
    $("#ddlStartOperatorNameValue").val(jsonResult.stR_START_OPERATOR_CD);
    $("#ddlEndOperatorNameValue").val(jsonResult.stR_END_OPERATOR_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.stR_CREATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.stR_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
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
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    }
    else {
        $("#txtLengthToleranceT1").val('');
    }
  
    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);            
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
    $("#txtConditionT1").val(jsonResult.pP_CONDITION_DESCRIPTION);
   
    $("#hfld_StraightingCd").val(jsonResult.straighteninG_CD);
    //$("#txtStartDate").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    //$("#txtStartTime").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#txtStartDate").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "yyyy/MM/dd HH:mm"));
   // $("#txtStartDate").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#hiStartOperator").val(jsonResult.stR_START_OPERATOR);
    $("#ddlTPTest").val(parseInt(jsonResult.tP_TEST) || $("#ddlTPTest").prop('selectedIndex')); // Note : This logic is very crucial . Either set the data from DB. If data is not in DB then set what is default selected index on HTML.
    if (jsonResult.tP_TEST == null) {
        $('#ddlTPTest option:eq(0)').prop('selected', true);
    }
    $("#txtMetallicLabelNo").val(jsonResult.stR_METALLIC_PLATE_CODE);
    $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
    $("#txtActualDimDBeforeStraightening1").val(jsonResult.stR_DIM_BEFORE_1);
    $("#txtActualDimDBeforeStraightening2").val(jsonResult.stR_DIM_BEFORE_2);
    $("#txtActualLengthBeforeStraightening1").val(jsonResult.stR_LENGTH_BEFORE_1);
    $("#txtActualLengthBeforeStraightening2").val(jsonResult.stR_LENGTH_BEFORE_2);

    $("#txtReservationNum").val(jsonResult.reservatioN_NUMBER);
    
    if (jsonResult.restraighting == "1") {
        $("#chkRestraighting").prop('checked', true);
    }
    else {
        $("#chkRestraighting").prop('checked', false);
    }

    $("#txtEndDate").val($.format.date(new Date(jsonResult.stR_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    if ($.isNumeric(jsonResult.stR_DIM_CHK) && parseInt(jsonResult.stR_DIM_CHK) == 1) {
        $("#chkActualDim").prop('checked', true);
        enableDimAfterMandatory();
    }
    else {
        $("#chkActualDim").prop('checked', false);
        disableDimAfterMandatory();
    }
    $("#txtDimAfterX1").val(jsonResult.stR_DIM_AFTER_X_1);
    $("#txtDimAfterX2").val(jsonResult.stR_DIM_AFTER_X_2);
    $("#txtDimAfterX3").val(jsonResult.stR_DIM_AFTER_X_3);
    $("#txtDimAfterY1").val(jsonResult.stR_DIM_AFTER_Y_1);
    $("#txtDimAfterY2").val(jsonResult.stR_DIM_AFTER_Y_2);
    $("#txtDimAfterY3").val(jsonResult.stR_DIM_AFTER_Y_3);
    if ($.isNumeric(jsonResult.stR_LENGTH_CHK) && parseInt(jsonResult.stR_LENGTH_CHK) == 1) {
        $("#chkActualLength").prop('checked', true);
        $("#txtActualLength1AfterStraightening").addClass("MandatoryTextBox");
        $("#txtActualLength2AfterStraightening").addClass("MandatoryTextBox");
    }
    else {
        $("#chkActualLength").prop('checked', false);
        $("#txtActualLength1AfterStraightening").removeClass("MandatoryTextBox");
        $("#txtActualLength2AfterStraightening").removeClass("MandatoryTextBox");
    }
    $("#txtActualLength1AfterStraightening").val(jsonResult.stR_LENGTH_AFTER_1);
    $("#txtActualLength2AfterStraightening").val(jsonResult.stR_LENGTH_AFTER_2);
    $("#txtFinishedBarCount").val(jsonResult.stR_BARS);
    
    $("#ddlUnitType").val(jsonResult.uniT_TYPE);
    $("#txtFinishedQuantity").val(jsonResult.stR_QTY);
    if ($.isNumeric(jsonResult.stR_FINISHED_QTY_CHK) && parseInt(jsonResult.stR_FINISHED_QTY_CHK) == 1) {
        $("#chkFinishedQuantity").prop('checked', true);
    }
    else {
        $("#chkFinishedQuantity").prop('checked', false);
    }
    $("#txtThaba1").val(jsonResult.stR_TABA_NO_1);
    $("#txtThaba2").val(jsonResult.stR_TABA_NO_2);
    $("#txtThaba3").val(jsonResult.stR_TABA_NO_3);
    $("#txtThaba4").val(jsonResult.stR_TABA_NO_4);
    $("#ddlExternalSurfaceCondition").val(jsonResult.stR_EXT_TEXTURE);
    $("#ddlBendStatus").val(jsonResult.stR_BEND_STATUS);
    $("#ddlDecision").val(jsonResult.stR_DECISION);
    $("#ddlNextProcess").val(jsonResult.stR_NEXT_PROCESS);
    $("#txtReservationNumber").val(jsonResult.stR_RESERVATION_NUMBER);
    $("#txt_HostName").val(jsonResult.nyukA_CD);
   
    debugger;
 
    $("#txtNGQuantity1").val(jsonResult.stR_NG_BARS1);
    if (jsonResult.stR_NG_BARS1 > 0) {
        $("#ddlNGReason1").val(jsonResult.stR_NG_DETAILS1)
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass MandatoryTextBox')
        $("#ddlNGReason1").removeAttr("disabled")
        $("#ddlNGReason1").removeClass(DISABLED);
    }
    else {
        $("#ddlNGReason1").val(jsonResult.stR_NG_DETAILS1);
        ChangeControlDisplay('#ddlNGReason1', false, 'MandatoryDropdown DropdownClass MandatoryTextBox', 'dropListClass');
        $("#ddlNGReason1").attr("disabled", "disabled")
        $("#ddlNGReason1").addClass(DISABLED);
        $("#txtNGQuantity1").val("");
    }

    $("#txtNGQuantity2").val(jsonResult.stR_NG_BARS2);
    if (jsonResult.stR_NG_BARS2 > 0) {
        $("#ddlNGReason2").val(jsonResult.stR_NG_DETAILS2)
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass MandatoryTextBox')
        $("#ddlNGReason2").removeAttr("disabled")
        $("#ddlNGReason2").removeClass(DISABLED);
    }
    else {
        $("#ddlNGReason2").val(jsonResult.stR_NG_BARS2);
        ChangeControlDisplay('#ddlNGReason2', false, 'MandatoryDropdown DropdownClass MandatoryTextBox', 'dropListClass');
        $("#ddlNGReason2").attr("disabled", "disabled")
        $("#ddlNGReason2").addClass(DISABLED);
        $("#txtNGQuantity2").val("");
    }

    $("#txtNGQuantity3").val(jsonResult.stR_NG_BARS3);
    if (jsonResult.stR_NG_BARS3 > 0 ) {
        $("#ddlNGReason3").val(jsonResult.stR_NG_DETAILS3)
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass MandatoryTextBox')
        $("#ddlNGReason3").removeAttr("disabled")
        $("#ddlNGReason3").removeClass(DISABLED);
    }
    else {
        $("#ddlNGReason3").val(jsonResult.stR_NG_BARS3);
        ChangeControlDisplay('#ddlNGReason3', false, 'MandatoryDropdown DropdownClass MandatoryTextBox', 'dropListClass');
        $("#ddlNGReason3").attr("disabled", "disabled")
        $("#ddlNGReason3").addClass(DISABLED);
        $("#txtNGQuantity3").val("");
    }
    
    $("#txtRemarks").val(jsonResult.stR_REMARKS);

    dimTolMin = jsonResult.pP_DIM_TOL_MIN;
    dimTolMax = jsonResult.pP_DIM_TOL_MAX;
    dimStd = jsonResult.pP_PROD_DIM_D;
    lenTolMin = jsonResult.pP_LENGTH_TOL_MIN;
    lenTolMax = jsonResult.pP_LENGTH_TOL_MAX;
    lenStd = jsonResult.pP_PROD_LENGTH;
    setDecimalFormats();
    $(".FocusDefault").focus();
    
    $("#txtStartDate").focus();
    oldStraightingRemarks = $.trim(jsonResult.stR_REMARKS);
}




function OKQtyRegionClassChange() {

    var txtNGWeight1 = $("#txtNGQuantity1").val();
    var ddlDefects1 = document.getElementById("ddlNGReason1");
    if (txtNGWeight1 > 0) {
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason1").removeAttr("disabled")
        $("#ddlNGReason1").removeClass(DISABLED);
    }
    else {
        ChangeControlDisplay('#ddlNGReason1', false, ' dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlNGReason1 option:selected").prop("selected", false);
        $("#ddlNGReason1").attr("disabled", "disabled");
        $("#ddlNGReason1").addClass(DISABLED);
    }
    var txtNGWeight2 = $("#txtNGQuantity2").val();
    var ddlDefects2 = document.getElementById("ddlNGReason2");
    if (txtNGWeight2 > 0) {
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason2").removeAttr("disabled")
        $("#ddlNGReason2").removeClass(DISABLED);
    }
    else {
        ChangeControlDisplay('#ddlDefects2', false, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlNGReason2 option:selected").prop("selected", false);
        $("#ddlNGReason2").attr("disabled", "disabled");
        $("#ddlNGReason2").addClass(DISABLED);
    }
    var txtNGWeight3 = $("#txtNGQuantity3").val();
    var ddlDefects3 = document.getElementById("ddlNGReason3");
    if (txtNGWeight3 > 0) {
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason3").removeAttr("disabled")
        $("#ddlNGReason3").removeClass(DISABLED);
    }
    else {
        ChangeControlDisplay('#ddlDefects3', false, ' dropListClass', 'MandatoryDropdown DropdownClass Mandatory', 0);
        $("#ddlNGReason3 option:selected").prop("selected", false);
        $("#ddlNGReason3").attr("disabled", "disabled");
        $("#ddlNGReason3").addClass(DISABLED);
    }
}





function getStraighteningDetailsByStraighteningCD($callFrom) {
    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var straighteningCD = $.trim($callFrom.closest("tr").attr("data-lblStampingCD"));
    //var straighteningCD = $.trim($callFrom.closest("tr").find("#lblStraighteningCD").text())
    var jsonData = "{STR_STRAIGHTENING_CD :'" + straighteningCD + "', USER_NAME: '" + userName + "' }";
    // setProcessDefectList();
    // setNextProcess();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetStraighteningDetailsByStraighteningCD?STR_STRAIGHTENING_CD=" + straighteningCD + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            enableModelDataPanel();
            enableStartDataPanel();
            enableEndDataPanel();
            var jsonResult = null;
            jsonResult = response.message;
            //setMachineList();
            debugger;
            setStraighteningDetails(jsonResult);

            var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                $("#chkActualLength").removeAttr("disabled", "disabled");

            } else {
                $("#chkActualLength").attr("disabled", "disabled");
                $("#chkActualLength").prop("checked", false);
            }

            $("#btnUpdate").unbind("click")
            $("#btnUpdate").click(function (event) {
                var lengthValue = true;
                var finishedWeightValue = true;
                var actualDimValue = true;
                var focusTab = "";

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

                //===================================start data==============================================

                if ($('#chkActualDim').prop("checked") == true && $("#txtDimAfterX1").val().trim() == '')  {

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

                if ($("#chkActualLength").prop("checked")==true && ($("#txtActualLength1AfterStraightening").val().trim() == '' || $("#txtActualLength2AfterStraightening").val().trim() == '')) {

                    $("#txtActualLengthAftercheck").addClass("Mandatory");
                    $("#txtActualLengthAftercheck").removeClass("MandatoryTextBox");
                } else {
                    $("#txtActualLengthAftercheck").addClass("MandatoryTextBox");
                    $("#txtActualLengthAftercheck").removeClass("Mandatory");
                }


                if ($("#txtFinishedBarCount").val().trim() == '' || $("#ddlUnitType").val() == '0') {

                    $("#txtFinishedBarcheck").addClass("Mandatory");
                    $("#txtFinishedBarcheck").removeClass("MandatoryTextBox");
                } else {
                    $("#txtFinishedBarcheck").addClass("MandatoryTextBox");
                    $("#txtFinishedBarcheck").removeClass("Mandatory");
                }

                //check StartOperatorNameValue InActive
                //if ($("#ddlStartOperatorNameValue option:selected").attr("flag") == 1) {

                //    DisplayInactivePopup("ddlStartOperatorNameValue");
                //    return false;
                //}

                //valMsg_CanNotUseInactiveMaster check this msg.
                debugger;
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

                
                              
                //===================================end data==============================================

                   //check EndOperatorNameValue InActive
                //if ($("#ddlEndOperatorNameValue option:selected").attr("flag") == 1) {

                //    DisplayInactivePopup("ddlEndOperatorNameValue");
                //    return false;
                //}

                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }
                //if (!IsDropDownMandatory()) {
                //    return false;
                //}

                if (!checkDate($("#txtStartDate"))) {
                    var message = $("#lblErrorMessageE_STR_EDIT_DELETE_014").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtStartDate", WARNING);

                    return false;
                }

                if (!checkDate($("#txtEndDate"))) {
                    var message = $("#lblErrorMessageE_STR_EDIT_DELETE_015").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtEndDate", WARNING);

                    return false;
                }

                if (!IsEndDateValid()) {
                    return false;
                }
                if (jsonResult.pP_GYOUBAN.toString().length == 1) {
                    jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
                }

                //if (!IsDuplicateThabaNo(jsonResult.pP_CHUUBAN, jsonResult.pP_GYOUBAN, $callFrom)) {
                //    return false;
                //}
                if (!IsDuplicateThabaNo($("#hfld_ProcessPlanCD").val())) {
                    return false;
                }
                debugger;
                //if (!IsActiveMachine(jsonResult.machinE_CD)) {
                //    return false;
                //}

                //if (!IsActiveStartOpr(jsonResult.stR_START_OPERATOR_CD)) {
                //    return false;
                //}

                //if (!IsActiveEndOpr(jsonResult.stR_END_OPERATOR_CD)) {
                //    return false;
                //}

                //if (!isFinishedQuantityValid()) {

                //    var message = $("#lblConfirmationMessageC_STR_EDIT_DELETE_002").text().trim(); // This message will be displayed on dialog box
                //    showMessageDialog(message, "txtFinishedQuantity", WARNING);
                //    return false;


                //}

                //if (!isActualDimentionValid())
                //{
                //    focusTab = "txtDimAfterX1";
                //    actualDimValue = false;
                //}

                //if (!isActualLengthValid()) {

                //    if (focusTab == "")
                //    {
                //        focusTab = "txtActualLength1AfterStraightening";
                //    }
                //        lengthValue = false;

                //}


                //var message = "";
                //if (lengthValue == false || actualDimValue == false) {

                //    if (lengthValue == false || actualDimValue == false) {
                //        message = $.trim($("#lblConfirmationMessageC_STR_EDIT_DELETE_001").text());
                //    }


                    //if (finishedWeightValue == false) {
                    //    if (message == "")
                    //    {
                    //        message = $.trim($("#lblConfirmationMessageC_STR_EDIT_DELETE_002").text());

                    //    } else
                    //    {
                    //        message = message + "<br>" + $.trim($("#lblConfirmationMessageC_STR_EDIT_DELETE_002").text());
                    //    }

                    //}


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
                        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_023").text().trim(); //ActDimOutOfTolerance
                        showMessageDialog(message, focusText, WARNING);
                        return false;
                    }
                }

                            

                if ($("#chkActualLength").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK && $("#txtLengthToleranceT1").val() != "") {

                    if ($("#txtActualLength1AfterStraightening").val().trim() == "") {
                      
                        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_024").text().trim(); // LengthOutOfTolerance
                        showMessageDialog(message, "txtActualLength1AfterStraightening", WARNING);
                        return false;
                    } else if ($("#txtActualLength2AfterStraightening").val().trim() == "") {

                        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_024").text().trim(); // LengthOutOfTolerance
                        showMessageDialog(message, "txtActualLength2AfterStraightening", WARNING);
                        return false;
                    }

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
                        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_024").text().trim(); // LengthOutOfTolerance
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
                
                if (!format.test($("#txtLengthT1").val()) && $("#ddlUnitType").val() != 2) {
                    getResponse = IsFinishedQuantityValid(weightValues);
                } else {
                    getResponse["IsSuccess"] = true;
                }
              
                if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true) {
                    var message = $("#lblConfirmationMessageC_STR_EDIT_DELETE_002").text().trim(); // FinishedQtyNotIn2PercentRange
                    showMessageDialog(message, "txtFinishedQuantity", WARNING);
                    
                } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false) {
                    var message = "";
                  
                        message = $.trim($("#lblConfirmationMessageC_STR_EDIT_DELETE_001").text());  //WeightIsInvalid
                  
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
                               
                                UpdateStraighteningDetails();
                                
                            }
                            else {
                                $("#txtFinishedQuantity").focus();
                              //  $("#" + focusTab).focus();
                                // return false;

                            }

                        },
                        centerVertical: true
                    });

                } else
                {
                    UpdateStraighteningDetails();
                }
            });
         
            
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


}

function isStrictNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
    return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
}

$("#chkActualDim").change(function (event) {
    toggleActualDimFieldsMandatory($(this));
});
$("#chkActualLength").click(function (event) {
    if ($("#chkActualLength").prop("checked") == true) {
       
        $("#txtActualLength1AfterStraightening").addClass("MandatoryTextBox");
        $("#txtActualLength2AfterStraightening").addClass("MandatoryTextBox");

    } else {
        $("#txtActualLength1AfterStraightening").removeClass("MandatoryTextBox");
        $("#txtActualLength2AfterStraightening").removeClass("MandatoryTextBox");
       
    }
});

$("#btnStartDateTime").click(function (event) {
    setStartSysDateTime();
});
$("#btnEndDateTime").click(function (event) {
    setEndSysDateTime();
});

//$("#ddlUnitType").change(function (event) {

//    toggleUnitTypeFieldsEnabled($(this));
//});

//$("#btnYobinaStartOpeartorSearch").click(function (event) {
//      getOperatorList($("#txtYobinaStartOperatorSearch").val(), $("#ddlStartOperatorNameValue"));
//});
//$("#btnYobinaEndOpeartorSearch").click(function (event) {
//    getOperatorList($("#txtYobinaEndOperatorSearch").val(), $("#ddlEndOperatorNameValue"));
//});


function UpdateStraighteningDetails() {
    debugger;
    var ajaxTime = new Date().getTime();

    var jsonDataForUpdate = JSON.stringify({
        STRAIGHTENING_CD: $("#hfld_StraighteningCd").val() ? $("#hfld_StraighteningCd").val() : null,
        PROCESS_PLAN_CD: $("#hfld_ProcessPlanCD").val() ? $("#hfld_ProcessPlanCD").val() : null,
        MACHINE_CD: $("#ddlModel").val() ? $("#ddlModel").val() : null,
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
        STR_IS_REMARKS_UPDATE: isStraightingRemarksUpdate,
        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_START_OPERATOR_CD: $("#ddlStartOperatorNameValue").find(":selected").val() ? $("#ddlStartOperatorNameValue").find(":selected").val() : null,
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
        STR_END_OPERATOR_CD: $("#ddlEndOperatorNameValue").find(":selected").val() ? $("#ddlEndOperatorNameValue").find(":selected").val() : null,
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
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/StraighteningDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           // disableEditDeleteButtons();
           // disableModelDataPanel();
          //  disableStartDataPanel();
            //   disableEndDataPanel();
            if (parseInt(response.message) > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }
            var jsonResult = response.message;
            var successMessage = $.trim($("#lblSuccessMessageS_STR_EDIT_DELETE_001").text());
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            Straightening2GridSearch(null);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnDelete").click(function (event) {

  DeleteStraighteningDetails();
});

function DeleteStraighteningDetails() {
    debugger;
    var ajaxTime = new Date().getTime();

    var MSG_CD = $("#hfld_MessageCD").val();

    if (MSG_CD == null || MSG_CD == "")
    {
        MSG_CD = 0;
    }
    var jsonData = JSON.stringify({
        STRAIGHTENING_CD: $("#hfld_StraighteningCd").val() ? $("#hfld_StraighteningCd").val() : null,
        PP_PROCESS_PLAN_CD: $("#hfld_ProcessPlanCD").val() ? $("#hfld_ProcessPlanCD").val() : null,
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
     
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/StraighteningDetailsDelete",
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
            //resetGlobalVariables();
            disableModelDataPanel();
            disableEditDeleteButtons();
            disableStartDataPanel();
            disableEndDataPanel();

            var successMessage = $("#lblSuccessMessageS_STR_EDIT_DELETE_002").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);

            Straightening2GridSearch(null);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);

        });
}
function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
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

    if (focusFieldID == 'txtActualDimBeforeCheck') {
        if ($("#txtActualDimDBeforeStraightening1").val().trim() == "") {
            focusFieldID = "txtActualDimDBeforeStraightening1";
        } else {
            focusFieldID = "txtActualDimDBeforeStraightening2";
        }
    }
    if (focusFieldID == 'txtActualLengthBeforeCheck') {
        if ($("#txtActualLengthBeforeStraightening1").val().trim() == "") {
            focusFieldID = "txtActualLengthBeforeStraightening1";
        } else {
            focusFieldID = "txtActualLengthBeforeStraightening2";
        }
    }

    //================================Start Data========================================


    if (focusFieldID == 'txtDimAfterXCheck') {
        if ($("#txtDimAfterX1").val().trim() == "") {
            focusFieldID = "txtDimAfterX1";
        }
        //else if ($("#txtDimAfterX2").val().trim() == "") {
        //    focusFieldID = "txtDimAfterX2";
        //} else if ($("#txtDimAfterX3").val().trim() == "") {
        //    focusFieldID = "txtDimAfterX3";
        //}

    }

    if (focusFieldID == 'txtActualLengthAftercheck') {
        if ($("#txtActualLength1AfterStraightening").val().trim() == "") {
            focusFieldID = "txtActualLength1AfterStraightening";
        } else {
            focusFieldID = "txtActualLength2AfterStraightening";
        }
    }

    //if (focusFieldID == 'txtDimAfterYcheck') {
    //    if ($("#txtDimAfterY1").val().trim() == "") {
    //        focusFieldID = "txtDimAfterY1";
    //    } else if ($("#txtDimAfterY2").val().trim() == "") {
    //        focusFieldID = "txtDimAfterY2";
    //    } else if ($("#txtDimAfterY3").val().trim() == "") {
    //        focusFieldID = "txtDimAfterY3";
    //    }

    //}

    if (focusFieldID == 'txtFinishedBarcheck') {
        if ($("#txtFinishedBarCount").val().trim() == "") {
            focusFieldID = "txtFinishedBarCount";
        } else {
            focusFieldID = "ddlUnitType";
        }
    }

   
    //================================End Data========================================
    
    if (message !== "") {
      
      showMessageDialog(message, focusFieldID, WARNING)

    }

    return returnVal;
}
function IsDropDownMandatory() {
    var returnVal = true;
    var message = "";
    if ($("#ddlStartOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_017").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_0017").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    return returnVal;
}

//function IsDuplicateThabaNo(chuuban, gyouban, $callFrom) {
//    var thabaList = [];
//    var Thaba1 = $("#txtThaba1").val();
//    var Thaba2 = $("#txtThaba2").val();
//    var Thaba3 = $("#txtThaba3").val();
//    var Thaba4 = $("#txtThaba4").val();
//    var t1 = "";
//    var t2 = "";
//    var t3 = "";
//    var t4 = "";
//    var Thaba_No_String = "";
//    var stratghteningCD = $("#hfld_StraighteningCd").val();
//    var returnval = true;
//    var thabamessage = "";

//    var chk;
//    thabaList.push(Thaba1);
//    if (Thaba2 != "") {
//        thabaList.push(Thaba2);
//    }
//    if (Thaba3 != "") {
//        thabaList.push(Thaba3);
//    }
//    if (Thaba4 != "") {
//        thabaList.push(Thaba4);
//    }
    
//    chk = new Set();
//    var duplicateThaba = thabaList.filter(n => chk.size === chk.add(n).size);


//    if (duplicateThaba.length > 0) {
//        if (duplicateThaba.length > 1) {
//            message = $("#lblErrorMessageE_STR_EDIT_DELETE_022").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
//        } else {
//            message = $("#lblErrorMessageE_STR_EDIT_DELETE_022").text().trim().replace("NNNN", duplicateThaba[0]);;
//        }

//        showMessageDialog(message, "txtThaba1", WARNING);
//        returnval = false;
//        return false;
//    }



//    if (Thaba1 != null && Thaba1 != 0) {
//        Thaba_No_String = Thaba1.toString();
//    }
//    if (Thaba2 != null && Thaba2 != 0) {
//        Thaba_No_String = Thaba_No_String + "','" + Thaba2;
//    }
//    if (Thaba3 != null && Thaba3 != 0) {
//        Thaba_No_String = Thaba_No_String + "','" + Thaba3;
//    }
//    if (Thaba4 != null && Thaba4 != 0) {
//        Thaba_No_String = Thaba_No_String + "','" + Thaba4;
//    }

//    $callFrom.data('requestRunning', true)
//    var ajaxTime = new Date().getDate();

//    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', Thaba_No_String: '" + Thaba_No_String + "', USER_NAME: '" + userName + "' }";

//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/Straightening2/GetThabaDetailsByThabaNumber",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonThabaDetailData + "}"
//        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, Thaba_No_String: Thaba_No_String })
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var jsonThabaDetailData = [];
//            jsonThabaDetailData = response.message;
//            arrayData = [];
           
//            $.each(jsonThabaDetailData, function (index, jsonObject) {

//                if (jsonObject.stR_TABA_NO) {
                    
//                    var thabaNumbers = jsonObject.stR_TABA_NO.split(",");
//                    var cd = jsonObject.straighteninG_CD;
//                    combinedDecisionOK = jsonObject.stR_DECISION;
//                    $.each(thabaNumbers, function (index, thabaNo) {
//                        thabaNo = thabaNo.trim();
//                        arrayData.push({ Thaba: thabaNo, str_CD: + cd, Decision: combinedDecisionOK });
//                       // registeredThabaNumbersAndDecision["" + thabaNo] = cd;
//                    });
//                }
//            });
//            //if ((($.inArray(Thaba1, [Thaba2, Thaba3, Thaba4]) > -1) && Thaba1 != "") ||
//            //    (($.inArray(Thaba2, [Thaba1, Thaba3, Thaba4]) > -1) && Thaba2 != "") ||
//            //    (($.inArray(Thaba3, [Thaba1, Thaba2, Thaba4]) > -1) && Thaba3 != "") ||
//            //    (($.inArray(Thaba4, [Thaba1, Thaba2, Thaba3]) > -1) && Thaba4 != "")) {
//            //    firstFieldID = "txtThaba1";
//            //    message = $("#lblErrorMessageE_Multiple_SameThaba").text().trim();
//            //    showMessageDialog(message, firstFieldID, WARNING);
//            //    returnval = false;
//            //    return false;
//            //}

//            for (var i = 0; i <= arrayData.length - 1; i++) {
//                if (Thaba1 == arrayData[i].Thaba && arrayData[i].Decision == combinedDecisionOK && arrayData[i].str_CD != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//                    t1 = Thaba1;

//                } else {
//                    if (t1 == "") {
//                        t1 = "";
//                    }
//                }

//            }

//            for (var i = 0; i <= arrayData.length - 1; i++) {
//                if (Thaba2 == arrayData[i].Thaba && arrayData[i].Decision == combinedDecisionOK && arrayData[i].str_CD != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//                    t2 = Thaba2;

//                } else {
//                    if (t2 == "") {
//                        t2 = "";
//                    }
//                }

//            }

//            for (var i = 0; i <= arrayData.length - 1; i++) {
//                if (Thaba3 == arrayData[i].Thaba && arrayData[i].Decision == combinedDecisionOK && arrayData[i].str_CD != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//                    t3 = Thaba3;

//                } else {
//                    if (t3 == "") {
//                        t3 = "";
//                    }
//                }

//            }

//            for (var i = 0; i <= arrayData.length - 1; i++) {
//                if (Thaba4 == arrayData[i].Thaba && arrayData[i].Decision == combinedDecisionOK && arrayData[i].str_CD != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//                    t4 = Thaba4;

//                } else {
//                    if (t4 == "") {
//                        t4 = "";
//                    }
//                }

//            }

//            //if (Thaba1 in registeredThabaNumbersAndDecision) {
//            //    if (parseInt(registeredThabaNumbersAndDecision["" + Thaba1]) != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//            //        t1 = Thaba1;
//            //    } else {
//            //        t1 = "";
//            //    }
//            //}
//            //if (Thaba2 in registeredThabaNumbersAndDecision) {
//            //    if (parseInt(registeredThabaNumbersAndDecision["" + Thaba2]) != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//            //        t2 = Thaba2;
//            //    } else {
//            //        t2 = "";
//            //    }
//            //}
//            //if (Thaba3 in registeredThabaNumbersAndDecision) {
//            //    if (parseInt(registeredThabaNumbersAndDecision["" + Thaba3]) != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//            //        t3 = Thaba3;
//            //    } else {
//            //        t3 = "";
//            //    }
//            //}
//            //if (Thaba4 in registeredThabaNumbersAndDecision) {
//            //    if (parseInt(registeredThabaNumbersAndDecision["" + Thaba4]) != stratghteningCD && $("#chkRestraighting").prop("checked") == false) {
//            //        t4 = Thaba4;
//            //    } else {
//            //        t4 = "";
//            //    }
//            //}

//            if (t1 != "") {
//                thabamessage = t1 + ", ";
//            }
//            if (t2 != "") {
//                thabamessage = thabamessage + t2 + ", ";
//            }
//            if (t3 != "") {
//                thabamessage = thabamessage + t3 + ", ";
//            }
//            if (t4 != "") {
//                thabamessage = thabamessage + t4;
//            }
//            if (thabamessage != "") {
//                firstFieldID = "txtThaba1";
//                message = $("#lblErrorMessageE_STR_EDIT_DELETE_016").text().trim().replace("NNN", thabamessage.replace(/(.+), $/, '$1'));    //lblErrorMessageE_STRAIGHTENING_ENTRY_005
//                showMessageDialog(message, firstFieldID, WARNING);
//                returnval = false;
//            }
//            else {

//                returnval = true;
//            }

//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
//            showMessageDialog(message, focusFieldID, ERROR);

//        });

//    return returnval;
//}

function IsDuplicateThabaNo(processPlanCD) {
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
    var stratghteningCD = $("#hfld_StraighteningCd").val();
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_022").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_022").text().trim().replace("NNNN", duplicateThaba[0]);;
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

    //$callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    //var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', Thaba_No_String: '" + Thaba_No_String + "', USER_NAME: '" + userName + "' }";
    var jsonData = JSON.stringify({
        PROCESS_PLAN_CD: processPlanCD,
        TABA_NO_STRING: Thaba_No_String,
        STRAIGHTENING_CD: stratghteningCD,
        ISCHECKED: $('#chkRestraighting').is(':checked') ? 1 : 0,
        TABA_NO_1: $("#txtThaba1").val(),
        TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",        
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            var sucess = response.isSuccess
            if (sucess == false) {
                firstFieldID = "txtThaba1";
                message = $("#lblErrorMessageE_STR_EDIT_DELETE_016").text().trim().replace("NNN", jsonResult);
                showMessageDialog(message, firstFieldID, WARNING);
                returnval = false;
            } else {
                returnval = true;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

    return returnval;
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

function IsEndDateValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_008").text().trim();
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
        var message = $("#HdfOperatorInactiveMessage").val();
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


function IsActiveProcessDefect(processDefectCD, dropdownNo, isProcessDefect) {
    // var IsProcessDefect = "IsProcessDefect" + dropdownNo + "Exists";
    if (isProcessDefect == false && ($("#ddlNGReason" + dropdownNo)).val() == processDefectCD) {
        var focusFieldID = "ddlNGReason" + dropdownNo;
        var message = $("#hdfNGReasonInactiveMessage").val();
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    else {
        return true;
    }
}


function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

function setDecimalFormats() {
    $("#txtUpperLowerRollerAngle1, #txtUpperLowerRollerAngle2, #txtTonnage").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });

    $("#txtUpperRoleElevation, #txtAEdgeDistortion1, #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtBEdgeDistortion1, #txtBEdgeDistortion2, #txtBEdgeDistortion3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtActualDimDBeforeStraightening1, #txtActualDimDBeforeStraightening2, #txtActualDimDBeforeStraightening3, #txtActualDimDBeforeStraightening4, #txtActualDimDAfterStraightening1, #txtActualDimDAfterStraightening2, #txtActualDimDAfterStraightening3, #txtActualDimDAfterStraightening4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtActualLengthBeforeStraightening1, #txtActualLengthBeforeStraightening2, #txtActualLengthAfterStraightening1, #txtActualLengthAfterStraightening2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
    });

    $("#txtDimAfterX1, #txtDimAfterX2, #txtDimAfterX3, #txtDimAfterY1, #txtDimAfterY2, #txtDimAfterY3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
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

    $("#txtActualLength1AfterStraightening, #txtActualLength2AfterStraightening").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
    });

    
}



function Mask() {
    $("#txtOperationDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtOperationDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtFinishedBarCount").keyup(function (event) {
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

var gridViewScroll = null;
function FreeseHeaders() {
    gridViewScroll = new GridViewScroll({
        elementID: "grdvStraighteningDetails",
        width: 1027,                                //82 +105 +80 +100 +115+100 = 582 +100+110+125+25+60+130+125+60+80+63+63+80+80+80 = 1763 +60
        height: 630,
        freezeColumn: false,
        freezeFooter: true,
        freezeColumnCssClass: "GridViewScrollItemFreeze",
        freezeFooterCssClass: "GridViewScrollFooterFreeze",
        freezeHeaderRowCount: 1,
        freezeColumnCount: 1,
        onscroll: function (scrollTop, scrollLeft) {
            console.log(scrollTop + " - " + scrollLeft);
        }
    });
    gridViewScroll.enhance();
}


function GetThabaDetailsByChuuban(chuuban, gyouban, stratghteningCD) {
    // $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();
    var inputThaba1 = $("#txtThaba1").val().trim();
    var inputThaba2 = $("#txtThaba2").val().trim();
    var inputThaba3 = $("#txtThaba3").val().trim();
    var inputThaba4 = $("#txtThaba4").val().trim();

    var jsonData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "', stratghteningCD : '" + stratghteningCD + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetThabaDetailsByChuubanAndCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",
        data: JSON.stringify({ chuuban: chuuban, gyouban: String(gyouban), stratghteningCD: stratghteningCD })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonThabaDetailData1 = $.parseJSON(response.d);
            var thabaNumbers = null;
            $.each(response.message, function (key, jsonObj) {

                var message;   //lblErrorMessageE_STRAIGHTENING_ENTRY_005

                if (jsonObj.tabA_NO_1 != null) {
                    if (inputThaba1 === jsonObj.tabA_NO_1 || inputThaba1 === jsonObj.tabA_NO_2 || inputThaba1 === jsonObj.tabA_NO_3 || inputThaba1 === jsonObj.tabA_NO_4) {
                        if ($('#chkRestraightening').prop("checked") == false) {
                            message = $("#lblErrorMessageE_STR_EDIT_DELETE_016").text().trim().replace("NNNN", inputThaba1);
                            showMessageDialog(message, "txtThaba1", WARNING);
                            return true;
                        }
                    }
                    if (inputThaba2 === jsonObj.tabA_NO_1 || inputThaba2 === jsonObj.tabA_NO_2 || inputThaba2 === jsonObj.tabA_NO_3 || inputThaba2 === jsonObj.tabA_NO_4) {
                        if ($('#chkRestraightening').prop("checked") == false) {
                            message = $("#lblErrorMessageE_STR_EDIT_DELETE_016").text().trim().replace("NNNN", inputThaba2);
                            showMessageDialog(message, "txtThaba2", WARNING);
                            return true;
                        }
                    }
                    if (inputThaba3 === jsonObj.tabA_NO_1 || inputThaba3 === jsonObj.tabA_NO_2 || inputThaba3 === jsonObj.tabA_NO_3 || inputThaba3 === jsonObj.tabA_NO_4) {
                        if ($('#chkRestraightening').prop("checked") == false) {
                            message = $("#lblErrorMessageE_STR_EDIT_DELETE_016").text().trim().replace("NNNN", inputThaba3);
                            showMessageDialog(message, "txtThaba3", WARNING);
                            return true;
                        }
                    }
                    if (inputThaba4 === jsonObj.tabA_NO_1 || inputThaba4 === jsonObj.tabA_NO_2 || inputThaba4 === jsonObj.tabA_NO_3 || inputThaba4 === jsonObj.tabA_NO_4) {
                        if ($('#chkRestraightening').prop("checked") == false) {
                            message = $("#lblErrorMessageE_STR_EDIT_DELETE_016").text().trim().replace("NNNN", inputThaba4);
                            showMessageDialog(message, "txtThaba4", WARNING);
                            return true;
                        }
                    }

                }



            });

            // alert(registeredThabaNumbersWithDecisionOK);
            return false;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
//***************************************************
//****************************************************

function btnSearch_Click(allowNull = false) {
    debugger;
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $.trim($("#txtDimDMin").val()) ? $.trim($("#txtDimDMin").val()) : 0;//$("#txtDimDMin").val();
    var PROD_DIM_D_MAX = $.trim($("#txtDimDMax").val()) ? $.trim($("#txtDimDMax").val()) : 0; //$("#txtDimDMax").val();
    var SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $.trim($("#txtLengthSearchMin").val()) ? $.trim($("#txtLengthSearchMin").val()) : 0;// $("#txtLengthSearchMin").val();
    var PROD_LENGTH_MAX = $.trim($("#txtLengthSearchMax").val()) ? $.trim($("#txtLengthSearchMax").val()) : 0; //$("#txtLengthSearchMax").val();
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
        errorMessage = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_020").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_021").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_021").text());
        }
    }

    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }

    if (allowNull || CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PROD_DIM_D_MIN.length > 0 || PROD_DIM_D_MAX.length > 0 || SHAPE.length > 0 || PROD_LENGTH_MIN.length > 0 || PROD_LENGTH_MAX.length > 0) {

        var ajaxTime = new Date().getTime();
        
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening2/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX, SHAPE: SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX,
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
                        url: "/Straightening2/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                debugger;
                                $("#divStraighteningGridMain").html(response1.message);

                                //$("#btnLnkEdit").focus();

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                            showMessageDialog(message, focusFieldID, ERROR);

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
                message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {

        message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_019").text());
        showMessageDialog(message, 'txtChuubanSearch', WARNING);
    }
}

function bindOperatorList(search) {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: APIURL + "/Straightening/EmployeeList?name=" + search,
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
    Straightening2GridSearch(selectedColumns);
}

function Straightening2GridSearch(sortFields) {

    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $.trim($("#txtDimDMin").val()) ? $.trim($("#txtDimDMin").val()) : 0;//$("#txtDimDMin").val();
    var txtDimDMax = $.trim($("#txtDimDMax").val()) ? $.trim($("#txtDimDMax").val()) : 0; //$("#txtDimDMax").val();   
    var txtShap = $("#txtShap").val();    
    var txtLengthSearchMin = $.trim($("#txtLengthSearchMin").val()) ? $.trim($("#txtLengthSearchMin").val()) : 0;// $("#txtLengthSearchMin").val();
    var txtLengthSearchMax = $.trim($("#txtLengthSearchMax").val()) ? $.trim($("#txtLengthSearchMax").val()) : 0; //$("#txtLengthSearchMax").val();

    var userId = $("#hdfLoginId").val();

    if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0) {


        var ajaxTime = new Date().getTime();
            var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening2/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: txtChuubanSearch, CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin, PROD_DIM_D_MAX: txtDimDMax, SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax,
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
                        url: "/Straightening2/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divStraighteningGridMain").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                            showMessageDialog(message, focusFieldID, ERROR);
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
                message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                showMessageDialog(message, focusFieldID, ERROR);
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
                url: APIURL + "/Straightening2/GridSearch",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
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
                            url: "/Straightening2/GridSearch",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)
                            //data: "{jsonData : " + jsonData + "}"
                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#divStraighteningGridMain").html(response1.message);

                                }
                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                var errorCode = $.parseJSON(xhr.responseText).Message;

                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                                showMessageDialog(message, focusFieldID, ERROR);
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
                    message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        }
       
    }
    closepopup();
}

function btnSortReset_Click() {
    InitSortPopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}

function InitSortPopup() {
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/Straightening2/InitializeSortPopup",
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
        url: APIURL + "/Straightening2/GetMachineList",
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
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
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '"> ' + jsonObj.name + '</option>'));
    });


}

$(document).on('click', '#btnDownload', function () {
    $("#tblStraightening2").table2csv({
        separator: ',',
        newline: '\n',
        //quoteFields: true,
        excludeColumns: '.excludeclm',
        excludeRows: '',
        trimContent: true
    });
})


function bindEvents() {
   
    $("#txtNGQuantity1").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGQuantity2").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtNGQuantity3").keyup(function (event) {
        OKQtyRegionClassChange();
    });
    $("#txtActualDim4, #txtActualDim5, #txtActualDim6").change(function (event) {
        SetActDimD456();

    });

    $("#txtRemarks").change(function (event) {
        if (oldStraightingRemarks != $.trim($(this).val())) {
            isStraightingRemarksUpdate = 1;
        }
    });

    $("#btnInitialize").click(function (event) {
        switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
        initialize();
        window.resizeTo(1090, 890);
        location.reload();
        $("#txtChuubanSearch").focus();

       // initialize();
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
        url:"/Straightening2/GetUnitType",
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
    //$("#ddlUnitType").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindTPTestDropdown(jsonResult) {
    $("#ddlTPTest").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
        contentType: "application/json; charset=utf-8"
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindNextProcessDropdown(jsonResult) {

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

function setDecision() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening2/GetDecision",
        method: "GET",
        contentType: "application/json; charset=utf-8",
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function binddecisionDropdown(jsonResult) {
    $("#ddlDecision").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
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
        url:"/Straightening2/GetExteriorCondition",
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
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
        url:"/Straightening2/GetBendStatus",
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindBendStatusDropdown(jsonResult) {
    $("#ddlBendStatus").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
        url: APIURL + "/Straightening2/GetProcessDefect",
        method: "GET",
        contentType: "application/json; charset=utf-8"
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
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
    //    $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    //    $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    //    $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    //});
}

$(document).ready(function () {
    debugger;
    $("#txtChuubanSearch").focus();
    setUnitType();
    bindTPTestList();
    setProcessDefectList();
    setNextProcess();
    setDecision();
    setBendStatus();
    setExteriorConditionKubunGet();
    disableModelDataPanel();
    disableStartDataPanel();
    disableEndDataPanel();
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // CheckNumberValidation();
    SetGlobalVariables();
    window.resizeTo(1100, 860);

   // getOperatorList("", $("#ddlStartOperatorNameValue"));
   // getOperatorList("", $("#ddlEndOperatorNameValue"));
    
    bindEvents();
    
    disableEditDeleteButtons();

    userName = $("#hdfUserName").val();
    Mask();
    // FreeseHeaders();
   // btnSearch_Click(true);

  //  $('#tdDownload').show();
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
    $("#btnUpdate").focus();
});

//Barcode scan on list page.
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
            url: APIURL + "/Straightening2/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});