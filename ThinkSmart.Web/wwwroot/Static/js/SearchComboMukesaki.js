var _mukesakiCD = 0;
function BindMukesakiList($target) {
    // GetActiveOperatorList("", $target)
}

function GetMukesakiListByYobina($target) {
    debugger;
    var searchTextName = $.trim($target.closest("tr").find('input[type=text]').attr('id'));
    var dropdownName = $.trim($target.closest("tr").find('select').attr('id'));

    GetActiveMukesakiList($("#" + searchTextName).val(), dropdownName);
}
function GetActiveMukesakiList(yobina, $target) {
    debugger;
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetMukesakiList?name=" + yobina + "&mukesakiCD=" + _mukesakiCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMukesakiDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            return false;
        })
}
function GetMukesakiList(mukesakiCD) {
    var returnList = [];
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetMukesakiList?name=" + "&mukesakiCD=" + mukesakiCD,
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
function BindMukesakiDropdown(yobina, $target, jsonResult) {
    debugger;
    var dropdownName = $("#" + $target);

    $("#" + $target).html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $("#" + $target).append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });

}

function SetMukesakiDropdown($target, mukesakiCD) {
    debugger;
    var jsonResult = GetMukesakiList(mukesakiCD); //GET ALL ACTIVE AND INACTIVE EMPLOYEE LIST
    
    $target.html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });
    $target.val(mukesakiCD);
}

