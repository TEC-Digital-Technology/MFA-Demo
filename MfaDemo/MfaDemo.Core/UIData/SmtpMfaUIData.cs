using MfaDemo.Adapter.DataSource;
using MfaDemo.Core.Infos;
using MfaDemo.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MfaDemo.Core.UIData
{
    /// <summary>
    /// 處理 SMTP MFA 的商業邏輯類別
    /// </summary>
    public class SmtpMfaUIData
    {
        /// <summary>
        /// 新增指定帳號資料的 MFA 紀錄
        /// </summary>
        /// <param name="accountId">帳號 ID</param>
        /// <param name="expirySeconds">驗證碼過期時間(秒數)</param>
        /// <returns>回傳該筆記錄的 ID</returns>
        public async Task<Guid> AddMfaRecordAsync(Guid accountId, int expirySeconds = 180)
        {
            AccountUIData accountUIData = new AccountUIData();
            var account = await accountUIData.GetAccountAsync(accountId) ?? throw new ArgumentException($"查無指定 ID({accountId}) 的帳號資料", nameof(accountId));
            SmtpMfaRecordInfo result = new SmtpMfaRecordInfo()
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                VerifyCode = new String("0123456789".OrderBy(t => Guid.NewGuid().GetHashCode()).Take(6).ToArray()),
                CodeStatus = VerifyCodeStatus.Unused,
                CreatedDateTime = DateTime.Now,
                ExpiryDateTime = DateTime.Now.AddSeconds(expirySeconds)
            };
            using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
            {
                mfaDemoEntities.SmtpMfaRecord.Add(this.ConvertToSmtpMfaRecord(result));
                await mfaDemoEntities.SaveChangesAsync();
            }
            return result.Id;
        }
        /// <summary>
        /// 取得指定帳號的最後一筆未使用且未過期的驗證碼資料，若沒有資料則回傳 <c>null</c> 參考
        /// </summary>
        /// <param name="accountId">帳號 ID</param>
        /// <returns></returns>
        public async Task<SmtpMfaRecordInfo> GetLastestRecordAsync(Guid accountId)
        {
            AccountUIData accountUIData = new AccountUIData();
            var account = await accountUIData.GetAccountAsync(accountId) ?? throw new ArgumentException($"查無指定 ID({accountId}) 的帳號資料", nameof(accountId));
            using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
            {
                var record = mfaDemoEntities.SmtpMfaRecord
                    .Where(t => t.ExpiryDateTime > DateTimeOffset.Now && t.AccountID == account.Id && t.CodeStatus == (byte)VerifyCodeStatus.Unused)
                    .OrderByDescending(t => t.CreatedDateTime)
                    .FirstOrDefault();
                if (record is null)
                {
                    return null;
                }
                return this.ConvertToSmtpMfaRecordInfo(record);
            }
        }
        /// <summary>
        /// 驗證指定帳號的 SMTP OTP 是否正確
        /// </summary>
        /// <param name="accountId">要驗證的帳號 ID</param>
        /// <param name="verifyCode">驗證碼</param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(Guid accountId, string verifyCode)
        {
            var activatedRecordInfo = await this.GetLastestRecordAsync(accountId);
            if (activatedRecordInfo is null)
            {
                return false;//沒有資料時視為驗證失敗
            }
            if (activatedRecordInfo.VerifyCode == verifyCode)
            {
                //驗證碼正確
                using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
                {
                    var currentRecord = (await mfaDemoEntities.SmtpMfaRecord.FindAsync(activatedRecordInfo.Id));
                    currentRecord.CodeStatus = (byte)VerifyCodeStatus.Used;
                    await mfaDemoEntities.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
        #region Private Members
        private SmtpMfaRecordInfo ConvertToSmtpMfaRecordInfo(SmtpMfaRecord smtpMfaRecord)
        {
            return new SmtpMfaRecordInfo()
            {
                AccountId = smtpMfaRecord.AccountID,
                CreatedDateTime = smtpMfaRecord.CreatedDateTime,
                CodeStatus = (VerifyCodeStatus)smtpMfaRecord.CodeStatus,
                ExpiryDateTime = smtpMfaRecord.ExpiryDateTime,
                Id = smtpMfaRecord.ID,
                VerifyCode = smtpMfaRecord.VerifyCode
            };
        }
        private SmtpMfaRecord ConvertToSmtpMfaRecord(SmtpMfaRecordInfo smtpMfaRecordInfo)
        {
            return new SmtpMfaRecord()
            {
                AccountID = smtpMfaRecordInfo.AccountId,
                CreatedDateTime = smtpMfaRecordInfo.CreatedDateTime,
                CodeStatus = (byte)smtpMfaRecordInfo.CodeStatus,
                ExpiryDateTime = smtpMfaRecordInfo.ExpiryDateTime,
                ID = smtpMfaRecordInfo.Id,
                VerifyCode = smtpMfaRecordInfo.VerifyCode
            };
        }
        #endregion
    }
}
