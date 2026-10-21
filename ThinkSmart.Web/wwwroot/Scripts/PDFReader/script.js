

var curScale = 1.0;
var currentPage = 1;
var totalPageCount = 0;

document.querySelector("#fileUpload").addEventListener("change", function (e) {
  // var tmppath = URL.createObjectURL(e.target.files[0]);
  document.querySelector("#pages").innerHTML = "";
    //zoomReset();
    debugger;

   
    var file = e.target.files[0];



  if (file.type !== "application/pdf") {
    alert(file.name + " n'est pas un fichier PDF.");
    return;
  }
  curScale = 1.0;
  currentPage = 1;
    totalPageCount = 0;
    //var url = "/MakerMillSheet/PreviewUpload";
    ////Add the Field values to FormData object.
    //var formData = new FormData();
    //formData.append("postedFile2", $("#pdf-upload")[0].files[0]);

    //$.ajax({
    //    type: 'POST',
    //    url: url,
    //    data: formData,
    //    processData: false,
    //    contentType: false
    //}).done(function (response) {
    //    if (response.Status === "success") {
    //        $("#lblMessage").html(response.Message);
    //    }
    //});

    //var ajaxTime = new Date().getTime();
   


    loadPdfFile(file);
    document.querySelector("#pages").style.display = "None";
  //zoomReset();
});





//function zoomIn() {
//  if (curScale < 2.0) {
//    curScale += 0.1;
//    updateZoom();
//  }
//}

//function zoomOut() {
//  if (curScale > 0.5) {
//    curScale -= 0.1;
//    updateZoom();
//  }
//}

//function zoomReset() {
//  curScale = 1.0;
//  updateZoom();
//}

//function updateZoom() {
//  document.querySelector("#zoom-percent").innerHTML = Math.round(
//    curScale * 100
//  );

var pages = document.querySelectorAll(".page");
debugger;
  for (var i = 0; i < pages.length; i++) {
    var canvas = pages[i];
    var context = canvas.getContext("2d");
    context.setTransform(1, 0, 0, 1, 0, 0);
    context.scale(curScale, curScale);
    canvas.style.width = canvas.width / curScale + "px";
    canvas.style.height = canvas.height / curScale + "px";
  }



//window.onkeypress = function (e) {
//  if (e.code === "Equal") {
//    zoomIn();
//  }
//  if (e.code === "Minus") {
//    zoomOut();
//  }
//};

