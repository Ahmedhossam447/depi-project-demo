// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteconfirm(UserId , IsdDeletecClicked) {
    var spanid = "deleteConfirmSpan" + UserId;
    var deletespan = "deletespan" + UserId;
    if (IsdDeletecClicked) {
        document.getElementById(spanid).style.display = "block";
        document.getElementById(deletespan).style.display = "none";
    }
    else {
        document.getElementById(spanid).style.display = "none";
        document.getElementById(deletespan).style.display = "block";
    }
}