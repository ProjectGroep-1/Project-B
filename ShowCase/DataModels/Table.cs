using System.Text.Json.Serialization;

public class Table
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Seats")]
    public int TotalSeats { get; set; }

    [JsonPropertyName("RemainingSeats")]
    public int RemainingSeats {get; set; }


}