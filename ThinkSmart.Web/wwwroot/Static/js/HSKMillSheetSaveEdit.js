var $btnLnkEdit;
var combinedDecisionOK = 1;
var indexDDLDecisionNothing = 0;
var typeDropDown = "select-one";
var typeText = "text";
var $hdfSYSDATE;
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var oldHikinukiRemarks;
var isHikinukiRemarksUpdate = 0;
var IsExists = false;
var IsStartOprExists = false;
var IsEndOprExists = false;
var registeredThabaNumbersAndDecision = {};
var maxRegisteredThabaNo = 0;
var IsNGReason1 = false;
var IsNGReason2 = false;
var IsNGReason3 = false;
var IsProcessDefect1Exists = false;
var IsProcessDefect2Exists = false;
var IsProcessDefect3Exists = false;
var startOperatorCD = "";
var endOperatorCD = "";

userName = "";
var ProcessPlanCD = 0;
var m_hskMultiper = 0;
var m_mkrMultiper = 0;
var m_mkrActualRes = 0;
var m_hskActualRes = 0;
var ccRange = "";
var ccAct = "";
var ccNm = "";
var m_ccActual = "";
var m_ccRange = "";
var DISABLED = "disabled";
var typeText = "text";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var gridRowsCount = 0;
var flagValue = false;
var m_ccMin = "";
var m_ccMax = "";
var m_prodOrderNoInspection = "";
var m_prodCD = "";
var m_prodVerCondDesc
const m_MakerActualResultNonNumericList = [];

$(document).ready(function () {
    debugger;
    //  window.resizeTo(1080, 830);
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    //setDecimalFormats();
    Mask();

    InitSortPopup();
    $("#txtChuubanSearch").focus();
    userName = $("#hdfUserName").val();
    bindEvents();
    SetSysDateTime();
    setProductQuntityList();

    let ProcessPlanCD = getURLTRNProcPlanCdParameters(window.location.href).trim();
    if (parseInt(ProcessPlanCD) > 0) {
        GetHSKMillSheetProcessPlanCD(ProcessPlanCD);
    }
    $('input[id="selectRow1"]').eq(0).prop('checked', true);

    // Get parameters from current URL
    var postHTCD = getURLParameters(window.location.href).trim();
    if (postHTCD != "") {
        GetPostHTInspectionDetailsByPostHTCD(postHTCD);
    }

    var finalInspCD = getURLFIParameters(window.location.href).trim();
    if (finalInspCD != "") {
        GetFinalInspectionDetailsByPostFinalInspCD(finalInspCD);
    }
});

function SetSysDateTime() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var sysDateTime = response.message;
            debugger;
            var date1 = new Date(sysDateTime);
            date1.setFullYear(date1.getFullYear() - 1);
            $("#txtDate1").val($.format.date(date1, "yyyy/MM/dd"));

            $("#txtDate2").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });




        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}


function Mask() {
    $("#txtDate1").mask("9999/99/99", { placeholder: "YYYY/MM/DD HH:mm" });
    $("#txtDate1").mask("9999/99/99", { placeholder: "YYYY/MM/DD HH:mm" });
    $("#txtDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm 99:99" });
  
    if (!$("#txtDate").hasClass("hasDatepicker")) {
        $("#txtDate").datepicker({
            dateFormat: "yy/mm/dd"
        });
    }

    $("#txtDate").attr("placeholder", "yyyy/mm/dd");
}
function getURLTRNProcPlanCdParameters(url) {
    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('ProcessPlanCD=');
    result = sParameterName[1];
    return result;
}

//Get internal quality CD by url.
function getURLParameters(url) {
    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('internal_quality_CD=');
    var sParameterName = sPageURL.split('finalInspCD=');
    result = sParameterName[1];
    return result;
}

function getURLFIParameters(url) {
    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('finalInspCD=');
    result = sParameterName[1];
    return result;
}

function GetHSKMillSheetProcessPlanCD(ProcessPlanCD) {
    debugger;
  
    var ajaxTime = new Date().getTime();
   

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetHSKMillSheetProcessPlanCD?ProcessPlanCD=" + ProcessPlanCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
       // data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfcultureName").val() })
    }

    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (Object.keys(jsonResult).length > 0) {
                setChuubanDetails(jsonResult);
            }
            //setStartData(jsonResult);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}



function GetPostHTInspectionDetailsByPostHTCD(postHTCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetPostHTInspectionDetailsByPostHTCD?postHTCD=" + postHTCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            debugger;
            if (response.isSuccess == true) {
                $("#lblDMF_Val").text(result[0].dmF_VAL);
                $("#lblDMT_Val").text(result[0].dmT_VAL);
                $("#lblDMF").text(result[0].stD_DMF);
                $("#lblDMT").text(result[0].stD_DMT);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetFinalInspectionDetailsByPostFinalInspCD(finalInspCD) {
    debugger;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetFinalInspectionDetailsByPostFinalInspCD?finalInspCD=" + finalInspCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            debugger;
            if (response.isSuccess == true) {
                $("#lblDMF_Val").text(result[0].fI_DIM1);
                $("#lblMechHardenabilityTest").text("");
                // $("#lblMechHardenabilityTest").text("角Rの大きさ Camer Radius(mm)");

                $("#lblRadius_Val").text(result[0].corneR_RADIUS);
                //$("#lblDMF").text(result[0].stD_DMF);
                //$("#lblDMT").text(result[0].stD_DMT);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function bindEvents() {
    $("#btnChuubanSearch").click(function (event) {
        debugger;
        var $callFrom = $(this);

        if (!IsMandatoryChuubanSearchFieldsFilled()) {
            return false;
        }
        ValidateChubban($("#txtChuuban").val(), $("#txtGyouban").val(), $callFrom);
        //  ValidateChubban($("#txtChuuban").val(), $("#txtGyouban").val(), $callFrom);
        //getThabaDetailsByChuuban($("#txtChuubanNew").val(), $("#txtGyoubanNew").val(), $callFrom);
        //  getMachineDetails($("#hdfMachineCD").val(), $callFrom);
    });
}

$("#btnPostHTSearch").click(function (event) {
    debugger;
    var $callFrom = $(this);
    if ($("#txtChuubanNew").val() == "" && $("#txtGyoubanNew").val() == "") {
        //Required Chuuban.
        var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ChubanRequired").text();
        showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
        return false;
    }
    else {
        var chuuban = $("#txtChuubanNew").val();
        var gyouban = $("#txtGyoubanNew").val();
        GetProcessPlanCD(chuuban, gyouban, $callFrom);
    }

});


$("#btnChemicalCompositionPopup").click(function (event) {
    debugger;
    var FileName = $("#lblvalMillSheetNumber").text();
    GetMakerHardnessDetailJIS(FileName);
   

});


function GetMakerHardnessDetailJIS(FileName) {

   
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetMakerHardnessDetailJIS?FiileName=" + FileName,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        // data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfcultureName").val() })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;
           
            if (Object.keys(jsonResult).length > 0) {
                $('#MakerChemicalPopUp').modal('show');
                setMakerHardnessDetailJISData(jsonResult);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}


function setMakerHardnessDetailJISData(jsonResult) {
     
    function val(key) {
        var v = jsonResult[key];
        return v === null || v === undefined ? "" : v;
    }

    for (var i = 1; i <= 25; i++) {
        $("#lblStd" + i).text(val("harD_STD" + i));
        $("#lblMin" + i).text(val("harD_MIN" + i));
        $("#lblMax" + i).text(val("harD_MAX" + i));
        $("#lblActual" + i).text(val("harD_ACTUAL" + i));
    }

}
//$('#btn').click(function () {
//    $('#modelWindow').modal('show');
//});

function checkboxChanged(checkbox) {
    debugger;
    if (checkbox.checked) {
        const count = checkCount();

        if (count > 3) {
            alert("You can select a maximum of 3 checkboxes.");
            checkbox.checked = false;
        }
    }
}
function checkCount() {
    debugger;
   
    let count = 0;

    for (let i = 1; i <= 25; i++) {
        const checkbox = document.getElementById("chkval" + i);

        if (checkbox && checkbox.checked) {
            count++;
            $("#val" + i).text(1);
        }
    }

    return count;
}
$(document)
    .off("change.hskMillSheetCheckbox", "[id^='chkval']")
    .on("change.hskMillSheetCheckbox", "[id^='chkval']", function () {
        checkboxChanged(this);
    });


$('#chkhardnessCal').on('change', function () {
    if ($(this).is(':checked')) {
        // Checkbox checked
        var FileName = $("#lblvalMillSheetNumber").text();
        GetMakerHardnessDetailJISCal(FileName);
    } else {
        // Checkbox unchecked
        var FileName = $("#lblvalMillSheetNumber").text();
        GetMakerHardnessDetailJIS(FileName);
    }
});

function GetMakerHardnessDetailJISCal(FileName) {


    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetMakerHardnessDetailJISCal?FiileName=" + FileName,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        // data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfcultureName").val() })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (Object.keys(jsonResult).length > 0) {
                $('#MakerChemicalPopUp').modal('show');
                setMakerHardnessDetailJISData(jsonResult);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}




$("#btnCloseJISPopUp").click(function (event) {
    debugger;
    var selectedValues = [];
   
    var rows = $("#tblHardnessJISDetails tbody tr");

    // 1st row = column values
    var valueRow = rows.eq(0);

    // 4th row = checkboxes
    var checkboxRow = rows.eq(4);

    checkboxRow.find("td").each(function (index) {

        var checkbox = $(this).find("input[type='checkbox']");

        if (checkbox.is(":checked")) {
            var columnValues = [];
            for (var rowIndex = 0; rowIndex < 4; rowIndex++) {

                var value = rows.eq(rowIndex)
                    .find("td")
                    .eq(index)
                    .text()
                    .trim();

                columnValues.push(value);
            }
            selectedValues.push(columnValues);
        }
    });

    console.log(selectedValues);
    selectedValues.forEach(function (item, index) {

        var no = index + 1;

        $("#lblJIS" + no).prop("hidden", false);
        $("#lblJISRange" + no).text("J" + item[0]).prop("hidden", false);
        $("#lblJISMin" + no).text(item[1]).prop("hidden", false);
        $("#lblJISMax" + no).text(item[2]).prop("hidden", false);
        $("#lblJISActual" + no).text(item[3]).prop("hidden", false);

    });
    $('#MakerChemicalPopUp').modal('hide');
});


$("#btnMillSheetFinalInspectionPopup").click(function (event) {
    debugger;
    var $callFrom = $(this);
    if ($("#txtChuubanNew").val() == "" && $("#txtGyoubanNew").val() == "") {
        //Required Chuuban.
        var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ChubanRequired").text();
        showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
        return false;
    }
    else {
        var chuuban = $("#txtChuubanNew").val();
        var gyouban = $("#txtGyoubanNew").val();
        GetProcessPlanCDForFinalInspection(chuuban, gyouban, $callFrom);
    }

});


$(function () {
    $("#txtDate").datepicker({
        showOn: 'button',
        buttonImageOnly: true,
        buttonImage: '/images/calendar.gif',
        dateFormat: 'yyyy/mm/dd',

    }).attr("placeholder", "yyyy/mm/dd");
});

$("#btnCustomerNameSearch").click(function (event) {
    debugger;
    var yobina = $.trim($("#txtCustomerName").val());
    setCustomerNameList(yobina, "");
});
function setCustomerNameList(yobina, defaultSelectID) {
    setCustomerList(yobina, $("#ddlCustomerName"), defaultSelectID);
};




function setCustomerList(yobina, $target, defaultSelectID) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetCustomerList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);

            bindCustomerDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsg_COM_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindCustomerDropdown(jsonResult, $target, defaultSelectID) {
    debugger;
    $("#ddlCustomerName").html("");
    $("#ddlCustomerName").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        //$("#ddlCustomer").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $target.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

$("#btnscmbCustName").click(function (event) {
    debugger;
    var yobina = $.trim($("#txtscmbCustName").val());
    setscmbCustNameList(yobina, "");
});
function setscmbCustNameList(yobina, defaultSelectID) {
    setscmbCustList(yobina, $("#ddlscmbCustName"), defaultSelectID);
};




function setscmbCustList(yobina, $target, defaultSelectID) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetCustomerList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);

            bindscmbCustDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsg_COM_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindscmbCustDropdown(jsonResult, $target, defaultSelectID) {
    debugger;
    $("#ddlscmbCustName").html("");
    $("#ddlscmbCustName").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        //$("#ddlCustomer").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $target.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}





$("#btnMakerNameSearch").click(function (event) {
    debugger;
    var yobina = $.trim($("#txtMakerName").val());
    setMakerNameList(yobina, "");
});
function setMakerNameList(yobina, defaultSelectID) {
    setMakerList(yobina, $("#ddlMakerName"), defaultSelectID);
};
function setMakerList(yobina, $target, defaultSelectID) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/MakerList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);

            bindMakerDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsg_COM_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindMakerDropdown(jsonResult, $target, defaultSelectID) {
    debugger;
    $("#ddlMakerName").html("");
    $("#ddlMakerName").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        //$("#ddlCustomer").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $target.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

$("#btnCustomerChemSearch").click(function (event) {
    debugger;
    var yobina = $.trim($("#txtCustomerChemSearch").val());
    setCustomerChemNameList(yobina, "");
});
function setCustomerChemNameList(yobina, defaultSelectID) {
    setCustomerChemList(yobina, $("#ddlCustomerChemNameValue"), defaultSelectID);
};
function setCustomerChemList(yobina, $target, defaultSelectID) {
    var ajaxTime = new Date().getTime();

    var jsonData = "{yobina:'" + yobina + "' }";

    // var operatorCD = $target.val().trim().replace(/\D/g, ""); // this works for OPR20, 20OPR, opr20, 20opr. /\D/g is a regular expression, meaning non-digits. 

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetCustomerList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);

            bindCustomerCheDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");

            message = $.trim($("#valMsg_COM_SystemException").text());

            message = $("#lblErrorMessageE_HIKINUKIBARTOBAR_EditDelete_001").text().trim();

            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();

            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindCustomerCheDropdown(jsonResult, $target, defaultSelectID) {
    debugger;
    $("#ddlCustomerChemNameValue").html("");
    $("#ddlCustomerChemNameValue").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        //$("#ddlCustomer").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $target.append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}




function setProductQuntityList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProductQuntity",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindProductQuntityDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProductQuntityDropdown(jsonResult) {
    $("#ddlUnit").html("").append($('<option></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnit").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
    /*if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $("#ddlNGReason1").attr("id") + " option:selected").prop("selected", false);
        $("#" + $("#ddlNGReason1").attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }*/
}

function GetProcessPlanCD(chuuban, gyouban, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProcessPlanCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            ProcessPlanCD = response.message;
            if (ProcessPlanCD != 0) {
                var settings = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: APIURL + "/HSKMillSheet/GetHSKMillSheetPostHTInspectionDetails?processPlanCD=" + ProcessPlanCD,
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                }
                $.ajax(settings)
                    .done(function (response) {
                        debugger;
                        responseTime = new Date().getTime() - ajaxTime;
                        var Result = response.message;
                        if (Result.length == 1) {
                            
                            $("#lblDMF").text(Result[0].acT_DM_F);
                            $("#lblDMT").text(Result[0].acT_DM_T_AVG);
                         //   $('#PostHTModel').modal('show');
                           // GetHSKMillSheetPostHTInspectionDetails(ProcessPlanCD);
                          //  window.open("/HSKMillSheet/HSKMillSheetPostHTInspection?ProcessPlanCD=" + ProcessPlanCD, "toolbar=1, scrollbars=1, resizable=1, width=" + 806 + ", height=" + 510);
                        }
                       else if (Result.length > 1) {
                            $('#PostHTModel').modal('show');
                            GetHSKMillSheetPostHTInspectionDetails(ProcessPlanCD);
                            //  window.open("/HSKMillSheet/HSKMillSheetPostHTInspection?ProcessPlanCD=" + ProcessPlanCD, "toolbar=1, scrollbars=1, resizable=1, width=" + 806 + ", height=" + 510);
                        }
                        else {
                            $('#PostHTModel').modal('hide');
                            var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_PostHTInspecDataNotFound").text();
                            showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                            return false;
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });

            }
            else {
                var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                return false;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}


function GetHSKMillSheetPostHTInspectionDetails(processPlanCD) {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetHSKMillSheetPostHTInspectionDetails?processPlanCD=" + processPlanCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess) {
                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/HSKMillSheet/GetHSKMillSheetPostHTInspectionDetails",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        debugger;
                        var result = response1.message;
                        postHTInspectionGridData = response1.message;
                        if (response1.isSuccess) {
                            $("#divPostHTInspectionGrid").html(response1.message);

                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_SystemException").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });
            }
            else {
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + response.message + "</td></tr></table>",
                    centerVertical: true
                });
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

$("#btnPostHTSelected").click(function () {

    var row = $("input[name='radiobtn']:checked").closest("tr");

    if (row.length == 0) {
        alert("Please select a row.");
        return;
    }

    var inspectionDate = row.find("td:eq(1)").text().trim();
    var dmf = row.find("td:eq(3)").text().trim();
    var dmiAvg = row.find("td:eq(4)").text().trim();

    $("#lblDMF").text(dmf);
    $("#lblDMT").text(dmiAvg);
    $('#PostHTModel').modal('hide');
  
});




//FinalInspection
function GetProcessPlanCDForFinalInspection(chuuban, gyouban, $callFrom) {
    debugger;
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProcessPlanCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            ProcessPlanCD = response.message;
           
            if (ProcessPlanCD != 0) {
               
                $('#MillSheetFinalInspectionPopup').modal('show');
                GetFinalInspectionDetailsByProcessPlanCD(ProcessPlanCD);
            }
            else {
                var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                return false;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function GetFinalInspectionDetailsByProcessPlanCD(ProcessPlanCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetHSKMillSheetFinalInspectionDetails?processPlanCD=" + ProcessPlanCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            finalInspectionGridData = response.message;
            //debugger;
            //finalInspectionGridData = response.message;
            //if (response.isSuccess) {
            //    $("#divHSKMillSheetFinalInspectionList").html(response.message);

            //}
            if (response.isSuccess) {
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/HSKMillSheet/GetHSKMillSheetFinalInspectionDetails",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divHSKMillSheetFinalInspectionList").html(response1.message);
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function ValidateChubban(chuuban, gyouban, $callFrom) {

    debugger;
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME: '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/ValidateChubban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: gyouban })
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message

            if (result.responce == 4) {
                // Chuuban is marked as completed in the system/
                var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ENTRY_004").text();
                showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                return false;
            }

            else if (result.responce == 2) {
                // Chuuban not found in system.
                var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ENTRY_003").text();
                showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                return false;
            }
            else if (result.responce == 3) {
                // OrNo Not Started FromHandOver
                var message = $("#lblErrorMessageE_HSKMILLSHEET_ENTRY_021").text().trim().replace("99-999999-99", result.pP_CHUUBAN_GYOUBAN);
                showMessageDialog(message, "", WARNING);
                // ClearData();
                return false;
            }
            else if (result.responce == 5) {
                // OrNo Found More then two in TRN_PROCESS_PLAN PROCESS_PLAN_CMPL_DATE IS NULL
                window.open("/HSKMillSheet/HSKMillSheetChubanLiveCheck?chuuban=" + chuuban + "&gyouban=" + gyouban, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=980, height=750");
                return false;
            }

            else if (result.responce == 1) {

                GetHSKMillSheetDetailsByChuuban($("#txtChuuban").val(), $("#txtGyouban").val(), $callFrom);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function setProductQuntityList() {

    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProductQuntity",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            bindProductQuntityDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProductQuntityDropdown(jsonResult) {
    $("#ddlUnit").html("").append($('<option></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnit").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
    /*if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $("#ddlNGReason1").attr("id") + " option:selected").prop("selected", false);
        $("#" + $("#ddlNGReason1").attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }*/
}
function GetHSKMillSheetDetailsByChuuban(chuuban, gyouban, $callFrom) {
    debugger;
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME : '" + userName + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetHSKMillSheetDetailsByChuuban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfcultureName").val() })
    }

    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            setChuubanDetails(jsonResult);
            //setStartData(jsonResult);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setChuubanDetails(jsonResult) {
    debugger;
    setCustomerNameList(jsonResult.pP_YOBINA);
    setscmbCustNameList(jsonResult.pP_YOBINA);
    setMakerNameList(jsonResult.pP_MAKER_NAME);
    setCustomerChemNameList(jsonResult.pP_YOBINA);
    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#ddlCustomerName").val(jsonResult.pP_CUSTOMER_CD);
    $("#ddlscmbCustName").val(jsonResult.pP_CUSTOMER_CD);
    // $("#ddlOperatorName").val(jsonResult[0].inT_QLTY_TEST_OPERATOR_CD);
    $("#txtSpec_No").val(jsonResult.pP_PROD_SPEC_NO);
    $("#ddlMakerName").val(jsonResult.pP_MAKER_CD);

    $("#txtRowMaterial").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtHeatNo").val(jsonResult.pP_CHARGE);

    $("#txtProdNameCust").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtCondition").val(jsonResult.pP_PRODUCT_CONDITION);

    $("#txtShapInfo").val(jsonResult.pP_PROD_SHAPE);
    $("#txtSize1").val(jsonResult.pP_PROD_DIM_D);
    $("#txtSize2").val(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtMaterialDimD").val(jsonResult.pP_MATL_DIM_D);
    $("#txtQuantity").val(jsonResult.pP_NO_OF_UNITS);
    $("#ddlUnit").val(jsonResult.pP_UNIT);
  //  $("#txtMass").val(jsonResult.pP_DELIVERY_DATE_STRING);
 //   $("#txtControlNo").val(jsonResult.pP_DELIVERY_DATE_STRING);

    $("#ddlCustomerChemNameValue").val(jsonResult.pP_CUSTOMER_CD);
    $("#txtCharge").val(jsonResult.pP_CHARGE);
    $("#txtMatlNameMaker").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtDimD1").val(jsonResult.pP_MATL_DIM_D);
    $("#txtDimD2").val(jsonResult.pP_MATL_DIM_D1);
    $("#txtShap").val(jsonResult.pP_MATL_SHAPE);
    //$("#txtDate1").val(jsonResult.pP_DELIVERY_DATE_STRING);
    //$("#txtDate2").val(jsonResult.pP_DELIVERY_DATE_STRING);
    $("#txtControlNo").val(jsonResult.pP_DELIVERY_DATE_STRING);


    $("#lblvalCharge").text(jsonResult.pP_CHARGE);
    $("#lblvalMatlNameMaker").text(jsonResult.pP_MATL_NAME_MAK);
    $("#lblvalDimD").text(jsonResult.pP_MATL_DIM_D);
    $("#lblvalLength").text(jsonResult.pP_MATL_LENGTH);
   
    $("#txtChuuban").val(jsonResult.chuuban);
    $("#txtGyouban").val(jsonResult.gyoubaN_STRING);
    var m_ChuubanNew = $("#txtChuuban").val();
    var m_GyoubanNew = $("#txtGyouban").val();
    $("#txtChuubanNew").val(m_ChuubanNew);
    $("#txtGyoubanNew").val(m_GyoubanNew);
    $("#txtDetailNo1").val(1);
    m_prodCD = GetProductCD(jsonResult.pP_PROD_SPEC_NO1, jsonResult.pP_PROD_SPEC_NO2, jsonResult.pP_PROD_VER);
   


    var charge = jsonResult.pP_CHARGE;
    //var DimD = jsonResult.pP_PROD_DIM_D;
    var DimD = jsonResult.pP_MATL_DIM_D;
    if (charge != null && DimD != null) {
        GetMakerMillSheetDetailByChuban(charge, DimD);
    }


}

function GetProductCD(PROD_SPEC_NO1, pP_PROD_SPEC_NO2, PROD_VER) {
    debugger;
    // $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    // var jsonData = "{CHARGE :'" + charge + "'  , DIM_D : '" + DimD + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProductCD?ProdSpecNo1=" + PROD_SPEC_NO1 + '&ProdSpecNo2=' + pP_PROD_SPEC_NO2 + '&ProdVer=' + PROD_VER,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        //data: JSON.stringify({ CHARGE: charge, DIM_D: DimD })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            // setChuubanDetails(jsonResult);


            m_prodCD = response.message;
            $("#hdfProdCD").text(m_prodCD);
            GetProductDetails(m_prodCD);

            if (m_prodCD == "") {
                m_prodVerCondDesc = "";
            }
            else {
                m_prodVerCondDesc = GetCondDescFromProdCD(m_prodCD);
                m_prodVerCondDesc =  $("#txtCondition").text();
            }
            if (m_prodVerCondDesc == "") {
                var errorMessage = "製造仕様マスター「 m_prodSpecNo1 - m_prodSpecNo2 」のVer. prodVerNo に製品状態が登録されていません。 数値範囲　    納入状態 100-199  DB 200-299 HTDB 400-499   EG 500-599 DB 600-699 HTDB";
                showMessageDialog(errorMessage, "txtHeatNo", WARNING);
                return false;
            }
            else {
                
                $("#txtCondition").val(m_prodVerCondDesc);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}



function GetProductDetails(m_prodCD) {
    debugger;
    // $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    // var jsonData = "{CHARGE :'" + charge + "'  , DIM_D : '" + DimD + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProductDetails?Prod_CD=" + m_prodCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        //data: JSON.stringify({ CHARGE: charge, DIM_D: DimD })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            // setChuubanDetails(jsonResult);
            setProductDetails(jsonResult);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setProductDetails(jsonResult) {
    $("#txtRemarkMessage1").val(jsonResult[0].milL_SHEET_REMARK_1);
    $("#txtRemarkMessage2").val(jsonResult[0].milL_SHEET_REMARK_2);
    $("#txtRemarkMessage3").val(jsonResult[0].milL_SHEET_REMARK_3);
    $("#txtRemarkMessage4").val(jsonResult[0].milL_SHEET_REMARK_4);
    $("#txtRemarkMessage5").val(jsonResult[0].milL_SHEET_REMARK_5);
    $("#txtRemarkMessage6").val(jsonResult[0].milL_SHEET_REMARK_6);
    $("#txtRemarkMessage7").val(jsonResult[0].milL_SHEET_REMARK_7);
}


function GetCondDescFromProdCD(m_prodCD) {
    debugger;
    // $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    // var jsonData = "{CHARGE :'" + charge + "'  , DIM_D : '" + DimD + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetCondDescFromProdCD?Prod_CD=" + m_prodCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        //data: JSON.stringify({ CHARGE: charge, DIM_D: DimD })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            // setChuubanDetails(jsonResult);
            //  setProductDetails(jsonResult);
            if (result.length > 0)
            {
                m_prodVerCondDesc = result;
                $("#txtCondition").text(m_prodVerCondDesc);
            }
           

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function convertDate(dateString) {
    var parts = dateString.split("/");
    return parts[2] + parts[1] + parts[0];
}

function btnSelect_Click() {
    debugger;
     
    var CHARGE = $("#txtCharge").val();
    var MATL_NAME_MAKER = $("#txtMatlNameMaker").val();
    var DIM_D1 = $("#txtDimD1").val();
    var DIM_D2 = $("#txtDimD2").val();
    /*var SHAPE = $("#txtShap").val();*/
    var CREATED_DATE1 = $("#txtDate1").val();
    var CREATED_DATE2 = $("#txtDate2").val();

    console.log($("#txtDate1").val());
    console.log($("#txtDate1").val());
  
    
    if ($.trim(CHARGE) === "" &&
        $.trim(MATL_NAME_MAKER) === "" &&
        $.trim(DIM_D1) === "" &&
        $.trim(DIM_D2) === "" &&
        $.trim(CREATED_DATE1) === "" &&
        $.trim(CREATED_DATE2) === "") {

        var message = $("#lblErrorMessageE_SearchField").text().trim(); 
        showMessageDialog(message, "txtCharge", WARNING);
        return false;
    }
    
    //if (!checkDate($("#txtDate1"))) {

    //    var message = $("#lblErrorMessageE_SalesProd_Search_006").text().trim(); 
    //    showMessageDialog(message, "txtDate1", WARNING);
    //    return false;
    //}
    //if (!checkDate($("#txtDate2"))) {
    //    var message = $("#lblErrorMessageE_SalesProd_Search_007").text().trim(); 
    //    showMessageDialog(message, "txtDate2", WARNING);
    //    return false;
    //}

    if (CREATED_DATE1.trim() != "" && CREATED_DATE2.trim() != "") {
        var date1 = convertDate(CREATED_DATE1);
        var date2 = convertDate(CREATED_DATE2);
        if (date1 > date2) {
            var message = $("#lblErrorMessageE_DateField").text().trim();
            var focusFieldID = "txtDate2";
            showMessageDialog(message, focusFieldID, WARNING);
            return false;
        }
    }

    if ($("#txtDimD1").val().trim() == "" && $("#txtDimD2").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_DimStart").text());
        showMessageDialog(errorMessage, "txtDimD1", WARNING);
        return false;
    }

    //if ($("#txtDimD1").val().trim() != "" && $("#txtDimD2").val().trim() != "") {
    //    if (parseFloat($("#txtDimD1").val()) > parseFloat($("#txtDimD2").val())) {
    //        errorMessage = $.trim($("#lblErrorMessageE_SalesProd_Search_005").text());
    //        showMessageDialog(errorMessage, "txtDimD2", WARNING);
    //        return false;
    //    }
    //}

   
    if (MATL_NAME_MAKER.length > 0 || parseInt(CHARGE) > 0 || parseInt(DIM_D1) > 0 || parseInt(DIM_D2) > 0
        /*|| SHAPE.length > 0*/ || CREATED_DATE1.length > 0 || CREATED_DATE2.length > 0 ) {
        GetMakerMillSheetDetails(null)
    }

}


function checkDate(field) {

    var minYear = 1000;//2012
    var maxYear = 2100;


    //regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])$/
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

function GetMakerMillSheetDetails(sortFields) {
    debugger;
        
    var CHARGE = $("#txtCharge").val();
    var MATL_NAME_MAKER = $("#txtMatlNameMaker").val();
    var DIM_D1 = $("#txtDimD1").val();
    var DIM_D2 = $("#txtDimD2").val();
    /*var SHAPE = $("#txtShap").val();*/
    var CREATED_DATE1 = $("#txtDate1").val();
    var CREATED_DATE2 = $("#txtDate2").val();
    var USER_ID = $("#hdfLoginId").val();

    DIM_D1 = $.trim(DIM_D1) === "" ? null : parseFloat(DIM_D1);
    DIM_D2 = $.trim(DIM_D2) === "" ? null : parseFloat(DIM_D2);

    // Convert UI date format yyyy/MM/dd to yyyy-MM-dd
    CREATED_DATE1 = CREATED_DATE1
        ? CREATED_DATE1.replace(/\//g, "-")
        : null;

    CREATED_DATE2 = CREATED_DATE2
        ? CREATED_DATE2.replace(/\//g, "-")
        : null;

    if (MATL_NAME_MAKER.length > 0 || CHARGE.length > 0 || DIM_D1 != null
        || DIM_D2 != null  /*|| SHAPE.length > 0*/ || CREATED_DATE1.length > 0 || CREATED_DATE2.length > 0 ) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HSKMillSheet/GetMakerMillSheetDetails",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                 MATL_NAME_MAKER: MATL_NAME_MAKER, CHARGE: CHARGE, DIM_D1: DIM_D1, DIM_D2: DIM_D2,
               /* SHAPE: SHAPE,*/ CREATED_DATE1: CREATED_DATE1, CREATED_DATE2: CREATED_DATE2,
                 USER_ID: USER_ID, SORT_FIELDS: sortFields, CULTURE_NAME: $("#hdfCultureName").val()
            })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                // setChuubanDetails(jsonResult);
                //if (Object.keys(jsonResult).length > 0) {
                    //setMakerData(jsonResult);
                // }

                if (Array.isArray(jsonResult) && jsonResult.length > 0) {

                    makerMillSheetRecords = jsonResult;
                    currentMakerMillSheetIndex = 0;

                    // Display first record
                    //displayMakerMillSheetRecord();

                    // Pagination only if more than 1 record
                    if (makerMillSheetRecords.length > 1) {
                        bootbox.alert({
                            title: "<div style='color:white'></div>",

                            message: "<table width='100%'>" +
                                "<tr>" +
                                "<td width='70px'><br/>" +
                                "<img src='/Images/sucess.png' height='50px' width='50px'>" +
                                "</td>" +
                                "<td vertical-align='top'>" +
                                $.trim($("#lblErrorMessageE_MultipleMillSheets").text()) +
                                "</td>" +
                                "</tr>" +
                                "</table>",

                            buttons: {
                                ok: {
                                    label: 'OK',
                                    className: 'btn-primary'
                                }
                            },

                            centerVertical: true,

                            callback: function () {


                                displayMakerMillSheetRecord();
                                $("#makerMillSheetPagination").show();
                                // Hide empty row
                                $("#makerMillSheetEmptyRow").hide();
                                updateMakerMillSheetPagination();
                            }
                        });

                    }
                    else {
                        displayMakerMillSheetRecord();
                        $("#makerMillSheetPagination").hide();
                        // Show empty row
                        $("#makerMillSheetEmptyRow").show();
                    }

                }
                else {

                    // No records
                    makerMillSheetRecords = [];
                    currentMakerMillSheetIndex = 0;

                    displayEmptyMakerMillSheetRow();

                    // No pagination
                    $("#makerMillSheetPagination").hide();
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

    closepopup();
}

// DISPLAY CURRENT RECORD

function displayMakerMillSheetRecord() {

  

    var record =
        makerMillSheetRecords[currentMakerMillSheetIndex];

    if (!record) {
        return;
    }

    // Use existing function
    setMakerData([record]);

    // Page number
    $("#lblPageNo")
        .text(currentMakerMillSheetIndex + 1);

    // Total records
    $("#lblTotalPages")
        .text(makerMillSheetRecords.length);
}

// NEXT RECORD
// ==========================================

$("#btnNextRecord").on("click", function () {

    if (
        currentMakerMillSheetIndex <
        makerMillSheetRecords.length - 1
    ) {

        currentMakerMillSheetIndex++;

        displayMakerMillSheetRecord();

        updateMakerMillSheetPagination();
    }
});

$("#btnPrevRecord").on("click", function () {

    if (currentMakerMillSheetIndex > 0) {

        currentMakerMillSheetIndex--;

        displayMakerMillSheetRecord();

        updateMakerMillSheetPagination();
    }
});
function updateMakerMillSheetPagination() {

    if (makerMillSheetRecords.length <= 1) {
        $("#makerMillSheetPagination").hide();
        return;
    }

    $("#makerMillSheetPagination").show();

    $("#btnPrevRecord").prop(
        "disabled",
        currentMakerMillSheetIndex === 0
    );

    $("#btnNextRecord").prop(
        "disabled",
        currentMakerMillSheetIndex === makerMillSheetRecords.length - 1
    );
}

function closepopup() {
    $("#SortPopUpModal").modal('hide');
}



function GetMakerMillSheetDetailByChuban(charge, DimD) {
    debugger;
    // $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    // var jsonData = "{CHARGE :'" + charge + "'  , DIM_D : '" + DimD + "' }";

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetMakerMillSheetDetail?Charge=" + charge + '&DIM_D=' + DimD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        /* data: "{jsonData : " + jsonData + "}"*/
         //data: JSON.stringify({ CHARGE: charge, DIM_D: DimD })
        //data: JSON.stringify({
        //    CHARGE: charge,
        //    DIM_D: DimD
        //})
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            // setChuubanDetails(jsonResult);
            if (Object.keys(jsonResult).length > 0) {
                setMakerData(jsonResult);
            }
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function clearPDFViewer() {
    $("#pages").empty();

    totalPageCount = 0;

    $("#page-indicator").text("");

    console.log("PDF viewer cleared");
    console.log("Canvas count:", $("#pages canvas").length);
}

function setMakerData(jsonResult) {
    debugger;



    $("#hdfMakerMillsheetCD").val(jsonResult[0].makeR_MILL_SHEET_CD);
    $("#lblvalMillSheetNumber").text(jsonResult[0].filename);
    var pdf_Path = jsonResult[0].pdF_FILE_PATH;

    if (jsonResult[0].pdF_FILE_PATH) {

        // Clear old PDF first
        clearPDFViewer();

        DownloadAndShowPDF(jsonResult[0].pdF_FILE_PATH, jsonResult[0].filename);
        // Hide file upload
        // $("#fileUpload").hide();
        $("#fileUpload").attr("DISABLED", "DISABLED").addClass("DISABLED");
        // Show filename
        //$("#selectedFileName")
        //    .text(jsonResult.filename)
        //    .show();

        $("#page-indicator").text("1/1");
    }
    else {
        // Show file upload
        //$("#fileUpload").show();

        // Hide filename
        //  $("#selectedFileName").hide();

        // No PDF for current record
        clearPDFViewer();
    }



    setMakerChemicalSpecifcation(jsonResult)

}

function DownloadAndShowPDF(filePath, fileName) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/MakerMillSheet/DowloadAndSavePDF?filePath=" + filePath,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess == true) {
                ShowSavedPDF(response.message);
            }

        });

}
async function ShowSavedPDF(pdfPath) {

    $("#pages").html(""); // clear once

    const pdf = await pdfjsLib.getDocument(pdfPath).promise;
    // console.log("Le PDF contient", pdf.numPages, "page(s).");
    totalPageCount = pdf.numPages;

    for (let pageNum = 1; pageNum <= totalPageCount; pageNum++) {
        let page = await pdf.getPage(pageNum);
        let scale = 1.3;
        let viewport = page.getViewport({ scale: scale });

        let canvas = document.createElement("canvas");
        canvas.className = "page";
        canvas.title = "Page " + pageNum;
        canvas.height = 800;
        canvas.width = 1115;

        if (pageNum !== 1) {
            canvas.style.display = "none";
        }

        document.querySelector("#pages").appendChild(canvas);

        await page.render({
            canvasContext: canvas.getContext("2d"),
            viewport: viewport
        }).promise;

    }
}
function setMakerChemicalSpecifcation(jsonResult) {
    debugger;
    var cc1Min = '';
    var cc1Max = '';
    var ccRange = '';
    var ccAct = '';
    var ccNm = '';
    var m_ccActual = '';
    var m_ccMin = '';
    var m_ccMax = '';
    // CCName 1 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN1);
    cc1Max = (jsonResult[0].ccmaX1);
    ccNm = (jsonResult[0].ccnamE1);
    $("#LblCCNAME1").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN1").textContent = cc1Min;
        document.getElementById("lblCCMAX1").textContent = cc1Max;
        document.getElementById("lblMCC1Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN1").textContent = "MAX";
        document.getElementById("lblCCMAX1").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN1").textContent = "MIN";
        document.getElementById("lblCCMAX1").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN1").textContent = "";
        document.getElementById("lblCCMAX1").textContent = "";
    }

    if (jsonResult[0].ccrangE1 !== null) {
        $("#LblRANGE1").text("X" + jsonResult[0].ccrangE1);
    }

    if (jsonResult[0].ccactuaL1 !== null) {
        $("#lblMCCActual1").text(jsonResult[0].ccactuaL1);
    }

    // CCName 2 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN2);
    cc1Max = (jsonResult[0].ccmaX2);
    ccNm = (jsonResult[0].ccnamE2);
    $("#LblCCNAME2").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN2").textContent = cc1Min;
        document.getElementById("lblCCMAX2").textContent = cc1Max;
        document.getElementById("lblMCC2Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN2").textContent = "MAX";
        document.getElementById("lblCCMAX2").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN2").textContent = "MIN";
        document.getElementById("lblCCMAX2").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN2").textContent = "";
        document.getElementById("lblCCMAX2").textContent = "";
    }

    if (jsonResult[0].ccrangE2 !== null) {
        $("#LblRANGE2").text("X" + jsonResult[0].ccrangE2);
    }

    if (jsonResult[0].ccactuaL2 !== null) {
        $("#lblMCCActual2").text(jsonResult[0].ccactuaL2);
    }

    // CCName 3 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN3);
    cc1Max = (jsonResult[0].ccmaX3);
    ccNm = (jsonResult[0].ccnamE3);
    $("#LblCCNAME3").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN3").textContent = cc1Min;
        document.getElementById("lblCCMAX3").textContent = cc1Max;
        document.getElementById("lblMCC3Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN3").textContent = "MAX";
        document.getElementById("lblCCMAX3").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN3").textContent = "MIN";
        document.getElementById("lblCCMAX3").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN3").textContent = "";
        document.getElementById("lblCCMAX3").textContent = "";
    }

    if (jsonResult[0].ccrangE3 !== null) {
        $("#LblRANGE3").text("X" + jsonResult[0].ccrangE3);
    }

    if (jsonResult[0].ccactuaL3 !== null) {
        $("#lblMCCActual3").text(jsonResult[0].ccactuaL3);
    }


    // CCName 4 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN4);
    cc1Max = (jsonResult[0].ccmaX4);
    ccNm = (jsonResult[0].ccnamE4);
    $("#LblCCNAME4").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN4").textContent = cc1Min;
        document.getElementById("lblCCMAX4").textContent = cc1Max;
        document.getElementById("lblMCC4Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN4").textContent = "MAX";
        document.getElementById("lblCCMAX4").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN4").textContent = "MIN";
        document.getElementById("lblCCMAX4").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN4").textContent = "";
        document.getElementById("lblCCMAX4").textContent = "";
    }

    if (jsonResult[0].ccrangE4 !== null) {
        $("#LblRANGE4").text("X" + jsonResult[0].ccrangE4);
    }

    if (jsonResult[0].ccactuaL4 !== null) {
        $("#lblMCCActual4").text(jsonResult[0].ccactuaL4);
    }

    // CCName 5 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN5);
    cc1Max = (jsonResult[0].ccmaX5);
    ccNm = (jsonResult[0].ccnamE5);
    $("#LblCCNAME5").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN5").textContent = cc1Min;
        document.getElementById("lblCCMAX5").textContent = cc1Max;
        document.getElementById("lblMCC5Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN5").textContent = "MAX";
        document.getElementById("lblCCMAX5").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN5").textContent = "MIN";
        document.getElementById("lblCCMAX5").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN5").textContent = "";
        document.getElementById("lblCCMAX5").textContent = "";
    }

    if (jsonResult[0].ccrangE5 !== null) {
        $("#LblRANGE5").text("X" + jsonResult[0].ccrangE5);
    }

    if (jsonResult[0].ccactuaL5 !== null) {
        $("#lblMCCActual5").text(jsonResult[0].ccactuaL5);
    }

    // CCName 6 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN6);
    cc1Max = (jsonResult[0].ccmaX6);
    ccNm = (jsonResult[0].ccnamE6);
    $("#LblCCNAME6").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN6").textContent = cc1Min;
        document.getElementById("lblCCMAX6").textContent = cc1Max;
        document.getElementById("lblMCC6Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN6").textContent = "MAX";
        document.getElementById("lblCCMAX6").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN6").textContent = "MIN";
        document.getElementById("lblCCMAX6").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN6").textContent = "";
        document.getElementById("lblCCMAX6").textContent = "";
    }

    if (jsonResult[0].ccrangE6 !== null) {
        $("#LblRANGE6").text("X" + jsonResult[0].ccrangE6);
    }

    if (jsonResult[0].ccactuaL6 !== null) {
        $("#lblMCCActual6").text(jsonResult[0].ccactuaL6);
    }


    // CCName 7 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN7);
    cc1Max = (jsonResult[0].ccmaX7);
    ccNm = (jsonResult[0].ccnamE7);
    $("#LblCCNAME7").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN7").textContent = cc1Min;
        document.getElementById("lblCCMAX7").textContent = cc1Max;
        document.getElementById("lblMCC7Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN7").textContent = "MAX";
        document.getElementById("lblCCMAX7").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN7").textContent = "MIN";
        document.getElementById("lblCCMAX7").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN7").textContent = "";
        document.getElementById("lblCCMAX7").textContent = "";
    }

    if (jsonResult[0].ccrangE7 !== null) {
        $("#LblRANGE7").text("X" + jsonResult[0].ccrangE7);
    }

    if (jsonResult[0].ccactuaL7 !== null) {
        $("#lblMCCActual7").text(jsonResult[0].ccactuaL7);
    }


    // CCName 8 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN8);
    cc1Max = (jsonResult[0].ccmaX8);
    ccNm = (jsonResult[0].ccnamE8);
    $("#LblCCNAME8").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN8").textContent = cc1Min;
        document.getElementById("lblCCMAX8").textContent = cc1Max;
        document.getElementById("lblMCC8Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN8").textContent = "MAX";
        document.getElementById("lblCCMAX8").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN8").textContent = "MIN";
        document.getElementById("lblCCMAX8").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN8").textContent = "";
        document.getElementById("lblCCMAX8").textContent = "";
    }

    if (jsonResult[0].ccrangE8 !== null) {
        $("#LblRANGE8").text("X" + jsonResult[0].ccrangE8);
    }

    if (jsonResult[0].ccactuaL8 !== null) {
        $("#lblMCCActual8").text(jsonResult[0].ccactuaL8);
    }

    // CCName 9 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN9);
    cc1Max = (jsonResult[0].ccmaX9);
    ccNm = (jsonResult[0].ccnamE9);
    $("#LblCCNAME9").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN9").textContent = cc1Min;
        document.getElementById("lblCCMAX9").textContent = cc1Max;
        document.getElementById("lblMCC9Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN9").textContent = "MAX";
        document.getElementById("lblCCMAX9").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN9").textContent = "MIN";
        document.getElementById("lblCCMAX9").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN9").textContent = "";
        document.getElementById("lblCCMAX9").textContent = "";
    }

    if (jsonResult[0].ccrangE9 !== null) {
        $("#LblRANGE9").text("X" + jsonResult[0].ccrangE9);
    }

    if (jsonResult[0].ccactuaL9 !== null) {
        $("#lblMCCActual9").text(jsonResult[0].ccactuaL9);
    }

    // CCName 10 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN10);
    cc1Max = (jsonResult[0].ccmaX10);
    ccNm = (jsonResult[0].ccnamE10);
    $("#LblCCNAME10").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN10").textContent = cc1Min;
        document.getElementById("lblCCMAX10").textContent = cc1Max;
        document.getElementById("lblMCC10Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN10").textContent = "MAX";
        document.getElementById("lblCCMAX10").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN10").textContent = "MIN";
        document.getElementById("lblCCMAX10").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN10").textContent = "";
        document.getElementById("lblCCMAX10").textContent = "";
    }

    if (jsonResult[0].ccrangE10 !== null) {
        $("#LblRANGE10").text("X" + jsonResult[0].ccrangE10);
    }

    if (jsonResult[0].ccactuaL10 !== null) {
        $("#lblMCCActual10").text(jsonResult[0].ccactuaL10);
    }

    // CCName 11 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN11);
    cc1Max = (jsonResult[0].ccmaX11);
    ccNm = (jsonResult[0].ccnamE11);
    $("#LblCCNAME11").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN11").textContent = cc1Min;
        document.getElementById("lblCCMAX11").textContent = cc1Max;
        document.getElementById("lblMCC11Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN11").textContent = "MAX";
        document.getElementById("lblCCMAX11").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN11").textContent = "MIN";
        document.getElementById("lblCCMAX11").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN11").textContent = "";
        document.getElementById("lblCCMAX11").textContent = "";
    }

    if (jsonResult[0].ccrangE11 !== null) {
        $("#LblRANGE11").text("X" + jsonResult[0].ccrangE11);
    }

    if (jsonResult[0].ccactuaL11 !== null) {
        $("#lblMCCActual11").text(jsonResult[0].ccactuaL11);
    }

    // CCName 12 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN12);
    cc1Max = (jsonResult[0].ccmaX12);
    ccNm = (jsonResult[0].ccnamE12);
    $("#LblCCNAME12").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN12").textContent = cc1Min;
        document.getElementById("lblCCMAX12").textContent = cc1Max;
        document.getElementById("lblMCC12Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN12").textContent = "MAX";
        document.getElementById("lblCCMAX12").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN12").textContent = "MIN";
        document.getElementById("lblCCMAX12").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN12").textContent = "";
        document.getElementById("lblCCMAX12").textContent = "";
    }

    if (jsonResult[0].ccrangE12 !== null) {
        $("#LblRANGE12").text("X" + jsonResult[0].ccrangE12);
    }

    if (jsonResult[0].ccactuaL12 !== null) {
        $("#lblMCCActual12").text(jsonResult[0].ccactuaL12);
    }

    // CCName 13 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN13);
    cc1Max = (jsonResult[0].ccmaX13);
    ccNm = (jsonResult[0].ccnamE13);
    $("#LblCCNAME13").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN13").textContent = cc1Min;
        document.getElementById("lblCCMAX13").textContent = cc1Max;
        document.getElementById("lblMCC13Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN13").textContent = "MAX";
        document.getElementById("lblCCMAX13").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN13").textContent = "MIN";
        document.getElementById("lblCCMAX13").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN13").textContent = "";
        document.getElementById("lblCCMAX13").textContent = "";
    }

    if (jsonResult[0].ccrangE13 !== null) {
        $("#LblRANGE13").text("X" + jsonResult[0].ccrangE13);
    }

    if (jsonResult[0].ccactuaL13 !== null) {
        $("#lblMCCActual13").text(jsonResult[0].ccactuaL13);
    }

    // CCName 14 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN14);
    cc1Max = (jsonResult[0].ccmaX14);
    ccNm = (jsonResult[0].ccnamE14);
    $("#LblCCNAME14").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN14").textContent = cc1Min;
        document.getElementById("lblCCMAX14").textContent = cc1Max;
        document.getElementById("lblMCC14Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN14").textContent = "MAX";
        document.getElementById("lblCCMAX14").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN14").textContent = "MIN";
        document.getElementById("lblCCMAX14").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN14").textContent = "";
        document.getElementById("lblCCMAX14").textContent = "";
    }

    if (jsonResult[0].ccrangE14 !== null) {
        $("#LblRANGE14").text("X" + jsonResult[0].ccrangE14);
    }

    if (jsonResult[0].ccactuaL14 !== null) {
        $("#lblMCCActual14").text(jsonResult[0].ccactuaL14);
    }

    // CCName 15 ----------------------------------------------------------------------------------------------------------------------
    cc1Min = (jsonResult[0].ccmiN15);
    cc1Max = (jsonResult[0].ccmaX15);
    ccNm = (jsonResult[0].ccnamE15);
    $("#LblCCNAME15").text(ccNm);

    if (cc1Min !== null && cc1Max !== null) {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN15").textContent = cc1Min;
        document.getElementById("lblCCMAX15").textContent = cc1Max;
        document.getElementById("lblMCC15Minuse").style.display = "inline";

    } else if (cc1Min === null && cc1Max !== null) {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCCMIN15").textContent = "MAX";
        document.getElementById("lblCCMAX15").textContent = cc1Max;

    } else if (cc1Min !== null && cc1Max === null) {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCCMIN15").textContent = "MIN";
        document.getElementById("lblCCMAX15").textContent = cc1Min;

    } else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCCMIN15").textContent = "";
        document.getElementById("lblCCMAX15").textContent = "";
    }

    if (jsonResult[0].ccrangE15 !== null) {
        $("#LblRANGE15").text("X" + jsonResult[0].ccrangE15);
    }

    if (jsonResult[0].ccactuaL15 !== null) {
        $("#lblMCCActual15").text(jsonResult[0].ccactuaL15);
    }

}





function MakerMillSheetChemicalCompositionPopup(FileName) {
    debugger;
    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    var jsonData = "{chuuban :'" + chuuban + "'  , gyouban : '" + gyouban + "', USER_NAME : '" + userName + "' }";

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/MakerMillSheetChemicalCompositionPopup",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({ PP_CHUUBAN: chuuban, PP_GYOUBAN: gyouban, CULTURE_NAME: $("#hdfcultureName").val() })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            setChuubanDetails(jsonResult);
            // setStartData(jsonResult);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function IsMandatoryChuubanSearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#tblSearch .Mandatory1"), function (index, mandatoryField) {
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
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px' style='width:50px;height:50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/error.png'/ height='50px' width='50px'  style='width:50px;height:50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'  style='width:50px;height:50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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




$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})

function btnSortReset_Click() {
    InitSortPopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
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


        //for (var i = 0; i < selectedColumns.length; i++) {
        //    var text = $("#lstSortFields option[value='" + selectedColumns[i] + "']").text();
        //    $("#lstSortFields option[value='" + selectedColumns[i] + "']").remove();
        //    $('#lstSelectedSortFields').append($("<option></option>")
        //        .attr("value", selectedColumns[i])
        //        .text(text + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
        //}
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

function btnSortOk_Click() {
    $("#btnSort").focus();
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
    GetMakerMillSheetDetails(selectedColumns);
}
function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/HSKMillSheet/InitializeSortPopupMakerMillSheet",
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
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
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

function btnSortClose_Click() {
    $("#SortPopUpModal").modal('hide');

}
$("#btnSort").focusout(function () {
    $("#btnLnkEdit").focus();
});

$("#btnChemicalCompositionSearch").click(function (event) {
    debugger;
    var $callFrom = $(this);
    if ($("#txtHeatNo").val() == "") {
        //Required Chuuban.
        var errorMessage = "この注番に関するミルシートがありません。";
        showMessageDialog(errorMessage, "txtHeatNo", WARNING);
        return false;
    }
    else {
        var Charge = $("#txtHeatNo").val();

        GetChemicalComposition(Charge, $callFrom);
    }

});
function GetChemicalComposition(Charge, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var m_custNameCD = $("#ddlCustomerName").val();
    var m_prodNameCust = $("#txtProdNameCust").val();
    var charge = $("#txtHeatNo").val();
    var fileName = $("#lblvalMillSheetNumber").text();
    var m_MakerMillsheetCD = $("#hdfMakerMillsheetCD").val();

    if (m_prodNameCust == "") {
        //Required Chuuban.
        var errorMessage = "鋼種名とミルシートの転記の為の得意先を入力して下さい。";
        showMessageDialog(errorMessage, "txtProdNameCust", WARNING);
        return false;
    }
    else {
        var jsonData = "{ CUSTOMER_CD :'" + m_custNameCD + "'  , prodnamecust : '" + m_prodNameCust + "', USER_NAME: '" + userName + "' }";

        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HSKMillSheet/GetHSKChemicalCompositionDetails",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                var jsonResult = response.message;

                if (!jsonResult || jsonResult.length === 0) {
                    var errorMessage = "鋼種名 （" + m_prodNameCust + "） に該当する化学成分規格が存在しない為、メーカーミルシートの成績値を転記する事が出来ませんでした。 品質保証に連絡して化学成分規格マスターに化学成分規格を追加するように依頼して下さい。";
                    showMessageDialog(errorMessage, "txtProdNameCust", WARNING);
                    return false;
                }
                if (Object.keys(jsonResult).length > 0) {
                    if (jsonResult[0].producT_NAME_CUST == null) {

                        var errorMessage = "転記指示した得意先専用の化学成分規格が存在しました。社内規格は共通の化学成分規格を使用していません。";
                        showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                        return false;
                    }
                    else {

                        var jsonData = "{ CUSTOMER_CD :'" + m_custNameCD + "'  , prodnamecust : '" + m_prodNameCust + "', USER_NAME: '" + userName + "' }";

                        var settings = {
                            async: true,
                            crossDomain: true,
                            cache: false,
                            dataType: "json",
                            url: APIURL + "/HSKMillSheet/GetHSKChemicalCompositionAandMakerDetails",
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust, MAKER_MILL_SHEET_CD: m_MakerMillsheetCD })
                        }
                        $.ajax(settings)
                            .done(function (response) {
                                responseTime = new Date().getTime() - ajaxTime;
                                debugger;
                                var jsonResult = response.message;
                                if (Object.keys(jsonResult).length > 0) {

                                    setChemicalCompositionSpecifcation(jsonResult);

                                }
                                else { 
                                var errorMessage = "素材鋼種が、化学成分マスターに登録されていません。";
                                showMessageDialog(errorMessage, "txtProdNameCust", WARNING);
                                    return false;
                                }

                            })
                            .fail(function (xhr, textStatus, errorThrown) {
                                responseTime = new Date().getTime() - ajaxTime;
                                var errorCode = $.parseJSON(xhr.responseText).Message;

                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
                                showMessageDialog(message, focusFieldID, ERROR);
                            });


                    }

                }


                else {
                    var errorMessage = "素材鋼種が、化学成分マスターに登録されていません。";
                    showMessageDialog(errorMessage, "txtProdNameCust", WARNING);
                    return false;
                }

                
                //if (ProcessPlanCD != 0) {


                //}
                //else {
                //    var errorMessage = $("#lblErrorMessageE_HSKMILLSHEET_ENTRY_003").text();
                //    showMessageDialog(errorMessage, "txtChuubanNew", WARNING);
                //    return false;
                //}

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });

    }



}

function setChemicalCompositionSpecifcation(jsonResult) {

    SetChemicalCompCC1Data(jsonResult)
    SetChemicalCompCC2Data(jsonResult)
    SetChemicalCompCC3Data(jsonResult)
    SetChemicalCompCC4Data(jsonResult)
    SetChemicalCompCC5Data(jsonResult)
    SetChemicalCompCC6Data(jsonResult)
    SetChemicalCompCC7Data(jsonResult)
    SetChemicalCompCC8Data(jsonResult)
    SetChemicalCompCC9Data(jsonResult)
    SetChemicalCompCC10Data(jsonResult)
    SetChemicalCompCC11Data(jsonResult)
    SetChemicalCompCC12Data(jsonResult)
    SetChemicalCompCC13Data(jsonResult)
    SetChemicalCompCC14Data(jsonResult)
    SetChemicalCompCC15Data(jsonResult)



}
function SetChemicalCompCC1Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN1;
    cc1Max = jsonResult[0].ccmaX1;
    ccNm = jsonResult[0].ccnamE1;
    $("#lblCheCCName1").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN1").textContent = cc1Min;
        document.getElementById("lblCheCCMAX1").textContent = cc1Max;
        document.getElementById("lblCheMCC1Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN1").textContent = "MAX";
        document.getElementById("lblCheCCMAX1").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN1").textContent = "MIN";
        document.getElementById("lblCheCCMAX1").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN1").textContent = "";
        document.getElementById("lblCheCCMAX1").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE1;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange1").text("X" + jsonResult[0].ccrangE1);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual1").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN1").textContent = cc1Min;
        document.getElementById("lblCheCCMAX1").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN1").textContent = "MAX";
        document.getElementById("lblCheCCMAX1").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN1").textContent = "MIN";
        document.getElementById("lblCheCCMAX1").textContent = cc1Min;
    }

    //'Added  by vinod for set m_actualResultInrange value -CR 2021-06-16 on 
    //    If m_actualResultInrange = "1" Then
    //'No Change and No check required 
    //Else
    //        If ActualResultInRange(cc1Min, cc1Max, ccAct) Then
    //m_actualResultInrange = "0"
    //Else
    //m_actualResultInrange = "1"
    //        End If
    //    End If


}
function SetChemicalCompCC2Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN2;
    cc1Max = jsonResult[0].ccmaX2;
    ccNm = jsonResult[0].ccnamE2;
    $("#lblCheCCName2").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }


    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN2").textContent = cc1Min;
        document.getElementById("lblCheCCMAX2").textContent = cc1Max;
        document.getElementById("lblCheMCC2Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN2").textContent = "MAX";
        document.getElementById("lblCheCCMAX2").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN2").textContent = "MIN";
        document.getElementById("lblCheCCMAX2").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN2").textContent = "";
        document.getElementById("lblCheCCMAX2").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE2;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange2").text("X" + jsonResult[0].ccrangE2);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual2").textContent = m_ccActual;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN2").textContent = cc1Min;
        document.getElementById("lblCheCCMAX2").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN2").textContent = "MAX";
        document.getElementById("lblCheCCMAX2").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN2").textContent = "MIN";
        document.getElementById("lblCheCCMAX2").textContent = cc1Min;
    }

    //'Added  by vinod for set m_actualResultInrange value -CR 2021-06-16 on 
    //    If m_actualResultInrange = "1" Then
    //'No Change and No check required 
    //Else
    //        If ActualResultInRange(cc1Min, cc1Max, ccAct) Then
    //m_actualResultInrange = "0"
    //Else
    //m_actualResultInrange = "1"
    //        End If
    //    End If


}
function SetChemicalCompCC3Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN3;
    cc1Max = jsonResult[0].ccmaX3;
    ccNm = jsonResult[0].ccnamE3;
    $("#lblCheCCName3").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN3").textContent = cc1Min;
        document.getElementById("lblCheCCMAX3").textContent = cc1Max;
        document.getElementById("lblCheMCC3Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN3").textContent = "MAX";
        document.getElementById("lblCheCCMAX3").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN3").textContent = "MIN";
        document.getElementById("lblCheCCMAX3").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN3").textContent = "";
        document.getElementById("lblCheCCMAX3").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE3;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange3").text("X" + jsonResult[0].ccrangE3);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual3").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN3").textContent = cc1Min;
        document.getElementById("lblCheCCMAX3").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN3").textContent = "MAX";
        document.getElementById("lblCheCCMAX3").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN3").textContent = "MIN";
        document.getElementById("lblCheCCMAX3").textContent = cc1Min;
    }

    //'Added  by vinod for set m_actualResultInrange value -CR 2021-06-16 on 
    //    If m_actualResultInrange = "1" Then
    //'No Change and No check required 
    //Else
    //        If ActualResultInRange(cc1Min, cc1Max, ccAct) Then
    //m_actualResultInrange = "0"
    //Else
    //m_actualResultInrange = "1"
    //        End If
    //    End If


}

function SetChemicalCompCC4Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";

    ccRange = "";
    cc1Min = jsonResult[0].ccmiN4;
    cc1Max = jsonResult[0].ccmaX4;
    ccNm = jsonResult[0].ccnamE4;
    $("#lblCheCCName4").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN4").textContent = cc1Min;
        document.getElementById("lblCheCCMAX4").textContent = cc1Max;
        document.getElementById("lblCheMCC4Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN4").textContent = "MAX";
        document.getElementById("lblCheCCMAX4").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN4").textContent = "MIN";
        document.getElementById("lblCheCCMAX4").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN4").textContent = "";
        document.getElementById("lblCheCCMAX4").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE4;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange4").text("X" + jsonResult[0].ccrangE4);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual4").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN4").textContent = cc1Min;
        document.getElementById("lblCheCCMAX4").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN4").textContent = "MAX";
        document.getElementById("lblCheCCMAX4").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN4").textContent = "MIN";
        document.getElementById("lblCheCCMAX4").textContent = cc1Min;
    }




}
function SetChemicalCompCC5Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN5;
    cc1Max = jsonResult[0].ccmaX5;
    ccNm = jsonResult[0].ccnamE5;
    $("#lblCheCCName5").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN5").textContent = cc1Min;
        document.getElementById("lblCheCCMAX5").textContent = cc1Max;
        document.getElementById("lblCheMCC5Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN5").textContent = "MAX";
        document.getElementById("lblCheCCMAX5").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN5").textContent = "MIN";
        document.getElementById("lblCheCCMAX5").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN5").textContent = "";
        document.getElementById("lblCheCCMAX5").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE5;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange5").text("X" + jsonResult[0].ccrangE5);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual5").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN5").textContent = cc1Min;
        document.getElementById("lblCheCCMAX5").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN5").textContent = "MAX";
        document.getElementById("lblCheCCMAX5").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN5").textContent = "MIN";
        document.getElementById("lblCheCCMAX5").textContent = cc1Min;
    }




}
function SetChemicalCompCC6Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN6;
    cc1Max = jsonResult[0].ccmaX6;
    ccNm = jsonResult[0].ccnamE6;
    $("#lblCheCCName6").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN6").textContent = cc1Min;
        document.getElementById("lblCheCCMAX6").textContent = cc1Max;
        document.getElementById("lblCheMCC6Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN6").textContent = "MAX";
        document.getElementById("lblCheCCMAX6").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN6").textContent = "MIN";
        document.getElementById("lblCheCCMAX6").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN6").textContent = "";
        document.getElementById("lblCheCCMAX6").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE6;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange6").text("X" + jsonResult[0].ccrangE6);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual6").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN6").textContent = cc1Min;
        document.getElementById("lblCheCCMAX6").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN6").textContent = "MAX";
        document.getElementById("lblCheCCMAX6").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN6").textContent = "MIN";
        document.getElementById("lblCheCCMAX6").textContent = cc1Min;
    }




}
function SetChemicalCompCC7Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN7;
    cc1Max = jsonResult[0].ccmaX7;
    ccNm = jsonResult[0].ccnamE7;
    $("#lblCheCCName7").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN7").textContent = cc1Min;
        document.getElementById("lblCheCCMAX7").textContent = cc1Max;
        document.getElementById("lblCheMCC7Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN7").textContent = "MAX";
        document.getElementById("lblCheCCMAX7").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN7").textContent = "MIN";
        document.getElementById("lblCheCCMAX7").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN7").textContent = "";
        document.getElementById("lblCheCCMAX7").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE7;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange7").text("X" + jsonResult[0].ccrangE7);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual7").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN7").textContent = cc1Min;
        document.getElementById("lblCheCCMAX7").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN7").textContent = "MAX";
        document.getElementById("lblCheCCMAX7").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN7").textContent = "MIN";
        document.getElementById("lblCheCCMAX7").textContent = cc1Min;
    }




}
function SetChemicalCompCC8Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN8;
    cc1Max = jsonResult[0].ccmaX8;
    ccNm = jsonResult[0].ccnamE8;
    $("#lblCheCCName8").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN8").textContent = cc1Min;
        document.getElementById("lblCheCCMAX8").textContent = cc1Max;
        document.getElementById("lblCheMCC8Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN8").textContent = "MAX";
        document.getElementById("lblCheCCMAX8").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN8").textContent = "MIN";
        document.getElementById("lblCheCCMAX8").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN8").textContent = "";
        document.getElementById("lblCheCCMAX8").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE8;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange8").text("X" + jsonResult[0].ccrangE8);

        // m_CC1_MUL = ccRange
    }

    document.getElementById("lblCheMCCActual8").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN8").textContent = cc1Min;
        document.getElementById("lblCheCCMAX8").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN8").textContent = "MAX";
        document.getElementById("lblCheCCMAX8").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN8").textContent = "MIN";
        document.getElementById("lblCheCCMAX8").textContent = cc1Min;
    }




}
function SetChemicalCompCC9Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN9;
    cc1Max = jsonResult[0].ccmaX9;
    ccNm = jsonResult[0].ccnamE9;
    $("#lblCheCCName9").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();

    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }

    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN9").textContent = cc1Min;
        document.getElementById("lblCheCCMAX9").textContent = cc1Max;
        document.getElementById("lblCheMCC9Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN9").textContent = "MAX";
        document.getElementById("lblCheCCMAX9").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN9").textContent = "MIN";
        document.getElementById("lblCheCCMAX9").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN9").textContent = "";
        document.getElementById("lblCheCCMAX9").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE9;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange9").text("X" + jsonResult[0].ccrangE9);

        // m_CC1_MUL = ccRange
    }

    document.getElementById("lblCheMCCActual9").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN9").textContent = cc1Min;
        document.getElementById("lblCheCCMAX9").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN9").textContent = "MAX";
        document.getElementById("lblCheCCMAX9").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN9").textContent = "MIN";
        document.getElementById("lblCheCCMAX9").textContent = cc1Min;
    }




}
function SetChemicalCompCC10Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN10;
    cc1Max = jsonResult[0].ccmaX10;
    ccNm = jsonResult[0].ccnamE10;
    $("#lblCheCCName10").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN10").textContent = cc1Min;
        document.getElementById("lblCheCCMAX10").textContent = cc1Max;
        document.getElementById("lblCheMCC10Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN10").textContent = "MAX";
        document.getElementById("lblCheCCMAX10").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN10").textContent = "MIN";
        document.getElementById("lblCheCCMAX10").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN10").textContent = "";
        document.getElementById("lblCheCCMAX10").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE10;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange10").text("X" + jsonResult[0].ccrangE10);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual10").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN10").textContent = cc1Min;
        document.getElementById("lblCheCCMAX10").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN10").textContent = "MAX";
        document.getElementById("lblCheCCMAX10").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN10").textContent = "MIN";
        document.getElementById("lblCheCCMAX10").textContent = cc1Min;
    }

    //'Added  by vinod for set m_actualResultInrange value -CR 2021-06-16 on 
    //    If m_actualResultInrange = "1" Then
    //'No Change and No check required 
    //Else
    //        If ActualResultInRange(cc1Min, cc1Max, ccAct) Then
    //m_actualResultInrange = "0"
    //Else
    //m_actualResultInrange = "1"
    //        End If
    //    End If


}
function SetChemicalCompCC11Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN11;
    cc1Max = jsonResult[0].ccmaX11;
    ccNm = jsonResult[0].ccnamE11;
    $("#lblCheCCName11").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN11").textContent = cc1Min;
        document.getElementById("lblCheCCMAX11").textContent = cc1Max;
        document.getElementById("lblCheMCC11Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN11").textContent = "MAX";
        document.getElementById("lblCheCCMAX11").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN11").textContent = "MIN";
        document.getElementById("lblCheCCMAX11").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN11").textContent = "";
        document.getElementById("lblCheCCMAX11").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE11;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange11").text("X" + jsonResult[0].ccrangE11);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual11").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN11").textContent = cc1Min;
        document.getElementById("lblCheCCMAX11").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN11").textContent = "MAX";
        document.getElementById("lblCheCCMAX11").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN11").textContent = "MIN";
        document.getElementById("lblCheCCMAX11").textContent = cc1Min;
    }



}

function SetChemicalCompCC12Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN12;
    cc1Max = jsonResult[0].ccmaX12;
    ccNm = jsonResult[0].ccnamE12;
    $("#lblCheCCName12").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN12").textContent = cc1Min;
        document.getElementById("lblCheCCMAX12").textContent = cc1Max;
        document.getElementById("lblCheMCC12Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN12").textContent = "MAX";
        document.getElementById("lblCheCCMAX12").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN12").textContent = "MIN";
        document.getElementById("lblCheCCMAX12").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN12").textContent = "";
        document.getElementById("lblCheCCMAX12").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE12;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange12").text("X" + jsonResult[0].ccrangE12);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual12").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN12").textContent = cc1Min;
        document.getElementById("lblCheCCMAX12").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN12").textContent = "MAX";
        document.getElementById("lblCheCCMAX12").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN12").textContent = "MIN";
        document.getElementById("lblCheCCMAX12").textContent = cc1Min;
    }




}
function SetChemicalCompCC13Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN13;
    cc1Max = jsonResult[0].ccmaX13;
    ccNm = jsonResult[0].ccnamE13;
    $("#lblCheCCName13").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN13").textContent = cc1Min;
        document.getElementById("lblCheCCMAX13").textContent = cc1Max;
        document.getElementById("lblCheMCC13Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN13").textContent = "MAX";
        document.getElementById("lblCheCCMAX13").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN13").textContent = "MIN";
        document.getElementById("lblCheCCMAX13").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN13").textContent = "";
        document.getElementById("lblCheCCMAX13").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE13;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange13").text("X" + jsonResult[0].ccrangE13);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual13").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN13").textContent = cc1Min;
        document.getElementById("lblCheCCMAX13").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN13").textContent = "MAX";
        document.getElementById("lblCheCCMAX13").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN13").textContent = "MIN";
        document.getElementById("lblCheCCMAX13").textContent = cc1Min;
    }




}

function SetChemicalCompCC14Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN14;
    cc1Max = jsonResult[0].ccmaX14;
    ccNm = jsonResult[0].ccnamE14;
    $("#lblCheCCName14").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN14").textContent = cc1Min;
        document.getElementById("lblCheCCMAX14").textContent = cc1Max;
        document.getElementById("lblCheMCC14Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN14").textContent = "MAX";
        document.getElementById("lblCheCCMAX14").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN14").textContent = "MIN";
        document.getElementById("lblCheCCMAX14").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN14").textContent = "";
        document.getElementById("lblCheCCMAX14").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE14;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }


    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange14").text("X" + jsonResult[0].ccrangE14);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual14").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN14").textContent = cc1Min;
        document.getElementById("lblCheCCMAX14").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN14").textContent = "MAX";
        document.getElementById("lblCheCCMAX14").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN14").textContent = "MIN";
        document.getElementById("lblCheCCMAX14").textContent = cc1Min;
    }

    //'Added  by vinod for set m_actualResultInrange value -CR 2021-06-16 on 
    //    If m_actualResultInrange = "1" Then
    //'No Change and No check required 
    //Else
    //        If ActualResultInRange(cc1Min, cc1Max, ccAct) Then
    //m_actualResultInrange = "0"
    //Else
    //m_actualResultInrange = "1"
    //        End If
    //    End If


}

function SetChemicalCompCC15Data(jsonResult) {

    let cc1Min = "";
    let cc1Max = "";
    ccRange = "";

    cc1Min = jsonResult[0].ccmiN15;
    cc1Max = jsonResult[0].ccmaX15;
    ccNm = jsonResult[0].ccnamE15;
    $("#lblCheCCName15").text(ccNm);
    // m_NM1 = $("#lblCheCCName1").text();
    if (cc1Min === 0) {
        cc1Min = "";
    }
    if (cc1Max === 0) {
        cc1Max = "";
    }
    if (cc1Min !== "" && cc1Max !== "") {
        m_ccMin = cc1Min;
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN15").textContent = cc1Min;
        document.getElementById("lblCheCCMAX15").textContent = cc1Max;
        document.getElementById("lblCheMCC15Minuse").style.display = "inline";

    }
    else if (cc1Min === "" && cc1Max !== "") {
        m_ccMin = "";
        m_ccMax = cc1Max;
        document.getElementById("lblCheCCMIN15").textContent = "MAX";
        document.getElementById("lblCheCCMAX15").textContent = cc1Max;
    }
    else if (cc1Min !== "" && cc1Max === "") {
        m_ccMin = cc1Min;
        m_ccMax = "";
        document.getElementById("lblCheCCMIN15").textContent = "MIN";
        document.getElementById("lblCheCCMAX15").textContent = cc1Min;
    }
    else {
        m_ccMin = "";
        m_ccMax = "";
        document.getElementById("lblCheCCMIN15").textContent = "";
        document.getElementById("lblCheCCMAX15").textContent = "";
    }

    ccRange = jsonResult[0].ccrangE15;



    if (ccRange !== null && ccRange !== undefined && String(ccRange).trim() !== "") {
        const parsed = parseInt(ccRange);
        if (!isNaN(parsed)) {
            m_hskMultiper = parsed;
        }
    }
    else {
        m_hskMultiper = null;
    }

    MacthingChemCompoSetMakerActResMul(ccNm, jsonResult)
    // lblCCActual1.Text = ccAct


    if (ccRange !== null && ccRange !== "0") {
        $("#lblCheRange15").text("X" + jsonResult[0].ccrangE15);

        // m_CC1_MUL = ccRange
    }



    document.getElementById("lblCheMCCActual15").textContent = ccAct;

    if (m_ccMin !== "" && m_ccMax !== "") {

        document.getElementById("lblCheCCMIN15").textContent = cc1Min;
        document.getElementById("lblCheCCMAX15").textContent = cc1Max;

    }
    else if (m_ccMin === "" && m_ccMax !== "") {
        document.getElementById("lblCheCCMIN15").textContent = "MAX";
        document.getElementById("lblCheCCMAX15").textContent = cc1Max;
    }
    else if (m_ccMin !== "" && m_ccMax === "") {
        document.getElementById("lblCheCCMIN15").textContent = "MIN";
        document.getElementById("lblCheCCMAX15").textContent = cc1Min;
    }

    //'Added  by vinod for set m_actualResultInrange value -CR 2021-06-16 on 
    //    If m_actualResultInrange = "1" Then
    //'No Change and No check required 
    //Else
    //        If ActualResultInRange(cc1Min, cc1Max, ccAct) Then
    //m_actualResultInrange = "0"
    //Else
    //m_actualResultInrange = "1"
    //        End If
    //    End If


}

function MacthingChemCompoSetMakerActResMul(ccNm, jsonResult) {
    m_ccActual = "";
    ccAct = "";

    if (ccNm == jsonResult[1].ccnamE1) {

        if (jsonResult[1].ccactuaL1 !== null) {

            if (jsonResult[0].ccrangE1 !== null && jsonResult[0].ccrangE1 !== undefined && String(jsonResult[0].ccrangE1).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE1);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL1)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE2) {

        if (jsonResult[1].ccactuaL2 !== null) {

            if (jsonResult[0].ccrangE2 !== null && jsonResult[0].ccrangE2 !== undefined && String(jsonResult[0].ccrangE2).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE2);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL2)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE3) {

        if (jsonResult[1].ccactuaL3 !== null) {

            if (jsonResult[0].ccrangE3 !== null && jsonResult[0].ccrangE3 !== undefined && String(jsonResult[0].ccrangE3).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE3);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL3)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE4) {

        if (jsonResult[1].ccactuaL4 !== null) {

            if (jsonResult[0].ccrangE4 !== null && jsonResult[0].ccrangE4 !== undefined && String(jsonResult[0].ccrangE4).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE4);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL4)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }

    else if (ccNm == jsonResult[1].ccnamE5) {

        if (jsonResult[1].ccactuaL5 !== null) {

            if (jsonResult[0].ccrangE5 !== null && jsonResult[0].ccrangE5 !== undefined && String(jsonResult[0].ccrangE5).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE5);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL5)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE6) {

        if (jsonResult[1].ccactuaL6 !== null) {

            if (jsonResult[0].ccrangE6 !== null && jsonResult[0].ccrangE6 !== undefined && String(jsonResult[0].ccrangE6).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE6);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL6)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE7) {

        if (jsonResult[1].ccactuaL7 !== null) {

            if (jsonResult[0].ccrangE7 !== null && jsonResult[0].ccrangE7 !== undefined && String(jsonResult[0].ccrangE7).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE7);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL7)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }

    else if (ccNm == jsonResult[1].ccnamE8) {

        if (jsonResult[1].ccactuaL8 !== null) {

            if (jsonResult[0].ccrangE8 !== null && jsonResult[0].ccrangE8 !== undefined && String(jsonResult[0].ccrangE8).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE8);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL8)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE9) {

        if (jsonResult[1].ccactuaL9 !== null) {

            if (jsonResult[0].ccrangE9 !== null && jsonResult[0].ccrangE9 !== undefined && String(jsonResult[0].ccrangE9).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE9);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL9)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE10) {

        if (jsonResult[1].ccactuaL10 !== null) {

            if (jsonResult[0].ccrangE10 !== null && jsonResult[0].ccrangE10 !== undefined && String(jsonResult[0].ccrangE10).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE10);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL10)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE11) {

        if (jsonResult[1].ccactuaL11 !== null) {

            if (jsonResult[0].ccrangE11 !== null && jsonResult[0].ccrangE11 !== undefined && String(jsonResult[0].ccrangE11).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE11);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL11)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE12) {

        if (jsonResult[1].ccactuaL12 !== null) {

            if (jsonResult[0].ccrangE12 !== null && jsonResult[0].ccrangE12 !== undefined && String(jsonResult[0].ccrangE12).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE12);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL12)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE13) {

        if (jsonResult[1].ccactuaL13 !== null) {

            if (jsonResult[0].ccrangE13 !== null && jsonResult[0].ccrangE13 !== undefined && String(jsonResult[0].ccrangE13).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE13);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL13)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE14) {

        if (jsonResult[1].ccactuaL14 !== null) {

            if (jsonResult[0].ccrangE14 !== null && jsonResult[0].ccrangE14 !== undefined && String(jsonResult[0].ccrangE14).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE14);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL14)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
    else if (ccNm == jsonResult[1].ccnamE15) {

        if (jsonResult[1].ccactuaL15 !== null) {

            if (jsonResult[0].ccrangE15 !== null && jsonResult[0].ccrangE15 !== undefined && String(jsonResult[0].ccrangE15).trim() !== "") {
                const parsed = parseInt(jsonResult[1].ccrangE15);
                if (!isNaN(parsed)) {
                    m_mkrMultiper = parsed;
                }
                SetNewResult(m_hskMultiper, m_mkrMultiper, jsonResult[1].ccactuaL15)
            }
            if (m_hskMultiper == null) {
                ccRange = "";
                ccAct = "";
            }
            else {
                ccRange = m_hskMultiper;
                ccAct = m_ccActual;
            }
        }
        else {

            ccRange = m_hskMultiper;
            ccAct = m_ccActual;
        }

    }
}

function SetNewResult(m_hskMultiper, m_mkrMultiper, actualVal) {
    var actRes = "";
    var ccMin = "";
    var ccMax = "";
    var hskActRes1 = "";
    let hskActRes = "";
    var hskActRes1 = "";
    let hskActResDEC1 = "";
    var hskActRes2 = "";

    if (m_ccMin !== null) {
        ccMin = m_ccMin;
    }
    if (m_ccMin !== null) {
        ccMax = m_ccMax;
    }

    if (m_hskMultiper == m_mkrMultiper) {

        actRes = actualVal;
        m_ccMin = ccMin;
        m_ccMax = ccMax;
    }
    else if (actualVal !== null && actualVal !== "0" && m_mkrMultiper !== 0) {
        // numeric

        if (/^-?\d+(\.\d+)?$/.test(actualVal)) {


            hskActRes = (Number(actualVal) / Number(m_mkrMultiper) * Number(m_hskMultiper));
            hskActRes1 = hskActRes.toString().trim();
            // Round decimal value
            hskActRes2 = Math.round(Number(hskActRes1));
            actRes = hskActRes2.toString();


            actRes = Math.round((Number(actualVal) / Number(m_mkrMultiper) * Number(m_hskMultiper))).toString();



        }
        else {
            let newNumericActualResultValue = actualVal.substring(1);
            let NewResultValueSign = actualVal.substring(0, 1);

            if (Number.isFinite(Number(newNumericActualResultValue)) && (NewResultValueSign === "<" || NewResultValueSign === ">")) {
                let hskActRes = (Number(newNumericActualResultValue) / Number(m_mkrMultiper)) * Number(m_hskMultiper);

                let hskActRes1 = NewResultValueSign + hskActRes.toString().trim();

                // Extract numeric part again for rounding
                let hskActResDEC1 = Number(hskActRes1.substring(1));

                let hskActRes2 = NewResultValueSign + Math.round(hskActResDEC1).toString();

                actRes = hskActRes2;
            }
            else {
                actRes = actualVal.toString().trim();
                m_MakerActualResultNonNumericList.push(ccNm.toString().trim());
            }
        }
    }
    else {
        actRes = actualVal.toString().trim();

    }

    m_ccActual = actRes;
    m_ccMin = ccMin;
    m_ccMax = ccMax;

}
$("#btnHardnessPattern1").click(function (event) {
    debugger;
    var $callFrom = $(this);
    if ($("#txtChuubanNew").val() == "") {
        //Required Chuuban.
        var errorMessage = $("内質検査・機械検査を検索する為の注番を入力して下さい。").text();
        showMessageDialog(errorMessage, "txtHeatNo", WARNING);
        return false;
    }
    if ($("#txtGyoubanNew").val() == "") {
        //Required Chuuban.
        var errorMessage = $("内質検査・機械検査を検索する為の注番を入力して下さい。").text();
        showMessageDialog(errorMessage, "txtHeatNo", WARNING);
        return false;
    }
    else {
        var m_ChuubanNew = $("#txtChuubanNew").val();
        var m_GyoubanNew = $("#txtGyoubanNew").val();
        var m_chargeNo = $("#txtHeatNo").val();
        var m_mateDimD = $("#txtHeatNo").val();
        var m_mateNameMaker = $("#txtHeatNo").val();


        GetProdOrderNoInspectionHardness(m_ChuubanNew, m_GyoubanNew, m_chargeNo, m_mateDimD, m_mateNameMaker, $callFrom);

        if (m_prodOrderNoInspection == 0) {

            m_prodOrderNoInspection = GetProdOrderNoInspectionTensile(m_ChuubanNew, m_GyoubanNew, m_chargeNo, m_mateDimD, m_mateNameMaker, $callFrom);
        }


        if (m_prodOrderNoInspection > 0) {


            let detailNoList = [];

            if ($("#txtDetailNo1").val() !== "") {
                detailNoList.push($("#txtDetailNo1").val());
            }
            if ($("#txtDetailNo2").val() !== "") {
                detailNoList.push($("#txtDetailNo2").val());
            }
            if ($("#txtDetailNo3").val() !== "") {
                detailNoList.push($("#txtDetailNo3").val());
            }
            if ($("#txtDetailNo4").val() !== "") {
                detailNoList.push($("#txtDetailNo4").val());
            }
            if ($("#txtDetailNo5").val() !== "") {
                detailNoList.push($("#txtDetailNo5").val());
            }
            if ($("#txtDetailNo6").val() !== "") {
                detailNoList.push($("#txtDetailNo6").val());
            }
            if ($("#txtDetailNo7").val() !== "") {
                detailNoList.push($("#txtDetailNo7").val());
            }

            if (detailNoList.length > 0) {

                SetHDPatternDataWithProdOrderNoInspection(m_prodOrderNoInspection);
            }
            else {

                //  Utils.ShowMessage(Utils.GetPMSResource("lbl_DetailNo") + plsEnter, MessageTypesEnum.ValidationError)

                var errorMessage = $("Please Enter Detail No").text();
                showMessageDialog(errorMessage, "txtDetailNo1", WARNING);
                return false;
            }




        }
        else {


            var errorMessage = $(" 注番・チャージで一致する機械検査の検査データが存在しません。").text();
            showMessageDialog(errorMessage, "txtHeatNo", WARNING);
            return false;
        }

    }

});

function GetProdOrderNoInspectionHardness(m_ChuubanNew, m_GyoubanNew, m_chargeNo, m_mateDimD, m_mateNameMaker, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProdOrderNoInspectionHardness",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CHUUBAN: m_ChuubanNew, GYOUBAN: m_GyoubanNew, CHARGE: m_chargeNo, MATDIMD: m_mateDimD, MATNAMEMAKER: m_mateNameMaker })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            m_prodOrderNoInspection = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetProdOrderNoInspectionTensile(m_ChuubanNew, m_GyoubanNew, m_chargeNo, m_mateDimD, m_mateNameMaker, $callFrom) {

    $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProdOrderNoInspectionTensile",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CHUUBAN: m_ChuubanNew, GYOUBAN: m_GyoubanNew, CHARGE: m_chargeNo, MATDIMD: m_mateDimD, MATNAMEMAKER: m_mateNameMaker })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;
            if (jsonResult.length > 0) {
                m_prodOrderNoInspection = jsonResult;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function SetHDPatternDataWithProdOrderNoInspection(m_prodOrderNoInspection) {

    HardnessPattern1DetailNo1Data(m_prodOrderNoInspection);
    //HardnessPattern1DetailNo2Data(m_prodOrderNoInspection)
    //HardnessPattern1DetailNo3Data(m_prodOrderNoInspection)
    //HardnessPattern1DetailNo4Data(m_prodOrderNoInspection)
    //HardnessPattern1DetailNo5Data(m_prodOrderNoInspection)
    //HardnessPattern1DetailNo6Data(m_prodOrderNoInspection)
    //HardnessPattern1DetailNo7Data(m_prodOrderNoInspection)

}

function HardnessPattern1DetailNo1Data(m_prodOrderNoInspection) {



    if ($("#txtDetailNo1").val() !== "") {
        LoadHardnessPattern1DetailNo1Data(m_prodOrderNoInspection, $("#txtDetailNo1").val());
    }
    else {

        $("#lblTSActualDim1").text("");
        $("#lblTSYieldAct1").text("");
        $("#lblTensileAct1").text("");
        $("#lblTSElongAct1").text("");
        $("#lblTSReductAct1").text("");
        $("#lblHDD4Avg1").text("");

    }
    if ($("#txtDetailNo2").val() !== "") {
        LoadHardnessPattern1DetailNo2Data(m_prodOrderNoInspection, $("#txtDetailNo2").val());
    }
    else {
        $("#lblTSActualDim2").text("");
        $("#lblTSYieldAct2").text("");
        $("#lblTensileAct2").text("");
        $("#lblTSElongAct2").text("");
        $("#lblTSReductAct2").text("");
        $("#lblHDD4Avg2").text("");
    }
    if ($("#txtDetailNo3").val() !== "") {
        LoadHardnessPattern1DetailNo3Data(m_prodOrderNoInspection, $("#txtDetailNo3").val());
    }
    else {
        $("#lblTSActualDim3").text("");
        $("#lblTSYieldAct3").text("");
        $("#lblTensileAct3").text("");
        $("#lblTSElongAct3").text("");
        $("#lblTSReductAct3").text("");
        $("#lblHDD4Avg3").text("");
    }
    if ($("#txtDetailNo4").val() !== "") {
        LoadHardnessPattern1DetailNo4Data(m_prodOrderNoInspection, $("#txtDetailNo4").val());
    }
    else {
        $("#lblTSActualDim4").text("");
        $("#lblTSYieldAct4").text("");
        $("#lblTensileAct4").text("");
        $("#lblTSElongAct4").text("");
        $("#lblTSReductAct4").text("");
        $("#lblHDD4Avg4").text("");
    }
    if ($("#txtDetailNo5").val() !== "") {
        LoadHardnessPattern1DetailNo5Data(m_prodOrderNoInspection, $("#txtDetailNo5").val());
    }
    else {
        $("#lblTSActualDim5").text("");
        $("#lblTSYieldAct5").text("");
        $("#lblTensileAct5").text("");
        $("#lblTSElongAct5").text("");
        $("#lblTSReductAct5").text("");
        $("#lblHDD4Avg5").text("");
    }
    if ($("#txtDetailNo6").val() !== "") {
        LoadHardnessPattern1DetailNo6Data(m_prodOrderNoInspection, $("#txtDetailNo6").val());
    }
    else {
        $("#lblTSActualDim6").text("");
        $("#lblTSYieldAct6").text("");
        $("#lblTensileAct6").text("");
        $("#lblTSElongAct6").text("");
        $("#lblTSReductAct6").text("");
        $("#lblHDD4Avg6").text("");
    }
    if ($("#txtDetailNo7").val() !== "") {
        LoadHardnessPattern1DetailNo7Data(m_prodOrderNoInspection, $("#txtDetailNo7").val());
    }
    else {
        $("#lblTSActualDim7").text("");
        $("#lblTSYieldAct7").text("");
        $("#lblTensileAct7").text("");
        $("#lblTSElongAct7").text("");
        $("#lblTSReductAct7").text("");
        $("#lblHDD4Avg7").text("");
    }
}


function LoadHardnessPattern1DetailNo1Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;
            if (jsonResult.length > 0) {
                setHardnessPattern1DetailNo1Data(jsonResult);
                GetProductMasterDefaultValue(m_prodCD);
            }
            else {
                $("#lblTSActualDim1").text("");
                $("#lblTSYieldAct1").text("");
                $("#lblTensileAct1").text("");
                $("#lblTSElongAct1").text("");
                $("#lblTSReductAct1").text("");
                $("#lblHDD4Avg1").text("");
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetProductMasterDefaultValue(m_prodCD) {
    debugger;
    // $callFrom.data('requestRunning', true);
    var ajaxTime = new Date().getTime();
    // var jsonData = "{CHARGE :'" + charge + "'  , DIM_D : '" + DimD + "' }";

    if (m_prodCD == undefined) {

        m_prodCD = $("#hdfProdCD").text();
    }


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/GetProductMasterDefaultValue?Prod_CD=" + m_prodCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        //data: JSON.stringify({ CHARGE: charge, DIM_D: DimD })
    }

    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            // setChuubanDetails(jsonResult);
            setProductMasterDefaultValue(jsonResult);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setProductMasterDefaultValue(jsonResult) {
    $("#txtRemarkMessage1").val(jsonResult[0].milL_SHEET_REMARK_1);
    $("#txtRemarkMessage2").val(jsonResult[0].milL_SHEET_REMARK_2);
    $("#txtRemarkMessage3").val(jsonResult[0].milL_SHEET_REMARK_3);
    $("#txtRemarkMessage4").val(jsonResult[0].milL_SHEET_REMARK_4);
    $("#txtRemarkMessage5").val(jsonResult[0].milL_SHEET_REMARK_5);
    $("#txtRemarkMessage6").val(jsonResult[0].milL_SHEET_REMARK_6);
    $("#txtRemarkMessage7").val(jsonResult[0].milL_SHEET_REMARK_7);
}

function setHardnessPattern1DetailNo1Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }

    if (jsonResult[0].pP_DIM_TOL_MAX_CHR != null) {
        let m_Dim_Tol_Val = jsonResult[0].pP_DIM_TOL_MAX_CHR
        let m_Dim_Tol_Max = Math.round(m_Dim_Tol_Val * 100) / 100;
        let formatted = m_Dim_Tol_Max.toFixed(2);
        //let m_Dim_Tol_Max = Number(m_Dim_Tol_Val.toFixed(2));
        $("#lblDimTolMin").text(formatted);
    }
    else {
        $("#lblDimTolMin").text(jsonResult[0].pP_DIM_TOL_MAX_CHR);
    }

    if (jsonResult[0].pP_DIM_TOL_MIN_CHR != null) {

        let m_Dim_Tol_Min_Val = jsonResult[0].pP_DIM_TOL_MIN_CHR
        let m_Dim_Tol_Min = Math.round(m_Dim_Tol_Min_Val * 100) / 100;
        let formatted = m_Dim_Tol_Min.toFixed(2);
        $("#lblDimTolMax").text(formatted);
    }
    else {
        $("#lblDimTolMax").text(jsonResult[0].pP_DIM_TOL_MIN_CHR);
    }

    if (jsonResult[0].yeilD_PATTERN != null) {
        $("#lblYiedStrength1").text(jsonResult[0].yeilD_PATTERN);
    }
    else {
        $("#lblYiedStrength1").text(jsonResult[0].yeilD_PATTERN);
    }
    if (jsonResult[0].tensilE_STRENGTH_PATTERN != null) {
        $("#lblTensileStrength1").text(jsonResult[0].tensilE_STRENGTH_PATTERN);
    }
    else {
        $("#lblTensileStrength1").text(jsonResult[0].tensilE_STRENGTH_PATTERN);
    }
    if (jsonResult[0].elongatioN_1 != null) {
        $("#lblElongation1").text(jsonResult[0].elongatioN_1);
    }
    else {
        $("#lblElongation1").text(jsonResult[0].elongatioN_1);
    }
    if (jsonResult[0].reductioN_1 != null) {
        $("#lblReduction1").text(jsonResult[0].reductioN_1);
    }
    else {
        $("#lblReduction1").text(jsonResult[0].reductioN_1);
    }
    if (jsonResult[0].hD_STD_ACT_2 != null) {
        $("#lblHardness2").text(jsonResult[0].hD_STD_ACT_2);
    }
    else {
        $("#lblHardness2").text(jsonResult[0].hD_STD_ACT_2);
    }
    if (jsonResult[0].hD_STD_ACT_3 != null) {
        $("#lblHardness3").text(jsonResult[0].hD_STD_ACT_3);
    }
    else {
        $("#lblHardness3").text(jsonResult[0].hD_STD_ACT_3);
    }






    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim1").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim1").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct1").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct1").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct1").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct1").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct1").text(formatted);
    }
    else {
        $("#lblTSElongAct1").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct1").text(formatted);
    }
    else {
        $("#lblTSReductAct1").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg1").text(formatted);
    }
    else {
        $("#lblHDD4Avg1").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }











    //$("#ddlCustomerName").val(jsonResult.pP_CUSTOMER_CD);
    //$("#ddlscmbCustName").val(jsonResult.pP_CUSTOMER_CD);
    //// $("#ddlOperatorName").val(jsonResult[0].inT_QLTY_TEST_OPERATOR_CD);
    //$("#txtSpec_No").val(jsonResult.pP_PROD_SPEC_NO);
    //$("#ddlMakerName").val(jsonResult.pP_MAKER_CD);



}

function LoadHardnessPattern1DetailNo2Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                setHardnessPattern1DetailNo2Data(jsonResult);
            }
            else {
                $("#lblTSActualDim2").text("");
                $("#lblTSYieldAct2").text("");
                $("#lblTensileAct2").text("");
                $("#lblTSElongAct2").text("");
                $("#lblTSReductAct2").text("");
                $("#lblHDD4Avg2").text("");
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern1DetailNo2Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }


    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim2").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim2").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct2").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct2").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct2").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct2").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct2").text(formatted);
    }
    else {
        $("#lblTSElongAct2").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct2").text(formatted);
    }
    else {
        $("#lblTSReductAct2").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg2").text(formatted);
    }
    else {
        $("#lblHDD4Avg2").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }





}

function LoadHardnessPattern1DetailNo3Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                setHardnessPattern1DetailNo3Data(jsonResult);
            }
            else {
                $("#lblTSActualDim3").text("");
                $("#lblTSYieldAct3").text("");
                $("#lblTensileAct3").text("");
                $("#lblTSElongAct3").text("");
                $("#lblTSReductAct3").text("");
                $("#lblHDD4Avg3").text("");
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern1DetailNo3Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }


    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim3").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim3").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct3").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct3").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct3").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct3").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct3").text(formatted);
    }
    else {
        $("#lblTSElongAct3").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct3").text(formatted);
    }
    else {
        $("#lblTSReductAct3").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg3").text(formatted);
    }
    else {
        $("#lblHDD4Avg3").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }





}

function LoadHardnessPattern1DetailNo4Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                setHardnessPattern1DetailNo4Data(jsonResult);
            }
            else {
                $("#lblTSActualDim4").text("");
                $("#lblTSYieldAct4").text("");
                $("#lblTensileAct4").text("");
                $("#lblTSElongAct4").text("");
                $("#lblTSReductAct4").text("");
                $("#lblHDD4Avg4").text("");
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern1DetailNo4Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }


    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim4").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim4").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct4").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct4").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct4").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct4").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct4").text(formatted);
    }
    else {
        $("#lblTSElongAct4").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct4").text(formatted);
    }
    else {
        $("#lblTSReductAct4").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg4").text(formatted);
    }
    else {
        $("#lblHDD4Avg4").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }





}

function LoadHardnessPattern1DetailNo5Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                setHardnessPattern1DetailNo5Data(jsonResult);
            }
            else {
                $("#lblTSActualDim5").text("");
                $("#lblTSYieldAct5").text("");
                $("#lblTensileAct5").text("");
                $("#lblTSElongAct5").text("");
                $("#lblTSReductAct5").text("");
                $("#lblHDD4Avg5").text("");
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern1DetailNo5Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }


    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim5").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim5").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct5").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct5").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct5").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct5").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct5").text(formatted);
    }
    else {
        $("#lblTSElongAct5").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct5").text(formatted);
    }
    else {
        $("#lblTSReductAct5").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg5").text(formatted);
    }
    else {
        $("#lblHDD4Avg5").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }





}

function LoadHardnessPattern1DetailNo6Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                setHardnessPattern1DetailNo6Data(jsonResult);
            }
            else {
                $("#lblTSActualDim6").text("");
                $("#lblTSYieldAct6").text("");
                $("#lblTensileAct6").text("");
                $("#lblTSElongAct6").text("");
                $("#lblTSReductAct6").text("");
                $("#lblHDD4Avg6").text("");
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern1DetailNo6Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }


    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim6").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim6").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct6").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct6").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct6").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct6").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct6").text(formatted);
    }
    else {
        $("#lblTSElongAct6").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct6").text(formatted);
    }
    else {
        $("#lblTSReductAct6").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg6").text(formatted);
    }
    else {
        $("#lblHDD4Avg6").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }





}

function LoadHardnessPattern1DetailNo7Data(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;

            if (jsonResult.length > 0) {
                setHardnessPattern7DetailNoData(jsonResult);
            }
            else {
                $("#lblTSActualDim7").text("");
                $("#lblTSYieldAct7").text("");
                $("#lblTensileAct7").text("");
                $("#lblTSElongAct7").text("");
                $("#lblTSReductAct7").text("");
                $("#lblHDD4Avg7").text("");
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern1DetailNo7Data(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }


    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim7").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim7").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct7").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct7").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct7").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct7").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct7").text(formatted);
    }
    else {
        $("#lblTSElongAct7").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct7").text(formatted);
    }
    else {
        $("#lblTSReductAct7").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].hD_D4_AVG != null) {
        let m_hD_D4_AVG_val = jsonResult[0].hD_D4_AVG
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg7").text(formatted);
    }
    else {
        $("#lblHDD4Avg7").text(jsonResult[0].hD_D4_AVG);
    }

    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }





}

$("#btnHardnessPattern2").click(function (event) {
    debugger;
    var $callFrom = $(this);
    if ($("#txtChuubanNew").val() == "") {
        //Required Chuuban.
        var errorMessage = $("内質検査・機械検査を検索する為の注番を入力して下さい。").text();
        showMessageDialog(errorMessage, "txtHeatNo", WARNING);
        return false;
    }
    if ($("#txtGyoubanNew").val() == "") {
        //Required Chuuban.
        var errorMessage = $("内質検査・機械検査を検索する為の注番を入力して下さい。").text();
        showMessageDialog(errorMessage, "txtHeatNo", WARNING);
        return false;
    }
    else {
        var m_ChuubanNew = $("#txtChuubanNew").val();
        var m_GyoubanNew = $("#txtGyoubanNew").val();
        var m_chargeNo = $("#txtHeatNo").val();
        var m_mateDimD = $("#txtHeatNo").val();
        var m_mateNameMaker = $("#txtHeatNo").val();


        GetProdOrderNoInspectionHardness(m_ChuubanNew, m_GyoubanNew, m_chargeNo, m_mateDimD, m_mateNameMaker, $callFrom);

        if (m_prodOrderNoInspection == 0) {

            m_prodOrderNoInspection = GetProdOrderNoInspectionTensile(m_ChuubanNew, m_GyoubanNew, m_chargeNo, m_mateDimD, m_mateNameMaker, $callFrom);
        }


        const details = [
            "#txtDetailNo2",
            "#txtDetailNo3",
            "#txtDetailNo4",
            "#txtDetailNo5",
            "#txtDetailNo6",
            "#txtDetailNo7"
        ];


         if ($("#txtDetailNo1").val() == "" && $("#txtDetailNo2").val() == "" && $("#txtDetailNo3").val() == "" && $("#txtDetailNo4").val() == "" && $("#txtDetailNo5").val() == "" && $("#txtDetailNo6").val() == "" && $("#txtDetailNo7").val() == "") {

            var errorMessage = "分割番号の値を入力してください。";
            showMessageDialog(errorMessage, "txtDetailNo1", WARNING);
            return false;

        }
        

      else  if ($("#imedtDetailNo1").val() == "") {

            var errorMessage = "転記②では分割番号1のみ入力可能です";
            showMessageDialog(errorMessage, "txtDetailNo1", WARNING);
            return false;

         } else if (details.some(id => $(id).val() !== "")) {
            var errorMessage = "分割番号は一つのみ入力可能です。";
            showMessageDialog(errorMessage, "txtDetailNo1", WARNING);
            return false;
        }

        else {
             if (m_prodOrderNoInspection > 0) {

                 HardnessPattern2DetailNoData(m_prodOrderNoInspection);
             }
             else {
                 var errorMessage = "注番・チャージで一致する機械検査の検査データが存在しません。";
                 showMessageDialog(errorMessage, "txtDetailNo1", WARNING);
                 return false;
             }
        }


    }

});


function HardnessPattern2DetailNoData(m_prodOrderNoInspection) {



    if ($("#txtDetailNo1").val() !== "") {
        LoadHardnessPattern2DetailNoData(m_prodOrderNoInspection, $("#txtDetailNo1").val());
    }
    else {

        $("#lblTSActualDim1").text("");
        $("#lblTSYieldAct1").text("");
        $("#lblTensileAct1").text("");
        $("#lblTSElongAct1").text("");
        $("#lblTSReductAct1").text("");
        $("#lblHDD4Avg1").text("");

    }
   
}

function LoadHardnessPattern2DetailNoData(prodOrderNo, DetailNo) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HSKMillSheet/LoadHardnessPattern1DetailNoData?prodOrderNo=" + prodOrderNo + "&DetailNo=" + DetailNo,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        // data: JSON.stringify({ CUSTOMER_CD: m_custNameCD, PRODUCT_NAME_CUST: m_prodNameCust })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            var jsonResult = response.message;
            if (jsonResult.length > 0) {
                setHardnessPattern2DetailNoData(jsonResult);
            }
            else {
                $("#lblTSActualDim1").text("");
                $("#lblTSYieldAct1").text("");
                $("#lblTensileAct1").text("");
                $("#lblTSElongAct1").text("");
                $("#lblTSReductAct1").text("");
                $("#lblHDD4Avg1").text("");
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_ENTRY_014").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function setHardnessPattern2DetailNoData(jsonResult) {
    if (jsonResult[0].tesT_SPECIMEN != null) {
        $("#lblTSJISVal").text(jsonResult[0].tesT_SPECIMEN);
        document.getElementById("lblTSJISVal").style.color = "lightgreen";
    }
    else {
        $("#lblTSJISVal").val(jsonResult[0].tesT_SPECIMEN);
    }
    if (jsonResult[0].hD_PATTERN_ACT != null) {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
        document.getElementById("lblHardness1Val").style.color = "lightgreen";
    }
    else {
        $("#lblHardness1Val").text(jsonResult[0].hD_PATTERN_ACT);
    }

    if (jsonResult[0].pP_DIM_TOL_MAX_CHR != null) {
        let m_Dim_Tol_Val = jsonResult[0].pP_DIM_TOL_MAX_CHR
        let m_Dim_Tol_Max = Math.round(m_Dim_Tol_Val * 100) / 100;
        let formatted = m_Dim_Tol_Max.toFixed(2);
        //let m_Dim_Tol_Max = Number(m_Dim_Tol_Val.toFixed(2));
        $("#lblDimTolMin").text(formatted);
    }
    else {
        $("#lblDimTolMin").text(jsonResult[0].pP_DIM_TOL_MAX_CHR);
    }

    if (jsonResult[0].pP_DIM_TOL_MIN_CHR != null) {

        let m_Dim_Tol_Min_Val = jsonResult[0].pP_DIM_TOL_MIN_CHR
        let m_Dim_Tol_Min = Math.round(m_Dim_Tol_Min_Val * 100) / 100;
        let formatted = m_Dim_Tol_Min.toFixed(2);
        $("#lblDimTolMax").text(formatted);
    }
    else {
        $("#lblDimTolMax").text(jsonResult[0].pP_DIM_TOL_MIN_CHR);
    }

    if (jsonResult[0].yeilD_PATTERN != null) {
        $("#lblYiedStrength1").text(jsonResult[0].yeilD_PATTERN);
    }
    else {
        $("#lblYiedStrength1").text(jsonResult[0].yeilD_PATTERN);
    }
    if (jsonResult[0].tensilE_STRENGTH_PATTERN != null) {
        $("#lblTensileStrength1").text(jsonResult[0].tensilE_STRENGTH_PATTERN);
    }
    else {
        $("#lblTensileStrength1").text(jsonResult[0].tensilE_STRENGTH_PATTERN);
    }
    if (jsonResult[0].elongatioN_1 != null) {
        $("#lblElongation1").text(jsonResult[0].elongatioN_1);
    }
    else {
        $("#lblElongation1").text(jsonResult[0].elongatioN_1);
    }
    if (jsonResult[0].reductioN_1 != null) {
        $("#lblReduction1").text(jsonResult[0].reductioN_1);
    }
    else {
        $("#lblReduction1").text(jsonResult[0].reductioN_1);
    }
    if (jsonResult[0].hD_STD_ACT_2 != null) {
        $("#lblHardness2").text(jsonResult[0].hD_STD_ACT_2);
    }
    else {
        $("#lblHardness2").text(jsonResult[0].hD_STD_ACT_2);
    }
    if (jsonResult[0].hD_STD_ACT_3 != null) {
        $("#lblHardness3").text(jsonResult[0].hD_STD_ACT_3);
    }
    else {
        $("#lblHardness3").text(jsonResult[0].hD_STD_ACT_3);
    }






    if (jsonResult[0].acT_DIM != null) {
        $("#lblTSActualDim1").text(jsonResult[0].acT_DIM);
    }
    else {
        $("#lblTSActualDim1").text(jsonResult[0].acT_DIM);
    }

    if (jsonResult[0].yeilD_ACTUAL != null) {
        $("#lblTSYieldAct1").text(jsonResult[0].yeilD_ACTUAL);
    }
    else {
        $("#lblTSYieldAct1").text(jsonResult[0].yeilD_ACTUAL);
    }


    if (jsonResult[0].tS_RESULT != null) {
        $("#lblTensileAct1").text(jsonResult[0].tS_RESULT);
    }
    else {
        $("#lblTensileAct1").text(jsonResult[0].tS_RESULT);
    }

    if (jsonResult[0].elonG_RESULT != null) {
        let m_elongatioN_1 = jsonResult[0].elonG_RESULT
        let m_elongatioN = Math.round(m_elongatioN_1 * 100) / 100;
        let formatted = m_elongatioN.toFixed(1);
        $("#lblTSElongAct1").text(formatted);
    }
    else {
        $("#lblTSElongAct1").text(jsonResult[0].elonG_RESULT);
    }


    if (jsonResult[0].reducT_RESULT != null) {
        let m_reducT_RES_val = jsonResult[0].reducT_RESULT
        let m_reducT_RES = Math.round(m_reducT_RES_val * 100) / 100;
        let formatted = m_reducT_RES.toFixed(1);
        $("#lblTSReductAct1").text(formatted);
    }
    else {
        $("#lblTSReductAct1").text(jsonResult[0].reducT_RESULT);
    }

    if (jsonResult[0].d4_1 != null) {
        let m_hD_D4_AVG_val = jsonResult[0].d4_1
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg1").text(formatted);
    }
    else {
        $("#lblHDD4Avg1").text(jsonResult[0].d4_1);
    }

    if (jsonResult[0].d4_2 != null) {
        let m_hD_D4_AVG_val = jsonResult[0].d4_2
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg2").text(formatted);
    }
    else {
        $("#lblHDD4Avg2").text(jsonResult[0].d4_2);
    }

    if (jsonResult[0].d4_3 != null) {
        let m_hD_D4_AVG_val = jsonResult[0].d4_3
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg3").text(formatted);
    }
    else {
        $("#lblHDD4Avg3").text(jsonResult[0].d4_3);
    }

    if (jsonResult[0].d4_4 != null) {
        let m_hD_D4_AVG_val = jsonResult[0].d4_4
        let m_hD_D4_AVG = Math.round(m_hD_D4_AVG_val * 100) / 100;
        let Round = Math.round(m_hD_D4_AVG);
        let formatted = Round.toFixed(1);
        $("#lblHDD4Avg4").text(formatted);
    }
    else {
        $("#lblHDD4Avg4").text(jsonResult[0].d4_4);
    }



    //If Not IsDBNull(.Rows(0)(DAO.CONSTANTS.D4_1)) Then
    //lblHDD4Avg1.Text = Convert.ToString(.Rows(0)(DAO.CONSTANTS.D4_1)).Trim()
    //                End If
    //                If Not IsDBNull(.Rows(0)(DAO.CONSTANTS.D4_2)) Then
    //lblHDD4Avg2.Text = Convert.ToString(.Rows(0)(DAO.CONSTANTS.D4_2)).Trim()
    //                End If
    //                If Not IsDBNull(.Rows(0)(DAO.CONSTANTS.D4_3)) Then
    //lblHDD4Avg3.Text = Convert.ToString(.Rows(0)(DAO.CONSTANTS.D4_3)).Trim()
    //                End If
    //                If Not IsDBNull(.Rows(0)(DAO.CONSTANTS.D4_4)) Then
    //lblHDD4Avg4.Text = Convert.ToString(.Rows(0)(DAO.CONSTANTS.D4_4)).Trim()
    //                End If





    if (jsonResult[0].tS_TEST_IMPACT_1 != null) {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }
    else {
        $("#lblTSTestImpact1").text(jsonResult[0].tS_TEST_IMPACT_1);
    }

    if (jsonResult[0].tS_TEST_IMPACT_2 != null) {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }
    else {
        $("#lblTSTestImpact2").text(jsonResult[0].tS_TEST_IMPACT_2);
    }

    if (jsonResult[0].tS_TEST_IMPACT_3 != null) {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }
    else {
        $("#lblTSTestImpact3").text(jsonResult[0].tS_TEST_IMPACT_3);
    }











    //$("#ddlCustomerName").val(jsonResult.pP_CUSTOMER_CD);
    //$("#ddlscmbCustName").val(jsonResult.pP_CUSTOMER_CD);
    //// $("#ddlOperatorName").val(jsonResult[0].inT_QLTY_TEST_OPERATOR_CD);
    //$("#txtSpec_No").val(jsonResult.pP_PROD_SPEC_NO);
    //$("#ddlMakerName").val(jsonResult.pP_MAKER_CD);



}



$("#btnRegister").click(function (event) {
    debugger;

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

    
});

function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        $("#dialogMessageIcon img").hide();
        // $("#imgError").show();
        //$("#imgWarning").show();

        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
            centerVertical: true,
            callback: function () {
                if (firstFieldID !== "") {
                    $("#" + firstFieldID).focus();
                }
            }
        });

    }
    return returnVal;

}


