using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebVella.Pulsar.Services
{
	public class FieldValueService
	{
		public static bool InitAsBool(object input)
		{
			if (input == null)
				return false;
			if (input is Boolean)
				return (bool)input;

			if (input is String)
			{
				if (Boolean.TryParse((string)input, out bool outBool))
				{
					return outBool;
				}
			}

			return false;
		}

		public static bool? InitAsNullBool(object input)
		{
			if (input == null)
				return null;
			if (input is Boolean)
				return (bool?)input;

			if (input is String)
			{
				if (Boolean.TryParse((string)input, out bool outBool))
				{
					return outBool;
				}
			}

			return false;
		}

		public static decimal? InitAsDecimal(object input, CultureInfo culture = null)
		{
			if (culture == null)
				culture = CultureInfo.InvariantCulture;

			if (input == null)
				return null;

			if (input is Decimal)
				return (decimal)input;

			if (Decimal.TryParse(input.ToString(), NumberStyles.Float, culture, out decimal outDec))
			{
				return outDec;
			}
			return null;
		}

		public static int? InitAsInt(object input)
		{
			if (input == null)
				return null;

			if (input is int)
				return (int)input;

			if (int.TryParse(input.ToString(), out int outInt))
			{
				return outInt;
			}
			return null;
		}

		public static string InitAsHtml(object input)
		{
			if (input == null)
				return "";
			if (input is String)
				return (string)input;

			return input.ToString();
		}

		public static List<string> InitAsListString(object input)
		{
			if (input == null)
				return new List<string>();

			if (input is List<string>)
				return ((List<string>)input).ToList();

			if (input is List<Guid>)
			{
				var result = new List<string>();
				((List<Guid>)input).ForEach(x => result.Add(x.ToString()));
				return result;
			}
			if (input is List<decimal>)
			{
				var result = new List<string>();
				((List<decimal>)input).ForEach(x => result.Add(x.ToString()));
				return result;
			}

			if (input is List<int>)
			{
				var result = new List<string>();
				((List<int>)input).ForEach(x => result.Add(x.ToString()));
				return result;
			}


			return new List<string>();
		}

		public static List<T> InitAsGenericList<T>(object input)
		{
			if (input != null && input is List<T>)
				return input as List<T>;

			return new List<T>();
		}

		public static T InitAsGeneric<T>(object input)
		{
			if (input != null && input is T)
				return (T)input;

			return default;
		}

		public static string InitAsString(object input)
		{
			if (input == null)
				return "";
			if (input is String)
				return (string)input;

			return input.ToString();
		}

		public static string ValidateAsBool(object input)
		{
			if (input is Boolean)
				return "";
			else
				return "invalid type";
		}

		public static string ValidateAsString(object input)
		{
			if (input is String)
				return "";
			else
				return "invalid type";
		}

		public static DateTime? InitAsDateTime(object input)
		{
			if (input == null)
				return null;

			if(input is DateTime)
				return (DateTime)input;

			if (DateTime.TryParse(input.ToString(), out DateTime outDt))
			{
				return outDt;
			}

			return null;
		}

		public static void SafeSetPropertyValue(object target, string propName, object value)
		{
			Type type = target.GetType();
			PropertyInfo prop = type.GetProperty(propName);
			if (prop != null)
			{
				//Check if try to assing null to a non nullable value
				if ((value == null && Nullable.GetUnderlyingType(prop.PropertyType) == null))
					throw new Exception("Target value is not nullable type");
				else if ((value == null && Nullable.GetUnderlyingType(prop.PropertyType) != null))
					prop.SetValue(target, value, null);
				else
				{
					if (prop.PropertyType == value.GetType())
						prop.SetValue(target, value, null);
					else
						throw new Exception("Target value does not match the provided type");
				}
			}
			else
			{
				throw new Exception("Property not found in target");
			}
		}

		public static object SafeGetropertyValue(object target, string propName)
		{
			Type type = target.GetType();
			PropertyInfo prop = type.GetProperty(propName);
			if (prop != null)
			{
				return prop.GetValue(target);
			}
			else
			{
				throw new Exception("Property not found in target");
			}
		}
	}
}
