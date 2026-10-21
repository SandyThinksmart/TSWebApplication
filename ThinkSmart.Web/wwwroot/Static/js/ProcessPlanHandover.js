var $btnChuubanSearch;
var displayProcessPlanData;
var processName;
var chuban1;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var FIRST_PROCESS_CD;

var messageWithRowNo = "";
var messageWithChubanGyouban = "";
var notFoundmessageWithChubanGyouban = "";
var processPlanDetails = [];
var chubbanData = [];
$(document).ready(function () {
    $("#btnInput").removeAttr("DISABLED").removeClass("DISABLED");
    $("#btnCancel").attr("DISABLED", "DISABLED");
    $('#rdoPrcessPlanSearchBeforeEntry').attr("checked", "checked");
    setNextProcessList();
});
function setNextProcessList() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ProcessPlanHandover/GetNextProcessNameList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            processName = response.message;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            //focusFieldID = $(':focus').attr("id");
            //message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            //showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindNextProcessDropdown(rowid, processName) {
    if ($('#rdoPrcessPlanSearchBeforeEntry').is(':checked')) {
        $("#ddlNextProcess" + rowid).html("").append($('<option></option>'));
        $.each(processName, function (key, jsonObj) {
            $("#ddlNextProcess" + rowid).append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        });
       
    } else {
        $("#ddlNextProcess" + rowid).append($('<option value="0" >'  + processName + '</option>'));
    }
   
}
$("#btnOrderNoSrNoSearch").click(function (event) {
    var $callFrom = $(this);
    event.preventDefault();
    var isOrderNoExist = false;
    chubbanData = [];
    var ajaxTime = new Date().getDate();
    for (var i = 1; i <= 30; i++) {
        chuban1 = $("#txtchuuban" + i).val();
        if ($("#txtchuuban" + i).val() != "") {
            chubbanData.push(chuban1);
        }

    }
    if (chubbanData.length > 0) {
        var chuubanList = "";
        var duplicateChuuban = chubbanData.filter((currentValue, currentIndex) =>
            chubbanData.indexOf(currentValue) !== currentIndex);

        if (duplicateChuuban.length > 0) {

            for (var i = 0; i <= duplicateChuuban.length - 1; i++) {
                debugger;
                var getIndex = 1 + parseInt(chubbanData.lastIndexOf(duplicateChuuban[i]));
                if (chuubanList == "") {
                    chuubanList = getIndex;
                } else {
                    chuubanList += ", " + getIndex;
                }
            }
        }
        if (chuubanList != "") {
            message = $("#valMsgFollowingOrNoIsDuplicate").text().replace("NNN", "<br>" + chuubanList);
            showMessageDialog(message, "txtchuuban1", WARNING);
        }
        else {
            getDetailsByChuuban();
        }
    }
    else 
    {
        message = $("#lblOrNoIsRequired").text().trim();
        showMessageDialog(message, "txtchuuban1", WARNING);
        return false;
    }
});

function clickOnBeforeEntryChange() {
    $('#rdoPrcessPlanSearchBeforeEntry').attr("checked", "checked");
    $('#rdoPrcessPlanSearchAfterEntry').removeAttr("checked").removeClass("checked");
    $("#btnCancel").attr("DISABLED", "DISABLED");
    $("#btnInput").removeAttr("DISABLED").removeClass("DISABLED");
}
function clickOnAfterEntryChange() {
    $('#rdoPrcessPlanSearchAfterEntry').attr("checked", "checked");
    $('#rdoPrcessPlanSearchBeforeEntry').removeAttr("checked").removeClass("checked");
    $("#btnInput").attr("DISABLED", "DISABLED");
    $("#btnCancel").removeAttr("DISABLED").removeClass("DISABLED");
}

function getDetailsByChuuban() {
    //$callFrom.data('requestRunning', true)
    messageWithChubanGyouban = "";
    var repeatedMsg1 = "";
    var repeatedMsg2 = "";
    var repeatedMsg3 = "";
    var repeatedMsg4 = "";
    var chubanGyouban1 = "";
    var chubanGyouban2 = "";
    var chubanGyouban3 = "";
    var chubanGyouban4 = "";
    debugger;
    var chuban = [];
    processPlanDetails = [];
    const chubanGyoubanNo = [];
            for (var i = 1; i <= 50; i++) {
        chuban1 = $("#txtchuuban" + i).val();
        if ($('#rdoPrcessPlanSearchBeforeEntry').is(':checked')) {
            displayProcessPlanData = "beforeProcessStart";
        }
        else {
            displayProcessPlanData = "afterProcessStart";
        }
        var jsonPlanData = ({
            CHUUBAN: chuban1,
            DISPLAYPROCESSPLANDATA: displayProcessPlanData
        });
        if ($("#txtchuuban" + i).val() != "") {

            processPlanDetails.push(jsonPlanData);
        }
    }
            var ajaxTime = new Date().getDate();
            var settings = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/ProcessPlanHandover/GetDetailsByChuuban",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(processPlanDetails)
            }
    $.ajax(settings)
        .done(function (response) {
            debugger;
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess) {
                var jsonresult = response.data;
              
                for (let p = 0; p < jsonresult.length; p++) {
                    
                    
                    if (jsonresult[p].responce == "Or No Not Found")
                    {
                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblFollowingOrNoNotFound").text());
                        if (repeatedMsg1 != "") {
                            
                            
                            chubanGyouban1 = chubanGyouban1 + ",<br>" + jsonresult[p].chuubaN_GYOUBAN;
                            
                        }
                        else
                        {
                            repeatedMsg1 = "Found";
                            chubanGyouban1 = jsonresult[p].chuubaN_GYOUBAN;
                            
                        }
                      
                    } 
                    else if (jsonresult[p].responce == "Process Is Completed")
                    {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblFollowingOrNoAlredyCompleted").text());
                        if (repeatedMsg2 != "")
                        {
                        chubanGyouban2 = chubanGyouban2 + ",<br>" + jsonresult[p].chuubaN_GYOUBAN;
                    }
                    else {
                        repeatedMsg2 = "Found";
                        chubanGyouban2 = jsonresult[p].chuubaN_GYOUBAN;

                        } 

                    }
                    else if (jsonresult[p].responce == "Process Is Already Started") {

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblFollowingOrNoProcessAlreadyStarted").text());

                        if (repeatedMsg3 != "") {
                            chubanGyouban3 = chubanGyouban3 + ",<br>" + jsonresult[p].chuubaN_GYOUBAN;
                        } else {
                            repeatedMsg3 = "Found";
                            chubanGyouban3 = jsonresult[p].chuubaN_GYOUBAN;
                        }


                    }
                    else if (jsonresult[p].responce == "Process Is Not Started") {
                    focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblFollowingOrNoProcessNotStarted").text());
                    if (repeatedMsg4 != "") {
                        chubanGyouban4 = chubanGyouban4 + ",<br>" + jsonresult[p].chuubaN_GYOUBAN;
                    }
                    else {
                        repeatedMsg4 = "Found";
                        chubanGyouban4 = jsonresult[p].chuubaN_GYOUBAN;

                    } 
                    }
                   
                        
                                else {
                                    var jsonresult = response.data;
                                    for (let i = 1; i < 51; i++) {
                                    if (jsonresult[p].chuubaN_GYOUBAN == $("#txtchuuban" + parseInt(i)).val().toString()) {
                                        $("#txtCustomer" + parseInt(i)).text(jsonresult[p].customeR_NAME);

                                        $("#txtProdName" + parseInt(i)).text(jsonresult[p].proD_NAME_CUST);
                                        $("#txtShape" + parseInt(i)).text(jsonresult[p].proD_SHAPE);
                                        $("#txtDia" + parseInt(i)).text(jsonresult[p].proD_DIM_D);
                                        $("#txtDiaW" + parseInt(i)).text(jsonresult[p].proD_DIM_W);
                                        $("#txtOther" + parseInt(i)).text(jsonresult[p].proD_DIM_OTHER_STRING);
                                        $("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH);
                                        $("#txtCondition" + parseInt(i)).text(jsonresult[p].proC_CONDITION_STRING);
                                        $("#txtCharge" + parseInt(i)).text(jsonresult[p].charge);
                                        $("#txtActAllPiec" + parseInt(i)).text(jsonresult[p].nO_OF_UNITS + jsonresult[p].uniT_TYPE_STRING);
                                        $("#txtActAllWt" + parseInt(i)).text(jsonresult[p].acT_ALLOC_QTY);
                                        $("#txtActAllDt" + parseInt(i)).text(jsonresult[p].acT_ALLOC_DATE_STRING);
                                        $("#txtProdSpecNo" + parseInt(i)).text(jsonresult[p].proD_SPEC_NO);
                                        $("#txtProcessPlanCd" + parseInt(i)).text(jsonresult[p].procesS_PLAN_CD);
                                        $("#txtLocationCd" + parseInt(i)).text(jsonresult[p].locatioN_CD);
                                        $("#txtActAllPieces" + parseInt(i)).text(jsonresult[p].nO_OF_UNITS);
                                        $("#txtActAllPiecesUnit" + parseInt(i)).text(jsonresult[p].unit);
                                        if ($('#rdoPrcessPlanSearchBeforeEntry').is(':checked')) {
                                            bindNextProcessDropdown(parseInt(i), processName);
                                        }
                                        else {
                                            bindNextProcessDropdown(parseInt(i), jsonresult[p].procesS_NAME_STRING);
                                            $("#ddlNextProcessCD" + parseInt(i)).text(jsonresult[p].nexT_PROCESS_CD);
                                        }
                                    }
                                }
                            }
                }
                if (chubanGyouban1 != "") {
                    messageWithChubanGyouban = $.trim($("#lblFollowingOrNoNotFound").text()).replace("9999999-99", "<br>" + chubanGyouban1);
                }

                if (chubanGyouban2 != "") {

                    if (messageWithChubanGyouban != "") {
                        messageWithChubanGyouban = messageWithChubanGyouban + "<br>" + $.trim($("#lblFollowingOrNoAlredyCompleted").text()).replace("9999999-99", "<br>" + chubanGyouban2);
                    } else {
                        messageWithChubanGyouban = $.trim($("#lblFollowingOrNoAlredyCompleted").text()).replace("9999999-99", "<br>" + chubanGyouban2);
                    }
                }
                if (chubanGyouban3 != "") {


                    if (messageWithChubanGyouban != "") {
                        messageWithChubanGyouban = messageWithChubanGyouban + "<br>" + $.trim($("#lblFollowingOrNoProcessAlreadyStarted").text()).replace("9999999-99", "<br>" + chubanGyouban3);
                    } else {
                        messageWithChubanGyouban = $.trim($("#lblFollowingOrNoProcessAlreadyStarted").text()).replace("9999999-99", "<br>" + chubanGyouban3);
                    }
                }
                if (chubanGyouban4 != "") {

                    if (messageWithChubanGyouban != "") {
                        messageWithChubanGyouban = messageWithChubanGyouban + "<br>" + $.trim($("#lblFollowingOrNoProcessNotStarted").text()).replace("9999999-99", "<br>" + chubanGyouban4);
                    } else {
                        messageWithChubanGyouban = $.trim($("#lblFollowingOrNoProcessNotStarted").text()).replace("9999999-99", "<br>" + chubanGyouban4);
                    }
                }
                if (messageWithChubanGyouban != "") {
                    showMessageDialog(messageWithChubanGyouban, "", WARNING);
                        }
                        
                    }
                    else {
                        message = $.trim($("#lblFollowingOrNoNotFound").text());
                        showMessageDialog(message, "", WARNING);
                    }
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $("#lblSystemException").text().trim();
                    showMessageDialog(message, "", ERROR);

                });
      //  }

}
$("#btnInput").click(function (event) {
    saveProcessPlanHandoverData();

});
$("#btnCancel").click(function (event) {
    DeleteProcessPlanHandoverData();

});
function saveProcessPlanHandoverData() {
    debugger;
    var isddlProcessNotEmpty = false;
    jsonPlanData = [];
    processPlanDetails = [];
    var ajaxTime = new Date().getTime();
    for (var i = 1; i <= 50; i++) {
        FIRST_PROCESS_CD = $("#ddlNextProcess" + i).val();
        if (FIRST_PROCESS_CD == "")
        {
            isddlProcessNotEmpty = false;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblInRowProcessIsRequired").text().trim();
            messageWithRowNo = message.replace("NNN", i);
            showMessageDialog(messageWithRowNo, i, WARNING);
            break;
        }
        else
        {
            isddlProcessNotEmpty = true;
           
        }
    }
    if (isddlProcessNotEmpty == true) { 

    for (var i = 1; i <= 50; i++) {
        var jsonPlanData = ({
            CHUUBAN_GYOUBAN: $("#txtchuuban" + i).val(),
            PROCESS_PLAN_CD: $("#txtProcessPlanCd" + i).text(),
            LOCATION_CD: $("#txtLocationCd" + i).text(),
            NO_OF_UNITS: $("#txtActAllPieces" + i).text(),
            UNIT: $("#txtActAllPiecesUnit" + i).text(),
            ACT_ALLOC_QTY: $("#txtActAllWt" + i).text(),
            PROCESS_CD: $("#ddlNextProcess" + i).val(),
        });
        if ($("#txtProcessPlanCd" + i).text() != "") {
            processPlanDetails.push(jsonPlanData);
        }
    }
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/ProcessPlanHandover/SaveProcessPlanHandoverData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(processPlanDetails)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                processName = response.message;
                if (response.message == "Data Inserted Successfully") {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblProcessStarted").text());
                    showMessageDialog(message, focusFieldID, SUCCESS);
                    btnInitializeClick();
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblSystemException").text());
                //showMessageDialog(message, focusFieldID, ERROR);
            });

       
    }
    
}
function DeleteProcessPlanHandoverData() {
    jsonPlanData = [];
    processPlanDetails = [];
    messageWithChubanGyouban = "";
    var repeatedMsg1 = "";
    chubanList = [];
    var chubanGyouban1 = "";
    var ajaxTime = new Date().getTime();
    for (var i = 1; i <= 50; i++) {
            var jsonPlanData = ({
                CHUUBAN_GYOUBAN: $("#txtchuuban" + i).val(),
                PROCESS_PLAN_CD: $("#txtProcessPlanCd" + i).text(),
                NEXT_PROCESS_CD: $("#ddlNextProcessCD" + i).text()
            });
            if ($("#txtProcessPlanCd" + i).text() != "") {
                processPlanDetails.push(jsonPlanData);
            }
        
    }
        debugger;
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/ProcessPlanHandover/DeleteProcessPlanHandoverData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(processPlanDetails)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonresult = response.data;
                
                for (let p = 0; p < jsonresult.length; p++) {
                    var chubanGyouban = jsonresult[p].chuubaN_GYOUBAN;
                    var isProcessStart = false;
                    if (jsonresult.length == 1) {
                        if (jsonresult[p].responce == "Process Started") {
                            isProcessStart == true;
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblFollowingOrNoProcessStarted").text());
                            messageWithChubanGyouban = message.replace("9999999-99", chubanGyouban);
                            showMessageDialog(messageWithChubanGyouban, "", WARNING);
                        }
                        else {
                            focusFieldID = $(':focus').attr("id");
                            messageWithChubanGyouban = $.trim($("#lblProcessCancelled").text());
                            showMessageDialog(messageWithChubanGyouban, "", SUCCESS);
                            btnInitializeClick();
                        }
                    }
                    else{
                        if (jsonresult[p].responce == "Process Started") {
                            if (repeatedMsg1 != "") {

                               
                                chubanGyouban1 = chubanGyouban1 + ",<br>" + jsonresult[p].chuubaN_GYOUBAN;
                                

                            }
                            else {
                                repeatedMsg1 = "Found";
                                chubanGyouban1 = jsonresult[p].chuubaN_GYOUBAN;

                            }
                        }
                        else {
                            focusFieldID = $(':focus').attr("id");
                            messageWithChubanGyouban = $.trim($("#lblProcessCancelled").text());
                            chubanList.push(jsonresult[p].chuubaN_GYOUBAN);
                        }
                    }
                    
                }
                if (jsonresult.length != 1) {
                   
                    if (chubanGyouban1 != "") {
                        messageWithChubanGyouban = $.trim($("#lblFollowingOrNoProcessStarted").text()).replace("9999999-99", "<br>" + chubanGyouban1);
                        showMessageDialog(messageWithChubanGyouban, "", WARNING);
                    } else {
                        focusFieldID = $(':focus').attr("id");
                        messageWithChubanGyouban = $.trim($("#lblProcessCancelled").text());
                        showMessageDialog(messageWithChubanGyouban, "", SUCCESS);
                        btnInitializeClick();
                    }
                }
                if (chubanList.length >0) {
                    alert(chubanList);
                    for (let i = 0; i < chubanList.length; i++) {
                        for (let p = 1; p < 51; p++) {
                            var chubanNo = document.getElementById("txtchuuban" + parseInt(p)).value;
                            if (chubanList[i] == chubanNo) {
                                $('#txtchuuban' + parseInt(p)).val('');
                                $("#txtCustomer" + parseInt(p)).text('');
                                $("#txtProdName" + parseInt(p)).text('');
                                $("#txtShape" + parseInt(p)).text('');
                                $("#txtDia" + parseInt(p)).text('');
                                $("#txtDiaW" + parseInt(p)).text('');
                                $("#txtOther" + parseInt(p)).text('');
                                $("#txtLen" + parseInt(p)).text('');
                                $("#txtCondition" + parseInt(p)).text('');
                                $("#txtCharge" + parseInt(p)).text('');
                                $("#txtActAllPiec" + parseInt(p)).text('');
                                $("#txtActAllWt" + parseInt(p)).text('');
                                $("#txtActAllDt" + parseInt(p)).text('');
                                $("#txtProdSpecNo" + parseInt(p)).text('');
                                $("#txtProcessPlanCd" + parseInt(p)).text('');
                                $("#txtLocationCd" + parseInt(p)).text('');
                                $("#txtActAllPieces" + parseInt(p)).text('');
                                $("#txtActAllPiecesUnit" + parseInt(p)).text('');
                                $("#ddlNextProcess" + parseInt(p)).text('');
                            }
                        }
                        
                    }
                }
                  
               
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblSystemException").text());
                //showMessageDialog(message, focusFieldID, ERROR);
            });
    
}
$("#btnInitialize").click(function () {
    btnInitializeClick();
});
function btnInitializeClick() {
    $("#btnInput").removeAttr("DISABLED").removeClass("DISABLED");
    $("#btnCancel").attr("DISABLED", "DISABLED");
    $('#rdoPrcessPlanSearchBeforeEntry').prop("checked", "checked");
    setNextProcessList();

    for (let i = 1; i < 51; i++) {
        $('#txtchuuban' + parseInt(i)).val('');
        $("#txtCustomer" + parseInt(i)).text('');
        $("#txtProdName" + parseInt(i)).text('');
        $("#txtShape" + parseInt(i)).text('');
        $("#txtDia" + parseInt(i)).text('');
        $("#txtDiaW" + parseInt(i)).text('');
        $("#txtOther" + parseInt(i)).text('');
        $("#txtLen" + parseInt(i)).text('');
        $("#txtCondition" + parseInt(i)).text('');
        $("#txtCharge" + parseInt(i)).text('');
        $("#txtActAllPiec" + parseInt(i)).text('');
        $("#txtActAllWt" + parseInt(i)).text('');
        $("#txtActAllDt" + parseInt(i)).text('');
        $("#txtProdSpecNo" + parseInt(i)).text('');
        $("#txtProcessPlanCd" + parseInt(i)).text('');
        $("#txtLocationCd" + parseInt(i)).text('');
        $("#txtActAllPieces" + parseInt(i)).text('');
        $("#txtActAllPiecesUnit" + parseInt(i)).text('');
        $("#ddlNextProcess" + parseInt(i)).text('');
    }
}
function myFunction(ev,index) {
    if (ev.type == "input") {
        var value = document.getElementById("txtchuuban" + index).value;
        if (value.length == 11) {
            setFocus(index);
        }
    }
}
function setFocus(index) {
    document.getElementById("txtchuuban" + (index + 1)).focus();
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
