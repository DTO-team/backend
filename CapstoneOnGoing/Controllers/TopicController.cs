using System;
using System.Collections.Generic;
 using System.Linq;
 using System.Net;
 using System.Security.Claims;
 using System.Text;
 using System.Threading.Tasks;
 using AutoMapper;
 using CapstoneOnGoing.Filter;
 using CapstoneOnGoing.Helper;
 using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Models.Dtos;
 using Models.Request;
using Models.Response;
 using Newtonsoft.Json;

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
		private const string PAGEREPONSEREDIS = "PageReponseRedis";

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
		public async Task<IActionResult> GetAllTopics([FromQuery] PaginationFilter paginationFilter)
		{
			PagedResponse<IEnumerable<GetTopicsResponse>> pagedResponse;
			string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			string route = Request.Path.Value;
			var headers = Request.Headers;
            StringValues currentsemester;
            if (!string.Equals(headers.Keys.ToString(), "currentsemester", StringComparison.OrdinalIgnoreCase) || !headers.TryGetValue("currentsemester", out currentsemester))
            {
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllTopics)}: Semester {currentsemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}
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
			//get list from Redis
			//var serializedResponseRedis = await _redisService.GetAsync(PAGEREPONSEREDIS);
			//if (serializedResponseRedis != null)
			//{
			//	var topicCachedRedis = Encoding.UTF8.GetString(serializedResponseRedis);
			//	IEnumerable<GetTopicsResponse> topics = JsonConvert.DeserializeObject<IEnumerable<GetTopicsResponse>>(topicCachedRedis);
			//	var topicsResponse = validFilter.SearchString.Equals(String.Empty)
			//		? topics
			//		: topics.Where(x => x.TopicName == validFilter.SearchString);
			//	var pageResponse =  PaginationHelper<GetTopicsResponse>.CreatePagedResponse(topicsResponse,validFilter,)
			//	return Ok();
			//}
			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(currentsemester.ToString());
			IEnumerable<GetTopicsDTO> topicsDtos =  _topicService.GetAllTopics(validFilter, email, semesterDto, out int totalRecords);
			if (topicsDtos != null)
			{
				IEnumerable<GetTopicsResponse> getTopicsResponses = _mapper.Map<IEnumerable<GetTopicsResponse>>(topicsDtos);
				pagedResponse =
					PaginationHelper<GetTopicsResponse>.CreatePagedResponse(getTopicsResponses, validFilter,
						totalRecords, _uriService, route);
				string serializedPageResponse = JsonConvert.SerializeObject(getTopicsResponses);
				byte[] redisPageResponse = Encoding.UTF8.GetBytes(serializedPageResponse);
				var options = new DistributedCacheEntryOptions();
				await _redisService.SetAsync(PAGEREPONSEREDIS, redisPageResponse, options);
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

		// [Authorize(Roles = "ADMIN,STUDENT,LECTURER,COMPANY")]
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
