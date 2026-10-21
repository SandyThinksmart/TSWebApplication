let currentDiv = 1;
const totalDiv = 3;
let originThaba = 0;
let processPlanCD = 0;
$(document).ready(function () {
   
    window.resize(255, 445);
   
    let divisionCD = GetURLParameters(window.location.href).trim().toLowerCase();
    if (parseInt(divisionCD) > 0) {
        GetUnitType();
        GetNextProcess();
        GetWIPTagSplitDetails(divisionCD);
    }
    $("#btnNext").focus();
   
});
function GetURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) {
        result = "";
    } else {
        var sPageURL = url.substring(searchIndex + 1);
        var sParameterName = url.split('divisionCD=');
        result = sParameterName[1];
    }
    return result;
}
$("#btnPrev").click(function () {
    var btnCloseText = $("#hdfButtonClose").val();
    var btnNextText = $("#hdfButtonNext").val();
    if ($("#btnPrev").val() == btnCloseText) {
        self.close();
    } else
    {
        
        if (currentDiv >= 1)
        {
            // Hide current 
            $("#div" + currentDiv).hide();
            // Move to prev
            currentDiv--;
            $("#btnNext").val(btnNextText);
            // Show prev
            $("#div" + currentDiv).show();
                if (currentDiv == 1) {
                    
                    $("#btnPrev").val(btnCloseText);
                }
            
        }
    }
});
$("#btnCloseWindow").click(function () {
    self.close();
});
$("#btnNext").click(function () {
    
    var btnPrevText = $("#hdfButtonPrev").val();
    var btnDeleteText = $("#hdfButtonDelete").val();
    let divisionCD = GetURLParameters(window.location.href).trim().toLowerCase();
    if ($("#btnNext").val() == btnDeleteText && parseInt(divisionCD) > 0)
    {
        DeleteWIPTagSplit(divisionCD);
    } else
    {
        if (currentDiv <= totalDiv) {
            // Hide current 
            $("#div" + currentDiv).hide();
            // Move to next 
            currentDiv++;
            // Show next
            $("#div" + currentDiv).show();
            $("#btnPrev").val(btnPrevText);
            if (currentDiv == 3)
            {
                $("#btnNext").val(btnDeleteText);
            }
        }
    }
});
function GetUnitType() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/WIPTagSplit/GetProductUnitKubun",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindUnitTypeDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_WIP_Tag_Split_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindUnitTypeDropdown(jsonResult) {
    $("#ddlUnitType1").html("");//.append($('<option></option>'));
    $("#ddlUnitType2").html("");//.append($('<option></option>'));
    $("#ddlUnitType3").html("");//.append($('<option></option>'));
    $("#ddlUnitType4").html("");//.append($('<option></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType4").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
    
}
function GetNextProcess() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL+ "/WIPTagSplit/GetNextProcess",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindNextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_WIP_Tag_Split_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindNextProcessDropdown(jsonResult) {
    $("#ddlNextProcess1").html("");//.append($('<option></option>'));
    $("#ddlNextProcess2").html("");//.append($('<option></option>'));
    $("#ddlNextProcess3").html("");//.append($('<option></option>'));
    $("#ddlNextProcess4").html("");//.append($('<option></option>'));
   
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess1").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
            $("#ddlNextProcess2").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
            $("#ddlNextProcess3").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
            $("#ddlNextProcess4").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess1").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
            $("#ddlNextProcess2").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
            $("#ddlNextProcess3").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
            $("#ddlNextProcess4").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
        });
    }

}
function GetWIPTagSplitDetails(divisionCD) {

    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTagSplit/GetTagDetailsByDivisionCD?divisionCD=" + divisionCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var jsonResult = response.message;
            if (response.isSuccess)
            {
                originThaba = jsonResult.origiN_TABA_NO;
                processPlanCD = jsonResult.procesS_PLAN_CD;
                //==============div1======================
                $("#lblChuubanValue").text(jsonResult.pP_CHUUBAN_GYOUBAN);
                $("#lblCustomerValue").text(jsonResult.customeR_NAME);
                $("#lblProductNameValue").text(jsonResult.proD_NAME_CUST);
                $("#lblDimDValue").text(jsonResult.proD_DIM_D + " " + jsonResult.proD_SHAPE);
                if (jsonResult.chamfer != null)
                {
                    $("#lblChamferValue").text("(C-" + jsonResult.chamfer + ")");
                }
                
                $("#lblToleranceValue").text(jsonResult.diM_TOLERANCE);
                $("#lblLengthValue").text(jsonResult.proD_LENGTH);
                $("#lblChargeValue").text(jsonResult.charge);
                $("#lblOperatorValue").text(jsonResult.employeE_NAME);
                //=========================div2===============

                $("#txtThaba1_div2").val(jsonResult.divisioN_TABA_NO_1);
                $("#txtNoOfUnit1").val(jsonResult.divisioN_NO_OF_UNITS_1);
                $("#ddlUnitType1").val(jsonResult.divisioN_UNIT_1);
                $("#txtQuantity1").val(jsonResult.divisioN_QTY_1);

                $("#txtThaba2_div2").val(jsonResult.divisioN_TABA_NO_2);
                $("#txtNoOfUnit2").val(jsonResult.divisioN_NO_OF_UNITS_2);
                $("#ddlUnitType2").val(jsonResult.divisioN_UNIT_2);
                $("#txtQuantity2").val(jsonResult.divisioN_QTY_2);

                $("#txtThaba3_div2").val(jsonResult.divisioN_TABA_NO_3);
                $("#txtNoOfUnit3").val(jsonResult.divisioN_NO_OF_UNITS_3);
                $("#ddlUnitType3").val(jsonResult.divisioN_UNIT_3);
                $("#txtQuantity3").val(jsonResult.divisioN_QTY_3);

                $("#txtThaba4_div2").val(jsonResult.divisioN_TABA_NO_4);
                $("#txtNoOfUnit4").val(jsonResult.divisioN_NO_OF_UNITS_4);
                $("#ddlUnitType4").val(jsonResult.divisioN_UNIT_4);
                $("#txtQuantity4").val(jsonResult.divisioN_QTY_4);
                //=========================div3===============
                $("#txtThaba1_div3").val(jsonResult.divisioN_TABA_NO_1);
                $("#ddlNextProcess1").val(jsonResult.divisioN_NEXT_PROCESS_1);

                $("#txtThaba2_div3").val(jsonResult.divisioN_TABA_NO_2);
                $("#ddlNextProcess2").val(jsonResult.divisioN_NEXT_PROCESS_2);

                $("#txtThaba3_div3").val(jsonResult.divisioN_TABA_NO_3);
                $("#ddlNextProcess3").val(jsonResult.divisioN_NEXT_PROCESS_3);

                $("#txtThaba4_div3").val(jsonResult.divisioN_TABA_NO_4);
                $("#ddlNextProcess4").val(jsonResult.divisioN_NEXT_PROCESS_4);
            }
          
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIP_Tag_Split_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function ShowMessageDialog(message, focusFieldID, messageType) {

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
function DeleteWIPTagSplit(divisionCD)
{
    var ajaxTime = new Date().getDate();
    var jsonDataDelete = JSON.stringify({
        PROCESS_PLAN_CD: processPlanCD,
        DIVISION_TABA_NO_1: $("#txtThaba1_div2").val() ? $("#txtThaba1_div2").val() : null,
        DIVISION_TABA_NO_2: $("#txtThaba2_div2").val() ? $("#txtThaba2_div2").val() : null,
        DIVISION_TABA_NO_3: $("#txtThaba3_div2").val() ? $("#txtThaba3_div2").val() : null,
        DIVISION_TABA_NO_4: $("#txtThaba4_div2").val() ? $("#txtThaba4_div2").val() : null,
        TABA_DIVISION_CD: divisionCD,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTagSplit/DeleteTagSplitData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            if (response.isSuccess) {
                $("#div3").hide();

                $("#div4").show();

                
                $("#lblSuccessMsg").text($("#lblSuccessMsg").text().trim().replace("NNN", originThaba));
                $("#btnNext").hide();

                $("#btnPrev").hide();
                $("#btnCloseWindow").show();
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIP_Tag_Split_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
