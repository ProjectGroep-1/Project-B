public static class ContactInformation
{
    public static string PhoneNumber = "";
    public static string Adress = "";
    public static string Email = "";

    public static void ChangePhoneNumber(string new_PhoneNumber)
    {
        PhoneNumber = new_PhoneNumber;
    }

    public static void ChangeAdress(string new_Adress)
    {
        Adress = new_Adress;
    }

    public static void ChangeEmail(string new_Email)
    {
        Email = new_Email;
    }

}