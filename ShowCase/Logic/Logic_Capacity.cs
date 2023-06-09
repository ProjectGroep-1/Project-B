public class Logic_Capacity : Logic_TimeSlots
{
    private enum Table 
    {
    Low = 2,
    Medium = 4,
    High = 6
    }

    public List<Model_Capacity> _capacity;
    public Logic_Capacity()
    {
        _capacity = Access_Capacity.LoadAll();
    }

    public void ReFillTabels(DateTime Date)
    {
        var query = Functions_Reservation.reservationLogic._reservations.Where(x=> x.Date == Date.Date && x.Id != -1).Select(r => r).ToList(); // Reservations on this Date
        foreach (var reservation in query) {Console.WriteLine( reservation.ToString()); }
        Console.WriteLine("Clean:");
        if (query.Count == 0) {Console.WriteLine("Empty"); return;} 
        // Reservations Clean = (customers amount = 6, 4, 2)
        // Reservations Dirty = (customers amount = 1, 3, 5, >6)
        var queryCleanReservations = query.Where(c => c.CustomersAmount % 2 == 0 && c.CustomersAmount <= (int)Table.High).OrderByDescending(c => c.CustomersAmount).ToList();
        var queryDirtyReservations = query.Where(c => c.CustomersAmount % 2 != 0 || c.CustomersAmount > (int)Table.High).OrderByDescending(c => c.CustomersAmount).ToList();
        foreach (var reservation in queryCleanReservations) {Console.WriteLine( reservation.ToString()); }
        Console.WriteLine("Dirty:");
        foreach (var reservation in queryDirtyReservations) {Console.WriteLine( reservation.ToString()); }
        // Resetting all RemainingSeats on this Date
        for (int i = 0; i < _capacity.Count; i++) { if (_capacity[i].Date == Date.Date) { _capacity[i].RemainingSeats = _capacity[i].TotalSeats; UpdateList(_capacity[i]); } }
        // Refilling tables with Reservations Clean
        Console.WriteLine("Refilling tables:");
        var res = ManualRefill(queryCleanReservations);
        // Refilling tables with Reservations Dirty
        // var resDirty = ManualRefill(queryDirtyReservations, true);
        
        // test writeline
        if (res != null)
        {
            Console.WriteLine($"Reservations without a table: {res.Count}");
            foreach(var reservation in res) { Console.WriteLine(reservation.ToString()); }
        }
    }

    public List<Model_Reservation> ManualRefill(List<Model_Reservation> reservations, bool dirty = false)
    {
        List<Model_Reservation> LeftoverReservations = new();
        if (!dirty)
        {   
            foreach (Model_Reservation res in reservations)
            {
                List<Model_Capacity> free_cap = Functions_Capacity.New_Customer_Table(res.CustomersAmount, res.Arrival, res.Date, true);
                if (free_cap == null || free_cap.Count == 0) { LeftoverReservations.Add(res); }
                else
                {   
                    // test writeline
                    Console.WriteLine($"Reservation {res.Id}, cap id {res.CapacityIDS[0]}, was given the table {free_cap[0].TableID}, new cap id = {free_cap[0].ID}");
                    Model_Capacity new_cap = free_cap[0];
                    new_cap.RemainingSeats = new_cap.TotalSeats - res.CustomersAmount;
                    res.CapacityIDS = new List<int>() { new_cap.ID };
                    UpdateList(new_cap);
                    Functions_Reservation.reservationLogic.UpdateList(res);
                }
            }

        }

        if (dirty)
        {

        }
        return LeftoverReservations;
    }
    public void DailyUpdateCapacity(int days)
    {
        if (_capacity.Count > 0)
        {
            List<Model_Capacity> old_capacity = new List<Model_Capacity>();
            for (int i = 0; i < _capacity.Count; i++)
            {
                if (_capacity[i].RemainingSeats < _capacity[i].TotalSeats) { old_capacity.Add(_capacity[i]); }
            }

            List<Model_Capacity> l = CreateCapacity(days);
            ClearOldReservations(l);

            List<Model_Reservation> Current_Reservations = Functions_Reservation.reservationLogic._reservations;
            for (int i = 0; i < l.Count; i++)
            {
                for (int j = 0; j < old_capacity.Count; j++)
                {
                    if (l[i].Date == old_capacity[j].Date && l[i].Time == old_capacity[j].Time && l[i].TableID == old_capacity[j].TableID)
                    {
                        for (int r = 0; r < Current_Reservations.Count; r++)
                        {
                            if (Current_Reservations[r].CapacityIDS.Contains(old_capacity[j].ID))
                            {
                                int index_cap_id = Current_Reservations[r].CapacityIDS.FindIndex(x=> x == old_capacity[j].ID);
                                Current_Reservations[r].CapacityIDS[index_cap_id] = l[i].ID;
                                Functions_Reservation.reservationLogic.UpdateList(Current_Reservations[r]);
                                Console.WriteLine($"Current Reservation Id:{Current_Reservations[r].Id} Contains old cap id at index:{index_cap_id}. New id:{l[i].ID}");
                            }
                        }
                       
                        l[i].RemainingSeats = old_capacity[j].RemainingSeats;
                    }
                }
            }
            Access_Capacity.WriteAll(l);
        }

        else
        {
            List<Model_Capacity> n = CreateCapacity(days);
            Access_Capacity.WriteAll(n);
        }
    }

    private void ClearOldReservations(List<Model_Capacity> updated_list)
    {
        List<Model_Reservation> Current_Reservations = Functions_Reservation.reservationLogic._reservations;
        List<Model_Account> Current_Accounts = Access_Account.LoadAll();

        for (int r = 0; r < Current_Reservations.Count; r++)
        {
            if (Current_Reservations[r].Date < updated_list[0].Date)
            {
                for (int a = 0; a < Current_Accounts.Count; a++)
                {
                    if (Current_Accounts[a].ReservationIDs.Contains(Current_Reservations[r].Id))
                    {
                        Functions_Reservation.RemoveReservation(Current_Accounts[a], Current_Reservations[r]); 
                    }
                }
            }
        }
    }

    public List<Model_Capacity> CreateCapacity(int days)
    {
        List<Model_Capacity> CapList = new();

        this.CreateTimeSlots();
        DateTime CurrentDate = DateTime.Now.Date;

        int IDs = 1; 
        for (int i = 0; i < days; i++)
        {
            for (int j = 0; j < TimeSlots.Count; j ++)
            {
                Model_TimeSlot tslot = TimeSlots[j];

                for (int k = 0; k < tslot.Tables.Count; k++)
                {
                    Model_Table tbl = tslot.Tables[k];
                    Model_Capacity cap = new(IDs, CurrentDate, tslot.Time, tbl.ID, tbl.TotalSeats, tbl.RemainingSeats);
                    CapList.Add(cap);
                    IDs += 1;
                }
            }
            
            CurrentDate = CurrentDate.AddDays(1);
        }

        return CapList;
    }

    public DateTime? CheckDate(string date)
    {
        DateTime? new_date = this.CovertToDatetime(date);
        return new_date;
    }

    private DateTime? CovertToDatetime(string Date)
    {
        string[] format = {"yyyy-MM-dd"};
        DateTime date;

        if (DateTime.TryParseExact(Date, 
                                format, 
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, 
                                out date))
        {
            return date;
        }

        else
        {
            return null;
        }
    }

    public List<Model_Capacity> Search(string searchType, string searchTerm)
    {
        List<Model_Capacity> usedCapacity = GetUsedCapacity();
        List<Model_Capacity> SearchItems = new List<Model_Capacity>();
        
        foreach(var cap in usedCapacity)
        {
            if (searchType == "1")
            {
                if (!int.TryParse(searchTerm, out int searchTermInt))
                {
                    Console.WriteLine($"You've entered the wrong type of value. Press any key to continue.");
                    return null; 
                }
                if (searchTermInt == cap.ID)
                    SearchItems.Add(cap);
            }
            if (searchType == "2")
            {
                DateTime date;
                if (DateTime.TryParseExact(searchTerm, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    if (cap.Date == date)
                    {
                        SearchItems.Add(cap);
                    }

                }
                else
                    Console.WriteLine("Invalid date format");
            }
            if (searchType == "3")
            {
                if (!int.TryParse(searchTerm, out int searchTermInt))
                {
                    Console.WriteLine($"You've entered the wrong type of value. Press any key to continue.");
                    return null;                  
                }
                if (searchTermInt == cap.TotalSeats)
                    SearchItems.Add(cap);
            }
        }
        return SearchItems;
    }

    public List<Model_Capacity> GetUsedCapacity()
    {
        List<Model_Capacity> usedCapacity = new List<Model_Capacity>();
        foreach (Model_Capacity cap in _capacity)
        {
            if (cap.RemainingSeats < cap.TotalSeats)
                usedCapacity.Add(cap);
        }
        return usedCapacity;
    }

    public Model_Capacity GetById(int id)
    {
        return _capacity.Find(i => i.ID == id);
    }

    public void UpdateList(Model_Capacity capacity)
    {
        //Find if there is already an model with the same id
        int index = _capacity.FindIndex(x => x.ID == capacity.ID);

        if (index != -1)
        {
            //update existing model
            _capacity[index] = capacity;
            Access_Capacity.WriteAll(_capacity);
        }
    }
}