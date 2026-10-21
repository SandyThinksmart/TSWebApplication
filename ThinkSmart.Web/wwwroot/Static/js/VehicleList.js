var typeDropDown = "select-one";
var DISABLED = "disabled";
var typeText = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var gridRowsCount = 0;
var flagValue = false; // IF FLAG = TRUE THEN SELECT DATA GROUP NUMBER WISER, FLAG= FALSE THEN SELECT DATA AS PER SEARCH PANEL EX- MAKER CD, MUKESAKI CD, YEAR, MONTH, CHANCE
$(document).ready(function () {
    disableCopyButtons();
    disableEditButtons();
    disableDeleteButtons();
    disableUnUseButtons();


    $("#lblMasterName").text($("#lblTitle_MakerMaterial_001").text());
    /* $("#tdbtnCopy").css("display", "none");*/
    SetDefaultDate();
    SetMaterialType();
    SetMakerList();
    debugger;
    let groupNo = getURLParameters(window.location.href).trim();

    if (parseInt(groupNo) > 0) {
        $("#btnMukesakiSearch").click();
        GetMakerMaterialDetailsByCD(groupNo)
        $("#hdfGroupNo").val(groupNo);
        flagValue = true;
        disableSearchPanel();

    }
    $("#ddlMakerName").focus();

});
function disableGrid() {

    var $startDataPanel = $("#tblMatrial");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        if ($(textBox).val() == "") {
            $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
        }


    });

}


function enableGrid() {

    var $startDataPanel = $("#tblMatrial");

    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

}


$(document).on('keyup', '.txtRate', function (e) {
    RestrictToValidDecimalFormat($("#" + e.target.id), 7, 4);
});
$(document).on('keyup', '.txtDimStart', function (e) {
    RestrictToValidDecimalFormat($("#" + e.target.id), 4, 3);
});
$(document).on('keyup', '.txtDimEnd', function (e) {
    RestrictToValidDecimalFormat($("#" + e.target.id), 4, 3);
});
function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('groupNo=');
    result = sParameterName[1];

    return result;
}

function disableSearchPanel() {
    $("#ddlMakerName").attr("disabled", "disabled");
    $("#txtMukesakiName").attr("disabled", "disabled");
    $("#btnMukesakiSearch").attr("disabled", "disabled");
    $("#ddlMukesakiName").attr("disabled", "disabled");
    $("#ddlMaterialType").attr("disabled", "disabled");
    $("#txtYear").attr("disabled", "disabled");
    $("#txtMonth").attr("disabled", "disabled");
    $("#txtChance").attr("disabled", "disabled");

}
function enableSearchPanel() {
    $("#ddlMakerName").removeAttr("disabled", "disabled");
    $("#txtMukesakiName").removeAttr("disabled", "disabled");
    $("#btnMukesakiSearch").removeAttr("disabled", "disabled");
    $("#ddlMukesakiName").removeAttr("disabled", "disabled");
    $("#ddlMaterialType").removeAttr("disabled", "disabled");
    $("#txtYear").removeAttr("disabled", "disabled");
    $("#txtMonth").removeAttr("disabled", "disabled");
    $("#txtChance").removeAttr("disabled", "disabled");
}
function disableEditButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
}
function disableCopyButtons() {
    $("#btnCopy").attr("disabled", "disabled");
}

function disableUnUseButtons() {
    $("#btnUnUse").attr("disabled", "disabled");
}
function disableDeleteButtons() {
    $("#btnDelete").attr("disabled", "disabled");
}
function disableSaveButtons() {
    $("#btnRegister").attr("disabled", "disabled");
}

function enableEditButtons() {
    $("#btnUpdate").removeAttr("disabled", "disabled");
}
function enableDeleteButtons() {
    $("#btnDelete").removeAttr("disabled", "disabled");
}
function enableSaveButtons() {
    $("#btnRegister").removeAttr("disabled", "disabled");
}
function SetDefaultDate() {
    var currentTime = new Date()
    $("#txtYear").val(currentTime.getFullYear());

    var getMonth = currentTime.getMonth() + 1;

    if (getMonth.toString().length == 1) {
        getMonth = "0" + getMonth
    }
    $("#txtMonth").val(getMonth);
    $("#txtChance").val("");

}
function SetMaterialType() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/MakerMaterial/GetMaterialType",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMaterialTypeDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMaterialTypeDropdown(jsonResult) {
    $("#ddlMaterialType").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMaterialType").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function SetMakerList() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMaterial/GetMakerList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMakerDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMakerDropdown(jsonResult) {
    $("#ddlMakerName").html("").append($('<option value="0"></option>'));;
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMakerName").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function SetMukesakiList(yobina) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMaterial/GetMukesakiList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMukesakiDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMukesakiDropdown(jsonResult) {
    $("#ddlMukesakiName").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMukesakiName").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function btnInitialize_Click() {
    var url = document.location.href;
    window.history.pushState({}, "", url.split("?")[0]);
    Initialize();

}

function Initialize() {
    var arrayList = [];
    SetDefaultDate();
    disableEditButtons();
    disableDeleteButtons();
    enableSearchPanel();
    $("#hdfGroupNo").val("");
    $("#ddlMakerName").val(0);
    $("#ddlMaterialType option:first").attr('selected', 'selected');
    $("#ddlMukesakiName").html("").append($('<option value="0"></option>'));

    FillMaterialGrid(arrayList);
}
function btnSelect_Click() {
    window.location.href = "/VehicleMaster/VehicleList";
}

$("#btnMukesakiSearch").click(function () {
    var yobina = $("#txtMukesakiName").val() ? $("#txtMukesakiName").val() : "";
    SetMukesakiList(yobina);
});

function btnExtract_Click() { 
/*$("#btnExtractGrid").click(function () {*/
    if ($("#txtYear").val() == "" || $("#txtMonth").val() == "" || $("#txtChance").val() == "") {
        $("#txtCheckApplicationDate").addClass("Mandatory");
    } else {
        $("#txtCheckApplicationDate").removeClass("Mandatory");
    }

    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }
    debugger;

    var jsonData = JSON.stringify({
        MAKER_CD: $("#ddlMakerName").val(),
        MUKESAKI_CD: $("#ddlMukesakiName").val(),
        MATL_KUBUN: $("#ddlMaterialType").val(),
        APPLY_ROLL_YEAR: $("#txtYear").val(),
        APPLY_ROLL_MONTH: $("#txtMonth").val(),
        APPLY_ROLL_CHANCE: $("#txtChance").val()
    });

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMaterial/GetMaterialDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess) {
                gridRowsCount = response.message.length;
                if (gridRowsCount > 0) {
                    disableSearchPanel();
                    var resultData = response.message;
                    var isRateExist = resultData.filter(function (item) {
                        return item.rate > 0
                    });
                    if (isRateExist.length > 0) {
                        disableSaveButtons();
                    } else {
                        enableSaveButtons();
                    }


                } else {
                    enableSearchPanel();
                    disableSaveButtons();

                }

                FillMaterialGrid(response.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
});

function FillMaterialGrid(result) {
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/MakerMaterial/FillGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(result)

    }
    $.ajax(settings1)
        .done(function (response) {
            if (response.isSuccess) {
                $("#divMaterialMainGrid").html(response.message);

                if ($("#ddlMaterialType").val() == 1) {
                    $("tbody tr td input[type='text'][id^='txtDimStart']").removeClass("Mandatory");
                    $("tbody tr td input[type='text'][id^='txtDimEnd']").removeClass("Mandatory");
                } else {

                    $("tbody tr td input[type='text'][id^='txtDimStart']").addClass("Mandatory");
                    $("tbody tr td input[type='text'][id^='txtDimEnd']").addClass("Mandatory");
                }
                $("#txtDimStart1").focus();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function btnRegister_Click() {
    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }
    var isGridRateEmpty = true;
    var listData = [];
    var dimStartList = [];
    var dimEndList = [];
    var rateList = [];
    if (gridRowsCount > 0) {
        for (var i = 1; i <= gridRowsCount; i++) {

            if ($("#txtDimStart" + i).val() == "" && $("#txtDimEnd" + i).val() != "" && $("#ddlMaterialType").val() == 2) {
                dimStartList.push(i);
            }
            if ($("#txtDimStart" + i).val() != "" && $("#txtDimEnd" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
                dimEndList.push(i);
            }

            if ($("#txtDimStart" + i).val() != "" && $("#txtDimEnd" + i).val() != "" && $("#txtPrice" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
                rateList.push(i);
            }

            if ($("#txtPrice" + i).val() > 0) {

                var gridData = ({
                    MAKER_CD: $("#ddlMakerName").val(),
                    MUKESAKI_CD: $("#ddlMukesakiName").val(),
                    APPLY_ROLL_YEAR: $("#txtYear").val(),
                    APPLY_ROLL_MONTH: $("#txtMonth").val(),
                    APPLY_ROLL_CHANCE: $("#txtChance").val(),
                    MATL_KUBUN: $("#ddlMaterialType").val(),
                    MATL_NAME_MAKER_CD: $("#lblMatlNameMakerCD" + i).text(),
                    DIM_START: $("#txtDimStart" + i).val() ? $("#txtDimStart" + i).val() : null,
                    DIM_END: $("#txtDimEnd" + i).val() ? $("#txtDimEnd" + i).val() : null,
                    RATE: $("#txtPrice" + i).val(),
                    USER_ID: $("#hdfLoginId").val()
                });

                if ($("#txtDimStart" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
                    dimStartList.push(i);
                } else if ($("#txtDimEnd" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
                    dimEndList.push(i);
                }
                listData.push(gridData);
                isGridRateEmpty = false;

            }
        }
    }


    if (isGridRateEmpty == true) {
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_002").text()), "txtPrice1", WARNING);
        return false;

    }

    if (dimStartList.length > 0) {
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_003").text().replace("NNN", dimStartList.toString())), ("txtDimStart" + dimStartList[0]), WARNING);
        return false;
    }

    if (dimEndList.length > 0) {
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_004").text().replace("NNN", dimEndList.toString())), ("txtDimEnd" + dimEndList[0]), WARNING);
        return false;
    }

    if (rateList.length > 0) {
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_005").text().replace("NNN", rateList.toString())), ("txtPrice" + rateList[0]), WARNING);
        return false;
    }

    if (listData.length > 0) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/MakerMaterial/SaveMakerMaterialDetails",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(listData)

        }
        $.ajax(settings)
            .done(function (response) {
                disableSearchPanel();
                disableSaveButtons();
                enableDeleteButtons();
                enableEditButtons();
                flagValue = false;
                var jsonResult = response.message;
                var matlGroupCD = jsonResult.find(x => x.matL_GROUP_NO !== null)?.matL_GROUP_NO;
                $("#hdfGroupNo").val(matlGroupCD);// GROUP NO SET TO HIDDEN FILED BCZ DELETE CODE IMPLEMENTED BY GROUP NO
                FillMaterialGrid(response.message);
                var successMessage = $("#lblSuccessMessageS_Maker_MaterialEntry_001").text();
                showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }
}
function GetMakerMaterialDetailsByCD(groupNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMaterial/GetMaterialDetailsByGroupNo?groupNo=" + groupNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;


            if (response.message.length > 0) {
                gridRowsCount = response.message.length;
                $("#txtYear").val(jsonResult[0].applY_ROLL_YEAR);
                if (jsonResult[0].applY_ROLL_MONTH.toString().length == 1) {
                    jsonResult[0].applY_ROLL_MONTH = "0" + jsonResult[0].applY_ROLL_MONTH;
                }
                $("#txtMonth").val(jsonResult[0].applY_ROLL_MONTH);
                $("#txtChance").val(jsonResult[0].applY_ROLL_CHANCE);
                $("#ddlMukesakiName").val(jsonResult[0].mukesakI_CD);
                $("#ddlMakerName").val(jsonResult[0].makeR_CD);
                $("#ddlMaterialType").val(jsonResult[0].matL_KUBUN);
                enableEditButtons();
                enableDeleteButtons();


                FillMaterialGrid(response.message);

            } else {
                var emptyList = [];
                gridRowsCount = 0;

                FillMaterialGrid(emptyList);
                disableEditButtons();
                disableDeleteButtons();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function btnUpdate_Click() {
    var isGridRateEmpty = true;
    var arrayList = [];
    var dimStartList = [];
    var dimEndList = [];
    var rateList = [];

    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }
    debugger;
    if (parseInt(gridRowsCount) <= 0) {
        return false;
    }
    for (let i = 1; i <= gridRowsCount; i++) {
        if ($("#txtDimStart" + i).val() == "" && $("#txtDimEnd" + i).val() != "" && $("#ddlMaterialType").val() == 2) {
            dimStartList.push(i);
        }
        if ($("#txtDimStart" + i).val() != "" && $("#txtDimEnd" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
            dimEndList.push(i);
        }

        if ($("#txtDimStart" + i).val() != "" && $("#txtDimEnd" + i).val() != "" && $("#txtPrice" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
            rateList.push(i);
        }

        if ($("#txtPrice" + i).val() > 0) {
            var gridData = ({
                // HT_MAT_C_RATE_CD: $("#lblRateCD" + i).text(),
                SELECT_EVENT_FLAG: flagValue,
                MATL_GROUP_NO: $("#hdfGroupNo").val(),
                MAKER_CD: $("#ddlMakerName").val(),
                MUKESAKI_CD: $("#ddlMukesakiName").val(),
                MATL_NAME_MAKER_CD: $("#lblMatlNameMakerCD" + i).text(),
                MATL_KUBUN: $("#ddlMaterialType").val(),
                APPLY_ROLL_YEAR: $("#txtYear").val(),
                APPLY_ROLL_MONTH: $("#txtMonth").val(),
                APPLY_ROLL_CHANCE: $("#txtChance").val(),
                DIM_START: $("#txtDimStart" + i).val() ? $("#txtDimStart" + i).val() : null,
                DIM_END: $("#txtDimEnd" + i).val() ? $("#txtDimEnd" + i).val() : null,
                RATE: $("#txtPrice" + i).val(),
                USER_ID: $("#hdfLoginId").val()
            });
            if ($("#txtDimStart" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
                dimStartList.push(i);
            } else if ($("#txtDimEnd" + i).val() == "" && $("#ddlMaterialType").val() == 2) {
                dimEndList.push(i);
            }

            arrayList.push(gridData);
            isGridRateEmpty = false;
        }

    }
    if (isGridRateEmpty == true) {

        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_002").text()), "txtPrice1", WARNING);
        return false;

    }
    if (dimStartList.length > 0) {
        var uniqueDimStart = [...new Set(dimStartList)];
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_003").text().replace("NNN", uniqueDimStart.toString())), ("txtDimStart" + uniqueDimStart[0]), WARNING);
        return false;
    }

    if (dimEndList.length > 0) {
        var uniqueDimEnd = [...new Set(dimEndList)];
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_004").text().replace("NNN", uniqueDimEnd.toString())), ("txtDimEnd" + uniqueDimEnd[0]), WARNING);
        return false;
    }

    if (rateList.length > 0) {
        var uniqueRate = [...new Set(rateList)];
        showMessageDialog($.trim($("#lblErrorMessageE_Maker_MaterialEntry_005").text().replace("NNN", uniqueRate.toString())), ("txtPrice" + uniqueRate[0]), WARNING);
        return false;
    }

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMaterial/UpdateMakerMaterialDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(arrayList)

    }
    $.ajax(settings)
        .done(function (response) {
            FillMaterialGrid(response.message);
            if (response.message.length <= 0) {
                disableEditButtons();
                disableDeleteButtons();
            } else {
                gridRowsCount = response.message.length;
                var jsonResult = response.message;
                var matlGroupCD = jsonResult.find(x => x.matL_GROUP_NO !== null)?.matL_GROUP_NO;
                $("#hdfGroupNo").val(matlGroupCD); // GROUP NO SET TO HIDDEN FILED BCZ DELETE CODE IMPLEMENTED BY GROUP NO
                let url = new URL(window.location.href);

                // replace or add parameter
                url.searchParams.set("groupNo", matlGroupCD);

                // update URL in browser without reload
                window.history.replaceState({}, "", url);
            }
            var successMessage = $("#lblSuccessMessageS_Maker_MaterialEntry_002").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function btnDelete_Click() {

    var deleteData = ({
        MUKESAKI_CD: $("#ddlMukesakiName").val(),
        APPLY_ROLL_YEAR: $("#txtYear").val(),
        APPLY_ROLL_MONTH: $("#txtMonth").val(),
        APPLY_ROLL_CHANCE: $("#txtChance").val()
    });

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: APIURL + "/MakerMaterial/DeleteMakerMaterialDetails?groupNo=" + $("#hdfGroupNo").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify(deleteData)

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            disableDeleteButtons();
            disableEditButtons();
            var successMessage = $("#lblSuccessMessageS_Maker_MaterialEntry_003").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialEntry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function btnActiveInactive_Click() {

}
function IsMandatorySearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#divSearchCondition .Mandatory"), function (index, mandatoryField) {

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

    if (focusFieldID == "txtCheckApplicationDate") {
        if ($("#txtYear").val() == "") {
            focusFieldID = "txtYear";
        } else if ($("#txtMonth").val() == "") {
            focusFieldID = "txtMonth";
        } else {
            focusFieldID = "txtChance";
        }
    }

    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
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
function btnCloseMaster_Click() {
    self.close();
}
$('#tblMatrial').on('keydown', 'input[type="text"]', function (e) {
    if (e.key === 'Enter') {
        e.preventDefault();

        let $td = $(this).closest('td');
        let $tr = $(this).closest('tr');
        let colIndex = $td.index() + 1; // column number (1,2,3)

        if (colIndex === 1) {
            // Move to column 2
            $tr.find('td:nth-child(2) input').focus();
        }
        else if (colIndex === 2) {
            // Move to column 3
            $tr.find('td:nth-child(3) input').focus();
        }
        else if (colIndex === 3) {
            // Move to next row, column 1
            let $nextRow = $tr.next('tr');
            if ($nextRow.length > 0) {
                $nextRow.find('td:nth-child(1) input').focus();
            }
        }
    }
});

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})
function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
}
function closepopup() {
    $("#SortPopUpModal").modal('hide');
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
    /*btnExtract_Click();*/
}
function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}
function btnSortReset_Click() {
    InitSortPopup();
}
function InitSortPopup() {
    debugger
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/YieldSearch/InitializeSortPopup",
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
