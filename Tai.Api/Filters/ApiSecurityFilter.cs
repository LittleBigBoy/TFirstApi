using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using System.Web.UI;
using Newtonsoft.Json;
using Tai.Api.Common;
using Tai.Api.Enum;
using Tai.Api.Models;

namespace Tai.Api.Filters
{
    public class ApiSecurityFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            ResultMsg resultMsg = null;
            var request = actionContext.Request;
            string method = request.Method.Method;
            string staffid = string.Empty, timestamp = string.Empty, nonce = string.Empty, signature = string.Empty;
            int id = 0;
            if (request.Headers.Contains("staffid"))
            {
                staffid = HttpUtility.HtmlDecode(request.Headers.GetValues("staffid").FirstOrDefault());
            }
            if (request.Headers.Contains("timestamp"))
            {
                timestamp = HttpUtility.HtmlDecode(request.Headers.GetValues("timestamp").FirstOrDefault());
            }
            if (request.Headers.Contains("nonce"))
            {
                nonce = HttpUtility.HtmlDecode(request.Headers.GetValues("nonce").FirstOrDefault());
            }
            if (request.Headers.Contains("signature"))
            {
                signature = HttpUtility.HtmlDecode(request.Headers.GetValues("signature").FirstOrDefault());
            }
            #region GetToken方法不需要进行签名验证
         
            if (actionContext.ActionDescriptor.ActionName == "GetToken")
            {
                if (string.IsNullOrWhiteSpace(staffid) || !int.TryParse(staffid, out id) ||
                    string.IsNullOrWhiteSpace(timestamp) || string.IsNullOrWhiteSpace(nonce))
                {
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int) StatusCodeEnum.ParameterError;
                    resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                    resultMsg.Data = "";
                    actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                    base.OnActionExecuting(actionContext);
                    return;
                }
                else
                {
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }
            #endregion
            #region 判断请求头中是否包含以下参数

            if (string.IsNullOrWhiteSpace(staffid) || !int.TryParse(staffid, out id) ||
                string.IsNullOrWhiteSpace(timestamp) || string.IsNullOrWhiteSpace(nonce) ||
                string.IsNullOrWhiteSpace(signature))
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
            #region 判断timespan是否有效
     
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
            #region 判断Token是否有效

            Token token = (Token) HttpRuntime.Cache.Get(id.ToString());
            string signToken = string.Empty;
            if (HttpRuntime.Cache.Get(id.ToString()) == null)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int) StatusCodeEnum.TokenInvalid;
                resultMsg.Info = StatusCodeEnum.TokenInvalid.GetEnumText();
                resultMsg.Data = "";
                actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                base.OnActionExecuting(actionContext);
                return;
            }
            else
            {
                signToken = token.SignToken.ToString();
            }
            #endregion
            #region 根据请求类型拼接参数

            NameValueCollection form = HttpContext.Current.Request.Form;
            string data = string.Empty;
            switch (method)
            {
                case "POST":
                    Stream stream = HttpContext.Current.Request.InputStream;
                    StreamReader sr = new StreamReader(stream);
                    data = sr.ReadToEnd();
                    break;
                case "GET":
                    //第一步取出所有的get参数
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    for (int f = 0; f < form.Count; f++)
                    {
                        string key = form.Keys[f];
                        parameters.Add(key, form[key]);
                    }
                    //第二步把字典按key值得字母顺序排序
                    IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
                    IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
                    //第三步把所有参数名和参数值串在一起
                    StringBuilder query = new StringBuilder();
                    while (dem.MoveNext())
                    {
                        var key = dem.Current.Key;
                        var value = dem.Current.Value;
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            query.Append(key).Append(value);
                        }
                    }
                    data = query.ToString();
                    break;
                default:
                    resultMsg = new ResultMsg();
                    resultMsg.StatusCode = (int) StatusCodeEnum.HttpMethodError;
                    resultMsg.Info = StatusCodeEnum.HttpMethodError.GetEnumText();
                    resultMsg.Data = "";
                    actionContext.Response = HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                    base.OnActionExecuting(actionContext);
                    return;
            }
            bool result = SignExtension.Validate(id, timestamp, nonce, signToken, data, signature);
            if (!result)
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int) StatusCodeEnum.HttpRequestError;
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
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }

}