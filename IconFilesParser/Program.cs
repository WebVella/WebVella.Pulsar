using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace IconFileParser
{
	static class Program
	{

		static void Main(string[] args)
		{

			var codeFileName = "icon-code.txt";
			var enumFileName = "icon-enum.txt";
			//var enumPrefix = "Mdf";
			//var folderPath = "D:/temp/";
			var enumPrefix = "Bs";
			var folderPath = "D:/temp2/";
			var processedCount = 0;
			if (args.Length > 0)
			{
				folderPath = args[0];
			}
			var files = new DirectoryInfo(folderPath).GetFiles();
			StringBuilder generatedCodeSb = new();
			var generatedEnums = "";

			foreach (var file in files)
			{
				if (!file.Name.EndsWith("svg"))
					continue;

				processedCount++;
				var fileLines = File.ReadAllLines(file.FullName);
				StringBuilder stringBld = new StringBuilder();
				if (enumPrefix == "Bs")
				{
					foreach (var line in fileLines)
					{
						if (fileLines.First() == line || fileLines.Last() == line)
							continue;

						stringBld.Append(line.Trim());
					}
				}
				if (enumPrefix == "Mdf")
				{
					if (!file.Name.Contains("_24px"))
						continue;

					foreach (var line in fileLines)
					{
						stringBld.Append(line.Trim());
					}

					HtmlDocument doc = new HtmlDocument();
					doc.LoadHtml(stringBld.ToString());

					foreach (HtmlNode svg in doc.DocumentNode.SelectNodes("//svg"))
					{
						stringBld.Clear();
						stringBld.Append(svg.InnerHtml);
					}

				}
				var currentFileContent = stringBld.ToString().Replace("\"", "'");


				var nr = System.Environment.NewLine;
				var fileName = file.Name.Replace(".svg", "").Replace("ic_", "").Replace("_24px", "");
				StringBuilder stringBld2 = new StringBuilder();
				var fileNameParts = new List<string>();
				if (enumPrefix == "Bs")
				{
					fileNameParts = fileName.Split("-", StringSplitOptions.RemoveEmptyEntries).ToList();
				}
				if (enumPrefix == "Mdf")
				{
					fileNameParts = fileName.Split("_", StringSplitOptions.RemoveEmptyEntries).ToList();
				}
				foreach (var part in fileNameParts)
				{
					if (part.Length == 0)
						continue;

					if (part.Length == 1)
						stringBld2.Append(char.ToUpper(part[0]));
					else
						stringBld2.Append(char.ToUpper(part[0]) + part.Substring(1));
				}
				var enumString = enumPrefix + stringBld2.ToString();
				var enumDescription = enumPrefix.ToLowerInvariant() + "-" + fileName;

				generatedCodeSb.Append($"iconTypeSvgDict[WvpIconType.{enumString}] = \"{currentFileContent}\";" + nr);

				generatedCodeSb.Append($"[Description(\"{enumDescription}\")]" + nr);
				generatedCodeSb.Append($"{enumString}," + nr);
			}
			File.WriteAllText($"{folderPath}{codeFileName}", generatedCodeSb.ToString());
			File.WriteAllText($"{folderPath}{enumFileName}", generatedEnums);

			Console.WriteLine($"Success. Processed {processedCount} files");
			Console.ReadKey();
		}
	}
}
