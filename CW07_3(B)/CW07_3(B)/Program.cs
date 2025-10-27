using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class ProductStoreV2
{
    const string Magic = "MAGC";
    const int VersionV2 = 2;

    record Product(int Id, double Price, string Name, string Category);

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Використання: ProductStoreV2 write | read");
            Console.WriteLine("У дерикторії Properties виберіть файл Settings.json");
            Console.WriteLine("В полі \"commandLineArgs\" вписуйте write - зберегти данні | read - відобразити все що зберегли");
            return;
        }

        string path = Path.Combine("data", "products_v2.bin");
        Directory.CreateDirectory("data");

        if (args[0] == "write")
            WriteFileV2(path);
        else if (args[0] == "read")
            ReadFile(path);
    }
    static void WriteFileV2(string path)
    {
        var products = new List<Product>
        {
            new(1, 10.5, "Клавіатура", "Переферія"),
            new(2, 25.63, "Миш", "Переферія"),
            new(3, 55.45, "Навушники", "Переферія"),
            new(2, 355.50, "Монітор", "Переферія"),
            new(4, 999.99, "Ноутбук", "Компьютер")
        };

        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        using (var bw = new BinaryWriter(fs, Encoding.UTF8, false))
        {
            bw.Write(Encoding.ASCII.GetBytes(Magic));
            bw.Write(VersionV2);

            foreach (var p in products)
            {
                bw.Write(p.Id);
                bw.Write(p.Price);
                bw.Write(p.Name);
                bw.Write(p.Category);
            }
        }

        Console.WriteLine($"Файл версії 2 записано: {path}");
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
                string category = version >= 2 ? br.ReadString() : "(none)";
                Console.WriteLine($"#{id} {name} {price}  Категорія: {category}");
            }
        }
    }
}
