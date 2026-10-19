
function OnPurchaseOrderQtyBtnClick() {
    window.open('/RollPoMainAnalysis/OrderAndReceiptHistoryPopUp', 'OrderAndReceiptHistoryPopUp', 'height = 850px, width = 1170px, resizable = no');
}
function OnMaterialWiseOrderQtyBtnClick() {
    window.open('/RollPoMainAnalysis/MatlNameWiseOrderQtyPopUp', 'MatlNameWiseOrderQtyPopUp', 'height = 710px, width = 490px, resizable = no');
}
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
let apiResponses = [];
var materialCD;
var materialMukesakiCdList = [];
var materialPoData = [];
var mukesakiList;
var gridRows = 0;
var gridRowsi = 0;
let fcSummaryNo;
var current_Roll;
var allAnalysisOverFlag;
var poCompleteFlag;
var dataBindFlag = false;
$(document).ready(function () {
    document.getElementById("materialData").value = "";
    const params = new URLSearchParams(window.location.search);
    fcSummaryNo = params.get('FcSummaryNo');
    SetMaterialType();
    SetMatlSpecName();
    SetDecimalFormatsOnMaterial();
    getRoll();
    checkAnalysisOverFlag(fcSummaryNo);
    checkPoCompleteFlag();
    if (poCompleteFlag == true) {
        document.getElementById("btnDeleteRow").disabled = true;
    }
    GetInternalNameWIseMaterialDetails(fcSummaryNo);
    GetInternalNameWiseMakerList(fcSummaryNo);
    SetGridColumnHeadingRValues();
    GetRollPoDetails(fcSummaryNo);
});
function checkAnalysisOverFlag() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/checkAnalysisOverFlag?INT_MATL_FC_SUMMARY_CD=" + fcSummaryNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.data == 1) {
                allAnalysisOverFlag = true;
            }
            else {
                allAnalysisOverFlag = false;
            }
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function checkPoCompleteFlag() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/checkPoCompleteFlag",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.data > 0) {
                poCompleteFlag = true;
            }
            else {
                poCompleteFlag = false;
            }
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function OnMaterialListBtnClick(editButton) {


    $("#AnalysePoQtyMaterialListPopupModal").modal('show');
    $("#AnalysePoQtyMaterialListPopupModal").addClass("show");
    var elements = document.getElementsByClassName('');
    for (var i in elements) {
        if (elements.hasOwnProperty(i)) {
            elements[i].className = 'modal-backdrop fade show';
        }
    }
    InitSortPopupOnMaterial();
    debugger;
    document.getElementById("AnalysePoQtyMaterialListPopupModal").style.display = "block";
    if ($("#txtInternalName").val() != '') {
        $('#txtInternalNameOnMatl').val($("#txtInternalName").val() ? $("#txtInternalName").val() : null).prop("disabled", true);
    }
    if ($("#txtDia").val() != '') {
        $('#txtDiaMinOnMatl').val($("#txtDia").val() ? $("#txtDia").val() : null).prop("disabled", true);
    }
    if ($("#txtShape").val() != '') {
        $('#txtShapeOnMatl').val($("#txtShape").val() ? $("#txtShape").val() : null).prop("disabled", true);
    }
    if ($("#txtLength").val() != '') {
        $('#txtLengthMinOnMatl').val($("#txtLength").val() ? $("#txtLength").val() : null).prop("disabled", true);
    }
    if ($("#hdfMatlKubunType").val() != '') {
        $("#ddlMaterialType").val($("#hdfMatlKubunType").val()).prop("disabled", true);
    }
    if ($("#hdfGenSpecType").val() != '') {
        $('#ddlMatlSpecName').val($("#hdfGenSpecType").val()).prop("disabled", true);
    }
}
function SetDecimalFormatsOnMaterial() {
    $("#txtDiaMinOnMatl,#txtDiaMaxOnMatl").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthMinOnMatl,#txtLengthMaxOnMatl").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtRate").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

}
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
        url: APIURL + "/AnalysePOQuantity/GetMaterialMasterDataOnAnalysePoQty",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            MAKER_NAME: maker_NAME, INTERNAL_NAME_STRING: internal_Name, MATL_SHAPE: shape, MATL_MAKER_NAME: material_Name_Maker,
            MATL_DIM_D_MIN: dia_Min, MATL_DIM_D_MAX: dia_Max, MATL_LENGTH_MIN: length_Min, MATL_LENGTH_MAX: length_Max, MATL_SPECIAL_KUBUN: Material_Spl_Name,
            MATL_TYPE_CBK: Material_Type_cbk, SORT_FIELDS: sortFields, INT_MATL_FC_SUMMARY_CD: fcSummaryNo,
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
                url: "/RollPoMainAnalysis/MaterialMasterListGrid",
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
$(document).on('click', '#btnSortOnMaterial', function () {
    $('#SortPopUpModal').css('margin-left', '-408px');
    $("#SortPopUpModal").modal('show');

});
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
function btnSortClose_Click() {

    closepopup();
}
function InitSortPopupOnMaterial() {

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: "/RollPoMainAnalysis/InitializeSortPopupForMaterialList",
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

    GetMaterialMasterDataList(selectedColumns);

}
function btnSortReset_Click() {
    InitSortPopupOnMaterial();
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
function BindMaterialTypeDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {

        $("#ddlMaterialType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
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
function SetMaterialType() {
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
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindMatlSpecNameDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {

        $("#ddlMatlSpecName").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

function SetMatlSpecName() {
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
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
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
function OnMaterialCancelBtnClick() {

    var element = document.getElementById("AnalysePoQtyMaterialListPopupModal");
    $("#AnalysePoQtyMaterialListPopupModal").removeClass("show");
    var test = document.getElementsByClassName("modal-backdrop fade show");

    var elements = document.getElementsByClassName('modal-backdrop fade show');
    for (var i in elements) {
        if (elements.hasOwnProperty(i)) {
            elements[i].className = '';
        }
    }

    document.getElementById("AnalysePoQtyMaterialListPopupModal").style.display = "none";
    clearCommonSearchFieldsOnMaterial();
}
function OnClickMaterialList($callFrom) {
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    MaterialCD = $.trim($callFrom.closest("tr").attr("data-lblMaterialCD"));
    MukesakiCD = $.trim($callFrom.closest("tr").attr("data-lblMukesakiCD"));
    GetMaterialDetailsByCD(MaterialCD, MukesakiCD);
}
function GetMaterialDetailsByCD(MaterialCD, MukesakiCD) {

    materialMukesakiCdList.push(MaterialCD + "_" + MukesakiCD);
    var duplicateMaterialMukesakiCdList = materialMukesakiCdList.filter((currentValue, currentIndex) =>
        materialMukesakiCdList.indexOf(currentValue) !== currentIndex);
    let index = materialMukesakiCdList.indexOf(duplicateMaterialMukesakiCdList);
    if (duplicateMaterialMukesakiCdList != "") {
        materialMukesakiCdList.splice(index);
        duplicateMaterialMukesakiCdList = [];
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
            url: APIURL + "/AnalysePOQuantity/GetMaterialDetailsByMaterialCD?MATERIAL_CD=" + MaterialCD + " &MUKESAKI_CD=" + MukesakiCD,
            method: "GET",
            contentType: "application/json; charset=utf-8",
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var mukesaki_Cd;
                var supplier_Cd;
                var mochikomisaki_CD;
                var futou_CD;
                var htsaki_CD;
                const materialRows = document.querySelectorAll('.MaterialGridRow');
                var materialDataList = [];
                var allRowMaterialData = {};

                //get existing row data from grid
                if (materialRows.length >= 1 && dataBindFlag == true) {
                    materialRows.forEach(row => {
                        allRowMaterialData = {};
                        const lblMateriallName = row.cells[1];
                        const txtBoxRawMatl = row.cells[2];
                        const lblMaterialDimD = row.cells[3];
                        const labMaterialShape = row.cells[4];
                        const lblMaterialDimW = row.cells[5];
                        const lblMaterialOther = row.cells[6];
                        const lblMaterialLength = row.cells[7];
                        const lblCondDescription = row.cells[8];
                        const lblMaterialTypeCbk = row.cells[9];
                        const lblMaterialSplKubun = row.cells[10];
                        const mukesakiDropdownCell = row.cells[11];
                        const textboxMukesakiRate = row.cells[12];
                        const textboxQty = row.cells[13];
                        const textboxRollYear = row.cells[14];
                        const textboxRoll = row.cells[15];
                        const textboxRollChance = row.cells[16];
                        const supplierDropdownCell = row.cells[17];
                        const mochikomisakiDropdownCell = row.cells[18];
                        const futouDropdownCell = row.cells[19];
                        const hikitorisakiDropdownCell = row.cells[20];
                        const textboxRemark = row.cells[21];
                        const textboxMatlNameMakCd = row.cells[23];
                        const textboxMatlKubun = row.cells[24];

                        //get value from grid dropdown
                        if (mukesakiDropdownCell || supplierDropdownCell || mochikomisakiDropdownCell || futouDropdownCell || hikitorisakiDropdownCell) {

                            const mukesakiDropdown = mukesakiDropdownCell.querySelector('select');
                            const supplierDropdown = supplierDropdownCell.querySelector('select');
                            const mochikomisakiDropdown = mochikomisakiDropdownCell.querySelector('select');
                            const futouDropdown = futouDropdownCell.querySelector('select');
                            const hikitorisakiDropdown = hikitorisakiDropdownCell.querySelector('select'); // Get the <select> element inside the <td>

                            if (mukesakiDropdown || supplierDropdown || mochikomisakiDropdown || futouDropdown || hikitorisakiDropdown) {
                                allRowMaterialData.MUKESAKI_CD = mukesakiDropdown.value.trim() || ''; // Get the selected value of the dropdown
                                allRowMaterialData.SUPPLIER_CD = supplierDropdown.value.trim() || '';
                                allRowMaterialData.MOCHIKOMISAKI_CD = mochikomisakiDropdown.value.trim() || '';
                                allRowMaterialData.FUTOU_CD = futouDropdown.value.trim() || '';
                                allRowMaterialData.HTSAKI_CD = hikitorisakiDropdown.value.trim() || '';
                            }
                        }
                        //get value from grid textbox
                        if (textboxQty || textboxRollYear || textboxRoll || textboxRollChance || textboxRemark
                            || lblMateriallName || txtBoxRawMatl || lblMaterialDimD || labMaterialShape
                            || lblMaterialDimW || lblMaterialOther || lblMaterialLength || lblCondDescription || lblMaterialTypeCbk || lblMaterialSplKubun || textboxMukesakiRate || textboxMatlNameMakCd || textboxMatlKubun) {
                            const lblMatlName = lblMateriallName.querySelector('.materialNameLabel')?.textContent.trim();
                            const txtRawMatl = txtBoxRawMatl.querySelector('input[type="text"]');
                            const lblMatlDimD = lblMaterialDimD.querySelector('.materialDimDLabel')?.textContent.trim();
                            const lblMatlShape = labMaterialShape.querySelector('.materialShapeLabel')?.textContent.trim();
                            const lblMatlDimW = lblMaterialDimW.querySelector('.materialDimWLabel')?.textContent.trim();
                            const lblMatlOther = lblMaterialOther.querySelector('.materialOtherLabel')?.textContent.trim();
                            const lblMatlLength = lblMaterialLength.querySelector('.materiallengthLabel')?.textContent.trim();
                            const lblMatlCondDescription = lblCondDescription.querySelector('.materialCondiLabel')?.textContent.trim();
                            const lblMatlTypeCbk = lblMaterialTypeCbk.querySelector('.materialtypeLabel')?.textContent.trim();
                            const lblMatlSplKubun = lblMaterialSplKubun.querySelector('.materialKubunLabel')?.textContent.trim();
                            const textQty = textboxQty.querySelector('input[type="text"]'); // Get the <input type="text"> element inside the <td>
                            const textRollYear = textboxRollYear.querySelector('input[type="text"]');
                            const textRoll = textboxRoll.querySelector('input[type="text"]');
                            const textRollChance = textboxRollChance.querySelector('input[type="text"]');
                            const textRemark = textboxRemark.querySelector('input[type="text"]');
                            const textMukesakiRate = textboxMukesakiRate.querySelector('input[type="text"]');
                            const textMatlNameMakerCd = textboxMatlNameMakCd.querySelector('input[type="text"]');
                            const textMatlKubun = textboxMatlKubun.querySelector('input[type="text"]');

                            if (textQty || textRollYear || textRoll || textRollChance || textRemark || lblMatlName
                                || txtRawMatl || lblMatlDimD || textMatlShape || lblMatlDimW || lblMatlOther || lblMatlLength || textMukesakiRate || textMatlNameMakerCd || textMatlKubun
                                || lblMatlCondDescription || lblMatlTypeCbk || lblMatlSplKubun) {
                                allRowMaterialData.MAKER_NAME = lblMatlName || '';
                                allRowMaterialData.MATL_MAKER_NAME = txtRawMatl.value.trim() || '';
                                allRowMaterialData.MATL_DIM_D = lblMatlDimD || '';
                                allRowMaterialData.MATL_SHAPE = lblMatlShape || '';
                                allRowMaterialData.MATL_DIM_W = lblMatlDimW || '';
                                allRowMaterialData.MATL_DIM_OTHER = lblMatlOther || '';
                                allRowMaterialData.MATERIAL_LENGTH = lblMatlLength || '';
                                allRowMaterialData.CONDITION_DESCRIPTION = lblMatlCondDescription || '';
                                allRowMaterialData.MATERIAL_TYPE_NAME_CBK = lblMatlTypeCbk || '';
                                allRowMaterialData.MATL_SPECIAL_KUBUN = lblMatlSplKubun || '';
                                allRowMaterialData.QTY = textQty.value.trim() || ''; // Get the value of the textbox
                                allRowMaterialData.ROLL_YEAR = textRollYear.value.trim() || '';
                                allRowMaterialData.ROLL_R = textRoll.value.trim() || '';
                                allRowMaterialData.ROLL_CHANCE = textRollChance.value.trim() || '';
                                allRowMaterialData.REMARKS = textRemark.value.trim() || '';
                                allRowMaterialData.RATE = textMukesakiRate.value.trim() || null;
                                allRowMaterialData.MATL_NAME_MAKER_CD = textMatlNameMakerCd.value.trim() || null;
                                allRowMaterialData.MATL_KUBUN = textMatlKubun.value.trim() || null;
                            }
                        }
                        allRowMaterialData.MATERIAL_CD = row.cells[22]?.innerText.trim() || '';
                        //allRowMaterialData.RATE = row.cells[22]?.innerText.trim() || '';
                        allRowMaterialData.USER_ID = $("#hdfLoginId").val();

                        materialDataList.push(allRowMaterialData);
                    });
                }
                //get data from api responce
                if (response.message != "") {
                    dataBindFlag = true;
                    allRowMaterialData = {};
                    allRowMaterialData.MAKER_NAME = response.message.makeR_NAME;
                    allRowMaterialData.MATL_MAKER_NAME = response.message.matL_MAKER_NAME;
                    allRowMaterialData.MATL_DIM_D = response.message.matL_DIM_D;
                    allRowMaterialData.MATL_SHAPE = response.message.matL_SHAPE;
                    allRowMaterialData.MATL_DIM_W = response.message.matL_DIM_W;
                    allRowMaterialData.MATL_DIM_OTHER = response.message.matL_DIM_OTHER;
                    allRowMaterialData.MATERIAL_LENGTH = response.message.materiaL_LENGTH;
                    allRowMaterialData.CONDITION_DESCRIPTION = response.message.conditioN_DESCRIPTION;
                    allRowMaterialData.MATERIAL_TYPE_NAME_CBK = response.message.materiaL_TYPE_NAME_CBK;
                    allRowMaterialData.MATL_SPECIAL_KUBUN = response.message.matL_SPECIAL_KUBUN;
                    allRowMaterialData.MUKESAKI_CD = response.message.mukesakI_CD;
                    allRowMaterialData.SUPPLIER_CD = response.message.supplieR_CD;
                    allRowMaterialData.MOCHIKOMISAKI_CD = response.message.mochikomisakI_CD;
                    allRowMaterialData.FUTOU_CD = response.message.futoU_CD;
                    allRowMaterialData.HTSAKI_CD = response.message.htsakI_CD;
                    allRowMaterialData.MATERIAL_CD = response.message.materiaL_CD;
                    allRowMaterialData.RATE = response.message.rate;
                    allRowMaterialData.MATL_NAME_MAKER_CD = response.message.matL_NAME_MAKER_CD;
                    allRowMaterialData.MATL_KUBUN = response.message.matL_KUBUN;
                    allRowMaterialData.ROLL_YEAR = response.message.rolL_YEAR;
                    allRowMaterialData.ROLL_R = response.message.rolL_R;
                    allRowMaterialData.USER_ID = $("#hdfLoginId").val();
                    materialDataList.push(allRowMaterialData);

                }


                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoMainAnalysis/GetMaterialMasterDataView",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(materialDataList)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                        var result = response.message;
                        $("#pnlMaterialPOGrid").html(response.message);
                        AddNewRowEndOfMaterialGrid();
                        gridRows = materialRows.length + 1;
                        //SetMukesaki(gridRows);
                        SetSupplier(gridRows);
                        SetDestination(gridRows);
                        SetHikitorisaki(gridRows);
                        SetFutou(gridRows);
                        var gridRowsCnt = materialDataList.length;
                        for (var i = 1; i <= gridRowsCnt; i++) {
                            var mukesakiCd = materialDataList[i - 1].MUKESAKI_CD;

                            if (mukesakiCd != null) {
                                // Bind dropdown options per row
                                SetMukesakiDropdown($("#ddlMukesaki" + i), mukesakiCd);
                            }
                            else {
                                SetMukesakiDropdown($("#ddlMukesaki" + i), "0");
                            }

                            // Set selected value
                            $("#ddlMukesaki" + i).val(mukesakiCd);

                            //$("#ddlMukesaki" + (i)).val(materialDataList[i - 1].MUKESAKI_CD);

                            var supplierCd = materialDataList[i - 1].SUPPLIER_CD;
                            if (supplierCd != null) {
                                // Bind dropdown options per row
                                SetSupplierDropdown($("#ddlSupplier" + i), supplierCd);
                            }
                            else {
                                SetSupplierDropdown($("#ddlSupplier" + i), "0");
                            }
                            // Bind dropdown options per row


                            // Set selected value
                            $("#ddlSupplier" + i).val(supplierCd);
                            var mochikomisakiCd = materialDataList[i - 1].MOCHIKOMISAKI_CD;
                            // Bind dropdown options per row
                            if (mochikomisakiCd != null) {
                                // Bind dropdown options per row
                                SetMochikomisakiDropdown($("#ddlDestination" + i), mochikomisakiCd);
                            }
                            else {
                                SetMochikomisakiDropdown($("#ddlDestination" + i), "0");
                            }


                            // Set selected value
                            $("#ddlDestination" + i).val(mochikomisakiCd);
                            $("#ddlFutou" + (i)).val(materialDataList[i - 1].FUTOU_CD);
                            $("#ddlHikitorisaki" + (i)).val(materialDataList[i - 1].HTSAKI_CD);

                        }
                        $(".materialqty").keyup(function (event) {
                            RestrictToValidDecimalFormat($(this), 5, 2);
                        });
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
    // Get the table by its ID
    var table = document.getElementById("tblMaterialPoGrid");

    var newRow = table.insertRow(table.rows.length);
    newRow.className = "GridNormalRow";
    // Loop to create 18 cells and add input fields
    for (var i = 0; i < 22; i++) {
        var cell = newRow.insertCell(i);

        // Add specific input type for the second cell (for button)
        if (i == 0) {
            cell.className = "labelClass ";
            cell.innerHTML = `<input type="radio" id="btnRadio" name="matlRadio"/>`;
        }
        if (i === 2) {
            cell.className = "labelClass padddingLeft padddingRight GridMandatoryTextBox";
            cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnMaterialListBtnClick(this)" style="height: 16px;float: right;width: 25px;" />`;
        }

        else if (i == 11 || i == 17 || i == 18 || i == 19 || i == 20) {
            cell.innerHTML = `<select ID="" data-id="" runat="server"  Class="labelClass GridMandatoryDropdown" style="float: right;border:none;width:208px;" disabled"></select>`
        }
        else if (i == 13 || i == 14 || i == 15 || i == 16) {
            cell.className = "labelClass GridMandatoryTextBox";
            cell.innerHTML = `<input type="text" runat="server" style ="border:none;width:10px; />`;
        }
        else if (i == 21) {
            // For other cells, add a text input field
            cell.className = "";
            cell.innerHTML = `<input type="text" style="border:none;width: 49px; />`;
        }
        else {
            // For other cells, add a text input field
            cell.className = "labelClass";
            cell.innerHTML = ``;
        }
    }


}
function SetMukesaki(rowsCount) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetMukesakiList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            mukesakiList = response.data;
            BindMukesakiDropdown(response.data, rowsCount);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindMukesakiDropdown(jsonResult, rowsCount) {
    for (var i = 1; i <= rowsCount; i++) {


        $("#ddlMukesaki" + i).html("").append($('<option value="0"></option>'));
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlMukesaki" + i).append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });

    }
}
function SetSupplier(rowsCount) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetSupplierList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindSupplierDropdown(response.data, rowsCount);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindSupplierDropdown(jsonResult, rowsCount) {

    for (var i = 1; i <= rowsCount; i++) {


        $("#ddlSupplier" + i).html("").append($('<option value="0"></option>'));
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlSupplier" + i).append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });

    }
}
function SetDestination(rowsCount) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetDestinationList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDestinationDropdown(response.data, rowsCount);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindDestinationDropdown(jsonResult, rowsCount) {
    for (var i = 1; i <= rowsCount; i++) {
        $("#ddlDestination" + i).html("").append($('<option value="0"></option>'));
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlDestination" + i).append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });

    }
}
function SetHikitorisaki(rowsCount) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetHikitorisakiList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindHikitorisakiDropdown(response.data, rowsCount);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindHikitorisakiDropdown(jsonResult, rowsCount) {
    for (var i = 1; i <= rowsCount; i++) {
        $("#ddlHikitorisaki" + i).html("").append($('<option value="0"></option>'));
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlHikitorisaki" + i).append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });
    }
}
function SetFutou(rowsCount) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetFutouList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindFutouDropdown(response.data, rowsCount);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindFutouDropdown(jsonResult, rowsCount) {

    for (var i = 1; i <= rowsCount; i++) {


        $("#ddlFutou" + i).html("").append($('<option value="0"></option>'));
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlFutou" + i).append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });

    }
}
$("#btnCancel").click(function (event) {
    //self.close();
    window.open('', 'AnalysePOQuantity').close();
    window.open('', 'RollForecastPopUp').close();
});
$("#btnSaveMaterialPo").click(function (event) {
    debugger;  

    validationMessage = "";
    message = "";

    const materialRows = document.querySelectorAll('.MaterialGridRow'); // Select all rows with the class GridNormalRow
    var materialDataList = [];
    /* materialRows.forEach(row => {*/
    gridRows = materialRows.length;
    for (i = 1; i <= gridRows; i++) {
        if ($("#txtQty" + i).val() == "") {
            validationMessage = $.trim($("#valMsgPoWtRequired").text());
        }
        if ($("#txtRollChance" + i).val() == "") {
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#valMsgRollChanceRequired").text());
            }
            else {
                validationMessage = $.trim($("#valMsgRollChanceRequired").text());
            }
        }
        if ($("#ddlMukesaki" + i).val() == '0' || $("#ddlMukesaki" + i).val() == null) {
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#valMsgMukesakiRequired").text());
            }
            else {
                validationMessage = $.trim($("#valMsgMukesakiRequired").text());
            }
        }
        if ($("#ddlSupplier" + i).val() == '0' || $("#ddlSupplier" + i).val() == null) {
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#valMsgSupplierRequired").text());
            }
            else {
                validationMessage = $.trim($("#valMsgSupplierRequired").text());
            }
        }
        if ($("#ddlDestination" + i).val() == '0' || $("#ddlDestination" + i).val() == null) {
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#valMsgDestinationRequired").text());
            }
            else {
                validationMessage = $.trim($("#valMsgDestinationRequired").text());
            }
        }
        if ($("#ddlFutou" + i).val() == '0' || $("#ddlFutou" + i).val() == null) {
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#valMsgWHStockRequired").text());
            }
            else {
                validationMessage = $.trim($("#valMsgWHStockRequired").text());
            }
        }
        if ($("#ddlHikitorisaki" + i).val() == '0' || $("#ddlHikitorisaki" + i).val() == null) {
            if (validationMessage != "") {
                validationMessage = validationMessage + "<br>" + $.trim($("#valMsgTransCompRequired").text());
            }
            else {
                validationMessage = $.trim($("#valMsgTransCompRequired").text());
            }
        }
    }

    /* });*/

    if (validationMessage != "") {

        focusFieldID = $(':focus').attr("id");
        showMessageDialog(validationMessage, focusFieldID, WARNING);
    }
    else {
        debugger;
        const materialRows = document.querySelectorAll('.MaterialGridRow'); // Select all rows with the class GridNormalRow
        var materialDataList = [];
        var gridFcSummaryNo = {};
        materialRows.forEach(row => {
            // Create an object to store the material data for the current row
            var allMaterialRowData = {};

            // Extract text content from <td> elements in the row
            const mukesakiDropdownCell = row.cells[11];
            const supplierDropdownCell = row.cells[17];
            const mochikomisakiDropdownCell = row.cells[18];
            const futouDropdownCell = row.cells[19];
            const hikitorisakiDropdownCell = row.cells[20];
            if (mukesakiDropdownCell || supplierDropdownCell || mochikomisakiDropdownCell || futouDropdownCell || hikitorisakiDropdownCell) {
                const mukesakiDropdown = mukesakiDropdownCell.querySelector('select');
                const supplierDropdown = supplierDropdownCell.querySelector('select');
                const mochikomisakiDropdown = mochikomisakiDropdownCell.querySelector('select');
                const futouDropdown = futouDropdownCell.querySelector('select');
                const hikitorisakiDropdown = hikitorisakiDropdownCell.querySelector('select'); // Get the <select> element inside the <td>
                if (mukesakiDropdown || supplierDropdown || mochikomisakiDropdown || futouDropdown || hikitorisakiDropdown) {
                    allMaterialRowData.MUKESAKI_CD = mukesakiDropdown.value.trim() || ''; // Get the selected value of the dropdown
                    allMaterialRowData.SUPPLIER_CD = supplierDropdown.value.trim() || '';
                    allMaterialRowData.MOCHIKOMISAKI_CD = mochikomisakiDropdown.value.trim() || '';
                    allMaterialRowData.FUTOU_CD = futouDropdown.value.trim() || '';
                    allMaterialRowData.HTSAKI_CD = hikitorisakiDropdown.value.trim() || '';
                }
            }
            const textboxMukesakiRate = row.cells[12];
            const textboxQty = row.cells[13]; // Replace 23 with the actual index of the textbox column
            const textboxRollYear = row.cells[14];
            const textboxRoll = row.cells[15];
            const textboxRollChance = row.cells[16];
            const textboxRemark = row.cells[21];
            const textboxMatlNameMakCd = row.cells[23];
            const textboxMatlKubun = row.cells[24];
            if (textboxQty || textboxRollYear || textboxRoll || textboxRollChance || textboxRemark || textboxMukesakiRate || textboxMatlNameMakCd || textboxMatlKubun) {
                const textQty = textboxQty.querySelector('input[type="text"]'); // Get the <input type="text"> element inside the <td>
                const textRollYear = textboxRollYear.querySelector('input[type="text"]');
                const textRoll = textboxRoll.querySelector('input[type="text"]');
                const textRollChance = textboxRollChance.querySelector('input[type="text"]');
                const textRemark = textboxRemark.querySelector('input[type="text"]');
                const textMukesakiRate = textboxMukesakiRate.querySelector('input[type="text"]');
                const textMatlNameMakerCd = textboxMatlNameMakCd.querySelector('input[type="text"]');
                const textMatlKubun = textboxMatlKubun.querySelector('input[type="text"]');
                if (textQty || textRollYear || textRoll || textRollChance || textRemark || textMukesakiRate || textMatlNameMakerCd || textMatlKubun) {
                    allMaterialRowData.QTY = textQty.value.trim() || ''; // Get the value of the textbox
                    allMaterialRowData.ROLL_YEAR = textRollYear.value.trim() || '';
                    allMaterialRowData.ROLL_R = textRoll.value.trim() || '';
                    allMaterialRowData.ROLL_CHANCE = textRollChance.value.trim() || '';
                    allMaterialRowData.REMARKS = textRemark.value.trim() || '';
                    allMaterialRowData.RATE = textMukesakiRate.value.trim() || null;
                }
            }
            allMaterialRowData.MATERIAL_CD = row.cells[22]?.innerText.trim() || '';
            //allMaterialRowData.RATE = (row.cells[22]?.innerText.trim() || null);
            allMaterialRowData.USER_ID = $("#hdfLoginId").val();
            allMaterialRowData.INT_MATL_FC_SUMMARY_CD = fcSummaryNo;
            const checkbox = document.querySelector('.large-checkbox');
            if (checkbox.checked) {
                allMaterialRowData.CHECKEDBOX_VALUE = true;
            } else {
                allMaterialRowData.CHECKEDBOX_VALUE = false;
            }

            materialDataList.push(allMaterialRowData);

        });
        if (materialDataList.length > 0) {
            materialDataList = materialDataList;
        }
        else {
            gridFcSummaryNo.INT_MATL_FC_SUMMARY_CD = fcSummaryNo;
            materialDataList.push(gridFcSummaryNo);
        }

        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/AnalysePOQuantity/SaveMaterialPoData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(materialDataList)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.message == "Data Inserted Successfully") {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#successMsgMonthlyRollOrder").text());
                    //showMessageDialog(message, focusFieldID, SUCCESS);
                    bootbox.alert({
                        title: "<div style='color:white'></div>",
                        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
                        centerVertical: true,
                        callback: function () {
                            //self.close();
                            window.open('', 'AnalysePOQuantity').close();
                            window.open('', 'RollForecastPopUp').close();
                        }
                    });

                    let isChanged = localStorage.getItem("forecastChanged");
                    if (isChanged === "true") {

                        let forecastData = localStorage.getItem("forecastData");


                        forecastData = JSON.parse(forecastData);

                        var ajaxTime = new Date().getTime();
                        var settings = {
                            async: true,
                            crossDomain: true,
                            cache: false,
                            dataType: "json",
                            url: APIURL + "/AnalysePOQuantity/UpdateForecastData",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(forecastData)
                        }
                        $.ajax(settings)
                            .done(function (response) {
                                responseTime = new Date().getTime() - ajaxTime;
                                console.log("data updated");
                                localStorage.removeItem("forecastChanged");
                                localStorage.removeItem("forecastData");

                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                var errorCode = $.parseJSON(xhr.responseText).Message;

                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblSystemException").text());
                                showMessageDialog(message, focusFieldID, ERROR);
                            });
                    }
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblSystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

    

});
function deleteRowMaterialPoGrid() {

    var table = document.getElementById("tblMaterialPoGrid");
    var rows = table.rows;

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var radioButton = row.querySelector('input[type="radio"]');

        if (radioButton && radioButton.checked) {
            // If the radio button is checked, delete the row
            table.deleteRow(i);
            break; // Stop the loop after deleting the row
        }
    }
    const materialRows = document.querySelectorAll('.MaterialGridRow');
    //materialMukesakiCdList = [];
    //materialRows.forEach(row => {

    //    materialMukesakiCdList.push(row.cells[21]?.innerText.trim() || '');
    //});
    if (materialRows.length > 0) {
        var mukesaki_Cd;
        var supplier_Cd;
        var mochikomisaki_CD;
        var futou_CD;
        var htsaki_CD;
        //  const materialRows = document.querySelectorAll('.MaterialGridRow');
        var materialDataList = [];
        var allRowMaterialData = {};

        //get existing row data from grid
        if (materialRows.length >= 1 && dataBindFlag == true) {
            materialRows.forEach(row => {
                allRowMaterialData = {};
                const lblMateriallName = row.cells[1];
                const txtBoxRawMatl = row.cells[2];
                const lblMaterialDimD = row.cells[3];
                const labMaterialShape = row.cells[4];
                const lblMaterialDimW = row.cells[5];

                const lblMaterialOther = row.cells[6];
                const lblMaterialLength = row.cells[7];
                const lblCondDescription = row.cells[8];
                const lblMaterialTypeCbk = row.cells[9];
                const lblMaterialSplKubun = row.cells[10];
                const mukesakiDropdownCell = row.cells[11];
                const textboxMukesakiRate = row.cells[12];
                const textboxQty = row.cells[13];
                const textboxRollYear = row.cells[14];
                const textboxRoll = row.cells[15];
                const textboxRollChance = row.cells[16];
                const supplierDropdownCell = row.cells[17];
                const mochikomisakiDropdownCell = row.cells[18];
                const futouDropdownCell = row.cells[19];
                const hikitorisakiDropdownCell = row.cells[20];
                const textboxRemark = row.cells[21];
                const textboxMatlNameMakCd = row.cells[23];
                const textboxMatlKubun = row.cells[24];

                //get value from grid dropdown
                if (mukesakiDropdownCell || supplierDropdownCell || mochikomisakiDropdownCell || futouDropdownCell || hikitorisakiDropdownCell) {

                    const mukesakiDropdown = mukesakiDropdownCell.querySelector('select');
                    const supplierDropdown = supplierDropdownCell.querySelector('select');
                    const mochikomisakiDropdown = mochikomisakiDropdownCell.querySelector('select');
                    const futouDropdown = futouDropdownCell.querySelector('select');
                    const hikitorisakiDropdown = hikitorisakiDropdownCell.querySelector('select'); // Get the <select> element inside the <td>

                    if (mukesakiDropdown || supplierDropdown || mochikomisakiDropdown || futouDropdown || hikitorisakiDropdown) {
                        allRowMaterialData.MUKESAKI_CD = mukesakiDropdown.value.trim() || ''; // Get the selected value of the dropdown
                        allRowMaterialData.SUPPLIER_CD = supplierDropdown.value.trim() || '';
                        allRowMaterialData.MOCHIKOMISAKI_CD = mochikomisakiDropdown.value.trim() || '';
                        allRowMaterialData.FUTOU_CD = futouDropdown.value.trim() || '';
                        allRowMaterialData.HTSAKI_CD = hikitorisakiDropdown.value.trim() || '';
                    }
                }
                //get value from grid textbox
                if (textboxQty || textboxRollYear || textboxRoll || textboxRollChance || textboxRemark
                    || lblMateriallName || txtBoxRawMatl || lblMaterialDimD || labMaterialShape
                    || lblMaterialDimW || lblMaterialOther || lblMaterialLength || lblCondDescription || lblMaterialTypeCbk || lblMaterialSplKubun || textboxMukesakiRate || textboxMatlNameMakCd || textboxMatlKubun) {
                    const lblMatlName = lblMateriallName.querySelector('.materialNameLabel')?.textContent.trim();
                    const txtRawMatl = txtBoxRawMatl.querySelector('input[type="text"]');
                    const lblMatlDimD = lblMaterialDimD.querySelector('.materialDimDLabel')?.textContent.trim();
                    const lblMatlShape = labMaterialShape.querySelector('.materialShapeLabel')?.textContent.trim();
                    const lblMatlDimW = lblMaterialDimW.querySelector('.materialDimWLabel')?.textContent.trim();
                    const lblMatlOther = lblMaterialOther.querySelector('.materialOtherLabel')?.textContent.trim();
                    const lblMatlLength = lblMaterialLength.querySelector('.materiallengthLabel')?.textContent.trim();
                    const lblMatlCondDescription = lblCondDescription.querySelector('.materialCondiLabel')?.textContent.trim();
                    const lblMatlTypeCbk = lblMaterialTypeCbk.querySelector('.materialtypeLabel')?.textContent.trim();
                    const lblMatlSplKubun = lblMaterialSplKubun.querySelector('.materialKubunLabel')?.textContent.trim();
                    const textQty = textboxQty.querySelector('input[type="text"]'); // Get the <input type="text"> element inside the <td>
                    const textRollYear = textboxRollYear.querySelector('input[type="text"]');
                    const textRoll = textboxRoll.querySelector('input[type="text"]');
                    const textRollChance = textboxRollChance.querySelector('input[type="text"]');
                    const textRemark = textboxRemark.querySelector('input[type="text"]');
                    const textMukesakiRate = textboxMukesakiRate.querySelector('input[type="text"]');
                    const textMatlNameMakerCd = textboxMatlNameMakCd.querySelector('input[type="text"]');
                    const textMatlKubun = textboxMatlKubun.querySelector('input[type="text"]');

                    if (textQty || textRollYear || textRoll || textRollChance || textRemark || lblMatlName
                        || txtRawMatl || lblMatlDimD || textMatlShape || lblMatlDimW || lblMatlOther || lblMatlLength || textMukesakiRate || textMatlNameMakerCd || textMatlKubun
                        || lblMatlCondDescription || lblMatlTypeCbk || lblMatlSplKubun) {
                        allRowMaterialData.MAKER_NAME = lblMatlName || '';
                        allRowMaterialData.MATL_MAKER_NAME = txtRawMatl.value.trim() || '';
                        allRowMaterialData.MATL_DIM_D = lblMatlDimD || '';
                        allRowMaterialData.MATL_SHAPE = lblMatlShape || '';
                        allRowMaterialData.MATL_DIM_W = lblMatlDimW || '';
                        allRowMaterialData.MATL_DIM_OTHER = lblMatlOther || '';
                        allRowMaterialData.MATERIAL_LENGTH = lblMatlLength || '';
                        allRowMaterialData.CONDITION_DESCRIPTION = lblMatlCondDescription || '';
                        allRowMaterialData.MATERIAL_TYPE_NAME_CBK = lblMatlTypeCbk || '';
                        allRowMaterialData.MATL_SPECIAL_KUBUN = lblMatlSplKubun || '';
                        allRowMaterialData.QTY = textQty.value.trim() || ''; // Get the value of the textbox
                        allRowMaterialData.ROLL_YEAR = textRollYear.value.trim() || '';
                        allRowMaterialData.ROLL_R = textRoll.value.trim() || '';
                        allRowMaterialData.ROLL_CHANCE = textRollChance.value.trim() || '';
                        allRowMaterialData.RATE = textMukesakiRate.value.trim() || null;
                        allRowMaterialData.MATL_NAME_MAKER_CD = textMatlNameMakerCd.value.trim() || null;
                        allRowMaterialData.MATL_KUBUN = textMatlKubun.value.trim() || null;
                        /*allRowMaterialData.RATE = row.cells[22]?.innerText.trim() || '';*/
                        allRowMaterialData.REMARKS = textRemark.value.trim() || '';
                    }
                }
                allRowMaterialData.MATERIAL_CD = row.cells[22]?.innerText.trim() || '';
                //allRowMaterialData.RATE = row.cells[22]?.innerText.trim() || '';
                allRowMaterialData.USER_ID = $("#hdfLoginId").val();

                materialDataList.push(allRowMaterialData);
            });
        }
        materialMukesakiCdList = [];
        if (materialDataList.length > 0) {
            for (var i = 1; materialDataList.length>=i; i++) {
                materialMukesakiCdList.push(materialDataList[i-1].MATERIAL_CD + "_" + materialDataList[i-1].MUKESAKI_CD);

            }
           
        }
        var settings1 = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/RollPoMainAnalysis/GetMaterialMasterDataView",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(materialDataList)
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings1)
            .done(function (response) {
                var result = response.message;
                $("#pnlMaterialPOGrid").html(response.message);
                AddNewRowEndOfMaterialGrid();
                gridRows = materialRows.length + 1;
                SetSupplier(gridRows);
                SetDestination(gridRows);
                SetHikitorisaki(gridRows);
                SetFutou(gridRows);
                var gridRowsCnt = materialDataList.length;
                for (var i = 1; i <= gridRowsCnt; i++) {
                    var mukesakiCd = materialDataList[i - 1].MUKESAKI_CD;

                    if (mukesakiCd != null) {
                        // Bind dropdown options per row
                        SetMukesakiDropdown($("#ddlMukesaki" + i), mukesakiCd);
                    }
                    else {
                        SetMukesakiDropdown($("#ddlMukesaki" + i), "0");
                    }

                    // Set selected value
                    $("#ddlMukesaki" + i).val(mukesakiCd);

                    //$("#ddlMukesaki" + (i)).val(materialDataList[i - 1].MUKESAKI_CD);

                    var supplierCd = materialDataList[i - 1].SUPPLIER_CD;
                    if (supplierCd != null) {
                        // Bind dropdown options per row
                        SetSupplierDropdown($("#ddlSupplier" + i), supplierCd);
                    }
                    else {
                        SetSupplierDropdown($("#ddlSupplier" + i), "0");
                    }
                    // Bind dropdown options per row


                    // Set selected value
                    $("#ddlSupplier" + i).val(supplierCd);
                    var mochikomisakiCd = materialDataList[i - 1].MOCHIKOMISAKI_CD;
                    // Bind dropdown options per row
                    if (mochikomisakiCd != null) {
                        // Bind dropdown options per row
                        SetMochikomisakiDropdown($("#ddlDestination" + i), mochikomisakiCd);
                    }
                    else {
                        SetMochikomisakiDropdown($("#ddlDestination" + i), "0");
                    }


                    // Set selected value
                    $("#ddlDestination" + i).val(mochikomisakiCd);
                    $("#ddlFutou" + (i)).val(materialDataList[i - 1].FUTOU_CD);
                    $("#ddlHikitorisaki" + (i)).val(materialDataList[i - 1].HTSAKI_CD);

                }
                $(".materialqty").keyup(function (event) {
                    RestrictToValidDecimalFormat($(this), 5, 2);
                })
            })

            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_system_exception").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }



}
function GetInternalNameWIseMaterialDetails(fcSummaryNo) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetInternalNameWIseMaterialDetails?INT_MATL_FC_SUMMARY_CD=" + fcSummaryNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            const checkbox = document.querySelector('.large-checkbox');
            $("#txtInternalName").val(result.internaL_NAME_STRING);
            $("#txtDia").val(result.diM_D);
            $("#txtShape").val(result.shape);
            $("#txtWidth").val(result.diM_W);
            $("#txtAuxilary").val(result.diM_OTHER);
            $("#txtLength").val(result.length);
            $("#txtCondition").val(result.conditioN_DESCRIPTION);
            $("#txtCoilBar").val(result.matL_KUBUN_NAME);
            $("#hdfMatlKubunType").val(result.matL_KUBUN);
            $("#txtGenSpecName").val(result.matL_SPECIAL_KUBUN_NAME);
            $("#hdfGenSpecType").val(result.matL_SPECIAL_KUBUN);
            if (result.pO_REVIEWED_DATE != null) {
                checkbox.checked = true;
            }
            else {
                checkbox.checked = false;
            }

        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetInternalNameWiseMakerList(fcSummaryNo) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetInternalNameWiseMakerList?INT_MATL_FC_SUMMARY_CD=" + fcSummaryNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/GetInternalNameWiseMakerList",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {

                    var result = response1.message;
                    $("#pnlMakerListGrid").html(response1.message);
                    if (poCompleteFlag == true) {
                        const table = document.getElementById('tblMakerGrid');
                        const elements = table.querySelectorAll('input, select, textarea, button');
                        elements.forEach(el => el.disabled = true);

                    }
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function OnClickMaterialMakerList($callFrom) {
    debugger;
    $callFrom.data('requestRunning', true);
    MaterialCD = $.trim($callFrom.closest("tr").attr("data-lblMaterialCD"));
    MukesakiCD = $.trim($callFrom.closest("tr").attr("data-lblMukesakiCD"));

    materialMukesakiCdList.push(MaterialCD + "_" + MukesakiCD);
    var duplicateMaterialMukesakiCdList = materialMukesakiCdList.filter((currentValue, currentIndex) =>
        materialMukesakiCdList.indexOf(currentValue) !== currentIndex);
    let index = materialMukesakiCdList.indexOf(duplicateMaterialMukesakiCdList);
    if (duplicateMaterialMukesakiCdList != "") {
        materialMukesakiCdList.splice(index);
        duplicateMaterialMukesakiCdList = [];
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgRecordAlreadySelected").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }
    else {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/AnalysePOQuantity/GetMakerMaterialDetailsByCD?INT_MATL_FC_SUMMARY_CD=" + fcSummaryNo + "&MATERIAL_CD=" + MaterialCD + "&MUKESAKI_CD=" + MukesakiCD,
            method: "POST",
            contentType: "application/json; charset=utf-8",
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                //materialMukesakiCdList.push(response.message.materiaL_CD.toString());
                //materialMukesakiCdList.push(response.message.mukesakI_CD.toString());

                var mukesaki_Cd;
                var supplier_Cd;
                var mochikomisaki_CD;
                var futou_CD;
                var htsaki_CD;
                const materialRows = document.querySelectorAll('.MaterialGridRow');
                var materialDataList = [];
                var allRowMaterialData = {};

                //get existing row data from grid
                if (materialRows.length >= 1 && dataBindFlag == true) {
                    materialRows.forEach(row => {
                        allRowMaterialData = {};
                        const lblMateriallName = row.cells[1];
                        const txtBoxRawMatl = row.cells[2];
                        const lblMaterialDimD = row.cells[3];
                        const labMaterialShape = row.cells[4];
                        const lblMaterialDimW = row.cells[5];

                        const lblMaterialOther = row.cells[6];
                        const lblMaterialLength = row.cells[7];
                        const lblCondDescription = row.cells[8];
                        const lblMaterialTypeCbk = row.cells[9];
                        const lblMaterialSplKubun = row.cells[10];
                        const mukesakiDropdownCell = row.cells[11];
                        const textboxMukesakiRate = row.cells[12];
                        const textboxQty = row.cells[13];
                        const textboxRollYear = row.cells[14];
                        const textboxRoll = row.cells[15];
                        const textboxRollChance = row.cells[16];
                        const supplierDropdownCell = row.cells[17];
                        const mochikomisakiDropdownCell = row.cells[18];
                        const futouDropdownCell = row.cells[19];
                        const hikitorisakiDropdownCell = row.cells[20];
                        const textboxRemark = row.cells[21];
                        const textboxMatlNameMakCd = row.cells[23];
                        const textboxMatlKubun = row.cells[24];

                        //get value from grid dropdown
                        if (mukesakiDropdownCell || supplierDropdownCell || mochikomisakiDropdownCell || futouDropdownCell || hikitorisakiDropdownCell) {

                            const mukesakiDropdown = mukesakiDropdownCell.querySelector('select');
                            const supplierDropdown = supplierDropdownCell.querySelector('select');
                            const mochikomisakiDropdown = mochikomisakiDropdownCell.querySelector('select');
                            const futouDropdown = futouDropdownCell.querySelector('select');
                            const hikitorisakiDropdown = hikitorisakiDropdownCell.querySelector('select'); // Get the <select> element inside the <td>

                            if (mukesakiDropdown || supplierDropdown || mochikomisakiDropdown || futouDropdown || hikitorisakiDropdown) {
                                allRowMaterialData.MUKESAKI_CD = mukesakiDropdown.value.trim() || ''; // Get the selected value of the dropdown
                                allRowMaterialData.SUPPLIER_CD = supplierDropdown.value.trim() || '';
                                allRowMaterialData.MOCHIKOMISAKI_CD = mochikomisakiDropdown.value.trim() || '';
                                allRowMaterialData.FUTOU_CD = futouDropdown.value.trim() || '';
                                allRowMaterialData.HTSAKI_CD = hikitorisakiDropdown.value.trim() || '';
                            }
                        }
                        //get value from grid textbox
                        if (textboxQty || textboxRollYear || textboxRoll || textboxRollChance || textboxRemark
                            || lblMateriallName || txtBoxRawMatl || lblMaterialDimD || labMaterialShape
                            || lblMaterialDimW || lblMaterialOther || lblMaterialLength || lblCondDescription || lblMaterialTypeCbk || lblMaterialSplKubun || textboxMukesakiRate || textboxMatlNameMakCd || textboxMatlKubun) {
                            const lblMatlName = lblMateriallName.querySelector('.materialNameLabel')?.textContent.trim();
                            const txtRawMatl = txtBoxRawMatl.querySelector('input[type="text"]');
                            const lblMatlDimD = lblMaterialDimD.querySelector('.materialDimDLabel')?.textContent.trim();
                            const lblMatlShape = labMaterialShape.querySelector('.materialShapeLabel')?.textContent.trim();
                            const lblMatlDimW = lblMaterialDimW.querySelector('.materialDimWLabel')?.textContent.trim();
                            const lblMatlOther = lblMaterialOther.querySelector('.materialOtherLabel')?.textContent.trim();
                            const lblMatlLength = lblMaterialLength.querySelector('.materiallengthLabel')?.textContent.trim();
                            const lblMatlCondDescription = lblCondDescription.querySelector('.materialCondiLabel')?.textContent.trim();
                            const lblMatlTypeCbk = lblMaterialTypeCbk.querySelector('.materialtypeLabel')?.textContent.trim();
                            const lblMatlSplKubun = lblMaterialSplKubun.querySelector('.materialKubunLabel')?.textContent.trim();
                            const textQty = textboxQty.querySelector('input[type="text"]'); // Get the <input type="text"> element inside the <td>
                            const textRollYear = textboxRollYear.querySelector('input[type="text"]');
                            const textRoll = textboxRoll.querySelector('input[type="text"]');
                            const textRollChance = textboxRollChance.querySelector('input[type="text"]');
                            const textRemark = textboxRemark.querySelector('input[type="text"]');
                            const textMukesakiRate = textboxMukesakiRate.querySelector('input[type="text"]');
                            const textMatlNameMakerCd = textboxMatlNameMakCd.querySelector('input[type="text"]');
                            const textMatlKubun = textboxMatlKubun.querySelector('input[type="text"]');

                            if (textQty || textRollYear || textRoll || textRollChance || textRemark || lblMatlName
                                || txtRawMatl || lblMatlDimD || textMatlShape || lblMatlDimW || lblMatlOther || lblMatlLength || textMukesakiRate || textMatlNameMakerCd || textMatlKubun
                                || lblMatlCondDescription || lblMatlTypeCbk || lblMatlSplKubun) {
                                allRowMaterialData.MAKER_NAME = lblMatlName || '';
                                allRowMaterialData.MATL_MAKER_NAME = txtRawMatl.value.trim() || '';
                                allRowMaterialData.MATL_DIM_D = lblMatlDimD || '';
                                allRowMaterialData.MATL_SHAPE = lblMatlShape || '';
                                allRowMaterialData.MATL_DIM_W = lblMatlDimW || '';
                                allRowMaterialData.MATL_DIM_OTHER = lblMatlOther || '';
                                allRowMaterialData.MATERIAL_LENGTH = lblMatlLength || '';
                                allRowMaterialData.CONDITION_DESCRIPTION = lblMatlCondDescription || '';
                                allRowMaterialData.MATERIAL_TYPE_NAME_CBK = lblMatlTypeCbk || '';
                                allRowMaterialData.MATL_SPECIAL_KUBUN = lblMatlSplKubun || '';
                                allRowMaterialData.QTY = textQty.value.trim() || ''; // Get the value of the textbox
                                allRowMaterialData.ROLL_YEAR = textRollYear.value.trim() || '';
                                allRowMaterialData.ROLL_R = textRoll.value.trim() || '';
                                allRowMaterialData.ROLL_CHANCE = textRollChance.value.trim() || '';
                                allRowMaterialData.RATE = textMukesakiRate.value.trim() || null;
                                allRowMaterialData.MATL_NAME_MAKER_CD = textMatlNameMakerCd.value.trim() || null;
                                allRowMaterialData.MATL_KUBUN = textMatlKubun.value.trim() || null;
                                /*allRowMaterialData.RATE = row.cells[22]?.innerText.trim() || '';*/
                                allRowMaterialData.REMARKS = textRemark.value.trim() || '';
                            }
                        }
                        allRowMaterialData.MATERIAL_CD = row.cells[22]?.innerText.trim() || '';
                        //allRowMaterialData.RATE = row.cells[22]?.innerText.trim() || '';
                        allRowMaterialData.USER_ID = $("#hdfLoginId").val();

                        materialDataList.push(allRowMaterialData);
                    });
                }
                //get data from api responce
                if (response.message != "") {

                    allRowMaterialData = {};
                    dataBindFlag = true;
                    allRowMaterialData.MAKER_NAME = response.message.makeR_NAME;
                    allRowMaterialData.MATL_MAKER_NAME = response.message.matL_MAKER_NAME;
                    allRowMaterialData.MATL_DIM_D = response.message.matL_DIM_D;
                    allRowMaterialData.MATL_SHAPE = response.message.matL_SHAPE;
                    allRowMaterialData.MATL_DIM_W = response.message.matL_DIM_W;
                    allRowMaterialData.MATL_DIM_OTHER = response.message.matL_DIM_OTHER;
                    allRowMaterialData.MATERIAL_LENGTH = response.message.materiaL_LENGTH;
                    allRowMaterialData.CONDITION_DESCRIPTION = response.message.conditioN_DESCRIPTION;
                    allRowMaterialData.MATERIAL_TYPE_NAME_CBK = response.message.materiaL_TYPE_NAME_CBK;
                    allRowMaterialData.MATL_SPECIAL_KUBUN = response.message.matL_SPECIAL_KUBUN;
                    allRowMaterialData.MUKESAKI_CD = response.message.mukesakI_CD;
                    allRowMaterialData.SUPPLIER_CD = response.message.supplieR_CD;
                    allRowMaterialData.MOCHIKOMISAKI_CD = response.message.mochikomisakI_CD;
                    allRowMaterialData.FUTOU_CD = response.message.futoU_CD;
                    //allRowMaterialData.HTSAKI_CD = response.message.htsakI_CD;
                    allRowMaterialData.ROLL_YEAR = response.message.rolL_YEAR;
                    allRowMaterialData.ROLL_R = response.message.rolL_R;
                    allRowMaterialData.MATERIAL_CD = response.message.materiaL_CD;
                    allRowMaterialData.MATL_NAME_MAKER_CD = response.message.matL_NAME_MAKER_CD;
                    allRowMaterialData.MATL_KUBUN = response.message.matL_KUBUN;
                    allRowMaterialData.RATE = response.message.rate;
                    allRowMaterialData.USER_ID = $("#hdfLoginId").val();
                    materialDataList.push(allRowMaterialData);

                }


                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoMainAnalysis/GetMaterialMasterDataView",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(materialDataList)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response) {
                        var result = response.message;
                        $("#pnlMaterialPOGrid").html(response.message);
                        AddNewRowEndOfMaterialGrid();
                        gridRows = materialRows.length + 1;
                        SetSupplier(gridRows);
                        SetDestination(gridRows);
                        SetHikitorisaki(gridRows);
                        SetFutou(gridRows);
                        var gridRowsCnt = materialDataList.length;
                        for (var i = 1; i <= gridRowsCnt; i++) {
                            var mukesakiCd = materialDataList[i - 1].MUKESAKI_CD;

                            if (mukesakiCd != null) {
                                // Bind dropdown options per row
                                SetMukesakiDropdown($("#ddlMukesaki" + i), mukesakiCd);
                            }
                            else {
                                SetMukesakiDropdown($("#ddlMukesaki" + i), "0");
                            }

                            // Set selected value
                            $("#ddlMukesaki" + i).val(mukesakiCd);

                            //$("#ddlMukesaki" + (i)).val(materialDataList[i - 1].MUKESAKI_CD);

                            var supplierCd = materialDataList[i - 1].SUPPLIER_CD;
                            if (supplierCd != null) {
                                // Bind dropdown options per row
                                SetSupplierDropdown($("#ddlSupplier" + i), supplierCd);
                            }
                            else {
                                SetSupplierDropdown($("#ddlSupplier" + i), "0");
                            }
                            // Bind dropdown options per row


                            // Set selected value
                            $("#ddlSupplier" + i).val(supplierCd);
                            var mochikomisakiCd = materialDataList[i - 1].MOCHIKOMISAKI_CD;
                            // Bind dropdown options per row
                            if (mochikomisakiCd != null) {
                                // Bind dropdown options per row
                                SetMochikomisakiDropdown($("#ddlDestination" + i), mochikomisakiCd);
                            }
                            else {
                                SetMochikomisakiDropdown($("#ddlDestination" + i), "0");
                            }


                            // Set selected value
                            $("#ddlDestination" + i).val(mochikomisakiCd);
                            $("#ddlFutou" + (i)).val(materialDataList[i - 1].FUTOU_CD);
                            $("#ddlHikitorisaki" + (i)).val(materialDataList[i - 1].HTSAKI_CD);

                        }
                        $(".materialqty").keyup(function (event) {
                            RestrictToValidDecimalFormat($(this), 5, 2);
                        })
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
    }
}
function getRoll() {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/GetForecastRoll",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult != null) {
                current_Roll = jsonResult.rolL_R;
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function SetGridColumnHeadingRValues() {

    var currentR = current_Roll;

    var resultRoll = calLastForecastRollHeaders(currentR);
    $("#lblRMonth3").text($("#lblRMonth3").text().replace("xx", resultRoll.forecastLastRoll_R1).replace("nn", resultRoll.forecastRoll_M1));
    $("#lblRMonth2").text($("#lblRMonth2").text().replace("xx", resultRoll.forecastLastRoll_R2).replace("nn", resultRoll.forecastRoll_M2));
    $("#lblRMonth1").text($("#lblRMonth1").text().replace("xx", resultRoll.forecastLastRoll_R3).replace("nn", resultRoll.forecastRoll_M3));
    $("#lblRMonth31").text($("#lblRMonth31").text().replace("xx", resultRoll.forecastLastRoll_R1).replace("nn", resultRoll.forecastRoll_M1));
    $("#lblRMonth21").text($("#lblRMonth21").text().replace("xx", resultRoll.forecastLastRoll_R2).replace("nn", resultRoll.forecastRoll_M2));
    $("#lblRMonth11").text($("#lblRMonth11").text().replace("xx", resultRoll.forecastLastRoll_R3).replace("nn", resultRoll.forecastRoll_M3));

    var resultNextRoll = calNextForecastRollHeaders(currentR);

    $("#lblNextR5").text($("#lblNextR5").text().replace("xx", resultNextRoll.forecastNextRoll_R1).replace("nn", resultNextRoll.forecastNextRoll_M1));
    $("#lblNextR4").text($("#lblNextR4").text().replace("xx", resultNextRoll.forecastNextRoll_R2).replace("nn", resultNextRoll.forecastNextRoll_M2));
    $("#lblNextR3").text($("#lblNextR3").text().replace("xx", resultNextRoll.forecastNextRoll_R3).replace("nn", resultNextRoll.forecastNextRoll_M3));
    $("#lblNextR2").text($("#lblNextR2").text().replace("xx", resultNextRoll.forecastNextRoll_R4).replace("nn", resultNextRoll.forecastNextRoll_M4));
    $("#lblNextR1").text($("#lblNextR1").text().replace("xx", resultNextRoll.forecastNextRoll_R5).replace("nn", resultNextRoll.forecastNextRoll_M5));
    $("#lblNextR51").html($("#lblNextR51").html().replace("xx", resultNextRoll.forecastNextRoll_R1).replace("nn", resultNextRoll.forecastNextRoll_M1));
    $("#lblNextR41").html($("#lblNextR41").html().replace("xx", resultNextRoll.forecastNextRoll_R2).replace("nn", resultNextRoll.forecastNextRoll_M2));
    $("#lblNextR31").html($("#lblNextR31").html().replace("xx", resultNextRoll.forecastNextRoll_R3).replace("nn", resultNextRoll.forecastNextRoll_M3));
}


function calLastForecastRollHeaders(currentR) {


    var returnLastRoll = {};

    var date = new Date();
    var baseMonth1 = new Date(date.getFullYear(), currentR - 1, 1);  // currentR-1 since month is 0-indexed in JS

    // Calculate R1, R2, and R3 with adjustments for 6, 5, and 4 months respectively.
    var R1 = new Date(baseMonth1);
    R1.setMonth(baseMonth1.getMonth() - 6);

    var R2 = new Date(baseMonth1);
    R2.setMonth(baseMonth1.getMonth() - 5);

    var R3 = new Date(baseMonth1);
    R3.setMonth(baseMonth1.getMonth() - 4);

    // Get forecast for R1, R2, R3 - making sure we handle the wrapping of months beyond 12
    returnLastRoll["forecastLastRoll_R1"] = adjustMonth(R1);
    returnLastRoll["forecastLastRoll_R2"] = adjustMonth(R2);
    returnLastRoll["forecastLastRoll_R3"] = adjustMonth(R3);

    // Forecast for R1, R2, R3 plus 3 months - this may roll over into next year.
    returnLastRoll["forecastRoll_M1"] = adjustMonth(R1, 2);
    returnLastRoll["forecastRoll_M2"] = adjustMonth(R2, 2);
    returnLastRoll["forecastRoll_M3"] = adjustMonth(R3, 2);

    return returnLastRoll;
}

function adjustMonth(date, monthsToAdd = 0) {

    let originalMonth = date.getMonth(); // Get the current month (0-indexed)
    let adjustedMonth = (originalMonth + monthsToAdd) % 12; // Handle overflow of months
    let yearAdjustment = Math.floor((originalMonth + monthsToAdd) / 12); // Adjust the year if needed
    let newYear = date.getFullYear() + yearAdjustment; // Adjust the year based on overflow
    return adjustedMonth + 1; // Convert from 0-indexed to 1-indexed month
}

function calNextForecastRollHeaders(currentR) {


    var returnNextRoll = {};

    var date = new Date();
    var baseMonth1 = new Date(date.getFullYear(), currentR - 1, 1);

    var R1 = new Date(baseMonth1);
    R1.setMonth(baseMonth1.getMonth() - 3);

    var R2 = new Date(baseMonth1);
    R2.setMonth(baseMonth1.getMonth() - 2);

    var R3 = new Date(baseMonth1);
    R3.setMonth(baseMonth1.getMonth() - 1);

    var R4 = new Date(baseMonth1);
    R4.setMonth(baseMonth1.getMonth() - 0);

    var R5 = new Date(baseMonth1);
    R5.setMonth(baseMonth1.getMonth() + 1);

    returnNextRoll["forecastNextRoll_R1"] = adjustMonth(R1);
    returnNextRoll["forecastNextRoll_R2"] = adjustMonth(R2);
    returnNextRoll["forecastNextRoll_R3"] = adjustMonth(R3);
    returnNextRoll["forecastNextRoll_R4"] = adjustMonth(R4);
    returnNextRoll["forecastNextRoll_R5"] = adjustMonth(R5);


    returnNextRoll["forecastNextRoll_M1"] = adjustMonth(R1, 2);
    returnNextRoll["forecastNextRoll_M2"] = adjustMonth(R2, 2);
    returnNextRoll["forecastNextRoll_M3"] = adjustMonth(R3, 2);
    returnNextRoll["forecastNextRoll_M4"] = adjustMonth(R4, 2);
    returnNextRoll["forecastNextRoll_M5"] = adjustMonth(R5, 2);

    return returnNextRoll;
}
function GetRollPoDetails(fcSummaryNo) {
    const materialRows = document.querySelectorAll('.MaterialGridRow');
    var materialDataList = [];
    var allRowMaterialData = {};
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetRollPoDetails?INT_MATL_FC_SUMMARY_CD=" + fcSummaryNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var poDetailsCnt = response.message.length;

            for (var i = 0; i <= poDetailsCnt - 1; i++) {
                dataBindFlag = true;
                allRowMaterialData = {};
                materialMukesakiCdList.push(response.message[i].materiaL_CD.toString() + "_" + response.message[i].mukesakI_CD.toString());
                allRowMaterialData.MAKER_NAME = response.message[i].makeR_NAME;
                allRowMaterialData.MATL_MAKER_NAME = response.message[i].matL_MAKER_NAME;
                allRowMaterialData.MATL_DIM_D = response.message[i].matL_DIM_D;
                allRowMaterialData.MATL_SHAPE = response.message[i].matL_SHAPE;
                allRowMaterialData.MATL_DIM_W = response.message[i].matL_DIM_W;
                allRowMaterialData.MATL_DIM_OTHER = response.message[i].matL_DIM_OTHER;
                allRowMaterialData.MATERIAL_LENGTH = response.message[i].materiaL_LENGTH;
                allRowMaterialData.CONDITION_DESCRIPTION = response.message[i].conditioN_DESCRIPTION;
                allRowMaterialData.MATERIAL_TYPE_NAME_CBK = response.message[i].materiaL_TYPE_NAME_CBK;
                allRowMaterialData.MATL_SPECIAL_KUBUN = response.message[i].matL_SPECIAL_KUBUN_NAME;
                allRowMaterialData.MUKESAKI_CD = response.message[i].mukesakI_CD;
                allRowMaterialData.SUPPLIER_CD = response.message[i].supplieR_CD;
                allRowMaterialData.MOCHIKOMISAKI_CD = response.message[i].mochikomisakI_CD;
                allRowMaterialData.FUTOU_CD = response.message[i].futoU_CD;
                allRowMaterialData.HTSAKI_CD = response.message[i].htsakI_CD;
                allRowMaterialData.QTY = response.message[i].qty;
                allRowMaterialData.ROLL_YEAR = response.message[i].rolL_YEAR;
                allRowMaterialData.ROLL_R = response.message[i].rolL_R;
                allRowMaterialData.ROLL_CHANCE = response.message[i].rolL_CHANCE;
                allRowMaterialData.REMARKS = response.message[i].remarks;
                allRowMaterialData.MATERIAL_CD = response.message[i].materiaL_CD;
                allRowMaterialData.MATL_NAME_MAKER_CD = response.message[i].matL_NAME_MAKER_CD;
                allRowMaterialData.MATL_KUBUN = response.message[i].matL_KUBUN;
                allRowMaterialData.RATE = response.message[i].rate;
                allRowMaterialData.USER_ID = $("#hdfLoginId").val();
                materialDataList.push(allRowMaterialData);

            }
            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/GetMaterialMasterDataView",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(materialDataList)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response) {
                    var result = response.message;
                    $("#pnlMaterialPOGrid").html(response.message);
                    if (materialDataList.length != 0) {
                        AddNewRowEndOfMaterialGrid();
                        if (poCompleteFlag == true) {
                            const table = document.getElementById('tblMaterialPoGrid');
                            const elements = table.querySelectorAll('input, select, textarea, button');
                            elements.forEach(el => el.disabled = true);

                        }
                    }

                    gridRows = materialRows.length + 1;
                    var gridRowsCnt = materialDataList.length;
                    //SetMukesaki(gridRowsCnt);
                    SetSupplier(gridRowsCnt);
                    SetDestination(gridRowsCnt);
                    SetHikitorisaki(gridRowsCnt);
                    SetFutou(gridRowsCnt);

                    for (var i = 1; i <= gridRowsCnt; i++) {
                        var mukesakiCd = materialDataList[i - 1].MUKESAKI_CD;

                        if (mukesakiCd != null) {
                            // Bind dropdown options per row
                            SetMukesakiDropdown($("#ddlMukesaki" + i), mukesakiCd);
                        }
                        else {
                            SetMukesakiDropdown($("#ddlMukesaki" + i), "0");
                        }

                        // Set selected value
                        $("#ddlMukesaki" + i).val(mukesakiCd);

                        //$("#ddlMukesaki" + (i)).val(materialDataList[i - 1].MUKESAKI_CD);

                        var supplierCd = materialDataList[i - 1].SUPPLIER_CD;
                        if (supplierCd != null) {
                            // Bind dropdown options per row
                            SetSupplierDropdown($("#ddlSupplier" + i), supplierCd);
                        }
                        else {
                            SetSupplierDropdown($("#ddlSupplier" + i), "0");
                        }
                        // Bind dropdown options per row


                        // Set selected value
                        $("#ddlSupplier" + i).val(supplierCd);
                        var mochikomisakiCd = materialDataList[i - 1].MOCHIKOMISAKI_CD;
                        // Bind dropdown options per row
                        if (mochikomisakiCd != null) {
                            // Bind dropdown options per row
                            SetMochikomisakiDropdown($("#ddlDestination" + i), mochikomisakiCd);
                        }
                        else {
                            SetMochikomisakiDropdown($("#ddlDestination" + i), "0");
                        }


                        // Set selected value
                        $("#ddlDestination" + i).val(mochikomisakiCd);
                        $("#ddlFutou" + (i)).val(materialDataList[i - 1].FUTOU_CD);
                        $("#ddlHikitorisaki" + (i)).val(materialDataList[i - 1].HTSAKI_CD);

                    }

                    $(".materialqty").keyup(function (event) {
                        RestrictToValidDecimalFormat($(this), 5, 2);
                    });
                })

                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}





$(document).on("change", "select[id^='ddlMukesaki']", function () {
    debugger;
    var $this = $(this); // the select element
    var $row = $this.closest('tr');
    var id = this.id; // ddlMukesaki0
    var rowIndex = id.replace("ddlMukesaki", "");
    var mukesakiCd = $this.val();
    var matlDimD = $("#lblMatlDimD" + rowIndex).text().trim();
    var matlKubun = $("#hdfMatlKubun" + rowIndex).val();
    var matlNameMakCd = $("#hdfMatlNameMakerCd" + rowIndex).val();

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/AnalysePOQuantity/GetMukesakiMaterialRate?mukesakiCd=" + mukesakiCd + "&matlDimD=" + matlDimD + "&matlKubun=" + matlKubun + "&matlNameMakerCd=" + matlNameMakCd,
        contentType: "application/json; charset=utf-8"
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var Response = response.message;
            if (Response != 0) {
                $("#txtRate" + rowIndex).val(Response);
            }
            else {
                $("#txtRate" + rowIndex).val("");
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Tensile_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });



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


