using System.Text.Json;


public static class Access_Menu{
    static string data = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/menu.json"));
    
    public static List<Model_Menu> LoadMenuJSON()
    {
        string load_json = File.ReadAllText(data);
        return JsonSerializer.Deserialize<List<Model_Menu>>(load_json);
    }

    public static void WriteAll(List<Model_Menu> menu){
        string json = JsonSerializer.Serialize(menu, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(data, json);
    }
}




/* Implement the menu */