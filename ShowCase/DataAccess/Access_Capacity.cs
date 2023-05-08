using System.Text.Json;

static class Access_Capacity 
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/capacity.json"));


    public static List<Model_Capacity> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Model_Capacity>>(json);
    }


    public static void WriteAll(List<Model_Capacity> capacity)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(capacity, options);
        File.WriteAllText(path, json);
    }

}