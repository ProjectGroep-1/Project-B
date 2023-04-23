public static class ReservationFunctions
{
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
            if (groupSize == "1" || groupSize == "2" || groupSize == "3" || groupSize == "4" || groupSize == "5" || groupSize == "6")
            {
                // Should be CustomersAmount from the Reservation Model.
                int CustomersAmount = Convert.ToInt32(groupSize);
            }
            TableFunctions.ShowCapacity();
            
        }

        else { Console.WriteLine("Wrong input"); }
    }

    public static void CheckOrder(AccountModel account)
    {
        if (account.ReservationID == 0) 
        { 
            Console.WriteLine("\n" + "You currently have 0 reservations");
            return;
        }

        else
        {
            Console.WriteLine("\n" + "Reservation");
        }
    }

}