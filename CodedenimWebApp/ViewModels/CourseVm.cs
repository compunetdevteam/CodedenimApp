using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class CourseVm
    {
        public int CourseId { get; set; }
        public int CourseCategoryId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int ExpectedTime { get; set; }
        public DateTime? DateAdded { get; set; }
        [Range(0, 5)]
        public int Points { get; set; }
        public byte[] CourseImage { get; set; }
    }
}