public static class TableFunctions
{
    private static TableLogic tableLogic = new TableLogic(); 

    public static Table PickTable(int people)
    {
        foreach (Table table in tableLogic._tableList)
        {
            if (people <= table.RemainingSeats)
            {
                table.RemainingSeats -= people;
                tableLogic.UpdateList(table);
                return table;
            }
        }
        return null;
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