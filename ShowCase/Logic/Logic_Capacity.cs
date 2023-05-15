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
            
            for (int i = 0; i < l.Count; i++)
            {
                for (int j = 0; j < old_capacity.Count; j++)
                {
                    if (l[i].Date == old_capacity[j].Date && l[i].Time == old_capacity[j].Time && l[i].TableID == old_capacity[j].TableID)
                    {
                        Model_Reservation new_id_reservation = Functions_Reservation.reservationLogic.GetById(old_capacity[j].ID);
                        Model_Account new_res_id_account = UserLogin.accountsLogic.GetByReservationId(old_capacity[j].ID);
                        if (new_id_reservation != null && new_res_id_account != null)
                        {
                            new_id_reservation.Id = l[i].ID;
                            new_res_id_account.ReservationID = l[i].ID;
                            Functions_Reservation.reservationLogic.UpdateListbyFullName(new_id_reservation); /* Should be by email */
                            UserLogin.accountsLogic.UpdateList(new_res_id_account); /* Updating account and reservation IDS */
                            old_capacity[j].ID = l[i].ID;
                            l[i] = old_capacity[j];
                        }
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
        for (int i = 0; i < Functions_Reservation.reservationLogic._reservations.Count; i++)
        {
            if (Functions_Reservation.reservationLogic._reservations[i].Date < updated_list[0].Date)
            {
                Model_Account acc = UserLogin.accountsLogic.GetByReservationId(Functions_Reservation.reservationLogic._reservations[i].Id);
                if (acc != null)
                {
                    acc.ReservationID = -1;
                    Model_Reservation bad_res = Functions_Reservation.reservationLogic._reservations[i];
                    bad_res.Id = -1;
                    Functions_Reservation.reservationLogic.UpdateListbyFullName(bad_res);
                    // Functions_Reservation.reservationLogic.DeleteReservation(Functions_Reservation.reservationLogic._reservations[i]);
                    UserLogin.accountsLogic.UpdateList(acc);
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

}