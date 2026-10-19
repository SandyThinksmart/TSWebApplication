var R4; 
var R3;
var R2;
var R1;
var M1;
var M2;
var M3;
var M4;
var R5;
var decisionLegends = {
    "1": "<span id='lblMaru' class='labelClass' style='color:Blue;'>●</span>",
    "2": "<span id='lblCross' class='labelClass' style='color:Red;font-weight:bold;'>×</span>",
    "3": "<span id='lblTraingle' class='labelClass' style='color:Orange;'>▲</span>",
    "4": "<span id='lblDiamond' class='labelClass' style='color:Green;'>◆</span>",
    "5": "<span id='lblSquare' class='labelClass' style='color:DarkRed;'>■</span>"
} // decisionLegends used for all processing screens to display taba on top side with color

$('input, :input').attr('autocomplete', 'off');
var screenNames = {
    "LOGIN": [500, 400],        //(width ,height)
    "MENU": [850, 768],
    "MENU_DEV": [850, 768],
    "HTLOGIN": [255, 445],
    "MASTER_COUNTRY": [830, 360],

    "MASTER_CUSTOMER_TYPE": [900, 370],
    "MASTER_CURRENCY": [830, 470],
    "MASTER_CUSTOMER": [1190, 780],
    "MASTER_RECEIVER": [1120, 480],
    "MASTER_SUPPLIER": [1200, 770],
    "MASTER_BANK": [950, 400],
    "MASTER_DESIGNATION": [835, 340],
    "MASTER_EMPLOYEE": [1100, 500],
    "MASTER_DEPARTMENT_GROUP": [900, 420],
    "MASTER_LOCATION": [940, 440],
    "MASTER_DEPARTMENT": [900, 360],
    "MASTER_SALES_INCHARGE": [1180, 770],
    "MASTER_MAKER": [1220, 440],
    "MASTER_MUKESAKI": [1100, 450],
    "MASTER_FUTOU": [1110, 470],
    "MASTER_HIKITORISAKI": [1110, 520],
    "MASTER_SUBCONTRACTOR": [1100, 725],
    "MASTER_PACKING": [830, 370],
    "MASTER_PROCESS": [1020, 420],
    "MASTER_PROCESS_TEMPLATE": [1820, 710],
    "MASTER_MACHINE": [840, 400],
    "MASTER_MACHINE_GROUP": [840, 390],
    "MASTER_PRODUCT": [1280, 920],
    "MASTER_PROD_NAME_INTERNAL": [980, 390],
    "MASTER_STEEL_TYPE": [870, 425],
    "MASTER_MATERIAL_NAME_INTERNAL": [840, 400],
    "MASTER_MATERIAL_NAME_MAKER": [840, 430],
    "MASTER_MATERIAL": [1210, 610],
    "MASTER_CONDITION": [850, 350],
    "MASTER_CONFIGURATION": [860, 400],
    "MASTER_PRODUCT_TYPE": [870, 425],
    "MASTER_CURRENCY_ENTRY": [850, 950],
    "MASTER_SHUKKA_DEFECT_ENTRY": [830, 430],
    "MASTER_PROCESS_DEFECT": [835, 340],
    "MASTER_CHEMICAL_COMPOSITION": [870, 570],
    "MASTER_JIS_TEST": [835, 340],

    "LIST_MASTER_COUNTRY": [720, 700],
    "LIST_MASTER_CURRENCY": [930, 700],
    "LIST_MASTER_BANK": [1250, 740],
    "LIST_MASTER_CONDITION": [650, 720],
    "LIST_MASTER_CUSTOMER": [1280, 700],
    "LIST_MASTER_CUSTOMER_TYPE": [750, 740],
    "LIST_MASTER_DEPARTMENT_GROUP": [1010, 640],
    "LIST_MASTER_DEPARTMENT": [600, 660],
    "LIST_MASTER_EMPLOYEE": [1660, 680],
    "LIST_MASTER_FUTOU": [1480, 750],
    "LIST_MASTER_HIKITORISAKI": [1280, 750],
    "LIST_MASTER_LOCATION": [1320, 640],
    "LIST_MASTER_MACHINE_GROUP": [750, 650],
    "LIST_MASTER_MACHINE": [990, 660],
    "LIST_MASTER_MAKER": [1280, 700],
    "LIST_MASTER_MATERIAL": [1059, 750],
    "LIST_MASTER_MATERIAL_NAME_INTERNAL": [730, 700],
    "LIST_MASTER_MATERIAL_NAME_MAKER": [980, 730],
    "LIST_MASTER_STEEL_TYPE": [690, 800],
    "LIST_MASTER_MUKESAKI": [1280, 750],
    "LIST_MASTER_PACKING": [720, 700],
    "LIST_MASTER_PROCESS": [1150, 650],
    "LIST_MASTER_PROCESS_TEMPLATE": [700, 700],
    "LIST_MASTER_PRODUCT": [1200, 780],
    "LIST_MASTER_PROD_NAME_INTERNAL": [900, 700],
    "LIST_MASTER_RECEIVER": [1200, 700],
    "LIST_MASTER_SUBCONTRACTOR": [1280, 750],
    "LIST_MASTER_SUPPLIER": [1330, 750],
    "LIST_MASTER_CONFIGURATION": [1150, 700],
    "LIST_MASTER_PRODUCT_TYPE": [650, 750],
    "LIST_MASTER_SHUKKA_DEFECT": [650, 700],
    "LIST_MASTER_PROCESS_DEFECT": [800, 700],
    "LIST_MASTER_CHEMICAL_COMPOSITION": [1320, 700],
    "LIST_JIS_TEST": [780, 660],

    "M_CUSTOMER_FINANCE_DEPARTMENT_ENTRY": [1180, 560],
    "M_PAYEE_ENTRY": [1380, 750],
    "M_MOCHIKOMISAKI_ENTRY": [1100, 500],
    "M_SPOT_AND_SHIKYU_MATERIAL_ENTRY": [1075, 700],
    "M_CUSTOMER_FINANCE_DEPARTMENT_LIST": [1280, 720],
    "M_DESIGNATION_LIST": [540, 680],
    "M_MOCHIKOMISAKI_LIST": [1320, 700],
    "M_PAYEE_LIST": [1280, 700],
    "M_SPOT_AND_SHIKYU_MATERIAL_LIST": [970, 480],
    "M_PRODUCT_VERSION_HISTORY": [1500, 700],
    "USER_LIST": [840, 570],
    "ACT_ALLOCATION_ENTRY": [1290, 600],
    "PROCESS_STATUS_SEARCH_FOR_FACTORY": [1800, 940],

    "MMS_MATERIAL_STATUS_SEARCH_SCREEN_1": [1970, 1050],  //Bundled
    "MMS_MATERIAL_STATUS_SEARCH_SCREEN_2": [1300, 1400],  //Bundled


    "KOUBAN_ENTRY": [1430, 830],            //Bundled
    //"PURCHASE_ORDER_ENTRY": [1400, 900],    //Bundled check
    "KAKOUIRAI_ENTRY": [960, 920],            //Bundled
    "FUTOU_NYUKO_ENTRY": [1390, 800],        //Bundled
    "FUTOU_NYUKO_EDIT_DELETE": [1290, 750], //Bundled
    "KOUBAN_COMPLETION_SCREEN": [1385, 810], //Bundled
    "KHAKOIRAI_COMPLETION_SCREEN": [1395, 720],
    "FUTOU_SHUKKO_ENTRY": [1320, 730],      //Bundled
    "FUTOU_SHUKKO_EDIT_DELETE": [1420,850],//Bundled
    "JIZAI_NYUKA_ENTRY": [1520, 700],        //Bundled
    "JIZAI_NYUKA_EDIT_DELETE": [1420, 820],  //Bundled
    "NIFUDA_DIVISION_SCREEN": [1290, 645],  //Bundled
    "PROCESS_PLAN_REPROCESSING_1": [900, 600], //Bundled
    "PROCESS_PLAN_REPROCESSING_2": [1280, 750], //Bundled
    "MATERIAL_SHUKKO_ENTRY_SCREEN": [1300, 770], //Bundled
    "MATERIAL_SHUKKO_EDIT_DELETE": [1130,350 ],   //Bundled
    "SPOT_PO_ENTRY_SCREEN": [690, 700],   //Bundled
    "HIKITORIIRAI_ENTRY_SCREEN": [1090, 745],
    "INVOICE_ENTRY_SCREEN": [1300, 770],
    "TEMP_ALLOCATION_ENTRY": [1260, 850],
    "INVOICE_LIST": [880, 669],
    "MATERIAL_NYUKO_KOUBAN_POPUP": [1330, 820],
    "PURCHASE_ORDER_ENTRY": [1330, 810],
    "REQUEST_MATL_ENTRY_SCREEN": [1090, 745],
    "HEAT_TREATMENT_ENTRY": [1580, 1000],
}

$('.CharOnly').on('input', function () {
    
    $(this).val($(this).val().replace(/[^a-zA-Z]/g, ''));
});
function ConfirmDelete() {
    //ToDo: Need to get this message from resources
    var result = confirm("This will permenently delete the record. Are you sure you want to continue?");
    if (result == true)
        return true;
    else
        return false;
}

function resize(width, height) {
    window.resizeTo(width, height);
}


function onLoad() {
    var screenName = $("#hdfScreenName").val();
   // window.moveTo(0, 0);

    if (screenName != undefined) {
        var widthHeight = screenNames[screenName];

        resize(widthHeight[0], widthHeight[1]);
    }
}
////function Mask() {
////    $(".txtDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

////}
function window_onkeydown(ev, index) {
    if (ev.keyCode != undefined) {
        if (ev.keyCode.toString() == "13") {
            setFocus(index);
        }

        if (ev.keyCode.toString() == "66") {
            setFocus(index);
        }
    }
}

function setFocus(index) {

    var controls;
    controls = this.form1.elements;

    for (var i = 0; i < (controls.length) ; i++) {
        if (controls[i].tabIndex == index)
            controls[i].focus();
    }
}


function onComboKeyDown(id, ev, index) {

    var Flag = document.getElementById('hdfEditModeValue').value;


    // check for Up Navigation Key Press to change the menuItems//
    if (Flag.toString() == "1") {

        if (ev.keyCode.toString() == "1") {

            var selectedIndex = document.getElementById(id).selectedIndex;

            if (selectedIndex.toString() == "0") {
                var ind = index - 1;
                ev.preventDefault();
            }
            else {
                document.getElementById(id).selectedIndex = selectedIndex - 1;
                ev.preventDefault();
            }
        }

        // check for Down Navigation Key Press to change the menuItems//
        if (ev.keyCode.toString() == "2") {
            var selectedIndex = document.getElementById(id).selectedIndex;
            document.getElementById(id).selectedIndex = selectedIndex + 1;
            ev.preventDefault();
        }


    }

    // check for 'Enter' Key press to put the Dropdownlist in edit mode and set Flag Value to 1 //   
    if (ev.keyCode.toString() == "13") {

        var FlagValue = document.getElementById('hdfEditModeValue').value;

        if (FlagValue.toString() == "0" || FlagValue.toString() == "") {
            document.getElementById('hdfEditModeValue').value = "1";
        }
        else {
            document.getElementById('hdfEditModeValue').value = "0";
        }
        ev.preventDefault();

    }
}

function setInitFocus() {
    var controls;
    if (navigator.appName.toString() == "Netscape")
        controls = document.getElementById("form1").elements;
    else
        controls = this.form1.elements;

    for (var i = 0; i < (controls.length) ; i++) {
        if (controls[i].tabIndex == 1)
            controls[i].focus();
    }
}

function PageRedirect(path) {
    window.location.href = path;
}


function ToUpper(ctrl) {
    var e = window.event;
    var charCode = event.keyCode;

    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
    {
        var t = ctrl.value;
         ctrl.value = t.toUpperCase();
    }
    else
        e.cancelBubble = true;
}

function Confirmation() {
    var win = window.open('', '_self');
    win.close();
}
function CloseScreen() {
    var win = window.open('', '_self');
    win.close();
}

function movetoNext(current, nextFieldID) {
    if (current.value.length >= current.maxLength) {
        document.getElementById(nextFieldID).focus();
    }
} //For Hikinuki & EC Screen

$(document).ready(function () {
    SetIME();
    onLoad();
    // Mask();
    /**
    2017/02/22 Girish Badwaik
    Added common code to prevent ENTER keypress event on textboxes.
    **/
    $(document).keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);

        // Only stop the 'Enter' for entry fields, not buttons.
        if (event.keyCode == 13 && $(event.target).is("input[type=text]")) {
            //.hasClass("_functionButtons"))) {
            event.stopPropagation();
            event.preventDefault();
        }
        else {
            return true;
        }


        // Following code is specific to Kouban Reply Screen.
        // To auto-click the PO search button on main screen when scanned with bar-code scanner
        if ($(event.target).hasClass("PoSearchText")) {
            var $thisChuubanGyouban = $(event.target);
            var $btnPOSearch = $("#btnPoSearch");
            $btnPOSearch.trigger("click");
        }

        // Following code is is specific to Multi PO, Kouban Search Popup.
        // To move focus among PO search inputs on popup screen when scanned with bar-code scanner
        if ($(event.target).hasClass("textBoxHSKPOKoubanSearchComponent")) {
            // Get the current element's ID. Next element's ID is ID + 1. Hence the logic.
            var $thisInput = $(event.target);
            var arraySplittedID = $thisInput.attr("id").split("_");  // id is like mePoNoSearchPopup_grdvSearchGrid_txtEntryField_5
            var idNo = parseInt(arraySplittedID[arraySplittedID.length - 1]); // get the sequence no from id
            var nextIdNo = idNo + 1; // increment the sequence number
            arraySplittedID.pop(); // remove id number
            var nextId = arraySplittedID.join("_") + "_" + nextIdNo; // create new id with new sequence no
            var $nextInput = $("#" + nextId);
            // if next input is found, focus there. Otherwise focus on search button.
            if ($nextInput.length) {
                $nextInput.focus();
            }
            else {
                $(".btnSearchHSKPOKoubanSearchComponent").focus();
            }
        }

        // Following code is specific to Product Nifuda Generation Screen actAllocChuuban
        if ($(event.target).hasClass("ChuubanGyouban")) {
            var $thisChuubanGyouban = $(event.target);
            var thisChuubanGyoubanID = $thisChuubanGyouban.attr("id");
            var nextChuubanGyoubanIDNumber = parseInt(thisChuubanGyoubanID.split("grdvChubanInfo_txtChuuban_")[1]) + 1;
            var $nextChuubanGyouban = $("#grdvChubanInfo_txtChuuban_" + nextChuubanGyoubanIDNumber);
            if ($nextChuubanGyouban.length) {
                // Set focus on next chuubanGyouban
                $nextChuubanGyouban.focus();
            }
            else {
                $("#btnSelect").focus();
            }
        }

        // Following code is specific to Product Nifuda Generation Screen 
        if ($(event.target).hasClass("actAllocChuuban")) {
            var $thisChuubanGyouban = $("#btnProductSearch");
            //var thisChuubanGyoubanID = $thisChuubanGyouban.attr("id");
            //var nextChuubanGyoubanIDNumber = parseInt(thisChuubanGyoubanID.split("grdvChubanInfo_txtChuuban_")[1]) + 1;
            //var $nextChuubanGyouban = $("#grdvChubanInfo_txtChuuban_" + nextChuubanGyoubanIDNumber);
            //if ($nextChuubanGyouban.length) {
            //    // Set focus on next chuubanGyouban
            //    $nextChuubanGyouban.focus();
            //}
            //else {
            //    $("#btnSelect").focus();
            //}
            $thisChuubanGyouban.click();
        }

        // Following code is specific to PMS Entry Screens
        if ($(event.target).hasClass("PMSEntryMachine")) {
            $("#txtChuuban1").focus();
        }

        if ($(event.target).hasClass("PMSEntryChuuban")) {
            var $thisChuuban = $("#txtChuuban1");
            var $thisGyouban = $("#txtGyouban1");
            var chuubanGyoubanVal = $thisChuuban.val();
            if (chuubanGyoubanVal.length > 9) {
                var chuuban = chuubanGyoubanVal.slice(0, 9); // Chuuban's length is 9
                var gyouban = chuubanGyoubanVal.slice(9); // Remaining is Gyouban

                $thisChuuban.val(chuuban);
                $thisGyouban.val(gyouban);

                $("#btnChuubanSearch").focus().click();
            }
        }
        // End code for PMS Entry Screens


    });

});



//Fractions into decimals
function RestrictToValidDecimalFormat($target, validPreDotLength, validPostdotLength) {

    var isValueDefined = typeof $target.value != 'undefined';
    var originalValue = isValueDefined ? $target.value : $target.val();
    var preDotValue = originalValue;
    var postDotValue = "";
    var isNegative = originalValue.indexOf("-") === 0;
    var isDotPresent = originalValue.indexOf(".") > -1;

    if (isNegative) {
        preDotValue = preDotValue.substring(1, preDotValue.length);
        // alert(preDotValue);
    }

    if (isDotPresent) {
        // split dot
        var splitValues = preDotValue.split(".");
        preDotValue = splitValues[0];
        postDotValue = splitValues[1];
    }


    if (preDotValue.length > validPreDotLength) {
        var newPreDotValue = preDotValue.substring(0, validPreDotLength);
        preDotValue = newPreDotValue;
    }

    if (postDotValue.length > validPostdotLength) {
        var newPostDotValue = postDotValue.substring(0, validPostdotLength);
        postDotValue = newPostDotValue;
    }

    var resetNumber = "";

    if (isNegative) {
        resetNumber += "-";
    }

    resetNumber += String(preDotValue);

    if (isDotPresent) {
        resetNumber += ".";
    }
    if (String(postDotValue) !== "") {
        resetNumber += String(postDotValue);
    }

    if (isValueDefined) {
        $target.value = resetNumber;
    }
    else {
        $target.val(resetNumber);
    }

}

function IsNumberValid(number) {

    var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
    if (keyCodeEntered == 46) {
        if ((number.value) && (number.value.indexOf('.') >= 0)) {
            return false;
        }
        else

            return true;
    }
}

// CREATED BY RAHUL SUTAR 2024/06/12
// FUNCTION IsActualDimentionValid USED TO HANDLE ACTUAL DIMENTION IS IN TOLERANCE RANGE OR NOT
// PRODUCT_DIM, DIM_TOLERANCE AND ENTERED DIM PASSED IN PARAMETER ARRAY AS PER SCREEN DATA
// DIM1 TO DIM6 PARAMETERS ARE REQUIRED TO PASS, IF LESS THAN SIX FIELDS FOR DIMENTION ON SCREEN THEN PASS NULL(EXAMPLE DIM4=NULL; DIM5=NULL;
// IF DIMENTION IS IN RANGE THEN GET RETURN THE FUNCTION TRUE, IF NOT IN RANGE THEN GET RETURN THE FUNCTION FALSE WITH INCORRECT DIMENTION VALUE.
function IsActualDimentionValid(request) {
    var actDimention = request.PRODUCT_DIM;
    var dimTol = request.DIM_TOLERANCE;
    var arrayDimTol = dimTol.split(",");
    var maxDimValue = actDimention;
    var minDimValue = actDimention;
    var invalidDim = "";
    var isSuccess = true;
    var returnFunction = {};
    if (arrayDimTol[0] != "") {
        if (arrayDimTol[0].startsWith("+")) {
            maxDimValue = parseFloat(actDimention) + parseFloat(arrayDimTol[0].replace("+", ""));
        } else {

            maxDimValue = parseFloat(actDimention) - parseFloat(arrayDimTol[0].replace("-", ""));
        }

        if (arrayDimTol[1].startsWith("+")) {
            minDimValue = parseFloat(actDimention) + parseFloat(arrayDimTol[1].replace("+", ""));
        } else {

            minDimValue = parseFloat(actDimention) - parseFloat(arrayDimTol[1].replace("-", ""));
        }
        maxDimValue = maxDimValue.toFixed(3);
        minDimValue = minDimValue.toFixed(3);

        if (parseFloat(request.DIM1) < minDimValue || parseFloat(request.DIM1) > maxDimValue) {

            invalidDim = request.DIM1;
            isSuccess = false;
        }

        if (request.DIM2 != null && invalidDim == "") {
            if (parseFloat(request.DIM2) < minDimValue || parseFloat(request.DIM2) > maxDimValue) {

                invalidDim = request.DIM2;
                isSuccess = false;
            }
        }

        if (request.DIM3 != null && invalidDim == "") {
            if (parseFloat(request.DIM3) < minDimValue || parseFloat(request.DIM3) > maxDimValue) {

                invalidDim = request.DIM3;
                isSuccess = false;
            }
        }

        if (request.DIM4 != null && invalidDim == "") {
            if (parseFloat(request.DIM4) < minDimValue || parseFloat(request.DIM4) > maxDimValue) {

                invalidDim = request.DIM4;
                isSuccess = false;
            }
        }

        if (request.DIM5 != null && invalidDim == "") {
            if (parseFloat(request.DIM5) < minDimValue || parseFloat(request.DIM5) > maxDimValue) {

                invalidDim = request.DIM5;
                isSuccess = false;
            }
        }

        if (request.DIM6 != null && invalidDim == "") {
            if (parseFloat(request.DIM6) < minDimValue || parseFloat(request.DIM6) > maxDimValue) {

                invalidDim = request.DIM6;
                isSuccess = false;
            }
        }
    } else
    {
        isSuccess = true;
    }
        returnFunction["IsSuccess"] = isSuccess;
        returnFunction["InvalidDim"] = invalidDim;
    return returnFunction;

}

// CREATED BY RAHUL SUTAR 2024/06/12
// FUNCTION IsActualLengthValid USED TO HANDLE ACTUAL LENGTH IS IN TOLERANCE RANGE OR NOT
// PRODUCT_LENGTH, LENGTH_TOLERANCE AND ENTERED LENGTH PASSED IN PARAMETER ARRAY AS PER SCREEN DATA
// LENGTH1 TO LENGTH3 PARAMETERS ARE REQUIRED TO PASS, IF LESS THAN THREE FIELDS FOR LENGTH ON SCREEN THEN PASS NULL(EXAMPLE LENGTH2=NULL; LENGTH3=NULL;
// IF PRODUCT_LENGTH CONTAIN HAS CHAR THE DIRECTLY RETURN THE FUNCTION TRUE
// IF LENGTH IS IN RANGE THEN GET RETURN THE FUNCTION TRUE, IF NOT IN RANGE THEN GET RETURN THE FUNCTION FALSE WITH INCORRECT LENGTH VALUE.
function IsActualLengthValid(request) {
    var actualLength = request.PRODUCT_LENGTH;
    var lengthTol = request.LENGTH_TOLERANCE;
    var arrayLenTol = lengthTol.split(",");
    var maxLengthValue = actualLength;
    var minLengthValue = actualLength;
    var invalidLength = "";
    var isSuccess = true;
    var returnFunction = {};
    var format = /[a-zA-Zﾂｷﾞﾘｲｺｲﾙﾂｷﾘﾅｶﾞﾓﾉ]+/;

    if (!format.test(actualLength) && actualLength != null) {

        if (arrayLenTol[0].startsWith("+")) {
            maxLengthValue = parseFloat(actualLength) + parseFloat(arrayLenTol[0].replace("+", ""));
        } else {

            maxLengthValue = parseFloat(actualLength) - parseFloat(arrayLenTol[0].replace("-", ""));
        }

        if (arrayLenTol[1].startsWith("+")) {
            minLengthValue = parseFloat(actualLength) + parseFloat(arrayLenTol[1].replace("+", ""));
        } else {

            minLengthValue = parseFloat(actualLength) - parseFloat(arrayLenTol[1].replace("-", ""));
        }
       
        maxLengthValue = maxLengthValue.toFixed(2);

        minLengthValue = minLengthValue.toFixed(2);

        if (parseFloat(request.LENGTH1) < minLengthValue || parseFloat(request.LENGTH1) > maxLengthValue && request.LENGTH1 != null) {

            invalidLength = request.LENGTH1;
            isSuccess = false;
        }

        if (request.LENGTH2 != null && invalidLength == "") {
            if (parseFloat(request.LENGTH2) < minLengthValue || parseFloat(request.LENGTH2) > maxLengthValue) {

                invalidLength = request.LENGTH2;
                isSuccess = false;
            }
        }
        if (request.LENGTH3 != null && invalidLength == "") {
            if (parseFloat(request.LENGTH3) < minLengthValue || parseFloat(request.LENGTH3) > maxLengthValue) {

                invalidLength = request.LENGTH3;
                isSuccess = false;
            }
        }
    }

    returnFunction["IsSuccess"] = isSuccess;
    returnFunction["InvalidLength"] = invalidLength;
    return returnFunction;

}
// CREATED BY RAHUL SUTAR 2024/06/12
// FUNCTION IsFinishedQuantityValid USED TO HANDLE PRODUCT WEIGHT IS IN TOLERANCE RANGE OR NOT
// PRODUCT_WEIGHT AND ENTERED WEIGHT PASSED IN PARAMETER ARRAY AS PER SCREEN DATA
// DEFAULT ± 2% USED TO CALCULATE WEIGHT;
// IF WEIGHT IS IN RANGE THEN GET RETURN THE FUNCTION TRUE, IF NOT IN RANGE THEN GET RETURN THE FUNCTION FALSE.
function IsFinishedQuantityValid(request) {
    
    var actualWeight = request.PRODUCT_WEIGHT;
    var inputWeight = request.INPUT_WEIGHT;
    var prod_Shape = request.PRODUCT_SHAPE;
    var prod_Dim = request.PRODUCT_DIM;
    var no_Of_Units = request.NO_OF_UNITS;
    var prod_Length = request.PRODUCT_LENGTH;
    var constant=0;
    
    if (prod_Shape == "R")
    {
        constant = 0.00617;
    } else if (prod_Shape == "H")
    {
        constant = 0.00676;
    } else if (prod_Shape == "S")
    {
        constant = 0.00785;
    }
    var optQty = (parseFloat(prod_Dim) * parseFloat(prod_Dim) * parseFloat(constant) * parseFloat(prod_Length)  * parseFloat(no_Of_Units)) / 1000;

    var minQty = (parseFloat(optQty) * 0.98).toFixed(2);
    var maxQty = (parseFloat(optQty) * 1.02).toFixed(2);
    //var avgquantity = parseFloat(actualWeight) * 2 / 100;
    //var maxWeightValue = parseFloat(actualWeight) + parseFloat(avgquantity);
    //var minWeightValue = parseFloat(actualWeight) - parseFloat(avgquantity);
    var isSuccess = true;
    var returnFunction = {};

    //minWeightValue = minWeightValue.toFixed(2);
    //maxWeightValue = maxWeightValue.toFixed(2);

    if (parseFloat(inputWeight) < parseFloat(minQty) || parseFloat(inputWeight) > parseFloat(maxQty))
    {
            isSuccess = false;
    }
 

    returnFunction["IsSuccess"] = isSuccess;
    return returnFunction;
}

function caldispatchHistoryMonthHeaders(currentR) {
    debugger;
    var returnBaseMonth = {};
    var date  = new Date();
    var currentDate = new Date(date.getFullYear(), date.getMonth() - 1 , 1);
    var baseMonth1 = new Date(date.getFullYear(), currentR - 1, 1);
    
    var M3 = new Date(currentDate.setMonth(baseMonth1.getMonth() - 3));
    var M2 = new Date(currentDate.setMonth(baseMonth1.getMonth() - 4));
    var M1 = new Date(currentDate.setMonth(baseMonth1.getMonth() - 5));
    returnBaseMonth["historyMonth_M3"] = M3.getMonth()+1;
    returnBaseMonth["historyMonth_M2"] = M2.getMonth()+1;
    returnBaseMonth["historyMonth_M1"] = M1.getMonth()+1;
    return returnBaseMonth;
}
//function calLastForecastRollHeaders(currentR) {
//    debugger;
//    var returnLastRoll = {};
    
//     var date = new Date();
//    var currentDate = new Date(date.getFullYear(), date.getMonth() - 1, 1);
//    var baseMonth1 = new Date(date.getFullYear(), currentR - 1, 1);
//    R5 = new Date(currentDate.setMonth(baseMonth1.getMonth() + 1));
//    R4 = new Date(currentR);
//    R3 = new Date(currentDate.setMonth(baseMonth1.getMonth() - 1));
//    R2 = new Date(currentDate.setMonth(baseMonth1.getMonth() - 2));
//    R1 = new Date(currentDate.setMonth(baseMonth1.getMonth() - 3));
    
//    returnLastRoll["forecastRoll_R1"] = R1.getMonth() + 1;
//    returnLastRoll["forecastRoll_R2"] = R2.getMonth() + 1;
//    returnLastRoll["forecastRoll_R3"] = R3.getMonth() + 1;
//    returnLastRoll["forecastRoll_R4"] = R4.getMonth() + 1;
//    returnLastRoll["forecastRoll_R5"] = R5.getMonth() + 1;
   
//    return returnLastRoll;
//}

function calLastForecastRollHeaders(currentR) {
    debugger;
    var returnLastRoll = {};
    var date = new Date();
    var year = date.getFullYear();

    var baseMonth = new Date(year, currentR - 1, 1);

     R5 = new Date(year, baseMonth.getMonth() + 1, 1);
     R4 = new Date(year, baseMonth.getMonth(), 1);
     R3 = new Date(year, baseMonth.getMonth() - 1, 1);
     R2 = new Date(year, baseMonth.getMonth() - 2, 1);
     R1 = new Date(year, baseMonth.getMonth() - 3, 1);

    returnLastRoll["forecastRoll_R1"] = R1.getMonth() + 1;
    returnLastRoll["forecastRoll_R2"] = R2.getMonth() + 1;
    returnLastRoll["forecastRoll_R3"] = R3.getMonth() + 1;
    returnLastRoll["forecastRoll_R4"] = R4.getMonth() + 1;
    returnLastRoll["forecastRoll_R5"] = R5.getMonth() + 1;

    return returnLastRoll;
}

function calLastForecastMonthForRollHeaders() {

    var returnLastRollBaseMonth = {};
    var date = new Date();
    var currentDate = new Date(date.getFullYear(), date.getMonth() - 1, 1);

    M1 = new Date(currentDate.getFullYear(), R1.getMonth() + 2 , 1);
    M2 = new Date(currentDate.getFullYear(), R2.getMonth() + 2, 1);
    M3 = new Date(currentDate.getFullYear(), R3.getMonth() + 2, 1);
    M4 = new Date(currentDate.getFullYear(), R4.getMonth() + 2, 1);

    returnLastRollBaseMonth["forecastMonth_M1"] = M1.getMonth()+1;
    returnLastRollBaseMonth["forecastMonth_M2"] = M2.getMonth()+1;
    returnLastRollBaseMonth["forecastMonth_M3"] = M3.getMonth()+1;
    returnLastRollBaseMonth["forecastMonth_M4"] = M4.getMonth()+1;

    return returnLastRollBaseMonth;
}

function calLastForecastMonthForRollHeadersForcastPopup(R1, R2, R3, R4) {
    debugger;
    var returnLastRollBaseMonth = {};
    var year = new Date().getFullYear();

    // Convert month numbers to Date objects
    var dR1 = new Date(year, R1 - 1, 1);
    var dR2 = new Date(year, R2 - 1, 1);
    var dR3 = new Date(year, R3 - 1, 1);
    var dR4 = new Date(year, R4 - 1, 1);

    // Add +2 months
    var M1 = new Date(dR1.getFullYear(), dR1.getMonth() + 2, 1);
    var M2 = new Date(dR2.getFullYear(), dR2.getMonth() + 2, 1);
    var M3 = new Date(dR3.getFullYear(), dR3.getMonth() + 2, 1);
    var M4 = new Date(dR4.getFullYear(), dR4.getMonth() + 2, 1);

    returnLastRollBaseMonth["forecastMonth_M1"] = M1.getMonth() + 1;
    returnLastRollBaseMonth["forecastMonth_M2"] = M2.getMonth() + 1;
    returnLastRollBaseMonth["forecastMonth_M3"] = M3.getMonth() + 1;
    returnLastRollBaseMonth["forecastMonth_M4"] = M4.getMonth() + 1;

    return returnLastRollBaseMonth;
}

//function columnDispatchHistoryMonthHeaders(currentR) {

//    var returnBaseMonth = {};
//    var date = new Date();
//   // var baseMonth = new Date(date.getFullYear(), date.getMonth(currentR-2), 1);
//    var baseMonth = new Date(date.getFullYear(), currentR - 2, 1);

//    M3 = new Date(date.setMonth(baseMonth.getMonth() - 1));
//    M2 = new Date(date.setMonth(baseMonth.getMonth() - 2));
//    M1 = new Date(date.setMonth(baseMonth.getMonth() - 3));

//    returnBaseMonth["historyMonth_M3"] = M3.getMonth() + 1;
//    returnBaseMonth["historyMonth_M2"] = M2.getMonth() + 1;
//    returnBaseMonth["historyMonth_M1"] = M1.getMonth() + 1;
//    return returnBaseMonth;


//}

function columnDispatchHistoryMonthHeaders(currentR) {
    debugger;
    var returnBaseMonth = {};
    var date = new Date();
    var year = date.getFullYear();

    var baseMonth = new Date(year, currentR - 3, 1);

    var M3 = new Date(year, baseMonth.getMonth() - 1, 1);
    var M2 = new Date(year, baseMonth.getMonth() - 2, 1);
    var M1 = new Date(year, baseMonth.getMonth() - 3, 1);

    returnBaseMonth["historyMonth_M3"] = M3.getMonth() + 1;
    returnBaseMonth["historyMonth_M2"] = M2.getMonth() + 1;
    returnBaseMonth["historyMonth_M1"] = M1.getMonth() + 1;

    return returnBaseMonth;
}
//function lastForcastRColumnHeader(currentR) {
//    debugger;
//    var returnLastRoll = {};
//    var date = new Date();
//    var baseMonth1 = new Date(date.getFullYear(), currentR-1 , 1);
//   // R4 = new Date(currentR - 1);
//    R4 = new Date(date.setMonth(baseMonth1.getMonth() - 1));
//    R3 = new Date(date.setMonth(baseMonth1.getMonth() - 2));
//    R2 = new Date(date.setMonth(baseMonth1.getMonth() - 3));
//    R1 = new Date(date.setMonth(baseMonth1.getMonth() - 4));

//    returnLastRoll["forecastRoll_R1"] = R1.getMonth() + 1;
//    returnLastRoll["forecastRoll_R2"] = R2.getMonth() + 1;
//    returnLastRoll["forecastRoll_R3"] = R3.getMonth() + 1;
//    returnLastRoll["forecastRoll_R4"] = R4.getMonth() + 1;

//    return returnLastRoll;
//}

function lastForcastRColumnHeader(currentR) {
    var returnLastRoll = {};
    var year = new Date().getFullYear();

    var baseMonth1 = new Date(year, currentR - 1, 1);

    var R4 = new Date(year, baseMonth1.getMonth() - 1, 1);
    var R3 = new Date(year, baseMonth1.getMonth() - 2, 1);
    var R2 = new Date(year, baseMonth1.getMonth() - 3, 1);
    var R1 = new Date(year, baseMonth1.getMonth() - 4, 1);

    // Roll months
    returnLastRoll["forecastRoll_R1"] = R1.getMonth() + 1;
    returnLastRoll["forecastRoll_R2"] = R2.getMonth() + 1;
    returnLastRoll["forecastRoll_R3"] = R3.getMonth() + 1;
    returnLastRoll["forecastRoll_R4"] = R4.getMonth() + 1;

    // Forecast months (+2)
    returnLastRoll["forecastMonth_M1"] = new Date(year, R1.getMonth() + 2, 1).getMonth() + 1;
    returnLastRoll["forecastMonth_M2"] = new Date(year, R2.getMonth() + 2, 1).getMonth() + 1;
    returnLastRoll["forecastMonth_M3"] = new Date(year, R3.getMonth() + 2, 1).getMonth() + 1;
    returnLastRoll["forecastMonth_M4"] = new Date(year, R4.getMonth() + 2, 1).getMonth() + 1;

    return returnLastRoll;
}


$('.hover-row tr').on('click', function () // created for select html table row to highlight
{  
    // Remove .selected-row from all rows in this table
    $(this).closest('table').find('tr').removeClass('selected-row');

    // Add .selected-row to the clicked row
    $(this).addClass('selected-row');
});
function TrimAndNullify(obj) {

    if (typeof obj === "string") {
        const trimmed = obj.trim();
        return trimmed === "" ? null : trimmed;
    }
    if (Array.isArray(obj)) {
        return obj.map(item => TrimAndNullify(item));
    }
    if (obj && typeof obj === "object") {
        const result = {};
        for (const key in obj) {
            result[key] = TrimAndNullify(obj[key]);
        }
        return result;
    }
    return obj;
}

function SetIME()
{
    // Enable IME (Japanese example, you can change to "ko" or "zh")
    $(".IME-Active").each(function () {
        $(this).attr("lang", "ja");
    });


    $(".IME-Inactive").on("keypress", function () {
        // Hint to browser
        $(this).attr("lang", "en");
        $(this).attr("inputmode", "text");
    }).on("input", function () {
        // Allow only ASCII letters and digits
        this.value = this.value.replace(/[^A-Za-z0-9\s\.,!?@#\$%\^\&\*\-_=+]/g, "").toUpperCase();
    });

}
function IsEndDateTimeGreaterThanStartDateTimeByTenMin(startDateTime, endDateTime)
{
    debugger;
    var isValidDate = false;
    
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Common/IsEndDateTimeGreaterThanStartDateTimeByTenMin?startDateTime=" + startDateTime + "&endDateTime=" + endDateTime + "&minutes=" + StartAndEndDateTime, // hardcode for minutes default 5 min
        method: "POST",
        contentType: "application/json; charset=utf-8",

    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;

            isValidDate = response.isSuccess;            
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageCommon_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });
    return isValidDate;
}
// Button click event handled for all buttons to avoid double issue
(function () {

    if (window.globalClickProtectionLoaded) {
        return;
    }
    window.globalClickProtectionLoaded = true;

    document.addEventListener("click", function (e) {

        var button = e.target.closest(
            "button, input[type='button'], input[type='submit']"
        );

        if (!button) {
            return;
        }
        // Skip click timeout for specific buttons
        if (button.classList.contains("no-timeout")) {
            return;
        }

        if (button.dataset.clickLocked === "true") {
            e.preventDefault();
            e.stopImmediatePropagation();
            return false;
        }

        button.dataset.clickLocked = "true";

        setTimeout(function () {
            button.dataset.clickLocked = "false";
        }, 3000);

    }, true);

})();