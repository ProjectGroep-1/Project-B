static class Access_Information
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/RestaurantInfo.txt"));


    public static void LoadAll()
    {

        string info = File.ReadAllText(path);
        Console.WriteLine(info + "\nPress escape to exit back to the main menu");
    }
}