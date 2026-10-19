var $ActiveID = "";
var userName = "";
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";
var rowLimitValue = "";

var heatTreatmentFlag = 0;
var drawingBarToBarFlag = 0;
var drawingCoilToBarFlag = 0;
var drawingCoilToCoilFlag = 0;
var straighteing1Flag = 0;
var straighteing2Flag = 0;
var chamferFlag = 0;
var ecFlag = 0;
var stamppingFlag = 0;
var hardnessTemperingFlag = 0;
var postHTFlag = 0;
var mechHardnessFlag = 0;
var mechTensileFlag = 0;
var finalInspectionFlag = 0;
var generalProcessFlag = 0;
var transferReportFlag = 0;


$(document).ready(function () {  
    Mask();   
    
    $("#activeTabID").val("tabHeatTreatment");
    var targetValue = "titleHeatTreatment"
    $ActiveID = targetValue;
    if ($ActiveID == "titleHeatTreatment") {
        InitSortPopup($ActiveID);  
       
        SetMachineTypeList($("#hdfMachineTypeHeatTreatment").val());
        $("#lbldate").text($("#labelFuranceOutDate").text());
        $("#lblOperator").text($("#labelFurnaceVerifyBy").text());        
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();

    }
   
    setLocationList();
    //setCustomerList("", $("#ddlCustomer"),"");     
    setDecimalFormats();
    $("#btnExportCSV").attr("DISABLED", "DISABLED");
    $("#txtChuuban").focus();
    $("#ddlOperator").removeClass("Mandatory");
    $("#ddlOperator").css('background-color', 'white');  
    $("#ddlCustomer").removeClass("Mandatory");
    $("#ddlCustomer").css('background-color', 'white');  
    
});

function OnEditClick(editButton) {
    var ProcessPlanCD = $.trim(editButton.closest("tr").attr("data-lblProcessPlanCD"));
    // location.href = '/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=' + ProcessPlanCD;
    window.open("/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=" + ProcessPlanCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1900 + ", height=" + 1490);
}

var element = document.getElementById("tabIndex");
element.addEventListener('click', function (e) {
    debugger;
    var targetValue = $("#" + e.target.id).data("title");
    if (targetValue != "" && targetValue != undefined) {
        $("#activeTabID").val(e.target.id);
        $('#' + targetValue).show();
        $ActiveID = targetValue;
    }    

    if ($ActiveID == "titleHeatTreatment") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);
       
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);       
        SetMachineTypeList($("#hdfMachineTypeHeatTreatment").val());
        if (heatTreatmentFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelFuranceOutDate").text());
        $("#lblOperator").text($("#labelFurnaceVerifyBy").text());
       
    }
    if ($ActiveID == "titleDrawingCoilToBar") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);
        
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);       
        SetCoilToBarMachineTypeList();
        if (drawingCoilToBarFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleStraightening1") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);       

        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);       
        SetMachineTypeList($("#hdfMachineTypeStraighteing").val());
        if (straighteing1Flag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleChamfer") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
       
        SetMachineTypeList($("#hdfMachineTypeChamfer").val());

        if (chamferFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleEC") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
      
        SetMachineTypeList($("#hdfMachineTypeEc").val());
        if (ecFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleStampping") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false);
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
       
        SetMachineTypeList($("#hdfMachineTypeStampping").val());
        if (stamppingFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleQATestHardness") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
        SetMachineTypeList($("#hdfMachineTypeHeatTreatment").val());
        //SetMachineList();
        if (postHTFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelInspectionDate").text());
        $("#lblOperator").text($("#labelInspector").text());
    }
    if ($ActiveID == "titleMechHardness") {  
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', true); 
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').hide();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
        if (mechHardnessFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelInspectionDate").text());
        $("#lblOperator").text($("#labelInspector").text());
    }
    if ($ActiveID == "titleMechTensile") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val(""); 
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', true); 
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').hide();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
        if (mechTensileFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelInspectionDate").text());
        $("#lblOperator").text($("#labelInspector").text());
    }
    if ($ActiveID == "titleFinalInspection") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', true);
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').hide();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
        if (finalInspectionFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }

        $("#lbldate").text($("#labelInspectionDate").text());
        $("#lblOperator").text($("#labelInspector").text());
    }
    if ($ActiveID == "titleDrawingBarToBar") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false);
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
       
        SetMachineTypeList($("#hdfMachineTypeDrawingBarTobar").val());
        if (drawingBarToBarFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleWireDrawing") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false);
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);       
        SetMachineTypeList($("#hdfMachineTypeDrawingCoilToCoil").val());

        if (drawingCoilToCoilFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleStraightening2") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false);
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);        
        SetMachineTypeList($("#hdfMachineTypeStraighteing").val());
        if (straighteing2Flag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleHardnessTempering") {
        $("#lblLocation").hide();
        $("#lblHrdTempLocation").show();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false); 
        //$("#ddlProcess").prop('disabled', true);
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
        //SetMachineList();
        SetMachineTypeList($("#hdfmachineTypeHardnessTempering").val());
        if (hardnessTemperingFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelFrncInOutDtTmInspDt").text());
        $("#lblOperator").text($("#labelOperatorInspector").text());
    }
    if ($ActiveID == "titleGeneralDailyProcess") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', false);
        //$("#ddlProcess").prop('disabled', false);
        $('#tdProcess, #tdddlProcess').show();
        $('#tdMachine,#tdddlMachine').show();
        $('#tdOperator,#tdddlOperator').show();
        InitSortPopup($ActiveID);
        SetMachineList();
        setProcessKubun();
        if (generalProcessFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelOperationDayEnd").text());
        $("#lblOperator").text($("#labelEndOperator").text());
    }
    if ($ActiveID == "titleTransfer") {
        $("#lblLocation").show();
        $("#lblHrdTempLocation").hide();
        $("#ddlMachine").val("");
        $("#ddlProcess").val("");
        //$("#ddlMachine").prop('disabled', true);
        //$("#ddlProcess").prop('disabled', true);      
        $('#tdProcess, #tdddlProcess').hide();
        $('#tdMachine,#tdddlMachine').hide();
        $('#tdOperator,#tdddlOperator').hide();
        InitSortPopup($ActiveID);
      
        if (transferReportFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        $("#lbldate").text($("#labelDeliveryDateTransferDate").text());
    }

});

document.getElementById('ddlProcess').addEventListener('change', function () {
    debugger;
    const selectedProcessId = this.value;
   
    if (selectedProcessId == $("#hdfHeatTreatment").val()) {
        SetMachineTypeList($("#hdfMachineTypeHeatTreatment").val());
    }

    else if (selectedProcessId == $("#hdfDrawingBarToBar").val()) {
        SetMachineTypeList($("#hdfMachineTypeDrawingBarTobar").val());
    }

    else if (selectedProcessId == $("#hdfRollStraightening").val()) {
        SetMachineTypeList($("#hdfMachineTypeStraighteing").val());
    }

    else if (selectedProcessId == $("#hdfPressStraightening").val()) {
        SetMachineTypeList($("#hdfMachineTypeStraighteing").val());
    }

    else if (selectedProcessId == $("#hdfAcidWash").val()) {
        SetMachineTypeList($("#hdfMachineTypeAcidWash").val());
    }

    else if (selectedProcessId == $("#hdfAttaching").val()) {
        SetMachineTypeList($("#hdfMachineTypeAttaching").val());
    }

    else if (selectedProcessId == $("#hdfShotBlast").val()) {
        SetMachineTypeList($("#hdfMachineTypeShotBlast").val());
    }

    else if (selectedProcessId == $("#hdfDrawingCoilToBar").val()) {
       
        SetCoilToBarMachineTypeList();
    }

    else if (selectedProcessId == $("#hdfDrawingCoilToCoil").val()) {
        SetMachineTypeList($("#hdfMachineTypeDrawingCoilToCoil").val());
    }

    else if (selectedProcessId == $("#hdfChamfer").val()) {
        SetMachineTypeList($("#hdfMachineTypeChamfer").val());
    }

    else if (selectedProcessId == $("#hdfEc").val()) {
        SetMachineTypeList($("#hdfMachineTypeEc").val());
    }

    //else if (selectedProcessId == $("#hdfUt").val()) {
    //    SetMachineTypeList($("#hdfMachineTypeUt").val());
    //}

    else if (selectedProcessId == $("#hdfStamping").val()) {
        SetMachineTypeList($("#hdfMachineTypeStampping").val());
    }

    else if (selectedProcessId == $("#hdfCutting").val()) {
        SetMachineTypeList($("#hdfMachineTypeCutting").val());
    }

    else if (selectedProcessId == $("#hdfCuttingOpening").val()) {
        SetMachineTypeList($("#hdfMachineTypeCutting").val());
    }

    else if (selectedProcessId == $("#hdfCuttingTrimming").val()) {
        SetMachineTypeList($("#hdfMachineTypeCutting").val());
    }

    else if (selectedProcessId == $("#hdfRefining").val()) {
        SetMachineTypeList($("#hdfmachineTypeHardnessTempering").val());
    }

    else if (selectedProcessId == $("#hdfHardTemperingInspection").val()) {
        SetMachineTypeList($("#hdfmachineTypeHardnessTempering").val());
    }
    else if (selectedProcessId == $("#hdfInternalQualityInspection").val())
    {
        SetMachineTypeList($("#hdfMachineTypeHeatTreatment").val());
    }
    else if (selectedProcessId == $("#hdfotherProcess").val()) {
         SetMachineList();
    }
    else if (selectedProcessId == 0)
    {
        SetMachineList();
    }
    else {
        $("#ddlMachine").html("");
    }
    
});

function setProcessKubun() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetProcessKubunList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if ($("#hdfCultureName").val() == "en") {
                $.each(jsonResult, function (key, object) {

                    object.procesS_KUBUN_STRING = object.procesS_KUBUN_EN;
                });
            } else {
                $.each(jsonResult, function (key, object) {

                    object.procesS_KUBUN_STRING = object.procesS_KUBUN_JP;
                });
            }
            bindProcessKubunDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProcessDisplayConfig001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcessKubunDropdown(jsonResult) {
    $("#ddlProcess").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcess").append($('<option value="' + jsonObj.kubuN_ID + '"> ' + jsonObj.procesS_KUBUN_STRING + '</option>'));
    });
}
function setDecimalFormats() {
    $("#txtDiaMin,#txtDiaMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthMin,#txtLengthMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

}

function Mask() {
   // $("#txtInspDate1").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    //  $("#txtInspDate2").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtInspDate1").mask("9999/99/99 99:99", {
        placeholder: "YYYY/MM/DD HH:mm"
    });

    $("#txtInspDate2").mask("9999/99/99 99:99", {
        placeholder: "YYYY/MM/DD HH:mm"
    });
}

$(".csvDisable").change(function () {
    debugger;
    $("#btnExportCSV").attr("DISABLED", "DISABLED");
});

function SetCoilToBarMachineTypeList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetHikinukiCoilToBarMachineTypeList",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetMachineTypeList(machineTypeCd) {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetMachineTypeList?machineType=" + machineTypeCd,
        method: "POST",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetMachineList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetMachineList",
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
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMachineDropdown(jsonResult) {
    $("#ddlMachine").html("");
    $("#ddlMachine").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMachine").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        
    });
}

function setLocationList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetFactoryLocationNameList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindLocationDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindLocationDropdown(jsonResult) {
    $("#ddlLocation").html("");
    $("#ddlLocation").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlLocation").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        //$("#ddlSrchMachine").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

//$("#btnYobinaCustomerSearch").click(function (event) {

//    var yobina = $.trim($("#txtYobinaCustomerSearch").val());
//    setCustomerNameList(yobina, "");
//});
//function setCustomerNameList(yobina, defaultSelectID) {
//    setCustomerList(yobina, $("#ddlCustomer"), defaultSelectID);
//};
//function setCustomerList(yobina, $target, defaultSelectID) {    

//    var ajaxTime = new Date().getTime();
//    var jsonData = "{yobina:'" + yobina + "' , USER_NAME: '" + userName + "' }";
//    var settings = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/HeatTreatment/CustomerList?yobina=" + yobina,
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        data: "{jsonData : " + jsonData + "}"
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            // var jsonResult = $.parseJSON(response.d);

//            bindCustomerDropdown(response.message, $target, defaultSelectID);
//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}


function formatStartEndDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear(),
        hour = '' + d.getHours(),
        minute = '' + d.getMinutes();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;
    if (hour.length < 2)
        hour = '0' + hour;
    if (minute.length < 2)
        minute = '0' + minute;

    return [year, month, day].join('/') + ' ' + hour + ':' + minute;
}

//function bindCustomerDropdown(jsonResult, $target, defaultSelectID) {
//    $("#ddlCustomer").html("");
//    $("#ddlCustomer").html("").append($('<option value="0"></option>'));
  
//    $.each(jsonResult, function (key, jsonObj) {
//        //$("#ddlCustomer").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
//        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '">' + jsonObj.name + '</option>'));
//    });
//    if (defaultSelectID != null && defaultSelectID != "") {
//        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
//        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
//    }
//}



var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
$("#btnSearch").click(function () {
    if (!IsMandatoryDateSearchFieldsFilled()) {
        return false;
    }
    debugger;
    var startDate = document.getElementById("txtInspDate1").value;
    var endDate = document.getElementById("txtInspDate2").value;
       

    if (!validateDateFormat(startDate) && !validateDateFormat(endDate)) {
        var message1 = $("#valMsgProcessOrInspStartDateInvalid").text().trim();
        var message2 = $("#valMsgProcessOrInspEndDateInvalid").text().trim();
        var invaliMessage = message1 + "<br>" + message2;
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(invaliMessage, focusFieldID, WARNING);
        return false;
    }

    else if (!validateDateFormat(startDate)) {
        var message = $("#valMsgProcessOrInspStartDateInvalid").text().trim();
        showMessageDialog(message, "txtInspDate1", WARNING);
        $("#txtInspDate1").focus;
        return false;
    }

    else if (!validateDateFormat(endDate)) {
        var message = $("#valMsgProcessOrInspEndDateInvalid").text().trim();
        showMessageDialog(message, "txtInspDate2", WARNING);
        $("#txtInspDate2").focus;
        return false;
    }

    var inspStartDate = formatStartEndDate(startDate);
    var inspEndDate = formatStartEndDate(endDate);

    if (inspEndDate < inspStartDate) {
        var message = $("#valMsgendDateValidate").text().trim();
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(message, "txtInspDate1", WARNING);
        return false;
    }

    var rowLimit = $("#rowLimitValue").val();
    var maxLimit = $("#maxLimit").val();
    if (parseInt(rowLimit) > parseInt(maxLimit)) {
        var message = $("#valMsgDataRowsMaxLimitExceed").text().trim();
        focusFieldID = 'rowLimitValue';
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }    
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

    if ($ActiveID == "titleHeatTreatment") {

        GetRowCountHeatTreatment(selectedColumns);
       
        //GetRowCountHeatTreatment(null)      
       
    }
    if ($ActiveID == "titleDrawingCoilToBar") {
       
        GetRowCountCombinedDailyReport(selectedColumns)
    }
    if ($ActiveID == "titleStraightening1") {

        GetRowCountStraightening(selectedColumns)
    }
    if ($ActiveID == "titleChamfer") {

        GetRowCountChamffer(selectedColumns)
    }
    if ($ActiveID == "titleEC") {

        GetRowCountEC(selectedColumns)

    }
    if ($ActiveID == "titleStampping") {

        GetRowCountStampping(selectedColumns)
    }
    if ($ActiveID == "titleQATestHardness") {

        GetRowCountQATestHardness(selectedColumns)
    }
    if ($ActiveID == "titleMechHardness") {
        GetRowCountMechHardness(selectedColumns)
    }
    if ($ActiveID == "titleMechTensile") {
        GetRowCountMECHTensile(selectedColumns)
    }
    if ($ActiveID == "titleFinalInspection") {
        GetRowCountFinalInspection(selectedColumns)
    }
    if ($ActiveID == "titleDrawingBarToBar") {

        GetRowCountDrawingBarToBar(selectedColumns)
       
    }
    if ($ActiveID == "titleWireDrawing") {

        GetRowCountWireDrawingReport(selectedColumns)
    }
    if ($ActiveID == "titleStraightening2") {
        
        GetRowCountStraightening2(selectedColumns)
    }
    if ($ActiveID == "titleHardnessTempering") {
        GetRowCountHardnessTempering(selectedColumns)
    }
    if ($ActiveID == "titleGeneralDailyProcess") {
        GetRowCountGeneralProcess(selectedColumns)
    }
    if ($ActiveID == "titleTransfer") {
        GetRowCountTransferReport(selectedColumns)
    }

}).ajaxStart(function () {

    $("#LoadingImage").show();
}).ajaxStop(function () {
    $("#LoadingImage").hide();
}).ajaxError(function () {
    $("#LoadingImage").hide();
});

$(function () {
    $(document).ajaxStart(function () {

        $("#LoadingImage").show();
    });

    $(document).ajaxStop(function () {
        $("#LoadingImage").hide();
    });

    $(document).ajaxError(function () {
        $("#LoadingImage").hide();
    });

});


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



function GetHeatTreatmentDetails(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;  
    var culture_name = $("#hdfCultureName").val();
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetHeatTreatmentDetailsOnFactoryDailyWOrkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: culture_name, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
            if (response.isSuccess == true) {
               
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                heatTreatmentFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {
                    
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {    
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                heatTreatmentFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/HeatTreatmentGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",               
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {
                    $("#divHeatTreatmentGridMain").html(response1.message);                   
                   
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#errorMsgCOMSystemException").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            
          
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetCombinedDailyReportDetails(sortFields) {
    // Combined Daily Report api-------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val();

    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetCombinedDailyReportData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, CULTURE_NAME: cultureName,SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                drawingCoilToBarFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {
                   
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                drawingCoilToBarFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/CombinedDailyReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divCombineDailyReportGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetStraighteningDailyReportDetails(sortFields) {
    // StraighteningDailyReportGrid--------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val();
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetStraighteningDailyReportOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                straighteing1Flag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.nexT_PROCESS_EN;

                    });
                } else {

                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.nexT_PROCESS_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                straighteing1Flag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/StraighteningDailyReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {


                    $("#divStraighteningDailyReportGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //drop down bind
        });
    closepopup();
}

function GetChamfferDailyReportDetails(sortFields) {

    //Chamffer Daily Report-----------------------------------------------------------------------------------------   

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;  
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetChamferDailyReportOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                chamferFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.nexT_PROCESS_EN;

                    });
                } else {

                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.nexT_PROCESS_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                chamferFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/ChamferDailyReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divChamferDailyReportGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetECDailyReportDetails(sortFields) {

    ////GetECDailyReportOnFactoryDailyWorkStatus-----------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetECDailyReportOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                ecFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.eC_NG_DETAILS1_STRING = object.eC_NG_DETAILS1_EN;
                        object.eC_NG_DETAILS2_STRING = object.eC_NG_DETAILS2_EN;
                        object.eC_NG_DETAILS3_STRING = object.eC_NG_DETAILS3_EN;

                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.eC_NG_DETAILS1_STRING = object.eC_NG_DETAILS1_JP;
                        object.eC_NG_DETAILS2_STRING = object.eC_NG_DETAILS2_JP;
                        object.eC_NG_DETAILS3_STRING = object.eC_NG_DETAILS3_JP;
                    });
                }
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {

                    $.each(jsonResult, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                ecFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/ECDailyReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {


                    $("#divECDailyReportGridMain").html(response1.message);
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    closepopup();
}

function GetStamppingDailyReportDetails(sortFields) {

    ////Stampping Daily Report--------------------------------------------------------------------------------------------------
    debugger;
    var ajaxTime = new Date().getTime();
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val();
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetStamppingDailyReportOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                stamppingFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {

                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                stamppingFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/StamppingDailyReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divStamppingDailyReportGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetQATestHardnessDetails(sortFields) {


    //QA Test Hardness--------------------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHTInspection/GetQATestHardnessOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                postHTFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                postHTFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/QATestHardnessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;

                    $("#divQATestHardnessGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetMechHardnessDetails(sortFields) {

    //mech hardness--------------------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetMechHardnessOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            /*MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                mechHardnessFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                mechHardnessFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/MechHardnessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {


                    $("#divMechHardnessGridMain").html(response1.message);
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetMECHTensileDetails(sortFields) {

    ////MECH Tensile--------------------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetMECHTensileOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
           /* MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                mechTensileFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                mechTensileFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/MechTensileGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divMechTensileGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetFinalInspectionDetails(sortFields) {

    //Final Inspection--------------------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetFinalInspectionDataOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            /*MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                finalInspectionFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                finalInspectionFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/FinalIGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {


                    $("#divFinalIGridMain").html(response1.message);
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetWireDrawingReportDetails(sortFields) {

    //Wire Drawing Report--------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetWireDrawingDailyReportOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                drawingCoilToCoilFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {

                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                drawingCoilToCoilFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/WireDrawingReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divWireDrawingGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetDrawingBarToBarReportDetails(sortFields) {

    //DrawingBarToBar Report--------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var cultureName = $("#hdfCultureName").val();
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetDrawingBarToBarDataOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                drawingBarToBarFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {

                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                drawingBarToBarFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/DrawingBarToBarGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divDrawingBarToBarGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetStraightening2ReportDetails(sortFields) {

    //straightening2 Report--------------------------------------------------------------------------------------

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val();

    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetStraightning2DailyReportDataOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                straighteing2Flag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_EN;

                    });
                } else {

                    $.each(result, function (key, object) {
                        object.nexT_PROCESS_NAME = object.procesS_KUBUN_JP;

                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                straighteing2Flag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/Straightening2Grid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divStraightening2GridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetHardnessTemperingReportDetails(sortFields) {

    //Hardness Tempering Report
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardnessTempringOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                hardnessTemperingFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                hardnessTemperingFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/HardnessTemperingGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divHardnessTemperingGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
   
    closepopup();
}


function GetGeneralProcessDetails(sortFields) {       

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;  
    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetGeneralProcessOnFactoryDailyWorkStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, PROCESS_CD: process_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                generalProcessFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.procesS_KUBUN_STRING = object.procesS_KUBUN_EN; 
                        object.gP_NEXT_PROCESS_NAME = object.nexT_PROCESS_EN

                        object.proC_DEFECT_STRING1 = object.proC_DEFECT1_EN;  
                        object.proC_DEFECT_STRING2 = object.proC_DEFECT2_EN;  
                        object.proC_DEFECT_STRING3 = object.proC_DEFECT3_EN;  
                        

                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.procesS_KUBUN_STRING = object.procesS_KUBUN_JP;  
                        object.gP_NEXT_PROCESS_NAME = object.nexT_PROCESS_JP

                        object.proC_DEFECT_STRING1 = object.proC_DEFECT1_JP;
                        object.proC_DEFECT_STRING2 = object.proC_DEFECT2_JP;
                        object.proC_DEFECT_STRING3 = object.proC_DEFECT3_JP; 
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                generalProcessFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/GeneralProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divGeneralProcessGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetTransferDetails(sortFields) {

    var ajaxTime = new Date().getTime();
    /*var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;*/
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;

    var rowLimit = $("#rowLimitValue").val();
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensou/GetTransferDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            /*MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, PROCESS_CD: process_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                transferReportFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.nexT_PROCESS_NAME = object.nexT_PROCESS_EN;

                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.nexT_PROCESS_NAME = object.nexT_PROCESS_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                transferReportFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/FactoryDailyWorkStatusSearchProcessWise/TransferReportGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#divTransferReportGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}


$("#btnInitialize").click(function () {
    debugger;    
   // $ActiveID = "";
    userName = "";
    DISABLED = "disabled";
    typeDropDown = "select-one";
    typeText = "text";
    rowLimitValue = "";

    $("#txtChuuban").focus();
    $("#ddlProcess").val("");
    $("#ddlMachine").val("");
    $("#btnExportCSV").attr("DISABLED", "DISABLED");
    $('#rowLimitValue').val('200');
    removeTabClass();
    heatTreatmentFlag = 0;
    drawingBarToBarFlag = 0;
    drawingCoilToBarFlag = 0;
    drawingCoilToCoilFlag = 0;
    straighteing1Flag = 0;
    straighteing2Flag = 0;
    chamferFlag = 0;
    ecFlag = 0;
    stamppingFlag = 0;
    hardnessTemperingFlag = 0;
    postHTFlag = 0;
    mechHardnessFlag = 0;
    mechTensileFlag = 0;
    finalInspectionFlag = 0;
    generalProcessFlag = 0;
    transferReportFlag = 0;
    var list = [];
    $("#lbldate").text($("#labelFuranceOutDate").text());
    $("#lblOperator").text($("#labelFurnaceVerifyBy").text());
    $('#tdProcess, #tdddlProcess').hide();
    $('#tdMachine,#tdddlMachine').show();
    $('#tdOperator,#tdddlOperator').show();
    clearCommonSearchFields();

    //if ($ActiveID == "titleHeatTreatment") {
       
        clearTable($("#tblHeatTreatmentReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/HeatTreatmentGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divHeatTreatmentGridMain").html(response.message);
            })
      

   // }
    //if ($ActiveID == "titleDrawingCoilToBar") {
        
        clearTable($("#tblCombineReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/CombinedDailyReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divCombineDailyReportGridMain").html(response.message);
            })

   // }
    //if ($ActiveID == "titleStraightening1") {
       
        clearTable($("#tblCorrectionReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/StraighteningDailyReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divStraighteningDailyReportGridMain").html(response.message);
            })
   // }
    //if ($ActiveID == "titleChamfer") {
        
        clearTable($("#tblChamferReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/ChamferDailyReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divChamferDailyReportGridMain").html(response.message);
            })
    //}
   // if ($ActiveID == "titleEC") {
        
        clearTable($("#tblECReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/ECDailyReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {


                $("#divECDailyReportGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleStampping") {
        
        clearTable($("#tblStamppingReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/StamppingDailyReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divStamppingDailyReportGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleQATestHardness") {
       
        clearTable($("#tblQAHardnessReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/QATestHardnessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divQATestHardnessGridMain").html(response.message);
            })
   // }
  //  if ($ActiveID == "titleMechHardness") {
       
        clearTable($("#tblMechanicalHardnessReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/MechHardnessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divMechHardnessGridMain").html(response.message);
            })

   // }
   // if ($ActiveID == "titleMechTensile") {
        
        clearTable($("#tblMechanicalTensileReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/MechTensileGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divMechTensileGridMain").html(response.message);
            })
  //  }
   // if ($ActiveID == "titleFinalInspection") {
       
        clearTable($("#tblFinalInspectionReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/FinalIGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divFinalIGridMain").html(response.message);
            })
    //}
   // if ($ActiveID == "titleDrawingBarToBar") {
        
        clearTable($("#tblTbPMSShimoguchiHitorireport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/DrawingBarToBarGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divDrawingBarToBarGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleWireDrawing") {
        
        clearTable($("#tblTbPMSWireDrawingreport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/WireDrawingReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divWireDrawingGridMain").html(response.message);
            })
  //  }
   // if ($ActiveID == "titleStraightening2") {
        
        clearTable($("#tblTbPMSCorrectionDailyReport2"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/Straightening2Grid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {

                $("#divStraightening2GridMain").html(response.message);
            })
  //  }
   // if ($ActiveID == "titleHardnessTempering") {
        
        clearTable($("#tblTbPMSConditioningreport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/HardnessTemperingGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divHardnessTemperingGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleGeneralDailyProcess") {
        
        clearTable($("#tblGeneralProcess"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/GeneralProcessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divGeneralProcessGridMain").html(response.message); 
            })
    // }
    clearTable($("#tblTransferReport"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/FactoryDailyWorkStatusSearchProcessWise/TransferReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divTransferReportGridMain").html(response.message); 
            })
});
function clearTable($table, trId, trClass) {
    debugger;
    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }

}
//function formatDate(d) {
//    function pad(n) { return n < 10 ? '0' + n : n }
//    return d.getUTCFullYear() + '/' + pad(d.getMonth() + 1) + '/' + pad(d.getDate())
//}
function pad(n) {
    return n < 10 ? '0' + n : n;
}

function formatDateTime(d) {
    return d.getFullYear() + '/' +
        pad(d.getMonth() + 1) + '/' +
        pad(d.getDate()) + ' ' +
        pad(d.getHours()) + ':' +
        pad(d.getMinutes());
}

function clearCommonSearchFields() {

    //setMachineList();
    // SetHeatTreatmentMachineList();
    SetMachineTypeList($("#hdfMachineTypeHeatTreatment").val());
    setLocationList();
    //setCustomerList("", $("#ddlCustomer"), "");
    $('#txtProdName').val('');
    $('#txtInternalName').val('');
    $('#txtDiaMin').val('');
    $('#txtDiaMax').val('');
    $('#txtShape').val('');
    $('#txtLengthMin').val('');
    $('#txtLengthMax').val('');
    $('#txtCharge').val('');
    $('#txtChuuban').val('');
    $('#txtGoubanCom').val('');
    $('#txtProdSpecNo1').val('');
    $('#txtProdSpecNo2').val('');
    $('#txtYobinaCustomerSearch').val('');
    $('#txtYobinaOperatorSearch').val('');    
    document.getElementById("ddlOperator").innerHTML = null;
    document.getElementById("ddlCustomer").innerHTML = null;
    var fromDate = new Date();    
    
    fromDate.setDate(fromDate.getDate() - 1);
    fromDate.setHours(8, 0, 0, 0);

    var toDate = new Date();
    toDate.setHours(7, 59, 0, 0);
    $('#txtInspDate1').val(formatDateTime(fromDate));
    $('#txtInspDate2').val(formatDateTime(toDate));

}

function InitSortPopup(activeTabID) {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/FactoryDailyWorkStatusSearchProcessWise/InitializeSortPopup?activeTabID=" + activeTabID,
        method: "Post",
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
$(document).on('click', '#btnSort', function () {
    debugger;

    if ($ActiveID == "titleHeatTreatment") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleDrawingCoilToBar") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleStraightening1") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleChamfer") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleEC") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleStampping") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleQATestHardness") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleMechHardness") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleMechTensile") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleFinalInspection") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleDrawingBarToBar") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleWireDrawing") {        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleStraightening2") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleHardnessTempering") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleGeneralDailyProcess") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleTransfer") {
        $("#SortPopUpModal").modal('show');
    }
})

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

function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortClose_Click() {

    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
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
    //else {
    //    alert("Please select the sort field to be added.");
    //}
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
    //else {
    //    alert("Please select the sort field to be added.");
    //}
    EnableDisableSortButtons();
}
function btnSortOk_Click() {

    if (!IsMandatoryDateSearchFieldsFilled()) {
        return false;
    }

    var startDate = document.getElementById("txtInspDate1").value;
    var endDate = document.getElementById("txtInspDate2").value;   
   
    if (!validateDateFormat(startDate) && !validateDateFormat(endDate)) {
        var message1 = $("#valMsgProcessOrInspStartDateInvalid").text().trim();
        var message2 = $("#valMsgProcessOrInspEndDateInvalid").text().trim();
        var invaliMessage = message1 + "<br>" + message2;
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(invaliMessage, focusFieldID, WARNING);
        return false;
    }

    else if (!validateDateFormat(startDate)) {
        var message = $("#valMsgProcessOrInspStartDateInvalid").text().trim();
        showMessageDialog(message, "txtInspDate1", WARNING);
        $("#txtInspDate1").focus;
        return false;
    }

    else if (!validateDateFormat(endDate)) {
        var message = $("#valMsgProcessOrInspEndDateInvalid").text().trim();
        showMessageDialog(message, "txtInspDate2", WARNING);
        $("#txtInspDate2").focus;
        return false;
    }


    var inspStartDate = formatStartEndDate(startDate);
    var inspEndDate = formatStartEndDate(endDate);
    if (inspEndDate < inspStartDate) {
        var message = $("#valMsgendDateValidate").text().trim();
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(message, "txtInspDate1", WARNING);
        return false;
    }

    var rowLimit = $("#rowLimitValue").val();
    var maxLimit = $("#maxLimit").val();
    if (parseInt(rowLimit) > parseInt(maxLimit)) {
        var message = $("#valMsgDataRowsMaxLimitExceed").text().trim();
        focusFieldID = 'rowLimitValue';
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

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
    if ($ActiveID == "titleHeatTreatment") {
        
        GetRowCountHeatTreatment(selectedColumns);
        
    }
    if ($ActiveID == "titleDrawingCoilToBar") {
        GetRowCountCombinedDailyReport(selectedColumns);
    }
    if ($ActiveID == "titleStraightening1") {
        GetRowCountStraightening(selectedColumns);
    }
    if ($ActiveID == "titleChamfer") {
        GetRowCountChamffer(selectedColumns);
    }
    if ($ActiveID == "titleEC") {
        GetRowCountEC(selectedColumns);
    }
    if ($ActiveID == "titleStampping") {
        GetRowCountStampping(selectedColumns);
    }
    if ($ActiveID == "titleQATestHardness") {
        GetRowCountQATestHardness(selectedColumns);
    }
    if ($ActiveID == "titleMechHardness") {
        GetRowCountMechHardness(selectedColumns);
    }
    if ($ActiveID == "titleMechTensile") {
        GetRowCountMECHTensile(selectedColumns);
    }
    if ($ActiveID == "titleFinalInspection") {
        GetRowCountFinalInspection(selectedColumns);
    }
    if ($ActiveID == "titleDrawingBarToBar") {
        GetRowCountDrawingBarToBar(selectedColumns);
    }
    if ($ActiveID == "titleWireDrawing") {
        GetRowCountWireDrawingReport(selectedColumns);
    }
    if ($ActiveID == "titleStraightening2") {
        GetRowCountStraightening2(selectedColumns);
    }
    if ($ActiveID == "titleHardnessTempering") {
        GetRowCountHardnessTempering(selectedColumns);
    }
    if ($ActiveID == "titleGeneralDailyProcess") {
        GetRowCountGeneralProcess(selectedColumns);
    }
    if ($ActiveID == "titleTransfer") {
        GetRowCountTransferReport(selectedColumns);
    }

}

function btnSortReset_Click() {
    if ($ActiveID == "titleHeatTreatment") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleDrawingCoilToBar") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleStraightening1") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleChamfer") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleEC") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleStampping") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleQATestHardness") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleMechHardness") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleMechTensile") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleFinalInspection") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleDrawingBarToBar") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleWireDrawing") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleStraightening2") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleHardnessTempering") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleGeneralDailyProcess") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titleTransfer") {
        InitSortPopup($ActiveID);

    }

}
$(document).on('click', '#btnExportCSV', function () {
    debugger;
    var ajaxTime = new Date().getTime();
    var sortFilter = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            sortFilter.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });
    if ($ActiveID == "titleHeatTreatment") {
        downloadHeatTreatmentCSV(sortFilter);
    }
    if ($ActiveID == "titleDrawingCoilToBar") {
        downloadCombinedDailyReportCSV(sortFilter);

    }
    if ($ActiveID == "titleStraightening1") {
        downloadStraighteningDailyReportCSV(sortFilter);

    }
    if ($ActiveID == "titleChamfer") {
        downloadChamferDailyReportCSV(sortFilter);

    }
    if ($ActiveID == "titleEC") {
        downloadECDailyReportCSV(sortFilter);

    }
    if ($ActiveID == "titleStampping") {
        downloadStampingDailyReportCSV(sortFilter)

    }
    if ($ActiveID == "titleQATestHardness") {
        downloadQATestHardnessReportCSV(sortFilter);

    }
    if ($ActiveID == "titleMechHardness") {
        downloadMechHardnessCSV(sortFilter);

    }
    if ($ActiveID == "titleMechTensile") {
        downloadMechTensileReportCSV(sortFilter);

    }
    if ($ActiveID == "titleFinalInspection") {
        downloadFinalInspectionReportCSV(sortFilter);

    }
    if ($ActiveID == "titleDrawingBarToBar") {
        downloadDrawingBarToBarCSV(sortFilter);

    }
    if ($ActiveID == "titleWireDrawing") {
        downloadWireDrawingDailyReportCSV(sortFilter);

    }
    if ($ActiveID == "titleStraightening2") {
        downloadStraightening2DailyReportCSV(sortFilter);

    }
    if ($ActiveID == "titleHardnessTempering") {
        downloadHardnessTemperingCSV(sortFilter);

    }
    if ($ActiveID == "titleGeneralDailyProcess") {
        downloadGeneralDailyProcessReportCSV(sortFilter);

    }
    if ($ActiveID == "titleTransfer") {
        downloadTransferReportCSV(sortFilter);

    }

});
function downloadHeatTreatmentCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;  
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportHeatTreatmentCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadCombinedDailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportCombinedDailyReportCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadStraighteningDailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportStraighteningCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD,CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
   
};
function downloadChamferDailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportChamfferCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadECDailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportECCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadStampingDailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportStamppingCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadQATestHardnessReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportQATestHardnessCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadMechTensileReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportMechTensileCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
           /* MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadMechHardnessCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportMechHardnessCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
             /*MACHINE_CD: machine_CD,*/LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function downloadFinalInspectionReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    //var jsonData = "{USER_NAME: '" + userName + "' }";
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportFinalInspectionCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
           /* MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2,
            END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadWireDrawingDailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();  
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportColdDrawingWireDrawingCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadDrawingBarToBarCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportDrawingBarToBarCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })

    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


}
function downloadStraightening2DailyReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportStraightening2CSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
function downloadHardnessTemperingCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/FactoryDailyWorkStatusSearchProcessWise/ExportHardnessTemperingCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function downloadGeneralDailyProcessReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportGeneralProcessCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, PROCESS_CD: process_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function downloadTransferReportCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/FactoryDailyWorkStatusSearchProcessWise/ExportTransferCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, PROCESS_CD: process_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function IsMandatoryDateSearchFieldsFilled() {
    debugger;
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";
    if ($("#txtDiaMin").val().trim() == "" && $("#txtDiaMax").val().trim() != "") {
        message = $.trim($("#lblErrorMessageE_DimDStartRequired").text());
        focusFieldID = 'txtDiaMin'
        returnVal = false;
    }

    if ($("#txtLengthMin").val().trim() == "" && $("#txtLengthMax").val().trim() != "") {
        if (focusFieldID == "") {
            focusFieldID = 'txtLengthMin';
            returnVal = false;
        }
        if (message == "") {
            message = $.trim($("#lblErrorMessageE_LengthStartRequired").text());
            returnVal = false;
        } else {
            message = message + "<br>" + $.trim($("#lblErrorMessageE_LengthStartRequired").text());
            returnVal = false;
        }
    }
    if ($("#rowLimitValue").val().trim() == "") {
        message = $.trim($("#valMsgMaxRowRequired").text());
        focusFieldID = 'rowLimitValue'
        returnVal = false;
    }
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
//function validateDateFormat(dateVal) {
//    if (dateVal == null)
//        return false;
//    //var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;
//    var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})\s(\d{1,2}):(\d{2})$/;
//    dateValues = dateVal.match(validatePattern);

//    if (dateValues == null)
//        return false;
//    debugger;
//    var dtYear = dateValues[1];
//    dtMonth = dateValues[3];
//    dtDay = dateValues[5];

//    if (dtMonth < 1 || dtMonth > 12)
//        return false;
//    else if (dtDay < 1 || dtDay > 31)
//        return false;
//    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
//        return false;
//    else if (dtMonth == 2) {
//        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
//        if (dtDay > 29 || (dtDay == 29 && !isleap))
//            return false;
//    }

//    return true;
//}

function validateDateFormat(dateVal) {
    if (dateVal == null)
        return false;

    // Format: yyyy/MM/dd HH:mm
    var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})\s(\d{1,2}):(\d{2})$/;

    var dateValues = dateVal.match(validatePattern);

    if (dateValues == null)
        return false;

    var dtYear = parseInt(dateValues[1], 10);
    var dtMonth = parseInt(dateValues[3], 10);
    var dtDay = parseInt(dateValues[5], 10);
    var dtHour = parseInt(dateValues[6], 10);
    var dtMinute = parseInt(dateValues[7], 10);

    // Date validation
    if (dtMonth < 1 || dtMonth > 12)
        return false;

    if (dtDay < 1 || dtDay > 31)
        return false;

    if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay > 30)
        return false;

    if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }

    // Hour validation (00-23)
    if (dtHour < 0 || dtHour > 23)
        return false;

    // Minute validation (00-59)
    if (dtMinute < 0 || dtMinute > 59)
        return false;

    return true;
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

//OnInitialize set default tab and grid as HeatTreatment 
function removeTabClass()
{
    var heatTreatmentTabPanel = document.getElementById("tabHeatTreatment");    
    heatTreatmentTabPanel.classList.add("active");
    var heatTreatmentGridPanel = document.getElementById("divFactoryHeatTreatment");
    heatTreatmentGridPanel.classList.add("active");
    heatTreatmentGridPanel.classList.add("show");


    $("#activeTabID").val("tabHeatTreatment");
    var targetValue = "titleHeatTreatment"
    $ActiveID = targetValue;
    if ($ActiveID == "titleHeatTreatment") {
        //$("#ddlMachine").prop('disabled', false); 
        $('#tdMachine').show();
        $('#tdddlMachine').show();
        InitSortPopup($ActiveID);
    }

    

    var drawingBarToBarTabPanel = document.getElementById("tabDrawingBarToBar");
    drawingBarToBarTabPanel.classList.remove("active");
    var drawingBarToBarGridPanel = document.getElementById("divFactoryDrawingBarToBar");
    drawingBarToBarGridPanel.classList.remove("active");
    drawingBarToBarGridPanel.classList.remove("show");

    var drawingCoilToBarTabPanel = document.getElementById("tabDrawingCoilToBar");
    drawingCoilToBarTabPanel.classList.remove("active");
    var drawingCoilToBarGridPanel = document.getElementById("divFactoryDrawingCoilToBar");
    drawingCoilToBarGridPanel.classList.remove("active");
    drawingCoilToBarGridPanel.classList.remove("show");


    var wireDrawingTabPanel = document.getElementById("tabWireDrawing");
    wireDrawingTabPanel.classList.remove("active");
    var wireDrawingGirdPanel = document.getElementById("divFactoryWireDrawing");
    wireDrawingGirdPanel.classList.remove("active");
    wireDrawingGirdPanel.classList.remove("show");

    var straightening1TabPanel = document.getElementById("tabStraightening1");
    straightening1TabPanel.classList.remove("active");
    var straightening1GirdPanel = document.getElementById("divStraightening1");
    straightening1GirdPanel.classList.remove("active");
    straightening1GirdPanel.classList.remove("show");

    var straightening2TabPanel = document.getElementById("tabStraightening2");
    straightening2TabPanel.classList.remove("active");
    var straightening2GridPanel = document.getElementById("divStraightening2");
    straightening2GridPanel.classList.remove("active");
    straightening2GridPanel.classList.remove("show");

    var chamferTabPanel = document.getElementById("tabChamfer");
    chamferTabPanel.classList.remove("active");
    var chamferGirdPanel = document.getElementById("divChamfer");
    chamferGirdPanel.classList.remove("active");
    chamferGirdPanel.classList.remove("show");

    var ecTabPanel = document.getElementById("tabEC");
    ecTabPanel.classList.remove("active");
    var eCGirdPanel = document.getElementById("divEC");
    eCGirdPanel.classList.remove("active");
    eCGirdPanel.classList.remove("show");


    var stamppingTabPanel = document.getElementById("tabStampping");
    stamppingTabPanel.classList.remove("active");
    var stamppingGridPanel = document.getElementById("divStampping");
    stamppingGridPanel.classList.remove("active");
    stamppingGridPanel.classList.remove("show");


    var hardnessTemperingTabPanel = document.getElementById("tabHardnessTempering");
    hardnessTemperingTabPanel.classList.remove("active");
    var hardnessTemperingGirdPanel = document.getElementById("divHardnessTempering");
    hardnessTemperingGirdPanel.classList.remove("active");
    hardnessTemperingGirdPanel.classList.remove("show");

    var qATestHardnessTabPanel = document.getElementById("tabQATestHardness");
    qATestHardnessTabPanel.classList.remove("active");
    var qATestHardnessGirdPanel = document.getElementById("divQATestHardness");
    qATestHardnessGirdPanel.classList.remove("active");
    qATestHardnessGirdPanel.classList.remove("show");

    var mechHardnessTabPanel = document.getElementById("tabMechHardness");
    mechHardnessTabPanel.classList.remove("active");
    var mechHardnessGirdPanel = document.getElementById("divMechHardness");
    mechHardnessGirdPanel.classList.remove("active");
    mechHardnessGirdPanel.classList.remove("show");

    var mechTensileTabPanel = document.getElementById("tabMechTensile");
    mechTensileTabPanel.classList.remove("active");
    var mechTensileGridPanel = document.getElementById("divMechTensile");
    mechTensileGridPanel.classList.remove("active");
    mechTensileGridPanel.classList.remove("show");

    var finalInspectionTabPanel = document.getElementById("tabFinalInspection");
    finalInspectionTabPanel.classList.remove("active");
    var finalInspectionGridPanel = document.getElementById("divFinalInspection");
    finalInspectionGridPanel.classList.remove("active");
    finalInspectionGridPanel.classList.remove("show");

    var generalDailyProcessTabPanel = document.getElementById("tabGeneralDailyProcess");
    generalDailyProcessTabPanel.classList.remove("active");
    var generalDailyProcessGirdPanel = document.getElementById("divGeneralDailyProcess");
    generalDailyProcessGirdPanel.classList.remove("active");
    generalDailyProcessGirdPanel.classList.remove("show");

    var transferReportTabPanel = document.getElementById("tabTransfer");
    transferReportTabPanel.classList.remove("active");
    var generalDailyProcessGirdPanel = document.getElementById("divTransferReport");
    generalDailyProcessGirdPanel.classList.remove("active");
    generalDailyProcessGirdPanel.classList.remove("show");
    
}

function GetRowCountHeatTreatment(selectedColumns) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;    

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetRowCountForHeatTreatmentFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);
                
                if (parseInt(rowLimit) > 0) {
                    GetHeatTreatmentDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblHeatTreatmentReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/HeatTreatmentGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divHeatTreatmentGridMain").html(response.message);
                        })

                }
                
            }
            else {
                GetHeatTreatmentDetails(selectedColumns);
            }                    


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountDrawingBarToBar(selectedColumns) {
    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;    
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;  
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetRowCountForHikinukiBarToBarFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetDrawingBarToBarReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblTbPMSShimoguchiHitorireport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/DrawingBarToBarGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divDrawingBarToBarGridMain").html(response.message);
                        })

                }
            }
            else {
                GetDrawingBarToBarReportDetails(selectedColumns);
            }   
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    
}

function GetRowCountCombinedDailyReport(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetRowCountForCombinedDailyReportFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetCombinedDailyReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblCombineReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/CombinedDailyReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divCombineDailyReportGridMain").html(response.message);
                        })
                }
            }
            else {
                GetCombinedDailyReportDetails(selectedColumns);
            }   
            
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    
}

function GetRowCountWireDrawingReport(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;    
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetRowCountForWireDrawingFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetWireDrawingReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblTbPMSWireDrawingreport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/WireDrawingReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divWireDrawingGridMain").html(response.message);
                        })

                }
            }
            else {
                GetWireDrawingReportDetails(selectedColumns);
            }   
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    
}

function GetRowCountStraightening(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetRowCountForStraighteningFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetStraighteningDailyReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblCorrectionReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/StraighteningDailyReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divStraighteningDailyReportGridMain").html(response.message);
                        })

                }
            }
            else {
                GetStraighteningDailyReportDetails(selectedColumns);
            }   
           
        })
    .fail(function (xhr, textStatus, errorThrown) {
        responseTime = new Date().getTime() - ajaxTime;
        var errorCode = $.parseJSON(xhr.responseText).Message;

        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#lblErrorMessage_system_exception").text());
        showMessageDialog(message, focusFieldID, ERROR);
    });
}

function GetRowCountStraightening2(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;  
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetRowCountForStraightening2Factory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
           
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetStraightening2ReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblTbPMSCorrectionDailyReport2"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/Straightening2Grid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {

                            $("#divStraightening2GridMain").html(response.message);
                        })

                }
            }
            else {
                GetStraightening2ReportDetails(selectedColumns);
            }  
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
   
}

function GetRowCountChamffer(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;    
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetRowCountForChamfferFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetChamfferDailyReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblChamferReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/ChamferDailyReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divChamferDailyReportGridMain").html(response.message);
                        })
                }
            }
            else {
                GetChamfferDailyReportDetails(selectedColumns);
            }             
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountEC(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetRowCountForECFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetECDailyReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblECReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/ECDailyReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {

                            $("#divECDailyReportGridMain").html(response.message);
                        })
                    
                }
            }
            else {
                GetECDailyReportDetails(selectedColumns);
            }   
           
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountStampping(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;   
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetRowCountForStamppingFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;           

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetStamppingDailyReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblStamppingReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/StamppingDailyReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divStamppingDailyReportGridMain").html(response.message);
                        })

                }
            }
            else {
                GetStamppingDailyReportDetails(selectedColumns);
            }   
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountHardnessTempering(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;    
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetRowCountForHardnessTemperingtFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })


    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetHardnessTemperingReportDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblTbPMSConditioningreport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/HardnessTemperingGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divHardnessTemperingGridMain").html(response.message);
                        })

                }
            }
            else {
                GetHardnessTemperingReportDetails(selectedColumns);
            }   
            
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function GetRowCountQATestHardness(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;    
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHTInspection/GetRowCountForQATestHardnessFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetQATestHardnessDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblQAHardnessReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/QATestHardnessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divQATestHardnessGridMain").html(response.message);
                        })

                }
            }
            else {
                GetQATestHardnessDetails(selectedColumns);
            }   
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountMechHardness(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetRowCountForMechHardnessFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            /*MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetMechHardnessDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblMechanicalHardnessReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/MechHardnessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divMechHardnessGridMain").html(response.message);
                        })

                }
            }
            else {
                GetMechHardnessDetails(selectedColumns);
            }   
           
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountMECHTensile(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetRowCountForMechTensileFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
           /* MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;            

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetMECHTensileDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblMechanicalTensileReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/MechTensileGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divMechTensileGridMain").html(response.message);
                        })

                }
            }
            else {
                GetMECHTensileDetails(selectedColumns);
            }  
           
           })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountFinalInspection(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;    
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetRowCountForFinalInspectionFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            /*MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2,
            END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetFinalInspectionDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblFinalInspectionReport"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/FinalIGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divFinalIGridMain").html(response.message);
                        })

                }
            }
            else {
                GetFinalInspectionDetails(selectedColumns);
            }  
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountGeneralProcess(selectedColumns) {

    var ajaxTime = new Date().getTime();
    var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
    var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
    var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
    var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
    var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;
    var operatorCD = $("#ddlOperator").val() ? $("#ddlOperator").val() : null; 
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetRowCountForGeneralDailyProcessFactory",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            MACHINE_CD: machine_CD, LOCATION_CD: location_CD, PROCESS_CD: process_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
            GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2, END_OPERATOR_CD: operatorCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetGeneralProcessDetails(selectedColumns);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tblGeneralProcess"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/FactoryDailyWorkStatusSearchProcessWise/GeneralProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divGeneralProcessGridMain").html(response.message);
                        })

                }
            }
            else {
                GetGeneralProcessDetails(selectedColumns);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


    function GetRowCountTransferReport(selectedColumns) {

        var ajaxTime = new Date().getTime();
       /* var machine_CD = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;*/
        var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
        var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
        var customer_CD = $("#ddlCustomer").val() ? $("#ddlCustomer").val() : null;
        var prod_Name = $("#txtProdName").val() ? $("#txtProdName").val() : null;
        var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
        var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
        var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
        var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
        var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
        var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
        var charge = $("#txtCharge").val() ? $("#txtCharge").val() : null;
        var chuuban = $("#txtChuuban").val() ? $("#txtChuuban").val() : null;
        var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
        var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
        var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
        var insp_Date1 = $("#txtInspDate1").val() ? $("#txtInspDate1").val() : null;
        var insp_Date2 = $("#txtInspDate2").val() ? $("#txtInspDate2").val() : null;

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/WIPTensou/GetRowCountForTransferReport",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                /*MACHINE_CD: machine_CD,*/ LOCATION_CD: location_CD, PROCESS_CD: process_CD, CUSTOMER_CD: customer_CD, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, CHARGE: charge, CHUUBAN: chuuban,
                GYOUBAN: gyouban, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
                PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, INSPECTION_DATE1: insp_Date1, INSPECTION_DATE2: insp_Date2
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var result = response.message;

                var rowLimit = $("#rowLimitValue").val();
                if (parseInt(result) > parseInt(rowLimit)) {
                    var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                    focusFieldID = 'rowLimitValue';
                    showMessageDialog(message, focusFieldID, WARNING);

                    if (parseInt(rowLimit) > 0) {
                        GetTransferDetails(selectedColumns);
                    }
                    else {
                        $("#btnExportCSV").attr("DISABLED", "DISABLED");
                        var list = [];
                        clearTable($("#tblTransferReport"), "", "GridNormalRow");
                        var settings = {
                            async: true,
                            crossDomain: true,
                            cache: false,
                            dataType: "json",
                            url: "/FactoryDailyWorkStatusSearchProcessWise/TransferReportGrid",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(list)
                        }
                        $.ajax(settings)
                            .done(function (response) {
                                $("#divTransferReportGridMain").html(response.message);
                            })

                    }
                }
                else {
                    GetTransferDetails(selectedColumns);
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_system_exception").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
}

$('#txtChuuban').keypress(function (e) {
    var orderNo = $('#txtChuuban').val() ? $('#txtChuuban').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Common/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuuban').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtGoubanCom').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#ddlLocation').focus();
                } else {
                    $('#txtChuuban').val("");
                    $('#txtGoubanCom').val("");
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

});