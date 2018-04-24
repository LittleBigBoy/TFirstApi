using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tai.Api.Domain.Interface;
using Tai.Api.Domain.Service;

namespace Tai.Api.Domain.Base
{
    public static class MemberShipBase
    {
        public static IMemberShipService GetMemberShipServiceInstance()
        {
            return MemberShipService.GetInstance();
        }
    }
}