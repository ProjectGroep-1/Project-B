static class FoodMenuFunctions{
    static private FoodMenuLogic menuLogic = new FoodMenuLogic();
    public static void MenuSummary(){
        Console.WriteLine("Current menu:");
        Console.Write("Choose page: ");
        int Page = 1;
        try{
            var itemsOnPage= (dynamic)null;
            int pageNumber = Convert.ToInt32(Console.ReadLine());
            if (menuLogic._menuitems.Count > 10){
                itemsOnPage = menuLogic._menuitems.Skip((pageNumber - 1) * 10).Take(10);
            }
            else{
                itemsOnPage = menuLogic._menuitems;
            }
             // pagination, partly from https://stackoverflow.com/questions/319973/how-to-get-first-n-elements-of-a-list-in-c
            foreach (MenuItem CurrentItem in itemsOnPage)
            {
                Console.WriteLine(CurrentItem.ToString());
            }
            Console.WriteLine($"End of page {Page}, press Escape to go back to the main menu");
        }
        catch (Exception ArgumentOutOfRangeException){
            Console.WriteLine($"End of page {Page}, press Escape to go back to the main menu");
        }
       
    }
    
    public static MenuItem FindItem(int id){
        return menuLogic.GetById(id);
    }

    public static void AddItem(int id, string name, string category, string course, double price){
        menuLogic.UpdateList(new MenuItem(id, name, category, course, price));
    }

    public static void ReplaceItem(MenuItem ItemReplace){
        menuLogic.UpdateList(ItemReplace);
    }

    public static void RemoveItem(MenuItem item){
        menuLogic.RemoveItem(item);
    }
}