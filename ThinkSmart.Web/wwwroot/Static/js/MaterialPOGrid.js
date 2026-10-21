//var WARNING = "warning";
//var ERROR = "error";
//var SUCCESS = "success";
//let apiResponses = [];
//var MaterialCD;
//var MaterialCdList = [];
//$(document).ready(function () {

//    setMaterialType();
//    setMatlSpecName();
//    setDecimalFormatsOnMaterial();
//    debugger;
//});
//function OnMaterialListBtnClick(editButton) {
    
   
//    $("#MaterialListPopupModal").modal('show');
//    $("#MaterialListPopupModal").addClass("show");
//    var elements = document.getElementsByClassName('');
//    for (var i in elements) {
//        if (elements.hasOwnProperty(i)) {
//            elements[i].className = 'modal-backdrop fade show';
//        }
//    }
//    InitSortPopupOnMaterial();
//    document.getElementById("MaterialListPopupModal").style.display = "block";

//}
//function setDecimalFormatsOnMaterial() {
//    $("#txtDiaMinOnMatl,#txtDiaMaxOnMatl").keyup(function (event) {
//        RestrictToValidDecimalFormat($(this), 3, 3);
//    });
//    $("#txtLengthMinOnMatl,#txtLengthMaxOnMatl").keyup(function (event) {
//        RestrictToValidDecimalFormat($(this), 4, 2);
//    });

//}
//$("#btnSearchMaterial").click(function () {
//    if (!IsMandatoryDateSearchFieldsFilledOnMaterial()) {
//        return false;
//    }
//    var selectedColumns = [];
//    $("#lstSelectedSortFields option").each(function () {
//        var $this = $(this);
//        if ($this.length) {
//            var selText = $this.text().split(' ');
//            var selValue = $this.val();
//            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
//            selectedColumns.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
//        }
//    });
//    GetMaterialMasterDataList(selectedColumns);
//});
//function GetMaterialMasterDataList(sortFields) {
//    var ajaxTime = new Date().getTime();
//    var maker_NAME = $("#txtMakerNameOnMatl").val() ? $("#txtMakerNameOnMatl").val() : null;
//    var internal_Name = $("#txtInternalNameOnMatl").val() ? $("#txtInternalNameOnMatl").val() : null;
//    var material_Name_Maker = $("#txtRawMaterialNameOnMatl").val() ? $("#txtRawMaterialNameOnMatl").val() : null;
//    var dia_Min = $("#txtDiaMinOnMatl").val() ? $("#txtDiaMinOnMatl").val() : null;
//    var dia_Max = $("#txtDiaMaxOnMatl").val() ? $("#txtDiaMaxOnMatl").val() : null;
//    var shape = $("#txtShapeOnMatl").val() ? $("#txtShapeOnMatl").val() : null;
//    var length_Min = $("#txtLengthMinOnMatl").val() ? $("#txtLengthMinOnMatl").val() : null;
//    var length_Max = $("#txtLengthMaxOnMatl").val() ? $("#txtLengthMaxOnMatl").val() : null;
//    var Material_Spl_Name = $("#ddlMatlSpecName").val() ? $("#ddlMatlSpecName").val() : null;
//    var Material_Type_cbk = $("#ddlMaterialType").val() ? $("#ddlMaterialType").val() : null;
//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: APIURL + "/RollPoForecast/GetMaterialMasterData",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",

//        data: JSON.stringify({
//            MAKER_NAME: maker_NAME, INTERNAL_NAME_STRING: internal_Name, MATL_SHAPE: shape, MATL_MAKER_NAME: material_Name_Maker,
//            MATL_DIM_D_MIN: dia_Min, MATL_DIM_D_MAX: dia_Max, MATL_LENGTH_MIN: length_Min, MATL_LENGTH_MAX: length_Max, MATL_SPECIAL_KUBUN: Material_Spl_Name,
//            MATL_TYPE_CBK: Material_Type_cbk, SORT_FIELDS: sortFields
//        })
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var result = response.message;
//            ProductList = response.message;
//            var settings1 = {
//                async: true,
//                crossDomain: true,
//                cache: false,
//                dataType: "json",
//                url: "/RollPoForecast/MaterialMasterListGrid",
//                method: "POST",
//                contentType: "application/json; charset=utf-8",
//                data: JSON.stringify(response.message)
//                //data: "{jsonData : " + jsonData + "}"
//            }
//            $.ajax(settings1)
//                .done(function (response) {

//                    var result = response.message;
//                    $("#divMaterialMasterListGrid").html(response.message);
//                })

//                .fail(function (xhr, textStatus, errorThrown) {
//                    responseTime = new Date().getTime() - ajaxTime;
//                    var errorCode = $.parseJSON(xhr.responseText).Message;

//                    focusFieldID = $(':focus').attr("id");
//                    message = $.trim($("#lblErrorMessage_system_exception").text());
//                    showMessageDialog(message, focusFieldID, ERROR);
//                });
//        })

//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessage_system_exception").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//    closepopup();
//}
//$(document).on('click', '#btnSortOnMaterial', function () {
//    $('#SortPopUpModal').css('margin-left', '-408px');
//    $("#SortPopUpModal").modal('show');

//});
//function closepopup() {

//    $("#SortPopUpModal").modal('hide');
//}
//function btnSortClose_Click() {

//    closepopup();
//}
//function InitSortPopupOnMaterial() {
//    debugger;
//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        url: "/RollPoForecast/InitializeSortPopupForMaterialList",
//        method: "GET",
//        contentType: "application/json; charset=utf-8",
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            $('#lstSortFields').find('option')
//                .remove()
//                .end();
//            $('#lstSelectedSortFields').find('option')
//                .remove()
//                .end();
//            for (var i = 0; i < response.message.length; i++) {
//                if (response.message[i].isDefaultSort) {
//                    $('#lstSelectedSortFields').append($("<option></option>")
//                        .attr("value", response.message[i].fieldName)
//                        .text(response.message[i].displayName + GetSortDirectionSuffix(response.message[i].sortOrder)));
//                }
//                else {
//                    $('#lstSortFields').append($("<option></option>")
//                        .attr("value", response.message[i].fieldName)
//                        .text(response.message[i].displayName));
//                }

//            }
//            EnableDisableSortButtons();
//            $("#rdoSortAsc").prop("checked", true);
//            $("#rdoSortDsc").prop("checked", false);
//        })
//}
//function btnSortOk_Click() {

//    var selectedColumns = [];
//    $("#lstSelectedSortFields option").each(function () {
//        var $this = $(this);
//        if ($this.length) {
//            var selText = $this.text().split(' ');
//            var selValue = $this.val();
//            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
//            selectedColumns.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
//        }
//    });
   
//        GetMaterialMasterDataList(selectedColumns);
   
//}
//function btnSortReset_Click() {
//    InitSortPopupOnMaterial();
//}
//function GetSortDirectionSuffix(sortDirection) {
//    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
//    return " " + sortDirectionText;
//}

//function btnSortAdd_Click() {

//    var selectedColumns = $("#lstSortFields").val();
//    if (selectedColumns.length > 0) {
//        $("#lstSortFields option:selected").each(function () {
//            var $this = $(this);
//            if ($this.length) {
//                var selText = $this.text();
//                console.log(selText);
//                $('#lstSelectedSortFields').append($("<option></option>")
//                    .attr("value", $this.val())
//                    .text(selText + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
//                $this.remove();
//            }
//        });
//    }
//    else {
//        alert("Please select the sort field to be added.");
//    }
//    EnableDisableSortButtons();
//}
//function EnableDisableSortButtons() {
//    var available = $("#lstSortFields option").length;
//    var selected = $("#lstSelectedSortFields option").length;
//    if (available > 0) {
//        $("#SortPopUpModal #btnAdd").prop('disabled', false);
//    }
//    else {
//        $("#SortPopUpModal #btnAdd").prop('disabled', true);
//    }

//    if (selected > 0) {
//        $("#SortPopUpModal #btnRemove").prop('disabled', false);
//    }
//    else {
//        $("#SortPopUpModal #btnRemove").prop('disabled', true);
//    }
//}
//function BindMaterialTypeDropdown(jsonResult) {
//    $.each(jsonResult, function (key, jsonObj) {
//        debugger;
//        $("#ddlMaterialType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
//    });
//}
//function btnSortRemove_Click() {
//    var selectedColumns = $("#lstSelectedSortFields").val();
//    if (selectedColumns.length > 0) {
//        var selectedColumns = $("#lstSelectedSortFields").val();
//        if (selectedColumns.length > 0) {
//            $("#lstSelectedSortFields option:selected").each(function () {
//                var $this = $(this);
//                if ($this.length) {
//                    var selText = $this.text().split(' ');
//                    console.log(selText);
//                    selText.splice(-1);
//                    $('#lstSortFields').append($("<option></option>")
//                        .attr("value", $this.val())
//                        .text(selText.join(' ')));
//                    $this.remove();
//                }
//            });
//        }
//    }
//    else {
//        alert("Please select the sort field to be added.");
//    }
//    EnableDisableSortButtons();
//}

//$("#btnInitializeOnMaterial").click(function () {
//    clearCommonSearchFieldsOnMaterial();
//});
//function clearCommonSearchFieldsOnMaterial() {

//    $('#txtMakerNameOnMatl').val('');
//    $('#txtInternalNameOnMatl').val('');
//    $('#txtRawMaterialNameOnMatl').val('');
//    $('#txtDiaMinOnMatl').val('');
//    $('#txtDiaMaxOnMatl').val('');
//    $('#txtShapeOnMatl').val('');
//    $('#txtLengthMinOnMatl').val('');
//    $('#txtLengthMaxOnMatl').val('');
//    var result = "";
//    var settings1 = {
//        async: true,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: "/RollPoForecast/MaterialMasterListGrid",
//        method: "POST",
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify(result)
//        //data: "{jsonData : " + jsonData + "}"
//    }
//    $.ajax(settings1)
//        .done(function (response) {

//            var result = response.message;
//            $("#divMaterialMasterListGrid").html(response.message);
//        })

//        .fail(function (xhr, textStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.parseJSON(xhr.responseText).Message;

//            focusFieldID = $(':focus').attr("id");
//            message = $.trim($("#lblErrorMessage_system_exception").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });

//}
//function setMaterialType() {
//    var ajaxTime = new Date().getTime();
//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: "/RollPoForecast/GetMaterialTypeList",
//        method: "GET",
//        contentType: "application/json; charset=utf-8",
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            BindMaterialTypeDropdown(response.message);
//        })
//        .fail(function (xhr, testStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}
//function BindMatlSpecNameDropdown(jsonResult) {
//    $.each(jsonResult, function (key, jsonObj) {
//        debugger;
//        $("#ddlMatlSpecName").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
//    });
//}

//function setMatlSpecName() {
//    var ajaxTime = new Date().getTime();
//    var settings = {
//        async: false,
//        crossDomain: true,
//        cache: false,
//        dataType: "json",
//        url: "/RollPoForecast/GetMatlSpecNameList",
//        method: "GET",
//        contentType: "application/json; charset=utf-8",
//    }
//    $.ajax(settings)
//        .done(function (response) {
//            responseTime = new Date().getTime() - ajaxTime;
//            BindMatlSpecNameDropdown(response.message);
//        })
//        .fail(function (xhr, testStatus, errorThrown) {
//            responseTime = new Date().getTime() - ajaxTime;
//            var errorCode = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
//            showMessageDialog(message, focusFieldID, ERROR);
//        });
//}
//function IsMandatoryDateSearchFieldsFilledOnMaterial() {
//    debugger;
//    var returnVal = true;  // default true is required for screens with no mandatory fields

//    var message = ""; // This message will be displayed on dialog box
//    var focusFieldID = "";
//    if ($("#txtDiaMinOnMatl").val().trim() == "" && $("#txtDiaMaxOnMatl").val().trim() != "") {
//        message = $.trim($("#lblErrorMessageE_DimDStartRequired").text());
//        focusFieldID = 'txtDiaMin'
//        returnVal = false;
//    }

//    if ($("#txtLengthMinOnMatl").val().trim() == "" && $("#txtLengthMaxOnMatl").val().trim() != "") {
//        if (focusFieldID == "") {
//            focusFieldID = 'txtLengthMin';
//            returnVal = false;
//        }
//        if (message == "") {
//            message = $.trim($("#lblErrorMessageE_LengthStartRequired").text());
//            returnVal = false;
//        } else {
//            message = message + "<br>" + $.trim($("#lblErrorMessageE_LengthStartRequired").text());
//            returnVal = false;
//        }
//    }
//    if (message !== "") {
//        showMessageDialog(message, focusFieldID, WARNING);
//    }
//    return returnVal;
//}
//function OnMaterialCancelBtnClick() {

//    var element = document.getElementById("MaterialListPopupModal");
//    $("#MaterialListPopupModal").removeClass("show");
//    var test = document.getElementsByClassName("modal-backdrop fade show");

//    var elements = document.getElementsByClassName('modal-backdrop fade show');
//    for (var i in elements) {
//        if (elements.hasOwnProperty(i)) {
//            elements[i].className = '';
//        }
//    }

//    document.getElementById("MaterialListPopupModal").style.display = "none";
//    clearCommonSearchFieldsOnMaterial();
//}
//function OnClickMaterialList($callFrom) {
//    $callFrom.data('requestRunning', true);
//    var ajaxTime = new Date().getTime();
//    MaterialCD = $.trim($callFrom.closest("tr").attr("data-lblMaterialCD"));
//    GetMaterialDetailsByCD(MaterialCD);
//}
//function GetMaterialDetailsByCD(MaterialCD) {
//    MaterialCdList.push(MaterialCD);
//    var duplicateMaterialCdList = MaterialCdList.filter((currentValue, currentIndex) =>
//        MaterialCdList.indexOf(currentValue) !== currentIndex);
//    let index = MaterialCdList.indexOf(duplicateMaterialCdList);
//    if (duplicateMaterialCdList != "") {
//        MaterialCdList.splice(index);
//        duplicateMaterialCdList = [];
//        focusFieldID = $(':focus').attr("id");
//        message = $.trim($("#valMsgRecordAlreadySelected").text());
//        showMessageDialog(message, focusFieldID, WARNING);
//    } else {
//        var ajaxTime = new Date().getTime();
//        var jsonData = "{MATERIAL_CD :'" + MaterialCD + "' }";
//        var settings = {
//            async: false,
//            crossDomain: true,
//            cache: false,
//            dataType: "json",
//            url: APIURL + "/RollPoForecast/GetMaterialDetailsByMaterialCD?MATERIAL_CD=" + MaterialCD,
//            method: "GET",
//            contentType: "application/json; charset=utf-8",
//        }
//        $.ajax(settings)
//            .done(function (response) {
//                responseTime = new Date().getTime() - ajaxTime;

//                var result = response.message;
//                ProductList = response.message;
//                var settings1 = {
//                    async: false,
//                    crossDomain: true,
//                    cache: false,
//                    dataType: "json",
//                    url: "/RollPoMainAnalysis/GetMaterialMasterData",
//                    method: "POST",
//                    contentType: "application/json; charset=utf-8",
//                    data: JSON.stringify(response.message)
//                    //data: "{jsonData : " + jsonData + "}"
//                }
//                $.ajax(settings1)
//                    .done(function (response) {

//                        var result = response.message;
//                        debugger;
                       
//                        $("#pnlMaterialPOGrid").html(response.message);
//                    })

//                    .fail(function (xhr, textStatus, errorThrown) {
//                        responseTime = new Date().getTime() - ajaxTime;
//                        var errorCode = $.parseJSON(xhr.responseText).Message;

//                        focusFieldID = $(':focus').attr("id");
//                        message = $.trim($("#lblErrorMessage_system_exception").text());
//                        showMessageDialog(message, focusFieldID, ERROR);
//                    });
//            })

//            .fail(function (xhr, textStatus, errorThrown) {
//                responseTime = new Date().getTime() - ajaxTime;
//                var errorCode = $.parseJSON(xhr.responseText).Message;

//                focusFieldID = $(':focus').attr("id");
//                message = $.trim($("#lblErrorMessage_system_exception").text());
//                showMessageDialog(message, focusFieldID, ERROR);
//            });
//        OnMaterialCancelBtnClick();
//    }
//}
//function AddNewRowEndOfMaterialGrid() {
//    // Get the table by its ID
//    var table = document.getElementById("tblMaterialPoGrid");
   
//        var newRow = table.insertRow(table.rows.length);
//        newRow.className = "GridNormalRow";
//        // Loop to create 18 cells and add input fields
//        for (var i = 0; i < 21; i++) {
//            var cell = newRow.insertCell(i);

//            // Add specific input type for the second cell (for button)
//            if (i === 1) {
//                cell.className = "labelClass padddingLeft padddingRight";
//                cell.innerHTML = `<input type="button" class="btnArrow" onclick="OnProductListBtnClick(this)" style="height:16px;float: right" />`;
//            }
//            else {
//                // For other cells, add a text input field
//                cell.className = "labelClass ";
//                cell.innerHTML = ``;
//            }
//        }

    
//}
//function showMessageDialog(message, focusFieldID, messageType) {

//    $("#dialogMessageIcon img").hide();
//    switch (messageType) {
//        case WARNING:
//            bootbox.alert({
//                title: "<div style='color:white'></div>",
//                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
//                centerVertical: true,
//                callback: function () {
//                    if (focusFieldID !== "") {
//                        $("#" + focusFieldID).focus();
//                    }
//                }
//            });
//            break;
//        case ERROR:
//            bootbox.alert({
//                title: "<div style='color:white'></div>",
//                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/error.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
//                centerVertical: true,
//                callback: function () {
//                    if (focusFieldID !== "") {
//                        $("#" + focusFieldID).focus();
//                    }
//                }
//            });
//            break;
//        case SUCCESS:
//            bootbox.alert({
//                title: "<div style='color:white'></div>",
//                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
//                centerVertical: true,
//                callback: function () {
//                    if (focusFieldID !== "") {
//                        $("#" + focusFieldID).focus();
//                    }
//                }
//            });
//            break;
//        default:
//            break;
//    };


//}
