// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ฟังก์ชันเปิด/ปิด popupchat
function toggleChatPopup() {
    var chatPopup = document.getElementById("chatPopup");
    if (chatPopup.style.display === "none" || chatPopup.style.display === "") {
        chatPopup.style.display = "block";
    } else {
        chatPopup.style.display = "none";
    }
};

function changeIconColor(color) {
    document.getElementById('chatIcon').querySelector('path').setAttribute('fill', color);
}
