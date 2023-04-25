public static class ReservationFunctions
{
    public static ReservationLogic reservationLogic = new ReservationLogic();
    public static void ReservationMenu()
    {
        Console.WriteLine("Reservation Menu" + "\n" + "\n" + "1: Your reservations" + "\n" + "2: Make a reservation");
        string user = Console.ReadLine();
        Console.Clear();

        if (user == "1")
        {
            AccountModel acc = UserLogin.Start();
            if (acc != null)
            {
                ReservationFunctions.CheckOrder(acc);
            }
        }

        else if (user == "2")
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
            Table ChosenTable = TableFunctions.PickTable(CustomersAmount);

            if (ChosenTable == null)
            {
                Console.WriteLine("No tables available for that amount of people.");
                return;
            }
            

            Console.WriteLine("Already have an account? y/n");
            string question = Console.ReadLine().ToLower();
            AccountModel new_customer = null;
            if (question == "yes" || question == "y")
            {
                 new_customer = UserLogin.Start();
            }
            else
            {
                new_customer = UserLogin.CreateAccount();
             }
            if (new_customer == null) { return; }
            
            int n = RandomId();

            ReservationModel new_reservation_model = new ReservationModel(new_customer.Id, new_customer.FullName, CustomersAmount, ChosenTable.Id, null);
            reservationLogic.UpdateList(new_reservation_model);
            
            ReservationAccess.WriteAll(reservationLogic._reservations);
            new_customer.ReservationID = n;
            Console.WriteLine("Your reservation has been made. You can check your reservation in the 'Your reservations' tab. Press any key to continue.");
        }

        else { Console.WriteLine("Wrong input"); }
    }

    public static void CheckOrder(AccountModel account)
    {
        if (reservationLogic.GetById(account.Id) == null) 
        { 
            Console.WriteLine("\n" + "You currently have 0 reservations");
            return;
        }

        else
        {
            Console.WriteLine("\n" + $"{reservationLogic.GetById(account.Id)}");
        }
    }

    public static int RandomId()
    {
        Random r = new Random();
        int n = r.Next(1,999999);
        return n;
    }

}