static class FoodMenuFunctions{
    static private FoodMenuLogic menuLogic = new FoodMenuLogic();
    
    public static void MenuSummary(){
        Console.WriteLine("Current menu:");
        foreach (MenuItem currentItem in menuLogic._menuitems)
        {
            Console.WriteLine(currentItem.ToString());
        }
    }
    
    public static MenuItem FindItem(int id){
        return menuLogic.GetById(id);
    }

    public static void AddItem(int id, string name, string category, string course, double price){
        menuLogic.UpdateList(new MenuItem(id, name, category, course, price));
    }
}