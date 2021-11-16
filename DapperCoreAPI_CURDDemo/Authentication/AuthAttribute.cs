using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperCoreAPI_CURDDemo.Authentication
{
    public class AuthAttribute : AuthorizeAttribute
    {
        private bool localAllowed;
        public AuthAttribute(bool allowedParam)
        {
            localAllowed = allowedParam;
        }
        /*protected new bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.IsLocal)
            {
                return localAllowed;
            }
            else
            {
                return true;
            }
        }*/
    }
}
