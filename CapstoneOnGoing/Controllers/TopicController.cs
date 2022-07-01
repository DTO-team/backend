
﻿using System;
using System.Collections.Generic;
using System.Net;
 using System.Security.Claims;
 using AutoMapper;
 using CapstoneOnGoing.Filter;
 using CapstoneOnGoing.Helper;
 using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 using Microsoft.Extensions.Caching.Distributed;
 using Models.Dtos;
 using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/topics")]
	[ApiController]
	public class TopicController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly ITopicService _topicService;
		private readonly IMapper _mapper;
		private readonly IUriService _uriService;
		private readonly IDistributedCache _redisService;

		public TopicController(ILoggerManager logger, ITopicService topicService, IMapper mapper, IUriService uriService, IDistributedCache redisService)
		{
			_logger = logger;
			_topicService = topicService;
			_mapper = mapper;
			_uriService = uriService;
			_redisService = redisService;
		}

		[Authorize(Roles = "ADMIN,STUDENT,LECTURER,COMPANY")]
		[HttpGet]
		[ProducesResponseType(typeof(PagedResponse<IEnumerable<GetTopicsResponse>>),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status404NotFound)]
		public IActionResult GetAllTopics([FromQuery] PaginationFilter paginationFilter)
		{
			string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			string route = Request.Path.Value;
			PaginationFilter validFilter;
			if (string.IsNullOrEmpty(paginationFilter.SearchString) ||
			    string.IsNullOrWhiteSpace(paginationFilter.SearchString))
			{
				validFilter = new PaginationFilter(String.Empty,paginationFilter.PageNumber,paginationFilter.PageSize);
			}
			else
			{
				validFilter = new PaginationFilter(paginationFilter.SearchString,paginationFilter.PageNumber,paginationFilter.PageSize);
			}
			IEnumerable<GetTopicsDTO> topicsDtos =  _topicService.GetAllTopics(validFilter, email, out int totalRecords);
			if (topicsDtos != null)
			{
				IEnumerable<GetTopicsResponse> getTopicsResponses = _mapper.Map<IEnumerable<GetTopicsResponse>>(topicsDtos);
				PagedResponse<IEnumerable<GetTopicsResponse>> pagedResponse =
					PaginationHelper<GetTopicsResponse>.CreatePagedResponse(getTopicsResponses, validFilter,
						totalRecords, _uriService, route);
				return Ok(pagedResponse);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TopicController)},Method: {nameof(GetAllTopics)}: Can not load topic for semester");
				return NotFound(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status404NotFound,
					Message = "No topics found. Cause no Semester is not in current in-progress or No topic in current semester",
					TimeStamp = DateTime.Now,
				});
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost("list")]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest)
		{
			bool isSuccessful =  _topicService.ImportTopics(importTopicsRequest);
			if (isSuccessful)
			{
				return Ok(new GenericResponse {HttpStatus = (int)HttpStatusCode.OK,Message = "Import topics successfully", TimeStamp = DateTime.Now});
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TopicController)}, Method: {nameof(ImportTopics)}: Import topics failed");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int) HttpStatusCode.BadRequest,
					Message = "Import topics failed",
					TimeStamp = DateTime.Now
				});
			}
		}

		[Authorize]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetTopicsResponse),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult GetTopicDetails(Guid id)
		{
			GetTopicsDTO topicDTO = _topicService.GetTopicDetails(id);
			GetTopicsResponse topicResponse = _mapper.Map<GetTopicsResponse>(topicDTO);
			return Ok(topicResponse);
		}
	}
}
