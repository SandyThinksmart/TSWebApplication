
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
var lenStd;
var IsOprExists = false;
var chuuban = null;
var gyouban = null;
userName = "";
var combinedDecisionOK = "";
var processPlanCD = "";
var $btnActual;
var tensileCD;
var FIResultOk = $("#hdfFIResultOk").val();
var FIResultNg = $("#hdfFIResultNg").val();
var FIResultUntested = $("#hdfFIResultUntested").val();
var FIResultSparkUntested = $("#hdfFIResultSparkUntested").val();
var FISparkUntested = $("#hdfFISparkUntested").val();
var FISparkOk = $("#hdfFISparkOk").val();
var FISparkNg = $("#hdfFISparkNg").val();
var fICombinedDecisions = {};
let CombinedDecisionArr = [];
let SparkDecisionArr = [];

function disableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
    $("#btnActual").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
    $("#btnActual").removeAttr("disabled");
}

function disableDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#pnlBottomData");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("select"), function (index, dropdown) {
        $(dropdown).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(disabled, disabled).addClass(disabled);
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });
    var $endDataPanel = $("#pnlBottomData");
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).attr(disabled, disabled).addClass(disabled);
    });

}

function enableDataPanel() {

    var $startDataPanel = $("#pnlBottomData");

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

    var $endDataPanel = $("#pnlBottomData");
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).removeClass(disabled).removeAttr(disabled);
    });
    $("#txtActualDim99").attr(disabled, disabled).addClass(disabled);
    $("#txtLastSplitNo").attr(disabled, disabled).addClass(disabled);
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

function switchTab($tabFrom, $tabTo) {

    $tabTo.click();

    enableEditDeleteButtons();
}

$("#txtBreakTest1").change(function (event) {

    calculateD4Avg();
});
$("#txtBreakTest2").change(function (event) {

    calculateD4Avg();
});
$("#txtBreakTest3   ").change(function (event) {

    calculateD4Avg();
});
function calculateD4Avg() {

    var Test1;
    var Test2;
    var Test3;

    var TestAvg;
    var dividedby = 0;
    if ($("#txtBreakTest1").val() == "" || $("#txtBreakTest1").val() == null) {
        Test1 = 0;

    } else {

        Test1 = $("#txtBreakTest1").val();
    }
    if ($("#txtBreakTest2").val() == "" || $("#txtBreakTest2").val() == null) {
        Test2 = 0;
    } else {

        Test2 = $("#txtBreakTest2").val();
    }

    if ($("#txtBreakTest3").val() == "" || $("#txtBreakTest3").val() == null) {
        Test3 = 0;
    } else {

        Test3 = $("#txtBreakTest3").val();
    }


    if (Test1 == 0 && Test2 == 0 && Test3 == 0) {
        $("#txtActualDim99").val("");


    } else {

        if (Test1 > 0) {
            dividedby = parseInt(dividedby) + 1;
        } else {
            dividedby = parseInt(dividedby) + 0;
        }
        if (Test2 > 0) {
            dividedby = parseInt(dividedby) + 1;
        } else {
            dividedby = parseInt(dividedby) + 0;
        }
        if (Test3 > 0) {
            dividedby = parseInt(dividedby) + 1;
        } else {
            dividedby = parseInt(dividedby) + 0;
        }

        TestAvg = (parseInt(Test1) + parseInt(Test2) + parseInt(Test3)) / dividedby;
        $("#txtActualDim99").val(TestAvg.toFixed(2));
    }
}

function setTensileDetails(jsonResult) {

    combinedDecisionOK = jsonResult.decisioN_OK;
    switchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));
    $("#hfId_MessageCD").val(jsonResult.msG_CD);
    $("#hfId_TensileCD").val(jsonResult.mI_TS_CD);
    $("#hfId_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    processPlanCD = $("#hfId_ProcessPlanCD").val();
    $("#txt_HostName").val(jsonResult.pP_NYUKA_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.createD_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.updateD_DATE_STRING), "yyyy/MM/dd HH:mm"))
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
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D.toFixed(2));    //PP_PROD_DIM_D_STRING
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
    // $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").val(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);            //text(jsonResult.PP_NO_OF_UNITS + '' + jsonResult.PP_UNIT);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);

    $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
    $("#hfId_TensileCD").val(jsonResult.mI_TS_CD);
    //$("#txtDate").val($.format.date(new Date(jsonResult.tS_TEST_DATE_STRING), "yyyy/MM/dd HH:mm"))
    $("#txtDate").val($.format.date(new Date(jsonResult.tS_TEST_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#hiStartOperator").val(jsonResult.tS_TEST_OPERATOR_CD);
    $("#txtSplitNo").val(jsonResult.detaiL_NO);
    $("#txtFurnaceNum").val(jsonResult.tS_FURNACE_NO);

    //if ($.isNumeric(jsonResult.iS_ACT_DIM_RANGE_CHECK) && parseInt(jsonResult.iS_ACT_DIM_RANGE_CHECK) == 1) {
    //    $("#chkActualDim").prop('checked', true);

    //}
    //else {

    //    $("#chkActualDim").prop('checked', false);
    //}
    $("#chkActualDim").prop('checked', true);
    $("#txtActDim").val(jsonResult.tS_ACT_DIM);
    if (jsonResult.tS_ACT_DIM != null && jsonResult.tS_ACT_DIM !== "") {
        $("#ddlDimShape").removeAttr(disabled).removeClass(disabled);
        $("#ddlDimShape").addClass("Mandatory");
        $("#ddlDimShape").val(jsonResult.tS_ACT_DIM_SHAPE);

    }
    else {
        $("#ddlDimShape").attr(disabled, disabled).addClass(disabled);
        $("#ddlDimShape").removeClass("Mandatory");
        setShapeList();
    }

    $("#txtTestSpecimentDim").val(jsonResult.tS_TEST_SPECIMEN_DIM);
    $("#txtGaugeLength").val(jsonResult.tS_GAUGE_LENGTH);

    $("#txtMaxLoadN").val(jsonResult.tS_MAX_LOAD_N);
    $("#txtYieldLoadN").val(jsonResult.tS_YIELD_LOAD_N);
    $("#txtCuttingElongation").val(jsonResult.tS_EXTENDED_LENGTH);
    $("#txtCutDim").val(jsonResult.tS_CUT_DIM);

    $("#txtBreakTest1").val(jsonResult.tS_TEST_IMPACT_1);
    $("#txtBreakTest2").val(jsonResult.tS_TEST_IMPACT_2);
    $("#txtBreakTest3").val(jsonResult.tS_TEST_IMPACT_3);

    $("#txtJISTestSpecimen1").val(jsonResult.pP_JIS_TEST_SPEC);
    $("#txtTensileStrength1").val(jsonResult.pP_TENSILE_STRENGTH);
    $("#txtYieldPoint1").val(jsonResult.pP_YEILD);
    $("#txtElongation1").val(jsonResult.pP_ELONGATION);
    $("#txtReduction1").val(jsonResult.pP_PRESS);
    $("#txtHardness1").val(jsonResult.pP_HARDNESS);

    if (jsonResult.elongatioN_PATTERN1_MIN_MAX != null) {
        $("#hdfElongationKubunType").val(jsonResult.elongatioN_PATTERN1_MIN_MAX);
    }
    if (jsonResult.tensilE_STRN_PATTERN1_MIN_MAX != null) {
        $("#hdfTensileKubunType").val(jsonResult.tensilE_STRN_PATTERN1_MIN_MAX);
    }
    if (jsonResult.hardnesS_PATTERN1_MIN_MAX != null) {
        $("#hdfHardnessKubunType").val(jsonResult.hardnesS_PATTERN1_MIN_MAX);
    }
    if (jsonResult.yeilD_PATTERN1_MIN_MAX != null) {
        $("#hdfYeildKubunType").val(jsonResult.yeilD_PATTERN1_MIN_MAX);
    }
    if (jsonResult.presS_PATTERN1_MIN_MAX != null) {
        $("#hdfPressKubunType").val(jsonResult.presS_PATTERN1_MIN_MAX);
    }

    $("#ddlDecision").val(jsonResult.tS_DECISION);
    $("#txtRemarks").val(jsonResult.remarks);

    if (jsonResult.tS_LAST_DETAIL_NO != null) {
        $("#txtLastSplitNo").val(jsonResult.tS_LAST_DETAIL_NO);
    }
    SetOperatorDropdown($("#ddlOperatorNameValue"), jsonResult.tS_TEST_OPERATOR_CD);
    setDecimalFormats();

    $("#ddlJISTest").val(jsonResult.tesT_SPECIMEN_CD);
    //$("#ddlnextprocess").val(jsonResult.tS_NEXT_PROCESS);
    calculateD4Avg();
    $("#btnActual").click();
}
function setDecisionList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/Tensile/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindDecisionDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function bindDecisionDropdown(jsonResult) {
    $("#ddlDecision").html("");
    // $("#ddlUnitType").html("").append($('<option value="0"></option>'));
    /*$("#ddlDecision").html("").append($('<option value="0"></option>'));*/
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function setShapeList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetDimShape",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindShapeDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindShapeDropdown(jsonResult) {

    $("#ddlDimShape").html("").append($('<option value="0"></option>'));
    //  $("#ddlDimShape").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDimShape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}



function setOperatorList(yobina, $target, defaultSelectID) {

    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/EmployeeList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);

            bindOperatorDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindOperatorDropdown(jsonResult, $target, defaultSelectID) {

    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

//function setJISTestList(defaultSelectID) {
//    setJISList($("#ddlJISTest"), defaultSelectID);
//};

function setJISList() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{ USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetJISTestList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            bindJISDropdown(jsonResult);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindJISDropdown(jsonResult) {
    $("#ddlJISTest").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {

        $("#ddlJISTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
    //if (defaultSelectID != null && defaultSelectID != "") {
    //    $("#" + $target.attr("id") + " option:selected").prop("selected", false);
    //    $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    //}
}


function resetResponseTime() {
    responseTime = ""; // Clear server response time
}
function resetGlobalVariables() {

    dimTolMin = "";
    dimTolMax = "";
    dimStd = "";
    lenTolMin = "";
    lenTolMax = "";
    lenStd = "";

    //  isStraightingRemarksUpdate = 0;
    chuuban = null;
    gyouban = null;
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
        url: APIURL + "/Tensile/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
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
        url: APIURL + "/Tensile/GetSysDateTime",
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
            message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function checkDate(field) {
    debugger;
    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    //re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
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
function IsDateDifferentFromRange(boolVal) {
    debugger;
    var returnval = false;
    var getDate;
    if (boolVal === true) {
        getDate = ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

    }
    //else {
    //    getDate = ($("#txtInspDate").val().split(" ")[0] + "T" + $("#txtInspDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    //}

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/IsValidDateRange",
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
            var message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });




    return returnval;
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

function setNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
    /*$("#ddlnextprocess").html("").append($('<option value="0"></option>'));*/
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

function getTensileDetailsByCD($callFrom) {
    var processPlan = $("#hfID_ProcessPlanCD").val();
    // Do not proceed if request is already running
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    tensileCD = $.trim($callFrom.closest("tr").attr("data-lblTensileCD"));
    var chuuban = $.trim($callFrom.closest("tr").attr("data-lblChuuban"));
    var gyouban = $.trim($callFrom.closest("tr").attr("data-lblgyouban"));
    var actualDim = true;
    document.getElementById('FIDoneWarning').style.display = 'none';

    var jsonData = "{tensileCD :'" + tensileCD + "',chuuban :'" + chuuban + "', gyouban: '" + gyouban + "', USER_NAME: '" + userName + "' }";
    /* setShapeList();
     setDecisionList();*/

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetTensileDetailsByCD?tensileCD=" + parseInt(tensileCD) + "&chuuban=" + chuuban + "&gyouban=" + gyouban + "&CULTURE_NAME=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = null;
            //setStartSysDateTime();
            jsonResult = response.message;
            setNextProcess();
            setShapeList();
            //tDecisionList();
            setJISList();
            enableDataPanel();
            GetFICombinedDecisionDetails(chuuban, gyouban);
            setTensileDetails(jsonResult);

            window.resize(1130, 970);
            if ((jsonResult.pP_GYOUBAN.toString().length) == 1) {
                jsonResult.pP_GYOUBAN = "0" + jsonResult.pP_GYOUBAN;
            }


            $("#btnUpdate").unbind("click")
            $("#btnUpdate").click(function (event) {
                actualDim = true;

                if (!IsMandatoryFieldsFilled()) {
                    return false;
                }


                if (!isDuplicateThabaNoWithDecisionOK()) {
                    return false;
                }

                if (!checkDate($("#txtDate"))) {
                    var message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_002").text().trim(); // This message will be displayed on dialog box
                    var focusFieldID = "txtDate";
                    showMessageDialog(message, focusFieldID, WARNING);
                    return false;
                }
                if (!IsDateDifferentFromRange(true)) {
                    var message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_014").text().trim() + "(" + $("#lblErrorMessageE_Tensile_EDIT_DELETE_015").text().trim() + commonDateRange + ")";
                    var focusFieldID = "txtStartDate";
                    showMessageDialog(message, focusFieldID, WARNING);
                    return false;
                }
                //Deff. furnance no 
                if ($("#txtFurnaceNum").val() != "") {
                    if (!IsDifferentFurnanceNo()) {
                        var errorMessage = $("#lblErrorMessageE_Tensile_ENTRY_010").text();
                        var focusFieldID = "txtFurnaceNum";
                        showMessageDialog(errorMessage, focusFieldID, WARNING);
                        return false;
                    }
                }


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
                    var message = $("#lblConfirmationMessageC_TENSILE_EDITDELETE_001").text().trim(); //ActDimOutOfTolerance
                    var focusFieldID = "txtActDim";
                    showMessageDialog(message, focusFieldID, WARNING);
                    return false;

                }
                ////Invalid tensile
                if (!InvalidTensileValues()) {
                    //if ($("#ddlDecision").val() == 10) {
                    //    var message = $("#lblErrorMessageE_Tensile_ENTRY_011").text().trim();
                    //    showMessageDialog(message, "", WARNING);
                    return false;
                    //}
                }

                else if (fICombinedDecisions.length > 0) {
                    var tensileDecision = $("#ddlDecision").val();
                    for (i = 0; i < fICombinedDecisions.length; i++) {
                        if (CombinedDecisionArr.includes(parseInt(FIResultUntested)) && SparkDecisionArr.includes(parseInt(FISparkOk)) && !(CombinedDecisionArr.includes(parseInt(FIResultNg))) && !(CombinedDecisionArr.includes(parseInt(FIResultSparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkUntested))) && !(SparkDecisionArr.includes(parseInt(FISparkNg))) && !(SparkDecisionArr.includes(null))) {
                            if (tensileDecision == FIResultOk) {
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
                                            ValidateTensileDetailsOnUpdateBtn();
                                        }
                                        else {
                                            bootbox.hideAll();
                                            $("#ddlDecision").focus();
                                        }


                                    },
                                    centerVertical: true
                                });
                            }
                            else if (tensileDecision != FIResultOk) {
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
                                            ValidateTensileDetailsOnUpdateBtn();
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
                                ValidateTensileDetailsOnUpdateBtn();
                            }
                        }
                        else if (CombinedDecisionArr.includes(parseInt(FIResultOk))) {
                            if (tensileDecision != FIResultOk) {
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
                                            ValidateTensileDetailsOnUpdateBtn();
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
                                ValidateTensileDetailsOnUpdateBtn();
                            }
                        }
                        else {
                            ValidateTensileDetailsOnUpdateBtn();
                        }
                        break;
                    }
                }
                else {
                    ValidateTensileDetailsOnUpdateBtn();
                }
            });


            $("#txtSplitNo").focus();

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


}
function UpdateFICombinedDecision() {
    var ajaxTime = new Date().getTime();
    var tensileDecision = $("#ddlDecision").val();
    var processPlanCd = $("#hfId_ProcessPlanCD").val();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/UpdateFICombinedDecision?tensileDecision=" + tensileDecision + "&processplancd=" + processPlanCd,
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
        url: APIURL + "/Tensile/GetFurnanceNo",
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
            //    var hd_Furnace_No = jsonResult.hD_FURNACE_NO;
            //    if (hd_Furnace_No != txtFurnance_No) {
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
                if (response.message.hD_FURNACE_NO != null) {
                    var txtFurnance_No = ($("#txtFurnaceNum").val()).toUpperCase();
                    var hd_Furnace_No = jsonResult.hD_FURNACE_NO;
                    if (hd_Furnace_No != txtFurnance_No) {
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
            showMessageDialog(message, focusFieldID, ERROR);
        });
    return returnval;
}

//InvalidTensileValues
function InvalidTensileValues() {

    var endTensileStrength = $("#txtTensileStrength").val();
    var endYieldPoint = $("#txtYieldYS").val();
    var endElongation = $("#txtElongation").val();
    var endReduction = $("#txtReduction").val();
    var tensileStrength1 = $("#txtTensileStrength1").val().split(' ');
    //var textArray = tensileStrength1.split(' ');
    var yieldPoint1 = ($("#txtYieldPoint1").val().split(' '));
    var elongation1 = ($("#txtElongation1").val().split(' '));
    var reduction1 = ($("#txtReduction1").val().split(' '));
    var returnval = true;
    var tensilKubun = $("#hdfTensileKubunType").val();
    var elongationKubun = $("#hdfElongationKubunType").val();
    //var hardnessKubun = $("#hdfHardnessKubunType").val();
    var yeildKubun = $("#hdfYeildKubunType").val();
    var pressKubun = $("#hdfPressKubunType").val();
    var result = ActualTensileValue();
    if (result == false) {
        return false;
    }
    //}
    if ($("#ddlDecision").val() == 10) {
        //Tensile Strength
        if (tensileStrength1 != "") {
            if (CheckPatternValidation(tensilKubun, tensileStrength1, parseFloat($("#txtTensileStrength").val()))) {
                returnval = true;
            }
            else {
                return false;
            }
        }
        else {
            returnval = true
        }
        //Yield Point
        if (yieldPoint1 != "") {
            if (CheckPatternValidation(yeildKubun, yieldPoint1, parseFloat($("#txtYieldYS").val()))) {
                returnval = true;
            }
            else {
                return false;
            }
        }
        else {
            returnval = true
        }
        //Elongation
        if (elongation1 != "") {
            if (CheckPatternValidation(elongationKubun, elongation1, parseFloat($("#txtElongation").val()))) {
                returnval = true;
            }
            else {
                return false;
            }
        }
        else {
            returnval = true
        }
        //Reduction
        if (reduction1 != "") {
            if (CheckPatternValidation(pressKubun, reduction1, parseFloat($("#txtReduction").val()))) {
                returnval = true;
            }
            else {
                return false;
            }
        }
        else {
            returnval = true
        }
    }
    return returnval;

}
function CheckPatternValidation(kubunType, stdPatternVal, calVal) {
    var stdMinLimitVal;
    var stdMaxLimitVal;
    var calculateVal;
    var returnval = false;
    var prodSpecMinVal = $("#hdfProdSpecMin").val();
    var prodSpecMaxVal = $("#hdfProdSpecMax").val();
    if (!Number.isNaN(calVal)) {
        if (kubunType == prodSpecMinVal) {
            stdMinLimitVal = parseFloat(stdPatternVal[0]); // 980
            calculateVal = calVal; //900

            if (calculateVal > stdMinLimitVal) {
                returnval = true;  // data will save
            }
            else {
                returnval = false; // message will display
                var message = $("#lblErrorMessageE_InvalidTensileValues").text().trim();
                showMessageDialog(message, "", WARNING);
                return false;
            }
        }
        else if (kubunType == prodSpecMaxVal) {
            stdMinLimitVal = parseFloat(stdPatternVal[0]);
            calculateVal = calVal;
            if (calculateVal < stdMinLimitVal) {
                returnval = true;
            }
            else {
                returnval = false;
                var message = $("#lblErrorMessageE_InvalidTensileValues").text().trim();
                showMessageDialog(message, "", WARNING);
                return false;
            }
        }
        else if (String(stdPatternVal).includes("～") && kubunType == 0) {
            var stdMinMaxVal = String(stdPatternVal).split("～");
            stdMinLimitVal = parseFloat(stdMinMaxVal[0]);
            stdMaxLimitVal = parseFloat(stdMinMaxVal[1]);
            calculateVal = calVal;
            if (calculateVal > stdMinLimitVal && calculateVal < stdMaxLimitVal) {
                returnval = true;
            } else {
                returnval = false;
                var message = $("#lblErrorMessageE_InvalidTensileValues").text().trim();
                showMessageDialog(message, "", WARNING);
                return false;
            }
        }
        else {
            returnval = true;
        }
    }
    else {
        returnval = true;
    }
    return returnval;
}
$("#btnDelete").click(function (event) {
    DeleteTensileDetails();
});
$("#btnInitialize").click(function (event) {

    switchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));
    initialize();
    disableEditDeleteButtons();
    disableDataPanel();
    // btnSearch_Click(true);

    window.resizeTo(1090, 890);
    location.reload();


});
function ClearDataAfterDelete() {  //clear screen data after delete record

    initialize();
    disableEditDeleteButtons();
    disableDataPanel();
}

$("#btnDateTime").click(function (event) {
    setStartSysDateTime();

});



$("#btnActual").click(function (event) {
    ActualTensileValue();
});
function ActualTensileValue() {
    var yieldLoadN = null;
    var actDim = null;
    var cutDim = null;
    $("#txtYieldYS").val("");
    $("#txtReduction").val("");
    $("#txtElongation").val("");
    $("#txtTensileStrength").val("");
    var returnVal = true;
    //TS_YIELD_STRENGTH
    if ($("#txtYieldLoadN").val().length >= 1 && $("#txtActDim").val().length >= 1) {
        yieldLoadN = $("#txtYieldLoadN").val();
        actDim = $("#txtActDim").val();
        var result1 = (yieldLoadN / (3.14 * (actDim / 2) * (actDim / 2))).toFixed(2);

        var numResult = Number(result1);
        // Check if result fits within NUMBER(4,0)

        if (numResult >= -999 && numResult <= 9999) {
            $("#txtYieldYS").val(Math.round(Number(result1)));
            var txtYieldYS = document.getElementById("txtYieldYS");
            txtYieldYS.style.color = "black";
            txtYieldYS.style.textAlign = "right";
            returnVal = true;
        } else {
            var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
            var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
            $("#txtYieldYS").val(errorLable);
            var txtYieldYS = document.getElementById("txtYieldYS");
            txtYieldYS.style.color = "red";
            txtYieldYS.style.textAlign = "left";
            focusFieldID = $(':focus').attr("id");
            showMessageDialog(message, focusFieldID, WARNING);
            returnVal = false;
            return false;

        }
    }


    //TS_REDUCTION
    if ($("#txtCutDim").val().length >= 1 && $("#txtActDim").val().length >= 1) {
        cutDim = $("#txtCutDim").val();
        actDim = $("#txtActDim").val();
        //var result2 = ((((cutDim * cutDim * 0.785) - (actDim * actDim * 0.785)) * (-100)) / (-(actDim * actDim * 0.785))).toFixed(1);   ////for '-' sign line commented as per windows code
        var result2 = ((((cutDim * cutDim * 0.785) - (actDim * actDim * 0.785)) * (-100)) / ((actDim * actDim * 0.785))).toFixed(1);
        var numResult = Number(result2);
        // Check if result fits within NUMBER(3,1)

        if (numResult >= -99.9 && numResult <= 99.9) {
            $("#txtReduction").val(result2);
            var txtReduction = document.getElementById("txtReduction");
            txtReduction.style.color = "black";
            txtReduction.style.textAlign = "right";
            returnVal = true;
        } else {
            var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
            var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
            $("#txtReduction").val(errorLable);
            var txtReduction = document.getElementById("txtReduction");
            txtReduction.style.color = "red";
            txtReduction.style.textAlign = "left";
            focusFieldID = $(':focus').attr("id");
            showMessageDialog(message, focusFieldID, WARNING);
            returnVal = false;
            return false;

        }

    }
    //TS_ELONGATION

    if ($("#txtCuttingElongation").val().length >= 1 && $("#txtGaugeLength").val().length >= 1) {
        var cuttingElongation = $("#txtCuttingElongation").val();
        var gaugeLength = $("#txtGaugeLength").val();
        var result3 = (((cuttingElongation - gaugeLength) * 100) / gaugeLength).toFixed(1);
        //159900 =(((800 - 0.5) * 100) / 0.5).toFixed(1); //error
        var numResult = Number(result3);

        // Check if result fits within NUMBER(3,1)

        if (numResult >= -99.9 && numResult <= 99.9) {
            $("#txtElongation").val(result3);
            var txtElongation = document.getElementById("txtElongation");
            txtElongation.style.color = "black";
            txtElongation.style.textAlign = "right";
            returnVal = true;
        } else {
            var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
            var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
            $("#txtElongation").val(errorLable);
            var txtElongation = document.getElementById("txtElongation");
            txtElongation.style.color = "red";
            txtElongation.style.textAlign = "left";
            focusFieldID = $(':focus').attr("id");
            showMessageDialog(message, focusFieldID, WARNING);
            returnVal = false;
            return false;
        }

    }

    //TS_TENSILE
    if ($("#txtMaxLoadN").val().length >= 1 && $("#txtActDim").val().length >= 1) {
        var maxLoadN = parseFloat($("#txtMaxLoadN").val());
        var actDim = parseFloat($("#txtActDim").val());


        var result;

        // Calculate based on selected shape
        if ($("#ddlDimShape").val() == 2) {
            result = (maxLoadN / (actDim * actDim * 0.866)).toFixed(3);
        } else if ($("#ddlDimShape").val() == 1) {
            result = (maxLoadN / (actDim * actDim * 0.785)).toFixed(3);
        } else if ($("#ddlDimShape").val() == 3) {
            result = (maxLoadN / (actDim * actDim)).toFixed(3);
        }

        // Convert to number and validate NUMBER(7,3)
        var numResult = parseFloat(result);
        if (numResult >= -9999.999 && numResult <= 9999.999) {
            $("#txtTensileStrength").val(Math.round(result));
            var txtTensileStrength = document.getElementById("txtTensileStrength");
            txtTensileStrength.style.color = "black";
            txtTensileStrength.style.textAlign = "right";
            returnVal = true;
        } else {
            var message = $("#lblErrorMessageE_Tensile_ENTRY_012").text().trim();
            var errorLable = $("#lblErrorMessageE_Tensile_ENTRY_013").text().trim();
            $("#txtTensileStrength").val(errorLable);
            var txtTensileStrength = document.getElementById("txtTensileStrength");
            txtTensileStrength.style.color = "red";
            txtTensileStrength.style.textAlign = "left";
            focusFieldID = $(':focus').attr("id");
            showMessageDialog(message, focusFieldID, WARNING);
            returnVal = false;
            return false;
        }
    }
    return returnVal;
}
function initialize() {

    $("#txtActDim").removeClass("Mandatory");

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
    $("#txtChuubanSearch").focus();
    clearPanel($("#pnlProductDetails"));
    clearPanel($("#pnlSearch"));
    clearPanel($("#pnlProcessStartData"));
    clearPanel($("#pnlProcessEndData"));
    clearPanel($("#pnlBottomData"));
    clearPanel($("#pnlModel"));
    clearTable($("#grdvTensileList"), "", "GridNormalRow");
    clearTable($("#grdvTensileList"), "", "GridAlternateRow");

    resetGlobalVariables();
    disableEditDeleteButtons();
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
    lenStd;
    IsOprExists = false;
    chuuban = null;
    gyouban = null;
    userName = "";
    combinedDecisionOK = "";
    processPlanCD = "";
    $btnActual;
    tensileCD;
    $("#txt_Chuuban1").hide();
    $("#ddlModel").hide();
    $("#lblModel").hide();

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    $(".FocusDefault").focus();

    Mask();
    userName = $("#hdfUserName").val();

    setShapeList();
    //DecisionList();
    setJISList();
    setNextProcess();
    // btnSearch_Click(true);
    disableEditDeleteButtons();
    disableDataPanel();
    setSearchStartEndDateTime();
    $("#ddlModel").removeClass("Mandatory");
    $("#txtLastSplitNo").attr(disabled, disabled).addClass(disabled);
    $("#chkActualDim").prop('checked', true);
    document.getElementById('FIDoneWarning').style.display = 'none';
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


///Delete
function DeleteTensileDetails() {

    var ajaxTime = new Date().getTime();
    var jsonData = JSON.stringify({
        MI_TS_CD: $("#hfId_TensileCD").val(),
        PROCESS_PLAN_CD: $("#hfID_ProcessPlanCD").val(),
        DETAIL_NO: $("#txtSplitNo").val(),
        MSG_CD: $("#hfId_MessageCD").val() ? $("#hfId_MessageCD").val() : null
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/TensileDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            //clearPanel($("#pnlProcessStartData"));
            //clearPanel($("#pnlProcessEndData"));
            //clearPanel($("#pnlBottomData"));
            //clearPanel($("#pnlModel"));
            //clearPanel($("#pnlProductDetails"));
            //resetGlobalVariables();
            disableEditDeleteButtons();
            disableDataPanel();
            if (response.message == "0") { //0:Data not Deleted  
                var successMessage = $("#valMsgDeleteNotAllowedUsedInMillSheet").text();
                showMessageDialog(successMessage, "btnInitialize", WARNING);
            }
            else if (response.message == "1") { //1:Data Deleted Successfully 
                var successMessage = $("#lblSuccessMessageS_Tensile_EDIT_DELETE_002").text();
                showMessageDialog(successMessage, "btnInitialize", SUCCESS);
                //btnSearch_Click(false);
                TensileGridSearch(null);
                ClearDataAfterDelete();
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);

        });
}

///Update
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

        showMessageDialog(message, focusFieldID, WARNING)
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
    TensileGridSearch(selectedColumns);
}
function TensileGridSearch(sortFields) {

    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtCustomerName = $("#txtCustomerName").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $.trim($("#txtDimDMin").val()) ? $.trim($("#txtDimDMin").val()) : null;
    var txtDimDMax = $.trim($("#txtDimDMax").val()) ? $.trim($("#txtDimDMax").val()) : null;
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $.trim($("#txtLengthSearchMin").val()) ? $.trim($("#txtLengthSearchMin").val()) : null;
    var txtLengthSearchMax = $.trim($("#txtLengthSearchMax").val()) ? $.trim($("#txtLengthSearchMax").val()) : null;
    var txtSrchDateStart = $("#txtInspectionDateStart").val();
    var txtSrchDateEnd = $("#txtInspectionDateEnd").val();
    var userId = $("#hdfLoginId").val();

    //if (txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0) {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: txtChuubanSearch,
            CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin,
            PROD_DIM_D_MAX: txtDimDMax, PROD_SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax, START_DATETIME: txtSrchDateStart, END_DATETIME: txtSrchDateEnd,
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
                    url: "/Tensile/GridSearch",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divTensileGridMain").html(response1.message);

                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
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
            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    //}
    //else {

    //    if (sortFields.length > 0) {
    //        var ajaxTime = new Date().getTime();
    //        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
    //        var settings = {
    //            async: true,
    //            crossDomain: true,
    //            cache: false,
    //            dataType: "json",
    //            url: APIURL + "/Tensile/GridSearch",
    //            method: "POST",
    //            contentType: "application/json; charset=utf-8",
    //            data: JSON.stringify({
    //                CHUUBAN: txtChuubanSearch,
    //                CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin,
    //                PROD_DIM_D_MAX: txtDimDMax, PROD_SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax,
    //                USER_ID: userId, SORT_FIELDS: sortFields
    //            })
    //        }
    //        $.ajax(settings)
    //            .done(function (response) {
    //                responseTime = new Date().getTime() - ajaxTime;
    //                if (response.isSuccess) {
    //                    var settings1 = {
    //                        async: true,
    //                        crossDomain: true,
    //                        cache: false,
    //                        dataType: "json",
    //                        url: "/Tensile/GridSearch",
    //                        method: "POST",
    //                        contentType: "application/json; charset=utf-8",
    //                        data: JSON.stringify(response.message)
    //                        //data: "{jsonData : " + jsonData + "}"
    //                    }
    //                    $.ajax(settings1)
    //                        .done(function (response1) {
    //                            if (response1.isSuccess) {
    //                                $("#divTensileGridMain").html(response1.message);

    //                            }
    //                        })
    //                        .fail(function (xhr, textStatus, errorThrown) {
    //                            responseTime = new Date().getTime() - ajaxTime;
    //                            var errorCode = $.parseJSON(xhr.responseText).Message;

    //                            focusFieldID = $(':focus').attr("id");
    //                            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
    //                            showMessageDialog(message, focusFieldID, ERROR);
    //                        });
    //                }
    //                else {
    //                    bootbox.alert({
    //                        title: "<div style='color:white'></div>",
    //                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
    //                        centerVertical: true
    //                    });
    //                }

    //            })
    //            .fail(function (xhr, textStatus, errorThrown) {
    //                responseTime = new Date().getTime() - ajaxTime;
    //                var errorCode = $.parseJSON(xhr.responseText).Message;

    //                focusFieldID = $(':focus').attr("id");
    //                message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
    //                showMessageDialog(message, focusFieldID, ERROR);
    //            });
    //    }

    //}
    closepopup();
}
function IsDropDownMandatory() {
    var returnVal = true;
    var message = "";
    if ($("#ddlOperatorNameValue").val() == "0") {
        var focusFieldID = "ddlOperatorNameValue";
        var message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_003").text());
        showMessageDialog(message, focusFieldID, WARNING);
        returnVal = false;
    }
    return returnVal;
}

//Server Site : Check old thaba present with OK decision 
function isDuplicateThabaNoWithDecisionOK() {
    var tabaNo;
    var returnVal = true;
    var ajaxTime = new Date().getDate();
    var processPlan_CD = processPlanCD;
    tabaNo = $("#txtSplitNo").val();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetThabaDetailsByThabaNumber?processPlanCD=" + processPlan_CD + "&tabaNo=" + tabaNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult == null) {
                returnVal = true;
            } else {
                if (jsonResult.mI_TS_CD == tensileCD) { //need to confirm tensile cd logic for compair
                    returnVal = true;
                } else {
                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_Tensile_OkDecision").text().trim().replace("NNN", tabaNo);

                    showMessageDialog(message, focusFieldID, WARNING);

                    returnVal = false;
                }

            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

    return returnVal;
}

function IsActiveOperator(OprCD) {

    if (IsOprExists == false && $("#ddlOperatorNameValue").val() == OprCD) {
        var focusFieldID = "ddlOperatorNameValue";
        var message = $("#hfId_OperatorInactiveMessage").val();
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    else {
        return true;
    }
}
//$("#chkActualDim").click(function () {

//    if ($("#chkActualDim").prop("checked") == true) {
//        $("#txtActDim").addClass("Mandatory");
//    } else {

//        $("#txtActDim").removeClass("Mandatory");
//    }
//});

function setDecimalFormats() {
    $("#txtMaxLoadN").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 1);
    });
    $("#txtGaugeLength").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtTestSpecimentDim, #txtCuttingElongation").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
    $("#txtActDim").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtYieldLoadN").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 1);
    });
    $("#txtCutDim").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
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
function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/Tensile/InitializeSortPopup",
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
function btnSortReset_Click() {
    InitSortPopup();
}
function btnSortClose_Click() {
    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
//Validation for Update button
function ValidateTensileDetailsOnUpdateBtn() {

    var ajaxTime = new Date().getTime();
    var MI_TS_CD = $("#hfId_TensileCD").val();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/Tensile/ValidateTensileDetailsOnUpdateBtn?MITensileCD=" + MI_TS_CD,
        method: "POST",
        contentType: "application/json; charset=utf-8"

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;


            if (response.message == "0") { //0:Data not Deleted  
                var successMessage = $("#valMsgEditNotAllowedUsedInMillSheet").text();
                showMessageDialog(successMessage, "btnInitialize", WARNING);
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
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function UpdateDetails($callFrom) {

    var ajaxTime = new Date().getDate();
    var jsonDataForSave = JSON.stringify({
        // START DATA
        MSG_CD: $("#hfId_MessageCD").val() ? $("#hfId_MessageCD").val() : null,
        MI_TS_CD: $("#hfId_TensileCD").val(),
        PROCESS_PLAN_CD: $("#hfId_ProcessPlanCD").val(),
        TS_TEST_OPERATOR_CD: parseInt($("#ddlOperatorNameValue").find(":selected").val()),//$("#ddlOperatorNameValue").val(),
        TS_TEST_DATE: ($("#txtDate").val().split(" ")[0] + "T" + $("#txtDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        TS_FURNACE_NO: ($("#txtFurnaceNum").val() || "").toUpperCase() || null,
        DETAIL_NO: $("#txtSplitNo").val(),
        TS_MAX_LOAD_N: $("#txtMaxLoadN").val(),

        TEST_SPECIMEN_CD: $("#ddlJISTest option:selected").val() ? $("#ddlJISTest option:selected").val() : null,
        IS_ACT_DIM_RANGE_CHECK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        TS_ACT_DIM: $("#txtActDim").val() ? $("#txtActDim").val() : null,

        TS_ACT_DIM_SHAPE: $("#ddlDimShape").val() ? $("#ddlDimShape").val() : null,
        TS_TEST_SPECIMEN_DIM: $("#txtTestSpecimentDim").val() ? $("#txtTestSpecimentDim").val() : null,
        TS_GAUGE_LENGTH: $("#txtGaugeLength").val() ? $("#txtGaugeLength").val() : null,
        TS_YIELD_LOAD_N: $("#txtYieldLoadN").val() ? $("#txtYieldLoadN").val() : null,
        TS_TEST_IMPACT_1: $("#txtBreakTest1").val() ? $("#txtBreakTest1").val() : null,
        TS_TEST_IMPACT_2: $("#txtBreakTest2").val() ? $("#txtBreakTest2").val() : null,
        TS_TEST_IMPACT_3: $("#txtBreakTest3").val() ? $("#txtBreakTest3").val() : null,
        TS_CUT_DIM: $("#txtCutDim").val() ? $("#txtCutDim").val() : null,
        TS_EXTENDED_LENGTH: $("#txtCuttingElongation").val() ? $("#txtCuttingElongation").val() : null,

        TS_DECISION: $("#ddlDecision").val(),
        //TS_NEXT_PROCESS: $("#ddlnextprocess").val(),
        REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,

        TS_TENSILE: $("#txtTensileStrength").val() ? $("#txtTensileStrength").val() : 0,
        TS_YIELD_STRENGTH: $("#txtYieldYS").val() ? $("#txtYieldYS").val() : 0,
        TS_ELONGATION: $("#txtElongation").val() ? $("#txtElongation").val() : 0,
        TS_REDUCTION: $("#txtReduction").val() ? $("#txtReduction").val() : 0,

        //TS_TENSILE: $("#txtTensileStrength").val() ? $("#txtTensileStrength").val() : null,
        //TS_YIELD_STRENGTH: $("#txtYieldYS").val() ? $("#txtYieldYS").val() : null,
        //TS_ELONGATION: $("#txtElongation").val() ? $("#txtElongation").val() : null,
        //TS_REDUCTION: $("#txtReduction").val() ? $("#txtReduction").val() : null,

        USER_ID: $("#hdfLoginId").val()

        //USER_NAME:  userName
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/TensileDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: jsonDataForSave
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (parseInt(response.message) > 0) {
                $("#hfId_MessageCD").val(response.message);
            }
            else {
                $("#hfId_MessageCD").val("");
            }
            var successMessage = $.trim($("#lblSuccessMessageS_Tensile_EDIT_DELETE_001").text());
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            TensileGridSearch(null);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
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
function btnSearch_Click(allowNull = false) {

    var txtChuubanSearch = $("#txtChuubanSearch").val();// ? $("#txtChuubanSearch").val() : null;
    var txtCustomerName = $("#txtCustomerName").val();// ? $("#txtCustomerName").val() : null;
    var txtProdNameCust = $("#txtProdNameCust").val();// ? $("#txtProdNameCust").val() : null;
    var txtProdNameInt = $("#txtProdNameInt").val();// ? $("#txtProdNameInt").val() : null;
    var txtDimDMin = $.trim($("#txtDimDMin").val()) ? $.trim($("#txtDimDMin").val()) : null; //$("#txtDimDMin").val() ? $("#txtDimDMin").val() : 0;
    var txtDimDMax = $.trim($("#txtDimDMax").val()) ? $.trim($("#txtDimDMax").val()) : null;//$("#txtDimDMax").val() ? $("#txtDimDMax").val() : 0;
    var txtShap = $("#txtShap").val();// ? $("#txtShap").val() : null;
    var txtLengthSearchMin = $.trim($("#txtLengthSearchMin").val()) ? $.trim($("#txtLengthSearchMin").val()) : null;//$("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : 0;
    var txtLengthSearchMax = $.trim($("#txtLengthSearchMax").val()) ? $.trim($("#txtLengthSearchMax").val()) : null;//$("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : 0;
    var txtSrchDateStart = $("#txtInspectionDateStart").val();
    var txtSrchDateEnd = $("#txtInspectionDateEnd").val();
    var userId = $("#hdfLoginId").val();
    var errorMessage = "";
    var checkFocusField = "";
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
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }

    if (allowNull || txtChuubanSearch.length > 0 || txtCustomerName.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || parseInt(txtDimDMin) > 0 || parseInt(txtDimDMax) > 0 || txtShap.length > 0 || parseInt(txtLengthSearchMin) > 0 || parseInt(txtLengthSearchMax) > 0 || txtSrchDateStart.length > 0 || txtSrchDateEnd.length > 0) {


        var ajaxTime = new Date().getTime();
        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Tensile/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: txtChuubanSearch,
                CUSTOMER_NAME: txtCustomerName, PROD_NAME_CUST: txtProdNameCust, INTERNAL_NAME_STRING: txtProdNameInt, PROD_DIM_D_MIN: txtDimDMin,
                PROD_DIM_D_MAX: txtDimDMax, PROD_SHAPE: txtShap, PROD_LENGTH_MIN: txtLengthSearchMin, PROD_LENGTH_MAX: txtLengthSearchMax, START_DATETIME: txtSrchDateStart, END_DATETIME: txtSrchDateEnd,
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
                        url: "/Tensile/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divTensileGridMain").html(response1.message);

                                $("#btnLnkEdit").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
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
                message = $.trim($("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {
        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessageE_Tensile_Searchcondition").text()) + "</td></tr></table>",
            centerVertical: true

        });
    }
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
        url: APIURL + "/Tensile/GetFICombinedDecisionDetails",
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

$(document).ready(function () {


    $("#txtChuubanSearch").focus();
    $("#txt_Chuuban1").hide();
    $("#ddlModel").hide();
    $("#lblModel").hide();

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    $(".FocusDefault").focus();

    Mask();
    userName = $("#hdfUserName").val();
    setShapeList();
    setDecisionList();
    setJISList();
    setNextProcess();
    // btnSearch_Click(true);
    disableEditDeleteButtons();
    disableDataPanel();
    setSearchStartEndDateTime();
    $("#ddlModel").removeClass("Mandatory");
    $("#txtLastSplitNo").attr(disabled, disabled).addClass(disabled);

});
$("#btnActual").focusout(function (event) {
    $("#btnUpdate").focus();
});
$("#txtActDim").change(function (event) {
    if ($("#txtActDim").val() != null && $("#txtActDim").val().trim() !== "") {
        $("#ddlDimShape").removeAttr(disabled).removeClass(disabled);
        $("#ddlDimShape").addClass("Mandatory");
        $("#ddlDimShape").focus();
    }
    else {
        $("#txtTestSpecimentDim").focus();
        $("#ddlDimShape").attr(disabled, disabled).addClass(disabled);
        $("#ddlDimShape").removeClass("Mandatory");
        setShapeList();
    }
});
$("#ddlJISTest").change(function (event) {
    var ajaxTime = new Date().getTime();
    var testSpecCd = $("#ddlJISTest option:selected").val() ? $("#ddlJISTest option:selected").val() : null;
    var jsonData = "{ USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetJISTestMagnification?testSpecCd=" + testSpecCd,
        method: "POST",
        contentType: "application/json; charset=utf-8"
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var testSpecMagnification = response.message;
            if ($("#txtActDim").val() != null && $("#txtActDim").val().trim() !== "" && testSpecCd != 0) {
                var actDimVal = $("#txtActDim").val();
                $("#txtGaugeLength").val(Number((Number(actDimVal) * testSpecMagnification).toFixed(2)));
                $("#txtGaugeLength").attr(disabled, disabled).addClass(disabled);
            }
            else {
                $("#txtGaugeLength").removeClass(disabled).removeAttr(disabled);
                $("#txtGaugeLength").val("");
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
});

//barCode scan on list page
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
            url: APIURL + "/Tensile/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                message = $("#lblErrorMessageE_Tensile_EDIT_DELETE_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});