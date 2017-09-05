using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class TutorRegistrationVm
    {
        [Display(Name = "Tutor ID")]
        [Required(ErrorMessage = "Tutor ID Number is required")]
        [StringLength(25, ErrorMessage = "Tutor ID is too long")]
        public string TutorId { get; set; }

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


        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
    }
}