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
        if (query.Count == 0) { return; } 
        // Reservations Clean = (customers amount = 6, 4, 2)
        // Reservations Dirty = (customers amount = 1, 3, 5, >6)
        var queryCleanReservations = query.Where(c => c.CustomersAmount % 2 == 0 && c.CustomersAmount <= (int)Table.High).OrderByDescending(c => c.CustomersAmount).ToList();
        var queryDirtyReservations = query.Where(c => c.CustomersAmount % 2 != 0 || c.CustomersAmount > (int)Table.High).OrderByDescending(c => c.CustomersAmount).ToList();
        // Resetting all RemainingSeats on this Date
        for (int i = 0; i < _capacity.Count; i++) { if (_capacity[i].Date == Date.Date) { _capacity[i].RemainingSeats = _capacity[i].TotalSeats; UpdateList(_capacity[i]); } }
        // Refilling tables with Reservations Clean
        var resClean = ManualRefill(queryCleanReservations);
        // Any Reservations Clean not given a table will be added to the Reservations Dirty
        if  (resClean != null && resClean.Count > 0) {foreach (Model_Reservation reservation in resClean) { queryDirtyReservations.Add(reservation); }}
        // Refilling tables with Reservations Dirty
        var resDirty = ManualRefill(queryDirtyReservations, true);
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
            foreach (Model_Reservation res in reservations)
            {
                List<Model_Capacity> free_cap_list = SplitReservations(res.CustomersAmount, res.Arrival, res.Date, true);
                if (free_cap_list != null && free_cap_list.Count > 0)
                {
                    if (LeftoverReservations.Contains(res)) { LeftoverReservations.Remove(res); }
                    res.CapacityIDS = new List<int>();
                    foreach (Model_Capacity new_cap_split in free_cap_list) {res.CapacityIDS.Add(new_cap_split.ID); UpdateList(new_cap_split); }
                    Functions_Reservation.reservationLogic.UpdateList(res);
                }

            }
        }
        return LeftoverReservations;
    }

    public List<Model_Capacity> SplitReservations(int customers, string hour, DateTime date, bool Manually = false)
    {
        List<Model_Capacity> tables_with_remaining_seats = new();
        if (customers < 4) { tables_with_remaining_seats = _capacity.Where(c => c.Date == date.Date && c.Time == hour && c.RemainingSeats > 0).Select(c => c).OrderBy(t => t.RemainingSeats).ToList(); }
        else { tables_with_remaining_seats = _capacity.Where(c => c.Date == date.Date && c.Time == hour && c.RemainingSeats > 0).Select(c => c).OrderByDescending(t => t.RemainingSeats).ToList(); }

        return Splitting(tables_with_remaining_seats, customers);
    }

    public List<Model_Capacity> Splitting(List<Model_Capacity> capacity, int customers)
    {
        if (capacity == null || capacity.Count == 0) { return null; }
        Dictionary<List<Model_Capacity>, int> options = new Dictionary<List<Model_Capacity>, int> {};
        List<Model_Capacity> tables = new();
        int group_to_split = customers;
        int amount_of_splits = 0;
        List<Tuple<Model_Capacity, int>> TablesToLower = new List<Tuple<Model_Capacity, int>>();
        for (int c = 0; c < capacity.Count; c++)
        {
            bool split_has_occured = false;
            if (group_to_split == 0) 
            {
                tables.OrderBy(t => t.TotalSeats);
                options.Add(tables, amount_of_splits);
                tables = new(); 
                amount_of_splits = 0; 
                group_to_split = customers; 
            }
            if (group_to_split >= capacity[c].RemainingSeats) 
            { 
                group_to_split -= capacity[c].RemainingSeats; 
                TablesToLower.Add(new Tuple<Model_Capacity, int>(capacity[c], capacity[c].RemainingSeats));
                if(group_to_split != 0)
                {
                    amount_of_splits += 1;
                } 
                tables.Add(capacity[c]); 
                split_has_occured = true; 
            }
            if (!split_has_occured) 
            { 
                if (group_to_split < capacity[c].RemainingSeats) 
                { 
                    TablesToLower.Add(new Tuple<Model_Capacity, int>(capacity[c], group_to_split));                   
                    group_to_split = 0; 
                    tables.Add(capacity[c]);
                } 
            }
        }

        if (options != null && options.Count > 0)
        {


            var best_option = options.MinBy(c => c.Value);

            foreach (Model_Capacity table in best_option.Key)
            {
                foreach (var table2 in TablesToLower)
                {
                    if (table2.Item1 == table)
                        table.RemainingSeats -= table2.Item2; 
                }
            }   
            return best_option.Key;

        }

        return null;
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

    public List<Model_Reservation> Search(string searchType, string searchTerm)
    {
        List<Model_Reservation> Reservations = new();

        if (searchType == "1") // Res ID
            {
                if (!int.TryParse(searchTerm, out int searchTermInt))
                {
                    Console.WriteLine($"You've entered the wrong type of value. Press any key to continue.");
                    return null; 
                }
                Reservations = Functions_Reservation.reservationLogic._reservations.Where(r => r.Id == searchTermInt).Select(r => r).ToList();
                return Reservations;
            }
       
        if (searchType == "2") // Date
                {
                    DateTime date;
                    if (DateTime.TryParseExact(searchTerm, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
                    {
                        Reservations = Functions_Reservation.reservationLogic._reservations.Where(r => r.Date == date.Date && r.Id > 0).Select(r => r).ToList();
                        return Reservations;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format");
                        return null;
                    }

                }
        
        if (searchType == "3") // Total Seats
            {
                    if (!int.TryParse(searchTerm, out int searchTermInt))
                    {
                        Console.WriteLine($"You've entered the wrong type of value. Press any key to continue.");
                        return null;                  
                    }
                    var temp = Functions_Reservation.reservationLogic._reservations.Where(r => r.Id > 0).Select(r => r).ToList();
                    List<Model_Reservation> reservations_out = new();
                    if (temp != null && temp.Count > 0)
                    {
                        foreach (Model_Reservation res in temp)
                        {
                            foreach (int cap_id in res.CapacityIDS)
                            {
                                Model_Capacity cap = GetById(cap_id);
                                if (cap != null && cap.TotalSeats == searchTermInt)
                                {reservations_out.Add(res);}
                            }
                        }
                        Reservations = reservations_out;
                        return Reservations;
                    }
            }

        return Reservations;
    }

    public List<Model_Capacity> GetUsedCapacity()
    {
        List<Model_Capacity> usedCapacity = new List<Model_Capacity>();
        List<Model_Reservation> uniqueReservations = new List<Model_Reservation>();
        foreach (Model_Reservation res in Functions_Reservation.reservationLogic._reservations)
        {
            if (!uniqueReservations.Contains(res))
            {
                foreach(int id in res.CapacityIDS)
                {
                    Model_Capacity cap = GetById(id);
                    if (!usedCapacity.Contains(cap))
                    {
                        usedCapacity.Add(cap);
                    }

                }
            }

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