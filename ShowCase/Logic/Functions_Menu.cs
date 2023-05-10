public static class Functions_Menu{
    static private Logic_Menu menuLogic = new Logic_Menu();
    private static List<Model_Menu> itemsOnPageList = new List<Model_Menu>();
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
        foreach (Model_Menu item in itemsOnPage)
        {
            itemsOnPageList.Add(item);
        }
        int foodCounter = 1;
        Console.WriteLine("Menu:");
        foreach (Model_Menu CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{foodCounter}. {CurrentItem.Name} | {CurrentItem.Price} EUR");
            foodCounter++;
        }
        Console.WriteLine($"\x1b[1mPage {pageNumber}/{pageTotal}\x1b[0m");
        pageNumber = FlipPage(pageNumber, pageTotal, foodCounter);
        if (pageNumber == 0)
        {
            Console.WriteLine("Going back to Main menu. Press any key to confirm.");
            pageNumber = 1;
            return;
        }
        else
        {
            Console.Clear();
            MenuSummary(pageNumber);
        }
       
    }

    private static void SearchSummary(string searchType, string searchTerm, int pageNumber)
    {
        List<Model_Menu> SearchedItems = menuLogic.Search(searchType, searchTerm);
        if (SearchedItems == null)
            return;
        int pageTotal = Convert.ToInt32(Math.Ceiling(SearchedItems.Count / 10.0));
        var itemsOnPage = (dynamic)null;
        itemsOnPageList.Clear();
        if (SearchedItems.Count > 10){
            itemsOnPage = SearchedItems.Skip((pageNumber - 1) * 10).Take(10);
        }
        else{
            itemsOnPage = SearchedItems;
        }
        foreach (Model_Menu item in itemsOnPage)
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
        foreach (Model_Menu CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{foodCounter}. {CurrentItem.Name} | {CurrentItem.Price} EUR");
            foodCounter++;
        }
        Console.WriteLine($"\x1b[1mPage {pageNumber}/{pageTotal}\x1b[0m");

        pageNumber = FlipPage(pageNumber, pageTotal, foodCounter);
        if (pageNumber == 0)
        {
            Console.WriteLine("Going back to Main menu. Press any key to confirm.");
            pageNumber = 1;
            return;
        }
        else
        {
            Console.Clear();
            SearchSummary(searchType, searchTerm, pageNumber);
        }
    }
    
    public static int FlipPage(int pageNumber, int pageTotal, int foodCounter)
    {
        (string FlipOptions, bool PrevAvailable, bool NextAvailable) = CheckPrevNextPage(pageNumber, pageTotal);
        Console.WriteLine($"{FlipOptions}Add an item to reservation[A], Quit[Q]");
        string pageFlip = Console.ReadLine();
        switch(pageFlip)
        {
            case "P": case "p":
                if (!PrevAvailable)
                {   
                    Console.WriteLine("Incorrect input. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                    pageNumber--;
                break;
            case "N": case "n":
                if (!NextAvailable)
                {
                    Console.WriteLine("Incorrect input. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                    pageNumber++;
                break;
            case "A": case "a":
                Console.WriteLine($"Enter the number of the dish you want.[1-{foodCounter-1}]");
                bool foundItem = false;
                bool loggedIn = true;
                string dishChoice = Console.ReadLine();
                int dishCounter = 1;
                foreach (Model_Menu item in itemsOnPageList)
                {
                        if (dishChoice == $"{dishCounter}")
                        {
                            if (Functions_Reservation.AddItemToReservation(item))   
                            {
                                Console.WriteLine($"{item.Name} was added to your reservation. Press any key to continue.");
                                Console.ReadKey();
                                foundItem = true;
                            }
                            else
                                loggedIn = false;
                        }
                        dishCounter++;
                }
                if (!foundItem && loggedIn)
                {
                    Console.WriteLine($"Please enter a value between [1-{foodCounter-1}]. Press any key to continue.");
                    Console.ReadKey();
                    return pageNumber;
                }
                break;
            case "Q": case "q":
                return 0;
            default:
                Console.WriteLine("Incorrect input. Press any key to continue.");
                Console.ReadKey();
                break;
        }
        return pageNumber;
    }

    public static (string, bool, bool) CheckPrevNextPage(int pageNumber, int pageTotal)
    {
        string FlipOptions = "";
        bool PrevAvailable = true;
        bool NextAvailable = true;
        if(pageNumber != 1)
            FlipOptions += "Previous Page[P], ";
        else
            PrevAvailable = false;
        if (pageNumber != pageTotal)
            FlipOptions += "Next Page[N], ";
        else
            NextAvailable = false;
            
        return (FlipOptions, PrevAvailable, NextAvailable);
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
                Functions_Menu.SearchSummary(searchType, maxPrice, 1);
                break;
            case "2":
                Console.WriteLine("Enter a category");
                string category = Console.ReadLine();
                Functions_Menu.SearchSummary(searchType, category, 1);
                break;
            case "3":
                Console.WriteLine("Enter a dish name");
                string dish = Console.ReadLine();
                Functions_Menu.SearchSummary(searchType, dish, 1);
                break;
            case "4":
                Console.WriteLine("Going back to Main Menu. Press any key to continue");
                return;
            default:
                Console.WriteLine("You entered the wrong value. Press any key to continue.");
                break;
        }
    }
    
    public static Model_Menu FindItem(int id){
        return menuLogic.GetById(id);
    }

    public static void AddItem(int id, string name, string category, string course, double price){
        menuLogic.UpdateList(new Model_Menu(id, name, category, course, price));
    }

    public static void ReplaceItem(Model_Menu ItemReplace){
        menuLogic.UpdateList(ItemReplace);
    }

    public static void RemoveItem(Model_Menu item){
        menuLogic.RemoveItem(item);
    }
}