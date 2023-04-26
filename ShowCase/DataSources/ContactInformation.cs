public static class ContactInformation
{
    public static string PhoneNumber = "0104962016";
    public static string Address = "Wijnhaven 107, 3011 WN in Rotterdam";
    public static string Email = "JakeDarcy@email.com";

    public static void ChangePhoneNumber(string new_PhoneNumber)
    {
        PhoneNumber = new_PhoneNumber;
    }

    public static void ChangeAdress(string new_Address)
    {
        Address = new_Address;
    }

    public static void ChangeEmail(string new_Email)
    {
        Email = new_Email;
    }

    public static void DisplayInformation()
    {
        Console.WriteLine("Contact Information:" + "\n" + "\n" + "Phone Number: " + PhoneNumber + "\n" + "Address: " + Address + "\n" + "Email: " + Email);
    }

}