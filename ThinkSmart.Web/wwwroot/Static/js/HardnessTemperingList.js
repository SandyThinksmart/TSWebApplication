var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
$(document).ready(function () {
   var condition = GetURLParameters(window.location.href).trim().toLowerCase();
    if (condition == "false") {
        document.title = $.trim($("#lblTitle_TemperingEditDelete").text().replace("</br>", ""));
    } else
    {
        document.title = $.trim($("#lblTitle_TemperingJudgementEntryEditDelete").text().replace("</br>", ""));
    }
   
    Mask();
    //BindDropdown();
    $("#txtchuban").focus();
    InitSortPopup();
});

function Mask() {
    $("#txtSrchTransferStartDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtSrchTransferEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
}
$("#btnSort").click(function () {
    
    SetSortFocus();
    $("#SortPopUpModal").modal('show');
});
function OnEditClick(editButton) {
    let screenURL = false;
    screenURL = GetURLParameters(window.location.href).trim().toLowerCase();

    var tempCD = $.trim(editButton.closest("tr").attr("data-HARDENING_TEMPERING_CD"));

    if (screenURL == "true") {
        location.href = "/HardnessTempering/HardnessTemperingEntryEditDelete?QualityCD=" + tempCD;
    }
    if (screenURL == "false") {
        location.href = "/HardnessTempering/HardnessTemperingEditDelete?TempCD=" + tempCD;
    }
  
    
  
}

$("#btnGoBack").click(function () {
    let condition = false;
    condition = GetURLParameters(window.location.href).trim().toLowerCase();
    if (condition == "false") {
        location.href = "/HardnessTempering/HardnessTemperingEditDelete";
    }
    if (condition == "true") {
        location.href = "/HardnessTempering/HardnessTemperingEntryEditDelete";
    }
});

$("#btnInitialize").click(function () {
    ClearPanel($("#divSearch"));
    $("#ddlCustomer").html("").append($('<option value="0"></option>'));
    FillEmptyGrid();
});
function BindDropdown() {
    SetMachineList();
    SetProcConditionKubun();
}

function SetMachineList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetMachineList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMachineDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindMachineDropdown(jsonResult) {
    $("#ddlMachine").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMachine").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function SetProcConditionKubun() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetProcConditionKubun",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindProcConditionKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function BindProcConditionKubunDropdown(jsonResult) {
    $("#ddlProcCondition").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function btnSearch_Click() {
    let screenUrl = false;
    screenUrl = GetURLParameters(window.location.href).trim().toLowerCase();
    if (screenUrl == "false") {
        HardnessTemperingGridSearch(null);
    }
    if (screenUrl == "true") {
        HardnessTemperingEntryEditDeleteGridSearch(null);
    }
   
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
    var fieldLength = $('#lstSortFields option').length;
    if (fieldLength > 0) {

        $("#lstSortFields").prop('selectedIndex', parseInt(fieldLength) - 1);
    } else {
        var fieldLength1 = $('#lstSelectedSortFields option').length;
        if (fieldLength1 > 0) {
            $("#lstSelectedSortFields").prop('selectedIndex', parseInt(fieldLength1) - 1);
        }
    }
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
    SetSortFocus();
}
function btnSortClose_Click() {
    ClosePopup();
}
function btnSortOk_Click() {
    let screenUrl = false;
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

    screenUrl = GetURLParameters(window.location.href).trim().toLowerCase();
    if (screenUrl == "false") {
        HardnessTemperingGridSearch(selectedColumns);
    } 
    if (screenUrl == "true") {
        HardnessTemperingEntryEditDeleteGridSearch(selectedColumns);
    }


   // HardnessTemperingGridSearch(selectedColumns);
    ClosePopup();
}
function ClosePopup() {
    $("#SortPopUpModal").modal('hide');
}
function HardnessTemperingGridSearch(sortFields) {

    var txtchuban = $("#txtchuban").val() ? $("#txtchuban").val() : null;
    var txtFurnaceNum = $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null;
    var ddlNameValue = $("#ddlCustomer").val();
    var txtProdName = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var txtMatrialD = $("#txtMatrialD").val() ? $("#txtMatrialD").val() : null;
    var txtProductDim = $("#txtProductDim").val() ? $("#txtProductDim").val() : null;
    var txtSrchTransferStartDate = $("#txtSrchTransferStartDate").val() ? $("#txtSrchTransferStartDate").val() : null;
    var txtSrchTransferEndDate = $("#txtSrchTransferEndDate").val() ? $("#txtSrchTransferEndDate").val() : null;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
    var errorMsg = "";
    var focusField = "";


    if (txtSrchTransferStartDate != null) {
        if (!CheckDate($("#txtSrchTransferStartDate"))) {
            focusField = "txtSrchTransferStartDate";
            errorMsg = $.trim($("#lblErrorMessageE_HARDNESS_LIST_003").text())
        }



    }

    if (txtSrchTransferEndDate != null) {

        if (!CheckDate($("#txtSrchTransferEndDate"))) {
            if (focusField == "") {
                focusField = "txtSrchTransferEndDate";
            }

            if (errorMsg == "") {
                errorMsg = $.trim($("#lblErrorMessageE_HARDNESS_LIST_004").text())
            } else {
                errorMsg = errorMsg + "<br>" + $.trim($("#lblErrorMessageE_HARDNESS_LIST_004").text())
            }


        }

    }

    if (errorMsg != "")
    {
        ShowMessageDialog(errorMsg, focusField, WARNING);
        return false;
     }

    if (txtSrchTransferStartDate != null && txtSrchTransferEndDate != null) {
    }

    if (Date.parse(txtSrchTransferStartDate) >= Date.parse(txtSrchTransferEndDate)) {

        ShowMessageDialog($.trim($("#lblErrorMessageE_HARDNESS_LIST_005").text()), "txtSrchTransferStartDate", WARNING);
        return false;
    }

    if (txtSrchTransferEndDate != null && txtSrchTransferStartDate == null) {

        ShowMessageDialog($.trim($("#lblErrorMessageE_HARDNESS_LIST_002").text()), "txtSrchTransferStartDate", WARNING);
        return false;
    }

    if (txtchuban != null || txtFurnaceNum != null || ddlNameValue > 0 || txtProdName != null || txtMatrialD != null || txtProductDim != null || txtSrchTransferStartDate != null || txtSrchTransferEndDate != null) {


        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ CHUUBAN: txtchuban, FURNACE_NO: txtFurnaceNum, CUSTOMER_CD: ddlNameValue, PROD_NAME_CUST: txtProdName, MATL_DIM_D: txtMatrialD, PROD_DIM_D: txtProductDim, INSPECTION_DATE1: txtSrchTransferStartDate, INSPECTION_DATE2: txtSrchTransferEndDate, USER_ID: userID, SORT_FIELDS: sortFields })

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
                        url: "/HardnessTempering/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divMainHardnessTemperingList").html(response1.message);
                                $("#btnSearch").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                            ShowMessageDialog(message, focusFieldID, ERROR);
                        });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {


        ShowMessageDialog($.trim($("#lblErrorMessageE_HARDNESS_LIST_001").text()), "txtchuban", WARNING);
    }
}

function btnSortReset_Click() {
    InitSortPopup();
    SetSortFocus();
}
function SetSortFocus() {
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
function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) {
        result ="";
    } else {
        var sPageURL = url.substring(searchIndex + 1);
        var sParameterName = url.split('EntryEditDelete=');
        result = sParameterName[1];
    }
    return result;
}
function InitSortPopup() {
    let condition = false;

    condition = GetURLParameters(window.location.href).trim().toLowerCase();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/HardnessTempering/InitializeSortPopup?isEntryEditDeleteScreen=" + condition,
        method: "POST",
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
        }).fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
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
function CheckDate(field) {
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
            var totdalDays = new Date(regs[1], regs[2], 0).getDate();
            if (parseInt(regs[3]) > totdalDays) {

                return false;
            } else if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {

                return false;
            }
            else {
                return true;
            }
        }
    }
}
function ClearPanel($panel) {

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
    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
    });


    $.each($panel.find("textarea"), function (index, textarea) {
        $(textarea).val("");
    });

    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}

function FillEmptyGrid() {
    var emptyData = [];
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HardnessTempering/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyData)
        
    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divMainHardnessTemperingList").html(response1.message);
                $("#btnSearch").focus();
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}

function HardnessTemperingEntryEditDeleteGridSearch(sortFields) {
    
    var txtchuban = $("#txtchuban").val() ? $("#txtchuban").val() : null;
    var txtFurnaceNum = $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null;
    var ddlNameValue = $("#ddlCustomer").val();
    var txtProdName = $("#txtProdName").val() ? $("#txtProdName").val() : null;
    var txtMatrialD = $("#txtMatrialD").val() ? $("#txtMatrialD").val() : null;
    var txtProductDim = $("#txtProductDim").val() ? $("#txtProductDim").val() : null;
    var txtSrchTransferStartDate = $("#txtSrchTransferStartDate").val() ? $("#txtSrchTransferStartDate").val() : null;
    var txtSrchTransferEndDate = $("#txtSrchTransferEndDate").val() ? $("#txtSrchTransferEndDate").val() : null;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
    var errorMsg = "";
    var focusField = "";

    if (txtSrchTransferStartDate != null) {
        if (!CheckDate($("#txtSrchTransferStartDate"))) {
            focusField = "txtSrchTransferStartDate";
            errorMsg = $.trim($("#lblErrorMessageE_HARDNESS_LIST_003").text())
        }



    }

    if (txtSrchTransferEndDate != null) {

        if (!CheckDate($("#txtSrchTransferEndDate"))) {
            if (focusField == "") {
                focusField = "txtSrchTransferEndDate";
            }

            if (errorMsg == "") {
                errorMsg = $.trim($("#lblErrorMessageE_HARDNESS_LIST_004").text())
            } else {
                errorMsg = errorMsg + "<br>" + $.trim($("#lblErrorMessageE_HARDNESS_LIST_004").text())
            }


        }

    }
    if (errorMsg != "")
    {
        ShowMessageDialog(errorMsg, focusField, WARNING);
        return false;
    }
    
    if (txtSrchTransferStartDate != null && txtSrchTransferEndDate != null) {
    }

    if (Date.parse(txtSrchTransferStartDate) >= Date.parse(txtSrchTransferEndDate)) {

        ShowMessageDialog($.trim($("#lblErrorMessageE_HARDNESS_LIST_005").text()), "txtSrchTransferStartDate", WARNING);
        return false;
    }

    if (txtSrchTransferEndDate != null && txtSrchTransferStartDate == null) {

        ShowMessageDialog($.trim($("#lblErrorMessageE_HARDNESS_LIST_002").text()), "txtSrchTransferStartDate", WARNING);
        return false;
    }

    if (txtchuban != null || txtFurnaceNum != null || ddlNameValue > 0 || txtProdName != null || txtMatrialD != null || txtProductDim != null || txtSrchTransferStartDate != null || txtSrchTransferEndDate != null) {


        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/EntryEditDeleteGridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ CHUUBAN: txtchuban, FURNACE_NO: txtFurnaceNum, CUSTOMER_CD: ddlNameValue, PROD_NAME_CUST: txtProdName, MATL_DIM_D: txtMatrialD, PROD_DIM_D: txtProductDim, INSPECTION_DATE1: txtSrchTransferStartDate, INSPECTION_DATE2: txtSrchTransferEndDate, USER_ID: userID, SORT_FIELDS: sortFields })

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
                        url: "/HardnessTempering/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divMainHardnessTemperingList").html(response1.message);
                                $("#btnSearch").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                            ShowMessageDialog(message, focusFieldID, ERROR);
                        });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
                ShowMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {


        ShowMessageDialog($.trim($("#lblErrorMessageE_HARDNESS_LIST_001").text()), "txtchuban", WARNING);
    }
}
$('#txtchuban').keypress(function (e) {
    var orderNo = $('#txtchuban').val() ? $('#txtchuban').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;

                if (response.isSuccess) {
                    $('#txtchuban').val(jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#txtFurnaceNum').focus();
                } else {
                    $('#txtchuban').val("");
                   
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HARDNESS_LIST_006").text().trim();
                ShowMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});