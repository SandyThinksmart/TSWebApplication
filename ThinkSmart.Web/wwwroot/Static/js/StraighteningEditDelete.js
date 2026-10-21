
var $btnLnkEdit;
var $btnDelete;
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
var lengthValue = true;
var finishedWeightValue = true;
var DimentionValue = true;
var operatorCD=0;

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
    combinedDecisionOK = "";
    operatorCD = 0;
    $(".FocusDefault").focus();

    clearPanel($("#SearchContent"));

    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    //clearTable($("#grdvProductList"), "", "GridNormalRow");
    //clearTable($("#grdvProductList"), "", "GridAlternateRow");

    clearTable($("#tblStraightening"), "", "GridNormalRow");
    clearTable($("#tblStraightening"), "", "GridAlternateRow");

    disableEditDeleteButtons();
    resetGlobalVariables();
  //  fillEmptyGrid();
   
}


function fillEmptyGrid() {
    var emptyList = [];
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#tblStraightening").html(response1.message);

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
    $.each($panel.find("span[id^='txt']"), function (index, label) {
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
    //if ($("#chkActualDim").prop('checked', true)) {
    if ($this.is(':checked')) {
        $("#txtActualDimDAfterStraightening1").addClass("Mandatory");
        //$("#txtActualDimDAfterStraightening2").addClass("Mandatory");
        //$("#txtActualDimDAfterStraightening3").addClass("Mandatory");
        //$("#txtActualDimDAfterStraightening4").addClass("Mandatory");
    } else {
        $("#txtActualDimDAfterStraightening1").removeClass("Mandatory");
        //$("#txtActualDimDAfterStraightening2").removeClass("Mandatory");
        //$("#txtActualDimDAfterStraightening3").removeClass("Mandatory");
        //$("#txtActualDimDAfterStraightening4").removeClass("Mandatory");
    }
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

//function toggleUnitTypeFieldsEnabled($this) {
//    if ($this.find(":selected").val() == '1') {
//        $("#chkFinishedQuantity").removeAttr("disabled");
//        $("#chkFinishedQuantity").prop('checked', true);
//    } else {
//        $("#chkFinishedQuantity").attr("disabled", "disabled");
//        $("#chkFinishedQuantity").prop('checked', false);
//    }
//}

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
        url: APIURL + "/Straightening/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            //var jsonResult = $.parseJSON(response.d);
            var sysDateTime = response.message;

            $("#txtStartDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

            //$("#txtStartTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
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
        url: APIURL + "/Straightening/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

            $("#txtEndDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            /* $("#txtEndTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });*/
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
    //var tab = $find('TabContainerPMS');
    //var fromTabHeaderid = "#" + ($tabFrom.get(0).id) + "_tab";
    //var toTabHeaderid = "#" + ($tabTo.get(0).id) + "_tab";

    //if ($(fromTabHeaderid).hasClass("ajax__tab_active")) {
    //    tab.set_activeTabIndex(1); // active the second tab panel       
    //} else {
    //    tab.set_activeTabIndex(0); // active the second tab panel        
    //}

    //var fromTabHeaderid = "#" + ($tabFrom.get(0).id) + "_tab";
    //var toTabHeaderid = "#" + ($tabTo.get(0).id) + "_tab";
    //$(fromTabHeaderid).removeClass("ajax__tab_active");
    //$(toTabHeaderid).addClass("ajax__tab_active");
    //$tabFrom.css({ 'visibility': "" });
    //$tabTo.css({ 'visibility': "" }).css({ 'display': "" });
    //$tabTo.css({ 'visibility': "visible" });
    //$tabFrom.css({ 'visibility': "hidden" }).css({ 'display': "none" });

    enableEditDeleteButtons();
}

function setStraighteningDetails(jsonResult) {
    //console.log(jsonResult);
    debugger;
    $("#txtYobinaStartOperatorSearch").val("");
    $("#txtYobinaEndOperatorSearch").val("");
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    $("#hdfMachineCD").val(jsonResult.stR_MACHINE_CD);
    $("#hfld_MessageCD").val(jsonResult.stR_MSG_CD);
    $("#hdfProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
   // $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#hfld_StraighteningCd").val(jsonResult.stR_STRAIGHTENING_CD);
    $("#hfld_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);


    /*Check machine is Active/Inactive*/

    //$('#ddlModel option').each(function () {
    //    if (this.value == jsonResult.stR_MACHINE_CD)
    //        IsExists = true;
    //});

    //// If machine is inactive then first add it in drop down
    //if (IsExists === false) {
    //    $("#ddlModel").append(new Option(jsonResult.stR_MACHINE_NAME, jsonResult.stR_MACHINE_CD));
    //}

    $("#ddlModel").val(jsonResult.stR_MACHINE_CD);
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

    $("#txtConditionT1").val(jsonResult.conditioN_DESCRIPTION); 
    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);  //PP_MATL_DIM_D_STRING
    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txt_HostName").val(jsonResult.producT_NYUKO_CD);
    
    $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);    //PP_PROD_DIM_D_STRING
    $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
   // $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    }
    else {
        $("#txtLengthToleranceT1").val('');
    }
    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);            //text(jsonResult.PP_NO_OF_UNITS + '' + jsonResult.PP_UNIT);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);

    $("#hfld_StraightingCd").val(jsonResult.stR_STRAIGHTENING_CD);
    $("#txtStartDate").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    
    //$("#txtStartTime").val($.format.date(new Date(jsonResult.stR_START_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#hiStartOperator").val(jsonResult.stR_START_OPERATOR);
    $("#ddlTPTest").val(jsonResult.stR_TP_TEST);
    $("#txtFurnaceNum").val(jsonResult.stR_FURNACE_NO);
    $("#txtNoOfCoils").val(jsonResult.pP_NO_OF_COILS);
    $("#txtFinishedQuantity").val(jsonResult.stR_QTY);
    $("#txtMetallicLabelNo").val(jsonResult.stR_METALLIC_PLATE_CODE);
    $("#txtUpperLowerRollerAngle1").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE1);
    $("#txtUpperLowerRollerAngle2").val(jsonResult.stR_UPPER_LOWER_ROLLER_ANGLE2);
    $("#txtUpperRoleElevation").val(jsonResult.stR_UPPER_ROLLER_ELEVATION);
    $("#txtTonnage").val(jsonResult.stR_TONNAGE);
    if ($.isNumeric(jsonResult.stR_RESTRAIGHTING) && parseInt(jsonResult.stR_RESTRAIGHTING) == 1) {
        $("#chkRestraightening").prop('checked', true);
    }
    else {
        $("#chkRestraightening").prop('checked', false);
    }
    $("#txtEndDate").val($.format.date(new Date(jsonResult.stR_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //$("#txtEndTime").val($.format.date(new Date(jsonResult.stR_END_DATE_STRING), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
    $("#hiEndOperator").val(jsonResult.stR_END_OPERATOR);
    $("#ddlDecision").val(jsonResult.stR_DECISION);
    $("#ddlUnitType").val(jsonResult.stR_UNIT_TYPE);

    if ($.isNumeric(jsonResult.stR_DIM_CHK) && parseInt(jsonResult.stR_DIM_CHK) == 1) {
        $("#chkActualDim").prop('checked', true);
        //$("#txtActualDimDAfterStraightening1").addClass("Mandatory");
        toggleActualDimFieldsMandatory($("#chkActualDim"));
    }
    else {
        $("#chkActualDim").prop('checked', false);
        // $("#txtActualDimDAfterStraightening1").removeAttr("Mandatory");
        toggleActualDimFieldsMandatory($("#chkActualDim"));
    }
    //toggleActualDimFieldsMandatory($("#chkActualDim"));
    if ($.isNumeric(jsonResult.stR_LENGTH_CHK) && parseInt(jsonResult.stR_LENGTH_CHK) == 1) {
        $("#chkActualLength").prop('checked', true);
    }
    else {
        $("#chkActualLength").prop('checked', false);
    }
    toggleActualLengthFieldsMandatory($("#chkActualLength"));
    $("#txtFinishedQuantity").val(jsonResult.stR_QTY);
    if ($.isNumeric(jsonResult.stR_FINISHED_QTY_CHK) && parseInt(jsonResult.stR_FINISHED_QTY_CHK) == 1) {
        $("#chkFinishedQuantity").prop('checked', true);
    }
    else {
        $("#chkFinishedQuantity").prop('checked', false);
    }


    $("#txtAEdgeDistortion1").val(jsonResult.stR_A_SIDE_FURE1);
    $("#txtAEdgeDistortion2").val(jsonResult.stR_A_SIDE_FURE2);
    $("#txtAEdgeDistortion3").val(jsonResult.stR_A_SIDE_FURE3);
    $("#ddlBendStatus").val(jsonResult.stR_BEND_STATUS);

    $("#txtBEdgeDistortion1").val(jsonResult.stR_B_SIDE_FURE1);
    $("#txtBEdgeDistortion2").val(jsonResult.stR_B_SIDE_FURE2);
    $("#txtBEdgeDistortion3").val(jsonResult.stR_B_SIDE_FURE3);
    $("#ddlExternalSurfaceCondition").val(jsonResult.stR_EXT_TEXTURE);

    $("#txtThaba1").val(jsonResult.stR_TABA_NO_1);
    $("#txtThaba2").val(jsonResult.stR_TABA_NO_2);
    $("#txtThaba3").val(jsonResult.stR_TABA_NO_3);
    $("#txtThaba4").val(jsonResult.stR_TABA_NO_4);
    $("#txtActualDimDBeforeStraightening1").val(jsonResult.stR_DIM_BEFORE_1);
    $("#txtActualDimDBeforeStraightening2").val(jsonResult.stR_DIM_BEFORE_2);
    $("#txtActualDimDBeforeStraightening3").val(jsonResult.stR_DIM_BEFORE_3);
    $("#txtActualDimDBeforeStraightening4").val(jsonResult.stR_DIM_BEFORE_4);
    $("#txtActualDimDAfterStraightening1").val(jsonResult.stR_DIM_AFTER_1);
    $("#txtActualDimDAfterStraightening2").val(jsonResult.stR_DIM_AFTER_2);
    $("#txtActualDimDAfterStraightening3").val(jsonResult.stR_DIM_AFTER_3);
    $("#txtActualDimDAfterStraightening4").val(jsonResult.stR_DIM_AFTER_4);
    $("#txtActualDimDAfterStraightening5").val(jsonResult.stR_DIM_AFTER_5);
    $("#txtActualDimDAfterStraightening6").val(jsonResult.stR_DIM_AFTER_6);
    $("#txtActualLengthBeforeStraightening1").val(jsonResult.stR_LENGTH_BEFORE_1);
    $("#txtActualLengthBeforeStraightening2").val(jsonResult.stR_LENGTH_BEFORE_2);
    $("#txtActualLengthAfterStraightening1").val(jsonResult.stR_LENGTH_AFTER_1);
    $("#txtActualLengthAfterStraightening2").val(jsonResult.stR_LENGTH_AFTER_2);
    $("#txtReservationNumber").val(jsonResult.stR_RESERVATION_NUMBER);
    $("#txtTonnage").val(jsonResult.stR_TONNAGE);
    $("#txtFinishedBarCount").val(jsonResult.stR_BARS);
    $("#ddlNextProcess").val(jsonResult.stR_NEXT_PROCESS);
    // $("#ddlStartOperatorNameValue").val(toString(jsonResult.stR_START_OPERATOR));
    //$("#ddlEndOperatorNameValue").val(toString(jsonResult.stR_END_OPERATOR));


    debugger;
    //$('#ddlStartOperatorNameValue option').each(function () {
    //    if (this.value == jsonResult.stR_START_OPERATOR)
    //        IsStartOprExists = true;
    //});

   //  If start opr is inactive then first add it in drop down
    //if (IsStartOprExists === false)
    //    $("#ddlStartOperatorNameValue").append(new Option(jsonResult.stR_START_OPERATOR_NAME, jsonResult.stR_START_OPERATOR));

  //  $("#ddlStartOperatorNameValue").val(jsonResult.stR_START_OPERATOR);
    //$("#ddlModel").val(jsonResult.stR_MACHINE_CD);
    /*Check end opr is Active/Inactive*/
    //$('#ddlEndOperatorNameValue option').each(function () {
    //    if (this.value == jsonResult.stR_END_OPERATOR)
    //        IsEndOprExists = true;
    //});

    // If end opr is inactive then first add it in drop down
    //if (IsEndOprExists === false)
    //    $("#ddlEndOperatorNameValue").append(new Option(jsonResult.stR_END_OPERATOR_NAME, jsonResult.stR_END_OPERATOR));

   

    startOperatorCD = jsonResult.stR_START_OPERATOR;
    operatorCD = startOperatorCD;
   // getOperatorList("", $("#ddlStartOperatorNameValue"));
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), operatorCD)

    endOperatorCD = jsonResult.stR_END_OPERATOR;
    operatorCD = endOperatorCD;
   // getOperatorList("", $("#ddlEndOperatorNameValue"));
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), operatorCD)

    $("#ddlStartOperatorNameValue").val(jsonResult.stR_START_OPERATOR);
    $("#ddlEndOperatorNameValue").val(jsonResult.stR_END_OPERATOR);
    
    $("#txtNGQuantity1").val(jsonResult.stR_NG_BARS1);
    if (jsonResult.stR_NG_BARS1 > 0 && jsonResult.stR_NG_BARS1 != 0) {
        $("#ddlNGReason1").val(jsonResult.stR_NG_DETAILS1)
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#ddlNGReason1").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason1").val(jsonResult.stR_NG_DETAILS1);
        ChangeControlDisplay('#ddlNGReason1', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlNGReason1").attr("disabled", "disabled")
        $("#txtNGQuantity1").val("");
    }

    $("#txtNGQuantity2").val(jsonResult.stR_NG_BARS2);
    if (jsonResult.stR_NG_BARS2 > 0 && jsonResult.stR_NG_BARS2 != 0) {
        $("#ddlNGReason2").val(jsonResult.stR_NG_DETAILS2)
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#ddlNGReason2").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason2").val(jsonResult.stR_NG_DETAILS2);
        ChangeControlDisplay('#ddlNGReason2', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlNGReason2").attr("disabled", "disabled")
        $("#txtNGQuantity2").val("");
    }

    $("#txtNGQuantity3").val(jsonResult.stR_NG_BARS3);
    if (jsonResult.stR_NG_BARS3 > 0 && jsonResult.stR_NG_BARS3 != 0) {
        $("#ddlNGReason3").val(jsonResult.stR_NG_DETAILS3)
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory')
        $("#ddlNGReason3").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason3").val(jsonResult.stR_NG_DETAILS3);
        ChangeControlDisplay('#ddlNGReason3', false, 'MandatoryDropdown DropdownClass Mandatory', 'dropListClass');
        $("#ddlNGReason3").attr("disabled", "disabled")
        $("#txtNGQuantity3").val("");
    }


    // $("#ddlNGReason1").val(jsonResult.stR_NG_DETAILS1);
    ////$("#ddlNGReason1").val(jsonResult.stR_NG_DETAILS1);
    ////if (jsonResult.stR_NG_BARS2 == 0) {
    ////    $("#txtNGQuantity2").val("");
    ////   //OKQtyRegionClassChange();

    ////    $("#ddlNGReason2").attr("disabled", "disabled");

    ////}
    ////else {
    ////    $("#txtNGQuantity2").val(jsonResult.stR_NG_BARS2);
    ////    //OKQtyRegionClassChange();
    ////}

    //$("#ddlNGReason2").val(jsonResult.stR_NG_DETAILS2);

    ////$("#txtNGQuantity3").val(jsonResult.stR_NG_BARS3);
    //if (jsonResult.stR_NG_BARS3 == 0) {
    //    $("#txtNGQuantity3").val("");
    //   //OKQtyRegionClassChange();
    //    $("#ddlNGReason3").attr("disabled", "disabled");
    //}
    //else {
    //    $("#txtNGQuantity3").val(jsonResult.stR_NG_BARS3);
    //   //OKQtyRegionClassChange();
    //}

    //$("#ddlNGReason3").val(jsonResult.stR_NG_DETAILS3);




    $("#txtRemarks").val(jsonResult.stR_REMARKS);

    dimTolMin = jsonResult.pP_DIM_TOL_MIN;
    dimTolMax = jsonResult.pP_DIM_TOL_MAX;
    dimStd = jsonResult.pP_PROD_DIM_D;
    lenTolMin = jsonResult.pP_LENGTH_TOL_MIN;
    lenTolMax = jsonResult.pP_LENGTH_TOL_MAX;
    lenStd = jsonResult.pP_PROD_LENGTH;
    setDecimalFormats();
    $(".FocusDefault").focus();


    oldStraightingRemarks = $.trim(jsonResult.stR_REMARKS);
}




function OKQtyRegionClassChange() {
    debugger;
    var txtNGWeight1 = $("#txtNGQuantity1").val();
    var ddlDefects1 = document.getElementById("ddlNGReason1");
    if (txtNGWeight1 > 0) {
        ChangeControlDisplay('#ddlNGReason1', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason1").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason1").removeClass('MandatoryDropdown DropdownClass Mandatory ');
        ChangeControlDisplay('#ddlNGReason1', true, ' dropListClass', 'TextBoxClass TextFieldsClass form-control', 0);
        $("#ddlNGReason1 option:selected").prop("selected", false);
        $("#ddlNGReason1").attr("disabled", "disabled");
    }
    var txtNGWeight2 = $("#txtNGQuantity2").val();
    var ddlDefects2 = document.getElementById("ddlNGReason2");
    if (txtNGWeight2 > 0) {
        ChangeControlDisplay('#ddlNGReason2', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason2").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason2").removeClass('MandatoryDropdown DropdownClass Mandatory ');
        ChangeControlDisplay('#ddlNGReason2', true, ' dropListClass', 'TextBoxClass TextFieldsClass form-control', 0);
        $("#ddlNGReason2 option:selected").prop("selected", false);
        $("#ddlNGReason2").attr("disabled", "disabled");
    }
    var txtNGWeight3 = $("#txtNGQuantity3").val();
    var ddlDefects3 = document.getElementById("ddlNGReason3");
    if (txtNGWeight3 > 0) {
        ChangeControlDisplay('#ddlNGReason3', true, 'dropListClass', 'MandatoryDropdown DropdownClass Mandatory ', 0);
        $("#ddlNGReason3").removeAttr("disabled")
    }
    else {
        $("#ddlNGReason3").removeClass('MandatoryDropdown DropdownClass Mandatory ');
        ChangeControlDisplay('#ddlNGReason3', true, ' dropListClass', 'TextBoxClass TextFieldsClass form-control', 0);
        $("#ddlNGReason3 option:selected").prop("selected", false);
        $("#ddlNGReason3").attr("disabled", "disabled");
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
        url: APIURL + "/Straightening/GetStraighteningDetailsByStraighteningCD?STR_STRAIGHTENING_CD=" + straighteningCD,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = null;
            jsonResult = response.message;
            setStraighteningDetails(jsonResult);
            var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                $("#chkActualLength").removeAttr("disabled", "disabled");

            } else {
                $("#chkActualLength").attr("disabled", "disabled");
                $("#chkActualLength").prop("checked", false);
                $("#txtActualLengthAfterStraightening1").removeClass("Mandatory");
            }

            //checkbox uncheck Remove length mandatory
            if ($("#chkActualLength").prop("checked") != true) {
                $("#txtActualLengthAfterStraightening1").removeClass("Mandatory");
            }
            $("#btnUpdate").unbind("click");
            $("#btnUpdate").click(function (event) {
                debugger;

                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }
                if (!IsDropDownMandatory()) {
                    return false;
                }

                if (!IsEndDataValid()) {
                    return false;
                }
                if (jsonResult.pP_GYOUBAN.toString().length == 1) {
                    jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
                }

                if (!IsDuplicateThabaNo($("#hdfProcessPlanCD").val())) {
                    return false;
                }
                if (!checkDate($("#txtStartDate"))) {
                    var message = $("#lblErrorMessageE_STR_EDIT_DELETE_014").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, $("#txtStartDate"), WARNING);
                    return false;
                }
                if (!checkDate($("#txtEndDate"))) {
                    var message = $("#lblErrorMessageE_STR_EDIT_DELETE_015").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, $("#txtEndDate"), WARNING);
                    return false;
                }
                //if (!IsActiveMachine(jsonResult.stR_MACHINE_CD)) {
                //    return false;
                //}
                //if (!IsActiveStartOpr(jsonResult.stR_START_OPERATOR)) {
                //    return false;
                //}
                //if (!IsActiveEndOpr(jsonResult.stR_END_OPERATOR)) {
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
                ///

                if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == 10) {
                    var dimValues = {};
                    dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
                    dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
                    dimValues["DIM1"] = $("#txtActualDimDAfterStraightening1").val() ? $("#txtActualDimDAfterStraightening1").val() : null;
                    dimValues["DIM2"] = $("#txtActualDimDAfterStraightening2").val() ? $("#txtActualDimDAfterStraightening2").val() : null;
                    dimValues["DIM3"] = $("#txtActualDimDAfterStraightening3").val() ? $("#txtActualDimDAfterStraightening3").val() : null;
                    dimValues["DIM4"] = $("#txtActualDimDAfterStraightening4").val() ? $("#txtActualDimDAfterStraightening4").val() : null;
                    dimValues["DIM5"] = $("#txtActualDimDAfterStraightening5").val() ? $("#txtActualDimDAfterStraightening5").val() : null;;
                    dimValues["DIM6"] = $("#txtActualDimDAfterStraightening6").val() ? $("#txtActualDimDAfterStraightening6").val() : null;;

                    var result = IsActualDimentionValid(dimValues);
                    if (result.IsSuccess == false) {
                        var focusText = "";
                        if (result.InvalidDim == $("#txtActualDimDAfterStraightening1").val()) {
                            focusText = "txtActualDimDAfterStraightening1";
                        }
                        if (result.InvalidDim == $("#txtActualDimDAfterStraightening2").val() && focusText == "") {
                            focusText = "txtActualDimDAfterStraightening2";
                        }
                        if (result.InvalidDim == $("#txtActualDimDAfterStraightening3").val() && focusText == "") {
                            focusText = "txtActualDimDAfterStraightening3";
                        }
                        if (result.InvalidDim == $("#txtActualDimDAfterStraightening4").val() && focusText == "") {
                            focusText = "txtActualDimDAfterStraightening4";
                        }

                        if (result.InvalidDim == $("#txtActualDimDAfterStraightening5").val() && focusText == "") {
                            focusText = "txtActualDimDAfterStraightening5";
                        }
                        if (result.InvalidDim == $("#txtActualDimDAfterStraightening6").val() && focusText == "") {
                            focusText = "txtActualDimDAfterStraightening6";
                        }

                        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_009").text().trim(); //ActDimOutOfTolerance
                        showMessageDialog(message, focusText, WARNING);
                        return false;
                    }
                }

                if ($("#chkActualLength").prop("checked") == true && $("#ddlDecision").val() == 10 && $("#txtLengthToleranceT1").val() != "") {
                    var lengthValues = {};
                    lengthValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;
                    lengthValues["LENGTH_TOLERANCE"] = $("#txtLengthToleranceT1").val() ? $("#txtLengthToleranceT1").val() : null;
                    lengthValues["LENGTH1"] = $("#txtActualLengthAfterStraightening1").val() ? $("#txtActualLengthAfterStraightening1").val() : null;
                    lengthValues["LENGTH2"] = $("#txtActualLengthAfterStraightening2").val() ? $("#txtActualLengthAfterStraightening2").val() : null;
                    lengthValues["LENGTH3"] = null;


                    var result = IsActualLengthValid(lengthValues);

                    if (result.IsSuccess == false) {
                        var focusText = "";
                        if (result.InvalidLength == $("#txtActualLengthAfterStraightening1").val()) {
                            focusText = "txtActualLengthAfterStraightening1";
                        }
                        if (result.InvalidLength == $("#txtActualLengthAfterStraightening2").val() && focusText == "") {
                            focusText = "txtActualLengthAfterStraightening2";
                        }
                        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_029").text().trim(); // LengthOutOfTolerance
                        showMessageDialog(message, focusText, WARNING);
                        return false;
                    }
                    
                }

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

                        if (!isStrictNumeric(value) || value.trim() == null || value.trim() == "" || $("#ddlUnitType").val() == 2) {

                            var message = $("#lblErrorMessageE_WeightCanNotValidate").text().trim();
                            showMessageDialog(message, "chkFinishedQuantity", WARNING);
                            return false;
                        }

                    } else
                    {
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

                    var message = $("#lblConfirmationMessageC_STRAIGHTENING_FIWeight_025").text().trim(); // FinishedQtyNotIn2PercentRange
                    showMessageDialog(message, "txtFinishedQuantity", WARNING);

                } else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false) {
                    var message = "";

                    message = $.trim($("#lblConfirmationMessageC_STR_EDIT_DELETE_002").text());  //WeightIsInvalid

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
                                UpdateStraighteningDetails($callFrom);
                               // getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
                            }
                            else {
                                $("#txtFinishedQuantity").focus();
                            }

                        },
                        centerVertical: true
                    });

                } else {
                    UpdateStraighteningDetails($callFrom);

                }

            });

            function isStrictNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
                return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
            } 
            $("#btnInitialize").click(function (event) {
                debugger;
                initialize();
            });

            $("#btnStartDateTime").click(function (event) {
                setStartSysDateTime();
            });
            $("#btnEndDateTime").click(function (event) {
                setEndSysDateTime();
            });

            //$("#btnYobinaStartOpeartorSearch").click(function (event) {
            //    getOperatorList($("#txtYobinaStartOperatorSearch").val(), $("#ddlStartOperatorNameValue"));
            //});
            //$("#btnYobinaEndOpeartorSearch").click(function (event) {

            //    getOperatorList($("#txtYobinaEndOperatorSearch").val(), $("#ddlEndOperatorNameValue"));
            //});
            //$("#btnYobinaStartOpeartorSearch").click(function (event) {
            //    var yobina = $.trim($("#txtYobinaStartOperatorSearch").val());
            //    setStartOperatorList(yobina, "");
            //});
            //$("#btnYobinaEndOpeartorSearch").click(function (event) {
            //    var yobina = $.trim($("#txtYobinaEndOperatorSearch").val());
            //    setEndOperatorList(yobina, "");
            //});
            //$("#ddlDecision").change(function (event) {   //Commented on 17/8/2017 by Sagar B.When Next Process will be implemented then uncomment this event
            //    decisionVal = $(this).val();
            //    $ddlNextProcess = $("#ddlNextProcess");
            //    if (decisionVal == 1 || decisionVal == 4) {
            //        $ddlNextProcess.removeAttr("disabled");
            //        $ddlNextProcess.addClass("Mandatory MandatoryDropdown");
            //        $ddlNextProcess.removeClass("disabled");
            //    }
            //    else {
            //        $ddlNextProcess.removeClass("Mandatory MandatoryDropdown");
            //        $ddlNextProcess.addClass("disabled");
            //        $("#" + $ddlNextProcess.attr("id") + " option:selected").prop("selected", false);
            //        $("#" + $ddlNextProcess.attr("id") + " option:first").prop("selected", "selected");
            //        $ddlNextProcess.attr("disabled", "disabled");
            //    }
            //});

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

//Focus on ddlNextProcess to btnUpdate.
$("#ddlNextProcess").focusout(function (event) {
    $("#btnUpdate").focus();
});

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

function OperatorIsInactive(dropdownName) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
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

function UpdateStraighteningDetails($callFrom) {
    
    debugger;
    var ajaxTime = new Date().getDate();
  
    // END DATA
    var jsonDataEditDelete = JSON.stringify({
        STR_STRAIGHTENING_CD: $("#hfld_StraighteningCd").val() ? $("#hfld_StraighteningCd").val() : 0,
        STR_PROCESS_PLAN_CD: $("#hfld_ProcessPlanCD").val() ? $("#hfld_ProcessPlanCD").val() : 0,
        STR_MACHINE_CD: $("#ddlModel").val() ? $("#ddlModel").val() : null,//$("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : 0,
        STR_MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        //ERROR
        STR_START_OPERATOR: $("#ddlStartOperatorNameValue").find(":selected").val() ? $("#ddlStartOperatorNameValue").find(":selected").val() : null, //$("#ddlStartOperatorNameValue").val() ? $("#ddlStartOperatorNameValue").val() : 0, 

        STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        STR_LENGTH_BEFORE_1: $("#txtActualLengthBeforeStraightening1").val() ? $("#txtActualLengthBeforeStraightening1").val() : null,
        STR_LENGTH_BEFORE_2: $("#txtActualLengthBeforeStraightening2").val() ? $("#txtActualLengthBeforeStraightening2").val() : null, 
        STR_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
        STR_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
        STR_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
        STR_TONNAGE: $("#txtTonnage").val() ? $("#txtTonnage").val() : null,

        STR_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        STR_END_OPERATOR: $("#ddlEndOperatorNameValue").find(":selected").val() ? $("#ddlEndOperatorNameValue").find(":selected").val() : null,//$("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : 0,
        STR_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        STR_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        STR_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
        STR_LENGTH_AFTER_1: $("#txtActualLengthAfterStraightening1").val() ? $("#txtActualLengthAfterStraightening1").val() : null,
        STR_LENGTH_AFTER_2: $("#txtActualLengthAfterStraightening2").val() ? $("#txtActualLengthAfterStraightening2").val() : null,
        STR_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
        STR_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
        STR_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : null,
        STR_NG_DETAILS1: $("#ddlNGReason1").val() ? $("#ddlNGReason1").val() : null,
        STR_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : null,
        STR_NG_DETAILS2: $("#ddlNGReason2").val() ? $("#ddlNGReason2").val() : null,
        STR_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : null,
        STR_NG_DETAILS3: $("#ddlNGReason3").val() ? $("#ddlNGReason3").val() : null,
        STR_EXT_TEXTURE: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
        STR_RESERVATION_NUMBER: $("#txtReservationNumber").val() ? $("#txtReservationNumber").val() : null,
        STR_A_SIDE_FURE1: $("#txtAEdgeDistortion1").val() ? $("#txtAEdgeDistortion1").val() : null,
        STR_A_SIDE_FURE2: $("#txtAEdgeDistortion2").val() ? $("#txtAEdgeDistortion2").val() : null,
        STR_A_SIDE_FURE3: $("#txtAEdgeDistortion3").val() ? $("#txtAEdgeDistortion3").val() : null,
        STR_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
        STR_B_SIDE_FURE1: $("#txtBEdgeDistortion1").val() ? $("#txtBEdgeDistortion1").val() : null,
        STR_B_SIDE_FURE2: $("#txtBEdgeDistortion2").val() ? $("#txtBEdgeDistortion2").val() : null,
        STR_B_SIDE_FURE3: $("#txtBEdgeDistortion3").val() ? $("#txtBEdgeDistortion3").val() : null,
        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        STR_TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
        STR_UNIT_TYPE: $("#ddlUnitType").find(":selected").val() ? $("#ddlUnitType").find(":selected").val() : null,
        STR_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        STR_FINISHED_QTY_CHK: ($('#chkFinishedQuantity').is(':checked') ? 1 : 0),
        STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
        STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
        STR_DIM_BEFORE_3: $("#txtActualDimDBeforeStraightening3").val() ? $("#txtActualDimDBeforeStraightening3").val() : null,
        STR_DIM_BEFORE_4: $("#txtActualDimDBeforeStraightening4").val() ? $("#txtActualDimDBeforeStraightening4").val() : null,
        STR_DIM_AFTER_1: $("#txtActualDimDAfterStraightening1").val() ? $("#txtActualDimDAfterStraightening1").val() : null,
        STR_DIM_AFTER_2: $("#txtActualDimDAfterStraightening2").val() ? $("#txtActualDimDAfterStraightening2").val() : null,
        STR_DIM_AFTER_3: $("#txtActualDimDAfterStraightening3").val() ? $("#txtActualDimDAfterStraightening3").val() : null,
        STR_DIM_AFTER_4: $("#txtActualDimDAfterStraightening4").val() ? $("#txtActualDimDAfterStraightening4").val() : null,
        STR_DIM_AFTER_5: $("#txtActualDimDAfterStraightening5").val() ? $("#txtActualDimDAfterStraightening5").val() : null,
        STR_DIM_AFTER_6: $("#txtActualDimDAfterStraightening6").val() ? $("#txtActualDimDAfterStraightening6").val() : null,
        STR_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
        STR_RESTRAIGHTING: ($('#chkRestraightening').is(':checked') ? 1 : 0), 
        STR_REMARKS: $.trim($("#txtRemarks").val()) ? $.trim($("#txtRemarks").val()) : null,
        STR_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
        USER_NAME: userName ? userName : null
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/StraighteningDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            /* After EndData is saved sucessfully:
                1. Disable all panels
                2. Show success message
                3. Refresh Thaba details grid 
            */
            if (parseInt(response.message) > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }

            $("#dialogMessageIcon img").hide();
            $("#imgSuccess").show();

            var successMessage = $("#lblSuccessMessageS_STR_EDIT_DELETE_001").text();
            showMessageDialog(successMessage, "SUCCESS", SUCCESS);

            //var chuu = $("#txtChuubanT1").text();
            //var chuubanGyouban = chuu.split("-");
            //var ch = chuubanGyouban[0];
            //var chh = chuubanGyouban[1];
            //var chuuban = ch.concat(chh);
            //var gyouban = chuubanGyouban[2];

          //  getThabaDetailsByChuuban(chuuban, gyouban, $callFrom);
            //getMachineDetails($("#hdfMachineCD").val(), $callFrom);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//function UpdateStraighteningDetails() {
//    debugger;

//    if ($("#hfld_MessageCD").val() == 0) {
//        $("#hfld_MessageCD").val(null)
//    }

//    var ajaxTime = new Date().getTime();
//    var jsonDataEditDelete = JSON.stringify({
//        //STR_STRAIGHTENING_CD: $("#hfld_StraighteningCd").val() ? $("#hfld_StraighteningCd").val() : null,
//        //STR_PROCESS_PLAN_CD: $("#hdfProcessPlanCD").val() ? $("#hdfProcessPlanCD").val() : null,
//        //STR_MACHINE_CD: $("#ddlModel").val() ? $("#ddlModel").val() : null,
//        //STR_MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
//        //STR_IS_REMARKS_UPDATE: isStraightingRemarksUpdate,
//        //STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),

//        //STR_START_OPERATOR: $("#ddlStartOperatorNameValue").find(":selected").val() ? $("#ddlStartOperatorNameValue").find(":selected").val() : null,
//        //STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
//        //STR_LENGTH_BEFORE_1: $("#txtActualLengthBeforeStraightening1").val() ? $("#txtActualLengthBeforeStraightening1").val() : null,
//        //STR_LENGTH_BEFORE_2: $("#txtActualLengthBeforeStraightening2").val() ? $("#txtActualLengthBeforeStraightening2").val() : null,
//        //STR_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
//        //STR_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
//        //STR_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
//        //STR_TONNAGE: $("#txtTonnage").val() ? $("#txtTonnage").val() : null,
//        //STR_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),

//        //STR_END_OPERATOR: $("#ddlEndOperatorNameValue").find(":selected").val() ? $("#ddlEndOperatorNameValue").find(":selected").val() : null,
//        //STR_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
//        //STR_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
//        //STR_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
//        //STR_LENGTH_AFTER_1: $("#txtActualLengthAfterStraightening1").val() ? $("#txtActualLengthAfterStraightening1").val() : null,
//        //STR_LENGTH_AFTER_2: $("#txtActualLengthAfterStraightening2").val() ? $("#txtActualLengthAfterStraightening2").val() : null,
//        //STR_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
//        //STR_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
//        //STR_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : null,
//        //STR_NG_DETAILS1: $("#ddlNGReason1").val() ? $("#ddlNGReason1").val() : null,
//        //STR_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : null,
//        //STR_NG_DETAILS2: $("#ddlNGReason2").val() ? $("#ddlNGReason2").val() : null,
//        //STR_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : null,
//        //STR_NG_DETAILS3: $("#ddlNGReason3").val() ? $("#ddlNGReason3").val() : null,
//        //STR_EXT_TEXTURE: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
//        //STR_RESERVATION_NUMBER: $("#txtReservationNumber").val() ? $("#txtReservationNumber").val() : null,
//        //STR_A_SIDE_FURE1: $("#txtAEdgeDistortion1").val() ? $("#txtAEdgeDistortion1").val() : null,
//        //STR_A_SIDE_FURE2: $("#txtAEdgeDistortion2").val() ? $("#txtAEdgeDistortion2").val() : null,
//        //STR_A_SIDE_FURE3: $("#txtAEdgeDistortion3").val() ? $("#txtAEdgeDistortion3").val() : null,
//        //STR_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
//        //STR_B_SIDE_FURE1: $("#txtBEdgeDistortion1").val() ? $("#txtBEdgeDistortion1").val() : null,
//        //STR_B_SIDE_FURE2: $("#txtBEdgeDistortion2").val() ? $("#txtBEdgeDistortion2").val() : null,
//        //STR_B_SIDE_FURE3: $("#txtBEdgeDistortion3").val() ? $("#txtBEdgeDistortion3").val() : null,
//        //STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
//        //STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
//        //STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
//        //STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
//        //STR_TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
//        //STR_UNIT_TYPE: $("#ddlUnitType").find(":selected").val() ? $("#ddlUnitType").find(":selected").val() : null,
//        //STR_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
//        //STR_FINISHED_QTY_CHK: ($('#chkFinishedQuantity').is(':checked') ? 1 : 0),
//        //STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
//        //STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
//        //STR_DIM_BEFORE_3: $("#txtActualDimDBeforeStraightening3").val() ? $("#txtActualDimDBeforeStraightening3").val() : null,
//        //STR_DIM_BEFORE_4: $("#txtActualDimDBeforeStraightening4").val() ? $("#txtActualDimDBeforeStraightening4").val() : null,
//        //STR_DIM_AFTER_1: $("#txtActualDimDAfterStraightening1").val() ? $("#txtActualDimDAfterStraightening1").val() : null,
//        //STR_DIM_AFTER_2: $("#txtActualDimDAfterStraightening2").val() ? $("#txtActualDimDAfterStraightening2").val() : null,
//        //STR_DIM_AFTER_3: $("#txtActualDimDAfterStraightening3").val() ? $("#txtActualDimDAfterStraightening3").val() : null,
//        //STR_DIM_AFTER_4: $("#txtActualDimDAfterStraightening4").val() ? $("#txtActualDimDAfterStraightening4").val() : null,
//        //STR_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
//        //STR_RESTRAIGHTING: ($('#chkRestraightening').is(':checked') ? 1 : 0),
//        //STR_REMARKS: $.trim($("#txtRemarks").val()) ? $.trim($("#txtRemarks").val()) : null,
//        //STR_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
//        //USER_NAME: userName ? userName : null

//        STR_STRAIGHTENING_CD: $("#hfld_StraighteningCd").val() ? $("#hfld_StraighteningCd").val() : null,
//        STR_PROCESS_PLAN_CD: $("#hdfProcessPlanCD").val() ? $("#hdfProcessPlanCD").val() : null,
//        STR_MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
//        STR_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
//        STR_START_OPERATOR: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
//        STR_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
//        STR_LENGTH_BEFORE_1: $("#txtActualLength1BeforeStraightening").val() ? $("#txtActualLength1BeforeStraightening").val() : null,
//        STR_LENGTH_BEFORE_2: $("#txtActualLength2BeforeStraightening").val() ? $("#txtActualLength2BeforeStraightening").val() : null,
//        STR_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
//        STR_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
//        STR_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
//        STR_TONNAGE: $("#txtSTRTonnage").val() ? $("#txtSTRTonnage").val() : null,
//        STR_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
//        STR_END_OPERATOR: $("#hdfEndOperatorCD").val() ? $("#hdfEndOperatorCD").val() : null,
//        STR_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
//        STR_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
//        STR_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
//        STR_LENGTH_AFTER_1: $("#txtActualLength1AfterStraightening").val() ? $("#txtActualLength1AfterStraightening").val() : null,
//        STR_LENGTH_AFTER_2: $("#txtActualLength2AfterStraightening").val() ? $("#txtActualLength2AfterStraightening").val() : null,
//        STR_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
//        STR_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
//        STR_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : 0,
//        STR_NG_DETAILS1: $("#ddlNGReason1").val() ? $("#ddlNGReason1").val() : 0,
//        STR_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : 0,
//        STR_NG_DETAILS2: $("#ddlNGReason2").val() ? $("#ddlNGReason2").val() : 0,
//        STR_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : 0,
//        STR_NG_DETAILS3: $("#ddlNGReason3").val() ? $("#ddlNGReason3").val() : 0,
//        STR_EXT_TEXTURE: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
//        STR_RESERVATION_NUMBER: $("#txtReservationNum").val() ? $("#txtReservationNum").val() : null,
//        STR_A_SIDE_FURE1: $("#txtAEdgeDistortion1").val() ? $("#txtAEdgeDistortion1").val() : null,
//        STR_A_SIDE_FURE2: $("#txtAEdgeDistortion2").val() ? $("#txtAEdgeDistortion2").val() : null,
//        STR_A_SIDE_FURE3: $("#txtAEdgeDistortion3").val() ? $("#txtAEdgeDistortion3").val() : null,
//        STR_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
//        STR_B_SIDE_FURE1: $("#txtBEdgeDistortion1").val() ? $("#txtBEdgeDistortion1").val() : null,
//        STR_B_SIDE_FURE2: $("#txtBEdgeDistortion2").val() ? $("#txtBEdgeDistortion2").val() : null,
//        STR_B_SIDE_FURE3: $("#txtBEdgeDistortion3").val() ? $("#txtBEdgeDistortion3").val() : null,
//        STR_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
//        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
//        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
//        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
//        STR_TP_TEST: $("#ddlTPTest").find(":selected").val() ? $("#ddlTPTest").find(":selected").val() : null,
//        STR_UNIT_TYPE: $("#ddlUnitType").find(":selected").val() ? $("#ddlUnitType").find(":selected").val() : null,
//        STR_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
//        STR_FINISHED_QTY_CHK: ($('#chkFinishedQuantity').is(':checked') ? 1 : 0),
//        STR_DIM_BEFORE_1: $("#txtActualDimDBeforeStraightening1").val() ? $("#txtActualDimDBeforeStraightening1").val() : null,
//        STR_DIM_BEFORE_2: $("#txtActualDimDBeforeStraightening2").val() ? $("#txtActualDimDBeforeStraightening2").val() : null,
//        STR_DIM_BEFORE_3: $("#txtActualDimDBeforeStraightening3").val() ? $("#txtActualDimDBeforeStraightening3").val() : null,
//        STR_DIM_BEFORE_4: $("#txtActualDimDBeforeStraightening4").val() ? $("#txtActualDimDBeforeStraightening4").val() : null,
//        STR_DIM_AFTER_1: $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null,
//        STR_DIM_AFTER_2: $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null,
//        STR_DIM_AFTER_3: $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null,
//        STR_DIM_AFTER_4: $("#txtActualDim4").val() ? $("#txtActualDim4").val() : null,
//        STR_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
//        STR_RESTRAIGHTING: ($('#chkRestraighting').is(':checked') ? 1 : 0),
//        STR_REMARKS: $.trim($("#txtRemarks").val()) ? $.trim($("#txtRemarks").val()) : null,
//        STR_USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
//        USER_NAME: userName ? userName : null
//    });

//    var settings = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/Straightening/StraighteningDetailsUpdate",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        data: jsonDataEditDelete
//       //  data: JSON.stringify(jsonDataEditDelete)
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            debugger;
//            responseTime = new Date().getTime() - ajaxTime;
//            disableEditDeleteButtons();
//            var jsonResult = response.message;
//            var successMessage = $.trim($("#lblSuccessMessageS_STR_EDIT_DELETE_001").text());
//            showMessageDialog(successMessage, "", SUCCESS);

//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}

$("#btnDelete").click(function (event) {
    DeleteStraighteningDetails();
    initialize();
   // switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    //document.location.reload();
});

function DeleteStraighteningDetails() {
    debugger;
    var ajaxTime = new Date().getTime();
   
    var jsonDataDelete = JSON.stringify({

       
        STR_STRAIGHTENING_CD: $("#hfld_StraighteningCd").val() ? $("#hfld_StraighteningCd").val() : 0,
        STR_PROCESS_PLAN_CD: $("#hfld_ProcessPlanCD").val() ? $("#hfld_ProcessPlanCD").val() : 0,
        STR_MSG_CD: parseInt($("#hfld_MessageCD").val()),
        STR_TABA_NO_1: $("#txtThaba1").val(),
        STR_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        STR_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        STR_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/StraighteningDetailsDelete",
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
            // resetGlobalVariables();
            disableEditDeleteButtons();

            var successMessage = $("#lblSuccessMessageS_STR_EDIT_DELETE_002").text();
            showMessageDialog(successMessage, "", SUCCESS);

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
        var isRequired = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_001").text());

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
    var ddlmodel = $("#ddlModel").val();

    if ($("#ddlStartOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlStartOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_017").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlEndOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_021").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }

    else if ($("#ddlTPTest").val() == "0") {
        var focusFieldID = "ddlTPTest";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_019").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlDecision").val() == "0") {
        var focusFieldID = "ddlDecision";
        var message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_020").text());
        showMessageDialog(messsage, focusFieldID, WARNING);
        returnVal = false;
    }
    else if (ddlmodel == null) {
        var focusFieldID = "ddlModel";
        var message = $.trim($("#lblErrorMessage_Straightening_MachineRequired").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    } 
   
    return returnVal;
}

//function IsDuplicateThabaNo(chuuban, gyouban, $callFrom) {
//    debugger;
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
//        url: APIURL + "/Straightening/GetThabaDetailsByThabaNumber",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonThabaDetailData + "}"
//        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban, Thaba_No_String: Thaba_No_String })
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            debugger;
//            responseTime = new Date().getTime() - ajaxTime;
//            var jsonThabaDetailData = [];
//            jsonThabaDetailData = response.message;

//            $.each(jsonThabaDetailData, function (index, jsonObject) {

//                if (jsonObject.stR_TABA_NO) {

//                    var thabaNumbers = jsonObject.stR_TABA_NO.split(",");
//                    var cd = jsonObject.stR_STRAIGHTENING_CD;

//                    $.each(thabaNumbers, function (index, thabaNo) {
//                        thabaNo = thabaNo.trim();
//                        registeredThabaNumbersAndDecision["" + thabaNo] = cd;
//                    });
//                }
//            });
//            if ((($.inArray(Thaba1, [Thaba2, Thaba3, Thaba4]) > -1) && Thaba1 != "") ||
//                (($.inArray(Thaba2, [Thaba1, Thaba3, Thaba4]) > -1) && Thaba2 != "") ||
//                (($.inArray(Thaba3, [Thaba1, Thaba2, Thaba4]) > -1) && Thaba3 != "") ||
//                (($.inArray(Thaba4, [Thaba1, Thaba2, Thaba3]) > -1) && Thaba4 != "")) {
//                firstFieldID = Thaba1;
//                message = $("#lblErrorMessageE_STR_EDIT_DELETE_006").text().trim();
//                showMessageDialog(message, firstFieldID, WARNING);
//                returnval = false;
//                return false;
//            }

//            if (Thaba1 in registeredThabaNumbersAndDecision) {
//                if (parseInt(registeredThabaNumbersAndDecision["" + Thaba1]) != stratghteningCD && $("#chkRestraightening").prop("checked") == false) {
//                    t1 = Thaba1;
//                } else {
//                    t1 = "";
//                }
//            }
//            if (Thaba2 in registeredThabaNumbersAndDecision) {
//                if (parseInt(registeredThabaNumbersAndDecision["" + Thaba2]) != stratghteningCD && $("#chkRestraightening").prop("checked") == false) {
//                    t2 = Thaba2;
//                } else {
//                    t2 = "";
//                }
//            }
//            if (Thaba3 in registeredThabaNumbersAndDecision) {
//                if (parseInt(registeredThabaNumbersAndDecision["" + Thaba3]) != stratghteningCD && $("#chkRestraightening").prop("checked") == false) {
//                    t3 = Thaba3;
//                } else {
//                    t3 = "";
//                }
//            }
//            if (Thaba4 in registeredThabaNumbersAndDecision) {
//                if (parseInt(registeredThabaNumbersAndDecision["" + Thaba4]) != stratghteningCD && $("#chkRestraightening").prop("checked") == false) {
//                    t4 = Thaba4;
//                } else {
//                    t4 = "";
//                }
//            }

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
//                debugger;
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
    debugger;
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
    var strCD = $("#hfld_StraighteningCd").val();
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
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_006").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_006").text().trim().replace("NNNN", duplicateThaba[0]);;
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


    var ajaxTime = new Date().getDate();
    var jsonData = JSON.stringify({
        PROCESS_PLAN_CD: processPlanCD,
        TABA_NO_STRING: Thaba_No_String,
        STR_STRAIGHTENING_CD: strCD,
        ISCHECKED: $('#chkRestraightening').is(':checked') ? 1 : 0,
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
        url: APIURL + "/Straightening/GetThabaDetailsByThabaNumber", 
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
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

//function isActualDimentionValid() {

//    var returnVal = false;
//    var ActualDim1 = $("#txtActualDim1").val();
//    var ActualDim2 = $("#txtActualDim2").val();
//    var ActualDim3 = $("#txtActualDim3").val();
//    var ActualDim4 = $("#txtActualDim4").val();

//    var DimTol = $("#txtDimToleranceT1").text();
//    var arrayDimTol = DimTol.split(",");
//    var ProdDimT1 = $("#txtProdDimT1").text();

//    var maxVal = parseFloat(ProdDimT1) + parseFloat(arrayDimTol[0].replace("+", ""));
//    var minVal = parseFloat(ProdDimT1) - parseFloat(arrayDimTol[1].replace("-", ""));

//    if ((ActualDim1 <= maxVal && ActualDim1 >= minVal && $("#chkActualDim").prop("checked") == true) && (ActualDim2 <= maxVal && ActualDim2 >= minVal && $("#chkActualDim").prop("checked") == true) && (ActualDim3 <= maxVal && ActualDim3 >= minVal && $("#chkActualDim").prop("checked") == true) && (ActualDim4 <= maxVal && ActualDim4 >= minVal && $("#chkActualDim").prop("checked") == true)) {
//        returnVal = true;
//    }
//    else {
//        if ($("#chkActualDim").prop("checked") == false) {
//            returnVal = true;
//        }
//        else {
//            returnVal = false;
//        }
//    }
//    return returnVal;
//}

//function isActualLengthValid() {

//    var returnVal = false;
//    var actualLengthQTY1 = $("#txtActualLength1AfterStraightening").val();
//    var actualLengthQTY2 = $("#txtActualLength2AfterStraightening").val();
//    var lenTol = $("#txtLengthToleranceT1").text();
//    var arrayLenTol = lenTol.split(",");
//    var length = $("#txtLengthT1").text();
//    var maxquantity = parseFloat(length) + parseFloat(arrayLenTol[0].replace("+", ""));
//    var minquantity = parseFloat(length) - parseFloat(arrayLenTol[1].replace("-", ""));

//    if ((actualLengthQTY1 >= minquantity && actualLengthQTY1 <= maxquantity && $("#chkActualLength").prop("checked") == true) && actualLengthQTY2 >= minquantity && actualLengthQTY2 <= maxquantity && $("#chkActualLength").prop("checked") == true) {
//        returnVal = true;
//    } else {
//        if ($("#chkActualLength").prop("checked") == false) {
//            returnVal = true;

//        } else {
//            returnVal = false;
//        }

//    }
//    return returnVal;
//}

$('#chkActualDim').click(function () {
    if ($("#chkActualDim").prop("checked") == true) {
        DimentionValue = false;
        $("#txtActualDimDAfterStraightening1").addClass("Mandatory");
    } else {
        DimentionValue = true;
        $("#txtActualDimDAfterStraightening1").removeClass("Mandatory");
    }
});

$('#chkActualLength').click(function () {
    
    if ($("#chkActualLength").prop("checked") == true) {
       // lengthValue = false;
        $("#txtActualLengthAfterStraightening1").addClass("Mandatory");
        $("#txtActualLengthAfterStraightening2").addClass("Mandatory");
    } else {
      //  lengthValue = true;
        $("#txtActualLengthAfterStraightening1").removeClass("Mandatory");
        $("#txtActualLengthAfterStraightening2").removeClass("Mandatory");

    }
});

$('#chkFinishedQuantity').click(function () {

    if ($("#chkFinishedQuantity").prop("checked") == true) {
        finishedWeightValue = false;
    } else {
        finishedWeightValue = true;
    }
});



function IsEndDataValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_007").text().trim();
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_STR_EDIT_DELETE_008").text().trim();
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

//function IsActiveMachine(machineCd) {

//    if (IsExists == false && $("#ddlModel").val() == machineCd) {
//        var focusFieldID = "ddlModel";
//        var message = $("#HdfModelInactiveMessage").val();     //$.trim($("#lblErrorMessageE_STR_EDIT_DELETE_013").text());
//        showMessageDialog(message, focusFieldID, WARNING);
//        return false;
//    }
//    else {
//        return true;
//    }
//}

//function IsActiveStartOpr(startOprCD) {
//    debugger;
//    if (IsStartOprExists == false && $("#ddlStartOperatorNameValue").val() == startOprCD) {
//        var focusFieldID = "ddlStartOperatorNameValue";
//        var message = $("#HdfOperatorInactiveMessage").val();
//        showMessageDialog(message, focusFieldID, WARNING);
//        return false;
//    }
//    else {
//        return true;
//    }
//}

//function IsActiveEndOpr(endOprCD) {
//    debugger;
//    if (IsEndOprExists == false && $("#ddlEndOperatorNameValue").val() == endOprCD) {
//        var focusFieldID = "ddlEndOperatorNameValue";
//        var message = $("#hdfOperatorInactiveMessage").val();
//        showMessageDialog(message, focusFieldID, WARNING);
//        return false;
//    }
//    else {
//        return true;
//    }
//}


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


//function setStartOperatorList(yobina, defaultSelectID) {
//    getOperatorList(yobina, $("#ddlStartOperatorNameValue"), defaultSelectID);
//};

//function setEndOperatorList(yobina, defaultSelectID) {
//    getOperatorList(yobina, $("#ddlEndOperatorNameValue"), defaultSelectID);
//};

//function setOperatorList(yobina, $target, defaultSelectID) {
//    var ajaxTime = new Date().getTime();
//    var jsonData = "{yobina:'" + yobina + "', USER_NAME: '" + userName + "' }";
//    var settings = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/Straightening/EmployeeList?name=" + yobina,
//        method: "GET",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonData + "}"
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            // var jsonResult = $.parseJSON(response.d);
//            bindOperatorDropdown(response.message, $target, defaultSelectID);
//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}

function getOperatorList(yobina, $target) {

    var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/EmployeeList?name=" + yobina + "&operatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
            bindOperatorDropdown(response.message, $target, yobina);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STR_EDIT_DELETE_012").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}

function bindOperatorDropdown(jsonResult, $target, defaultSelectID) {
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
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
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

    $("#txtActualDimDBeforeStraightening1, #txtActualDimDBeforeStraightening2, #txtActualDimDBeforeStraightening3, #txtActualDimDBeforeStraightening4, #txtActualDimDAfterStraightening1, #txtActualDimDAfterStraightening2, #txtActualDimDAfterStraightening3, #txtActualDimDAfterStraightening4, #txtActualDimDAfterStraightening5, #txtActualDimDAfterStraightening6").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });

    $("#txtActualLengthBeforeStraightening1, #txtActualLengthBeforeStraightening2, #txtActualLengthAfterStraightening1, #txtActualLengthAfterStraightening2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
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


//***************************************************
//****************************************************

function btnSearch_Click(allowNull = false) {
    debugger;
    var CHUUBAN_GYOUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $.trim($("#txtDimDMin").val()) ? $.trim($("#txtDimDMin").val()) : 0; //$("#txtDimDMin").val();
    var PROD_DIM_D_MAX = $.trim($("#txtDimDMax").val()) ? $.trim($("#txtDimDMax").val()) : 0;//$("#txtDimDMax").val();
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $.trim($("#txtLengthSearchMin").val()) ? $.trim($("#txtLengthSearchMin").val()) : 0;//$("#txtLengthSearchMin").val();
    var PROD_LENGTH_MAX = $.trim($("#txtLengthSearchMax").val()) ? $.trim($("#txtLengthSearchMax").val()) : 0;//$("#txtLengthSearchMax").val();
    //var ddlNameValue = $("#ddlNameValue").val();
    var USER_ID = $("#hdfLoginId").val();
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

    if (allowNull || CHUUBAN_GYOUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PROD_DIM_D_MIN.length > 0 || PROD_DIM_D_MAX.length > 0 || PROD_SHAPE.length > 0 || PROD_LENGTH_MIN.length > 0 || PROD_LENGTH_MAX.length > 0) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN_GYOUBAN: CHUUBAN_GYOUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX, PROD_SHAPE: PROD_SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX,
                USER_ID: USER_ID
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    var settings1 = {
                        async: false,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/Straightening/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;
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

        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_Straightening_SearchField").text()) + "</td></tr></table>",

            centerVertical: true
        });
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
    StraighteningSearch(selectedColumns);
}

function StraighteningGridSearch(sortFields) {
    var txtChuubanSearch = $("#txtChuubanSearch").val();
    //var txtTabaNo = $("#txtTabaNo").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val();
    var txtDimDMax = $("#txtDimDMax").val();
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val();
    var txtLengthSearchMax = $("#txtLengthSearchMax").val();
    //var txtOperationDateStart = $("#txtOperationDateStart").val();
    //var txtOperationDateEnd = $("#txtOperationDateEnd").val();
    var ddlNameValue = $("#ddlNameValue").val();
    if (txtChuubanSearch.length > 0 ||
        //txtTabaNo.length > 0 || 
        txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0
        //|| txtOperationDateStart.length > 0 || txtOperationDateEnd.length > 0 
        //|| (ddlNameValue.length > 0 && parseInt(ddlNameValue) > 0)
    ) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Straightening/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                txtChuubanSearch: txtChuubanSearch,
                //txtTabaNo: txtTabaNo, 
                txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax,
                //txtOperationDateStart: txtOperationDateStart, txtOperationDateEnd: txtOperationDateEnd, 
                //scOperator: ddlNameValue.length > 0 ? parseInt(ddlNameValue) : 0,
                sortFields: sortFields
            })
            //data: { request: JSON.stringify(objRequest), sortFields: JSON.stringify(sortFields) }
            //data: "{jsonData : " + jsonData + "}"
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
                        url: "/Straightening/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: response.message
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
                            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
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
                message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {
        bootbox.alert({
            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
            message: "This is test message",
            centerVertical: true
        });
    }
}

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {
    debugger;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/Straightening/InitializeSortPopup",
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

    window.resizeTo(1080, 770);
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
        url: APIURL + "/Straightening/GetMachineList",
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
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '"> ' + jsonObj.name + '</option>'));
    });


    ////  $("#ddlNextProcess").html("").append($('<option value="0"></option>'));

    //$.each(jsonResult, function (key, jsonObj) {
    //    $("#ddlModel").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    //});
}

function setTPTest() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:"/Straightening/GetTPTest",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindTPTestDropdown(jsonResult) {

    $("#ddlTPTest").html("");//.append($('<option value="0"></option>'));
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
        url: APIURL + "/Straightening/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
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

function setUnitType() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetUnitType",
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
            var errorCode = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitTypeDropdown(jsonResult) {
  //  $("#ddlUnitType").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setDecision() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Straightening/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            binddecisionDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function binddecisionDropdown(jsonResult) {
    $("#ddlDecision").html("").append($('<option value="0"></option>'));
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
        url: APIURL + "/Straightening/GetProcessDefect",
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
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
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

function setExternalSurfaceCondition() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:"/Straightening/GetExternalSurfaceCondition",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindExternalSurfaceConditionDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindExternalSurfaceConditionDropdown(jsonResult) {
    $("#ddlExternalSurfaceCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalSurfaceCondition").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setBendStatus() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME:'" + userName + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:"/Straightening/GetBendStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindddlBendStatusDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindddlBendStatusDropdown(jsonResult) {
    $("#ddlBendStatus").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function SetDropDownLists() {
    setNextProcess();
    setTPTest();
    setUnitType();
    setExternalSurfaceCondition();
    setBendStatus();
    setDecision();
    setProcessDefectList();
   // getOperatorList();
}

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
        debugger;
        switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
        initialize();
        window.resizeTo(1090, 890);
        location.reload();
        $("#txtChuubanSearch").focus();
    });

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
            url: APIURL + "/Straightening/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    //$('#txtChuuban2').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    //$('#txtGyouban2').val(jsonResult.gyoubaN_IN_CHUUBAN);
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

$(document).ready(function () {
    debugger;
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    // CheckNumberValidation();
    SetGlobalVariables();
    // SetMaxLengthAndFormat();
  //  window.resizeTo(1130, 980);
  //  setStartOperatorList("", "");
   // setEndOperatorList("", "");
    SetDropDownLists();
    bindEvents();
    setMachineList();
    /*$("#btnLnkEdit").click(function (event) {
        resetResponseTime();
        var $callFrom = $(this);
        alert("ok");
        var straighteningCD = $(event.target).find("#lblStraighteningCD").val();
       
        getStraighteningDetailsByStraighteningCD(straighteningCD, $callFrom);
    });*/

    disableEditDeleteButtons();
    
    userName = $("#hdfUserName").val();
    Mask();
    

    $("#ddlNextProcess").on('click', function () {
        focusText($("#btnUpdate").val());
    });
  
});