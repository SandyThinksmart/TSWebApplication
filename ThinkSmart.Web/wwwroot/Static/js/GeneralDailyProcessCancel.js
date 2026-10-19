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
var processData =[];
var processPlanCd = "";
var generalProcessCd = "";
var gpDate = "";

var checked = false;
var tabaArray = [];
$(document).ready(function () {
    setFocus();
    RestrictToDigitsOnly();
});
function setFocus() {
    document.getElementById("chubbanNo1").focus();
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
                    debugger;

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
                var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_01").text());
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
                        message = $("#lblErrorMessageE_GENREALPROCESS_ENTRY_013").text().trim();
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
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    operatorCD = $("#operatorCD").val().toUpperCase();
    ProcessCD = $("#processCD").val().toUpperCase();
    machineCD = $("#machineCD").val().toUpperCase();
    var inputElement = $('#chubbanNo1').val().toUpperCase();
    if ($("#chubbanNo1").val() == "") {
        validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_02").text());
    }
    //if (Thaba1 == "") {
    //    if (validationMessage != "") {
    //        validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_03").text());
    //    }
    //    else {
    //        validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_03").text());
    //    }
    //}
    if (operatorCD == "") {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_05").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_05").text());
        }
    }
    if (ProcessCD == "") {
        focusFieldID = $(':focus').attr("id");
        if (validationMessage != "") {
            validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_06").text());
        }
        else {
            validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_06").text());
        }
    }
    if (operatorCD != "") {
        if (!operatorCD.match("OPR")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_04").text().replace("*****", $("#lblOperator").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_04").text().replace("*****", $("#lblOperator").text()));
            }
        }
    }
    if (ProcessCD != "") {
        if (!ProcessCD.match("PCS")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_04").text().replace("*****", $("#lblProcess").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_04").text().replace("*****", $("#lblProcess").text()));
            }
        }
    }

    if (machineCD != "") {
        if (!machineCD.match("MAC")) {
            focusFieldID = $(':focus').attr("id");
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_04").text().replace("*****", $("#lblMachine").text()));
            }
            else {
                validationMessage = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_04").text().replace("*****", $("#lblMachine").text()));
            }
        }
    }
    if (validationMessage != "") {
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(validationMessage, focusFieldID, WARNING);
    }
    if (validationMessage == "") {
        if (!clickBtnNextFirstFormForValidation()) {
            return false;
        }
        else {
            $(cancelSecondForm).show();
            $(cancelFirstForm).hide();
        }
        
    }
   
});
$("#btnBackSecondForm").click(function (event) {
    $(cancelFirstForm).show();
    $(cancelSecondForm).hide();
});
$("#btnNextSecondForm").click(function (event) {
     gpDate = "";
    for (let i = 1; i <= processData.length; i++) {
         checked = $('#selectRow' + i).is(':checked');
        
        if (checked) {
            generalProcessCd = $("#generalProcessCD" + i).text();
             processPlanCd = $("#processPlanCD" + i).text();
            gpDate = $("#gpDate" + i).text();
            if (!BindGeneralProcessData()) {
                returnVal = false;
            }
            else {
                $(cancelThirdForm).show();
                $(cancelSecondForm).hide();
            }
        }
    }
    });
$("#btnBackThirdForm").click(function (event) {
    $(cancelSecondForm).show();
    $(cancelThirdForm).hide();
});
$("#btnCancel").click(function (event) {
    deleteGeneralProcessPlanData();
});
function clickBtnNextFirstFormForValidation() {
    
    operatorCD = $("#operatorCD").val().slice(3);
    ProcessCD = $("#processCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    var ajaxTime = new Date().getTime();
    var jsonPlanData = ({
        GP_OPERATOR_CD: operatorCD,
        PROCESS_KUBUN_CD: ProcessCD,
        MACHINE_CD: machineCD,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/CheckValidationsProcessMachineOprOnCancel",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            debugger;
            var data = response.data;   
             if (data== "Process is not found") {
                $(cancelFirstForm).show();
                $(cancelSecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_08").text().replace("*****", $("#lblProcess").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else if (data== "Operator is not found") {
                $(cancelFirstForm).show();
                $(cancelSecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_08").text().replace("*****", $("#lblOperator").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

             }
             else if (data == "Operator is Inactive") {
                 $(cancelFirstForm).show();
                 $(cancelSecondForm).hide();
                 focusFieldID = $(':focus').attr("id");
                 message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_011").text());
                 showMessageDialog(message, focusFieldID, WARNING);
                 returnVal = false;

             }
            else if (data== "Machine is not found") {
                $(cancelFirstForm).show();
                $(cancelSecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_08").text().replace("*****", $("#lblMachine").text()));
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

             }
             else if (data == "Machine is Inactive") {
                 $(cancelFirstForm).show();
                 $(cancelSecondForm).hide();
                 focusFieldID = $(':focus').attr("id");
                 message = $.trim($("#lblErrorMessageE_GENREALPROCESS_ENTRY_012").text());
                 showMessageDialog(message, focusFieldID, WARNING);
                 returnVal = false;

             }
            else if (data== "Success") {
                BindGeneralProcessDataGrid();
            }
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    
    return returnVal;

}
function BindGeneralProcessDataGrid() {
    processData = [];
    tabaArray = [];
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
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
    operatorCD = $("#operatorCD").val().slice(3);
    ProcessCD = $("#processCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    var ajaxTime = new Date().getTime();
    var jsonPlanData = ({
        PP_CHUUBAN: $("#chubbanNo1").val(),
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
        GP_OPERATOR_CD: operatorCD,
        PROCESS_KUBUN_CD: ProcessCD,
        MACHINE_CD: machineCD,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/CheckValidationsOnGeneralDailyProcessCancel",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            var data = response.data;
            processData = response.data;
            if (data[0].responce == "Daily Process Does not Exist") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_09").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;
                $(cancelSecondForm).hide();
                $(cancelFirstForm).show();
            }
            else if (data[0].responce == "Chubban is already completed") {
                $(cancelFirstForm).show();
                $(cancelSecondForm).hide();
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_07").text());
                showMessageDialog(message, focusFieldID, WARNING);
                returnVal = false;

            }
            else {
                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/GeneralDailyProcessCancel/GeneralDailyProcessGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.data)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        var jsonResult = response1.message;
                        $("#GeneralDailyProcessDataDisplayGrid").html(response1.message);
                        $("#selectRow1").attr('checked', true);
                        returnVal = true;
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_01").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });




            }

        })
}
function BindGeneralProcessData() {
    var ajaxTime = new Date().getTime();
    tabaArray = [];
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
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
        GEN_PROCESS_CD: generalProcessCd,
        MACHINE_CD: machineCD,
        PP_CHUUBAN: PP_CHUUBAN,
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
        url: APIURL + "/GeneralDailyProcess/GetGeneralProcessDataForCancel",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var jsonresult = response.data;
            if (jsonresult !=null) {
                console.log(jsonresult);
                $("#chubbanNo3").val($("#chubbanNo1").val());
                $("#bundleNo21").val($("#bundleNo1").val());
                $("#bundleNo22").val($("#bundleNo2").val());
                $("#bundleNo23").val($("#bundleNo3").val());
                $("#bundleNo24").val($("#bundleNo4").val());
                $("#txtDateTime").val(gpDate);
                $("#txtOperator").val(jsonresult.pP_EMPLOYEE_NAME);
                $("#txtProcess").val(jsonresult.pP_PROCESS_NAME);
                $("#txtMachine").val(jsonresult.pP_MACHINE_NAME);
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
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
function deleteGeneralProcessPlanData() {

    var ajaxTime = new Date().getTime();
    ProcessCD = $("#processCD").val().slice(3);
    operatorCD = $("#operatorCD").val().slice(3);
    if ($("#machineCD").val() != "") {
        machineCD = $("#machineCD").val().slice(3);
    }
    else {
        machineCD = null;
    }
    tabaArray = [];
    Thaba1 = $("#bundleNo1").val() ? $("#bundleNo1").val() : null;
    Thaba2 = $("#bundleNo2").val() ? $("#bundleNo2").val() : null;
    Thaba3 = $("#bundleNo3").val() ? $("#bundleNo3").val() : null;
    Thaba4 = $("#bundleNo4").val() ? $("#bundleNo4").val() : null;
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
        GP_START_OPERATOR_CD: operatorCD,
        PROCESS_KUBUN_CD: ProcessCD,
        MACHINE_CD: machineCD,
        PP_PROCESS_PLAN_CD: processPlanCd,
        GEN_PROCESS_CD: generalProcessCd,
        GP_TABA_NO_1: Thaba1,
        GP_TABA_NO_2: Thaba2,
        GP_TABA_NO_3: Thaba3,
        GP_TABA_NO_4: Thaba4,
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/DeleteGeneralProcessPlanData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            if (response.message == "Success") {
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
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_010").text());
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                    centerVertical: true,
                    callback: function () {
                        $(cancelThirdForm).hide();
                        $(cancelFirstForm).show();
                        $("#chubbanNo1").val("");
                        $("#bundleNo1").val("");
                        $("#bundleNo2").val("");
                        $("#bundleNo3").val("");
                        $("#bundleNo4").val("");
                        $("#operatorCD").val("");
                        $("#processCD").val("");
                        $("#machineCD").val("");
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
            var message = $.trim($("#lblErrorMessageE_GENREALPROCESS_CANCEL_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

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