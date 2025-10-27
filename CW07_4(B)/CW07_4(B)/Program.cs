using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

class UserProfileV2
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = "";
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";
    [JsonPropertyName("registered_utc")]
    public DateTimeOffset RegisteredUtc { get; set; }
    [JsonIgnore]
    public bool IsInternal { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; } = "N/A";
}

class ProgramV2
{
    static void Main(string[] args)
    {
        string path = Path.Combine("data", "profiles.json");
        Directory.CreateDirectory("data");

        if (args.Length == 0)
        {
            Console.WriteLine("Використання: save | load");
            Console.WriteLine("У дерикторії Properties виберіть файл Settings.json");
            Console.WriteLine("В полі \"commandLineArgs\" вписуйте save - зберегти данні  load - відобразити все що зберегли");
            return;
        }

        if (args[0] == "save")
            SaveList(path);
        else if (args[0] == "load")
            LoadList(path);
    }

    static void SaveList(string path)
    {
        var profiles = new List<UserProfileV2>
        {
            new() { Id = 1, FullName = "Іван Иванов", Email = "ivan@example.com", RegisteredUtc = DateTimeOffset.UtcNow, Phone = "+380501234567" },
            new() { Id = 2, FullName = "Абубакар Магамедович", Email = "abubakar@chubaka.com", RegisteredUtc = DateTimeOffset.UtcNow, Phone = "+380671234567" },
            new() { Id = 3, FullName = "Testik Testiokv", Email = "Test@chubaka.com", RegisteredUtc = DateTimeOffset.UtcNow, Phone = "+380TestCount" }
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(profiles, options);
        File.WriteAllText(path, json);

        Console.WriteLine($"Збережено {profiles.Count} профілі(в) у {path}");
    }

    static void LoadList(string path)
    {
        string json = File.ReadAllText(path);
        var profiles = JsonSerializer.Deserialize<List<UserProfileV2>>(json) ?? new();

        foreach (var p in profiles)
            Console.WriteLine($"ID={p.Id}, Ім’я={p.FullName}, Email={p.Email}, Телефон={p.Phone}, UTC={p.RegisteredUtc:O}");
    }
}
