var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var operatorCD;
var ProcessCD;
var machineCD;
var validationMessage = "";
var message = "";
var Thaba1;
var Thaba2;
var Thaba3;
var Thaba4;
var returnVal;
var defaultDateValidation = 10;
var characterVal = false;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var tabaArray = [];
$(document).ready(function () {
    setMandatoryCssUnitType();
    setMandatoryCssNoOfUnit();
    setFocus();
    SetDecimalFormats();
    RestrictToDigitsOnly();
});
function setFocus() {
    document.getElementById("chubbanNo1").focus();
}
$('#txtOkUnits, #txtDefect1, #txtDefect2, #txtDefect3,#txtWeight,#txtWeight1, #txtWeight2, #txtWeight3').on('input', function () {
    let v = $(this).val().replace(/,/g, '.');
    const parts = v.split('.');
    parts[0] = parts[0].replace(/\D+/g, '');
    if (parts.length > 1) {
        parts[1] = parts.slice(1).join('').replace(/\D+/g, '');
        v = parts[0] + '.' + parts[1];
    } else {
        v = parts[0];
    }
    $(this).val(v);
});

$('#txtOkUnits, #txtDefect1, #txtDefect2, #txtDefect3,#txtWeight,#txtWeight1, #txtWeight2, #txtWeight3').on('paste', function (e) {
    e.preventDefault();
    const text = (e.originalEvent.clipboardData || window.clipboardData).getData('text');
    $(this).val(text.replace(/,/g, '.').replace(/[^0-9.]/g, '').replace(/\.(?=.*\.)/g, ''));
});
function SetDecimalFormats() {

    $("#txtOkUnits, #txtDefect1, #txtDefect2, #txtDefect3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtWeight, #txtWeight1, #txtWeight2, #txtWeight3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
}
function RestrictToDigitsOnly() {
    $("#bundleNo1,#bundleNo2,#bundleNo3,#bundleNo4").on('input', function () {
        // Replace all non-digit characters
        this.value = this.value.replace(/[^0-9]/g, '');
    });

}

$("#btnMenu").click(function (event) {
    self.close();
});
$("#chubbanNo1").change(function () {
    var orderNo = $('#chubbanNo1').val() ? $('#chubbanNo1').val() : null;

    //var keycode = (e.keyCode ? e.keyCode : e.which);
    if (orderNo) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/GeneralDailyProcess/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $("#chubbanNo1").val(jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN);
                    //$("#bundleNo1").focus();
                    var orderNo = $('#chubbanNo1').val() ? $('#chubbanNo1').val() : null;
                    if (SPLCHUUBANGYOUBAN == orderNo) {
                        if (orderNo) {
                            orderNo = orderNo.slice(0, -2);
                            var ajaxTime = new Date().getTime();
                            var settings = {
                                async: false,
                                crossDomain: true,
                                cache: false,
                                dataType: "json",
                                url: APIURL + "/GeneralDailyProcess/GetNextThabaForSpecialChuubanNo?orderNo=" + orderNo,
                                method: "POST",
                                contentType: "application/json; charset=utf-8",

                            }
                            $.ajax(settings)
                                .done(function (response) {
                                    responseTime = new Date().getTime() - ajaxTime;
                                    $("#bundleNo1").val(response.data);
                                })
                                .fail(function (xhr, textStatus, errorThrown) {
                                    responseTime = new Date().getTime() - ajaxTime;
                                    var errorCode = $.parseJSON(xhr.responseText).Message;

                                    focusFieldID = $(':focus').attr("id");
                                    var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
                                    showMessageDialog(message, focusFieldID, ERROR);
                                    return false;
                                });
                        }
                    }
                    var orderNo = $('#chubbanNo1').val();
                    if (SPLCHUUBANGYOUBAN == orderNo) {
                        $('#bundleNo2').prop("disabled", true);
                        $('#bundleNo3').prop("disabled", true);
                        $('#bundleNo4').prop("disabled", true);
                    }
                    setDecision();
                    setUnitType();
                } else {
                    $("#chubbanNo1").val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }



});
$("#processCD").change(function () {
    var processCD = $('#processCD').val() ? $('#processCD').val() : null;
    processCD = processCD.slice(3);
    var orderNo = $('#chubbanNo1').val();
    if (SPLCHUUBANGYOUBAN != orderNo) {
        if (processCD) {
            var ajaxTime = new Date().getTime();
            var settings = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/GeneralDailyProcess/ValidateGeneralProcess?PROCESS_KUBUN_CD=" + processCD,
                method: "POST",
                contentType: "application/json; charset=utf-8",

            }
            $.ajax(settings)
                .done(function (response) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var jsonResult = response.generalProcess;
                    if (response.generalProcess == false) {
                        focusFieldID = $(':focus').attr("id");
                        message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_035").text().trim();
                        showMessageDialog(message, focusFieldID, WARNING);
                        $('#processCD').val("")
                    }
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text().trim();
                    showMessageDialog(message, focusFieldID, ERROR);
                    return false;
                });
        }
    }

});
$("#btnNextFirstForm").click(function (event) {
    validationMessage = "";
    message = "";
    debugger;
    var arrBundle;
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    var inputElement = $('#chubbanNo1').val().toUpperCase();
    if ($("#chubbanNo1").val() == "") {
        validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_002").text());
    }
    //if (Thaba1 == "") {
    //    if (validationMessage != "") {
    //        validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_003").text());
    //    }
    //    else {
    //        validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_003").text());
    //    }
    //}
    if (validationMessage != "") {
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(validationMessage, focusFieldID, WARNING);
    }
    else if (validationMessage == "") {
        if (Thaba1 != null) {
            arrBundle = [parseInt(Thaba1)];
        }
        if (Thaba2 != null) {
            arrBundle = [parseInt(Thaba1), parseInt(Thaba2)];
        }
        if (Thaba3 != null) {
            arrBundle = [parseInt(Thaba1), parseInt(Thaba2), parseInt(Thaba3)];
        }
        if (Thaba4 != null) {
            arrBundle = [parseInt(Thaba1), parseInt(Thaba2), parseInt(Thaba3), parseInt(Thaba4)];
        }
        var bundleList = "";
        if (Thaba1 != null || Thaba2 != null || Thaba3 != null || Thaba4 != null) {
            var duplicateBundle = arrBundle.filter((bundleNo, bundleIndex) =>
                arrBundle.indexOf(bundleNo) !== bundleIndex);
            for (var i = 0; i <= duplicateBundle.length - 1; i++) {
                var firstindex = 1 + arrBundle.indexOf(duplicateBundle[i]);
                var Lastindex = 1 + arrBundle.lastIndexOf(duplicateBundle[i]);
                if (firstindex == 1) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_029").text());
                }
                else if (firstindex == 2) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_030").text());
                }
                else if (firstindex == 3) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_031").text());
                }
                else if (firstindex == 4) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_032").text());
                }
                if (Lastindex == 1) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_029").text());
                }
                else if (Lastindex == 2) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_030").text());
                }
                else if (Lastindex == 3) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_031").text());
                }
                else if (Lastindex == 4) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_032").text());
                }
                if (bundleList == "") {
                    if (firstindex != 0)
                        bundleList = firstindex + "・" + Lastindex
                }
                else {
                    bundleList += "・" + firstindex + "・" + Lastindex
                }

            }
            if (bundleList != "") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_005").text());
                message = message.replace("①・②・③・④", bundleList);
                showMessageDialog(message, focusFieldID, WARNING);
            }
        }
        if (message == "") {
            if (Thaba1 == null  && Thaba2 == null && Thaba3 == null && Thaba4 == null) {
                if (!ValidateChubban()) {
                    return false;
                }
                else {
                    $(startEndSecondForm).show();
                    $(startEndFirstForm).hide();
                    setStartSysDateTime();
                    var orderNo = $('#chubbanNo1').val();
                    if (SPLCHUUBANGYOUBAN == orderNo) {
                        document.getElementById("metalicPlateNo").style.backgroundColor = "#fff";
                        $("#metalicPlateNo").val("");
                        $('#checkBoxForConfirmation').prop("disabled", true);
                    }
                }
            }
            else {
                if (!clickBtnNextFirstFormForValidation()) {
                    return false;
                }
                else {
                    $(startEndSecondForm).show();
                    $(startEndFirstForm).hide();
                    setStartSysDateTime();
                    var orderNo = $('#chubbanNo1').val();
                    if (SPLCHUUBANGYOUBAN == orderNo) {
                        document.getElementById("metalicPlateNo").style.backgroundColor = "#fff";
                        $("#metalicPlateNo").val("");
                        $('#checkBoxForConfirmation').prop("disabled", true);
                    }
                }
            }



        }
    }
});
function ValidateChubban() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/ValidateChubbanForProcessStart?PP_CHUUBAN=" + $("#chubbanNo1").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var data = response.data;

            if (data.responce == "Chubban is Not Found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_006").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;
                $(startEndSecondForm).hide();
                $(startEndFirstForm).show();
            }
            else if (data.responce == "Chubban is already completed") {
                $(startEndFirstForm).show();
                $(startEndSecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_008").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else if (data.responce == "Process is already Started for this chubban") {
                $(startEndFirstForm).show();
                $(startEndSecondForm).hide();

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgDuplicateGenProcessWithoutTaba").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else {
                //$("#metalicPlateNo").val(data.metaliC_PLATE_NO);
                $("#chargrNo").val(data.pP_CHARGE);
                $("#chubbanNo2").val($("#chubbanNo1").val());
                $("#bundleNo21").val("");
                $("#bundleNo22").val("");
                $("#bundleNo23").val("");
                $("#bundleNo24").val("");
                returnVal = true;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnVal;

}
function clickBtnNextFirstFormForValidation() {
    var tabaList = [];
    var outputTabaList = [];
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    //tabaList.push(parseInt(Thaba1));
    if (Thaba1 != null) {
        tabaList.push(parseInt(Thaba1));
    }
    if (Thaba2 != null) {
        tabaList.push(parseInt(Thaba2));
    }
    if (Thaba3 != null) {
        tabaList.push(parseInt(Thaba3));
    }
    if (Thaba4 != null) {
        tabaList.push(parseInt(Thaba4));
    }
    var ajaxTime = new Date().getTime();
    var jsonPlanData = ({
        PP_CHUUBAN: $("#chubbanNo1").val(),
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/CheckValidationsOnGeneralProcessStart",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var data = response.data;
            if (data.gP_TABA_NO_1 != null) {
                outputTabaList.push(data.gP_TABA_NO_1);
            }
            if (data.gP_TABA_NO_2 != null) {
                outputTabaList.push(data.gP_TABA_NO_2);
            }
            if (data.gP_TABA_NO_3 != null) {
                outputTabaList.push(data.gP_TABA_NO_3);
            }
            if (data.gP_TABA_NO_4 != null) {
                outputTabaList.push(data.gP_TABA_NO_4);
            }
            console.log(outputTabaList);
            var result = outputTabaList.filter(function (a) {
                return tabaList.indexOf(a) !== -1;
            });
            if (data.responce == "Chubban is Not Found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_006").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;
                $(startEndSecondForm).hide();
                $(startEndFirstForm).show();
            }
            else if (data.responce == "Chubban is already completed") {
                $(startEndFirstForm).show();
                $(startEndSecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_008").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else if (data.responce == "Process is already started for this bundle no") {
                $(startEndFirstForm).show();
                $(startEndSecondForm).hide();

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_007").text().replace('NNNN,NNN,NNN,NNN', result));
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else {
                $("#metalicPlateNo").val(data.metaliC_PLATE_NO);
                $("#chargrNo").val(data.pP_CHARGE);
                $("#chubbanNo2").val($("#chubbanNo1").val());
                $("#bundleNo21").val($("#bundleNo1").val());
                $("#bundleNo22").val($("#bundleNo2").val());
                $("#bundleNo23").val($("#bundleNo3").val());
                $("#bundleNo24").val($("#bundleNo4").val());
                returnVal = true;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnVal;

}
$("#btnBackSecondForm").click(function (event) {
    $(startEndSecondForm).hide();
    $(startEndFirstForm).show();
});
$("#btnNextSecondForm").click(function (event) {
    validationMessage = "";
    operatorCD = $("#operatorCD").val().toUpperCase();
    ProcessCD = $("#processCD").val().toUpperCase();
    machineCD = $("#machineCD").val().toUpperCase();
    if (operatorCD == "") {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_012").text());
        validationMessage = message;
    }
    if (ProcessCD == "") {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_013").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_013").text());
        }
    }
    var orderNo = $('#chubbanNo1').val();
    if (SPLCHUUBANGYOUBAN != orderNo) {
        if ($("#metalicPlateNo").val() == "") {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_014").text());
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_014").text());
            }
        }
    }
    if ($("#txtStartDateTime").val() == "") {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_010").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_010").text());
        }
    }
    if (operatorCD != "") {
        if (!operatorCD.match("OPR")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblOperator").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblOperator").text()));
            }
        }
    }
    if (ProcessCD != "") {
        if (!ProcessCD.match("PCS")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblProcess").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblProcess").text()));
            }
        }
    }

    if (machineCD != "") {
        if (!machineCD.match("MAC")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblMachine").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblMachine").text()));
            }
        }
    }
    if ($("#txtStartDateTime").val() != "") {
        if (!IsDateValid()) {
            return false;
        }
    }
    if (validationMessage != "") {
        showMessageDialog(validationMessage, focusFieldID, WARNING);
    }
    else {
        if (!checkBarcodeCDValidationAndTabaOkDecision()) {
            return false;
        }
        else {
            $(startEndThirdForm).show();
            $(startEndSecondForm).hide();
        }
    }

});

function checkBarcodeCDValidationAndTabaOkDecision() {
    var ajaxTime = new Date().getTime();
    PP_CHUUBAN = $("#chubbanNo1").val();
    operatorCD = $("#operatorCD").val().slice(3);
    ProcessCD = $("#processCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    var jsonPlanData = ({
        GP_START_OPERATOR_CD: operatorCD,
        PROCESS_KUBUN_CD: ProcessCD,
        MACHINE_CD: machineCD,
        PP_CHUUBAN: PP_CHUUBAN,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/CheckValidationsForProcessOperatorMachineCDOnEntry",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var jsonresult = response.data;
            if (jsonresult.responce == "Operator is not found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_016").text().replace("*****", $("#lblOperator").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                $(startEndSecondForm).show();
                $(startEndThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Operator is Inactive") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_033").text());
                showMessageDialog(message, focusFieldID, WARNING);
                $(startEndSecondForm).show();
                $(startEndThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Process is not found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_016").text().replace("*****", $("#lblProcess").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                $(startEndSecondForm).show();
                $(startEndThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Machine is not found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_016").text().replace("*****", $("#lblMachine").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                $(startEndSecondForm).show();
                $(startEndThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Machine is Inactive") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_034").text());
                showMessageDialog(message, focusFieldID, WARNING);
                $(startEndSecondForm).show();
                $(startEndThirdForm).hide();
                returnVal = false;
            }
            else {
                var orderNo = $('#chubbanNo1').val();
                if (SPLCHUUBANGYOUBAN != orderNo && $("#checkBoxForConfirmation").prop("checked") == false) {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_015").text());
                    showMessageDialog(message, focusFieldID, WARNING);
                    $(startEndSecondForm).show();
                    $(startEndThirdForm).hide();
                    returnVal = false;
                }
                else {
                    console.log(jsonresult);
                    $("#chubbanNo3").val($("#chubbanNo2").val());
                    $("#bundleNo31").val($("#bundleNo1").val());
                    $("#bundleNo32").val($("#bundleNo2").val());
                    $("#bundleNo33").val($("#bundleNo3").val());
                    $("#bundleNo34").val($("#bundleNo4").val());
                    $("#txtStartDateTime2").val($("#txtStartDateTime").val());
                    $("#txtOperator").val(jsonresult.pP_EMPLOYEE_NAME);
                    $("#txtProcess").val(jsonresult.gP_START_PROCESS_NAME);
                    $("#txtMachine").val(jsonresult.gP_MACHINE_NAME);
                    $("#txtCustomerName").val(jsonresult.pP_CUSTOMER_NAME);
                    $("#txtProdName").val(jsonresult.pP_PROD_NAME_CUST);
                    if (jsonresult.pP_PROD_DIM_D != null && jsonresult.pP_PROD_DIM_W != null) {
                        $("#txtDim").val(jsonresult.pP_PROD_DIM_D + "*" + jsonresult.pP_PROD_DIM_W);
                    }
                    else if (jsonresult.pP_PROD_DIM_D == null) {
                        $("#txtDim").val(jsonresult.pP_PROD_DIM_W);
                    }
                    else if (jsonresult.pP_PROD_DIM_W == null) {
                        $("#txtDim").val(jsonresult.pP_PROD_DIM_D);
                    }
                    //$("#txtDim").val(jsonresult.pP_PROD_DIM_D + "*" + jsonresult.pP_PROD_DIM_W);
                    $("#txtShape").val(jsonresult.pP_PROD_SHAPE);
                    $("#txtDimOther").val(jsonresult.pP_PROD_DIM_OTHER);
                    if (jsonresult.pP_DIM_TOL_MIN_CHR != null && jsonresult.pP_DIM_TOL_MAX_CHR != null) {
                        $("#txtDimTolMinMaxChr").val(jsonresult.pP_DIM_TOL_MAX_CHR + "," + jsonresult.pP_DIM_TOL_MIN_CHR);
                    }
                    else if (jsonresult.pP_DIM_TOL_MIN_CHR == null) {
                        $("#txtDimTolMinMaxChr").val(jsonresult.pP_DIM_TOL_MAX_CHR);
                    }
                    else if (jsonresult.pP_DIM_TOL_MAX_CHR == null) {
                        $("#txtDimTolMinMaxChr").val(jsonresult.pP_DIM_TOL_MIN_CHR);
                    }
                    $("#txtLength").val(jsonresult.length);
                    if (jsonresult.pP_LENGTH_TOL_MIN_CHR != null && jsonresult.pP_LENGTH_TOL_MAX_CHR != null) {
                        $("#txtLengthTolMinMaxChr").val(jsonresult.pP_LENGTH_TOL_MAX_CHR + "," + jsonresult.pP_LENGTH_TOL_MIN_CHR);
                    }
                    else if (jsonresult.pP_LENGTH_TOL_MIN_CHR == null) {
                        $("#txtLengthTolMinMaxChr").val(jsonresult.pP_LENGTH_TOL_MAX_CHR);
                    }
                    else if (jsonresult.pP_LENGTH_TOL_MAX_CHR == null) {
                        $("#txtLengthTolMinMaxChr").val(jsonresult.pP_LENGTH_TOL_MIN_CHR);
                    }
                    $("#hdProcessPlanCD").val(jsonresult.pP_PROCESS_PLAN_CD);
                    returnVal = true;

                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
function clickTabaValidationForOkDecision() {
    var tabaValue = [];
    var outputTabaValue = [];
    //Thaba1 = $("#bundleNo1").val();
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    ProcessCD = $("#processCD").val().slice(3);
    var ajaxTime = new Date().getTime();
    var jsonPlanData = ({
        PP_CHUUBAN: $("#chubbanNo1").val(),
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
        PROCESS_KUBUN_CD: ProcessCD,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/checkTabaValidationForOkDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var data = response.data;
            if (data != null) {
                if (data.gP_TABA_NO_1 != null) {
                    outputTabaValue.push(data.gP_TABA_NO_1);
                }
                if (data.gP_TABA_NO_2 != null) {
                    outputTabaValue.push(data.gP_TABA_NO_2);
                }
                if (data.gP_TABA_NO_3 != null) {
                    outputTabaValue.push(data.gP_TABA_NO_3);
                }
                if (data.gP_TABA_NO_4 != null) {
                    outputTabaValue.push(data.gP_TABA_NO_4);
                }
                console.log(outputTabaValue);
                for (i = 0; i < outputTabaValue.length; i++) {
                    if (outputTabaValue[i] == Thaba1) {
                        if (tabaValue == "") {
                            tabaValue = Thaba1;
                        }
                        else {
                            tabaValue = tabaValue + ',' + Thaba1;
                        }
                    }
                    else if (outputTabaValue[i] == Thaba2) {
                        if (tabaValue == "") {
                            tabaValue = Thaba2;
                        }
                        else {
                            tabaValue = tabaValue + ',' + Thaba2;
                        }
                    }
                    else if (outputTabaValue[i] == Thaba3) {
                        if (tabaValue == "") {
                            tabaValue = Thaba3;
                        }
                        else {
                            tabaValue = tabaValue + ',' + Thaba3;
                        }
                    }
                    else if (outputTabaValue[i] == Thaba4) {
                        if (tabaValue == "") {
                            tabaValue = Thaba4;
                        }
                        else {
                            tabaValue = tabaValue + ',' + Thaba4;
                        }
                    }
                }

                if (data.responce == "Process is  started with ok decision for this bundle no") {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_004").text().replace('NNNN,NNN,NNN,NNN', tabaValue));
                    showMessageDialog(message, focusFieldID, WARNING);
                    $(startEndSecondForm).show();
                    $(startEndThirdForm).hide();
                    returnVal = false;
                }
            }
            else {
                returnVal = true;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnVal;

}
$("#btnBackThirdForm").click(function (event) {
    $(startEndSecondForm).show();
    $(startEndThirdForm).hide();
});
$("#btnNextThirdForm").click(function (event) {
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    $("#chubbanNo4").val($("#chubbanNo1").val());
    $("#bundleNo41").val(Thaba1);
    $("#bundleNo42").val(Thaba2);
    $("#bundleNo43").val(Thaba3);
    $("#bundleNo44").val(Thaba4);
    $("#txtProcessEndDateTime").val($("#txtStartDateTime").val());
    setNextProcessList();
    setDefectList();
    getProcessNameOnMergeScreen();
    $(startEndFourthForm).show();
    $(startEndThirdForm).hide();
    var orderNo = $('#chubbanNo1').val();
    if (SPLCHUUBANGYOUBAN != orderNo) {
        $('#chkFinishedWeight').prop("checked", true);
        $("#ddlUnit").addClass("Mandatory");
        $("#txtOkUnits").addClass("Mandatory");
        //if (Thaba1 != null || Thaba2 != null || Thaba3 != null || Thaba4 != null) {
        //    GetUnitWeight();
        //}
    }
    else {
        document.getElementById("txtWeight").style.backgroundColor = "#fff";
        $('#chkFinishedWeight').prop("disabled", true);
    }
});
function GetUnitWeight(){
     tabaArray = [];
    var resouceTabaList = [];
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    //tabaArray.push(parseInt(Thaba1));
    if (Thaba1 != null) {
        tabaArray.push(parseInt(Thaba1));
    }
    if (Thaba2 != null) {
        tabaArray.push(parseInt(Thaba2));
    }
    if (Thaba3 != null) {
        tabaArray.push(parseInt(Thaba3));
    }
    if (Thaba4 != null) {
        tabaArray.push(parseInt(Thaba4));
    }
    tabaArray.sort(function (a, b) {
        return parseInt(a) - parseInt(b);
    });

    console.log(tabaArray);

    if (tabaArray.length > 0) {
        Thaba1 = tabaArray[0];
    }
    else {
        Thaba1 = null;
    }
    if (tabaArray.length > 1) {
        Thaba2 = tabaArray[1];
    }
    else {
        Thaba2 = null;
    }
    if (tabaArray.length > 2) {
        Thaba3 = tabaArray[2];
    }
    else {
        Thaba3 = null;
    }
    if (tabaArray.length > 3) {
        Thaba4 = tabaArray[3];
    }
    else {
        Thaba4 = null;
    }
    setDefectList();
    setUnitType();
    var ajaxTime = new Date().getTime();
    var jsonPlanData = ({
        PP_CHUUBAN: $("#chubbanNo1").val(),
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetUnitsWeightOnGeneralProcessEnd",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var data = response.data;
            if (data == null) {
                $("#txtOkUnits").val("");
                $("#txtWeight").val("");
                $("#startProcessName").text("");
            }
            else {
                $("#txtOkUnits").val(data.pP_NO_OF_UNITS);
                $("#txtWeight").val(data.pP_ACT_ALLOC_QTY);
                $("#ddlUnit").val(data.uniT_TYPE);
            }

            //$("#chubbanNo4").val($("#chubbanNo1").val());
            //$("#bundleNo41").val($("#bundleNo1").val());
            //$("#bundleNo42").val($("#bundleNo2").val());
            //$("#bundleNo43").val($("#bundleNo3").val());
            //$("#bundleNo44").val($("#bundleNo4").val());
            //$("#txtProcessEndDateTime").val($("#txtStartDateTime").val());
            //setNextProcessList();
            //setDecision();
            //getProcessNameOnMergeScreen();
            //returnVal = true;
            //}
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    //return returnVal;

}
$("#btnBackFourthForm").click(function (event) {
    $(startEndThirdForm).show();
    $(startEndFourthForm).hide();
});
$("#btnNextFourthForm").click(function (event) {
    validationMessage = "";
    var orderNo = $('#chubbanNo1').val();
    operatorCD = $("#operatorCD").val().toUpperCase();
    if (operatorCD == "") {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_012").text());
        validationMessage = message;
    }
    if ($("#txtProcessEndDateTime").val() == "") {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_011").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_011").text());
        }
    }
    if (SPLCHUUBANGYOUBAN != orderNo) {
        if ($("#txtWeight").val() == "") {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_021").text());
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_021").text());
            }
        }
    }
    if (operatorCD != "") {
        if (!operatorCD.match("OPR")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblOperator").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_017").text().replace("*****", $("#lblOperator").text()));
            }
        }
    }
    if ($("#txtDefect1").val() != '0') {
        if ($("#ddlDefect1").val() != 0) {
            if ($("#txtDefect1").val() == "" || $("#txtWeight1").val() == "" || $("#ddlUnit1").val() == 0) {
                focusFieldID = $(':focus').attr("id");
                if (validationMessage != "") {
                    validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                }
                else {
                    validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                }
            }
        }
        else if ($("#txtDefect1").val() != "" || $("#txtWeight1").val() != "" || $("#ddlUnit1").val() != 0) {
            if ($("#ddlDefect1").val() == 0) {
                focusFieldID = $(':focus').attr("id");
                if (validationMessage != "") {
                    validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text());
                }
                else {
                    validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text());
                }
            }
        }
    }
    if ($("#txtDefect2").val() != '0') {
        if ($("#ddlDefect2").val() != 0) {
            if ($("#txtDefect2").val() == "" || $("#txtWeight2").val() == "" || $("#ddlUnit2").val() == 0) {
                focusFieldID = $(':focus').attr("id");
                if (validationMessage != "") {
                    validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                }
                else {
                    validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                }
            }
        }
        else if ($("#txtDefect2").val() != "" || $("#txtWeight2").val() != "" || $("#ddlUnit2").val() != 0) {
            if ($("#ddlDefect2").val() == 0) {
                focusFieldID = $(':focus').attr("id");
                if (validationMessage != "") {
                    validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text());
                }
                else {
                    validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text());
                }
            }
        }
    }
    if ($("#txtDefect3").val() != '0') {
        if ($("#ddlDefect3").val() != 0) {
            if ($("#txtDefect3").val() == "" || $("#txtWeight3").val() == "" || $("#ddlUnit3").val() == 0) {
                focusFieldID = $(':focus').attr("id");
                if (validationMessage != "") {
                    validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                }
                else {
                    validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                }
            }
        }
        else if ($("#txtDefect3").val() != "" || $("#txtWeight3").val() != "" || $("#ddlUnit3").val() != 0) {
            if ($("#ddlDefect3").val() == 0) {
                focusFieldID = $(':focus').attr("id");
                if (validationMessage != "") {
                    validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text());
                }
                else {
                    validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text());
                }
            }
        }
    }
    /* if (SPLCHUUBANGYOUBAN != orderNo) {*/
    if ($("#ddlUnit").val() != 0) {
        if ($("#txtOkUnits").val() == "") {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_028").text());
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_028").text());
            }
        }
    }
    /*}*/
    /* if (SPLCHUUBANGYOUBAN != orderNo) {*/
    if ($("#txtOkUnits").val() != "") {
        if ($("#ddlUnit").val() == 0) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_027").text());
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_027").text());
            }
        }
    }
    /* }*/
    if ($("#chkFinishedWeight").prop("checked") == true) {
        if ($("#ddlUnit").val() == 0 && $("#txtOkUnits").val() == "") {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_028").text()) + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_027").text());
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_028").text()) + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_027").text());
            }

        }
    }
    if ($("#ddlDecision").val() == 0) {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_023").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_023").text());
        }
    }
    if ($("#ddlNextProcess").val() == 0) {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_022").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_022").text());
        }
    }

    if (validationMessage != "") {
        showMessageDialog(validationMessage, focusFieldID, WARNING);
    } else {
        if ($("#txtProcessEndDateTime").val() != "") {
            if (!IsDateValid()) {
                return false;
            }
        }
        var orderNo = $('#chubbanNo1').val();
        if (SPLCHUUBANGYOUBAN != orderNo) {
            if (!CheckExistingData()) {
                return false;
            }
        }
        //if (SPLCHUUBANGYOUBAN != orderNo) {
        //    if (!checkFinishedQty()) {
        //        return false;
        //    }
        //}
        if (SPLCHUUBANGYOUBAN == orderNo) {
            if (!GetProcessPlanData()) {
                return false;
            }
            else {
                $(startEndFifthForm).show();
                $(startEndFourthForm).hide();
            }
        }
       
    }


});
function CheckExistingData() {
    var ajaxTime = new Date().getTime();
    var orderNo = $('#chubbanNo1').val();
    ProcessCD = $("#processCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    tabaArray = [];
    //tabaArray.push(parseInt(Thaba1));
    if (Thaba1 != null) {
        tabaArray.push(parseInt(Thaba1));
    }
    if (Thaba2 != null) {
        tabaArray.push(parseInt(Thaba2));
    }
    if (Thaba3 != null) {
        tabaArray.push(parseInt(Thaba3));
    }
    if (Thaba4 != null) {
        tabaArray.push(parseInt(Thaba4));
    }
    tabaArray.sort(function (a, b) {
        return parseInt(a) - parseInt(b);
    });

    console.log(tabaArray);

    if (tabaArray.length > 0) {
        Thaba1 = tabaArray[0];
    }
    else {
        Thaba1 = null;
    }
    if (tabaArray.length > 1) {
        Thaba2 = tabaArray[1];
    }
    else {
        Thaba2 = null;
    }
    if (tabaArray.length > 2) {
        Thaba3 = tabaArray[2];
    }
    else {
        Thaba3 = null;
    }
    if (tabaArray.length > 3) {
        Thaba4 = tabaArray[3];
    }
    else {
        Thaba4 = null;
    }
    var jsonPlanData = ({
        PP_CHUUBAN: $("#chubbanNo1").val(),
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
        PROCESS_KUBUN_CD: ProcessCD,
        MACHINE_CD: machineCD
    });
    returnVal = true;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/CheckExistingDataValidation",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var jsonresult = response.data;
            if (jsonresult == "Process already exist for this chuuban with Ok decision") {
                var message = "";

                message = $.trim($("#valMsgGeneralDailyDataExistWIthOk").text());

                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
                            bootbox.hideAll();
                            if (SPLCHUUBANGYOUBAN != orderNo) {
                                if (!checkFinishedQty()) {
                                    return false;
                                }
                            }
                            if (!GetProcessPlanData()) {
                                return false;
                            }
                            else {
                                $(startEndFifthForm).show();
                                $(startEndFourthForm).hide();
                            }
                        }
                        else {
                            returnVal = false;
                        }

                    },
                    centerVertical: true
                });
                returnVal = false;

            }
            else {
               
                    if (SPLCHUUBANGYOUBAN != orderNo) {
                        if (!checkFinishedQty()) {
                            return false;
                        }
                    }
                    if (!GetProcessPlanData()) {
                        return false;
                    }
                    else {
                        $(startEndFifthForm).show();
                        $(startEndFourthForm).hide();
                    }
               
            }
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
function checkFinishedQty() {
    var ajaxTime = new Date().getTime();
    var PP_CHUUBAN = $("#chubbanNo1").val();
    returnVal = true;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetChubbanDetailsForWeight?PP_CHUUBAN=" + PP_CHUUBAN,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var jsonresult = response.data;

            // Check all confirmation conditions
            // If +-2% checkbox is not checked and Finished Qty is not in 2% Range, show confirmation message

            var weightValues = {};
            weightValues["PRODUCT_WEIGHT"] = jsonresult.pP_ACT_ALLOC_QTY ? jsonresult.pP_ACT_ALLOC_QTY : null;
            weightValues["INPUT_WEIGHT"] = $("#txtWeight").val() ? $("#txtWeight").val() : null;
            weightValues["PRODUCT_SHAPE"] = jsonresult.pP_PROD_SHAPE ? jsonresult.pP_PROD_SHAPE : null;
            weightValues["PRODUCT_DIM"] = jsonresult.pP_PROD_DIM_D ? jsonresult.pP_PROD_DIM_D : null;
            weightValues["NO_OF_UNITS"] = $("#txtOkUnits").val() ? $("#txtOkUnits").val() : null;
            weightValues["PRODUCT_LENGTH"] = jsonresult.length ? jsonresult.length : null;
            if ($("#chkFinishedWeight").prop("checked") == true) {
                debugger;
                if ([
                    "PRODUCT_SHAPE",
                    "PRODUCT_DIM",
                    "PRODUCT_LENGTH"
                ].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== "")) {

                    var value = jsonresult.length;
                    // Check - Length only allow numeric &  Unit Type - Not allow BN 

                    if (!IsNumeric(value) || value.trim() == "" || $("#ddlUnit").val() == 2) {

                        var message = $("#lblErrorMessageE_WeightCanNotValidate").text().trim();
                        showMessageDialog(message, "chkFinishedWeight", WARNING);
                        returnVal = false;
                        return false;
                    }

                }
                else {
                    var message = $("#lblErrorMessageE_WeightCanNotValidate").text().trim();
                    showMessageDialog(message, "chkFinishedWeight", WARNING);
                    returnVal = false;
                    return false;
                }
            }
            var format = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            var getResponse = {};
            debugger;
            if (!format.test(jsonresult.length) && $("#ddlUnit").val() != 2) {
                getResponse = IsFinishedQuantityValid(weightValues);
            } else {
                getResponse["IsSuccess"] = true;
            }
            if (getResponse.IsSuccess == false && $("#chkFinishedWeight").prop("checked") == true) {
                var message = "";

                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_037").text());
                showMessageDialog(message, "txtFinishedQuantity", WARNING);
                returnVal = false;
                return false;
            }
            else if (($("#chkFinishedWeight").prop("checked") == false) && getResponse.IsSuccess == false) {
                var message = "";

                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_036").text());

                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {

                        if (result) {
                            if (!GetProcessPlanData()) {
                                return false;
                            }
                            else {
                                $(startEndFifthForm).show();
                                $(startEndFourthForm).hide();
                            }
                        }
                        else {
                            returnVal = false;
                        }

                    },
                    centerVertical: true
                });
                returnVal = false;

            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
function IsNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
    return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
}
function GetProcessPlanData() {
    var ajaxTime = new Date().getTime();
    PP_CHUUBAN = $("#chubbanNo1").val();
    operatorCD = $("#operatorCD").val().slice(3);
    var jsonPlanData = ({
        GP_START_OPERATOR_CD: operatorCD,
        PP_CHUUBAN: PP_CHUUBAN,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetProcessPlanData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var jsonresult = response.data;
            $("#chubbanNo5").val($("#chubbanNo4").val());
            $("#bundleNo51").val($("#bundleNo1").val());
            $("#bundleNo52").val($("#bundleNo2").val());
            $("#bundleNo53").val($("#bundleNo3").val());
            $("#bundleNo54").val($("#bundleNo4").val());
            $("#txtEndDateTime").val($("#txtStartDateTime").val());
            $("#txtOperatorEnd").val(jsonresult.pP_EMPLOYEE_NAME);
            $("#txtCustomerNameEnd").val(jsonresult.pP_CUSTOMER_NAME);
            $("#txtProdNameEnd").val(jsonresult.pP_PROD_NAME_CUST);
            if (jsonresult.pP_PROD_DIM_D != null && jsonresult.pP_PROD_DIM_W != null) {
                $("#txtDimEnd").val(jsonresult.pP_PROD_DIM_D + "*" + jsonresult.pP_PROD_DIM_W);
            }
            else if (jsonresult.pP_PROD_DIM_D == null) {
                $("#txtDimEnd").val(jsonresult.pP_PROD_DIM_W);
            }
            else if (jsonresult.pP_PROD_DIM_W == null) {
                $("#txtDimEnd").val(jsonresult.pP_PROD_DIM_D);
            }
            $("#txtShapeEnd").val(jsonresult.pP_PROD_SHAPE);
            $("#txtDimOtherEnd").val(jsonresult.pP_PROD_DIM_OTHER);
            if (jsonresult.pP_DIM_TOL_MIN_CHR != null && jsonresult.pP_DIM_TOL_MAX_CHR != null) {
                $("#txtDimTolMinMaxChrEnd").val(jsonresult.pP_DIM_TOL_MAX_CHR + "," + jsonresult.pP_DIM_TOL_MIN_CHR);
            }
            else if (jsonresult.pP_DIM_TOL_MIN_CHR == null) {
                $("#txtDimTolMinMaxChrEnd").val(jsonresult.pP_DIM_TOL_MAX_CHR);
            }
            else if (jsonresult.pP_DIM_TOL_MAX_CHR == null) {
                $("#txtDimTolMinMaxChrEnd").val(jsonresult.pP_DIM_TOL_MIN_CHR);
            }
            $("#txtLengthEnd").val(jsonresult.length);
            if (jsonresult.pP_LENGTH_TOL_MIN_CHR != null && jsonresult.pP_LENGTH_TOL_MAX_CHR != null) {
                $("#txtLengthTolMinMaxChrEnd").val(jsonresult.pP_LENGTH_TOL_MAX_CHR + "," + jsonresult.pP_LENGTH_TOL_MIN_CHR);
            }
            else if (jsonresult.pP_LENGTH_TOL_MIN_CHR == null) {
                $("#txtLengthTolMinMaxChrEnd").val(jsonresult.pP_LENGTH_TOL_MAX_CHR);
            }
            else if (jsonresult.pP_LENGTH_TOL_MAX_CHR == null) {
                $("#txtLengthTolMinMaxChrEnd").val(jsonresult.pP_LENGTH_TOL_MIN_CHR);
            }
            $("#hdProcessPlanCD").val(jsonresult.pP_PROCESS_PLAN_CD);
            $("#totalOkUnitEnd").val($("#txtOkUnits").val());
            $("#UnitValue1End").val($("#ddlUnit option:selected").text());
            $("#totalOkWeightEnd").val($("#txtWeight").val());

            var defectUnit1 = parseInt($("#txtDefect1").val(), 10) || 0;
            var defectUnit2 = parseInt($("#txtDefect2").val(), 10) || 0;
            var defectUnit3 = parseInt($("#txtDefect3").val(), 10) || 0;
            if ($("#txtDefect1").val() == "" && $("#txtDefect2").val() == "" && $("#txtDefect3").val() == "") {
                $("#totalDefectUnitEnd").val('');
            }
            else {
                $("#totalDefectUnitEnd").val(defectUnit1 + defectUnit2 + defectUnit3);
            }


            var defectWeight1 = parseInt($("#txtWeight1").val(), 10) || 0;
            var defectWeight2 = parseInt($("#txtWeight2").val(), 10) || 0;
            var defectWeight3 = parseInt($("#txtWeight3").val(), 10) || 0;
            if ($("#txtWeight1").val() == "" && $("#txtWeight2").val() == "" && $("#txtWeight3").val() == "") {
                $("#totalDefectWeightEnd").val('');
            }
            else {
                $("#totalDefectWeightEnd").val(defectWeight1 + defectWeight2 + defectWeight3);
            }

            $("#UnitValue2End").val($("#ddlUnit1 option:selected").text());
            returnVal = true;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
$("#btnBackFifthForm").click(function (event) {
    $(startEndFifthForm).hide();
    $(startEndFourthForm).show();
});
$("#btnNextFifthForm").click(function (event) {
    var remark = $('#txtRemark').val()
    $(startEndFifthForm).hide();
    $(startEndSixthForm).show();
    if (remark == "") {
        getRemarkOfProcess();
    }
    else {
        $("#txtRemark").val(remark);
        $("#txtDecision").val($("#ddlDecision option:selected").text());
        $("#txtNextProcess").val($("#ddlNextProcess option:selected").text());
    }

});
$("#btnBackSixthForm").click(function (event) {
    $(startEndSixthForm).hide();
    $(startEndFifthForm).show();
});
$("#btnRegister").click(function (event) {

    var remark = $('#txtRemark').val();
    if (remark.indexOf(',') > -1 || remark.indexOf('"') > -1 || remark.indexOf("'") > -1) { //|| remark.indexOf(' ') > -1
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_019").text());
        showMessageDialog(message, focusFieldID, WARNING);
        $(startEndSixthForm).show();
        $(startEndFifthForm).hide();
    }
    else {
        saveGeneralProcessPlanEntryData();

    }
});


function IsDateValid(boolVal) {
    debugger;
    var returnval = false;
    var getDate;
    getDate = ($("#txtStartDateTime").val().split(" ")[0] + "T" + $("#txtStartDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/IsValidDateRange",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DATERANGE: commonDateRange, RANGE_DATETIME: getDate })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnval = response.isSuccess;
            if (returnval == false) {
                focusFieldID = $(':focus').attr("id");
                var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_011").text()).replace("NN", commonDateRange);
                showMessageDialog(message, focusFieldID, WARNING);
                return returnval;
            }
            else {
                return returnval;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });




    return returnval;
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
function setStartSysDateTime() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var sysDateTime = response.message;
            $("#txtStartDateTime").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
$("#txtOkUnits").keyup(function (event) {
    var orderNo = $('#chubbanNo1').val();
    /*if (SPLCHUUBANGYOUBAN != orderNo) {*/
    if ($("#txtOkUnits").val() != "" || $("#ddlUnit").val() != 0 || $("#chkFinishedWeight").prop("checked") == true) {
        $("#ddlUnit").addClass("Mandatory");
        $("#txtOkUnits").addClass("Mandatory");
    }
    else {
        $("#ddlUnit").removeClass("Mandatory");
        $("#txtOkUnits").removeClass("Mandatory");
    }
    /* }*/
});
$("#ddlUnit").change(function (event) {
    var orderNo = $('#chubbanNo1').val();
    /*if (SPLCHUUBANGYOUBAN != orderNo) {*/
    if ($("#ddlUnit").val() != 0 || $("#txtOkUnits").val() != "" || $("#chkFinishedWeight").prop("checked") == true) {
        $("#ddlUnit").addClass("Mandatory");
        $("#txtOkUnits").addClass("Mandatory");
    }
    else {
        $("#ddlUnit").removeClass("Mandatory");
        $("#txtOkUnits").removeClass("Mandatory");
    }
    /*}*/
});
function setMandatoryCssUnitType() {
    var orderNo = $('#chubbanNo1').val();
    /* if (SPLCHUUBANGYOUBAN != orderNo) {*/
    if ($("#ddlUnit").val() != 0 && $("#txtOkUnits").val() == "") {
        $("#ddlUnit").addClass("Mandatory");
        $("#txtOkUnits").addClass("Mandatory");
    }
    else {
        $("#ddlUnit").removeClass("Mandatory");
        $("#txtOkUnits").removeClass("Mandatory");
    }
    /* }*/
}
function setMandatoryCssNoOfUnit() {
    var orderNo = $('#chubbanNo1').val();
    /*  if (SPLCHUUBANGYOUBAN != orderNo) {*/
    if ($("#txtOkUnits").val() != "" || $("#ddlUnit").val() == 0) {
        $("#ddlUnit").addClass("Mandatory");
        $("#txtOkUnits").addClass("Mandatory");
    }
    else {
        $("#ddlUnit").removeClass("Mandatory");
        $("#txtOkUnits").removeClass("Mandatory");
    }
    /* }*/
}
$("#chkFinishedWeight").change(function (event) {
    if ($("#chkFinishedWeight").prop("checked") == true) {
        $("#ddlUnit").addClass("Mandatory");
        $("#txtOkUnits").addClass("Mandatory");
    }
    else if ($("#txtOkUnits").val() == "" && $("#ddlUnit").val() == 0) {
        $("#ddlUnit").removeClass("Mandatory");
        $("#txtOkUnits").removeClass("Mandatory");
    }
});
$("#txtDefect1").keyup(function (event) {
    if ($("#txtDefect1").val() == '0') {
        $("#ddlDefect1").val(0);
        $("#ddlUnit1").val(0);
        $("#txtWeight1").val('');
        removeMandatoryCSS($("#txtDefect1"), $("#ddlDefect1"), $("#txtWeight1"), $("#ddlUnit1"));
    } else {
        if ($("#txtDefect1").val() != "" || $("#txtWeight1").val() != "" || $("#ddlUnit1").val() != 0) {
            addMandatoryCSS($("#txtDefect1"), $("#ddlDefect1"), $("#txtWeight1"), $("#ddlUnit1"));
        }
        else {
            $("#ddlDefect1").val(0);
            removeMandatoryCSS($("#txtDefect1"), $("#ddlDefect1"), $("#txtWeight1"), $("#ddlUnit1"));
        }
    }
});
$("#txtWeight1").keyup(function (event) {
    if ($("#txtDefect1").val() == '0') {
        if ($("#txtDefect1").val() != "" || $("#txtWeight1").val() != "" || $("#ddlUnit1").val() != 0) {
            $("#txtDefect1").val('');
            addMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
        else {
            $("#ddlDefect1").val(0);
            removeMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
    }
    else {
        if ($("#txtDefect1").val() != "" || $("#txtWeight1").val() != "" || $("#ddlUnit1").val() != 0) {
            addMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
        else {
            $("#ddlDefect1").val(0);
            removeMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
    }
});
$("#ddlUnit1").change(function (event) {
    if ($("#txtDefect1").val() == '0') {
        if ($("#ddlUnit1").val() != 0) {
            $("#txtDefect1").val('');
            addMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
        else if ($("#txtDefect1").val() == "" && $("#txtWeight1").val() == "") {
            $("#ddlDefect1").val(0);
            removeMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
    }
    else {
        if ($("#ddlUnit1").val() != 0) {
            addMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
        else if ($("#txtDefect1").val() == "" && $("#txtWeight1").val() == "") {
            $("#ddlDefect1").val(0);
            removeMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
    }
});

$("#ddlDefect1").change(function (event) {
    if ($("#txtDefect1").val() == '0') {
        if ($("#ddlDefect1").val() != 0) {
            $("#txtDefect1").val('');
            addMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
        else {
            $("#ddlUnit1").val(0);
            $("#txtWeight1").val("");
            $("#txtDefect1").val("");
            removeMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
    }
    else {
        if ($("#ddlDefect1").val() != 0) {
            addMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
        else {
            $("#ddlUnit1").val(0);
            $("#txtWeight1").val("");
            $("#txtDefect1").val("");
            removeMandatoryCSS($("#txtWeight1"), $("#ddlDefect1"), $("#txtDefect1"), $("#ddlUnit1"));
        }
    }
});

$("#txtDefect2").keyup(function (event) {
    if ($("#txtDefect2").val() == '0') {
        $("#ddlDefect2").val(0);
        $("#ddlUnit2").val(0);
        $("#txtWeight2").val('');
        removeMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
    }
    else {
        if ($("#txtDefect2").val() != "" || $("#txtWeight2").val() != "" || $("#ddlUnit2").val() != 0) {
            addMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
        else {
            $("#ddlDefect2").val(0);
            removeMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
    }
});
$("#txtWeight2").keyup(function (event) {
    if ($("#txtDefect2").val() == '0') {
        if ($("#txtDefect2").val() != "" || $("#txtWeight2").val() != "" || $("#ddlUnit2").val() != 0) {
            $("#txtDefect2").val('');
            addMandatoryCSS($("#txtWeight2"), $("#ddlDefect2"), $("#txtDefect2"), $("#ddlUnit2"));
        }
        else {
            $("#ddlDefect2").val(0);
            removeMandatoryCSS($("#txtWeight2"), $("#ddlDefect2"), $("#txtDefect2"), $("#ddlUnit2"));
        }
    }
    else {
        if ($("#txtDefect2").val() != "" || $("#txtWeight2").val() != "" || $("#ddlUnit2").val() != 0) {
            addMandatoryCSS($("#txtWeight2"), $("#ddlDefect2"), $("#txtDefect2"), $("#ddlUnit2"));
        }
        else {
            $("#ddlDefect2").val(0);
            removeMandatoryCSS($("#txtWeight2"), $("#ddlDefect2"), $("#txtDefect2"), $("#ddlUnit2"));
        }
    }
});
$("#ddlUnit2").change(function (event) {
    if ($("#txtDefect2").val() == '0') {
        if ($("#ddlUnit2").val() != 0) {
            $("#txtDefect2").val(''); 
            addMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
        else if ($("#txtDefect2").val() == "" && $("#txtWeight2").val() == "") {
            $("#ddlDefect2").val(0);
            removeMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
    }
    else {
        if ($("#ddlUnit2").val() != 0) {
            addMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
        else if ($("#txtDefect2").val() == "" && $("#txtWeight2").val() == "") {
            $("#ddlDefect2").val(0);
            removeMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
    }
});

$("#ddlDefect2").change(function (event) {
    if ($("#txtDefect2").val() == '0') {
        if ($("#ddlDefect2").val() != 0) {
            $("#txtDefect2").val('');
            addMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
        else {
            $("#ddlUnit2").val(0);
            $("#txtWeight2").val("");
            $("#txtDefect2").val("");
            removeMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
    }
    else {
        if ($("#ddlDefect2").val() != 0) {
            addMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
        else {
            $("#ddlUnit2").val(0);
            $("#txtWeight2").val("");
            $("#txtDefect2").val("");
            removeMandatoryCSS($("#txtDefect2"), $("#ddlDefect2"), $("#txtWeight2"), $("#ddlUnit2"));
        }
    }
});

$("#txtDefect3").keyup(function (event) {
    if ($("#txtDefect3").val() == '0') {
        $("#ddlDefect3").val(0);
        $("#ddlUnit3").val(0);
        $("#txtWeight3").val('');
        removeMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
    }
    else {
        if ($("#txtDefect3").val() != "" || $("#txtWeight3").val() != "" || $("#ddlUnit3").val() != 0) {
            addMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
        else {
            $("#ddlDefect3").val(0);
            removeMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
    }
});
$("#txtWeight3").keyup(function (event) {
    if ($("#txtDefect3").val() == '0') {
        if ($("#txtDefect3").val() != "" || $("#txtWeight3").val() != "" || $("#ddlUnit3").val() != 0) {
            $("#txtDefect3").val('');
            addMandatoryCSS($("#txtWeight3"), $("#ddlDefect3"), $("#txtDefect3"), $("#ddlUnit3"));
        }
        else {
            $("#ddlDefect3").val(0);
            removeMandatoryCSS($("#txtWeight3"), $("#ddlDefect3"), $("#txtDefect3"), $("#ddlUnit3"));
        }
    }
    else {
        if ($("#txtDefect3").val() != "" || $("#txtWeight3").val() != "" || $("#ddlUnit3").val() != 0) {
            addMandatoryCSS($("#txtWeight3"), $("#ddlDefect3"), $("#txtDefect3"), $("#ddlUnit3"));
        }
        else {
            $("#ddlDefect3").val(0);
            removeMandatoryCSS($("#txtWeight3"), $("#ddlDefect3"), $("#txtDefect3"), $("#ddlUnit3"));
        }
    }
});
$("#ddlUnit3").change(function (event) {
    if ($("#txtDefect3").val() == '0') {
        if ($("#ddlUnit3").val() != 0) {
            $("#txtDefect3").val('');
            addMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
        else if ($("#txtDefect3").val() == "" && $("#txtWeight3").val() == "") {
            $("#ddlDefect3").val(0);
            removeMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
    }
    else {
        if ($("#ddlUnit3").val() != 0) {
            addMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
        else if ($("#txtDefect3").val() == "" && $("#txtWeight3").val() == "") {
            $("#ddlDefect3").val(0);
            removeMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
    }
});
$("#ddlDefect3").change(function (event) {
    if ($("#txtDefect3").val() == '0') {
        if ($("#ddlDefect3").val() != 0) {
            $("#txtDefect3").val('');
            addMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
        else {

            $("#ddlUnit3").val(0);
            $("#txtWeight3").val("");
            $("#txtDefect3").val("");
            removeMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
    }
    else {
        if ($("#ddlDefect3").val() != 0) {
            addMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
        else {

            $("#ddlUnit3").val(0);
            $("#txtWeight3").val("");
            $("#txtDefect3").val("");
            removeMandatoryCSS($("#txtDefect3"), $("#ddlDefect3"), $("#txtWeight3"), $("#ddlUnit3"));
        }
    }
});

function addMandatoryCSS($txtTarget, $ddlTarget, $txtTarget1, $ddlTarget1) {

    //if ($txtTarget.val() > 0 || $ddlTarget.val() > 0 || $txtTarget1.val() > 0 || $ddlTarget1.val() > 0) {
    $txtTarget.addClass("Mandatory");
    $ddlTarget.addClass("Mandatory");
    $txtTarget1.addClass("Mandatory");
    $ddlTarget1.addClass("Mandatory");
    //}
}
function removeMandatoryCSS($txtTarget, $ddlTarget, $txtTarget1, $ddlTarget1) {

    //if ($txtTarget.val() > 0 || $ddlTarget.val() > 0 || $txtTarget1.val() > 0 || $ddlTarget1.val() > 0) {
    $txtTarget.removeClass("Mandatory");
    $ddlTarget.removeClass("Mandatory");
    $txtTarget1.removeClass("Mandatory");
    $ddlTarget1.removeClass("Mandatory");
    //}
}
function removeMandatoryCSSOnNextBtn() {
    $("#txtDefect1").removeClass("Mandatory");
    $("#ddlDefect1").removeClass("Mandatory");
    $("#txtWeight1").removeClass("Mandatory");
    $("#ddlUnit1").removeClass("Mandatory");
    $("#txtDefect2").removeClass("Mandatory");
    $("#ddlDefect2").removeClass("Mandatory");
    $("#txtWeight2").removeClass("Mandatory");
    $("#ddlUnit2").removeClass("Mandatory");
    $("#txtDefect3").removeClass("Mandatory");
    $("#ddlDefect3").removeClass("Mandatory");
    $("#txtWeight3").removeClass("Mandatory");
    $("#ddlUnit3").removeClass("Mandatory");
    $("#ddlUnit").removeClass("Mandatory");
    $("#txtOkUnits").removeClass("Mandatory");
}
function getRemarkOfProcess() {
    var ajaxTime = new Date().getTime();
    ProcessCD = $("#processCD").val().toUpperCase().slice(3)
    var jsonPlanData = ({

        PROCESS_KUBUN_CD: ProcessCD,
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/getRemarksOfProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            if (response.message != "") {
                $("#txtRemark").val(response.message);
                $("#txtDecision").val($("#ddlDecision option:selected").text());
                $("#txtNextProcess").val($("#ddlNextProcess option:selected").text());
            }
            else {
                $("#txtRemark").val("");
                $("#txtDecision").val($("#ddlDecision option:selected").text());
                $("#txtNextProcess").val($("#ddlNextProcess option:selected").text());
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function setNextProcessList() {
    var ajaxTime = new Date().getTime();
    $("#hdStartProcessCD").val($("#processCD").val().slice(3));
    var jsonData = ({
        PROCESS_KUBUN_CD: $("#hdStartProcessCD").val()
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindNextProcessDropdown(response.data);
            var orderNo = $('#chubbanNo1').val();
            if (SPLCHUUBANGYOUBAN == orderNo && response.data[0].nexT_PROCESS_FLAG == 1) {
                for (i = 1; i <= response.data.length; i++) {
                    if (response.data[i - 1].procesS_KUBUN_CD == 999) {
                        $("#ddlNextProcess").val(response.data[i - 1].procesS_KUBUN_CD);
                    }
                }

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function bindNextProcessDropdown(jsonResult) {
    $("#ddlNextProcess").text('');
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
function getProcessNameOnMergeScreen() {
    var ajaxTime = new Date().getTime();
    $("#hdStartProcessCD").val($("#processCD").val().slice(3));
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetProcessNameOnMergeScreen?PROCESS_KUBUN_CD=" + $("#hdStartProcessCD").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.data;
            if ($("#hdfCultureName").val() == "en") // > -1 means substring is present
            {
                processName = data.procesS_KUBUN_EN;
            }
            else {
                processName = data.procesS_KUBUN_JP;
            }
            $("#startProcessName").text(processName);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function binddecisionDropdown(jsonResult) {
    $("#ddlDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setDecision() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/GeneralDailyProcessStartEnd/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
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
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function setUnitType() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/GeneralDailyProcessStartEnd/GetUnitType",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindUnitTypeDropdown(jsonResult) {
    $("#ddlUnit").html("").append($('<option value="0"></option>'));
    $("#ddlUnit1").html("").append($('<option value="0"></option>'));
    $("#ddlUnit2").html("").append($('<option value="0"></option>'));
    $("#ddlUnit3").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnit").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
        $("#ddlUnit1").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
        $("#ddlUnit2").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
        $("#ddlUnit3").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));

    });
}
function setDefectList() {
    var ajaxTime = new Date().getTime();
    var process_CD = $("#processCD").val().slice(3)
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetProcessDefectList?KUBUN_ID=" + process_CD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDefectListDropdown(response.data);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindDefectListDropdown(jsonResult) {
    $("#ddlDefect1").html("").append($('<option value="0"></option>'));
    $("#ddlDefect2").html("").append($('<option value="0"></option>'));
    $("#ddlDefect3").html("").append($('<option value="0"></option>'));
    //$.each(jsonResult, function (key, jsonObj) {
    //    $("#ddlDefect1").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    //    $("#ddlDefect2").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    //    $("#ddlDefect3").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    //});
    if ($("#hdfCultureName").val() == "en") {

        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefect1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
        });
    } else {

        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefect1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }
    if ($("#hdfCultureName").val() == "en") {

        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefect2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
        });
    } else {

        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefect2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }
    if ($("#hdfCultureName").val() == "en") {

        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefect3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
        });
    } else {

        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDefect3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }
}
function saveGeneralProcessPlanEntryData() {

    var ajaxTime = new Date().getTime();

    PP_PROCESS_PLAN_CD = $("#hdProcessPlanCD").val();
    ProcessCD = $("#processCD").val().slice(3);
    operatorCD = $("#operatorCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    USER_ID = $("#hdfLoginId").val();
    GP_START_DATE = ($("#txtStartDateTime").val().split(" ")[0] + "T" + $("#txtStartDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    tabaArray = [];
    //tabaArray.push(parseInt(Thaba1));
    if (Thaba1 != null) {
        tabaArray.push(parseInt(Thaba1));
    }
    if (Thaba2 != null) {
        tabaArray.push(parseInt(Thaba2));
    }
    if (Thaba3 != null) {
        tabaArray.push(parseInt(Thaba3));
    }
    if (Thaba4 != null) {
        tabaArray.push(parseInt(Thaba4));
    }
    tabaArray.sort(function (a, b) {
        return parseInt(a) - parseInt(b);
    });

    console.log(tabaArray);

    if (tabaArray.length > 0) {
        Thaba1 = tabaArray[0];
    }
    else {
        Thaba1 = null;
    }
    if (tabaArray.length > 1) {
        Thaba2 = tabaArray[1];
    }
    else {
        Thaba2 = null;
    }
    if (tabaArray.length > 2) {
        Thaba3 = tabaArray[2];
    }
    else {
        Thaba3 = null;
    }
    if (tabaArray.length > 3) {
        Thaba4 = tabaArray[3];
    }
    else {
        Thaba4 = null;
    }
    GP_METALLIC_PLATE_CODE = $("#metalicPlateNo").val();
    GP_END_DATE = ($("#txtProcessEndDateTime").val().split(" ")[0] + "T" + $("#txtProcessEndDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    GP_REMARK = $("#txtRemark").val() ? $("#txtRemark").val() : null;
    GP_DECISION = $("#ddlDecision option:selected").val();
    GP_NEXT_PROCESS = $("#ddlNextProcess option:selected").val();
    GP_OK_UNITS = $("#txtOkUnits").val() ? $("#txtOkUnits").val() : null;
    UNIT_TYPE = $("#ddlUnit option:selected").val();
    if (UNIT_TYPE == 0) {
        UNIT_TYPE = null;
    }
    else {
        UNIT_TYPE = $("#ddlUnit option:selected").val();
    }
    GP_QTY = $("#txtWeight").val();
    GP_QTY = $("#txtWeight").val();
    if (GP_QTY == "") {
        GP_QTY = null;
    }
    else {
        GP_QTY = $("#txtWeight").val();
    }
    GP_NG_DETAILS1 = $("#ddlDefect1 option:selected").val();
    if (GP_NG_DETAILS1 == 0) {
        GP_NG_DETAILS1 = null;
    }
    else {
        GP_NG_DETAILS1 = $("#ddlDefect1 option:selected").val();
    }
    GP_NG_UNITS1 = $("#txtDefect1").val() ? $("#txtDefect1").val() : null;
    GP_NG_UNIT_TYPE1 = $("#ddlUnit1 option:selected").val();
    if (GP_NG_UNIT_TYPE1 == 0) {
        GP_NG_UNIT_TYPE1 = null;
    }
    else {
        GP_NG_UNIT_TYPE1 = $("#ddlUnit1 option:selected").val();
    }
    GP_NG_QTY1 = $("#txtWeight1").val() ? $("#txtWeight1").val() : null;

    GP_NG_DETAILS2 = $("#ddlDefect2 option:selected").val();
    if (GP_NG_DETAILS2 == 0) {
        GP_NG_DETAILS2 = null;
    }
    else {
        GP_NG_DETAILS2 = $("#ddlDefect2 option:selected").val();
    }
    GP_NG_UNITS2 = $("#txtDefect2").val() ? $("#txtDefect2").val() : null;
    GP_NG_UNIT_TYPE2 = $("#ddlUnit2 option:selected").val();
    if (GP_NG_UNIT_TYPE2 == 0) {
        GP_NG_UNIT_TYPE2 = null;
    }
    else {
        GP_NG_UNIT_TYPE2 = $("#ddlUnit2 option:selected").val();
    }
    GP_NG_QTY2 = $("#txtWeight2").val() ? $("#txtWeight2").val() : null;

    GP_NG_DETAILS3 = $("#ddlDefect3 option:selected").val();
    if (GP_NG_DETAILS3 == 0) {
        GP_NG_DETAILS3 = null;
    }
    else {
        GP_NG_DETAILS3 = $("#ddlDefect3 option:selected").val();
    }
    GP_NG_UNITS3 = $("#txtDefect3").val() ? $("#txtDefect3").val() : null;
    GP_NG_UNIT_TYPE3 = $("#ddlUnit3 option:selected").val();
    if (GP_NG_UNIT_TYPE3 == 0) {
        GP_NG_UNIT_TYPE3 = null;
    }
    else {
        GP_NG_UNIT_TYPE3 = $("#ddlUnit3 option:selected").val();
    }
    GP_NG_QTY3 = $("#txtWeight3").val() ? $("#txtWeight3").val() : null;
    var jsonPlanData = ({
        GP_START_OPERATOR_CD: operatorCD,
        PROCESS_KUBUN_CD: ProcessCD,
        MACHINE_CD: machineCD,
        PP_PROCESS_PLAN_CD: PP_PROCESS_PLAN_CD,
        USER_ID: USER_ID,
        GP_START_DATE: GP_START_DATE,
        GP_REMARK: GP_REMARK,
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
        GP_METALLIC_PLATE_CODE: GP_METALLIC_PLATE_CODE,
        GP_END_OPERATOR_CD: operatorCD,
        GP_END_DATE: GP_END_DATE,
        GP_DECISION: GP_DECISION,
        GP_NEXT_PROCESS: GP_NEXT_PROCESS,
        GP_OK_UNITS: GP_OK_UNITS,
        UNIT_TYPE: UNIT_TYPE,
        GP_QTY: GP_QTY,
        GP_NG_DETAILS1: GP_NG_DETAILS1,
        GP_NG_UNITS1: GP_NG_UNITS1,
        GP_NG_UNIT_TYPE1: GP_NG_UNIT_TYPE1,
        GP_NG_QTY1: GP_NG_QTY1,
        GP_NG_DETAILS2: GP_NG_DETAILS2,
        GP_NG_UNITS2: GP_NG_UNITS2,
        GP_NG_UNIT_TYPE2: GP_NG_UNIT_TYPE2,
        GP_NG_QTY2: GP_NG_QTY2,
        GP_NG_DETAILS3: GP_NG_DETAILS3,
        GP_NG_UNITS3: GP_NG_UNITS3,
        GP_NG_UNIT_TYPE3: GP_NG_UNIT_TYPE3,
        GP_NG_QTY3: GP_NG_QTY3,
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/saveGeneralProcessPlanEntryEndData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            if (response.message == "Data Already Exist") {

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgDataAlreadyExists").text());
                showMessageDialog(message, focusFieldID, WARNING);

            }
           else if (response.message == "Data Inserted Successfully") {
                var bundleList = "";
                if (Thaba1 != null) {
                    bundleList = Thaba1;
                }
                if (Thaba2 != null) {
                    bundleList = bundleList + "," + Thaba2;
                }
                if (Thaba3 != null) {
                    bundleList = bundleList + "," + Thaba3;
                }
                if (Thaba4 != null) {
                    bundleList = bundleList + "," + Thaba4;
                }
                if (Thaba1 == null && Thaba2 == null && Thaba3 == null && Thaba4 == null) {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#successMsgDataSavedWithoutTaba").text());
                   
                }
                else {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_018").text()).replace('NNNN,NNN,NNN,NNN', bundleList);
                }
                //showMessageDialog(message, focusFieldID, SUCCESS);
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                    centerVertical: true,
                    callback: function () {
                        $(startEndSixthForm).hide();
                        $(startEndFirstForm).show();
                        $("#chubbanNo1").val("");
                        $("#bundleNo1").val("");
                        $("#bundleNo2").val("");
                        $("#bundleNo3").val("");
                        $("#bundleNo4").val("");
                    }
                });

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}