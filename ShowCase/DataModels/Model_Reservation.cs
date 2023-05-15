using System.Text.Json.Serialization;


public class Model_Reservation
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("Fullname")]
    public string FullName { get; set; }

    [JsonPropertyName("CustomersAmount")]
    public int CustomersAmount { get; set; }
    
    [JsonPropertyName("ItemList")]
    public List<Model_Menu> ItemList {get; private set;}

    [JsonPropertyName("Arrival")]
    public string Arrival { get; set; }

    [JsonPropertyName("CategoryPreference")]
    public string CategoryPreference { get; set; }

    public Model_Reservation(int id, DateTime date, string fullName, int customersAmount, string arrival, string categoryPreference)
    {
        Id = id;
        Date = date;
        FullName = fullName;
        CustomersAmount = customersAmount;
        ItemList = new List<Model_Menu>();
        Arrival = arrival;
        CategoryPreference = categoryPreference;
    }
    public override string ToString()
    {
        return $"Reservation {Id}, Reserved by: {FullName}, {CustomersAmount} people";
    }
}