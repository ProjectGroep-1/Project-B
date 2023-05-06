using System.Text.Json;

static class Access_Account
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/accounts.json"));


    public static List<Model_Account> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Model_Account>>(json);
    }


    public static void WriteAll(List<Model_Account> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }

    public static void GetContactInformation()
    {
        ContactInformation.DisplayInformation();
    }
}