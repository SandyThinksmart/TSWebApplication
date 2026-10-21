
function onLoad() {
    window.resizeTo(1640, 1070); ///1220,1000
    hideBlankRow();
    Mask();
   // window.moveTo(0, 0);
}

function hideBlankRow() {
    $(".hHide").remove();
    $(".dHide").remove();

};

function Mask() {
    $(".maskingDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" }) ;
    $(".maskingDate").mask("9999/99/99", { placeholder: "YYYY/MM/DD" });
}




function setDecimalFormats() {

    $("#txtDimDStart").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3)
    });

    $("#txtDimDEnd").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 3, 3)
    });

    $("#txtLenthFrom").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2)
    });

    $("#txtLenthTo").keyup(function (event) {
        RestrictToValidDecimalFormat($(this), 4, 2)
    });
  
}



$(document).ready(function () {
   
    $.support.cors = true;  // Special case for IE7. To get past weird errors.
    Mask();
    hideBlankRow();
    window.addEventListener("load", function () {

        InitSortPopup();
    });
   
    });

 /****************************************************************/

    function divHeightSet() {
        var winheight = $(window).height();
        var dvheight = winheight - 250;
        var gvhight = $("#<%=grdvNifudaRePrintCancelationDetails.ClientID%>").height();
        if (gvhight < dvheight)
            $("#divData").css("height", gvhight + 100);
        else
            $("#divData").css("height", dvheight);
    }

function scrollEnd() {
    divHeightSet();
    var mydiv = $("#divData");
    mydiv.scrollTop(mydiv.prop("scrollHeight"));
    mydiv.scrollLeft(0);
    var left = $('#divData').scrollLeft();
    var top = $('#divData').scrollTop();
    var dleft = document.getElementById('<%= dataleft.ClientID %>');
    var dtop = document.getElementById('<%= dataTop.ClientID %>');
    dleft.value = left;
    dtop.value = top;
}
function scrllGet() {
    divHeightSet();
    var dleft = document.getElementById('<%= dataleft.ClientID %>');
    var dtop = document.getElementById('<%= dataTop.ClientID %>');
    var hleft = document.getElementById('<%= headlefts.ClientID %>');
    var htop = document.getElementById('<%= headtops.ClientID %>');
    $("#divData").scrollLeft(dleft.value);
    $("#divData").scrollTop(dtop.value);
    $("#divHeader").scrollLeft(hleft.value);
    $("#divHeader").scrollTop(htop.value);
}

function scrollstart() {
    divHeightSet();
    $('#divHeader').scrollLeft($('#divData').scrollLeft());
    var left = $('#divData').scrollLeft();
    var top = $('#divData').scrollTop();
    var dleft = document.getElementById('<%= dataleft.ClientID %>');
    var dtop = document.getElementById('<%= dataTop.ClientID %>');
    dleft.value = left;
    dtop.value = top;
}

function scrollstart1() {
    divHeightSet();
    $('#divData').scrollLeft($('#divHeader').scrollLeft());
    var left = $('#divHeader').scrollLeft();
    var top = $('#divData').scrollTop();
    var dleft = document.getElementById('<%= dataleft.ClientID %>');
    var dtop = document.getElementById('<%= dataTop.ClientID %>');
    dleft.value = left;
    dtop.value = top;
}
/************************************************************************/
function btnSortClose_Click() {
    closepopup();
}
function closepopup() {

    $("#SortPopUpModal").modal('hide');
}
$(document).on('click', '#btnSort', function () {
    
    $("#SortPopUpModal").modal('show');
})
function InitSortPopup() {
    debugger;
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        url: "/TagWiseStockStatusSearch/InitializeSortPopup",
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
}
function initialize() {

       InitSortPopup();
}
function GetSortDirectionSuffix(sortDirection) {
    var sortDirectionText = sortDirection == 0 ? $("#rdoSortAsc").attr("data-text") : $("#rdoSortDsc").attr("data-text");
    return " " + sortDirectionText;
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


        //for (var i = 0; i < selectedColumns.length; i++) {
        //    var text = $("#lstSortFields option[value='" + selectedColumns[i] + "']").text();
        //    $("#lstSortFields option[value='" + selectedColumns[i] + "']").remove();
        //    $('#lstSelectedSortFields').append($("<option></option>")
        //        .attr("value", selectedColumns[i])
        //        .text(text + GetSortDirectionSuffix(parseInt($('input[name=rdSortOrder]:checked').val()))));
        //}
    }
    else {
        alert("Please select the sort field to be added.");
    }
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
    else {
        alert("Please select the sort field to be added.");
    }
    EnableDisableSortButtons();
}
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