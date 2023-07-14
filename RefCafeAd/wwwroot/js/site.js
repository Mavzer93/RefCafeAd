// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function Validation() {
    var username = document.getElementById('Username').value;
    var password = document.getElementById('Password').value;
    if (username === "" || password === "") {
        alert("Kutucuklar boş bırakılamaz");
        return false;
    }
    return true;
}