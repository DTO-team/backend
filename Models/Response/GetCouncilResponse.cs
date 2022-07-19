using System;
using System.Collections.Generic;
using Models.Dtos;

namespace Models.Response
{
	public class GetCouncilResponse
	{
		public Guid Id { get; set; }
		public GetEvaluationSessionResponse EvaluationSession { get; set; }
		public List<GetLecturerResponse> Lecturers { get; set; }
		public List<GetProjectDetailDTO> Projects { get; set; }
	}
}