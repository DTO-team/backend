using System;

namespace Models.Dtos
{
    public class GetFeedbackDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public long CreatedDateTime { get; set; }
        public Guid ReportId { get; set; }
    }
}
