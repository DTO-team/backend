﻿using System;

namespace Models.Dtos
{
    public class GradeDTO
    {
        public Guid Id { get; set; }
        public Guid CriteriaId { get; set; }
        public string Level { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public string Description { get; set; }
    }
}
