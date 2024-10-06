using MfaDemo.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MfaDemo.Core.Infos
{
    public class SmtpMfaRecordInfo
    {
        /// <summary>
        /// 設定或取得 SMTP MFA 的資料 ID
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// 設定或取得關聯的帳號 ID
        /// </summary>
        public Guid AccountId { set; get; }
        /// <summary>
        /// 設定或取得驗證碼
        /// </summary>
        public string VerifyCode { set; get; }
        /// <summary>
        /// 設定或取得驗證碼狀態
        /// </summary>
        public VerifyCodeStatus CodeStatus { set; get; }
        /// <summary>
        /// 設定或取得到期時間
        /// </summary>
        public DateTimeOffset ExpiryDateTime { set; get; }
        /// <summary>
        /// 設定或取得資料建立時間
        /// </summary>
        public DateTimeOffset CreatedDateTime { set; get; }
    }
}
