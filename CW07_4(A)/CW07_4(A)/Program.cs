using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

class UserProfile
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
}

class Program
{
    static void Main(string[] args)
    {
        string path = Path.Combine("data", "profile.json");
        Directory.CreateDirectory("data");

        if (args.Length == 0)
        {
            Console.WriteLine("Використання: save | load");
            return;
        }
        if (args[0] == "save")
            Save(path);
        else if (args[0] == "load")
            Load(path);
    }

    static void Save(string path)
    {
        var profile = new UserProfile
        {
            Id = 3,
            FullName = "Верещагін Сергій",
            Email = "testSerhii@gmail.com",
            RegisteredUtc = DateTimeOffset.UtcNow,
            IsInternal = true
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(profile, options);
        File.WriteAllText(path, json);

        Console.WriteLine($"Профіль збережено у {path}");
    }

    static void Load(string path)
    {
        string json = File.ReadAllText(path);
        var profile = JsonSerializer.Deserialize<UserProfile>(json);
        Console.WriteLine($"ID={profile!.Id}, Ім’я={profile.FullName}, Email={profile.Email}, Зареєстровано={profile.RegisteredUtc:O}");
    }
}
