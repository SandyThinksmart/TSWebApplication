var _operatorCD = 0;
function BindEmployeeList($target) {
   // GetActiveOperatorList("", $target)
}

function GetEmployeeListByYobina($target)
{
    debugger;
    var searchTextName = $.trim($target.closest("tr").find('input[type=text]').attr('id'));
    var dropdownName = $.trim($target.closest("tr").find('select').attr('id'));

    GetActiveOperatorList($("#" + searchTextName).val(), dropdownName);
}
function GetActiveOperatorList(yobina, $target) {
    debugger;
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetOperatorList?name=" + yobina + "&operatorCD=" + _operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindOperatorDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            return false;
        })
}
function GetOperatorList(operatorCD) {
    var returnList = [];
    var ajaxTime = new Date().getTime();
    

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetOperatorList?name=" + "&operatorCD=" + operatorCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
      
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnList = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            return false;
        });
    return returnList;
}
function BindOperatorDropdown(yobina, $target, jsonResult) {
    debugger;
    var dropdownName =$("#"+ $target);
    
    $("#" + $target).html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $("#" + $target).append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });

}

function SetOperatorDropdown($target, operatorCD) {
    debugger;
    var jsonResult = GetOperatorList(operatorCD); //GET ALL ACTIVE AND INACTIVE EMPLOYEE LIST
    //var jsonResult = employeeList.filter(row => row.iN_ACTIVE_FLAG == 0) // GET ACTIVE ONLY 
    //var currentOperatorList = employeeList.filter(r => r.cd == operatorCD); // SELECTED EMPLOYEE DATA AS PER EMPOYEE_CD


    //if (currentOperatorList.length > 0) {
    //    if (currentOperatorList[0].iN_ACTIVE_FLAG == 1) { // IF SELECTED EMPOYEE_CD IS INACTIVE, THEN ONLY THIS INACTIVE IS BINDED IN ALL ACTIVE LIST
    //        jsonResult.push({ cd: currentOperatorList[0].cd, name: currentOperatorList[0].name, yobina: currentOperatorList[0].yobina, iN_ACTIVE_FLAG: currentOperatorList[0].iN_ACTIVE_FLAG });
    //    }
    //}
    $target.html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });
    $target.val(operatorCD);
}

