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
                Model_Account acc = UserLogin.Start();
                if (acc != null)
                {
                    Functions_Reservation.CheckOrder(acc);
                }
            }

        }

        else if (user == "2")
        {

            Console.WriteLine("Already have an account? y/n");
            string question = Console.ReadLine().ToLower();
            Model_Account new_customer = null;
            if (question == "yes" || question == "y")
            {
                 new_customer = UserLogin.Start();
            }

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

            if (Functions_Capacity.New_Customer_Table(CustomersAmount, correct_hour, (DateTime) date) == null)
            {
                Console.WriteLine("No Table found. Please enter other date/time.");
                return;
            }

            Model_Capacity new_res = Functions_Capacity.New_Customer_Table(CustomersAmount, correct_hour, (DateTime) date);

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

            Functions_Capacity.Confirm_New_Customer(new_res, CustomersAmount);
            
            if (new_customer == null)
            {
                new_customer = UserLogin.CreateAccount();
            }
            
            Console.WriteLine("Please enter a CategoryPreference.");
            string CategoryPreference = Console.ReadLine();

            Model_Reservation new_reservation_model = new Model_Reservation(new_res.ID, new_customer.FullName, CustomersAmount, new_res.Time, CategoryPreference);
            reservationLogic.UpdateList(new_reservation_model);
            
            List<int> old = new_customer.ReservationIDs;
            old.Add(new_res.ID);
            new_customer.ReservationIDs = old;
            UserLogin.accountsLogic.UpdateList(new_customer);
            

            Console.WriteLine("Your reservation has been made. You can check your reservation in the 'Your reservations' tab. Press any key to continue.");
        }

        else { Console.WriteLine("Wrong input"); }
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

            Console.WriteLine("\n" + $"{r}\nThese are the dishes that you added to your reservation: \n");

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
        if(!reservationLogic.CheckReservationList(Functions_Account.CurrentAccount))
        {
            Console.WriteLine("You do not have a reservation to add items to. Press any key to continue.");
            Console.ReadKey();
            return false;
        }
        PrintReservations(Functions_Account.CurrentAccount.ReservationIDs);
        Model_Reservation r = FindReservation(Functions_Account.CurrentAccount);
        r.ItemList.Add(dish);
        reservationLogic.UpdateList(r);
        return true;
    }

    private static void PrintReservations(List<int> ids)
    {
        int counter = 1;
        foreach(int id in ids)
        {
            Model_Reservation r = reservationLogic.GetById(id);
            Console.WriteLine($"{counter}. {r.Id}");
        }
    }

    public static Model_Reservation ChooseReservation(Model_Account account, int id)
    {
        if (id <= account.ReservationIDs.Count)
        {
            id -= 1;
            return reservationLogic.GetById(account.ReservationIDs[id]);
        }
        return null;
    }

    public static Model_Reservation FindReservation(Model_Account account)
    {
        Console.WriteLine($"Choose a reservation [1-{Functions_Account.CurrentAccount.ReservationIDs.Count}]");

        int resID;
        string input = Console.ReadLine();
        bool convertInput = int.TryParse(input, out resID);
        if (!convertInput)
        {
            Console.WriteLine("Please enter a number");
            Console.ReadKey();
            return null;
        }
        Model_Reservation r = ChooseReservation(Functions_Account.CurrentAccount, resID);
        if (r == null)
        {
            Console.WriteLine($"Please enter a number between [1-{Functions_Account.CurrentAccount.ReservationIDs.Count}");
            Console.ReadKey();
            return null;
        }
        return r;
    }





    public static int RandomId()
    {
        Random r = new Random();
        int n = r.Next(1,999999);
        return n;
    }

}