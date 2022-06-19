using System;
using CapstoneOnGoing.Filter;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IUriService
	{
		Uri GetPageUri(PaginationFilter filter, string route);
	}
}
