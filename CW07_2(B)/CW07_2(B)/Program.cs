using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string configDir = "data";
        string configPath = Path.Combine(configDir, "appsettings.txt");
        string tempPath = Path.Combine(configDir, "appsettings.tmp");
        string backupPath = Path.Combine(configDir, "appsettings.bak");

        Directory.CreateDirectory(configDir);

        string[] newConfig =
        {
            "Testkey=abramchik-228",
            "TestValue=322",
            "TestWork=rabotaet",
            "TestTime=45"
        };

        try
        {
            using (FileStream fs = new FileStream(
                tempPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
            {
                foreach (string line in newConfig)
                    await writer.WriteLineAsync(line);
                await writer.FlushAsync();
                fs.Flush(true);
            }

           
            File.Replace(tempPath, configPath, backupPath, ignoreMetadataErrors: true);

            Console.WriteLine("Конфігурацію оновлено успішно.");
            Console.WriteLine($"Резервна копія створена: {backupPath}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Помилка вводу/виводу: {ex.Message}");
        }

    }
}
