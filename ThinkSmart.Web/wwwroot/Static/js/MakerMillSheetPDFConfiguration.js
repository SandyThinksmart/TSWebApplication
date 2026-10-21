var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";
var isDefaultLayoutSaved = false;
var isCreateMode = false;
var pdfDoc = null;
var isDrawing = false;
var pdfjsLib = window['pdfjs-dist/build/pdf'];
pdfjsLib.GlobalWorkerOptions.workerSrc = '/Scripts/PDFReader/pdf.worker.min.js'; // 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.worker.min.js';
var pdfCanvas = document.getElementById("pdfCanvas");
var drawCanvas = document.getElementById("drawCanvas");
var pdfCtx = pdfCanvas.getContext("2d");
var drawCtx = drawCanvas.getContext("2d");
var startX = "", startY = "", endX = "", endY = "";
var checkboxID = "";
var fieldID = "";
var pdfHeight = "";
var pdfWidth = "";
var savedpdfPath = "";
var templateNameCheckFlag = false;
var jsonArray = [];

$(document).ready(function () {
    window.resizeTo(1750, 1020);
    jsonArray = [];
    isDefaultLayoutSaved = false;
    GetMakerList();
    GetPageFieldsList();
    var templateName = GetURLParameters(window.location.href);
    if (templateName != "") {
        GetPDFConfigurationDetailsByTemplateName(templateName);
    }
    
});
function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) {
        return result;
    } else {
        var sPageURL = url.substring(searchIndex + 1);
        var sParameterName = url.split('templateName=');
        result = sParameterName[1];
        return result;
    }
}
function EnableEditDeleteButton() {
    $("#btnEdit").removeClass(DISABLED).removeAttr(DISABLED);
    $("#btnDelete").removeClass(DISABLED).removeAttr(DISABLED);
}
function DisableEditDeleteButton() {
    $("#btnEdit").attr(DISABLED, DISABLED);
    $("#btnDelete").attr(DISABLED, DISABLED);
}
function DisableCheckbox(checkbox)
{
    $("#"+checkbox).attr(DISABLED, DISABLED).addClass(DISABLED);
}
function GetMakerList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GeMatlMakerList",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMakerDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MillSheet_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function BindMakerDropdown(jsonResult) {

    $("#ddlMakerName").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMakerName").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
        });
   
}
function GetPageFieldsList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/MakerMillSheet/GetMillsheetPageLables",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            $("#divMillSheetLabelGrid").html(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_Maker_MillSheet_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}

function DefaultLayoutExist(templateName)
{
    var returnVal = false;

    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetDefaultLayoutData?templateName=" + templateName,
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if (parseInt(jsonResult) > 0)
            {
                returnVal = true;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnVal;
}
$("#btnSave").click(function () {
    var errorMsg = "";
    var focusFieldID = "";
   
    if ($("#ddlMakerName").val() == 0)
    {
        errorMsg = $.trim($("#lblErrorMessageE_Maker_MillSheet_002").text());
        focusFieldID = "ddlMakerName";
    }
   
    
    const file = $("#pdfUpload")[0].files[0];
    if (!file) {
        if (errorMsg == "") {
            errorMsg = "Select File";
        } else {
            errorMsg = errorMsg + "<br>" + "Select File";
        }
        if (focusFieldID == "") {
            focusFieldID = "pdfUpload";
        }

    } 
    if ($("#txtTemplateName").val() == "")
    {
        if (errorMsg == "") {
            errorMsg = "Unique template name is required.";
        } else {
            errorMsg = errorMsg + "<br>" + "Unique template name is required.";
        }
        if (focusFieldID == "") {
            focusFieldID = "txtTemplateName";
        }
    }
    if (jsonArray.length==0) {
        if (errorMsg == "") {
            errorMsg = "please select at least one element.";
        } else {
            errorMsg = errorMsg + "<br>" + "please select at least one element.";
        }
        if (focusFieldID == "") {
            focusFieldID = "chkSelectField1";
        }
    }
   
    if (errorMsg != "")
    {
        showMessageDialog(errorMsg, focusFieldID, WARNING);
        return false;
    }

   
    if (templateNameCheckFlag == false)
    {
        if (IsTemplateNameAlreadyExist()) {
            showMessageDialog("Unique template name already exist in record", focusFieldID, WARNING);
            return false;

        }
    }
    
    var invalidRect = jsonArray.filter(r => r.TOP_LEFT_X == 0 && r.TOP_LEFT_Y == 0 && r.TOP_RIGHT_X == 0 && r.TOP_RIGHT_Y == 0)

    if (invalidRect.length > 0) {
        var fieldName = "";
        $("#grdvMillSheetFields tbody tr").filter(function () {

            if ($(this).find("label[id^='lblLabelFieldCD']").text().trim() == invalidRect[0].FIELD_CD) {
                fieldName = $(this).closest("tr").find("label[id^='lblLabelName']").attr("id");
               
            }

        });
        showMessageDialog("Please create box for " + $("#" + fieldName).text(), fieldName, WARNING);
        return false;
    }


        var boxDetails = GetBoxLayoutDetails();
    
        if (boxDetails.toP_LEFT_X > 0 && boxDetails.toP_RIGHT_X > 0 && boxDetails.bottoM_LEFT_X > 0 && boxDetails.bottoM_RIGHT_X > 0) {
           
            jsonArray.push({
                MAKER_CD: $("#ddlMakerName").val(),
                UNIQUE_TEMPLATE_NAME: $("#txtTemplateName").val(),
                FIELD_CD: "999",
                TOP_LEFT_X: boxDetails.toP_LEFT_X,
                TOP_LEFT_Y: boxDetails.toP_LEFT_Y,
                TOP_RIGHT_X: boxDetails.toP_RIGHT_X,
                TOP_RIGHT_Y: boxDetails.toP_RIGHT_Y,
                BOTTOM_LEFT_X: boxDetails.bottoM_LEFT_X,
                BOTTOM_LEFT_Y: boxDetails.bottoM_LEFT_Y,
                BOTTOM_RIGHT_X: boxDetails.bottoM_RIGHT_X,
                BOTTOM_RIGHT_Y: boxDetails.bottoM_RIGHT_Y,
                USER_ID: $("#hdfLoginId").val()
            });
        } else
        {
            showMessageDialog("Table format not found for this pdf, hence please change pdf layout", "ddlMakerName", WARNING);
            return false;
        }

    
    jsonArray.forEach(function (r) {
        if (r.FIELD_CD != "999") {
            r.MAKER_CD = $("#ddlMakerName").val();
            r.UNIQUE_TEMPLATE_NAME = $("#txtTemplateName").val();
            r.TOP_LEFT_X = (r.TOP_LEFT_X / 0.27).toFixed(2);
            r.TOP_LEFT_Y = (r.TOP_LEFT_Y / 0.27).toFixed(2);
            r.TOP_RIGHT_X = (r.TOP_RIGHT_X / 0.27).toFixed(2);
            r.TOP_RIGHT_Y = (r.TOP_RIGHT_Y / 0.27).toFixed(2);
            r.BOTTOM_LEFT_X = (r.BOTTOM_LEFT_X / 0.27).toFixed(2);
            r.BOTTOM_LEFT_Y = (r.BOTTOM_LEFT_Y / 0.27).toFixed(2);
            r.BOTTOM_RIGHT_X = (r.BOTTOM_RIGHT_X / 0.27).toFixed(2);
            r.BOTTOM_RIGHT_Y = (r.BOTTOM_RIGHT_Y / 0.27).toFixed(2);
            r.USER_ID = $("#hdfLoginId").val();
        }
    });
  
  SaveConfigurationDetails(jsonArray);
  
});
function SaveConfigurationDetails(jsonData)
{
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/SavePDFSettings",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            $("#btnSave").attr(DISABLED, DISABLED);
            jsonArray = [];
            templateNameCheckFlag = true;
            var templateName = GetURLParameters(window.location.href);
            if (templateName != "") {
                GetPDFConfigurationDetailsByTemplateName(templateName);
            }
          //  DisableCheckbox(checkboxID);
            var successMessage = $("#lblSuccessMessageS_Maker_MillSheet_001").text();
            showMessageDialog(successMessage, "ddlFieldName", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function GetBoxLayoutDetails()
{
    var returnData = null;
  
    const file = $("#pdfUpload")[0].files[0];

    var formData = new FormData();
    formData.append('file', file);
    formData.append('makerCD', $("#ddlMakerName").val()); 
    formData.append('templateName', $("#txtTemplateName").val()); 
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        processData: false,       // important for FormData
        dataType: "json",
        url: "/MakerMillSheet/GetPDFBoxLayout",
        method: "POST",
        contentType: false,
        data: formData
    }
    $.ajax(settings)
        .done(function (response) {
           returnData = response.message;
            

        })
        .fail(function (xhr, textStatus, errorThrown) {
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return returnData;

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
$(document).on("change", ".checkbox-Grid", function () {

    checkboxID = $(this).attr("id"); //$(".checkbox-Grid:checked").attr("id");
    var labelID = $("#" + checkboxID).closest("tr").find("label[id^='lblLabelFieldCD']").attr("id");
   
    var invalidRect = false;

    debugger;
    if ($(this).is(":checked")) {
        debugger;
        var invalidSaveData = [];
        invalidSaveData = jsonArray.filter(r => r.TOP_LEFT_X == 0 && r.TOP_LEFT_Y == 0 && r.TOP_RIGHT_X == 0 && r.TOP_RIGHT_Y == 0)

        if (invalidSaveData.length > 0)
        {
            $("#grdvMillSheetFields tbody tr").filter(function ()
            {

                if ($(this).find("label[id^='lblLabelFieldCD']").text().trim() == invalidSaveData[0].FIELD_CD)
                {
                    var fieldName = $(this).closest("tr").find("label[id^='lblLabelName']").attr("id");
                    invalidRect = true;
                    showMessageDialog("Please create box for " + $("#" + fieldName).text(), fieldName, WARNING);
                    
                }


            });
        }
        if (invalidRect == true)
        {
            $(this).prop("checked", false);


        } else
        {
            $(this).closest("tr").addClass("row-highlight");
            fieldID = $("#" + labelID).text();
            var isDataExist = jsonArray.filter(r => r.FIELD_CD == fieldID && r.TOP_LEFT_X != 0 && r.TOP_RIGHT_X != 0);
           
            if (isDataExist.length == 1)
            {
                jsonArray.filter(r => r.FIELD_CD == fieldID).map(r => r.ACTION = "U");
                drawCtx.beginPath();
                drawCtx.strokeStyle = "darkgreen";
                drawCtx.lineWidth = 2;
                drawCtx.rect(isDataExist[0].TOP_LEFT_X, isDataExist[0].TOP_LEFT_Y, isDataExist[0].WIDTH, isDataExist[0].HEIGHT);
                drawCtx.stroke();
            } else
            {
                jsonArray.push({
                    ACTION:"A",
                    MAKER_CD: 0,
                    UNIQUE_TEMPLATE_NAME: "",
                    FIELD_CD: fieldID,
                    TOP_LEFT_X: 0,
                    TOP_LEFT_Y: 0,
                    TOP_RIGHT_X: 0,
                    TOP_RIGHT_Y: 0,
                    BOTTOM_LEFT_X: 0,
                    BOTTOM_LEFT_Y: 0,
                    BOTTOM_RIGHT_X: 0,
                    BOTTOM_RIGHT_Y: 0,
                    HEIGHT: 0,
                    WIDTH: 0,
                    USER_ID: 0
                });
                var hiddenID = $(this).closest("tr").find("input[type='hidden'][id^='hdfRectValue']").attr("id");
                var radioID = $(this).closest("tr").find("input[type='radio'][id^='rdoEditDelete']").attr("id");
                var templateName = GetURLParameters(window.location.href);
                //if (($("#" + hiddenID).val() == "" && templateName == "") || ($("#" + hiddenID).val() == "" && $("#" + radioID).is(":disabled")))
                //{
                //    CreateCrosshair();
                //}
            }
           
                CreateCrosshair();
           
        }
    } else
    {
        fieldID = $("#" + labelID).text();
        $(this).closest("tr").removeClass("row-highlight");

        var templateName = GetURLParameters(window.location.href);
        if (templateName != "") {
            debugger;
           
            var getResult = jsonArray.filter(r => r.FIELD_CD == fieldID); // remove specific field row

            if (getResult[0].ACTION == "A")
            {
                jsonArray = jsonArray.filter(r => r.FIELD_CD != fieldID);
            }
            drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);
            jsonArray.forEach(function (r) {
                if (r.FIELD_CD == fieldID || r.ACTION == "D") {
                    r.ACTION = "D";

                    drawCtx.beginPath();
                    drawCtx.strokeStyle = "red";
                    drawCtx.lineWidth = 2;
                    drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
                    drawCtx.stroke();

                } else if (r.FIELD_CD != 999)
                {
                    drawCtx.beginPath();
                    drawCtx.lineWidth = 2;
                    drawCtx.strokeStyle = "darkgreen";
                    drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
                    drawCtx.stroke();
                }
            });

           

        } else
        {

            if (jsonArray.length > 0)
            {
                jsonArray = jsonArray.filter(r => r.FIELD_CD != fieldID); // remove specific field row

                drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);

                jsonArray.forEach(function (r) {
                    if (r.FIELD_CD != 999)
                    {
                        drawCtx.beginPath();
                        drawCtx.lineWidth = 2;
                        drawCtx.strokeStyle = "darkgreen";
                        drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
                        drawCtx.stroke();
                    }
                });
            }
        }
    }
  
   
  
    
});


function LoadPDF(url) {
    pdfjsLib.getDocument(url).promise.then(function (pdf) {
        pdfDoc = pdf;
       RenderPage(1);
    });
}

// Render page 1 (you can expand to handle multi-page later)
function RenderPage(pageNum) {

     pdfDoc.getPage(pageNum).then(function (page) {
        var scale = 1.5;
        var viewport = page.getViewport({ scale });
        pdfWidth = viewport.width;
        pdfHeight = viewport.height;
        pdfCanvas.width = viewport.width;
        pdfCanvas.height = viewport.height;

        drawCanvas.width = viewport.width;
        drawCanvas.height = viewport.height;

        const renderTask= page.render({
            canvasContext: pdfCtx,
            viewport: viewport
        });
        renderTask.promise;
    });
   
}

$('#pdfUpload').on('change', async (e) => {

    const file = e.target.files[0];
    if (file) {
        
        let fileURL = URL.createObjectURL(file);
        LoadPDF(fileURL);
    }
});
$("#drawCanvas").on("mousedown", (e) => {
    if (isCreateMode == true) {
        isDrawing = true;
        startX = e.offsetX;
        startY = e.offsetY;
    }
});

$("#drawCanvas").on("mousemove", (e) => {

    if (isDrawing == true) {

        const rect = drawCanvas.getBoundingClientRect();
        let currentX = e.clientX - rect.left;
        let currentY = e.clientY - rect.top;

        // 1️ Clear canvas
        drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);

        // 2️ Redraw stored rectangles
        jsonArray.forEach(function (r) {
            if (r.FIELD_CD != "999") {
                drawCtx.beginPath();
                drawCtx.lineWidth = 2;
                drawCtx.strokeStyle = "darkgreen";
                drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
                drawCtx.stroke();
            }
        });

        // 3️ Draw current preview rectangle
        drawCtx.beginPath();
        drawCtx.lineWidth = 2;
        drawCtx.strokeStyle = "darkgreen";
        drawCtx.rect(startX, startY, currentX - startX, currentY - startY);
        drawCtx.stroke();
    }
});

$("#drawCanvas").on("mouseup", (e) => {
   
    if (isDrawing == true && isCreateMode == true) {
        
        endX = e.offsetX;
        endY = e.offsetY;
        drawCtx.beginPath();
        drawCtx.strokeStyle = 'darkgreen';
        drawCtx.lineWidth = 2;
        drawCtx.rect(startX, startY, endX - startX, endY - startY);
        drawCtx.stroke();

        var templateName = GetURLParameters(window.location.href);
        var result = GetRowElementIds(fieldID);
        jsonArray.forEach(function (r) {
            if (r.FIELD_CD == fieldID) {
                r.TOP_LEFT_X = startX;
                r.TOP_LEFT_Y = startY;
                r.TOP_RIGHT_X = endX;
                r.TOP_RIGHT_Y = startY;
                r.BOTTOM_LEFT_X = startX;
                r.BOTTOM_LEFT_Y = endY;
                r.BOTTOM_RIGHT_X = endX;
                r.BOTTOM_RIGHT_Y = endY;
                r.HEIGHT= endY - startY;
                r.WIDTH = endX - startX;
                if (templateName != "" && $("#" + result.checkboxId).prop("checked") == true && $("#" + result.radioId).is(":disabled"))
                {
                    r.ACTION = "A";

                }else if (templateName != "")
                {
                    r.ACTION = "U";
                }
            }
        });
        if (templateName != "") {
            DrawAllRectangles(jsonArray);
        }
        
        //startX = (startX / 0.27).toFixed(2);
        //startY = (startY / 0.27).toFixed(2);
        //endX = (endX / 0.27).toFixed(2);
        //endY = (endY / 0.27).toFixed(2);
        $("#txtOutput").text(startX + "   " + startY + " " + endX + "  " + startY + "  " + endX + "  " + endY + "  " + startX + "  " + endY);
        RemoveCrosshair();
        isDrawing = false;
        isCreateMode = false;
       
    }


});
function DrawAllRectangles(data) {

    drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);
    
    data.forEach(r => {
        if (r.FIELD_CD != "999")
        {
            drawCtx.beginPath();
            drawCtx.strokeStyle = 'darkgreen';
            drawCtx.lineWidth = 2;
            drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
            drawCtx.stroke();
        }
    });

}
function CreateCrosshair()
{
    $("#btnCreateBox").removeClass(DISABLED).removeAttr(DISABLED);
    $("#btnRemoveBox").removeClass(DISABLED).removeAttr(DISABLED);
    isCreateMode = true;
    drawCanvas.style.cursor = "crosshair";
    drawCanvas.classList.add("cursor");
}
function RemoveCrosshair() {
    isCreateMode = false;
    isDrawing = false;
    drawCanvas.style.cursor = "auto";
    drawCanvas.classList.remove("cursor");
}
$('#btnCreateBox').on('click', () => {
    const file = $("#pdfUpload")[0].files[0];
    if (file) {
        CreateCrosshair();
        isCreateMode = true;
    } else if (savedpdfPath != "") {
        CreateCrosshair();
        isCreateMode = true;
    }
});
$('#btnRemoveBox').on('click', () => {
  
    //if (jsonArray.length > 0) {
    //    jsonArray.pop(); // remove last rectangle
    //}
   
    jsonArray.forEach(function (r) {
        
        if (r.FIELD_CD == fieldID)
        {
            r.TOP_LEFT_X = 0;
            r.TOP_LEFT_Y = 0;
            r.TOP_RIGHT_X = 0;
            r.TOP_RIGHT_Y = 0;
            r.BOTTOM_LEFT_X = 0;
            r.BOTTOM_LEFT_Y = 0;
            r.BOTTOM_RIGHT_X = 0;
            r.BOTTOM_RIGHT_Y = 0;
            r.HEIGHT = 0;
            r.WIDTH = 0;
        }
    });
    drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);

    var templateName = GetURLParameters(window.location.href);
    if (templateName != "") {

        jsonArray.forEach(function (r) {
            if (r.FIELD_CD != "999")
            {
                let x = r.TOP_LEFT_X;
                let y = r.TOP_LEFT_Y;
                let width =r.WIDTH;
                let height = r.HEIGHT;

                drawCtx.beginPath();
                drawCtx.strokeStyle = "darkgreen";
                drawCtx.lineWidth = 2;
                drawCtx.rect(x, y, width, height);
                drawCtx.stroke();
            }
        });
    } else
    {
        jsonArray.forEach(function (r) {

            drawCtx.beginPath();
            drawCtx.lineWidth = 2;
            drawCtx.strokeStyle = "darkgreen";
            drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
            drawCtx.stroke();

        });
    }
    
});
$("#btnSelect").click(function () {
   
    location.href = "/MakerMillSheet/MakerMillSheetConfigListPage";
    window.resizeTo(590, 780);
});
function GetPDFConfigurationDetailsByTemplateName(templateName)
{
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetPDFDetailsByTemplateName?templateName=" + templateName,
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            $("#btnSave").attr(DISABLED, DISABLED);
           
            SetGridData(jsonResult);
            EnableEditDeleteButton();
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function SetGridData(jsonResult)
{
    jsonArray = [];
    $("#grdvMillSheetFields tbody tr").filter(function () {

        var rdoID = $(this).closest("tr").find("input[type='radio'][id^='rdoEditDelete']").attr("id");
        $("#" + rdoID).closest("tr").removeClass("row-highlight");
        $("#" + rdoID).attr(DISABLED, DISABLED);
        $("#" + rdoID).prop("checked", false);
    });

    jsonResult.forEach(function (r) {
        
        let t_left_x = parseFloat(r.toP_LEFT_X * 27) / 100;
        let t_left_y = parseFloat(r.toP_LEFT_Y * 27) / 100;
        let t_right_x = parseFloat(r.toP_RIGHT_X * 27) / 100;
        let t_right_y = parseFloat(r.toP_RIGHT_Y * 27) / 100;

        let b_left_x = parseFloat(r.bottoM_LEFT_X * 27) / 100;
        let b_left_y = parseFloat(r.bottoM_LEFT_Y * 27) / 100;
        let b_right_x = parseFloat(r.bottoM_RIGHT_X * 27) / 100;
        let b_right_y = parseFloat(r.bottoM_RIGHT_Y * 27) / 100;

        let width = parseFloat((r.toP_RIGHT_X - r.toP_LEFT_X) * 27) / 100;
        let height = parseFloat((r.bottoM_RIGHT_Y - r.toP_LEFT_Y) * 27) / 100;

        jsonArray.push({
            FIELD_CD: r.fielD_CD,
            MAKER_CD: r.makeR_CD,
            UNIQUE_TEMPLATE_NAME: r.uniquE_TEMPLATE_NAME,
            TOP_LEFT_X: t_left_x,
            TOP_LEFT_Y: t_left_y,
            TOP_RIGHT_X: t_right_x,
            TOP_RIGHT_Y: t_right_y,
            BOTTOM_LEFT_X: b_left_x,
            BOTTOM_LEFT_Y: b_left_y,
            BOTTOM_RIGHT_X: b_right_x,
            BOTTOM_RIGHT_Y: b_right_y,
            HEIGHT: height,
            WIDTH: width,
            USER_ID: r.useR_ID,
            ACTION: ""
        });
    });
    $.each(jsonResult, function (key, obj) {
        if ($("#ddlMakerName").val() == 0) {
            $("#ddlMakerName").val(obj.makeR_CD);
            $("#ddlMakerName").attr(DISABLED, DISABLED).addClass(DISABLED);
           
        } else if ($("#txtTemplateName").val() == "")
        {
            $("#txtTemplateName").val(obj.uniquE_TEMPLATE_NAME);
            $("#txtTemplateName").attr(DISABLED, DISABLED).addClass(DISABLED);
            $("#pdfUpload").attr(DISABLED, DISABLED);
            savedpdfPath = "/MillsheetTemplate/" + $("#txtTemplateName").val() + ".pdf";

            LoadPDF(savedpdfPath);
          
        }

        if (obj.fielD_CD != 999)
        {
            var rectHeight = obj.bottoM_RIGHT_Y - obj.toP_LEFT_Y;
            var rectWidth = obj.toP_RIGHT_X - obj.toP_LEFT_X;
            var rectValue = (obj.toP_LEFT_X).toFixed(2) + "," + (obj.toP_LEFT_Y).toFixed(2) + "," + (rectWidth).toFixed(2) + "," + (rectHeight).toFixed(2);
         
            var result = GetRowElementIds(obj.fielD_CD);
            $("#" + result.checkboxId).prop("checked", true);
            $("#" + result.checkboxId).closest("tr").addClass("row-highlight"); 
         
            $("#" + result.hiddenFieldId).val(rectValue);//set hidden field rectangle value and enable radio button
            $("#" + result.radioId).removeClass(DISABLED).removeAttr(DISABLED);
            
          
        }
       
    });
    drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);
    setTimeout(function () {
        jsonArray.forEach(function (r) {
            
            if (r.FIELD_CD != "999")
            {
                drawCtx.beginPath();
                drawCtx.strokeStyle = "darkgreen";
                drawCtx.lineWidth = 2;
                drawCtx.rect(r.TOP_LEFT_X, r.TOP_LEFT_Y, r.WIDTH, r.HEIGHT);
                drawCtx.stroke();
            }
        });
       
    }, 800);

}
function GetRowElementIds(labelValue) {
   
    // Find the correct row based on label text
    var row = $("#grdvMillSheetFields tbody tr").filter(function () {
        // return $(this).find("label.Hidden").text().trim() == labelValue;
        return $(this).find("label[id^='lblLabelFieldCD']").text().trim() == labelValue;
    });

    // Get IDs
    return {
        checkboxId: row.find("input[type='checkbox']").attr("id"),
        radioId: row.find("input[type='radio']").attr("id"),
        hiddenFieldId: row.find("input[type='hidden']").attr("id")
    };
}
function DrawFieldRectangle(rdoButton)
{
   
    var invalidRect = jsonArray.filter(r => r.TOP_LEFT_X == 0 && r.TOP_LEFT_Y == 0 && r.TOP_RIGHT_X == 0 && r.TOP_RIGHT_Y == 0)

    if (invalidRect.length > 0) {
        var fieldName = "";
        $("#grdvMillSheetFields tbody tr").filter(function () {

            if ($(this).find("label[id^='lblLabelFieldCD']").text().trim() == invalidRect[0].FIELD_CD) {
                fieldName = $(this).closest("tr").find("label[id^='lblLabelName']").attr("id");
                var radioID = $(this).closest("tr").find("input[type='radio'][id^='rdoEditDelete']").attr("id");
                $(this).prop("checked", false);
                $("#" + radioID).prop("checked", true);
            }

        });
        showMessageDialog("Please create box for " + $("#" + fieldName).text(), fieldName, WARNING);
        return false;
    }
    
    var labelID = rdoButton.closest("tr").find("label[id^='lblLabelFieldCD']").attr("id");
    fieldID = $("#" + labelID).text();

    jsonArray.forEach(function (r) {
        if (r.FIELD_CD != "999")
        {
            if (r.FIELD_CD == fieldID) {
                debugger;
                let x = r.TOP_LEFT_X;
                let y = r.TOP_LEFT_Y;
                let width = r.TOP_RIGHT_X - r.TOP_LEFT_X;
                let height = r.BOTTOM_RIGHT_Y - r.TOP_LEFT_Y;

                drawCtx.beginPath();
                drawCtx.strokeStyle = "MediumTurquoise";
                drawCtx.lineWidth = 2;
                drawCtx.rect(x, y, width, height);
                drawCtx.stroke();
            } else
            {
                let x = r.TOP_LEFT_X;
                let y = r.TOP_LEFT_Y;
                let width = r.TOP_RIGHT_X - r.TOP_LEFT_X;
                let height = r.BOTTOM_RIGHT_Y - r.TOP_LEFT_Y;

                drawCtx.beginPath();
                drawCtx.strokeStyle = "darkgreen";
                drawCtx.lineWidth = 2;
                drawCtx.rect(x, y, width, height);
                drawCtx.stroke();
            }
        }
    });

    //drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);
    //var hiddenID = rdoButton.closest("td").find("input:hidden").attr("id");
    //var splitValues = $("#" + hiddenID).val().split(",");

    //let x = parseFloat(splitValues[0] * 27) / 100;
    //let y = parseFloat(splitValues[1] * 27) / 100;
    //let width = parseFloat(splitValues[2] * 27) / 100;
    //let height = parseFloat(splitValues[3] * 27) / 100;

    //drawCtx.beginPath();
    //drawCtx.strokeStyle = "darkgreen";
    //drawCtx.lineWidth = 2;
    //drawCtx.rect(x, y, width, height);
    //drawCtx.stroke();bt
    CreateCrosshair();
    
}
$("#btnEdit").click(function () {
    
    var invalidRect = jsonArray.filter(r => r.TOP_LEFT_X == 0 && r.TOP_LEFT_Y == 0 && r.TOP_RIGHT_X == 0 && r.TOP_RIGHT_Y == 0 && r.ACTION != "D");

    if (invalidRect.length > 0) {
        var fieldName = "";
        $("#grdvMillSheetFields tbody tr").filter(function () {

            if ($(this).find("label[id^='lblLabelFieldCD']").text().trim() == invalidRect[0].FIELD_CD) {
                fieldName = $(this).closest("tr").find("label[id^='lblLabelName']").attr("id");

            }

        });
        showMessageDialog("Please create box for " + $("#" + fieldName).text(), fieldName, WARNING);
        return false;
    }
    jsonArray.forEach(function (r) {
        if (r.FIELD_CD != "999") {
            r.MAKER_CD = $("#ddlMakerName").val();
            r.UNIQUE_TEMPLATE_NAME = $("#txtTemplateName").val();
            if (r.ACTION == "A" || "U")
            {
                r.TOP_LEFT_X = (r.TOP_LEFT_X / 0.27).toFixed(2);
                r.TOP_LEFT_Y = (r.TOP_LEFT_Y / 0.27).toFixed(2);
                r.TOP_RIGHT_X = (r.TOP_RIGHT_X / 0.27).toFixed(2);
                r.TOP_RIGHT_Y = (r.TOP_RIGHT_Y / 0.27).toFixed(2);
                r.BOTTOM_LEFT_X = (r.BOTTOM_LEFT_X / 0.27).toFixed(2);
                r.BOTTOM_LEFT_Y = (r.BOTTOM_LEFT_Y / 0.27).toFixed(2);
                r.BOTTOM_RIGHT_X = (r.BOTTOM_RIGHT_X / 0.27).toFixed(2);
                r.BOTTOM_RIGHT_Y = (r.BOTTOM_RIGHT_Y / 0.27).toFixed(2);
            }
           
            r.USER_ID = $("#hdfLoginId").val();
        }
    });
   
  UpdateConfigurationDetails(jsonArray);         

});
function UpdateConfigurationDetails(jsonData) {
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/UpdatePDFSettings",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            fieldID = "";
            startX = "";
            startY = "";
            endX = "";
            endY = "";
            
            var templateName = GetURLParameters(window.location.href);
            if (templateName != "") {
                GetPDFConfigurationDetailsByTemplateName(templateName);
            }
            var successMessage = $("#lblSuccessMessageS_Maker_MillSheet_002").text();
            showMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function RemoveUrlParameter(param) {
    const url = new URL(window.location.href);
    url.searchParams.delete(param);
    window.history.replaceState({}, "", url);
}
$("#btnInitialize").click(function () {
    jsonArray = [];
    $("#pdfUpload").removeClass(DISABLED).removeAttr(DISABLED);
    $("#ddlMakerName").removeClass(DISABLED).removeAttr(DISABLED);
    $("#ddlMakerName").val(0);
    $("#txtTemplateName").removeClass(DISABLED).removeAttr(DISABLED);
    $("#txtTemplateName").val("");
    $("#btnSave").removeAttr(DISABLED);
    DisableEditDeleteButton();
    FillEmptyLabelGrid();
    RemoveCrosshair();
    ClearCanvas();
    RemoveUrlParameter("templateName");
    $("#pdfUpload").val("");
    templateNameCheckFlag = false;
    isDefaultLayoutSaved = false;
});
function FillEmptyLabelGrid()
{
    $("#grdvMillSheetFields input[type='checkbox']").prop("checked", false).removeClass(DISABLED).removeAttr(DISABLED);
    $("#grdvMillSheetFields .checkbox-Grid").not(":disabled").each(function () {
        $(this).closest("tr").removeClass("row-highlight");
        var hiddenName = $(this).closest("tr").find("input[type='hidden'][id^='hdfRectValue']").attr("id");
        var rdoName = $(this).closest("tr").find("input[type='radio'][id^='rdoEditDelete']").attr("id");
        
        $("#" + hiddenName).val("");
        $("#" + rdoName).attr(DISABLED, DISABLED);
    });
   
}
function ClearCanvas() {
 
    pdfCtx.clearRect(0, 0, pdfCanvas.width, pdfCanvas.height);
    drawCtx.clearRect(0, 0, drawCanvas.width, drawCanvas.height);

}
function DeleteRowData($delete)
{
    var labelID = $delete.closest("tr").find("label[id^='lblLabelFieldCD']").attr("id");
    var hiddenID = $delete.closest("tr").find("input[type='hidden'][id^='hdfRectValue']").attr("id");
    var rdoButtonID = $delete.closest("tr").find("input[type='radio'][id^='rdoEditDelete']").attr("id");
    var checkboxID = $delete.closest("tr").find("input[type='checkbox']").attr("id");

    var deleteData = {
       
        FIELD_CD: $("#" + labelID).text(),
        UNIQUE_TEMPLATE_NAME: $("#txtTemplateName").val()
      
    }
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/DeleteConfigDataByFieldCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(deleteData)
    }
    $.ajax(settings)
        .done(function (response) {

            $("#" + hiddenID).val("");
            $("#" + rdoButtonID).prop("checked", false).attr(DISABLED, DISABLED);
            $("#" + checkboxID).prop("checked", false).removeAttr(DISABLED, DISABLED);
            $delete.closest("tr").removeClass("row-highlight");
            $delete.attr(DISABLED, DISABLED);
            var successMessage = $("#lblSuccessMessageS_Maker_MillSheet_003").text();
            showMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
$("#btnDelete").click(function () {

    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/DeleteConfigDataByTemplateName?templateName=" + $("#txtTemplateName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            DisableEditDeleteButton();
            var successMessage = $("#lblSuccessMessageS_Maker_MillSheet_003").text();
            showMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
});
function IsTemplateNameAlreadyExist()
{
    var retunVal = false;
    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/IsTemplateNameAlreadyExist?templateName=" + $("#txtTemplateName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            retunVal = response.message;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return retunVal;
}
function IsFieldElementAlreadySaved(fieldCD, templateName)
{
    var isSaved = false;
    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/IsFieldElementSaved?fieldCD=" + fieldCD + "&templateName=" + templateName,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            isSaved = response.message;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_Maker_MillSheet_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return isSaved;
}