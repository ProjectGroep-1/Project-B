static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static AccountModel? Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        Console.Clear();
        if (acc != null && acc.UserType != "admin")
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your email number is " + acc.EmailAddress);
            return acc;
        }
        else
        {
            Console.WriteLine("No account found with that email and password" + "\n");
            Console.WriteLine("Create a account? y/n");
            string choice = Console.ReadLine().ToLower();
            if (choice == "y") { CreateAccount(); }
            if (choice == "yes") { CreateAccount(); }

            return null;
        }
    }


        public static void CreateAccount()
        { Console.Clear();
        Console.WriteLine("Enter a emailadress:");
        string mail_1 = Console.ReadLine();
        Console.WriteLine("Enter a Password:");
        string password_1 = Console.ReadLine();
        Console.WriteLine("Enter your name:");
        string name_1 = Console.ReadLine();
        var User = new NewUserLogic(mail_1, password_1, name_1);
        User.add_acc();
        
        Console.Clear();
        Console.WriteLine("Your account has been created, you can now login");
    }
}