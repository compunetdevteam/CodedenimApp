using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeninModel;

namespace CodedenimWebApp.ViewModels
{
    public class StudentTypeVm
    {
        public List<Student> Corpers { get; set; }
        public List<Student> Undergraduate { get; set; }
        public List<Student> OtherStudent { get; set; }
    }
}