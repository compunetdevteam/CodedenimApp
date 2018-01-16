using CodedenimWebApp.Abstractions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Infrastructure
{
    public class CodedenimIdentityUserMgr : ICodedenimUserMgr
    {
        /// <summary>
        /// Gets and return current Identity Principal logged in 
        /// using Microsoft.AspNet.Identity
        /// </summary>
        /// <returns>string - username or email address</returns>
        public string GetUser()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}