using CodeninModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class RegistrationVm
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public State NyscState { get; set; }
        public Batch NyscBatch { get; set; }
        public string CallUpNumber { get; set; }
        public string Institution { get; set; }
        public string Discpline { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}