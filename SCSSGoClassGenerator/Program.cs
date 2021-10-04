using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SCSSGoClassGenerator
{
	static class Program
	{
		static void Main(string[] args)
		{
			var fileName = "_colors.scss";
			var folderPath = "D:/temp/";
			if (args.Length > 0)
			{
				folderPath = args[0];
			}

			var files = new DirectoryInfo(folderPath).GetFiles();
			StringBuilder stringBldGeneratedCode = new();
			var generatedClasses = 0;
			foreach (var file in files)
			{
				if(file.Name != fileName)
					continue;

				var nr = System.Environment.NewLine;

				var fileLines = File.ReadAllLines(file.FullName);
				foreach (var line in fileLines)
				{
					if (!line.StartsWith("$"))
						continue;

					var lineText = line.Trim().Replace(";","");
					var columnSplit = lineText.Split(":",StringSplitOptions.RemoveEmptyEntries);
					var colorVariable = columnSplit[0];
					var isNewColor = false;
					var dashColorVariableSplit = colorVariable.Split("-",StringSplitOptions.RemoveEmptyEntries);
					if(dashColorVariableSplit.Length == 2){
						isNewColor = true;
					}

					var goClassName = colorVariable.Replace("$ttg","go");
					var goBkgClassName = colorVariable.Replace("$ttg","go-bkg");

					var goClassLine = $".{goClassName}{{color: {colorVariable} !important;}}";
					var goBkgClassLine = $".{goBkgClassName}{{background-color: {colorVariable} !important;}}";

					if(isNewColor){
						stringBldGeneratedCode.Append(nr);
						stringBldGeneratedCode.Append($"//{dashColorVariableSplit[1]}" + nr);
					}

					stringBldGeneratedCode.Append($"{goClassLine}" + nr);
					stringBldGeneratedCode.Append($"{goBkgClassLine}" + nr);
					generatedClasses++;
				}
			}

			File.WriteAllText($"{folderPath}_go-color.scss", stringBldGeneratedCode.ToString());

			Console.WriteLine($"Success! {generatedClasses} classes generated");
			Console.ReadKey();
		}
	}
}
