using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class ProductStore
{
    const string Magic = "MAGC";
    const int Version = 1;
    record Product(int Id, double Price, string Name);

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Використання: ProductStore write | read");
                        Console.WriteLine("У дерикторії Properties виберіть файл Settings.json");
            Console.WriteLine("В полі \"commandLineArgs\" вписуйте write - зберегти данні | read - відобразити все що зберегли");
            return;
        }
        string path = Path.Combine("data", "products.bin");
        Directory.CreateDirectory("data");

        if (args[0] == "write")
            WriteFileV1(path);
        else if (args[0] == "read")
            ReadFile(path);
    }

    static void WriteFileV1(string path)
    {
        var products = new List<Product>
        {
            new(1, 10.5, "Молоко"),
            new(2, 25.99, "Помідор"),
            new(3, 99.99, "Шоколад"),
            new(4, 75.20, "Кавун")
        };

        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        using (var bw = new BinaryWriter(fs, Encoding.UTF8, false))
        {
            bw.Write(Encoding.ASCII.GetBytes(Magic));
            bw.Write(Version);

            foreach (var p in products)
            {
                bw.Write(p.Id);
                bw.Write(p.Price);
                bw.Write(p.Name);
            }
        }

        Console.WriteLine($"Файл версії записано: {path}");
    }

    static void ReadFile(string path)
    {
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var br = new BinaryReader(fs, Encoding.UTF8, false))
        {
            string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
            int version = br.ReadInt32();

            Console.WriteLine($"Файл: {magic}, версія: {version}");

            while (fs.Position < fs.Length)
            {
                int id = br.ReadInt32();
                double price = br.ReadDouble();
                string name = br.ReadString();
                Console.WriteLine($"#{id} {name} {price}");
            }
        }
    }
}
