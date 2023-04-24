public static class TableFunctions
{
    private static TableLogic tableLogic = new TableLogic(); 

    public static void ShowCapacity(int people)
    {
        Console.Clear();
        Console.WriteLine("-=- Available Tables -=-");
        foreach (Table table in tableLogic._tableList)
        {
            if (table.RemainingSeats > 0 &&  people <= table.RemainingSeats)
            {
                
                Console.WriteLine($"Table: \x1b[1m{table.Id}\x1b[0m. Available Seats: \x1b[1m{table.RemainingSeats}/{table.TotalSeats}\x1b[0m");
            }
        }
        Console.WriteLine();
    }
    public static Table GetByID(int id)
    {
        foreach (Table table in tableLogic._tableList)
        {
            if (table.Id == id)
            {
                return table;
            }
        }
        return null;
    }

    

}