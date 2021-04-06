using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace IconFileParser
{
	class Program
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
			var generatedCode = "";
			var generatedEnums = "";

			foreach (var file in files)
			{
				if (!file.Name.EndsWith("svg"))
					continue;

				processedCount++;
				var fileLines = File.ReadAllLines(file.FullName);
				var currentFileContent = "";
				if (enumPrefix == "Bs")
				{
					foreach (var line in fileLines)
					{
						if (fileLines.First() == line || fileLines.Last() == line)
							continue;

						currentFileContent += line.Trim();
					}
				}
				if (enumPrefix == "Mdf")
				{
					if (!file.Name.Contains("_24px"))
						continue;

					foreach (var line in fileLines)
					{
						currentFileContent += line.Trim();
					}

					HtmlDocument doc = new HtmlDocument();
					doc.LoadHtml(currentFileContent);

					foreach (HtmlNode svg in doc.DocumentNode.SelectNodes("//svg"))
					{
						currentFileContent = svg.InnerHtml;
					}

				}
				currentFileContent = currentFileContent.Replace("\"", "'");


				var nr = System.Environment.NewLine;
				var fileName = file.Name.Replace(".svg", "").Replace("ic_", "").Replace("_24px", "");
				var processedFileName = "";
				var fileNameParts = new List<string>();
				if (enumPrefix == "Bs"){
					fileNameParts = fileName.Split("-", StringSplitOptions.RemoveEmptyEntries).ToList();
				}
				if (enumPrefix == "Mdf"){
					fileNameParts = fileName.Split("_", StringSplitOptions.RemoveEmptyEntries).ToList();
				}
				foreach (var part in fileNameParts)
				{
					if (part.Length == 0)
						continue;
					else if (part.Length == 1)
						processedFileName += char.ToUpper(part[0]);
					else
						processedFileName += char.ToUpper(part[0]) + part.Substring(1);
				}
				var enumString = enumPrefix + processedFileName;
				var enumDescription = enumPrefix.ToLowerInvariant() + "-" + fileName;

				generatedCode += $"iconTypeSvgDict[WvpIconType.{enumString}] = \"{currentFileContent}\";" + nr;

				generatedEnums += $"[Description(\"{enumDescription}\")]" + nr;
				generatedEnums += $"{enumString}," + nr;
			}
			File.WriteAllText($"{folderPath}{codeFileName}", generatedCode);
			File.WriteAllText($"{folderPath}{enumFileName}", generatedEnums);

			Console.WriteLine($"Success. Processed {processedCount} files");
			Console.ReadKey();
		}
	}
}
