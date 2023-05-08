using System.Text.Json.Serialization;

public class Model_Capacity
{
    [JsonPropertyName("ID")]
    public int ID { get; set; }

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("Time")]
    public string Time { get; set; }

    [JsonPropertyName("TableID")]
    public int TableID {get; set; }

    [JsonPropertyName("TotalSeats")]
    public int TotalSeats {get; set; }

    [JsonPropertyName("RemainingSeats")]
    public int RemainingSeats {get; set; }

    public Model_Capacity(int id, DateTime date, string time, int tableID, int totalSeats, int remainingSeats)
    {
        ID = id;
        Date = date;
        Time = time;
        TableID = tableID;
        TotalSeats = totalSeats;
        RemainingSeats = remainingSeats;
    }
}