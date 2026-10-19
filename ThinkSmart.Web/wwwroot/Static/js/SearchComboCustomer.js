
function BindCustomerList($target) {
    // GetActiveCustomerList("", $target)
}
function GetCustomerListByYobina($target)
{

    var searchTextName = $.trim($target.closest("tr").find('input[type=text]').attr('id'));
    var dropdownName = $.trim($target.closest("tr").find('select').attr('id'));

    GetActiveCustomerList($("#" + searchTextName).val(), dropdownName);
}
function GetActiveCustomerList(yobina, $target) {
    debugger;
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetCustomerList?name=" + yobina,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            SetCustomerDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
           
            return false;
        })
}
function SetCustomerDropdown(yobina, $target, jsonResult) {
    debugger;
    var dropdownName =$("#"+ $target);
   
    $("#" + $target).html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $("#" + $target).append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });

}
