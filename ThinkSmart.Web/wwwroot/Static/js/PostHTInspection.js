userName = "";
var $btnChuubanSearch;
var $btnSave;
var combinedDecisionOK = 1;
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var $btnInitializeIsMandatoryFieldsFilled
var $btnSave;
var $btnEdit;
var $btnDelete;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var operatorCD = 0;
var emptyRow = 30;

$(document).ready(function () {

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    window.resizeTo(1520, 780); /*window.resize(1920, 1080);*/


    let InternalGroupNo = getURLParameters(window.location.href).trim();

    bindEvents();
    $("#txtInspectionDate").focus();
    userName = $("#hdfUserName").val();
    userName = $("#hdfUserName").val();

    if (parseInt(InternalGroupNo) > 0) {
        getPostHTDetailsByInternalQCD(InternalGroupNo)

        enableSaveButtons();
        enableDeleteRowButtons();
    }
    //$("#btnYobinaStartOpeartorSearch").click(function (event) {

    //    var yobina = $.trim($("#txtInputSearch").val());
    //    setStartOperatorListStartDetails(yobina, "");
    //    $("#ddlOperatorName").focus();
    //});
    disableGridCheckBox();

    // as();
});


document.addEventListener('change', function (e) {


    var titlevalue = $("#" + e.target.id).data("title");
    var rowID = $("#" + e.target.id).data("rowid");

    if (titlevalue == "txtDMTMin") {
        var DMTMin = $("#" + e.target.id).val();

        var DMTMax = $("#" + ('txtDMTMax' + rowID)).val();
        if ($("#txtDMTMax" + rowID).val() == "") {
            $("#txtActDmtAvg" + rowID).text(0);
        } else if ($("#txtDMTMin" + rowID).val() == "") {
            $("#txtActDmtAvg" + rowID).text(0);
        } else {
            var min = parseFloat(DMTMin);
            var max = parseFloat(DMTMax);

            if (!isNaN(min) && !isNaN(max)) {
                var totalAmount = min + max;
                $("#" + ('txtActDmtAvg' + rowID)).text(totalAmount / 2);
            }
            else {
                $("#" + ('txtActDmtAvg' + rowID)).text("");
            }
            //var totalAmount = (parseInt(DMTMin) + parseInt(DMTMax));
            //if (totalAmount != NaN) {
            //    $("#" + ('txtActDmtAvg' + rowID)).text(parseInt(totalAmount) / 2);
            //}
        }
    }

    if (titlevalue == "txtDMTMax") {
        var DMTMin = $("#" + ('txtDMTMin' + rowID)).val();

        var DMTMax = $("#" + ('txtDMTMax' + rowID)).val();
        if ($("#txtDMTMax" + rowID).val() == "") {
            $("#txtActDmtAvg" + rowID).text(0);
        } else if ($("#txtDMTMin" + rowID).val() == "") {
            $("#txtActDmtAvg" + rowID).text(0);
        } else {
            var min = parseFloat(DMTMin);
            var max = parseFloat(DMTMax);

            if (!isNaN(min) && !isNaN(max)) {
                var totalAmount = min + max;
                $("#" + ('txtActDmtAvg' + rowID)).text(totalAmount / 2);
            }
            else {
                $("#" + ('txtActDmtAvg' + rowID)).text("");
            }

            //var totalAmount = (parseInt(DMTMin) + parseInt(DMTMax));
            //if (totalAmount != NaN) {
            //    $("#" + ('txtActDmtAvg' + rowID)).text(parseFloat(totalAmount) / 2);
            //}

        }
    }

    debugger;
    var txtcenter = $("#" + ("txtCenter" + rowID)).val();
    var txtD1 = $("#" + ("txtD1" + rowID)).val();
    var txtD2 = $("#" + ("txtD2" + rowID)).val();
    var txtD3 = $("#" + ("txtD3" + rowID)).val();
    var txtAVG = $("#" + ("txtAvg" + rowID)).val();




    if (parseInt(txtcenter) >= 0) {
        debugger;
        if (parseInt(txtcenter) >= 0 && parseInt(txtD1) >= 0 && parseInt(txtD2) >= 0 && parseInt(txtD3) >= 0) {
            var total = parseFloat(txtcenter) + parseFloat(txtD1) + parseFloat(txtD2) + parseFloat(txtD3);
            var totalAvg = total / 4;
            if (totalAvg == 0) {
                totalAvg = "";
                $("#" + ('txtAvg' + rowID)).text(totalAvg);
            }
            else {
                $("#" + ('txtAvg' + rowID)).text(totalAvg.toFixed(2));
            }

        }
        else if (parseInt(txtcenter) == 0 && parseInt(txtD1) == 0 && parseInt(txtD2) == 0 && parseInt(txtD3) == 0) {
            var total = parseFloat(txtcenter) + parseFloat(txtD1) + parseFloat(txtD2) + parseFloat(txtD3);
            var totalAvg = total / 4;

            if (totalAvg == 0) {
                totalAvg = "";
                $("#" + ('txtAvg' + rowID)).text(totalAvg);
            }
            else {
                $("#" + ('txtAvg' + rowID)).text(totalAvg.toFixed(2));
            }

        }


    }
    else {
        if (parseInt(txtD1) >= 0 && parseInt(txtD2) >= 0 && parseInt(txtD3) >= 0) {
            var total = parseFloat(txtD1) + parseFloat(txtD2) + parseFloat(txtD3);
            var totalAvg = total / 3;

            if (totalAvg == 0) {
                totalAvg = "";
                $("#" + ('txtAvg' + rowID)).text(totalAvg);
            }
            else {
                $("#" + ('txtAvg' + rowID)).text(totalAvg.toFixed(2));
            }

        }
        else if (txtD1 == "" && txtD2 == "" && txtD3 == "") {
            if (txtD1 == "") {
                txtD1 = 0;
            }
            if (txtD2 == "") {
                txtD2 = 0;
            }
            if (txtD3 == "") {
                txtD3 = 0;
            }
            var total = parseFloat(txtD1) + parseFloat(txtD2) + parseFloat(txtD3);
            var totalAvg = total / 3;
            if (totalAvg == 0) {
                totalAvg = "";
                $("#" + ('txtAvg' + rowID)).text(totalAvg);
            }
            else {
                $("#" + ('txtAvg' + rowID)).text(totalAvg.toFixed(2));
            }



        }

        if (parseInt(txtD1) >= 0 || parseInt(txtD2) >= 0 || parseInt(txtD3) >= 0) {
            if (txtD1 == "") {
                txtD1 = 0;
            }
            if (txtD2 == "") {
                txtD2 = 0;
            }
            if (txtD3 == "") {
                txtD3 = 0;
            }

            var total = parseFloat(txtD1) + parseFloat(txtD2) + + parseFloat(txtD3);
            var totalAvg = total / 3;
            if (totalAvg == 0) {
                totalAvg = "";
                $("#" + ('txtAvg' + rowID)).text(totalAvg);
            }
            else {
                $("#" + ('txtAvg' + rowID)).text(totalAvg.toFixed(2));
            }


        }
        else if (txtD1 == "" || txtD2 == "" || txtD3 == "") {
            if (txtD1 == "") {
                txtD1 = 0;
            }
            if (txtD2 == "") {
                txtD2 = 0;
            }
            if (txtD3 == "") {
                txtD3 = 0;
            }
            var total = parseFloat(txtD1) + parseFloat(txtD2) + parseFloat(txtD3);
            var totalAvg = total / 3;
            if (totalAvg == 0) {
                totalAvg = "";
                $("#" + ('txtAvg' + rowID)).text(totalAvg);
            }
            else {
                $("#" + ('txtAvg' + rowID)).text(totalAvg.toFixed(2));
            }


        }



    }



    if (titlevalue == "txtChuuban") {

        if ($("#txtChuuban" + rowID).val() != "") {
            $("#chkPostGrid" + rowID).removeAttr("disabled");
            //$("#btnChuubanSearch").focus();
        } else {
            $("#chkPostGrid" + rowID).attr("disabled", true);
        }
    }

});

function setDecimalFormats() {
    var decimalVal = [];
    var fieldNameInList = "";
    var structure = "";

    for (var i = 1; i <= 50; i++) {
        if (fieldNameInList == "") {
            fieldNameInList = "#txtD1" + i + ", #txtD2" + i + ", #txtD3" + i + ", #txtCenter" + i;
        }
        else {
            fieldNameInList = "#txtD1" + i + ", #txtD2" + i + ", #txtD3" + i + ", #txtCenter" + i;
        }
        if (structure == "") {
            structure += "#txtStructure" + i;
        }
        else {
            structure += "#txtStructure" + i;
        }

        decimalVal.push(fieldNameInList);
        $(fieldNameInList).keyup(function (event) {
            RestrictToValidDecimalFormat($(this), 3, 1);
        });

        decimalVal.push(structure);
        $(structure).keyup(function (event) {
            RestrictToValidDecimalFormat($(this), 2, 1);
        });
    }


}

function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('InternalGroupNo=');
    result = sParameterName[1];

    return result;
}


function disableGridCheckBox() {
    for (var i = 1; i < emptyRow; i++) {
        $("#chkPostGrid" + i).attr("disabled", true);
    }
}
function getPostHTDetailsByInternalQCD(groupNo) {

    //window.resizeTo(1050, 990);
    EditCheck = "1";

    var ajaxTime = new Date().getTime();

    var jsonData = "{INTERNAL_QUALITY_GROUP_NO :'" + groupNo + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHTInspection/getPostHTDetailsByInternalQCD?groupNo=" + parseInt(groupNo),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            SetPostHTDetails(jsonResult);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetPostHTDetails(jsonResult) {

    // setStartOperatorListStartDetails($("#txtInputSearch").val(), "");
    $("#txtInspectionDate").val($.format.date(new Date(jsonResult[0].inT_QLTY_TEST_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtStartOperator").val(jsonResult[0].iQ_START_OPERATOR_NAME);
    $("#hdfStartOperatorCD").val(jsonResult[0].inT_QLTY_TEST_OPERATOR_CD);
    $("#ddlMachine").val(jsonResult[0].machinE_CD);
    $('#txtFurnaceNo').val(jsonResult[0].furnacE_NO);
    $("#ddlProcCondition").val(jsonResult[0].hT_CONDITION);
    $("#txtPatternNo").val(jsonResult[0].patterN_NO);
    $("#hdfGroupNo").val(jsonResult[0].internaL_QUALITY_GROUP_NO);

    //grid data
    $.each(jsonResult, function (key, jsonObj) {

        key = parseInt(key) + 1;
        $("#txtChuuban" + key).val(jsonObj.chuuban.replace(/-/g, ''));
        $("#txtCustomer" + key).text(jsonObj.customeR_NAME);
        $("#txtProdName" + key).text(jsonObj.proD_NAME);
        $("#txtCharge" + key).text(jsonObj.charge);
        $("#txtMatlDim" + key).text(jsonObj.pP_MATL_DIM_D_STRING);
        $("#txtProdDim" + key).text(jsonObj.pP_PROD_DIM_D_STRING);

        $("#txtPosition1" + key).val(jsonObj.locatioN_IN_FURNACE1);
        $("#txtPosition2" + key).val(jsonObj.locatioN_IN_FURNACE2);
        $("#txtStackPos" + key).val(jsonObj.stacK_POSITION);
        $("#txtOrientation" + key).val(jsonObj.orientation);
        $("#txtInOut" + key).val(jsonObj.iN_OUT);

        $("#lblDMFDMT" + key).val(jsonObj.dmF_DMF);
        $("#txtDMF" + key).val(jsonObj.acT_DM_F);
        $("#txtDMTMin" + key).val(jsonObj.acT_DM_T_MIN);
        $("#txtDMTMax" + key).val(jsonObj.acT_DM_T_MAX);
        $("#txtActDmtAvg" + key).text(jsonObj.acT_DM_T_AVG);
        $("#txtStructure" + key).val(jsonObj.acT_STRUCTURE);
        $("#txtCenter" + key).val(jsonObj.hrD_CENTER);
        $("#txtD1" + key).val(jsonObj.hrD_D4_1);
        $("#txtD2" + key).val(jsonObj.hrD_D4_2);
        $("#txtD3" + key).val(jsonObj.hrD_D4_3);
        $("#txtAvg" + key).text(jsonObj.hrD_D4_AVG);

        $("#ddlDecision" + key).val(jsonObj.internaL_QUALITY_DECISION);
        //  $("#ddlNextProcess" + key).val(jsonObj.iQ_NEXT_PROCESS);

        $("#hdfProcessPlanCD" + key).text(jsonObj.procesS_PLAN_CD);
        $("#hdfInternal_Q_CD" + key).text(jsonObj.internaL_QUALITY_CD);
        $("#chkPostGrid" + key).removeAttr("disabled");
    });
    enableEditButtons();
    enableDeleteButtons();
    disableSaveButtons();

}

function Mask() {
    $("#txtInspectionDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    SetDecimalFormat();
    // alert("mask");
}

function SetDecimalFormat() {
    //$(".structure").keyup(function (event) {
    //    RestrictToValidDecimalFormat($(this), 2, 1)
    //});

    //$(".Center").keyup(function (event) {
    //    RestrictToValidDecimalFormat($(this), 3, 1)
    //});


    //$(".HrdD").keyup(function (event) {
    //    RestrictToValidDecimalFormat($(this), 3, 1)
    //});
}

function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

//function setStartOperatorListStartDetails(yobina, defaultSelectID) {

//    getOperatorList(yobina, $("#ddlOperatorName"), defaultSelectID);
//};


//Start Operator validation on Machine Textbox(tab,enter,click)
$('#txtStartOperator')
    .on('focus', function () {
        operatorStartModified = false;
    })
    .on('input', function () {
        operatorStartModified = true;
    });
$('#txtStartOperator').on('keydown focusout', function (e) {
    var operator = e.target.value ? e.target.value : null;
    var operatorCD = $(this).val().trim();
    var operatorName = operatorCD.substring(0, 3);
    var operatorNumber = operatorCD.substring(3, 9);
    if (e.which === 13 && operator && operatorStartModified == true) {
        $("#hdfStartOperatorCD").val(operatorNumber);
        enterPressedStartOpr = true;
        getOperatorNameByCD($(this));
        $("#txtFurnaceNo").focus();


    }

    if (e.type === 'focusout' && operator && operatorStartModified == true) {
        if (enterPressedStartOpr) {

            enterPressedStartOpr = false; // Reset the flag
            return false;
        }
        $("#hdfStartOperatorCD").val(operatorNumber);
        getOperatorNameByCD($(this));
        $("#txtFurnaceNo").focus();
    }
    enterPressedStartOpr = false; // Reset the flag
});

function getOperatorNameByCD($target) {

    var ajaxTime = new Date().getTime();
    var operatorCD = $target.val().trim();
    var operatorVal = operatorCD.substring(0, 3);
    var operatorName = operatorVal.toUpperCase();
    var operatorNumber = operatorCD.substring(3, 9);

    // var operatorCd = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 
    if (operatorName != OPRATORNAME) {
        var message = $("#lblErrorMessageE_PostHT_033").text().trim().replace("*****", $("#lblStartOperator").text());

        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }

    if (operatorNumber == "") {
        var message = $("#lblErrorMessageE_PostHT_030").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else if (operatorNumber.length > 6) {
        var message = $("#lblErrorMessageE_PostHT_033").text().trim();
        showMessageDialog(message, $target.attr("id"), WARNING);
        $target.val("");
        return false;
    }
    else {

        var jsonData = "{OperatorCD: '" + operatorNumber + "', USER_NAME: '" + userName + "' }";

        var jsonRetval = "";

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/PostHtInspection/GetOperatorNamebyCD?OperatorCD=" + operatorNumber,
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;

                var operatorName = response.message;

                if (operatorName == null || operatorName == "") {
                    var message = $("#lblErrorMessageE_PostHT_031").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }
                else if (operatorName.iN_ACTIVE_FLAG == 1) {
                    var message = $("#lblErrorMessageE_PostHT_032").text().trim();
                    showMessageDialog(message, $target.attr("id"), WARNING);
                    $target.val(""); // clear the textbox so that user does not need to clear it manually and scan the operator barcode again
                }
                else {
                    $target.val(operatorName.yobina);
                    $("#txtFurnaceNo").focus();
                }
            })

            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_PostHTInspection").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            })
    }
}


function bindEvents() {
    getMachineList();
    getStackPositionList();
    setStartSysDateTime();
    //  getNextProcessList();
    Mask();
    getDecisionList();
    setDecimalFormats();
    setProcConditionKubun();
    setInOutKubun();
    setOrientationKubun();
    disableSaveButtons();
    disableEditButtons();
    disableDeleteButtons();
    disableDeleteRowButtons();

    $("#btnSave").unbind("click")
    $("#btnSave").click(function (event) {
        debugger;
        var txtD4List = "";
        var ddlDecision = "";
        var ddlNextProcess = "";

        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

        if (!IsMandatoryFieldsFilled()) {
            return false;
        }

        if (!checkDate($("#txtInspectionDate"))) {
            var message = $("#lblErrorMessageE_PostHT_01").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }
        if (!IsDateValid()) {
            return false;
        }

        for (var i = 1; i < emptyRow; i++) {
            var txtProdName = $("#txtD1" + i).val();
            if ($("#hdfProcessPlanCD" + i).text() != "") {
                if ($("#txtD1" + i).val() == "" || $("#txtD2" + i).val() == "" || $("#txtD3" + i).val() == "") {
                    if (txtD4List == "") {
                        txtD4List = i;
                    } else {
                        txtD4List = txtD4List + ", " + i;
                    }
                }
                if ($("#ddlDecision" + i).val() == "0") {
                    if (ddlDecision == "") {
                        ddlDecision = i;
                    } else {
                        ddlDecision = ddlDecision + ", " + i;
                    }
                }

            }
        }

        if (txtD4List != "" || ddlDecision != "") {

            if (ddlDecision != "") {
                var message = $("#lblErrorMessageE_PostHT_17").text().trim().replace("NNN", ddlDecision);
            }

            if (txtD4List != "") {
                var message = $("#lblErrorMessageE_PostHT_19").text().trim().replace("NNN", txtD4List);
            }
            showMessageDialog(message, "", WARNING);
        }
        else {

            savePostHTInspectionDetails($callFrom);
        }
    });

    $("#btnEdit").click(function (event) {

        var txtD4List = "";
        var ddlDecision = "";
        var ddlNextProcess = "";
        if (!IsMandatoryFieldsFilled()) {
            return false;
        }
        resetResponseTime();
        var $callFrom = $(this);
        event.preventDefault();

        if (!checkDate($("#txtInspectionDate"))) {
            var message = $("#lblErrorMessageE_PostHT_01").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "", WARNING);
            return false;
        }
        if (!IsDateValid()) {
            return false;
        }

        for (var i = 1; i < emptyRow; i++) {
            var txtProdName = $("#txtD1" + i).val();
            if ($("#hdfProcessPlanCD" + i).text() != "") {
                if ($("#txtD1" + i).val() == "" || $("#txtD2" + i).val() == "" || $("#txtD3" + i).val() == "") {
                    if (txtD4List == "") {
                        txtD4List = i;
                    } else {
                        txtD4List = txtD4List + ", " + i;
                    }
                }
                if ($("#ddlDecision" + i).val() == "0") {
                    if (ddlDecision == "") {
                        ddlDecision = i;
                    } else {
                        ddlDecision = ddlDecision + ", " + i;
                    }
                }
                //if ($("#ddlNextProcess" + i).val() == "0") {
                //    if (ddlNextProcess == "") {
                //        ddlNextProcess = i;
                //    } else {
                //        ddlNextProcess = ddlNextProcess + ", " + i;
                //    }
                //}
            }
        }

        if (txtD4List != "" || ddlDecision != "") {

            if (ddlDecision != "") {
                var message = $("#lblErrorMessageE_PostHT_17").text().trim().replace("NNN", ddlDecision);
            }
            //if (ddlNextProcess != "") {
            //    var message = message + "<br>" + $("#lblErrorMessageE_PostHT_18").text().trim().replace("NNN", ddlNextProcess);
            //}
            if (txtD4List != "") {
                var message = message + "<br>" + $("#lblErrorMessageE_PostHT_19").text().trim().replace("NNN", txtD4List);
            }
            showMessageDialog(message, "", WARNING);
        }
        else {

            var ajaxTime = new Date().getDate();
            //const queryString1 = window.location.search;
            var groupNo = parseInt($("#hdfGroupNo").val());



            var settings = {
                async: true,
                crossDomain: true,
                cache: false,
                //dataType: "json",
                url: APIURL + "/PostHtInspection/PostHTInspectionCheckMilkSheetData?groupNo=" + groupNo,
                method: "POST",
                contentType: "application/json; charset=utf-8",

            }
            $.ajax(settings)
                .done(function (response) {

                    responseTime = new Date().getTime() - ajaxTime;
                    var jsonResult = response.message;

                    if (jsonResult > 0) {

                        var message = $("#valMsgEditNotAllowedUsedInMillSheet").text();
                        showMessageDialog(message, "btnInitialize", WARNING);
                    }
                    else {

                        updatePostHTInspectionDetails($callFrom);
                    }


                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });






            
        }
    });

}

function duplicateFurnaceNo() {

    var furnaceNo;
    //var machineCd = $("#ddlMachine").val() ? $("#ddlMachine").val() : null;
    var furNo = $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null;

    var returnVal = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetFurnaceNo?furNo=" + furNo,// + "&isNull=" + true,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
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
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, "", ERROR);
        });
    return returnVal;

}

function savePostHTInspectionDetails($callFrom) {

    debugger;
    if (duplicateFurnaceNo()) {
        var message = $("#lblErrorMessageE_PostHT_027").text();
        showMessageDialog(message, "", WARNING);
        return false;
    }

    var isdataSaved = false;
    var MaxGruopNo;
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetMaxGruopNo",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            MaxGruopNo = response.message;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            //var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

    const val = $("#txtInspectionDate").val();
    const InsDate = new Date(val);
    var currentTime = new Date();

    var saveData = [];
    for (var i = 1; i <= emptyRow; i++) {

        var Seconds = parseInt(i, 10).toString().padStart(2, '0');

        var DateVal = InsDate.getFullYear() + '/' + String(InsDate.getMonth() + 1).padStart(2, '0') + '/' + String(InsDate.getDate()).padStart(2, '0') + ' ' + String(currentTime.getHours()).padStart(2, '0') + ':' + String(currentTime.getMinutes()).padStart(2, '0') + ':' + Seconds;
        var aa = $("#hdfProcessPlanCD" + i).text();
        if ($("#hdfProcessPlanCD" + i).text() !== '') {

            var ajaxTime = new Date().getDate();
            var jsonDataForSave = ({

                procesS_PLAN_CD: $("#hdfProcessPlanCD" + i).text(),
                SERIAL_NUMBER: i,
                INTERNAL_QUALITY_GROUP_NO: MaxGruopNo,

                INT_QLTY_TEST_DATE_STRING: DateVal,


                // INT_QLTY_TEST_DATE: ($("#txtInspectionDate").val().split(" ")[0]).replaceAll('/', '-'),  //date format replace('/', '-') because of  undefine value
                INT_QLTY_TEST_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
                //  CH_START_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
                MACHINE_CD: $("#ddlMachine").val() ? $("#ddlMachine").val() : null,
                FURNACE_NO: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null,
                HT_CONDITION: $("#ddlProcCondition").val() ? $("#ddlProcCondition").val() : null,
                PATTERN_NO: $("#txtPatternNo").val() ? $("#txtPatternNo").val() : null,

                //grid data
                LOCATION_IN_FURNACE1: $("#txtPosition1" + i).val() ? $("#txtPosition1" + i).val() : null,
                LOCATION_IN_FURNACE2: $("#txtPosition2" + i).val() ? $("#txtPosition2" + i).val() : null,
                STACK_POSITION: $("#txtStackPos" + i).val() ? $("#txtStackPos" + i).val() : null,

                ORIENTATION: $("#txtOrientation" + i).val() ? $("#txtOrientation" + i).val() : null,
                IN_OUT: $("#txtInOut" + i).val() ? $("#txtInOut" + i).val() : null,

                DMF_DMT: $("#txtDMFDMT" + i).val() ? $("#txtDMFDMT" + i).val() : null,
                ACT_DM_F: $("#txtDMF" + i).val() ? $("#txtDMF" + i).val() : null,
                ACT_DM_T_MIN: $("#txtDMTMin" + i).val() ? $("#txtDMTMin" + i).val() : null,
                ACT_DM_T_MAX: $("#txtDMTMax" + i).val() ? $("#txtDMTMax" + i).val() : null,
                ACT_DM_T_AVG: $("#txtActDmtAvg" + i).text() ? $("#txtActDmtAvg" + i).text() : null,
                ACT_STRUCTURE: $("#txtStructure" + i).val() ? $("#txtStructure" + i).val() : null,
                HRD_CENTER: $("#txtCenter" + i).val() ? $("#txtCenter" + i).val() : null,
                HRD_D4_1: $("#txtD1" + i).val() ? $("#txtD1" + i).val() : null,
                HRD_D4_2: $("#txtD2" + i).val() ? $("#txtD2" + i).val() : null,
                HRD_D4_3: $("#txtD3" + i).val() ? $("#txtD3" + i).val() : null,
                HRD_D4_AVG: $("#txtAvg" + i).text() ? $("#txtAvg" + i).text() : null,
                INTERNAL_QUALITY_DECISION: $("#ddlDecision" + i).val() ? $("#ddlDecision" + i).val() : null,
                //  IQ_NEXT_PROCESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
                USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,


                //USER_NAME: userName ? userName : null
            });
            saveData.push(jsonDataForSave);
        }

    }

    var settings = {

        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/savePostHTInspectionDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(saveData)
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            jsonResult = response.message;
            //$.each(jsonResult, function (key, jsonObj) {

            //    $("#hdfInternal_Q_CD" + key).text(jsonObj.internaL_QUALITY_CD);
            //});
            $.each(jsonResult, function (key, jsonObj) {
                debugger;
                key = parseInt(key) + 1;
                $("#hdfInternal_Q_CD" + key).text(jsonObj.internaL_QUALITY_CD);

            });


            isdataSaved = true;


        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
    if (isdataSaved == true) {
        var successMessage = $("#lblSuccessMessageS_PostHT_001").text();
        showMessageDialog(successMessage, "", SUCCESS);
        disableSaveButtons();
        disableDeleteButtons();
        disableEditButtons();
    }
}


function updatePostHTInspectionDetails($callFrom) {
    debugger;
    var updateData = [];

    const val = $("#txtInspectionDate").val();  //Added Time on Screen Input Inspection Date.
    const InsDate = new Date(val);
    var currentTime = new Date();

    for (var i = 1; i <= emptyRow; i++) {
        var aa = $("#hdfProcessPlanCD" + i).text();
        var bb = $("#hdfInternal_Q_CD" + i).val();

        //Update Each Row second in Inspection Date.
        var Seconds = parseInt(i, 10).toString().padStart(2, '0');

        var DateVal = InsDate.getFullYear() + '/' + String(InsDate.getMonth() + 1).padStart(2, '0') + '/' + String(InsDate.getDate()).padStart(2, '0') + ' ' + String(currentTime.getHours()).padStart(2, '0') + ':' + String(currentTime.getMinutes()).padStart(2, '0') + ':' + Seconds;

        if ($("#hdfProcessPlanCD" + i).text() !== '') {
            var ajaxTime = new Date().getDate();
            var jsonDataForSave = ({

                procesS_PLAN_CD: $("#hdfProcessPlanCD" + i).text(),
                INTERNAL_QUALITY_CD: $("#hdfInternal_Q_CD" + i).text(),
                INT_QLTY_TEST_DATE_STRING: DateVal, //($("#txtInspectionDate").val().split(" ")[0]).replaceAll('/', '-'),  //date format replace('/', '-') because of  undefine value
                INT_QLTY_TEST_OPERATOR_CD: $("#hdfStartOperatorCD").val() ? $("#hdfStartOperatorCD").val() : null,
                MACHINE_CD: $("#ddlMachine").val() ? $("#ddlMachine").val() : null,
                FURNACE_NO: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null,
                HT_CONDITION: $("#ddlProcCondition").val() ? $("#ddlProcCondition").val() : null,
                PATTERN_NO: $("#txtPatternNo").val() ? $("#txtPatternNo").val() : null,

                //grid data
                LOCATION_IN_FURNACE1: $("#txtPosition1" + i).val() ? $("#txtPosition1" + i).val() : null,
                LOCATION_IN_FURNACE2: $("#txtPosition2" + i).val() ? $("#txtPosition2" + i).val() : null,
                STACK_POSITION: $("#txtStackPos" + i).val() ? $("#txtStackPos" + i).val() : null,
                ORIENTATION: $("#txtOrientation" + i).val() ? $("#txtOrientation" + i).val() : null,
                IN_OUT: $("#txtInOut" + i).val() ? $("#txtInOut" + i).val() : null,
                DMF_DMT: $("#txtDMFDMT" + i).val() ? $("#txtDMFDMT" + i).val() : null,
                ACT_DM_F: $("#txtDMF" + i).val() ? $("#txtDMF" + i).val() : null,
                ACT_DM_T_MIN: $("#txtDMTMin" + i).val() ? $("#txtDMTMin" + i).val() : null,
                ACT_DM_T_MAX: $("#txtDMTMax" + i).val() ? $("#txtDMTMax" + i).val() : null,
                ACT_DM_T_AVG: $("#txtActDmtAvg" + i).text() ? $("#txtActDmtAvg" + i).text() : null,
                ACT_STRUCTURE: $("#txtStructure" + i).val() ? $("#txtStructure" + i).val() : null,
                HRD_CENTER: $("#txtCenter" + i).val() ? $("#txtCenter" + i).val() : null,
                HRD_D4_1: $("#txtD1" + i).val() ? $("#txtD1" + i).val() : null,
                HRD_D4_2: $("#txtD2" + i).val() ? $("#txtD2" + i).val() : null,
                HRD_D4_3: $("#txtD3" + i).val() ? $("#txtD3" + i).val() : null,
                HRD_D4_AVG: $("#txtAvg" + i).text() ? $("#txtAvg" + i).text() : null,
                INTERNAL_QUALITY_DECISION: $("#ddlDecision" + i).val() ? $("#ddlDecision" + i).val() : null,
                //IQ_NEXT_PROCESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
                USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,
                USER_NAME: userName ? userName : null
            });
            updateData.push(jsonDataForSave);

        }
    }
    var settings = {

        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/updatePostHTInspectionDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(updateData)
    }
    $.ajax(settings)
        .done(function (response) {


            responseTime = new Date().getTime() - ajaxTime;
            var successMessage = $("#lblSuccessMessageS_PostHT_001").text();
            showMessageDialog(successMessage, "", SUCCESS);
            disableSaveButtons();
            var jsonResult = response.message;
            //location.href = removeParameter("InternalGroupNo", window.location.href);
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function removeParameter(key, sourceURL) {
    var rtn = sourceURL.split("?")[0],
        param,
        params_arr = [],
        queryString = (sourceURL.indexOf("?") !== -1) ? sourceURL.split("?")[1] : "";
    if (queryString !== "") {
        params_arr = queryString.split("&");
        for (var i = params_arr.length - 1; i >= 0; i -= 1) {
            param = params_arr[i].split("=")[0];
            if (param === key) {
                params_arr.splice(i, 1);
            }
        }
        if (params_arr.length) rtn = rtn + "?" + params_arr.join("&");
    }
    return rtn;
}

$("#btnInitialize").click(function (event) {
    initialize();
    location.href = removeParameter("InternalGroupNo", window.location.href);
});

function FillGridDropDown() {
    // getNextProcessList();
    getDecisionList();
}
function initialize() {
    $("#txtInspectionDate").val("");
    $("#ddlOperatorName").empty();
    $("#txtFurnaceNum").val("");
    $("#txtFurnaceNo").val("");
    $("#txtPatternNo").val("");
    $("#ddlProcCondition").val(0);
    clearPanel($("#pnlPostHtDetailsOnGrid"));
    clearTable($("#tblPostHTInspection"));
    getMachineList();
    disableGridCheckBox();
    setStartSysDateTime();
    disableDeleteButtons();
    disableEditButtons();
    disableSaveButtons();
}

function clearData(e) {

    $("#txtInspectionDate").val("");
    $("#ddlOperatorName").val("");
    $("#ddlMachine").val("");
    $("#txtFurnaceNum").text("");
    $("#txtFurnaceNo").val("");
    $("#txtPatternNo").val("");
    $("#ddlProcCondition").val(0);
    $("#dmtAvg").val();
    clearPanel($("#pnlPostHtDetailsOnGrid"));
}
function clearTableRow(rowid) {

    // $('#' + rowid).find('input:text').val('');
    $.each($('#' + rowid).find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });
    $.each($('#' + rowid).find("select"), function (index, dropdown) {
        $("#" + dropdown.id + " option:selected").prop("selected", false);
        $("#" + dropdown.id + " option:first").prop("selected", "selected");
    });
    $.each($('#' + rowid).find("input:checkbox"), function (index, checkBox) {
        $(checkBox).prop('checked', false);
    });

    $.each($('#' + rowid).find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });

    $.each($('#' + rowid).find("input:hidden"), function (index, label) {
        $(label).text("");
    });

    //$('#' + rowid).find('select').val(0);
    //$('#' + rowid).find("label[id^='txt']").text("");
    //$('#txtActDmtAvg2' + rowid).find("TextBox[id^='txt']").text("");
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

function disableEditButtons() {
    $("#btnEdit").attr("disabled", "disabled");
}
function enableEditButtons() {
    $("#btnEdit").removeAttr("disabled");
}
function disableSaveButtons() {
    $("#btnSave").attr("disabled", "disabled");
}
function enableSaveButtons() {
    $("#btnSave").removeAttr("disabled");
}
function disableDeleteButtons() {
    $("#btnDelete").attr("disabled", "disabled");
}
function enableDeleteButtons() {
    $("#btnDelete").removeAttr("disabled");
}
function disableDeleteRowButtons() {
    $("#btnDeleteRow").attr("disabled", "disabled");
}
function enableDeleteRowButtons() {
    $("#btnDeleteRow").removeAttr("disabled");
}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
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
        //$("#imgError").show();
        //$("#imgWarning").show();

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
    }

    return returnVal;

}

function IsFillGridViewMandatoryFields() {
    var i = 0;
    var gridData = [];
    $('table#tblPostHTInspection tbody tr').each(function (index, item) {

        var $tr = $(this);
        var data = {
            POST_HT_INSPECTION_ROW: i,
            POST_HT_ORDER_NO: $tr.find("#txtChuuban").val() ? $tr.find("#txtChuuban").val() : null,
            POST_HT_D1: $tr.find("#txtD1").val() ? $tr.find("#txtD1").val() : null,
            POST_HT_D2: $tr.find("#txtD2").val() ? $tr.find("#txtD2").val() : null,
            POST_HT_D3: $tr.find("#txtD3").val() ? $tr.find("#txtD3").val() : null,
            POST_HT_Result: $tr.find("#ddlDecision").val() ? $tr.find("#ddlDecision").val() : 0,
            //  POST_HT_NEXT_PROCESS: $tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : 0,
        };
        i++;
        gridData.push(data);
    });
    /*console.log(JSON.stringify(gridData));*/
    var success = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/IsFillGridViewMandatoryFields",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                success = true;
            }
            else {
                showMessageDialog(response.message, '', WARNING);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    return success;
}

function IsChuubanMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".MandatoryTextBoxForGrid  "), function (index, mandatoryField) {
        if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value) {
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
        // $("#imgError").show();
        bootbox.alert({
            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
            message: message,
            centerVertical: true,
            callback: function () {
                if (focusFieldID !== "") {
                    $("#" + focusFieldID).focus();
                }
            }
        });
    }

    return returnVal;

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
            //  console.log("Server time is: " + serverTime.toLocaleTimeString());
            $("#txtInspectionDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            showErrorDiv(errorCode);
        });

}



function getOperatorList(yobina, $target) {

    var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetOperatorList?name=" + yobina + "&operatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            //ddlOperatorName
            var result = response.message;
            bindOperatorDropdown(response.message, $target, yobina);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
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

function getMachineList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetMachineList",
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

            //focusFieldID = $(':focus').attr("id");
            //message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            //showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMachineDropdown(jsonResult) {
    $("#ddlMachine").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMachine").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
}

function getStackPositionList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/PostHtInspection/GetStackPosition",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindStackPositionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            //var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            //message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            // showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindStackPositionDropdown(jsonResult) {
    for (var i = 1; i <= emptyRow; i++) {
        $('table#tblPostHTInspection tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $txtStackPos = $tr.find("#txtStackPos" + i);
            $txtStackPos.html("").append($('<option value="0"></option>'));
            $.each(jsonResult, function (key, jsonObj) {
                $txtStackPos.append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            });
        });
    }
}

function getNextProcessList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetNextProcess",
        method: "GET",
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
            //var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            //message = $.trim($("#lblErrorMessageE_Cold_Drawing_001").text());
            // showMessageDialog(message, focusFieldID, ERROR);
        });
}


function bindNextProcessDropdown(jsonResult) {
    for (var i = 1; i <= emptyRow; i++) {

        if ($("#hdfCultureName").val() == "en") {
            $.each(jsonResult, function (key, jsonObj) {
                $("#ddlNextProcess" + i).append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));


            });
        }
        if ($("#hdfCultureName").val() == "ja-JP") {
            $.each(jsonResult, function (key, jsonObj) {
                $("#ddlNextProcess" + i).append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));



            });
        }

    }
}

function getDecisionList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/PostHtInspection/GetResult",
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
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            // showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindproductunitkubundropdown(jsonResult) {
    for (var i = 1; i <= emptyRow; i++) {
        $('table#tblPostHTInspection tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $ddlDecision = $tr.find("#ddlDecision" + i);
            /* $ddlDecision.html("").append($('<option value="0"></option>'));*/
            $.each(jsonResult, function (key, jsonObj) {
                $ddlDecision.append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            });
        });
    }
}

function showMessageDialog(message, focusFieldID, messageType) {
    $("#dialogMessageIcon img").hide();
    switch (messageType) {
        case WARNING:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        case ERROR:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/error.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        case SUCCESS:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        default:
            break;
    };
}

function showMsgDuplicateChuuban(message, focusFieldID, messageType, chuuban, rowNo) {
    debugger;
    $("#dialogMessageIcon img").hide();
    message = message.replace('NNN', rowNo);
    message = message.replace('XXX', chuuban);
    switch (messageType) {
        case WARNING:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        default:
            break;
    };
}
function checkDate(field) {

    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
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

function IsDateValid() {

    retval = true; // assume end data is valid
    // If Date is greater than current Date, show error
    var currentDate = new Date();
    var aa = $("#txtInspectionDate").val();
    if (Date.parse($("#txtInspectionDate").val()) > Date.parse(currentDate)) {
        var message = $("#lblErrorMessageE_PostHT_021").text().trim();
        var focusFieldID = "txtInspectionDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
}

$("#btnChuubanSearch").click(function (event) {
    disableEditButtons();
    disableDeleteButtons();
    btnChuubanSearch();
});

$("#txtPatternNo").on("keydown", function (event) {
    debugger;
    if (
        event.type === "click" ||
        event.type === "change" ||
        event.key === "Tab" ||
        event.keyCode === 9
    ) {
        event.preventDefault(); // stop default tab behavior
        $("#txtChuuban1").focus();
    }
});

$("#txtChuuban20").on("click change keydown", function (event) {
    debugger;
    if (
        event.type === "click" ||
        event.type === "change" ||
        event.key === "Tab" ||
        event.keyCode === 9
    ) {
        event.preventDefault(); // stop default tab behavior
        $("#btnChuubanSearch").focus();
    }
});

$("#ddlDecision").on("click", function () {
    $("#btnSave").focus();
});

$("#ddlDecision").on("keydown", function (event) {
    if (event.key === "Tab" || event.keyCode === 9) {
        event.preventDefault(); // stop normal tab
        $("#btnSave").focus();
    }
});

$("#btnYobinaStartOpeartorSearch").on("keydown", function (event) {
    if (event.key === "Tab" || event.keyCode === 9) {
        event.preventDefault(); // stop default tab behavior
        $("#ddlOperatorName").focus(); // move focus
    }
});


$("#txtFurnaceNo").on("keydown", function (event) {
    if (event.key === "Tab" || event.keyCode === 9) {
        event.preventDefault(); // stop default tabbing
        $("#ddlMachine").focus();
    }
});

$("#ddlMachine").on("keydown", function (event) {
    if (event.key === "Tab" || event.keyCode === 9) {
        event.preventDefault(); // stop default tabbing
        $("#ddlProcCondition").focus();
    }
});




$("#ddlProcCondition").on("keydown", function (event) {
    if (event.key === "Tab" || event.keyCode === 9) {
        event.preventDefault(); // stop default tabbing
        $("#txtPatternNo").focus();
    }
});

function btnChuubanSearch() {
    var chuban = [];
    var isOrderNoExist = false;
    var invalidChuuban = [];
    var completedChuuban = [];
    var processPlanStartDate = [];
    var notFoundChuubanInHT = [];
    var focusID = "";
    var focusID1 = "";
    var focusID2 = "";
    var focusID3 = "";
    var message = "";

    for (var i = 1; i <= emptyRow; i++) {
        var chuban1 = $("#txtChuuban" + i).val();
        if ($.trim(chuban1) !== "" && $.trim(chuban1) !== null && $.trim(chuban1) !== 'undefined') {
            isOrderNoExist = true;
            chuban.push(chuban1);
        }
    }

    // Now display message for duplicate.
    //var duplicates = []; // store duplicate info
    //var chuuban;
    //var rowNo;
    //for (var i = 0; i < chuban.length; i++) {
    //    for (var j = i + 1; j < chuban.length; j++) { // avoid duplicate comparisons
    //        if (chuban[i] === chuban[j] && chuban[i] !== "") {
    //            // check if not already recorded
    //            var exists = duplicates.some(function (d) {
    //                return d.value === chuban[i] && d.rowNo === j + 1;
    //            });
    //            if (!exists) {
    //                duplicates.push({ value: chuban[i], rowNo: j + 1 });
    //            }
    //        }
    //    }
    //}

    //if (duplicates.length > 0) {
    //    for (var k = 0; k < duplicates.length; k++) {
    //        chuuban = duplicates.map(function (d) { return d.value; }).join(", ");
    //        rowNo = duplicates.map(function (d) { return d.rowNo; }).join(", ");

    //    }

    //    var message = $.trim($("#lblErrorMessageE_PostHT_29").text());
    //    showMsgDuplicateChuuban(message, "txtChuuban", WARNING, chuuban, rowNo);
    //    return false;
    //}
    ////////////////////////////////////////////////

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/SearchBychuuban?chuuban=" + chuban1,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(chuban)
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                var jsonresult = response.message;
                var order = jsonresult.filter(o => o.procesS_PLAN_START_DATE != null);
                var chubanList = jsonresult.filter(row => row.heaT_TREATMENT_CD == null);
                var chuban1 = $("#txtOrderNo" + k).val();
                invalidChuuban = jsonresult.filter(r => r.chuubaN_CODE == 2);
                processPlanStartDate = jsonresult.filter(r => r.chuubaN_CODE == 3);
                completedChuuban = jsonresult.filter(r => r.chuubaN_CODE == 4);

                for (var k = 1; k <= emptyRow; k++) {

                    var isDataFound = false;
                    var chuban1 = $("#txtChuuban" + k).val();
                    if (chuban1 > 0) {

                        var isDataExist = "";
                        isDataExist = chubanList.filter(r => r.chuuban == chuban1);
                        if (isDataExist.length > 0) {
                            $.each(jsonresult, function (i, jsonObj) {

                                if (jsonresult[i].chuuban == chuban1 && jsonresult[i].chuubaN_CODE == 1/*&& jsonresult[i].procesS_PLAN_START_DATE != null*/) {

                                    $("#txtChuuban" + k).text(jsonresult[i].chuuban);
                                    $("#txtCustomer" + k).text(jsonresult[i].customeR_NAME);
                                    $("#txtProdName" + k).text(jsonresult[i].proD_NAME_CUST);
                                    $("#txtCharge" + k).text(jsonresult[i].charge);
                                    $("#txtMatlDim" + k).text(jsonresult[i].pP_MATL_DIM_D_STRING);
                                    $("#txtProdDim" + k).text(jsonresult[i].pP_PROD_DIM_D_STRING);
                                    $("#hdfProcessPlanCD" + k).text(jsonresult[i].procesS_PLAN_CD);
                                    $("#hdfInternal_Q_CD").text(jsonresult[i].internaL_QUALITY_CD);
                                    $("#txtPosition11").focus();
                                    enableSaveButtons();
                                    enableDeleteRowButtons();
                                }

                            });
                        } else {
                            // var notInHeatTreatment = chuban1;
                            notFoundChuubanInHT.push(chuban1);
                            isDataFound = true;
                        }

                    }
                    else {
                        clearTableRow("trId" + k);
                    }
                }
            }
            if (invalidChuuban != "") {

                var msg = "";
                for (var i = 0; i < invalidChuuban.length; i++) {
                    if (msg == "") {
                        msg = invalidChuuban[i].chuuban;
                    } else {
                        msg += " <br>" + invalidChuuban[i].chuuban;
                    }
                    if (focusID == "") {
                        focusID = invalidChuuban[i].chuuban;
                    }
                }

                message = $("#lblErrorMessageE_PostHT_09").text().trim() + "<br>" + msg.toString();
            }
            if (completedChuuban != "") {

                var completedChuubanList = completedChuuban.filter(function (val, index, list) {
                    return index === list.indexOf(val);
                });
                var msg1 = "";
                for (var i = 0; i < completedChuuban.length; i++) {
                    if (msg1 == "") {
                        msg1 = completedChuuban[i].chuuban;
                    } else {
                        msg1 += " <br>" + completedChuuban[i].chuuban;
                    }
                    if (focusID1 == "") {
                        focusID1 = completedChuuban[i].chuuban;
                    }
                }

                message = message + "<br>" + $("#lblErrorMessageE_PostHT_10").text().trim() + "<br>" + msg1.toString();
            }

            if (processPlanStartDate != "") {

                var processList = processPlanStartDate.filter(function (val, index, list) {
                    return index === list.indexOf(val);
                });
                var msg2 = "";
                for (var i = 0; i < processPlanStartDate.length; i++) {
                    if (msg2 == "") {
                        msg2 = processPlanStartDate[i].chuuban;
                    } else {
                        msg2 += " <br>" + processPlanStartDate[i].chuuban;
                    }
                    if (focusID2 == "") {
                        focusID2 = processPlanStartDate[i].chuuban;
                    }
                }

                message = message + "<br>" + $("#lblErrorMessageE_PostHT_025").text().trim() + "<br>" + msg2.toString();
            }

            if (notFoundChuubanInHT != "") {

                var msg3 = "";
                for (var i = 0; i < notFoundChuubanInHT.length; i++) {
                    if (msg3 == "") {
                        msg3 = notFoundChuubanInHT[i].chuuban;
                    } else {
                        msg3 += " <br>" + notFoundChuubanInHT[i].chuuban;
                    }
                    if (focusID3 == "") {
                        focusID3 = notFoundChuubanInHT[i].chuuban;
                    }
                }

                message = message + "<br>" + $("#lblErrorMessageE_PostHT_026").text().trim() + "<br>" + msg3.toString();
            }

            if (message != "") {

                showMessageDialog(message, "", WARNING);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    if (isOrderNoExist == false) {

        message = $.trim($("#lblErrorMessageE_PostHT_020").text());
        showMessageDialog(message, "txtOrderNo1", WARNING);
        FillEmptyGrid();
    }
}

$("#btnFurnaceSearch").click(function (event) {
    disableGridCheckBox();
    disableSaveButtons();
    FarnaceSearch();
    setStartSysDateTime();
});

function FarnaceSearch() {

    var furnanceNo = $("#txtFurnaceNum").val();
    if (furnanceNo == "" || furnanceNo == null) {
        message = $.trim($("#lblErrorMessageE_PostHT_028").text());
        showMessageDialog(message, "txtFurnaceNum", WARNING);
    }
    if (furnanceNo.length > 0) {
        /*  setStartOperatorListStartDetails("", "");*/

        var ajaxTime = new Date().getTime();

        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/PostHtInspection/SearchByFurnaceNo?furnanceNo=" + furnanceNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //  data: JSON.stringify(furnanceNo)
        }
        $.ajax(settings)
            .done(function (response) {

                responseTime = new Date().getTime() - ajaxTime;

                var jsonResult = response.message;
                var jsonResult1 = response.validationMsg;

                debugger;
                if (response.isSuccess == true) {

                    clearData();

                    //  $("#txtInspectionDate").val($.format.date(new Date(jsonResult[0].inT_QLTY_TEST_DATE), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
                    // $("#ddlOperatorName").val(jsonResult[0].inT_QLTY_TEST_OPERATOR_CD);

                    $("#txtStartOperator").val(jsonResult[0].iQ_START_OPERATOR_NAME);
                    $("#hdfStartOperatorCD").val(jsonResult[0].inT_QLTY_TEST_OPERATOR_CD);

                    $("#ddlMachine").val(jsonResult[0].machinE_CD);
                    $('#txtFurnaceNo').val(jsonResult[0].furnacE_NO);
                    $("#ddlProcCondition").val(jsonResult[0].hT_CONDITION);
                    $("#txtPatternNo").val(jsonResult[0].patterN_NO);

                    $("#hdfGroupNo").val(jsonResult[0].internaL_QUALITY_GROUP_NO);
                    //grid data
                    $.each(jsonResult, function (key, jsonObj) {

                        key = parseInt(key) + 1;
                        $("#txtChuuban" + key).val(jsonObj.chuuban);
                        $("#txtCustomer" + key).text(jsonObj.customeR_NAME);
                        $("#txtProdName" + key).text(jsonObj.proD_NAME);
                        $("#txtCharge" + key).text(jsonObj.charge);
                        $("#txtMatlDim" + key).text(jsonObj.pP_MATL_DIM_D_STRING);
                        $("#txtProdDim" + key).text(jsonObj.pP_PROD_DIM_D_STRING);

                        //$("#txtPosition1" + key).val(jsonObj.locatioN_IN_FURNACE1);
                        //$("#txtPosition2" + key).val(jsonObj.locatioN_IN_FURNACE2);
                        //$("#txtStackPos" + key).val(jsonObj.stacK_POSITION);
                        //$("#txtOrientation" + key).val(jsonObj.orientation);
                        //$("#txtInOut" + key).val(jsonObj.iN_OUT);
                        //$("#lblDMFDMT" + key).val(jsonObj.dmF_DMF);
                        //$("#txtDMF" + key).val(jsonObj.acT_DM_F);
                        //$("#txtDMTMin" + key).val(jsonObj.acT_DM_T_MIN);
                        //$("#txtDMTMax" + key).val(jsonObj.acT_DM_T_MAX);
                        //$("#txtActDmtAvg" + key).text(jsonObj.acT_DM_T_AVG);
                        //$("#txtStructure" + key).val(jsonObj.acT_STRUCTURE);
                        //$("#txtCenter" + key).val(jsonObj.hrD_CENTER);
                        //$("#txtD1" + key).val(jsonObj.hrD_D4_1);
                        //$("#txtD2" + key).val(jsonObj.hrD_D4_2);
                        //$("#txtD3" + key).val(jsonObj.hrD_D4_3);
                        //$("#txtAvg" + key).text(jsonObj.hrD_D4_AVG);

                        //$("#ddlDecision" + key).val(jsonObj.internaL_QUALITY_DECISION);
                        //// $("#ddlNextProcess" + key).val(jsonObj.iQ_NEXT_PROCESS);

                        $("#hdfProcessPlanCD" + key).text(jsonObj.procesS_PLAN_CD);
                        $("#hdfInternal_Q_CD" + key).text(jsonObj.internaL_QUALITY_CD);
                        $("#chkPostGrid" + key).removeAttr("disabled");
                    });
                    disableEditButtons();
                    disableDeleteButtons();
                    enableSaveButtons();
                    enableDeleteRowButtons();
                    setStartSysDateTime();
                } else {
                    message = $.trim($("#lblErrorMessageE_PostHT_016").text());
                    showMessageDialog(message, "", WARNING);
                    disableSaveButtons();
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {

                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
}

//select
$("#btnSelect").click(function event() {

    location.href = "/PostHTInspection/PostHTInspectionList";
    window.resizeTo(790, 780);
});
//Delete
$("#btnDelete").click(function (event) {
    debugger;
    var ajaxTime = new Date().getDate();
    //const queryString1 = window.location.search;
    var groupNo = parseInt($("#hdfGroupNo").val());
    
    
    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/PostHtInspection/PostHTInspectionCheckMilkSheetData?groupNo=" + groupNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if (jsonResult > 0) {

                var message = $("#valMsgDeleteNotAllowedUsedInMillSheet").text();
                showMessageDialog(message, "btnInitialize", WARNING);
            }
            else {

                deletePostHTDetails();
            }
            

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });



   // deletePostHTDetails();



    /* clear();*/
});

function deletePostHTDetails() {


    var ajaxTime = new Date().getDate();
    //const queryString1 = window.location.search;
    var groupNo = parseInt($("#hdfGroupNo").val());


    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        //dataType: "json",
        url: APIURL + "/PostHtInspection/PostHTInspectionDetailsDelete?groupNo=" + groupNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            clearData();
            var successMessage = $("#lblSuccessMessageS_PostHT_DataDeleted").text();
            showMessageDialog(successMessage, "", SUCCESS);
            //location.href = removeParameter("InternalGroupNo", window.location.href);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function GetValidEntries() {
    var lstChuban = [];
    $('table#tblPostHTInspection tbody tr').each(function (index, item) {
        var $tr = $(this);
        var chuuban = $tr.find("#txtChuuban").val();
        if (chuuban != null) {
            lstChuban.push(chuuban);
        }
    });
    return lstChuban;
}
//bind data grid

function FillGrid(jsonResult, bindGridDD = true) {
    var ajaxTime = new Date().getTime();
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/FillGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)
        //data: "{jsonResult : " + jsonResult + "}"
    }
    $.ajax(settings1)
        .done(function (response1) {

            if (response1.isSuccess) {
                jsonResult = response1.message;
                $("#divPostHTInspectionListGrid").html(jsonResult);

                if (bindGridDD) {
                    FillGridDropDown();
                }
                //setPostHTDetails(jsonResult);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function FillEmptyGrid() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/FillEmptyGrid",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                FillGrid(response.message, false);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}



//Delete Data(row) only on grid view front side.

$("#btnDeleteRow").click(function () {
    isCheckPost = false;
    for (var i = 1; i < emptyRow; i++) {
        if ($("#chkPostGrid" + i).is(':checked')) {
            $("#chkPostGrid" + i).prop('checked', false)
            clearTableRow("trId" + i);
            $("#chkPostGrid" + i).attr("disabled", true);
            isCheckPost = true
        }
    }
    if ($("#chkPostGrid" + i).prop('checked', false) && isCheckPost == false) {
        var message = $("#lblErrorMessageE_PostHT_022").text().trim();
        showMessageDialog(message, "", WARNING);
    }
});

function setProcConditionKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/PostHtInspection/GetProcConditionKubun",
        method: "GET",
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
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

//function bindProcConditionKubunDropdown(jsonResult) {
//    $("#ddlProcCondition").html("").append($('<option value="0"></option>'));
//    $.each(jsonResult, function (key, jsonObj) {
//        $("#ddlProcCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
//    });
//}
function bindProcConditionKubunDropdown(jsonResult) {
    // $("#ddlProcCondition").html("").append($('<option value="0"></option>'));
    $("#listProcCondition").html("").append($('<option value=""></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        // $("#ddlProcCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

        $("#listProcCondition").append($('<option value="' + jsonObj.name + '"</option>'));
    });
}
function setInOutKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/PostHtInspection/GetInOutKubun",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            bindInOutKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindInOutKubunDropdown(jsonResult) {
    for (var i = 1; i <= emptyRow; i++) {
        $('table#tblPostHTInspection tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $txtInOut = $tr.find("#txtInOut" + i);
            $txtInOut.html("").append($('<option value="0"></option>'));
            $.each(jsonResult, function (key, jsonObj) {
                $txtInOut.append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            });
        });
    }
}

function setOrientationKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/PostHtInspection/GetOrientationKubun",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            bindOrientationKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindOrientationKubunDropdown(jsonResult) {
    for (var i = 1; i <= emptyRow; i++) {
        $('table#tblPostHTInspection tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $Orientation = $tr.find("#txtOrientation" + i);
            $Orientation.html("").append($('<option value="0"></option>'));
            $.each(jsonResult, function (key, jsonObj) {
                $Orientation.append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
            });
        });
    }
}

document.querySelector("#tblPostHTInspection tbody").addEventListener("change", function (event) {
    debugger;
    const row = event.target.closest("tr");
    var $val = row.id;
    const input = $val;
    const [, letters, number] = input.match(/^([a-zA-Z]+)(\d+)$/) || [];
    var txtval = ('#txtChuuban' + number)
    let nextVal = parseInt(number) + 1;
    let barcode = $(txtval).val();

    //barcode = barcode.replace(/\D/g, ''); // Keep only digits
    //$(txtval).val(barcode);
    //var txtval = ('#txtChuuban' + nextVal)

    //$(txtval).focus();


    //const elementOrderNo = document.getElementById(txtval).val();

    //    if (elementOrderNo) {
    //        elementOrderNo.addEventListener("change", onOrderNoInputChange(number));
    //    }




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
                url: APIURL + "/PostHtInspection/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
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
                            $("#txtChuuban" + (parseInt(txtID) + 1)).focus();
                        }

                    } else {
                        e.target.value = "";
                    }

                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_PostHTInspection").text().trim();
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        }
    }


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
