var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var userName = '';
$(document).ready(function () {
   
    $("#ddlMachine").focus();
    window.resizeTo(840, 800);
    Mask();
    BindDropdown();
    InitSortPopup();
    setDateTime();
});

function Mask() {
    $("#txtSrchDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtSrchDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

}

$(document).on('click', '#buttonClose', function () {
    debugger;
    BtnClose();
})

function BtnClose() {
    debugger;
    location.href = "/HeatTreatment/HeatTreatmentEditDelete";
}
function OnEditClick(editButton) {
    
    var HeatTreatMentCd = $.trim(editButton.closest("tr").attr("data-HEAT_TREATMENT_NUMBER"));
    var furNo = $.trim(editButton.closest("tr").attr("data-furNo"));
    location.href = '/HeatTreatment/HeatTreatmentEditDelete?furNo=' + furNo + '&HeatTreatMentCd=' + HeatTreatMentCd;
   
   
    //let screenURL = false;
    //screenURL = GetURLParameters(window.location.href).trim().toLowerCase();
    //location.href = "/HeatTreatment/HeatTreatmentEditDelete";
    //var tempCD = $.trim(editButton.closest("tr").attr("data-HEAT_TREATMENT_NUMBER"));

    if (screenURL == "true") {
        location.href = "/HeatTreatment/HeatTreatmentEditDelete?HeatTreatMentCd=" + tempCD;
    }
    //if (screenURL == "false") {
    //    location.href = "/HeatTreatment/HeatTreatmentEditDelete?HeatTreatMentCd=" + tempCD;
    //}




}
function GetURLParameters(url) {

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


function BindDropdown() {
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
            message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
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
            message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcConditionKubunDropdown(jsonResult) {
    $("#ddlProcCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcCondition").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}

function setDateTime() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            console.log(response);
            var jsonResult = response.message;
            var startDateObj = new Date(jsonResult);
            startDateObj.setMonth(startDateObj.getMonth() - 1);
            $("#txtSrchDateStart").val($.format.date(new Date(startDateObj), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtSrchDateEnd").val($.format.date(new Date(jsonResult), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function checkDate(field) {
    debugger;
    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var minYear = 2019;
    var maxYear = 2100;
    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
            debugger;
            console.log(regs);
            var totdalDays = new Date(regs[1], regs[2], 0).getDate();
            if (parseInt(regs[3]) > totdalDays) {

                return false;
            } else if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {
                debugger;
                return false;
            }

            else {
                debugger;
                return true;
            }
        }
    }
}

function btnSearch_Click() {
    debugger;
    var isSearchFieldExist = false;
    if ($("#txtSrchDateStart").val() == "" && $("#txtSrchDateEnd").val() != "") {
        var message = $("#lblErrorMessageE_HT_EDIT_DELETE_List_004_SearchField").text().trim(); // This message will be displayed on dialog box 
        showMessageDialog(message, "ddlMachine", WARNING);
        return false;
    }
    if ($("#txtSrchDateStart").val() != "") {
        if (!checkDate($("#txtSrchDateStart"))) {
            var message = $("#lblErrorMessageE_HT_EDIT_DELETE_List_002_SearchField").text().trim(); // This message will be displayed on dialog box              
            showMessageDialog(message, "txtSrchDateStart", WARNING);
            return false;
        }
    }
    if ($("#txtSrchDateEnd").val() != "") {
        if (!checkDate($("#txtSrchDateEnd"))) {
            var message = $("#lblErrorMessageE_HT_EDIT_DELETE_List_003_SearchField").text().trim(); // This message will be displayed on dialog box             
            showMessageDialog(message, "txtSrchDateEnd", WARNING);
            return false;
        }
    }
    var ddlMachine = $("#ddlMachine").val();
    var txtFurnaceNo = $("#txtFurnaceNo").val();
    var ddlProcCondition = $("#ddlProcCondition option:selected").text();
    var txtPatternNo = $("#txtPatternNo").val();
    var txtSrchDateStart = $("#txtSrchDateStart").val();
    var txtSrchDateEnd = $("#txtSrchDateEnd").val();
    var USER_ID = $("#hdfLoginId").val();
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
    
    if (ddlMachine > 0 || txtFurnaceNo.length > 0 || ddlProcCondition.length > 0 || txtPatternNo.length > 0 || txtSrchDateStart.length > 0 || txtSrchDateEnd.length > 0) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ MACHINE_CD: ddlMachine, FURNACE_NO: txtFurnaceNo, PROD_CONDITION_STRING: ddlProcCondition, PATTERN_NO: txtPatternNo, START_DATETIME: txtSrchDateStart, END_DATETIME: txtSrchDateEnd, USER_ID: USER_ID, SORT_FIELDS: sortFields })
            //data: "{jsonData : " + jsonData + "}"
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
                        url: "/HeatTreatment/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                jsonResult = response.message;
                                // setHeatTretmentDetails(jsonResult);
                                //FillGrid(response1.message);
                                isSearchFieldExist = true;
                                $("#divMainHeatTreatMentList").html(response1.message);
                                $("#lnkEdit").focus();
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    if (isSearchFieldExist == false) {
        var message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_001_SearchField").text());
        showMessageDialog(message, "ddlMachine", WARNING);
    }
}

function bindOperatorList(search) {

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: APIURL + "/HeatTreatment/EmployeeList?name=" + search,
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            $('#ddlNameValue').find('option')
                .remove()
                .end();
            for (var i = 0; i < response.message.length; i++) {
                $('#ddlNameValue').append($("<option></option>")
                    .attr("value", response.message[i].cd)
                    .text(response.message[i].name));
            }
        })
}

$(document).on('click', '#CustomerSearch', function () {
    var name = $("#txtInputSearch").val();
    bindOperatorList(name);
})

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})


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
    HeatTreatmentGridSearch(selectedColumns);
}

function HeatTreatmentGridSearch(sortFields) {
    debugger;
    var ddlMachine = $("#ddlMachine").val();
    var txtFurnaceNo = $("#txtFurnaceNo").val();
    var ddlProcCondition = $("#ddlProcCondition").val();
    var txtPatternNo = $("#txtPatternNo").val();
    var USER_ID = $("#hdfLoginId").val();
    //var txtInspDateStart = $("#txtSrchInspectionDateStart").val();
    //var txtInspDateEnd = $("#txtSrchInspectionDateEnd").val();

    if (ddlMachine.length > 0 || txtFurnaceNo.length > 0 || ddlProcCondition.length > 0 || txtPatternNo.length > 0 /*|| txtInspDateStart.length > 0 || txtInspDateEnd.length > 0*/) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HeatTreatment/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                MACHINE_CD: ddlMachine,
                //FURNACE_NO: txtFurnaceNo, PROD_CONDITION_CD: ddlProcCondition,
                PATTERN_NO: txtPatternNo,
                //INSPECTION_DATE1: txtInspDateStart, INSPECTION_DATE2: txtInspDateEnd, 
                USER_ID: USER_ID,
                SORT_FIELDS: sortFields
            })
            //data: "{jsonData : " + jsonData + "}"
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
                        url: "/HeatTreatment/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            if (response1.isSuccess) {
                                $("#divMainHeatTreatMentList").html(response1.message);
                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
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
                message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {

        if (sortFields.length > 0) {
            var ajaxTime = new Date().getTime();
            var settings = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/HeatTreatment/GridSearch",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    USER_ID: USER_ID, 
                    sortFields: sortFields
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
                            url: "/HeatTreatment/GridSearch",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)
                            //data: "{jsonData : " + jsonData + "}"
                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#divMainHeatTreatMentList").html(response1.message);

                                }
                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                var errorCode = $.parseJSON(xhr.responseText).Message;

                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
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
                    message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        }
    }
    closepopup();
}


function btnSortClose_Click() {
    closepopup();
}

function closepopup() {
    $("#SortPopUpModal").modal('hide');
}

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/HeatTreatment/InitializeSortPopup",
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



//$(document).on('click', '#btnClose', function () { 
//    location.href = "/HeatTreatment/HeatTreatmentEditDelete";
//})
function FillEmptyGrid() {
    var data = [];
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HeatTreatment/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data)
      
    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                jsonResult = response1.message;
             
                isSearchFieldExist = true;
                $("#divMainHeatTreatMentList").html(response1.message);
               
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HT_EDIT_DELETE_List_005_SearchField").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
$(document).on('click', '#btnInitialize', function () {
    $("#ddlMachine").val(0);
    $("#ddlProcCondition").val(0);
    $("#txtFurnaceNo").val("");
    $("#txtPatternNo").val("");
    setDateTime();
    clearPanel($("#HeatTreatMentList"));
    FillEmptyGrid();
})


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
function showMessageDialog(message, focusFieldID, messageType) {
    $("#dialogMessageIcon img").hide();
    switch (messageType) {
        case WARNING:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        case ERROR:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/error.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        case SUCCESS:
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                centerVertical: true
            });
            break;
        default:
            break;
    };

} 
