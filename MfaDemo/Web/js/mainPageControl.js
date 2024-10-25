// 取得 cookie 物件
const getCookieAsObject = function() {
    var cookieObject = {};
    document.cookie.split(';').forEach(function(el) {
        var [key, value] = el.split('=');
        cookieObject[key.trim()] = value.trim();
    });
    return cookieObject;
}

var cookie = getCookieAsObject();
var requiredCookie = ['accountId', 'passOtp'];
var activeAuthenticatorBtn = document.getElementById('activeAuthenticatorBtn');
var logoutBtn = document.getElementById('logoutBtn');
var bindAuthenticatorBtn = document.getElementById('bindAuthenticatorBtn');

// 如果沒有綁定 Authenticator，則顯示綁定訊息
if (cookie.isAuthenticatorEnabled != 'true') {
    var bindAuthenticatorMessage = document.getElementById('bindAuthenticatorMessage');
    bindAuthenticatorMessage.style.display = '';
}

// 檢查必要 cookie 是否存在
for (var i = 0; i < requiredCookie.length; i++) {
    if (!cookie[requiredCookie[i]]) {
        window.location.href = 'index.html';
    }
}

// 顯示綁定 Authenticator 頁面
var showBindAuthenticatorPage = function(qrCodeBase64) {
    var qrCode = `data:image/jpeg;base64,${qrCodeBase64}`;
    var mainPage = document.getElementById('mainPage');
    var bindAuthenticator = document.getElementById('bindAuthenticator');
    var qrCodeImg = document.getElementById('qrCodeImg');
    mainPage.style.display = 'none';
    bindAuthenticator.style.display = '';
    qrCodeImg.setAttribute('src', qrCode);
}

// 綁定 Authenticator
bindAuthenticatorBtn.addEventListener('click', function() {
    fetch(`api/Account/CreateTotpQrCode?accountId=${cookie.accountId}`, {
        method: 'POST',
    }).then(response => response.json())
    .then(data => {
        showBindAuthenticatorPage(data.QRCode);
    });
});

// 驗證 OTP 輸入
otpInput.addEventListener('input', function (event) {
    otpInput.value = otpInput.value.replace(/\D/g, '');

    if (otpInput.value.length > 6) {
        otpInput.value = otpInput.value.slice(0, 6);
    }
});

// 啟用 Authenticator
activeAuthenticatorBtn.addEventListener('click', function() {
    var bindAuthenticator = document.getElementById('bindAuthenticator');
    var bindAuthenticatorMessage = document.getElementById('bindAuthenticatorMessage');
    fetch(`api/Account/ValidateTotp?accountId=${cookie.accountId}&otp=${otpInput.value}`, {
        method: 'POST',
    }).then(response => response.json())
    .then(data => {
        if (data.ResultCode == '0000') {
            mainPage.style.display = '';
            bindAuthenticator.style.display = 'none';
            bindAuthenticatorMessage.style.display = 'none';
        } else {
            alert('驗證 OTP 錯誤');
        }
    });
});

// 登出
logoutBtn.addEventListener('click', function() {
    window.location.href = 'index.html';
});

