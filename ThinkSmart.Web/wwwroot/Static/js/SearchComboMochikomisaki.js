var _mochikomisakiCD = 0;
function BindMochikomisakiList($target) {
    // GetActiveOperatorList("", $target)
}

function GetMochikomisakiListByYobina($target) {
    debugger;
    var searchTextName = $.trim($target.closest("tr").find('input[type=text]').attr('id'));
    var dropdownName = $.trim($target.closest("tr").find('select').attr('id'));

    GetActiveMochikomisakiList($("#" + searchTextName).val(), dropdownName);
}
function GetActiveMochikomisakiList(yobina, $target) {
    debugger;
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetMochikomisakiList?name=" + yobina + "&mochikomisakiCD=" + _mochikomisakiCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMochikomisakiDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            return false;
        })
}
function GetMochikomisakiList(mochikomisakiCD) {
    var returnList = [];
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetMochikomisakiList?name=" + "&mochikomisakiCD=" + mochikomisakiCD,
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
function BindMochikomisakiDropdown(yobina, $target, jsonResult) {
    debugger;
    var dropdownName = $("#" + $target);

    $("#" + $target).html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $("#" + $target).append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });

}

function SetMochikomisakiDropdown($target, mochikomisakiCD) {
    debugger;
    var jsonResult = GetMochikomisakiList(mochikomisakiCD); //GET ALL ACTIVE AND INACTIVE EMPLOYEE LIST
    
    $target.html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });
    $target.val(mochikomisakiCD);
}

