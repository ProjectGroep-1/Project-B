public class Logic_TimeSlots
{
    public List<Model_TimeSlot> TimeSlots { get; set; }
    protected Logic_TimeSlots()
    {
        TimeSlots = new List<Model_TimeSlot>();
    }
    protected void CreateTimeSlots()
    {
        List<Model_TimeSlot> TimeSlot = new List<Model_TimeSlot>();
        for (int openingtime = 16; openingtime < 24; openingtime += 2)
        {
            string time = Convert.ToString(openingtime) + "H";
            List<Model_Table> tables = this.CreateTables();
            Model_TimeSlot new_timeslot = new(time , tables);
            TimeSlot.Add(new_timeslot);
        }
        TimeSlots = TimeSlot;
    }

    private List<Model_Table> CreateTables()
    {
        List<Model_Table> tables = new List<Model_Table>();
        int seats = 2;

        for (int i = 1; i < 17; i++)
        {
            if (i == 9 || i == 15 ) { seats += 2; }

            Model_Table new_table = new(i, seats);
            tables.Add(new_table);
        }
        return tables; 
    }
}