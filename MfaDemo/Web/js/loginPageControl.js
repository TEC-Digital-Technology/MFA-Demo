
var mfaForm = document.getElementById('otp');
var loginBtn = document.getElementById('loginBtn');
var otpInput = document.getElementById('otpInput');
var otpSubmitBtn = document.getElementById('otpSubmitBtn');
var useAuthenticatorBtn = document.getElementById('useAuthenticatorBtn');
var useAuthenticatorOtp = false;

// 取得 cookie 物件
const getCookieAsObject = function() {
    var cookieObject = {};
    document.cookie.split(';').forEach(function(el) {
        var [key, value] = el.split('=');
        cookieObject[key.trim()] = value.trim();
    });
    return cookieObject;
}

// 刪除所有 cookie  
function deleteAllCookies() {
    const cookies = document.cookie.split(";");
  
    for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i];
      const eqPos = cookie.indexOf("=");
      const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
      document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    }
  }

deleteAllCookies(); 

// 顯示 MFA 表單
const showMfaForm = function(accountId, isAuthenticatorEnabled) {
    var loginForm = document.getElementById('login');
    var otpMessage = document.getElementById('otpMessage');
    var mfaMessage = document.getElementById('mfaMessage');

    loginForm.style.display = 'none';
    mfaForm.style.display = '';
    mfaMessage.innerHTML = `已傳送驗證碼至您的信箱，請至信箱查看驗證碼`;
    if (isAuthenticatorEnabled) {
        otpMessage.style.display = '';
    }
}

// 登入
loginBtn.addEventListener('click', async function() {
    var username = document.getElementById('usernameInput').value;
    var password = document.getElementById('passwordInput').value;

    fetch(`api/Account/AuthenticateUser?username=${username}&password=${password}`, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(data => {
        if (data.ResultCode == '0000') {
            document.cookie = `accountId=${data.AccountId}`;
            document.cookie = `isAuthenticatorEnabled=${data.IsAuthenticatorEnabled == 'True'}`;
            showMfaForm(data.AccountId, data.IsAuthenticatorEnabled == 'True'); 
            return fetch(`api/Account/SendSmtpOtp?accountId=${data.AccountId}`, {
                method: 'POST'
            });
        } else {
            alert(data.Message);
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.ResultCode != '0000') {
            alert(data.Message);
        }
    }).catch(error => {
        alert(error);
    });
});

// 驗證 OTP 輸入
otpInput.addEventListener('input', function (event) {
    otpInput.value = otpInput.value.replace(/\D/g, '');

    if (otpInput.value.length > 6) {
        otpInput.value = otpInput.value.slice(0, 6);
    }
});

// 提交 OTP
otpSubmitBtn.addEventListener('click', function() {
    var accountId = getCookieAsObject().accountId;
    var otpApiPath = useAuthenticatorOtp ? 
        `api/Account/ValidateTotp?accountId=${accountId}&otp=${otpInput.value}` : 
        `api/Account/ValidateSmtpOtp?accountId=${accountId}&otpCode=${otpInput.value}`;
    fetch(otpApiPath, {
        method: 'POST'
    })
    .then(response => response.json())
    .then(data => {
        if (data.ResultCode == '0000') {
            document.cookie = 'passOtp=true';
            window.location.href = 'main.html';
        } else {
            alert(data.Message);
        }
    }).catch(error => {
        alert(error);
    });
});

// 使用 Authenticator
useAuthenticatorBtn.addEventListener('click', function() {
    var mfaMessage = document.getElementById('mfaMessage');
    useAuthenticatorOtp = true;
    mfaMessage.innerHTML = '請打開 Authenticator 並且輸入上面顯示的 6 位數的驗證碼';
});

