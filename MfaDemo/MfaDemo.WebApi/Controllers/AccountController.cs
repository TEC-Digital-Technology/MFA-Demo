using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MfaDemo.WebApi.Controllers
{
    public class AccountController : ApiController
    {
        /// <summary>
        /// 測試用的
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> SomeAction()
        {
            return "123";
        }
    }
}
