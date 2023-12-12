document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector('.departmentMaster');

    const ReasonInput = form.querySelector('.rejectReason');
    const rejectError = form.querySelector('.ErrorReject');


    function checkRejectReason() {
        //const regex = /\d/;

        if (ReasonInput.value === "") {
            ReasonInput.classList.add('is-invalid');
            ReasonInput.classList.remove('is-valid');
            rejectError.classList.remove('d-none');
            return false;
        } else {
            ReasonInput.classList.remove('is-invalid');
            ReasonInput.classList.add('is-valid');
            rejectError.classList.add('d-none');
            return true;
        }
    }

    form.addEventListener("submit", (e) => {

        if (!checkRejectReason()) {
            e.preventDefault();
        }

    });

    ReasonInput.addEventListener('keyup', checkName);
});