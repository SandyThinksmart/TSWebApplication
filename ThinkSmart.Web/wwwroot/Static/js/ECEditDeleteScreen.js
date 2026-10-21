var $btnLnkEdit;
var $btnDelete;
var combinedDecisionOK = "";
var indexDDLDecisionNothing = 0;

var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";

var registeredThabaNumbersWithDecisionOK = {};
var registeredThabaNumbersAndDecision = {};

var startOperatorCD = "";
var endOperatorCD = "";
var operatorCD = 0;

function SetGlobalVariables() {
    $btnLnkEdit = $("#btnLnkEdit");
    $btnUpdate = $("#btnUpdate");
    $btnDelete = $("#btnDelete");
        
}

function DisableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function EnableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
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
    $("#ddlModel").removeClass(DISABLED).removeAttr(DISABLED);

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

    $("#ddlModel").attr(DISABLED, DISABLED).addClass(DISABLED);
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
    var $fiDataPanel = $("#pnlFIEntry");
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
    $("#ddlDimDecision").val(0);
    $("#ddlColorDecision").val(0);
    $("#ddlActLenDecision").val(0);
    $("#ddlChamferDecision").val(0);
    
    $("#ddlExtTextureDecision").val(0);
    $("#ddlFINextProcess").val(0);
}


function EnableFIDataPanel() {

    var $startDataPanel = $("#pnlFIEntry");

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

function Initialize() {
    combinedDecisionOK = "";
    operatorCD = 0;
   // $(".FocusDefault").focus();

    ClearPanel($("#pnlModel"));
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    ClearPanel($("#pnlFIEntry"));
    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableFIDataPanel();
    DisableEditDeleteButtons();
    FillEmptyGrid();
    ClearGridFilters();
}
function ClearGridFilters() {
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
}
function FillEmptyGrid() {
    var emptyList = [];
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ECEntry/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divECGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function ClearPanel($panel) {
    // Set specified panel elements to disabled
    $.each($panel.find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });
    $.each($panel.find("select"), function (index, dropdown) {
        $("#" + dropdown.id).empty();
       
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

function ClearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}

function ToggleECOnlineFieldsMandatory($this) {
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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function SwitchTab($tabFrom, $tabTo) {
    $tabTo.click();
  
    EnableEditDeleteButtons();
}

function SetECDetails(jsonResult) {
   
    SwitchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    combinedDecisionOK = jsonResult.decisioN_OK;
    $("#hfID_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD); 
    $("#hfld_ECCD").val(jsonResult.eC_CD); 
    $("#hdfMachineCD").val(jsonResult.machinE_CD); 
    $("#hfld_MessageCD").val(jsonResult.msG_CD)
    $("#hdfNifudaCD").val(jsonResult.nifudA_CD) 
    $("#hdfFinal_Insp").val(jsonResult.finaL_INSP) 
    
    SetMachineList();
    $("#ddlModel").val(jsonResult.machinE_CD);
    
    startOperatorCD = jsonResult.eC_START_OPERATOR_CD;
    
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), startOperatorCD);

    endOperatorCD = jsonResult.eC_END_OPERATOR_CD;
    
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), endOperatorCD);

    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.createD_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.stR_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOBAN); 
    $("#txt_HostName").val(jsonResult.producT_NYUKO_CD);

    $("#txtDeliveryDateT1").val($.format.date(new Date(jsonResult.pP_DELIVERY_DATE_STRING), "yyyy/MM/dd"));
    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);
    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
    }
    if (jsonResult.pP_UNIT_WT_STRING !="") {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_STRING);
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
    $("#txtPackingT1").val(jsonResult.pP_PACKING);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_STRING);            //text(jsonResult.PP_NO_OF_UNITS + '' + jsonResult.PP_UNIT);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING_STRING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    
        $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
   
    $("#hfld_ECCD").val(jsonResult.eC_CD);
    $("#txtStartDate").val($.format.date(new Date(jsonResult.eC_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    
    $("#txtTabaNo").val(jsonResult.eC_TABA_NO); 
    $("#ddlTPTest").val(jsonResult.tP_TEST);
    $("#txtMetalicPltNo").val(jsonResult.eC_METALLIC_PLATE_CODE); 
    $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
    $("#txtNoOfCoils").val(jsonResult.acT_ALLOC_UNITS);
    $("#txtCoilQuantity").val(jsonResult.acT_ALLOC_QTY);
    $("#txtIsOK").val(jsonResult.iS_OK);
    $("#txtCoilSize").val(jsonResult.eC_COIL_SIZE);
    $("#txtCoilShape").val(jsonResult.eC_COIL_SHAPE);
    $("#txtFurnaceNum").val(jsonResult.furnacE_NO);
    $("#txtShikii").val(jsonResult.shikiI_VALUE);
    $("#txtLineSpeed").val(jsonResult.linE_SPEED);
    
   
    
    if (jsonResult.reec == 1) {
        $("#chkReEC").prop("checked", true);
    } else {
        $("#chkReEC").prop("checked", false);
    }
    
    
    $("#txtEndDate").val($.format.date(new Date(jsonResult.eC_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

   
    $("#ddlDecision").val(jsonResult.eC_DECISION);
    $("#ddlInspection").val(jsonResult.eC_VISUAL_CHK); 
    
    if (jsonResult.finaL_INSP == 1) {
        $("#chkFICheck").prop("checked", true);
        EnableFIDataPanel();
        DisableDefaultField();
        GetHardnessTensileDecision();
    } else {
        $("#chkFICheck").prop("checked", false);
        DisableFIDataPanel();
    }
    
    $("#ddlNextProcess").val(jsonResult.eC_NEXT_PROCESS);

    $("#txtNGBars1").val(jsonResult.eC_NG_BARS1);
    $("#ddlNGReason1").val(jsonResult.eC_NG_DETAILS1);
    DdlNGReasonMandatory($("#txtNGBars1"), $("#ddlNGReason1"));


    $("#txtNGBars2").val(jsonResult.eC_NG_BARS2)
    $("#ddlNGReason2").val(jsonResult.eC_NG_DETAILS2);

    DdlNGReasonMandatory($("#txtNGBars2"), $("#ddlNGReason2"));

    $("#txtNGBars3").val(jsonResult.eC_NG_BARS3);
    $("#ddlNGReason3").val(jsonResult.eC_NG_DETAILS3);
    DdlNGReasonMandatory($("#txtNGBars3"), $("#ddlNGReason3"));

    $("#txtOKBars").val(jsonResult.eC_OK_BARS);
    $("#ddlUnit").val(jsonResult.uniT_TYPE),
    $("#txtOKQty").val(jsonResult.eC_OK_QTY);
    $("#txtNGWt").val(jsonResult.eC_NG_QTY);
    $("#txtActualLength").val(jsonResult.eC_ACT_LENGTH);
    $("#txtActDim1").val(jsonResult.eC_ACT_DIM1);
    $("#txtActDim2").val(jsonResult.eC_ACT_DIM2);
    $("#txtActDim3").val(jsonResult.eC_ACT_DIM3);
    $("#txtActDim4").val(jsonResult.eC_ACT_DIM4);
    $("#txtDeadLengthA").val(jsonResult.deaD_LENGTH_A);
    $("#txtDeadLengthB").val(jsonResult.deaD_LENGTH_B); 
    if (parseInt(jsonResult.eC_NG_BAR_TOTAL) > 0) {
        $("#txtNGBarsTotal").val(jsonResult.eC_NG_BAR_TOTAL);
    }
    if (parseInt(jsonResult.eC_NG_PERCENTAGE) > 0) {
        $("#txtDefectRate").val(jsonResult.eC_NG_PERCENTAGE);
    }
    if (jsonResult.eC_NG_BAR_TOTAL > 0) {
        CalculateBarCount();
    }
   
    $("#txtMagnetism").val(jsonResult.magnetism);
    $("#txtRemarks").val(jsonResult.eC_REMARKS);
    
    $("#ddlDimDecision").val(jsonResult.fI_DIM_DECISION); 
    $("#ddlColorDecision").val(jsonResult.fI_COLOR_DECISION);
    $("#ddlActLenDecision").val(jsonResult.fI_LENGTH_DECISION); 
    $("#ddlChamferDecision").val(jsonResult.fI_CHAMFERING_DECISION); 
    $("#ddlCombinedDecision").val(jsonResult.combineD_DECISION);
    $("#ddlExtTextureDecision").val(jsonResult.fI_EXT_TEXTURE_DECISION);
    $("#ddlFINextProcess").val(jsonResult.fI_NEXT_PROCESS); 
    
    $("#txtColorDecision").val(jsonResult.pP_COLOR);
    if (jsonResult.pP_CHAMFER > 0) {
        $("#txtChamferDecision").val("C-" + jsonResult.pP_CHAMFER);
    }
    SetDecimalFormats();
 
}


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
        $("#txtDefectRate").val(defectPercent.toFixed(2));
    }
}
function GetECDetailsByECCD($callFrom) {

    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var ecCD = $.trim($callFrom.closest("tr").attr("data-lblECCD"));
    
      
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetECDetailsByECCD?ecCD=" + ecCD + "&cultureName=" + $("#hdfCultureName").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = null;
            jsonResult = response.message;
           
            SetFIDropdown();
           
            SetDropDownLists();
            SetUnit();
            EnableStartDataPanel();
            EnableEndDataPanel();
            DisableDefaultField();
            SetECDetails(jsonResult);
            window.resizeTo(1080, 970);
            $("#ddlModel").focus();
       

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });


}

function UpdateECDetails() {
    
    var ajaxTime = new Date().getDate();

    var EC_Percentage = null;
    if ($("#txtDefectRate").val() > 0) {
        EC_Percentage = Math.round($("#txtDefectRate").val());
    }
    if ($("#chkFICheck").prop("checked") == true)
    {
        if (parseInt($("#hdfFinal_Insp").val()) == 0) {
            $("#hdfFinal_Insp").val(1);
        } else {
            $("#hdfFinal_Insp").val(0);
        }
    }
    var jsonDataForSave = JSON.stringify({
        EC_CD: $("#hfld_ECCD").val() ? $("#hfld_ECCD").val() : null,

        PROCESS_PLAN_CD: $("#hfID_ProcessPlanCD").val(),
        MACHINE_CD: $("#ddlModel").val(),
        EC_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        EC_START_OPERATOR_CD: $("#ddlStartOperatorNameValue").val(),
        EC_TABA_NO: $("#txtTabaNo").val(),
        TP_TEST: $("#ddlTPTest").val(),
        EC_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val(),
        ACT_ALLOC_UNITS: $("#txtNoOfCoils").val(),
        ACT_ALLOC_QTY: $("#txtCoilQuantity").val(),
        IS_OK: $("#txtIsOK").val(),
        EC_COIL_SIZE: $("#txtCoilSize").val(),
        EC_COIL_SHAPE: $("#txtCoilShape").val(),
        FURNACE_NO: $("#txtFurnaceNum").val(),
        SHIKII_VALUE: $("#txtShikii").val() ? $("#txtShikii").val() : null,
        LINE_SPEED: $("#txtLineSpeed").val() ? $("#txtLineSpeed").val() : null,
        REEC: ($('#chkReEC').is(':checked') ? 1 : 0),

        EC_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        EC_END_OPERATOR_CD: $("#ddlEndOperatorNameValue").val(),
        EC_VISUAL_CHK: $('#ddlInspection').val(),
        EC_NG_BARS1: $("#txtNGBars1").val() ? $("#txtNGBars1").val() : null,
        EC_NG_DETAILS1: $("#ddlNGReason1").val() > 0 ? $("#ddlNGReason1").val() : null,
        EC_NG_BARS2: $("#txtNGBars2").val() ? $("#txtNGBars2").val() : null,
        EC_NG_DETAILS2: $("#ddlNGReason2").val() > 0 ? $("#ddlNGReason2").val() : null,
        EC_NG_BARS3: $("#txtNGBars3").val() ? $("#txtNGBars3").val() : null,
        EC_NG_DETAILS3: $("#ddlNGReason3").val() > 0 ? $("#ddlNGReason3").val() : null,
        EC_OK_BARS: $("#txtOKBars").val(),
        UNIT_TYPE: $("#ddlUnit").val(),
        EC_OK_QTY: $("#txtOKQty").val(),
        EC_NG_QTY: $("#txtNGWt").val() ? $("#txtNGWt").val() : null,
        EC_ACT_DIM1: $("#txtActDim1").val(),
        EC_ACT_DIM2: $("#txtActDim2").val() ? $("#txtActDim2").val() : null,
        EC_ACT_DIM3: $("#txtActDim3").val() ? $("#txtActDim3").val() : null,
        EC_ACT_DIM4: $("#txtActDim4").val() ? $("#txtActDim4").val() : null,
        DEAD_LENGTH_A: $("#txtDeadLengthA").val() ? $("#txtDeadLengthA").val() : null,
        DEAD_LENGTH_B: $("#txtDeadLengthB").val() ? $("#txtDeadLengthB").val() : null,
        EC_ACT_LENGTH: $("#txtActualLength").val(),   
       
        EC_NG_BAR_TOTAL: $("#txtNGBarsTotal").val() ? $("#txtNGBarsTotal").val() : null,
        EC_NG_PERCENTAGE: EC_Percentage,
        MAGNETISM: $("#txtMagnetism").val() ? $("#txtMagnetism").val() : null,
        EC_DECISION: $("#ddlDecision").val(),
        EC_NEXT_PROCESS: $("#ddlNextProcess").val(),

        EC_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,   
        NIFUDA_CD: $("#hdfNifudaCD").val() ? $("#hdfNifudaCD").val() : null,
        IS_FI_CHECK: $("#hdfFinal_Insp").val() ? $("#hdfFinal_Insp").val() : 0, 
        FINAL_INSP: ($('#chkFICheck').is(':checked') ? 1 : 0), 
        FI_DIM_DECISION: $("#ddlDimDecision").val() ? $("#ddlDimDecision").val() : null,
        FI_LENGTH_DECISION: $("#ddlActLenDecision").val() ? $("#ddlActLenDecision").val() : null,
        COMBINED_DECISION: $("#ddlCombinedDecision").val() ? $("#ddlCombinedDecision").val() : null,
        FI_NEXT_PROCESS: $("#ddlFINextProcess").val() ? $("#ddlFINextProcess").val() : null,
        FI_COLOR_DECISION: $("#ddlColorDecision").val() ? $("#ddlColorDecision").val() : null,
        FI_CHAMFERING_DECISION: $("#ddlChamferDecision").val() ? $("#ddlChamferDecision").val() : null,
        FI_EXT_TEXTURE_DECISION: $("#ddlExtTextureDecision").val() ? $("#ddlExtTextureDecision").val() : null,

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
            if (response.message > 0) {
                $("#hfld_MessageCD").val(response.message);
            } else {
                $("#hfld_MessageCD").val("");
            }
            var successMessage = $("#lblSuccessMessageS_HIKI_EDIT_DELETE_001").text();
            ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnDelete").click(function (event) {
   
    DeleteSECDetails();
    btnSearch_Click();
    
});

function DeleteSECDetails() {
    var ajaxTime = new Date().getTime();
    var jsonData = JSON.stringify({
        EC_CD: $("#hfld_ECCD").val() ? $("#hfld_ECCD").val() : null,
        PROCESS_PLAN_CD: $("#hfID_ProcessPlanCD").val(),
        MACHINE_CD: $("#ddlModel").val(),
        EC_TABA_NO: $("#txtTabaNo").val(),
        MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null
       
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/ECDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            DisableEditDeleteButtons();
           
            var successMessage = $("#lblSuccessMessageS_HIKI_EDIT_DELETE_002").text();
            ShowMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);

        });
}
$("#btnUpdate").click(function (event) {

    if ($("#txtCoilSize").val() == "" || $("#txtCoilShape").val() == "") {
        $("#checkCoil").addClass("Mandatory");
    } else {
        $("#checkCoil").removeClass("Mandatory");
    }
        

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

     if (!CheckDate($("#txtStartDate"))) {
         var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_014").text().trim(); // This message will be displayed on dialog box
        ShowMessageDialog(message, "txtStartDate", WARNING);
        return false;
    }
    if (!CheckDate($("#txtEndDate"))) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_015").text().trim(); // This message will be displayed on dialog box
        ShowMessageDialog(message, "txtEndDate", WARNING);
        return false;
    }

    if (!IsEndDataValid()) {
        return false;
    }
    
   
    if ($("#ddlModel option:selected").attr("flag") == 1) {

        DisplayInactivePopup("ddlModel");
        return false;
    }

    if ($("#ddlStartOperatorNameValue option:selected").attr("flag") == 1) {

        DisplayInactivePopup("ddlStartOperatorNameValue");
        return false;
    }

    if ($("#ddlEndOperatorNameValue option:selected").attr("flag") == 1) {

        DisplayInactivePopup("ddlEndOperatorNameValue");
        return false;
    }
    if (!IsDuplicateThabaNo($("#hfID_ProcessPlanCD").val())) {
        return false;
    }

    if (parseInt($("#hdfNifudaCD").val()) > 0)
    {
        if (!IsFIAlreadyDone() && $("#chkReEC").prop("checked") == false) {
            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_020").text().trim();
            ShowMessageDialog(message, "txtTabaNo", WARNING);
            return false;
        }
    }
    if ($("#chkFICheck").prop("checked") == true && $("#ddlCombinedDecision").val() == combinedDecisionOK) {
        if (($("#txtHardnessDecision").val() != $("#lblOK").text() && $("#txtHardnessDecision").val() != $("#lblNA").text())) {
            ShowMessageDialog($("#lblErrorMessageE_HIKI_EDIT_DELETE_021").text().trim(), "ddlCombinedDecision", WARNING);
            return false;
        } else if (($("#txtTensileDecision").val() != $("#lblOK").text() && $("#txtTensileDecision").val() != $("#lblNA").text())) {
            ShowMessageDialog($("#lblErrorMessageE_HIKI_EDIT_DELETE_021").text().trim(), "ddlCombinedDecision", WARNING);
            return false;
        }
    }
    
    SetFinalInspStatus($("#hfld_ECCD").val()); 
  
    UpdateECDetails();
    btnSearch_Click();


});

function SetFinalInspStatus(ecCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetFinalInspByECCD?ecCD=" + ecCD,
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            $("#hdfFinal_Insp").val(response.message)
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function DisplayInactivePopup(fieldName) {
    
    var tableName = "";
    var fieldID = fieldName + " option:selected";
    var nameValue = $('#' + fieldID).text();
    if (fieldName == "ddlModel") {
        tableName = "machine";
    } else {
        tableName = "employee";
    }
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ECEntry/DisplayInactivePopup?nameValue=" + nameValue + "&tableName=" + tableName,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            ShowMessageDialog(response.message, fieldName, WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnStartDateTime").click(function (event) {
    SetStartSysDateTime();
});
$("#btnEndDateTime").click(function (event) {
    SetEndSysDateTime();
});

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
   
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

        if (focusFieldID == "checkCoil") {
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
function IsDuplicateThabaNo(processPlanCD) {
    
    var returnVal = true;
    var thabaNo = $("#txtTabaNo").val();
    var eCCD = $("#hfld_ECCD").val();
    
    var ajaxTime = new Date().getDate();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetThabaDetailsByThabaNumber?processPlanCD=" + processPlanCD + "&thabaNo=" + thabaNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
           
            if (jsonResult == null) {
                returnVal = true;
            } else
            {
                if (jsonResult.eC_CD == eCCD || jsonResult.eC_CD != eCCD && $("#chkReEC").is(":checked") == true) {
                    returnVal = true;
                } else {
                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_016").text();
                    ShowMessageDialog(message, "txtTabaNo", WARNING);

                    returnVal = false;
                }
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

    return returnVal;
}


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

function IsEndDataValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_007").text().trim();
        var focusFieldID = "txtEndTime";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_009").text().trim();
        var focusFieldID = "txtEndTime";
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

function ResetResponseTime() {
    responseTime = ""; // Clear server response time
}
function IsFIAlreadyDone() {
    var ajaxTime = new Date().getTime();
    var returnval = false;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/IsFinalInspectionCompleted?nifudaCD=" + $("#hdfNifudaCD").val() + "&thabaNo=" + $("#txtTabaNo").val() + "&ecCD=" + $("#hfld_ECCD").val(),
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
            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnval;
}
function btnSearch_Click() {
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $("#txtDimDMin").val();
    var PROD_DIM_D_MAX = $("#txtDimDMax").val();
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val();
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val();
    var errorMessage = "";
    var checkFocusField = "";

    if ($("#txtDimDMin").val().trim() == "" && $("#txtDimDMax").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_018").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_019").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_019").text());
        }
    }

    if (errorMessage != "") {
        ShowMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }
    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PROD_DIM_D_MIN.length > 0 || PROD_DIM_D_MAX.length > 0 || PROD_SHAPE.length > 0 || PROD_LENGTH_MIN.length > 0 || PROD_LENGTH_MAX.length > 0

    ) {
        ECGridSearch(null);
    }
    else {
        var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_017").text());
        ShowMessageDialog(message, 'txtChuubanSearch', WARNING);
       
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
    ECGridSearch(selectedColumns);
}


function ECGridSearch(sortfields) {
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var PROD_DIM_D_MAX = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    var USER_ID = $("#hdfLoginId").val();


        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/EC/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX, PROD_SHAPE: PROD_SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX,
                SORT_FIELDS: sortfields, USER_ID: USER_ID
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
                            url: "/ECEntry/GridSearch",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)

                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#divECGridMain").html(response1.message);
                                }
                            })
                            .fail(function (xhr, textStatus, errorThrown) {

                                responseTime = new Date().getTime() - ajaxTime;
                                var errorCode = $.parseJSON(xhr.responseText).Message;

                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text()); /*errorMsg_SelectAtLeastOne*/
                                ShowMessageDialog(message, focusFieldID, ERROR);
                            });
                    
                    
                }


                //else {
                //    focusFieldID = $(':focus').attr("id");
                //    message = $.trim($("#lblErrorMessage_Hikinuki_SearchField").text());
                //    showMessageDialog(message, focusFieldID, WARNING);
                //}

            })
            .fail(function (xhr, textStatus, errorThrown) {
               
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    ClosePopup();
}
function btnSortReset_Click() {
    InitSortPopup();
}
function ClosePopup() {

    $("#SortPopUpModal").modal('hide');
}
function InitSortPopup() {
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/ECEntry/InitializeSortPopup",
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
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
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

function LoadReSizeScreen() {

    window.resizeTo(1080, 970);
    document.getElementById('TabContainerPMS').style.height = '380px';

}
$("#chkFICheck").click(function () {
    if ($("#chkFICheck").prop("checked") == true) {
        EnableFIDataPanel();
        DisableDefaultField();
        GetHardnessTensileDecision();
        $("#ddlCombinedDecision").prop('selectedIndex', 0);
        $("#ddlFINextProcess").prop('selectedIndex', 0);
    } else {
       
        DisableFIDataPanel();
        $("#ddlCombinedDecision").prop('selectedIndex', -1);
        $("#ddlFINextProcess").prop('selectedIndex', -1);
        $("#txtHardnessDecision").val("");
        $("#txtTensileDecision").val("");
    }
});
function DisableDefaultField() {
    $("#txtDefectRate").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtColorDecision").attr(DISABLED, DISABLED).addClass(DISABLED); 
    $("#txtChamferDecision").attr(DISABLED, DISABLED).addClass(DISABLED); 
    $("#txtNGBarsTotal").attr(DISABLED, DISABLED).addClass(DISABLED); 
    $("#txtHardnessDecision").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtTensileDecision").attr(DISABLED, DISABLED).addClass(DISABLED);
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

function SetMachineList() {
   
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetMachineList?machineType=120, 130",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMachineDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindMachineDropdown(jsonResult) {
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

function SetTPTest() {

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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindTPTestDropdown(jsonResult) {

    $("#ddlTPTest").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindNextProcessDropdown(jsonResult) {

    $("#ddlNextProcess").html("");
   // $("#ddlFINextProcess").html("");

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
          //  $("#ddlFINextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));

        });
    }
    if ($("#hdfCultureName").val() == "ja-JP") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
          //  $("#ddlFINextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));

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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindFINextProcessDropdown(jsonResult) {

    $("#ddlFINextProcess").html("");

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlFINextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));

        });
    }
    if ($("#hdfCultureName").val() == "ja-JP") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlFINextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));

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
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDecisionDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindDecisionDropdown(jsonResult) {
    $("#ddlDecision").html("");
    $("#ddlCombinedDecision").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
        $("#ddlCombinedDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function SetProcessDefectList() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL+ "/EC/GetProcessDefect",
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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
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
function SetDropDownLists() {
    SetNextProcess();
    SetFINextProcess();
    SetTPTest();
    SetDecision();
    SetProcessDefectList();
}

$("#btnInitialize").click(function (event) {
    
    
    SwitchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    window.resizeTo(1080, 970);
    Initialize();
    $("#txtChuubanSearch").focus();
});
$("#TbPMSEditDelTab1-tab").click(function () {
    window.resizeTo(1080, 970);
});
$("#TbPMSEditDelTab2-tab").click(function () {
    window.resizeTo(1080, 970);
});

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
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindFIDropdown(jsonResult) {
    $("#ddlDimDecision").html("").append($('<option></option>'));
    $("#ddlColorDecision").html("").append($('<option></option>'));
    $("#ddlActLenDecision").html("").append($('<option></option>'));
    $("#ddlChamferDecision").html("").append($('<option></option>'));
    
    $("#ddlExtTextureDecision").html("").append($('<option></option>'));
    $("#ddlInspection").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDimDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlColorDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlActLenDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlChamferDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    
        $("#ddlExtTextureDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlInspection").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
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
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindUnitDropdown(jsonResult) {
    $("#ddlUnit").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnit").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function Mask() {
    $("#txtStartDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm" });
    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm" });

}
const fixedWidth = 1080;
const fixedHeight = 970;

let resizeTimer;

window.addEventListener("resize", function () { // alert popup shown if screen resize
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function () {
        if (window.outerWidth !== fixedWidth || window.outerHeight !== fixedHeight) {
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_022").text());
            showMessageDialog(message, "", WARNING);
        }
    }, 300); // Fires after resizing stops
});


$(document).ready(function () {
    $("#txtChuubanSearch").focus();
    Mask();
    SetDecimalFormats();

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableFIDataPanel();
    SetGlobalVariables();
    
    window.resizeTo(1080, 970);
      
    DisableEditDeleteButtons();
   
       
});

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
$("#ddlNextProcess").focusout(function () {
   
    if ($("#ddlDimDecision").hasClass("disabled")) {
        $("#btnUpdate").focus();
    } else {
        $("#ddlDimDecision").focus();
    }
});

$("#ddlFINextProcess").focusout(function () {
      
        $("#btnUpdate").focus();
   
});
$("#btnSort").focusout(function () {

    $("#btnlnkedit").focus();
    
});

function GetHardnessTensileDecision() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetHardnessTensileDescion?processPlanCD=" + $("#hfID_ProcessPlanCD").val() + "&cultureName=" + $("#hdfCultureName").val(),
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
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
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
            url: APIURL + "/EC/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});