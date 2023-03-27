using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class FoodMenuLogic
{
    public List<MenuItem> _menuitems;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself

    public FoodMenuLogic()
    {
        _menuitems = FoodMenuAccess.LoadMenuJSON();
    }


    public void UpdateList(MenuItem item)
    {
        //Find if there is already an model with the same id
        int index = _menuitems.FindIndex(s => s.Id == item.Id);

        if (index != -1)
        {
            //update existing model
            _menuitems[index] = item;
        }
        else
        {
            //add new model
            _menuitems.Add(item);
        }
        FoodMenuAccess.WriteAll(_menuitems);

    }

    public MenuItem GetById(int id)
    {
        return _menuitems.Find(i => i.Id == id);
    }

}
