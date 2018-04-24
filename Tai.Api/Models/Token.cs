﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tai.Api.Models
{
    public class Token
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string StaffId { get; set; }

        /// <summary>
        /// 用户名对应签名Token
        /// </summary>
        public string SignToken { get; set; }

        /// <summary>
        /// Token过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}