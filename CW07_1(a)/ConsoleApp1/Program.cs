using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class ConsoleApp1
{
    static async Task Main()
    {
        string inputPath = Path.Combine("data", "input.txt");
        string outputPath = Path.Combine("data", "output.txt");

        string text = File.ReadAllText(inputPath, new UTF8Encoding(false)).Replace("\r\n", "\n");

        int rs = 0;
        string[] lines = text.Split('\n');
        using (var reader = new StreamReader(inputPath, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: true))
        using (var writer = new StreamWriter(outputPath, false, new UTF8Encoding(false)))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                int before = line.Length;
                line = line.TrimEnd();
                rs += before - line.Length;

                await writer.WriteLineAsync(line);
            }
        }
        Console.WriteLine($"Рядків: {lines.Length}");
        Console.WriteLine($"Видалених пробілів: {rs}");
    }

}