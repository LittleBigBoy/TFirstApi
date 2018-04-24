using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationInsights.Web;
using Newtonsoft.Json;
using Tai.Api.Common;
using Tai.Api.Domain.Interface;
using Tai.Api.Enum;
using Tai.Api.Models;
using Tai.Api.Models.Account;

namespace Tai.Api.Domain.Service
{
    internal class MemberShipService : IMemberShipService
    {
        private static readonly MemberShipService Instance = new MemberShipService();
        protected MemberShipService()
        {

        }

        public static MemberShipService GetInstance()
        {
            return Instance;
        }

        public bool ValidateLogin(string userName, string passWord)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(passWord))
            {
                return false;
            }
            return true;
        }

        public bool LoginOut(string userName)
        {
            HttpRuntime.Cache.Remove(userName);
            return true;
        }

        public string GetToken(string userName, string timestamp, bool remeberMe)
        {
            Token getToken = (Token)HttpRuntime.Cache.Get(userName);
            var signToken = Guid.NewGuid().ToString();
            if (getToken != null)
            {
                getToken.SignToken = SignExtension.GetSignToken(userName, signToken, timestamp);
                getToken.ExpireTime = remeberMe ? DateTime.Now.AddYears(1) : DateTime.Now.AddDays(1);
                HttpRuntime.Cache.Remove(userName);
                HttpRuntime.Cache.Insert(userName, getToken);
                return signToken;
            }
            Token token = new Token();
            token.SignToken = SignExtension.GetSignToken(userName, signToken, timestamp);
            token.StaffId = userName;
            token.ExpireTime = remeberMe ? DateTime.Now.AddYears(1) : DateTime.Now.AddDays(1);
            HttpRuntime.Cache.Insert(userName, token);
            return signToken;
        }

        public string GetUserInfo(string userName)
        {
            User user = new User();
            if (userName.Contains("admin"))
            {
                user.Id = 1;
                user.UserName = userName;
                user.NickName = "administator";
                user.CelPhone = "13735824764";
                var roles=new List<string>();
                roles.Add("admin");
                roles.Add("edit");
                user.Roles = roles;
                return JsonConvert.SerializeObject(user);
            }
            else
            {
                user.Id = 2;
                user.UserName = userName;
                user.NickName = "editor";
                user.CelPhone = "1681886869";
                var roles = new List<string>();
                roles.Add("edit");
                user.Roles = roles;
                return JsonConvert.SerializeObject(user);
            }
        }
    }
}