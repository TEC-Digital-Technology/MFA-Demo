using MfaDemo.Core.UIData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            return base.GenerateResponse();
        }

        /// <summary>
        /// 發送信件 OTP 驗證碼
        /// </summary>
        /// <param name="accountId">帳號 ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SendSmtpOtp(Guid accountId)
        {
            return base.GenerateResponse();
        }
    }
}
