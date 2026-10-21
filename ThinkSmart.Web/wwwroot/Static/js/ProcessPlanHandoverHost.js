var $btnChuubanSearch;
var displayProcessPlanData;
var processNameList;
var chuban1;
var internalName;
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var FIRST_PROCESS_CD;
var internameNameCD;

var messageWithRowNo = "";
var messageWithChubanGyouban = "";
var NotMatchMsgWithChubanGyouban = "";
var processPlanDetails = [];
var processPlanHostData = [];
var chubbanData = [];
var internalNameList = [];
var valMsgWithInternalName = "";
var chubban = "";
var gyouban = "";
var returnValue = false;
var gridRowsCnt = 30;
$(document).ready(function () {
    window.resizeTo(1890, 1100);
    $("#btnAddNew").attr("DISABLED", "DISABLED");
    $("#btnUpdate").attr("DISABLED", "DISABLED");
    $("#btnCancel").attr("DISABLED", "DISABLED");
    $("#btnSetProcess").attr("DISABLED", "DISABLED");
    //$("#btnDeleteRow").attr("DISABLED", "DISABLED");
    $('#rdoPrcessPlanSearchBeforeEntry').attr("checked", "checked");
    setNextProcessList();
});
function clickOnBeforeEntryChange() {
    $('#rdoPrcessPlanSearchBeforeEntry').attr("checked", "checked");
    $('#rdoPrcessPlanSearchAfterEntry').removeAttr("checked").removeClass("checked");
    //$("#btnCancel").attr("DISABLED", "DISABLED");
    //$("#btnAddNew").removeAttr("DISABLED").removeClass("DISABLED");
}
function clickOnAfterEntryChange() {
    $('#rdoPrcessPlanSearchAfterEntry').attr("checked", "checked");
    $('#rdoPrcessPlanSearchBeforeEntry').removeAttr("checked").removeClass("checked");
    //$("#btnAddNew").attr("DISABLED", "DISABLED");
    //$("#btnCancel").removeAttr("DISABLED").removeClass("DISABLED");
}
function setNextProcessList() {
    var ajaxTime = new Date().getTime();
    var jsonData = ({
        PROCESS_KUBUN_CD: $("#hdStartProcessCD").val()
    });
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessPlanHandoverHost/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            processNameList = response.data;
            //bindNextProcessDropdown(response.data);
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
$("#btnOrderNoSrNoSearch").click(function (event) {
    var $callFrom = $(this);
    event.preventDefault();
    chubbanData = [];
    var ajaxTime = new Date().getDate();
    for (var i = 1; i <= gridRowsCnt; i++) {
        chuban1 = $("#txtchuuban" + i).val();
        if ($("#txtchuuban" + i).val() != "") {
            chubbanData.push(chuban1);
        }

    }
    debugger;
    if (chubbanData.length > 0) {
        var i;

        var chubbanList = [];

        for (let i = 1; i <= gridRowsCnt; i++) {
            var val = $("#txtchuuban" + i).val().trim();

            // Only push if value is not empty
            if (val !== "") {
                chubbanList.push({ value: val, row: i });
            }
        }

        var duplicateChuubanListIndex = [];

        for (var i = 0; i < chubbanList.length; i++) {
            var currentVal = chubbanList[i].value;
            var currentRow = chubbanList[i].row;

            // Check if this value appears earlier in the array (duplicate)
            var isDuplicate = chubbanList.findIndex((item, index) =>
                item.value === currentVal && index < i
            ) !== -1;

            if (isDuplicate) {
                duplicateChuubanListIndex.push(currentRow);
            }
        }

        // Show message if any duplicates
        if (duplicateChuubanListIndex.length > 0) {
            message = $("#valMsgFollowingOrNoIsDuplicate").text().replace("NNN", duplicateChuubanListIndex.join(", "));
            showMessageDialog(message, "txtchuuban1", WARNING);
        }

        else {
            getDetailsByChuuban();
        }
    }
    else {
        message = $("#lblOrNoIsRequired").text().trim();
        showMessageDialog(message, "txtchuuban1", WARNING);
        return false;
    }

});
function getDetailsByChuuban() {
    messageWithChubanGyouban = "";
    var repeatedMsg1 = "";
    var repeatedMsg2 = "";
    var repeatedMsg3 = "";
    var repeatedMsg4 = "";
    var repeatedMsg5 = "";
    var repeatedMsg6 = "";
    var repeatedMsg7 = "";
    var repeatedMsg8 = "";
    var repeatedMsg9 = "";
    var repeatedMsg10 = "";
    var chubanNotFoundList = "";
    var NifudaNotFoundList = "";
    var processAlreadyStartedList = "";
    var processNotStartedList = "";
    var Unit_Type = "";
    debugger;
    var chuban = [];
    processPlanDetails = [];
    processPlanHostData = [];
    const chubanGyoubanNo = [];
    let isNifudaNotFoundFlag = false;

    for (var i = 1; i <= gridRowsCnt; i++) {
        chuban1 = $("#txtchuuban" + i).val();
        var cultureName = $("#hdfCultureName").val();
        if ($('#rdoPrcessPlanSearchBeforeEntry').is(':checked')) {
            displayProcessPlanData = "beforeProcessStart";
        }
        else {
            displayProcessPlanData = "afterProcessStart";
        }
        var jsonPlanData = ({
            CHUUBAN: chuban1,
            DISPLAYPROCESSPLANDATA: displayProcessPlanData,
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
        url: APIURL + "/ProcessPlanHandoverHost/ValidateChuuban",
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
                if (jsonresult.length > 0) {
                    for (let p = 0; p < jsonresult.length; p++) {
                        chubban = jsonresult[p].chuubaN_GYOUBAN.slice(0, -2);
                        gyouban = jsonresult[p].chuubaN_GYOUBAN.slice(-2);

                        debugger;

                        if (jsonresult[p].responce == "Or No Not Found") {
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblFollowingOrNoNotFound").text());
                            /*  val chuban = '35353535'*/

                            if (repeatedMsg1 != "") {
                                chubanNotFoundList = chubanNotFoundList + ",<br>" + chubban + '-' + gyouban;
                            }
                            else {
                                repeatedMsg1 = "Found";
                                chubanNotFoundList = chubban + '-' + gyouban;

                            }

                        }
                        else if (jsonresult[p].responce == "Process Is Not Started") {
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblFollowingOrNoProcessNotStarted").text());
                            if (repeatedMsg4 != "") {
                                processNotStartedList = processNotStartedList + ",<br>" + chubban + '-' + gyouban;
                            }
                            else {
                                repeatedMsg4 = "Found";
                                processNotStartedList = chubban + '-' + gyouban;

                            }
                        }
                        else if (jsonresult[p].responce == "Nifuda Not Found For This OrNo") {
                            focusFieldID = $(':focus').attr("id");
                            message = $.trim($("#lblNifudaNoNotFound").text());
                            if (repeatedMsg2 != "") {
                                NifudaNotFoundList = NifudaNotFoundList + ",<br>" + chubban + '-' + gyouban;
                            }
                            else {
                                repeatedMsg2 = "Found";
                                NifudaNotFoundList = chubban + '-' + gyouban;

                            }

                        }
                    }
                }
                
                 else {

                    GetProcessPlanDetails();
                }

                if (chubanNotFoundList != "") {
                    var message = $.trim($("#lblFollowingOrNoNotFound").text()).replace("9999999-99", "<br>" + chubanNotFoundList);
                    showMessageDialog(message, "", WARNING);
                    return false;
                }
                if (processNotStartedList != "") {
                    var message = $.trim($("#lblFollowingOrNoProcessNotStarted").text()).replace("9999999-99", "<br>" + processNotStartedList);
                    showMessageDialog(message, "", WARNING);
                    return false;
                }

                if (NifudaNotFoundList != "") {
                    var message = $.trim($("#lblNifudaNoNotFound").text()).replace("9999999-99", "<br>" + NifudaNotFoundList);
                    //showMessageDialog(message, "", WARNING);
                    isNifudaNotFoundFlag = true;

                    if (isNifudaNotFoundFlag == true) {
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
                                    debugger;
                                    bootbox.hideAll();
                                    GetProcessPlanDetails();
                                }
                                else {
                                    bootbox.hideAll();
                                }


                            },
                            centerVertical: true
                        });
                        debugger;
                    }
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


}
function GetProcessPlanDetails() {
    messageWithChubanGyouban = "";
    var repeatedMsg1 = "";
    var repeatedMsg2 = "";
    var repeatedMsg3 = "";
    var repeatedMsg4 = "";
    var repeatedMsg5 = "";
    var repeatedMsg6 = "";
    var repeatedMsg7 = "";
    var repeatedMsg8 = "";
    var repeatedMsg9 = "";
    var repeatedMsg10 = "";
    var chubanNotFoundList = "";
    var NifudaNotFoundList = "";
    var processAlreadyStartedList = "";
    var processNotStartedList = "";
    var Unit_Type = "";
    debugger;
    var chuban = [];
    processPlanDetails = [];
    processPlanHostData = [];
    const chubanGyoubanNo = [];
    let isNifudaNotFoundFlag = false;

    for (var i = 1; i <= gridRowsCnt; i++) {
        chuban1 = $("#txtchuuban" + i).val();
        var cultureName = $("#hdfCultureName").val();
        if ($('#rdoPrcessPlanSearchBeforeEntry').is(':checked')) {
            displayProcessPlanData = "beforeProcessStart";
        }
        else {
            displayProcessPlanData = "afterProcessStart";
        }
        var jsonPlanData = ({
            CHUUBAN: chuban1,
            DISPLAYPROCESSPLANDATA: displayProcessPlanData,
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
        url: APIURL + "/ProcessPlanHandoverHost/GetProcessPlanDetails",
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

                console.log(jsonresult);
                for (let p = 0; p < jsonresult.length; p++) {
                    for (let i = 1; i <= gridRowsCnt; i++) {
                        if (jsonresult[p].chuubaN_GYOUBAN == $("#txtchuuban" + parseInt(i)).val().toString()) {
                            if (jsonresult[p].hpO_INZU == null) {  //data bind from trn_process_plan on after radio btn selection
                                $("#txtCustomer" + parseInt(i)).text(jsonresult[p].customeR_NAME);

                                $("#txtProdName" + parseInt(i)).text(jsonresult[p].proD_NAME_CUST);
                                $("#txtIntName" + parseInt(i)).text(jsonresult[p].internaL_NAME_STRING);
                                $("#txtShape" + parseInt(i)).text(jsonresult[p].proD_SHAPE);
                                $("#txtDia" + parseInt(i)).text(jsonresult[p].proD_DIM_D);
                                $("#txtDiaW" + parseInt(i)).text(jsonresult[p].proD_DIM_W);
                                $("#txtOther" + parseInt(i)).text(jsonresult[p].proD_DIM_OTHER_STRING);
                                if (jsonresult[p].diM_TOL == ",") {
                                    $("#txtDimTol" + parseInt(i)).text("");
                                }
                                else {
                                    $("#txtDimTol" + parseInt(i)).text(jsonresult[p].diM_TOL);
                                }

                                if (jsonresult[p].proD_LENGTH != null) {
                                    $("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH);
                                }
                                else {
                                    $("#txtLen" + parseInt(i)).text(jsonresult[p].lengtH_OTHER);
                                }

                                debugger;
                                $("#txtCondition" + parseInt(i)).text(jsonresult[p].proD_CONDITION_STRING);
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
                                $("#txtRemark" + parseInt(i)).val(jsonresult[p].remark);
                                bindNextProcessDropdown(parseInt(i), processNameList);
                                $("#ddlNextProcess" + parseInt(i)).val(jsonresult[p].nexT_PROCESS_CD);
                                $("#ddlNextProcessCD" + parseInt(i)).text(jsonresult[p].nexT_PROCESS_CD);
                                $("#ddlNextProcess" + parseInt(i)).attr("DISABLED", "DISABLED");
                                $("#hdfChuubanGyouban" + parseInt(i)).text(jsonresult[p].chuubaN_GYOUBAN_STRING);
                                debugger;
                               
                                $("#btnAddNew").attr("DISABLED", "DISABLED");
                                $("#btnUpdate").attr("DISABLED", "DISABLED");
                                $("#btnCancel").removeAttr("DISABLED").removeClass("DISABLED");
                                $("#btnSetProcess").attr("DISABLED", "DISABLED");
                                jsonresult[p].griD_ROW_NO = i;
                                processPlanHostData.push(jsonresult[p]);
                                //$("#btnDeleteRow").removeAttr("DISABLED").removeClass("DISABLED");

                            }
                            else {
                                $("#txtCustomer" + parseInt(i)).text(jsonresult[p].customeR_NAME); //data bind from HTMP_PROD_ORDER on before radio btn selection
                                $("#txtProdName" + parseInt(i)).text(jsonresult[p].proD_NAME_CUST);
                                $("#txtIntName" + parseInt(i)).text(jsonresult[p].matL_NAME_INT);
                                $("#txtShape" + parseInt(i)).text(jsonresult[p].shape);
                                $("#txtDia" + parseInt(i)).text(jsonresult[p].proD_DIM_D);
                                $("#txtDiaW" + parseInt(i)).text(jsonresult[p].proD_DIM_W);
                                $("#txtOther" + parseInt(i)).text(jsonresult[p].proD_DIM_OTHER_STRING);
                                $("#txtDimTol" + parseInt(i)).text(jsonresult[p].diM_TOL);

                                if (jsonresult[p].proD_LENGTH != null) {
                                    $("#txtLen" + parseInt(i)).text(jsonresult[p].proD_LENGTH);
                                }
                                else {
                                    $("#txtLen" + parseInt(i)).text(jsonresult[p].lengtH_OTHER);
                                }
                                $("#txtCondition" + parseInt(i)).text(jsonresult[p].proD_CONDITION_STRING);
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
                                $("#hdfCustCode" + parseInt(i)).text(jsonresult[p].customeR_CODE);
                                $("#hdfInchargeCode" + parseInt(i)).text(jsonresult[p].inchargE_CODE);
                                $("#hdfMakerCode" + parseInt(i)).text(jsonresult[p].makeR_CODE);
                                $("#hdfMatlMakerName" + parseInt(i)).text(jsonresult[p].matL_NAME_MAK);
                                $("#hdfReceiverName" + parseInt(i)).text(jsonresult[p].receiveR_NAME);
                                $("#hdfDeptCode" + parseInt(i)).text(jsonresult[p].depT_CODE);
                                $("#hdfProdSpecNo1" + parseInt(i)).text(jsonresult[p].proD_SPEC_NO1);
                                $("#hdfProdSpecNo2" + parseInt(i)).text(jsonresult[p].proD_SPEC_NO2);
                                $("#hdfPackingName" + parseInt(i)).text(jsonresult[p].packinG_NAME);
                                $("#hdfProcessNameA" + parseInt(i)).text(jsonresult[p].procesS_NAME1);//processName1
                                $("#hdfProcessNameB" + parseInt(i)).text(jsonresult[p].procesS_NAME2);//processName2
                                $("#hdfProcessNameC" + parseInt(i)).text(jsonresult[p].procesS_NAME3);//processName3
                                $("#hdfProcessNameD" + parseInt(i)).text(jsonresult[p].procesS_NAME4);//processName4
                                $("#hdfProcessNameE" + parseInt(i)).text(jsonresult[p].procesS_NAME5);//processName5
                                $("#hdfProcessNameF" + parseInt(i)).text(jsonresult[p].procesS_NAME6);//processName6
                                $("#hdfProcessNameG" + parseInt(i)).text(jsonresult[p].procesS_NAME7);//processName7
                                $("#hdfProcessNameH" + parseInt(i)).text(jsonresult[p].procesS_NAME8);//processName8
                                $("#hdfProcessNameI" + parseInt(i)).text(jsonresult[p].procesS_NAME9);//processName9
                                $("#hdfProcessNameJ" + parseInt(i)).text(jsonresult[p].procesS_NAME10);//processName10
                                $("#hdfProcessNameK" + parseInt(i)).text(jsonresult[p].procesS_NAME11);//processName11
                                $("#hdfProcessNameL" + parseInt(i)).text(jsonresult[p].procesS_NAME12);//processName12
                                $("#hdfProcessNameM" + parseInt(i)).text(jsonresult[p].procesS_NAME13);//processName13
                                $("#hdfChuubanGyouban" + parseInt(i)).text(jsonresult[p].chuubaN_GYOUBAN_STRING);
                                $("#hdfMakerName" + parseInt(i)).text(jsonresult[p].makeR_NAME);
                                $("#hdfBendTol" + parseInt(i)).text(jsonresult[p].magarI_BEND_TOL_STRING);
                                $("#hdfMagariSpan" + parseInt(i)).text(jsonresult[p].magarI_SPAN_STRING); 

                                $("#hdfDimTolMaxSplit" + parseInt(i)).text(jsonresult[p].diM_TOL_MAX_SPLIT); 
                                $("#hdfDimTolMinSplit" + parseInt(i)).text(jsonresult[p].diM_TOL_MIN_SPLIT); 
                                $("#hdfDimTolMax" + parseInt(i)).text(jsonresult[p].diM_TOL_MAX); 
                                $("#hdfDimTolMin" + parseInt(i)).text(jsonresult[p].diM_TOL_MIN); 
                                $("#hdfDimTolMaxChar" + parseInt(i)).text(jsonresult[p].diM_TOL_MAX_CHR); 
                                $("#hdfDimTolMinChar" + parseInt(i)).text(jsonresult[p].diM_TOL_MIN_CHR); 

                                $("#hdfLenTolMaxSplit" + parseInt(i)).text(jsonresult[p].leN_TOL_MAX_SPLIT);
                                $("#hdfLenTolMinSplit" + parseInt(i)).text(jsonresult[p].leN_TOL_MIN_SPLIT);
                                $("#hdfLenTolMax" + parseInt(i)).text(jsonresult[p].hosT_LENGTH_TOL_MAX);
                                $("#hdfLenTolMin" + parseInt(i)).text(jsonresult[p].hosT_LENGTH_TOL_MIN);
                                $("#hdfLenTolMaxChar" + parseInt(i)).text(jsonresult[p].lengtH_TOL_MAX_CHR);
                                $("#hdfLenTolMinChar" + parseInt(i)).text(jsonresult[p].lengtH_TOL_MIN_CHR);
                                $("#hdfFuhenNo" + parseInt(i)).text(jsonresult[p].fuheN_NO);
                                $("#txtRemark" + parseInt(i)).val('');
                                $("#ddlNextProcess" + parseInt(i)).html("");
                                bindNextProcessDropdown(parseInt(i), processNameList);
                                debugger;
                                jsonresult[p].griD_ROW_NO = i;
                                processPlanHostData.push(jsonresult[p]);
                                console.log(processPlanHostData);

                                $("#btnCancel").attr("DISABLED", "DISABLED");
                                $("#btnAddNew").removeAttr("DISABLED").removeClass("DISABLED");
                                $("#btnUpdate").removeAttr("DISABLED").removeClass("DISABLED");
                                $("#btnSetProcess").removeAttr("DISABLED").removeClass("DISABLED");
                                //$("#btnDeleteRow").removeAttr("DISABLED").removeClass("DISABLED");
                                $("#ddlNextProcess" + parseInt(i)).removeAttr("DISABLED").removeClass("DISABLED");
                            }


                        }


                    }
                }
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblSystemException").text().trim();
            showMessageDialog(message, "", ERROR);

        });
}
$("#btnAddNew").click(function (event) {
    $("#btnAddNew").attr("DISABLED", "DISABLED");
    var Chuuban_No;
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessPlanHandoverHost/ValidateDataForAddNewBtn",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(processPlanHostData)
    }
    $.ajax(settings)
        .done(function (response) {
            response = response.data;
            responseTime = new Date().getTime() - ajaxTime;
            var notFoundchuubanData = response.filter(r => r.chuubaN_GYOUBAN != null && r.responce == "Data Already exist");
            if (notFoundchuubanData.length != 0) {
                Chuuban_No = notFoundchuubanData[0].chuubaN_GYOUBAN;
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgOrderNoAlreadyExist").text()).replace("9999999-99", Chuuban_No);
                showMessageDialog(message, focusFieldID, WARNING);
                $("#btnAddNew").removeAttr("DISABLED").removeClass("DISABLED");
                $("#btnUpdate").removeAttr("DISABLED").removeClass("DISABLED");
            }
            else {
                if (!CheckValidation()) {
                    return false;
                }
                else {

                    saveProcessPlanHandoverData();

                }

            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    //if (!CheckValidation()) {
    //    return false;
    //}
    //else {
    //    saveProcessPlanHandoverData();
    //}
});
function saveProcessPlanHandoverData() {
    debugger;
    var isddlProcessNotEmpty = true;
    jsonPlanData = [];
    processPlanDetails = [];
    var ajaxTime = new Date().getTime();
    for (var i = 1; i <= gridRowsCnt; i++) {
        if ($("#txtchuuban" + i).val() != "") {
            FIRST_PROCESS_CD = $("#ddlNextProcess" + i).val();
            if (FIRST_PROCESS_CD == null) {
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


    }
    
    if (isddlProcessNotEmpty == true) {
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
            for (var i = 1; i <= gridRowsCnt; i++) {
                var remark = $("#txtRemark" + i).val();
                if (processPlanHostData[p].chuubaN_GYOUBAN == $("#txtchuuban" + i).val()) {
                    processPlanHostData[p].remark = remark;
                }
            }
        }
        for (p = 0; p < processPlanHostData.length; p++) {
            var jsonHostData = ({
                CUSTOMER_CD: processPlanHostData[p].CUSTOMER_CD,
                RECEIVER_CD: processPlanHostData[p].RECEIVER_CD,
                INTERNAL_NAME_CD: processPlanHostData[p].INTERNAL_NAME_CD,
                DEPARTMENT_GROUP_CD: processPlanHostData[p].DEPARTMENT_GROUP_CD,
                USER_ID: USER_ID,
                PACKING_CD: processPlanHostData[p].PACKING_CD,
                CHUUBAN: processPlanHostData[p].chuuban,
                GYOUBAN: processPlanHostData[p].gyoubaN_HOST,
                PROD_SPEC_NO1: processPlanHostData[p].proD_SPEC_NO1,
                PROD_SPEC_NO2: processPlanHostData[p].proD_SPEC_NO2,
                VER: processPlanHostData[p].ver,
                STD_DEL_PERIOD: processPlanHostData[p].stD_DEL_PERIOD,
                PROD_NAME_CUST: processPlanHostData[p].proD_NAME_CUST,
                SHAPE: processPlanHostData[p].shape,
                PROD_DIM_D: processPlanHostData[p].proD_DIM_D,
                PROD_DIM_OTHER_STRING: processPlanHostData[p].proD_DIM_OTHER_STRING,
                DIM_TOL_MIN: processPlanHostData[p].DIM_TOL_MIN,
                DIM_TOL_MAX: processPlanHostData[p].DIM_TOL_MAX,
                DIM_TOL_MIN_CHR: processPlanHostData[p].DIM_TOL_MIN_CHR,
                DIM_TOL_MAX_CHR: processPlanHostData[p].DIM_TOL_MAX_CHR,
                PROD_LENGTH: processPlanHostData[p].proD_LENGTH,
                LENGTH_OTHER: processPlanHostData[p].lengtH_OTHER,
                LENGTH_TOL_MIN: processPlanHostData[p].HOST_LENGTH_TOL_MIN,
                LENGTH_TOL_MAX: processPlanHostData[p].HOST_LENGTH_TOL_MAX,
                LENGTH_TOL_MIN_CHR: processPlanHostData[p].LENGTH_TOL_MIN_CHR,
                LENGTH_TOL_MAX_CHR: processPlanHostData[p].LENGTH_TOL_MAX_CHR,
                IMPACT_TEST_PATTERN: processPlanHostData[p].IMPACT_TEST_PATTERN,
                IMPACT_TEST_MIN: processPlanHostData[p].IMPACT_TEST_MIN,
                IMPACT_TEST_MAX: processPlanHostData[p].IMPACT_TEST_MAX,
                IMPACT_TEST_INFO: processPlanHostData[p].IMPACT_TEST_INFO,
                COIL_DIA_INT: processPlanHostData[p].coiL_DIA_INT,
                COIL_DIA_EXT: processPlanHostData[p].coiL_DIA_EXT,
                COIL_WND_DIR: processPlanHostData[p].coiL_WND_DIR,
                MAGARI_KUBUN_STRING: processPlanHostData[p].magarI_KUBUN_STRING,
                MAGARI_BEND_TOL_STRING: processPlanHostData[p].MAGARI_BEND_TOL_STRING,
                MAGARI_SPAN_STRING: processPlanHostData[p].MAGARI_SPAN_STRING,
                UNIT_WT: processPlanHostData[p].uniT_WT,
                EXT_TEXTURE: processPlanHostData[p].exT_TEXTURE,
                COLOR_NAME: processPlanHostData[p].coloR_NAME,
                DISPLAY: processPlanHostData[p].display,
                STAMP: processPlanHostData[p].stamp,
                TEST_SPECIMEN: processPlanHostData[p].tesT_SPECIMEN,
                TENSILE_STRN_PATTERN1_STRING: processPlanHostData[p].tensilE_STRN_PATTERN1_STRING,
                ELONGATION_1: processPlanHostData[p].elongatioN_1,
                ELONGATION_2: processPlanHostData[p].elongatioN_2,
                HARDNESS_1: processPlanHostData[p].hardnesS_1,
                HARDNESS_2: processPlanHostData[p].hardnesS_2,
                HARDNESS_3: processPlanHostData[p].hardnesS_3,
                YEILD: processPlanHostData[p].yeild,
                PRESS_1: processPlanHostData[p].presS_1,
                PRESS_2: processPlanHostData[p].presS_2,
                DECARB_1: processPlanHostData[p].decarB_1,
                DECARB_2: processPlanHostData[p].decarB_2,
                DECARB_3: processPlanHostData[p].decarB_3,
                DECARB_4: processPlanHostData[p].decarB_4,
                PROCESS_1_CD: processPlanHostData[p].PROCESS_1_CD,
                PROCESS_2_CD: processPlanHostData[p].PROCESS_2_CD,
                PROCESS_3_CD: processPlanHostData[p].PROCESS_3_CD,
                PROCESS_4_CD: processPlanHostData[p].PROCESS_4_CD,
                PROCESS_5_CD: processPlanHostData[p].PROCESS_5_CD,
                PROCESS_6_CD: processPlanHostData[p].PROCESS_6_CD,
                PROCESS_7_CD: processPlanHostData[p].PROCESS_7_CD,
                PROCESS_8_CD: processPlanHostData[p].PROCESS_8_CD,
                PROCESS_9_CD: processPlanHostData[p].PROCESS_9_CD,
                PROCESS_10_CD: processPlanHostData[p].PROCESS_10_CD,
                PROCESS_11_CD: processPlanHostData[p].PROCESS_11_CD,
                PROCESS_12_CD: processPlanHostData[p].PROCESS_12_CD,
                PROCESS_13_CD: processPlanHostData[p].PROCESS_13_CD,
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
                MATL_SHAPE: processPlanHostData[p].matL_SHAPE,
                HOST_MATL_DIM_D: processPlanHostData[p].hosT_MATL_DIM_D,
                MATL_DIM_OTHER: processPlanHostData[p].matL_DIM_OTHER,
                KAKOUIARI_QTY: processPlanHostData[p].kakouiarI_QTY,
                HOST_DELIVERY_DATE: processPlanHostData[p].hosT_DELIVERY_DATE,
                HPO_INZU: processPlanHostData[p].hpO_INZU,
                HOST_UNIT_TYPE_STRING: processPlanHostData[p].hosT_UNIT_TYPE_STRING,
                ACT_ALLOC_QTY: processPlanHostData[p].acT_ALLOC_QTY,
                ACT_ALLOC_DATE: processPlanHostData[p].hosT_ACT_ALLOC_DATE,
                CHARGE: processPlanHostData[p].charge,
                PROCESS_PLAN_KUBUN: processPlanHostData[p].procesS_PLAN_KUBUN,
                PRINT_DATE: processPlanHostData[p].prinT_DATE,
                EMPLOYEE_CD: processPlanHostData[p].EMPLOYEE_CD,
                CHHUBAN_YEAR: processPlanHostData[p].chhubaN_YEAR,
                CREATED_DATE: processPlanHostData[p].createD_DATE,
                PROCESS_CD: processPlanHostData[p].nexT_PROCESS_CD,
                CULTURE_NAME: $("#hdfCultureName").val(),
                MAKER_CD: processPlanHostData[p].MAKER_CD,
                HOST_REFERENCE_NO: processPlanHostData[p].hosT_REFERENCE_NO,
                CHAMFER: processPlanHostData[p].chamfer,
                KAKOUIRAI_REMARK: processPlanHostData[p].kakouiraI_REMARK,
                REMARK: processPlanHostData[p].remark,
                PROD_CONDITION_STRING: processPlanHostData[p].proD_CONDITION_STRING
                
            });
            if (processPlanDetails.length != processPlanHostData.length) {
                processPlanDetails.push(jsonHostData);
            }

        }
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/ProcessPlanHandoverHost/getValueCode",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(processPlanDetails)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                processPlanData = response.message;


            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblSystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });
        //return false;


        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/ProcessPlanHandoverHost/SaveProcessPlanHandoverData",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(processPlanData)
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                //processNameList = response.message;
                if (response.message == "Data Inserted Successfully") {
                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblProcessStarted").text());
                    showMessageDialog(message, focusFieldID, SUCCESS);
                    $("#btnCancel").attr("DISABLED", "DISABLED");
                    $("#btnAddNew").attr("DISABLED", "DISABLED");
                    //$("#btnUpdate").attr("DISABLED", "DISABLED");
                    $("#btnUpdate").removeAttr("DISABLED").removeClass("DISABLED");
                    $("#btnSetProcess").attr("DISABLED", "DISABLED");
                    //$("#btnDeleteRow").attr("DISABLED", "DISABLED");
                    //btnInitializeClick();
                }
                else if (response.message == "Data Already exist") {

                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblSystemException").text());
                showMessageDialog(message, focusFieldID, ERROR);
            });


    }

}
$("#btnCancel").click(function (event) {
    DeleteProcessPlanHandoverData();

});
function DeleteProcessPlanHandoverData() {
    jsonPlanData = [];
    processPlanDetails = [];
    messageWithChubanGyouban = "";
    var repeatedMsg1 = "";
    chubanList = [];
    var otherProcessStartedList = "";
    var ajaxTime = new Date().getTime();
    for (var i = 1; i <= gridRowsCnt; i++) {
        var jsonPlanData = ({
            //CHUUBAN_GYOUBAN: $("#txtchuuban" + i).val(),
            CHUUBAN_GYOUBAN_STRING: $("#hdfChuubanGyouban" + i).text(),
            PROCESS_PLAN_CD: $("#txtProcessPlanCd" + i).text(),
            //NEXT_PROCESS_CD: $("#ddlNextProcessCD" + i).text()
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
        url: APIURL + "/ProcessPlanHandoverHost/DeleteProcessPlanHandoverData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(processPlanDetails)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonresult = response.data;

            for (let p = 0; p < jsonresult.length; p++) {
                var chubanGyouban = jsonresult[p].chuubaN_GYOUBAN_STRING;

                var isProcessStart = false;
                if (jsonresult.length == 1) {
                    if (jsonresult[p].responce == "Process Started") {
                        isProcessStart == true;
                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblFollowingOrNoProcessStarted").text());
                        messageWithChubanGyouban = message.replace("9999999-99", "<br>" + jsonresult[p].chuubaN_GYOUBAN_STRING);
                        showMessageDialog(messageWithChubanGyouban, "", WARNING);
                        $("#btnCancel").attr("DISABLED", "DISABLED");
                        $("#btnAddNew").attr("DISABLED", "DISABLED");
                        $("#btnUpdate").attr("DISABLED", "DISABLED");
                        $("#btnSetProcess").attr("DISABLED", "DISABLED");
                    }
                    else {
                        focusFieldID = $(':focus').attr("id");
                        messageWithChubanGyouban = $.trim($("#lblProcessCancelled").text());
                        showMessageDialog(messageWithChubanGyouban, "", SUCCESS);
                        $("#btnCancel").attr("DISABLED", "DISABLED");
                        $("#btnAddNew").attr("DISABLED", "DISABLED");
                        $("#btnUpdate").attr("DISABLED", "DISABLED");
                        $("#btnSetProcess").attr("DISABLED", "DISABLED");
                        //$("#btnDeleteRow").attr("DISABLED", "DISABLED");
                        //btnInitializeClick();
                    }
                }
                else {
                    if (jsonresult[p].responce == "Process Started") {
                        if (repeatedMsg1 != "") {


                            otherProcessStartedList = otherProcessStartedList + ",<br>" + jsonresult[p].chuubaN_GYOUBAN_STRING;


                        }

                        else {
                            repeatedMsg1 = "Found";
                            otherProcessStartedList = jsonresult[p].chuubaN_GYOUBAN_STRING;

                        }
                    }
                    else {
                        focusFieldID = $(':focus').attr("id");
                        messageWithChubanGyouban = $.trim($("#lblProcessCancelled").text());
                        chubanList.push(jsonresult[p].chuubaN_GYOUBAN_STRING);
                    }
                }

            }
            if (jsonresult.length != 1) {

                if (otherProcessStartedList != "") {
                    messageWithChubanGyouban = $.trim($("#lblFollowingOrNoProcessStarted").text()).replace("9999999-99", "<br>" + otherProcessStartedList);
                    showMessageDialog(messageWithChubanGyouban, "", WARNING);
                    $("#btnCancel").attr("DISABLED", "DISABLED");
                    $("#btnAddNew").attr("DISABLED", "DISABLED");
                    $("#btnUpdate").attr("DISABLED", "DISABLED");
                    $("#btnSetProcess").attr("DISABLED", "DISABLED");
                } else {
                    focusFieldID = $(':focus').attr("id");
                    messageWithChubanGyouban = $.trim($("#lblProcessCancelled").text());
                    showMessageDialog(messageWithChubanGyouban, "", SUCCESS);
                    $("#btnCancel").attr("DISABLED", "DISABLED");
                    $("#btnAddNew").attr("DISABLED", "DISABLED");
                    $("#btnUpdate").attr("DISABLED", "DISABLED");
                    $("#btnSetProcess").attr("DISABLED", "DISABLED");
                    //$("#btnDeleteRow").attr("DISABLED", "DISABLED");
                    //btnInitializeClick();
                }
            }
            chubanList = chubanList.filter(function (item) {
                return item !== "-";
            });
            if (chubanList.length > 0) {
                for (let i = 0; i < chubanList.length; i++) {
                    for (let p = 1; p <= gridRowsCnt; p++) {
                        var chubanNo = document.getElementById("txtchuuban" + parseInt(p)).value;
                        if (chubanList[i].replace('-', '') == chubanNo) {
                            $('#txtchuuban' + parseInt(p)).val('');
                            $("#txtCustomer" + parseInt(p)).text('');
                            $("#txtProdName" + parseInt(p)).text('');
                            $("#txtShape" + parseInt(p)).text('');
                            $("#txtDia" + parseInt(p)).text('');
                            $("#txtDiaW" + parseInt(p)).text('');
                            $("#txtOther" + parseInt(p)).text('');
                            $("#txtDimTol" + parseInt(p)).text('');
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
                            $("#hdfCustCode" + parseInt(p)).text('');
                            $("#hdfInchargeCode" + parseInt(p)).text('');
                            $("#hdfMakerCode" + parseInt(p)).text('');
                            $("#hdfMatlMakerName" + parseInt(p)).text('');
                            $("#hdfReceiverName" + parseInt(p)).text('');
                            $("#hdfProdSpecNo1" + parseInt(p)).text('');
                            $("#hdfProdSpecNo2" + parseInt(p)).text('');
                            $("#hdfDeptCode" + parseInt(p)).text('');
                            $("#hdfPackingName" + parseInt(p)).text('');
                            $("#hdfProcessNameA" + parseInt(p)).text('');//processName1
                            $("#hdfProcessNameB" + parseInt(p)).text('');//processName2
                            $("#hdfProcessNameC" + parseInt(p)).text('');//processName3
                            $("#hdfProcessNameD" + parseInt(p)).text('');//processName4
                            $("#hdfProcessNameE" + parseInt(p)).text('');//processName5
                            $("#hdfProcessNameF" + parseInt(p)).text('');//processName6
                            $("#hdfProcessNameG" + parseInt(p)).text('');//processName7
                            $("#hdfProcessNameH" + parseInt(p)).text('');//processName8
                            $("#hdfProcessNameI" + parseInt(p)).text('');//processName9
                            $("#hdfProcessNameJ" + parseInt(p)).text('');//processName10
                            $("#hdfProcessNameK" + parseInt(p)).text('');//processName11
                            $("#hdfProcessNameL" + parseInt(p)).text('');//processName12
                            $("#hdfProcessNameM" + parseInt(p)).text('');//processName13
                            $("#hdfChuubanGyouban" + parseInt(p)).text('');
                            $("#ddlNextProcess" + parseInt(p)).removeAttr("DISABLED", "DISABLED");
                            $("#txtIntName" + parseInt(p)).text('');
                            $("#hdfBendTol" + parseInt(p)).text('');
                            $("#hdfMagariSpan" + parseInt(p)).text('');
                            $("#hdfDimTolMinSplit" + parseInt(p)).text('');
                            $("#hdfDimTolMaxSplit" + parseInt(p)).text('');
                            $("#hdfDimTolMin" + parseInt(p)).text('');
                            $("#hdfDimTolMax" + parseInt(p)).text('');
                            $("#hdfDimTolMinChar" + parseInt(p)).text('');
                            $("#hdfDimTolMaxChar" + parseInt(p)).text('');
                            $("#hdfLenTolMinSplit" + parseInt(p)).text('');
                            $("#hdfLenTolMaxSplit" + parseInt(p)).text('');
                            $("#hdfLenTolMin" + parseInt(p)).text('');
                            $("#hdfLenTolMax" + parseInt(p)).text('');
                            $("#hdfLenTolMinChar" + parseInt(p)).text('');
                            $("#hdfLenTolMaxChar" + parseInt(p)).text('');
                            $("#hdfFuhenNo" + parseInt(p)).text('');
                            $('#txtRemark' + parseInt(p)).val('');
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

    $("#btnAddNew").attr("DISABLED", "DISABLED");
    $("#btnUpdate").attr("DISABLED", "DISABLED");
    $("#btnCancel").attr("DISABLED", "DISABLED");
    $("#btnSetProcess").attr("DISABLED", "DISABLED");
    // $("#btnDeleteRow").removeAttr("DISABLED").removeClass("DISABLED");
    $('#rdoPrcessPlanSearchBeforeEntry').prop("checked", "checked");
    setNextProcessList();

    for (let i = 1; i <= gridRowsCnt; i++) {
        $('#txtchuuban' + parseInt(i)).val('');
        $("#txtCustomer" + parseInt(i)).text('');
        $("#txtProdName" + parseInt(i)).text('');
        $("#txtShape" + parseInt(i)).text('');
        $("#txtDia" + parseInt(i)).text('');
        $("#txtDiaW" + parseInt(i)).text('');
        $("#txtOther" + parseInt(i)).text('');
        $("#txtDimTol" + parseInt(i)).text('');
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
        $("#ddlNextProcess" + parseInt(i)).removeAttr("DISABLED", "DISABLED");
        $("#txtIntName" + parseInt(i)).text('');
        $("#hdfCustCode" + parseInt(i)).text('');
        $("#hdfInchargeCode" + parseInt(i)).text('');
        $("#hdfMakerCode" + parseInt(i)).text('');
        $("#hdfMatlMakerName" + parseInt(i)).text('');
        $("#hdfReceiverName" + parseInt(i)).text('');
        $("#hdfProdSpecNo1" + parseInt(i)).text('');
        $("#hdfProdSpecNo2" + parseInt(i)).text('');
        $("#hdfDeptCode" + parseInt(i)).text('');
        $("#hdfPackingName" + parseInt(i)).text('');
        $("#hdfProcessNameA" + parseInt(i)).text('');//processName1
        $("#hdfProcessNameB" + parseInt(i)).text('');//processName2
        $("#hdfProcessNameC" + parseInt(i)).text('');//processName3
        $("#hdfProcessNameD" + parseInt(i)).text('');//processName4
        $("#hdfProcessNameE" + parseInt(i)).text('');//processName5
        $("#hdfProcessNameF" + parseInt(i)).text('');//processName6
        $("#hdfProcessNameG" + parseInt(i)).text('');//processName7
        $("#hdfProcessNameH" + parseInt(i)).text('');//processName8
        $("#hdfProcessNameI" + parseInt(i)).text('');//processName9
        $("#hdfProcessNameJ" + parseInt(i)).text('');//processName10
        $("#hdfProcessNameK" + parseInt(i)).text('');//processName11
        $("#hdfProcessNameL" + parseInt(i)).text('');//processName12
        $("#hdfProcessNameM" + parseInt(i)).text('');//processName13
        $("#hdfChuubanGyouban" + parseInt(i)).text('');
        $("#selectRow" + parseInt(i)).prop("checked", false);
        $("#hdfBendTol" + parseInt(i)).text('');
        $("#hdfMagariSpan" + parseInt(i)).text('');
        $("#hdfDimTolMinSplit" + parseInt(i)).text('');
        $("#hdfDimTolMaxSplit" + parseInt(i)).text('');
        $("#hdfDimTolMin" + parseInt(i)).text('');
        $("#hdfDimTolMax" + parseInt(i)).text('');
        $("#hdfDimTolMinChar" + parseInt(i)).text('');
        $("#hdfDimTolMaxChar" + parseInt(i)).text('');
        $("#hdfLenTolMinSplit" + parseInt(i)).text('');
        $("#hdfLenTolMaxSplit" + parseInt(i)).text('');
        $("#hdfLenTolMin" + parseInt(i)).text('');
        $("#hdfLenTolMax" + parseInt(i)).text('');
        $("#hdfLenTolMinChar" + parseInt(i)).text('');
        $("#hdfLenTolMaxChar" + parseInt(i)).text('');
        $("#hdfFuhenNo" + parseInt(i)).text('');
        $('#txtRemark' + parseInt(i)).val('');
    }
}
function bindNextProcessDropdown(rowid, processNameList) {

    if ($("#hdfCultureName").val() == "en") {

        // $("#ddlNextProcess" + rowid).html("").append($('<option></option>'));
        $.each(processNameList, function (key, jsonObj) {
            $("#ddlNextProcess" + rowid).append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_EN + '</option>'));
        });


    }
    else {
        //$("#ddlNextProcess" + rowid).html("").append($('<option></option>'));
        $.each(processNameList, function (key, jsonObj) {
            $("#ddlNextProcess" + rowid).append($('<option value="' + jsonObj.procesS_KUBUN_CD + '"> ' + jsonObj.kubuN_NAME_JP + '</option>'));
        });


    }

}

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('/');
}
$(document).keypress(function (e) {
    if ($(e.target).is('[data-orderno]')) {
        var orderNo = e.target.value ? e.target.value : null;
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == 13 && orderNo) {
            var ajaxTime = new Date().getTime();
            var settings = {
                async: false,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: APIURL + "/ProcessPlanHandoverHost/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
                method: "POST",
                contentType: "application/json; charset=utf-8",

            }
            $.ajax(settings)
                .done(function (response) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var jsonResult = response.message;
                    if (response.isSuccess) {
                        e.target.value = jsonResult.ordeR_NO_IN_CHUUBAN + jsonResult.gyoubaN_IN_CHUUBAN;
                        debugger;
                        let txtID = $(e.target).attr("data-id");
                        if (parseInt(txtID) < gridRowsCnt) {
                            $("#txtchuuban" + (parseInt(txtID) + 1)).focus();
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

function CheckValidation() {
    returnValue = false;
    var custNameNotFoundChuuban;
    var intNameProdSpecNotFoundChuuban;
    var matlmakerCode;
    var matlMakName;
    var matlNameInt;
    var matlNameMak;
    var processName;
    var intNameNotFoundChuuban;
    var deptGrNotFoundChuuban;
    var empNotFoundChuuban;
    var recNotFoundChuuban;
    var procNotFoundChuuban;
    var makerCdNotFoundChuuban;
    var bendTolAndMagariSpanNotFoundChuuban;
    var dimTolAndLenTolNotFoundChuuban;
    validateDataList = [];
    var validationMsg = "";
    for (var i = 1; i <= gridRowsCnt; i++) {
        chuubanGyoubanString = $("#hdfChuubanGyouban" + i).text();
        customerCode = $("#hdfCustCode" + i).text();
        inchargeCode = $("#hdfInchargeCode" + i).text();
        makerCode = $("#hdfMakerCode" + i).text();
        matlMakerName = $("#hdfMatlMakerName" + i).text();
        receiverName = $("#hdfReceiverName" + i).text();
        prodSpecNo1 = $("#hdfProdSpecNo1" + i).text();
        prodSpecNo2 = $("#hdfProdSpecNo2" + i).text();
        deptCode = $("#hdfDeptCode" + i).text();
        packingName = $("#hdfPackingName" + i).text();
        processName1 = $("#hdfProcessNameA" + i).text();//processName1
        processName2 = $("#hdfProcessNameB" + i).text();//processName2
        processName3 = $("#hdfProcessNameC" + i).text();//processName3
        processName4 = $("#hdfProcessNameD" + i).text();//processName4
        processName5 = $("#hdfProcessNameE" + i).text();//processName5
        processName6 = $("#hdfProcessNameF" + i).text();//processName6
        processName7 = $("#hdfProcessNameG" + i).text();//processName7
        processName8 = $("#hdfProcessNameH" + i).text();//processName8
        processName9 = $("#hdfProcessNameI" + i).text();//processName9
        processName10 = $("#hdfProcessNameJ" + i).text();//processName10
        processName11 = $("#hdfProcessNameK" + i).text();//processName11
        processName12 = $("#hdfProcessNameL" + i).text();//processName12
        processName13 = $("#hdfProcessNameM" + i).text();//processName13
        actAlloDate = $("#txtActAllDt" + i).text();
        matlInternalName = $("#txtIntName" + i).text();
        makerName = $("#hdfMakerName" + i).text();

        DimTolMinSplit = $("#hdfDimTolMinSplit" + i).text();
        DimTolMaxSplit = $("#hdfDimTolMaxSplit" + i).text();
        DimTolMin = $("#hdfDimTolMin" + i).text();
        DimTolMax = $("#hdfDimTolMax" + i).text();
        DimTolMinChar = $("#hdfDimTolMinChar" + i).text();
        DimTolMaxChar = $("#hdfDimTolMaxChar" + i).text(); 

        LenTolMinSplit = $("#hdfLenTolMinSplit" + i).text();
        LenTolMaxSplit = $("#hdfLenTolMaxSplit" + i).text();
        LenTolMin = $("#hdfLenTolMin" + i).text();
        LenTolMax = $("#hdfLenTolMax" + i).text();
        LenTolMinChar = $("#hdfLenTolMinChar" + i).text();
        LenTolMaxChar = $("#hdfLenTolMaxChar" + i).text();
        FuhenNo = $("#hdfFuhenNo" + i).text();


        bendTol = $("#hdfBendTol" + i).text() ? $("#hdfBendTol" + i).text() : null;
        magariSpan = $("#hdfMagariSpan" + i).text() ? $("#hdfMagariSpan" + i).text() : null;

        var jsonPlanData = ({
            CHUUBAN_GYOUBAN_STRING: chuubanGyoubanString,
            CUSTOMER_CODE: customerCode,
            INCHARGE_CODE: inchargeCode,
            MAKER_CODE: makerCode,
            MAKER_NAME: makerName,
            MATL_NAME_INT: matlInternalName,
            MATL_NAME_MAK: matlMakerName,
            RECEIVER_NAME: receiverName,
            PROD_SPEC_NO1: prodSpecNo1,
            PROD_SPEC_NO2: prodSpecNo2,
            DEPT_CODE: deptCode,
            PACKING_NAME: packingName,
            PROCESS_NAME1: processName1,
            PROCESS_NAME2: processName2,
            PROCESS_NAME3: processName3,
            PROCESS_NAME4: processName4,
            PROCESS_NAME5: processName5,
            PROCESS_NAME6: processName6,
            PROCESS_NAME7: processName7,
            PROCESS_NAME8: processName8,
            PROCESS_NAME9: processName9,
            PROCESS_NAME10: processName10,
            PROCESS_NAME11: processName11,
            PROCESS_NAME12: processName12,
            PROCESS_NAME13: processName13,
            ACT_ALLOC_DATE: actAlloDate,
            MAGARI_BEND_TOL_STRING: bendTol,
            MAGARI_SPAN_STRING: magariSpan,

            DIM_TOL_MIN_SPLIT: DimTolMinSplit,
            DIM_TOL_MAX_SPLIT: DimTolMaxSplit,
            DIM_TOL_MIN: DimTolMin,
            DIM_TOL_MAX: DimTolMax,
            DIM_TOL_MIN_CHR: DimTolMinChar,
            DIM_TOL_MAX_CHR: DimTolMaxChar,

            LEN_TOL_MIN_SPLIT: LenTolMinSplit,
            LEN_TOL_MAX_SPLIT: LenTolMaxSplit,
            HOST_LENGTH_TOL_MIN: LenTolMin,
            HOST_LENGTH_TOL_MAX: LenTolMax,
            LENGTH_TOL_MIN_CHR: LenTolMinChar,
            LENGTH_TOL_MAX_CHR: LenTolMaxChar,
            FUHEN_NO:FuhenNo,

        });

        if ($("#txtchuuban" + i).val() != "") {
            validateDataList.push(jsonPlanData);
        }
    }
    var ajaxTime = new Date().getDate();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessPlanHandoverHost/ValidateDataExist",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(validateDataList)
    }
    $.ajax(settings)
        .done(function (response) {
            response = response.data;
            responseTime = new Date().getTime() - ajaxTime;

            var customerNotFoundData = response.filter(r => r.customeR_CD == null);
            if (customerNotFoundData.length != 0) {
                custNameNotFoundChuuban = customerNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }

            var employeeNotFoundData = response.filter(r => r.employeE_CD == null);
            if (employeeNotFoundData.length != 0) {
                empNotFoundChuuban = employeeNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }

            //var internalProdSpecNotFoundData = response.filter(r => r.internaL_NAME_CD == 0);
            var internalNotFoundData = response.filter(r => r.internaL_NAME_CD == null);
            if (internalNotFoundData.length != 0) {
                intNameNotFoundChuuban = internalNotFoundData[0].chuubaN_GYOUBAN_STRING;
                matlmakerCode = internalNotFoundData[0].makeR_CODE;
                matlMakName = internalNotFoundData[0].makeR_NAME;
                matlNameInt = internalNotFoundData[0].matL_NAME_INT;
                matlNameMak = internalNotFoundData[0].matL_NAME_MAK;
            }
            //else if (internalNotFoundData.length!=0) {
            //    intNameNotFoundChuuban = internalNotFoundData[0].chuubaN_GYOUBAN_STRING;
            //}

            var departmentGrNotFoundData = response.filter(r => r.departmenT_GROUP_CD == null);
            if (departmentGrNotFoundData.length != 0) {
                deptGrNotFoundChuuban = departmentGrNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }

            var receiverNotFoundData = response.filter(r => r.receiveR_CD == null);
            if (receiverNotFoundData.length != 0) {
                recNotFoundChuuban = receiverNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }

            var processNotFoundData = response.filter(r => r.procesS_1_CD == null || r.procesS_2_CD == null || r.procesS_3_CD == null || r.procesS_4_CD == null || r.procesS_5_CD == null ||
                r.procesS_6_CD == null || r.procesS_7_CD == null || r.procesS_8_CD == null || r.procesS_9_CD == null || r.procesS_10_CD == null || r.procesS_11_CD == null ||
                r.procesS_12_CD == null || r.procesS_13_CD == null);
            if (processNotFoundData.length != 0) {
                procNotFoundChuuban = processNotFoundData[0].chuubaN_GYOUBAN_STRING;
                processName = processNotFoundData[0].procesS_NAME_STRING;
            }
            var makerCdNotFoundData = response.filter(r => r.makeR_CD == null);
            if (makerCdNotFoundData.length != 0) {
                makerCdNotFoundChuuban = makerCdNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }
            var bendTolAndMagariSpanNotFoundData = response.filter(r => r.magarI_BEND_TOL_STRING == "" && r.magarI_SPAN_STRING == "");
            if (bendTolAndMagariSpanNotFoundData.length != 0) {
                bendTolAndMagariSpanNotFoundChuuban = bendTolAndMagariSpanNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }
            var dimTolAndLenTolNotFoundData = response.filter(r => (r.diM_TOL_MAX == 0 && r.diM_TOL_MIN == 0 && r.diM_TOL_MAX_CHR == "" && r.diM_TOL_MIN_CHR == "") || (r.hosT_LENGTH_TOL_MAX == 0 && r.hosT_LENGTH_TOL_MIN == 0 && r.lengtH_TOL_MAX_CHR == "" && r.lengtH_TOL_MIN_CHR == ""));
            if (dimTolAndLenTolNotFoundData.length != 0) {
                dimTolAndLenTolNotFoundChuuban = dimTolAndLenTolNotFoundData[0].chuubaN_GYOUBAN_STRING;
            }

            if (customerNotFoundData.length == 0 && internalNotFoundData.length == 0 && departmentGrNotFoundData.length == 0 && receiverNotFoundData.length == 0 && employeeNotFoundData.length == 0 && processNotFoundData.length == 0 && makerCdNotFoundData.length == 0 && bendTolAndMagariSpanNotFoundData.length == 0 && dimTolAndLenTolNotFoundData.length==0) {

                for (i = 0; i < response.length; i++) {
                    processPlanHostData[i].CUSTOMER_CD = response[i].customeR_CD;
                    processPlanHostData[i].INTERNAL_NAME_CD = response[i].internaL_NAME_CD;
                    processPlanHostData[i].DEPARTMENT_GROUP_CD = response[i].departmenT_GROUP_CD;
                    if (response[i].receiveR_CD != 0) {
                        processPlanHostData[i].RECEIVER_CD = response[i].receiveR_CD;
                    }
                    else {
                        processPlanHostData[i].RECEIVER_CD = null; 
                    }
                    processPlanHostData[i].EMPLOYEE_CD = response[i].employeE_CD;
                    processPlanHostData[i].PACKING_CD = response[i].packinG_CD;
                    if (response[i].procesS_1_CD != 0) {
                        processPlanHostData[i].PROCESS_1_CD = response[i].procesS_1_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_1_CD = null;
                    }
                    if (response[i].procesS_2_CD != 0) {
                        processPlanHostData[i].PROCESS_2_CD = response[i].procesS_2_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_2_CD = null;
                    }
                    if (response[i].procesS_3_CD != 0) {
                        processPlanHostData[i].PROCESS_3_CD = response[i].procesS_3_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_3_CD = null;
                    }
                    if (response[i].procesS_4_CD != 0) {
                        processPlanHostData[i].PROCESS_4_CD = response[i].procesS_4_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_4_CD = null;
                    }
                    if (response[i].procesS_5_CD != 0) {
                        processPlanHostData[i].PROCESS_5_CD = response[i].procesS_5_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_5_CD = null;
                    }
                    if (response[i].procesS_6_CD != 0) {
                        processPlanHostData[i].PROCESS_6_CD = response[i].procesS_6_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_6_CD = null;
                    }
                    if (response[i].procesS_7_CD != 0) {
                        processPlanHostData[i].PROCESS_7_CD = response[i].procesS_7_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_7_CD = null;
                    }
                    if (response[i].procesS_8_CD != 0) {
                        processPlanHostData[i].PROCESS_8_CD = response[i].procesS_8_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_8_CD = null;
                    }
                    if (response[i].procesS_9_CD != 0) {
                        processPlanHostData[i].PROCESS_9_CD = response[i].procesS_9_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_9_CD = null;
                    }
                    if (response[i].procesS_10_CD != 0) {
                        processPlanHostData[i].PROCESS_10_CD = response[i].procesS_10_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_10_CD = null;
                    }
                    if (response[i].procesS_11_CD != 0) {
                        processPlanHostData[i].PROCESS_11_CD = response[i].procesS_11_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_11_CD = null;
                    }
                    if (response[i].procesS_12_CD != 0) {
                        processPlanHostData[i].PROCESS_12_CD = response[i].procesS_12_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_12_CD = null;
                    }
                    if (response[i].procesS_13_CD != 0) {
                        processPlanHostData[i].PROCESS_13_CD = response[i].procesS_13_CD;
                    }
                    else {
                        processPlanHostData[i].PROCESS_13_CD = null;
                    }
                    processPlanHostData[i].MAKER_CD = response[i].makeR_CD;
                    processPlanHostData[i].MAGARI_BEND_TOL_STRING = response[i].magarI_BEND_TOL_STRING;
                    processPlanHostData[i].MAGARI_SPAN_STRING = response[i].magarI_SPAN_STRING;

                    processPlanHostData[i].DIM_TOL_MAX = response[i].diM_TOL_MAX;
                    processPlanHostData[i].DIM_TOL_MIN = response[i].diM_TOL_MIN;
                    processPlanHostData[i].DIM_TOL_MAX_CHR = response[i].diM_TOL_MAX_CHR;
                    processPlanHostData[i].DIM_TOL_MIN_CHR = response[i].diM_TOL_MIN_CHR;

                    processPlanHostData[i].HOST_LENGTH_TOL_MAX = response[i].hosT_LENGTH_TOL_MAX;
                    processPlanHostData[i].HOST_LENGTH_TOL_MIN = response[i].hosT_LENGTH_TOL_MIN;
                    processPlanHostData[i].LENGTH_TOL_MAX_CHR = response[i].lengtH_TOL_MAX_CHR;
                    processPlanHostData[i].LENGTH_TOL_MIN_CHR = response[i].lengtH_TOL_MIN_CHR;
                    processPlanHostData[i].IMPACT_TEST_PATTERN = response[i].impacT_TEST_PATTERN;
                    processPlanHostData[i].IMPACT_TEST_MIN = response[i].impacT_TEST_MIN;
                    processPlanHostData[i].IMPACT_TEST_MAX = response[i].impacT_TEST_MAX;
                    processPlanHostData[i].IMPACT_TEST_INFO = response[i].impacT_TEST_INFO;
                }
            }
            var isMaterialFound = true;
            if (customerNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgCustomerNotFoundInMaster").text()).replace("99999999-99", "<br>" + custNameNotFoundChuuban);
            }
            else if (employeeNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgEmployeeNameNotFoundInMaster").text()).replace("99999999-99", "<br>" + empNotFoundChuuban);
            }
            else if (internalNotFoundData.length != 0) {
                isMaterialFound = false;
                validationMsg = $.trim($("#valMsgMaterialDetailsNotFount").text()).replace("99999999-99", intNameNotFoundChuuban).replace("MAKER_CODE", matlmakerCode).replace("MAKER_NAME", matlMakName).replace("INTERNAL_NAME", matlNameInt).replace("MATL_MAKER_NAME", matlNameMak);
            }
            //else if (internalNotFoundData.length != 0) {
            //    validationMsg = $.trim($("#valMsgIntNameNotFoundInMaster").text()).replace("99999999-99", "<br>" + intNameNotFoundChuuban);
            //}
            else if (departmentGrNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgEmployeeNameNotFoundInMaster").text()).replace("99999999-99", "<br>" + deptGrNotFoundChuuban);
            }
            else if (receiverNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgReceiverNotFoundInMaster").text()).replace("99999999-99", "<br>" + recNotFoundChuuban);
            }
            else if (processNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgProcessNameNotFoundInMaster").text()).replace("99999999-99", "<br>" + procNotFoundChuuban).replace("PROCESS_NAME", processName);
            }
            else if (makerCdNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgMakerNotFoundInMaster").text()).replace("99999999-99", "<br>" + makerCdNotFoundChuuban);
            }
            else if (bendTolAndMagariSpanNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgMissMatchBendAndSpanSpecification").text()).replace("99999999-99", "<br>" + bendTolAndMagariSpanNotFoundChuuban);
            }
            else if (dimTolAndLenTolNotFoundData.length != 0) {
                validationMsg = $.trim($("#valMsgMissMatchDimAndLenTolSpecification").text()).replace("99999999-99", "<br>" + dimTolAndLenTolNotFoundChuuban);
            } 
            if (validationMsg != "") {
                bootbox.alert({
                    title: "<div style='color:white'></div>",
                    message: "<table width='100%'><tr><td width='54px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + validationMsg + "</td></tr></table>",
                    centerVertical: true,
                    callback: function () {
                        if (isMaterialFound == false) {
                            bootbox.alert({
                                message: 'This is the large alert!',
                                size: 'large',//extra-large
                                message: "<table width='100%'><tr><td> <img src='/Images/MaterialScreen.png'/ height='100%' width='100%'></td></tr></table>",
                                centerVertical: true

                            });
                        }



                    }
                });
                returnValue = false;
                $("#btnCancel").attr("DISABLED", "DISABLED");
                //$("#btnUpdate").attr("DISABLED", "DISABLED");
                $("#btnUpdate").removeAttr("DISABLED").removeClass("DISABLED");
                $("#btnAddNew").removeAttr("DISABLED").removeClass("DISABLED");
                $("#btnSetProcess").removeAttr("DISABLED").removeClass("DISABLED");
            }
            else {
                returnValue = true;
            }
            

        });
   return returnValue;
}

$("#btnUpdate").click(function (event) {
    $("#btnUpdate").attr("DISABLED", "DISABLED");
    var Chuuban_No;
    var ajaxTime = new Date().getTime();

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessPlanHandoverHost/ValidateDataForUpdate",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(processPlanHostData)
    }
    $.ajax(settings)
        .done(function (response) {
            response = response.data;
            responseTime = new Date().getTime() - ajaxTime;
            var notFoundchuubanData = response.filter(r => r.chuubaN_GYOUBAN != null && r.responce == "Can not update existing data");
            if (notFoundchuubanData.length != 0) {
                Chuuban_No = notFoundchuubanData[0].chuubaN_GYOUBAN;
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#valMsgOrderNoNotExist").text()).replace("9999999-99", Chuuban_No);
                showMessageDialog(message, focusFieldID, WARNING);
                $("#btnAddNew").removeAttr("DISABLED").removeClass("DISABLED");
                $("#btnUpdate").removeAttr("DISABLED").removeClass("DISABLED");
            }
            else {
                if (!CheckValidation()) {
                    return false;
                }
                else {
                    UpdateProcessPlanHandoverData();
                }
            }



        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });



});
function UpdateProcessPlanHandoverData() {
    debugger;
    var isddlProcessNotEmpty = true;
    jsonPlanData = [];
    processPlanDetails = [];
    var ajaxTime = new Date().getTime();

    debugger;
    var USER_ID = $("#hdfLoginId").val();
    $("#ddlNextProcess").prop("selectedIndex", 0);
    //for (p = 0; p < processPlanHostData.length; p++) {

    for (var i = 1; i <= gridRowsCnt; i++) {
        $("#ddlNextProcess" + i).val('');
        //var Process_CD = $("#ddlNextProcess" + i).val();//code commented on 02/12/2025 as per client requirement
        //if (processPlanHostData[p].chuubaN_GYOUBAN == $("#txtchuuban" + i).val()) { 
        //    processPlanHostData[p].nexT_PROCESS_CD = Process_CD;
        //}
    }
    //}
    for (p = 0; p < processPlanHostData.length; p++) {
        for (var i = 1; i <= gridRowsCnt; i++) {
            var remark = $("#txtRemark" + i).val();
            if (processPlanHostData[p].chuubaN_GYOUBAN == $("#txtchuuban" + i).val()) {
                processPlanHostData[p].remark = remark;
            }
        }
    }
    for (p = 0; p < processPlanHostData.length; p++) {
        var jsonHostData = ({
            CUSTOMER_CD: processPlanHostData[p].CUSTOMER_CD,
            RECEIVER_CD: processPlanHostData[p].RECEIVER_CD,
            INTERNAL_NAME_CD: processPlanHostData[p].INTERNAL_NAME_CD,
            DEPARTMENT_GROUP_CD: processPlanHostData[p].DEPARTMENT_GROUP_CD,
            USER_ID: USER_ID,
            PACKING_CD: processPlanHostData[p].PACKING_CD,
            CHUUBAN: processPlanHostData[p].chuuban,
            GYOUBAN: processPlanHostData[p].gyoubaN_HOST,
            PROD_SPEC_NO1: processPlanHostData[p].proD_SPEC_NO1,
            PROD_SPEC_NO2: processPlanHostData[p].proD_SPEC_NO2,
            VER: processPlanHostData[p].ver,
            STD_DEL_PERIOD: processPlanHostData[p].stD_DEL_PERIOD,
            PROD_NAME_CUST: processPlanHostData[p].proD_NAME_CUST,
            SHAPE: processPlanHostData[p].shape,
            PROD_DIM_D: processPlanHostData[p].proD_DIM_D,
            PROD_DIM_OTHER_STRING: processPlanHostData[p].proD_DIM_OTHER_STRING,
            DIM_TOL_MIN: processPlanHostData[p].DIM_TOL_MIN,
            DIM_TOL_MAX: processPlanHostData[p].DIM_TOL_MAX,
            DIM_TOL_MIN_CHR: processPlanHostData[p].DIM_TOL_MIN_CHR,
            DIM_TOL_MAX_CHR: processPlanHostData[p].DIM_TOL_MAX_CHR,
            PROD_LENGTH: processPlanHostData[p].proD_LENGTH,
            LENGTH_OTHER: processPlanHostData[p].lengtH_OTHER,
            LENGTH_TOL_MIN: processPlanHostData[p].HOST_LENGTH_TOL_MIN,
            LENGTH_TOL_MAX: processPlanHostData[p].HOST_LENGTH_TOL_MAX,
            LENGTH_TOL_MIN_CHR: processPlanHostData[p].LENGTH_TOL_MIN_CHR,
            LENGTH_TOL_MAX_CHR: processPlanHostData[p].LENGTH_TOL_MAX_CHR,
            IMPACT_TEST_PATTERN: processPlanHostData[p].IMPACT_TEST_PATTERN,
            IMPACT_TEST_MIN: processPlanHostData[p].IMPACT_TEST_MIN,
            IMPACT_TEST_MAX: processPlanHostData[p].IMPACT_TEST_MAX,
            IMPACT_TEST_INFO: processPlanHostData[p].IMPACT_TEST_INFO,
            COIL_DIA_INT: processPlanHostData[p].coiL_DIA_INT,
            COIL_DIA_EXT: processPlanHostData[p].coiL_DIA_EXT,
            COIL_WND_DIR: processPlanHostData[p].coiL_WND_DIR,
            MAGARI_KUBUN_STRING: processPlanHostData[p].magarI_KUBUN_STRING,
            MAGARI_BEND_TOL_STRING: processPlanHostData[p].MAGARI_BEND_TOL_STRING,
            MAGARI_SPAN_STRING: processPlanHostData[p].MAGARI_SPAN_STRING,
            UNIT_WT: processPlanHostData[p].uniT_WT,
            EXT_TEXTURE: processPlanHostData[p].exT_TEXTURE,
            COLOR_NAME: processPlanHostData[p].coloR_NAME,
            DISPLAY: processPlanHostData[p].display,
            STAMP: processPlanHostData[p].stamp,
            TEST_SPECIMEN: processPlanHostData[p].tesT_SPECIMEN,
            TENSILE_STRN_PATTERN1_STRING: processPlanHostData[p].tensilE_STRN_PATTERN1_STRING,
            ELONGATION_1: processPlanHostData[p].elongatioN_1,
            ELONGATION_2: processPlanHostData[p].elongatioN_2,
            HARDNESS_1: processPlanHostData[p].hardnesS_1,
            HARDNESS_2: processPlanHostData[p].hardnesS_2,
            HARDNESS_3: processPlanHostData[p].hardnesS_3,
            YEILD: processPlanHostData[p].yeild,
            PRESS_1: processPlanHostData[p].presS_1,
            PRESS_2: processPlanHostData[p].presS_2,
            DECARB_1: processPlanHostData[p].decarB_1,
            DECARB_2: processPlanHostData[p].decarB_2,
            DECARB_3: processPlanHostData[p].decarB_3,
            DECARB_4: processPlanHostData[p].decarB_4,
            PROCESS_1_CD: processPlanHostData[p].PROCESS_1_CD,
            PROCESS_2_CD: processPlanHostData[p].PROCESS_2_CD,
            PROCESS_3_CD: processPlanHostData[p].PROCESS_3_CD,
            PROCESS_4_CD: processPlanHostData[p].PROCESS_4_CD,
            PROCESS_5_CD: processPlanHostData[p].PROCESS_5_CD,
            PROCESS_6_CD: processPlanHostData[p].PROCESS_6_CD,
            PROCESS_7_CD: processPlanHostData[p].PROCESS_7_CD,
            PROCESS_8_CD: processPlanHostData[p].PROCESS_8_CD,
            PROCESS_9_CD: processPlanHostData[p].PROCESS_9_CD,
            PROCESS_10_CD: processPlanHostData[p].PROCESS_10_CD,
            PROCESS_11_CD: processPlanHostData[p].PROCESS_11_CD,
            PROCESS_12_CD: processPlanHostData[p].PROCESS_12_CD,
            PROCESS_13_CD: processPlanHostData[p].PROCESS_13_CD,
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
            MATL_SHAPE: processPlanHostData[p].matL_SHAPE,
            HOST_MATL_DIM_D: processPlanHostData[p].hosT_MATL_DIM_D,
            MATL_DIM_OTHER: processPlanHostData[p].matL_DIM_OTHER,
            KAKOUIARI_QTY: processPlanHostData[p].kakouiarI_QTY,
            HOST_DELIVERY_DATE: processPlanHostData[p].hosT_DELIVERY_DATE,
            HPO_INZU: processPlanHostData[p].hpO_INZU,
            HOST_UNIT_TYPE_STRING: processPlanHostData[p].hosT_UNIT_TYPE_STRING,
            ACT_ALLOC_QTY: processPlanHostData[p].acT_ALLOC_QTY,
            ACT_ALLOC_DATE: processPlanHostData[p].hosT_ACT_ALLOC_DATE,
            CHARGE: processPlanHostData[p].charge,
            PROCESS_PLAN_KUBUN: processPlanHostData[p].procesS_PLAN_KUBUN,
            PRINT_DATE: processPlanHostData[p].prinT_DATE,
            EMPLOYEE_CD: processPlanHostData[p].EMPLOYEE_CD,
            CHHUBAN_YEAR: processPlanHostData[p].chhubaN_YEAR,
            CREATED_DATE: processPlanHostData[p].createD_DATE,
            //PROCESS_CD: processPlanHostData[p].nexT_PROCESS_CD,//code commented on 02/12/2025 as per client requirement
            CULTURE_NAME: $("#hdfCultureName").val(),
            MAKER_CD: processPlanHostData[p].MAKER_CD,
            HOST_REFERENCE_NO: processPlanHostData[p].hosT_REFERENCE_NO, 
            CHAMFER: processPlanHostData[p].chamfer,
            KAKOUIRAI_REMARK: processPlanHostData[p].kakouiraI_REMARK,
            REMARK: processPlanHostData[p].remark,
            PROD_CONDITION_STRING: processPlanHostData[p].proD_CONDITION_STRING
        });
        if (processPlanDetails.length != processPlanHostData.length) {
            processPlanDetails.push(jsonHostData);
        }

    }
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/ProcessPlanHandoverHost/getValueCode",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(processPlanDetails)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            processPlanData = response.message;


        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    //return false;


    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ProcessPlanHandoverHost/UpdateProcessPlanHandoverData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(processPlanData)
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            if (response.message == "Data Updated Successfully") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#successMsg_ProcessPlanUpdated").text());
                showMessageDialog(message, focusFieldID, SUCCESS);
                $("#btnCancel").attr("DISABLED", "DISABLED");
                //$("#btnAddNew").attr("DISABLED", "DISABLED");
                //$("#btnUpdate").attr("DISABLED", "DISABLED");
                $("#btnAddNew").removeAttr("DISABLED").removeClass("DISABLED");
                $("#btnUpdate").removeAttr("DISABLED").removeClass("DISABLED");
                $("#btnSetProcess").attr("DISABLED", "DISABLED");
                //$("#btnDeleteRow").attr("DISABLED", "DISABLED");
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblSystemException").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });


    //}

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
//        var value = document.getElementById("txtchuuban" + index).value;
//        if (value.length == 11 || value.length == 9) {
//            setFocus(index);
//        }
//    }
//}
//function setFocus(index) {
//    document.getElementById("txtchuuban" + (index + 1)).focus();
//}

$("#btnSetProcess").click(function () {
    debugger;
    if ($("#txtchuuban1").val() != "" && $("#ddlNextProcess1").val() != "") {
        var selectedProcessVal = $("#ddlNextProcess1").val();

    }
    for (let i = 1; i <= gridRowsCnt; i++) {
        var chubanNo = document.getElementById("txtchuuban" + parseInt(i)).value;
        if (chubanNo > 0) {
            $("#ddlNextProcess" + parseInt(i)).val(selectedProcessVal);
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
    $('#txtchuuban' + index).val('');
    $("#txtCustomer" + index).text('');
    $("#txtProdName" + index).text('');
    $("#txtShape" + index).text('');
    $("#txtDia" + index).text('');
    $("#txtDiaW" + index).text('');
    $("#txtOther" + index).text('');
    $("#txtDimTol" + index).text('');
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
    $("#ddlNextProcess" + index).text('');
    $("#txtIntName" + index).text('');
    $("#hdfCustCode" + index).text('');
    $("#hdfInchargeCode" + index).text('');
    $("#hdfMakerCode" + index).text('');
    $("#hdfMatlMakerName" + index).text('');
    $("#hdfReceiverName" + index).text('');
    $("#hdfProdSpecNo1" + index).text('');
    $("#hdfProdSpecNo2" + index).text('');
    $("#hdfDeptCode" + index).text('');
    $("#hdfPackingName" + index).text('');
    $("#hdfProcessNameA" + index).text('');//processName1
    $("#hdfProcessNameB" + index).text('');//processName2
    $("#hdfProcessNameC" + index).text('');//processName3
    $("#hdfProcessNameD" + index).text('');//processName4
    $("#hdfProcessNameE" + index).text('');//processName5
    $("#hdfProcessNameF" + index).text('');//processName6
    $("#hdfProcessNameG" + index).text('');//processName7
    $("#hdfProcessNameH" + index).text('');//processName8
    $("#hdfProcessNameI" + index).text('');//processName9
    $("#hdfProcessNameJ" + index).text('');//processName10
    $("#hdfProcessNameK" + index).text('');//processName11
    $("#hdfProcessNameL" + index).text('');//processName12
    $("#hdfProcessNameM" + index).text('');//processName13
    $("#hdfChuubanGyouban" + index).text('');
    $('#txtRemark' + + index).val('');
    $("#selectRow" + index).prop("checked", false);

    // processPlanHostData.splice(index - 1, 1); 
    if (processPlanHostData.length > 0) {
        processPlanHostData = processPlanHostData.filter(function (item) {
            return item.griD_ROW_NO != index;
        });
    }
    console.log(processPlanHostData);
}
