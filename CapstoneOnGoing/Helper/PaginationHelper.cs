using System;
using System.Collections.Generic;
using CapstoneOnGoing.Filter;
using CapstoneOnGoing.Services.Interfaces;
using Models.Response;

namespace CapstoneOnGoing.Helper
{
	public class PaginationHelper<T>
	{
		public static PagedResponse<IEnumerable<T>> CreatePagedResponse(IEnumerable<T> pagedData, PaginationFilter validFilter,
			int totalRecords, IUriService uriService, string route)
		{
			PagedResponse<IEnumerable<T>> response =
				new PagedResponse<IEnumerable<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
			var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
			int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
			response.NextPage = validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
				? uriService.GetPageUri(new PaginationFilter(validFilter.SearchString,validFilter.PageNumber + 1, validFilter.PageSize), route)
				: null;
			response.PreviousPage = validFilter.PageNumber - 1 >= 1 && validFilter.PageSize <= roundedTotalPages
				? uriService.GetPageUri(new PaginationFilter(validFilter.SearchString,validFilter.PageNumber - 1, validFilter.PageSize), route)
				: null;
			response.FirstPage = uriService.GetPageUri(new PaginationFilter(validFilter.SearchString,1, validFilter.PageSize), route);
			response.LastPage =
				uriService.GetPageUri(new PaginationFilter(validFilter.SearchString,roundedTotalPages, validFilter.PageSize), route);
			response.TotalPages = roundedTotalPages;
			response.TotalRecords = totalRecords;
			return response;
		}
	}
}
