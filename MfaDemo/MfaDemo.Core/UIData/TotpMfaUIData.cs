using MfaDemo.Adapter.DataSource;
using MfaDemo.Core.Infos;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MfaDemo.Core.UIData
{
    /// <summary>
    /// 處理 TOTP MFA 的邏輯處理類別
    /// </summary>
    public class TotpMfaUIData
    {
        /// <summary>
        /// 產生 Totp 的 Uri
        /// </summary>
        /// <param name="accountId">使用者 ID</param>
        /// <returns></returns>
        public async Task<OtpUri> GenerateTotpUri(Guid accountId)
        {
            AccountUIData accountUIData = new AccountUIData();
            AccountInfo accountInfo = await accountUIData.GetAccountAsync(accountId);
            string secretKey = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey());
            await accountUIData.SetTotpSecretKeyAsync(accountInfo.Id, secretKey);
            return new OtpUri(OtpType.Totp, secretKey, accountInfo.Email);
        }

        /// <summary>
        /// 驗證 TOTP
        /// </summary>
        /// <param name="accountId">使用者 ID</param>
        /// <param name="otp">OTP</param>
        /// <returns></returns>
        public async Task<bool> VerifyTotpCode(Guid accountId, string otp) 
        {
            AccountUIData accountUIData = new AccountUIData();
            AccountInfo accountInfo = await accountUIData.GetAccountAsync(accountId);
            Totp totp = new Totp(Base32Encoding.ToBytes(accountInfo.TotpSecretKey));
            return totp.VerifyTotp(otp, out long timeStepMatched);
        }
    }
}
