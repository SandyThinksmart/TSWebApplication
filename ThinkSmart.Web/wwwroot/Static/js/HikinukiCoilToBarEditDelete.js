var $btnLnkEdit;
var combinedDecisionOK = "";
var indexDDLDecisionNothing = 0;
var $hdfSYSDATE;
var dimTolMin;
var dimTolMax;
var dimStd;
var lenTolMin;
var lenTolMax;
var lenStd;
var txtShikii = "";
var txtInspectionCoilSize = "";
var txtInspectionCoilShape = "";
var txtIsOK = "";
var oldHikinukiRemarks;
var isHikinukiRemarksUpdate = 0;
var IsExists = false;
var IsStartOprExists = false;
var IsEndOprExists = false;
var registeredThabaNumbersAndDecision = {};
var maxRegisteredThabaNo = 0;
var IsNGReason1 = false;
var IsNGReason2 = false;
var IsNGReason3 = false;
var IsProcessDefect1Exists = false;
var IsProcessDefect2Exists = false;
var IsProcessDefect3Exists = false;
var startOperatorCD = "";
var endOperatorCD = "";

var TYPEDROPDOWN = "select-one";
var TYPETEXT = "text";
var DISABLED = "disabled";
var PRODUCTSHAPE_R = "R";
var PRODUCTSHAPE_H = "H";
var PRODUCTSHAPE_S = "S";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";

function Mask() {
    $("#txtStartDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm" });
    $("#txtEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD HH:mm" });

}
function DisableEditDeleteButtons() {
    $("#btnUpdate").attr("disabled", "disabled");
    $("#btnDelete").attr("disabled", "disabled");
}

function EnableEditDeleteButtons() {
    $("#btnUpdate").removeAttr("disabled");
    $("#btnDelete").removeAttr("disabled");
}

$(document).ready(function () {
   
    window.resizeTo(1080, 830);


    $.support.cors = true; // Special case for IE7. To get past weird errors.
    SetDecimalFormats();
    Mask();
    
    DisableStartDataPanel();
    DisableEndDataPanel();
    DisableEditDeleteButtons();
    $("#txtChuubanSearch").focus();

});
const fixedWidth1 = 1080;
const fixedHeight1 = 830;

const fixedWidth2 = 1080;
const fixedHeight2 = 935;

let resizeTimer;

window.addEventListener("resize", function () { // alert popup shown if screen resize
    clearTimeout(resizeTimer);
    debugger;
    resizeTimer = setTimeout(function () {
        if ((window.outerWidth !== fixedWidth1 || window.outerHeight !== fixedHeight1) && $("#TbPMSEditDelTab1-tab").hasClass("active")) {
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_032").text());
            ShowMessageDialog(message, "", WARNING);
        } else if ((window.outerWidth !== fixedWidth2 || window.outerHeight !== fixedHeight2) && $("#TbPMSEditDelTab2-tab").hasClass("active")) {
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_032").text());
            ShowMessageDialog(message, "", WARNING);
        }
    }, 300); // Fires after resizing stops
});
function SetDecimalFormats() {
    $("#txtAEdgeDistortion1, #txtAEdgeDistortion2, #txtAEdgeDistortion3, #txtBEdgeDistortion1, #txtBEdgeDistortion2, #txtBEdgeDistortion3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 1, 1);
    });

    $("#txtUpperLowerRollerAngle1, #txtUpperLowerRollerAngle2, #txtCompressionTonnage, #txtHexR").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 1);
    });

    $("#txtMn").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 2);
    });

    $("#txtC, #txtUpperRoleElevation, #txtInspectionCoilSize, #txtMagnetism").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 2, 2);
    });

    $("#txtDieDim, #txtDrawDim1, #txtDrawDim2, #txtActualDim1, #txtActualDim2, #txtActualDim3, #txtActualDim4, #txtActualDim5, #txtActualDim6").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    $("#txtActualLength1, #txtActualLength2, #txtActualLength3").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2);
    });

    $("#txtFinishedQuantity").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 7, 2);
    });
    $("#txtFinishedBarCount").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 5, 2);
    });
}
function btnSearch_Click() {

    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $("#txtDimDMin").val();
    var PROD_DIM_D_MAX = $("#txtDimDMax").val();
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val();
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val();
    var errorMessage = "";
    var checkFocusField = "";

    if ($("#txtDimDMin").val().trim() == "" && $("#txtDimDMax").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_019").text());
        checkFocusField = 'txtDimDMin'
    }

    if ($("#txtLengthSearchMin").val().trim() == "" && $("#txtLengthSearchMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthSearchMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_020").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_020").text());
        }
    }

    if (errorMessage != "") {
        ShowMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }
    if (CHUUBAN.length > 0 || CUSTOMER_NAME.length > 0 || PROD_NAME_CUST.length > 0 || INTERNAL_NAME_STRING.length > 0 || PROD_DIM_D_MIN.length > 0 || PROD_DIM_D_MAX.length > 0 || PROD_SHAPE.length > 0 || PROD_LENGTH_MIN.length > 0 || PROD_LENGTH_MAX.length > 0

    ) {
        HikinukiGridSearch(null);
    }
    else {
        var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_017").text());
        ShowMessageDialog(message, 'txtChuubanSearch', WARNING);

    }
}

function HikinukiGridSearch(sortfields) {
    var CHUUBAN = $("#txtChuubanSearch").val();
    var CUSTOMER_NAME = $("#txtCustomerName").val();
    var PROD_NAME_CUST = $("#txtProdNameCust").val();
    var INTERNAL_NAME_STRING = $("#txtProdNameInt").val();
    var PROD_DIM_D_MIN = $("#txtDimDMin").val() ? $("#txtDimDMin").val() : null;
    var PROD_DIM_D_MAX = $("#txtDimDMax").val() ? $("#txtDimDMax").val() : null;
    var PROD_SHAPE = $("#txtShap").val();
    var PROD_LENGTH_MIN = $("#txtLengthSearchMin").val() ? $("#txtLengthSearchMin").val() : null;
    var PROD_LENGTH_MAX = $("#txtLengthSearchMax").val() ? $("#txtLengthSearchMax").val() : null;
    var USER_ID = $("#hdfLoginId").val();

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: CHUUBAN, CUSTOMER_NAME: CUSTOMER_NAME, PROD_NAME_CUST: PROD_NAME_CUST, INTERNAL_NAME_STRING: INTERNAL_NAME_STRING, PROD_DIM_D_MIN: PROD_DIM_D_MIN, PROD_DIM_D_MAX: PROD_DIM_D_MAX, PROD_SHAPE: PROD_SHAPE, PROD_LENGTH_MIN: PROD_LENGTH_MIN, PROD_LENGTH_MAX: PROD_LENGTH_MAX,
            SORT_FIELDS: sortfields, USER_ID: USER_ID
        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            if (response.isSuccess) {

                var settings1 = {
                    async: false,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/HikinukiCoilToBar/GridSearch",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)

                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divHikinukiGridMain").html(response1.message);
                        }
                    })
                    .fail(function (xhr, textStatus, errorThrown) {

                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text()); 
                        ShowMessageDialog(message, focusFieldID, ERROR);
                        return false;
                    });


            }

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    Closepopup();
}
function btnSortReset_Click() {
    InitSortPopup();
}
function Closepopup() {

    $("#SortPopUpModal").modal('hide');
}

function InitSortPopup() {
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/HikinukiCoilToBar/InitializeSortPopup",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            $('#lstSortFields').find('option')
                .remove()
                .end();
            $('#lstSelectedSortFields').find('option')
                .remove()
                .end();
            for (var i = 0; i < response.message.length; i++) {
                if (response.message[i].isDefaultSort) {
                    $('#lstSelectedSortFields').append($("<option></option>")
                        .attr("value", response.message[i].fieldName)
                        .text(response.message[i].displayName + GetSortDirectionSuffix(response.message[i].sortOrder)));
                }
                else {
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", response.message[i].fieldName)
                        .text(response.message[i].displayName));
                }

            }
            EnableDisableSortButtons();
            $("#rdoSortAsc").prop("checked", true);
            $("#rdoSortDsc").prop("checked", false);
        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
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
function GetHikinukiDetailsByDrawingCD($callFrom) {

    var ajaxTime = new Date().getTime();
    
    var hikinukiCD = $.trim($callFrom.closest("tr").attr("data-lblDrawingCD"));
    
      var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetHikinukiDetailsByDrawingCD?DrawingCD=" + hikinukiCD + "&cultureName=" + $("#hdfCultureName").val(),
        method: "POST",
        contentType: "application/json; charset=utf-8",
        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
           
            EnableEditDeleteButtons();
            EnableStartDataPanel();
            EnableEndDataPanel();

            BindTPTestList();
            BindDieDiaShape();
            BindUnitType();
            BindDecision();
            BindNextProcess();
            BindBendStatus();
            BindExteriorCondition();
            SetProcessDefectList();

            SetHikinukiDetails(jsonResult);
            var checkFormat = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
            if (!checkFormat.test($("#txtLengthT1").val()) && $("#txtLengthT1").val() != "") {
                $("#chkActualLength").removeAttr("disabled", "disabled");

            } else {
                $("#chkActualLength").attr("disabled", "disabled");
                $("#chkActualLength").prop("checked", false);
            }
            DisableDefaultFields();
            $("#ddlModel").focus();
            
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function SetHikinukiDetails(jsonResult) {
    combinedDecisionOK = jsonResult.decisioN_OK;
    SwitchTab($("#TbPMSEditDelTab1-tab"), $("#TbPMSEditDelTab2-tab"));

    $("#txtYobinaStartOperatorSearch").val("");
    $("#txtYobinaEndOperatorSearch").val("");
    startOperatorCD = jsonResult.dW_START_OPERATOR;
    SetOperatorDropdown($("#ddlStartOperatorNameValue"), startOperatorCD);

    endOperatorCD = jsonResult.dW_END_OPERATOR;
    SetOperatorDropdown($("#ddlEndOperatorNameValue"), endOperatorCD);

    $("#hdfMachineCD").val(jsonResult.dW_MACHINE_CD);
    SetMachineList();
    
    $("#ddlModel").val(jsonResult.dW_MACHINE_CD);
    $("#txtCreatedDate").val($.format.date(new Date(jsonResult.createD_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtUpdatedDate").val($.format.date(new Date(jsonResult.stR_UPDATED_DATE_STRING), "yyyy/MM/dd HH:mm"));
    $("#txtProductSpecNoT1").text(jsonResult.pP_PROD_SPEC_NO);
    $("#txtChuubanT1").val(jsonResult.pP_CHUUBAN_GYOUBAN);
    $("#txt_HostName").val(jsonResult.producT_NYUKO_CD);

    $("#txtDeliveryDateT1").val($.format.date(new Date(jsonResult.pP_DELIVERY_DATE_STRING), "yyyy/MM/dd"));
    $("#txtCustomerInchargeT1").val(jsonResult.pP_EMPLOYEE_NAME);
    $("#txtStdDeliveryDateT1").val(jsonResult.pP_STD_DEL_PERIOD_STRING);
    $("#txtCustomerT1").val(jsonResult.pP_CUSTOMER_NAME);
    $("#txtReceiverT1").val(jsonResult.pP_RECEIVER_NAME);
    if (parseInt(jsonResult.pP_CHAMFER) > 0) {
        $("#txtChamferT1").val(jsonResult.pP_CHAMFER);
    }
    if (jsonResult.pP_UNIT_WT_STRING !="") {
        $("#txtUnitWtT1").val(jsonResult.pP_UNIT_WT_STRING);
    }
    $("#txtConditionT1").val(jsonResult.pP_PRODUCT_CONDITION);
    $("#txtColorT1").val(jsonResult.pP_COLOR);
    $("#txtMaterialNameMakerT1").val(jsonResult.pP_MATL_NAME_MAK);
    $("#txtMatlShapeT1").val(jsonResult.pP_MATL_SHAPE);
    $("#txtMatlDimT1").val(jsonResult.pP_MATL_DIM_D);  //PP_MATL_DIM_D_STRING
    $("#txtChargeT1").val(jsonResult.pP_CHARGE);
    $("#txtBendStandardT1").val(jsonResult.pP_MAGARI_BEND_TOL);
    $("#txtProdNameCustomerT1").val(jsonResult.pP_PROD_NAME_CUST);
    $("#txtProdShapeT1").val(jsonResult.pP_PROD_SHAPE);
    $("#txtProdDimT1").val(jsonResult.pP_PROD_DIM_D);    //PP_PROD_DIM_D_STRING
    $("#txtProdDimOtherT1").val(jsonResult.pP_PROD_DIM_OTHER);
    $("#txtExtTextureT1").val(jsonResult.pP_EXT_TEXTURE);
    $("#txtDimToleranceT1").val(jsonResult.pP_DIM_TOLERANCE);
    $("#txtDisplayT1").val(jsonResult.pP_DISPLAY);
    $("#txtStampT1").val(jsonResult.pP_STAMP);
    $("#txtLengthT1").val(jsonResult.pP_PROD_LENGTH_STRING);
    if (jsonResult.pP_LEN_TOL != "0,0") {
        $("#txtLengthToleranceT1").val(jsonResult.pP_LEN_TOL);
    }
    $("#txtPackingT1").val(jsonResult.pP_PACKING);
    $("#txtNoOfUnitsT1").val(jsonResult.pP_NO_OF_UNITS_DECODE);            //text(jsonResult.PP_NO_OF_UNITS + '' + jsonResult.PP_UNIT);
    $("#txtQuantityT1").val(jsonResult.pP_ACT_ALLOC_QTY);
    $("#txtIntDiaMinT1").val(jsonResult.pP_COIL_DIA_INTERNAL);
    $("#txtExtDiaMaxT1").val(jsonResult.pP_COIL_DIA_EXTERNAL);
    $("#txtWindingDirectionT1").val(jsonResult.pP_COIL_DIA_WINDING_STRING);
    $("#txtKakouiraiRemarksT1").val(jsonResult.pP_KAKOUIRAI_REMARKS);
    $("#txtProdOrderRemarkT1").val(jsonResult.pP_PROCESS_PLAN_REMARK);
    
        $("#txtProductionProcessT1").val(jsonResult.pP_PRODUCTION_PROCESS);
    
    //-----------------------------------------------start data---------------------------------------------------------------
   
    $("#ddlTPTest").val(jsonResult.dW_TP_TEST); 
    $("#txtFurnaceNum").val(jsonResult.dW_FURNACE_NO);
    $("#txtNoOfCoils").val(jsonResult.pP_NO_OF_UNITS_DECODE);
    $("#txtCoilQuantity").val(jsonResult.dW_QTY);
    $("#txtMetallicLabelNo").val(jsonResult.dW_METALLIC_PLATE_CODE);
    $("#txtC").val(jsonResult.dW_CARBON_PERCENTAGE);
    $("#txtMn").val(jsonResult.dW_MAGNIZ_PERCENTAGE);

    $("#txtDieDim").val(jsonResult.dW_DIE_DIM);

    $("#ddlDieShape").val(jsonResult.dW_DIE_SHAPE);
    $("#txtDieNo").val(jsonResult.dW_DIE_NO);


    $("#txtDrawDim1").val(jsonResult.dW_DIM1);
    $("#txtDrawDim2").val(jsonResult.dW_DIM2);

    $("#txtUpperLowerRollerAngle1").val(jsonResult.dW_UPPER_LOWER_ROLLER_ANGLE1);
    $("#txtUpperLowerRollerAngle2").val(jsonResult.dW_UPPER_LOWER_ROLLER_ANGLE2);


    $("#txtUpperRoleElevation").val(jsonResult.dW_UPPER_ROLLER_ELEVATION);

    $("#txtCompressionTonnage").val(jsonResult.dW_TONNAGE);

    $("#txtHexR").val(jsonResult.dW_HEX_R);

    if (parseInt(jsonResult.dW_EC_DONE_CHK) == 1) {
        $("#chkECOnline").prop('checked', true);
    }
    else {
        $("#chkECOnline").prop('checked', false);
    }
    ToggleClassForMagnetism();

    if (parseInt(jsonResult.dW_REDRAWING) == 1) {
        $("#chkReHikinuki").prop('checked', true);
    }
    else {
        $("#chkReHikinuki").prop('checked', false);
    }


    $("#txtShikii").val(jsonResult.dW_SHIKII_VALUE);
    $("#txtInspectionCoilSize").val(jsonResult.dW_EC_COIL_SIZE);
    $("#txtInspectionCoilShape").val(jsonResult.dW_EC_COIL_SHAPE);
    $("#txtIsOK").val(jsonResult.dW_EC_IS_OK);

    $("#txtStartDate").val($.format.date(new Date(jsonResult.dW_START_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    //--------------------------End Data-----------------------------------------------------

    $("#txtEndDate").val($.format.date(new Date(jsonResult.dW_END_DATE_STRING), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#ddlDecision").val(jsonResult.dW_DECISION); 
    $("#ddlNextProcess").val(jsonResult.dW_NEXT_PROCESS);

    $("#txtActualDim1").val(jsonResult.dW_ACT_DIM1);
    $("#txtActualDim2").val(jsonResult.dW_ACT_DIM2);
    $("#txtActualDim3").val(jsonResult.dW_ACT_DIM3);
    $("#txtActualDim4").val(jsonResult.dW_ACT_DIM4);
    $("#txtActualDim5").val(jsonResult.dW_ACT_DIM5);
    $("#txtActualDim6").val(jsonResult.dW_ACT_DIM6);

    $("#txtActualLength1").val(jsonResult.dW_ACT_LENGTH1);
    $("#txtActualLength2").val(jsonResult.dW_ACT_LENGTH2);
    $("#txtActualLength3").val(jsonResult.dW_ACT_LENGTH3);
    $("#txtFinishedBarCount").val(jsonResult.dW_FINISHED_BARS);
   
    $("#ddlUnitType").val(jsonResult.dW_UNIT_TYPE); 

    $("#txtFinishedQuantity").val(jsonResult.dW_FINISHED_QTY);
    $("#txtCorrectedBars").val(jsonResult.dW_CORRECTED_BARS);
    $("#txtAEdgeDistortion1").val(jsonResult.dW_A_SIDE_FURE1);
    $("#txtAEdgeDistortion2").val(jsonResult.dW_A_SIDE_FURE2);
    $("#txtAEdgeDistortion3").val(jsonResult.dW_A_SIDE_FURE3);
    $("#txtBEdgeDistortion1").val(jsonResult.dW_B_SIDE_FURE1);
    $("#txtBEdgeDistortion2").val(jsonResult.dW_B_SIDE_FURE2);
    $("#txtBEdgeDistortion3").val(jsonResult.dW_B_SIDE_FURE3);
    $("#txtThaba1").val(jsonResult.dW_TABA_NO_1);
    $("#txtThaba2").val(jsonResult.dW_TABA_NO_2);
    $("#txtThaba3").val(jsonResult.dW_TABA_NO_3);
    $("#txtThaba4").val(jsonResult.dW_TABA_NO_4);
    $("#ddlBendStatus").val(jsonResult.dW_BEND_STATUS);
    $("#ddlExternalSurfaceCondition").val(jsonResult.dW_EXT_TEXTURE_STATUS);
    $("#txtDisposalBars").val(jsonResult.dW_DISPOSAL_NO);
    $("#txtMagnetism").val(jsonResult.dW_MAGNETISM);
    $("#txtNGQuantity1").val(jsonResult.dW_NG_BARS1);
    $("#txtNGQuantity2").val(jsonResult.dW_NG_BARS2);
    $("#txtNGQuantity3").val(jsonResult.dW_NG_BARS3);
    
    $("#ddlNGReason1").val(jsonResult.dW_NG_DETAILS1);
    $("#ddlNGReason2").val(jsonResult.dW_NG_DETAILS2);
    $("#ddlNGReason3").val(jsonResult.dW_NG_DETAILS3);
    
        $("#txtECNGBars").val(jsonResult.dW_EC_NG_BAR_TOTAL);
   
    $("#txtRemarks").val(jsonResult.dW_REMARKS);
    $("#hfld_DrawingCd").val(jsonResult.dW_DRAWING_CD);
    $("#hfID_ProcessPlanCD").val(jsonResult.pP_PROCESS_PLAN_CD);
    $("#hfld_MessageCD").val(jsonResult.dW_MSG_CD);

    if (parseInt(jsonResult.dW_ACT_DIM_CHK) == 1) {
        $("#chkActualDim").prop('checked', true);
    }
    else {
        $("#chkActualDim").prop('checked', false);
    }
    IsActualDimMandatory();

    if (parseInt(jsonResult.dW_ACT_LENGTH_CHK) == 1) {
        $("#chkActualLength").prop('checked', true);
    }
    else {
        $("#chkActualLength").prop('checked', false);
    }
    IsActualLengthMandatory();

    if (parseInt(jsonResult.dW_FINISHED_QTY_CHK) == 1) {
        $("#chkFinishedQuantity").prop('checked', true);
    }
    else {
        $("#chkFinishedQuantity").prop('checked', false);
    }    

    $("#ddlNGReason1").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#ddlNGReason2").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#ddlNGReason3").attr(DISABLED, DISABLED).addClass(DISABLED);

    if (parseInt(jsonResult.dW_NG_BARS1) > 0) {
        DdlNGReasonMandatory($("#txtNGQuantity1"), $("#ddlNGReason1"));
    }
    if (parseInt(jsonResult.dW_NG_BARS2) > 0) {
        DdlNGReasonMandatory($("#txtNGQuantity2"), $("#ddlNGReason2"));
    }
    if (parseInt(jsonResult.dW_NG_BARS3) > 0) {
        DdlNGReasonMandatory($("#txtNGQuantity3"), $("#ddlNGReason3"));
    }
    //if (parseInt(jsonResult.dW_NG_BARS1) > 0 || parseInt(jsonResult.dW_NG_BARS2) > 0 || parseInt(jsonResult.dW_NG_BARS3) > 0) {

    //    CalculateBarCount();
    //}
}

$("#txtNGQuantity1").keyup(function () {
    DdlNGReasonMandatory($("#txtNGQuantity1"), $("#ddlNGReason1"));
    if ($("#chkECOnline").prop("checked") == true) {
        CalculateBarCount();
    }
});
$("#txtNGQuantity2").keyup(function () {
    DdlNGReasonMandatory($("#txtNGQuantity2"), $("#ddlNGReason2"));
    if ($("#chkECOnline").prop("checked") == true) {
        CalculateBarCount();
    }
});
$("#txtNGQuantity3").keyup(function () {
    DdlNGReasonMandatory($("#txtNGQuantity3"), $("#ddlNGReason3"));
    if ($("#chkECOnline").prop("checked") == true) {
        CalculateBarCount();
    }

});
function CalculateBarCount() {
    var bar1 = 0;
    var bar2 = 0;
    var bar3 = 0;
    if ($("#txtNGQuantity1").val() != "") {
        bar1 = $("#txtNGQuantity1").val();
    }
    if ($("#txtNGQuantity2").val() != "") {
        bar2 = $("#txtNGQuantity2").val();
    }
    if ($("#txtNGQuantity3").val() != "") {
        bar3 = $("#txtNGQuantity3").val();
    }
    var totalValue = parseInt(bar1) + parseInt(bar2) + parseInt(bar3);
    if (totalValue > 0) {
        $("#txtECNGBars").val(totalValue);
        //  calculateDefectRate();

    } else {
        $("#txtECNGBars").val("");
        //  $("#txtDefectRate").val("");
    }
}
function DdlNGReasonMandatory($txtTarget, $ddlTarget) {

    if ($txtTarget.val() > 0) {
        $ddlTarget.removeAttr(DISABLED, DISABLED).removeClass(DISABLED);
        $ddlTarget.addClass("Mandatory");

    } else {
        $ddlTarget.attr(DISABLED, DISABLED).addClass(DISABLED);
        $ddlTarget.removeClass("Mandatory");
        $ddlTarget.val(0);
    }
}

function SwitchTab($tabFrom, $tabTo) {

    $tabTo.click();

}


$("#chkActualDim").click(function () {
    IsActualDimMandatory();
});
function IsActualDimMandatory() {
    if ($("#chkActualDim").is(':checked')) {
        $("#txtActualDim1, #txtActualDim2, #txtActualDim3").addClass("Mandatory");
    } else {
        $("#txtActualDim1, #txtActualDim2, #txtActualDim3").removeClass("Mandatory");
    }
}
$("#chkActualLength").click(function () {
    IsActualLengthMandatory();
});
function IsActualLengthMandatory() {
    if ($("#chkActualLength").is(':checked')) {
        $("#txtActualLength1, #txtActualLength2, #txtActualLength3").addClass("Mandatory");
    } else {
        $("#txtActualLength1, #txtActualLength2, #txtActualLength3").removeClass("Mandatory");
    }
}

function SetProcessDefectList() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetProcessDefect",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindProcessDefectDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindProcessDefectDropdown(jsonResult) {
    
    $("#ddlNGReason1").html("").append($('<option></option>'));
    $("#ddlNGReason2").html("").append($('<option></option>'));
    $("#ddlNGReason3").html("").append($('<option></option>'));
   
    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
            $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
            $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_EN + '</option>'));
        });
    } else {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNGReason1").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlNGReason2").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
            $("#ddlNGReason3").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.defecT_NAME_JP + '</option>'));
        });
    }
}

function BindExteriorCondition() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetExteriorCondition",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindExteriorList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindExteriorList(jsonResult) {
    $("#ddlExternalSurfaceCondition").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlExternalSurfaceCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function BindBendStatus() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetBendStatus",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindBendStatusList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindBendStatusList(jsonResult) {
    $("#ddlBendStatus").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlBendStatus").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindNextProcess() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindNextProcessList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}


function BindNextProcessList(jsonResult) {
    
    $("#ddlNextProcess").html("");//.append($('<option></option>'));

    if ($("#hdfCultureName").val() == "en") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));

        });
    }
    if ($("#hdfCultureName").val() == "ja-JP") {
        $.each(jsonResult, function (key, jsonObj) {
            $("#ddlNextProcess").append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));

        });
    }

}
function BindDecision() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetDecision",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDecisionList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindDecisionList(jsonResult) {
    $("#ddlDecision").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecision").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindUnitType() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetUnitType",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindUnitList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindUnitList(jsonResult) {
    $("#ddlUnitType").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlUnitType").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function SetMachineList() {
    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetMachineList?machineType=70",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindMachineDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindMachineDropdown(jsonResult) {
    var getResult = jsonResult;
    jsonResult = jsonResult.filter(row2 => row2.iN_ACTIVE_FLAG == 0)
    var activeMachineList = getResult.filter(row3 => row3.cd == $("#hdfMachineCD").val());

    if (activeMachineList.length > 0) {
        if (activeMachineList[0].iN_ACTIVE_FLAG == 1) {
            jsonResult.push({ cd: activeMachineList[0].cd, name: activeMachineList[0].name, yobina: activeMachineList[0].yobina, iN_ACTIVE_FLAG: activeMachineList[0].iN_ACTIVE_FLAG });

        }
    }


    $("#ddlModel").html("").append($('<option value="0"></option>'));

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlModel").append($('<option value="' + jsonObj.cd + '" flag="' + jsonObj.iN_ACTIVE_FLAG + '" >' + jsonObj.name + '</option>'));

    });
}

function EnableStartDataPanel() {

    var $startDataPanel = $("#pnlProcessStartData");

    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($startDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($startDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });
    $("#ddlModel").removeClass(DISABLED).removeAttr(DISABLED);
    $("#chkReHikinuki").prop("checked", false);
}

function EnableEndDataPanel() {

    var $endDataPanel = $("#pnlProcessEndData");

    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        if (!($(textBox).hasClass("AlwaysDisabled"))) {
            $(textBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($endDataPanel.find("select"), function (index, dropdown) {
        if (!($(dropdown).hasClass("AlwaysDisabled"))) {
            $(dropdown).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        if (!($(checkBox).hasClass("AlwaysDisabled"))) {
            $(checkBox).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

    $.each($endDataPanel.find("input:button"), function (index, button) {
        if (!($(button).hasClass("AlwaysDisabled"))) {
            $(button).removeClass(DISABLED).removeAttr(DISABLED);
        }
    });

}
function DisableStartDataPanel() {
    // Set StartData Panel Elements to Disabled
    var $startDataPanel = $("#pnlProcessStartData");
    $.each($startDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("select"), function (index, dropdown) {
        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($startDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });

    $("#ddlModel").attr(DISABLED, DISABLED).addClass(DISABLED);
}

function DisableEndDataPanel() {
    // Set EndData Panel Elements to Disabled
    var $endDataPanel = $("#pnlProcessEndData");
    $.each($endDataPanel.find("input:text"), function (index, textBox) {
        $(textBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("select"), function (index, dropdown) {

        $(dropdown).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
    $.each($endDataPanel.find("input:button"), function (index, button) {
        $(button).attr(DISABLED, DISABLED).addClass(DISABLED);
    });
}
function DisableDefaultFields() {
    $("#txtNoOfCoils").attr(DISABLED, DISABLED).addClass(DISABLED);
    $("#txtCoilQuantity").attr(DISABLED, DISABLED).addClass(DISABLED);
   // $("#txtECNGBars").attr(DISABLED, DISABLED).addClass(DISABLED); 
}
$("#btnStartDateTime").click(function () {
    SetSysDateTime($("#txtStartDate"))
});
$("#btnEndDateTime").click(function () {
    SetSysDateTime($("#txtEndDate"))
});
function SetSysDateTime($target) {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetSysDateTime",
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            var sysDateTime = response.message;

            $target.val($.format.date(new Date(sysDateTime), "yyyy/MM/dd HH:mm")).mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });

}
function BindDieDiaShape() {

    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetDieDiaShape",
        method: "GET",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            BindDieDiaList(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function BindDieDiaList(jsonResult) {
    $("#ddlDieShape").html("");
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDieShape").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function BindTPTestList() {

    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GetTPTestList",
        method: "GET",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            BindTPTestDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            var message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function BindTPTestDropdown(jsonResult) {
    $("#ddlTPTest").html("");//.append($('<option></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlTPTest").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}
function IsMandatoryFieldsFilled() {
    var returnVal = true;  // default true is required for screens with no mandatory fields

    var message = ""; // This message will be displayed on dialog box
    //    var isRequired = $("#lblErrorMessageE_HIKI_ENTRY_001").text().trim();
    var focusFieldID = "";

    $.each($.find(".Mandatory"), function (index, mandatoryField) {
        if (mandatoryField.type === TYPEDROPDOWN && !($(mandatoryField).hasClass(DISABLED))) {
            if (($("#" + mandatoryField.id + " :selected").text().trim()) == "") {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
        else if (mandatoryField.type === TYPETEXT && !($(mandatoryField).hasClass(DISABLED))) {
            if (!mandatoryField.value) {
                returnVal = false;
                message = message + "<br>" + $(mandatoryField).attr("ErrorLabel");
                if (focusFieldID === "") {
                    focusFieldID = $(mandatoryField).attr("id");
                }
            }
        }
    });

    if (message !== "") {
        ShowMessageDialog(message, focusFieldID, WARNING);
    }

    return returnVal;

}

$("#btnUpdate").click(function () {
    if (!IsMandatoryFieldsFilled()) {
        return false;
    }
    if (!CheckDate($("#txtStartDate"))) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_021").text().trim(); // This message will be displayed on dialog box
        ShowMessageDialog(message, "txtStartDate", WARNING);
        return false;
    }
    if (!CheckDate($("#txtEndDate"))) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_022").text().trim(); // This message will be displayed on dialog box
        ShowMessageDialog(message, "txtEndDate", WARNING);
        return false;
    }

    if (!IsEndDataValid()) {
        return false;
    }
    if ($("#ddlModel option:selected").attr("flag") == 1) {

        DisplayInactivePopup("ddlModel");
        return false;
    }

    if ($("#ddlStartOperatorNameValue option:selected").attr("flag") == 1) {

        DisplayInactivePopup("ddlStartOperatorNameValue");
        return false;
    }

    if ($("#ddlEndOperatorNameValue option:selected").attr("flag") == 1) {

        DisplayInactivePopup("ddlEndOperatorNameValue");
        return false;
    }
    if (!IsDuplicateThabaNo($("#hfID_ProcessPlanCD").val())) {
        return false;
    }
    
    if ($("#chkActualDim").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK) {
        var dimValues = {};
        dimValues["PRODUCT_DIM"] = $("#txtProdDimT1").val();
        dimValues["DIM_TOLERANCE"] = $("#txtDimToleranceT1").val();
        dimValues["DIM1"] = $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null;
        dimValues["DIM2"] = $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null;
        dimValues["DIM3"] = $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null;
        dimValues["DIM4"] = $("#txtActualDim4").val() ? $("#txtActualDim4").val() : null;
        dimValues["DIM5"] = $("#txtActualDim5").val() ? $("#txtActualDim5").val() : null;
        dimValues["DIM6"] = $("#txtActualDim6").val() ? $("#txtActualDim6").val() : null;

        var result = IsActualDimentionValid(dimValues);
        if (result.IsSuccess == false) {
            var focusText = "";
            if (result.InvalidDim == $("#txtActualDim1").val()) {
                focusText = "txtActualDim1";
            }
            if (result.InvalidDim == $("#txtActualDim2").val() && focusText == "") {
                focusText = "txtActualDim2";
            }
            if (result.InvalidDim == $("#txtActualDim3").val() && focusText == "") {
                focusText = "txtActualDim3";
            }
            if (result.InvalidDim == $("#txtActualDim4").val() && focusText == "") {
                focusText = "txtActualDim4";
            }
            if (result.InvalidDim == $("#txtActualDim5").val() && focusText == "") {
                focusText = "txtActualDim5";
            }
            if (result.InvalidDim == $("#txtActualDim6").val() && focusText == "") {
                focusText = "txtActualDim6";
            }
            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_027").text().trim(); //ActDimOutOfTolerance
            ShowMessageDialog(message, focusText, WARNING);
            return false;
        }
    }

    if ($("#chkActualLength").prop("checked") == true && $("#ddlDecision").val() == combinedDecisionOK && $("#txtLengthToleranceT1").val() != "") {
        var lengthValues = {};
        lengthValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;
        lengthValues["LENGTH_TOLERANCE"] = $("#txtLengthToleranceT1").val() ? $("#txtLengthToleranceT1").val() : null;
        lengthValues["LENGTH1"] = $("#txtActualLength1").val() ? $("#txtActualLength1").val() : null;
        lengthValues["LENGTH2"] = $("#txtActualLength2").val() ? $("#txtActualLength2").val() : null;
        lengthValues["LENGTH3"] = $("#txtActualLength3").val() ? $("#txtActualLength3").val() : null;


        var result = IsActualLengthValid(lengthValues);

        if (result.IsSuccess == false) {
            var focusText = "";
            if (result.InvalidLength == $("#txtActualLength1").val()) {
                focusText = "txtActualLength1";
            }
            if (result.InvalidLength == $("#txtActualLength2").val() && focusText == "") {
                focusText = "txtActualLength2";
            }
            if (result.InvalidLength == $("#txtActualLength3").val() && focusText == "") {
                focusText = "txtActualLength3";
            }
            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_028").text().trim(); // LengthOutOfTolerance
            ShowMessageDialog(message, focusText, WARNING);
            return false;
        }


    }
    var weightValues = {};
    weightValues["PRODUCT_WEIGHT"] = $("#txtQuantityT1").val() ? $("#txtQuantityT1").val() : null;
    weightValues["INPUT_WEIGHT"] = $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null;
    weightValues["PRODUCT_SHAPE"] = $("#txtProdShapeT1").val() ? $("#txtProdShapeT1").val() : null;
    weightValues["PRODUCT_DIM"] = $("#txtProdDimT1").val() ? $("#txtProdDimT1").val() : null;
    weightValues["NO_OF_UNITS"] = $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null;
    weightValues["PRODUCT_LENGTH"] = $("#txtLengthT1").val() ? $("#txtLengthT1").val() : null;

    if ($("#chkFinishedQuantity").prop("checked") == true) {

        if (["PRODUCT_SHAPE", "PRODUCT_DIM", "PRODUCT_LENGTH"].every(key => weightValues[key] != null && weightValues[key].toString().trim() !== "")) {

            var value = $("#txtLengthT1").val();
            // Check - Length only allow numeric &  Unit Type - Not allow BN 

            if (!IsNotNumeric(value) || value.trim() == "" || $("#ddlUnitType").val() == 2) {

                var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_031").text().trim();
                ShowMessageDialog(message, "chkFinishedQuantity", WARNING);
                return false;
            }

        } else {
            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_031").text().trim();
            ShowMessageDialog(message, "chkFinishedQuantity", WARNING);
            return false;
        }
    }

    var format = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;
    var getResponse = {};
    if ($("#ddlUnitType").val() == 2 && $("#chkFinishedQuantity").prop("checked") == false) {
        getResponse["IsSuccess"] = true;
    } else if (!format.test($("#txtLengthT1").val())) {
        getResponse = IsFinishedQuantityValid(weightValues);
    } else {
        getResponse["IsSuccess"] = true;
    }

    if (getResponse.IsSuccess == false && $("#chkFinishedQuantity").prop("checked") == true) {
        var message = "";

        //message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_030").text());  //WeightIsInvalid

        //bootbox.confirm({
        //    title: "<div style='color:white'></div>",
        //    message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
        //    buttons: {
        //        confirm: {
        //            label: 'Yes',
        //            className: 'btn-success'
        //        },
        //        cancel: {
        //            label: 'No',
        //            className: 'btn-danger'
        //        }
        //    },
        //    callback: function (result) {

        //        if (result) {

        //            var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_029").text().trim(); // FinishedQtyNotIn2PercentRange
        //            showMessageDialog(message, "txtFinishedQuantity", WARNING);
        //        }



        //    },
        //    centerVertical: true
        //});

                    message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_029").text().trim(); // FinishedQtyNotIn2PercentRange
                    ShowMessageDialog(message, "txtFinishedQuantity", WARNING);
        
    }else if (($("#chkFinishedQuantity").prop("checked") == false) && getResponse.IsSuccess == false) {
        var message = "";
       
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_030").text());  //WeightIsInvalid
        
        bootbox.confirm({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
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
                    
                    UpdateHikinukiDetails();
                    btnSearch_Click();
                }
                else {
                    $("#txtFinishedQuantity").focus();

                }


            },
            centerVertical: true
        });

    } else {
        
        UpdateHikinukiDetails();
        btnSearch_Click();
    }
});
function IsNotNumeric(value) { // ONLY NUMERIC NUMBER EXIST OR NOT CHECKED 
    return !isNaN(value) && !isNaN(parseFloat(value)) && typeof value !== 'boolean';
}
function CheckDate(field) {

    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
            console.log(regs);
            var totdalDays = new Date(regs[1], regs[2], 0).getDate();
            if (parseInt(regs[3]) > totdalDays) {

                return false;
            } else if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {

                return false;
            }
            else if (regs[4] > 23 || regs[5] > 59) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
function IsEndDataValid() {
    retval = true; // assume end data is valid

    // 1. If Start Date is greater than End Date, show error
    var startDateTime = $("#txtStartDate").val();
    var endDateTime = $("#txtEndDate").val();

    if (new Date(startDateTime) > new Date(endDateTime)) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_023").text().trim();
        var focusFieldID = "txtEndTime";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    // 2. If End Date and Start Date difference is less than 1 minute, show error    
    var diff = Math.abs(new Date(startDateTime) - new Date(endDateTime));
    var diffMinutes = Math.floor((diff / 1000) / 60);

    if (parseInt(diffMinutes) < 1) {
        var message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_024").text().trim();
        var focusFieldID = "txtEndTime";
        ShowMessageDialog(message, focusFieldID, WARNING);
        return false;
    }

    return true;
}

function DisplayInactivePopup(fieldName) {

    var tableName = "";
    var fieldID = fieldName + " option:selected";
    var nameValue = $('#' + fieldID).text();
    if (fieldName == "ddlModel") {
        tableName = "machine";
    } else {
        tableName = "employee";
    }
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/DisplayInactivePopup?nameValue=" + nameValue + "&tableName=" + tableName,
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            ShowMessageDialog(response.message, fieldName, WARNING);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}
function IsDuplicateThabaNo(processPlanCD) {
    var thabaList = [];
    var Thaba1 = $("#txtThaba1").val();
    var Thaba2 = $("#txtThaba2").val();
    var Thaba3 = $("#txtThaba3").val();
    var Thaba4 = $("#txtThaba4").val();
    var t1 = "";
    var t2 = "";
    var t3 = "";
    var t4 = "";
    var Thaba_No_String = "";
    var drawingCD = $("#hfld_DrawingCd").val();
    var returnval = true;
    var thabamessage = "";
    

    var chk;
    thabaList.push(Thaba1);
    if (Thaba2 != "") {
        thabaList.push(Thaba2);
    }
    if (Thaba3 != "") {
        thabaList.push(Thaba3);
    }
    if (Thaba4 != "") {
        thabaList.push(Thaba4);
    }

    chk = new Set();
    var duplicateThaba = thabaList.filter(n => chk.size === chk.add(n).size);


    if (duplicateThaba.length > 0) {
        if (duplicateThaba.length > 1) {
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_025").text().trim().replace("NNNN", duplicateThaba[0] + ", " + duplicateThaba[1]);;
        } else {
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_025").text().trim().replace("NNNN", duplicateThaba[0]);;
        }

        ShowMessageDialog(message, "txtThaba1", WARNING);
        returnval = false;
        return false;
    }

    if (Thaba1 != null && Thaba1 != 0) {
        Thaba_No_String = Thaba1.toString();
    }
    if (Thaba2 != null && Thaba2 != 0) {
        Thaba_No_String = Thaba_No_String + "','" + Thaba2;
    }
    if (Thaba3 != null && Thaba3 != 0) {
        Thaba_No_String = Thaba_No_String + "','" + Thaba3;
    }
    if (Thaba4 != null && Thaba4 != 0) {
        Thaba_No_String = Thaba_No_String + "','" + Thaba4;
    }


    var ajaxTime = new Date().getDate();
    var jsonData = JSON.stringify({
        PROCESS_PLAN_CD: processPlanCD,
        TABA_NO_STRING: Thaba_No_String,
        DRAWING_CD: drawingCD,
        ISCHECKED: $('#chkReHikinuki').is(':checked') ? 1 : 0,
        TABA_NO_1: $("#txtThaba1").val(),
        TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
    });
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetThabaDetailsByThabaNumber",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            var sucess = response.isSuccess
            if (sucess == false) {
                firstFieldID = "txtThaba1";
                message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_026").text().trim().replace("NNN", jsonResult);
                ShowMessageDialog(message, firstFieldID, WARNING);
                returnval = false;
            } else {
                returnval = true;
            }
       
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;

        });

    return returnval;
}

function UpdateHikinukiDetails() {
    var ajaxTime = new Date().getDate();

    var jsonDataForUpdate = JSON.stringify({
        DW_DRAWING_CD: $("#hfld_DrawingCd").val(),
        PP_PROCESS_PLAN_CD: $("#hfID_ProcessPlanCD").val(),
        DW_MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null,
        DW_START_DATE: ($("#txtStartDate").val().split(" ")[0] + "T" + $("#txtStartDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_START_OPERATOR: $("#ddlStartOperatorNameValue").val(),
        DW_TP_TEST: $("#ddlTPTest").val() ? $("#ddlTPTest").val() : null,
        DW_FURNACE_NO: $("#txtFurnaceNum").val() ? $("#txtFurnaceNum").val() : null,
        DW_NO_OF_COILS: $("#txtNoOfCoils").val() ? $("#txtNoOfCoils").val() : null,
        DW_QTY: $("#txtCoilQuantity").val() ? $("#txtCoilQuantity").val() : null,
        DW_METALLIC_PLATE_CODE: $("#txtMetallicLabelNo").val() ? $("#txtMetallicLabelNo").val() : null,
        DW_CARBON_PERCENTAGE: $("#txtC").val() ? $("#txtC").val() : null,
        DW_MAGNIZ_PERCENTAGE: $("#txtMn").val() ? $("#txtMn").val() : null,
        DW_DIE_DIM: $("#txtDieDim").val() ? $("#txtDieDim").val() : null,
        DW_DIE_SHAPE: $("#ddlDieShape").val() ? $("#ddlDieShape").val() : null,
        DW_DIE_NO: $("#txtDieNo").val() ? $("#txtDieNo").val() : null,
        DW_DIM1: $("#txtDrawDim1").val() ? $("#txtDrawDim1").val() : null,
        DW_DIM2: $("#txtDrawDim2").val() ? $("#txtDrawDim2").val() : null,
        DW_UPPER_LOWER_ROLLER_ANGLE1: $("#txtUpperLowerRollerAngle1").val() ? $("#txtUpperLowerRollerAngle1").val() : null,
        DW_UPPER_LOWER_ROLLER_ANGLE2: $("#txtUpperLowerRollerAngle2").val() ? $("#txtUpperLowerRollerAngle2").val() : null,
        DW_UPPER_ROLLER_ELEVATION: $("#txtUpperRoleElevation").val() ? $("#txtUpperRoleElevation").val() : null,
        DW_TONNAGE: $("#txtCompressionTonnage").val() ? $("#txtCompressionTonnage").val() : null,
        DW_HEX_R: $("#txtHexR").val() ? $("#txtHexR").val() : null,
        DW_EC_DONE_CHK: ($('#chkECOnline').is(':checked') ? 1 : 0),
        DW_SHIKII_VALUE: $("#txtShikii").val() ? $("#txtShikii").val() : null,
        DW_EC_COIL_SIZE: $("#txtInspectionCoilSize").val() ? $("#txtInspectionCoilSize").val() : null,
        DW_EC_COIL_SHAPE: $("#txtInspectionCoilShape").val() ? $("#txtInspectionCoilShape").val() : null,
        DW_EC_IS_OK: $("#txtIsOK").val() ? $("#txtIsOK").val() : null,

        DW_END_DATE: ($("#txtEndDate").val().split(" ")[0] + "T" + $("#txtEndDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        DW_END_OPERATOR: $("#ddlEndOperatorNameValue").val() ? $("#ddlEndOperatorNameValue").val() : null,
        DW_DECISION: $("#ddlDecision").val() ? $("#ddlDecision").val() : null,
        DW_ACT_DIM_CHK: ($('#chkActualDim').is(':checked') ? 1 : 0),
        DW_ACT_DIM1: $("#txtActualDim1").val() ? $("#txtActualDim1").val() : null,
        DW_ACT_DIM2: $("#txtActualDim2").val() ? $("#txtActualDim2").val() : null,
        DW_ACT_DIM3: $("#txtActualDim3").val() ? $("#txtActualDim3").val() : null,
        DW_ACT_DIM4: $("#txtActualDim4").val() ? $("#txtActualDim4").val() : null,
        DW_ACT_DIM5: $("#txtActualDim5").val() ? $("#txtActualDim5").val() : null,
        DW_ACT_DIM6: $("#txtActualDim6").val() ? $("#txtActualDim6").val() : null,
        DW_ACT_LENGTH_CHK: ($('#chkActualLength').is(':checked') ? 1 : 0),
        DW_ACT_LENGTH1: $("#txtActualLength1").val() ? $("#txtActualLength1").val() : null,
        DW_ACT_LENGTH2: $("#txtActualLength2").val() ? $("#txtActualLength2").val() : null,
        DW_ACT_LENGTH3: $("#txtActualLength3").val() ? $("#txtActualLength3").val() : null,
        DW_FINISHED_BARS: $("#txtFinishedBarCount").val() ? $("#txtFinishedBarCount").val() : null,
        DW_UNIT_TYPE: $("#ddlUnitType").val() ? $("#ddlUnitType").val() : null,
        DW_FINISHED_QTY: $("#txtFinishedQuantity").val() ? $("#txtFinishedQuantity").val() : null,
        DW_FINISHED_QTY_CHK: ($("#chkFinishedQuantity").is(':checked') ? 1 : 0),
        DW_CORRECTED_BARS: $("#txtCorrectedBars").val() ? $("#txtCorrectedBars").val() : null,
        DW_NG_BARS1: $("#txtNGQuantity1").val() ? $("#txtNGQuantity1").val() : null,
        DW_NG_BARS2: $("#txtNGQuantity2").val() ? $("#txtNGQuantity2").val() : null,
        DW_NG_BARS3: $("#txtNGQuantity3").val() ? $("#txtNGQuantity3").val() : null,
        DW_NG_DETAILS1: $("#ddlNGReason1").val() > 0 ? $("#ddlNGReason1").val() : null,
        DW_NG_DETAILS2: $("#ddlNGReason2").val() > 0 ? $("#ddlNGReason2").val() : null,
        DW_NG_DETAILS3: $("#ddlNGReason3").val() > 0 ? $("#ddlNGReason3").val() : null,
        DW_A_SIDE_FURE1: $("#txtAEdgeDistortion1").val() ? $("#txtAEdgeDistortion1").val() : null,
        DW_A_SIDE_FURE2: $("#txtAEdgeDistortion2").val() ? $("#txtAEdgeDistortion2").val() : null,
        DW_A_SIDE_FURE3: $("#txtAEdgeDistortion3").val() ? $("#txtAEdgeDistortion3").val() : null,
        DW_B_SIDE_FURE1: $("#txtBEdgeDistortion1").val() ? $("#txtBEdgeDistortion1").val() : null,
        DW_B_SIDE_FURE2: $("#txtBEdgeDistortion2").val() ? $("#txtBEdgeDistortion2").val() : null,
        DW_B_SIDE_FURE3: $("#txtBEdgeDistortion3").val() ? $("#txtBEdgeDistortion3").val() : null,
        DW_TABA_NO_1: $("#txtThaba1").val() ? $("#txtThaba1").val() : null,
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val() : null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        DW_BEND_STATUS: $("#ddlBendStatus").val() ? $("#ddlBendStatus").val() : null,
        DW_EXT_TEXTURE_STATUS: $("#ddlExternalSurfaceCondition").val() ? $("#ddlExternalSurfaceCondition").val() : null,
        DW_DISPOSAL_NO: $("#txtDisposalBars").val() ? $("#txtDisposalBars").val() : null,
        DW_EC_NG_BAR_TOTAL: $("#txtECNGBars").val() ? $("#txtECNGBars").val() : null,
        DW_MAGNETISM: $("#txtMagnetism").val() ? $("#txtMagnetism").val() : null,
        DW_REMARKS: $("#txtRemarks").val() ? $("#txtRemarks").val() : null,
        DW_NEXT_PROCESS: $("#ddlNextProcess").val() ? $("#ddlNextProcess").val() : null,
        DW_REDRAWING: ($('#chkReHikinuki').is(':checked') ? 1 : 0),
        DW_MACHINE_CD: $("#ddlModel").val(),
        DW_USER_ID: $("#hdfLoginId").val()
    });
    

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/HikinukiDetailsUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataForUpdate
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            
            if (parseInt(response.message) > 0) {
                $("#hfld_MessageCD").val(response.message)
            } else {
                $("#hfld_MessageCD").val("")
            }
            

            var successMessage = $("#lblSuccessMessageS_HIKI_EDIT_DELETE_001").text();
            ShowMessageDialog(successMessage, "btnInitialize", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

$("#btnDelete").click(function (event) {

    DeleteHikinukiDetails();
    btnSearch_Click();

});

function DeleteHikinukiDetails() {
    var ajaxTime = new Date().getTime();
    var jsonData = JSON.stringify({
        DW_DRAWING_CD: $("#hfld_DrawingCd").val(),
        PP_PROCESS_PLAN_CD: $("#hfID_ProcessPlanCD").val(),
        MACHINE_CD: $("#ddlModel").val(),
        DW_TABA_NO_1:$("#txtThaba1").val(),
        DW_TABA_NO_2: $("#txtThaba2").val() ? $("#txtThaba2").val():null,
        DW_TABA_NO_3: $("#txtThaba3").val() ? $("#txtThaba3").val() : null,
        DW_TABA_NO_4: $("#txtThaba4").val() ? $("#txtThaba4").val() : null,
        DW_MSG_CD: $("#hfld_MessageCD").val() ? $("#hfld_MessageCD").val() : null

    });

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/HikinukiDetailsDelete",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonData
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            DisableEditDeleteButtons();

            var successMessage = $("#lblSuccessMessageS_HIKI_EDIT_DELETE_002").text();
            ShowMessageDialog(successMessage, "", SUCCESS);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;

        });
}
$("#btnInitialize").click(function (event) {
    SwitchTab($("#TbPMSEditDelTab2-tab"), $("#TbPMSEditDelTab1-tab"));

    Initialize();
    $("#txtChuubanSearch").focus();

});
function Initialize() {
    combinedDecisionOK = "";
    ClearPanel($("#pnlModel"));
    ClearPanel($("#pnlProductDetails"));
    ClearPanel($("#pnlProcessStartData"));
    ClearPanel($("#pnlProcessEndData"));
    DisableStartDataPanel();
    DisableEndDataPanel();
    
    DisableEditDeleteButtons();
    ClearGridFilters();
    FillEmptyGrid();
}
function FillEmptyGrid() {
    var emptyList = [];
    var settings1 = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/HikinukiCoilToBar/GridSearch",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(emptyList)

    }
    $.ajax(settings1)
        .done(function (response1) {
            if (response1.isSuccess) {
                $("#divHikinukiGridMain").html(response1.message);

            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            ShowMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
}

function ClearGridFilters() {
    $("#txtChuubanSearch").val("");
    $("#txtCustomerName").val("");
    $("#txtProdNameCust").val("");
    $("#txtProdNameInt").val("");
    $("#txtDimDMin").val("");
    $("#txtDimDMax").val("");
    $("#txtShap").val("");
    $("#txtLengthSearchMin").val("");
    $("#txtStartDate").val("");
    $("#txtEndDate").val("");
    $("#txtLengthSearchMax").val("");
}
function ClearPanel($panel) {
    // Set specified panel elements to disabled
    $.each($panel.find("input:text"), function (index, textBox) {
        $(textBox).val("");
    });
    $.each($panel.find("select"), function (index, dropdown) {
        $("#" + dropdown.id).empty();

    });
    $.each($panel.find("input:checkbox"), function (index, checkBox) {
        $(checkBox).prop('checked', false);
    });
    // Fllowing is to clear hidden fields
    $.each($panel.find("input:hidden"), function (index, hidden) {
        $(hidden).val("");
    });
    // Fllowing is to clear readonly labels that are shown as textboxes using css
    $.each($panel.find("label[id^='txt']"), function (index, label) {
        $(label).text("");
    });
}
function btnSortOk_Click() {
    var selectedColumns = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            selectedColumns.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });
    HikinukiGridSearch(selectedColumns);
}
function ToggleClassForMagnetism() {
    if ($("#chkECOnline").prop("checked") == true) {
        if (!$("#txtMagnetism").hasClass("Mandatory")) {
            $("#txtMagnetism").addClass("Mandatory");
        }
        $("#txtECNGBars").addClass("Mandatory");
        if ($("#txtECNGBars").val()=="")
        {
            $("#txtECNGBars").val(0);
        }
    } else {
        $("#txtMagnetism").removeClass("Mandatory");
        $("#txtECNGBars").removeClass("Mandatory");
        $("#txtECNGBars").val("");
    }
}
$("#chkECOnline").click(function () {
    ToggleClassForMagnetism();
});

$('#txtChuubanSearch').keypress(function (e) {
    var orderNo = $('#txtChuubanSearch').val() ? $('#txtChuubanSearch').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HikinukiCoilToBar/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuubanSearch').val(jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#txtCustomerName').focus();
                } else {
                    $('#txtChuubanSearch').val("");

                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
                return false;
            });
    }

});