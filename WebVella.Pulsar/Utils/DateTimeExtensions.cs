namespace System
{
	public static class DateTimeExtensions
	{

		public static DateTime WvpClearKind(this DateTime datetime)
		{
			return ((DateTime?)datetime).WvpClearKind().Value;
		}

		public static DateTime? WvpClearKind(this DateTime? datetime)
		{
			if (datetime == null)
				return null;

			return new DateTime(datetime.Value.Ticks, DateTimeKind.Unspecified);
		}


		public static DateTime WvpConvertToTZDate(this DateTime datetime, string timeZoneName)
		{
			return ((DateTime?)datetime).WvpConvertToTZDate(timeZoneName).Value;
		}

		public static DateTime? WvpConvertToTZDate(this DateTime? datetime, string timeZoneName)
		{
			if (datetime == null)
				return null;

			TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
			return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(datetime.Value, appTimeZone.Id);
		}

		public static DateTime WvpConvertTZDateToUtc(this DateTime datetime, string timeZoneName)
		{
			return ((DateTime?)datetime).WvpConvertTZDateToUtc(timeZoneName).Value;
		}

		public static DateTime? WvpConvertTZDateToUtc(this DateTime? inputDate, string timeZoneName)
		{
			if (inputDate == null)
				return null;
			TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);

			DateTime tmpDT = inputDate.Value;
			if (tmpDT.Kind == DateTimeKind.Utc)
				return tmpDT;
			else if (tmpDT.Kind == DateTimeKind.Local && appTimeZone != TimeZoneInfo.Local)
			{
				var convertedToAppZoneDate = TimeZoneInfo.ConvertTime(tmpDT, appTimeZone);
				return TimeZoneInfo.ConvertTimeToUtc(convertedToAppZoneDate, appTimeZone);
			}

			return TimeZoneInfo.ConvertTimeToUtc(inputDate.Value, appTimeZone);
		}

		public static DateTime? WvpConvertDateToBrowserLocal(this DateTime? inputDate, int browserUtcOffsetInMinutes = 0, string timeZoneName = "")
		{
			//NOTE: have in mind that browser offset needs to be reversed as it shows minus in it means ahead of UTC (which in our case should add minutes not substract them)
			browserUtcOffsetInMinutes = browserUtcOffsetInMinutes * -1;

			//If the requested zone is UTC then the browser offset should be ignored
			//var isUtcTargetTimeZone = false;
			//if(timeZoneName == "UTC")
			//	isUtcTargetTimeZone = true;

			if (inputDate == null)
				return null;

			TimeZoneInfo timezone = null;
			if (!String.IsNullOrWhiteSpace(timeZoneName))
			{
				try
				{
					timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
				}
				catch
				{
					throw new Exception("Timezone name not found");
				}
			}
			//Case UTC 
			if (inputDate.Value.Kind == DateTimeKind.Utc)
			{
				//If timezone is not null 
				// 1. convert utc to timezone 
				// 2. convert to unspecified
				if (timezone != null)
				{
					inputDate = TimeZoneInfo.ConvertTimeFromUtc(inputDate.Value, timezone);

					return DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Unspecified);
				}
				//If timezone is null 
				// 1. convert utc to unspecified kind 
				// 2. add the offset minutes
				else
				{
					inputDate = DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Unspecified);
					return inputDate.Value.AddMinutes(browserUtcOffsetInMinutes);
				}
			}
			//Case Local
			else if (inputDate.Value.Kind == DateTimeKind.Local)
			{
				//If timezone is not null 
				// 1. convert local to timezone 
				// 2. convert to unspecified
				if (timezone != null)
				{
					inputDate = TimeZoneInfo.ConvertTime(inputDate.Value, timezone);

					return DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Unspecified);
				}
				//If timezone is null 
				// 1. convert local to utc
				// 2. convert to unspecified
				// 3. add the offset minutes
				else
				{
					inputDate = inputDate.Value.ToUniversalTime();
					inputDate = DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Unspecified);
					return inputDate.Value.AddMinutes(browserUtcOffsetInMinutes);
				}
			}
			//Unspecified
			else
			{
				//1. Just return the value
				return inputDate.Value;
			}
		}

		public static DateTime? WvpBrowserLocalToDate(this DateTime? inputDate, int browserUtcOffsetInMinutes = 0, string timeZoneName = "")
		{
			//NOTE: have in mind that browser offset needs to be reversed as it shows minus in it means ahead of UTC (which in our case should add minutes not substract them)
			browserUtcOffsetInMinutes = browserUtcOffsetInMinutes * -1;

			var isUtcTargetTimeZone = false;
			if(timeZoneName == "UTC")
				isUtcTargetTimeZone = true;


			if (inputDate == null)
				return null;

			TimeZoneInfo timezone = null;
			if (!String.IsNullOrWhiteSpace(timeZoneName))
			{
				try
				{
					timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
				}
				catch
				{
					throw new Exception("Timezone name not found");
				}
			}

			if (inputDate.Value.Kind == DateTimeKind.Utc || inputDate.Value.Kind == DateTimeKind.Local)
				throw new Exception("Browser Local date should be with unspecified kind");

			//Timezone is not null
			//1. Remove offset
			//2. Convert to utc kind
			//3. Convert to timezone date
			if (timezone != null)
			{
				//Input in UTC
				if(isUtcTargetTimeZone){
					return DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Utc);

				}
				else{
					inputDate = inputDate.Value.AddMinutes(-1 * browserUtcOffsetInMinutes);
					inputDate = DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Utc);

					return DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(inputDate.Value, timezone), DateTimeKind.Local);				
				}

			}
			//Timezone is null
			//1. Remove offset
			//2. Convert to utc kind
			else
			{
				inputDate = inputDate.Value.AddMinutes(-1 * browserUtcOffsetInMinutes);
				return DateTime.SpecifyKind(DateTime.SpecifyKind(inputDate.Value, DateTimeKind.Utc), DateTimeKind.Utc);
			}
		}
	}
}
