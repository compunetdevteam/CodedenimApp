using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Your First Name is required")]
        [StringLength(50, ErrorMessage = "Your First Name is too long")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Your Last Name is required")]
        [StringLength(50, ErrorMessage = "Your Last Name is too long")]
        public string LastName { get; set; }


    public string Gender { get; set; }
    [Required]
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