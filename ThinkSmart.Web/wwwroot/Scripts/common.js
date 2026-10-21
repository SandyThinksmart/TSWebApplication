/* [2017/07/24] Girish badwaik.
    Following code is added to support IE 7 and 8 on Handy Terminal.
    IE 7 and 8 cannot parse 'trim' function of javascript.
    So, adding custom trim function.
*/
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}
/* End of Custom 'trim' function */

var closeWindow = function () {
    var win = window.open('', '_parent', '');
    win.close();
}

//var reposition = function (x, y) {        commented by Sagar B on 13/6/2017
//    window.moveTo(x, y);
//}

var resize = function (width, height) {
    window.resizeTo(width, height);
}

//--Parameters Sequence
////
//// controlId = controls'sId 
//// status = enable=true / disable=false
//// mandatory = isThisFieldMandarory true / false
function ChangeControlDisplay(controlId,
                              status,
                              classToRemove,
                              classToAdd,
                              flag) {

    if (status == true) {
        $(controlId).removeClass(classToRemove);
        $(controlId).addClass(classToAdd);
        $(controlId).prop("disabled", false);
 
        if(flag=="1")    
        $(controlId).val('');
    }
    else {
        $(controlId).removeClass(classToRemove);
        $(controlId).addClass(classToAdd);
        $(controlId).prop("disabled", true);
 
       if(flag=="1")
        $(controlId).val('');
    }
}



function ChangeComboDisplay(controlId, status) {

    ////-- Properties for Combo Fields --////
    if (status == true) {
      //  $(controlId).removeClass("dropListClass");
        $(controlId).addClass("dropListClass");
        $(controlId).prop("disabled", false);
        $(controlId).val('0');              // Remove comment because Radio button checked then Dropdown select index is 0.  (Pradeep 3-July-2015)
    }
    else {
       $(controlId).removeClass("dropListClass");
       //  $(controlId).addClass("dropListClass");
        $(controlId).prop("disabled", true);
        $(controlId).val('0');              // Remove comment because Radio button checked then Dropdown select index is 0.  (pradeep 3-July-2015)

    }
}

function ChangeButtonDisplay(controlId,status) {
    $(controlId).prop("disabled", status);

}


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
