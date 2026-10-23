var dbOperation;
var typeDropDown = "select-one";
var $ActiveID;
$(document).ready(function () {
    $("#ddlCustomerName").css('width', '200px');
    $("#ddlInchargeName").css('width', '200px');
    document.getElementById("materialData").value = "";
    setRequestedDateTime();
    setRoll_R();
    disableButtons();
    setDecimalFormatsOnProduct();
   
});



$(document).on('keyup', '#tblProductGrid input', function () {

    // Store active input's ID
    $("#activeTabID").val(this.id);

    var $row = $(this).closest('tr'); // or closest container for your inputs
    var total = 0;

    // Sum all numeric inputs in this row with class 'historyMonth'
    $row.find('input.historyMonth').each(function () {
        var val = parseFloat($(this).val()) || 0;
        total += val;
    });

    // Find the result input dynamically in the same row
    $row.find('input.historyMonthResult').val(total);
});

$("#btnInitialize").click(function (event) {
    initialize();
    document.getElementById("materialData").value = "";
    $("#activeTabID").val("TbSalesProductTab");
    var targetValue = "titleTbSalesProductTab1"
    $ActiveID = targetValue;
});
function initialize() {
    $("#ddlInchargeName").empty();
    $("#ddlCustomerName").empty();
    $("#chkForecastDone").prop("checked", false);
    clearTable($("#tblMaterialGrid"), "", "GridNormalRow"); 
    clearTable($("#tblProductGrid"), "", "GridNormalRow"); 
    $grdvMessage = $("#tblProductGrid");
    $grdvMessage.find("tr.GridNormalRow").remove();
    AddNewRowStartOfProductGrid();
    disableButtons();
}
function clearTable($table, trId, trClass) {
    // If class is provided, clear rows by class (empty cells in those rows)
    if (trClass != "") {
        $table.find("tr." + trClass).each(function () {
            $(this).find("td").empty(); // Empty the cells, not remove the rows
        });
    }                                                                   
    // If ID is provided, clear rows by ID suffix (empty cells in those rows)
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").each(function () {
            $(this).find("td").empty(); // Empty the cells, not remove the rows
        });
    }
    // If neither class nor ID is provided, clear all rows
    else {
        $table.find("tr").each(function () {
            $(this).find("td").empty(); // Empty the cells in all rows
        });
    }
}
function btnSortReset_Click() {
    
    if ($ActiveID == "titleTbSalesProductTab1") {
        InitSortPopupOnProduct();
    }
    else {
        InitSortPopupOnMaterial();
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
    if ($ActiveID == "titleTbSalesProductTab1") {
        GetProductMasterDataList(selectedColumns);
    }
    else {
        GetMaterialMasterDataList(selectedColumns);
    }


}

function setDecimalFormatsOnProduct() {
    $("#txtDiaMin,#txtDiaMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthMin,#txtLengthMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

}
function setDecimalFormatsOnMaterial() {
    $("#txtDiaMinOnMatl,#txtDiaMaxOnMatl").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthMinOnMatl,#txtLengthMaxOnMatl").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

}
function IsMandatoryDateSearchFieldsFilledOnProduct() {
   
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";
    if ($("#txtDiaMin").val().trim() == "" && $("#txtDiaMax").val().trim() != "") {
        message = $.trim($("#lblErrorMessageE_DimDStartRequired").text());
        focusFieldID = 'txtDiaMin'
        returnVal = false;
    }

    if ($("#txtLengthMin").val().trim() == "" && $("#txtLengthMax").val().trim() != "") {
        if (focusFieldID == "") {
            focusFieldID = 'txtLengthMin';
            returnVal = false;
        }
        if (message == "") {
            message = $.trim($("#lblErrorMessageE_LengthStartRequired").text());
            returnVal = false;
        } else {
            message = message + "<br>" + $.trim($("#lblErrorMessageE_LengthStartRequired").text());
            returnVal = false;
        }
    }
    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
}
function IsMandatoryDateSearchFieldsFilledOnMaterial() {
    
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";
    if ($("#txtDiaMinOnMatl").val().trim() == "" && $("#txtDiaMaxOnMatl").val().trim() != "") {
        message = $.trim($("#lblErrorMessageE_DimDStartRequired").text());
        focusFieldID = 'txtDiaMin'
        returnVal = false;
    }

    if ($("#txtLengthMinOnMatl").val().trim() == "" && $("#txtLengthMaxOnMatl").val().trim() != "") {
        if (focusFieldID == "") {
            focusFieldID = 'txtLengthMin';
            returnVal = false;
        }
        if (message == "") {
            message = $.trim($("#lblErrorMessageE_LengthStartRequired").text());
            returnVal = false;
        } else {
            message = message + "<br>" + $.trim($("#lblErrorMessageE_LengthStartRequired").text());
            returnVal = false;
        }
    }
    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
}

$("#btnSearchProduct").click(function () {
    if (!IsMandatoryDateSearchFieldsFilledOnProduct()) {
        return false;
    }
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
    var customer_NAME = $("#txtCustomerOnProd").val() ? $("#txtCustomerOnProd").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var prod_Name = $("#txtProdNameCust").val() ? $("#txtProdNameCust").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    if (customer_NAME != null || internal_Name != null || prod_Name != null || dia_Min != null || dia_Max != null || shape != null || length_Min != null || length_Max != null || prod_Spec_No1 != null || prod_Spec_No2 != null) {
        GetProductMasterDataList(selectedColumns);
    }
    else {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgForSearchCondition").text());
        showMessageDialog(message, "", WARNING);
    }
    
});

$("#btnSearchMaterial").click(function () {
    if (!IsMandatoryDateSearchFieldsFilledOnMaterial()) {
        return false;
    }
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
    GetMaterialMasterDataList(selectedColumns);
});
function GetProductMasterDataList(sortFields) {
    var ajaxTime = new Date().getTime();
    var customer_NAME = $("#txtCustomerOnProd").val() ? $("#txtCustomerOnProd").val() : null;
    var internal_Name = $("#txtInternalName").val() ? $("#txtInternalName").val() : null;
    var prod_Name = $("#txtProdNameCust").val() ? $("#txtProdNameCust").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var shape = $("#txtShape").val() ? $("#txtShape").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var prod_Spec_No1 = $("#txtProdSpecNo1").val() ? $("#txtProdSpecNo1").val() : null;
    var prod_Spec_No2 = $("#txtProdSpecNo2").val() ? $("#txtProdSpecNo2").val() : null;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetProductMasterData",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            CUSTOMER_NAME: customer_NAME, PROD_NAME_CUST: prod_Name, INTERNAL_NAME_STRING: internal_Name, SHAPE: shape, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_LENGTH_MIN: length_Min, PROD_LENGTH_MAX: length_Max, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            ProductList = response.message;
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoForecast/ProductMasterListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response) {
                    var result = response.message;
                    $("#divProductMasterListGrid").html(response.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function GetMaterialMasterDataList(sortFields) {
    var ajaxTime = new Date().getTime();
    var maker_NAME = $("#txtMakerNameOnMatl").val() ? $("#txtMakerNameOnMatl").val() : null;
    var internal_Name = $("#txtInternalNameOnMatl").val() ? $("#txtInternalNameOnMatl").val() : null;
    var material_Name_Maker = $("#txtRawMaterialNameOnMatl").val() ? $("#txtRawMaterialNameOnMatl").val() : null;
    var dia_Min = $("#txtDiaMinOnMatl").val() ? $("#txtDiaMinOnMatl").val() : null;
    var dia_Max = $("#txtDiaMaxOnMatl").val() ? $("#txtDiaMaxOnMatl").val() : null;
    var shape = $("#txtShapeOnMatl").val() ? $("#txtShapeOnMatl").val() : null;
    var length_Min = $("#txtLengthMinOnMatl").val() ? $("#txtLengthMinOnMatl").val() : null;
    var length_Max = $("#txtLengthMaxOnMatl").val() ? $("#txtLengthMaxOnMatl").val() : null;
    var Material_Spl_Name = $("#ddlMatlSpecName").val() ? $("#ddlMatlSpecName").val() : null;
    var Material_Type_cbk = $("#ddlMaterialType").val() ? $("#ddlMaterialType").val() : null;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetMaterialMasterData",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            MAKER_NAME: maker_NAME, INTERNAL_NAME_STRING: internal_Name, MATL_SHAPE: shape, MATL_MAKER_NAME: material_Name_Maker,
            MATL_DIM_D_MIN: dia_Min, MATL_DIM_D_MAX: dia_Max, MATL_LENGTH_MIN: length_Min, MATL_LENGTH_MAX: length_Max, MATL_SPECIAL_KUBUN: Material_Spl_Name,
            MATL_TYPE_CBK: Material_Type_cbk, SORT_FIELDS: sortFields
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            ProductList = response.message;
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoForecast/MaterialMasterListGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response) {

                    var result = response.message;
                    $("#divMaterialMasterListGrid").html(response.message);
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
function btnSortClose_Click() {

    closepopup();
}
function InitSortPopupOnProduct() {
   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/RollPoForecast/InitializeSortPopupForProductList",
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
function InitSortPopupOnMaterial() {
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/RollPoForecast/InitializeSortPopupForMaterialList",
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
            $("#txtRequestedDate").attr("disabled", "disabled");
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

// Bind Incharge Dropdown
$("#btnInchargeSearch").click(function (event) {
    debugger;
    var yobina = $.trim($("#txtIncharge").val());
    setInchargeDetails(yobina, "");
});
function setInchargeDetails(yobina, defaultSelectID) {
    getInchargeList(yobina, $("#ddlInchargeName"), defaultSelectID);
};
function getInchargeList(yobina, $target) {

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
            message = $("#lblErrorMessageE_PostHTInspection").text().trim();
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


// Set RollerbtnInchargeSearch
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
            message = $.trim($("#lblErrorMessageE_SalesProd_Search_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function setRoll_R() {
    debugger;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/GetForecastRoll",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult != null) {
                $("#lblForecastR").text(jsonResult.currenT_R);
                $("#hdcurrent_yyyy").text(jsonResult.currenT_YYYY);
                CalculateAllGridMonthRoll();
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
function disableButtons() {
    $("#btnRegister").attr("disabled", "disabled");
     $("#btnAddRowBelow").attr("disabled", "disabled");
    $("#btnAddRowAbove").attr("disabled", "disabled");
    $("#btnDeleteRow").attr("disabled", "disabled");
    $("#btnSpecInput1").attr("disabled", "disabled");
    $("#btnSpecInput2").attr("disabled", "disabled");
    const table = document.getElementById("tblProductGrid");
    const elements = table.querySelectorAll("input, button, select, textarea");
    elements.forEach(el => el.disabled = true);
    $("#TbSalesMaterialTab").addClass("disabled").css({
            "pointer-events": "none",
            "opacity": "0.5"
        });
}    
function enableButtons() {
    $("#btnRegister").removeAttr("disabled");
    $("#btnAddRowBelow").removeAttr("disabled");
    $("#btnAddRowAbove").removeAttr("disabled");
    $("#btnDeleteRow").removeAttr("disabled");
    const table = document.getElementById("tblProductGrid");
    const elements = table.querySelectorAll("input, button, select, textarea");
    elements.forEach(el => el.disabled = false);
    $("#TbSalesMaterialTab").removeClass("disabled").css({
        "pointer-events": "",
            "opacity": ""
        });
}

$("#ddlCustomerName").change(function () {
    $("#btnSpecInput1").removeAttr("disabled");
    $("#btnSpecInput2").removeAttr("disabled");  
});
function disableSearchCriteria() {
    $("#ddlCustomerName").attr("disabled", "disabled");
    $("#txtIncharge").attr("disabled", "disabled");
    $("#btnInchargeSearch").attr("disabled", "disabled");
    $("#ddlInchargeName").attr("disabled", "disabled");
    $("#btnSpecInput1").attr("disabled", "disabled");
    $("#btnSpecInput2").attr("disabled", "disabled");
}
function CalculateAllGridMonthRoll() {
    
    currentR = $("#lblForecastR").text();
    var resultMonth = caldispatchHistoryMonthHeaders(currentR);
    $("#lblHistoryMonth3").text($("#lblHistoryMonth3").text().replace("nn", resultMonth.historyMonth_M3));
    $("#lblHistoryMonth2").text($("#lblHistoryMonth2").text().replace("nn", resultMonth.historyMonth_M2));
    $("#lblHistoryMonth1").text($("#lblHistoryMonth1").text().replace("nn", resultMonth.historyMonth_M1));
    var resultRoll = calLastForecastRollHeaders(currentR);
    $("#lblLastRoll1").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R1));
    $("#lblLastRoll2").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
    $("#lblLastRoll3").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R3));
    $("#lblLastRoll4").text($("#lblLastRoll4").text().replace("xx", resultRoll.forecastRoll_R4));
    var resultMonthForRoll = calLastForecastMonthForRollHeaders(currentR);
    $("#lblRollForMonth1").text($("#lblRollForMonth1").text().replace("nn", resultMonthForRoll.forecastMonth_M1));
    $("#lblRollForMonth2").text($("#lblRollForMonth2").text().replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblRollForMonth3").text($("#lblRollForMonth3").text().replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblRollForMonth4").text($("#lblRollForMonth4").text().replace("nn", resultMonthForRoll.forecastMonth_M4));
    //Set Material Grid Roll and Month
    $("#lblMaterilGridLastRoll1").text($("#lblMaterilGridLastRoll1").text().replace("xx", resultRoll.forecastRoll_R1));
    $("#lblMaterilGridLastRoll2").text($("#lblMaterilGridLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
    $("#lblMaterilGridLastRoll3").text($("#lblMaterilGridLastRoll3").text().replace("xx", resultRoll.forecastRoll_R3));
    $("#lblMaterilGridLastRoll4").text($("#lblMaterilGridLastRoll4").text().replace("xx", resultRoll.forecastRoll_R4));
    $("#lblMaterilGridRollForMonth1").text($("#lblMaterilGridRollForMonth1").text().replace("nn", resultMonthForRoll.forecastMonth_M1));
    $("#lblMaterilGridRollForMonth2").text($("#lblMaterilGridRollForMonth2").text().replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblMaterilGridRollForMonth3").text($("#lblMaterilGridRollForMonth3").text().replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblMaterilGridRollForMonth4").text($("#lblMaterilGridRollForMonth4").text().replace("nn", resultMonthForRoll.forecastMonth_M4));
    //Set Footer Grid Roll and Month
    $("#lblFooterLastRoll1").text($("#lblFooterLastRoll1").text().replace("xx", resultRoll.forecastRoll_R2));
    $("#lblFooterLastRoll2").text($("#lblFooterLastRoll2").text().replace("xx", resultRoll.forecastRoll_R3));
    $("#lblFooterLastRoll3").text($("#lblFooterLastRoll3").text().replace("xx", resultRoll.forecastRoll_R4));
    $("#lblFooterRollForMonth1").text($("#lblFooterRollForMonth1").text().replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblFooterRollForMonth2").text($("#lblFooterRollForMonth2").text().replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblFooterlRollForMonth3").text($("#lblFooterlRollForMonth3").text().replace("nn", resultMonthForRoll.forecastMonth_M4));


}


function AddNewRowEndOfGrid() {
    if ($ActiveID == "titleTbSalesProductTab1") {
        
        AddNewRowEndOfProductGrid();
    }
    else {
        AddNewRowEndOfMaterialGrid();
    }
}
function AddNewRowStartOfGrid() {
    
    if ($ActiveID == "titleTbSalesProductTab1") {
        AddNewRowStartOfProductGrid();
    }
    else {
        AddNewRowStartOfMaterialGrid();
    }
}
function AddNewRowEndOfProductGrid() {
    // Get the table by its ID
    var table = document.getElementById("tblProductGrid");

    // Function to insert a row with 18 cells
    function insertRow1() {
        var newRow = table.insertRow(table.rows.length);
        newRow.className = "GridNormalRow";
        // Loop to create 18 cells and add input fields
        for (var i = 0; i < 18; i++) {
            var cell = newRow.insertCell(i);

            // Add specific input type for the second cell (for button)
            if (i === 1) {
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnProductListBtnClick(this)" style="height:16px;float:right" />`;
            }
            else if (i === 9 || i === 10 || i === 11 || i === 13 || i === 14 || i === 15 || i === 16) {
                cell.className = "labelClass ";
                cell.innerHTML = `<input type="text" class="NumericFieldsClass InActiveIMEMode addTextBoxStyle tdInputBoxWidthRoll tdInputBoxBorderWidth" value="" />`;
            }
            else {
                // For other cells, add a text input field
                cell.className = "labelClass ";
                cell.innerHTML = ``;
            }
        }

    }

    function insertRow2() {
        var newRow = table.insertRow(table.rows.length);
        newRow.className = "GridNormalRow";
        for (var i = 0; i < 8; i++) {
            var cell = newRow.insertCell(i);
            if (i === 2) {
                cell.colSpan = 4
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }
            else if (i === 4) {

                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtOrderRemainingStock" class="labelClass tdInputBoxBorderWidth tdInputBoxWidthRoll" value="" />`;

            }
            else if (i === 5) {

                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtProductStock" class="labelClass tdInputBoxBorderWidth tdInputBoxWidthRoll" value="" />`;

            }
            else if (i === 6) {
                cell.colSpan = 8
                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtMsgMatl" class="labelClass  tdInputBoxWidthRoll" value="" />`;

            }
            // Add specific input type for the second cell (for button)
            else {
                // For other cells, add a text input field
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }





        }
    }
    // Add two rows by calling insertRow twice
    insertRow1();
    insertRow2();
}
function AddNewRowStartOfProductGrid() {

    // Get the table by its ID
    var table = document.getElementById("tblProductGrid");

    // Function to insert a row with 18 cells
    function insertRow1() {
        var newRow = table.insertRow(4);
        newRow.className = "GridNormalRow";
        // Loop to create 18 cells and add input fields
        for (var i = 0; i < 18; i++) {
            var cell = newRow.insertCell(i);

            // Add specific input type for the second cell (for button)
            if (i === 1) {
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnProductListBtnClick(this)" style="height:16px;height:16px;float: right;" />`;
            }
            else if (i === 9 || i === 10 || i === 11 || i === 13 || i === 14 || i === 15 || i === 16) {
                cell.className = "labelClass ";
                cell.innerHTML = `<input type="text" class="NumericFieldsClass InActiveIMEMode addTextBoxStyle tdInputBoxWidthRoll tdInputBoxBorderWidth" value="" />`;
            }
            else {
                // For other cells, add a text input field
                cell.className = "labelClass ";
                cell.innerHTML = ``;
            }
        }

    }

    function insertRow2() {
        var newRow = table.insertRow(5);
        newRow.className = "GridNormalRow";
        for (var i = 0; i < 8; i++) {
            var cell = newRow.insertCell(i);
            if (i === 2) {
                cell.colSpan = 4
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }
            else if (i === 4) {

                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtOrderRemainingStock" class="labelClass tdInputBoxBorderWidth " value="" />`;

            }
            else if (i === 5) {

                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtProductStock" class="labelClass tdInputBoxBorderWidth " value="" />`;

            }
            else if (i === 6) {
                cell.colSpan = 8
                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtMsgMatl" class="labelClass tdInputBoxBorderWidth " value="" />`;

            }
            // Add specific input type for the second cell (for button)
            else {
                // For other cells, add a text input field
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }





        }
    }
    // Add two rows by calling insertRow twice
    insertRow1();
    insertRow2();
}
function AddNewRowStartOfMaterialGrid() {
    // Get the table by its ID
    var table = document.getElementById("tblMaterialGrid");
    function insertRow11() {
        var newRow = table.insertRow(4);
        newRow.className = "GridNormalRow";
        // Loop to create 18 cells and add input fields
        for (var i = 0; i < 12; i++) {
            var cell = newRow.insertCell(i);

            // Add specific input type for the second cell (for button)
            if (i === 1) {
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnProductListBtnClick(this)" style="height:16px;height:16px;margin-left: 40px;" />`;
            }
            else {
                // For other cells, add a text input field
                cell.className = "labelClass ";
                cell.innerHTML = ``;
            }
        }

    }

    function insertRow22() {
   
       
        var newRow = table.insertRow(5);
        newRow.className = "GridNormalRow";
        for (var i = 0; i < 6; i++) {
            
            var cell = newRow.insertCell(i);
            if (i === 3 || i === 5) {
                cell.colSpan = 4
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }
            // Add specific input type for the second cell (for button)
            else {
                // For other cells, add a text input field
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }





        }
    }
    // Add two rows by calling insertRow twice
    insertRow11();
    insertRow22();

}

function AddNewRowEndOfMaterialGrid() {
    // Get the table by its ID
    var table = document.getElementById("tblMaterialGrid");
    function insertRow11() {
        var newRow = table.insertRow(table.rows.length);
        newRow.className = "GridNormalRow";
        // Loop to create 18 cells and add input fields
        for (var i = 0; i < 12; i++) {
            var cell = newRow.insertCell(i);

            // Add specific input type for the second cell (for button)
            if (i === 1) {
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnProductListBtnClick(this)" style="height:16px;float: right" />`;
            }
            else {
                // For other cells, add a text input field
                cell.className = "labelClass ";
                cell.innerHTML = ``;
            }
        }

    }

    function insertRow22() {
      

        var newRow = table.insertRow(table.rows.length);
        newRow.className = "GridNormalRow";
        for (var i = 0; i < 6; i++) {

            var cell = newRow.insertCell(i);
            if (i === 3 || i === 5) {
                cell.colSpan = 4
                cell.className = "labelClass";
                cell.innerHTML = ``;
            }
            // Add specific input type for the second cell (for button)
            else {
                // For other cells, add a text input field
                cell.className = "labelClass";
                cell.innerHTML = ``;
            }





        }
    }
    // Add two rows by calling insertRow twice
    insertRow11();
    insertRow22();

}
function DeleteRow() {
   
    if ($ActiveID == "titleTbSalesProductTab1") {
        deleteRowProductGrid();
    }
    else {
        deleteRowMaterialGrid();
    }

    
}
function deleteRowProductGrid() {
  
    var table = document.getElementById("tblProductGrid");
    var rowCount = table.rows.length;
    if (rowCount != 6) {
        table.deleteRow(rowCount - 1);  // Delete the last row
        table.deleteRow(rowCount - 2);
    }
}
function deleteRowMaterialGrid() {

    var table = document.getElementById("tblMaterialGrid");
    var rowCount = table.rows.length;
    if (rowCount != 6) {
        table.deleteRow(rowCount - 1);  // Delete the last row
        table.deleteRow(rowCount - 2);
    }
}
function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";


    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown) {
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
        $("#dialogMessageIcon img").hide();
        //$("#imgError").show();
        //$("#imgWarning").show();

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

$("#btnRegister").click(function (event) {
    var $callFrom = $(this);
    event.preventDefault();
    
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    else {
        saveProductMaterialData();
       
    }

})
function saveProductMaterialData() {
    debugger;
    const val = $("#txtRequestedDate").val();
    const ReqDate = new Date(val);
    var currentTime = new Date();
    var ajaxTime = new Date().getDate();
    const productRows = document.querySelectorAll('.ProductGridRow'); // Select all rows with the class GridNormalRow
    const PRODUCT_LIST = [];

    for (let i = 0; i < productRows.length; i += 2) {
        const row1 = productRows[i];
        const row2 = productRows[i + 1];
        var Seconds = parseInt(i, 10).toString().padStart(2, '0');
        var DateVal = ReqDate.getFullYear() + '/' + String(ReqDate.getMonth() + 1).padStart(2, '0') + '/' + String(ReqDate.getDate()).padStart(2, '0') + ' ' + String(currentTime.getHours()).padStart(2, '0') + ':' + String(currentTime.getMinutes()).padStart(2, '0') + ':' + Seconds;
        // Create an object to store the data for the current row
        const productRowData = {};
        productRowData.ROLL_FC_CD = row1.dataset.rollfccd ? Number(row1.dataset.rollfccd) : null;
        productRowData.ROLL_FC_NO = row1.dataset.rollfcno ? Number(row1.dataset.rollfcno) : null;
        productRowData.FC_SRLNO = (i / 2) + 1;
        productRowData.PRODUCT_CD = row1.getAttribute('data-lblProductCD');

        // Extract text content from <td> elements in the first part of the row
        productRowData.PROCDUCT_KUBUN_STRING = row1.cells[0]?.innerText.trim() || '';
        productRowData.CUSTOMER_NAME = row1.cells[1]?.querySelector('label')?.innerText.trim() || '';
        productRowData.PROD_DIM_D = parseFloat(row1.cells[2]?.querySelector('input')?.value.trim()) || null; //row1.cells[2]?.innerText.trim() || '';
        productRowData.SHAPE = row1.cells[3]?.innerText.trim() || '';
        productRowData.PROD_DIM_W = parseFloat(row1.cells[4]?.querySelector('input')?.value.trim()) || null;
        productRowData.PROD_DIM_OTHER = row1.cells[5]?.innerText.trim() || '';  
        productRowData.PROD_LENGTH = row1.cells[6]?.innerText.trim() || '';
        productRowData.MAGARI_BEND_TOLLERENCE = row1.cells[7]?.innerText.trim() || '';
        productRowData.CONDITION_DESCRIPTION = row1.cells[8]?.innerText.trim() || '';


        // Extract values from <input> fields first part of the row
        productRowData.DISPATCH_QTY1 = parseFloat(row1.cells[9]?.querySelector('input')?.value.trim()) || null;
        productRowData.DISPATCH_QTY2 = parseFloat(row1.cells[10]?.querySelector('input')?.value.trim()) || null;
        productRowData.DISPATCH_QTY3 = parseFloat(row1.cells[11]?.querySelector('input')?.value.trim()) || null;
        productRowData.AVG_DISPATCH_QTY = row1.cells[12]?.querySelector('input')?.value.trim() || '';
        productRowData.FC_QTY_R1 = parseFloat(row1.cells[13]?.querySelector('input')?.value.trim()) || null;
        productRowData.FC_QTY_R2 = parseFloat(row1.cells[14]?.querySelector('input')?.value.trim()) || null;
        productRowData.FC_QTY_R3 = parseFloat(row1.cells[15]?.querySelector('input')?.value.trim()) || null;
        productRowData.FC_QTY_R4 = parseFloat(row1.cells[16]?.querySelector('input')?.value.trim()) || null;
        productRowData.PROD_SPEC_NO = row1.cells[17]?.innerText.trim() || '';

        // Extract second row values (MATL_MAKER_NAME, INTERNAL_NAME_STRING, etc.)
        productRowData.MATL_MAKER_NAME = row2.cells[0]?.innerText.trim() || '';
        productRowData.INTERNAL_NAME_STRING = row2.cells[1]?.innerText.trim() || '';
        productRowData.DIM_TOL = row2.cells[2]?.innerText.trim() || '';
        productRowData.LEN_TOL = row2.cells[3]?.innerText.trim() || '';
        productRowData.WIP_STOCK = row2.cells[4]?.querySelector('input')?.value.trim() || null;
        productRowData.PROD_STOCK = row2.cells[5]?.querySelector('input')?.value.trim() || null;
        productRowData.REMARKS = row2.cells[6]?.querySelector('input')?.value.trim() || null;
        productRowData.MAKER_NAME = row2.cells[7]?.innerText.trim() || '';

      
            PRODUCT_LIST.push(productRowData);
        }

    
// Store the row data in the array
   
    const materialRows = document.querySelectorAll('.MaterialGridRow');
    const MATERIAL_LIST = [];

    for (let i = 0; i < materialRows.length; i += 2) {
        const row1 = materialRows[i];     
        const row2 = materialRows[i + 1]; 

        const materialRowData = {};

        // Row 1 data
        materialRowData.FC_SRLNO = (i / 2) + 1;
        materialRowData.MATERIAL_CD = row1.getAttribute('data-lblMaterialCD');
        materialRowData.ROLL_FC_CD = row1.dataset.rollfccd ? Number(row1.dataset.rollfccd) : null;
        materialRowData.MATERIAL_TYPE = row1.cells[2]?.innerText.trim() || '';
        materialRowData.DIMENSION_L = row1.cells[3]?.innerText.trim() || '';
        materialRowData.MATERIAL_SHAPE = row1.cells[4]?.innerText.trim() || '';
        materialRowData.DIMENSION_W = row1.cells[5]?.innerText.trim() || '';
       
        materialRowData.DIMENSION_OTHER = row1.cells[6]?.innerText.trim() || '';
        materialRowData.MATERIAL_LENGTH = row1.cells[7]?.innerText.trim() || '0';

        materialRowData.FC_QTY_R1 = parseFloat(row1.cells[8]?.querySelector('input')?.value.trim()) || null;
        materialRowData.FC_QTY_R2 = parseFloat(row1.cells[9]?.querySelector('input')?.value.trim()) || null;
        materialRowData.FC_QTY_R3 = parseFloat(row1.cells[10]?.querySelector('input')?.value.trim()) || null;
        materialRowData.FC_QTY_R4 = parseFloat(row1.cells[11]?.querySelector('input')?.value.trim()) || null;

        // Row 2 data
        materialRowData.MATL_MAKER_NAME = row2.cells[0]?.innerText.trim() || '';
        materialRowData.MAKER_NAME = row2.cells[1]?.innerText.trim() || '';
        materialRowData.MATL_SPECIAL_KUBUN = row2.cells[2]?.innerText.trim() || '';
        materialRowData.CONDITION_DESCRIPTION = row2.cells[3]?.innerText.trim() || '';
        materialRowData.MOCHIKOMISAKI_NAME = row2.cells[4]?.innerText.trim() || '';
        materialRowData.REMARKS = row2.cells[5]?.querySelector('input')?.value.trim() || '';

        
        MATERIAL_LIST.push(materialRowData);
    }
   
   
    // ✅ Common Header Data
    const headerData = {};

    headerData.EMPLOYEE_CD = $("#ddlInchargeName").val() || null;
    headerData.CUSTOMER_CD = $("#ddlCustomerName").val() || null;
    headerData.ROLL_FC_R = $("#lblForecastR").text() || null;
    headerData.ROLL_FC_YEAR = $("#hdcurrent_yyyy").text() || null;
    headerData.ROLL_FC_DATE_STRING = DateVal;
    headerData.ROLL_FC_LOCK_DATE_STRING = DateVal;
    headerData.USER_ID = $("#hdfLoginId").val();
    const checkbox = document.getElementById('chkForecastDone');
    headerData.SALES_FC_DONE_DATE_STRING = checkbox.checked ? DateVal : null;

    const combinedData = {
        HEADER: headerData,
        MATERIAL_LIST: MATERIAL_LIST,
        PRODUCT_LIST: PRODUCT_LIST
    };

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoForecast/SaveProductMaterialForecastDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(combinedData) 
    };
    $.ajax(settings)
        .done(function (response) {
           debugger
            responseTime = new Date().getTime() - ajaxTime;
            var successMessage = $("#lblforecastSaveSuccessfully_SalesForecast_03").text();
            showMessageDialog(successMessage, "", SUCCESS);
            //disableSaveButtons();
            var result = response.message;
            //Assign 
            var jsonResultProduct = result.producT_LIST;
            for (let i = 0; i < jsonResultProduct.length; i++) {

                const row = productRows[i * 2];

                row.dataset.rollfccd = jsonResultProduct[i].rolL_FC_CD;
              
            }
            var jsonResultMaterial = result.materiaL_LIST;
            for (let i = 0; i < jsonResultMaterial.length; i++) {

                const row = materialRows[i * 2];

                row.dataset.rollfccd = jsonResultMaterial[i].rolL_FC_CD;
               
            }
        })

        .fail(function (xhr, textStatus, errorThrown) {
            debugger;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_SalesForecast_01").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

    
  
}

function OnClickProductList($callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    ProductCD = $.trim($callFrom.closest("tr").attr("data-lblProductCD"));
    GetProductDetailsByCD(ProductCD);
} 

$("#btnSpecInput1").click(function (event) {
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    else {
        var ajaxTime = new Date().getDate();
        var cuct_Cd = $("#ddlCustomerName").val() ? $("#ddlCustomerName").val() : null; 
        var incharge_Cd = $("#ddlInchargeName").val() ? $("#ddlInchargeName").val() : null;
        var current_R = $("#lblForecastR").text() ? $("#lblForecastR").text() : null;
        var current_YYYY = $("#hdcurrent_yyyy").text();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/RollPoForecast/GetSpecInput1Data",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CUSTOMER_CD: cuct_Cd, EMPLOYEE_CD: incharge_Cd, CURRENT_R: current_R, CURRENT_YYYY: current_YYYY
            })

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                var result = response.message;
                enableButtons();
                disableSearchCriteria();
                if(result.producT_LIST.length > 0)
                {
                    ProductList = response.message;
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/RollPoForecast/ProductListGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(result.producT_LIST)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response) {
                            debugger;
                            var result = response.message;
                          

                            $("#divRollSalesForecastProductGrid").html(response.message);
                            AddNewRowEndOfProductGrid();
                            CalculateAllGridMonthRoll();
                            enableButtons();
                        })

                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_SalesForecast_01").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
                if (result.materiaL_LIST.length > 0) {
                   
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/RollPoForecast/MaterailListGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(result.materiaL_LIST)
                        //data: "{jsonData : " + jsonData + "}"
                    }
                    $.ajax(settings1)
                        .done(function (response) {
                           
                            var result = response.message;
                            

                            $("#divRollSalesForecastMaterialGrid").html(response.message);
                            AddNewRowEndOfMaterialGrid();
                            CalculateAllGridMonthRoll();
                        })

                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_SalesForecast_01").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
               
            })
            .fail(function (xhr, textStatus, errorThrown) {

                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_SalesForecast_01").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);

            });
    }
});

$("#btnSpecInput2").click(function (event) {
   
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    else {
        var ajaxTime = new Date().getDate();
        var cuct_Cd = $("#ddlInchargeName").val() ? $("#ddlInchargeName").val() : null;
        var incharge_Cd = $("#ddlCustomerName").val() ? $("#ddlCustomerName").val() : null;
        var current_R = $("#lblForecastR").text() ? $("#lblForecastR").text() : null;
        var current_YYYY = $("#hdcurrent_yyyy").text();
        var historyMonth3 = $("#lblHistoryMonth3").text() ? $("#lblHistoryMonth3").text() : null;
        var historyMonth2 = $("#lblHistoryMonth2").text() ? $("#lblHistoryMonth2").text() : null;
        var historyMonth1 = $("#lblHistoryMonth1").text() ? $("#lblHistoryMonth1").text() : null;
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/RollPoForecast/GetSpecInput2Data",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CUSTOMER_CD: cuct_Cd, EMPLOYEE_CD: incharge_Cd, CURRENT_R: current_R, CURRENT_YYYY: current_YYYY, historyMonth1, historyMonth2, historyMonth3
            })

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
               
                var result = response.message;
                ProductList = response.message;
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/ProductListGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                      
                        var result = response.message;
                       

                        $("#divRollSalesForecastProductGrid").html(response.message);
                        AddNewRowEndOfProductGrid();
                        CalculateAllGridMonthRoll();
                        enableButtons();
                    })

                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_SalesForecast_01").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });


            })
            .fail(function (xhr, textStatus, errorThrown) {

                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_SalesForecast_01").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);

            });
    }
});

 $("#btnStartDateTime").click(function (event) {
        setStartSysDateTime();
    });
function OnProductListBtnClick() {
    debugger;
    $("#ProductListPopupModal").modal('show');
    $("#ProductListPopupModal").addClass("show");
    var elements = document.getElementsByClassName('');
    for (var i in elements) {
        if (elements.hasOwnProperty(i)) {
            elements[i].className = 'modal-backdrop fade show';
        }
    }
    InitSortPopupOnProduct();
    document.getElementById("ProductListPopupModal").style.display = "block";
}
function OnMaterialListBtnClick(editButton) {
    $("#MaterialListPopupModal").modal('show');
    $("#MaterialListPopupModal").addClass("show");
    var elements = document.getElementsByClassName('');
    for (var i in elements) {
        if (elements.hasOwnProperty(i)) {
            elements[i].className = 'modal-backdrop fade show';
        }
    }
    InitSortPopupOnMaterial();
    document.getElementById("MaterialListPopupModal").style.display = "block";
}




