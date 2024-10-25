var loginForm = document.getElementById("login");
var mfaForm = document.getElementById("otp");
var loginBtn = document.getElementById("loginBtn");
var mfaMessage = document.getElementById("mfaMessage");
var otpInput = document.getElementById("otpInput");

const showMfaForm = function(email) {
    mfaForm.style.display = "";
    mfaMessage.innerHTML = `已傳送驗證碼至 ${email}`;
}

loginBtn.addEventListener('click', async function() {
    var email = document.getElementById('emailInput').value;
    loginForm.style.display = "none";
    showMfaForm(email);
});

otpInput.addEventListener('input', function (event) {
    // 只允許輸入數字
    otpInput.value = otpInput.value.replace(/\D/g, '');

    // 如果長度超過 6 位，截斷多餘部分
    if (otpInput.value.length > 6) {
        otpInput.value = otpInput.value.slice(0, 6);
    }
});