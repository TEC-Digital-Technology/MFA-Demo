using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MfaDemo.Utils.Enums
{
    /// <summary>
    /// 驗證碼的狀態
    /// </summary>
    public enum VerifyCodeStatus
    {
        /// <summary>
        /// 尚未使用
        /// </summary>
        Unused = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        Used = 2
    }
}
