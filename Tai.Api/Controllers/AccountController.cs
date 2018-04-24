using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Tai.Api.Common;
using Tai.Api.Domain.Base;
using Tai.Api.Domain.Interface;
using Tai.Api.Enum;
using Tai.Api.Models;

namespace Tai.Api.Controllers
{
    public class AccountController : ApiController
    {
        public ResultMsg resultMsg = null;

        public HttpResponseMessage LoginOn(string userName, string passWord, string timestamp, bool remeberMe = false,
            string returnUrl = "")
        {

            if (!MemberShipBase.GetMemberShipServiceInstance().ValidateLogin(userName, passWord))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.Unauthorized;
                resultMsg.Info = StatusCodeEnum.Unauthorized.GetEnumText();
                resultMsg.Data = "请输入正确的用户名或密码!";
                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }
            var data = MemberShipBase.GetMemberShipServiceInstance().GetToken(userName, timestamp, remeberMe);
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int) StatusCodeEnum.Success;
            resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
            resultMsg.Data = data;
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }

        public HttpResponseMessage LoginOut(string userName)
        {
            bool loginOut = MemberShipBase.GetMemberShipServiceInstance().LoginOut(userName);
            if (loginOut)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int) StatusCodeEnum.Success;
                resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
                resultMsg.Data = "登出成功!";
            }
            else
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.Error;
                resultMsg.Info = StatusCodeEnum.Error.GetEnumText();
                resultMsg.Data = "退出出错!";
            }
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }

        public HttpResponseMessage GetUserInfo(string userName)
        {
            var data = MemberShipBase.GetMemberShipServiceInstance().GetUserInfo(userName);
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Success;
            resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
            resultMsg.Data = data;
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }

        public HttpResponseMessage GetProduct(string id)
        {
            
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Success;
            resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
            resultMsg.Data = "";
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }
    }
}
