
var typeText = "text";
var typeDropDown = "select-one";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var userName = '';
var operatorCD = 0;
var gridRows = 30;

var operatorAssembleVerifyByModified = false;
let enterPressedAssembleVerifyByOpr = false;

var operatorFurncInVerifyByModified = false;
let enterPressedFurncInVerifyByOpr = false;

var operatorFurnaceOutInchargeModified = false;
let enterPressedFurnaceOutInchargeOpr = false;

var operatorFurnaceVerifyByModified = false;
let enterPressedFurnaceVerifyByOpr = false;
//function onLoad() {
//    window.resizeTo(1370, 850);
//}
//document.addEventListener('keyup', function (e) {

//    var fieldname = e.target.id;
//    var aa = $("#" + fieldname).data("id");

//    if ($("#" + fieldname).data("orderno") === 'yes') {
//        if ($("#" + fieldname).val() == '' || $("#" + fieldname).val() == null) {
//            $("#" + fieldname).removeClass("Mandatory", "Mandatory");
//        } else {

//            $("#" + fieldname).addClass("Mandatory", "Mandatory");
//        }

//    }
//});

function hideBlankRow() {
    $(".hHide").remove();
    $(".dHide").remove();
};

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


function setMachineList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetHeatTreatmentMachineNameList",
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
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMachineDropdown(jsonResult) {
    debugger;
    $("#ddlMachine").html("").append($('<option value="0"></option>'));
    $("#ddlSrchMachine").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMachine").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
        $("#ddlSrchMachine").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setProcConditionKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/GetProcConditionKubun",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            bindProcConditionKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcConditionKubunDropdown(jsonResult) {
    // $("#ddlProcCondition").html("").append($('<option value="0"></option>'));
    $("#ddlProcCondition").html("");//.append($('<option value=""></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcCondition").append($('<option value="' + jsonObj.name + '">' + jsonObj.name + '</option>'));
        if ($("#txtProcCondition").val() == "") {
            $("#txtProcCondition").val(jsonObj.name);
        }
    });
}

//function setStartOperatorListStartDetails(yobina, defaultSelectID) {

//    getOperatorList(yobina, $("#ddlscAssembleVerifyBy"), defaultSelectID);
//};

//function setEndOperatorListStartDetails(yobina, defaultSelectID) {
//    getOperatorList(yobina, $("#ddlscFurncInVerifyBy"), defaultSelectID);
//};

//function setStartOperatorListEndDetails(yobina, defaultSelectID) {

//    getOperatorList(yobina, $("#ddlFurnaceOutIncharge"), defaultSelectID);
//};

//function setEndOperatorListEndDetails(yobina, defaultSelectID) {
//    getOperatorList(yobina, $("#ddlFurnaceVerifyBy"), defaultSelectID);
//};

//function getOperatorList(yobina, $target) {

//    var ajaxTime = new Date().getTime();
//    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/HeatTreatment/GetOperatorList?name=" + yobina + "&operatorCD=" + operatorCD,
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonData + "}",

//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;

//            var result = response.message;
//            bindOperatorDropdown(response.message, $target, yobina);

//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $("#lblErrorMessageE_HeatTreat_001").text().trim();
//            showMessageDialog(message, focusFieldID, ERROR);
//        })
//}

//function bindOperatorDropdown(jsonResult, $target, defaultSelectID) {
//    $target.html("").append($('<option value="0"></option>'));
//    $.each(jsonResult, function (key, jsonObj) {
//        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '">' + jsonObj.name + '</option>'));
//    });
//    if (defaultSelectID != null && defaultSelectID != "") {
//        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
//        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
//    }
//}



//function FillEmptyGrid() {

//    var ajaxTime = new Date().getTime();
//    var jsonData = "{USER_NAME: '" + userName + "' }";
//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/HeatTreatment/FillEmptyGrid",
//        method: "GET",
//        contentType: "application/json; charset=utf-8",
//        //data: "{jsonData : " + jsonData + "}"
//    }
//    $.ajax(settings)
//        .done(function (response) {

//            responseTime = new Date().getTime() - ajaxTime;
//            if (response.isSuccess) {
//                FillGrid(response.message, false);
//            }
//        })
//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}

function SetStartDetails(jsonResult) {
    var startDetails = jsonResult[0];
    $("#ddlMachine").val(startDetails.machinE_CD);
    $("#txtFurnaceNo").val(startDetails.furnacE_NO);
    $("#ddlProcCondition").val(startDetails.proC_CONDITION);
    $("#txtPatternNo").val(startDetails.patterN_NO);
    $("#txtFurnaceInDateTime").val($.format.date(new Date(jsonResult.hT_START_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //$("#ddlscAssembleVerifyBy").val(startDetails.setuP_CONF_BY);
    //$("#ddlscFurncInVerifyBy").val(startDetails.loadinG_CONF_BY);
    SetOperatorDropdown($("#ddlscAssembleVerifyBy"), jsonresult[0].setuP_CONF_BY);
    SetOperatorDropdown($("#ddlscFurncInVerifyBy"), jsonresult[0].loadinG_CONF_BY);
    $("#txtPlannedUploadDate").val(startDetails.planneD_UNLOAD_DATE);
    EnableDisableData(true);
}

function btnSearchByFurnace_Click() {

    disableGridCheckBox();
    //setStartOperatorListStartDetails($("#txtscAssembleVerifyByInputSearch").val(), "");
    //setEndOperatorListStartDetails($("#txtscFurncInVerifyByInputSearch").val(), "");

    var machineCd = $("#ddlSrchMachine").val() ? $("#ddlSrchMachine").val() : null;
   // var furNo = $("#txtSrchFurnaceNo").val() ? $("#txtSrchFurnaceNo").val() : null;

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetHeatTreatmentStartDetails?machineCD=" + machineCd,// + "&isNull=" + true,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            /*if (response.isSuccess) {*/

            if (response.message.length > 0) {

                for (k = 1; k < 30; k++) {
                    clearTableRow("trId" + k);
                    var aa = $("#hdfProcessPlanCD" + k).text("");
            //        $("#ddlMatlType" + k).removeClass("Mandatory2", "Mandatory2");
                    //  $("#txtLotNo" + k).removeClass("Mandatory2", "Mandatory2");
                    $("#txtFurnacePosition1" + k).removeClass("Mandatory2", "Mandatory2");
                    $("#txtFurnacePosition2" + k).removeClass("Mandatory2", "Mandatory2");
                    $("#ddlNextProcess" + k).removeClass("Mandatory2", "Mandatory2");
                }

                var jsonresult = response.message;
                if (jsonresult[0].hT_END_DATE != null) {
                    var message = $("#lblErrorMessageE_HeatTreat_025").text();
                    showMessageDialog(message, "", WARNING);
                }
                else {

                    $("#ddlMachine").val(jsonresult[0].machinE_CD);
                    $("#txtFurnaceNo").val(jsonresult[0].furnacE_NO);
                    // $("#ddlProcCondition").val(jsonresult[0].hT_CONDITION);
                    $("#ddlProcCondition").append($('<option>' + jsonresult[0].hT_CONDITION + '</option>'));
                    $("#txtProcCondition").val(jsonresult[0].hT_CONDITION);
                    $("#txtPatternNo").val(jsonresult[0].patterN_NO);
                    debugger;
                    $("#txtFurnaceInDateTime").val($.format.date(new Date(jsonresult[0].hT_START_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                    $("#txtscAssembleVerifyByInputSearch").val(jsonresult[0].setuP_CONF_BY_EMPLOYEE_NAME); 
                    $("#hdftxtscAssembleVerifyByInputSearch").val(jsonresult[0].setuP_CONF_BY);
                    $("#txtscFurncInVerifyByInputSearch").val(jsonresult[0].loadinG_CONF_BY_EMPLOYEE_NAME);
                    $("#hdftxtscFurncInVerifyByInputSearch").val(jsonresult[0].loadinG_CONF_BY);
                    if (jsonresult[0].planneD_UNLOAD_DATE == null) {
                        $("#txtPlannedUploadDate").val("");
                    } else {
                        $("#txtPlannedUploadDate").val($.format.date(new Date(jsonresult[0].planneD_UNLOAD_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
                    }
                   
                    $("#txtHtHeaderRemark").val(jsonresult[0].hT_HEADER_REMARK);

                    //grid data
                    $.each(jsonresult, function (key, jsonObj) {
                        key = parseInt(key) + 1;
                        $("#txtOrderNo" + key).val(jsonObj.chuuban.replace(/-/g, ''));
                        $("#txtOrderNo" + key).addClass("Mandatory2", "Mandatory2");
                        $("#txtCustomer" + key).text(jsonObj.customeR_NAME);
                        $("#lblProdNameCust" + key).text(jsonObj.proD_NAME_CUST);
                        $("#lblCharge" + key).text(jsonObj.charge);
                        $("#lblMatlDim" + key).text(jsonObj.matL_DIM_STRING);
                        $("#lblProdDim" + key).text(jsonObj.proD_DIM_STRING);

                        $("#hdfProcessPlanCD" + key).text(jsonObj.procesS_PLAN_CD);
                        $("#hdfheattreatmentCD" + key).text(jsonObj.heaT_TREATMENT_CD);

                        $("#ddlMatlType" + key).val(jsonObj.coiL_BAR_TYPE);
                     //   $("#ddlMatlType" + key).addClass("Mandatory2", "Mandatory2");
                        $("#txtLotNo" + key).val(jsonObj.loT_NO);
                        //  $("#txtLotNo" + key).addClass("Mandatory2", "Mandatory2");

                        $("#txtNoOfUnit" + key).val(jsonObj.hT_NO_OF_UNITS);
                        $("#ddlUnit" + key).val(jsonObj.uniT_TYPE);
                        $("#txtQuantity" + key).val(jsonObj.hT_WEIGHT);
                        $("#txtRemark" + key).val(jsonObj.hT_REMARKS);
                        $("#hdfMsgCD" + key).text(jsonObj.msG_CD);
                        $("#txtRoll" + key).val(jsonObj.rolL_MONTH);
                        $("#txtCoilNo" + key).val(jsonObj.coiL_NO);

                        $("#txtFurnacePosition1" + key).val(jsonObj.locatioN_IN_FURNACE1);
                        $("#txtFurnacePosition1" + key).addClass("Mandatory2", "Mandatory2");
                        $("#txtFurnacePosition2" + key).val(jsonObj.locatioN_IN_FURNACE2);
                        $("#txtFurnacePosition2" + key).addClass("Mandatory2", "Mandatory2");

                        $("#txtTabaNo1" + key).val(jsonObj.hT_TABA_NO_1);
                        //$("#txtTabaNo1" + key).addClass("Mandatory2", "Mandatory2");
                        $("#txtTabaNo2" + key).val(jsonObj.hT_TABA_NO_2);
                        $("#txtTabaNo3" + key).val(jsonObj.hT_TABA_NO_3);
                        $("#txtTabaNo4" + key).val(jsonObj.hT_TABA_NO_4);
                        $("#ddlNextProcess" + key).val(jsonObj.hT_NEXT_PROCESS ? jsonObj.hT_NEXT_PROCESS : 0);
                       
                        $("#ddlNextProcess" + key).addClass("Mandatory2", "Mandatory2");

                        $("#btnSearchByChubaanPopup").attr("disabled", "disabled");

                        // $(this).parent().siblings('div.bottom').show();
                        setFurnaceOutDateTime();
                        $('#btnFurnaceOutDateTimeNow').focus();
                        $("#selectRow" + key).removeAttr("disabled");
                       

                    });

                    // setEndOperatorListStartDetails(yobina, "");

                    /*$("#btnStartDataSave").removeAttr(disabled).removeClass(disabled);*/
                    // DisableStartdetails();
                    EnableStartdetails();
                    EnableEndDetails();
                    enableDeleteRowButtons();
                    $("#btnStartDataSave").attr("disabled", "disabled");

                    $("#btnEndDataSave").removeAttr("disabled");
                    disableDeleteRowButtons();
                    disableGridCheckBox();
                }

            }
            else {

                responseTime = new Date().getTime() - ajaxTime;
                var SrchFurnaceNo = $("#txtSrchFurnaceNo").val();
                if (SrchFurnaceNo == "") {
                    var message = $("#lblErrorMessageE_HeatTreat_017").text().trim();
                    showMessageDialog(message, "txtSrchFurnaceNo", WARNING);
                }
                else {
                    var message = $("#lblErrorMessageE_HeatTreat_024").text();
                    showMessageDialog(message, "txtSrchFurnaceNo", WARNING);

                }
            }
            calculateTotalWeight();
            calculateTotalWeightOnGridLoad();
        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, "", ERROR);
        });

}

function btnSearchByCodeSearchPopUp_Click() {

    var chuban = [];
    var isOrderNoExist = false;
    var focusID = "";
    var focusID1 = "";
    var focusID2 = "";
    var focusID3 = "";
    var message = "";
    for (var i = 1; i <= 30; i++) {
        $("#hdfProcessPlanCD" + i).val("");

        var chuban1 = $("#txtOrderNo" + i).val();
        if (chuban1.trim() !== "" && chuban1.trim() !== null) {

            isOrderNoExist = true;
            chuban.push(chuban1);
        }
    }

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/SearchBychuuban?chuuban=" + chuban1,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify(chuban1)
        data: JSON.stringify(chuban)
    }
    $.ajax(settings)
        .done(function (response) {
            // return 1 for Success
            // return 2 for OrNo not found
            // return 3  for OrNo not started from Handover
            // return 4 for complete date is null record not found for OrNo
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess) {
                var jsonresult = response.message;
                debugger;
                for (var k = 1; k <= 30; k++) {

                    var isDataFound = false;
                    var chuban1 = $("#txtOrderNo" + k).val();
                    var chuubanNotFoundList = jsonresult.filter(r => r.chuubaN_CODE == 2);
                    var processPlanNotStartedList = jsonresult.filter(r => r.chuubaN_CODE == 3);
                    var processPlanCompletedList = jsonresult.filter(r => r.chuubaN_CODE == 4);


                    if (chuban1 != "") {

                        $.each(jsonresult, function (i, jsonObj) {

                            debugger;

                            if (jsonresult[i].chuuban == chuban1 && jsonresult[i].chuubaN_CODE == 1 /*&& jsonresult[i].procesS_PLAN_START_DATE != null*/) {
                                debugger;
                                $("#txtCustomer" + k).text(jsonresult[i].customeR_NAME);
                                $("#lblProdNameCust" + k).text(jsonresult[i].proD_NAME_CUST);
                                $("#lblCharge" + k).text(jsonresult[i].charge);
                                $("#lblMatlDim" + k).text(jsonresult[i].matL_DIM_STRING);
                                $("#lblProdDim" + k).text(jsonresult[i].proD_DIM_STRING);
                                $("#txtNoOfUnit" + k).val(jsonresult[i].pP_NO_OF_UNITS);
                                $("#ddlUnit" + k).val(jsonresult[i].pP_UNIT);
                                // Comment below code for Don’t show default  Weight client requirement 2026-07-07 
                             //   $("#txtQuantity" + k).val(jsonresult[i].pP_ACT_ALLOC_QTY);


                                $("#hdfProcessPlanCD" + k).text(jsonresult[i].procesS_PLAN_CD);

                                // for next Mandatory field
                       //         $("#ddlMatlType" + k).addClass("Mandatory2", "Mandatory2");
                                //  $("#txtLotNo" + k).addClass("Mandatory2", "Mandatory2");
                                $("#txtFurnacePosition1" + k).addClass("Mandatory2", "Mandatory2");
                                $("#txtFurnacePosition2" + k).addClass("Mandatory2", "Mandatory2");
                                //$("#txtTabaNo1" + k).addClass("Mandatory2", "Mandatory2");
                                // $("#ddlNextProcess" + k).addClass("Mandatory2", "Mandatory2");

                                $("#btnSearchByFurnace").attr("disabled", "disabled");


                                $("#btnStartDataSave").removeAttr("disabled");
                                DisableEndDetails();
                                EnableStartdetails();
                                $("#btnEndDataSave").attr("disabled", "disabled");
                                $("#ddlSrchMachine").val("");
                                $("#txtSrchFurnaceNo").val("");
                                $("#ddlFurnaceOutIncharge").val("");
                                $("#ddlFurnaceVerifyBy").val("");


                                $("#ddlMachine").focus();
                                $("#selectRow" + k).removeAttr("disabled");
                                enableDeleteRowButtons();
                            }
                            else {
                                debugger;

                                // clearTableRow("trId" + k);
                                //   $("#ddlMatlType" + k).removeClass("Mandatory2", "Mandatory2");
                                //  $("#txtLotNo" + k).removeClass("Mandatory2", "Mandatory2");
                                //   $("#txtFurnacePosition1" + k).removeClass("Mandatory2", "Mandatory2");
                                //   $("#txtFurnacePosition2" + k).removeClass("Mandatory2", "Mandatory2");
                                //   $("#txtTabaNo1" + k).removeClass("Mandatory2", "Mandatory2");
                                //$("#ddlNextProcess" + k).removeClass("Mandatory2", "Mandatory2");
                                EnableStartdetails();
                            }

                        });

                        //if (isDataFound == false) {
                        //    clearTableRow("trId" + k);
                        //    $("#ddlMatlType" + k).removeClass("Mandatory2", "Mandatory2");
                        //    $("#txtLotNo" + k).removeClass("Mandatory2", "Mandatory2");
                        //    $("#txtFurnacePosition1" + k).removeClass("Mandatory2", "Mandatory2");
                        //    $("#txtFurnacePosition2" + k).removeClass("Mandatory2", "Mandatory2");
                        //    $("#ddlNextProcess" + k).removeClass("Mandatory2", "Mandatory2");
                        //    invalidChuuban.push(chuban1);
                        //}

                    }


                }
                debugger;
                calculateTotalWeight();
                calculateTotalWeightOnGridLoad();
            }
            debugger;
            if (chuubanNotFoundList != "") {

                var msg = "";
                for (var i = 0; i < chuubanNotFoundList.length; i++) {
                    if (msg == "") {
                        msg = chuubanNotFoundList[i].chuuban;
                    } else {
                        msg += " <br>" + chuubanNotFoundList[i].chuuban;
                    }
                    if (focusID1 == "") {
                        focusID1 = chuubanNotFoundList[i].chuuban;
                    }
                }

                message = $("#lblErrorMessageE_HeatTreat_028").text().trim() + "<br>" + msg.toString();
            }


            if (processPlanCompletedList != "") {

                var msg1 = "";
                for (var i = 0; i < processPlanCompletedList.length; i++) {
                    if (msg1 == "") {
                        msg1 = processPlanCompletedList[i].chuuban;
                    } else {
                        msg1 += " <br>" + processPlanCompletedList[i].chuuban;
                    }
                    if (focusID2 == "") {
                        focusID2 = processPlanCompletedList[i].chuuban;
                    }
                }

                message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_029").text().trim() + "<br>" + msg1.toString();
            }
            if (processPlanNotStartedList != "") {

                var msg2 = "";
                for (var i = 0; i < processPlanNotStartedList.length; i++) {
                    if (msg2 == "") {
                        msg2 = processPlanNotStartedList[i].chuuban;
                    } else {
                        msg2 += " <br>" + processPlanNotStartedList[i].chuuban;
                    }
                    if (focusID3 == "") {
                        focusID3 = processPlanNotStartedList[i].chuuban;
                    }
                }

                message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_030").text().trim() + "<br>" + msg2.toString();
            }

            for (var i = 1; i <= 30; i++) {
                if ($("#txtOrderNo" + i).val() == focusID1 || $("#txtOrderNo" + i).val() == focusID2 || $("#txtOrderNo" + i).val() == focusID3) {
                    focusID = "txtOrderNo" + i;
                    break;

                }
            }

            if (message != "") {

                showMessageDialog(message, focusID, WARNING);
            }
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    if (isOrderNoExist == false) {

        message = $.trim($("#lblErrorMessageE_HeatTreat_018").text());
        showMessageDialog(message, "txtOrderNo1", WARNING);
        //  FillEmptyGrid();
    }
    //$("ddlMachine").focus();
}

function clearTable($table, trId, trClass) {

    // If class is present, use class to delete, else use ID.
    $.each($('.' + trClass).find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });

    $.each($('.' + trClass).find("select"), function (index, dropdown) {
        $("#" + dropdown.id + " option:selected").prop("selected", false);
        $("#" + dropdown.id + " option:first").prop("selected", "selected");
    });

    $.each($('.' + trClass).find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
    $.each($('.' + trClass).find("label[id^='lbl']"), function (index, label) {
        $(label).text("");
    });

    $.each($('.' + trClass).find("input:hidden"), function (index, label) {
        $(label).val("");
    });
}

function clearTableRow(rowid) {

    $.each($('#' + rowid).find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });

    $.each($('#' + rowid).find("select"), function (index, dropdown) {
        $("#" + dropdown.id + " option:selected").prop("selected", false);
        $("#" + dropdown.id + " option:first").prop("selected", "selected");
    });

    $.each($('#' + rowid).find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
    $.each($('#' + rowid).find("label[id^='lbl']"), function (index, label) {
        $(label).text("");
    });

    $.each($('#' + rowid).find("input:hidden"), function (index, label) {
        $(label).val("");
    });
}

function IsStartDateValidStart() {
    debugger;
    retval = true; // assume end data is valid
  

    var currentDate = $("#txtFurnaceInDateTime").val();
   
    //if (isOneDayAfter(currentDate)) {
    //    //var planned = $("#txtPlannedUploadDate").val();
    //    //var furnace = $("#txtFurnaceInDateTime").val();
    //    //// if Furnace In Date Time >= CarriedOut Schedule
    //    //if (Date.parse($("#txtPlannedUploadDate").val()) < Date.parse($("#txtFurnaceInDateTime").val())) {
    //    //    var message = $("#lblErrorMessageE_HeatTreat_027").text().trim() ;
    //    //    var focusFieldID = "txtPlannedUploadDate";
    //    //    showMessageDialog(message, focusFieldID, WARNING);
    //    //    return false;
    //    //}
    //    return true;
    //} else {
    //    var message = $("#lblErrorMessageE_HeatTreat_026").text().trim() + "(±" + commonDateRange + ")";
    //    var focusFieldID = "txtFurnaceInDateTime";
    //    showMessageDialog(message, focusFieldID, WARNING);
    //    return false;
    //}

  
}

function isOneDayAfter(date) {
    //when Furnace In DateTime is +/- 1day
     var SystemDate = $("#hdfSystemDate").val();
    let nextDay1 = moment(SystemDate).add(1, 'day').format('YYYY/MM/DD HH:mm');
    let lastDay1 = moment(SystemDate).subtract(1, 'day').format('YYYY/MM/DD HH:mm');
    //return date <= nextDay1 && date >= lastDay1;

    if (date >= lastDay1 && date <= nextDay1) {
        return true;
    } else {
        return false;
    }
  
}
function IsEndDateValidStart() {
    debugger;
    retval = true; // assume end data is valid


    var currentDate = $("#txtFurnaceOutDateTime").val();

    if (isOneDayBefore(currentDate)) {
        //var furnaceOut = $("#txtFurnaceOutDateTime").val();
        //var furnaceIn = $("#txtFurnaceInDateTime").val();
        //// if Furnace In Date Time >= Furnace 
        //if (Date.parse($("#txtFurnaceInDateTime").val()) > Date.parse($("#txtFurnaceOutDateTime").val())) {
        //    var message = $("#lblErrorMessageE_HeatTreat_035").text().trim() ;
        //    var focusFieldID = "txtFurnaceOutDateTime";
        //    showMessageDialog(message, focusFieldID, WARNING);
        //    return false;
        //}
        return true;
    } else {
        var message = $("#lblErrorMessageE_HeatTreat_034").text().trim() + "(±"  + commonDateRange + ")";
        var focusFieldID = "txtFurnaceOutDateTime";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }


}
function isOneDayBefore(date) {
    //when Furnace In DateTime is +/- 1day
    var SystemDate = $("#hdfSystemDate").val();
    let nextDay1 = moment(SystemDate).add(1, 'day').format('YYYY/MM/DD HH:mm');
    let lastDay1 = moment(SystemDate).subtract(1, 'day').format('YYYY/MM/DD HH:mm');
    return date <= nextDay1 && date >= lastDay1;

}




function checkDate(field) {
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    //re = /^(\d{4})\/(\d{1,2})\/(\d{1,2})$/;
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    /*if (field.val() != '') {*/
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
        // }
    }
}
function IsMachineAlreadyStarted() {

    var machineCd = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var returnVal = false;

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/IsMachineAlreadyStarted?machineCD=" + machineCd,// + "&isNull=" + true,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            returnVal = response.message

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, "", ERROR);
        });
    return returnVal;
}
function IsFurnaceNoDuplicate() {

    var furnaceNo;
    var machineCd = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var furNo = $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null;

    var returnVal = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetFurnaceNo?furNo=" + furNo,// + "&isNull=" + true,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;


            furnaceNo = response.message.length;
            if (furnaceNo > 0) {

                returnVal = true;
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, "", ERROR);
        });
    return returnVal;

}

function btnSaveStartData_Click() {
    debugger;
    var MatlType = "";
    var NextProcess = "";
    var FurnacePosition1 = "";
    var FurnacePosition2 = "";
    var LotNo = ""
    var bundleNo = "";
    var MaxHTNumber;
    var ListData = [];
    var fieldToFocus = "";
    //$("#ddlFurnaceOutIncharge").removeClass("Mandatory", "Mandatory");
    //$("#ddlFurnaceVerifyBy").removeClass("Mandatory", "Mandatory");

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    if (!checkDate($("#txtFurnaceInDateTime"))) {

        var message = $("#lblErrorMessageE_HeatTreat_021").text().trim(); // This message will be displayed on dialog box          
        showMessageDialog(message, "txtFurnaceInDateTime", WARNING);
        return false;
    }
    if (!checkDate($("#txtPlannedUploadDate"))) {

        var message = $("#lblErrorMessageE_HeatTreat_022").text().trim(); // This message will be displayed on dialog box  
        showMessageDialog(message, "txtPlannedUploadDate", WARNING);
        return false;
    }

    //if (!IsStartDateValidStart()) {
    //    return false;
    //}
    if (IsMachineAlreadyStarted()) {
        var message = $("#lblErrorMessageE_HeatTreat_031").text().replace("NNN", $("#ddlMachine option:selected").text());
        showMessageDialog(message, "ddlMachine", WARNING);
        return false;
    }

    if (IsFurnaceNoDuplicate()) {
        var message = $("#lblErrorMessageE_HeatTreat_010").text();
        showMessageDialog(message, "txtFurnaceNo", WARNING);
        return false;
    }

    for (var i = 1; i <= 30; i++) {

        var aa = $("#hdfProcessPlanCD" + i).text();
        if ($("#hdfProcessPlanCD" + i).text() != "") {
            //if ($("#ddlMatlType" + i).val() == 0) {
            //    if (MatlType == "") {
            //        MatlType = i;
            //    }
            //    else {
            //        MatlType = MatlType + ", " + i;
            //    }
            //    if (fieldToFocus == "") {
            //        fieldToFocus = "ddlMatlType" + i;
            //    }
            //}

            //if ($("#txtLotNo" + i).val() == "") {
            //    if (LotNo == "") {
            //        LotNo = i;
            //    } else {
            //        LotNo = LotNo + ", " + i;
            //    }
            //}
            if ($("#txtFurnacePosition1" + i).val() == "") {
                if (FurnacePosition1 == "") {
                    FurnacePosition1 = i;
                } else {
                    FurnacePosition1 = FurnacePosition1 + ", " + i;
                }
                if (fieldToFocus == "") {
                    fieldToFocus = "txtFurnacePosition1" + i;
                }
            }
            if ($("#txtFurnacePosition2" + i).val() == "") {
                if (FurnacePosition2 == "") {
                    FurnacePosition2 = i;
                } else {
                    FurnacePosition2 = FurnacePosition2 + ", " + i;
                }
                if (fieldToFocus == "") {
                    fieldToFocus = "txtFurnacePosition2" + i;
                }
            }
            //if ($("#txtTabaNo1" + i).val() == "") {
            //    if (bundleNo == "") {
            //        bundleNo = i;
            //    } else {
            //        bundleNo = bundleNo + ", " + i;
            //    }
            //    if (fieldToFocus == "") {
            //        fieldToFocus = "txtTabaNo1" + i;
            //    }
            //}

            //if ($("#ddlNextProcess" + i).val() == 0) {
            //    if (NextProcess == "") {
            //        NextProcess = i;
            //    } else {
            //        NextProcess = NextProcess + ", " + i;
            //    }
            //    if (fieldToFocus == "") {
            //        fieldToFocus = "ddlNextProcess" + i;
            //    }
            //}
        }

    }
    var message = "";
    if (FurnacePosition1 != "" || FurnacePosition2 != "") {

        // NextProcess != "" || 

        //if (LotNo != "") {
        //    message = $("#lblErrorMessageE_HeatTreat_011").text().trim().replace("NNN", LotNo);
        //}

        //if (MatlType != "") {
        //    if (message == "") {
        //        message = $("#lblErrorMessageE_HeatTreat_015").text().trim().replace("NNN", MatlType);
        //    } else {
        //        message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_015").text().trim().replace("NNN", MatlType);
        //    }
        //}
        if (FurnacePosition1 != "") {
            if (message == "") {
                message = $("#lblErrorMessageE_HeatTreat_012").text().trim().replace("NNN", FurnacePosition1);
            } else {
                message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_012").text().trim().replace("NNN", FurnacePosition1);
            }

        }
        if (FurnacePosition2 != "") {
            if (message == "") {
                message = $("#lblErrorMessageE_HeatTreat_013").text().trim().replace("NNN", FurnacePosition2);
            } else {
                message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_013").text().trim().replace("NNN", FurnacePosition2);
            }
        }

        //if (bundleNo != "") {
        //    if (message == "") {
        //        message = $("#lblErrorMessageE_HeatTreat_032").text().trim().replace("NNN", bundleNo);
        //    } else {
        //        message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_032").text().trim().replace("NNN", bundleNo);
        //    }
        //}


        //if (NextProcess != "") {
        //    if (message == "") {
        //        message = $("#lblErrorMessageE_HeatTreat_014").text().trim().replace("NNN", NextProcess);
        //    } else {
        //        message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_014").text().trim().replace("NNN", NextProcess);
        //    }
        //}
        showMessageDialog(message, fieldToFocus, WARNING);
    }
    else {

        var ajaxTime = new Date().getTime();

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/GetMaxHTNumber",
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;

                HTNumber = response.message;
                MaxHTNumber = HTNumber;

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                //var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HeatTreat_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });


        for (var i = 1; i <= 30; i++) {
            var uploaddate;
            if ($("#txtPlannedUploadDate").val() == "") {
                Planneduploaddate = $("#txtPlannedUploadDate").val() ? $("#txtPlannedUploadDate").val() : null;
            }
            else {
                Planneduploaddate = ($("#txtPlannedUploadDate").val().split(" ")[0] + "T" + $("#txtPlannedUploadDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

            }




            var aa = $("#hdfProcessPlanCD" + i).text();
            if ($("#hdfProcessPlanCD" + i).text() !== '') {
                var aa = $("#txtCustomer" + i).val();

                var ajaxTime = new Date().getDate();
                var jsonSaveStartData = ({
                    HEAT_TREATMENT_ROW: i,
                    HEAT_TREATMENT_NUMBER: MaxHTNumber,
                    MACHINE_CD: $("#ddlMachine").val() ? $("#ddlMachine").val() : null,
                    FURNACE_NO: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null,
                    hT_CONDITION: $("#txtProcCondition").val() ? $("#txtProcCondition").val() : null,
                    PATTERN_NO: $("#txtPatternNo").val() ? $("#txtPatternNo").val() : null,
                    HT_START_DATE: ($("#txtFurnaceInDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceInDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    SETUP_CONF_BY: $("#hdftxtscAssembleVerifyByInputSearch").val() ? $("#hdftxtscAssembleVerifyByInputSearch").val() : null,
                    LOADING_CONF_BY: $("#hdftxtscFurncInVerifyByInputSearch").val() ? $("#hdftxtscFurncInVerifyByInputSearch").val() : null,
                    PLANNED_UNLOAD_DATE: Planneduploaddate,
                    HT_HEADER_REMARK: $("#txtHtHeaderRemark").val() ? $("#txtHtHeaderRemark").val() : null,
                    PROCESS_PLAN_CD: $("#hdfProcessPlanCD" + i).text() ? $("#hdfProcessPlanCD" + i).text() : null,

                    COIL_BAR_TYPE: $("#ddlMatlType" + i).val() ? $("#ddlMatlType" + i).val() : null,
                    LOT_NO: $("#txtLotNo" + i).val() ? $("#txtLotNo" + i).val() : null,
                    HT_NO_OF_UNITS: $("#txtNoOfUnit" + i).val() ? $("#txtNoOfUnit" + i).val() : null,
                    UNIT_TYPE: $("#ddlUnit" + i).val() ? $("#ddlUnit" + i).val() : null,
                    HT_WEIGHT: $("#txtQuantity" + i).val() ? $("#txtQuantity" + i).val() : null,
                    HT_REMARKS: $("#txtRemark" + i).val() ? $("#txtRemark" + i).val() : null,
                    MSG_CD: $("#hdfMsgCD" + i).text() ? $("#hdfMsgCD" + i).text() : null,
                    ROLL_MONTH: $("#txtRoll" + i).val() ? $("#txtRoll" + i).val() : null,
                    COIL_NO: $("#txtCoilNo" + i).val() ? $("#txtCoilNo" + i).val() : null,
                    LOCATION_IN_FURNACE1: $("#txtFurnacePosition1" + i).val() ? $("#txtFurnacePosition1" + i).val() : null,
                    LOCATION_IN_FURNACE2: $("#txtFurnacePosition2" + i).val() ? $("#txtFurnacePosition2" + i).val() : null,
                    HT_TABA_NO_1: $("#txtTabaNo1" + i).val() ? $("#txtTabaNo1" + i).val() : null,
                    HT_TABA_NO_2: $("#txtTabaNo2" + i).val() ? $("#txtTabaNo2" + i).val() : null,
                    HT_TABA_NO_3: $("#txtTabaNo3" + i).val() ? $("#txtTabaNo3" + i).val() : null,
                    HT_TABA_NO_4: $("#txtTabaNo4" + i).val() ? $("#txtTabaNo4" + i).val() : null,
                    HT_NEXT_PROCESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
                    USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,

                });
                ListData.push(jsonSaveStartData);
            }
        }

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/SaveStartData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(ListData)
        }
        $.ajax(settings)
            .done(function (response) {

                responseTime = new Date().getTime() - ajaxTime;

                var jsonResult = response.message;
                var index = 0;
                $.each(jsonResult, function (key, jsonObj) {
                    index = jsonObj.heaT_TREATMENT_ROW

                    key = parseInt(index) + 1;

                    $("#hdfProcessPlanCD" + index).text(jsonObj.procesS_PLAN_CD);
                    $("#hdfheattreatmentCD" + index).text(jsonObj.heaT_TREATMENT_CD);
                    disableDeleteRowButtons();
                    disableGridCheckBox();

                });

                var successMessage = $("#lblSuccessMessageS_HeatTreat_Save_ENTRY_001").text();
                showMessageDialog(successMessage, "txtFurnaceOutDateTime", SUCCESS);
               
                EnableEndDetails();
               // $("#txtFurnaceOutDateTime").focus();
                DisableStartSaveButton();
               // DisableStartdetails();
                //$("#btnSearchByFurnace").removeAttr("disabled");
                $("#btnEndDataSave").removeAttr("disabled");


            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });





    }
}
$('#txtscAssembleVerifyByInputSearch')
    .on('focus', function () {
        operatorAssembleVerifyByModified = false;
    })
    .on('input', function () {
        operatorAssembleVerifyByModified = true;
    });

$('#txtscAssembleVerifyByInputSearch').on('keydown focusout', function (e) {
    debugger;
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorAssembleVerifyByModified == true) {
        
        $("#hdftxtscAssembleVerifyByInputSearch").val(operatorNumber);
        enterPressedAssembleVerifyByOpr = true;
        GetOperatorNameByCD($(this));
        $("#txtscFurncInVerifyByInputSearch").focus();
    }
    if (e.type === 'focusout' && operator && operatorAssembleVerifyByModified == true) {
        if (enterPressedAssembleVerifyByOpr) {

            enterPressedAssembleVerifyByOpr = false; // Reset the flag
            return false;
        }
        $("#hdftxtscAssembleVerifyByInputSearch").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtscFurncInVerifyByInputSearch").focus();
    }
    enterPressedAssembleVerifyByOpr = false; // Reset the flag
});




$('#txtscFurncInVerifyByInputSearch')
    .on('focus', function () {
        operatorFurncInVerifyByModified = false;
    })
    .on('input', function () {
        operatorFurncInVerifyByModified = true;
    });

$('#txtscFurncInVerifyByInputSearch').on('keydown focusout', function (e) {
    debugger;
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorFurncInVerifyByModified == true) {

        $("#hdftxtscFurncInVerifyByInputSearch").val(operatorNumber);
        enterPressedFurncInVerifyByOpr = true;
        GetOperatorNameByCD($(this));
        $("#txtPlannedUploadDate").focus();
    }
    if (e.type === 'focusout' && operator && operatorFurncInVerifyByModified == true) {
        if (enterPressedFurncInVerifyByOpr) {

            enterPressedFurncInVerifyByOpr = false; // Reset the flag
            return false;
        }
        $("#hdftxtscFurncInVerifyByInputSearch").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtPlannedUploadDate").focus();
    }
    enterPressedFurncInVerifyByOpr = false; // Reset the flag
});
$('#txtFurnaceOutInchargeInputSearch')
    .on('focus', function () {
        operatorFurnaceOutInchargeModified = false;
    })
    .on('input', function () {
        operatorFurnaceOutInchargeModified = true;
    });

$('#txtFurnaceOutInchargeInputSearch').on('keydown focusout', function (e) {
    debugger;
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorFurnaceOutInchargeModified == true) {

        $("#hdfFurnaceOutInchargeInputSearch").val(operatorNumber);
        enterPressedFurnaceOutInchargeOpr = true;
        GetOperatorNameByCD($(this));
        $("#txtFurnaceVerifyByInputSearch").focus();
    }
    if (e.type === 'focusout' && operator && operatorFurnaceOutInchargeModified == true) {
        if (enterPressedFurnaceOutInchargeOpr) {

            enterPressedFurnaceOutInchargeOpr = false; // Reset the flag
            return false;
        }
        $("#hdfFurnaceOutInchargeInputSearch").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtFurnaceVerifyByInputSearch").focus();
    }
    enterPressedFurnaceOutInchargeOpr = false; // Reset the flag
});
$('#txtFurnaceVerifyByInputSearch')
    .on('focus', function () {
        operatorFurnaceVerifyByModified = false;
    })
    .on('input', function () {
        operatorFurnaceVerifyByModified = true;
    });

$('#txtFurnaceVerifyByInputSearch').on('keydown focusout', function (e) {
    debugger;
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);

    if (e.which === 13 && operator && operatorFurnaceVerifyByModified == true) {

        $("#hdfFurnaceVerifyByInputSearch").val(operatorNumber);
        enterPressedFurnaceVerifyByOpr = true;
        GetOperatorNameByCD($(this));
        $("#txtProcessingTime").focus();
    }
    if (e.type === 'focusout' && operator && operatorFurnaceVerifyByModified == true) {
        if (enterPressedFurnaceVerifyByOpr) {

            enterPressedFurnaceVerifyByOpr = false; // Reset the flag
            return false;
        }
        $("#hdfFurnaceVerifyByInputSearch").val(operatorNumber);
        GetOperatorNameByCD($(this));
        $("#txtProcessingTime").focus();
    }
    enterPressedFurnaceVerifyByOpr = false; // Reset the flag
});
function GetOperatorNameByCD($target) {
    debugger;
    var ajaxTime = new Date().getTime();
    var message = "";
    var operatorNumber = $target.val().trim();
    var operatorVal = operatorNumber.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorCD = operatorNumber.substring(3, 9);
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
      if (operatorAssembleVerifyByModified == true) {
            message = $("#lblErrorMessageValMsg_BarcodeIncorrect").text().trim().replace("*****", $("#lblAssembleVerifyBy").text());
        }
      else if (operatorFurncInVerifyByModified == true) {
            message = $("#lblErrorMessageValMsg_BarcodeIncorrect").text().trim().replace("*****", $("#lblFurncInVerify").text());
        }
      else if (operatorFurnaceOutInchargeModified == true) {
          message = $("#lblErrorMessageValMsg_BarcodeIncorrect").text().trim().replace("*****", $("#lblFurnaceOutIncharge").text());
        }
      else if (operatorFurnaceVerifyByModified == true) {
          message = $("#lblErrorMessageValMsg_BarcodeIncorrect").text().trim().replace("*****", $("#lblFurnaceVerifyBy").text());
      }
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {

        var message = $("#lblErrorMessageValMsg_HardnessOperatorBarcode").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorCD == "") {
        var message = $("#lblErrorMessageValMsg_HardnessOperatorBarcode").text().trim();
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
            url: APIURL + "/HeatTreatment/GetOperatorNamebyCD?OperatorCD=" + operatorCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",


        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                debugger;

                if (jsonResult == null || jsonResult == "") {
                    if (operatorAssembleVerifyByModified == true) {
                        message = $("#lblErrorMessageValMsg_BarcodeNotFound").text().trim().replace("*****", $("#lblAssembleVerifyBy").text());
                    }
                    else if (operatorFurncInVerifyByModified == true) {
                        message = $("#lblErrorMessageValMsg_BarcodeNotFound").text().trim().replace("*****", $("#lblFurncInVerify").text());
                    }
                    else if (operatorFurnaceOutInchargeModified == true) {
                        message = $("#lblErrorMessageValMsg_BarcodeNotFound").text().trim().replace("*****", $("#lblFurnaceOutIncharge").text());
                    }
                    else if (operatorFurnaceVerifyByModified == true) {
                        message = $("#lblErrorMessageValMsg_BarcodeNotFound").text().trim().replace("*****", $("#lblFurnaceVerifyBy").text());
                    }
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }//employee is inactive.
                else if (jsonResult.iN_ACTIVE_FLAG == 1) {

                    var message = $("#lblErrorMessageValMsg_COM_HardnessEmployeeInactive").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val("");
                }

                else {
                    $target.val(jsonResult.yobina);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_Tensile_ENTRY_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            })
    }
}

function btnSaveEndData_Click() {


    var MatlType = "";
    var NextProcess = "";
    var FurnacePosition1 = "";
    var FurnacePosition2 = "";
    var LotNo = ""
    var bundleNo = "";
    var ListData = [];
    var fieldToFocus = "";

    if (!IsMandatoryFieldsFilledEnd()) {
        return false;
    }
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    if (!checkDate($("#txtFurnaceOutDateTime"))) {
        var message = $("#lblErrorMessageE_HeatTreat_023").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "", WARNING);
        return false;
    }
    if (!IsEndDateValidStart()) {
        return false;
    }

    for (var i = 1; i <= 30; i++) {

        $("#hdfProcessPlanCD" + i).val("");
        var aa = $("#hdfProcessPlanCD" + i).text();
        if ($("#hdfProcessPlanCD" + i).text() != "") {
            //if ($("#ddlMatlType" + i).val() == 0) {
            //    if (MatlType == "") {
            //        MatlType = i;
            //    }
            //    else {
            //        MatlType = MatlType + ", " + i;
            //    }
            //}
           
            if ($("#ddlNextProcess" + i).val() == 0) {
               
                if (NextProcess == "") {
                    NextProcess = i;
                } else {
                    NextProcess = NextProcess + ", " + i;
                }
                if (fieldToFocus == "") {
                    fieldToFocus = "ddlNextProcess" + i;
                }
            }
           
            //if ($("#txtLotNo" + i).val() == "") {
            //    if (LotNo == "") {
            //        LotNo = i;
            //    } else {
            //        LotNo = LotNo + ", " + i;
            //    }
            //}
            if ($("#txtFurnacePosition1" + i).val() == "") {
                if (FurnacePosition1 == "") {
                    FurnacePosition1 = i;
                } else {
                    FurnacePosition1 = FurnacePosition1 + ", " + i;
                }
                if (fieldToFocus == "") {
                    fieldToFocus = "txtFurnacePosition1" + i;
                }
            }
            if ($("#txtFurnacePosition2" + i).val() == "") {
                if (FurnacePosition2 == "") {
                    FurnacePosition2 = i;
                } else {
                    FurnacePosition2 = FurnacePosition2 + ", " + i;
                }
                if (fieldToFocus == "") {
                    fieldToFocus = "txtFurnacePosition2" + i;
                }
            }
            //if ($("#txtTabaNo1" + i).val() == "") {
            //    if (bundleNo == "") {
            //        bundleNo = i;
            //    } else {
            //        bundleNo = bundleNo + ", " + i;
            //    }
            //    if (fieldToFocus == "") {
            //        fieldToFocus = "txtTabaNo1" + i;
            //    }
            //}
          //  $("#ddlNextProcess option[value='0']").remove();
        }
    }
    var message = "";
    debugger;
    if (NextProcess != "" || FurnacePosition1 != "" || FurnacePosition2 != "") {


        //if (MatlType != "") {
        //    message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_015").text().trim().replace("NNN", MatlType);
        //}
        if (FurnacePosition1 != "") {
            message = $("#lblErrorMessageE_HeatTreat_012").text().trim().replace("NNN", FurnacePosition1);
        }
        if (FurnacePosition2 != "") {
            message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_013").text().trim().replace("NNN", FurnacePosition2);
        }
        //if (bundleNo != "") {
        //    if (message == "") {
        //        message = $("#lblErrorMessageE_HeatTreat_032").text().trim().replace("NNN", bundleNo);
        //    } else {
        //        message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_032").text().trim().replace("NNN", bundleNo);
        //    }
        //}
        if (NextProcess != "") {
            message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_014").text().trim().replace("NNN", NextProcess);
        }


        //if (LotNo != "") {
        //    message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_011").text().trim().replace("NNN", LotNo);
        //}

        showMessageDialog(message, fieldToFocus, WARNING);
    }
    else {
        var ajaxTime = new Date().getTime();
        for (var i = 1; i <= 30; i++) {
            var aa = $("#hdfProcessPlanCD" + i).text();
            if ($("#hdfProcessPlanCD" + i).text() !== '') {

                var ajaxTime = new Date().getTime();
                var jsonSaveStartData = ({

                    MACHINE_CD: $("#ddlMachine").val() ? $("#ddlMachine").val() : null,
                    FURNACE_NO: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null,
                    hT_CONDITION: $("#txtProcCondition").val() ? $("#txtProcCondition").val() : null,
                    PATTERN_NO: $("#txtPatternNo").val() ? $("#txtPatternNo").val() : null,
                    HT_START_DATE: ($("#txtFurnaceInDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceInDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    SETUP_CONF_BY: $("#hdftxtscAssembleVerifyByInputSearch").val() ? $("#hdftxtscAssembleVerifyByInputSearch").val() : null,
                    LOADING_CONF_BY: $("#hdftxtscFurncInVerifyByInputSearch").val() ? $("#hdftxtscFurncInVerifyByInputSearch").val() : null,
                    PLANNED_UNLOAD_DATE: ($("#txtPlannedUploadDate").val().split(" ")[0] + "T" + $("#txtPlannedUploadDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    HT_HEADER_REMARK: $("#txtHtHeaderRemark").val() ? $("#txtHtHeaderRemark").val() : null,
                    HT_END_DATE: ($("#txtFurnaceOutDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceOutDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    OFFLOADED_BY: $("#hdfFurnaceOutInchargeInputSearch").val() ? $("#hdfFurnaceOutInchargeInputSearch").val() : null,
                    OFFLOAD_CONF_BY: $("#hdfFurnaceVerifyByInputSearch").val() ? $("#hdfFurnaceVerifyByInputSearch").val() : null,
                    USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,

                    // Grid Values
                    PROCESS_PLAN_CD: $("#hdfProcessPlanCD" + i).text(),
                    HEAT_TREATMENT_CD: $("#hdfheattreatmentCD" + i).text(),

                    COIL_BAR_TYPE: $("#ddlMatlType" + i).val() ? $("#ddlMatlType" + i).val() : null,
                    LOT_NO: $("#txtLotNo" + i).val() ? $("#txtLotNo" + i).val() : null,
                    HT_NO_OF_UNITS: $("#txtNoOfUnit" + i).val() ? $("#txtNoOfUnit" + i).val() : null,
                    UNIT_TYPE: $("#ddlUnit" + i).val() ? $("#ddlUnit" + i).val() : null,
                    HT_WEIGHT: $("#txtQuantity" + i).val() ? $("#txtQuantity" + i).val() : null,
                    HT_REMARKS: $("#txtRemark" + i).val() ? $("#txtRemark" + i).val() : null,
                    MSG_CD: $("#hdfMsgCD" + i).text() ? $("#hdfMsgCD" + i).text() : null,
                    ROLL_MONTH: $("#txtRoll" + i).val() ? $("#txtRoll" + i).val() : null,
                    COIL_NO: $("#txtCoilNo" + i).val() ? $("#txtCoilNo" + i).val() : null,
                    LOCATION_IN_FURNACE1: $("#txtFurnacePosition1" + i).val() ? $("#txtFurnacePosition1" + i).val() : null,
                    LOCATION_IN_FURNACE2: $("#txtFurnacePosition2" + i).val() ? $("#txtFurnacePosition2" + i).val() : null,
                    HT_TABA_NO_1: $("#txtTabaNo1" + i).val() ? $("#txtTabaNo1" + i).val() : null,
                    HT_TABA_NO_2: $("#txtTabaNo2" + i).val() ? $("#txtTabaNo2" + i).val() : null,
                    HT_TABA_NO_3: $("#txtTabaNo3" + i).val() ? $("#txtTabaNo3" + i).val() : null,
                    HT_TABA_NO_4: $("#txtTabaNo4" + i).val() ? $("#txtTabaNo4" + i).val() : null,
                    HT_NEXT_PROCESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,

                });
                ListData.push(jsonSaveStartData)
            }
        }
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/UpdateEndData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //data: JSON.stringify(data)
            data: JSON.stringify(ListData)
        }
        $.ajax(settings)
            .done(function (response) {

                responseTime = new Date().getTime() - ajaxTime;


                var successMessage = $("#lblSuccessMessageS_HeatTreat_Save_ENTRY_001").text();
                showMessageDialog(successMessage, "saved", SUCCESS);
                DisableEndSaveButton();
                DisableEndDetails();
                $("#btnSearchByChubaanPopup").attr("disabled", "disabled");
                $("#btnSearchByFurnace").attr("disabled", "disabled");
                $("#txtSrchFurnaceNo").val("");
                $("#ddlSrchMachine").prop("selectedIndex", 0);
            })
            .fail(function (xhr, textStatus, errorThrown) {

                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

}


function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find("#pnlHeader .Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        showMessageDialog(message, firstFieldID, WARNING);
    }
    return returnVal;
}

function BindEvent() {


    $("#btnStartDataSave").unbind("click");

    setFurnaceSysDateTime();
    setPlannedUploadSysDateTime();

    // Start Details
    $("#btnFurnaceInDateTimeNow").click(function (event) {
        setFurnaceSysDateTime();
    });
    $("#btnPlannedUploadDateNow").click(function (event) {
        debugger;
        $("#ddlMatlType1").focus();
        setPlannedUploadSysDateTime();
        $("#ddlMatlType1").focus();
    });
    //$("#btnscAssembleVerifyByOpeartorSearch").click(function (event) {
    //    var yobina = $.trim($("#txtscAssembleVerifyByInputSearch").val());
    //    setStartOperatorListStartDetails(yobina, "");
    //});
    //$("#btnscFurncInVerifyByOpeartorSearch").click(function (event) {
    //    var yobina = $.trim($("#txtscFurncInVerifyByInputSearch").val());
    //    setEndOperatorListStartDetails(yobina, "");SSSS
    //});

    // End Details
    $("#btnFurnaceOutDateTimeNow").click(function (event) {
        setFurnaceOutDateTime();
        $("#txtFurnaceOutDateTime").focus();
    });


    setMachineList();
    setNextProcess();
    setProductUnitKubun();
    setProcConditionKubun();
    setMatlType();


    //$("#btnFurnaceOutInchargeSearch").click(function (event) {
    //    var yobina = $.trim($("#txtFurnaceOutInchargeInputSearch").val());
    //    setStartOperatorListEndDetails(yobina, "");
    //});
    //$("#btnFurnaceVerifyBySearch").click(function (event) {
    //    var yobina = $.trim($("#txtFurnaceVerifyByInputSearch").val());
    //    setEndOperatorListEndDetails(yobina, "");
    //});
}

function setFurnaceSysDateTime() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var sysDateTime = response.message;
            // $("#txtFurnaceInDateTime").val($.format.date(new Date(sysDateTime.starT_DATETIME_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            $("#txtFurnaceInDateTime").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            $("#hdfSystemDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function setPlannedUploadSysDateTime() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var sysDateTime = response.message;
            $("#txtPlannedUploadDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            $("#ddlMatlType1").focus();
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function setFurnaceOutDateTime() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var sysDateTime = response.message;
            $("#txtFurnaceOutDateTime").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            CalculateRequiredHours();
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
//document.addEventListener('change',"txtFurnaceOutDateTime", function (e) {


//});
$("#txtFurnaceOutDateTime").change(function (event) {

    var furnaceOutDateTime = $('#txtFurnaceOutDateTime').val();
    if (furnaceOutDateTime != "") {
        CalculateRequiredHours();
    } else {
        $('#txtProcessingTime').val(0);
    }
});

$("#ddlNextProcess1").change(function (event) {



    $("#btnStartDataSave").focus();
});





function CalculateRequiredHours() {

    plannedUploadDate = new Date($('#txtFurnaceInDateTime').val());
    furnaceOutDateTime = new Date($('#txtFurnaceOutDateTime').val());
    let daysDiff = (new Date(furnaceOutDateTime).valueOf()) - (new Date(plannedUploadDate).valueOf());
    let hours = Math.floor(daysDiff / 1000 / 60 / 60);
    let minutes = Math.floor(daysDiff / 1000 / 60 - hours * 60);
    if (minutes.toString().length == 1) {
        minutes = "0" + minutes;
    }
    $("#txtProcessingTime").val(hours + ":" + minutes);
}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {

        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {

            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                debugger;
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                debugger;
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }

    });

    if (message !== "") {

        //  $("#dialogMessageIcon img").hide();
        // $("#imgError").show();
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

function IsMandatoryFieldsFilledEnd() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory1"), function (index, mandatoryField) {

        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {

            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }

    });
   

    if (message !== "") {

        //  $("#dialogMessageIcon img").hide();
        // $("#imgError").show();
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

function IsFurProcFurNoIsExist() {
    var success = false;
    var machineCD = $("#ddlMachine").val();
    var furnaceNo = $("#txtFurnaceNo").val();

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/IsFurProcFurNoIsExist?machineCD=" + machineCD + "&furnaceNo=" + furnaceNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                showMessageDialog(response.message, '', WARNING);
            }
            success = response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    return success;
}

function setNextProcess() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetNextProcess",
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
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
    for (var i = 1; i <= 30; i++) {
        $('table#tblHeatTreatMent tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $ddlNextProcess = $tr.find("#ddlNextProcess" + i);
            $ddlNextProcess.html("").append($('<option value="0" selected="selected"></option>'));
            if ($("#hdfCultureName").val() == "en") {
                $.each(jsonResult, function (key, jsonObj) {
                    $ddlNextProcess.append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
                });
            } else {
                $.each(jsonResult, function (key, jsonObj) {
                    $ddlNextProcess.append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
                });
            }

        });
    }
}

function setProductUnitKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/GetProductUnitKubun",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            bindproductunitkubundropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindproductunitkubundropdown(jsonResult) {
    /*var count = $('#tblWIPTensou tbody tr').length;*/

    for (var i = 1; i <= 30; i++) {
        $('table#tblHeatTreatMent tbody tr').each(function (index, item) {
            var $tr = $(this);

            var $ddlUnit = $tr.find("#ddlUnit" + i);
            $ddlUnit.html("").append($('<option value="0"></option>'));
            $.each(jsonResult, function (key, jsonObj) {
                $ddlUnit.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
            });

        });
    }

}

function setMatlType() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/GetMatlType",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            bindMatlType(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMatlType(jsonResult) {
    /*var count = $('#tblWIPTensou tbody tr').length;*/
    for (var i = 1; i <= 30; i++) {

        $('table#tblHeatTreatMent tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $ddlMatlType = $tr.find("#ddlMatlType" + i);
            $ddlMatlType.html("").append($('<option value="0"></option>'));
            $.each(jsonResult, function (key, jsonObj) {

                $ddlMatlType.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
            });
        });
    }
}

$("#btnInitialize").click(function (event) {

    initialize();
    $("#txtOrderNo1").focus();
});

function initialize() {
    debugger;
    $("#lblTotal").text(0);
   
    $("#txtProcCondition").css({ display: 'revert' });
    $("#txtSrchFurnaceNo").val("");
    $("#ddlSrchMachine").prop("selectedIndex", 0);

    //start details colunm
    $("#ddlMachine").val("");
    $("#txtFurnaceNo").val("");
    $("#txtPatternNo").val("");
    $("#txtscAssembleVerifyByInputSearch").val("");
    $("#ddlscAssembleVerifyBy").val("");
    $("#txtscFurncInVerifyByInputSearch").val("");
    $("#ddlscFurncInVerifyBy").val("");

    //End Details Colunm       
    $("#txtFurnaceOutDateTime").val("");
    $("#txtFurnaceOutInchargeInputSearch").val("");
    $("#txtFurnaceVerifyByInputSearch").val("");
    $("#ddlFurnaceOutIncharge").val("");
    $("#ddlFurnaceVerifyBy").val("");
    $("#txtProcessingTime").val("");
    $("#txtHtHeaderRemark").val("");
    setFurnaceSysDateTime();
    setPlannedUploadSysDateTime();


    clearTable($("#tblHeatTreatMent"), "", "DetailData");

    for (var i = 1; i <= 30; i++) {

        $("#hdfProcessPlanCD" + i).text("");
        $("#hdfheattreatmentCD" + i).text("");
        $("#hdfProcessPlanCD" + i).val("");
      //  $("#ddlMatlType" + i).removeClass("Mandatory2", "Mandatory2");
        //  $("#txtLotNo" + i).removeClass("Mandatory2", "Mandatory2");
        $("#txtFurnacePosition1" + i).removeClass("Mandatory2", "Mandatory2");
        $("#txtFurnacePosition2" + i).removeClass("Mandatory2", "Mandatory2");
        //$("#txtTabaNo1" + i).removeClass("Mandatory2", "Mandatory2");
        $("#ddlNextProcess" + i).removeClass("Mandatory2", "Mandatory2");

        $("#selectRow" + i).prop("checked", false);

    }

    //EnableStartdetails();
    DisableStartdetails();
    DisableStartSaveButton();
    DisableEndDetails();
    DisableEndSaveButton();
    $("#btnSearchByFurnace").removeAttr("disabled");
    $("#btnSearchByChubaanPopup").removeAttr("disabled");
    disableGridCheckBox();
    disableDeleteRowButtons();
    $("#txtProcCondition").val("");
    $("#ddlProcCondition").html("");
   setProcConditionKubun();
}

function clearData() {

    $("#txtSrchFurnaceNo").val("");

    //start details colunm
    $("#ddlMachine").val("");
    $("#txtFurnaceNo").val("");
    $("#ddlProcCondition").html("");
    $("#txtPatternNo").val("");
    $("#txtscAssembleVerifyByInputSearch").val("");
    $("#ddlscAssembleVerifyBy").val("");
    $("#txtscFurncInVerifyByInputSearch").val("");
    $("#ddlscFurncInVerifyBy").val("");

    //End Details Colunm   
    $("#txtFurnaceOutInchargeInputSearch").val("");
    $("#txtFurnaceVerifyByInputSearch").val("");
    $("#ddlFurnaceOutIncharge").val("");
    $("#ddlFurnaceVerifyBy").val("");

    setFurnaceSysDateTime();
    setPlannedUploadSysDateTime();

    //FillEmptyGrid();
    //FillDropDown();
    EnableStartdetails();
    DisableStartSaveButton();
    DisableEndDetails();
    DisableEndSaveButton();
    $("#btnSearchByFurnace").removeAttr("disabled");
    $("#btnSearchByChubaanPopup").removeAttr("disabled");
}

function DisableStartdetails() {
    $("#ddlMachine").attr("disabled", "disabled");
    $("#txtFurnaceNo").attr("disabled", "disabled");
    $("#txtProcCondition").css({ display: 'none' });
    $("#ddlProcCondition").html("");
    $("#ddlProcCondition").attr("disabled", "disabled");
    $("#ddlProcCondition").append($('<option>' + $("#txtProcCondition").val() + '</option>'));
    $("#txtProcCondition").val("");
    $("#txtPatternNo").attr("disabled", "disabled");
    $("#txtFurnaceInDateTime").attr("disabled", "disabled");
    $("#txtscAssembleVerifyByInputSearch").attr("disabled", "disabled");
    $("#ddlscAssembleVerifyBy").attr("disabled", "disabled");
    $("#txtscFurncInVerifyByInputSearch").attr("disabled", "disabled");
    $("#ddlscFurncInVerifyBy").attr("disabled", "disabled");
    $("#txtPlannedUploadDate").attr("disabled", "disabled");
    $("#btnFurnaceInDateTimeNow").attr("disabled", "disabled");
    $("#btnPlannedUploadDateNow").attr("disabled", "disabled");
    $("#btnscAssembleVerifyByOpeartorSearch").attr("disabled", "disabled");
    $("#btnscFurncInVerifyByOpeartorSearch").attr("disabled", "disabled");
    

    //$("#txtProcCondition").removeClass("Mandatory", "Mandatory");
    //$("#txtFurnaceNo").removeClass("Mandatory", "Mandatory");
    //$("#txtFurnaceInDateTime").removeClass("Mandatory", "Mandatory");
    
}

function EnableStartdetails() {
    $("#ddlMachine").removeAttr("disabled");
    $("#txtFurnaceNo").removeAttr("disabled");
    $("#txtProcCondition").css({ display: 'revert' });
    $("#ddlProcCondition").removeAttr("disabled");
    $("#txtPatternNo").removeAttr("disabled");
    $("#txtFurnaceInDateTime").removeAttr("disabled");
    $("#txtscAssembleVerifyByInputSearch").removeAttr("disabled");
    $("#btnscAssembleVerifyByOpeartorSearch").removeAttr("disabled");
    $("#ddlscAssembleVerifyBy").removeAttr("disabled");
    $("#txtscFurncInVerifyByInputSearch").removeAttr("disabled");
    $("#btnscFurncInVerifyByOpeartorSearch").removeAttr("disabled");
    $("#ddlscFurncInVerifyBy").removeAttr("disabled");
    $("#txtPlannedUploadDate").removeAttr("disabled");
    $("#btnFurnaceInDateTimeNow").removeAttr("disabled");
    $("#btnPlannedUploadDateNow").removeAttr("disabled");
}

function DisableEndDetails() {

    $("#txtFurnaceOutDateTime").attr("disabled", "disabled");
    $("#txtFurnaceOutInchargeInputSearch").attr("disabled", "disabled");
    $("#txtFurnaceVerifyByInputSearch").attr("disabled", "disabled");
    $("#ddlFurnaceOutIncharge").attr("disabled", "disabled");
    $("#ddlFurnaceVerifyBy").attr("disabled", "disabled");
    $("#txtProcessingTime").attr("disabled", "disabled");
    $("#btnFurnaceOutDateTimeNow").attr("disabled", "disabled");
    $("#btnFurnaceOutInchargeSearch").attr("disabled", "disabled");
    $("#btnFurnaceVerifyBySearch").attr("disabled", "disabled");


}

function EnableEndDetails() {
    debugger;
    $("#txtFurnaceOutDateTime").removeAttr("disabled");
    $("#txtFurnaceOutInchargeInputSearch").removeAttr("disabled");
    $("#txtFurnaceVerifyByInputSearch").removeAttr("disabled");
    $("#txtFurnaceOutInchargeInputSearch").removeAttr("disabled");
    $("#txtFurnaceVerifyByInputSearch").removeAttr("disabled");
    $("#txtProcessingTime").removeAttr("disabled");
    $("#btnFurnaceOutDateTimeNow").removeAttr("disabled");
    $("#btnFurnaceOutInchargeSearch").removeAttr("disabled");
    $("#btnFurnaceVerifyBySearch").removeAttr("disabled");

    $("#txtFurnaceOutInchargeInputSearch").addClass("Mandatory1", "Mandatory1");
    $("#txtFurnaceVerifyByInputSearch").addClass("Mandatory1", "Mandatory1");


    for (var k = 1; k <= 30; k++) {
        var chuban1 = $("#txtOrderNo" + k).val();
        if (chuban1 != '') {
            $("#ddlNextProcess" + k).addClass("Mandatory2", "Mandatory2");
        }
    }
}

function EnableSaveButton() {

    //$("#btnStartDataSave").removeAttr("disabled").removeClass("disabled")
    //$("#btnStartDataSave").removeAttr(disabled).removeClass(disabled)
    $("#btnStartDataSave").removeAttr("disabled");
}

function DisableStartSaveButton() {
    $("#btnStartDataSave").attr("disabled", "disabled");
}

function DisableEndSaveButton() {
    $("#btnEndDataSave").attr("disabled", "disabled");
}

function setDecimalFormats() {

    for (var i = 1; i < 30; i++) {
        $("#txtNoOfUnit" + i).keyup(function (event) {
            RestrictToValidDecimalFormat($(this), 5, 2)
        });

        $("#txtQuantity" + i).keyup(function (event) {
            RestrictToValidDecimalFormat($(this), 7, 2)
        });
    }
}


function calculateTotalWeight() {
    debugger;
    for (var k = 1; k < 30; k++) {

     $("#txtQuantity" + k).keyup(function () {

    var quantity = 0;
    for (var a = 1; a < 30; a++) {
        if ($("#hdfProcessPlanCD" + a).text() !== "" && $("#txtQuantity" + a).val() != "") {
            quantity = parseInt(quantity) + parseInt($("#txtQuantity" + a).val());
        }
    }
    $("#lblTotal").text(quantity);
    });
    }
}

function calculateTotalWeightOnGridLoad() {

    //$("#txtQuantity" + k).keyup(function () {
   
        var quantity = 0;
        for (var a = 1; a < 30; a++) {
            if ($("#hdfProcessPlanCD" + a).text() !== "" && $("#txtQuantity" + a).val() != "") {
                quantity = parseInt(quantity) + parseInt($("#txtQuantity" + a).val());
            }
        }
        $("#lblTotal").text(quantity);
    //});
}
$(document).ready(function () {
    //window.resizeTo(500, 500);
    // window.resizeTo(1000, 1530);

    //$("#btnStartDataSave").attr("disabled", "disabled");
    $("#ddlscAssembleVerifyBy").css("width", "215px");
    $("#ddlscFurncInVerifyBy").css("width", "215px");
    $("#ddlFurnaceOutIncharge").css("width", "215px");
    $("#ddlFurnaceVerifyBy").css("width", "215px");
    DisableStartdetails();
    DisableStartSaveButton();
    DisableEndSaveButton();
    DisableEndDetails();
    BindEvent();
   
    disableDeleteRowButtons();
    document.getElementById("txtOrderNo1").tabIndex = 1;
    calculateTotalWeight();
    //EnableDisableData(false);
    setDecimalFormats();
    window.resizeTo(1800, 1040);
    document.querySelectorAll('th').forEach(th => {
        th.id = th.id.replace(/\s*/g, '');
    });
    $("#txtOrderNo1").focus();
    document.getElementById("txtOrderNo1").tabIndex = 1;
    disableGridCheckBox();
   // IsDateValid();
    
});
$("#btnPlannedUploadDateNow").focusout(function (event) {
    $("#btnSearchByChubaanPopup").focus();
});

$("#txtHtHeaderRemark").focusout(function (event) {
    $("#ddlMatlType1").focus();
});

$("#txtFurnaceVerifyByInputSearch").focusout(function (event) {
    $("#txtHtHeaderRemark").focus();
});

$(document).keypress(function (e) {
    if ($(e.target).is('[data-orderno]')) {
        var orderNo = e.target.value ? e.target.value : null;
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == 13 && orderNo) {
            var ajaxTime = new Date().getTime();
            var settings = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/HeatTreatment/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
                method: "POST",
                contentType: "application/json; charset=utf-8",

            }
            $.ajax(settings)
                .done(function (response) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var jsonResult = response.message;
                    if (response.isSuccess) {
                        e.target.value = jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN;
                        debugger;
                        let txtID = $(e.target).attr("data-id");
                        if (parseInt(txtID) < 30) {
                            $("#txtOrderNo" + (parseInt(txtID) + 1)).focus();
                        }

                    } else {
                        e.target.value = "";
                    }

                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_HeatTreat_001").text().trim();
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        }
    }


});
$("#btnPlannedUploadDateNow").focusout(function () {
    $("#ddlMatlType1").focus();
});

//$("#btnStartDataSave").focusout(function () {
//    $("#txtFurnaceOutDateTime").focus();
//});


$("#btnDeleteRow").click(function () {
    isSelectRow = false;
    for (var i = 1; i <= 30; i++) {
        if ($("#selectRow" + i).is(':checked')) {
            $("#selectRow" + i).prop('checked', false)
            clearTableRow("trId" + i);
            $("#selectRow" + i).attr("disabled", true);
            $("#hdfProcessPlanCD" + i).text("");
            $("#hdfheattreatmentCD" + i).text("");
            $("#hdfProcessPlanCD" + i).val("");
       //     $("#ddlMatlType" + i).removeClass("Mandatory2", "Mandatory2");
            $("#txtFurnacePosition1" + i).removeClass("Mandatory2", "Mandatory2");
            $("#txtFurnacePosition2" + i).removeClass("Mandatory2", "Mandatory2");
            $("#ddlNextProcess" + i).removeClass("Mandatory2", "Mandatory2");
            isSelectRow = true
        }
    }
    if ($("#selectRow" + i).prop('checked', false) && isSelectRow == false) {
        var message = $("#lblErrorMessageE_HeatTreat_033").text().trim();
        showMessageDialog(message, "", WARNING);
    }
});


function disableGridCheckBox() {
    for (var i = 1; i <= 30; i++) {
        $("#selectRow" + i).attr("disabled", true);
    }
}


document.querySelector("#tblHeatTreatMent tbody").addEventListener("change", function (event) {
    debugger;
    const row = event.target.closest("tr");
    var $val = row.id;
    const input = $val;
    const [, letters, number] = input.match(/^([a-zA-Z]+)(\d+)$/) || [];
   


});


function onOrderNoInputChange(rowID) {
    debugger;
    //alert("call");   
    var orderNo = $("#txtOrderNo" + rowID).val();
    if (orderNo.trim() !== "") {
        $("#selectRow" + rowID).removeAttr("disabled");
    }
    else {
        $("#selectRow" + rowID).attr("disabled", true);
    }
}

function disableDeleteRowButtons() {
    $("#btnDeleteRow").attr("disabled", "disabled");
}
function enableDeleteRowButtons() {
    $("#btnDeleteRow").removeAttr("disabled");
}