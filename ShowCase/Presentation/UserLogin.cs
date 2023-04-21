static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static AccountModel? Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("1: Login with a exisiting account"+ "\n"+ "2: Create a new account");
        string input = Console.ReadLine();
        if (input == "1")
        {
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
            Console.WriteLine("No account found with that email and password");
            return null;
        }
        }
        else if (input == "2")
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
        return null;
        }
        return null;
    }
}