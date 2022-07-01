﻿using System;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetTopicAllProjectResponse
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
        public GetCompanyResponse CompanyDetail { get; set; }
        public IEnumerable<GetLecturerResponse> LecturersDetails { get; set; }
    }
}
