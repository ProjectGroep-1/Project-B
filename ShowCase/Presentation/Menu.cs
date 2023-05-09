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
            Console.WriteLine("1: Information about our restaurant" + "\n" + "2: View menu" + "\n" + "3: Search for specific dish" + "\n" + "4: Your reservation" + "\n" + "5: Contact" + "\n" + "6: Close application" + "\n");

            string message = "Under maintenance";
            string input = Console.ReadLine();
            
            if (input == Admin.Login())
            {
                Console.Clear();
                Console.WriteLine("1: View the menu" + "\n" + "2: Add item" + "\n" + "3: Modify item" + "\n" + "4: Remove items" + "\n" + "5: Search items" + "\n" + "6: Edit Contact Information");
                Console.WriteLine("7: View Capacity" + "\n" + "8: Go back" + "\n");
                string subinput1 = Console.ReadLine();
                int variable = 0;
                int.TryParse(subinput1, out variable);


                switch (variable){
                    case 1:
                        FoodMenuFunctions.MenuSummary(1);
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
                        FoodMenuFunctions.GetSearchOptions();
                        break;
                    case 6:
                    TextFile ContactFile = new TextFile("DataSources\\ContactFile.txt");
                    Console.WriteLine("1: View contact information"+ "\n" + "2: Edit contact information");
                    string ContactInput = Console.ReadLine();
                    int switcher = 0;
                    int.TryParse(ContactInput, out switcher);
                    switch (switcher){
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
                                try{
                                Console.WriteLine("What is your new phone number?");
                                string new_number = $"Phone number: {Console.ReadLine()}";
                                ContactFile.ChangeValueById(0, new_number);}
                                catch (IOException e) {
                                Console.WriteLine("Error occurred while editing file: " + e.Message);
                                }}

                            else if(contactinput_2 == "2"){
                                try{
                                Console.WriteLine("What is your new Adress?");
                                string new_adress = $"Adress: {Console.ReadLine()}";
                                ContactFile.ChangeValueById(1, new_adress);}
                                catch (IOException e) {
                                Console.WriteLine("Error occurred while editing file: " + e.Message);
                                }}

                            else if(contactinput_2 == "3"){
                                try{
                                Console.WriteLine("What is your new Email adress?");
                                string new_email = $"Email : {Console.ReadLine()}";
                                ContactFile.ChangeValueById(2, new_email);}
                                catch (IOException e) {
                                Console.WriteLine("Error occurred while editing file: " + e.Message);
                                }}
                            else{
                                Console.WriteLine("Invalid input");
                                Console.ReadKey(true);
                            } 

                        break; 
                    default:
                        Console.WriteLine("You've entered an invalid input, try again");
                        break;
                    
                }
                break;
                    
                    case 7:
                        TableAccess.ShowCapacity();
                        Console.WriteLine("Press any key");
                        string PressEnter2 = Console.ReadLine();

                        break; 
                    default:
                        Console.WriteLine("You've entered an invalid input, try again");
                        break;
                    
                }
                
            }
            else if (input == "1")
            {
                Console.Clear();
                InformationAccess.LoadAll();
                Console.ReadKey(true);
            }
            else if (input == "2")
            {
                Console.Clear();
                FoodMenuFunctions.MenuSummary(1);
                Console.ReadKey(true);
            }
            else if (input == "3")
            {
                Console.Clear();
                FoodMenuFunctions.GetSearchOptions();
                Console.ReadKey(true);
            }

            else if (input == "4")
            {
                Console.Clear();
                ReservationFunctions.ReservationMenu();
                Console.ReadKey(true);
                
            }
            else if (input == "5")
            {   TextFile ContactFile  = new TextFile(@"DataSources\ContactFile.txt");
            
                    Console.Clear();
                    Console.WriteLine(ContactFile.Read());
                    Console.ReadKey(true); 
                
            }
                
            
            else if (input == "6"){
                break;
            }

            else{
                Console.WriteLine("Invalid input");
                Console.ReadKey(true);
            }
            
        }


    }
}

