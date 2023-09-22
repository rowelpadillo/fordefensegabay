document.addEventListener("DOMContentLoaded", () => {
    const mngCB = document.querySelector('.adminMaster');

    const kywrdInpt = mngCB.querySelector('.KeywordTextArea');
    const errorKywrd = mngCB.querySelector('.keywordError');

    function checkKeyword() {
        const regexComma = /,/;
        const regexSpace = /\s/;

        if (!regexComma.test(kywrdInpt.value) || regexSpace.test(kywrdInpt.value)) {
            kywrdInpt.classList.add('is-invalid');
            kywrdInpt.classList.remove('is-valid');
            errorKywrd.classList.remove('d-none');
            return false;
        } else {
            kywrdInpt.classList.remove('is-invalid');
            kywrdInpt.classList.add('is-valid');
            errorKywrd.classList.add('d-none');
            return true;
        }
    }

    mngCB.addEventListener("submit", (e) => {

        if (!checkKeyword()) {
            e.preventDefault();
        }
    });

    kywrdInpt.addEventListener('keyup', checkKeyword);
});