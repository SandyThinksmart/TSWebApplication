var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
function onLoad() {
    window.resizeTo(1640, 1070); ///1220,1000
   
    checkAlreadyUserExist();
   // window.moveTo(0, 0);
    
}

function checkAlreadyUserExist() {
    debugger;
    var $callFrom = $(this);
    // var url = '@Url.Action("ValidateUser", "LoginPage", new { Area = "Controllers"} )'; 
    var url1 = "/LoginPage/LoginPage";
   
    if (username.trim() == "") {
        focusFieldID = $(':focus').attr("ErrorLabel");
        message = $("#lblErrorMessageE_LoginUserName").text().trim();
        showMessageDialog(message, focusFieldID, WARNING);



    } else {

        $callFrom.data('requestRunning', true)
        var ajaxTime = new Date().getDate();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: url1,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
          
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonLoginData = response.message;

                if (jsonLoginData == null) {
                    focusFieldID = $(':focus').attr("ErrorLabel");
                    message = $("#lblErrorMessageE_LoginUserName").text().trim();
                    showMessageDialog(message, focusFieldID, WARNING);



                } else {
                    var url = 'http://localhost:6243/Menu.aspx';//@Url.Action("Index", "MenuMaster");

                    window.location.href = url;



                    // window.location.href = url + '/?UserName=' + username + '&&Password=' + password;
                }

            })
    }
}

 /****************************************************************/

function ValidateUser() {
    var $callFrom = $(this);
    // var url = '@Url.Action("ValidateUser", "LoginPage", new { Area = "Controllers"} )'; 
    var url1 = "/LoginPage/ValidateUser";
    var username = $('#txtUserName').val();
    var password = $('#txtPassword').val();
   if (username.trim() == "") {
        focusFieldID = $(':focus').attr("ErrorLabel");
        message = $("#lblErrorMessageE_LoginUserName").text().trim();
        showMessageDialog(message, focusFieldID, WARNING);



    } else {
       
        $callFrom.data('requestRunning', true)
      var ajaxTime = new Date().getDate();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
               dataType: "json",
            url: url1 + '/?UserName=' + username + '&&Password=' + password,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
            data: JSON.stringify({ UserName: username, Password: password })
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonLoginData = response.message;
               
                if (jsonLoginData == null) {
                    focusFieldID = $(':focus').attr("ErrorLabel");
                    message = $("#lblErrorMessageE_LoginUserName").text().trim();
                    showMessageDialog(message, focusFieldID, WARNING);



                } else {
                   // var url = '/MenuMaster/Index';
                    var url = 'http://localhost:6243/Menu.aspx';
                   
                    window.location.href = url;
                   
                    

                   // window.location.href = url + '/?UserName=' + username + '&&Password=' + password;
                }

            })
    }
}

    //$("#LoginButton").click(function (event) {
    //    var $callFrom = $(this);
    //    debugger;

    //    var username = $('#txtUserName').val();
    //    var password = $('#txtPassword').val();
    //    if (username.trim() == "") {
    //            focusFieldID = $(':focus').attr("ErrorLabel");
    //            message = $("#lblErrorMessageE_LoginUserName").text().trim();
    //            showMessageDialog(message, focusFieldID, WARNING);



    //    } else {


    //        Validation(username, password, $callFrom);

    //    }

    //});

    //function Validation(uname, pass, $callFrom) {
    //   var url1 = '@Url.Action("ValidateUser", "LoginPage")';
    //    $callFrom.data('requestRunning', true)
    //    var ajaxTime = new Date().getDate();
    //    var jsonLoginData = "{SYSTEM_NAME : '" + uname + "' , PASSWORD : '" + pass + "' }";
    //       var settings = {
    //        async: true,
    //        crossDomain: true,
    //        cache: false,
    //           dataType: "json",
    //           url: url1,
    //        method: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        //data: "{jsonData : " + jsonData + "}"     

    //        data: JSON.stringify({ SYSTEM_NAME: uname, PASSWORD: pass })
    //    }
    //    $.ajax(settings)
    //        .done(function (response) {
    //            responseTime = new Date().getTime() - ajaxTime;
    //            var jsonLoginData = response.message;
    //            debugger;
    //            if (jsonLoginData == null) {
    //                focusFieldID = $(':focus').attr("ErrorLabel");
    //                message = $("#lblErrorMessageE_LoginUserName").text().trim();
    //                showMessageDialog(message, focusFieldID, WARNING);



    //            }

    //        })

    //}
    function showMessageDialog(message, focusFieldID, messageType) {

        $("#dialogMessageIcon img").hide();
        switch (messageType) {
            case WARNING:
                bootbox.alert({
                    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Warning",
                    message: message,
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
                    title: "<i class='fas fa-exclamation-triangle' aria-hidden='true'></i> " + "Error",
                    message: message,
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
                    title: "<i class='fas fa-check-circle' aria-hidden='true'></i> " + "Success",
                    message: message,
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

        //$("#dialogMessageText").html("").append($("<p>" + message + "<p>"));
        //$("#dialog").dialog({
        //    modal: true,
        //    title: document.title,
        //    buttons: {
        //        Ok: function () {
        //            $(this).dialog("close");
        //            if (focusFieldID !== "") {
        //                $("#" + focusFieldID).focus();
        //            }
        //        }
        //    },
        //    minWidth: 550,
        //    width: 550
        //});
    }
