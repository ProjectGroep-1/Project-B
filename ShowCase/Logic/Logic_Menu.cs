using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class Logic_Menu
{
    public List<Model_Menu> _menuitems;

    public Logic_Menu()
    {
        _menuitems = Access_Menu.LoadMenuJSON();
    }


    public void UpdateList(Model_Menu item)
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
        Access_Menu.WriteAll(_menuitems);

    }

    public List<Model_Menu> Search(string searchType, string searchTerm)
    {
        List<Model_Menu> SearchItems = new List<Model_Menu>();
        foreach (var item in _menuitems)
        { 
            if (searchType == "1")
            {
                double.TryParse(searchTerm, out double searchTermDouble);
                if (searchTermDouble == 0)
                {
                    Console.WriteLine($"You've entered the wrong type of value. Press any key to continue.");
                    return null; 
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
        

    public Model_Menu GetById(int id)
    {
        return _menuitems.Find(i => i.Id == id);
    }

    public void RemoveItem(Model_Menu item){
        _menuitems.Remove(item);
        _menuitems.IndexOf(item);
        Access_Menu.WriteAll(_menuitems);
    }


}
