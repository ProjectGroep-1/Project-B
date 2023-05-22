public static class Functions_Capacity
{
    public static Logic_Capacity capacitylogic = new(); 

    private static List<Model_Capacity> itemsOnPageList = new List<Model_Capacity>();

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

    public static void SearchSummary(string searchType, string searchTerm, int pageNumber)
    {
        List<Model_Capacity> SearchedItems = capacitylogic.Search(searchType, searchTerm);
        if (SearchedItems == null)
            return;

        List<Model_Capacity> usedCapacity = capacitylogic.GetUsedCapacity();
        
        int pageTotal = Convert.ToInt32(Math.Ceiling(usedCapacity.Count / 10.0));
        var itemsOnPage= (dynamic)null;
        itemsOnPageList.Clear();
        if (usedCapacity.Count > 10){
            itemsOnPage = usedCapacity.Skip((pageNumber - 1) * 10).Take(10);
        }
        else{
            itemsOnPage = usedCapacity;
        }

        foreach (Model_Capacity cap in itemsOnPage)
        {
            itemsOnPageList.Add(cap);
        }
        int capCounter = 1;
        Console.WriteLine("Capacity:");
        foreach (Model_Capacity CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{capCounter}. {CurrentItem.ID} ");
            capCounter++;
        }
        Console.WriteLine($"\x1b[1mPage {pageNumber}/{pageTotal}\x1b[0m");
        // pageNumber = FlipPage(pageNumber, pageTotal, foodCounter);
        // if (pageNumber == 0)
        // {
        //     Console.WriteLine("Going back to Main menu. Press any key to confirm.");
        //     pageNumber = 1;
        //     return;
        // }
        // else
        // {
        //     Console.Clear();
        //     MenuSummary(pageNumber);
        // }
    }

    public static void GetSearchOptions()
    {
        Console.WriteLine("Search by [1] ID, [2] Date, [3] Total seats, [4] Quit");
        string searchType = Console.ReadLine();

        switch(searchType)
        {
            case "1":
                Console.WriteLine("Enter an id");
                string id = Console.ReadLine();
                SearchSummary(searchType, id, 1);
                break;
            case "2":
                Console.WriteLine("Enter a date (yyyy-mm-dd)");
                string date = Console.ReadLine();
                SearchSummary(searchType, date, 1);
                break;
            case "3":
                Console.WriteLine("Enter an amount of seats");
                string amount = Console.ReadLine();
                SearchSummary(searchType, amount, 1);
                break;
            case "4":
                Console.WriteLine("Going back to Main Menu. Press any key to continue");
                return;
            default:
                Console.WriteLine("You entered the wrong value. Press any key to continue.");
                break;
        }
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

        capacitylogic._capacity = Access_Capacity.LoadAll();
        for (int i = 0; i < capacitylogic._capacity.Count; i ++)
        {
            if (return_list.Contains(capacitylogic._capacity[i].Time)) {}
            else { return_list.Add(capacitylogic._capacity[i].Time); }    
        }

        return return_list;
    }

    public static string DisplayDate(int resID)
    {
        foreach (var cap in capacitylogic._capacity)
        {
            if (cap.ID == resID)
                return ($"On {cap.Date.Day}" + $"-{cap.Date.Month}" + $"-{cap.Date.Year}, Time: {cap.Time}");
        }
        return "";
    }

}