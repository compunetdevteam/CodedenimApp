﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodedenimWebApp.Controllers.Api.ApiViewModel
{
    public class CourseCategoryVm
    {
        public int CourseCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public string StudentType { get; set; }
        public string CategoryDescription { get; set; }
        public string ImageLocation { get; set; }
    }
}