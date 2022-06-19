using System;

namespace CapstoneOnGoing.Filter
{
	public class PaginationFilter
	{
		public string SearchString { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

		public PaginationFilter()
		{
			this.SearchString = string.Empty;
			this.PageNumber = 1;
			this.PageSize = 10;
		}

		public PaginationFilter(string searchString,int pageNumber, int pageSize)
		{
			this.SearchString = searchString == String.Empty ? String.Empty : searchString;
			this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
			this.PageSize = pageSize > 10 ? 10 : pageSize;
		}
	}
}
