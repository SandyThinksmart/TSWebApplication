var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
let apiResponses = [];
var MaterialCD;
var MaterialCdList = [];

$(document).ready(function () {
   
    setMaterialType();
    setMatlSpecName();
    setDecimalFormatsOnMaterial();
   
});
function setDecimalFormatsOnMaterial() {
    $("#txtDiaMinOnMatl,#txtDiaMaxOnMatl").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthMinOnMatl,#txtLengthMaxOnMatl").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

}

function GetMaterialDetailsByCD(MaterialCD) {
    debugger;
    MaterialCdList.push(MaterialCD);
    var duplicateMaterialCdList = MaterialCdList.filter((currentValue, currentIndex) =>
        MaterialCdList.indexOf(currentValue) !== currentIndex);
    let index = MaterialCdList.indexOf(duplicateMaterialCdList);
    if (duplicateMaterialCdList != "") {
        MaterialCdList.splice(index);
        duplicateMaterialCdList = [];
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgRecordAlreadySelected").text());
        showMessageDialog(message, focusFieldID, WARNING);
    } else {
        var ajaxTime = new Date().getTime();
        var jsonData = "{MATERIAL_CD :'" + MaterialCD + "' }";
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/RollPoForecast/GetMaterialDetailsByMaterialCD?MATERIAL_CD=" + MaterialCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                const materialGridRows = document.querySelectorAll('.MaterialGridRow');
                const MATERIAL_LIST = [];
                if (materialGridRows.length >= 1) {
                    for (let i = 0; i < materialGridRows.length; i += 2) {
                        let materialRowData = {};
                        const row1 = materialGridRows[i];
                        const row2 = materialGridRows[i + 1];
                        if (!row1 || !row2) break;

                        // Row 1 data
                        materialRowData.MATERIAL_CD = row1.getAttribute('data-lblMaterialCD');
                        materialRowData.ROLL_FC_CD = row1.dataset.rollfccd ? Number(row1.dataset.rollfccd) : null;
                        materialRowData.MAKER_NAME = row1.cells[1]?.innerText.trim() || '';
                        materialRowData.MATERIAL_TYPE_NAME_CBK = row1.cells[2]?.innerText.trim() || '';
                        materialRowData.MATL_DIM_D = row1.cells[3]?.innerText.trim() || '';
                        materialRowData.MATL_SHAPE = row1.cells[4]?.innerText.trim() || '';
                        materialRowData.MATL_DIM_W = row1.cells[5]?.innerText.trim() || '';
                        materialRowData.MATL_DIM_OTHER = row1.cells[6]?.innerText.trim() || '';
                        materialRowData.MATERIAL_LENGTH = row1.cells[7]?.innerText.trim() || '0';
                        materialRowData.FC_QTY_R1 = parseFloat(row1.cells[8]?.querySelector('input')?.value.trim()) || null;
                        materialRowData.FC_QTY_R2 = parseFloat(row1.cells[9]?.querySelector('input')?.value.trim()) || null;
                        materialRowData.FC_QTY_R3 = parseFloat(row1.cells[10]?.querySelector('input')?.value.trim()) || null;
                        materialRowData.FC_QTY_R4 = parseFloat(row1.cells[11]?.querySelector('input')?.value.trim()) || null;

                        // Row 2 data
                        materialRowData.MATL_MAKER_NAME = row2.cells[0]?.innerText.trim() || '';
                        materialRowData.INTERNAL_NAME_STRING = row2.cells[1]?.innerText.trim() || '';
                        materialRowData.MATL_SPECIAL_KUBUN = row2.cells[2]?.innerText.trim() || '';
                        materialRowData.CONDITION_DESCRIPTION = row2.cells[3]?.innerText.trim() || '';
                        materialRowData.MOCHIKOMISAKI_NAME = row2.cells[4]?.innerText.trim() || '';
                        materialRowData.REMARKS = row2.cells[5]?.querySelector('input')?.value.trim() || '';
                        MATERIAL_LIST.push(materialRowData);
                    }
                }
                if (response.message != "") {
                    debugger;
                    let responseData = {};
                    responseData.MATERIAL_CD = response.message.materiaL_CD;
                    responseData.MATERIAL_TYPE_NAME_CBK = response.message.materiaL_TYPE_NAME_CBK; 
                    responseData.MATL_DIM_D = response.message.matL_DIM_D;
                    responseData.MATL_SHAPE = response.message.matL_SHAPE; //row1.cells[2]?.innerText.trim() || '';
                    responseData.MATL_DIM_W = response.message.matL_DIM_W;
                    responseData.MATL_DIM_OTHER = response.message.matL_DIM_OTHER;
                    responseData.MATERIAL_LENGTH = response.message.materiaL_LENGTH;
                    responseData.CONDITION_DESCRIPTION = response.message.conditioN_DESCRIPTION;
                    responseData.MATL_MAKER_NAME = response.message.matL_MAKER_NAME;
                    responseData.INTERNAL_NAME_STRING = response.message.internaL_NAME_STRING
                    responseData.MAKER_NAME = response.message.makeR_NAME;
                    responseData.MATL_SPECIAL_KUBUN = response.message.matL_SPECIAL_KUBUN;
                    responseData.MOCHIKOMISAKI_NAME = response.message.mochikomisakI_NAME;
                    MATERIAL_LIST.push(responseData);
                }









                var result = response.message;
                ProductList = response.message;
                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoForecast/GetRollSalesForecastMaterialGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(MATERIAL_LIST)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                       
                        var result = response.message;
                       
                        switchTab($("#TbSalesProductTab"), $("#TbSalesMaterialTab"));
                        $("#divRollSalesForecastMaterialGrid").html(response.message);
                        CalculateAllGridMonthRoll();
                        AddNewRowEndOfMaterialGrid()
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
        OnMaterialCancelBtnClick();
    }
}

function AddNewRowEndOfMaterialGrid() {
    debugger;
    let lblYes = $("#lbl_Yes").text();
    let lblNo = $("#lbl_No").text();
    var table = document.getElementById("tblMaterialGrid");
    function insertRow11() {
        var newRow = table.insertRow(table.rows.length);
        newRow.className = "GridNormalRow";
        // Loop to create 18 cells and add input fields
        for (var i = 0; i < 12; i++) {
            var cell = newRow.insertCell(i);
            if (i === 0) {
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = `<select id="ddlMakerDetails" style="width: 88px; border-style: none;">
                                  <option>${lblYes}</option>
                                  <option>${lblNo}</option>
                                  </select>`;
//               
                               
            }
            // Add specific input type for the second cell (for button)
            else if (i === 1) {
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = `<input type="button" class="btnArrow" onclick = "OnMaterialListBtnClick($(this))" style="height:16px;float: right" />`;
                
            }
            else if (i === 8 || i === 9 || i === 10 || i === 11) {
                // For other cells, add a text input field
                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" class="txtRoll tdInputBoxBorderWidth tdInputBoxWidthRoll NumericFieldsClass" />`;
            }
            else  {
                // For other cells, add a text input field
                cell.className = "labelClass padddingLeft padddingRight";
                cell.innerHTML = ``;
            }
        }

    }

    function insertRow22() {
        var newRow = table.insertRow(table.rows.length);
        newRow.className = "GridNormalRow";
        for (var i = 0; i < 6; i++) {
            var cell = newRow.insertCell(i);
            if (i === 3  ) {
                cell.colSpan = 4
                cell.className = "labelClass";
                cell.innerHTML = ``;
            }
            else if (i === 5)
            {
                cell.colSpan = 4
                cell.className = "labelClass";
                cell.innerHTML = `<input type="text" class="tdInputBoxBorderWidth  GridRemark" />`;
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
function switchTab($tabFrom, $tabTo) {

    $tabTo.click();
}

$(document).on('click', '#btnSortOnMaterial', function () {
    $("#SortPopUpModal").modal('show');

});
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
                }
            });
        }
    }
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}

$("#btnInitializeOnMaterial").click(function () {
    clearCommonSearchFieldsOnMaterial();
});
function clearCommonSearchFieldsOnMaterial() {
    $('#txtMakerNameOnMatl').val('');
    $('#txtInternalNameOnMatl').val('');
    $('#txtRawMaterialNameOnMatl').val('');
    $('#txtDiaMinOnMatl').val('');
    $('#txtDiaMaxOnMatl').val('');
    $('#txtShapeOnMatl').val('');
    $('#txtLengthMinOnMatl').val('');
    $('#txtLengthMaxOnMatl').val('');
    var result = "";
    var settings1 = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoForecast/MaterialMasterListGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(result)
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

}
function BindMaterialTypeDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
       
        $("#ddlMaterialType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setMaterialType() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoForecast/GetMaterialTypeList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMaterialTypeDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindMatlSpecNameDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {
       
        $("#ddlMatlSpecName").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function setMatlSpecName() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoForecast/GetMatlSpecNameList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMatlSpecNameDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function OnClickMaterialList($callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    MaterialCD = $.trim($callFrom.closest("tr").attr("data-lblMaterialCD"));
    GetMaterialDetailsByCD(MaterialCD);
} 
function OnMaterialCancelBtnClick() {

    var element = document.getElementById("MaterialListPopupModal");
    $("#MaterialListPopupModal").removeClass("show");
    var test = document.getElementsByClassName("modal-backdrop fade show");

    var elements = document.getElementsByClassName('modal-backdrop fade show');
    for (var i in elements) {
        if (elements.hasOwnProperty(i)) {
            elements[i].className = '';
        }
    }

    document.getElementById("MaterialListPopupModal").style.display = "none";
    clearCommonSearchFieldsOnMaterial();
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