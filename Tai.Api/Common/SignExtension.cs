using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Tai.Api.Common
{
    public class SignExtension
    {
        public static bool Validate(int staffId, string timeStamp, string nonce, string token, string data,
            string signature)
        {
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = staffId + timeStamp + nonce + token + data;
            //字符串按照升序排列
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //将二进制转化为大写的十六进制
            StringBuilder resultStr = new StringBuilder();
            foreach (var c in md5Val)
            {
                resultStr.Append(c.ToString("X2"));
            }
            return resultStr.ToString().ToUpper() == signature;
        }

        public static bool ValidateBase(string staffid, string token, string timestamp, string admintoken)
        {
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = staffid + admintoken + timestamp;
            //字符串按照升序排列
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //将二进制转化为大写的十六进制
            StringBuilder resultStr = new StringBuilder();
            foreach (var c in md5Val)
            {
                resultStr.Append(c.ToString("X2"));
            }
            return resultStr.ToString().ToUpper() == token;
        }

        public static string GetSignToken(string staffid, string adminToken, string timestamp)
        {
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = staffid + adminToken + timestamp;
            //字符串按照升序排列
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //将二进制转化为大写的十六进制
            StringBuilder resultStr = new StringBuilder();
            foreach (var c in md5Val)
            {
                resultStr.Append(c.ToString("X2"));
            }
            return resultStr.ToString().ToUpper();
        }
    }
}