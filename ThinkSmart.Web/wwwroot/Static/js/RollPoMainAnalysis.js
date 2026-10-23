var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var fcSummaryNo;
var typeDropDown = "select-one";
var typeText = "text";
var DISABLED = "disabled";
var poMainAnalysisGridData = 0;
var intMatlFcSummaryCd = "";
let selectedRow = false;
var message = "";

var chkRollPOLockMonthly = document.getElementById("chkRollPOLockMonthly");
var btnExecuteBatch = document.getElementById("btnExecuteBatch");
var btnSearch = document.getElementById("btnSearch");
var btnUnconfirmedMatlSearch = document.getElementById("btnUnconfirmedMatlSearch");
var btnMarkAsAnalyzed = document.getElementById("btnMarkAsAnalyzed");
var btnAnalyzePOQty = document.getElementById("btnAnalyzePOQty");
var btnPlaceOrder = document.getElementById("btnPlaceOrder");
var btnOrderQty = document.getElementById("btnOrderQty");
var btnMaterialWiseSubTotal = document.getElementById("btnMaterialWiseSubTotal");
var btnForcastStatus = document.getElementById("btnForcastStatus");
var btnMaterialForcastDataReport = document.getElementById("btnMaterialForcastDataReport");
var chkRollPOCompleteMonthly = document.getElementById("chkRollPOCompleteMonthly");
var btnMaterialForcastDataReport = document.getElementById("btnMaterialForcastDataReport");
function openScreens() {
    debugger;
    for (let i = 1; i <= poMainAnalysisGridData.length; i++) {
        checked = $('#selectRow' + i).is(':checked');

        if (checked) {
            intMatlFcSummaryCd = $("#intMatlFcSummaryCd" + i).text();
            selectedRow = true;
            break;
        }
       
    }
    if (selectedRow)
    {
        var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
        var year = $("#txtYear").val() ? $("#txtYear").val() : null;
        window.open('/RollPoMainAnalysis/AnalysePOQuantity?FcSummaryNo=' + intMatlFcSummaryCd, 'AnalysePOQuantity', 'height=1000px,width=2000px,resizable=no');
        debugger;
        window.open(
            '/RollPoMainAnalysis/RollForecastPopUp?FcSummaryNo=' + intMatlFcSummaryCd +
            '&Roll=' + R +
            '&Year=' + year,
            'RollForecastPopUp',
            'height=1000px,width=2000px,resizable=no'
        );

    }
    else
    {
        const focusFieldID = $(':focus').attr("id");
        const message = $.trim($("#lblSelectAtLeastOneRecord_ROLL_PO_02").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }
   
           
}
$(document).ready(function () {
    setMaterialType();  
   
   // setOperatorList("");
    InitSortPopup();
    setRoll();
    setDecimalFormats();
    if (IsPODone()) {
        debugger;
        var reprintPOReport = $.trim($("#btn_ReprintPOReport").text());
        $("#btnPlaceOrder").val(reprintPOReport);
    }
    else {
        var placeOrder = $.trim($("#btn_PlaceOrder").text());
        $("#btnPlaceOrder").val(placeOrder);
    }
   
});
function OnForecastStatusBtnClick() {
    window.open('/RollPoMainAnalysis/ForecastStatusPopUp', 'ForecastStatusPopUp', 'height = 830px, width = 525px, resizable = no');
}

function setDecimalFormats() {
    $("#txtDimD").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
  
}

function setRoll() {   
   
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
                
                $("#txtRoll").val(jsonResult.rolL_R);
                $("#txtYear").val(jsonResult.rolL_YEAR);                   
                SetControlsInitialStates();
               
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function IsAlreadyBatchRunForSameR()
{
    debugger;
    if (IsBatchRunSuccess()) {
        return true;
    }
    else {
        //*************** code not implement for now/
        //if (CheckMatchingYearRRecords()) {
        //  // EmptyRequiredTables() not done
        //} 
        return false;
    }

}

function IsBatchRunSuccess() {
    debugger;
    var ajaxTime = new Date().getTime();
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;
    var returnBatchRunVal = false;


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/IsBatchRunSuccess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ROLL_R: R, ROLL_YEAR: year
        })

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult != null && jsonResult != 0) {
                returnBatchRunVal = true;
            }
            else {
                returnBatchRunVal = false;
            }
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnBatchRunVal;
}

function IsSalesForecastLocked() {
    debugger;
    var ajaxTime = new Date().getTime();
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;
    var returnForecastLockedVal = false;


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/CheckForecastLocked",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ROLL_R: R, ROLL_YEAR: year
        })

    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult != null && jsonResult != 0) {
                returnForecastLockedVal = true;
            }
            else {
                returnForecastLockedVal = false;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnForecastLockedVal;
}

function SetControlsInitialStates() {

    if (IsAlreadyBatchRunForSameR())
    {
        chkRollPOLockMonthly.disabled = true;
        chkRollPOLockMonthly.checked = true;
        btnExecuteBatch.disabled = true;

        if (IsPODone()) {
            chkRollPOCompleteMonthly.checked = true;
            chkRollPOCompleteMonthly.disabled = true;            
            btnPlaceOrder.disabled = false;


        }
        else {
            IsAllAnalysisIsOver();           
            btnPlaceOrder.disabled = true;
        }     
        setOperatorList("", "");
        GetEmployee();
        GetRollPOMainAnalysisGridData(null);
        btnMaterialForcastDataReport.disabled = false;
    }

    
    else {

        //if (IsSalesForecastLocked()) {
        //    chkRollPOLockMonthly.disabled = true;
        //    chkRollPOLockMonthly.checked = true;
        //    btnExecuteBatch.disabled = false;
        //}
        //else {

        //}
        chkRollPOLockMonthly.disabled = false;
        btnExecuteBatch.disabled = true;
        chkRollPOCompleteMonthly.disabled = true;
        btnMarkAsAnalyzed.disabled = true;      
        btnSearch.disabled = true;
        btnSort.disabled = true;
        btnAnalyzePOQty.disabled = true;
        btnPlaceOrder.disabled = true;
        btnOrderQty.disabled = true;
        btnMaterialWiseSubTotal.disabled = true;        
        btnForcastStatus.disabled = false;
        btnMaterialForcastDataReport.disabled = true;      


    }
    SetGridColumnHeadingRValues();


}

chkRollPOLockMonthly.addEventListener('change', () => {
    if (chkRollPOLockMonthly.checked) {
        btnExecuteBatch.disabled = false; 
    } else {
        btnExecuteBatch.disabled = true; 
        
    }
});

chkRollPOCompleteMonthly.addEventListener("change", function () {
   
    if (chkRollPOCompleteMonthly.checked) {
        btnPlaceOrder.disabled = false;
    } else {
        btnPlaceOrder.disabled = true;

    }
});

$("#btnExecuteBatch").click(function (event) {

    //if (Boolean($("#hdfExecuteBatch").val()) == true)
    //{
    //    ImportBatchDataFromHost();
    //}
    if (!IsMandatorySearchFieldsFilled()) {
        return false;
    }
    ExecuteMaterialForecastBatch();
    setOperatorList("", "");
    GetEmployee();


}).ajaxStart(function () {

    $("#LoadingImage").show();
}).ajaxStop(function () {
    $("#LoadingImage").hide();
}).ajaxError(function () {
    $("#LoadingImage").hide();
});

$(function () {
    $(document).ajaxStart(function () {

        $("#LoadingImage").show();
    });

    $(document).ajaxStop(function () {
        $("#LoadingImage").hide();
    });

    $(document).ajaxError(function () {
        $("#LoadingImage").hide();
    });

});


$("#btnMaterialForcastDataReport").click(function (event) { 
    debugger;

    var ajaxTime = new Date().getDate();
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;

    var mywindow = window.open(REPORTURL + "/Report/r_forecastreport.aspx?userid=" + userID +
        "&year=" + year +
        "&r=" + R, "width=1940,height=1140", "resizable = no");
    //focusFieldID = $(':focus').attr("id");
    //var message = $("#lblUnregisteredMaterial_ROLL_PO_05").text().trim();
    //showMessageDialog(message, focusFieldID, WARNING);
    //var myWindow = window.open(REPORTURL + "/Report/MatlNotFound.aspx?userID=" + userID + '&year=' + year, +'&R=' + R, "width=1940,height=1140", "Resizable = no");


});


function ExecuteMaterialForecastBatch() {
    debugger;
   
    var ajaxTime = new Date().getDate();  
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
    var employeeCD = $("#ddlscAssembleVerifyBy").val() ? $("#ddlscAssembleVerifyBy").val() : null;
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/ExecuteMaterialForecastBatch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ROLL_R: R, ROLL_YEAR: year, USER_ID: userID, EMPLOYEE_CD: employeeCD
        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
           
            //code comment for testing batch

            if (response.message == "Matl Not Found for one or more product") {
                focusFieldID = $(':focus').attr("id");
                var message = $("#lblUnregisteredMaterial_ROLL_PO_05").text().trim();
                showMessageDialog(message, focusFieldID, WARNING);
                var myWindow = window.open(REPORTURL + "/Report/MatlNotFound.aspx?userID=" + userID + '&year=' + year,+'&R=' + R, "width=1940,height=1140", "Resizable = no");
            }
            else {
            debugger;
            console.log("Batch Execute successfully");
            chkRollPOLockMonthly.disabled = true;
            btnExecuteBatch.disabled = true;
            btnMarkAsAnalyzed.disabled = false;
            btnAnalyzePOQty.disabled = false;
            btnMaterialWiseSubTotal.disabled = false;
            btnOrderQty.disabled = false;
            btnSearch.disabled = false;
            btnSort.disabled = false;
            btnMaterialForcastDataReport.disabled = false;
            btnPlaceOrder.disabled = false;
            GetRollPOMainAnalysisGridData();

            }
        })
        

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
           
            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ROLL_PO_01").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
        
}


function IsPODone() {
    var ajaxTime = new Date().getDate();
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;

    let yearR = year.toString() + R.toString().padStart(2, '0');
    var userID = $("#hdfLoginId").val();
    var returnPODoneVal = false;

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/IsPODone",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CURRENT_R: R, CURRENT_YYYY: year
        })

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if (jsonResult != null && jsonResult != 0) {
                returnPODoneVal = true;
            }
            else {
                returnPODoneVal = false;
              }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnPODoneVal;
}

   $("#btnPlaceOrder").click(function (event) {      
    
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;

    let yearR = year.toString() + R.toString().padStart(2, '0');
    var userID = $("#hdfLoginId").val();       
      
   
     if (IsPODone()) {
         var myWindow = window.open(REPORTURL + "/Report/PurchaseOrderReport.aspx?userID=" + userID + '&yearR=' + yearR, "width=1940,height=1140", "Resizable = no"); 
     }
     else {
         PlaceOrder(); // comment for testing report 
         var myWindow = window.open(REPORTURL + "/Report/PurchaseOrderReport.aspx?userID=" + userID + '&yearR=' + yearR, "width=1940,height=1140", "Resizable = no");
     }
                
         
});

function PlaceOrder() {
    
    var ajaxTime = new Date().getDate();
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;  
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
    var employeeCD = $("#ddlscAssembleVerifyBy").val() ? $("#ddlscAssembleVerifyBy").val() : null;

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/PlaceOrder",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ROLL_R: R, ROLL_YEAR: year, USER_ID: userID, EMPLOYEE_CD: employeeCD
        })
    }
    $.ajax(settings)
        .done(function (response) {
            
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult == 2) {
                chkRollPOCompleteMonthly.disabled = false;
                btnPlaceOrder.disabled = false;
                focusFieldID = $(':focus').attr("id");
                 message = $("#lblPOSaveSuccessfully_ROLL_PO_03").text().trim();
                showMessageDialog(message, focusFieldID, SUCCESS);
            }
            else if (jsonResult == 1) {
                focusFieldID = $(':focus').attr("id");
                var message = $("#lblNoDataForPO_ROLL_PO_04").text().trim();
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else if (jsonResult == 0) {

                focusFieldID = $(':focus').attr("id");
               var message = $("#lblErrorMessageE_ROLL_PO_01").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
           var message = $("#lblErrorMessageE_ROLL_PO_01").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

$("#btnMarkAsAnalyzed").click(function (event) {
    debugger;
    var message = "";

    message = $.trim($("#lblconfMsg_MarkAsAnalysed").text());

    bootbox.confirm({
        title: "<div style='color:white'></div>",
        message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
            if (result) {
                MarkAsAnalyzed();
                IsAllAnalysisIsOver();
            }
            else {
               
            }

        },
        centerVertical: true
    });

    
});

function MarkAsAnalyzed() {

    var ajaxTime = new Date().getTime();  

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/SetAnalysisOverForFcSummaryWithNoFC",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //data: jsonDataSaveEditDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response;
            //if (result.isSuccess == true) {
            //    chkRollPOCompleteMonthly.disabled = false;
            //}

           

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function IsAllAnalysisIsOver() {
    var ajaxTime = new Date().getDate();
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/IsAllAnalysisIsOver",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if (jsonResult != null && jsonResult != 0) {
                chkRollPOCompleteMonthly.disabled = true;
            }
            else {
                chkRollPOCompleteMonthly.disabled = false;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function ImportBatchDataFromHost()
{
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/ImportBatchDataFromHost",
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ROLL_PO_01").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });

}
function OnMaterialWiseOrderQtyBtnClick() {
    window.open('/RollPoMainAnalysis/MatlNameWiseOrderQtyPopUp', 'MatlNameWiseOrderQtyPopUp', 'height = 710px, width = 490px, resizable = no');
}

function SetGridColumnHeadingRValues() {
    
    var  currentR = $("#txtRoll").val() ? $("#txtRoll").val() : null;

    var resultRoll = calLastForecastRollHeaders(currentR);
    $("#lblRMonth1").text($("#lblRMonth1").text().replace("xx", resultRoll.forecastLastRoll_R1).replace("nn", resultRoll.forecastRoll_M1));
    $("#lblRMonth2").text($("#lblRMonth2").text().replace("xx", resultRoll.forecastLastRoll_R2).replace("nn", resultRoll.forecastRoll_M2));
    $("#lblRMonth3").text($("#lblRMonth3").text().replace("xx", resultRoll.forecastLastRoll_R3).replace("nn", resultRoll.forecastRoll_M3));

    var resultNextRoll = calNextForecastRollHeaders(currentR);
    $("#lblNextR1").text($("#lblNextR1").text().replace("xx", resultNextRoll.forecastNextRoll_R1));
    $("#lblNextR2").text($("#lblNextR2").text().replace("xx", resultNextRoll.forecastNextRoll_R2));
    $("#lblNextR3").text($("#lblNextR3").text().replace("xx", resultNextRoll.forecastNextRoll_R3));
    $("#lblNextR4").text($("#lblNextR4").text().replace("xx", resultNextRoll.forecastNextRoll_R4));
    $("#lblNextR5").text($("#lblNextR5").text().replace("xx", resultNextRoll.forecastNextRoll_R5));
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

    var R4 = currentR;

    var R5 = new Date(baseMonth1);
    R5.setMonth(baseMonth1.getMonth() + 1);

    returnNextRoll["forecastNextRoll_R1"] = R1.getMonth() + 1;
    returnNextRoll["forecastNextRoll_R2"] = R2.getMonth() + 1;
    returnNextRoll["forecastNextRoll_R3"] = R3.getMonth() + 1;   
    returnNextRoll["forecastNextRoll_R4"] = R4; 
    returnNextRoll["forecastNextRoll_R5"] = R5.getMonth() + 1; 

    return returnNextRoll;
}

$("#btnscAssembleVerifyByOpeartorSearch").click(function (event) {

    yobina = $.trim($("#txtscAssembleVerifyByInputSearch").val());
    setOperatorList(yobina, "");
});
function setOperatorList(yobina) {

    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/EmployeeList?yobina=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindOperatorDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function BindOperatorDropdown(jsonResult) {

    $("#ddlscAssembleVerifyBy").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlscAssembleVerifyBy").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });

}

var employeeDropdown = document.getElementById('ddlscAssembleVerifyBy');
employeeDropdown.addEventListener("change", function () {
    var employeeCD = $("#ddlscAssembleVerifyBy").val() 
    GetLocationNameByEmpCd(employeeCD);
});

function GetLocationNameByEmpCd(employeeCD) {
    var ajaxTime = new Date().getTime();
   // var employeeCD = $("#ddlscAssembleVerifyBy").val() ? $("#ddlscAssembleVerifyBy").val() : null;

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/GetLocationName?employeeCD=" + employeeCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            $('#txtLocation').val(response.message);


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
$(document).on('click', '#btnSort', function () {
    $("#SortPopUpModal").modal('show');
})
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
    }Pplace
    EnableDisableSortButtons();
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
    setOperatorList("", "");
    GetEmployee();
    GetRollPOMainAnalysisGridData(selectedColumns);
    //GetLocationNameByEmpCd();
}
function btnSortClose_Click() {
    closepopup();
}

function closepopup() {
    $("#SortPopUpModal").modal('hide');
}

function btnSortReset_Click() {
    InitSortPopup();
}

function InitSortPopup() {

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/RollPoMainAnalysis/InitializeSortPopup",
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

$("#btnSearch").click(function (event) {
    setOperatorList("", "");
    GetEmployee();
    GetRollPOMainAnalysisGridData(null);    

});
function GetEmployee()
{
    var ajaxTime = new Date().getTime();
    //var employeeCD = $("#ddlscAssembleVerifyBy").val() ? $("#ddlscAssembleVerifyBy").val() : null;

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/GetEmployeeCd",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            
            // $('#ddlscAssembleVerifyBy').val(response.message);
           // $("#ddlscAssembleVerifyBy").val(response.message || $("#ddlscAssembleVerifyBy").prop('selectedIndex'));
           $("#ddlscAssembleVerifyBy").val(response.message);
            GetLocationNameByEmpCd(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function GetRollPOMainAnalysisGridData(sortFields) {
    var ajaxTime = new Date().getTime();
   
    var R = $("#txtRoll").val() ? $("#txtRoll").val() : null;
    var year = $("#txtYear").val() ? $("#txtYear").val() : null;
    var internalName = $("#txtMatlNameInt").val() ? $("#txtMatlNameInt").val() : null;
    var dimD = $("#txtDimD").val() ? $("#txtDimD").val() : null;
    var matlTypeCBK = $("#ddlCoilBarKurokawa").val() ? $("#ddlCoilBarKurokawa").val() : null;
    
    var chkUnconfirmed = document.getElementById('chkUnconfirmed').checked ? 1 : 0;

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/GetRollPOMainAnalysisGridData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
         data: JSON.stringify({
             ROLL_R: R, ROLL_YEAR: year,INTERNAL_NAME_STRING: internalName, DIM_D: dimD, MATL_TYPE_CBK: matlTypeCBK, UNCONFIRMED: chkUnconfirmed, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            poMainAnalysisGridData = response.message;

            var settings1 = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/RollPoMainAnalysis/RollPOMainAnalysisGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
            }
            $.ajax(settings1)
                .done(function (response1) {
                    debugger;
                   
                    $("#divRollPOMainAnalysisGrid").html(response1.message);
                   
                    SetGridColumnHeadingRValues();
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function BindMaterialTypeDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {        
        $("#ddlCoilBarKurokawa").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}
function setMaterialType() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoMainAnalysis/GetMaterialTypeList",
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
            var errorCode = $.trim($("#lblErrorMessageE_ROLL_PO_01").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function IsMandatorySearchFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";

    $.each($.find("#pnlHeader .Mandatory"), function (index, mandatoryField) {
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



