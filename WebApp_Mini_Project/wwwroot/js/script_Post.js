// ฟังก์ชั่น joinRoom
function joinRoom(id_room, countPersons, maxPersons) {
    var personCountElement = document.getElementById('person-count-' + id_room);
    var button = document.getElementById('button-' + id_room);

    if (personCountElement && button) {
        var currentCount = parseInt(personCountElement.innerText.split('/')[0]);
        var maxPersons = parseInt(personCountElement.innerText.split('/')[1]);
        if (currentCount < maxPersons) {
            currentCount++;
            personCountElement.innerText = currentCount + '/' + maxPersons;
        }
        else if (currentCount >= maxPersons) {
            button.style.backgroundColor = "red";
        }
    } else {
        console.error("Element not found: person-count-" + id_room + " or join-button-" + id_room);
    }
};


// ฟังก์ชันเปิด/ปิด popupchat
function toggleChatPopup() {
   var chatPopup = document.getElementById("chatPopup");
   if (chatPopup.style.display === "none" || chatPopup.style.display === "") {
      chatPopup.style.display = "block";
 } else {
      chatPopup.style.display = "none";
       }
};

function sendMessage() {
   var chatBody = document.getElementById("chatBody");
   var chatMessage = document.getElementById("chatMessage");

   if (chatMessage.value !== "") {
       var newMessage = document.createElement("div");
       newMessage.textContent = chatMessage.value;
       chatBody.appendChild(newMessage);

       chatMessage.value = ""; // ล้างข้อความใน input หลังจากส่งข้อความ
     }
};









