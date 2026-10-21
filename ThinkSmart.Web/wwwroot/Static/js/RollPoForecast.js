var typeDropDown = "select-one";
var typeText = "text";
var DISABLED = "disabled";
var TabTitle1 = "";
$(document).ready(function () {
    window.resizeTo(1330, 1100);

    $("#btnCSV").attr("DISABLED", "DISABLED");
    setRequestedDateTime();


});
$('#btnSort').click(function () {
    debugger;
    InitSortPopup();
    $("#SortPopUpModal").modal('show');
});
function InitSortPopup() {

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/RollPoForecast/InitializeSortPopup",
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

    EnableDisableSortButtons();
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
    RollPoForecastGridSearch(selectedColumns);
}
function RollPoForecastGridSearch(sortFields) {
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    CalculateAllGridMonthRoll();
    var customerId = $("#ddlCustomerName").val();
    var forecastYear = $("#txtForecastYear").val().trim();
    var forecastRoll = $("#txtForecastRoll").val().trim();
    var ajaxTime = new Date().getTime();
    var settings = {
        //async: false,
        //crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CUSTOMER_CD: customerId, ROLL_YEAR: forecastYear, ROLL_R: forecastRoll, SORT_FIELDS: sortFields })
    };

    $.ajax(settings)
        .done(function (response) {

            var result = response.message;
            if (result.producT_LIST.length > 0) {
                $("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
                ProductList = response.message;
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/GridSearchProductList",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(result.producT_LIST)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                        debugger;
                        var result = response.message;


                        $("#pnlProductGrid").html(response.message);
                        CalculateAllGridMonthRoll();

                    })

                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#valMsgCOMSystemException").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
            if (result.materiaL_LIST.length > 0) {
                $("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/GridSearchMaterialList",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(result.materiaL_LIST)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {

                        var result = response.message;


                        $("#pnlMaterialGrid").html(response.message);
                        CalculateAllGridMonthRoll();
                    })

                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#valMsgCOMSystemException").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            $("#btnCSV").attr("DISABLED", "DISABLED");
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function btnSortReset_Click() {
    InitSortPopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
}

// Set Today Date
function setRequestedDateTime() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            var jsonResult = response.message;
            $("#txtRequestedDate").val($.format.date(new Date(jsonResult.starT_DATETIME_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

// Bind Incharge Dropdown
$("#btnInchargeSearch").click(function (event) {

    var yobina = $.trim($("#ddlInchargeName").val());
    setInchargeDetails(yobina, "");
});
function setInchargeDetails(yobina, defaultSelectID) {
    getInchargeList(yobina, $("#ddlInchargeName"), defaultSelectID);
};
function getInchargeList(yobina, $target) {
    debugger;
    var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetInchargeList?name=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            //ddlOperatorName
            var result = response.message;
            bindInchargeDropdown(response.message, $target, yobina);

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#valMsgCOMSystemException").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}
function bindInchargeDropdown(jsonResult, $target, defaultSelectID) {
    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

// Bind Customer Dropdown
$("#btnCustomerSearch").click(function (event) {

    var yobina = $.trim($("#ddlCustomerName1").val());
    setCustomerDetails(yobina, "");
});
function setCustomerDetails(yobina, defaultSelectID) {
    getCustomerList(yobina, $("#ddlCustomerName1"), defaultSelectID);
};
function getCustomerList(yobina, $target) {
    /*debugger;*/
    var ajaxTime = new Date().getTime();
    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetCustomerList?name=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}",

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            //ddlOperatorName
            var result = response.message;
            bindCustomerDropdown(response.message, $target, yobina);

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#valMsgCOMSystemException").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}
function bindCustomerDropdown(jsonResult, $target, defaultSelectID) {
    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

// Set Roller
function setRequestedDateTime() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            var jsonResult = response.message;
            $("#txtRequestedDate").val($.format.date(new Date(jsonResult.starT_DATETIME_STRING), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}



$("#btnExtract").click(function () {
    debugger;
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    CalculateAllGridMonthRoll();
    var customerId = $("#ddlCustomerName").val();
    var forecastYear = $("#txtForecastYear").val().trim();
    var forecastRoll = $("#txtForecastRoll").val().trim();
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
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CUSTOMER_CD: customerId, ROLL_YEAR: forecastYear, ROLL_R: forecastRoll, SORT_FIELDS: sortFields })
    };

    $.ajax(settings)
        .done(function (response) {

            var result = response.message;
            if (result.producT_LIST.length > 0) {
                $("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
                ProductList = response.message;
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/GridSearchProductList",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(result.producT_LIST)
                    // data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                        debugger;
                        var result = response.message;


                        $("#pnlProductGrid").html(response.message);
                        CalculateAllGridMonthRoll();

                    })

                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#valMsgCOMSystemException").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
            else {
                /*$("#btnExportCSV").attr("DISABLED", "DISABLED");*/
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblProductDataNotFOund").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            if (result.materiaL_LIST.length > 0) {
                $("#btnCSV").removeAttr("DISABLED").removeClass("DISABLED");
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/GridSearchMaterialList",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(result.materiaL_LIST)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                        debugger;
                        var result = response.message;


                        $("#pnlMaterialGrid").html(response.message);
                        CalculateAllGridMonthRoll();
                    })

                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#valMsgCOMSystemException").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }
            else {
                /*$("#btnExportCSV").attr("DISABLED", "DISABLED");*/
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblMaterialDataNotFOund").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            $("#btnCSV").attr("DISABLED", "DISABLED");
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


});

function CalculateAllGridMonthRoll() {
    currentR = $("#txtForecastRoll").val();

    var resultMonth = caldispatchHistoryMonthHeaders(currentR);
    $("#lblHistoryMonth3").text($("#lblHistoryMonth3").data("template").replace("nn", resultMonth.historyMonth_M3));
    $("#lblHistoryMonth2").text($("#lblHistoryMonth2").data("template").replace("nn", resultMonth.historyMonth_M2));
    $("#lblHistoryMonth1").text($("#lblHistoryMonth1").data("template").replace("nn", resultMonth.historyMonth_M1));

    var resultRoll = calLastForecastRollHeaders(currentR);
    $("#lblLastRoll1").text($("#lblLastRoll1").data("template").replace("xx", resultRoll.forecastRoll_R1));
    $("#lblLastRoll2").text($("#lblLastRoll2").data("template").replace("xx", resultRoll.forecastRoll_R2));
    $("#lblLastRoll3").text($("#lblLastRoll3").data("template").replace("xx", resultRoll.forecastRoll_R3));
    $("#lblLastRoll4").text($("#lblLastRoll4").data("template").replace("xx", resultRoll.forecastRoll_R4));
    var resultMonthForRoll = calLastForecastMonthForRollHeaders(currentR);
    $("#lblRollForMonth1").text($("#lblRollForMonth1").data("template").replace("nn", resultMonthForRoll.forecastMonth_M1));
    $("#lblRollForMonth2").text($("#lblRollForMonth2").data("template").replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblRollForMonth3").text($("#lblRollForMonth3").data("template").replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblRollForMonth4").text($("#lblRollForMonth4").data("template").replace("nn", resultMonthForRoll.forecastMonth_M4));
    //Set Material Grid Roll and Month
    $("#lblMaterilGridLastRoll1").text($("#lblMaterilGridLastRoll1").data("template").replace("xx", resultRoll.forecastRoll_R1));
    $("#lblMaterilGridLastRoll2").text($("#lblMaterilGridLastRoll2").data("template").replace("xx", resultRoll.forecastRoll_R2));
    $("#lblMaterilGridLastRoll3").text($("#lblMaterilGridLastRoll3").data("template").replace("xx", resultRoll.forecastRoll_R3));
    $("#lblMaterilGridLastRoll4").text($("#lblMaterilGridLastRoll4").data("template").replace("xx", resultRoll.forecastRoll_R4));
    $("#lblMaterilGridRollForMonth1").text($("#lblMaterilGridRollForMonth1").data("template").replace("nn", resultMonthForRoll.forecastMonth_M1));
    $("#lblMaterilGridRollForMonth2").text($("#lblMaterilGridRollForMonth2").data("template").replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblMaterilGridRollForMonth3").text($("#lblMaterilGridRollForMonth3").data("template").replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblMaterilGridRollForMonth4").text($("#lblMaterilGridRollForMonth4").data("template").replace("nn", resultMonthForRoll.forecastMonth_M4));
    //Set Footer Grid Roll and Month
    $("#lblFooterLastRoll1").text($("#lblFooterLastRoll1").text().replace("xx", resultRoll.forecastRoll_R2));
    $("#lblFooterLastRoll2").text($("#lblFooterLastRoll2").text().replace("xx", resultRoll.forecastRoll_R3));
    $("#lblFooterLastRoll3").text($("#lblFooterLastRoll3").text().replace("xx", resultRoll.forecastRoll_R4));
    $("#lblFooterRollForMonth1").text($("#lblFooterRollForMonth1").text().replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblFooterRollForMonth2").text($("#lblFooterRollForMonth2").text().replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblFooterlRollForMonth3").text($("#lblFooterlRollForMonth3").text().replace("nn", resultMonthForRoll.forecastMonth_M4));


}


$("#btnInitialize").click(function (event) {
    initialize();

});
function initialize() {
    $("#txtCustomer").val("");
    $("#ddlCustomerName").empty();
    $("#txtForecastYear").val("");
    $("#txtForecastRoll").val("");

    clearTable($("#tblProductGrid"), "", "GridNormalRow");
    clearTable($("#tblMaterialGrid"), "", "GridNormalRow");

    $("#btnCSV").attr("DISABLED", "DISABLED");

}

function clearTable($table, trId, trClass) {

    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}


//$("#btnCSV").click(function () {
//    let csv = [];
//    $("#tblProductGrid").each(function () {
//        let row = [];
//        $(this).find("th, td").each(function () {
//            row.push($(this).text().trim());
//        });
//        csv.push(row.join(","));
//    });

//    let csvString = csv.join("\n");
//    let blob = new Blob([csvString], { type: "text/csv" });
//    let url = URL.createObjectURL(blob);
//    let a = $("<a>").attr({ href: url, download: "data.csv" })[0];
//    a.click();
//});



function IsMandatoryFieldsFilled() {
    var returnVal = true;

    var message = "";
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
    }

    return returnVal;

}


$(document).on('click', '#btnCSV', function () {
    debugger

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
    downloadProductSalesForecastDataCSV(selectedColumns);

});


function downloadProductSalesForecastDataCSV(sortFields) {
    debugger;
    var ajaxTime = new Date().getDate();
    var customerId = $("#ddlCustomerName").val() ? $("#ddlCustomerName").val() : null;
    var forecastYear = $("#txtForecastYear").val() ? $("#txtForecastYear").val() : null;
    var forecastRoll = $("#txtForecastRoll").val() ? $("#txtForecastRoll").val() : null;
    var cultureName = $("#requestCulture_RequestCulture_UICulture_Name").val();
    var monthRollHeaders = {
        month1: $('#lblHistoryMonth1').text().trim(),
        month2: $('#lblHistoryMonth2').text().trim(),
        month3: $('#lblHistoryMonth3').text().trim(),
        roll1: $('#lblLastRoll1').text().trim(),
        roll2: $('#lblLastRoll2').text().trim(),
        roll3: $('#lblLastRoll3').text().trim(),
        roll4: $('#lblLastRoll4').text().trim(),
    };

    var body = {
        /*CUSTOMER_CD: customerId,*/
        ROLL_FC_YEAR: forecastYear,
        ROLL_FC_R: forecastRoll,
        CULTURE_NAME: cultureName,
        SORT_FIELDS: sortFields,
        MonthRollHeaders: monthRollHeaders
    }

    if (customerId && customerId !== "0") {
        body.CUSTOMER_CD = customerId;
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoForecast/ExportCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(body)

    };
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblProductDataNotFOund").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                //alert("Product csv download");
                var selectedColumns1 = [];
                $("#lstSelectedSortFields option").each(function () {
                    var $this = $(this);
                    if ($this.length) {
                        var selText = $this.text().split(' ');
                        var selValue = $this.val();
                        var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
                        selectedColumns1.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
                    }
                });
                downloadMaterialSalesForecastDataCSV(selectedColumns1);
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

function downloadMaterialSalesForecastDataCSV(sortFields) {
    debugger;
    //alert("Material csv call");
    var ajaxTime = new Date().getDate();
    var customerId = $("#ddlCustomerName").val() ? $("#ddlCustomerName").val() : null;
    var forecastYear = $("#txtForecastYear").val() ? $("#txtForecastYear").val() : null;
    var forecastRoll = $("#txtForecastRoll").val() ? $("#txtForecastRoll").val() : null;
    var cultureName = $("#requestCulture_RequestCulture_UICulture_Name").val();
    var monthRollHeaders = {
        month1: $('#lblHistoryMonth1').text().trim(),
        month2: $('#lblHistoryMonth2').text().trim(),
        month3: $('#lblHistoryMonth3').text().trim(),
        roll1: $('#lblLastRoll1').text().trim(),
        roll2: $('#lblLastRoll2').text().trim(),
        roll3: $('#lblLastRoll3').text().trim(),
        roll4: $('#lblLastRoll4').text().trim(),
    };

    var body = {
        /*CUSTOMER_CD: customerId,*/
        ROLL_FC_YEAR: forecastYear,
        ROLL_FC_R: forecastRoll,
        CULTURE_NAME: cultureName,
        SORT_FIELDS: sortFields,
        MonthRollHeaders: monthRollHeaders
    }

    if (customerId && customerId !== "0") {
        body.CUSTOMER_CD = customerId;
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoForecast/ExportMaterialCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(body)

    };
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblMaterialDataNotFOund").text());
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

