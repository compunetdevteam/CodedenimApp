using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodedenimWebApp.Abstractions
{
    public interface ICodedenimUserMgr
    {
        /// <summary>
        /// Get and return the currently logged in User
        /// </summary>
        /// <returns></returns>
        string GetUser();
    }
}
