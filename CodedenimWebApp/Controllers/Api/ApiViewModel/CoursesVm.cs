using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class CoursesVm
    {
        public int CourseId { get; set; }
        public int CourseCategoryId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
        public string CourseDescription { get; set; }
        public int ExpectedTime { get; set; }
        public string FileLocation { get; set; }
        public int ProgressCount { get; set; }
        public IEnumerable<ModuleVm> Modules { get; set; }
    }
}