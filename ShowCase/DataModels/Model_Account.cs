﻿using System.Text.Json.Serialization;


public class Model_Account
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    
    [JsonPropertyName("userType")]

    public string UserType {get; private set;}

    [JsonPropertyName("ReservationID")]
    public List<int> ReservationIDs { get; set; }

    public Model_Account(int id, string emailAddress, string password, string fullName, string userType)
    {
        Id = id;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        UserType = userType;
        ReservationIDs = new List<int>();
    }

}




