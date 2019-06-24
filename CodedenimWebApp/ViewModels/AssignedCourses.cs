using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class AssignedCourses
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public bool Assigned { get; set; }
    }

    public class AssigedCoursesToTutorVm
    {
        public  string TutorId { get; set; }
        public int[] CourseId { get; set; }
    }
    public class AssignCourseToCategory
    {
        public int CourseCategoryId { get; set; }
        public int[] CourseId { get; set; }
    }
    public class AssignCourseCategoryPaymentVm
    {
        public int CourseCategoryId { get; set; }
        public int[] CourseId { get; set; }
        public string PaymentTypeName { get; set; }
    }
        
 }