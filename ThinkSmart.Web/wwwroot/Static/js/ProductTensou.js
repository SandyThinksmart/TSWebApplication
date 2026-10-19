
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
    for (var k = 0; k <= jsonResult.length; k++) {
        var row = k + 1;
        $("#lblCustomer" + row).text(jsonResult[k].customeR_NAME);
        $("#lblProdNameCust" + row).text(jsonResult[k].producT_NAME_CUST);
        $("#lblCondition" + row).text(jsonResult[k].conditioN_DESCRIPTION);
        $("#lblDimD" + row).text(jsonResult[k].proD_DIM_D);
        $("#lblShape" + row).text(jsonResult[k].proD_SHAPE);
        $("#lblWidthAndOther" + row).text(jsonResult[k].proD_DIM_OTHER_STRING);
        $("#lblLength" + row).text(jsonResult[k].proD_LENGTH);
        $("#lblCharge" + row).text(jsonResult[k].chargE);

       /* $("#lblTabaNo" + row).text(jsonResult[k].customeR_NAME);*/
        $("#lblNoOfUnits" + row).text(jsonResult[k].nO_OF_UNITS);
        $("#lblWeightWithoutKG" + row).text(jsonResult[k].uniT);
        $("#lblAmount" + row).text(jsonResult[k].acT_ALLOC_QTY);
        //$("#lblS" + row).text(jsonResult[k].customeR_NAME);
        $("#lblShukkaLocation" + row).text(jsonResult[k].inpuT_LOCATION_CD);
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
           
            $("#txtWIPAcceptanceDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
            $("#txtDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
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
        if ($("#chkTensouAccepGrid" + i).is(':checked')) {
            $("#chkTensouAccepGrid" + i).prop('checked', false)
            clearTableRow("tr" + i);
            $("#chkTensouAccepGrid" + i).attr("disabled", true);
            isCheckPost = true
        }
    }
    if ($("#chkTensouAccepGrid" + i).prop('checked', false) && isCheckPost == false) {
        var message = $("#lblErrorMessageE_WIPTensou_022").text().trim();
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
    alert("hi");
    $("#SortPopUpModal").modal('show');
});

$("#btnSelect").click(function () {

    window.open(
        "/ProductTensou/ProductTensouListPage",
        "ListPage",
        "height=650,width=1100,resizable=no"
    );

});

//shukka scan then Auto focus on next textBox.
$(document).on('keypress', '[id^="txtShukkaCode"]', function (e) {
    var lstShukka = [];
    for (var i = 1; i <= emptyRow; i++) {
        var $tr = $(this);
        var shukka = $("#txtShukkaCode" + i).val();
        if (shukka != "") {
            lstShukka.push(tensou);
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
document.querySelector("#tblProductTensou tbody").addEventListener("change", function (event) {
    debugger;
    const row = event.target.closest("tr");
    var $val = row.id;
    const input = $val;
    const [, letters, number] = input.match(/^([a-zA-Z]+)(\d+)$/) || [];
    var txtval = ('txtShukkaCode' + number)
    const elementTransferNo = document.getElementById(txtval);
    if (elementTransferNo) {
        elementTransferNo.addEventListener("change", onTransferNoInputChange(number));
    }

});

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



    