using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodedenimWebApp.Models;

namespace CodedenimWebApp.Controllers.Api
{
    public class Converter
    {
        private readonly ApplicationDbContext _db;
        public Converter()
        {
            _db = new ApplicationDbContext();
        }
        /// <summary>
        /// this converter method takes email and return the user type
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>Account Type</returns>
        public string UserType(string userEmail)
        {
            var studentType = _db.Students.Where(x => x.Email.Equals(userEmail)).Select(x => x.AccountType).ToString();
            return studentType;
        }
    }
}