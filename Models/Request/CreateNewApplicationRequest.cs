using System;

namespace Models.Request
{
    public class CreateNewApplicationRequest
    {
        public Guid TeamId { get; set; }
        public Guid TopicId { get; set; }
    }
}
