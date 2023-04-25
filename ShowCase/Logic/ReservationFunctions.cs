public static class ReservationFunctions
{
    private static ReservationLogic reservationLogic = new ReservationLogic();
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
                Console.WriteLine(err);
            }
            Table ChosenTable = TableFunctions.PickTable(CustomersAmount);

            if (ChosenTable == null)
            {
                Console.WriteLine("No tables available");
                return;
            }

            AccountModel new_costumer = UserLogin.CreateAccount();
            if (new_costumer == null) { return; }
            
            Random r = new Random();
            int n = r.Next(1,9999);

            ReservationModel new_reservation_model = new ReservationModel(n, new_costumer.FullName, ChosenTable.TotalSeats, ChosenTable.Id, null);
            reservationLogic.Reservations.Add(new_reservation_model);
            ReservationAccess.WriteAll(reservationLogic.Reservations);
            new_costumer.ReservationID = n;
            UserLogin.accountsLogic.UpdateList(new_costumer);
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