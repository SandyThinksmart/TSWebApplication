
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var typeText = "text";
var DISABLED = "disabled";

$(document).ready(function () {
    $("#LoadingImage").hide();
    InitSortPopup();
    setActualDateTime();
    Mask();
    setDecimalFormats();
    $("#btnCSV").attr("DISABLED", "DISABLED");
   /* $("#btnExcel").attr("DISABLED", "DISABLED");*/
    $("#txtChuubanSearch").focus();

});
function Mask() {
    $("#txtCompleteDateStart").val($.format.date(), "yyyy/MM/dd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtCompleteDateEnd").val($.format.date(), "yyyy/MM/dd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
}

function setDecimalFormats() {
    $("#txtDimD1,#txtDimD2").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 6, 3)
    });
    $("#txtLengthMinSearch,#txtLengthMaxSearch").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 6, 2)
    });

}
//disable CSV button on change event
$("input[type=text]").change(function () {
    $("#btnCSV").attr("DISABLED", "DISABLED");
});
$("#txtCompleteDateEnd").focusout(function () {
    $("#btnSearch").focus();
});
function Initialize_Click() {

    $("#txtChuubanSearch").val("");
    $("#txtProdNameCust").val("");
    $("#txtProdNameInt").val("");
    $("#txtDimD1").val("");
    $("#txtDimD2").val("");
    $("#txtShapeSearch").val("");
    $("#txtLengthMinSearch").val("");
    $("#txtLengthMaxSearch").val("");
    $("#txtCustomerName").val("");
    $("#txtActAllocDate1Search").val("");
    $("#txtActAllocDate2Search").val("");
    $("#txtProdSpecNo1Search").val("");
    $("#txtProdSpecNo2Search").val("");
   
    fillEmptyGrid();
    setActualDateTime();
    $("#txtChuubanSearch").focus();
    $("#btnCSV").attr("DISABLED", "DISABLED");
}
function fillEmptyGrid() {
    debugger
    var emptyList = [];
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/YieldSearch/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divYieldSearchGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProd_Search_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
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


$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})


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

function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
}

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {

        if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
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
        $("#dialogMessageIcon img").hide();

        if (focusFieldID == 'checkActualDimension') {
            if ($("#txtActual_Start").val().trim() == "") {
                focusFieldID = "txtActual_Start";
            } else {
                focusFieldID = "txtActual_End";
            }
        }

        showMessageDialog(message, focusFieldID, WARNING)

    }
    return returnVal;
}

function btnSelect_Click() {
    debugger;
    var MAXROWCOUNT = $("#txtMaxRowCount").val() ? $("#txtMaxRowCount").val() : null;
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var PROD_DIM_D_MIN = $("#txtDimD1").val();
    var PROD_DIM_D_MAX = $("#txtDimD2").val();
    var PROD_SHAPE = $("#txtShapeSearch").val();
    var PROD_LENGTH_MIN = $("#txtLengthMinSearch").val();
    var PROD_LENGTH_MAX = $("#txtLengthMaxSearch").val();
    var ACTALLOC_STARTDATE = $("#txtActAllocDate1Search").val();
    var ACTALLOC_ENDDATE = $("#txtActAllocDate2Search").val();
    var ProdSpecNo1 = $("#txtProdSpecNo1Search").val();
    var ProdSpecNo2 = $("#txtProdSpecNo2Search").val();
    
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    if (!checkDate($("#txtActAllocDate1Search"))) {

        var message = $("#lblErrorMessageE_SalesProd_Search_006").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtActAllocDate1Search", WARNING);
        return false;
    }
    if (!checkDate($("#txtActAllocDate2Search"))) {
        var message = $("#lblErrorMessageE_SalesProd_Search_007").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtActAllocDate2Search", WARNING);
        return false;
    }

    if (new Date($("#txtActAllocDate1Search").val()) > new Date($("#txtActAllocDate2Search").val())) {
        var message = $("#lblErrorMessageE_SalesProd_Search_008").text().trim();
        var focusFieldID = "txtActAllocDate2Search";
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    if ($("#txtDimD1").val().trim() == "" && $("#txtDimD2").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_SalesProd_Search_003").text());
        showMessageDialog(errorMessage, "txtDimD1", WARNING);
        return false;
    }

    if ($("#txtLengthMinSearch").val().trim() == "" && $("#txtLengthMaxSearch").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_SalesProd_Search_004").text());
        showMessageDialog(errorMessage, "txtLengthMinSearch", WARNING);
        return false;
    }
   
    if (MAXROWCOUNT > 0 || CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || parseInt(PROD_DIM_D_MIN) > 0 || parseInt(PROD_DIM_D_MAX) > 0
        || PROD_SHAPE.length > 0 || parseInt(PROD_LENGTH_MIN) > 0 || parseInt(PROD_LENGTH_MAX) > 0 || ACTALLOC_STARTDATE.length > 0 || ACTALLOC_ENDDATE.length > 0 || ProdSpecNo1.length > 0 || ProdSpecNo2.length > 0)
    {
        YieldGridSearch(null)
    }
        
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
    YieldGridSearch(selectedColumns);
}

function YieldGridSearch(sortFields) {
    debugger;
    var MAXROWCOUNT = $("#txtMaxRowCount").val() ? $("#txtMaxRowCount").val() : null;
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var PROD_DIM_D_MIN = $("#txtDimD1").val() ? $("#txtDimD1").val() : null;
    var PROD_DIM_D_MAX = $("#txtDimD2").val() ? $("#txtDimD2").val() : null;
    var PROD_SHAPE = $("#txtShapeSearch").val();
    var PROD_LENGTH_MIN = $("#txtLengthMinSearch").val() ? $("#txtLengthMinSearch").val() : null;
    var PROD_LENGTH_MAX = $("#txtLengthMaxSearch").val() ? $("#txtLengthMaxSearch").val() : null;
    var ACTALLOC_STARTDATE = $("#txtActAllocDate1Search").val();
    var ACTALLOC_ENDDATE = $("#txtActAllocDate2Search").val();
    var ProdSpecNo1 = $("#txtProdSpecNo1Search").val();
    var ProdSpecNo2 = $("#txtProdSpecNo2Search").val();
    var USER_ID = $("#hdfLoginId").val();

    if (MAXROWCOUNT > 0 || CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || parseInt(PROD_DIM_D_MIN).length > 0
        || parseInt(PROD_DIM_D_MAX).length > 0 || PROD_SHAPE.length > 0 || parseInt(PROD_LENGTH_MIN).length > 0 || parseInt(PROD_LENGTH_MAX).length > 0
        || ACTALLOC_STARTDATE.length > 0 || ACTALLOC_ENDDATE.length > 0 || ProdSpecNo1.length > 0 || ProdSpecNo2.length > 0)
    {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            beforeSend: function () {
            $("#LoadingImage").show();
        },
        complete: function () {
            $("#LoadingImage").hide();
        },
        success: function () {
            $("#LoadingImage").hide();
        },
        error: function () {
            $("#LoadingImage").hide();
        },
            dataType: "json",
            url: APIURL + "/YieldSearch/GridSearch",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                MAX_ROW_COUNT: MAXROWCOUNT, CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX,
                PROD_SHAPE: PROD_SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX, START_DATETIME: ACTALLOC_STARTDATE, END_DATETIME: ACTALLOC_ENDDATE,
                PROD_SPEC_NO1: ProdSpecNo1, PROD_SPEC_NO2: ProdSpecNo2, USER_ID: USER_ID, SORT_FIELDS: sortFields, CULTURE_NAME: $("#hdfCultureName").val()
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
                        url: "/YieldSearch/GridSearch",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                    }
                    $.ajax(settings1)
                        .done(function (response1) {

                            var jsonResult = response.message.length;
                            if (jsonResult == 0) {
                                $("#btnCSV").attr("DISABLED", "DISABLED");
                                message = $.trim($("#lblErrorMessageE_SalesProd_Search_002").text());
                                showMessageDialog(message, "txtChuubanSearch", WARNING);
                                return false;

                            }
                            if (response1.isSuccess) {
                                $("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
                                $("#divYieldSearchGridMain").html(response1.message);

                            }
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_SalesProd_Search_001").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                            return false;
                        });
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_SalesProd_Search_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

    closepopup();
}

function closepopup() {
    $("#SortPopUpModal").modal('hide');
}

function setActualDateTime() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/YieldSearch/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            var jsonResult = response.message;
            $("#txtActAllocDate1Search").val($.format.date(new Date(jsonResult.starT_DATETIME_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtActAllocDate2Search").val($.format.date(new Date(jsonResult.enD_DATETIME_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProd_Search_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function checkDate(field) {

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

//function OnEditClick(editButton) {
//    var ProcessPlanCD = $.trim(editButton.closest("tr").attr("data-lblProcessPlanCD"));
//    // location.href = '/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=' + ProcessPlanCD;
//    window.open("/SalesProductionStatusSearch/SalesProductionStatusSearch2?ProcessPlanCD=" + ProcessPlanCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1900 + ", height=" + 1490);
//}

$(document).on('click', '#btnCSV', function () {
    $("#btnCSV").attr("DISABLED", "DISABLED");
    debugger;


    var MAXROWCOUNT = $("#txtMaxRowCount").val() ? $("#txtMaxRowCount").val() : null;
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var PROD_DIM_D_MIN = $("#txtDimD1").val();
    var PROD_DIM_D_MAX = $("#txtDimD2").val();
    var PROD_SHAPE = $("#txtShapeSearch").val();
    var PROD_LENGTH_MIN = $("#txtLengthMinSearch").val();
    var PROD_LENGTH_MAX = $("#txtLengthMaxSearch").val();
    var ACTALLOC_STARTDATE = $("#txtActAllocDate1Search").val();
    var ACTALLOC_ENDDATE = $("#txtActAllocDate2Search").val();
    var ProdSpecNo1 = $("#txtProdSpecNo1Search").val();
    var ProdSpecNo2 = $("#txtProdSpecNo2Search").val();
    

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    if (!checkDate($("#txtActAllocDate1Search"))) {

        var message = $("#lblErrorMessageE_SalesProd_Search_006").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtActAllocDate1Search", WARNING);
        return false;
    }
    if (!checkDate($("#txtActAllocDate2Search"))) {
        var message = $("#lblErrorMessageE_SalesProd_Search_007").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtActAllocDate2Search", WARNING);
        return false;
    }

    if ($("#txtDimD1").val().trim() == "" && $("#txtDimD2").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_SalesProd_Search_003").text());
        showMessageDialog(errorMessage, "txtDimD1", WARNING);
        return false;
    }

    if ($("#txtLengthMinSearch").val().trim() == "" && $("#txtLengthMaxSearch").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_SalesProd_Search_004").text());
        showMessageDialog(errorMessage, "txtLengthMinSearch", WARNING);
        return false;
    }
    //if ($("#txtCompleteDateStart").val() != "") {
    //    if (!checkDate($("#txtCompleteDateStart"))) {

    //        var message = $("#lblErrorMessageE_SalesProd_Search_006").text().trim(); // This message will be displayed on dialog box
    //        showMessageDialog(message, "txtCompleteDateStart", WARNING);
    //        return false;
    //    }
    //}
    //if ($("#txtCompleteDateEnd").val() != "") {

    //    if (!checkDate($("#txtCompleteDateEnd"))) {
    //        var message = $("#lblErrorMessageE_SalesProd_Search_007").text().trim(); // This message will be displayed on dialog box
    //        showMessageDialog(message, "txtCompleteDateEnd", WARNING);
    //        return false;
    //    }
    //}
    //if ($("#txtCompleteDateStart").val() != "" && $("#txtCompleteDateEnd").val() != "") {
    //    if (new Date($("#txtCompleteDateStart").val()) > new Date($("#txtCompleteDateEnd").val())) {
    //        var message = $("#lblErrorMessageE_SalesProd_Search_009").text().trim();
    //        var focusFieldID = "txtCompleteDateEnd";
    //        showMessageDialog(message, focusFieldID, WARNING);
    //        return false;
    //    }
    //}

    //if ($("#txtCompleteDateStart").val() == "" && $("#txtCompleteDateEnd").val() != "") {
    //    var message = $("#lblErrorMessageE_SalesProd_Search_010").text().trim();
    //    var focusFieldID = "txtCompleteDateStart";
    //    showMessageDialog(message, focusFieldID, WARNING);
    //    return false;
    //}

    if (MAXROWCOUNT > 0 || CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || parseInt(PROD_DIM_D_MIN) > 0 || parseInt(PROD_DIM_D_MAX) > 0
        || PROD_SHAPE.length > 0 || parseInt(PROD_LENGTH_MIN) > 0 || parseInt(PROD_LENGTH_MAX) > 0 || ACTALLOC_STARTDATE.length > 0 || ACTALLOC_ENDDATE.length > 0 ||
         ProdSpecNo1.length > 0 || ProdSpecNo2.length > 0) {

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
        SalesProductionStatusCSV(selectedColumns)
        //$("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
    }

});

function SalesProductionStatusCSV(sortFields) {

    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var PROD_DIM_D_MIN = $("#txtDimD1").val() ? $("#txtDimD1").val() : null;
    var PROD_DIM_D_MAX = $("#txtDimD2").val() ? $("#txtDimD2").val() : null;
    var PROD_SHAPE = $("#txtShapeSearch").val();
    var PROD_LENGTH_MIN = $("#txtLengthMinSearch").val() ? $("#txtLengthMinSearch").val() : null;
    var PROD_LENGTH_MAX = $("#txtLengthMaxSearch").val() ? $("#txtLengthMaxSearch").val() : null;
    var ACTALLOC_STARTDATE = $("#txtActAllocDate1Search").val();
    var ACTALLOC_ENDDATE = $("#txtActAllocDate2Search").val();
    var ProdSpecNo1 = $("#txtProdSpecNo1Search").val();
    var ProdSpecNo2 = $("#txtProdSpecNo2Search").val();
    var USER_ID = $("#hdfLoginId").val();
    var cultureName = $("#hdfCultureName").val()

    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || parseInt(PROD_DIM_D_MIN).length > 0
        || parseInt(PROD_DIM_D_MAX).length > 0 || PROD_SHAPE.length > 0 || parseInt(PROD_LENGTH_MIN).length > 0 || parseInt(PROD_LENGTH_MAX).length > 0
        || ACTALLOC_STARTDATE.length > 0 || ACTALLOC_ENDDATE.length > 0 || ProdSpecNo1.length > 0 || ProdSpecNo2.length > 0)
    {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/YieldSearch/ExportCSV",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, PROD_DIM_D_MIN: PROD_DIM_D_MIN,
                PROD_DIM_D_MAX: PROD_DIM_D_MAX, PROD_SHAPE: PROD_SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX,
                 START_DATETIME: ACTALLOC_STARTDATE, END_DATETIME: ACTALLOC_ENDDATE, PROD_SPEC_NO1: ProdSpecNo1,
                PROD_SPEC_NO2: ProdSpecNo2, USER_ID: USER_ID, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields,
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;

                var data = response.message;
                debugger;
                window.location = data;

                setTimeout(function () {
                   // $("#btnCSV").prop("disabled", false);
                    $("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
                }, 3000); 
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_SalesProd_Search_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

}
