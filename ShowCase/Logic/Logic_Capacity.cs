public class Logic_Capacity : Logic_TimeSlots
{
    public List<Model_Capacity> _capacity;
    public Logic_Capacity()
    {
        _capacity = Access_Capacity.LoadAll();
    }

    public void CreateCapacity()
    {
        List<Model_Capacity> CapList = new();

        this.CreateTimeSlots();
        DateTime CurrentDate = DateTime.Now.Date;

        int IDs = 1; 
        for (int i = 1; i < 101; i++)
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

        Access_Capacity.WriteAll(CapList);
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