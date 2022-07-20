using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
    public class ReviewGradeRequest
    {
        [Required]
        public Guid GradeId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
    public class ReviewQuestionRequest
    {
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        public string Comment { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
    }
    public class CreateNewReviewRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid CouncilId { get; set; }
        [Required]
        public bool IsFinal { get; set; }
        public long Date { get; set; }
        public ICollection<ReviewGradeRequest> ReviewGrade { get; set; }
        public ICollection<ReviewQuestionRequest> ReviewQuestion { get; set; }
    }
}
