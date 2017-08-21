using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using CodeninModel.Quiz;

namespace CodeninModel
{
    public class TopicAssignment
    {
        public int TopicAssignmentId { get; set; }
        public int TopicId { get; set; }
        public string AssignmentTitle { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime? AssignmentDueDate { get; set; }
        public virtual Topic Topic { get; set; }
    }

    public class SubmitAssignment
    {
        public int SubmitAssignmentId { get; set; }
        public int TopicId { get; set; }
        public string StudentId { get; set; }
        public string AssignmentBody { get; set; }
        public string AttachmentLocation { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }

        public virtual Student Student { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<AssignmentReview> AssignmentReviews { get; set; }
    }

    public class AssignmentReview
    {
        public int AssignmentReviewId { get; set; }
        public int SubmitAssignmentId { get; set; }
        public string ReviewComment { get; set; }
        public string Rating { get; set; }
        public virtual SubmitAssignment SubmitAssignment { get; set; }
    }
}