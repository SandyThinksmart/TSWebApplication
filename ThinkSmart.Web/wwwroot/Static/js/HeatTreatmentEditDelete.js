var typeDropDown = "select-one";
var typeText = "text";
var WARNING = "warning";
var disabled = "disabled";
var ERROR = "error";
var SUCCESS = "success";
var userName = '';
var gridRowsCount = 0;
var processPlanCompltedList = [];
var operatorCD = 0;
function EnableEditButton() {
    $("#btnEdit").removeAttr("disabled");
}

function DisableEditButton() {
    $("#btnEdit").attr("disabled", "disabled");
}

function EnableDeleteButton() {
    $("#btnDelete").removeAttr("disabled");
}
function DisableGridPanel() {
    $("#pnlDetails").css("pointer-events", "none");

}

function DisableHeaderPanel() {
    $("#pnlHeader").css("pointer-events", "none");

}
function EnableGridPanel() {

    $("#pnlDetails").css("pointer-events", "auto");

}

function EnableHeaderPanel() {
    $("#pnlHeader").css("pointer-events", "auto");
}


function DisableDeleteButton() {
    $("#btnDelete").attr("disabled", "disabled");
}

function setDecimalFormats() {

    for (var i = 1; i <= 30; i++) {
        $("#txtNoOfUnit" + i).keyup(function (event) {
            RestrictToValidDecimalFormat($(this), 5, 2)
        });

        $("#txtQuantity" + i).keyup(function (event) {
            RestrictToValidDecimalFormat($(this), 7, 2)
        });
    }
}

function btnSearchByFurnace_Click() {
    var machineCd = $("#ddlSrchMachine").val();
    var furNo = $("#txtSrchFurnaceNo").val();

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetHeatTreatmentStartDetails?furNo=" + yobina + "&machineCD=" + (machineCd ? machineCd : null),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                if (response.message.length > 0) {
                    SetStartDetails(response.message);
                    FillGrid(response.message);
                }
            }
            else {
                showMessageDialog(response.message, '', WARNING);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, '', ERROR);
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

function FillDropDown() {
    setMachineList();
    setProcConditionKubun();
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
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindMachineDropdown(jsonResult) {
    $("#ddlMachine").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMachine").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
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
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
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
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
    for (var i = 1; i <= 30; i++) {
        $('table#tblHeatTreatMent tbody tr').each(function (index, item) {
            var $tr = $(this);
            var $ddlNextProcess = $tr.find("#ddlNextProcess" + i);
            $ddlNextProcess.html("").append($('<option value="0"></option>'));
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

function setMatlType() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/GetMatlType",
        method: "POST",
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
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
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

function setProductUnitKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/GetProductUnitKubun",
        method: "POST",
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
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindproductunitkubundropdown(jsonResult) {
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
//            message = $("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text().trim();
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

function EnableDisableData(flag) {
    //End Data
    $("#txtFurnaceInDateTime").attr('disabled', true);
    $("#ddlFurnaceOutIncharge").attr('disabled', !flag);
    $("#ddlFurnaceVerifyBy").attr('disabled', !flag);
    $("#btnFurnaceOutDateTimeNow").attr('disabled', !flag);
    $("#btnEndDataSave").attr('disabled', !flag);

    // Start data
    $("#ddlMachine").attr('disabled', flag);
    $("#txtFurnaceNo").attr('disabled', flag);
    $("#ddlProcCondition").attr('disabled', flag);
    $("#txtPatternNo").attr('disabled', flag);
    $("#txtFurnaceInDateTime").attr('disabled', flag);
    $("#btnFurnaceInDateTimeNow").attr('disabled', flag);
    $("#ddlscAssembleVerifyBy").attr('disabled', flag);
    $("#ddlscFurncInVerifyBy").attr('disabled', flag);
    $("#txtPlannedUploadDate").attr('disabled', flag);
    $("#btnPlannedUploadDateNow").attr('disabled', flag);
    $("#btnStartDataSave").attr('disabled', flag);
    $("#btnSearchByChubaanPopup").attr('disabled', flag);
}

function FillGrid(jsonResult, bindGridDD = true) {

    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/FillGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                //  $("#divHeatTreatmentEntryMain").html(response1.message); 
                var result = response1.message;

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function FillEmptyGrid() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/FillEmptyGrid",
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
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function btnSearchByCodeSearchPopUp_Click() {
    var lstChuban = GetValidEntries();
    if (lstChuban.length > 0) {
        var ajaxTime = new Date().getTime();
        var jsonData = "{USER_NAME: '" + userName + "' }";
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/SearchByCodeSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(lstChuban)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                showMessageDialog(response.message, '', WARNING);
                if (response.isSuccess) {
                    FillGrid(response.data, false);
                }
                else {
                    FillEmptyGrid();
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
}

function GetValidEntries() {
    var lstChuban = [];
    $('table#tblHeatTreatMent tbody tr').each(function (index, item) {
        var $tr = $(this);
        var chuban = $tr.find("#txtOrderNo").val();
        if (chuban.trim()) {
            lstChuban.push(chuban);
        }
    });
    return lstChuban;
}

function btnSaveStartData_Click() {
    var jsonDataForSave = [];

    if (IsFillMandatorySaveStartData()) {
        if (IsFillGridViewMandatoryFields()) {
            if (!IsFurProcFurNoIsExist()) {
                $('table#tblHeatTreatMent tbody tr').each(function (index, item) {
                    var i = 1;
                    var $tr = $(this);
                    var data = {
                        HEAT_TREATMENT_ROW: i,
                        HEAT_TREATMENT_NUMBER: i,
                        MACHINE_CD: $("#ddlMachine").val() ? $("#ddlMachine").val() : null,
                        FURNACE_NO: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null,
                        PROC_CONDITION: $("#ddlProcCondition").val() ? $("#ddlProcCondition").val() : null,
                        PATTERN_NO: $("#txtPatternNo").val() ? $("#txtPatternNo").val() : null,

                        HT_START_DATE: ($("#txtFurnaceInDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceInDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                        SETUP_CONF_BY: $("#ddlscAssembleVerifyBy").val() ? $("#ddlscAssembleVerifyBy").val() : null,
                        LOADING_CONF_BY: $("#ddlscFurncInVerifyBy").val() ? $("#ddlscFurncInVerifyBy").val() : null,

                        PLANNED_UNLOAD_DATE: ($("#txtPlannedUploadDate").val().split(" ")[0] + "T" + $("#txtPlannedUploadDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                        HT_END_DATE: $("#txtFurnaceOutDateTime").val() ? ($("#txtFurnaceOutDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceOutDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-') : null,
                        OFFLOADED_BY: $("#ddlFurnaceOutIncharge").val() ? $("#ddlFurnaceOutIncharge").val() : null,

                        OFFLOAD_CONF_BY: $("#ddlFurnaceVerifyBy").val() ? $("#ddlFurnaceVerifyBy").val() : null,
                        PROCESS_PLAN_CD: $tr.find("#hdfProcessPlanCD").val() ? $tr.find("#hdfProcessPlanCD").val() : null,
                        COIL_BAR_TYPE: $tr.find("#ddlMatlType").val() ? $tr.find("#ddlMatlType").val() : null,

                        LOT_NO: $tr.find("#txtLotNo").val() ? $tr.find("#txtLotNo").val() : null,
                        HT_NO_OF_UNITS: $tr.find("#txtNoOfUnit").val() ? $tr.find("#txtNoOfUnit").val() : null,
                        UNIT_TYPE: $tr.find("#ddlUnit").val() ? $tr.find("#ddlUnit").val() : null,

                        HT_WEIGHT: $tr.find("#txtNoOfUnit").val() ? $tr.find("#txtNoOfUnit").val() : null,
                        ROLL_MONTH: $tr.find("#txtRoll").val() ? $tr.find("#txtRoll").val() : null,
                        COIL_NO: $tr.find("#txtCoilNo").val() ? $tr.find("#txtCoilNo").val() : null,

                        LOCATION_IN_FURNACE1: $tr.find("#txtFurnacePosition1").val() ? $tr.find("#txtFurnacePosition1").val() : null,
                        LOCATION_IN_FURNACE2: $tr.find("#txtFurnacePosition2").val() ? $tr.find("#txtFurnacePosition2").val() : null,
                        HT_TABA_NO_1: $tr.find("#txtTabaNo1").val() ? $tr.find("#txtTabaNo1").val() : null,


                        HT_TABA_NO_2: $tr.find("#txtTabaNo2").val() ? $tr.find("#txtTabaNo2").val() : null,
                        HT_TABA_NO_3: $tr.find("#txtTabaNo3").val() ? $tr.find("#txtTabaNo3").val() : null,
                        HT_TABA_NO_4: $tr.find("#txtTabaNo4").val() ? $tr.find("#txtTabaNo4").val() : null,
                        HT_NEXT_PROCESS: $tr.find("#ddlNextProcess").val() ? $tr.find("#ddlNextProcess").val() : null,
                        // USER_ID: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null
                    };
                    i++;
                    jsonDataForSave.push(data);
                });

                var ajaxTime = new Date().getTime();
                var jsonData = "{USER_NAME: '" + userName + "' }";
                var settings = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: APIURL + "/HeatTreatment/SaveStartData",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(jsonDataForSave)
                }
                $.ajax(settings)
                    .done(function (response) {
                        responseTime = new Date().getTime() - ajaxTime;
                        showMessageDialog(response.message, '', SUCCESS)
                        EnableDisableData(true);
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
        }
    }



}

function IsFillMandatorySaveStartData() {
    var ddlMachine = $("#ddlMachine").val();
    var txtFurnaceNo = $("#txtFurnaceNo").val();
    var ddlProcCondition = $("#ddlProcCondition").val();
    var txtFurnaceInDateTime = $("#txtFurnaceInDateTime").val();
    var scAssembleVerifyBy = $("#ddlscAssembleVerifyBy").val();
    var scFurncInVerifyBy = $("#scFurncInVerifyBy").val();

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/IsFillMandatorySaveStartData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: { ddlMachine: ddlMachine, txtFurnaceNo: txtFurnaceNo, ddlProcCondition: ddlProcCondition, txtFurnaceInDateTime: txtFurnaceInDateTime, scAssembleVerifyBy: scAssembleVerifyBy, scFurncInVerifyBy: scFurncInVerifyBy }
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                return true;
            }
            else {
                showMessageDialog(response.message, '', WARNING);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function IsFurProcFurNoIsExist() {
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
            return response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function IsMandatoryTextBoxFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";


    $.each($.find("#pnlHeader .MandatoryTextBox"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText) {
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

    $.each($.find(".Mandatory"), function (index, mandatoryField) {

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

function duplicateFurnaceNo() {



    var furnace = $("#hdfFurNo").val();


    var heatTreatmentNo = $("#hdfHTNumber").val();
    var furnaceNo;
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
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            furnaceNo = response.message.length;
            var List = jsonResult.filter(function (f) {
                return f.heaT_TREATMENT_NUMBER, f.furnacE_NO;
            });
            if ((List.length == 0) || (List[0].heaT_TREATMENT_NUMBER == heatTreatmentNo && List[0].furnacE_NO == furnace)) {
                returnVal = false;
            }
            else {
                returnVal = true;
            }

            //if (furnaceNo > 0) {

            //    returnVal = true;
            //}


        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, "", ERROR);
        });
    return returnVal;

}
function calculateTotalWeight() {

    for (var k = 1; k < 30; k++) {

        $("#txtQuantity" + k).keyup(function () {

            var quantity = 0;
            for (var a = 1; a < 30; a++) {
                if ($("#hdfProcessPlanCD" + a).val() !== "" && $("#txtQuantity" + a).val() != "") {
                    quantity = parseInt(quantity) + parseInt($("#txtQuantity" + a).val());
                }
            }
            $("#lblTotal").text(quantity);
        });
    }
}
function btnEdit_Click() {

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

    if (duplicateFurnaceNo()) {

        var message = $("#lblErrorMessageE_HT_EDIT_DELETE_003").text();
        showMessageDialog(message, "", WARNING);
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
    if (!checkDate($("#txtFurnaceOutDateTime"))) {
        var message = $("#lblErrorMessageE_HeatTreat_023").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtFurnaceOutDateTime", WARNING);
        return false;
    }

    if (!IsStartDateValidEdit()) {
        return false;
    }

    if (processPlanCompltedList.length > 0) {
        var message = "";
        for (var i = 1; i <= processPlanCompltedList.length; i++) {
            if (message == "") {
                message = "<br>" + processPlanCompltedList[parseInt(i - 1)].chuuban;
            } else {
                message = message + "<br>" + processPlanCompltedList[parseInt(i - 1)].chuuban;
            }

        }
        showMessageDialog($("#lblErrorMessageE_HeatTreat_024").text().trim().replace("99-99999999-99", message), "btnInitialize", WARNING);
        return false;
    }

    for (var i = 1; i <= 30; i++) {

        var aa = $("#hdfProcessPlanCD" + i).val();
        if ($("#hdfProcessPlanCD" + i).val() != "") {
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
        }

    }
    var messageVal = "";
    if (NextProcess != "" || FurnacePosition1 != "" || FurnacePosition2 != "") {
        debugger;
        //if (MatlType != "") {
        //    messageVal = $("#lblErrorMessageE_HeatTreat_015").text().trim().replace("NNN", MatlType);
        //}

        if (FurnacePosition1 != "") {
            if (messageVal == "") {
                messageVal = $("#lblErrorMessageE_HeatTreat_012").text().trim().replace("NNN", FurnacePosition1);
            } else {
                messageVal = messageVal + "<br>" + $("#lblErrorMessageE_HeatTreat_012").text().trim().replace("NNN", FurnacePosition1);
            }
        }

        if (FurnacePosition2 != "") {
            if (messageVal == "") {
                messageVal = $("#lblErrorMessageE_HeatTreat_013").text().trim().replace("NNN", FurnacePosition2);
            } else {
                messageVal = messageVal + "<br>" + $("#lblErrorMessageE_HeatTreat_013").text().trim().replace("NNN", FurnacePosition2);
            }
        }
        //if (bundleNo != "") {
        //    if (messageVal == "") {
        //        messageVal = $("#lblErrorMessageE_HeatTreat_025").text().trim().replace("NNN", bundleNo);
        //    } else {
        //        messageVal = messageVal + "<br>" + $("#lblErrorMessageE_HeatTreat_025").text().trim().replace("NNN", bundleNo);
        //    }
        //}
        if (NextProcess != "") {
            if (messageVal == "") {
                messageVal = $("#lblErrorMessageE_HeatTreat_014").text().trim().replace("NNN", NextProcess);
            } else {
                messageVal = messageVal + "<br>" + $("#lblErrorMessageE_HeatTreat_014").text().trim().replace("NNN", NextProcess);
            }
        }

        //if (LotNo != "") {
        //    var message = message + "<br>" + $("#lblErrorMessageE_HeatTreat_011").text().trim().replace("NNN", LotNo);
        //}
        debugger;
        showMessageDialog(messageVal, fieldToFocus, WARNING);
    }
    else {

        for (var i = 1; i <= 30; i++) {
            var aa = $("#hdfProcessPlanCD" + i).val();
            var bb = $("#hdfheattreatmentCD" + i).text();
            if ($("#hdfProcessPlanCD" + i).val() !== '') {
                var aa = $("#txtCustomer" + i).text();

                var ajaxTime = new Date().getTime();
                var jsonSaveStartData = ({

                    MACHINE_CD: $("#ddlMachine").val() ? $("#ddlMachine").val() : null,
                    FURNACE_NO: $("#txtFurnaceNo").val() ? $("#txtFurnaceNo").val() : null,
                    HT_CONDITION: $("#txtProcCondition").val() ? $("#txtProcCondition").val() : null,
                    PATTERN_NO: $("#txtPatternNo").val() ? $("#txtPatternNo").val() : null,

                    HT_START_DATE: ($("#txtFurnaceInDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceInDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    SETUP_CONF_BY: $("#ddlscAssembleVerifyBy").val() ? $("#ddlscAssembleVerifyBy").val() : null,
                    LOADING_CONF_BY: $("#ddlscFurncInVerifyBy").val() ? $("#ddlscFurncInVerifyBy").val() : null,
                    PLANNED_UNLOAD_DATE: ($("#txtPlannedUploadDate").val().split(" ")[0] + "T" + $("#txtPlannedUploadDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    HT_HEADER_REMARK: $("#txtHtHeaderRemark").val() ? $("#txtHtHeaderRemark").val() : null,
                    HT_END_DATE: ($("#txtFurnaceOutDateTime").val().split(" ")[0] + "T" + $("#txtFurnaceOutDateTime").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
                    OFFLOADED_BY: $("#ddlFurnaceOutIncharge").val() ? $("#ddlFurnaceOutIncharge").val() : null,
                    OFFLOAD_CONF_BY: $("#ddlFurnaceVerifyBy").val() ? $("#ddlFurnaceVerifyBy").val() : null,
                    USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null,

                    // Grid Values
                    PROCESS_PLAN_CD: $("#hdfProcessPlanCD" + i).val(),
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
        //var jsonData = "{USER_NAME: '" + userName + "' }";

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/UpdateEndData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(ListData)
        }
        $.ajax(settings)
            .done(function (response) {

                responseTime = new Date().getTime() - ajaxTime;
                var successMessage = $("#lblSuccessMessageS_HTEditDelete_001").text();
                showMessageDialog(successMessage, "saved", SUCCESS);
                var jsonResult = response.message;

                //clearData();
            })
            .fail(function (xhr, textStatus, errorThrown) {

                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });


    }
}
function IsStartDateValidEdit() {
    debugger;
    retval = true; // assume end data is valid


    var currentDate = $("#txtFurnaceInDateTime").val();


    var plannedupload = $("#txtPlannedUploadDate").val();
    var furnaceIn = $("#txtFurnaceInDateTime").val();
    var furnaceOut = $("#txtFurnaceOutDateTime").val();
    // if Furnace In Date Time >= CarriedOut Schedule
    if (Date.parse($("#txtPlannedUploadDate").val()) < Date.parse($("#txtFurnaceInDateTime").val())) {
        var message = $("#lblErrorMessageE_HeatTreat_027").text().trim();
        var focusFieldID = "txtPlannedUploadDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    } 
    if (Date.parse($("#txtFurnaceOutDateTime").val()) < Date.parse($("#txtFurnaceInDateTime").val())) {
        var message = $("#lblErrorMessageE_HeatTreat_035").text().trim();
        var focusFieldID = "txtPlannedUploadDate";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    return true;



}
function btnDelete_Click() {
    if (processPlanCompltedList.length > 0) {
        var message = "";
        for (var i = 1; i <= processPlanCompltedList.length; i++) {
            if (message == "") {
                message = "<br>" + processPlanCompltedList[parseInt(i - 1)].chuuban;
            } else {
                message = message + "<br>" + processPlanCompltedList[parseInt(i - 1)].chuuban;
            }

        }
        showMessageDialog($("#lblErrorMessageE_HeatTreat_024").text().trim().replace("99-99999999-99", message), "btnInitialize", WARNING);
        return false;
    }
    
    var HTNumberCD = parseInt($("#hdfHTNumber").val());
    var machineCD = parseInt($("#hdfMachineCD").val());
    var furNo = $("#hdfFurNo").val();
    var ListData = [];

    for (var i = 1; i <= 30; i++) {

        if ($("#hdfProcessPlanCD" + i).val() !== '') {
            var ajaxTime = new Date().getTime();
            var jsonDeleteData = ({
                HEAT_TREATMENT_NUMBER: $("#hdfHTNumber").val() ? $("#hdfHTNumber").val() : null,
                MACHINE_CD: $("#hdfMachineCD").val() ? $("#hdfMachineCD").val() : null,
                FURNACE_NO: $("#hdfFurNo").val() ? $("#hdfFurNo").val() : null,
                PROCESS_PLAN_CD: $("#hdfProcessPlanCD" + i).val(),
                MSG_CD: $("#hdfMsgCD" + i).text() ? $("#hdfMsgCD" + i).text() : null,
                HT_TABA_NO_1: $("#txtTabaNo1" + i).val() ? $("#txtTabaNo1" + i).val() : null,
                HT_TABA_NO_2: $("#txtTabaNo2" + i).val() ? $("#txtTabaNo2" + i).val() : null,
                HT_TABA_NO_3: $("#txtTabaNo3" + i).val() ? $("#txtTabaNo3" + i).val() : null,
                HT_TABA_NO_4: $("#txtTabaNo4" + i).val() ? $("#txtTabaNo4" + i).val() : null

            });
            ListData.push(jsonDeleteData)
        }
    }
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/Delete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(ListData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var successMessage = $("#lblSuccessMessageS_HTEditDelete_DataDeleted").text();
            showMessageDialog(successMessage, 'btnInitialize', SUCCESS);
            DisableDeleteButton();
            DisableEditButton();
            //  Clear();

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
            showMessageDialog(message, '', ERROR);
        });
}

function Clear() {
    $("#hdfMachineCD").val("");
    $("#hdfFurNo").val("");
    clearPanel($("#pnlHeader"));
    clearPanel($("#pnlDetails"));
    DisableDeleteButton();
    DisableEditButton();

    for (var i = 0; i <= 31; i++) {

        $("#lblCustomer" + i).text("");
        $("#lblProdNameCust" + i).text("");
        $("#lblCharge" + i).text("");
        $("#lblMatlDim" + i).text("");
        $("#lblProdDim" + i).text("");

      //  $("#ddlMatlType" + i).removeClass("Mandatory2", "Mandatory2");
        // $("#txtLotNo" + i).removeClass("Mandatory2", "Mandatory2");
        $("#txtFurnacePosition1" + i).removeClass("Mandatory2", "Mandatory2");
        $("#txtFurnacePosition2" + i).removeClass("Mandatory2", "Mandatory2");
        //$("#txtTabaNo1" + i).removeClass("Mandatory2", "Mandatory2");
        $("#ddlNextProcess" + i).removeClass("Mandatory2", "Mandatory2");
    }
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
                params_arr.splice(i, 2);
            }
        }
        if (params_arr.length) rtn = rtn + "?" + params_arr.join("&");
    }
    return rtn;
}
$("#btnInitialize").click(function (event) {
    $("#hdfMachineCD").val("");
    $("#hdfFurNo").val("");
    processPlanCompltedList = [];

    location.href = removeParameter("furNo", window.location.href);
    //Clear();
    $("#ddlMachine").focus();

});

function removeParam(key, sourceURL) {
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

function checkDate(field) {
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    //re = /^(\d{4})\/(\d{1,2})\/(\d{1,2})$/;
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    if (field.val() != '') {
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
        }
    }
}

function GetInactiveEmp() {
    var result = false;
    var FurnaceVerifyBy = $("#ddlFurnaceVerifyBy").val();
    var FurnaceOutIncharge = $("#ddlFurnaceOutIncharge").val();
    var AssembleVerifyBy = $("#ddlscAssembleVerifyBy").val();
    var FurncInVerifyBy = $("#ddlscFurncInVerifyBy").val();
    if (FurnaceVerifyBy != '0') {
        IsOperatorActiveInactive(FurnaceVerifyBy);
        result = true;
    }

    if (FurnaceOutIncharge != '0') {
        IsOperatorActiveInactive(FurnaceOutIncharge);
        result = true;
    }
    if (AssembleVerifyBy != '0') {
        IsOperatorActiveInactive(AssembleVerifyBy);
        result = true;
    }

    if (FurncInVerifyBy != '0') {
        IsOperatorActiveInactive(FurncInVerifyBy);
        result = true;
    }
    return false;
}

function IsOperatorActiveInactive(cd) {
    var retvalue;
    var jsonresult;
    var ajaxTime = new Date().getTime();
    var operatorCD = cd; // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var jsonData = "{OperatorCD: '" + operatorCD + "', USER_NAME : '" + userName + "'}"

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/CheckOperatorIncativeActive?OperatorCD=" + operatorCD,
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }

    $.ajax(settings)
        .done(function (response) {


            responseTime = new Date().getTime() - ajaxTime;

            //return retvalue;
            jsonresult = response;
            var operatorStatus = response.message.value;

            if (jsonresult.isSuccess == true) {

                retvalue = true;
            }
            else {

                showMessageDialog(response.message, "", WARNING);
                retvalue = false;
            }
        })
    return retvalue;

}

function bindEvent() {

    setDecimalFormats();
    setNextProcess();
    setMatlType();
    setProductUnitKubun();
    DisableDeleteButton();
    DisableEditButton();

    //$("#btnscAssembleVerifyByOpeartorSearch").click(function (event) {
    //    var yobina = $.trim($("#txtscAssembleVerifyByInputSearch").val());
    //    getOperatorList(yobina, $("#ddlscAssembleVerifyBy"));
    //});
    //$("#btnscFurncInVerifyByOpeartorSearch").click(function (event) {
    //    var yobina = $.trim($("#txtscFurncInVerifyByInputSearch").val());
    //    getOperatorList(yobina, $("#ddlscFurncInVerifyBy"));
    //});
    //$("#btnFurnaceOutInchargeSearch").click(function (event) {
    //    var yobina = $.trim($("#txtFurnaceOutInchargeInputSearch").val());
    //    getOperatorList(yobina, $("#ddlFurnaceOutIncharge"));
    //});
    //$("#btnFurnaceVerifyBySearch").click(function (event) {
    //    var yobina = $.trim($("#txtFurnaceVerifyByInputSearch").val());
    //    getOperatorList(yobina, $("#ddlFurnaceVerifyBy"));
    //});
}

function SetStartDetails(jsonResult) {
    var startDetails = jsonResult[0];
    //Start Details
    $("#ddlMachine").val(startDetails.machinE_CD);
    $("#txtFurnaceNo").val(startDetails.furnacE_NO);
    $("#ddlProcCondition").val(startDetails.hT_CONDITION);
    $("#txtProcCondition").val(startDetails.hT_CONDITION);
    $("#txtPatternNo").val(startDetails.patterN_NO);
    $("#txtFurnaceInDateTime").val($.format.date(new Date(startDetails.hT_START_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    SetOperatorDropdown($("#ddlscAssembleVerifyBy"), startDetails.setuP_CONF_BY);
    SetOperatorDropdown($("#ddlscFurncInVerifyBy"), startDetails.loadinG_CONF_BY);
    //$("#ddlscAssembleVerifyBy").val(startDetails.setuP_CONF_BY);
    //$("#ddlscFurncInVerifyBy").val(startDetails.loadinG_CONF_BY);
    $("#txtPlannedUploadDate").val($.format.date(new Date(startDetails.planneD_UNLOAD_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //End Details
    $("#txtFurnaceOutDateTime").val($.format.date(new Date(startDetails.hT_END_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    SetOperatorDropdown($("#ddlFurnaceOutIncharge"), startDetails.offloadeD_BY != null ? startDetails.offloadeD_BY : 0);
    SetOperatorDropdown($("#ddlFurnaceVerifyBy"), startDetails.offloaD_CONF_BY != null ? startDetails.offloaD_CONF_BY : 0);
    //$("#ddlFurnaceOutIncharge").val(startDetails.offloadeD_BY != null ? startDetails.offloadeD_BY : 0);
    //$("#ddlFurnaceVerifyBy").val(startDetails.offloaD_CONF_BY != null ? startDetails.offloaD_CONF_BY : 0);
    // $("#txtProcessingTime").val($.format.date(new Date(startDetails.planneD_UNLOAD_DATE), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#txtHtHeaderRemark").val(startDetails.hT_HEADER_REMARK);
}

function GetHeatTreatmentDetails(tempCD) {
    debugger;
    var HeatTreatMentCd = tempCD;

    var furNo = $("#hdfFurNo").val();
    if (HeatTreatMentCd > 0 && furNo) {
        EnableGridPanel();
        EnableHeaderPanel();
        calculateTotalWeight();
        FillDropDown();
        //getOperatorList("", $("#ddlscAssembleVerifyBy"));
        //getOperatorList("", $("#ddlscFurncInVerifyBy"));
        //getOperatorList("", $("#ddlFurnaceOutIncharge"));
        //getOperatorList("", $("#ddlFurnaceVerifyBy"));


        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/GetHeatTreatmentEditDeleteStartEndDetails?furNo=" + furNo + "&HeatTreatMentCd=" + HeatTreatMentCd + "&isNull=" + false,
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {

                    if (response.message.length > 0) {
                        gridRowsCount = response.message.length;
                        SetStartDetails(response.message);
                        var result = response.message;
                        processPlanCompltedList = result.filter(r => r.pP_PROCESS_PLAN_CMPL_DATE_STRING != null);
                        $("#hdfFurNo").val(result[0].furnacE_NO);
                        $("#hdfHTNumber").val(result[0].heaT_TREATMENT_NUMBER);

                        var j = 0;
                        var sumVal = 0;
                        for (var i = 0; i <= response.message.length - 1; i++) {

                            j = parseInt(i + 1);
                            $("#txtOrderNo" + j).val(result[i].chuuban);
                            $("#txtOrderNo" + k).removeClass('Mandatory2');
                            $("#txtCustomer" + j).text(result[i].customeR_NAME);
                            $("#lblProdNameCust" + j).text(result[i].proD_NAME_CUST);
                            $("#lblCharge" + j).text(result[i].charge);
                            $("#lblMatlDim" + j).text(result[i].matL_DIM_STRING);
                            $("#lblProdDim" + j).text(result[i].proD_DIM_STRING);
                            //$("#ddlMatlType" + j).val(result[i].coiL_BAR_TYPE);
                            $("#ddlMatlType" + j).val(result[i].coiL_BAR_TYPE);
                       //     $("#ddlMatlType" + j).addClass("Mandatory2", "Mandatory2");
                            $("#txtLotNo" + j).val(result[i].loT_NO);
                            //  $("#txtLotNo" + j).addClass("Mandatory2", "Mandatory2");
                            $("#txtNoOfUnit" + j).val(result[i].hT_NO_OF_UNITS);
                            $("#ddlUnit" + j).val(result[i].uniT_TYPE);
                            $("#txtQuantity" + j).val(result[i].hT_WEIGHT);

                            $("#txtRemark" + j).val(result[i].hT_REMARKS);
                            $("#txtRoll" + j).val(result[i].rolL_MONTH);
                            $("#txtCoilNo" + j).val(result[i].coiL_NO);
                            $("#txtFurnacePosition1" + j).val(result[i].locatioN_IN_FURNACE1);
                            $("#txtFurnacePosition1" + j).addClass("Mandatory2", "Mandatory2");
                            $("#txtFurnacePosition2" + j).val(result[i].locatioN_IN_FURNACE2);
                            $("#txtFurnacePosition2" + j).addClass("Mandatory2", "Mandatory2");
                            $("#txtTabaNo1" + j).val(result[i].hT_TABA_NO_1);
                            //$("#txtTabaNo1" + j).addClass("Mandatory2", "Mandatory2");
                            $("#txtTabaNo2" + j).val(result[i].hT_TABA_NO_2);
                            $("#txtTabaNo3" + j).val(result[i].hT_TABA_NO_3);
                            $("#txtTabaNo4" + j).val(result[i].hT_TABA_NO_4);
                            $("#ddlNextProcess" + j).val(result[i].hT_NEXT_PROCESS);
                            $("#ddlNextProcess" + j).addClass("Mandatory2", "Mandatory2");
                            $("#hdfheattreatmentCD" + j).text(result[i].heaT_TREATMENT_CD);
                            $('#hdfProcessPlanCD' + j).val(result[i].procesS_PLAN_CD);


                            CalculateRequiredHours();
                            EnableDeleteButton();
                            EnableEditButton();
                            $("#ddlMachine").focus();
                            if ($("#hdfProcessPlanCD" + j).val() !== "" && $("#txtQuantity" + j).val() != "") {
                                sumVal = parseInt(sumVal) + parseInt($("#txtQuantity" + j).val());
                            }
                        }
                        $("#lblTotal").text(sumVal);
                        for (var k = gridRowsCount + 1; k <= 30; k++) {
                            $("#txtOrderNo" + k).prop('disabled', true);
                            $("#txtOrderNo" + k).removeClass('Mandatory2');
                            $("#txtCustomer" + k).prop('disabled', true);
                            $("#lblProdNameCust" + k).prop('disabled', true);
                            $("#lblCharge" + k).prop('disabled', true);
                            $("#lblMatlDim" + k).prop('disabled', true);
                            $("#lblProdDim" + k).prop('disabled', true);

                            $("#ddlMatlType" + k).prop('disabled', true);
                            $("#ddlMatlType" + k).prop('disabled', true);
                            $("#txtLotNo" + k).prop('disabled', true);

                            $("#txtNoOfUnit" + k).prop('disabled', true);
                            $("#ddlUnit" + k).prop('disabled', true);
                            $("#txtQuantity" + k).prop('disabled', true);

                            $("#txtRemark" + k).prop('disabled', true);
                            $("#txtRoll" + k).prop('disabled', true);
                            $("#txtCoilNo" + k).prop('disabled', true);
                            $("#txtFurnacePosition1" + k).prop('disabled', true);
                            $("#txtFurnacePosition1" + k).prop('disabled', true);
                            $("#txtFurnacePosition2" + k).prop('disabled', true);
                            $("#txtFurnacePosition2" + k).prop('disabled', true);
                            $("#txtTabaNo1" + k).prop('disabled', true);

                            $("#txtTabaNo2" + k).prop('disabled', true);
                            $("#txtTabaNo3" + k).prop('disabled', true);
                            $("#txtTabaNo4" + k).prop('disabled', true);
                            $("#ddlNextProcess" + k).prop('disabled', true);
                            $("#ddlNextProcess" + k).prop('disabled', true);

                        }
                    }
                }
                else {
                    showMessageDialog(response.message, '', WARNING);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HeatTreatment_EDIT_DELETE_001").text());
                showMessageDialog(message, '', ERROR);
            });
    }
    else {
        FillEmptyGrid();
        FillDropDown();
        //FillGridDropDown();

    }
}

$("#ddlFurnaceVerifyBy").focusout(function (event) {
    $("#txtHtHeaderRemark").focus();
});

$("#txtHtHeaderRemark").focusout(function (event) {
    $("#ddlMatlType1").focus();
});
txtHtHeaderRemark

$("#txtFurnaceOutDateTime").change(function (event) {

    var furnaceOutDateTime = $('#txtFurnaceOutDateTime').val();
    CalculateRequiredHours();
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
function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('HeatTreatMentCd=');
    result = sParameterName[1];

    return result;
}
$(document).ready(function () {
   
    debugger;
    $("#tblHeatTreatMent tfoot td[colspan='10']").attr("colspan", 9);
    $("#tblHeatTreatMent tfoot td[colspan='11']").attr("colspan", 10);
    DisableGridPanel();
    DisableHeaderPanel();
    let tempCD = GetURLParameters(window.location.href).trim();

   // GetHeatTreatmentDetails();
   
    window.resizeTo(1580, 1000);
    bindEvent();

    if (parseInt(tempCD) > 0) {
        GetHeatTreatmentDetails(tempCD);
    }


    //var uri = window.location.toString();
    //if (uri.indexOf("?") > 0) {
    //    var clean_uri = uri.substring(0, uri.indexOf("?"));
    //    window.history.replaceState({}, document.title, clean_uri);
    //}
    $("#btnSelect").focus();

    for (let i = 1; i < 31; i++) {
        $("#txtOrderNo" + i).attr(disabled, disabled).addClass(disabled);
        $("#txtOrderNo" + i).removeClass('Mandatory2');
    }
    clearGridCheckbox();
});


function clearGridCheckbox() {
    debugger;

    const cell = document.getElementById("chkTitle");
    cell.colSpan = 12;
    document.getElementById("chkheader").style.display = "none";
    for (var i = 1; i <= 30; i++) {

        var chkId = ('chkRow' + i);
        document.getElementById(chkId).style.display = "none";
    }



}
