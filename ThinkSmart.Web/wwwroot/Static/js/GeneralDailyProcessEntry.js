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
var tabaArray = [];
$(document).ready(function () {
    setFocus();
    RestrictToDigitsOnly();

});
function setFocus() {
    document.getElementById("chubbanNo1").focus();
}

$("#btnMenu").click(function (event) {
    self.close();
});
function RestrictToDigitsOnly() {
    $("#bundleNo1,#bundleNo2,#bundleNo3,#bundleNo4").on('input', function () {
        // Replace all non-digit characters
        this.value = this.value.replace(/[^0-9]/g, '');
    });
  
}
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
                    $("#bundleNo1").focus();
                    var orderNo = $('#chubbanNo1').val();
                    if (SPLCHUUBANGYOUBAN == orderNo) {
                        $('#bundleNo2').prop("disabled", true);
                        $('#bundleNo3').prop("disabled", true);
                        $('#bundleNo4').prop("disabled", true);
                    }
                } else {
                    $("#chubbanNo1").val("");
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
                        message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_026").text().trim();
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
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_020").text());
                }
                else if (firstindex == 2) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_021").text());
                }
                else if (firstindex == 3) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_022").text());
                }
                else if (firstindex == 4) {
                    firstindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_023").text());
                }
                if (Lastindex == 1) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_020").text());
                }
                else if (Lastindex == 2) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_021").text());
                }
                else if (Lastindex == 3) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_022").text());
                }
                else if (Lastindex == 4) {
                    Lastindex = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_023").text());
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
            if (Thaba1 == null && Thaba2 == null && Thaba3 == null && Thaba4 == null) {
                if (!ValidateChubban()) {
                    return false;
                }
                else {
                    $(entrySecondForm).show();
                    $(entryFirstForm).hide();
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
                    $(entrySecondForm).show();
                    $(entryFirstForm).hide();
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
$("#btnBackSecondForm").click(function (event) {
    $(entrySecondForm).hide();
    $(entryFirstForm).show();
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
            $(entryThirdForm).show();
            $(entrySecondForm).hide();
        }
    }
});
$("#btnBackThirdForm").click(function (event) {
    $(entryThirdForm).hide();
    $(entrySecondForm).show();
});
$("#btnNextThirdForm").click(function (event) {
    var remark = $('#txtRemark').val();
    $(entryFourthForm).show();
    $(entryThirdForm).hide();
    if (remark == "") {
        getRemarkOfProcess();
    }
    else {
        $("#txtRemark").val(remark);
    }

});
$("#btnBackFourthForm").click(function (event) {
    $(entryFourthForm).hide();
    $(entryThirdForm).show();
});
$("#btnRegister").click(function (event) {
    var remark = $('#txtRemark').val();
    if (remark.indexOf(',') > -1 || remark.indexOf('"') > -1 || remark.indexOf("'") > -1) { //|| remark.indexOf(' ') > -1
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_019").text());
        showMessageDialog(message, focusFieldID, WARNING);
        $(entryFourthForm).show();
        $(entryThirdForm).hide();
    }
    else {
        saveGeneralProcessPlanEntryData();

    }

});
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
            message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
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
                $(entrySecondForm).hide();
                $(entryFirstForm).show();
            }
            else if (data.responce == "Chubban is already completed") {
                $(entryFirstForm).show();
                $(entrySecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_008").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else if (data.responce == "Process is already Started for this chubban") {
                $(entryFirstForm).show();
                $(entrySecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgDuplicateGenProcessWithoutTaba").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;
            }
            else {
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
    // tabaList.push(parseInt(Thaba1));
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
                $(entrySecondForm).hide();
                $(entryFirstForm).show();
            }
            else if (data.responce == "Chubban is already completed") {
                $(entryFirstForm).show();
                $(entrySecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_008").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else if (data.responce == "Process is already started for this bundle no") {
                $(entryFirstForm).show();
                $(entrySecondForm).hide();
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
            message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text().trim();
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
                    $(entrySecondForm).show();
                    $(entryThirdForm).hide();
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
            message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

    return returnVal;

}
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
                $(entrySecondForm).show();
                $(entryThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Operator is Inactive") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_024").text());
                showMessageDialog(message, focusFieldID, WARNING);
                $(entrySecondForm).show();
                $(entryThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Process is not found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_016").text().replace("*****", $("#lblProcess").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                $(entrySecondForm).show();
                $(entryThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Machine is not found") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_016").text().replace("*****", $("#lblMachine").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                $(entrySecondForm).show();
                $(entryThirdForm).hide();
                returnVal = false;
            }
            else if (jsonresult.responce == "Machine is Inactive") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_025").text());
                showMessageDialog(message, focusFieldID, WARNING);
                $(entrySecondForm).show();
                $(entryThirdForm).hide();
                returnVal = false;
            }
            else {

                var orderNo = $('#chubbanNo1').val();
                if (SPLCHUUBANGYOUBAN != orderNo) {
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
                            var response = response.data;
                            if (response == "Process already exist for this chuuban with Ok decision") {
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
                                            if (SPLCHUUBANGYOUBAN != orderNo && $("#checkBoxForConfirmation").prop("checked") == false) {
                                                focusFieldID = $(':focus').attr("id");
                                                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_015").text());
                                                showMessageDialog(message, focusFieldID, WARNING);
                                                $(entrySecondForm).show();
                                                $(entryThirdForm).hide();
                                                returnVal = false;
                                            }
                                            else {
                                                console.log(jsonresult);
                                                $(entryThirdForm).show();
                                                $(entrySecondForm).hide();
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
                                                // $("#txtDimTolMinMaxChr").val(jsonresult.pP_DIM_TOL_MIN_CHR + "," + jsonresult.pP_DIM_TOL_MAX_CHR);
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
                                        else {
                                            returnVal = false;
                                        }

                                    },
                                    centerVertical: true
                                });
                                returnVal = false;

                            }
                            else {

                                if (SPLCHUUBANGYOUBAN != orderNo && $("#checkBoxForConfirmation").prop("checked") == false) {
                                    focusFieldID = $(':focus').attr("id");
                                    message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_015").text());
                                    showMessageDialog(message, focusFieldID, WARNING);
                                    $(entrySecondForm).show();
                                    $(entryThirdForm).hide();
                                    returnVal = false;
                                }
                                else {
                                    console.log(jsonresult);
                                    $(entryThirdForm).show();
                                    $(entrySecondForm).hide();
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
                                    // $("#txtDimTolMinMaxChr").val(jsonresult.pP_DIM_TOL_MIN_CHR + "," + jsonresult.pP_DIM_TOL_MAX_CHR);
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
                }

              else  if (SPLCHUUBANGYOUBAN != orderNo && $("#checkBoxForConfirmation").prop("checked") == false) {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_015").text());
                    showMessageDialog(message, focusFieldID, WARNING);
                    $(entrySecondForm).show();
                    $(entryThirdForm).hide();
                    returnVal = false;
                }
                else {
                    console.log(jsonresult);
                    $(entryThirdForm).show();
                    $(entrySecondForm).hide();
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
                    // $("#txtDimTolMinMaxChr").val(jsonresult.pP_DIM_TOL_MIN_CHR + "," + jsonresult.pP_DIM_TOL_MAX_CHR);
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
            message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
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
                            bootbox.hideAll();
                            returnVal = true;
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
            }
            else {
                $("#txtRemark").val("");
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
function saveGeneralProcessPlanEntryData() {

    var ajaxTime = new Date().getTime();
    ProcessCD = $("#processCD").val().slice(3);
    operatorCD = $("#operatorCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    PP_PROCESS_PLAN_CD = $("#hdProcessPlanCD").val();
    USER_ID = $("#hdfLoginId").val();
    GP_START_DATE = ($("#txtStartDateTime").val().split(" ")[0] + "T" + $("#txtStartDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
    GP_REMARK = $("#txtRemark").val() ? $("#txtRemark").val() : null;
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
    GP_METALLIC_PLATE_CODE = $("#metalicPlateNo").val();
    tabaArray = [];
    //tabaArray.push(Thaba1);
    if (Thaba1 != null) {
        tabaArray.push(Thaba1);
    }
    if (Thaba2 != null) {
        tabaArray.push(Thaba2);
    }
    if (Thaba3 != null) {
        tabaArray.push(Thaba3);
    }
    if (Thaba4 != null) {
        tabaArray.push(Thaba4);
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
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/saveGeneralProcessPlanEntryData",
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
                    message = $.trim($("#lblSuccessMessageE_GENREALPROCESS_ENTRY_018").text()).replace('NNNN,NNN,NNN,NNN', bundleList);
                }
                //showMessageDialog(message, focusFieldID, SUCCESS);
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                    centerVertical: true,
                    callback: function () {
                        $(entryFourthForm).hide();
                        $(entryFirstForm).show();
                        $("#chubbanNo1").val("");
                        $("#bundleNo1").val("");
                        $("#bundleNo2").val("");
                        $("#bundleNo3").val("");
                        $("#bundleNo4").val("");
                        $('#bundleNo2').prop("disabled", false);
                        $('#bundleNo3').prop("disabled", false);
                        $('#bundleNo4').prop("disabled", false);
                    }
                });
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
