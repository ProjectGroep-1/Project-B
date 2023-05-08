using System.Text.Json.Serialization;

public class Model_Menu
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Category")]
    public string Category { get; set; }

    [JsonPropertyName("Course")]
    public string Course {get; set;}
    [JsonPropertyName("Price")]
    public double Price {get; set;}

    public Model_Menu(int id, string name, string category, string course, double price)
    {
        Id = id;
        Name = name;
        Category = category;
        Course = course;
        Price = price;
    }

    public override string ToString()
    {
        return $"ID: {this.Id} | Name: {this.Name} | Category: {this.Category} | Course: {this.Course} | Price: {$"{this.Price:0.00}"} EUR";
    }

}
