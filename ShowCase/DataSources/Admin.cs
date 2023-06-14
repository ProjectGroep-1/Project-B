public static class Admin
{
    public static bool isAdmin = false;
    private static string Username = "Admin1";
    private static string Password = "123";
    public static string Login() => Username + " " + Password;

    public static void ChangeUsername(string new_Username)
    {
        Username = new_Username;
    }

    public static void ChangePassword(string new_Password)
    {
        Password = new_Password;
    }

}