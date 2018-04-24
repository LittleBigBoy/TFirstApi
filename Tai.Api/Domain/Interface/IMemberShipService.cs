using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tai.Api.Domain.Interface
{
    public interface IMemberShipService
    {
        /// <summary>
        /// Validates the user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool ValidateLogin(string userName, string passWord);

        /// <summary>
        /// Delete cookies
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool LoginOut(string userName);

        /// <summary>
        /// Insert cookies
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="remeberMe"></param>
        /// <returns>adminToken</returns>
        string GetToken(string userName, string timestamp, bool remeberMe);

        /// <summary>
        /// Get user information by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string GetUserInfo(string userName);

    }
}