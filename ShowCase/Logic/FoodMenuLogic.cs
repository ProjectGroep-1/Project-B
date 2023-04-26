using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class FoodMenuLogic
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

    public List<MenuItem> Search(string searchType, string searchTerm)
    {
        List<MenuItem> SearchItems = new List<MenuItem>();
        foreach (var item in _menuitems)
        { 
            if (searchType == "1")
            {
                double.TryParse(searchTerm, out double searchTermDouble);
                if (searchTermDouble == 0)
                {
                    Console.WriteLine($"You've entered the wrong type of value. Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();  
                }
                if (item.Price < Convert.ToDouble(searchTermDouble))
                {
                    SearchItems.Add(item);
                }
            }
            if (searchType == "2")
            {
                if (item.Category.ToLower() == searchTerm.ToLower())
                {
                    SearchItems.Add(item);
                }
            }
            if (searchType == "3")
            {
                if (item.Name.ToLower().Contains(searchTerm.ToLower()))
                {
                    SearchItems.Add(item);
                }
            }
        }
            
        
        return SearchItems;
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
