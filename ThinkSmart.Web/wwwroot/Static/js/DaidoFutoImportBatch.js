let pageNumber = null;
let KoubanName = null;
var DISABLED = "disabled";
var typeDropDown = "select-one";
var typeText = "text";
let file;

$(document).ready(function () { 
    debugger;
    setFutou();
    Mask();
    setStartSysDateTime();
});

$("#btnChooseTextFile").click(function () {
    
    $("#hiddenTextFile").click();  
});

$("#hiddenTextFile").on("change", function () {
    debugger;
    const file = this.files[0];

    if (file) {
        $("#selectTextFile").val(file.name);   
    } else {
        $("#selectTextFile").val("");
    }
});


$("#btnImport").click(function () {
    debugger;
   

      // Check file selected
    if ($("#hiddenTextFile").val() === "") {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgSelectEDIFile").text());       
        showMessageDialog(message, focusFieldID, WARNING);       
        return false;
    }

    // Check dropdown selected
    let dropVal = $("#ddlFutouLocDropdown").val();
    if (dropVal === "" || dropVal === "0" || dropVal === null) {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgSelectWarehouseValue").text());       
        showMessageDialog(message, focusFieldID, WARNING);
       
        return false;
    }
 
    var selectedTextFile = $("#hiddenTextFile")[0].files[0];

    if (!selectedTextFile || selectedTextFile.size === 0) {
        focusFieldID = $(':focus').attr("id");
        message = $.trim($("#valMsgDataNotFoundInEDIFile").text());       
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }
    
    
    DaidoFutoImport();
});


function DaidoFutoImport() {
    debugger;
    EDIBatchImport();
    console.log("uploaded file");
    
}




function EDIBatchImport() {
    debugger;

    var fileInput = document.getElementById("hiddenTextFile");

    if (fileInput.files.length === 0) {
        return;
    }

    var formData = new FormData();
    formData.append("file", fileInput.files[0]);
    formData.append("futouCd", $("#ddlFutouLocDropdown").val());

    var ajaxTime = new Date().getTime();
    var settings = {

        url: "/DaidoFutoBatch/ReadTextFile",
        method: "POST",
        processData: false,
        contentType: false,
        data: formData

    }
   
    $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                debugger;
                if (response.isSuccess) {
                    var cultureName = $("#hdfCultureName").val();                   
                    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;
                    var settings1 = {
                        async: false,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: APIURL + "/DaidoFutoBatch/DaidoNyukoEDIBatchDataInsert?cultureName=" + cultureName + "&userID=" + userID,
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(response.data)
                    }
                    $.ajax(settings1)
                        .done(function (response) {
                            responseTime = new Date().getTime() - ajaxTime;                            
                            debugger;
                          
                            //if (response.message == "Data Insert Successfully") {
                            //    focusFieldID = $(':focus').attr("id");
                            //    message = $.trim($("#successMsgEDIfileImported").text());
                            //    showMessageDialog(message, focusFieldID, SUCCESS);


                            //    var allDataReport = window.open(REPORTURL + "/Report/DaidoFutouNyukoBatch.aspx?userID=" + userID + '&printDate=' + response.printDate, "width=1940,height=1140", "Resizable = no");
                            //    var errorReport = window.open(REPORTURL + "/Report/DaidoFutouNyukoBatchError.aspx?userID=" + userID + '&printDate=' + response.printDate, "width=1940,height=1140", "Resizable = no");

                            //    return false;
                            //}

                            if (response.message == "Data Insert Successfully") {
                                focusFieldID = $(':focus').attr("id");
                                message = $.trim($("#successMsgEDIfileImported").text());
                                showMessageDialog(message, focusFieldID, SUCCESS);
                                var printFlag = 1;

                                var reportUrl1 = REPORTURL + "/Report/DaidoFutouNyukoBatch.aspx" + "?userID=" + userID + "&printDate=" + response.data.procesS_DATE_AND_TIME + "&printFlag=" + printFlag;
                                window.open(reportUrl1, "DaidoFutouNyukoBatch", "width=1940,height=1140,resizable=no");
                                
                                if (response.data.executE_RESULT > 0)
                                {
                                    var reportUrl2 = REPORTURL + "/Report/DaidoFutouNyukoBatchError.aspx" + "?userID=" + userID + "&printDate=" + response.data.procesS_DATE_AND_TIME + "&printFlag=" + printFlag;
                                    window.open(reportUrl2, "DaidoFutouNyukoBatchError", "width=1940,height=1140,resizable=no");

                                }
                               
                                return false;
                            }

                        })
                        .fail(function (xhr, textStatus, errorThrown) {
                            responseTime = new Date().getTime() - ajaxTime;
                            var errorCode = $.parseJSON(xhr.responseText).Message;
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#valMsgCOMSystemException").text());
                            showMessageDialog(message, focusFieldID, ERROR);
                        });
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgCOMSystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });


    
}


function BindFutouDropdown(jsonResult) {
   
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlFutouLocDropdown").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function setFutou() {
    debugger;
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/DaidoFutoBatch/GetFutouList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindFutouDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#valMsgCOMSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function Mask() {
    $("#txtDate").mask("9999/99/99", { placeholder: "yyyy/MM/dd" });
}


function setStartSysDateTime() {
    var ajaxTime = new Date().getTime();
   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/DaidoFutoBatch/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           
            var sysDateTime = response.message.sysDate;
           
            $("#txtDate").val($.format.date(new Date(sysDateTime), "yyyy/MM/dd")).mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
           

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#valMsgCOMSystemException").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function IsMandatoryDateSearchFieldsFilled() {
    debugger;
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var focusFieldID = "";
   
    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        showMessageDialog(message, focusFieldID, WARNING);
    }
    return returnVal;
}
function validateDateFormat(dateVal) {
    if (dateVal == null || dateVal.trim() === "")
        return false;

    // Format: yyyy/MM/dd or yyyy-MM-dd
    var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;

    var dateValues = dateVal.match(validatePattern);

    if (dateValues == null)
        return false;

    var dtYear = parseInt(dateValues[1], 10);
    var dtMonth = parseInt(dateValues[3], 10);
    var dtDay = parseInt(dateValues[5], 10);

    // Month validation
    if (dtMonth < 1 || dtMonth > 12)
        return false;

    // Day validation
    if (dtDay < 1 || dtDay > 31)
        return false;

    // 30-day months
    if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay > 30)
        return false;

    // February validation
    if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));

        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }

    return true;
}

$("#btnPrintReport").click(function () {
    var printFlag = 0;
    var userID = $("#hdfLoginId").val() ? $("#hdfLoginId").val() : null;


    if (!IsMandatoryDateSearchFieldsFilled()) {
        return false;
    }

    var date = document.getElementById("txtDate").value;
    if (!validateDateFormat(date)) {
        var message = $("#valMsgEnterValidImportDate").text().trim();
        showMessageDialog(message, "txtDate", WARNING);
        $("#txtDate").focus;
        return false;        
    }
    else {
        debugger;
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",         
            url: APIURL + "/DaidoFutoBatch/GetDataCount?Importdate=" + date,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var result = response.message;
                
                if (parseInt(result.totaL_COUNT) > 0) {
                    var reportUrl1 = REPORTURL + "/Report/DaidoFutouNyukoBatch.aspx" + "?userID=" + userID + "&printDate=" + date + "&printFlag=" + printFlag;
                    window.open(reportUrl1, "DaidoFutouNyukoBatch", "width=1940,height=1140,resizable=no");
                }
                if (parseInt(result.erroR_COUNT) > 0) {
                    var reportUrl2 = REPORTURL + "/Report/DaidoFutouNyukoBatchError.aspx" + "?userID=" + userID + "&printDate=" + date + "&printFlag=" + printFlag;
                    window.open(reportUrl2, "DaidoFutouNyukoBatchError", "width=1940,height=1140,resizable=no");
                }

                if (parseInt(result.totaL_COUNT) == 0) {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#valMsgCOMNoRecordFoundForPrint").text());
                    showMessageDialog(message, focusFieldID, WARNING);
                }
               
                    //var ajaxTime = new Date().getTime();
                    //var settings = {
                    //    async: true,
                    //    crossDomain: true,
                    //    cache: false,
                    //    dataType: "json",
                    //    url: APIURL + "/DaidoFutoBatch/GetErrorDataCount?Importdate=" + date,
                    //    method: "POST",
                    //    contentType: "application/json; charset=utf-8",

                    //}
                    //$.ajax(settings)
                    //    .done(function (response) {
                    //        responseTime = new Date().getTime() - ajaxTime;
                    //        var result = response.message;
                    //        debugger;
                    //        if (parseInt(result) > 0) {
                    //            var reportUrl2 = REPORTURL + "/Report/DaidoFutouNyukoBatchError.aspx" + "?userID=" + userID + "&printDate=" + date + "&printFlag=" + printFlag;
                    //            window.open(reportUrl2, "DaidoFutouNyukoBatchError", "width=1940,height=1140,resizable=no");
                    //        }
                    //        //else {
                    //        //    focusFieldID = $(':focus').attr("id");
                    //        //    message = $.trim($("#valMsgCOMNoRecordFoundForPrint").text());
                    //        //    showMessageDialog(message, focusFieldID, WARNING);
                    //        //}
                            
                    //    })
                    //    .fail(function (xhr, textStatus, errorThrown) {
                    //        responseTime = new Date().getTime() - ajaxTime;
                    //        var errorCode = $.parseJSON(xhr.responseText).Message;

                    //        focusFieldID = $(':focus').attr("id");
                    //        message = $.trim($("#valMsgCOMSystemException").text());
                    //        showMessageDialog(message, focusFieldID, ERROR);
                    //    });




            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgCOMSystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
       
        
      
    }
});



















