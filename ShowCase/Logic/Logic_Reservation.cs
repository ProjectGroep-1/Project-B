using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class Logic_Reservation
{
    public List<Model_Reservation> _reservations;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public Logic_Reservation()
    {
        _reservations = Access_Reservation.LoadAll();
    }


    public void UpdateList(Model_Reservation reservation)
    {
        //Find if there is already an model with the same id
        int index = _reservations.FindIndex(s => s.Id == reservation.Id);

        if (index != -1)
        {
            //update existing model
            _reservations[index] = reservation;
        }
        else
        {
            //add new model
            _reservations.Add(reservation);
        }
        Access_Reservation.WriteAll(_reservations);

    }

    public bool CheckReservationList(Model_Account account)
    {
        if (account.ReservationIDs == null && !account.ReservationIDs.Any())
            return false;
        return true;
    }

    public  Model_Reservation ChooseReservation(Model_Account account, int id)
    {
        if (id <= account.ReservationIDs.Count)
        {
            id -= 1;
            return GetById(account.ReservationIDs[id]);
        }
        return null;
    }

    public Model_Reservation GetById(int id)
    {
        return _reservations.Find(i => i.Id == id);
    }

}