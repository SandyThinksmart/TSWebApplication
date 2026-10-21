$("#btnChooseTextFile").click(function () {

    $("#hiddenTextFile").click();
});

$("#hiddenTextFile").on("change", function () {
    debugger;
    const file = this.files[0];

    if (file) {
        $("#selectTextFile").val(file.name);
    } else {
        $("#selectTextFile").val("");
    }
});

function ReadJFENyukoNyukaBatch() {
    var ajaxTime = new Date().getTime();
    var fileInput = document.getElementById("hiddenTextFile");

    if (fileInput.files.length === 0) {
        return;
    }

    var formData = new FormData();
    formData.append("file", fileInput.files[0]);
    formData.append("futouCd", $("#ddlFutouLocDropdown").val());

    var jsonData = "{"
        + "USER_NAME" + " : '" + userName
        + "'}";
    var settings = {
        async: true,
        crossDomain: true,
        cache: false,
        dataType: "json",
        url: APIURL + "/JFENyukoNyukaBatch/ReadCSVData",
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: formData
    }
    $.ajax(settings)
        .done(function (response) {
            responseTime = new Date().getTime() - ajaxTime;
            var responce = response.message;
        })
        .fail(function (xhr, textStatus, errorThrown) {
            responseTime = new Date().getTime() - ajaxTime;
            var errorCode = $.parseJSON(xhr.responseText).Message;

            focusFieldID = $(':focus').attr("id");
            message = $("#lblErrorMessageE_HARDNESS_EDIT_DELETE_001").text().trim();
            showMessageDialog(message, focusFieldID, ERROR);
        });
}
