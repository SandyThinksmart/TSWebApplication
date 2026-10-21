var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var typeText = "text";
var DISABLED = "disabled";
var makerMillsheetPDF = "";
var allocQty = 0;
$(document).ready(function () {
    makerMillsheetPDF = "";
    $("#btnHSKMillsheet").attr("disabled", "disabled");
    $("#btnMakerMillsheet").attr("disabled", "disabled");
    $("#lblWIP").css("display", "none")
    let ProcessPlanCD = getURLParameters(window.location.href).trim();
    if (parseInt(ProcessPlanCD) > 0) {
        GetProcessPlanDetails(ProcessPlanCD);
        GetFIFailureRateDetails(ProcessPlanCD);
        GetECDefectRateDetails(ProcessPlanCD);
        GetWIPDetails(ProcessPlanCD);
    }
});

$(document).bind("ajaxSend", function () {
    $("#LoadingImage").show();
}).bind("ajaxStop", function () {
    $("#LoadingImage").hide();
}).bind("ajaxError", function () {
    $("#LoadingImage").hide();
});

function getURLParameters(url) {

    let result = "";
    var searchIndex = url.indexOf("?");
    if (searchIndex == -1) return result;
    var sPageURL = url.substring(searchIndex + 1);
    var sParameterName = sPageURL.split('ProcessPlanCD=');
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


function GetProcessPlanDetails(processPlanCD) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        beforeSend: function () {
            $("#LoadingImage").show();
        },
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/getSalesProdStatusByProcessPlanCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ PP_PROCESS_PLAN_CD: processPlanCD, CULTURE_NAME: $("#hdfCultureName").val() })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult.getprocessdata != null) 
                {
                    bindProcessData(jsonResult.getprocessdata);
                }
            
            
            SetSalesProdStatusDetails(jsonResult);
            if (jsonResult.getdatatimewise != null) {
                bindTimeWise(jsonResult.getdatatimewise);
            }
            if (jsonResult.getdatabundlewise != null) {
                bindBundleWise(jsonResult);

            }

           
            if (jsonResult.getdatahardnessandpostht != null) {
                bindHardnessAndPostHT(jsonResult.getdatahardnessandpostht);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function bindTimeWise(jsonResult) {

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, object) {
            object.pP_PROCESS_NAME = object.procesS_NAME_EN;
        });
    } else {
        $.each(jsonResult, function (key, object) {
            object.pP_PROCESS_NAME = object.procesS_NAME_JP;
        });
    }
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/SalesProductionStatusSearch/getSalesProdStatusByTimewise",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)

    }
    $.ajax(settings1)
        .done(function (response1) {
            var jsonResult1 = response1.message;
            //var jsonResult1 = response1.message1;

            if (response1.isSuccess) {
                $("#divSalesProductionTimewiseGrid").html(jsonResult1);
            }
           

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function bindBundleWise(jsonResult) {
    //if ($("#hdfCultureName").val() == "en") {
    //    $.each(jsonResult, function (key, object) {
    //        object.pP_PROCESS_NAME = object.procesS_NAME_EN;
    //    });
    //} else {
    //    $.each(jsonResult, function (key, object) {
    //        object.pP_PROCESS_NAME = object.procesS_NAME_JP;
    //    });
    //}
    jsonResult.CULTURE_NAME = $("#hdfCultureName").val();
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/SalesProductionStatusSearch/getSalesProdStatusByBundlewise",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)

    }
    $.ajax(settings1)
        .done(function (response1) {
            var jsonResult1 = response1.message;
            //var jsonResult1 = response1.message1;

            if (response1.isSuccess) {
                $("#divSalesProductionBundlewiseGrid").html(jsonResult1);
            }


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindProcessData(jsonResult) {

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, object) {
            object.pP_PROCESS_NAME = object.procesS_NAME_EN;
        });
    } else {
        $.each(jsonResult, function (key, object) {
            object.pP_PROCESS_NAME = object.procesS_NAME_JP;
        });
    }
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/SalesProductionStatusSearch/getProcessData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)

    }
    $.ajax(settings1)
        .done(function (response1) {
            var jsonResult = response1.message;
            if (response1.isSuccess) {
                $("#divProcessGridMain").html(jsonResult);
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function bindHardnessAndPostHT(jsonResult) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/SalesProductionStatusSearch/getHardnessAndPostHTByProcessPlanCD",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonResult)
    }
    $.ajax(settings)
        .done(function (response1) {
            var jsonResult = response1.message;

            if (response1.isSuccess) {
                $("#divHardnessAndPostHTGridGridMain").html(response1.message);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function GetECDefectRateDetails(ProcessPlanCD) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/getECDefectRateByProcessPlanCD?processPlanCD=" + parseInt(ProcessPlanCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            debugger;
            var ECDefectRate = 0;
            var ecSuccessPer = 0;
            var NGQtySum = 0;
            var ECOkQtySum = 0;
            if (jsonResult != null) {
                ECOkQtySum = parseInt(jsonResult.eC_OK_QTY) || 0;
                NGQtySum = parseInt(jsonResult.eC_NG_QTY) || 0;
                // ECDefectRate = NGQtySum / (ECOkQtySum + NGQtySum) * 100;
                //$("#ECDefectRate").text(ECDefectRate.toFixed(2));
                //ecSuccessPer = (ECOkQtySum / allocQty) * 100;
                ecSuccessPer = (ECOkQtySum / (ECOkQtySum + NGQtySum)) * 100;
                $("#lblECSuccessPer").text(ecSuccessPer.toFixed(2) + '%');

            } else {
                $("#lblECSuccessPer").text("0.00%");
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function GetFIFailureRateDetails(ProcessPlanCD) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/getFIFailureRateByProcessPlanCD?processPlanCD=" + parseInt(ProcessPlanCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            var FI_Failure_Rate = 0;
            var successRate = 0;
            var FiQty_Sum = 0;
            var ActAllocQty = 0;
            if (jsonResult != null) {
                FiQty_Sum = parseInt(jsonResult.finisheD_QTYS) || 0 || null;

                ActAllocQty = parseInt(jsonResult.pP_ACT_ALLOC_QTY) || 0;
                allocQty = ActAllocQty;
                debugger;
                // FI_Failure_Rate = 100 -((FiQty_Sum * 100) / ActAllocQty);
                successRate = (FiQty_Sum / ActAllocQty) * 100;
                var a = successRate.toFixed(2) + "%";
                $("#lblFISuccessPer").text(successRate.toFixed(2) + "%");
            } else
            {
                $("#lblFISuccessPer").text("0.00%");
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function GetWIPDetails(ProcessPlanCD) {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        complete: function () {
            $("#LoadingImage").hide();
        },
        dataType: "json",
        url: APIURL + "/SalesProductionStatusSearch/getWIPByProcessPlanCD?processPlanCD=" + parseInt(ProcessPlanCD),
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (jsonResult.procesS_PLAN_CMPL_DATE == null) {
                var aa = $.trim($("#lbl_WIP_Search_001").text());
                $("#WIPTotal").text(aa);
            }
            else {
                var aa = $.trim($("#lbl_Completed_Search_002").text());
                $("#WIPTotal").text(aa);
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function SetSalesProdStatusDetails(jsonResult) {

    $("#hdfldProcessPlanCd").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#txtProdSpecNo").val(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").text(jsonResult.pP_CHUUBAN);
    $("#txtDeliveryDateT1").text($.format.date(new Date(jsonResult.pP_DELIVERY_DATE_STRING), "yyyy/MM/dd"));
    $("#txtSalesInchargeT1").text(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDate").text(jsonResult.pP_STD_DEL_PERIOD_STRING);

    $("#txtCustomerT1").text(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").text(jsonResult.pP_RECEIVER_NAME);
    $("#txtChamfer").text(jsonResult.pP_CHAMFER);
    if (jsonResult.pP_UNIT_WT_STRING != "") {
        $("#txtUnitWtT1").text(jsonResult.pP_UNIT_WT_STRING);
    }

    $("#txtColor").text(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").text(jsonResult.pP_PROD_NAME_INT);
    $("#txtMatlDimT1").text(jsonResult.pP_MATL_DIM_D);
    $("#txtChargeT1").text(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").text(jsonResult.pP_MAGARI_BEND_TOL);

    $("#txtProdNameCustomerT1").text(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdDimT1").text(jsonResult.pP_PROD_DIM_D);
    $("#txtProdDimOtherT1").text(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT1").text(jsonResult.pP_EXT_TEXTURE);

    $("#txtDimToleranceT1").text(jsonResult.pP_DIM_TOLERANCE);
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtDisplayT1").text(jsonResult.pP_DISPLAY);

    $("#txtStampT1").text(jsonResult.pP_STAMP);
    $("#txtLengthT1").text(jsonResult.pP_PROD_LENGTH_STRING);
    $("#txtLengthToleranceT1").text(jsonResult.pP_LEN_TOL);
    $("#txtPackingT1").text(jsonResult.pP_PACKING_DECODE);
    $("#txtNoOfUnitsT1").text(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtQuantityT1").text(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").text(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").text(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirection").text(jsonResult.pP_COIL_DIA_WINDING);

    $("#txtKakouiraiRemarksT1").text(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemark").text(jsonResult.pP_PROCESS_PLAN_REMARK);
    $("#txtHostRefNo").text(jsonResult.nyukA_CD);
   // makerMillsheetPDF = jsonResult.makeR_MILLSHEET_PDF_FILE_PATH;
    
}

function OnMessageListClick(editButton) {

    var ProcessPlanCD = $("#hdfldProcessPlanCd").val();
    // location.href = '/SalesProductionStatusSearch/MessageList?ProcessPlanCD=' + ProcessPlanCD;
    window.open("/SalesProductionStatusSearch/MessageList?ProcessPlanCD=" + ProcessPlanCD, "_blank", "toolbar=1, scrollbars=1, resizable=1, width=" + 1015 + ", height=" + 800);
}
$("#btnClose").click(function () {
    self.close();
});
$("#btnMakerMillsheet").click(function () {
    
    if (makerMillsheetPDF) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/SalesProductionStatusSearch/DowloadAndSavePDF?filePath=" + makerMillsheetPDF,
            method: "POST",
            contentType: "application/json; charset=utf-8",
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                   // window.open(response.message, "_blank");
                    window.open(response.message, "pdfWindow", "width=1900,height=1000,scrollbars=yes");
                }


            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_SalesProcessStatus_001").text());
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    } else
    {
        showMessageDialog("PDF File not exist for this process plan", "", WARNING);
    }
});