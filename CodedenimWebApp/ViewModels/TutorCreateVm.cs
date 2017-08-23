using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class TutorCreateVm
    {
        public string Designation { get; set; }
        public string MaritalStatus { get; set; }
        public string IsActiveTutor { get; set; }
        public string ActiveStatus { get; set; }
        public string StaffRole { get; set; }
        public FileType FileType { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string TownOfBirth { get; set; }
    public string StateOfOrigin { get; set; }
    public string Nationality { get; set; }
    public string CountryOfBirth { get; set; }
    public bool IsAcctive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] Passport { get; set; }
}
}