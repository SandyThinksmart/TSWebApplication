
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
$(document).ready(function () {
    ManageSessionForUser();
});

$(window).on("load", function () {
    ManageSessionForUser();
  
});
$("input[type='button']").on("click", function () {
   //$('._functionButtons').click(function () {
  
    var getValue = getCookie("Cookieval");
    if (!getValue) {

        window.location.href = PMSLOGINMENUURL;
        return false;
    } else {
        var ajaxTime = new Date().getDate()
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/SessionHandler/ManageSession?CookieValue=" + getValue,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var result = response.isSuccess;
              
                if (result == false) {

                    eraseCookie();
                    window.location.href = PMSLOGINMENUURL;
                    return false;
                } else {
                    $("#hdfLoginId").val(response.message.userID);
                    $("#hdfLocationName").val(response.message.locationName);
                    $("#lblusername").text(response.message.username); //used on Menu Page to show username
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageCommon_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }
   
});
function getSessionURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('readData=');
    result = sParameterName[1];

    return result;
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
function createCookie(name, value, days) {
    if (days) {

        var date = new Date();

        date.setTime(date.getTime() + (days * 23 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();

    }
    else var expires = "";

    document.cookie = name + "=" + value + expires + "; path=/";
}
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function eraseCookie() {

    createCookie("Cookieval", "", -1);

}
function ManageSessionForUser() {
  
   // let cookievalue = getSessionURLParameters(window.location.href);
    var getValue = getCookie("Cookieval");

    if (!getValue) {

        window.location.href = PMSLOGINMENUURL;
        return false;
    } else {
        var ajaxTime = new Date().getDate()
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/SessionHandler/ManageSession?CookieValue=" + getValue,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var result = response.isSuccess;
               
                if (result == false) {

                    eraseCookie();
                    window.location.href = PMSLOGINMENUURL;
                    return false;
                } else {
                   
                    $("#hdfLoginId").val(response.message.userID);
                    $("#hdfLocationName").val(response.message.locationName);
                    $("#lblusername").text(response.message.userName); //used on Menu Page to show username
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageCommon_001").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }
  

}