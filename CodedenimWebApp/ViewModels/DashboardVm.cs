﻿using CodeninModel;
using CodeninModel.Forums;
using CodeninModel.Quiz;
using System.Collections.Generic;

namespace CodedenimWebApp.ViewModels
{
    public class DashboardVm
    {
        public List<AssignCourseCategory> AssignCourseCategories { get; set; }
        public List<StudentPayment> Student { get; set; }
        public List<CourseCategory> CourseCategories { get; set; }
        public List<Student> StudentInfo { get; set; }
        public List<ForumQuestion> ForumQuestion { get; set; }
        public List<QuizLog> StudentQuiz { get; set; }

    }
}