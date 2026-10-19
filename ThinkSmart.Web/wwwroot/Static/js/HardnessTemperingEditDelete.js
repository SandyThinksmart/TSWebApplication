var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var operatorCD = "";
var machineCD = "";

function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('TempCD=');
    result = sParameterName[1];

    return result;
}
function Mask() {
    $("#txtHardeningFuranceInDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtHardeningFuranceOutDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFuranceInDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFuranceOutDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
}

function SetDecimalFormats() {

    $("#txt_h_NorthSideUp1, #txt_h_NorthSideUp2, #txt_h_NorthSideUp3, #txt_h_NorthSideUpCent, #txt_h_NorthSideDown1, #txt_h_NorthSideDown2, #txt_h_NorthSideDown3, #txt_h_NorthSideDownCent, #txt_h_SouthSideUp1, #txt_h_SouthSideUp2, #txt_h_SouthSideUp3, #txt_h_SouthSideUpCent, #txt_h_SouthSideDown1, #txt_h_SouthSideDown2, #txt_h_SouthSideDown3, #txt_h_SouthSideDownCent, #txt_t_NorthSideUp1, #txt_t_NorthSideUp2, #txt_t_NorthSideUp3, #txt_t_NorthSideUpCent, #txt_t_NorthSideDown1, #txt_t_NorthSideDown2, #txt_t_NorthSideDown3, #txt_t_NorthSideDownCent, #txt_t_SouthSideUp1, #txt_t_SouthSideUp2, #txt_t_SouthSideUp3, #txt_t_SouthSideUpCent, #txt_t_SouthSideDown1, #txt_t_SouthSideDown2, #txt_t_SouthSideDown3, #txt_t_SouthSideDownCent").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 1);
    });

    $("#txtQuantity").keyup(function (event) {

        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtWeight").keyup(function (event) {

        RestrictToValidDecimalFormat($(this), 7, 2);
    });

}
$(document).ready(function () {
    window.resizeTo(1480, 480);
    RemoveOperatorClass();
    let tempCD = GetURLParameters(window.location.href).trim();
    SetDecimalFormats();
    Mask();
   
    SetTempType();
    SetNextProcess();
    if (parseInt(tempCD) > 0) {
        GetHardnessTemperingDetailsByCD(tempCD)
        EnableSaveButton();
    }
    $("#txtFuranceNumber").focus();
});
function RemoveOperatorClass() {
    $("#ddlQuenchingOperator").addClass("MandatoryTextBoxhard").removeClass("Mandatory");
    $("#ddlTemperingOperator").addClass("MandatoryTextBoxhard").removeClass("Mandatory");
 
}
function SetMachineList($target) {
    var ajaxTime = new Date().getTime();
  
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetMachineList?machineType=30",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMachineDropdown($target, response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMachineDropdown($target, jsonResult) {
   
    var getResult = jsonResult;
    jsonResult = jsonResult.filter(row => row.iN_ACTIVE_FLAG == 0)
    var machineList = getResult.filter(row1 => row1.cd == machineCD);

    if (machineList.length > 0) {
        if (machineList[0].iN_ACTIVE_FLAG == 1) {
            jsonResult.push({ cd: machineList[0].cd, name: machineList[0].name, yobina: machineList[0].yobina, iN_ACTIVE_FLAG: machineList[0].iN_ACTIVE_FLAG });
        }
    }

    $target.html("").append($('<option value="0"></option>'));

  
    $.each(jsonResult, function (key, jsonObj) {
      //  $("#ddlQuenchingMachine").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $target.append($('<option value="' + jsonObj.cd + '" flag= "' + (jsonObj.iN_ACTIVE_FLAG) + '"> ' + jsonObj.name + '</option>'));
     
    });
}
function GetHardnessTemperingDetailsByCD(tempCD) {
  
    
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/GetHardnessTemperingDetailsByHardnessTemperingCD?hardnessTemperingCD=" + tempCD,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                
                SetHardeningTemperingData(jsonResult);
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    
}

function SetHardeningTemperingData(jsonResult) {
    
    machineCD = jsonResult.hardeninG_MACHINE_CD;
    operatorCD = jsonResult.operatoR_CD_HARDENING;
    SetOperatorDropdown($("#ddlQuenchingOperator"), operatorCD);
  
    SetMachineList($("#ddlQuenchingMachine"));

    $("#hdfldProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#hdfldHardeningTemperingCD").val(jsonResult.hardeninG_TEMPERING_CD); 
    $("#hdfldMsgCD").val(jsonResult.msG_CD);
    $("#txtcustomer").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtProdName").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtCharge").val(jsonResult.pP_CHARGE);
    $("#txtMaterialDim").val(jsonResult.pP_MATL_DIM_D_SHAPE);
    $("#txtProductDim").val(jsonResult.pP_PROD_DIM_D_SHAPE_STRING);
    $("#txt_Weight").val(jsonResult.pP_QUANTITY); 
    $("#txt_Unit").val(jsonResult.pP_NO_OF_UNITS);

    $("#txtRange1").val(jsonResult.hardnesS_PATTERN2_MIN);
    $("#txtRange2").val(jsonResult.hardnesS_PATTERN2_MAX);

    $("#txtFuranceNumber").val(jsonResult.furnacE_NO);
    $("#txtThaba").val(jsonResult.tabA_NO);
    $("#txtChuuban").val(jsonResult.pP_CHUUBAN);

    if (jsonResult.pP_GYOUBAN.toString().length == 1)
    {
        jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
    }

    $("#txtgyouban").val(jsonResult.pP_GYOUBAN);
    $("#txtMetallicLabelNo").val(jsonResult.metalliC_PLATE_CODE);
    $("#txtQuantity").val(jsonResult.nO_OF_UNITS);
    $("#txtWeight").val(jsonResult.weight);
    $("#ddlTempType").val(jsonResult.wateR_OIL_TYPE);
    $("#txtTemprature").val(jsonResult.temperaturE_FURNACE);
    $("#txtConcentration").val(jsonResult.concentratioN_FURNACE);
    $("#txtfrequency").val(jsonResult.frequencY_FURNACE);
    $("#txtC_FURNACE").val(jsonResult.c_FURNACE);
    $("#txtMN_FURNACE").val(jsonResult.mN_FURNACE);
    $("#txtMO_FURNACE").val(jsonResult.mO_FURNACE);
    $("#txtCR_FURNACE").val(jsonResult.cR_FURNACE);
    $("#txtHardnessMin").val(jsonResult.hardnesS_MIN);
    $("#txtHardnessMax").val(jsonResult.hardnesS_MAX);
    $("#txtRemark").val(jsonResult.remark);
    $("#ddlNextProcess").val(jsonResult.hT_NEXT_PROCESS);

    $("#ddlQuenchingMachine").val(jsonResult.hardeninG_MACHINE_CD);
   
    $("#txtHardeningFuranceInDate").val($.format.date(new Date(jsonResult.starT_DATE_HARDENING_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtHardeningFuranceOutDate").val($.format.date(new Date(jsonResult.enD_DATE_HARDENING_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtTemperatureHardening").val(jsonResult.temperaturE_HARDENING);
    $("#txtTimeHardening").val(jsonResult.timE_HARDENING);
    $("#txt_h_NorthSideUp1").val(jsonResult.q_HARDNESS_NORTH_UP_1);
    $("#txt_h_NorthSideUp2").val(jsonResult.q_HARDNESS_NORTH_UP_2);
    $("#txt_h_NorthSideUp3").val(jsonResult.q_HARDNESS_NORTH_UP_3);
    $("#txt_h_NorthSideUpCent").val(jsonResult.q_HARDNESS_NORTH_UP_CEN);
    $("#txt_h_NorthSideDown1").val(jsonResult.q_HARDNESS_NORTH_DOWN_1);
    $("#txt_h_NorthSideDown2").val(jsonResult.q_HARDNESS_NORTH_DOWN_2);
    $("#txt_h_NorthSideDown3").val(jsonResult.q_HARDNESS_NORTH_DOWN_3);
    $("#txt_h_NorthSideDownCent").val(jsonResult.q_HARDNESS_NORTH_DOWN_CEN);
    $("#txt_h_SouthSideUp1").val(jsonResult.q_HARDNESS_SOUTH_UP_1);
    $("#txt_h_SouthSideUp2").val(jsonResult.q_HARDNESS_SOUTH_UP_2);
    $("#txt_h_SouthSideUp3").val(jsonResult.q_HARDNESS_SOUTH_UP_3);
    $("#txt_h_SouthSideUpCent").val(jsonResult.q_HARDNESS_SOUTH_UP_CEN);
    $("#txt_h_SouthSideDown1").val(jsonResult.q_HARDNESS_SOUTH_DOWN_1);
    $("#txt_h_SouthSideDown2").val(jsonResult.q_HARDNESS_SOUTH_DOWN_2);
    $("#txt_h_SouthSideDown3").val(jsonResult.q_HARDNESS_SOUTH_DOWN_3);
    $("#txt_h_SouthSideDownCent").val(jsonResult.q_HARDNESS_SOUTH_DOWN_CEN);

   
    
    machineCD = jsonResult.temperinG_MACHINE_CD;
    operatorCD = jsonResult.operatoR_CD_TEMPERING;
    SetOperatorDropdown($("#ddlTemperingOperator"), operatorCD);

    SetMachineList($("#ddlTemperingMachine"));
    $("#ddlTemperingMachine").val(jsonResult.temperinG_MACHINE_CD);
   
    $("#txtTemperingFuranceInDate").val(jsonResult.starT_DATE_TEMPERING_STRING);
    $("#txtTemperingFuranceOutDate").val(jsonResult.enD_DATE_TEMPERING_STRING);
    $("#txtTemperatureTempering").val(jsonResult.temperaturE_TEMPERING);
    $("#txtTimeTempering").val(jsonResult.timE_TEMPERING);
    $("#txt_t_NorthSideUp1").val(jsonResult.t_HARDNESS_NORTH_UP_1);
    $("#txt_t_NorthSideUp2").val(jsonResult.t_HARDNESS_NORTH_UP_2);
    $("#txt_t_NorthSideUp3").val(jsonResult.t_HARDNESS_NORTH_UP_3);
    $("#txt_t_NorthSideUpCent").val(jsonResult.t_HARDNESS_NORTH_UP_CENT);
    $("#txt_t_NorthSideDown1").val(jsonResult.t_HARDNESS_NORTH_DOWN_1);
    $("#txt_t_NorthSideDown2").val(jsonResult.t_HARDNESS_NORTH_DOWN_2);
    $("#txt_t_NorthSideDown3").val(jsonResult.t_HARDNESS_NORTH_DOWN_3);
    $("#txt_t_NorthSideDownCent").val(jsonResult.t_HARDNESS_NORTH_DOWN_CEN);
    $("#txt_t_SouthSideUp1").val(jsonResult.t_HARDNESS_SOUTH_UP_1);
    $("#txt_t_SouthSideUp2").val(jsonResult.t_HARDNESS_SOUTH_UP_2);
    $("#txt_t_SouthSideUp3").val(jsonResult.t_HARDNESS_SOUTH_UP_3);
    $("#txt_t_SouthSideUpCent").val(jsonResult.t_HARDNESS_SOUTH_UP_CENT);
    $("#txt_t_SouthSideDown1").val(jsonResult.t_HARDNESS_SOUTH_DOWN_1);
    $("#txt_t_SouthSideDown2").val(jsonResult.t_HARDNESS_SOUTH_DOWN_2);
    $("#txt_t_SouthSideDown3").val(jsonResult.t_HARDNESS_SOUTH_DOWN_3);
    $("#txt_t_SouthSideDownCent").val(jsonResult.t_HARDNESS_SOUTH_DOWN_CEN);

}
function SetTempType() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/GetTemperingType",
        method: "GET",
        contentType: "application/json; charset=utf-8"
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindTempTypeDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindTempTypeDropdown(jsonResult) {
    $("#ddlTempType").html("");
    
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTempType").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
       
    });
}

function SetNextProcess() {
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8"
        
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindNextProcessDropdown(jsonResult) {
  
    $("#ddlNextProcess").html("");//.append($('<option value="0"></option>'));


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

$("#btn_Delete").click(function () {
   
    var jsonData = ({
        HARDENING_TEMPERING_CD: $("#hdfldHardeningTemperingCD").val() ? $("#hdfldHardeningTemperingCD").val() : null,
        PROCESS_PLAN_CD: $("#hdfldProcessPlanCD").val() ? $("#hdfldProcessPlanCD").val() : null,
        TABA_NO: $("#txtThaba").val() ? $("#txtThaba").val() : null,
        MSG_CD: $("#hdfldMsgCD").val() ? $("#hdfldMsgCD").val() : null
    });

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/DeleteHardnessTemperingDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess == true) {
                DisableSaveButton();
                message = $("#lblSuccessMessageE_HARDNESS_EDIT_DELETE_002").text().trim();
                ShowMessageDialog(message, "btnInitialize", SUCCESS);
            }
          
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
});

function OperatorIsInactive(textValue, $target) {
    
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/OperatorIsInactive?operatorName=" + textValue,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            ShowMessageDialog(response.message, $target, WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function MachineIsInactive(textValue, $target) {
    
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/MachineIsInactive?machineName=" + textValue,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           
            ShowMessageDialog(response.message, $target, WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
$("#btn_Edit").click(function () {
    
    var focusField = "";
    var focusField2 = "";
    var isQuenchingInDateValid = true;
    var isQuenchingOutDateValid = true;
    var isTemperingInDateValid = true;
    var istemperingOutDateValid = true;
    var errorMsg = "";
    var errorMsg3 = "";

 
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    if (!IsDuplicateThabaNo($("#hdfldProcessPlanCD").val(), $("#txtThaba").val())) {
        ShowMessageDialog($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_015").text().trim().replace("NNN", $("#txtThaba").val()), "txtThaba", WARNING);
        return false;
    }

    if (!CheckDate($("#txtHardeningFuranceInDate"))) {
        focusField = "txtHardeningFuranceInDate";
        isQuenchingInDateValid = false;
    }
    if (!CheckDate($("#txtHardeningFuranceOutDate"))) {

        if (focusField == "") {
            focusField = "txtHardeningFuranceOutDate";
        }

        isQuenchingOutDateValid = false;
    }

    if (!CheckDate($("#txtTemperingFuranceInDate"))) {
        if (focusField == "") {
            focusField = "txtTemperingFuranceInDate";
        }
        isTemperingInDateValid = false;
    }
    if (!CheckDate($("#txtTemperingFuranceOutDate"))) {
        if (focusField == "") {
            focusField = "txtTemperingFuranceOutDate";
        }
        istemperingOutDateValid = false;
    }

    if (isQuenchingInDateValid == false) {
        errorMsg = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_009").text().trim();
    }

    if (isQuenchingOutDateValid == false) {

        if (errorMsg == "") {
            errorMsg = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_010").text().trim();
        } else {
            errorMsg = errorMsg + "<br>" + $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_010").text().trim();
        }
    }
    if (isTemperingInDateValid == false) {

        if (errorMsg == "") {
            errorMsg = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_011").text().trim();
        } else {
            errorMsg = errorMsg + "<br>" + $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_011").text().trim();
        }
    }

    if (istemperingOutDateValid == false) {

        if (errorMsg == "") {
            errorMsg = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_012").text().trim();
        } else {
            errorMsg = errorMsg + "<br>" + $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_012").text().trim();
        }
    }
  

    if (errorMsg != "") {
        ShowMessageDialog(errorMsg, focusField, WARNING);
        return false;
    }

   
    errorMsg3 = "";
    if ($("#txtHardeningFuranceInDate").val() >= $("#txtHardeningFuranceOutDate").val()) {

        errorMsg3 = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_013").text().trim();
        focusField2 = "txtHardeningFuranceInDate";
    }

    if ($("#txtTemperingFuranceInDate").val() >= $("#txtTemperingFuranceOutDate").val()) {

        if (errorMsg3 == "") {
            errorMsg3 = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_014").text().trim();
        } else {
            errorMsg3 = errorMsg3 + "<br>" + $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_014").text().trim();
        }
        if (focusField2 == "") {
            focusField2 = "txtTemperingFuranceInDate";
        }
    }
    if (errorMsg3 != "") {
        ShowMessageDialog(errorMsg3, focusField2, WARNING);
        return false;
    }

    if (!IsFurnaceNumberAlreadyExist($("#txtFuranceNumber").val())) {

        ShowMessageDialog($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_008").text().trim(), "txtFuranceNumber", WARNING);
        return false;
    }
    if ($("#ddlQuenchingMachine option:selected").attr("flag") == 1) {

        MachineIsInactive($("#ddlQuenchingMachine option:selected").text(), "ddlQuenchingMachine");
        return false;
    }
    

    if ($("#ddlQuenchingOperator option:selected").attr("flag") == 1) {

        OperatorIsInactive($("#ddlQuenchingOperator option:selected").text(), "ddlQuenchingOperator");
        return false;
    }

    if ($("#ddlTemperingMachine option:selected").attr("flag") == 1) {

        MachineIsInactive($("#ddlTemperingMachine option:selected").text(), "ddlTemperingMachine");
        return false;
    }


    if ($("#ddlTemperingOperator option:selected").attr("flag") == 1) {

        OperatorIsInactive($("#ddlTemperingOperator option:selected").text(), "ddlTemperingOperator");
        return false;
    }


    var processPlanCD = $("#hdfldProcessPlanCD").val();

    if (processPlanCD > 0) {

        var jsonData = ({

            HARDENING_TEMPERING_CD: $("#hdfldHardeningTemperingCD").val() ? $("#hdfldHardeningTemperingCD").val() : null,
            PROCESS_PLAN_CD: $("#hdfldProcessPlanCD").val() ? $("#hdfldProcessPlanCD").val() : null,
            FURNACE_NO: $("#txtFuranceNumber").val() ? $("#txtFuranceNumber").val() : null,
            TABA_NO: $("#txtThaba").val() ? $("#txtThaba").val() : null,
            HARDENING_MACHINE_CD: $("#ddlQuenchingMachine").val() ? $("#ddlQuenchingMachine").val() : null,
            TEMPERING_MACHINE_CD: $("#ddlTemperingMachine").val() ? $("#ddlTemperingMachine").val() : null,
            METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
            NO_OF_UNITS: $("#txtQuantity").val() ? $("#txtQuantity").val() : null,
            MSG_CD: $("#hdfldMsgCD").val() ? $("#hdfldMsgCD").val() : null, 
            UNIT_TYPE: $("#hdfUnitType").val(),
            WEIGHT: $("#txtWeight").val() ? $("#txtWeight").val() : null,
            WATER_OIL_TYPE: $("#ddlTempType").val() ? $("#ddlTempType").val() : null,
            TEMPERATURE_FURNACE: $("#txtTemprature").val() ? $("#txtTemprature").val() : null,
            CONCENTRATION_FURNACE: $("#txtConcentration").val() ? $("#txtConcentration").val() : null,
            FREQUENCY_FURNACE: $("#txtfrequency").val() ? $("#txtfrequency").val() : null,
            C_FURNACE: $("#txtC_FURNACE").val() ? $("#txtC_FURNACE").val() : null,
            MN_FURNACE: $("#txtMN_FURNACE").val() ? $("#txtMN_FURNACE").val() : null,
            MO_FURNACE: $("#txtMO_FURNACE").val() ? $("#txtMO_FURNACE").val() : null,
            CR_FURNACE: $("#txtCR_FURNACE").val() ? $("#txtCR_FURNACE").val() : null,
            HARDNESS_MIN: $("#txtHardnessMin").val() ? $("#txtHardnessMin").val() : null,
            HARDNESS_MAX: $("#txtHardnessMax").val() ? $("#txtHardnessMax").val() : null,
            OPERATOR_CD_HARDENING: $("#ddlQuenchingOperator").val() ? $("#ddlQuenchingOperator").val() : null,
            START_DATE_HARDENING: $("#txtHardeningFuranceInDate").val() != "" ? (($("#txtHardeningFuranceInDate").val().split(" ")[0] + "T" + $("#txtHardeningFuranceInDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
            END_DATE_HARDENING: $("#txtHardeningFuranceOutDate").val() != "" ? (($("#txtHardeningFuranceOutDate").val().split(" ")[0] + "T" + $("#txtHardeningFuranceOutDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
            TEMPERATURE_HARDENING: $("#txtTemperatureHardening").val() ? $("#txtTemperatureHardening").val() : null,
            TIME_HARDENING: $("#txtTimeHardening").val() ? $("#txtTimeHardening").val() : null,
            Q_HARDNESS_NORTH_UP_1: $("#txt_h_NorthSideUp1").val() ? $("#txt_h_NorthSideUp1").val() : null,
            Q_HARDNESS_NORTH_UP_2: $("#txt_h_NorthSideUp2").val() ? $("#txt_h_NorthSideUp2").val() : null,
            Q_HARDNESS_NORTH_UP_3: $("#txt_h_NorthSideUp3").val() ? $("#txt_h_NorthSideUp3").val() : null,
            Q_HARDNESS_NORTH_UP_CEN: $("#txt_h_NorthSideUpCent").val() ? $("#txt_h_NorthSideUpCent").val() : null,
            Q_HARDNESS_NORTH_DOWN_1: $("#txt_h_NorthSideDown1").val() ? $("#txt_h_NorthSideDown1").val() : null,
            Q_HARDNESS_NORTH_DOWN_2: $("#txt_h_NorthSideDown2").val() ? $("#txt_h_NorthSideDown2").val() : null,
            Q_HARDNESS_NORTH_DOWN_3: $("#txt_h_NorthSideDown3").val() ? $("#txt_h_NorthSideDown3").val() : null,
            Q_HARDNESS_NORTH_DOWN_CEN: $("#txt_h_NorthSideDownCent").val() ? $("#txt_h_NorthSideDownCent").val() : null,
            Q_HARDNESS_SOUTH_UP_1: $("#txt_h_SouthSideUp1").val() ? $("#txt_h_SouthSideUp1").val() : null,
            Q_HARDNESS_SOUTH_UP_2: $("#txt_h_SouthSideUp2").val() ? $("#txt_h_SouthSideUp2").val() : null,
            Q_HARDNESS_SOUTH_UP_3: $("#txt_h_SouthSideUp3").val() ? $("#txt_h_SouthSideUp3").val() : null,
            Q_HARDNESS_SOUTH_UP_CEN: $("#txt_h_SouthSideUpCent").val() ? $("#txt_h_SouthSideUpCent").val() : null,
            Q_HARDNESS_SOUTH_DOWN_1: $("#txt_h_SouthSideDown1").val() ? $("#txt_h_SouthSideDown1").val() : null,
            Q_HARDNESS_SOUTH_DOWN_2: $("#txt_h_SouthSideDown2").val() ? $("#txt_h_SouthSideDown2").val() : null,
            Q_HARDNESS_SOUTH_DOWN_3: $("#txt_h_SouthSideDown3").val() ? $("#txt_h_SouthSideDown3").val() : null,
            Q_HARDNESS_SOUTH_DOWN_CEN: $("#txt_h_SouthSideDownCent").val() ? $("#txt_h_SouthSideDownCent").val() : null,
            OPERATOR_CD_TEMPERING: $("#ddlTemperingOperator").val() ? $("#ddlTemperingOperator").val() : null,
            START_DATE_TEMPERING: $("#txtTemperingFuranceInDate").val() != "" ? (($("#txtTemperingFuranceInDate").val().split(" ")[0] + "T" + $("#txtTemperingFuranceInDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
            END_DATE_TEMPERING: $("#txtTemperingFuranceOutDate").val() != "" ? (($("#txtTemperingFuranceOutDate").val().split(" ")[0] + "T" + $("#txtTemperingFuranceOutDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
            TEMPERATURE_TEMPERING: $("#txtTemperatureTempering").val() ? $("#txtTemperatureTempering").val() : null,
            TIME_TEMPERING: $("#txtTimeTempering").val() ? $("#txtTimeTempering").val() : null,
            T_HARDNESS_NORTH_UP_1: $("#txt_t_NorthSideUp1").val() ? $("#txt_t_NorthSideUp1").val() : null,
            T_HARDNESS_NORTH_UP_2: $("#txt_t_NorthSideUp2").val() ? $("#txt_t_NorthSideUp2").val() : null,
            T_HARDNESS_NORTH_UP_3: $("#txt_t_NorthSideUp3").val() ? $("#txt_t_NorthSideUp3").val() : null,
            T_HARDNESS_NORTH_UP_CENT: $("#txt_t_NorthSideUpCent").val() ? $("#txt_t_NorthSideUpCent").val() : null,
            T_HARDNESS_NORTH_DOWN_1: $("#txt_t_NorthSideDown1").val() ? $("#txt_t_NorthSideDown1").val() : null,
            T_HARDNESS_NORTH_DOWN_2: $("#txt_t_NorthSideDown2").val() ? $("#txt_t_NorthSideDown2").val() : null,
            T_HARDNESS_NORTH_DOWN_3: $("#txt_t_NorthSideDown3").val() ? $("#txt_t_NorthSideDown3").val() : null,
            T_HARDNESS_NORTH_DOWN_CEN: $("#txt_t_NorthSideDownCent").val() ? $("#txt_t_NorthSideDownCent").val() : null,
            T_HARDNESS_SOUTH_UP_1: $("#txt_t_SouthSideUp1").val() ? $("#txt_t_SouthSideUp1").val() : null,
            T_HARDNESS_SOUTH_UP_2: $("#txt_t_SouthSideUp2").val() ? $("#txt_t_SouthSideUp2").val() : null,
            T_HARDNESS_SOUTH_UP_3: $("#txt_t_SouthSideUp3").val() ? $("#txt_t_SouthSideUp3").val() : null,
            T_HARDNESS_SOUTH_UP_CENT: $("#txt_t_SouthSideUpCent").val() ? $("#txt_t_SouthSideUpCent").val() : null,

            T_HARDNESS_SOUTH_DOWN_1: $("#txt_t_SouthSideDown1").val() ? $("#txt_t_SouthSideDown1").val() : null,
            T_HARDNESS_SOUTH_DOWN_2: $("#txt_t_SouthSideDown2").val() ? $("#txt_t_SouthSideDown2").val() : null,
            T_HARDNESS_SOUTH_DOWN_3: $("#txt_t_SouthSideDown3").val() ? $("#txt_t_SouthSideDown3").val() : null,
            T_HARDNESS_SOUTH_DOWN_CEN: $("#txt_t_SouthSideDownCent").val() ? $("#txt_t_SouthSideDownCent").val() : null,
            HT_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
            REMARK: $("#txtRemark").val() ? $("#txtRemark").val() : null,
            USER_ID: $("#hdfLoginId").val()

        });

    }

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/UpdateHardnessTemperingDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                if (parseInt(jsonResult) > 0) {
                    $("#hdfldMsgCD").val(jsonResult);
                } else
                {
                    $("#hdfldMsgCD").val("");
                }

                //  disableSaveButton();
                message = $("#lblSuccessMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
                ShowMessageDialog(message, "btnInitialize", SUCCESS);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
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
$("#btn_extract").click(function (event) {
    
    var message = "";
    var furnaceNo = $("#txtFuranceNumber").val().trim();
    var chuuban = $("#txtChuuban").val().trim();
    var gyoban = $("#txtgyouban").val().trim();
    var userID = $("#hdfLoginId").val(); 
    focusFieldID = "";

    if (!furnaceNo) {
        message = $("#txtFuranceNumber").attr("ErrorLabel");

        focusFieldID = "txtFuranceNumber";

    }

    if (!chuuban) {
        if (message == "") {
            message = $("#txtChuuban").attr("ErrorLabel");
        } else {
            message = message + "<br>" + $("#txtChuuban").attr("ErrorLabel");
        }
        if (focusFieldID == "") {
            focusFieldID = "txtChuuban";
        }
       
    }
    if (!gyoban) {
        if (message == "") {
            message = $("#txtgyouban").attr("ErrorLabel");
        } else {
            message = message + "<br>" + $("#txtgyouban").attr("ErrorLabel");
        }
        if (focusFieldID == "") {
            focusFieldID = "txtgyouban";
        }
    }

    if (message !== "") {

        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;

    }
    else {
        var ajaxTime = new Date().getTime();
  
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/ValidateChubban",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ chuuban: chuuban, gyouban: gyoban })
         
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var result = response.message
               
                if (result.responce == 4) {
                    // OrNo completed
                    ClearPanel($("#ScreenData"));

                    $("#txtFuranceNumber").val(furnaceNo);
                    $("#txtChuuban").val(chuuban);
                    $("#txtgyouban").val(gyoban);
                    DisableSaveButton();
                    var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_006").text();
                    ShowMessageDialog(Message, "txtChuuban", WARNING);

                } else if (result.responce == 3) {

                   
                    // OrNo Not Started FromHandOver
                    ClearPanel($("#ScreenData"));

                    $("#txtFuranceNumber").val(furnaceNo);
                    $("#txtChuuban").val(chuuban);
                    $("#txtgyouban").val(gyoban);
                    var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_005").text().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                    ShowMessageDialog(Message, "txtChuuban", WARNING);

                }
                else if (result.responce == 2) {
                  
                    // Chuuban not found in system.
                    ClearPanel($("#ScreenData"));
                    $("#txtFuranceNumber").val(furnaceNo);
                    $("#txtChuuban").val(chuuban);
                    $("#txtgyouban").val(gyoban);
                    var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_002").text();
                    ShowMessageDialog(Message, "txtFuranceNumber", WARNING);
                }
                else if (result.responce == 1) {

                    GetHardnessTemperingDetailsByChuubanGyoban(furnaceNo, chuuban, gyoban, userID);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });


        
    }

});

function GetHardnessTemperingDetailsByChuubanGyoban(furnaceNo, chuuban, gyouban, userID) {
   
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardnessTemperingDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ FURNACE_NO: furnaceNo, CHUUBAN: chuuban, GYOUBAN: gyouban, USER_ID: userID })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var jsonResult = response.message;
            if (jsonResult != null) {
                EnableSaveButton();
                SetHardeningTemperingData(jsonResult);
            } else {
                ClearPanel($("#ScreenData"));
                $("#txtFuranceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyouban").val(gyouban);
                var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_002").text();
                ShowMessageDialog(Message, "txtFuranceNumber", WARNING);
            }
            //if (jsonResult != null) {

            //    if (jsonResult.procesS_PLAN_CMPL_DATE != null) {
            //        clearPanel($("#ScreenData"));

            //        $("#txtFuranceNumber").val(furnaceNo);
            //        $("#txtChuuban").val(chuuban);
            //        $("#txtgyouban").val(gyouban);
            //        disableSaveButton();
            //        var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_006").text();
            //        showMessageDialog(Message, "txtChuuban", WARNING);
                  
            //    } else if (jsonResult.procesS_PLAN_START_DATE == null) {

            //         clearPanel($("#ScreenData"));
                                        
            //        $("#txtFuranceNumber").val(furnaceNo);
            //        $("#txtChuuban").val(chuuban);
            //        $("#txtgyouban").val(gyouban);
            //        var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_005").text().replace("99-999999-99", jsonResult.pP_CHUUBAN_GYOUBAN);
            //        showMessageDialog(Message, "txtChuuban", WARNING);
                   
            //    } else {
            //        enableSaveButton();
            //        setHardeningTemperingData(jsonResult);
            //    }
               
            //} else {
            //    clearPanel($("#ScreenData"));
            //    $("#txtFuranceNumber").val(furnaceNo);
            //    $("#txtChuuban").val(chuuban);
            //    $("#txtgyouban").val(gyouban);
            //    var Message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_002").text();
            //    showMessageDialog(Message, "txtFuranceNumber", WARNING);
                

            //}
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}


function RemoveParameter(param) {
    const url = new URL(window.location.href);
    url.searchParams.delete(param);
    window.history.replaceState({}, "", url);
}

$("#btnInitialize").click(function () {
    
    ClearPanel($("#ScreenData"));
    DisableSaveButton();
     
    RemoveParameter("TempCD");
       
   operatorCD = "";
   machineCD = "";
  
});
function EnableSaveButton() {
    $("#btn_Edit").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btn_Delete").removeAttr(DISABLED).removeClass(DISABLED);
}

function DisableSaveButton() {

    $("#btn_Edit").attr("disabled", "disabled");
    $("#btn_Delete").attr("disabled", "disabled");

}
function ClearPanel($panel) {

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
    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
    });


    $.each($panel.find("textarea"), function (index, textarea) {
        $(textarea).val("");
    });

    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}

function IsMandatoryFieldsFilled() {
    
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find(".MandatoryTextBoxhard"), function (index, mandatoryField) {
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
        ShowMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
}

function CheckDate(field) {
    
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
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

function IsFurnaceNumberAlreadyExist(furnaceNo) {
    
    var returnVal = false;

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/CheckFurnaceIsAlreadyEntered?furnaceNo=" + furnaceNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({ FURNACE_NO: furnaceNo, CHUUBAN: chuuban, GYOUBAN: gyouban })
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            var tempcd = $("#hdfldHardeningTemperingCD").val();
            if (jsonResult == null) {
                returnVal = true;
            }else if (jsonResult.hardeninG_TEMPERING_CD == tempcd ) {
                returnVal = true;
            } else {
                returnVal = false;
            }
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnVal;
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
            url: APIURL + "/HardnessTempering/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
               
                if (response.isSuccess) {
                    $('#txtChuuban').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtgyouban').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btn_extract').focus();
                } else {
                    $('#txtChuuban').val("");
                    $('#txtgyouban').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});
function IsDuplicateThabaNo(processCD, tabaNo) {
    var returnVal = false;

    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetThabaDetailsByProcessPlan?processPlanCD=" + processCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            var temperingCD = $("#hdfldHardeningTemperingCD").val();
           
            if (jsonResult.length == 0) {
                returnVal = true;
            } else {

                var tabaList = $.map(jsonResult, function (item) {
                    if (item.tabA_NO == tabaNo) { // WHERE condition
                        return {
                            TABA_NO: item.tabA_NO,
                            HARDENING_TEMPERING_CD: item.hardeninG_TEMPERING_CD
                        };
                    }
                });

                if (tabaList.length == 0)
                {
                    returnVal = true;
                }else if (tabaList[0].HARDENING_TEMPERING_CD != temperingCD) {
                    returnVal = false;
                } else {
                    returnVal = true;
                }

            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });
    return returnVal;
}