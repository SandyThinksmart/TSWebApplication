var $btnChuubanSearch;
var $btnSave;
var combinedDecisionOK = 1;
var typeDropDown = "select-one";
var typeText = "text";
var disabled = "disabled";
var WARNING = "warning";
var ERROR = "error";
var SUCCESS = "success";
var DISABLED = "disabled";

$(document).ready(function () {

    window.resize(1880, 980);

    $("#linkIsMemo").click(function (event) {
        location.href = "/MaterialSearchSales2/MaterialSearchSales2";
        window.resizeTo(1190, 810);
    });
});

