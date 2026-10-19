var $btnChuubanSearch;
var $btnSave;
var combinedDecisionOK = "";
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "DISABLED";

let FileNamesplitPath = null;
let PDFFilePath = null;
let PDFPageCount = null;
let PDFpath = null;
var tempMillsheetData = [];
let KoubanName = null;
var curScale = 1.0;
var currentPage = 1;
var totalPageCount = 0;
var runLogCD = 0;
var formData = new FormData();

$(document).ready(function () {
    window.resizeTo(1930, 1300);
    SetDecimalFormats();
    DisableEditDeleteButton();
    SetMatlMakerList();
    DisableButton("prev-page");
    DisableButton("next-page");
    DisableButton("btnRead");
    GetTemplateNameList();
    SetUnitType();
    SetChemicalSpecifcation();
    $("#fileUpload").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    var millsheetCD = GetURLParameters(window.location.href);
    if (millsheetCD > 0) {
        GetMakerMillSheetEditDetailsByCD(millsheetCD);
        EnableEditDeleteButton();
        DisableSaveButton();
        $("#chkConfirmPage").attr("DISABLED", "DISABLED").addClass("DISABLED");
        $("#chkConfirmPage").prop("checked", true);
        $("#fileUpload").attr("DISABLED", "DISABLED").addClass("DISABLED");

    }
    
});
function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) {
        return result;
    } else {
        var sPageURL = url.substring(searchIndex + 1);
        var sParameterName = url.split('millsheetCD=');
        result = sParameterName[1];
        return result;
    }
}
function SetDecimalFormats()
{
    $("#txtDimension").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });
    $("#txtLength").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });
    $("#txtNoOfUnits").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
    $("#txtWeightWithoutKG").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    
    $("#txtGainsizeSTD, #txtGainSizeActual").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });
    $(".HardnessTextFormat").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 1);
    });
    
}
$("#btnSelect").click(function () {
    location.href = "/MakerMillSheet/MakerMillSheetListPage";
    window.resizeTo(1300, 970);
});

function DisableButton(buttonId) {
    $("#"+buttonId).attr("DISABLED", "DISABLED").addClass("DISABLED");
}

function EnableButton(buttonId) {
    $("#" + buttonId).removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
}
function SetMatlMakerList() {
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
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindMakerDropdown(jsonResult) {
    $("#ddlMakerName").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMakerName").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
    });
}
function UpdatePageButtons() {
    if (currentPage === 1) {
        DisableButton("prev-page");
    } else {
        EnableButton("prev-page");
    }

    if (currentPage === totalPageCount) {
        DisableButton("next-page");
    } else {
        EnableButton("next-page");
    }
}

$("#prev-page").click(function () {
    debugger;
    if (currentPage > 1) {
        
     
        if ($("#chkConfirmPage").prop("checked") == true && !($("#chkConfirmPage").hasClass(DISABLED)))
        {
            if (!IsMandatoryFieldsFilled()) {

                return false;
            }
            
        }
        if ($("#txtkouban").val() != "" && $("#chkConfirmPage").prop("checked") == true) {
            if (IsKoubanAlreadyExist()) {
                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessageE_KubunAlreadyUse_03").text()) + "</td></tr></table>",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (r) {

                        if (r) {
                            FillTempMillsheetData();
                            ClearData();
                            SetTempMillsheetData(currentPage - 1);
                            ShowPage(currentPage - 1);

                        } else {
                            return false;
                        }
                    },
                    centerVertical: true
                });
            } else {
                FillTempMillsheetData();
                ClearData();
                SetTempMillsheetData(currentPage - 1);
                ShowPage(currentPage - 1);
            }

        } else
        {
            FillTempMillsheetData();
            ClearData();
            SetTempMillsheetData(currentPage - 1);
            ShowPage(currentPage - 1);
        }
        if ($("#chkConfirmPage").prop("checked") == true)
        {

            if ((IsMillSheetAlreadySaved())) {
                $("#chkConfirmPage").prop("checked", true);
                $("#chkConfirmPage").attr("DISABLED", "DISABLED").addClass("DISABLED");
            }
            else {
                $("#chkConfirmPage").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
            }
        }
    }
});
$("#next-page").click(function () {
    debugger;
    if (currentPage < totalPageCount) {
       
        if ($("#chkConfirmPage").prop("checked")==true && !($("#chkConfirmPage").hasClass(DISABLED)))
        {
            if (!IsMandatoryFieldsFilled())
            {
                return false;
            }

        }
        if ($("#txtkouban").val() != "" && $("#chkConfirmPage").prop("checked") == true) {
            if (IsKoubanAlreadyExist()) {

                bootbox.confirm({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessageE_KubunAlreadyUse_03").text()) + "</td></tr></table>",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (r) {

                        if (r) {
                            FillTempMillsheetData();
                            ClearData();
                            ShowPage(currentPage + 1);

                            var getTempData = tempMillsheetData.filter(x => x.PAGE_NO == currentPage)[0];

                            if (parseInt($("#ddlTemplateName").val()) > 0 && getTempData == null) {
                                $("#btnRead").trigger("click"); // called to read ocr button
                            } else {
                                SetTempMillsheetData(currentPage);
                            }

                        } 
                    },
                    centerVertical: true
                });
            }else {
                FillTempMillsheetData();
                ClearData();
                ShowPage(currentPage + 1);

                var getTempData = tempMillsheetData.filter(x => x.PAGE_NO == currentPage)[0];

                if (parseInt($("#ddlTemplateName").val()) > 0 && getTempData == null) {
                    $("#btnRead").trigger("click"); // called to read ocr button
                } else {
                    SetTempMillsheetData(currentPage);
                }
            }
        } else
        {
            FillTempMillsheetData();
            ClearData();
            ShowPage(currentPage + 1);

            var getTempData = tempMillsheetData.filter(x => x.PAGE_NO == currentPage)[0];

            if (parseInt($("#ddlTemplateName").val()) > 0 && getTempData == null) {
                $("#btnRead").trigger("click"); // called to read ocr button
            } else {
                SetTempMillsheetData(currentPage);
            }
        }
        if ($("#chkConfirmPage").prop("checked") == true)
        {
           
            if ((IsMillSheetAlreadySaved())) {
                $("#chkConfirmPage").prop("checked", true);
                $("#chkConfirmPage").attr("DISABLED", "DISABLED").addClass("DISABLED");
            }
            else {
                $("#chkConfirmPage").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
            }
        }
    }
});

function EnableEditDeleteButton() {

    $("#btnEdit").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    $("#btnDelete").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");

}
function DisableEditDeleteButton() {
    $("#btnEdit").attr("DISABLED", "DISABLED").addClass("DISABLED");
    $("#btnDelete").attr("DISABLED", "DISABLED").addClass("DISABLED");

}
function EnableSaveButton() {

    $("#btnSave").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");

}
function DisableSaveButton() {
    $("#btnSave").attr("DISABLED", "DISABLED").addClass("DISABLED");
}
$("#fileUpload").change(function () {
    $("#pages").html("");
    var file = this.files[0];
    //Added by Sunil For updating pdf
    //var millsheetCD = GetURLParameters(window.location.href);
    //if (millsheetCD > 0) {
    //    DisableButton("btnRead");
    //}
    //-----------------------
    if (file) {
        var extension = file.name.split('.').pop().toLowerCase();
        if (extension != "pdf") {
            DisableButton("prev-page");
            DisableButton("next-page");
            DisableButton("btnRead");
            $("#page-indicator").text("99/99");
            ShowMessageDialog($.trim($("#lblErrorMessageE_PDF_Extention_02").text()), "fileUpload", WARNING);
            $(this).val("");
            return false;
        } else {

            curScale = 1.0;
            currentPage = 1;
            totalPageCount = 0;
            ShowPdfFile(file);
                      
            EnableButton("prev-page");
            EnableButton("next-page");
            EnableButton("btnRead");
        }
    }else
    {
        $("#page-indicator").text("99/99");
    }
   
});
function GetTemplateNameList()
{
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetUniqueTemplateList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            $("#ddlTemplateName").html("").append($('<option></option>'));
            $.each(jsonResult, function (key, jsonObj) {
                $("#ddlTemplateName").append($('<option value="' + jsonObj.cd + '">' + jsonObj.name + '</option>'));
            });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
$("#ddlTemplateName").change(function () {
    var configCD = $("#ddlTemplateName").val();

    if (configCD > 0) {
        $("#ddlMakerName").val(configCD);
    } else {
        $('#ddlMakerName').prop('selectedIndex', 0);

    }
});
function SetUnitType() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/MakerMillSheet/GetUnitType",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindUnitTypeDropdown(jsonResult) {
    $("#ddlUnitType").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '" >' + jsonObj.name + '</option>'));
    });
}

$("#btnRead").click(function ()
{
    var msg = "";
    var focusField = "";
    if ($("#ddlTemplateName").val() <= 0)
    {
        msg = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_005").text());
        focusField = "ddlTemplateName";
    }
    //  EnableSaveButton();
   
    var file = $("#fileUpload")[0].files[0];

    if (!file)
    {
        if (msg == "") {
            msg = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_006").text());
        } else
        {
            msg = msg + "<br>" + $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_006").text());
        }
        if (focusField == "")
        {
            focusField = "fileUpload";
        }
    }
    if (msg != "")
    {
        ShowMessageDialog(msg, focusField, WARNING);
        return false;
    }
    ClearData();
    ProcessOCRData();
    //ReadPdfData();
    //if ((IsMillSheetAlreadySaved())) {
    //    $("#chkConfirmPage").prop("checked", true);
    //    $("#chkConfirmPage").attr("DISABLED", "DISABLED").addClass("DISABLED");
    //}
    //else {
    //    $("#chkConfirmPage").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    //}
});
async function ProcessOCRData() {
    
    await ReadPdfData();
   
}
async function ShowSavedPDF(pdfPath) {
    
    $("#pages").html(""); // clear once

    const pdf = await pdfjsLib.getDocument(pdfPath).promise;
   // console.log("Le PDF contient", pdf.numPages, "page(s).");
    totalPageCount = pdf.numPages;

    for (let pageNum = 1; pageNum <= totalPageCount; pageNum++) {
        let page = await pdf.getPage(pageNum);
        let scale = 1.7;
        let viewport = page.getViewport({ scale: scale });

        let canvas = document.createElement("canvas");
        canvas.className = "page";
        canvas.title = "Page " + pageNum;
        canvas.height = viewport.height; //920;
        canvas.width = viewport.width; //1400;

        if (pageNum !== 1) {
            canvas.style.display = "none";
        }

        document.querySelector("#pages").appendChild(canvas);

        await page.render({
            canvasContext: canvas.getContext("2d"),
            viewport: viewport
        }).promise;

    }
}

async function ShowPdfFile(file) {

    $("#pages").html("");

    var typedarray = new Uint8Array(await file.arrayBuffer());

    const pdf = await pdfjsLib.getDocument(typedarray).promise;
   // console.log("Le PDF contient", pdf.numPages, "page(s).");
    totalPageCount = pdf.numPages;

    for (let pageNum = 1; pageNum <= totalPageCount; pageNum++) {
        let page = await pdf.getPage(pageNum);
        let scale = 1.7;
        let viewport = page.getViewport({ scale: scale });

        let canvas = document.createElement("canvas");
        canvas.className = "page";
        canvas.title = "Page " + pageNum;
        canvas.height = viewport.height;//920;
        canvas.width = viewport.width;//1400;

        if (pageNum !== 1) {
            canvas.style.display = "none";
        }

        document.querySelector("#pages").appendChild(canvas);

        await page.render({
            canvasContext: canvas.getContext("2d"),
            viewport: viewport
        }).promise;

       // console.log("Page render :", pageNum);
    }
    ShowPage(1);
    UpdatePageButtons();

}
function ShowPage(pageNumber) {
    currentPage = pageNumber;
  
    var pages = document.querySelectorAll(".page");

    for (var i = 0; i < pages.length; i++) {

        var page = pages[i];
        page.style.display = i + 1 === pageNumber ? "block" : "none";

    }
    var pageIndicator = document.querySelector("#page-indicator");
    pageIndicator.textContent = pageNumber + "/" + totalPageCount;

    UpdatePageButtons();
}
$("#btnSave").click(function () {
    
    var fileExist = $("#fileUpload")[0].files[0];

    if (!fileExist)
    {
        ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_010").text()), "", WARNING);
        return false;
    }

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }

   
    for (var i = 1; i <= 25; i++)
    {
        if (parseInt($("#txtMin" + i).val()) > 0 && parseInt($("#txtMax" + i).val()) > 0)
        {
            if (parseInt($("#txtMin" + i).val()) > parseInt($("#txtMax" + i).val()))
            {
                ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_007").text()), "txtMin"+i, WARNING);
                return false;
            }
        }
    }

    for (var i = 1; i <= 25; i++) {
        if (parseInt($("#txtHardMin" + i).val()) > 0 && parseInt($("#txtHardMax" + i).val()) > 0) {
            if (parseInt($("#txtHardMin" + i).val()) > parseInt($("#txtHardMax" + i).val())) {
                ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_008").text()), "txtHardMin" + i, WARNING);
                return false;
            }
        }
    }
    
    var ftpDetails = IsFTPServerActive();
    if (ftpDetails.length == 0)
    {
        return false;
    }
    if (ftpDetails.IsSuccess == false)
    {
        ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_011").text().replace("NNN", ftpDetails.IpAddress)), "", WARNING);
        return false;

    }

    if (runLogCD == 0) {
        runLogCD = GetLogRunCD();
    }
    BindFormDataForSave();

    var saveData = tempMillsheetData.filter(x => x.IS_CONFIRMED == 1 && x.ALREADY_SAVED == 'NO'); // checked if checkbox chcked or not
    if (saveData.length == 0) {
        ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_009").text()), "chkConfirmPage", WARNING);
        return false;

    }

    if ($("#chkConfirmPage").prop("checked") == true && !($("#chkConfirmPage").hasClass(DISABLED))) {
        if (IsMillSheetAlreadySaved()) {
            ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_009").text()), "", WARNING);
            return false;
        }
    }

    if ($("#txtkouban").val() != "") {
        if (IsKoubanAlreadyExist())
        {
            bootbox.confirm({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessageE_KubunAlreadyUse_03").text()) + "</td></tr></table>",
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {

                    if (result) {
                        SaveMakerMillSheetDetails();
                    }
                    else {
                        return false;

                    }
                },
                centerVertical: true
            });
        }else {

            SaveMakerMillSheetDetails();
        }
    }else
    {

        SaveMakerMillSheetDetails();
    }
   
});

function IsKoubanAlreadyExist()
{
    var returnKoubun = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetKubunName?KubunName=" + $("#txtkouban").val(),
        method: "GET",
        contentType: "application/json; charset=utf-8",
     
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            returnKoubun = response.isSuccess;
            
        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnKoubun;
}
function GetLogRunCD()
{
    var runCD =0;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetMaxLogRunCD",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
           runCD = response.message;
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);

        });
    return runCD;
}
function SetChemicalSpecifcation() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetChemicalSpecifcation",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindChemicalSpecifcationDropdown(response.message);
        })
        .fail(function (xhr, testStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);

        });
}
function BindChemicalSpecifcationDropdown(jsonResult) {
    $.each(jsonResult, function (key, jsonObj) {

        $("#ddltd1").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));

        $("#ddltd2").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));

        $("#ddltd3").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd4").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd5").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd6").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd7").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd8").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd9").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd10").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd11").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd12").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd13").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd14").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd15").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd16").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd17").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd18").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd19").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd20").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd21").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd22").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd23").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd24").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
        $("#ddltd25").append($('<option value="' + jsonObj.name + '" >' + jsonObj.name + '</option>'));
    });

    SetChemicalComposion();
   
}
function SetChemicalComposion()
{
    var txtCCName1 = $("#txtCCName1").val();
    var txtCCName2 = $("#txtCCName2").val();
    var txtCCName3 = $("#txtCCName3").val();
    var txtCCName4 = $("#txtCCName4").val();
    var txtCCName5 = $("#txtCCName5").val();
    var txtCCName6 = $("#txtCCName6").val();
    var txtCCName7 = $("#txtCCName7").val();
    var txtCCName8 = $("#txtCCName8").val();
    var txtCCName9 = $("#txtCCName9").val();
    var txtCCName10 = $("#txtCCName10").val();
    
    if (txtCCName1 == "") {
        var valueCCName1 = $("#ddltd1 option:eq(0)").val();
        $("#txtCCName1").val(valueCCName1);
        $("#ddltd1").val(valueCCName1);
    }
    if (txtCCName2 == "") {
        var valueCCName2 = $("#ddltd1 option:eq(1)").val();
        $("#txtCCName2").val(valueCCName2);
        $("#ddltd2").val(valueCCName2);
    }
    if (txtCCName3 == "") {
        var valueCCName3 = $("#ddltd1 option:eq(2)").val();
        $("#txtCCName3").val(valueCCName3);
        $("#ddltd3").val(valueCCName3);
    }
    if (txtCCName4 == "") {
        var valueCCName4 = $("#ddltd1 option:eq(3)").val();
        $("#txtCCName4").val(valueCCName4);
        $("#ddltd4").val(valueCCName4);
    }
    if (txtCCName5 == "") {
        var valueCCName5 = $("#ddltd1 option:eq(4)").val();
        $("#txtCCName5").val(valueCCName5);
        $("#ddltd5").val(valueCCName5);
    }
    if (txtCCName6 == "") {
        var valueCCName6 = $("#ddltd1 option:eq(5)").val();
        $("#txtCCName6").val(valueCCName6);
        $("#ddltd6").val(valueCCName6);
    }
    if (txtCCName7 == "") {
        var valueCCName7 = $("#ddltd1 option:eq(6)").val();
        $("#txtCCName7").val(valueCCName7);
        $("#ddltd7").val(valueCCName7);

    }
    if (txtCCName8 == "") {
        var valueCCName8 = $("#ddltd1 option:eq(7)").val();
        $("#txtCCName8").val(valueCCName8);
        $("#ddltd8").val(valueCCName8);
    }
    if (txtCCName9 == "") {
        var valueCCName9 = $("#ddltd1 option:eq(8)").val();
        $("#txtCCName9").val(valueCCName9);
        $("#ddltd9").val(valueCCName9);
    }
    if (txtCCName10 == "") {
        var valueCCName10 = $("#ddltd1 option:eq(9)").val();
        $("#txtCCName10").val(valueCCName10);
        $("#ddltd10").val(valueCCName10);
    }
    $("#ddltd11").prop("selectedIndex",-1);
    $("#ddltd12").prop("selectedIndex",-1);
    $("#ddltd13").prop("selectedIndex",-1);
    $("#ddltd14").prop("selectedIndex",-1);
    $("#ddltd15").prop("selectedIndex",-1);
    $("#ddltd16").prop("selectedIndex",-1);
    $("#ddltd17").prop("selectedIndex",-1);
    $("#ddltd18").prop("selectedIndex",-1);
    $("#ddltd19").prop("selectedIndex",-1);
    $("#ddltd20").prop("selectedIndex",-1);
    $("#ddltd21").prop("selectedIndex",-1);
    $("#ddltd22").prop("selectedIndex",-1);
    $("#ddltd23").prop("selectedIndex",-1);
    $("#ddltd24").prop("selectedIndex",-1);
    $("#ddltd25").prop("selectedIndex",-1);
}
function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    var firstFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === typeDropDown && !($(mandatoryField).hasClass(disabled))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === typeText && !($(mandatoryField).hasClass(disabled))) {
            if (!mandatoryField.value || ($("#" + mandatoryField.id).val().trim().length) <= 0) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (firstFieldID === "") {
                    firstFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
       
        ShowMessageDialog(message, firstFieldID, WARNING);
      
    }
    return returnVal;

}
function SaveMakerMillSheetDetails()
{
    
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        processData: false,       // important for FormData
        contentType:false, //fileExist ? false : "application/json; charset=utf-8",
        dataType: "json",
        url: APIURL + "/MakerMillSheet/MakerMillSheetDetailsSave",
        method: "POST",
        data:formData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.message == 0) // 0 for successfull save
            {
                DisableSaveButton();
                var successMessage = $("#lblSuccessMessageS_MAKERMILLSHEET_ENTRY_001").text();
                ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);
            } 
       
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function ShowMessageDialog(message, focusFieldID, messageType) {
    
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
function ReadFile(pageNo)
{
   // var pageNo = $("#page-indicator").text().split('/')[0];
    var listData = []
    var ajaxTime = new Date().getTime();
    var frmData = new FormData();
    frmData.append('file', $("#fileUpload")[0].files[0]);
    frmData.append('pageNo', pageNo);
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        processData: false,       // important for FormData
        dataType: "json",
        url: "/MakerMillSheet/SetBase64StringForUpload",
        method: "POST",
        contentType: false,
        data: frmData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            listData = response.message;

        })
        .fail(function (xhr, textStatus, errorThrown) {
            $("#LoadingImage").hide();
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return listData;
}
function GetMakerMillSheetEditDetailsByCD(millsheetCD) {
   
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetMakerMillSheetDetailsByCD?millsheetCD=" + millsheetCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
           
            $("#hdfMakerMillSheetCD").val(jsonResult.makeR_MILL_SHEET_CD);
            $("#ddlMakerName").val(jsonResult.makeR_CD);
            $("#txtTypeofSteel").val(jsonResult.matL_NAME_MAKER);
            $("#txtDimension").val(jsonResult.diM_D);
            $("#txtLength").val(jsonResult.length);
            $("#txtcharge").val(jsonResult.charge);
            $("#txtkouban").val(jsonResult.kouban);
            $("#txtNoOfUnits").val(jsonResult.nO_OF_UNITS);
            $("#ddlUnitType").val(jsonResult.unit);
            $("#txtWeightWithoutKG").val(jsonResult.quantity);
            $("#pdf-upload").text(jsonResult.filename);
            $("#ddlUnitType").val(jsonResult.unit);
            $("#txtGainsizeSTD").val(jsonResult.graiN_SIZE_STD);
            $("#txtGainSizeActual").val(jsonResult.graiN_SIZE_ACTUAL);
            $("#txtHardnessmakerSTD").val(jsonResult.hardnesS_STD);
            $("#txtHardnessMin").val(jsonResult.hardnesS_MIN);
            $("#txtHardnessMax").val(jsonResult.hardnesS_MAX);
            $("#txtHardnessMakerActual").val(jsonResult.hardnesS_ACTUAL);
            
            // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------

            $("#txtCCName1").val(jsonResult.ccnamE1)
            $("#txtCCName2").val(jsonResult.ccnamE2)
            $("#txtCCName3").val(jsonResult.ccnamE3)
            $("#txtCCName4").val(jsonResult.ccnamE4)
            $("#txtCCName5").val(jsonResult.ccnamE5)
            $("#txtCCName6").val(jsonResult.ccnamE6)
            $("#txtCCName7").val(jsonResult.ccnamE7)
            $("#txtCCName8").val(jsonResult.ccnamE8)
            $("#txtCCName9").val(jsonResult.ccnamE9)
            $("#txtCCName10").val(jsonResult.ccnamE10)
            $("#txtCCName11").val(jsonResult.ccnamE11)
            $("#txtCCName12").val(jsonResult.ccnamE12)
            $("#txtCCName13").val(jsonResult.ccnamE13)
            $("#txtCCName14").val(jsonResult.ccnamE14)
            $("#txtCCName15").val(jsonResult.ccnamE15)
            $("#txtCCName16").val(jsonResult.ccnamE16)
            $("#txtCCName17").val(jsonResult.ccnamE17)
            $("#txtCCName18").val(jsonResult.ccnamE18)
            $("#txtCCName19").val(jsonResult.ccnamE19)
            $("#txtCCName20").val(jsonResult.ccnamE20)
            $("#txtCCName21").val(jsonResult.ccnamE21)
            $("#txtCCName22").val(jsonResult.ccnamE22)
            $("#txtCCName23").val(jsonResult.ccnamE23)
            $("#txtCCName24").val(jsonResult.ccnamE24)
            $("#txtCCName25").val(jsonResult.ccnamE25)
            $("#txtCCRange1").val(jsonResult.ccrangE1)
            $("#txtCCRange2").val(jsonResult.ccrangE2)
            $("#txtCCRange3").val(jsonResult.ccrangE3)
            $("#txtCCRange4").val(jsonResult.ccrangE4)
            $("#txtCCRange5").val(jsonResult.ccrangE5)
            $("#txtCCRange6").val(jsonResult.ccrangE6)
            $("#txtCCRange7").val(jsonResult.ccrangE7)
            $("#txtCCRange8").val(jsonResult.ccrangE8)
            $("#txtCCRange9").val(jsonResult.ccrangE9)
            $("#txtCCRange10").val(jsonResult.ccrangE10)
            $("#txtCCRange11").val(jsonResult.ccrangE11)
            $("#txtCCRange12").val(jsonResult.ccrangE12)
            $("#txtCCRange13").val(jsonResult.ccrangE13)
            $("#txtCCRange14").val(jsonResult.ccrangE14)
            $("#txtCCRange15").val(jsonResult.ccrangE15)
            $("#txtCCRange16").val(jsonResult.ccrangE16)
            $("#txtCCRange17").val(jsonResult.ccrangE17)
            $("#txtCCRange18").val(jsonResult.ccrangE18)
            $("#txtCCRange19").val(jsonResult.ccrangE19)
            $("#txtCCRange20").val(jsonResult.ccrangE20)
            $("#txtCCRange21").val(jsonResult.ccrangE21)
            $("#txtCCRange22").val(jsonResult.ccrangE22)
            $("#txtCCRange23").val(jsonResult.ccrangE23)
            $("#txtCCRange24").val(jsonResult.ccrangE24)
            $("#txtCCRange25").val(jsonResult.ccrangE25)
            $("#txtMin1").val(jsonResult.ccmiN1)
            $("#txtMin2").val(jsonResult.ccmiN2)
            $("#txtMin3").val(jsonResult.ccmiN3)
            $("#txtMin4").val(jsonResult.ccmiN4)
            $("#txtMin5").val(jsonResult.ccmiN5)
            $("#txtMin6").val(jsonResult.ccmiN6)
            $("#txtMin7").val(jsonResult.ccmiN7)
            $("#txtMin8").val(jsonResult.ccmiN8)
            $("#txtMin9").val(jsonResult.ccmiN9)
            $("#txtMin10").val(jsonResult.ccmiN10)
            $("#txtMin11").val(jsonResult.ccmiN11)
            $("#txtMin12").val(jsonResult.ccmiN12)
            $("#txtMin13").val(jsonResult.ccmiN13)
            $("#txtMin14").val(jsonResult.ccmiN14)
            $("#txtMin15").val(jsonResult.ccmiN15)
            $("#txtMin16").val(jsonResult.ccmiN16)
            $("#txtMin17").val(jsonResult.ccmiN17)
            $("#txtMin18").val(jsonResult.ccmiN18)
            $("#txtMin19").val(jsonResult.ccmiN19)
            $("#txtMin20").val(jsonResult.ccmiN20)
            $("#txtMin21").val(jsonResult.ccmiN21)
            $("#txtMin22").val(jsonResult.ccmiN22)
            $("#txtMin23").val(jsonResult.ccmiN23)
            $("#txtMin24").val(jsonResult.ccmiN24)
            $("#txtMin25").val(jsonResult.ccmiN25)
            $("#txtMax1").val(jsonResult.ccmaX1)
            $("#txtMax2").val(jsonResult.ccmaX2)
            $("#txtMax3").val(jsonResult.ccmaX3)
            $("#txtMax4").val(jsonResult.ccmaX4)
            $("#txtMax5").val(jsonResult.ccmaX5)
            $("#txtMax6").val(jsonResult.ccmaX6)
            $("#txtMax7").val(jsonResult.ccmaX7)
            $("#txtMax8").val(jsonResult.ccmaX8)
            $("#txtMax9").val(jsonResult.ccmaX9)
            $("#txtMax10").val(jsonResult.ccmaX10)
            $("#txtMax11").val(jsonResult.ccmaX11)
            $("#txtMax12").val(jsonResult.ccmaX12)
            $("#txtMax13").val(jsonResult.ccmaX13)
            $("#txtMax14").val(jsonResult.ccmaX14)
            $("#txtMax15").val(jsonResult.ccmaX15)
            $("#txtMax16").val(jsonResult.ccmaX16)
            $("#txtMax17").val(jsonResult.ccmaX17)
            $("#txtMax18").val(jsonResult.ccmaX18)
            $("#txtMax19").val(jsonResult.ccmaX19)
            $("#txtMax20").val(jsonResult.ccmaX20)
            $("#txtMax21").val(jsonResult.ccmaX21)
            $("#txtMax22").val(jsonResult.ccmaX22)
            $("#txtMax23").val(jsonResult.ccmaX23)
            $("#txtMax24").val(jsonResult.ccmaX24)
            $("#txtMax25").val(jsonResult.ccmaX25)
            
            $("#txtCCActual1").val(jsonResult.ccactuaL1)
            $("#txtCCActual2").val(jsonResult.ccactuaL2)
            $("#txtCCActual3").val(jsonResult.ccactuaL3)
            $("#txtCCActual4").val(jsonResult.ccactuaL4)
            $("#txtCCActual5").val(jsonResult.ccactuaL5)
            $("#txtCCActual6").val(jsonResult.ccactuaL6)
            $("#txtCCActual7").val(jsonResult.ccactuaL7)
            $("#txtCCActual8").val(jsonResult.ccactuaL8)
            $("#txtCCActual9").val(jsonResult.ccactuaL9)
            $("#txtCCActual10").val(jsonResult.ccactuaL10)
            $("#txtCCActual11").val(jsonResult.ccactuaL11)
            $("#txtCCActual12").val(jsonResult.ccactuaL12)
            $("#txtCCActual13").val(jsonResult.ccactuaL13)
            $("#txtCCActual14").val(jsonResult.ccactuaL14)
            $("#txtCCActual15").val(jsonResult.ccactuaL15)
            $("#txtCCActual16").val(jsonResult.ccactuaL16)
            $("#txtCCActual17").val(jsonResult.ccactuaL17)
            $("#txtCCActual18").val(jsonResult.ccactuaL18)
            $("#txtCCActual19").val(jsonResult.ccactuaL19)
            $("#txtCCActual20").val(jsonResult.ccactuaL20)
            $("#txtCCActual21").val(jsonResult.ccactuaL21)
            $("#txtCCActual22").val(jsonResult.ccactuaL22)
            $("#txtCCActual23").val(jsonResult.ccactuaL23)
            $("#txtCCActual24").val(jsonResult.ccactuaL24)
            $("#txtCCActual25").val(jsonResult.ccactuaL25)

            // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------

            $("#txtHardSTD1").val(jsonResult.harD_STD1)
            $("#txtHardSTD2").val(jsonResult.harD_STD2)
            $("#txtHardSTD3").val(jsonResult.harD_STD3)
            $("#txtHardSTD4").val(jsonResult.harD_STD4)
            $("#txtHardSTD5").val(jsonResult.harD_STD5)
            $("#txtHardSTD6").val(jsonResult.harD_STD6)
            $("#txtHardSTD7").val(jsonResult.harD_STD7)
            $("#txtHardSTD8").val(jsonResult.harD_STD8)
            $("#txtHardSTD9").val(jsonResult.harD_STD9)
            $("#txtHardSTD10").val(jsonResult.harD_STD10)
            $("#txtHardSTD11").val(jsonResult.harD_STD11)
            $("#txtHardSTD12").val(jsonResult.harD_STD12)
            $("#txtHardSTD13").val(jsonResult.harD_STD13)
            $("#txtHardSTD14").val(jsonResult.harD_STD14)
            $("#txtHardSTD15").val(jsonResult.harD_STD15)
            $("#txtHardSTD16").val(jsonResult.harD_STD16)
            $("#txtHardSTD17").val(jsonResult.harD_STD17)
            $("#txtHardSTD18").val(jsonResult.harD_STD18)
            $("#txtHardSTD19").val(jsonResult.harD_STD19)
            $("#txtHardSTD20").val(jsonResult.harD_STD20)
            $("#txtHardSTD21").val(jsonResult.harD_STD21)
            $("#txtHardSTD22").val(jsonResult.harD_STD22)
            $("#txtHardSTD23").val(jsonResult.harD_STD23)
            $("#txtHardSTD24").val(jsonResult.harD_STD24)
            $("#txtHardSTD25").val(jsonResult.harD_STD25)
            $("#txtHardMin1").val(jsonResult.harD_MIN1)
            $("#txtHardMin2").val(jsonResult.harD_MIN2)
            $("#txtHardMin3").val(jsonResult.harD_MIN3)
            $("#txtHardMin4").val(jsonResult.harD_MIN4)
            $("#txtHardMin5").val(jsonResult.harD_MIN5)
            $("#txtHardMin6").val(jsonResult.harD_MIN6)
            $("#txtHardMin7").val(jsonResult.harD_MIN7)
            $("#txtHardMin8").val(jsonResult.harD_MIN8)
            $("#txtHardMin9").val(jsonResult.harD_MIN9)
            $("#txtHardMin10").val(jsonResult.harD_MIN10)
            $("#txtHardMin11").val(jsonResult.harD_MIN11)
            $("#txtHardMin12").val(jsonResult.harD_MIN12)
            $("#txtHardMin13").val(jsonResult.harD_MIN13)
            $("#txtHardMin14").val(jsonResult.harD_MIN14)
            $("#txtHardMin15").val(jsonResult.harD_MIN15)
            $("#txtHardMin16").val(jsonResult.harD_MIN16);
            $("#txtHardMin17").val(jsonResult.harD_MIN17);
            $("#txtHardMin18").val(jsonResult.harD_MIN18);
            $("#txtHardMin19").val(jsonResult.harD_MIN19);
            $("#txtHardMin20").val(jsonResult.harD_MIN20);
            $("#txtHardMin21").val(jsonResult.harD_MIN21);
            $("#txtHardMin22").val(jsonResult.harD_MIN22);
            $("#txtHardMin23").val(jsonResult.harD_MIN23);
            $("#txtHardMin24").val(jsonResult.harD_MIN24);
            $("#txtHardMin25").val(jsonResult.harD_MIN25);
            $("#txtHardMax1").val(jsonResult.harD_MAX1);
            $("#txtHardMax2").val(jsonResult.harD_MAX2);
            $("#txtHardMax3").val(jsonResult.harD_MAX3);
            $("#txtHardMax4").val(jsonResult.harD_MAX4);
            $("#txtHardMax5").val(jsonResult.harD_MAX5);
            $("#txtHardMax6").val(jsonResult.harD_MAX6);
            $("#txtHardMax7").val(jsonResult.harD_MAX7);
            $("#txtHardMax8").val(jsonResult.harD_MAX8);
            $("#txtHardMax9").val(jsonResult.harD_MAX9);
            $("#txtHardMax10").val(jsonResult.harD_MAX10);
            $("#txtHardMax11").val(jsonResult.harD_MAX11);
            $("#txtHardMax12").val(jsonResult.harD_MAX12);
            $("#txtHardMax13").val(jsonResult.harD_MAX13);
            $("#txtHardMax14").val(jsonResult.harD_MAX14);
            $("#txtHardMax15").val(jsonResult.harD_MAX15);
            $("#txtHardMax16").val(jsonResult.harD_MAX16);
            $("#txtHardMax17").val(jsonResult.harD_MAX17);
            $("#txtHardMax18").val(jsonResult.harD_MAX18);
            $("#txtHardMax19").val(jsonResult.harD_MAX19);
            $("#txtHardMax20").val(jsonResult.harD_MAX20);
            $("#txtHardMax21").val(jsonResult.harD_MAX21);
            $("#txtHardMax22").val(jsonResult.harD_MAX22);
            $("#txtHardMax23").val(jsonResult.harD_MAX23);
            $("#txtHardMax24").val(jsonResult.harD_MAX24);
            $("#txtHardMax25").val(jsonResult.harD_MAX25);
            $("#txtHardActual1").val(jsonResult.harD_ACTUAL1);
            $("#txtHardActual2").val(jsonResult.harD_ACTUAL2);
            $("#txtHardActual3").val(jsonResult.harD_ACTUAL3);
            $("#txtHardActual4").val(jsonResult.harD_ACTUAL4);
            $("#txtHardActual5").val(jsonResult.harD_ACTUAL5);
            $("#txtHardActual6").val(jsonResult.harD_ACTUAL6);
            $("#txtHardActual7").val(jsonResult.harD_ACTUAL7);
            $("#txtHardActual8").val(jsonResult.harD_ACTUAL8);
            $("#txtHardActual9").val(jsonResult.harD_ACTUAL9);
            $("#txtHardActual10").val(jsonResult.harD_ACTUAL10);
            $("#txtHardActual11").val(jsonResult.harD_ACTUAL11);
            $("#txtHardActual12").val(jsonResult.harD_ACTUAL12);
            $("#txtHardActual13").val(jsonResult.harD_ACTUAL13);
            $("#txtHardActual14").val(jsonResult.harD_ACTUAL14);
            $("#txtHardActual15").val(jsonResult.harD_ACTUAL15);
            $("#txtHardActual16").val(jsonResult.harD_ACTUAL16);
            $("#txtHardActual17").val(jsonResult.harD_ACTUAL17);
            $("#txtHardActual18").val(jsonResult.harD_ACTUAL18);
            $("#txtHardActual19").val(jsonResult.harD_ACTUAL19);
            $("#txtHardActual20").val(jsonResult.harD_ACTUAL20);
            $("#txtHardActual21").val(jsonResult.harD_ACTUAL21);
            $("#txtHardActual22").val(jsonResult.harD_ACTUAL22);
            $("#txtHardActual23").val(jsonResult.harD_ACTUAL23);
            $("#txtHardActual24").val(jsonResult.harD_ACTUAL24);
            $("#txtHardActual25").val(jsonResult.harD_ACTUAL25);
            
            if (jsonResult.pdF_FILE_PATH)
            {
                DownloadAndShowPDF(jsonResult.pdF_FILE_PATH, jsonResult.filename);
                // Hide file upload
               // $("#fileUpload").hide();
                $("#fileUpload").attr("DISABLED", "DISABLED").addClass("DISABLED");
                // Show filename
                //$("#selectedFileName")
                //    .text(jsonResult.filename)
                //    .show();
                 
                $("#page-indicator").text("1/1");
            }
            else {
                // Show file upload
                //$("#fileUpload").show();

                // Hide filename
              //  $("#selectedFileName").hide();
            }

          
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function DownloadAndShowPDF(filePath,fileName)
{
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/MakerMillSheet/DowloadAndSavePDF?filePath=" + filePath,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess == true)
            {
                ShowSavedPDF(response.message);
            }

        });
        
}
$("#btnInitialize").click(function (event) {
    Initialize();
});
function Initialize() {
    currentPage = 1;
    tempMillsheetData = [];
    $('#ddlTemplateName').prop("selectedIndex", 0);
    DisableButton("btnRead");
    $("#pages").html(""); // clear once
    $("#page-indicator").text("99/99");
    $("#fileUpload").val("");
    $("#fileUpload").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    EnableButton("next-page");
    EnableButton("prev-page");
    EnableSaveButton();
    $("#hdfMakerMillSheetCD").val("");
    $('#ddlMakerName').prop("selectedIndex", 0);
    ClearData();
    $("#chkConfirmPage").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    RemoveParameter("millsheetCD");

}
function ClearData()
{
    $("#chkConfirmPage").prop("checked", false);
    $("#chkConfirmPage").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
    $("#txtTypeofSteel").val("");
    $("#txtDimension").val("");
    $("#txtLength").val("");
    $("#txtcharge").val("");
    $("#txtkouban").val("");
    $("#txtNoOfUnits").val("");
    $("#ddlUnitType").prop("selectedIndex", 0);
    $("#txtWeightWithoutKG").val("");
    $("#fileUpload").text("");
    $("#txtGainsizeSTD").val("");
    $("#txtGainSizeActual").val("");
    $("#txtHardnessmakerSTD").val("");
    $("#txtHardnessMin").val("");
    $("#txtHardnessMax").val("");
    $("#txtHardnessMakerActual").val("");

    // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------

    $("#txtCCName1").val("");
    $("#txtCCName2").val("");
    $("#txtCCName3").val("");
    $("#txtCCName4").val("");
    $("#txtCCName5").val("");
    $("#txtCCName6").val("");
    $("#txtCCName7").val("");
    $("#txtCCName8").val("");
    $("#txtCCName9").val("");
    $("#txtCCName10").val("");
    SetChemicalComposion();

    $("#txtCCName11").val("");
    $("#txtCCName12").val("");
    $("#txtCCName13").val("");
    $("#txtCCName14").val("");
    $("#txtCCName15").val("");
    $("#txtCCName16").val("");
    $("#txtCCName17").val("");
    $("#txtCCName18").val("");
    $("#txtCCName19").val("");
    $("#txtCCName20").val("");
    $("#txtCCName21").val("");
    $("#txtCCName22").val("");
    $("#txtCCName23").val("");
    $("#txtCCName24").val("");
    $("#txtCCName25").val("");
    $("#txtCCRange1").val("");
    $("#txtCCRange2").val("");
    $("#txtCCRange3").val("");
    $("#txtCCRange4").val("");
    $("#txtCCRange5").val("");
    $("#txtCCRange6").val("");
    $("#txtCCRange7").val("");
    $("#txtCCRange8").val("");
    $("#txtCCRange9").val("");
    $("#txtCCRange10").val("");
    $("#txtCCRange11").val("");
    $("#txtCCRange12").val("");
    $("#txtCCRange13").val("");
    $("#txtCCRange14").val("");
    $("#txtCCRange15").val("");
    $("#txtCCRange16").val("");
    $("#txtCCRange17").val("");
    $("#txtCCRange18").val("");
    $("#txtCCRange19").val("");
    $("#txtCCRange20").val("");
    $("#txtCCRange21").val("");
    $("#txtCCRange22").val("");
    $("#txtCCRange23").val("");
    $("#txtCCRange24").val("");
    $("#txtCCRange25").val("");
    $("#txtMin1").val("");
    $("#txtMin2").val("");
    $("#txtMin3").val("");
    $("#txtMin4").val("");
    $("#txtMin5").val("");
    $("#txtMin6").val("");
    $("#txtMin7").val("");
    $("#txtMin8").val("");
    $("#txtMin9").val("");
    $("#txtMin10").val("");
    $("#txtMin11").val("");
    $("#txtMin12").val("");
    $("#txtMin13").val("");
    $("#txtMin14").val("");
    $("#txtMin15").val("");
    $("#txtMin16").val("");
    $("#txtMin17").val("");
    $("#txtMin18").val("");
    $("#txtMin19").val("");
    $("#txtMin20").val("");
    $("#txtMin21").val("");
    $("#txtMin22").val("");
    $("#txtMin23").val("");
    $("#txtMin24").val("");
    $("#txtMin25").val("");
    $("#txtMax1").val("");
    $("#txtMax2").val("");
    $("#txtMax3").val("");
    $("#txtMax4").val("");
    $("#txtMax5").val("");
    $("#txtMax6").val("");
    $("#txtMax7").val("");
    $("#txtMax8").val("");
    $("#txtMax9").val("");
    $("#txtMax10").val("");
    $("#txtMax11").val("");
    $("#txtMax12").val("");
    $("#txtMax13").val("");
    $("#txtMax14").val("");
    $("#txtMax15").val("");
    $("#txtMax16").val("");
    $("#txtMax17").val("");
    $("#txtMax18").val("");
    $("#txtMax19").val("");
    $("#txtMax20").val("");
    $("#txtMax21").val("");
    $("#txtMax22").val("");
    $("#txtMax23").val("");
    $("#txtMax24").val("");
    $("#txtMax25").val("");
    $("#txtCCActual1").val("");
    $("#txtCCActual2").val("");
    $("#txtCCActual3").val("");
    $("#txtCCActual4").val("");
    $("#txtCCActual5").val("");
    $("#txtCCActual6").val("");
    $("#txtCCActual7").val("");
    $("#txtCCActual8").val("");
    $("#txtCCActual9").val("");
    $("#txtCCActual10").val("");
    $("#txtCCActual11").val("");
    $("#txtCCActual12").val("");
    $("#txtCCActual13").val("");
    $("#txtCCActual14").val("");
    $("#txtCCActual15").val("");
    $("#txtCCActual16").val("");
    $("#txtCCActual17").val("");
    $("#txtCCActual18").val("");
    $("#txtCCActual19").val("");
    $("#txtCCActual20").val("");
    $("#txtCCActual21").val("");
    $("#txtCCActual22").val("");
    $("#txtCCActual23").val("");
    $("#txtCCActual24").val("");
    $("#txtCCActual25").val("");

    // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------

    $("#txtHardSTD1").val("");
    $("#txtHardSTD2").val("");
    $("#txtHardSTD3").val("");
    $("#txtHardSTD4").val("");
    $("#txtHardSTD5").val("");
    $("#txtHardSTD6").val("");
    $("#txtHardSTD7").val("");
    $("#txtHardSTD8").val("");
    $("#txtHardSTD9").val("");
    $("#txtHardSTD10").val("");
    $("#txtHardSTD11").val("");
    $("#txtHardSTD12").val("");
    $("#txtHardSTD13").val("");
    $("#txtHardSTD14").val("");
    $("#txtHardSTD15").val("");
    $("#txtHardSTD16").val("");
    $("#txtHardSTD17").val("");
    $("#txtHardSTD18").val("");
    $("#txtHardSTD19").val("");
    $("#txtHardSTD20").val("");
    $("#txtHardSTD21").val("");
    $("#txtHardSTD22").val("");
    $("#txtHardSTD23").val("");
    $("#txtHardSTD24").val("");
    $("#txtHardSTD25").val("");
    $("#txtHardMin1").val("");
    $("#txtHardMin2").val("");
    $("#txtHardMin3").val("");
    $("#txtHardMin4").val("");
    $("#txtHardMin5").val("");
    $("#txtHardMin6").val("");
    $("#txtHardMin7").val("");
    $("#txtHardMin8").val("");
    $("#txtHardMin9").val("");
    $("#txtHardMin10").val("");
    $("#txtHardMin11").val("");
    $("#txtHardMin12").val("");
    $("#txtHardMin13").val("");
    $("#txtHardMin14").val("");
    $("#txtHardMin15").val("");
    $("#txtHardMin16").val("");
    $("#txtHardMin17").val("");
    $("#txtHardMin18").val("");
    $("#txtHardMin19").val("");
    $("#txtHardMin20").val("");
    $("#txtHardMin21").val("");
    $("#txtHardMin22").val("");
    $("#txtHardMin23").val("");
    $("#txtHardMin24").val("");
    $("#txtHardMin25").val("");
    $("#txtHardMax1").val("");
    $("#txtHardMax2").val("");
    $("#txtHardMax3").val("");
    $("#txtHardMax4").val("");
    $("#txtHardMax5").val("");
    $("#txtHardMax6").val("");
    $("#txtHardMax7").val("");
    $("#txtHardMax8").val("");
    $("#txtHardMax9").val("");
    $("#txtHardMax10").val("");
    $("#txtHardMax11").val("");
    $("#txtHardMax12").val("");
    $("#txtHardMax13").val("");
    $("#txtHardMax14").val("");
    $("#txtHardMax15").val("");
    $("#txtHardMax16").val("");
    $("#txtHardMax17").val("");
    $("#txtHardMax18").val("");
    $("#txtHardMax19").val("");
    $("#txtHardMax20").val("");
    $("#txtHardMax21").val("");
    $("#txtHardMax22").val("");
    $("#txtHardMax23").val("");
    $("#txtHardMax24").val("");
    $("#txtHardMax25").val("");
    $("#txtHardActual1").val("");
    $("#txtHardActual2").val("");
    $("#txtHardActual3").val("");
    $("#txtHardActual4").val("");
    $("#txtHardActual5").val("");
    $("#txtHardActual6").val("");
    $("#txtHardActual7").val("");
    $("#txtHardActual8").val("");
    $("#txtHardActual9").val("");
    $("#txtHardActual10").val("");
    $("#txtHardActual11").val("");
    $("#txtHardActual12").val("");
    $("#txtHardActual13").val("");
    $("#txtHardActual14").val("");
    $("#txtHardActual15").val("");
    $("#txtHardActual16").val("");
    $("#txtHardActual17").val("");
    $("#txtHardActual18").val("");
    $("#txtHardActual19").val("");
    $("#txtHardActual20").val("");
    $("#txtHardActual21").val("");
    $("#txtHardActual22").val("");
    $("#txtHardActual23").val("");
    $("#txtHardActual24").val("");
    $("#txtHardActual25").val("");
}
function RemoveParameter(param) {
    const url = new URL(window.location.href);
    url.searchParams.delete(param);
    window.history.replaceState({}, "", url);
}
function IsKoubanAlreadySaved() {
    var returnKoubun = false;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/IsKoubanAlreadySaved?KubunName=" + $("#txtkouban").val() + "&millsheetCD=" + $("#hdfMakerMillSheetCD").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            returnKoubun = response.isSuccess;

        })

        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnKoubun;
}
$("#btnEdit").click(function () {
    if (!IsMandatoryFieldsFilled())
    {
        return false;
    }
  
    if ($("#txtkouban").val() != "")
    {
        if (IsKoubanAlreadySaved())
        {
            ShowMessageDialog($.trim($("#lblErrorMessageE_KubunAlreadyUse_03").text()), "txtkouban", WARNING);
            return false;
        }
    }
    for (var i = 1; i <= 25; i++) {
        if (parseInt($("#txtMin" + i).val()) > 0 && parseInt($("#txtMax" + i).val()) > 0) {
            if (parseInt($("#txtMin" + i).val()) > parseInt($("#txtMax" + i).val())) {
                ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_007").text()), "txtMin" + i, WARNING);
                return false;
            }
        }
    }

    for (var i = 1; i <= 25; i++) {
        if (parseInt($("#txtHardMin" + i).val()) > 0 && parseInt($("#txtHardMax" + i).val()) > 0) {
            if (parseInt($("#txtHardMin" + i).val()) > parseInt($("#txtHardMax" + i).val())) {
                ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_008").text()), "txtHardMin" + i, WARNING);
                return false;
            }
        }
    }
    UpdateMakerMillSheetDetails();
});
function UpdateMakerMillSheetDetails() {
    var ajaxTime = new Date().getTime();
    var formDataUpdate = new FormData();

    //var fileExist = $("#fileUpload")[0].files[0];

    //if (fileExist && !$("#fileUpload").is(":disabled"))
    //{
    //    var base64 = ReadFile();
       
    //    // Remove prefix if exists
    //    if (base64.indexOf(",") !== -1) {
    //        base64 = base64.split(",")[1];
    //    }

    //    var byteCharacters = atob(base64);
    //    var byteNumbers = new Array(byteCharacters.length);

    //    for (var i = 0; i < byteCharacters.length; i++) {
    //        byteNumbers[i] = byteCharacters.charCodeAt(i);
    //    }

    //    var byteArray = new Uint8Array(byteNumbers);
    //    var blob = new Blob([byteArray], { type: "application/pdf" });

    //    // Create File object
    //    var uFile = new File([blob], "MakerMillsheet.pdf", {
    //        type: "application/pdf"
    //    });

    //    formDataUpdate.append("file", uFile);
    //} // while update PDF is disabled
    
    formDataUpdate.append("MAKER_CD", $("#ddlMakerName").val());
    formDataUpdate.append("MAKER_NAME", $("#ddlMakerName option:selected").text());
    formDataUpdate.append("MAKER_MILL_SHEET_CD", $("#hdfMakerMillSheetCD").val());
    formDataUpdate.append("KOUBAN", $("#txtkouban").val());
    formDataUpdate.append("CHARGE", $("#txtcharge").val());
    formDataUpdate.append("MATL_NAME_MAKER", $("#txtTypeofSteel").val());
    formDataUpdate.append("DIM_D", $("#txtDimension").val());
    formDataUpdate.append("LENGTH", $("#txtLength").val());
    formDataUpdate.append("NO_OF_UNITS", $("#txtNoOfUnits").val());
    formDataUpdate.append("UNIT", $("#ddlUnitType").val());
    formDataUpdate.append("QUANTITY", $("#txtWeightWithoutKG").val());
    formDataUpdate.append("GRAIN_SIZE_STD", $("#txtGainsizeSTD").val());
    formDataUpdate.append("GRAIN_SIZE_ACTUAL", $("#txtGainSizeActual").val());
    formDataUpdate.append("HARDNESS_STD", $("#txtHardnessmakerSTD").val());
    formDataUpdate.append("HARDNESS_MIN", $("#txtHardnessMin").val());
    formDataUpdate.append("HARDNESS_MAX", $("#txtHardnessMax").val());
    formDataUpdate.append("HARDNESS_ACTUAL", $("#txtHardnessMakerActual").val());

        // ---------------------------------------------------------------- Chemical Composition Detail ------------------------------------------------
     formDataUpdate.append("CCNAME1", $("#txtCCName1").val());
     formDataUpdate.append("CCNAME2", $("#txtCCName2").val());
     formDataUpdate.append("CCNAME3", $("#txtCCName3").val());
     formDataUpdate.append("CCNAME4", $("#txtCCName4").val());
     formDataUpdate.append("CCNAME5", $("#txtCCName5").val());
     formDataUpdate.append("CCNAME6", $("#txtCCName6").val());
     formDataUpdate.append("CCNAME7", $("#txtCCName7").val());
     formDataUpdate.append("CCNAME8", $("#txtCCName8").val());
     formDataUpdate.append("CCNAME9", $("#txtCCName9").val());
     formDataUpdate.append("CCNAME10", $("#txtCCName10").val());
     formDataUpdate.append("CCNAME11", $("#txtCCName11").val());
     formDataUpdate.append("CCNAME12", $("#txtCCName12").val());
     formDataUpdate.append("CCNAME13", $("#txtCCName13").val());
     formDataUpdate.append("CCNAME14", $("#txtCCName14").val());
     formDataUpdate.append("CCNAME15", $("#txtCCName15").val());
     formDataUpdate.append("CCNAME16", $("#txtCCName16").val());
     formDataUpdate.append("CCNAME17", $("#txtCCName17").val());
     formDataUpdate.append("CCNAME18", $("#txtCCName18").val());
     formDataUpdate.append("CCNAME19", $("#txtCCName19").val());
     formDataUpdate.append("CCNAME20", $("#txtCCName20").val());
     formDataUpdate.append("CCNAME21", $("#txtCCName21").val());
     formDataUpdate.append("CCNAME22", $("#txtCCName22").val());
     formDataUpdate.append("CCNAME23", $("#txtCCName23").val());
     formDataUpdate.append("CCNAME24", $("#txtCCName24").val());
     formDataUpdate.append("CCNAME25", $("#txtCCName25").val());
     formDataUpdate.append("CCRANGE1", $("#txtCCRange1").val());
     formDataUpdate.append("CCRANGE2", $("#txtCCRange2").val());
     formDataUpdate.append("CCRANGE3", $("#txtCCRange3").val());
     formDataUpdate.append("CCRANGE4", $("#txtCCRange4").val());
     formDataUpdate.append("CCRANGE5", $("#txtCCRange5").val());
     formDataUpdate.append("CCRANGE6", $("#txtCCRange6").val());
     formDataUpdate.append("CCRANGE7", $("#txtCCRange7").val());
     formDataUpdate.append("CCRANGE8", $("#txtCCRange8").val());
     formDataUpdate.append("CCRANGE9", $("#txtCCRange9").val());
     formDataUpdate.append("CCRANGE10", $("#txtCCRange10").val());
     formDataUpdate.append("CCRANGE11", $("#txtCCRange11").val());
     formDataUpdate.append("CCRANGE12", $("#txtCCRange12").val());
     formDataUpdate.append("CCRANGE13", $("#txtCCRange13").val());
     formDataUpdate.append("CCRANGE14", $("#txtCCRange14").val());
     formDataUpdate.append("CCRANGE15", $("#txtCCRange15").val());
     formDataUpdate.append("CCRANGE16", $("#txtCCRange16").val());
     formDataUpdate.append("CCRANGE17", $("#txtCCRange17").val());
     formDataUpdate.append("CCRANGE18", $("#txtCCRange18").val());
     formDataUpdate.append("CCRANGE19", $("#txtCCRange19").val());
     formDataUpdate.append("CCRANGE20", $("#txtCCRange20").val());
     formDataUpdate.append("CCRANGE21", $("#txtCCRange21").val());
     formDataUpdate.append("CCRANGE22", $("#txtCCRange22").val());
     formDataUpdate.append("CCRANGE23", $("#txtCCRange23").val());
     formDataUpdate.append("CCRANGE24", $("#txtCCRange24").val());
     formDataUpdate.append("CCRANGE25", $("#txtCCRange25").val());
     formDataUpdate.append("CCMIN1", $("#txtMin1").val());
     formDataUpdate.append("CCMIN2", $("#txtMin2").val());
     formDataUpdate.append("CCMIN3", $("#txtMin3").val());
     formDataUpdate.append("CCMIN4", $("#txtMin4").val());
     formDataUpdate.append("CCMIN5", $("#txtMin5").val());
     formDataUpdate.append("CCMIN6", $("#txtMin6").val());
     formDataUpdate.append("CCMIN7", $("#txtMin7").val());
     formDataUpdate.append("CCMIN8", $("#txtMin8").val());
     formDataUpdate.append("CCMIN9", $("#txtMin9").val());
     formDataUpdate.append("CCMIN10", $("#txtMin10").val());
     formDataUpdate.append("CCMIN11", $("#txtMin11").val());
     formDataUpdate.append("CCMIN12", $("#txtMin12").val());
     formDataUpdate.append("CCMIN13", $("#txtMin13").val());
     formDataUpdate.append("CCMIN14", $("#txtMin14").val());
     formDataUpdate.append("CCMIN15", $("#txtMin15").val());
     formDataUpdate.append("CCMIN16", $("#txtMin16").val());
     formDataUpdate.append("CCMIN17", $("#txtMin17").val());
     formDataUpdate.append("CCMIN18", $("#txtMin18").val());
     formDataUpdate.append("CCMIN19", $("#txtMin19").val());
     formDataUpdate.append("CCMIN20", $("#txtMin20").val());
     formDataUpdate.append("CCMIN21", $("#txtMin21").val());
     formDataUpdate.append("CCMIN22", $("#txtMin22").val());
     formDataUpdate.append("CCMIN23", $("#txtMin23").val());
     formDataUpdate.append("CCMIN24", $("#txtMin24").val());
     formDataUpdate.append("CCMIN25", $("#txtMin25").val());
     formDataUpdate.append("CCMAX1", $("#txtMax1").val());
     formDataUpdate.append("CCMAX2", $("#txtMax2").val());
     formDataUpdate.append("CCMAX3", $("#txtMax3").val());
     formDataUpdate.append("CCMAX4", $("#txtMax4").val());
     formDataUpdate.append("CCMAX5", $("#txtMax5").val());
     formDataUpdate.append("CCMAX6", $("#txtMax6").val());
     formDataUpdate.append("CCMAX7", $("#txtMax7").val());
     formDataUpdate.append("CCMAX8", $("#txtMax8").val());
     formDataUpdate.append("CCMAX9", $("#txtMax9").val());
     formDataUpdate.append("CCMAX10", $("#txtMax10").val());
     formDataUpdate.append("CCMAX11", $("#txtMax11").val());
     formDataUpdate.append("CCMAX12", $("#txtMax12").val());
     formDataUpdate.append("CCMAX13", $("#txtMax13").val());
     formDataUpdate.append("CCMAX14", $("#txtMax14").val());
     formDataUpdate.append("CCMAX15", $("#txtMax15").val());
     formDataUpdate.append("CCMAX16", $("#txtMax16").val());
     formDataUpdate.append("CCMAX17", $("#txtMax17").val());
     formDataUpdate.append("CCMAX18", $("#txtMax18").val());
     formDataUpdate.append("CCMAX19", $("#txtMax19").val());
     formDataUpdate.append("CCMAX20", $("#txtMax20").val());
     formDataUpdate.append("CCMAX21", $("#txtMax21").val());
     formDataUpdate.append("CCMAX22", $("#txtMax22").val());
     formDataUpdate.append("CCMAX23", $("#txtMax23").val());
     formDataUpdate.append("CCMAX24", $("#txtMax24").val());
     formDataUpdate.append("CCMAX25", $("#txtMax25").val());
     formDataUpdate.append("CCACTUAL1", $("#txtCCActual1").val());
     formDataUpdate.append("CCACTUAL2", $("#txtCCActual2").val());
     formDataUpdate.append("CCACTUAL3", $("#txtCCActual3").val());
     formDataUpdate.append("CCACTUAL4", $("#txtCCActual4").val());
     formDataUpdate.append("CCACTUAL5", $("#txtCCActual5").val());
     formDataUpdate.append("CCACTUAL6", $("#txtCCActual6").val());
     formDataUpdate.append("CCACTUAL7", $("#txtCCActual7").val());
     formDataUpdate.append("CCACTUAL8", $("#txtCCActual8").val());
     formDataUpdate.append("CCACTUAL9", $("#txtCCActual9").val());
     formDataUpdate.append("CCACTUAL10", $("#txtCCActual10").val());
     formDataUpdate.append("CCACTUAL11", $("#txtCCActual11").val());
     formDataUpdate.append("CCACTUAL12", $("#txtCCActual12").val());
     formDataUpdate.append("CCACTUAL13", $("#txtCCActual13").val());
     formDataUpdate.append("CCACTUAL14", $("#txtCCActual14").val());
     formDataUpdate.append("CCACTUAL15", $("#txtCCActual15").val());
     formDataUpdate.append("CCACTUAL16", $("#txtCCActual16").val());
     formDataUpdate.append("CCACTUAL17", $("#txtCCActual17").val());
     formDataUpdate.append("CCACTUAL18", $("#txtCCActual18").val());
     formDataUpdate.append("CCACTUAL19", $("#txtCCActual19").val());
     formDataUpdate.append("CCACTUAL20", $("#txtCCActual20").val());
     formDataUpdate.append("CCACTUAL21", $("#txtCCActual21").val());
     formDataUpdate.append("CCACTUAL22", $("#txtCCActual22").val());
     formDataUpdate.append("CCACTUAL23", $("#txtCCActual23").val());
     formDataUpdate.append("CCACTUAL24", $("#txtCCActual24").val());
     formDataUpdate.append("CCACTUAL25", $("#txtCCActual25").val());

        // ---------------------------------------------------------------- Hardness Detail ------------------------------------------------

    formDataUpdate.append("HARD_STD1", $("#txtHardSTD1").val());
    formDataUpdate.append("HARD_STD2", $("#txtHardSTD2").val());
    formDataUpdate.append("HARD_STD3", $("#txtHardSTD3").val());
    formDataUpdate.append("HARD_STD4", $("#txtHardSTD4").val());
    formDataUpdate.append("HARD_STD5", $("#txtHardSTD5").val());
    formDataUpdate.append("HARD_STD6", $("#txtHardSTD6").val());
    formDataUpdate.append("HARD_STD7", $("#txtHardSTD7").val());
    formDataUpdate.append("HARD_STD8", $("#txtHardSTD8").val());
    formDataUpdate.append("HARD_STD9", $("#txtHardSTD9").val());
    formDataUpdate.append("HARD_STD10", $("#txtHardSTD10").val());
    formDataUpdate.append("HARD_STD11", $("#txtHardSTD11").val());
    formDataUpdate.append("HARD_STD12", $("#txtHardSTD12").val());
    formDataUpdate.append("HARD_STD13", $("#txtHardSTD13").val());
    formDataUpdate.append("HARD_STD14", $("#txtHardSTD14").val());
    formDataUpdate.append("HARD_STD15", $("#txtHardSTD15").val());
    formDataUpdate.append("HARD_STD16", $("#txtHardSTD16").val());
    formDataUpdate.append("HARD_STD17", $("#txtHardSTD17").val());
    formDataUpdate.append("HARD_STD18", $("#txtHardSTD18").val());
    formDataUpdate.append("HARD_STD19", $("#txtHardSTD19").val());
    formDataUpdate.append("HARD_STD20", $("#txtHardSTD20").val());
    formDataUpdate.append("HARD_STD21", $("#txtHardSTD21").val());
    formDataUpdate.append("HARD_STD22", $("#txtHardSTD22").val());
    formDataUpdate.append("HARD_STD23", $("#txtHardSTD23").val());
    formDataUpdate.append("HARD_STD24", $("#txtHardSTD24").val());
    formDataUpdate.append("HARD_STD25", $("#txtHardSTD25").val());
    formDataUpdate.append("HARD_MIN1", $("#txtHardMin1").val());
    formDataUpdate.append("HARD_MIN2", $("#txtHardMin2").val());
    formDataUpdate.append("HARD_MIN3", $("#txtHardMin3").val());
    formDataUpdate.append("HARD_MIN4", $("#txtHardMin4").val());
    formDataUpdate.append("HARD_MIN5", $("#txtHardMin5").val());
    formDataUpdate.append("HARD_MIN6", $("#txtHardMin6").val());
    formDataUpdate.append("HARD_MIN7", $("#txtHardMin7").val());
    formDataUpdate.append("HARD_MIN8", $("#txtHardMin8").val());
    formDataUpdate.append("HARD_MIN9", $("#txtHardMin9").val());
    formDataUpdate.append("HARD_MIN10", $("#txtHardMin10").val());
    formDataUpdate.append("HARD_MIN11", $("#txtHardMin11").val());
    formDataUpdate.append("HARD_MIN12", $("#txtHardMin12").val());
    formDataUpdate.append("HARD_MIN13", $("#txtHardMin13").val());
    formDataUpdate.append("HARD_MIN14", $("#txtHardMin14").val());
    formDataUpdate.append("HARD_MIN15", $("#txtHardMin15").val());
    formDataUpdate.append("HARD_MIN16", $("#txtHardMin16").val());
    formDataUpdate.append("HARD_MIN17", $("#txtHardMin17").val());
    formDataUpdate.append("HARD_MIN18", $("#txtHardMin18").val());
    formDataUpdate.append("HARD_MIN19", $("#txtHardMin19").val());
    formDataUpdate.append("HARD_MIN20", $("#txtHardMin20").val());
    formDataUpdate.append("HARD_MIN21", $("#txtHardMin21").val());
    formDataUpdate.append("HARD_MIN22", $("#txtHardMin22").val());
    formDataUpdate.append("HARD_MIN23", $("#txtHardMin23").val());
    formDataUpdate.append("HARD_MIN24", $("#txtHardMin24").val());
    formDataUpdate.append("HARD_MIN25", $("#txtHardMin25").val());
    formDataUpdate.append("HARD_MAX1", $("#txtHardMax1").val());
    formDataUpdate.append("HARD_MAX2", $("#txtHardMax2").val());
    formDataUpdate.append("HARD_MAX3", $("#txtHardMax3").val());
    formDataUpdate.append("HARD_MAX4", $("#txtHardMax4").val());
    formDataUpdate.append("HARD_MAX5", $("#txtHardMax5").val());
    formDataUpdate.append("HARD_MAX6", $("#txtHardMax6").val());
    formDataUpdate.append("HARD_MAX7", $("#txtHardMax7").val());
    formDataUpdate.append("HARD_MAX8", $("#txtHardMax8").val());
    formDataUpdate.append("HARD_MAX9", $("#txtHardMax9").val());
    formDataUpdate.append("HARD_MAX10", $("#txtHardMax10").val());
    formDataUpdate.append("HARD_MAX11", $("#txtHardMax11").val());
    formDataUpdate.append("HARD_MAX12", $("#txtHardMax12").val());
    formDataUpdate.append("HARD_MAX13", $("#txtHardMax13").val());
    formDataUpdate.append("HARD_MAX14", $("#txtHardMax14").val());
    formDataUpdate.append("HARD_MAX15", $("#txtHardMax15").val());
    formDataUpdate.append("HARD_MAX16", $("#txtHardMax16").val());
    formDataUpdate.append("HARD_MAX17", $("#txtHardMax17").val());
    formDataUpdate.append("HARD_MAX18", $("#txtHardMax18").val());
    formDataUpdate.append("HARD_MAX19", $("#txtHardMax19").val());
    formDataUpdate.append("HARD_MAX20", $("#txtHardMax20").val());
    formDataUpdate.append("HARD_MAX21", $("#txtHardMax21").val());
    formDataUpdate.append("HARD_MAX22", $("#txtHardMax22").val());
    formDataUpdate.append("HARD_MAX23", $("#txtHardMax23").val());
    formDataUpdate.append("HARD_MAX24", $("#txtHardMax24").val());
    formDataUpdate.append("HARD_MAX25", $("#txtHardMax25").val());
    formDataUpdate.append("HARD_ACTUAL1", $("#txtHardActual1").val());
    formDataUpdate.append("HARD_ACTUAL2", $("#txtHardActual2").val());
    formDataUpdate.append("HARD_ACTUAL3", $("#txtHardActual3").val());
    formDataUpdate.append("HARD_ACTUAL4", $("#txtHardActual4").val());
    formDataUpdate.append("HARD_ACTUAL5", $("#txtHardActual5").val());
    formDataUpdate.append("HARD_ACTUAL6", $("#txtHardActual6").val());
    formDataUpdate.append("HARD_ACTUAL7", $("#txtHardActual7").val());
    formDataUpdate.append("HARD_ACTUAL8", $("#txtHardActual8").val());
    formDataUpdate.append("HARD_ACTUAL9", $("#txtHardActual9").val());
    formDataUpdate.append("HARD_ACTUAL10", $("#txtHardActual10").val());
    formDataUpdate.append("HARD_ACTUAL11", $("#txtHardActual11").val());
    formDataUpdate.append("HARD_ACTUAL12", $("#txtHardActual12").val());
    formDataUpdate.append("HARD_ACTUAL13", $("#txtHardActual13").val());
    formDataUpdate.append("HARD_ACTUAL14", $("#txtHardActual14").val());
    formDataUpdate.append("HARD_ACTUAL15", $("#txtHardActual15").val());
    formDataUpdate.append("HARD_ACTUAL16", $("#txtHardActual16").val());
    formDataUpdate.append("HARD_ACTUAL17", $("#txtHardActual17").val());
    formDataUpdate.append("HARD_ACTUAL18", $("#txtHardActual18").val());
    formDataUpdate.append("HARD_ACTUAL19", $("#txtHardActual19").val());
    formDataUpdate.append("HARD_ACTUAL20", $("#txtHardActual20").val());
    formDataUpdate.append("HARD_ACTUAL21", $("#txtHardActual21").val());
    formDataUpdate.append("HARD_ACTUAL22", $("#txtHardActual22").val());
    formDataUpdate.append("HARD_ACTUAL23", $("#txtHardActual23").val());
    formDataUpdate.append("HARD_ACTUAL24", $("#txtHardActual24").val());
    formDataUpdate.append("HARD_ACTUAL25", $("#txtHardActual25").val());

    var settings = {
        async: false,
        crossDomain: true,
        processData: false,       // important for FormData
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/MakerMillSheetDetailsUpdate",
        method: "POST",
        contentType: false, //fileExist ? false : "application/json; charset=utf-8",
       // contentType: "application/json; charset=utf-8",
        data: formDataUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var statusCode = response.message;
            if (statusCode == 0) {
               var message = $("#lblSuccessMessageE_MAKERMILLSHEET_ENTRY_EDIT_DELETE_002").text().trim();
                ShowMessageDialog(message, "btnInitialize", SUCCESS);
            } 
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
$("#btnDelete").click(function () {
   
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/DeleteMakerMillSheetDetails?MakerMillSheetCD=" + $("#hdfMakerMillSheetCD").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.message == 0) {
                DisableEditDeleteButton();
                var message = $("#lblSuccessMessageE_MAKERMILLSHEET_ENTRY_EDIT_DELETE_003").text().trim();
                ShowMessageDialog(message, "btnInitialize", SUCCESS);
            } else if (response.message == 1)
            {
                var message = $("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_011").text().trim();
                ShowMessageDialog(message, "btnInitialize", WARNING);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
});
function ReadPdfData() {
    $("#LoadingImage").show();
    var pageNo = $("#page-indicator").text().split('/')[0];
    var inputfiles = document.getElementById("fileUpload");
    const file = inputfiles.files[0];

    var ajaxTime = new Date().getTime();
    var settings = {
        crossDomain: true,
        cache: false,
        processData: false,       // important for FormData
       // contentType: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/GetMillSheetPDFDetails?makerCD=" + $("#ddlMakerName").val() + "&templateName=" + $("#ddlTemplateName option:selected").text(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
   return $.ajax(settings)
        .then(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var result = response.message;
            if (result.length > 0) {
                var frmData = new FormData();
                frmData.append('file', file);
                frmData.append('details', JSON.stringify(result));
                frmData.append('pageNo', pageNo);
                var settings = {
                    crossDomain: true,
                    cache: false,
                    processData: false,       // important for FormData
                    dataType: "json",
                    url: "/MakerMillSheet/GetPDFData",
                    method: "POST",
                    contentType: false,
                    data: frmData
                }
                return $.ajax(settings)
                    .then(function (response) {
                        responseTime = new Date().getTime() - ajaxTime;
                        

                        var result = response.message;

                        if (result != null) {
                            arrayData = result;
                            setMillSheetDetails(0, result);
                            $("#LoadingImage").hide();
                        }

                    })
                    
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            $("#LoadingImage").hide();
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function setMillSheetDetails(index, result) {
    
    if (result != null) {
        $("#txtTypeofSteel").val(result[index].makeR_DATA.matL_NAME_MAKER);
        $("#txtcharge").val(result[index].makeR_DATA.charge);
        if (result[index].makeR_DATA.diM_D > 0) {
            $("#txtDimension").val(result[index].makeR_DATA.diM_D);
        }
        if (result[index].makeR_DATA.nO_OF_UNITS > 0) {
            $("#txtNoOfUnits").val(result[index].makeR_DATA.nO_OF_UNITS);
        }
        
        if (result[index].makeR_DATA.quantity > 0) {
            $("#txtWeightWithoutKG").val(result[index].makeR_DATA.quantity);
        }


        $("#txtCCName1").val(result[index].makeR_DATA.ccnamE1);
        $("#txtCCName2").val(result[index].makeR_DATA.ccnamE2);
        $("#txtCCName3").val(result[index].makeR_DATA.ccnamE3);
        $("#txtCCName4").val(result[index].makeR_DATA.ccnamE4);
        $("#txtCCName5").val(result[index].makeR_DATA.ccnamE5);
        $("#txtCCName6").val(result[index].makeR_DATA.ccnamE6);
        $("#txtCCName7").val(result[index].makeR_DATA.ccnamE7);
        $("#txtCCName8").val(result[index].makeR_DATA.ccnamE8);
        $("#txtCCName9").val(result[index].makeR_DATA.ccnamE9);
        $("#txtCCName10").val(result[index].makeR_DATA.ccnamE10);
        $("#txtCCName11").val(result[index].makeR_DATA.ccnamE11);
        $("#txtCCName12").val(result[index].makeR_DATA.ccnamE12);
        $("#txtCCName13").val(result[index].makeR_DATA.ccnamE13);
        $("#txtCCName14").val(result[index].makeR_DATA.ccnamE14);
        $("#txtCCName15").val(result[index].makeR_DATA.ccnamE15);
        $("#txtCCName16").val(result[index].makeR_DATA.ccnamE16);
        $("#txtCCName17").val(result[index].makeR_DATA.ccnamE17);
        $("#txtCCName18").val(result[index].makeR_DATA.ccnamE18);
        $("#txtCCName19").val(result[index].makeR_DATA.ccnamE19);
        $("#txtCCName20").val(result[index].makeR_DATA.ccnamE20);
        $("#txtCCName21").val(result[index].makeR_DATA.ccnamE21);
        $("#txtCCName22").val(result[index].makeR_DATA.ccnamE22);
        $("#txtCCName23").val(result[index].makeR_DATA.ccnamE23);
        $("#txtCCName24").val(result[index].makeR_DATA.ccnamE24);
        $("#txtCCName25").val(result[index].makeR_DATA.ccnamE25);
        
        if (result[index].makeR_DATA.ccnamE1 == null)
        {
            $("#ddltd1").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE2 == null) {
            $("#ddltd2").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE3 == null) {
            $("#ddltd3").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE4 == null) {
            $("#ddltd4").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE5 == null) {
            $("#ddltd5").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE6 == null) {
            $("#ddltd6").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE7 == null) {
            $("#ddltd7").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE8 == null) {
            $("#ddltd8").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE9 == null) {
            $("#ddltd9").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE10 == null) {
            $("#ddltd10").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE11 == null) {
            $("#ddltd11").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE12 == null) {
            $("#ddltd12").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE13 == null) {
            $("#ddltd13").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE14 == null) {
            $("#ddltd14").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE15 == null) {
            $("#ddltd15").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE16 == null) {
            $("#ddltd16").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE17 == null) {
            $("#ddltd17").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE18 == null) {
            $("#ddltd18").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE19 == null) {
            $("#ddltd19").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE20 == null) {
            $("#ddltd20").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE21 == null) {
            $("#ddltd21").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE22 == null) {
            $("#ddltd22").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE23 == null) {
            $("#ddltd23").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE24 == null) {
            $("#ddltd24").prop("selectedIndex", -1);
        }
        if (result[index].makeR_DATA.ccnamE25 == null) {
            $("#ddltd25").prop("selectedIndex", -1);
        }

        $("#txtCCRange1").val(result[index].makeR_DATA.cC_CHEMICAL1_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL1_STRING : null);
        $("#txtCCRange2").val(result[index].makeR_DATA.cC_CHEMICAL2_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL2_STRING : null);
        $("#txtCCRange3").val(result[index].makeR_DATA.cC_CHEMICAL3_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL3_STRING : null);
        $("#txtCCRange4").val(result[index].makeR_DATA.cC_CHEMICAL4_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL4_STRING : null);
        $("#txtCCRange5").val(result[index].makeR_DATA.cC_CHEMICAL5_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL5_STRING : null);
        $("#txtCCRange6").val(result[index].makeR_DATA.cC_CHEMICAL6_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL6_STRING : null);
        $("#txtCCRange7").val(result[index].makeR_DATA.cC_CHEMICAL7_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL7_STRING : null);
        $("#txtCCRange8").val(result[index].makeR_DATA.cC_CHEMICAL8_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL8_STRING : null);
        $("#txtCCRange9").val(result[index].makeR_DATA.cC_CHEMICAL9_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL9_STRING : null);
        $("#txtCCRange10").val(result[index].makeR_DATA.cC_CHEMICAL10_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL10_STRING : null);
        $("#txtCCRange11").val(result[index].makeR_DATA.cC_CHEMICAL11_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL11_STRING : null);
        $("#txtCCRange12").val(result[index].makeR_DATA.cC_CHEMICAL12_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL12_STRING : null);
        $("#txtCCRange13").val(result[index].makeR_DATA.cC_CHEMICAL13_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL13_STRING : null);
        $("#txtCCRange14").val(result[index].makeR_DATA.cC_CHEMICAL14_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL14_STRING : null);
        $("#txtCCRange15").val(result[index].makeR_DATA.cC_CHEMICAL15_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL15_STRING : null);
        $("#txtCCRange16").val(result[index].makeR_DATA.cC_CHEMICAL16_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL16_STRING : null);
        $("#txtCCRange17").val(result[index].makeR_DATA.cC_CHEMICAL17_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL17_STRING : null);
        $("#txtCCRange18").val(result[index].makeR_DATA.cC_CHEMICAL18_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL18_STRING : null);
        $("#txtCCRange19").val(result[index].makeR_DATA.cC_CHEMICAL19_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL19_STRING : null);
        $("#txtCCRange20").val(result[index].makeR_DATA.cC_CHEMICAL20_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL20_STRING : null);
        $("#txtCCRange21").val(result[index].makeR_DATA.cC_CHEMICAL21_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL21_STRING : null);
        $("#txtCCRange22").val(result[index].makeR_DATA.cC_CHEMICAL22_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL22_STRING : null);
        $("#txtCCRange23").val(result[index].makeR_DATA.cC_CHEMICAL23_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL23_STRING : null);
        $("#txtCCRange24").val(result[index].makeR_DATA.cC_CHEMICAL24_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL24_STRING : null);
        $("#txtCCRange25").val(result[index].makeR_DATA.cC_CHEMICAL25_STRING > 0 ? result[index].makeR_DATA.cC_CHEMICAL25_STRING : null);


        $("#txtMin1").val(result[index].makeR_DATA.ccmiN1_STRING);
        $("#txtMin2").val(result[index].makeR_DATA.ccmiN2_STRING);
        $("#txtMin3").val(result[index].makeR_DATA.ccmiN3_STRING);
        $("#txtMin4").val(result[index].makeR_DATA.ccmiN4_STRING);
        $("#txtMin5").val(result[index].makeR_DATA.ccmiN5_STRING);
        $("#txtMin6").val(result[index].makeR_DATA.ccmiN6_STRING);
        $("#txtMin7").val(result[index].makeR_DATA.ccmiN7_STRING);
        $("#txtMin8").val(result[index].makeR_DATA.ccmiN8_STRING);
        $("#txtMin9").val(result[index].makeR_DATA.ccmiN9_STRING);
        $("#txtMin10").val(result[index].makeR_DATA.ccmiN10_STRING);
        $("#txtMin11").val(result[index].makeR_DATA.ccmiN11_STRING);
        $("#txtMin12").val(result[index].makeR_DATA.ccmiN12_STRING);
        $("#txtMin13").val(result[index].makeR_DATA.ccmiN13_STRING);
        $("#txtMin14").val(result[index].makeR_DATA.ccmiN14_STRING);
        $("#txtMin15").val(result[index].makeR_DATA.ccmiN15_STRING);
        $("#txtMin16").val(result[index].makeR_DATA.ccmiN16_STRING);
        $("#txtMin17").val(result[index].makeR_DATA.ccmiN17_STRING);
        $("#txtMin18").val(result[index].makeR_DATA.ccmiN18_STRING);
        $("#txtMin19").val(result[index].makeR_DATA.ccmiN19_STRING);
        $("#txtMin20").val(result[index].makeR_DATA.ccmiN20_STRING);
        $("#txtMin21").val(result[index].makeR_DATA.ccmiN21_STRING);
        $("#txtMin22").val(result[index].makeR_DATA.ccmiN22_STRING);
        $("#txtMin23").val(result[index].makeR_DATA.ccmiN23_STRING);
        $("#txtMin24").val(result[index].makeR_DATA.ccmiN24_STRING);
        $("#txtMin25").val(result[index].makeR_DATA.ccmiN25_STRING);


        $("#txtMax1").val(result[index].makeR_DATA.ccmaX1_STRING);
        $("#txtMax2").val(result[index].makeR_DATA.ccmaX2_STRING);
        $("#txtMax3").val(result[index].makeR_DATA.ccmaX3_STRING);
        $("#txtMax4").val(result[index].makeR_DATA.ccmaX4_STRING);
        $("#txtMax5").val(result[index].makeR_DATA.ccmaX5_STRING);
        $("#txtMax6").val(result[index].makeR_DATA.ccmaX6_STRING);
        $("#txtMax7").val(result[index].makeR_DATA.ccmaX7_STRING);
        $("#txtMax8").val(result[index].makeR_DATA.ccmaX8_STRING);
        $("#txtMax9").val(result[index].makeR_DATA.ccmaX9_STRING);
        $("#txtMax10").val(result[index].makeR_DATA.ccmaX10_STRING);
        $("#txtMax11").val(result[index].makeR_DATA.ccmaX11_STRING);
        $("#txtMax12").val(result[index].makeR_DATA.ccmaX12_STRING);
        $("#txtMax13").val(result[index].makeR_DATA.ccmaX13_STRING);
        $("#txtMax14").val(result[index].makeR_DATA.ccmaX14_STRING);
        $("#txtMax15").val(result[index].makeR_DATA.ccmaX15_STRING);
        $("#txtMax16").val(result[index].makeR_DATA.ccmaX16_STRING);
        $("#txtMax17").val(result[index].makeR_DATA.ccmaX17_STRING);
        $("#txtMax18").val(result[index].makeR_DATA.ccmaX18_STRING);
        $("#txtMax19").val(result[index].makeR_DATA.ccmaX19_STRING);
        $("#txtMax20").val(result[index].makeR_DATA.ccmaX20_STRING);
        $("#txtMax21").val(result[index].makeR_DATA.ccmaX21_STRING);
        $("#txtMax22").val(result[index].makeR_DATA.ccmaX22_STRING);
        $("#txtMax23").val(result[index].makeR_DATA.ccmaX23_STRING);
        $("#txtMax24").val(result[index].makeR_DATA.ccmaX24_STRING);
        $("#txtMax25").val(result[index].makeR_DATA.ccmaX25_STRING);


        $("#txtCCActual1").val(result[index].makeR_DATA.ccactuaL1 > 0 ? result[index].makeR_DATA.ccactuaL1 : null);
        $("#txtCCActual2").val(result[index].makeR_DATA.ccactuaL2 > 0 ? result[index].makeR_DATA.ccactuaL2 : null);
        $("#txtCCActual3").val(result[index].makeR_DATA.ccactuaL3 > 0 ? result[index].makeR_DATA.ccactuaL3 : null);
        $("#txtCCActual4").val(result[index].makeR_DATA.ccactuaL4 > 0 ? result[index].makeR_DATA.ccactuaL4 : null);
        $("#txtCCActual5").val(result[index].makeR_DATA.ccactuaL5 > 0 ? result[index].makeR_DATA.ccactuaL5 : null);
        $("#txtCCActual6").val(result[index].makeR_DATA.ccactuaL6 > 0 ? result[index].makeR_DATA.ccactuaL6 : null);
        $("#txtCCActual7").val(result[index].makeR_DATA.ccactuaL7 > 0 ? result[index].makeR_DATA.ccactuaL7 : null);
        $("#txtCCActual8").val(result[index].makeR_DATA.ccactuaL8 > 0 ? result[index].makeR_DATA.ccactuaL8 : null);
        $("#txtCCActual9").val(result[index].makeR_DATA.ccactuaL9 > 0 ? result[index].makeR_DATA.ccactuaL9 : null);
        $("#txtCCActual10").val(result[index].makeR_DATA.ccactuaL10 > 0 ? result[index].makeR_DATA.ccactuaL10 : null);
        $("#txtCCActual11").val(result[index].makeR_DATA.ccactuaL11 > 0 ? result[index].makeR_DATA.ccactuaL11 : null);
        $("#txtCCActual12").val(result[index].makeR_DATA.ccactuaL12 > 0 ? result[index].makeR_DATA.ccactuaL12 : null);
        $("#txtCCActual13").val(result[index].makeR_DATA.ccactuaL13 > 0 ? result[index].makeR_DATA.ccactuaL13 : null);
        $("#txtCCActual14").val(result[index].makeR_DATA.ccactuaL14 > 0 ? result[index].makeR_DATA.ccactuaL14 : null);
        $("#txtCCActual15").val(result[index].makeR_DATA.ccactuaL15 > 0 ? result[index].makeR_DATA.ccactuaL15 : null);
        $("#txtCCActual16").val(result[index].makeR_DATA.ccactuaL16 > 0 ? result[index].makeR_DATA.ccactuaL16 : null);
        $("#txtCCActual17").val(result[index].makeR_DATA.ccactuaL17 > 0 ? result[index].makeR_DATA.ccactuaL17 : null);
        $("#txtCCActual18").val(result[index].makeR_DATA.ccactuaL18 > 0 ? result[index].makeR_DATA.ccactuaL18 : null);
        $("#txtCCActual19").val(result[index].makeR_DATA.ccactuaL19 > 0 ? result[index].makeR_DATA.ccactuaL19 : null);
        $("#txtCCActual20").val(result[index].makeR_DATA.ccactuaL20 > 0 ? result[index].makeR_DATA.ccactuaL20 : null);
        $("#txtCCActual21").val(result[index].makeR_DATA.ccactuaL21 > 0 ? result[index].makeR_DATA.ccactuaL21 : null);
        $("#txtCCActual22").val(result[index].makeR_DATA.ccactuaL22 > 0 ? result[index].makeR_DATA.ccactuaL22 : null);
        $("#txtCCActual23").val(result[index].makeR_DATA.ccactuaL23 > 0 ? result[index].makeR_DATA.ccactuaL23 : null);
        $("#txtCCActual24").val(result[index].makeR_DATA.ccactuaL24 > 0 ? result[index].makeR_DATA.ccactuaL24 : null);
        $("#txtCCActual25").val(result[index].makeR_DATA.ccactuaL25 > 0 ? result[index].makeR_DATA.ccactuaL25 : null);

        $("#txtHardSTD1").val(result[index].makeR_DATA.harD_POSITION1_STRING > 0 ? result[index].makeR_DATA.harD_POSITION1_STRING : null);
        $("#txtHardSTD2").val(result[index].makeR_DATA.harD_POSITION2_STRING > 0 ? result[index].makeR_DATA.harD_POSITION2_STRING : null);
        $("#txtHardSTD3").val(result[index].makeR_DATA.harD_POSITION3_STRING > 0 ? result[index].makeR_DATA.harD_POSITION3_STRING : null);
        $("#txtHardSTD4").val(result[index].makeR_DATA.harD_POSITION4_STRING > 0 ? result[index].makeR_DATA.harD_POSITION4_STRING : null);
        $("#txtHardSTD5").val(result[index].makeR_DATA.harD_POSITION5_STRING > 0 ? result[index].makeR_DATA.harD_POSITION5_STRING : null);
        $("#txtHardSTD6").val(result[index].makeR_DATA.harD_POSITION6_STRING > 0 ? result[index].makeR_DATA.harD_POSITION6_STRING : null);
        $("#txtHardSTD7").val(result[index].makeR_DATA.harD_POSITION7_STRING > 0 ? result[index].makeR_DATA.harD_POSITION7_STRING : null);
        $("#txtHardSTD8").val(result[index].makeR_DATA.harD_POSITION8_STRING > 0 ? result[index].makeR_DATA.harD_POSITION8_STRING : null);
        $("#txtHardSTD9").val(result[index].makeR_DATA.harD_POSITION9_STRING > 0 ? result[index].makeR_DATA.harD_POSITION9_STRING : null);
        $("#txtHardSTD10").val(result[index].makeR_DATA.harD_POSITION10_STRING > 0 ? result[index].makeR_DATA.harD_POSITION10_STRING : null);
        $("#txtHardSTD11").val(result[index].makeR_DATA.harD_POSITION11_STRING > 0 ? result[index].makeR_DATA.harD_POSITION11_STRING : null);
        $("#txtHardSTD12").val(result[index].makeR_DATA.harD_POSITION12_STRING > 0 ? result[index].makeR_DATA.harD_POSITION12_STRING : null);
        $("#txtHardSTD13").val(result[index].makeR_DATA.harD_POSITION13_STRING > 0 ? result[index].makeR_DATA.harD_POSITION13_STRING : null);
        $("#txtHardSTD14").val(result[index].makeR_DATA.harD_POSITION14_STRING > 0 ? result[index].makeR_DATA.harD_POSITION14_STRING : null);
        $("#txtHardSTD15").val(result[index].makeR_DATA.harD_POSITION15_STRING > 0 ? result[index].makeR_DATA.harD_POSITION15_STRING : null);
        $("#txtHardSTD16").val(result[index].makeR_DATA.harD_POSITION16_STRING > 0 ? result[index].makeR_DATA.harD_POSITION16_STRING : null);
        $("#txtHardSTD17").val(result[index].makeR_DATA.harD_POSITION17_STRING > 0 ? result[index].makeR_DATA.harD_POSITION17_STRING : null);
        $("#txtHardSTD18").val(result[index].makeR_DATA.harD_POSITION18_STRING > 0 ? result[index].makeR_DATA.harD_POSITION18_STRING : null);
        $("#txtHardSTD19").val(result[index].makeR_DATA.harD_POSITION19_STRING > 0 ? result[index].makeR_DATA.harD_POSITION19_STRING : null);
        $("#txtHardSTD20").val(result[index].makeR_DATA.harD_POSITION20_STRING > 0 ? result[index].makeR_DATA.harD_POSITION20_STRING : null);
        $("#txtHardSTD21").val(result[index].makeR_DATA.harD_POSITION21_STRING > 0 ? result[index].makeR_DATA.harD_POSITION21_STRING : null);
        $("#txtHardSTD22").val(result[index].makeR_DATA.harD_POSITION22_STRING > 0 ? result[index].makeR_DATA.harD_POSITION22_STRING : null);
        $("#txtHardSTD23").val(result[index].makeR_DATA.harD_POSITION23_STRING > 0 ? result[index].makeR_DATA.harD_POSITION23_STRING : null);
        $("#txtHardSTD24").val(result[index].makeR_DATA.harD_POSITION24_STRING > 0 ? result[index].makeR_DATA.harD_POSITION24_STRING : null);
        $("#txtHardSTD25").val(result[index].makeR_DATA.harD_POSITION25_STRING > 0 ? result[index].makeR_DATA.harD_POSITION25_STRING : null);

        $("#txtHardMin1").val(result[index].makeR_DATA.harD_MIN1_STRING);
        $("#txtHardMin2").val(result[index].makeR_DATA.harD_MIN2_STRING);
        $("#txtHardMin3").val(result[index].makeR_DATA.harD_MIN3_STRING);
        $("#txtHardMin4").val(result[index].makeR_DATA.harD_MIN4_STRING);
        $("#txtHardMin5").val(result[index].makeR_DATA.harD_MIN5_STRING);
        $("#txtHardMin6").val(result[index].makeR_DATA.harD_MIN6_STRING);
        $("#txtHardMin7").val(result[index].makeR_DATA.harD_MIN7_STRING);
        $("#txtHardMin8").val(result[index].makeR_DATA.harD_MIN8_STRING);
        $("#txtHardMin9").val(result[index].makeR_DATA.harD_MIN9_STRING);
        $("#txtHardMin10").val(result[index].makeR_DATA.harD_MIN10_STRING);
        $("#txtHardMin11").val(result[index].makeR_DATA.harD_MIN11_STRING);
        $("#txtHardMin12").val(result[index].makeR_DATA.harD_MIN12_STRING);
        $("#txtHardMin13").val(result[index].makeR_DATA.harD_MIN13_STRING);
        $("#txtHardMin14").val(result[index].makeR_DATA.harD_MIN14_STRING);
        $("#txtHardMin15").val(result[index].makeR_DATA.harD_MIN15_STRING);
        $("#txtHardMin16").val(result[index].makeR_DATA.harD_MIN16_STRING);
        $("#txtHardMin17").val(result[index].makeR_DATA.harD_MIN17_STRING);
        $("#txtHardMin18").val(result[index].makeR_DATA.harD_MIN18_STRING);
        $("#txtHardMin19").val(result[index].makeR_DATA.harD_MIN19_STRING);
        $("#txtHardMin20").val(result[index].makeR_DATA.harD_MIN20_STRING);
        $("#txtHardMin21").val(result[index].makeR_DATA.harD_MIN21_STRING);
        $("#txtHardMin22").val(result[index].makeR_DATA.harD_MIN22_STRING);
        $("#txtHardMin23").val(result[index].makeR_DATA.harD_MIN23_STRING);
        $("#txtHardMin24").val(result[index].makeR_DATA.harD_MIN24_STRING);
        $("#txtHardMin25").val(result[index].makeR_DATA.harD_MIN25_STRING);

        $("#txtHardMax1").val(result[index].makeR_DATA.harD_MAX1_STRING);
        $("#txtHardMax2").val(result[index].makeR_DATA.harD_MAX2_STRING);
        $("#txtHardMax3").val(result[index].makeR_DATA.harD_MAX3_STRING);
        $("#txtHardMax4").val(result[index].makeR_DATA.harD_MAX4_STRING);
        $("#txtHardMax5").val(result[index].makeR_DATA.harD_MAX5_STRING);
        $("#txtHardMax6").val(result[index].makeR_DATA.harD_MAX6_STRING);
        $("#txtHardMax7").val(result[index].makeR_DATA.harD_MAX7_STRING);
        $("#txtHardMax8").val(result[index].makeR_DATA.harD_MAX8_STRING);
        $("#txtHardMax9").val(result[index].makeR_DATA.harD_MAX9_STRING);
        $("#txtHardMax10").val(result[index].makeR_DATA.harD_MAX10_STRING);
        $("#txtHardMax11").val(result[index].makeR_DATA.harD_MAX11_STRING);
        $("#txtHardMax12").val(result[index].makeR_DATA.harD_MAX12_STRING);
        $("#txtHardMax13").val(result[index].makeR_DATA.harD_MAX13_STRING);
        $("#txtHardMax14").val(result[index].makeR_DATA.harD_MAX14_STRING);
        $("#txtHardMax15").val(result[index].makeR_DATA.harD_MAX15_STRING);
        $("#txtHardMax16").val(result[index].makeR_DATA.harD_MAX16_STRING);
        $("#txtHardMax17").val(result[index].makeR_DATA.harD_MAX17_STRING);
        $("#txtHardMax18").val(result[index].makeR_DATA.harD_MAX18_STRING);
        $("#txtHardMax19").val(result[index].makeR_DATA.harD_MAX19_STRING);
        $("#txtHardMax20").val(result[index].makeR_DATA.harD_MAX20_STRING);
        $("#txtHardMax21").val(result[index].makeR_DATA.harD_MAX21_STRING);
        $("#txtHardMax22").val(result[index].makeR_DATA.harD_MAX22_STRING);
        $("#txtHardMax23").val(result[index].makeR_DATA.harD_MAX23_STRING);
        $("#txtHardMax24").val(result[index].makeR_DATA.harD_MAX24_STRING);
        $("#txtHardMax25").val(result[index].makeR_DATA.harD_MAX25_STRING);


        $("#txtHardActual1").val(result[index].makeR_DATA.harD_ACTUAL1_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL1_STRING : null);
        $("#txtHardActual2").val(result[index].makeR_DATA.harD_ACTUAL2_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL2_STRING : null);
        $("#txtHardActual3").val(result[index].makeR_DATA.harD_ACTUAL3_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL3_STRING : null);
        $("#txtHardActual4").val(result[index].makeR_DATA.harD_ACTUAL4_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL4_STRING : null);
        $("#txtHardActual5").val(result[index].makeR_DATA.harD_ACTUAL5_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL5_STRING : null);
        $("#txtHardActual6").val(result[index].makeR_DATA.harD_ACTUAL6_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL6_STRING : null);
        $("#txtHardActual7").val(result[index].makeR_DATA.harD_ACTUAL7_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL7_STRING : null);
        $("#txtHardActual8").val(result[index].makeR_DATA.harD_ACTUAL8_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL8_STRING : null);
        $("#txtHardActual9").val(result[index].makeR_DATA.harD_ACTUAL9_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL9_STRING : null);
        $("#txtHardActual10").val(result[index].makeR_DATA.harD_ACTUAL10_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL10_STRING : null);
        $("#txtHardActual11").val(result[index].makeR_DATA.harD_ACTUAL11_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL11_STRING : null);
        $("#txtHardActual12").val(result[index].makeR_DATA.harD_ACTUAL12_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL12_STRING : null);
        $("#txtHardActual13").val(result[index].makeR_DATA.harD_ACTUAL13_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL13_STRING : null);
        $("#txtHardActual14").val(result[index].makeR_DATA.harD_ACTUAL14_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL14_STRING : null);
        $("#txtHardActual15").val(result[index].makeR_DATA.harD_ACTUAL15_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL15_STRING : null);
        $("#txtHardActual16").val(result[index].makeR_DATA.harD_ACTUAL16_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL16_STRING : null);
        $("#txtHardActual17").val(result[index].makeR_DATA.harD_ACTUAL17_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL17_STRING : null);
        $("#txtHardActual18").val(result[index].makeR_DATA.harD_ACTUAL18_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL18_STRING : null);
        $("#txtHardActual19").val(result[index].makeR_DATA.harD_ACTUAL19_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL19_STRING : null);
        $("#txtHardActual20").val(result[index].makeR_DATA.harD_ACTUAL20_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL20_STRING : null);
        $("#txtHardActual21").val(result[index].makeR_DATA.harD_ACTUAL21_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL21_STRING : null);
        $("#txtHardActual22").val(result[index].makeR_DATA.harD_ACTUAL22_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL22_STRING : null);
        $("#txtHardActual23").val(result[index].makeR_DATA.harD_ACTUAL23_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL23_STRING : null);
        $("#txtHardActual24").val(result[index].makeR_DATA.harD_ACTUAL24_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL24_STRING : null);
        $("#txtHardActual25").val(result[index].makeR_DATA.harD_ACTUAL25_STRING > 0 ? result[index].makeR_DATA.harD_ACTUAL25_STRING : null);

    }
}



$("#btnEDIUpdate").click(function () {

    var fileExist = $("#fileUpload")[0].files[0];

    if (!fileExist) {
        ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_010").text()), "", WARNING);
        return false;
    }

    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
   
    
    var ftpDetails = IsFTPServerActive();
    if (ftpDetails.length == 0) {
        return false;
    }
    if (ftpDetails.IsSuccess == false) {
        ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_011").text().replace("NNN", ftpDetails.IpAddress)), "", WARNING);
        return false;

    }
    runLogCD = GetLogRunCD();
    console.log(runLogCD);
    BindFormDataForSave();

    var saveData = tempMillsheetData.filter(x => x.IS_CONFIRMED == 1 && x.ALREADY_SAVED == 'NO'); // all checkbox checked and checkbox not disabled
    if (saveData.length != totalPageCount) {
        ShowMessageDialog($.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_012").text()), "chkConfirmPage", WARNING);
        return false;

    }

    UpdateEDIData();
        
});



function UpdateEDIData() {
   
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        processData: false,       // important for FormData
        contentType: false, //fileExist ? false : "application/json; charset=utf-8",
        dataType: "json",
        url: APIURL + "/MakerMillSheet/UpdateEDIData",
        method: "POST",
        data: formData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess == 0) // 0 for successful save
            {
               
               
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/sucess.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $("#lblSuccessMessageE_MAKERMILLSHEET_ENTRY_EDIT_DELETE_002").text() + "</td></tr></table>",
                    buttons: {
                        ok: {
                            label: 'OK',
                            className: 'btn-primary'
                        }
                    },
                    centerVertical: true,
                    callback: function () {
                        if (response.message == true)
                        {
                            window.open(REPORTURL + "/Report/MillsheetEDIErrorReport.aspx?runCD=" + runLogCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1900 + ", height=" + 1490);
                        }
                        
                    }
                });
            } 

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });

}

function FillTempMillsheetData()
{
    var alreadysaved = 'NO'
    if ($("#chkConfirmPage").hasClass(DISABLED)) {
        alreadysaved = 'YES'
    }
    var millsheetData =
        ({
            ALREADY_SAVED: alreadysaved,
            IS_CONFIRMED: ($("#chkConfirmPage").is(":checked") ? 1 : 0),
            PAGE_NO: currentPage,
            FORM_FILE: null,
            CULTURE_NAME: $("#hdfCultureName").val(),
            MAKER_CD: $("#ddlMakerName").val(),
            MAKER_NAME: $("#ddlMakerName option:selected").text(),
            RUN_CD: runLogCD,
            KOUBAN: $("#txtkouban").val(),
            CHARGE: $("#txtcharge").val(),
            MATL_NAME_MAKER: $("#txtTypeofSteel").val(),
            DIM_D: $("#txtDimension").val(),
            LENGTH: $("#txtLength").val(),
            NO_OF_UNITS: $("#txtNoOfUnits").val(),
            UNIT: $("#ddlUnitType").val(),
            QUANTITY: $("#txtWeightWithoutKG").val(),
            GRAIN_SIZE_STD: $("#txtGainsizeSTD").val(),
            GRAIN_SIZE_ACTUAL: $("#txtGainSizeActual").val(),
            HARDNESS_STD: $("#txtHardnessmakerSTD").val(),
            HARDNESS_MIN: $("#txtHardnessMin").val(),
            HARDNESS_MAX: $("#txtHardnessMax").val(),
            HARDNESS_ACTUAL: $("#txtHardnessMakerActual").val(),
            CCNAME1: $("#txtCCName1").val(),
            CCNAME2: $("#txtCCName2").val(),
            CCNAME3: $("#txtCCName3").val(),
            CCNAME4: $("#txtCCName4").val(),
            CCNAME5: $("#txtCCName5").val(),
            CCNAME6: $("#txtCCName6").val(),
            CCNAME7: $("#txtCCName7").val(),
            CCNAME8: $("#txtCCName8").val(),
            CCNAME9: $("#txtCCName9").val(),
            CCNAME10: $("#txtCCName10").val(),
            CCNAME11: $("#txtCCName11").val(),
            CCNAME12: $("#txtCCName12").val(),
            CCNAME13: $("#txtCCName13").val(),
            CCNAME14: $("#txtCCName14").val(),
            CCNAME15: $("#txtCCName15").val(),
            CCNAME16: $("#txtCCName16").val(),
            CCNAME17: $("#txtCCName17").val(),
            CCNAME18: $("#txtCCName18").val(),
            CCNAME19: $("#txtCCName19").val(),
            CCNAME20: $("#txtCCName20").val(),
            CCNAME21: $("#txtCCName21").val(),
            CCNAME22: $("#txtCCName22").val(),
            CCNAME23: $("#txtCCName23").val(),
            CCNAME24: $("#txtCCName24").val(),
            CCNAME25: $("#txtCCName25").val(),
            CCRANGE1: $("#txtCCRange1").val(),
            CCRANGE2: $("#txtCCRange2").val(),
            CCRANGE3: $("#txtCCRange3").val(),
            CCRANGE4: $("#txtCCRange4").val(),
            CCRANGE5: $("#txtCCRange5").val(),
            CCRANGE6: $("#txtCCRange6").val(),
            CCRANGE7: $("#txtCCRange7").val(),
            CCRANGE8: $("#txtCCRange8").val(),
            CCRANGE9: $("#txtCCRange9").val(),
            CCRANGE10: $("#txtCCRange10").val(),
            CCRANGE11: $("#txtCCRange11").val(),
            CCRANGE12: $("#txtCCRange12").val(),
            CCRANGE13: $("#txtCCRange13").val(),
            CCRANGE14: $("#txtCCRange14").val(),
            CCRANGE15: $("#txtCCRange15").val(),
            CCRANGE16: $("#txtCCRange16").val(),
            CCRANGE17: $("#txtCCRange17").val(),
            CCRANGE18: $("#txtCCRange18").val(),
            CCRANGE19: $("#txtCCRange19").val(),
            CCRANGE20: $("#txtCCRange20").val(),
            CCRANGE21: $("#txtCCRange21").val(),
            CCRANGE22: $("#txtCCRange22").val(),
            CCRANGE23: $("#txtCCRange23").val(),
            CCRANGE24: $("#txtCCRange24").val(),
            CCRANGE25: $("#txtCCRange25").val(),
            CCMIN1: $("#txtMin1").val(),
            CCMIN2: $("#txtMin2").val(),
            CCMIN3: $("#txtMin3").val(),
            CCMIN4: $("#txtMin4").val(),
            CCMIN5: $("#txtMin5").val(),
            CCMIN6: $("#txtMin6").val(),
            CCMIN7: $("#txtMin7").val(),
            CCMIN8: $("#txtMin8").val(),
            CCMIN9: $("#txtMin9").val(),
            CCMIN10: $("#txtMin10").val(),
            CCMIN11: $("#txtMin11").val(),
            CCMIN12: $("#txtMin12").val(),
            CCMIN13: $("#txtMin13").val(),
            CCMIN14: $("#txtMin14").val(),
            CCMIN15: $("#txtMin15").val(),
            CCMIN16: $("#txtMin16").val(),
            CCMIN17: $("#txtMin17").val(),
            CCMIN18: $("#txtMin18").val(),
            CCMIN19: $("#txtMin19").val(),
            CCMIN20: $("#txtMin20").val(),
            CCMIN21: $("#txtMin21").val(),
            CCMIN22: $("#txtMin22").val(),
            CCMIN23: $("#txtMin23").val(),
            CCMIN24: $("#txtMin24").val(),
            CCMIN25: $("#txtMin25").val(),
            CCMAX1: $("#txtMax1").val(),
            CCMAX2: $("#txtMax2").val(),
            CCMAX3: $("#txtMax3").val(),
            CCMAX4: $("#txtMax4").val(),
            CCMAX5: $("#txtMax5").val(),
            CCMAX6: $("#txtMax6").val(),
            CCMAX7: $("#txtMax7").val(),
            CCMAX8: $("#txtMax8").val(),
            CCMAX9: $("#txtMax9").val(),
            CCMAX10: $("#txtMax10").val(),
            CCMAX11: $("#txtMax11").val(),
            CCMAX12: $("#txtMax12").val(),
            CCMAX13: $("#txtMax13").val(),
            CCMAX14: $("#txtMax14").val(),
            CCMAX15: $("#txtMax15").val(),
            CCMAX16: $("#txtMax16").val(),
            CCMAX17: $("#txtMax17").val(),
            CCMAX18: $("#txtMax18").val(),
            CCMAX19: $("#txtMax19").val(),
            CCMAX20: $("#txtMax20").val(),
            CCMAX21: $("#txtMax21").val(),
            CCMAX22: $("#txtMax22").val(),
            CCMAX23: $("#txtMax23").val(),
            CCMAX24: $("#txtMax24").val(),
            CCMAX25: $("#txtMax25").val(),
            CCACTUAL1: $("#txtCCActual1").val(),
            CCACTUAL2: $("#txtCCActual2").val(),
            CCACTUAL3: $("#txtCCActual3").val(),
            CCACTUAL4: $("#txtCCActual4").val(),
            CCACTUAL5: $("#txtCCActual5").val(),
            CCACTUAL6: $("#txtCCActual6").val(),
            CCACTUAL7: $("#txtCCActual7").val(),
            CCACTUAL8: $("#txtCCActual8").val(),
            CCACTUAL9: $("#txtCCActual9").val(),
            CCACTUAL10: $("#txtCCActual10").val(),
            CCACTUAL11: $("#txtCCActual11").val(),
            CCACTUAL12: $("#txtCCActual12").val(),
            CCACTUAL13: $("#txtCCActual13").val(),
            CCACTUAL14: $("#txtCCActual14").val(),
            CCACTUAL15: $("#txtCCActual15").val(),
            CCACTUAL16: $("#txtCCActual16").val(),
            CCACTUAL17: $("#txtCCActual17").val(),
            CCACTUAL18: $("#txtCCActual18").val(),
            CCACTUAL19: $("#txtCCActual19").val(),
            CCACTUAL20: $("#txtCCActual20").val(),
            CCACTUAL21: $("#txtCCActual21").val(),
            CCACTUAL22: $("#txtCCActual22").val(),
            CCACTUAL23: $("#txtCCActual23").val(),
            CCACTUAL24: $("#txtCCActual24").val(),
            CCACTUAL25: $("#txtCCActual25").val(),
            HARD_STD1: $("#txtHardSTD1").val(),
            HARD_STD2: $("#txtHardSTD2").val(),
            HARD_STD3: $("#txtHardSTD3").val(),
            HARD_STD4: $("#txtHardSTD4").val(),
            HARD_STD5: $("#txtHardSTD5").val(),
            HARD_STD6: $("#txtHardSTD6").val(),
            HARD_STD7: $("#txtHardSTD7").val(),
            HARD_STD8: $("#txtHardSTD8").val(),
            HARD_STD9: $("#txtHardSTD9").val(),
            HARD_STD10: $("#txtHardSTD10").val(),
            HARD_STD11: $("#txtHardSTD11").val(),
            HARD_STD12: $("#txtHardSTD12").val(),
            HARD_STD13: $("#txtHardSTD13").val(),
            HARD_STD14: $("#txtHardSTD14").val(),
            HARD_STD15: $("#txtHardSTD15").val(),
            HARD_STD16: $("#txtHardSTD16").val(),
            HARD_STD17: $("#txtHardSTD17").val(),
            HARD_STD18: $("#txtHardSTD18").val(),
            HARD_STD19: $("#txtHardSTD19").val(),
            HARD_STD20: $("#txtHardSTD20").val(),
            HARD_STD21: $("#txtHardSTD21").val(),
            HARD_STD22: $("#txtHardSTD22").val(),
            HARD_STD23: $("#txtHardSTD23").val(),
            HARD_STD24: $("#txtHardSTD24").val(),
            HARD_STD25: $("#txtHardSTD25").val(),
            HARD_MIN1: $("#txtHardMin1").val(),
            HARD_MIN2: $("#txtHardMin2").val(),
            HARD_MIN3: $("#txtHardMin3").val(),
            HARD_MIN4: $("#txtHardMin4").val(),
            HARD_MIN5: $("#txtHardMin5").val(),
            HARD_MIN6: $("#txtHardMin6").val(),
            HARD_MIN7: $("#txtHardMin7").val(),
            HARD_MIN8: $("#txtHardMin8").val(),
            HARD_MIN9: $("#txtHardMin9").val(),
            HARD_MIN10: $("#txtHardMin10").val(),
            HARD_MIN11: $("#txtHardMin11").val(),
            HARD_MIN12: $("#txtHardMin12").val(),
            HARD_MIN13: $("#txtHardMin13").val(),
            HARD_MIN14: $("#txtHardMin14").val(),
            HARD_MIN15: $("#txtHardMin15").val(),
            HARD_MIN16: $("#txtHardMin16").val(),
            HARD_MIN17: $("#txtHardMin17").val(),
            HARD_MIN18: $("#txtHardMin18").val(),
            HARD_MIN19: $("#txtHardMin19").val(),
            HARD_MIN20: $("#txtHardMin20").val(),
            HARD_MIN21: $("#txtHardMin21").val(),
            HARD_MIN22: $("#txtHardMin22").val(),
            HARD_MIN23: $("#txtHardMin23").val(),
            HARD_MIN24: $("#txtHardMin24").val(),
            HARD_MIN25: $("#txtHardMin25").val(),
            HARD_MAX1: $("#txtHardMax1").val(),
            HARD_MAX2: $("#txtHardMax2").val(),
            HARD_MAX3: $("#txtHardMax3").val(),
            HARD_MAX4: $("#txtHardMax4").val(),
            HARD_MAX5: $("#txtHardMax5").val(),
            HARD_MAX6: $("#txtHardMax6").val(),
            HARD_MAX7: $("#txtHardMax7").val(),
            HARD_MAX8: $("#txtHardMax8").val(),
            HARD_MAX9: $("#txtHardMax9").val(),
            HARD_MAX10: $("#txtHardMax10").val(),
            HARD_MAX11: $("#txtHardMax11").val(),
            HARD_MAX12: $("#txtHardMax12").val(),
            HARD_MAX13: $("#txtHardMax13").val(),
            HARD_MAX14: $("#txtHardMax14").val(),
            HARD_MAX15: $("#txtHardMax15").val(),
            HARD_MAX16: $("#txtHardMax16").val(),
            HARD_MAX17: $("#txtHardMax17").val(),
            HARD_MAX18: $("#txtHardMax18").val(),
            HARD_MAX19: $("#txtHardMax19").val(),
            HARD_MAX20: $("#txtHardMax20").val(),
            HARD_MAX21: $("#txtHardMax21").val(),
            HARD_MAX22: $("#txtHardMax22").val(),
            HARD_MAX23: $("#txtHardMax23").val(),
            HARD_MAX24: $("#txtHardMax24").val(),
            HARD_MAX25: $("#txtHardMax25").val(),
            HARD_ACTUAL1: $("#txtHardActual1").val(),
            HARD_ACTUAL2: $("#txtHardActual2").val(),
            HARD_ACTUAL3: $("#txtHardActual3").val(),
            HARD_ACTUAL4: $("#txtHardActual4").val(),
            HARD_ACTUAL5: $("#txtHardActual5").val(),
            HARD_ACTUAL6: $("#txtHardActual6").val(),
            HARD_ACTUAL7: $("#txtHardActual7").val(),
            HARD_ACTUAL8: $("#txtHardActual8").val(),
            HARD_ACTUAL9: $("#txtHardActual9").val(),
            HARD_ACTUAL10: $("#txtHardActual10").val(),
            HARD_ACTUAL11: $("#txtHardActual11").val(),
            HARD_ACTUAL12: $("#txtHardActual12").val(),
            HARD_ACTUAL13: $("#txtHardActual13").val(),
            HARD_ACTUAL14: $("#txtHardActual14").val(),
            HARD_ACTUAL15: $("#txtHardActual15").val(),
            HARD_ACTUAL16: $("#txtHardActual16").val(),
            HARD_ACTUAL17: $("#txtHardActual17").val(),
            HARD_ACTUAL18: $("#txtHardActual18").val(),
            HARD_ACTUAL19: $("#txtHardActual19").val(),
            HARD_ACTUAL20: $("#txtHardActual20").val(),
            HARD_ACTUAL21: $("#txtHardActual21").val(),
            HARD_ACTUAL22: $("#txtHardActual22").val(),
            HARD_ACTUAL23: $("#txtHardActual23").val(),
            HARD_ACTUAL24: $("#txtHardActual24").val(),
            HARD_ACTUAL25: $("#txtHardActual25").val(),
            USER_ID: $("#hdfLoginId").val().trim(),
        });
    tempMillsheetData = tempMillsheetData.filter(x => x.PAGE_NO !== currentPage);
    tempMillsheetData.push(millsheetData);
}
function SetTempMillsheetData(page_)
{
    var getTempData = tempMillsheetData.filter(x => x.PAGE_NO == page_)[0];
    if (getTempData != null) {

        if (getTempData.IS_CONFIRMED == 1) {
            $("#chkConfirmPage").prop("checked", true);
        } else {
            $("#chkConfirmPage").prop("checked", false);
        }
        if (getTempData.ALREADY_SAVED == 'YES') {
            $("#chkConfirmPage").attr("DISABLED", "DISABLED").addClass("DISABLED");
        } else {
            $("#chkConfirmPage").removeAttr("DISABLED", "DISABLED").removeClass("DISABLED");
        }

        $("#txtkouban").val(getTempData.KOUBAN);
        $("#txtcharge").val(getTempData.CHARGE);
        $("#txtTypeofSteel").val(getTempData.MATL_NAME_MAKER);
        $("#txtDimension").val(getTempData.DIM_D);
        $("#txtLength").val(getTempData.LENGTH);
        $("#txtNoOfUnits").val(getTempData.NO_OF_UNITS);
        $("#ddlUnitType").val(getTempData.UNIT);
        $("#txtWeightWithoutKG").val(getTempData.QUANTITY)
        $("#txtGainsizeSTD").val(getTempData.GRAIN_SIZE_STD);
        $("#txtGainSizeActual").val(getTempData.GRAIN_SIZE_ACTUAL);
        $("#txtHardnessmakerSTD").val(getTempData.HARDNESS_STD);
        $("#txtHardnessMin").val(getTempData.HARDNESS_MIN);
        $("#txtHardnessMax").val(getTempData.HARDNESS_MAX);
        $("#txtHardnessMakerActual").val(getTempData.HARDNESS_ACTUAL);
        $("#txtCCName1").val(getTempData.CCNAME1);
        $("#txtCCName2").val(getTempData.CCNAME2);
        $("#txtCCName3").val(getTempData.CCNAME3);
        $("#txtCCName4").val(getTempData.CCNAME4);
        $("#txtCCName5").val(getTempData.CCNAME5);
        $("#txtCCName6").val(getTempData.CCNAME6);
        $("#txtCCName7").val(getTempData.CCNAME7);
        $("#txtCCName8").val(getTempData.CCNAME8);
        $("#txtCCName9").val(getTempData.CCNAME9);
        $("#txtCCName10").val(getTempData.CCNAME10);
        $("#txtCCName11").val(getTempData.CCNAME11);
        $("#txtCCName12").val(getTempData.CCNAME12);
        $("#txtCCName13").val(getTempData.CCNAME13);
        $("#txtCCName14").val(getTempData.CCNAME14);
        $("#txtCCName15").val(getTempData.CCNAME15);
        $("#txtCCName16").val(getTempData.CCNAME16);
        $("#txtCCName17").val(getTempData.CCNAME17);
        $("#txtCCName18").val(getTempData.CCNAME18);
        $("#txtCCName19").val(getTempData.CCNAME19);
        $("#txtCCName20").val(getTempData.CCNAME20);
        $("#txtCCName21").val(getTempData.CCNAME21);
        $("#txtCCName22").val(getTempData.CCNAME22);
        $("#txtCCName23").val(getTempData.CCNAME23);
        $("#txtCCName24").val(getTempData.CCNAME24);
        $("#txtCCName25").val(getTempData.CCNAME25);
        $("#txtCCRange1").val(getTempData.CCRANGE1);
        $("#txtCCRange2").val(getTempData.CCRANGE2);
        $("#txtCCRange3").val(getTempData.CCRANGE3);
        $("#txtCCRange4").val(getTempData.CCRANGE4);
        $("#txtCCRange5").val(getTempData.CCRANGE5);
        $("#txtCCRange6").val(getTempData.CCRANGE6);
        $("#txtCCRange7").val(getTempData.CCRANGE7);
        $("#txtCCRange8").val(getTempData.CCRANGE8);
        $("#txtCCRange9").val(getTempData.CCRANGE9);
        $("#txtCCRange10").val(getTempData.CCRANGE10);
        $("#txtCCRange11").val(getTempData.CCRANGE11);
        $("#txtCCRange12").val(getTempData.CCRANGE12);
        $("#txtCCRange13").val(getTempData.CCRANGE13);
        $("#txtCCRange14").val(getTempData.CCRANGE14);
        $("#txtCCRange15").val(getTempData.CCRANGE15);
        $("#txtCCRange16").val(getTempData.CCRANGE16);
        $("#txtCCRange17").val(getTempData.CCRANGE17);
        $("#txtCCRange18").val(getTempData.CCRANGE18);
        $("#txtCCRange19").val(getTempData.CCRANGE19);
        $("#txtCCRange20").val(getTempData.CCRANGE20);
        $("#txtCCRange21").val(getTempData.CCRANGE21);
        $("#txtCCRange22").val(getTempData.CCRANGE22);
        $("#txtCCRange23").val(getTempData.CCRANGE23);
        $("#txtCCRange24").val(getTempData.CCRANGE24);
        $("#txtCCRange25").val(getTempData.CCRANGE25);
        $("#txtMin1").val(getTempData.CCMIN1);
        $("#txtMin2").val(getTempData.CCMIN2);
        $("#txtMin3").val(getTempData.CCMIN3);
        $("#txtMin4").val(getTempData.CCMIN4);
        $("#txtMin5").val(getTempData.CCMIN5);
        $("#txtMin6").val(getTempData.CCMIN6);
        $("#txtMin7").val(getTempData.CCMIN7);
        $("#txtMin8").val(getTempData.CCMIN8);
        $("#txtMin9").val(getTempData.CCMIN9);
        $("#txtMin10").val(getTempData.CCMIN10);
        $("#txtMin11").val(getTempData.CCMIN11);
        $("#txtMin12").val(getTempData.CCMIN12);
        $("#txtMin13").val(getTempData.CCMIN13);
        $("#txtMin14").val(getTempData.CCMIN14);
        $("#txtMin15").val(getTempData.CCMIN15);
        $("#txtMin16").val(getTempData.CCMIN16);
        $("#txtMin17").val(getTempData.CCMIN17);
        $("#txtMin18").val(getTempData.CCMIN18);
        $("#txtMin19").val(getTempData.CCMIN19);
        $("#txtMin20").val(getTempData.CCMIN20);
        $("#txtMin21").val(getTempData.CCMIN21);
        $("#txtMin22").val(getTempData.CCMIN22);
        $("#txtMin23").val(getTempData.CCMIN23);
        $("#txtMin24").val(getTempData.CCMIN24);
        $("#txtMin25").val(getTempData.CCMIN25);
        $("#txtMax1").val(getTempData.CCMAX1);
        $("#txtMax2").val(getTempData.CCMAX2);
        $("#txtMax3").val(getTempData.CCMAX3);
        $("#txtMax4").val(getTempData.CCMAX4);
        $("#txtMax5").val(getTempData.CCMAX5);
        $("#txtMax6").val(getTempData.CCMAX6);
        $("#txtMax7").val(getTempData.CCMAX7);
        $("#txtMax8").val(getTempData.CCMAX8);
        $("#txtMax9").val(getTempData.CCMAX9);
        $("#txtMax10").val(getTempData.CCMAX10);
        $("#txtMax11").val(getTempData.CCMAX11);
        $("#txtMax12").val(getTempData.CCMAX12);
        $("#txtMax13").val(getTempData.CCMAX13);
        $("#txtMax14").val(getTempData.CCMAX14);
        $("#txtMax15").val(getTempData.CCMAX15);
        $("#txtMax16").val(getTempData.CCMAX16);
        $("#txtMax17").val(getTempData.CCMAX17);
        $("#txtMax18").val(getTempData.CCMAX18);
        $("#txtMax19").val(getTempData.CCMAX19);
        $("#txtMax20").val(getTempData.CCMAX20);
        $("#txtMax21").val(getTempData.CCMAX21);
        $("#txtMax22").val(getTempData.CCMAX22);
        $("#txtMax23").val(getTempData.CCMAX23);
        $("#txtMax24").val(getTempData.CCMAX24);
        $("#txtMax25").val(getTempData.CCMAX25);
        $("#txtCCActual1").val(getTempData.CCACTUAL1);
        $("#txtCCActual2").val(getTempData.CCACTUAL2);
        $("#txtCCActual3").val(getTempData.CCACTUAL3);
        $("#txtCCActual4").val(getTempData.CCACTUAL4);
        $("#txtCCActual5").val(getTempData.CCACTUAL5);
        $("#txtCCActual6").val(getTempData.CCACTUAL6);
        $("#txtCCActual7").val(getTempData.CCACTUAL7);
        $("#txtCCActual8").val(getTempData.CCACTUAL8);
        $("#txtCCActual9").val(getTempData.CCACTUAL9);
        $("#txtCCActual10").val(getTempData.CCACTUAL10);
        $("#txtCCActual11").val(getTempData.CCACTUAL11);
        $("#txtCCActual12").val(getTempData.CCACTUAL12);
        $("#txtCCActual13").val(getTempData.CCACTUAL13);
        $("#txtCCActual14").val(getTempData.CCACTUAL14);
        $("#txtCCActual15").val(getTempData.CCACTUAL15);
        $("#txtCCActual16").val(getTempData.CCACTUAL16);
        $("#txtCCActual17").val(getTempData.CCACTUAL17);
        $("#txtCCActual18").val(getTempData.CCACTUAL18);
        $("#txtCCActual19").val(getTempData.CCACTUAL19);
        $("#txtCCActual20").val(getTempData.CCACTUAL20);
        $("#txtCCActual21").val(getTempData.CCACTUAL21);
        $("#txtCCActual22").val(getTempData.CCACTUAL22);
        $("#txtCCActual23").val(getTempData.CCACTUAL23);
        $("#txtCCActual24").val(getTempData.CCACTUAL24);
        $("#txtCCActual25").val(getTempData.CCACTUAL25);
        $("#txtHardSTD1").val(getTempData.HARD_STD1);
        $("#txtHardSTD2").val(getTempData.HARD_STD2);
        $("#txtHardSTD3").val(getTempData.HARD_STD3);
        $("#txtHardSTD4").val(getTempData.HARD_STD4);
        $("#txtHardSTD5").val(getTempData.HARD_STD5);
        $("#txtHardSTD6").val(getTempData.HARD_STD6);
        $("#txtHardSTD7").val(getTempData.HARD_STD7);
        $("#txtHardSTD8").val(getTempData.HARD_STD8);
        $("#txtHardSTD9").val(getTempData.HARD_STD9);
        $("#txtHardSTD10").val(getTempData.HARD_STD10);
        $("#txtHardSTD11").val(getTempData.HARD_STD11);
        $("#txtHardSTD12").val(getTempData.HARD_STD12);
        $("#txtHardSTD13").val(getTempData.HARD_STD13);
        $("#txtHardSTD14").val(getTempData.HARD_STD14);
        $("#txtHardSTD15").val(getTempData.HARD_STD15);
        $("#txtHardSTD16").val(getTempData.HARD_STD16);
        $("#txtHardSTD17").val(getTempData.HARD_STD17);
        $("#txtHardSTD18").val(getTempData.HARD_STD18);
        $("#txtHardSTD19").val(getTempData.HARD_STD19);
        $("#txtHardSTD20").val(getTempData.HARD_STD20);
        $("#txtHardSTD21").val(getTempData.HARD_STD21);
        $("#txtHardSTD22").val(getTempData.HARD_STD22);
        $("#txtHardSTD23").val(getTempData.HARD_STD23);
        $("#txtHardSTD24").val(getTempData.HARD_STD24);
        $("#txtHardSTD25").val(getTempData.HARD_STD25);
        $("#txtHardMin1").val(getTempData.HARD_MIN1);
        $("#txtHardMin2").val(getTempData.HARD_MIN2);
        $("#txtHardMin3").val(getTempData.HARD_MIN3);
        $("#txtHardMin4").val(getTempData.HARD_MIN4);
        $("#txtHardMin5").val(getTempData.HARD_MIN5);
        $("#txtHardMin6").val(getTempData.HARD_MIN6);
        $("#txtHardMin7").val(getTempData.HARD_MIN7);
        $("#txtHardMin8").val(getTempData.HARD_MIN8);
        $("#txtHardMin9").val(getTempData.HARD_MIN9);
        $("#txtHardMin10").val(getTempData.HARD_MIN10);
        $("#txtHardMin11").val(getTempData.HARD_MIN11);
        $("#txtHardMin12").val(getTempData.HARD_MIN12);
        $("#txtHardMin13").val(getTempData.HARD_MIN13);
        $("#txtHardMin14").val(getTempData.HARD_MIN14);
        $("#txtHardMin15").val(getTempData.HARD_MIN15);
        $("#txtHardMin16").val(getTempData.HARD_MIN16);
        $("#txtHardMin17").val(getTempData.HARD_MIN17);
        $("#txtHardMin18").val(getTempData.HARD_MIN18);
        $("#txtHardMin19").val(getTempData.HARD_MIN19);
        $("#txtHardMin20").val(getTempData.HARD_MIN20);
        $("#txtHardMin21").val(getTempData.HARD_MIN21);
        $("#txtHardMin22").val(getTempData.HARD_MIN22);
        $("#txtHardMin23").val(getTempData.HARD_MIN23);
        $("#txtHardMin24").val(getTempData.HARD_MIN24);
        $("#txtHardMin25").val(getTempData.HARD_MIN25);
        $("#txtHardMax1").val(getTempData.HARD_MAX1);
        $("#txtHardMax2").val(getTempData.HARD_MAX2);
        $("#txtHardMax3").val(getTempData.HARD_MAX3);
        $("#txtHardMax4").val(getTempData.HARD_MAX4);
        $("#txtHardMax5").val(getTempData.HARD_MAX5);
        $("#txtHardMax6").val(getTempData.HARD_MAX6);
        $("#txtHardMax7").val(getTempData.HARD_MAX7);
        $("#txtHardMax8").val(getTempData.HARD_MAX8);
        $("#txtHardMax9").val(getTempData.HARD_MAX9);
        $("#txtHardMax10").val(getTempData.HARD_MAX10);
        $("#txtHardMax11").val(getTempData.HARD_MAX11);
        $("#txtHardMax12").val(getTempData.HARD_MAX12);
        $("#txtHardMax13").val(getTempData.HARD_MAX13);
        $("#txtHardMax14").val(getTempData.HARD_MAX14);
        $("#txtHardMax15").val(getTempData.HARD_MAX15);
        $("#txtHardMax16").val(getTempData.HARD_MAX16);
        $("#txtHardMax17").val(getTempData.HARD_MAX17);
        $("#txtHardMax18").val(getTempData.HARD_MAX18);
        $("#txtHardMax19").val(getTempData.HARD_MAX19);
        $("#txtHardMax20").val(getTempData.HARD_MAX20);
        $("#txtHardMax21").val(getTempData.HARD_MAX21);
        $("#txtHardMax22").val(getTempData.HARD_MAX22);
        $("#txtHardMax23").val(getTempData.HARD_MAX23);
        $("#txtHardMax24").val(getTempData.HARD_MAX24);
        $("#txtHardMax25").val(getTempData.HARD_MAX25);
        $("#txtHardActual1").val(getTempData.HARD_ACTUAL1);
        $("#txtHardActual2").val(getTempData.HARD_ACTUAL2);
        $("#txtHardActual3").val(getTempData.HARD_ACTUAL3);
        $("#txtHardActual4").val(getTempData.HARD_ACTUAL4);
        $("#txtHardActual5").val(getTempData.HARD_ACTUAL5);
        $("#txtHardActual6").val(getTempData.HARD_ACTUAL6);
        $("#txtHardActual7").val(getTempData.HARD_ACTUAL7);
        $("#txtHardActual8").val(getTempData.HARD_ACTUAL8);
        $("#txtHardActual9").val(getTempData.HARD_ACTUAL9);
        $("#txtHardActual10").val(getTempData.HARD_ACTUAL10);
        $("#txtHardActual11").val(getTempData.HARD_ACTUAL11);
        $("#txtHardActual12").val(getTempData.HARD_ACTUAL12);
        $("#txtHardActual13").val(getTempData.HARD_ACTUAL13);
        $("#txtHardActual14").val(getTempData.HARD_ACTUAL14);
        $("#txtHardActual15").val(getTempData.HARD_ACTUAL15);
        $("#txtHardActual16").val(getTempData.HARD_ACTUAL16);
        $("#txtHardActual17").val(getTempData.HARD_ACTUAL17);
        $("#txtHardActual18").val(getTempData.HARD_ACTUAL18);
        $("#txtHardActual19").val(getTempData.HARD_ACTUAL19);
        $("#txtHardActual20").val(getTempData.HARD_ACTUAL20);
        $("#txtHardActual21").val(getTempData.HARD_ACTUAL21);
        $("#txtHardActual22").val(getTempData.HARD_ACTUAL22);
        $("#txtHardActual23").val(getTempData.HARD_ACTUAL23);
        $("#txtHardActual24").val(getTempData.HARD_ACTUAL24);
        $("#txtHardActual25").val(getTempData.HARD_ACTUAL25);
        
    }
}
function BindFormDataForSave()
{
    var k = 0;
    
    var fileExist = $("#fileUpload")[0].files[0];

    FillTempMillsheetData();
    formData = new FormData();
    for (let i = 0; i < tempMillsheetData.length; i++) {
        if (tempMillsheetData[i].IS_CONFIRMED == 1 && tempMillsheetData[i].ALREADY_SAVED =='NO') {
            
            if (fileExist) {
                var base64 = ReadFile(tempMillsheetData[i].PAGE_NO);

                // Remove prefix if exists
                if (base64.indexOf(",") !== -1) {
                    base64 = base64.split(",")[1];
                }

                var byteCharacters = atob(base64);
                var byteNumbers = new Array(byteCharacters.length);

                for (var j = 0; j < byteCharacters.length; j++) {
                    byteNumbers[j] = byteCharacters.charCodeAt(j);
                }

                var byteArray = new Uint8Array(byteNumbers);
                var blob = new Blob([byteArray], { type: "application/pdf" });

                // Create File object
                var uFile = new File([blob], "Millsheet_" + i + ".pdf", {
                    type: "application/pdf"
                });

                tempMillsheetData[i].FORM_FILE = uFile;
            } else {

                tempMillsheetData[i].FORM_FILE = null;
            }

            if (tempMillsheetData[i].CCMIN1.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN1 = tempMillsheetData[i].CCMAX1;
                tempMillsheetData[i].CCMAX1 = "";
            } else if (tempMillsheetData[i].CCMIN1.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN1 = "";
            }
            if (tempMillsheetData[i].CCMAX1.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX1 = "";
            } else if (tempMillsheetData[i].CCMAX1.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX1 = tempMillsheetData[i].CCMIN1;
                tempMillsheetData[i].CCMIN1 = "";
            }

            if (tempMillsheetData[i].CCMIN2.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN2 = tempMillsheetData[i].CCMAX2;
                tempMillsheetData[i].CCMAX2 = "";
            } else if (tempMillsheetData[i].CCMIN2.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN2 = "";
            }
            if (tempMillsheetData[i].CCMAX2.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX2 = "";
            } else if (tempMillsheetData[i].CCMAX2.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX2 = tempMillsheetData[i].CCMIN2;
                tempMillsheetData[i].CCMIN2 = "";
            }

            if (tempMillsheetData[i].CCMIN3.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN3 = tempMillsheetData[i].CCMAX3;
                tempMillsheetData[i].CCMAX3 = "";
            } else if (tempMillsheetData[i].CCMIN3.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN3 = "";
            }
            if (tempMillsheetData[i].CCMAX3.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX3 = "";
            } else if (tempMillsheetData[i].CCMAX3.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX3 = tempMillsheetData[i].CCMIN3;
                tempMillsheetData[i].CCMIN3 = "";
            }

            if (tempMillsheetData[i].CCMIN4.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN4 = tempMillsheetData[i].CCMAX4;
                tempMillsheetData[i].CCMAX4 = "";
            } else if (tempMillsheetData[i].CCMIN4.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN4 = "";
            }
            if (tempMillsheetData[i].CCMAX4.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX4 = "";
            } else if (tempMillsheetData[i].CCMAX4.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX4 = tempMillsheetData[i].CCMIN4;
                tempMillsheetData[i].CCMIN4 = "";
            }

            if (tempMillsheetData[i].CCMIN5.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN5 = tempMillsheetData[i].CCMAX5;
                tempMillsheetData[i].CCMAX5 = "";
            } else if (tempMillsheetData[i].CCMIN5.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN5 = "";
            }
            if (tempMillsheetData[i].CCMAX5.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX5 = "";
            } else if (tempMillsheetData[i].CCMAX5.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX5 = tempMillsheetData[i].CCMIN5;
                tempMillsheetData[i].CCMIN5 = "";
            }

            if (tempMillsheetData[i].CCMIN6.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN6 = tempMillsheetData[i].CCMAX6;
                tempMillsheetData[i].CCMAX6 = "";
            } else if (tempMillsheetData[i].CCMIN6.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN6 = "";
            }
            if (tempMillsheetData[i].CCMAX6.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX6 = "";
            } else if (tempMillsheetData[i].CCMAX6.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX6 = tempMillsheetData[i].CCMIN6;
                tempMillsheetData[i].CCMIN6 = "";
            }

            if (tempMillsheetData[i].CCMIN7.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN7 = tempMillsheetData[i].CCMAX7;
                tempMillsheetData[i].CCMAX7 = "";
            } else if (tempMillsheetData[i].CCMIN7.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN7 = "";
            }
            if (tempMillsheetData[i].CCMAX7.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX7 = "";
            } else if (tempMillsheetData[i].CCMAX7.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX7 = tempMillsheetData[i].CCMIN7;
                tempMillsheetData[i].CCMIN7 = "";
            }

            if (tempMillsheetData[i].CCMIN8.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN8 = tempMillsheetData[i].CCMAX8;
                tempMillsheetData[i].CCMAX8 = "";
            } else if (tempMillsheetData[i].CCMIN8.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN8 = "";
            }
            if (tempMillsheetData[i].CCMAX8.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX8 = "";
            } else if (tempMillsheetData[i].CCMAX8.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX8 = tempMillsheetData[i].CCMIN8;
                tempMillsheetData[i].CCMIN8 = "";
            }

            if (tempMillsheetData[i].CCMIN9.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN9 = tempMillsheetData[i].CCMAX9;
                tempMillsheetData[i].CCMAX9 = "";
            } else if (tempMillsheetData[i].CCMIN9.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN9 = "";
            }
            if (tempMillsheetData[i].CCMAX9.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX9 = "";
            } else if (tempMillsheetData[i].CCMAX9.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX9 = tempMillsheetData[i].CCMIN9;
                tempMillsheetData[i].CCMIN9 = "";
            }

            if (tempMillsheetData[i].CCMIN10.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN10 = tempMillsheetData[i].CCMAX10;
                tempMillsheetData[i].CCMAX10 = "";
            } else if (tempMillsheetData[i].CCMIN10.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN10 = "";
            }
            if (tempMillsheetData[i].CCMAX10.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX10 = "";
            } else if (tempMillsheetData[i].CCMAX10.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX10 = tempMillsheetData[i].CCMIN10;
                tempMillsheetData[i].CCMIN10 = "";
            }

            if (tempMillsheetData[i].CCMIN11.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN11 = tempMillsheetData[i].CCMAX11;
                tempMillsheetData[i].CCMAX11 = "";
            } else if (tempMillsheetData[i].CCMIN11.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN11 = "";
            }
            if (tempMillsheetData[i].CCMAX11.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX11 = "";
            } else if (tempMillsheetData[i].CCMAX11.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX11 = tempMillsheetData[i].CCMIN11;
                tempMillsheetData[i].CCMIN11 = "";
            }

            if (tempMillsheetData[i].CCMIN12.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN12 = tempMillsheetData[i].CCMAX12;
                tempMillsheetData[i].CCMAX12 = "";
            } else if (tempMillsheetData[i].CCMIN12.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN12 = "";
            }
            if (tempMillsheetData[i].CCMAX12.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX12 = "";
            } else if (tempMillsheetData[i].CCMAX12.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX12 = tempMillsheetData[i].CCMIN12;
                tempMillsheetData[i].CCMIN12 = "";
            }

            if (tempMillsheetData[i].CCMIN13.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN13 = tempMillsheetData[i].CCMAX13;
                tempMillsheetData[i].CCMAX13 = "";
            } else if (tempMillsheetData[i].CCMIN13.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN13 = "";
            }
            if (tempMillsheetData[i].CCMAX13.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX13 = "";
            } else if (tempMillsheetData[i].CCMAX13.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX13 = tempMillsheetData[i].CCMIN13;
                tempMillsheetData[i].CCMIN13 = "";
            }

            if (tempMillsheetData[i].CCMIN14.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN14 = tempMillsheetData[i].CCMAX14;
                tempMillsheetData[i].CCMAX14 = "";
            } else if (tempMillsheetData[i].CCMIN14.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN14 = "";
            }
            if (tempMillsheetData[i].CCMAX14.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX14 = "";
            } else if (tempMillsheetData[i].CCMAX14.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX14 = tempMillsheetData[i].CCMIN14;
                tempMillsheetData[i].CCMIN14 = "";
            }

            if (tempMillsheetData[i].CCMIN15.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN15 = tempMillsheetData[i].CCMAX15;
                tempMillsheetData[i].CCMAX15 = "";
            } else if (tempMillsheetData[i].CCMIN15.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN15 = "";
            }
            if (tempMillsheetData[i].CCMAX15.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN15 = "";
            } else if (tempMillsheetData[i].CCMAX15.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX15 = tempMillsheetData[i].CCMIN15;
                tempMillsheetData[i].CCMAX15 = "";
            }

            if (tempMillsheetData[i].CCMIN16.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN16 = tempMillsheetData[i].CCMAX16;
                tempMillsheetData[i].CCMAX16 = "";
            } else if (tempMillsheetData[i].CCMIN16.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN16 = "";
            }
            if (tempMillsheetData[i].CCMAX16.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX16 = "";
            } else if (tempMillsheetData[i].CCMAX16.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX16 = tempMillsheetData[i].CCMIN16;
                tempMillsheetData[i].CCMIN16 = "";
            }

            if (tempMillsheetData[i].CCMIN17.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN17 = tempMillsheetData[i].CCMAX17;
                tempMillsheetData[i].CCMAX17 = "";
            } else if (tempMillsheetData[i].CCMIN17.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN17 = "";
            }
            if (tempMillsheetData[i].CCMAX17.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX17 = "";
            } else if (tempMillsheetData[i].CCMAX17.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX17 = tempMillsheetData[i].CCMIN17;
                tempMillsheetData[i].CCMIN17 = "";
            }

            if (tempMillsheetData[i].CCMIN18.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN18 = tempMillsheetData[i].CCMAX18;
                tempMillsheetData[i].CCMAX18 = "";
            } else if (tempMillsheetData[i].CCMIN18.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN18 = "";
            }
            if (tempMillsheetData[i].CCMAX18.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX18 = "";
            } else if (tempMillsheetData[i].CCMAX18.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX18 = tempMillsheetData[i].CCMIN18;
                tempMillsheetData[i].CCMIN18 = "";
            }

            if (tempMillsheetData[i].CCMIN19.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN19 = tempMillsheetData[i].CCMAX19;
                tempMillsheetData[i].CCMAX19 = "";
            } else if (tempMillsheetData[i].CCMIN19.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN19 = "";
            }
            if (tempMillsheetData[i].CCMAX19.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN19 = "";
            } else if (tempMillsheetData[i].CCMAX19.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX19 = tempMillsheetData[i].CCMIN19;
                tempMillsheetData[i].CCMAX19 = "";
            }

            if (tempMillsheetData[i].CCMIN20.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN20 = tempMillsheetData[i].CCMAX20;
                tempMillsheetData[i].CCMAX20 = "";
            } else if (tempMillsheetData[i].CCMIN20.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN20 = "";
            }
            if (tempMillsheetData[i].CCMAX20.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX20 = "";
            } else if (tempMillsheetData[i].CCMAX20.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX20 = tempMillsheetData[i].CCMIN20;
                tempMillsheetData[i].CCMIN20 = "";
            }

            if (tempMillsheetData[i].CCMIN21.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN21 = tempMillsheetData[i].CCMAX21;
                tempMillsheetData[i].CCMAX21 = "";
            } else if (tempMillsheetData[i].CCMIN21.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN21 = "";
            }
            if (tempMillsheetData[i].CCMAX21.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX21 = "";
            } else if (tempMillsheetData[i].CCMAX21.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX21 = tempMillsheetData[i].CCMIN21;
                tempMillsheetData[i].CCMIN21 = "";
            }

            if (tempMillsheetData[i].CCMIN22.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN22 = tempMillsheetData[i].CCMAX22;
                tempMillsheetData[i].CCMAX22 = "";
            } else if (tempMillsheetData[i].CCMIN22.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN22 = "";
            }
            if (tempMillsheetData[i].CCMAX22.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX22 = "";
            } else if (tempMillsheetData[i].CCMAX22.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX22 = tempMillsheetData[i].CCMIN22;
                tempMillsheetData[i].CCMIN22 = "";
            }
            if (tempMillsheetData[i].CCMIN23.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN23 = tempMillsheetData[i].CCMAX23;
                tempMillsheetData[i].CCMAX23 = "";
            } else if (tempMillsheetData[i].CCMIN23.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN23 = "";
            }
            if (tempMillsheetData[i].CCMAX23.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX23 = "";
            } else if (tempMillsheetData[i].CCMAX23.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX23 = tempMillsheetData[i].CCMIN23;
                tempMillsheetData[i].CCMIN23 = "";
            }

            if (tempMillsheetData[i].CCMIN24.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN24 = tempMillsheetData[i].CCMAX24;
                tempMillsheetData[i].CCMAX24 = "";
            } else if (tempMillsheetData[i].CCMIN24.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN24 = "";
            }
            if (tempMillsheetData[i].CCMAX24.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX24 = "";
            } else if (tempMillsheetData[i].CCMAX24.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX24 = tempMillsheetData[i].CCMIN24;
                tempMillsheetData[i].CCMIN24 = "";
            }

            if (tempMillsheetData[i].CCMIN25.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMIN25 = tempMillsheetData[i].CCMAX25;
                tempMillsheetData[i].CCMAX25 = "";
            } else if (tempMillsheetData[i].CCMIN25.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMIN25 = "";
            }
            if (tempMillsheetData[i].CCMAX25.toUpperCase().includes("MI")) {
                tempMillsheetData[i].CCMAX25 = "";
            } else if (tempMillsheetData[i].CCMAX25.toUpperCase().includes("MA")) {
                tempMillsheetData[i].CCMAX25 = tempMillsheetData[i].CCMIN25;
                tempMillsheetData[i].CCMIN25 = "";
            }
            // EXAMPLE
            //CC_MIN = MAX AND CC_MAX= 50 THEN CC_MIN= EMPTY
            //CC_MIN = MIN AND CC_MAX= 90 THEN CC_MIN= 90 AND CC_MAX=EMPTY
            //=================================================================================== ABOVE EX. APPLIED FOR MIN AND MAX FIELDS =======================================
            if (tempMillsheetData[i].HARD_MIN1.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN1 = tempMillsheetData[i].HARD_MAX1;
                tempMillsheetData[i].HARD_MAX1 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN1.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN1 = "";
            }
            if (tempMillsheetData[i].HARD_MAX1.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX1 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX1.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX1 = tempMillsheetData[i].HARD_MIN1;
                tempMillsheetData[i].HARD_MIN1 = "";
            }

            if (tempMillsheetData[i].HARD_MIN2.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN2 = tempMillsheetData[i].HARD_MAX2;
                tempMillsheetData[i].HARD_MAX2 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN2.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN2 = "";
            }
            if (tempMillsheetData[i].HARD_MAX2.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX2 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX2.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX2 = tempMillsheetData[i].HARD_MIN2;
                tempMillsheetData[i].HARD_MIN2 = "";
            }

            if (tempMillsheetData[i].HARD_MIN3.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN3 = tempMillsheetData[i].HARD_MAX3;
                tempMillsheetData[i].HARD_MAX3 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN3.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN3 = "";
            }
            if (tempMillsheetData[i].HARD_MAX3.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX3 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX3.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX3 = tempMillsheetData[i].HARD_MIN3;
                tempMillsheetData[i].HARD_MIN3 = "";
            }

            if (tempMillsheetData[i].HARD_MIN4.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN4 = tempMillsheetData[i].HARD_MAX4;
                tempMillsheetData[i].HARD_MAX4 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN4.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN4 = "";
            }
            if (tempMillsheetData[i].HARD_MAX4.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX4 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX4.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX4 = tempMillsheetData[i].HARD_MIN4;
                tempMillsheetData[i].HARD_MIN4 = "";
            }

            if (tempMillsheetData[i].HARD_MIN5.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN5 = tempMillsheetData[i].HARD_MAX5;
                tempMillsheetData[i].HARD_MAX5 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN5.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN5 = "";
            }
            if (tempMillsheetData[i].HARD_MAX5.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX5 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX5.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX5 = tempMillsheetData[i].HARD_MIN5;
                tempMillsheetData[i].HARD_MIN5 = "";
            }

            if (tempMillsheetData[i].HARD_MIN6.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN6 = tempMillsheetData[i].HARD_MAX6;
                tempMillsheetData[i].HARD_MAX6 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN6.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN6 = "";
            }
            if (tempMillsheetData[i].HARD_MAX6.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX6 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX6.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX6 = tempMillsheetData[i].HARD_MIN6;
                tempMillsheetData[i].HARD_MIN6 = "";
            }

            if (tempMillsheetData[i].HARD_MIN7.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN7 = tempMillsheetData[i].HARD_MAX7;
                tempMillsheetData[i].HARD_MAX7 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN7.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN7 = "";
            }
            if (tempMillsheetData[i].HARD_MAX7.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX7 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX7.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX7 = tempMillsheetData[i].HARD_MIN7;
                tempMillsheetData[i].HARD_MIN7 = "";
            }

            if (tempMillsheetData[i].HARD_MIN8.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN8 = tempMillsheetData[i].HARD_MAX8;
                tempMillsheetData[i].HARD_MAX8 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN8.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN8 = "";
            }
            if (tempMillsheetData[i].HARD_MAX8.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX8 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX8.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX8 = tempMillsheetData[i].HARD_MIN8;
                tempMillsheetData[i].HARD_MIN8 = "";
            }

            if (tempMillsheetData[i].HARD_MIN9.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN9 = tempMillsheetData[i].HARD_MAX9;
                tempMillsheetData[i].HARD_MAX9 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN9.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN9 = "";
            }
            if (tempMillsheetData[i].HARD_MAX9.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX9 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX9.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX9 = tempMillsheetData[i].HARD_MIN9;
                tempMillsheetData[i].HARD_MIN9 = "";
            }

            if (tempMillsheetData[i].HARD_MIN10.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN10 = tempMillsheetData[i].HARD_MAX10;
                tempMillsheetData[i].HARD_MAX10 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN10.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN10 = "";
            }
            if (tempMillsheetData[i].HARD_MAX10.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX10 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX10.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX10 = tempMillsheetData[i].HARD_MIN10;
                tempMillsheetData[i].HARD_MIN10 = "";
            }

            if (tempMillsheetData[i].HARD_MIN11.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN11 = tempMillsheetData[i].HARD_MAX11;
                tempMillsheetData[i].HARD_MAX11 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN11.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN11 = "";
            }
            if (tempMillsheetData[i].HARD_MAX11.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX11 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX11.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX11 = tempMillsheetData[i].HARD_MIN11;
                tempMillsheetData[i].HARD_MIN11 = "";
            }

            if (tempMillsheetData[i].HARD_MIN12.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN12 = tempMillsheetData[i].HARD_MAX12;
                tempMillsheetData[i].HARD_MAX12 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN12.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN12 = "";
            }
            if (tempMillsheetData[i].HARD_MAX12.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX12 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX12.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX12 = tempMillsheetData[i].HARD_MIN12;
                tempMillsheetData[i].HARD_MIN12 = "";
            }

            if (tempMillsheetData[i].HARD_MIN13.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN13 = tempMillsheetData[i].HARD_MAX13;
                tempMillsheetData[i].HARD_MAX13 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN13.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN13 = "";
            }
            if (tempMillsheetData[i].HARD_MAX13.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX13 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX13.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX13 = tempMillsheetData[i].HARD_MIN13;
                tempMillsheetData[i].HARD_MIN13 = "";
            }

            if (tempMillsheetData[i].HARD_MIN14.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN14 = tempMillsheetData[i].HARD_MAX14;
                tempMillsheetData[i].HARD_MAX14 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN14.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN14 = "";
            }
            if (tempMillsheetData[i].HARD_MAX14.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX14 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX14.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX14 = tempMillsheetData[i].HARD_MIN14;
                tempMillsheetData[i].HARD_MIN14 = "";
            }

            if (tempMillsheetData[i].HARD_MIN15.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN15 = tempMillsheetData[i].HARD_MAX15;
                tempMillsheetData[i].HARD_MAX15 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN15.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN15 = "";
            }
            if (tempMillsheetData[i].HARD_MAX15.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX15 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX15.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX15 = tempMillsheetData[i].HARD_MIN15;
                tempMillsheetData[i].HARD_MIN15 = "";
            }

            if (tempMillsheetData[i].HARD_MIN16.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN16 = tempMillsheetData[i].HARD_MAX16;
                tempMillsheetData[i].HARD_MAX16 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN16.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN16 = "";
            }
            if (tempMillsheetData[i].HARD_MAX16.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX16 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX16.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX16 = tempMillsheetData[i].HARD_MIN16;
                tempMillsheetData[i].HARD_MIN16 = "";
            }

            if (tempMillsheetData[i].HARD_MIN17.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN17 = tempMillsheetData[i].HARD_MAX17;
                tempMillsheetData[i].HARD_MAX17 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN17.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN17 = "";
            }
            if (tempMillsheetData[i].HARD_MAX17.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX17 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX17.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX17 = tempMillsheetData[i].HARD_MIN17;
                tempMillsheetData[i].HARD_MIN17 = "";
            }

            if (tempMillsheetData[i].HARD_MIN18.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN18 = tempMillsheetData[i].HARD_MAX18;
                tempMillsheetData[i].HARD_MAX18 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN18.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN18 = "";
            }
            if (tempMillsheetData[i].HARD_MAX18.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX18 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX18.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX18 = tempMillsheetData[i].HARD_MIN18;
                tempMillsheetData[i].HARD_MIN18 = "";
            }

            if (tempMillsheetData[i].HARD_MIN19.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN19 = tempMillsheetData[i].HARD_MAX19;
                tempMillsheetData[i].HARD_MAX19 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN19.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN19 = "";
            }
            if (tempMillsheetData[i].HARD_MAX19.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX19 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX19.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX19 = tempMillsheetData[i].HARD_MIN19;
                tempMillsheetData[i].HARD_MIN19 = "";
            }

            if (tempMillsheetData[i].HARD_MIN20.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN20 = tempMillsheetData[i].HARD_MAX20;
                tempMillsheetData[i].HARD_MAX20 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN20.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN20 = "";
            }
            if (tempMillsheetData[i].HARD_MAX20.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX20 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX20.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX20 = tempMillsheetData[i].HARD_MIN20;
                tempMillsheetData[i].HARD_MIN20 = "";
            }

            if (tempMillsheetData[i].HARD_MIN21.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN21 = tempMillsheetData[i].HARD_MAX21;
                tempMillsheetData[i].HARD_MAX21 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN21.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN21 = "";
            }
            if (tempMillsheetData[i].HARD_MAX21.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX21 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX21.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX21 = tempMillsheetData[i].HARD_MIN21;
                tempMillsheetData[i].HARD_MIN21 = "";
            }

            if (tempMillsheetData[i].HARD_MIN22.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN22 = tempMillsheetData[i].HARD_MAX22;
                tempMillsheetData[i].HARD_MAX22 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN22.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN22 = "";
            }
            if (tempMillsheetData[i].HARD_MAX22.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX22 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX22.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX22 = tempMillsheetData[i].HARD_MIN22;
                tempMillsheetData[i].HARD_MIN22 = "";
            }
            if (tempMillsheetData[i].HARD_MIN23.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN23 = tempMillsheetData[i].HARD_MAX23;
                tempMillsheetData[i].HARD_MAX23 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN23.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN23 = "";
            }
            if (tempMillsheetData[i].HARD_MAX23.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX23 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX23.toUpperCase().includes("MA")) {
                
                tempMillsheetData[i].HARD_MAX23 = tempMillsheetData[i].HARD_MIN23;
                tempMillsheetData[i].HARD_MIN23 = "";
            }

            if (tempMillsheetData[i].HARD_MIN24.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN24 = tempMillsheetData[i].HARD_MAX24;
                tempMillsheetData[i].HARD_MAX24 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN24.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN24 = "";
            }
            if (tempMillsheetData[i].HARD_MAX24.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX24 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX24.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX24 = tempMillsheetData[i].HARD_MIN24;
                tempMillsheetData[i].HARD_MIN24 = "";
            }

            if (tempMillsheetData[i].HARD_MIN25.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MIN25 = tempMillsheetData[i].HARD_MAX25;
                tempMillsheetData[i].HARD_MAX25 = "";
            }
            else if (tempMillsheetData[i].HARD_MIN25.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MIN25 = "";
            }
            if (tempMillsheetData[i].HARD_MAX25.toUpperCase().includes("MI")) {
                tempMillsheetData[i].HARD_MAX25 = "";
            }
            else if (tempMillsheetData[i].HARD_MAX25.toUpperCase().includes("MA")) {
                tempMillsheetData[i].HARD_MAX25 = tempMillsheetData[i].HARD_MIN25;
                tempMillsheetData[i].HARD_MIN25 = "";
            }
            // EXAMPLE
            //HARD_MIN = MAX AND HARD_MAX= 50 THEN HARD_MIN= EMPTY
            //HARD_MIN = MIN AND HARD_MAX= 90 THEN HARD_MIN= 90 AND HARD_MAX=EMPTY
            //=================================================================================== ABOVE EX. APPLIED FOR MIN AND MAX FIELDS =======================================
            formData.append(`request[${k}].FORM_FILE`, tempMillsheetData[i].FORM_FILE);
            formData.append(`request[${k}].CULTURE_NAME`, $("#hdfCultureName").val());
            formData.append(`request[${k}].MAKER_CD`, $("#ddlMakerName").val());
            formData.append(`request[${k}].MAKER_NAME`, $("#ddlMakerName option:selected").text());
            formData.append(`request[${k}].RUN_CD`, runLogCD);
            formData.append(`request[${k}].KOUBAN`, tempMillsheetData[i].KOUBAN);
            formData.append(`request[${k}].CHARGE`, tempMillsheetData[i].CHARGE);
            formData.append(`request[${k}].MATL_NAME_MAKER`, tempMillsheetData[i].MATL_NAME_MAKER);
            formData.append(`request[${k}].DIM_D`, tempMillsheetData[i].DIM_D);
            formData.append(`request[${k}].LENGTH`, tempMillsheetData[i].LENGTH);
            formData.append(`request[${k}].NO_OF_UNITS`, tempMillsheetData[i].NO_OF_UNITS);
            formData.append(`request[${k}].UNIT`, tempMillsheetData[i].UNIT);
            formData.append(`request[${k}].QUANTITY`, tempMillsheetData[i].QUANTITY);
            formData.append(`request[${k}].GRAIN_SIZE_STD`, tempMillsheetData[i].GRAIN_SIZE_STD);
            formData.append(`request[${k}].GRAIN_SIZE_ACTUAL`, tempMillsheetData[i].GRAIN_SIZE_ACTUAL);
            formData.append(`request[${k}].HARDNESS_STD`, tempMillsheetData[i].HARDNESS_STD);
            formData.append(`request[${k}].HARDNESS_MIN`, tempMillsheetData[i].HARDNESS_MIN);
            formData.append(`request[${k}].HARDNESS_MAX`, tempMillsheetData[i].HARDNESS_MAX);
            formData.append(`request[${k}].HARDNESS_ACTUAL`, tempMillsheetData[i].HARDNESS_ACTUAL);
            formData.append(`request[${k}].CCNAME1`, tempMillsheetData[i].CCNAME1);
            formData.append(`request[${k}].CCNAME2`, tempMillsheetData[i].CCNAME2);
            formData.append(`request[${k}].CCNAME3`, tempMillsheetData[i].CCNAME3);
            formData.append(`request[${k}].CCNAME4`, tempMillsheetData[i].CCNAME4);
            formData.append(`request[${k}].CCNAME5`, tempMillsheetData[i].CCNAME5);
            formData.append(`request[${k}].CCNAME6`, tempMillsheetData[i].CCNAME6);
            formData.append(`request[${k}].CCNAME7`, tempMillsheetData[i].CCNAME7);
            formData.append(`request[${k}].CCNAME8`, tempMillsheetData[i].CCNAME8);
            formData.append(`request[${k}].CCNAME9`, tempMillsheetData[i].CCNAME9);
            formData.append(`request[${k}].CCNAME10`, tempMillsheetData[i].CCNAME10);
            formData.append(`request[${k}].CCNAME11`, tempMillsheetData[i].CCNAME11);
            formData.append(`request[${k}].CCNAME12`, tempMillsheetData[i].CCNAME12);
            formData.append(`request[${k}].CCNAME13`, tempMillsheetData[i].CCNAME13);
            formData.append(`request[${k}].CCNAME14`, tempMillsheetData[i].CCNAME14);
            formData.append(`request[${k}].CCNAME15`, tempMillsheetData[i].CCNAME15);
            formData.append(`request[${k}].CCNAME16`, tempMillsheetData[i].CCNAME16);
            formData.append(`request[${k}].CCNAME17`, tempMillsheetData[i].CCNAME17);
            formData.append(`request[${k}].CCNAME18`, tempMillsheetData[i].CCNAME18);
            formData.append(`request[${k}].CCNAME19`, tempMillsheetData[i].CCNAME19);
            formData.append(`request[${k}].CCNAME20`, tempMillsheetData[i].CCNAME20);
            formData.append(`request[${k}].CCNAME21`, tempMillsheetData[i].CCNAME21);
            formData.append(`request[${k}].CCNAME22`, tempMillsheetData[i].CCNAME22);
            formData.append(`request[${k}].CCNAME23`, tempMillsheetData[i].CCNAME23);
            formData.append(`request[${k}].CCNAME24`, tempMillsheetData[i].CCNAME24);
            formData.append(`request[${k}].CCNAME25`, tempMillsheetData[i].CCNAME25);
            formData.append(`request[${k}].CCRANGE1`, tempMillsheetData[i].CCRANGE1);
            formData.append(`request[${k}].CCRANGE2`, tempMillsheetData[i].CCRANGE2);
            formData.append(`request[${k}].CCRANGE3`, tempMillsheetData[i].CCRANGE3);
            formData.append(`request[${k}].CCRANGE4`, tempMillsheetData[i].CCRANGE4);
            formData.append(`request[${k}].CCRANGE5`, tempMillsheetData[i].CCRANGE5);
            formData.append(`request[${k}].CCRANGE6`, tempMillsheetData[i].CCRANGE6);
            formData.append(`request[${k}].CCRANGE7`, tempMillsheetData[i].CCRANGE7);
            formData.append(`request[${k}].CCRANGE8`, tempMillsheetData[i].CCRANGE8);
            formData.append(`request[${k}].CCRANGE9`, tempMillsheetData[i].CCRANGE9);
            formData.append(`request[${k}].CCRANGE10`, tempMillsheetData[i].CCRANGE10);
            formData.append(`request[${k}].CCRANGE11`, tempMillsheetData[i].CCRANGE11);
            formData.append(`request[${k}].CCRANGE12`, tempMillsheetData[i].CCRANGE12);
            formData.append(`request[${k}].CCRANGE13`, tempMillsheetData[i].CCRANGE13);
            formData.append(`request[${k}].CCRANGE14`, tempMillsheetData[i].CCRANGE14);
            formData.append(`request[${k}].CCRANGE15`, tempMillsheetData[i].CCRANGE15);
            formData.append(`request[${k}].CCRANGE16`, tempMillsheetData[i].CCRANGE16);
            formData.append(`request[${k}].CCRANGE17`, tempMillsheetData[i].CCRANGE17);
            formData.append(`request[${k}].CCRANGE18`, tempMillsheetData[i].CCRANGE18);
            formData.append(`request[${k}].CCRANGE19`, tempMillsheetData[i].CCRANGE19);
            formData.append(`request[${k}].CCRANGE20`, tempMillsheetData[i].CCRANGE20);
            formData.append(`request[${k}].CCRANGE21`, tempMillsheetData[i].CCRANGE21);
            formData.append(`request[${k}].CCRANGE22`, tempMillsheetData[i].CCRANGE22);
            formData.append(`request[${k}].CCRANGE23`, tempMillsheetData[i].CCRANGE23);
            formData.append(`request[${k}].CCRANGE24`, tempMillsheetData[i].CCRANGE24);
            formData.append(`request[${k}].CCRANGE25`, tempMillsheetData[i].CCRANGE25);
            formData.append(`request[${k}].CCMIN1`, tempMillsheetData[i].CCMIN1);
            formData.append(`request[${k}].CCMIN2`, tempMillsheetData[i].CCMIN2);
            formData.append(`request[${k}].CCMIN3`, tempMillsheetData[i].CCMIN3);
            formData.append(`request[${k}].CCMIN4`, tempMillsheetData[i].CCMIN4);
            formData.append(`request[${k}].CCMIN5`, tempMillsheetData[i].CCMIN5);
            formData.append(`request[${k}].CCMIN6`, tempMillsheetData[i].CCMIN6);
            formData.append(`request[${k}].CCMIN7`, tempMillsheetData[i].CCMIN7);
            formData.append(`request[${k}].CCMIN8`, tempMillsheetData[i].CCMIN8);
            formData.append(`request[${k}].CCMIN9`, tempMillsheetData[i].CCMIN9);
            formData.append(`request[${k}].CCMIN10`, tempMillsheetData[i].CCMIN10);
            formData.append(`request[${k}].CCMIN11`, tempMillsheetData[i].CCMIN11);
            formData.append(`request[${k}].CCMIN12`, tempMillsheetData[i].CCMIN12);
            formData.append(`request[${k}].CCMIN13`, tempMillsheetData[i].CCMIN13);
            formData.append(`request[${k}].CCMIN14`, tempMillsheetData[i].CCMIN14);
            formData.append(`request[${k}].CCMIN15`, tempMillsheetData[i].CCMIN15);
            formData.append(`request[${k}].CCMIN16`, tempMillsheetData[i].CCMIN16);
            formData.append(`request[${k}].CCMIN17`, tempMillsheetData[i].CCMIN17);
            formData.append(`request[${k}].CCMIN18`, tempMillsheetData[i].CCMIN18);
            formData.append(`request[${k}].CCMIN19`, tempMillsheetData[i].CCMIN19);
            formData.append(`request[${k}].CCMIN20`, tempMillsheetData[i].CCMIN20);
            formData.append(`request[${k}].CCMIN21`, tempMillsheetData[i].CCMIN21);
            formData.append(`request[${k}].CCMIN22`, tempMillsheetData[i].CCMIN22);
            formData.append(`request[${k}].CCMIN23`, tempMillsheetData[i].CCMIN23);
            formData.append(`request[${k}].CCMIN24`, tempMillsheetData[i].CCMIN24);
            formData.append(`request[${k}].CCMIN25`, tempMillsheetData[i].CCMIN25);
            formData.append(`request[${k}].CCMAX1`, tempMillsheetData[i].CCMAX1);
            formData.append(`request[${k}].CCMAX2`, tempMillsheetData[i].CCMAX2);
            formData.append(`request[${k}].CCMAX3`, tempMillsheetData[i].CCMAX3);
            formData.append(`request[${k}].CCMAX4`, tempMillsheetData[i].CCMAX4);
            formData.append(`request[${k}].CCMAX5`, tempMillsheetData[i].CCMAX5);
            formData.append(`request[${k}].CCMAX6`, tempMillsheetData[i].CCMAX6);
            formData.append(`request[${k}].CCMAX7`, tempMillsheetData[i].CCMAX7);
            formData.append(`request[${k}].CCMAX8`, tempMillsheetData[i].CCMAX8);
            formData.append(`request[${k}].CCMAX9`, tempMillsheetData[i].CCMAX9);
            formData.append(`request[${k}].CCMAX10`, tempMillsheetData[i].CCMAX10);
            formData.append(`request[${k}].CCMAX11`, tempMillsheetData[i].CCMAX11);
            formData.append(`request[${k}].CCMAX12`, tempMillsheetData[i].CCMAX12);
            formData.append(`request[${k}].CCMAX13`, tempMillsheetData[i].CCMAX13);
            formData.append(`request[${k}].CCMAX14`, tempMillsheetData[i].CCMAX14);
            formData.append(`request[${k}].CCMAX15`, tempMillsheetData[i].CCMAX15);
            formData.append(`request[${k}].CCMAX16`, tempMillsheetData[i].CCMAX16);
            formData.append(`request[${k}].CCMAX17`, tempMillsheetData[i].CCMAX17);
            formData.append(`request[${k}].CCMAX18`, tempMillsheetData[i].CCMAX18);
            formData.append(`request[${k}].CCMAX19`, tempMillsheetData[i].CCMAX19);
            formData.append(`request[${k}].CCMAX20`, tempMillsheetData[i].CCMAX20);
            formData.append(`request[${k}].CCMAX21`, tempMillsheetData[i].CCMAX21);
            formData.append(`request[${k}].CCMAX22`, tempMillsheetData[i].CCMAX22);
            formData.append(`request[${k}].CCMAX23`, tempMillsheetData[i].CCMAX23);
            formData.append(`request[${k}].CCMAX24`, tempMillsheetData[i].CCMAX24);
            formData.append(`request[${k}].CCMAX25`, tempMillsheetData[i].CCMAX25);
            formData.append(`request[${k}].CCACTUAL1`, tempMillsheetData[i].CCACTUAL1);
            formData.append(`request[${k}].CCACTUAL2`, tempMillsheetData[i].CCACTUAL2);
            formData.append(`request[${k}].CCACTUAL3`, tempMillsheetData[i].CCACTUAL3);
            formData.append(`request[${k}].CCACTUAL4`, tempMillsheetData[i].CCACTUAL4);
            formData.append(`request[${k}].CCACTUAL5`, tempMillsheetData[i].CCACTUAL5);
            formData.append(`request[${k}].CCACTUAL6`, tempMillsheetData[i].CCACTUAL6);
            formData.append(`request[${k}].CCACTUAL7`, tempMillsheetData[i].CCACTUAL7);
            formData.append(`request[${k}].CCACTUAL8`, tempMillsheetData[i].CCACTUAL8);
            formData.append(`request[${k}].CCACTUAL9`, tempMillsheetData[i].CCACTUAL9);
            formData.append(`request[${k}].CCACTUAL10`, tempMillsheetData[i].CCACTUAL10);
            formData.append(`request[${k}].CCACTUAL11`, tempMillsheetData[i].CCACTUAL11);
            formData.append(`request[${k}].CCACTUAL12`, tempMillsheetData[i].CCACTUAL12);
            formData.append(`request[${k}].CCACTUAL13`, tempMillsheetData[i].CCACTUAL13);
            formData.append(`request[${k}].CCACTUAL14`, tempMillsheetData[i].CCACTUAL14);
            formData.append(`request[${k}].CCACTUAL15`, tempMillsheetData[i].CCACTUAL15);
            formData.append(`request[${k}].CCACTUAL16`, tempMillsheetData[i].CCACTUAL16);
            formData.append(`request[${k}].CCACTUAL17`, tempMillsheetData[i].CCACTUAL17);
            formData.append(`request[${k}].CCACTUAL18`, tempMillsheetData[i].CCACTUAL18);
            formData.append(`request[${k}].CCACTUAL19`, tempMillsheetData[i].CCACTUAL19);
            formData.append(`request[${k}].CCACTUAL20`, tempMillsheetData[i].CCACTUAL20);
            formData.append(`request[${k}].CCACTUAL21`, tempMillsheetData[i].CCACTUAL21);
            formData.append(`request[${k}].CCACTUAL22`, tempMillsheetData[i].CCACTUAL22);
            formData.append(`request[${k}].CCACTUAL23`, tempMillsheetData[i].CCACTUAL23);
            formData.append(`request[${k}].CCACTUAL24`, tempMillsheetData[i].CCACTUAL24);
            formData.append(`request[${k}].CCACTUAL25`, tempMillsheetData[i].CCACTUAL25);
            formData.append(`request[${k}].HARD_STD1`, tempMillsheetData[i].HARD_STD1);
            formData.append(`request[${k}].HARD_STD2`, tempMillsheetData[i].HARD_STD2);
            formData.append(`request[${k}].HARD_STD3`, tempMillsheetData[i].HARD_STD3);
            formData.append(`request[${k}].HARD_STD4`, tempMillsheetData[i].HARD_STD4);
            formData.append(`request[${k}].HARD_STD5`, tempMillsheetData[i].HARD_STD5);
            formData.append(`request[${k}].HARD_STD6`, tempMillsheetData[i].HARD_STD6);
            formData.append(`request[${k}].HARD_STD7`, tempMillsheetData[i].HARD_STD7);
            formData.append(`request[${k}].HARD_STD8`, tempMillsheetData[i].HARD_STD8);
            formData.append(`request[${k}].HARD_STD9`, tempMillsheetData[i].HARD_STD9);
            formData.append(`request[${k}].HARD_STD10`, tempMillsheetData[i].HARD_STD10);
            formData.append(`request[${k}].HARD_STD11`, tempMillsheetData[i].HARD_STD11);
            formData.append(`request[${k}].HARD_STD12`, tempMillsheetData[i].HARD_STD12);
            formData.append(`request[${k}].HARD_STD13`, tempMillsheetData[i].HARD_STD13);
            formData.append(`request[${k}].HARD_STD14`, tempMillsheetData[i].HARD_STD14);
            formData.append(`request[${k}].HARD_STD15`, tempMillsheetData[i].HARD_STD15);
            formData.append(`request[${k}].HARD_STD16`, tempMillsheetData[i].HARD_STD16);
            formData.append(`request[${k}].HARD_STD17`, tempMillsheetData[i].HARD_STD17);
            formData.append(`request[${k}].HARD_STD18`, tempMillsheetData[i].HARD_STD18);
            formData.append(`request[${k}].HARD_STD19`, tempMillsheetData[i].HARD_STD19);
            formData.append(`request[${k}].HARD_STD20`, tempMillsheetData[i].HARD_STD20);
            formData.append(`request[${k}].HARD_STD21`, tempMillsheetData[i].HARD_STD21);
            formData.append(`request[${k}].HARD_STD22`, tempMillsheetData[i].HARD_STD22);
            formData.append(`request[${k}].HARD_STD23`, tempMillsheetData[i].HARD_STD23);
            formData.append(`request[${k}].HARD_STD24`, tempMillsheetData[i].HARD_STD24);
            formData.append(`request[${k}].HARD_STD25`, tempMillsheetData[i].HARD_STD25);
            formData.append(`request[${k}].HARD_MIN1`, tempMillsheetData[i].HARD_MIN1);
            formData.append(`request[${k}].HARD_MIN2`, tempMillsheetData[i].HARD_MIN2);
            formData.append(`request[${k}].HARD_MIN3`, tempMillsheetData[i].HARD_MIN3);
            formData.append(`request[${k}].HARD_MIN4`, tempMillsheetData[i].HARD_MIN4);
            formData.append(`request[${k}].HARD_MIN5`, tempMillsheetData[i].HARD_MIN5);
            formData.append(`request[${k}].HARD_MIN6`, tempMillsheetData[i].HARD_MIN6);
            formData.append(`request[${k}].HARD_MIN7`, tempMillsheetData[i].HARD_MIN7);
            formData.append(`request[${k}].HARD_MIN8`, tempMillsheetData[i].HARD_MIN8);
            formData.append(`request[${k}].HARD_MIN9`, tempMillsheetData[i].HARD_MIN9);
            formData.append(`request[${k}].HARD_MIN10`, tempMillsheetData[i].HARD_MIN10);
            formData.append(`request[${k}].HARD_MIN11`, tempMillsheetData[i].HARD_MIN11);
            formData.append(`request[${k}].HARD_MIN12`, tempMillsheetData[i].HARD_MIN12);
            formData.append(`request[${k}].HARD_MIN13`, tempMillsheetData[i].HARD_MIN13);
            formData.append(`request[${k}].HARD_MIN14`, tempMillsheetData[i].HARD_MIN14);
            formData.append(`request[${k}].HARD_MIN15`, tempMillsheetData[i].HARD_MIN15);
            formData.append(`request[${k}].HARD_MIN16`, tempMillsheetData[i].HARD_MIN16);
            formData.append(`request[${k}].HARD_MIN17`, tempMillsheetData[i].HARD_MIN17);
            formData.append(`request[${k}].HARD_MIN18`, tempMillsheetData[i].HARD_MIN18);
            formData.append(`request[${k}].HARD_MIN19`, tempMillsheetData[i].HARD_MIN19);
            formData.append(`request[${k}].HARD_MIN20`, tempMillsheetData[i].HARD_MIN20);
            formData.append(`request[${k}].HARD_MIN21`, tempMillsheetData[i].HARD_MIN21);
            formData.append(`request[${k}].HARD_MIN22`, tempMillsheetData[i].HARD_MIN22);
            formData.append(`request[${k}].HARD_MIN23`, tempMillsheetData[i].HARD_MIN23);
            formData.append(`request[${k}].HARD_MIN24`, tempMillsheetData[i].HARD_MIN24);
            formData.append(`request[${k}].HARD_MIN25`, tempMillsheetData[i].HARD_MIN25);
            formData.append(`request[${k}].HARD_MAX1`, tempMillsheetData[i].HARD_MAX1);
            formData.append(`request[${k}].HARD_MAX2`, tempMillsheetData[i].HARD_MAX2);
            formData.append(`request[${k}].HARD_MAX3`, tempMillsheetData[i].HARD_MAX3);
            formData.append(`request[${k}].HARD_MAX4`, tempMillsheetData[i].HARD_MAX4);
            formData.append(`request[${k}].HARD_MAX5`, tempMillsheetData[i].HARD_MAX5);
            formData.append(`request[${k}].HARD_MAX6`, tempMillsheetData[i].HARD_MAX6);
            formData.append(`request[${k}].HARD_MAX7`, tempMillsheetData[i].HARD_MAX7);
            formData.append(`request[${k}].HARD_MAX8`, tempMillsheetData[i].HARD_MAX8);
            formData.append(`request[${k}].HARD_MAX9`, tempMillsheetData[i].HARD_MAX9);
            formData.append(`request[${k}].HARD_MAX10`, tempMillsheetData[i].HARD_MAX10);
            formData.append(`request[${k}].HARD_MAX11`, tempMillsheetData[i].HARD_MAX11);
            formData.append(`request[${k}].HARD_MAX12`, tempMillsheetData[i].HARD_MAX12);
            formData.append(`request[${k}].HARD_MAX13`, tempMillsheetData[i].HARD_MAX13);
            formData.append(`request[${k}].HARD_MAX14`, tempMillsheetData[i].HARD_MAX14);
            formData.append(`request[${k}].HARD_MAX15`, tempMillsheetData[i].HARD_MAX15);
            formData.append(`request[${k}].HARD_MAX16`, tempMillsheetData[i].HARD_MAX16);
            formData.append(`request[${k}].HARD_MAX17`, tempMillsheetData[i].HARD_MAX17);
            formData.append(`request[${k}].HARD_MAX18`, tempMillsheetData[i].HARD_MAX18);
            formData.append(`request[${k}].HARD_MAX19`, tempMillsheetData[i].HARD_MAX19);
            formData.append(`request[${k}].HARD_MAX20`, tempMillsheetData[i].HARD_MAX20);
            formData.append(`request[${k}].HARD_MAX21`, tempMillsheetData[i].HARD_MAX21);
            formData.append(`request[${k}].HARD_MAX22`, tempMillsheetData[i].HARD_MAX22);
            formData.append(`request[${k}].HARD_MAX23`, tempMillsheetData[i].HARD_MAX23);
            formData.append(`request[${k}].HARD_MAX24`, tempMillsheetData[i].HARD_MAX24);
            formData.append(`request[${k}].HARD_MAX25`, tempMillsheetData[i].HARD_MAX25);
            formData.append(`request[${k}].HARD_ACTUAL1`, tempMillsheetData[i].HARD_ACTUAL1);
            formData.append(`request[${k}].HARD_ACTUAL2`, tempMillsheetData[i].HARD_ACTUAL2);
            formData.append(`request[${k}].HARD_ACTUAL3`, tempMillsheetData[i].HARD_ACTUAL3);
            formData.append(`request[${k}].HARD_ACTUAL4`, tempMillsheetData[i].HARD_ACTUAL4);
            formData.append(`request[${k}].HARD_ACTUAL5`, tempMillsheetData[i].HARD_ACTUAL5);
            formData.append(`request[${k}].HARD_ACTUAL6`, tempMillsheetData[i].HARD_ACTUAL6);
            formData.append(`request[${k}].HARD_ACTUAL7`, tempMillsheetData[i].HARD_ACTUAL7);
            formData.append(`request[${k}].HARD_ACTUAL8`, tempMillsheetData[i].HARD_ACTUAL8);
            formData.append(`request[${k}].HARD_ACTUAL9`, tempMillsheetData[i].HARD_ACTUAL9);
            formData.append(`request[${k}].HARD_ACTUAL10`, tempMillsheetData[i].HARD_ACTUAL10);
            formData.append(`request[${k}].HARD_ACTUAL11`, tempMillsheetData[i].HARD_ACTUAL11);
            formData.append(`request[${k}].HARD_ACTUAL12`, tempMillsheetData[i].HARD_ACTUAL12);
            formData.append(`request[${k}].HARD_ACTUAL13`, tempMillsheetData[i].HARD_ACTUAL13);
            formData.append(`request[${k}].HARD_ACTUAL14`, tempMillsheetData[i].HARD_ACTUAL14);
            formData.append(`request[${k}].HARD_ACTUAL15`, tempMillsheetData[i].HARD_ACTUAL15);
            formData.append(`request[${k}].HARD_ACTUAL16`, tempMillsheetData[i].HARD_ACTUAL16);
            formData.append(`request[${k}].HARD_ACTUAL17`, tempMillsheetData[i].HARD_ACTUAL17);
            formData.append(`request[${k}].HARD_ACTUAL18`, tempMillsheetData[i].HARD_ACTUAL18);
            formData.append(`request[${k}].HARD_ACTUAL19`, tempMillsheetData[i].HARD_ACTUAL19);
            formData.append(`request[${k}].HARD_ACTUAL20`, tempMillsheetData[i].HARD_ACTUAL20);
            formData.append(`request[${k}].HARD_ACTUAL21`, tempMillsheetData[i].HARD_ACTUAL21);
            formData.append(`request[${k}].HARD_ACTUAL22`, tempMillsheetData[i].HARD_ACTUAL22);
            formData.append(`request[${k}].HARD_ACTUAL23`, tempMillsheetData[i].HARD_ACTUAL23);
            formData.append(`request[${k}].HARD_ACTUAL24`, tempMillsheetData[i].HARD_ACTUAL24);
            formData.append(`request[${k}].HARD_ACTUAL25`, tempMillsheetData[i].HARD_ACTUAL25);
            formData.append(`request[${k}].USER_ID`, $("#hdfLoginId").val().trim());
            k = k + 1;
        }
    }
}
function IsMillSheetAlreadySaved()
{
    var millsheet_ = 
        JSON.stringify({
            MAKER_CD: $("#ddlMakerName").val(),
            MAKER_NAME: $("#ddlMakerName option:selected").text(),
            KOUBAN: $("#txtkouban").val(),
            CHARGE: $("#txtcharge").val(),
            MATL_NAME_MAKER: $("#txtTypeofSteel").val(),
            DIM_D: $("#txtDimension").val() ? $("#txtDimension").val() : null,
            LENGTH: $("#txtLength").val() ? $("#txtLength").val() : null,
            NO_OF_UNITS: $("#txtNoOfUnits").val() ? $("#txtNoOfUnits").val() : null,
            UNIT: $("#ddlUnitType").val(),
            QUANTITY: $("#txtWeightWithoutKG").val() ? $("#txtWeightWithoutKG").val() : null,
            GRAIN_SIZE_STD: $("#txtGainsizeSTD").val() ? $("#txtGainsizeSTD").val() : null,
            GRAIN_SIZE_ACTUAL: $("#txtGainSizeActual").val() ? $("#txtGainSizeActual").val() : null,
            HARDNESS_STD: $("#txtHardnessmakerSTD").val(),
            HARDNESS_MIN: $("#txtHardnessMin").val(),
            HARDNESS_MAX: $("#txtHardnessMax").val(),
            HARDNESS_ACTUAL: $("#txtHardnessMakerActual").val(),
            CCNAME1: $("#txtCCName1").val(),
            CCNAME2: $("#txtCCName2").val(),
            CCNAME3: $("#txtCCName3").val(),
            CCNAME4: $("#txtCCName4").val(),
            CCNAME5: $("#txtCCName5").val(),
            CCNAME6: $("#txtCCName6").val(),
            CCNAME7: $("#txtCCName7").val(),
            CCNAME8: $("#txtCCName8").val(),
            CCNAME9: $("#txtCCName9").val(),
            CCNAME10: $("#txtCCName10").val(),
            CCNAME11: $("#txtCCName11").val(),
            CCNAME12: $("#txtCCName12").val(),
            CCNAME13: $("#txtCCName13").val(),
            CCNAME14: $("#txtCCName14").val(),
            CCNAME15: $("#txtCCName15").val(),
            CCNAME16: $("#txtCCName16").val(),
            CCNAME17: $("#txtCCName17").val(),
            CCNAME18: $("#txtCCName18").val(),
            CCNAME19: $("#txtCCName19").val(),
            CCNAME20: $("#txtCCName20").val(),
            CCNAME21: $("#txtCCName21").val(),
            CCNAME22: $("#txtCCName22").val(),
            CCNAME23: $("#txtCCName23").val(),
            CCNAME24: $("#txtCCName24").val(),
            CCNAME25: $("#txtCCName25").val(),
            CCRANGE1: $("#txtCCRange1").val() ? $("#txtCCRange1").val() : null,
            CCRANGE2: $("#txtCCRange2").val() ? $("#txtCCRange2").val() : null,
            CCRANGE3: $("#txtCCRange3").val() ? $("#txtCCRange3").val() : null,
            CCRANGE4: $("#txtCCRange4").val() ? $("#txtCCRange4").val() : null,
            CCRANGE5: $("#txtCCRange5").val() ? $("#txtCCRange5").val() : null,
            CCRANGE6: $("#txtCCRange6").val() ? $("#txtCCRange6").val() : null,
            CCRANGE7: $("#txtCCRange7").val() ? $("#txtCCRange7").val() : null,
            CCRANGE8: $("#txtCCRange8").val() ? $("#txtCCRange8").val() : null,
            CCRANGE9: $("#txtCCRange9").val() ? $("#txtCCRange9").val() : null,
            CCRANGE10: $("#txtCCRange10").val() ? $("#txtCCRange10").val() : null,
            CCRANGE11: $("#txtCCRange11").val() ? $("#txtCCRange11").val() : null,
            CCRANGE12: $("#txtCCRange12").val() ? $("#txtCCRange12").val() : null,
            CCRANGE13: $("#txtCCRange13").val() ? $("#txtCCRange13").val() : null,
            CCRANGE14: $("#txtCCRange14").val() ? $("#txtCCRange14").val() : null,
            CCRANGE15: $("#txtCCRange15").val() ? $("#txtCCRange15").val() : null,
            CCRANGE16: $("#txtCCRange16").val() ? $("#txtCCRange16").val() : null,
            CCRANGE17: $("#txtCCRange17").val() ? $("#txtCCRange17").val() : null,
            CCRANGE18: $("#txtCCRange18").val() ? $("#txtCCRange18").val() : null,
            CCRANGE19: $("#txtCCRange19").val() ? $("#txtCCRange19").val() : null,
            CCRANGE20: $("#txtCCRange20").val() ? $("#txtCCRange20").val() : null,
            CCRANGE21: $("#txtCCRange21").val() ? $("#txtCCRange21").val() : null,
            CCRANGE22: $("#txtCCRange22").val() ? $("#txtCCRange22").val() : null,
            CCRANGE23: $("#txtCCRange23").val() ? $("#txtCCRange23").val() : null,
            CCRANGE24: $("#txtCCRange24").val() ? $("#txtCCRange24").val() : null,
            CCRANGE25: $("#txtCCRange25").val() ? $("#txtCCRange25").val() : null,
            CCMIN1: parseInt($("#txtMin1").val()) > 0 ? $("#txtMin1").val() : null,
            CCMIN2: parseInt($("#txtMin2").val()) > 0 ? $("#txtMin2").val() : null,
            CCMIN3: parseInt($("#txtMin3").val()) > 0 ? $("#txtMin3").val() : null,
            CCMIN4: parseInt($("#txtMin4").val()) > 0 ? $("#txtMin4").val() : null,
            CCMIN5: parseInt($("#txtMin5").val()) > 0 ? $("#txtMin5").val() : null,
            CCMIN6: parseInt($("#txtMin6").val()) > 0 ? $("#txtMin6").val() : null,
            CCMIN7: parseInt($("#txtMin7").val()) > 0 ? $("#txtMin7").val() : null,
            CCMIN8: parseInt($("#txtMin8").val()) > 0 ? $("#txtMin8").val() : null,
            CCMIN9: parseInt($("#txtMin9").val()) > 0 ? $("#txtMin9").val() : null,
            CCMIN10: parseInt($("#txtMin10").val()) > 0 ? $("#txtMin10").val() : null,
            CCMIN11: parseInt($("#txtMin11").val()) > 0 ? $("#txtMin11").val() : null,
            CCMIN12: parseInt($("#txtMin12").val()) > 0 ? $("#txtMin12").val() : null,
            CCMIN13: parseInt($("#txtMin13").val()) > 0 ? $("#txtMin13").val() : null,
            CCMIN14: parseInt($("#txtMin14").val()) > 0 ? $("#txtMin14").val() : null,
            CCMIN15: parseInt($("#txtMin15").val()) > 0 ? $("#txtMin15").val() : null,
            CCMIN16: parseInt($("#txtMin16").val()) > 0 ? $("#txtMin16").val() : null,
            CCMIN17: parseInt($("#txtMin17").val()) > 0 ? $("#txtMin17").val() : null,
            CCMIN18: parseInt($("#txtMin18").val()) > 0 ? $("#txtMin18").val() : null,
            CCMIN19: parseInt($("#txtMin19").val()) > 0 ? $("#txtMin19").val() : null,
            CCMIN20: parseInt($("#txtMin20").val()) > 0 ? $("#txtMin20").val() : null,
            CCMIN21: parseInt($("#txtMin21").val()) > 0 ? $("#txtMin21").val() : null,
            CCMIN22: parseInt($("#txtMin22").val()) > 0 ? $("#txtMin22").val() : null,
            CCMIN23: parseInt($("#txtMin23").val()) > 0 ? $("#txtMin23").val() : null,
            CCMIN24: parseInt($("#txtMin24").val()) > 0 ? $("#txtMin24").val() : null,
            CCMIN25: parseInt($("#txtMin25").val()) > 0 ? $("#txtMin25").val() : null,
            CCMAX1: parseInt($("#txtMax1").val()) > 0 ? $("#txtMax1").val() : null,
            CCMAX2: parseInt($("#txtMax2").val()) > 0 ? $("#txtMax2").val() : null,
            CCMAX3: parseInt($("#txtMax3").val()) > 0 ? $("#txtMax3").val() : null,
            CCMAX4: parseInt($("#txtMax4").val()) > 0 ? $("#txtMax4").val() : null,
            CCMAX5: parseInt($("#txtMax5").val()) > 0 ? $("#txtMax5").val() : null,
            CCMAX6: parseInt($("#txtMax6").val()) > 0 ? $("#txtMax6").val() : null,
            CCMAX7: parseInt($("#txtMax7").val()) > 0 ? $("#txtMax7").val() : null,
            CCMAX8: parseInt($("#txtMax8").val()) > 0 ? $("#txtMax8").val() : null,
            CCMAX9: parseInt($("#txtMax9").val()) > 0 ? $("#txtMax9").val() : null,
            CCMAX10: parseInt($("#txtMax10").val()) > 0 ? $("#txtMax10").val() : null,
            CCMAX11: parseInt($("#txtMax11").val()) > 0 ? $("#txtMax11").val() : null,
            CCMAX12: parseInt($("#txtMax12").val()) > 0 ? $("#txtMax12").val() : null,
            CCMAX13: parseInt($("#txtMax13").val()) > 0 ? $("#txtMax13").val() : null,
            CCMAX14: parseInt($("#txtMax14").val()) > 0 ? $("#txtMax14").val() : null,
            CCMAX15: parseInt($("#txtMax15").val()) > 0 ? $("#txtMax15").val() : null,
            CCMAX16: parseInt($("#txtMax16").val()) > 0 ? $("#txtMax16").val() : null,
            CCMAX17: parseInt($("#txtMax17").val()) > 0 ? $("#txtMax17").val() : null,
            CCMAX18: parseInt($("#txtMax18").val()) > 0 ? $("#txtMax18").val() : null,
            CCMAX19: parseInt($("#txtMax19").val()) > 0 ? $("#txtMax19").val() : null,
            CCMAX20: parseInt($("#txtMax20").val()) > 0 ? $("#txtMax20").val() : null,
            CCMAX21: parseInt($("#txtMax21").val()) > 0 ? $("#txtMax21").val() : null,
            CCMAX22: parseInt($("#txtMax22").val()) > 0 ? $("#txtMax22").val() : null,
            CCMAX23: parseInt($("#txtMax23").val()) > 0 ? $("#txtMax23").val() : null,
            CCMAX24: parseInt($("#txtMax24").val()) > 0 ? $("#txtMax24").val() : null,
            CCMAX25: parseInt($("#txtMax25").val()) > 0 ? $("#txtMax25").val() : null,
            CCACTUAL1: $("#txtCCActual1").val() ? $("#txtCCActual1").val() : null,
            CCACTUAL2: $("#txtCCActual2").val() ? $("#txtCCActual2").val() : null,
            CCACTUAL3: $("#txtCCActual3").val() ? $("#txtCCActual3").val() : null,
            CCACTUAL4: $("#txtCCActual4").val() ? $("#txtCCActual4").val() : null,
            CCACTUAL5: $("#txtCCActual5").val() ? $("#txtCCActual5").val() : null,
            CCACTUAL6: $("#txtCCActual6").val() ? $("#txtCCActual6").val() : null,
            CCACTUAL7: $("#txtCCActual7").val() ? $("#txtCCActual7").val() : null,
            CCACTUAL8: $("#txtCCActual8").val() ? $("#txtCCActual8").val() : null,
            CCACTUAL9: $("#txtCCActual9").val() ? $("#txtCCActual9").val() : null,
            CCACTUAL10: $("#txtCCActual10").val() ? $("#txtCCActual10").val() : null,
            CCACTUAL11: $("#txtCCActual11").val() ? $("#txtCCActual11").val() : null,
            CCACTUAL12: $("#txtCCActual12").val() ? $("#txtCCActual12").val() : null,
            CCACTUAL13: $("#txtCCActual13").val() ? $("#txtCCActual13").val() : null,
            CCACTUAL14: $("#txtCCActual14").val() ? $("#txtCCActual14").val() : null,
            CCACTUAL15: $("#txtCCActual15").val() ? $("#txtCCActual15").val() : null,
            CCACTUAL16: $("#txtCCActual16").val() ? $("#txtCCActual16").val() : null,
            CCACTUAL17: $("#txtCCActual17").val() ? $("#txtCCActual17").val() : null,
            CCACTUAL18: $("#txtCCActual18").val() ? $("#txtCCActual18").val() : null,
            CCACTUAL19: $("#txtCCActual19").val() ? $("#txtCCActual19").val() : null,
            CCACTUAL20: $("#txtCCActual20").val() ? $("#txtCCActual20").val() : null,
            CCACTUAL21: $("#txtCCActual21").val() ? $("#txtCCActual21").val() : null,
            CCACTUAL22: $("#txtCCActual22").val() ? $("#txtCCActual22").val() : null,
            CCACTUAL23: $("#txtCCActual23").val() ? $("#txtCCActual23").val() : null,
            CCACTUAL24: $("#txtCCActual24").val() ? $("#txtCCActual24").val() : null,
            CCACTUAL25: $("#txtCCActual25").val() ? $("#txtCCActual25").val() : null,
            HARD_STD1: $("#txtHardSTD1").val() ? $("#txtHardSTD1").val() : null,
            HARD_STD2: $("#txtHardSTD2").val() ? $("#txtHardSTD2").val() : null,
            HARD_STD3: $("#txtHardSTD3").val() ? $("#txtHardSTD3").val() : null,
            HARD_STD4: $("#txtHardSTD4").val() ? $("#txtHardSTD4").val() : null,
            HARD_STD5: $("#txtHardSTD5").val() ? $("#txtHardSTD5").val() : null,
            HARD_STD6: $("#txtHardSTD6").val() ? $("#txtHardSTD6").val() : null,
            HARD_STD7: $("#txtHardSTD7").val() ? $("#txtHardSTD7").val() : null,
            HARD_STD8: $("#txtHardSTD8").val() ? $("#txtHardSTD8").val() : null,
            HARD_STD9: $("#txtHardSTD9").val() ? $("#txtHardSTD9").val() : null,
            HARD_STD10: $("#txtHardSTD10").val() ? $("#txtHardSTD10").val() : null,
            HARD_STD11: $("#txtHardSTD11").val() ? $("#txtHardSTD11").val() : null,
            HARD_STD12: $("#txtHardSTD12").val() ? $("#txtHardSTD12").val() : null,
            HARD_STD13: $("#txtHardSTD13").val() ? $("#txtHardSTD13").val() : null,
            HARD_STD14: $("#txtHardSTD14").val() ? $("#txtHardSTD14").val() : null,
            HARD_STD15: $("#txtHardSTD15").val() ? $("#txtHardSTD15").val() : null,
            HARD_STD16: $("#txtHardSTD16").val() ? $("#txtHardSTD16").val() : null,
            HARD_STD17: $("#txtHardSTD17").val() ? $("#txtHardSTD17").val() : null,
            HARD_STD18: $("#txtHardSTD18").val() ? $("#txtHardSTD18").val() : null,
            HARD_STD19: $("#txtHardSTD19").val() ? $("#txtHardSTD19").val() : null,
            HARD_STD20: $("#txtHardSTD20").val() ? $("#txtHardSTD20").val() : null,
            HARD_STD21: $("#txtHardSTD21").val() ? $("#txtHardSTD21").val() : null,
            HARD_STD22: $("#txtHardSTD22").val() ? $("#txtHardSTD22").val() : null,
            HARD_STD23: $("#txtHardSTD23").val() ? $("#txtHardSTD23").val() : null,
            HARD_STD24: $("#txtHardSTD24").val() ? $("#txtHardSTD24").val() : null,
            HARD_STD25: $("#txtHardSTD25").val() ? $("#txtHardSTD25").val() : null,
            HARD_MIN1: parseInt($("#txtHardMin1").val()) > 0 ? $("#txtHardMin1").val() : null,
            HARD_MIN2: parseInt($("#txtHardMin2").val()) > 0 ? $("#txtHardMin2").val() : null,
            HARD_MIN3: parseInt($("#txtHardMin3").val()) > 0 ? $("#txtHardMin3").val() : null,
            HARD_MIN4: parseInt($("#txtHardMin4").val()) > 0 ? $("#txtHardMin4").val() : null,
            HARD_MIN5: parseInt($("#txtHardMin5").val()) > 0 ? $("#txtHardMin5").val() : null,
            HARD_MIN6: parseInt($("#txtHardMin6").val()) > 0 ? $("#txtHardMin6").val() : null,
            HARD_MIN7: parseInt($("#txtHardMin7").val()) > 0 ? $("#txtHardMin7").val() : null,
            HARD_MIN8: parseInt($("#txtHardMin8").val()) > 0 ? $("#txtHardMin8").val() : null,
            HARD_MIN9: parseInt($("#txtHardMin9").val()) > 0 ? $("#txtHardMin9").val() : null,
            HARD_MIN10: parseInt($("#txtHardMin10").val()) > 0 ? $("#txtHardMin10").val() : null,
            HARD_MIN11: parseInt($("#txtHardMin11").val()) > 0 ? $("#txtHardMin11").val() : null,
            HARD_MIN12: parseInt($("#txtHardMin12").val()) > 0 ? $("#txtHardMin12").val() : null,
            HARD_MIN13: parseInt($("#txtHardMin13").val()) > 0 ? $("#txtHardMin13").val() : null,
            HARD_MIN14: parseInt($("#txtHardMin14").val()) > 0 ? $("#txtHardMin14").val() : null,
            HARD_MIN15: parseInt($("#txtHardMin15").val()) > 0 ? $("#txtHardMin15").val() : null,
            HARD_MIN16: parseInt($("#txtHardMin16").val()) > 0 ? $("#txtHardMin16").val() : null,
            HARD_MIN17: parseInt($("#txtHardMin17").val()) > 0 ? $("#txtHardMin17").val() : null,
            HARD_MIN18: parseInt($("#txtHardMin18").val()) > 0 ? $("#txtHardMin18").val() : null,
            HARD_MIN19: parseInt($("#txtHardMin19").val()) > 0 ? $("#txtHardMin19").val() : null,
            HARD_MIN20: parseInt($("#txtHardMin20").val()) > 0 ? $("#txtHardMin20").val() : null,
            HARD_MIN21: parseInt($("#txtHardMin21").val()) > 0 ? $("#txtHardMin21").val() : null,
            HARD_MIN22: parseInt($("#txtHardMin22").val()) > 0 ? $("#txtHardMin22").val() : null,
            HARD_MIN23: parseInt($("#txtHardMin23").val()) > 0 ? $("#txtHardMin23").val() : null,
            HARD_MIN24: parseInt($("#txtHardMin24").val()) > 0 ? $("#txtHardMin24").val() : null,
            HARD_MIN25: parseInt($("#txtHardMin25").val()) > 0 ? $("#txtHardMin25").val() : null,
            HARD_MAX1: parseInt($("#txtHardMax1").val()) > 0 ? $("#txtHardMax1").val() : null,
            HARD_MAX2: parseInt($("#txtHardMax2").val()) > 0 ? $("#txtHardMax2").val() : null,
            HARD_MAX3: parseInt($("#txtHardMax3").val()) > 0 ? $("#txtHardMax3").val() : null,
            HARD_MAX4: parseInt($("#txtHardMax4").val()) > 0 ? $("#txtHardMax4").val() : null,
            HARD_MAX5: parseInt($("#txtHardMax5").val()) > 0 ? $("#txtHardMax5").val() : null,
            HARD_MAX6: parseInt($("#txtHardMax6").val()) > 0 ? $("#txtHardMax6").val() : null,
            HARD_MAX7: parseInt($("#txtHardMax7").val()) > 0 ? $("#txtHardMax7").val() : null,
            HARD_MAX8: parseInt($("#txtHardMax8").val()) > 0 ? $("#txtHardMax8").val() : null,
            HARD_MAX9: parseInt($("#txtHardMax9").val()) > 0 ? $("#txtHardMax9").val() : null,
            HARD_MAX10: parseInt($("#txtHardMax10").val()) > 0 ? $("#txtHardMax10").val() : null,
            HARD_MAX11: parseInt($("#txtHardMax11").val()) > 0 ? $("#txtHardMax11").val() : null,
            HARD_MAX12: parseInt($("#txtHardMax12").val()) > 0 ? $("#txtHardMax12").val() : null,
            HARD_MAX13: parseInt($("#txtHardMax13").val()) > 0 ? $("#txtHardMax13").val() : null,
            HARD_MAX14: parseInt($("#txtHardMax14").val()) > 0 ? $("#txtHardMax14").val() : null,
            HARD_MAX15: parseInt($("#txtHardMax15").val()) > 0 ? $("#txtHardMax15").val() : null,
            HARD_MAX16: parseInt($("#txtHardMax16").val()) > 0 ? $("#txtHardMax16").val() : null,
            HARD_MAX17: parseInt($("#txtHardMax17").val()) > 0 ? $("#txtHardMax17").val() : null,
            HARD_MAX18: parseInt($("#txtHardMax18").val()) > 0 ? $("#txtHardMax18").val() : null,
            HARD_MAX19: parseInt($("#txtHardMax19").val()) > 0 ? $("#txtHardMax19").val() : null,
            HARD_MAX20: parseInt($("#txtHardMax20").val()) > 0 ? $("#txtHardMax20").val() : null,
            HARD_MAX21: parseInt($("#txtHardMax21").val()) > 0 ? $("#txtHardMax21").val() : null,
            HARD_MAX22: parseInt($("#txtHardMax22").val()) > 0 ? $("#txtHardMax22").val() : null,
            HARD_MAX23: parseInt($("#txtHardMax23").val()) > 0 ? $("#txtHardMax23").val() : null,
            HARD_MAX24: parseInt($("#txtHardMax24").val()) > 0 ? $("#txtHardMax24").val() : null,
            HARD_MAX25: parseInt($("#txtHardMax25").val()) > 0 ? $("#txtHardMax25").val() : null,
            HARD_ACTUAL1: $("#txtHardActual1").val() ? $("#txtHardActual1").val() : null,
            HARD_ACTUAL2: $("#txtHardActual2").val() ? $("#txtHardActual2").val() : null,
            HARD_ACTUAL3: $("#txtHardActual3").val() ? $("#txtHardActual3").val() : null,
            HARD_ACTUAL4: $("#txtHardActual4").val() ? $("#txtHardActual4").val() : null,
            HARD_ACTUAL5: $("#txtHardActual5").val() ? $("#txtHardActual5").val() : null,
            HARD_ACTUAL6: $("#txtHardActual6").val() ? $("#txtHardActual6").val() : null,
            HARD_ACTUAL7: $("#txtHardActual7").val() ? $("#txtHardActual7").val() : null,
            HARD_ACTUAL8: $("#txtHardActual8").val() ? $("#txtHardActual8").val() : null,
            HARD_ACTUAL9: $("#txtHardActual9").val() ? $("#txtHardActual9").val() : null,
            HARD_ACTUAL10: $("#txtHardActual10").val() ? $("#txtHardActual10").val() : null,
            HARD_ACTUAL11: $("#txtHardActual11").val() ? $("#txtHardActual11").val() : null,
            HARD_ACTUAL12: $("#txtHardActual12").val() ? $("#txtHardActual12").val() : null,
            HARD_ACTUAL13: $("#txtHardActual13").val() ? $("#txtHardActual13").val() : null,
            HARD_ACTUAL14: $("#txtHardActual14").val() ? $("#txtHardActual14").val() : null,
            HARD_ACTUAL15: $("#txtHardActual15").val() ? $("#txtHardActual15").val() : null,
            HARD_ACTUAL16: $("#txtHardActual16").val() ? $("#txtHardActual16").val() : null,
            HARD_ACTUAL17: $("#txtHardActual17").val() ? $("#txtHardActual17").val() : null,
            HARD_ACTUAL18: $("#txtHardActual18").val() ? $("#txtHardActual18").val() : null,
            HARD_ACTUAL19: $("#txtHardActual19").val() ? $("#txtHardActual19").val() : null,
            HARD_ACTUAL20: $("#txtHardActual20").val() ? $("#txtHardActual20").val() : null,
            HARD_ACTUAL21: $("#txtHardActual21").val() ? $("#txtHardActual21").val() : null,
            HARD_ACTUAL22: $("#txtHardActual22").val() ? $("#txtHardActual22").val() : null,
            HARD_ACTUAL23: $("#txtHardActual23").val() ? $("#txtHardActual23").val() : null,
            HARD_ACTUAL24: $("#txtHardActual24").val() ? $("#txtHardActual24").val() : null,
            HARD_ACTUAL25: $("#txtHardActual25").val() ? $("#txtHardActual25").val() : null
        });
    var returnVal = true;
    var ajaxTime = new Date().getTime();
    var settings = {
        async:false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/IsMillSheetAlreadySaved",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: millsheet_
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            returnVal = response.isSuccess;
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnVal;
}
function IsFTPServerActive()
{
    var returnVal = {};
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/MakerMillSheet/IsFTPServerActive",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
           
            responseTime = new Date().getTime() - ajaxTime; 
            returnVal["IsSuccess"] = response.isSuccess;
            returnVal["IpAddress"] = response.message;
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_MAKERMILLSHEET_ENTRY_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
    return returnVal;
}
$('[id^="txtCCName"]').on('focusout', function () { // on CCName text change focus set on below text 

    var $currentCell = $(this).closest('td');
    var columnIndex = $currentCell.index();

    var $nextRow = $currentCell.closest('tr').next('tr');
    var $nextTextbox = $nextRow.children('td').eq(columnIndex).find('input[type="text"]');

    if ($nextTextbox.length > 0) {
        $nextTextbox.focus();
    }

});