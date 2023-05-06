public class Model_TimeSlot
{
    public string Time { get; set; }
    public List<Model_Table> Tables;
    public Model_TimeSlot(string time, List<Model_Table> tables)
    {
        Time = time;
        Tables = tables;
    }
}