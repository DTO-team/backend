using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class ReportEvidence
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public Guid ReportId { get; set; }

        public virtual Report Report { get; set; }
    }
}
