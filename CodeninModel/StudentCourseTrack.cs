using System;

namespace CodeninModel
{
    public class StudentCourseTrack
    {
        public int StudentCourseTrackId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate
        {
            get
            {
                if (Course != null)
                {
                    return StartDate.AddDays(Course.NoOfDays);
                }
                return null;
            }
        }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
