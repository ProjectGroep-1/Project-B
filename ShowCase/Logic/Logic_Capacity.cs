public class Logic_Capacity : Logic_TimeSlots
{
    public List<Model_Capacity> _capacity;
    public Logic_Capacity()
    {
        _capacity = Access_Capacity.LoadAll();
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
                                Console.WriteLine($"Current Reservation Id:{Current_Reservations[r].Id} Contains old cap id at index:{index_cap_id}. New id:{l[i].ID}");
                            }
                        }
                        
                    }
                }
            }
            Access_Capacity.WriteAll(l);
            Access_Reservation.WriteAll(Current_Reservations);
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
                        int index_res_id = Current_Accounts[a].ReservationIDs.FindIndex(x=> x == Current_Reservations[r].Id);
                        Current_Accounts[a].ReservationIDs[index_res_id] = -1;
                        if (Current_Accounts[a].ReservationIDs.Contains(-1)) { Current_Accounts[a].ReservationIDs.RemoveAll(x=> x == -1); }
                    }
                }

                Current_Reservations[r].Id = -1;
            }
        }

        Access_Reservation.WriteAll(Current_Reservations);
        Access_Account.WriteAll(Current_Accounts);
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
}