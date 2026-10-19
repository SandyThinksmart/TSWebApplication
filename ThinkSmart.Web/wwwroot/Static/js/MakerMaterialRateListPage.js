var typeDropDown = "select-one";
var DISABLED = "disabled";
var typeText = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
$(document).ready(function () {
    
    $("#lblListPageName").text($("#lblTitle_MakerMaterialListPage_001").text());
    SetDefaultDate();
    SetMakerList();
    SetMaterialType();
    InitSortPopup();
    $("#txtYear").focus();

});
function SetDefaultDate() {
    var currentTime = new Date()
    $("#txtYear").val(currentTime.getFullYear());
   
    var getMonth = currentTime.getMonth() + 1;
   
    if (getMonth.toString().length == 1) {
        getMonth = "0" + getMonth
    }
    $("#txtMonth").val(getMonth);
    
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
            message = $.trim($("#lblErrorMessageE_Maker_MaterialList_001").text());
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
    debugger;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:APIURL + "/MakerMaterial/GetMakerList",
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
            message = $.trim($("#lblErrorMessageE_Maker_MaterialList_001").text());
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
    debugger;
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
            message = $.trim($("#lblErrorMessageE_Maker_MaterialList_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMukesakiDropdown(jsonResult) {
    $("#ddlMukesakiName").html("").append($('<option value="0"></option>'));;
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMukesakiName").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function btnSelect_Click() {
    window.location.href = "/MakerMaterial/MakerMaterialRateList";
}


$("#btnExtractGrid").click(function () {
    if ($("#txtYear").val() == "" || $("#txtMonth").val() == "" || $("#txtChance").val() == "") {
        $("#txtCheckApplicationDate").addClass("Mandatory");
    } else {
        $("#txtCheckApplicationDate").removeClass("Mandatory");
    }

    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }
});
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
    MakerMaterialGridSearch(selectedColumns);
}
function btnExtractList_Click()
{
    MakerMaterialGridSearch(null);
}
function MakerMaterialGridSearch(sortFields)
{

    var jsonData = JSON.stringify({
        APPLY_ROLL_YEAR: $("#txtYear").val() ? $("#txtYear").val() : null,
        APPLY_ROLL_MONTH: $("#txtMonth").val() ? $("#txtMonth").val() : null,
        APPLY_ROLL_CHANCE: $("#txtChance").val() ? $("#txtChance").val() : null,
        MAKER_CD: $("#ddlMakerName").val(),
        MUKESAKI_CD: $("#ddlMukesakiName").val(),
        MATL_KUBUN: $("#ddlMaterialType").val(),
        SORT_FIELDS: sortFields
    });

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMaterial/GetMakerMaterialListData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/MakerMaterial/FillListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)

            }
            $.ajax(settings1)
                .done(function (response1) {
                    if (response1.isSuccess) {
                        $("#divMaterialListGrid").html(response1.message);

                    }
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_Maker_MaterialList_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                    return false;
                });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MaterialList_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    closepopup();
}
$("#btnMukesakiSearch").click(function () {
    var yobina = $("#txtMukesakiName").val() ? $("#txtMukesakiName").val() : "";
    SetMukesakiList(yobina);
});

function SetMukesakiList(yobina) {
    debugger;
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
            message = $.trim($("#lblErrorMessageE_Maker_MaterialList_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function GetMakerMaterialDetailsByCD($callFrom) {

    
    $callFrom.data('requestRunning', true);
    
    var rateCD = $.trim($callFrom.closest("tr").attr("data-lblGroupCD"));
    window.location.href = "/MakerMaterial/MakerMaterialRateEntry?groupNo=" + rateCD;
}
function IsMandatorySearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#divSearchCondition .Mandatory"), function (index, mandatoryField) {
       
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                debugger;
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");

                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            debugger;
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

$("#btnClosePopup").click(function () {

    $("#divMaterialPopup").modal('hide');
});
function InitSortPopup() {

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/MakerMaterial/InitializeSortPopup",
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
            $("#rdoSortAsc").prop("checked", true);
            $("#rdoSortDsc").prop("checked", false);
        })
}
function btnCloseList_Click()
{
    window.location.href = "/MakerMaterial/MakerMaterialRateEntry";
}
$("#ddlMaterialType").focusout(function () {
    $("#btnExtract").focus();
});