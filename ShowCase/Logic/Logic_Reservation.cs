using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Logic_Reservation
{
    public List<Model_Reservation> _reservations;



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

     public void UpdateListbyDate(Model_Reservation reservation)
    {
        //Find if there is already an model with the same id
        int index = _reservations.FindIndex(s => s.Date == reservation.Date && s.FullName == reservation.FullName && s.Arrival == reservation.Arrival);

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
        if (account.ReservationIDs == null || !account.ReservationIDs.Any())
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

    public int GetResNewID()
    {
        if (_reservations.Count == 0 || _reservations[_reservations.Count -1].Id <= 0) { return 1; }
        return _reservations[_reservations.Count -1].Id +1;
    }

}