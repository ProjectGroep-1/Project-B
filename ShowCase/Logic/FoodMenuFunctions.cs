public static class FoodMenuFunctions{
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

    private static void SearchSummary(string searchType, string searchTerm, int pageNumber)
    {
        List<MenuItem> SearchedItems = menuLogic.Search(searchType, searchTerm);
        int pageTotal = Convert.ToInt32(Math.Ceiling(SearchedItems.Count / 10.0));
        
        var itemsOnPage = (dynamic)null;
        List<MenuItem> itemsOnPageList = new List<MenuItem>();
        if (SearchedItems.Count > 10){
            itemsOnPage = SearchedItems.Skip((pageNumber - 1) * 10).Take(10);
        }
        else{
            itemsOnPage = SearchedItems;
        }
        foreach (MenuItem item in itemsOnPage)
        {
            itemsOnPageList.Add(item);
        }
        int foodCounter = 1;
        if (itemsOnPage == null || itemsOnPageList.Count == 0)
        {
            Console.WriteLine("There aren't any dishes that comply with your search term.");
            return;
        }
        Console.WriteLine("Menu");
        foreach (MenuItem CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{foodCounter}. {CurrentItem.Name} | {CurrentItem.Price} EUR");
            foodCounter++;
        }
        Console.WriteLine($"Page {pageNumber}/{pageTotal}");
        Console.WriteLine("Previous Page[1], Next Page[2], Add an item to reservation[3], Quit[4]");
        
        string pageFlip = Console.ReadLine();
        switch(pageFlip)
        {
            case "1":
                if (pageNumber == 1)
                {   
                    Console.WriteLine("You are already on the first page. Press any key to continue.");
                    Console.ReadKey();
                    SearchSummary(searchType, searchTerm, pageNumber);
                }
                else
                    pageNumber--;
                    SearchSummary(searchType, searchTerm, pageNumber);
                break;
            case "2":
                if (pageNumber == pageTotal)
                {
                    Console.WriteLine("You are already on the last page. Press any key to continue.");
                    Console.ReadKey();
                    SearchSummary(searchType, searchTerm, pageNumber);
                }
                else
                    pageNumber++;
                    SearchSummary(searchType, searchTerm, pageNumber);
                break;
            case "3":
                if (pageFlip == "3")
                {
                    Console.WriteLine($"Enter the number of the dish you want.[1-{foodCounter-1}]");
                    int dishChoice = Convert.ToInt32(Console.ReadLine());
                    int dishCounter = 1;
                    foreach (var item in itemsOnPageList)
                    {
                        if (dishChoice == dishCounter)
                        {
                            Console.WriteLine(item);
                            Console.ReadKey();
                        }
                        dishCounter++;
                    }
                }
                break;
            case "4":
                break;
            default:
                Console.WriteLine("You entered the wrong value.");
                break;
        }
    }
    
    public static void GetSearchOptions()
    {
        Console.WriteLine("Search by: [1] Max Price, [2] Category, [3] Dish Name");
        string searchType = Console.ReadLine();
        
        switch(searchType)
        {
            case "1":
                Console.WriteLine("Enter a max price");
                string maxPrice = Console.ReadLine();
                FoodMenuFunctions.SearchSummary(searchType, maxPrice, 1);
                break;
            case "2":
                Console.WriteLine("Enter a category");
                string category = Console.ReadLine();
                FoodMenuFunctions.SearchSummary(searchType, category, 1);
                break;
            case "3":
                Console.WriteLine("Enter a dish name");
                string dish = Console.ReadLine();
                FoodMenuFunctions.SearchSummary(searchType, dish, 1);
                break;
            default:
                Console.WriteLine("You entered the wrong value.");
                break;
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