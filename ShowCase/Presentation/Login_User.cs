static class UserLogin
{
    static public Logic_Account accountsLogic = new Logic_Account();
    

    public static Model_Account? Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = "";
        while (true)
        {
        ConsoleKeyInfo key = Console.ReadKey(true);
    
        if (key.Key == ConsoleKey.Enter)
        {
        break;
        }
        else if (key.Key == ConsoleKey.Backspace)
        {
        if (password.Length > 0)
        {
            password = password.Substring(0, password.Length - 1);
            Console.Write("\b \b");
        }
        }
        else
        {
        password += key.KeyChar;
        Console.Write("*");
        }
    }   
        Model_Account acc = Functions_Account.CheckLogin(email, password);
        Console.Clear();
        if (acc != null && acc.UserType != "admin")
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your e-mail is " + acc.EmailAddress);
            Functions_Account.SetCurrentAccount(acc);
            return acc;
        }
        else
        {
            Console.WriteLine("No account found with that email and password" + "\n");
            return null;
        }
    }


        public static Model_Account CreateAccount()
        { Console.Clear();
        Console.WriteLine("This is the account page");
        Console.WriteLine("Enter an emailadress:");
        string mail_1 = Console.ReadLine();
        string password = "";
        Console.WriteLine("please enter your password: ");
        while (true)
        {
        ConsoleKeyInfo key = Console.ReadKey(true);
    
        if (key.Key == ConsoleKey.Enter)
        {
        break;
        }
        else if (key.Key == ConsoleKey.Backspace)
        {
        if (password.Length > 0)
        {
            password = password.Substring(0, password.Length - 1);
            Console.Write("\b \b");
        }
        }
        else
        {
        password += key.KeyChar;
        Console.Write("*");
        }
    }   
        Console.WriteLine();
        Console.WriteLine("Enter your name:");
        string name_1 = Console.ReadLine();


        int n = accountsLogic.GetNewID();
        Model_Account Acc = new Model_Account(n, mail_1, password, name_1, "user");
        List<Model_Account> accountlist = new List<Model_Account>();
        if (!accountsLogic.UpdateList(Acc))
            return null;

        accountlist = Access_Account.LoadAll();
        Functions_Account.SetCurrentAccount(Acc);
        Access_Account.WriteAll(accountlist);
        Console.Clear();

        return Acc;
    }
}