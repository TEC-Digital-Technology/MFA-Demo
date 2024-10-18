using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace MfaDemo.WebApi
{
    /// <summary>
    /// API 控制器基底型別
    /// </summary>
    public abstract class ApiControllerBase : ApiController
    {
        /// <summary>
        /// 初始化 API 控制器的基底型別
        /// </summary>
        protected ApiControllerBase() { }

        /// <summary>
        /// 建立回應物件
        /// </summary>
        /// <param name="code">回傳代號</param>
        /// <param name="additionalProperties">附加屬性集合</param>
        /// <returns></returns>
        protected HttpResponseMessage GenerateResponse(string code, params (string Key, string Value)[] additionalProperties)
        {
            string message;
            switch (code.ToLower())
            {
                case "0000":
                    message = "成功";
                    break;
                case "1001":
                    message = "帳號已存在";
                    break;
                default:
                    throw new NotImplementedException($"尚未實作代號「{code}」對應的訊息({nameof(message)})。");
            }
            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                {"ResultCode", code},
                {"Message", message}
            };
            additionalProperties.ToList().ForEach(additionalProperty => result.Add(additionalProperty.Key, additionalProperty.Value));
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json")
            };
        }
        /// <summary>
        /// 建立處理成功( code = 0000 )的回應物件
        /// </summary>
        /// <param name="additionalProperties">附加屬性集合</param>
        /// <returns></returns>
        protected HttpResponseMessage GenerateResponse(params (string Key, string Value)[] additionalProperties)
        {
            return this.GenerateResponse("0000", additionalProperties);
        }
    }
}