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
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Page {pageNumber}/{pageTotal}");
        Console.ResetColor();
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
            return;
        }

        
        Console.WriteLine("Menu");
        foreach (Model_Menu CurrentItem in itemsOnPageList)
        {
            Console.WriteLine($"{foodCounter}. {CurrentItem.Name} | {CurrentItem.Price} EUR");
            foodCounter++;
        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Page {pageNumber}/{pageTotal}");
        Console.ResetColor();
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
        (string PrintOptions, bool PrevAvailable, bool NextAvailable) = CheckPrevNextPage(pageNumber, pageTotal);
        if (!Admin.isAdmin)
            PrintOptions += "Add an item to reservation[A], ";
        Console.WriteLine($"{PrintOptions}Quit[Q]");
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
                if (Admin.isAdmin)
                {
                    Console.WriteLine("Incorrect input. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                {
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
    public static void Replacekey(string key_1)
    {
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
        
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (key_1.Length > 0)
                {
                    key_1 = key_1.Substring(0, key_1.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                key_1 += key.KeyChar;
                Console.Write("*");
            }
        }   
    }

    public static void AddItemOptions()
    {
        try 
        {
            Console.Write("Enter the ID for the item (a number): ");
            int id = Convert.ToInt32(Console.ReadLine());
            if (FindItem(id) != null)
                {
                    Console.WriteLine($"There is already an item with ID {id}! Use the modification option instead (press Escape to return to main menu)");
                    Console.ReadKey(true);
                    return;
                }
            
            Console.Write("Enter the name for the dish: ");
            string name = Console.ReadLine();
            Console.Write("Enter the category of the dish: ");
            string category = Console.ReadLine();
            Console.Write("Enter the course: ");
            string course = Console.ReadLine();
            Console.Write("Enter the price for the item (a number): ");
            double price =  Convert.ToDouble(Console.ReadLine());
            
                
            AddItem(id, name, category, course, price);
        }
        catch (System.FormatException WrongFormatting)
        {
            Console.WriteLine($"Error: You've entered the wrong type of value for this attribute");
            Console.ReadKey();
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
        }
    }

    public static void ModifyItemOptions()
    {
        try
        {
            Console.Write("Enter the ID to modify (a number): ");
            int id = Convert.ToInt32(Console.ReadLine());
            Model_Menu ItemToModify = FindItem(id);
            Console.WriteLine($"Found item: {ItemToModify}");
            Console.WriteLine("What do you want to modify?");
            string WhatToModify = Console.ReadLine().ToLower();
            switch (WhatToModify)
            {
                case "name":
                    Console.Write("Enter the name for the dish: ");
                    ItemToModify.Name = Console.ReadLine();
                    break;
                case "category":
                    Console.Write("Enter the category of the dish: ");
                    ItemToModify.Category = Console.ReadLine();
                    break;
                case "price":
                    Console.Write("Enter the price for the item (a number): ");
                    ItemToModify.Price = Convert.ToDouble(Console.ReadLine());
                    break;
                case "course":
                    Console.Write("Enter the course: ");
                    ItemToModify.Course = Console.ReadLine();
                    break;
            }
        Console.WriteLine($"Changed item: {ItemToModify}");
        ReplaceItem(ItemToModify);
        }
        catch (System.FormatException WrongFormatting)
        {
            Console.WriteLine($"Error: You've entered the wrong type of value for this attribute");
            Console.ReadKey();
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
        }
    }

    public static void RemoveItemOptions()
    {
        try
        {
            Console.Write("Enter the ID to delete (a number): ");
            int id = Convert.ToInt32(Console.ReadLine());
            Model_Menu ItemToDelete = FindItem(id);
            RemoveItem(ItemToDelete);
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
        }
    }
}