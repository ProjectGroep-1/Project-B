public class Model_Table
{
    public int ID {get; set; }
    public int TotalSeats {get; set; }
    public int RemainingSeats {get; set; }

    public Model_Table(int id, int totalSeats)
    {
        ID = id;
        TotalSeats = totalSeats;
        RemainingSeats = totalSeats;
    }
}