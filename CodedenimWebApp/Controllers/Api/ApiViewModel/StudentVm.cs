using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class StudentVm
    {
        public string MatricNo { get; set; }
        public string CallUpNo { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public bool Active { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Picture { get; set; }
        public string AccountType { get; set; }
        public string Institution { get; set; }
        public string StateOfService { get; set; }
        public string Batch { get; set; }
        public string Discpline { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}