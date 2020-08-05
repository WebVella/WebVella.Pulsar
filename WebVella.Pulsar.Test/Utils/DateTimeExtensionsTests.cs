using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebVella.Pulsar.Test
{
	[TestClass]
	public class DateTimeExtensionsTests
	{
		/// WvpConvertDateToBrowserLocal
		////////////////////////////////////////////////////////////////////////////////////
		[TestMethod]
		public void WvpConvertDateToBrowserLocal_NULL_ReturnsNull()
		{
			//Arrange
			var timeZone = "";
			int offsetMinutes = 0;
			DateTime? dateTime = null;


			//Act
			var result = dateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert

			Assert.IsTrue(result == null);

		}

		[TestMethod]
		public void WvpConvertDateToBrowserLocal_UtcNoTZ_ReturnsUTCWithOffset()
		{
			//Arrange
			var timeZone = "";
			int offsetMinutes = -180;
			DateTime? utcDateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"),DateTimeKind.Utc);


			//Act
			var result = utcDateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Unspecified ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 18:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpConvertDateToBrowserLocal_UtcTZ_ReturnsTZ()
		{
			//Arrange
			var timeZone = "FLE Standard Time";
			int offsetMinutes = -60; //should be ignored in this case
			DateTime? utcDateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"),DateTimeKind.Utc);

			//Act
			var result = utcDateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Unspecified ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 18:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpConvertDateToBrowserLocal_UtcUtcTZ_ReturnsTZ()
		{
			//Arrange
			var timeZone = "Utc";
			int offsetMinutes = -60; //should be ignored in this case
			DateTime? utcDateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"),DateTimeKind.Utc);

			//Act
			var result = utcDateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Unspecified ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 15:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpConvertDateToBrowserLocal_LocalNoTZ_ReturnsUTCWithOffset()
		{
			//Arrange
			var timeZone = "";
			var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
			int offsetMinutes = -60;
			DateTime? utcDateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"),DateTimeKind.Utc);
			DateTime? localDateTime = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.Value,localTimeZone),DateTimeKind.Local);

			//Act
			var result = localDateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Unspecified ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 16:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpConvertDateToBrowserLocal_LocalTZ_ReturnsTZ()
		{
			//Arrange
			var timeZone = "Central European Standard Time";
			var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
			int offsetMinutes = -60; //should be ignored in this case
			DateTime? utcDateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"), DateTimeKind.Utc);
			DateTime? localDateTime = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.Value,localTimeZone),DateTimeKind.Local);

			//Act
			var result = localDateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Unspecified ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 17:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpConvertDateToBrowserLocal_Unspecified_ReturnsItself()
		{
			//Arrange
			var timeZone = "Central European Standard Time";
			int offsetMinutes = -60; //should be ignored in this case
			DateTime? unspDateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"), DateTimeKind.Unspecified);

			//Act
			var result = unspDateTime.WvpConvertDateToBrowserLocal(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Unspecified ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 15:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}



		/// WvpBrowserLocalToDate
		////////////////////////////////////////////////////////////////////////////////////
		[TestMethod]
		public void WvpBrowserLocalToDate_NULL_ReturnsNull()
		{
			//Arrange
			var timeZone = "";
			int offsetMinutes = 0;
			DateTime? dateTime = null;


			//Act
			var result = dateTime.WvpBrowserLocalToDate(offsetMinutes, timeZone);

			//Assert

			Assert.IsTrue(result == null);

		}

		[TestMethod]
		public void WvpBrowserLocalToDate_TZ_ReturnsTZ()
		{
			//Arrange
			var timeZone = "FLE Standard Time";
			int offsetMinutes = -60;
			DateTime? dateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"), DateTimeKind.Unspecified);


			//Act
			var result = dateTime.WvpBrowserLocalToDate(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Local ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 17:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpBrowserLocalToDate_UTCTZ_ReturnsTZ()
		{
			//Arrange
			var timeZone = "UTC";
			int offsetMinutes = -60;
			DateTime? dateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"), DateTimeKind.Unspecified);


			//Act
			var result = dateTime.WvpBrowserLocalToDate(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Utc ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 15:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

		[TestMethod]
		public void WvpBrowserLocalToDate_NOTZ_ReturnsUTC()
		{
			//Arrange
			var timeZone = "";
			int offsetMinutes = -60;
			DateTime? dateTime = DateTime.SpecifyKind(DateTime.Parse("2020-08-01 15:00"), DateTimeKind.Unspecified);


			//Act
			var result = dateTime.WvpBrowserLocalToDate(offsetMinutes, timeZone);

			//Assert
			bool isTrue = true;

			if (result == null ||
				result.Value.Kind != DateTimeKind.Utc ||
				result.Value.ToString("yyyy-MM-dd HH:mm") != "2020-08-01 14:00")
			{
				isTrue = false;
			}

			Assert.IsTrue(isTrue);

		}

	}
}
