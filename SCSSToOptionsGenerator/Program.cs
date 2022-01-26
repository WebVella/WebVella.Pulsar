using System.Text;

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
    if (file.Name != fileName)
        continue;

    var nr = System.Environment.NewLine;

    var fileLines = File.ReadAllLines(file.FullName);
    foreach (var line in fileLines)
    {
        if (!line.StartsWith("$") && !line.StartsWith(@"//"))
            continue;
        var lineText = line.Trim().Replace(";", "");

        if (line.StartsWith(@"//"))
        {
            stringBldGeneratedCode.Append(line);
            stringBldGeneratedCode.Append(nr);
        }
        else if(line.StartsWith("$"))
        {
            var columnSplit = lineText.Split(":", StringSplitOptions.RemoveEmptyEntries);
            if(columnSplit.Length != 2)
                continue;

            var colorVariable = columnSplit[0];
            var colorHex = columnSplit[1];
            var colorName = colorVariable.Replace("$ttg-","");

            stringBldGeneratedCode.Append("result.Add(new WvSelectOption");
            stringBldGeneratedCode.Append(nr);
            stringBldGeneratedCode.Append("{");
            stringBldGeneratedCode.Append(nr);
            stringBldGeneratedCode.Append($"Color = \"{colorHex}\",");
            stringBldGeneratedCode.Append(nr);
            stringBldGeneratedCode.Append($"Value = \"{colorName}\",");
            stringBldGeneratedCode.Append(nr);
            stringBldGeneratedCode.Append($"Label = \"{colorName}\",");
            stringBldGeneratedCode.Append(nr);
            stringBldGeneratedCode.Append($"IconClass = \"fas fa-square\",");
            stringBldGeneratedCode.Append(nr);
            stringBldGeneratedCode.Append("});");
            stringBldGeneratedCode.Append(nr);
            generatedClasses++;
        }
    }
}

File.WriteAllText($"{folderPath}_color_options.cs", stringBldGeneratedCode.ToString());

Console.WriteLine($"Success! {generatedClasses} classes generated");
Console.ReadKey();