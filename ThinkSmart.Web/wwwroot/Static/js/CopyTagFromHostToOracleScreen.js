var $btnChuubanSearch;
var displayProcessPlanData;
var processName;
var nifudaNo;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var FIRST_PROCESS_CD;

var messageWithRowNo = "";
var messageWithChubanGyouban = "";
var NotMatchMsgWithChubanGyouban = "";
var processPlanDetails = [];
var processPlanHostData = [];
var chubbanData = [];
var nifudaNotFoundHost = "";
var nifudaAlreadyCopied = "";
var nifudaNotFoundLiveData = "";
var nifudaNotFound= "";
var repeatedMsg1 = "";
var repeatedMsg2 = "";
var repeatedMsg3 = "";
var repeatedMsg4 = "";
var gridRowsCnt = 30;

$(document).ready(function () {
    window.resizeTo(1430, 1100);
    disableButton();
    $('#rdoSearchNifudaFromHost').attr("checked", "checked");
});
function disableButton() {
    $("#btnInput").attr("DISABLED", "DISABLED");
    $("#btnCancel").attr("DISABLED", "DISABLED");
    
}

function clickOnBeforeEntryChange() {
    $('#rdoSearchNifudaFromHost').attr("checked", "checked");
    $('#rdoSearchAlreadyCopiedNifuda').removeAttr("checked").removeClass("checked");
    $("#btnCancel").attr("DISABLED", "DISABLED");
   // $("#btnInput").removeAttr("DISABLED").removeClass("DISABLED");
}
function clickOnAfterEntryChange() {
    $('#rdoSearchAlreadyCopiedNifuda').attr("checked", "checked");
    $('#rdoSearchNifudaFromHost').removeAttr("checked").removeClass("checked");
    $("#btnInput").attr("DISABLED", "DISABLED");
  //  $("#btnCancel").removeAttr("DISABLED").removeClass("DISABLED");
}

$("#btnSearchByNifudaNo").click(function (event) {
    var $callFrom = $(this);
    event.preventDefault();
    var isOrderNoExist = false;
    var duplicateNifuda = [];
    var message = "";
        nifudaNo = [];
    var ajaxTime = new Date().getDate();
    for (var i = 1; i <= gridRowsCnt; i++) {
        chuban1 = $("#txtNifudaNo" + i).val();
        if ($("#txtNifudaNo" + i).val() != "") {
            nifudaNo.push(chuban1);
        }

    }
    debugger;
    if (nifudaNo.length > 0) {
        
        var chk = new Set();
        duplicateNifuda = nifudaNo.filter(n => chk.size === chk.add(n).size);
        if (duplicateNifuda != "") {
            message += "<br>" + $("#lbl_ValMsgNifudaIsDuplicate").text().replace("99999999", "<br>" + duplicateNifuda); 
        }


        if (message != "") {
            showMessageDialog(message, "", WARNING);
        }
       
        else {
            getDetailsByNifuda();
        }
    }
    else {
        debugger;
        message = $("#lbl_ValMsgNifudaIsRequired").text().trim();
        showMessageDialog(message, "txtNifudaNo", WARNING);
        return false;
    }

});
function getDetailsByNifuda() {
    var message = "";
    processPlanHostData = [];
    processPlanDetails = [];
    var repeatedMsg1 = "";
    var repeatedMsg2 = "";
    var repeatedMsg3 = "";
    var repeatedMsg4 = "";
    debugger;
    var nifudaList = [];
    notfoundNifuda = [];
    nifudaNotFoundLiveData = "";
    nifudaNotFoundHost = "";
    nifudaAlreadyCopied = "";
    nifudaNotFound =""
    for (var i = 1; i <= gridRowsCnt; i++) {
        var nifuda = $("#txtNifudaNo" + i).val();
        if ($('#rdoSearchNifudaFromHost').is(':checked')) {
            displayProcessPlanData = "beforeProcessStart";
        }
        else {
            displayProcessPlanData = "afterProcessStart";
        }
        var jsonPlanData = ({
            NBNNO_NBNGYO: nifuda,
            DISPLAYPROCESSPLANDATA: displayProcessPlanData
        });
        if ($("#txtNifudaNo" + i).val() != "") {
            processPlanDetails.push(jsonPlanData);
        }
    }


   

    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/CopyTagFromHostToOracleScreen/GetDetailsByNifuda",
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
                    if (jsonresult[p].responce == "Tag No Not Found From Host") {
                        focusFieldID = $(':focus').attr("id");
                        
                        if (repeatedMsg1 != "") {
                            nifudaNotFoundHost = nifudaNotFoundHost + ",<br>" + jsonresult[p].nbnnO_NBNGYO;
                        }
                        else {
                            repeatedMsg1 = "Found";
                            nifudaNotFoundHost = jsonresult[p].nbnnO_NBNGYO;

                        }

                    }
                    else if (jsonresult[p].responce == "Tag No.Are already copied") {
                        focusFieldID = $(':focus').attr("id");
                       
                        if (repeatedMsg2 != "") {
                            nifudaAlreadyCopied = nifudaAlreadyCopied + ",<br>" + jsonresult[p].nbnnO_NBNGYO;
                        }
                        else {
                            repeatedMsg2 = "Found";
                            nifudaAlreadyCopied = jsonresult[p].nbnnO_NBNGYO;

                        }

                    }
                    else if (jsonresult[p].responce == "Tag No Can Not Data  Insert No live data found in Process plan  table") {
                        focusFieldID = $(':focus').attr("id");

                        if (repeatedMsg3 != "") {
                            nifudaNotFoundLiveData = nifudaNotFoundLiveData + ",<br>" + jsonresult[p].nbnnO_NBNGYO;
                        }
                        else {
                            repeatedMsg3 = "Found";
                            nifudaNotFoundLiveData = jsonresult[p].nbnnO_NBNGYO;

                        }

                    }
                    else if (jsonresult[p].responce == "Tag Not Found") {
                        focusFieldID = $(':focus').attr("id");

                        if (repeatedMsg4 != "") {
                            nifudaNotFound = nifudaNotFound + ",<br>" + jsonresult[p].nbnnO_NBNGYO;
                        }
                        else {
                            repeatedMsg4 = "Found";
                            nifudaNotFound = jsonresult[p].nbnnO_NBNGYO;

                        }

                    }
                    for (let i = 1; i <= gridRowsCnt; i++) {

                        //if (nifudaList[i] == jsonresult[p].nbnnO_NBNGYO) {

                        if (jsonresult[p].nbnnO_NBNGYO == $("#txtNifudaNo" + parseInt(i)).val().toString() && jsonresult[p].chuubaN_GYOUBAN != null) {

                            if ($('#rdoSearchNifudaFromHost').is(':checked')) {
                                $("#btnInput").removeAttr("DISABLED").removeClass("DISABLED");
                            }
                            else {
                                $("#btnCancel").removeAttr("DISABLED").removeClass("DISABLED");
                            }
                                if (jsonresult[p].hpO_INZU == null) {
                                    $("#txtCustomer" + parseInt(i)).text(jsonresult[p].customeR_NAME);
                                    $("#txtchuuban" + parseInt(i)).text(jsonresult[p].chuubaN_GYOUBAN);
                                    $("#txtProdName" + parseInt(i)).text(jsonresult[p].proD_NAME_CUST);
                                    $("#txtShape" + parseInt(i)).text(jsonresult[p].proD_SHAPE);
                                    $("#txtDia" + parseInt(i)).text(jsonresult[p].proD_DIM_D);
                                    $("#txtDiaW" + parseInt(i)).text(jsonresult[p].proD_DIM_W);
                                    $("#txtOther" + parseInt(i)).text(jsonresult[p].proD_DIM_OTHER_STRING);
                                    if (jsonresult[p].proD_LENGTH != null) {
                                        $("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH);
                                    }
                                    else {
                                        $("#txtLen" + parseInt(i)).text(jsonresult[p].lengtH_OTHER);
                                    }
                                    //$("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH);
                                    $("#txtCondition" + parseInt(i)).text(jsonresult[p].proC_CONDITION_STRING);
                                    $("#txtCharge" + parseInt(i)).text(jsonresult[p].charge);
                                    if ($("#hdfCultureName").val() == "en") {
                                        if (jsonresult[p].uniT_TYPE_STRING == "C") {
                                            Unit_Type = $.trim($("#lblUnitCoil").text());
                                        }
                                        else if (jsonresult[p].uniT_TYPE_STRING == "B") {
                                            Unit_Type = $.trim($("#lblUnitBar").text());
                                        }
                                        else if (jsonresult[p].uniT_TYPE_STRING == "BN") {
                                            Unit_Type = $.trim($("#lblUnitBundle").text());
                                        }
                                        else {
                                            Unit_Type = $.trim($("#lblUnitPieces").text());
                                        }
                                    }
                                    else {
                                        if (jsonresult[p].uniT_TYPE_STRING == "C") {
                                            Unit_Type = $.trim($("#lblUnitCoil").text());
                                        }
                                        else if (jsonresult[p].uniT_TYPE_STRING == "B") {
                                            Unit_Type = $.trim($("#lblUnitBar").text());
                                        }
                                        else if (jsonresult[p].uniT_TYPE_STRING == "BN") {
                                            Unit_Type = $.trim($("#lblUnitBundle").text());
                                        }
                                        else if (jsonresult[p].uniT_TYPE_STRING == null) {
                                            Unit_Type = null;
                                        }
                                        else {
                                            Unit_Type = $.trim($("#lblUnitPieces").text());
                                        }
                                    }
                                    $("#txtActAllPiec" + parseInt(i)).text(jsonresult[p].nO_OF_UNITS + Unit_Type);
                                    $("#txtActAllWt" + parseInt(i)).text(jsonresult[p].acT_ALLOC_QTY);
                                    $("#txtActAllDt" + parseInt(i)).text(jsonresult[p].acT_ALLOC_DATE_STRING);
                                    $("#txtProdSpecNo" + parseInt(i)).text(jsonresult[p].proD_SPEC_NO1 + "-" + jsonresult[p].proD_SPEC_NO2);
                                    $("#txtProcessPlanCd" + parseInt(i)).text(jsonresult[p].procesS_PLAN_CD_HOST);
                                    $("#txtLocationCd" + parseInt(i)).text(jsonresult[p].locatioN_CD);
                                    $("#txtActAllPieces" + parseInt(i)).text(jsonresult[p].nO_OF_UNITS);
                                    $("#txtActAllPiecesUnit" + parseInt(i)).text(jsonresult[p].unit);
                                    jsonresult[p].griD_ROW_NO = i;
                                    processPlanHostData.push(jsonresult[p]);
                                    console.log(processPlanHostData);

                                }
                                else {
                                    $("#txtCustomer" + parseInt(i)).text(jsonresult[p].customeR_NAME);
                                    $("#txtchuuban" + parseInt(i)).text(jsonresult[p].chuubaN_GYOUBAN);
                                    $("#txtProdName" + parseInt(i)).text(jsonresult[p].proD_NAME_CUST);
                                    $("#txtShape" + parseInt(i)).text(jsonresult[p].shape);
                                    $("#txtDia" + parseInt(i)).text(jsonresult[p].proD_DIM_D);
                                    $("#txtDiaW" + parseInt(i)).text(jsonresult[p].proD_DIM_W);
                                    $("#txtOther" + parseInt(i)).text(jsonresult[p].proD_DIM_OTHER_STRING);
                                    //$("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH_STRING);
                                    if (jsonresult[p].proD_LENGTH_STRING != null) {
                                        $("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH_STRING);
                                    }
                                    else {
                                        $("#txtLen" + parseInt(i)).text(jsonresult[p].lengtH_OTHER);
                                    }
                                    $("#txtCondition" + parseInt(i)).text(jsonresult[p].proC_CONDITION_STRING);
                                    $("#txtCharge" + parseInt(i)).text(jsonresult[p].charge);
                                    if ($("#hdfCultureName").val() == "en") {
                                        if (jsonresult[p].hosT_UNIT_TYPE_STRING == "C") {
                                            Unit_Type = $.trim($("#lblUnitCoil").text());
                                        }
                                        else if (jsonresult[p].hosT_UNIT_TYPE_STRING == "P") {
                                            Unit_Type = $.trim($("#lblUnitBar").text());
                                        }
                                        else if (jsonresult[p].hosT_UNIT_TYPE_STRING == "B") {
                                            Unit_Type = $.trim($("#lblUnitBundle").text());
                                        }
                                        else {
                                            Unit_Type = $.trim($("#lblUnitPieces").text());
                                        }
                                    }
                                    else {
                                        if (jsonresult[p].hosT_UNIT_TYPE_STRING == "C") {
                                            Unit_Type = $.trim($("#lblUnitCoil").text());
                                        }
                                        else if (jsonresult[p].hosT_UNIT_TYPE_STRING == "P") {
                                            Unit_Type = $.trim($("#lblUnitBar").text());
                                        }
                                        else if (jsonresult[p].hosT_UNIT_TYPE_STRING == "B") {
                                            Unit_Type = $.trim($("#lblUnitBundle").text());
                                        }
                                        else {
                                            Unit_Type = $.trim($("#lblUnitPieces").text());
                                        }
                                    }
                                    $("#txtActAllPiec" + parseInt(i)).text(jsonresult[p].hpO_INZU + Unit_Type);
                                    $("#txtActAllWt" + parseInt(i)).text(jsonresult[p].acT_ALLOC_QTY);
                                    var actAllocationDate = (jsonresult[p].hosT_ACT_ALLOC_DATE).slice(0, 10).replaceAll('-', '/');
                                    $("#txtActAllDt" + parseInt(i)).text(actAllocationDate);
                                    $("#txtProdSpecNo" + parseInt(i)).text(jsonresult[p].proD_SPEC_NO1 + "-" + jsonresult[p].proD_SPEC_NO2);
                                    $("#txtInzu" + parseInt(i)).text(jsonresult[p].hpO_INZU);
                                    //$("#txtCondition" + parseInt(i)).text(jsonresult[p].matL_CONDITION_CD);
                                    $("txtNifudaNo" + parseInt(i)).text(jsonresult[p].nbnnO_NBNGYO);
                                    jsonresult[p].griD_ROW_NO = i;
                                    processPlanHostData.push(jsonresult[p]);
                                    console.log(processPlanHostData);
                                }


                        }
                            

                    }
                }
            }
            if (nifudaNotFoundHost != "") {
                message = $.trim($("#lbl_valMsgNifudaNotFound").text()).replace("9999999", "<br>" + nifudaNotFoundHost);
            }

            if (nifudaAlreadyCopied != "") {

                if (message != "") {
                    message = message + "<br>" + $.trim($("#lbl_ValMsgNifudaAlreadyCopied").text()).replace("9999999", "<br>" + nifudaAlreadyCopied);
                } else {
                    message = $.trim($("#lbl_ValMsgNifudaAlreadyCopied").text()).replace("9999999", "<br>" + nifudaAlreadyCopied);
                }
            }
            if (nifudaNotFoundLiveData != "") {

                if (message != "") {
                    message = message + "<br>" + $.trim($("#lbl_ValMsgNifudaNotFoundLiveTable").text()).replace("9999999", "<br>" + nifudaNotFoundLiveData);
                } else {
                    message = $.trim($("#lbl_ValMsgNifudaNotFoundLiveTable").text()).replace("9999999", "<br>" + nifudaNotFoundLiveData);
                }
            }
            if (nifudaNotFound != "") {

                if (message != "") {
                    message = message + "<br>" + $.trim($("#lbl_valMsgNifudaNotFound").text()).replace("9999999", "<br>" + nifudaNotFound);
                } else {
                    message = $.trim($("#lbl_valMsgNifudaNotFound").text()).replace("9999999", "<br>" + nifudaNotFound);
                }
            }
            if (message != "") {
                showMessageDialog(message, "", WARNING);
            }
        

           
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lbl_ValMsgSystemException").text().trim();
            showMessageDialog(message, "", ERROR);

        });

}
$("#btnInput").click(function (event) {
    SaveNifudaData();
    


});
function SaveNifudaData() {
    debugger;
    var isddlProcessNotEmpty = false;
    jsonPlanData = [];
    processPlanDetails = [];
    var ajaxTime = new Date().getTime();
    for (var i = 1; i <= gridRowsCnt; i++) {
        FIRST_PROCESS_CD = $("#ddlNextProcess" + i).val();
        if (FIRST_PROCESS_CD == "") {
            isddlProcessNotEmpty = false;
            focusFieldID = $(':focus').attr("id");
            message = $("#lblInRowProcessIsRequired").text().trim();
            messageWithRowNo = message.replace("NNN", i);
            showMessageDialog(messageWithRowNo, i, WARNING);
            break;
        }
        else {
            isddlProcessNotEmpty = true;

        }
    }
    if (isddlProcessNotEmpty == true) {
        debugger;
        //var INPUT_LOCATION_CD = $("#hdfLoginId").val();
        var USER_ID = $("#hdfLoginId").val();
        for (p = 0; p < processPlanHostData.length; p++) {
            for (var i = 1; i <= gridRowsCnt; i++) {
                var Process_CD = $("#ddlNextProcess" + i).val();
                if (processPlanHostData[p].chuubaN_GYOUBAN == $("#txtchuuban" + i).val()) {
                    processPlanHostData[p].nexT_PROCESS_CD = Process_CD;
                }
            }
        }
        for (p = 0; p < processPlanHostData.length; p++) {
            var jsonHostData = ({
                HPO_INZU: processPlanHostData[p].hpO_INZU,
                CHUUBAN: processPlanHostData[p].chuuban,
                GYOUBAN: processPlanHostData[p].gyoubaN_HOST,
                PROD_SPEC_NO1: processPlanHostData[p].proD_SPEC_NO1,
                PROD_SPEC_NO2: processPlanHostData[p].proD_SPEC_NO2,
                PROD_NAME_CUST: processPlanHostData[p].proD_NAME_CUST,
                PROD_SHAPE: processPlanHostData[p].shape,
                PROD_DIM_D: processPlanHostData[p].proD_DIM_D,
                ACT_ALLOC_QTY: processPlanHostData[p].acT_ALLOC_QTY,
                HOST_UNIT_TYPE_STRING: processPlanHostData[p].hosT_UNIT_TYPE_STRING,
                CHARGE: processPlanHostData[p].charge,
                ACT_ALLOC_DATE: processPlanHostData[p].hosT_ACT_ALLOC_DATE,
                PROCESS_CD: processPlanHostData[p].nexT_PROCESS_CD,
                CHARGE: processPlanHostData[p].charge,
                MATL_SHAPE: processPlanHostData[p].matL_SHAPE,
                HOST_MATL_DIM_D: processPlanHostData[p].hosT_MATL_DIM_D,
                HOST_MATL_DIM_W: processPlanHostData[p].hosT_MATL_DIM_W,
                MATL_DIM_OTHER: processPlanHostData[p].matL_DIM_OTHER,
                MATERIAL_LENGTH: processPlanHostData[p].materiaL_LENGTH,
                UNIT_WT: processPlanHostData[p].uniT_WT,
                COLOR_NAME: processPlanHostData[p].coloR_NAME,
                MAGARI_KUBUN_STRING: processPlanHostData[p].magarI_KUBUN_STRING,
                MAGARI_SPAN_STRING: processPlanHostData[p].magarI_SPAN_STRING,
                MAGARI_BEND_TOL_STRING: processPlanHostData[p].magarI_BEND_TOL_STRING,
                EXT_TEXTURE: processPlanHostData[p].exT_TEXTURE,
                DISPLAY: processPlanHostData[p].display,
                TEST_SPECIMEN: processPlanHostData[p].tesT_SPECIMEN,
                PROD_POINT_1: processPlanHostData[p].proD_POINT_1,
                PROD_POINT_2: processPlanHostData[p].proD_POINT_2,
                PROD_POINT_3: processPlanHostData[p].proD_POINT_3,
                PROD_POINT_4: processPlanHostData[p].proD_POINT_4,
                PROD_POINT_5: processPlanHostData[p].proD_POINT_5,
                PROD_POINT_6: processPlanHostData[p].proD_POINT_6,
                PROD_POINT_7: processPlanHostData[p].proD_POINT_7,
                PROD_POINT_8: processPlanHostData[p].proD_POINT_8,
                PROD_POINT_9: processPlanHostData[p].proD_POINT_9,
                PROD_POINT_10: processPlanHostData[p].proD_POINT_10,
                PROD_POINT_11: processPlanHostData[p].proD_POINT_11,
                PROD_POINT_12: processPlanHostData[p].proD_POINT_12,
                PROD_POINT_13: processPlanHostData[p].proD_POINT_13,
                MATL_NAME_MAK: processPlanHostData[p].matL_NAME_MAK,
                PROCESS_1_CD: processPlanHostData[p].procesS_1_CD,
                PROCESS_2_CD: processPlanHostData[p].procesS_2_CD,
                PROCESS_3_CD: processPlanHostData[p].procesS_3_CD,
                PROCESS_4_CD: processPlanHostData[p].procesS_4_CD,
                PROCESS_5_CD: processPlanHostData[p].procesS_5_CD,
                PROCESS_6_CD: processPlanHostData[p].procesS_6_CD,
                PROCESS_7_CD: processPlanHostData[p].procesS_7_CD,
                PROCESS_8_CD: processPlanHostData[p].procesS_8_CD,
                PROCESS_9_CD: processPlanHostData[p].procesS_9_CD,
                PROCESS_10_CD: processPlanHostData[p].procesS_10_CD,
                PROCESS_11_CD: processPlanHostData[p].procesS_11_CD,
                PROCESS_12_CD: processPlanHostData[p].procesS_12_CD,
                PROCESS_13_CD: processPlanHostData[p].procesS_13_CD,
                CUSTOMER_CD: processPlanHostData[p].customeR_CD,
                RECEIVER_CD: processPlanHostData[p].receiveR_CD,
                DEPARTMENT_GROUP_CD: processPlanHostData[p].departmenT_GROUP_CD,
                MATL_TYPE_CBK: processPlanHostData[p].matL_TYPE_CBK,
                MATL_NAME_MAKER_CD: processPlanHostData[p].matL_NAME_MAKER_CD,
                MATL_CONDITION_CD: processPlanHostData[p].matL_CONDITION_CD,
                PRODUCT_CD: processPlanHostData[p].producT_CD,
                ROOT_VER_PRODUCT_KEY: processPlanHostData[p].rooT_VER_PRODUCT_KEY,
                INTERNAL_NAME_CD: processPlanHostData[p].internaL_NAME_CD,
                PROD_CONDITION_CD: processPlanHostData[p].proD_CONDITION_CD,
                VER: processPlanHostData[p].ver,
                STD_DEL_PERIOD: processPlanHostData[p].stD_DEL_PERIOD,
                PROD_DIM_W: processPlanHostData[p].proD_DIM_W,
                PROD_DIM_OTHER_STRING: processPlanHostData[p].proD_DIM_OTHER_STRING,
                DIM_TOL_MIN: processPlanHostData[p].diM_TOL_MIN,
                DIM_TOL_MAX: processPlanHostData[p].diM_TOL_MAX,
                DIM_TOL_MIN_CHR: processPlanHostData[p].diM_TOL_MIN_CHR,
                DIM_TOL_MAX_CHR: processPlanHostData[p].diM_TOL_MAX_CHR,
                PROD_LENGTH_STRING: processPlanHostData[p].proD_LENGTH_STRING,
                LENGTH_OTHER: processPlanHostData[p].lengtH_OTHER,
                LENGTH_TOL_MIN: processPlanHostData[p].hosT_LENGTH_TOL_MIN,
                LENGTH_TOL_MAX: processPlanHostData[p].hosT_LENGTH_TOL_MAX,
                LENGTH_TOL_MIN_CHR: processPlanHostData[p].lengtH_TOL_MIN_CHR,
                LENGTH_TOL_MAX_CHR: processPlanHostData[p].lengtH_TOL_MAX_CHR,
                COIL_DIA_INT: processPlanHostData[p].coiL_DIA_INT,
                COIL_DIA_EXT: processPlanHostData[p].coiL_DIA_EXT,
                COIL_WND_DIR: processPlanHostData[p].coiL_WND_DIR,
                //CHAMFERING_CD: processPlanHostData[p].chamferinG_CD,
                //CHAMFERING: processPlanHostData[p].chamferinG, 
                NEJIRE_ANGLE: processPlanHostData[p].nejirE_ANGLE,
                NEJIRE_SPAN: processPlanHostData[p].nejirE_SPAN,
                UNIT_WT_KUBUN: processPlanHostData[p].uniT_WT_KUBUN,
                UNIT_WT_MIN: processPlanHostData[p].uniT_WT_MIN,
                UNIT_WT_MAX: processPlanHostData[p].uniT_WT_MAX,
                PACKING_CD: processPlanHostData[p].packinG_CD,
                TENSILE_STRN_PATTERN1_STRING: processPlanHostData[p].tensilE_STRN_PATTERN1_STRING,
                //TENSILE_STRN_PATTERN1_MIN_MAX: processPlanHostData[p].tensilE_STRN_PATTERN1_MIN_MAX, 
                //TENSILE_STRN_PATTERN2_MIN: processPlanHostData[p].tensilE_STRN_PATTERN2_MIN, 
                //TENSILE_STRN_PATTERN2_MAX: processPlanHostData[p].tensilE_STRN_PATTERN2_MAX, 
                //TENSILE_STRN_PATTERN3_ACTUAL: processPlanHostData[p].tensilE_STRN_PATTERN3_ACTUAL, 
                ELONGATION_1: processPlanHostData[p].elongatioN_1,
                ELONGATION_2: processPlanHostData[p].elongatioN_2,
                //ELONGATION_PATTERN2_MIN: processPlanHostData[p].elongatioN_PATTERN2_MIN,
                //ELONGATION_PATTERN2_MAX: processPlanHostData[p].elongatioN_PATTERN2_MAX,
                //ELONGATION_PATTERN3_ACTUAL: processPlanHostData[p].elongatioN_PATTERN3_ACTUAL,
                PRESS_1: processPlanHostData[p].presS_1,
                PRESS_2: processPlanHostData[p].presS_2,
                HARDNESS_1: processPlanHostData[p].hardnesS_1,
                HARDNESS_2: processPlanHostData[p].hardnesS_2,
                HARDNESS_3: processPlanHostData[p].hardnesS_3,
                //HARDNESS_PATTERN1: processPlanHostData[p].hardnesS_PATTERN1_CODE,
                //HARDNESS_PATTERN1_MIN_MAX: processPlanHostData[p].hardnesS_PATTERN1_MIN_MAX,
                //HARDNESS_PATTERN2_CODE: processPlanHostData[p].hardnesS_PATTERN2_CODE,
                //HARDNESS_PATTERN2_MIN: processPlanHostData[p].hardnesS_PATTERN2_MIN,
                //HARDNESS_PATTERN2_MAX: processPlanHostData[p].hardnesS_PATTERN2_MAX,
                //HARDNESS_PATTERN3_ACTUAL: processPlanHostData[p].hardnesS_PATTERN3_ACTUAL,
                YEILD: processPlanHostData[p].yeild,
                //YEILD_PATTERN1_MIN_MAX: processPlanHostData[p].yeilD_PATTERN1_MIN_MAX,
                //YEILD_PATTERN2_MIN: processPlanHostData[p].yeilD_PATTERN2_MIN,
                //YEILD_PATTERN2_MAX: processPlanHostData[p].yeilD_PATTERN2_MAX,
                DECARB_1: processPlanHostData[p].decarB_1,
                DECARB_2: processPlanHostData[p].decarB_2,
                DECARB_3: processPlanHostData[p].decarB_3,
                DECARB_4: processPlanHostData[p].decarB_4,
                //DECARB_3: processPlanHostData[p].decarB_3,
                //DECARB_VAL_3: processPlanHostData[p].decarB_VAL_3,
                IMPACT_TEST_PATTERN1: processPlanHostData[p].impacT_TEST_PATTERN1,
                IMPACT_TEST_PATTERN1_MIN_MAX: processPlanHostData[p].impacT_TEST_PATTERN1_MIN_MAX,
                IMPACT_TEST_PATTERN2_MIN: processPlanHostData[p].impacT_TEST_PATTERN2_MIN,
                IMPACT_TEST_PATTERN2_MAX: processPlanHostData[p].impacT_TEST_PATTERN2_MAX,
                IMPACT_TEST_PATTERN3_ACTUAL: processPlanHostData[p].impacT_TEST_PATTERN3_ACTUAL,
                TEST_SPEC_REMARKS: processPlanHostData[p].tesT_SPEC_REMARKS,
                RANK_STD: processPlanHostData[p].ranK_STD,
                CORNER_RADIUS: processPlanHostData[p].corneR_RADIUS,
                MBEU: processPlanHostData[p].mbeu,
                HT_CONDITION: processPlanHostData[p].hT_CONDITION,
                MACHINE_CD: processPlanHostData[p].machinE_ID,
                USER_ID: USER_ID,
                PRINT_DATE: processPlanHostData[p].prinT_DATE,
                HOST_DELIVERY_DATE: processPlanHostData[p].hosT_DELIVERY_DATE,
                PROCESS_PLAN_KUBUN: processPlanHostData[p].procesS_PLAN_KUBUN,
                STAMP: processPlanHostData[p].stamp,
                EMPLOYEE_CD: processPlanHostData[p].employeE_CD,
                //INPUT_LOCATION_CD: INPUT_LOCATION_CD,
                NBNNO_NBNGYO: processPlanHostData[p].nbnnO_NBNGYO,


            });
            if (processPlanDetails.length != processPlanHostData.length) {
                processPlanDetails.push(jsonHostData);
            }

        }
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/CopyTagFromHostToOracleScreen/SaveNifudaData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(processPlanDetails)
        }
        $.ajax(settings)
            .done(function (response) {
                debugger;
                responseTime = new Date().getTime() - ajaxTime;
                if (response.message == "Data Inserted Successfully") {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblSuccessMessageS_CopyTagHostToOracle_DataSave").text());
                    showMessageDialog(message, focusFieldID, SUCCESS);
                    $("#btnCancel").attr("DISABLED", "DISABLED");
                    $("#btnInput").attr("DISABLED", "DISABLED");
                    //btnInitializeClick();
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lbl_ValMsgSystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });


    }

}
$("#btnCancel").click(function (event) {
    DeleteCopyTagData();
    

});

function DeleteCopyTagData() {
    var $callFrom = $(this);
    event.preventDefault();
    var isOrderNoExist = false;
    var duplicateNifuda = [];
    var message = "";
    nifudaNo = [];
    var ajaxTime = new Date().getDate();
    for (var i = 1; i <= gridRowsCnt; i++) {
        chuban1 = $("#txtNifudaNo" + i).val();
        if ($("#txtNifudaNo" + i).val() != "") {
            nifudaNo.push(chuban1);
        }

    }
    debugger;
    if (nifudaNo.length > 0) {
        jsonPlanData = [];
        processPlanDetails = [];
        messageWithNifudaNo = "";
        var repeatedMsg1 = "";
        nifudaList = [];
        var nifuda_NO = "";
        var ajaxTime = new Date().getTime();
        for (var i = 1; i <= gridRowsCnt; i++) {
            var jsonPlanData = ({
                NBNNO_NBNGYO: $("#txtNifudaNo" + i).val(),
                PROCESS_PLAN_CD: $("#txtProcessPlanCd" + i).text(),

            });
            if ($("#txtNifudaNo" + i).val() != "") {
                processPlanDetails.push(jsonPlanData);
            }

        }
        debugger;
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/CopyTagFromHostToOracleScreen/DeleteCopyTagFromHostToOracleData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(processPlanDetails)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonresult = response.data;

                for (let p = 0; p < jsonresult.length; p++) {
                    var nifudaNo = jsonresult[p].nbnnO_NBNGYO;
                    var isProcessStart = false;
                    if (jsonresult.length == 1) {
                        if (jsonresult[p].responce == "Final Inspection done") {
                            isProcessStart == true;
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#valMsg_FICompletedForNifuda").text());
                            messageWithNifudaNo = message.replace("9999999","<br>"+ nifudaNo);
                            showMessageDialog(messageWithNifudaNo, "", WARNING);
                        }
                        else {
                            focusFieldID = $(':focus').attr("id");
                            messageWithNifudaNo = $.trim($("#lblSuccessMessageS_CopyTagHostToOracle_DataDeleted").text());
                            showMessageDialog(messageWithNifudaNo, "", SUCCESS);
                            disableButton();
                            //btnInitializeClick();
                        }
                    }
                    else {
                        if (jsonresult[p].responce == "Final Inspection done") {
                            if (repeatedMsg1 != "") {


                                nifuda_NO = nifuda_NO + ",<br>" + nifudaNo;


                            }
                            else {
                                repeatedMsg1 = "Found";
                                nifuda_NO = nifudaNo;

                            }
                        }
                        else {
                            focusFieldID = $(':focus').attr("id");
                            messageWithNifudaNo = $.trim($("#lblSuccessMessageS_CopyTagHostToOracle_DataDeleted").text());
                            nifudaList.push(nifudaNo);
                            //btnInitializeClick();
                        }
                    }

                }
                if (jsonresult.length != 1) {

                    if (nifuda_NO != "") {
                        messageWithNifudaNo = $.trim($("#valMsg_FICompletedForNifuda").text()).replace("9999999", "<br>" + nifuda_NO);
                        showMessageDialog(messageWithNifudaNo, "", WARNING);
                    } else {
                        focusFieldID = $(':focus').attr("id");
                        messageWithNifudaNo = $.trim($("#lblSuccessMessageS_CopyTagHostToOracle_DataDeleted").text());
                        showMessageDialog(messageWithNifudaNo, "", SUCCESS);
                        disableButton();
                        //btnInitializeClick();
                    }
                }
                if (nifudaList.length > 0) {
                    for (let i = 0; i < nifudaList.length; i++) {
                        for (let p = 1; p <= gridRowsCnt; p++) {
                            var txt_Nifuda_No = document.getElementById("txtNifudaNo" + parseInt(p)).value;
                            if (nifudaList[i] == txt_Nifuda_No) {
                                $('#txtNifudaNo' + parseInt(p)).val('');
                                $('#txtchuuban' + parseInt(p)).text('');
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
                message = $.trim($("#lbl_ValMsgSystemException").text());
                //showMessageDialog(message, focusFieldID, ERROR);
            });
        
    }
    else {
        debugger;
        message = $("#lbl_ValMsgNifudaIsRequired").text().trim();
        showMessageDialog(message, "txtNifudaNo", WARNING);
        return false;
    }

}


$("#btnInitialize").click(function () {
    btnInitializeClick();
});
function btnInitializeClick() {
    debugger;
    disableButton();
    for (let i = 1; i <= gridRowsCnt; i++) {
        $('#txtchuuban' + parseInt(i)).text('');
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
        $("#txtNifudaNo" + parseInt(i)).val('');
        $("#selectRow" + parseInt(i)).prop("checked", false);
        $('#rdoSearchNifudaFromHost').prop("checked", "checked");
    }
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

//function myFunction(ev, index) {
//    if (ev.type == "input") {
//        var value = document.getElementById("txtNifudaNo" + index).value;
//        if (value.length == 9) {
//            setFocus(index);
//        }
//    }
//    }

//function setFocus(index) {
//    document.getElementById("txtNifudaNo" + (index + 1)).focus();
//}
$(document).keypress(function (e) {
    if ($(e.target).is('[data-nifudano]')) {
        var nifudaNo = e.target.value ? e.target.value : null;
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == 13 && nifudaNo) {
            var ajaxTime = new Date().getTime();
            var settings = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/CopyTagFromHostToOracleScreen/MappNifudaByBarcode?nifudaNo=" + nifudaNo,
                method: "POST",
                contentType: "application/json; charset=utf-8",

            }
            $.ajax(settings)
                .done(function (response) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var jsonResult = response.message;
                    if (response.isSuccess) {
                        e.target.value = jsonResult.nifudA_NO;
                        debugger;
                        let txtID = $(e.target).attr("data-id");
                        if (parseInt(txtID) < gridRowsCnt) {
                            $("#txtNifudaNo" + (parseInt(txtID) + 1)).focus();
                        }

                    } else {
                        e.target.value = "";
                    }

                })
            
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    var message = $.trim($("#lblSystemException").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                    return false;
                });
        }
    }


});
$("#btnDeleteRow").click(function () {
    debugger;
    isSelectRow = false;
    const table = document.getElementById("tblProcessPlanHandover");
    for (let i = 1; i <= gridRowsCnt; i++) {
        checked = $('#selectRow' + i).is(':checked');
        if (checked) {
            ClearRowByIndex(i);

            isSelectRow = true;
        }

    }

    if (isSelectRow == false) {
        var message = $("#valMsgCOMSelectAtLeastOne").text().trim();
        showMessageDialog(message, "", WARNING);
    }
});
function ClearRowByIndex(index) {
    const row = $('#grdvProcessPlanHandoverList tr').eq(index);

    row.find('input[type="text"]').val('');
    $('#txtchuuban' + index).text('');
    $("#txtCustomer" + index).text('');
    $("#txtProdName" + index).text('');
    $("#txtShape" + index).text('');
    $("#txtDia" + index).text('');
    $("#txtDiaW" + index).text('');
    $("#txtOther" + index).text('');
    $("#txtLen" + index).text('');
    $("#txtCondition" + index).text('');
    $("#txtCharge" + index).text('');
    $("#txtActAllPiec" + index).text('');
    $("#txtActAllWt" + index).text('');
    $("#txtActAllDt" + index).text('');
    $("#txtProdSpecNo" + index).text('');
    $("#txtProcessPlanCd" + index).text('');
    $("#txtLocationCd" + index).text('');
    $("#txtActAllPieces" + index).text('');
    $("#txtActAllPiecesUnit" + index).text('');
    $("#txtNifudaNo" + index).val('');
    $("#selectRow" + index).prop("checked", false);

    // processPlanHostData.splice(index - 1, 1); 
    if (processPlanHostData.length > 0) {
        processPlanHostData = processPlanHostData.filter(function (item) {
            return item.griD_ROW_NO != index; // keep everything except the one with id 2
        });
    }
    console.log(processPlanHostData);
}