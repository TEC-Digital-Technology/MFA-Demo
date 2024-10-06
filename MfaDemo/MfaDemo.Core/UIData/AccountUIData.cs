using MfaDemo.Adapter.DataSource;
using MfaDemo.Core.Infos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MfaDemo.Core.UIData
{
    /// <summary>
    /// 帳號資料管理類別
    /// </summary>
    public class AccountUIData
    {
        /// <summary>
        /// 新增帳號並回傳代表該筆資料的 ID
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="email">電子郵件位址</param>
        /// <returns></returns>
        public async Task<AccountInfo> AddAccountAsync(string username, string password, string email)
        {
            AccountInfo result = new AccountInfo()
            {
                CreatedDateTime = DateTimeOffset.Now,
                Email = email,
                Id = Guid.NewGuid(),
                Password = password,
                TotpSecretKey = null,
                Username = username
            };
            using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
            {
                mfaDemoEntities.Account.Add(this.ConvertToAccount(result));
                await mfaDemoEntities.SaveChangesAsync();
            }
            return result;
        }
        /// <summary>
        /// 透過登入帳號取得帳號資料
        /// </summary>
        /// <param name="username">登入帳號</param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountAsync(string username)
        {
            using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
            {
                var account = await mfaDemoEntities.Account.FirstOrDefaultAsync(t => t.Username == username);
                if (account is null)
                {
                    return null;
                }
                return this.ConvertToAccountInfo(account);
            }
        }
        /// <summary>
        /// 透過帳號 ID 取得帳號資料
        /// </summary>
        /// <param name="username">登入帳號</param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountAsync(Guid accountId)
        {
            using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
            {
                var account = await mfaDemoEntities.Account.FindAsync(accountId);
                if (account is null)
                {
                    return null;
                }
                return this.ConvertToAccountInfo(account);
            }
        }
        /// <summary>
        /// 設定指定帳號的 TOTP Secret Key
        /// </summary>
        /// <param name="accountId">帳號 ID</param>
        /// <param name="secretKey">新的 Secret Key</param>
        /// <returns></returns>
        public async Task SetTotpSecretKeyAsync(Guid accountId, string secretKey)
        {
            using (MfaDemoEntities mfaDemoEntities = new MfaDemoEntities())
            {
                var account = await mfaDemoEntities.Account.FindAsync(accountId) ?? throw new ArgumentException($"查無指定 ID({accountId}) 的帳號資料", nameof(accountId));
                account.TotpSecretKey = secretKey;
                await mfaDemoEntities.SaveChangesAsync();
            }
        }
        #region Private Members
        private AccountInfo ConvertToAccountInfo(Account account)
        {
            return new AccountInfo()
            {
                Username = account.Username,
                CreatedDateTime = account.CreatedDateTime,
                TotpSecretKey = account.TotpSecretKey,
                Email = account.EmailAddress,
                Id = account.ID,
                Password = account.Password
            };
        }
        private Account ConvertToAccount(AccountInfo accountInfo)
        {
            return new Account()
            {
                Username = accountInfo.Username,
                CreatedDateTime = accountInfo.CreatedDateTime,
                TotpSecretKey = accountInfo.TotpSecretKey,
                EmailAddress = accountInfo.Email,
                ID = accountInfo.Id,
                Password = accountInfo.Password
            };
        }
        #endregion
    }
}
