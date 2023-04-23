using System.Text.Json;


public static class TableAccess{
    static string data = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/tables.json"));
    
    public static List<Table> LoadTableJSON()
    {
        string load_json = File.ReadAllText(data);
        return JsonSerializer.Deserialize<List<Table>>(load_json);
    }

}




/* Implement the menu */