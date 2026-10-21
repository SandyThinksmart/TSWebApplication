var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var focusFieldID = "";
var isFinalSaveR1 = false;
var isFinalSaveR2 = false;
var isTempSaveR1 = false;
var isTempSaveR2 = false;
var operatorHardness1Modified = false;
var enterPressedOprHardness1 = false;
var operatorHardness2Modified = false;
var enterPressedOprHardness2 = false;

var operatorTempering1Modified = false;
var enterPressedOprTempering1 = false;
var operatorTempering2Modified = false;
var enterPressedOprTempering2 = false;

var machineHardness1Modified = false;
let enterPressedMachineHardness1 = false;

var machineHardness2Modified = false;
let enterPressedMachineHardness2 = false;

var machineTempering1Modified = false;
let enterPressedMachineTempering1 = false;

var machineTempering2Modified = false;
let enterPressedMachineTempering2 = false;

$(window).on("load", function () {
    
   // RemoveOperatorClass();
    SetDefaultSysDate(0);
    SetDecimalFormats();
    window.resizeTo(1480, 780);
    SetMask();
   
    LoadHardnessData();
    $("#txtFuranceNum1").focus();

});
function LoadHardnessData()
{
    SetTempType();
    SetMachineList();
    SetNextProcess();
    ManageSessionForUser();
    GetHardnessTemperingTempDetailsByUser();
   
    
}
function RemoveOperatorClass()
{
    $("#ddlHardnessOperator1").addClass("MandatoryText1").removeClass("Mandatory");
    $("#ddlTemperingOperator1").addClass("MandatoryText1").removeClass("Mandatory");
    $("#ddlHardnessOperator2").addClass("MandatoryText2").removeClass("Mandatory");
    $("#ddlTemperingOperator2").addClass("MandatoryText2").removeClass("Mandatory");
}
function SetDefaultSysDate(panelIndex) {
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

            if (panelIndex == 0) {
                $("#txtHardeningFuranceInDate1").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                $("#txtTemperingFuranceInDate1").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

                $("#txtHardeningFuranceInDate2").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                $("#txtTemperingFuranceInDate2").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

            } else if (panelIndex == 1) {
                $("#txtHardeningFuranceInDate1").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                $("#txtTemperingFuranceInDate1").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            } else if (panelIndex == 2)
            {
                $("#txtHardeningFuranceInDate2").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                $("#txtTemperingFuranceInDate2").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            }
            
        
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_TEMP_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function SetMask()
{
    $("#txtHardeningFuranceInDate1").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtHardeningFuranceOutDate1").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFuranceInDate1").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFuranceOutDate1").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });

    $("#txtHardeningFuranceInDate2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtHardeningFuranceOutDate2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFuranceInDate2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
    $("#txtTemperingFuranceOutDate2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:MM" });
}

function SetDecimalFormats() {
    $("#txt_h_NorthSideUp1_1, #txt_h_NorthSideUp1_2, #txt_h_NorthSideUp1_3, #txt_h_NorthSideUpCent1, #txt_t_NorthSideUp1_1, #txt_t_NorthSideUp1_2, #txt_t_NorthSideUp1_3, #txt_t_NorthSideUpCent1, #txt_h_NorthSideDown1_1, #txt_h_NorthSideDown1_2, #txt_h_NorthSideDown1_3, #txt_h_NorthSideDownCent1, #txt_t_NorthSideDown1_1, #txt_t_NorthSideDown1_2, #txt_t_NorthSideDown1_3, #txt_t_NorthSideDownCent1, #txt_h_SouthSideUp1_1, #txt_h_SouthSideUp1_2, #txt_h_SouthSideUp1_3, #txt_h_SouthSideUpCent1, #txt_t_SouthSideUp1_1, #txt_t_SouthSideUp1_2, #txt_t_SouthSideUp1_3, #txt_t_SouthSideUpCent1, #txt_h_SouthSideDown1_1, #txt_h_SouthSideDown1_2, #txt_h_SouthSideDown1_3, #txt_h_SouthSideDownCent1, #txt_t_SouthSideDown1_1, #txt_t_SouthSideDown1_2, #txt_t_SouthSideDown1_3, #txt_t_SouthSideDownCent1, #txt_h_NorthSideUp2_1, #txt_h_NorthSideUp2_2, #txt_h_NorthSideUp2_3, #txt_h_NorthSideUpCent2, #txt_t_NorthSideUp2_1, #txt_t_NorthSideUp2_2, #txt_t_NorthSideUp2_3, #txt_t_NorthSideUpCent2, #txt_h_NorthSideDown2_1, #txt_h_NorthSideDown2_2, #txt_h_NorthSideDown2_3, #txt_h_NorthSideDownCent2, #txt_t_NorthSideDown2_1, #txt_t_NorthSideDown2_2, #txt_t_NorthSideDown2_3, #txt_t_NorthSideDownCent2, #txt_h_SouthSideUp2_1, #txt_h_SouthSideUp2_2, #txt_h_SouthSideUp2_3, #txt_h_SouthSideUpCent2, #txt_t_SouthSideUp2_1, #txt_t_SouthSideUp2_2, #txt_t_SouthSideUp2_3, #txt_t_SouthSideUpCent2, #txt_h_SouthSideDown2_1, #txt_h_SouthSideDown2_2, #txt_h_SouthSideDown2_3, #txt_h_SouthSideDownCent2, #txt_t_SouthSideDown2_1, #txt_t_SouthSideDown2_2, #txt_t_SouthSideDown2_3, #txt_t_SouthSideDownCent2").keyup(function (event) {
    
        RestrictToValidDecimalFormat($(this), 4, 1);
    });

    $("#txtunit_1, #txtunit_2").keyup(function (event) {

        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtweight_1, #txtweight_2").keyup(function (event) {

        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    
    
    
}


function SetMachineList() {
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
            BindMachineDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMachineDropdown(jsonResult) {
   
    $("#ddlMachineNumber1").html("").append($('<option value=""></option>'));

    
    $("#ddlMachineNumber2").html("").append($('<option value=""></option>'));

    
    $("#ddlMachineNumber3").html("").append($('<option value=""></option>'));

    
    $("#ddlMachineNumber4").html("").append($('<option value=""></option>'));
    if (jsonResult.length > 0) {
        jsonResult = jsonResult.filter(row => row.iN_ACTIVE_FLAG == 0);
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlMachineNumber1_1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            $("#ddlMachineNumber1_2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            $("#ddlMachineNumber2_1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            $("#ddlMachineNumber2_2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

        });
    }
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindTempTypeDropdown(jsonResult) {
    $("#ddltempType1").html("");
    $("#ddltempType2").html("");
     $.each(jsonResult, function (key, jsonObj) {
        $("#ddltempType1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddltempType2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindNextProcessDropdown(jsonResult) {
   
    $("#ddlNextProcess1").html("");//.append($('<option value="0"></option>'));
    $("#ddlNextProcess2").html("");//.append($('<option value="0"></option>'));
   
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess1").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
            $("#ddlNextProcess2").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));

        });
    }
    if ($("#hdfCultureName").val() == "ja-JP") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess1").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
            $("#ddlNextProcess2").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
         
        });
    }
}


$("#btn_extract1").click(function (event) {
   
    var message = "";
    var $callFrom = $(this);
    var chuuban = $("#txtChuubanNum1").val().trim();
    var gyoban = $("#txtgyoubanNum1").val().trim();
    focusFieldID = "";

    if (!chuuban) {
        message =$("#txtChuubanNum1").attr("ErrorLabel");
        
            focusFieldID = "txtChuubanNum1";
        
    }
    if (!gyoban) {
        message = message + "<br>" + $("#txtgyoubanNum1").attr("ErrorLabel");
        if (focusFieldID == "") {
            focusFieldID = "txtgyoubanNum1";
        }
    }

    if (message !== "") {
        
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;

    }
    else {

        ValidateChubbanbtn_extract1(chuuban, gyoban, "R1Data");
        //GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyoban, "R1Data");
    }

});

function ValidateChubbanbtn_extract1(chuuban, gyouban, section) {
    var i = "";
    
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/ValidateChubban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
    
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message
            if (section == "R1Data") {

                i = 1;
            }
            else {
                i = 2;
            }
            var furnaceVal = $("#txtFuranceNum" + i).val().trim() ? $("#txtFuranceNum" + i).val().trim() : null;
            if (result.responce == 4) {
                ClearPanel($("#" + section));

                if (furnaceVal != null) {
                    $("#txtFuranceNum" + i).val(furnaceVal);
                }
                $("#txtChuubanNum" + i).val(chuuban);
                $("#txtgyoubanNum" + i).val(gyouban);

                DisableSaveButton();
                var Message = $("#lblErrorMessageE_HARDNESS_TEMP_009").text();
                ShowMessageDialog(Message, "txtChuubanNum" + i, WARNING);

            } else if (result.responce == 3) {

                ClearPanel($("#" + section));

                if (furnaceVal != null) {
                    $("#txtFuranceNum" + i).val(furnaceVal);
                }
                $("#txtChuubanNum" + i).val(chuuban);
                $("#txtgyoubanNum" + i).val(gyouban);
                // OrNo Not Started FromHandOver
                var Message = $("#lblErrorMessageE_HARDNESS_TEMP_016").text().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                ShowMessageDialog(Message, "txtChuubanNum" + i, WARNING);

            }
            else if (result.responce == 2) {
                DisableSaveButton();
                // Chuuban not found in system.
                $("#hdfldProcessPlanCD" + i).val("");
                $("#txtcustomer" + i).val("");
                $("#txtProdName" + i).val("");
                $("#txtCharge" + i).val("");
                $("#txtMaterialDim" + i).val("");
                $("#txtProductDim" + i).val("");
                $("#txtquantity" + i).val("");
                $("#txtweight" + i).val("");
                $("#txtRange" + i + "_1").val("");
                $("#txtRange" + i + "_2").val("");

                var errorMessage = $("#lblErrorMessageE_HARDNESS_TEMP_002").text();
                ShowMessageDialog(errorMessage, "txtChuubanNum", WARNING);
                return false;
            }
            else if (result.responce == 1) {

                GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyouban, "R1Data")
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

$(document).on('click', '#btn_extract2', function () {
    var message = "";

    var chuuban = $("#txtChuubanNum2").val().trim();
    var gyoban = $("#txtgyoubanNum2").val().trim();
    focusFieldID = "";



    if (!chuuban) {
        message = $("#txtChuubanNum2").attr("ErrorLabel");

        focusFieldID = "txtChuubanNum2";

    }
    if (!gyoban) {
        message = message + "<br>" + $("#txtgyoubanNum2").attr("ErrorLabel");
        if (focusFieldID == "") {
            focusFieldID = "txtgyoubanNum2";
        }
    }

    if (message !== "") {

        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;

    }
    else {
        ValidateChubbanbtn_extract2(chuuban, gyoban, "R2Data");
        //GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyoban, "R2Data");
    }
});

function ValidateChubbanbtn_extract2(chuuban, gyouban, section) {
    var i = "";

    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/ValidateChubban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message
            if (section == "R1Data") {

                i = 1;
            }
            else {
                i = 2;
            }
            var furnaceVal = $("#txtFuranceNum" + i).val().trim() ? $("#txtFuranceNum" + i).val().trim() : null;
            if (result.responce == 4) {
                ClearPanel($("#" + section));

                if (furnaceVal != null) {
                    $("#txtFuranceNum" + i).val(furnaceVal);
                }
                $("#txtChuubanNum" + i).val(chuuban);
                $("#txtgyoubanNum" + i).val(gyouban);

                DisableSaveButton();
                var Message = $("#lblErrorMessageE_HARDNESS_TEMP_009").text();
                ShowMessageDialog(Message, "txtChuubanNum" + i, WARNING);

            } else if (result.responce == 3) {

                ClearPanel($("#" + section));

                if (furnaceVal != null) {
                    $("#txtFuranceNum" + i).val(furnaceVal);
                }
                $("#txtChuubanNum" + i).val(chuuban);
                $("#txtgyoubanNum" + i).val(gyouban);
                // OrNo Not Started FromHandOver
                var Message = $("#lblErrorMessageE_HARDNESS_TEMP_016").text().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN );
                ShowMessageDialog(Message, "txtChuubanNum" + i, WARNING);

            }
            else if (result.responce == 2) {
                DisableSaveButton();
                $("#hdfldProcessPlanCD" + i).val("");
                $("#txtcustomer" + i).val("");
                $("#txtProdName" + i).val("");
                $("#txtCharge" + i).val("");
                $("#txtMaterialDim" + i).val("");
                $("#txtProductDim" + i).val("");
                $("#txtquantity" + i).val("");
                $("#txtweight" + i).val("");
                $("#txtRange" + i + "_1").val("");
                $("#txtRange" + i + "_2").val("");

                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_HARDNESS_TEMP_002").text();
                ShowMessageDialog(errorMessage, "txtChuubanNum", WARNING);
                return false;
            }
            else if (result.responce == 1) {

                GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyouban, "R2Data");
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyouban, section) {
    var i = "";
    var ajaxTime = new Date().getDate();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardnessTemperingDetailsByChuubanGyoban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var jsonResult = response.message;
            if (section == "R1Data") {

                i = 1;
            }
            else {
                i = 2;
            }
            if (jsonResult != null) {
                

                    EnableSaveButton();

                if (i == 1) {
                    $("#btn_extract1").attr("disabled", "disabled");
                } else
                {
                    $("#btn_extract2").attr("disabled", "disabled");

                }
                    $("#hdfldProcessPlanCD" + i).val(jsonResult.pP_PROCESS_PLAN_CD);
                    $("#txtcustomer" + i).val(jsonResult.pP_CUSTOMER_NAME);
                    $("#txtProdName" + i).val(jsonResult.pP_PROD_NAME_CUST);
                    $("#txtCharge" + i).val(jsonResult.pP_CHARGE);
                    //   $("#txtMetallicLabelNo" + i).val(jsonResult.pP_MATL_NAME_MAK);
                $("#txtMaterialDim" + i).val(jsonResult.pP_MATL_DIM_D_SHAPE);
                $("#txtProductDim" + i).val(jsonResult.pP_PROD_DIM_D_SHAPE_STRING);
                    $("#txtweight" + i).val(jsonResult.pP_QUANTITY);
                    $("#txtquantity" + i).val(jsonResult.pP_NO_OF_UNITS); 

                    $("#txtRange" + i + "_1").val(jsonResult.hardnesS_PATTERN2_MIN);
                    $("#txtRange" + i + "_2").val(jsonResult.hardnesS_PATTERN2_MAX);

           }
           else {
                DisableSaveButton();
                $("#hdfldProcessPlanCD" + i).val("");
               $("#txtcustomer" + i).val("");
               $("#txtProdName" + i).val("");
               $("#txtCharge" + i).val("");
               $("#txtMaterialDim" + i).val("");
               $("#txtProductDim" + i).val("");
               $("#txtquantity" + i).val("");
               $("#txtweight" + i).val("");
               $("#txtRange" + i +"_1").val("");
                $("#txtRange" + i + "_2").val("");

                var Message = $("#lblErrorMessageE_HARDNESS_TEMP_002").text();
                ShowMessageDialog(Message, "txtChuuban" + i, WARNING);
                return false;

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

$("#btnSaveTemp").click(function (event) {
    var request = [];
    var message = "";
    var orderNoFound = false;
    var chuuban1 = $("#txtChuubanNum1").val().trim();
    var gyoban1 = $("#txtgyoubanNum1").val().trim();


    var chuuban2 = $("#txtChuubanNum2").val().trim();
    var gyoban2 = $("#txtgyoubanNum2").val().trim();
    focusFieldID = "";


    if (!chuuban1 && !chuuban2) {
        message = message + "<br>" + $("#txtChuubanNum1").attr("ErrorLabel");
        if (focusFieldID == "") {
            focusFieldID = "txtChuubanNum1";
        }
    }
    if (!gyoban1 && !gyoban2) {
        message = message + "<br>" + $("#txtgyoubanNum1").attr("ErrorLabel");
        if (focusFieldID == "") {
            focusFieldID = "txtgyoubanNum1";
        }
    }

    if (message !== "") {

        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;

    }
    else {
       

        for (var i = 1; i < 3; i++) {

            var processPlanCD = $("#hdfldProcessPlanCD" + i).val();

            if (processPlanCD > 0) {
                var upperDetails = null;
                var lowerDetails = null;
                if (i == 1)
                {
                    isTempSaveR1 = true;
                    upperDetails = 1;

                }
                if (i == 2) {
                    isTempSaveR2 = true;
                    lowerDetails = 1;

                }
                
                var jsonData = ({

                    PROCESS_PLAN_CD: $("#hdfldProcessPlanCD" + i).val() ? $("#hdfldProcessPlanCD" + i).val() : null,
                    TEMP_HARDENING_TEMPERING_CD: $("#hdfldHardeningCD" + i).val() ? $("#hdfldHardeningCD" + i).val() :null,
                    FURNACE_NO: $("#txtFuranceNum" + i).val() ? $("#txtFuranceNum" + i).val() : null,
                    TABA_NO: $("#txtThaba" + i).val() ? $("#txtThaba" + i).val() : null,
                    //HARDENING_MACHINE_CD: $("#ddlMachineNumber" + i + "_1").val() ? $("#ddlMachineNumber" + i + "_1").val() : null,
                    //TEMPERING_MACHINE_CD: $("#ddlMachineNumber" + i + "_2").val() ? $("#ddlMachineNumber" + i + "_2").val() : null,
                    HARDENING_MACHINE_CD: $("#hdfHardnessMachine" + i).val() ? $("#hdfHardnessMachine" + i).val() : null,
                    TEMPERING_MACHINE_CD: $("#hdfTemperingMachine" + i).val() ? $("#hdfTemperingMachine" + i ).val() : null,
                    METALLIC_PLATE_CODE: $("#txtMetallicLabelNo" + i).val() ? $("#txtMetallicLabelNo" + i).val() : null,
                    NO_OF_UNITS: $("#txtunit_" + i).val() ? $("#txtunit_" + i).val() : null,
                    UNIT_TYPE: $("#hdfUnitType").val(),
                    WEIGHT: $("#txtweight_" + i).val() ? $("#txtweight_" + i).val() : null,
                    WATER_OIL_TYPE: $("#ddltempType" + i).val() ? $("#ddltempType" + i).val() : null,
                    TEMPERATURE_FURNACE: $("#txtTemprature" + i).val() ? $("#txtTemprature" + i).val() : null,
                    CONCENTRATION_FURNACE: $("#txtConcentration" + i).val() ? $("#txtConcentration" + i).val() : null,
                    FREQUENCY_FURNACE: $("#txtfrequency" + i).val() ? $("#txtfrequency" + i).val() : null,
                    C_FURNACE: $("#txtC_FURNACE" + i).val() ? $("#txtC_FURNACE" + i).val() : null,
                    MN_FURNACE: $("#txtMN_FURNACE" + i).val() ? $("#txtMN_FURNACE" + i).val() : null,
                    MO_FURNACE: $("#txtMO_FURNACE" + i).val() ? $("#txtMO_FURNACE" + i).val() : null,
                    CR_FURNACE: $("#txtCR_FURNACE" + i).val() ? $("#txtCR_FURNACE" + i).val() : null,
                    HARDNESS_MIN: $("#txtHardnessMin" + i).val() ? $("#txtHardnessMin" + i).val() : null,
                    HARDNESS_MAX: $("#txtHardnessMax" + i).val() ? $("#txtHardnessMax" + i).val() : null,
                    OPERATOR_CD_HARDENING: $("#hdfldHardnessOperator" + i).val() ? $("#hdfldHardnessOperator" + i).val() : null,
                    START_DATE_HARDENING: $("#txtHardeningFuranceInDate" + i).val() != "" ? (($("#txtHardeningFuranceInDate" + i).val().split(" ")[0] + "T" + $("#txtHardeningFuranceInDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    END_DATE_HARDENING: $("#txtHardeningFuranceOutDate" + i).val() != "" ? (($("#txtHardeningFuranceOutDate" + i).val().split(" ")[0] + "T" + $("#txtHardeningFuranceOutDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    TEMPERATURE_HARDENING: $("#txtTemperatureHardening" + i).val() ? $("#txtTemperatureHardening" + i).val() : null,
                    TIME_HARDENING: $("#txtTimeHardening" + i).val() ? $("#txtTimeHardening" + i).val() : null,
                    Q_HARDNESS_NORTH_UP_1: $("#txt_h_NorthSideUp" + i + "_1").val() ? $("#txt_h_NorthSideUp" + i + "_1").val() : null,
                    Q_HARDNESS_NORTH_UP_2: $("#txt_h_NorthSideUp" + i + "_2").val() ? $("#txt_h_NorthSideUp" + i + "_2").val() : null,
                    Q_HARDNESS_NORTH_UP_3: $("#txt_h_NorthSideUp" + i + "_3").val() ? $("#txt_h_NorthSideUp" + i + "_3").val() : null,
                    Q_HARDNESS_NORTH_UP_CEN: $("#txt_h_NorthSideUpCent" + i).val() ? $("#txt_h_NorthSideUpCent" + i).val() : null,
                    Q_HARDNESS_NORTH_DOWN_1: $("#txt_h_NorthSideDown" + i + "_1").val() ? $("#txt_h_NorthSideDown" + i + "_1").val() : null,
                    Q_HARDNESS_NORTH_DOWN_2: $("#txt_h_NorthSideDown" + i + "_2").val() ? $("#txt_h_NorthSideDown" + i + "_2").val() : null,
                    Q_HARDNESS_NORTH_DOWN_3: $("#txt_h_NorthSideDown" + i + "_3").val() ? $("#txt_h_NorthSideDown" + i + "_3").val() : null,
                    Q_HARDNESS_NORTH_DOWN_CEN: $("#txt_h_NorthSideDownCent" + i).val() ? $("#txt_h_NorthSideDownCent" + i).val() : null,
                    Q_HARDNESS_SOUTH_UP_1: $("#txt_h_SouthSideUp" + i + "_1").val() ? $("#txt_h_SouthSideUp" + i + "_1").val() : null,
                    Q_HARDNESS_SOUTH_UP_2: $("#txt_h_SouthSideUp" + i + "_2").val() ? $("#txt_h_SouthSideUp" + i + "_2").val() : null,
                    Q_HARDNESS_SOUTH_UP_3: $("#txt_h_SouthSideUp" + i + "_3").val() ? $("#txt_h_SouthSideUp" + i + "_3").val() : null,
                    Q_HARDNESS_SOUTH_UP_CEN: $("#txt_h_SouthSideUpCent" + i).val() ? $("#txt_h_SouthSideUpCent" + i).val() : null,
                    Q_HARDNESS_SOUTH_DOWN_1: $("#txt_h_SouthSideDown" + i + "_1").val() ? $("#txt_h_SouthSideDown" + i + "_1").val() : null,
                    Q_HARDNESS_SOUTH_DOWN_2: $("#txt_h_SouthSideDown" + i + "_2").val() ? $("#txt_h_SouthSideDown" + i + "_2").val() : null,
                    Q_HARDNESS_SOUTH_DOWN_3: $("#txt_h_SouthSideDown" + i + "_3").val() ? $("#txt_h_SouthSideDown" + i + "_3").val() : null,
                    Q_HARDNESS_SOUTH_DOWN_CEN: $("#txt_h_SouthSideDownCent" + i).val() ? $("#txt_h_SouthSideDownCent" + i).val() : null,
                    OPERATOR_CD_TEMPERING: $("#hdfldTemperingOperator" + i).val() ? $("#hdfldTemperingOperator" + i).val() : null,
                    START_DATE_TEMPERING: $("#txtTemperingFuranceInDate" + i).val() != "" ? (($("#txtTemperingFuranceInDate" + i).val().split(" ")[0] + "T" + $("#txtTemperingFuranceInDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    END_DATE_TEMPERING: $("#txtTemperingFuranceOutDate" + i).val() != "" ? (($("#txtTemperingFuranceOutDate" + i).val().split(" ")[0] + "T" + $("#txtTemperingFuranceOutDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    TEMPERATURE_TEMPERING: $("#txtTemperatureTempering" + i).val() ? $("#txtTemperatureTempering" + i).val() : null,
                    TIME_TEMPERING: $("#txtTimeTempering" + i).val() ? $("#txtTimeTempering" + i).val() : null,
                    T_HARDNESS_NORTH_UP_1: $("#txt_t_NorthSideUp" + i + "_1").val() ? $("#txt_t_NorthSideUp" + i + "_1").val() : null,
                    T_HARDNESS_NORTH_UP_2: $("#txt_t_NorthSideUp" + i + "_2").val() ? $("#txt_t_NorthSideUp" + i + "_2").val() : null,
                    T_HARDNESS_NORTH_UP_3: $("#txt_t_NorthSideUp" + i + "_3").val() ? $("#txt_t_NorthSideUp" + i + "_3").val() : null,
                    T_HARDNESS_NORTH_UP_CENT: $("#txt_t_NorthSideUpCent" + i).val() ? $("#txt_t_NorthSideUpCent" + i).val() : null,
                    T_HARDNESS_NORTH_DOWN_1: $("#txt_t_NorthSideDown" + i + "_1").val() ? $("#txt_t_NorthSideDown" + i + "_1").val() : null,
                    T_HARDNESS_NORTH_DOWN_2: $("#txt_t_NorthSideDown" + i + "_2").val() ? $("#txt_t_NorthSideDown" + i + "_2").val() : null,
                    T_HARDNESS_NORTH_DOWN_3: $("#txt_t_NorthSideDown" + i + "_3").val() ? $("#txt_t_NorthSideDown" + i + "_3").val() : null,
                    T_HARDNESS_NORTH_DOWN_CEN: $("#txt_t_NorthSideDownCent" + i).val() ? $("#txt_t_NorthSideDownCent" + i).val() : null,
                    T_HARDNESS_SOUTH_UP_1: $("#txt_t_SouthSideUp" + i + "_1").val() ? $("#txt_t_SouthSideUp" + i + "_1").val() : null,
                    T_HARDNESS_SOUTH_UP_2: $("#txt_t_SouthSideUp" + i + "_2").val() ? $("#txt_t_SouthSideUp" + i + "_2").val() : null,
                    T_HARDNESS_SOUTH_UP_3: $("#txt_t_SouthSideUp" + i + "_3").val() ? $("#txt_t_SouthSideUp" + i + "_3").val() : null,
                    T_HARDNESS_SOUTH_UP_CENT: $("#txt_t_SouthSideUpCent" + i).val() ? $("#txt_t_SouthSideUpCent" + i).val() : null,
                    T_HARDNESS_SOUTH_DOWN_1: $("#txt_t_SouthSideDown" + i + "_1").val() ? $("#txt_t_SouthSideDown" + i + "_1").val() : null,
                    T_HARDNESS_SOUTH_DOWN_2: $("#txt_t_SouthSideDown" + i + "_2").val() ? $("#txt_t_SouthSideDown" + i + "_2").val() : null,
                    T_HARDNESS_SOUTH_DOWN_3: $("#txt_t_SouthSideDown" + i + "_3").val() ? $("#txt_t_SouthSideDown" + i + "_3").val() : null,
                    T_HARDNESS_SOUTH_DOWN_CEN: $("#txt_t_SouthSideDownCent" + i).val() ? $("#txt_t_SouthSideDownCent" + i).val() : null,
                    HT_NEXT_PROCESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
                    REMARK: $("#txtRemark" + i).val() ? $("#txtRemark" + i).val() : null,
                    USER_ID: $("#hdfLoginId").val(),
                    UPPER_DETAILS: upperDetails,
                    LOWER_DETAILS: lowerDetails


                });
                request.push(jsonData);


            }
        }
       
        //if (orderNoFound == false) {
        //    var Message = $("#lblErrorMessageE_HARDNESS_TEMP_002").text();
        //    ShowMessageDialog(Message, "txtChuuban1" , WARNING);
        //    return false;
        //}
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/InsertHardnessTemperingTempDetails",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(request)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess == true) {
                    DisableSaveButton();
                    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
                    message = $("#lblSuccessMessageS_HARDNESS_TEMP_002").text().trim();
                    ShowMessageDialog(message, "btnInitialize", SUCCESS);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;
                isTempSaveR1 = false;
                isTempSaveR2 = false;
                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_TEMP_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }
});

$("#btnSave").click(function () {
    var focusField = "";
    var focusField1 = "";
    var focusField2 = "";
    var isQuenchingInDateValid = true;
    var isQuenchingOutDateValid = true;
    var isTemperingInDateValid = true;
    var istemperingOutDateValid = true;
    var errorMsg = "";
    var errorMsg1 = "";
    var errorMsg3 = "";
    var i = "";
    var className = "";
    var upperDetails = null;
    var lowerDetails = null;
   
    if ($("#rdoButton1").is(":checked")) {
        upperDetails = 1;
        className = "MandatoryText1";
        i = 1;
    } else
    {
        lowerDetails = 1;
        className = "MandatoryText2";
        i = 2;
       
    }
  
    if (!IsMandatoryFieldsFilled(className)) {
        return false;
    }

    if (!IsDuplicateThabaNo($("#hdfldProcessPlanCD" + i).val(), $("#txtThaba" + i).val())) {
        ShowMessageDialog($("#lblErrorMessageE_HARDNESS_TEMP_018").text().trim().replace("NNN", $("#txtThaba" + i).val()), "txtThaba" + i, WARNING);
        return false;
    }
   
    if (!CheckDate($("#txtHardeningFuranceInDate" + i))) {
        focusField = "txtHardeningFuranceInDate" + i;
        isQuenchingInDateValid = false;
    }
    if (!CheckDate($("#txtHardeningFuranceOutDate" + i))) {

        if (focusField == "") {
            focusField = "txtHardeningFuranceOutDate" + i;
        }

        isQuenchingOutDateValid = false;
    }

    if (!CheckDate($("#txtTemperingFuranceInDate" + i))) {
        if (focusField == "") {
            focusField = "txtTemperingFuranceInDate" + i;
        }
        isTemperingInDateValid = false;
    }
    if (!CheckDate($("#txtTemperingFuranceOutDate" + i))) {
        if (focusField == "") {
            focusField = "txtTemperingFuranceOutDate" + i;
        }
        istemperingOutDateValid = false;
    }

    if (isQuenchingInDateValid == false) {
        errorMsg = $("#lblErrorMessageE_HARDNESS_TEMP_004").text().trim(); 
    }

    if (isQuenchingOutDateValid == false) {

        if (errorMsg == "") {
            errorMsg = $("#lblErrorMessageE_HARDNESS_TEMP_005").text().trim();
        } else {
            errorMsg = errorMsg + "<br>"  + $("#lblErrorMessageE_HARDNESS_TEMP_005").text().trim();
        }
    }
    if (isTemperingInDateValid == false) {

        if (errorMsg == "") {
            errorMsg = $("#lblErrorMessageE_HARDNESS_TEMP_006").text().trim();
        } else {
            errorMsg = errorMsg + "<br>" + $("#lblErrorMessageE_HARDNESS_TEMP_006").text().trim();
        }
    }

    if (istemperingOutDateValid == false) {

        if (errorMsg == "") {
            errorMsg = $("#lblErrorMessageE_HARDNESS_TEMP_007").text().trim();
        } else {
            errorMsg = errorMsg + "<br>" + $("#lblErrorMessageE_HARDNESS_TEMP_007").text().trim();
        }
    }

    if (errorMsg != "") {
        ShowMessageDialog(errorMsg, focusField, WARNING);
        return false;
    }

    if (!IsDateDifferentFromRange($("#txtHardeningFuranceInDate" + i).val())) {
        errorMsg1 = $("#lblErrorMessageE_HARDNESS_TEMP_010").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";

        focusField1 = "txtHardeningFuranceInDate" + i;
    }

    if (!IsDateDifferentFromRange($("#txtHardeningFuranceOutDate" + i).val())) {

        if (errorMsg1 == "") {
            errorMsg1 = $("#lblErrorMessageE_HARDNESS_TEMP_011").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";
        }
        else {
            errorMsg1 = errorMsg1 + "<br>" + $("#lblErrorMessageE_HARDNESS_TEMP_011").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";
        }
        if (focusField1 == "") {
            focusField1 = "txtHardeningFuranceOutDate" + i;
        }

    }

    if (!IsDateDifferentFromRange($("#txtTemperingFuranceInDate" + i).val())) {

        if (errorMsg1 == "") {
            errorMsg1 = $("#lblErrorMessageE_HARDNESS_TEMP_012").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";
        } else {

            errorMsg1 = errorMsg1 + "<br>" + $("#lblErrorMessageE_HARDNESS_TEMP_012").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";
        }
        if (focusField1 == "") {
            focusField1 = "txtTemperingFuranceInDate" + i;
        }
    }

    if (!IsDateDifferentFromRange($("#txtTemperingFuranceOutDate" + i).val())) {
        if (errorMsg1 == "") {
            errorMsg1 = $("#lblErrorMessageE_HARDNESS_TEMP_013").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";
        } else {
            errorMsg1 = errorMsg1 + "<br>" + $("#lblErrorMessageE_HARDNESS_TEMP_013").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_TEMP_017").text().trim() + commonDateRange + ")";
        }
        if (focusField1 == "") {
            focusField1 = "txtTemperingFuranceOutDate" + i;
        }
    }

    if (errorMsg1 != "") {
        ShowMessageDialog(errorMsg1, focusField1, WARNING);
        return false;
    }
    errorMsg3 = "";
    if ($("#txtHardeningFuranceInDate" + i).val() >= $("#txtHardeningFuranceOutDate" + i).val())
    {

        errorMsg3 = $("#lblErrorMessageE_HARDNESS_TEMP_015").text().trim();
        focusField2 = "txtHardeningFuranceInDate" + i;
    }

    if ($("#txtTemperingFuranceInDate" + i).val() >= $("#txtTemperingFuranceOutDate" + i).val()) {

        if (errorMsg3 == "") {
            errorMsg3 = $("#lblErrorMessageE_HARDNESS_TEMP_014").text().trim();
        } else {
            errorMsg3 = errorMsg3 + "<br>" + $("#lblErrorMessageE_HARDNESS_TEMP_014").text().trim();
        }
        if (focusField2 == "") {
            focusField2 = "txtTemperingFuranceInDate" + i;
        }
    }
    if (errorMsg3 != "") {
        ShowMessageDialog(errorMsg3, focusField2, WARNING);
        return false;
    }
    
    
    if (!IsFurnaceNumberAlreadyExist($("#txtFuranceNum" + i).val())) {

        ShowMessageDialog($("#lblErrorMessageE_HARDNESS_TEMP_008").text().trim(), "txtFuranceNum"+i, WARNING);
        return false;
    }
            var processPlanCD = $("#hdfldProcessPlanCD" + i).val();

            if (processPlanCD > 0) {

                var jsonData = ({

                    PROCESS_PLAN_CD: $("#hdfldProcessPlanCD" + i).val() ? $("#hdfldProcessPlanCD" + i).val() : null,
                    FURNACE_NO: $("#txtFuranceNum" + i).val() ? $("#txtFuranceNum" + i).val() : null,
                    TABA_NO: $("#txtThaba" + i).val() ? $("#txtThaba" + i).val() : null,
                    //HARDENING_MACHINE_CD: $("#ddlMachineNumber" + i + "_1").val() ? $("#ddlMachineNumber" + i + "_1").val() : null,
                    //TEMPERING_MACHINE_CD: $("#ddlMachineNumber" + i + "_2").val() ? $("#ddlMachineNumber" + i + "_2").val() : null,
                    HARDENING_MACHINE_CD: $("#hdfHardnessMachine" + i).val() ? $("#hdfHardnessMachine" + i).val() : null,
                    TEMPERING_MACHINE_CD: $("#hdfTemperingMachine" + i).val() ? $("#hdfTemperingMachine" + i).val() : null,
                    METALLIC_PLATE_CODE: $("#txtMetallicLabelNo" + i).val() ? $("#txtMetallicLabelNo" + i).val() : null,
                    NO_OF_UNITS: $("#txtunit_" + i).val() ? $("#txtunit_" + i).val() : null,
                    UNIT_TYPE: $("#hdfUnitType").val(),
                    WEIGHT: $("#txtweight_" + i).val() ? $("#txtweight_" + i).val() : null,
                    WATER_OIL_TYPE: $("#ddltempType" + i).val() ? $("#ddltempType" + i).val() : null,
                    TEMPERATURE_FURNACE: $("#txtTemprature" + i).val() ? $("#txtTemprature" + i).val() : null,
                    CONCENTRATION_FURNACE: $("#txtConcentration" + i).val() ? $("#txtConcentration" + i).val() : null,
                    FREQUENCY_FURNACE: $("#txtfrequency" + i).val() ? $("#txtfrequency" + i).val() : null,
                    C_FURNACE: $("#txtC_FURNACE" + i).val() ? $("#txtC_FURNACE" + i).val() : null,
                    MN_FURNACE: $("#txtMN_FURNACE" + i).val() ? $("#txtMN_FURNACE" + i).val() : null,
                    MO_FURNACE: $("#txtMO_FURNACE" + i).val() ? $("#txtMO_FURNACE" + i).val() : null,
                    CR_FURNACE: $("#txtCR_FURNACE" + i).val() ? $("#txtCR_FURNACE" + i).val() : null,
                    HARDNESS_MIN: $("#txtHardnessMin" + i).val() ? $("#txtHardnessMin" + i).val() : null,
                    HARDNESS_MAX: $("#txtHardnessMax" + i).val() ? $("#txtHardnessMax" + i).val() : null,
                    OPERATOR_CD_HARDENING: $("#hdfldHardnessOperator" + i).val() ? $("#hdfldHardnessOperator" + i).val() : null,
                    START_DATE_HARDENING: $("#txtHardeningFuranceInDate" + i).val() != "" ? (($("#txtHardeningFuranceInDate" + i).val().split(" ")[0] + "T" + $("#txtHardeningFuranceInDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    END_DATE_HARDENING: $("#txtHardeningFuranceOutDate" + i).val() != "" ? (($("#txtHardeningFuranceOutDate" + i).val().split(" ")[0] + "T" + $("#txtHardeningFuranceOutDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    TEMPERATURE_HARDENING: $("#txtTemperatureHardening" + i).val() ? $("#txtTemperatureHardening" + i).val() : null,
                    TIME_HARDENING: $("#txtTimeHardening" + i).val() ? $("#txtTimeHardening" + i).val() : null,
                    Q_HARDNESS_NORTH_UP_1: $("#txt_h_NorthSideUp" + i + "_1").val() ? $("#txt_h_NorthSideUp" + i + "_1").val() : null,
                    Q_HARDNESS_NORTH_UP_2: $("#txt_h_NorthSideUp" + i + "_2").val() ? $("#txt_h_NorthSideUp" + i + "_2").val() : null,
                    Q_HARDNESS_NORTH_UP_3: $("#txt_h_NorthSideUp" + i + "_3").val() ? $("#txt_h_NorthSideUp" + i + "_3").val() : null,
                    Q_HARDNESS_NORTH_UP_CEN: $("#txt_h_NorthSideUpCent" + i).val() ? $("#txt_h_NorthSideUpCent" + i).val() : null,
                    Q_HARDNESS_NORTH_DOWN_1: $("#txt_h_NorthSideDown" + i + "_1").val() ? $("#txt_h_NorthSideDown" + i + "_1").val() : null,
                    Q_HARDNESS_NORTH_DOWN_2: $("#txt_h_NorthSideDown" + i + "_2").val() ? $("#txt_h_NorthSideDown" + i + "_2").val() : null,
                    Q_HARDNESS_NORTH_DOWN_3: $("#txt_h_NorthSideDown" + i + "_3").val() ? $("#txt_h_NorthSideDown" + i + "_3").val() : null,
                    Q_HARDNESS_NORTH_DOWN_CEN: $("#txt_h_NorthSideDownCent" + i).val() ? $("#txt_h_NorthSideDownCent" + i).val() : null,
                    Q_HARDNESS_SOUTH_UP_1: $("#txt_h_SouthSideUp" + i + "_1").val() ? $("#txt_h_SouthSideUp" + i + "_1").val() : null,
                    Q_HARDNESS_SOUTH_UP_2: $("#txt_h_SouthSideUp" + i + "_2").val() ? $("#txt_h_SouthSideUp" + i + "_2").val() : null,
                    Q_HARDNESS_SOUTH_UP_3: $("#txt_h_SouthSideUp" + i + "_3").val() ? $("#txt_h_SouthSideUp" + i + "_3").val() : null,
                    Q_HARDNESS_SOUTH_UP_CEN: $("#txt_h_SouthSideUpCent" + i).val() ? $("#txt_h_SouthSideUpCent" + i).val() : null,
                    Q_HARDNESS_SOUTH_DOWN_1: $("#txt_h_SouthSideDown" + i + "_1").val() ? $("#txt_h_SouthSideDown" + i + "_1").val() : null,
                    Q_HARDNESS_SOUTH_DOWN_2: $("#txt_h_SouthSideDown" + i + "_2").val() ? $("#txt_h_SouthSideDown" + i + "_2").val() : null,
                    Q_HARDNESS_SOUTH_DOWN_3: $("#txt_h_SouthSideDown" + i + "_3").val() ? $("#txt_h_SouthSideDown" + i + "_3").val() : null,
                    Q_HARDNESS_SOUTH_DOWN_CEN: $("#txt_h_SouthSideDownCent" + i).val() ? $("#txt_h_SouthSideDownCent" + i).val() : null,
                    OPERATOR_CD_TEMPERING: $("#hdfldTemperingOperator" + i).val() ? $("#hdfldTemperingOperator" + i).val() : null,
                    START_DATE_TEMPERING: $("#txtTemperingFuranceInDate" + i).val() != "" ? (($("#txtTemperingFuranceInDate" + i).val().split(" ")[0] + "T" + $("#txtTemperingFuranceInDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    END_DATE_TEMPERING: $("#txtTemperingFuranceOutDate" + i).val() != "" ? (($("#txtTemperingFuranceOutDate" + i).val().split(" ")[0] + "T" + $("#txtTemperingFuranceOutDate" + i).val().split(" ")[1] + ":00.000Z").replaceAll('/', '-')) : null,
                    TEMPERATURE_TEMPERING: $("#txtTemperatureTempering" + i).val() ? $("#txtTemperatureTempering" + i).val() : null,
                    TIME_TEMPERING: $("#txtTimeTempering" + i).val() ? $("#txtTimeTempering" + i).val() : null,
                    T_HARDNESS_NORTH_UP_1: $("#txt_t_NorthSideUp" + i + "_1").val() ? $("#txt_t_NorthSideUp" + i + "_1").val() : null,
                    T_HARDNESS_NORTH_UP_2: $("#txt_t_NorthSideUp" + i + "_2").val() ? $("#txt_t_NorthSideUp" + i + "_2").val() : null,
                    T_HARDNESS_NORTH_UP_3: $("#txt_t_NorthSideUp" + i + "_3").val() ? $("#txt_t_NorthSideUp" + i + "_3").val() : null,
                    T_HARDNESS_NORTH_UP_CENT: $("#txt_t_NorthSideUpCent" + i).val() ? $("#txt_t_NorthSideUpCent" + i).val() : null,
                    T_HARDNESS_NORTH_DOWN_1: $("#txt_t_NorthSideDown" + i + "_1").val() ? $("#txt_t_NorthSideDown" + i + "_1").val() : null,
                    T_HARDNESS_NORTH_DOWN_2: $("#txt_t_NorthSideDown" + i + "_2").val() ? $("#txt_t_NorthSideDown" + i + "_2").val() : null,
                    T_HARDNESS_NORTH_DOWN_3: $("#txt_t_NorthSideDown" + i + "_3").val() ? $("#txt_t_NorthSideDown" + i + "_3").val() : null,
                    T_HARDNESS_NORTH_DOWN_CEN: $("#txt_t_NorthSideDownCent" + i).val() ? $("#txt_t_NorthSideDownCent" + i).val() : null,
                    T_HARDNESS_SOUTH_UP_1: $("#txt_t_SouthSideUp" + i + "_1").val() ? $("#txt_t_SouthSideUp" + i + "_1").val() : null,
                    T_HARDNESS_SOUTH_UP_2: $("#txt_t_SouthSideUp" + i + "_2").val() ? $("#txt_t_SouthSideUp" + i + "_2").val() : null,
                    T_HARDNESS_SOUTH_UP_3: $("#txt_t_SouthSideUp" + i + "_3").val() ? $("#txt_t_SouthSideUp" + i + "_3").val() : null,
                    T_HARDNESS_SOUTH_UP_CENT: $("#txt_t_SouthSideUpCent" + i).val() ? $("#txt_t_SouthSideUpCent" + i).val() : null,
                    T_HARDNESS_SOUTH_DOWN_1: $("#txt_t_SouthSideDown" + i + "_1").val() ? $("#txt_t_SouthSideDown" + i + "_1").val() : null,
                    T_HARDNESS_SOUTH_DOWN_2: $("#txt_t_SouthSideDown" + i + "_2").val() ? $("#txt_t_SouthSideDown" + i + "_2").val() : null,
                    T_HARDNESS_SOUTH_DOWN_3: $("#txt_t_SouthSideDown" + i + "_3").val() ? $("#txt_t_SouthSideDown" + i + "_3").val() : null,
                    T_HARDNESS_SOUTH_DOWN_CEN: $("#txt_t_SouthSideDownCent" + i).val() ? $("#txt_t_SouthSideDownCent" + i).val() : null,
                    HT_NEXT_PROCESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
                    REMARK: $("#txtRemark" + i).val() ? $("#txtRemark" + i).val() : null,
                    UPPER_DETAILS: upperDetails,
                    LOWER_DETAILS: lowerDetails,
                    USER_ID: $("#hdfLoginId").val()

                });
           
            }
        
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/InsertHardnessTemperingDetails",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonData)
        }
        $.ajax(settings)
            .done(function (response) {
                
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess == true) {
                    if (i == 1) {
                        isFinalSaveR1 = true;
                    } else {
                        isFinalSaveR2 = true;
                    }
                    DisableSaveButton();
                    message = $("#lblSuccessMessageS_HARDNESS_TEMP_001").text().trim();
                    ShowMessageDialog(message, "btnInitialize", SUCCESS);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_TEMP_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
});
function IsDuplicateThabaNo(processCD, tabaNo)
{
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
            if (jsonResult.length==0) {
                returnVal = true;
            } else
            {
                var tabaList = jsonResult.map(item => item.tabA_NO);
                if (tabaList.includes(tabaNo)) {
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

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


function CheckDate(field) {
    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
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
function IsMandatoryFieldsFilled(className) {
   
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("." + className), function (index, mandatoryField) {
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
$("#btnInitialize").click(function () {

    DisableSaveButton();
    if ($("#rdoButton1").is(":checked"))
    {
        isTempSaveR1 = false;
        isFinalSaveR1 = false;
        $("#btn_extract1").removeAttr(DISABLED).removeClass(DISABLED);
        ClearPanel($("#R1Data"));
        SetDefaultSysDate(1);
        $("#txtFuranceNum1").focus();
    } else
    {
        isTempSaveR2 = false;
        isFinalSaveR2 = false;
        $("#btn_extract2").removeAttr(DISABLED).removeClass(DISABLED);
        ClearPanel($("#R2Data"));
        SetDefaultSysDate(2);
    }
   
   
   // $("#rdoButton1").prop('checked', true);
   
   
   
});
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
function EnableSaveButton() {
    $("#btnSaveTemp").removeAttr(DISABLED).removeClass(DISABLED);
    $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
}

function DisableSaveButton() {

    $("#btnSaveTemp").attr("disabled", "disabled");
    $("#btnSave").attr("disabled", "disabled");

}
function IsFurnaceNumberAlreadyExist(furnaceNo)
{
    var returnVal = false;

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/CheckFurnaceIsExist?furnaceNo=" + furnaceNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({FURNACE_NO: furnaceNo, CHUUBAN: chuuban, GYOUBAN: gyouban })
       }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess == true) {
                returnVal = true;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_TEMP_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnVal;
}
function IsDateDifferentFromRange(dateVal)
{
    
    var returnval = false;
   
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/IsValidDateRange",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DATERANGE: commonDateRange, RANGE_DATETIME: (dateVal.split(" ")[0] + "T" + dateVal.split(" ")[1] + ":00.000Z").replaceAll('/', '-') })
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
            var message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnval;
}

function GetHardnessTemperingTempDetailsByUser() {
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardnessTemperingTempDetailsByUser?userID=" + $("#hdfLoginId").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            if (result.length > 0) {
                EnableSaveButton();
                $("#btnSaveTemp").attr("DISABLED", "DISABLED").addClass("DISABLED");
                $.each(result, function (k, jsonResult) {

             if (jsonResult.procesS_PLAN_CMPL_DATE != null) {


             } else if (jsonResult.procesS_PLAN_START_DATE == null)
             {
                   
                }
                else
                {
                 
                    var i = 0;
                    if (jsonResult.uppeR_DETAILS == 1)
                    {
                        $("#btn_extract1").attr("disabled", "disabled");
                        isTempSaveR1 = true;
                        i = 1;
                    } else if (jsonResult.loweR_DETAILS == 1) {
                        $("#btn_extract2").attr("disabled", "disabled");
                        isTempSaveR2 = true;
                        i = 2;
                    }

                     $("#hdfldProcessPlanCD" + i).val(jsonResult.procesS_PLAN_CD);
                     $("#hdfldHardeningCD" + i).val(jsonResult.temP_HARDENING_TEMPERING_CD); 

                     $("#txtcustomer" + i).val(jsonResult.pP_CUSTOMER_NAME);
                     $("#txtProdName" + i).val(jsonResult.pP_PROD_NAME_CUST);
                     $("#txtCharge" + i).val(jsonResult.pP_CHARGE);
                     $("#txtMaterialDim" + i).val(jsonResult.pP_MATL_DIM_D_SHAPE);
                     $("#txtProductDim" + i).val(jsonResult.pP_PROD_DIM_D_SHAPE_STRING);
                     $("#txtweight" + i).val(jsonResult.pP_QUANTITY);
                     $("#txtquantity" + i).val(jsonResult.pP_NO_OF_UNITS);
                     $("#txtRange" + i + "_1").val(jsonResult.hardnesS_PATTERN2_MIN);
                     $("#txtRange" + i + "_2").val(jsonResult.hardnesS_PATTERN2_MAX);
                     $("#txtFuranceNum" + i).val(jsonResult.furnacE_NO);
                     $("#txtThaba" + i).val(jsonResult.tabA_NO);
                    $("#txtChuubanNum" + i).val(jsonResult.pP_CHUUBAN);
                    $("#txtgyoubanNum" + i).val(jsonResult.pP_GYOUBAN);

                    //$("#ddlMachineNumber" + i + "_1").val(jsonResult.hardeninG_MACHINE_CD);
                 // $("#ddlMachineNumber" + i + "_2").val(jsonResult.temperinG_MACHINE_CD);
                 $("#txtHardnessMachine" + i).val(jsonResult.hardeninG_MACHINE_NAME);   
                 $("#hdfHardnessMachine" + i).val(jsonResult.hardeninG_MACHINE_CD);



                    $("#txtMetallicLabelNo" + i).val(jsonResult.metalliC_PLATE_CODE);
                    $("#txtunit_" + i).val(jsonResult.nO_OF_UNITS);
                    $("#txtweight_" + i).val(jsonResult.weight);
                    $("#ddltempType" + i).val(jsonResult.wateR_OIL_TYPE);
                    $("#txtTemprature" + i).val(jsonResult.temperaturE_FURNACE);
                    $("#txtConcentration" + i).val(jsonResult.concentratioN_FURNACE);
                    $("#txtfrequency" + i).val(jsonResult.frequencY_FURNACE);
                    $("#txtC_FURNACE" + i).val(jsonResult.c_FURNACE);
                    $("#txtMN_FURNACE" + i).val(jsonResult.mN_FURNACE);
                    $("#txtMO_FURNACE" + i).val(jsonResult.mO_FURNACE);
                    $("#txtCR_FURNACE" + i).val(jsonResult.cR_FURNACE);
                    $("#txtHardnessMin" + i).val(jsonResult.hardnesS_MIN);
                    $("#txtHardnessMax" + i).val(jsonResult.hardnesS_MAX);

                 $("#txtHardnessOperator" + i).val(jsonResult.hardeninG_OPERATOR_NAME);   
                 $("#hdfldHardnessOperator" + i).val(jsonResult.operatoR_CD_HARDENING);
                 
                 //  SetOperatorDropdown($("#ddlHardnessOperator" + i), jsonResult.operatoR_CD_HARDENING);
                     
                    
                        if (jsonResult.starT_DATE_HARDENING_STRING != "0001/01/01 00:00")
                        {
                            $("#txtHardeningFuranceInDate" + i).val($.format.date(new Date(jsonResult.starT_DATE_HARDENING_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                        }
                        if (jsonResult.enD_DATE_HARDENING_STRING != "0001/01/01 00:00")
                        {
                            $("#txtHardeningFuranceOutDate" + i).val($.format.date(new Date(jsonResult.enD_DATE_HARDENING_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                        }

                    $("#txtTemperatureHardening" + i).val(jsonResult.temperaturE_HARDENING);

                    $("#txtTimeHardening" + i).val(jsonResult.timE_HARDENING);
                    $("#txt_h_NorthSideUp" + i + "_1").val(jsonResult.q_HARDNESS_NORTH_UP_1);
                    $("#txt_h_NorthSideUp" + i + "_2").val(jsonResult.q_HARDNESS_NORTH_UP_2);
                    $("#txt_h_NorthSideUp" + i + "_3").val(jsonResult.q_HARDNESS_NORTH_UP_3);
                    $("#txt_h_NorthSideUpCent" + i).val(jsonResult.q_HARDNESS_NORTH_UP_CEN);   
                    $("#txt_h_NorthSideDown" + i + "_1").val(jsonResult.q_HARDNESS_NORTH_DOWN_1);
                    $("#txt_h_NorthSideDown" + i + "_2").val(jsonResult.q_HARDNESS_NORTH_DOWN_2);
                    $("#txt_h_NorthSideDown" + i + "_3").val(jsonResult.q_HARDNESS_NORTH_DOWN_3);
                    $("#txt_h_NorthSideDownCent" + i).val(jsonResult.q_HARDNESS_NORTH_DOWN_CEN);
                    $("#txt_h_SouthSideUp" + i + "_1").val(jsonResult.q_HARDNESS_SOUTH_UP_1);
                    $("#txt_h_SouthSideUp" + i + "_2").val(jsonResult.q_HARDNESS_SOUTH_UP_2);
                    $("#txt_h_SouthSideUp" + i + "_3").val(jsonResult.q_HARDNESS_SOUTH_UP_3);
                    $("#txt_h_SouthSideUpCent" + i).val(jsonResult.q_HARDNESS_SOUTH_UP_CEN);
                    $("#txt_h_SouthSideDown" + i + "_1").val(jsonResult.q_HARDNESS_SOUTH_DOWN_1);
                    $("#txt_h_SouthSideDown" + i + "_2").val(jsonResult.q_HARDNESS_SOUTH_DOWN_2);
                    $("#txt_h_SouthSideDown" + i + "_3").val(jsonResult.q_HARDNESS_SOUTH_DOWN_3);
                        $("#txt_h_SouthSideDownCent" + i).val(jsonResult.q_HARDNESS_SOUTH_DOWN_CEN);

                 $("#txtTemperingOperator" + i).val(jsonResult.temperinG_OPERATOR_NAME); 
                 $("#hdfldTemperingOperator" + i).val(jsonResult.operatoR_CD_TEMPERING);

                 $("#txtTemperingMachine" + i).val(jsonResult.temperinG_MACHINE_NAME);
                 $("#hdfTemperingMachine" + i).val(jsonResult.temperinG_MACHINE_CD);
                       // SetOperatorDropdown($("#ddlTemperingOperator" + i), jsonResult.operatoR_CD_TEMPERING);
                
                        if (jsonResult.starT_DATE_TEMPERING_STRING != "0001/01/01 00:00")
                        {
                            $("#txtTemperingFuranceInDate" + i).val($.format.date(new Date(jsonResult.starT_DATE_TEMPERING_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                        }

                        if (jsonResult.enD_DATE_TEMPERING_STRING != "0001/01/01 00:00")
                        {
                            $("#txtTemperingFuranceOutDate" + i).val($.format.date(new Date(jsonResult.enD_DATE_TEMPERING_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                        }

                    $("#txtTemperatureTempering" + i).val(jsonResult.temperaturE_TEMPERING);
                    $("#txtTimeTempering" + i).val(jsonResult.timE_TEMPERING);
                    $("#txt_t_NorthSideUp" + i + "_1").val(jsonResult.t_HARDNESS_NORTH_UP_1);
                    $("#txt_t_NorthSideUp" + i + "_2").val(jsonResult.t_HARDNESS_NORTH_UP_2);
                    $("#txt_t_NorthSideUp" + i + "_3").val(jsonResult.t_HARDNESS_NORTH_UP_3);
                    $("#txt_t_NorthSideUpCent" + i).val(jsonResult.t_HARDNESS_NORTH_UP_CENT);
                    $("#txt_t_NorthSideDown" + i + "_1").val(jsonResult.t_HARDNESS_NORTH_DOWN_1);
                    $("#txt_t_NorthSideDown" + i + "_2").val(jsonResult.t_HARDNESS_NORTH_DOWN_2);
                    $("#txt_t_NorthSideDown" + i + "_3").val(jsonResult.t_HARDNESS_NORTH_DOWN_3);
                    $("#txt_t_NorthSideDownCent" + i).val(jsonResult.t_HARDNESS_NORTH_DOWN_CEN);
                    $("#txt_t_SouthSideUp" + i + "_1").val(jsonResult.t_HARDNESS_SOUTH_UP_1);
                    $("#txt_t_SouthSideUp" + i + "_2").val(jsonResult.t_HARDNESS_SOUTH_UP_2);
                    $("#txt_t_SouthSideUp" + i + "_3").val(jsonResult.t_HARDNESS_SOUTH_UP_3);
                    $("#txt_t_SouthSideUpCent" + i).val(jsonResult.t_HARDNESS_SOUTH_UP_CENT);
                    $("#txt_t_SouthSideDown" + i + "_1").val(jsonResult.t_HARDNESS_SOUTH_DOWN_1);
                    $("#txt_t_SouthSideDown" + i + "_2").val(jsonResult.t_HARDNESS_SOUTH_DOWN_2);
                    $("#txt_t_SouthSideDown" + i + "_3").val(jsonResult.t_HARDNESS_SOUTH_DOWN_3);
                    $("#txt_t_SouthSideDownCent" + i).val(jsonResult.t_HARDNESS_SOUTH_DOWN_CEN);
                    $("#ddlNextProcess" + i).val(jsonResult.hT_NEXT_PROCESS);
                    $("#txtRemark" + i).val(jsonResult.remark); 
            
                }
                });
                
            } else {
               // disableSaveButton();
          
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_TEMP_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

$('#txtChuubanNum1, #txtChuubanNum2').keypress(function (e) {
    var orderNo = e.target.value ? e.target.value : null;
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
                let i = 0;
                if (e.target.id == 'txtChuubanNum1') {
                    i = 1;
                } else {
                    i = 2;
                }
                
                if (response.isSuccess) {
                    $('#txtChuubanNum'+i).val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtgyoubanNum'+i).val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#btn_extract'+i).focus();
                } else {
                    $('#txtChuubanNum' + i).val("");
                    $('#txtgyoubanNum' + i).val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_TEMP_001").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});
$("#ddlNextProcess2").focusout(function () {
    $("#btnInitialize").focus();
});
$("#rdoButton1").click(function () {
    if (!($('#btn_extract1').attr('disabled'))) {
        DisableSaveButton();
    } else {
        if (isFinalSaveR1 == true) {
            DisableSaveButton();
        } else if (isFinalSaveR1 == false && isTempSaveR1 == true) {
            $("#btnSaveTemp").attr("disabled", "disabled");
            $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
        } else {
            EnableSaveButton();
        }
    }
});
$("#rdoButton2").click(function () {
    if (!($('#btn_extract2').attr('disabled'))) {
        DisableSaveButton();
    } else {
        if (isFinalSaveR2 == true) {
            DisableSaveButton();
        } else if (isFinalSaveR2 == false && isTempSaveR2 == true) {
            $("#btnSaveTemp").attr("disabled", "disabled");
            $("#btnSave").removeAttr(DISABLED).removeClass(DISABLED);
        } else
        {
            EnableSaveButton();
        }
    }
});
function GetOperatorNameByCD($target) {

    var ajaxTime = new Date().getTime();

    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_HARDNESS_TEMP_019").text().trim().replace("*****", $("#lblOperator").text());

        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageE_HARDNESS_TEMP_021").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageE_HARDNESS_TEMP_021").text().trim();
        ShowMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

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


            var operator = response.message;

            if (operator == null || operator == "") {
                var message = $("#lblErrorMessageE_HARDNESS_TEMP_020").text().trim().replace("*****", $("#lblOperator").text());
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }
            //employee is inactive.
            else if (operator.iN_ACTIVE_FLAG == 1) {

                var message = $("#lblErrorMessageE_HARDNESS_TEMP_022").text().trim();
                ShowMessageDialog(message, $target.attr("id"), WARNING);
                $target.val("");
            }

            else {
                $target.val(operator.yobina);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $("#lblErrorMessageE_HARDNESS_TEMP_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        })
}


$('#txtHardnessOperator1')
    .on('focus', function () {
        operatorHardness1Modified = false;
    })
    .on('input', function () {
        operatorHardness1Modified = true;
    });

$('#txtHardnessOperator1').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);
    if (keycode == 13 && operator && operatorHardness1Modified == true) {

        $("#hdfldHardnessOperator1").val(operatorNumber);
        enterPressedOprHardness1 = true;
        GetOperatorNameByCD($(this));
        $("#txtHardeningFuranceInDate1").focus();

    }
    if (e.type === 'focusout' && operator && operatorHardness1Modified == true) {
        if (enterPressedOprHardness1) {

            enterPressedOprHardness1 = false; // Reset the flag
            return false;
        }
        $("#hdfldHardnessOperator1").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtHardeningFuranceInDate1").focus();
    }
    enterPressedOprHardness1 = false; // Reset the flag
});

$('#txtHardnessOperator2')
    .on('focus', function () {
        operatorHardness2Modified = false;
    })
    .on('input', function () {
        operatorHardness2Modified = true;
    });

$('#txtHardnessOperator2').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);
    if (keycode == 13 && operator && operatorHardness2Modified == true) {

        $("#hdfldHardnessOperator2").val(operatorNumber);
        enterPressedOprHardness2 = true;
        GetOperatorNameByCD($(this));
        $("#txtHardeningFuranceInDate2").focus();

    }
    if (e.type === 'focusout' && operator && operatorHardness2Modified == true) {
        if (enterPressedOprHardness2) {

            enterPressedOprHardness2 = false; // Reset the flag
            return false;
        }
        $("#hdfldHardnessOperator2").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtHardeningFuranceInDate2").focus();
    }
    enterPressedOprHardness2 = false; // Reset the flag
});

$('#txtTemperingOperator1')
    .on('focus', function () {
        operatorTempering1Modified = false;
    })
    .on('input', function () {
        operatorTempering1Modified = true;
    });

$('#txtTemperingOperator1').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);
    if (keycode == 13 && operator && operatorTempering1Modified == true) {

        $("#hdfldTemperingOperator1").val(operatorNumber);
        enterPressedOprTempering1 = true;
        GetOperatorNameByCD($(this));
        $("#txtTemperingFuranceInDate1").focus();

    }
    if (e.type === 'focusout' && operator && operatorTempering1Modified == true) {
        if (enterPressedOprTempering1) {

            enterPressedOprTempering1 = false; // Reset the flag
            return false;
        }
        $("#hdfldTemperingOperator1").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtTemperingFuranceInDate1").focus();
    }
    enterPressedOprTempering1 = false; // Reset the flag
});

$('#txtTemperingOperator2')
    .on('focus', function () {
        operatorTempering2Modified = false;
    })
    .on('input', function () {
        operatorTempering2Modified = true;
    });

$('#txtTemperingOperator2').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);
    if (keycode == 13 && operator && operatorTempering2Modified == true) {

        $("#hdfldTemperingOperator2").val(operatorNumber);
        enterPressedOprTempering2 = true;
        GetOperatorNameByCD($(this));
        $("#txtTemperingFuranceInDate2").focus();

    }
    if (e.type === 'focusout' && operator && operatorTempering2Modified == true) {
        if (enterPressedOprTempering2) {

            enterPressedOprTempering2 = false; // Reset the flag
            return false;
        }
        $("#hdfldTemperingOperator2").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtTemperingFuranceInDate2").focus();
    }
    enterPressedOprTempering2 = false; // Reset the flag
});


function getMachineNamebyCD($target) {


    var ajaxTime = new Date().getTime();
    var machineNumber = $target.val().trim();
    var machineval = machineNumber.substring(0, 3);
    var machineName = machineval.toUpperCase();
    var machineCD = machineNumber.substring(3, 7);
    
    if (machineName != MACHINENAME) {
        var message = $("#lblErrorMessageE_HARDNESS_TEMP_019").text().trim().replace("*****", $("#lblMachine").text());//pending//barcode incorrect

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    if (machineCD == "") {
        var message = $("#lblErrorMessageE_HARDNESS_ScanMachineRequired").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (machineCD.length > 4) {
        var message = $("#lblErrorMessageE_HARDNESS_TEMP_023").text().trim();//pms machine not found
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

   
    else {
        var jsonData = "{machineCD: '" + machineCD + "'}";
        var settings = {
            async: false,
            crossDomain: true,
            dataType: "json",
            url: APIURL + "/HardnessTempering/GetMachineNamebyCD?machineCD=" + parseInt(machineCD),
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)

            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var machineName = response.message;

                // machine not found.
                if (machineName.key == null || machineName.key == "") {
                    var message = $("#lblErrorMessage_HARDNESS_TEMP_024").text().trim().replace("*****", $("#lblMachine").text());//pending//barcode not found
                    //var message = $("#lblErrorMessageE_HIKI_ENTRY_013").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val("");
                }
                //machine is inactive.
                else if (machineName.value == "inactive") {

                    var message = $("#lblErrorMessageE_HARDNESS_TEMP_025").text().trim();//machine is inactive
                    
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val("");
                }
                else {
                    $target.val(machineName.key);

                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_Cold_Drawing_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }


}


$('#txtHardnessMachine1')
    .on('focus', function () {
        machineHardness1Modified = false;
    })
    .on('input', function () {
        machineHardness1Modified = true;
    });
$('#txtHardnessMachine1').on('input keydown focusout', function (e) {
    debugger;
    var machine = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);

    if (e.which == 13 && machine && machineHardness1Modified == true) {
       
        if ($('#txtHardnessMachine1').val() == "" || $('#txtHardnessMachine1').val() == null) {            

            var message = $("#lblErrorMessageE_HARDNESS_ScanMachineRequired").text().trim();
            showMessageDialog(message, "txtHardnessMachine1", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfHardnessMachine1").val(machineNumber);
            enterPressedMachineHardness1 = true;
            getMachineNamebyCD($(this));
            $("#txtHardnessOperator1").focus();

        }
    }

    if (e.type === 'focusout' && machine && machineHardness1Modified == true) {
        if (enterPressedMachineHardness1) {

            enterPressedMachineHardness1 = false; // Reset the flag
            return false;
        }
        
        var machineCD = $(this).val().trim();
        var machineName = machineCD.substring(0, 3);
        var machineNumber = machineCD.substring(3, 7);
        $("#hdfHardnessMachine1").val(machineNumber);
        getMachineNamebyCD($(this));
        $("#txtHardnessOperator1").focus();
    }
    enterPressedMachineHardness1 = false; // Reset the flag
});




$('#txtHardnessMachine2')
    .on('focus', function () {       
        machineHardness2Modified = false;
    })
    .on('input', function () {
        machineHardness2Modified = true;
    });
$('#txtHardnessMachine2').on('input keydown focusout', function (e) {
    debugger;
    var machine = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);

    if (e.which == 13 && machine && machineHardness2Modified == true) {

        if ($('#txtHardnessMachine2').val() == "" || $('#txtHardnessMachine2').val() == null) {

            var message = $("#lblErrorMessageE_HARDNESS_ScanMachineRequired").text().trim();
            showMessageDialog(message, "txtHardnessMachine2", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfHardnessMachine2").val(machineNumber);
            enterPressedMachineHardness2 = true;
            getMachineNamebyCD($(this));
            $("#txtHardnessOperator2").focus();

        }
    }

    if (e.type === 'focusout' && machine && machineHardness2Modified == true) {
        if (enterPressedMachineHardness2) {

            enterPressedMachineHardness2 = false; // Reset the flag
            return false;
        }

        var machineCD = $(this).val().trim();
        var machineName = machineCD.substring(0, 3);
        var machineNumber = machineCD.substring(3, 7);
        $("#hdfHardnessMachine2").val(machineNumber);
        getMachineNamebyCD($(this));
        $("#txtHardnessOperator2").focus();
    }
    enterPressedMachineHardness2 = false; // Reset the flag
});



$('#txtTemperingMachine1')
    .on('focus', function () {
        machineTempering1Modified = false;
    })
    .on('input', function () {
        machineTempering1Modified = true;
    });
$('#txtTemperingMachine1').on('input keydown focusout', function (e) {
    debugger;
    var machine = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);

    if (e.which == 13 && machine && machineTempering1Modified == true) {

        if ($('#txtTemperingMachine1').val() == "" || $('#txtTemperingMachine1').val() == null) {

            var message = $("#lblErrorMessageE_HARDNESS_ScanMachineRequired").text().trim();
            showMessageDialog(message, "txtTemperingMachine1", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfTemperingMachine1").val(machineNumber);
            enterPressedMachineTempering1 = true;
            getMachineNamebyCD($(this));
            $("#txtTemperingOperator1").focus();

        }
    }

    if (e.type === 'focusout' && machine && machineTempering1Modified == true) {
        if (enterPressedMachineTempering1) {

            enterPressedMachineTempering1 = false; // Reset the flag
            return false;
        }

        var machineCD = $(this).val().trim();
        var machineName = machineCD.substring(0, 3);
        var machineNumber = machineCD.substring(3, 7);
        $("#hdfTemperingMachine1").val(machineNumber);
        getMachineNamebyCD($(this));
        $("#txtTemperingOperator1").focus();
    }
    enterPressedMachineTempering1 = false; // Reset the flag
});


$('#txtTemperingMachine2')
    .on('focus', function () {
        machineTempering2Modified = false;
    })
    .on('input', function () {
        machineTempering2Modified = true;
    });
$('#txtTemperingMachine2').on('input keydown focusout', function (e) {
    debugger;
    var machine = e.target.value ? e.target.value : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);

    if (e.which == 13 && machine && machineTempering2Modified == true) {

        if ($('#txtTemperingMachine2').val() == "" || $('#txtTemperingMachine2').val() == null) {

            var message = $("#lblErrorMessageE_HARDNESS_ScanMachineRequired").text().trim();
            showMessageDialog(message, "txtTemperingMachine2", WARNING);
        } else {
            var machineCD = $(this).val().trim();
            var machineName = machineCD.substring(0, 3);
            var machineNumber = machineCD.substring(3, 7);
            $("#hdfTemperingMachine2").val(machineNumber);
            enterPressedMachineTempering2 = true;
            getMachineNamebyCD($(this));
            $("#txtTemperingOperator2").focus();

        }
    }

    if (e.type === 'focusout' && machine && machineTempering2Modified == true) {
        if (enterPressedMachineTempering2) {

            enterPressedMachineTempering2 = false; // Reset the flag
            return false;
        }

        var machineCD = $(this).val().trim();
        var machineName = machineCD.substring(0, 3);
        var machineNumber = machineCD.substring(3, 7);
        $("#hdfTemperingMachine2").val(machineNumber);
        getMachineNamebyCD($(this));
        $("#txtTemperingOperator2").focus();
    }
    enterPressedMachineTempering2 = false; // Reset the flag
});