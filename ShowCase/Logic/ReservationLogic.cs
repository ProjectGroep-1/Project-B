using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class ReservationLogic
{
    private List<ReservationModel> _reservations;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public ReservationLogic()
    {
        _reservations = ReservationAccess.LoadAll();
    }


    public void UpdateList(ReservationModel reservation)
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
        ReservationAccess.WriteAll(_reservations);

    }

    public ReservationModel GetById(int id)
    {
        return _reservations.Find(i => i.Id == id);
    }

}