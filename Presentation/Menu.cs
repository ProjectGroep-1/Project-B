static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        while (true)
        {   Console.WriteLine("\n");
            Console.WriteLine("Main menu" + "\n");
            Console.WriteLine("1: Menu options" + "\n" + "2: Information about our restaurant" + "\n" + "3: Your reservation" + "\n" + "4: Contact" + "\n");

            string message = "Under maintenance";
            string input = Console.ReadLine();
            
            if (input == "1")
            {
                Console.WriteLine("1: View the menu" + "\n" + "2: Add item" + "\n" + "3: Modify item" + "\n" + "4: Remove items" + "\n");

                string subinput1 = Console.ReadLine();
                int variable = 0;
                int.TryParse(subinput1, out variable);


                switch (variable){
                    case 1:
                        FoodMenuFunctions.MenuSummary();
                        break;
                    case 2:
                    try{
                        Console.Write("Enter the ID for the item (a number): ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the name for the dish: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter the category of the dish: ");
                        string category = Console.ReadLine();
                        Console.Write("Enter the course: ");
                        string course = Console.ReadLine();
                        Console.Write("Enter the price for the item (a number): ");
                        double price =  Convert.ToDouble(Console.ReadLine());
                        if (FoodMenuFunctions.FindItem(id) != null)
                        {
                            Console.WriteLine("Item already exists! Use replace option instead");
                            break;
                        }
            
                        FoodMenuFunctions.AddItem(id, name, category, course, price);
                    }
                    catch (System.FormatException WrongFormatting){
                        Console.WriteLine($"Error: You've entered the wrong type of value for this attribute");
                    }
                    catch (Exception err){
                        Console.WriteLine(err.Message);
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
                            Console.Write("Enter the ID to modify (a number): ");
                            int id = Convert.ToInt32(Console.ReadLine());
                            MenuItem ItemToDelete = FoodMenuFunctions.FindItem(id);
                            FoodMenuFunctions.RemoveItem(ItemToDelete);
                            }
                        catch (Exception err){
                            Console.WriteLine(err.Message);
                        }
                        break;
                    default:
                        Console.WriteLine("You've entered an invalid input, try again");
                        break;
                }
                
            }
            else if (input == "2")
            {
                Console.WriteLine(message);
                
            }
            else if (input == "3")
            {
                Console.WriteLine(message);
                
            }
            else if (input == "4")
            {
                AccountsAccess.GetContactInformation();
                
            }
            else{
                Console.WriteLine("Invalid input");
            }
            
        }


    }
}
