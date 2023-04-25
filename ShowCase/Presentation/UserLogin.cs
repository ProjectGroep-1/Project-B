static class UserLogin
{
    static public AccountsLogic accountsLogic = new AccountsLogic();
    

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
            Console.WriteLine("Your e-mail is " + acc.EmailAddress);
            return acc;
        }
        else
        {
            Console.WriteLine("No account found with that email and password" + "\n");
            return null;
        }
    }


        public static AccountModel CreateAccount()
        { Console.Clear();
        Console.WriteLine("Enter an emailadress:");
        string mail_1 = Console.ReadLine();
        Console.WriteLine("Enter a Password:");
        string password_1 = Console.ReadLine();
        Console.WriteLine("Enter your name:");
        string name_1 = Console.ReadLine();

        Random r = new Random();
        int n = r.Next(1,9999);
        AccountModel Acc = new AccountModel(n, mail_1, password_1, name_1, "user");
        List<AccountModel> accountlist = new List<AccountModel>();
        accountlist = AccountsAccess.LoadAll();
        accountlist.Add(Acc);
        AccountsAccess.WriteAll(accountlist);
        
        Console.Clear();
        Console.WriteLine("Your account has been created, you can now login");

        return Acc;
    }
}