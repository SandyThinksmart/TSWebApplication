function highlight($area, $element) {
    $area.find(".selected").removeClass("selected");
    $element.addClass("selected");
}

function ShowUnderDevMessage() {


    var UnderConMSG = $('#TxtUnderConstruction').val()

    alert(UnderConMSG);

    return false;
}

$(document).ready(function () {
  //  DisableMenu();
   // GetCSVMenuList();
    $.support.cors = true; // Special case for IE7. To get past weird errors.
    window.resizeTo(400, 800); /*window.resize(1920, 1080);*/
    moveTo(0, 0);
    var $mainMenu = $(".mainMenu");
    var $subMenu = $(".subMenu");


    /////////////////////////////////////// Show demo div by default  ///////////////////////////////////////
    highlight($mainMenu, $(".linkDefault").parent("td"));
    $subMenu.find("div").hide();
    $subMenu.find(".divDefault").show();


    ////////////////////////////////////// Handle menu click events ///////////////////////////////////////
    $mainMenu.click(function (event) {
        var target = event.target;
        var aId = "";
        var subMenuDivId = "";
        var $highlightTarget = "";

        if ((target.nodeName === "A" || target.nodeName === "SPAN")) {
            $highlightTarget = $(target).parent("td");
            aId = target.id; // should be "lnk<something>"
        }

        else if ((target.nodeName.toUpperCase() === "TD") && ($(target).has("span").length > 0 || $(target).has("a").length > 0)) {
            $highlightTarget = $(target);
            aId = $(target).has("span").length > 0 ? $(target).find("span").attr("id") : $(target).find("a").attr("id"); // should be "lnk<something>"
        }

        if (aId !== "" && $highlightTarget !== "") {

            subMenuDivId = aId.replace("lnk", "div"); // create "div<something>"

            if ($subMenu.has("#" + subMenuDivId).length > 0) {
                highlight($mainMenu, $highlightTarget);
                $subMenu.find("div").hide(); // hide all divs
                $subMenu.find("#" + subMenuDivId).show(); // show target menu div
            }

        }
    });

    function DisableMenu()
    {
        $("#HikinukiOracleToHost").css("pointer-events","none");
    
    }

    $("#ProcessPlanCompletion").click(function () {
        var getValue = getCookie("Cookieval");
        var myWindow = window.open($("#aspHTvalue").val() + "/Menu.aspx?ReadData=" + getValue + "&SiteURL=Transaction/ProdOrderCompletionScreen.aspx", "width=1940,height=1140", "Resizable = no");
    });

    function getCookie(cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    //////////////////////////////////// Handle sub-menu click events ///////////////////////////////////////
    var features = "left=0,top=0,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes,titlebar=no";
    $subMenu.click(function (event) {
        var target = event.target;
        var $highlightTarget = "";
        var url = "";

        if (target.nodeName === "SPAN" || target.nodeName === "A") {
            $highlightTarget = $(target).parent("td");
            url = target.nodeName === "SPAN" ? $(target).attr("url") : "";
        }
        else if ((target.nodeName.toUpperCase() === "TD") && ($(target).has("span").length > 0 || $(target).has("a").length > 0)) {
            $highlightTarget = $(target);
            url = $(target).has("span").length > 0 ? $(target).find("span").attr("url") : "";
        }

        if (url !== "" && $highlightTarget !== "") {
            highlight($subMenu, $highlightTarget);
            window.open(url, "_blank", features);
        }
    });

    //////////////////////////////////// Handle close button click event ///////////////////////////////////////
    $("#btnMainMenu").click(function (event) {
        window.location.href = $("#pmsmenuurl").val();
    });

    var bthUnderDevelopment = document.getElementsByClassName("UnderDevelopment");
    $(bthUnderDevelopment).click(function () {

    });


});

//Master page.
//Redirect master(Asp) on MachineEntry page.
const machineEntry = document.getElementById("lblMachineEntry");
if (machineEntry) {
    machineEntry.addEventListener("click", myFunction);
}
function myFunction() {
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/Masters/M_MachineEntry.aspx?coreSessionID=" + coreSessionID, "width=840px,height=400px", "Resizable = no");
}

//Redirect to master(Asp) on EmployeeEntry page.
const employeeEntry = document.getElementById("lblEmployeeEntry");
if (employeeEntry) {
    employeeEntry.addEventListener("click", myFunctionEmp);
}
function myFunctionEmp() {
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/Masters/M_EmployeeEntry.aspx?coreSessionID=" + coreSessionID, "width=1100px,height=455px", "Resizable = no");

}

//Redirect to master(Asp) on ProcessDefectMaster page.
const processDefectMaster = document.getElementById("lblProcessDefectMaster");
if (processDefectMaster) {
    processDefectMaster.addEventListener("click", myFunProcessDefect);
}
function myFunProcessDefect() {
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/Masters/M_ProcessDefectEntry.aspx?coreSessionID=" + coreSessionID, "width=835px,height=360px", "Resizable = no");

}

//Redirect to master(Asp) on Machine Group Master page.
const machineGroup = document.getElementById("lblMachineGroup");
if (machineGroup) {
    machineGroup.addEventListener("click", myMachineGroup);
}
function myMachineGroup() {
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/Masters/M_MachineGroupEntry.aspx?coreSessionID=" + coreSessionID, "width=835px,height=340px", "Resizable = no");

}

const lnkmainmenu = document.getElementById("lnkmainmunu");
if (lnkmainmenu) {
    lnkmainmenu.addEventListener("click", lnkmainmenu_Click);
}
function lnkmainmenu_Click() {
    var coreSessionID = $("#hdfLoginId").val();
    //var myWindow = window.open(PMSLOGINMENUURL + "/Menu/MenuPage/");
    window.location.href= PMSLOGINMENUURL + "/Menu/MenuPage/";

}

const lnkmainmenuMobile = document.getElementById("lnkmainmunuMobile");
if (lnkmainmenuMobile) {
    lnkmainmenuMobile.addEventListener("click", lnkmainmenuMobile_Click);
}
function lnkmainmenuMobile_Click() {
    var coreSessionID = $("#hdfLoginId").val();
    var pmsMenu = PMSLOGINMENUURL.substr(0, PMSLOGINMENUURL.indexOf('Menu/'));
    window.location.href = PMSLOGINMENUURL + "/Menu/PMSMenuMobilePage/";

}



//Handy Terminal Screen 

const elementFinalInspectionWithoutNyuko = document.getElementById("lnkFinalInspectionWithoutNyuko");
if (elementFinalInspectionWithoutNyuko) {
    elementFinalInspectionWithoutNyuko.addEventListener("click", myFunctionFinalInspectionWithoutNyuko);
}
function myFunctionFinalInspectionWithoutNyuko() {
    debugger;
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/HandyTerminal/FinalInspectionWithoutNyuko/FIWithoutNyuko.aspx?coreSessionID=" + coreSessionID + "&operation=create", "_blank", "resizable=no");
}
//
const elementFIBulk = document.getElementById("lnkFinalInspectionBulk");
if (elementFIBulk) {
    elementFIBulk.addEventListener("click", myFunctionFinalInspectionBulk);
}
function myFunctionFinalInspectionBulk() {
    debugger;
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/HandyTerminal/HTFIBulkWithoutNyuko/HTFIBulkWithoutNyuko.aspx?coreSessionID=" + coreSessionID + "&operation=create", "_blank", "resizable=no");
}
//
const elementFinalInspectionDelete = document.getElementById("lnkFinalInspectionDelete");
if (elementFinalInspectionDelete) {
    elementFinalInspectionDelete.addEventListener("click", myFunctionFinalInspectionDelete);
}
function myFunctionFinalInspectionDelete() {
   
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/HandyTerminal/FinalInspectionWithoutNyuko/FIWithoutNyuko.aspx?coreSessionID=" + coreSessionID + "&operation=delete", "_blank", "resizable=no");

}
//
const elementFinalInspectionSpark = document.getElementById("lnkFinalInspectionSpark");
if (elementFinalInspectionSpark) {
    elementFinalInspectionSpark.addEventListener("click", myFunctionFinalInspectionSpark);
}
function myFunctionFinalInspectionSpark() {
  
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/HandyTerminal/FinalInspectionSpark/HTFISpark.aspx?coreSessionID=" + coreSessionID + "&operation=entry", "_blank", "resizable=no");

}
//
const elementFinalInspectionWIPtagSplit = document.getElementById("lnkFinalInspectionWIPtagSplit");
if (elementFinalInspectionWIPtagSplit) {
    elementFinalInspectionWIPtagSplit.addEventListener("click", myFunctionFinalInspectionWIPtagSplit);
}
function myFunctionFinalInspectionWIPtagSplit() {
   
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/HandyTerminal/TabaDivision/HTTabaDivision.aspx?coreSessionID=" + coreSessionID + "&operation=create", "_blank", "resizable=no");

}

const elementFinalInspectionWIPtagCombining = document.getElementById("lnkFinalInspectionWIPtagCombining");
if (elementFinalInspectionWIPtagCombining) {
    elementFinalInspectionWIPtagCombining.addEventListener("click", myFunctionFinalInspectionWIPtagCombining);
}
function myFunctionFinalInspectionWIPtagCombining() {
   
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open(AspHTUrl + "/HandyTerminal/TabaMerge/HTTabaCombine.aspx?coreSessionID=" + coreSessionID + "&operation=create", "_blank", "resizable=no");

}

//General Process.
const lnkGPEntry = document.getElementById("lnkGeneralProcessEntry");
if (lnkGPEntry) {
    lnkGPEntry.addEventListener("click", lnkGeneralProcessEntry_Click);
}
function lnkGeneralProcessEntry_Click() {
   
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open("/GeneralDailyProcessEntry/GeneralDailyProcessEntry?coreSessionID=" + coreSessionID, "width=480px,height=700px", "Resizable = no");

}

const lnkGPEnd = document.getElementById("lnkGeneralProcessEnd");
if (lnkGPEnd) {
    lnkGPEnd.addEventListener("click", lnkGeneralProcessEnd_Click);
}
function lnkGeneralProcessEnd_Click() {
    
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open("/GeneralDailyProcessEnd/GeneralDailyProcessEnd?coreSessionID=" + coreSessionID, "width=480px,height=700px", "Resizable = no");

}

const lnkGPStartEnd = document.getElementById("lnkGeneralProcessStartEnd");
if (lnkGPStartEnd) {
    lnkGPStartEnd.addEventListener("click", lnkGeneralProcessStartEnd_Click);
}
function lnkGeneralProcessStartEnd_Click() {
    
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open("/GeneralDailyProcessStartEnd/GeneralDailyProcessStartEnd?coreSessionID=" + coreSessionID, "width=480px,height=700px", "Resizable = no");
}

const lnkGPCancel = document.getElementById("lnkGeneralProcessCancel");
if (lnkGPCancel) {
    lnkGPCancel.addEventListener("click", lnkGeneralProcessCancel_Click);
}
function lnkGeneralProcessCancel_Click() {
   
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open("/GeneralDailyProcessCancel/GeneralDailyProcessCancel?coreSessionID=" + coreSessionID, "width=480px,height=700px", "Resizable = no");

}

//Tensou Screen
const lnkWIPTensou = document.getElementById("lnkWIPTensouEntryEditDelete");
if (lnkWIPTensou) {
    lnkWIPTensou.addEventListener("click", lnkWIPTensou_Click);
}
function lnkWIPTensou_Click() {
    debugger;
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open("/WIPTensou/WIPTensouEntryEditDelete?coreSessionID" , "width=480px,height=700px", "Resizable = no");

}

const lnkWIPTensouAccept = document.getElementById("lnkWIPTensouAcceptanceEntryDelete");
if (lnkWIPTensouAccept) {
    lnkWIPTensouAccept.addEventListener("click", lnkWIPTensouAccept_Click);
}
function lnkWIPTensouAccept_Click() {
    debugger;
    var coreSessionID = $("#hdfLoginId").val();
    var myWindow = window.open("/WIPTensouAcceptance/WIPTensouAcceptanceEntryEditDelete?coreSessionID", "width=480px,height=700px", "Resizable = no");

}
function GetCSVMenuList() {


    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/CSVGeneration/GetGroupTitle",
        method: "GET",
        contentType: "application/json; charset=utf-8",

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
                    url: "/MenuMaster/GridSearch",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(response.message)

                }
                $.ajax(settings1)
                    .done(function (response1) {
                        if (response1.isSuccess) {
                            $("#divCSVPDFOutputGrid").html(response1.message);

                        }
                    }).fail(function (xhr, textStatus, errorThrown) {

                        return false;
                    });
            }



        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CSV_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
            return false;
        });


}