var $btninitialize;
var $btnSave;
var $btnTransferNoSearch;
var $btnDelete;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var TensouNotFound;
var TensouCompleted;
var TensouAccepted;
var emptyRow = 20;

function setGlobalVariables() {
    $("#hdfDBOperation").val("");
    $btnTransferNoSearch = $("#btnTransferNoSearch");
    $btnSave = $("#btnSave");
    $btninitialize = $("#btninitialize");
    $btnDelete = $("#btnDelete");

}

function disableEntryDeleteButtons() {
    $("#btnSave").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function enableEditDeleteButtons() {
    $("#btnSave").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

$(document).ready(function () {
    debugger;
    $.support.cors = true;
   // window.resizeTo(1200, 675);
    //if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    //    window.resizeTo(360, 780);
    //    document.body.style.zoom = "400%";
    //} else {
    //    window.resizeTo(1200, 675);
    //}

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

    setNextProcess();
    //Mask();
    setStartSysDateTime();

    if ($("#hdfLoginId").val() != "") {
        setAcceptanceLocation($("#hdfLoginId").val());
    }
    disableEntryDeleteButtons();
    CheckNumberValidation();
    $("#txtTransferCD1").focus();
    disableGridCheckBox();

    for (var i = 1; i <= emptyRow; i++) {
        RestrictToDigitsOnly(i);
    }
    //disableEntryDeleteButtons();
});

function Mask() {
 
  //  $("#txtWIPAcceptanceDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#txtWIPAcceptanceDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $(document.getElementsByClassName('DateField')).mask("9999/99/99", { placeholder: "yyyy/MM/dd" });

}

function RestrictToDigitsOnly(index) {
    $("#txtTransferCD" + index).on('keypress input', function () {
        // Replace all non-digit characters
        this.value = this.value.replace(/[^0-9]/g, '');
    });
}

function CheckNumberValidation() {
    
    for (var i = 1; i <= emptyRow; i++) {
        $("[id^=txtTransferCD]").on("keypress", function (event) {
            var charCode = event.which ? event.which : event.keyCode;
            // allow only digits (0-9)
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
            }
        });
    }
}

//
function setStartSysDateTime() {
    debugger;
    var ajaxTime = new Date().getTime();
    //var jsonData = "{"
    //    + "USER_NAME" + " : '" + userName
    //    + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}" 
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;
           // $("#txtWIPAcceptanceDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
            $("#txtWIPAcceptanceDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
//bind next Process dropdown
function setNextProcess() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/GetNextProcess",
        method: "Get",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindNextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {

    var count = $('#WIPTensouAcceptance tbody tr').length;
    $('table#WIPTensouAcceptance tbody tr').each(function (index, item) {
        var rowNo = index + 1;

        var $ddlNextProcess = $(this).find("#ddlNextProcess" + rowNo);

        if ($("#hdfCultureName").val() == "en") {
            $.each(jsonResult, function (key, jsonObj) {
                $ddlNextProcess.append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
            });
        } else {
            $.each(jsonResult, function (key, jsonObj) {
                $ddlNextProcess.append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
            });
        }
    });

}


function setAcceptanceLocation(USER_ID, $callFrom) {
    debugger;
    if ($callFrom) {
        $callFrom.data('requestRunning', true);
    }

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_ID: '" + USER_ID + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/GetAcceptanceLocation?USER_ID=" + USER_ID,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jResult = response.message;
            var jsonResult = jResult[0];
            $('#txtInvoicePlace').val(jsonResult.name);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


$(document).on('click', '#btnTransferNoSearch', function (event) {

    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();
    checkValidTransferNo();
    // setNextProcess();
    $("#btnSave").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
    // enableEditDeleteButtons();
    // GetTensouAcceptanceDetailsByTransferCD();

});

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


function resetResponseTime() {
    responseTime = ""; // Clear server response time
}

function disableGridCheckBox() {
    for (var i = 1; i <= emptyRow; i++) {
        $("#chkTensouAccepGrid" + i).attr("disabled", true);
    }
}
function GetTensouAcceptanceDetailsByTransferCD(tensouCD) {
    debugger;

    var i = 1;
    var gridData = [];
    var tensou = [];

    var success = false;
    var ajaxTime = new Date().getTime();
    var jsonData = "{tensouCD: '" + tensouCD + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/GetTensouAcceptanceDetailsByTransferCD?tensouCD=" + tensouCD + "&CultureName=" + $("#hdfCultureName").val(),       //?transferCD=" + parseInt(transferCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonThabaDetailData + "}"
        // data: JSON.stringify(gridData)

    }

    $.ajax(settings)
        .done(function (response) {
            debugger;
            var jsonResult = response.message;

            //Or No Not Started From HandOver.
            if (jsonResult[0].procesS_PLAN_START_DATE == null) {
                // initialize();
                var message = $("#lblErrorMessageE_WIPTensou_012").text().trim().replace("99-999999-99", jsonResult[0].pP_CHUUBAN);
                showMessageDialog(message, "", WARNING);
                return false;
            }

            //for (var i = 1; i <= emptyRow; i++) {
            //    if ($("#txtTransferCD" + i).val() !== "") {
            //        var tensouNo =$("#txtTransferCD" + i).val();
            //        isDataExist = jsonResult.filter(r => r.wiP_TRANSFER_CD == tensouNo);
            //        if (isDataExist.length > 0) { 
            //            debugger;
            //            var jsonObj = isDataExist[0];
            //            $("#hdfTransferNo" + i).text(jsonObj.wiP_TRANSFER_CD);
            //            $("#hdfUnitType" + i).text(jsonObj.uniT_TYPE);
            //            $("#hdfNoOfUnits" + i).text(jsonObj.nO_OF_UNITS);
            //        }
                   
                  
            //    }
            //}
            
            //if (response.isSuccess) {
            //    $("#WIPTensouAcceptanceGrid").html(response.message);
            //}


            //Remove Enter Transfer no 
            //for (var i = 1; i < emptyRow; i++) {
            //    $('table#WIPTensouAcceptance tbody tr').each(function () {
            //        $(this).find('#txtTransferCD' + i).val("");
            //    });
            //}

            setTensouDetails(jsonResult);

        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);

        });
}


//function setTensouDetails(jsonResult) {
//    debugger;

//    const data = jsonResult;

//    var i = 0;

//    dbOperation = "Insert";

//    $("#hdfDBOperation").val(dbOperation);

//    //$('table#WIPTensouAcceptance tbody tr').each(function () {
//    //    debugger;
//    //    if (i < jsonResult.length) {

//    for (var k = 1; k <= emptyRow; k++) {

//        var isDataFound = false;
//        var chuban1 = $("#txtChuuban" + k).val();
//        if (chuban1 > 0) {

//            var isDataExist = "";
//            isDataExist = chubanList.filter(r => r.chuuban == chuban1);
//            if (isDataExist.length > 0) {
//                $.each(jsonresult, function (i, jsonObj) {



//                    let row = i + 1;
//                    // Assuming 'row' and 'jsonResult' are defined in the context.
//                    let transferCode = jsonResult[i].wiP_TRANSFER_CD.toString(); // Convert to string if it's a number.
//                    // Pad with leading zeroes to make it 10 digits long.
//                    let paddedCode = transferCode.padStart(10, '0');
//                    // Assign the padded code to the input field.
//                    $(this).find('#txtTransferCD' + row).val(paddedCode);
//                    $(this).find('#lblChuuban' + row).text(jsonResult[i].pP_CHUUBAN);
//                    $(this).find('#lblCustomer' + row).text(jsonResult[i].pP_CUSTOMER_NAME);

//                    $(this).find('#lblProdNameCust' + row).text(jsonResult[i].pP_PROD_NAME_CUST);
//                    $(this).find('#lblLength' + row).text(jsonResult[i].pP_PROD_LENGTH_STRING);
//                    $(this).find('#lblCharge' + row).text(jsonResult[i].pP_CHARGE);
//                    $(this).find('#lblDimD' + row).text(jsonResult[i].pP_MATL_DIM_D);
//                    $(this).find('#lblShape' + row).text(jsonResult[i].pP_MATL_SHAPE);
//                    $(this).find('#lblWidthAndOther' + row).text(jsonResult[i].pP_PROD_DIM_W_OTHER);

//                    $(this).find('#lblNoOfUnits' + row).text(jsonResult[i].nO_OF_UNITS + jsonResult[i].uniT_TYPE_STRING);

//                    $(this).find('#lblTabaNo' + row).text(jsonResult[i].tabA_STRING);
//                    $(this).find('#lblWeightWithoutKG' + row).text(jsonResult[i].qty);
//                    $(this).find('#ddlNextProcess' + row).val(jsonResult[i].nexT_PROSESS);
//                    $(this).find('#txtSource' + row).val(jsonResult[i].msG_CONTENTS);
//                    $('#hdProcessPlanCD').val(jsonResult[i].procesS_PLAN_CD);
//                    $('#hdfldTabaNo1').val(jsonResult[i].tabA_NO_1);
//                    $('#hdfldTabaNo2').val(jsonResult[i].tabA_NO_2);
//                    $('#hdfldTabaNo3').val(jsonResult[i].tabA_NO_3);
//                    $('#hdfldTabaNo4').val(jsonResult[i].tabA_NO_4);
//                    //$('#hdflNoOfUnits').val(jsonResult[i].nO_OF_UNITS);
//                    //$('#hdflUnitType').val(jsonResult[i].uniT_TYPE);
//                    $('#hdflocationcdfrom').val(jsonResult[i].locatioN_CD_FROM);
//                    $('#hdflocationCDTo').val(jsonResult[i].locatioN_CD_TO);
//                    $('#hfld_MessageCD').val(jsonResult[i].msG_CD);
//                }
//           }
//        }
//        i++;
//    });
//}
//}

function setTensouDetails(jsonResult) {
    debugger;
    const data = jsonResult;
    dbOperation = "Insert";
    $("#hdfDBOperation").val(dbOperation);

    for (var k = 1; k <= emptyRow; k++) {

        var isDataFound = false;
        var tensouNo = $("#txtTransferCD" + k).val();

        if (tensouNo > 0) {

            var isDataExist = "";
            isDataExist = jsonResult.filter(r => r.wiP_TRANSFER_CD == tensouNo);
            var transNo = isDataExist[0].wiP_TRANSFER_CD;
            if (transNo != "") {
                if (transNo == tensouNo) {
                    $.each(isDataExist, function (i, jsonObj) {

                        let row = k;
                        jsonResult[i] = isDataExist[0];
                        // Convert transfer code to string and pad with leading zeros (10 digits)
                        let transferCode = jsonResult[i].wiP_TRANSFER_CD.toString();
                        let paddedCode = transferCode.padStart(10, '0');

                        // Assign values to fields
                        $('#txtTransferCD' + row).val(paddedCode);
                        $('#lblChuuban' + row).text(jsonResult[i].pP_CHUUBAN);
                        $('#lblCustomer' + row).text(jsonResult[i].pP_CUSTOMER_NAME);
                        $('#lblProdNameCust' + row).text(jsonResult[i].pP_PROD_NAME_CUST);
                        $('#lblLength' + row).text(jsonResult[i].pP_PROD_LENGTH_STRING);
                        $('#lblCharge' + row).text(jsonResult[i].pP_CHARGE);
                        $('#lblDimD' + row).text(jsonResult[i].pP_MATL_DIM_D);
                        $('#lblShape' + row).text(jsonResult[i].pP_MATL_SHAPE);
                        $('#lblWidthAndOther' + row).text(jsonResult[i].pP_PROD_DIM_W_OTHER);
                        $('#lblNoOfUnits' + row).text(jsonResult[i].nO_OF_UNITS + jsonResult[i].uniT_TYPE_STRING);
                        $('#lblTabaNo' + row).text(jsonResult[i].tabA_STRING);
                        $('#lblWeightWithoutKG' + row).text(jsonResult[i].qty);
                        $('#ddlNextProcess' + row).val(jsonResult[i].nexT_PROSESS);
                        $('#txtSource' + row).val(jsonResult[i].msG_CONTENTS);
                        // Hidden fields
                        $("#hdfTransferNo" + row).val(jsonResult[i].wiP_TRANSFER_CD);
                        $("#hdfUnitType" + row).val(jsonResult[i].uniT_TYPE);
                        $("#hdfNoOfUnits" + row).val(jsonResult[i].nO_OF_UNITS);
                        $("#hdProcessPlanCD" + row).val(jsonResult[i].procesS_PLAN_CD);
                       /// $('#hdProcessPlanCD').val(jsonResult[i].procesS_PLAN_CD);
                        $('#hdfldTabaNo1').val(jsonResult[i].tabA_NO_1);
                        $('#hdfldTabaNo2').val(jsonResult[i].tabA_NO_2);
                        $('#hdfldTabaNo3').val(jsonResult[i].tabA_NO_3);
                        $('#hdfldTabaNo4').val(jsonResult[i].tabA_NO_4);
                        $('#hdflocationcdfrom').val(jsonResult[i].locatioN_CD_FROM);
                        $('#hdflocationCDTo').val(jsonResult[i].locatioN_CD_TO);
                        $('#hfld_MessageCD').val(jsonResult[i].msG_CD);



                    });

                }
            }
        }
       // i++;
    }
}

function checkValidTransferNo() {
    debugger;
    var i = 1;
    var gridData = [];
    var tensou = [];
    $('table#WIPTensouAcceptance tbody tr').each(function (index, item) {
        var row = index + 1;
        if ($("#txtTransferCD" + row).val() != "") {
            var $tr = $(this);
            var data = {
                WIP_TENSOU_ROW: index,
                WIP_TRANSFER_CD: $tr.find("#txtTransferCD" + row).val() ? $tr.find("#txtTransferCD" + row).val() : 0,

            };
            row++;
            gridData.push(data);
        }
    });

    // var date = ($('#txtTransferCD').val());
    // if (date == "") {
    if (gridData.length == 0) {
        var message = $("#lblErrorMessageE_WIPTensou_009").text().trim();
        // This message will be displayed on dialog box
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
        url: APIURL + "/WIPTensouAcceptance/GetValidTensouNo",        //?transferCD=" + parseInt(transferCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify(gridData)

    }

    $.ajax(settings)
        .done(function (response) {
          
            var jsonResult = response.message;
            TensouNotFound = jsonResult.notFound;
            if (jsonResult.found.length) {
                var FoundTransferNo = jsonResult.found;
                GetChuubanCompleteOrNot(FoundTransferNo);
                // GetTensouAcceptanceDetailsByTransferCD(FoundTransferNo);
            } else {
                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_WIPTensou_008").text().trim();
                MessageData = message + "<br>" + TensouNotFound;
                showMessageDialog(MessageData, focusFieldID, WARNING);

                //TensouNotFound no remove on grid
                debugger;
                for (var i = 0; i < emptyRow; i++) {
                    $('table#WIPTensouAcceptance tbody tr').each(function () {
                        var temp = $(this).find('#txtTransferCD').val();
                        if (temp == TensouNotFound[i]) {
                            $(this).find('#txtTransferCD').val('');
                            temp.remove;
                        }
                    })
                }

            }


        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })


}


function GetChuubanCompleteOrNot(completeTransferNo) {

    var ajaxTime = new Date().getTime();
    var jsonData = "{completeTransferNo: '" + completeTransferNo + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/GetCompleteChuuban?completeTransferNo=" + completeTransferNo,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{FoundTransferNo : " + FoundTransferNo + "}"
    }
    $.ajax(settings)
        .done(function (response) {
        
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            TensouCompleted = jsonResult.complete;
            if (jsonResult.notComplete.length) {
                var notCompleteTransferNo = jsonResult.notComplete;
                GetTensouNoAlreadyAccepted(notCompleteTransferNo);
            }
            else {
                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_WIPTensou_007").text().trim();
                MessageData = message + "<br>" + TensouCompleted;
                showMessageDialog(MessageData, focusFieldID, WARNING);
                return false;
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

function GetTensouNoAlreadyAccepted(transferNoAccepted) {

    var ajaxTime = new Date().getTime();
    var jsonData = "{transferNoAccepted: '" + transferNoAccepted + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/TransferNoAccepted?transferNoAccepted=" + transferNoAccepted,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{FoundTransferNo : " + FoundTransferNo + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            // var jsonResult = jResult[0];
            TensouAccepted = jsonResult.accepted;
           
            if (jsonResult.notAccepted.length) {
                var notAcceptedTransferNo = jsonResult.notAccepted;
                GetTensouAcceptanceDetailsByTransferCD(notAcceptedTransferNo);
            }
            else {
                GetTensouAcceptanceDetailsByTransferCD(TensouAccepted);
                $("#btnSave").attr("disabled", "disabled");
                
            }
            //else {
            //    focusFieldID = $(':focus').attr("id");
            //    message = $("#lblErrorMessageE_WIPTensou_006").text().trim();
            //    MessageData = message + "<br>" + TensouAccepted;
            //    showMessageDialog(MessageData, focusFieldID, WA RNING);
            //}
       
            if (TensouNotFound != null || TensouCompleted != null || TensouAccepted != null) {
                focusFieldID = $(':focus').attr("id");
                var message = "";
                var message1 = "";
                var message2 = "";
                if (TensouNotFound.length > 0) { message = $("#lblErrorMessageE_WIPTensou_008").text().trim(); }
                if (TensouCompleted.length > 0) { message1 = $("#lblErrorMessageE_WIPTensou_007").text().trim(); }
                if (TensouAccepted.length > 0) { message2 = $("#lblErrorMessageE_WIPTensou_006").text().trim(); }
                var MessageD = message + " <br> " + TensouNotFound + " <br> " + message1 + " <br> " + TensouCompleted + " <br> " + message2 + " <br> " + TensouAccepted;
                //Remove <br> message box.
                var MessageData = MessageD.replace(/<br>/g, "");

                // Transfer No not found display on msg popup so these transfer no remove on grid same condition TensouCompleted & TensouAccepted
                if (TensouNotFound != null) {

                    for (var i = 0; i < TensouNotFound.length; i++) {
                        $('table#WIPTensouAcceptance tbody tr').each(function () {
                            var temp = $(this).find('#txtTransferCD').val();
                            if (temp == TensouNotFound[i]) {
                                $(this).find('#txtTransferCD').val('');
                                temp.remove;                                          //TensouNotFound no remove on grid
                            }
                        })
                    }
                }

                if (TensouCompleted != null) {

                    for (var i = 0; i < TensouCompleted.length; i++) {
                        $('table#WIPTensouAcceptance tbody tr').each(function () {
                            var temp = $(this).find('#txtTransferCD').val();
                            if (temp == TensouCompleted[i]) {
                                $(this).find('#txtTransferCD').val('');
                                temp.remove;                                      // TensouCompleted remove on grid
                            }
                        })
                    }
                }

                if (TensouAccepted != null) {

                    for (var i = 0; i < TensouAccepted.length; i++) {
                        $('table#WIPTensouAcceptance tbody tr').each(function () {
                            var temp = $(this).find('#txtTransferCD').val();
                            if (temp == TensouAccepted[i]) {
                                $(this).find('#txtTransferCD').val('');
                                temp.remove;
                                // TensouAccepted remove on grid
                            }
                        })
                    }
                }

              

                if (TensouNotFound.length > 0 || TensouCompleted.length > 0 || TensouAccepted.length > 0) {
                    showMessageDialog(MessageData.trim(), focusFieldID, WARNING);
                }
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


$(document).on('click', '#btnSave', function (event) {
   
    resetResponseTime();
    var $callFrom = $(this);
    event.preventDefault();

    var lstChuban = [];

    //Acceptance date Required.
    var date = ($('#txtWIPAcceptanceDate').val());
    if (date == "") {

        var message = $("#lblErrorMessageE_WIPTensou_002").text().trim();
        // This message will be displayed on dialog box
        showMessageDialog(message, "", WARNING);
        $("#txtWIPAcceptanceDate").focus;
        return false;

    }

    // Acceptance date Invalid
    if (!checkDate($("#txtWIPAcceptanceDate"))) {
        var message = $("#lblErrorMessageE_WIPTensou_003").text().trim(); // invalid acceptance date
        showMessageDialog(message, "", WARNING);
        $("#txtWIPAcceptanceDate").focus;
        return false;

    }

    //date not range
    //if (!IsDateDifferentFromRange(true)) {
    //    var message = $("#lblErrorMessageE_WIPTensou_010").text().trim() + "(" + $("#lblErrorMessageE_WIPTensou_011").text().trim() + commonDateRange + ")";
    //    var focusFieldID = "txtWIPAcceptanceDate";
    //    showMessageDialog(message, focusFieldID, WARNING);
    //    return false;
    //} Comment By Komal

    //Next Process required.

    for (var i = 1; i <= emptyRow; i++) {
        var $tr = $(this);
        var chuuban = $("#txtTransferCD" + i).val();
        if (chuuban != "") {
            
            lstChuban.push(chuuban);
        }
    }


    for (var i = 1; i <= lstChuban.length; i++) {
        if ($("#ddlNextProcess" + i).val() == 0) {
            
            var message = $("#lblErrorMessageE_WIPTensou_013").text().replace("NNN", i);
            showMessageDialog(message, "", WARNING);
            $("#ddlNextProcess").focus;
            return false;
        }
    }

    if ($("#hdfDBOperation").val() == "Insert") {
        SaveWIPTensouAcceptanceDetails($callFrom);
    }

});

function SaveWIPTensouAcceptanceDetails($callFrom) {

    debugger;
    var ajaxTime = new Date().getDate();
    var gridData = [];

    
    var jsonDataForSave = [];
   
    for (var i = 1; i <= emptyRow; i++) {
        debugger;
      
        if ($("#txtTransferCD" + i).val() !== "") {

            let taba = $("#lblTabaNo" + i).text();
            let splitTaba = taba.split(',');
          
            var ajaxTime = new Date().getDate();
            jsonDataForSave = ({

            PROCESS_PLAN_CD: $("#hdProcessPlanCD" + i).val() ? $("#hdProcessPlanCD" + i).val() : null,
            WIP_TRANSFER_CD: $("#txtTransferCD" + i).val() ? $("#txtTransferCD" + i).val() : null,
            LOCATION_CD_FROM: $("#hdflocationcdfrom").val() ? $("#hdflocationcdfrom").val() : null,
            LOCATION_CD_TO: $("#hdflocationCDTo").val() ? $("#hdflocationCDTo").val() : null,
            //ACCEPTANCE_DATE: ($("#txtWIPAcceptanceDate").val().split(" ")[0] + "T" + $("#txtWIPAcceptanceDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),\
            TRANSFER_DATE_STRING: $("#txtWIPAcceptanceDate").val(),
            TABA_NO_1: splitTaba[0] ? splitTaba[0] : null,
            TABA_NO_2: splitTaba[1] ? splitTaba[1] : null,
            TABA_NO_3: splitTaba[2] ? splitTaba[2] : null,
            TABA_NO_4: splitTaba[3] ? splitTaba[3] : null,
            UNIT_TYPE: $("#hdfUnitType" + i).val() ? $("#hdfUnitType" + i).val() : null,
            NO_OF_UNITS: $("#hdfNoOfUnits" + i).val() ? $("#hdfNoOfUnits" + i).val() : null,
            QTY: $("#lblWeightWithoutKG" + i).text() || 0,
            NEXT_PROSESS: $("#ddlNextProcess" + i).val() ? $("#ddlNextProcess" + i).val() : null,
            MSG_CONTENTS: $("#txtSource" + i).val() ? $("#txtSource" + i).val() : null,
            MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
            USER_ID: $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null
        });
            gridData.push(jsonDataForSave);
        }
       
}
        
    
    
    var settings = {

        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/SaveWIPTensouAcceptanceDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
          
            var jsonResult = response.message;

            var successMessage = $("#lblSuccessMessageS_WIPTensou_001").text();
            showMessageDialog(successMessage, "", SUCCESS);
            
            $("#btnSave").attr("disabled", "disabled");
            $("#btnDelete").removeAttr("disabled");
        })

        .fail(function (xhr, textStatus, errorThrown) {
           
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIPTensou").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        })
}

//checkDate
function checkDate(field) {

    var minYear = 2019;
    var maxYear = 2100;
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/      //CHECK ONLY DATE
   // re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/   //CHECK DATE TIME
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

//Validation
function IsMandatoryFieldsFilled() {
   
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box

    var focusFieldID = "";
    $.each($.find(".MandatoryTextBox"), function (index, mandatoryField) {
        if (mandatoryField.type === text) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("errorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        showMessageDialog(message, focusFieldID, WARNING)

    }
    return returnVal;
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

        let taba = $("#lblTabaNo" + i).text(); 
        let splitTaba = taba.split(',');
        var transferCD = $("#txtTransferCD" + i).val();
       // var transferCD = $("#hdfTransferNo" + i).text();
        if (transferCD !== "") {
            var ajaxTime = new Date().getDate();
            var $tr = $(this);
            var data = {
                WIP_TENSOU_ROW: i,
                PROCESS_PLAN_CD: $("#hdProcessPlanCD" + i).val() ? $("#hdProcessPlanCD" + i).val() : null,// $("#hdProcessPlanCD").val() ? $("#hdProcessPlanCD").val() : null,
                WIP_TRANSFER_CD: $("#txtTransferCD" + i).val() ? $("#txtTransferCD" + i).val() : null,
                TABA_NO_1: splitTaba[0] || 0,
                TABA_NO_2: splitTaba[1] || 0,
                TABA_NO_3: splitTaba[2] || 0,
                TABA_NO_4: splitTaba[3] || 0,
                MSG_CONTENTS: $("#txtSource" + i).val() ? $("#txtSource" + i).val() : null,
                MSG_CD: $("#hfld_MessageCD" + i).val() ? $("#hfld_MessageCD" + i).val() : 0,
            };
           // i++;
            gridData.push(data);
        }
       
    }
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        url: APIURL + "/WIPTensouAcceptance/WIPTensouAcceptanceDetailsDelete",  
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(gridData)
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            var successMessage = $("#lblSuccessMessageS_WIPTensou_DataDeleted").text();
            showMessageDialog(successMessage, "", SUCCESS);
            disableEntryDeleteButtons();
            clear();
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnClose").click(function (event) {
    self.close();
});

$("#btninitialize").click(function (event) {
    clear();
    setStartSysDateTime();


});

function clear() {
    
    var i = 0;
   
    clearPanel($("#divTensouGridMain"));
    $('table#tblWIPTensouAcceptance tbody tr').each(function (i) {
        debugger;
        if (i <= emptyRow) {
            var row = i + 1;
            $("#lblChuuban" + row).text("");
            $("#lblCustomer" + row).text("");
            $("#lblProdNameCust" + row).text("");
            $("#lblShape" + row).text("");
            $("#lblDimD" + row).text("");
            $("#lblWidthAndOther" + row).text("");
            $("#lblLength" + row).text("");
            $("#lblCharge" + row).text("");
            $("#txtWIPAcceptanceDate" + row).val("");
            $("#lblTabaNo" + row).text("");
            $("#lblNoOfUnits" + row).text("");
            $("#lblWeightWithoutKG" + row).text(""); 
            $("#hfld_MessageCD" + row).val("");
            $("#chkTensouAccepGrid" + i).attr("disabled", true);
            
        }
    });

    disableEntryDeleteButtons();

}

function clearPanel($panel) {
    $.each($panel.find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });
    $.each($panel.find("select"), function (index, dropdown) {
        $("#" + dropdown.id + " option:selected").prop("selected", false);
        $("#" + dropdown.id + " option:first").prop("selected", "selected");
    });
    $.each($panel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).prop('checked', false);
    });

    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });


}


function IsDateDifferentFromRange(boolVal) {

    var returnval = false;
    var getDate;
    if (boolVal === true) {
        //getDate = ($("#txtWIPAcceptanceDate").val().split(" ")[0] + "T" + $("#txtWIPAcceptanceDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-');
        getDate = ($("#txtWIPAcceptanceDate").val()).replaceAll('/', '-');
    }
    else {
        getDate = ($("#txtWIPAcceptanceDate").val()).replaceAll('/', '-');
    }

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTensouAcceptance/IsValidDateRange",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ DATERANGE: commonDateRange, RANGE_DATETIME: getDate })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnval = response.isSuccess;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_WIPTensou").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


    return returnval;
}


//TransferCD scan then Auto focus on next textBox.
$(document).on('keypress', '[id^="txtTransferCD"]', function (e) {
var lstTensou = [];
    for (var i = 1; i <= emptyRow; i++) {
        var $tr = $(this);
        var tensou = $("#txtTransferCD" + i).val();
        if (tensou != "") {
            lstTensou.push(tensou);
        }
    }

    for (var i = 1; i <= lstTensou.length; i++) {
        var row = i + 1;
        $("#txtTransferCD" + row).focus();
    }

});

//If textbox-[txtTransferCD] has a value → it enables the checkbox[chkTensouAccepGrid].
//If textbox[txtTransferCD] is empty → it disables that checkbox.
document.querySelector("#tblWIPTensouAcceptance tbody").addEventListener("change", function (event) {
    debugger;
    const row = event.target.closest("tr");  
    var $val = row.id;
    const input = $val;
    const [, letters, number] = input.match(/^([a-zA-Z]+)(\d+)$/) || [];
    var txtval = ('txtTransferCD' + number)
    const elementTransferNo = document.getElementById(txtval);
    if (elementTransferNo) {
        elementTransferNo.addEventListener("change", onTransferNoInputChange(number));
    }

});

//Function to enable/disable checkbox
function onTransferNoInputChange(rowID) {
    debugger;
    var transferNo = $("#txtTransferCD" + rowID).val();
    if (transferNo.trim() !== "") {
        $("#chkTensouAccepGrid" + rowID).removeAttr("disabled");
    }
    else {
        $("#chkTensouAccepGrid" + rowID).attr("disabled", true);
    }
}

//////////////////



























