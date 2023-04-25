using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class ReservationLogic
{
    public List<ReservationModel> Reservations;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public ReservationLogic()
    {
        Reservations = ReservationAccess.LoadAll();
    }


    public void UpdateList(ReservationModel reservation)
    {
        //Find if there is already an model with the same id
        int index = Reservations.FindIndex(s => s.Id == reservation.Id);

        if (index != -1)
        {
            //update existing model
            Reservations[index] = reservation;
        }
        else
        {
            //add new model
            Reservations.Add(reservation);
        }
        ReservationAccess.WriteAll(Reservations);

    }

    public ReservationModel GetById(int id)
    {
        return Reservations.Find(i => i.Id == id);
    }

}