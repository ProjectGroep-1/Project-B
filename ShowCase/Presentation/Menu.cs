static class Menu
{
    static public void Start()
    {
        while (true)
        {   
            Admin.isAdmin = false;
            Console.Clear();
            Console.WriteLine("Main menu" + "\n");
            Console.WriteLine("1: Information about our restaurant" + "\n" + "2: View menu" + "\n" + "3: Search for dishes in menu" + "\n" + "4: Reservations" + "\n" + "5: Contact information" + "\n" + "6: Close application" + "\n");
            string input = Console.ReadLine();
            

            if (input == Admin.Login())
            {
                AdminMenu();
            }
            else if (input == "1")
            {
                Console.Clear();
                Access_Information.LoadAll();
                Console.ReadKey(true);
            }
            else if (input == "2")
            {
                Console.Clear();
                Functions_Menu.MenuSummary(1);
                Console.ReadKey(true);
            }
            else if (input == "3")
            {
                Console.Clear();
                Functions_Menu.GetSearchOptions();
                Console.ReadKey(true);
            }

            else if (input == "4")
            {
                Console.Clear();
                Functions_Reservation.ReservationMenu();
                Console.ReadKey(true);
                
            }
            else if (input == "5")
            {   
                    Console.Clear();
                    Functions_Contact.DisplayContactInformation();
                    Console.ReadKey(true); 
                
            }
                
            
            else if (input == "6"){
                break;
            }

            else{
                Console.WriteLine("Invalid input");
                Console.ReadKey(true);
            }
            
        }
    }

    public static void AdminMenu()
    {
        while (true)
        {
            Admin.isAdmin = true;
            Console.Clear();
            Console.WriteLine("1: View the menu" + "\n" + "2: Add item" + "\n" + "3: Modify item" + "\n" + "4: Remove items" + "\n" + "5: Search items" + "\n" + "6: Edit Contact Information");
            Console.WriteLine("7: View Capacity" + "\n" + "8: Go back" + "\n");
            string subinput1 = Console.ReadLine();
            int variable = 0;
            int.TryParse(subinput1, out variable);

            if (variable == 1)
            {
                Functions_Menu.MenuSummary(1);
                Console.ReadKey(true);
            }
            else if (variable == 2)
            {
                Functions_Menu.AddItemOptions();
            }
            else if (variable == 3)
            {
                Functions_Menu.ModifyItemOptions();
            }
            else if (variable == 4)
            {
                Functions_Menu.RemoveItemOptions();
            }
            else if (variable == 5)
            {
                Functions_Menu.GetSearchOptions();
            }
            else if (variable == 6)
            {
                Functions_Contact.EditContactInformation();
            }
            else if (variable == 7)
            {
                Console.Clear();
                Functions_Capacity.GetSearchOptions();
                Console.ReadKey(true);
            }
            else if (variable == 8)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input");
                Console.ReadKey(true);
            }
        }
    }
}

