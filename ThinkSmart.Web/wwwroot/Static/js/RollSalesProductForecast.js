var ProductList = [];
var $btnSpecInput1;
var screenName = "";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var productCD;
var ProductCdList = [];

$(document).ready(function () {
    $(document).getElementById("txtCustomerOnProd").style = "ime - mode:enable";
});


function GetProductDetailsByCD(productCD) {
    ProductCdList.push(productCD);
    var duplicateProductCdList = ProductCdList.filter((currentValue, currentIndex) =>
        ProductCdList.indexOf(currentValue) !== currentIndex);
    let index = ProductCdList.indexOf(duplicateProductCdList);
    if (duplicateProductCdList != "") {
        
        ProductCdList.splice(index);
        duplicateProductCdList = [];
        //ProductCdList.remove(MaterialCD);
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgRecordAlreadySelected").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }
    else {

        var ajaxTime = new Date().getTime();
        var jsonData = "{PRODUCT_CD :'" + productCD + "' }";
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/RollPoForecast/GetProductDetailsByProductCD?PRODUCT_CD=" + productCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                var result = response.message;
               // const productRowData = {};
                //const responseData = {};
                //const serial_No = 1;
                const productGridRows = document.querySelectorAll('.ProductGridRow'); // Select all rows with the class GridNormalRow
                const PRODUCT_LIST = [];
                if (productGridRows.length >= 1) {
                    for (let i = 0; i < productGridRows.length; i += 2) {
                        let productRowData = {}; 
                        const row1 = productGridRows[i];
                        const row2 = productGridRows[i + 1]; 
                        if (!row1 || !row2) break;
                        productRowData.PRODUCT_CD = row1.getAttribute('data-lblProductCD');
                        productRowData.ROLL_FC_CD = row1.dataset.rollfccd ? Number(row1.dataset.rollfccd) : null;
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
                        //serial_No++;
                    }
                }
                debugger;
                var result = response.message;
                ProductList = response.message;
                if (response.message != "") {
                    debugger;
                    let responseData = {}; 
                    responseData.PRODUCT_CD = response.message.producT_CD;
                    responseData.PROCDUCT_KUBUN_STRING = response.message.procducT_KUBUN_STRING;
                    responseData.CUSTOMER_NAME = response.message.customeR_NAME;
                    responseData.PROD_DIM_D = response.message.proD_DIM_D; //row1.cells[2]?.innerText.trim() || '';
                    responseData.SHAPE = response.message.shapE;
                    responseData.PROD_DIM_W = response.message.PROD_DIM_W;
                    responseData.PROD_DIM_OTHER = response.message.proD_DIM_OTHER;
                    responseData.PROD_LENGTH = response.message.proD_LENGTH;
                    responseData.MAGARI_BEND_TOLLERENCE = response.message.magarI_BEND_TOLLERENCE;
                    responseData.CONDITION_DESCRIPTION = response.message.conditioN_DESCRIPTION;
                    responseData.MATL_MAKER_NAME = response.message.matL_MAKER_NAME;
                    responseData.INTERNAL_NAME_STRING = response.message.internaL_NAME_STRING;
                    responseData.DIM_TOL = response.message.diM_TOL;
                    responseData.LEN_TOL = response.message.leN_TOL;
                    responseData.MAKER_NAME = response.message.makeR_NAME;
                    PRODUCT_LIST.push(responseData);
                }

                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/GetRollSalesForecastProductGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(PRODUCT_LIST)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                        debugger;
                        var result = response.message;
                        

                        $("#divRollSalesForecastProductGrid").html(response.message);
                        AddNewRowEndOfProductGrid();
                        CalculateAllGridMonthRoll();
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
        OnProductCancelBtnClick();
    }
}

$(document).on('click', '#btnSortOnProduct', function () {
    $("#SortPopUpModal").modal('show');

})
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
                }99
            });
        }
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

$("#btnInitializeOnProduct").click(function () {
    clearCommonSearchFieldsOnProduct();
});
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
                cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnProductListBtnClick(this)" style="height:16px;height:16px;float:right" />`;
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
                cell.innerHTML = `<input type="text" ID="txtMagariBlend" class="labelClass tdInputBoxBorderWidth" value="" />`;

            }
            else if (i === 5) {

                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtCondition" class="labelClass tdInputBoxBorderWidth" value="" />`;

            }
            else if (i === 6) {
                cell.colSpan = 8
                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" ID="txtMsgMatl" class="labelClass tdInputBoxBorderWidth" value="" />`;

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
function clearCommonSearchFieldsOnProduct() {

    $('#txtCustomerOnProd').val('');
    $('#txtInternalName').val('');
    $('#txtProdNameCust').val('');
    $('#txtDiaMin').val('');
    $('#txtDiaMax').val('');
    $('#txtShape').val('');
    $('#txtLengthMin').val('');
    $('#txtLengthMax').val('');
    $('#txtProdSpecNo1').val('');
    $('#txtProdSpecNo2').val('');
    var result = "";
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoForecast/ProductMasterListGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(result)
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings1)
        .done(function (response) {

            var result = response.message;
            $("#divProductMasterListGrid").html(response.message);
            CalculateAllGridMonthRoll();
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function OnClickProductList($callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    productCD = $.trim($callFrom.closest("tr").attr("data-lblProductCD"));
    GetProductDetailsByCD(productCD);

} 
function OnProductCancelBtnClick() {
    var element = document.getElementById("ProductListPopupModal");
    $("#ProductListPopupModal").removeClass("show");
    var test = document.getElementsByClassName("modal-backdrop fade show");

    var elements = document.getElementsByClassName('modal-backdrop fade show');
    for (var i in elements) {
        if (elements.hasOwnProperty(i)) {
            elements[i].className = '';
        }
    }

    document.getElementById("ProductListPopupModal").style.display = "none";
    clearCommonSearchFieldsOnProduct();
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



