static class UserLogin
{
    static public Logic_Account accountsLogic = new Logic_Account();
    
    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");

        string password = accountsLogic.EnteringPassword();

        Model_Account acc = Functions_Account.CheckLogin(email, password);
        Console.Clear();
        if (acc != null && acc.UserType != "admin")
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your e-mail is " + acc.EmailAddress);
            Functions_Account.SetCurrentAccount(acc);
        }
        else
        {
            Console.WriteLine("No account found with that email and password" + "\n");
            return;
        }
    }

    public static void CreateAccount()
    { 
        Console.Clear();
        Console.WriteLine("This is the account page");

        Console.WriteLine("\nEnter an emailadress:");
        string mail_1 = Console.ReadLine();

        Console.WriteLine("Enter your name");
        string name_1 = Console.ReadLine();

        Console.WriteLine("Enter your password");
        string password = accountsLogic.EnteringPassword();
        
        int n = accountsLogic.GetNewID();
        Model_Account Acc = new Model_Account(n, mail_1, password, name_1, "user");

        accountsLogic.UpdateList(Acc);
        Functions_Account.SetCurrentAccount(Acc);
        Console.Clear();
    }
}