var typeDropDown = "select-one";
var DISABLED = "disabled";
var typeText = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var gridRowsCount = 0;
var flagValue = false;
$(document).ready(function () {
    disableCopyButtons();
    disableEditButtons();
    disableDeleteButtons();
    disableUnUseButtons();
    InitSortPopup();
    SetDefaultDate();

    $("#txtVehicleName").focus();
    $("#txtVehicleCodeSearch").focus();

    //var vehicleId = '@ViewBag.VehicleId';

    //if (vehicleId && vehicleId != '0') {
    //    loadVehicleDetails(vehicleId);

    //    enableCopyButtons();
    //    enableEditButtons();
    //    enableDeleteButtons();
    //    enableUnUseButtons();
    //    disableSaveButtons();
    //    enableSelectButtons();
    //}

});

//function disableSearchPanel() {
//    $("#ddlMakerName").attr("disabled", "disabled");
//    $("#txtMukesakiName").attr("disabled", "disabled");
//    $("#btnMukesakiSearch").attr("disabled", "disabled");
//    $("#ddlMukesakiName").attr("disabled", "disabled");
//    $("#ddlMaterialType").attr("disabled", "disabled");
//    $("#txtYear").attr("disabled", "disabled");
//    $("#txtMonth").attr("disabled", "disabled");
//    $("#txtChance").attr("disabled", "disabled");

//}
//function enableSearchPanel() {
//    $("#ddlMakerName").removeAttr("disabled", "disabled");
//    $("#txtMukesakiName").removeAttr("disabled", "disabled");
//    $("#btnMukesakiSearch").removeAttr("disabled", "disabled");
//    $("#ddlMukesakiName").removeAttr("disabled", "disabled");
//    $("#ddlMaterialType").removeAttr("disabled", "disabled");
//    $("#txtYear").removeAttr("disabled", "disabled");
//    $("#txtMonth").removeAttr("disabled", "disabled");
//    $("#txtChance").removeAttr("disabled", "disabled");
//}
function disableEditButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
}
function disableCopyButtons() {
    $("#btnCopy").attr("disabled", "disabled");
}

function disableUnUseButtons() {
    $("#btnUnUse").attr("disabled", "disabled");
}
function disableDeleteButtons() {
    $("#btnDelete").attr("disabled", "disabled");
}
function disableSaveButtons() {
    $("#btnRegister").attr("disabled", "disabled");
}
function disableSelectButtons() {
    $("#btnSelect").attr("disabled", "disabled");
}


function enableEditButtons() {
    $("#btnUpdate").removeAttr("disabled", "disabled");
}
function enableDeleteButtons() {
    $("#btnDelete").removeAttr("disabled", "disabled");
}
function enableSaveButtons() {
    $("#btnRegister").removeAttr("disabled", "disabled");
}
function enableCopyButtons() {
    $("#btnCopy").removeAttr("disabled", "disabled");
}
function enableUnUseButtons() {
    $("#btnUnUse").removeAttr("disabled", "disabled");
}
function enableSelectButtons() {
    $("#btnSelect").removeAttr("disabled", "disabled");
}


function SetDefaultDate() {
    var currentTime = new Date()
    $("#txtYear").val(currentTime.getFullYear());

    var getMonth = currentTime.getMonth() + 1;

    if (getMonth.toString().length == 1) {
        getMonth = "0" + getMonth
    }
    $("#txtMonth").val(getMonth);
    $("#txtChance").val("");

}
function btnInitialize_Click() {
    //var url = document.location.href;
    //window.history.pushState({}, "", url.split("?")[0]);
    Initialize();

}

function Initialize() {

    $("#txtVehicleCode").val('');
    $("#txtVehicleName").val('');
    $("#txtShortName").val('');
    $("#txtYobigana").val('');
    $("#txtYobina").val('');
    $("#txtVehicleNumber").val('');
    $("#txtDriverName").val('');
    $("#txtDisplacement").val('');
    $("#txtVhclMaxLoad").val('');
    $("#txtTelphoneNumber").val('');
    $("#txtVhclTransportCompany").val('');
    $("#txtRemark").val('');
    $("#txtCreatedDate").val('');
    $("#txtUpdatedDate").val('');

    $("#btnUnUse").val($("#lbl_Inactive").text().trim());

    disableCopyButtons();
    disableEditButtons();
    disableDeleteButtons();
    disableUnUseButtons();
    enableSaveButtons();
    InitSortPopup();

    /* FillMaterialGrid(arrayList);*/
}
function btnSelect_Click() {

    window.open('/VehicleMaster/VehicleList', 'VehicleList',
        'height=750,width=1250,resizable=no');


    //win.onload = function () {
    //    win.btnExtract_Click();
    //};

}

function btnExtract_Click(sortFields) {

    var request = {
        VEHICLE_CD: $("#txtVehicleCodeSearch").val()
            ? parseInt($("#txtVehicleCodeSearch").val())
            : null,

        VEHICLE_NAME: $("#txtVehicleNameSearch").val() || null,
        SHORT_NAME: $("#txtShortNameSearch").val() || null,
        YOBIGANA: $("#txtYobiganaSearch").val() || null,
        YOBINA: $("#txtYobinaSearch").val() || null,
        DRIVER_NAME: $("#txtDriverNameSearch").val() || null,

        SORT_FIELDS: sortFields || null
    };

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/VehicleMaster/FetchVehicleDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(request)
    };

    $.ajax(settings)
        .done(function (response) {
            if (response.success) {
                /* $("#divVehicleDetailsGridMain").html(response.message);*/
                FillVehicleGrid(response.message);

            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function FillVehicleGrid(result) {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/VehicleMaster/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(result)
    }
    $.ajax(settings)
        .done(function (response) {

            $("#divVehicleDetailsGridMain").html(response.message);

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


$(document).on('click', '.btnEdit', function () {

    var vehicleId = $(this).data('id');

    /*window.location.href = '/VehicleMaster/VehicleEntryEditDelete?vehicleId=' + vehicleId;*/
    window.open(
        '/VehicleMaster/VehicleEntryEditDelete?vehicleId=' + vehicleId,
        'VehicleEntryEditDelete',
        'height=380,width=900,resizable=yes'
    );
});


function btnUpdate_Click() {

    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }

    bootbox.confirm({
        title: "<div style='color:white'></div>",
        message: "<table width='100%'><tr><td width='70px'><br/>" +
            "<img src='/Images/warning.png' height='50px' width='50px'>" +
            "</td><td vertical-align='top'>" +
            $.trim($("#lbl_ConfMsg_COM_MasterEdit").text()) +
            "</td></tr></table>",

        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },

        callback: function (result) {

            //  If user clicks YES → proceed with update
            if (result) {

                var jsonDataRegisterVehicle = JSON.stringify({
                    VEHICLE_CD: $("#txtVehicleCode").val() ? $("#txtVehicleCode").val() : 0,
                    VEHICLE_NAME: $("#txtVehicleName").val() ? $("#txtVehicleName").val() : null,
                    SHORT_NAME: $("#txtShortName").val() ? $("#txtShortName").val() : null,
                    YOBIGANA: $("#txtYobigana").val() ? $("#txtYobigana").val() : null,
                    YOBINA: $("#txtYobina").val() ? $("#txtYobina").val() : null,
                    VEHICLE_NUMBER: $("#txtVehicleNumber").val() ? $("#txtVehicleNumber").val() : null,
                    DRIVER_NAME: $("#txtDriverName").val() ? $("#txtDriverName").val() : null,
                    DISPLACEMENT: $("#txtDisplacement").val() ? $("#txtDisplacement").val() : null,
                    MAX_LOAD: $("#txtVhclMaxLoad").val() ? $("#txtVhclMaxLoad").val() : null,
                    TELEPHONE_NO: $("#txtTelphoneNumber").val() ? $("#txtTelphoneNumber").val() : null,
                    TRANSPORT_COMPANY_NAME: $("#txtVhclTransportCompany").val() ? $("#txtVhclTransportCompany").val() : null,
                    REMARKS: $("#txtRemark").val() ? $("#txtRemark").val() : null,
                    USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null
                });

                var ajaxTime = new Date().getTime();

                var settings = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: APIURL + "/VehicleMaster/UpdateVehicleDetails",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: jsonDataRegisterVehicle
                };

                $.ajax(settings)
                    .done(function (response) {
                        if (response.success) {

                            loadVehicleDetails(response.vehicleCD);
                            disableSaveButtons();
                            enableCopyButtons();
                            enableUnUseButtons();
                            enableDeleteButtons();
                            enableEditButtons();

                            var successMessage = $("#lblSuccessMessageS_VehicleUpdateEntry_002").text();
                            showMessageDialog(successMessage, "btnInitialize", SUCCESS);
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        var responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        var focusFieldID = $(':focus').attr("id");
                        var message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });

            }
            else {

                $("#txtVehicleName").focus();
            }
        },

        centerVertical: true
    });
}

function btnDelete_Click() {

    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }

    bootbox.confirm({
        title: "<div style='color:white'></div>",
        message: "<table width='100%'><tr><td width='70px'><br/>" +
            "<img src='/Images/warning.png' height='50px' width='50px'>" +
            "</td><td vertical-align='top'>" +
            $.trim($("#lbl_ConfMsg_COM_MasterDelete").text()) +
            "</td></tr></table>",

        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },

        callback: function (result) {

            //  If user clicks YES → proceed with update
            if (result) {

                var ajaxTime = new Date().getTime();
                var settings = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    url: APIURL + "/VehicleMaster/DeleteVehicleDetailsById?vehicleId=" + $("#txtVehicleCode").val(),
                    method: "GET",
                    contentType: "application/json; charset=utf-8",
                    // data: JSON.stringify(deleteData)

                }
                $.ajax(settings)
                    .done(function (response) {
                        responseTime = new Date().getTime() - ajaxTime;
                        disableDeleteButtons();
                        disableEditButtons();
                        disableCopyButtons();
                        disableUnUseButtons();
                        enableSaveButtons()
                        Initialize();
                        var successMessage = $("#lblSuccessMessageS_VehicleDelete").text();
                        showMessageDialog(successMessage, "btnInitialize", SUCCESS);

                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        var response = xhr.responseJSON;
                        focusFieldID = $(':focus').attr("id");
                       
                        //FK_CONSTRAIN
                        if (response.errorNumber == 2292) {
                            message = $.trim($("#lbl_ConfMsg_COM_DeleteNotAllowedAlreadyInuse").text());
                            showMessageDialog(message, focusFieldID, WARNING);
                              
                            return;
                        }


                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });

            }
            
        },

        centerVertical: true
    });
}


function DeleteVehicle() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: APIURL + "/VehicleMaster/DeleteVehicleDetailsById?vehicleId=" + $("#txtVehicleCode").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify(deleteData)

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            disableDeleteButtons();
            disableEditButtons();
            disableCopyButtons();
            disableUnUseButtons();
            enableSaveButtons()
            Initialize();
            var successMessage = $("#lblSuccessMessageS_VehicleDelete").text();
            showMessageDialog(successMessage, "btnInitialize", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function btnRegister_Click() {
    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }


    var ajaxTime = new Date().getDate();
    var jsonDataRegisterVehicle = JSON.stringify({
        /* VEHICLE_CD: $("#txtVehicleCode").val() ? $("#txtVehicleCode").val() : 0,*/
        VEHICLE_NAME: $("#txtVehicleName").val() ? $("#txtVehicleName").val() : null,

        SHORT_NAME: $("#txtShortName").val() ? $("#txtShortName").val() : null,
        YOBIGANA: $("#txtYobigana").val() ? $("#txtYobigana").val() : null,
        YOBINA: $("#txtYobina").val() ? $("#txtYobina").val() : null,
        VEHICLE_NUMBER: $("#txtVehicleNumber").val() ? $("#txtVehicleNumber").val() : null,
        DRIVER_NAME: $("#txtDriverName").val() ? $("#txtDriverName").val() : null,

        DISPLACEMENT: $("#txtDisplacement").val() ? $("#txtDisplacement").val() : null,
        MAX_LOAD: $("#txtVhclMaxLoad").val() ? $("#txtVhclMaxLoad").val() : null,
        TELEPHONE_NO: $("#txtTelphoneNumber").val() ? $("#txtTelphoneNumber").val() : null,
        TRANSPORT_COMPANY_NAME: $("#txtVhclTransportCompany").val() ? $("#txtVhclTransportCompany").val() : null,
        REMARKS: $("#txtRemark").val() ? $("#txtRemark").val() : null,
        /* IN_ACTIVE_FLAG:*/
        //CREATED_DATE: $("#txtCreatedDate").val() ? $("#txtCreatedDate").val() : null,
        //UPDATED_DATE: $("#txtUpdatedDate").val() ? $("#txtUpdatedDate").val() : null,


        USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null

    });

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/VehicleMaster/registerVehicle",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataRegisterVehicle

    }
    $.ajax(settings)
        .done(function (response) {
            if (response.success) {

                loadVehicleDetails(response.vehicleCD);
                disableSaveButtons()
                enableCopyButtons();
                enableUnUseButtons();
                enableDeleteButtons();
                enableEditButtons();

                var successMessage = $("#lblSuccessMessage_VehicleEntry_001").text();
                showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function loadVehicleDetails(vehicleId) {

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/VehicleMaster/GetVehicleById?vehicleId=" + vehicleId,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        /*data: vehicleId*/

    }
    $.ajax(settings)
        .done(function (response) {

            bindVehicleData(response.message);
            /*showMessageDialog(successMessage, "btnInitialize", SUCCESS);*/
        }
        )

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function bindVehicleData(data) {

    /*$("#txtVehicleCode").val(data.vehiclE_CD);*/
    $("#txtVehicleCode").val(data.vehiclE_CD ? data.vehiclE_CD.toString().padStart(3, '0') : '');
   
    $("#txtVehicleName").val(data.vehiclE_NAME);
    $("#txtShortName").val(data.shorT_NAME);
    $("#txtYobigana").val(data.yobigana);
    $("#txtYobina").val(data.yobina);
    $("#txtVehicleNumber").val(data.vehiclE_NUMBER);
    $("#txtDriverName").val(data.driveR_NAME);
    $("#txtDisplacement").val(data.displacement);
    $("#txtVhclMaxLoad").val(data.maX_LOAD);
    $("#txtTelphoneNumber").val(data.telephonE_NO);
    $("#txtVhclTransportCompany").val(data.transporT_COMPANY_NAME);
    $("#txtRemark").val(data.remarks);
    $("#txtCreatedDate").val(formatDate(data.createD_DATE));
    $("#txtUpdatedDate").val(formatDate(data.updateD_DATE));


    //  Handle Active/Inactive UI
    if (data.uN_USE_FLAG == 0) {
        $("#btnUnUse").val($("#lbl_Inactive").text().trim());
    } else {
        $("#btnUnUse").val($("#lbl_Active").text().trim());
    }

}

function formatDate(dateStr) {
    if (!dateStr) return '';

    var date = new Date(dateStr);

    var y = date.getFullYear();
    var m = ('0' + (date.getMonth() + 1)).slice(-2);
    var d = ('0' + date.getDate()).slice(-2);

    return y + '/' + m + '/' + d;
}

function btnCopy_Click() {

    $("#txtVehicleCode").val('');
    $("#txtCreatedDate").val('');
    $("#txtUpdatedDate").val('');

    $("#btnUnUse").val($("#lbl_Inactive").text().trim());
    disableEditButtons();
    disableDeleteButtons();
    disableUnUseButtons();
    enableSaveButtons()
}

function btnActiveInactive_Click() {

    var vehicleId = $("#txtVehicleCode").val();

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/VehicleMaster/ChangeVehicleStatus?vehicleId=" + $("#txtVehicleCode").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: JSON.stringify({
        //    vehicleId: parseInt(vehicleId)
        //}),

    }
    $.ajax(settings)
        .done(function (response) {
            if (response.success) {

                loadVehicleDetails(vehicleId);
                disableSaveButtons()
                enableCopyButtons();
                enableUnUseButtons();
                enableDeleteButtons();
                enableEditButtons();

                var successMessage = $("#lblSuccessMessage_VehicleEntry_001").text();
                showMessageDialog(successMessage, "btnInitialize", SUCCESS);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_vehicle_Entry_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}


function IsMandatorySearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {

        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
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

    //if (focusFieldID == "txtCheckApplicationDate") {
    //    if ($("#txtYear").val() == "") {
    //        focusFieldID = "txtYear";
    //    } else if ($("#txtMonth").val() == "") {
    //        focusFieldID = "txtMonth";
    //    } else {
    //        focusFieldID = "txtChance";
    //    }
    //}

    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
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
function btnCloseMaster_Click() {
    self.close();
}
$('#tblMatrial').on('keydown', 'input[type="text"]', function (e) {
    if (e.key === 'Enter') {
        e.preventDefault();

        let $td = $(this).closest('td');
        let $tr = $(this).closest('tr');
        let colIndex = $td.index() + 1; // column number (1,2,3)

        if (colIndex === 1) {
            // Move to column 2
            $tr.find('td:nth-child(2) input').focus();
        }
        else if (colIndex === 2) {
            // Move to column 3
            $tr.find('td:nth-child(3) input').focus();
        }
        else if (colIndex === 3) {
            // Move to next row, column 1
            let $nextRow = $tr.next('tr');
            if ($nextRow.length > 0) {
                $nextRow.find('td:nth-child(1) input').focus();
            }
        }
    }
});

$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})
function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');
}
function closepopup() {
    $("#SortPopUpModal").modal('hide');
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
    btnExtract_Click(selectedColumns);
    closepopup();
}
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
        url: "/VehicleMaster/InitializeSortPopup",
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


$("#txtDisplacement, #txtVhclMaxLoad, #txtTelphoneNumber, #txtVehicleCode, #txtVehicleCodeSearch")  
    .on("input", function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

$(document).on('input', 'input[type="text"]', function () {
    this.value = this.value
        .replace(/\t/g, '')
        .replace(/\s{2,}/g, ' '); // only prevent multiple spaces
});

// on leave → clean
$(document).on('blur', 'input[type="text"]', function () {
    this.value = this.value.trim();
});


$("#txtDisplacement, #txtVhclMaxLoad, #txtTelphoneNumber, #txtVehicleCode")
    .on("paste", function (e) {

        e.preventDefault();

        let pasted = (e.originalEvent || e).clipboardData.getData('text');
        pasted = pasted.replace(/[^0-9]/g, '');

        const input = this;
        let maxLength = input.maxLength > 0 ? input.maxLength : null;

        let start = input.selectionStart;
        let end = input.selectionEnd;

        let newValue = input.value.substring(0, start) + pasted + input.value.substring(end);

        if (maxLength !== null) {
            newValue = newValue.substring(0, maxLength);
        }

        input.value = newValue;

        let cursorPos = start + pasted.length;
        if (maxLength !== null && cursorPos > maxLength) {
            cursorPos = maxLength;
        }

        input.selectionStart = input.selectionEnd = cursorPos;
    });



$(document).on('paste', 'input[type="text"]:not(.only-number)', function (e) {

    e.preventDefault();

    let pasted = (e.originalEvent || e).clipboardData.getData('text');

    pasted = pasted
        .replace(/\t/g, '')
        .replace(/\u00A0/g, ' ')
        .replace(/\s+/g, ' ')
        .trim();

    const input = this;

    let maxLength = input.maxLength > 0 ? input.maxLength : null;

    let start = input.selectionStart;
    let end = input.selectionEnd;

    let newValue = input.value.substring(0, start) + pasted + input.value.substring(end);

    // 🔥 Enforce maxlength
    if (maxLength !== null) {
        newValue = newValue.substring(0, maxLength);
    }

    input.value = newValue;

    // cursor position fix
    let cursorPos = start + pasted.length;
    if (maxLength !== null && cursorPos > maxLength) {
        cursorPos = maxLength;
    }

    input.selectionStart = input.selectionEnd = cursorPos;
});

