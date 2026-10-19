var userName = '';
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";

$(document).ready(function () {
    setOperatorList($("#txtInputSearch").val(), $('#ddlNameValue'), "");
    setNextProcess();
    var hdfHARDENING_TEMPERING_CD = parseInt($("#hdfHARDENING_TEMPERING_CD").val());
    var furNo = $("#hdfFurNo").val();

    $("#R1Data #txtHardeningFuranceInDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#R1Data #txtHardeningFuranceOutDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#R1Data #txtTemperingFuranceInDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    $("#R1Data #txtTemperingFuranceOutDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    if (hdfHARDENING_TEMPERING_CD > 0 && furNo) {
        var ajaxTime = new Date().getTime();
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/HardnessTempering/GetHardnessTemperingDetailsByHardnessTemperingCD?hardnessTemperingCD=" + hdfHARDENING_TEMPERING_CD,
            method: "GET",
            contentType: "application/json; charset=utf-8",
            //data: "{jsonData : " + jsonData + "}"
        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                if (response.isSuccess) {
                    var section = "R1Data";
                    console.log(response.message);
                    var jsonResult = response.message;
                    $("#" + section + " #PROCESS_PLAN_CD").val(jsonResult.procesS_PLAN_CD);
                    $("#" + section + " #txtcustomer").val(jsonResult.pP_CUSTOMER_NAME);
                    $("#" + section + " #txtProdName").val(jsonResult.pP_PROD_NAME_CUST);
                    $("#" + section + " #txtCharge").val(jsonResult.pP_CHARGE);
                    //$("#" + section + " #txtMetallicLabelNo").val(jsonResult.pP_MATL_NAME_MAK);
                    $("#" + section + " #txtMaterialDim").val(jsonResult.pP_MATL_DIM_D);
                    $("#" + section + " #txtProductDim").val(jsonResult.pP_PROD_DIM_D);
                    $("#" + section + " #HARDENING_TEMPERING_CD").val(jsonResult.hardeninG_TEMPERING_CD);
                    $("#" + section + " #txtFuranceNumber").val(jsonResult.furnacE_NO);

                    //$("#" + section + " #").val(jsonResult.hardeninG_MACHINE_CD);
                    //$("#" + section + " #").val(jsonResult.TEMPERING_MACHINE_CD);
                    $("#" + section + " #txtMetallicLabelNo").val(jsonResult.metalliC_PLATE_CODE);
                    //$("#" + section + " #").val(jsonResult.NO_OF_UNITS);
                    //$("#" + section + " #").val(jsonResult.WEIGHT);
                    //$("#" + section + " #").val(jsonResult.WATER_OIL_TYPE);
                    $("#" + section + " #txtTemprature").val(jsonResult.temperaturE_FURNACE);
                    $("#" + section + " #txtConcentration").val(jsonResult.concentratioN_FURNACE);
                    $("#" + section + " #txtfrequency").val(jsonResult.frequencY_FURNACE);
                    //$("#" + section + " #").val(jsonResult.C_FURNACE);
                    //$("#" + section + " #").val(jsonResult.MN_FURNACE);
                    //$("#" + section + " #").val(jsonResult.MO_FURNACE);
                    //$("#" + section + " #").val(jsonResult.CR_FURNACE);
                    $("#" + section + " #txtHardnessMin").val(jsonResult.hardnesS_MIN);
                    $("#" + section + " #txtHardnessMax").val(jsonResult.hardnesS_MAX);

                    $("#" + section + " #ddlNameValue1").val(jsonResult.operatoR_CD_HARDENING);
                    $("#" + section + " #txtHardeningFuranceInDate").val(jsonResult.starT_DATE_HARDENING);
                    $("#" + section + " #txtHardeningFuranceOutDate").val(jsonResult.enD_DATE_HARDENING);
                    $("#" + section + " #txtTemperatureHardening").val(jsonResult.temperaturE_HARDENING);
                    $("#" + section + " #txtTimeHardening").val(jsonResult.timE_HARDENING);
                    $("#" + section + " #txt_h_NorthSideUp1").val(jsonResult.q_HARDNESS_NORTH_UP_1);
                    $("#" + section + " #txt_h_NorthSideUp2").val(jsonResult.q_HARDNESS_NORTH_UP_2);
                    $("#" + section + " #txt_h_NorthSideUp3").val(jsonResult.q_HARDNESS_NORTH_UP_3);
                    $("#" + section + " #txt_h_NorthSideUpCent").val(jsonResult.q_HARDNESS_NORTH_UP_CEN);
                    $("#" + section + " #txt_h_NorthSideDown1").val(jsonResult.q_HARDNESS_NORTH_DOWN_1);
                    $("#" + section + " #txt_h_NorthSideDown2").val(jsonResult.q_HARDNESS_NORTH_DOWN_2);
                    $("#" + section + " #txt_h_NorthSideDown3").val(jsonResult.q_HARDNESS_NORTH_DOWN_3);
                    $("#" + section + " #txt_h_NorthSideDownCent").val(jsonResult.q_HARDNESS_NORTH_DOWN_CEN);
                    $("#" + section + " #txt_h_SouthSideUp1").val(jsonResult.q_HARDNESS_SOUTH_UP_1);
                    $("#" + section + " #txt_h_SouthSideUp2").val(jsonResult.q_HARDNESS_SOUTH_UP_2);
                    $("#" + section + " #txt_h_SouthSideUp3").val(jsonResult.q_HARDNESS_SOUTH_UP_3);
                    $("#" + section + " #txt_h_SouthSideUpCent").val(jsonResult.q_HARDNESS_SOUTH_UP_CEN);
                    $("#" + section + " #txt_h_SouthSideDown1").val(jsonResult.q_HARDNESS_SOUTH_DOWN_1);
                    $("#" + section + " #txt_h_SouthSideDown2").val(jsonResult.q_HARDNESS_SOUTH_DOWN_2);
                    $("#" + section + " #txt_h_SouthSideDown3").val(jsonResult.q_HARDNESS_SOUTH_DOWN_3);

                    $("#" + section + " #txt_h_SouthSideDownCent").val(jsonResult.q_HARDNESS_SOUTH_DOWN_CEN);
                    $("#" + section + " #ddlNameValue2").val(jsonResult.operatoR_CD_TEMPERING);
                    $("#" + section + " #txtTemperingFuranceInDate").val(jsonResult.starT_DATE_TEMPERING);
                    $("#" + section + " #txtTemperingFuranceOutDate").val(jsonResult.enD_DATE_TEMPERING);
                    $("#" + section + " #txtTemperatureTempering").val(jsonResult.temperaturE_TEMPERING);
                    $("#" + section + " #txtTimeTempering").val(jsonResult.timE_TEMPERING);
                    $("#" + section + " #txt_t_NorthSideUp1").val(jsonResult.t_HARDNESS_NORTH_UP_1);
                    $("#" + section + " #txt_t_NorthSideUp2").val(jsonResult.t_HARDNESS_NORTH_UP_2);
                    $("#" + section + " #txt_t_NorthSideUp3").val(jsonResult.t_HARDNESS_NORTH_UP_3);
                    $("#" + section + " #txt_t_NorthSideUpCent").val(jsonResult.t_HARDNESS_NORTH_UP_CENT);
                    $("#" + section + " #txt_t_NorthSideDown1").val(jsonResult.t_HARDNESS_NORTH_DOWN_1);
                    $("#" + section + " #txt_t_NorthSideDown2").val(jsonResult.t_HARDNESS_NORTH_DOWN_2);
                    $("#" + section + " #txt_t_NorthSideDown3").val(jsonResult.t_HARDNESS_NORTH_DOWN_3);
                    $("#" + section + " #txt_t_NorthSideDownCent").val(jsonResult.t_HARDNESS_NORTH_DOWN_CEN);
                    $("#" + section + " #txt_t_SouthSideUp1").val(jsonResult.t_HARDNESS_SOUTH_UP_1);
                    $("#" + section + " #txt_t_SouthSideUp2").val(jsonResult.t_HARDNESS_SOUTH_UP_2);
                    $("#" + section + " #txt_t_SouthSideUp3").val(jsonResult.t_HARDNESS_SOUTH_UP_3);
                    $("#" + section + " #txt_t_SouthSideUpCent").val(jsonResult.t_HARDNESS_SOUTH_UP_CENT);
                    $("#" + section + " #txt_t_SouthSideDown1").val(jsonResult.t_HARDNESS_SOUTH_DOWN_1);
                    $("#" + section + " #txt_t_SouthSideDown2").val(jsonResult.t_HARDNESS_SOUTH_DOWN_2);
                    $("#" + section + " #txt_t_SouthSideDown3").val(jsonResult.t_HARDNESS_SOUTH_DOWN_3);
                    $("#" + section + " #txt_t_SouthSideDownCent").val(jsonResult.t_HARDNESS_SOUTH_DOWN_CEN);
                    $("#" + section + " #ddlNextProcess1").val(jsonResult.hT_NEXT_PROCESS);
                    //$("#" + section + " #").val(jsonResult.TIME_HARDENING);
                }
                else {
                    showMessageDialog(response.message, '', WARNING);
                }
            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
                showMessageDialog(message, '', ERROR);
            });
    }
})

function setOperatorList(yobina, $target, defaultSelectID) {
    var ajaxTime = new Date().getTime();
    var jsonData = "{yobina:'" + yobina + "', USER_NAME: '" + userName + "' , employeeCD: '" + defaultSelectID + "' }";
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/EmployeeList?name=" + yobina + "&addEmpty=false",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            // var jsonResult = $.parseJSON(response.d);
            bindOperatorDropdown(response.message, $target, defaultSelectID);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HIKI_EDIT_DELETE_013").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindOperatorDropdown(jsonResult, $target, defaultSelectID) {
    $target.html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $target.append($('<option value="' + jsonObj.cd + '" yobina= "' + (jsonObj.yobina).toLowerCase() + '">' + jsonObj.name + '</option>'));
    });
    if (defaultSelectID != null && defaultSelectID != "") {
        $("#" + $target.attr("id") + " option:selected").prop("selected", false);
        $("#" + $target.attr("id") + " option[value=" + defaultSelectID + "]").prop("selected", "selected");
    }
}

function setNextProcess() {
    var ajaxTime = new Date().getTime();
    var jsonData = "{USER_NAME: '" + userName + "' }";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetNextProcess",
        method: "POST",
        contentType: "application/json; charset=utf-8"
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindNextProcessDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindNextProcessDropdown(jsonResult) {
    $("#ddlNextProcess").html("");

    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlNextProcess").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

$(document).on('click', '#btnYobinaStartOpeartorSearch', function () {
    setOperatorList($("#txtInputSearch").val(), $('#ddlNameValue'), "");
})

$(document).on('click', '#btn_extract', function () {
    var message = "";
    var chuuban = $('#txtChuuban').val();
    var gyoban = $('#txtgyubaan').val();
    if (!chuuban) {
        message = message + "<br>" + $('#txtChuuban').attr("ErrorLabel");
    }
    if (!gyoban) {
        message = message + "<br>" + $('#txtgyubaan').attr("ErrorLabel");
    }
    if (message !== "") {
        $("#dialogMessageIcon img").hide();

        bootbox.alert({
            title: "<div style='color:white'></div>",
            message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + message + "</td></tr></table>",
            centerVertical: true,
            callback: function () {
            }
        });

    }
    else {
        GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyoban, "R1Data");
    }
    
})

function GetHardnessTemperingDetailsByChuubanGyoban(chuuban, gyouban, section) {
    var ajaxTime = new Date().getDate();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardnessTemperingDetailsByChuubanGyoban",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ chuuban: chuuban, gyouban: String(gyouban) })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            console.log(jsonResult);
            $("#" + section + " #PROCESS_PLAN_CD").val(jsonResult.pP_PROCESS_PLAN_CD);
            $("#" + section + " #txtcustomer").val(jsonResult.pP_CUSTOMER_NAME);
            $("#" + section + " #txtProdName").val(jsonResult.pP_PROD_NAME_CUST);
            $("#" + section + " #txtCharge").val(jsonResult.pP_CHARGE);
            $("#" + section + " #txtMetallicLabelNo").val(jsonResult.pP_MATL_NAME_MAK);
            $("#" + section + " #txtMaterialDim").val(jsonResult.pP_MATL_DIM_D);
            $("#" + section + " #txtProductDim").val(jsonResult.pP_PROD_DIM_D);


            //$("#" + section + " ").val(jsonResult.pP_CUSTOMER_CD);
            //$("#" + section + " ").val(jsonResult.pP_EMPLOYEE_CD);
            //$("#" + section + " ").val(jsonResult.pP_CHUUBAN);
            //$("#" + section + " ").val(jsonResult.pP_GYOUBAN);
            //$("#" + section + " ").val(jsonResult.pP_CHUUBAN_GYOUBAN);
            //$("#" + section + " ").val(jsonResult.pP_PRODUCT_HISTORY_CD);
            //$("#" + section + " ").val(jsonResult.pP_PROD_SPEC_NO);
            //$("#" + section + " ").val(jsonResult.pP_KAKOUIRAI_CD);
            //$("#" + section + " ").val(jsonResult.pP_CHUUBAN_YEAR);
            //$("#" + section + " ").val(jsonResult.pP_ACT_ALLOC_CD);
            //$("#" + section + " ").val(jsonResult.pP_ACT_ALLOC_TYPE);
            //$("#" + section + " ").val(jsonResult.pP_DELIVERY_DATE);
            //$("#" + section + " ").val(jsonResult.pP_DELIVERY_DATE_STRING);
            //$("#" + section + " ").val(jsonResult.pP_ACT_ALLOC_QTY);
            //$("#" + section + " ").val(jsonResult.pP_QUANTITY);
            //$("#" + section + " ").val(jsonResult.pP_ORIGINAL_QTY);
            //$("#" + section + " ").val(jsonResult.pP_NO_OF_UNITS);
            //$("#" + section + " ").val(jsonResult.pP_NO_OF_UNITS_DECODE);
            //$("#" + section + " ").val(jsonResult.pP_UNIT);
            //$("#" + section + " ").val(jsonResult.UNIT_TYPE);
            //$("#" + section + " ").val(jsonResult.pP_MATL_NAME_MAK_CD);
            //$("#" + section + " ").val(jsonResult.pP_MATL_NAME_INT_CD);
            //$("#" + section + " ").val(jsonResult.pP_EMPLYOEE_CD);
            //$("#" + section + " ").val(jsonResult.pP_PROD_NAME_CUST_CD);
            //$("#" + section + " ").val(jsonResult.pP_RECEIVER_CD);
            //$("#" + section + " ").val(jsonResult.pP_MATL_SHAPE);

            //$("#" + section + " ").val(jsonResult.pP_MATL_DIM_D_SHAPE);
            //$("#" + section + " ").val(jsonResult.pP_MATL_DIM_OTHER);

            //$("#" + section + " ").val(jsonResult.pP_PROD_SHAPE);

            //$("#" + section + " ").val(jsonResult.pP_PROD_DIM_OTHER);
            //$("#" + section + " ").val(jsonResult.pP_DIM_TOL_MAX);
            //$("#" + section + " ").val(jsonResult.pP_DIM_TOL_MIN);
            //$("#" + section + " ").val(jsonResult.pP_DIM_TOLERANCE);
            //$("#" + section + " ").val(jsonResult.pP_PROD_LENGTH);
            //$("#" + section + " ").val(jsonResult.pP_PROD_LENGTH_STRING);
            //$("#" + section + " ").val(jsonResult.pP_LENGTH_TOL_MAX);
            //$("#" + section + " ").val(jsonResult.pP_LENGTH_TOL_MIN);
            //$("#" + section + " ").val(jsonResult.pP_LEN_TOL);
            //$("#" + section + " ").val(jsonResult.pP_CHAMFER);
            //$("#" + section + " ").val(jsonResult.pP_UNIT_WT);
            //$("#" + section + " ").val(jsonResult.pP_COLOR_ID);
            //$("#" + section + " ").val(jsonResult.pP_COLOR);
            //$("#" + section + " ").val(jsonResult.pP_MAGARI_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_BEND_TOL);
            //$("#" + section + " ").val(jsonResult.pP_MAGARI_SPAN);
            //$("#" + section + " ").val(jsonResult.pP_EXT_TEXTURE);
            //$("#" + section + " ").val(jsonResult.pP_DISPLAY);
            //$("#" + section + " ").val(jsonResult.pP_PACKING);
            //$("#" + section + " ").val(jsonResult.pP_PACKING_DECODE);
            //$("#" + section + " ").val(jsonResult.pP_TEST_SPECIMEN);
            //$("#" + section + " ").val(jsonResult.pP_TENSILE_STRENGTH);
            //$("#" + section + " ").val(jsonResult.pP_TENSILE_STRN_MIN);
            //$("#" + section + " ").val(jsonResult.pP_TENSILE_STRN_MAX);
            //$("#" + section + " ").val(jsonResult.pP_YEILD);
            //$("#" + section + " ").val(jsonResult.pP_ELONGATION1);
            //$("#" + section + " ").val(jsonResult.pP_HARDNESS1);
            //$("#" + section + " ").val(jsonResult.pP_DECARB);
            //$("#" + section + " ").val(jsonResult.pP_PROD_MNGMNT_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_SPCL_MNGMNT_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_STORAGE_MNGMNT_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_STD_DEL_PERIOD);
            //$("#" + section + " ").val(jsonResult.pP_STORAGE_LOCATION);
            //$("#" + section + " ").val(jsonResult.pP_MATL_NAME_MAK);
            //$("#" + section + " ").val(jsonResult.pP_MATL_NAME_INT);
            //$("#" + section + " ").val(jsonResult.pP_EMPLOYEE_NAME);

            //$("#" + section + " ").val(jsonResult.pP_RECEIVER_NAME);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_PLAN_REMARK);
            //$("#" + section + " ").val(jsonResult.pP_FINISHED_QTY);
            //$("#" + section + " ").val(jsonResult.pP_REFERENCE_CHUBBAN);
            //$("#" + section + " ").val(jsonResult.pP_SETSUZAN_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_LOCATION_NAME);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_1);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_2);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_3);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_4);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_5);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_6);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_7);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_8);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_9);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_10);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_11);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_12);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_13);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_1);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_2);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_3);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_4);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_5);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_6);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_7);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_8);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_9);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_10);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_11);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_12);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_13);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_PLAN_PLNG_DATE);
            //$("#" + section + " ").val(jsonResult.PROCESS_PLAN_CMPL_DATE);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_PLAN_PRINT_FLAG);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_PLAN_INACTIVE);
            //$("#" + section + " ").val(jsonResult.pP_USER_ID);
            //$("#" + section + " ").val(jsonResult.pP_CREATED_DATE);
            //$("#" + section + " ").val(jsonResult.pP_UPDATED_DATE);
            //$("#" + section + " ").val(jsonResult.pP_CUSTOMER_NAME);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_PLAN_CMPL_DATE);

            //$("#" + section + " ").val(jsonResult.pP_PROCESS_PLAN_CMPL_DATE);
            //$("#" + section + " ").val(jsonResult.pP_ELONGATION2);
            //$("#" + section + " ").val(jsonResult.pP_PRESS1);
            //$("#" + section + " ").val(jsonResult.pP_PRESS2);
            //$("#" + section + " ").val(jsonResult.pP_HARDNESS_2);
            //$("#" + section + " ").val(jsonResult.pP_HARDNESS_3);
            //$("#" + section + " ").val(jsonResult.pP_HARDNESS);
            //$("#" + section + " ").val(jsonResult.pP_DECARB_STD_1);
            //$("#" + section + " ").val(jsonResult.pP_DECARB_STD_2);
            //$("#" + section + " ").val(jsonResult.pP_DECARB_STD_3);
            //$("#" + section + " ").val(jsonResult.pP_DECARB_VAL_1);
            //$("#" + section + " ").val(jsonResult.pP_DECARB_VAL_2);
            //$("#" + section + " ").val(jsonResult.pP_DECARB_VAL_3);
            //$("#" + section + " ").val(jsonResult.pP_COIL_DIA_INTERNAL);
            //$("#" + section + " ").val(jsonResult.pP_COIL_DIA_EXTERNAL);
            //$("#" + section + " ").val(jsonResult.pP_COIL_DIA_WINDING);
            //$("#" + section + " ").val(jsonResult.COIL_DIA_WINDING);
            //$("#" + section + " ").val(jsonResult.pP_PRESS);
            //$("#" + section + " ").val(jsonResult.pP_ELONGATION);
            //$("#" + section + " ").val(jsonResult.pP_INZU);
            //$("#" + section + " ").val(jsonResult.pP_INZU_UNIT);
            //$("#" + section + " ").val(jsonResult.pP_NO_OF_COILS);
            //$("#" + section + " ").val(jsonResult.pP_STAMP);
            //$("#" + section + " ").val(jsonResult.pP_PRODUCT_CONDITION);
            //$("#" + section + " ").val(jsonResult.pP_ACT_ALLOC_DATE);
            //$("#" + section + " ").val(jsonResult.pP_PROD_NAME_INT);
            //$("#" + section + " ").val(jsonResult.pP_KAKOUIRAI_REMARKS);
            //$("#" + section + " ").val(jsonResult.pP_PRODUCTION_PROCESS);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_SPECIFICATION_REMARK);
            //$("#" + section + " ").val(jsonResult.pP_FACTORY_PRODUCTION_FLAG);
            //$("#" + section + " ").val(jsonResult.pP_DIM_W);
            //$("#" + section + " ").val(jsonResult.pP_HT_CONDITION);
            //$("#" + section + " ").val(jsonResult.pP_CORNER_RADIUS);
            //$("#" + section + " ").val(jsonResult.pP_MBEU);
            //$("#" + section + " ").val(jsonResult.pP_DIM_D_X_DIM_W);
            //$("#" + section + " ").val(jsonResult.pP_MAGARI_BEND_TOL);
            //$("#" + section + " ").val(jsonResult.pP_MATL_LENGTH);
            //$("#" + section + " ").val(jsonResult.pP_GENERATION_DATE);
            //$("#" + section + " ").val(jsonResult.pP_PROD_SPEC_NO1);
            //$("#" + section + " ").val(jsonResult.pP_PROD_SPEC_NO2);
            //$("#" + section + " ").val(jsonResult.pP_UNIT_WT_KUBUN);
            //$("#" + section + " ").val(jsonResult.pP_IMPACT_TEST_MIN);
            //$("#" + section + " ").val(jsonResult.pP_IMPACT_TEST_MAX);
            //$("#" + section + " ").val(jsonResult.pP_IMPACT_TEST);
            //$("#" + section + " ").val(jsonResult.pP_DELETE_FLAG);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_1);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_2);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_3);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_4);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_5);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_6);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_7);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_8);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_9);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_10);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_11);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_12);
            //$("#" + section + " ").val(jsonResult.pP_PROD_POINT_SUB_13);
            //$("#" + section + " ").val(jsonResult.pP_MATL_TYPE_CBK);
            //$("#" + section + " ").val(jsonResult.pP_MAKER_NAME_SHORT);
            //$("#" + section + " ").val(jsonResult.pP_MATL_CONDITION);
            //$("#" + section + " ").val(jsonResult.pP_MATL_DIM_W);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_1);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_2);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_3);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_4);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_5);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_6);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_7);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_8);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_9);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_10);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_11);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_12);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_ANOTHER_NAME_13);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_1);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_2);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_3);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_4);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_5);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_6);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_7);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_8);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_9);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_10);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_11);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_12);
            //$("#" + section + " ").val(jsonResult.pP_PROCESS_COLOR_KUBUN_13);
            //$("#" + section + " ").val(jsonResult.pP_CUSTOMER_FULL_NAME);
            //$("#" + section + " ").val(jsonResult.pP_RECEIVER_FULL_NAME);
            //$("#" + section + " ").val(jsonResult.pP_PROD_VER);
            //$("#" + section + " ").val(jsonResult.pP_JIS_TEST_SPEC);
            //$("#" + section + " ").val(jsonResult.TEST_SPEC_REMARK);
            //$("#" + section + " ").val(jsonResult.pP_PROD_DIM_D_SHAPE_STRING);
            //$("#" + section + " ").val(jsonResult.pP_PROD_LENGTH_OTHER);
            //$("#" + section + " ").val(jsonResult.MAGARI_KUBUN);
            //$("#" + section + " ").val(jsonResult.MAGARI_BEND_TOL);
            //$("#" + section + " ").val(jsonResult.MAGARI_SPAN);
            //$("#" + section + " ").val(jsonResult.TEST_SPECIMEN);
            //$("#" + section + " ").val(jsonResult.TENSILE_STRN_PATTERN1);
            //$("#" + section + " ").val(jsonResult.TENSILE_STRN_PATTERN1_MIN_MAX);
            //$("#" + section + " ").val(jsonResult.TENSILE_STRN_PATTERN2_MIN);
            //$("#" + section + " ").val(jsonResult.TENSILE_STRN_PATTERN2_MAX);
            //$("#" + section + " ").val(jsonResult.TENSILE_STRN_PATTERN3_ACTUAL);
            //$("#" + section + " ").val(jsonResult.ELONGATION_PATTERN1);
            //$("#" + section + " ").val(jsonResult.ELONGATION_PATTERN1_MIN_MAX);
            //$("#" + section + " ").val(jsonResult.ELONGATION_PATTERN2_MIN);
            //$("#" + section + " ").val(jsonResult.ELONGATION_PATTERN2_MAX);
            //$("#" + section + " ").val(jsonResult.ELONGATION_PATTERN3_ACTUAL);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN1);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN1_MIN_MAX);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN2_MIN);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN2_MAX);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN3_ACTUAL);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN1_CODE);
            //$("#" + section + " ").val(jsonResult.HARDNESS_PATTERN2_CODE);
            //$("#" + section + " ").val(jsonResult.YEILD_PATTERN1);
            //$("#" + section + " ").val(jsonResult.YEILD_PATTERN1_MIN_MAX);
            //$("#" + section + " ").val(jsonResult.YEILD_PATTERN2_MIN);
            //$("#" + section + " ").val(jsonResult.YEILD_PATTERN2_MAX);
            //$("#" + section + " ").val(jsonResult.YEILD_PATTERN3_ACTUAL);
            //$("#" + section + " ").val(jsonResult.PRESS_PATTERN1);
            //$("#" + section + " ").val(jsonResult.PRESS_PATTERN1_MIN_MAX);
            //$("#" + section + " ").val(jsonResult.PRESS_PATTERN2_MIN);
            //$("#" + section + " ").val(jsonResult.PRESS_PATTERN2_MAX);
            //$("#" + section + " ").val(jsonResult.PRESS_PATTERN3_ACTUAL);
            //$("#" + section + " ").val(jsonResult.IMPACT_TEST_PATTERN1);
            //$("#" + section + " ").val(jsonResult.IMPACT_TEST_PATTERN1_MIN_MAX);
            //$("#" + section + " ").val(jsonResult.IMPACT_TEST_PATTERN2_MIN);
            //$("#" + section + " ").val(jsonResult.IMPACT_TEST_PATTERN2_MAX);
            //$("#" + section + " ").val(jsonResult.IMPACT_TEST_PATTERN3_ACTUAL);
            //$("#" + section + " ").val(jsonResult.DECARB);
            //$("#" + section + " ").val(jsonResult.DECARB_1);
            //$("#" + section + " ").val(jsonResult.DECARB_2);
            //$("#" + section + " ").val(jsonResult.DECARB_3);
            //$("#" + section + " ").val(jsonResult.DECARB_VAL_1);
            //$("#" + section + " ").val(jsonResult.DECARB_VAL_2);
            //$("#" + section + " ").val(jsonResult.DECARB_VAL_3);
            //$("#" + section + " ").val(jsonResult.INTERNAL_NAME);
            $("#" + section + " #HARDENING_TEMPERING_CD").val(jsonResult.hardeninG_TEMPERING_CD);
            //$("#" + section + " ").val(jsonResult.LOCATION_CD);
            //$("#" + section + " ").val(jsonResult.MSG_CD);
            //$("#" + section + " ").val(jsonResult.PROCESS_PLAN_CD);
            $("#" + section + " #txtFuranceNumber").val(jsonResult.furnacE_NO);
            //$("#" + section + " ").val(jsonResult.HARDENING_MACHINE_CD);
            //$("#" + section + " ").val(jsonResult.TEMPERING_MACHINE_CD);
            //$("#" + section + " ").val(jsonResult.METALLIC_PLATE_CODE);
            //$("#" + section + " ").val(jsonResult.NO_OF_UNITS);
            //$("#" + section + " ").val(jsonResult.WEIGHT);
            //$("#" + section + " ").val(jsonResult.WATER_OIL_TYPE);
            $("#" + section + " #txtTemprature").val(jsonResult.temperaturE_FURNACE);
            $("#" + section + " #txtConcentration").val(jsonResult.concentratioN_FURNACE);
            $("#" + section + " #txtfrequency").val(jsonResult.frequencY_FURNACE);
            //$("#" + section + " #").val(jsonResult.C_FURNACE);
            //$("#" + section + " #").val(jsonResult.MN_FURNACE);
            //$("#" + section + " #").val(jsonResult.MO_FURNACE);
            //$("#" + section + " #").val(jsonResult.CR_FURNACE);
            $("#" + section + " #txtHardnessMin").val(jsonResult.hardnesS_MIN);
            $("#" + section + " #txtHardnessMax").val(jsonResult.hardnesS_MAX);

            $("#" + section + " #ddlNameValue1").val(jsonResult.operatoR_CD_HARDENING);
            $("#" + section + " #txtHardeningFuranceInDate").val(jsonResult.starT_DATE_HARDENING);
            $("#" + section + " #txtHardeningFuranceOutDate").val(jsonResult.enD_DATE_HARDENING);
            $("#" + section + " #txtTemperatureHardening").val(jsonResult.temperaturE_HARDENING);
            $("#" + section + " #txtTimeHardening").val(jsonResult.timE_HARDENING);
            $("#" + section + " #txt_h_NorthSideUp1").val(jsonResult.q_HARDNESS_NORTH_UP_1);
            $("#" + section + " #txt_h_NorthSideUp2").val(jsonResult.q_HARDNESS_NORTH_UP_2);
            $("#" + section + " #txt_h_NorthSideUp3").val(jsonResult.q_HARDNESS_NORTH_UP_3);
            $("#" + section + " #txt_h_NorthSideUpCent").val(jsonResult.q_HARDNESS_NORTH_UP_CEN);
            $("#" + section + " #txt_h_NorthSideDown1").val(jsonResult.q_HARDNESS_NORTH_DOWN_1);
            $("#" + section + " #txt_h_NorthSideDown2").val(jsonResult.q_HARDNESS_NORTH_DOWN_2);
            $("#" + section + " #txt_h_NorthSideDown3").val(jsonResult.q_HARDNESS_NORTH_DOWN_3);
            $("#" + section + " #txt_h_NorthSideDownCent").val(jsonResult.q_HARDNESS_NORTH_DOWN_CEN);
            $("#" + section + " #txt_h_SouthSideUp1").val(jsonResult.q_HARDNESS_SOUTH_UP_1);
            $("#" + section + " #txt_h_SouthSideUp2").val(jsonResult.q_HARDNESS_SOUTH_UP_2);
            $("#" + section + " #txt_h_SouthSideUp3").val(jsonResult.q_HARDNESS_SOUTH_UP_3);
            $("#" + section + " #txt_h_SouthSideUpCent").val(jsonResult.q_HARDNESS_SOUTH_UP_CEN);
            $("#" + section + " #txt_h_SouthSideDown1").val(jsonResult.q_HARDNESS_SOUTH_DOWN_1);
            $("#" + section + " #txt_h_SouthSideDown2").val(jsonResult.q_HARDNESS_SOUTH_DOWN_2);
            $("#" + section + " #txt_h_SouthSideDown3").val(jsonResult.q_HARDNESS_SOUTH_DOWN_3);

            $("#" + section + " #txt_h_SouthSideDownCent").val(jsonResult.q_HARDNESS_SOUTH_DOWN_CEN);
            $("#" + section + " #ddlNameValue2").val(jsonResult.operatoR_CD_TEMPERING);
            $("#" + section + " #txtTemperingFuranceInDate").val(jsonResult.starT_DATE_TEMPERING);
            $("#" + section + " #txtTemperingFuranceOutDate").val(jsonResult.enD_DATE_TEMPERING);
            $("#" + section + " #txtTemperatureTempering").val(jsonResult.temperaturE_TEMPERING);
            $("#" + section + " #txtTimeTempering").val(jsonResult.timE_TEMPERING);
            $("#" + section + " #txt_t_NorthSideUp1").val(jsonResult.t_HARDNESS_NORTH_UP_1);
            $("#" + section + " #txt_t_NorthSideUp2").val(jsonResult.t_HARDNESS_NORTH_UP_2);
            $("#" + section + " #txt_t_NorthSideUp3").val(jsonResult.t_HARDNESS_NORTH_UP_3);
            $("#" + section + " #txt_t_NorthSideUpCent").val(jsonResult.t_HARDNESS_NORTH_UP_CENT);
            $("#" + section + " #txt_t_NorthSideDown1").val(jsonResult.t_HARDNESS_NORTH_DOWN_1);
            $("#" + section + " #txt_t_NorthSideDown2").val(jsonResult.t_HARDNESS_NORTH_DOWN_2);
            $("#" + section + " #txt_t_NorthSideDown3").val(jsonResult.t_HARDNESS_NORTH_DOWN_3);
            $("#" + section + " #txt_t_NorthSideDownCent").val(jsonResult.t_HARDNESS_NORTH_DOWN_CEN);
            $("#" + section + " #txt_t_SouthSideUp1").val(jsonResult.t_HARDNESS_SOUTH_UP_1);
            $("#" + section + " #txt_t_SouthSideUp2").val(jsonResult.t_HARDNESS_SOUTH_UP_2);
            $("#" + section + " #txt_t_SouthSideUp3").val(jsonResult.t_HARDNESS_SOUTH_UP_3);
            $("#" + section + " #txt_t_SouthSideUpCent").val(jsonResult.t_HARDNESS_SOUTH_UP_CENT);
            $("#" + section + " #txt_t_SouthSideDown1").val(jsonResult.t_HARDNESS_SOUTH_DOWN_1);
            $("#" + section + " #txt_t_SouthSideDown2").val(jsonResult.t_HARDNESS_SOUTH_DOWN_2);
            $("#" + section + " #txt_t_SouthSideDown3").val(jsonResult.t_HARDNESS_SOUTH_DOWN_3);
            $("#" + section + " #txt_t_SouthSideDownCent").val(jsonResult.t_HARDNESS_SOUTH_DOWN_CEN);
            $("#" + section + " #ddlNextProcess1").val(jsonResult.hT_NEXT_PROCESS);
            console.log(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;
        });

}

$(document).on('click', '#btnDelete', function () {
    var hdfHARDENING_TEMPERING_CD = parseInt($("#hdfHARDENING_TEMPERING_CD").val());
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/DeleteHardnessTemperingDetails?hardeningCd=" + hdfHARDENING_TEMPERING_CD,
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            showMessageDialog(response.message, '', SUCCESS);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STR_EDIT_DELETE_012").text());
            showMessageDialog(message, '', ERROR);
        });
})

$(document).on('click', '#btnEdit', function () {
    var jsonDataR1 = JSON.stringify({
        HARDENING_TEMPERING_CD: $("#hdfHARDENING_TEMPERING_CD").val() ? $("#hdfHARDENING_TEMPERING_CD").val() : null,
        //MSG_CD: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        PROCESS_PLAN_CD: $("#R1Data #PROCESS_PLAN_CD").val() ? $("#R1Data #PROCESS_PLAN_CD").val() : null,
        FURNACE_NO: $("#R1Data #txtFuranceNumber").val() ? $("#R1Data #txtFuranceNumber").val() : null,
        //TEMPERING_MACHINE_CD: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        METALLIC_PLATE_CODE: $("#R1Data #txtMetallicLabelNo").val() ? $("#R1Data #txtMetallicLabelNo").val() : null,
        //NO_OF_UNITS: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //UNIT_TYPE: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //WEIGHT: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //WATER_OIL_TYPE: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        TEMPERATURE_FURNACE: $("#R1Data #txtTemprature").val() ? $("#R1Data #txtTemprature").val() : null,
        CONCENTRATION_FURNACE: $("#R1Data #txtConcentration").val() ? $("#R1Data #txtConcentration").val() : null,
        FREQUENCY_FURNACE: $("#R1Data #txtfrequency").val() ? $("#R1Data #txtfrequency").val() : null,
        C_FURNACE: $("#R1Data #txtC_FURNACE").val() ? $("#R1Data #txtC_FURNACE").val() : null,
        MN_FURNACE: $("#R1Data #txtMN_FURNACE").val() ? $("#R1Data #txtMN_FURNACE").val() : null,
        MO_FURNACE: $("#R1Data #txtMO_FURNACE").val() ? $("#R1Data #txtMO_FURNACE").val() : null,
        CR_FURNACE: $("#R1Data #txtCR_FURNACE").val() ? $("#R1Data #txtCR_FURNACE").val() : null,
        HARDNESS_MIN: $("#R1Data #txtHardnessMin").val() ? $("#R1Data #txtHardnessMin").val() : null,
        HARDNESS_MAX: $("#R1Data #txtHardnessMax").val() ? $("#R1Data #txtHardnessMax").val() : null,
        OPERATOR_CD_HARDENING: $("#R1Data #ddlNameValue1").val() ? $("#R1Data #ddlNameValue1").val() : null,
        START_DATE_HARDENING: ($("#R1Data #txtHardeningFuranceInDate").val().split(" ")[0] + "T" + $("#R1Data #txtHardeningFuranceInDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        END_DATE_HARDENING: ($("#R1Data #txtHardeningFuranceOutDate").val().split(" ")[0] + "T" + $("#R1Data #txtHardeningFuranceOutDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        TEMPERATURE_HARDENING: $("#R1Data #txtTemperatureHardening").val() ? $("#R1Data #txtTemperatureHardening").val() : null,
        TIME_HARDENING: $("#R1Data #txtTimeHardening").val() ? $("#R1Data #txtTimeHardening").val() : null,
        q_HARDNESS_NORTH_UP_1: $("#R1Data #txt_h_NorthSideUp1").val() ? $("#R1Data #txt_h_NorthSideUp1").val() : null,
        q_HARDNESS_NORTH_UP_2: $("#R1Data #txt_h_NorthSideUp2").val() ? $("#R1Data #txt_h_NorthSideUp2").val() : null,
        q_HARDNESS_NORTH_UP_3: $("#R1Data #txt_h_NorthSideUp3").val() ? $("#R1Data #txt_h_NorthSideUp3").val() : null,
        q_HARDNESS_NORTH_UP_CEN: $("#R1Data #txt_h_NorthSideUpCent").val() ? $("#R1Data #txt_h_NorthSideUpCent").val() : null,
        q_HARDNESS_NORTH_DOWN_1: $("#R1Data #txt_h_NorthSideDown1").val() ? $("#R1Data #txt_h_NorthSideDown1").val() : null,
        q_HARDNESS_NORTH_DOWN_2: $("#R1Data #txt_h_NorthSideDown2").val() ? $("#R1Data #txt_h_NorthSideDown2").val() : null,
        q_HARDNESS_NORTH_DOWN_3: $("#R1Data #txt_h_NorthSideDown3").val() ? $("#R1Data #txt_h_NorthSideDown3").val() : null,
        q_HARDNESS_NORTH_DOWN_CEN: $("#R1Data #txt_h_NorthSideDownCent").val() ? $("#R1Data #txt_h_NorthSideDownCent").val() : null,
        q_HARDNESS_SOUTH_UP_1: $("#R1Data #txt_h_SouthSideUp1").val() ? $("#R1Data #txt_h_SouthSideUp1").val() : null,
        q_HARDNESS_SOUTH_UP_2: $("#R1Data #txt_h_SouthSideUp2").val() ? $("#R1Data #txt_h_SouthSideUp2").val() : null,
        q_HARDNESS_SOUTH_UP_3: $("#R1Data #txt_h_SouthSideUp3").val() ? $("#R1Data #txt_h_SouthSideUp3").val() : null,
        q_HARDNESS_SOUTH_UP_CEN: $("#R1Data #txt_h_SouthSideUpCent").val() ? $("#R1Data #txt_h_SouthSideUpCent").val() : null,
        q_HARDNESS_SOUTH_DOWN_1: $("#R1Data #txt_h_SouthSideDown1").val() ? $("#R1Data #txt_h_SouthSideDown1").val() : null,
        q_HARDNESS_SOUTH_DOWN_2: $("#R1Data #txt_h_SouthSideDown2").val() ? $("#R1Data #txt_h_SouthSideDown2").val() : null,
        q_HARDNESS_SOUTH_DOWN_3: $("#R1Data #txt_h_SouthSideDown3").val() ? $("#R1Data #txt_h_SouthSideDown3").val() : null,
        q_HARDNESS_SOUTH_DOWN_CEN: $("#R1Data #txt_h_SouthSideDownCent").val() ? $("#R1Data #txt_h_SouthSideDownCent").val() : null,
        OPERATOR_CD_TEMPERING: $("#R1Data #ddlNameValue2").val() ? $("#R1Data #ddlNameValue2").val() : null,
        START_DATE_TEMPERING: ($("#R1Data #txtTemperingFuranceInDate").val().split(" ")[0] + "T" + $("#R1Data #txtTemperingFuranceInDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        END_DATE_TEMPERING: ($("#R1Data #txtTemperingFuranceOutDate").val().split(" ")[0] + "T" + $("#R1Data #txtTemperingFuranceOutDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        TEMPERATURE_TEMPERING: $("#R1Data #txtTemperatureTempering").val() ? $("#R1Data #txtTemperatureTempering").val() : null,
        TIME_TEMPERING: $("#R1Data #txtTimeTempering").val() ? $("#R1Data #txtTimeTempering").val() : null,
        t_hARDNESS_NORTH_UP_1: $("#R1Data #txt_t_NorthSideUp1").val() ? $("#R1Data #txt_t_NorthSideUp1").val() : null,
        t_hARDNESS_NORTH_UP_2: $("#R1Data #txt_t_NorthSideUp2").val() ? $("#R1Data #txt_t_NorthSideUp2").val() : null,
        t_hARDNESS_NORTH_UP_3: $("#R1Data #txt_t_NorthSideUp3").val() ? $("#R1Data #txt_t_NorthSideUp3").val() : null,
        t_hARDNESS_NORTH_UP_CENT: $("#R1Data #txt_t_NorthSideUpCent").val() ? $("#R1Data #txt_t_NorthSideUpCent").val() : null,
        t_hARDNESS_NORTH_DOWN_1: $("#R1Data #txt_t_NorthSideDown1").val() ? $("#R1Data #txt_t_NorthSideDown1").val() : null,
        t_hARDNESS_NORTH_DOWN_2: $("#R1Data #txt_t_NorthSideDown2").val() ? $("#R1Data #txt_t_NorthSideDown2").val() : null,
        t_hARDNESS_NORTH_DOWN_3: $("#R1Data #txt_t_NorthSideDown3").val() ? $("#R1Data #txt_t_NorthSideDown3").val() : null,
        t_hARDNESS_NORTH_DOWN_CEN: $("#R1Data #txt_t_NorthSideDownCent").val() ? $("#R1Data #txt_t_NorthSideDownCent").val() : null,
        t_hARDNESS_SOUTH_UP_1: $("#R1Data #txt_t_SouthSideUp1").val() ? $("#R1Data #txt_t_SouthSideUp1").val() : null,
        t_hARDNESS_SOUTH_UP_2: $("#R1Data #txt_t_SouthSideUp2").val() ? $("#R1Data #txt_t_SouthSideUp2").val() : null,
        t_hARDNESS_SOUTH_UP_3: $("#R1Data #txt_t_SouthSideUp3").val() ? $("#R1Data #txt_t_SouthSideUp3").val() : null,
        t_hARDNESS_SOUTH_UP_CENT: $("#R1Data #txt_t_SouthSideUpCent").val() ? $("#R1Data #txt_t_SouthSideUpCent").val() : null,
        t_hARDNESS_SOUTH_DOWN_1: $("#R1Data #txt_t_SouthSideDown1").val() ? $("#R1Data #txt_t_SouthSideDown1").val() : null,
        t_hARDNESS_SOUTH_DOWN_2: $("#R1Data #txt_t_SouthSideDown2").val() ? $("#R1Data #txt_t_SouthSideDown2").val() : null,
        t_hARDNESS_SOUTH_DOWN_3: $("#R1Data #txt_t_SouthSideDown3").val() ? $("#R1Data #txt_t_SouthSideDown3").val() : null,
        t_hARDNESS_SOUTH_DOWN_CEN: $("#R1Data #txt_t_SouthSideDownCent").val() ? $("#R1Data #txt_t_SouthSideDownCent").val() : null,
        HT_NEXT_PROCESS: $("#R1Data #ddlNextProcess1").val() ? $("#R1Data #ddlNextProcess1").val() : null,
        USER_NAME: userName ? userName : null
    });

    console.log(jsonDataR1);
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/UpdateHardnessTemperingDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataR1
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            $("#dialogMessageIcon img").hide();
            $("#imgSuccess").show();

            var successMessage = $("#lblSuccessMessageS_STRAIGHTENING2_ENTRY_001").text();
            bootbox.alert({
                title: "<i class='fas fa-check-circle' aria-hidden='true'></i> " + "Success",
                message: successMessage,
                centerVertical: true,
                callback: function () {
                }
            });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
})

$(document).on('click', '#btnRegister', function () {
    var jsonDataR1 = JSON.stringify({
        HARDENING_MACHINE_CD: HARDENING_TEMPERING_CD ? HARDENING_TEMPERING_CD : null,
        MSG_CD: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        PROCESS_PLAN_CD: $("#R1Data #PROCESS_PLAN_CD").val() ? $("#R1Data #PROCESS_PLAN_CD").val() : null,
        FURNACE_NO: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //TEMPERING_MACHINE_CD: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        METALLIC_PLATE_CODE: $("#R1Data #txtMetallicLabelNo").val() ? $("#R1Data #txtMetallicLabelNo").val() : null,
        //NO_OF_UNITS: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //UNIT_TYPE: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //WEIGHT: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        //WATER_OIL_TYPE: $("#R1Data #hdfldProcessPlanCd").val() ? $("#R1Data #hdfldProcessPlanCd").val() : null,
        TEMPERATURE_FURNACE: $("#R1Data #txtTemprature").val() ? $("#R1Data #txtTemprature").val() : null,
        CONCENTRATION_FURNACE: $("#R1Data #txtConcentration").val() ? $("#R1Data #txtConcentration").val() : null,
        FREQUENCY_FURNACE: $("#R1Data #txtfrequency").val() ? $("#R1Data #txtfrequency").val() : null,
        C_FURNACE: $("#R1Data #txtC_FURNACE").val() ? $("#R1Data #txtC_FURNACE").val() : null,
        MN_FURNACE: $("#R1Data #txtMN_FURNACE").val() ? $("#R1Data #txtMN_FURNACE").val() : null,
        MO_FURNACE: $("#R1Data #txtMO_FURNACE").val() ? $("#R1Data #txtMO_FURNACE").val() : null,
        CR_FURNACE: $("#R1Data #txtCR_FURNACE").val() ? $("#R1Data #txtCR_FURNACE").val() : null,
        HARDNESS_MIN: $("#R1Data #txtHardnessMin").val() ? $("#R1Data #txtHardnessMin").val() : null,
        HARDNESS_MAX: $("#R1Data #txtHardnessMax").val() ? $("#R1Data #txtHardnessMax").val() : null,
        OPERATOR_CD_HARDENING: $("#R1Data #ddlNameValue1").val() ? $("#R1Data #ddlNameValue1").val() : null,
        START_DATE_HARDENING: ($("#R1Data #txtHardeningFuranceInDate").val().split(" ")[0] + "T" + $("#R1Data #txtHardeningFuranceInDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        END_DATE_HARDENING: ($("#R1Data #txtHardeningFuranceOutDate").val().split(" ")[0] + "T" + $("#R1Data #txtHardeningFuranceOutDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        TEMPERATURE_HARDENING: $("#R1Data #txtTemperatureHardening").val() ? $("#R1Data #txtTemperatureHardening").val() : null,
        TIME_HARDENING: $("#R1Data #txtTimeHardening").val() ? $("#R1Data #txtTimeHardening").val() : null,
        Q_HARDNESS_NORTH_UP_1: $("#R1Data #txt_h_NorthSideUp1").val() ? $("#R1Data #txt_h_NorthSideUp1").val() : null,
        Q_HARDNESS_NORTH_UP_2: $("#R1Data #txt_h_NorthSideUp2").val() ? $("#R1Data #txt_h_NorthSideUp2").val() : null,
        Q_HARDNESS_NORTH_UP_3: $("#R1Data #txt_h_NorthSideUp3").val() ? $("#R1Data #txt_h_NorthSideUp3").val() : null,
        Q_HARDNESS_NORTH_UP_CEN: $("#R1Data #txt_h_NorthSideUpCent").val() ? $("#R1Data #txt_h_NorthSideUpCent").val() : null,
        Q_HARDNESS_NORTH_DOWN_1: $("#R1Data #txt_h_NorthSideDown1").val() ? $("#R1Data #txt_h_NorthSideDown1").val() : null,
        Q_HARDNESS_NORTH_DOWN_2: $("#R1Data #txt_h_NorthSideDown2").val() ? $("#R1Data #txt_h_NorthSideDown2").val() : null,
        Q_HARDNESS_NORTH_DOWN_3: $("#R1Data #txt_h_NorthSideDown3").val() ? $("#R1Data #txt_h_NorthSideDown3").val() : null,
        Q_HARDNESS_NORTH_DOWN_CEN: $("#R1Data #txt_h_NorthSideDownCent").val() ? $("#R1Data #txt_h_NorthSideDownCent").val() : null,
        Q_HARDNESS_SOUTH_UP_1: $("#R1Data #txt_h_SouthSideUp1").val() ? $("#R1Data #txt_h_SouthSideUp1").val() : null,
        Q_HARDNESS_SOUTH_UP_2: $("#R1Data #txt_h_SouthSideUp2").val() ? $("#R1Data #txt_h_SouthSideUp2").val() : null,
        Q_HARDNESS_SOUTH_UP_3: $("#R1Data #txt_h_SouthSideUp3").val() ? $("#R1Data #txt_h_SouthSideUp3").val() : null,
        Q_HARDNESS_SOUTH_UP_CEN: $("#R1Data #txt_h_SouthSideUpCent").val() ? $("#R1Data #txt_h_SouthSideUpCent").val() : null,
        Q_HARDNESS_SOUTH_DOWN_1: $("#R1Data #txt_h_SouthSideDown1").val() ? $("#R1Data #txt_h_SouthSideDown1").val() : null,
        Q_HARDNESS_SOUTH_DOWN_2: $("#R1Data #txt_h_SouthSideDown2").val() ? $("#R1Data #txt_h_SouthSideDown2").val() : null,
        Q_HARDNESS_SOUTH_DOWN_3: $("#R1Data #txt_h_SouthSideDown3").val() ? $("#R1Data #txt_h_SouthSideDown3").val() : null,
        Q_HARDNESS_SOUTH_DOWN_CEN: $("#R1Data #txt_h_SouthSideDownCent").val() ? $("#R1Data #txt_h_SouthSideDownCent").val() : null,
        OPERATOR_CD_TEMPERING: $("#R1Data #ddlNameValue2").val() ? $("#R1Data #ddlNameValue2").val() : null,
        START_DATE_TEMPERING: ($("#R1Data #txtTemperingFuranceInDate").val().split(" ")[0] + "T" + $("#R1Data #txtTemperingFuranceInDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        END_DATE_TEMPERING: ($("#R1Data #txtTemperingFuranceOutDate").val().split(" ")[0] + "T" + $("#R1Data #txtTemperingFuranceOutDate").val().split(" ")[1] + ":00.000Z").replaceAll('/', '-'),
        TEMPERATURE_TEMPERING: $("#R1Data #txtTemperatureTempering").val() ? $("#R1Data #txtTemperatureTempering").val() : null,
        TIME_TEMPERING: $("#R1Data #txtTimeTempering").val() ? $("#R1Data #txtTimeTempering").val() : null,
        T_HARDNESS_NORTH_UP_1: $("#R1Data #txt_t_NorthSideUp1").val() ? $("#R1Data #txt_t_NorthSideUp1").val() : null,
        T_HARDNESS_NORTH_UP_2: $("#R1Data #txt_t_NorthSideUp2").val() ? $("#R1Data #txt_t_NorthSideUp2").val() : null,
        T_HARDNESS_NORTH_UP_3: $("#R1Data #txt_t_NorthSideUp3").val() ? $("#R1Data #txt_t_NorthSideUp3").val() : null,
        T_HARDNESS_NORTH_UP_CENT: $("#R1Data #txt_t_NorthSideUpCent").val() ? $("#R1Data #txt_t_NorthSideUpCent").val() : null,
        T_HARDNESS_NORTH_DOWN_1: $("#R1Data #txt_t_NorthSideDown1").val() ? $("#R1Data #txt_t_NorthSideDown1").val() : null,
        T_HARDNESS_NORTH_DOWN_2: $("#R1Data #txt_t_NorthSideDown2").val() ? $("#R1Data #txt_t_NorthSideDown2").val() : null,
        T_HARDNESS_NORTH_DOWN_3: $("#R1Data #txt_t_NorthSideDown3").val() ? $("#R1Data #txt_t_NorthSideDown3").val() : null,
        T_HARDNESS_NORTH_DOWN_CEN: $("#R1Data #txt_t_NorthSideDownCent").val() ? $("#R1Data #txt_t_NorthSideDownCent").val() : null,
        T_HARDNESS_SOUTH_UP_1: $("#R1Data #txt_t_SouthSideUp1").val() ? $("#R1Data #txt_t_SouthSideUp1").val() : null,
        T_HARDNESS_SOUTH_UP_2: $("#R1Data #txt_t_SouthSideUp2").val() ? $("#R1Data #txt_t_SouthSideUp2").val() : null,
        T_HARDNESS_SOUTH_UP_3: $("#R1Data #txt_t_SouthSideUp3").val() ? $("#R1Data #txt_t_SouthSideUp3").val() : null,
        T_HARDNESS_SOUTH_UP_CENT: $("#R1Data #txt_t_SouthSideUpCent").val() ? $("#R1Data #txt_t_SouthSideUpCent").val() : null,
        T_HARDNESS_SOUTH_DOWN_1: $("#R1Data #txt_t_SouthSideDown1").val() ? $("#R1Data #txt_t_SouthSideDown1").val() : null,
        T_HARDNESS_SOUTH_DOWN_2: $("#R1Data #txt_t_SouthSideDown2").val() ? $("#R1Data #txt_t_SouthSideDown2").val() : null,
        T_HARDNESS_SOUTH_DOWN_3: $("#R1Data #txt_t_SouthSideDown3").val() ? $("#R1Data #txt_t_SouthSideDown3").val() : null,
        T_HARDNESS_SOUTH_DOWN_CEN: $("#R1Data #txt_t_SouthSideDownCent").val() ? $("#R1Data #txt_t_SouthSideDownCent").val() : null,
        HT_NEXT_PROCESS: $("#R1Data #ddlNextProcess1").val() ? $("#R1Data #ddlNextProcess1").val() : null,
        USER_NAME: userName ? userName : null
    });

    console.log(jsonDataR1);
    var ajaxTime = new Date().getTime();
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/InsertHardnessTemperingDetails",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: jsonDataR1
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            $("#dialogMessageIcon img").hide();
            $("#imgSuccess").show();

            var successMessage = $("#lblSuccessMessageS_STRAIGHTENING2_ENTRY_001").text();
            bootbox.alert({
                title: "<i class='fas fa-check-circle' aria-hidden='true'></i> " + "Success",
                message: successMessage,
                centerVertical: true,
                callback: function () {
                }
            });
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_STRAIGHTENING2_ENTRY_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
})

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

function checkDate(field) {
    //var startDateTime = field.val();// + " " + $("#txtStartTime").val();
    //var endDateTime = $("#txtEndDate").val() + " " + $("#txtEndTime").val();
    var minYear = 2019;
    var maxYear = 2100;

    // regular expression to match required date format
    re = /^(\d{4})\/(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01]) (([0-1]{0,1}[0-9])|(2[0-3])):[0-5]{0,1}[0-9]$/
    if (field.val() != '') {
        if (regs = field.val().match(re)) {
            console.log(regs);
            if ((regs[3] < 1 || regs[3] > 31) || (regs[2] < 1 || regs[2] > 12) || (regs[1] < minYear || regs[1] > maxYear)) {

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