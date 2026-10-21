

$("#txtDimDMin").keyup(function (event) {
   
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtDimDMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthSearchMin").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtLengthSearchMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

//function btnSearch_Click() {
//    debugger;
//    var txtChuubanSearch = $("#txtChuubanSearch").val();
//    var txtTabaNo = $.trim($("#txtTabaNo").val()) ? $.trim($("#txtTabaNo").val()) : 0; //$("#txtTabaNo").val();
//    var txtProdNameCust = $("#txtProdNameCust").val();
//    var txtProdNameInt = $("#txtProdNameInt").val();
//    var txtDimDMin = $("#txtDimDMin").val();
//    var txtDimDMax = $("#txtDimDMax").val();
//    var txtShap = $("#txtShap").val();
//    var txtLengthSearchMin = $("#txtLengthSearchMin").val();
//    var txtLengthSearchMax = $("#txtLengthSearchMax").val();
//    var txtOperationDateStart = $.trim($("#txtOperationDateStart").val()) ? $.trim($("#txtOperationDateStart").val()) : null; //$("#txtOperationDateStart").val();
//    var txtOperationDateEnd = $.trim($("#txtOperationDateEnd").val()) ? $.trim($("#txtOperationDateEnd").val()) : null;  //$("#txtOperationDateEnd").val();
//    var ddlNameValue = $.trim($("#ddlNameValue").val()) ? $.trim($("#ddlNameValue").val()) : 0; //$("#ddlNameValue").val();
//    if (txtChuubanSearch.length > 0 || txtTabaNo.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0 || txtOperationDateStart.length > 0 || txtOperationDateEnd.length > 0 || (ddlNameValue.length > 0 && parseInt(ddlNameValue) > 0)) {
//        var ajaxTime = new Date().getTime();
//        var jsonData = JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart });
//        var settings = {
//            async: true,
//            crossDomain: true,
//            cache: false,
//            dataType: "json",
//            url: "/Stampping/GridSearch",
//            method: "POST",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart: txtOperationDateStart, txtOperationDateEnd: txtOperationDateEnd, scOperator: ddlNameValue.length > 0 ? parseInt(ddlNameValue) : 0 })
//            //data: "{jsonData : " + jsonData + "}"
//        }
//        $.ajax(settings)
//            .done(function (response) {
//                responseTime = new Date().getTime() - ajaxTime;
//                if (response.isSuccess)
//                    $("#divStamppingGridMain").html(response.message);
//                else {
//                    bootbox.alert({
//                        title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
//                        message: response.message,
//                        centerVertical: true
//                    });
//                }

//            })
//            .fail(function (xhr, textStatus, errorThrown) {
//                responseTime = new Date().getTime() - ajaxTime;
//                var errorCode = $.parseJSON(xhr.responseText).Message;

//                focusFieldID = $(':focus').attr("id");
//                message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
//                showMessageDialog(message, focusFieldID, ERROR);
//            });
//    }
//    else {
//        bootbox.alert({
//            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
//            message: "This is test message",
//            centerVertical: true
//        });
//    }
//}

$(document).ready(function () {
   
  //  bindOperatorList('');
    InitSortPopup();
   
})
function bindOperatorList(search) {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/Stampping/EmployeeList?name=" + search,
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
            //$.each(response.message, function (key, value) {
            //    $('#ddlNameValue').find('option')
            //        .remove()
            //        .end()
            //        .append($("<option></option>")
            //            .attr("value", key)
            //            .text(value));
            //});
        })
}

$(document).on('click', '#CustomerSearch', function () {
    var name = $("#txtInputSearch").val();
    bindOperatorList(name);
})

$(document).on('click', '#btnSort', function () {
   
    var PROD_DIM_D_MIN = $("#txtDimDMin").val();
    var PROD_DIM_D_MAX = $("#txtDimDMax").val();
   
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val();
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val();
    var errorMessage = "";
    var checkFocusField = "";

    if ($("#txtDimDMin").val().trim() == "" && $("#txtDimDMax").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_DIMD_START_REQUIRED").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_LENGTH_START_REQUIRED").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_LENGTH_START_REQUIRED").text());
        }
    }

    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }
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
    StamppingGridSearch(selectedColumns);
}

function StamppingGridSearch(sortFields) {
    var txtChuubanSearch = $("#txtChuubanSearch").val();
    var txtTabaNo = $("#txtTabaNo").val();
    var txtProdNameCust = $("#txtProdNameCust").val();
    var txtProdNameInt = $("#txtProdNameInt").val();
    var txtDimDMin = $("#txtDimDMin").val();
    var txtDimDMax = $("#txtDimDMax").val();
    var txtShap = $("#txtShap").val();
    var txtLengthSearchMin = $("#txtLengthSearchMin").val();
    var txtLengthSearchMax = $("#txtLengthSearchMax").val();
    var txtOperationDateStart = $("#txtOperationDateStart").val();
    var txtOperationDateEnd = $("#txtOperationDateEnd").val();
    var ddlNameValue = $("#ddlNameValue").val();
    if (txtChuubanSearch.length > 0 || txtTabaNo.length > 0 || txtProdNameCust.length > 0 || txtProdNameInt.length > 0 || txtDimDMin.length > 0 || txtDimDMax.length > 0 || txtShap.length > 0 || txtLengthSearchMin.length > 0 || txtLengthSearchMax.length > 0 || txtOperationDateStart.length > 0 || txtOperationDateEnd.length > 0 || (ddlNameValue.length > 0 && parseInt(ddlNameValue) > 0)) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/Stampping/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ txtChuubanSearch: txtChuubanSearch, txtTabaNo: txtTabaNo, txtProdNameCust: txtProdNameCust, txtProdNameInt: txtProdNameInt, txtDimDMin: txtDimDMin, txtDimDMax: txtDimDMax, txtShap: txtShap, txtLengthSearchMin: txtLengthSearchMin, txtLengthSearchMax: txtLengthSearchMax, txtOperationDateStart: txtOperationDateStart, txtOperationDateEnd: txtOperationDateEnd, scOperator: ddlNameValue.length > 0 ? parseInt(ddlNameValue) : 0, sortFields: sortFields })
            //data: { request: JSON.stringify(objRequest), sortFields: JSON.stringify(sortFields) }
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess)
                    $("#divStamppingGridMain").html(response.message);
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
                message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }
    else {
        bootbox.alert({
            title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
            message: "This is test message",
            centerVertical: true
        });
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
        url: "/Stampping/InitializeSortPopup",
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

function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
}