var registeredThabaNumbersAndCD = {};
var registeredThabaNumbersAndDecision = {};
var registeredCD = {};
var $btnLnkEdit;
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
var oldHikinukiRemarks;
var isDrawingRemarksUpdate = 0;
var IsExists = false;
var DISABLED = "disabled";
var arrayData = [];
var activeMachineCD = "";
var endOperatorCD = "";

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
    isDrawingRemarksUpdate = 0;
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

function toggleActualLengthFieldsMandatory($this) {

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

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
            // $("#txtEndTime").val($.format.date(new Date(sysDateTime), "HH:mm")).mask("99:99", { placeholder: "hh:mm" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function getColdDrawingWireDrawingDetailsByDrawingCD($callFrom) {

    //GetWindingShape();
    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var drawingCD = $.trim($callFrom.closest("tr").attr("data-lblDrawingCD"));
    var jsonData = "{DrawingCD :'" + drawingCD + "', USER_NAME: '" + userName + "' }";
    var actualDim = true;

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/getColdDrawingWireDrawingDetailsByDrawingCD?drawingCD=" + parseInt(drawingCD) + "&cultureName=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            
            // setProcessDefectList();

            //setStamp();
            enableEditDeleteButtons();
            enableStartDataPanel();
            enableEndDataPanel();
            enableModelDataPanel();
            setColdDrawingWireDrawingDetails(jsonResult);
           
            // setStartOperatorList("", jsonResult.dW_START_OPERATOR_CD);
            //setEndOperatorList("", jsonResult.dW_END_OPERATOR_CD);

            //setActiveTab("TbPMSEditDelTab2");
            //SetActDimD456();
            $("#btnUpdate").unbind("click")
            $("#btnUpdate").click(function (event) {

                if ($("#chkActualDim").prop("checked") == true) {

                    if ($("#txtActual_Start").val() != "" && $("#txtActual_End").val() != "") {
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




                if (!IsEndDateValid()) {
                    return false;
                }
                if (!checkDate($("#txtStartDate"))) {

                    var message = $("#lblErrorMessageE_Cold_Drawing_StartDateInvalid_010").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtStartDate", WARNING);
                    $("#txtStartDate").focus;
                    return false;
                }

                if (!checkDate($("#txtEndDate"))) {
                    var message = $("#lblErrorMessageE_Cold_Drawing_EndDateInvalid_011").text().trim(); // This message will be displayed on dialog box
                    showMessageDialog(message, "txtEndDate", WARNING);
                    $("#txtEndDate").focus;
                    return false;
                }

                if (jsonResult.pP_GYOUBAN.toString().length == 1) {
                    jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
                }
               
                
                if ($("#ddlModel option:selected").attr("flag") == 1) {

                    MachineIsInactive();
                    return false;
                }

                if ($("#ddlStartOprName option:selected").attr("flag") == 1) {

                    OperatorIsInactive("ddlStartOprName");
                    return false;
                }

                if ($("#ddlEndOprName option:selected").attr("flag") == 1) {

                    OperatorIsInactive("ddlEndOprName");
                    return false;
                }

                if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK) {
                    var dimValues = {};
                    dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
                    dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
                    dimValues["DIM1"] = $("#txtActual_Start").val() ? $("#txtActual_Start").val() : null;
                    dimValues["DIM2"] = $("#txtActual_End").val() ? $("#txtActual_End").val() : null;
                    dimValues["DIM3"] = null;
                    dimValues["DIM4"] = null;
                    dimValues["DIM5"] = null;
                    dimValues["DIM6"] = null;

                    var result = IsActualDimentionValid(dimValues);
                    if (result.IsSuccess == false) {
                        var focusText = "";
                        if (result.InvalidDim == $("#txtActual_Start").val()) {
                            focusText = "txtActual_Start";
                        }
                        if (result.InvalidDim == $("#txtActual_End").val() && focusText == "") {
                            focusText = "txtActual_End";
                        }

                        var message = $("#lblConfirmationMessageC_DRAWING_W_EditDelete_002").text().trim(); //ActDimOutOfTolerance
                        showMessageDialog(message, focusText, WARNING);
                        return false;
                    }
                }
                

                if (!IsDuplicateThabaNo(jsonResult.pP_CHUUBAN, jsonResult.pP_GYOUBAN, $callFrom)) {

                    return false;
                }

                updateColdDrawingWireDrawingDetails();
                $("#txtStartDate").focus();
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
        url: "/ColdDrawingWIREDRAWING/MachineIsInactive?machineName=" + $("#ddlModel option:selected").text(),
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
            return false;
        });
}
$("#btnInitialize").click(function (event) {
    debugger;
    $("#ddlModel").html("").append($('<option value="0"></option>'));
    initialize();
    disableEditDeleteButtons();
    switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    $("#txtChuubanSearch").focus();
});
$("#btnDelete").click(function (event) {
    deleteColdDrawingWireDrawingDetails();
   

});

//$("#TbPMSEditDelTab1-tab").click(function (event) {
//    btnSearch_Click();

//});

$("#btnSearch").click(function (event) {
    btnSearch_Click();

});

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
    var drawingCD = $("#hfld_DrawingCd").val();
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
            message = $("#lblErrorMessageE_Multiple_SameThaba").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_Multiple_SameThaba").text().trim().replace("NNNN", duplicateThaba[0]);;
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


    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();

    var jsonData = JSON.stringify({
        CHUUBAN: chuuban,
        GYOUBAN: gyouban,
        TABA_NO_STRING: Thaba_No_String,
        DRAWING_CD: drawingCD,
        ISCHECKED: $('#chkReDrawing').is(':checked') ? 1 : 0,
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
        url: APIURL + "/ColdDrawingWIREDRAWING/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
       
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            jsonThabaDetailData = response.message;

            var jsonResult = response.message;
            var sucess = response.isSuccess
            if (sucess == false) {
                firstFieldID = "txtThaba1";
                message = $("#lblErrorMessageE_Duplicate_thaba_WithOk").text().trim().replace("NNN", jsonResult.replace(/(.+), $/, '$1'));    //lblErrorMessageE_STRAIGHTENING_ENTRY_005
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
            message = $("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

    return returnval;
}




function setColdDrawingWireDrawingDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#txtStartOprSearch").val("");
    $("#txtEndOprSearch").val("");

    setNextProcess();
    setDieDiaShape();
    setwindingShape();
    setDiceInspStart();
    setDiceInspEnd();
    setExteriorConditionKubunGet();
    setMagneticDust();
    setDecision();

    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));

    $("#hdfMachineCD").val(jsonResult.machinE_CD);
    $("#hdfProcessPlanCD").val(jsonResult.procesS_PLAN_CD);
    setMachineList();
  
    $("#ddlModel").val(jsonResult.machinE_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.sT_CREATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.sT_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    
    $("#hfld_MessageCD").val(jsonResult.dW_MSG_CD);
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtConditionT1").val(jsonResult.conditioN_DESCRIPTION); 
   
    $("#txt_HostName").val(jsonResult.producT_NYUKO_CD);
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
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D.toFixed(2));//$("#txtProdDimT1").val(jsonResult.PP_PROD_DIM_D_STRING);
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
    
    //Start Data
    $("#hfld_DrawingCd").val(jsonResult.drawinG_CD);

    $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    SetOperatorDropdown($("#ddlStartOprName"), jsonResult.dW_START_OPERATOR_CD)
   

    $("#txtRollingnumber").val(jsonResult.dW_ROLLING_NUMBER);
    $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);
    $("#txtDieNo").val(jsonResult.dW_DIE_NO);
    $("#txtDieDim").val(jsonResult.dW_DIE_DIM);

    $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
    //$("#ddlWindingshape").val(jsonResult.dW_WINDING_SHAPE);
    $("#ddlWindingshape").val(jsonResult.dW_WINDING_SHAPE || $("#ddlWindingshape").prop('selectedIndex'));
    $("#ddlDiceInspectionStart").val(jsonResult.dW_DIE_CHK_START);

    if (jsonResult.dW_IM_DRAWING == 0) {
        $("#chkmiddle").prop('checked', false);
        enableRedrawCheckbox();
    } else {

        $("#chkmiddle").prop('checked', true);
        disableRedrawCheckbox();
    }

    if (jsonResult.dW_REDRAWING == 0) {
        $("#chkReDrawing").prop('checked', false);
    } else {

        $("#chkReDrawing").prop('checked', true);
    }

    //End Data


    $("#txtEndDate").val($.format.date(new Date(jsonResult.dW_END_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    SetOperatorDropdown($("#ddlEndOprName"), jsonResult.dW_END_OPERATOR_CD)
    
    $("#ddlDecision").val(jsonResult.dW_DECISION || $("#ddlDecision").prop('selectedIndex'));
    $("#ddlNextProcess").val(jsonResult.dW_DW_NEXT_PROCESS);

    if (jsonResult.dW_ACT_DIM_START != 0) {
        $("#txtActual_Start").val(jsonResult.dW_ACT_DIM_START);
    }

    if (jsonResult.dW_ACT_DIM_END != 0) {
        $("#txtActual_End").val(jsonResult.dW_ACT_DIM_END);
    }

    $("#txtNoOfUnits").val(jsonResult.dW_FINISHED_COILS);
    $("#txtWeight").val(jsonResult.dW_FINISHED_QTY);
    $("#ddlExternalSurfaceCondition").val(jsonResult.dW_EXT_TEXTURE_STATUS || $("#ddlExternalSurfaceCondition").prop('selectedIndex'));

    $("#ddlDiceInspectionEnd").val(jsonResult.dW_DIE_CHK_END || $("#ddlDiceInspectionEnd").prop('selectedIndex'));
    $("#ddldustdetection").val(jsonResult.dW_MAGNETIC_INSPECTIONS || $("#ddldustdetection").prop('selectedIndex'));
    $("#txtThaba1").val(jsonResult.dW_DW_TABA_NO_1);

    if (jsonResult.dW_DW_TABA_NO_2 == 0) {
        jsonResult.dW_DW_TABA_NO_2 = "";
    }
    if (jsonResult.dW_DW_TABA_NO_3 == 0) {
        jsonResult.dW_DW_TABA_NO_3 = "";
    }
    if (jsonResult.dW_DW_TABA_NO_4 == 0) {
        jsonResult.dW_DW_TABA_NO_4 = "";
    }
    $("#txtThaba2").val(jsonResult.dW_DW_TABA_NO_2);
    $("#txtThaba3").val(jsonResult.dW_DW_TABA_NO_3);
    $("#txtThaba4").val(jsonResult.dW_DW_TABA_NO_4);
    $("#txtRemarks").val(jsonResult.dW_REMARKS);

    if (jsonResult.dW_DW_ACT_DIM_CHK == 0) {
        $("#chkActualDim").prop('checked', false);
        $("#txtActual_Start").removeClass("Mandatory1");
        $("#txtActual_End").removeClass("Mandatory1");

    } else {

        $("#chkActualDim").prop('checked', true);

        $("#txtActual_Start").addClass("Mandatory1");
        $("#txtActual_End").addClass("Mandatory1");
    }



    bindEvents();
    // oldHikinukiRemarks = $.trim(jsonResult.DW_REMARKS);
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
    debugger;
    if ($("#chkmiddle").is(":checked") == true) {
        disableRedrawCheckbox();
        $("#chkActualDim").click();
    } else {
        enableRedrawCheckbox();
        $("#chkActualDim").click();
    }
});
function setNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
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
            
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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

$("#chkActualDim").click(function () {



    if ($("#chkActualDim").is(":checked")) {
        $("#txtActual_Start").addClass("Mandatory1");
        $("#txtActual_End").addClass("Mandatory1");


    } else {

        $("#txtActual_Start").removeClass("Mandatory1");
        $("#txtActual_End").removeClass("Mandatory1");
    }
});
function GetWindingShape() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetWindingShape",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            $("#ddlWindingshape").html("").append($('<option value="0"></option>'));
            $.each(response.message, function (key, jsonObj) {
                $("#ddlWindingshape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

            });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
        url: APIURL + "/ColdDrawingWIREDRAWING/GetMachineList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '" >' + jsonObj.name + '</option>'));

    });
}
function switchTab($tabFrom, $tabTo) {

    $tabTo.click();

    /* enableEditDeleteButtons();*/
}
function deleteColdDrawingWireDrawingDetails() {

   var ajaxTime = new Date().getTime();

   var jsonDataDelete= JSON.stringify({


        DRAWING_CD: parseInt($("#hfld_DrawingCd").val()),
        
        PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),
        MSG_CD: parseInt($("#hfld_MessageCD").val()),
        //DW_TABA_NO_1: $("#txtThaba1").val(),
        //DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        //DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
       //DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
       DW_TABA_NO_1: $("#txtThaba1").val().trim(),
       DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val().trim() : null,
       DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val().trim() : null,
       DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val().trim() : null,
    });


    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            resetGlobalVariables();
            disableStartDataPanel();
            disableEndDataPanel();
            disableModelDataPanel();
            disableEditDeleteButtons();
            //initialize();
            clearPanel($("#pnlProductDetails"));
            clearPanel($("#pnlProcessStartData"));
            clearPanel($("#pnlProcessEndData"));   
            var successMessage = $("#lblErrorMessageE_ColdDrawing_EDITDELETE_DataDeleted").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            // switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
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
        DW_IS_REMARKS_UPDATE: isDrawingRemarksUpdate,
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR_CD: parseInt($("#ddlStartOprName").find(":selected").val()),
        //ROLLING_NUMBER: $("#txtRollingnumber").val(),
        //FURNACE_NO: $("#txtFurnaceNum").val(),
        //DIE_DIM: $("#txtDieDim").val(),
        //DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        //DIE_NO: $("#txtDieNo").val(),
        //DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        //WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        //IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        //REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),
        //// end Operator
        //DW_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        //DW_END_OPERATOR_CD: parseInt($("#ddlEndOprName").find(":selected").val()),
        //DW_ACT_DIM_CHK: ($("#chkActualDim").is(':checked') ? 1 : 0),
        //DW_ACT_DIM_START: $("#txtActual_Start").val() ? $("#txtActual_Start").val() : null,
        //DW_ACT_DIM_END: $("#txtActual_End").val() ? $("#txtActual_End").val() : null,
        //DW_FINISHED_COILS: $("#txtNoOfUnits").val(),

        //DW_FINISHED_QTY: $("#txtWeight").val(),
        //DW_TABA_NO_1: $("#txtThaba1").val(),
        //DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        //DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        //DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
       
        ROLLING_NUMBER: $("#txtRollingnumber").val().trim(),
        FURNACE_NO: $("#txtFurnaceNum").val().trim(),
        DIE_DIM: $("#txtDieDim").val().trim(),
        DIE_SHAPE: $("#ddlDieShape option:selected").val(),
        DIE_NO: $("#txtDieNo").val().trim(),
        DIE_CHK_START: $("#ddlDiceInspectionStart option:selected").val(),
        WINDING_SHAPE: $("#ddlWindingshape option:selected").val(),
        IM_DRAWING: ($("#chkmiddle").is(':checked') ? 1 : 0),
        REDRAWING: ($("#chkReDrawing").is(':checked') ? 1 : 0),

        DW_END_DATE: ($("#txtEndDate").val().split(" ")[0].trim() + "T" + $("#txtEndDate").val().split(" ")[1].trim() + ":00.000Z").replaceAll('/', '-'),
        DW_END_OPERATOR_CD: parseInt($("#ddlEndOprName").find(":selected").val()),

        DW_ACT_DIM_CHK: ($("#chkActualDim").is(':checked') ? 1 : 0),
        DW_ACT_DIM_START: $("#txtActual_Start").val() ? $("#txtActual_Start").val().trim() : null,
        DW_ACT_DIM_END: $("#txtActual_End").val() ? $("#txtActual_End").val().trim() : null,

        DW_FINISHED_COILS: $("#txtNoOfUnits").val().trim(),
        DW_FINISHED_QTY: $("#txtWeight").val().trim(),

        DW_TABA_NO_1: $("#txtThaba1").val().trim(),
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val().trim() : null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val().trim() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val().trim() : null,

        DW_EXT_TEXTURE_STATUS: $("#ddlExternalSurfaceCondition option:selected").val(),
        DW_MAGNETIC_INSPECTIONS: $("#ddldustdetection option:selected").val(),
        DIE_CHK_END: $("#ddlDiceInspectionEnd option:selected").val(),
        DW_DECISION: $("#ddlDecision option:selected").val(),
        DW_NEXT_PROCESS: $("#ddlNextProcess option:selected").val(),

        DW_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        USER_ID: parseInt($("#hdfLoginId").val()),
        USER_NAME: userName
    });

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/ColdDrawingWireDrawingDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            disableModelDataPanel();
            if (response.message > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }
            var successMessage = $("#lblSuccessMessageS_ColdDrawing_EditDelete_001").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            var jsonResult = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
            isDrawingRemarksUpdate = 1;
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
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
        url: "/ColdDrawingWIREDRAWING/GetDecision",
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
function setMagneticDust() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
function setDiceInspEnd() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
function setDiceInspStart() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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

function bindwindingshapedropdown(jsonResult) {
    $("#ddlWindingshape").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlWindingshape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setExteriorConditionKubunGet() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
function setDieDiaShape() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
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
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
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
            if ($("#txtActual_Start").val().trim() == "") {
                focusFieldID = "txtActual_Start";
            } else {
                focusFieldID = "txtActual_End";
            }
        }

        showMessageDialog(message, focusFieldID, WARNING)

    }
    return returnVal;
}

function IsDropDownMandatory() {
    var returnVal = true;
    var message = "";
    if ($("#ddlStartOprName").val() == "0") {
        var focusFieldID = "ddlStartOprName";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        showMessageDialog(message, "ddlStartOprName", WARNING);
        returnVal = false;
    }
    else if ($("#ddlEndOprName").val() == "0") {
        var focusFieldID = "ddlEndOprName";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        showMessageDialog(message, "ddlEndOprName", WARNING);
        returnVal = false;
    }

    return returnVal;
}


function initialize() {
    combinedDecisionOK = "";
    activeMachineCD = "";
    endOperatorCD = "";
    fillEmptyGrid();
    $("#txtChuubanSearch").val("");
    $("#txtCustomerName").val("");
    $("#txtProdNameCust").val("");
    $("#txtProdNameInt").val("");
    $("#txtDimDMin").val("");
    $("#txtDimDMax").val("");
    $("#txtShap").val("");
    $("#txtLengthSearchMin").val("");
    $("#txtStartDate").val("");
    $("#txtEndDate").val("");
    $("#txtLengthSearchMax").val("");

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
    disableStartDataPanel();
    disableEndDataPanel();
    disableModelDataPanel();
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

    // Fllowing is to clear readonly hidden fields
    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
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
        url: "/ColdDrawingWireDrawing/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divColdDrawingWireDrawingGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
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
function setDecimalFormats() {
    $("#txtFinishedBarCount, #txtNoOfUnits").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtNGQty, #txtNgWeight, #txtWeight").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    }); 
    $("#txtDieDim, #txtActualDieDimStart, #txtActualDieDimEnd, #txtActual_Start, #txtActual_End").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
   
}

function Mask() {
    $("#txtOperationDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtOperationDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

}
function IsEndDateValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_Cold_Drawing_EditDelete_008").text().trim();
        var focusFieldID = "txtEndTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_Cold_Drawing_007").text().trim();
        var focusFieldID = "txtEndDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
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


var gridViewScroll = null;



$(document).ready(function () {
    // bindOperatorList(''); 
    $("#txtChuubanSearch").focus();
    disableEndDataPanel();
    disableStartDataPanel();
    disableModelDataPanel();
    InitSortPopup();

    $.support.cors = true;
    SetGlobalVariables();

    window.resizeTo(1090, 800);


    setDecimalFormats();

    disableEditDeleteButtons();

    userName = $("#hdfUserName").val();
    Mask();
    $("#lbl_DateOfIssue").show();
    $("#txtCreatedDate").show();
    $("#lblUpdatedDate").show();
    $("#txtUpdatedDate").show();

    $("#lbl_DateOfIssue").hide();
    $("#txtCreatedDate").hide();
    $("#lblUpdatedDate").hide();
    $("#txtUpdatedDate").hide();
});
function btnSearch_Click() {

    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var txtDimDMax = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var txtShap = $("#txtShap").val();
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

    if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || parseInt(txtDimDMin) > 0 || parseInt(txtDimDMax) > 0 || txtShap.length > 0 || parseInt(txtLengthSearchMin) > 0 || parseInt(txtLengthSearchMax) > 0) {

        ColdDrawingGridSearch(null);

    }
    else {

        message = $.trim($("#lblErrorMessageE_ColdDrawing_SearchField").text());
        showMessageDialog(message, 'txtChuubanSearch', WARNING);
    }
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
    var txtDimDMin = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var txtDimDMax = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
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
        url: APIURL + "/ColdDrawingWireDrawing/GridSearch",
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
                    url: "/ColdDrawingWireDrawing/GridSearch",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)

                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divColdDrawingWireDrawingGridMain").html(response1.message);

                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });
            }



        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });


    closepopup();
}


function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {

    var settings = {
        async: false,
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

function OperatorIsInactive(dropdownName) {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ColdDrawingWIREDRAWING/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
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
            return false;
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
            url: APIURL + "/ColdDrawingWIREDRAWING/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_Cold_Drawing_EditDelete_001").text().trim(); 
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});



