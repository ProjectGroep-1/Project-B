using System.Text.Json;


static class FoodMenuAccess{
    static string data = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/menu.json"));
    
    public static List<MenuItem> LoadMenuJSON()
    {
        string load_json = File.ReadAllText(data);
        return JsonSerializer.Deserialize<List<MenuItem>>(load_json);
    }

    public static void WriteAll(List<MenuItem> menu){
        string json = JsonSerializer.Serialize(menu);
        File.WriteAllText(data, json);
    }
}




/* Implement the menu */