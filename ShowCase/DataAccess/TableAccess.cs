using System.Text.Json;


public static class TableAccess{
    static string data = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/tables.json"));
    
    public static List<Table> LoadTableJSON()
    {
        string load_json = File.ReadAllText(data);
        return JsonSerializer.Deserialize<List<Table>>(load_json);
    }
    public static void WriteAll(List<Table> tables)
    {
        string json = JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(data, json);
    }

}




/* Implement the menu */