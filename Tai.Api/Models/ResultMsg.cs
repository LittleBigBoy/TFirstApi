using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tai.Api.Models
{
    public class ResultMsg
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public object Data { get; set; }
    }
}