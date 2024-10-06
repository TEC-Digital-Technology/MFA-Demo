using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MfaDemo.Core.Infos
{
    /// <summary>
    /// 代表帳號資料的類別
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// 設定或取得帳號 ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 設定或取得帳號
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 設定或取得密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 設定或取得電子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 設定或取得 TOTP 密鑰
        /// </summary>
        public string TotpSecretKey { get; set; }
        /// <summary>
        /// 設定或取得資料建立時間
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
