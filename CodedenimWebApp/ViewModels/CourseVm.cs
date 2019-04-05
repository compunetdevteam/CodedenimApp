using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.ViewModels
{
    public class CourseVm
    {


        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CourseNumber { get; set; }
        public string CourseDescription { get; set; }
        public int ExpectedTime { get; set; }
        [DisplayName("Day Interval")]
        public int NoOfDays { get; set; }
        public DateTime? DateAdded { get; set; }
        [Range(0, 5)]
        public int Points { get; set; }
        public string ImageName { get; set; }
        public string VideoName { get; set; }
    }
}