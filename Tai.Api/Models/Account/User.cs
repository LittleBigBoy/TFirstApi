using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tai.Api.Models.Account
{
    public class User
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// loginname
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// nickname
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// celphone
        /// </summary>
        public string CelPhone { get; set; }

        /// <summary>
        /// roles
        /// </summary>
        public IList<string> Roles { get; set; }

    }
}