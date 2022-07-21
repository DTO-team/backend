using System;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetCouncilReviewOnProjectResponse
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CouncilId { get; set; }
        public bool IsFinal { get; set; }
        public long Date { get; set; }
        public IEnumerable<GetGradeCopyResponse> GradeCopyResponses { get; set; }
        public IEnumerable<GetQuestionCopyResponse> QuestionCopyResponses { get; set; }
    }
}
