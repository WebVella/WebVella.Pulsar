﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WebVella.Pulsar.Models;
using CsvHelper;

namespace WebVella.Pulsar.Utils
{
	public static class WvpHelpers
	{
		#region << Embedded resources >>
		public static string GetEmbeddedTextResource(string name, string nameSpace, string assemblyName = null)
		{
			string resourceName = $"{nameSpace}.{name}";
			Assembly assembly = null;
			if (!String.IsNullOrWhiteSpace(assemblyName))
			{
				assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName);
			}
			else
			{
				assembly = Assembly.GetExecutingAssembly();
			}
			Stream resource = assembly.GetManifestResourceStream(resourceName);
			if (resource == null)
				throw new Exception($"file: {name} in resource: {resourceName} not found as embedded resource");

			StreamReader reader = new StreamReader(resource);
			return reader.ReadToEnd();
		}

		public static string GetEmbeddedTextResource(string name, string nameSpace, Assembly assembly)
		{
			string resourceName = $"{nameSpace}.{name}";
			Stream resource = assembly.GetManifestResourceStream(resourceName);
			StreamReader reader = new StreamReader(resource);
			return reader.ReadToEnd();
		}
		public static bool EmbeddedResourceExists(string name, string nameSpace, string assemblyName = null)
		{
			string resourceName = $"{nameSpace}.{name}";
			Assembly assembly = null;
			if (!String.IsNullOrWhiteSpace(assemblyName))
			{
				assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName);
			}
			else
			{
				assembly = Assembly.GetExecutingAssembly();
			}
			var resources = assembly.GetManifestResourceNames();
			return resources.Contains(resourceName);
		}
		public static bool EmbeddedResourceExists(string name, string nameSpace, Assembly assembly)
		{
			string resourceName = $"{nameSpace}.{name}";
			var resources = assembly.GetManifestResourceNames();
			return resources.Contains(resourceName);
		}
		public static Assembly GetTypeAssembly(string typeName)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
								 || a.FullName.ToLowerInvariant().StartsWith("system.")));
			foreach (var assembly in assemblies)
			{
				foreach (Type t in assembly.GetTypes())
				{
					string name = $"{t.Namespace}.{t.Name}";
					if (name == typeName)
						return assembly;
				}
			}
			return null;
		}
		public static Type GetType(string typeName)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
								 || a.FullName.ToLowerInvariant().StartsWith("system.")));
			foreach (var assembly in assemblies)
			{
				foreach (Type t in assembly.GetTypes())
				{
					string name = $"{t.Namespace}.{t.Name}";
					if (name == typeName)
						return t;
				}
			}
			return null;
		}

		#endregion

		#region << Fields >>
		public static void ValidateValueToFieldType(WvpFieldType? fieldType, dynamic InValue, out dynamic OutValue, out List<string> errorList)
		{
			OutValue = null;
			errorList = new List<string>();
			if (InValue != null && InValue is Enum)
			{
				InValue = ((int)InValue).ToString();
			}

			switch (fieldType)
			{
				case WvpFieldType.AutoNumber:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is decimal)
						{
							OutValue = (decimal)InValue;
						}
						else if (Decimal.TryParse(InValue.ToString(), out decimal result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a decimal");
						}
					}
					break;
				case WvpFieldType.Checkbox:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is bool)
						{
							OutValue = (bool)InValue;
						}
						else if (Boolean.TryParse(InValue.ToString(), out bool result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a boolean");
						}
					}
					break;
				case WvpFieldType.Currency:
				case WvpFieldType.Number:
				case WvpFieldType.Percent:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is decimal)
						{
							OutValue = (decimal)InValue;
						}
						else if (Decimal.TryParse(InValue.ToString(), out decimal result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a decimal");
						}
					}
					break;
				case WvpFieldType.Date:
				case WvpFieldType.DateTime:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is DateTime)
						{
							OutValue = (DateTime)InValue;
						}
						else if (DateTime.TryParse(InValue.ToString(), out DateTime result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a DateTime");
						}
					}
					break;
				case WvpFieldType.Email:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = "";
						}
						else
						{
							var emailRgx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
							if (!String.IsNullOrWhiteSpace(InValue) && !emailRgx.IsMatch(InValue.ToString()))
							{
								errorList.Add("Value is not a valid email!");
							}
							OutValue = InValue.ToString();
						}
					}
					break;
				case WvpFieldType.Guid:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is Guid)
						{
							OutValue = (Guid)InValue;
						}
						else if (Guid.TryParse(InValue.ToString(), out Guid result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a Guid");
						}
					}
					break;
				case WvpFieldType.Html:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = "";
						}
						else
						{
							InValue = InValue.ToString();
							InValue = InValue.Replace("<script>", "&lt;script&gt;").Replace("</script>", "&lt;/script&gt;");
							OutValue = InValue;
							//Check if Html value is valid
							//HtmlDocument doc = new HtmlDocument();
							//doc.LoadHtml(InValue);
							//doc.OptionFixNestedTags = true;
							//doc.OptionAutoCloseOnEnd = true;
							//if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
							//{
							//    foreach (var error in doc.ParseErrors)
							//    {
							//        errorList.Add($"Invalid html on line {error.Line}. {error.Reason}");
							//    }
							//}
							//else
							//{
							//    OutValue = doc.DocumentNode.OuterHtml;
							//}
						}
					}
					break;
				case WvpFieldType.MultiSelect:
					{
						var inValueListString = InValue as List<string>;
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = new List<string>();
						}
						else if (inValueListString != null)
						{
							OutValue = inValueListString;
						}
						else
						{
							var newList = new List<string>();
							newList.Add(InValue.ToString());
							OutValue = newList;
						}
					}
					break;
				case WvpFieldType.File:
				case WvpFieldType.Image:
				case WvpFieldType.Textarea:
				case WvpFieldType.Password:
				case WvpFieldType.Phone:
				case WvpFieldType.Select:
				case WvpFieldType.Text:
				case WvpFieldType.Url:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = "";
						}
						else
						{
							OutValue = InValue.ToString();
						}
					}
					break;
				default:
					{
						OutValue = InValue;
					}
					break;
			}
		}
		//public static List<WvFilterType> GetFilterTypesForFieldType(WvFieldType fieldType)
		//{
		//    var result = new List<WvFilterType>();

		//    switch (fieldType)
		//    {
		//        case WvFieldType.CheckboxField:
		//            {
		//                result.Add(WvFilterType.EQ);
		//            }
		//            break;
		//        case WvFieldType.AutoNumberField:
		//        case WvFieldType.CurrencyField:
		//        case WvFieldType.NumberField:
		//        case WvFieldType.PercentField:
		//            {
		//                result.Add(WvFilterType.EQ);
		//                result.Add(WvFilterType.NOT);
		//                result.Add(WvFilterType.LT);
		//                result.Add(WvFilterType.LTE);
		//                result.Add(WvFilterType.GT);
		//                result.Add(WvFilterType.GTE);
		//                result.Add(WvFilterType.BETWEEN);
		//                result.Add(WvFilterType.NOTBETWEEN);
		//            }
		//            break;
		//        case WvFieldType.DateField:
		//        case WvFieldType.DateTimeField:
		//            {
		//                result.Add(WvFilterType.EQ);
		//                result.Add(WvFilterType.NOT);
		//                result.Add(WvFilterType.LT);
		//                result.Add(WvFilterType.LTE);
		//                result.Add(WvFilterType.GT);
		//                result.Add(WvFilterType.GTE);
		//                result.Add(WvFilterType.BETWEEN);
		//                result.Add(WvFilterType.NOTBETWEEN);
		//            }
		//            break;
		//        case WvFieldType.GuidField:
		//            {
		//                result.Add(WvFilterType.EQ);
		//            }
		//            break;
		//        case WvFieldType.MultiSelectField:
		//            {
		//                result.Add(WvFilterType.CONTAINS);
		//            }
		//            break;
		//        default:
		//            {
		//                result.Add(WvFilterType.STARTSWITH);
		//                result.Add(WvFilterType.CONTAINS);
		//                result.Add(WvFilterType.EQ);
		//                result.Add(WvFilterType.NOT);
		//                result.Add(WvFilterType.REGEX);
		//                result.Add(WvFilterType.FTS);
		//            }
		//            break;
		//    }
		//    return result;
		//}

		public static List<WvpCurrency> GetAllCurrency()
		{
			var result = new List<WvpCurrency>();
			var currencyJson = "";
			#region << Currency List as String >>
			currencyJson = @"{
  ""aed"": {
  	""priority"": 100,
    ""iso_code"": ""AED"",
    ""name"": ""United Arab Emirates Dirham"",
    ""symbol"": ""د.إ"",
    ""alternate_symbols"": [""DH"", ""Dhs""],
    ""subunit"": ""Fils"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""784"",
    ""smallest_denomination"": 25
  },
  ""afn"": {
    ""priority"": 100,
    ""iso_code"": ""AFN"",
    ""name"": ""Afghan Afghani"",
    ""symbol"": ""؋"",
    ""alternate_symbols"": [""Af"", ""Afs""],
    ""subunit"": ""Pul"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""971"",
    ""smallest_denomination"": 100
  },
  ""all"": {
    ""priority"": 100,
    ""iso_code"": ""ALL"",
    ""name"": ""Albanian Lek"",
    ""symbol"": ""L"",
    ""disambiguate_symbol"": ""Lek"",
    ""alternate_symbols"": [""Lek""],
    ""subunit"": ""Qintar"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""008"",
    ""smallest_denomination"": 100
  },
  ""amd"": {
    ""priority"": 100,
    ""iso_code"": ""AMD"",
    ""name"": ""Armenian Dram"",
    ""symbol"": ""դր."",
    ""alternate_symbols"": [""dram""],
    ""subunit"": ""Luma"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""051"",
    ""smallest_denomination"": 10
  },
  ""ang"": {
    ""priority"": 100,
    ""iso_code"": ""ANG"",
    ""name"": ""Netherlands Antillean Gulden"",
    ""symbol"": ""ƒ"",
    ""alternate_symbols"": [""NAƒ"", ""NAf"", ""f""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x0192;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""532"",
    ""smallest_denomination"": 1
  },
  ""aoa"": {
    ""priority"": 100,
    ""iso_code"": ""AOA"",
    ""name"": ""Angolan Kwanza"",
    ""symbol"": ""Kz"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cêntimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""973"",
    ""smallest_denomination"": 10
  },
  ""ars"": {
    ""priority"": 100,
    ""iso_code"": ""ARS"",
    ""name"": ""Argentine Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""$m/n"",
    ""alternate_symbols"": [""$m/n"", ""m$n""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""032"",
    ""smallest_denomination"": 1
  },
  ""aud"": {
    ""priority"": 4,
    ""iso_code"": ""AUD"",
    ""name"": ""Australian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""A$"",
    ""alternate_symbols"": [""A$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""036"",
    ""smallest_denomination"": 5
  },
  ""awg"": {
    ""priority"": 100,
    ""iso_code"": ""AWG"",
    ""name"": ""Aruban Florin"",
    ""symbol"": ""ƒ"",
    ""alternate_symbols"": [""Afl""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x0192;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""533"",
    ""smallest_denomination"": 5
  },
  ""azn"": {
    ""priority"": 100,
    ""iso_code"": ""AZN"",
    ""name"": ""Azerbaijani Manat"",
    ""symbol"": ""₼"",
    ""alternate_symbols"": [""m"", ""man""],
    ""subunit"": ""Qəpik"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""944"",
    ""smallest_denomination"": 1
  },
  ""bam"": {
    ""priority"": 100,
    ""iso_code"": ""BAM"",
    ""name"": ""Bosnia and Herzegovina Convertible Mark"",
    ""symbol"": ""КМ"",
    ""alternate_symbols"": [""KM""],
    ""subunit"": ""Fening"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""977"",
    ""smallest_denomination"": 5
  },
  ""bbd"": {
    ""priority"": 100,
    ""iso_code"": ""BBD"",
    ""name"": ""Barbadian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""Bds$"",
    ""alternate_symbols"": [""Bds$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""052"",
    ""smallest_denomination"": 1
  },
  ""bdt"": {
    ""priority"": 100,
    ""iso_code"": ""BDT"",
    ""name"": ""Bangladeshi Taka"",
    ""symbol"": ""৳"",
    ""alternate_symbols"": [""Tk""],
    ""subunit"": ""Paisa"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""050"",
    ""smallest_denomination"": 1
  },
  ""bgn"": {
    ""priority"": 100,
    ""iso_code"": ""BGN"",
    ""name"": ""Bulgarian Lev"",
    ""symbol"": ""лв."",
    ""alternate_symbols"": [""lev"", ""leva"", ""лев"", ""лева""],
    ""subunit"": ""Stotinka"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""975"",
    ""smallest_denomination"": 1
  },
  ""bhd"": {
    ""priority"": 100,
    ""iso_code"": ""BHD"",
    ""name"": ""Bahraini Dinar"",
    ""symbol"": ""ب.د"",
    ""alternate_symbols"": [""BD""],
    ""subunit"": ""Fils"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""048"",
    ""smallest_denomination"": 5
  },
  ""bif"": {
    ""priority"": 100,
    ""iso_code"": ""BIF"",
    ""name"": ""Burundian Franc"",
    ""symbol"": ""Fr"",
    ""disambiguate_symbol"": ""FBu"",
    ""alternate_symbols"": [""FBu""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""108"",
    ""smallest_denomination"": 100
  },
  ""bmd"": {
    ""priority"": 100,
    ""iso_code"": ""BMD"",
    ""name"": ""Bermudian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""BD$"",
    ""alternate_symbols"": [""BD$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""060"",
    ""smallest_denomination"": 1
  },
  ""bnd"": {
    ""priority"": 100,
    ""iso_code"": ""BND"",
    ""name"": ""Brunei Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""BND"",
    ""alternate_symbols"": [""B$""],
    ""subunit"": ""Sen"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""096"",
    ""smallest_denomination"": 1
  },
  ""bob"": {
    ""priority"": 100,
    ""iso_code"": ""BOB"",
    ""name"": ""Bolivian Boliviano"",
    ""symbol"": ""Bs."",
    ""alternate_symbols"": [""Bs""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""068"",
    ""smallest_denomination"": 10
  },
  ""brl"": {
    ""priority"": 100,
    ""iso_code"": ""BRL"",
    ""name"": ""Brazilian Real"",
    ""symbol"": ""R$"",
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""R$"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""986"",
    ""smallest_denomination"": 5
  },
  ""bsd"": {
    ""priority"": 100,
    ""iso_code"": ""BSD"",
    ""name"": ""Bahamian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""BSD"",
    ""alternate_symbols"": [""B$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""044"",
    ""smallest_denomination"": 1
  },
  ""btn"": {
    ""priority"": 100,
    ""iso_code"": ""BTN"",
    ""name"": ""Bhutanese Ngultrum"",
    ""symbol"": ""Nu."",
    ""alternate_symbols"": [""Nu""],
    ""subunit"": ""Chertrum"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""064"",
    ""smallest_denomination"": 5
  },
  ""bwp"": {
    ""priority"": 100,
    ""iso_code"": ""BWP"",
    ""name"": ""Botswana Pula"",
    ""symbol"": ""P"",
    ""alternate_symbols"": [],
    ""subunit"": ""Thebe"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""072"",
    ""smallest_denomination"": 5
  },
  ""byn"": {
    ""priority"": 100,
    ""iso_code"": ""BYN"",
    ""name"": ""Belarusian Ruble"",
    ""symbol"": ""Br"",
    ""disambiguate_symbol"": ""BYN"",
    ""alternate_symbols"": [""бел. руб."", ""б.р."", ""руб."", ""р.""],
    ""subunit"": ""Kapeyka"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": "" "",
    ""iso_numeric"": ""933"",
    ""smallest_denomination"": 1
  },
  ""byr"": {
    ""priority"": 50,
    ""iso_code"": ""BYR"",
    ""name"": ""Belarusian Ruble"",
    ""symbol"": ""Br"",
    ""disambiguate_symbol"": ""BYR"",
    ""alternate_symbols"": [""бел. руб."", ""б.р."", ""руб."", ""р.""],
    ""subunit"": null,
    ""subunit_to_unit"": 1,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": "" "",
    ""iso_numeric"": ""974"",
    ""smallest_denomination"": 100
  },
  ""bzd"": {
    ""priority"": 100,
    ""iso_code"": ""BZD"",
    ""name"": ""Belize Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""BZ$"",
    ""alternate_symbols"": [""BZ$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""084"",
    ""smallest_denomination"": 1
  },
  ""cad"": {
    ""priority"": 5,
    ""iso_code"": ""CAD"",
    ""name"": ""Canadian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""C$"",
    ""alternate_symbols"": [""C$"", ""CAD$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""124"",
    ""smallest_denomination"": 5
  },
  ""cdf"": {
    ""priority"": 100,
    ""iso_code"": ""CDF"",
    ""name"": ""Congolese Franc"",
    ""symbol"": ""Fr"",
    ""disambiguate_symbol"": ""FC"",
    ""alternate_symbols"": [""FC""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""976"",
    ""smallest_denomination"": 1
  },
  ""chf"": {
    ""priority"": 100,
    ""iso_code"": ""CHF"",
    ""name"": ""Swiss Franc"",
    ""symbol"": ""CHF"",
    ""alternate_symbols"": [""SFr"", ""Fr""],
    ""subunit"": ""Rappen"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""756"",
    ""smallest_denomination"": 5
  },
  ""clf"": {
    ""priority"": 100,
    ""iso_code"": ""CLF"",
    ""name"": ""Unidad de Fomento"",
    ""symbol"": ""UF"",
    ""alternate_symbols"": [],
    ""subunit"": ""Peso"",
    ""subunit_to_unit"": 10000,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""990""
  },
  ""clp"": {
    ""priority"": 100,
    ""iso_code"": ""CLP"",
    ""name"": ""Chilean Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""CLP"",
    ""alternate_symbols"": [],
    ""subunit"": ""Peso"",
    ""subunit_to_unit"": 1,
    ""symbol_first"": true,
    ""html_entity"": ""&#36;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""152"",
    ""smallest_denomination"": 1
  },
  ""cny"": {
    ""priority"": 100,
    ""iso_code"": ""CNY"",
    ""name"": ""Chinese Renminbi Yuan"",
    ""symbol"": ""¥"",
    ""alternate_symbols"": [""CN¥"", ""元"", ""CN元""],
    ""subunit"": ""Fen"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""￥"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""156"",
    ""smallest_denomination"": 1
  },
  ""cop"": {
    ""priority"": 100,
    ""iso_code"": ""COP"",
    ""name"": ""Colombian Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""COL$"",
    ""alternate_symbols"": [""COL$""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""170"",
    ""smallest_denomination"": 20
  },
  ""crc"": {
    ""priority"": 100,
    ""iso_code"": ""CRC"",
    ""name"": ""Costa Rican Colón"",
    ""symbol"": ""₡"",
    ""alternate_symbols"": [""¢""],
    ""subunit"": ""Céntimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A1;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""188"",
    ""smallest_denomination"": 500
  },
  ""cuc"": {
    ""priority"": 100,
    ""iso_code"": ""CUC"",
    ""name"": ""Cuban Convertible Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""CUC$"",
    ""alternate_symbols"": [""CUC$""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""931"",
    ""smallest_denomination"": 1
  },
  ""cup"": {
    ""priority"": 100,
    ""iso_code"": ""CUP"",
    ""name"": ""Cuban Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""$MN"",
    ""alternate_symbols"": [""$MN""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""192"",
    ""smallest_denomination"": 1
  },
  ""cve"": {
    ""priority"": 100,
    ""iso_code"": ""CVE"",
    ""name"": ""Cape Verdean Escudo"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""Esc"",
    ""alternate_symbols"": [""Esc""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""132"",
    ""smallest_denomination"": 100
  },
  ""czk"": {
    ""priority"": 100,
    ""iso_code"": ""CZK"",
    ""name"": ""Czech Koruna"",
    ""symbol"": ""Kč"",
    ""alternate_symbols"": [],
    ""subunit"": ""Haléř"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""203"",
    ""smallest_denomination"": 100
  },
  ""djf"": {
    ""priority"": 100,
    ""iso_code"": ""DJF"",
    ""name"": ""Djiboutian Franc"",
    ""symbol"": ""Fdj"",
    ""alternate_symbols"": [],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""262"",
    ""smallest_denomination"": 100
  },
  ""dkk"": {
    ""priority"": 100,
    ""iso_code"": ""DKK"",
    ""name"": ""Danish Krone"",
    ""symbol"": ""kr."",
    ""disambiguate_symbol"": ""DKK"",
    ""alternate_symbols"": ["",-""],
    ""subunit"": ""Øre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""208"",
    ""smallest_denomination"": 50
  },
  ""dop"": {
    ""priority"": 100,
    ""iso_code"": ""DOP"",
    ""name"": ""Dominican Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""RD$"",
    ""alternate_symbols"": [""RD$""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""214"",
    ""smallest_denomination"": 100
  },
  ""dzd"": {
    ""priority"": 100,
    ""iso_code"": ""DZD"",
    ""name"": ""Algerian Dinar"",
    ""symbol"": ""د.ج"",
    ""alternate_symbols"": [""DA""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""012"",
    ""smallest_denomination"": 100
  },
  ""egp"": {
    ""priority"": 100,
    ""iso_code"": ""EGP"",
    ""name"": ""Egyptian Pound"",
    ""symbol"": ""ج.م"",
    ""alternate_symbols"": [""LE"", ""E£"", ""L.E.""],
    ""subunit"": ""Piastre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""818"",
    ""smallest_denomination"": 25
  },
  ""ern"": {
    ""priority"": 100,
    ""iso_code"": ""ERN"",
    ""name"": ""Eritrean Nakfa"",
    ""symbol"": ""Nfk"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""232"",
    ""smallest_denomination"": 1
  },
  ""etb"": {
    ""priority"": 100,
    ""iso_code"": ""ETB"",
    ""name"": ""Ethiopian Birr"",
    ""symbol"": ""Br"",
    ""disambiguate_symbol"": ""ETB"",
    ""alternate_symbols"": [],
    ""subunit"": ""Santim"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""230"",
    ""smallest_denomination"": 1
  },
  ""eur"": {
    ""priority"": 2,
    ""iso_code"": ""EUR"",
    ""name"": ""Euro"",
    ""symbol"": ""€"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20AC;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""978"",
    ""smallest_denomination"": 1
  },
  ""fjd"": {
    ""priority"": 100,
    ""iso_code"": ""FJD"",
    ""name"": ""Fijian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""FJ$"",
    ""alternate_symbols"": [""FJ$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""242"",
    ""smallest_denomination"": 5
  },
  ""fkp"": {
    ""priority"": 100,
    ""iso_code"": ""FKP"",
    ""name"": ""Falkland Pound"",
    ""symbol"": ""£"",
    ""disambiguate_symbol"": ""FK£"",
    ""alternate_symbols"": [""FK£""],
    ""subunit"": ""Penny"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""238"",
    ""smallest_denomination"": 1
  },
  ""gbp"": {
    ""priority"": 3,
    ""iso_code"": ""GBP"",
    ""name"": ""British Pound"",
    ""symbol"": ""£"",
    ""alternate_symbols"": [],
    ""subunit"": ""Penny"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""826"",
    ""smallest_denomination"": 1
  },
  ""gel"": {
    ""priority"": 100,
    ""iso_code"": ""GEL"",
    ""name"": ""Georgian Lari"",
    ""symbol"": ""ლ"",
    ""alternate_symbols"": [""lari""],
    ""subunit"": ""Tetri"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""981"",
    ""smallest_denomination"": 1
  },
  ""ghs"": {
    ""priority"": 100,
    ""iso_code"": ""GHS"",
    ""name"": ""Ghanaian Cedi"",
    ""symbol"": ""₵"",
    ""alternate_symbols"": [""GH¢"", ""GH₵""],
    ""subunit"": ""Pesewa"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B5;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""936"",
    ""smallest_denomination"": 1
  },
  ""gip"": {
    ""priority"": 100,
    ""iso_code"": ""GIP"",
    ""name"": ""Gibraltar Pound"",
    ""symbol"": ""£"",
    ""disambiguate_symbol"": ""GIP"",
    ""alternate_symbols"": [],
    ""subunit"": ""Penny"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""292"",
    ""smallest_denomination"": 1
  },
  ""gmd"": {
    ""priority"": 100,
    ""iso_code"": ""GMD"",
    ""name"": ""Gambian Dalasi"",
    ""symbol"": ""D"",
    ""alternate_symbols"": [],
    ""subunit"": ""Butut"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""270"",
    ""smallest_denomination"": 1
  },
  ""gnf"": {
    ""priority"": 100,
    ""iso_code"": ""GNF"",
    ""name"": ""Guinean Franc"",
    ""symbol"": ""Fr"",
    ""disambiguate_symbol"": ""FG"",
    ""alternate_symbols"": [""FG"", ""GFr""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""324"",
    ""smallest_denomination"": 100
  },
  ""gtq"": {
    ""priority"": 100,
    ""iso_code"": ""GTQ"",
    ""name"": ""Guatemalan Quetzal"",
    ""symbol"": ""Q"",
    ""alternate_symbols"": [],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""320"",
    ""smallest_denomination"": 1
  },
  ""gyd"": {
    ""priority"": 100,
    ""iso_code"": ""GYD"",
    ""name"": ""Guyanese Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""G$"",
    ""alternate_symbols"": [""G$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""328"",
    ""smallest_denomination"": 100
  },
  ""hkd"": {
    ""priority"": 100,
    ""iso_code"": ""HKD"",
    ""name"": ""Hong Kong Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""HK$"",
    ""alternate_symbols"": [""HK$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""344"",
    ""smallest_denomination"": 10
  },
  ""hnl"": {
    ""priority"": 100,
    ""iso_code"": ""HNL"",
    ""name"": ""Honduran Lempira"",
    ""symbol"": ""L"",
    ""disambiguate_symbol"": ""HNL"",
    ""alternate_symbols"": [],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""340"",
    ""smallest_denomination"": 5
  },
  ""hrk"": {
    ""priority"": 100,
    ""iso_code"": ""HRK"",
    ""name"": ""Croatian Kuna"",
    ""symbol"": ""kn"",
    ""alternate_symbols"": [],
    ""subunit"": ""Lipa"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""191"",
    ""smallest_denomination"": 1
  },
  ""htg"": {
    ""priority"": 100,
    ""iso_code"": ""HTG"",
    ""name"": ""Haitian Gourde"",
    ""symbol"": ""G"",
    ""alternate_symbols"": [],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""332"",
    ""smallest_denomination"": 5
  },
  ""huf"": {
    ""priority"": 100,
    ""iso_code"": ""HUF"",
    ""name"": ""Hungarian Forint"",
    ""symbol"": ""Ft"",
    ""alternate_symbols"": [],
    ""subunit"": ""Fillér"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""348"",
    ""smallest_denomination"": 500
  },
  ""idr"": {
    ""priority"": 100,
    ""iso_code"": ""IDR"",
    ""name"": ""Indonesian Rupiah"",
    ""symbol"": ""Rp"",
    ""alternate_symbols"": [],
    ""subunit"": ""Sen"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""360"",
    ""smallest_denomination"": 5000
  },
  ""ils"": {
    ""priority"": 100,
    ""iso_code"": ""ILS"",
    ""name"": ""Israeli New Sheqel"",
    ""symbol"": ""₪"",
    ""alternate_symbols"": [""ש״ח"", ""NIS""],
    ""subunit"": ""Agora"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20AA;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""376"",
    ""smallest_denomination"": 10
  },
  ""inr"": {
    ""priority"": 100,
    ""iso_code"": ""INR"",
    ""name"": ""Indian Rupee"",
    ""symbol"": ""₹"",
    ""alternate_symbols"": [""Rs"", ""৳"", ""૱"", ""௹"", ""रु"", ""₨""],
    ""subunit"": ""Paisa"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20b9;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""356"",
    ""smallest_denomination"": 50
  },
  ""iqd"": {
    ""priority"": 100,
    ""iso_code"": ""IQD"",
    ""name"": ""Iraqi Dinar"",
    ""symbol"": ""ع.د"",
    ""alternate_symbols"": [],
    ""subunit"": ""Fils"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""368"",
    ""smallest_denomination"": 50000
  },
  ""irr"": {
    ""priority"": 100,
    ""iso_code"": ""IRR"",
    ""name"": ""Iranian Rial"",
    ""symbol"": ""﷼"",
    ""alternate_symbols"": [],
    ""subunit"": null,
    ""subunit_to_unit"": 1,
    ""symbol_first"": true,
    ""html_entity"": ""&#xFDFC;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""364"",
    ""smallest_denomination"": 5000
  },
  ""isk"": {
    ""priority"": 100,
    ""iso_code"": ""ISK"",
    ""name"": ""Icelandic Króna"",
    ""symbol"": ""kr"",
    ""alternate_symbols"": [""Íkr""],
    ""subunit"": null,
    ""subunit_to_unit"": 1,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""352"",
    ""smallest_denomination"": 1
  },
  ""jmd"": {
    ""priority"": 100,
    ""iso_code"": ""JMD"",
    ""name"": ""Jamaican Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""J$"",
    ""alternate_symbols"": [""J$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""388"",
    ""smallest_denomination"": 1
  },
  ""jod"": {
    ""priority"": 100,
    ""iso_code"": ""JOD"",
    ""name"": ""Jordanian Dinar"",
    ""symbol"": ""د.ا"",
    ""alternate_symbols"": [""JD""],
    ""subunit"": ""Fils"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""400"",
    ""smallest_denomination"": 5
  },
  ""jpy"": {
    ""priority"": 6,
    ""iso_code"": ""JPY"",
    ""name"": ""Japanese Yen"",
    ""symbol"": ""¥"",
    ""alternate_symbols"": [""円"", ""圓""],
    ""subunit"": null,
    ""subunit_to_unit"": 1,
    ""symbol_first"": true,
    ""html_entity"": ""&#x00A5;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""392"",
    ""smallest_denomination"": 1
  },
  ""kes"": {
    ""priority"": 100,
    ""iso_code"": ""KES"",
    ""name"": ""Kenyan Shilling"",
    ""symbol"": ""KSh"",
    ""alternate_symbols"": [""Sh""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""404"",
    ""smallest_denomination"": 50
  },
  ""kgs"": {
    ""priority"": 100,
    ""iso_code"": ""KGS"",
    ""name"": ""Kyrgyzstani Som"",
    ""symbol"": ""som"",
    ""alternate_symbols"": [""сом""],
    ""subunit"": ""Tyiyn"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""417"",
    ""smallest_denomination"": 1
  },
  ""khr"": {
    ""priority"": 100,
    ""iso_code"": ""KHR"",
    ""name"": ""Cambodian Riel"",
    ""symbol"": ""៛"",
    ""alternate_symbols"": [],
    ""subunit"": ""Sen"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x17DB;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""116"",
    ""smallest_denomination"": 5000
  },
  ""kmf"": {
    ""priority"": 100,
    ""iso_code"": ""KMF"",
    ""name"": ""Comorian Franc"",
    ""symbol"": ""Fr"",
    ""disambiguate_symbol"": ""CF"",
    ""alternate_symbols"": [""CF""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""174"",
    ""smallest_denomination"": 100
  },
  ""kpw"": {
    ""priority"": 100,
    ""iso_code"": ""KPW"",
    ""name"": ""North Korean Won"",
    ""symbol"": ""₩"",
    ""alternate_symbols"": [],
    ""subunit"": ""Chŏn"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x20A9;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""408"",
    ""smallest_denomination"": 1
  },
  ""krw"": {
    ""priority"": 100,
    ""iso_code"": ""KRW"",
    ""name"": ""South Korean Won"",
    ""symbol"": ""₩"",
    ""subunit"": null,
    ""subunit_to_unit"": 1,
    ""alternate_symbols"": [],
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A9;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""410"",
    ""smallest_denomination"": 1
  },
  ""kwd"": {
    ""priority"": 100,
    ""iso_code"": ""KWD"",
    ""name"": ""Kuwaiti Dinar"",
    ""symbol"": ""د.ك"",
    ""alternate_symbols"": [""K.D.""],
    ""subunit"": ""Fils"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""414"",
    ""smallest_denomination"": 5
  },
  ""kyd"": {
    ""priority"": 100,
    ""iso_code"": ""KYD"",
    ""name"": ""Cayman Islands Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""CI$"",
    ""alternate_symbols"": [""CI$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""136"",
    ""smallest_denomination"": 1
  },
  ""kzt"": {
    ""priority"": 100,
    ""iso_code"": ""KZT"",
    ""name"": ""Kazakhstani Tenge"",
    ""symbol"": ""〒"",
    ""alternate_symbols"": [],
    ""subunit"": ""Tiyn"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""398"",
    ""smallest_denomination"": 100
  },
  ""lak"": {
    ""priority"": 100,
    ""iso_code"": ""LAK"",
    ""name"": ""Lao Kip"",
    ""symbol"": ""₭"",
    ""alternate_symbols"": [""₭N""],
    ""subunit"": ""Att"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x20AD;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""418"",
    ""smallest_denomination"": 10
  },
  ""lbp"": {
    ""priority"": 100,
    ""iso_code"": ""LBP"",
    ""name"": ""Lebanese Pound"",
    ""symbol"": ""ل.ل"",
    ""alternate_symbols"": [""£"", ""L£""],
    ""subunit"": ""Piastre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""422"",
    ""smallest_denomination"": 25000
  },
  ""lkr"": {
    ""priority"": 100,
    ""iso_code"": ""LKR"",
    ""name"": ""Sri Lankan Rupee"",
    ""symbol"": ""₨"",
    ""disambiguate_symbol"": ""SLRs"",
    ""alternate_symbols"": [""රු"", ""ரூ"", ""SLRs"", ""/-""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x0BF9;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""144"",
    ""smallest_denomination"": 100
  },
  ""lrd"": {
    ""priority"": 100,
    ""iso_code"": ""LRD"",
    ""name"": ""Liberian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""L$"",
    ""alternate_symbols"": [""L$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""430"",
    ""smallest_denomination"": 5
  },
  ""lsl"": {
    ""priority"": 100,
    ""iso_code"": ""LSL"",
    ""name"": ""Lesotho Loti"",
    ""symbol"": ""L"",
    ""disambiguate_symbol"": ""M"",
    ""alternate_symbols"": [""M""],
    ""subunit"": ""Sente"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""426"",
    ""smallest_denomination"": 1
  },
  ""ltl"": {
    ""priority"": 100,
    ""iso_code"": ""LTL"",
    ""name"": ""Lithuanian Litas"",
    ""symbol"": ""Lt"",
    ""alternate_symbols"": [],
    ""subunit"": ""Centas"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""440"",
    ""smallest_denomination"": 1
  },
  ""lvl"": {
    ""priority"": 100,
    ""iso_code"": ""LVL"",
    ""name"": ""Latvian Lats"",
    ""symbol"": ""Ls"",
    ""alternate_symbols"": [],
    ""subunit"": ""Santīms"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""428"",
    ""smallest_denomination"": 1
  },
  ""lyd"": {
    ""priority"": 100,
    ""iso_code"": ""LYD"",
    ""name"": ""Libyan Dinar"",
    ""symbol"": ""ل.د"",
    ""alternate_symbols"": [""LD""],
    ""subunit"": ""Dirham"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""434"",
    ""smallest_denomination"": 50
  },
  ""mad"": {
    ""priority"": 100,
    ""iso_code"": ""MAD"",
    ""name"": ""Moroccan Dirham"",
    ""symbol"": ""د.م."",
    ""alternate_symbols"": [],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""504"",
    ""smallest_denomination"": 1
  },
  ""mdl"": {
    ""priority"": 100,
    ""iso_code"": ""MDL"",
    ""name"": ""Moldovan Leu"",
    ""symbol"": ""L"",
    ""alternate_symbols"": [""lei""],
    ""subunit"": ""Ban"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""498"",
    ""smallest_denomination"": 1
  },
  ""mga"": {
    ""priority"": 100,
    ""iso_code"": ""MGA"",
    ""name"": ""Malagasy Ariary"",
    ""symbol"": ""Ar"",
    ""alternate_symbols"": [],
    ""subunit"": ""Iraimbilanja"",
    ""subunit_to_unit"": 5,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""969"",
    ""smallest_denomination"": 1
  },
  ""mkd"": {
    ""priority"": 100,
    ""iso_code"": ""MKD"",
    ""name"": ""Macedonian Denar"",
    ""symbol"": ""ден"",
    ""alternate_symbols"": [],
    ""subunit"": ""Deni"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""807"",
    ""smallest_denomination"": 100
  },
  ""mmk"": {
    ""priority"": 100,
    ""iso_code"": ""MMK"",
    ""name"": ""Myanmar Kyat"",
    ""symbol"": ""K"",
    ""disambiguate_symbol"": ""MMK"",
    ""alternate_symbols"": [],
    ""subunit"": ""Pya"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""104"",
    ""smallest_denomination"": 50
  },
  ""mnt"": {
    ""priority"": 100,
    ""iso_code"": ""MNT"",
    ""name"": ""Mongolian Tögrög"",
    ""symbol"": ""₮"",
    ""alternate_symbols"": [],
    ""subunit"": ""Möngö"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x20AE;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""496"",
    ""smallest_denomination"": 2000
  },
  ""mop"": {
    ""priority"": 100,
    ""iso_code"": ""MOP"",
    ""name"": ""Macanese Pataca"",
    ""symbol"": ""P"",
    ""alternate_symbols"": [""MOP$""],
    ""subunit"": ""Avo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""446"",
    ""smallest_denomination"": 10
  },
  ""mro"": {
    ""priority"": 100,
    ""iso_code"": ""MRO"",
    ""name"": ""Mauritanian Ouguiya"",
    ""symbol"": ""UM"",
    ""alternate_symbols"": [],
    ""subunit"": ""Khoums"",
    ""subunit_to_unit"": 5,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""478"",
    ""smallest_denomination"": 1
  },
  ""mur"": {
    ""priority"": 100,
    ""iso_code"": ""MUR"",
    ""name"": ""Mauritian Rupee"",
    ""symbol"": ""₨"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A8;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""480"",
    ""smallest_denomination"": 100
  },
  ""mvr"": {
    ""priority"": 100,
    ""iso_code"": ""MVR"",
    ""name"": ""Maldivian Rufiyaa"",
    ""symbol"": ""MVR"",
    ""alternate_symbols"": [""MRF"", ""Rf"", ""/-"", ""ރ""],
    ""subunit"": ""Laari"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""462"",
    ""smallest_denomination"": 1
  },
  ""mwk"": {
    ""priority"": 100,
    ""iso_code"": ""MWK"",
    ""name"": ""Malawian Kwacha"",
    ""symbol"": ""MK"",
    ""alternate_symbols"": [],
    ""subunit"": ""Tambala"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""454"",
    ""smallest_denomination"": 1
  },
  ""mxn"": {
    ""priority"": 100,
    ""iso_code"": ""MXN"",
    ""name"": ""Mexican Peso"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""MEX$"",
    ""alternate_symbols"": [""MEX$""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""484"",
    ""smallest_denomination"": 5
  },
  ""myr"": {
    ""priority"": 100,
    ""iso_code"": ""MYR"",
    ""name"": ""Malaysian Ringgit"",
    ""symbol"": ""RM"",
    ""alternate_symbols"": [],
    ""subunit"": ""Sen"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""458"",
    ""smallest_denomination"": 5
  },
  ""mzn"": {
    ""priority"": 100,
    ""iso_code"": ""MZN"",
    ""name"": ""Mozambican Metical"",
    ""symbol"": ""MTn"",
    ""alternate_symbols"": [""MZN""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""943"",
    ""smallest_denomination"": 1
  },
  ""nad"": {
    ""priority"": 100,
    ""iso_code"": ""NAD"",
    ""name"": ""Namibian Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""N$"",
    ""alternate_symbols"": [""N$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""516"",
    ""smallest_denomination"": 5
  },
  ""ngn"": {
    ""priority"": 100,
    ""iso_code"": ""NGN"",
    ""name"": ""Nigerian Naira"",
    ""symbol"": ""₦"",
    ""alternate_symbols"": [],
    ""subunit"": ""Kobo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A6;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""566"",
    ""smallest_denomination"": 50
  },
  ""nio"": {
    ""priority"": 100,
    ""iso_code"": ""NIO"",
    ""name"": ""Nicaraguan Córdoba"",
    ""symbol"": ""C$"",
    ""alternate_symbols"": [],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""558"",
    ""smallest_denomination"": 5
  },
  ""nok"": {
    ""priority"": 100,
    ""iso_code"": ""NOK"",
    ""name"": ""Norwegian Krone"",
    ""symbol"": ""kr"",
    ""disambiguate_symbol"": ""NOK"",
    ""alternate_symbols"": ["",-""],
    ""subunit"": ""Øre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""kr"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""578"",
    ""smallest_denomination"": 100
  },
  ""npr"": {
    ""priority"": 100,
    ""iso_code"": ""NPR"",
    ""name"": ""Nepalese Rupee"",
    ""symbol"": ""₨"",
    ""disambiguate_symbol"": ""NPR"",
    ""alternate_symbols"": [""Rs"", ""रू""],
    ""subunit"": ""Paisa"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A8;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""524"",
    ""smallest_denomination"": 1
  },
  ""nzd"": {
    ""priority"": 100,
    ""iso_code"": ""NZD"",
    ""name"": ""New Zealand Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""NZ$"",
    ""alternate_symbols"": [""NZ$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""554"",
    ""smallest_denomination"": 10
  },
  ""omr"": {
    ""priority"": 100,
    ""iso_code"": ""OMR"",
    ""name"": ""Omani Rial"",
    ""symbol"": ""ر.ع."",
    ""alternate_symbols"": [],
    ""subunit"": ""Baisa"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": true,
    ""html_entity"": ""&#xFDFC;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""512"",
    ""smallest_denomination"": 5
  },
  ""pab"": {
    ""priority"": 100,
    ""iso_code"": ""PAB"",
    ""name"": ""Panamanian Balboa"",
    ""symbol"": ""B/."",
    ""alternate_symbols"": [],
    ""subunit"": ""Centésimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""590"",
    ""smallest_denomination"": 1
  },
  ""pen"": {
    ""priority"": 100,
    ""iso_code"": ""PEN"",
    ""name"": ""Peruvian Nuevo Sol"",
    ""symbol"": ""S/."",
    ""alternate_symbols"": [],
    ""subunit"": ""Céntimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""S/."",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""604"",
    ""smallest_denomination"": 1
  },
  ""pgk"": {
    ""priority"": 100,
    ""iso_code"": ""PGK"",
    ""name"": ""Papua New Guinean Kina"",
    ""symbol"": ""K"",
    ""disambiguate_symbol"": ""PGK"",
    ""alternate_symbols"": [],
    ""subunit"": ""Toea"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""598"",
    ""smallest_denomination"": 5
  },
  ""php"": {
    ""priority"": 100,
    ""iso_code"": ""PHP"",
    ""name"": ""Philippine Peso"",
    ""symbol"": ""₱"",
    ""alternate_symbols"": [""PHP"", ""PhP"", ""P""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""608"",
    ""smallest_denomination"": 1
  },
  ""pkr"": {
    ""priority"": 100,
    ""iso_code"": ""PKR"",
    ""name"": ""Pakistani Rupee"",
    ""symbol"": ""₨"",
    ""disambiguate_symbol"": ""PKR"",
    ""alternate_symbols"": [""Rs""],
    ""subunit"": ""Paisa"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A8;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""586"",
    ""smallest_denomination"": 100
  },
  ""pln"": {
    ""priority"": 100,
    ""iso_code"": ""PLN"",
    ""name"": ""Polish Złoty"",
    ""symbol"": ""zł"",
    ""alternate_symbols"": [],
    ""subunit"": ""Grosz"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""z&#322;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": "" "",
    ""iso_numeric"": ""985"",
    ""smallest_denomination"": 1
  },
  ""pyg"": {
    ""priority"": 100,
    ""iso_code"": ""PYG"",
    ""name"": ""Paraguayan Guaraní"",
    ""symbol"": ""₲"",
    ""alternate_symbols"": [],
    ""subunit"": ""Céntimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B2;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""600"",
    ""smallest_denomination"": 5000
  },
  ""qar"": {
    ""priority"": 100,
    ""iso_code"": ""QAR"",
    ""name"": ""Qatari Riyal"",
    ""symbol"": ""ر.ق"",
    ""alternate_symbols"": [""QR""],
    ""subunit"": ""Dirham"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#xFDFC;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""634"",
    ""smallest_denomination"": 1
  },
  ""ron"": {
    ""priority"": 100,
    ""iso_code"": ""RON"",
    ""name"": ""Romanian Leu"",
    ""symbol"": ""Lei"",
    ""alternate_symbols"": [],
    ""subunit"": ""Bani"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""946"",
    ""smallest_denomination"": 1
  },
  ""rsd"": {
    ""priority"": 100,
    ""iso_code"": ""RSD"",
    ""name"": ""Serbian Dinar"",
    ""symbol"": ""РСД"",
    ""alternate_symbols"": [""RSD"", ""din"", ""дин""],
    ""subunit"": ""Para"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""941"",
    ""smallest_denomination"": 100
  },
  ""rub"": {
    ""priority"": 100,
    ""iso_code"": ""RUB"",
    ""name"": ""Russian Ruble"",
    ""symbol"": ""₽"",
    ""alternate_symbols"": [""руб."", ""р.""],
    ""subunit"": ""Kopeck"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x20BD;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""643"",
    ""smallest_denomination"": 1
  },
  ""rwf"": {
    ""priority"": 100,
    ""iso_code"": ""RWF"",
    ""name"": ""Rwandan Franc"",
    ""symbol"": ""FRw"",
    ""alternate_symbols"": [""RF"", ""R₣""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""646"",
    ""smallest_denomination"": 100
  },
  ""sar"": {
    ""priority"": 100,
    ""iso_code"": ""SAR"",
    ""name"": ""Saudi Riyal"",
    ""symbol"": ""ر.س"",
    ""alternate_symbols"": [""SR"", ""﷼""],
    ""subunit"": ""Hallallah"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#xFDFC;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""682"",
    ""smallest_denomination"": 5
  },
  ""sbd"": {
    ""priority"": 100,
    ""iso_code"": ""SBD"",
    ""name"": ""Solomon Islands Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""SI$"",
    ""alternate_symbols"": [""SI$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""090"",
    ""smallest_denomination"": 10
  },
  ""scr"": {
    ""priority"": 100,
    ""iso_code"": ""SCR"",
    ""name"": ""Seychellois Rupee"",
    ""symbol"": ""₨"",
    ""disambiguate_symbol"": ""SRe"",
    ""alternate_symbols"": [""SRe"", ""SR""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x20A8;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""690"",
    ""smallest_denomination"": 1
  },
  ""sdg"": {
    ""priority"": 100,
    ""iso_code"": ""SDG"",
    ""name"": ""Sudanese Pound"",
    ""symbol"": ""£"",
    ""disambiguate_symbol"": ""SDG"",
    ""alternate_symbols"": [],
    ""subunit"": ""Piastre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""938"",
    ""smallest_denomination"": 1
  },
  ""sek"": {
    ""priority"": 100,
    ""iso_code"": ""SEK"",
    ""name"": ""Swedish Krona"",
    ""symbol"": ""kr"",
    ""disambiguate_symbol"": ""SEK"",
    ""alternate_symbols"": ["":-""],
    ""subunit"": ""Öre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": "" "",
    ""iso_numeric"": ""752"",
    ""smallest_denomination"": 100
  },
  ""sgd"": {
    ""priority"": 100,
    ""iso_code"": ""SGD"",
    ""name"": ""Singapore Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""S$"",
    ""alternate_symbols"": [""S$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""702"",
    ""smallest_denomination"": 1
  },
  ""shp"": {
    ""priority"": 100,
    ""iso_code"": ""SHP"",
    ""name"": ""Saint Helenian Pound"",
    ""symbol"": ""£"",
    ""disambiguate_symbol"": ""SHP"",
    ""alternate_symbols"": [],
    ""subunit"": ""Penny"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""654"",
    ""smallest_denomination"": 1
  },
  ""skk"": {
    ""priority"": 100,
    ""iso_code"": ""SKK"",
    ""name"": ""Slovak Koruna"",
    ""symbol"": ""Sk"",
    ""alternate_symbols"": [],
    ""subunit"": ""Halier"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""703"",
    ""smallest_denomination"": 50
  },
  ""sll"": {
    ""priority"": 100,
    ""iso_code"": ""SLL"",
    ""name"": ""Sierra Leonean Leone"",
    ""symbol"": ""Le"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""694"",
    ""smallest_denomination"": 1000
  },
  ""sos"": {
    ""priority"": 100,
    ""iso_code"": ""SOS"",
    ""name"": ""Somali Shilling"",
    ""symbol"": ""Sh"",
    ""alternate_symbols"": [""Sh.So""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""706"",
    ""smallest_denomination"": 1
  },
  ""srd"": {
    ""priority"": 100,
    ""iso_code"": ""SRD"",
    ""name"": ""Surinamese Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""SRD"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""968"",
    ""smallest_denomination"": 1
  },
  ""ssp"": {
    ""priority"": 100,
    ""iso_code"": ""SSP"",
    ""name"": ""South Sudanese Pound"",
    ""symbol"": ""£"",
    ""disambiguate_symbol"": ""SSP"",
    ""alternate_symbols"": [],
    ""subunit"": ""piaster"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""728"",
    ""smallest_denomination"": 5
  },
  ""std"": {
    ""priority"": 100,
    ""iso_code"": ""STD"",
    ""name"": ""São Tomé and Príncipe Dobra"",
    ""symbol"": ""Db"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cêntimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""678"",
    ""smallest_denomination"": 10000
  },
  ""svc"": {
    ""priority"": 100,
    ""iso_code"": ""SVC"",
    ""name"": ""Salvadoran Colón"",
    ""symbol"": ""₡"",
    ""alternate_symbols"": [""¢""],
    ""subunit"": ""Centavo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20A1;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""222"",
    ""smallest_denomination"": 1
  },
  ""syp"": {
    ""priority"": 100,
    ""iso_code"": ""SYP"",
    ""name"": ""Syrian Pound"",
    ""symbol"": ""£S"",
    ""alternate_symbols"": [""£"", ""ل.س"", ""LS"", ""الليرة السورية""],
    ""subunit"": ""Piastre"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x00A3;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""760"",
    ""smallest_denomination"": 100
  },
  ""szl"": {
    ""priority"": 100,
    ""iso_code"": ""SZL"",
    ""name"": ""Swazi Lilangeni"",
    ""symbol"": ""E"",
    ""disambiguate_symbol"": ""SZL"",
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""748"",
    ""smallest_denomination"": 1
  },
  ""thb"": {
    ""priority"": 100,
    ""iso_code"": ""THB"",
    ""name"": ""Thai Baht"",
    ""symbol"": ""฿"",
    ""alternate_symbols"": [],
    ""subunit"": ""Satang"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x0E3F;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""764"",
    ""smallest_denomination"": 1
  },
  ""tjs"": {
    ""priority"": 100,
    ""iso_code"": ""TJS"",
    ""name"": ""Tajikistani Somoni"",
    ""symbol"": ""ЅМ"",
    ""alternate_symbols"": [],
    ""subunit"": ""Diram"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""972"",
    ""smallest_denomination"": 1
  },
  ""tmt"": {
    ""priority"": 100,
    ""iso_code"": ""TMT"",
    ""name"": ""Turkmenistani Manat"",
    ""symbol"": ""T"",
    ""alternate_symbols"": [],
    ""subunit"": ""Tenge"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""934"",
    ""smallest_denomination"": 1
  },
  ""tnd"": {
    ""priority"": 100,
    ""iso_code"": ""TND"",
    ""name"": ""Tunisian Dinar"",
    ""symbol"": ""د.ت"",
    ""alternate_symbols"": [""TD"", ""DT""],
    ""subunit"": ""Millime"",
    ""subunit_to_unit"": 1000,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""788"",
    ""smallest_denomination"": 10
  },
  ""top"": {
    ""priority"": 100,
    ""iso_code"": ""TOP"",
    ""name"": ""Tongan Paʻanga"",
    ""symbol"": ""T$"",
    ""alternate_symbols"": [""PT""],
    ""subunit"": ""Seniti"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""776"",
    ""smallest_denomination"": 1
  },
  ""try"": {
    ""priority"": 100,
    ""iso_code"": ""TRY"",
    ""name"": ""Turkish Lira"",
    ""symbol"": ""₺"",
    ""alternate_symbols"": [""TL""],
    ""subunit"": ""kuruş"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#8378;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""949"",
    ""smallest_denomination"": 1
  },
  ""ttd"": {
    ""priority"": 100,
    ""iso_code"": ""TTD"",
    ""name"": ""Trinidad and Tobago Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""TT$"",
    ""alternate_symbols"": [""TT$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""780"",
    ""smallest_denomination"": 1
  },
  ""twd"": {
    ""priority"": 100,
    ""iso_code"": ""TWD"",
    ""name"": ""New Taiwan Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""NT$"",
    ""alternate_symbols"": [""NT$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""901"",
    ""smallest_denomination"": 50
  },
  ""tzs"": {
    ""priority"": 100,
    ""iso_code"": ""TZS"",
    ""name"": ""Tanzanian Shilling"",
    ""symbol"": ""Sh"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""834"",
    ""smallest_denomination"": 5000
  },
  ""uah"": {
    ""priority"": 100,
    ""iso_code"": ""UAH"",
    ""name"": ""Ukrainian Hryvnia"",
    ""symbol"": ""₴"",
    ""alternate_symbols"": [],
    ""subunit"": ""Kopiyka"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#x20B4;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""980"",
    ""smallest_denomination"": 1
  },
  ""ugx"": {
    ""priority"": 100,
    ""iso_code"": ""UGX"",
    ""name"": ""Ugandan Shilling"",
    ""symbol"": ""USh"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""800"",
    ""smallest_denomination"": 1000
  },
  ""usd"": {
    ""priority"": 1,
    ""iso_code"": ""USD"",
    ""name"": ""United States Dollar"",
    ""symbol"": ""$"",
    ""alternate_symbols"": [""US$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""840"",
    ""smallest_denomination"": 1
  },
  ""uyu"": {
    ""priority"": 100,
    ""iso_code"": ""UYU"",
    ""name"": ""Uruguayan Peso"",
    ""symbol"": ""$"",
    ""alternate_symbols"": [""$U""],
    ""subunit"": ""Centésimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20B1;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""858"",
    ""smallest_denomination"": 100
  },
  ""uzs"": {
    ""priority"": 100,
    ""iso_code"": ""UZS"",
    ""name"": ""Uzbekistani Som"",
    ""symbol"": null,
    ""alternate_symbols"": [],
    ""subunit"": ""Tiyin"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""860"",
    ""smallest_denomination"": 100
  },
  ""vef"": {
    ""priority"": 100,
    ""iso_code"": ""VEF"",
    ""name"": ""Venezuelan Bolívar"",
    ""symbol"": ""Bs"",
    ""alternate_symbols"": [""Bs.F""],
    ""subunit"": ""Céntimo"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""937"",
    ""smallest_denomination"": 1
  },
  ""vnd"": {
    ""priority"": 100,
    ""iso_code"": ""VND"",
    ""name"": ""Vietnamese Đồng"",
    ""symbol"": ""₫"",
    ""alternate_symbols"": [],
    ""subunit"": ""Hào"",
    ""subunit_to_unit"": 1,
    ""symbol_first"": true,
    ""html_entity"": ""&#x20AB;"",
    ""decimal_mark"": "","",
    ""thousands_separator"": ""."",
    ""iso_numeric"": ""704"",
    ""smallest_denomination"": 100
  },
  ""vuv"": {
    ""priority"": 100,
    ""iso_code"": ""VUV"",
    ""name"": ""Vanuatu Vatu"",
    ""symbol"": ""Vt"",
    ""alternate_symbols"": [],
    ""subunit"": null,
    ""subunit_to_unit"": 1,
    ""symbol_first"": true,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""548"",
    ""smallest_denomination"": 1
  },
  ""wst"": {
    ""priority"": 100,
    ""iso_code"": ""WST"",
    ""name"": ""Samoan Tala"",
    ""symbol"": ""T"",
    ""disambiguate_symbol"": ""WS$"",
    ""alternate_symbols"": [""WS$"", ""SAT"", ""ST""],
    ""subunit"": ""Sene"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""882"",
    ""smallest_denomination"": 10
  },
  ""xaf"": {
    ""priority"": 100,
    ""iso_code"": ""XAF"",
    ""name"": ""Central African Cfa Franc"",
    ""symbol"": ""Fr"",
    ""disambiguate_symbol"": ""FCFA"",
    ""alternate_symbols"": [""FCFA""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""950"",
    ""smallest_denomination"": 100
  },
  ""xag"": {
    ""priority"": 100,
    ""iso_code"": ""XAG"",
    ""name"": ""Silver (Troy Ounce)"",
    ""symbol"": ""oz t"",
    ""disambiguate_symbol"": ""XAG"",
    ""alternate_symbols"": [],
    ""subunit"": ""oz"",
    ""subunit_to_unit"": 1,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""961""
  },
  ""xau"": {
    ""priority"": 100,
    ""iso_code"": ""XAU"",
    ""name"": ""Gold (Troy Ounce)"",
    ""symbol"": ""oz t"",
    ""disambiguate_symbol"": ""XAU"",
    ""alternate_symbols"": [],
    ""subunit"": ""oz"",
    ""subunit_to_unit"": 1,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""959""
  },
  ""xcd"": {
    ""priority"": 100,
    ""iso_code"": ""XCD"",
    ""name"": ""East Caribbean Dollar"",
    ""symbol"": ""$"",
    ""disambiguate_symbol"": ""EX$"",
    ""alternate_symbols"": [""EC$""],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""951"",
    ""smallest_denomination"": 1
  },
  ""xdr"": {
    ""priority"": 100,
    ""iso_code"": ""XDR"",
    ""name"": ""Special Drawing Rights"",
    ""symbol"": ""SDR"",
    ""alternate_symbols"": [""XDR""],
    ""subunit"": """",
    ""subunit_to_unit"": 1,
    ""symbol_first"": false,
    ""html_entity"": ""$"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""960""
  },
  ""xof"": {
    ""priority"": 100,
    ""iso_code"": ""XOF"",
    ""name"": ""West African Cfa Franc"",
    ""symbol"": ""Fr"",
    ""disambiguate_symbol"": ""CFA"",
    ""alternate_symbols"": [""CFA""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""952"",
    ""smallest_denomination"": 100
  },
  ""xpf"": {
    ""priority"": 100,
    ""iso_code"": ""XPF"",
    ""name"": ""Cfp Franc"",
    ""symbol"": ""Fr"",
    ""alternate_symbols"": [""F""],
    ""subunit"": ""Centime"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""953"",
    ""smallest_denomination"": 100
  },
  ""yer"": {
    ""priority"": 100,
    ""iso_code"": ""YER"",
    ""name"": ""Yemeni Rial"",
    ""symbol"": ""﷼"",
    ""alternate_symbols"": [],
    ""subunit"": ""Fils"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": ""&#xFDFC;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""886"",
    ""smallest_denomination"": 100
  },
  ""zar"": {
    ""priority"": 100,
    ""iso_code"": ""ZAR"",
    ""name"": ""South African Rand"",
    ""symbol"": ""R"",
    ""alternate_symbols"": [],
    ""subunit"": ""Cent"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": true,
    ""html_entity"": ""&#x0052;"",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""710"",
    ""smallest_denomination"": 10
  },
  ""zmk"": {
    ""priority"": 100,
    ""iso_code"": ""ZMK"",
    ""name"": ""Zambian Kwacha"",
    ""symbol"": ""ZK"",
    ""disambiguate_symbol"": ""ZMK"",
    ""alternate_symbols"": [],
    ""subunit"": ""Ngwee"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""894"",
    ""smallest_denomination"": 5
  },
  ""zmw"": {
    ""priority"": 100,
    ""iso_code"": ""ZMW"",
    ""name"": ""Zambian Kwacha"",
    ""symbol"": ""ZK"",
    ""disambiguate_symbol"": ""ZMW"",
    ""alternate_symbols"": [],
    ""subunit"": ""Ngwee"",
    ""subunit_to_unit"": 100,
    ""symbol_first"": false,
    ""html_entity"": """",
    ""decimal_mark"": ""."",
    ""thousands_separator"": "","",
    ""iso_numeric"": ""967"",
    ""smallest_denomination"": 5
  }
}";
			#endregion
			var currencyObj = (JObject)JsonConvert.DeserializeObject(currencyJson);

			foreach (var property in currencyObj)
			{
				var objectJson = JsonConvert.SerializeObject(property.Value);
				var currency = JsonConvert.DeserializeObject<WvpCurrency>(objectJson);
				result.Add(currency);
			}
			result = result.OrderBy(x => x.Priority).ToList();

			return result;
		}

		public static WvpCurrency GetCurrency(string currencyCode)
		{
			return GetAllCurrency().FirstOrDefault(x => x.IsoCode.ToLowerInvariant() == currencyCode.ToLowerInvariant());
		}

		public static List<WvpCurrencyType> GetAllCurrencyType()
		{
			var result = new List<WvpCurrencyType>();
			var getAllCurrency = GetAllCurrency();
			foreach (var currency in getAllCurrency)
			{
				var decimalDigits = 2;
				if (currency.SubUnitToUnit == 1000)
				{
					decimalDigits = 3;
				}

				var symbol = currency.IsoCode.ToUpperInvariant();
				if (currency.AlternateSymbols.Count > 0)
				{
					symbol = currency.AlternateSymbols[0];
				}

				var symPlacement = CurrencySymbolPlacement.After;
				if (currency.SymbolFirst)
				{
					symPlacement = CurrencySymbolPlacement.Before;
				}

				WvpCurrencyType model = new WvpCurrencyType()
				{
					Code = currency.IsoCode.ToUpperInvariant(),
					DecimalDigits = decimalDigits,
					Name = currency.Name,
					NamePlural = currency.Name,
					Rounding = 0,
					SymbolNative = currency.Symbol,
					Symbol = symbol,
					SymbolPlacement = symPlacement
				};
				result.Add(model);
			}

			return result;
		}

		public static WvpCurrencyType GetCurrencyType(string currencyCode)
		{
			WvpCurrencyType result = null;
			var currency = GetCurrency(currencyCode);

			if (currency != null)
			{
				var decimalDigits = 2;
				if (currency.SubUnitToUnit == 1000)
				{
					decimalDigits = 3;
				}

				var symbol = currency.IsoCode.ToUpperInvariant();
				if (currency.AlternateSymbols.Count > 0)
				{
					symbol = currency.AlternateSymbols[0];
				}

				var symPlacement = CurrencySymbolPlacement.After;
				if (currency.SymbolFirst)
				{
					symPlacement = CurrencySymbolPlacement.Before;
				}

				WvpCurrencyType model = new WvpCurrencyType()
				{
					Code = currency.IsoCode.ToUpperInvariant(),
					DecimalDigits = decimalDigits,
					Name = currency.Name,
					NamePlural = currency.Name,
					Rounding = 0,
					SymbolNative = currency.Symbol,
					Symbol = symbol,
					SymbolPlacement = symPlacement
				};

				return model;
			}
			return result;
		}


		public static string GetPathTypeIcon(string filePath)
		{
			var fontAwesomeIconName = "fa-file";
			if (filePath.EndsWith(".txt"))
			{
				fontAwesomeIconName = "fa-file-alt";
			}
			else if (filePath.EndsWith(".pdf"))
			{
				fontAwesomeIconName = "fa-file-pdf";
			}
			else if (filePath.EndsWith(".doc") || filePath.EndsWith(".docx"))
			{
				fontAwesomeIconName = "fa-file-word";
			}
			else if (filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx"))
			{
				fontAwesomeIconName = "fa-file-excel";
			}
			else if (filePath.EndsWith(".ppt") || filePath.EndsWith(".pptx"))
			{
				fontAwesomeIconName = "fa-file-powerpoint";
			}
			else if (filePath.EndsWith(".gif") || filePath.EndsWith(".jpg")
				 || filePath.EndsWith(".jpeg") || filePath.EndsWith(".png")
				 || filePath.EndsWith(".bmp") || filePath.EndsWith(".tif"))
			{
				fontAwesomeIconName = "fa-file-image";
			}
			else if (filePath.EndsWith(".zip") || filePath.EndsWith(".zipx")
				  || filePath.EndsWith(".rar") || filePath.EndsWith(".tar")
					|| filePath.EndsWith(".gz") || filePath.EndsWith(".dmg")
					  || filePath.EndsWith(".iso"))
			{
				fontAwesomeIconName = "fa-file-archive";
			}
			else if (filePath.EndsWith(".wav") || filePath.EndsWith(".mp3")
				  || filePath.EndsWith(".fla") || filePath.EndsWith(".flac")
					|| filePath.EndsWith(".ra") || filePath.EndsWith(".rma")
					  || filePath.EndsWith(".aif") || filePath.EndsWith(".aiff")
					  || filePath.EndsWith(".aa") || filePath.EndsWith(".aac")
						|| filePath.EndsWith(".aax") || filePath.EndsWith(".ac3")
						 || filePath.EndsWith(".au") || filePath.EndsWith(".ogg")
							|| filePath.EndsWith(".avr") || filePath.EndsWith(".3ga")
							|| filePath.EndsWith(".mid") || filePath.EndsWith(".midi")
							 || filePath.EndsWith(".m4a") || filePath.EndsWith(".mp4a")
							  || filePath.EndsWith(".amz") || filePath.EndsWith(".mka")
								 || filePath.EndsWith(".asx") || filePath.EndsWith(".pcm")
								 || filePath.EndsWith(".m3u") || filePath.EndsWith(".wma")
								  || filePath.EndsWith(".xwma"))
			{
				fontAwesomeIconName = "fa-file-audio";
			}
			else if (filePath.EndsWith(".avi") || filePath.EndsWith(".mpg")
				  || filePath.EndsWith(".mp4") || filePath.EndsWith(".mkv")
					|| filePath.EndsWith(".mov") || filePath.EndsWith(".wmv")
					  || filePath.EndsWith(".vp6") || filePath.EndsWith(".264")
					  || filePath.EndsWith(".vid") || filePath.EndsWith(".rv")
						|| filePath.EndsWith(".webm") || filePath.EndsWith(".swf")
						 || filePath.EndsWith(".h264") || filePath.EndsWith(".flv")
							|| filePath.EndsWith(".mk3d") || filePath.EndsWith(".gifv")
							|| filePath.EndsWith(".oggv") || filePath.EndsWith(".3gp")
							 || filePath.EndsWith(".m4v") || filePath.EndsWith(".movie")
							  || filePath.EndsWith(".divx"))
			{
				fontAwesomeIconName = "fa-file-video";
			}
			else if (filePath.EndsWith(".c") || filePath.EndsWith(".cpp")
				  || filePath.EndsWith(".css") || filePath.EndsWith(".js")
				  || filePath.EndsWith(".py") || filePath.EndsWith(".git")
					|| filePath.EndsWith(".cs") || filePath.EndsWith(".cshtml")
					|| filePath.EndsWith(".xml") || filePath.EndsWith(".html")
					  || filePath.EndsWith(".ini") || filePath.EndsWith(".config")
					  || filePath.EndsWith(".json") || filePath.EndsWith(".h"))
			{
				fontAwesomeIconName = "fa-file-code";
			}
			else if (filePath.EndsWith(".exe") || filePath.EndsWith(".jar")
				  || filePath.EndsWith(".dll") || filePath.EndsWith(".bat")
				  || filePath.EndsWith(".pl") || filePath.EndsWith(".scr")
					|| filePath.EndsWith(".msi") || filePath.EndsWith(".app")
					|| filePath.EndsWith(".deb") || filePath.EndsWith(".apk")
					  || filePath.EndsWith(".jar") || filePath.EndsWith(".vb")
					  || filePath.EndsWith(".prg") || filePath.EndsWith(".sh"))
			{
				fontAwesomeIconName = "fa-cogs";
			}
			else if (filePath.EndsWith(".com") || filePath.EndsWith(".net")
				  || filePath.EndsWith(".org") || filePath.EndsWith(".edu")
				  || filePath.EndsWith(".gov") || filePath.EndsWith(".mil")
					|| filePath.EndsWith("/") || filePath.EndsWith(".html")
					|| filePath.EndsWith(".htm") || filePath.EndsWith(".xhtml")
					  || filePath.EndsWith(".jhtml") || filePath.EndsWith(".php")
					  || filePath.EndsWith(".php3") || filePath.EndsWith(".php4")
					  || filePath.EndsWith(".php5") || filePath.EndsWith(".phtml")
					  || filePath.EndsWith(".asp") || filePath.EndsWith(".aspx")
					  || filePath.EndsWith(".aspx") || filePath.EndsWith("?")
					  || filePath.EndsWith("#"))
			{
				fontAwesomeIconName = "fa-globe";
			}
			return fontAwesomeIconName;
		}

		public static string GetFileNameFromPath(string hreflink)
		{
			if (!hreflink.StartsWith("http"))
			{
				hreflink = "http://domain.com" + hreflink;
			}
			try
			{
				Uri uri = new Uri(hreflink);
				return Path.GetFileName(uri.LocalPath);
			}
			catch
			{
				return "unknown name";
			}
		}

		public static string GetSizeStringFromSize(int sizeInBytesInt)
		{
			var sizeString = "";
			if (sizeInBytesInt < 1024)
			{
				sizeString = sizeInBytesInt + " B";
			}
			else if (sizeInBytesInt < Math.Pow(1024, 2))
			{
				sizeString = Math.Round((decimal)(sizeInBytesInt / 1024), 1) + " KB";
			}
			else if (sizeInBytesInt < Math.Pow(1024, 3))
			{
				sizeString = Math.Round((decimal)(sizeInBytesInt / Math.Pow(1024, 2)), 1) + " MB";
			}
			else
			{
				sizeString = Math.Round((decimal)(sizeInBytesInt / Math.Pow(1024, 3)), 1) + " GB";
			}

			return sizeString;
		}

		public static List<TItem> GetCsvData<TItem>(string csvData, bool hasHeader = true, WvpDelimiterType delimiterType = WvpDelimiterType.COMMA, CultureInfo Culture = null)
		{
			var records = new List<TItem>();
			if (Culture == null)
				Culture = CultureInfo.InvariantCulture;

			using (TextReader reader = new StringReader(csvData))
			{
				using (var csvReader = new CsvReader(reader, Culture))
				{
					switch (delimiterType)
					{
						case WvpDelimiterType.TAB:
							csvReader.Configuration.Delimiter = "\t";
							break;
						default:
							break;
					}

					csvReader.Configuration.Encoding = Encoding.UTF8;
					csvReader.Configuration.IgnoreBlankLines = true;
					csvReader.Configuration.BadDataFound = null;
					csvReader.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
					csvReader.Configuration.HasHeaderRecord = hasHeader;
					csvReader.Configuration.MissingFieldFound = null;
					records = csvReader.GetRecords<TItem>().ToList();
				}
			}
			return records;
		}

		public static string WriteCsvData<TItem>(List<TItem> records, bool hasHeader = true, WvpDelimiterType delimiterType = WvpDelimiterType.COMMA, CultureInfo Culture = null)
		{
			var result = new StringBuilder();
			if (Culture == null)
				Culture = CultureInfo.InvariantCulture;
         
			using (var writer = new StringWriter(result))
			{
				using (var csv = new CsvWriter(writer, Culture))
				{
					switch (delimiterType)
					{
						case WvpDelimiterType.TAB:
							csv.Configuration.Delimiter = "\t";
							break;
						default:
							break;
					}
               csv.Configuration.HasHeaderRecord = hasHeader;
               csv.Configuration.CultureInfo = Culture;
               csv.Configuration.SanitizeForInjection = true;
               csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
					csv.WriteRecords(records);
				}
			}
			return result.ToString();
		}


		#endregion

	}
}
