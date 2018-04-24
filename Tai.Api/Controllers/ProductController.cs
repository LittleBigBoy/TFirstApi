using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Tai.Api.Common;
using Tai.Api.Enum;
using Tai.Api.Models;
using Tai.Api.Models.Product;

namespace Tai.Api.Controllers
{
    public class ProductController : ApiController
    {
        private ResultMsg resultMsg = null;
        public HttpResponseMessage GetProduct(string id)
        {
            var product = new Product {Id = 110, Name = "HH", Price = 92, Count = 1};
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int) StatusCodeEnum.Success;
            resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
            resultMsg.Data = product;
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }
    }
}