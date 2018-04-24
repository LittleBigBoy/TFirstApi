using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Tai.Api.Common;
using Tai.Api.Enum;
using Tai.Api.Models;

namespace Tai.Api.Controllers
{
    public class ServiceController : ApiController
    {
        private ResultMsg resultMsg = null;
        /// <summary>
        /// 根据用户名获取Token
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string staffId)
        {
            int id = 0;
            //判断参数是否可用
            if (string.IsNullOrWhiteSpace(staffId) || !int.TryParse(staffId,out id))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int) StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }
            Token token = (Token) HttpRuntime.Cache.Get(id.ToString());
            if ((Token)HttpRuntime.Cache.Get(id.ToString()) == null)
            {
                token = new Token();
                token.StaffId = id.ToString();
                token.SignToken = new Guid().ToString();
                token.ExpireTime = DateTime.Now.AddDays(1);
                HttpRuntime.Cache.Insert(token.StaffId.ToString(), token, null, token.ExpireTime, TimeSpan.Zero);
            }
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Success;
            resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
            resultMsg.Data = token;
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }
    }
}
