using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

class TextNormalizer
{
    static async Task Main()
    {
        string inputPatch = Path.Combine("data", "input");
        string outputPatch = Path.Combine("data", "output");
        long maxSize = 10 * 1024 * 1024;

        Directory.CreateDirectory(outputPatch);


        List<string> reportLines = new() { "Файл;Кількість_рядків;Видалено_пробілів" };

        foreach (string filePath in Directory.EnumerateFiles(inputPatch, "*.txt", SearchOption.AllDirectories))
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                if (fi.Length > maxSize)
                {
                    Console.WriteLine($"Пропущено (файл >10 МБ): {filePath}");
                    continue;
                }

                string relativePath = Path.GetRelativePath(inputPatch, filePath);
                string destPath = Path.Combine(outputPatch, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);

                int totalSpacesRemoved = 0;
                int lineCount = 0;

                using (var reader = new StreamReader(filePath, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: true))
                using (var writer = new StreamWriter(destPath, false, new UTF8Encoding(false)))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        int before = line.Length;
                        line = line.TrimEnd();
                        totalSpacesRemoved += before - line.Length;
                        await writer.WriteLineAsync(line);
                        lineCount++;
                    }
                }

                Console.WriteLine($"{relativePath} — рядків: {lineCount}, видалено пробілів: {totalSpacesRemoved}");
                reportLines.Add($"{relativePath};{lineCount};{totalSpacesRemoved}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка з файлом {filePath}: {ex.Message}");
            }
        }

        string Zvit = Path.Combine(outputPatch, "report.csv");
        await File.WriteAllLinesAsync(Zvit, reportLines, new UTF8Encoding(false));
        Console.WriteLine($"\nЗвіт збережено у: {Zvit}");
    }
}
