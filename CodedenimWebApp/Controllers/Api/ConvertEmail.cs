using System.Data;
using System.Data.Entity;
using System.Linq;
using CodedenimWebApp.Models;

namespace CodedenimWebApp.Controllers.Api
{
    public  class ConvertEmail
    {
        private  readonly  ApplicationDbContext  _db ;

        public ConvertEmail()
        {
            _db = new ApplicationDbContext();
        }
        public string ConvertEmailToId(string email)
        {
            var studentEmail = _db.Students.AsNoTracking().Where(x => x.Email.Equals(email))
                                            .Select(x => x.Id).FirstOrDefault();
            return studentEmail;
        }
    }

    public class ConvertEmail1
    {
        private readonly ApplicationDbContext _db;

        public ConvertEmail1()
        {
            _db = new ApplicationDbContext();
        }
        public  string ConvertEmailToId(string email)
        {
            var studentEmail = _db.Students.AsNoTracking().Where(x => x.Email.Equals(email))
                .Select(x => x.Id).FirstOrDefault();
            return studentEmail;
        }
    }
}