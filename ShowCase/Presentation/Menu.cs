static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        while (true)
        {   
            Console.Clear();
            Console.WriteLine("Main menu" + "\n");
            Console.WriteLine("1: Restaurant menu" + "\n" + "2: Information about our restaurant" + "\n" + "3: Your reservation" + "\n" + "4: Contact" + "\n" + "5: Close application" + "\n");

            string message = "Under maintenance";
            string input = Console.ReadLine();
            
            
            if (input == "1" && AccountsLogic.CurrentAccount == null)
            {
                Console.Clear();
                FoodMenuFunctions.MenuSummary();
                Console.ReadKey(true);
            }

            else if (input == "1" && AccountsLogic.CurrentAccount.UserType == "admin")
            {
                Console.Clear();
                Console.WriteLine("1: View the menu" + "\n" + "2: Add item" + "\n" + "3: Modify item" + "\n" + "4: Remove items" + "\n" + "5: Search items" + "\n");
                Console.WriteLine("6: Go back" + "\n");
                string subinput1 = Console.ReadLine();
                int variable = 0;
                int.TryParse(subinput1, out variable);


                switch (variable){
                    case 1:
                        FoodMenuFunctions.MenuSummary();
                        Console.ReadKey(true);
                        break;
                    case 2:
                    try{
                        Console.Write("Enter the ID for the item (a number): ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        if (FoodMenuFunctions.FindItem(id) != null)
                            {
                                Console.WriteLine($"There is already an item with ID {id}! Use the modification option instead (press Escape to return to main menu)");
                                Console.ReadKey(true);
                                break;
                            }
                        
                        Console.Write("Enter the name for the dish: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter the category of the dish: ");
                        string category = Console.ReadLine();
                        Console.Write("Enter the course: ");
                        string course = Console.ReadLine();
                        Console.Write("Enter the price for the item (a number): ");
                        double price =  Convert.ToDouble(Console.ReadLine());
                        
                           
                        FoodMenuFunctions.AddItem(id, name, category, course, price);
                    }
                    catch (System.FormatException WrongFormatting){
                        Console.WriteLine($"Error: You've entered the wrong type of value for this attribute");
                    }
                    catch (Exception err){
                        Console.WriteLine(err);
                    }
                    break; 
                    case 3:
                        try{
                            Console.Write("Enter the ID to modify (a number): ");
                            int id = Convert.ToInt32(Console.ReadLine());
                            MenuItem ItemToModify = FoodMenuFunctions.FindItem(id);
                            Console.WriteLine($"Found item: {ItemToModify}");
                            Console.WriteLine("What do you want to modify?");
                            string WhatToModify = Console.ReadLine().ToLower();
                            switch (WhatToModify){
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
                        FoodMenuFunctions.ReplaceItem(ItemToModify);


                        }
                        catch (System.FormatException WrongFormatting){
                        Console.WriteLine($"Error: You've entered the wrong type of value for this attribute");
                    }
                    catch (Exception err){
                        Console.WriteLine(err.Message);
                    }
                        break;
                    case 4:
                        try{
                            Console.Write("Enter the ID to delete (a number): ");
                            int id = Convert.ToInt32(Console.ReadLine());
                            MenuItem ItemToDelete = FoodMenuFunctions.FindItem(id);
                            FoodMenuFunctions.RemoveItem(ItemToDelete);
                            }
                        catch (Exception err){
                            Console.WriteLine(err.Message);
                        }
                        break;
                    case 5:
                        Console.WriteLine("Search by: [1] Max Price, [2] Category, [3] Dish Name");
                        string searchType = Console.ReadLine();
                        
                        if (searchType == "1")
                        {
                            Console.WriteLine("Enter a max price");
                            string maxPrice = Console.ReadLine();
                            FoodMenuFunctions.SearchSummary(searchType, maxPrice);

                        }
                        if (searchType == "2")
                        {
                            Console.WriteLine("Enter a category");
                            string category = Console.ReadLine();
                            FoodMenuFunctions.SearchSummary(searchType, category);
                        }
                        if (searchType == "3")
                        {
                            Console.WriteLine("Enter a dish name");
                            string dish = Console.ReadLine();
                            FoodMenuFunctions.SearchSummary(searchType, dish);
                        }
                        Console.ReadKey(true);
                        break;
                    default:
                        Console.WriteLine("You've entered an invalid input, try again");
                        break;
                }
                
            }

            else if (input == "2")
            {
                Console.Clear();
                InformationAccess.LoadAll();
                Console.ReadKey(true);
            }
            else if (input == "3")
            {
                Console.Clear();
                if (AccountsLogic.CurrentAccount == null){
                    UserLogin.Start();
                }
                else{

                }
                Console.ReadKey(true);
                
            }
            else if (input == "4")
            {   TextFile ContactFile  = new TextFile(@"DataSources\ContactFile.txt");
                if (AccountsLogic.CurrentAccount.UserType == "admin"){
                Console.WriteLine("1: View contact information"+ "\n" + "2: Edit contact information");
                string ContactInput = Console.ReadLine();
                int variable = 0;
                int.TryParse(ContactInput, out variable);
                switch (variable){
                    case 1:
                        Console.Clear();
                        Console.WriteLine(ContactFile.Read());
                        Console.ReadKey(true);
                        break;
                    case 2: 
                        Console.Clear();
                        Console.WriteLine("1: Edit Phone number"+ "\n" + "2: Edit Adress" + "\n" + "3: Edit E-mail");
                        string contactinput_2 = Console.ReadLine();
                        if(contactinput_2 == "1"){
                            Console.WriteLine("What is your new phone number?");
                            string new_number = $"Phone number: {Console.ReadLine()}";
                             ContactFile.ChangeValueById(0, new_number);}

                        else if(contactinput_2 == "2"){
                            Console.WriteLine("What is your new Adress?");
                            string new_adress = $"Adress: {Console.ReadLine()}";
                             ContactFile.ChangeValueById(1, new_adress);}

                        else if(contactinput_2 == "3"){
                            Console.WriteLine("What is your new Email adress?");
                            string new_email = $"Email : {Console.ReadLine()}";
                             ContactFile.ChangeValueById(2, new_email);}
                         else{
                            Console.WriteLine("Invalid input");
                            Console.ReadKey(true);
                         }
                    break;}
                }
                else{
                    Console.Clear();
                    Console.WriteLine(ContactFile.Read());
                    Console.ReadKey(true); 
                }
            }
                
            
             else if (input == "5"){
                break;
             }

            else{
                Console.WriteLine("Invalid input");
                Console.ReadKey(true);
            }
            
        }


    }
}

