using System;
using System.Collections.Generic;
using System.Linq;
using Cmn.Enums;

namespace Clt.Logic.Helpers
{
    public static class UserHelper
    {
        public static List<UserRight> GetAllRights()
        {
            var list = Enum.GetValues(typeof(UserRight)).Cast<UserRight>().ToList();

            return list;
        }
    }
}