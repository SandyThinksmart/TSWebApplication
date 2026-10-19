var $btnLnkEdit;
var combinedDecisionOK = 1;
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";
var $hdfSYSDATE;
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var oldHikinukiRemarks;
var isHikinukiRemarksUpdate = 0;
var IsExists = false;
var IsStartOprExists = false;
var IsEndOprExists = false;
var registeredThabaNumbersAndDecision = {};
var maxRegisteredThabaNo = 0;
var IsNGReason1 = false;
var IsNGReason2 = false;
var IsNGReason3 = false;
var IsProcessDefect1Exists = false;
var IsProcessDefect2Exists = false;
var IsProcessDefect3Exists = false;
var startOperatorCD = "";
var endOperatorCD = "";
var DISABLED = "disabled";
let screenUrl = false;


$(document).ready(function () {
     window.resizeTo(1380, 970);
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    setDecimalFormats();
    Mask();
    SetMatlMakerList();
    InitEditSortPopup();
    $("#ddlMakerName").focus();
    var date = new Date();
    var newdate = formatDate(date)
    $("#txUpdateDateStart").val(newdate);
   
});
function setDecimalFormats() {
    $("#txtDim1,#txtDim2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
   
}
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear()-1;

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('/');
}
function Mask() {
    debugger;
    $("#txSaveDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txSaveDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txUpdateDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txUpdateDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    let date = new Date().toLocaleDateString('en-ZA')
    $("#txUpdateDateEnd").val(date);
  
}
function subtractYears(date, years) {
    date.setFullYear(date.getFullYear() - years);
    return date;
}
function SetMatlMakerList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GeMatlMakerList",
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
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_LIST_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindMakerDropdown(jsonResult) {
    $("#ddlMakerName").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMakerName").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}
$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})
function btnSearch_Click() {

    if ($("#txSaveDateStart").val() != "")
    {
        if (!CheckDate($("#txSaveDateStart"))) {
            var message = $("#lblErrorMessageE_MAKERMILLSHEET_LIST_002").text().trim();
            ShowMessageDialog(message, "txSaveDateStart", WARNING);
            return false;
        }
    }

    if ($("#txSaveDateEnd").val() != "")
    {
        if (!CheckDate($("#txSaveDateEnd"))) {
            var message = $("#lblErrorMessageE_MAKERMILLSHEET_LIST_003").text().trim();
            ShowMessageDialog(message, "txSaveDateEnd", WARNING);
            return false;
        }
    }

    if ($("#txUpdateDateStart").val() != "")
    {
        if (!CheckDate($("#txUpdateDateStart"))) {
            var message = $("#lblErrorMessageE_MAKERMILLSHEET_LIST_004").text().trim();
            ShowMessageDialog(message, "txUpdateDateStart", WARNING);
            return false;
        }
    }

    if ($("#txUpdateDateEnd").val() != "")
    {

        if (!CheckDate($("#txUpdateDateEnd"))) {
            var message = $("#lblErrorMessageE_MAKERMILLSHEET_LIST_005").text().trim();
            ShowMessageDialog(message, "txUpdateDateEnd", WARNING);
            return false;
        }
    }

    if ($("#txSaveDateStart").val() != "" && $("#txSaveDateEnd").val() != "")
    {
        if (new Date($("#txSaveDateStart").val()) > new Date($("#txSaveDateEnd").val())) {
            var message = $("#lblErrorMessageE_MAKERMILLSHEET_LIST_06").text().trim();
            showMessageDialog(message, "txSaveDateStart", WARNING);
            return false;
        }
    }
    if ($("#txUpdateDateStart").val() != "" && $("#txUpdateDateEnd").val() != "") {
        if (new Date($("#txUpdateDateStart").val()) > new Date($("#txUpdateDateEnd").val())) {
            var message = $("#lblErrorMessageE_MAKERMILLSHEET_LIST_07").text().trim();
            showMessageDialog(message, "txUpdateDateStart", WARNING);
            return false;
        }
    }
        MakerMillSheetGridSearch(null);
   

}
function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) {
        result = "";
    } else {
        var sPageURL = url.substring(searchIndex + 1);
        var sParameterName = url.split('EntryEditDelete=');
        result = sParameterName[1];
    }
    return result;
}
function MakerMillSheetGridSearch(sortFields) {

    var txtmakername = $("#ddlMakerName").val() ? $("#ddlMakerName").val() : null;
    var txttypeofsteel = $("#txtTypeofSteel").val() ? $("#txtTypeofSteel").val() : null;
    var txtdim1 = $("#txtDim1").val() ? $("#txtDim1").val() : null;
    var txtdim2 = $("#txtDim2").val() ? $("#txtDim2").val() : null;
    var txtcharge = $("#txtcharge").val() ? $("#txtcharge").val() : null;
    var txtkouban = $("#txtkouban").val() ? $("#txtkouban").val() : null;
    var txsavesatestart = $("#txSaveDateStart").val() ? $("#txSaveDateStart").val() : null;
    var txsavedateend = $("#txSaveDateEnd").val() ? $("#txSaveDateEnd").val() : null;
    var txupdatedatestart = $("#txUpdateDateStart").val() ? $("#txUpdateDateStart").val() : null;
    var txupdatedateend = $("#txUpdateDateEnd").val() ? $("#txUpdateDateEnd").val() : null;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
    var errorMsg = "";
    var focusField = "";

    if (txtmakername != null || txttypeofsteel != null || txtdim1 != null || txtdim2 != null || txtcharge != null || txtkouban != null || txsavesatestart != null || txsavedateend != null || txupdatedatestart != null || txupdatedateend != null) {


        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/MakerMillSheet/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ MAKER_CD: txtmakername, MATL_NAME_MAK: txttypeofsteel, MATL_DIM_D: txtdim1, MATL_DIM_W: txtdim2, CHARGE: txtcharge, KOUBAN: txtkouban, COMPLETE_START_DATE: txsavesatestart, COMPLETE_END_DATE: txsavedateend, UPDATE_START_DATE: txupdatedatestart, UPDATE_END_DATE: txupdatedateend, USER_ID: userID, SORT_FIELDS: sortFields })

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    var settings1 = {
                        async: false,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/MakerMillSheet/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divMakerMillSheetList").html(response1.message);
                                $("#btnSearch").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_LIST_001").text());
                            ShowMessageDialog(message, focusFieldID, ERROR);
                        });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_LIST_001").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {


        
    }
}

function OnEditClick(editButton) {
 
    var millsheetCD = $.trim(editButton.closest("tr").attr("data-MAKER_MILL_SHEET_CD"));

   location.href = "/MakerMillSheet/MakerMillsheetSaveEdit?millsheetCD=" + millsheetCD;
  
}

function btnSortReset_Click() {
    InitEditSortPopup();
    setSortFocus();
}
function setSortFocus() {
    var fieldLength = $('#lstSelectedSortFields option').length;
    if (fieldLength > 0) {

        $("#lstSelectedSortFields").prop('selectedIndex', parseInt(fieldLength) - 1);
    } else {
        var fieldLength1 = $('#lstSortFields option').length;
        if (fieldLength1 > 0) {
            $("#lstSortFields").prop('selectedIndex', parseInt(fieldLength1) - 1);
        }
    }
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortOk_Click() {
    $("#btnSort").focus();
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
    MakerMillSheetGridSearch(selectedColumns);
    closepopup();
}
function InitEditSortPopup() {
    let condition = false;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/MakerMillSheet/InitializeSortPopup?isEntryEditDeleteScreen=" + condition,
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
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_LIST_001").text());
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
    $("#SortPopUpModal").modal('hide');

}
$("#btnSort").focusout(function () {
    $("#btnLnkEdit").focus();
});
$("#btnClose_W").click(function () {
    location.href = "/MakerMillSheet/MakerMillsheetSaveEdit";
    window.resizeTo(1930, 1300);
});
function CheckDate(field) {

    var minYear = 1000;//2012
    var maxYear = 2100;


    //regular expression to match required date format
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