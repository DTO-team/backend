using System.Collections.Generic;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/semesters")]
	[ApiController]
	public class SemesterController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ILoggerManager _logger;
		private readonly ISemesterService _semesterService;

		public SemesterController(IMapper mapper, ILoggerManager logger, ISemesterService semesterService)
		{
			_mapper = mapper;
			_logger = logger;
			_semesterService = semesterService;
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		public IActionResult GetAllSemester([FromQuery] int page, [FromQuery] int limit)
		{
			IEnumerable<Semester> semesters = _semesterService.GetAllSemesters(page, limit);
			IEnumerable<GetSemesterDTO> semestersDTO = _mapper.Map<IEnumerable<GetSemesterDTO>>(semesters);
			return Ok(semestersDTO);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		public IActionResult CreateNewSemester([FromBody] CreateNewSemesterDTO newSemesterDTO)
		{
			Semester newSemester = _mapper.Map<Semester>(newSemesterDTO);
			newSemester = _semesterService.CreateNewSemester(newSemester);
			if (newSemester != null)
			{
				GetSemesterDTO result = _mapper.Map<GetSemesterDTO>(newSemester);
				return CreatedAtAction(nameof(CreateNewSemester), result);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(SemesterController)},Method: {nameof(CreateNewSemester)}, The {newSemesterDTO.Year} - {newSemesterDTO.Season} is already existed");
				return BadRequest($"Semester {newSemesterDTO.Year} - {newSemesterDTO.Season} is already existed");
			}
		}


		[Authorize(Roles = "ADMIN")]
		[HttpPut("{id}")] 
		public IActionResult UpdateSemester([FromBody] UpdateSemesterDTO updateSemesterDTO)
		{
			Semester updatedSemester = _semesterService.GetSemesterById(updateSemesterDTO.Id);
			if (updatedSemester != null)
			{
				bool isSuccessful = _semesterService.UpdateSemester(updatedSemester, updateSemesterDTO);
				if (isSuccessful)
				{
					return Ok(updateSemesterDTO);
				}
				_logger.LogWarn($"Controller: {nameof(SemesterController)},Method: {nameof(UpdateSemester)}, The {updatedSemester.Year} - {updatedSemester.Season} update failed");
				return BadRequest($"The semester {updatedSemester.Year} - {updatedSemester.Season} updated failed");
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(SemesterController)},Method: {nameof(UpdateSemester)}, The {updateSemesterDTO.Id} is not existed");
				return BadRequest($"Semester does not exist");
			}
		}
	}
}
