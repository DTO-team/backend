﻿using System;
using System.Collections.Generic;
using Models.Dtos;

namespace Models.Response
{
	public class GetEvaluationSessionResponse
	{
		public Guid Id { get; set; }
		public GetSemesterDTO Semester { get; set; }
		public int Round { get; set; }
		public bool IsFinal { get; set; }
		public int Status { get; set; }
		public long DeadLine { get; set; }
		public List<GetSemesterCriteriaResponse> SemesterCriterias { get; set; }
		public List<GetLecturerResponse> LecturerInCouncils { get; set; }
		public List<GetProjectDetailDTO> Projects { get; set; }
	}
}