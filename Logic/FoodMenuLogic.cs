using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class FoodMenuLogic
{
    public List<MenuItem> _menuitems;

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

    public void RemoveItem(MenuItem item){
        _menuitems.Remove(item);
        _menuitems.IndexOf(item);
        FoodMenuAccess.WriteAll(_menuitems);
    }


}
