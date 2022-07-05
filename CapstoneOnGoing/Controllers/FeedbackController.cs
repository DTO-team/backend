using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/feedbacks")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILecturerService _lecturerService;
        private readonly IMapper _mapper;

        public FeedbackController(IFeedbackService feedbackService, ILecturerService lecturerService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _lecturerService = lecturerService;
            _mapper = mapper;
        }

        private GetFeedbackResponse MappingToGetFeedbackResponse(GetFeedbackDTO feedbackDto)
        {
            GetFeedbackResponse feedbackResponse = new GetFeedbackResponse();
            feedbackResponse.Id = feedbackDto.Id;
            feedbackResponse.CreatedDateTime = feedbackDto.CreatedDateTime;
            feedbackResponse.Content = feedbackDto.Content;

            User lecturer = _lecturerService.GetLecturerById(feedbackDto.AuthorId);
            feedbackResponse.Author = _mapper.Map<GetLecturerResponse>(lecturer);

            return feedbackResponse;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetAllTeamFeedBack([FromQuery] Guid weekId, [FromQuery] Guid projectId)
        {
            GetFeedbackRequest feedbackRequest = new GetFeedbackRequest();
            feedbackRequest.WeekId = weekId;
            feedbackRequest.ProjectId = projectId;
            IEnumerable<GetFeedbackDTO> getFeedbackDtos =
                _feedbackService.GetAllFeedback(feedbackRequest);
            List<GetFeedbackResponse> feedbackResponses = new List<GetFeedbackResponse>();
            if (getFeedbackDtos != null)
            {
                GetFeedbackResponse feedbackResponse;
                Array.ForEach(getFeedbackDtos.ToArray(), feedbackDto =>
                {
                    feedbackResponse = MappingToGetFeedbackResponse(feedbackDto);
                    feedbackResponses.Add(feedbackResponse);
                });
            }

            return Ok(feedbackResponses);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("students")]
        public IActionResult GetAllStudentOfTeamFeedBack([FromQuery]Guid weekId, [FromQuery]Guid projectId)
        {
            GetFeedbackRequest studentsOfATeamFeedbackRequest = new GetFeedbackRequest();
            studentsOfATeamFeedbackRequest.WeekId = weekId;
            studentsOfATeamFeedbackRequest.ProjectId = projectId;
            IEnumerable<GetFeedbackDTO> getFeedbackDtos =
                _feedbackService.GetAllStudentFeedback(studentsOfATeamFeedbackRequest);

            List<GetFeedbackResponse> feedbackResponses = new List<GetFeedbackResponse>();
            if (getFeedbackDtos != null)
            {
                GetFeedbackResponse feedbackResponse;
                Array.ForEach(getFeedbackDtos.ToArray(), feedbackDto =>
                {
                    feedbackResponse = MappingToGetFeedbackResponse(feedbackDto);
                    feedbackResponses.Add(feedbackResponse);
                });
            }

            return Ok(feedbackResponses);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("student")]
        public IActionResult GetStudentFeedback([FromQuery] Guid studentId, [FromQuery] Guid weekId,
            [FromQuery] Guid projectId)
        {
            GetFeedbackOfStudentReport feedbackOfStudentReport = new GetFeedbackOfStudentReport();
            feedbackOfStudentReport.StudentId = studentId;
            feedbackOfStudentReport.ProjectId = projectId;
            feedbackOfStudentReport.WeekId = weekId;

            GetFeedbackDTO feedbackDto = _feedbackService.GetFeedbackOfStudentReport(feedbackOfStudentReport);
            GetFeedbackResponse feedbackResponse = null;
            if (feedbackDto is not null)
            {
                feedbackResponse = MappingToGetFeedbackResponse(feedbackDto);
                return Ok(feedbackResponse);
            }    
            return Ok(feedbackResponse);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("team")]
        public IActionResult GetTeamFeedback([FromQuery] Guid weekId,
            [FromQuery] Guid projectId)
        {
            GetFeedbackRequest feedbackOfTeamReport = new GetFeedbackOfStudentReport();
            feedbackOfTeamReport.ProjectId = projectId;
            feedbackOfTeamReport.WeekId = weekId;

            GetFeedbackDTO feedbackDto = _feedbackService.GetFeedbackOfTeamReport(feedbackOfTeamReport);
            GetFeedbackResponse feedbackResponse = null;
            if (feedbackDto is not null)
            {
                feedbackResponse = MappingToGetFeedbackResponse(feedbackDto);
                return Ok(feedbackResponse);
            }
            return Ok(feedbackResponse);
        }
    }
}
