using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Tai.Api.Common;
using Tai.Api.Enum;
using Tai.Api.Models;

namespace Tai.Api.Filters
{
    public class ApiSecurityBaseFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            ResultMsg resultMsg = null;
            var request = actionContext.Request;
            string method = request.Method.Method;
            string username = string.Empty;
            string adminToken = string.Empty;//staffid+signToken+timestamp=admintoken
            string timestamp = string.Empty;
            string signToken = string.Empty;
            if (request.Headers.Contains("username"))
            {
                username = HttpUtility.HtmlDecode(request.Headers.GetValues("username").FirstOrDefault());
            }
            if (request.Headers.Contains("adminToken"))
            {
                adminToken = HttpUtility.HtmlDecode(request.Headers.GetValues("adminToken").FirstOrDefault());
            }
            if (request.Headers.Contains("timestamp"))
            {
                timestamp = HttpUtility.HtmlDecode(request.Headers.GetValues("timestamp").FirstOrDefault());
            }

            #region GetToken方法不需要验证

            if (actionContext.ActionDescriptor.ActionName == "LoginOn")
            {
                //if (string.IsNullOrWhiteSpace(staffid))
                //{
                //    resultMsg = new ResultMsg();
                //    resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                //    resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                //    resultMsg.Data = "";
                //    actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                //    base.OnActionExecuting(actionContext);
                //    return;
                //}
                //else
                //{
                    base.OnActionExecuting(actionContext);
                    return;
                //}
            }

            #endregion

            #region 头参数是否都存在

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(adminToken) ||
                string.IsNullOrWhiteSpace(timestamp))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                base.OnActionExecuting(actionContext);
                return;
            }

            #endregion

            #region timestamp 是否过期

            double ts1 = 0;
            double ts2 = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
            bool timespanvalidate = double.TryParse(timestamp, out ts1);
            double ts = ts2 - ts1;
            bool falg = ts > int.Parse(WebSettingsConfig.UrlExpireTime) * 1000;
            if (falg || (!timespanvalidate))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.UrlExpireError;
                resultMsg.Info = StatusCodeEnum.UrlExpireError.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                base.OnActionExecuting(actionContext);
                return;
            }


            #endregion

            #region 判断token是否有效

            Token token =(Token) HttpRuntime.Cache.Get(username.ToString());
            if (HttpRuntime.Cache.Get(username.ToString()) == null)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int) StatusCodeEnum.TokenInvalid;
                resultMsg.Info = StatusCodeEnum.TokenInvalid.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                base.OnActionExecuting(actionContext);
            }
            else
            {
                signToken = token.SignToken.ToString();
            }

            #endregion

            #region 验证请求参数是否正确

            bool result = SignExtension.ValidateBase(username, signToken, timestamp, adminToken);
            if (!result)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.HttpRequestError;
                resultMsg.Info = StatusCodeEnum.HttpRequestError.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                base.OnActionExecuting(actionContext);
                return;
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
            #endregion
        }
    }
}