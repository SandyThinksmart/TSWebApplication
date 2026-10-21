var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";

$(document).ready(function () {
    window.resizeTo(520, 710);
    var outputCD = getURLParameters(window.location.href).trim();
   
    if (parseInt(outputCD) > 0)
    {
        GetLocationCDByUserID();
        GetConditionsList(outputCD);
        if ($("#hdfLocationName").val() != "") {
            $("#txtUserLocation").val($("#hdfLocationName").val());
        }
    }
    //SetLocationList();
    //SetNextProcessList();
});
function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('outputCD=');
    result = sParameterName[1];

    return result;
}
//$("#btnCloseWindow").click(function () {
  
//    // window.location.href = "/CSVGeneration/CSVTitleList";
//    self.close();
//});
function GetConditionsList(outputCD) {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/CSVGeneration/GetCSVConditionsList?outputCD=" + outputCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess) {
                var getData = response.message[0];
                debugger;
                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/CSVGeneration/GridSearchConditions",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(getData)

                }
                $.ajax(settings1)
                    .done(function (response1) {
                    
                       
                        if (response1.message) {
                            $("#divConditionGrid").html(response1.message);

                        }
                        var result = response1;
                        if (result.wherecondition.length > 0)
                        {
                           
                            var condition_0 = result.wherecondition.filter(r => r.cd == 0);
                            var condition_1 = result.wherecondition.filter(r => r.cd == 1);
                            var condition_2 = result.wherecondition.filter(r => r.cd == 2);
                            var condition_3 = result.wherecondition.filter(r => r.cd == 3);
                            var condition_4 = result.wherecondition.filter(r => r.cd == 4);
                            var getRowID = $('#tblMain table tr[condition-id]').map(function () {
                                return $(this).attr('id');
                            }).get();

                            if (getRowID.length > 0)
                            {
                                for (var i = 0; i <= getRowID.length - 1; i++)
                                {
                                    var getFilterType = $("#" + getRowID[i]).attr("condition-id");
                                    var dropdownId = $('#' + getRowID[i]).find('select').attr('id');

                                    if (getFilterType == 0)
                                    {
                                        BindConditionDropDown(dropdownId, condition_0);
                                    }else if (getFilterType == 1) {
                                        var textid = $('#' + getRowID[i]).find('input[type="text"]').attr('id');
                                        $("#" + textid).keyup(function (event) {
                                            RestrictToValidDecimalFormat($(this), 9, 3);// number format set to text
                                        });
                                        BindConditionDropDown(dropdownId, condition_1);
                                    } else if (getFilterType == 2) {
                                        var textid = $('#' + getRowID[i]).find('input[type="text"]').attr('id');
                                        $("#" + textid).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
                                        BindConditionDropDown(dropdownId, condition_2);
                                    }else if (getFilterType == 3) {
                                      
                                        var textid = $('#' + getRowID[i]).find('input[type="text"]').attr('id');
                                        $("#" + textid).mask("9999/99/99 99:99:99", { placeholder: "YYYY/MM/DD HH:MM:SS" });
                                        BindConditionDropDown(dropdownId, condition_4);
                                    } else if (getFilterType == 4) {
                                        
                                        var startTextId = $('#' + getRowID[i]).closest("td").find('input[type="text"]').first().attr('id');
                                        var endTextId = $('#' + getRowID[i]).closest("td").find('input[type="text"]').last().attr('id');
                                        $("#" + startTextId).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
                                        $("#" + endTextId).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
                                        BindConditionDropDown(dropdownId, condition_4);
                                   
                                    } else if (getFilterType == 5)
                                    {
                                         var startTextId = $('#' + getRowID[i]).closest("td").find('input[type="text"]').first().attr('id');
                                         var endTextId = $('#' + getRowID[i]).closest("td").find('input[type="text"]').last().attr('id');
                                             $("#" + startTextId).mask("9999/99/99 99:99:99", { placeholder: "YYYY/MM/DD HH:MM:SS" });
                                             $("#" + endTextId).mask("9999/99/99 99:99:99", { placeholder: "YYYY/MM/DD HH:MM:SS" });
                                         BindConditionDropDown(dropdownId, condition_4);
                                    }

                                }
                            }

                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {
                        return false;
                    });
            }



        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CSV_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });


}
function BindConditionDropDown(id, jsonResult)
{
    //$("#" + id).html("").append($('<option value="0"></option>'));
    $("#" + id).html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#" + id).append($('<option value="' + jsonObj.value + '"> ' + jsonObj.name + '</option>'));
    });
}
$("#btnPDF").click(function () {
     if ($("#hfdSqlType").val() == 1) {
        GenerateFileWithProcedure("pdf");
      
    } else
    {
         GenerateFileWithSQL("pdf");
    }
 
});



$("#btnCSV").click(function () {
    if ($("#hfdSqlType").val() == 1) {
        GenerateFileWithProcedure("csv");
      
    } else
    {
        GenerateFileWithSQL("csv");
    }
 
});
const DownloadCSV = (function () {
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";
    return function (url, fileName) {
        a.href = url;
        a.download = fileName;
        a.click();
    };
}());

function GenerateFileWithProcedure(getFileType)
{
    var errorMessage = "";
    var focusID = "";

    var parameters = [];
    var getRowID = $('#tblMain table tr[condition-id]').map(function () {
        return $(this).attr('id');
    }).get();
    if (getRowID.length > 0) {
        for (var i = 0; i <= getRowID.length - 1; i++)
        {
            debugger;
            var getFilterType = $("#" + getRowID[i]).attr("condition-id");
            var textID1 = $('#' + getRowID[i]).find('input[type="text"]').first().attr('id');
            var columnID = $('#' + getRowID[i]).closest('table').closest('td').prev('td').find('label').attr('id');
            var conditionValue1 = $("#" + textID1).val();
            var columnName = $("#" + columnID).text();
            var on_Off_Type = null;
            if (conditionValue1 != "")
            {
                if (getFilterType == 2) {
                    if (!checkDate($("#" + textID1))) {
                        errorMessage = $("#lblErrorMessageE_CSV_004").text().trim(); // if date in invalid popup will display
                        focusID = textID1;
                    }
                } else if (getFilterType == 3)
                {
                    if (!checkDateTime($("#" + textID1)) && errorMessage == "") {
                        errorMessage = $("#lblErrorMessageE_CSV_004").text().trim(); // if date in invalid popup will display
                        focusID = textID1;
                    }

                }else if (getFilterType == 6)
                {
                    var getFlagID = $("#" + getRowID[i]).find('td').attr('flag-id');
                    if (getFlagID == 0) {
                        on_Off_Type = 0;
                        if ($("#radioButtonON" + parseInt(i + 1)).prop("checked") == true) {
                            conditionValue1 = $("#radioButtonON" + parseInt(i + 1)).val();
                        } else {
                            conditionValue1 = $("#radioButtonOFF" + parseInt(i + 1)).val();
                        }
                    } else
                    {
                        on_Off_Type = 1;
                        if ($("#radioButtonNULL" + parseInt(i + 1)).prop("checked") == true) {
                            conditionValue1 = $("#radioButtonNULL" + parseInt(i + 1)).val();
                        } else {
                            conditionValue1 = $("#radioButtonNOTNULL" + parseInt(i + 1)).val();
                        }
                    }
                }
                var jsonData = ({
                    WHERE_CONDITION_TYPE: getFilterType,
                    ON_OFF_FLAG_TYPE: on_Off_Type,
                    WHERE_CONDITION_COLUMN: columnName,
                    WHERE_CONDITION_VALUE: conditionValue1,
                    PAGE_SIZE: $("#hfdPageSize").val(),
                    PAGE_ORIENTATION: $("#hfdPageOrientation").val()
                });
                parameters.push(jsonData);
            }
           
        }
    }
    if (parameters.length == 0)
    {
        showMessageDialog($("#lblErrorMessageE_CSV_002").text().trim(), "", WARNING);
        return false;
    }

    if (errorMessage != "") {
        showMessageDialog(errorMessage, focusID, WARNING);
        return false;
    }
   
    var titleName = "";
    var urlPath = "";
    if (getFileType == "csv") {
        urlPath = "/CSVGeneration/CreateCSV";
    } else {
        urlPath = "/CSVGeneration/CreatePDF";
        titleName = $("#lblGridHeader").text();
    }

    $("#LoadingImage").show();
    var ajaxTime = new Date().getDate();
    var jsonData = JSON.stringify({
        TITLE: titleName,
        SQL_PLSQL_TYPE: $("#hfdSqlType").val(),
        SQL_PLSQL_NAME: $("#hfdSqlName").val(),
        WHERE_CONDITION_LIST: parameters,
        CSV_FILE_NAME: $("#hfdFileName").val(),
        ADD_DATE_FLAG: $("#hfdDateFlag").val(),
        ADD_TIME_FLAG: $("#hfdTimeFlag").val(),
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        timeout: 240000, // 4 minutes
        dataType: "json",
        url: urlPath,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            var result = response;
            $("#LoadingImage").hide();
            if (result.downloadpath != "") {
                DownloadCSV(result.downloadpath, result.filename);
            } else {
                showMessageDialog($("#lblErrorMessageE_CSV_003").text().trim(), "", WARNING);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            $("#LoadingImage").hide();
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_CSV_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

}
function GenerateFileWithSQL(getFileType)
{
    debugger;
    var errorMessage = "";
    var focusID = "";
    var textCondition = "";
    var txtAND = " AND ";
    var txtWHERE = " WHERE ";
    var getRowID = $('#tblMain table tr[condition-id]').map(function () {
        return $(this).attr('id');
    }).get();

    //if ($("#ddlLocationName").val() > 0)
    //{
       // textCondition = "INPUT_LOCATION_CD = " + $("#ddlLocationName").val();
    // }
    if (parseInt($("#locationCDvalue").val()) > 0)
    {
        textCondition = "INPUT_LOCATION_CD = " + $("#locationCDvalue").val();
    }

    //if ($("#ddlNextProcess").val() > 0) {
    //    if (textCondition != "") {
    //        textCondition = textCondition + txtAND; // AND added in where condition for text
    //    }
    //    textCondition = "NEXT_PROCESS = " + $("#ddlNextProcess").val();
    //}
    textCondition += " AND NEXT_PROCESS = " + $("#tensouProcessvalue").val();
    

    if (getRowID.length > 0) {
        for (var i = 0; i <= getRowID.length - 1; i++) {
            debugger;
            var getFilterType = $("#" + getRowID[i]).attr("condition-id");
            var dropdownID = $('#' + getRowID[i]).find('select').attr('id');
            var textID1 = $('#' + getRowID[i]).find('input[type="text"]').first().attr('id');
            var textID2 = $('#' + getRowID[i]).find('input[type="text"]').last().attr('id');
            var columnID = $('#' + getRowID[i]).closest('table').closest('td').prev('td').find('label').attr('id');
            var conditionValue1 = $("#" + textID1).val();
            var conditionValue2 = $("#" + textID2).val();
            var columnName = $("#" + columnID).text();


            if (getFilterType == 0 && conditionValue1 != "") {
                if (textCondition != "") {
                    textCondition = textCondition + txtAND; // AND added in where condition for text
                }
                if ($("#" + dropdownID).val().includes("NNN")) {

                    textCondition += " LOWER(" + columnName + ") " + $("#" + dropdownID).val().replace("NNN", conditionValue1.toLowerCase());
                } else {
                    textCondition += " LOWER(" + columnName + ") " + $("#" + dropdownID).val() + "'" + conditionValue1.toLowerCase() + "'";
                }


            } else if (getFilterType == 1 && conditionValue1 != "") {
                if (textCondition != "") {
                    textCondition = textCondition + txtAND; // AND added in where condition for numeric
                }
                textCondition += columnName + $("#" + dropdownID).val() + conditionValue1;
            } else if (getFilterType == 2 && conditionValue1 != "") {
                if (!checkDate($("#" + textID1))) {
                    errorMessage = $("#lblErrorMessageE_CSV_004").text().trim(); // if date in invalid popup will display
                    focusID = textID1;
                }

                if (textCondition != "") {
                    textCondition = textCondition + txtAND; // AND added in where condition with date format condition YYYY/MM/DD
                }
                textCondition += "TRUNC(" + columnName + ")" +$("#" + dropdownID).val() + "TO_DATE('" + conditionValue1 + "','YYYY/MM/DD')";
            } else if (getFilterType == 3 && conditionValue1 != "") {
                if (!checkDateTime($("#" + textID1)) && errorMessage == "")
                {
                    errorMessage = $("#lblErrorMessageE_CSV_004").text().trim(); // if date in invalid popup will display
                    focusID = textID1;
                }
                if (textCondition != "") {
                    textCondition = textCondition + txtAND; // AND added in where condition with date format condition YYYY/MM/DD HH24:MI:SS
                }
                textCondition += columnName + $("#" + dropdownID).val() + "TO_DATE('" + conditionValue1 + "','YYYY/MM/DD HH24:MI:SS')";
            } else if (getFilterType == 4 && conditionValue1 != "" && conditionValue2 != "") {
                if (!checkDate($("#" + textID1)) && errorMessage == "")
                {
                    errorMessage = $("#lblErrorMessageE_CSV_005").text().trim(); // if date in invalid popup will display
                    focusID = textID1;
                }
                if (!checkDate($("#" + textID2)) && errorMessage == "") {
                    errorMessage = $("#lblErrorMessageE_CSV_006").text().trim(); // if date in invalid popup will display
                    focusID = textID2;
                }
                if (textCondition != "") {
                    textCondition = textCondition + txtAND; // AND added in where condition
                }
                textCondition += "TO_DATE(" + columnName + ",'YYYY/MM/DD HH24:MI:SS') BETWEEN TO_DATE('" + conditionValue1 + " 00:00:01','YYYY/MM/DD HH24:MI:SS')" + " AND TO_DATE('" + conditionValue2 + " 23:59:59','YYYY/MM/DD HH24:MI:SS')";
            } else if (getFilterType == 5 && conditionValue1 != "" && conditionValue2 != "") {

                if (!checkDateTime($("#" + textID1)) && errorMessage == "") {
                    errorMessage = $("#lblErrorMessageE_CSV_005").text().trim(); // if date in invalid popup will display
                    focusID = textID1;
                }
                if (!checkDateTime($("#" + textID2)) && errorMessage == "") {
                    errorMessage = $("#lblErrorMessageE_CSV_006").text().trim(); // if date in invalid popup will display
                    focusID = textID2;
                }

                if (textCondition != "") {
                    textCondition = textCondition + txtAND; // AND added in where condition
                }
                textCondition += columnName + " BETWEEN TO_DATE('" + conditionValue1 + "','YYYY/MM/DD HH24:MI:SS') AND TO_DATE('" + conditionValue2 + "','YYYY/MM/DD HH24:MI:SS')";
            } else if (getFilterType == 6) {
                var type = "";
                var getFlagID = $("#" + getRowID[i]).find('td').attr('flag-id');
                if (getFlagID == 0) {
                    if ($("#radioButtonON" + parseInt(i + 1)).prop("checked") == true) {
                        type = $("#radioButtonON" + parseInt(i + 1)).val();
                    } else {
                        type = $("#radioButtonOFF" + parseInt(i + 1)).val();
                    }

                    if (textCondition != "") {
                        textCondition = textCondition + txtAND; // AND added in where condition
                    }
                    textCondition += columnName + " =  " + type;
                } else {
                    if ($("#radioButtonNULL" + parseInt(i + 1)).prop("checked") == true) {
                        type = $("#radioButtonNULL" + parseInt(i + 1)).val();
                    } else {
                        type = $("#radioButtonNOTNULL" + parseInt(i + 1)).val();
                    }

                    if (textCondition != "") {
                        textCondition = textCondition + txtAND; // AND added in where condition
                    }
                    textCondition += columnName + " " + type;
                }

            }
        }
    }
  
    if (textCondition == "") {
        showMessageDialog($("#lblErrorMessageE_CSV_002").text().trim(), "", WARNING);
        return false;
    }
    if (errorMessage != "")
    {
        showMessageDialog(errorMessage, focusID, WARNING);
        return false;
    }
    if (textCondition != "") {
        textCondition = txtWHERE + textCondition;
    }

    var urlPath = "";
    var titleName = "";
    if (getFileType == "csv") {
        urlPath = "/CSVGeneration/CreateCSV";
    } else
    {
        urlPath = "/CSVGeneration/CreatePDF";
        titleName = $("#lblGridHeader").text();

    }
    $("#LoadingImage").show();
    var ajaxTime = new Date().getDate();
    var jsonData = JSON.stringify({
        TITLE: titleName,
        SQL_PLSQL_TYPE: $("#hfdSqlType").val(),
        SQL_PLSQL_NAME: $("#hfdSqlName").val() + textCondition,
        CSV_FILE_NAME: $("#hfdFileName").val(),
        ADD_DATE_FLAG: $("#hfdDateFlag").val(),
        ADD_TIME_FLAG: $("#hfdTimeFlag").val(),
        PAGE_SIZE: $("#hfdPageSize").val(),
        PAGE_ORIENTATION: $("#hfdPageOrientation").val(),
        REPORT_TITLE_FONT: $("#hfdTitleFont").val(),
        REPORT_HEADER_FONT: $("#hfdHeaderFont").val(),
        REPORT_DETAILS_FONT: $("#hfdDetailsFont").val(),
        USER_ID: $("#hdfLoginId").val()
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        timeout: 240000, // 4 minutes
        dataType: "json",
        url: urlPath,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            var result = response;
            $("#LoadingImage").hide();
            if (result.downloadpath != "") {
                DownloadCSV(result.downloadpath, result.filename);
            } else {
                showMessageDialog($("#lblErrorMessageE_CSV_003").text().trim(), "", WARNING);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            $("#LoadingImage").hide();
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_CSV_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;

        });
  
      
}
function checkDate(field) {
    debugger;
    var minYear = 2000;//2012
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
function checkDateTime(field) {
    var minYear = 2019;
    var maxYear = 2100;
    // regular expression to match required date format
    //re = /^(\d{4})\/(\d{1,2})\/(\d{1,2})$/;
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]:[0-5]{0,1}[0-9]$/
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
            } else {
                return true;
            }
        }
    }
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


function SetLocationList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/CSVGeneration/GetLocationList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindLocationDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CSV_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindLocationDropdown(jsonResult) {
    
    $("#ddlLocationName").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlLocationName").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));

    });
}
function GetLocationCDByUserID()
{
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/CSVGeneration/GetLocationCDByUserID?userID=" + $("#hdfLoginId").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            debugger;
            if (response.isSuccess == true)
            {
                $("#locationCDvalue").val(response.message);
            }
                    
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CSV_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function SetNextProcessList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/CSVGeneration/GetNextProcessList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindNextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CSV_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindNextProcessDropdown(jsonResult) {
   
    $("#ddlNextProcess").html("").append($('<option value="0"></option>'));
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
        });
    }
   
}