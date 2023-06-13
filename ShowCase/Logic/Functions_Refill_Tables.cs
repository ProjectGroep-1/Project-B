public static class Functions_Refill_Tables
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/LastRefillDate.txt"));

    public static string Read()
    {
        return File.ReadAllText(path);
    }

    public static void Write(string text)
    {
        File.WriteAllText(path, text);
    }

    public static bool Refill()
    {
        
        string last_date = Read();
        if (last_date == DateTime.Now.ToString("yyyyMMdd"))
            {
                return false;
            }

        Write(DateTime.Now.ToString("yyyyMMdd"));
        Functions_Capacity.capacitylogic.ReFillTabels(DateTime.Now);
        return true;
    }
}