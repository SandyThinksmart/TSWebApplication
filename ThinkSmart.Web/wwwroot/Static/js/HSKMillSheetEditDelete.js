var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";

$(document).ready(function () {
    $.support.cors = true;
    setDecimalFormats();
    InitSortPopup();
    $("#txtDelDate1").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtDelDate2").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
});

function setDecimalFormats() {
    $("#txtDim1,#txtDim2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLength1,#txtLength2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
}
$("#btnYobinaStartOpeartorSearch").click(function (event) {
    debugger;
    var yobina = $.trim($("#txtMakerName").val());
    setMakerNameList(yobina, "");
});
function setMakerNameList(yobina, defaultSelectID) {
    setMakerList(yobina, $("#ddlMakerName"), defaultSelectID);
};
function setMakerList(yobina, $target, defaultSelectID) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/MakerList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);

            bindMakerDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsg_COM_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindMakerDropdown(jsonResult, $target, defaultSelectID) {
    debugger;
    $("#ddlMakerName").html("");
    $("#ddlMakerName").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        //$("#ddlCustomer").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $target.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}


//Extract
$("#btnExtract").click(function (event) {
    debugger;
   if (!IsMandatoryFieldsFilled()) {
        return false;
     }
  
    btnSearch_Click();
});

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields
    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find("#tblSearch .Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.tagName.toLowerCase() === "select" && !$(mandatoryField).hasClass("disabled")) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
 });

  

    function IsInActiveOperator(dwStartOperater) {
        alert(dwStartOperater);
        var returnVal = true;  // default true is required for screens with no mandatory fields

        var message = ""; // This message will be displayed on dialog box
        var isRequired = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_001").text());

    };


    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        //$("#imgError").show();
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
function btnSearch_Click() {
    debugger;
    var CHUUBAN = $("#txtOrderNo").val();
    var CUSTOMER_NAME = $("#txtCustomer").val();
    var RECEIVER_NAME = $("#txtHeatNo").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var HT_CONDITION = $("#txtCondition").val();
    var SHAPE = $("#txtShape").val();
    var PROD_DIM_D_MIN = $("#txtDim1").val() ? $("#txtDim1").val() : null;
    var PROD_DIM_D_MAX = $("#txtDim2").val() ? $("#txtDim2").val() : null;
    var PROD_LENGHT_MIN = $("#txtLength1").val() ? $("#txtLength1").val() : null;
    var PROD_LENGHT_MAX = $("#txtLength2").val() ? $("#txtLength2").val() : null;
    var CHARGE = $("#txtCharge").val();
    var MAKER_NAME = $("#ddlMakerName").val() ? $("#ddlMakerName").val() : null;
    var DELIVERY_DATE_MIN = $("#txtDelDate1").val() ? $("#txtDelDate1").val() : null;
    var DELIVERY_DATE_MAX = $("#txtDelDate2").val() ? $("#txtDelDate2").val() : null;
    var UNIT = $("#txtIssue").val();
    var USER_ID = $("#hdfLoginId").val();
    var sortFields = [];

    $("#lstSelectedSortFields option").each(function () {
        debugger;
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            sortFields.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });

    var errorMessage = "";
    var checkFocusField = "";
    var dim = $("#ddlMakerName").val();
    var dim1 = $("#txtDim2").val();

    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }

    var CHARGE = $("#txtCharge").val();
    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || RECEIVER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || HT_CONDITION.length > 0 || SHAPE.length > 0 || parseInt(PROD_DIM_D_MIN) > 0 || parseInt(PROD_DIM_D_MAX) > 0 || parseInt(PROD_LENGHT_MIN) > 0 || parseInt(PROD_LENGHT_MAX) > 0 || CHARGE.length > 0 || MAKER_NAME.length > 0 || DELIVERY_DATE_MIN.length > 0 || DELIVERY_DATE_MAX.length > 0 || UNIT.length > 0 || USER_ID.length > 0) {
       MilkSheetListGridSearch(sortFields)
    }
    else {
        message = $.trim($("#lblErrorMessage_SearchField").text());
        showMessageDialog(message, 'txtChuubanSearch', WARNING);
    }
}

function MilkSheetListGridSearch(sortFields) {
    var CHUUBAN = $("#txtOrderNo").val();
    var CUSTOMER_NAME = $("#txtCustomer").val();
    var RECEIVER_NAME = $("#txtHeatNo").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var HT_CONDITION = $("#txtCondition").val();
    var SHAPE = $("#txtShape").val();
    var PROD_DIM_D_MIN = $("#txtDim1").val() ? $("# txtDim1").val() : 0;
    var PROD_DIM_D_MAX = $("#txtDim2").val() ? $("# txtDim2").val() : 0;
    var PROD_LENGHT_MIN = $("#txtLength1").val() ? $("#txtLength1").val() : 0;
    var PROD_LENGHT_MAX = $("#txtLength2").val() ? $("#txtLength2").val() : 0;
    var CHARGE = $("#txtCharge").val();
    var MAKER_NAME = $("#ddlMakerName").val() ? $("#ddlMakerName").val() : null;
    var DELIVERY_DATE_MIN = $("#txtDelDate1").val() ? $("#txtDelDate1").val() : null;
    var DELIVERY_DATE_MAX = $("#txtDelDate2").val() ? $("#txtDelDate2").val() : null;
    var UNIT = $("#txtIssue").val();
    var USER_ID = $("#hdfLoginId").val();
    //var SORT_FIELDS = [];
    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || RECEIVER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || HT_CONDITION.length > 0 || SHAPE.length > 0 || PROD_DIM_D_MIN.length > 0
        || PROD_DIM_D_MAX.length > 0 || PROD_LENGHT_MIN.length > 0 || PROD_LENGHT_MAX.length > 0 || CHARGE.length > 0 || MAKER_NAME.length > 0 || UNIT.length > 0 || USER_ID.length > 0) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HSKMillSheet/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, RECEIVER_NAME: RECEIVER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, HT_CONDITION: HT_CONDITION, SHAPE: SHAPE, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX, PROD_LENGHT_MIN: PROD_LENGHT_MIN,
                PROD_LENGHT_MAX: PROD_LENGHT_MAX, CHARGE: CHARGE, MAKER_NAME: MAKER_NAME, DELIVERY_DATE_MIN: DELIVERY_DATE_MIN, DELIVERY_DATE_MAX: DELIVERY_DATE_MAX, USER_ID: USER_ID, SORT_FIELDS: sortFields
            })
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    var settings1 = {
                        async: false,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/HSKMillSheet/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                            var result = response1.message;
                            if (response1.isSuccess) {
                                $("#divHSKMillSheetListGrid").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_ProdSpec_001").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                            return false;
                        });
                }
                else {
                    bootbox.alert({
                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
                        centerVertical: true
                    });
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_ProdSpec_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }
    else {

        if (sortFields.length > 0) {
            var ajaxTime = new Date().getTime();
            var settings = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/HSKMillSheet/GridSearch",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    USER_ID: USER_ID, SORT_FIELDS: sortFields
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
                            url: "/HSKMillSheet/GridSearch",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(response.message)
                        }
                        $.ajax(settings1)
                            .done(function (response1) {
                                if (response1.isSuccess) {
                                    $("#divHSKMillSheetListGrid").html(response1.message);

                                }
                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_ProdSpec_001").text());
                                showMessageDialog(message, focusFieldID, ERROR);
                                return false;
                            });
                    }
                    else {
                        bootbox.alert({
                            title: "<div style='color:white'></div>",
                            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
                            centerVertical: true
                        });
                    }

                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_ProdSpec_001").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                    return false;
                });
        }
    }
    closepopup();
}



$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})

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
    MilkSheetListGridSearch(selectedColumns);
}
function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/HSKMillSheet/InitializeSortPopupMikSheetList",
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