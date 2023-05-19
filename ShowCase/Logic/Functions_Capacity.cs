public static class Functions_Capacity
{
    public static Logic_Capacity capacitylogic = new(); 

    public static void JsonSize()
    {
        Console.WriteLine("Size json: " + capacitylogic._capacity.Count);
    }

    public static void DisplayReservations()
    { 
        int total_reservation = 0;
        for (int i = 0; i < capacitylogic._capacity.Count; i ++)
        {
            Model_Capacity cap = capacitylogic._capacity[i];

           if (cap.RemainingSeats < cap.TotalSeats)
           {
                Console.WriteLine($"Reservation on: {cap.Date.Day}" + $"-{cap.Date.Month}" + $"-{cap.Date.Year}" + "\n" + $"Time: {cap.Time}" + "\n" + $"Table #{cap.TableID}" + "\n" + $"People: {cap.TotalSeats - cap.RemainingSeats}" + "\n" + $"Reservation ID: {cap.ID}" + "\n");
                total_reservation += 1;
           }
        }

        if (total_reservation == 0) { Console.WriteLine("No future Reservations"); }
        else { Console.WriteLine($"Total future reservations: {total_reservation}"); }

    }

    public static Model_Capacity? GetCapByDate(string date)
    {
        Model_Capacity cap = GetCapByDate(date);
        return cap;
    }

    public static void ClearByID(Model_Capacity model_capacity)
    {
        int index = capacitylogic._capacity.FindIndex(s => s.ID == model_capacity.ID);

        if (index > 0) 
        {
            capacitylogic._capacity[index].RemainingSeats = capacitylogic._capacity[index].TotalSeats; 
            Access_Capacity.WriteAll(capacitylogic._capacity);
        }

    }

    public static Model_Capacity? New_Customer_Table(int customers, string hour, DateTime date)
    {
        // Checking for free capacity
        DateTime customer_date = date.Date;
        Model_Capacity free_cap = null;

        for (int i = 0; i < capacitylogic._capacity.Count; i ++)
        {
            if (capacitylogic._capacity[i].Date == customer_date && capacitylogic._capacity[i].Time == hour && capacitylogic._capacity[i].RemainingSeats >= customers)
            {
                free_cap = capacitylogic._capacity[i];
                break;
            }
        }

        return free_cap;
    }

    public static void Confirm_New_Customer(Model_Capacity model_capacity, int costumers)
    {
        // model capacity alread checked
        int index = capacitylogic._capacity.FindIndex(s => s.ID == model_capacity.ID);
        capacitylogic._capacity[index].RemainingSeats -= costumers;
        Access_Capacity.WriteAll(capacitylogic._capacity);
    }

    public static DateTime? CheckCostumerDate(string date)
    {
        DateTime? new_date =  capacitylogic.CheckDate(date);
        return new_date;
    }

    public static List<string> ShowCurrentOpeningHours()
    {
        List<string> return_list = new List<string>();

        for (int i = 0; i < capacitylogic._capacity.Count; i ++)
        {
            if (return_list.Contains(capacitylogic._capacity[i].Time)) {}
            else { return_list.Add(capacitylogic._capacity[i].Time); }    
        }

        return return_list;
    }

    public static string DisplayDate(Model_Reservation reservation)
    {
        foreach (var cap in capacitylogic._capacity)
        {
            if (cap.ID == reservation.Id)
                return ($"On {cap.Date.Day}" + $"-{cap.Date.Month}" + $"-{cap.Date.Year}, Time: {cap.Time}");
        }
        return "";
    }

}