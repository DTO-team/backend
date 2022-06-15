using System;

namespace Models.Dtos
{
    public class ApplicationFields
    {
        public string TeamName { get; set; }
        public Guid TeamLeaderId { get; set; }
        public Guid TeamSemesterId { get; set; }
    }

    public class TopicFields
    {
        public Guid TopicId { get; set; }
        public string Description { get; set; }
        // public string? CompanyName { get; set; }
    }

    public class GetApplicationDTO
    {
        public ApplicationFields TeamInformation { get; set; }

        public TopicFields Topic { get; set; }

        public int Status { get; set; }
    }
}
