using System;
namespace Models.Response
{
    public class GetFeedbackResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public long CreatedDateTime { get; set; }
        public bool IsTeamReport { get; set; }
        public GetLecturerResponse Author { get; set; }
    }
}
