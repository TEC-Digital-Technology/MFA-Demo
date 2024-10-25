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
if (!(cookie.isAuthenticatorEnabled == 'true')) {
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
    // TODO: 呼叫 API 產生 QR Code
    showBindAuthenticatorPage('iVBORw0KGgoAAAANSUhEUgAAAmIAAAJiAQAAAABMLZNLAAAEwUlEQVR4nO2aQW7kQAhFkfoAOZKv7iP5AJaYNvCB6kQzs8im0K+F0+6iXmXzBR9a9BfXKaSRRhpppJFGGmmkkUbaOJrEej3fxNLz647dQ2+R43rv6vXe0Mvi4tXXQRppM2j28kSJPIwH5MdPDwX32X0/1DecWwDSSBtAeySSx8FVFxDuCgFZ8PvvjbgMIY20QTRj+KcDx/1kph/fcAZppE2m3WB46LKO5EYBRhpp02g4HsvyzRfyyKGhtgo2xj/rN9JI244WC4nj/x8lOdJIm0CrhexRNjuSjp15qy10597kY5FG2v40a0OFskTqgQ1BsZWv5lLshKCTSxpp+9NiL9tQz6qTqrDetotHX6SRNoQWC51XKafh2pGXdo0Z3Acaz60C00IaafvTPBRjubAWHhDVlWcZxTV+9QkFkkbaJFoEfNZU0gQk+dsO+97MSISQRtoIWs0oEHU32XjSiWSC3KJXhy/KIo20fWlNWQb345497iYqp5n1PvFYUhJppO1OM6+dNVUf1YEbukNfVrKFhbOkkTaB1o/7J4GKJJASXdtXi4NpUSWNtCm08tqRb/qhmEfAei+vacxJI20IDQJqdrwlHZRiCM4FUX0oizTStqUdGRp6updKarEbHhivivRDGmkjaGHCXVSKJtWFRqz3ojw+0o8Ipt29ACONtAG0rK4wtm4a+1CRD7WTJqU20kjbneYCqnlE+50T2lBuPLqKXj+EkEba7jRPHOgxBdKOWxS0U7cupdg390EaafvS8nh4chFISfpKp1H12I9OnDTSdqW53YB2JCqu50Nu1Ouz2oRPSmikkbY9zRXjAdGXRTJJT76kmmbWj4s00ubQbLmAsjnrnryUhUc0rqAnqVkGaaQNoH0tyFDR3SuumHErrHdWXD/4etJI25aWCUYEuSVsdpqRiosEI3Hhs0gjbQitRhH2yU+aJ/eGFAqrQNajjDlppI2gQTH2TSQONGxv70/5wzWGzBM/+vhWcZFG2s40wRTCTHhO7lxPEFDrRUWTCmdJI20GTXMcLdJKLBRWZ3kTb1fJMvIW3EUaadvTmmw81ZhiWq9W4pNm2eWiqrOkkTaElgLKgKXb9HLJLd2r7FnhGGmk7U9TxYAu4JpTOriPDyl5iKWa81vOIo20zWmCliz2YkWWwQ3VpIp9jLdJI20AzRNMsxYCobkJz9rL6zHMMpoxJ420ETQXkKcaaMc3MrSKLYsDDeJT0kgbQWsZRTUVk3nEbjh9eJFIXBhc0kibQdOKf0K9JWtO/Fpe3W6gk9vuIo20GTSRBooU0v2FbXhXqn7O9JeKizTSdqVpzOHUu01+KBUTuSUbsXpFw7asyqIs0kjbmFY6iahXHs859bVs1DogL9JIG0GDitohEeQbLVHZ19WuQu1FGmkzaBAJerAKPVXrNk666w7r3USlpJE2g+bfZC+qRtTlL+rC7OTi8dGVIo20nWmWM6p0KmMOCmovXChtA4s00ubRBMVWZJ4bNOtZSTSpPCWdiSSNtEG01pw9NBjetX3yjaZjjx94yMc/QRpp29PsT3mJxW6gQ2sgybmFHYdtJ420IbRYEIu0rlQcAjd0h/6U+F2+SCNte9ovLdJII4000kgjjTTSSCNtFO0PxURByaqxzeoAAAAASUVORK5CYII=');
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
    mainPage.style.display = '';
    bindAuthenticator.style.display = 'none';
    bindAuthenticatorMessage.style.display = 'none';
    // TODO: 呼叫 API 啟用 Authenticator
});

// 登出
logoutBtn.addEventListener('click', function() {
    window.location.href = 'index.html';
});

