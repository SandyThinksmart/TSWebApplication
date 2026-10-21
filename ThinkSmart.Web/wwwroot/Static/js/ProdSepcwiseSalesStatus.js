var chartLabels = [];
var chart1Row1Data = [];
var chart1Row2Data = [];
var chart1Row3Data = [];
var chart1Row4Data = [];
var chart2Row1Data = [];
var chart2Row2Data = [];
var chart1Data;
var chart2Data;
$(document).ready(function () {
    const params = new URLSearchParams(window.location.search);
    productCD = params.get('ProductCD');
    if (productCD)
    {
        SetDateRange();
        GetProdSpecDetailsByProductCD(productCD);
        GetMonthWiseProdStockByProductCD(productCD);
        GetPendingStockByProductCD(productCD);
        BindChartsData(chart1Data, chart2Data);
    }
});

function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('product_CD=');
    result = sParameterName[1];

    return result;
}
function SetDateRange()
{
    var today = new Date();
    var currentMonth = String(today.getMonth()+1).padStart(2, '0');
    var lastMonth = String(today.getMonth()).padStart(2, '0');
    var lastYear = (today.getFullYear() - 1) + "/" + currentMonth + $("#lblSymbolRange").val() + today.getFullYear() + "/" + lastMonth;
    var lastToLastYear = (today.getFullYear() - 2) + "/" + currentMonth + $("#lblSymbolRange").val() + (today.getFullYear()-1) + "/" + lastMonth;

    $("#lblShukkaLastYearRange").text(lastYear);
    $("#lblShukkaLastToLastYearRange").text(lastToLastYear); // Product Shukka


    $("#lblStockLastYearRange").text(lastYear);
    $("#lblStockLastToLastYearRange").text(lastToLastYear);  // Product Stock

    $("#lblPendingStockLastYearRange").text(lastYear);
    $("#lblPendingStockLastToLastYearRange").text(lastToLastYear);  // Pending Stock
    
    

    $("#lblLastYearShukkaDateRange").text(lastYear);
    $("#lblLastToLastYearShukkaDateRange").text(lastToLastYear); 
    $("#lblLastYearStockDateRange").text(lastYear);
    $("#lblLastToLastYearStockDateRange").text(lastToLastYear); // Chart 1

    $("#lblReaminStockLastYearRange").text(lastYear);
    $("#lblReaminStockLastToLastYearRange").text(lastToLastYear); // Chart 2
    
    
}
function GetProdSpecDetailsByProductCD(productCD)
  {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProdSpecwiseSalesStatus/GetProdSpecDetailsByProductCD?productCD=" + productCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true)
            {
                FillProductGrid(jsonResult);
                SetGridHeader();
                if (jsonResult.shukkA_DETAILS != null) {
                    //First Row
                    $("#lblRow1_Month1").text(jsonResult.shukkA_DETAILS[0].montH1_QTY.toFixed(1));
                    $("#lblRow1_Month2").text(jsonResult.shukkA_DETAILS[0].montH2_QTY.toFixed(1));
                    $("#lblRow1_Month3").text(jsonResult.shukkA_DETAILS[0].montH3_QTY.toFixed(1));
                    $("#lblRow1_Month4").text(jsonResult.shukkA_DETAILS[0].montH4_QTY.toFixed(1));
                    $("#lblRow1_Month5").text(jsonResult.shukkA_DETAILS[0].montH5_QTY.toFixed(1));
                    $("#lblRow1_Month6").text(jsonResult.shukkA_DETAILS[0].montH6_QTY.toFixed(1));
                    $("#lblRow1_Month7").text(jsonResult.shukkA_DETAILS[0].montH7_QTY.toFixed(1));
                    $("#lblRow1_Month8").text(jsonResult.shukkA_DETAILS[0].montH8_QTY.toFixed(1));
                    $("#lblRow1_Month9").text(jsonResult.shukkA_DETAILS[0].montH9_QTY.toFixed(1));
                    $("#lblRow1_Month10").text(jsonResult.shukkA_DETAILS[0].montH10_QTY.toFixed(1));
                    $("#lblRow1_Month11").text(jsonResult.shukkA_DETAILS[0].montH11_QTY.toFixed(1));
                    $("#lblRow1_Month12").text(jsonResult.shukkA_DETAILS[0].montH12_QTY.toFixed(1));
                    $("#lblRow1MIN_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[0].miN_QTY.toFixed(1));
                    $("#lblRow1MAX_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[0].maX_QTY.toFixed(1));
                    $("#lblRow1AVG_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[0].avG_QTY.toFixed(1));
                    $("#lblRow1TOTAL_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[0].totaL_QTY.toFixed(1));

                    //Second Row
                    $("#lblRow2_Month1").text(jsonResult.shukkA_DETAILS[1].montH1_QTY.toFixed(1));
                    $("#lblRow2_Month2").text(jsonResult.shukkA_DETAILS[1].montH2_QTY.toFixed(1));
                    $("#lblRow2_Month3").text(jsonResult.shukkA_DETAILS[1].montH3_QTY.toFixed(1));
                    $("#lblRow2_Month4").text(jsonResult.shukkA_DETAILS[1].montH4_QTY.toFixed(1));
                    $("#lblRow2_Month5").text(jsonResult.shukkA_DETAILS[1].montH5_QTY.toFixed(1));
                    $("#lblRow2_Month6").text(jsonResult.shukkA_DETAILS[1].montH6_QTY.toFixed(1));
                    $("#lblRow2_Month7").text(jsonResult.shukkA_DETAILS[1].montH7_QTY.toFixed(1));
                    $("#lblRow2_Month8").text(jsonResult.shukkA_DETAILS[1].montH8_QTY.toFixed(1));
                    $("#lblRow2_Month9").text(jsonResult.shukkA_DETAILS[1].montH9_QTY.toFixed(1));
                    $("#lblRow2_Month10").text(jsonResult.shukkA_DETAILS[1].montH10_QTY.toFixed(1));
                    $("#lblRow2_Month11").text(jsonResult.shukkA_DETAILS[1].montH11_QTY.toFixed(1));
                    $("#lblRow2_Month12").text(jsonResult.shukkA_DETAILS[1].montH12_QTY.toFixed(1));
                    $("#lblRow2MIN_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[1].miN_QTY.toFixed(1));
                    $("#lblRow2MAX_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[1].maX_QTY.toFixed(1));
                    $("#lblRow2AVG_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[1].avG_QTY.toFixed(1));
                    $("#lblRow2TOTAL_SHUKKA_QTY").text(jsonResult.shukkA_DETAILS[1].totaL_QTY.toFixed(1));

                    //Third Row
                    $("#lblSum_Month1").text((jsonResult.shukkA_DETAILS[2].montH1_QTY).toFixed(1));
                    $("#lblSum_Month2").text((jsonResult.shukkA_DETAILS[2].montH2_QTY).toFixed(1));
                    $("#lblSum_Month3").text((jsonResult.shukkA_DETAILS[2].montH3_QTY).toFixed(1));
                    $("#lblSum_Month4").text((jsonResult.shukkA_DETAILS[2].montH4_QTY).toFixed(1));
                    $("#lblSum_Month5").text((jsonResult.shukkA_DETAILS[2].montH5_QTY).toFixed(1));
                    $("#lblSum_Month6").text((jsonResult.shukkA_DETAILS[2].montH6_QTY).toFixed(1));
                    $("#lblSum_Month7").text((jsonResult.shukkA_DETAILS[2].montH7_QTY).toFixed(1));
                    $("#lblSum_Month8").text((jsonResult.shukkA_DETAILS[2].montH8_QTY).toFixed(1));
                    $("#lblSum_Month9").text((jsonResult.shukkA_DETAILS[2].montH9_QTY).toFixed(1));
                    $("#lblSum_Month10").text((jsonResult.shukkA_DETAILS[2].montH10_QTY).toFixed(1));
                    $("#lblSum_Month11").text((jsonResult.shukkA_DETAILS[2].montH11_QTY).toFixed(1));
                    $("#lblSum_Month12").text((jsonResult.shukkA_DETAILS[2].montH12_QTY).toFixed(1));
                    $("#lblSum_AVG_SHUKKA_QTY").text((jsonResult.shukkA_DETAILS[2].avG_QTY).toFixed(1));
                    $("#lblSum_MAX_SHUKKA_QTY").text((jsonResult.shukkA_DETAILS[2].maX_QTY).toFixed(1));
                    $("#lblSum_MIN_SHUKKA_QTY").text((jsonResult.shukkA_DETAILS[2].miN_QTY).toFixed(1));
                    $("#lblSum_TOTAL_SHUKKA_QTY").text((jsonResult.shukkA_DETAILS[2].totaL_QTY).toFixed(1));
                    var getResult = jsonResult.shukkA_DETAILS.slice(0, 2).map(function (item) {
                        return {
                            MONTH1_QTY: item.montH1_QTY, MONTH2_QTY: item.montH2_QTY, MONTH3_QTY: item.montH3_QTY, MONTH4_QTY: item.montH4_QTY, MONTH5_QTY: item.montH5_QTY, MONTH6_QTY: item.montH6_QTY
                            ,MONTH7_QTY: item.montH7_QTY, MONTH8_QTY: item.montH8_QTY, MONTH9_QTY: item.montH9_QTY, MONTH10_QTY: item.montH10_QTY, MONTH11_QTY: item.montH11_QTY, MONTH12_QTY: item.montH12_QTY
                        };
                    });
                    chart1Data=getResult;
                }
               
            }
           
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ProdSpec_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function GetMonthWiseProdStockByProductCD(productCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProdSpecwiseSalesStatus/GetMonthWiseProdStockByProductCD?productCD=" + productCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
                      
            if (jsonResult != null) {
                //jsonResult[0].montH1_QTY = 3;
                //jsonResult[1].montH1_QTY = 2;
                //First Row
                $("#lblProdRow1_Month1").text(jsonResult[0].montH1_QTY.toFixed(1));
                $("#lblProdRow1_Month2").text(jsonResult[0].montH2_QTY.toFixed(1));
                $("#lblProdRow1_Month3").text(jsonResult[0].montH3_QTY.toFixed(1));
                $("#lblProdRow1_Month4").text(jsonResult[0].montH4_QTY.toFixed(1));
                $("#lblProdRow1_Month5").text(jsonResult[0].montH5_QTY.toFixed(1));
                $("#lblProdRow1_Month6").text(jsonResult[0].montH6_QTY.toFixed(1));
                $("#lblProdRow1_Month7").text(jsonResult[0].montH7_QTY.toFixed(1));
                $("#lblProdRow1_Month8").text(jsonResult[0].montH8_QTY.toFixed(1));
                $("#lblProdRow1_Month9").text(jsonResult[0].montH9_QTY.toFixed(1));
                $("#lblProdRow1_Month10").text(jsonResult[0].montH10_QTY.toFixed(1));
                $("#lblProdRow1_Month11").text(jsonResult[0].montH11_QTY.toFixed(1));
                $("#lblProdRow1_Month12").text(jsonResult[0].montH12_QTY.toFixed(1));
                $("#lblProdRow1MIN_SHUKKA_QTY").text(jsonResult[0].miN_QTY.toFixed(1));
                $("#lblProdRow1MAX_SHUKKA_QTY").text(jsonResult[0].maX_QTY.toFixed(1));
                $("#lblProdRow1AVG_SHUKKA_QTY").text(jsonResult[0].avG_QTY.toFixed(1));
               
                //Second Row
                $("#lblProdRow2_Month1").text(jsonResult[1].montH1_QTY.toFixed(1));
                $("#lblProdRow2_Month2").text(jsonResult[1].montH2_QTY.toFixed(1));
                $("#lblProdRow2_Month3").text(jsonResult[1].montH3_QTY.toFixed(1));
                $("#lblProdRow2_Month4").text(jsonResult[1].montH4_QTY.toFixed(1));
                $("#lblProdRow2_Month5").text(jsonResult[1].montH5_QTY.toFixed(1));
                $("#lblProdRow2_Month6").text(jsonResult[1].montH6_QTY.toFixed(1));
                $("#lblProdRow2_Month7").text(jsonResult[1].montH7_QTY.toFixed(1));
                $("#lblProdRow2_Month8").text(jsonResult[1].montH8_QTY.toFixed(1));
                $("#lblProdRow2_Month9").text(jsonResult[1].montH9_QTY.toFixed(1));
                $("#lblProdRow2_Month10").text(jsonResult[1].montH10_QTY.toFixed(1));
                $("#lblProdRow2_Month11").text(jsonResult[1].montH11_QTY.toFixed(1));
                $("#lblProdRow2_Month12").text(jsonResult[1].montH12_QTY.toFixed(1));
                $("#lblProdRow2MIN_SHUKKA_QTY").text(jsonResult[1].miN_QTY.toFixed(1));
                $("#lblProdRow2MAX_SHUKKA_QTY").text(jsonResult[1].maX_QTY.toFixed(1));
                $("#lblProdRow2AVG_SHUKKA_QTY").text(jsonResult[1].avG_QTY.toFixed(1));

                //Third Row
                $("#lblProdSum_Month1").text((jsonResult[2].montH1_QTY).toFixed(1));
                $("#lblProdSum_Month2").text((jsonResult[2].montH2_QTY).toFixed(1));
                $("#lblProdSum_Month3").text((jsonResult[2].montH3_QTY).toFixed(1));
                $("#lblProdSum_Month4").text((jsonResult[2].montH4_QTY).toFixed(1));
                $("#lblProdSum_Month5").text((jsonResult[2].montH5_QTY).toFixed(1));
                $("#lblProdSum_Month6").text((jsonResult[2].montH6_QTY).toFixed(1));
                $("#lblProdSum_Month7").text((jsonResult[2].montH7_QTY).toFixed(1));
                $("#lblProdSum_Month8").text((jsonResult[2].montH8_QTY).toFixed(1));
                $("#lblProdSum_Month9").text((jsonResult[2].montH9_QTY).toFixed(1));
                $("#lblProdSum_Month10").text((jsonResult[2].montH10_QTY).toFixed(1));
                $("#lblProdSum_Month11").text((jsonResult[2].montH11_QTY).toFixed(1));
                $("#lblProdSum_Month12").text((jsonResult[2].montH12_QTY).toFixed(1));
                $("#lblProdSum_AVG_SHUKKA_QTY").text((jsonResult[2].avG_QTY).toFixed(1));
                $("#lblProdSum_MAX_SHUKKA_QTY").text((jsonResult[2].maX_QTY).toFixed(1));
                $("#lblProdSum_MIN_SHUKKA_QTY").text((jsonResult[2].miN_QTY).toFixed(1));
                var getResult = jsonResult.slice(0, 2).map(function (item) {
                    return {
                        MONTH1_QTY: item.montH1_QTY, MONTH2_QTY: item.montH2_QTY, MONTH3_QTY: item.montH3_QTY, MONTH4_QTY: item.montH4_QTY, MONTH5_QTY: item.montH5_QTY, MONTH6_QTY: item.montH6_QTY
                        , MONTH7_QTY: item.montH7_QTY, MONTH8_QTY: item.montH8_QTY, MONTH9_QTY: item.montH9_QTY, MONTH10_QTY: item.montH10_QTY, MONTH11_QTY: item.montH11_QTY, MONTH12_QTY: item.montH12_QTY
                    };
                });
                chart1Data = chart1Data.concat(getResult);
            }

           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ProdSpec_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function GetPendingStockByProductCD(productCD) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProdSpecwiseSalesStatus/GetPendingStockByProductCD?productCD=" + productCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
           
            if (jsonResult != null) {
                //First Row
                $("#lblPenRow1_Month1").text(jsonResult[0].montH1_QTY.toFixed(1));
                $("#lblPenRow1_Month2").text(jsonResult[0].montH2_QTY.toFixed(1));
                $("#lblPenRow1_Month3").text(jsonResult[0].montH3_QTY.toFixed(1));
                $("#lblPenRow1_Month4").text(jsonResult[0].montH4_QTY.toFixed(1));
                $("#lblPenRow1_Month5").text(jsonResult[0].montH5_QTY.toFixed(1));
                $("#lblPenRow1_Month6").text(jsonResult[0].montH6_QTY.toFixed(1));
                $("#lblPenRow1_Month7").text(jsonResult[0].montH7_QTY.toFixed(1));
                $("#lblPenRow1_Month8").text(jsonResult[0].montH8_QTY.toFixed(1));
                $("#lblPenRow1_Month9").text(jsonResult[0].montH9_QTY.toFixed(1));
                $("#lblPenRow1_Month10").text(jsonResult[0].montH10_QTY.toFixed(1));
                $("#lblPenRow1_Month11").text(jsonResult[0].montH11_QTY.toFixed(1));
                $("#lblPenRow1_Month12").text(jsonResult[0].montH12_QTY.toFixed(1));
                $("#lblPenRow1MIN_SHUKKA_QTY").text(jsonResult[0].miN_QTY.toFixed(1));
                $("#lblPenRow1MAX_SHUKKA_QTY").text(jsonResult[0].maX_QTY.toFixed(1));
                $("#lblPenRow1AVG_SHUKKA_QTY").text(jsonResult[0].avG_QTY.toFixed(1));

                //Second Row
                $("#lblPenRow2_Month1").text(jsonResult[1].montH1_QTY.toFixed(1));
                $("#lblPenRow2_Month2").text(jsonResult[1].montH2_QTY.toFixed(1));
                $("#lblPenRow2_Month3").text(jsonResult[1].montH3_QTY.toFixed(1));
                $("#lblPenRow2_Month4").text(jsonResult[1].montH4_QTY.toFixed(1));
                $("#lblPenRow2_Month5").text(jsonResult[1].montH5_QTY.toFixed(1));
                $("#lblPenRow2_Month6").text(jsonResult[1].montH6_QTY.toFixed(1));
                $("#lblPenRow2_Month7").text(jsonResult[1].montH7_QTY.toFixed(1));
                $("#lblPenRow2_Month8").text(jsonResult[1].montH8_QTY.toFixed(1));
                $("#lblPenRow2_Month9").text(jsonResult[1].montH9_QTY.toFixed(1));
                $("#lblPenRow2_Month10").text(jsonResult[1].montH10_QTY.toFixed(1));
                $("#lblPenRow2_Month11").text(jsonResult[1].montH11_QTY.toFixed(1));
                $("#lblPenRow2_Month12").text(jsonResult[1].montH12_QTY.toFixed(1));
                $("#lblPenRow2MIN_SHUKKA_QTY").text(jsonResult[1].miN_QTY.toFixed(1));
                $("#lblPenRow2MAX_SHUKKA_QTY").text(jsonResult[1].maX_QTY.toFixed(1));
                $("#lblPenRow2AVG_SHUKKA_QTY").text(jsonResult[1].avG_QTY.toFixed(1));

                //Third Row
                $("#lblPenSum_Month1").text((jsonResult[2].montH1_QTY).toFixed(1));
                $("#lblPenSum_Month2").text((jsonResult[2].montH2_QTY).toFixed(1));
                $("#lblPenSum_Month3").text((jsonResult[2].montH3_QTY).toFixed(1));
                $("#lblPenSum_Month4").text((jsonResult[2].montH4_QTY).toFixed(1));
                $("#lblPenSum_Month5").text((jsonResult[2].montH5_QTY).toFixed(1));
                $("#lblPenSum_Month6").text((jsonResult[2].montH6_QTY).toFixed(1));
                $("#lblPenSum_Month7").text((jsonResult[2].montH7_QTY).toFixed(1));
                $("#lblPenSum_Month8").text((jsonResult[2].montH8_QTY).toFixed(1));
                $("#lblPenSum_Month9").text((jsonResult[2].montH9_QTY).toFixed(1));
                $("#lblPenSum_Month10").text((jsonResult[2].montH10_QTY).toFixed(1));
                $("#lblPenSum_Month11").text((jsonResult[2].montH11_QTY).toFixed(1));
                $("#lblPenSum_Month12").text((jsonResult[2].montH12_QTY).toFixed(1));
                $("#lblPenSum_AVG_SHUKKA_QTY").text((jsonResult[2].avG_QTY).toFixed(1));
                $("#lblPenSum_MAX_SHUKKA_QTY").text((jsonResult[2].maX_QTY).toFixed(1));
                $("#lblPenSum_MIN_SHUKKA_QTY").text((jsonResult[2].miN_QTY).toFixed(1));
                var getResult = jsonResult.slice(0, 2).map(function (item) {
                    return {
                        MONTH1_QTY: item.montH1_QTY, MONTH2_QTY: item.montH2_QTY, MONTH3_QTY: item.montH3_QTY, MONTH4_QTY: item.montH4_QTY, MONTH5_QTY: item.montH5_QTY, MONTH6_QTY: item.montH6_QTY
                        , MONTH7_QTY: item.montH7_QTY, MONTH8_QTY: item.montH8_QTY, MONTH9_QTY: item.montH9_QTY, MONTH10_QTY: item.montH10_QTY, MONTH11_QTY: item.montH11_QTY, MONTH12_QTY: item.montH12_QTY
                    };
                });
                chart2Data = getResult;
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_ProdSpec_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function FillProductGrid(jsonResult)
{
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/RollPoMainAnalysis/FillProductGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (result.productGrid.length > 0) {
                $("#pnlProdSpecWiseSalesStatusGrid").html(result.productGrid);
            }
          
        })
        .fail(function (xhr, textStatus, errorThrown) {
           
            return false;
        });

}

function SetGridHeader()
{
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProdSpecwiseSalesStatus/SetGridHeaderWithMonth",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult != null) {
        
                $("#lblDispatchMonth1").text($("#lblDispatchMonth1").text().replace("nn", jsonResult.month_1));
                $("#lblDispatchMonth2").text($("#lblDispatchMonth2").text().replace("nn", jsonResult.month_2));
                $("#lblDispatchMonth3").text($("#lblDispatchMonth3").text().replace("nn", jsonResult.month_3));
                $("#lblDispatchMonth4").text($("#lblDispatchMonth4").text().replace("nn", jsonResult.month_4));
                $("#lblDispatchMonth5").text($("#lblDispatchMonth5").text().replace("nn", jsonResult.month_5));
                $("#lblDispatchMonth6").text($("#lblDispatchMonth6").text().replace("nn", jsonResult.month_6));
                $("#lblDispatchMonth7").text($("#lblDispatchMonth7").text().replace("nn", jsonResult.month_7));
                $("#lblDispatchMonth8").text($("#lblDispatchMonth8").text().replace("nn", jsonResult.month_8));
                $("#lblDispatchMonth9").text($("#lblDispatchMonth9").text().replace("nn", jsonResult.month_9));
                $("#lblDispatchMonth10").text($("#lblDispatchMonth10").text().replace("nn", jsonResult.month_10));
                $("#lblDispatchMonth11").text($("#lblDispatchMonth11").text().replace("nn", jsonResult.month_11));
                $("#lblDispatchMonth12").text($("#lblDispatchMonth12").text().replace("nn", jsonResult.month_12));

                $("#lblProdStockMonth1").text($("#lblProdStockMonth1").text().replace("nn", jsonResult.month_1));
                $("#lblProdStockMonth2").text($("#lblProdStockMonth2").text().replace("nn", jsonResult.month_2));
                $("#lblProdStockMonth3").text($("#lblProdStockMonth3").text().replace("nn", jsonResult.month_3));
                $("#lblProdStockMonth4").text($("#lblProdStockMonth4").text().replace("nn", jsonResult.month_4));
                $("#lblProdStockMonth5").text($("#lblProdStockMonth5").text().replace("nn", jsonResult.month_5));
                $("#lblProdStockMonth6").text($("#lblProdStockMonth6").text().replace("nn", jsonResult.month_6));
                $("#lblProdStockMonth7").text($("#lblProdStockMonth7").text().replace("nn", jsonResult.month_7));
                $("#lblProdStockMonth8").text($("#lblProdStockMonth8").text().replace("nn", jsonResult.month_8));
                $("#lblProdStockMonth9").text($("#lblProdStockMonth9").text().replace("nn", jsonResult.month_9));
                $("#lblProdStockMonth10").text($("#lblProdStockMonth10").text().replace("nn", jsonResult.month_10));
                $("#lblProdStockMonth11").text($("#lblProdStockMonth11").text().replace("nn", jsonResult.month_11));
                $("#lblProdStockMonth12").text($("#lblProdStockMonth12").text().replace("nn", jsonResult.month_12)); 

                $("#lblPendingStockMonth1").text($("#lblPendingStockMonth1").text().replace("nn", jsonResult.month_1));
                $("#lblPendingStockMonth2").text($("#lblPendingStockMonth2").text().replace("nn", jsonResult.month_2));
                $("#lblPendingStockMonth3").text($("#lblPendingStockMonth3").text().replace("nn", jsonResult.month_3));
                $("#lblPendingStockMonth4").text($("#lblPendingStockMonth4").text().replace("nn", jsonResult.month_4));
                $("#lblPendingStockMonth5").text($("#lblPendingStockMonth5").text().replace("nn", jsonResult.month_5));
                $("#lblPendingStockMonth6").text($("#lblPendingStockMonth6").text().replace("nn", jsonResult.month_6));
                $("#lblPendingStockMonth7").text($("#lblPendingStockMonth7").text().replace("nn", jsonResult.month_7));
                $("#lblPendingStockMonth8").text($("#lblPendingStockMonth8").text().replace("nn", jsonResult.month_8));
                $("#lblPendingStockMonth9").text($("#lblPendingStockMonth9").text().replace("nn", jsonResult.month_9));
                $("#lblPendingStockMonth10").text($("#lblPendingStockMonth10").text().replace("nn", jsonResult.month_10));
                $("#lblPendingStockMonth11").text($("#lblPendingStockMonth11").text().replace("nn", jsonResult.month_11));
                $("#lblPendingStockMonth12").text($("#lblPendingStockMonth12").text().replace("nn", jsonResult.month_12));
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_1)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_2)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_3)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_4)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_5)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_6)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_7)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_8)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_9)  );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_10) );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_11) );
                chartLabels.push($("#hfdlblMonth").val().replace("nn", jsonResult.month_12) );
                
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
           
            return false;
        });

}
function BindChartsData(chart1Data, chart2Data) {
   
    //=====================================================Chart 1 ==================================================================
    var chart1Row1Data = [];
    var chart1Row2Data = [];
    var chart1Row3Data = [];
    var chart1Row4Data = [];
    var chart1Row1 = chart1Data[0];
    var chart1Row2 = chart1Data[1];
    var chart1Row3 = chart1Data[2];
    var chart1Row4 = chart1Data[3];

    // Convert the row's values to an array
    chart1Row1Data = Object.values(chart1Row1);
    chart1Row2Data = Object.values(chart1Row2);
    chart1Row3Data = Object.values(chart1Row3);
    chart1Row4Data = Object.values(chart1Row4);

    

    new Chart($("#myChart01"), {
        type: 'line',
        data: {
            labels: chartLabels,
            datasets: [{
                label: '',
                data: chart1Row1Data,
                borderColor: 'rgba(247, 100, 42, 0.91)',
                borderWidth: 1
            },
            {
                label: '',
                data: chart1Row2Data,
                borderColor: 'rgba(241, 210, 10, 0.91)',
                borderWidth: 1
            },
            {
                label: '',
                data: chart1Row3Data,
                borderColor: 'rgba(247, 8, 234, 0.86)',
                borderWidth: 1
            }
                ,
            {
                label: '',
                data: chart1Row4Data,
                borderColor: 'rgba(8, 11, 247, 0.89)',
                borderWidth: 1
            }]

        },
        options: {
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }

        }
    });
    //=====================================================Chart 2 ==================================================================
    var chart2Row1Data = [];
    var chart2Row2Data = [];
    debugger;
    var chart2Row1 = chart2Data[0];
    var chart2Row2 = chart2Data[1];


    // Convert the row's values to an array
    chart2Row1Data = Object.values(chart2Row1);
    chart2Row2Data = Object.values(chart2Row2);
    new Chart($("#myChart02"), {
        type: 'line',
        data: {
            labels: chartLabels,
            datasets: [{
                label: '',
                data: chart2Row1Data,
                borderColor: 'rgba(247, 8, 234, 0.86)',
                borderWidth: 1
            }
                ,
            {
                label: '',
                data: chart2Row2Data,
                borderColor: 'rgba(8, 11, 247, 0.89)',
                borderWidth: 1
            }]

        },

        options: {
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }

        }
    });
}