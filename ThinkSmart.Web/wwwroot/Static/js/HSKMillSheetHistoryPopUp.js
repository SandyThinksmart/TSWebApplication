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
userName = "";
let screenUrl = false;
$(document).ready(function () {
    debugger;
    //  window.resizeTo(1080, 830);
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    setDecimalFormats();
    //Mask();


    InitSortPopup();
    $("#txtChuubanSearch").focus();
    userName = $("#hdfUserName").val();
    bindEvents();

    $("#txtDatestart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtDateend").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
});
function setDecimalFormats() {
    $("#txtDimD1,#txtDimD2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 6, 3);
    });
    $("#txtChuban").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 10);
    });
    $("#txtgyouban").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 10);
    });
}
function bindEvents() {
    debugger;
    $("#btnExtract").click(function (event) {
        debugger;
        var $callFrom = $(this);

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }
      //  ValidateChubban($("#txtChuuban").val(), $("#txtGyouban").val(), $callFrom);
        //  ValidateChubban($("#txtChuuban").val(), $("#txtGyouban").val(), $callFrom);
        //getThabaDetailsByChuuban($("#txtChuuban2").val(), $("#txtGyouban2").val(), $callFrom);
        //  getMachineDetails($("#hdfMachineCD").val(), $callFrom);
    });
}
function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#tblSearch .Mandatory"), function (index, mandatoryField) {
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


function btnSearch_Click() {

    debugger;
    screenUrl;
    screenUrl = getURLParameters(window.location.href).trim().toLowerCase();
    if (!IsMandatoryChuubanSearchFieldsFilled()) {
        return false;
    }else
    {
        if (screenUrl == "") {
            screenUrl = "false";
        }

        if (screenUrl == "false") {
            HSKMillSheetGridSearch(null);
        }
        if (screenUrl == "true") {
            HSKMillSheetGridSearch(null);
        }
    }
   

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
function HSKMillSheetGridSearch(sortFields) {
    debugger;
    var txtProdNameCust = $("#txtProdNameCust").val() ? $("#txtProdNameCust").val() : null;
    var txtshape = $("#txtshape").val() ? $("#txtshape").val() : null;
    var txtdim1 = $("#txtDimD1").val() ? $("#txtDimD1").val() : null;
    var txtdim2 = $("#txtDimD2").val() ? $("#txtDimD2").val() : null;
    var txtLength1 = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;
    var txtLength2 = $("#txtLengthT2").val() ? $("#txtLengthT2").val() : null;
    var txtcharge = $("#txtcharge").val() ? $("#txtcharge").val() : null;
    var txtDatestart = $("#txtDatestart").val() ? $("#txtDatestart").val() : null;
    var txtDateend = $("#txtDateend").val() ? $("#txtDateend").val() : null;
    var txtChuuban = $("#txtChuban").val() ? $("#txtChuban").val() : null;
    var txtgyouban = $("#txtgyouban").val() ? $("#txtgyouban").val() : null;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
    var errorMsg = "";
    var focusField = "";


    if (txtProdNameCust != null || txtshape != null || txtdim1 != null || txtdim2 != null || txtLength1 != null || txtLength2 != null || txtcharge != null || txtDatestart != null || txtDateend != null || txtChuuban != null || txtgyouban != null) {


        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HSKMillSheet/GridSearchHistoryPopup",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ PROD_NAME_CUST: txtProdNameCust, PROD_SHAPE: txtshape, PROD_DIM_D_MIN: txtdim1, PROD_DIM_D_MAX: txtdim2, PROD_LENGTH_MIN: txtLength1, PROD_LENGTH_MAX: txtLength2, CHARGE: txtcharge, DELIVERY_DATE_MIN: txtDatestart, DELIVERY_DATE_MAX: txtDateend, CHUUBAN: txtChuuban, GYOUBAN: txtgyouban, USER_ID: userID, SORT_FIELDS: sortFields })

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
                        url: "/HSKMillSheet/GridSearchHistoryPopup",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divHSKMillSheetHistoryList").html(response1.message);
                                $("#btnSearch").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {



    }
}





$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})

function btnSortReset_Click() {
    InitSortPopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortAdd_Click() {
    var selectedColumns = $("#lstSortFields").val();
    if (selectedColumns.length > 0) {
        $("#lstSortFields option:selected").each(function () {
            var $this = $(this);
            if ($this.length) {
                var selText = $this.text();
                console.log(selText);
                $('#lstSelectedSortFields').append($("<option></option>")
                    .attr("value", $this.val())
                    .text(selText + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
                $this.remove();
            }
        });


        //for (var i = 0; i < selectedColumns.length; i++) {
        //    var text = $("#lstSortFields option[value='" + selectedColumns[i] + "']").text();
        //    $("#lstSortFields option[value='" + selectedColumns[i] + "']").remove();
        //    $('#lstSelectedSortFields').append($("<option></option>")
        //        .attr("value", selectedColumns[i])
        //        .text(text + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
        //}
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

function btnSortRemove_Click() {
    var selectedColumns = $("#lstSelectedSortFields").val();
    if (selectedColumns.length > 0) {
        var selectedColumns = $("#lstSelectedSortFields").val();
        if (selectedColumns.length > 0) {
            $("#lstSelectedSortFields option:selected").each(function () {
                var $this = $(this);
                if ($this.length) {
                    var selText = $this.text().split(' ');
                    console.log(selText);
                    selText.splice(-1);
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", $this.val())
                        .text(selText.join(' ')));
                    $this.remove();
                }
            });
        }
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
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
    HSKMillSheetGridSearch(selectedColumns);
}
function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/HSKMillSheet/InitializeSortPopup",
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
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            showMessageDialog(message, focusFieldID, ERROR);
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

function OnEditClick(editButton) {
    var ProcessPlanCD = $.trim(editButton.closest("tr").attr("data-lblProcessPlanCD"));
    // location.href = '/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=' + ProcessPlanCD;
    window.open("/HSKMillSheet/HSKMillSheetSaveEdit?ProcessPlanCD=" + ProcessPlanCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1880 + ", height=" + 950);
}