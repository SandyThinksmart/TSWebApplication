let currentDiv = 1;
const totalDiv = 3;
let processPlanCD = 0;
$(document).ready(function () {

    window.resize(255, 445);
    let combineCD = GetURLParameters(window.location.href).trim().toLowerCase();
    if (parseInt(combineCD) > 0)
    {
        GetUnitType();
        GetNextProcess();
        GetWIPTagCombineDetails(combineCD);
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
        var sParameterName = url.split('combineCD=');
        result = sParameterName[1];
    }
    return result;
}
$("#btnPrev").click(function () {
    var btnCloseText = $("#hdfButtonClose").val();
    var btnNextText = $("#hdfButtonNext").val();
    if ($("#btnPrev").val() == btnCloseText) {
        self.close();
    } else {
        
        if (currentDiv >= 1) {
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
    let combineCD = GetURLParameters(window.location.href).trim().toLowerCase();
    if ($("#btnNext").val() == btnDeleteText && parseInt(combineCD) > 0) {
        
       DeleteWIPCombine(combineCD);
        
    } else {
        if (currentDiv <= totalDiv) {
            // Hide current 
            $("#div" + currentDiv).hide();
            // Move to next 
            currentDiv++;
            // Show next
            $("#div" + currentDiv).show();
            $("#btnPrev").val(btnPrevText);
            if (currentDiv == 3) {
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
        url: "/WIPTagCombine/GetProductUnitKubun",
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
            var message = $.trim($("#lblErrorMessageE_WIP_Tag_Combine_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindUnitTypeDropdown(jsonResult) {
    $("#ddlUnitType1_div2").html("");//.append($('<option></option>'));
    $("#ddlUnitType2_div2").html("");//.append($('<option></option>'));
    $("#ddlUnitType3_div2").html("");//.append($('<option></option>'));
    $("#ddlUnitType4_div2").html("");//.append($('<option></option>'));
    $("#ddlUnitType1_div3").html("");//.append($('<option></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType1_div2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType2_div2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType3_div2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType4_div2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
        $("#ddlUnitType1_div3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

}
function GetNextProcess() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTagCombine/GetNextProcess",
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
            var message = $.trim($("#lblErrorMessageE_WIP_Tag_Combine_001").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
}
function BindNextProcessDropdown(jsonResult) {
    $("#ddlNextProcess").html("");//.append($('<option></option>'));
   
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_EN + '</option>'));
            
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '">' + jsonObj.kubuN_NAME_JP + '</option>'));
           
        });
    }

}
function DeleteWIPCombine(combineCD) {

    var ajaxTime = new Date().getDate();
    
    var jsonDataDelete = JSON.stringify({
        PROCESS_PLAN_CD: processPlanCD,
        COMBINE_SOURCES_TABA_NO_1: $("#txtThaba1_div2").val() ? $("#txtThaba1_div2").val() : null,
        COMBINE_SOURCES_TABA_NO_2: $("#txtThaba2_div2").val() ? $("#txtThaba2_div2").val() : null,
        COMBINE_SOURCES_TABA_NO_3: $("#txtThaba3_div2").val() ? $("#txtThaba3_div2").val() : null,
        COMBINE_SOURCES_TABA_NO_4: $("#txtThaba4_div2").val() ? $("#txtThaba4_div2").val() : null,
        TABA_COMBINE_CD: combineCD,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTagCombine/DeleteTagCombineData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataDelete
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            if (response.isSuccess) {
                $("#div3").hide();
                let combinedThaba = $("#txtThaba1_div2").val();
                if (parseInt($("#txtThaba2_div2").val()) > 0)
                {
                    combinedThaba = combinedThaba + "・" + $("#txtThaba2_div2").val();
                }
                if (parseInt($("#txtThaba3_div2").val()) > 0) {
                    combinedThaba = combinedThaba + "・" + $("#txtThaba3_div2").val();
                }
                if (parseInt($("#txtThaba4_div2").val()) > 0) {
                    combinedThaba = combinedThaba + "・" + $("#txtThaba4_div2").val();
                }
                $("#div4").show();
                $("#lblSuccessMsg").text($("#lblSuccessMsg").text().trim().replace("NNN", combinedThaba));
                $("#btnNext").hide();

                $("#btnPrev").hide();
                $("#btnCloseWindow").show();
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIP_Tag_Combine_001").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
        });
   

}

function GetWIPTagCombineDetails(combineCD) {

    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/WIPTagCombine/GetTagDetailsByCombineCD?combineCD=" + combineCD,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            var jsonResult = response.message;
            if (response.isSuccess) {
                processPlanCD = jsonResult.procesS_PLAN_CD;
                //==============div1======================
                $("#lblChuubanValue").text(jsonResult.pP_CHUUBAN_GYOUBAN);
                $("#lblCustomerValue").text(jsonResult.customeR_NAME);
                $("#lblProductNameValue").text(jsonResult.proD_NAME_CUST);
                $("#lblDimDValue").text(jsonResult.proD_DIM_D + " " + jsonResult.proD_SHAPE);
                if (jsonResult.chamfer != null) {
                    $("#lblChamferValue").text("(C-" + jsonResult.chamfer + ")");
                }

                $("#lblToleranceValue").text(jsonResult.diM_TOLERANCE);
                $("#lblLengthValue").text(jsonResult.proD_LENGTH);
                $("#lblChargeValue").text(jsonResult.charge);
                $("#lblOperatorValue").text(jsonResult.employeE_NAME);
                //=========================div2===============

                $("#txtThaba1_div2").val(jsonResult.combinE_SOURCES_TABA_NO_1);
                $("#txtNoOfUnit1_div2").val(jsonResult.combinE_SOURCES_NO_OF_UNITS_1);
                $("#ddlUnitType1_div2").val(jsonResult.combinE_SOURCES_UNIT_1);
                $("#txtQuantity1_div2").val(jsonResult.combinE_SOURCES_QTY_1);

                $("#txtThaba2_div2").val(jsonResult.combinE_SOURCES_TABA_NO_2);
                $("#txtNoOfUnit2_div2").val(jsonResult.combinE_SOURCES_NO_OF_UNITS_2);
                $("#ddlUnitType2_div2").val(jsonResult.combinE_SOURCES_UNIT_2);
                $("#txtQuantity2_div2").val(jsonResult.combinE_SOURCES_QTY_2);

                $("#txtThaba3_div2").val(jsonResult.combinE_SOURCES_TABA_NO_3);
                $("#txtNoOfUnit3_div2").val(jsonResult.combinE_SOURCES_NO_OF_UNITS_3);
                $("#ddlUnitType3_div2").val(jsonResult.combinE_SOURCES_UNIT_3);
                $("#txtQuantity3_div2").val(jsonResult.combinE_SOURCES_QTY_3);

                $("#txtThaba4_div2").val(jsonResult.combinE_SOURCES_TABA_NO_4);
                $("#txtNoOfUnit4_div2").val(jsonResult.combinE_SOURCES_NO_OF_UNITS_4);
                $("#ddlUnitType4_div2").val(jsonResult.combinE_SOURCES_UNIT_4);
                $("#txtQuantity4_div2").val(jsonResult.combinE_SOURCES_QTY_4);
                //=========================div3===============
                $("#txtThaba1_div3").val(jsonResult.combinE_TABA_NO);
                $("#txtNoOfUnit1_div3").val(jsonResult.combinE_NO_OF_UNITS);
                $("#ddlUnitType1_div3").val(jsonResult.combinE_UNITS);
                $("#txtQuantity1_div3").val(jsonResult.combinE_QTY);

                $("#ddlNextProcess").val(jsonResult.combinE_NEXT_PROCESS);

            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_WIP_Tag_Combine_001").text().trim();
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