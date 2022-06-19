using System;
using CapstoneOnGoing.Filter;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace CapstoneOnGoing.Services.Implements
{
	public class UriService : IUriService
	{
		private readonly string _baseUri;

		public UriService(string baseUri)
		{
			_baseUri = baseUri;
		}
		public Uri GetPageUri(PaginationFilter filter, string route)
		{
			Uri endpointUri = new Uri(string.Concat(_baseUri,route));
			string modifiedUri;
			if (filter.SearchString != string.Empty)
			{
				modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "searchString", filter.SearchString.ToString());
				modifiedUri =  QueryHelpers.AddQueryString(modifiedUri, "pageNumber", filter.PageNumber.ToString());
				modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
			}
			else
			{
				modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
				modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
			}
			return new Uri(modifiedUri);
		}
	}
}
