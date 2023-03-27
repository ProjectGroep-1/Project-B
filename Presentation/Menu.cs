static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        while (true)
        {   Console.WriteLine("\n");
            Console.WriteLine("Main menu" + "\n");
            Console.WriteLine("1: Current menu" + "\n" + "2: Information about our restaurant" + "\n" + "3: Your reservation" + "\n" + "4: Contact" + "\n");

            string message = "Under maintenance";
            string input = Console.ReadLine();
            
            if (input == "1")
            {
                FoodMenuFunctions.MenuSummary();
                
            }
            else if (input == "2")
            {
                Console.WriteLine(message);
                
            }
            else if (input == "3")
            {
                Console.WriteLine(message);
                
            }
            else if (input == "4")
            {
                AccountsAccess.GetContactInformation();
                
            }
            else{
                Console.WriteLine("Invalid input");
            }
            
        }


    }
}
