public static class FoodMenuFunctions{
    static private FoodMenuLogic menuLogic = new FoodMenuLogic();
    private static List<MenuItem> itemsOnPageList = new List<MenuItem>();
    public static void MenuSummary(int pageNumber){
        
        int pageTotal = Convert.ToInt32(Math.Ceiling(menuLogic._menuitems.Count / 10.0));
        var itemsOnPage= (dynamic)null;
        itemsOnPageList.Clear();
        if (menuLogic._menuitems.Count > 10){
            itemsOnPage = menuLogic._menuitems.Skip((pageNumber - 1) * 10).Take(10);
        }
        else{
            itemsOnPage = menuLogic._menuitems;
        }
        foreach (MenuItem item in itemsOnPage)
        {
            itemsOnPageList.Add(item);
        }
        int foodCounter = 1;
        Console.WriteLine("Menu:");
        foreach (MenuItem CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{foodCounter}. {CurrentItem.Name} | {CurrentItem.Price} EUR");
            foodCounter++;
        }
        Console.WriteLine($"\x1b[1mPage {pageNumber}/{pageTotal}\x1b[0m");
        var pageFlipTuple = FlipPage(pageNumber, pageTotal, foodCounter);
        pageNumber = pageFlipTuple.Item1;
        if (pageNumber == 0)
        {
            Console.WriteLine("Going back to Main menu. Press any key to confirm.");
            return;
        }
        pageTotal = pageFlipTuple.Item2;
        foodCounter = pageFlipTuple.Item3;
        Console.Clear();
        MenuSummary(pageNumber);
       
    }

    private static void SearchSummary(string searchType, string searchTerm, int pageNumber)
    {
        List<MenuItem> SearchedItems = menuLogic.Search(searchType, searchTerm);
        int pageTotal = Convert.ToInt32(Math.Ceiling(SearchedItems.Count / 10.0));
        if (SearchedItems == null)
            return;
        var itemsOnPage = (dynamic)null;
        itemsOnPageList.Clear();
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
            Console.WriteLine("There aren't any dishes that comply with your search term. Press any key to continue.");
            Console.ReadKey();
            Console.Clear();
            GetSearchOptions();
        }
        if (pageTotal == 0)
            return;
        
        Console.WriteLine("Menu");
        foreach (MenuItem CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{foodCounter}. {CurrentItem.Name} | {CurrentItem.Price} EUR");
            foodCounter++;
        }
        Console.WriteLine($"\x1b[1mPage {pageNumber}/{pageTotal}\x1b[0m");

        var pageFlipTuple = FlipPage(pageNumber, pageTotal, foodCounter);
        if (pageFlipTuple.Item1 == 0)
        {
            Console.WriteLine("Going back to Main menu. Press any key to confirm.");
            pageNumber = 1;
            pageTotal = 1;
            foodCounter = 1;
            return;
        }
        else
        {
            pageNumber = pageFlipTuple.Item1;
            pageTotal = pageFlipTuple.Item2;
            foodCounter = pageFlipTuple.Item3;
            Console.Clear();
            SearchSummary(searchType, searchTerm, pageNumber);
        }
    }
    
    public static (int, int, int) FlipPage(int pageNumber, int pageTotal, int foodCounter)
    {
        Console.WriteLine("Previous Page[1], Next Page[2], Add an item to reservation[3], Quit[4]");
        string pageFlip = Console.ReadLine();
        switch(pageFlip)
        {
            case "1":
                if (pageNumber == 1)
                {   
                    Console.WriteLine("You are already on the first page. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                    pageNumber--;
                break;
            case "2":
                if (pageNumber == pageTotal)
                {
                    Console.WriteLine("You are already on the last page. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                    pageNumber++;
                break;
            case "3":
                Console.WriteLine($"Enter the number of the dish you want.[1-{foodCounter-1}]");
                bool foundItem = false;
                string dishChoice = Console.ReadLine();
                int dishCounter = 1;
                foreach (MenuItem item in itemsOnPageList)
                {
                        if (dishChoice == $"{dishCounter}")
                        {
                            Console.WriteLine($"{item.Name}\nThis is your dish. Press any key to continue.");
                            Console.ReadKey();
                            foundItem = true;
                        }
                        dishCounter++;
                }
                if (!foundItem)
                {
                    Console.WriteLine($"Please enter a value between [1-{foodCounter-1}]. Press any key to continue.");
                    Console.ReadKey();
                    return (pageNumber, pageTotal, foodCounter);
                }
                break;
            case "4":
                return (0,0,0);
            default:
                Console.WriteLine("You entered the wrong value. Press any key to continue.");
                Console.ReadKey();
                break;
        }
        return (pageNumber, pageTotal, foodCounter);
    }
    public static void GetSearchOptions()
    {
        Console.WriteLine("Search by: [1] Max Price, [2] Category, [3] Dish Name or [4] Quit");
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
            case "4":
                Console.WriteLine("Going back to Main Menu. Press any key to continue");
                return;
            default:
                Console.WriteLine("You entered the wrong value. Press any key to continue.");
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