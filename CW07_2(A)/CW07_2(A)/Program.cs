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

        Directory.CreateDirectory(configDir);


        string[] settings =
        {
            "Testkey=abramchik-228",
            "TestValue=322",
            "TestWork=rabotaet"
        };

        try
        {
 
            using (FileStream fs = new FileStream(
                configPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
            {
                foreach (string line in settings)
                    await writer.WriteLineAsync(line);

               
                await writer.FlushAsync();

         
                fs.Flush(true);
            }

            Console.WriteLine($"Конфігурацію успішно збережено у: {configPath}");
        }
    }
}