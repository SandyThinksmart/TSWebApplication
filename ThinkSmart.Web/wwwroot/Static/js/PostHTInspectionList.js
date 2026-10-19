var userName = "";
var combinedDecisionOK = 1;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var $btnInitialize;
var $btnSave;
var $btnEdit;
var $btnDelete;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";


$(document).ready(function () {

    $.support.cors = true; // Special case for IE7. To get past weird errors.
    window.resizeTo(800, 780);
    Mask();
    getMachineList();
    setProcConditionKubun();
    InitSortPopup();
   // btnSearch_Click(false);
});

function Mask() {
    $("#txtSrchInspectionDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtSrchInspectionDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
}

$("#btnInitialize").click(function (event) {
    clearPanel($("#pnlProductSearch"));
   // clearPanel($("#pnlListDetailGrid")); 
    $("#machine").text("");
    $("#InspDate").text("");
    $("#FurnanceNo").text("");
    $("#ProcCondition").text("");
    $("#PatternNo").text("");
    $("#Inspector").text("");
});

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

//sort popup
$(document).on('click', '#btnSort', function () {
    debugger;
    $("#SortPopUpModal").modal("show");
})

function btnSortOk_Click() {
   
    debugger;
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
    PostHTGridSearch(selectedColumns);
}

function PostHTGridSearch(sortFields) {
    debugger;
    var txtMachine = $("#ddlMachine").val();
    var txtFurnaceNum = $("#txtSrchFurnaceNum").val();
    var txtProductCond = $("#ddlProcCondition").val();
    var txtPatternNo = $("#txtSrchPatternNo").val();
    var txtInspDateStart = $("#txtSrchInspectionDateStart").val();
    var txtInspDateEnd = $("#txtSrchInspectionDateEnd").val();
    var userId = $("#hdfLoginId").val();

    if (txtMachine.length > 0 || txtFurnaceNum.length > 0 || txtProductCond.length > 0 || txtPatternNo.length > 0 || txtInspDateStart.length > 0 || txtInspDateEnd.length > 0) {
        var ajaxTime = new Date().getTime();

        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/PostHtInspection/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                MACHINE_CD: txtMachine, FURNACE_NO: txtFurnaceNum, PROD_CONDITION_CD: txtProductCond, PATTERN_NO: txtPatternNo, INSPECTION_DATE1: txtInspDateStart,
                INSPECTION_DATE2: txtInspDateEnd, USER_ID: userId, SORT_FIELDS: sortFields
            })
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
                        url: "/PostHtInspection/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;
                            if (response1.isSuccess) {
                                $("#pnlListDetailGrid").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            debugger;
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                else {
                    bootbox.alert({

                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {

        if (sortFields.length > 0) {
            var ajaxTime = new Date().getTime();
            //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
            var settings = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/PostHtInspection/GridSearch",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    USER_ID: userId, sortFields: sortFields
                })
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
                            url: "/PostHtInspection/GridSearch",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)
                            //data: "{jsonData : " + jsonData + "}"
                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#pnlListDetailGrid").html(response1.message);

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
                    else {
                        bootbox.alert({
                            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                            message: response.message,
                            centerVertical: true
                        });
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
        //bootbox.alert({
        //    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
        //    message: $.trim($("#lblErrorMessageE_ColdDrawing_SearchField").text()),
        //    centerVertical: true
        //});
    }
    closepopup();
}

function btnSortClose_Click() {
    closepopup();
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

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/PostHtInspection/InitializeSortPopup",
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

function checkDate(field) {

    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var minYear = 2019;
    var maxYear = 2100;
    // regular expression to match required date format
    //re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
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

function btnSearch_Click() {
    debugger;
    var isSearchFieldExist = false;
    if ($("#txtSrchInspectionDateStart").val() == "" && $("#txtSrchInspectionDateEnd").val() != "") {
        //if (!checkDate($("#txtSrchDateEnd"))) {
            var message = $("#lblErrorMessageE_PostHT_013").text().trim(); // This message will be displayed on dialog box 
        showMessageDialog(message, "txtSrchInspectionDateStart", WARNING);
        //}
        return false;
    }
    if ($("#txtSrchInspectionDateStart").val() != "") {
        if (!checkDate($("#txtSrchInspectionDateStart"))) {
            var message = $("#lblErrorMessageE_PostHT_012").text().trim(); // This message will be displayed on dialog box              
            showMessageDialog(message, "txtSrchInspectionDateStart", WARNING);
            return false;
        }
    }
    if ($("#txtSrchInspectionDateEnd").val() != "") {
        if (!checkDate($("#txtSrchInspectionDateEnd"))) {
            var message = $("#lblErrorMessageE_PostHT_014").text().trim(); // This message will be displayed on dialog box             
            showMessageDialog(message, "txtSrchInspectionDateEnd", WARNING);
            return false;
        }
    }
    debugger;
  //  var txtMachine = $("#ddlMachine").val() ;
    var txtFurnaceNum = $("#txtSrchFurnaceNum").val() ;
    var txtProductCond = $("#ddlProcCondition option:selected").text(); //$("#ddlProcCondition").val() ;
    var txtPatternNo = $("#txtSrchPatternNo").val() ;
    var txtInspDateStart = $("#txtSrchInspectionDateStart").val();
    var txtInspDateEnd = $("#txtSrchInspectionDateEnd").val();
    var userId = $("#hdfLoginId").val();
    var sortFields = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            sortFields.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });
    if (txtFurnaceNum.length > 0 || txtProductCond.length > 0 || txtPatternNo.length > 0 || txtInspDateStart.length > 0 || txtInspDateEnd.legth > 0) {
        var ajaxTime = new Date().getTime();
        isSearchFieldExist = true;
        //var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/PostHtInspection/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                FURNACE_NO: txtFurnaceNum, PROD_CONDITION_STRING: txtProductCond, PATTERN_NO: txtPatternNo, INSPECTION_DATE1: txtInspDateStart,
                INSPECTION_DATE2: txtInspDateEnd, USER_ID: userId, SORT_FIELDS: sortFields
            })
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
                        url: "/PostHtInspection/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#pnlListDetailGrid").html(response1.message);
                                isSearchFieldExist = true;
                                $("#lnkEdit").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            debugger;
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                
            })
            .fail(function (xhr, textStatus, errorThrown) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_PostHTInspection").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    if (isSearchFieldExist == false) {
        var message = $.trim($("#lblErrorMessageE_PostHT_011").text());
        showMessageDialog(message, "ddlMachine", WARNING);
    }
}

$(document).on('click', '#btnCloseOnList', function () {
    location.href = "/PostHtInspection/PostHtInspectionEntryEditDelete";
})

$(document).on('click', '#btnlnk', function () {
    var GroupNo = $("#hdfGroupNo").val();
    var proc_cd =  $("#hdfProcessPlan_CD").val();
    location.href = "/PostHtInspection/PostHtInspectionEntryEditDelete?GroupNo=" + GroupNo;
})


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

function setProcConditionKubun() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetProcConditionKubun",
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
            message = $.trim($("#lblErrorMessageE_HeatTreat_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcConditionKubunDropdown(jsonResult) {
    debugger;
    $("#ddlProcCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcCondition").append($('<option value="' + jsonObj.cd + '">'+ jsonObj.name +'</option>'));
    });

}

function OnEditClick(editButton) {
    var internalGroupNo = $.trim(editButton.closest("tr").attr("data-lblInternalQualityCD"));
    location.href = '/PostHtInspection/PostHTInspectionEntryEditDelete?InternalGroupNo=' + internalGroupNo;   
}
