const popupBtn = document.getElementById('openPopupBtn');
const popup = document.getElementById('popup');
const closePopup = document.getElementById('closePopup');

popupBtn.addEventListener('click', function () {
   popup.style.display = 'flex';
});

closePopup.addEventListener('click', function () {
   popup.style.display = 'none';
});



// ฟังก์ชันเปิด/ปิด popup
function toggleChatPopup() {
   var chatPopup = document.getElementById("chatPopup");
   if (chatPopup.style.display === "none" || chatPopup.style.display === "") {
      chatPopup.style.display = "block";
 } else {
      chatPopup.style.display = "none";
       }
}

    function sendMessage() {
        var chatBody = document.getElementById("chatBody");
        var chatMessage = document.getElementById("chatMessage");

        if (chatMessage.value !== "") {
            var newMessage = document.createElement("div");
            newMessage.textContent = chatMessage.value;
            chatBody.appendChild(newMessage);

            chatMessage.value = ""; // ล้างข้อความใน input หลังจากส่งข้อความ
        }
    }

