var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var intMatlFcSummaryCd = ""; 
var roll = "" ;
var year = "" ;
let selectedRow = false;
var customer_CD = 0;
var productGridData = 0;
$(Document).ready(function () {
    debugger;
    
    //Calculate Roll And Month Both Grid Header.
    let fullURL = window.location.href;
    var rMainPoScreen = fullURL.split('FcSummaryNo=');
    var fcSummaryCd = rMainPoScreen[1].split('&');
    intMatlFcSummaryCd = fcSummaryCd[0];
    r = fcSummaryCd[1].split('Roll=');
    roll = parseInt(r[1],10);
    y = fcSummaryCd[2].split('Year=');
    year = parseInt(y[1],10);

    //Roll pass for calculate roll & month on grid .
    GetOneMonthProductForecastGrid();
    GetOneMonthMaterialForecastGrid();
    SetMaterialReplyDropDown();
    CalculateAllGridMonthRoll(roll);

    localStorage.removeItem("forecastChanged");
    localStorage.removeItem("forecastData");
}); 

$("#btnByCustomer").click(function () {
    debugger;
    for (let i = 1; i <= productGridData.length; i++) {
        checked = $('#selectRow' + i).is(':checked');

        if (checked) {
            customer_CD = $("#CUSTOMER_CD" + i).text();
            selectedRow = true;
            break;
        }
    }
    if (selectedRow) {
        window.open('/RollPoMainAnalysis/CustomerWiseProdStockShukkaDetails?CustomerCD=' + customer_CD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1020 + ", height=" + 910);
    }
    else {
        const focusFieldID = $(':focus').attr("id");
        const message = $.trim($("#lblErrorMessageE_SelectAtLeastOne").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }

});
$("#btnByProductSpec").click(function () {
    debugger;
    for (let i = 1; i <= productGridData.length; i++) {
        checked = $('#selectRow' + i).is(':checked');

        if (checked) {
            product_CD = $("#PRODUCT_CD" + i).text();
            selectedRow = true;
            break;
        }
    }
    if (selectedRow) {
        window.open('/RollPoMainAnalysis/ProdSpecWiseSalesStatus?ProductCD=' + product_CD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1040 + ", height=" + 890);
    }
    else {
        const focusFieldID = $(':focus').attr("id");
        const message = $.trim($("#lblErrorMessageE_SelectAtLeastOne").text());
        showMessageDialog(message, focusFieldID, WARNING);
    }

});
function SetMaterialReplyDropDown() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollPoMainAnalysis/GetMaterialReply",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindMaterialReplyDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindMaterialReplyDropdown(jsonResult) {
    debugger;
    $("#listMaterialReply").html("").append($('<option value=""></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#listMaterialReply").append($('<option value="' + jsonObj.name + '"</option>'));
    });
}


function GetOneMonthProductForecastGrid() {
    debugger;

    var historyMonth1 = $("#lblHistoryMonth1").text().split('M');
    var historyMonth2 = $("#lblHistoryMonth2").text().split('M');
    var historyMonth3 = $("#lblHistoryMonth3").text().split('M');

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollForcastPopup/GetOneMonthProductForecastDetails?FcSummaryNo=" + intMatlFcSummaryCd + "&Roll=" + roll + "&Year=" + year,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            productGridData = response.message;
            if (response.isSuccess) {
               
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                 
                    url: "/RollPoMainAnalysis/GetOneMonthProductForecastDetails", 
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        debugger;
                        jsonResult = response1.message;
                        if (response1.isSuccess) {
                            $("#pnlOneMonthProductForecastGrid").html(response1.message);
                        }
                        //Roll pass for calculate roll & month on grid .
                        CalculateAllGridMonthRoll(roll);
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;
                        
                    });
            }
           
           
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetOneMonthMaterialForecastGrid() {
    debugger;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/RollForcastPopup/GetOneMonthMaterialForecastDetails?FcSummaryNo=" + intMatlFcSummaryCd,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                var settings1 = {
                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/RollPoMainAnalysis/GetOneMonthMaterialForecastDetails", 
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        jsonResult = response1.message;
                        if (response1.isSuccess) {
                            $("#pnlOneMonthMaterialForecastGrid").html(response1.message);
                        }
                        //Calculate Roll And Month Both Grid Header.
                        CalculateAllGridMonthRoll(roll);
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        
                    });
            }
         

        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

///calculate R & M

function CalculateAllGridMonthRoll(currentR) {
    var resultMonth = columnDispatchHistoryMonthHeaders(currentR);
    $("#lblHistoryMonth3").text($("#lblHistoryMonth3").text().replace("nn", resultMonth.historyMonth_M3));
    $("#lblHistoryMonth2").text($("#lblHistoryMonth2").text().replace("nn", resultMonth.historyMonth_M2));
    $("#lblHistoryMonth1").text($("#lblHistoryMonth1").text().replace("nn", resultMonth.historyMonth_M1));
    //
    $("#lblHistoryMonth6").text($("#lblHistoryMonth6").text().replace("nn", resultMonth.historyMonth_M3));
    $("#lblHistoryMonth5").text($("#lblHistoryMonth5").text().replace("nn", resultMonth.historyMonth_M2));
    $("#lblHistoryMonth4").text($("#lblHistoryMonth4").text().replace("nn", resultMonth.historyMonth_M1));
    var resultRoll = calLastForecastRollHeaders(currentR);
    $("#lblLastRoll1").text($("#lblLastRoll1").text().replace("xx", resultRoll.forecastRoll_R1));
    $("#lblLastRoll2").text($("#lblLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
    $("#lblLastRoll3").text($("#lblLastRoll3").text().replace("xx", resultRoll.forecastRoll_R3));
    $("#lblLastRoll4").text($("#lblLastRoll4").text().replace("xx", resultRoll.forecastRoll_R4));
    var resultMonthForRoll = calLastForecastMonthForRollHeadersForcastPopup(resultRoll.forecastRoll_R1, resultRoll.forecastRoll_R2, resultRoll.forecastRoll_R3, resultRoll.forecastRoll_R4);
    $("#lblRollForMonth1").text($("#lblRollForMonth1").text().replace("nn", resultMonthForRoll.forecastMonth_M1));
    $("#lblRollForMonth2").text($("#lblRollForMonth2").text().replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblRollForMonth3").text($("#lblRollForMonth3").text().replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblRollForMonth4").text($("#lblRollForMonth4").text().replace("nn", resultMonthForRoll.forecastMonth_M4));
    ////Set Material Grid Roll and Month
    $("#lblFooterLastRoll1").text($("#lblFooterLastRoll1").text().replace("xx", resultRoll.forecastRoll_R1));
    $("#lblFooterLastRoll2").text($("#lblFooterLastRoll2").text().replace("xx", resultRoll.forecastRoll_R2));
    $("#lblFooterLastRoll3").text($("#lblFooterLastRoll3").text().replace("xx", resultRoll.forecastRoll_R3));
    $("#lblFooterLastRoll4").text($("#lblFooterLastRoll4").text().replace("xx", resultRoll.forecastRoll_R4));
    $("#lblFooterRollForMonth1").text($("#lblFooterRollForMonth1").text().replace("nn", resultMonthForRoll.forecastMonth_M1));
    $("#lblFooterRollForMonth2").text($("#lblFooterRollForMonth2").text().replace("nn", resultMonthForRoll.forecastMonth_M2));
    $("#lblFooterRollForMonth3").text($("#lblFooterRollForMonth3").text().replace("nn", resultMonthForRoll.forecastMonth_M3));
    $("#lblFooterRollForMonth4").text($("#lblFooterRollForMonth4").text().replace("nn", resultMonthForRoll.forecastMonth_M4));
    ////Set Footer Grid Roll and Month
    var lastForcast = lastForcastRColumnHeader(currentR);
    $("#lblMaterilGridLastRoll1").text($("#lblMaterilGridLastRoll1").text().replace("xx", lastForcast.forecastRoll_R1)); 
    $("#lblMaterilGridLastRoll2").text($("#lblMaterilGridLastRoll2").text().replace("xx", lastForcast.forecastRoll_R2));
    $("#lblMaterilGridLastRoll3").text($("#lblMaterilGridLastRoll3").text().replace("xx", lastForcast.forecastRoll_R3));
    $("#lblMaterilGridLastRoll4").text($("#lblMaterilGridLastRoll4").text().replace("xx", lastForcast.forecastRoll_R4));
    $("#lblMaterilGridRollForMonth1").text($("#lblMaterilGridRollForMonth1").text().replace("nn", lastForcast.forecastMonth_M1));
    $("#lblMaterilGridRollForMonth2").text($("#lblMaterilGridRollForMonth2").text().replace("nn", lastForcast.forecastMonth_M2));
    $("#lblMaterilGridRollForMonth3").text($("#lblMaterilGridRollForMonth3").text().replace("nn", lastForcast.forecastMonth_M3));
    $("#lblMaterilGridRollForMonth4").text($("#lblMaterilGridRollForMonth4").text().replace("nn", lastForcast.forecastMonth_M4));
    
}


//function saveForecastData() {
//    let forecastData = [];

//    $("input[id^='chkProdSalesFcDoneDate'], input[id^='chkMatlSalesFcDoneDate']").each(function () {

//        let checkboxId = $(this).attr("id");

//        let isProd = checkboxId.startsWith("chkProdSalesFcDoneDate");

//        let index = checkboxId
//            .replace("chkProdSalesFcDoneDate", "")
//            .replace("chkMatlSalesFcDoneDate", "");

//        let rollFcCd = isProd
//            ? $("#prodRollFCCd" + index).text()
//            : $("#matlRollFCCd" + index).text();

//        forecastData.push({
//            ROLL_FC_CD: parseInt(rollFcCd),
//            CHECKEDBOX_VALUE: $(this).prop("checked")
//        });
//    });

//    localStorage.setItem("forecastData", JSON.stringify(forecastData));
//}

//$(document).on(
//    "change",
//    "input[id^='chkProdSalesFcDoneDate'], input[id^='chkMatlSalesFcDoneDate'], input[id ^= 'txtMaterialReply']",
//    function () {
//        saveForecastData();
//    }
//);


//function saveForecastData() {
//    let forecastData = [];
//    let processedIndexes = [];

//    $("input[id^='chkProdSalesFcDoneDate'], " +
//        "input[id^='chkMatlSalesFcDoneDate'], " +
//        "input[id^='txtMaterialReplyProd'], " +
//        "input[id^='txtMaterialReplyMatl']").each(function () {

//            let id = $(this).attr("id");
//            let index = id.replace(/\D/g, '');

//            // Skip invalid IDs
//            if (!index) return;

//            if (processedIndexes.includes(index)) return;
//            processedIndexes.push(index);

//            let rollFcCd =
//                $("#prodRollFCCd" + index).text().trim() ||
//                $("#matlRollFCCd" + index).text().trim();

//            // Skip blank rollFcCd
//            if (!rollFcCd) return;

//            let chkProd = $("#chkProdSalesFcDoneDate" + index).prop("checked") || false;
//            let chkMatl = $("#chkMatlSalesFcDoneDate" + index).prop("checked") || false;

//            let replyProd = $("#txtMaterialReplyProd" + index).val() || "";
//            let replyMatl = $("#txtMaterialReplyMatl" + index).val() || "";

//            forecastData.push({
//                ROLL_FC_CD: parseInt(rollFcCd),
//                CHECKEDBOX_VALUE: chkProd || chkMatl,
//                REPLY_FROM_MATL_DEPT: replyProd || replyMatl
//            });
//        });

//    localStorage.setItem("forecastData", JSON.stringify(forecastData));
//}

//$(document).on(
//    "change keyup",
//    "input[id^='chkProdSalesFcDoneDate'], " +
//    "input[id^='chkMatlSalesFcDoneDate'], " +
//    "input[id^='txtMaterialReplyProd'], " +
//    "input[id^='txtMaterialReplyMatl']",
//    function () {
//        saveForecastData();
//    }
//);
//let changedRows = {};

//function saveForecastData(index) {

//    let rollFcCd =
//        $("#prodRollFCCd" + index).text() ||
//        $("#matlRollFCCd" + index).text();

//    let chkProd = $("#chkProdSalesFcDoneDate" + index).prop("checked") || false;
//    let chkMatl = $("#chkMatlSalesFcDoneDate" + index).prop("checked") || false;

//    let replyProd = $("#txtMaterialReplyProd" + index).val() || "";
//    let replyMatl = $("#txtMaterialReplyMatl" + index).val() || "";

//    changedRows[index] = {
//        ROLL_FC_CD: parseInt(rollFcCd),
//        CHECKEDBOX_VALUE: chkProd || chkMatl,
//        REPLY_FROM_MATL_DEPT: replyProd || replyMatl
//    };

//    localStorage.setItem(
//        "forecastData",
//        JSON.stringify(Object.values(changedRows))
//    );
//}

//$(document).on(
//    "change keyup",
//    "input[id^='chkProdSalesFcDoneDate'], " +
//    "input[id^='chkMatlSalesFcDoneDate'], " +
//    "input[id^='txtMaterialReplyProd'], " +
//    "input[id^='txtMaterialReplyMatl']",
//    function () {

//        let id = $(this).attr("id");
//        let index = id.replace(/\D/g, '');

//        saveForecastData(index);
//    }
//);

let changedRows = {};

function saveForecastData(index, type) {

    let rollFcCd, checkboxValue, materialReply;

    if (type === "prod") {
        rollFcCd = $("#prodRollFCCd" + index).text();
        checkboxValue = $("#chkProdSalesFcDoneDate" + index).prop("checked") || false;
        materialReply = $("#txtMaterialReplyProd" + index).val() || "";
    }
    else if (type === "matl") {
        rollFcCd = $("#matlRollFCCd" + index).text();
        checkboxValue = $("#chkMatlSalesFcDoneDate" + index).prop("checked") || false;
        materialReply = $("#txtMaterialReplyMatl" + index).val() || "";
    }

    changedRows[type + index] = {
        ROLL_FC_CD: parseInt(rollFcCd),
        CHECKEDBOX_VALUE: checkboxValue,
        REPLY_FROM_MATL_DEPT: materialReply
    };

    localStorage.setItem(
        "forecastData",
        JSON.stringify(Object.values(changedRows))
    );
}

$(document).on(
    "change keyup",
    "input[id^='chkProdSalesFcDoneDate'], " +
    "input[id^='chkMatlSalesFcDoneDate'], " +
    "input[id^='txtMaterialReplyProd'], " +
    "input[id^='txtMaterialReplyMatl']",
    function () {

        localStorage.setItem("forecastChanged", "true");

        let id = $(this).attr("id");
        let index = id.replace(/\D/g, '');
        let type = "";

        if (id.includes("Prod")) {
            type = "prod";
        } else if (id.includes("Matl")) {
            type = "matl";
        }

        saveForecastData(index, type);
    }
);