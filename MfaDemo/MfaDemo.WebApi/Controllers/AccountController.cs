using MfaDemo.Core.UIData;
using Newtonsoft.Json;
using OtpNet;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace MfaDemo.WebApi.Controllers
{
    /// <summary>
    /// 帳號控制器
    /// </summary>
    public class AccountController : ApiControllerBase
    {
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="email">信箱</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> AddUser(string username, string password, string email)
        {
            AccountUIData accountUIData = new AccountUIData();
            if (await accountUIData.GetAccountAsync(username) != null)
            {
                return base.GenerateResponse("1001");//帳號存在
            }
            var accountInfo = await accountUIData.AddAccountAsync(username, password, email);
            return base.GenerateResponse(("AccountId", accountInfo.Id.ToString()));
        }

        /// <summary>
        /// 驗證使用者帳號及密碼
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> AuthenticateUser(string username, string password)
        {
            AccountUIData accountUIData = new AccountUIData();
            var accountInfo = await accountUIData.GetAccountAsync(username);
            if (accountInfo == null || String.Compare(accountInfo.Password, password) != 0)
            {
                return base.GenerateResponse("1002");
            }
            bool isAuthenticatorEnabled = !String.IsNullOrEmpty(accountInfo.TotpSecretKey);
            return base.GenerateResponse(("AccountId", accountInfo.Id.ToString()), ("IsAuthenticatorEnabled", isAuthenticatorEnabled.ToString()));
        }

        /// <summary>
        /// 發送信件 OTP 驗證碼
        /// </summary>
        /// <param name="accountId">帳號 ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SendSmtpOtp(Guid accountId)
        {
            SmtpMfaUIData smtpMfaUIData = new SmtpMfaUIData();
            AccountUIData accountUIData = new AccountUIData();
            var accountInfo = await accountUIData.GetAccountAsync(accountId);
            await smtpMfaUIData.AddMfaRecordAsync(accountId, 180);
            var smtpRecodrd = await smtpMfaUIData.GetLastestRecordAsync(accountId);
            using (SmtpClient smtpClient = new SmtpClient("localhost", 25))
            {
                smtpClient.Credentials = new NetworkCredential("NoReplyExample", "aY0zwE_8MiRw");
                smtpClient.Send("NoReply@example.com", accountInfo.Email, "Verify Your Account", $"Please verify your account by the code \"{smtpRecodrd.VerifyCode}\"");
            }
            return base.GenerateResponse();
        }

        /// <summary>
        /// 檢查信件 OTP 驗證碼
        /// </summary>
        /// <param name="accountId">帳號 ID</param>
        /// <param name="otpCode">OTP</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ValidateSmtpOtp(Guid accountId, string otpCode)
        {
            SmtpMfaUIData smtpMfaUIData = new SmtpMfaUIData();
            if (!await smtpMfaUIData.ValidateAsync(accountId, otpCode))
            {
                return base.GenerateResponse("1003");
            }
            return base.GenerateResponse();
        }

        /// <summary>
        /// 建立 Totp 的 QR Code
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> CreateTotpQrCode(Guid accountId) 
        {
            TotpMfaUIData totpMfaUIData = new TotpMfaUIData();
            OtpUri otpUri = await totpMfaUIData.GenerateTotpUri(accountId);
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(otpUri.ToString(), QRCodeGenerator.ECCLevel.Q);
            Base64QRCode base64QRCode = new Base64QRCode(qRCodeData);
            return base.GenerateResponse(("QRCode", base64QRCode.GetGraphic(10)));
        }

        /// <summary>
        /// 驗證 OTP
        /// </summary>
        /// <param name="accountId">使用者 ID</param>
        /// <param name="otp">OTP</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ValidateTotp(Guid accountId, string otp) 
        {
            TotpMfaUIData totpMfaUIData = new TotpMfaUIData();
            if (!await totpMfaUIData.VerifyTotpCode(accountId, otp)) 
            {
                return base.GenerateResponse("1003");
            }

            return base.GenerateResponse();
        }
    }
}
