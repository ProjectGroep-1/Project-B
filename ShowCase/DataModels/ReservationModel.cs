using System.Text.Json.Serialization;


class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("Fullname")]
    public string FullName { get; set; }

    [JsonPropertyName("CostumersAmount")]
    public int CostumersAmount { get; set; }
    
    [JsonPropertyName("ItemList")]
    public List<string> ItemList {get; private set;}

    [JsonPropertyName("TimeDuration")]
    public double TimeDuration { get; set; }

    [JsonPropertyName("TableId")]
    public int TableId { get; set; }

    [JsonPropertyName("CatagoryPreference")]
    public string CatagoryPreference { get; set; }

    public ReservationModel(int id, string fullName, int costumersAmount, double timeDuration, int tableId, string catagoryPreference)
    {
        Id = id;
        FullName = fullName;
        CostumersAmount = costumersAmount;
        ItemList = new List<string>();
        TimeDuration = timeDuration;
        TableId = tableId;
        CatagoryPreference = catagoryPreference;
    }

}