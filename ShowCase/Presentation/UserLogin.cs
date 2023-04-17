static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static AccountModel Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        Console.Clear();
        if (acc != null)
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your email number is " + acc.EmailAddress);
            return acc;
        }
        else
        {
            Console.WriteLine("No account found with that email and password");
            return null;
        }
    }
}