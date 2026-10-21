
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";

var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
function Selrdbtn(id) {
    var rdoBtnSelect = document.getElementById(id);
    var List = document.getElementsByTagName("input");
    for (i = 0; i < List.length; i++) {
        if (List[i].type == "radio" && List[i].id != rdoBtnSelect.id) {
            List[i].checked = false;
        }
    }
}

//$(window).load(function () {
//    window.resizeTo(1430, 835);
//});

$(document).ready(function () {
    $.support.cors = true; // Special case for IE7. To get past weird errors.    
    $("#btnExportCSV").attr("DISABLED", "DISABLED");
   
    setLocationList();
    setNextProcess();
    InitSortPopup();
    $("#ddlStatus").focus();
});
function OnEditClick(editButton) {
    var ProcessPlanCD = $.trim(editButton.closest("tr").attr("data-lblProcessPlanCD"));
    // location.href = '/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=' + ProcessPlanCD;
    window.open("/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=" + ProcessPlanCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1900 + ", height=" + 1490);
}

function setLocationList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/NextProcessWaitingStatusSearch/GetLocationList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindLocationDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindLocationDropdown(jsonResult) {
    $("#ddlLocation").html("");
    $("#ddlLocation").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlLocation").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
       
    });
}

function setNextProcess() { 
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/NextPrcessWaitingStatusSearch/GetProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
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
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);

        });
}

function bindNextProcessDropdown(jsonResult) {
    $("#ddlProcess").html("");
    $("#ddlProcess").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcess").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

$("#btnInitialize").click(function () {
   
    clearTable($("#tblNextProcessWaitingStatusSearch"), "", "GridNormalRow");   
    var list = [];
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/NextPrcessWaitingStatusSearch/NextProcessWaitingStatusSearchGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(list)
    }
    $.ajax(settings)
        .done(function (response) {
            $("#divNextProcessWaitingStatusSearchGridMain").html(response.message);
        })
    $("#btnExportCSV").attr("DISABLED", "DISABLED");  
    setLocationList();
    setNextProcess();
    InitSortPopup();
    $("#ddlStatus").val(1);
    $("#ddlStatus").focus();
    DISABLED = "disabled";
    typeDropDown = "select-one";
    typeText = "text";
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
   
});
function clearTable($table, trId, trClass) {
    debugger;
    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }

}
$("#btnExtract").click(function (event) {

    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }
   
    GetNextProcessWaitingSearchData(null)
    
}).ajaxStart(function () {

    $("#LoadingImage").show();
}).ajaxStop(function () {
    $("#LoadingImage").hide();
}).ajaxError(function () {
    $("#LoadingImage").hide();
});

$(function () {
    $(document).ajaxStart(function () {

        $("#LoadingImage").show();
    });

    $(document).ajaxStop(function () {
        $("#LoadingImage").hide();
    });

    $(document).ajaxError(function () {
        $("#LoadingImage").hide();
    });

});


function GetNextProcessWaitingSearchData(sortFields) {
    debugger;
    var ajaxTime = new Date().getDate();
    var wip_status = $("#ddlStatus").val() ? $("#ddlStatus").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;    

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/NextProcessWaitingStatusSearch/GetNextProcessWaitingStatusSearch", 
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            WIP_STATUS: wip_status, LOCATION_CD: location_CD, PROCESS_CD: process_CD, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                debugger;
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                if ($("#hdfCultureName").val() == "en") {

                    $.each(jsonResult, function (key, object) {
                        object.prE_PROSESS_STRING = object.procesS_KUBUN_EN;
                        object.nexT_PROCESS_STRING = object.nexT_PROCESS_EN;
                        object.prE_PROSESS_DECISION_STRING = object.prE_PROSESS_DECISION;

                    });
                } else {
                    $.each(jsonResult, function (key, object) {

                        object.prE_PROSESS_STRING = object.procesS_KUBUN_JP;
                        object.nexT_PROCESS_STRING = object.nexT_PROCESS_JP;
                        object.prE_PROSESS_DECISION_STRING = object.prE_PROSESS_DECISION;
                    });
                }
            }
           
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFOund").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/NextPrcessWaitingStatusSearch/NextProcessWaitingStatusSearchGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {                   
                        var result = response1.message;
                        $("#divNextProcessWaitingStatusSearchGridMain").html(response1.message);                    
                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#valMsgCOMSystemException").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}


$(document).on('click', '#btnExportCSV', function () {
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
    downloadNextProcessWaitingStatusSearchCSV(selectedColumns);    
    
});
function downloadNextProcessWaitingStatusSearchCSV(sortFields) {
    debugger;
    var ajaxTime = new Date().getDate();
    var wip_status = $("#ddlStatus").val() ? $("#ddlStatus").val() : null;
    var location_CD = $("#ddlLocation").val() ? $("#ddlLocation").val() : null;
    var process_CD = $("#ddlProcess").val() ? $("#ddlProcess").val() : null;
    var cultureName = $("#hdfCultureName").val()
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/NextPrcessWaitingStatusSearch/ExportCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            WIP_STATUS: wip_status, LOCATION_CD: location_CD, PROCESS_CD: process_CD, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;            
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblDataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};
$(document).on('click', '#btnSort', function () {    
    $("#SortPopUpModal").modal('show');

})
function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/NextPrcessWaitingStatusSearch/InitializeSortPopup",
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

function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortClose_Click() {

    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
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
    }
    //else {
    //    alert("Please select the sort field to be added.");
    //}
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
    //else {
    //    alert("Please select the sort field to be added.");
    //}
    EnableDisableSortButtons();
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
    GetNextProcessWaitingSearchData(selectedColumns);
   
}

function btnSortReset_Click() {
   
  InitSortPopup();

}

function IsMandatorySearchFieldsFilled() {    
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#pnlSearch .Mandatory"), function (index, mandatoryField) {
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