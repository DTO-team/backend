using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Models;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IFeedbackService
    {
        IEnumerable<GetFeedbackDTO> GetFeedbacksByReportId(Guid reportId);
        IEnumerable<GetFeedbackDTO> GetAllFeedback(GetFeedbackRequest getFeedbackRequest);
        IEnumerable<GetFeedbackDTO> GetAllStudentFeedback(GetFeedbackRequest studentsOfATeamFeedbackRequest);
        GetFeedbackDTO GetFeedbackOfStudentReport(GetFeedbackOfStudentReport feedbackOfStudentOfStudentReport);
        GetFeedbackDTO GetFeedbackOfTeamReport(GetFeedbackRequest feedbackRequest);
    }
}
