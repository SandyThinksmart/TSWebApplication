var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var OldRemarkMsg = "";
let enterPressedOpr = false;
var operatorModified = false;
var isOperatorActive = 0;
function Mask() {
    $("#txtHardenessFuranceInDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtHardenessFuranceOutDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFurnaceInDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFurnaceOutDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" }); 

    $("#txtInspDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" }); 
}
$(document).ready(function () {

    window.resizeTo(1500, 580);
    Mask();
   // SetDefaultProperty();
    $("#txtFurnaceNumber").focus();
    var HTQualityCD = GetURLParameters(window.location.href);

    if (HTQualityCD > 0) {
        GetHTQualityDetailsByCD(HTQualityCD);
        EnableEditDeleteButton();
        DisableSearchPanel();
    }
});

function EnableSearchPanel() {
    $("#txtFurnaceNumber").removeAttr(disabled).removeClass(disabled);
    $("#txtChuuban").removeAttr(disabled).removeClass(disabled);
    $("#txtgyubaan").removeAttr(disabled).removeClass(disabled);
    $("#btn_extract").removeAttr(disabled).removeClass(disabled);
}

function DisableSearchPanel() {

    $("#txtFurnaceNumber").attr(disabled, disabled);
    $("#txtChuuban").attr(disabled, disabled);
    $("#txtgyubaan").attr(disabled, disabled);
    $("#btn_extract").attr(disabled, disabled);
}

function SetDefaultProperty()
{
    $("#tdOperator").css("vertical-align", "middle");
    $("#tdTextYobina").css("vertical-align", "middle");
    $("#txtOperatorYobina").css({ "width": "70px", "border-style" : "none"});
    $("#btnYobinaStartOpeartorSearch").css({ "height": "49px", "width": "20px" });
    $('#ddlOperator').css({ "border-style": "none", "padding-bottom": "2px", "font-family": "MS Gothic", "Height": "24px", "min-width": "200px" }).addClass("MandatoryTextBoxhard");
    
}
function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) {
        return result;
    } else {
        var sPageURL = url.substring(searchIndex + 1);
        var sParameterName = url.split('QualityCD=');
        result = sParameterName[1];
        return result;
    }
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
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

$("#btn_extract").click(function () {
    
    var message = "";
    var furnaceNo = $("#txtFurnaceNumber").val();
    var chuuban = $("#txtChuuban").val();
    var gyoban = $("#txtgyubaan").val();
    var focusFieldID = "";

    if (!furnaceNo) {
        message = $('#txtFurnaceNumber').attr("ErrorLabel");
        focusFieldID = "txtFurnaceNumber";
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
            message = $("#txtgyubaan").attr("ErrorLabel");

        } else {
            message = message + "<br>" + $("#txtgyubaan").attr("ErrorLabel");
        }

        if (focusFieldID == "") {
            focusFieldID = "txtgyubaan";
        }
    }
    if (message !== "") {
        ShowMessageDialog(message, focusFieldID, WARNING);
       
    }
    else {
        ValidateChubban(furnaceNo, chuuban, gyoban);
        
    }

});
function ValidateChubban(furnaceNo, chuuban, gyouban) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/ValidateChubban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (result.responce == 4) {
                // Chuuban is marked as completed in the system/
                ClearPanel($("#divData"));
                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_005").text();
                ShowMessageDialog(errorMessage, "txtChuuban", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                // Chuuban not found in system.
                ClearPanel($("#divData"));
                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);
                var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_003").text();
                ShowMessageDialog(errorMessage, "txtChuuban", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                ClearPanel($("#divData"));
                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);
                var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_004").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(message, "txtChuuban", WARNING);
                return false;
            }
            else if (result.responce == 1) {

                GetHardnessTemperingDetailsByChuubanGyoban(furnaceNo, chuuban, gyouban);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function GetHardnessTemperingDetailsByChuubanGyoban(furnaceNo, chuuban, gyouban) {
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardeningTemperingData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ FURNACE_NO: furnaceNo, CHUUBAN: chuuban, GYOUBAN: gyouban })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            DisableSaveButton();
            if (response.isSuccess == false) {

                ClearPanel($("#divData"));

                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);

                var Message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_002").text();
                ShowMessageDialog(Message, "txtFurnaceNumber", WARNING);
              

            } else if (jsonResult == null) {
                ClearPanel($("#divData"));

                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);
                var Message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_003").text();
                ShowMessageDialog(Message, "txtChuuban", WARNING);


            } else if (jsonResult.procesS_PLAN_START_DATE == null) {
                ClearPanel($("#divData"));

                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);

                var Message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_004").text().replace("99-999999-99", jsonResult.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(Message, "txtChuuban", WARNING);

            } else if (jsonResult.procesS_PLAN_CMPL_DATE != null) {
                ClearPanel($("#divData"));

                $("#txtFurnaceNumber").val(furnaceNo);
                $("#txtChuuban").val(chuuban);
                $("#txtgyubaan").val(gyouban);

                var Message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_005").text();
                ShowMessageDialog(Message, "txtChuuban", WARNING);

            } 


            else {
                if (jsonResult != null) {
                    DisableSearchPanel();
                    EnableSaveButton();
                   // SetOperatorDropdown($("#ddlOperator"), 0);
                    //SetNextProcess();
                    SetStartSysDateTime();
                    SetDecision();
                    SetTexture();

                    $("#hdfProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
                    $("#txtcustomer").val(jsonResult.pP_CUSTOMER_NAME);
                    $("#txtProdName").val(jsonResult.pP_PROD_NAME_CUST);
                    $("#txtCharge").val(jsonResult.pP_CHARGE);
                    $("#txtMetallicLabelNo").val(jsonResult.metalliC_PLATE_CODE);
                    $("#txtMaterialDim").val(jsonResult.pP_MATL_DIM_D_SHAPE);
                    $("#txtProductDim").val(jsonResult.pP_PROD_DIM_D_SHAPE_STRING);
                    $("#txtActUnits").val(jsonResult.pP_NO_OF_UNITS);
                    $("#txtActWeight").val(jsonResult.pP_QUANTITY);

                    $("#txtQuantity").val(jsonResult.nO_OF_UNITS);
                    $("#txtWeight").val(jsonResult.weight);
                    $("#txtRange1").val(jsonResult.hardnesS_PATTERN2_MIN);
                    $("#txtRange2").val(jsonResult.hardnesS_PATTERN2_MAX);

                    $("#txtHardnessMachineNo").val(jsonResult.hardeninG_MACHINE_NAME);
                    $("#txtTemperingMachineNo").val(jsonResult.temperinG_MACHINE_NAME);
                    $("#txtHardnessOperator").val(jsonResult.hardeninG_OPERATOR_NAME);
                    $("#txtTemperingOperator").val(jsonResult.temperinG_OPERATOR_NAME);

                    $("#txtHardenessFuranceInDate").val(jsonResult.starT_DATE_HARDENING_STRING);
                    $("#txtHardenessFuranceOutDate").val(jsonResult.enD_DATE_HARDENING_STRING);
                    $("#txtTemperingFurnaceInDate").val(jsonResult.starT_DATE_TEMPERING_STRING);
                    $("#txtTemperingFurnaceOutDate").val(jsonResult.enD_DATE_TEMPERING_STRING);

                    $("#txtOilType").val(jsonResult.wateR_OIL_STRING);
                    $("#txtFuranceNumber").val(jsonResult.furnacE_NO);

                    $("#txtTemprature").val(jsonResult.temperaturE_FURNACE);
                    $("#txtConcentration").val(jsonResult.concentratioN_FURNACE);
                    $("#txtfrequency").val(jsonResult.frequencY_FURNACE);
                    $("#txtC_FURNACE").val(jsonResult.c_FURNACE);
                    $("#txtMN_FURNACE").val(jsonResult.mN_FURNACE);
                    $("#txtMO_FURNACE").val(jsonResult.mO_FURNACE);
                    $("#txtCR_FURNACE").val(jsonResult.cR_FURNACE);
                    $("#txtHardnessMin").val(jsonResult.hardnesS_MIN);
                    $("#txtHardnessMax").val(jsonResult.hardnesS_MAX);

                    $("#txtHardeningFuranceInDate").val(jsonResult.starT_DATE_HARDENING);
                    $("#txtHardeningFuranceOutDate").val(jsonResult.enD_DATE_HARDENING);
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

                    $("#txtTemperingFuranceInDate").val(jsonResult.starT_DATE_TEMPERING);
                    $("#txtTemperingFuranceOutDate").val(jsonResult.enD_DATE_TEMPERING);
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
                   // $("#ddlNextProcess").val(jsonResult.hT_NEXT_PROCESS);

                    $("#txtRemark").val(jsonResult.remark);
                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

$("#btnDelete").click(function () {
    
    var htQuality_CD = $("#hdfHT_QualityCD").val();
    var msg_CD = $("#hdfHT_MsgCD").val();
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/DeleteHTQualityDetails?htQualityCD=" + htQuality_CD + "&&msgCD=" + msg_CD,
        method: "GET",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            DisableEditDeleteButton();
            var message = $("#lblSuccessMessageE_HARDNESS_ENTRY_EDIT_DELETE_003").text().trim();
            ShowMessageDialog(message, "btnInitialize", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
});

$("#btnEdit").click(function () {

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    if (!CheckDate($("#txtInspDate"))) {

        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_006").text().trim();
        ShowMessageDialog(message, "txtInspDate", WARNING);
        return false;
    }

    if (isOperatorActive == 1) {

        OperatorIsInactive();
            return false;
    }

    var ajaxTime = new Date().getTime();
    var jsonData = ({

        PROCESS_PLAN_CD: $("#hdfProcessPlanCD").val(),
        HT_QUALITY_CD: $("#hdfHT_QualityCD").val(),
        INSPECTION_DATE: ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        INSPECTED_BY_CD: $("#hdfOperatorCD").val(),
        DM_F_MIN: $("#txtDMFMin").val() ? $("#txtDMFMin").val() : null,
        DM_F_MAX: $("#txtDMFMax").val() ? $("#txtDMFMax").val() : null,
        DM_T_MIN: $("#txtDMTMin").val() ? $("#txtDMTMin").val() : null,
        DM_T_MAX: $("#txtDMTMax").val() ? $("#txtDMTMax").val() : null,
        STRUCTURE_OUT: $("#txtTextureOut").val() ? $("#txtTextureOut").val() : null,
        STRUCTURE_IN: $("#txtTextureIn").val() ? $("#txtTextureIn").val() : null,
        MSG_CD: $("#hdfHT_MsgCD").val() ? $("#hdfHT_MsgCD").val() : null,
        REMARK: $("#txtAreaRemark").val() ? $("#txtAreaRemark").val() : null,
        DECISION: $("#ddlDecision").val(),
       // HTQ_NEXT_PROCESS: $("#ddlNextProcess").val(),
        HT_REMARK_PREVIOUS:OldRemarkMsg,
        USER_ID: $("#hdfLoginId").val()

    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/UpdateHTQualityDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult > 0) {
                $("#hdfHT_MsgCD").val(jsonResult);
            } else
            {
                $("#hdfHT_MsgCD").val("");
            }
            
            // DisableEditDeleteButton();
            message = $("#lblSuccessMessageE_HARDNESS_ENTRY_EDIT_DELETE_002").text().trim();
            ShowMessageDialog(message, "btnInitialize", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
});
function OperatorIsInactive() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/OperatorIsInactive?operatorName=" + $("#txtOperator").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            ShowMessageDialog(response.message, "txtOperator", WARNING);
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function RemoveParameter(param) {
    const url = new URL(window.location.href);
    url.searchParams.delete(param);
    window.history.replaceState({}, "", url);
}

$("#btnInitialize").click(function () {
    EnableSearchPanel();
    ClearPanel($("#divData"));
    RemoveParameter("QualityCD");
    OldRemarkMsg = "";
    isOperatorActive = 0;
    SetStartSysDateTime();
    DisableEditDeleteButton();
    $("#txtFurnaceNumber").focus();
});

$("#btnRegister").click(function () {
      
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    if (!CheckDate($("#txtInspDate"))) {
        
        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_006").text().trim();
        ShowMessageDialog(message, "txtInspDate", WARNING);
        return false;
    }

    //if (!IsDateDifferentFromRange($("#txtInspDate").val())) 
    if (!IsDateValid(true)) {
        ShowMessageDialog($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_008").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_009").text().trim() + commonDateRange + ")", "txtInspDate", WARNING);
        return false;

    }

    //if ($("#ddlOperator option:selected").attr("flag") == 1) {
    //    var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_007").text().trim().replace("DATA_VALUE", $("#ddlOperator option:selected").text());
    //    ShowMessageDialog(message, "ddlOperator", WARNING);

    //    return false;
    //}

    if (IsDataAlreadyExist())
    {
        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_014").text().trim();
        ShowMessageDialog(message, "btnInitialize", WARNING);
        return false;
        
    }
    var processPlanCD = $("#hdfProcessPlanCD").val();

    if (processPlanCD > 0) {

        var jsonData = ({

            PROCESS_PLAN_CD: $("#hdfProcessPlanCD").val(),
            FURNACE_NO: $("#txtFurnaceNumber").val(),
            INSPECTION_DATE: ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'), //(($("#txtInspDate").val().split(" ")[0]).replaceAll('/', '-')),
            INSPECTED_BY_CD: $("#hdfOperatorCD").val(),
            DM_F_MIN: $("#txtDMFMin").val() ? $("#txtDMFMin").val() : null,
            DM_F_MAX: $("#txtDMFMax").val() ? $("#txtDMFMax").val() : null,
            DM_T_MIN: $("#txtDMTMin").val() ? $("#txtDMTMin").val() : null,
            DM_T_MAX: $("#txtDMTMax").val() ? $("#txtDMTMax").val() : null,
            STRUCTURE_OUT: $("#txtTextureOut").val() ? $("#txtTextureOut").val() : null,
            STRUCTURE_IN: $("#txtTextureIn").val() ? $("#txtTextureIn").val() : null,
            REMARK: $("#txtAreaRemark").val() ? $("#txtAreaRemark").val() : null,
            DECISION: $("#ddlDecision").val(),
           // HTQ_NEXT_PROCESS: $("#ddlNextProcess").val(),
            USER_ID: $("#hdfLoginId").val(),
                          
        });

    }

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/InsertHTQualityDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess == true) {
                DisableSaveButton();
                var successMessage = $("#lblSuccessMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text();
                ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);
            }

           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
})

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
function SetStartSysDateTime() {
    var ajaxTime = new Date().getTime();
   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetSysDateTime",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

            //$("#txtInspDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtInspDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    
}
function SetDecision() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDecisionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindDecisionDropdown(jsonResult) {
    $("#ddlDecision").html("");//.append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function SetTexture() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/GetTextureType",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindTextureDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindTextureDropdown(jsonResult) {
    $("#dataTextureOut").html("").append($('<option value=""></option>'));
    $("#dataTextureIn").html("").append($('<option value=""></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#dataTextureOut").append($('<option value="' + jsonObj.name + '">' + jsonObj.name + '</option>'));
        $("#dataTextureIn").append($('<option value="' + jsonObj.name + '"> ' + jsonObj.name + '</option>'));
    });
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
    $("#ddlDecision").html("").append($('<option value="0"></option>'));
    $("#dataTextureOut").html("").append($('<option value=""></option>'));
    $("#dataTextureIn").html("").append($('<option value=""></option>'));
   // $("#ddlNextProcess").html("").append($('<option value="0"></option>'));
   // $("#ddlOperator").html("").append($('<option value="0"></option>'));
}

function EnableSaveButton() {

    $("#btnRegister").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    
}
function DisableSaveButton() {
    $("#btnRegister").attr("DISABLED", "DISABLED").addClass("DISABLED");
}

function EnableEditDeleteButton() {

    $("#btnEdit").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    $("#btnDelete").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
}
function DisableEditDeleteButton() {
    $("#btnEdit").attr("DISABLED", "DISABLED").addClass("DISABLED");
    $("#btnDelete").attr("DISABLED", "DISABLED").addClass("DISABLED");
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
function GetHTQualityDetailsByCD(HTQualityCD) {

    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHTQualityDetailsByCD?HTQualityCD=" + HTQualityCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            OldRemarkMsg = "";
            debugger;
           // SetNextProcess();
            SetStartSysDateTime();
            SetDecision();
            SetTexture();
            // SetOperatorDropdown($("#ddlOperator"), jsonResult.inspecteD_BY_CD);
            $("#hdfOperatorCD").val(jsonResult.inspecteD_BY_CD); 
            $("#txtOperator").val(jsonResult.inspecteD_OPERATOR_NAME); 
            isOperatorActive = jsonResult.iN_ACTIVE_FLAG;
            $("#hdfHT_QualityCD").val(jsonResult.hT_QUALITY_CD); 
            $("#hdfProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
            $("#txtcustomer").val(jsonResult.pP_CUSTOMER_NAME);
            $("#txtProdName").val(jsonResult.pP_PROD_NAME_CUST);
            $("#txtCharge").val(jsonResult.pP_CHARGE);
            $("#txtMetallicLabelNo").val(jsonResult.metalliC_PLATE_CODE);
            $("#txtMaterialDim").val(jsonResult.pP_MATL_DIM_D_SHAPE);
            $("#txtProductDim").val(jsonResult.pP_PROD_DIM_D_SHAPE_STRING);
            $("#txtActUnits").val(jsonResult.pP_NO_OF_UNITS);
            $("#txtActWeight").val(jsonResult.pP_QUANTITY);

            $("#txtQuantity").val(jsonResult.nO_OF_UNITS);
            $("#txtWeight").val(jsonResult.weight);
            $("#txtRange1").val(jsonResult.hardnesS_PATTERN2_MIN);
            $("#txtRange2").val(jsonResult.hardnesS_PATTERN2_MAX);

            $("#txtHardnessMachineNo").val(jsonResult.hardeninG_MACHINE_NAME);
            $("#txtTemperingMachineNo").val(jsonResult.temperinG_MACHINE_NAME);
            $("#txtHardnessOperator").val(jsonResult.hardeninG_OPERATOR_NAME);
            $("#txtTemperingOperator").val(jsonResult.temperinG_OPERATOR_NAME);

            $("#txtHardenessFuranceInDate").val(jsonResult.starT_DATE_HARDENING_STRING);
            $("#txtHardenessFuranceOutDate").val(jsonResult.enD_DATE_HARDENING_STRING);
            $("#txtTemperingFurnaceInDate").val(jsonResult.starT_DATE_TEMPERING_STRING);
            $("#txtTemperingFurnaceOutDate").val(jsonResult.enD_DATE_TEMPERING_STRING);


            $("#txtOilType").val(jsonResult.wateR_OIL_STRING);
            $("#txtFuranceNumber").val(jsonResult.furnacE_NO);

            $("#txtTemprature").val(jsonResult.temperaturE_FURNACE);
            $("#txtConcentration").val(jsonResult.concentratioN_FURNACE);
            $("#txtfrequency").val(jsonResult.frequencY_FURNACE);
            $("#txtC_FURNACE").val(jsonResult.c_FURNACE);
            $("#txtMN_FURNACE").val(jsonResult.mN_FURNACE);
            $("#txtMO_FURNACE").val(jsonResult.mO_FURNACE);
            $("#txtCR_FURNACE").val(jsonResult.cR_FURNACE);
            $("#txtHardnessMin").val(jsonResult.hardnesS_MIN);
            $("#txtHardnessMax").val(jsonResult.hardnesS_MAX);

            $("#txtHardeningFuranceInDate").val(jsonResult.starT_DATE_HARDENING);
            $("#txtHardeningFuranceOutDate").val(jsonResult.enD_DATE_HARDENING);
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

            $("#txtTemperingFuranceInDate").val(jsonResult.starT_DATE_TEMPERING);
            $("#txtTemperingFuranceOutDate").val(jsonResult.enD_DATE_TEMPERING);
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
            $("#txtRemark").val(jsonResult.remark);
            $("#txtFurnaceNumber").val(jsonResult.furnacE_NO);
            $("#txtChuuban").val(jsonResult.pP_CHUUBAN);
            if (jsonResult.pP_CHUUBAN.toString().length == 1) {
                jsonResult.pP_CHUUBAN ="0" + jsonResult.pP_CHUUBAN;
            }
            $("#txtgyubaan").val(jsonResult.pP_GYOUBAN);

            $("#txtInspDate").val($.format.date(new Date(jsonResult.inspectioN_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
            $("#txtDMFMin").val(jsonResult.dM_F_MIN);
            $("#txtDMFMax").val(jsonResult.dM_F_MAX);
            $("#txtDMTMin").val(jsonResult.dM_T_MIN);
            $("#txtDMTMax").val(jsonResult.dM_T_MAX);
            $("#txtTextureOut").val(jsonResult.structurE_OUT);
            $("#txtTextureIn").val(jsonResult.structurE_IN);
            $("#dataTextureOut").val(jsonResult.structurE_OUT);
            $("#dataTextureIn").val(jsonResult.structurE_IN);

            $("#hdfHT_MsgCD").val(jsonResult.hT_MSG_CD);
            $("#txtAreaRemark").val(jsonResult.hT_REMARK);
            OldRemarkMsg = jsonResult.hT_REMARK;
            $("#ddlDecision").val(jsonResult.decision);
           // $("#ddlNextProcess").val(jsonResult.htQ_NEXT_PROCESS);
        
            })
       
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
//function IsDateDifferentFromRange(dateVal) {
//    debugger;
//    var returnval = true;
//    var currentDate = new Date(dateVal);

//    var minDate = new Date();
//    var maxDate = new Date();
//    minDate.setDate(currentDate.getMonth() - parseInt(commonDateRange));
//    maxDate.setDate(currentDate.getMonth() + parseInt(commonDateRange));

//    var minimumDate = minDate.getFullYear() + "/" + String('00' + minDate.getMonth() + 1).slice(-2) + "/" + String('00' + minDate.getDate()).slice(-2);
//    var maximumDate = maxDate.getFullYear() + "/" + String('00' + maxDate.getMonth() + 1).slice(-2) + "/" + String('00' + maxDate.getDate()).slice(-2);

//    if (Date.parse(dateVal) < Date.parse(minimumDate) || Date.parse(dateVal) > Date.parse(maximumDate)) {
//        returnval = false;
//    }


//    return returnval;
//}

function IsDateValid(boolVal) {
    var returnval = false;
    var getDate;
    // getDate = (($("#txtInspDate").val().split(" ")[0]).replaceAll('/', '-'));     
    getDate = ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/IsValidDateRange",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DATERANGE: commonDateRange, RANGE_DATETIME: getDate })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnval = response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });




    return returnval;
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
                    $('#txtgyubaan').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btn_extract').focus();
                } else {
                    $('#txtChuuban').val("");
                    $('#txtgyubaan').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }

});

$('#txtOperator')
    .on('focus', function () {
        operatorModified = false;
    })
    .on('input', function () {
        operatorModified = true;
    });

$('#txtOperator').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorModified == true) {
        $("#hdfOperatorCD").val(operatorNumber);
        enterPressedOpr = true;
        GetOperatorNameByCD($(this));

        $("#txtDMFMin").focus();

    }

    if (e.type === 'focusout' && operator && operatorModified == true) {
        if (enterPressedOpr) {

            enterPressedOpr = false; // Reset the flag
            return false;
        }
        $("#hdfOperatorCD").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtDMFMin").focus();
    }
    enterPressedOpr = false; // Reset the flag
});
function GetOperatorNameByCD($target) {
    var ajaxTime = new Date().getTime();
    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_010").text().trim().replace("*****", $("#hdfOperator").val());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorCD == "") {
        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_012").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {
       
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;

                var jsonresult = response.message;

                if (jsonresult == null || jsonresult == "") {
                    var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_011").text().trim().replace("*****", $("#hdfOperator").val());
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                    $target.focus();
                }

                else {
                    if (jsonresult.iN_ACTIVE_FLAG == 1) {
                        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_013").text().trim();
                        showMessageDialog(message, $target.attr("id"), WARNING);
                        $target.val("");
                        $target.focus();
                    }
                    else {
                        isOperatorActive = 0;
                        $target.val(jsonresult.yobina);
                    }

                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            })
    }
}
function IsDataAlreadyExist()
{
    var isSaved = false;
    var furnaceNo = $("#txtFurnaceNumber").val();
    var chuuban = $("#txtChuuban").val();
    var gyouban = $("#txtgyubaan").val();
    
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/IsProcessPlanAlreadyExist",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CHUUBAN: chuuban, GYOUBAN: gyouban, FURNACE_NO: furnaceNo })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var result = response;
            isSaved = response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return isSaved;
}