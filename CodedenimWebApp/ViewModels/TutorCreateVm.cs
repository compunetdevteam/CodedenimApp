using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class TutorCreateVm
    {
        public string TutorId { get; set; }
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
    public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] TutorPassport { get; set; }

        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    TutorPassport = target.ToArray();
                }
                catch (Exception)
                {
                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }
    }
}