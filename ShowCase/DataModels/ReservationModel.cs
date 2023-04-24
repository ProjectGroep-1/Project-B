using System.Text.Json.Serialization;


class ReservationModel
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Fullname")]
    public string FullName { get; set; }

    [JsonPropertyName("CustomersAmount")]
    public int CustomersAmount { get; set; }
    
    [JsonPropertyName("ItemList")]
    public List<MenuItem> ItemList {get; private set;}

    [JsonPropertyName("TimeDuration")]
    public double TimeDuration { get; set; }

    [JsonPropertyName("TableId")]
    public int TableId { get; set; }

    [JsonPropertyName("CategoryPreference")]
    public string CategoryPreference { get; set; }

    public ReservationModel(int id, string fullName, int customersAmount, double timeDuration, int tableId, string categoryPreference)
    {
        Id = id;
        FullName = fullName;
        CustomersAmount = customersAmount;
        ItemList = new List<MenuItem>();
        TimeDuration = timeDuration;
        TableId = tableId;
        CategoryPreference = categoryPreference;
    }

}