
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd
var chuuban = null;
var gyouban = null;
var isOprExists = false;
userName = "";
var registeredThabaNumbersAndDecision = {};
var fICombinedDecisions = {};
let CombinedDecisionArr = [];
let SparkDecisionArr = [];
var combinedDecisionOK = "";
var operatorCD = "";
var m_hardnessStd = null;
let validMinVal = null;
let validMaxVal = null;
var FIResultOk = $("#hdfFIResultOk").val();
var FIResultNg = $("#hdfFIResultNg").val();
var FIResultUntested = $("#hdfFIResultUntested").val();
var FIResultSparkUntested = $("#hdfFIResultSparkUntested").val();
var FISparkUntested = $("#hdfFISparkUntested").val();
var FISparkOk = $("#hdfFISparkOk").val();
var FISparkNg = $("#hdfFISparkNg").val();

function DisableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function EnableEditDeleteButtons() {
    HardnessGridSearch(null);
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
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

function SwitchTab($tabFrom, $tabTo) {

    $tabTo.click();

    EnableEditDeleteButtons();

}
function GetFICombinedDecisionDetails(chuuban, gyouban) {
    var returnVal = true;
    CombinedDecisionArr = [];
    SparkDecisionArr = [];
    //var displayNote = false;

    var jsonData = JSON.stringify({
        CHUUBAN: chuuban,
        GYOUBAN: gyouban
    });
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetFICombinedDecisionDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData

    }

    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            fICombinedDecisions = jsonResult;
            debugger;
            if (jsonResult.length > 0) {
                debugger;
                document.getElementById('FIDoneWarning').style.display = 'block';

                for (i = 0; i < fICombinedDecisions.length; i++) {
                    CombinedDecisionArr.push(fICombinedDecisions[i].combineD_DECISION);
                    SparkDecisionArr.push(fICombinedDecisions[i].fI_SPARK_DECISION);
                }

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnVal;

}

function SetHardnessDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    SwitchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));

    $("#hfId_MessageCD").val(jsonResult.msG_CD);
    $("#hfId_HardnessCD").val(jsonResult.mI_HARDNESS_CD);
    $("#hfId_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.createD_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.updateD_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txt_HostName").val(jsonResult.pP_NYUKA_CD);
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOUBAN);
    $("#txt_Chuuban1").val(jsonResult.pP_CHUUBAN_GYOUBAN);
    $("#txtDeliveryDateT1").val($.format.date(new Date(jsonResult.pP_DELIVERY_DATE_STRING), "yyyy/MM/dd"));
    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);

    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
    }
    if (parseInt(jsonResult.pP_UNIT_WT_KUBUN_STRING) != null) {
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
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);

    }
    else {
        $("#txtLengthToleranceT1").val('');

    }
    //$("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);            //text(jsonResult.PP_NO_OF_UNITS + '' + jsonResult.PP_UNIT);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);


    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
    //Hardness Details

    if (jsonResult.hardnesS_PATTERN1_CODE > 0) {
        $("#txtHardnessData1").val(jsonResult.hardnesS_PATTERN1_CODE_STRING);
        $("#txtHardnessData2").val(jsonResult.hardnesS_PATTERN1_MIN_MAX_STRING);
        $("#txtHardnessData3").val(jsonResult.hardnesS_PATTERN1);
    }
    else if (jsonResult.hardnesS_PATTERN2_CODE > 0) {
        $("#txtHardnessData1").val(jsonResult.hardnesS_PATTERN2_CODE_STRING);
        $("#txtHardnessData2").val(jsonResult.hardnesS_PATTERN2_MIN);
        $("#txtHardnessData3").val(jsonResult.hardnesS_PATTERN2_MAX);
    } else {
        $("#txtHardnessData1").val("");
        $("#txtHardnessData2").val("");
        $("#txtHardnessData3").val("");
    }



    $("#hfId_HardnessCD").val(jsonResult.mI_HARDNESS_CD);
    operatorCD = jsonResult.hD_TEST_OPERATOR_CD;
    SetOperatorDropdown($("#ddlOperatorName"), operatorCD);
    //setOperatorList("", $("#ddlOperatorName"));

    $("#txtDate").val($.format.date(new Date(jsonResult.hD_TEST_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });;

    $("#hiStartOperator").val(jsonResult.HD_TEST_OPERATOR_CD);
    $("#txtSplitNo").val(jsonResult.detaiL_NO);
    $("#txtFurnaceNum").val(jsonResult.hD_FURNACE_NO);

    if ($.isNumeric(jsonResult.iS_ACT_DIM_IN_RANGE) && parseInt(jsonResult.iS_ACT_DIM_IN_RANGE) == 1) {
        $("#chkActualDim").prop('checked', true);
    }
    else {
        $("#chkActualDim").prop('checked', false);
    }

    $("#txtActDim").val(jsonResult.hD_ACT_DIM);


    $("#ddlHardnessKubun2").val(jsonResult.hD_PATTERN_ACT);
    //$("#ddlnextprocess").val(jsonResult.hD_NEXT_PROCESS); 
    $("#txtCenterRef").val(jsonResult.hD_SUTEOSHI);
    $("#txtD1").val(jsonResult.hD_D4_1);
    $("#txtD2").val(jsonResult.hD_D4_2);
    $("#txtD3").val(jsonResult.hD_D4_3);
    $("#txtD4").val(jsonResult.hD_D4_4);
    $("#txtD4Avg").val(jsonResult.hD_D4_AVG);

    if ($.isNumeric(jsonResult.iS_INCLUDE_CENTER_AVG) && parseInt(jsonResult.iS_INCLUDE_CENTER_AVG) == 1) {
        $("#chkIncludesCenterInAvg").prop('checked', true);
        $("#txtCenterRef").addClass("Mandatory");
    }
    else {
        $("#chkIncludesCenterInAvg").prop('checked', false);
        $("#txtCenterRef").removeClass("Mandatory");
    }

    $("#ddlDecision").val(jsonResult.hD_DECISION);
    $("#txtRemarks").val(jsonResult.remarks);

    if (jsonResult.lasT_DETAIL_NO != null) {
        $("#txtLastSplitNo").val(jsonResult.lasT_DETAIL_NO);
    }

    $("#ddlOperatorName").val(jsonResult.hD_TEST_OPERATOR_CD);

    SetDecimalFormats();

}
function ResetGlobalVariables() {

    dimTolMin = "";
    dimTolMax = "";
    dimStd = "";
    lenTolMin = "";
    lenTolMax = "";
    lenStd = "";

    //  isStraightingRemarksUpdate = 0;
    chuuban = null;
    gyouban = null;
    registeredThabaNumbersAndDecision = {};

}
function SetNextProcess() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
        });
}
function CalculateD4Avg() {
    debugger;
    var D4_1;
    var D4_2;
    var D4_3;
    var D4_4;
    var D4Avg;
    var totalsum;
    if ($("#txtD1").val() == "" || $("#txtD1").val() == null) {
        D4_1 = 0;

    } else {

        D4_1 = $("#txtD1").val();
    }
    if ($("#txtD2").val() == "" || $("#txtD2").val() == null) {
        D4_2 = 0;
    } else {

        D4_2 = $("#txtD2").val();
    }

    if ($("#txtD3").val() == "" || $("#txtD3").val() == null) {
        D4_3 = 0;
    } else {

        D4_3 = $("#txtD3").val();
    }
    if ($("#txtD4").val() == "" || $("#txtD4").val() == null) {
        D4_4 = 0;
    } else {

        D4_4 = $("#txtD4").val();
    }
    if ($("#txtCenterRef").val() == "" || $("#txtCenterRef").val() == null) {
        centerVal = 0;
    } else {

        centerVal = $("#txtCenterRef").val();
    }

    if ($("#txtD1").val() == "" || $("#txtD1").val() == null || $("#txtD2").val() == "" || $("#txtD2").val() == null || $("#txtD3").val() == "" || $("#txtD3").val() == null) {
        $("#txtD4Avg").val("");

        $("#txtcheckD4").addClass("Mandatory");
    } else {

        $("#txtcheckD4").removeClass("Mandatory");
        if ($("#txtD4").val() == "" || $("#txtD4").val() == null) {

            if (centerVal > 0 && $("#chkIncludesCenterInAvg").prop("checked") == true) {
                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3) + parseFloat(centerVal);
                D4Avg = totalsum / 4;

            } else {

                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3);
                D4Avg = totalsum / 3;
            }


        } else {


            if (centerVal > 0 && $("#chkIncludesCenterInAvg").prop("checked") == true) {
                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3) + parseFloat(D4_4) + parseFloat(centerVal);
                D4Avg = totalsum / 5;

            } else {
                totalsum = parseFloat(D4_1) + parseFloat(D4_2) + parseFloat(D4_3) + parseFloat(D4_4);

                D4Avg = totalsum / 4;
            }
        }
        $("#txtD4Avg").val(D4Avg.toFixed(2));
    }
}
$("#txtD1").change(function () {

    CalculateD4Avg();
});
$("#txtD2").change(function () {

    CalculateD4Avg();
});
$("#txtD3").change(function () {

    CalculateD4Avg();
});
$("#txtD4").change(function () {

    CalculateD4Avg();
});
$("#txtCenterRef").change(function () {

    CalculateD4Avg();
});
$("#chkIncludesCenterInAvg").click(function () {
    if ($("#chkIncludesCenterInAvg").prop("checked") == true) {
        $("#txtCenterRef").addClass("Mandatory");
    } else {
        $("#txtCenterRef").removeClass("Mandatory");
    }
    CalculateD4Avg();
});

function BindNextProcessDropdown(jsonResult) {

    /* $("#ddlnextprocess").html("").append($('<option value="0"></option>'));*/
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlnextprocess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlnextprocess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
        });
    }
}



function SetDecisionList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Hardness/SetDecisionList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindDecisionDropdown(jsonResult) {
    /*$("#ddlDecision").html("").append($('<option value="0"></option>'));*/
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function SetHardnessKubuns() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Hardness/setHardnessKubuns",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindHardnessKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindHardnessKubunDropdown(jsonResult) {
    $("#ddlHardnessKubun2").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlHardnessKubun2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
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
        //  $("#imgError").show();

        ShowMessageDialog(message, focusFieldID, WARNING)
        //$("#imgWarning").show();

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
        //    }
        //});
    }
    return returnVal;
}

function IsDropDownMandatory() {
    var returnVal = true;
    var message = "";
    if ($("#ddlOperatorName").val() == "0") {
        var focusFieldID = "ddlOperatorName";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        ShowMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    else if ($("#ddlDecision").val() == "0") {
        var focusFieldID = "ddlDecision";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        ShowMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    //else if ($("#ddlnextprocess").val() == "0") {
    //    var focusFieldID = "ddlnextprocess";
    //    var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
    //    ShowMessageDialog(message, focusFieldID, WARNING);
    //    returnVal = false;
    //}
    else if ($("#ddlHardnessKubun2").val() == "0") {
        var focusFieldID = "ddlHardnessKubun2";
        var message = $.trim($("#lblErrorMessageE_ColdDrawing_EDITDELETE").text());
        ShowMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }


    return returnVal;
}
function SetThabaDetails(jsonResult) {

    // First, clear thaba details
    //$("[id$='lblDecisionNo']").html("");

    $("#lblDecisionNo").html("");

    // Then, set thaba details
    $.each(jsonResult, function (key, jsonObj) {

        if (jsonObj.tabA_NO) {
            var thabaNumbers = jsonObj.tabA_NO.split(",");
            var decision = jsonObj.decision;


            $.each(thabaNumbers, function (index, thabaNo) {
                thabaNo = thabaNo.trim();
                registeredThabaNumbersAndDecision["" + thabaNo] = decision;



            });
        }
    });


}

function GetThabadetails(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true)
    var ajaxTime = new Date().getDate();
    var inputThaba1 = $("#txtSplitNo").val().trim();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetThabaDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            chuuban: chuuban, gyouban: gyouban
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonThabaDetailData = response.message;
            SetThabaDetails(jsonThabaDetailData);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

//Server Site : Check old thaba present with OK decision 



function CheckDate(field) {

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
function checkSearchInspeDateFormat(field) {
    debugger;
    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    //re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
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
function formatStartEndDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('/');
}
function IsDateDifferentFromRange(boolVal) {

    var returnval = false;
    var getDate;
    if (boolVal === true) {
        getDate = ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

    }


    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_HARDNESS_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });




    return returnval;
}


function CheckTime(field) {
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

function IsStartOpr(startOprCD) {

    if (isOprExists == false && $("#ddlOperatorName").val() == startOprCD) {
        var focusFieldID = "ddlOperatorName";
        var message = $("#hfId_OperatorInactiveMessage").val();
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    else {
        return true;
    }
}

function SetStartSysDateTime() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;


            $("#txtDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });




        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function setSearchStartEndDateTime() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;

            var startDateObj = new Date(sysDateTime);
            startDateObj.setMonth(startDateObj.getMonth() - 1);
            $("#txtInspectionDateStart").val($.format.date(new Date(startDateObj), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtInspectionDateEnd").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function UpdateFICombinedDecision() {
    var ajaxTime = new Date().getTime();
    var hdDecision = $("#ddlDecision").val();
    var processPlanCd = $("#hfId_ProcessPlanCD").val();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/UpdateFICombinedDecision?hddecision=" + hdDecision + "&processplancd=" + processPlanCd,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }

    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function UpdateDetails() {

    var ajaxTime = new Date().getDate();

    var jsonDataForSave = JSON.stringify({
        MSG_CD: $("#hfId_MessageCD").val() ? $("#hfId_MessageCD").val() : null,
        MI_HARDNESS_CD: parseInt($("#hfId_HardnessCD").val()),
        PROCESS_PLAN_CD: parseInt($("#hfId_ProcessPlanCD").val()),
        HD_TEST_OPERATOR_CD: $("#ddlOperatorName").val(),
        HD_TEST_DATE: ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),

        HD_FURNACE_NO: ($("#txtFurnaceNum").val() || "").toUpperCase() || null,

        DETAIL_NO: $("#txtSplitNo").val(),
        IS_ACT_DIM_IN_RANGE: ($('#chkActualDim').is(':checked') ? 1 : 0),
        HD_ACT_DIM: $("#txtActDim").val(),
        HD_PATTERN_ACT: $("#ddlHardnessKubun2").val(),

        HD_SUTEOSHI: $("#txtCenterRef").val() ? $("#txtCenterRef").val() : null,
        HD_D4_1: $("#txtD1").val(),
        HD_D4_2: $("#txtD2").val(),
        HD_D4_3: $("#txtD3").val(),
        HD_D4_4: $("#txtD4").val() ? $("#txtD4").val() : null,
        HD_D4_AVG: $("#txtD4Avg").val(),
        IS_INCLUDE_CENTER_AVG: ($('#chkIncludesCenterInAvg').is(':checked') ? 1 : 0),
        HD_DECISION: $("#ddlDecision").val(),
        // HD_NEXT_PROCESS: $("#ddlnextprocess").val(),
        REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        USER_ID: $("#hdfLoginId").val(),
        //+ "USER_NAME" + " : '" + userName

    });


    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/HardnessDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({STAMPING_CD: $("#hfld_StamppingCd").val(),MACHINE_CD: $("#ddlModel").val(),PROCESS_PLAN_CD: parseInt($("#hdfProcessPlanCD").val()),MSG_CD: parseInt($("#hfld_MessageCD").val()),ST_IS_REMARKS_UPDATE: isStamppingRemarksUpdate,ST_START_DATE: $("#txtStartDate").val() + " " + $("#txtStartTime").val(),ST_START_OPERATOR_CD: parseInt($("#ddlStartOperatorNameValue").find(":selected").val()),ST_METALLIC_PLATE_CODE: $("#txtMetalicPltNo").val(),ST_DECISION: parseInt($("#ddlDecision").val()),ST_NG_QTY: parseFloat($("#txtNGQty").val()),ST_OK_QTY: parseFloat($("#txtNgWeight").val()),ST_NG_BARS1: parseInt($("#txtNGWeight1").val()),ST_NG_DETAILS1: parseInt($("#ddlDefects1").val()),ST_NG_BARS2: parseInt($("#txtNGWeight2").val()),ST_NG_DETAILS2: parseInt($("#ddlDefects2").val()),ST_NG_BARS3: parseInt($("#ddlDefects3").val()),ST_NG_DETAILS3: parseInt($("#txtNGWeight3").val()),ST_TABA_NO: parseInt($("#txtBundleNo").val()),ST_OK_BARS: parseInt($("#txtFinishedBarCount").val()),UNIT_TYPE: parseInt($("#ddlUnitType").val()),ST_END_DATE: $("#txtEndDate").val() + " " + $("#txtEndTime").val(),ST_END_OPERATOR_CD: parseInt($("#ddlEndOperatorNameValue").find(":selected").val()),ST_NEXT_PROCESS: parseInt($("#ddlNextProcess").val()),ST_REMARKS: $("#txtRemarks").val(),USER_NAME: userName})
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            //DisableDataPanel();
            //DisableEditDeleteButtons();
            if (parseInt(response.message) > 0) {
                $("#hfId_MessageCD").val(response.message);
               
            } 
            else {
                $("#hfId_MessageCD").val("");
                
            }
            var successMessage = $("#lblSuccessMessageS_HARDNESS_EDIT_DELETE_001").text();
            ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);
            debugger;
            //btnSearch_Click();
            HardnessGridSearch(null);


        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}
//Validation for Update button
function ValidateHardnessDetailsOnUpdateBtn() {

    var ajaxTime = new Date().getTime();
    var MI_HARDNESS_CD= $("#hfId_HardnessCD").val();
    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/Hardness/ValidateHardnessDetailsOnUpdateBtn?MIHardnessCD=" + MI_HARDNESS_CD,
        method: "POST",
        contentType: "application/json; charset=utf-8"

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;


            if (response.message == "0") { //0:Data not Deleted  
                var successMessage = $("#valMsgEditNotAllowedUsedInMillSheet").text();
                ShowMessageDialog(successMessage, "btnInitialize", WARNING);
            }
            else if (response.message == "1") { //1:Data Deleted Successfully 
                UpdateDetails();
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

//Delete hardness Detils 
function DeleteHardnessDetails() {

    var ajaxTime = new Date().getTime();

    var jsonData = JSON.stringify({
        MI_HARDNESS_CD: $("#hfId_HardnessCD").val(),
        PROCESS_PLAN_CD: $("#hfId_ProcessPlanCD").val(),
        DETAIL_NO: $("#txtSplitNo").val(),
        MSG_CD: $("#hfId_MessageCD").val() ? $("#hfId_MessageCD").val() : null

    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/Hardness/HardnessDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            //clearPanel($("#pnlProductDetails"));
            //clearPanel($("#divProcessStartData"));
            //ResetGlobalVariables();
            DisableEditDeleteButtons();
            DisableDataPanel();

            if (response.message == "0") { //0:Data not Deleted  
                var successMessage = $("#valMsgDeleteNotAllowedUsedInMillSheet").text();
                ShowMessageDialog(successMessage, "btnInitialize", WARNING);
            }
            else if (response.message == "1") { //1:Data Deleted Successfully 
                //  SwitchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
                var successMessage = $("#lblSuccessMessageS_HARDNESS_EDIT_DELETE_002").text();
                ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);
                //btnSearch_Click();
                HardnessGridSearch(null);
                ClearDataOnDelete();
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function ClearPanel($panel) {
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
function ClearTable($table, trId, trClass) {
    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}
function DisableDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#divProcessStartData");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("select"), function (index, dropdown) {
        //$("#" + dropdown.id + " option:selected").prop("selected", false);
        //$("#" + dropdown.id + " option:first").prop("selected", "selected");
        $(dropdown).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });
    var $endDataPanel = $("#pnlProcessEndData");
    $.each($endDataPanel.find("input:submit"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });
}

function EnableDataPanel(jsonResult) {

    var $startDataPanel = $("#divProcessStartData");

    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(disabled).removeAttr(disabled);
        }
    });

    $.each($startDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(disabled).removeAttr(disabled);
        }
    });

    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(disabled).removeAttr(disabled);
        }
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(disabled).removeAttr(disabled);
        }
    });
}
function Initialize() {
    typeDropDown = "select-one";
    typeText = "text";
    disabled = "disabled";
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
    DISABLED = "disabled";
    dimTolMin;
    dimTolMax;
    dimStd;
    lenTolMin;
    lenTolMax;
    lenStd
    chuuban = null;
    gyouban = null;
    isOprExists = false;
    userName = "";
    registeredThabaNumbersAndDecision = {};
    combinedDecisionOK = "";
    operatorCD = "";
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
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlSearch"));
    ClearPanel($("#divProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));

    ResetGlobalVariables();
    DisableEditDeleteButtons();

    SwitchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    //DisableEditDeleteButtons();
    DisableDataPanel();
    fICombinedDecisions = {};
    document.getElementById('FIDoneWarning').style.display = 'none';
}
//new function created for clear all data after delete action
function ClearDataOnDelete() {
    combinedDecisionOK = "";
    operatorCD = "";
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
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlSearch"));
    ClearPanel($("#divProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));

    ResetGlobalVariables();
    DisableEditDeleteButtons();

    //DisableEditDeleteButtons();
    DisableDataPanel();
}
function GetHardnessDetailsByCD($callFrom) {

    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var MIHardnessCD = $.trim($callFrom.closest("tr").attr("data-lblHardnessCD"));
    var chuuban = $.trim($callFrom.closest("tr").attr("data-chuuban"));
    var gyouban = $.trim($callFrom.closest("tr").attr("data-gyouban"));
    document.getElementById('FIDoneWarning').style.display = 'none';
    //var actualDim = true;

    var jsonData = "{MIHardnessCD :'" + MIHardnessCD + "',chuuban :'" + chuuban + "', gyouban: '" + gyouban + "' , USER_NAME: '" + userName + "' }";
    /* SetDecisionList();
     SetHardnessKubuns();*/

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetMIHardnessDetailsbyHardnessCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"

        data: JSON.stringify({ MI_HARDNESS_CD: MIHardnessCD, PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            EnableDataPanel();

            GetFICombinedDecisionDetails(chuuban, gyouban);
            SetHardnessDetails(jsonResult);

            if ((jsonResult.pP_GYOUBAN.toString().length) == 1) {
                jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
            }
            //   GetThabadetails(jsonResult.pP_CHUUBAN, jsonResult.pP_GYOUBAN, $callFrom);
            $("#txtSplitNo").focus();
            $("#btnUpdate").unbind("click")
            $("#btnUpdate").click(function (event) {
                debugger;
                var actualDim = true;
                var D4Avg = parseFloat($("#txtD4Avg").val());

                var hardnessStandardValue = parseFloat($("#txtHardnessData3").val());
                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }

                if (!IsDuplicateThabaNo(jsonResult.pP_CHUUBAN, jsonResult.pP_GYOUBAN, $callFrom)) {
                    return false;
                }

                if (!CheckDate($("#txtDate"))) {
                    var message = $("#lblErrorMessageE_HARDNESS_ENTRY_011").text().trim(); // This message will be displayed on dialog box
                    ShowMessageDialog(message, "", WARNING);
                    $("#txtDate").focus;
                    return false;
                }
                if (!IsDateDifferentFromRange(true)) {
                    var message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_002").text().trim() + "(" + $("#lblErrorMessageE_HARDNESS_ENTRY_010").text().trim() + commonDateRange + ")";
                    var focusFieldID = "txtDate";
                    ShowMessageDialog(message, focusFieldID, WARNING);
                    return false;
                }

                if ($("#ddlOperatorName option:selected").attr("flag") == 1) {

                    OperatorIsInactive("ddlOperatorName");
                    return false;
                }
                //Deff. furnance no 
                if ($("#txtFurnaceNum").val() != "") {
                    if (!IsDifferentFurnanceNo()) {

                        var errorMessage = $("#lblErrorMessageE_HARDNESS_ENTRY_012").text();
                        ShowMessageDialog(errorMessage, "txtFurnaceNum", WARNING);
                        return false;
                    }
                }
                //if (D4Avg >= hardnessStandardValue) {
                //    debugger;
                //    if ($("#ddlDecision").val() == 10) {
                //        var message = $("#lblErrorMessageE_HARDNESS_ENTRY_009").text().trim(); //Avg 
                //        var focusFieldID = "txtD4Avg";
                //        ShowMessageDialog(message, focusFieldID, WARNING);
                //        return false;
                //    }
                //}
                if ($("#chkActualDim").prop("checked") == true) {  //$("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK  now removing okdecision condition for all decision type as per windows
                    var dimValues = {};
                    dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
                    dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
                    dimValues["DIM1"] = $("#txtActDim").val() ? $("#txtActDim").val() : null;
                    dimValues["DIM2"] = null;
                    dimValues["DIM3"] = null;
                    dimValues["DIM4"] = null;
                    dimValues["DIM5"] = null;
                    dimValues["DIM6"] = null;

                    var result = IsActualDimentionValid(dimValues);
                    if (result.IsSuccess == false) {
                        actualDim = false;
                    }
                }

                if (actualDim == false) {
                    var message = $("#lblConfirmationMessageC_HARDNESS_EDIT_DELETE_001").text().trim(); //ActDimOutOfTolerance
                    var focusFieldID = "txtActDim";
                    ShowMessageDialog(message, focusFieldID, WARNING);
                    return false;

                }
                var D4Avg = parseFloat($("#txtD4Avg").val());
                var hardnessPatternValue = $("#txtHardnessData1").val();
                var hardnessValue1 = $("#txtHardnessData2").val();
                var hardnessValue2 = $("#txtHardnessData3").val();
                var txtD1 = $("#txtD1").val();
                var txtD2 = $("#txtD2").val();
                var txtD3 = $("#txtD3").val();
                var txtD4 = $("#txtD4").val();


                if (hardnessPatternValue.length > 0) {

                    let hdPatternValue = String(hardnessPatternValue || "");
                    let hardness2Value = String(hardnessValue1 || "");
                    let hardness3Value = String(hardnessValue2 || "");

                    let validMinVal = null;
                    let validMaxVal = null;

                    // If hdPatternValue is not empty
                    if (hdPatternValue !== "") {

                        // If HARDNESS_1 != JITSU
                        // if (hdPatternValue !== DAO.CONSTANTS.JITSU) {

                        // Check if HARDNESS_2 is numeric
                        if (!isNaN(Number(hardness2Value))) {
                            validMinVal = Number(hardness2Value);
                            validMaxVal = Number(hardness3Value);
                        }

                        // HARDNESS_2 = MIN
                        if (hardness2Value.includes("MIN")) {
                            validMinVal = Number(hardness3Value);
                        }

                        // HARDNESS_2 = MAX
                        if (hardness2Value.includes("MAX")) {
                            validMaxVal = Number(hardness3Value);
                        }
                        // }
                    }

                    let avgVal = Number(D4Avg);

                    let isOutOfRangeFlag = false;

                    // Check AVG < MIN
                    if (!isNaN(validMinVal) && validMinVal != null) {
                        //50            60
                        if (avgVal <= validMinVal) {
                            isOutOfRangeFlag = true;
                        }
                    }

                    // Check AVG > MAX
                    if (!isNaN(validMaxVal) && validMaxVal != null) {

                        //100   150      
                        if (avgVal >= validMaxVal) {
                            isOutOfRangeFlag = true;
                        }
                    }
                    if (isOutOfRangeFlag == true) {

                        bootbox.confirm({
                            title: "<div style='color:white'></div>",
                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#lblConfirmationMessageC_HARDNESS_EDIT_DELETE_002").text().trim() + "</td></tr></table>",
                            buttons: {
                                confirm: {
                                    label: $("#lblYes").text(),
                                    className: 'btn-success'
                                },
                                cancel: {
                                    label: $("#lblNo").text(),
                                    className: 'btn-danger'
                                }
                            },
                            callback: function (result) {

                                if (result) {
                                    debugger;
                                    bootbox.hideAll();
                                    SetHardnessStdValue();
                                    var result = CheckHardnessStdValue(txtD1, txtD2, txtD3, txtD4, validMinVal, validMaxVal);
                                    if (result == true) {
                                        if (fICombinedDecisions.length > 0) {
                                            var hardnessDecision = $("#ddlDecision").val();
                                            for (i = 0; i < fICombinedDecisions.length; i++) {
                                                if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                                                    if (hardnessDecision == FIResultOk) {
                                                        bootbox.confirm({
                                                            title: "<div style='color:white'></div>",
                                                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#confMsgFIExistUpdateFIAsOK").text().trim() + "</td></tr></table>",
                                                            buttons: {
                                                                confirm: {
                                                                    label: $("#lblYes").text(),
                                                                    className: 'btn-success'
                                                                },
                                                                cancel: {
                                                                    label: $("#lblNo").text(),
                                                                    className: 'btn-danger'
                                                                }
                                                            },
                                                            callback: function (result) {

                                                                if (result) {
                                                                    debugger;
                                                                    bootbox.hideAll();
                                                                    UpdateFICombinedDecision();
                                                                    ValidateHardnessDetailsOnUpdateBtn();
                                                                }
                                                                else {
                                                                    bootbox.hideAll();
                                                                    $("#ddlDecision").focus();
                                                                }


                                                            },
                                                            centerVertical: true
                                                        });
                                                    }
                                                    else if (hardnessDecision != FIResultOk) {
                                                        bootbox.confirm({
                                                            title: "<div style='color:white'></div>",
                                                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                                                            buttons: {
                                                                confirm: {
                                                                    label: $("#lblYes").text(),
                                                                    className: 'btn-success'
                                                                },
                                                                cancel: {
                                                                    label: $("#lblNo").text(),
                                                                    className: 'btn-danger'
                                                                }
                                                            },
                                                            callback: function (result) {

                                                                if (result) {
                                                                    debugger;
                                                                    bootbox.hideAll();
                                                                    ValidateHardnessDetailsOnUpdateBtn();
                                                                }
                                                                else {
                                                                    bootbox.hideAll();
                                                                    $("#ddlDecision").focus();
                                                                }


                                                            },
                                                            centerVertical: true
                                                        });
                                                    }
                                                    else {
                                                        ValidateHardnessDetailsOnUpdateBtn();
                                                    }
                                                }
                                                else if (CombinedDecisionArr.includes(parseInt(FIResultOk))) {
                                                    if (hardnessDecision != FIResultOk) {
                                                        bootbox.confirm({
                                                            title: "<div style='color:white'></div>",
                                                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                                                            buttons: {
                                                                confirm: {
                                                                    label: $("#lblYes").text(),
                                                                    className: 'btn-success'
                                                                },
                                                                cancel: {
                                                                    label: $("#lblNo").text(),
                                                                    className: 'btn-danger'
                                                                }
                                                            },
                                                            callback: function (result) {

                                                                if (result) {
                                                                    debugger;
                                                                    bootbox.hideAll();
                                                                    ValidateHardnessDetailsOnUpdateBtn();
                                                                }
                                                                else {
                                                                    bootbox.hideAll();
                                                                    $("#ddlDecision").focus();
                                                                }


                                                            },
                                                            centerVertical: true
                                                        });
                                                    }

                                                    else {
                                                        ValidateHardnessDetailsOnUpdateBtn();
                                                    }
                                                }
                                                else {
                                                    ValidateHardnessDetailsOnUpdateBtn();
                                                }
                                                break;
                                            }
                                        }
                                        else {
                                            ValidateHardnessDetailsOnUpdateBtn();
                                        }
                                    }

                                }
                                else {
                                    bootbox.hideAll();
                                    $("#txtD4Avg").focus();
                                }


                            },
                            centerVertical: true
                        });
                        debugger;
                    }
                    else {
                        debugger;
                        SetHardnessStdValue();
                        var result = CheckHardnessStdValue(txtD1, txtD2, txtD3, txtD4, validMinVal, validMaxVal);
                        if (result == true) {
                            if (fICombinedDecisions.length > 0) {
                                var hardnessDecision = $("#ddlDecision").val();
                                for (i = 0; i < fICombinedDecisions.length; i++) {
                                    if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                                        if (hardnessDecision == FIResultOk) {
                                            bootbox.confirm({
                                                title: "<div style='color:white'></div>",
                                                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#confMsgFIExistUpdateFIAsOK").text().trim() + "</td></tr></table>",
                                                buttons: {
                                                    confirm: {
                                                        label: $("#lblYes").text(),
                                                        className: 'btn-success'
                                                    },
                                                    cancel: {
                                                        label: $("#lblNo").text(),
                                                        className: 'btn-danger'
                                                    }
                                                },
                                                callback: function (result) {

                                                    if (result) {
                                                        debugger;
                                                        bootbox.hideAll();
                                                        UpdateFICombinedDecision();
                                                        ValidateHardnessDetailsOnUpdateBtn();
                                                    }
                                                    else {
                                                        bootbox.hideAll();
                                                        $("#ddlDecision").focus();
                                                    }


                                                },
                                                centerVertical: true
                                            });
                                        }
                                        else if (hardnessDecision != FIResultOk) {
                                            bootbox.confirm({
                                                title: "<div style='color:white'></div>",
                                                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                                                buttons: {
                                                    confirm: {
                                                        label: $("#lblYes").text(),
                                                        className: 'btn-success'
                                                    },
                                                    cancel: {
                                                        label: $("#lblNo").text(),
                                                        className: 'btn-danger'
                                                    }
                                                },
                                                callback: function (result) {

                                                    if (result) {
                                                        debugger;
                                                        bootbox.hideAll();
                                                        ValidateHardnessDetailsOnUpdateBtn();
                                                    }
                                                    else {
                                                        bootbox.hideAll();
                                                        $("#ddlDecision").focus();
                                                    }


                                                },
                                                centerVertical: true
                                            });
                                        }
                                        else {
                                            ValidateHardnessDetailsOnUpdateBtn();
                                        }
                                    }
                                    else if (CombinedDecisionArr.includes(parseInt(FIResultOk))) {
                                        if (hardnessDecision != FIResultOk) {
                                            bootbox.confirm({
                                                title: "<div style='color:white'></div>",
                                                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                                                buttons: {
                                                    confirm: {
                                                        label: $("#lblYes").text(),
                                                        className: 'btn-success'
                                                    },
                                                    cancel: {
                                                        label: $("#lblNo").text(),
                                                        className: 'btn-danger'
                                                    }
                                                },
                                                callback: function (result) {

                                                    if (result) {
                                                        debugger;
                                                        bootbox.hideAll();
                                                        ValidateHardnessDetailsOnUpdateBtn();
                                                    }
                                                    else {
                                                        bootbox.hideAll();
                                                        $("#ddlDecision").focus();
                                                    }


                                                },
                                                centerVertical: true
                                            });
                                        }

                                        else {
                                            ValidateHardnessDetailsOnUpdateBtn();
                                        }
                                    }
                                    else {
                                        ValidateHardnessDetailsOnUpdateBtn();
                                    }
                                    break;
                                }
                            }
                            else {
                                ValidateHardnessDetailsOnUpdateBtn();
                            }


                        }

                    }
                }
                else {
                    if (fICombinedDecisions.length > 0) {
                        var hardnessDecision = $("#ddlDecision").val();
                        for (i = 0; i < fICombinedDecisions.length; i++) {
                            if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                                if (hardnessDecision == FIResultOk) {
                                    bootbox.confirm({
                                        title: "<div style='color:white'></div>",
                                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#confMsgFIExistUpdateFIAsOK").text().trim() + "</td></tr></table>",
                                        buttons: {
                                            confirm: {
                                                label: $("#lblYes").text(),
                                                className: 'btn-success'
                                            },
                                            cancel: {
                                                label: $("#lblNo").text(),
                                                className: 'btn-danger'
                                            }
                                        },
                                        callback: function (result) {

                                            if (result) {
                                                debugger;
                                                bootbox.hideAll();
                                                UpdateFICombinedDecision();
                                                ValidateHardnessDetailsOnUpdateBtn();
                                            }
                                            else {
                                                bootbox.hideAll();
                                                $("#ddlDecision").focus();
                                            }


                                        },
                                        centerVertical: true
                                    });
                                }
                                else if (hardnessDecision != FIResultOk) {
                                    bootbox.confirm({
                                        title: "<div style='color:white'></div>",
                                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                                        buttons: {
                                            confirm: {
                                                label: $("#lblYes").text(),
                                                className: 'btn-success'
                                            },
                                            cancel: {
                                                label: $("#lblNo").text(),
                                                className: 'btn-danger'
                                            }
                                        },
                                        callback: function (result) {

                                            if (result) {
                                                debugger;
                                                bootbox.hideAll();
                                                ValidateHardnessDetailsOnUpdateBtn();
                                            }
                                            else {
                                                bootbox.hideAll();
                                                $("#ddlDecision").focus();
                                            }


                                        },
                                        centerVertical: true
                                    });
                                }
                                else {
                                    ValidateHardnessDetailsOnUpdateBtn();
                                }
                            }
                            else if (CombinedDecisionArr.includes(parseInt(FIResultOk))) {
                                if (hardnessDecision != FIResultOk) {
                                    bootbox.confirm({
                                        title: "<div style='color:white'></div>",
                                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#ConfMsgProcessResultDiscrepancy").text().trim() + "</td></tr></table>",
                                        buttons: {
                                            confirm: {
                                                label: $("#lblYes").text(),
                                                className: 'btn-success'
                                            },
                                            cancel: {
                                                label: $("#lblNo").text(),
                                                className: 'btn-danger'
                                            }
                                        },
                                        callback: function (result) {

                                            if (result) {
                                                debugger;
                                                bootbox.hideAll();
                                                ValidateHardnessDetailsOnUpdateBtn();
                                            }
                                            else {
                                                bootbox.hideAll();
                                                $("#ddlDecision").focus();
                                            }


                                        },
                                        centerVertical: true
                                    });
                                }

                                else {
                                    ValidateHardnessDetailsOnUpdateBtn();
                                }
                            }
                            else {
                                ValidateHardnessDetailsOnUpdateBtn();
                            }
                            break;
                        }
                    }
                    else {
                        ValidateHardnessDetailsOnUpdateBtn();
                    }

                }
            });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function SetHardnessStdValue() {
    var hardnessPatternValue = $("#txtHardnessData1").val();
    var hardnessValue1 = $("#txtHardnessData2").val();
    var hardnessValue2 = $("#txtHardnessData3").val();
    const minVal = hardnessValue1;
    const maxVal = hardnessValue2;

    if (minVal.includes("MIN") && maxVal) {
        m_hardnessStd = "MIN" + maxVal;

    } else if (minVal.includes("MAX") && maxVal) {
        m_hardnessStd = "MAX" + maxVal;

    } else if (minVal && maxVal) {
        m_hardnessStd = minVal + "-" + maxVal;

    } else {
        m_hardnessStd = "";
    }
}

function CheckHardnessStdValue(txtD1, txtD2, txtD3, txtD4, validMinVal, validMaxVal) {
    var txtD1 = txtD1;
    var txtD2 = txtD2;
    var txtD3 = txtD3;
    var txtD4 = txtD4;
    var validMinVal = validMinVal;
    var validMaxVal = validMaxVal;
    var returnVal = true;
    if (m_hardnessStd != "" && $("#ddlDecision").val() == 10) {
        if (CheckHardnessPatterValidation(m_hardnessStd, txtD1, validMinVal, validMaxVal)) {
            returnval = false;
            var message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_009").text().trim();
            showMessageDialog(message, "", WARNING);
            //returnVal = true;
            return false;
        }
        else {
            returnval = true;
        }
        if (CheckHardnessPatterValidation(m_hardnessStd, txtD2, validMinVal, validMaxVal)) {
            returnval = false;
            var message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_009").text().trim();
            showMessageDialog(message, "", WARNING);
            //returnVal = true;
            return false;
        }
        else {
            returnval = true;
        }
        if (CheckHardnessPatterValidation(m_hardnessStd, txtD3, validMinVal, validMaxVal)) {
            returnval = false;
            var message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_009").text().trim();
            showMessageDialog(message, "", WARNING);
            //returnVal = true;
            return false;
        }
        else {
            returnval = true;
        }
        if (CheckHardnessPatterValidation(m_hardnessStd, txtD4, validMinVal, validMaxVal)) {
            returnval = false;
            var message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_009").text().trim();
            showMessageDialog(message, "", WARNING);
            //returnVal = true;
            return false;
        }
        else {
            returnval = true;
        }
    }
    return returnVal;
}
function CheckHardnessPatterValidation(stdPatternValue, calVal, hardnessMinVal, hardnessMaxVal) {
    var returnval = false;

    if (stdPatternValue && stdPatternValue.trim() !== "" && calVal != "") {

        const pattern = stdPatternValue.trim().substring(0, 3).toUpperCase();
        let stdMinLimitVal, stdMaxLimitVal, calculateVal;

        if (pattern.includes("MIN")) {

            stdMinLimitVal = parseInt(hardnessMinVal, 10);
            calculateVal = parseFloat(calVal);

            if (calculateVal < stdMinLimitVal) {
                returnval = true;  // validation error
            } else {
                returnval = false; // no error
            }

        } else if (pattern.includes("MAX")) {

            stdMaxLimitVal = parseInt(hardnessMaxVal, 10);
            calculateVal = parseFloat(calVal);

            if (calculateVal > stdMaxLimitVal) {
                returnval = true;  // validation error
            } else {
                returnval = false; // no error
            }

        } else if (stdPatternValue.includes("-")) {

            stdMinLimitVal = parseInt(hardnessMinVal, 10);
            stdMaxLimitVal = parseInt(hardnessMaxVal, 10);
            calculateVal = parseFloat(calVal);

            if (calculateVal < stdMinLimitVal || calculateVal > stdMaxLimitVal) {
                returnval = true;  // validation error
            } else {
                returnval = false; // no error
            }

        } else {
            returnval = false; // no validation error
        }
    }

    return returnval; // default
}
function IsDifferentFurnanceNo() {
    var txtchuuban = $("#txtChuubanT1").val();
    var chuubanGyouban = txtchuuban.split("-");
    var chuuban = chuubanGyouban[1];
    var gyouban = chuubanGyouban[2];
    var splitNo = $("#txtSplitNo").val();

    //var gyouban = chuubanGyouban[2];
    var returnval = false;

    var ajaxTime = new Date().getTime();
    var jsonData = "{PP_CHUUBAN :'" + chuuban + "', TS_FURNACE_NO : '" + $("#txtFurnaceNum").val() + "'  , PP_GYOUBAN : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetFurnanceNo",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, DETAIL_NO: splitNo })
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            //if (response.message != null) {
            //    var txtFurnance_No = ($("#txtFurnaceNum").val()).toUpperCase();
            //    var TS_Furnace_No = jsonResult.tS_FURNACE_NO;
            //    if (TS_Furnace_No != txtFurnance_No) {
            //        returnval = false;
            //    }
            //    else {
            //        returnval = true;
            //    }
            //}
            //else {
            //    returnval = true;

            //}
            if (response.message != null) {
                if (response.message.tS_FURNACE_NO != null) {
                    var txtFurnance_No = ($("#txtFurnaceNum").val()).toUpperCase();
                    var TS_Furnace_No = jsonResult.tS_FURNACE_NO;
                    if (TS_Furnace_No != txtFurnance_No) {
                        returnval = false;
                    }
                    else {
                        returnval = true;
                    }
                }

                else {
                    returnval = true;
                }
            }
            else {
                returnval = true;

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING_ENTRY_014").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnval;
}
function IsDuplicateThabaNo(chuuban, gyouban, $callFrom) {

    var returnVal = true;
    var Thaba_No = $("#txtSplitNo").val();
    var hardnessCD = $("#hfId_HardnessCD").val();
    $callFrom.data('requestRunning', false)
    var ajaxTime = new Date().getDate();

    var jsonThabaDetailData = "{chuuban : '" + chuuban + "' , gyouban : '" + gyouban + "', Thaba_No : '" + Thaba_No + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, DETAIL_NO: Thaba_No
        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult == 0) {
                returnVal = true;
            } else {


                if (jsonResult[0].mI_HARDNESS_CD == hardnessCD) {
                    returnVal = true;
                } else {
                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_004").text().trim().replace("NNN", Thaba_No);

                    ShowMessageDialog(message, focusFieldID, WARNING);

                    returnVal = false;
                }

            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

    return returnVal;
}
$("#btnDelete").click(function (event) {
    DeleteHardnessDetails();

});
$("#btnInitialize").click(function (event) {
    Initialize();
    FillEmptyGrid();
    window.location.reload();
    $("#txtChuubanSearch").focus();
});

$("#btnDateTime").click(function (event) {
    SetStartSysDateTime();
});

$("#btnYobinaStartOpeartorSearch").click(function (event) {
    setOperatorList($("#txtInputSearch").val(), $("#ddlOperatorName"));
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
    var txtSrchDateStart = $("#txtInspectionDateStart").val();
    var txtSrchDateEnd = $("#txtInspectionDateEnd").val();
    var userId = $("#hdfLoginId").val();
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
        errorMessage = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_007").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_008").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_008").text());
        }
    }
    if ($("#txtInspectionDateStart").val() == "" && $("#txtInspectionDateEnd").val() != "") {
        var message = $("#lblErrorMessageE_InspStartDate_Required").text().trim(); // This message will be displayed on dialog box 
        showMessageDialog(message, "ddlMachine", WARNING);
        return false;
    }
    if ($("#txtInspectionDateStart").val() != "") {
        if (!checkSearchInspeDateFormat($("#txtInspectionDateStart"))) {
            var message = $("#lblErrorMessageE_InspStartDate_Invalid").text().trim(); // This message will be displayed on dialog box              
            showMessageDialog(message, "txtInspectionDateStart", WARNING);
            return false;
        }
    }
    if ($("#txtInspectionDateEnd").val() != "") {
        if (!checkSearchInspeDateFormat($("#txtInspectionDateEnd"))) {
            var message = $("#lblErrorMessageE_InspEndDate_Invalid").text().trim(); // This message will be displayed on dialog box             
            showMessageDialog(message, "txtInspectionDateEnd", WARNING);
            return false;
        }
    }
    var inspStartDate = formatStartEndDate($("#txtInspectionDateStart").val());
    var inspEndDate = formatStartEndDate($("#txtInspectionDateEnd").val());

    if (inspEndDate < inspStartDate) {
        var message = $("#lblErrorMessageE_StartEndDateValidate").text().trim();
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(message, "txtInspectionDateEnd", WARNING);
        return false;
    }

    if (errorMessage != "") {
        ShowMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }

    if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || parseInt(txtDimDMin) > 0 || parseInt(txtDimDMax) > 0 || txtShap.length > 0 || parseInt(txtLengthSearchMin) > 0 || parseInt(txtLengthSearchMax) > 0 || txtSrchDateStart.length > 0 || txtSrchDateEnd.length > 0) {

        //HardnessGridSearch(null);
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Hardness/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: txtChuubanSearch, CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin, PROD_DIM_D_MAX: txtDimDMax, SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax, START_DATETIME: txtSrchDateStart, END_DATETIME: txtSrchDateEnd,
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
                        url: "/Hardness/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)

                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divHardnessGridMain").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                            ShowMessageDialog(message, focusFieldID, ERROR);
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
                message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
            });


    }
    else {

        message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_005").text());
        ShowMessageDialog(message, 'txtChuubanSearch', WARNING);
    }
}


function SetDecimalFormats() {
    $("#txtActDim").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtD4Avg").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
    $("#txtCenterRef, #txtD1, #txtD2, #txtD3, #txtD4").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 1);
    });

}

function Mask() {
    $("#txtOperationDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtOperationDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

    $("#txtInspectionDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtInspectionDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

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
    HardnessGridSearch(selectedColumns);
}


function HardnessGridSearch(sortFields) {

    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var txtDimDMax = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var txtLengthSearchMax = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    var txtSrchDateStart = $("#txtInspectionDateStart").val();
    var txtSrchDateEnd = $("#txtInspectionDateEnd").val();
    var userId = $("#hdfLoginId").val();

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: txtChuubanSearch, CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin, PROD_DIM_D_MAX: txtDimDMax, SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax, START_DATETIME: txtSrchDateStart, END_DATETIME: txtSrchDateEnd,
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
                    url: "/Hardness/GridSearch",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)

                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divHardnessGridMain").html(response1.message);

                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
                        ShowMessageDialog(message, focusFieldID, ERROR);
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
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
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
        url: "/Hardness/InitializeSortPopup",
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
        }).fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
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
    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}


$(document).ready(function () {

    window.resizeTo(1850, 790);
    $('#lblModel').hide();
    $('#ddlModel').hide();
    $('#ddlModel').removeClass("Mandatory");
    $("#txtChuubanSearch").focus();
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    $(".FocusDefault").focus();
    // SetGlobalVariables();
    SetDecimalFormats();
    DisableDataPanel();
    DisableEditDeleteButtons();
    setSearchStartEndDateTime();
    Mask();
    userName = $("#hdfUserName").val();

    SetDecisionList();
    SetNextProcess();
    SetHardnessKubuns();


});

function OperatorIsInactive(dropdownName) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Hardness/OperatorIsInactive?operatorName=" + $("#" + dropdownName + ' option:selected').text(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            ShowMessageDialog(response.message, dropdownName, WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function FillEmptyGrid() {
    var emptyList = [];
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Hardness/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divHardnessGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
$("#ddlDecision").focusout(function (event) {
    $("#btnUpdate").focus();
});

//barCode scan on list page.
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
            url: APIURL + "/Hardness/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});