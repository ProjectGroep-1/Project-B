static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine($"You are a {acc.UserType}");
            Console.WriteLine("Your email number is " + acc.EmailAddress);
            Console.WriteLine("Press escape to continue");
            //Write some code to go back to the menu
            Console.ReadKey(true);
            Menu.Start();
        }
        else
        {
            Console.WriteLine("No account found with that email and password");
        }
    }
}