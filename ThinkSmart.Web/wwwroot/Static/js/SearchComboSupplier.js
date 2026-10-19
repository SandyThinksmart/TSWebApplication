var _supplierCD = 0;
function BindSupplierList($target) {
    // GetActiveOperatorList("", $target)
}

function GetSupplierListByYobina($target) {
    debugger;
    var searchTextName = $.trim($target.closest("tr").find('input[type=text]').attr('id'));
    var dropdownName = $.trim($target.closest("tr").find('select').attr('id'));

    GetActiveSupplierList($("#" + searchTextName).val(), dropdownName);
}
function GetActiveSupplierList(yobina, $target) {
    debugger;
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetSupplierList?name=" + yobina + "&supplierCD=" + _supplierCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindSupplierDropdown(yobina, $target, response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            return false;
        })
}
function GetSupplierList(supplierCD) {
    var returnList = [];
    var ajaxTime = new Date().getTime();


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/GetSupplierList?name=" + "&supplierCD=" + supplierCD,
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
function BindSupplierDropdown(yobina, $target, jsonResult) {
    debugger;
    var dropdownName = $("#" + $target);

    $("#" + $target).html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $("#" + $target).append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });

}

function SetSupplierDropdown($target, supplierCD) {
    debugger;
    var jsonResult = GetSupplierList(supplierCD); //GET ALL ACTIVE AND INACTIVE EMPLOYEE LIST
    
    $target.html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {

        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '"flag="' + jsonObj.iN_ACTIVE_FLAG + '">' + jsonObj.name + '</option>'));
    });
    $target.val(supplierCD);
}

