
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var returnVal = "returnVal";
var $ActiveID = "";
var rowLimitValue = "";
var heatTreatmentFlag = 0;
var drawingBarToBarFlag = 0;
var drawingCoilToBarFlag = 0;
var drawingCoilToCoilFlag = 0;
var straighteing1Flag = 0;
var straighteing2Flag = 0;
var chamferFlag = 0;
var ecFlag = 0;
var stamppingFlag = 0;
var hardnessTemperingFlag = 0;
var postHTFlag = 0;
var mechHardnessFlag = 0;
var mechTensileFlag = 0;
var finalInspectionFlag = 0;
var generalProcessFlag = 0;

var $callFrom = this;
$(document).ready(function () {    
    $("#txtChuubanCom").focus();
    $("#activeTabID").val("tabHeatTreatment");
    var targetValue = "titleHeatTreatment"
    $ActiveID = targetValue;
    if ($ActiveID == "titleHeatTreatment") {
       InitSortPopup($ActiveID);
    }
    Mask();
    setDecimalFormats();
    setProdConditionName();
    setMatlConditionName();
    setProcessKubun();
    setDecision();
    setProcConditionKubun();
    $("#btnExportCSV").attr("DISABLED", "DISABLED");

    //Hide Ok button from sort popup and make close button as Ok button
    var buttonOK = document.getElementById("btnOk");
    buttonOK.style.display = 'none';
    document.getElementById("btnSortClose").value = "OK";
    var buttonClose = document.getElementById("btnSortClose");
    buttonClose.style.width = '70px';
    

});

function setDecimalFormats() {
    $("#txtDiaMin,#txtDiaMax,#txtProductDimWCom,#txtMaterialDimMin,#txtMaterialDimMax,#txtMaterialDimWCom").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3);
    });
    $("#txtLengthMin,#txtLengthMax").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4,2);
    });
   
}


function Mask() {
    $("#txtAllocDateStart").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtAllocDateEnd").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    
    //HardnessTempering
    $("#txtInspectionStartDate,#txtInspectionEndDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
    $("#txtFurnaceInStartDateTimeQ,#txtFurnaceInEndDateTimeQ,#txtFurnaceOutStartDateTimeQ,#txtFurnaceOutEndDateTimeQ,#txtFurnaceInStartDateTimeT,#txtFurnaceInEndDateTimeT,#txtFurnaceOutStartDatetimeT,#txtFurnaceOutEndDatetimeT").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" }); 

    //HeatTreatment
    $("#txtFurnanceInStartDateTime,#txtFurnanceInEndDateTime,#txtFurnanceOutStartDateTime,#txtFurnanceOutEndDateTime").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    //HikinukiCoilToBar
    $("#txtStartDateTimeDR1,#txtStartDateTimeDR2,#txtEndDateTimeDR1,#txtEndDateTimeDR2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    //HikinukiCoilToBar
    $("#txtStartDateTimeDRA1,#txtStartDateTimeDRA2,#txtEndDateTimeDRA1,#txtEndDateTimeDRA2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    //ColdDrawingWireDrawing
    $("#txtStartDateTimeDW1,#txtStartDateTimeDW2,#txtEndDateTimeDW1,#txtEndDateTimeDW2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });

    //Straightening1
    $("#txtStartDateTimeSTR1_1,#txtStartDateTimeSTR1_2,#txtEndDateTimeSTR1_1,#txtEndDateTimeSTR1_2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //Straightening2
    $("#txtStartDateTimeSTR2_1,#txtStartDateTimeSTR2_2,#txtEndDateTimeSTR2_1,#txtEndDateTimeSTR2_2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //Chamfer
    $("#txtStartDateTimeCHA1,#txtStartDateTimeCHA2,#txtEndDateTimeCHA1,#txtEndDateTimeCHA2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
    //stampping
    $("#txtStartDateTimeST1,#txtStartDateTimeST2,#txtEndDateTimeST1,#txtEndDateTimeST2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });    
    //EC
    $("#txtStartDateTimeEC1,#txtStartDateTimeEC2,#txtEndDateTimeEC1,#txtEndDateTimeEC2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });    
    //PostHT
    $("#txtInspectionStartDateInter,#txtInspectionEndDateInter").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });    
    //MechHardness
    $("#txtInspectionStartDateHard,#txtInspectionEndDateHard").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });  
    //MechTensile
    $("#txtInspectionStartDateTen,#txtInspectionEndDateTen").mask("9999/99/99", { placeholder: "YYYY/MM/DD" }); 
    //FI
    $("#txtInspectionStartDateFI,#txtInspectionEndDateFI,#txtSparkFIStartDate,#txtSparkFIEndDate").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" }); 
   
    //General Daily Process
    $("#txtStartDateGP1,#txtStartDateGP2,#txtEndDateGp1,#txtEndDateGp2").mask("9999/99/99 99:99", { placeholder: "YYYY/MM/DD hh:mm" });
   // $("#txtStartDateGP1,#txtStartDateGP2,#txtEndDateGp1,#txtEndDateGp2").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });

}

$(".csvDisable").change(function () {
    debugger;
    $("#btnExportCSV").attr("DISABLED", "DISABLED");
});

var element = document.getElementById("tabIndex");
element.addEventListener('click', function (e) {

    var targetValue = $("#" + e.target.id).data("title");
    if (targetValue != "" && targetValue != undefined) {
        $("#activeTabID").val(e.target.id);
        HideHeaderTab();
        $('#' + targetValue).show();
        $ActiveID = targetValue;
    }

    if ($ActiveID == "titleHeatTreatment") {
        InitSortPopup($ActiveID);
        if (heatTreatmentFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {
           
            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }

        
    }   
    if ($ActiveID == "titleDrawingBarToBar") {
        InitSortPopup($ActiveID);
        if (drawingBarToBarFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleDrawingCoilToBar") {
        InitSortPopup($ActiveID);
        if (drawingCoilToBarFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleDrawingCoilToCoil") {
        InitSortPopup($ActiveID);
        if (drawingCoilToCoilFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleStraightening1") {
        InitSortPopup($ActiveID);
        if (straighteing1Flag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleStraightening2") {
        InitSortPopup($ActiveID);
        if (straighteing2Flag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleChamfer") {
        InitSortPopup($ActiveID);
        if (chamferFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleEC") {
        InitSortPopup($ActiveID);
        if (ecFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleStamping") {
        InitSortPopup($ActiveID);
        if (stamppingFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleHardnessTempering") {
        InitSortPopup($ActiveID);
        if (hardnessTemperingFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titlePostHTHardness") {
        InitSortPopup($ActiveID);
        if (postHTFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleMIHardness") {
        InitSortPopup($ActiveID);
        if (mechHardnessFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }
    if ($ActiveID == "titleMechTensile") {
        InitSortPopup($ActiveID);
        if (mechTensileFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
        
    }
    if ($ActiveID == "titleFinalInspection") {
        InitSortPopup($ActiveID);
        if (finalInspectionFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }

    if ($ActiveID == "titleGeneralDailyProcess") {
        InitSortPopup($ActiveID);
        if (generalProcessFlag == 1) {
            $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
        }
        else {

            $("#btnExportCSV").attr("DISABLED", "DISABLED");
        }
    }

});

function setProdConditionName() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetConditionName",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindProdConditionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindProdConditionDropdown(jsonResult) {
    $("#ddlProdCondition").html("");
    $("#ddlProdCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProdCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));       
    });
}
function setMatlConditionName() {
    var ajaxTime = new Date().getTime();

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetConditionName",
        method: "GET",
        contentType: "application/json; charset=utf-8",
        //data: "{jsonData : " + jsonData + "}"
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindMatlConditionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_STA_EDIT_DELETE_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
function bindMatlConditionDropdown(jsonResult) {
    $("#ddlMatlCondition").html("");
    $("#ddlMatlCondition").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMatlCondition").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });
}

function HideHeaderTab() {
    $('#titleHeatTreatment').hide();    
    $('#titleDrawingBarToBar').hide();
    $('#titleDrawingCoilToBar').hide();
    $('#titleDrawingCoilToCoil').hide();
    $('#titleStraightening1').hide();
    $('#titleStraightening2').hide();
    $('#titleChamfer').hide();
    $('#titleEC').hide();
    $('#titleStamping').hide();
    $('#titleHardnessTempering').hide();
    $('#titlePostHTHardness').hide();
    $('#titleMIHardness').hide();
    $('#titleMechTensile').hide();
    $('#titleFinalInspection').hide();
    $('#titleGeneralDailyProcess').hide();
}


$(document).on('click', '#btnSort', function () {    

    if ($ActiveID == "titleHeatTreatment") {      
        $("#SortPopUpModal").modal('show');
    }    
    if ($ActiveID == "titleDrawingBarToBar") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleDrawingCoilToBar") {       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleDrawingCoilToCoil") {      
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleStraightening1") {
       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleStraightening2") {
        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleChamfer") {
        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleEC") {
      
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleStamping") {
      
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleHardnessTempering") {
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titlePostHTHardness") {
        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleMIHardness") {
       
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleMechTensile") {
        
        $("#SortPopUpModal").modal('show');
    }
    if ($ActiveID == "titleFinalInspection") {
       
        $("#SortPopUpModal").modal('show');
    }

    if ($ActiveID == "titleGeneralDailyProcess") {
        
        $("#SortPopUpModal").modal('show');
    }
})


function EnableDisableSortButtons() {
    var available = $("#lstSortFields option").length;
    var selected = $("#lstSelectedSortFields option").length;
    if (available > 0) {
        $("#SortPopUpModal #btnAdd").prop('disabled', false);
    }
    else { 
        $("#SortPopUpModal #btnAdd").prop('disabled', true);
    }

    if (selected > 0) {
        $("#SortPopUpModal #btnRemove").prop('disabled', false);
    }
    else {
        $("#SortPopUpModal #btnRemove").prop('disabled', true);
    }
}

function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
}

function btnSortClose_Click() {

    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}

function btnSortAdd_Click() {
     
    var selectedColumns = $("#lstSortFields").val();
    if (selectedColumns.length > 0) {
        $("#lstSortFields option:selected").each(function () {
            var $this = $(this);
            if ($this.length) {
                var selText = $this.text();
                console.log(selText);
                $('#lstSelectedSortFields').append($("<option></option>")
                    .attr("value", $this.val())
                    .text(selText + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
                $this.remove();
            }
        });
    }
    //else {
    //    alert("Please select the sort field to be added.");
    //}
    EnableDisableSortButtons();
}

function btnSortRemove_Click() {
    var selectedColumns = $("#lstSelectedSortFields").val();
    if (selectedColumns.length > 0) {
        var selectedColumns = $("#lstSelectedSortFields").val();
        if (selectedColumns.length > 0) {
            $("#lstSelectedSortFields option:selected").each(function () {
                var $this = $(this);
                if ($this.length) {
                    var selText = $this.text().split(' ');
                    console.log(selText);
                    selText.splice(-1);
                    $('#lstSortFields').append($("<option></option>")
                        .attr("value", $this.val())
                        .text(selText.join(' ')));
                    $this.remove();
                }
            });
        }
    }
    //else {
    //    alert("Please select the sort field to be added.");
    //}
    EnableDisableSortButtons();
}

function btnSortReset_Click() {
    if ($ActiveID == "titleHeatTreatment") {
        InitSortPopup($ActiveID);
       
    }    
    if ($ActiveID == "titleDrawingBarToBar") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleDrawingCoilToBar") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleDrawingCoilToCoil") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleStraightening1") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleStraightening2") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleChamfer") {
        InitSortPopup($ActiveID);
        
    }
    if ($ActiveID == "titleEC") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleStamping") {
        InitSortPopup($ActiveID);
       
    }
    if ($ActiveID == "titleHardnessTempering") {
        InitSortPopup($ActiveID);

    }
    if ($ActiveID == "titlePostHTHardness") {
        InitSortPopup($ActiveID);
      
    }
    if ($ActiveID == "titleMIHardness") {
        InitSortPopup($ActiveID);
      
    }
    if ($ActiveID == "titleMechTensile") {
        InitSortPopup($ActiveID);
        
    }
    if ($ActiveID == "titleFinalInspection") {
        InitSortPopup($ActiveID);
        
    }
    if ($ActiveID == "titleGeneralDailyProcess") {
        InitSortPopup($ActiveID);

    }
   
}

function InitSortPopup(activeTabID) {
     
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/DailyProcessAnalysisWithCSV/InitializeSortPopup?activeTabID=" + activeTabID,
        method: "Post",
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
}

//Initialize btn click

$("#btnInitialize").click(function () {
    WARNING = "warning";
    ERROR = "error";
    SUCCESS = "success";
    returnVal = "returnVal";
    //$ActiveID = "";
    rowLimitValue = "";
    clearSearchFields();
    removeTabClass();
    $("#txtChuubanCom").focus();
    heatTreatmentFlag = 0;
    drawingBarToBarFlag = 0;
    drawingCoilToBarFlag = 0;
    drawingCoilToCoilFlag = 0;
    straighteing1Flag = 0;
    straighteing2Flag = 0;
    chamferFlag = 0;
    ecFlag = 0;
    stamppingFlag = 0;
    hardnessTemperingFlag = 0;
    postHTFlag = 0;
    mechHardnessFlag = 0;
    mechTensileFlag = 0;
    finalInspectionFlag = 0;
    generalProcessFlag = 0;
    var list = [];
    
    //if ($ActiveID == "titleHardnessTempering") {
       
        clearTable($("#tableHardnessTempering"), "", "GridNormalRow"); 
            var settings = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/HardnessTemperingDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(list)
            }
            $.ajax(settings)
                .done(function (response) {
                    $("#divHardnessTemperingDailyProcessGridMain").html(response.message);
                })


       

    //}
   // if ($ActiveID == "titleHeatTreatment") {
       
        clearTable($("#tableHeatTreatment"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/HeatTreatmentGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divHeatTreatmentDailyProcessGridMain").html(response.message);
            })
    //}
    //if ($ActiveID == "titleDrawingBarToBar") {
       
        clearTable($("#tableDrawingBarToBar"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessDrawingDGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divDailyProcessAnalysisDrawingDGridMain").html(response.message);
            })
   // }
    //if ($ActiveID == "titleDrawingCoilToBar") {
        
        clearTable($("#tableCoilToBar"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/HikinukiCoilToBarDailyProcessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divHikinukiCoilToBarDailyProcessGridMain").html(response.message);
            })
   // }
    //if ($ActiveID == "titleDrawingCoilToCoil") {
       
        clearTable($("#tableCoilToCoil"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessColdDrawingWGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divDailyProcessAnalysisColdDrawingWGridMain").html(response.message);
            })
  //  }
   // if ($ActiveID == "titleStraightening1") {
        
        clearTable($("#tableStraightening1"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divStraighteningDailyProcessGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleStraightening2") {
        
        clearTable($("#tableStraightening2"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid2",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divStraighteningDailyProcessGridMain2").html(response.message);
            })
    //}
   // if ($ActiveID == "titleChamfer") {
       
        clearTable($("#tableChamfer"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/ChamfferDailyProcessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divChamfferDailyProcessGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleEC") {
       
        clearTable($("#tableEC"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessECDetailsGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divDailyProcessAnalysisECGridMain").html(response.message);
            })
    //}
   // if ($ActiveID == "titleStamping") {
       
        clearTable($("#tableStampping"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessStamppingReportGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divDailyProcessAnalysisStamppingGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titlePostHTHardness") {
       
        clearTable($("#tableQATestHardness"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/QATestHardnessDailyProcessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divQATestHardnessDailyProcessGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleMIHardness") {
        
        clearTable($("#tableMIHardness"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessMechHardnessDetailsGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divMiHardnessDailyProcessGridMain").html(response.message);
            })
   // }
   // if ($ActiveID == "titleMechTensile") {
       
        clearTable($("#tableMechTensile"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessMechTensileDetailsGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divMechTensileDailyProcessGridMain").html(response.message);
            })
   // }
    //if ($ActiveID == "titleFinalInspection") {
        
        clearTable($("#tableFinalInspection"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GetDailyProcessFinalInspectionDetailsGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divFinalInspectionDailyProcessGridMain").html(response.message);
            })
   // }
    //if ($ActiveID == "titleGeneralDailyProcess") {

        clearTable($("#tableGeneralDailyProcess"), "", "GridNormalRow");
        var settings = {
            async: true,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: "/DailyProcessAnalysisWithCSV/GeneralProcessDailyProcessGrid",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(list)
        }
        $.ajax(settings)
            .done(function (response) {
                $("#divGeneralProcessDailyProcessGridMain").html(response.message);
            })
    //}
});
function clearTable($table, trId, trClass) {
   
    // If class is present, use class to delete, else use ID.
    if (trClass != "") {
        $table.find("tr." + trClass).remove();
    }
    else if (trId != "") {
        $table.find("tr[id$='" + trId + "']").remove();
    }
}
function clearSearchFields() {
    $('#rowLimitValue').val('200');
    $('#txtChuubanCom').val('');
    $('#txtGoubanCom').val('');
    $('#txtManufacturingSpecificationNo1Com').val('');
    $('#txtManufacturingSpecificationNo2Com').val('');
    $('#txtVER').val('');
    $('#txtCustomerCOm').val('');
    $('#txtProdNameCustCom').val('');
    $('#txtInternalNameCom').val('');
    $('#txtMadeShapeRptCom').val('');
    $('#txtDiaMin').val('');
    $('#txtDiaMax').val('');    
    $('#txtProductDimWCom').val('');
    $('#txtProductDimOtherCom').val('');
    $('#txtLengthMin').val('');
    $('#txtLengthMax').val('');
    $('#txtProdLengthOther').val('');
    //
    $('#ddlMatlTypeCBK option:eq(0)').prop('selected', true);
    

    $('#txtMaterialNameMakerCom').val('');
    $('#txtMaterialShapeCom').val('');
    $('#txtMaterialDimMin').val('');
    $('#txtMaterialDimMax').val('');
    $('#txtMaterialDimWCom').val('');
    $('#txtMaterialDimOtherCom').val('');
    $('#txtMatlLengthMin').val('');
    $('#txtMatlLengthMax').val('');
    //
    $('#txtChargeCom').val('');
    $('#txtAllocDateStart').val('');
    $('#txtAllocDateEnd').val('');

    //HardnessTempering
    $('#txtLocationCodeHRD').val('');
    $('#txtFurnaceNumHRD').val('');
    $('#txtQuenchingMachine').val('');
    $('#txtTemperingMachine').val('');
    $('#txtOperatorQuenching').val('');
    $('#txtFurnaceInStartDateTimeQ').val('');
    $('#txtFurnaceInEndDateTimeQ').val('');
    $('#txtFurnaceOutStartDateTimeQ').val('');    
    $('#txtFurnaceOutEndDateTimeQ').val('');
    $('#txtOperatorTempering').val('');
    $('#txtFurnaceInStartDateTimeT').val('');
    $('#txtFurnaceInEndDateTimeT').val('');
    $('#txtFurnaceOutStartDatetimeT').val('');
    $('#txtFurnaceOutEndDatetimeT').val('');
    $('#txtInspectionStartDate').val('');
    $('#txtInspectionEndDate').val('');
    $('#txtInspectorHRD').val('');    
    $('#ddlInspectionDecesion option:eq(0)').prop('selected', true);


    //HeatTreatment
    $('#txtLocationHT').val('');
    $('#txtMachineHT').val('');
    $('#txtFurnaceNumHT').val('');
    //
    $('#txtPatternNoHT').val('');
    $('#txtFurnanceInStartDateTime').val('');
    $('#txtFurnanceInEndDateTime').val('');
    $('#txtSetUPConfByOperator').val('');
    $('#txtLoadingConfByOperator').val('');
    $('#txtFurnanceOutStartDateTime').val('');

    $('#txtFurnanceOutEndDateTime').val('');
    $('#txtOffloadedByOperator').val('');
    $('#txtOffloadConfByOperator').val('');
    $('#txtMetallicLablNoRM').val('');
    $('#txtMetallicLablNoCNo').val('');

    //HikinukiBarToBar
    $('#txtMachineDR').val('');
    $('#txtLocationDR').val('');
    $('#txtStartDateTimeDR1').val('');
    
    $('#txtStartDateTimeDR2').val('');
    $('#txtStartOperatorDR').val('');
    $('#txtMetallicLabelNoDR').val('');
    $('#txtDieNoDR').val('');
    $('#ddlProcConditionDR option:eq(0)').prop('selected', true);
    $('#txtEndDateTimeDR1').val('');
    $('#txtEndDateTimeDR2').val('');
    $('#txtEndOperatorDR').val('');     
    $('#ddlDRDecision option:eq(0)').prop('selected', true);

    //HikinukiCoilToBar
    $('#txtMachineDRA').val('');
    $('#txtLocationDRA').val('');
    $('#txtStartDateTimeDRA1').val('');
    $('#txtStartDateTimeDRA2').val('');
    $('#txtStartOperatorDRA').val('');
    $('#txtFurnaceNumDRA').val('');
    $('#txtMetallicLabelNoDRA').val('');    
    $('#txtDieNoDRA').val('');
    $('#txtEndDateTimeDRA1').val('');
    $('#txtEndDateTimeDRA2').val('');
    $('#txtEndOperatorDRA').val('');
    $('#ddlDRADecision option:eq(0)').prop('selected', true);

    //ColdDrawing(Coil TO Coil)

    $('#txtMachineCodeDW').val('');
    $('#txtLocationCodeDW').val('');
    $('#txtStartDateTimeDW1').val('');
    $('#txtStartDateTimeDW2').val('');
    $('#txtStartOperatorDW').val('');
    $('#txtRollingNumberDW').val('');
    $('#txtFurnaceNumDW').val('');    
    $('#txtDieNoDW').val('');
    $('#txtEndDateTimeDW1').val('');
    $('#txtEndDateTimeDW2').val('');
    $('#txtEndOperatorDW').val('');
    $('#ddlDWDecision option:eq(0)').prop('selected', true);


    //STRAIGHTENING 1
    $('#txtMachineSTR').val('');
    $('#txtLocationSTR').val('');
    $('#txtStartDateTimeSTR1_1').val('');
    $('#txtStartDateTimeSTR1_2').val('');
    $('#txtStartOperatorSTR').val('');
    $('#txtMetallicLabelNoSTR').val('');
    $('#txtFurnaceNumSTR').val('');
    $('#txtEndDateTimeSTR1_1').val('');
    $('#txtEndDateTimeSTR1_2').val('');
    $('#txtEndOperatorSTR').val('');    
    $('#ddlSTR1Decision option:eq(0)').prop('selected', true);


    //STRAIGHTENING 2
    $('#txtMachineSTR2').val('');
    $('#txtLocationSTR2').val('');
    $('#txtStartDateTimeSTR2_1').val('');
    $('#txtStartDateTimeSTR2_2').val('');
    $('#txtStartOperatorSTR2').val('');
    $('#txtMetallicLabelNoSTR2').val('');
    $('#txtFurnaceNumSTR2').val('');
    $('#txtEndDateTimeSTR2_1').val('');
    $('#txtEndDateTimeSTR2_2').val('');
    $('#txtEndOperatorSTR2').val('');    
    $('#ddlSTR2Decision option:eq(0)').prop('selected', true);

    //CHAMFERING
    $('#txtMachineCHA').val('');
    $('#txtLocationCHA').val('');
    $('#txtStartDateTimeCHA1').val('');
    $('#txtStartDateTimeCHA2').val('');
    $('#txtStartOperatorCHA').val('');
    $('#txtMetallicLabelNoCHA').val('');
    $('#txtFurnaceNumCHA').val('');
    $('#txtEndDateTimeCHA1').val('');
    $('#txtEndDateTimeCHA2').val('');
    $('#txtEndOperatorCHA').val('');
    $('#ddlChamferDecision option:eq(0)').prop('selected', true);


    //EC
    $('#txtMachineEC').val('');
    $('#txtLocationEC').val('');
    $('#txtStartDateTimeEC1').val('');
    $('#txtStartDateTimeEC2').val('');
    $('#txtStartOperatorEC').val('');
    $('#txtMetallicLabelNoEC').val('');
    $('#txtFurnaceNumEC').val('');
    $('#txtEndDateTimeEC1').val('');
    $('#txtEndDateTimeEC2').val('');
    $('#txtEndOperatorEC').val('');    
    $('#ddlECDecision option:eq(0)').prop('selected', true);


    //STAMPING
    $('#txtMachineST').val('');
    $('#txtLocationST').val('');
    $('#txtStartDateTimeST1').val('');
    $('#txtStartDateTimeST2').val('');
    $('#txtStartOperatorST').val('');
    $('#txtMetallicLabelNoST').val('');
    $('#txtEndDateTimeST1').val('');
    $('#txtEndDateTimeST2').val('');
    $('#txtEndOperatorST').val('');        
    $('#ddlStamppingDecision option:eq(0)').prop('selected', true);

    //INTERNAL_QUALITY
    $('#txtMachineInter').val('');
    $('#txtRptLocationInter').val('');
    $('#txtInspectionStartDateInter').val('');
    $('#txtInspectionEndDateInter').val('');
    $('#txtInspectorInter').val('');    
    $('#ddlProcConditionInter option:eq(0)').prop('selected', true);
    $('#txtPatternNoInter').val('');
    $('#txtFurnaceNumInter').val('');
    $('#ddlPostHTDecision option:eq(0)').prop('selected', true);


    //MI_HARDNESS
    $('#txtRptLocationHard').val('');
    $('#txtInspectionStartDateHard').val('');
    $('#txtInspectionEndDateHard').val('');
    $('#txtInspectorHard').val('');
    $('#txtFurnaceNumHard').val('');    
    $('#ddlMIHardnessDecision option:eq(0)').prop('selected', true);


    //MI_TENSILE_STRENGTH
    $('#txtRptLocationTen').val('');
    $('#txtInspectionStartDateTen').val('');
    $('#txtInspectionEndDateTen').val('');
    $('#txtInspectorTen').val('');
    $('#txtFurnaceNumTen').val('');    
    $('#ddlMITensileDecision option:eq(0)').prop('selected', true);

    //FINAL INSPECTION
    $('#txtLocationCode').val('');
    $('#txtInspectionStartDateFI').val('');
    $('#txtInspectionEndDateFI').val('');
    $('#txtInspectorFI').val('');
    $('#txtSparkFIStartDate').val('');
    $('#txtSparkFIEndDate').val('');
    $('#txtSparkFIOperator').val('');    
    $('#ddlFICombinedDecision option:eq(0)').prop('selected', true);

    // General Daily Process
    $('#txtMachineGP').val('');
    $('#txtLocationGP').val('');
    $('#txtStartDateGP1').val('');
    $('#txtStartDateGP2').val('');
    $('#txtStartOperatorGP').val('');
    $('#txtEndDateGp1').val('');
    $('#txtEndDateGp2').val('');
    $('#txtEndOperatorGP').val('');
    $('#ddlDecisionGP option:eq(0)').prop('selected', true);


    setProdConditionName();
    setMatlConditionName();
    setProcessKubun();
    setDecision();
    setProcConditionKubun();

    $("#btnExportCSV").attr("DISABLED", "DISABLED");

}
//function formatStartEndDate(date) {
//    debugger;
//    var d = new Date(date),
//        month = '' + (d.getMonth() + 1),
//        day = '' + d.getDate(),
//        year = d.getFullYear();

//    if (month.length < 2)
//        month = '0' + month;
//    if (day.length < 2)
//        day = '0' + day;

//    return [year, month, day].join('/');
//}
function formatStartEndDate(date) {
    debugger;

    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear(),
        hour = '' + d.getHours(),
        minute = '' + d.getMinutes();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;
    if (hour.length < 2)
        hour = '0' + hour;
    if (minute.length < 2)
        minute = '0' + minute;

    // If original value contains time
    if (date.indexOf(' ') > -1) {
        return [year, month, day].join('/') + ' ' + hour + ':' + minute;
    }

    // Date only
    return [year, month, day].join('/');
}


$(document).on('click', '#btnExportCSV', function () {
    var errorMessage = "";
    var checkFocusField = "";
    var issearchField = false;  

    var sortFilter = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            sortFilter.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });


    if ($ActiveID == "titleHeatTreatment") {   

        DownloadHeatTreatmentCSV(sortFilter);
       

    }    
    if ($ActiveID == "titleDrawingBarToBar") {

        DownloadHikinukiBarToBarCSV(sortFilter);

    }
    if ($ActiveID == "titleDrawingCoilToBar") {
       
        DownloadHikinukiCoilToBarCSV(sortFilter);
             
    }
    if ($ActiveID == "titleDrawingCoilToCoil") {  
        
        DownloadColdDrawingWireDrawingCSV(sortFilter);               

    }
    if ($ActiveID == "titleStraightening1") {

        DownloadStraighteningCSV(sortFilter);

    }
    if ($ActiveID == "titleStraightening2") {

        DownloadStraightening2CSV(sortFilter);

    }

    if ($ActiveID == "titleChamfer") {
        DownloadChamfferCSV(sortFilter);
           
    }
    if ($ActiveID == "titleEC") {

        DownloadECCSV(sortFilter);

    }
    if ($ActiveID == "titleStamping") {

        DownloadStamppingCSV(sortFilter);

    }
    if ($ActiveID == "titleHardnessTempering") {

        DownloadHardnessTemperingCSV(sortFilter);

    }
    if ($ActiveID == "titlePostHTHardness") {

        DownloadQATestHardnessCSV(sortFilter);


    }
    if ($ActiveID == "titleMIHardness") {

        DownloadMechHardnessCSV(sortFilter);

    }
    if ($ActiveID == "titleMechTensile") {

        DownloadMechTensileCSV(sortFilter);

    }
    if ($ActiveID == "titleFinalInspection") {

        DownloadFinalInspectionCSV(sortFilter);

    }

    if ($ActiveID == "titleGeneralDailyProcess") {

        DownloadGeneralDailyProcessCSV(sortFilter);

    }

})    
   

$("#btnSearch").click(function () {
    debugger;
    var errorMessage = "";
    var checkFocusField = "";
    var issearchField = false;  
    
    var sortFilter = [];
    $("#lstSelectedSortFields option").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.text().split(' ');
            var selValue = $this.val();
            var order = selText.splice(-1) == $("#rdoSortAsc").attr("data-text") ? 0 : 1;
            sortFilter.push({ FieldName: selValue, DisplayName: selText.join(' '), SortOrder: order });
        }
    });

    if ($("#txtChuubanCom").val() != "" || $("#txtGoubanCom").val() != "" || $("#txtManufacturingSpecificationNo1Com").val() != "" || $("#txtManufacturingSpecificationNo2Com").val() != "" ||
        $("#txtVER").val() != "" || $("#txtCustomerCOm").val() != "" || $("#txtProdNameCustCom").val() != "" || $("#txtInternalNameCom").val() != "" || $("#txtMadeShapeRptCom").val() != "" ||
        $("#txtDiaMin").val() != "" || $("#txtDiaMax").val() != "" || $("#txtProductDimWCom").val() != "" || $("#txtProductDimOtherCom").val() != "" || $("#txtLengthMin").val() != "" ||
        $("#txtLengthMax").val() != "" || $("#txtProdLengthOther").val() != "" || $("#ddlProdCondition").val() != "0" || $("#ddlMatlTypeCBK").val() != "0" || $("#txtMaterialNameMakerCom").val() != "" ||
        $("#txtMaterialShapeCom").val() != "" || $("#txtMaterialDimMin").val() != "" || $("#txtMaterialDimMax").val() != "" || $("#txtMaterialDimWCom").val() != "" || $("#txtMaterialDimOtherCom").val() != "" ||
        $("#txtMatlLengthMin").val() != "" || $("#txtMatlLengthMax").val() != "" || $("#ddlMatlCondition").val() != "0" || $("#txtChargeCom").val() != "" || $("#txtAllocDateStart").val() != "" || $("#txtAllocDateEnd").val() != "") {

        issearchField = true;
    }
    else {
        issearchField = false;
    }

    if ($("#txtDiaMin").val().trim() == "" && $("#txtDiaMax").val().trim() != "") {
        errorMessage = $.trim($("#lblErrorMessageE_DimDStartRequired").text());
        checkFocusField = 'txtDiaMin'
    }

    if ($("#txtLengthMin").val().trim() == "" && $("#txtLengthMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtLengthMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_LengthStartRequired").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_LengthStartRequired").text());
        }
    }
    if ($("#txtMaterialDimMin").val().trim() == "" && $("#txtMaterialDimMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtMaterialDimMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_MatlDimDStartRequired").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_MatlDimDStartRequired").text());
        }
    }
    if ($("#txtMatlLengthMin").val().trim() == "" && $("#txtMatlLengthMax").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtMatlLengthMin';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_MatlLengthStartRequired").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_MatlLengthStartRequired").text());
        }
    }
    if ($("#txtAllocDateStart").val().trim() == "" && $("#txtAllocDateEnd").val().trim() != "") {
        if (checkFocusField == "") {
            checkFocusField = 'txtAllocDateStart';
        }
        if (errorMessage == "") {
            errorMessage = $.trim($("#lblErrorMessageE_ActAllocDateStartRequired").text());
        } else {
            errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ActAllocDateStartRequired").text());
        }
    }

    if ($("#rowLimitValue").val().trim() == "") {
        errorMessage = $.trim($("#valMsgMaxRowRequired").text());
        checkFocusField = 'rowLimitValue'
        
    }



    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }   
  
    
    if (errorMessage == "") {
        debugger;
        checkFocusField = "";   

        var allocDateStart = document.getElementById("txtAllocDateStart").value;
        var allocDateEnd = document.getElementById("txtAllocDateEnd").value;               
        
        if (allocDateStart != "") {
            if (!validateOnlyDateFormat(allocDateStart)) {
                    errorMessage = $("#lblErrorMessageE_ActAllocStartDateInvalid").text().trim();
                    checkFocusField = "txtAllocDateStart";
                }
            }

        if (allocDateEnd != "" ) {
            if (!validateOnlyDateFormat(allocDateEnd)) {
                    if (errorMessage == "") {
                        errorMessage = $("#lblErrorMessageE_ActAllocEndDateInvalid").text().trim();
                    } else {
                        errorMessage += "<br>" + $("#lblErrorMessageE_ActAllocEndDateInvalid").text().trim();
                    }
                    if (checkFocusField == "") {
                        checkFocusField = "txtAllocDateEnd";
                    }

                }
            }
    
      
    }
    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    } 

    if (errorMessage == "") {
        checkFocusField = "";
        if (allocDateStart != "" && allocDateEnd != "") {
            var txtallocDateStart = formatStartEndDate(allocDateStart);
            var txtallocDateEnd = formatStartEndDate(allocDateEnd);
            if (txtallocDateEnd < txtallocDateStart) {
                errorMessage = $("#lblErrorMessageE_ActAllocEndDateValidate").text().trim();
                checkFocusField = "txtAllocDateEnd";                
            }
        }
    }
    if (errorMessage != "") {
        showMessageDialog(errorMessage, checkFocusField, WARNING);
        return false;
    }   

    var rowLimit = $("#rowLimitValue").val();
    var maxLimit = $("#maxLimit").val();
    if (parseInt(rowLimit) > parseInt(maxLimit)) {
        var message = $("#valMsgDataRowsMaxLimitExceed").text().trim();
        focusFieldID = 'rowLimitValue';
        showMessageDialog(message, focusFieldID, WARNING);
        return false;
    }    
    
    //Hardness Tempering Validations
   
    if ($ActiveID == "titleHardnessTempering") {
        if (issearchField == true || $("#txtLocationCodeHRD").val() != "" || $("#txtFurnaceNumHRD").val() != "" || $("#txtQuenchingMachine").val() != "" || $("#txtTemperingMachine").val() != "" || $("#txtOperatorQuenching").val() != "" || $("#txtFurnaceInStartDateTimeQ").val() != "" ||
            $("#txtFurnaceInEndDateTimeQ").val() != "" || $("#txtFurnaceOutStartDateTimeQ").val() != "" || $("#txtFurnaceOutEndDateTimeQ").val() != "" || $("#txtOperatorTempering").val() != "" || $("#txtFurnaceInStartDateTimeT").val() != "" || $("#txtFurnaceInEndDateTimeT").val() != "" ||
            $("#txtFurnaceOutStartDatetimeT").val() != "" || $("#txtFurnaceOutEndDatetimeT").val() != "" || $("#txtInspectionStartDate").val() != "" || $("#txtInspectionEndDate").val() != "" || $("#txtInspectorHRD").val() != "" || $("#ddlInspectionDecesion").val() != "0") {

            if ($("#txtFurnaceInStartDateTimeQ").val().trim() == "" && $("#txtFurnaceInEndDateTimeQ").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_QFurnaceInDateStart_Required").text());
                checkFocusField = "txtFurnaceInStartDateTimeQ";
            }

            if ($("#txtFurnaceOutStartDateTimeQ").val().trim() == "" && $("#txtFurnaceOutEndDateTimeQ").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtFurnaceOutStartDateTimeQ';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_QFurnaceOutDateStart_Required").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_QFurnaceOutDateStart_Required").text());
                }

            }

            if ($("#txtFurnaceInStartDateTimeT").val().trim() == "" && $("#txtFurnaceInEndDateTimeT").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtFurnaceInStartDateTimeT';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_TFurnaceInDateStart_Required").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_TFurnaceInDateStart_Required").text());
                }

            }

            if ($("#txtFurnaceOutStartDatetimeT").val().trim() == "" && $("#txtFurnaceOutEndDatetimeT").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtFurnaceOutStartDatetimeT';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_TFurnaceOutDateStart_Required").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_TFurnaceOutDateStart_Required").text());
                }

            }

            if ($("#txtInspectionStartDate").val().trim() == "" && $("#txtInspectionEndDate").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtInspectionStartDate';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_InspStartDate_Required").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_InspStartDate_Required").text());
                }

            }

            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }



            if (errorMessage == "") {
                checkFocusField = "";
                var txtFurnaceInStartDateTimeQ = document.getElementById("txtFurnaceInStartDateTimeQ").value;
                var txtFurnaceInEndDateTimeQ = document.getElementById("txtFurnaceInEndDateTimeQ").value;

                var txtFurnaceOutStartDateTimeQ = document.getElementById("txtFurnaceOutStartDateTimeQ").value;
                var txtFurnaceOutEndDateTimeQ = document.getElementById("txtFurnaceOutEndDateTimeQ").value;

                var txtFurnaceInStartDateTimeT = document.getElementById("txtFurnaceInStartDateTimeT").value;
                var txtFurnaceInEndDateTimeT = document.getElementById("txtFurnaceInEndDateTimeT").value;

                var txtFurnaceOutStartDatetimeT = document.getElementById("txtFurnaceOutStartDatetimeT").value;
                var txtFurnaceOutEndDatetimeT = document.getElementById("txtFurnaceOutEndDatetimeT").value;

                var txtInspectionStartDate = document.getElementById("txtInspectionStartDate").value;
                var txtInspectionEndDate = document.getElementById("txtInspectionEndDate").value;

                if (txtFurnaceInStartDateTimeQ != "") {
                    if (!validateDateFormat(txtFurnaceInStartDateTimeQ)) {
                        errorMessage = $("#lblErrorMessageE_QFurnaceInStartDate_Invalid").text().trim();
                        checkFocusField = "txtFurnaceInStartDateTimeQ";
                    }
                }

                if (txtFurnaceInEndDateTimeQ != "") {
                    if (!validateDateFormat(txtFurnaceInEndDateTimeQ)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_QFurnaceInEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_QFurnaceInEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceInEndDateTimeQ";
                        }

                    }
                }


                if (txtFurnaceOutStartDateTimeQ != "") {
                    if (!validateDateFormat(txtFurnaceOutStartDateTimeQ)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_QFurnaceOutStartDate_Invalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_QFurnaceOutStartDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceOutStartDateTimeQ";
                        }

                    }


                }
                if (txtFurnaceOutEndDateTimeQ != "") {
                    if (!validateDateFormat(txtFurnaceOutEndDateTimeQ)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_QFurnaceOutEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_QFurnaceOutEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceOutEndDateTimeQ";
                        }

                    }
                }

                if (txtFurnaceInStartDateTimeT != "") {
                    if (!validateDateFormat(txtFurnaceInStartDateTimeT)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_TFurnaceInStartDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_TFurnaceInStartDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceInStartDateTimeT";
                        }

                    }
                }

                if (txtFurnaceInEndDateTimeT != "") {
                    if (!validateDateFormat(txtFurnaceInEndDateTimeT)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_TFurnaceInEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_TFurnaceInEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceInEndDateTimeT";
                        }

                    }
                }

                if (txtFurnaceOutStartDatetimeT != "") {
                    if (!validateDateFormat(txtFurnaceOutStartDatetimeT)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_TFurnaceOutStartDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_TFurnaceOutStartDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceOutStartDatetimeT";
                        }

                    }
                }

                if (txtFurnaceOutEndDatetimeT != "") {
                    if (!validateDateFormat(txtFurnaceOutEndDatetimeT)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_TFurnaceOutEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_TFurnaceOutEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnaceOutEndDatetimeT";
                        }

                    }
                }



                if (txtInspectionStartDate != "") {
                    if (!validateOnlyDateFormat(txtInspectionStartDate)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_InspStartDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_InspStartDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtInspectionStartDate";
                        }

                    }
                }

                if (txtInspectionEndDate != "") {
                    if (!validateOnlyDateFormat(txtInspectionEndDate)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtInspectionEndDate";
                        }

                    }
                }


                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtFurnaceInStartDateTimeQ != "" && txtFurnaceInEndDateTimeQ != "") {
                        var furnaceInStartDateTimeQ = formatStartEndDate(txtFurnaceInStartDateTimeQ);
                        var furnaceInEndDateTimeQ = formatStartEndDate(txtFurnaceInEndDateTimeQ);

                        if (furnaceInEndDateTimeQ < furnaceInStartDateTimeQ) {
                            errorMessage = $("#lblErrorMessageE_QFurnaceInEndDate_Validate").text().trim();
                            checkFocusField = "txtFurnaceInEndDateTimeQ";
                        }
                    }
                    if (txtFurnaceOutStartDateTimeQ != "" && txtFurnaceOutEndDateTimeQ != "") {
                        var furnaceOutStartDateTimeQ = formatStartEndDate(txtFurnaceOutStartDateTimeQ);
                        var furnaceOutEndDateTimeQ = formatStartEndDate(txtFurnaceOutEndDateTimeQ);

                        if (furnaceOutEndDateTimeQ < furnaceOutStartDateTimeQ) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_QFurnaceOutEndDate_Validate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_QFurnaceOutEndDate_Validate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtFurnaceOutEndDateTimeQ";
                            }

                        }
                    }


                    if (txtFurnaceInStartDateTimeT != "" && txtFurnaceInEndDateTimeT != "") {
                        var furnaceInStartDateTimeT = formatStartEndDate(txtFurnaceInStartDateTimeT);
                        var furnaceInEndDateTimeT = formatStartEndDate(txtFurnaceInEndDateTimeT);

                        if (furnaceInEndDateTimeT < furnaceInStartDateTimeT) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_TFurnaceInEndDate_Validate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_TFurnaceInEndDate_Validate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtFurnaceInEndDateTimeT";
                            }

                        }
                    }

                    if (txtFurnaceOutStartDatetimeT != "" && txtFurnaceOutEndDatetimeT != "") {
                        var furnaceOutStartDatetimeT = formatStartEndDate(txtFurnaceOutStartDatetimeT);
                        var furnaceOutEndDatetimeT = formatStartEndDate(txtFurnaceOutEndDatetimeT);

                        if (furnaceOutEndDatetimeT < furnaceOutStartDatetimeT) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_TFurnaceOutEndDate_Validate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_TFurnaceOutEndDate_Validate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtFurnaceOutEndDatetimeT";
                            }

                        }
                    }

                    if (txtInspectionStartDate != "" && txtInspectionEndDate != "") {
                        var inspectionStartDate = formatStartEndDate(txtInspectionStartDate);
                        var inspectionEndDate = formatStartEndDate(txtInspectionEndDate);

                        if (inspectionEndDate < inspectionStartDate) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_InspEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_InspEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtInspectionEndDate";
                            }

                        }
                    }

                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountHardnessTempering(sortFilter);

                }
            }  


            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Heat Treatment Validations
    
    if ($ActiveID == "titleHeatTreatment") {
        if (issearchField == true || $("#txtLocationHT").val() != "" || $("#txtMachineHT").val() != "" || $("#txtFurnaceNumHT").val() != "" || $("#ddlProcConditionHT").val() != "0" || $("#txtPatternNoHT").val() != "" || $("#txtFurnanceInStartDateTime").val() != "" ||
            $("#txtFurnanceInEndDateTime").val() != "" || $("#txtSetUPConfByOperator").val() != "" || $("#txtLoadingConfByOperator").val() != "" || $("#txtFurnanceOutStartDateTime").val() != "" || $("#txtFurnanceOutEndDateTime").val() != "" || $("#txtOffloadedByOperator").val() != "" ||
            $("#txtOffloadConfByOperator").val() != "" || $("#txtMetallicLablNoRM").val() != "" || $("#txtMetallicLablNoCNo").val() != "") {


            if ($("#txtFurnanceInStartDateTime").val().trim() == "" && $("#txtFurnanceInEndDateTime").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_FurnaceInStartDateTime_Required").text());
                checkFocusField = "txtFurnanceInStartDateTime";
            }

            if ($("#txtFurnanceOutStartDateTime").val().trim() == "" && $("#txtFurnanceOutEndDateTime").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtFurnanceOutStartDateTime';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_FurnaceOutStartDateTime_Required").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_FurnaceOutStartDateTime_Required").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }





            if (errorMessage == "") {
                checkFocusField = "";
                var txtFurnanceInStartDateTime = document.getElementById("txtFurnanceInStartDateTime").value;
                var txtFurnanceInEndDateTime = document.getElementById("txtFurnanceInEndDateTime").value;
                var txtFurnanceOutStartDateTime = document.getElementById("txtFurnanceOutStartDateTime").value;
                var txtFurnanceOutEndDateTime = document.getElementById("txtFurnanceOutEndDateTime").value;              


                if (txtFurnanceInStartDateTime != "") {
                    if (!validateDateFormat(txtFurnanceInStartDateTime)) {
                        errorMessage = $("#lblErrorMessageE_FurnaceInStartDate_Invalid").text().trim();
                        checkFocusField = "txtFurnanceInStartDateTime";
                    }
                }

                if (txtFurnanceInEndDateTime != "") {
                    if (!validateDateFormat(txtFurnanceInEndDateTime)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_FurnaceInEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_FurnaceInEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnanceInEndDateTime";
                        }

                    }
                }
                if (txtFurnanceOutStartDateTime != "") {
                    if (!validateDateFormat(txtFurnanceOutStartDateTime)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_FurnaceOutStartDate_Invalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_FurnaceOutStartDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnanceOutStartDateTime";
                        }

                    }


                }
                if (txtFurnanceOutEndDateTime != "") {
                    if (!validateDateFormat(txtFurnanceOutEndDateTime)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_FurnaceOutEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_FurnaceOutEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtFurnanceOutEndDateTime";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtFurnanceInStartDateTime != "" && txtFurnanceInEndDateTime != "") {
                        var furnanceInStartDateTime = formatStartEndDate(txtFurnanceInStartDateTime);
                        var furnanceInEndDateTime = formatStartEndDate(txtFurnanceInEndDateTime);

                        if (furnanceInEndDateTime < furnanceInStartDateTime) {
                            errorMessage = $("#lblErrorMessageE_FurnaceInEndDate_Validate").text().trim();
                            checkFocusField = "txtFurnanceInEndDateTime";
                        }
                    }
                    if (txtFurnanceOutStartDateTime != "" && txtFurnanceOutEndDateTime != "") {
                        var furnanceOutStartDateTime = formatStartEndDate(txtFurnanceOutStartDateTime);
                        var furnanceOutEndDateTime = formatStartEndDate(txtFurnanceOutEndDateTime);

                        if (furnanceOutEndDateTime < furnanceOutStartDateTime) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_FurnaceOutEndDate_Validate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_FurnaceOutEndDate_Validate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtFurnanceOutEndDateTime";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountHeatTreatment(sortFilter);

                }
            }  


        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Drawing Bar To Bar Validations
    if ($ActiveID == "titleDrawingBarToBar") {
        if (issearchField == true || $("#txtMachineDR").val() != "" || $("#txtLocationDR").val() != "" || $("#txtStartDateTimeDR1").val() != "" || $("#txtStartDateTimeDR2").val() != "" || $("#txtStartOperatorDR").val() != "" || $("#txtMetallicLabelNoDR").val() != "" ||
            $("#txtDieNoDR").val() != "" || $("#ddlProcConditionDR").val() != "0" || $("#txtEndDateTimeDR1").val() != "" || $("#txtEndDateTimeDR2").val() != "" || $("#txtEndOperatorDR").val() != "" || $("#ddlDRDecision").val() != "0") {

            if ($("#txtStartDateTimeDR1").val().trim() == "" && $("#txtStartDateTimeDR2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeDR1";
            }

            if ($("#txtEndDateTimeDR1").val().trim() == "" && $("#txtEndDateTimeDR2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeDR1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }


            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeDR1 = document.getElementById("txtStartDateTimeDR1").value;
                var txtStartDateTimeDR2 = document.getElementById("txtStartDateTimeDR2").value;
                var txtEndDateTimeDR1 = document.getElementById("txtEndDateTimeDR1").value;
                var txtEndDateTimeDR2 = document.getElementById("txtEndDateTimeDR2").value;


               
                if (txtStartDateTimeDR1 != "") {
                    if (!validateDateFormat(txtStartDateTimeDR1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeDR1";
                    }
                }

                if (txtStartDateTimeDR2 != "") {
                    if (!validateDateFormat(txtStartDateTimeDR2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeDR2";
                        }

                    }
                }
                if (txtEndDateTimeDR1 != "") {
                    if (!validateDateFormat(txtEndDateTimeDR1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeDR1";
                        }

                    }


                }
                if (txtEndDateTimeDR2 != "") {
                    if (!validateDateFormat(txtEndDateTimeDR2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeDR2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeDR1 != "" && txtStartDateTimeDR2 != "") {
                        var startDateTimeDR1 = formatStartEndDate(txtStartDateTimeDR1);
                        var startDateTimeDR2 = formatStartEndDate(txtStartDateTimeDR2);

                        if (startDateTimeDR2 < startDateTimeDR1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeDR2";
                        }
                    }
                    if (txtEndDateTimeDR1 != "" && txtEndDateTimeDR2 != "") {
                        var endDateTimeDR1 = formatStartEndDate(txtEndDateTimeDR1);
                        var endDateTimeDR2 = formatStartEndDate(txtEndDateTimeDR2);
                        if (endDateTimeDR2 < endDateTimeDR1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeDR2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountHikinukiBarToBar(sortFilter)
                }
            }     
            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Drawing Coil To Bar Validations
    if ($ActiveID == "titleDrawingCoilToBar") {
        if (issearchField == true || $("#txtMachineDRA").val() != "" || $("#txtLocationDRA").val() != "" || $("#txtStartDateTimeDRA1").val() != "" || $("#txtStartDateTimeDRA2").val() != "" || $("#txtStartOperatorDRA").val() != "" || $("#txtFurnaceNumDRA").val() != "" ||
            $("#txtMetallicLabelNoDRA").val() != "" || $("#txtDieNoDRA").val() != "" || $("#txtEndDateTimeDRA1").val() != "" || $("#txtEndDateTimeDRA2").val() != "" || $("#txtEndOperatorDRA").val() != "" || $("#ddlDRADecision").val() != "0") {

            if ($("#txtStartDateTimeDRA1").val().trim() == "" && $("#txtStartDateTimeDRA2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeDRA1";
            }

            if ($("#txtEndDateTimeDRA1").val().trim() == "" && $("#txtEndDateTimeDRA2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeDRA1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }


            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeDRA1 = document.getElementById("txtStartDateTimeDRA1").value;
                var txtStartDateTimeDRA2 = document.getElementById("txtStartDateTimeDRA2").value;
                var txtEndDateTimeDRA1 = document.getElementById("txtEndDateTimeDRA1").value;
                var txtEndDateTimeDRA2 = document.getElementById("txtEndDateTimeDRA2").value;

                if (txtStartDateTimeDRA1 != "") {
                    if (!validateDateFormat(txtStartDateTimeDRA1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeDRA1";
                    }
                }

                if (txtStartDateTimeDRA2 != "") {
                    if (!validateDateFormat(txtStartDateTimeDRA2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeDRA2";
                        }

                    }
                }
                if (txtEndDateTimeDRA1 != "") {
                    if (!validateDateFormat(txtEndDateTimeDRA1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeDRA1";
                        }

                    }


                }
                if (txtEndDateTimeDRA2 != "") {
                    if (!validateDateFormat(txtEndDateTimeDRA2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeDRA2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeDRA1 != "" && txtStartDateTimeDRA2 != "") {
                        var startDateTimeDRA1 = formatStartEndDate(txtStartDateTimeDRA1);
                        var startDateTimeDRA2 = formatStartEndDate(txtStartDateTimeDRA2);

                        if (startDateTimeDRA2 < startDateTimeDRA1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeDRA2";
                        }
                    }
                    if (txtEndDateTimeDRA1 != "" && txtEndDateTimeDRA2 != "") {
                        var endDateTimeDRA1 = formatStartEndDate(txtEndDateTimeDRA1);
                        var endDateTimeDRA2 = formatStartEndDate(txtEndDateTimeDRA2);

                        if (endDateTimeDRA2 < endDateTimeDRA1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeDRA2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountHikinukiCoilToBar(sortFilter)
                }
            }     

            
        }
        else {
             bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Drawing Coil To Coil Validations
    if ($ActiveID == "titleDrawingCoilToCoil") {
        if (issearchField == true || $("#txtMachineCodeDW").val() != "" || $("#txtLocationCodeDW").val() != "" || $("#txtStartDateTimeDW1").val() != "" || $("#txtStartDateTimeDW2").val() != "" || $("#txtStartOperatorDW").val() != "" || $("#txtRollingNumberDW").val() != "" ||
            $("#txtFurnaceNumDW").val() != "" || $("#txtDieNoDW").val() != "" || $("#txtEndDateTimeDW1").val() != "" || $("#txtEndDateTimeDW2").val() != "" || $("#txtEndOperatorDW").val() != "" || $("#ddlDWDecision").val() != "0") {

            if ($("#txtStartDateTimeDW1").val().trim() == "" && $("#txtStartDateTimeDW2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeDW1";
            }

            if ($("#txtEndDateTimeDW1").val().trim() == "" && $("#txtEndDateTimeDW2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeDW1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }

            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeDW1 = document.getElementById("txtStartDateTimeDW1").value;
                var txtStartDateTimeDW2 = document.getElementById("txtStartDateTimeDW2").value;
                var txtEndDateTimeDW1 = document.getElementById("txtEndDateTimeDW1").value;
                var txtEndDateTimeDW2 = document.getElementById("txtEndDateTimeDW2").value;

                if (txtStartDateTimeDW1 != "") {
                    if (!validateDateFormat(txtStartDateTimeDW1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeDW1";
                    }
                }

                if (txtStartDateTimeDW2 != "") {
                    if (!validateDateFormat(txtStartDateTimeDW2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeDW2";
                        }

                    }
                }
                if (txtEndDateTimeDW1 != "") {
                    if (!validateDateFormat(txtEndDateTimeDW1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeDW1";
                        }

                    }


                }
                if (txtEndDateTimeDW2 != "") {
                    if (!validateDateFormat(txtEndDateTimeDW2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeDW2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeDW1 != "" && txtStartDateTimeDW2 != "") {
                        var startDateTimeDW1 = formatStartEndDate(txtStartDateTimeDW1);
                        var startDateTimeDW2 = formatStartEndDate(txtStartDateTimeDW2);

                        if (startDateTimeDW2 < startDateTimeDW1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeDW2";
                        }
                    }
                    if (txtEndDateTimeDW1 != "" && txtEndDateTimeDW2 != "") {
                        var endDateTimeDW1 = formatStartEndDate(txtEndDateTimeDW1);
                        var endDateTimeDW2 = formatStartEndDate(txtEndDateTimeDW2);

                        if (endDateTimeDW2 < endDateTimeDW1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeDW2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountColdDrawingWireDrawing(sortFilter)
                }
            }           
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Straightening1 Validations
    if ($ActiveID == "titleStraightening1") {
        if (issearchField == true || $("#txtMachineSTR").val() != "" || $("#txtLocationSTR").val() != "" || $("#txtStartDateTimeSTR1_1").val() != "" || $("#txtStartDateTimeSTR1_2").val() != "" || $("#txtStartOperatorSTR").val() != "" || $("#txtMetallicLabelNoSTR").val() != "" ||
            $("#txtFurnaceNumSTR").val() != "" || $("#txtEndDateTimeSTR1_1").val() != "" || $("#txtEndDateTimeSTR1_2").val() != "" || $("#txtEndOperatorSTR").val() != "" || $("#ddlSTR1Decision").val() != "0") {

            if ($("#txtStartDateTimeSTR1_1").val().trim() == "" && $("#txtStartDateTimeSTR1_2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeSTR1_1";
            }

            if ($("#txtEndDateTimeSTR1_1").val().trim() == "" && $("#txtEndDateTimeSTR1_2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeSTR1_1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }

            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeSTR1_1 = document.getElementById("txtStartDateTimeSTR1_1").value;
                var txtStartDateTimeSTR1_2 = document.getElementById("txtStartDateTimeSTR1_2").value;
                var txtEndDateTimeSTR1_1 = document.getElementById("txtEndDateTimeSTR1_1").value;
                var txtEndDateTimeSTR1_2 = document.getElementById("txtEndDateTimeSTR1_2").value;               

                if (txtStartDateTimeSTR1_1 != "") {
                    if (!validateDateFormat(txtStartDateTimeSTR1_1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeSTR1_1";
                    }
                }

                if (txtStartDateTimeSTR1_2 != "") {
                    if (!validateDateFormat(txtStartDateTimeSTR1_2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeSTR1_2";
                        }

                    }
                }
                if (txtEndDateTimeSTR1_1 != "") {
                    if (!validateDateFormat(txtEndDateTimeSTR1_1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeSTR1_1";
                        }

                    }


                }
                if (txtEndDateTimeSTR1_2 != "") {
                    if (!validateDateFormat(txtEndDateTimeSTR1_2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeSTR1_2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeSTR1_1 != "" && txtStartDateTimeSTR1_2 != "") {
                        var startDateTimeSTR1_1 = formatStartEndDate(txtStartDateTimeSTR1_1); 
                        var startDateTimeSTR1_2 = formatStartEndDate(txtStartDateTimeSTR1_2);

                        if (startDateTimeSTR1_2 < startDateTimeSTR1_1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeSTR1_2";
                        }
                    }
                    if (txtEndDateTimeSTR1_1 != "" && txtEndDateTimeSTR1_2 != "") {
                        var endDateTimeSTR1_1 = formatStartEndDate(txtEndDateTimeSTR1_1);
                        var endDateTimeSTR1_2 = formatStartEndDate(txtEndDateTimeSTR1_2);

                        if (endDateTimeSTR1_2 < endDateTimeSTR1_1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeSTR1_2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountStraightening(sortFilter)
                }
            }    
            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Straightening2 Validations
    if ($ActiveID == "titleStraightening2") {
        if (issearchField == true || $("#txtMachineSTR2").val() != "" || $("#txtLocationSTR2").val() != "" || $("#txtStartDateTimeSTR2_1").val() != "" || $("#txtStartDateTimeSTR2_2").val() != "" || $("#txtStartOperatorSTR2").val() != "" || $("#txtMetallicLabelNoSTR2").val() != "" ||
            $("#txtFurnaceNumSTR2").val() != "" || $("#txtEndDateTimeSTR2_1").val() != "" || $("#txtEndDateTimeSTR2_2").val() != "" || $("#txtEndOperatorSTR2").val() != "" || $("#ddlSTR2Decision").val() != "0") {

            if ($("#txtStartDateTimeSTR2_1").val().trim() == "" && $("#txtStartDateTimeSTR2_2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeSTR2_1";
            }

            if ($("#txtEndDateTimeSTR2_1").val().trim() == "" && $("#txtEndDateTimeSTR2_2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeSTR2_1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }

            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeSTR2_1 = document.getElementById("txtStartDateTimeSTR2_1").value;
                var txtStartDateTimeSTR2_2 = document.getElementById("txtStartDateTimeSTR2_2").value;
                var txtEndDateTimeSTR2_1 = document.getElementById("txtEndDateTimeSTR2_1").value;
                var txtEndDateTimeSTR2_2 = document.getElementById("txtEndDateTimeSTR2_2").value;

                if (txtStartDateTimeSTR2_1 != "") {
                    if (!validateDateFormat(txtStartDateTimeSTR2_1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeSTR2_1";
                    }
                }

                if (txtStartDateTimeSTR2_2 != "") {
                    if (!validateDateFormat(txtStartDateTimeSTR2_2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeSTR2_2";
                        }

                    }
                }
                if (txtEndDateTimeSTR2_1 != "") {
                    if (!validateDateFormat(txtEndDateTimeSTR2_1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeSTR2_1";
                        }

                    }


                }
                if (txtEndDateTimeSTR2_2 != "") {
                    if (!validateDateFormat(txtEndDateTimeSTR2_2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeSTR2_2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeSTR2_1 != "" && txtStartDateTimeSTR2_2 != "") {
                        var startDateTimeSTR2_1 = formatStartEndDate(txtStartDateTimeSTR2_1);
                        var startDateTimeSTR2_2 = formatStartEndDate(txtStartDateTimeSTR2_2);

                        if (startDateTimeSTR2_2 < startDateTimeSTR2_1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeSTR2_2";
                        }
                    }
                    if (txtEndDateTimeSTR2_1 != "" && txtEndDateTimeSTR2_2 != "") {
                        var endDateTimeSTR2_1 = formatStartEndDate(txtEndDateTimeSTR2_1);
                        var endDateTimeSTR2_2 = formatStartEndDate(txtEndDateTimeSTR2_2);

                        if (endDateTimeSTR2_2 < endDateTimeSTR2_1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeSTR2_2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountStraightening2(sortFilter)
                }
            }            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Chamfer Validations
    if ($ActiveID == "titleChamfer") {
        if (issearchField == true || $("#txtMachineCHA").val() != "" || $("#txtLocationCHA").val() != "" || $("#txtStartDateTimeCHA1").val() != "" || $("#txtStartDateTimeCHA2").val() != "" || $("#txtStartOperatorCHA").val() != "" || $("#txtMetallicLabelNoCHA").val() != "" ||
            $("#txtFurnaceNumCHA").val() != "" || $("#txtEndDateTimeCHA1").val() != "" || $("#txtEndDateTimeCHA2").val() != "" || $("#txtEndOperatorCHA").val() != "" || $("#ddlChamferDecision").val() != "0") {

            if ($("#txtStartDateTimeCHA1").val().trim() == "" && $("#txtStartDateTimeCHA2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeCHA1";
            }

            if ($("#txtEndDateTimeCHA1").val().trim() == "" && $("#txtEndDateTimeCHA2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeCHA1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }

            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeCHA1 = document.getElementById("txtStartDateTimeCHA1").value;
                var txtStartDateTimeCHA2 = document.getElementById("txtStartDateTimeCHA2").value;
                var txtEndDateTimeCHA1 = document.getElementById("txtEndDateTimeCHA1").value;
                var txtEndDateTimeCHA2 = document.getElementById("txtEndDateTimeCHA2").value;

                if (txtStartDateTimeCHA1 != "") {
                    if (!validateDateFormat(txtStartDateTimeCHA1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeCHA1";
                    }
                }

                if (txtStartDateTimeCHA2 != "") {
                    if (!validateDateFormat(txtStartDateTimeCHA2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeCHA2";
                        }

                    }
                }
                if (txtEndDateTimeCHA1 != "") {
                    if (!validateDateFormat(txtEndDateTimeCHA1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeCHA1";
                        }

                    }


                }
                if (txtEndDateTimeCHA2 != "") {
                    if (!validateDateFormat(txtEndDateTimeCHA2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeCHA2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeCHA1 != "" && txtStartDateTimeCHA2 != "") {
                        var startDateTimeCHA1 = formatStartEndDate(txtStartDateTimeCHA1);
                        var startDateTimeCHA2 = formatStartEndDate(txtStartDateTimeCHA2);

                        if (startDateTimeCHA2 < startDateTimeCHA1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeCHA2";
                        }
                    }
                    if (txtEndDateTimeCHA1 != "" && txtEndDateTimeCHA2 != "") {
                        var endDateTimeCHA1 = formatStartEndDate(txtEndDateTimeCHA1);
                        var endDateTimeCHA2 = formatStartEndDate(txtEndDateTimeCHA2);

                        if (endDateTimeCHA2 < endDateTimeCHA1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeCHA2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }



                else {
                    GetRowCountChamffer(sortFilter)
                }



            }
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });

        }

    }  
   
    //EC Validations
    if ($ActiveID == "titleEC") {       
        if (issearchField == true || $("#txtMachineEC").val() != "" || $("#txtLocationEC").val() != "" || $("#txtStartDateTimeEC1").val() != "" || $("#txtStartDateTimeEC2").val() != "" || $("#txtStartOperatorEC").val() != "" || $("#txtMetallicLabelNoEC").val() != "" ||
            $("#txtFurnaceNumEC").val() != "" || $("#txtEndDateTimeEC1").val() != "" || $("#txtEndDateTimeEC2").val() != "" || $("#txtEndOperatorEC").val() != "" || $("#ddlECDecision").val() != "0") {

            if ($("#txtStartDateTimeEC1").val().trim() == "" && $("#txtStartDateTimeEC2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeEC1";
            }

            if ($("#txtEndDateTimeEC1").val().trim() == "" && $("#txtEndDateTimeEC2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeEC1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }




            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeEC1 = document.getElementById("txtStartDateTimeEC1").value;
                var txtStartDateTimeEC2 = document.getElementById("txtStartDateTimeEC2").value;
                var txtEndDateTimeEC1 = document.getElementById("txtEndDateTimeEC1").value;
                var txtEndDateTimeEC2 = document.getElementById("txtEndDateTimeEC2").value;

                if (txtStartDateTimeEC1 != "") {
                    if (!validateDateFormat(txtStartDateTimeEC1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeEC1";
                    }
                }

                if (txtStartDateTimeEC2 != "") {
                    if (!validateDateFormat(txtStartDateTimeEC2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeEC2";
                        }

                    }
                }
                if (txtEndDateTimeEC1 != "") {
                    if (!validateDateFormat(txtEndDateTimeEC1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeEC1";
                        }

                    }


                }
                if (txtEndDateTimeEC2 != "") {
                    if (!validateDateFormat(txtEndDateTimeEC2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeEC2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeEC1 != "" && txtStartDateTimeEC2 != "") {
                        var startDateTimeEC1 = formatStartEndDate(txtStartDateTimeEC1);
                        var startDateTimeEC2 = formatStartEndDate(txtStartDateTimeEC2);

                        if (startDateTimeEC2 < startDateTimeEC1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeEC2";
                        }
                    }
                    if (txtEndDateTimeEC1 != "" && txtEndDateTimeEC2 != "") {
                        var endDateTimeEC1 = formatStartEndDate(txtEndDateTimeEC1);
                        var endDateTimeEC2 = formatStartEndDate(txtEndDateTimeEC2);

                        if (endDateTimeEC2 < endDateTimeEC1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeEC2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                else {

                    GetRowCountEC(sortFilter);
                }

            }


           
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    // Stampping Validations
    if ($ActiveID == "titleStamping") {
        //GetDailyProcessStamppingDetails($callFrom);
        if (issearchField == true || $("#txtMachineST").val() != "" || $("#txtLocationST").val() != "" || $("#txtStartDateTimeST1").val() != "" || $("#txtStartDateTimeST2").val() != "" ||
            $("#txtStartOperatorST").val() != "" || $("#txtMetallicLabelNoST").val() != "" || $("#txtEndDateTimeST1").val() != "" || $("#txtEndDateTimeST2").val() != "" || $("#txtEndOperatorST").val() != "" || $("#ddlStamppingDecision").val() != "0")
        {
            if ($("#txtStartDateTimeST1").val().trim() == "" && $("#txtStartDateTimeST2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateTimeST1";
            }

            if ($("#txtEndDateTimeST1").val().trim() == "" && $("#txtEndDateTimeST2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateTimeST1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }





            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateTimeST1 = document.getElementById("txtStartDateTimeST1").value;
                var txtStartDateTimeST2 = document.getElementById("txtStartDateTimeST2").value;
                var txtEndDateTimeST1 = document.getElementById("txtEndDateTimeST1").value;
                var txtEndDateTimeST2 = document.getElementById("txtEndDateTimeST2").value;

                if (txtStartDateTimeST1 != "") {
                    if (!validateDateFormat(txtStartDateTimeST1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateTimeST1";
                    }
                }

                if (txtStartDateTimeST2 != "") {
                    if (!validateDateFormat(txtStartDateTimeST2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateTimeST2";
                        }

                    }
                }
                if (txtEndDateTimeST1 != "") {
                    if (!validateDateFormat(txtEndDateTimeST1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeST1";
                        }

                    }


                }
                if (txtEndDateTimeST2 != "") {
                    if (!validateDateFormat(txtEndDateTimeST2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateTimeST2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateTimeST1 != "" && txtStartDateTimeST2 != "") {
                        var startDateTimeST1 = formatStartEndDate(txtStartDateTimeST1);
                        var startDateTimeST2 = formatStartEndDate(txtStartDateTimeST2);

                        if (startDateTimeST2 < startDateTimeST1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateTimeST2";
                        }
                    }
                    if (txtEndDateTimeST1 != "" && txtEndDateTimeST2 != "") {
                        var endDateTimeST1 = formatStartEndDate(txtEndDateTimeST1);
                        var endDateTimeST2 = formatStartEndDate(txtEndDateTimeST2);

                        if (endDateTimeST2 < endDateTimeST1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateTimeST2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                else {

                    GetRowCountStampping(sortFilter);
                }

            }

        }
        
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }


    //PostHT Hardness Validations

    if ($ActiveID == "titlePostHTHardness") {
        if (issearchField == true || $("#txtMachineInter").val() != "" || $("#txtRptLocationInter").val() != "" || $("#txtInspectionStartDateInter").val() != "" || $("#txtInspectionEndDateInter").val() != "" ||
            $("#txtInspectorInter").val() != "" || $("#ddlProcConditionInter").val() != "0" || $("#txtPatternNoInter").val() != "" || $("#txtFurnaceNumInter").val() != "" || $("#ddlPostHTDecision").val() != "0") {

            if ($("#txtInspectionStartDateInter").val().trim() == "" && $("#txtInspectionEndDateInter").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_InspStartDate_Required").text());
                checkFocusField = "txtInspectionStartDateInter";
            }

           
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }
            if (errorMessage == "") {
                checkFocusField = "";
                var txtInspectionStartDateInter = document.getElementById("txtInspectionStartDateInter").value;
                var txtInspectionEndDateInter = document.getElementById("txtInspectionEndDateInter").value;

                if (txtInspectionStartDateInter != "") {
                    if (!validateOnlyDateFormat(txtInspectionStartDateInter)) {
                        errorMessage = $("#lblErrorMessageE_InspStartDate_Invalid").text().trim();
                        checkFocusField = "txtInspectionStartDateInter";
                    }
                }

                if (txtInspectionEndDateInter != "") {
                    if (!validateOnlyDateFormat(txtInspectionEndDateInter)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtInspectionEndDateInter";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtInspectionStartDateInter != "" && txtInspectionEndDateInter != "") {
                        var inspectionStartDateInter = formatStartEndDate(txtInspectionStartDateInter);
                        var inspectionEndDateInter = formatStartEndDate(txtInspectionEndDateInter);

                        if (inspectionEndDateInter < inspectionStartDateInter) {
                            errorMessage = $("#lblErrorMessageE_InspEndDateValidate").text().trim();
                            checkFocusField = "txtInspectionEndDateInter";
                        }
                    }

                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }


                else {
                    GetRowCountQATestHardness(sortFilter);
                }
            }
            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //MI Hardness Validations
    if ($ActiveID == "titleMIHardness") {      
        if (issearchField == true || $("#txtRptLocationHard").val() != "" || $("#txtInspectionStartDateHard").val() != "" || $("#txtInspectionEndDateHard").val() != "" || $("#txtInspectorHard").val() != "" ||
            $("#txtFurnaceNumHard").val() != "" || $("#ddlMIHardnessDecision").val() != "0") {

            if ($("#txtInspectionStartDateHard").val().trim() == "" && $("#txtInspectionEndDateHard").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_InspStartDate_Required").text());
                checkFocusField = "txtInspectionStartDateHard";
            }


            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }

            if (errorMessage == "") {
                checkFocusField = "";
                var txtInspectionStartDateHard = document.getElementById("txtInspectionStartDateHard").value;
                var txtInspectionEndDateHard = document.getElementById("txtInspectionEndDateHard").value;

                if (txtInspectionStartDateHard != "") {
                    if (!validateOnlyDateFormat(txtInspectionStartDateHard)) {
                        errorMessage = $("#lblErrorMessageE_InspStartDate_Invalid").text().trim();
                        checkFocusField = "txtInspectionStartDateHard";
                    }
                }

                if (txtInspectionEndDateHard != "") {
                    if (!validateOnlyDateFormat(txtInspectionEndDateHard)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtInspectionEndDateHard";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtInspectionStartDateHard != "" && txtInspectionEndDateHard != "") {
                        var inspectionStartDateHard = formatStartEndDate(txtInspectionStartDateHard);
                        var inspectionEndDateHard = formatStartEndDate(txtInspectionEndDateHard);

                        if (inspectionEndDateHard < inspectionStartDateHard) {
                            errorMessage = $("#lblErrorMessageE_InspEndDateValidate").text().trim();
                            checkFocusField = "txtInspectionEndDateHard";
                        }
                    }

                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }


                else {
                    GetRowCountMechHardness(sortFilter);
                }
            }
            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Mech Tensile Validations

    if ($ActiveID == "titleMechTensile") {     
        if (issearchField == true || $("#txtRptLocationTen").val() != "" || $("#txtInspectionStartDateTen").val() != "" || $("#txtInspectionEndDateTen").val() != "" || $("#txtInspectorTen").val() != "" ||
            $("#txtFurnaceNumTen").val() != "" || $("#ddlMITensileDecision").val() != "0") {


            if ($("#txtInspectionStartDateTen").val().trim() == "" && $("#txtInspectionEndDateTen").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_InspStartDate_Required").text());
                checkFocusField = "txtInspectionStartDateTen";
            }


            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }

            if (errorMessage == "") {
                checkFocusField = "";
                var txtInspectionStartDateTen = document.getElementById("txtInspectionStartDateTen").value;
                var txtInspectionEndDateTen = document.getElementById("txtInspectionEndDateTen").value;

                if (txtInspectionStartDateTen != "") {
                    if (!validateOnlyDateFormat(txtInspectionStartDateTen)) {
                        errorMessage = $("#lblErrorMessageE_InspStartDate_Invalid").text().trim();
                        checkFocusField = "txtInspectionStartDateTen";
                    }
                }

                if (txtInspectionEndDateTen != "") {
                    if (!validateOnlyDateFormat(txtInspectionEndDateTen)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_InspEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtInspectionEndDateTen";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtInspectionStartDateTen != "" && txtInspectionEndDateTen != "") {
                        var inspectionStartDateTen = formatStartEndDate(txtInspectionStartDateTen);
                        var inspectionEndDateTen = formatStartEndDate(txtInspectionEndDateTen);

                        if (inspectionEndDateTen < inspectionStartDateTen) {
                            errorMessage = $("#lblErrorMessageE_InspEndDateValidate").text().trim();
                            checkFocusField = "txtInspectionEndDateTen";
                        }
                    }

                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }


                else {
                    GetRowCountMechTensile(sortFilter);
                }
            }

            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    //Final Inspection Validations

    if ($ActiveID == "titleFinalInspection") {
        if (issearchField == true || $("#txtLocationCode").val() != "" || $("#txtInspectionStartDateFI").val() != "" || $("#txtInspectionEndDateFI").val() != "" || $("#txtInspectorFI").val() != "" || $("#txtSparkFIStartDate").val() != "" || $("#txtSparkFIEndDate").val() != "" ||
            $("#txtSparkFIOperator").val() != "" || $("#ddlFICombinedDecision").val() != "0") {

            if ($("#txtInspectionStartDateFI").val().trim() == "" && $("#txtInspectionEndDateFI").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_InspStartDateTimeRequired").text());
                checkFocusField = "txtInspectionStartDateFI";
            }

            if ($("#txtSparkFIStartDate").val().trim() == "" && $("#txtSparkFIEndDate").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtSparkFIStartDate';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_FISparkStartDate_Required").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_FISparkStartDate_Required").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }
            if (errorMessage == "") {
                checkFocusField = "";
                var txtInspectionStartDateFI = document.getElementById("txtInspectionStartDateFI").value;
                var txtInspectionEndDateFI = document.getElementById("txtInspectionEndDateFI").value;
                var txtSparkFIStartDate = document.getElementById("txtSparkFIStartDate").value;
                var txtSparkFIEndDate = document.getElementById("txtSparkFIEndDate").value;


                if (txtInspectionStartDateFI != "") {
                    if (!validateDateFormat(txtInspectionStartDateFI)) {
                        errorMessage = $("#lblErrorMessageE_InspStartDateTimeInvalid").text().trim();
                        checkFocusField = "txtInspectionStartDateFI";
                    }
                }

                if (txtInspectionEndDateFI != "") {
                    if (!validateDateFormat(txtInspectionEndDateFI)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_InspEndDateTimeInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_InspEndDateTimeInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtInspectionEndDateFI";
                        }

                    }
                }
                if (txtSparkFIStartDate != "") {
                    if (!validateDateFormat(txtSparkFIStartDate)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_FISparkStartDate_Invalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_FISparkStartDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtSparkFIStartDate";
                        }

                    }


                }
                if (txtSparkFIEndDate != "") {
                    if (!validateDateFormat(txtSparkFIEndDate)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_FISparkEndDate_Invalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_FISparkEndDate_Invalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtSparkFIEndDate";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtInspectionStartDateFI != "" && txtInspectionEndDateFI != "") {
                        var inspectionStartDateFI = formatStartEndDate(txtInspectionStartDateFI);
                        var inspectionEndDateFI = formatStartEndDate(txtInspectionEndDateFI);

                        if (inspectionEndDateFI < inspectionStartDateFI) {
                            errorMessage = $("#lblErrorMessageE_InspEndDateTimeValidate").text().trim();
                            checkFocusField = "txtInspectionEndDateFI";
                        }
                    }
                    if (txtSparkFIStartDate != "" && txtSparkFIEndDate != "") {
                        var sparkFIStartDate = formatStartEndDate(txtSparkFIStartDate);
                        var sparkFIEndDate = formatStartEndDate(txtSparkFIEndDate);

                        if (sparkFIEndDate < sparkFIStartDate) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_FISparkEndDate_Validate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_FISparkEndDate_Validate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtSparkFIEndDate";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                else {

                    GetRowCountFinalInspection(sortFilter);
                }

            }            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }


    //General Daily Process Validations
    if ($ActiveID == "titleGeneralDailyProcess") {

        if (issearchField == true || $("#ddlGPProcessKubun").val() != "0" || $("#txtMachineGP").val() != "" || $("#txtLocationGP").val() != "" || $("#txtStartDateGP1").val() != "" || $("#txtStartDateGP2").val() != "" || $("#txtStartOperatorGP").val() != "" ||
            $("#txtEndDateGp1").val() != "" || $("#txtEndDateGp2").val() != "" || $("#txtEndOperatorGP").val() != "" || $("#ddlDecisionGP").val() != "0") {

            if ($("#txtStartDateGP1").val().trim() == "" && $("#txtStartDateGP2").val().trim() != "") {
                errorMessage = $.trim($("#lblErrorMessageE_ProcessBeginningStartDateRequired").text());
                checkFocusField = "txtStartDateGP1";
            }

            if ($("#txtEndDateGp1").val().trim() == "" && $("#txtEndDateGp2").val().trim() != "") {
                if (checkFocusField == "") {
                    checkFocusField = 'txtEndDateGp1';
                }
                if (errorMessage == "") {
                    errorMessage = $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                } else {
                    errorMessage = errorMessage + "<br>" + $.trim($("#lblErrorMessageE_ProcessEndStartDateRequired").text());
                }

            }
            if (errorMessage != "") {
                showMessageDialog(errorMessage, checkFocusField, WARNING);
                return false;
            }





            if (errorMessage == "") {
                checkFocusField = "";
                var txtStartDateGP1 = document.getElementById("txtStartDateGP1").value;
                var txtStartDateGP2 = document.getElementById("txtStartDateGP2").value;
                var txtEndDateGp1 = document.getElementById("txtEndDateGp1").value;
                var txtEndDateGp2 = document.getElementById("txtEndDateGp2").value;


                if (txtStartDateGP1 != "") {
                    if (!validateDateFormat(txtStartDateGP1)) {
                        errorMessage = $("#lblErrorMessageE_ProcessBeginningStartDateInvalid").text().trim();
                        checkFocusField = "txtStartDateGP1";
                    }
                }

                if (txtStartDateGP2 != "") {
                    if (!validateDateFormat(txtStartDateGP2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessBeginningEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtStartDateGP2";
                        }

                    }
                }
                if (txtEndDateGp1 != "") {
                    if (!validateDateFormat(txtEndDateGp1)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndStartDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateGp1";
                        }

                    }


                }
                if (txtEndDateGp2 != "") {
                    if (!validateDateFormat(txtEndDateGp2)) {
                        if (errorMessage == "") {
                            errorMessage = $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        } else {
                            errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateInvalid").text().trim();
                        }
                        if (checkFocusField == "") {
                            checkFocusField = "txtEndDateGp2";
                        }

                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                if (errorMessage == "") {
                    checkFocusField = "";
                    if (txtStartDateGP1 != "" && txtStartDateGP2 != "") {
                        var startDateGP1 = formatStartEndDate(txtStartDateGP1);
                        var startDateGP2 = formatStartEndDate(txtStartDateGP2);

                        if (startDateGP2 < startDateGP1) {
                            errorMessage = $("#lblErrorMessageE_ProcessBeginningEndDateValidate").text().trim();
                            checkFocusField = "txtStartDateGP2";
                        }
                    }
                    if (txtEndDateGp1 != "" && txtEndDateGp2 != "") {
                        var endDateGp1 = formatStartEndDate(txtEndDateGp1);
                        var endDateGp2 = formatStartEndDate(txtEndDateGp2);

                        if (endDateGp2 < endDateGp1) {
                            if (errorMessage == "") {
                                errorMessage = $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            else {
                                errorMessage += "<br>" + $("#lblErrorMessageE_ProcessEndEndDateValidate").text().trim();
                            }
                            if (checkFocusField == "") {
                                checkFocusField = "txtEndDateGp2";
                            }

                        }
                    }
                }
                if (errorMessage != "") {
                    showMessageDialog(errorMessage, checkFocusField, WARNING);
                    return false;
                }

                else {

                    GetRowCountGeneralDailyProcess(sortFilter);
                }

            }

            
        }
        else {
            bootbox.alert({
                title: "<div style='color:white'></div>",
                message: "<table width='100%'><tr><td width='70px'><br/> <img src='/Images/warning.png'/ height='50px' width='50px'> </td><td vertical-align='top'>" + $.trim($("#lblErrorMessage_DailyProcess_SearchField").text()) + "</td></tr></table>",
                centerVertical: true
            });
        }
    }

    }).ajaxStart(function () {

        $("#LoadingImage").show();
    }).ajaxStop(function () {
        $("#LoadingImage").hide();
    }).ajaxError(function () {
        $("#LoadingImage").hide();
});

$(function () {
    $(document).ajaxStart(function () {

        $("#LoadingImage").show();
    });

    $(document).ajaxStop(function () {
        $("#LoadingImage").hide();
    });

    $(document).ajaxError(function () {
        $("#LoadingImage").hide();
    });
   
});
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

function GetHeatTreatmentDetails(sortFields) {
    
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_name = $("#txtLocationHT").val() ? $("#txtLocationHT").val() : null;
    var machine_name = $("#txtMachineHT").val() ? $("#txtMachineHT").val() : null;
    var funance_no = $("#txtFurnaceNumHT").val() ? $("#txtFurnaceNumHT").val() : null;

    var proc_condition = $("#ddlProcConditionHT option:selected").text() ? $("#ddlProcConditionHT option:selected").text() : null;
    var patternNo = $("#txtPatternNoHT").val() ? $("#txtPatternNoHT").val() : null;

    var beginning_Date_Start = $("#txtFurnanceInStartDateTime").val() ? $("#txtFurnanceInStartDateTime").val() : null;
    var beginning_Date_END = $("#txtFurnanceInEndDateTime").val() ? $("#txtFurnanceInEndDateTime").val() : null;

    var setUPConfByOperator = $("#txtSetUPConfByOperator").val() ? $("#txtSetUPConfByOperator").val() : null;
    var loadingConfByOperator = $("#txtLoadingConfByOperator").val() ? $("#txtLoadingConfByOperator").val() : null;    
   
    var end_Date1 = $("#txtFurnanceOutStartDateTime").val() ? $("#txtFurnanceOutStartDateTime").val() : null;
    var end_Date2 = $("#txtFurnanceOutEndDateTime").val() ? $("#txtFurnanceOutEndDateTime").val() : null;

    var offloadedByOperator = $("#txtOffloadedByOperator").val() ? $("#txtOffloadedByOperator").val() : null;
    var offloadConfByOperator = $("#txtOffloadConfByOperator").val() ? $("#txtOffloadConfByOperator").val() : null;

    var metallicLablNoRM = $("#txtMetallicLablNoRM").val() ? $("#txtMetallicLablNoRM").val() : null;
    var metallicLablNoCNo = $("#txtMetallicLablNoCNo").val() ? $("#txtMetallicLablNoCNo").val() : null;

   
    var rowLimit = $("#rowLimitValue").val(); 
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetHeatTreatmentDetailsOnDailyProcessAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,
            LOCATION_NAME: location_name, MACHINE_NAME: machine_name, FURNACE_NO: funance_no, HT_CONDITION: proc_condition, PATTERN_NO: patternNo, BEGINNING_START_DATE: beginning_Date_Start,
            BEGINNING_END_DATE: beginning_Date_END, HT_SETUP_CONF_BY_OPERATOR: setUPConfByOperator, HT_LOADING_CONF_BY_OPERATOR: loadingConfByOperator, END_DATE1: end_Date1,
            END_DATE2: end_Date2, HT_OFFLOADED_BY_OPERATOR: offloadedByOperator, HT_OFFLOAD_CONF_BY_OPERATOR: offloadConfByOperator, METALLIC_LABEL_NO_ROLL_MONTH: metallicLablNoRM,
            METALLIC_PLATE_CODE_STRING: metallicLablNoCNo, ROW_LIMIT: rowLimitValue,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                heatTreatmentFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                heatTreatmentFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/HeatTreatmentGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                    
                        $("#divHeatTreatmentDailyProcessGridMain").html(response1.message);                  
                   
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    closepopup();
}

function GetChamfferDetails(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineCHA").val() ? $("#txtMachineCHA").val() : null;
    var location_name = $("#txtLocationCHA").val() ? $("#txtLocationCHA").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeCHA1").val() ? $("#txtStartDateTimeCHA1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeCHA2").val() ? $("#txtStartDateTimeCHA2").val() : null;
    var start_operator = $("#txtStartOperatorCHA").val() ? $("#txtStartOperatorCHA").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoCHA").val() ? $("#txtMetallicLabelNoCHA").val() : null;
    var funance_no = $("#txtFurnaceNumCHA").val() ? $("#txtFurnaceNumCHA").val() : null;
    var end_Date1 = $("#txtEndDateTimeCHA1").val() ? $("#txtEndDateTimeCHA1").val() : null;
    var end_Date2 = $("#txtEndDateTimeCHA2").val() ? $("#txtEndDateTimeCHA2").val() : null;
    var end_operator = $("#txtEndOperatorCHA").val() ? $("#txtEndOperatorCHA").val() : null;
    var ch_Decision = $("#ddlChamferDecision").val() ? $("#ddlChamferDecision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetChamferDetailsOnDailyProcessAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: ch_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                chamferFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                chamferFlag = 0;
            }

            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/ChamfferDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {                   
                        $("#divChamfferDailyProcessGridMain").html(response1.message);                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetHardnessTemperingDetails(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;
    
    var location_Name = $("#txtLocationCodeHRD").val() ? $("#txtLocationCodeHRD").val() : null;  
    var funance_No = $("#txtFurnaceNumHRD").val() ? $("#txtFurnaceNumHRD").val() : null;

    var quenching_Machine = $("#txtQuenchingMachine").val() ? $("#txtQuenchingMachine").val() : null;
    var tempering_Machine = $("#txtTemperingMachine").val() ? $("#txtTemperingMachine").val() : null;

    var operator_Quenching = $("#txtOperatorQuenching").val() ? $("#txtOperatorQuenching").val() : null;

    var furnaceIn_StartDateTimeQ = $("#txtFurnaceInStartDateTimeQ").val() ? $("#txtFurnaceInStartDateTimeQ").val() : null;
    var furnaceIn_EndDateTimeQ = $("#txtFurnaceInEndDateTimeQ").val() ? $("#txtFurnaceInEndDateTimeQ").val() : null;

    var furnaceOut_StartDateTimeQ = $("#txtFurnaceOutStartDateTimeQ").val() ? $("#txtFurnaceOutStartDateTimeQ").val() : null;
    var furnaceOut_EndDateTimeQ = $("#txtFurnaceOutEndDateTimeQ").val() ? $("#txtFurnaceOutEndDateTimeQ").val() : null;

    var Operator_Tempering = $("#txtOperatorTempering").val() ? $("#txtOperatorTempering").val() : null;

    var furnaceIn_StartDateTimeT = $("#txtFurnaceInStartDateTimeT").val() ? $("#txtFurnaceInStartDateTimeT").val() : null;
    var furnaceIn_EndDateTimeT = $("#txtFurnaceInEndDateTimeT").val() ? $("#txtFurnaceInEndDateTimeT").val() : null;

    var furnaceOut_StartDatetimeT = $("#txtFurnaceOutStartDatetimeT").val() ? $("#txtFurnaceOutStartDatetimeT").val() : null;
    var furnaceOut_EndDatetimeT = $("#txtFurnaceOutEndDatetimeT").val() ? $("#txtFurnaceOutEndDatetimeT").val() : null;

    var inspection_StartDate = $("#txtInspectionStartDate").val() ? $("#txtInspectionStartDate").val() : null;
    var inspection_EndDate = $("#txtInspectionEndDate").val() ? $("#txtInspectionEndDate").val() : null;

    var inspectorHRD = $("#txtInspectorHRD").val() ? $("#txtInspectorHRD").val() : null;
    var hrd_Decision = $("#ddlInspectionDecesion").val() ? $("#ddlInspectionDecesion").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetHardnessTemperingDetailsOnDailyProcessAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,
            LOCATION_NAME: location_Name, FURNACE_NO: funance_No, QUENCHING_MACHINE: quenching_Machine, TEMPERING_MACHINE: tempering_Machine, Q_OPERATOR: operator_Quenching,
            Q_FURNACE_IN_START_DATE: furnaceIn_StartDateTimeQ, Q_FURNACE_IN_END_DATE: furnaceIn_EndDateTimeQ, Q_FURNACE_OUT_START_DATE: furnaceOut_StartDateTimeQ,
            Q_FURNACE_OUT_END_DATE: furnaceOut_EndDateTimeQ, T_OPERATOR: Operator_Tempering, T_FURNACE_IN_START_DATE: furnaceIn_StartDateTimeT, T_FURNACE_IN_END_DATE: furnaceIn_EndDateTimeT,
            T_FURNACE_OUT_START_DATE: furnaceOut_StartDatetimeT, T_FURNACE_OUT_END_DATE: furnaceOut_EndDatetimeT, INSPECTION_DATE1: inspection_StartDate, INSPECTION_DATE2: inspection_EndDate,
            OPERATOR_NAME: inspectorHRD, DECISION: hrd_Decision, ROW_LIMIT: rowLimitValue,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                hardnessTemperingFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                hardnessTemperingFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/HardnessTemperingDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                    
                        $("#divHardnessTemperingDailyProcessGridMain").html(response1.message);                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetHikinukiCoilToBarDetails(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineDRA").val() ? $("#txtMachineDRA").val() : null;
    var location_name = $("#txtLocationDRA").val() ? $("#txtLocationDRA").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDRA1").val() ? $("#txtStartDateTimeDRA1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDRA2").val() ? $("#txtStartDateTimeDRA2").val() : null;
    var start_operator = $("#txtStartOperatorDRA").val() ? $("#txtStartOperatorDRA").val() : null;
    var funance_no = $("#txtFurnaceNumDRA").val() ? $("#txtFurnaceNumDRA").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoDRA").val() ? $("#txtMetallicLabelNoDRA").val() : null;   
    var die_no = $("#txtDieNoDRA").val() ? $("#txtDieNoDRA").val() : null;   
    var end_Date1 = $("#txtEndDateTimeDRA1").val() ? $("#txtEndDateTimeDRA1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDRA2").val() ? $("#txtEndDateTimeDRA2").val() : null;
    var end_operator = $("#txtEndOperatorDRA").val() ? $("#txtEndOperatorDRA").val() : null;
    var dra_Decision = $("#ddlDRADecision").val() ? $("#ddlDRADecision").val() : null;
   
    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetHikinukiCoilToBarDetailsOnDailyProcessAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator, FURNACE_NO: funance_no,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, DIE_NO: die_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dra_Decision,
            ROW_LIMIT: rowLimitValue,SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                drawingCoilToBarFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_EN;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_EN;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_EN;


                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_JP;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_JP;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                drawingCoilToBarFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/HikinukiCoilToBarDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                   
                        $("#divHikinukiCoilToBarDailyProcessGridMain").html(response1.message);
                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function GetStraighteningDetails(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineSTR").val() ? $("#txtMachineSTR").val() : null;
    var location_name = $("#txtLocationSTR").val() ? $("#txtLocationSTR").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeSTR1_1").val() ? $("#txtStartDateTimeSTR1_1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeSTR1_2").val() ? $("#txtStartDateTimeSTR1_2").val() : null;
    var start_operator = $("#txtStartOperatorSTR").val() ? $("#txtStartOperatorSTR").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoSTR").val() ? $("#txtMetallicLabelNoSTR").val() : null;
    var funance_no = $("#txtFurnaceNumSTR").val() ? $("#txtFurnaceNumSTR").val() : null;
    var end_Date1 = $("#txtEndDateTimeSTR1_1").val() ? $("#txtEndDateTimeSTR1_1").val() : null;
    var end_Date2 = $("#txtEndDateTimeSTR1_2").val() ? $("#txtEndDateTimeSTR1_2").val() : null;
    var end_operator = $("#txtEndOperatorSTR").val() ? $("#txtEndOperatorSTR").val() : null;
    var str1_Decision = $("#ddlSTR1Decision").val() ? $("#ddlSTR1Decision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetStraighteningDailyReportOnDailyProcessAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: str1_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                straighteing1Flag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_EN;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_EN;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_EN;


                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_JP;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_JP;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                straighteing1Flag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                    
                        $("#divStraighteningDailyProcessGridMain").html(response1.message);
                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function GetStraightening2Details(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineSTR2").val() ? $("#txtMachineSTR2").val() : null;
    var location_name = $("#txtLocationSTR2").val() ? $("#txtLocationSTR2").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeSTR2_1").val() ? $("#txtStartDateTimeSTR2_1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeSTR2_2").val() ? $("#txtStartDateTimeSTR2_2").val() : null;
    var start_operator = $("#txtStartOperatorSTR2").val() ? $("#txtStartOperatorSTR2").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoSTR2").val() ? $("#txtMetallicLabelNoSTR2").val() : null;
    var funance_no = $("#txtFurnaceNumSTR2").val() ? $("#txtFurnaceNumSTR2").val() : null;
    var end_Date1 = $("#txtEndDateTimeSTR2_1").val() ? $("#txtEndDateTimeSTR2_1").val() : null;
    var end_Date2 = $("#txtEndDateTimeSTR2_2").val() ? $("#txtEndDateTimeSTR2_2").val() : null;
    var end_operator = $("#txtEndOperatorSTR2").val() ? $("#txtEndOperatorSTR2").val() : null;
    var str2_Decision = $("#ddlSTR2Decision").val() ? $("#ddlSTR2Decision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetStraightning2DailyProcessAnalysisStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: str2_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                straighteing2Flag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_EN;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_EN;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_EN;


                    });
                } else {
                   
                    $.each(jsonResult, function (key, object) {
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_JP;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_JP;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                straighteing2Flag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid2",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                   
                        $("#divStraighteningDailyProcessGridMain2").html(response1.message);
                   
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function GetFinalInspectionDetails(sortFields) {   
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var location_name = $("#txtLocationCode").val() ? $("#txtLocationCode").val() : null;
    var inspection_StartDate_FI = $("#txtInspectionStartDateFI").val() ? $("#txtInspectionStartDateFI").val() : null;
    var inspection_EndDate_FI = $("#txtInspectionEndDateFI").val() ? $("#txtInspectionEndDateFI").val() : null;
    var inspector = $("#txtInspectorFI").val() ? $("#txtInspectorFI").val() : null;        
    var sparkFI_StartDate = $("#txtSparkFIStartDate").val() ? $("#txtSparkFIStartDate").val() : null;
    var sparkFI_EndDate = $("#txtSparkFIEndDate").val() ? $("#txtSparkFIEndDate").val() : null;
    var sparkFI_Operator = $("#txtSparkFIOperator").val() ? $("#txtSparkFIOperator").val() : null;
    var fi_Decision = $("#ddlFICombinedDecision").val() ? $("#ddlFICombinedDecision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetFinalInspectionDataOnDailyProcessStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_FI, INSPECTION_DATE2: inspection_EndDate_FI,
            EMPLOYEE_NAME: inspector, START_DATETIME: sparkFI_StartDate, END_DATETIME: sparkFI_EndDate, OPERATOR_NAME: sparkFI_Operator, DECISION: fi_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                finalInspectionFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                finalInspectionFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/GetDailyProcessFinalInspectionDetailsGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                    
                        $("#divFinalInspectionDailyProcessGridMain").html(response1.message);
                   
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function GetMechTensileDetails(sortFields) {   
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var location_name = $("#txtRptLocationTen").val() ? $("#txtRptLocationTen").val() : null;
    var inspection_StartDate_Ten = $("#txtInspectionStartDateTen").val() ? $("#txtInspectionStartDateTen").val() : null;
    var inspection_EndDate_Ten = $("#txtInspectionEndDateTen").val() ? $("#txtInspectionEndDateTen").val() : null;
    var inspector = $("#txtInspectorTen").val() ? $("#txtInspectorTen").val() : null;
    var funance_no = $("#txtFurnaceNumTen").val() ? $("#txtFurnaceNumTen").val() : null;
    var miTensile_Decision = $("#ddlMITensileDecision").val() ? $("#ddlMITensileDecision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetMECHTensileOnDailyProcessStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Ten,
            INSPECTION_DATE2: inspection_EndDate_Ten, EMPLOYEE_NAME: inspector, FURNACE_NO: funance_no, DECISION: miTensile_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                mechTensileFlag = 1;

            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                mechTensileFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/GetDailyProcessMechTensileDetailsGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                   
                        $("#divMechTensileDailyProcessGridMain").html(response1.message);
                   
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
function GetQATestHardnessDetails(sortFields) {
    var ajaxTime = new Date().getTime();    
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var machine_name = $("#txtMachineInter").val() ? $("#txtMachineInter").val() : null;
    var location_name = $("#txtRptLocationInter").val() ? $("#txtRptLocationInter").val() : null;
    var inspection_StartDate_Inter = $("#txtInspectionStartDateInter").val() ? $("#txtInspectionStartDateInter").val() : null;
    var inspection_EndDate_Inter = $("#txtInspectionEndDateInter").val() ? $("#txtInspectionEndDateInter").val() : null;
    var inspector = $("#txtInspectorInter").val() ? $("#txtInspectorInter").val() : null;
    var proc_condition = $("#ddlProcConditionInter option:selected").text() ? $("#ddlProcConditionInter option:selected").text() : null;
    var pattern_no = $("#txtPatternNoInter").val() ? $("#txtPatternNoInter").val() : null;
    var funance_no = $("#txtFurnaceNumInter").val() ? $("#txtFurnaceNumInter").val() : null;
    var postHT_Decision = $("#ddlPostHTDecision").val() ? $("#ddlPostHTDecision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetQATestHardnessDetailsOnDailyProcessAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Inter,
            INSPECTION_DATE2: inspection_EndDate_Inter, EMPLOYEE_NAME: inspector, HT_CONDITION: proc_condition, PATTERN_NO: pattern_no, FURNACE_NO: funance_no, DECISION: postHT_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                postHTFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                postHTFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/QATestHardnessDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {                   
                        $("#divQATestHardnessDailyProcessGridMain").html(response1.message);                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetStamppingDetails(sortFields) {
   
    var ajaxTime = new Date().getDate();   
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineST").val() ? $("#txtMachineST").val() : null;
    var location_name = $("#txtLocationST").val() ? $("#txtLocationST").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeST1").val() ? $("#txtStartDateTimeST1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeST2").val() ? $("#txtStartDateTimeST2").val() : null;
    var start_operator = $("#txtStartOperatorST").val() ? $("#txtStartOperatorST").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoST").val() ? $("#txtMetallicLabelNoST").val() : null;    
    var end_Date1 = $("#txtEndDateTimeST1").val() ? $("#txtEndDateTimeST1").val() : null;
    var end_Date2 = $("#txtEndDateTimeST2").val() ? $("#txtEndDateTimeST2").val() : null;
    var end_operator = $("#txtEndOperatorST").val() ? $("#txtEndOperatorST").val() : null;
    var st_Decision = $("#ddlStamppingDecision").val() ? $("#ddlStamppingDecision").val() : null;   


    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetDailyProcessStamppingReportGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: st_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                stamppingFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.sT_NG_DETAILS1_STRING = object.sT_NG_DETAILS1_EN;
                        object.sT_NG_DETAILS2_STRING = object.sT_NG_DETAILS2_EN;
                        object.sT_NG_DETAILS3_STRING = object.sT_NG_DETAILS3_EN;

                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.sT_NG_DETAILS1_STRING = object.sT_NG_DETAILS1_JP;
                        object.sT_NG_DETAILS2_STRING = object.sT_NG_DETAILS2_JP;
                        object.sT_NG_DETAILS3_STRING = object.sT_NG_DETAILS3_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                stamppingFlag = 0;
            }
           

                var settings1 = {

                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/DailyProcessAnalysisWithCSV/GetDailyProcessStamppingReportGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    //data: jsonDataForSave
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {                       
                            $("#divDailyProcessAnalysisStamppingGridMain").html(response1.message);                       
                       
                    })
                    .fail(function (xhr, textStatus, errorThrown) {

                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessage_system_exception").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
           

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetECDetails(sortFields) {
    
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineEC").val() ? $("#txtMachineEC").val() : null;
    var location_name = $("#txtLocationEC").val() ? $("#txtLocationEC").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeEC1").val() ? $("#txtStartDateTimeEC1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeEC2").val() ? $("#txtStartDateTimeEC2").val() : null;
    var start_operator = $("#txtStartOperatorEC").val() ? $("#txtStartOperatorEC").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoEC").val() ? $("#txtMetallicLabelNoEC").val() : null;
    var funance_no = $("#txtFurnaceNumEC").val() ? $("#txtFurnaceNumEC").val() : null;
    var end_Date1 = $("#txtEndDateTimeEC1").val() ? $("#txtEndDateTimeEC1").val() : null;
    var end_Date2 = $("#txtEndDateTimeEC2").val() ? $("#txtEndDateTimeEC2").val() : null;
    var end_operator = $("#txtEndOperatorEC").val() ? $("#txtEndOperatorEC").val() : null;
    var ec_Decision = $("#ddlECDecision").val() ? $("#ddlECDecision").val() : null;   

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetDailyProcessECDetailsGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: ec_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                ecFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {
                        object.eC_NG_DETAILS1_STRING = object.eC_NG_DETAILS1_EN;
                        object.eC_NG_DETAILS2_STRING = object.eC_NG_DETAILS2_EN;
                        object.eC_NG_DETAILS3_STRING = object.eC_NG_DETAILS3_EN;

                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        object.eC_NG_DETAILS1_STRING = object.eC_NG_DETAILS1_JP;
                        object.eC_NG_DETAILS2_STRING = object.eC_NG_DETAILS2_JP;
                        object.eC_NG_DETAILS3_STRING = object.eC_NG_DETAILS3_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                ecFlag = 0;
            }

                var settings1 = {

                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/DailyProcessAnalysisWithCSV/GetDailyProcessECDetailsGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",                    
                    data: JSON.stringify(response.message)                    
                }
                $.ajax(settings1)
                    .done(function (response1) {                        
                            $("#divDailyProcessAnalysisECGridMain").html(response1.message);
                        
                    })
                    .fail(function (xhr, textStatus, errorThrown) {

                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessage_system_exception").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            
           

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetDailyProcessHikinukiBarToBarDetails(sortFields) {
    
    var ajaxTime = new Date().getDate(); 
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineDR").val() ? $("#txtMachineDR").val() : null;
    var location_name = $("#txtLocationDR").val() ? $("#txtLocationDR").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDR1").val() ? $("#txtStartDateTimeDR1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDR2").val() ? $("#txtStartDateTimeDR2").val() : null;
    var start_operator = $("#txtStartOperatorDR").val() ? $("#txtStartOperatorDR").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoDR").val() ? $("#txtMetallicLabelNoDR").val() : null;
    var die_no = $("#txtDieNoDR").val() ? $("#txtDieNoDR").val() : null;    
    var proc_condition = $("#ddlProcConditionDR option:selected").text() ? $("#ddlProcConditionDR option:selected").text() : null;
    var end_Date1 = $("#txtEndDateTimeDR1").val() ? $("#txtEndDateTimeDR1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDR2").val() ? $("#txtEndDateTimeDR2").val() : null;
    var end_operator = $("#txtEndOperatorDR").val() ? $("#txtEndOperatorDR").val() : null;
    var dr_Decision = $("#ddlDRDecision").val() ? $("#ddlDRDecision").val() : null;   

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetDailyProcessDrawingDGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, DIE_NO: die_no, HT_CONDITION: proc_condition, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dr_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                drawingBarToBarFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {                        
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_EN;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_EN;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_EN;


                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {                        
                        object.proC_DEFECT_SHORT1 = object.proC_DEFECT_SHORT1_JP;
                        object.proC_DEFECT_SHORT2 = object.proC_DEFECT_SHORT2_JP;
                        object.proC_DEFECT_SHORT3 = object.proC_DEFECT_SHORT3_JP;
                    });
                }
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                drawingBarToBarFlag = 0;
            }
                var settings1 = {

                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/DailyProcessAnalysisWithCSV/GetDailyProcessDrawingDGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    //data: jsonDataForSave
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        
                            $("#divDailyProcessAnalysisDrawingDGridMain").html(response1.message);                        
                       
                    })
                    .fail(function (xhr, textStatus, errorThrown) {

                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessage_system_exception").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetDailyProcessColdDrawingWireDrawingDetails(sortFields) {
    
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineCodeDW").val() ? $("#txtMachineCodeDW").val() : null;
    var location_name = $("#txtLocationCodeDW").val() ? $("#txtLocationCodeDW").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDW1").val() ? $("#txtStartDateTimeDW1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDW2").val() ? $("#txtStartDateTimeDW2").val() : null;
    var start_operator = $("#txtStartOperatorDW").val() ? $("#txtStartOperatorDW").val() : null;
    var rolling_no = $("#txtRollingNumberDW").val() ? $("#txtRollingNumberDW").val() : null;
    var funance_no = $("#txtFurnaceNumDW").val() ? $("#txtFurnaceNumDW").val() : null;
    var dieNO = $("#txtDieNoDW").val() ? $("#txtDieNoDW").val() : null;
    var end_Date1 = $("#txtEndDateTimeDW1").val() ? $("#txtEndDateTimeDW1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDW2").val() ? $("#txtEndDateTimeDW2").val() : null;
    var end_operator = $("#txtEndOperatorDW").val() ? $("#txtEndOperatorDW").val() : null;
    var dw_Decision = $("#ddlDWDecision").val() ? $("#ddlDWDecision").val() : null;   

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetDailyProcessColdDrawingWGrid",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            ROLLING_NUMBER: rolling_no, FURNACE_NO: funance_no, DIE_NO: dieNO, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dw_Decision,
            ROW_LIMIT: rowLimitValue,SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                drawingCoilToCoilFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                drawingCoilToCoilFlag = 0;
            }

                var settings1 = {

                    async: true,
                    crossDomain: true,
                    cache: false,
                    dataType: "json",
                    url: "/DailyProcessAnalysisWithCSV/GetDailyProcessColdDrawingWGrid",
                    method: "POST",
                    contentType: "application/json; charset=utf-8",
                    //data: jsonDataForSave
                    data: JSON.stringify(response.message)
                    //data: "{jsonData : " + jsonData + "}"
                }
                $.ajax(settings1)
                    .done(function (response1) {
                        
                            $("#divDailyProcessAnalysisColdDrawingWGridMain").html(response1.message);
                        
                    })
                    .fail(function (xhr, textStatus, errorThrown) {

                        responseTime = new Date().getTime() - ajaxTime;
                        var errorCode = $.parseJSON(xhr.responseText).Message;

                        focusFieldID = $(':focus').attr("id");
                        message = $.trim($("#lblErrorMessage_system_exception").text());
                        showMessageDialog(message, focusFieldID, ERROR);
                    });
            

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}

function GetMechHardnessDetails(sortFields) {
    var ajaxTime = new Date().getTime();    
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

   
    var location_name = $("#txtRptLocationHard").val() ? $("#txtRptLocationHard").val() : null;
    var inspection_StartDate_Hard = $("#txtInspectionStartDateHard").val() ? $("#txtInspectionStartDateHard").val() : null;
    var inspection_EndDate_Hard = $("#txtInspectionEndDateHard").val() ? $("#txtInspectionEndDateHard").val() : null;
    var inspector = $("#txtInspectorHard").val() ? $("#txtInspectorHard").val() : null;    
    var funance_no = $("#txtFurnaceNumHard").val() ? $("#txtFurnaceNumHard").val() : null;
    var miHardness_Decision = $("#ddlMIHardnessDecision").val() ? $("#ddlMIHardnessDecision").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetMechHardnessOnDailyProcessStatus",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Hard,
            INSPECTION_DATE2: inspection_EndDate_Hard, EMPLOYEE_NAME: inspector, FURNACE_NO: funance_no, DECISION: miHardness_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                mechHardnessFlag = 1;
            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                mechHardnessFlag = 0;
            }
            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/GetDailyProcessMechHardnessDetailsGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
                //data: "{jsonData : " + jsonData + "}"
            }
            $.ajax(settings1)
                .done(function (response1) {
                    
                        $("#divMiHardnessDailyProcessGridMain").html(response1.message);                    
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            //EnableDisableData(true);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
} 

function GetGeneralDailyProcessDetails(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var processKubun = $("#ddlGPProcessKubun").val() ? $("#ddlGPProcessKubun").val() : null;
    var machine_name = $("#txtMachineGP").val() ? $("#txtMachineGP").val() : null;
    var location_name = $("#txtLocationGP").val() ? $("#txtLocationGP").val() : null;    
  
    var beginning_Date_Start = $("#txtStartDateGP1").val() ? $("#txtStartDateGP1").val() : null;
    var beginning_Date_END = $("#txtStartDateGP2").val() ? $("#txtStartDateGP2").val() : null;
    var start_operator = $("#txtStartOperatorGP").val() ? $("#txtStartOperatorGP").val() : null;
   
    var end_Date1 = $("#txtEndDateGp1").val() ? $("#txtEndDateGp1").val() : null;
    var end_Date2 = $("#txtEndDateGp2").val() ? $("#txtEndDateGp2").val() : null;
    var end_operator = $("#txtEndOperatorGP").val() ? $("#txtEndOperatorGP").val() : null;
    var gp_Decision = $("#ddlDecisionGP").val() ? $("#ddlDecisionGP").val() : null;

    var rowLimit = $("#rowLimitValue").val();;
    if (rowLimit > 0) {
        rowLimitValue = rowLimit;

    }
    else {
        rowLimitValue = 0
    }

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetGeneralDailyProcessData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, PROCESS_KUBUN_CD: processKubun, MACHINE_NAME: machine_name, LOCATION_NAME: location_name, 
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
             END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: gp_Decision,
            ROW_LIMIT: rowLimitValue, SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;
            
            if (response.isSuccess == true) {
                $("#btnExportCSV").removeAttr("DISABLED").removeClass("DISABLED");
                generalProcessFlag = 1;
                if ($("#hdfCultureName").val() == "en") {
                    $.each(jsonResult, function (key, object) {                        
                        object.procesS_KUBUN_STRING = object.procesS_KUBUN_EN;
                        object.gP_NEXT_PROCESS_NAME = object.nexT_PROCESS_EN;

                        object.proC_DEFECT_STRING1 = object.proC_DEFECT1_EN;
                        object.proC_DEFECT_STRING2 = object.proC_DEFECT2_EN;
                        object.proC_DEFECT_STRING3 = object.proC_DEFECT3_EN;  
                    });
                } else {
                    
                    $.each(jsonResult, function (key, object) {
                        
                        object.procesS_KUBUN_STRING = object.procesS_KUBUN_JP;
                        object.gP_NEXT_PROCESS_NAME = object.nexT_PROCESS_JP;

                        object.proC_DEFECT_STRING1 = object.proC_DEFECT1_JP;
                        object.proC_DEFECT_STRING2 = object.proC_DEFECT2_JP;
                        object.proC_DEFECT_STRING3 = object.proC_DEFECT3_JP; 
                    });
                }

            }
            else {
                $("#btnExportCSV").attr("DISABLED", "DISABLED");
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
                generalProcessFlag = 0;
            }

            var settings1 = {
                async: true,
                crossDomain: true,
                cache: false,
                dataType: "json",
                url: "/DailyProcessAnalysisWithCSV/GeneralProcessDailyProcessGrid",
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(response.message)
               
            }
            $.ajax(settings1)
                .done(function (response1) {
                    $("#divGeneralProcessDailyProcessGridMain").html(response1.message);
                })
                .fail(function (xhr, textStatus, errorThrown) {
                    responseTime = new Date().getTime() - ajaxTime;
                    var errorCode = $.parseJSON(xhr.responseText).Message;

                    focusFieldID = $(':focus').attr("id");
                    message = $.trim($("#lblErrorMessage_system_exception").text());
                    showMessageDialog(message, focusFieldID, ERROR);
                });
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    closepopup();
}
//function validateDateFormat(dateVal) {

//    if (dateVal == null)
//        return false;

//    // YYYY-MM-DD HH:mm[:ss]
//    var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})\s(\d{1,2}):(\d{1,2})(?::(\d{1,2}))?$/;

//    var dateValues = dateVal.match(validatePattern);

//    if (dateValues == null)
//        return false;

//    var dtYear = parseInt(dateValues[1], 10);
//    var dtMonth = parseInt(dateValues[3], 10);
//    var dtDay = parseInt(dateValues[5], 10);
//    var dtHour = parseInt(dateValues[6], 10);
//    var dtMinute = parseInt(dateValues[7], 10);
//    var dtSecond = dateValues[8] ? parseInt(dateValues[8], 10) : 0;

//    // Date validation
//    if (dtMonth < 1 || dtMonth > 12)
//        return false;

//    if (dtDay < 1 || dtDay > 31)
//        return false;

//    if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
//        return false;

//    if (dtMonth == 2) {
//        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));

//        if (dtDay > 29 || (dtDay == 29 && !isleap))
//            return false;
//    }

//    // Time validation
//    if (dtHour < 0 || dtHour > 23)
//        return false;

//    if (dtMinute < 0 || dtMinute > 59)
//        return false;

//    //if (dtSecond < 0 || dtSecond > 59)
//    //    return false;

//    return true;
//}
function validateOnlyDateFormat(dateVal) {

    debugger;
    if (dateVal == null)
        return false;
    var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;

    dateValues = dateVal.match(validatePattern);

    if (dateValues == null)
        return false;
    debugger;
    var dtYear = dateValues[1];
    dtMonth = dateValues[3];
    dtDay = dateValues[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }

    return true;

}

function validateDateFormat(dateVal) {

    if (dateVal == null)
        return false;

    // YYYY-MM-DD or YYYY-MM-DD HH:mm[:ss]   
    var validatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})\s(\d{1,2}):(\d{1,2})(?::(\d{1,2}))?$/;
    var dateValues = dateVal.match(validatePattern);

    if (dateValues == null)
        return false;

    var dtYear = parseInt(dateValues[1], 10);
    var dtMonth = parseInt(dateValues[3], 10);
    var dtDay = parseInt(dateValues[5], 10);

    var dtHour = dateValues[6] ? parseInt(dateValues[6], 10) : 0;
    var dtMinute = dateValues[7] ? parseInt(dateValues[7], 10) : 0;
    var dtSecond = dateValues[8] ? parseInt(dateValues[8], 10) : 0;

    // Date validation
    if (dtMonth < 1 || dtMonth > 12)
        return false;

    if (dtDay < 1 || dtDay > 31)
        return false;

    if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;

    if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));

        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }

    // Time validation (only if time is provided)
    if (dateValues[6] !== undefined) {

        if (dtHour < 0 || dtHour > 23)
            return false;

        if (dtMinute < 0 || dtMinute > 59)
            return false;

        //if (dtSecond < 0 || dtSecond > 59)
        //    return false;
    }

    return true;
}

function DownloadHardnessTemperingCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_Name = $("#txtLocationCodeHRD").val() ? $("#txtLocationCodeHRD").val() : null;
    var funance_No = $("#txtFurnaceNumHRD").val() ? $("#txtFurnaceNumHRD").val() : null;

    var quenching_Machine = $("#txtQuenchingMachine").val() ? $("#txtQuenchingMachine").val() : null;
    var tempering_Machine = $("#txtTemperingMachine").val() ? $("#txtTemperingMachine").val() : null;

    var operator_Quenching = $("#txtOperatorQuenching").val() ? $("#txtOperatorQuenching").val() : null;

    var furnaceIn_StartDateTimeQ = $("#txtFurnaceInStartDateTimeQ").val() ? $("#txtFurnaceInStartDateTimeQ").val() : null;
    var furnaceIn_EndDateTimeQ = $("#txtFurnaceInEndDateTimeQ").val() ? $("#txtFurnaceInEndDateTimeQ").val() : null;

    var furnaceOut_StartDateTimeQ = $("#txtFurnaceOutStartDateTimeQ").val() ? $("#txtFurnaceOutStartDateTimeQ").val() : null;
    var furnaceOut_EndDateTimeQ = $("#txtFurnaceOutEndDateTimeQ").val() ? $("#txtFurnaceOutEndDateTimeQ").val() : null;

    var Operator_Tempering = $("#txtOperatorTempering").val() ? $("#txtOperatorTempering").val() : null;

    var furnaceIn_StartDateTimeT = $("#txtFurnaceInStartDateTimeT").val() ? $("#txtFurnaceInStartDateTimeT").val() : null;
    var furnaceIn_EndDateTimeT = $("#txtFurnaceInEndDateTimeT").val() ? $("#txtFurnaceInEndDateTimeT").val() : null;

    var furnaceOut_StartDatetimeT = $("#txtFurnaceOutStartDatetimeT").val() ? $("#txtFurnaceOutStartDatetimeT").val() : null;
    var furnaceOut_EndDatetimeT = $("#txtFurnaceOutEndDatetimeT").val() ? $("#txtFurnaceOutEndDatetimeT").val() : null;

    var inspection_StartDate = $("#txtInspectionStartDate").val() ? $("#txtInspectionStartDate").val() : null;
    var inspection_EndDate = $("#txtInspectionEndDate").val() ? $("#txtInspectionEndDate").val() : null;

    var inspectorHRD = $("#txtInspectorHRD").val() ? $("#txtInspectorHRD").val() : null;
    var hrd_Decision = $("#ddlInspectionDecesion").val() ? $("#ddlInspectionDecesion").val() : null;
    
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/DailyProcessAnalysisWithCSV/ExportHardnessTemperingCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        //  data: "{jsonData : " + jsonData + "}"
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,
            LOCATION_NAME: location_Name, FURNACE_NO: funance_No, QUENCHING_MACHINE: quenching_Machine, TEMPERING_MACHINE: tempering_Machine, Q_OPERATOR: operator_Quenching,
            Q_FURNACE_IN_START_DATE: furnaceIn_StartDateTimeQ, Q_FURNACE_IN_END_DATE: furnaceIn_EndDateTimeQ, Q_FURNACE_OUT_START_DATE: furnaceOut_StartDateTimeQ,
            Q_FURNACE_OUT_END_DATE: furnaceOut_EndDateTimeQ, T_OPERATOR: Operator_Tempering, T_FURNACE_IN_START_DATE: furnaceIn_StartDateTimeT, T_FURNACE_IN_END_DATE: furnaceIn_EndDateTimeT,
            T_FURNACE_OUT_START_DATE: furnaceOut_StartDatetimeT, T_FURNACE_OUT_END_DATE: furnaceOut_EndDatetimeT, INSPECTION_DATE1: inspection_StartDate, INSPECTION_DATE2: inspection_EndDate,
            OPERATOR_NAME: inspectorHRD, DECISION: hrd_Decision, CULTURE_NAME: cultureName, SORT_FIELDS: sortFields
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    
}

function DownloadHeatTreatmentCSV(sortFields) {
    debugger;
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_name = $("#txtLocationHT").val() ? $("#txtLocationHT").val() : null;
    var machine_name = $("#txtMachineHT").val() ? $("#txtMachineHT").val() : null;
    var funance_no = $("#txtFurnaceNumHT").val() ? $("#txtFurnaceNumHT").val() : null;

    var proc_condition = $("#ddlProcConditionHT option:selected").text() ? $("#ddlProcConditionHT option:selected").text() : null;
    var patternNo = $("#txtPatternNoHT").val() ? $("#txtPatternNoHT").val() : null;

    var beginning_Date_Start = $("#txtFurnanceInStartDateTime").val() ? $("#txtFurnanceInStartDateTime").val() : null;
    var beginning_Date_END = $("#txtFurnanceInEndDateTime").val() ? $("#txtFurnanceInEndDateTime").val() : null;

    var setUPConfByOperator = $("#txtSetUPConfByOperator").val() ? $("#txtSetUPConfByOperator").val() : null;
    var loadingConfByOperator = $("#txtLoadingConfByOperator").val() ? $("#txtLoadingConfByOperator").val() : null;

    var end_Date1 = $("#txtFurnanceOutStartDateTime").val() ? $("#txtFurnanceOutStartDateTime").val() : null;
    var end_Date2 = $("#txtFurnanceOutEndDateTime").val() ? $("#txtFurnanceOutEndDateTime").val() : null;

    var offloadedByOperator = $("#txtOffloadedByOperator").val() ? $("#txtOffloadedByOperator").val() : null;
    var offloadConfByOperator = $("#txtOffloadConfByOperator").val() ? $("#txtOffloadConfByOperator").val() : null;

    var metallicLablNoRM = $("#txtMetallicLablNoRM").val() ? $("#txtMetallicLablNoRM").val() : null;
    var metallicLablNoCNo = $("#txtMetallicLablNoCNo").val() ? $("#txtMetallicLablNoCNo").val() : null;

    var cultureName = $("#hdfCultureName").val()

   
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/DailyProcessAnalysisWithCSV/ExportHeatTreatmentCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",       
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,
            LOCATION_NAME: location_name, MACHINE_NAME: machine_name, FURNACE_NO: funance_no, HT_CONDITION: proc_condition, PATTERN_NO: patternNo, BEGINNING_START_DATE: beginning_Date_Start,
            BEGINNING_END_DATE: beginning_Date_END, HT_SETUP_CONF_BY_OPERATOR: setUPConfByOperator, HT_LOADING_CONF_BY_OPERATOR: loadingConfByOperator, END_DATE1: end_Date1,
            END_DATE2: end_Date2, HT_OFFLOADED_BY_OPERATOR: offloadedByOperator, HT_OFFLOAD_CONF_BY_OPERATOR: offloadConfByOperator, METALLIC_LABEL_NO_ROLL_MONTH: metallicLablNoRM,
            METALLIC_PLATE_CODE_STRING: metallicLablNoCNo, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {           
            responseTime = new Date().getTime() - ajaxTime;            
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

    
}

function DownloadHikinukiBarToBarCSV(sortFields) {

    var ajaxTime = new Date().getDate();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineDR").val() ? $("#txtMachineDR").val() : null;
    var location_name = $("#txtLocationDR").val() ? $("#txtLocationDR").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDR1").val() ? $("#txtStartDateTimeDR1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDR2").val() ? $("#txtStartDateTimeDR2").val() : null;
    var start_operator = $("#txtStartOperatorDR").val() ? $("#txtStartOperatorDR").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoDR").val() ? $("#txtMetallicLabelNoDR").val() : null;
    var die_no = $("#txtDieNoDR").val() ? $("#txtDieNoDR").val() : null;    
    var proc_condition = $("#ddlProcConditionDR option:selected").text() ? $("#ddlProcConditionDR option:selected").text() : null;
    var end_Date1 = $("#txtEndDateTimeDR1").val() ? $("#txtEndDateTimeDR1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDR2").val() ? $("#txtEndDateTimeDR2").val() : null;
    var end_operator = $("#txtEndOperatorDR").val() ? $("#txtEndOperatorDR").val() : null;
    var dr_Decision = $("#ddlDRDecision").val() ? $("#ddlDRDecision").val() : null;

    var cultureName = $("#hdfCultureName").val()
    

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportHikinukiBarToBarCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, DIE_NO: die_no, HT_CONDITION: proc_condition, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator,
            DECISION: dr_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;

            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    
}

function DownloadHikinukiCoilToBarCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineDRA").val() ? $("#txtMachineDRA").val() : null;
    var location_name = $("#txtLocationDRA").val() ? $("#txtLocationDRA").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDRA1").val() ? $("#txtStartDateTimeDRA1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDRA2").val() ? $("#txtStartDateTimeDRA2").val() : null;
    var start_operator = $("#txtStartOperatorDRA").val() ? $("#txtStartOperatorDRA").val() : null;
    var funance_no = $("#txtFurnaceNumDRA").val() ? $("#txtFurnaceNumDRA").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoDRA").val() ? $("#txtMetallicLabelNoDRA").val() : null;
    var die_no = $("#txtDieNoDRA").val() ? $("#txtDieNoDRA").val() : null;
    var end_Date1 = $("#txtEndDateTimeDRA1").val() ? $("#txtEndDateTimeDRA1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDRA2").val() ? $("#txtEndDateTimeDRA2").val() : null;
    var end_operator = $("#txtEndOperatorDRA").val() ? $("#txtEndOperatorDRA").val() : null;
    var dra_Decision = $("#ddlDRADecision").val() ? $("#ddlDRADecision").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportHikinukiCoilToBarCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",        
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator, FURNACE_NO: funance_no,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, DIE_NO: die_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dra_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;  
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
    
}

function DownloadColdDrawingWireDrawingCSV(sortFields)
{
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineCodeDW").val() ? $("#txtMachineCodeDW").val() : null;
    var location_name = $("#txtLocationCodeDW").val() ? $("#txtLocationCodeDW").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDW1").val() ? $("#txtStartDateTimeDW1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDW2").val() ? $("#txtStartDateTimeDW2").val() : null;
    var start_operator = $("#txtStartOperatorDW").val() ? $("#txtStartOperatorDW").val() : null;
    var rolling_no = $("#txtRollingNumberDW").val() ? $("#txtRollingNumberDW").val() : null;
    var funance_no = $("#txtFurnaceNumDW").val() ? $("#txtFurnaceNumDW").val() : null;
    var dieNO = $("#txtDieNoDW").val() ? $("#txtDieNoDW").val() : null;
    var end_Date1 = $("#txtEndDateTimeDW1").val() ? $("#txtEndDateTimeDW1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDW2").val() ? $("#txtEndDateTimeDW2").val() : null;
    var end_operator = $("#txtEndOperatorDW").val() ? $("#txtEndOperatorDW").val() : null;
    var dw_Decision = $("#ddlDWDecision").val() ? $("#ddlDWDecision").val() : null;   

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportHikinukiCoilToCoilCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            ROLLING_NUMBER: rolling_no, FURNACE_NO: funance_no, DIE_NO: dieNO, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dw_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function DownloadStraighteningCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineSTR").val() ? $("#txtMachineSTR").val() : null;
    var location_name = $("#txtLocationSTR").val() ? $("#txtLocationSTR").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeSTR1_1").val() ? $("#txtStartDateTimeSTR1_1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeSTR1_2").val() ? $("#txtStartDateTimeSTR1_2").val() : null;
    var start_operator = $("#txtStartOperatorSTR").val() ? $("#txtStartOperatorSTR").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoSTR").val() ? $("#txtMetallicLabelNoSTR").val() : null;
    var funance_no = $("#txtFurnaceNumSTR").val() ? $("#txtFurnaceNumSTR").val() : null;
    var end_Date1 = $("#txtEndDateTimeSTR1_1").val() ? $("#txtEndDateTimeSTR1_1").val() : null;
    var end_Date2 = $("#txtEndDateTimeSTR1_2").val() ? $("#txtEndDateTimeSTR1_2").val() : null;
    var end_operator = $("#txtEndOperatorSTR").val() ? $("#txtEndOperatorSTR").val() : null;
    var str1_Decision = $("#ddlSTR1Decision").val() ? $("#ddlSTR1Decision").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportStraighteningCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: str1_Decision,  CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function DownloadStraightening2CSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineSTR2").val() ? $("#txtMachineSTR2").val() : null;
    var location_name = $("#txtLocationSTR2").val() ? $("#txtLocationSTR2").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeSTR2_1").val() ? $("#txtStartDateTimeSTR2_1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeSTR2_2").val() ? $("#txtStartDateTimeSTR2_2").val() : null;
    var start_operator = $("#txtStartOperatorSTR2").val() ? $("#txtStartOperatorSTR2").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoSTR2").val() ? $("#txtMetallicLabelNoSTR2").val() : null;
    var funance_no = $("#txtFurnaceNumSTR2").val() ? $("#txtFurnaceNumSTR2").val() : null;
    var end_Date1 = $("#txtEndDateTimeSTR2_1").val() ? $("#txtEndDateTimeSTR2_1").val() : null;
    var end_Date2 = $("#txtEndDateTimeSTR2_2").val() ? $("#txtEndDateTimeSTR2_2").val() : null;
    var end_operator = $("#txtEndOperatorSTR2").val() ? $("#txtEndOperatorSTR2").val() : null;
    var str2_Decision = $("#ddlSTR2Decision").val() ? $("#ddlSTR2Decision").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportStraightening2CSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: str2_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function DownloadChamfferCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineCHA").val() ? $("#txtMachineCHA").val() : null;
    var location_name = $("#txtLocationCHA").val() ? $("#txtLocationCHA").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeCHA1").val() ? $("#txtStartDateTimeCHA1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeCHA2").val() ? $("#txtStartDateTimeCHA2").val() : null;
    var start_operator = $("#txtStartOperatorCHA").val() ? $("#txtStartOperatorCHA").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoCHA").val() ? $("#txtMetallicLabelNoCHA").val() : null;
    var funance_no = $("#txtFurnaceNumCHA").val() ? $("#txtFurnaceNumCHA").val() : null;
    var end_Date1 = $("#txtEndDateTimeCHA1").val() ? $("#txtEndDateTimeCHA1").val() : null;
    var end_Date2 = $("#txtEndDateTimeCHA2").val() ? $("#txtEndDateTimeCHA2").val() : null;
    var end_operator = $("#txtEndOperatorCHA").val() ? $("#txtEndOperatorCHA").val() : null;
    var ch_Decision = $("#ddlChamferDecision").val() ? $("#ddlChamferDecision").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportChamfferCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: ch_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function DownloadECCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineEC").val() ? $("#txtMachineEC").val() : null;
    var location_name = $("#txtLocationEC").val() ? $("#txtLocationEC").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeEC1").val() ? $("#txtStartDateTimeEC1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeEC2").val() ? $("#txtStartDateTimeEC2").val() : null;
    var start_operator = $("#txtStartOperatorEC").val() ? $("#txtStartOperatorEC").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoEC").val() ? $("#txtMetallicLabelNoEC").val() : null;
    var funance_no = $("#txtFurnaceNumEC").val() ? $("#txtFurnaceNumEC").val() : null;
    var end_Date1 = $("#txtEndDateTimeEC1").val() ? $("#txtEndDateTimeEC1").val() : null;
    var end_Date2 = $("#txtEndDateTimeEC2").val() ? $("#txtEndDateTimeEC2").val() : null;
    var end_operator = $("#txtEndOperatorEC").val() ? $("#txtEndOperatorEC").val() : null;
    var ec_Decision = $("#ddlECDecision").val() ? $("#ddlECDecision").val() : null;   

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportECCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: ec_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function DownloadStamppingCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineST").val() ? $("#txtMachineST").val() : null;
    var location_name = $("#txtLocationST").val() ? $("#txtLocationST").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeST1").val() ? $("#txtStartDateTimeST1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeST2").val() ? $("#txtStartDateTimeST2").val() : null;
    var start_operator = $("#txtStartOperatorST").val() ? $("#txtStartOperatorST").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoST").val() ? $("#txtMetallicLabelNoST").val() : null;
    var end_Date1 = $("#txtEndDateTimeST1").val() ? $("#txtEndDateTimeST1").val() : null;
    var end_Date2 = $("#txtEndDateTimeST2").val() ? $("#txtEndDateTimeST2").val() : null;
    var end_operator = $("#txtEndOperatorST").val() ? $("#txtEndOperatorST").val() : null;
    var st_Decision = $("#ddlStamppingDecision").val() ? $("#ddlStamppingDecision").val() : null;   


    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportStamppingCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: st_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function DownloadQATestHardnessCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var machine_name = $("#txtMachineInter").val() ? $("#txtMachineInter").val() : null;
    var location_name = $("#txtRptLocationInter").val() ? $("#txtRptLocationInter").val() : null;
    var inspection_StartDate_Inter = $("#txtInspectionStartDateInter").val() ? $("#txtInspectionStartDateInter").val() : null;
    var inspection_EndDate_Inter = $("#txtInspectionEndDateInter").val() ? $("#txtInspectionEndDateInter").val() : null;
    var inspector = $("#txtInspectorInter").val() ? $("#txtInspectorInter").val() : null;
    var proc_condition = $("#ddlProcConditionInter option:selected").text() ? $("#ddlProcConditionInter option:selected").text() : null;
    var pattern_no = $("#txtPatternNoInter").val() ? $("#txtPatternNoInter").val() : null;
    var funance_no = $("#txtFurnaceNumInter").val() ? $("#txtFurnaceNumInter").val() : null;
    var postHT_Decision = $("#ddlPostHTDecision").val() ? $("#ddlPostHTDecision").val() : null;
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/DailyProcessAnalysisWithCSV/ExportQATestHardnessCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Inter,
            INSPECTION_DATE2: inspection_EndDate_Inter, EMPLOYEE_NAME: inspector, HT_CONDITION: proc_condition, PATTERN_NO: pattern_no, FURNACE_NO: funance_no, DECISION: postHT_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function DownloadMechHardnessCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_name = $("#txtRptLocationHard").val() ? $("#txtRptLocationHard").val() : null;
    var inspection_StartDate_Hard = $("#txtInspectionStartDateHard").val() ? $("#txtInspectionStartDateHard").val() : null;
    var inspection_EndDate_Hard = $("#txtInspectionEndDateHard").val() ? $("#txtInspectionEndDateHard").val() : null;
    var inspector = $("#txtInspectorHard").val() ? $("#txtInspectorHard").val() : null;
    var funance_no = $("#txtFurnaceNumHard").val() ? $("#txtFurnaceNumHard").val() : null;
    var miHardness_Decision = $("#ddlMIHardnessDecision").val() ? $("#ddlMIHardnessDecision").val() : null;
    
    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportMechHardnessCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Hard,
            INSPECTION_DATE2: inspection_EndDate_Hard, EMPLOYEE_NAME: inspector, FURNACE_NO: funance_no, DECISION: miHardness_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function DownloadMechTensileCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_name = $("#txtRptLocationTen").val() ? $("#txtRptLocationTen").val() : null;
    var inspection_StartDate_Ten = $("#txtInspectionStartDateTen").val() ? $("#txtInspectionStartDateTen").val() : null;
    var inspection_EndDate_Ten = $("#txtInspectionEndDateTen").val() ? $("#txtInspectionEndDateTen").val() : null;
    var inspector = $("#txtInspectorTen").val() ? $("#txtInspectorTen").val() : null;
    var funance_no = $("#txtFurnaceNumTen").val() ? $("#txtFurnaceNumTen").val() : null;
    var miTensile_Decision = $("#ddlMITensileDecision").val() ? $("#ddlMITensileDecision").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportMechTensileCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Ten,
            INSPECTION_DATE2: inspection_EndDate_Ten, EMPLOYEE_NAME: inspector, FURNACE_NO: funance_no, DECISION: miTensile_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function DownloadFinalInspectionCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_name = $("#txtLocationCode").val() ? $("#txtLocationCode").val() : null;
    var inspection_StartDate_FI = $("#txtInspectionStartDateFI").val() ? $("#txtInspectionStartDateFI").val() : null;
    var inspection_EndDate_FI = $("#txtInspectionEndDateFI").val() ? $("#txtInspectionEndDateFI").val() : null;
    var inspector = $("#txtInspectorFI").val() ? $("#txtInspectorFI").val() : null;
    var sparkFI_StartDate = $("#txtSparkFIStartDate").val() ? $("#txtSparkFIStartDate").val() : null;
    var sparkFI_EndDate = $("#txtSparkFIEndDate").val() ? $("#txtSparkFIEndDate").val() : null;
    var sparkFI_Operator = $("#txtSparkFIOperator").val() ? $("#txtSparkFIOperator").val() : null;
    var fi_Decision = $("#ddlFICombinedDecision").val() ? $("#ddlFICombinedDecision").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportFinalInspectionCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_FI, INSPECTION_DATE2: inspection_EndDate_FI,
            EMPLOYEE_NAME: inspector, START_DATETIME: sparkFI_StartDate, END_DATETIME: sparkFI_EndDate, OPERATOR_NAME: sparkFI_Operator, DECISION: fi_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function DownloadGeneralDailyProcessCSV(sortFields) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var processKubun = $("#ddlGPProcessKubun").val() ? $("#ddlGPProcessKubun").val() : null;
    var machine_name = $("#txtMachineGP").val() ? $("#txtMachineGP").val() : null;
    var location_name = $("#txtLocationGP").val() ? $("#txtLocationGP").val() : null;

    var beginning_Date_Start = $("#txtStartDateGP1").val() ? $("#txtStartDateGP1").val() : null;
    var beginning_Date_END = $("#txtStartDateGP2").val() ? $("#txtStartDateGP2").val() : null;
    var start_operator = $("#txtStartOperatorGP").val() ? $("#txtStartOperatorGP").val() : null;

    var end_Date1 = $("#txtEndDateGp1").val() ? $("#txtEndDateGp1").val() : null;
    var end_Date2 = $("#txtEndDateGp2").val() ? $("#txtEndDateGp2").val() : null;
    var end_operator = $("#txtEndOperatorGP").val() ? $("#txtEndOperatorGP").val() : null;
    var gp_Decision = $("#ddlDecisionGP").val() ? $("#ddlDecisionGP").val() : null;

    var cultureName = $("#hdfCultureName").val()

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/ExportGeneralDailyProcessCSV",
        method: "POST",
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, PROCESS_KUBUN_CD: processKubun, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: gp_Decision, CULTURE_NAME: cultureName,
            SORT_FIELDS: sortFields

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var data = response.message;
            if (data == "Data not available") {
                focusFieldID = $(':focus').attr("id");
                message = $.trim($("#lblErrorMessage_DataNotFound").text());
                showMessageDialog(message, focusFieldID, WARNING);
            }
            else {
                window.location = data;
            }

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function setProcessKubun() {
    var ajaxTime = new Date().getTime();
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetProcessKubunList",
        method: "POST",
        contentType: "application/json; charset=utf-8",
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var jsonResult = response.message;

            if ($("#hdfCultureName").val() == "en") {
                $.each(jsonResult, function (key, object) {

                    object.procesS_KUBUN_STRING = object.procesS_KUBUN_EN;
                });
            } else {
                $.each(jsonResult, function (key, object) {

                    object.procesS_KUBUN_STRING = object.procesS_KUBUN_JP;
                });
            }
            bindProcessKubunDropdown(response.message);

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_ProcessDisplayConfig001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcessKubunDropdown(jsonResult) {
    $("#ddlGPProcessKubun").html("");
    $("#ddlGPProcessKubun").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlGPProcessKubun").append($('<option value="' + jsonObj.kubuN_ID + '"> ' + jsonObj.procesS_KUBUN_STRING + '</option>'));
    });
}

function setDecision() {

    var ajaxTime = new Date().getTime();
    
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: "/DailyProcessAnalysisWithCSV/GetDecision",
        method: "POST",
        contentType: "application/json; charset=utf-8",
       
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            binddecisionDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_CHAMFER_ENTRY_001").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function binddecisionDropdown(jsonResult) {
    $("#ddlDRDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDRDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlDRADecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDRADecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlDWDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDWDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlSTR1Decision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlSTR1Decision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlSTR2Decision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlSTR2Decision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlChamferDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlChamferDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlECDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlECDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlStamppingDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlStamppingDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlInspectionDecesion").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlInspectionDecesion ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlPostHTDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlPostHTDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlMIHardnessDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMIHardnessDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlMITensileDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlMITensileDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlFICombinedDecision").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlFICombinedDecision ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

    $("#ddlDecisionGP").html("").append($('<option value="0"></option>'));
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlDecisionGP ").append($('<option value="' + jsonObj.cd + '"> ' + jsonObj.name + '</option>'));
    });

}

function setProcConditionKubun() {
    var ajaxTime = new Date().getTime();   
    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url:  "/DailyProcessAnalysisWithCSV/GetProcConditionKubun",
        method: "GET",
        contentType: "application/json; charset=utf-8",        
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            bindProcConditionKubunDropdown(response.message);
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessageE_HARDNESS_LIST_006").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function bindProcConditionKubunDropdown(jsonResult) {
    $("#ddlProcConditionHT").html("").append($('<option value="0"></option>'));   
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcConditionHT").append($('<option value="' + jsonObj.cd + '">'+ jsonObj.name +'</option>'));
    });

   
    $("#ddlProcConditionDR").html("").append($('<option value="0"></option>'));  
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcConditionDR").append($('<option value="' + jsonObj.cd + '">'+ jsonObj.name +'</option>'));
    });

   
    $("#ddlProcConditionInter").html("").append($('<option value="0"></option>'));  
    $.each(jsonResult, function (key, jsonObj) {
        $("#ddlProcConditionInter").append($('<option value="' + jsonObj.cd + '">'+ jsonObj.name +'</option>'));
    });

}


function removeTabClass() {
    var heatTreatmentTabPanel = document.getElementById("tabHeatTreatment");
    heatTreatmentTabPanel.classList.add("active");
    var heatTreatmentGridPanel = document.getElementById("divHeatTreatment");
    heatTreatmentGridPanel.classList.add("active");
    heatTreatmentGridPanel.classList.add("show");


    $("#activeTabID").val("tabHeatTreatment");
    var targetValue = "titleHeatTreatment"
    $ActiveID = targetValue;
    if ($ActiveID == "titleHeatTreatment") {
        
        InitSortPopup($ActiveID);
    }



    var drawingBarToBarTabPanel = document.getElementById("tabDrawingBarToBar");
    drawingBarToBarTabPanel.classList.remove("active");
    var drawingBarToBarGridPanel = document.getElementById("divDrawingBarToBar");
    drawingBarToBarGridPanel.classList.remove("active");
    drawingBarToBarGridPanel.classList.remove("show");

    var drawingCoilToBarTabPanel = document.getElementById("tabDrawingCoilToBar");
    drawingCoilToBarTabPanel.classList.remove("active");
    var drawingCoilToBarGridPanel = document.getElementById("divDrawingCoilToBar");
    drawingCoilToBarGridPanel.classList.remove("active");
    drawingCoilToBarGridPanel.classList.remove("show");


    var wireDrawingTabPanel = document.getElementById("tabDrawingCoilToCoil");
    wireDrawingTabPanel.classList.remove("active");
    var wireDrawingGirdPanel = document.getElementById("divDrawingCoilToCoil");
    wireDrawingGirdPanel.classList.remove("active");
    wireDrawingGirdPanel.classList.remove("show");

    var straightening1TabPanel = document.getElementById("tabStraightening1");
    straightening1TabPanel.classList.remove("active");
    var straightening1GirdPanel = document.getElementById("divStraightening1");
    straightening1GirdPanel.classList.remove("active");
    straightening1GirdPanel.classList.remove("show");

    var straightening2TabPanel = document.getElementById("tabStraightening2");
    straightening2TabPanel.classList.remove("active");
    var straightening2GridPanel = document.getElementById("divStraightening2");
    straightening2GridPanel.classList.remove("active");
    straightening2GridPanel.classList.remove("show");

    var chamferTabPanel = document.getElementById("tabChamfer");
    chamferTabPanel.classList.remove("active");
    var chamferGirdPanel = document.getElementById("divChamfer");
    chamferGirdPanel.classList.remove("active");
    chamferGirdPanel.classList.remove("show");

    var ecTabPanel = document.getElementById("tabEC");
    ecTabPanel.classList.remove("active");
    var eCGirdPanel = document.getElementById("divEC");
    eCGirdPanel.classList.remove("active");
    eCGirdPanel.classList.remove("show");


    var stamppingTabPanel = document.getElementById("tabStamping");
    stamppingTabPanel.classList.remove("active");
    var stamppingGridPanel = document.getElementById("divStamping");
    stamppingGridPanel.classList.remove("active");
    stamppingGridPanel.classList.remove("show");


    var hardnessTemperingTabPanel = document.getElementById("tabHardnessTempering");
    hardnessTemperingTabPanel.classList.remove("active");
    var hardnessTemperingGirdPanel = document.getElementById("divHardnessTempering");
    hardnessTemperingGirdPanel.classList.remove("active");
    hardnessTemperingGirdPanel.classList.remove("show");

    var qATestHardnessTabPanel = document.getElementById("tabPostHTHardness");
    qATestHardnessTabPanel.classList.remove("active");
    var qATestHardnessGirdPanel = document.getElementById("divPostHTHardness");
    qATestHardnessGirdPanel.classList.remove("active");
    qATestHardnessGirdPanel.classList.remove("show");

    var mechHardnessTabPanel = document.getElementById("tabMIHardness");
    mechHardnessTabPanel.classList.remove("active");
    var mechHardnessGirdPanel = document.getElementById("divMIHardness");
    mechHardnessGirdPanel.classList.remove("active");
    mechHardnessGirdPanel.classList.remove("show");

    var mechTensileTabPanel = document.getElementById("tabMechTensile");
    mechTensileTabPanel.classList.remove("active");
    var mechTensileGridPanel = document.getElementById("divMechTensile");
    mechTensileGridPanel.classList.remove("active");
    mechTensileGridPanel.classList.remove("show");

    var finalInspectionTabPanel = document.getElementById("tabFinalInspection");
    finalInspectionTabPanel.classList.remove("active");
    var finalInspectionGridPanel = document.getElementById("divFinalInspection");
    finalInspectionGridPanel.classList.remove("active");
    finalInspectionGridPanel.classList.remove("show");

    var generalDailyProcessTabPanel = document.getElementById("tabGeneralDailyProcess");
    generalDailyProcessTabPanel.classList.remove("active");
    var generalDailyProcessGirdPanel = document.getElementById("divGeneralDailyProcess");
    generalDailyProcessGirdPanel.classList.remove("active");
    generalDailyProcessGirdPanel.classList.remove("show");


}

function GetRowCountHeatTreatment(sortFilter) {

    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_name = $("#txtLocationHT").val() ? $("#txtLocationHT").val() : null;
    var machine_name = $("#txtMachineHT").val() ? $("#txtMachineHT").val() : null;
    var funance_no = $("#txtFurnaceNumHT").val() ? $("#txtFurnaceNumHT").val() : null;

    var proc_condition = $("#ddlProcConditionHT option:selected").text() ? $("#ddlProcConditionHT option:selected").text() : null;
    var patternNo = $("#txtPatternNoHT").val() ? $("#txtPatternNoHT").val() : null;

    var beginning_Date_Start = $("#txtFurnanceInStartDateTime").val() ? $("#txtFurnanceInStartDateTime").val() : null;
    var beginning_Date_END = $("#txtFurnanceInEndDateTime").val() ? $("#txtFurnanceInEndDateTime").val() : null;

    var setUPConfByOperator = $("#txtSetUPConfByOperator").val() ? $("#txtSetUPConfByOperator").val() : null;
    var loadingConfByOperator = $("#txtLoadingConfByOperator").val() ? $("#txtLoadingConfByOperator").val() : null;

    var end_Date1 = $("#txtFurnanceOutStartDateTime").val() ? $("#txtFurnanceOutStartDateTime").val() : null;
    var end_Date2 = $("#txtFurnanceOutEndDateTime").val() ? $("#txtFurnanceOutEndDateTime").val() : null;

    var offloadedByOperator = $("#txtOffloadedByOperator").val() ? $("#txtOffloadedByOperator").val() : null;
    var offloadConfByOperator = $("#txtOffloadConfByOperator").val() ? $("#txtOffloadConfByOperator").val() : null;

    var metallicLablNoRM = $("#txtMetallicLablNoRM").val() ? $("#txtMetallicLablNoRM").val() : null;
    var metallicLablNoCNo = $("#txtMetallicLablNoCNo").val() ? $("#txtMetallicLablNoCNo").val() : null;


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HeatTreatment/GetRowCountForHeatTreatmentDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,
            LOCATION_NAME: location_name, MACHINE_NAME: machine_name, FURNACE_NO: funance_no, HT_CONDITION: proc_condition, PATTERN_NO: patternNo, BEGINNING_START_DATE: beginning_Date_Start,
            BEGINNING_END_DATE: beginning_Date_END, HT_SETUP_CONF_BY_OPERATOR: setUPConfByOperator, HT_LOADING_CONF_BY_OPERATOR: loadingConfByOperator, END_DATE1: end_Date1,
            END_DATE2: end_Date2, HT_OFFLOADED_BY_OPERATOR: offloadedByOperator, HT_OFFLOAD_CONF_BY_OPERATOR: offloadConfByOperator, METALLIC_LABEL_NO_ROLL_MONTH: metallicLablNoRM,
            METALLIC_PLATE_CODE_STRING: metallicLablNoCNo

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetHeatTreatmentDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableHeatTreatment"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/HeatTreatmentGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divHeatTreatmentDailyProcessGridMain").html(response.message);
                        })

                }

            }
            else {
                GetHeatTreatmentDetails(sortFilter);
            }       
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });

}

function GetRowCountHikinukiBarToBar(sortFilter) {
    
    var ajaxTime = new Date().getDate(); 
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineDR").val() ? $("#txtMachineDR").val() : null;
    var location_name = $("#txtLocationDR").val() ? $("#txtLocationDR").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDR1").val() ? $("#txtStartDateTimeDR1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDR2").val() ? $("#txtStartDateTimeDR2").val() : null;
    var start_operator = $("#txtStartOperatorDR").val() ? $("#txtStartOperatorDR").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoDR").val() ? $("#txtMetallicLabelNoDR").val() : null;
    var die_no = $("#txtDieNoDR").val() ? $("#txtDieNoDR").val() : null;    
    var proc_condition = $("#ddlProcConditionDR option:selected").text() ? $("#ddlProcConditionDR option:selected").text() : null;
    var end_Date1 = $("#txtEndDateTimeDR1").val() ? $("#txtEndDateTimeDR1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDR2").val() ? $("#txtEndDateTimeDR2").val() : null;
    var end_operator = $("#txtEndOperatorDR").val() ? $("#txtEndOperatorDR").val() : null;
    var dr_Decision = $("#ddlDRDecision").val() ? $("#ddlDRDecision").val() : null;   

   

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiBarToBar/GetRowCountForHikinukiBarToBarDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, DIE_NO: die_no, HT_CONDITION: proc_condition, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dr_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetDailyProcessHikinukiBarToBarDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableDrawingBarToBar"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessDrawingDGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divDailyProcessAnalysisDrawingDGridMain").html(response.message);
                        })

                }

            }
            else {
                GetDailyProcessHikinukiBarToBarDetails(sortFilter);
            }    
            
            

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountHikinukiCoilToBar(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineDRA").val() ? $("#txtMachineDRA").val() : null;
    var location_name = $("#txtLocationDRA").val() ? $("#txtLocationDRA").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDRA1").val() ? $("#txtStartDateTimeDRA1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDRA2").val() ? $("#txtStartDateTimeDRA2").val() : null;
    var start_operator = $("#txtStartOperatorDRA").val() ? $("#txtStartOperatorDRA").val() : null;
    var funance_no = $("#txtFurnaceNumDRA").val() ? $("#txtFurnaceNumDRA").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoDRA").val() ? $("#txtMetallicLabelNoDRA").val() : null;
    var die_no = $("#txtDieNoDRA").val() ? $("#txtDieNoDRA").val() : null;
    var end_Date1 = $("#txtEndDateTimeDRA1").val() ? $("#txtEndDateTimeDRA1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDRA2").val() ? $("#txtEndDateTimeDRA2").val() : null;
    var end_operator = $("#txtEndOperatorDRA").val() ? $("#txtEndOperatorDRA").val() : null;
    var dra_Decision = $("#ddlDRADecision").val() ? $("#ddlDRADecision").val() : null;    

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HikinukiCoilToBar/GetRowCountForHikinukiCoilToBarDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator, FURNACE_NO: funance_no,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, DIE_NO: die_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dra_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetHikinukiCoilToBarDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableCoilToBar"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/HikinukiCoilToBarDailyProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divHikinukiCoilToBarDailyProcessGridMain").html(response.message);
                        })

                }

            }
            else {
                GetHikinukiCoilToBarDetails(sortFilter);
            }    
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountColdDrawingWireDrawing(sortFilter) {

    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineCodeDW").val() ? $("#txtMachineCodeDW").val() : null;
    var location_name = $("#txtLocationCodeDW").val() ? $("#txtLocationCodeDW").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeDW1").val() ? $("#txtStartDateTimeDW1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeDW2").val() ? $("#txtStartDateTimeDW2").val() : null;
    var start_operator = $("#txtStartOperatorDW").val() ? $("#txtStartOperatorDW").val() : null;
    var rolling_no = $("#txtRollingNumberDW").val() ? $("#txtRollingNumberDW").val() : null;
    var funance_no = $("#txtFurnaceNumDW").val() ? $("#txtFurnaceNumDW").val() : null;
    var dieNO = $("#txtDieNoDW").val() ? $("#txtDieNoDW").val() : null;
    var end_Date1 = $("#txtEndDateTimeDW1").val() ? $("#txtEndDateTimeDW1").val() : null;
    var end_Date2 = $("#txtEndDateTimeDW2").val() ? $("#txtEndDateTimeDW2").val() : null;
    var end_operator = $("#txtEndOperatorDW").val() ? $("#txtEndOperatorDW").val() : null;
    var dw_Decision = $("#ddlDWDecision").val() ? $("#ddlDWDecision").val() : null;
       

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/ColdDrawingWIREDRAWING/GetRowCountForColdDrawingCoilToCoilDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            ROLLING_NUMBER: rolling_no, FURNACE_NO: funance_no, DIE_NO: dieNO, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: dw_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;
           
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetDailyProcessColdDrawingWireDrawingDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableCoilToCoil"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessColdDrawingWGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divDailyProcessAnalysisColdDrawingWGridMain").html(response.message);
                        })

                }

            }
            else {
                GetDailyProcessColdDrawingWireDrawingDetails(sortFilter);
            }  
            


        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountStraightening(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineSTR").val() ? $("#txtMachineSTR").val() : null;
    var location_name = $("#txtLocationSTR").val() ? $("#txtLocationSTR").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeSTR1_1").val() ? $("#txtStartDateTimeSTR1_1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeSTR1_2").val() ? $("#txtStartDateTimeSTR1_2").val() : null;
    var start_operator = $("#txtStartOperatorSTR").val() ? $("#txtStartOperatorSTR").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoSTR").val() ? $("#txtMetallicLabelNoSTR").val() : null;
    var funance_no = $("#txtFurnaceNumSTR").val() ? $("#txtFurnaceNumSTR").val() : null;
    var end_Date1 = $("#txtEndDateTimeSTR1_1").val() ? $("#txtEndDateTimeSTR1_1").val() : null;
    var end_Date2 = $("#txtEndDateTimeSTR1_2").val() ? $("#txtEndDateTimeSTR1_2").val() : null;
    var end_operator = $("#txtEndOperatorSTR").val() ? $("#txtEndOperatorSTR").val() : null;
    var str1_Decision = $("#ddlSTR1Decision").val() ? $("#ddlSTR1Decision").val() : null;    

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening/GetRowCountForStraighteningDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: str1_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetStraighteningDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableStraightening1"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divStraighteningDailyProcessGridMain").html(response.message);
                        })

                }

            }
            else {
                GetStraighteningDetails(sortFilter);
            }  
            
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountStraightening2(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineSTR2").val() ? $("#txtMachineSTR2").val() : null;
    var location_name = $("#txtLocationSTR2").val() ? $("#txtLocationSTR2").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeSTR2_1").val() ? $("#txtStartDateTimeSTR2_1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeSTR2_2").val() ? $("#txtStartDateTimeSTR2_2").val() : null;
    var start_operator = $("#txtStartOperatorSTR2").val() ? $("#txtStartOperatorSTR2").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoSTR2").val() ? $("#txtMetallicLabelNoSTR2").val() : null;
    var funance_no = $("#txtFurnaceNumSTR2").val() ? $("#txtFurnaceNumSTR2").val() : null;
    var end_Date1 = $("#txtEndDateTimeSTR2_1").val() ? $("#txtEndDateTimeSTR2_1").val() : null;
    var end_Date2 = $("#txtEndDateTimeSTR2_2").val() ? $("#txtEndDateTimeSTR2_2").val() : null;
    var end_operator = $("#txtEndOperatorSTR2").val() ? $("#txtEndOperatorSTR2").val() : null;
    var str2_Decision = $("#ddlSTR2Decision").val() ? $("#ddlSTR2Decision").val() : null;
      

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Straightening2/GetRowCountForStraightening2DailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: str2_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetStraightening2Details(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableStraightening2"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/StraighteningDailyProcessGrid2",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divStraighteningDailyProcessGridMain2").html(response.message);
                        })

                }

            }
            else {
                GetStraightening2Details(sortFilter);
            }  
           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountChamffer(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineCHA").val() ? $("#txtMachineCHA").val() : null;
    var location_name = $("#txtLocationCHA").val() ? $("#txtLocationCHA").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeCHA1").val() ? $("#txtStartDateTimeCHA1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeCHA2").val() ? $("#txtStartDateTimeCHA2").val() : null;
    var start_operator = $("#txtStartOperatorCHA").val() ? $("#txtStartOperatorCHA").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoCHA").val() ? $("#txtMetallicLabelNoCHA").val() : null;
    var funance_no = $("#txtFurnaceNumCHA").val() ? $("#txtFurnaceNumCHA").val() : null;
    var end_Date1 = $("#txtEndDateTimeCHA1").val() ? $("#txtEndDateTimeCHA1").val() : null;
    var end_Date2 = $("#txtEndDateTimeCHA2").val() ? $("#txtEndDateTimeCHA2").val() : null;
    var end_operator = $("#txtEndOperatorCHA").val() ? $("#txtEndOperatorCHA").val() : null;
    var ch_Decision = $("#ddlChamferDecision").val() ? $("#ddlChamferDecision").val() : null;
       

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Chamffer/GetRowCountForChamfferDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: ch_Decision
           

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetChamfferDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableChamfer"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/ChamfferDailyProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divChamfferDailyProcessGridMain").html(response.message);
                        })

                }

            }
            else {
                GetChamfferDetails(sortFilter);
            }  

           
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountEC(sortFilter) {

    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineEC").val() ? $("#txtMachineEC").val() : null;
    var location_name = $("#txtLocationEC").val() ? $("#txtLocationEC").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeEC1").val() ? $("#txtStartDateTimeEC1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeEC2").val() ? $("#txtStartDateTimeEC2").val() : null;
    var start_operator = $("#txtStartOperatorEC").val() ? $("#txtStartOperatorEC").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoEC").val() ? $("#txtMetallicLabelNoEC").val() : null;
    var funance_no = $("#txtFurnaceNumEC").val() ? $("#txtFurnaceNumEC").val() : null;
    var end_Date1 = $("#txtEndDateTimeEC1").val() ? $("#txtEndDateTimeEC1").val() : null;
    var end_Date2 = $("#txtEndDateTimeEC2").val() ? $("#txtEndDateTimeEC2").val() : null;
    var end_operator = $("#txtEndOperatorEC").val() ? $("#txtEndOperatorEC").val() : null;
    var ec_Decision = $("#ddlECDecision").val() ? $("#ddlECDecision").val() : null;   

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/EC/GetRowCountForECDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, FURNACE_NO: funance_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: ec_Decision
           

        })
    }
    $.ajax(settings)
        .done(function (response) {

            responseTime = new Date().getTime() - ajaxTime;

            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetECDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableEC"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessECDetailsGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divDailyProcessAnalysisECGridMain").html(response.message);
                        })

                }

            }
            else {
                GetECDetails(sortFilter);
            }                        

        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountStampping(sortFilter) {

    var ajaxTime = new Date().getDate();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var machine_name = $("#txtMachineST").val() ? $("#txtMachineST").val() : null;
    var location_name = $("#txtLocationST").val() ? $("#txtLocationST").val() : null;
    var beginning_Date_Start = $("#txtStartDateTimeST1").val() ? $("#txtStartDateTimeST1").val() : null;
    var beginning_Date_END = $("#txtStartDateTimeST2").val() ? $("#txtStartDateTimeST2").val() : null;
    var start_operator = $("#txtStartOperatorST").val() ? $("#txtStartOperatorST").val() : null;
    var metallic_lab_no = $("#txtMetallicLabelNoST").val() ? $("#txtMetallicLabelNoST").val() : null;
    var end_Date1 = $("#txtEndDateTimeST1").val() ? $("#txtEndDateTimeST1").val() : null;
    var end_Date2 = $("#txtEndDateTimeST2").val() ? $("#txtEndDateTimeST2").val() : null;
    var end_operator = $("#txtEndOperatorST").val() ? $("#txtEndOperatorST").val() : null;
    var st_Decision = $("#ddlStamppingDecision").val() ? $("#ddlStamppingDecision").val() : null;
  

    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Stampping/GetRowCountForStamppingDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            METALLIC_PLATE_CODE_STRING: metallic_lab_no, END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: st_Decision
           

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;           
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetStamppingDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableStampping"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessStamppingReportGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divDailyProcessAnalysisStamppingGridMain").html(response.message);
                        })
                }

            }
            else {
                GetStamppingDetails(sortFilter);
            }                       


        })
        .fail(function (xhr, textStatus, errorThrown) {

            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountHardnessTempering(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var location_Name = $("#txtLocationCodeHRD").val() ? $("#txtLocationCodeHRD").val() : null;
    var funance_No = $("#txtFurnaceNumHRD").val() ? $("#txtFurnaceNumHRD").val() : null;

    var quenching_Machine = $("#txtQuenchingMachine").val() ? $("#txtQuenchingMachine").val() : null;
    var tempering_Machine = $("#txtTemperingMachine").val() ? $("#txtTemperingMachine").val() : null;

    var operator_Quenching = $("#txtOperatorQuenching").val() ? $("#txtOperatorQuenching").val() : null;

    var furnaceIn_StartDateTimeQ = $("#txtFurnaceInStartDateTimeQ").val() ? $("#txtFurnaceInStartDateTimeQ").val() : null;
    var furnaceIn_EndDateTimeQ = $("#txtFurnaceInEndDateTimeQ").val() ? $("#txtFurnaceInEndDateTimeQ").val() : null;

    var furnaceOut_StartDateTimeQ = $("#txtFurnaceOutStartDateTimeQ").val() ? $("#txtFurnaceOutStartDateTimeQ").val() : null;
    var furnaceOut_EndDateTimeQ = $("#txtFurnaceOutEndDateTimeQ").val() ? $("#txtFurnaceOutEndDateTimeQ").val() : null;

    var Operator_Tempering = $("#txtOperatorTempering").val() ? $("#txtOperatorTempering").val() : null;

    var furnaceIn_StartDateTimeT = $("#txtFurnaceInStartDateTimeT").val() ? $("#txtFurnaceInStartDateTimeT").val() : null;
    var furnaceIn_EndDateTimeT = $("#txtFurnaceInEndDateTimeT").val() ? $("#txtFurnaceInEndDateTimeT").val() : null;

    var furnaceOut_StartDatetimeT = $("#txtFurnaceOutStartDatetimeT").val() ? $("#txtFurnaceOutStartDatetimeT").val() : null;
    var furnaceOut_EndDatetimeT = $("#txtFurnaceOutEndDatetimeT").val() ? $("#txtFurnaceOutEndDatetimeT").val() : null;

    var inspection_StartDate = $("#txtInspectionStartDate").val() ? $("#txtInspectionStartDate").val() : null;
    var inspection_EndDate = $("#txtInspectionEndDate").val() ? $("#txtInspectionEndDate").val() : null;

    var inspectorHRD = $("#txtInspectorHRD").val() ? $("#txtInspectorHRD").val() : null;
    var hrd_Decision = $("#ddlInspectionDecesion").val() ? $("#ddlInspectionDecesion").val() : null;


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/HardnessTempering/GetRowCountForHardnessTemperingtDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End,
            LOCATION_NAME: location_Name, FURNACE_NO: funance_No, QUENCHING_MACHINE: quenching_Machine, TEMPERING_MACHINE: tempering_Machine, Q_OPERATOR: operator_Quenching,
            Q_FURNACE_IN_START_DATE: furnaceIn_StartDateTimeQ, Q_FURNACE_IN_END_DATE: furnaceIn_EndDateTimeQ, Q_FURNACE_OUT_START_DATE: furnaceOut_StartDateTimeQ,
            Q_FURNACE_OUT_END_DATE: furnaceOut_EndDateTimeQ, T_OPERATOR: Operator_Tempering, T_FURNACE_IN_START_DATE: furnaceIn_StartDateTimeT, T_FURNACE_IN_END_DATE: furnaceIn_EndDateTimeT,
            T_FURNACE_OUT_START_DATE: furnaceOut_StartDatetimeT, T_FURNACE_OUT_END_DATE: furnaceOut_EndDatetimeT, INSPECTION_DATE1: inspection_StartDate, INSPECTION_DATE2: inspection_EndDate,
            OPERATOR_NAME: inspectorHRD, DECISION: hrd_Decision

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetHardnessTemperingDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableHardnessTempering"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/HardnessTemperingDailyProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divHardnessTemperingDailyProcessGridMain").html(response.message);
                        })
                }

            }
            else {
                GetHardnessTemperingDetails(sortFilter);
            } 
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountQATestHardness(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var machine_name = $("#txtMachineInter").val() ? $("#txtMachineInter").val() : null;
    var location_name = $("#txtRptLocationInter").val() ? $("#txtRptLocationInter").val() : null;
    var inspection_StartDate_Inter = $("#txtInspectionStartDateInter").val() ? $("#txtInspectionStartDateInter").val() : null;
    var inspection_EndDate_Inter = $("#txtInspectionEndDateInter").val() ? $("#txtInspectionEndDateInter").val() : null;
    var inspector = $("#txtInspectorInter").val() ? $("#txtInspectorInter").val() : null;
    var proc_condition = $("#ddlProcConditionInter option:selected").text() ? $("#ddlProcConditionInter option:selected").text() : null;
    var pattern_no = $("#txtPatternNoInter").val() ? $("#txtPatternNoInter").val() : null;
    var funance_no = $("#txtFurnaceNumInter").val() ? $("#txtFurnaceNumInter").val() : null;
    var postHT_Decision = $("#ddlPostHTDecision").val() ? $("#ddlPostHTDecision").val() : null;

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/PostHtInspection/GetRowCountForPostHTHardnessDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, MACHINE_NAME: machine_name, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Inter,
            INSPECTION_DATE2: inspection_EndDate_Inter, EMPLOYEE_NAME: inspector, HT_CONDITION: proc_condition, PATTERN_NO: pattern_no, FURNACE_NO: funance_no, DECISION: postHT_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetQATestHardnessDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableQATestHardness"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/QATestHardnessDailyProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divQATestHardnessDailyProcessGridMain").html(response.message);
                        })
                }

            }
            else {
                GetQATestHardnessDetails(sortFilter);
            } 

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountMechHardness(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var location_name = $("#txtRptLocationHard").val() ? $("#txtRptLocationHard").val() : null;
    var inspection_StartDate_Hard = $("#txtInspectionStartDateHard").val() ? $("#txtInspectionStartDateHard").val() : null;
    var inspection_EndDate_Hard = $("#txtInspectionEndDateHard").val() ? $("#txtInspectionEndDateHard").val() : null;
    var inspector = $("#txtInspectorHard").val() ? $("#txtInspectorHard").val() : null;
    var funance_no = $("#txtFurnaceNumHard").val() ? $("#txtFurnaceNumHard").val() : null;
    var miHardness_Decision = $("#ddlMIHardnessDecision").val() ? $("#ddlMIHardnessDecision").val() : null;
        

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Hardness/GetRowCountForMechHardnessDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Hard,
            INSPECTION_DATE2: inspection_EndDate_Hard, EMPLOYEE_NAME: inspector, FURNACE_NO: funance_no, DECISION: miHardness_Decision
           

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;

            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetMechHardnessDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableMIHardness"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessMechHardnessDetailsGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divMiHardnessDailyProcessGridMain").html(response.message);
                        })
                }

            }
            else {
                GetMechHardnessDetails(sortFilter);
            } 

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
} 

function GetRowCountMechTensile(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var location_name = $("#txtRptLocationTen").val() ? $("#txtRptLocationTen").val() : null;
    var inspection_StartDate_Ten = $("#txtInspectionStartDateTen").val() ? $("#txtInspectionStartDateTen").val() : null;
    var inspection_EndDate_Ten = $("#txtInspectionEndDateTen").val() ? $("#txtInspectionEndDateTen").val() : null;
    var inspector = $("#txtInspectorTen").val() ? $("#txtInspectorTen").val() : null;
    var funance_no = $("#txtFurnaceNumTen").val() ? $("#txtFurnaceNumTen").val() : null;
    var miTensile_Decision = $("#ddlMITensileDecision").val() ? $("#ddlMITensileDecision").val() : null;

   

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/Tensile/GetRowCountForMechTensileDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_Ten,
            INSPECTION_DATE2: inspection_EndDate_Ten, EMPLOYEE_NAME: inspector, FURNACE_NO: funance_no, DECISION: miTensile_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetMechTensileDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableMechTensile"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessMechTensileDetailsGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divMechTensileDailyProcessGridMain").html(response.message);
                        })
                }

            }
            else {
                GetMechTensileDetails(sortFilter);
            } 
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


function GetRowCountFinalInspection(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;


    var location_name = $("#txtLocationCode").val() ? $("#txtLocationCode").val() : null;
    var inspection_StartDate_FI = $("#txtInspectionStartDateFI").val() ? $("#txtInspectionStartDateFI").val() : null;
    var inspection_EndDate_FI = $("#txtInspectionEndDateFI").val() ? $("#txtInspectionEndDateFI").val() : null;
    var inspector = $("#txtInspectorFI").val() ? $("#txtInspectorFI").val() : null;
    var sparkFI_StartDate = $("#txtSparkFIStartDate").val() ? $("#txtSparkFIStartDate").val() : null;
    var sparkFI_EndDate = $("#txtSparkFIEndDate").val() ? $("#txtSparkFIEndDate").val() : null;
    var sparkFI_Operator = $("#txtSparkFIOperator").val() ? $("#txtSparkFIOperator").val() : null;
    var fi_Decision = $("#ddlFICombinedDecision").val() ? $("#ddlFICombinedDecision").val() : null;


    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/FinalInspection/GetRowCountForFinalInspectionDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, LOCATION_NAME: location_name, INSPECTION_DATE1: inspection_StartDate_FI, INSPECTION_DATE2: inspection_EndDate_FI,
            EMPLOYEE_NAME: inspector, START_DATETIME: sparkFI_StartDate, END_DATETIME: sparkFI_EndDate, OPERATOR_NAME: sparkFI_Operator, DECISION: fi_Decision
            

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetFinalInspectionDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableFinalInspection"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GetDailyProcessFinalInspectionDetailsGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divFinalInspectionDailyProcessGridMain").html(response.message);
                        })
                }

            }
            else {
                GetFinalInspectionDetails(sortFilter);
            } 
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}

function GetRowCountGeneralDailyProcess(sortFilter) {
    var ajaxTime = new Date().getTime();
    var chuuban = $("#txtChuubanCom").val() ? $("#txtChuubanCom").val() : null;
    var gyouban = $("#txtGoubanCom").val() ? $("#txtGoubanCom").val() : null;
    var prod_Spec_No1 = $("#txtManufacturingSpecificationNo1Com").val() ? $("#txtManufacturingSpecificationNo1Com").val() : null;
    var prod_Spec_No2 = $("#txtManufacturingSpecificationNo2Com").val() ? $("#txtManufacturingSpecificationNo2Com").val() : null;
    var ver = $("#txtVER").val() ? $("#txtVER").val() : null;
    var customer_name = $("#txtCustomerCOm").val() ? $("#txtCustomerCOm").val() : null;
    var prodname_customer = $("#txtProdNameCustCom").val() ? $("#txtProdNameCustCom").val() : null;
    var internal_name = $("#txtInternalNameCom").val() ? $("#txtInternalNameCom").val() : null;
    var shape = $("#txtMadeShapeRptCom").val() ? $("#txtMadeShapeRptCom").val() : null;
    var dia_Min = $("#txtDiaMin").val() ? $("#txtDiaMin").val() : null;
    var dia_Max = $("#txtDiaMax").val() ? $("#txtDiaMax").val() : null;
    var proddim_W = $("#txtProductDimWCom").val() ? $("#txtProductDimWCom").val() : null;
    var proddim_other = $("#txtProductDimOtherCom").val() ? $("#txtProductDimOtherCom").val() : null;
    var length_Min = $("#txtLengthMin").val() ? $("#txtLengthMin").val() : null;
    var length_Max = $("#txtLengthMax").val() ? $("#txtLengthMax").val() : null;
    var Prod_Length_Other = $("#txtProdLengthOther").val() ? $("#txtProdLengthOther").val() : null;
    var prod_Condition_CD = $("#ddlProdCondition").val() ? $("#ddlProdCondition").val() : null;
    var matl_Type_CBK = $("#ddlMatlTypeCBK").val() ? $("#ddlMatlTypeCBK").val() : null;
    var material_name_maker = $("#txtMaterialNameMakerCom").val() ? $("#txtMaterialNameMakerCom").val() : null;
    var material_shape = $("#txtMaterialShapeCom").val() ? $("#txtMaterialShapeCom").val() : null;
    var matl_Dia_Min = $("#txtMaterialDimMin").val() ? $("#txtMaterialDimMin").val() : null;
    var matl_Dia_Max = $("#txtMaterialDimMax").val() ? $("#txtMaterialDimMax").val() : null;
    var materialdim_W = $("#txtMaterialDimWCom").val() ? $("#txtMaterialDimWCom").val() : null;
    var materialdim_other = $("#txtMaterialDimOtherCom").val() ? $("#txtMaterialDimOtherCom").val() : null;
    var Matl_Length_Min = $("#txtMatlLengthMin").val() ? $("#txtMatlLengthMin").val() : null;
    var Matl_Length_Max = $("#txtMatlLengthMax").val() ? $("#txtMatlLengthMax").val() : null;
    var matl_Condition_CD = $("#ddlMatlCondition").val() ? $("#ddlMatlCondition").val() : null;
    var charge = $("#txtChargeCom").val() ? $("#txtChargeCom").val() : null;
    var allo_Date_Start = $("#txtAllocDateStart").val() ? $("#txtAllocDateStart").val() : null;
    var allo_Date_End = $("#txtAllocDateEnd").val() ? $("#txtAllocDateEnd").val() : null;

    var processKubun = $("#ddlGPProcessKubun").val() ? $("#ddlGPProcessKubun").val() : null;
    var machine_name = $("#txtMachineGP").val() ? $("#txtMachineGP").val() : null;
    var location_name = $("#txtLocationGP").val() ? $("#txtLocationGP").val() : null;

    var beginning_Date_Start = $("#txtStartDateGP1").val() ? $("#txtStartDateGP1").val() : null;
    var beginning_Date_END = $("#txtStartDateGP2").val() ? $("#txtStartDateGP2").val() : null;
    var start_operator = $("#txtStartOperatorGP").val() ? $("#txtStartOperatorGP").val() : null;

    var end_Date1 = $("#txtEndDateGp1").val() ? $("#txtEndDateGp1").val() : null;
    var end_Date2 = $("#txtEndDateGp2").val() ? $("#txtEndDateGp2").val() : null;
    var end_operator = $("#txtEndOperatorGP").val() ? $("#txtEndOperatorGP").val() : null;
    var gp_Decision = $("#ddlDecisionGP").val() ? $("#ddlDecisionGP").val() : null;

  

    var settings = {
        async: false,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/GeneralDailyProcess/GetRowCountForGeneralProcessDailyAnalysis",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CHUUBAN: chuuban, GYOUBAN: gyouban, PROD_SPEC_NO1: prod_Spec_No1, PROD_SPEC_NO2: prod_Spec_No2, PROD_VER: ver,
            CUSTOMER_NAME: customer_name, PROD_NAME_CUST: prodname_customer, INTERNAL_NAME_STRING: internal_name, PROD_SHAPE: shape,
            PROD_DIM_D_MIN: dia_Min, PROD_DIM_D_MAX: dia_Max, PROD_DIM_W: proddim_W, PROD_DIM_OTHER_STRING: proddim_other, PROD_LENGTH_MIN: length_Min,
            PROD_LENGTH_MAX: length_Max, LENGTH_OTHER: Prod_Length_Other, PROD_CONDITION_CD: prod_Condition_CD, MATL_TYPE_CBK: matl_Type_CBK,
            MATL_NAME_MAK: material_name_maker, MATL_SHAPE: material_shape, MATL_DIM_D_MIN: matl_Dia_Min, MATL_DIM_D_MAX: matl_Dia_Max,
            MATL_DIM_W: materialdim_W, MATL_DIM_OTHER: materialdim_other, MATL_LENGTH_MIN: Matl_Length_Min, MATL_LENGTH_MAX: Matl_Length_Max, MATL_CONDITION_CD: matl_Condition_CD,
            CHARGE: charge, ACT_ALLOC_START_DATE: allo_Date_Start, ACT_ALLOC_END_DATE: allo_Date_End, PROCESS_KUBUN_CD: processKubun, MACHINE_NAME: machine_name, LOCATION_NAME: location_name,
            BEGINNING_START_DATE: beginning_Date_Start, BEGINNING_END_DATE: beginning_Date_END, START_OPERATOR: start_operator,
            END_DATE1: end_Date1, END_DATE2: end_Date2, END_OPERATOR: end_operator, DECISION: gp_Decision
           

        })
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var result = response.message;
            var rowLimit = $("#rowLimitValue").val();
            if (parseInt(result) > parseInt(rowLimit)) {
                var message = $("#valMsgMoreDataRowsAvailableInSystem").text().replace("NNNN", rowLimit).replace("NNNN", rowLimit);
                focusFieldID = 'rowLimitValue';
                showMessageDialog(message, focusFieldID, WARNING);

                if (parseInt(rowLimit) > 0) {
                    GetGeneralDailyProcessDetails(sortFilter);
                }
                else {
                    $("#btnExportCSV").attr("DISABLED", "DISABLED");
                    var list = [];
                    clearTable($("#tableGeneralDailyProcess"), "", "GridNormalRow");
                    var settings = {
                        async: true,
                        crossDomain: true,
                        cache: false,
                        dataType: "json",
                        url: "/DailyProcessAnalysisWithCSV/GeneralProcessDailyProcessGrid",
                        method: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(list)
                    }
                    $.ajax(settings)
                        .done(function (response) {
                            $("#divGeneralProcessDailyProcessGridMain").html(response.message);
                        })
                }

            }
            else {
                GetGeneralDailyProcessDetails(sortFilter);
            } 
          

        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $.trim($("#lblErrorMessage_system_exception").text());
            showMessageDialog(message, focusFieldID, ERROR);
        });
}


$("#txtMetallicLablNoCNo,#ddlDRDecision,#ddlDRADecision,#ddlDWDecision,#ddlSTR1Decision,#ddlSTR2Decision,#ddlChamferDecision,#ddlECDecision,#ddlStamppingDecision,#ddlInspectionDecesion,#ddlPostHTDecision,#ddlMIHardnessDecision,#ddlMITensileDecision,#ddlFICombinedDecision,#ddlDecisionGP").focusout(function (event) {
    $("#btnSearch").focus();
});


$('#txtChuubanCom').keypress(function (e) {
    var orderNo = $('#txtChuubanCom').val() ? $('#txtChuubanCom').val() : null;
    var keycode = (e.keyCode ? e.keyCode : e.which);
    if (keycode == 13 && orderNo) {

        var ajaxTime = new Date().getTime();
        var settings = {
            async: false,
            crossDomain: true,
            cache: false,
            dataType: "json",
            url: APIURL + "/Common/MappChuubanAndGyoubanByBarcode?orderNo=" + orderNo,
            method: "POST",
            contentType: "application/json; charset=utf-8",

        }
        $.ajax(settings)
            .done(function (response) {
                responseTime = new Date().getTime() - ajaxTime;
                var jsonResult = response.message;
                if (response.isSuccess) {
                    $('#txtChuubanCom').val(jsonResult.ordeR_NO_IN_CHUUBAN);
                    $('#txtGoubanCom').val(jsonResult.gyoubaN_IN_CHUUBAN);
                    $('#txtManufacturingSpecificationNo1Com').focus();
                } else {
                    $('#txtChuubanCom').val("");
                    $('#txtGoubanCom').val("");
                }

            })
            .fail(function (xhr, textStatus, errorThrown) {
                responseTime = new Date().getTime() - ajaxTime;
                var errorCode = $.parseJSON(xhr.responseText).Message;

                focusFieldID = $(':focus').attr("id");
                message = $("#lblErrorMessageE_STA_ENTRY_014").text().trim();
                showMessageDialog(message, focusFieldID, ERROR);
            });
    }

});