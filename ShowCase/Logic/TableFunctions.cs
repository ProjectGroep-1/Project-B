public static class TableFunctions
{
    private static TableLogic tableLogic = new TableLogic(); 

    public static void ShowCapacity()
    {
        Console.Clear();
        foreach (Table table in tableLogic._tableList)
        {
            if (table.RemainingSeats > 0 /* && Reservation.CustomersAmount <= table.RemainingSeats */)
            {
                Console.WriteLine($"Table: {table.Id}. Available Seats: {table.RemainingSeats}/{table.TotalSeats}");
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