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

           if (cap.RemainingSeats < cap.TotalSeats && cap.Date > DateTime.Now)
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

        // List<Model_Capacity> usedCapacity = capacitylogic.GetUsedCapacity();
        
        int pageTotal = Convert.ToInt32(Math.Ceiling(SearchedItems.Count / 10.0));
        var itemsOnPage = (dynamic)null;
        itemsOnPageList.Clear();
        if (SearchedItems.Count > 10){
            itemsOnPage = SearchedItems.Skip((pageNumber - 1) * 10).Take(10);
        }
        else{
            itemsOnPage = SearchedItems;
        }

        foreach (Model_Capacity cap in itemsOnPage)
        {
            itemsOnPageList.Add(cap);
        }
        int capCounter = 1;

        if (itemsOnPage == null || itemsOnPageList.Count == 0)
        {
            Console.WriteLine("There aren't any reservations that comply with your search term. Press any key to continue.");
            return;
        }

        Console.WriteLine("Capacity:");
        foreach (Model_Capacity CurrentItem in itemsOnPageList)
        {
            Model_Reservation r = Functions_Reservation.reservationLogic.GetByCapacityId(CurrentItem.ID);
            Console.WriteLine($"{capCounter}. {r} {Functions_Capacity.DisplayDate(r.Id)}, {DisplayTableIDS(r)}");
            capCounter++;
        }
        Console.WriteLine($"\x1b[1mPage {pageNumber}/{pageTotal}\x1b[0m");
        pageNumber = FlipPage(pageNumber, pageTotal, capCounter, itemsOnPageList);
        if (pageNumber == 0)
        {
            Console.WriteLine("Going back to Main menu. Press any key to confirm.");
            pageNumber = 1;
            return;
        }
        else
        {
            Console.Clear();
            SearchSummary(searchType, searchTerm, pageNumber);
        }
    }

    public static int FlipPage(int pageNumber, int pageTotal, int capCounter, List<Model_Capacity> itemsOnPageList)
    {
        (string FlipOptions, bool PrevAvailable, bool NextAvailable) = Functions_Menu.CheckPrevNextPage(pageNumber, pageTotal);
        Console.WriteLine($"{FlipOptions}View dishes in reservation[V], Quit[Q]");
        string pageFlip = Console.ReadLine();
        switch(pageFlip)
        {
            case "P": case "p":
                if (!PrevAvailable)
                {   
                    Console.WriteLine("Incorrect input. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                    pageNumber--;
                break;
            case "N": case "n":
                if (!NextAvailable)
                {
                    Console.WriteLine("Incorrect input. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                    pageNumber++;
                break;
            case "V": case "v":
                Console.WriteLine($"Enter the number of the reservation you want.[1-{capCounter-1}]");
                string reservationChoice = Console.ReadLine();
                int reservationCounter = 1;
                foreach (Model_Capacity cap in itemsOnPageList)
                {
                    if (reservationChoice == $"{reservationCounter}")
                    {
                        Model_Reservation r = Functions_Reservation.reservationLogic.GetByCapacityId(cap.ID); 
                        if (r != null) { Functions_Reservation.PrintReservationDishes(r.Id); }
                    }
                    reservationCounter++;
                }
                break;
            case "Q": case "q":
                return 0;
            default:
                Console.WriteLine("Incorrect input. Press any key to continue.");
                Console.ReadKey();
                break;
        }
        return pageNumber;
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

    public static List<Model_Capacity> New_Customer_Table(int customers, string hour, DateTime date, bool Manually = false)
    {
        // Checking for free capacity
        List<Model_Capacity> free_cap_list = new();
        Model_Capacity free_cap = null;
        for (int i = 0; i < capacitylogic._capacity.Count; i++)
        {
            if (capacitylogic._capacity[i].Date == date && capacitylogic._capacity[i].Time == hour && capacitylogic._capacity[i].RemainingSeats >= customers)
            {
                free_cap = capacitylogic._capacity[i];
                free_cap_list.Add(free_cap);
                return free_cap_list;
            }
        }

        if (!Manually) { if (free_cap_list.Count == 0 || free_cap_list == null) { 
            free_cap_list = Multiple_Customer_Tables(customers, hour, date); } }

        return free_cap_list;
    }

    public static List<Model_Capacity> Multiple_Customer_Tables(int customers, string hour, DateTime date){
        Dictionary<int, int> options = new Dictionary<int, int> {};
        Console.WriteLine("Looking to split");

        for (int i = 0; i < capacitylogic._capacity.Count; i ++)
        {
            if (capacitylogic._capacity[i].Date == date && capacitylogic._capacity[i].Time == hour && capacitylogic._capacity[i].RemainingSeats <= customers / 2){
                int AmountSplits = 4;
                int SplitTable = 4;
                if (capacitylogic._capacity[i].RemainingSeats > 0)
                {
                    
                    AmountSplits = customers / capacitylogic._capacity[i].RemainingSeats;
                    SplitTable =  customers % capacitylogic._capacity[i].RemainingSeats;
                    
                    Console.WriteLine($"{capacitylogic._capacity[i].ID}, {AmountSplits}");
                    if (SplitTable == 0){
                        options.Add(capacitylogic._capacity[i].ID, AmountSplits);
                
                    }
                }
                
            }
        }
        // trying to split by the least amount of tables
        int LeastSplits = options.Values.Min();
        var selection = options.Where(x => x.Value == LeastSplits).Take(LeastSplits);
        Dictionary<int, int> AvaiableTables = new Dictionary<int, int>{};
        // You can't add a K-V pair directly to a Dictionary, which is why we do this loop
        foreach (KeyValuePair<int,int> table in selection){
            AvaiableTables.Add(table.Key, table.Value);
        }
        List<Model_Capacity> ConfirmedTables = Confirm_New_Customer_MultTables(AvaiableTables, customers);
        return ConfirmedTables;
    }
    public static void Confirm_New_Customer(Model_Capacity model_capacity, int costumers)
    {
        // model capacity alread checked
        int index = capacitylogic._capacity.FindIndex(s => s.ID == model_capacity.ID);
        capacitylogic._capacity[index].RemainingSeats -= costumers;
        Access_Capacity.WriteAll(capacitylogic._capacity);
    }

    public static List<Model_Capacity> Confirm_New_Customer_MultTables(Dictionary<int, int> model_capacity, int costumers)
    {
        List<Model_Capacity> picked_caps_list = new();
        Model_Capacity cap = null;
        //only thing to do there is divide the customers by the amount of splits and write the taken tablesfile
        foreach (KeyValuePair<int, int> capacity in model_capacity){
            int index = capacitylogic._capacity.FindIndex(s => s.ID == capacity.Key);
            Console.WriteLine("bonk");
            int splits = costumers / capacity.Value;
            if (capacitylogic._capacity[index].RemainingSeats <= 0){
                Console.WriteLine("plonk");
            }
            else if (capacitylogic._capacity[index].RemainingSeats >= splits){
                capacitylogic._capacity[index].RemainingSeats -= splits;
                cap = capacitylogic._capacity[index];
                picked_caps_list.Add(cap);
            }
        }
        Console.WriteLine("Free Tables found. Confirm reservation? y/n");
        string q = Console.ReadLine().ToLower();
        if (q != "yes")
        {
            if (q != "y")
            {
                Console.WriteLine("Reservation cancelled.");
                List<Model_Capacity> empty = new();
                return empty;
            }
        }
        Access_Capacity.WriteAll(capacitylogic._capacity);

        return picked_caps_list;
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

    public static string DisplayDate(int ResID)
    {
        string return_string = "";
        Model_Reservation reservation = Functions_Reservation.reservationLogic.GetById(ResID);
        if (reservation != null && reservation.CapacityIDS.Count > 0)
        {
            Model_Capacity cap = Functions_Capacity.capacitylogic.GetById(reservation.CapacityIDS[0]);
            if (cap != null) { return_string += $"On {cap.Date.Day}" + $"-{cap.Date.Month}" + $"-{cap.Date.Year}, Time: {cap.Time}"; }
        }
        return return_string;
    }

    public static string DisplayTableIDS(Model_Reservation reservation)
    {
        string return_string = "Tables: ";
        if (reservation.CapacityIDS.Count < 2)
            return_string = return_string.Replace("Tables", "Table");

        foreach (int capID in reservation.CapacityIDS)
        {
            Model_Capacity cap = Functions_Capacity.capacitylogic.GetById(capID);
            return_string += $"{cap.TableID}, ";
        }
        // removing the last comma and space
        return return_string.Substring(0, return_string.Length -2);
    }
}