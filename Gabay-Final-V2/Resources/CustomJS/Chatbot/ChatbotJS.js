document.addEventListener("DOMContentLoaded", () => {
    const studentFom = document.querySelector('.studentMaster');

    function scrollToBottom() {
        var chatContainer = studentFom.querySelector('.chatContainer');
        chatContainer.scrollTop = chatContainer.scrollHeight;
    }
    scrollToBottom();
});