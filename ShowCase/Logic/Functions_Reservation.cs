public static class Functions_Reservation
{
    public static Logic_Reservation reservationLogic = new Logic_Reservation();
    public static void ReservationMenu()
    {
        Console.WriteLine("Reservation Menu" + "\n" + "\n" + "1: Your reservations" + "\n" + "2: Make a reservation");
        string user = Console.ReadLine();
        Console.Clear();

        if (user == "1")
        {
            if (Functions_Account.CurrentAccount != null)
            {
                Functions_Reservation.CheckOrder(Functions_Account.CurrentAccount);
            }
            else
            {
                UserLogin.Start();
                if (Functions_Account.CurrentAccount != null)
                {
                    Functions_Reservation.CheckOrder(Functions_Account.CurrentAccount);
                }
            }

        }

        else if (user == "2")
        {
            Console.WriteLine("Already have an account? y/n");
            string question = Console.ReadLine().ToLower();
            if (question == "yes" || question == "y")
            {
                if (Functions_Account.CurrentAccount == null)
                    UserLogin.Start();
            }
            if (question == "no" || question == "n")
                UserLogin.CreateAccount();

            if (Functions_Account.CurrentAccount == null)
            {
                Console.WriteLine("You didn't manage to login. Going back to the main menu");
                return;
            }

            ReservationOptions(Functions_Account.CurrentAccount);
        }

        else { Console.WriteLine("Wrong input"); }
    }

    private static void ReservationOptions(Model_Account new_customer)
    {
        Console.WriteLine("For how many people is this reservation?");
        string groupSize = Console.ReadLine();
        int CustomersAmount = 0;
        try{
            CustomersAmount = Convert.ToInt32(groupSize);
        }
        catch (Exception err){
            Console.WriteLine("Please enter a number. Press any key to continue.");
            return;
        }

        Console.WriteLine("Pick a Date. YYYY-MM-DD");
        string user_date = Console.ReadLine();

        if (user_date == null) 
        {
            Console.WriteLine("Please enter a correct date");
            return;
        }
        DateTime? date = Functions_Capacity.CheckCostumerDate(user_date);

        if (date == null)
        {
            Console.WriteLine("Please enter a date in the correct format. YYYY-MM-DD");
            return;
        }

        Console.WriteLine("Pick an hour" + "\n");
        List<string> hours = Functions_Capacity.ShowCurrentOpeningHours();
        if (hours.Count > 8 || hours.Count < 1) { return; }

        int options = 1;
        for (int i = 0; i < hours.Count; i++)
        {
            string option = Convert.ToString(options) + ": ";
            Console.WriteLine(option + hours[i]); 
            options += 1;
        }

        string hourinstring = Console.ReadLine();

        int hour = 0;
        try{
            hour = Convert.ToInt32(hourinstring);
        }
        catch (Exception err){
            Console.WriteLine("Please enter a number. Press any key to continue.");
            return;
        }

        if (hour < 1 || hour > hours.Count)
        {
            Console.WriteLine("Wrong option");
            return;
        }

        string correct_hour = hours[hour - 1];

        List<Model_Capacity> capacity_list = Functions_Capacity.New_Customer_Table(CustomersAmount, correct_hour, (DateTime) date);

        if (capacity_list == null || capacity_list.Count < 1)
        {
            Console.WriteLine("No Table found. Please enter other date/time.");
            return;
        }

        if (capacity_list.Count == 1) 
        { 
            Console.WriteLine("Free Table found. Confirm reservation? y/n");
            string q = Console.ReadLine().ToLower();
            if (q != "yes")
            {
                if (q != "y")
                {
                    Console.WriteLine("Reservation cancelled.");
                    return;
                }
            }
            Functions_Capacity.Confirm_New_Customer(capacity_list[0], CustomersAmount); 
            Console.WriteLine("Please enter a CategoryPreference.");
            string CategoryPreference = Console.ReadLine();
            ConfirmReservation(capacity_list[0], new_customer, CustomersAmount, CategoryPreference);
            return;
        }
        if (capacity_list.Count > 1) 
        { 
            Console.WriteLine("Please enter a CategoryPreference.");
            string CategoryPreferenceMulticap = Console.ReadLine();
            ConfirmReservationMulticaps(capacity_list, new_customer, CustomersAmount, CategoryPreferenceMulticap);
        }
        
    }

    private static void ConfirmReservation(Model_Capacity capacity, Model_Account new_customer, int CustomersAmount, string CategoryPreference)
    {
        int new_res_id = Functions_Reservation.reservationLogic.GetResNewID();
        List<int> CapIDs = new(){capacity.ID};
        Model_Reservation new_reservation_model = new Model_Reservation(new_res_id, capacity.Date, new_customer.FullName, CustomersAmount, capacity.Time, CategoryPreference, CapIDs);
        reservationLogic.UpdateList(new_reservation_model);
        
        Functions_Account.AddReservationToAccount(new_res_id);
        
        
        Console.WriteLine("Your reservation has been made. You can check your reservation in the 'Your reservations' tab. Press any key to continue.");
    }

    private static void ConfirmReservationMulticaps(List<Model_Capacity> capacity, Model_Account new_customer, int CustomersAmount, string CategoryPreference)
    {
        int new_res_id = Functions_Reservation.reservationLogic.GetResNewID();
        List<int> CapIDs = new();
        foreach (Model_Capacity caps in capacity) 
        {
            CapIDs.Add(caps.ID);
        }
        Model_Reservation new_reservation_model = new Model_Reservation(new_res_id, capacity[0].Date, new_customer.FullName, CustomersAmount, capacity[0].Time, CategoryPreference, CapIDs);
        reservationLogic.UpdateList(new_reservation_model);
        Functions_Account.AddReservationToAccount(new_res_id);
        Functions_Capacity.Confirm_Multiple_Customers(capacity);
        
        Console.WriteLine("Your reservation has been made. You can check your reservation in the 'Your reservations' tab. Press any key to continue.");
    }

    public static void CheckOrder(Model_Account account)
    {
        if (!reservationLogic.CheckReservationList(account)) 
        { 
            Console.WriteLine("\n" + "You currently have 0 reservations");
            return;
        }

        else
        {

            PrintReservations(account.ReservationIDs);
            Model_Reservation r = FindReservation(Functions_Account.CurrentAccount);
            if (r == null) { return; }
            Console.Clear();
            int option = CancelMenu();
            Console.Clear();
            if (option == 1)
            {
                Console.WriteLine("Confirm cancellation? y/n");
                string confirm_cancel = Console.ReadLine().ToLower();
                if (confirm_cancel == "y" || confirm_cancel == "yes") 
                { 
                    Model_Account new_current_user = RemoveReservation(Functions_Account.CurrentAccount, r); 
                    Functions_Account.SetCurrentAccount(new_current_user);
                }
                return;
            }
            Console.WriteLine($"{r}\nThese are the dishes that you added to your reservation: \n");

            foreach(Model_Menu item in r.ItemList)
            {
                Console.WriteLine(item.Name);
            }
        }
    }

    public static bool AddItemToReservation(Model_Menu dish)
    {
        if (Functions_Account.CurrentAccount == null)
        {
            Console.WriteLine("You need to be logged in to add an item to a reservation. Press any key to continue.");
            Console.ReadKey();
            return false;
        }
        if (!reservationLogic.CheckReservationList(Functions_Account.CurrentAccount))
        {
            Console.WriteLine("You do not have a reservation to add items to. Press any key to continue.");
            Console.ReadKey();
            return false;
        }
        PrintReservations(Functions_Account.CurrentAccount.ReservationIDs);
        Model_Reservation r = FindReservation(Functions_Account.CurrentAccount);
        if (r == null)
            return false;
        r.ItemList.Add(dish);
        reservationLogic.UpdateList(r);
        return true;
    }

    private static void PrintReservations(List<int> ids)
    {
        Console.Clear();
        int counter = 1;
        if (ids.Count > 0)
        {
            foreach(int id in ids)
            {
                Model_Reservation r = reservationLogic.GetById(id);
                Console.WriteLine($"{counter}. {r} {Functions_Capacity.DisplayDate(id)}");
                counter++;
            }
        }

    }

    public static Model_Reservation FindReservation(Model_Account account)
    {

        Console.WriteLine($"Choose a reservation [1-{account.ReservationIDs.Count}]");

        int resID;
        string input = Console.ReadLine();
        bool convertInput = int.TryParse(input, out resID);
        if (!convertInput)
        {
            Console.WriteLine("Please enter a number");
            Console.ReadKey();
            Console.Clear();
            return null;
        }
        Model_Reservation r = reservationLogic.ChooseReservation(account, resID);
        if (r == null)
        {
            Console.WriteLine($"Please enter a number between [1-{account.ReservationIDs.Count}]");
            Console.ReadKey();
            Console.Clear();
            return null;
        }
        return r;
    }

    public static void PrintReservationDishes(int resID)
    {
        Console.Clear();
        Model_Reservation r = Functions_Reservation.reservationLogic.GetById(resID);
        Console.WriteLine($"{r} {Functions_Capacity.DisplayDate(resID)}");
        if (r.ItemList.Count == 0)
            Console.WriteLine("This reservation doesn't have any dishes yet.");
        foreach (Model_Menu item in r.ItemList)
        {
            Console.WriteLine(item.Name);
        } 
        Console.ReadKey();
    }

    public static int CancelMenu()
    {
        int option;
        while (1 < 2)
            {
                Console.Clear();
                Console.WriteLine("1: Cancel reservation" + "\n" + "2: Show dishes you added to your reservation");
                string input = Console.ReadLine();
                bool convertInput = int.TryParse(input, out option);
                if (convertInput)
                {
                    if (option == 1 || option == 2)
                    {
                        return option;
                    }
                }
                

            }
    }

    public static Model_Account RemoveReservation(Model_Account account, Model_Reservation removal)
    {
        List<Model_Reservation> Current_Reservations = Functions_Reservation.reservationLogic._reservations;
        if (account.ReservationIDs.Contains(removal.Id))
        {
            account.ReservationIDs.Remove(removal.Id);
            Functions_Account.accountLogic.UpdateList(account);
            int index_reservation_id = Current_Reservations.FindIndex(x=> x.Id == removal.Id);
            if (index_reservation_id != -1) { removal.Id = -1; Functions_Reservation.reservationLogic.UpdateListbyDate(removal); }
        }

        return account;
    }

}