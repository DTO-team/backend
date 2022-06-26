using System;

namespace CapstoneOnGoing.Helper
{
	public static class DateTimeHelper
	{
		public static long ConvertDateTimeToLong(DateTime date)
		{
			DateTime epoch = DateTimeOffset.UnixEpoch.DateTime.ToLocalTime();
			var convertResult = Convert.ToInt64((date - epoch).TotalSeconds);
			return convertResult;
		}

		public static DateTime ConvertLongToDateTime(long seconds)
		{
			DateTime convertResult = DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime.ToLocalTime();
			return convertResult;
		}
	}
}
