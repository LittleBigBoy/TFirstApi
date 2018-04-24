using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using Tai.Api.Common;

namespace Tai.Api.Enum
{
    public enum StatusCodeEnum
    {
        [Text("请求(或处理)成功")] Success = 200,

        [Text("内部请求出错")] Error = 500,

        [Text("未授权标识")] Unauthorized = 401,

        [Text("请求参数不完整或错误")] ParameterError = 400,

        [Text("请求TOKEN失效")] TokenInvalid = 403,

        [Text("HTTP请求类型不合法")] HttpMethodError = 405,

        [Text("HTTP请求不合法，请求有可能被篡改")] HttpRequestError = 406,

        [Text("该Url已经失效")] UrlExpireError = 407,
    }
}