
var emptyRow = 20;

$(document).ready(function () {
    debugger;
    $.support.cors = true;
    setVehicleName();
    setForwadingDestination();
    setStartSysDateTime();
    disableGridCheckBox();
    if ($("#hdfLoginId").val() != "") {
        setSourceLocation($("#hdfLoginId").val());
    }
    const ua = navigator.userAgent;
    // Detect Tablet (iPad, Android Tablet, etc.)
    const isTablet = /(iPad|Tablet|Nexus 7|Nexus 10|KFAPWI|SM-T|Lenovo Tab|Pixel C)/i.test(ua);

    // Detect Mobile (Phones)
    const isMobile = /(Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini)/i.test(ua);

            if (isTablet) {
                // Tablet device
                window.resizeTo(768, 1024);   // typical tablet size (only works in popup windows)
                document.body.style.zoom = "100%";
            } else if (isMobile) {
                // Mobile device
                alert("Mobile");
                window.resizeTo(360, 780);    // typical mobile size
                document.body.style.zoom = "50%";
            } else {
                // Desktop
                window.resizeTo(1450, 670);
            }
});


$(document).on('click', '[id^="btnTensouGridArrow"]', function (event) {
    $("#ProductPopupModal").modal('show');
    InitSortPopupOnProduct();
});

function OnClickProductList($callFrom) {
    debugger;
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var productCD = $.trim($callFrom.closest("tr").attr("data-lblProductCD"));
    var culture = $("#hdfCultureName").val();
   // var jsonData = "PRODUCT_CD :'" + productCD + "' }";
   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/GetProductDetailsByProductCD?PRODUCT_CD=" + productCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = null;
            jsonResult = response.message;
            setProductTensouDetails(jsonResult);
            $("#ProductPopupModal").modal("hide");
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
};

function setProductTensouDetails(jsonResult) {
    debugger;
    for (var k = 0; k < jsonResult.length; k++) {
        var row = k + 1;
        $("#lblCustomer" + row).text(jsonResult[k].customeR_NAME);
        $("#lblProdNameCust" + row).text(jsonResult[k].producT_NAME_CUST);
        $("#lblCondition" + row).text(jsonResult[k].conditioN_DESCRIPTION);
        $("#lblDimD" + row).text(jsonResult[k].proD_DIM_D);
        $("#lblShape" + row).text(jsonResult[k].proD_SHAPE);
        $("#lblWidthAndOther" + row).text(jsonResult[k].proD_DIM_OTHER_STRING);
        $("#lblLength" + row).text(jsonResult[k].proD_LENGTH);
        $("#txtCharge" + row).val(jsonResult[k].chargE).css("background-color", "#C1FFFF").removeAttr("disabled");

        $("#txtTabaNo" + row).val(jsonResult[k].tabA_NO_STRING).css("background-color", "#C1FFFF").removeAttr("disabled");
       
        $("#txtNoOfUnits" + row)
            .val(jsonResult[k].uniT_TYPE_STRING)
            .css("background-color", "#C1FFFF");
        $("#txtNoOfUnits" + row).removeAttr("disabled");
        $("#txtWeightWithoutKG" + row).val(jsonResult[k].uniT).css("background-color", "#C1FFFF").removeAttr("disabled");
        $("#txtAmount" + row).val(jsonResult[k].acT_ALLOC_QTY).css("background-color", "#C1FFFF").removeAttr("disabled");
        $("#txtRate" + row).val(jsonResult[k].ratE).css("background-color", "#C1FFFF").removeAttr("disabled");
        $("#txtShukkaLocation" + row).val(jsonResult[k].inpuT_LOCATION_CD).css("background-color", "#C1FFFF").removeAttr("disabled");

       
    }
}

function setVehicleName() {
    debugger;
    var ajaxTime = new Date().getTime();
     //var jsonData = "{USER_ID: '" + USER_ID + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/GetVehicleName", 
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;

            bindVehicledropdown(response.message);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindVehicledropdown(jsonResult) {
    
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlVehicleName").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setForwadingDestination() {

    var ajaxTime = new Date().getTime();
    // var jsonData = "{USER_ID: '" + USER_ID + "', USER_NAME: '" + userName + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/GetForwordingLocation",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;

            bindlocationdropdown(response.message);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindlocationdropdown(jsonResult) {

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTensoDestination").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>')); 
        $("#ddlTransitLoc").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>')); 
    });
}

function setSourceLocation(USER_ID, $callFrom) {

    debugger;
    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_ID: '" + USER_ID + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/GetLocation?USER_ID=" + USER_ID,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;
            var jsonResult = jResult[0];
            $('#txtSource1').val(jsonResult.name);

            // bindlocationdropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setStartSysDateTime() {
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message.sysDate;
            var nextDayDateTime = response.message.nextDayDate;
           
            $("#txtPlanDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtTransferDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnDeleteRow").click(function () {
    isCheckPost = false;
    for (var i = 1; i <= emptyRow; i++) {
        if ($("#chkProdTensouGrid" + i).is(':checked')) {
            $("#chkProdTensouGrid" + i).prop('checked', false)
            clearTableRow("tr" + i);
            $("#chkProdTensouGrid" + i).attr("disabled", false);
            isCheckPost = true
        }
    }
    if ($("#chkProdTensouGrid" + i).prop('checked', false) && isCheckPost == false) {
        var message = $("#lblErrorMessageE_ProductTensou_022").text().trim();
        showMessageDialog(message, "", WARNING);
    }
});

function clearTableRow(rowid) {

    $.each($('#' + rowid).find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });
    $.each($('#' + rowid).find("select"), function (index, dropdown) {
        $("#" + dropdown.id + " option:selected").prop("selected", false);
        $("#" + dropdown.id + " option:first").prop("selected", "selected");
    });
    $.each($('#' + rowid).find("input:checkbox"), function (index, checkBox) {
        $(checkBox).prop('checked', false);
    });

    $.each($('#' + rowid).find("td[id^='lbl']"), function (index, label) {
        $(label).text("");
    });

    $.each($('#' + rowid).find("input:hidden"), function (index, label) {
        $(label).text("");
    });

}

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
});

$(document).on('click', '#btnSortOnProduct', function () {
   
    $("#SortPopUpModal").modal('show');
});

$("#btnSelect").click(function () {

    window.open(
        "/ProductTensou/ProductTensouListPage",
        "ListPage",
        "height=650,width=1100,resizable=no"
    );

});

//Initialize
$("#btnInitialize").click(function (event) {
    clear();
    setStartSysDateTime();
});
function clear() {

    $("#divTensouGridMain").find("input[type='text']").val("");
    $("#divTensouGridMain").find("label").text("");

    $('table#tblProductTensou tbody tr').each(function (i) {
        if (i < emptyRow) {
            var row = i + 1;

            $("#lblChuuban" + row).text("");
            $("#lblCustomer" + row).text("");
            $("#lblProdNameCust" + row).text("");
            $("#lblCondition" + row).text("");
            $("#lblShape" + row).text("");
            $("#lblDimD" + row).text("");
            $("#lblLength" + row).text("");

            $("#txtNoOfUnits" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            $("#txtWeightWithoutKG" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            $("#txtRate" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            $("#txtAmount" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            $("#txtCharge" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            $("#txtTabaNo" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            $("#txtShukkaLocation" + row).val("").css("background-color", "#E2E2E2").prop("disabled", true);
            
            $("#chkProdTensouGrid" + row).prop("disabled", false);
            $("#chkProdTensouGrid" + row).prop("checked", false);
        }
    });

    disableEntryDeleteButtons();

}

function disableEntryDeleteButtons() {
    $("#btnSave").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
    $("#btnUpdate").attr("disabled", "disabled");
}

//shukka scan then Auto focus on next textBox.
$(document).on('keypress', '[id^="txtShukkaCode"]', function (e) {
    var lstShukka = [];
    for (var i = 1; i <= emptyRow; i++) {
        var $tr = $(this);
        var shukka = $("#txtShukkaCode" + i).val();
        if (shukka != "") {
            lstShukka.push(shukka);
        }
    }

    for (var i = 1; i <= lstShukka.length; i++) {
        var row = i + 1;
        $("#txtShukkaCode" + row).focus();
    }

});

//disable checkBOX on page load.
function disableGridCheckBox() {
    for (var i = 1; i <= emptyRow; i++) {
        $("#chkTensouAccepGrid" + i).attr("disabled", true);
    }
}

//If textbox-[txtTransferCD] has a value → it enables the checkbox[chkTensouAccepGrid].
//If textbox[txtTransferCD] is empty → it disables that checkbox.
//document.querySelector("#tblProductTensou tbody").addEventListener("change", function (event) {
//    debugger;
//    const row = event.target.closest("tr");
//    var $val = row.id;
//    const input = $val;
//    const [, letters, number] = input.match(/^([a-zA-Z]+)(\d+)$/) || [];
//    var txtval = ('txtShukkaCode' + number)
//    const elementTransferNo = document.getElementById(txtval);
//    if (elementTransferNo) {
//        elementTransferNo.addEventListener("change", onTransferNoInputChange(number));
//    }

//});

//Function to enable/disable checkbox
function onTransferNoInputChange(rowID) {
    
    var shukkaNo = $("#txtSukkaCode" + rowID).val();
    if (shukkaNo !== "") {
        $("#chkTensouAccepGrid" + rowID).removeAttr("disabled");
    }
    else {
        $("#chkTensouAccepGrid" + rowID).attr("disabled", true);
    }
}


function InitSortPopupOnProduct() {
    debugger;
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/ProductTensou/InitializeSortPopupForProductList",
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

function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
function btnSortClose_Click() {

    closepopup();
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


$("#btnSearchProduct").click(function () {
    //if (!IsMandatoryDateSearchFieldsFilledOnProduct()) {
    //    return false;
    //}
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
        url: APIURL + "/ProductTensou/GetProductMasterData",
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
                url: "/ProductTensou/ProductMasterListGrid",
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

//save
$(document).on('click', '#btnSave', function (event) {
   
    debugger;
    //Transfer date required.
    if ($("#txtPlanDate").val() == "") {
        var message = $("#lblErrorMessageE_WIPTensou_003").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtPlanDate", WARNING);
        $("#txtPlanDate").focus;
        return false;
    }

    //Transfer date Invalid.
    if (!checkDate($("#txtPlanDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_005").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtPlanDate", WARNING);
        $("#txtPlanDate").focus;
        return false;
    }
    //Acceptance date required.
    if ($("#txtTransferDate").val() == "") {
        var message = $("#lblErrorMessageE_WIPTensou_021").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtTransferDate", WARNING);
        $("#txtTransferDate").focus;
        return false;
    }
    //Acceptance date Invalid.
    if (!checkDate($("#txtTransferDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_022").text().trim(); // This message will be displayed on dialog box
        showMessageDialog(message, "txtTransferDate", WARNING);
        $("#txtTransferDate").focus;
        return false;
    }


    var transferDate = document.getElementById("txtTransferDate").value;
    var planDate = document.getElementById("txtPlanDate").value;

    var inspStartDate = formatStartEndDate(transferDate);
    var inspEndDate = formatStartEndDate(planDate);

    if (inspEndDate < inspStartDate) {
        var message = $("#lblErrorMessageE_WIPTensou_023").text().trim();
        focusFieldID = $(':focus').attr("id");
        showMessageDialog(message, "txtTransferDate", WARNING);
        return false;
    }
    
    //Date Range
    //if (!IsDateDifferentFromRange(true)) {
    //    var message = $("#lblErrorMessageE_WIPTensou_013").text().trim() + "(" + $("#lblErrorMessageE_WIPTensou_014").text().trim() + commonDateRange + ")";
    //    var focusFieldID = "txtDate";
    //    showMessageDialog(message, focusFieldID, WARNING);
    //    return false;
    //}

    //if (!IsDropDownMandatory()) {
    //    return false;
    //}

    //if (!IsMandatoryFieldsFilledOnGrid()) {
    //    return false;
    //}
    SaveWIPTensouDetails();
    if ($("#hdfDBOperation").val() == "Insert") {
        SaveWIPTensouDetails($callFrom);
    }
    else {
      //  updateWIPTensouDetails();
    }
});


function SaveWIPTensouDetails() {
    debugger;
    var ajaxTime = new Date().getTime();
    var jsonDataForSave = [];
    var gridData = [];
    
    for (var i = 1; i <= emptyRow; i++) {
       
        if ($("#txtWeightWithoutKG" + i).val() !== "") {

            var $tr = $(this);
            let taba = $("#txtTabaNo" + i).val();
            let splitTaba = taba.split(',');

            var ajaxTime = new Date().getDate();
            jsonDataForSave = ({

                //PROCESS_PLAN_CD: $("#hdProcessPlanCD" + i).val() ? $("#hdProcessPlanCD" + i).val() : null,
               // WIP_TRANSFER_CD: $("#txtTransferCD" + i).val() ? $("#txtTransferCD" + i).val() : null,
                LOCATION_CD_TO: $("#ddlTensoDestination").val() || null,
                START_VEHICLEL_CD: $("#ddlVehicleName").val() || null,
                TRANSIT_LOCATION_CD: $("#ddlTransitLoc").val() || null,
                TRANSFER_DATE_STRING: $("#txtTransferDate").val(),
                TRANSIT_PLANNED_DATE: $("#txtPlanDate").val(),
                TABA_NO_1: splitTaba[0] ? splitTaba[0] : null,
                TABA_NO_2: splitTaba[1] ? splitTaba[1] : null,
                TABA_NO_3: splitTaba[2] ? splitTaba[2] : null,
                TABA_NO_4: splitTaba[3] ? splitTaba[3] : null,
                UNIT_TYPE: $("#hdfUnitType" + i).val() ? $("#hdfUnitType" + i).val() : null,
                NO_OF_UNITS: $("#hdfNoOfUnits" + i).val() ? $("#hdfNoOfUnits" + i).val() : null,
                QTY: $("#txtWeightWithoutKG" + i).val() || 0,
                RATE: $("#txtRate" + i).val() || 0,
                CHARGE: $("#txtCharge" + i).val() || 0,
                SHUKKA_LOCATION: $("#txtShukkaLocation" + i).val() || 0,
               // NEXT_PROSESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
               // MSG_CONTENTS: $("#txtSource" + i).val() ? $("#txtSource" + i).val() : null,
               // MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
                USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null
            });
            // Add only this row to the array
            gridData.push(jsonDataForSave);
        }
    }
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/SaveProductTensouDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    };

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var jsonResult = response.message;

            $.each(jsonResult, function (index, jsonObj) {
                $("#hdfTransfer_CD" + index).val(jsonObj.wiP_TRANSFER_CD);
                $("#hdWIPTensouCD").val(jsonObj.wiP_TRANSFER_GROUP_NO);
            });

            var successMessage = $("#lblSuccessMessageS_WIPTensou_001").text();
            showMessageDialog(successMessage, "txtEndOperator", SUCCESS);

            $("#btnSave").prop("disabled", true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            var errorCode = "";

            try {
                errorCode = xhr.responseJSON
                    ? xhr.responseJSON.Message
                    : JSON.parse(xhr.responseText).Message;
            } catch (e) {
                errorCode = errorThrown || textStatus;
            }

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();

            showMessageDialog(message, focusFieldID, ERROR);
        });
}
//List Page
$(document).on('click', '#btnSearch', function (event) {
    debugger;
    //Required Field
    if ($('#txtDimDEndSearch').val()) {
        if (!$('#txtDimDStartSearch').val()) {
            var message = $("#lblErrorMessageE_WIPTensou_020").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtDimDStartSearch", WARNING);
            $("#txtDimDStartSearch").focus;
            return false;
        }
    }

    if ($('#txtTransferEndDateSearch').val()) {
        if (!$('#txtTransferStartDateSearch').val()) {
            var message = $("#lblErrorMessageE_WIPTensou_016").text().trim(); // This message will be displayed on dialog box
            showMessageDialog(message, "txtTransferStartDateSearch", WARNING);
            $("#txtTransferStartDateSearch").focus;
            return false;
        }
    }

    //Invalid Date
    var date = ($('#txtTransferStartDateSearch').val());
    if (date != "") {
        if (!checkDateOnListPage($("#txtTransferStartDateSearch"))) {
            var message = $("#lblErrorMessageE_WIPTensou_015").text(); // Transfer Start date
            showMessageDialog(message, "", WARNING);
            $("#txtTransferStartDateSearch").focus;
            return false;
        }
    }

    var endDate = ($('#txtTransferEndDateSearch').val());
    if (endDate != "") {
        if (!checkDateOnListPage($("#txtTransferEndDateSearch"))) {
            var message = $("#lblErrorMessageE_WIPTensou_015").text().trim(); // Transfer End date
            showMessageDialog(message, "", WARNING);
            $("#txtTransferEndDateSearch").focus;
            return false;
        }
    }


    var CULTURE = $("#hdfCultureName").val();
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerSearch").val();
    var PROD_NAME_CUST = $("#txtProdNameSearch").val();
    var INTERNAL_NAME_STRING = $("#txtInternalNameSearch").val();
    var PP_DIM_TOL_MIN = $("#txtDimDStartSearch").val();
    var PP_DIM_TOL_MAX = $("#txtDimDEndSearch").val();
    var PROD_SHAPE = $("#txtShapeSearch").val();
    var CHARGE = $("#txtChargeSearch").val();
    var TRANSFER_DATE_START = $("#txtTransferStartDateSearch").val();
    var TRANSFER_DATE_END = $("#txtTransferEndDateSearch").val();
    var USER_ID = $("#hdfLoginId").val();

    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PP_DIM_TOL_MIN.length > 0 || PP_DIM_TOL_MAX.length > 0 || PROD_SHAPE.length > 0 || CHARGE.length > 0 || TRANSFER_DATE_START.length > 0 || TRANSFER_DATE_END.length > 0) {
        var ajaxTime = new Date().getTime();
        
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/ProductTensou/TensouDataSearchInGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PP_DIM_TOL_MIN: PP_DIM_TOL_MIN, PP_DIM_TOL_MAX: PP_DIM_TOL_MAX, PROD_SHAPE: PROD_SHAPE, CHARGE: CHARGE, TRANSFER_DATE_START: TRANSFER_DATE_START, TRANSFER_DATE_END: TRANSFER_DATE_END,
                USER_ID: USER_ID, CULTURE_NAME: CULTURE,
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    debugger;
                    var settings1 = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/ProductTensou/TensouDataSearchInGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.message)
                        
                    }
                    $.ajax(settings1)
                        .done(function (response1) {
                            debugger;
                            jsonResult = response1.message;
                            if (response1.isSuccess) {
                                $("#divTensouGridMain").html(response1.message);
                            }
                           
                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;

                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
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
                message = $.trim($("#lblErrorMessageE_WIPTensou").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });

    } else {

        var Message = $("#lblErrorMessageE_WIPTensou_017").text();
        showMessageDialog(Message, "", WARNING);
        
    }

});
function checkDateOnListPage(field) {

    var minYear = 2019;
    var maxYear = 2100;

    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/      //CHECK ONLY DATE

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
//checkDate
function checkDate(field) {
    var minYear = 2019;
    var maxYear = 2100;

    var re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12]\d|3[01])$/;

    var value = field.val();

    if (value == '') {
        return true; // Empty date allowed
    }

    var regs = value.match(re);

    if (!regs) {
        return false;
    }

    var year = parseInt(regs[1], 10);
    var month = parseInt(regs[2], 10);
    var day = parseInt(regs[3], 10);

    if (year < minYear || year > maxYear) {
        return false;
    }

    var totalDays = new Date(year, month, 0).getDate();
    if (day < 1 || day > totalDays) {
        return false;
    }
    return true;
}

function formatStartEndDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('/');
}

//Delete
$("#btnDelete").click(function (event) {
    deleteWIPTensouDetails();
});

function deleteWIPTensouDetails() {
    debugger;
    var ajaxTime = new Date().getDate();

    var gridData = [];

    for (var i = 1; i <= emptyRow; i++) {

        let taba = $("#txtTabaNo" + i).text();
        let splitTaba = taba.split(',');
       // var transferCD = $("#txtTransferCD" + i).val();
        // var transferCD = $("#hdfTransferNo" + i).text();
        if (transferCD !== "") {
            var ajaxTime = new Date().getDate();
            var $tr = $(this);
            var data = {
                WIP_TENSOU_ROW: i,
                //PROCESS_PLAN_CD: $("#hdProcessPlanCD" + i).val() ? $("#hdProcessPlanCD" + i).val() : null,// $("#hdProcessPlanCD").val() ? $("#hdProcessPlanCD").val() : null,
                WIP_TRANSFER_CD: $("#txtTransferCD" + i).val() ? $("#txtTransferCD" + i).val() : null,
                TABA_NO_1: splitTaba[0] || 0,
                TABA_NO_2: splitTaba[1] || 0,
                TABA_NO_3: splitTaba[2] || 0,
                TABA_NO_4: splitTaba[3] || 0,
               // MSG_CONTENTS: $("#txtSource" + i).val() ? $("#txtSource" + i).val() : null,
               //MSG_CD: $("#hfld_MessageCD" + i).val() ? $("#hfld_MessageCD" + i).val() : 0,
            };
            // i++;
            gridData.push(data);
        }

    }
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: APIURL + "/productTensou/ProductTensouDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            var successMessage = $("#lblSuccessMessageS_ProductTensou_DataDeleted").text();
            showMessageDialog(successMessage, "", SUCCESS);
            //disableEntryDeleteButtons();
           // clear();
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProductTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnClose").click(function (event) {
    self.close();
});
function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

//Scan Shukka Code
$(document).on('click', '#btnShukkaNoSearch', function (event) {

    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();
    checkValidShukkaCode();
    
    $("#btnSave").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
    $("#btnUpdate").removeAttr("disabled");
   
});

function checkValidShukkaCode() {
    debugger;
    var i = 1;
    var gridData = [];
    var tensou = [];

    $('table#ProductTensou tbody tr').each(function (index, item) {
        var row = index + 1;
        if ($("#txtShukkaCode" + row).val() != "") {
            var $tr = $(this);
            var data = {
                PRODUCT_TENSOU_ROW: index,
                SHUKKA_INSTRUCTION_CD: $tr.find("#txtShukkaCode" + row).val() ? $tr.find("#txtShukkaCode" + row).val() : 0,

            };
            row++;
            gridData.push(data);
        }
    });

    if (gridData.length == 0) {
        var message = $("#lblErrorMessageE_WIPTensou_009").text().trim();
        showMessageDialog(message, "", WARNING);
        $("#txtTransferCD").focus;
        return false;

    }

    var success = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProductTensou/GetValidShukkaCD",        
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify(gridData)

    }

    $.ajax(settings)
        .done(function (response) {

            var jsonResult = response.message;
            var shukkaNotFound = jsonResult.notFound;
            if (jsonResult.found.length) {
                GetShukkaProductDetails(jsonResult.found);
            } else {
                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_ProductTensou_008").text().trim();
                MessageData = message + "<br>" + shukkaNotFound;
                showMessageDialog(MessageData, focusFieldID, WARNING);

            }


        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ProductTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })


}

function GetShukkaProductDetails(ShukkaCD) {
    debugger;

    var i = 1;
    var gridData = [];
    var tensou = [];

    var success = false;
    var ajaxTime = new Date().getTime();
    var jsonData = "{SHUKKA_INSTRUCTION_CD: '" + ShukkaCD + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
       // url: APIURL + "/ProductTensou/GetShukkaProductDetails?SHUKKA_INSTRUCTION_CD=" + ShukkaCD + "&CultureName=" + $("#hdfCultureName").val(),       //?transferCD=" + parseInt(transferCD),
        url: APIURL + "/ProductTensou/GetShukkaProductDetails?SHUKKA_INSTRUCTION_CD=" + encodeURIComponent(ShukkaCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }

    $.ajax(settings)
        .done(function (response) {
            debugger;
            var jsonResult = response.message;

            setProductShukkaDetails(jsonResult);

        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);

        });
}

function setProductShukkaDetails(jsonResult) {
    debugger;
    for (var k = 0; k < jsonResult.length; k++) {
        var row = k + 1;
        $("#lblCustomer" + row).text(jsonResult[k].pP_CUSTOMER_NAME);
        $("#lblProdNameCust" + row).text(jsonResult[k].pP_PROD_NAME_CUST);
        $("#lblCondition" + row).text(jsonResult[k].conditioN_DESCRIPTION);
        $("#lblDimD" + row).text(jsonResult[k].proD_DIM_D);
        $("#lblShape" + row).text(jsonResult[k].proD_SHAPE);
        $("#lblWidthAndOther" + row).text(jsonResult[k].pP_PROD_DIM_OTHER);
        $("#lblLength" + row).text(jsonResult[k].proD_LENGTH);
        $("#txtCharge" + row).val(jsonResult[k].charge);

        $("#txtTabaNo" + row).val(jsonResult[k].tabA_NO_STRING);

        $("#txtNoOfUnits" + row).val(jsonResult[k].uniT_TYPE_STRING);
        //$("#txtNoOfUnits" + row).removeAttr("disabled");
        $("#txtWeightWithoutKG" + row).val(jsonResult[k].uniT);
        $("#txtAmount" + row).val(jsonResult[k].acT_ALLOC_QTY);
        $("#txtRate" + row).val(jsonResult[k].ratE);
        $("#txtShukkaLocation" + row).val(jsonResult[k].inpuT_LOCATION_CD);


    }
}


    